using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using DevExpress.XtraRichEdit;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class ConsolidadoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();

        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
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

        #region Reportes
       
        /// <summary>
        /// Devuelve el listado de periodos trimestrales asociados al semestral
        /// </summary>
        /// <param name="periodoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPeriodosTrimestralesAsociados(int periodoId)
        {
            ConsolidadoModel model = new ConsolidadoModel();

            try
            {
                RePeriodoDTO periodo = servicioResarcimiento.GetByIdRePeriodo(periodoId);                
                model.ListaPeriodos = servicioResarcimiento.ObtenerPeriodosTrimestralesAsociados(periodo);
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
        /// Genera cierto reporte a exportar en excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteEnExcel(int codigoReporte, int periodoId, int periodoTrimestral)
        {
            ConsolidadoModel model = new ConsolidadoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = servicioResarcimiento.ObtenerNombreReporteEnExcel(codigoReporte);
                

                servicioResarcimiento.GenerarReporteEnExcel(periodoId, periodoTrimestral, codigoReporte, ruta, pathLogo, nameFile);
                model.Resultado = nameFile;
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
        /// Genera cierto reporte a exportar en word
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteEnWord(int codigoReporte, int periodoId)
        {
            ConsolidadoModel model = new ConsolidadoModel();

            try
            {
                base.ValidarSesionJsonResult();

                RePeriodoDTO periodo = servicioResarcimiento.GetByIdRePeriodo(periodoId);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string pathPlantilla = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                string nameFile = servicioResarcimiento.ObtenerNombreReporteEnWord(periodo);

                servicioResarcimiento.GenerarReporteEnWord(periodo, codigoReporte, ruta, pathLogo, nameFile, pathPlantilla);                
                model.Resultado = nameFile;               
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
        /// Genera cierto reporte a exportar en zip
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <param name="periodoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteEnZip(int codigoReporte, int periodoId)
        {
            ConsolidadoModel model = new ConsolidadoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = servicioResarcimiento.ObtenerNombreReporteEnZip(codigoReporte);

                servicioResarcimiento.GenerarReporteZip(periodoId, codigoReporte, ruta, pathLogo, nameFile);               

                model.Resultado = nameFile;
                model.idReporteZip = codigoReporte == ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorAgenteResponsable ? "1" : (codigoReporte == ConstantesCalculoResarcimiento.ReporteZipInterrupcionesPorSuministrador ? "2" : "");
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
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarZip()
        {
            string nombreArchivo = Request["file_name"];
            string idReporte = Request["reporteId"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            string fullPath = "";
            if (idReporte == "1")
                fullPath = ruta + "ReportePorAgente\\" + nombreArchivo;
            if (idReporte == "2")
                fullPath = ruta + "ReportePorSuministrador\\" + nombreArchivo;

            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar extension
        }

        /// <summary>
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }
        #endregion

        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConsolidadoModel model = new ConsolidadoModel();
            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicioResarcimiento.ObtenerPeriodosPorAnio(model.Anio);
            //model.ListaSuministrador = this.servicioResarcimiento.ObtenerEmpresasSuministradorasTotal();           
            return View(model);
        }

        /// <summary>
        /// Permite obtener los periodos semestrales por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicioResarcimiento.ObtenerPeriodosPorAnio(anio));
        }

        /// <summary>
        /// Permite obtener la habilitacion del consolidado
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HabilitarConsulta(int periodo, string tipo)
        {
            return Json(this.servicioResarcimiento.HabilitarConsolidado(periodo, tipo));
        }

        /// <summary>
        /// Permite obtener los filtros actualizados
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="suministrador"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosPorSuministrador(int periodo, int suministrador, string tipo)
        {
            return Json(this.servicioResarcimiento.ObtenerDatosPorSuministrador(suministrador, periodo, tipo));
        }

        /// <summary>
        /// Permite obtener la consulta del consolidado
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarInterrupcion(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
            string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
            string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            EstructuraConsolidado estructura = this.servicioResarcimiento.ObtenerConsultaConsolidado(periodo, tipo, suministrador, barra, causaInterrupcion,
                conformidadResponsable, conformidadSuministrador, evento, alimentadorSED, estado, responsable, disposicion, compensacion, buscar);
            estructura.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return Json(estructura);
        }

        /// <summary>
        /// Permite grabar la data de interrupciones
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarInterrupcion(string[][] data, int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
            string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
            string estado, int responsable, string disposicion, string compensacion, string buscar)        
        {
            return Json(this.servicioResarcimiento.GrabarDatosAdicionalesInterrupciones(data, periodo, tipo, suministrador, barra, causaInterrupcion,
                conformidadResponsable, conformidadSuministrador, evento, alimentadorSED, estado, responsable, disposicion, compensacion, buscar));
        }

        /// <summary>
        /// Exportar el consolidado de interrupciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarConsolidado(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar)
        {                                    
            try {                            
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string fileName = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ?
                    ConstantesCalculoResarcimiento.ArchivoConsolidadoInterrupcion : ConstantesCalculoResarcimiento.ArchivoConsolidadoRechazoCarga;

                this.servicioResarcimiento.ExportarConsolidado(path, fileName, periodo, tipo, suministrador, barra, causaInterrupcion,
                    conformidadResponsable, conformidadSuministrador, evento, alimentadorSED, estado, responsable, disposicion, compensacion, buscar);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar el archivo de puntos de entrega
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCalculoResarcimiento.ArchivoImportacionConsolidado;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importacion de suministros
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInterrupciones(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos;
                string file = ConstantesCalculoResarcimiento.ArchivoImportacionConsolidado;

                List<string> validaciones = new List<string>();
                string[][] data = this.servicioResarcimiento.CargarConsolidadoExcel(path, file, periodo, tipo, suministrador, barra, causaInterrupcion,
                    conformidadResponsable, conformidadSuministrador, evento, alimentadorSED, estado, responsable, disposicion, compensacion, buscar, out validaciones);

                if (validaciones.Count == 0)
                    return Json(new { Result = 1, Data = data, Errores = new List<string>() });
                else
                    return Json(new { Result = 2, Data = new string[1][], Errores = validaciones });
            }
            catch
            {
                return Json(new { Result = -1, Data = new string[1][], Errores = new List<string>() });
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarConsolidado(string tipo)
        {
            string fileName = (tipo == ConstantesCalculoResarcimiento.EnvioTipoInterrupcion) ?
                    ConstantesCalculoResarcimiento.ArchivoConsolidadoInterrupcion : ConstantesCalculoResarcimiento.ArchivoConsolidadoRechazoCarga;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCalculoResarcimiento.RutaExportacionInsumos +
                fileName;
            return File(fullPath, Constantes.AppExcel, fileName);
        }

        /// <summary>
        /// Permite obtener la trazabilidad de un registro
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerTrazabilidad(string tipo, int id)
        {
            return Json(this.servicioResarcimiento.ObtenerTrazabilidad(tipo, id));
        }

        /// <summary>      
        /// Permite obtener el motivo de anulación de un registro
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerMotivoAnulacion(string tipo, int id)
        {
            return Json(this.servicioResarcimiento.ObtenerMotivoAnulacion(tipo, id));
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesCalculoResarcimiento.ModuloManualUsuario;
            string nombreArchivo = ConstantesCalculoResarcimiento.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesCalculoResarcimiento.FolderRaizResarcimientosModuloManual;
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
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoInterrupcion(int id, string extension)
        {            
            string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoInterrupcion, id.ToString(), extension);

            /*
            string fullPath = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento() + fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
               ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoObservacion(int id, int idDet, string extension)
        {
            int idDetalle = this.servicioResarcimiento.ObtenerIdDetalleInterrupcion(id, idDet);
            string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoObservacion, idDetalle.ToString(), extension);
            /*string fullPath = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento() + fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
               ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite descargar el archivo de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoRespuesta(int id, int idDet, string extension)
        {
            int idDetalle = this.servicioResarcimiento.ObtenerIdDetalleInterrupcion(id, idDet);
            string fileName = string.Format(ConstantesCalculoResarcimiento.ArchivoRespuesta, idDetalle.ToString(), extension);
            /*string fullPath = this.servicioResarcimiento.ObtenerPathArchivosResarcimiento() + fileName;
            return File(fullPath, Constantes.AppExcel, fileName);*/
            Stream stream = FileServer.DownloadToStream(ConstantesCalculoResarcimiento.RutaResarcimientos + fileName,
               ConstantesCalculoResarcimiento.RutaBaseArchivoResarcimiento);


            return File(stream, extension, fileName);
        }

        /// <summary>
        /// Permite descarga masiva de archivos
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="suministrador"></param>
        /// <param name="barra"></param>
        /// <param name="causaInterrupcion"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="evento"></param>
        /// <param name="alimentadorSED"></param>
        /// <param name="estado"></param>
        /// <param name="responsable"></param>
        /// <param name="disposicion"></param>
        /// <param name="compensacion"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivosGeneral(int periodo, string tipo, int suministrador, int barra, int causaInterrupcion,
           string conformidadResponsable, string conformidadSuministrador, int evento, string alimentadorSED,
           string estado, int responsable, string disposicion, string compensacion, string buscar)
        {
            string fileName = string.Empty;
            Stream stream = this.servicioResarcimiento.GenerarArchivoConsolidadoComprimido(periodo, tipo, suministrador, barra, causaInterrupcion,
                    conformidadResponsable, conformidadSuministrador, evento, alimentadorSED, estado, responsable, disposicion, compensacion, buscar, out fileName);

            return File(stream, ".zip", fileName);
        }
    }
}