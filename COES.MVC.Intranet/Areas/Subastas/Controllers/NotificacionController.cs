using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class NotificacionController : BaseController
    {
        readonly SubastasAppServicio servicio = new SubastasAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public NotificacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            NotificacionModel model = new NotificacionModel();
            //model.ListaTipoPlantilla = servicioFT.ListarTipoPlantilla();
            model.LogoEmail = ConstantesAppServicio.LogoCoesEmail;

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarPlantillaCorreo()
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                model.AccionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                //model.AccionGrabar = true;
                model.ListadoPlantillasCorreo = servicio.ListarPlantillaNotificacion();
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ObtenerDetalleCorreo(int plantillacodi)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                SiPlantillacorreoDTO objPlantilla = servicio.GetByIdSiPlantillacorreo(plantillacodi);
                model.TieneEjecucionManual = ConfigurationManager.AppSettings["RSFFlagEnviarNotificacionManual"];
                model.PlantillaCorreo = objPlantilla;
                model.TipoCorreo = 2;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarPlantillaCorreo(SiPlantillacorreoDTO plantillaCorreo)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Actualizo la plantilla
                servicio.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        ///  Ejecutar recordatorio de manera manual
        /// </summary>
        /// <param name="plantcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarRecordatorio(int plantcodi)
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                servicio.EjecutarProcesoAutomaticoOfertaRSFXPlantilla(plantcodi, out int resultadoEjec, out string mensajeEjec);

                model.Resultado = resultadoEjec.ToString();
                model.Mensaje = mensajeEjec;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Ejecutar eliminación lógica de envios de enero del año siguiente
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetearRecordatorio()
        {
            NotificacionModel model = new NotificacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                var resultado = servicio.ResetearRecordatoriosManualmente(out string mensaje);

                model.Resultado = resultado.ToString();
                model.Mensaje = mensaje;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
    }
}