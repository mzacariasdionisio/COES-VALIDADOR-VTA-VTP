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
            //List<VteVersionDTO> lstVersiones = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions("2025.Marzo", "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

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
        public ActionResult CargarReporteConsolidadoHtml(string periodo, string version, int inicializar)
        {
            try
            {
                // 1. Obtener todos los datos necesarios, igual que antes
                var listaBarras = new List<COES.Dominio.DTO.Transferencias.BarraDTO>();
                var model =new ValidadorVTPEntradaModel();
                model.ListBarrasBrg = listaBarras;
                model.ListBarrasNoBrg = listaBarras;               

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

                model.StrMensaje = "NOTA: Se realizó la evaluación el " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ".";

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
    }
}
