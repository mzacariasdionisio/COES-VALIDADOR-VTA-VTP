using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class PuntoEntregaController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();
     
        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
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

        #region Listado PE
        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            model.ListaNivelTension = servicioResarcimiento.ListReNivelTensions();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Lista los puntos de entrega
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult listarPuntosEntrega()
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            try
            {
                base.ValidarSesionJsonResult();                

                model.ListadoPuntoEntrega = servicioResarcimiento.ListarPuntosDeEntrega();
                model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
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
        /// Guarda el punto de entrega
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="niveltension"></param>
        /// <param name="estado"></param>
        /// <param name="repentcodi"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult guardarPuntosEntrega(string nombre, int niveltension, string estado, int? repentcodi, int accion)
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            try
            {
                base.ValidarSesionJsonResult();
                

                servicioResarcimiento.GuardarDatosPuntoEntrega(nombre, niveltension, estado, User.Identity.Name, accion, repentcodi);
                model.ListadoPuntoEntrega = new List<RePuntoEntregaDTO>();
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
        /// Obtiene los detalles de cierto punto de entrega
        /// </summary>
        /// <param name="repentcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult abrirPuntosEntrega(int repentcodi)
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            try
            {
                base.ValidarSesionJsonResult();
            
                model.PuntoEntrega = servicioResarcimiento.GetByIdRePuntoEntrega(repentcodi);
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
        /// Genera el archivo a exportar el listado de puntos de entrega
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoPE()
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            try
            {
                                
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Maestro Punto de Entrega.xlsx";

                servicioResarcimiento.GenerarExportacionPE(ruta, pathLogo,  nameFile);
                model.Resultado = nameFile;
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
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Elimina cierto punto de entrega
        /// </summary>
        /// <param name="repentcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarPE(int repentcodi)
        {
            PuntoEntregaModel model = new PuntoEntregaModel();

            try
            {
                base.ValidarSesionJsonResult();
                servicioResarcimiento.EliminarPuntoEntrega(repentcodi, base.UserName);
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