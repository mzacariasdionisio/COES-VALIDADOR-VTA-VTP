using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.CortoPlazo.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using System.IO;

namespace COES.MVC.Extranet.Areas.CortoPlazo.Controllers
{
    public class InformacionOperativaController : BaseController
    {
        /// <summary>
        /// Creación de la instancia del servicio correspondiente
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(InformacionOperativaController));

        public InformacionOperativaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("InformacionOperativaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("InformacionOperativaController", ex);
                throw;
            }
        }

        /// <summary>
        /// Muestra la pagina de resultados
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Validación de Seguridad (Sesión Iniciada y Opción permitida para el usuario)
            if (!base.IsValidSesion) return Redirect(Constantes.PaginaLogin);
            if (!base.VerificarAccesoAccion(4, base.UserName)) return base.RedirectToHomeDefault();

            InformacionOperativaModel model = new InformacionOperativaModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Muestra la página de resultados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string fecha, int tipoProceso)
        {
            InformacionOperativaModel model = new InformacionOperativaModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Listado = this.servicio.ObtenerResultadoCostosMarginalesExtranet(fechaConsulta, tipoProceso);        
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar los archivos relacionado la corrida
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult FileManager(string fecha, int correlativo)
        {
            InformacionOperativaModel model = new InformacionOperativaModel();
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string path = this.servicio.GetPathCorrida(fechaProceso, correlativo);
            model.PathResultado = path;
            string libreria = HttpUtility.UrlDecode(path);
            model.BaseDirectory = libreria;
            model.Origen = ConfigurationManager.AppSettings["PathCostosMarginales"].ToString();


            List<FileData> list = FileServerScada.ListarArhivos(model.BaseDirectory, model.Origen);
            model.DocumentList = list.Where(x => x.Extension == ".raw").ToList();
            model.BreadList = FileServer.ObtenerBreadCrumb(model.Origen, model.BaseDirectory);
            return PartialView(model);
        }

        /// <summary>
        /// Permite descargar el archivo seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {

            string pathAlternativo = ConfigurationManager.AppSettings["PathCostosMarginales"].ToString();
            Stream stream = FileServerScada.DownloadToStream(url, pathAlternativo);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            return File(stream, extension, fileName);
        }

    }
}
