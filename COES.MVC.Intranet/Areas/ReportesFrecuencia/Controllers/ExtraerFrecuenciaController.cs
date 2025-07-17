using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System.Reflection;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ExtraerFrecuenciaController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ExtraerFrecuenciaModel model = new ExtraerFrecuenciaModel();

            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            model.FechaFinal = System.DateTime.Now;
            model.FechaInicial = System.DateTime.Now.AddMonths(-12);

            List<ExtraerFrecuenciaDTO> lista = new List<ExtraerFrecuenciaDTO>();
            model.ListaExtraerFrecuencia = lista;

            return View(model);
        }

        public ActionResult Lista(ExtraerFrecuenciaModel model)
        {
            //ExtraerFrecuenciaModel model = new ExtraerFrecuenciaModel();
            //model.ListaExtraerFrecuencia = new ExtraerFrecuenciaAppServicio().GetListaExtraerFrecuencia();
            string mensajeError = "";
            if (DateTime.ParseExact(model.FechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else
            {
                model.ListaExtraerFrecuencia = new ExtraerFrecuenciaAppServicio().GetListaExtraerFrecuencia(model.FechaIni, model.FechaFin);

            }
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ExtraerFrecuenciaModel modelo = new ExtraerFrecuenciaModel();
            modelo.Entidad = new ExtraerFrecuenciaDTO();
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            modelo.ListaEquipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(m => m.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(modelo.ListaEquipos, "GPSCODI", "NOMBREEQUIPO", modelo.Entidad.GPSCodi);


            return View(modelo);
        }

        public ActionResult VerRegistroCarga(int IdCarga)
        {
            ExtraerFrecuenciaModel modelo = new ExtraerFrecuenciaModel();
            modelo.Entidad = new ExtraerFrecuenciaAppServicio().GetBydId(IdCarga);
            modelo.ListaMilisegundos = new ExtraerFrecuenciaAppServicio().GetListaMilisegundos(IdCarga);

            return View(modelo);
        }

        /// <summary>
        /// Genera el reporte excel 
        /// </summary>
        /// <param name="IdCarga"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcel(int IdCarga)
        {
            int indicador = 1;
            try
            {
                ExtraerFrecuenciaModel modelo = new ExtraerFrecuenciaModel();
                modelo.Entidad = new ExtraerFrecuenciaAppServicio().GetBydId(IdCarga);
                modelo.ListaMilisegundos = new ExtraerFrecuenciaAppServicio().GetListaMilisegundos(IdCarga);

                ExcelDocument.GenerarArchivoReporteMilisegundos(modelo);
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                throw ex;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = FormatoReportesFrecuencia.NombreReporteMilisegundos;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);

        }

        public ActionResult Save(ExtraerFrecuenciaModel modelo)
        {
            string strFileName = string.Empty;
            string mensajeError = "Se ha producido un error al insertar la información";
            try
            {
                if (modelo.sAccion == "Grabar")
                {
                    if (Convert.ToDateTime(modelo.Entidad.FechaHoraInicio) > Convert.ToDateTime(modelo.Entidad.FechaHoraFin))
                    {
                        mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                        TempData["sMensajeExito"] = mensajeError;
                    }
                    else
                    {
                        EquipoGPSDTO equipoDTO = new EquipoGPSDTO();
                        equipoDTO = new EquipoGPSAppServicio().GetBydId(modelo.Entidad.GPSCodi);
                        if (string.IsNullOrEmpty(equipoDTO.RutaFile))
                        {
                            mensajeError = "No existe ruta configurada para ese equipo GPS.";
                            TempData["sMensajeExito"] = mensajeError;
                        }
                        else
                        {
                            modelo.Entidad.RutaFile = equipoDTO.RutaFile;
                            ResultadoDTO<ExtraerFrecuenciaDTO> resultado = new ExtraerFrecuenciaAppServicio().Save(modelo.Entidad);
                            if (resultado.EsCorrecto >= 0)
                            {
                                TempData["sMensajeExito"] = "Se ha registrado correctamente.";
                                mensajeError = "";
                            }
                            else
                            {
                                mensajeError = resultado.Mensaje;
                                TempData["sMensajeExito"] = mensajeError;
                            }
                        }

                    }

                }
                modelo.sError = mensajeError;
                return Json(modelo, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                mensajeError = "Hubo un error en la lectura de archivos.";
                TempData["sMensajeExito"] = mensajeError;
                modelo.sError = mensajeError;
                return Json(modelo, JsonRequestBehavior.AllowGet);
            }            
        }

        /// <summary>
        /// Descargar manual de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesReportesFrecuencia.ModuloManualUsuario;
            string nombreArchivo = ConstantesReportesFrecuencia.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesReportesFrecuencia.FolderRaizAutomatizacionModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);
                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                {
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}