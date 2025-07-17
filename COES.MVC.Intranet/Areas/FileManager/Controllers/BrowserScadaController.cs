using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.FileManager.Models;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.FileManager.Controllers
{
    public class BrowserScadaController : Controller
    {
        RpfAppServicio servicio = new RpfAppServicio();

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionMigraciones.SesionNombreArchivo] != null) ?
                    Session[DatosSesionMigraciones.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionMigraciones.SesionNombreArchivo] = value; }
        }

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
            if (url.Contains("Nuevo")) { url = url.Replace("Nuevo", ""); }
            if (baseDirectory.Contains("Nuevo")) { baseDirectory = baseDirectory.Replace("Nuevo", ""); }
            List<FileData> list = FileServerScada.ListarArhivos(url, pathAlternativo);
            model.DocumentList = list;
            model.BreadList = FileServerScada.ObtenerBreadCrumb(baseDirectory, url);
            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url, string pathAlternativo)
        {
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

        /// <summary>
        /// Permite descargar pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual FileContentResult DownloadPdf(string url, string pathAlternativo)
        {
            byte[] stream = FileServerScada.DownloadToArrayByte(url, pathAlternativo);

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
                int result = FileServerScada.DownloadAsZip(urls.ToList(), fileName, pathAlternativo);
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
            return File(fullPath, Constantes.AppZip, Constantes.FileZip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpArchivo(string nameFile)
        {
            string LocalDirectory = FileServerScada.GetDirectory();
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string url2 = "Intranet/Pr21/";
            //LocalDirectory = LocalDirectory.Replace("servernas01", "192.168.1.4");
            string targetPath = LocalDirectory + url2;

            BrowserModel model = new BrowserModel();
            var arrZip = nameFile.Split(',');
            foreach (var dd in arrZip)
            {
                string msj = string.Empty;
                string file = path + dd;

                if (System.IO.Directory.Exists(path))
                {
                    if (!FileServerScada.VerificarExistenciaFile(string.Empty, dd, path))
                    {
                        System.IO.File.Copy(targetPath + dd, file, true);
                    }

                    try
                    {
                        FileServerScada.DescomprimirZip(dd, path, path);

                        //Leemos el archivo ZIP
                        using (ZipArchive archive = ZipFile.OpenRead(file))
                        {
                            //Obtenemos solo los archivos que necesitamos para obtener informacion
                            var datos = archive.Entries.Where(x => x.FullName.Contains(".csv")).ToList();

                            if (datos.Count > 0)
                            {
                                List<MeMedicion60DTO> list = new List<MeMedicion60DTO>();
                                foreach (var d in datos)
                                {
                                    list.AddRange(servicio.ProcesarArchivoZipRpf(path + d.ToString()));
                                    System.IO.File.Delete(path + d.ToString());
                                }

                                if (list.Count > 0)
                                {
                                    var ptos = list.Select(x => x.Ptomedicodi).Distinct().ToList();
                                    var fechas = list.Select(x => x.Fechahora.ToString(ConstantesAppServicio.FormatoFecha)).Distinct().ToList();
                                    DateTime f1 = DateTime.Parse(fechas.Min());
                                    DateTime f2 = DateTime.Parse(fechas.Max());

                                    var Tipoinfocodi = list.Select(x => x.Tipoinfocodi).Distinct().ToList();

                                    servicio.EliminarCargaRpf(string.Join(",", ptos), f1, f2.AddDays(1), 0, string.Join(",", Tipoinfocodi));

                                    int nSize = 30000;
                                    for (int i = 0; i < list.Count; i += nSize)
                                    {
                                        var listaSmall = list.GetRange(i, Math.Min(nSize, list.Count - i));
                                        servicio.GrabarDatosRpf(listaSmall, new DateTime(), 0);
                                    }
                                }
                            }
                            else { msj = "El archivo ZIP no contiene archivos .csv"; }

                            model.Origen = datos.Count + "," + msj;
                        }
                    }
                    catch { }

                    this.DeleteTmpZip(dd, path + dd, path);
                }
                else { model.Origen = "0,No existe el directorio"; }
            }
            return Json(model);
        }

        /// <summary>
        /// Funcion de eliminacion de zip
        /// </summary>
        /// <param name="NombreFile"></param>
        /// <param name="file"></param>
        /// <param name="path"></param>
        public void DeleteTmpZip(string NombreFile, string file, string path)
        {
            #region Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)

            if (NombreFile.ToLower().Contains(".zip"))
            {
                List<string> archivos = new List<string>();
                using (FileStream zipToOpen = new FileStream(file, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        for (int i = 0; i < archive.Entries.Count; i++)
                        {
                            foreach (var item in archive.Entries)
                            {
                                archivos.Add(item.ToString());
                                item.Delete();
                                break; //needed to break out of the loop
                            }

                            i = -1;
                        }
                    }
                }
                System.IO.File.Delete(file);
                List<string> directorys = new List<string>();
                string direc = string.Empty;
                foreach (var d in archivos)
                {
                    if (d.Contains("/"))
                    {
                        if (direc.Split('/')[0] != d.Split('/')[0])
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(path + d);

                            foreach (FileInfo de in di.GetFiles())
                            {
                                de.Delete();
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                            }
                            direc = d;
                            directorys.Add(direc.Split('/')[0]);
                        }
                    }
                    else { System.IO.File.Delete(path + d); }
                }
                foreach (var d in directorys)
                {
                    System.IO.Directory.Delete(path + d);
                }
            }
            #endregion
        }
    }
}
