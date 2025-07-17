using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Intervenciones.Models;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Intervenciones.Controllers
{
    public class ConfiguracionController : BaseController
    {

        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        IntervencionesAppServicio servicio = new IntervencionesAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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
        /// Permite visualizar la pantalla de configuración de notificación
        /// </summary>
        /// <returns></returns>
        public ActionResult Notificacion()
        {
            NotificacionModel model = new NotificacionModel();


            return View(model);
        }

        /// <summary>
        /// Permite listar la configuración de notificaciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaNotificacion()
        {
            NotificacionModel model = new NotificacionModel();
            List<SiEmpresaDTO> listEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
              Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            int usercode = base.UserCode;

            List<InConfiguracionNotificacion> list = this.servicio.ObtenerConfiguracionNotificacion(0, ConstantesAppServicio.Activo);
            model.ListaConfiguracion = list.Where(x => listEmpresa.Any(y => x.Emprcodi == y.Emprcodi) && x.Usercodi == usercode).ToList();
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
            return Json(this.servicio.GrabarConfiguracionNotificacionExtranet(data, base.UserName));
        }
    }
}