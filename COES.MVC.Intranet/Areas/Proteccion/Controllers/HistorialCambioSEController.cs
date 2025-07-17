using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
namespace COES.MVC.Intranet.Areas.Proteccion.Controllers
{
    public class HistorialCambioSEController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        /// 
        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        HistorialCambioSEAppServicio historialCambio = new HistorialCambioSEAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(HistorialCambioSEController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public HistorialCambioSEController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            HistorialCambioSEModel modelo = new HistorialCambioSEModel();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.listaSubestacion = equipoProteccion.ListSubEstacion();

            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(HistorialCambioSEModel datos)
        {
            return View(datos);
        }

        public PartialViewResult Editar(int id, string accion)
        {
            var model = new HistorialCambioSEEditarModel();
            model.listaSubestacion = equipoProteccion.ListSubEstacion();
            model.listaProyecto = equipoProteccion.ListProyectoProyectoActualizacion(id);
            model.accion = accion;

            if (accion == ConstantesProteccion.Editar || accion == ConstantesProteccion.Consulta)
            {
                EprSubestacionDTO se = historialCambio.GetById(id);


                model.epsubecodi = se.Epsubecodi;
                model.areacodi = se.Areacodi;
                model.epproycodi = se.Epproycodi;
                model.epsubemotivo = se.Epsubemotivo;
                model.epsubefecha = se.Epsubefecha;
                model.epsubememoriacalculo = se.Epsubememoriacalculo;
                model.epsubememoriacalculoTexto = ProteccionHelper.modificarNombreArchivo(se.Epsubememoriacalculo);

            }

            return PartialView("~/Areas/Proteccion/Views/HistorialCambioSE/Editar.cshtml", model);
        }


     

        /// <summary>
        /// Recibe los archivos de medida de cálculo
        /// </summary>
        /// <param name="epsubecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int epsubecodi)
        {
            string sNombreArchivo = "";
            string sNombreArchivoOriginal = "";
            string path = "";
            string pathTemporal = "";

            HistorialCambioSEFileModel model =new HistorialCambioSEFileModel();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string extension = file.FileName.Split('.').Last();
                    var nombreOriginal = file.FileName.Split('.').First();
                    sNombreArchivo = equipoProteccion.GetNombreArchivoFormato(epsubecodi.ToString(), "SE", extension, nombreOriginal);
                    sNombreArchivoOriginal = file.FileName;

                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    string fileName = ruta + file.FileName;
                    string ruta2 = string.Format("{0}\\{1}\\", base.PathFiles, ConstantesProteccion.FolderSubestacion);
                    path = FileServer.GetDirectory() + ruta2;

                    file.SaveAs(fileName); //Guarda en temporal con nombfisico
                    pathTemporal = Server.MapPath("~/Uploads/") + file.FileName;

                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderGestProtec, "");
                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderMemoriaCalculo, "");
                    FileServer.CreateFolder(base.PathFiles, ConstantesProteccion.FolderSubestacion, "");
                    FileServer.UploadFromFileDirectory(pathTemporal, ruta2, sNombreArchivo, string.Empty); //graba en raiz

                    //Elimina el archivo temporal
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                }

                model.estado = 1;
                model.nombreArchivo = sNombreArchivo;
                model.nombreArchivoTexto = ProteccionHelper.modificarNombreArchivo(sNombreArchivo);
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.estado = -1;
                model.nombreArchivo = sNombreArchivo;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Guardar(HistorialCambioSEEditarModel model)
        {
            try
            {
                EprSubestacionDTO se = null;

                int existeCodigo = 0;

                List<EprSubestacionDTO> lHistorialCambio = historialCambio.List(0, 0).ToList();

                if (model.accion == ConstantesProteccion.Editar)
                {
                    foreach (var item in lHistorialCambio)
                    {

                        if ((item.Epsubecodi != model.epsubecodi) && (item.Areacodi.Equals(model.areacodi) && item.Epsubefecha.Equals(model.epsubefecha)))
                        {
                            existeCodigo++;
                        }
                    }   
                }
                else
                {
                    foreach (var item in lHistorialCambio)
                    {
                        if (item.Areacodi.Equals(model.areacodi) && item.Epsubefecha.Equals(model.epsubefecha))
                        {
                            existeCodigo++;
                        }
                    }
                }


                if (existeCodigo > 0)
                {
                    return Json(2);
                }

                if (model.accion == ConstantesProteccion.Editar)
                {
                    se = historialCambio.GetById(model.epsubecodi);
                    se.Epsubecodi = model.epsubecodi;
                    se.Epproycodi = model.epproycodi;
                    se.Epsubemotivo = model.epsubemotivo;
                    se.Epsubefecha = model.epsubefecha;
                    se.Epsubememoriacalculo = model.epsubememoriacalculo;
                    se.Epsubeestregistro = ConstantesProteccion.Activo;
                    se.Epsubeusucreacion = User.Identity.Name;
                    se.Areacodi = model.areacodi;
                    historialCambio.Update(se);
                }
                else
                {
                    se = new EprSubestacionDTO();
                    se.Epsubecodi = model.epsubecodi;
                    se.Epproycodi = model.epproycodi;
                    se.Epsubemotivo = model.epsubemotivo;
                    se.Epsubefecha = model.epsubefecha;
                    se.Epsubememoriacalculo = model.epsubememoriacalculo;
                    se.Epsubeestregistro = ConstantesProteccion.Activo;
                    se.Epsubeusucreacion = User.Identity.Name;
                    se.Areacodi = model.areacodi;
                    historialCambio.Save(se);
                }


                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(-1);
            }
        }

        public JsonResult Eliminar(int epsubecodi)
        {
            try
            {
                EprSubestacionDTO se = null;

                se = new EprSubestacionDTO();
                se.Epsubecodi = epsubecodi;
                se.Epsubeestregistro = ConstantesProteccion.Inactivo;
                se.Epsubeusucreacion = User.Identity.Name;
                historialCambio.Delete_UpdateAuditoria(se);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(-1);
            }
        }




        [HttpPost]
        public PartialViewResult ListaHistorialCambioSE(int areacodi, int zonacodi)
        {
            ListadoHistorialCambioSEModel model = new ListadoHistorialCambioSEModel();
            var lista = historialCambio.List(areacodi, zonacodi).ToList();

            List<EprSubestacionDTO> ListModificada = new List<EprSubestacionDTO>();

            foreach (var item in lista) {
                item.EpsubememoriacalculoText = ProteccionHelper.modificarNombreArchivo(item.Epsubememoriacalculo);
                ListModificada.Add(item);
            }

            model.ListaHistorialCambioSE = ListModificada;
           

            return PartialView("~/Areas/Proteccion/Views/HistorialCambioSE/Lista.cshtml", model);
        }

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="epsubecodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivo(string fileName, int epsubecodi)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new ProteccionHelper().GetBufferArchivoAdjunto(epsubecodi, fileName, base.PathFiles, ConstantesProteccion.FolderSubestacion);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, ProteccionHelper.modificarNombreArchivo(fileName));
            }
            catch (Exception ex) 
            { 
                Log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }

        [HttpPost]
        public JsonResult GenerarArchivoZip(int areacodi, int zonacodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string carpetaTemporal = "ZipRele_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second);
                string nameFile = carpetaTemporal + ".zip";

                List<EprEquipoDTO> lExportar = equipoProteccion.ArchivoZipHistarialCambio(areacodi, zonacodi).ToList();

                new ProteccionHelper().GenerarArchizoZipHC(nameFile, lExportar, base.PathFiles, carpetaTemporal);

                model.Resultado = nameFile;
                model.Ruta = carpetaTemporal;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpGet]
        public virtual FileResult ExportarArchivoZip()
        {
            string nombreArchivo = Request["file_name"];
            string rutaTemporal = Request["ruta"];
            string rutaTemporalCompleta = FileServer.GetDirectory() + base.PathFiles + "/" + ConstantesProteccion.FolderTemporal + "/" + rutaTemporal;
            DirectoryInfo directory = new DirectoryInfo(rutaTemporalCompleta);

            if (directory.Exists)
            {
                directory.Delete(true);
            }

            string ruta = FileServer.GetDirectory() + base.PathFiles + "/" + ConstantesProteccion.FolderArchivoZIP + "/" + nombreArchivo;
            byte[] buffer = null;

            if (System.IO.File.Exists(ruta))
            {
                buffer = System.IO.File.ReadAllBytes(ruta);
                System.IO.File.Delete(ruta);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
        }

    }


}
