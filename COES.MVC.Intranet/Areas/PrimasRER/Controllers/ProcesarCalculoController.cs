using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using System.Text;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class ProcesarCalculoController : BaseController
    {
        // GET: /PrimasRER/CentralRER/
        private readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ProcesarCalculoController()
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

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaAniosTarifario = primasRERAppServicio.ListarAniosTarifario();
                model.ListaVersiones = primasRERAppServicio.ListarVersiones();
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
        /// Procesar Calculo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ProcesarCalculo(int anio, string version)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                primasRERAppServicio.ProcesarCalculo(anio, version, User.Identity.Name.Trim());
                model.Mensaje = "Se PROCESÓ el cálculo de la Prima RER para la versión del Año Tarifario seleccionada satisfactoriamente.";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Borrar Calculo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <returns>JsonResult</returns>
        public JsonResult BorrarCalculo(int anio, string version)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                primasRERAppServicio.EliminarCalculo(anio, version, User.Identity.Name.Trim());
                model.Mensaje = "Se ELIMINÓ el cálculo de la Prima RER para la versión del Año Tarifario seleccionada satisfactoriamente";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Exportar un Reporte de Prima RER
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="tipoReporte"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporte(int anio, string version, int tipoReporte)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                primasRERAppServicio.GenerarArchivoExcelReporte(anio, version, tipoReporte, ConstantesPrimasRER.invocadoPorIntranet, out string nombreArchivo, out List<RerExcelHoja> listExcelHoja);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
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

            byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
            System.IO.File.Delete(rutaNombreArchivo.ToString());
            return File(bFile, Constantes.AppExcel, rutaNombreArchivoDescarga.ToString());
        }

    }
}