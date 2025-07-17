using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.DevolucionAportes;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class AmortizacionesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AmortizacionesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("AmortizacionesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("AmortizacionesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listado() {
            List<DaiPresupuestoDTO> listadoPresupuesto = new List<DaiPresupuestoDTO>();

            try
            {
                listadoPresupuesto = _svcDevolucionAporte.GetByCriteriaDaiPresupuestos();
            }
            catch (Exception ex)
            {
                log.Error("Listado", ex);
            }
            
            return PartialView(listadoPresupuesto);
        }

        public JsonResult GuardarAmortizacion(DaiPresupuestoDTO presupuesto) {
            try
            {
                presupuesto.Presmonto = Convert.ToDecimal(presupuesto.monto);
                presupuesto.Presusucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                presupuesto.Presfeccreacion = DateTime.Now;

                _svcDevolucionAporte.SaveDaiPresupuesto(presupuesto);

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("GuardarAmortizacion", ex);
            }

            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarAmortizacion(int prescodi)
        {
            try
            {
                DaiPresupuestoDTO presupuesto = new DaiPresupuestoDTO
                {
                    Prescodi = prescodi,
                    Presusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
                    Presfecmodificacion = DateTime.Now
                };

                _svcDevolucionAporte.DeleteDaiPresupuesto(presupuesto);

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("EliminarAmortizacion", ex);
            }

            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerAmortizacion(int prescodi)
        {
            DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(prescodi);

            return Json(presupuesto, JsonRequestBehavior.AllowGet);
        }
    }
}
