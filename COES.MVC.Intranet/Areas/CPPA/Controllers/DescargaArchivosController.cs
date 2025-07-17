using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.Dominio.DTO.Transferencias;
using System.Text.Json;
using COES.Servicios.Aplicacion.CPPA.Helper;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using COES.MVC.Intranet.Controllers;
using WebGrease.Activities;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class DescargaArchivosController : BaseController
    {
        private readonly CPPAAppServicio servicio = new CPPAAppServicio();

        public DescargaArchivosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ActionResult Index(int? anio, string ajuste, int? revision, int? emprcodi)
        {
            if (anio.HasValue)
            {
                ViewBag.Anio = anio;
                ViewBag.Ajuste = ajuste;
                ViewBag.Revision = revision;
                ViewBag.Emprcodi = emprcodi;
            }
            CPPAModel model = new CPPAModel();
            model.ListaAnio = servicio.ObtenerAnios(out List<CpaRevisionDTO> ListRevision);
            model.ListaEmpresasDescarga = servicio.ListaEmpresasDescargaIntegrantes();
            SiEmpresaDTO opTodas = new SiEmpresaDTO() { 
                Emprcodi = -2,
                Emprnomb = "TODAS"
            };
            model.ListaEmpresasDescarga.Insert(0, opTodas);

            ViewBag.ListRevision = Newtonsoft.Json.JsonConvert.SerializeObject(ListRevision);
            return View(model);
        }

        /// <summary>
        /// Lista los documentos segun parametros 
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        ///public JsonResult ListaDocumentosGrilla(int cparcodi, string inicio, string fin, int emprcodi)
        public JsonResult ListaDocumentosGrilla(int cparcodi, int emprcodi)
        {
            CPPAModel model = new CPPAModel()
            {
                ListaDocumentosGrilla = servicio.GetDocumentosByFilters(cparcodi, User.Identity.Name, emprcodi)
            };

            return Json(model);
        }

        /// <summary>
        /// Interfaz donde se visualiza los documentos registrados por envio
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ajuste"></param>
        /// <param name="revision"></param>
        /// <param name="idRevision"></param>
        /// <returns></returns>
        public ActionResult ListaDetalleGrilla(int documento, int anio, string ajuste, int revision, string revisionName, string envio, int emprcodi, string emprnomb)
        {
            ViewBag.Documento = documento;
            ViewBag.Anio = anio;
            ViewBag.Ajuste = ajuste;
            ViewBag.Revision = revision;
            ViewBag.RevisionName = revisionName;
            ViewBag.Envio = envio;
            ViewBag.Emprcodi = emprcodi;
            ViewBag.Emprnomb = emprnomb;

            return View();
        }

        /// <summary>
        /// Lista el detalle de un documento
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public JsonResult ListaDocumentosDetalleGrilla(int documento)
        {
            CPPAModel model = new CPPAModel()
            {
                ListaDocumentosDetalleGrilla = servicio.GetDetalleByDocumento(documento)
            };

            return Json(model);
        }

        [HttpGet]
        public ActionResult DescargarDocumento(string rutaArchivo)
        {
            if (System.IO.File.Exists(rutaArchivo))
            {
                var fileName = Path.GetFileName(rutaArchivo);
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaArchivo);

                var JsonResult = Json(new
                {
                    success = true,
                    fileName = fileName,
                    fileBytes = Convert.ToBase64String(fileBytes)
                }, JsonRequestBehavior.AllowGet);

                JsonResult.MaxJsonLength = Int32.MaxValue;

                return JsonResult;
                //return Json(new
                //{
                //    success = true,
                //    fileName = fileName,
                //    fileBytes = Convert.ToBase64String(fileBytes)
                //}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult DescargarDocumentos(string rutasArchivo, int anio, string ajuste, string revision, int envio, string empresa)
        {
            string[] rutasArray = rutasArchivo.Split(',');

            string revisionNombre = (revision == "Normal") ? "" : $"_{revision}";
            string empresaRecortada = empresa.Length > 15 ? empresa.Substring(0, 15) : empresa;
            var zipFileName = $"{anio}_{ajuste}{revisionNombre}_{empresaRecortada}_{envio}";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var ruta in rutasArray)
                    {
                        if (System.IO.File.Exists(ruta))
                        {
                            var fileName = Path.GetFileName(ruta);
                            var zipEntry = archive.CreateEntry(fileName);

                            try
                            {
                                using (var fileStream = System.IO.File.OpenRead(ruta))
                                using (var entryStream = zipEntry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al copiar el archivo {fileName}:                  {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"El archivo {ruta} no existe.");
                        }
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);

                byte[] zipBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(zipBytes);

                var JsonResult = Json(new
                {
                    success = true,
                    fileName = zipFileName,
                    fileBytes = base64String
                }, JsonRequestBehavior.AllowGet);

                JsonResult.MaxJsonLength = Int32.MaxValue;

                return JsonResult;
            }
        }

        [HttpGet]
        public ActionResult DescargaDocumentosZipeados(int documento, int anio, string ajuste, string revision, int envio, string empresa)
        {
            List<CpaDocumentosDetalleDTO> detalle = servicio.GetDetalleByDocumento(documento);
            string[] rutasArray = detalle.Select(d => d.Cpaddtruta).ToArray();

            string revisionNombre = (revision == "Normal") ? "" : $"_{revision}";
            string empresaRecortada = empresa.Length > 15 ? empresa.Substring(0, 15) : empresa;
            var zipFileName = $"{anio}_{ajuste}{revisionNombre}_{empresaRecortada}_{envio}";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var ruta in rutasArray)
                    {
                        if (System.IO.File.Exists(ruta))
                        {
                            var fileName = Path.GetFileName(ruta);
                            var zipEntry = archive.CreateEntry(fileName);

                            try
                            {
                                using (var fileStream = System.IO.File.OpenRead(ruta))
                                using (var entryStream = zipEntry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al copiar el archivo {fileName}:                  {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"El archivo {ruta} no existe.");
                        }
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);

                byte[] zipBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(zipBytes);

                var JsonResult = Json(new
                {
                    success = true,
                    fileName = zipFileName,
                    fileBytes = base64String
                }, JsonRequestBehavior.AllowGet);

                JsonResult.MaxJsonLength = Int32.MaxValue;

                return JsonResult;
            }
        }

    }
}