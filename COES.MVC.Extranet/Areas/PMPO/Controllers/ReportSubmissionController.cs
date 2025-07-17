using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.PMPO.Helper;
using COES.MVC.Extranet.Areas.PMPO.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.PMPO.Controllers
{
    /// <summary>
    /// Clase controladora del envio de información para PMPO
    /// </summary>
    public class ReportSubmissionController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();
        readonly FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #region Variables session

        public string Empresas
        {
            get
            {
                return (Session[ConstantesPMPO.SesionEmpresas] != null) ?
                   Session[ConstantesPMPO.SesionEmpresas].ToString() : "0";
            }
            set { Session[ConstantesPMPO.SesionEmpresas] = value; }
        }

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

        /// <summary>
        /// Vista Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            RemisionModel model = new RemisionModel();
            model.IdHoja = 0; //hoja principal
            model.IdEnvio = 0; //no existe envio
            model.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW; //MW

            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);

            model.ListaEmpresa = ListarEmpresaAgente();
            if (model.ListaEmpresa.Any())
            {
                model.ListaFormato = ListarFormatoXEmpresa(model.ListaEmpresa.First().Emprcodi);
            }
            this.Empresas = string.Join(",", (model.ListaEmpresa.Select(x => x.Emprcodi).Distinct().ToList()));

            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            return View(model);
        }

        /// <summary>
        /// Listar empresas permitidas segun usuario
        /// </summary>
        /// <returns></returns>
        private List<SiEmpresaDTO> ListarEmpresaAgente()
        {
            var listaEmpAll = pmpo.GetListaEmpresasPMPO();

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            var listaEmpresas = new List<SiEmpresaDTO>();
            if (permisoEmpresas)
            {
                listaEmpresas = listaEmpAll;
            }
            else
            {
                var lstEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(base.UserName).ToList();
                foreach (var reg in listaEmpAll)
                {
                    var find = lstEmpresa.Find(x => x.EMPRCODI == (short)reg.Emprcodi);
                    if (find != null)
                    {
                        listaEmpresas.Add(reg);
                    }
                }
            }

            if (!listaEmpresas.Any())
            {
                listaEmpresas.Add(new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No existe" });
            }

            return listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
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

        #region FormatoController

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

                text = "ENVIO DE MENSAJE: " + (text ?? "");

                int idEnvioNuevo = pmpo.GrabarExcelWeb(model, base.UserEmail, text, arrFiles);

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
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Recordatorio y Mensajes de envío

        [HttpPost]
        public JsonResult ListaRecordatorio(string mes, string tipoRemitente)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                jsModel.ListaMensaje = pmpo.ListarMensajeRecordatorio(this.Empresas, fecha1Mes, tipoRemitente);

                pmpo.MarcarRecordatorioDeCOESComoLeido(this.Empresas, fecha1Mes, base.UserEmail);

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
        public JsonResult ListaMensajeXAgente(string mes, int idEmpresa, int idFormato, string tipoRemitente, string estadoMensaje)
        {
            RemisionModel jsModel = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mes, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                jsModel.ListaMensaje = pmpo.ListarMensajeXAgente(idEmpresa, idFormato, fecha1Mes, tipoRemitente, estadoMensaje);

                pmpo.MarcarMensajesDeCOESComoLeido(idEmpresa, idFormato, fecha1Mes, base.UserEmail);

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
                regTextoEnvio.Msjusucreacion = base.UserEmail;
                regTextoEnvio.Emprcodi = idEmpresa;
                regTextoEnvio.Formatcodi = idFormato;

                int idMsj = pmpo.GuardarMensaje(regTextoEnvio, arrFiles);

                //guardar en el fileServer

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
                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesPMPO.FolderUpload;

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
    }
}