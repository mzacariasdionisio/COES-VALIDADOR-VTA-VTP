using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ReportSubmissionController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();
        readonly FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #region Variables session

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesFormato.SesionNombreArchivo] != null) ?
                    Session[ConstantesFormato.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesFormato.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[ConstantesFormato.SesionMatrizExcel] != null) ?
                    (string[][])Session[ConstantesFormato.SesionMatrizExcel] : new string[1][];
            }
            set { Session[ConstantesFormato.SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Lista de Matriz de datos
        /// </summary>
        public List<string[][]> ListaMatrizExcel
        {
            get
            {
                return (Session[ConstantesFormato.SesionListaMatrizExcel] != null) ?
                    (List<string[][]>)Session[ConstantesFormato.SesionListaMatrizExcel] : new List<string[][]>();
            }
            set { Session[ConstantesFormato.SesionListaMatrizExcel] = value; }
        }

        /// <summary>
        /// Lista hoja
        /// </summary>
        public List<int> ListaHoja
        {
            get
            {
                return (Session[ConstantesFormato.SesionListaHoja] != null) ?
                    (List<int>)Session[ConstantesFormato.SesionListaHoja] : new List<int>();
            }
            set { Session[ConstantesFormato.SesionListaHoja] = value; }
        }

        #endregion

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();

            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);
            model.ListaFormato = pmpo.ListFormatosPmpoExtranet();
            model.ListaEmpresa = pmpo.GetListaEmpresasPMPO();
            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            return View(model);
        }

        /// <summary>
        /// Reporte de envíos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="mesElaboracion"></param>
        /// <param name="estEnvio"></param>
        /// <param name="estDerivacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaReporteEnvio(int emprcodi, int formatcodi, string mesElaboracion, int estEnvio, int estDerivacion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");

                model.PlazoFormato = pmpo.GetPlazoFormatoPmpoXFecha(fecha1Mes);
                model.HtmlReporte = pmpo.GenerarHtmlReporteEnvio(url, emprcodi, formatcodi, fecha1Mes, estEnvio, estDerivacion);

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

        #region Derivación a COES

        /// <summary>
        /// Verificar existencia de derivación
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="mesElaboracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExisteDerivacion(int emprcodi, int formatcodi, string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                model.Resultado = "1"; //puede continuar con el registro de la derivación
                if (pmpo.ExisteDerivacionCOES(emprcodi, formatcodi, fecha1Mes))
                    model.Resultado = "0"; //pregunta al usuario si requiere registrar una derivación adicional
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
        /// Usuario SPR realiza derivación
        /// </summary>
        /// <param name="enviocodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="mesElaboracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DerivarValidacionCOES(int enviocodi, int emprcodi, int formatcodi, string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                int idEnvio = pmpo.DerivarValidacionCoes(enviocodi, emprcodi, formatcodi, fecha1Mes, base.UserName);
                model.Resultado = idEnvio.ToString();
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
        /// Reporte de envíos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="mesElaboracion"></param>
        /// <param name="estEnvio"></param>
        /// <param name="estDerivacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaReporteValidacion(int emprcodi, int formatcodi, string mesElaboracion, string estadoCumplimiento)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");

                model.PlazoFormato = pmpo.GetPlazoFormatoPmpoXFecha(fecha1Mes);
                model.HtmlReporte = pmpo.GenerarHtmlReporteValidacion(url, emprcodi, formatcodi, fecha1Mes, estadoCumplimiento);

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
        /// Formatos de la empresa seleccionada
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        private List<MeFormatoDTO> ListarFormatoXEmpresa(int emprcodi)
        {
            var listaFmt = pmpo.ListarTipoPMPOXEmpresa(emprcodi);
            if (!listaFmt.Any())
            {
                listaFmt.Add(new MeFormatoDTO() { Formatcodi = 0, Formatnombre = "No existe" });
            }

            foreach (var reg in listaFmt)
            {
                reg.Formatnombre = reg.Formatnombre.Replace("PMPO_", "");
            }

            return listaFmt;
        }

        #region Reportes información formatos

        /// <summary>
        /// Generar Reporte masivo de formatos
        /// </summary>
        /// <param name="formatos"></param>
        /// <param name="mesElaboracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReportesFormatos(string formatos, string mesElaboracion)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                string rutaCarpeta = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                pmpo.ListarFeriadosXAnio(fecha1Mes.Year + 1, out List<string> listMsjVal, out PmoAnioOperativoDTO objAnioOperativoSgte);
                if (objAnioOperativoSgte == null)
                {
                    throw new ArgumentException("No está configurado el año operativo " + fecha1Mes.Year + 1);
                }

                //Generar reportes
                pmpo.GenerarReportesExcel(rutaCarpeta, pathLogo, formatos, fecha1Mes);

                //generar archivo zip
                pmpo.GenerarArchivosSalidaFormatosZip(rutaCarpeta, fecha1Mes, out string nameFile);

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
        /// Descargar archivo comprimido de reportes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarZip()
        {
            string strArchivoNombre = Request["file_name"];
            string strArchivoTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte + strArchivoNombre;
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strArchivoNombre);
        }

        #endregion

        #region Notificar a agentes

        public JsonResult NotificarPlazo(int indicador, string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                model.ListaResultadoNotif = pmpo.NotificarPmpo(indicador, base.UserName, fecha1Mes, ruta, out string fileLog);
                model.Resultado = "1";
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

        #region Mensajes de envío

        [HttpPost]
        public JsonResult ListaMensajeXAgente(string mes, int idEmpresa, int idFormato, string tipoRemitente, string estadoMensaje)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                jsModel.ListaMensaje = pmpo.ListarMensajeXAgente(idEmpresa, idFormato, fecha1Mes, tipoRemitente, estadoMensaje);

                pmpo.MarcarMensajesDeAgenteComoLeido(idEmpresa, idFormato, fecha1Mes, base.UserName);

                jsModel.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="sModulo">Nombre del modulo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult DescargarArchivoXMensaje(string fileName, int codigo)
        {
            byte[] buffer = pmpo.GetBufferArchivoMensajeFinal(codigo, fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public JsonResult ObtenerNroMsjPendiente(string mes, int idEmpresa, int idFormato)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                jsModel.NumMsjPendiente = pmpo.GetNumMsjPendiente(idEmpresa, idFormato, fecha1Mes);

                jsModel.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GuardarMensaje(string mes, int idEmpresa, int idFormato, string text, string files)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<PmpoFile> arrFiles = new List<PmpoFile>();
                if (!string.IsNullOrEmpty(files)) arrFiles = JsonConvert.DeserializeObject<List<PmpoFile>>(files);

                //guardar mensaje
                MeMensajeDTO regTextoEnvio = new MeMensajeDTO();
                regTextoEnvio.Msjestado = "P";
                regTextoEnvio.Msjfeccreacion = DateTime.Now;
                if ((text ?? "").Length > 600) text = text.Substring(0, 600);
                regTextoEnvio.Msjdescripcion = text;
                regTextoEnvio.Msjfecperiodo = fecha1Mes;
                regTextoEnvio.Msjusucreacion = base.UserName;
                regTextoEnvio.Emprcodi = idEmpresa;
                regTextoEnvio.Formatcodi = idFormato;

                int idMsj = pmpo.GuardarMensaje(regTextoEnvio, arrFiles);

                jsModel.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = "No se pudo grabar el comentario";
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Adjunta archivo
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult FileAttachment(FormCollection collection)
        {
            RemisionModel jsModel = new RemisionModel();

            List<PmpoFile> files = new List<PmpoFile>();

            if (Request.Files != null && Request.Files.Count > 0)
            {
                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                string tmpFileFullName = rutaUpload + Request.Params["name"];

                for (int i = 0, j = Request.Files.Count; i < j; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];

                    file.SaveAs(tmpFileFullName);

                    files.Add(new PmpoFile()
                    {
                        TmpFileName = Path.GetFileName(tmpFileFullName),
                        FileName = file.FileName
                    });
                }
            }

            return Json(jsModel);
        }

        /// <summary>
        /// Listado de mensajes exportado en pdf
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="tipoRemitente"></param>
        /// <param name="estadoMensaje"></param>
        /// <returns></returns>
        public FileResult DownloadFilePdfListadoMensaje(string mes, int idEmpresa, int idFormato, string tipoRemitente, string estadoMensaje)
        {
            DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            string logo = @"logocoes.png";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + @"Content\Images\";

            //pdf
            byte[] buffer = pmpo.PdfReporteListadoMensaje(idEmpresa, idFormato, fecha1Mes, tipoRemitente, estadoMensaje, ruta, logo, out string filename);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Pdf, filename);
        }

        #endregion

        #region FormatoController

        /// <summary>
        /// Vista Index
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult IndexEnvio(int idEnvio, int tipoinfocodi)
        {
            RemisionModel model = new RemisionModel();
            model.IdHoja = 0; //hoja principal
            model.IdEnvio = idEnvio; //no existe envio
            model.Tipoinfocodi = tipoinfocodi; //MW

            MeEnvioDTO regEnvio = servFormato.GetByIdMeEnvio(idEnvio);
            model.Mes = regEnvio.Enviofechaperiodo.Value.ToString(ConstantesAppServicio.FormatoMes);
            model.ListaEmpresa = new List<SiEmpresaDTO>() { servFormato.GetByIdSiEmpresa(regEnvio.Emprcodi.Value) };
            model.ListaFormato = new List<MeFormatoDTO>() { servFormato.GetByIdMeFormato(regEnvio.Formatcodi.Value) };
            foreach (var reg in model.ListaFormato)
            {
                reg.Formatnombre = reg.Formatnombre.Replace("PMPO_", "");
            }

            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            return PartialView(model);
        }

        /// <summary>
        /// Vista Index
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult IndexValidacion(int idEnvio, int tipoinfocodi)
        {
            RemisionModel model = new RemisionModel();
            model.IdHoja = 0; //hoja principal
            model.IdEnvio = idEnvio; //no existe envio
            model.Tipoinfocodi = tipoinfocodi; //MW

            MeEnvioDTO regEnvio = servFormato.GetByIdMeEnvio(idEnvio);
            model.Mes = regEnvio.Enviofechaperiodo.Value.ToString(ConstantesAppServicio.FormatoMes);
            model.ListaEmpresa = new List<SiEmpresaDTO>() { servFormato.GetByIdSiEmpresa(regEnvio.Emprcodi.Value) };
            model.ListaFormato = new List<MeFormatoDTO>() { servFormato.GetByIdMeFormato(regEnvio.Formatcodi.Value) };
            foreach (var reg in model.ListaFormato)
            {
                reg.Formatnombre = reg.Formatnombre.Replace("(DERIVACION INTRANET) ", "");
                reg.Formatnombre = reg.Formatnombre.Replace("PMPO_", "");
            }

            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            return PartialView(model);
        }

        /// <summary>
        /// Hojas segun el formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public JsonResult CargarFormatoXEmpresa(int emprcodi)
        {
            RemisionModel model = new RemisionModel();
            model.ListaFormato = ListarFormatoXEmpresa(emprcodi);

            return Json(model);
        }

        /// <summary>
        /// Hojas segun el formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public JsonResult CargarFormato(int formatcodi)
        {
            RemisionModel model = this.GenerarValoresDefecto(formatcodi);
            return Json(model);
        }

        /// <summary>
        /// Generar valor por defecto
        /// </summary>
        /// <param name="model"></param>
        private RemisionModel GenerarValoresDefecto(int formatcodi)
        {
            RemisionModel model = new RemisionModel();

            //Hojas
            model.ListaMeHoja = servFormato.GetByCriteriaMeHoja(formatcodi);
            model.ListaMeHojaPadre = servFormato.ListHojaPadre(formatcodi);

            return model;
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Demanda diaria
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string mes, int idFormato, int tipoinfocodi, int verUltimoEnvio, int? idHoja = 0)
        {
            FormatoModel jsModel = new FormatoModel();
            try
            {
                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                if (idEnvio >= 0)
                {
                    //obtener de la base de datos
                    jsModel = pmpo.GetFormatoModelEnvioPmpo(idEnvio, fecha1Mes, idEmpresa, idFormato, tipoinfocodi, verUltimoEnvio == 1);
                }
                else
                {
                    //obtener del archivo importado
                    jsModel = pmpo.GetFormatoModelEnvioPmpo(0, fecha1Mes, idEmpresa, idFormato, tipoinfocodi, false);
                    pmpo.ActualizarFormatoMainFromExcel(jsModel, this.ListaHoja, this.ListaMatrizExcel);
                }

                foreach (var reg in jsModel.ListaFormatoModel)
                {
                    reg.ListaBloque = null;
                    reg.ListaMxInt = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.ToString();
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite generar el formato en formato excel de Demanda diaria
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEnvio, string mes, int idEmpresa, int idFormato, int tipoinfocodi, List<int> listaHoja, List<string[][]> listaData, int? idHoja = 0)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                FormatoModel model = pmpo.GetFormatoModelEnvioPmpo(idEnvio, fecha1Mes, idEmpresa, idFormato, tipoinfocodi, false);
                pmpo.ActualizarFormatoMain(model, listaHoja, listaData, false);

                pmpo.GenerarExcelEnvioPmpo(rutaBase, model, out string fileExcel);

                jsModel.Resultado = fileExcel;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.ToString();
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(string mes, int idFormato, int idEmpresa, int tipoinfocodi, int? idHoja = 0)
        {
            RemisionModel jsModel = new RemisionModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (pmpo.ValidarArchivoEnvio(this.NombreFile, idEmpresa, idFormato, tipoinfocodi, fecha1Mes, out List<string> listaMsj))
                {
                    this.ListaHoja = new List<int>();
                    this.ListaMatrizExcel = new List<string[][]>();

                    pmpo.LeerArchivoExcelEnvio(this.NombreFile, idEmpresa, idFormato, tipoinfocodi, fecha1Mes, out List<int> listaHoja, out List<string[][]> listaData);
                    this.ListaHoja = listaHoja;
                    this.ListaMatrizExcel = listaData;
                }

                FileInfo fileInfo = new FileInfo(this.NombreFile);
                if (fileInfo.Exists)
                {
                    ToolsFormato.BorrarArchivo(this.NombreFile);
                }

                if (listaMsj.Any())
                {
                    jsModel.Resultado = "-1";
                    jsModel.Mensaje = string.Join(" ", listaMsj);
                }
                else { jsModel.Resultado = "1"; }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            return Json(jsModel);
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Demanda Diaria
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string mes, int idEmpresa, int idFormato, int tipoinfocodi, List<int> listaHoja, List<string[][]> listaData
                                        , string text, string files, int? idHoja = 0)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<PmpoFile> arrFiles = new List<PmpoFile>();
                if (!string.IsNullOrEmpty(files)) arrFiles = JsonConvert.DeserializeObject<List<PmpoFile>>(files);

                FormatoModel model = pmpo.GetFormatoModelEnvioPmpo(0, fecha1Mes, idEmpresa, idFormato, tipoinfocodi, false);
                pmpo.ActualizarFormatoMain(model, listaHoja, listaData, true);

                //Obtener data requerida
                int dataRequerida = pmpo.ObtenerTotalCeldasDataRequerida(model);
                int dataEditada = pmpo.ObtenerTotalCeldasDataEditada(model, idEmpresa, idFormato, fecha1Mes, out MeEnvioDTO objEnvioExtranet);

                text = "ENVIO DE MENSAJE: " + (text ?? "");

                int idEnvioNuevo = pmpo.GrabarExcelWeb(model, base.UserName, text, arrFiles);

                //ACTUALIZAR ME_VALIDACIÓN
                pmpo.ActualizarValidacionCumplimiento(idFormato, idEmpresa, fecha1Mes, dataEditada, dataRequerida, objEnvioExtranet, base.UserName);

                jsModel.Resultado = idEnvioNuevo.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = "No se procesó la información. " + ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato(string archivo)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
            string fullPath = ruta + archivo;

            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, archivo);
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesFormato.ExtensionFile;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Importación masiva

        /// <summary>
        /// UploadPropiedad
        /// </summary>
        /// <param name="sFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadValidacion()
        {
            try
            {
                base.ValidarSesionUsuario();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte;

                if (Request.Files != null)
                {
                    int totalFiles = Request.Files.Count;
                    for (var i = 0; i < totalFiles; i++)
                    {
                        var file = Request.Files[i];
                        string sNombreArchivo = "IMP_" + file.FileName;

                        file.SaveAs(path + sNombreArchivo);
                    }
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
        [HttpPost]
        public JsonResult MostrarArchivoImportacion(string opcion)
        {
            if (opcion == "E")
                pmpo.EliminarArchivosReporteFormatos();

            RemisionModel model = new RemisionModel();
            model.ListaDocumentos = pmpo.ListarDocumentosArchivoReporteFormatos(AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte);

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

            RemisionModel modelArchivos = new RemisionModel();
            modelArchivos.ListaDocumentos = pmpo.ListarDocumentosArchivoReporteFormatos(AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte);
            foreach (var item in modelArchivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(null, nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte))
            {
                FileServer.DeleteBlob(nombreFile, AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte);
            }

            return -1;
        }

        /// <summary>
        /// Procesamiento de archivos
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarValidacionFormatosExcel(string fechaProceso)
        {
            RemisionModel model = new RemisionModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaProces = DateTime.ParseExact(fechaProceso, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                //Si la generacion es diferente al 100% nos devolvera valor 0 y no permitira el registro
                var estado = pmpo.NoExisteProcesoValidacionMasivaEnProceso();
                model.Resultado = estado.ToString();

                if (estado == 1) //si no existe proceso ejecutandose
                {
                    DateTime fechaInicioProceso = DateTime.Now;

                    List<LogErrorFormato> listaLogFilas = new List<LogErrorFormato>();
                    List<int> listaFormatos = new List<int>();
                    List<ValidacionFormato> listaDataFormato = new List<ValidacionFormato>();

                    //Obtener documentos
                    model.ListaDocumentos = pmpo.ListarDocumentosArchivoReporteFormatos(AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte);

                    foreach (var archivo in model.ListaDocumentos)
                    {
                        // Validar datos de Excel y realiza la importacion de los registros de este archivo
                        string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderReporte + archivo.FileName;
                        if (pmpo.ValidarArchivoFormatoProcesoMasivo(path, out int idFormato, fechaProces, out List<PmpoBloqueHorario> bloques,
                                                        out List<MePtomedicionDTO> listaGlobalPuntos, ref listaLogFilas))
                        {
                            idFormato = pmpo.GetFormatcodiIntranet(idFormato); //el formato extranet del excel pasarlo a intranet
                            listaFormatos.Add(idFormato);

                            //Leer data Excel para validación
                            pmpo.LeerCeldasProcesoMasivo(path, idFormato, fechaProces, bloques, listaGlobalPuntos, ref listaLogFilas, out ValidacionFormato DataFormato);
                            listaDataFormato.Add(DataFormato);

                            // INICIA VALIDACIÓN Y LOG
                            pmpo.ValidarCeldasProcesoMasivo(DataFormato, fechaProces, idFormato, bloques, listaGlobalPuntos, ref listaLogFilas);
                        }
                    }

                    if (!listaDataFormato.Any() || !listaFormatos.Any())
                        listaLogFilas.Add(new LogErrorFormato() { Observacion = "No existen archivos válidos a procesar." });

                    //validación si existen errores en la data
                    if (listaLogFilas.Any())
                    {
                        model.Resultado = "-5";// error log

                        //DIBUJAR EXCELL LOG
                        string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                        string ruta = pmpo.GenerarExcelLogErrores(pathLogo, DateTime.Now, base.UserName, listaLogFilas);
                        model.PathLog = ruta;
                        model.NameFileLog = "LogErrores_Validacion_" + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaYMD) + "_" + DateTime.Now.ToString(ConstantesAppServicio.FormatoHHmmss).Replace(":", "");
                        model.Mensaje = "¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.";
                    }
                    else
                    {
                        //Ejecución del guardado masivo
                        string usuario = base.UserName;
                        int enviocodi = pmpo.GetNuevoEnvioValidacion(fechaProces, fechaInicioProceso, usuario);
                        HostingEnvironment.QueueBackgroundWorkItem(token => SaveSDDPSegundoPlano(enviocodi, fechaProces, listaFormatos, listaDataFormato, usuario, token));
                    }
                }
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// ejecución en segundo plano
        /// </summary>
        /// <param name="enviocodi"></param>
        /// <param name="fechaProces"></param>
        /// <param name="listaFormatos"></param>
        /// <param name="listaDataFormato"></param>
        /// <param name="usuario"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SaveSDDPSegundoPlano(int enviocodi, DateTime fechaProces, List<int> listaFormatos, List<ValidacionFormato> listaDataFormato, string usuario, CancellationToken cancellationToken)
        {
            try
            {
                //Segundo plano Generacion de version
                pmpo.RealizarProcesoMasivoValidacion(enviocodi, fechaProces, listaFormatos, listaDataFormato, usuario);

                pmpo.EliminarArchivosReporteFormatos();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);

                //si hay error en la ejecucion entonces actualizar estado del proceso y agregarlo al log
                MeEnvioDTO reg = pmpo.GetByIdMeEnvio(enviocodi);
                reg.Enviodesc = "-1";
                pmpo.UpdateMeEnvioDesc(reg);

                //mensajes de error
                pmpo.AgregarLog(enviocodi, "Ejecución en segundo plano. " + ex.ToString(), ConstantesPMPO.TipoEventoLogError, "SISTEMA");
            }
        }

        /// <summary>
        /// Lista detalle de proceso
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaLogxMes(string mes)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                model.Resultado = pmpo.NoExisteProcesoValidacionMasivaEnProceso().ToString();
                model.ListaLog = pmpo.ListarLogValidacionXMes(fecha1Mes, out MeEnvioDTO version);
                model.Envio = version;
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
        /// Descarga de archivo log de errores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelLog()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region Ingresar comentario Osinergmin

        [HttpPost]
        public JsonResult GuardarComentarioOsinergmin(string mes, int idEmpresa, int idFormato, string text)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                //guardar mensaje
                MeValidacionDTO regVal = new MeValidacionDTO();
                regVal.Emprcodi = idEmpresa;
                regVal.Formatcodi = idFormato;
                regVal.Validfechaperiodo = fecha1Mes;
                regVal.Validcomentario = text;
                regVal.Validestado = 1;
                regVal.Validusumodificacion = base.UserName;
                regVal.Validfecmodificacion = DateTime.Now;

                pmpo.GuardarComentarioOsinergmin(regVal);

                jsModel.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = "No se pudo grabar el comentario";
                jsModel.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}
