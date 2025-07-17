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
using System.Globalization;
using System.IO;
using System.Reflection;

using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class QnMensualController : BaseController
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
        /// Index Serie Hidrologica
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            QnMensualModel model = new QnMensualModel();

            try
            {
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.UsarLayoutModulo = false;
                string mesAnioMensual = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroMensual); 
                string mesAnioSemanal = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroSemanal);

                model.MesAnioMensual = mesAnioMensual;
                model.MesAnioSemanal = mesAnioSemanal;
                model.Mes = model.MesAnioMensual.Replace("*", "");
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
        /// Devuelve el html del listado de series hidrologicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarSeriesHidrologica()
        {
            QnMensualModel model = new QnMensualModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.HtmlListadoSeriesHidrologica = pmpoServicio.HtmlListadoSerieHidrologica(url, tienePermiso);

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
        /// Devuelve el html de la ventana de versiones de un tipo de serie Hidrologica
        /// </summary>
        /// <param name="anioTipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int anio, int mes, int tipo)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = pmpoServicio.GenerarHtmlVersionesSerieHidrologia(url, anio, mes, tipo, tienePermiso);
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
            QnMensualModel model = new QnMensualModel();

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
        /// Actualiza informacion general en Detalles de la SH (rango de años, anio, mes,...)
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosGeneralesSerieHidrologica(int accion, int? qnbenvcodi)
        {
            QnMensualModel model = new QnMensualModel();

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
                    model.Mes = regEnvioVersion.Qnbenvfechaperiodo.Value.Month.ToString();
                    model.RangoIni = (regEnvioVersion.Qnbenvanho - 4).ToString();
                    model.RangoFin = (regEnvioVersion.Qnbenvanho + 1).ToString();
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
        /// Obtener datos generales del handson para armar la tabla web
        /// </summary>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="tipo"></param>
        /// <param name="accion"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosParaHandson(int anioSerie, int mesSerie, int anioIni, int anioFin, int tipo, int accion, int? qnbenvcodi = 0)
        {
            QnMensualModel model = new QnMensualModel();
            try
            {
                base.ValidarSesionJsonResult();

                //Validar rango de 6 años
                string msgValidacion = pmpoServicio.ValidarRangoAnios(anioIni, anioFin, tipo);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                //Obtiene data almacenada en forma de columnas en la BD (4 registros por año si es Semanal y 1 registro por año si es Mensual)
                List<PmoQnMedicionDTO> lstQNMedicion = pmpoServicio.ListarQnMedicionParaArmarHandsonSerieHidrologica(base.UserName, accion, anioSerie, mesSerie, anioIni, anioFin, tipo, qnbenvcodi ?? 0, out int codigoBase, out int enviocodiAlCrear, out string msgNota);
                model.NotaNuevoOficial = msgNota;

                lstQNMedicion = pmpoServicio.ConsiderarCambiosEnCantidadDeEstaciones(accion, tipo, lstQNMedicion);

                model.DataHandsonSeriesHidrologica = pmpoServicio.ArmarHandsonParaSerieHidrologica(anioIni, anioFin, tipo, accion, lstQNMedicion);
                int codi = accion == ConstantesPMPO.AccionCrear ? enviocodiAlCrear : qnbenvcodi.Value;
                model.NotaVersion = pmpoServicio.ObtenerNotaDeVersionMostrada(codi, anioIni, anioFin);
                model.NumFilas = pmpoServicio.ObtenerNumeroDeFilasDelHandson(anioIni, anioFin, tipo);
                model.CodigoInfoBase = codigoBase;
                model.LstCabeceras = pmpoServicio.ObtenerArrayCabeceras(model.DataHandsonSeriesHidrologica.Columnas);

                //actualizo las fechas (ya que al crear SH se autoguarda)
                string mesAnioMensual = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroMensual);
                string mesAnioSemanal = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroSemanal);
                model.MesAnioMensual = mesAnioMensual;
                model.MesAnioSemanal = mesAnioSemanal;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        /// <summary>
        /// Guarda una serie hidrologica desde la pestaña DETALLES
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="codigoBase"></param>
        /// <param name="stringJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarSerieHidrologica(int tipo, int anioSerie, int mesSerie, int anioIni, int anioFin, int codigoBase, string stringJson)
        {
            QnMensualModel model = new QnMensualModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                List<PmoQnMedicionDTO> lstSerieBase = pmpoServicio.ObtenerListaQnMedicionDesdeElHandson(tipo, anioIni, anioFin, stringJson);

                string nombre = "";
                switch (tipo)
                {
                    case ConstantesPMPO.SerieHidroSemanal:
                        nombre = "Serie Semanal " + pmpoServicio.ObtenerNombreMes(mesSerie) + " " + anioSerie;
                        break;
                    case ConstantesPMPO.SerieHidroMensual:
                        nombre = "Serie Mensual " + pmpoServicio.ObtenerNombreMes(mesSerie) + " " + anioSerie;
                        break;
                    default:
                        break;
                }
                PmoQnEnvioDTO miEnvio = new PmoQnEnvioDTO()
                {
                    Qnbenvanho = anioSerie,
                    Qnbenvnomb = nombre,
                    Qnbenvfechaperiodo = new DateTime(anioSerie, mesSerie, 1)
                };

                //Guardar información importada                
                int envio = pmpoServicio.GuardarDatosSerieHidro(tipo, miEnvio, lstSerieBase, anioSerie, codigoBase, base.UserName);
                model.CodigoEnvio = envio;
                string mesAnioMensual = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroMensual);
                string mesAnioSemanal = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroSemanal);

                model.MesAnioMensual = mesAnioMensual;
                model.MesAnioSemanal = mesAnioSemanal;


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
        /// Valida la serie a guardar 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidacionNuevaSerie(int tipo, int anio, int mes)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();

                string msgValidacion = pmpoServicio.ValidarNuevaSerie(tipo, anio, mes);  //Rosmel PENDIENTE
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
        /// Eliminar serie hidrologica
        /// </summary>
        /// <param name="qnbenvcodi"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarSerieHidrologica(int qnbenvcodi, int anio, int mes, int tipo)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.EliminarSerieGeneral(qnbenvcodi, anio, mes, tipo);

                string mesAnioMensual = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroMensual);
                string mesAnioSemanal = pmpoServicio.ultimoMesAnioRegistradoMasUnMes(ConstantesPMPO.SerieHidroSemanal);
                model.MesAnioMensual = mesAnioMensual;
                model.MesAnioSemanal = mesAnioSemanal;

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
        /// Escoger serie hidrologica como vigente
        /// </summary>
        /// <param name="qnbenvcodi"></param>
        /// <param name="tipo"></param>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarVigencia(int qnbenvcodi, int tipo, int anioSerie, int mesSerie)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.AprobarVersionSerieGeneral(qnbenvcodi, base.UserName, anioSerie, mesSerie, tipo);

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
        /// <param name="mes"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarSerieHidroCompleta(int codEnvio, int anio, int mes, int tipo, int codigoBase, int accion)
        {
            QnMensualModel model = new QnMensualModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = pmpoServicio.GenerarArchivoExcelDesde1965(ruta, codEnvio, mes, anio, tipo, codigoBase, accion); //Rosmel

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
        /// Devuelve handson actualizado segun el boton presionado (historico, pronostico o autocompletado) para lo cual se le envia el handson previo
        /// </summary>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="tipo"></param>
        /// <param name="accion"></param>
        /// <param name="origen"></param>
        /// <param name="codigoBase"></param>
        /// <param name="stringJson"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarBloqueHandsonSegunOrigen(int anioSerie, int mesSerie, int anioIni, int anioFin, int tipo, int accion, int origen, int codigoBase, string stringJson, int? qnbenvcodi = 0)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<PmoQnMedicionDTO> lstSerieHidroDeTablaWeb = pmpoServicio.ObtenerListaQnMedicionDesdeElHandson(tipo, anioIni, anioFin, stringJson);

                List<PmoQnMedicionDTO> lstQNMedicionActualizada = pmpoServicio.ActualizarListaQnMedicionSegunOrigen(codigoBase, anioSerie, mesSerie, anioIni, anioFin, tipo, origen, lstSerieHidroDeTablaWeb);

                model.DataHandsonSeriesHidrologica = pmpoServicio.ArmarHandsonParaSerieHidrologica(anioIni, anioFin, tipo, accion, lstQNMedicionActualizada);
                model.NumFilas = pmpoServicio.ObtenerNumeroDeFilasDelHandson(anioIni, anioFin, tipo);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }        


        /// <summary>
        /// Genera los archivos .dat de salida
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoDat(int qnbenvcodi, int tipo, int anio, int mes, int codigoBase, int accion)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                pmpoServicio.GenerarArchivoDatSerieHidro(ruta, tipo, anio, mes, qnbenvcodi, codigoBase,  accion, out string nameFile);  //rosmel
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

        #region Notificación

        /// <summary>
        /// Notificar envíos pendientes a agentes
        /// </summary>
        /// <returns></returns>
        public JsonResult NotificarPendiente(string mesElaboracion)
        {
            QnMensualModel model = new QnMensualModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                pmpoServicio.EnviarNotificacionEnvioPendiente(fecha1Mes, ruta, out string fileLog, out int totalPendiente);
                model.Resultado = totalPendiente.ToString();
                model.NameFileLog = fileLog;
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
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarLogNotificacion(string archivo)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
            string fullPath = ruta + archivo;

            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, "emailsResult.txt");
        }

        #endregion
    }
}