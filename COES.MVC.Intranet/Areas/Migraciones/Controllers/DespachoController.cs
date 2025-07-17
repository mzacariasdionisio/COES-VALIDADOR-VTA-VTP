using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using static COES.Servicios.Aplicacion.Migraciones.Helper.UtilCdispatch;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class DespachoController : BaseController
    {
        readonly MigracionesAppServicio servicio = new MigracionesAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            MigracionesModel model = new MigracionesModel();

            string lectcodiExcel = ConstantesAppServicio.LectcodiReprogDiario + "," + ConstantesAppServicio.LectcodiEjecutadoHisto;

            string lectcodiSicoes = ConstantesAppServicio.LectcodiProgSemanal + "," + ConstantesAppServicio.LectcodiProgDiario
                                    + "," + ConstantesAppServicio.LectcodiReprogDiario + "," + ConstantesAppServicio.LectcodiEjecutadoHisto;

            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.TipoProgramacion3 = servicio.GetByCriteriaMeLectura(lectcodiExcel);
            model.TipoProgramacion = servicio.GetByCriteriaMeLectura(lectcodiSicoes);

            model.ListaTipoInfo = UtilCdispatch.ListarTipoinfo();

            return View(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesAppServicio.ModuloManualUsuario;
            string nombreArchivo = ConstantesAppServicio.ArchivoManualUsuarioIntranetv1;
            string pathDestino = modulo + ConstantesAppServicio.FolderRaizv1ModuloManual;
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

        #region Cargar Htrabajo

        /// <summary>
        /// Subir archivos Carga Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = DateTime.Now.Ticks + "_" + file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }

                    //guardar archivo en temporales
                    file.SaveAs(path + sNombreArchivo);

                    return Json(new
                    {
                        success = true,
                        nuevonombre = sNombreArchivo,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fec1"></param>
        /// <param name="fec2"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public JsonResult ValidarFileUpxls(int lectcodi, string nombreFile)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                this.ValidarSesionJsonResult();

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                List<string> listaOK = new List<string>();
                List<string> listaError = new List<string>();
                if (string.IsNullOrEmpty(nombreFile))
                {
                    listaError.Add("Debe ingresar un archivo. ");
                }
                else
                {
                    int posBarraBaja = nombreFile.IndexOf('_');
                    string nombreFileSinAleatorio = nombreFile.Substring(posBarraBaja+1, nombreFile.Length - posBarraBaja - 1);

                    string sDiaFecha = "";
                    if (nombreFileSinAleatorio.Length > 32) sDiaFecha = nombreFileSinAleatorio.Substring(20, 8);

                    try
                    {
                        DateTime fechaInicio = DateTime.ParseExact(sDiaFecha, ConstantesAppServicio.FormatoFechaYMD2, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        listaError.Add("Debe seleccionar un archivo Hoja de Trabajo con el formato Htrabajo_generación_YYYYMMDD.xlsm. ");

                    }
                    if (!listaError.Any())
                    {
                        string nombreFormato = nombreFileSinAleatorio.Replace(sDiaFecha, "YYYYMMDD");

                        if (nombreFormato.ToLower() != "Htrabajo_generación_YYYYMMDD.xlsm".ToLower())
                        {
                            listaError.Add("Debe seleccionar un archivo Hoja de Trabajo con el formato Htrabajo_generación_YYYYMMDD.xlsm. ");
                        }
                    }
                }

                if (!listaError.Any())
                {
                    HtFiltro flagCarga = new HtFiltro();
                    flagCarga.FlagCDispatchCargaActiva = true;
                    flagCarga.FlagCDispatchCargaReactiva = true;
                    flagCarga.FlagCDispatchCargaHidrologia = true;
                    flagCarga.FlagCDispatchCargaReprograma = true;

                    //revisar errores en el contenido del archivo
                    servicio.LeerFileUpxls(nombreFile, path, lectcodi, flagCarga, false, out List<MeMedicion48DTO> listaMe48, out listaOK, out listaError, out _);
                    model.nRegistros = listaMe48.Count;
                }

                model.MensajeOK = string.Join(". ", listaOK);
                model.MensajeError = string.Join(". ", listaError);
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Cargamos a tablas temporales o guardamos informacion
        /// </summary>
        /// <param name="tip"></param>
        /// <returns></returns>
        public JsonResult SaveLoadXls(int lectcodi, string nombreFile)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                this.ValidarSesionJsonResult();

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                HtFiltro flagCarga = new HtFiltro();
                flagCarga.FlagCDispatchCargaActiva = true;
                flagCarga.FlagCDispatchCargaReactiva = true;
                flagCarga.FlagCDispatchCargaHidrologia = true;
                flagCarga.FlagCDispatchCargaReprograma = true;

                servicio.LeerFileUpxls(nombreFile, path, lectcodi, flagCarga, false, out List<MeMedicion48DTO> listaMe48, out List<string> listaOK, out List<string> listaError, out _);

                if (listaMe48.Any())
                {
                    servicio.LoadDispatchFromHtrabajo(lectcodi, listaMe48, out CDespachoGlobal regCDespacho);

                    servicio.GrabarCDispatch(regCDespacho, base.UserEmail, base.UserName, false);

                    model.FechaIni = regCDespacho.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    model.FechaFin = regCDespacho.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    model.nRegistros = 1;

                    //Eliminar archivo xlsm
                    System.IO.File.Delete(path + nombreFile);
                }

            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Consulta CDispatch

        /// <summary>
        /// Reporte web
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="esConsultaBD"></param>
        /// <returns></returns>
        public JsonResult ConsultarCdispatch(string fecha1, string fecha2, int lectcodi, bool esConsultaBD, string nombreFile, bool flagRecalcularCosto)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                this.ValidarSesionJsonResult();

                //variables debug
                DateTime fechaHoraIni = DateTime.Now;
                int contadorIni = 1;

                CDespachoGlobal regCDespacho;
                if (esConsultaBD)
                {
                    DateTime fechaInicio = DateTime.ParseExact(fecha1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFin = DateTime.ParseExact(fecha2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    servicio.Load_Dispatch(fechaInicio, fechaFin, lectcodi, ConstantesAppServicio.ParametroDefecto, esConsultaBD, flagRecalcularCosto, true, null, out regCDespacho);

                    UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);
                }
                else
                {
                    HtFiltro flagCarga = new HtFiltro();
                    flagCarga.FlagCDispatchCargaActiva = true;
                    flagCarga.FlagCDispatchCargaReactiva = true;
                    flagCarga.FlagCDispatchCargaHidrologia = true;
                    flagCarga.FlagCDispatchCargaReprograma = true;

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    servicio.LeerFileUpxls(nombreFile, path, lectcodi, flagCarga, false, out List<MeMedicion48DTO> listaMe48, out List<string> listaOK, out List<string> listaError, out _);
                    servicio.LoadDispatchFromHtrabajo(lectcodi, listaMe48, out regCDespacho);
                }

                //reporte web 30min
                model.ListaTipoInfo = UtilCdispatch.ListarTipoinfo();
                model.ListaResultado = UtilCdispatch.CdispatchHtml(regCDespacho.FechaIni, regCDespacho.FechaFin, regCDespacho);
                model.ResultadoObservaciones = UtilCdispatch.GenerarCdispatchObservacionesHtml(regCDespacho);

                //costo operación
                string[] arrayCosto = UtilCdispatch.GetDescripcionCostoOperacion(Acciones.Consultar, regCDespacho.ListaAllMe1, regCDespacho.FechaIni, regCDespacho.TipoCambioIni);
                model.CostoTotalOperacion = arrayCosto[0] + arrayCosto[1];

                //Comparación de costo
                if (flagRecalcularCosto)
                {
                    UtilCdispatch.DiferenciaCostoOperacion(regCDespacho.FechaIni, regCDespacho.ListaAllMe1BD, regCDespacho.ListaAllMe1, out bool hayDiferencia, out decimal costoActual, out decimal costoNuevo);

                    string textoCostoActual = "";
                    string textoCostoNuevo = "";
                    if (hayDiferencia)
                    {
                        textoCostoActual = string.Format("Antes del recálculo: Costo Operación[{0}]= S/. {1}", regCDespacho.FechaIni.Day, costoActual);
                        textoCostoNuevo = string.Format("Después del recálculo: Costo Operación[{0}]= S/. {1}", regCDespacho.FechaIni.Day, costoNuevo);
                    }

                    model.CostoTotalOperacion = textoCostoActual;
                    model.CostoTotalOperacionNuevo = textoCostoNuevo;
                }

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                model.nRegistros = 1;
                model.Mensaje = "";
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return jsonResult;
        }

        /// <summary>
        /// Exportar CDispatch
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="esConsultaBD"></param>
        /// <param name="tieneMostrarDetallaAdicional"></param>
        /// <returns></returns>
        public JsonResult ExportarCdispatch(string fecha1, string fecha2, int lectcodi, int tipoinfocodi, bool esConsultaBD, string nombreFile, bool tieneMostrarDetallaAdicional)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                this.ValidarSesionJsonResult();

                CDespachoGlobal regCDespacho;
                if (esConsultaBD)
                {
                    //Se setea y se establece el formato de fecha y mostrarlo en reporte matriz excel
                    DateTime fechaInicio = DateTime.ParseExact(fecha1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFin = DateTime.ParseExact(fecha2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    servicio.Load_Dispatch(fechaInicio, fechaFin, lectcodi, ConstantesAppServicio.ParametroDefecto, esConsultaBD, false, true, null, out regCDespacho);
                }
                else
                {
                    HtFiltro flagCarga = new HtFiltro();
                    flagCarga.FlagCDispatchCargaActiva = true;
                    flagCarga.FlagCDispatchCargaReactiva = true;
                    flagCarga.FlagCDispatchCargaHidrologia = true;
                    flagCarga.FlagCDispatchCargaReprograma = true;

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    servicio.LeerFileUpxls(nombreFile, path, lectcodi, flagCarga, false, out List <MeMedicion48DTO> listaMe48, out List<string> listaOK, out List<string> listaError, out _);
                    servicio.LoadDispatchFromHtrabajo(lectcodi, listaMe48, out regCDespacho);
                }

                //reporte excel 
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = ConstantesMigraciones.RptCDispatch + "_" + regCDespacho.FechaIni.ToString("yyyyMMdd") + "_" + regCDespacho.FechaFin.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;

                UtilCdispatch.GenerarArchivoExcelCdispatch(tieneMostrarDetallaAdicional, regCDespacho, ruta + nameFile, tipoinfocodi);

                model.Resultado = nameFile;
                model.nRegistros = 1;
                model.Mensaje = "";
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Descarga el archivo excel exportado
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar(string fi)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            return base.DescargarArchivoTemporalYEliminarlo(path, fi);
        }

        #endregion

        #region Recalcular Costo Operación

        public JsonResult VerificarPermisoRecalculo(string fecha, int lectcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (lectcodi.ToString() == ConstantesAppServicio.LectcodiEjecutadoHisto)
                {
                    var idArea = base.IdArea;

                    if (idArea == ConstantesMigraciones.AreacodiDTI || idArea == ConstantesMigraciones.AreacodiSGI)
                        model.TienePermisoRecalculo = true;

                    if (idArea == ConstantesMigraciones.AreacodiSubdirCoord)
                    {
                        DateTime fechaInicio = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime fechaMax = fechaInicio.AddDays(1).AddHours(7).AddMinutes(30);

                        if (fechaInicio >= DateTime.Now && DateTime.Now <= fechaMax) model.TienePermisoRecalculo = true;
                    }
                }

                model.nRegistros = 1;
                model.Mensaje = "";
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        public JsonResult GuardarRecalculoCostoOperacion(string fecha, int lectcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //costo de la operación
                servicio.Load_Dispatch(fechaInicio, fechaInicio, lectcodi, ConstantesAppServicio.ParametroDefecto, true, true, true, null, out CDespachoGlobal regCDespacho);

                UtilCdispatch.DiferenciaCostoOperacion(fechaInicio, regCDespacho.ListaAllMe1BD, regCDespacho.ListaAllMe1, out bool hayDiferencia, out decimal costoActual, out decimal costoNuevo);

                if (hayDiferencia)
                {
                    servicio.GrabarRecalculoCostoOperacionEjecutado(fechaInicio, costoActual, costoNuevo, base.UserName, base.UserEmail);
                }

                model.nRegistros = 1;
                model.Mensaje = "";
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

    }
}
