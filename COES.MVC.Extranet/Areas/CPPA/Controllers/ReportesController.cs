using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.CPPA.Models;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.PrimasRER;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CPPA.Controllers
{
    public class ReportesController : BaseController
    {
        #region Declaración de variables
        private readonly CPPAAppServicio CppaAppServicio = new CPPAAppServicio();

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ReportesController()
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
        /// Obtiene reportes de una Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerReportes(int cparcodi)
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
                CppaAppServicio.GenerarArchivoExcelReporteGeneracion(cparcodi, cpacemes, ConstantesCPPA.invocadoPorExtranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
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
                CppaAppServicio.GenerarArchivoExcelReporteDemanda(cparcodi, cpatdmes, ConstantesCPPA.invocadoPorExtranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
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
                CppaAppServicio.GenerarArchivoExcelReporteTransmision(cparcodi, ConstantesCPPA.invocadoPorExtranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
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
                CppaAppServicio.GenerarArchivoExcelReportePorcentaje(cparcodi, ConstantesCPPA.invocadoPorExtranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
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
            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString());
            rutaNombreArchivo.Append(nombreArchivo);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(nombreArchivo);

            byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
            System.IO.File.Delete(rutaNombreArchivo.ToString());
            return File(bFile, ConstantesCPPA.AppExcel, rutaNombreArchivoDescarga.ToString());
        }

    }
}