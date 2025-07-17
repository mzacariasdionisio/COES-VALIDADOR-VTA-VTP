using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.CPPA.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class CalcularPorcentajeController : BaseController
    {
        #region Declaración de variables
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly CPPAAppServicio CppaAppServicio = new CPPAAppServicio();

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

        public CalcularPorcentajeController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        /// <summary>
        /// Pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.sResultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Obtener datos iniciales de la página inicial (index)
        /// </summary>
        /// <returns></returns>
        public JsonResult ObtenerDatosIniciales()
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListaAnio = CppaAppServicio.ObtenerAnios("'A','C'", out List<CpaRevisionDTO> ListRevision);
                model.ListRevision = ListRevision;
                model.sResultado = "1";
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene el log del proceso del Cálculo de Porcentaje para una Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerLogProceso(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                model.sDetalle = CppaAppServicio.GetLogProcesoPorcentaje(cparcodi, out string estadoPublicacion, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales);
                model.ListaReporte = listaReporteMensuales;
                model.ListaReporte2 = listaReporteAnuales;
                model.EstadoPublicacion = estadoPublicacion;
                model.sResultado = "1";
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Procesa el Cálculo de Porcentaje de Presupuesto para una Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarCalculo(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                CppaAppServicio.ProcesarCalculoPorcentaje(cparcodi, User.Identity.Name.Trim(), out string logProceso, out string estadoPublicacion, out List<GenericoDTO> listaReporteMensuales, out List<GenericoDTO> listaReporteAnuales);
                model.sMensaje = "Se PROCESÓ el cálculo de porcentaje de presupuesto para la Revisión del Ajuste del Año Presupuestal seleccionada satisfactoriamente.";
                model.sResultado = "1";
                model.sDetalle = logProceso;
                model.ListaReporte = listaReporteMensuales;
                model.ListaReporte2 = listaReporteAnuales;
                model.EstadoPublicacion = estadoPublicacion;
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina el Cálculo de Porcentaje de Presupuesto para una Revisión.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EliminarCalculo(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                CppaAppServicio.EliminarCalculoPorcentaje(cparcodi);
                model.sMensaje = "Se ELIMINÓ el cálculo de porcentaje de presupuesto para la Revisión del Ajuste del Año Presupuestal seleccionada satisfactoriamente.";
                model.sResultado = "1";
                model.sDetalle = "";
                model.ListaReporte = new List<GenericoDTO>();
                model.ListaReporte2 = new List<GenericoDTO>();
                model.EstadoPublicacion = "";
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Publicar todos los reportes relacionados a una Revisión de un mes determinado en la Extranet
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PublicarReportes(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                CppaAppServicio.ActualizarEstadoPublicacionDeReportes(cparcodi, ConstantesCPPA.estadoPublicacionSi, User.Identity.Name.Trim());
                model.EstadoPublicacion = ConstantesCPPA.estadoPublicacionSi;
                model.sMensaje = "Se publicaron todos los reportes relacionados a la Revisión de Ajuste Presupuestal seleccionada satisfactoriamente en la Extranet.";
                model.sResultado = "1";
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Despublicar todos los reportes relacionados a una Revisión de un mes determinado en la Extranet
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DespublicarReportes(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                CppaAppServicio.ActualizarEstadoPublicacionDeReportes(cparcodi, ConstantesCPPA.estadoPublicacionNo, User.Identity.Name.Trim());
                model.EstadoPublicacion = ConstantesCPPA.estadoPublicacionNo;
                model.sMensaje = "Se despublicarón todos los reportes relacionados a la Revisión del Ajuste Presupuestal seleccionada satisfactoriamente de la Extranet.";
                model.sResultado = "1";
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportar un Reporte de Generacion de una Revisión de un mes determinado
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpacemes"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporteGeneracion(int cparcodi, int cpacemes)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReporteGeneracion(cparcodi, cpacemes, ConstantesCPPA.invocadoPorIntranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportar un Reporte de Demanda de una Revisión de un mes determinado
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpatdmes"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporteDemanda(int cparcodi, int cpatdmes)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReporteDemanda(cparcodi, cpatdmes, ConstantesCPPA.invocadoPorIntranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportar un Reporte de Transmisión de una Revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporteTransmision(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReporteTransmision(cparcodi, ConstantesCPPA.invocadoPorIntranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Exportar un Reporte de Porcentaje (Ajuste de Aportes)
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReportePorcentaje(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReportePorcentaje(cparcodi, ConstantesCPPA.invocadoPorIntranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (MyCustomException ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar archivo
        /// </summary>
        /// <param name="tipo">Tipo de archivo</param>
        /// <param name="nombreArchivo">Nombre del archivo a descargar</param>
        /// <returns>Retorna el archivo descargado</returns>
        public virtual ActionResult AbrirArchivo(int tipo, string nombreArchivo)
        {
            try
            {
                StringBuilder rutaNombreArchivo = new StringBuilder();
                rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString());
                rutaNombreArchivo.Append(nombreArchivo);

                StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
                rutaNombreArchivoDescarga.Append(nombreArchivo);

                byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
                System.IO.File.Delete(rutaNombreArchivo.ToString());
                return File(bFile, ConstantesCPPA.AppExcel, rutaNombreArchivoDescarga.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                string mensajeError = ConstantesCPPA.NoSePudoDescargarElArchivo;
                byte[] errorFile = System.Text.Encoding.UTF8.GetBytes(mensajeError); 
                return File(errorFile, ConstantesCPPA.TextPlain, ConstantesCPPA.NombreArchivoError);
            }
        }

    }
}