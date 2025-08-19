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
using System.Configuration;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers
{
    public class ValidadorVTPVTEAController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        ValidacionVTEAVTPAppServicio validacionVTEAVTPAppServicio = new ValidacionVTEAVTPAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PruebaServicioController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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
            ValidadorVTPVTEAModel model = new ValidadorVTPVTEAModel();
            model.PeriodoValorizacion = new TrnPeriodoDTO();
            model.VersionesVtp = new VtpVersionDTO();
            model.VersionesVtea = new VteaVersionDTO();

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                TrnPeriodoDTO periodo = await validacionVTEAVTPAppServicio.ObtenerSmeTrnPeriodo(rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                if (periodo.Resultado == 0)
                {
                    model.PeriodoValorizacion = periodo;

                    var primerPeriodo = periodo.Periodos.FirstOrDefault();

                    VtpVersionDTO versionesVtp = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(primerPeriodo.PeriNombre, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    VteaVersionDTO versionesVtea = await validacionVTEAVTPAppServicio.ObtenerSmeVteaVersions(primerPeriodo.PeriNombre, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    if (versionesVtp.Resultado == 0)
                    {
                        model.VersionesVtp = versionesVtp;
                    }
                    else if (versionesVtp.Resultado == -1)
                    {
                        model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                        log.Error(versionesVtp.Mensaje);
                        model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
                    }
                    else if (versionesVtp.Resultado == 1)
                    {
                        model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                        model.StrMensajeError = versionesVtp.Mensaje;
                    }

                    if (versionesVtea.Resultado == 0)
                    {
                        model.VersionesVtea = versionesVtea;
                    }
                    else if (versionesVtp.Resultado == -1)
                    {
                        model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                        log.Error(versionesVtp.Mensaje);
                        model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
                    }
                    else if (versionesVtp.Resultado == 1)
                    {
                        model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                        model.StrMensajeError = versionesVtp.Mensaje;
                    }

                }
                else if (periodo.Resultado == -1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    log.Error(periodo.Mensaje);
                    model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
                }
                else if (periodo.Resultado == 1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    model.StrMensajeError = periodo.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
            }

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
        public async Task<ActionResult> CargarReporteConsolidadoHtml(string periodo, string versionVTP, string versionVTEA, int inicializar)
        {
            var model = new ValidadorVTPVTEAModel();
            model.StrMensaje = "";
            model.StrMensajeError = "";

            try
            {
                model.VtpVteaDatos = new VtpVteaDTO();
                model.VtpVteaDatos.TablesXY = new List<TableXY>();
                model.VtpVteaDatos.TablesYX = new List<TableYX>();
                model.VtpVteaDatos.TablesD = new List<TableD>();

                if (inicializar == 0)
                {                  
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

                    string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                    var datosSalidaVTP = await validacionVTEAVTPAppServicio.FuncionVtpVtea(periodo, versionVTEA, versionVTP, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                    //model.VtpVteaDatos = datosSalidaVTP;

                    if (datosSalidaVTP.Resultado == 0)
                    {
                        model.VtpVteaDatos = datosSalidaVTP;
                    }
                    else
                    {
                        if (datosSalidaVTP.Resultado == -1)
                        {
                            model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";

                            log.Error(model.StrMensajeError);

                        }
                        else if (datosSalidaVTP.Resultado == 1)
                        {
                            model.StrMensajeError = datosSalidaVTP.Mensaje;
                        }
                    }

                }
               
                string rutaBaseVista = $"~/Areas/ValidacionVTEAVTP/Views/ValidadorVTPVTEA/";

                string htmlBarrasBrg = RenderViewToString($"{rutaBaseVista}ListaComparacionDiferencias.cshtml", model);
                string htmlBarrasNoBrg = RenderViewToString($"{rutaBaseVista}ListaComparacionVTEA.cshtml", model);
                string htmlBarrasSinAnalizar = RenderViewToString($"{rutaBaseVista}ListaComparacionVTP.cshtml", model);              
                               
                model.VistaComparacionDiferencia = htmlBarrasBrg;
                model.VistaComparacionVTEA = htmlBarrasNoBrg;
                model.VistaComparacionVTP = htmlBarrasSinAnalizar;

                var fecha = DateTime.Now;
                model.StrMensaje = inicializar > 0 ? "NOTA: Dar clic en \"Procesar\" para realizar la evaluación" :
                     string.Format("NOTA: Se realizó la evaluación el {0} a las {1}.", fecha.ToString("dd/MM/yyyy"), fecha.ToString("hh:mm:ss"));

                Session[Helper.ConstantesValidacionVTEAVTP.D_Datos_VTEA_VTP] = model.VtpVteaDatos;

               
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerVersiones(string periodo)
        {
            ValidadorVTPVTEAModel model = new ValidadorVTPVTEAModel();
            model.VersionesVtp = new VtpVersionDTO();
            model.VersionesVtea = new VteaVersionDTO();
            model.StrMensajeError = "";

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                VtpVersionDTO versionesVtp = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(periodo, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                VteaVersionDTO versionesVtea = await validacionVTEAVTPAppServicio.ObtenerSmeVteaVersions(periodo, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVTEAVTP.FolderLog);

                if (versionesVtp.Resultado == 0)
                {
                    model.VersionesVtp = versionesVtp;
                }
                else if (versionesVtp.Resultado == -1)
                {
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    log.Error(versionesVtp.Mensaje);
                    model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
                }
                else if (versionesVtp.Resultado == 1)
                {
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    model.StrMensajeError = versionesVtp.Mensaje;
                }

                if (versionesVtea.Resultado == 0)
                {
                    model.VersionesVtea = versionesVtea;
                }
                else if (versionesVtp.Resultado == -1)
                {
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    log.Error(versionesVtp.Mensaje);
                    model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
                }
                else if (versionesVtp.Resultado == 1)
                {
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    model.StrMensajeError = versionesVtp.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
            }


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerarReporteSeccion(string periodo, string versionVTP, string versionVTEA, string seccion)
        {
            base.ValidarSesionUsuario();

            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";

            var datosVTP = new VtpVteaDTO();

            if (Session[Helper.ConstantesValidacionVTEAVTP.D_Datos_VTEA_VTP] != null)
            {
                datosVTP = (VtpVteaDTO)Session[Helper.ConstantesValidacionVTEAVTP.D_Datos_VTEA_VTP];
            }

            try
            {
                switch (seccion)
                {
                    case "DiferenciaVTPVTEA":
                       
                        nombreArchivo = Helper.ExcelDocument.GenerarReporteDiferenciaVTPVTEA(datosVTP, periodo, versionVTP, versionVTEA, rutaLogo);

                        break;

                    case "ComparacionVTEA":

                        nombreArchivo = Helper.ExcelDocument.GenerarReporteComparacionVTEA(datosVTP, periodo, versionVTP, versionVTEA, rutaLogo);

                        break;

                    case "ComparacionVTP":
                        
                        nombreArchivo = Helper.ExcelDocument.GenerarReporteComparacionVTP(datosVTP, periodo, versionVTP, versionVTEA, rutaLogo);
                        break;

                }
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                nombreArchivo = "-1";
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
