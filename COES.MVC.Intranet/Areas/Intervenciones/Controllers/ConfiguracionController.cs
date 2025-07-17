using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class ConfiguracionController : BaseController
    {

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        IntervencionesAppServicio servicio = new IntervencionesAppServicio();

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

        /// <summary>
        /// Permite mostrar la pantalla de configuración de notificaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Notificacion()
        {
            //if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            NotificacionModel model = new NotificacionModel();
            model.ListaEmpresas = this.servicio.ListarComboEmpresas();
            return View(model);
        }

        /// <summary>
        /// Permite listar la configuración de notificaciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaNotificacion(int empresa, string estado)
        {
            NotificacionModel model = new NotificacionModel();
            model.ListaConfiguracion = this.servicio.ObtenerConfiguracionNotificacion(empresa, estado);
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el historial de cambios de configuración de notificaciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="empresa"></param>
        /// <param name="idUsuario"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoricoNotificacion(int idEmpresa, string empresa, int idUsuario, string usuario)
        {
            NotificacionModel model = new NotificacionModel();
            model.Empresa = empresa;
            model.Usuario = usuario;
            model.ListaConfiguracion = this.servicio.ObtenerHistoricoConfiguracionNotificacion(idEmpresa, idUsuario);
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la configuración de notificaciones
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarNotificacion(string[][] data)
        {
            return Json(this.servicio.GrabarConfiguracionNotificacion(data, base.UserName));
        }

        /// <summary>
        /// Permite generar el reporte de configuracion de notificaciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarNotificacion(int empresa, string estado)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string file = ConstantesIntervencionesAppServicio.ArchivoExportacionConfiguracionNotificacion;
                int result = this.servicio.ExportarConfiguracionNotificacion(path, file, empresa, estado);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite descargar el achivo de reporte de configuración de notificaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarNotificacion()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes +
                ConstantesIntervencionesAppServicio.ArchivoExportacionConfiguracionNotificacion;
            return File(fullPath, Constantes.AppExcel, ConstantesIntervencionesAppServicio.ArchivoExportacionConfiguracionNotificacion);
        }

    }
}