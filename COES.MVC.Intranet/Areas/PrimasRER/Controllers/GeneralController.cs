using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class GeneralController : BaseController
    {
        readonly INDAppServicio indAppServicio = new INDAppServicio();
        readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

        #region Declaración de variables

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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public GeneralController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// PrimasRER.2023
        /// Página Inicial del Cálculo EDI
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            PrimasRERModel model = new PrimasRERModel
            {
                TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)
            };

            //realizar proceso 
            this.indAppServicio.CrearIndPeriodoAutomatico();

            DateTime fechaPeriodo = indAppServicio.GetPeriodoActual(); // primer dia del mes anterior
            //model.ListaCuadro = indServicio.GetByCriteriaIndCuadros();
            model.ListaAnio = indAppServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = indAppServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);

            var regPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month);
            model.IdPeriodo = regPeriodo.Ipericodi;
            model.AnioActual = regPeriodo.FechaIni.Year;
            
            model.ListaRevision = new List<RerRevisionDTO>();
            bool existeListaPeriodo = (model.ListaPeriodo != null && model.ListaPeriodo.Count > 0);
            if (existeListaPeriodo)
            {
                model.ListaRevision = primasRERAppServicio.ListarRevisiones(model.IdPeriodo);
                model.IdRevision = model.ListaRevision.FirstOrDefault() != null ? model.ListaRevision.FirstOrDefault().Rerrevcodi : 0;
            }

            return View(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPeriodos(int anio)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListaPeriodo = indAppServicio.GetByCriteriaIndPeriodos(anio);
                DateTime fechaPeriodo = indAppServicio.GetPeriodoActual(); // primer dia del mes anterior
                if (fechaPeriodo.Year == anio)
                {
                    model.IdPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month).Ipericodi;
                }
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
        /// Listar revisiones por periodo en formato JSON
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRevisiones(int ipericodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListaRevision = primasRERAppServicio.ListarRevisiones(ipericodi);
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
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesPrimasRER.ModuloManualUsuario;
            string nombreArchivo = ConstantesPrimasRER.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesPrimasRER.FolderRaizPrimaRERModuloManual;
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

    }
}