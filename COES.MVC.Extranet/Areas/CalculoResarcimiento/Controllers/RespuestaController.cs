using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Controllers
{
    public class RespuestaController : BaseController
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
            RespuestaModel model = new RespuestaModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicio.ObtenerPeriodosPorAnio(model.Anio);
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count > 1) ? Constantes.SI : Constantes.NO;
            if (model.ListaEmpresas.Count > 0)
            {
                model.Emprcodi = model.ListaEmpresas[0].Emprcodi;
                model.Emprnombre = model.ListaEmpresas[0].Emprnomb;
            }            

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el dialogo de cambio de empresa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresa()
        {
            RespuestaModel model = new RespuestaModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            return PartialView(model);
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
            result.Content = serializer.Serialize(this.servicio.ObtenerEstructuraRespuestas(empresa, periodo, true));
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
            string template = ConstantesCalculoResarcimiento.PlantillaRespuesta;
            string file = ConstantesCalculoResarcimiento.FormatoCargaRespuesta;

            return Json(this.servicio.GenerarFormatoRespuestas(path, template, file, empresa, periodo));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.FormatoCargaRespuesta;
            return File(fullPath, Constantes.AppExcel, ConstantesCalculoResarcimiento.FormatoCargaRespuesta);
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
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionRespuesta;

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
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionRespuesta;
               
                List<string> validaciones = new List<string>();
                List<EstructuraCargaFile> listadoArchivos = new List<EstructuraCargaFile>();
                string[][] data = this.servicio.CargarRespuestasExcel(path, file, empresa, periodo, out validaciones, out listadoArchivos);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Data = data, Errores = new List<string>(), Archivos = listadoArchivos });
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
            List<TipoError> listErrores = new List<TipoError>();
            EstructuraGrabado result = this.servicio.GrabarRespuestas(data, empresa, periodo, base.UserName, comentario, path);
            return Json(result);
        }

        /// <summary>
        /// Permite anular una interrupcion
        /// </summary>
        /// <param name="idInterrupcion"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AnularInterrupcion(int idPeriodo, int idInterrupcion, string comentario, string recalculo, string[][] data)
        {
            return Json(this.servicio.AnularInterrupcion(idPeriodo, idInterrupcion, comentario, base.UserName,
                ConstantesCalculoResarcimiento.FuenteIngresoExtranet, recalculo, data));
        }


        /// <summary>
        /// Permite obtener los datos de anulacion de una interrupcion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAnulacion(string[][] data, int periodo)
        {
            return Json(this.servicio.ObtenerDatosAnulacion(data, periodo));
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
                ConstantesCalculoResarcimiento.EnvioTipoRespuesta));
        }

        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult UploadArchivoInterrupcion(int? id, int? idDet, int? row)
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

                    if (id != null && idDet == -1) 
                    {
                        fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);
                    }
                    else
                    {
                        int idDetalle = this.servicio.ObtenerIdDetalleInterrupcion((int)id, (int)idDet);
                        fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle, extension);
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
        public virtual ActionResult DescargarArchivoInterrupcion(int? id, int? idDet, int? row, string extension)
        {
            string fileName = string.Empty;
            if (id != null && idDet == -1)
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);
            else
            {
                int idDetalle = this.servicio.ObtenerIdDetalleInterrupcion((int)id, (int)idDet);
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle, extension);
            }
            /*
            string fullPath = this.servicio.ObtenerPathArchivosResarcimiento() +
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
        public JsonResult EliminarArchivoInterrupcion(int? id, int? idDet, int? row, string extension)
        {
            string fileName = string.Empty;
            if (id != null && idDet == -1)
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);
            else
            {
                int idDetalle = this.servicio.ObtenerIdDetalleInterrupcion((int)id, (int)idDet);
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle, extension);
                this.servicio.ActualizarArchivoRespuesta(idDetalle, string.Empty);
            }

            return Json(this.servicio.EliminarArchivoInterrupcion(fileName));
            /*
            string fullPath = this.servicio.ObtenerPathArchivosResarcimiento() +
                fileName;

            return Json(this.servicio.EliminarArchivoInterrupcion(fullPath));*/
        }

        /// <summary>
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoObservacion(int? id, int? col, string extension)
        {
            string fileName = string.Empty;
            if (id != null)
            {
                int idDetalle = this.servicio.ObtenerIdDetalleInterrupcion((int)id, (int)col);
                fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDetalle.ToString(), extension);
            }

            /*string fullPath = this.servicio.ObtenerPathArchivosResarcimiento() +
                fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
              ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite exportar los errores en la grilla
        /// </summary>      
        [HttpPost]
        public JsonResult ExportarErrores(string[][] errores)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoErroresInterrucpcion;
                this.servicio.ExportarErroresInterrupcion(errores, path, file);
                return Json(1);
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
        public virtual ActionResult DescargarErrores(int empresa, int periodo)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                ConstantesCalculoResarcimiento.ArchivoErroresInterrucpcion;
            RePeriodoDTO entityPeriodo = this.servicio.GetByIdRePeriodo(periodo);
            SiEmpresaDTO entityEmpresa = (new EmpresaAppServicio()).ObtenerEmpresa(empresa);
            string fileName = string.Format("Errores_{0}_{1}.xlsx", entityEmpresa.Emprnomb, entityPeriodo.Repernombre);
            return File(fullPath, Constantes.AppExcel, fileName);
        }


        /// <summary>
        /// Permite cargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult UploadZip(int empresa, int periodo, string archivos)
        {
            try
            {
                string extension = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    List<EstructuraCargaFile> archivosList = JsonConvert.DeserializeObject<List<EstructuraCargaFile>>(archivos);

                    if (archivosList == null || !archivosList.Any())
                    {
                        return Json(new { success = false });
                    }

                    string path = this.servicio.ObtenerPathArchivosResarcimiento();
                    string archivo = file.FileName;
                    int indexOf = archivo.LastIndexOf('.');
                    extension = archivo.Substring(indexOf + 1, archivo.Length - indexOf - 1);
                    string fileName = ConstantesCalculoResarcimiento.ArchivoSustentoRespuestas;

                    if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                      ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
                            ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    FileServer.UploadFromStream(file.InputStream, ConstantesCalculoResarcimiento.RutaResarcimientos, fileName,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                    if (FileServer.VerificarExistenciaDirectorio(ConstantesCalculoResarcimiento.RutaResarcimientos + ConstantesCalculoResarcimiento.FolderSustentoRespuesta,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                    {
                        FileServer.DeleteFolderAlter(ConstantesCalculoResarcimiento.RutaResarcimientos + ConstantesCalculoResarcimiento.FolderSustentoRespuesta,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                    }

                    FileServer.DescomprimirZipAlter(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
                        ConstantesCalculoResarcimiento.RutaResarcimientos + ConstantesCalculoResarcimiento.FolderSustentoRespuesta,
                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                    foreach (EstructuraCargaFile item in archivosList)
                    {
                        if (!string.IsNullOrEmpty(item.FileName))
                        {
                            int indexOfInterrupcion = item.FileName.LastIndexOf('.');
                            string extensionInterrupcion = item.FileName.Substring(indexOfInterrupcion + 1, item.FileName.Length - indexOfInterrupcion - 1);

                            string fileInterrupcion = string.Empty;
                            if (item.Id != 0)
                            {
                                fileInterrupcion = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, item.Id, extensionInterrupcion);
                                this.servicio.GrabarArchivoInterrupcion(item.Id, extensionInterrupcion);
                            }
                            else
                                fileInterrupcion = string.Format(ConstantesCalculoResarcimiento.TemporalInterrupcion, item.Fila - 1, extensionInterrupcion);


                            if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileInterrupcion,
                              ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                            {
                                FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + fileInterrupcion,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                            }

                            //- Mover el archivo
                            FileServer.CopiarArchivo(ConstantesCalculoResarcimiento.RutaResarcimientos,
                                ConstantesCalculoResarcimiento.FolderSustentoRespuesta + "/" + item.FileName, fileInterrupcion,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                            //this.servicio.ActualizarArchivoObservacion(item.Id, extensionObservacion);
                        }
                        int orden = 1;
                        foreach(EstructuraCargaFileItem itemFile in item.ListaItems)
                        {
                            if (item.Id != 0 && itemFile.Indicador == 1 && !string.IsNullOrEmpty(itemFile.FileName))
                            {
                                int indexOfRespuesta = itemFile.FileName.LastIndexOf('.');
                                string extensionRespuesta = itemFile.FileName.Substring(indexOfRespuesta + 1, itemFile.FileName.Length - indexOfRespuesta - 1);
                                int idDetalle = this.servicio.ObtenerIdDetalleInterrupcion(item.Id, orden);
                                string fileNameRespuesta = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle, extensionRespuesta);

                                if (FileServer.VerificarExistenciaFile(ConstantesCalculoResarcimiento.RutaResarcimientos, fileNameRespuesta,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento))
                                {
                                    FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos + fileNameRespuesta,
                                        ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);
                                }
                                //- Mover el archivo
                                FileServer.CopiarArchivo(ConstantesCalculoResarcimiento.RutaResarcimientos,
                                    ConstantesCalculoResarcimiento.FolderSustentoRespuesta + "/" + itemFile.FileName, fileNameRespuesta,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                            }
                            orden++;
                        }
                    }

                    foreach (EstructuraCargaFile item in archivosList)
                    {
                        if (!string.IsNullOrEmpty(item.FileName))
                        {
                            //- Mover el archivo
                            FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos +
                                ConstantesCalculoResarcimiento.FolderSustentoRespuesta + "/" + item.FileName,
                                ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                            //this.servicio.ActualizarArchivoObservacion(item.Id, extensionObservacion);
                        }
                        int orden = 1;
                        foreach (EstructuraCargaFileItem itemFile in item.ListaItems)
                        {
                            if (item.Id != 0 && itemFile.Indicador == 1 && !string.IsNullOrEmpty(itemFile.FileName))
                            {                                
                                //- Mover el archivo
                                FileServer.DeleteBlob(ConstantesCalculoResarcimiento.RutaResarcimientos +
                                    ConstantesCalculoResarcimiento.FolderSustentoRespuesta + "/" + itemFile.FileName,
                                    ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);

                            }
                            orden++;
                        }
                    }

                }
                return Json(new { success = true, indicador = 1, extension = extension }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }


}
