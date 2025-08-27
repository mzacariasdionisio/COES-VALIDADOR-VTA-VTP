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
using WebGrease.Activities;
using System.Configuration;
using Microsoft.Office.Interop.Excel;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers
{
    public class ValidadorVtpEntradaController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        readonly ValidacionVTEAVTPAppServicio validacionVTEAVTPAppServicio = new ValidacionVTEAVTPAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ValidadorVtpvteaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public const string ErrorInterno ="Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
        public ValidadorVtpEntradaController()
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
                throw new ApplicationException($"Error crítico en el controlador {NameController}. Consulte el log para más detalles.", ex);
            }
        }

        public async Task<ActionResult> Index()
        {
            ValidadorVtpEntradaModel model = new ValidadorVtpEntradaModel();
            model.PeriodoValorizacion = new TrnPeriodoDTO();
            model.VersionesVtp = new VtpVersionDTO();

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                TrnPeriodoDTO periodo = await validacionVTEAVTPAppServicio.ObtenerSmeTrnPeriodo(rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                if(periodo.Resultado == 0)
                {
                    model.PeriodoValorizacion = periodo;

                    var primerPeriodo = periodo.Periodos.FirstOrDefault();

                    VtpVersionDTO versionesVtp = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(primerPeriodo.PeriNombre, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                    if (versionesVtp.Resultado == 0)
                    {
                        model.VersionesVtp = versionesVtp;
                    }
                    else if (versionesVtp.Resultado == -1)
                    {
                        model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                        log.Error(versionesVtp.Mensaje);
                        model.StrMensajeError = ErrorInterno;
                    }
                    else if (versionesVtp.Resultado == 1)
                    {
                        model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                        model.StrMensajeError = versionesVtp.Mensaje;
                    }
                }
                else if (periodo.Resultado == -1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    log.Error(periodo.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (periodo.Resultado == 1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    model.StrMensajeError = periodo.Mensaje;
                }
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
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
        public async Task<ActionResult> CargarReporteConsolidadoHtml(string periodo, string version, int esInicio)
        {
            var model = new ValidadorVtpEntradaModel();
            model.StrMensaje = "";
            model.StrMensajeError = "";
            model.StrMensajeErrorApi = "";

            try
            {
                model.DatosVTP = new VtpDTO();
                model.DatosVTP.TableVtpBrg = new List<TableVtpBrgResultDTO>();
                model.DatosVTP.TableVtpNoBrg = new List<TablaVtpNoBrgResultDTO>();
                model.DatosVTP.TableAnas = new List<TablaAnaResultDTO>();
                model.DatosVTP.TableVtpSinAnalizar = new List<TablaVtpSinAnalizarResultDTO>();
                model.EmpresasBarra = new List<string>();

                if (esInicio == 0)               
                {
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                    string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                    var datosEntradaVTP = await validacionVTEAVTPAppServicio.FuncionVtp(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                    if(datosEntradaVTP.Resultado == 0)
                    {
                        model.DatosVTP = datosEntradaVTP;

                        var empresas = datosEntradaVTP.TableVtpBrg
                                        .Select(p => p.Empresa)
                                        .Concat(datosEntradaVTP.TableVtpNoBrg.Select(p => p.Empresa))
                                        .Distinct()
                                        .OrderBy(p => p).ToList();

                        model.EmpresasBarra = empresas;
                    }
                    else
                    {
                        if(datosEntradaVTP.Resultado == -1)
                        {
                            model.StrMensajeError = ErrorInterno;

                            log.Error(model.StrMensajeError);

                        }else if(datosEntradaVTP.Resultado == 1)
                        {
                            model.StrMensajeError = datosEntradaVTP.Mensaje;
                        }                            
                    }
                }                                              

               
                string rutaBaseVista = $"~/Areas/ValidacionVTEAVTP/Views/ValidadorVTPEntrada/";

                string htmlBarrasBrg = RenderViewToString($"{rutaBaseVista}ListaBarrasBrg.cshtml", model);
                string htmlBarrasNoBrg = RenderViewToString($"{rutaBaseVista}ListaBarrasNoBrg.cshtml", model);
                string htmlBarrasSinAnalizar = RenderViewToString($"{rutaBaseVista}ListaBarrasSinAnalizar.cshtml", model);
                string htmlBarrasDiferencia = RenderViewToString($"{rutaBaseVista}ListaBarrasDiferencia.cshtml", model);
               
                model.VistaBarrasBrg = htmlBarrasBrg;
                model.VistaBarrasNoBrg = htmlBarrasNoBrg;
                model.VistaBarrasSinAnalizar = htmlBarrasSinAnalizar;
                model.VistaBarrasDiferencia = htmlBarrasDiferencia;

                var fecha = DateTime.Now;
                model.StrMensaje = esInicio > 0 ? "NOTA: Dar clic en \"Procesar\" para realizar la evaluación." : 
                    string.Format("NOTA: Se realizó la evaluación el {0} a las {1}.", fecha.ToString("dd/MM/yyyy"), fecha.ToString("HH:mm:ss"));

                Session[Helper.ConstantesValidacionVteavtp.D_Datos_Entrada_VTP] = model.DatosVTP;
               
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerVersiones(string periodo)
        {
            ValidadorVtpEntradaModel model = new ValidadorVtpEntradaModel();
            model.VersionesVtp = new VtpVersionDTO();
            model.StrMensajeError = "";

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                VtpVersionDTO versionesVtp = await validacionVTEAVTPAppServicio.ObtenerSmeVtpVersions(periodo, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                if(versionesVtp.Resultado == 0)
                {
                    model.VersionesVtp = versionesVtp;

                } else if(versionesVtp.Resultado == -1)
                {
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    log.Error(versionesVtp.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (versionesVtp.Resultado == 1)
                {
                    model.VersionesVtp.Versiones = new List<TableVersionVtpDTO>();

                    model.StrMensajeError = versionesVtp.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerarReporteSeccion(string periodo, string version, string empresa, string seccion)
        {
            base.ValidarSesionUsuario();
            
            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";          

            var datosVTP = new VtpDTO();

            if(Session[Helper.ConstantesValidacionVteavtp.D_Datos_Entrada_VTP] != null)
            {
                var datosinicio = (VtpDTO)Session[Helper.ConstantesValidacionVteavtp.D_Datos_Entrada_VTP];

                datosVTP.TableVtpBrg = datosinicio.TableVtpBrg;
                datosVTP.TableVtpNoBrg = datosinicio.TableVtpNoBrg;
                datosVTP.TableVtpSinAnalizar = datosinicio.TableVtpSinAnalizar;
                datosVTP.TableAnas = datosinicio.TableAnas;
            }

            try
            {
                switch (seccion)
                {
                    case "Barras":

                        if (!string.IsNullOrEmpty(empresa))
                        {
                            datosVTP.TableVtpBrg = datosVTP.TableVtpBrg.Where(p => p.Empresa == empresa).ToList();
                            datosVTP.TableVtpNoBrg = datosVTP.TableVtpNoBrg.Where(p => p.Empresa == empresa).ToList();
                        }

                        nombreArchivo = Helper.ExcelDocument.GenerarReporteBarras(datosVTP, periodo, version, empresa, rutaLogo);

                        break;

                    case "BarrasSinAnalizar":

                        nombreArchivo = Helper.ExcelDocument.GenerarReporteBarrasSinAnalizar(datosVTP, periodo, version, rutaLogo);

                        break;

                    case "BarrasDiferencia":                       

                        nombreArchivo = Helper.ExcelDocument.GenerarReporteBarrasDiferencia(datosVTP, periodo, version, rutaLogo);

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
