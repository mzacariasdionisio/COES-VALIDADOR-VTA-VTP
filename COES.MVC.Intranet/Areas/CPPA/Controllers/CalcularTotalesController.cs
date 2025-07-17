using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CPPA;
using COES.Servicios.Aplicacion.CPPA.Helper;
using log4net;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class CalcularTotalesController : BaseController
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

        public CalcularTotalesController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        /// <summary>
        /// Pagina inicial de la modulo.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CPPAModel model = new CPPAModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.ListaAnio = CppaAppServicio.ObtenerAnios(out List<CpaRevisionDTO> ListRevision);
                model.ListaReporte = new List<GenericoDTO>();
                ViewBag.ListRevision = Newtonsoft.Json.JsonConvert.SerializeObject(ListRevision);
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
        /// Obtiene el log del proceso del Cálculo de Generación para una Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerLogProceso(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                model.sDetalle = CppaAppServicio.GetLogProceso(cparcodi, out List<GenericoDTO> listaReporte);
                model.ListaReporte = listaReporte;
                model.sResultado = "1";
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
        /// Procesa el Cálculo de Totales de Generación para una Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarCalculo(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                CppaAppServicio.ProcesarCalculo(cparcodi, User.Identity.Name.Trim(), out string logProceso, out List<GenericoDTO> listaReporte);
                model.sMensaje = "Se PROCESÓ el cálculo de Totales de Generación para la Revisión del Ajuste del Año Presupuestal seleccionada satisfactoriamente.";
                model.sResultado = "1";
                model.sDetalle = logProceso;
                model.ListaReporte = listaReporte;
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
        /// Elimina el Cálculo de Totales de Generación para una Revisión.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EliminarCalculo(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                CppaAppServicio.EliminarCalculo(cparcodi);
                model.sMensaje = "Se ELIMINÓ el cálculo de Totales de Generación para la Revisión del Ajuste del Año Presupuestal seleccionada satisfactoriamente.";
                model.sResultado = "1";
                model.sDetalle = "";
                model.ListaReporte = new List<GenericoDTO>();

                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Exportar un Reporte de Generación de una Revisión de un mes determinado
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpacemes"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporte(int cparcodi, int cpacemes)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReporteGeneracion(cparcodi, cpacemes, ConstantesCPPA.invocadoPorIntranet, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
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