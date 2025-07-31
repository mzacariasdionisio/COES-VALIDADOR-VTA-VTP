using System;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System.Threading.Tasks;
using COES.Dominio.DTO.ValidacionVTEAVTP;
using System.Collections.Generic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers
{
    public class PruebaServicioController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        
        ValidacionVTEAVTPAppServicio validacionVTEAVTPAppServicio = new ValidacionVTEAVTPAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PruebaServicioController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public PruebaServicioController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            PruebaServicioModel modelo = new PruebaServicioModel();
            modelo.StrMensaje = "hola";
            modelo.Resultado = "prueba";
            FileServer.CreateFolder(base.PathFiles, ConstantesValidacionVTEAVTP.FolderValidacion, "");
            FileServer.CreateFolder(base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog, "");

            string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

            List<TrnPeriodoDTO> lstPeriodo = await validacionVTEAVTPAppServicio.ObtenerSmeTrnPeriodo(rutaUpload, base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog);
            List<VteVersionDTO> lstVeriones = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions("2025.Marzo", "", rutaUpload, base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog);
            VtpDTO vtp = await validacionVTEAVTPAppServicio.FuncionVtp("2025.Marzo", "Revisión 01", rutaUpload, base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog);
            VtpValidacionDTO valiacion = await validacionVTEAVTPAppServicio.FuncionVtpValidar("2025.Marzo", "", rutaUpload, base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog);
            VtpVteaDTO vtpVtea = await validacionVTEAVTPAppServicio.FuncionVtpVtea("2025.Marzo", "", "", rutaUpload, base.PathFiles, ConstantesValidacionVTEAVTP.FolderLog);
            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(PruebaServicioModel datos)
        {
            return View(datos);
        }

    }
}
