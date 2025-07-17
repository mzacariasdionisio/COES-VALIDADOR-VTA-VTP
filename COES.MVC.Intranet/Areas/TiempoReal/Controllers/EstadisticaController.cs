using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class EstadisticaController : BaseController
    {
        readonly ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();

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

        public EstadisticaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();
            if (!base.VerificarAccesoAccion(Acciones.Importar, base.UserName)) return base.RedirectToHomeDefault();

            EstadisticaModel model = new EstadisticaModel();
            model.Anio = DateTime.Today.Year - 1;
            model.ListaTipoArchivo = new List<GenericoDTO>();
            model.ListaTipoArchivo.Add(new GenericoDTO() { Entero1 = ConstantesTiempoReal.TipoArchivoLineaTrafo, String1 = "Líneas y Transformadores" });
            model.ListaTipoArchivo.Add(new GenericoDTO() { Entero1 = ConstantesTiempoReal.TipoArchivoBarra, String1 = "Barras" });

            return View(model);
        }

        /// <summary>
        /// Importación y generación de archivos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="tipoArchivo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarExcel(int anio, int tipoArchivo, string fileName)
        {
            EstadisticaModel model = new EstadisticaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Importar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaProceso = new DateTime(anio, 1, 1);
                DateTime fechaInicioProceso = DateTime.Now;

                //Si la generacion es diferente al 100% nos devolvera valor 0 y no permitira el registro
                var estado = servScadaSp7.NoExisteProcesoProcesoArchivoEstadisticaEnProceso();
                model.Resultado = estado.ToString();

                if (estado == 1) //si no existe proceso ejecutandose
                {
                    // Ruta de los archivos EXCEL leidos
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes;

                    // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                    servScadaSp7.ValidarArchivoEstadisticaCodigoSP7(tipoArchivo, path, fileName,
                                                   out List<TrEstadisticaEquipo> listaEquiposCorrectos,
                                                   out List<TrEstadisticaEquipo> listaEquiposErroneos,
                                                   out List<TrEstadisticaLog> listaObservaciones);

                    //validación si existen errores
                    if (listaObservaciones.Any())
                    {
                        string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                        string archivoLog = "LogErrores_Validacion_" + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaYMD) + "_" + DateTime.Now.ToString(ConstantesAppServicio.FormatoHHmmss).Replace(":", "");
                        servScadaSp7.GenerarExcelLogErrores(pathLogo, path + archivoLog, DateTime.Now, base.UserName, listaObservaciones);

                        model.FileName = archivoLog;
                        model.Resultado = "-1";
                        model.Mensaje = "¡Existen observaciones al archivo cargado!";
                    }
                    else
                    {
                        //Ejecución del proceso
                        string usuario = base.UserName;
                        int enviocodi = servScadaSp7.GetNuevoEnvioProcesoArchivoEstadistica(fechaProceso, fechaInicioProceso, usuario);
                        HostingEnvironment.QueueBackgroundWorkItem(token => ProcesamientoSegundoPlano(enviocodi, anio, tipoArchivo, listaEquiposCorrectos, token));

                        model.Resultado = "1";
                        model.Mensaje = "Archivo cargado correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);

                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        private async Task ProcesamientoSegundoPlano(int enviocodi, int anio, int tipoArchivo, List<TrEstadisticaEquipo> listaEquiposCorrectos,
                                                CancellationToken cancellationToken)
        {
            try
            {
                //Segundo plano Generacion de version
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes;

                servScadaSp7.GenerarArchivoEstadisticaTiempoReal(enviocodi, anio, path, tipoArchivo, listaEquiposCorrectos);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);

                //si hay error en la ejecucion entonces actualizar estado del proceso y agregarlo al log
                MeEnvioDTO reg = servScadaSp7.GetByIdMeEnvio(enviocodi);
                reg.Enviodesc = "-1";
                servScadaSp7.UpdateMeEnvioDesc(reg);
            }
        }

        /// <summary>
        /// Lista detalle de proceso
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarProcesamiento(int anio)
        {
            EstadisticaModel model = new EstadisticaModel();

            try
            {
                DateTime fechaProceso = new DateTime(anio, 1, 1);

                model.Resultado = servScadaSp7.NoExisteProcesoProcesoArchivoEstadisticaEnProceso().ToString();
                model.Envio = servScadaSp7.GetUltimoEnvioProcesoArchivoEstadistica(fechaProceso);
                model.Resultado = "0";

                //generar archivo zip
                if (model.Envio != null && model.Envio.Enviodesc == "100")
                {
                    string rutaCarpeta = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes;
                    servScadaSp7.GenerarArchivosSalidaProcesoArchivoEstadisticaZip(rutaCarpeta, anio, out string nameFile);

                    model.FileName = nameFile;
                    model.Resultado = "1";
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
        /// Descargar archivo comprimido
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarZip()
        {
            string strArchivoNombre = Request["file_name"];

            string strArchivoTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes + strArchivoNombre;
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strArchivoNombre);
        }

        /// <summary>
        /// Descarga de log de errores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelLog()
        {
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes;
            string strArchivoTemporal = path + strArchivoNombre;
            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// Upload archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadArchivo()
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        FileServer.DeleteBlob(sNombreArchivo, path + ConstantesTiempoReal.RutaReportes);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// MostrarArchivoImportacion
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarArchivoImportacion(string sFileName)
        {
            base.ValidarSesionUsuario();

            EstadisticaModel model = new EstadisticaModel();

            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes);

            foreach (var item in model.ListaDocumentos)
            {
                if (String.Equals(item.FileName, sFileName))
                {
                    model.Documento = new FileData();
                    model.Documento = item;
                    break;
                }
            }

            return Json(model);
        }

        /// <summary>
        /// EliminarArchivosImportacionNuevo
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        [HttpPost]
        public int EliminarArchivosImportacionNuevo(string nombreArchivo)
        {
            base.ValidarSesionUsuario();

            string nombreFile = string.Empty;

            EstadisticaModel model = new EstadisticaModel();
            model.ListaDocumentos = FileServer.ListarArhivos(null, AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes);
            foreach (var item in model.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesTiempoReal.RutaReportes);
            }

            return -1;
        }

    }
}