using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using System.IO;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Controllers;
using System.Text;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class InsumosRERController : BaseController
    {
        // GET: /PrimasRER/CentralRER/
        private readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public InsumosRERController()
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
        /// Pagina inicial de la modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PrimasRERModel model = new PrimasRERModel();
            model.RutaArchivosSddp = ConfigurationManager.AppSettings[ConstantesPrimasRER.RutaArchivosSddp];

            return View(model);
        }

        /// <summary>
        /// Lista los Periodos
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult ListarPeriodos()
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.ListaAniosTarifario = primasRERAppServicio.ListarAniosTarifario();
                model.Mensaje = "Todo correcto";
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
        /// Lista las Versiones de un Anio Tarifario
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult ListarVersiones()
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.ListaVersiones = primasRERAppServicio.ListarVersiones();
                model.Mensaje = "Todo correcto";
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
        /// Consultar el estado del Anio tarifario y versión seleccionado
        /// </summary>
        /// <param name="iAnioTarifario"></param>
        /// <param name="iVersion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EstadoAnioTarifarioVersion(int iAnioTarifario, int iVersion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.AnioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iAnioTarifario, iVersion);
                model.Detalle = model.AnioVersion.Reravestado;
                if (model.Detalle == "1")
                {
                    model.Mensaje = "El estado del año tarifario seleccionado es Abierto";
                }
                else {
                    model.Mensaje = string.Format("La versión '{1}' del Año Tarifario '{0}' no tiene su estado ‘abierto’. Realice la actualización de dicho estado en 'Parámetros de Prima RER', para continuar con la importación ", model.AnioVersion.Reravaniotarifdesc, model.AnioVersion.Reravversiondesc);
                }
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
        /// Consultar el estado del procesar calculo del Anio tarifario y versión seleccionado
        /// </summary>
        /// <param name="iAnioTarifario"></param>
        /// <param name="iVersion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EstadoDelProcesarCalculo(int iAnioTarifario, int iVersion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.Detalle = "1";
                RerAnioVersionDTO anioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iAnioTarifario, iVersion);

                List<RerCalculoMensualDTO> listCalculoMensual = primasRERAppServicio.GetRerCalculoMensualByReravcodi(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (existeListCalculoMensual)
                {
                    model.Detalle = "0";
                    model.Mensaje = string.Format("Por favor, ejecute borrar cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}' para realizar una nueva importación.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc);
                }
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
        /// Lista los insumos
        /// </summary>
        /// <param name="sAnioTarifario"></param>
        /// <param name="sVersion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ListarInsumos(string sAnioTarifario, string sVersion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.ListaInsumos = primasRERAppServicio.ListarInsumos(sAnioTarifario, sVersion);
                model.Mensaje = "Todo correcto";
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
        /// Obtener el log de rer_insumo segun su Anio Tarifario y Versión
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="opcion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ObtenerLog(string idPeriodo, string idVersion, string opcion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                int iRerAVAnioTarif = int.Parse(idPeriodo);
                int iRerAVVersion = int.Parse(idVersion);
                model.AnioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iRerAVAnioTarif, iRerAVVersion);
                RerInsumoDTO RerInsumoDTO = primasRERAppServicio.GetByReravcodiByRerinstipinsumoRerInsumo(model.AnioVersion.Reravcodi, opcion);
                model.Version = ConstantesPrimasRER.insumosDesc[int.Parse(opcion) - 1];

                if (RerInsumoDTO!=null) {
                    model.Mensaje = RerInsumoDTO.Rerinslog;
                }
                else {
                    model.Mensaje = "Aun no se realizó la importación manual ni la importación automática ";
                }
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
        /// Lista de los meses de un Anio Tarifario
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="opcion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ListarMesesAnioTarifario(string idPeriodo, string idVersion, string opcion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                int iRerAVAnioTarif = int.Parse(idPeriodo);
                int iRerAVVersion = int.Parse(idVersion);
                model.AnioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iRerAVAnioTarif, iRerAVVersion);
                model.ListarMesesAnioTarifario = primasRERAppServicio.ListarMesesAnioTarifario(model.AnioVersion.Reravcodi, opcion);
                model.Version = ConstantesPrimasRER.insumosDesc[int.Parse(opcion) - 1];
                model.Mensaje = "Todo correcto";
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
        /// Procesar Archivos
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="pathDirectorio"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ProcesarArchivosSddp(string idPeriodo, string idVersion, string pathDirectorio)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                int iRerAVAnioTarif = int.Parse(idPeriodo);
                int iRerAVVersion = int.Parse(idVersion);
                model.entidadRerAnioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iRerAVAnioTarif, iRerAVVersion);

                if (model.entidadRerAnioVersion == null) {
                    model.Mensaje = "No existe el Año Tarifario y la Versión seleccionada";
                    model.Resultado = "-1";
                    return Json(model);
                }
                if (!pathDirectorio.EndsWith("\\")) { 
                    pathDirectorio += "\\";
                }
                string resultado = primasRERAppServicio.ProcesarArchivosSddp(model.entidadRerAnioVersion.Reravcodi.ToString(), idVersion, pathDirectorio, User.Identity.Name);
                if (resultado == "1")
                {
                    model.Mensaje = "Se proceso correctamente los archivos";
                    model.Resultado = "1";
                }
                else
                {
                    model.Mensaje = resultado;
                    model.Resultado = "-1";
                }

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
        /// Descargar Excel de los archivos SDDP y CSV
        /// </summary>
        /// <param name="sAnioTarifario"></param>
        /// <param name="sVersion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DescargarArchivoSDDP(string sAnioTarifario, string sVersion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                primasRERAppServicio.GenerarArchivoExcelSDDP(sAnioTarifario, sVersion, rutaArchivo, out string nombreArchivo, out List<RerExcelHoja> listExcelHoja);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #region CU21
        /// <summary>
        /// Importacion automatica de los insumos
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="opcion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ImportarTotalInsumos(string idPeriodo, string idVersion, int opcion)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                model.Resultado = "1";
                int iRerAVAnioTarif = int.Parse(idPeriodo);
                int iRerAVVersion = int.Parse(idVersion);
                model.entidadRerAnioVersion = primasRERAppServicio.GetRerAnioVersionByAnioVersion(iRerAVAnioTarif, iRerAVVersion);
                
                //Traemos la lista de Meses del Año tarifario
                model.ListaParametroPrima = primasRERAppServicio.GetParametroPrimaRerByAnioVersion(model.entidadRerAnioVersion.Reravcodi);
                if (model.ListaParametroPrima != null)
                {
                    string sUser = User.Identity.Name;
                    int iRerPPrCodi = model.ListaParametroPrima[0].Rerpprcodi;
                    switch (opcion)
                    {
                        case 1:
                            //Inyección Neta 15 min Mensual
                            model.Mensaje = primasRERAppServicio.ImportarInyeccionNeta(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser, "8");
                            //Inyección Neta 15 min Última revisión
                            model.Mensaje = primasRERAppServicio.ImportarInyeccionNeta(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser, "1");
                            break;
                        case 2:
                            //Costo Marginal c/15 minutos
                            model.Mensaje = primasRERAppServicio.ImportarCostoMarginal(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser);
                            break;
                        case 3:
                            //Ingresos por Potencia
                            model.Mensaje = primasRERAppServicio.ImportarIngresosPorPotencia(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser);
                            break;
                        case 4:
                            //Ingresos por Cargo Prima RER
                            model.Mensaje = primasRERAppServicio.ImportarIngresoPorCargoPrimaRER(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser);
                            break;
                        case 5:
                            //Energía Dejada de Inyectar (EDI) c/15 minutos
                            model.Mensaje = primasRERAppServicio.ImportarEnergiaDejadaInyectar(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, iRerAVAnioTarif, sUser);
                            break;
                        case 6:
                            //Saldos VTEA c/15 minutos
                            model.Mensaje = primasRERAppServicio.ImportarSaldosVTEA(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, idVersion, iRerAVAnioTarif, sUser);
                            break;
                        case 7:
                            //Saldos VTP
                            model.Mensaje = primasRERAppServicio.ImportarSaldosVTP(model.ListaParametroPrima, model.entidadRerAnioVersion.Reravcodi, iRerPPrCodi, idVersion, iRerAVAnioTarif, sUser);
                            break;
                        default:
                            model.Mensaje = "Opción no disponible";
                            break;
                    }
                }
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
        /// Exportar archivo Excel de Total Insumos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="meses"></param>
        /// <param name="tipoInsumo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarTotalInsumos(int anio, string version, int[] meses, string tipoInsumo)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string descTipoInsumo = UtilPrimasRER.ObtenerNombreTipoResultado(tipoInsumo, true);
                string nombreArchivo = ("Insumo_" + descTipoInsumo + "_" + anio + "_" + version);
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoExcelExportarTotalInsumos(anio, version, meses, tipoInsumo);
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

        /// <summary>
        /// Exportar plantilla del insumo seleccionado en la carga manual.
        /// </summary>
        /// <param name="iRerPprCodi"></param>
        /// <param name="sVersion"></param>
        /// <param name="sTipoInsumo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarPlantillaCargaManualInsumos(int iRerPprCodi, string sVersion, string sTipoInsumo)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                List<RerExcelHoja> listRerExcelHoja = primasRERAppServicio.GenerarArchivoPlantillaExcelCargaManual(iRerPprCodi, sVersion, sTipoInsumo, out string nombreArchivo);
                model.Resultado = primasRERAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, true);

                model.Detalle = sTipoInsumo;
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

        /// <summary>
        /// Eliminar archivos guardados previamente
        /// </summary>
        /// <param name="sAnioTarifario">Año Tarifario seleccionado</param>
        /// <param name="sMes">Mes que se realizo la importación</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public ActionResult EliminaraArchivosGuardadosPreviamente(string sAnioTarifario, string sMes, string sVersion, string sTipoInsumo)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

            try
            {
                if (sMes == "-1")
                {
                    sNombreArchivo = sAnioTarifario + "*";
                }
                else {
                    int iAnioTarifario = int.Parse(sAnioTarifario);
                    int iMes = int.Parse(sMes);
                    int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente
                    
                    sNombreArchivo = iAnio + sMes.PadLeft(2, '0') + "_v" + sVersion + "_i" + sTipoInsumo + "*";     //Cuando se necesite eliminar un archivo en especifico
                }
                // Obtener la lista de archivos que comienzan con "202205_v2_i4" en el directorio
                string[] archivosAEliminar = Directory.GetFiles(path, sNombreArchivo);

                // Eliminar los archivos encontrados
                foreach (var archivoExistente in archivosAEliminar)
                {
                    System.IO.File.Delete(archivoExistente);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Paso 3: Permite cargar un archivo Excel
        /// </summary>
        /// <param name="sMes">Mes que se realizo la importación</param>
        /// <param name="iAnioTarifario">Año Tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public ActionResult UploadExcel(string sMes, int iAnioTarifario, string sVersion, string sTipoInsumo)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

            try
            {
                int iMes = int.Parse(sMes);
                int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = iAnio.ToString() + sMes + "_v" + sVersion + "_i" + sTipoInsumo;   //Por ejemplo: 202205_v2_i4
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Paso 4: Lee datos desde el archivo excel
        /// </summary>
        /// <param name="sAnioTarifario">Año Tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sNumMes">Mes que se realizo la importación</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sAnioTarifario, string sVersion, string sNumMes, string sTipoInsumo)
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                model.MesActual = int.Parse(sNumMes);
                string sRutaArchivo = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();
                primasRERAppServicio.ValidarExcelImportado(sAnioTarifario, sVersion, sNumMes, sRutaArchivo, sTipoInsumo);
                model.Mensaje = "Los datos del archivo importado en el mes de '" + ConstantesPrimasRER.mesesDesc[int.Parse(sNumMes)-1] + "' son correctos.";
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
        /// Procesar y guarda los registros de los archivos Excels importados de los meses seleccionados
        /// </summary>
        /// <param name="iAnioTarifario">Año Tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivosInsumo(int iAnioTarifario, string sVersion, string sTipoInsumo, int[] iMeses)
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                string sUser = User.Identity.Name.Trim();
                primasRERAppServicio.procesarArchivosInsumo(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                model.Mensaje = "Se completó la importación manual del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo)-1] + "'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";
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
    }
}