using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class PlantillaCorreoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();
        CalidadProductoAppServicio servicioCalidad = new CalidadProductoAppServicio();
        CorreoAppServicio servCorreo = new CorreoAppServicio();

        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion


        #region Configuracion Plantilla Correos
        /// <summary>
        /// Pagina principal de plantilla de correos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PlantillaCorreoModel model = new PlantillaCorreoModel();

            return View(model);
        }

        /// <summary>
        /// Devuelve el listado de plantillas de correos
        /// </summary>
        /// <param name="tipoPlantilla"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPlantillasCorreos()
        {
            PlantillaCorreoModel model = new PlantillaCorreoModel();

            try
            {
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillasCorreo = servicioCalidad.ListarPlantillasCorreo();
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
        /// Devuelve los detalles de la plantilla del correo
        /// </summary>
        /// <param name="plantillacodi"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDetalleCorreo(int plantillacodi)
        {
            PlantillaCorreoModel model = new PlantillaCorreoModel();

            try
            {
                //Obtengo el registro de la plantilla
                SiPlantillacorreoDTO plantillaCompleta = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaConHoraEjecucion = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaBD = servCorreo.GetByIdSiPlantillacorreo(plantillacodi);

                plantillaCompleta = plantillaBD;

                model.PlantillaCorreo = plantillaCompleta;

                //obtengo las variables del Contenido
                model.ListaVariables = servicioCalidad.ObtenerListadoVariables(plantillacodi, ConstantesCalidadProducto.VariableContenido);
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
        /// devuelve el listado de variables 
        /// </summary>
        /// <param name="idBoton"></param>
        /// <param name="idPlantilla"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarVariables(int idPlantilla, int campo)
        {
            PlantillaCorreoModel model = new PlantillaCorreoModel();

            try
            {
                base.ValidarSesionJsonResult();               

                model.ListaVariables = servicioCalidad.ObtenerListadoVariables(idPlantilla, campo);
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
        /// Guarda la informacion de una plantilla de correo
        /// </summary>
        /// <param name="plantillaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPlantillaCorreo(SiPlantillacorreoDTO plantillaCorreo)
        {
            PlantillaCorreoModel model = new PlantillaCorreoModel();

            try
            {
                base.ValidarSesionJsonResult();
                //Actualizo la plantilla
                servicioCalidad.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
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

        #endregion

    }
}