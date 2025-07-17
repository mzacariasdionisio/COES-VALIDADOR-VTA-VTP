using System;
using System.Collections.Generic;
using log4net;
using System.Web.Mvc;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Reflection;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using System.Text;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using System.Configuration;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class InsumoController : BaseController
    {
        #region Declaración de variables
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly INDAppServicio indAppServicio = new INDAppServicio();
        private readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

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

        public InsumoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Insumos
        /// <summary>
        /// PrimasRER.2023
        /// Muestra vista de inicio del SubMódulo Solicitudes EDI
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexSolicitudEdi(int? anio, int? ipericodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdPeriodo = ipericodi.Value;
                model.ListaPeriodo = indAppServicio.GetByCriteriaIndPeriodos(anio.Value);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Muestra una lista de solicitudes edi de la extranet con respecto a un periodo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarSolicitudesEdi(int ipericodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.IdPeriodo = ipericodi;
                model.Resultado = primasRERAppServicio.GenerarHtmlListadoSolicitudesEdi(Url.Content("~/"), ipericodi);
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

        /// <summary>
        /// PrimasRER.2023
        /// Exporta las energías de las unidades generadoras de una solicitud edi de la extranet
        /// </summary>
        /// <returns></returns>
        public JsonResult ExportaraExcelEnergiaUnidad(int rersedcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string nombreArchivo = "ArchivoRegistrosMWh_id_" + rersedcodi;
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEnergiaUnidad(rersedcodi);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo.ToString(), false);
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

        /// <summary>
        /// PrimasRER.2023
        /// Exporta un archivo de sustento (archivo digital) de una solicitud edi de la extranet
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarSustentoSolicitudEdi(string nombreArchivo)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                StringBuilder sbRutayNombreArchivo = new StringBuilder();
                sbRutayNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivoSustento].ToString());
                sbRutayNombreArchivo.Append(nombreArchivo);
                return File(sbRutayNombreArchivo.ToString(), Constantes.AppExcel, nombreArchivo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                throw;
            }
        }

        /// <summary>
        /// PrimasRER.2023
        /// Descarga un archivo Excel con los Eventos y Causas del SubMódulo Eventos y Causas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IndexEventosCausas(int ipericodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string nombreArchivo = "Eventos_y_Mantenimientos";
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEventosCausas(ipericodi);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo.ToString(), true);
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

        /// <summary>
        /// PrimasRER.2023
        /// Descargar archivo
        /// </summary>
        /// <param name="tipo">Tipo de archivo</param>
        /// <param name="nombreArchivo">Nombre del archivo a descargar</param>
        /// <returns>Retorna el archivo descargado</returns>
        public virtual ActionResult AbrirArchivo(int tipo, string nombreArchivo)
        {
            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString());
            rutaNombreArchivo.Append(nombreArchivo);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(nombreArchivo);

            return File(rutaNombreArchivo.ToString(), ConstantesPrimasRER.AppExcel, rutaNombreArchivoDescarga.ToString());
        }
        #endregion
    }
}