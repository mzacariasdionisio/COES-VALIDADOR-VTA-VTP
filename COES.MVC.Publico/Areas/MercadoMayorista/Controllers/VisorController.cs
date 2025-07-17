using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Areas.MercadoMayorista.Models;

namespace COES.MVC.Publico.Areas.MercadoMayorista.Controllers
{
    public class VisorController : Controller
    {
        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string url, string pathAlternativo)
        {
            string libreria = HttpUtility.UrlDecode(url);
            BrowserModel model = new BrowserModel();
            model.BaseDirectory = libreria;
            model.Origen = pathAlternativo;
            return View(model);
        }

        /// <summary>
        /// Permite habilitar la vista de datos
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Folder(string baseDirectory, string url, string pathAlternativo)
        {
            BrowserModel model = new BrowserModel();
            List<FileData> list = FileServer.ListarArhivos(url, pathAlternativo);

            model.DocumentList = list.Where(x => x.FileName.Contains("ENTRADAGAMS") || x.FileName.Contains("RESULTADOGAMS")).ToList();
            model.BreadList = FileServer.ObtenerBreadCrumb(baseDirectory, url);
            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url, string pathAlternativo)
        {
            Stream stream = FileServer.DownloadToStream(url, pathAlternativo);

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

        /// <summary>
        /// Permite descargar pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult DownloadPdf(string url, string pathAlternativo)
        {
            byte[] stream = FileServer.DownloadToArrayByte(url, pathAlternativo);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            byte[] doc = stream;
            string mimeType = Constantes.AppZip;
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(doc, mimeType);
        }

        /// <summary>
        /// Permite generar un archivo zip de otros seleccionados
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarZip(List<String> urls, string pathAlternativo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                string fileName = path + Constantes.FileZip;
                int result = FileServer.DownloadAsZip(urls.ToList(), fileName, pathAlternativo);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DownloadZip()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.FileZip;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppZip, Constantes.FileZip);
        }
    }
}
