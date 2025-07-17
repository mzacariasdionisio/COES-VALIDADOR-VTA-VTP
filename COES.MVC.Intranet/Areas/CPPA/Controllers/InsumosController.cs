using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CPPA;
using System.IO;
using COES.MVC.Intranet.Controllers;
using System.Text;
using System.Text.Json;
using DevExpress.XtraRichEdit.Import.Doc;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using COES.Servicios.Aplicacion.PrimasRER;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class InsumosController : BaseController
    {
        // GET: /CPPA/Insumo/
        private readonly CPPAAppServicio CppaAppServicio = new CPPAAppServicio();

        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public InsumosController()
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
        /// Pagina inicial de la modulo de insumos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CPPAModel model = new CPPAModel();
            model.ListRevision = CppaAppServicio.ListarRevisiones(-1, -1, ConstantesCPPA.todos, "'A','C','X'");
            base.ValidarSesionJsonResult();
            for (int i = 0; i < model.ListRevision.Count; i++)
            {
                string sAdicional = "";
                if (model.ListRevision[i].Cparestado == "C") {
                    //sAdicional = " [C]";
                }
                else if (model.ListRevision[i].Cparestado == "X") {
                    sAdicional = " [X]";
                }
                else {
                    sAdicional = "";
                }

                model.ListRevision[i].Cparrevision = model.ListRevision[i].Cparrevision + sAdicional;
            }
            ViewBag.RevisionData = Newtonsoft.Json.JsonConvert.SerializeObject(model.ListRevision);
            model.sRutaArchivosSddp = ConfigurationManager.AppSettings[ConstantesCPPA.RutaArchivosSddp];

            return View(model);
        }

        /// <summary>
        /// Muestra la lista de insumos
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListInsumos(string sAnioPresupuestal, string sAjuste, string sRevision)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListInsumo = CppaAppServicio.ListarInsumos(sAnioPresupuestal, sAjuste, sRevision);
                model.sEstadoRevision = CppaAppServicio.GetByIdCpaRevision(Int32.Parse(sRevision)).Cparestado;
                model.sExiteCentrales = CppaAppServicio.ValidacionCentrales(sRevision);
                model.sSeProcesoCalculo = CppaAppServicio.GetByCriteriaCpaCalculo(Int32.Parse(sRevision)) == null ? "N" : "S";
                model.sMensaje = "Todo correcto";
                model.sResultado = "1";

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
        /// Obtener el log de cpa_insumo segun su Anio Presupuestal, ajuste y revision
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sOpcion">Tipo de insumo seleccionado</param>
        /// <returns>JsonResult</returns>
        public JsonResult ListarLogs(string sAnioPresupuestal, string sAjuste, string sRevision, string sOpcion)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListInsumo = CppaAppServicio.ListarInsumoParaLog(sAnioPresupuestal, sAjuste, sRevision, sOpcion);
                model.NombInsumo = ConstantesCPPA.insumosDesc[int.Parse(sOpcion) - 1];

                if (model.ListInsumo == null)
                { 
                    model.sMensaje = "Aun no se realizó la importación manual ni la importación automática ";
                }
                model.sResultado = "1";

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
        /// Importacion automatica de los insumos
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sOpcion">Tipo de insumo seleccionado</param>
        /// <returns>JsonResult</returns>
        public JsonResult ImportarTotalInsumos(string sAnioPresupuestal, string sAjuste, string sRevision, string sOpcion)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                model.sResultado = "1";
                CpaRevisionDTO dtoCpaRevision = CppaAppServicio.GetByIdCpaRevision(Int32.Parse(sRevision));
                model.AjustePresupuestal = CppaAppServicio.GetByIdCpaAjustePresupuestal(dtoCpaRevision.Cpaapcodi);
                //Traemos la información del Año de ejercicio: model.AjustePresupuestal.Cpaapanioejercicio
                if (dtoCpaRevision != null && model.AjustePresupuestal != null)
                {
                    string sUser = User.Identity.Name;
                    switch (sOpcion)
                    {
                        case ConstantesCPPA.insumoMedidoresGeneracion:
                            //CU07: Medidores generacion 15 min.
                            model.sMensaje = CppaAppServicio.ImportarMedidoresGeneración(model.AjustePresupuestal, dtoCpaRevision, sUser);
                            break;
                        case ConstantesCPPA.insumoCostoMarginalLVTEA:
                            //Costo Marginal LVTEA 15 min.
                            model.sMensaje = CppaAppServicio.ImportarCostoMarginal(model.AjustePresupuestal, dtoCpaRevision, sUser);
                            break;
                        case ConstantesCPPA.insumoCostoMarginalPMPO:
                            //Costo Marginal PMPO 15 min.
                            model.sMensaje = CppaAppServicio.ImportarCostoMarginalPMPO(model.AjustePresupuestal, dtoCpaRevision, sUser);
                            break;
                        default:
                            model.sMensaje = "Opción no disponible";
                            break;
                    }
                }
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
        /// Lista de los meses de un Anio Tarifario
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sOpcion">Tipo de insumo seleccionado</param>
        /// <returns>JsonResult</returns>
        public JsonResult ListarMeses(string sAnioPresupuestal, string sAjuste, string sRevision, string sOpcion)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListarMesesAnioPresupuestal = CppaAppServicio.ListarMeses(sAnioPresupuestal, sAjuste, sRevision, sOpcion);
                model.NombInsumo = ConstantesCPPA.insumosDesc[int.Parse(sOpcion) - 1];
                model.NombMeses = ConstantesCPPA.mesesDesc;

                model.sMensaje = "Todo correcto";
                model.sResultado = "1";

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
        /// Eliminar archivos guardados previamente
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sMes">Mes que se realizo la importación</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public ActionResult EliminaraArchivosGuardadosPreviamente(string sAnioPresupuestal, string sMes, string sAjuste, string sRevision, string sTipoInsumo)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
            CpaRevisionDTO cpaRevisionDto = CppaAppServicio.GetByIdCpaRevision(Int32.Parse(sRevision));

            try
            {
                if (sMes == "-1")
                {
                    sNombreArchivo = ConstantesCPPA.insumosNombArchivoDesc[Int32.Parse(sTipoInsumo) - 1] + "*";
                }
                else
                {
                    sNombreArchivo = ConstantesCPPA.insumosNombArchivoDesc[Int32.Parse(sTipoInsumo) - 1] + "_" + (int.Parse(sAnioPresupuestal) - 1).ToString() + "_" + int.Parse(sMes).ToString("D2") + "-" + ConstantesCPPA.mesesDescCorta[int.Parse(sMes) - 1];
                }

                // Obtener la lista de archivos que comienzan con el nombre del insumo en el directorio
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
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public ActionResult UploadExcel(string sMes, string sAnioPresupuestal, string sAjuste, string sRevision, string sTipoInsumo)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = ConstantesCPPA.insumosNombArchivoDesc[Int32.Parse(sTipoInsumo) - 1] + "_" + (int.Parse(sAnioPresupuestal) - 1).ToString() + "_" + int.Parse(sMes).ToString("D2") + "-" + ConstantesCPPA.mesesDescCorta[int.Parse(sMes) - 1];
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
        /// Exportar plantilla del insumo seleccionado en la carga manual.
        /// </summary>
        /// <param name="sMes">Mes que se realizo la importación</param>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sOpcion">Tipo de insumo seleccionado</param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarPlantillaCargaManualInsumos(string sMes, string sAnioPresupuestal, string sAjuste, string sRevision, string sOpcion)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                List<CpaExcelHoja> listRerExcelHoja = CppaAppServicio.GenerarArchivoPlantillaExcelCargaManual(sMes, sAnioPresupuestal, sAjuste, sRevision, sOpcion, out string nombreArchivo);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, true);

                model.sDetalle = sOpcion;
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
        /// Exportar datos del insumo seleccionado para los meses seleccionados en la opción "Descargar".
        /// </summary>
        /// <param name="idRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses seleccionados</param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportarTotalInsumos(int idRevision, string sTipoInsumo, int[] iMeses)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionJsonResult();
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                List<CpaExcelHoja> listRerExcelHoja = CppaAppServicio.GenerarArchivoExcelDescarga(idRevision, sTipoInsumo, iMeses, out string nombreArchivo);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listRerExcelHoja, rutaArchivo, nombreArchivo, true);

                model.sDetalle = sTipoInsumo;
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

        #region CU07-CU08-CU09-CU10

        /// <summary>
        /// Descargar archivo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a descargar</param>
        /// <returns>Retorna el archivo descargado</returns>
        public virtual ActionResult AbrirArchivo(string nombreArchivo)
        {
            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString());
            rutaNombreArchivo.Append(nombreArchivo);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(nombreArchivo);

            byte[] bFile = System.IO.File.ReadAllBytes(rutaNombreArchivo.ToString());
            System.IO.File.Delete(rutaNombreArchivo.ToString());
            return File(bFile, Constantes.AppExcel, rutaNombreArchivoDescarga.ToString());
        }

        /// <summary>
        /// Paso 4: Lee y valida los datos desde el archivo excel
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="sNumMes">Mes que se realizo la importación</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sAnioPresupuestal, string sAjuste, string sRevision, string sTipoInsumo, string sNumMes)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.iNumMes = int.Parse(sNumMes);
                string sRutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.ValidarExcelImportado15min(sAnioPresupuestal, sAjuste, sRevision, sNumMes, sRutaArchivo, sTipoInsumo);
                if (sTipoInsumo == "2") {
                    model.sMensaje = "Los Costos marginales en el mes de '" + ConstantesCPPA.mesesDesc[int.Parse(sNumMes) - 1] + "' se cargaron correctamente.";
                }
                else {
                    model.sMensaje = "Los datos de las centrales en el mes de '" + ConstantesCPPA.mesesDesc[int.Parse(sNumMes) - 1] + "' son correctos.";
                }
                model.sResultado = "1";
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
        /// Procesar y guarda los registros de los archivos Excels importados de los meses seleccionados
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses seleccionados</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivosInsumo(string sAnioPresupuestal, string sAjuste, string sRevision, string sTipoInsumo, int[] iMeses)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                base.ValidarSesionJsonResult();
                string sUser = User.Identity.Name.Trim();
                CppaAppServicio.procesarTodosArchivosInsumo15min(sAnioPresupuestal, sAjuste, sRevision, sTipoInsumo, iMeses, sUser);
                model.sMensaje = "Finalizó satisfactoriamente la importación de '" + ConstantesCPPA.insumosDesc[int.Parse(sTipoInsumo) - 1] + "' a la base de datos de los archivos Excel seleccionados.";
                //model.sMensaje += sTipoInsumo == "1" || sTipoInsumo == "2" || sTipoInsumo == "3" ? "Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'." : "";
                model.sResultado = "1";
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
        /// Procesar Archivos SDDP
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <param name="pathDirectorio"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ProcesarArchivosSddp(string sAnioPresupuestal, string sAjuste, string sRevision, string pathDirectorio)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                CpaRevisionDTO dtoCpaRevision = CppaAppServicio.GetByIdCpaRevision(Int32.Parse(sRevision));
                model.AjustePresupuestal = CppaAppServicio.GetByIdCpaAjustePresupuestal(dtoCpaRevision.Cpaapcodi);
                string sUser = User.Identity.Name;
                //pathDirectorio = "C:\\tmp\\CSV"; //Se emplea en pruebas, luego de ello siempre comentado

                if (!pathDirectorio.EndsWith("\\"))
                {
                    pathDirectorio += "\\";
                }

                string sLog = "[Inf] [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] Inicio de Proceso [Usuario: " + sUser + "]";
                sLog += CppaAppServicio.ProcesarArchivosSddp(dtoCpaRevision, pathDirectorio, sUser, out bool continuar, out string error);

                if (!continuar)
                {
                    model.sMensaje = sLog;
                    model.sResultado = "-1";
                    //Insertamos el Insumo Año
                    CppaAppServicio.InsertarInsumo(dtoCpaRevision.Cparcodi, ConstantesCPPA.insumoGeneraciónProgramadaPMPO, "A", sLog, sUser);
                    return Json(model);
                }

                string sResultado = CppaAppServicio.ImportarGeneraciónPMPO(model.AjustePresupuestal, dtoCpaRevision, sUser, sLog, out bool continuar2);

                if (!continuar2)
                {
                    model.sMensaje = sResultado;
                    model.sResultado = "-1";
                    return Json(model);
                }


                model.ListInsumo = CppaAppServicio.ListarInsumos(sAnioPresupuestal, sAjuste, sRevision);
                model.sLog = model.ListInsumo[4].Cpainslog;

                if (sResultado == "1")
                {
                    model.sMensaje = "Se completó la importación automática de los archivos SDDP. Para ver el resultado de esta importación, consultar el Log del proceso";
                    model.sResultado = "1";
                }
                else
                {
                    model.sMensaje = sResultado;
                    model.sResultado = "-1";
                }

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
        /// Descargar Excel de los archivos SDDP y CSV
        /// </summary>
        /// <param name="sAnioPresupuestal">Año Presupuestal seleccionado</param>
        /// <param name="sAjuste">Ajuste del Año Presupuestal seleccionado</param>
        /// <param name="sRevision">Id de la revisión seleccionado</param>
        /// <returns>JsonResult</returns>
        public JsonResult DescargarArchivoSDDP(string sAnioPresupuestal, string sAjuste, string sRevision)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                base.ValidarSesionJsonResult();
                CpaRevisionDTO dtoCpaRevision = CppaAppServicio.GetByIdCpaRevision(Int32.Parse(sRevision));
                model.AjustePresupuestal = CppaAppServicio.GetByIdCpaAjustePresupuestal(dtoCpaRevision.Cpaapcodi);
                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesCPPA.ReporteDirectorio].ToString();
                CppaAppServicio.GenerarArchivoExcelSDDP(model.AjustePresupuestal, dtoCpaRevision, rutaArchivo, out string nombreArchivo, out List<CpaExcelHoja> listExcelHoja);
                model.sResultado = CppaAppServicio.ExportarReporteaExcel(listExcelHoja, rutaArchivo, nombreArchivo, true);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }
        #endregion
    }
}