using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.IEOD.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class CargaDatosController : BaseController
    {
        readonly CargaDatosAppServicio servicio = new CargaDatosAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Flujos()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaDatosModel model = new CargaDatosModel();
            model.Fecha = DateTime.Today.ToString(Constantes.FormatoFecha);

            model.IdReporteIeodPotencia = ConstantesPR5ReportesServicio.IdReporteFlujo;

            return View(model);
        }

        public ActionResult Tension()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CargaDatosModel model = new CargaDatosModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.IdReporteIeodTension = ConstantesPR5ReportesServicio.IdReporteTension;
            return View(model);
        }

        public ActionResult FlujosIDCOS()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CargaDatosModel model = new CargaDatosModel();
            model.Fecha = DateTime.Today.ToString(Constantes.FormatoFecha);

            model.IdReporteIeodPotencia = ConstantesPR5ReportesServicio.IdReporteFlujoIDCOS;

            return View(model);
        }

        #region Carga de datos

        /// <summary>
        /// Permite realizar la consulta de datos guardados en ME_MEDICION48
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConsultarBD(int idReporte, int tipoinfocodi, string fecha, int tipo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            EstructuraRespuesta estructura = this.servicio.ConsultarDatos(idReporte, tipoinfocodi, fechaConsulta, tipo);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite realizar la consulta de datos Scada (ME_SCADA_SP7 o circulares)
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConsultarScada(int idReporte, int tipoinfocodi, string fecha, int tipo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            EstructuraRespuesta estructura = this.servicio.ConsultarDatosScada(idReporte, tipoinfocodi, fechaConsulta, tipo);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="tipoDato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idReporte, int tipoinfocodi, string fecha, int tipo, int tipoDato)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos;
            string file = ConstantesIEOD.NombreArchivoDatos;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            return Json(this.servicio.GenerarFormatoCarga(idReporte, path, file, tipoinfocodi, fechaConsulta, tipo, tipoDato));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato(int tipo)
        {
            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos, ConstantesIEOD.NombreArchivoDatos);
        }

        /// <summary>
        /// Permite cargar el archivo
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesIEOD.NombreImportacionArchivo;

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
        /// Permite realizar la importacion
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Importar(int idReporte, int tipoinfocodi, string fecha, int tipo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos;
                string file = ConstantesIEOD.NombreImportacionArchivo;
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<string> validaciones = new List<string>();
                EstructuraRespuesta result = this.servicio.CargarDataExcel(idReporte, path, file, tipoinfocodi, fechaConsulta, out validaciones, tipo);

                return Json(new { Result = result.Result, Data = result, Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Data = new EstructuraRespuesta(), Errores = new List<string>() });
            }
        }

        /// <summary>
        /// Permite grabar los datos
        /// </summary>
        /// <param name="data"></param>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDatos(int idReporte, string[][] data, int tipoinfocodi, string fecha, int tipo, List<InformacionCelda> listaInfoCelda)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaRegistro = DateTime.Now;

            int resultado = this.servicio.GrabarMediciones(idReporte, data, listaInfoCelda, tipoinfocodi, fechaConsulta, tipo, base.UserName, fechaRegistro);

            //si se grabó correctamente entonces copiarlo a la lectura antigua
            if (resultado == 1 && ConstantesPR5ReportesServicio.IdReporteFlujo == idReporte && ConstantesPR5ReportesServicio.TipoinfoMW == tipoinfocodi)
            {
                servicio.CopiarDataALecturaCargaIEOD(fechaConsulta, base.UserName, fechaRegistro);
            }

            return Json(resultado);
        }

        #endregion

        #region Consulta

        /// <summary>
        /// Permite mostrar la consulta por rango de fechas
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="reporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ObtenerConsulta(int idReporte, int dia, int tipoinfocodi, string fechaInicio, string fechaFin, int tipo)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            EstructuraRespuesta estructura = this.servicio.ObtenerReporte(idReporte, dia, tipoinfocodi, fecInicio, fecFin, tipo);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarConsulta(int idReporte, int dia, int tipoinfocodi, string fechaInicio, string fechaFin, int tipo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos;
            string file = ConstantesIEOD.NombreArchivoConsulta;
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            return Json(this.servicio.GenererReporteConsulta(idReporte, path, file, dia, tipoinfocodi, fecInicio, fecFin, tipo));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarConsulta(int tipo)
        {
            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos, ConstantesIEOD.NombreArchivoConsulta);
        }

        #endregion

        #region Interconexiones

        /// <summary>
        /// Permite visualizar las interconexiones entre sistemas operativos del SEIN
        /// </summary>
        /// <returns></returns>
        public ActionResult Interconexion()
        {
            if (!IsValidSesion) return base.RedirectToLogin();
            CargaDatosModel model = new CargaDatosModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permote mostrar la consulta por rango de fechas
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="reporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ObtenerInterconexiones(string fecha)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            EstructuraRespuesta estructura = this.servicio.ObtenerReporteInterconexion(fechaConsulta);
            result.Content = serializer.Serialize(estructura);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Permite generar el formato de carga de interrupciones
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarInterconexion(string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos;
            string file = ConstantesIEOD.InterconexionesEntreSSOOSein;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            return Json(this.servicio.ExportarInterconexiones(path, file, fechaConsulta));
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesIEOD.ModuloManualUsuarioSGI;
            string nombreArchivo = ConstantesIEOD.ArchivoManualUsuarioIntranetSGI;
            string pathDestino = modulo + ConstantesIEOD.FolderRaizSGIModuloManual;
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

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarInterconexion()
        {
            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + ConstantesIEOD.RutaArchivos, ConstantesIEOD.InterconexionesEntreSSOOSein);
        }

        #endregion
    }
}
