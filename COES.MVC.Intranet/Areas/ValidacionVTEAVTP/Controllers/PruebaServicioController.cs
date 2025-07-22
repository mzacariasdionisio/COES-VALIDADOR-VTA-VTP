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
            List<TrnPeriodoDTO> lstPeriodo = await validacionVTEAVTPAppServicio.ObtenerSmeTrnPeriodo();
            List<VteVersionDTO> lstVeriones = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions("2025.Marzo", "");
            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(PruebaServicioModel datos)
        {
            return View(datos);
        }

    }
}
