using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class CargaMasivaController : BaseController
    {
        readonly ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        readonly EjecutadoAppServicio servEjec = new EjecutadoAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        //
        // GET: /Medidores/Reportes/GenerarMaximaDemanda
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.FechaIni = DateTime.Today.AddMonths(-4).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = DateTime.Today.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Procesar maxima demanda segun fecha escogida por el usuario
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public JsonResult GuardarProduccionGeneracion(string mesIni, string mesFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = EPDate.GetFechaIniPeriodo(5, mesIni, string.Empty, string.Empty, string.Empty);
                DateTime fechaFinProceso = EPDate.GetFechaIniPeriodo(5, mesFin, string.Empty, string.Empty, string.Empty);

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }

                if (fechaIniProceso.AddMonths(4) <= fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                this.servReporte.GuardarEstructurasProduccionGeneracionYResumen(fechaIniProceso, fechaFinProceso, base.UserName);

                /*
                for (DateTime day = fechaIniProceso; day <= fechaFinProceso; day = day.AddMonths(1))
                {
                    DateTime f1 = day;
                    DateTime f2 = day.AddMonths(1).AddDays(-1);
                    servReporte.GuardarEstructurasProduccionGeneracionYResumen(f1, f2, base.UserName);
                }*/
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }

        public JsonResult GuardarDespachoDiario(string fechaIni, string fechaFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinProceso = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }
                
                if (fechaIniProceso.AddMonths(4) < fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                servEjec.GuardarDespachoDiarioProdGen(fechaIniProceso, fechaFinProceso, ConstantesMedicion.TipoDatoDespachoEjec, ConstantesMedicion.TipoDatoDespachoProg);
                
                /*for (DateTime day = fechaIniProceso; day <= fechaFinProceso; day = day.AddMonths(2))
                {
                    DateTime f1 = day;
                    DateTime f2 = day.AddMonths(2).AddDays(-1);
                    servEjec.GuardarDespachoDiarioProdGen(fechaIniProceso, fechaFinProceso, ConstantesMedicion.TipoDatoDespachoEjec, ConstantesMedicion.TipoDatoDespachoProg);
                }*/
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }

    }
}
