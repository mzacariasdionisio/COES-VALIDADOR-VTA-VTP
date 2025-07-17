using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class QnSerieBaseController : BaseController
    {
        private readonly ProgramacionAppServicio pmpoServicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametrosFechasController));
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


        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesPMPO.SesionNombreArchivo] != null) ?
                    Session[ConstantesPMPO.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesPMPO.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[ConstantesPMPO.SesionMatrizExcel] != null) ?
                    (string[][])Session[ConstantesPMPO.SesionMatrizExcel] : new string[1][];
            }
            set { Session[ConstantesPMPO.SesionMatrizExcel] = value; }
        }


        #region Métodos 

        /// <summary>
        /// Index Serie Base
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.UsarLayoutModulo = false;
                int ultimoAnioMensualConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseMensual).Qnbenvanho.Value; 
                int ultimoAnioSemanalConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseSemanal).Qnbenvanho.Value;
                
                model.AnioMensual = (ultimoAnioMensualConOficial + 1).ToString();
                model.AnioSemanal = (ultimoAnioSemanalConOficial + 1).ToString();                
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
        /// Devuelve el html del listado de series base
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarSeriesBase()
        {
            QnSerieBaseModel model = new QnSerieBaseModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.HtmlListadoSeriesBase = pmpoServicio.HtmlListadoSerieBase(url, tienePermiso);
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
        /// Devuelve el html de la ventana de versiones de un tipo de serie base
        /// </summary>
        /// <param name="anioTipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(string anio, int tipo)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = pmpoServicio.GenerarHtmlVersionesSerieBase(url, int.Parse(anio), tipo, tienePermiso);
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
        /// Muestra html de la pestaña "Detalles"
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int qnbenvcodi)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Accion = accion;
                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Actualiza informacion general en Detalles de la SB (rango de años, numEstaciones,...)
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosGeneralesSerieBase( int accion, int? qnbenvcodi)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva funcionalidades en el handsonTable               

                if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                {
                    PmoQnEnvioDTO regEnvioVersion = pmpoServicio.GetByIdPmoQnEnvio(qnbenvcodi.Value);
                    model.Anio = regEnvioVersion.Qnbenvanho.ToString();
                    model.RangoIni = (regEnvioVersion.Qnbenvanho - 2).ToString();
                    model.RangoFin = (regEnvioVersion.Qnbenvanho).ToString();
                }
                
                model.NumEstaciones = pmpoServicio.ListPmoEstacionhs().Count;                
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
        /// Obtener datos generales del handson para armar la tabla web
        /// </summary>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="tipo"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosParaHandson(int anioSerie, int anioIni, int anioFin, int tipo, int accion, int? qnbenvcodi = 0)
        {            
            QnSerieBaseModel model = new QnSerieBaseModel();            
            try
            {
                base.ValidarSesionJsonResult();

                //Validar rango de 3 años
                string msgValidacion = pmpoServicio.ValidarRangoAnios(anioIni, anioFin, tipo);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                //Obtiene data almacenada en forma de columnas en la BD (4 registros por año si es Semanal y 1 registro por año si es Mensual)
                List<PmoQnMedicionDTO> lstQNMedicion = pmpoServicio.ListarQnMedicionParaArmarHandsonSerieBase(accion, anioSerie, anioIni, anioFin, tipo, qnbenvcodi ?? 0, out int codigoBase);
                
                lstQNMedicion = pmpoServicio.ConsiderarCambiosEnCantidadDeEstaciones(accion, tipo, lstQNMedicion);

                model.DataHandsonSeriesBase = pmpoServicio.ArmarHandsonParaSerieBase(anioIni, anioFin, tipo, accion, lstQNMedicion); 
                model.NotaVersion = pmpoServicio.ObtenerNotaDeVersionMostrada(qnbenvcodi, anioIni, anioFin);
                model.NumFilas = pmpoServicio.ObtenerNumeroDeFilasDelHandson(anioIni, anioFin, tipo);
                model.CodigoInfoBase = codigoBase;
                model.LstCabeceras = pmpoServicio.ObtenerArrayCabeceras(model.DataHandsonSeriesBase.Columnas);
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
        /// Guarda una serie base desde la pestaña DETALLES
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="stringJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarSeriesBase(int tipo, int anioSerie, int anioIni, int anioFin, int codigoBase, string stringJson)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                List<PmoQnMedicionDTO> lstSerieBase = pmpoServicio.ObtenerListaQnMedicionDesdeElHandson(tipo, anioIni, anioFin, stringJson);

                string nombre = "";
                switch (tipo)
                {
                    case ConstantesPMPO.SerieBaseSemanal:
                        nombre = "Serie Base Semanal ";
                        break;
                    case ConstantesPMPO.SerieBaseMensual:
                        nombre = "Serie Base Mensual ";
                        break;
                    default:
                        break;
                }
                PmoQnEnvioDTO miEnvio = new PmoQnEnvioDTO()
                {
                    Qnbenvanho = anioSerie,
                    Qnbenvnomb = nombre + anioSerie.ToString(),
                    Qnbenvfechaperiodo = new DateTime(anioSerie, 1, 1)
                };

                lstSerieBase = pmpoServicio.ActualizarDataSerieBaseInicial(tipo,anioSerie, lstSerieBase, codigoBase);

                //Guardar información importada
                int envio = pmpoServicio.GuardarDatosWeb(tipo, miEnvio, lstSerieBase, anioSerie, base.UserName);
                model.CodigoEnvio = envio;
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
        /// (Descargar Formato) Permite  descargar plantilla para la importación masiva, 
        /// en el explorador
        /// </summary>        
        /// <returns>Archivo</returns>
        [HttpPost]
        public JsonResult DescargarFormatoPlantilla( int tipo , int codEnvio, int anio)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = pmpoServicio.GenerarPlantillaExcellSerieBase(ruta, codEnvio,  anio,  tipo);

                model.Resultado = nameFile;
                model.Resultado2 = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado2 = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        

        /// <summary>
        /// Importar excel 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesPMPO.ExtensionFile;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Leer archivo y guardar informacion del excel importado
        /// </summary>
        /// <param name="anioBase"></param>
        /// <param name="tipo"></param>
        /// <param name="numEstaciones"></param>
        /// <returns></returns>
        public JsonResult LeerArchivoExcelYGuardar(int anioBase, int tipo, int numEstaciones)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();
            try
            {
                base.ValidarSesionJsonResult();

                // Inicializa y lee excel importado
                List<String> validaciones = new List<String>();
                pmpoServicio.LeerArchivoExcelImportado(this.NombreFile, tipo, numEstaciones, anioBase, out string[][] matrizDatos, out validaciones);
                if (validaciones.Count > 0) {
                    string msgValidacion = pmpoServicio.DarFormatoValidaciones(validaciones);
                    throw new ArgumentException(msgValidacion);
                }
                    
                this.MatrizExcel = matrizDatos;

                //grabar ecxel
                var envio = this.GuardarDatosDeExcelImportado(tipo, anioBase, numEstaciones, matrizDatos);
                model.CodigoEnvio = envio;

                FileInfo fileInfo = new FileInfo(this.NombreFile);
                if (fileInfo.Exists)
                {
                    ToolsFormato.BorrarArchivo(this.NombreFile);
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

            return Json(model);
        }
        
        
        /// <summary>
        /// Guarda informacion desde el formato descargado
        /// </summary>
        /// <param name="tipoConfig"></param>
        /// <param name="anioBase"></param>
        /// <param name="sddpCount"></param>
        /// <param name="matrizDatos"></param>
        /// <returns></returns>
        public int GuardarDatosDeExcelImportado(int tipoConfig, int anioBase, int sddpCount, string[][] matrizDatos)
        {
            QnSerieBaseModel jsModel = new QnSerieBaseModel();
            int envio = 0;
            try
            {
                base.ValidarSesionJsonResult();
                envio = pmpoServicio.GrabarDatosDeExcelImportado(tipoConfig, anioBase, matrizDatos, sddpCount, base.UserName);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = "No se procesó la información. " + ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            return envio;
        }

        /// <summary>
        /// Valida la serie a guardar 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidacionNuevaSerie(int tipo, int anio)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();

                string msgValidacion = pmpoServicio.ValidarNuevaSerie(tipo, anio, 0);  //Rosmel Completar
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

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
        /// Eliminar serie base
        /// </summary>
        /// <param name="qnbenvcodi"></param>
        /// <param name="anio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarSerieBase(int qnbenvcodi, int anio, int tipo)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.EliminarSerieGeneral(qnbenvcodi, anio, 0, tipo);

                //actualizar fechas popup crear
                int ultimoAnioMensualConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseMensual).Qnbenvanho.Value;
                int ultimoAnioSemanalConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseSemanal).Qnbenvanho.Value;
                model.AnioMensual = (ultimoAnioMensualConOficial + 1).ToString();
                model.AnioSemanal = (ultimoAnioSemanalConOficial + 1).ToString();

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
        /// Escoger serie base como vigente
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarVigencia(int qnbenvcodi, int tipo, int anioSerie)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.AprobarVersionSerieGeneral(qnbenvcodi, base.UserName, anioSerie, 0, tipo);

                model.Resultado = "1";
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

        /// <summary>
        /// Marca como OFICIAL a una serie base
        /// </summary>
        /// <param name="qnbenvcodi"></param>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MarcarOficial(int qnbenvcodi, int tipo, int anioSerie)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.AsignarOficialSerieBase(qnbenvcodi, base.UserName, anioSerie, tipo);

                //actualizar fechas popup crear
                int ultimoAnioMensualConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseMensual).Qnbenvanho.Value;
                int ultimoAnioSemanalConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseSemanal).Qnbenvanho.Value;
                model.AnioMensual = (ultimoAnioMensualConOficial + 1).ToString();
                model.AnioSemanal = (ultimoAnioSemanalConOficial + 1).ToString();

                model.Resultado = "1";
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

        /// <summary>
        /// DESOFICIALIZAR a una serie base
        /// </summary>
        /// <param name="qnbenvcodi"></param>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QuitarOficial(int qnbenvcodi, int tipo, int anioSerie)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.DesoficializarSerieBase(qnbenvcodi, base.UserName);

                //actualizar fechas popup crear
                int ultimoAnioMensualConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseMensual).Qnbenvanho.Value;
                int ultimoAnioSemanalConOficial = pmpoServicio.ObtenerUltimoAnioOficial(ConstantesPMPO.SerieBaseSemanal).Qnbenvanho.Value;
                model.AnioMensual = (ultimoAnioMensualConOficial + 1).ToString();
                model.AnioSemanal = (ultimoAnioSemanalConOficial + 1).ToString();

                model.Resultado = "1";
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

        /// <summary>
        /// (Exportar Información) descarga datos de tabla web completa (desde 1965) en excel
        /// </summary>
        /// <param name="codEnvio"></param>
        /// <param name="anio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarSerieBaseCompleta(int codEnvio, int anio, int tipo, int accion)
        {
            QnSerieBaseModel model = new QnSerieBaseModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = pmpoServicio.GenerarArchivoExcelDesde1965(ruta, codEnvio, 0, anio, tipo, 0, accion);

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
        /// Devuelve un archivo excel exportado
        /// </summary>
        /// <returns></returns>
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        #endregion
    }
}