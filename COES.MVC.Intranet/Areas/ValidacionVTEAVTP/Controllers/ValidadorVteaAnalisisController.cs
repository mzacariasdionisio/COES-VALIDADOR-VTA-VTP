using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using log4net;
using Newtonsoft.Json;
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
    public class ValidadorVteaAnalisisController : BaseController
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>

        readonly ValidacionVteavtpAppServicio validacionVteavtpAppServicio = new ValidacionVteavtpAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ValidadorVteaSalidaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public const string ErrorInterno = "Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.";
        public ValidadorVteaAnalisisController()
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
            ValidadorVteaAnalisisModel model = new ValidadorVteaAnalisisModel();
            model.PeriodoValorizacion = new TrnPeriodoDTO();
            model.VersionesVtea = new VteaVersionDTO();

            try
            {

                TrnPeriodoDTO periodo = await validacionVteavtpAppServicio.ObtenerSmeTrnPeriodo();

                if (periodo.Resultado == 0)
                {
                    model.PeriodoValorizacion = periodo;

                    var primerPeriodo = periodo.Periodos.FirstOrDefault();

                    VteaVersionDTO versionesVtea = await validacionVteavtpAppServicio.ObtenerSmeVteaVersions(primerPeriodo.PeriNombre, "");

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
            ValidadorVteaAnalisisModel model = new ValidadorVteaAnalisisModel();
            model.VersionesVtea = new VteaVersionDTO();
            model.StrMensajeError = "";

            try
            {

                VteaVersionDTO versionesVtea = await validacionVteavtpAppServicio.ObtenerSmeVteaVersions(periodo, "");

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
            var model = new ValidadorVteaAnalisisModel();
            model.StrMensaje = "";
            model.StrMensajeError = "";

            try
            {
                model.DatosValidadorVTEA = new VteaValidadorDTO();
                model.DatosValidadorVTEA.InfoEmpresaResumen = new List<InfoEmpresaResumen>();
                model.DatosValidadorVTEA.InfoDeclaracionResumen = new List<InfoDeclaracionResumen>();
                //model.DatosValidadorVTEA.TableFC = new List<TableFC>();
                //model.DatosValidadorVTEA.RetirosNegativos = new List<RetirosNegativos>();

                if (esInicio == 0)
                {
                    var datosSalidaVTEA = await validacionVteavtpAppServicio.FuncionVteaValidador(periodo, version);

                    if (datosSalidaVTEA.Resultado == 0)
                    {
                        model.DatosValidadorVTEA = datosSalidaVTEA;

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


                string rutaBaseVista = $"~/Areas/ValidacionVTEAVTP/Views/ValidadorVteaAnalisis/";

                string htmlBarrasBrg = RenderViewToString($"{rutaBaseVista}ListaRolEmpresa.cshtml", model);
                string htmlBarrasNoBrg = RenderViewToString($"{rutaBaseVista}ListaEnergia.cshtml", model);              

                model.VistaBarrasBrg = htmlBarrasBrg;
                model.VistaBarrasNoBrg = htmlBarrasNoBrg;               

                var fecha = DateTime.Now;
                model.StrMensaje = esInicio > 0 ? "NOTA: Dar clic en \"Procesar\" para realizar la evaluación." :
                    string.Format("NOTA: Se ejecutó la evaluación el {0} a las {1}.", fecha.ToString("dd/MM/yyyy"), fecha.ToString("HH:mm:ss"));

                Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_VTEA] = model.DatosValidadorVTEA;

                var jsonResult = Json(model, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReporte(string periodo, string version, string seccion)
        {
            base.ValidarSesionUsuario();


            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";

            var datosVTEA = new VteaValidadorDTO();

            if (Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_VTEA] != null)
            {
                var datosinicio = (VteaValidadorDTO)Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_VTEA];

                datosVTEA.InfoEmpresaResumen = datosinicio.InfoEmpresaResumen;
                datosVTEA.InfoDeclaracionResumen = datosinicio.InfoDeclaracionResumen;
               
            }

            try
            {

                switch (seccion)
                {
                    case "RolEmpresa":
                       

                        nombreArchivo = Helper.ExcelDocumentVteaAnalisis.GenerarReporteRolEmpresa(datosVTEA, periodo, version, rutaLogo);

                        break;

                    case "Energia":

                        nombreArchivo = Helper.ExcelDocumentVteaAnalisis.GenerarReporteEnergia(datosVTEA, periodo, version, rutaLogo);

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

        public async Task<ActionResult> ObtenerRolHistorico(string empresa, string periodo)
        {
            ValidadorVteaAnalisisModel model = new ValidadorVteaAnalisisModel();
            model.VersionesVtea = new VteaVersionDTO();
            model.StrMensajeError = "";

            try
            {

                var versionesVtea = await validacionVteavtpAppServicio.FuncionVteaHistRol(empresa.Trim(), periodo);

                if (versionesVtea.Resultado == 0)
                {
                    model.DatosHisRol = versionesVtea;
                }
                else if (versionesVtea.Resultado == -1)
                {
                    model.DatosHisRol.VteaRolHist = new List<VteaRolHist>();

                    log.Error(versionesVtea.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (versionesVtea.Resultado == 1)
                {
                    model.DatosHisRol.VteaRolHist = new List<VteaRolHist>();

                    model.StrMensajeError = versionesVtea.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            return Content(json, "application/json");
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DetalleEmpresaEnergia(string codigo, string empresa, string cliente, string barra, 
                                                                string pericodi, string recacodi, string periodoTexto, string versionTexto)
        {
            ValidadorVteaAnalisisModel model = new ValidadorVteaAnalisisModel();
           
            model.PeriodoSeleccionado = periodoTexto;
            model.VersionSeleccionado = versionTexto;

            ViewBag.Codigo = codigo;
            ViewBag.Empresa = empresa;
            ViewBag.Cliente = cliente;
            ViewBag.Barra = barra;

            try
            {
              
                VteaDcUnitDTO vteaDcUnitDTO = await validacionVteavtpAppServicio.SmeVteaDcUnit(barra, cliente, codigo, empresa, pericodi, recacodi);


                if (vteaDcUnitDTO.Resultado == 0)
                {
                    
                    model.DetalleDiasPeriodo = vteaDcUnitDTO;
                   
                }
                else if (vteaDcUnitDTO.Resultado == -1)
                {
                    model.DetalleDiasPeriodo = new VteaDcUnitDTO();
                    model.DetalleDiasPeriodo.VteaDcUnit = new List<VteaDcUnit>();

                    log.Error(vteaDcUnitDTO.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (vteaDcUnitDTO.Resultado == 1)
                {
                    model.DetalleDiasPeriodo = new VteaDcUnitDTO();
                    model.DetalleDiasPeriodo.VteaDcUnit = new List<VteaDcUnit>();

                    model.StrMensajeError = vteaDcUnitDTO.Mensaje;
                }
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            return View($"~/Areas/ValidacionVTEAVTP/Views/ValidadorVteaAnalisis/DetalleEmpresaEnergia.cshtml", model);
        }

        public async Task<ActionResult> ObtenerEmpresaEnergiaDia(string codigo, string empresa, string cliente, string barra, string dia, string periodo, string version)
        {
            ValidadorVteaAnalisisModel model = new ValidadorVteaAnalisisModel();
            model.DetalleEmpresaEnergiaDia = new VteaDetailDTO();
            model.StrMensajeError = "";

            try
            {

                VteaDetailDTO vteaDetailDTO = await validacionVteavtpAppServicio.FuntionVteaDetail(barra, cliente, codigo, empresa, dia, periodo, version);

                if (vteaDetailDTO.Resultado == 0)
                {                   
                    model.DetalleEmpresaEnergiaDia = vteaDetailDTO;

                    Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_Energia_Dia_VTEA] = vteaDetailDTO;
                }
                else if (vteaDetailDTO.Resultado == -1)
                {
                    model.DetalleEmpresaEnergiaDia.Tmp = new List<Tmp>();

                    log.Error(vteaDetailDTO.Mensaje);
                    model.StrMensajeError = ErrorInterno;
                }
                else if (vteaDetailDTO.Resultado == 1)
                {
                    model.DetalleEmpresaEnergiaDia.Tmp = new List<Tmp>();

                    model.StrMensajeError = vteaDetailDTO.Mensaje;
                }

            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.StrMensajeError = ErrorInterno;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            return Content(json, "application/json");
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReporteEnergiaDia(string periodo, string version, string dia)
        {
            base.ValidarSesionUsuario();


            string rutaLogo = Server.MapPath("~/Areas/ValidacionVTEAVTP/Content/Images/logocoes_black.png");

            string nombreArchivo = "-1";

            var datosVTEA = new VteaDetailDTO();

            if (Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_Energia_Dia_VTEA] != null)
            {
                var datosinicio = (VteaDetailDTO)Session[Helper.ConstantesValidacionVteavtp.D_Datos_Analisis_Energia_Dia_VTEA];

                datosVTEA.Tmp = datosinicio.Tmp;              

            }

            try
            {
                nombreArchivo = Helper.ExcelDocumentVteaAnalisis.GenerarReporteEnergiaDia(datosVTEA, periodo, version, dia,rutaLogo);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                nombreArchivo = "-1";
            }

            return Json(nombreArchivo);
        }

    }
}
