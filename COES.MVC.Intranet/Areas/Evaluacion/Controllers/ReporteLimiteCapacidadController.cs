using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using System.Configuration;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;


namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class ReporteLimiteCapacidadController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReporteLimiteCapacidadController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ReporteLimiteCapacidadAppServicio reporteServicio = new ReporteLimiteCapacidadAppServicio();
        LineaAppServicio linea = new LineaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ReporteLimiteCapacidadController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ReporteLimiteCapacidadModel modelo = new ReporteLimiteCapacidadModel();

            modelo.ListaRevision = reporteServicio.ListaReporteLimiteCapacidad(0, "");

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoNuevo = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Nuevo, User.Identity.Name);
            bool permisoExportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Exportar, User.Identity.Name);
            bool permisoImportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Importar, User.Identity.Name);

            ViewBag.PermisoExportar = permisoExportar ? "1" : "0";
            ViewBag.PermisoNuevo = permisoNuevo ? "1" : "0";
            ViewBag.PermisoImportar = permisoImportar ? "1" : "0";

            return View(modelo);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(ReporteLimiteCapacidadModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaReporteLimiteCapacidad(int revision, string descripcion)
        {
            ListadoReporteLimiteCapacidadModel model = new ListadoReporteLimiteCapacidadModel();
            List<EprEquipoDTO> listReporte = reporteServicio.ListaReporteLimiteCapacidad(revision, descripcion).ToList();

            EprEquipoDTO reporteUltimo = listReporte.OrderByDescending(p => Convert.ToInt32(p.EprtlcRevision)).FirstOrDefault();
            if (reporteUltimo != null)
            {
                model.RevisionMax = reporteUltimo.EprtlcRevision;
            }

            int id = (Session[DatosSesion.SesionIdOpcion] != null) ? Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]) : 0;
            bool permisoEdicion = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Editar, User.Identity.Name);
            bool permisoEliminar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Eliminar, User.Identity.Name);

            bool permisoExportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Exportar, User.Identity.Name);
            bool permisoImportar = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, Acciones.Importar, User.Identity.Name);

            ViewBag.PermisoEditar = permisoEdicion ? "1" : "0";
            ViewBag.PermisoEliminar = permisoEliminar ? "1" : "0";

            ViewBag.PermisoExportar = permisoExportar ? "1" : "0";
            ViewBag.PermisoImportar = permisoImportar ? "1" : "0";

            model.ListaReporteLimiteCapacidad = listReporte;
            return PartialView("~/Areas/Evaluacion/Views/ReporteLimiteCapacidad/ListaReporteLimiteCapacidad.cshtml", model);
        }

        public ActionResult Editar(int id)
        {
            var model = new ReporteLimiteCapacidadEditarModel();

            if (id == 0)
            {
                model.Accion = "Crear";
            }
            else
            {
                model.Accion = "Modificar";
                EprEquipoDTO reporte = reporteServicio.ObtenerReporteLimiteCapacidadPorId(id);

                if (reporte != null)
                {
                    model.Codigo = reporte.EprtlcCodi;
                    model.Revision = reporte.EprtlcRevision;
                    model.Descripcion = reporte.EprtlcDescripcion;
                    model.EmitidoEl = reporte.EprtlcFecemision;
                    model.ElaboradoPor = reporte.EprtlcUsuelabora;
                    model.RevisadoPor = reporte.EprtlcUsurevisa;
                    model.AprobadoPor = reporte.EprtlcUsuaprueba;
                    model.EsRegistro = reporte.EprtlcEstregistro;
                    model.UsuarioCreacion = reporte.EprtlcUsucreacion;
                    model.FechaCreacion = reporte.EprtlcFeccreacion;
                    model.UsuarioModificacion = reporte.EprtlcUsumodificacion;
                    model.FechaModificacion = reporte.EprtlcFecmodificacion;
                }
            }


            return View("~/Areas/Evaluacion/Views/ReporteLimiteCapacidad/Editar.cshtml", model);
        }

        [HttpPost]
        public JsonResult GuardarReporteLimiteCapacidad(ReporteLimiteCapacidadEditarModel model)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = null;

                oEquipo = new EprEquipoDTO();

                String hourMinute = DateTime.Now.ToString("HH:mm");

                string formato = "dd/MM/yyyy";
                string fecha = "";

                if (model.Codigo != null && model.Codigo != "")
                {
                    fecha = reporteServicio.ObtenerFechaReportePorId(Convert.ToInt32(model.Codigo));
                }
                else
                {
                    fecha = reporteServicio.ObtenerFechaReportePorId(0);
                }

                DateTime dEmitido = DateTime.ParseExact(model.EmitidoEl, formato, CultureInfo.InvariantCulture);
                DateTime dUltFecha = DateTime.Today;
                if (fecha != null && fecha != "")
                {
                    dUltFecha = DateTime.ParseExact(fecha, formato, CultureInfo.InvariantCulture);
                }
                
                DateTime fechaActual = DateTime.Today;

                if ((fecha != null && fecha != "") && (dUltFecha > dEmitido || dUltFecha == dEmitido))
                {
                    return Json(2);
                }
                else if (dEmitido > fechaActual)
                {
                    return Json(3);
                }
                else
                {
                    if (model.Codigo != null && model.Codigo != "")
                    {
                        oEquipo.EprtlcCodi = model.Codigo;
                        oEquipo.EprtlcDescripcion = model.Descripcion;
                        oEquipo.EprtlcFecemision = model.EmitidoEl + " " + hourMinute;
                        oEquipo.EprtlcUsuelabora = model.ElaboradoPor;
                        oEquipo.EprtlcUsurevisa = model.RevisadoPor;
                        oEquipo.EprtlcUsuaprueba = model.AprobadoPor;
                        oEquipo.EprtlcUsumodificacion = User.Identity.Name;

                        reporteServicio.ActualizarReporteLimiteCapacidad(oEquipo);
                    }
                    else
                    {
                        oEquipo.EprtlcCodi = model.Codigo;
                        oEquipo.EprtlcRevision = model.Revision;
                        oEquipo.EprtlcDescripcion = model.Descripcion;
                        oEquipo.EprtlcFecemision = model.EmitidoEl + " " + hourMinute;
                        oEquipo.EprtlcUsuelabora = model.ElaboradoPor;
                        oEquipo.EprtlcUsurevisa = model.RevisadoPor;
                        oEquipo.EprtlcUsuaprueba = model.AprobadoPor;
                        oEquipo.EprtlcEstregistro = "1";
                        oEquipo.EprtlcUsucreacion = User.Identity.Name;

                        reporteServicio.GuardarReporteLimiteCapacidad(oEquipo);
                    }

                    return Json(1);
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }

        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            string resultado = string.Empty;
            try
            {
                EprEquipoDTO oEquipo = new EprEquipoDTO();

                oEquipo.EprtlcCodi = id.ToString();
                oEquipo.EprtlcEstregistro = "0";
                oEquipo.EprtlcUsumodificacion = User.Identity.Name;

                reporteServicio.EliminarReporteLimiteCapacidad(oEquipo);

                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                resultado = "Ocurrio un error";
                return Json(resultado);
            }
        }

        [HttpPost]
        public JsonResult GenerarReporteDesdePlantilla()
        {
            base.ValidarSesionUsuario();

            string rspta = "-1";
            string file = string.Empty;
            string path = ConstantesEvaluacion.FolderTemporal;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
            string pathPlantilla = ConstantesEvaluacion.FolderGestProtec + "\\" + ConstantesEvaluacion.Plantilla;
            string pathArchivoPlantilla = FileServer.GetDirectory() + pathPlantilla + ConstantesEvaluacion.NombrePlantillaWord;
            string extension = ConstantesEvaluacion.NombrePlantillaWord.Split('.').Last();
            string nombreCortado = ConstantesEvaluacion.NombrePlantillaWord.Split('.').First();
            string fecha = DateTime.Now.ToString("yyyyMMddhhmm");
            string nombreDinamico = nombreCortado + "_" + fecha + "." + extension;
            //copiando plantilla a reporte
            FileServer.CopiarFileRename(pathPlantilla, ConstantesEvaluacion.FolderTemporal + "/", ConstantesEvaluacion.NombrePlantillaWord, base.PathFiles, nombreDinamico);


            file = ConstantesEvaluacion.NombrePlantillaWord;
            linea.GenerarReporteDesdePlantillaDEVExpress(nombreDinamico, pathLogo, path);

            rspta = nombreDinamico;

            return Json(rspta);
        }


        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivo(string fileName)
        {
            try
            {
                base.ValidarSesionUsuario();
                byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto(fileName, base.PathFiles, ConstantesEvaluacion.FolderTemporal);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }

        public ActionResult Upload(string fecha)
        {
            try
            {
                string LocalDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
                string path = ConstantesEvaluacion.FolderTemporal;
                string extension = string.Empty;
                string nombreArchivo = string.Empty;
                string nombreArchivoFinal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);

                    extension = System.IO.Path.GetExtension(file.FileName);
                    nombreArchivoFinal = nombreArchivo + "_" + fecha + extension;
                    string fileName = LocalDirectory + path + "/" + nombreArchivoFinal;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    this.RutaCompletaArchivo = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Fatal("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionEvaluacion.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionEvaluacion.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionEvaluacion.RutaCompletaArchivo] = value; }
        }

        public JsonResult CargarPlantilla(string nombreArchivo, string fechaPlantilla)
        {

            try
            {
                base.ValidarSesionJsonResult();

                string pathTemporal = ConstantesEvaluacion.FolderTemporal;
                string nombreSinExtensionTemporal = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                string extensionTemporal = System.IO.Path.GetExtension(nombreArchivo);
                string nombreArchivoTemporal = nombreSinExtensionTemporal + "_" + fechaPlantilla + extensionTemporal;

                string pathPlantilla = ConstantesEvaluacion.FolderGestProtec + "\\" + ConstantesEvaluacion.Plantilla;
                string pathArchivoPlantilla = FileServer.GetDirectory() + pathPlantilla + ConstantesEvaluacion.NombrePlantillaWord;

                if (System.IO.File.Exists(pathArchivoPlantilla))
                {
                    System.IO.File.Delete(pathArchivoPlantilla);
                }

                FileServer.CopiarFileRename(pathTemporal + "/",
                    pathPlantilla,
                    nombreArchivoTemporal,
                    base.PathFiles,
                    ConstantesEvaluacion.NombrePlantillaWord);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(-1);
            }
        }

        public virtual ActionResult DescargarPlantilla()
        {
            try
            {
                base.ValidarSesionUsuario();
                string pathPlantilla = ConstantesEvaluacion.FolderGestProtec + "\\" + ConstantesEvaluacion.Plantilla;
                byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto(ConstantesEvaluacion.NombrePlantillaWord, base.PathFiles, pathPlantilla);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, ConstantesEvaluacion.NombrePlantillaWord);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }

        public JsonResult CargarArchivoReporte(string nombreArchivo, string fechaPlantilla, string revision, int id)
        {

            try
            {
                base.ValidarSesionJsonResult();

                string pathTemporal = ConstantesEvaluacion.FolderTemporal;
                string nombreSinExtensionTemporal = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                string extensionTemporal = System.IO.Path.GetExtension(nombreArchivo);
                string nombreArchivoTemporal = nombreSinExtensionTemporal + "_" + fechaPlantilla + extensionTemporal;

                string pathReporte = ConstantesEvaluacion.FolderReporte;
                string nombreSinExtension = System.IO.Path.GetFileNameWithoutExtension(ConstantesEvaluacion.NombrePlantillaWord);
                string nombreArchivoReporte = nombreSinExtension + "_" + revision + "_" + fechaPlantilla + extensionTemporal;
                string pathArchivoReporte = pathReporte + "/" + nombreArchivoReporte;


                if (System.IO.File.Exists(pathArchivoReporte))
                {
                    System.IO.File.Delete(pathArchivoReporte);
                }

                FileServer.CopiarFileRename(pathTemporal + "/",
                    pathReporte + "/",
                    nombreArchivoTemporal,
                    base.PathFiles,
                    nombreArchivoReporte);

                EprEquipoDTO equipo = new EprEquipoDTO();
                equipo.EprtlcCodi = id.ToString();
                equipo.EprtlcNoarchivo = nombreArchivoReporte;
                equipo.EprtlcUsumodificacion = User.Identity.Name;
                reporteServicio.AgregarEliminarArchivoReporteLimiteCapacidad(equipo);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(-1);
            }
        }

        public virtual ActionResult DescargarArchivoReporte(int id)
        {
            try
            {
                base.ValidarSesionUsuario();
                EprEquipoDTO reporte = reporteServicio.ObtenerReporteLimiteCapacidadPorId(id);

                string pathReporte = ConstantesEvaluacion.FolderReporte + "/";
                byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto(reporte.EprtlcNoarchivo, base.PathFiles, pathReporte);
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, reporte.EprtlcNoarchivo);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return null;
            }
        }

        public JsonResult EliminarArchivoReporte(int id)
        {
            try
            {
                base.ValidarSesionJsonResult();

                EprEquipoDTO reporte = reporteServicio.ObtenerReporteLimiteCapacidadPorId(id);

                string pathReporte = ConstantesEvaluacion.FolderReporte;
                string pathArchivoReporte = pathReporte + "/" + reporte.EprtlcNoarchivo;


                if (System.IO.File.Exists(pathArchivoReporte))
                {
                    System.IO.File.Delete(pathArchivoReporte);
                }

                EprEquipoDTO equipo = new EprEquipoDTO();
                equipo.EprtlcCodi = id.ToString();
                equipo.EprtlcNoarchivo = "";
                equipo.EprtlcUsumodificacion = User.Identity.Name;
                reporteServicio.AgregarEliminarArchivoReporteLimiteCapacidad(equipo);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                var stak = ex.StackTrace.ToString();
                var msgError = ex.Message.ToString();
                return Json(-1);
            }

        }

    }
}
