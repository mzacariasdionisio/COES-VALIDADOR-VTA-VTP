using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Planificacion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class VersionModplanController : BaseController
    {
        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";
        string folder = @"Modelos\ModPlan\";
        int IdModplan = 1;

        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ModplanAppServicio servicio = new ModplanAppServicio();

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string tipo)
        {
            return View();
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado()
        {
            VersionModplanModel model = new VersionModplanModel();
            model.Listado = this.servicio.ListWbVersionModplans(this.IdModplan);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el formulario de edicion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int id, int tipo)
        {
            VersionModplanModel model = new VersionModplanModel();
            model.ListaPadres = this.servicio.ObtenerVersionPorPadre(-1, this.IdModplan);

            if (id > 0)
            {
                model.Entidad = this.servicio.GetByIdWbVersionModplan(id);
            }
            else
            {
                model.Entidad = new WbVersionModplanDTO();
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Grabar(VersionModplanModel model)
        {
            try
            {
                WbVersionModplanDTO entity = new WbVersionModplanDTO();
                entity.Vermplcodi = model.Codigo;
                entity.Vermpldesc = model.Descripcion;
                entity.Vermplestado = model.Estado;
                entity.Vermpltipo = this.IdModplan;
                entity.Vermplpadre = (model.Padre == null) ? -1 : model.Padre;
                if (model.Codigo == 0)
                {
                    entity.Vermplusucreacion = base.UserName;
                    entity.Vermplfeccreacion = DateTime.Now;
                    entity.Vermplusumodificacion = base.UserName;
                    entity.Vermplfecmodificacion = DateTime.Now;
                }
                else
                {                    
                    entity.Vermplusumodificacion = base.UserName;
                    entity.Vermplfecmodificacion = DateTime.Now;
                }

                this.servicio.SaveWbVersionModplan(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el registro del modplan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id, int tipo)
        {
            try
            {
                this.servicio.DeleteWbVersionModplan(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite visualizar el detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int id)
        {
            VersionModplanModel model = new VersionModplanModel();
            model.Codigo = id;
            string url = "VERSION_" + id + @"\";
            WbVersionModplanDTO entity = this.servicio.ObtenerDetalleVersion(id, pathModelo, folder + url);
            WbVersionModplanDTO entityPadre = this.servicio.GetByIdWbVersionModplan((int)entity.Vermplpadre);
            
            model.NombreVersion = entity.Vermpldesc;
            model.NombrePlan = entityPadre.Vermpldesc;
            model.NombreModelo = entity.RutaModelo;
            model.NombreManual = entity.RutaManual;

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el reporte de descargas de una versión
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte(int id)
        {
            VersionModplanModel model = new VersionModplanModel();
            model.ListaReporte = this.servicio.ObtenerReporteDescarga(id, this.IdModplan);
            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar los documentos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int chunks, int chunk, string name, int idVersion, int indicador)
        {
            try
            {
               
                string url = "VERSION_" + idVersion + @"\";
                string nombreFile = name;
                string extension = name.Split('.')[1];
               

                if (Request.Files.Count > 0)
                {
                    if (chunks > 1)
                    {
                        var file = Request.Files[0];
                        string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {
                            if (!FileServer.VerificarExistenciaDirectorio(folder + url, pathModelo))
                            {
                                FileServer.CreateFolder(folder, url, pathModelo);
                            }

                            if (FileServer.VerificarExistenciaFile(url, nombreFile, pathModelo))
                            {
                                FileServer.DeleteBlob(url + nombreFile, pathModelo);
                            }

                            FileServer.UploadFromFileDirectory(path + name, folder + url, nombreFile, pathModelo);

                            WbArchivoModplanDTO entity = new WbArchivoModplanDTO();
                            entity.Arcmplindtc = indicador.ToString();
                            entity.Arcmplestado = Constantes.EstadoActivo;
                            entity.Vermplcodi = idVersion;
                            entity.Arcmplnombre = nombreFile;
                            entity.Arcmplext = extension;
                            entity.Arcmpltipo = this.IdModplan;
                            this.servicio.SaveWbArchivoModplan(entity);                           
                        }
                    }
                    else
                    {                        
                        if (!FileServer.VerificarExistenciaDirectorio(folder + url, pathModelo))
                        {
                            FileServer.CreateFolder(folder, url, pathModelo);
                        }

                        if (FileServer.VerificarExistenciaFile(folder + url, nombreFile, pathModelo))
                        {
                            FileServer.DeleteBlob(folder + url + nombreFile, pathModelo);
                        }

                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, folder + url, nombreFile, pathModelo);

                        WbArchivoModplanDTO entity = new WbArchivoModplanDTO();
                        entity.Arcmplindtc = indicador.ToString();
                        entity.Arcmplestado = Constantes.EstadoActivo;
                        entity.Vermplcodi = idVersion;
                        entity.Arcmplnombre = nombreFile;
                        entity.Arcmplext = extension;
                        entity.Arcmpltipo = this.IdModplan;
                        this.servicio.SaveWbArchivoModplan(entity);
                    }
                }

                return Json(new { success = true, indicador = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int idVersion, string indicador)
        {
            string url = "VERSION_" + idVersion + @"\";
            string extension = string.Empty;
            string nombre = string.Empty;
            Stream stream = this.servicio.ObtenerArchivo(idVersion, indicador, pathModelo, folder + url, out extension, out nombre);      
            return File(stream, extension, nombre);
        }

        /// <summary>
        /// Permite eliminar el archivo cargado
        /// </summary>
        [HttpPost]
        public JsonResult EliminarArchivo(int idVersion, string indicador)
        {
            try
            {
                string url = "VERSION_" + idVersion + @"\";
                this.servicio.EliminarArchivo(idVersion, indicador, pathModelo, folder + url);            
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
