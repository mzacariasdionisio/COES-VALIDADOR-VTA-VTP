using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Models;
using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Servicio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO;
using System.Configuration;
using static iTextSharp.text.pdf.AcroFields;
using COES.Servicios.Aplicacion.General;
using Org.BouncyCastle.Crypto;

namespace COES.MVC.Publico.Controllers
{
    public class BrowserController : Controller
    {
        /// <summary>
        /// Instancias para el manipulamiento de archivos y datos
        /// </summary>
        FileManager fileManager = new FileManager();
        Servicio servicio = new Servicio();
        public int IdFuente = 1;

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string url)
        {
            string libreria = HttpUtility.UrlDecode(url);
            BrowserModel model = new BrowserModel();
            model.BaseDirectory = libreria;
            model.RelativeDirectory = libreria;
            model.Origen = ConstantesBase.FuenteLocal;

            if (Request[Constantes.RequestPath] != null) {
                if (Request[Constantes.RequestPath] != string.Empty)
                {
                    string relativePath = HttpUtility.UrlDecode(Request[Constantes.RequestPath]);
                    WbBlobDTO relativo = this.servicio.ObtenerBlobPorUrl(relativePath, this.IdFuente);

                    if (relativo != null)
                    {
                        model.RelativeDirectory = relativePath;
                    }
                }
            }

            WbBlobDTO blob = this.servicio.ObtenerBlobPorUrl(libreria,  this.IdFuente);

            if (blob != null)
            {
                model.IndicadorHeader = blob.Blobhiddcol;
                model.BreadName = blob.Blobbreadname;
                model.OrderFolder = blob.Bloborderfolder;
                model.BlobCodi = blob.Blobcodi.ToString();
            }
            else
            {
                model.IndicadorHeader = Constantes.NO;
                model.BreadName = Constantes.TextoInicial;
                model.OrderFolder = Constantes.OrdenamientoAscendente;
                model.BlobCodi = "0";
            }
           
            return View(model);
        }

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexArbol(string url)
        {
            string libreria = HttpUtility.UrlDecode(url);
            BrowserModel model = new BrowserModel();
            model.BaseDirectory = libreria;
            model.Origen = ConstantesBase.FuenteLocal;

            WbBlobDTO blob = this.servicio.ObtenerBlobPorUrl(libreria, this.IdFuente);

            if (blob != null)
            {
                model.IndicadorHeader = blob.Blobhiddcol;
                model.BreadName = blob.Blobbreadname;
            }
            else
            {
                model.IndicadorHeader = Constantes.NO;
                model.BreadName = Constantes.TextoInicial;
            }

            return View(model);
        }

        /// <summary>
        /// Permite habilitar la vista de datos
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VistaDatos(string baseDirectory, string url, string indicador, string initialLink, string orderFolder)
        {
            BrowserModel model = new BrowserModel();
            List<WbBlobcolumnDTO> columnas = new List<WbBlobcolumnDTO>();
            string libreria = HttpUtility.UrlDecode(url);
            model.BaseDirectory = libreria;
            string ordenamiento = string.Empty;
            string issuuind = string.Empty;
            string issuulink = string.Empty;
            string issuupos = string.Empty;
            string issuulenx = string.Empty;
            string issuuleny = string.Empty;
            bool indHeader = true;
            bool indicadorTree = false;
            List<FileData> list = this.fileManager.HabilitarVistaDatosWeb(url, out columnas, string.Empty, out ordenamiento, out issuuind,
                out issuulink, out issuupos, out issuulenx, out issuuleny, indicador, out indHeader, out indicadorTree, this.IdFuente);

            if (!indicadorTree)
            {

                model.IndicadorHeader = (!indHeader) ? Constantes.SI : Constantes.NO;

                if (model.IndicadorHeader != Constantes.SI)
                {
                    if (ordenamiento == Constantes.OrdenamientoAscendente)
                    {
                        list = list.OrderBy(x => x.Columnorder).Where(x => x.FileHide != Constantes.SI).ToList();
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            if (item.Columnorder == null || item.Columnorder.ToString() == "")
                            {
                                item.Columnorder = "01/01/2020 00:00:00";
                                DateTime oDate = Convert.ToDateTime(item.Columnorder);
                                item.Columnorder = oDate;
                            }

                        }
                        list = list.OrderByDescending(x => x.Columnorder).Where(x => x.FileHide != Constantes.SI).ToList();
                    }
                }
                else
                {
                    if (orderFolder == Constantes.OrdenamientoDescendente)
                    {                                    
                        list = list.OrderByDescending(x => x.Columnorder).Where(x => x.FileHide != Constantes.SI).ToList();
                    }
                    else
                    {
                        list = list.OrderBy(x => x.Columnorder).Where(x => x.FileHide != Constantes.SI).ToList();
                    }
                }

                model.ListaColumnas = columnas;
                model.DocumentList = list;
                model.BreadList = this.fileManager.ObtenerBreadCrumb(baseDirectory, url, initialLink);


                List<WbBlobcolumnDTO> busqueda = columnas.Where(x => x.Columnbusqueda == Constantes.SI).ToList();
              
                foreach (WbBlobcolumnDTO item in busqueda)
                {
                    if (item.Typecodi == TiposDeDato.ListaDesplegable)
                    {
                        //model.ListaItems = new List<WbColumnitemDTO>();
                        item.ListaItems = this.servicio.ObtenerListaItemColumna(item.Columncodi);
                        //model.ListaItems = item.ListaItems;
                    }
                }

                model.Formulario = Helper.Helper.ObtenerFormulario(busqueda);
                model.IndicadorIssuu = Constantes.NO;
                model.TipoVisor = string.Empty;

                if (issuuind == Constantes.VisorISSUU || issuuind == Constantes.VisorPDF)
                {
                    int len = 1200;
                    int ancho = 50;
                    int alto = 400;
                    if (int.TryParse(issuulenx, out ancho)) { }
                    if (int.TryParse(issuuleny, out alto)) { }
                    ancho = (int)(len * ancho / 100);
                    string contenido = string.Empty;

                    if (issuuind == Constantes.VisorISSUU)
                    {
                        model.TipoVisor = Constantes.VisorISSUU;
                        contenido = string.Format(@"<div data-configid='{0}' style='width:{1}px; height:{2}px;' class='issuuembed fondoissuu' ></div>",
                            issuulink, ancho, alto);
                    }
                    else if (issuuind == Constantes.VisorPDF)
                    {
                        model.TipoVisor = Constantes.VisorPDF;

                        string file = (list.Count > 0) ? list[0].FileUrl : string.Empty;

                        if (!string.IsNullOrEmpty(issuulink))
                            issuulink = ConfigurationManager.AppSettings["UrlPortal"].ToString() + "browser/downloadpdf?url=" + url + issuulink;
                        else
                            issuulink = ConfigurationManager.AppSettings["UrlPortal"].ToString() + "browser/downloadpdf?url=" + url + file;

                        contenido = string.Format(@"<input type='hidden' value='{0}' id='hfIdFileVisor' /><div id='pdf' style='width:{1}px; height:{2}px;'></div>",
                            issuulink, ancho, alto);

                        contenido = contenido.Replace('[', '{');
                        contenido = contenido.Replace(']', '}');
                    }

                    model.ContenidoIssuu = contenido;
                    model.PosicionIssuu = issuupos;
                    if (issuupos == "C") model.IndicadorIssuu = Constantes.SI;
                }
                else if (issuuind == Constantes.VisorEspecial)
                {
                    model.TipoVisor = Constantes.VisorEspecial;
                    model.ArchivoDefecto = (list.Count > 0) ? list[0].FileUrl : string.Empty;
                }

                return PartialView(model);
            }
            else {

                List<FileData> listGeneral = list.Where(x => x.TreePadre == null || x.TreePadre == -1).ToList();
                List<int> idsAnidado = list.Where(x => x.TreePadre != null && x.TreePadre != -1).Select(x => (int)x.TreePadre).Distinct().ToList();
                List<FileData> listResultado = new List<FileData>();

                foreach (FileData item in listGeneral)
                {
                    if (idsAnidado.Contains(item.Blobcodi)) item.IndMain = Constantes.SI;
                    item.Padrecodi = -1;
                    listResultado.Add(item);

                    if (idsAnidado.Contains(item.Blobcodi))
                    {
                        List<FileData> subList = list.Where(x => x.TreePadre == item.Blobcodi).ToList();
                        foreach (FileData itemSub in subList)
                        {
                            itemSub.IndMain = Constantes.NO;
                            itemSub.Padrecodi = item.Blobcodi;
                            listResultado.Add(itemSub);
                        }
                    }
                }

                model.ListaColumnas = columnas;
                model.DocumentList = listResultado;
                model.BreadList = this.fileManager.ObtenerBreadCrumb(baseDirectory, url, initialLink);

                return PartialView("VistaDatosArbol", model);
            }
        }

        /// <summary>
        /// Permite habilitar la vista de datos
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VistaDatosArbol(string baseDirectory, string url, string indicador, string initialLink)
        {
            BrowserModel model = new BrowserModel();
            List<WbBlobcolumnDTO> columnas = new List<WbBlobcolumnDTO>();

            string ordenamiento = string.Empty;
            string issuuind = string.Empty;
            string issuulink = string.Empty;
            string issuupos = string.Empty;
            string issuulenx = string.Empty;
            string issuuleny = string.Empty;
            bool indHeader = true;
            bool indicadorTree = false;
            List<FileData> listPrincipal = this.fileManager.HabilitarVistaDatosWeb(url, out columnas, string.Empty, out ordenamiento, out issuuind,
                out issuulink, out issuupos, out issuulenx, out issuuleny, indicador, out indHeader, out indicadorTree, this.IdFuente);         

            List<FileData> listGeneral = listPrincipal.Where(x => x.TreePadre == null || x.TreePadre == -1).ToList();
            List<int> idsAnidado = listPrincipal.Where(x => x.TreePadre != null && x.TreePadre != -1).Select(x => (int)x.TreePadre).Distinct().ToList();
            List<FileData> list = new List<FileData>();

            foreach (FileData item in listGeneral)
            {
                if (idsAnidado.Contains(item.Blobcodi)) item.IndMain = Constantes.SI;
                item.Padrecodi = -1;
                list.Add(item);

                if (idsAnidado.Contains(item.Blobcodi))
                {
                    List<FileData> subList = listPrincipal.Where(x => x.TreePadre == item.Blobcodi).ToList();
                    foreach (FileData itemSub in subList)
                    {
                        itemSub.IndMain = Constantes.NO;
                        itemSub.Padrecodi = item.Blobcodi;
                        list.Add(itemSub);
                    }
                }
            }

            model.ListaColumnas = columnas;
            model.DocumentList = list;
            model.BreadList = this.fileManager.ObtenerBreadCrumb(baseDirectory, url, initialLink);

            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url)
        {
            Stream stream = this.fileManager.DownloadToStream(url);

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
        public virtual FileContentResult DownloadPdf(string url)
        {
            byte[] stream = this.fileManager.DownloadToArrayByte(url);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            byte[] doc = stream;
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(doc, mimeType);
        }

        /// <summary>
        /// Permite descargar pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult DownloadHtml(string url)
        {
            byte[] stream = this.fileManager.DownloadToArrayByte(url);

            int indexOf = url.LastIndexOf('/');
            string fileName = url;
            if (indexOf >= 0)
            {
                fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
            }
            indexOf = fileName.LastIndexOf('.');
            string extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

            byte[] doc = stream;
            string mimeType = "text/html";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(doc, mimeType);
        }

        /// <summary>
        /// Permite generar un archivo zip de otros seleccionados
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarZip(List<String> urls)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                string fileName = path + Constantes.FileZip;
                int result = this.fileManager.DownloadAsZip(urls.ToList(), fileName);
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

        /// <summary>
        /// Permite realizar la búsqueda de archivos
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Busqueda(string url, string ids, string valores, int nroPagina, string indicador, string baseDirectory, string initialLink, string orderFolder)
        {
            ids = ids == "campo7_3" ? "7" : ids;
            BrowserModel model = new BrowserModel();
            List<DatoItem> datos = new List<DatoItem>();
            if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(valores))
            {
                string[] listId = ids.Split(Constantes.CaracterComa);
                string[] listValores = valores.Split(Constantes.CaracterAnd);

                if (listId.Count() == listValores.Count())
                {
                    for (int i = 0; i < listId.Count(); i++)
                    {
                        DatoItem item = new DatoItem();
                        item.Id = int.Parse(listId[i]);
                        item.Valor = listValores[i];
                        datos.Add(item);
                    }
                }
               

                List<WbBlobcolumnDTO> columnas = new List<WbBlobcolumnDTO>();
                List<FileData> list = this.fileManager.BuscarArchivos(url, out columnas, datos, nroPagina, Constantes.PageSize, this.IdFuente);
                model.ListaColumnas = columnas;
                model.DocumentList = list;
            
                model.BreadList = this.fileManager.ObtenerBreadCrumb(baseDirectory, url, initialLink);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string url, string ids, string valores)
        {
            ids = ids == "campo7_3" ? "7" : ids;
            BrowserModel model = new BrowserModel();
            List<DatoItem> datos = new List<DatoItem>();
            if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(valores))
            {
                string[] listId = ids.Split(Constantes.CaracterComa);
                string[] listValores = valores.Split(Constantes.CaracterAnd);

                if (listId.Count() == listValores.Count())
                {
                    for (int i = 0; i < listId.Count(); i++)
                    {
                        DatoItem item = new DatoItem();
                        item.Id = int.Parse(listId[i]);
                        item.Valor = listValores[i];
                        datos.Add(item);
                    }
                }

                List<WbBlobcolumnDTO> columnas = new List<WbBlobcolumnDTO>();
                int nroRegistros = this.fileManager.ObtenerNroRegistrosBusqueda(url, datos, this.IdFuente);

                if (nroRegistros > Constantes.NroPageShow)
                {
                    int pageSize = Constantes.PageSize;
                    int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult VisorVideo(string url)
        {
            BrowserModel model = new BrowserModel();
            model.Url = url;
            return PartialView(model);
        }

        public ActionResult GetVideo(string url)
        {
            byte[] video = this.fileManager.DownloadToArrayByte(url);
            string mimeType = "video/mp4";

            //switch (contentType.ToUpper())
            //{
            //    case "MOV":
            //        mimeType = "video/quicktime";
            //        break;
            //    case "MP4":
            //        mimeType = "video/mp4";
            //        break;
            //    case "FLV":
            //        mimeType = "video/x-flv";
            //        break;
            //    case "AVI":
            //        mimeType = "video/x-msvideo";
            //        break;
            //    case "WMV":
            //        mimeType = "video/x-ms-wmv";
            //        break;
            //    case "MJPG":
            //        mimeType = "video/x-motion-jpeg";
            //        break;
            //    case "TS":
            //        mimeType = "video/MP2T";
            //        break;
            //    default:
            //        mimeType = "video/mp4";
            //        break;
            //}
            FileInfo fi = this.fileManager.ObtenerFileInfo(url);
            //return new FileContentResult(video, mimeType);
            return new RangeFileContentResult(video, mimeType, fi.Name, fi.LastWriteTimeUtc);
        }

        [HttpPost]
        public JsonResult ListaFiltroCombo(string ids)
        {
            BrowserModel model = new BrowserModel();
            List<WbColumnitemDTO> ListaItems = new List<WbColumnitemDTO>();
            ListaItems = this.servicio.ObtenerListaItemColumna(Convert.ToInt32(ids));

            return Json(ListaItems);
        }

    }
}
