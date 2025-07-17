using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTCorreoController : BaseController
    {
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTProyectoController));
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

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FtPlantillaCorreo model = new FtPlantillaCorreo();
            model.ListaTipoPlantilla = servicioFT.ListarTipoPlantilla();
            model.ListaEtapa = servicioFT.ListarEtapaXTipoPlantilla(model.ListaTipoPlantilla.First().Tpcorrcodi).OrderBy(x => x.Ftetcodi).ToList();
            model.LogoEmail = ConstantesAppServicio.LogoCoesEmail;

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarPlantillaCorreo(int tpcorrcodi, int ftetcodi)
        {
            FtPlantillaCorreo model = new FtPlantillaCorreo();

            try
            {
                model.AccionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillasCorreo = servicioFT.ListarPlantillaXTipoYEtapa(tpcorrcodi, ftetcodi);
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
            FtPlantillaCorreo model = new FtPlantillaCorreo();

            try
            {
                SiPlantillacorreoDTO objPlantilla = servicioFT.GetByIdSiPlantillacorreo(plantillacodi);

                SiPlantillacorreoDTO plantillaCompleta = servicioFT.AgregarParametrosDiaRecepcion(objPlantilla); //Agrega la hora y dias de la variable del contenido 
                model.PlantillaCorreo = plantillaCompleta;
                model.TipoCorreo = servicioFT.ObtenerTipoCorreo(plantillacodi);                
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
            FtPlantillaCorreo model = new FtPlantillaCorreo();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Actualizo la plantilla
                servicioFT.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
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
        public JsonResult EjecutarRecordatorio(int plantcodi, int ftetcodi)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                servicioFT.EjecutarRecordatoriosManualmente(plantcodi, ftetcodi);

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
    }
}