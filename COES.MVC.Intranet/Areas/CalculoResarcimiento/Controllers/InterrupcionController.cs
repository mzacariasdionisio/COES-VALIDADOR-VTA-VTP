using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class InterrupcionController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();

        /// <summary>
        /// Permite mostrar la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InterrupcionModel model = new InterrupcionModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicio.ObtenerPeriodosPorAnio(model.Anio);
            model.ListaEmpresas = this.servicio.ObtenerEmpresasSuministradorasTotal();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }              

        /// <summary>
        /// Permite mostrar los periodos por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicio.ObtenerPeriodosPorAnio(anio));
        }

        /// <summary>
        /// Permite realizar la consulta de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int empresa, int periodo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            EstructuraInterrupcion estructura = this.servicio.ObtenerEstructuraInterrupciones(empresa, periodo, true);
            estructura.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int empresa, int periodo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
            string template = ConstantesCalculoResarcimiento.PlantillaInterrupcion;
            string file = ConstantesCalculoResarcimiento.FormatoCargaInterrupcion;

            return Json(this.servicio.GenerarFormatoInterrupciones(path, template, file, empresa, periodo));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.FormatoCargaInterrupcion;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.FormatoCargaInterrupcion);
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionInterrupcion;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importacion de suministros
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInterrupciones(int empresa, int periodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionInterrupcion;
               
                List<string> validaciones = new List<string>();
                List<EstructuraCargaFile> archivos = new List<EstructuraCargaFile>();
                string[][] data = this.servicio.CargarInterrupcionesExcel(path, file, empresa, periodo, out validaciones, out archivos);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Data = data, Errores = new List<string>() });
                else
                    return Json(new { Result = 2, Data = new string[1][], Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Data = new string[1][], Errores = new List<string>() });
            }
        }

        /// <summary>
        /// Permite grabar los datos de interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarInterrupciones(string[][] data, int empresa, int periodo, string comentario) 
        {
            string path = this.servicio.ObtenerPathArchivosResarcimiento();
            return Json(this.servicio.GrabarInterrupciones(data, empresa, periodo, base.UserName, comentario, path, ConstantesCalculoResarcimiento.FuenteIngresoIntranet));
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularInterrupcion(int idPeriodo, int idInterrupcion, string comentario)
        {
            return Json(this.servicio.AnularInterrupcion(idPeriodo, idInterrupcion, comentario, base.UserName, ConstantesCalculoResarcimiento.FuenteIngresoIntranet, 
                string.Empty, null));
        }

        /// <summary>
        /// Permite obtener los envios de interrpciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Envios(int idEmpresa, int idPeriodo)
        {
            return Json(this.servicio.ObtenerEnviosInterrupciones(idEmpresa, idPeriodo, 
                ConstantesCalculoResarcimiento.EnvioTipoInterrupcion));
        }

        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult UploadArchivoInterrupcion(int? id, int? row)
        {
            try
            {
                string extension = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = this.servicio.ObtenerPathArchivosResarcimiento();
                    string fileName = string.Empty;
                    string archivo = file.FileName;
                    int indexOf = archivo.LastIndexOf('.');
                    extension = archivo.Substring(indexOf + 1, archivo.Length - indexOf - 1);

                    if (id != null) {
                        fileName =  string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);
                    }
                    else
                    {
                        fileName = string.Format(ConstantesCalculoResarcimiento.TemporalInterrupcion,row.ToString(), extension);
                    }

                    /*if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);*/
                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    /*
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);*/

                    FileServer.UploadFromStream(file.InputStream, ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    if (id != null)
                    {
                        this.servicio.GrabarArchivoInterrupcion((int)id, extension);
                    }
                }
                return Json(new { success = true, indicador = 1, extension= extension }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoInterrupcion(int? id, int? row, string extension)
        {
            string fileName = string.Empty;
            if (id != null)
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);            
            else
                fileName =  string.Format(ConstantesCalculoResarcimiento.TemporalInterrupcion, row.ToString(), extension);

            /*string fullPath = this.servicio.ObtenerPathArchivosResarcimiento() +
                fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite eliminar el archivo de una interrupcion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarArchivoInterrupcion(int? id, int? row, string extension)
        {
            string fileName = string.Empty;
            if (id != null)
            {
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);
                this.servicio.GrabarArchivoInterrupcion((int)id, string.Empty);
            }
            else
                fileName = string.Format(ConstantesCalculoResarcimiento.TemporalInterrupcion, row.ToString(), extension);

            /*string fullPath = this.servicio.ObtenerPathArchivosResarcimiento() +
                fileName;
            */
            return Json(this.servicio.EliminarArchivoInterrupcion(fileName));
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesCalculoResarcimiento.ModuloManualUsuario;
            string nombreArchivo = ConstantesCalculoResarcimiento.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesCalculoResarcimiento.FolderRaizResarcimientosModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}
