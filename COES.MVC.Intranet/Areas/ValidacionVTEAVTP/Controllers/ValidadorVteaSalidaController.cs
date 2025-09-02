using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using log4net;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers
{
    public class ValidadorVteaSalidaController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        readonly ValidacionVteavtpAppServicio validacionVteavtpAppServicio = new ValidacionVteavtpAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ValidadorVteaSalidaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public const string ErrorInterno = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
        public ValidadorVteaSalidaController()
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
                throw new InvalidOperationException($"Error crítico en el controlador {NameController}. Consulte el log para más detalles.", ex);
            }
        }

        public async Task<ActionResult> Index()
        {
            ValidadorVteaSalidaModel model = new ValidadorVteaSalidaModel();
            model.PeriodoValorizacion = new TrnPeriodoDTO();
            model.VersionesVtea = new VteaVersionDTO();

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                TrnPeriodoDTO periodo = await validacionVteavtpAppServicio.ObtenerSmeTrnPeriodo(rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                if (periodo.Resultado == 0)
                {
                    model.PeriodoValorizacion = periodo;

                    var primerPeriodo = periodo.Periodos.FirstOrDefault();

                    VteaVersionDTO versionesVtea = await validacionVteavtpAppServicio.ObtenerSmeVteaVersions(primerPeriodo.PeriNombre, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                    if (versionesVtea.Resultado == 0)
                    {
                        model.VersionesVtea = versionesVtea;
                    }
                    else if (versionesVtea.Resultado == -1)
                    {
                        model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                        log.Error(versionesVtea.Mensaje);
                        model.StrMensajeError = ErrorInterno;
                    }
                    else if (versionesVtea.Resultado == 1)
                    {
                        model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                        model.StrMensajeError = versionesVtea.Mensaje;
                    }
                }
                else if (periodo.Resultado == -1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    log.Error(periodo.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (periodo.Resultado == 1)
                {
                    model.PeriodoValorizacion.Periodos = new List<TablePeriodoDTO>();
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

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

        public async Task<ActionResult> ObtenerVersiones(string periodo)
        {
            ValidadorVtpvteaModel model = new ValidadorVtpvteaModel();           
            model.VersionesVtea = new VteaVersionDTO();
            model.StrMensajeError = "";

            try
            {
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                                

                VteaVersionDTO versionesVtea = await validacionVteavtpAppServicio.ObtenerSmeVteaVersions(periodo, "", rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);
                              
                if (versionesVtea.Resultado == 0)
                {
                    model.VersionesVtea = versionesVtea;
                }
                else if (versionesVtea.Resultado == -1)
                {
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    log.Error(versionesVtea.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (versionesVtea.Resultado == 1)
                {
                    model.VersionesVtea.Versiones = new List<TableVersionVteaDTO>();

                    model.StrMensajeError = versionesVtea.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }


            return Json(model, JsonRequestBehavior.AllowGet);
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

        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> CargarReporteConsolidadoHtml(string periodo, string version, int esInicio)
        {
            var model = new ValidadorVteaSalidaModel();
            model.StrMensaje = "";
            model.StrMensajeError = "";           

            try
            {
                model.DatosVTEA = new VteaDTO();
                model.DatosVTEA.TableEH = new List<TableEH>();
                model.DatosVTEA.TableHE = new List<TableHE>();
                model.DatosVTEA.TableFC = new List<TableFC>();
                model.DatosVTEA.RetirosNegativos = new List<RetirosNegativos>();               

                if (esInicio == 0)
                {
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderValidacion, "");
                    FileServer.CreateFolder(base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog, "");

                    string rutaUpload = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                    var datosSalidaVTEA = await validacionVteavtpAppServicio.FuncionVtea(periodo, version, rutaUpload, base.PathFiles, Helper.ConstantesValidacionVteavtp.FolderLog);

                    if (datosSalidaVTEA.Resultado == 0)
                    {
                        model.DatosVTEA = datosSalidaVTEA;
                      
                    }
                    else
                    {
                        if (datosSalidaVTEA.Resultado == -1)
                        {
                            model.StrMensajeError = ErrorInterno;

                            log.Error(model.StrMensajeError);

                        }
                        else if (datosSalidaVTEA.Resultado == 1)
                        {
                            model.StrMensajeError = datosSalidaVTEA.Mensaje;
                        }
                    }
                }


                string rutaBaseVista = $"~/Areas/ValidacionVTEAVTP/Views/ValidadorVteaSalida/";

                string htmlBarrasBrg = RenderViewToString($"{rutaBaseVista}ListaRetirosNegativos.cshtml", model);
                string htmlBarrasNoBrg = RenderViewToString($"{rutaBaseVista}ListaSinDeclaracion.cshtml", model);
                string htmlBarrasSinAnalizar = RenderViewToString($"{rutaBaseVista}ListaDeclaracionesNuevas.cshtml", model);
                string htmlBarrasDiferencia = RenderViewToString($"{rutaBaseVista}ListaFinContrato.cshtml", model);

                model.VistaBarrasBrg = htmlBarrasBrg;
                model.VistaBarrasNoBrg = htmlBarrasNoBrg;
                model.VistaBarrasSinAnalizar = htmlBarrasSinAnalizar;
                model.VistaBarrasDiferencia = htmlBarrasDiferencia;

                var fecha = DateTime.Now;
                model.StrMensaje = esInicio > 0 ? "NOTA: Dar clic en \"Procesar\" para realizar la evaluación." :
                    string.Format("NOTA: Se realizó la evaluación el {0} a las {1}.", fecha.ToString("dd/MM/yyyy"), fecha.ToString("HH:mm:ss"));

                Session[Helper.ConstantesValidacionVteavtp.D_Datos_Salida_VTEA] = model.DatosVTEA;

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReporte(string periodo, string version)
        {
            base.ValidarSesionUsuario();


            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";

            var datosVTEA = new VteaDTO();

            if (Session[Helper.ConstantesValidacionVteavtp.D_Datos_Salida_VTEA] != null)
            {
                var datosinicio = (ValidadorVteaSalidaModel)Session[Helper.ConstantesValidacionVteavtp.D_Datos_Salida_VTEA];

                datosVTEA.TableEH = datosinicio.DatosVTEA.TableEH;
                datosVTEA.TableHE = datosinicio.DatosVTEA.TableHE;
                datosVTEA.TableFC = datosinicio.DatosVTEA.TableFC;
                datosVTEA.RetirosNegativos = datosinicio.DatosVTEA.RetirosNegativos;
            }

            try
            {
                nombreArchivo = Helper.ExcelDocumentVteaSalida.GenerarReporte(datosVTEA, periodo, version, rutaLogo);
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
