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
    public class ComparativoController : BaseController
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

        public ComparativoController()
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
                model.ListaAnio = CppaAppServicio.ObtenerAnios(out List<CpaRevisionDTO> ListRevision);
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
        /// Exportar un Reporte de Comparativo de 2 Revisiones
        /// </summary>
        /// <param name="cparcodiBase"></param>
        /// <param name="cparcodiComparar"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarReporteComparativo(int cparcodiBase, int cparcodiComparar)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelReporteComparativo(cparcodiBase, cparcodiComparar, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
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