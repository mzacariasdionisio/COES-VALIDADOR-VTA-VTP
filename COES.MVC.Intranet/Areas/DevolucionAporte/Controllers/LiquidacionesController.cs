using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.DevolucionAportes;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class LiquidacionesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(LiquidacionesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("LiquidacionesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("LiquidacionesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            ViewBag.ListaAnioInversion = _svcDevolucionAporte.GetByCriteriaDaiPresupuestos();
            return View();
        }

        public ActionResult Listado(int prescodi, int tabcdcodiestado)
        {
            List<DaiAportanteDTO> listadoAportantes = new List<DaiAportanteDTO>();

            try
            {
                listadoAportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantesLiquidacion(prescodi, tabcdcodiestado);
            }
            catch (Exception ex)
            {
                log.Error("Listado", ex);
            }

            return PartialView(listadoAportantes);
        }


        public ActionResult Detalle(int aporcodi)
        {
            DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(aporcodi);

            return PartialView(aportante);
        }

        public JsonResult Liquidar(int aporcodi)
        {
            try
            {
                DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(aporcodi);
                aportante.Tabcdcodiestado = Convert.ToInt32(DaiEstadoAportante.Liquidado);
                aportante.Aporfecliquidacion = DateTime.Now;
                aportante.Aporusuliquidacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                aportante.Aporfecmodificacion = DateTime.Now;
                aportante.Aporusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                aportante.Aporliquidado = "1";

                _svcDevolucionAporte.UpdateDaiAportante(aportante);

                List<DaiCalendariopagoDTO> listadoCalendarioPagoPendiente = _svcDevolucionAporte.ListDaiCalendariopagos(aporcodi, Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente));
                int calecodi = 0;
                int anio = 0;

                if (listadoCalendarioPagoPendiente.Count > 0)
                {
                    calecodi = listadoCalendarioPagoPendiente[0].Calecodi;
                    anio = Convert.ToInt32(listadoCalendarioPagoPendiente[0].Caleanio);
                }

                List<DaiCalendariopagoDTO> calendariopago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(aportante.Aporcodi);
                List<DaiCalendariopagoDTO> calendariopagoactual = calendariopago.Where(cp => Convert.ToInt32(cp.Caleanio) >= anio).ToList();
                DaiCalendariopagoDTO calendariopagoanio = calendariopago.FirstOrDefault(c => c.Caleanio == anio.ToString());

                decimal totalCapital = calendariopagoactual.Sum(c => c.Calecapital.Value);

                if (anio == DateTime.Now.Year)
                {
                    DaiTablacodigoDetalleDTO tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.Tasa_Interes));

                    decimal tasa = 0;

                    if (tablaCodigoDetalle != null)
                    {
                        tasa = Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor);
                    }

                    TimeSpan elapsed = Convert.ToDateTime(DateTime.Now.ToShortDateString()) - Convert.ToDateTime("01/01/" + anio);
                    int days = (int)elapsed.TotalDays;
                    decimal tasaDiaria = tasa / 365m;
                    decimal intereses = tasaDiaria * calendariopagoanio.Calecapital.Value;
                    decimal totalIntereses = calendariopagoanio.Calecapital.Value + intereses;

                    calendariopagoanio.Caletotal = 0;
                    calendariopagoanio.Caleinteres = intereses;
                    calendariopagoanio.Caleamortizacion = calendariopagoanio.Calecapital + intereses;
                }
                else
                {
                    calendariopagoanio.Caletotal = 0;
                    calendariopagoanio.Caleinteres = 0;
                    calendariopagoanio.Caleamortizacion = totalCapital;
                }

                _svcDevolucionAporte.UpdateDaiCalendariopago(calendariopagoanio);

                _svcDevolucionAporte.LiquidarDaiCalendariopago(new DaiCalendariopagoDTO
                {
                    Aporcodi = Convert.ToInt32(aporcodi),
                    Calecodi = calecodi,
                    Calefecmodificacion = DateTime.Now,
                    Caleusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin
                });

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Liquidar", ex);
            }

            return Json("0", JsonRequestBehavior.AllowGet);
        }
    }
}
