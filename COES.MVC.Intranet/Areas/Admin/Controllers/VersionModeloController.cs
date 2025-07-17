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
    public class VersionModeloController : BaseController
    {
        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";
    

        public string Folder
        {
            get { return Session["Folder"].ToString(); }
            set { Session["Folder"] = value; }
        }

        public string NombreAplicativo
        {
            get { return Session["NombreAplicativo"].ToString(); }
            set { Session["NombreAplicativo"] = value; }
        }


        public int IdModelo
        {
            get { return int.Parse(Session["IdModelo"].ToString()); }
            set { Session["IdModelo"] = value; }
        }


        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ModplanAppServicio servicio = new ModplanAppServicio();

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string id)
        {
            VersionModeloModel model = new VersionModeloModel();
            
            if (id == "xgsddd")
            {
                this.Folder = @"Modelos\Yupana\";
                this.IdModelo = 2;
                model.NombreAplicativo = "Yupana";
                this.NombreAplicativo = "Yupana";
                model.Indicador = Constantes.SI;
            }
            else if (id == "sdwfacc")
            {
                this.Folder = @"Modelos\Arpay\";
                this.IdModelo = 3;
                model.NombreAplicativo = "Arpay";
                this.NombreAplicativo = "Arpay";
                model.Indicador = Constantes.SI;
            }
            else if (id == "frfagr")
            {
                this.Folder = @"Modelos\Quipu\";
                this.IdModelo = 4;
                model.NombreAplicativo = "Quipu";
                this.NombreAplicativo = "Quipu";
                model.Indicador = Constantes.SI;
            }
            else if (id == "qewrdfwer")
            {
                this.Folder = @"Modelos\Kumpliy\";
                this.IdModelo = 5;
                model.NombreAplicativo = "KUMPLIY";
                this.NombreAplicativo = "KUMPLIY";
                model.Indicador = Constantes.SI;
            }
            else
            {
                model.Indicador = Constantes.NO;
            }

            return View(model);
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado()
        {
            VersionModeloModel model = new VersionModeloModel();
            model.Listado = this.servicio.ListWbVersionModplans(this.IdModelo);
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
            VersionModeloModel model = new VersionModeloModel();
            model.ListaPadres = this.servicio.ObtenerVersionPorPadre(-1, this.IdModelo);

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
        public JsonResult Grabar(VersionModeloModel model)
        {
            try
            {
                WbVersionModplanDTO entity = new WbVersionModplanDTO();
                entity.Vermplcodi = model.Codigo;
                entity.Vermpldesc = model.Descripcion;
                entity.Vermplestado = model.Estado;
                entity.Vermpltipo = this.IdModelo;
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
            VersionModeloModel model = new VersionModeloModel();
            model.Codigo = id;
            string url = "VERSION_" + id + @"\";
            WbVersionModplanDTO entity = this.servicio.ObtenerDetalleVersionAdicional(id, pathModelo, this.Folder + url);
            WbVersionModplanDTO entityPadre = this.servicio.GetByIdWbVersionModplan((int)entity.Vermplpadre);
            model.ListadoModelo = entity.ListadoArchivos.Where(x => x.Arcmplindtc == 1.ToString()).ToList();
            model.ListadoManual = entity.ListadoArchivos.Where(x => x.Arcmplindtc == 2.ToString()).ToList();
            model.NombreVersion = entity.Vermpldesc;
            model.NombrePlan = entityPadre.Vermpldesc;
            model.NombreAplicativo = this.NombreAplicativo;

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
            VersionModeloModel model = new VersionModeloModel();
            model.ListaReporte = this.servicio.ObtenerReporteDescarga(id, this.IdModelo);
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
                string nombreFinal = name.Split('.')[0];

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
                            if (!FileServer.VerificarExistenciaDirectorio(this.Folder + url, pathModelo))
                            {
                                FileServer.CreateFolder(this.Folder, url, pathModelo);
                            }

                            if (FileServer.VerificarExistenciaFile(url, nombreFile, pathModelo))
                            {
                                FileServer.DeleteBlob(url + nombreFile, pathModelo);
                            }

                            FileServer.UploadFromFileDirectory(path + name, this.Folder + url, nombreFile, pathModelo);

                            WbArchivoModplanDTO entity = new WbArchivoModplanDTO();
                            entity.Arcmplindtc = indicador.ToString();
                            entity.Arcmplestado = Constantes.EstadoActivo;
                            entity.Vermplcodi = idVersion;
                            entity.Arcmplnombre = nombreFinal;
                            entity.Arcmplext = extension;
                            entity.Arcmpltipo = this.IdModelo;
                            this.servicio.SaveWbArchivoModelo(entity);                           
                        }
                    }
                    else
                    {                        
                        if (!FileServer.VerificarExistenciaDirectorio(this.Folder + url, pathModelo))
                        {
                            FileServer.CreateFolder(this.Folder, url, pathModelo);
                        }

                        if (FileServer.VerificarExistenciaFile(this.Folder + url, nombreFile, pathModelo))
                        {
                            FileServer.DeleteBlob(this.Folder + url + nombreFile, pathModelo);
                        }

                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, this.Folder + url, nombreFile, pathModelo);

                        WbArchivoModplanDTO entity = new WbArchivoModplanDTO();
                        entity.Arcmplindtc = indicador.ToString();
                        entity.Arcmplestado = Constantes.EstadoActivo;
                        entity.Vermplcodi = idVersion;
                        entity.Arcmplnombre = nombreFinal;
                        entity.Arcmplext = extension;
                        entity.Arcmpltipo = this.IdModelo;
                        this.servicio.SaveWbArchivoModelo(entity);
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
            Stream stream = this.servicio.ObtenerArchivoAdicional(idVersion, indicador, pathModelo, this.Folder + url, out extension, out nombre);      
            return File(stream, extension, nombre);
        }

        [HttpPost]
        public PartialViewResult EditArchivo(int idVersion, string indicador)
        {
            VersionModeloModel model = new VersionModeloModel();
            WbArchivoModplanDTO entity = this.servicio.ObtenerDetalleArchivo(indicador);
            model.ArchivoModel = entity;
            return PartialView(model);

        }

        [HttpPost]
        public JsonResult GrabarFile(int idVersion, int idFile, string nombre, string descripcion)
        {
            try
            {
                string url = "VERSION_" + idVersion + @"\";
                this.servicio.EditarFile(idFile, nombre, descripcion, this.Folder + url, pathModelo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
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
                this.servicio.EliminarArchivoAdicional(idVersion, indicador, pathModelo, this.Folder + url);            
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
