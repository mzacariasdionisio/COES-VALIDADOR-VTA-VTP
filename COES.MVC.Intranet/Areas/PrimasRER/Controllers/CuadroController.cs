using System;
using System.Collections.Generic;
using log4net;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System.Text;
using System.Configuration;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class CuadroController : BaseController
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

        public CuadroController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Evaluacion
        /// <summary>
        /// PrimasRER.2023
        /// Muestra vista de inicio del SubMódulo Evaluación
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexEvaluacion(int? ipericodi, int? rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdPeriodo = ipericodi.Value;
                model.IndPeriodo = indAppServicio.GetByIdIndPeriodo(ipericodi.Value);
                model.IdRevision = rerrevcodi.Value;
                model.RerRevision = primasRERAppServicio.ObtenerRevision(rerrevcodi.Value);
                model.CantidadRevisionesTipoRevision = primasRERAppServicio.CantidadRevisionesTipoRevision(ipericodi.Value);
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
        /// Muestra una lista de evaluaciones con respecto a un periodo y a una revisión
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEvaluaciones(int rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.CantidadEvaluacionValidado = primasRERAppServicio.CantidadEvaluacionValidado(rerrevcodi);
                model.Resultado = primasRERAppServicio.GenerarHtmlListadoEvaluaciones(Url.Content("~/"), model.TienePermisoEditar, rerrevcodi);
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
        /// Crear una nueva evaluacion para una revisión
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarEvaluacion(int rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                primasRERAppServicio.GenerarNuevaEvaluacion(rerrevcodi, User.Identity.Name.Trim());
                model.Resultado = "1";
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
        /// Muestra vista de inicio para ver una Evaluación específica
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewEvaluacion(int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoGuardar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdEvaluacion = rerevacodi;
                model.RerEvaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
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
        /// Obtiene en un handson table las solicitudes edi correspondientes a una evaluación específica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerHandsonTableListadoEvaluacionSolicitudEdi(int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoGuardar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.HandsonTable = primasRERAppServicio.GenerarHandsonTableEvaluacionSolicitudEdi(rerevacodi, Url.Content("~/"));
                model.Resultado = "1";
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
        /// Actualiza las solicitudes edi correspondientes a una evaluación específica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEvaluacionSolicitudEdi(int rerevacodi, string[][] dataht)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.Resultado = primasRERAppServicio.ActualizarEvaluacionSolicitudEdi(rerevacodi, dataht, User.Identity.Name);
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
        /// Generar archivo Excel de una evaluación específica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportaraExcelEvaluacion(int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                RerEvaluacionDTO rerEvaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
                string nombreArchivo = ("Evaluacion_" + rerEvaluacion.Iperinombre + "_" + rerEvaluacion.Rerrevnombre + "_Version_" + rerEvaluacion.Rerevanumversion).Replace(" ","_");
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEvaluacion(rerevacodi, ConstantesPrimasRER.tipoReporte);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, true);
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
        /// Generar archivo Excel con las energías de las unidades de generación de una central para una solicitud EDI de una evaluación específica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportaraExcelEvaluacionEnergiaUnidad(int reresecodi, int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                RerEvaluacionDTO evaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
                RerEvaluacionSolicitudEdiDTO ese = primasRERAppServicio.ObtenerEvaluacionSolicitudEdi(reresecodi);
                string nombreArchivo = ("Evaluacion_" + evaluacion.Iperinombre + "_" + evaluacion.Rerrevnombre + "_Version_" + evaluacion.Rerevanumversion + "_id_" + ese.Rersedcodi).Replace(" ", "_");
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEvaluacionEnergiaUnidad(reresecodi, rerevacodi);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, false);
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
        /// Exportar archivo de sustento (archivo digital) de una solicitud EDI de una evaluación específica
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarSustentoEvaluacionSolicitudEdi(string nombreArchivo)
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
        #endregion

        #region Comparativo
        /// <summary>
        /// PrimasRER.2023
        /// Muestra vista de inicio del SubMódulo Comparativo
        /// Principalmente, carga una lista con los siguientes datos de las solicitudes EDI: 
        /// id = Id de la central.
        /// value = id de la solicitud EDI y nombre de la central de la solicitud EDI.
        /// Con respecto a la última versión de una revisión
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexComparativo(int? ipericodi, int? rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdPeriodo = ipericodi.Value;
                model.IndPeriodo = indAppServicio.GetByIdIndPeriodo(ipericodi.Value);
                model.IdRevision = rerrevcodi.Value;
                model.RerRevision = primasRERAppServicio.ObtenerRevision(rerrevcodi.Value);
                model.ListaGenerica = primasRERAppServicio.CargarSolicitudesEdi(rerrevcodi.Value, out RerEvaluacionDTO rerEvaluacion);
                bool existeEvaluacion = (rerEvaluacion != null);
                if (existeEvaluacion)
                {
                    model.IdEvaluacion = rerEvaluacion.Rerevacodi;
                    model.RerEvaluacion = rerEvaluacion;
                }
                model.ListaEvaluacionEnergiaUnidad = new List<RerEvaluacionEnergiaUnidadDTO>();
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
        /// Carga las unidades de generación de una central correspondientes a una solicitud de una evaluación
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerevacodi"></param>
        /// <returns></returns>
        public JsonResult CargarUnidadesGeneracion(int reresecodi, int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListaEvaluacionEnergiaUnidad = primasRERAppServicio.ObtenerListadoEvaluacionEnergiaUnidad(reresecodi, rerevacodi);
                model.EnergiaEstimadaCentral = primasRERAppServicio.ObtenerTotalEnergiaEstimdaCentral(reresecodi);
                model.EnergiaEstimada15Min = 0;
                model.EnergiaEstimadaUnidad = 0;
                model.EnergiaSolicitadaUnidad = 0;
                model.Resultado = "1";
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
        /// Obtener la energía de una unidad de generación de una central en formato HTML 
        /// correspondientes a una evaluación energía unidad específica.
        /// Adicionalmente, la energía total de una central.
        /// </summary>
        /// <param name="rerevacodi"></param
        /// <param name="reresecodi"></param
        /// <param name="rereeucodi"></param>
        /// <returns></returns>
        public JsonResult ConsultarEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                primasRERAppServicio.GenerarHtmlEvaluacionEnergiaUnidad(rerevacodi, reresecodi, rereeucodi, 
                    out int rerccbcodi, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad, out string tablaHtml, out GraficoWeb graficoWeb);

                model.IdComparativoCab = rerccbcodi;
                model.EnergiaEstimadaCentral = primasRERAppServicio.ObtenerTotalEnergiaEstimdaCentral(reresecodi);
                model.EnergiaEstimada15Min = 0;
                model.EnergiaEstimadaUnidad = energia_estimada_unidad;
                model.EnergiaSolicitadaUnidad = energia_solicitada_unidad;
                model.Resultado = tablaHtml;
                model.Grafico = graficoWeb;
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
        /// Generar archivo Excel con los datos de la tabla de energía de una unidad de generación de una central correspondientes a una solicitud de una evaluación
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <returns></returns>
        public JsonResult ExportaraExcelTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                RerEvaluacionDTO evaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
                RerEvaluacionSolicitudEdiDTO ese = primasRERAppServicio.ObtenerEvaluacionSolicitudEdi(reresecodi);
                RerEvaluacionEnergiaUnidadDTO eeu = primasRERAppServicio.ObtenerEvaluacionEnergiaUnidad(rereeucodi);
                string nombreArchivo = ("Tabla_" + evaluacion.Iperinombre + "_" + evaluacion.Rerrevnombre + "_Version_" + evaluacion.Rerevanumversion + "_id_" + ese.Rersedcodi + "_" + eeu.Rereucodi).Replace(" ", "_");
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelTablaEvaluacionEnergiaUnidad(rerevacodi, reresecodi, rereeucodi); 
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, false);
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
        /// Importar la energía estimada de una unidad de generación de una central correspondientes a una solicitud de una evaluación.
        /// Es decir lee un archivo Excel desde la PC del cliente, para colocarlo en un directorio.
        /// </summary>
        /// <returns></returns>
        public JsonResult ImportarArchivoExcelParaTablaEvaluacionEnergiaUnidad()
        {
            base.ValidarSesionUsuario();
            string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string nombreArchivo = file.FileName;
                    if (System.IO.File.Exists(rutaArchivo + nombreArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo + nombreArchivo);
                    }
                    file.SaveAs(rutaArchivo + nombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// PrimasRER.2023
        /// Procesar los datos de importación de una energía estimada de una unidad de generación de una central correspondientes a una solicitud de una evaluación.
        /// Lee el archivo colocado en el directorio.
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public JsonResult ProcesarArchivoExcelParaTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi, int rerccbcodi, string nombreArchivo)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                StringBuilder rutaNombreArchivo = new StringBuilder();
                rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString());
                rutaNombreArchivo.Append(nombreArchivo);
                primasRERAppServicio.ProcesarArchivoExcelParaTablaEvaluacionEnergiaUnidad(rerevacodi, reresecodi, rereeucodi, rutaNombreArchivo.ToString(), 
                    out int numErrores, out string mensajeError,
                    out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad, out string tablaHtml, out GraficoWeb graficoWeb);

                model.RegError = numErrores;
                model.MensajeError = mensajeError;
                model.EnergiaEstimada15Min = energia_estimada_15_min;
                model.EnergiaEstimadaUnidad = energia_estimada_unidad;
                model.EnergiaSolicitadaUnidad = energia_solicitada_unidad;
                model.EnergiaEstimadaCentral = primasRERAppServicio.CalcularNuevoTotalEnergiaEstimdaCentral(reresecodi, rerccbcodi, energia_estimada_unidad);
                model.Resultado = tablaHtml;
                model.Grafico = graficoWeb;

                if (System.IO.File.Exists(rutaNombreArchivo.ToString()))
                {
                    System.IO.File.Delete(rutaNombreArchivo.ToString());
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
        /// Procesar los datos de valor típico de una energía estimada de una unidad de generación de una central correspondientes a una solicitud de una evaluación.
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="hora_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="hora_fin"></param>
        /// <returns></returns>
        public JsonResult ProcesarValorTipicoParaTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi, int rerccbcodi, string fecha_inicio, string hora_inicio, string fecha_fin, string hora_fin)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                primasRERAppServicio.ProcesarValorTipicoParaTablaEvaluacionEnergiaUnidad(
                    rerevacodi, reresecodi, rereeucodi, fecha_inicio, hora_inicio, fecha_fin, hora_fin,
                    out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad, out string tablaHtml, out GraficoWeb graficoWeb);

                model.EnergiaEstimada15Min = energia_estimada_15_min;
                model.EnergiaEstimadaUnidad = energia_estimada_unidad;
                model.EnergiaSolicitadaUnidad = energia_solicitada_unidad;
                model.EnergiaEstimadaCentral = primasRERAppServicio.CalcularNuevoTotalEnergiaEstimdaCentral(reresecodi, rerccbcodi, energia_estimada_unidad);
                model.Resultado = tablaHtml;
                model.Grafico = graficoWeb;
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
        /// Guarda los datos de la vista comparativa de una unidad de generación de una central correspondientes a una solicitud de una evaluación.
        /// y sus datos relacionados
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="reresetotenergiaestimada"></param>
        /// <param name="rerccbtoteneestimada"></param>
        /// <param name="rerccbtotenesolicitada"></param>
        /// <param name="datosTabla"></param>
        /// <param name="rerccboridatos"></param>
        /// <returns></returns>
        public JsonResult GuardarComparativo(int rerevacodi, int reresecodi, int rereeucodi, int rerccbcodi, decimal reresetotenergiaestimada, decimal rerccbtoteneestimada, decimal rerccbtotenesolicitada, string[][] datosTabla, string rerccboridatos)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.IdComparativoCab = primasRERAppServicio.GuardarComparativo(rerevacodi, reresecodi, rereeucodi, rerccbcodi, reresetotenergiaestimada, rerccbtoteneestimada, rerccbtotenesolicitada, datosTabla, rerccboridatos, User.Identity.Name);
                model.Resultado = "1"; 
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

        #endregion

        #region Resultados
        /// <summary>
        /// PrimasRER.2023
        /// Muestra vista de inicio del SubMódulo Resultados
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexResultados(int? ipericodi, int? rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.IdPeriodo = ipericodi.Value;
                model.IdRevision = rerrevcodi.Value;
                RerEvaluacionDTO lastEvaluacion = primasRERAppServicio.ObtenerUltimaEvaluacion(rerrevcodi.Value);
                bool existeLastEvaluacion = (lastEvaluacion != null);
                if (existeLastEvaluacion)
                {
                    model.IdEvaluacion = lastEvaluacion.Rerevacodi;
                    model.RerEvaluacion = lastEvaluacion;
                }
                else
                {
                    model.IdEvaluacion = 0;
                    model.RerEvaluacion = null;

                    try
                    {
                        IndPeriodoDTO periodo = indAppServicio.GetByIdIndPeriodo(ipericodi.Value);
                        RerRevisionDTO revision = primasRERAppServicio.ObtenerRevision(rerrevcodi.Value);
                        model.Mensaje = "Resultados - " + periodo.Iperinombre + " - " + revision.Rerrevnombre;
                    }
                    catch
                    {
                        model.Mensaje = "Resultados";
                    }
                }
                model.Resultado = "1";
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
        /// Obtener handsontable del listado de las solicitudes EDi de la Intranet para 
        /// la última versión de una revisión de una evaluación solicitud edi específica.
        /// Esto es invocado desde "Cuadro - Resultados"
        /// </summary>
        /// <returns></returns>
        public JsonResult ObtenerHandsonTableListadoEvaluacionSolicitudEdiParaResultados(int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.HandsonTable = primasRERAppServicio.GenerarHandsonTableEvaluacionSolicitudEdiParaResultados(rerevacodi, Url.Content("~/"), out bool esEditable);
                model.EsEditable = esEditable;
                model.Resultado = "1";
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
        /// Actualiza las solicitudes edi correspondientes a una evaluación específica
        /// Se ejecuta desde "Cuadros - Resultados"
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarEvaluacionSolicitudEdiParaResultados(int rerevacodi, string[][] dataht)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.Resultado = primasRERAppServicio.ActualizarEvaluacionSolicitudEdiParaResultados(rerevacodi, dataht, User.Identity.Name);
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
        /// Generar archivo Excel con la energía estimada de una central RER de una evolución solicitud edi. Es decir, la energía estimada en el cuadro comparativo
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerevacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportaraExcelEnergiaEstimadaEvaluacionSolicitudEdi(int reresecodi, int rerevacodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                RerEvaluacionDTO evaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
                RerEvaluacionSolicitudEdiDTO ese = primasRERAppServicio.ObtenerEvaluacionSolicitudEdi(reresecodi);
                string nombreArchivo = ("Resultados_EnergiaEstimada_" + evaluacion.Iperinombre + "_" + evaluacion.Rerrevnombre + "_Version_" + evaluacion.Rerevanumversion + "_id_" + ese.Rersedcodi).Replace(" ", "_");
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEnergiaEstimadaEvaluacionSolicitudEdi(reresecodi, rerevacodi);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, false);
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
        /// Generar archivo Excel con la energía estimada de una central RER de una evolución solicitud edi. Es decir, la energía estimada en el cuadro comparativo
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportaraExcelEvaluacionParaResultados(int rerevacodi, int tipoReporte)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                RerEvaluacionDTO rerEvaluacion = primasRERAppServicio.ObtenerEvaluacion(rerevacodi);
                string descTipoReporte = (tipoReporte == ConstantesPrimasRER.tipoReporteAprobados) ? ConstantesPrimasRER.descReporteAprobados : 
                    (tipoReporte == ConstantesPrimasRER.tipoReporteNoAprobados ? ConstantesPrimasRER.descReporteNoAprobados : 
                    (tipoReporte == ConstantesPrimasRER.tipoReporteFuerzaMayor ? ConstantesPrimasRER.descReporteFuerzaMayor : ""));
                StringBuilder sbNombreArchivo = new StringBuilder();
                sbNombreArchivo.AppendFormat("Resultados_{0}_{1}_Version_{2}_EDIS_{3}", rerEvaluacion.Iperinombre, rerEvaluacion.Rerrevnombre, rerEvaluacion.Rerevanumversion, descTipoReporte);
                sbNombreArchivo.Replace(" ", "_");
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelEvaluacion(rerevacodi, tipoReporte);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, sbNombreArchivo.ToString(), true);
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
        #endregion
    }
}