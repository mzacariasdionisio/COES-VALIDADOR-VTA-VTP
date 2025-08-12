using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

using log4net;
using COES.MVC.Intranet.Controllers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System.Reflection;
using COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models;
using COES.Framework.Base.Tools;
using System.Threading.Tasks;
using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using WebGrease.Activities;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers
{
    public class ValidadorVTPEntradaController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        ValidacionVTEAVTPAppServicio validacionVTEAVTPAppServicio = new ValidacionVTEAVTPAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PruebaServicioController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public ValidadorVTPEntradaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        public async Task<ActionResult> Index()
        {
            ValidadorVTPEntradaModel model = new ValidadorVTPEntradaModel();
            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

            string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

            List<TrnPeriodoDTO> lstPeriodo = await validacionVTEAVTPAppServicio.ObtenerSmeTrnPeriodo(rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);
            
            var primerPeriodo = lstPeriodo.FirstOrDefault();

            List<VteVersionDTO> lstVersiones = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(primerPeriodo.PeriNombre, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);
            
            model.ListPeriodos = lstPeriodo;
            model.ListVersiones = lstVersiones;

            return View(model);
        }


        
        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        public async Task<ActionResult> CargarReporteConsolidadoHtml(string periodo, string version, int inicializar)
        {
            var model = new ValidadorVTPEntradaModel();
            model.StrMensaje = "";
            model.StrMensajeError = "";

            try
            {
                if(inicializar > 0)
                {
                    model.DatosVTP = new VtpDTO();
                    model.DatosVTP.TableVtpBrg = new List<TableVtpBrgResultDTO>();
                    model.DatosVTP.TableVtpNoBrg = new List<TablaVtpNoBrgResultDTO>();
                    model.DatosVTP.TableAnas = new List<TablaAnaResultDTO>();
                    model.DatosVTP.TableVtpSinAnalizar = new List<TablaVtpSinAnalizarResultDTO>();
                    model.EmpresasBarra = new List<string>();
                }
                else
                {
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

                    string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                    var datosEntradaVTP = await validacionVTEAVTPAppServicio.FuncionVtp(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    model.DatosVTP = datosEntradaVTP;

                    var empresas = datosEntradaVTP.TableVtpBrg
                                    .Select(p => p.Empresa)
                                    .Concat(datosEntradaVTP.TableVtpNoBrg.Select(p => p.Empresa))
                                    .Distinct()
                                    .OrderBy(p => p).ToList();

                    model.EmpresasBarra = empresas;

                }

                // 1. Obtener todos los datos necesarios, igual que antes
                     

                // 2. Renderizar cada vista parcial a un string de HTML usando el método auxiliar
                string rutaBaseVista = $"~/Areas/ValidacionVTEAVTP/Views/ValidadorVTPEntrada/";

                string htmlBarrasBrg = RenderViewToString($"{rutaBaseVista}ListaBarrasBrg.cshtml", model);
                string htmlBarrasNoBrg = RenderViewToString($"{rutaBaseVista}ListaBarrasNoBrg.cshtml", model);
                string htmlBarrasSinAnalizar = RenderViewToString($"{rutaBaseVista}ListaBarrasSinAnalizar.cshtml", model);
                string htmlBarrasDiferencia = RenderViewToString($"{rutaBaseVista}ListaBarrasDiferencia.cshtml", model);

                // 3. Crear un objeto anónimo (o un DTO) para empaquetar los strings de HTML
                model.VistaBarrasBrg = htmlBarrasBrg;
                model.VistaBarrasNoBrg = htmlBarrasNoBrg;
                model.VistaBarrasSinAnalizar = htmlBarrasSinAnalizar;
                model.VistaBarrasDiferencia = htmlBarrasDiferencia;

                var fecha = DateTime.Now;
                model.StrMensaje = inicializar > 0 ? "NOTA: Dar clic en \"Procesar\" para realizar la evaluación." : 
                    string.Format("NOTA: Se realizó la evaluación el {0} a las {1}.", fecha.ToString("dd/MM/yyyy"), fecha.ToString("hh:mm:ss"));

                // 4. Devolver este objeto como JSON
                return Json(model, JsonRequestBehavior.AllowGet);
            }
         

            catch (System.Exception ex)
            {
                // Si hay un error, lo devolvemos como un error 500 y el mensaje
                // será visible en la consola de herramientas de desarrollador del navegador (F12).
                Response.StatusCode = 500;
                return Content(ex.Message, "text/plain");
            }
        }

        public async Task<ActionResult> ObtenerVersiones(string periodo)
        {
            ValidadorVTPEntradaModel model = new ValidadorVTPEntradaModel();
            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

            string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;       

            List<VteVersionDTO> lstVersiones = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(periodo, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);
            
            model.ListVersiones = lstVersiones;
            model.StrMensajeError = "0";

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GenerarReporteSeccion(string periodo, string version, string seccion)
        {
            base.ValidarSesionUsuario();
            
            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";

            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
            FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

            string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

            switch (seccion)
            {
                case "Barras":                   

                    var datosEntradaVTP = await validacionVTEAVTPAppServicio.FuncionVtp(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    nombreArchivo = Helper.ExcelDocument.GenerarReporteBarras(datosEntradaVTP, periodo, version, rutaLogo);
                   
                    break;

                case "BarrasSinAnalizar":                   

                    var datosBarraSinAnalizarVTP = await validacionVTEAVTPAppServicio.FuncionVtp(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    nombreArchivo = Helper.ExcelDocument.GenerarReporteBarrasSinAnalizar(datosBarraSinAnalizarVTP, periodo, version, rutaLogo);

                    break;

                case "BarrasDiferencia":
                    var datosBarraDiferenciaVTP = await validacionVTEAVTPAppServicio.FuncionVtp(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    nombreArchivo = Helper.ExcelDocument.GenerarReporteBarrasDiferencia(datosBarraDiferenciaVTP, periodo, version, rutaLogo);
                    break;

                default:
                    // Si el tipo de reporte no es válido, no hacer nada o devolver un error.
                    return new HttpNotFoundResult("El tipo de reporte solicitado no es válido.");
            }

            return Json(nombreArchivo);
        }

        public virtual ActionResult DescargarArchivo(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile].ToString() + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }
    }
}
