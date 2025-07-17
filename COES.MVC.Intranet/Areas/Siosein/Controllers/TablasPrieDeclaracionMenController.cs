using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ConsumoCombustible; // SIOSEIN-PRIE-2021
using COES.Servicios.Aplicacion.Factory;                // SIOSEIN-PRIE-2021
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.ReportesMedicion;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.SIOSEIN.Util; // SIOSEIN-PRIE-2021
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text; //SIOSEIN-PRIE-2021
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public partial class TablasPrieDeclaracionMenController : BaseController
    {
        private readonly FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        FormatoReporteAppServicio servFormatoRep = new FormatoReporteAppServicio();
        private readonly InterconexionesAppServicio _servicioInterconex;
        private readonly SIOSEINAppServicio _servicioSiosein;
        private readonly Siosein2AppServicio _servicioSiosein2;
        private readonly IEODAppServicio _servicioIEOD;
        private readonly ConsumoCombustibleAppServicio _servicioConsumoCombustible; //SIOSEIN-PRIE-2021
        private readonly PR5ReportesAppServicio servicioPR5 = new PR5ReportesAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TablasPrieDeclaracionMenController));
        private static string NameController = "TablasPrieDeclaracionMenController";

        /// <summary>
        /// Protected de log de errores page
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

        /// <summary>
        /// listado ventos del controller
        /// </summary>
        public TablasPrieDeclaracionMenController()
        {
            _servicioInterconex = new InterconexionesAppServicio();
            _servicioSiosein = new SIOSEINAppServicio();
            _servicioSiosein2 = new Siosein2AppServicio();
            _servicioIEOD = new IEODAppServicio();
            _servicioConsumoCombustible = new ConsumoCombustibleAppServicio(); //SIOSEIN-PRIE-2021
        }

        #region Propiedades

        /// <summary>
        /// Seteo de Periodo 
        /// </summary>
        public String sPeriodo
        {
            get
            {
                return (Session[DatosSesionDeclaracion.SPeriodo] != null) ?
                    Session[DatosSesionDeclaracion.SPeriodo].ToString() : null;
            }
            set { Session[DatosSesionDeclaracion.SPeriodo] = value; }
        }
        #endregion

        /// <summary>
        /// seteo de periodo
        /// </summary>
        /// <param name="Cabpriperiodo"></param>
        /// <returns></returns>
        public JsonResult LoadPeriodo(string Cabpriperiodo)
        {
            try
            {
                this.sPeriodo = Cabpriperiodo;
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permiter renortar informacion general para la pagina index
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private SioseinModel ObtenerInformacionAMostrarEnIndex(string periodo, int tpriecodi)
        {
            var model = new SioseinModel();
            model.Tpriecodi = tpriecodi;

            try
            {

                DateTime dt = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                model.Mes = dt.ToString(Constantes.FormatoMesAnio);

                model.Mensaje = "...";
                model.Resultado = "action-message";

                model.Resultado2 = dt.NombreMesAbrevAnho();
                model.Resultado3 = dt.AddMonths(-1).NombreMesAbrevAnho();

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                SioTablaprieDTO regPrie = _servicioSiosein.GetByIdSioTablaprie(tpriecodi);
                model.Tprieabrev = regPrie.Tprieabrev;
                model.TituloTabla = string.Format("Tabla {0}: {1} ({2})", regPrie.Tpriecodi.ToString("D2"), textInfo.ToTitleCase(regPrie.Tpriedscripcion.ToLower()), regPrie.Tprieabrev);
                model.TituloWeb = string.Format("Tabla {0} {1}", regPrie.Tpriecodi.ToString("D2"), regPrie.Tprieabrev);
            }
            catch (Exception e)
            {
                model.Mensaje = "Periodo no valido";
                model.Resultado = "action-error";
            }

            return model;
        }

        /// <summary>
        /// //Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }
    }

    public partial class TablasPrieDeclaracionMenController
    {
        #region TABLA 01: POTENCIA FIRME

        #region Verificación

        /// <summary>
        /// index verificacion potencia firme
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PotenciaFirme(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 1);
            DateTime fechaPeriodo = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            model.Resultado2 = string.Format("Potencia Firme (kW) <br>{0}", fechaPeriodo.NombreMesAnho());
            model.Resultado3 = string.Format("Potencia Firme (kW) <br>{0}", fechaPeriodo.AddMonths(-1).NombreMesAnho());

            return View(model);
        }

        /// <summary>
        /// carga lista de potencia firme
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaPotenciaFirme(string mesAnio)
        {
            try
            {
                if (mesAnio != null)
                {
                    var fechaIniAct = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                    var fechaIniAnt = fechaIniAct.AddMonths(-1);

                    List<EqEquipoDTO> listaPFirmeAct = _servicioSiosein.ListarDatosTxtTablaPriePFIR(fechaIniAct);
                    List<EqEquipoDTO> listaPFirmeAnt = _servicioSiosein.ListarDatosTxtTablaPriePFIR(fechaIniAnt);

                    var listaPFirme = _servicioSiosein.ObtenerReportePotenciaFirme(fechaIniAct, listaPFirmeAct, listaPFirmeAnt);
                    string listaPFirmeEmpresaHTML = _servicioSiosein.GenerarRHtmlPotenciaFirmeXEmpresa(fechaIniAct, listaPFirmeAct, listaPFirmeAnt);
                    string listaPFirmeEmpresaCentralHTML = _servicioSiosein.GenerarRHtmlPotenciaFirmeXEmpresaCentral(fechaIniAct, listaPFirmeAct, listaPFirmeAnt);

                    return Json(new { Estado = true, listaPFirme, listaPFirmeEmpresaHTML, listaPFirmeEmpresaCentralHTML });
                }
                return Json(new { Estado = false });
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);
                return Json(new { Estado = false });
            }
        }

        /// <summary>
        /// metodo de envio de informacion a osinergmin tabla 01
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendListaPotenciaFirme(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var listaPotFirmeFinal = _servicioSiosein.ListarDatosTxtTablaPriePFIR(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in listaPotFirmeFinal)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Equicodi = item.Equicodi,
                        Emprcodi = item.Emprcodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = periodo.ToString(ConstantesSioSein.FormatAnioMes),
                            CodigoCentral = item.Osinergcodi,
                            Valor = item.Pf ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie01, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie01, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);
                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion potencia firme
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionPotenciaFirme(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 1);
            model.Titulo = "Difusión web - Potencia firme";

            return View(model);
        }

        /// <summary>
        /// carga lista de difusion potencia firme
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarDifusionPotenciaFirme(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {

                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie01, fechaPeriodo);
                var lstEmpresas = _servicioIEOD.ListarEmpresasXID(string.Join(",", lstDatosPrieTXT.Select(x => x.Emprcodi).Distinct()));

                model.NRegistros = lstDatosPrieTXT.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlDifusionPotenciaFirme(lstDatosPrieTXT, lstEmpresas);
                model.Grafico = _servicioSiosein.GenerarGWebDifusionPotenciaFirme(lstDatosPrieTXT);

                return Json(model);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 02: POTENCIA EFECTIVA (PEFE)

        #region Verificación

        /// <summary>
        /// index potencia efectiva
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult PotenciaEfectiva(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 2);

            return View(model);
        }

        /// <summary>
        /// carga lista potencia efectiva
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaPotenciaEfectiva(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                if (mesAnio != null)
                {
                    var fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                    var fechaPeriodoAnt = fechaPeriodo.AddMonths(-1);

                    List<PotenciaEfectivaReporte> listaPEfectivaAct = _servicioSiosein.ListarDatosTxtTablaPriePEFE(fechaPeriodo);
                    List<PotenciaEfectivaReporte> listaPEfectivaAnt = _servicioSiosein.ListarDatosTxtTablaPriePEFE(fechaPeriodoAnt);

                    var listaPEfectiva = _servicioSiosein.ObtenerReportePotenciaEfectiva(fechaPeriodo, listaPEfectivaAct, listaPEfectivaAnt);
                    return Json(new { Estado = true, listaPEfectiva });
                }
                return Json(new { Estado = false });
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;

                return Json(new { Estado = false });
            }
        }

        [HttpPost]
        public JsonResult SendListaPotenciaEfectiva(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<PotenciaEfectivaReporte> listaPEfectivaAct = _servicioSiosein.ListarDatosTxtTablaPriePEFE(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in listaPEfectivaAct)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Grupocodi = dataEnv.Grupocodi,
                        Emprcodi = dataEnv.Emprcodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            FechaHoraInicio = dataEnv.FechaHoraInicio,
                            FechaHoraFin = dataEnv.FechaHoraFin,
                            CodigoCentral = dataEnv.Osinergcodi2,
                            CodigoModoOperacion = dataEnv.Osinergcodi,
                            Valor = dataEnv.ValorPeAct
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie02, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie02, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion de potencia efectiva
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionPotenciaEfectiva(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 2);
            model.Titulo = "Difusión web - Potencia efectiva";

            return View(model);
        }

        /// <summary>
        /// carga lista de difusion potencia efectiva
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionPotenciaEfectiva(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie02, fechaPeriodo);
                List<PotenciaEfectivaReporte> listaPEfectivaAct = _servicioSiosein.ListarDatosTxtTablaPriePEFE(fechaPeriodo.GetLastDateOfMonth());

                var lstGrupocodi = lstDatosPrieTXT.Select(x => x.Grupocodi).Distinct();
                List<PrGrupoDTO> lstGrupos = _servicioSiosein.GetPrGrupoByIdGrupo(string.Join(",", lstGrupocodi));

                List<SioDatoprieDTO> lstPEfectivaxTGeneracion = _servicioSiosein.ObtenerPEfectivaPotTGeneracion(lstDatosPrieTXT, lstGrupos);
                List<SioReporteDifusion> lstPEfectivaXRecurso = _servicioSiosein.ObtenerPEfectivaxFuenteenergia(lstDatosPrieTXT, lstGrupos);

                model.NRegistros = lstDatosPrieTXT.Count;
                model.Resultados = new List<string>()
                {
                    _servicioSiosein.GenerarRHtmlDifusionPEfectivaXCentral(lstDatosPrieTXT, listaPEfectivaAct),
                    _servicioSiosein.GenerarRHtmlDifusionPotenciaEfectivaXGeneracion(lstPEfectivaxTGeneracion),
                    _servicioSiosein.GenerarRHtmlDifusionPotenciaEfectivaXRecursoEnerg(lstPEfectivaXRecurso)
                };

                model.Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein.GenerarGWebDifusionPEfectivaPorEmpresa(lstDatosPrieTXT, listaPEfectivaAct),
                    _servicioSiosein.GenerarGWebDifusionPEfectivaPorTGeneracion(lstPEfectivaxTGeneracion),
                    _servicioSiosein.GenerarGWebDifusionPEfectivaPorFEnergia(lstPEfectivaXRecurso)
                };

                return Json(model);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 03: COSTOS MARGINALES (CMAR)

        #region Verificacion

        /// <summary>
        /// index verificacion costos marginales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VerificarCostosMarginales(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 3);

            return View(model);
        }

        [HttpPost]
        public JsonResult CargarListaCostosMarginales(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            string url = Url.Content("~/");

            List<CostoMarginalDTO> lstCostomarginal = _servicioSiosein.ListarDatosTxtTablaPrieCMAR(fechaPeriodo, ConstantesAppServicio.ParametroDefecto);
            string lstReportHTMl = _servicioSiosein.GenerarRHtmlCmgPromedioDiarioBarra(fechaPeriodo, lstCostomarginal, url);

            model.Resultado = lstReportHTMl;

            return Json(model);
        }

        /// <summary>
        /// carga grafico barra costos marginales
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarViewGraficoBarraCostoMarg(string mesAnio, string barracodi)
        {
            var model = new SioseinModel();

            DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            model.Grafico = _servicioSiosein.GenerarGWebCmgPromedioBarra(fechaPeriodo, barracodi);

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie costos marginales
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosMarginales(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<CostoMarginalDTO> lstCmgPromedioXBarra = _servicioSiosein.ListarDatosTxtTablaPrieCMAR(periodo, ConstantesAppServicio.ParametroDefecto);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstCmgPromedioXBarra.GroupBy(x => x.BarrCodi).OrderBy(x => x.First().Osinergcodi))
                {
                    foreach (var m96 in dataEnv.ToList())
                    {
                        decimal? valor = null;
                        for (int h = 1; h <= 96; h++)
                        {
                            valor = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterCosMar + h).GetValue(m96, null);
                            if (valor == null) valor = 0.0m;

                            DateTime fechaHora = m96.Fecha.AddMinutes(h * 15);

                            SioDatoprieDTO entityDet = new SioDatoprieDTO
                            {
                                Dprieperiodo = periodo,
                                Barrcodi = dataEnv.Key,//barrcodi
                                Dprieusuario = User.Identity.Name,
                                Dpriefecha = DateTime.Now,
                                SioReporte = new SioReporteDTO()
                                {
                                    FechaHora = fechaHora,
                                    CodigoBarra = m96.Osinergcodi,
                                    Valor = valor ?? 0
                                }
                            };

                            listaDatosPrie.Add(entityDet);
                        }
                    }
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie03, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie03, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;

            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion costos marginales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosMarginales(string periodo)
        {

            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 3);
            model.Titulo = "Difusion Web - Costos Marginales";
            model.Barras = _servicioSiosein.GetListaBarraArea(ConstantesAppServicio.ParametroDefecto).Select(x => new ListaSelect { id = x.BarrCodi, text = x.BarrNombBarrTran }).ToList();
            return View(model);
        }

        /// <summary>
        /// carga lista de difusion costos marginales
        /// </summary>
        /// <param name="mesAnio1"></param>
        /// <param name="mesAnio2"></param>
        /// <param name="barracodi"></param>
        /// <param name="idAreas"></param>
        /// <param name="Tensiones"></param>
        /// <param name="rangos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaDifusionCostosMarginales(string mesAnio, string barracodi)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaPeriodo.GetLastDateOfMonth();

                List<SioDatoprieDTO> lstDatosPrieTXT = new List<SioDatoprieDTO>();
                List<BarraDTO> lstBarras = new List<BarraDTO>();

                if (!string.IsNullOrEmpty(barracodi))
                {
                    lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie03, fechaPeriodo, ConstantesAppServicio.ParametroDefecto,
                        ConstantesAppServicio.ParametroDefecto, barracodi);
                    lstBarras = _servicioSiosein.GetListaBarraArea(barracodi);
                }
                model.Resultado = this._servicioSiosein.GenerarRHtmlDifusionCostosMarginales(lstDatosPrieTXT, lstBarras);


                List<MeMedicion48DTO> lstDemanda = _servicioSiosein.ObtenerMedidodesDespacho(fechaPeriodo, fechaFin, ConstantesAppServicio.TipoinfocodiMW, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto,
                    ConstantesAppServicio.ParametroDefecto, false);

                MeMedicion24DTO demandaHorario = _servicioSiosein.ObtenerDemandaHorario24(lstDemanda);
                List<SioReporteDifusion> lstCmgVsDemanda = _servicioSiosein.ObtenerCmgpromedioVsDemandapromSeinPorDia(lstDatosPrieTXT, demandaHorario);

                model.Resultado2 = this._servicioSiosein.GenerarRHtmlDifusionCmgPromDemanda(lstCmgVsDemanda);

                model.Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein.GenerarGWebCmgPromedioBarra(lstDatosPrieTXT, lstBarras),
                    _servicioSiosein.GenerarGWebCmgpromedioVsDemandapromSein(lstCmgVsDemanda)
                };

                model.NRegistros = lstDatosPrieTXT.Count;

                return Json(model);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista de tension de barras
        /// </summary>
        /// <param name="barras"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaTensionesXBarra(string barras)
        {
            SioseinModel model = new SioseinModel();
            model.Tensiones = new List<ListaSelect>();

            if (!string.IsNullOrEmpty(barras))
            {
                var lstTensionbarras = _servicioSiosein.GetListaBarraArea(barras).Select(x => x.BarrTension).Distinct();
                model.Tensiones = lstTensionbarras.Select(x => new ListaSelect { codigo = x }).ToList();
            }

            return PartialView(model);
        }

        /// <summary>
        /// carga combo lista de areas operativas
        /// </summary>
        /// <param name="barras"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaAreaOpeXBarra(string barras)
        {
            var model = new SioseinModel();
            model.AreasOperativas = new List<ListaSelect>();

            if (!string.IsNullOrEmpty(barras))
            {
                var lstAreaoperativa = _servicioSiosein.GetListaBarraArea(barras).Select(x => new { x.AreaCodi, x.AreaNombre }).Distinct();
                model.AreasOperativas = lstAreaoperativa.Select(x => new ListaSelect { id = x.AreaCodi, text = x.AreaNombre }).ToList();
            }
            return PartialView(model);
        }

        #endregion

        #endregion

        #region TABLA 04: COSTOS VARIABLES

        #region Verificación

        /// <summary>
        /// index costos variables
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosVariables(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 4);

            return View(model);
        }

        /// <summary>
        /// carga lista de costos variables
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosVariables(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                if (mesAnio != null)
                {
                    var fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                    var listaCV = _servicioSiosein.ListarDatosTxtTablaPrieCVAR(fechaPeriodo, ConstantesAppServicio.ParametroDefecto);

                    var lista = listaCV.Select(x => new { x.Fenergcodi, x.Fenergnomb }).Distinct().ToList();

                    model.Resultado = _servicioSiosein.GenerarRHtmlCostosVariables(listaCV);
                    model.Grafico = _servicioSiosein.GenerarGWebCcombModoOperacion(listaCV);
                    model.NRegistros = listaCV.Count();
                    model.Lista1 = lista.Select(x => new ListaSelect() { id = x.Fenergcodi, text = x.Fenergnomb.Trim() }).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = 0;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie costos variables
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosVariables(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<PrCvariablesDTO> listaCV = _servicioSiosein.ListarDatosTxtTablaPrieCVAR(periodo, ConstantesAppServicio.ParametroDefecto);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in listaCV)
                {

                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Grupocodi = dataEnv.Grupocodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            FechaHoraInicio = dataEnv.FechaIni,
                            FechaHoraFin = dataEnv.FechaFin,
                            CodigoModoOperacion = dataEnv.Osinergcodi,
                            RendimientoTermico = dataEnv.CecSi ?? 0,
                            CodigoTipoCombustible = dataEnv.OsinergcodiFe,
                            CostoCombustible = dataEnv.Ccomb ?? 0,
                            ValorCombustible = dataEnv.Cvc ?? 0,
                            ValorNoCombustible = dataEnv.Cvnc ?? 0,
                            ValorCostoVariable = dataEnv.Cv ?? 0
                        }
                    };

                    listaDatosPrie.Add(entityDet);

                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie04, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie04, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico de costos variables por tipo de combusti
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="tipComb"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoCostosVariablesXTComb(string mesAnio, string tipComb)
        {
            var model = new SioseinModel();
            try
            {
                if (mesAnio != null)
                {
                    var fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                    var listaCV = _servicioSiosein.ListarDatosTxtTablaPrieCVAR(fechaPeriodo, tipComb);
                    model.Grafico = _servicioSiosein.GenerarGWebEvolucionCVariablesPorTipCombustible(listaCV);
                }
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index  difusion de costos variables
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosVariables(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 4);
            model.Titulo = "Difusion Web - Costos Variables";

            model.ModoOpe = _servicioSiosein.ListaModosOperacionActivos();
            model.TipoCombustible = _servicioSiosein.ListaTipoCombustible().Where(x => x.Fenergcodi > 0).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos variables
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="modoOpe"></param>
        /// <param name="tipoCombustible"></param>
        /// <param name="precioComb"></param>
        /// <param name="tipoCostoVar"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosVariables(string mesAnio, string modoOpe, List<string> tipoCombustible, List<string> tipoCostoVar)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaPeriodo.GetLastDateOfMonth();

                List<SioDatoprieDTO> lstDatosPrieTXT = new List<SioDatoprieDTO>();
                List<PrGrupoDTO> lstModosoperacion = new List<PrGrupoDTO>();
                if (!string.IsNullOrEmpty(modoOpe))
                {
                    lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie04, fechaPeriodo, ConstantesAppServicio.ParametroDefecto, modoOpe);
                    var lstmodo = lstDatosPrieTXT.Select(x => x.Grupocodi).Distinct();
                    lstModosoperacion = _servicioSiosein.GetPrGrupoByIdGrupo(string.Join(",", lstmodo));
                }
                if (tipoCombustible.Any())
                {
                    var lstFuenteenergia = tipoCombustible.Select(x => int.Parse(x));
                    lstModosoperacion = lstModosoperacion.Where(x => lstFuenteenergia.Contains(x.Fenergcodi ?? 0)).ToList();
                }
                var lstCostovariable = _servicioSiosein.ObtenerCostoVariableMensualPorModo(lstDatosPrieTXT, lstModosoperacion, tipoCostoVar);
                model.Resultado = _servicioSiosein.GenerarRHtmlConsolidadoCostoVariables(lstCostovariable, 1);
                model.Resultado2 = _servicioSiosein.GenerarRHtmlConsolidadoCostoVariables(lstCostovariable, 2);
                model.Resultado3 = _servicioSiosein.GenerarRHtmlCostoVariableMensualPorTipoCombustible(lstCostovariable);

                model.Graficos = new List<GraficoWeb>
                {
                    _servicioSiosein.GenerarGwebCostoVariableMensualXModo(lstCostovariable, 1),
                    _servicioSiosein.GenerarGwebCostoVariableMensualXModo(lstCostovariable, 2),
                    _servicioSiosein.GenerarGWebCostoVariableMensualPorTipoCombustible(lstCostovariable)
                };
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 05: PRODUCCIÓN DE ENERGÍA

        #region Verificacion

        /// <summary>
        /// index produccion de energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult ProduccionEnergia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 5);

            return View(model);
        }

        /// <summary>
        /// carga lista de produccion de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaProduccionEnergia(string mesAnio)
        {
            SioseinModel model = new SioseinModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                model.Resultado = this._servicioSiosein.GenerarRHtmlProduccionEnergia(fechaInicio, fechaFin);
                model.ResultadoInt = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico de diferencias presentadas en el periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDiferenciasPresentadasDelPeriodo(string periodo, int equicodi)
        {
            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<MeMedicion96DTO> ListaDatosPrie = new List<MeMedicion96DTO>();
            //Data generada por JOB
            List<MeMedicion96DTO> ListaDatos = new List<MeMedicion96DTO>();

            var model = new SioseinModel();

            if (ListaDatos.Count > 0)
            {
                model.Grafico = new GraficoWeb();
                model.Grafico.Series = new List<RegistroSerie>();
                RegistroSerie EquipoTXT = new RegistroSerie();
                RegistroSerie EquipoSGCOES = new RegistroSerie();

                EquipoTXT.Name = string.Format("(TXT){0}", ListaDatos[0].Equinomb);
                EquipoSGCOES.Name = string.Format("(SGOCOES){0}", ListaDatos[0].Equinomb);

                EquipoTXT.Data = new List<DatosSerie>();
                EquipoSGCOES.Data = new List<DatosSerie>();

                EquipoTXT.Data = ResumenColeccionMedicionDatos(ListaDatosPrie.Where(x => x.Tipoinfocodi == ConstantesSioSein.EnergiaActiva).ToList());
                EquipoSGCOES.Data = ResumenColeccionMedicionDatos(ListaDatos.Where(x => x.Tipoinfocodi == ConstantesSioSein.EnergiaActiva).ToList());

                model.Grafico.Series.Add(EquipoTXT);
                model.Grafico.Series.Add(EquipoSGCOES);
            }

            return Json(model);
        }

        /// <summary>
        /// lista resumen coleccion de medicion de datos
        /// </summary>
        /// <param name="ListaData"></param>
        /// <returns></returns>
        private List<DatosSerie> ResumenColeccionMedicionDatos(List<MeMedicion96DTO> ListaData)
        {
            List<DatosSerie> ListaColeccion = new List<DatosSerie>();
            bool count = true;
            if (ListaData.Count > 0)
            {
                foreach (var item in ListaData)
                {
                    for (int i = 1; i <= 96; i++)
                    {
                        DatosSerie valor = new DatosSerie();
                        if (count) { valor.X = (DateTime)item.Medifecha; count = false; }
                        decimal? value = (decimal?)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                        if (value == null)
                            value = 0.00m;
                        valor.Y = value;
                        ListaColeccion.Add(valor);
                    }
                }
            }
            return ListaColeccion;
        }

        public JsonResult SendProduccionEnergia(string periodo)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                List<SioDatoprieDTO> listaDatosPrie = _servicioSiosein.PrepareDatosPrieProduccionEnergia(fechaInicio, fechaFin, User.Identity.Name);

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie05, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie05, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion produccion de energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionProduccionEnergia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = new SioseinModel();

            DateTime fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            model.Mes = fechaInicio.ToString(Constantes.FormatoMesAnio);

            model.Titulo = "Difusion Web - Produccion de Energia";
            model.ListaTipoGeneracion = _servicioSiosein.ListaTipoGeneracion().Where(x => x.Tgenercodi > 0).ToList();
            model.ListaRecursoEnergetico = _servicioSiosein.ListaTipoCombustible().Where(x => x.Fenergcodi > 0).ToList();
            //SIOSEIN-PRIE-2021
            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();
            //

            return View(model);
        }

        /// <summary>
        /// carga lista de difusion por produccion de energia
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProduccionEnergia(string periodo, List<string> tipoGene, List<string> recenerg)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie05, fechaInicio);

                model.Resultado = this._servicioSiosein.GenerarRHtmlDifusionProduccionEnergia(listaDataPrieTXT);
            }
            catch (Exception)
            {
                throw;
            }

            return Json(model);
        }

        #region "Difusion"


        /// <summary>
        /// carga lista difusion produccion de energia por empresas
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProduccionEnergiaXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.ReporteResumen05)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProduccionEnergiaXEmpresa(ListaContenido);
            return Json(model);
        }

        /// <summary>
        /// carga grafico de difusion produccion de energia por empresas
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProduccionEnergiaXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> ListaResumenXEmpresa = new List<MeMedicion96DTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.ReporteResumen05)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();
            ListaResumenXEmpresa = _servicioSiosein.ObtenerResumenReporteDifusionProdEnerXEmp(ListaContenido);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaResumenXEmpresa)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;

                model.Grafico.Series.Add(registro);
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico de produccion de energ por tipo de recurso energ
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoProdEnergXTipGenYRecurEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string mesAnio)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.ReporteResumen05)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();


            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.XAxisCategories = new List<string>();

            //Categorias
            var categoriasIncluidasEnRenovables = new[] { ConstantesSioSein.TgenerSolar, ConstantesSioSein.TgenerEolica };
            List<MeMedicion96DTO> ListaCategorias = ListaContenido
                .Where(x => !categoriasIncluidasEnRenovables.Contains(x.Tgenercodi))
                .GroupBy(x => x.Tgenercodi)
                .Select(x => new MeMedicion96DTO
                {
                    Tgenercodi = x.First().Tgenercodi,
                    Tgenernomb = x.First().Tgenernomb
                }).ToList();

            int renovables = ListaContenido.Where(x => x.Tipogrupocodi == ConstantesSioSein.GrupoRenovable).ToList().Count;

            if (renovables > 0) ListaCategorias.Add(new MeMedicion96DTO { Tgenernomb = ConstantesSioSein.CateRenovables });

            foreach (var item in ListaCategorias) model.Grafico.XAxisCategories.Add(item.Tgenernomb);
            var ListaFuenteEnergias = ListaContenido.GroupBy(x => x.Fenergcodi).Select(x => new MeMedicion96DTO { Fenergcodi = x.First().Fenergcodi, Fenergnomb = x.First().Fenergnomb }).ToList();


            foreach (var fuente in ListaFuenteEnergias)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = fuente.Fenergnomb;
                registro.Data = new List<DatosSerie>();


                foreach (var cat in ListaCategorias)
                {
                    DatosSerie datoInicializado = new DatosSerie();
                    datoInicializado.Y = 0.00m;
                    registro.Data.Add(datoInicializado);
                };
                foreach (var item in ListaContenido)
                {
                    for (int i = 0; i < ListaCategorias.Count; i++)
                    {
                        if (fuente.Fenergcodi == item.Fenergcodi && ListaCategorias[i].Tgenernomb
                            == ConstantesSioSein.CateRenovables && item.Tipogrupocodi == ConstantesSioSein.GrupoRenovable)
                            registro.Data[i].Y = registro.Data[i].Y + item.Total;
                        else
                            if (fuente.Fenergcodi == item.Fenergcodi && ListaCategorias[i].Tgenercodi
                                == item.Tgenercodi && item.Tipogrupocodi != ConstantesSioSein.GrupoRenovable)
                            registro.Data[i].Y = registro.Data[i].Y + item.Total;
                    }
                }
                model.Grafico.Series.Add(registro);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// carga difusion produccion de energ por central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProdEnergXCentralYRecursoEner(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.ReporteResumen05)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();
            model.Resultado = _servicioSiosein.ListarReporteProdEnergXCentralYRecursoEner(ListaContenido);

            return Json(model);
        }

        /// <summary>
        /// carga grafico de produccion de energia por tipo de central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoProdenergXTipoCentralYRecurEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.ReporteResumen05)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.XAxisCategories = new List<string>();

            //Categorias
            var categoriasIncluidasEnRenovables = new[] { ConstantesSioSein.TgenerSolar, ConstantesSioSein.TgenerEolica };
            List<MeMedicion96DTO> ListaCategorias = ListaContenido
                .Where(x => !categoriasIncluidasEnRenovables.Contains(x.Tgenercodi))
                .GroupBy(x => x.Tgenercodi)
                .Select(x => new MeMedicion96DTO
                {
                    Tgenercodi = x.First().Tgenercodi,
                    Tgenernomb = x.First().Tgenernomb
                }).ToList();

            int renovables = ListaContenido.Where(x => x.Tipogrupocodi == ConstantesSioSein.GrupoRenovable).ToList().Count;
            int nodoEnerg = ListaContenido.Where(x => x.Grupomiembro == ConstantesSioSein.GrupoNodoEnergetico).ToList().Count;
            int reservaFr = ListaContenido.Where(x => x.Grupomiembro == ConstantesSioSein.GrupoReservaFria).ToList().Count;
            int emergenc = ListaContenido.Where(x => x.Grupomiembro == ConstantesSioSein.GrupoEmergencia).ToList().Count;

            if (renovables > 0) ListaCategorias.Add(new MeMedicion96DTO { Tgenernomb = ConstantesSioSein.CateRenovables });
            if (nodoEnerg > 0) ListaCategorias.Add(new MeMedicion96DTO { Tgenernomb = ConstantesSioSein.CateNodoEnergetico, Grupomiembro = ConstantesSioSein.GrupoNodoEnergetico });
            if (nodoEnerg > 0) ListaCategorias.Add(new MeMedicion96DTO { Tgenernomb = ConstantesSioSein.CateReservaFria, Grupomiembro = ConstantesSioSein.GrupoReservaFria });
            if (nodoEnerg > 0) ListaCategorias.Add(new MeMedicion96DTO { Tgenernomb = ConstantesSioSein.CateEmergencia, Grupomiembro = ConstantesSioSein.GrupoEmergencia });

            foreach (var item in ListaCategorias) model.Grafico.XAxisCategories.Add(item.Tgenernomb);
            var ListaFuenteEnergias = ListaContenido.GroupBy(x => x.Fenergcodi).Select(x => new MeMedicion96DTO { Fenergcodi = x.First().Fenergcodi, Fenergnomb = x.First().Fenergnomb }).ToList();

            foreach (var categoria in ListaCategorias)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = categoria.Tgenernomb;
                registro.Acumulado = 0.00m;
                model.Grafico.Series.Add(registro);
            }
            foreach (var item in ListaContenido)
            {
                for (int i = 0; i < ListaCategorias.Count; i++)
                {
                    if (ListaCategorias[i].Tgenernomb == ConstantesSioSein.CateRenovables && item.Tipogrupocodi == ConstantesSioSein.GrupoRenovable && item.Grupomiembro != ConstantesSioSein.GrupoNodoEnergetico
                        && item.Grupomiembro != ConstantesSioSein.GrupoReservaFria && item.Grupomiembro != ConstantesSioSein.GrupoEmergencia)
                    {
                        model.Grafico.Series[i].Acumulado += item.Total;
                    }
                    else if (ListaCategorias[i].Grupomiembro == item.Grupomiembro)
                    {
                        model.Grafico.Series[i].Acumulado += item.Total;
                    }
                    else if (ListaCategorias[i].Tgenercodi == item.Tgenercodi)
                    {
                        model.Grafico.Series[i].Acumulado += item.Total;
                    }
                }
            }
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// carga lista difusion de produccion de energia por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProdEnergiaMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.Reporte05MaxDemanEmpresa).Where(x => idsEmpresa.Contains(x.Emprcodi)).OrderBy(x => x.Emprnomb).ToList();

            model.Resultado = _servicioSiosein.ListarReporteDifusionDifusionProdEnergiaMaxDemanda(ListaContenido);
            //model.Resultado = servicio.ListarReporteDifusionProgOperacionSemanalMaxDemanda(ListaContenido);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion produccion energia por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProdEnergiaMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            //Obteniendo datos de la tabla Datoprie
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.Reporte05MaxDemanEmpresa).Where(x => idsEmpresa.Contains(x.Emprcodi)).OrderBy(x => x.Emprnomb).ToList();

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;
                model.Grafico.Series.Add(registro);
            }
            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion produccion de energia por tipo de tecnologia
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProdEnergialXTipoTecnologia(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.Reporte05MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();
            List<MeMedicion96DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologia(ListaContenido, ConstantesSioSein.Grafico);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.Series = RegistrosResumenGraficoProdEnergiaXTipoTecnologia(ListaResumenContenido);

            if (ListaContenido.Count > 0)
            {
                List<string> categorias = new List<string>();

                int min = 0;
                DateTime fecha = DateTime.MinValue;
                for (int i = 1; i <= 96; i++)
                {
                    min = min + 15;
                    categorias.Add(fecha.AddMinutes(min).ToString("HH:mm"));
                }
                model.Grafico.XAxisCategories = categorias;
            }
            return Json(model);
        }

        /// <summary>
        /// resumen de produccion de energia por tipo de tecnologia
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ResumenProdEnergiaXTipoTecnologia(List<MeMedicion96DTO> data, string Tipo)
        {
            List<MeMedicion96DTO> ListaResumen = new List<MeMedicion96DTO>();
            #region Grafico
            if (Tipo == ConstantesSioSein.Grafico)
            {
                List<MeMedicion96DTO> ListCateEmprTec = new List<MeMedicion96DTO>();
                bool catBagazoReRAgregado = false, catBiogazReRAgregado = false, catCarbonAgregado = false, catDiesel2Agregado = false, catEolicoAgregado = false,
                     catEolicoReRAgregado = false, catAguaAgregado = false, catAguaReRAgregado = false, catSolarAgregado = false;
                foreach (var item in data)
                {
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && !catEolicoAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerEolica, Fenergnomb = ConstantesSioSein.Eolico });
                        catEolicoAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerSolar && !catSolarAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerSolar, Fenergnomb = ConstantesSioSein.Solar });
                        catSolarAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer == ConstantesSioSein.SiReR && !catAguaReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerAgua, Fenergnomb = ConstantesSioSein.HidroRER });
                        catAguaReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && !catAguaAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerAgua, Fenergnomb = ConstantesSioSein.Hidro });
                        catAguaAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Tipogenerrer == ConstantesSioSein.SiReR && !catBagazoReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerBagazo, Fenergnomb = ConstantesSioSein.BagazoReR });
                        catBagazoReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Tipogenerrer == ConstantesSioSein.SiReR && !catBiogazReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerBiogas, Fenergnomb = ConstantesSioSein.BiogasReR });
                        catBiogazReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon && !catCarbonAgregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerCarbon, Fenergnomb = ConstantesSioSein.CarbonReporte });
                        catCarbonAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerResidual)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerResidual, Fenergnomb = ConstantesSioSein.Residual });
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5 && !catDiesel2Agregado)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerDieselB5, Fenergnomb = ConstantesSioSein.Diesel2Reporte });
                        catDiesel2Agregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && !catEolicoReRAgregado && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        ListaResumen.Add(new MeMedicion96DTO { Fenergcodi = ConstantesSioSein.FuenteEnerEolica, Fenergnomb = ConstantesSioSein.EolicoRer });
                        catEolicoReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.GasLaIsla || item.Fenergcodi == ConstantesSioSein.GasDeAguaytia
                        || item.Fenergcodi == ConstantesSioSein.GasDeCamisea || item.Fenergcodi == ConstantesSioSein.GasDeMalacas)
                    {
                        if (item.Tipogrupocodi == ConstantesSioSein.GrupoCoGeneracion)
                        {
                            ListaResumen.Add(new MeMedicion96DTO { Fenergnomb = string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion) });
                            ListCateEmprTec.Add(new MeMedicion96DTO
                            {
                                Fenergnomb = string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion),
                                Fenergcodi = item.Emprcodi
                            });
                        }
                        else
                        {
                            ListaResumen.Add(new MeMedicion96DTO { Fenergnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb) });
                            ListCateEmprTec.Add(new MeMedicion96DTO
                            {
                                Fenergnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb),
                                Fenergcodi = item.Emprcodi
                            });
                        }
                    }
                }
                foreach (var item in ListaResumen)
                {
                    for (int i = 1; i <= 96; i++)
                    {
                        decimal inicializacion = 0.00m;
                        DateTime medifecha = (DateTime)data[0].Medifecha;
                        item.GetType().GetProperty("H" + i.ToString()).SetValue(item, inicializacion);
                        item.Medifecha = medifecha;
                    }
                }
                foreach (var cat in ListaResumen)
                {
                    foreach (var item in data)
                    {
                        if (cat.Fenergnomb == ConstantesSioSein.Eolico && cat.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerSolar && item.Fenergcodi == ConstantesSioSein.FuenteEnerSolar)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.HidroRER && cat.Fenergcodi == ConstantesSioSein.FuenteEnerAgua
                            && item.Tipogenerrer == ConstantesSioSein.SiReR && item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.Hidro && cat.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo &&
                            item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.BiogasReR && cat.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas &&
                            item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon && item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5 && item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.EolicoRer && item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        foreach (var EmprTec in ListCateEmprTec)
                        {
                            if (cat.Fenergnomb == EmprTec.Fenergnomb
                                && EmprTec.Fenergnomb == string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion)
                                )
                            {
                                for (int i = 1; i <= 96; i++)
                                {
                                    decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                    decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                    cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                                }
                            }
                            else if (cat.Fenergnomb == EmprTec.Fenergnomb
                                && EmprTec.Fenergnomb == string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb))
                            {
                                for (int i = 1; i <= 96; i++)
                                {
                                    decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                    decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                    cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                                }
                            }
                        }

                    }
                }
            }
            #endregion
            #region Lista
            if (Tipo == ConstantesSioSein.Lista)
            {
                foreach (var item in data)
                {
                    //FILTRO PARA HIDRICOS
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer != ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.Hidrico)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion96DTO entity = new MeMedicion96DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.Hidrico;
                            entity.Emprnomb = ConstantesSioSein.Hidroelectricas;
                            entity.MaxDemanda = item.MaxDemanda;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.Hidrico)
                                                        .Select(y => y.MaxDemanda).First();
                            decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.Hidrico).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA GAS NATURAL
                    else if (item.Fenergcodi == ConstantesSioSein.GasLaIsla || item.Fenergcodi == ConstantesSioSein.GasDeAguaytia
                        || item.Fenergcodi == ConstantesSioSein.GasDeCamisea || item.Fenergcodi == ConstantesSioSein.GasDeMalacas)
                    {
                        MeMedicion96DTO entity = new MeMedicion96DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = item.Fenergnomb;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionSiEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA CARBON
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon)
                    {
                        MeMedicion96DTO entity = new MeMedicion96DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = item.Fenergnomb;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA DIESEL 2
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion96DTO entity = new MeMedicion96DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = item.Fenergnomb;
                            entity.Emprnomb = ConstantesSioSein.MotoresDiesel;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                                                        .Select(y => y.MaxDemanda).First();
                            decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            ListaResumen.Where(x => x.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA HIDRICO (RER)
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.HidroRER)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion96DTO entity = new MeMedicion96DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.HidroRER;
                            entity.Emprnomb = ConstantesSioSein.Hidroelectricas20MV;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.HidroRER)
                                                        .Select(y => y.MaxDemanda).First();
                            decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.HidroRER).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA EOLICO RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.EolicoRer)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion96DTO entity = new MeMedicion96DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.EolicoRer;
                            entity.Emprnomb = ConstantesSioSein.Aerogenerador;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.EolicoRer)
                                                        .Select(y => y.MaxDemanda).First();
                            decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.EolicoRer).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA BAGAZO RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        MeMedicion96DTO entity = new MeMedicion96DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = ConstantesSioSein.BagazoReR;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA BIOGÁS RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.BiogasReR)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion96DTO entity = new MeMedicion96DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.BiogasReR;
                            entity.Emprnomb = ConstantesSioSein.MDieselBiogas;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.BiogasReR)
                                                        .Select(y => y.MaxDemanda).First();
                            decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.BiogasReR).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }
                }
                decimal total = 0;
                foreach (var item in ListaResumen) { total = total + (decimal)item.MaxDemanda; item.Medifecha = data[0].Medifecha; }
                foreach (var item in ListaResumen) { item.PorcentParticipacion = ((item.MaxDemanda / total) * 100); }
            }
            #endregion
            return ListaResumen;
        }

        /// <summary>
        /// resumen de grafico produccion de energia por tipo de tecnologia
        /// </summary>
        /// <param name="ListaContenido"></param>
        /// <returns></returns>
        public List<RegistroSerie> RegistrosResumenGraficoProdEnergiaXTipoTecnologia(List<MeMedicion96DTO> ListaContenido)
        {
            List<RegistroSerie> data = new List<RegistroSerie>();
            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Data = new List<DatosSerie>();
                registro.Name = item.Fenergnomb;
                registro.Type = "area";
                for (int i = 1; i <= 96; i++)
                {
                    DatosSerie dato = new DatosSerie();
                    dato.Y = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                    registro.Data.Add(dato);
                }
                data.Add(registro);
            }
            int minutos = 0;
            int indexH = 0;
            minutos = (((DateTime)ListaContenido[0].Medifecha).Hour * 60);
            minutos += ((DateTime)ListaContenido[0].Medifecha).Minute;
            indexH = minutos / 15;


            RegistroSerie registroColumn = new RegistroSerie();
            registroColumn.Data = new List<DatosSerie>();

            decimal maxDemanDelDia = 0.00m;
            foreach (var item in ListaContenido)
            {
                for (int i = 1; i <= 96; i++)
                {
                    if (maxDemanDelDia < ((decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null)))
                    {
                        maxDemanDelDia = ((decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null));
                    }
                }
            }
            registroColumn.Type = "column";
            registroColumn.Name = "máxima demanda del día";
            for (int i = 1; i <= 96; i++)
            {
                DatosSerie dato = new DatosSerie();
                if (i != indexH)
                {
                    dato.Y = 0.00m;
                }
                else
                {
                    dato.Y = maxDemanDelDia;
                }
                registroColumn.Data.Add(dato);
            }
            data.Add(registroColumn);

            return data;
        }

        /// <summary>
        /// carga lista difusion produccion de energia por tipo de tecnologia
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProdEnergiaXTipoTecnologia(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.Reporte05MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi) && idsTipoGen.Contains(x.Tgenercodi) && idsRecursoEner.Contains(x.Fenergcodi))
                                                                                        .OrderBy(x => x.Emprnomb).ToList();
            List<MeMedicion96DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologia(ListaContenido, ConstantesSioSein.Lista);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProdEnergiaXTipoTecnologia(ListaResumenContenido);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion intercambio energia dia maximo de demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionIntercambioEnergiaDiaMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie05).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla05(ListaDatos, ConstantesSioSein.Reporte05IntercambioEnerg);
            //.Where(x => idsEmpresa.Contains(x.Emprcodi))
            ////.OrderBy(x => x.Emprnomb)
            ////.ToList();

            model.Resultado = this._servicioSiosein.ListarIntercambioEnergiaPorDiaMaxDemanda(ListaContenido);

            return Json(model);
        }

        #endregion

        #endregion

        #endregion

        #region TABLA 06: DESVIACIONES

        #region VERIFICACIÓN

        /// <summary>
        /// index desviaciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult Desviaciones(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 6);

            return View(model);
        }

        /// <summary>
        /// carga lista desviaciones
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDesviaciones(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie06, fechaInicio);

                List<MeMedicion96DTO> lstPotenciaActEjecSGOCOES = _servicioSiosein.ObtenerMedidoresGeneracionXUnidadGeneracion(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW);

                List<MeMedicion96DTO> lstPotenciaEjecXTipogen = _servicioSiosein.ObtenerListaMedicion96xAgrupacion(lstPotenciaActEjecSGOCOES.GroupBy(x => new { x.Equicodi, x.Tgenercodi }));
                List<MeMedicion48DTO> lstPotenciaProgXTipogen = new List<MeMedicion48DTO>();

                model.NRegistros = lstPotenciaActEjecSGOCOES.Count();
                model.Resultado = _servicioSiosein.GenerarRHtmlDesviaciones(lstPotenciaEjecXTipogen, lstPotenciaProgXTipogen, listaDataPrieTXT);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = 0;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de desviaciones
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaDesviaciones(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                List<MeMedicion96DTO> lstPotenciaActEjecSGOCOES = _servicioSiosein.ObtenerMedidoresGeneracionXUnidadGeneracion(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW);
                lstPotenciaActEjecSGOCOES = _servicioSiosein.ObtenerListaMedicion96xAgrupacion(lstPotenciaActEjecSGOCOES.GroupBy(x => new { x.Equicodi, x.Tgenercodi }))
                                                .OrderBy(x => x.Osinergcodi).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstPotenciaActEjecSGOCOES)
                {
                    if (dataEnv.Meditotal > 0)
                    {
                        SioDatoprieDTO entityDet = new SioDatoprieDTO
                        {
                            Dprieperiodo = fechaInicio,
                            Equicodi = dataEnv.Equicodi,
                            Dprieusuario = User.Identity.Name,
                            Dpriefecha = DateTime.Now,
                            SioReporte = new SioReporteDTO()
                            {
                                Periodo = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                                CodigoGrupo = dataEnv.Osinergcodi ?? "",
                                Programada = 0M,//Programado diario se guarda por modos y central las hidraulicas, no es posible obtener por unidades de generación
                                Real = (dataEnv.Meditotal ?? 0) / 4 / 1000//Mw a Mwh (Energia ejecuta mensual)
                            }
                        };
                        listaDatosPrie.Add(entityDet);
                    }
                }


                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie06, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie06, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion desviaciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionDesviaciones(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            model.TipoCombustible = _servicioSiosein.ListaTipoCombustible();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion desviaciones
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionDesviaciones(string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfecha = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecha = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie06, dfecha);

            model.NRegistros = listDatoPrie.Count;
            model.Resultado = this._servicioSiosein.ListarReporteDifusionDesviaciones(listDatoPrie);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion desviaciones
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionDesviaciones(string mesAnio, int param)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie06, dfechaIni);

            if (listDatoPrie.Count > 0)
            {
                if (param != 0) { model = GraficoDifusionResumenDesviaciones(listDatoPrie, param); }
                else { model = GraficoDifusionDesviaciones(listDatoPrie); }
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dfechaFin"></param>
        /// <returns></returns>
        private SioseinModel GraficoDifusionDesviaciones(List<SioDatoprieDTO> data)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            foreach (var upd in data)
            {
                /*
                lista.Add(new MeMedicion48DTO()
                {
                    Lectcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0]),
                    Fenergcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1]),
                    Emprcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2]),
                    Emprnomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3],
                    Gruponomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4],
                    Grupocodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]),
                    Osinergcodi = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6],
                    Meditotal = decimal.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7])
                });
                */
                string Osinergcodi = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[2])[1];
                SioDatoprieDTO MeMedicion48 = _servicioSiosein.ObtenerMeMedicion48Prie(Osinergcodi);
                if (MeMedicion48 != null)
                {
                    lista.Add(new MeMedicion48DTO()
                    {
                        Lectcodi = 1,      //llenar con 1 ejecutado, 4 programado
                        Fenergcodi = MeMedicion48.Fenergcodi,
                        Emprcodi = MeMedicion48.Emprcodi ?? default(int),
                        Emprnomb = MeMedicion48.Emprnomb,
                        Gruponomb = MeMedicion48.Gruponomb,
                        Grupocodi = MeMedicion48.Grupocodi ?? default(int),
                        Osinergcodi = MeMedicion48.Osinergcodi,
                        Meditotal = decimal.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[2])[3])
                    });
                }
            }

            string titulo1 = "DESVIACIONES ENTRE LA ENERGIA PROGRAMADA Y EJECUTADA EN EL SEIN POR EMPRESAS", name = "Energia Ejecutada (GWh)", name1 = "Energia Programada (GWh)", name2 = "Participacion (%)",
                type = "column", type1 = "column", type2 = "spline";
            model.Grafico = grafico;
            model.Grafico.SeriesData = new decimal?[3][];
            if (lista.Count > 0)
            {
                {
                    model.Grafico.TitleText = titulo1;

                    model.Grafico.XAxisCategories = new List<string>();
                    model.Grafico.YAxixTitle = new List<string>();

                    model.Grafico.YAxixTitle.Add("Energia (MWh)");
                    model.Grafico.YAxixTitle.Add("Variacion (%)");

                    // Obtener lista de intervalos categoria del grafico 
                    var empresas = lista.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).Distinct().ToList();
                    model.Grafico.XAxisCategories = empresas.Select(x => x.Emprnomb).ToList();
                    int totalIntervalos = empresas.Count;

                    model.Grafico.Series = new List<RegistroSerie>();
                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series.Add(new RegistroSerie());

                    //inicio llenado de las series
                    model.Grafico.Series[0].Name = name;
                    model.Grafico.Series[0].Type = type;
                    model.Grafico.Series[0].YAxis = 0;

                    model.Grafico.Series[1].Name = name1;
                    model.Grafico.Series[1].Type = type1;
                    model.Grafico.Series[1].YAxis = 0;

                    model.Grafico.Series[2].Name = name2;
                    model.Grafico.Series[2].Type = type2;
                    model.Grafico.Series[2].YAxis = 1;

                    model.Grafico.SeriesData[0] = new decimal?[totalIntervalos];
                    model.Grafico.SeriesData[1] = new decimal?[totalIntervalos];
                    model.Grafico.SeriesData[2] = new decimal?[totalIntervalos];
                    int e = 0;
                    for (int x = 0; x < empresas.Count; x++)
                    {
                        decimal ejec = 0, prog = 0, vari = 0;
                        var eje = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiMedidores)).ToList();
                        var pro = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiProgDiario)).ToList();
                        if (eje.Count > 0)
                        {
                            foreach (var d in eje) { ejec += (decimal)d.Meditotal; lista.Remove(d); }
                        }
                        if (pro.Count > 0)
                        {
                            foreach (var d in pro) { prog += (decimal)d.Meditotal; lista.Remove(d); }
                        }
                        if (ejec != 0 && prog != 0)
                        {
                            vari = (prog - ejec) / ejec;
                        }

                        model.Grafico.SeriesData[0][e] = ejec;
                        model.Grafico.SeriesData[1][e] = prog;
                        model.Grafico.SeriesData[2][e] = vari;
                        e++;
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// carga lista difusion resumen desviaciones
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionResumenDesviaciones(string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfecha = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecha = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie06, dfecha);

            model.NRegistros = listDatoPrie.Count;
            model.Resultado = this._servicioSiosein.ListarReporteDifusionResumenDesviaciones(listDatoPrie);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private SioseinModel GraficoDifusionResumenDesviaciones(List<SioDatoprieDTO> data, int param)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> eje = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> pro = new List<MeMedicion48DTO>();

            foreach (var upd in data)
            {
                /*lista.Add(new MeMedicion48DTO()
                {
                    Lectcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0]),
                    Fenergcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1]),
                    Emprcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2]),
                    Emprnomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3],
                    Gruponomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4],
                    Grupocodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]),
                    Osinergcodi = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6],
                    Meditotal = decimal.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7])
                });
                */
                string Osinergcodi = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[2])[1];
                SioDatoprieDTO MeMedicion48 = _servicioSiosein.ObtenerMeMedicion48Prie(Osinergcodi);
                if (MeMedicion48 != null)
                {
                    lista.Add(new MeMedicion48DTO()
                    {
                        Lectcodi = 1,      //llenar con 1 ejecutado, 4 programado
                        Fenergcodi = MeMedicion48.Fenergcodi,
                        Emprcodi = MeMedicion48.Emprcodi ?? default(int),
                        Emprnomb = MeMedicion48.Emprnomb,
                        Gruponomb = MeMedicion48.Gruponomb,
                        Grupocodi = MeMedicion48.Grupocodi ?? default(int),
                        Osinergcodi = MeMedicion48.Osinergcodi,
                        Meditotal = decimal.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[2])[3])
                    });
                }
            }

            string titulo1 = "DESVIACIONES ENTRE LA ENERGIA PROGRAMADA Y EJECUTADA EN LAS CENTRALES ", name = "Energia Ejecutada (GWh)", name1 = "Energia Programada (GWh)", name2 = "Participacion (%)",
                type = "column", type1 = "column", type2 = "spline";
            model.Grafico = grafico;
            model.Grafico.SeriesData = new decimal?[3][];
            if (lista.Count > 0)
            {
                string text = string.Empty;
                switch (param)
                {
                    case 2: text = "HIDROELECTRICAS"; break;
                    case 3: text = "TERMOELECTRICAS"; break;
                    case 4: text = "SOLARES"; break;
                    case 5: text = "EOLICAS"; break;
                }
                model.Grafico.TitleText = titulo1 + text;

                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.YAxixTitle = new List<string>();

                model.Grafico.YAxixTitle.Add("Energia (MWh)");
                model.Grafico.YAxixTitle.Add("Variacion (%)");

                // Obtener lista de intervalos categoria del grafico 
                var empresas = lista.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).Distinct().ToList();
                model.Grafico.XAxisCategories = empresas.Select(x => x.Emprnomb).ToList();
                int totalIntervalos = empresas.Count;

                model.Grafico.Series = new List<RegistroSerie>();
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());

                //inicio llenado de las series
                model.Grafico.Series[0].Name = name;
                model.Grafico.Series[0].Type = type;
                model.Grafico.Series[0].YAxis = 0;

                model.Grafico.Series[1].Name = name1;
                model.Grafico.Series[1].Type = type1;
                model.Grafico.Series[1].YAxis = 0;

                model.Grafico.Series[2].Name = name2;
                model.Grafico.Series[2].Type = type2;
                model.Grafico.Series[2].YAxis = 1;

                model.Grafico.SeriesData[0] = new decimal?[totalIntervalos];
                model.Grafico.SeriesData[1] = new decimal?[totalIntervalos];
                model.Grafico.SeriesData[2] = new decimal?[totalIntervalos];
                int e = 0;
                for (int x = 0; x < empresas.Count; x++)
                {
                    decimal ejec = 0, prog = 0, vari = 0;
                    switch (param)
                    {
                        case 2:
                            eje = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiMedidores) && (c.Fenergcodi == ConstantesSioSein.fenercodiAgua)).ToList();
                            pro = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiProgDiario) && (c.Fenergcodi == ConstantesSioSein.fenercodiAgua)).ToList();
                            break;
                        case 3:
                            eje = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiMedidores) && (c.Fenergcodi == ConstantesSioSein.fenercodiGas ||
                    c.Fenergcodi == ConstantesSioSein.fenercodiDiesel || c.Fenergcodi == ConstantesSioSein.fenercodiResi || c.Fenergcodi == ConstantesSioSein.fenercodiCarb || c.Fenergcodi == ConstantesSioSein.fenercodiBaga ||
                    c.Fenergcodi == ConstantesSioSein.fenercodiBiogas || c.Fenergcodi == ConstantesSioSein.fenercodiResi500 || c.Fenergcodi == ConstantesSioSein.fenercodiResi6)).ToList();
                            pro = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiProgDiario) && (c.Fenergcodi == ConstantesSioSein.fenercodiGas ||
                    c.Fenergcodi == ConstantesSioSein.fenercodiDiesel || c.Fenergcodi == ConstantesSioSein.fenercodiResi || c.Fenergcodi == ConstantesSioSein.fenercodiCarb || c.Fenergcodi == ConstantesSioSein.fenercodiBaga ||
                    c.Fenergcodi == ConstantesSioSein.fenercodiBiogas || c.Fenergcodi == ConstantesSioSein.fenercodiResi500 || c.Fenergcodi == ConstantesSioSein.fenercodiResi6)).ToList();
                            break;
                        case 4:
                            eje = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiMedidores) && (c.Fenergcodi == ConstantesSioSein.fenercodiSolar)).ToList();
                            pro = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiProgDiario) && (c.Fenergcodi == ConstantesSioSein.fenercodiSolar)).ToList();
                            break;
                        case 5:
                            eje = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiMedidores) && (c.Fenergcodi == ConstantesSioSein.fenercodiEolic)).ToList();
                            pro = lista.Where(c => c.Emprcodi == empresas[x].Emprcodi && c.Lectcodi == int.Parse(ConstantesSioSein.LectCodiProgDiario) && (c.Fenergcodi == ConstantesSioSein.fenercodiEolic)).ToList();
                            break;
                    }

                    if (eje.Count > 0)
                    {
                        foreach (var d in eje) { ejec += (decimal)d.Meditotal; lista.Remove(d); }
                    }
                    if (pro.Count > 0)
                    {
                        foreach (var d in pro) { prog += (decimal)d.Meditotal; lista.Remove(d); }
                    }
                    if (ejec != 0 && prog != 0)
                    {
                        vari = (prog - ejec) / ejec;
                    }

                    model.Grafico.SeriesData[0][e] = ejec;
                    model.Grafico.SeriesData[1][e] = prog;
                    model.Grafico.SeriesData[2][e] = vari;
                    e++;
                }
            }
            return model;
        }

        #endregion

        #endregion

        #region TABLA 07: TRANSFERENCIAS DE ENERGÍA (TREN)

        #region Verificacion

        /// <summary>
        /// index transferencia de energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult TransferenciaEnergia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 7);

            return View(model);
        }

        /// <summary>
        /// carga lista transferencias de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaTransferenciaEnergia(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            var lstTransferenciaSGOCOES = _servicioSiosein.ObtenerValorTotalTransferenciaAgrp(periodo, null);
            var lstTransferenciaTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie07, periodo);

            model.NRegistros = lstTransferenciaSGOCOES.Count;
            model.Resultado = _servicioSiosein.GenerarRHtmlReporteTransferenciaEnergia(lstTransferenciaSGOCOES, lstTransferenciaTXT);
            model.Resultado2 = _servicioSiosein.GenerarRHtmlTransferenciaEnergiaConsolidado(lstTransferenciaSGOCOES.Where(x => x.EsNuevoRegistro).ToList());

            return Json(model);
        }

        /// <summary>
        /// carga grafico transferencia energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGraficoTransferenciaEnergia(string mesAnio, int idEmpresa)
        {
            var model = new SioseinModel();

            DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            var lstTransferenciaSGOCOES = _servicioSiosein.ObtenerValorTotalTransferenciaAgrp(periodo, idEmpresa);
            model.Grafico = _servicioSiosein.GenerarGWebTransgerenciaEnergia(lstTransferenciaSGOCOES);

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de transferencia de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaTransferenciaEnergia(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<ValorTransferenciaDTO> lstTransferenciaSGOCOES = _servicioSiosein.ListarDatosTxtTablaPrieTREN(periodo, null);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstTransferenciaSGOCOES)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Barrcodi = dataEnv.BarrCodi,
                        Emprcodi = dataEnv.Emprcodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            CodigoEmpresa = dataEnv.Emprcodosinergmin,
                            CodigoBarra = dataEnv.Osinergcodi,
                            CodigoEntregaRetiro = dataEnv.ValoTranCodEntRet,
                            EnergiaActivaEntrega = dataEnv.VTTotalEnergiaEnt ?? 0,
                            EnergiaActivaRetiro = dataEnv.VTTotalEnergiaRet ?? 0
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie07, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie07, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion de transferencia de energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionTransferenciaEnergia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 7);
            model.Titulo = "Difusión web - Transferencias de Energía";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion transferencia de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionTransferenciaEnergia(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaPeriodo.GetLastDateOfMonth();

                List<SioDatoprieDTO> lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie07, fechaPeriodo);
                var lstEmpresa = lstDatosPrieTXT.Select(x => x.Emprcodi).Distinct();
                List<SiEmpresaDTO> lstEmpresas = _servicioIEOD.ListarEmpresasXID(string.Join(",", lstEmpresa));
                model.Resultado = _servicioSiosein.GenerarRHtmlDifusionTransferenciaEnergia(lstDatosPrieTXT, lstEmpresas);

                model.Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein.GenerarGWebDifusionTransferenciaEnergia(lstDatosPrieTXT, lstEmpresas),
                    _servicioSiosein.GenerarGWebDifusionTransfEnergiaEmpresaPrivadaPublica(lstDatosPrieTXT, lstEmpresas)
                };
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 08: PAGOS POR VALORIZACIÓN DE TRANSFERENCIAS DE POTENCIA (TRPP)

        #region Verificacion

        /// <summary>
        /// index pago transferencia potencia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult PagoTransferenciaPotencia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 8);

            return View(model);
        }

        /// <summary>
        /// carga lista pago transferencia potencia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaPagoTransferenciaPotencia(string mesAnio)
        {
            SioseinModel ListaModel = new SioseinModel();
            var model = new SioseinModel();

            DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            List<VtpEmpresaPagoDTO> listaEmpresaPago = _servicioSiosein.ListarDatosTxtTablaPrieTRPP(periodo);
            List<SioDatoprieDTO> listaEmpresaPagoTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie08, periodo);

            model.NRegistros = listaEmpresaPago.Where(x => x.EsNuevoRegistro).ToList().Count;
            model.Resultado = this._servicioSiosein.GenerarRHtmlPagoTransferenciaPotencia(listaEmpresaPago, listaEmpresaPagoTXT);
            model.Resultado2 = _servicioSiosein.GenerarRHtmlPagoTransferenciaPotenciaConsolidado(listaEmpresaPago.Where(x => x.EsNuevoRegistro).ToList());

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de transferencia de potencia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaTransferenciaPotencia(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<VtpEmpresaPagoDTO> listaEmpresaPago = _servicioSiosein.ListarDatosTxtTablaPrieTRPP(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in listaEmpresaPago)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Emprcodi = dataEnv.Emprcodipago,
                        Emprcodi2 = dataEnv.Emprcodicobro,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            CodigoEmpresaEntrega = dataEnv.Emprcodosinergminpago?.Trim(),
                            CodigoEmpresaRecibe = dataEnv.Emprcodosinergmincobro?.Trim(),
                            ValorTransferencia = dataEnv.Potepmonto
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie08, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie08, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion de pago transferencia de potencia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionPagoTransferenciaPotencia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 8);
            model.Titulo = "Difusión web - Transferencias de Potencia";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion pago de transferencia potencia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionPagoTransferenciaPotencia(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            DateTime fechaFin = fechaPeriodo.GetLastDateOfMonth();

            List<SioDatoprieDTO> lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie08, fechaPeriodo);
            List<SiEmpresaDTO> lstEmpresas = _servicioSiosein.ObtenerEmpresasDatosPrieTXT(lstDatosPrieTXT);

            Func<SioDatoprieDTO, decimal> selectorSum = x => x.SioReporte.ValorTransferencia;
            model.Resultado = this._servicioSiosein.GenerarRHtmlDifusionPagoTransferencia(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE POTENCIA POR EMPRESA (Miles de Soles)", selectorSum);

            model.Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein.GenerarGWebDifusionPagoTransferencia(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE POTENCIA POR EMPRESA (Soles)", selectorSum),
                    _servicioSiosein.GenerarGWebDifusionPagoTransferenciaEmpresaPrivadaPublica(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE POTENCIA POR EMPRESA (Soles)", selectorSum)
                };

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 09: BALANCE POR EMPRESAS (COMP)

        #region Excel web - Carga de datos Tabla 09

        /// <summary>
        /// Index Carga de datos Tabla 09
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult IndexExcelWebCOMP(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = new SioseinTblCOMPModel();

            DateTime dt = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            model.Mes = dt.ToString(Constantes.FormatoMesAnio);

            return View(model);
        }

        /// <summary>
        /// Listar Tabla COMP
        /// </summary>
        /// <param name="esDefault"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarTablaCOMP(int esDefault, string fechaPeriodo)
        {
            SioseinTblCOMPModel model = new SioseinTblCOMPModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                //Generar handson
                model.ListaData = _servicioSiosein.ListarDatosExcelwebTablaCOMP(fecha, esDefault == 1, out string usuarioEnvio, out string fechaEnvio);
                model.UsuarioEnvio = usuarioEnvio;
                model.FechaEnvio = fechaEnvio;
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar Tabla COMP
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarTablaCOMP(string fechaPeriodo, List<SioPrieCompDTO> listaData)
        {
            SioseinTblCOMPModel model = new SioseinTblCOMPModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (listaData == null || !listaData.Any())
                    throw new ArgumentException("Debe ingresar información");

                DateTime fecha = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                _servicioSiosein.GuardarDatosExcelwebTablaCOMP(fecha, listaData, base.UserName);

                model.Resultado = "exito";
                model.StrMensaje = "El envío se realizó con éxito.";
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = "error";
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Plantilla Tabla COMP
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarPlantillaTablaCOMP(string fechaPeriodo, List<SioPrieCompDTO> listaData)
        {
            SioseinTblCOMPModel model = new SioseinTblCOMPModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(fechaPeriodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombArchivo = string.Format("ExcelWeb_COMP_{0}", fecha.ToString(ConstantesAppServicio.FormatoMesAnio));

                string nombreArchivoCompleto = _servicioSiosein.GenerarExcelwebTablaCOMP(rutaBase, nombArchivo, fecha, listaData);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Upload Tabla COMP
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadTablaCOMP(FormCollection formCollection)
        {
            SioseinTblCOMPModel model = new SioseinTblCOMPModel();

            try
            {
                base.ValidarSesionJsonResult();

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                //Generar handson
                model.ListaData = _servicioSiosein.ObtenerDatosExcelwebTablaCOMP(stremExcel);
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #endregion

        #region VERIFICACIÓN

        /// <summary>
        /// index balance de empresas
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult BalanceEmpresas(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 9);

            return View(model);
        }

        /// <summary>
        /// carga lista balance empresas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaBalanceEmpresas(string mesAnio)
        {
            var model = new SioseinModel();
            DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            var lstBalanceEmpresa = _servicioSiosein.ListarDatosTxtTablaPrieCOMP(periodo);

            model.NRegistros = lstBalanceEmpresa.Count;
            model.Resultado = _servicioSiosein.GenerarRHtmlReporteBalanceEmpresas(lstBalanceEmpresa);

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de balance de empresas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaBalanceEmpresas(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var listaBalanceEmpresas = this._servicioSiosein.ListarDatosTxtTablaPrieCOMP(periodo);//SIOSEIN-PRIE-2021

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in listaBalanceEmpresas) //SIOSEIN-PRIE-2021
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            //SIOSEIN-PRIE-2021
                            CodigoEmpresa = dataEnv.Emprcodosinergmin,
                            TransferenciasEnergia = dataEnv.Transfval ?? 0,
                            ProrrateoSaldoResultante = dataEnv.Prorsalresul ?? 0,
                            RetirosSinContratosDist = dataEnv.Retsincontdist ?? 0,
                            RetirosSinContratosULibres = dataEnv.Retsincontlib ?? 0,
                            ValorBajaEficienciaCombustible = dataEnv.Valbajeficomb ?? 0,
                            ValorRegulacionFrecuencia = dataEnv.Valregufrec ?? 0,
                            ValorOperacionInflexibilidadOperativa = dataEnv.Valopeinflexop ?? 0,
                            ValorPorPruebasAleatorias = dataEnv.Valpruebaleat ?? 0,
                            SaldoMesesAnteriores = dataEnv.Saldomesant ?? 0,
                            OtrasCompensaciones = dataEnv.Otrascompens ?? 0
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie09, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie09, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.IdEnvio = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion balance de empresas
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionBalanceEmpresas(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                int anio = Int32.Parse(periodo.Substring(3, 4));
                int mes = Int32.Parse(periodo.Substring(0, 2));
                model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");
            }

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion balance de empresas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionBalanceEmpresas(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();

            var lista1 = new List<SioDatoprieDTO>();

            DateTime dfecIniMesAnt = DateTime.Now, dfecFinMesAnt = DateTime.Now, dfecIniMesAct = DateTime.Now, dfecFinMesAct = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecIniMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(-1);
                dfecFinMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(-1);

                dfecIniMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfecFinMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionBalanceEmpresas(ConstantesSioSein.Prie09, dfecIniMesAct, dfecFinMesAct, idEmpresa);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionBalanceEmpresas(lista1);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion balance de empresas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionBalanceEmpresas(string mesAnio, string idEmpresa)
        {
            SioseinModel model = new SioseinModel();

            var lista1 = new List<SioDatoprieDTO>();

            DateTime dfecIniMesAnt = DateTime.Now, dfecFinMesAnt = DateTime.Now, dfecIniMesAct = DateTime.Now, dfecFinMesAct = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecIniMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(-1);
                dfecFinMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(-1);

                dfecIniMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfecFinMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionBalanceEmpresas(ConstantesSioSein.Prie09, dfecIniMesAct, dfecFinMesAct, idEmpresa);

            if (lista1.Count > 0)
            {
                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();
                model.Serie4 = new List<decimal>();
                model.Serie5 = new List<decimal>();
                model.Serie6 = new List<decimal>();

                foreach (var tem in lista1)
                {
                    var _datos = tem.Dprievalor.Split(ConstantesSioSein.SplitPrie[0]);

                    decimal retiro = Convert.ToDecimal(_datos[4]) + Convert.ToDecimal(_datos[5]);
                    decimal compensacion = Convert.ToDecimal(_datos[6]) + Convert.ToDecimal(_datos[7]) + Convert.ToDecimal(_datos[8]) + Convert.ToDecimal(_datos[11]);
                    decimal inter = 0;

                    model.Categoria.Add(_datos[1]);
                    model.Serie1.Add(Convert.ToDecimal(_datos[2]));
                    model.Serie2.Add(Convert.ToDecimal(_datos[3]));
                    model.Serie3.Add(retiro);
                    model.Serie4.Add(compensacion);
                    model.Serie5.Add(Convert.ToDecimal(_datos[10]));
                    model.Serie6.Add(inter);

                }
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 10: PAGOS POR VALORIZACIÓN DE TRANSFERENCIAS DE ENERGÍA (PTRA)

        #region Verificación
        /// <summary>
        /// index pago transferencia energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult PagoTransferenciaEnergia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 10);

            return View(model);
        }

        /// <summary>
        /// carga lista pago de transferencia energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaPagoTransferenciaEnergia(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime fecha = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            List<EmpresaPagoDTO> lstEmpresaPago = _servicioSiosein.ListarDatosTxtTablaPriePTRA(fecha);
            List<SioDatoprieDTO> lstEmpresaPagoTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie10, fecha);

            model.NRegistros = lstEmpresaPago.Where(x => x.EsNuevoRegistro).ToList().Count();
            model.Resultado = _servicioSiosein.GenerarRHtmlPagoTransferenciaEnergia(lstEmpresaPago, lstEmpresaPagoTXT);
            model.Resultado2 = _servicioSiosein.GenerarRHtmlPagoTransferenciaEnergiaConsolidado(lstEmpresaPago.Where(x => x.EsNuevoRegistro).ToList());

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de valor transferencia de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaValorTransferenciaEnergia(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<EmpresaPagoDTO> lstEmpresaPago = _servicioSiosein.ListarDatosTxtTablaPriePTRA(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstEmpresaPago)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Emprcodi = dataEnv.EmpCodi,
                        Emprcodi2 = dataEnv.EmpPagoCodEmpPago,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            CodigoEmpresaEntrega = dataEnv.Emprcodosinergmin?.Trim(),
                            CodigoEmpresaRecibe = dataEnv.Emprcodosinergminpago?.Trim(),
                            Valor = dataEnv.EmpPagoMonto
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie10, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie10, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion pago transferencia de energia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionPagoTransferenciaEnergia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 10);
            model.Titulo = "Difusión web - Pago Transferencia de Energía";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion pago transferencia de energia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionPagoTransferenciaEnergia(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            DateTime fechaFin = fechaPeriodo.GetLastDateOfMonth();

            List<SioDatoprieDTO> lstDatosPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie10, fechaPeriodo);
            List<SiEmpresaDTO> lstEmpresas = _servicioSiosein.ObtenerEmpresasDatosPrieTXT(lstDatosPrieTXT);

            Func<SioDatoprieDTO, decimal> selectorSum = x => x.SioReporte.Valor;
            model.Resultado = this._servicioSiosein.GenerarRHtmlDifusionPagoTransferencia(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE ENERGÍA POR EMPRESA (Miles de Soles)", selectorSum);
            model.Graficos = new List<GraficoWeb>()
                {
                    _servicioSiosein.GenerarGWebDifusionPagoTransferencia(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE ENERGÍA POR EMPRESA (Soles)",selectorSum),
                    _servicioSiosein.GenerarGWebDifusionPagoTransferenciaEmpresaPrivadaPublica(lstDatosPrieTXT, lstEmpresas, "TRANSFERENCIAS DE ENERGÍA POR EMPRESA (Soles)",selectorSum)
                };

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 11: COMPENSACIÓN INGRESO TARIFARIO (CITA)

        #region Verificación

        /// <summary>
        /// index compensacion ingreso tarifario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CompensacionIngresoTarifario(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 11);

            return View(model);
        }

        public JsonResult CargarListaCompensacionIngresoTarifario(string mesAnio)
        {
            var model = new SioseinModel();

            DateTime fecha = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            List<VtpIngresoTarifarioDTO> lstIgresoTarifario = _servicioSiosein.ListarDatosTxtTabla11Prie(fecha);
            List<SioDatoprieDTO> lstEmpresaPagoTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie11, fecha);

            model.NRegistros = lstIgresoTarifario.Count();
            model.Resultado = this._servicioSiosein.GenerarRHtmlCompensacionIngresoTarifario(lstIgresoTarifario, lstEmpresaPagoTXT);

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de compensacion de ingreso tarifario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCompensacionIngresoTarifario(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<VtpIngresoTarifarioDTO> lstIgresoTarifario = _servicioSiosein.ListarDatosTxtTabla11Prie(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstIgresoTarifario)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Emprcodi = dataEnv.Emprcodingpot,
                        Emprcodi2 = dataEnv.Emprcodiping,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            CodigoEmpresaEntrega = dataEnv.Emprcodosinergminingpot?.Trim(),
                            CodigoEmpresaRecibe = dataEnv.Emprcodosinergminping?.Trim(),
                            Valor = dataEnv.ImporteTotal
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie11, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie11, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusón

        /// <summary>
        /// index difusion compensacion de ingreso tarifario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCompensacionIngresoTarifario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 11);
            model.Titulo = "Difusión web - Compensacion de Ingreso Tarifario";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion compensacion ingreso tarifario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCompensacionIngresoTarifario(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstEmpresaPagoTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie11, fechaPeriodo);
                List<SioDatoprieDTO> lstEmpresaPagoTXTAnt = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie11, fechaPeriodo.AddYears(-1));
                List<VtpIngresoTarifarioDTO> lstIgresoTarifario = _servicioSiosein.ListarDatosTxtTabla11Prie(fechaPeriodo);

                var lstEmpresa = lstEmpresaPagoTXT.Select(x => x.Emprcodi).Distinct().ToList();
                List<SiEmpresaDTO> lstEmpresas = _servicioIEOD.ListarEmpresasXID(string.Join(",", lstEmpresa));

                model.NRegistros = lstIgresoTarifario.Count();
                model.Resultado = this._servicioSiosein.GenerarRHtmlCompensacionIngresoTarifario(lstIgresoTarifario, lstEmpresaPagoTXT);
                model.Grafico = _servicioSiosein.GenerarGWebCompensacionIngresoTarifario(lstEmpresaPagoTXT, lstEmpresaPagoTXTAnt, lstEmpresas);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 12: COMPENSACIÓN POR TRANSMISIÓN PCSPT Y PCSGT (PEAJ)

        #region Verificación

        /// <summary>
        /// index transmision PCSPTyPCSGT
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult TransmisionPCSPTyPCSGT(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 12);

            return View(model);
        }

        /// <summary>
        /// carga lista transmision PCSPTyPCSGT
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaTransmisionPCSPTyPCSGT(string mesAnio)
        {

            var model = new SioseinModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstCompensacion = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie12, fecha);
                List<VtpPeajeEmpresaPagoDTO> lstPeajePago = _servicioSiosein.ListarDatosTxtTablaPriePEAJ(fecha);

                model.NRegistros = lstCompensacion.Count();
                model.Resultado = _servicioSiosein.GenerarRhtmlTransmisionPCSPTyPCSGT(lstPeajePago, lstCompensacion);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
                model.Mensaje = "Ocurrio un error";
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de transmision PCSPTyPCSGT
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaTransmisionPCSPTyPCSGT(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<VtpPeajeEmpresaPagoDTO> lstPeajePago = _servicioSiosein.ListarDatosTxtTablaPriePEAJ(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in lstPeajePago)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Emprcodi = dataEnv.Emprcodipeaje,
                        Emprcodi2 = dataEnv.Emprcodicargo,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = $"{periodo.Year}{periodo.Month:D2}",
                            CodigoEmpresaEntrega = dataEnv.Emprcodosinergminpeaje?.Trim(),
                            CodigoEmpresaRecibe = dataEnv.Emprcodosinergmincargo?.Trim(),
                            CodigoClasificacTransmision = dataEnv.Pingtipo,
                            Valor = dataEnv.PeajeTotal
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie12, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie12, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion transmision PCSPTyPCSGT
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionTransmisionPCSPTyPCSGT(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 12);
            model.Titulo = "Difusión web - Transmision PCSPT y PCSGT";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion transmision PCSPTyPCSGT
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionTransmisionPCSPTyPCSGT(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstCompensacionTXTActual = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie12, fechaPeriodo);
                List<SioDatoprieDTO> lstCompensacionTXTAnterior = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie12, fechaPeriodo.AddYears(-1));

                List<VtpPeajeEmpresaPagoDTO> lstPeajePago = _servicioSiosein.ListarDatosTxtTablaPriePEAJ(fechaPeriodo);
                var lstEmpresa = lstCompensacionTXTActual.Select(x => x.Emprcodi).Distinct().ToList();
                List<SiEmpresaDTO> lstEmpresas = _servicioIEOD.ListarEmpresasXID(string.Join(",", lstEmpresa));

                model.NRegistros = lstCompensacionTXTActual.Count();
                model.Resultado = _servicioSiosein.GenerarRhtmlTransmisionPCSPTyPCSGT(lstPeajePago, lstCompensacionTXTActual);
                model.Grafico = _servicioSiosein.GenerarGWebTransmisionPCSPTyPCSGT(lstCompensacionTXTActual, lstCompensacionTXTAnterior, lstEmpresas);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 13: RECAUDACIÓN POR PEAJES CALCULADOS POR CONEXIÓN Y TRANSMISIÓN (RECA)

        #region VERIFICACION

        /// <summary>
        /// index recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult RecaudacionPeajesConexionTransmision(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 13);

            return View(model);
        }

        /// <summary>
        /// carga lista recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaRecaudacionPeajesConexionTransmision(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lista = _servicioSiosein.ListarDatosTxtTablaPrieRECA(fecha);

                model.NRegistros = lista.Count();
                model.Resultado = _servicioSiosein.GenerarRHtmlRecaudacionPeajesConexionTransmision(lista);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
                model.Mensaje = "Ocurrio un error";
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista recaudacion peajes conexion y transmision consolidado
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public SioseinModel CargarListaRecaudacionPeajesConexionTransmisionConsolidado(List<VtpPeajeEmpresaPagoDTO> Lista)
        {
            var model = new SioseinModel();

            model.Resultado = this._servicioSiosein.ListarReporteRecaudacionPeajesConexionTransmisionConsolidado(Lista);

            return model;
        }

        /// <summary>
        /// envio de datos a tablas prie de recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaRecaudacionPeajesConexionTransmision(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lista = _servicioSiosein.ListarDatosTxtTablaPrieRECA(fecha);

                int retorno = _servicioSiosein.GuardarListaRecaudacionPeajesConexionTransmision(mesAnio, User.Identity.Name, lista);
                model.IdEnvio = retorno;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionRecaudacionPeajesConexionTransmision(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                int anio = Int32.Parse(periodo.Substring(3, 4));
                int mes = Int32.Parse(periodo.Substring(0, 2));
                model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");
            }

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionRecaudacionPeajesConexionTransmision(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();

            DateTime dfecIniMesAnt = DateTime.Now, dfecFinMesAnt = DateTime.Now, dfecIniMesAct = DateTime.Now, dfecFinMesAct = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecIniMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(-1);
                dfecFinMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(-1);

                dfecIniMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfecFinMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionRecaudacionPeajesConexionTransmision(ConstantesSioSein.Prie13, dfecIniMesAct, dfecFinMesAct, idEmpresa);


            model.Resultado = this._servicioSiosein.ListarReporteDifusionRecaudacionPeajesConexionTransmision(lista1);


            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion recaudacion peajes conexion y transmision
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionRecaudacionPeajesConexionTransmision(string mesAnio, string idEmpresa)
        {
            SioseinModel model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();
            var listaFinal = new List<SioDatoprieDTO>();

            DateTime dfecIniMesAnt = DateTime.Now, dfecFinMesAnt = DateTime.Now, dfecIniMesAct = DateTime.Now, dfecFinMesAct = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecIniMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfecFinMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            lista1 = this._servicioSiosein.GetListaDifusionRecaudacionPeajesConexionTransmision(ConstantesSioSein.Prie13, dfecIniMesAct, dfecFinMesAct, idEmpresa);

            if (lista1.Count > 0)
            {
                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();
                model.Serie4 = new List<decimal>();
                model.Serie5 = new List<decimal>();
                model.Serie6 = new List<decimal>();
                model.Serie7 = new List<decimal>();
                model.Serie8 = new List<decimal>();
                model.Serie9 = new List<decimal>();


                foreach (var temp in lista1)
                {
                    var _datos = temp.Dprievalor.Split(ConstantesSioSein.SplitPrie[2]);

                    model.Categoria.Add(_datos[1]);
                    model.Serie1.Add(Convert.ToDecimal(_datos[2]));
                    model.Serie2.Add(Convert.ToDecimal(_datos[3]));
                    model.Serie3.Add(Convert.ToDecimal(_datos[4]));
                    model.Serie4.Add(Convert.ToDecimal(_datos[5]));
                    model.Serie5.Add(Convert.ToDecimal(_datos[6]));
                    model.Serie6.Add(Convert.ToDecimal(_datos[7]));
                    model.Serie7.Add(Convert.ToDecimal(_datos[8]));
                    model.Serie8.Add(Convert.ToDecimal(_datos[9]));
                    model.Serie9.Add(Convert.ToDecimal(_datos[10]));
                }
            }


            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 14: COSTOS DE OPERACIÓN EJECUTADOS (COST)

        #region Verificación

        /// <summary>
        /// index costos de operacion ejecutados
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosOperacionEjecutados(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 14);

            return View(model);
        }

        /// <summary>
        /// carga lista costos de operacion ejecutados consolidado
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="componentes"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaCostosOperacionEjecutadosConsolidado(string mesAnio)
        {
            SioseinModel model = new SioseinModel();

            var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            var listaDataReport = _servicioSiosein.ListarDatosTxtTablaPrieCOST(fechaInicio);

            model.Resultado = _servicioSiosein.GenerarRHtmlCostosOperacionEjecutadosConsolidado(listaDataReport);
            model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionEjecutadosConsolidado(listaDataReport);

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de costos de operacion ejecutados
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosOperacionEjecutados(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lCosto = this._servicioSiosein.ListarDatosTxtTablaPrieCOST(fechaInicio);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lCosto)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = item.Medifecha.ToString(ConstantesSioSein.FormatAnioMesDia),
                            Valor = item.Ejecutado ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie14, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie14, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion costos de operacion ejecutados
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosOperacionEjecutados(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 14);
            model.Titulo = "Difusión web - Costos de Operacion Ejecutados";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos de operacion ejecutados
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosOperacionEjecutados(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstCostoOperacionEjec = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie14, fechaPeriodo);
                List<SioDatoprieDTO> lstCostoOperacionProg = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie34, fechaPeriodo);

                var lstResultado = _servicioSiosein.ObtenerReporteCostoOperacionEjecutado(lstCostoOperacionEjec, lstCostoOperacionProg);

                model.NRegistros = lstResultado.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCostosOperacionEjecutados(lstResultado);
                model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionEjecutados(lstResultado, "EJECUTADOS Y PROGRAMADOS");
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 15: HORAS DE OPERACIÓN (HOPE)

        #region Verificación

        /// <summary>
        /// index horas de operacion
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult HorasOperacion(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 15);

            return View(model);
        }

        /// <summary>
        /// carga lista horas de operacion
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaHorasOperacion(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<EveHoraoperacionDTO> listaHoraOpe = _servicioSiosein.ListarDatosTxtTablaPrieHOPE(fechaInicio);

                model.NRegistros = listaHoraOpe.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlHorasOperacion(listaHoraOpe);
                model.Grafico = _servicioSiosein.GenerarGWebHorasOperacion(listaHoraOpe);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista horas de operacion consolidado
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaHorasOperacionConsolidado(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<EveHoraoperacionDTO> listaHoraOpe = _servicioSiosein.ObtenerListaHOPTermica(fechaInicio, fechaFin);
                List<EveHoraoperacionDTO> listadata = _servicioSiosein.ObtenerListaDataReportHOPConsolidado(listaHoraOpe);

                model.NRegistros = listaHoraOpe.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlHorasOperacionConsolidado(listadata, fechaInicio);
                model.Grafico = this._servicioSiosein.GenerarGWebHorasOperacionConsolidado(listadata, fechaInicio);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de horas de operacion
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaHorasOperacion(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<EveHoraoperacionDTO> listaHoraOpe = _servicioSiosein.ListarDatosTxtTablaPrieHOPE(periodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var dataEnv in listaHoraOpe)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Grupocodi = dataEnv.Grupocodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            CodigoModoOperacion = dataEnv.Osinergcodi,
                            SumaHorasOperacion = dataEnv.Duracion ?? 0,
                            TipoOperacion = $"{dataEnv.SubcausaOsi}",
                            SistemaElectrico = $"{dataEnv.Hopsaislado:D2}",
                            LimTransm = dataEnv.HoplimtransDesc,
                            Observacion = dataEnv.Hopdesc
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie15, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie15, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion horas de operacion
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionHorasOperacion(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 15);
            model.Titulo = "Difusión web - Horas de Operacion";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion horas de operacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionHorasOperacion(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstHorasOperacion = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie15, fechaPeriodo);
                var lstTipooperacionid = lstHorasOperacion.Select(x => x.SioReporte.TipoOperacion).Distinct();
                List<EveSubcausaeventoDTO> lstTipoOperacion = _servicioSiosein.GetEveSubcausaeventoByIds(string.Join(",", lstTipooperacionid));
                model.Resultado = _servicioSiosein.GenerarRHtmlHorasOperacion(lstHorasOperacion, lstTipoOperacion);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion horas de operacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionHorasOperacion(string idEmpresa, string mesAnio)
        {
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            List<SioDatoprieDTO> listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie15, dfechaIni);

            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionHorasOperacion(listDatoPrie, dfechaIni, dfechaFin);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionHorasOperacion(List<SioDatoprieDTO> data, DateTime dfechaIni, DateTime dfechaFin)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<EveHoraoperacionDTO> lista = this._servicioSiosein.ObtenerListaHOPTermica(dfechaIni, dfechaFin);

            //foreach (var upd in data)
            //{
            //    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();
            //    entity.Hophorfin = upd.Dpriefechadia;
            //    string[] arr__ = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[2]);
            //    if (arr__.Count() > 0)
            //    {
            //        //entity.Grupocodi = int.Parse(arr__[0]);
            //        entity.Subcausadesc = arr__[1];
            //        entity.Subcausacodi = int.Parse(arr__[2]);
            //        entity.Hopsaislado = int.Parse(arr__[3]);
            //        entity.Unidad = int.Parse(arr__[4]);

            //    }
            //    lista.Add(entity);
            //}

            string titulo1 = "N° DE HORAS DE OPERACION POR TIPO DE OPERACION", type = "column";
            model.Grafico = grafico;

            if (lista.Count > 0)
            {
                model.Grafico.YaxixTitle = "N° Horas de Operacion";
                model.Grafico.TitleText = titulo1;

                model.Grafico.XAxisCategories = new List<string>();

                // Obtener lista de intervalos categoria del grafico   
                for (int d = 1; d <= dfechaFin.Day; d++)
                {
                    model.Grafico.XAxisCategories.Add(d.ToString());
                }

                var series = lista.GroupBy(x => new { x.Subcausacodi, x.Subcausadesc }).Select(x => new { x.Key.Subcausacodi, x.Key.Subcausadesc }).Distinct().ToList();
                model.Grafico.Series = new List<RegistroSerie>();
                for (int s = 0; s < series.Count; s++)
                {
                    model.Grafico.SeriesData = new decimal?[s + 1][];
                    model.Grafico.Series.Add(new RegistroSerie());
                }

                for (var i = 0; i < series.Count; i++)
                {
                    var datos = lista.Where(x => x.Subcausacodi == series[i].Subcausacodi).ToList();
                    model.Grafico.Series[i].Name = series[i].Subcausadesc;
                    model.Grafico.Series[i].Type = type;
                    model.Grafico.Series[i].YAxis = 0;

                    model.Grafico.SeriesData[i] = new decimal?[dfechaFin.Day];
                    for (int j = 1; j <= dfechaFin.Day; j++)
                    {
                        decimal valor = 0;
                        var dat = datos.Where(x => x.Hophorfin.Value.Day == j).ToList();
                        foreach (var arr in dat) { valor += arr.Unidad; }
                        model.Grafico.SeriesData[i][j - 1] = valor;
                    }
                }
            }
            return model;
        }

        #endregion

        #endregion

        #region TABLA 16: ENERGIA NO SUMINISTRADA EJECUTADA MENSUAL (ENSE)

        #region Verificacion

        /// <summary>
        /// index energia no suministrada mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult EnergSumiEjecMensual(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 16);

            return View(model);
        }

        /// <summary>
        /// carga lista energia no suministrada mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaEnergSumiEjecMensual(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<EveEventoDTO> listaEventosInterrup = _servicioSiosein.ObtenerEventosConInterrupciones(fechaInicio, fechaFin);
                List<EveEventoDTO> listaDataRep = _servicioSiosein.ObtenerDataReportEnergiaNoSuministrada(listaEventosInterrup);
                model.NRegistros = listaDataRep.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlEnergSumiEjecMensual(listaDataRep);
                model.Grafico = _servicioSiosein.GenerarGWebEnergSumiEjecMensual(listaDataRep);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista energia no suministrada mensual detalle
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaEnergSumiEjecMensualDetalle(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<EveEventoDTO> listaEventosInterrup = _servicioSiosein.ObtenerEventosConInterrupciones(fechaInicio, fechaFin);
                model.NRegistros = listaEventosInterrup.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlEnergSumiEjecMensualDetalle(listaEventosInterrup);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de energia no suministrada mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult ValidarEnergiaNoSuministrada(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var periodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = periodo.GetLastDateOfMonth();

                List<EveEventoDTO> listaEventosInterrup = _servicioSiosein.ObtenerEventosConInterrupciones(periodo, fechaFin);
                List<EveEventoDTO> listaDataRep = _servicioSiosein.ObtenerDataReportEnergiaNoSuministrada(listaEventosInterrup);

                var listaDatosPrie = new List<SioDatoprieDTO>();

                SioDatoprieDTO entityDet = new SioDatoprieDTO
                {
                    Dprieperiodo = periodo,
                    Dprieusuario = User.Identity.Name,
                    Dpriefecha = DateTime.Now,
                    SioReporte = new SioReporteDTO()
                    {
                        Periodo = $"{periodo.Year}{periodo.Month:D2}",
                        ValorDeficit = listaDataRep.Sum(x => x.EnergiaNoSuministrada ?? 0)
                    }
                };

                listaDatosPrie.Add(entityDet);

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie16, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie16, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusionenergia no suministrada mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionEnergSumiEjecMensual(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 16);
            model.Titulo = "Difusión web - Energia no Suministrada Mensual";

            return View(model);
        }

        /// <summary>
        /// carga grafico difusionenergia no suministrada mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionEnergSumiEjecMensual(string idEmpresa, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatoPrie = new List<SioDatoprieDTO>();
            ListaDatoPrie = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie16).Cabpricodi.ToString());

            List<EveInterrupcionDTO> ListaContenido = (List<EveInterrupcionDTO>)_servicioSiosein.ObtenerContenidoDatosPrieTabla16(ListaDatoPrie, ConstantesSioSein.ReporteTipoEmpresa)[1];
            List<EveInterrupcionDTO> ListaDatoContenido = _servicioSiosein.ObtenerResumenEnergiaNoSuministradaEjecutada(ListaContenido);

            // <---- INICIO GRAFICO

            model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();

            model.Grafico = grafico;

            model.Grafico.Series = new List<RegistroSerie>();

            //List<EveInterrupcionDTO> ListaDatos = new List<EveInterrupcionDTO>();
            //ListaDatos = servicio.ListaTotalInterrupcionPorTipoEquipo(sFechaI, sFechaF);
            //model.ListaEveInterrupcion = servicio.ObtenerResumenEnergiaNoSuministradaEjecutada(ListaDatos);

            model.Grafico.XAxisCategories = new List<string>();
            List<EveInterrupcionDTO> Lista = ListaDatoContenido.GroupBy(x => x.Tipoemprcodi)
                .Select(
                x => new EveInterrupcionDTO
                {
                    Tipoemprcodi = x.First().Tipoemprcodi,
                    Tipoemprdesc = x.First().Tipoemprdesc
                }
                    )
                    .ToList();

            model.Grafico.Series = new List<RegistroSerie>();
            for (var j = 0; j < Lista.Count; j++)
            {
                model.Grafico.XAxisCategories.Add(Lista[j].Tipoemprdesc);
            }
            foreach (var item in ListaDatoContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Famnomb;
                registro.Data = new List<DatosSerie>();

                foreach (var tipEmpr in Lista)
                {
                    DatosSerie dato = new DatosSerie();

                    if (tipEmpr.Tipoemprcodi == ConstantesSioSein.CodTipEmprGeneracion)
                    {
                        dato.Y = item.Generacion;
                    }
                    if (tipEmpr.Tipoemprcodi == ConstantesSioSein.CodTipEmprTransmision)
                    {
                        dato.Y = item.Transmision;
                    }
                    if (tipEmpr.Tipoemprcodi == ConstantesSioSein.CodTipEmprDistribucion)
                    {
                        dato.Y = item.Distribucion;
                    }
                    if (tipEmpr.Tipoemprcodi == ConstantesSioSein.CodTipEmprUsuarioLibre)
                    {
                        dato.Y = item.UsuarioLibre;
                    }
                    registro.Data.Add(dato);
                }

                model.Grafico.Series.Add(registro);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// carga grafico difusionenergia no suministrada mensual detalle
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionEnergSumiEjecMensualDetalle(string idEmpresa, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            //Obteneniendo datos de tabla prie16
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie16).Cabpricodi.ToString());

            //Ordenando contenido de la data
            List<EveEventoDTO> ListaContenido = (List<EveEventoDTO>)_servicioSiosein.ObtenerContenidoDatosPrieTabla16(ListaDatos, ConstantesSioSein.ReporteTipoCausa)[0];

            model.Resultado = this._servicioSiosein.ListarReporteDifusionEnergSumiEjecMensualDetalle(
                ListaContenido.Where(x => idsEmpresa.Contains((int)x.Emprcodi))
                .ToList()
                );

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusionenergia no suministrada mensual por causa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionEnergSumiEjecMensualCausa(string idEmpresa, string mesAnio)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            var model = new SioseinModel();

            //Obteneniendo datos de tabla prie16
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie16).Cabpricodi.ToString());

            //Ordenando contenido de la data
            List<EveEventoDTO> ListaContenido = ((List<EveEventoDTO>)_servicioSiosein.ObtenerContenidoDatosPrieTabla16(ListaDatos, ConstantesSioSein.ReporteTipoCausa)[0]
                ).Where(x => idsEmpresa.Contains((int)x.Emprcodi))
                .ToList()
                ;

            model.ListaEveEvento = _servicioSiosein.ObtenerResumenDifusionEnergSumiEjecMensualDetalle(ListaContenido, 3);


            return Json(model);
        }

        /// <summary>
        /// carga grafico difusionenergia no suministrada mensual por equipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionEnergSumiEjecMensualEquipo(string idEmpresa, string mesAnio)
        {

            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            var model = new SioseinModel();

            //Obteneniendo datos de tabla prie16
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie16).Cabpricodi.ToString());

            //Ordenando contenido de la data
            List<EveEventoDTO> ListaContenido = ((List<EveEventoDTO>)_servicioSiosein.ObtenerContenidoDatosPrieTabla16(ListaDatos, ConstantesSioSein.ReporteTipoCausa)[0]
                ).Where(x => idsEmpresa.Contains((int)x.Emprcodi))
                .ToList()
                ;

            model.ListaEveEvento = _servicioSiosein.ObtenerResumenDifusionEnergSumiEjecMensualDetalle(ListaContenido, 2);


            return Json(model);
        }

        /// <summary>
        /// carga grafico difusionenergia no suministrada mensual por tipo de equipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionEnergSumiEjecMensualTipoEquipo(string idEmpresa, string mesAnio)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            var model = new SioseinModel();

            //Obteneniendo datos de tabla prie16
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie16).Cabpricodi.ToString());

            //Ordenando contenido de la data
            List<EveEventoDTO> ListaContenido = (List<EveEventoDTO>)_servicioSiosein.ObtenerContenidoDatosPrieTabla16(ListaDatos, ConstantesSioSein.ReporteTipoCausa)[0];
            //EnviarParametro = 5

            // <---- INICIO GRAFICO
            model.ListaEveEvento = _servicioSiosein.ObtenerResumenDifusionEnergSumiEjecMensualDetalle(ListaContenido.Where(x => idsEmpresa.Contains((int)x.Emprcodi))
                .ToList(), 5);


            GraficoWeb grafico = new GraficoWeb();

            model.Grafico = grafico;

            model.Grafico.Series = new List<RegistroSerie>();

            //List<EveInterrupcionDTO> ListaDatos = new List<EveInterrupcionDTO>();
            //ListaDatos = servicio.ListaTotalInterrupcionPorTipoEquipo(sFechaI, sFechaF);
            //model.ListaEveInterrupcion = servicio.ObtenerResumenEnergiaNoSuministradaEjecutada(ListaDatos);

            //model.Grafico.XAxisCategories = new List<string>();
            //List<EveInterrupcionDTO> Lista = ListaDatos.GroupBy(x => x.Tipoemprcodi)
            //    .Select(
            //    x => new EveInterrupcionDTO
            //    {
            //        Tipoemprcodi = x.First().Tipoemprcodi,
            //        Tipoemprdesc = x.First().Tipoemprdesc
            //    }
            //        )
            //        .ToList();

            model.Grafico.Series = new List<RegistroSerie>();
            //for (var j = 0; j < Lista.Count; j++)
            //{
            //    model.Grafico.XAxisCategories.Add(Lista[j].Tipoemprdesc);
            //}
            foreach (var item in model.ListaEveEvento)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Famnomb;
                registro.Acumulado = item.Interrupcion;

                model.Grafico.Series.Add(registro);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;



            //return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 17: FLUJOS DE INTERCONEXIÓN EJECUTADO (FLUJ)

        #region Verificación

        /// <summary>
        /// index flujo de interconecion ejecutado
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult FlujoInterconexionEjec(string periodo)
        {
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 17);

            DateTime dt = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            model.Mes = dt.ToString(Constantes.FormatoMesAnio);
            model.ListaInterconexion = _servicioInterconex.ListInInterconexions();

            if (model.ListaInterconexion.Any())
                model.IdPtomedicion = (int)model.ListaInterconexion[0].Ptomedicodi;

            return View(model);
        }

        /// <summary>
        /// carga lista flujo de interconecion ejecutado
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="interconex"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaFlujoInterconexionEjec(string fecha1, int param)
        {
            var model = new SioseinModel();

            DateTime fechaInicio = DateTime.ParseExact(fecha1, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            DateTime fechaFin = fechaInicio.GetLastDateOfMonth();

            model.IdParametro = param;
            model.Resultado = _servicioInterconex.GetHtmlInterconexionXParametro(1, param, fechaInicio, fechaFin);
            return PartialView(model);
        }

        /// <summary>
        /// carga grafico flujo de interconecion ejecutado
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="interconex"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoFlujoInterconexionEjec(string fecha1, string fecha2, int interconex, int param)
        {
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (fecha1 != null)
            {
                fecha1 = ConstantesSioSein.IniDiaFecha + fecha1.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(fecha1, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(fecha1, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            model = GraficoFlujoPotencia(1, interconex, param, dfechaIni, dfechaFin);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// metodo de grafico flujo de interconecion ejecutado
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private SioseinModel GraficoFlujoPotencia(int idPtomedicion, int tipoInterconexion, int parametro, DateTime fechaIni, DateTime fechaFin)
        {
            var model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            lista = _servicioInterconex.ObtenerInterconexionParametro(idPtomedicion, parametro, fechaIni, fechaFin);
            model.Grafico.SerieDataS = new DatosSerie[2][];
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series.Add(new RegistroSerie());
            switch (parametro)
            {
                case 1:
                    model.Grafico.Series[0].Name = "L-2280 (ZORRITOS) Exportación MW";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#3498DB";
                    model.Grafico.Series[0].YAxisTitle = "MW";
                    model.Grafico.Series[1].Name = "L-2280 (ZORRITOS) Importación MW";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#DC143C";
                    model.Grafico.Series[1].YAxisTitle = "MW";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    factor = 4;
                    model.Grafico.TitleText = @"Flujo en la línea L-2280 (Zorritos - Machala) de 220kV";
                    model.Grafico.YAxixTitle.Add("MW");
                    break;
                case 2:
                    model.Grafico.Series[0].Name = "L-2280 (ZORRITOS) Exportación MVAR";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#3498DB";
                    model.Grafico.Series[0].YAxisTitle = "MVAR";
                    model.Grafico.Series[1].Name = "L-2280 (ZORRITOS) Importación MVAR";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#DC143C";
                    model.Grafico.Series[1].YAxisTitle = "MVAR";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    factor = 4;
                    model.Grafico.TitleText = @"Flujo en la línea L-2280 (Zorritos - Machala) de 220kV";
                    model.Grafico.YAxixTitle.Add("MVAR");
                    break;
                case 3:
                    model.Grafico.Series[0].Name = "L-2280 (ZORRITOS) Exportación MWh";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#3498DB";
                    model.Grafico.Series[0].YAxisTitle = "MWh";
                    model.Grafico.Series[1].Name = "L-2280 (ZORRITOS) Importación MWh";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#DC143C";
                    model.Grafico.Series[1].YAxisTitle = "MWh";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.TitleText = @"Evolución del Flujo Energía Activa del Enlace Internacional 
en la línea L-2280 (Zorritos-Machala) de 220kV";
                    model.Grafico.YAxixTitle.Add("MWh");
                    break;
                case 4:
                    model.Grafico.Series[0].Name = "L-2280 (ZORRITOS) Exportación MVARh";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#3498DB";
                    model.Grafico.Series[1].Name = "L-2280 (ZORRITOS) Importación MVARh";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#DC143C";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.TitleText = @"Evolución del Flujo Energía Activa del Enlace Internacional 
en la línea L-2280 (Zorritos-Machala) de 220kV";
                    model.Grafico.YAxixTitle.Add("MVARh");
                    break;
                case 5:
                    model.Grafico.Series[0].Name = "L-2280 (ZORRITOS) kV";
                    model.Grafico.Series[0].Type = "line";
                    model.Grafico.Series[0].Color = "#3498DB";
                    model.Grafico.Series[0].YAxisTitle = "kV";
                    model.Grafico.Series[1].Name = "L-2280 (ZORRITOS) Amp";
                    model.Grafico.Series[1].Type = "line";
                    model.Grafico.Series[1].Color = "#DC143C";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.Series[1].YAxisTitle = "Amp";
                    model.Grafico.YAxixTitle.Add("kV");
                    model.Grafico.YAxixTitle.Add("Amp");
                    model.Grafico.TitleText = @"Evolución de la tensión y amperios de enlace internacional de linea L-2249
(Zorritos-Machala) durante los intercambios internacionales";
                    break;
            }

            if (lista.Count > 0)
            {
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte               

                //model.Grafico.subtitleText = lista24[0].Equinomb + " (MW)" + " Del" + model.FechaInicio + " Al" + model.FechaFin;
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);
                int indiceA = 0;
                int indiceB = 0;
                for (var i = 0; i < lista.Count(); i++)
                {
                    DateTime fecha = (DateTime)lista[i].Medifecha;
                    var registro = lista[i];
                    for (var j = 1; j <= 96; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        if (valor == null)
                            valor = 0;
                        if (i == 0)
                            model.Grafico.XAxisCategories.Add(registro.Medifecha.Value.AddMinutes(j * 15).ToString(Constantes.FormatoFechaHora));
                        var serie = new DatosSerie();
                        serie.X = fecha.AddMinutes(j * 15);
                        serie.Y = valor * factor;
                        switch (registro.Tipoinfocodi)
                        {
                            case 3:
                                if (registro.Ptomedicodi == ConstantesSioSein.IdExportacionL2280MWh)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    try
                                    {
                                        model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                    }
                                    catch (Exception ex)
                                    {
                                        var msg = ex.Message;
                                    }
                                }
                                break;
                            case 4:
                                if (registro.Ptomedicodi == ConstantesSioSein.IdExportacionL2280MVARr)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                }
                                break;
                            case 5:
                                model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                break;
                            case 9:

                                model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                break;
                        }
                    }
                    switch (registro.Tipoinfocodi)
                    {
                        case 3:
                            if (registro.Ptomedicodi == ConstantesSioSein.IdExportacionL2280MWh) indiceB++;
                            else indiceA++;
                            break;
                        case 4:
                            if (registro.Ptomedicodi == ConstantesSioSein.IdExportacionL2280MVARr) indiceB++;
                            else indiceA++;
                            break;
                        case 5:
                            indiceB++;
                            break;
                        case 9:
                            indiceA++;
                            break;

                    }
                }
                //modelGraficoDiario = model;
            }// end del if 
            return model;
        }

        /// <summary>
        /// validar datos flujo de interconecion ejecutado
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult ValidarFlujoInterconexionEjec(string fecha1)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime periodo = DateTime.ParseExact(fecha1, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = periodo.GetLastDateOfMonth();

                List<MeMedicion96DTO> lstDataHistInterconexiones = _servicioInterconex.ObtenerDataHistoricaInterconexionByMedidor(1, ConstantesInterconexiones.IdMedidorConsolidado, periodo, fechaFin, false);
                var lstTipoinfocodi = new List<int> { ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva, ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva };

                var listaInterActivaYReacti = lstDataHistInterconexiones.Where(x => lstTipoinfocodi.Contains(x.Tipoinfocodi));
                List<MeMedicion96DTO> lstInterconexiones = _servicioSiosein.ObtenenerDataReporteFlujoInterconexiones(listaInterActivaYReacti);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstInterconexiones)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = periodo,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            FechaHora = item.Medifecha.Value,
                            CodigoInterconexion = ConstantesAppServicio.CodigoInterconexion,
                            PotenciaActiva = item.PotenciaActiva,
                            PotenciaReactiva = item.PotenciaReactiva,
                            TipoFlujo = $"{item.TipoFlujo:D2}",
                            PaisIntercambio = ConstantesAppServicio.PaisIntercambio
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie17, periodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie17, periodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);

        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion flujo de interconecion ejecutado
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionFlujoInterconexionEjec(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 17);
            model.Titulo = "Difusión web - Flujo Interconexion Ejecutado";

            return View(model);
        }

        /// <summary>
        /// carga lista difusionenergia no suministrada mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionEnergSumiEjecMensual(string periodo)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstDataFlujoInter = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie17, fechaPeriodo);

                model.NRegistros = lstDataFlujoInter.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlDifusionEnergSumiEjecMensual(lstDataFlujoInter);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion flujo de interconecion ejecutado
        /// </summary>
        /// <param name="IDinterconexion"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionFlujoInterconexionEjec(string IDinterconexion, string periodo)
        {

            var model = new SioseinModel();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(fPeriodo, ConstantesSioSein.Prie17).Cabpricodi.ToString());

            model.Resultado = this._servicioSiosein.ListarReporteDifusionFlujoInterconexionEjec(ListaDatos);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion flujo de interconecion ejecutado
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionFlujoInterconexionEjec(string idEmpresa, string periodo)
        {

            var model = new SioseinModel();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            //Obteniendo datos de la tabla Datoprie
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(fPeriodo, ConstantesSioSein.Prie17).Cabpricodi.ToString());

            //Obteniendo datos ordenados
            List<MeMedicion96DTO> ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla17(ListaDatos, ConstantesSioSein.GraficoEvolucionEnergia);

            model.ListaMemedicion96 = ListaContenido;

            model.FechasCategoria = ListaContenido
                .Select(
                x => x.StrMedifecha
                )
                .ToList();

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion flujo de interconecion ejecutado de exportacion y importacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionFlujoInterconexionEjecExpImp(string idEmpresa, string periodo)
        {
            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            //Obteniendo datos de la tabla Datoprie
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(fPeriodo, ConstantesSioSein.Prie17).Cabpricodi.ToString());

            var model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();

            model.Grafico = grafico;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            //Obteniendo datos ordenados
            lista = _servicioSiosein.ObtenerContenidoDatosPrieTabla17(ListaDatos, ConstantesSioSein.GraficoContratoIntercambios)
                .OrderBy(x => x.Medifecha)
                .ToList();

            model.Grafico.SerieDataS = new DatosSerie[2][];
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series.Add(new RegistroSerie());

            model.Grafico.Series[0].Name = "Exportación";
            model.Grafico.Series[0].Type = "area";
            model.Grafico.Series[0].Color = "#3498DB";
            model.Grafico.Series[0].YAxisTitle = "MW";

            model.Grafico.Series[1].Name = "Importación";
            model.Grafico.Series[1].Type = "area";
            model.Grafico.Series[1].Color = "#DC143C";
            model.Grafico.Series[1].YAxisTitle = "MW";

            model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count];
            model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count];
            model.Grafico.TitleText = @"Contrato de Intercambios Internacionales EDEGEL(Perú) y CELEP EP(Ecuador)";
            model.Grafico.YAxixTitle.Add("MW");

            if (lista.Count > 0)
            {
                model.FechaInicio = dfechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = dfechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte               

                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);

                for (var i = 0; i < lista.Count(); i++)
                {
                    model.Grafico.XAxisCategories.Add(lista[i].StrMedifechhoramin);
                    var serie = new DatosSerie();
                    serie.X = (DateTime)lista[i].Medifecha;
                    serie.Y = lista[i].TotalEnergiaExportada;

                    var serie1 = new DatosSerie();
                    serie1.X = (DateTime)lista[i].Medifecha;
                    serie1.Y = lista[i].TotalEnergiaImportada;

                    model.Grafico.SerieDataS[0][i] = serie;
                    model.Grafico.SerieDataS[1][i] = serie1;

                }

            }
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #endregion

        #region TABLA 18: CAUDALES EJECUTADOS DIARIOS (CAUD)

        #region Verificacion

        /// <summary>
        /// index caudales ejecutados del dia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CaudalesEjecDia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 18);
            model.Url = Url.Content("~/");

            return View(model);
        }

        /// <summary>
        /// carga lista caudales ejecutados del dia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCaudalesEjecDia(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstCaudalEjecDiarioXPtoreporte = servicioPR5.ObtenerCaudalesDiariosPorReporte(ConstantesAppServicio.ReporcodiCaudalesEjecDiario, fechaInicio, fechaFin);

                model.NRegistros = lstCaudalEjecDiarioXPtoreporte.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCaudalesEjecDia(lstCaudalEjecDiarioXPtoreporte, fechaInicio, fechaFin);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de caudales ejecutados del dia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendCaudalesEjecutadosDiario(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstCaudalEjecDiarioXPtoreporte = servicioPR5.ObtenerCaudalesDiariosPorReporte(ConstantesAppServicio.ReporcodiCaudalesEjecDiario, fechaInicio, fechaFin)
                    .OrderBy(x => x.Osicodi).ThenBy(x => x.Medifecha).ThenBy(x => x.Codref).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstCaudalEjecDiarioXPtoreporte)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Ptomedicodi = item.Ptomedicodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = item.Medifecha.ToString(ConstantesSioSein.FormatAnioMesDia),
                            CodigoCuenca = item.Osicodi,
                            Tipo = $"{item.Codref:D2}",
                            Caudal = item.Meditotal ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie18, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie18, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion caudales ejecutados del dia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCaudalesEjecDia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 18);
            model.Titulo = "Difusión web - Caudales Ejecutadas Diaria";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion caudales ejecutados del dia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCaudalesEjecDia(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<SioDatoprieDTO> lstCaudales = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie18, fechaPeriodo);
                List<MeReporptomedDTO> listaPuntos = servFormatoRep.ObtenerPuntosReporteMedicion(ConstantesAppServicio.ReporcodiCaudalesEjecDiario);

                model.NRegistros = lstCaudales.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlDifusionCaudalesEjecDia(lstCaudales, listaPuntos);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 19: VOLUMENES DE RESERVORIOS EJECUTADOS DIARIOS (VRES)

        #region Verificacion

        /// <summary>
        /// index volumen de reservorios ejecutados del dia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VolumenReserEjecDia(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 19);
            model.Url = Url.Content("~/");

            return View(model);
        }

        /// <summary>
        /// carga lista volumen de reservorios ejecutados del dia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVolumenReserEjecDia(string mesAnio)
        {
            var model = new SioseinModel();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista24 = this._servicioSiosein.GetHidrologiaSioSein(ConstantesSioSein.ReporcodiVolRsvEjeDia, dfechaIni, dfechaFin);

            if (lista24.Count > 0)
            {
                for (DateTime fecha = dfechaIni; fecha <= dfechaFin; fecha = fecha.AddDays(1))
                {
                    var result = lista24.Where(x => x.Medifecha.Day == fecha.Day).FirstOrDefault();
                    if (result == null)
                    {
                        var copyObj = lista24.Where(x => x.Equicodi > 0).FirstOrDefault();
                        lista24.Add(new MeMedicion24DTO { Medifecha = fecha, Equicodi = copyObj.Equicodi, Equinomb = copyObj.Equinomb });
                    }
                }

            }

            model.NRegistros = lista24.Count;
            model.Resultado = this._servicioSiosein.ListarReporteVolumenReserEjecDia(lista24.OrderBy(x => x.Medifecha).ToList(), dfechaFin);

            return Json(model);
        }

        /// <summary>
        /// carga lista volumen de reservorios ejecutados del dia - desviaciones
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVolumenReserEjecDiaDesviacion(string mesAnio)
        {
            var model = new SioseinModel();
            var lista24 = new List<MeMedicion24DTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista24 = this._servicioSiosein.GetHidrologiaSioSein(ConstantesSioSein.ReporcodiVolRsvEjeDia, dfechaIni, dfechaFin);

            if (lista24.Count > 0)
            {
                for (DateTime fecha = dfechaIni; fecha <= dfechaFin; fecha = fecha.AddDays(1))
                {
                    var result = lista24.Where(x => x.Medifecha.Day == fecha.Day).FirstOrDefault();
                    if (result == null)
                    {
                        var copyObj = lista24.FirstOrDefault();
                        lista24.Add(new MeMedicion24DTO { Medifecha = fecha, Equicodi = copyObj.Equicodi, Equinomb = copyObj.Equinomb });
                    }
                }
            }


            List<MeMedicion24DTO> listaNextDay = new List<MeMedicion24DTO>();
            listaNextDay = this._servicioSiosein.GetHidrologiaSioSein(ConstantesSioSein.ReporcodiVolRsvEjeDia, dfechaIni.AddDays(1), dfechaFin.AddDays(1));

            if (listaNextDay.Count > 0)
            {
                for (DateTime fecha = dfechaIni; fecha <= dfechaFin; fecha = fecha.AddDays(1))
                {
                    var result = listaNextDay.Where(x => x.Medifecha.Day == fecha.Day).FirstOrDefault();
                    if (result == null)
                    {
                        var copyObj = listaNextDay.FirstOrDefault();
                        listaNextDay.Add(new MeMedicion24DTO { Medifecha = fecha, Equicodi = copyObj.Equicodi, Equinomb = copyObj.Equinomb });
                    }
                }
            }


            model.Resultado = this._servicioSiosein.ListarReporteVolumenReserEjecDiaDesviacion(lista24.OrderBy(x => x.Medifecha).ToList(), listaNextDay.OrderBy(x => x.Medifecha).ToList(), dfechaFin);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volumen de reservorios ejecutados del dia 1
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoVolumenReserEjecDia(int id, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            var lista24 = new List<MeMedicion24DTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista24 = this._servicioSiosein.GetHidrologiaSioSein(ConstantesSioSein.ReporcodiVolRsvEjeDia, dfechaIni, dfechaFin);

            List<MeMedicion24DTO> listaNextDay = new List<MeMedicion24DTO>();
            listaNextDay = this._servicioSiosein.GetHidrologiaSioSein(ConstantesSioSein.ReporcodiVolRsvEjeDia, dfechaIni.AddDays(1), dfechaFin.AddDays(1));

            //llenado de fechas sin data
            if (lista24.Count > 0 && listaNextDay.Count > 0)
            {
                for (DateTime fecha = dfechaIni; fecha <= dfechaFin; fecha = fecha.AddDays(1))
                {
                    var result = lista24.Where(x => x.Medifecha.Day == fecha.Day).FirstOrDefault();
                    if (result == null)
                    {
                        var copyObj = lista24.FirstOrDefault();
                        lista24.Add(new MeMedicion24DTO { Medifecha = fecha, Equicodi = copyObj.Equicodi, Equinomb = copyObj.Equinomb });
                    }
                }
                for (DateTime fecha = dfechaIni; fecha <= dfechaFin; fecha = fecha.AddDays(1))
                {
                    var result = listaNextDay.Where(x => x.Medifecha.Day == fecha.Day).FirstOrDefault();
                    if (result == null)
                    {
                        var copyObj = listaNextDay.FirstOrDefault();
                        listaNextDay.Add(new MeMedicion24DTO { Medifecha = fecha, Equicodi = copyObj.Equicodi, Equinomb = copyObj.Equinomb });
                    }
                }
            }
            //<--

            if (lista24.Count > 0)
            {
                model = GraficoVolumenReserEjecDia(lista24, listaNextDay, dfechaFin, id);
            }
            model.NRegistros = lista24.Count;

            return Json(model);
        }

        private SioseinModel GraficoVolumenReserEjecDia(List<MeMedicion24DTO> data, List<MeMedicion24DTO> dataNextDay, DateTime dfechaFin, int id)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();

            data = data.Where(x => x.Equicodi == id).ToList();

            string titulo1 = "DETALLE DE DESVIACION DE VOLUMENES INICIAL Y FINAL DE ", name = " INICIAL", name1 = " FINAL", type = "spline", type1 = "spline", yAxisTitle = string.Empty, yAxisTitle1 = string.Empty;
            model.Grafico = grafico;
            int totalIntervalos = data.Count;
            model.Grafico.SeriesData = new decimal?[2][];
            if (data.Count > 0)
            {
                {
                    model.Grafico.YaxixTitle = "%";
                    model.Grafico.XAxisCategories = new List<string>();
                    model.Grafico.SeriesName = new List<string>();
                    model.Grafico.SeriesType = new List<string>();
                    model.Grafico.SeriesYAxis = new List<int>();

                    // Obtener lista de intervalos categoria del grafico   
                    List<string> dias = new List<string>();
                    for (int i = 1; i <= dfechaFin.Day; i++)
                    {
                        dias.Add(i.ToString());
                    }
                    model.Grafico.XAxisCategories = dias;

                    model.Grafico.TitleText = titulo1 + data[0].Equinomb;

                    model.Grafico.Series = new List<RegistroSerie>();
                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series.Add(new RegistroSerie());

                    model.Grafico.Series[0].Name = data[0].Equinomb + name1;
                    model.Grafico.Series[0].Type = type1;
                    model.Grafico.Series[0].YAxis = 0;

                    model.Grafico.Series[1].Name = data[0].Equinomb + name;
                    model.Grafico.Series[1].Type = type;
                    model.Grafico.Series[1].YAxis = 0;

                    model.Grafico.SeriesData[0] = new decimal?[totalIntervalos];
                    model.Grafico.SeriesData[1] = new decimal?[totalIntervalos];
                    for (int i = 0; i < data.Count; i++)
                    {
                        var dat = data.Where(x => x.Equicodi == data[i].Equicodi).ToList();
                        var datND = dataNextDay.Where(x => x.Equicodi == data[i].Equicodi).ToList();

                        decimal volIni = 0;
                        foreach (var list in dat)
                        {
                            for (int m = 1; m < 25; m++)
                            {
                                var valor = (decimal?)list.GetType().GetProperty("H" + m).GetValue(list, null);
                                if (valor != null) { if (m == 1) { volIni = (decimal)valor; } }
                            }
                        }
                        int con = 0;
                        decimal val_ = 0;
                        foreach (var list in datND)
                        {
                            decimal volIniND = 0;
                            for (int m = 1; m < 25; m++)
                            {
                                var valor = (decimal?)list.GetType().GetProperty("H" + m).GetValue(list, null);
                                if (valor != null) { if (m == 1) { volIniND = (decimal)valor; } }
                            }

                            if (con + 1 == datND.Count) { val_ = 100; }
                            else
                            {
                                if (volIniND > 0 && volIni > 0) { val_ = ((volIni - volIniND) / volIniND) * 100; }
                                else { val_ = 0; }
                            }
                            model.Grafico.SeriesData[0][con] = val_;
                            con++;
                        }

                        decimal volFin = 0;
                        foreach (var list in dat)
                        {
                            for (int m = 1; m < 25; m++)
                            {
                                var valor = (decimal?)list.GetType().GetProperty("H" + m).GetValue(list, null);
                                if (valor != null) { if (m > 1) { volFin = (decimal)valor; } }
                            }

                            data.Remove(list);
                            i = -1;
                        }
                        con = 0;
                        val_ = 0;
                        foreach (var list in datND)
                        {
                            decimal volFinND = 0;
                            for (int m = 1; m < 25; m++)
                            {
                                var valor = (decimal?)list.GetType().GetProperty("H" + m).GetValue(list, null);
                                if (valor != null) { if (m > 1) { volFinND = (decimal)valor; } }
                            }
                            if (con + 1 == datND.Count) { val_ = 100; }
                            else
                            {
                                if (volFinND > 0 && volFin > 0) { val_ = ((volFin - volFinND) / volFinND) * 100; }
                                else { val_ = 0; }
                            }
                            model.Grafico.SeriesData[1][con] = val_;
                            con++;
                        }
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// envio de datos a tablas prie de volumen de reservorios ejecutados del dia
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendVolumenReserEjecDia(string mesAnio)
        {
            int retorno = 0;
            SioseinModel model = new SioseinModel();

            retorno = _servicioSiosein.GuardarVolumenReserEjecDia(mesAnio, ConstantesSioSein.ReporcodiVolRsvEjeDia, User.Identity.Name);

            model.IdEnvio = retorno;

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion volumen de reservorios ejecutados del dia
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionVolumenReserEjecDia(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion volumen de reservorios ejecutados del dia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenReserEjecDia(string idEmpresa, string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie19, dfechaIni);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionVolumenReserEjecDia(listDatoPrie, dfechaFin, 1);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volumen de reservorios ejecutados del dia 1
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoDifusionVolumenReserEjecDia(int id, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie19, dfechaIni);

            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionVolumenReserEjecDia(listDatoPrie, dfechaFin, id);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionVolumenReserEjecDia(List<SioDatoprieDTO> lista, DateTime dfechaFin, int id)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();

            foreach (var upd in lista)
            {
                string[] arr = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[1]);
                foreach (var arr_ in arr)
                {
                    MeMedicion24DTO entity = new MeMedicion24DTO();
                    entity.Medifecha = (DateTime)upd.Dpriefechadia;
                    string[] arr__ = arr_.Split(ConstantesSioSein.SplitPrie[0]);
                    if (arr__.Count() > 0)
                    {
                        entity.Ptomedicodi = int.Parse(arr__[0]);
                        entity.Tipoptomedicodi = int.Parse(arr__[1]);
                        entity.Tipoptomedinomb = arr__[2];
                        entity.Equicodi = int.Parse(arr__[3]);
                        entity.Equinomb = arr__[4];
                        entity.H1 = decimal.Parse(arr__[5]);
                        entity.H2 = decimal.Parse(arr__[6]);
                        entity.H3 = decimal.Parse(arr__[7]);
                        entity.H4 = decimal.Parse(arr__[8]);
                        entity.H5 = decimal.Parse(arr__[9]);
                        entity.H6 = decimal.Parse(arr__[10]);
                        entity.H7 = decimal.Parse(arr__[11]);
                        entity.H8 = decimal.Parse(arr__[12]);
                        entity.H9 = decimal.Parse(arr__[13]);
                        entity.H10 = decimal.Parse(arr__[14]);
                        entity.H11 = decimal.Parse(arr__[15]);
                        entity.H12 = decimal.Parse(arr__[16]);
                        entity.H13 = decimal.Parse(arr__[17]);
                        entity.H14 = decimal.Parse(arr__[18]);
                        entity.H15 = decimal.Parse(arr__[19]);
                        entity.H16 = decimal.Parse(arr__[20]);
                        entity.H17 = decimal.Parse(arr__[21]);
                        entity.H18 = decimal.Parse(arr__[22]);
                        entity.H19 = decimal.Parse(arr__[23]);
                        entity.H20 = decimal.Parse(arr__[24]);
                        entity.H21 = decimal.Parse(arr__[25]);
                        entity.H22 = decimal.Parse(arr__[26]);
                        entity.H23 = decimal.Parse(arr__[27]);
                        entity.H24 = decimal.Parse(arr__[28]);
                        entity.Meditotal = decimal.Parse(arr__[29]);
                    }
                    data.Add(entity);
                }
            }

            data = data.Where(x => x.Equicodi == id).ToList();

            string titulo1 = "EVOLUCION DIARIA DE LOS VOLUMENES INCIALES Y FINALES DE ", name = " INICIAL", name1 = " FINAL", type = "column", type1 = "area", yAxisTitle = string.Empty, yAxisTitle1 = string.Empty;
            int factor = 1;
            model.Grafico = grafico;
            int totalIntervalos = data.Count;
            model.Grafico.SeriesData = new decimal?[2][];
            if (data.Count > 0)
            {
                {
                    model.Grafico.YaxixTitle = "MW";
                    model.Grafico.XAxisCategories = new List<string>();
                    model.Grafico.SeriesName = new List<string>();
                    model.Grafico.SeriesType = new List<string>();
                    model.Grafico.SeriesYAxis = new List<int>();

                    // Obtener lista de intervalos categoria del grafico   
                    List<string> dias = new List<string>();
                    for (int i = 1; i <= dfechaFin.Day; i++)
                    {
                        dias.Add(i.ToString());
                    }
                    model.Grafico.XAxisCategories = dias;

                    model.Grafico.TitleText = titulo1 + data[0].Equinomb;

                    model.Grafico.Series = new List<RegistroSerie>();
                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series.Add(new RegistroSerie());
                    for (var i = 0; i < 2; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                model.Grafico.Series[i].Name = data[0].Equinomb + name1;
                                model.Grafico.Series[i].Type = type1;
                                //model.Grafico.Series[i].Color = "lightblue";
                                model.Grafico.Series[i].YAxis = 0;
                                //model.Grafico.Series[i].YAxisTitle = data[0].Equinomb + name1;
                                factor = 1;
                                break;
                            case 1:
                                model.Grafico.Series[i].Name = data[0].Equinomb + name;
                                model.Grafico.Series[i].Type = type;
                                //model.Grafico.Series[i].Color = "blue";
                                model.Grafico.Series[i].YAxis = 0;
                                //model.Grafico.Series[i].YAxisTitle = data[0].Equinomb + name;
                                break;
                        }
                        model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            decimal? valor = 0;
                            valor = data[j - 1].Meditotal;//(decimal?)lista[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista[j - 1], null);
                            model.Grafico.SeriesData[i][j - 1] = valor * factor;
                        }
                    }
                }
            }

            return model;
        }

        #endregion

        #endregion

        #region TABLA 20: VOLUMENES DE LAGOS (HLAG)

        #region Verificacion

        /// <summary>
        /// index volunes de lagos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VolumenLagos(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 20);
            model.Url = Url.Content("~/");

            return View(model);
        }

        /// <summary>
        /// carga lista volunes de lagos
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVolumenLagos(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstDataVolumenLagos = servicioPR5.ObtenerEmbalsesDiariosPorReporte(ConstantesAppServicio.ReporcodiVolumenLagos, fechaInicio, fechaFin);

                bool graficoIcono = true;
                model.NRegistros = lstDataVolumenLagos.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlVolumenEmbalsesLagunas(lstDataVolumenLagos, fechaInicio, fechaFin, graficoIcono);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// carga grafico volunes de lagos1 
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoVolumenLagos(int id, string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<MeReporptomedDTO> listaPuntos = servFormatoRep.ObtenerPuntosReporteMedicion(ConstantesAppServicio.ReporcodiVolumenLagos).Where(x => x.Ptomedicodi == id).ToList();
                List<MeReporptomedDTO> lstDataLago = servicioPR5.ObtenerEmbalseDiariosPorPunto(fechaInicio, fechaFin, listaPuntos);

                model.Grafico = _servicioSiosein.GenerarGWebVolumenLagos(lstDataLago, fechaInicio, fechaFin);
                model.NRegistros = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);

        }

        /// <summary>
        /// envio de datos a tablas prie de volunes de lagos
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendVolumenLagos(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstDataVolumenEmbalses = servicioPR5.ObtenerEmbalsesMensuales(ConstantesAppServicio.ReporcodiVolumenLagos, fechaInicio, fechaFin)
                    .OrderBy(x => x.Osicodi).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();
                var periodo = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes);
                foreach (var item in lstDataVolumenEmbalses)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = periodo,
                            CodigoLago = item.Osicodi,
                            Volumen = item.Meditotal ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie20, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie20, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion volunes de lagos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionVolumenLagos(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 20);
            model.Titulo = "Difusión web - Volumenes de Lagos";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion volunes de lagos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenLagos(string idEmpresa, string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie20, dfechaIni);

            model.Resultado = this._servicioSiosein.ListarReporteVolumenLagos(this._servicioSiosein.ObtenerDataDifucionVolumenPrie(listDatoPrie, dfechaFin), dfechaFin, "1");

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volunes de lagos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoDifusionVolumenLagos(int id, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie20, dfechaIni);

            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionVolumenLagos(listDatoPrie, dfechaFin, id);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionVolumenLagos(List<SioDatoprieDTO> lista, DateTime dfechaFin, int id)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();

            foreach (var upd in lista)
            {
                string[] arr = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[1]);
                foreach (var arr_ in arr)
                {
                    MeMedicion24DTO entity = new MeMedicion24DTO();
                    entity.Medifecha = (DateTime)upd.Dpriefechadia;
                    string[] arr__ = arr_.Split(ConstantesSioSein.SplitPrie[0]);
                    if (arr__.Count() > 0)
                    {
                        entity.Ptomedicodi = int.Parse(arr__[0]);
                        entity.Tipoptomedicodi = int.Parse(arr__[1]);
                        entity.Tipoptomedinomb = arr__[2];
                        entity.Equicodi = int.Parse(arr__[3]);
                        entity.Equinomb = arr__[4];
                        entity.Meditotal = decimal.Parse(arr__[5]);

                    }
                    data.Add(entity);
                }
            }

            data = data.Where(x => x.Equicodi == id).ToList();

            string titulo1 = "EVOLUCION DIARIA DE VOLUMENES DE LAGO ", type = "spline";
            int factor = 1;
            model.Grafico = grafico;
            int totalIntervalos = data.Count;
            model.Grafico.SeriesData = new decimal?[1][];
            if (data.Count > 0)
            {
                {
                    model.Grafico.YaxixTitle = "MW";
                    model.Grafico.XAxisCategories = new List<string>();
                    model.Grafico.SeriesName = new List<string>();
                    model.Grafico.SeriesType = new List<string>();
                    model.Grafico.SeriesYAxis = new List<int>();

                    // Obtener lista de intervalos categoria del grafico   
                    List<string> dias = new List<string>();
                    for (int i = 1; i <= dfechaFin.Day; i++)
                    {
                        dias.Add(i.ToString());
                    }
                    model.Grafico.XAxisCategories = dias;

                    model.Grafico.TitleText = titulo1 + data[0].Equinomb;

                    model.Grafico.Series = new List<RegistroSerie>();
                    model.Grafico.Series.Add(new RegistroSerie());
                    for (var i = 0; i < 1; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                model.Grafico.Series[i].Name = data[0].Equinomb;
                                model.Grafico.Series[i].Type = type;
                                //model.Grafico.Series[i].Color = "lightblue";
                                model.Grafico.Series[i].YAxis = 0;
                                //model.Grafico.Series[i].YAxisTitle = data[0].Equinomb + name1;
                                factor = 1;
                                break;
                        }
                        model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            decimal? valor = 0;
                            valor = data[j - 1].Meditotal;//(decimal?)lista[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista[j - 1], null);
                            model.Grafico.SeriesData[i][j - 1] = valor * factor;
                        }
                    }
                }
            }

            return model;
        }

        #endregion

        #endregion

        #region TABLA 21: VOLUMEN EMBALSES (HEMB)

        #region VERIFICACION

        /// <summary>
        /// index volumes de embalses
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VolumenEmbalses(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 21);
            model.Url = Url.Content("~/");

            return View(model);
        }

        /// <summary>
        /// carga lista volunes de embalses
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVolumenEmbalses(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstDataVolumenEmbalses = servicioPR5.ObtenerEmbalsesDiariosPorReporte(ConstantesAppServicio.ReporcodiVolumenEmbalses, fechaInicio, fechaFin);
                bool graficoIcono = false;
                model.NRegistros = lstDataVolumenEmbalses.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlVolumenEmbalsesLagunas(lstDataVolumenEmbalses, fechaInicio, fechaFin, graficoIcono);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de volunes de embalses
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendVolumenEmbalses(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstDataVolumenEmbalses = servicioPR5.ObtenerEmbalsesMensuales(ConstantesAppServicio.ReporcodiVolumenEmbalses, fechaInicio, fechaFin)
                    .OrderBy(x => x.Osicodi).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();
                var periodo = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes);
                foreach (var item in lstDataVolumenEmbalses)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = periodo,
                            CodigoEmbalse = item.Osicodi,
                            Volumen = item.Meditotal ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie21, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie21, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion volunes de embalses
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionVolumenEmbalses(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion volunes de embalses
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenEmbalses(string idEmpresa, string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie21, dfechaIni);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionEmbalses(listDatoPrie, dfechaFin, 1);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volunes de embalses 1
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoDifusionVolumenEmbalses(int id, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie21, dfechaIni);

            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionVolumenEmbalses(listDatoPrie, dfechaFin, id);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionVolumenEmbalses(List<SioDatoprieDTO> lista, DateTime dfechaFin, int id)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();

            foreach (var upd in lista)
            {
                string[] arr = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[1]);
                foreach (var arr_ in arr)
                {
                    MeMedicion24DTO entity = new MeMedicion24DTO();
                    entity.Medifecha = (DateTime)upd.Dpriefechadia;
                    string[] arr__ = arr_.Split(ConstantesSioSein.SplitPrie[0]);
                    if (arr__.Count() > 0)
                    {
                        entity.Ptomedicodi = int.Parse(arr__[0]);
                        entity.Tipoptomedicodi = int.Parse(arr__[1]);
                        entity.Tipoptomedinomb = arr__[2];
                        entity.Equicodi = int.Parse(arr__[3]);
                        entity.Equinomb = arr__[4];
                        entity.Meditotal = decimal.Parse(arr__[5]);
                    }
                    data.Add(entity);
                }
            }

            data = data.Where(x => x.Equicodi == id).ToList();

            string titulo1 = "EVOLUCION DIARIA DE LOS VOLUMENES DE EMBALSE ", type = "spline";
            int factor = 1;
            model.Grafico = grafico;
            int totalIntervalos = data.Count;
            model.Grafico.SeriesData = new decimal?[2][];
            if (data.Count > 0)
            {
                {
                    model.Grafico.YaxixTitle = "MW";
                    model.Grafico.XAxisCategories = new List<string>();
                    model.Grafico.SeriesName = new List<string>();
                    model.Grafico.SeriesType = new List<string>();
                    model.Grafico.SeriesYAxis = new List<int>();

                    // Obtener lista de intervalos categoria del grafico   
                    List<string> dias = new List<string>();
                    for (int i = 1; i <= dfechaFin.Day; i++)
                    {
                        dias.Add(i.ToString());
                    }
                    model.Grafico.XAxisCategories = dias;

                    model.Grafico.TitleText = titulo1 + data[0].Equinomb;

                    model.Grafico.Series = new List<RegistroSerie>();
                    model.Grafico.Series.Add(new RegistroSerie());
                    for (var i = 0; i < 1; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                model.Grafico.Series[i].Name = data[0].Equinomb;
                                model.Grafico.Series[i].Type = type;
                                //model.Grafico.Series[i].Color = "lightblue";
                                model.Grafico.Series[i].YAxis = 0;
                                //model.Grafico.Series[i].YAxisTitle = data[0].Equinomb + name1;
                                factor = 1;
                                break;
                        }
                        model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            decimal? valor = 0;
                            valor = data[j - 1].Meditotal;//(decimal?)lista[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista[j - 1], null);
                            model.Grafico.SeriesData[i][j - 1] = valor * factor;
                        }
                    }
                }
            }

            return model;
        }

        #endregion

        #endregion

        #region TABLA 22: HIDROLOGÍA CUENCAS (HCUE)

        #region Verificacion

        /// <summary>
        /// index hidrologia de cuencuas
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult HidrologiaCuencas(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 22);

            return View(model);
        }

        /// <summary>
        /// carga lista hidrologia de cuencuas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaHidrologiaCuencas(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstCaudalEjecDiarioXPtoreporte = servicioPR5.ObtenerCaudalesDiariosPorReporte(ConstantesAppServicio.ReporcodiCaudalesEjecDiario, fechaInicio, fechaFin);

                model.NRegistros = lstCaudalEjecDiarioXPtoreporte.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCaudalesEjecDia(lstCaudalEjecDiarioXPtoreporte, fechaInicio, fechaFin);
                //model.Resultado = this._servicioSiosein.ListarReporteHidrologiaCuencas(lista24, dfechaFin);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de hidrologia de cuencuas
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendHidrologiaCuencas(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();
                List<MeReporptomedDTO> lstCaudalEjecDiarioXPtoreporte = servicioPR5.ObtenerCaudalesDiariosPorReporte(ConstantesAppServicio.ReporcodiCaudalesEjecDiario, fechaInicio, fechaFin);

                var periodo = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstCaudalEjecDiarioXPtoreporte.GroupBy(x => new { x.Ptomedicodi, x.Codref, x.Osicodi }))
                {
                    var lstValores = item.Where(x => x.Meditotal.HasValue).Select(x => x.Meditotal.Value);
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Ptomedicodi = item.Key.Ptomedicodi,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = periodo,
                            CodigoCuenca = item.Key.Osicodi,
                            Tipo = $"{item.Key.Codref:D2}",
                            Caudal = lstValores.Any() ? lstValores.Average() : 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                listaDatosPrie = listaDatosPrie.OrderBy(x => x.SioReporte.CodigoCuenca).ThenBy(x => x.SioReporte.Tipo).ToList();

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie22, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie22, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion hidrologia de cuencuas
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionHidrologiaCuencas(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 22);
            model.Titulo = "Difusión web - Hidrologia Cuencas";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion hidrologia de cuencuas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionHidrologiaCuencas(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<SioDatoprieDTO> lstCuencas = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie22, fechaPeriodo);
                List<MeReporptomedDTO> listaPuntos = servFormatoRep.ObtenerPuntosReporteMedicion(ConstantesAppServicio.ReporcodiCaudalesEjecDiario);

                model.NRegistros = lstCuencas.Count;
                model.Resultado = _servicioSiosein.GenrarReporteDifusionHidrologiaCuencas(lstCuencas, listaPuntos);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 23: VOLUMEN DE COMBUSTIBLE (VCOM)

        #region VERIFICACIÓN

        public ActionResult VolumenCombustible(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 23);

            return View(model);
        }

        /// <summary>
        /// carga lista volumen de combustibles
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVolumenCombustible(string mesAnio)
        {

            SioseinModel model = new SioseinModel();
            try
            {
                var fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                List<CccVersionDTO> listaVersion = _servicioConsumoCombustible.GetByCriteriaCccVersions(fechaPeriodo, fechaPeriodo, ConstantesConsumoCombustible.HorizonteMensual).OrderByDescending(x => x.Cccvernumero).ToList();

                List<CccVcomDTO> listaRept = new List<CccVcomDTO>();
                List<string> listaObs = new List<string>();
                if (listaVersion != null && listaVersion.Count > 0)
                {
                    int vercodi = listaVersion[0].Cccvercodi;
                    _servicioConsumoCombustible.ListaDataVCOMXVersionReporte(vercodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                    , out List<CccVcomDTO> listaReptotOut
                                                    , out List<SiEmpresaDTO> listaEmpresa
                                                    , out List<EqEquipoDTO> listaCentral
                                                    , out List<EqEquipoDTO> listaEquipo);

                    string empresas = string.Join(",", listaEmpresa.Select(e => e.Emprcodi.ToString()).ToArray());
                    string centrales = string.Join(",", listaCentral.Select(c => c.Equipadre.ToString()).ToArray());

                    listaRept = _servicioConsumoCombustible.ListarReporteVCOMxFiltro(vercodi, empresas, centrales, out CccVersionDTO regVersion);
                    listaObs = regVersion.ListaObs;
                }
                model.Resultado = _servicioSiosein.GenerarRHtmlReporteVolumenCombustible(listaObs, listaRept);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);

        }

        /// <summary>
        /// validacion de volumen de combustibles
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult ValidarVolumenCombustible(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<CccVcomDTO> listaCostoXModo = _servicioSiosein.ListarDatosTxtTablaPrieVCOM(fechaPeriodo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in listaCostoXModo)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaPeriodo,
                        Grupocodi = item.Grupocodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = fechaPeriodo.ToString(ConstantesAppServicio.FormatoAnioMes),
                            CodigoModoOperacion = item.Vcomcodigomop,
                            CodigoTipoCombustible = item.Vcomcodigotcomb,
                            Valor = item.Vcomvalor ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie23, fechaPeriodo, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie23, fechaPeriodo, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion volumen de combustibles
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionVolumenCombustible(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion volumen de combustibles liquido
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenCombustibleLiquido(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            //.Where(x => idEmpresa.Contains(x.Emprcodi))
            //.OrderBy(x => x.Emprnomb)
            //.ToList();
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteLiquido23);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionVolumenCombustible(ListaContenido, ConstantesSioSein.ReporteLiquido23);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion volumen de combustibles gas
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenCombustibleGas(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            //.Where(x => idEmpresa.Contains(x.Emprcodi))
            //.OrderBy(x => x.Emprnomb)
            //.ToList();
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteGas23);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionVolumenCombustible(ListaContenido, ConstantesSioSein.ReporteGas23);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion volumen de combustibles carbon
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenCombustibleCarbon(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            //.Where(x => idEmpresa.Contains(x.Emprcodi))
            //.OrderBy(x => x.Emprnomb)
            //.ToList();
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteCarbon23);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionVolumenCombustible(ListaContenido, ConstantesSioSein.ReporteCarbon23);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion volumen de combustibles bagazo
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionVolumenCombustibleBagazo(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            //.Where(x => idEmpresa.Contains(x.Emprcodi))
            //.OrderBy(x => x.Emprnomb)
            //.ToList();
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteBagazo23);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionVolumenCombustible(ListaContenido, ConstantesSioSein.ReporteBagazo23);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volumen de combustibles liquido
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionVolumenCombustibleLiqui(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteLiquido23);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaContenido)
            {

                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Medinth1;

                model.Grafico.Series.Add(registro);

            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion volumen de combustibles gas natural
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionVolumenCombustibleGasNatu(string idsEmpresa, string periodo)
        {
            var idEmpresa = idsEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            ListaDatos = this._servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie23).Cabpricodi.ToString());
            List<MeMedicionxintervaloDTO> ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla23(ListaDatos, ConstantesSioSein.ReporteGas23);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Medinth1;
                model.Grafico.Series.Add(registro);
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 24: HECHOS RELEVANTES (IEVE)

        #region VERIFICACION

        /// <summary>
        /// index hechos relevantes
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult HechosRelevantes(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 24);

            return View(model);
        }

        /// <summary>
        /// carga lista hechos relevantes
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaHechosRelevantes(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lstHechosRelevantes = _servicioSiosein.ListarDatosTxtTablaPrieIEVE(fechaInicio);

                model.NRegistros = lstHechosRelevantes.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlHechosRelevantes(lstHechosRelevantes);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        [HttpPost]
        /// <summary>
        /// envio de datos a tablas prie de hechos relevantes
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaHechosRelevantes(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            int sioseinMaximoCaracteres = Int32.Parse(ConfigurationManager.AppSettings["sioseinMaximoCaracteres"]);

            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lstHechosRelevantes = _servicioSiosein.ListarDatosTxtTablaPrieIEVE(fechaInicio);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstHechosRelevantes)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Equicodi = item.Equicodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                            FechaHoraInicio = item.Evenini.Value,
                            FechaHoraFin = item.Evenfin.Value,
                            CodigoTipoEmpresa = item.OsiCodigoTipoEmpresa,
                            CodigoGrupo = item.Osinergcodi,
                            PotenciaIndisponible = item.Evenmwindisp ?? 0,
                            CodigoTipoIndisponibilidad = item.Tipoindisponibilidad,
                            Observacion = item.Evenobsrv.Substring(0, item.Evenobsrv.Length > sioseinMaximoCaracteres ? sioseinMaximoCaracteres : item.Evenobsrv.Length),
                            Motivo = item.Subcausadesc.Substring(0, item.Subcausadesc.Length > sioseinMaximoCaracteres ? sioseinMaximoCaracteres : item.Subcausadesc.Length)
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie24, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie24, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion hechos relevantes
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionHechosRelevantes(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion hechos relevantes
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionHechosRelevantes(string idEmpresa, string mesAnio)
        {
            var model = new SioseinModel();
            var listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie24, dfechaIni);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionHechosRelevantes(listDatoPrie);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion hechos relevantes
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionHechosRelevantes(string idEmpresa, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie24, dfechaIni);
            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionHechosRelevantes(listDatoPrie, dfechaIni, dfechaFin);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionHechosRelevantes(List<SioDatoprieDTO> data, DateTime dfechaIni, DateTime dfechaFin)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<EveManttoDTO> lista = new List<EveManttoDTO>();

            if (data != null)
            {
                foreach (var upd in data)
                {
                    lista.Add(new EveManttoDTO()
                    {
                        Emprnomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0],
                        Areanomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1],
                        Equicodi = upd.Grupocodi,
                        Equiabrev = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2],
                        Evenini = upd.Dpriefechadia,
                        Evenfin = DateTime.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3]),
                        Tipoemprdesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4],
                        Evenmwindisp = decimal.Parse((upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5] == "") ? "0" : upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]),
                        Evendescrip = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6],
                        Tipoindisponibilidad = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7],
                        Subcausadesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[8],
                        Famcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[9]),
                        Emprcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[10]),
                        Areacodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[11])
                    });
                }
            }

            string titulo1 = "PARTICIPACION DE LA POTENCIA INDISPONIBLE DE LOS EVENTOS RELEVANTES POR TIPO DE INDISPONIBILIDAD";// type = "pie";
            model.Grafico = grafico;

            if (lista.Count > 0)
            {
                model.Grafico.TitleText = titulo1;

                model.Grafico.Series = new List<RegistroSerie>();
                for (int s = 0; s < 2; s++)
                {
                    model.Grafico.SeriesData = new decimal?[s + 1][];
                    model.Grafico.Series.Add(new RegistroSerie());
                }

                for (var i = 0; i < lista.Count; i++)
                {
                    decimal? valor = 0;
                    var datos = lista.Where(x => x.Tipoindisponibilidad == lista[i].Tipoindisponibilidad).ToList();
                    model.Grafico.SeriesData[i] = new decimal?[datos.Count];
                    foreach (var arr in datos)
                    {
                        model.Grafico.Series[i].Name = (arr.Tipoindisponibilidad == "02") ? "FUERA DE SERVICIO" : "EN SERVICIO";
                        valor += arr.Evenmwindisp;
                        lista.Remove(arr);
                    }
                    model.Grafico.Series[i].Acumulado = valor;
                }
            }
            return model;
        }

        /// <summary>
        /// carga grafico difusion hechos relevantes por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionHechosRelevantesEmpresa(string idEmpresa, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie24, dfechaIni);
            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionHechosRelevantesEmpresa(listDatoPrie, dfechaIni, dfechaFin);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionHechosRelevantesEmpresa(List<SioDatoprieDTO> data, DateTime dfechaIni, DateTime dfechaFin)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<EveManttoDTO> lista = new List<EveManttoDTO>();

            if (data != null)
            {
                foreach (var upd in data)
                {
                    lista.Add(new EveManttoDTO()
                    {
                        Emprnomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0],
                        Areanomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1],
                        Equicodi = upd.Grupocodi,
                        Equiabrev = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2],
                        Evenini = upd.Dpriefechadia,
                        Evenfin = DateTime.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3]),
                        Tipoemprdesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4],
                        Evenmwindisp = decimal.Parse((upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5] == "") ? "0" : upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]),
                        Evendescrip = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6],
                        Tipoindisponibilidad = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7],
                        Subcausadesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[8],
                        Famcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[9]),
                        Emprcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[10]),
                        Areacodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[11])
                    });
                }
            }

            string titulo1 = "EVENTOS RELEVANTES POR TIPO DE INDISPONIBILIDAD POR EMPRESAS DEL SEIN", type = "column";
            model.Grafico = grafico;

            if (lista.Count > 0)
            {
                model.Grafico.YaxixTitle = "Potencia Efectiva (MW)";
                model.Grafico.TitleText = titulo1;

                model.Grafico.XAxisCategories = new List<string>();

                // Obtener lista de intervalos categoria del grafico   
                var categorias = lista.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).Distinct().ToList();
                model.Grafico.XAxisCategories = categorias.Select(x => x.Emprnomb).ToList();

                var series = lista.GroupBy(x => new { x.Tipoindisponibilidad }).Select(x => new { x.Key.Tipoindisponibilidad }).Distinct().ToList();
                model.Grafico.Series = new List<RegistroSerie>();
                for (int s = 0; s < series.Count; s++)
                {
                    model.Grafico.SeriesData = new decimal?[s + 1][];
                    model.Grafico.Series.Add(new RegistroSerie());
                }

                for (var i = 0; i < series.Count; i++)
                {
                    var datos = lista.Where(x => x.Tipoindisponibilidad == series[i].Tipoindisponibilidad).ToList();
                    model.Grafico.Series[i].Name = (series[i].Tipoindisponibilidad == "02") ? "FUERA DE SERVICIO" : "EN SERVICIO";
                    model.Grafico.Series[i].Type = type;
                    model.Grafico.Series[i].YAxis = 0;

                    model.Grafico.SeriesData[i] = new decimal?[categorias.Count];
                    for (var j = 1; j <= categorias.Count; j++)
                    {
                        decimal? valor = 0;
                        var dat = datos.Where(x => x.Emprcodi == categorias[j - 1].Emprcodi).ToList();
                        foreach (var arr in dat) { valor += arr.Evenmwindisp; }
                        model.Grafico.SeriesData[i][j - 1] = valor;
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// carga grafico difusion hechos relevantes por central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionHechosRelevantesCentral(string idEmpresa, string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            List<SioDatoprieDTO> listDatoPrie = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listDatoPrie = this._servicioSiosein.GetListaDifusion(ConstantesSioSein.Prie24, dfechaIni);
            if (listDatoPrie.Count > 0)
            {
                model = GraficoDifusionHechosRelevantesCentral(listDatoPrie, dfechaIni, dfechaFin);
            }
            model.NRegistros = listDatoPrie.Count;

            return Json(model);
        }

        private SioseinModel GraficoDifusionHechosRelevantesCentral(List<SioDatoprieDTO> data, DateTime dfechaIni, DateTime dfechaFin)
        {
            SioseinModel model = new SioseinModel();
            GraficoWeb grafico = new GraficoWeb();
            List<EveManttoDTO> lista = new List<EveManttoDTO>();

            if (data != null)
            {
                foreach (var upd in data)
                {
                    lista.Add(new EveManttoDTO()
                    {
                        Emprnomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0],
                        Areanomb = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1],
                        Equicodi = upd.Grupocodi,
                        Equiabrev = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2],
                        Evenini = upd.Dpriefechadia,
                        Evenfin = DateTime.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3]),
                        Tipoemprdesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4],
                        Evenmwindisp = decimal.Parse((upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5] == "") ? "0" : upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]),
                        Evendescrip = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6],
                        Tipoindisponibilidad = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7],
                        Subcausadesc = upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[8],
                        Famcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[9]),
                        Emprcodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[10]),
                        Areacodi = int.Parse(upd.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[11])
                    });
                }
            }

            string titulo1 = "POTENCIA INDISPONIBLE DE LOS EVENTOS RELEVANTES", type = "column";
            model.Grafico = grafico;

            if (lista.Count > 0)
            {
                model.Grafico.YaxixTitle = "Potencia Efectiva (MW)";
                model.Grafico.TitleText = titulo1;

                model.Grafico.XAxisCategories = new List<string>();

                // Obtener lista de intervalos categoria del grafico   
                var categorias = lista.GroupBy(x => new { x.Areacodi, x.Areanomb }).Select(x => new { x.Key.Areacodi, x.Key.Areanomb }).Distinct().ToList();
                model.Grafico.XAxisCategories = categorias.Select(x => x.Areanomb).ToList();

                List<EveManttoDTO> series = new List<EveManttoDTO>();
                var leyendas = lista.GroupBy(x => new { x.Famcodi }).Select(x => new { x.Key.Famcodi }).Distinct().ToList();
                for (int f = 0; f < leyendas.Count; f++)
                {
                    EveManttoDTO objDele = new EveManttoDTO();
                    switch (leyendas[f].Famcodi)
                    {
                        case ConstantesSioSein.famcodiTermoElectrica:
                        case ConstantesSioSein.IdGeneradorTermoelectrico: series.Add(new EveManttoDTO() { Famcodi = leyendas[f].Famcodi }); objDele.Famcodi = ((leyendas[f].Famcodi == ConstantesSioSein.famcodiTermoElectrica) ? ConstantesSioSein.IdGeneradorTermoelectrico : ConstantesSioSein.famcodiTermoElectrica); break;
                        case ConstantesSioSein.famcodiHidroElectrica:
                        case ConstantesSioSein.IdGeneradorHidroelectrico: series.Add(new EveManttoDTO() { Famcodi = leyendas[f].Famcodi }); objDele.Famcodi = ((leyendas[f].Famcodi == ConstantesSioSein.famcodiHidroElectrica) ? ConstantesSioSein.IdGeneradorHidroelectrico : ConstantesSioSein.famcodiHidroElectrica); break;
                        case ConstantesSioSein.famcodiSolar:
                        case ConstantesSioSein.IdGeneradorSolar: series.Add(new EveManttoDTO() { Famcodi = leyendas[f].Famcodi }); objDele.Famcodi = ((leyendas[f].Famcodi == ConstantesSioSein.famcodiSolar) ? ConstantesSioSein.IdGeneradorSolar : ConstantesSioSein.famcodiSolar); break;
                        case ConstantesSioSein.famcodiEolico:
                        case ConstantesSioSein.IdGeneradorEolico: series.Add(new EveManttoDTO() { Famcodi = leyendas[f].Famcodi }); objDele.Famcodi = ((leyendas[f].Famcodi == ConstantesSioSein.famcodiEolico) ? ConstantesSioSein.IdGeneradorEolico : ConstantesSioSein.famcodiEolico); break;
                    }
                    leyendas.Remove(leyendas[f]);
                    var dele = leyendas.Find(d => d.Famcodi == objDele.Famcodi);
                    if (dele != null) { leyendas.Remove(dele); }
                    f = -1;
                }
                model.Grafico.Series = new List<RegistroSerie>();
                for (int s = 0; s < series.Count; s++)
                {
                    model.Grafico.SeriesData = new decimal?[s + 1][]; model.Grafico.Series.Add(new RegistroSerie());
                }

                for (var i = 0; i < series.Count; i++)
                {
                    List<EveManttoDTO> datos = new List<EveManttoDTO>();
                    switch (series[i].Famcodi)
                    {
                        case ConstantesSioSein.famcodiTermoElectrica:
                        case ConstantesSioSein.IdGeneradorTermoelectrico: datos = lista.Where(x => (x.Famcodi == ConstantesSioSein.famcodiTermoElectrica || x.Famcodi == ConstantesSioSein.IdGeneradorTermoelectrico)).ToList(); break;
                        case ConstantesSioSein.famcodiHidroElectrica:
                        case ConstantesSioSein.IdGeneradorHidroelectrico: datos = lista.Where(x => (x.Famcodi == ConstantesSioSein.famcodiHidroElectrica || x.Famcodi == ConstantesSioSein.IdGeneradorHidroelectrico)).ToList(); break;
                        case ConstantesSioSein.famcodiSolar:
                        case ConstantesSioSein.IdGeneradorSolar: datos = lista.Where(x => (x.Famcodi == ConstantesSioSein.famcodiSolar || x.Famcodi == ConstantesSioSein.IdGeneradorSolar)).ToList(); break;
                        case ConstantesSioSein.famcodiEolico:
                        case ConstantesSioSein.IdGeneradorEolico: datos = lista.Where(x => (x.Famcodi == ConstantesSioSein.famcodiEolico || x.Famcodi == ConstantesSioSein.IdGeneradorEolico)).ToList(); break;
                    }
                    model.Grafico.Series[i].Name = (series[i].Famcodi == 38 || series[i].Famcodi == 39) ? "EOLICO" : ((series[i].Famcodi == 36 || series[i].Famcodi == 37) ? "SOLAR" : (series[i].Famcodi == 2 || series[i].Famcodi == 4) ? "HIDROELECTRICO" : "TERMOELECTRICO");
                    model.Grafico.Series[i].Type = type;
                    model.Grafico.Series[i].YAxis = 0;

                    model.Grafico.SeriesData[i] = new decimal?[categorias.Count];
                    for (var j = 1; j <= categorias.Count; j++)
                    {
                        decimal? valor = 0;
                        var dat = datos.Where(x => x.Areacodi == categorias[j - 1].Areacodi).ToList();
                        foreach (var arr in dat) { valor += arr.Evenmwindisp; }
                        model.Grafico.SeriesData[i][j - 1] = valor;
                    }
                }
            }
            return model;
        }

        #endregion

        #endregion

        #region TABLA 25: NUEVAS INSTALACIONES, REPOTENCIACIONES, Y/O RETIROS (REPO)

        #region VERIFICACION

        /// <summary>
        /// index instalacion de potencia retiros y ingresos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult InstallRepotenciaRetiros(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 25);

            return View(model);
        }

        /// <summary>
        /// carga lista instalacion de potencia ingresos
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaInstallRepotencia(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<OperacionComercialSein> lstOpeComercialIngreso = _servicioSiosein.ListarDatosTxtTabla25PrieRepo(fechaInicio); //SIOSEIN-PRIE-2021

                model.NRegistros = lstOpeComercialIngreso.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlInstallRepotenciaIngresoYRetiro(lstOpeComercialIngreso.Where(x => x.TipoOperacion == (int)ConstantesPR5ReportesServicio.TipoOperacion.Ingreso).ToList(), ConstantesPR5ReportesServicio.TipoOperacion.Ingreso);
                model.Resultado2 = this._servicioSiosein.GenerarRHtmlInstallRepotenciaIngresoYRetiro(lstOpeComercialIngreso.Where(x => x.TipoOperacion == (int)ConstantesPR5ReportesServicio.TipoOperacion.Retiro).ToList(), ConstantesPR5ReportesServicio.TipoOperacion.Retiro);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// validacion de instalacion de potencia retiros y ingresos
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult ValidarEquipRepotRegisRetir(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lstOpeComercial = _servicioSiosein.ListarDatosTxtTabla25PrieRepo(fechaInicio);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstOpeComercial)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Equicodi = item.Equicodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Fecha = item.FechaOperacion?.ToString("ddMMyyyy"),
                            Condicion = "01",
                            CodigoTipoEmpresa = item.OsiCodigoTipoEmpresa,
                            CodigoGrupo = item.Osinergcodi,
                            Correlativo = "0000",
                            CapacidadAnterior = item.PotCapacidadAnterior ?? 0,
                            CapacidadNueva = item.PotCapacidadNueva ?? 0,
                            Observacion = item.Observacion
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }
                var result = false;

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie25, fechaInicio, listaDatosPrie);
                result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie25, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 2;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion instalacion de potencia retiros y ingresos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionInstallRepotenciaRetiros(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<OperacionComercialSein> lstOpeComercialIngreso = _servicioSiosein.ListarDatosTxtTabla25PrieRepo(dfechaIni);

            model.ListaEmpresas = lstOpeComercialIngreso
                .Select(x => new ListaSelect
                {
                    codigo = x.Emprcodi.ToString(),
                    text = x.Empresa
                })
            .GroupBy(x => x.codigo)
            .Select(x => x.First())
            .OrderBy(x => x.text)
            .ToList();

            return View(model);

        }

        /// <summary>
        /// carga lista difusion instalacion de potencia ingresos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionInstallRepotenciaRetiros_Ing(string idEmpresa, string periodo)
        {
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie25).Cabpricodi.ToString());

            model.Resultado = this._servicioSiosein.ListarReporteDifusionInstallRepotenciaRetiros_Ing(ListaDatos, idEmpresa);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion instalacion de potencia retiros 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionInstallRepotenciaRetiros_Ret(string idEmpresa, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();


            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<EqEquipoDTO> ListaContenido = new List<EqEquipoDTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie25).Cabpricodi.ToString());

            foreach (var item in ListaDatos)
            {
                if (item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] == ConstantesSioSein.RetiroGenerador)
                {
                    EqEquipoDTO entity = new EqEquipoDTO();
                    entity.Emprcodi = Convert.ToInt32(item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[1]);
                    entity.Emprnomb = item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[2];
                    entity.Nombrecentral = item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[3];
                    entity.Equinomb = item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[4];
                    entity.Equitension = Convert.ToDecimal(item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[5]);
                    entity.Equipot = Convert.ToDecimal(item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[6]);
                    entity.Equifechfinopcom = DateTime.ParseExact(item.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[7], ConstantesSioSein.FormatFecha, CultureInfo.InvariantCulture);
                    ListaContenido.Add(entity);
                }
            }

            //model.Resultado = this._servicioSiosein.ListarReporteInstallRepotenciaRetiros_Ret(
            //    ListaContenido.Where(x => idsEmpresa.Contains((int)x.Emprcodi))
            //    .OrderBy(x => x.Equifechfinopcom)
            //    .ToList()
            //    );

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 26: PROGRAMA DE OPERACIÓN MENSUAL (POPE)

        #region VERIFICACION

        /// <summary>
        /// index verificar programa de operacion mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VerificarProgOperacionMensual(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 26);

            return View(model);
        }

        /// <summary>
        /// carga lista verificar programa de operacion mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVerificarProgOperacionMensual(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);

                _servicioSiosein.ListarDatosTxtTablaPriePOPE(fechaPeriodoPmpo, out List<MeMedicionxintervaloDTO> lista1, out List<MeMedicionxintervaloDTO> lstEnergiaProgSOGCOES);

                List<SioDatoprieDTO> lstEnergiaProgTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie26, fechaInicio);

                model.NRegistros = lstEnergiaProgSOGCOES.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlProgramadoOperacionMensual(lstEnergiaProgSOGCOES, lstEnergiaProgTXT);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de programa de operacion mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaProgOperMensual(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);

                _servicioSiosein.ListarDatosTxtTablaPriePOPE(fechaPeriodoPmpo, out List<MeMedicionxintervaloDTO> lstEnergiaProgSOGCOES, out List<MeMedicionxintervaloDTO> lista2);

                //lstEnergiaProgSOGCOES = lstEnergiaProgSOGCOES.Where(x => !string.IsNullOrEmpty(x.Osinergcodi)).ToList();
                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddMonths(11);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstEnergiaProgSOGCOES)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Grupocodi = item.Grupocodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            MesActual = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                            MesInicio = fechaIniC.ToString(ConstantesSioSein.FormatAnioMes),
                            MesFin = fechaIniF.ToString(ConstantesSioSein.FormatAnioMes),
                            MesProgramado = item.Periodo.Value.ToString(ConstantesSioSein.FormatAnioMes),
                            CodigoCentral = item.Osinergcodi,
                            ValorProgramado = item.Medinth1 ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie26, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie26, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion programa de operacion mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionProgOperacionMensual(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            model.ListaTipoGeneracion = _servicioSiosein.ListaTipoGeneracion();
            model.ListaRecursoEnergetico = _servicioSiosein.ListaTipoCombustible();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");

            return View(model);
        }

        /// <summary>
        /// cargar lista difusion programa de operacion mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionMensual(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionMensual(lista1);

            return Json(model);
        }

        /// <summary>
        /// cargar lista difusion programa de operacion mensual por empresa
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionMensualXEmpresa(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionMensualXEmpresa(lista1);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa de operacion mensual por central y recurso energetico
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionMensualXCentralRecEnerg(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionMensualXCentralRecEnerg(lista1);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa de operacion mensual por produccion de energia del sein
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult cargarListaDifusionProgOperacionMensualProdEnergSein(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionMensualProdEnergSein(lista1, dfechaIni);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa de operacion mensual por empresa
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionMensualXEmpresa(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            SioseinModel model = new SioseinModel();

            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            List<SioseinModel.ListaGenerica> listaTemp = new List<SioseinModel.ListaGenerica>();

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            List<MeMedicionxintervaloDTO> resultado = this._servicioSiosein.GetByReporteDifusionProgOperacionMensualXEmpresa(lista1);

            if (resultado.Count > 0)
            {
                decimal participaciontotal = 0;
                foreach (var temp in resultado)
                {
                    participaciontotal = participaciontotal + temp.Medinth1.GetValueOrDefault(0);
                }
                foreach (var temp in resultado)
                {
                    SioseinModel.ListaGenerica temporal = new SioseinModel.ListaGenerica();
                    temporal.String1 = temp.Equinomb;
                    temporal.Decimal1 = Math.Round(((temp.Medinth1.GetValueOrDefault(0) / participaciontotal) * 100), 2);
                    listaTemp.Add(temporal);
                }

                model.SeriesPie = listaTemp;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa de operacion mensual por produccion de energia del sein
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <returns></returns>
        public JsonResult cargarGraficoDifusionProgOperacionMensualProdEnergSein(string mesAnio, string idEmpresa, string tipoGene, string recenerg)
        {
            SioseinModel model = new SioseinModel();

            var lista1 = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            List<SioseinModel.ListaGenerica> listaTemp = new List<SioseinModel.ListaGenerica>();

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionProgOperacionMensualRecursoEnerg(ConstantesSioSein.LectCodiPrie26, dfechaIni, ConstantesSioSein.FamRerGenNoInt, idEmpresa, tipoGene, recenerg)
                .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            List<MeMedicionxintervaloDTO> resultado = this._servicioSiosein.GetByReporteDifusionProgOperacionMensualProdEnergSein(lista1);




            if (resultado.Count > 0)
            {

                List<string> cat = new List<string>();

                for (int s = 1; s < 13; s++)
                {
                    cat.Add(dfechaIni.AddMonths(s).ToString("MM/yyyy"));
                }
                model.Categoria = cat;


                string[,] matriz = new string[4, 13];

                matriz[0, 0] = "HIDROELECTRICO";
                matriz[1, 0] = "TERMOELECTRICO";
                matriz[2, 0] = "RENOVABLES";
                matriz[3, 0] = "Generacion SEIN";


                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();
                model.Serie4 = new List<decimal>();

                foreach (var tx in resultado)
                {
                    int index = 0;
                    if (tx.Tgenercodi == 1) { index = 0; }
                    if (tx.Tgenercodi == 2) { index = 1; }
                    if (tx.Tgenercodi == 3 || tx.Tgenercodi == 4) { index = 2; }

                    int index2 = 0;

                    index2 = (tx.Medintfechafin.Year - dfechaIni.Year) * 12 + (tx.Medintfechafin.Month - dfechaIni.Month);

                    if (matriz[index, index2] == "") { matriz[index, index2] = "0"; }

                    matriz[index, index2] = (Convert.ToDecimal(matriz[index, index2]) + tx.Medinth1).ToString();
                }
                for (int f = 1; f < 13; f++)
                {
                    if (matriz[0, f] == "") { matriz[0, f] = "0"; }
                    if (matriz[1, f] == "") { matriz[1, f] = "0"; }
                    if (matriz[2, f] == "") { matriz[2, f] = "0"; }

                    matriz[3, f] = (Convert.ToDecimal(matriz[0, f]) + Convert.ToDecimal(matriz[1, f]) + Convert.ToDecimal(matriz[2, f])).ToString();
                }

                for (int y = 0; y < matriz.Length; y++)
                {
                    for (int yy = 1; yy < 13; yy++)
                    {
                        if (y == 0)
                        {
                            model.Serie1.Add(Convert.ToDecimal(matriz[y, yy]));
                        }
                        if (y == 1)
                        {
                            model.Serie2.Add(Convert.ToDecimal(matriz[y, yy]));
                        }
                        if (y == 2)
                        {
                            model.Serie3.Add(Convert.ToDecimal(matriz[y, yy]));
                        }
                        if (y == 3)
                        {
                            model.Serie4.Add(Convert.ToDecimal(matriz[y, yy]));
                        }

                    }
                }


            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 27: PROGRAMA DE OPERACIÓN COSTOS MARGINALES (MENSUAL) (POCM)

        #region VERIFICACIÓN

        /// <summary>
        /// index verificar programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VerificarProgOperacionCostosMarginalesMensual(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 27);

            return View(model);
        }

        /// <summary>
        /// cargar lista verificar programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVerificarProgOperacionCostosMarginalesMensual(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(12);

                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);
                _servicioSiosein.ListarDatosTxtTablaPriePOCM(fechaPeriodoPmpo, out List<MeMedicionxintervaloDTO> listaTxt, out List<MeMedicionxintervaloDTO> listaWeb);

                model.NRegistros = listaWeb.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlProgOperacionCostosMarginalesMensual(listaWeb, fechaInicio.AddMonths(1), fechaFin);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico verificar programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="barrcodi"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoVerificarProgOperacionCostosMarginalesMensual(string mesAnio, int ptomedicodi)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddYears(1).AddDays(-1);

                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);
                _servicioSiosein.ListarDatosTxtTablaPriePOCM(fechaPeriodoPmpo, out List<MeMedicionxintervaloDTO> listaTxt, out List<MeMedicionxintervaloDTO> listaWeb);
                List<MeMedicionxintervaloDTO> lstCmgSOGCOES = listaWeb.Where(x => x.Ptomedicodi == ptomedicodi).ToList();

                model.NRegistros = lstCmgSOGCOES.Count;
                model.Grafico = _servicioSiosein.GenerarGWebProgOperacionCostosMarginalesMensual(lstCmgSOGCOES, fechaIniC, fechaIniF);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de verificar programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaProgOperCostosMarginalesMensual(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddMonths(11);

                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);
                _servicioSiosein.ListarDatosTxtTablaPriePOCM(fechaPeriodoPmpo, out List<MeMedicionxintervaloDTO> listaTxt, out List<MeMedicionxintervaloDTO> listaWeb);

                //tc
                DateTime fechaIniMesOperativo = _servicioSiosein.GetFechaInicioMesOperativoPmpo(fechaPeriodoPmpo);
                decimal tipocambio = _servicioSiosein.ObtenerTipoCambio(fechaIniMesOperativo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in listaTxt)
                {
                    decimal valorH = item.Medinth1 ?? 0;
                    decimal valorProg = (valorH * tipocambio) / 1000; //Mwh a GWh ;

                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Grupocodi = item.Barrcodi,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            MesActual = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                            MesInicio = fechaIniC.ToString(ConstantesSioSein.FormatAnioMes),
                            MesFin = fechaIniF.ToString(ConstantesSioSein.FormatAnioMes),
                            MesProgramado = item.Periodo.Value.ToString(ConstantesSioSein.FormatAnioMes),
                            CodigoBarra = item.Osinergcodi,
                            CodigoBloqueHorario = item.Pmbloqnombre,
                            ValorProgramado = valorProg
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie27, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie27, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionProgOperacionCostosMarginalesMensual(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            List<ListaSelect> Barras = new List<ListaSelect>();
            List<ListaSelect> BarrasTemp = new List<ListaSelect>();

            List<ListaSelect> Tensiones = new List<ListaSelect>();
            List<ListaSelect> TensionesTemp = new List<ListaSelect>();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");


            BarrasTemp = servFormato.ListPtoMedicionMeLectura(ConstantesSioSein.Origlectcodi, ConstantesSioSein.LectCodiPrie27, ConstantesSioSein.TipoInfocodi27).Where(x => x.Orden != 0).Select(x => new ListaSelect
            {
                /*id = x.Ptomedicodi,*/
                codigo = x.Osicodi,
                text = x.Ptomedielenomb.Trim()
            }).Distinct().ToList();

            if (BarrasTemp.Count > 0)
            {
                foreach (var tmp in BarrasTemp)
                {
                    ListaSelect temporal = new ListaSelect();
                    temporal.codigo = tmp.codigo;
                    temporal.text = tmp.text;
                    if (Barras.Count > 0)
                    {
                        int encontrado = 0;
                        foreach (var tmp2 in Barras)
                        {
                            if (tmp2.codigo == tmp.codigo)
                            {
                                encontrado = 1;
                            }
                        }
                        if (encontrado == 0)
                        {
                            Barras.Add(temporal);
                        }

                    }
                    else
                    {
                        Barras.Add(temporal);
                    }
                }
            }
            model.Barras = Barras;


            TensionesTemp = servFormato.ListPtoMedicionMeLectura(ConstantesSioSein.Origlectcodi, ConstantesSioSein.LectCodiPrie27, ConstantesSioSein.TipoInfocodi27).Where(x => x.Orden != 0).Select(x => new ListaSelect
            {
                /*id = x.Ptomedicodi,*/
                id = (int)x.Orden,
                text = x.Ptomedidesc.Trim()
            }).Distinct().ToList();


            if (TensionesTemp.Count > 0)
            {
                foreach (var tmp in TensionesTemp)
                {
                    ListaSelect temporal = new ListaSelect();
                    temporal.id = tmp.id;
                    temporal.text = tmp.text;
                    if (Barras.Count > 0)
                    {
                        int encontrado = 0;
                        foreach (var tmp2 in Tensiones)
                        {
                            if (tmp2.id == tmp.id)
                            {
                                encontrado = 1;
                            }
                        }
                        if (encontrado == 0)
                        {
                            Tensiones.Add(temporal);
                        }

                    }
                    else
                    {
                        Tensiones.Add(temporal);
                    }
                }
            }
            model.Tensiones = Tensiones;

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            //model.ListaTipoGeneracion = servicio.ListaTipoGeneracion();
            //model.ListaRecursoEnergetico = servicio.ListaTipoCombustible();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idBarras"></param>
        /// <param name="idTension"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionCostosMarginalesMensual(string mesAnio, string idEmpresa, string idBarras, string idTension)
        {
            var model = new SioseinModel();
            var listaX = new List<SioDatoprieDTO>();
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            var barras = idBarras.Split(',');
            string barra = "";
            for (int f = 0; f < barras.Length; f++)
            {
                barra = barra + "'" + barras[f] + "\',";
            }

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            //    lista1 = this.servicio.GetListaVerificarProgOperacionCostosMarginalesMensual(ConstantesSioSein.LectCodiPrie27, ConstantesSioSein.origlectcodi, dfechaIni);
            listaX = this._servicioSiosein.GetListaDifusionByEmpLectPtomedFechaOrden(idEmpresa, ConstantesSioSein.LectCodiPrie27, barra.Substring(0, barra.Length - 1), dfechaIni, idTension);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionCostosMarginalesMensual(listaX, dfechaIni);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa de operacion de costos marginales mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idBarras"></param>
        /// <param name="idTension"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionCostosMarginalesMensual(string mesAnio, string idEmpresa, string idBarras, string idTension)
        {
            SioseinModel model = new SioseinModel();
            List<string> substaciones = new List<string>();
            var dataX = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            var barras = idBarras.Split(',');
            string barra = "";
            for (int f = 0; f < barras.Length; f++)
            {
                barra = barra + "'" + barras[f] + "\',";
            }

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            dataX = this._servicioSiosein.GetListaDifusionByEmpLectPtomedFechaOrden(idEmpresa, ConstantesSioSein.LectCodiPrie27, barra.Substring(0, barra.Length - 1), dfechaIni, idTension);

            List<string> Tensiones = new List<string>();
            List<string> Bars = new List<string>();

            string[,] matriz = new string[60, 6];


            if (dataX.Count > 0)
            {
                var datos = dataX[0].Dprievalor.Split('@');

                foreach (var temp in dataX)
                {
                    var campos = temp.Dprievalor.Split('@');
                    if (Bars.Count > 0)
                    {
                        int encontrado = 0;
                        foreach (var rr in Bars)
                        {
                            if (rr == campos[0])
                            {
                                encontrado = 1;
                            }
                        }
                        if (encontrado == 0)
                        {
                            Bars.Add(campos[0]);
                        }

                    }
                    else
                    {
                        Bars.Add(campos[0]);
                    }
                }




                foreach (var temp in dataX)
                {
                    var campos = temp.Dprievalor.Split('@');
                    if (Tensiones.Count > 0)
                    {
                        int encontrado = 0;
                        foreach (var rr in Tensiones)
                        {
                            if (rr == campos[2])
                            {
                                encontrado = 1;
                            }
                        }
                        if (encontrado == 0)
                        {
                            Tensiones.Add(campos[2]);
                        }

                    }
                    else
                    {
                        Tensiones.Add(campos[2]);
                    }
                }

                matriz = new string[12 * Bars.Count, /*(Tensiones.Count) + 1*/ 6];



                int x = 0;
                int contador = 0;
                foreach (var temp in dataX)
                {
                    if (contador == Tensiones.Count)
                    {
                        x = x + 1;
                        contador = 0;
                    }
                    var campos = temp.Dprievalor.Split('@');
                    if (campos.Length > 0)
                    {
                        int nSuma = 0;
                        for (int yy = 3; yy < 15; yy++)
                        {
                            for (int y = 0; y < 12 * Bars.Count; y = y + Bars.Count)
                            {
                                matriz[y + x, 0] = campos[1];
                            }
                            matriz[x + yy + nSuma - 3, temp.Orden] = campos[yy];
                            nSuma = nSuma + Bars.Count - 1;
                        }
                    }
                    contador++;

                }

                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();
                model.Serie4 = new List<decimal>();
                model.Serie5 = new List<decimal>();

                for (int xx = 0; xx < matriz.GetLength(0); xx++)
                {
                    model.Categoria.Add(matriz[xx, 0]);
                    decimal temp1;
                    model.Serie1.Add(Decimal.TryParse(matriz[xx, 1], out temp1) ? temp1 : default(decimal));
                    decimal temp2;
                    model.Serie2.Add(Decimal.TryParse(matriz[xx, 2], out temp2) ? temp2 : default(decimal));
                    decimal temp3;
                    model.Serie3.Add(Decimal.TryParse(matriz[xx, 3], out temp3) ? temp3 : default(decimal));
                    decimal temp4;
                    model.Serie4.Add(Decimal.TryParse(matriz[xx, 4], out temp4) ? temp4 : default(decimal));
                    decimal temp5;
                    model.Serie5.Add(Decimal.TryParse(matriz[xx, 5], out temp5) ? temp5 : default(decimal));
                }




            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 28: COSTOS DE OPERACIÓN PROGRAMADO MENSUAL (POCV)

        #region VERIFICACIÓN

        /// <summary>
        /// carga verificar costos operacion de programa mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VerificarCostosOperacionProgMensual(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 28);

            return View(model);
        }

        /// <summary>
        /// carga lista verificacion costos operacion de programa mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVerificarCostosOperacionProgMensual(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);

                List<MeMedicionxintervaloDTO> lstDataRepCostoProgramado = _servicioSiosein.ObtenerDataReporteCostosOperacionProgMensual(fechaPeriodoPmpo);

                model.NRegistros = lstDataRepCostoProgramado.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlCostosOperacionProgMensual(lstDataRepCostoProgramado, fechaInicio);
                model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionProgMensual(lstDataRepCostoProgramado);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de costos operacion de programa mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosOperacionProgMensual(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);

                List<MeMedicionxintervaloDTO> lstData = _servicioSiosein.ListarDatosTxtTablaPriePOCV(fechaPeriodoPmpo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lstData)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            MesActual = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                            Periodo = item.Periodo.Value.ToString(ConstantesSioSein.FormatAnioMes),
                            Valor = item.Medinth1 ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie28, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie28, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion costos operacion de programa mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosOperacionProgMensual(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos operacion de programa mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosOperacionProgMensual(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();
            var listaAct = new List<SioDatoprieDTO>();
            var listaAnt = new List<SioDatoprieDTO>();
            List<MeMedicionxintervaloDTO> listaResult = new List<MeMedicionxintervaloDTO>();

            idEmpresa = "-1";

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }


            listaAct = this._servicioSiosein.GetListaDifusionCostosOperacionProgMensual(ConstantesSioSein.Prie28, dfechaIni, dfechaFin, idEmpresa);
            listaAnt = this._servicioSiosein.GetListaDifusionCostosOperacionProgMensual(ConstantesSioSein.Prie28, dfechaIni.AddMonths(-1), dfechaFin.AddMonths(-1), idEmpresa);

            listaResult = this._servicioSiosein.UnirListarCostosOperacionProgMensualPrie(listaAct, listaAnt);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionCostosOperacionProgMensual(listaResult);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion costos operacion de programa mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionCostosOperacionProgMensual(string mesAnio, string idEmpresa)
        {
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            List<MeMedicionxintervaloDTO> listaResult = new List<MeMedicionxintervaloDTO>();
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }


            List<SioDatoprieDTO> listaAct = this._servicioSiosein.GetListaDifusionCostosOperacionProgMensual(ConstantesSioSein.Prie28, dfechaIni, dfechaFin, idEmpresa);
            List<SioDatoprieDTO> listaAnt = this._servicioSiosein.GetListaDifusionCostosOperacionProgMensual(ConstantesSioSein.Prie28, dfechaIni.AddMonths(-1), dfechaFin.AddMonths(-1), idEmpresa);

            listaResult = this._servicioSiosein.UnirListarCostosOperacionProgMensualPrie(listaAct, listaAnt);

            if (listaResult.Count > 0)
            {
                model.NomGrafico = "COSTO DE OPERACION PROGRAMADO MENSUAL";

                int mes = dfechaIni.Month;

                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();

                for (int x = 1; x < 13; x++)
                {
                    model.Categoria.Add(dfechaIni.AddMonths(x).ToString("MM/yyyy"));
                }

                foreach (var tmp in listaResult)
                {
                    model.Serie1.Add(tmp.ProyActual.GetValueOrDefault(0));
                    model.Serie2.Add(tmp.ProyAnterior.GetValueOrDefault(0));
                    model.Serie3.Add(tmp.Variacion.GetValueOrDefault(0));
                }

            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 29: RESULTADOS DE EMBALSES ESTACIONALES PROGRAMADO MENSUAL (POLJ)

        #region VERIFICACIÓN

        /// <summary>
        /// index verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult VerificarEmbalsesEstacionalesProgMensual(string periodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (string.IsNullOrEmpty(periodo)) return base.RedirectToHomeDefault();

            var model = ObtenerInformacionAMostrarEnIndex(periodo, 29);

            return View(model);
        }

        /// <summary>
        /// carga lista verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaVerificarEmbalsesEstacionalesProgMensual(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);

                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddYears(1).AddDays(-1);

                var fechaIniAnt = fechaIniC.AddYears(-1);
                var fechaFinAnt = fechaIniAnt.AddYears(1).AddDays(-1);

                List<MeMedicionxintervaloDTO> listaEmbalsesResultado = _servicioSiosein.ListarDatosTxtTablaPriePOLJ(fechaPeriodoPmpo);
                List<MeMedicionxintervaloDTO> listaEmbalsesResultadoAnt = _servicioSiosein.ListarDatosTxtTablaPriePOLJ(fechaPeriodoPmpo.AddMonths(-1));

                model.NRegistros = 1;
                model.Resultado = _servicioSiosein.GenerarRHtmlEmbalsesEstacionalesProgMensual(listaEmbalsesResultado, fechaIniC, fechaIniF);
                model.Resultado2 = _servicioSiosein.GenerarRHtmlEmbalsesEstacionalesProgMensualVolumenes(listaEmbalsesResultado, listaEmbalsesResultadoAnt, fechaIniC, fechaIniF);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga detalle grafico verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoVerificarEmbalsesEstacionalesProgMensual(string mesAnio, int ptomedicodi)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddYears(1).AddDays(-1);

                List<MeMedicionxintervaloDTO> listaEmbalsesResultado = _servicioSiosein.ListarDatosTxtTablaPriePOLJ(fechaInicio);
                listaEmbalsesResultado = listaEmbalsesResultado.Where(x => x.Ptomedicodi == ptomedicodi && x.Medintblqnumero == 6).ToList();

                model.NRegistros = listaEmbalsesResultado.Count();
                model.Grafico = _servicioSiosein.GenerarGWebCEmbalsesEstacionalesProgMensual(listaEmbalsesResultado, fechaIniC, fechaIniF);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga view grafico verificacion embalses estacionales programa mensual por volumenes
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoVerificarEmbalsesEstacionalesProgMensualVolumenes(string mesAnio, int id)
        {
            SioseinModel model = new SioseinModel();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>(); //this._servicioSiosein.GetListaVerificarEmbalsesEstacionalesProgMensualVolumenes(ConstantesSioSein.concepcodiPE, dfechaIni, id);

            model.Lista1 = lista1.Select(x => new ListaSelect
            {
                text = x.Equinomb.Trim(),
                monto = x.H1
            }).ToList();

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaEmbalsesEstacionalesProgMensual(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var fechaIniC = fechaInicio.AddMonths(1);
                var fechaIniF = fechaIniC.AddMonths(11);

                DateTime fechaPeriodoPmpo = fechaInicio.AddMonths(1);
                List<MeMedicionxintervaloDTO> listaEmbalsesResultado = _servicioSiosein.ListarDatosTxtTablaPriePOLJ(fechaPeriodoPmpo);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in listaEmbalsesResultado)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            MesActual = fechaInicio.ToString(ConstantesSioSein.FormatAnioMes),
                            MesInicio = fechaIniC.ToString(ConstantesSioSein.FormatAnioMes),
                            MesFin = fechaIniF.ToString(ConstantesSioSein.FormatAnioMes),
                            MesProgramado = item.Periodo.Value.ToString(ConstantesSioSein.FormatAnioMes),
                            CodigoEmbalse = item.Osinergcodi,
                            CodigoResultados = $"{item.CodigoResultados:D2}",
                            ValorProgramado = item.Medinth1 ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie29, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie29, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionEmbalsesEstacionalesProgMensual(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionEmbalsesEstacionalesProgMensual(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();
            var listaAct = new List<SioDatoprieDTO>();

            idEmpresa = "-1";

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;

            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }


            listaAct = this._servicioSiosein.GetListaDifusionEmbalsesEstacionalesProgMensual(ConstantesSioSein.Prie29, dfechaIni, dfechaFin, idEmpresa);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionEmbalsesEstacionalesProgMensual(listaAct, dfechaIni, 1);

            return Json(model);
        }

        /// <summary>
        /// carga view grafico verificacion embalses estacionales programa mensual
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoDifusionEmbalsesEstacionalesProgMensual(string mesAnio, int id)
        {
            SioseinModel model = new SioseinModel();
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            List<MeMedicionxintervaloDTO> lista1 = new List<MeMedicionxintervaloDTO>();

            var listaAct = new List<SioDatoprieDTO>();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            listaAct = this._servicioSiosein.GetListaDifusionEmbalsesEstacionalesProgMensual(ConstantesSioSein.Prie29, dfechaIni, dfechaFin, "-1");
            if (listaAct.Count > 0)
            {
                List<MeMedicionxintervaloDTO> dataTemporal = new List<MeMedicionxintervaloDTO>();
                //data = data1;

                foreach (var tmpA in listaAct)
                {
                    var campos = tmpA.Dprievalor.Split(ConstantesSioSein.SplitPrie[0]);

                    for (int r = 3; r < 15; r++)
                    {
                        MeMedicionxintervaloDTO tempo = new MeMedicionxintervaloDTO();
                        tempo.Osicodi = campos[0];
                        tempo.Ptomedielenomb = campos[1];
                        tempo.Ptomedidesc = campos[2];
                        tempo.Medinth1 = Convert.ToDecimal(campos[r]);
                        tempo.Medintfechaini = (DateTime)tmpA.Dprieperiodo;
                        tempo.Medintfechafin = ((DateTime)tmpA.Dpriefechadia).AddMonths(r - 3);
                        tempo.Ptomedicodi = (int)tmpA.Equicodi;
                        dataTemporal.Add(tempo);
                    }
                }
                lista1 = dataTemporal;
            }
            if (lista1.Count > 0)
            {
                lista = this._servicioSiosein.ListarByGraficoEmbalsesEstacionalesProgMensual(lista1, id);
            }

            if (lista.Count > 0)
            {
                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.NomGrafico = "EVOLUCION MENSUAL DE VOLUMENES INICIAL Y FINAL DE " + lista[0].Ptomedielenomb + " (Mm3)";

                for (int x = 1; x < 13; x++)
                {
                    model.Categoria.Add(dfechaIni.AddMonths(x).ToString("MM/yyyy"));
                }


                foreach (var tmp in lista)
                {
                    model.Serie1.Add(tmp.VolInicial.GetValueOrDefault(0));
                    model.Serie2.Add(tmp.VolFinal.GetValueOrDefault(0));

                }
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 30: PROGRAMA DE OPERACIÓN SEMANAL (PS01)

        #region VERIFICACION

        /// <summary>
        /// index programa operacion semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult ProgOperacionSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 30);

            return View(model);
        }

        /// <summary>
        /// carga lista programa operacion semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaProgOperacionSemanal(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie30, fechaInicio);

                List<MeMedicion48DTO> listaDataDespacho = _servicioSiosein.ObtenerMedidodesDespacho(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW,
                    ConstantesPR5ReportesServicio.LectDespachoProgramadoSemanal, ConstantesAppServicio.ParametroDefecto, true);

                List<MeMedicion48DTO> lstEnergActivaXUnidadGenerYTipoGener = _servicioSiosein.ObtenerListaMedicion48xAgrupacion(listaDataDespacho.GroupBy(x => new { x.Equipadre, x.Tgenercodi }));
                List<MeMedicion48DTO> lstEnergActivaXFuenteEnerg = _servicioSiosein.ObtenerListaMedicion48xAgrupacion(listaDataDespacho.GroupBy(x => new { x.Fenergcodi }));

                model.NRegistros = lstEnergActivaXUnidadGenerYTipoGener.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlProgOperacion(lstEnergActivaXUnidadGenerYTipoGener, listaDataPrieTXT);
                model.Resultado2 = _servicioSiosein.GenerarRHtmlProgOperacionSemanalXRecEnergetico(lstEnergActivaXFuenteEnerg, listaDataPrieTXT);
                model.Grafico = _servicioSiosein.GenerarGWebProgOperacion(lstEnergActivaXFuenteEnerg, listaDataPrieTXT);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = 0;
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista programa operacion semanal consolidado
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaProgOperacionSemanalConsolidado(string periodo)
        {

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            DateTime afechaIni = DateTime.Now, afechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
                afechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(-1);
                afechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            string sFechaIPeriodoAnterior = afechaIni.ToString("dd/MM/yyyy");
            string sFechaFPeriodoAnterior = afechaFin.ToString("dd/MM/yyyy");

            var model = new SioseinModel();

            List<MeMedicion48DTO> ListaDatosPeriodo = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> ListaDatosPeriodoAnterior = new List<MeMedicion48DTO>();

            model.Resultado = _servicioSiosein.ListarReporteProgOperacionSemanalConsolidado(ListaDatosPeriodo, ListaDatosPeriodoAnterior);

            return Json(model);
        }

        /// <summary>
        /// carga detalle grafico programa operacion semanal por empresa
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoProgOperacionSemanalXEmpresa(string periodo, int emprcodi, int equipadre)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie30, fechaInicio);

                List<MeMedicion48DTO> listaDataDespacho = _servicioSiosein.ObtenerMedidodesDespacho(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW,
                    ConstantesPR5ReportesServicio.LectDespachoProgramadoSemanal, emprcodi.ToString(), true);

                List<SioDatoprieDTO> lstDataPrieTXTCentral = listaDataPrieTXT.Where(x => x.Equicodi == equipadre).ToList();
                List<MeMedicion48DTO> lstProgOperaXCentral = listaDataDespacho.Where(x => x.Equipadre == equipadre).ToList();

                model.Grafico = _servicioSiosein.GenerarGWebProgOperacion(lstProgOperaXCentral, lstDataPrieTXTCentral);
                model.NRegistros = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// validacion de programa operacion semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ValidarProgramaOperacionSemanal(string periodo)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                _servicioSiosein.GuardarTablaPriePS01(fechaInicio, User.Identity.Name);
                model.ResultadoInt = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion programa operacion semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionProgOperacionSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.ReporteResumen30);
            model.ListaEmpresas = ListaContenido.GroupBy(x => x.Emprcodi).Select(x => new ListaSelect { codigo = x.First().Emprcodi.ToString(), text = x.First().Emprnomb }).OrderBy(x => x.text).ToList();

            model.ListaTipoGeneracion = _servicioSiosein.ListaTipoGeneracion();
            model.ListaRecursoEnergetico = _servicioSiosein.ListaTipoCombustible();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion semanal
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionSemanal(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionSemanal(ListaDatos, idEmpresa);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion semanal por empresa
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionSemanalXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.ReporteResumen30)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionSemanalXEmpresa(ListaContenido);
            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion semanal por empresa
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionSemanalXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> ListaResumenXEmpresa = new List<MeMedicion48DTO>();
            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.ReporteResumen30)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            ListaResumenXEmpresa = _servicioSiosein.ObtenerResumenReporteDifusionProgDiariaSemanal(ListaContenido);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaResumenXEmpresa)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;

                model.Grafico.Series.Add(registro);
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion semanal por central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionSemanalXCentralRecEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.ReporteResumen30)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            model.Resultado = _servicioSiosein.ListarReporteDifusionProgOperacionSemanalXCentralRecEnerg(ListaContenido);
            //ListarReporteDifusionProgOperacionDiarioXCentralRecEnerg
            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion semanal por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionSemanalMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.Reporte30MaxDemanEmpresa)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            model.Resultado = _servicioSiosein.ListarReporteDifusionProgOperacionSemanalMaxDemanda(ListaContenido);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion semanal por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionSemanalMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            //Obteniendo datos de la tabla Datoprie
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.Reporte30MaxDemanEmpresa)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;
                model.Grafico.Series.Add(registro);
            }
            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion semanal por central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionSemanalXCentralRecEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.Reporte30MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            List<MeMedicion48DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologiaMe48(ListaContenido, ConstantesSioSein.Grafico);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.Series = RegistrosResumenGraficoProdEnergiaXTipoTecnologiaMe48(ListaResumenContenido);

            if (ListaContenido.Count > 0)
            {
                List<string> categorias = new List<string>();

                int min = 0;
                DateTime fecha = DateTime.MinValue;
                for (int i = 1; i <= 48; i++)
                {
                    min = min + 30;
                    categorias.Add(fecha.AddMinutes(min).ToString("HH:mm"));
                }
                model.Grafico.XAxisCategories = categorias;
            }
            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion semanal por recurso energetico y tipo de tecnologia
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionSemanalXRecEnergTipTecnologia(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie30).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie30, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla30(ListaDatos, ConstantesSioSein.Reporte30MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            List<MeMedicion48DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologiaMe48(ListaContenido, ConstantesSioSein.Lista);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionSemanalXRecEnergTipTecnologia(ListaResumenContenido);

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 31: COSTOS DE OPERACIÓN PROGRAMADO SEMANAL (PS02)

        #region VERIFICACIÓN

        /// <summary>
        /// index costos de operacion semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosOperacionSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 31);

            return View(model);
        }

        /// <summary>
        /// carga lista costos de operacion semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosOperacionSemanal(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var listaDataReport = _servicioSiosein.ListarDatosTxtTabla31PrieCostoOp(fechaInicio);

                model.NRegistros = listaDataReport.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCostosOperacionEjecutadosConsolidado(listaDataReport);
                model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionEjecutadosConsolidado(listaDataReport);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de costos de operacion semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosOperacionSemanal(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var lCosto = this._servicioSiosein.ListarDatosTxtTabla31PrieCostoOp(fechaInicio);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lCosto)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = item.Medifecha.ToString(ConstantesSioSein.FormatAnioMesDia),
                            Valor = item.Programado ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie31, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie31, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion costos de operacion semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosOperacionSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();

            if (periodo != null)
            {
                int anio = Int32.Parse(periodo.Substring(3, 4));
                int mes = Int32.Parse(periodo.Substring(0, 2));
                model.Mes = new DateTime(anio, mes, 1).ToString("MM yyyy");
            }

            model.ListaEmpresas = _servicioSiosein.GetListaCriteria(ConstantesSioSein.TipoEmpresa).Select(x => new ListaSelect
            {
                id = x.Emprcodi,
                text = x.Emprnomb.Trim()
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos de operacion semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosOperacionSemanal(string mesAnio, string idEmpresa)
        {
            var model = new SioseinModel();
            var lista1 = new List<SioDatoprieDTO>();

            Session["LISTACOSTOPER_FINAL"] = null;
            List<EqEquipoDTO> listaCentrales = new List<EqEquipoDTO>();

            DateTime dfecIniMesAnt = DateTime.Now, dfecFinMesAnt = DateTime.Now, dfecIniMesAct = DateTime.Now, dfecFinMesAct = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfecIniMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(-1);
                dfecFinMesAnt = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(-1);

                dfecIniMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfecFinMesAct = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            lista1 = this._servicioSiosein.GetListaDifusionCostosOperacionDiario(ConstantesSioSein.Prie31, dfecIniMesAct, dfecFinMesAct, idEmpresa)
                                      .Where(x => x.Dprievalor.Split(ConstantesSioSein.SplitPrie[0])[0] != ConstantesSioSein.ReporteContenidoTXT).ToList();

            List<MeMedicion48DTO> listaEnergiaEJ = new List<MeMedicion48DTO>();
            var listaDataEjec = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaFinal = _servicioSiosein.UnirListaCODiarioEjecutadoSein3134(lista1, listaDataEjec, dfecFinMesAct);

            if (listaFinal.Count > 0)
            {
                Session["LISTACOSTOPER_FINAL"] = listaFinal;
            }

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion costos de operacion semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionCostosOperacionSemanal(string mesAnio, string idEmpresa)
        {
            SioseinModel model = new SioseinModel();

            int mes = Convert.ToInt32(mesAnio.Substring(0, 2));
            int anho = Convert.ToInt32(mesAnio.Substring(3, 4));

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (mesAnio != null)
            {
                mesAnio = ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(mesAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            DateTime fechaInicial = new DateTime(anho, mes, 1); ;

            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            if (Session["LISTACOSTOPER_FINAL"] != null)
            {
                listaFinal = (List<MeMedicion48DTO>)Session["LISTACOSTOPER_FINAL"];
            }

            if (listaFinal.Count > 0)
            {
                model.Categoria = new List<string>();
                model.Serie1 = new List<decimal>();
                model.Serie2 = new List<decimal>();
                model.Serie3 = new List<decimal>();

                for (int d = 1; d <= dfechaFin.Day; d++)
                {
                    model.Categoria.Add(d.ToString());
                }
                /*******************/
                if (listaFinal.Count == 2)
                {
                    for (int x = 1; x <= dfechaFin.Day; x++)
                    {
                        decimal N1 = Convert.ToDecimal(listaFinal[0].GetType().GetProperty("H" + x).GetValue(listaFinal[0], null));
                        decimal N2 = Convert.ToDecimal(listaFinal[1].GetType().GetProperty("H" + x).GetValue(listaFinal[1], null));
                        decimal variacion = 0;
                        if (N2 != 0)
                        {
                            variacion = ((N1 - N2) / N2) * 100;
                        }
                        else { variacion = 100; }

                        model.Serie1.Add(N1);
                        model.Serie2.Add(N2);
                        model.Serie3.Add(Math.Round(variacion, 2));

                    }
                }
                if (listaFinal.Count == 1)
                {
                    for (int x = 1; x <= dfechaFin.Day; x++)
                    {
                        decimal N1 = 0;
                        decimal N2 = 0;
                        decimal variacion = 0;
                        if (listaFinal[0].Grupocodi == 1)
                        {
                            N1 = Convert.ToDecimal(listaFinal[0].GetType().GetProperty("H" + x).GetValue(listaFinal[0], null));
                            variacion = 100;
                        }
                        if (listaFinal[0].Grupocodi == 2)
                        {
                            N2 = Convert.ToDecimal(listaFinal[0].GetType().GetProperty("H" + x).GetValue(listaFinal[0], null));
                            variacion = -100;
                        }

                        if (N2 != 0)
                        {
                            variacion = ((N1 - N2) / N2) * 100;
                        }
                        if (N1 == 0 && N2 == 0)
                        {
                            variacion = 0;
                        }
                        model.Serie1.Add(N1);
                        model.Serie2.Add(N2);
                        model.Serie3.Add(Math.Round(variacion, 2));
                    }
                }
            }

            model.Resultado = this._servicioSiosein.ListarReporteDifusionCostosOperacionSemanal(model.Categoria, model.Serie1, model.Serie2, model.Serie3);//SIOSEIN-PRIE-2021
            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 32: COSTOS MARGINALES PROGRAMADO SEMANAL (PS03)

        #region VERIFICACION

        /// <summary>
        /// index costos marginales semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosMarginalesSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 32);

            return View(model);
        }

        /// <summary>
        /// carga lista costos marginales semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosMarginalesSemanal(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var listaCmgStaRosa = _servicioSiosein.GetCostosMarginalesProgPorRangoFechaStaRosa(fechaInicio, fechaFin, int.Parse(ConstantesAppServicio.LectcodiProgSemanal)).OrderBy(x => x.Medifecha).ToList();

                model.NRegistros = listaCmgStaRosa.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlCostosMarginalesSemanal(listaCmgStaRosa, fechaInicio, fechaFin);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga view grafico costos marginales semanal
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoBarraCostoMargSemanal(string mesAnio, string ptomedicodi)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var listaCmgStaRosa = _servicioSiosein2.GetListaObtenerMedicion48(fechaInicio, fechaFin, ConstantesAppServicio.LectcodiProgSemanal.ToString(), ConstantesAppServicio.TipoinfocodiSoles, ptomedicodi);

                model.Grafico = _servicioSiosein.GenerarGWebCostoMarginalSemanal(listaCmgStaRosa, fechaInicio, fechaFin);
                model.NRegistros = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// validacion de costos marginales semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ValidarCostosMarginalesSemanal(string periodo)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var listaCmgStaRosa = _servicioSiosein.GetCostosMarginalesProgPorRangoFechaStaRosa(fechaInicio, fechaFin, int.Parse(ConstantesAppServicio.LectcodiProgSemanal)).OrderBy(x => x.Medifecha).ToList();

                var listaCmgStaRosa48 = _servicioSiosein.PrepararDatosCostomarginalSemanal(listaCmgStaRosa)
                    .OrderBy(x => x.Medifecha).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();

                foreach (var item in listaCmgStaRosa48)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            FechaHora = item.Medifecha,
                            CodigoBarra = item.Osicodi,
                            Valor = item.Meditotal ?? 0
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie32, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie32, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion costos marginales semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosMarginalesSemanal(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            string barras = "-1";
            var model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.Barras = _servicioSiosein.GetListaBarraArea(barras)
                                .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                .Select(x => new ListaSelect
                                {
                                    id = x.BarrCodi,
                                    text = x.BarrNombBarrTran
                                }).ToList();

            model.Tensiones = _servicioSiosein.GetListaBarraArea(barras)
                                    .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                    .Select(x => new ListaSelect
                                    {
                                        codigo = x.BarrTension,
                                        text = x.BarrTension
                                    })
                                    .GroupBy(x => x.codigo).Select(x => x.First())
                                    .ToList();

            model.AreasOperativas = _servicioSiosein.GetListaBarraArea(barras)
                                        .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                        .Select(x => new ListaSelect
                                        {
                                            id = x.AreaCodi,
                                            text = x.AreaNombre
                                        }).GroupBy(x => x.id)
                                        .Select(x => x.First())
                                        .ToList();

            List<ListaSelect> rangos = new List<ListaSelect>();
            rangos.Add(new ListaSelect { id = 5, text = "[0 - 5]" });
            rangos.Add(new ListaSelect { id = 8, text = "[6 - 8]" });
            rangos.Add(new ListaSelect { id = 12, text = "[9 - 12]" });
            rangos.Add(new ListaSelect { id = 13, text = "[> 13]" });

            model.Rangos = rangos;

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos marginales semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosMarginalesSemanal(string periodo, string barracodi)
        {
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie32).Cabpricodi.ToString());

            model.Resultado = this._servicioSiosein.ListarReporteDifusionCostosMarginalesSemanal(ListaDatos, barracodi);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion costos marginales semanal
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionCostosMarginalesSemanal(string periodo, string barracodi)
        {
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");


            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();
            List<DateTime> ListaFechas = new List<DateTime>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie32).Cabpricodi.ToString());
            ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla32(ListaDatos);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.XAxisCategories = new List<string>();

            for (DateTime dia = dfechaIni; dia <= dfechaFin; dia = dia.AddDays(1))
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Data = new List<DatosSerie>();
                registro.Name = dia.ToString(COES.Servicios.Aplicacion.Helper.ConstantesAppServicio.FormatoDiaMes);
                DatosSerie valor = new DatosSerie();

                var entity = ListaContenido.Where(x => x.Medifecha == dia)          // SIOSEIN-PRIE-2021
                                           .Select(x => new MeMedicion48DTO
                                           {
                                               Barrnombre = x.Barrnombre,
                                               Promedio = x.Promedio,
                                               Medifecha = x.Medifecha
                                           }).FirstOrDefault();
                if (entity != null)
                {
                    valor.Y = entity.Promedio;
                }
                else
                {
                    valor.Y = 0.00m;
                }
                registro.Data.Add(valor);
                //registro.Data.Add(valor); // Descomentando aqui y..
                model.Grafico.Series.Add(registro);
            }
            //model.Grafico.XAxisCategories.Add(ListaContenido[0].Barrnombre); // ...aquí se puede visualizar mejor el gráfico, para una sola barra

            var barras = ListaContenido.GroupBy(x => x.Barrcodi).Select(x => new MeMedicion48DTO { Barrcodi = x.First().Barrcodi, Barrnombre = x.First().Barrnombre }).ToList();
            foreach (var item in barras)
            {
                model.Grafico.XAxisCategories.Add(item.Barrnombre);
            }

            return Json(model);

        }

        #endregion

        #endregion

        #region TABLA 33: PROGRAMA DE OPERACIÓN DIARIO (PD01)

        #region VERIFICACIÓN

        /// <summary>
        /// index programa operacion diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult ProgOperacionDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 33);

            return View(model);
        }

        /// <summary>
        /// carga lista programa operacion diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaProgOperacionDiario(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie33, fechaInicio);

                List<MeMedicion48DTO> listaDataDespacho = _servicioSiosein.ObtenerMedidodesDespacho(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW, ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario, ConstantesAppServicio.ParametroDefecto, true);
                List<MeMedicion48DTO> lstEnergActivaXUnidadGenerYTipoGener = _servicioSiosein.ObtenerListaMedicion48xAgrupacion(listaDataDespacho.GroupBy(x => new { x.Equipadre, x.Tgenercodi }));
                List<MeMedicion48DTO> lstEnergActivaXFuenteEnerg = _servicioSiosein.ObtenerListaMedicion48xAgrupacion(listaDataDespacho.GroupBy(x => new { x.Fenergcodi }));

                model.NRegistros = lstEnergActivaXUnidadGenerYTipoGener.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlProgOperacion(lstEnergActivaXUnidadGenerYTipoGener, listaDataPrieTXT);
                model.Resultado2 = _servicioSiosein.GenerarRHtmlProgOperacionSemanalXRecEnergetico(lstEnergActivaXFuenteEnerg, listaDataPrieTXT);
                model.Grafico = _servicioSiosein.GenerarGWebProgOperacion(lstEnergActivaXFuenteEnerg, listaDataPrieTXT);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = 0;
            }

            return Json(model);
        }

        /// <summary>
        /// carga detalle grafico programa operacion diario por empresa
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaProgOperacionDiarioPorCentral(string periodo, int emprcodi, int equipadre)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<SioDatoprieDTO> listaDataPrieTXT = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie33, fechaInicio);

                List<MeMedicion48DTO> listaDataDespacho = _servicioSiosein.ObtenerMedidodesDespacho(fechaInicio, fechaFin, ConstantesAppServicio.TipoinfocodiMW,
                    ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario, emprcodi.ToString(), true);


                List<SioDatoprieDTO> lstDataPrieTXTCentral = listaDataPrieTXT.Where(x => x.Equicodi == equipadre).ToList();
                List<MeMedicion48DTO> lstProgOperaXCentral = listaDataDespacho.Where(x => x.Equipadre == equipadre).ToList();

                model.Grafico = _servicioSiosein.GenerarGWebProgOperacion(lstProgOperaXCentral, lstDataPrieTXTCentral);
                model.NRegistros = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// validacion de programa operacion diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ValidarProgramaOperacionDiario(string periodo)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                _servicioSiosein.GuardarTablaPriePD01(fechaInicio, User.Identity.Name);
                model.ResultadoInt = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region "Difusion"

        /// <summary>
        /// index difusion programa operacion diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionProgOperacionDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.PeriodoDet = periodo;

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.ReporteResumen33);
            model.ListaEmpresas = ListaContenido.GroupBy(x => x.Emprcodi).Select(x => new ListaSelect { id = x.First().Emprcodi, text = x.First().Emprnomb }).OrderBy(x => x.text).ToList();

            model.ListaTipoGeneracion = _servicioSiosein.ListaTipoGeneracion();
            model.ListaRecursoEnergetico = _servicioSiosein.ListaTipoCombustible();

            return View(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion diario
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionDiario(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionDiario(ListaDatos, idEmpresa);

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion diario por empresas
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionDiarioXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.ReporteResumen33)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionDiarioXEmpresa(ListaContenido);
            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion diario por empresas
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionDiarioXEmpresa(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> ListaResumenXEmpresa = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.ReporteResumen33)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            ListaResumenXEmpresa = _servicioSiosein.ObtenerResumenReporteDifusionProgDiariaSemanal(ListaContenido);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaResumenXEmpresa)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;

                model.Grafico.Series.Add(registro);
            }

            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion diario por central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionDiarioXCentralRecEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.ReporteResumen33)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            model.Resultado = _servicioSiosein.ListarReporteDifusionProgOperacionDiarioXCentralRecEnerg(ListaContenido);
            //ListarReporteDifusionProgOperacionDiarioXCentralRecEnerg
            return Json(model);
        }

        /// <summary>
        /// carga lista difusion programa operacion diario por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionDiarioMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.Reporte33MaxDemanEmpresa)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            model.Resultado = _servicioSiosein.ListarReporteDifusionProgOperacionDiarioMaxDemanda(ListaContenido);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion diario por maxima demanda
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionDiarioMaxDemanda(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            int anio = Int32.Parse(periodo.Substring(3, 4));
            int mes = Int32.Parse(periodo.Substring(0, 2));
            DateTime fPeriodo = new DateTime(anio, mes, 1);

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }

            //Obteniendo datos de la tabla Datoprie
            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.Reporte33MaxDemanEmpresa)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();

            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Name = item.Emprnomb;
                registro.Acumulado = item.Total;
                model.Grafico.Series.Add(registro);
            }
            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion programa operacion diario por central y recurso energetico
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionProgOperacionDiarioXCentralRecEnerg(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.Reporte33MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            List<MeMedicion48DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologiaMe48(ListaContenido, ConstantesSioSein.Grafico);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.Series = RegistrosResumenGraficoProdEnergiaXTipoTecnologiaMe48(ListaResumenContenido);

            if (ListaContenido.Count > 0)
            {
                List<string> categorias = new List<string>();

                int min = 0;
                DateTime fecha = DateTime.MinValue;
                for (int i = 1; i <= 48; i++)
                {
                    min = min + 30;
                    categorias.Add(fecha.AddMinutes(min).ToString("HH:mm"));
                }
                model.Grafico.XAxisCategories = categorias;
            }
            return Json(model);

            //public const int IdExportacionL2280MWh = 41238;
            //public const int IdImportacionL2280MWh = 41239;
        }

        /// <summary>
        /// resumen de grafico difusion programa operacion diario por tipo de tecnologia
        /// </summary>
        /// <param name="ListaContenido"></param>
        /// <returns></returns>
        public List<RegistroSerie> RegistrosResumenGraficoProdEnergiaXTipoTecnologiaMe48(List<MeMedicion48DTO> ListaContenido)
        {
            List<RegistroSerie> data = new List<RegistroSerie>();
            foreach (var item in ListaContenido)
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Data = new List<DatosSerie>();
                registro.Name = item.Fenergnomb;
                registro.Type = "area";
                for (int i = 1; i <= 48; i++)
                {
                    DatosSerie dato = new DatosSerie();
                    dato.Y = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                    registro.Data.Add(dato);
                }
                data.Add(registro);
            }

            int minutos = 0;
            int indexH = 0;

            //SIOSEIN-PRIE-2021
            bool esListaContenidoMayoraCero = (ListaContenido != null && ListaContenido.Count > 0);
            if (esListaContenidoMayoraCero)
            {
                //
                minutos = (((DateTime)ListaContenido[0].Medifecha).Hour * 60);
                minutos += ((DateTime)ListaContenido[0].Medifecha).Minute;
            }
            indexH = minutos / 30; //<<<atencion

            RegistroSerie registroColumn = new RegistroSerie();
            registroColumn.Data = new List<DatosSerie>();
            decimal maxDemanDelDia = 0.00m;
            foreach (var item in ListaContenido)
            {
                for (int i = 1; i <= 48; i++)
                {
                    if (maxDemanDelDia < ((decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null)))
                    {
                        maxDemanDelDia = ((decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null));
                    }
                }
            }
            registroColumn.Type = "column";
            registroColumn.Name = "máxima demanda del día";
            for (int i = 1; i <= 48; i++)
            {
                DatosSerie dato = new DatosSerie();
                if (i != indexH)
                {
                    dato.Y = 0.00m;
                }
                else if (i == indexH)
                {
                    dato.Y = maxDemanDelDia;
                }
                registroColumn.Data.Add(dato);
            }
            data.Add(registroColumn);

            return data;
        }

        /// <summary>
        /// resumen produccion de energia por tipo de tecnologia de difusion programa operacion diario
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ResumenProdEnergiaXTipoTecnologiaMe48(List<MeMedicion48DTO> data, string Tipo)
        {
            List<MeMedicion48DTO> ListaResumen = new List<MeMedicion48DTO>();

            #region Grafico
            if (Tipo == ConstantesSioSein.Grafico)
            {
                List<MeMedicion48DTO> ListCateEmprTec = new List<MeMedicion48DTO>();
                bool catBagazoReRAgregado = false, catBiogazReRAgregado = false, catCarbonAgregado = false, catDiesel2Agregado = false, catEolicoAgregado = false,
                     catEolicoReRAgregado = false, catAguaAgregado = false, catAguaReRAgregado = false, catSolarAgregado = false;
                foreach (var item in data)
                {
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && !catEolicoAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerEolica, Fenergnomb = ConstantesSioSein.Eolico });
                        catEolicoAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerSolar && !catSolarAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerSolar, Fenergnomb = ConstantesSioSein.Solar });
                        catSolarAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer == ConstantesSioSein.SiReR && !catAguaReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerAgua, Fenergnomb = ConstantesSioSein.HidroRER });
                        catAguaReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && !catAguaAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerAgua, Fenergnomb = ConstantesSioSein.Hidro });
                        catAguaAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Tipogenerrer == ConstantesSioSein.SiReR && !catBagazoReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerBagazo, Fenergnomb = ConstantesSioSein.BagazoReR });
                        catBagazoReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Tipogenerrer == ConstantesSioSein.SiReR && !catBiogazReRAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerBiogas, Fenergnomb = ConstantesSioSein.BiogasReR });
                        catBiogazReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon && !catCarbonAgregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerCarbon, Fenergnomb = ConstantesSioSein.CarbonReporte });
                        catCarbonAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5 && !catDiesel2Agregado)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerDieselB5, Fenergnomb = ConstantesSioSein.Diesel2Reporte });
                        catDiesel2Agregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && !catEolicoReRAgregado && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        ListaResumen.Add(new MeMedicion48DTO { Fenergcodi = ConstantesSioSein.FuenteEnerEolica, Fenergnomb = ConstantesSioSein.EolicoRer });
                        catEolicoReRAgregado = true;
                    }
                    if (item.Fenergcodi == ConstantesSioSein.GasLaIsla || item.Fenergcodi == ConstantesSioSein.GasDeAguaytia
                        || item.Fenergcodi == ConstantesSioSein.GasDeCamisea || item.Fenergcodi == ConstantesSioSein.GasDeMalacas)
                    {
                        if (item.Tipogrupocodi == ConstantesSioSein.GrupoCoGeneracion)
                        {
                            ListaResumen.Add(new MeMedicion48DTO { Fenergnomb = string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion) });
                            ListCateEmprTec.Add(new MeMedicion48DTO
                            {
                                Fenergnomb = string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion),
                                Fenergcodi = item.Emprcodi
                            });
                        }
                        else
                        {
                            ListaResumen.Add(new MeMedicion48DTO { Fenergnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb) });
                            ListCateEmprTec.Add(new MeMedicion48DTO
                            {
                                Fenergnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb),
                                Fenergcodi = item.Emprcodi
                            });
                        }
                    }
                }
                foreach (var item in ListaResumen)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal inicializacion = 0.00m;
                        DateTime medifecha = (DateTime)data[0].Medifecha;
                        item.GetType().GetProperty("H" + i.ToString()).SetValue(item, inicializacion);
                        item.Medifecha = medifecha;
                    }
                }
                foreach (var cat in ListaResumen)
                {
                    foreach (var item in data)
                    {
                        if (cat.Fenergnomb == ConstantesSioSein.Eolico && cat.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerSolar && item.Fenergcodi == ConstantesSioSein.FuenteEnerSolar)
                        {
                            for (int i = 1; i <= 48; i++)
                            {
                                //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.HidroRER && cat.Fenergcodi == ConstantesSioSein.FuenteEnerAgua
                            && item.Tipogenerrer == ConstantesSioSein.SiReR && item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua)
                        {
                            for (int i = 1; i <= 48; i++)
                            {
                                //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.Hidro && cat.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo &&
                            item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.BiogasReR && cat.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas &&
                            item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon && item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5 && item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        if (cat.Fenergnomb == ConstantesSioSein.EolicoRer && item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Tipogenerrer == ConstantesSioSein.SiReR)
                        {
                            for (int i = 1; i <= 48; i++)
                            {   //SIOSEIN-PRIE-2021
                                //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                //

                                //SIOSEIN-PRIE-2021
                                decimal itemH = ObtenerValor(item, i);
                                decimal valueAnterior = ObtenerValor(cat, i);
                                //

                                cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                            }
                        }
                        foreach (var EmprTec in ListCateEmprTec)
                        {
                            if (cat.Fenergnomb == EmprTec.Fenergnomb
                                && EmprTec.Fenergnomb == string.Format("{0} {2} - {1}", item.Ctgdetnomb, item.Emprnomb, ConstantesSioSein.Cogeneracion)
                                )
                            {
                                for (int i = 1; i <= 48; i++)
                                {   //SIOSEIN-PRIE-2021
                                    //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                    //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                    //

                                    //SIOSEIN-PRIE-2021
                                    decimal itemH = ObtenerValor(item, i);
                                    decimal valueAnterior = ObtenerValor(cat, i);
                                    //

                                    cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                                }
                            }
                            else if (cat.Fenergnomb == EmprTec.Fenergnomb
                                && EmprTec.Fenergnomb == string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb))
                            {
                                for (int i = 1; i <= 48; i++)
                                {   //SIOSEIN-PRIE-2021
                                    //decimal itemH = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
                                    //decimal valueAnterior = (decimal)cat.GetType().GetProperty("H" + i.ToString()).GetValue(cat, null);
                                    //

                                    //SIOSEIN-PRIE-2021
                                    decimal itemH = ObtenerValor(item, i);
                                    decimal valueAnterior = ObtenerValor(cat, i);
                                    //

                                    cat.GetType().GetProperty("H" + i.ToString()).SetValue(cat, (valueAnterior + itemH));
                                }
                            }
                        }

                    }
                }
            }
            #endregion

            #region Lista
            if (Tipo == ConstantesSioSein.Lista)
            {
                foreach (var item in data)
                {
                    //FILTRO PARA HIDRICOS
                    if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer != ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.Hidrico)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion48DTO entity = new MeMedicion48DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.Hidrico;
                            entity.Emprnomb = ConstantesSioSein.Hidroelectricas;
                            entity.MaxDemanda = item.MaxDemanda;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            //SIOSEIN-PRIE-2021
                            //decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.Hidrico)
                            //                            .Select(y => y.MaxDemanda).First();
                            //decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            //

                            //SIOSEIN-PRIE-2021
                            var valor = ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.Hidrico).Select(y => y.MaxDemanda).First();
                            decimal valorAnterior = (valor != null) ? (decimal)valor : 0;
                            decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                            decimal valorSumado = valorAnterior + valorMaxDemanda;
                            //

                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.Hidrico).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA GAS NATURAL
                    else if (item.Fenergcodi == ConstantesSioSein.GasLaIsla || item.Fenergcodi == ConstantesSioSein.GasDeAguaytia
                        || item.Fenergcodi == ConstantesSioSein.GasDeCamisea || item.Fenergcodi == ConstantesSioSein.GasDeMalacas)
                    {
                        MeMedicion48DTO entity = new MeMedicion48DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = item.Fenergnomb;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionSiEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA CARBON
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerCarbon)
                    {
                        MeMedicion48DTO entity = new MeMedicion48DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = item.Fenergnomb;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA DIESEL 2
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion48DTO entity = new MeMedicion48DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = item.Fenergnomb;
                            entity.Emprnomb = ConstantesSioSein.MotoresDiesel;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {   //SIOSEIN-PRIE-2021
                            //decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5)
                            //                            .Select(y => y.MaxDemanda).First();
                            //decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            //

                            //SIOSEIN-PRIE-2021
                            var valor = ListaResumen.Where(x => x.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5).Select(y => y.MaxDemanda).First();
                            decimal valorAnterior = (valor != null) ? (decimal)valor : 0;
                            decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                            decimal valorSumado = valorAnterior + valorMaxDemanda;
                            //

                            ListaResumen.Where(x => x.Fenergcodi == ConstantesSioSein.FuenteEnerDieselB5).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA HIDRICO (RER)
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerAgua && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.HidroRER)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion48DTO entity = new MeMedicion48DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.HidroRER;
                            entity.Emprnomb = ConstantesSioSein.Hidroelectricas20MV;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            //SIOSEIN-PRIE-2021
                            //decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.HidroRER)
                            //                            .Select(y => y.MaxDemanda).First();
                            //decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            //

                            //SIOSEIN-PRIE-2021
                            var valor = ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.HidroRER).Select(y => y.MaxDemanda).First();
                            decimal valorAnterior = (valor != null) ? (decimal)valor : 0;
                            decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                            decimal valorSumado = valorAnterior + valorMaxDemanda;
                            //

                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.HidroRER).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA EOLICO RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerEolica && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.EolicoRer)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion48DTO entity = new MeMedicion48DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.EolicoRer;
                            entity.Emprnomb = ConstantesSioSein.Aerogenerador;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            //SIOSEIN-PRIE-2021
                            //decimal valorAnterior = (decimal) ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.EolicoRer)
                            //                            .Select(y => y.MaxDemanda).First();
                            //decimal valorSumado = valorAnterior + (decimal) item.MaxDemanda;
                            //

                            //SIOSEIN-PRIE-2021
                            var valor = ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.EolicoRer).Select(y => y.MaxDemanda).First();
                            decimal valorAnterior = (valor != null) ? (decimal)valor : 0;
                            decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                            decimal valorSumado = valorAnterior + valorMaxDemanda;
                            //

                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.EolicoRer).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }//FILTRO PARA BAGAZO RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBagazo && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        MeMedicion48DTO entity = new MeMedicion48DTO();
                        entity.Fenergcodi = item.Fenergcodi;
                        entity.Fenergnomb = ConstantesSioSein.BagazoReR;
                        entity.Emprcodi = item.Emprcodi;
                        entity.Emprnomb = string.Format("{0} - {1}", item.Ctgdetnomb, item.Emprnomb);
                        entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                        entity.MaxDemanda = item.MaxDemanda;
                        ListaResumen.Add(entity);
                    }//FILTRO PARA BIOGÁS RER
                    else if (item.Fenergcodi == ConstantesSioSein.FuenteEnerBiogas && item.Tipogenerrer == ConstantesSioSein.SiReR)
                    {
                        bool FuenteEnergExist = false;
                        foreach (var recursoEner in ListaResumen)
                        {
                            if (recursoEner.Fenergnomb == ConstantesSioSein.BiogasReR)
                            {
                                FuenteEnergExist = true;
                            }
                        }
                        if (!FuenteEnergExist)//Si no existe, creamos la categoria y le asignamos valor
                        {
                            MeMedicion48DTO entity = new MeMedicion48DTO();
                            entity.Fenergcodi = item.Fenergcodi;
                            entity.Fenergnomb = ConstantesSioSein.BiogasReR;
                            entity.Emprnomb = ConstantesSioSein.MDieselBiogas;
                            entity.PropiedadGas = ConstantesSioSein.VerificacionNoEsGas;
                            entity.MaxDemanda = item.MaxDemanda;
                            ListaResumen.Add(entity);
                        }
                        else //Si existe, solo aumentamos el valor
                        {
                            //SIOSEIN-PRIE-2021
                            //decimal valorAnterior = (decimal)ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.BiogasReR)
                            //                            .Select(y => y.MaxDemanda).First();
                            //decimal valorSumado = valorAnterior + (decimal)item.MaxDemanda;
                            //

                            //SIOSEIN-PRIE-2021
                            var valor = ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.BiogasReR).Select(y => y.MaxDemanda).First();
                            decimal valorAnterior = (valor != null) ? (decimal)valor : 0;
                            decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                            decimal valorSumado = valorAnterior + valorMaxDemanda;
                            //

                            ListaResumen.Where(x => x.Fenergnomb == ConstantesSioSein.BiogasReR).FirstOrDefault().MaxDemanda = valorSumado;
                        }
                    }
                }
                decimal total = 0;

                //SIOSEIN-PRIE-2021
                //foreach (var item in ListaResumen) { total = total + (decimal)item.MaxDemanda; item.Medifecha = data[0].Medifecha; }
                //foreach (var item in ListaResumen) { item.PorcentParticipacion = ((item.MaxDemanda / total) * 100); }
                //

                //SIOSEIN-PRIE-2021
                foreach (var item in ListaResumen)
                {
                    decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                    total += valorMaxDemanda;
                    item.Medifecha = data[0].Medifecha;
                }

                foreach (var item in ListaResumen)
                {
                    decimal valorMaxDemanda = (item.MaxDemanda != null) ? (decimal)item.MaxDemanda : 0;
                    decimal valorParticipacion = (total != 0) ? (valorMaxDemanda / total) : 0;
                    item.PorcentParticipacion = (valorParticipacion * 100);
                }
            }
            #endregion

            return ListaResumen;
        }

        /// <summary>
        /// envio de datos a tablas prie de difusion programa operacion diario por recurso energetico y tipo de tecnologia
        /// </summary>
        /// <param name="tipParametro"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoGene"></param>
        /// <param name="recenerg"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionProgOperacionDiarioXRecEnergTipTecnologia(string tipParametro, string idEmpresa, string tipoGene, string recenerg, string periodo)
        {
            var idsEmpresa = idEmpresa.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsTipoGen = tipoGene.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();
            var idsRecursoEner = recenerg.Split(ConstantesSioSein.SplitComa[0]).Select(Int32.Parse).ToList();

            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();

            //SIOSEIN-PRIE-2021
            //ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie33).Cabpricodi.ToString());
            ListaDatos = _servicioSiosein.ObtenerListaSioDatoprie2(ConstantesSioSein.Prie33, dfechaIni);
            //

            ListaContenido = _servicioSiosein.ObtenerContenidoDatosPrieTabla33(ListaDatos, ConstantesSioSein.Reporte33MaxDemanTecnologia)
                                                                                        .Where(x => idsEmpresa.Contains(x.Emprcodi))
                                                                                        .OrderBy(x => x.Emprnomb)
                                                                                        .ToList();
            List<MeMedicion48DTO> ListaResumenContenido = ResumenProdEnergiaXTipoTecnologiaMe48(ListaContenido, ConstantesSioSein.Lista);

            model.Resultado = this._servicioSiosein.ListarReporteDifusionProgOperacionDiarioXRecEnergTipTecnologia(ListaResumenContenido);

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 34: COSTOS DE OPERACIÓN PROGRAMADO DIARIO (PD02)

        #region Verificacion

        /// <summary>
        /// index costos de operacion diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosOperacionDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 34);

            return View(model);
        }

        /// <summary>
        /// carga lista costos de operacion diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosOperacionDiario(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var listaDataReport = _servicioSiosein.ListarDatosTxtTablaPrieCOST(fechaInicio);

                model.NRegistros = listaDataReport.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCostosOperacionEjecutadosConsolidado(listaDataReport);
                model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionEjecutadosConsolidado(listaDataReport);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// envio de datos a tablas prie de costos de operacion diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult SendListaCostosOperacionDiario(string mesAnio)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                var lCosto = this._servicioSiosein.ListarDatosTxtTablaPrieCOST(fechaInicio);

                var listaDatosPrie = new List<SioDatoprieDTO>();
                foreach (var item in lCosto)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            Periodo = item.Medifecha.ToString(ConstantesSioSein.FormatAnioMesDia),
                            Valor = item.Programado ?? 0
                        }
                    };
                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie34, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie34, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = -1;
            }

            return Json(model);
        }

        #endregion

        #region Difusión

        /// <summary>
        /// index difusion costos de operacion diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosOperacionDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 34);
            model.Titulo = "Difusión web - Costos operación programado diario";

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos de operacion diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosOperacionDiario(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

                List<SioDatoprieDTO> lstCostoOperacionEjec = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie14, fechaPeriodo);
                List<SioDatoprieDTO> lstCostoOperacionProg = _servicioSiosein.ObtenerDatosPrieRevertTxtAObjeto(ConstantesSioSein.Prie34, fechaPeriodo);

                var lstResultado = _servicioSiosein.ObtenerReporteCostoOperacionEjecutado(lstCostoOperacionEjec, lstCostoOperacionProg);

                model.NRegistros = lstResultado.Count;
                model.Resultado = _servicioSiosein.GenerarRHtmlCostosOperacionEjecutados(lstResultado);
                model.Grafico = _servicioSiosein.GenerarGWebCostosOperacionEjecutados(lstResultado, "PROGRAMADOS Y EJECUTADOS");
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        #endregion

        #endregion

        #region TABLA 35: COSTOS MARGINALES PROGRAMADO DIARIO (PD03)

        #region VERIFICACION

        /// <summary>
        /// index costos marginales diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult CostosMarginalesDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = ObtenerInformacionAMostrarEnIndex(periodo, 35);

            return View(model);
        }

        /// <summary>
        /// carga lista costos marginales diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosMarginalesDiario(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var listaCmgStaRosa = _servicioSiosein.GetCostosMarginalesProgPorRangoFechaStaRosa(fechaInicio, fechaFin, int.Parse(ConstantesAppServicio.LectcodiProgDiario)).OrderBy(x => x.Medifecha).ToList();

                model.NRegistros = listaCmgStaRosa.Count;
                model.Resultado = this._servicioSiosein.GenerarRHtmlCostosMarginalesSemanal(listaCmgStaRosa, fechaInicio, fechaFin);
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// carga detalle grafico costos marginales diario
        /// </summary>
        /// <param name="mesAnio"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarViewGraficoBarraCostoMargDiario(string mesAnio)
        {
            var model = new SioseinModel();
            try
            {
                var fechaInicio = DateTime.ParseExact(mesAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                List<CmCostomarginalprogDTO> listaCmgStaRosa = _servicioSiosein.GetCostomarginalprogByBarratranferencia(ConstantesAppServicio.BarrcodiSantaRosa220.ToString(), fechaInicio, fechaFin);

                model.Grafico = _servicioSiosein.GenerarGWebCostoMarginal(listaCmgStaRosa, fechaInicio, fechaFin);
                model.NRegistros = 1;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.NRegistros = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// validacion de costos marginales diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ValidarCostosMarginalesDiario(string periodo)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fechaInicio = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = fechaInicio.GetLastDateOfMonth();

                var listaCmgStaRosa = _servicioSiosein.GetCostosMarginalesProgPorRangoFechaStaRosa(fechaInicio, fechaFin, int.Parse(ConstantesAppServicio.LectcodiProgDiario)).OrderBy(x => x.Medifecha).ToList();

                var listaCmgStaRosa48 = _servicioSiosein.PrepararDatosCostomarginalSemanal(listaCmgStaRosa)
                    .OrderBy(x => x.Medifecha).ToList();

                var listaDatosPrie = new List<SioDatoprieDTO>();

                foreach (var item in listaCmgStaRosa48)
                {
                    SioDatoprieDTO entityDet = new SioDatoprieDTO
                    {
                        Dprieperiodo = fechaInicio,
                        Dprieusuario = User.Identity.Name,
                        Dpriefecha = DateTime.Now,
                        SioReporte = new SioReporteDTO()
                        {
                            FechaHora = item.Medifecha,
                            CodigoBarra = item.Osicodi,
                            Valor = item.Meditotal ?? 0
                        }
                    };

                    listaDatosPrie.Add(entityDet);
                }

                Tuple<List<SioDatoprieDTO>, List<SioCambioprieDTO>> datosPrie = _servicioSiosein.ProcesarDatosPrie(ConstantesSioSein.Prie35, fechaInicio, listaDatosPrie);
                var result = _servicioSiosein.GuardarDatosPrie(ConstantesSioSein.Prie35, fechaInicio, User.Identity.Name, datosPrie.Item1, datosPrie.Item2);

                model.ResultadoInt = result ? 1 : 0;
            }
            catch (Exception e)
            {
                Log.Error(NameController, e);

                model.Mensaje = e.ToString();
                model.Detalle = e.StackTrace;
                model.ResultadoInt = 0;
            }

            return Json(model);
        }

        #endregion

        #region Difusion

        /// <summary>
        /// index difusion costos marginales diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public ActionResult DifusionCostosMarginalesDiario(string periodo)
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            string barras = "-1";
            var model = new SioseinModel();
            model.PeriodoDet = periodo;

            model.Barras = _servicioSiosein.GetListaBarraArea(barras)
                                .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                .Select(x => new ListaSelect
                                {
                                    id = x.BarrCodi,
                                    text = x.BarrNombBarrTran
                                }).ToList();

            model.Tensiones = _servicioSiosein.GetListaBarraArea(barras)
                                    .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                    .Select(x => new ListaSelect
                                    {
                                        codigo = x.BarrTension,
                                        text = x.BarrTension
                                    })
                                    .GroupBy(x => x.codigo).Select(x => x.First())
                                    .ToList();

            model.AreasOperativas = _servicioSiosein.GetListaBarraArea(barras)
                                        .Where(x => x.BarrCodi == ConstantesSioSein.BarraCodiStaRosa220)
                                        .Select(x => new ListaSelect
                                        {
                                            id = x.AreaCodi,
                                            text = x.AreaNombre
                                        }).GroupBy(x => x.id)
                                        .Select(x => x.First())
                                        .ToList();

            List<ListaSelect> rangos = new List<ListaSelect>();
            rangos.Add(new ListaSelect { id = 5, text = "[0 - 5]" });
            rangos.Add(new ListaSelect { id = 8, text = "[6 - 8]" });
            rangos.Add(new ListaSelect { id = 12, text = "[9 - 12]" });
            rangos.Add(new ListaSelect { id = 13, text = "[> 13]" });

            model.Rangos = rangos;

            return View(model);
        }

        /// <summary>
        /// carga lista difusion costos marginales diario
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarListaDifusionCostosMarginalesDiario(string periodo, string barracodi)
        {
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");
            var model = new SioseinModel();

            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie35).Cabpricodi.ToString());

            model.Resultado = this._servicioSiosein.ListarReporteDifusionCostosMarginalesDiario(ListaDatos, barracodi);

            return Json(model);
        }

        /// <summary>
        /// carga grafico difusion costos marginales diarios
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public JsonResult CargarGraficoDifusionCostosMarginalesDiario(string periodo, string barracodi)
        {
            DateTime dfechaIni = DateTime.Now, dfechaFin = DateTime.Now;
            if (periodo != null)
            {
                periodo = ConstantesSioSein.IniDiaFecha + periodo.Replace(" ", "/");
                dfechaIni = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                dfechaFin = DateTime.ParseExact(periodo, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
            }
            string sFechaI = dfechaIni.ToString("dd/MM/yyyy");
            string sFechaF = dfechaFin.ToString("dd/MM/yyyy");


            List<SioDatoprieDTO> ListaDatos = new List<SioDatoprieDTO>();
            List<MeMedicion48DTO> ListaContenido = new List<MeMedicion48DTO>();
            List<DateTime> ListaFechas = new List<DateTime>();

            ListaDatos = _servicioSiosein.GetByCabpricodi("-1", _servicioSiosein.DevolverCabeceraPeriodo(dfechaIni, ConstantesSioSein.Prie35).Cabpricodi.ToString());
            ListaContenido = this._servicioSiosein.ObtenerContenidoDatosPrieTabla35(ListaDatos);

            var model = new SioseinModel();
            model.Grafico = new GraficoWeb();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.XAxisCategories = new List<string>();

            for (DateTime dia = dfechaIni; dia <= dfechaFin; dia = dia.AddDays(1))
            {
                RegistroSerie registro = new RegistroSerie();
                registro.Data = new List<DatosSerie>();
                registro.Name = dia.ToString(COES.Servicios.Aplicacion.Helper.ConstantesAppServicio.FormatoDiaMes);
                DatosSerie valor = new DatosSerie();

                var entity = ListaContenido.Where(x => x.Medifecha == dia)
                                           .Select(x => new MeMedicion48DTO
                                           {
                                               Barrnombre = x.Barrnombre,
                                               Promedio = x.Promedio,
                                               Medifecha = x.Medifecha
                                           }).FirstOrDefault();
                if (entity != null)
                {
                    valor.Y = entity.Promedio;
                }
                else
                {
                    valor.Y = 0.00m;
                }
                registro.Data.Add(valor);
                //registro.Data.Add(valor); // Descomentando aqui y..
                model.Grafico.Series.Add(registro);
            }
            //model.Grafico.XAxisCategories.Add(ListaContenido[0].Barrnombre); // ...aquí se puede visualizar mejor el gráfico, para una sola barra

            var barras = ListaContenido.GroupBy(x => x.Barrcodi).Select(x => new MeMedicion48DTO { Barrcodi = x.First().Barrcodi, Barrnombre = x.First().Barrnombre }).ToList();
            foreach (var item in barras)
            {
                model.Grafico.XAxisCategories.Add(item.Barrnombre);
            }

            return Json(model);

        }

        #endregion

        #endregion

    }

    public partial class TablasPrieDeclaracionMenController
    {
        #region Exportación Tablas PRIE - "SIOSEIN-PRIE-2021"

        /// <summary>
        /// Exporta los datos de las tablas PRIE a un archivo excel
        /// </summary>
        /// <param name="listaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="nombreArchivo">Nombre del archivo a exportar</param>
        /// <returns>Retorna el nombre del archivo exportado. En caso de haber error, retorna -1</returns>
        public JsonResult ExportaraExcel(List<SioExcelHoja> listaExcelHoja, string nombreArchivo)
        {
            string rutaArchivo = null;
            StringBuilder metodo = new StringBuilder();
            metodo.Append(NameController);
            metodo.Append(".ExportaraExcel(List<SioExcelHoja> listaExcelHoja, string nombreArchivo) - listaExcelHoja = ");
            metodo.Append(listaExcelHoja);
            metodo.Append(" , nombreArchivo = ");
            metodo.Append(nombreArchivo);

            try
            {
                rutaArchivo = ConfigurationManager.AppSettings[ConstantesSioSein.RutaReportePRIE].ToString();
                Log.Info("Exportación excel");
                string reporte = this._servicioSiosein.ExportarReporteaExcel(listaExcelHoja, rutaArchivo, nombreArchivo);
                return Json(reporte);
            }
            catch (Exception e)
            {
                metodo.Append(" , e.Message: ");
                metodo.Append(e.Message);
                Log.Error(metodo.ToString());
                return Json("-1");
            }
        }


        /// <summary>
        /// Exporta los datos de las tablas PRIE a un archivo de texto plano
        /// </summary>
        /// <param name="tpriecodi">Número de la tabla prie</param>
        /// <param name="periodo">Periodo de la consulta ('MM YYYY'). Ej: ('01 2021')</param>
        /// <param name="nombreArchivo">Nombre del archivo a exportar</param>
        /// <returns>Retorna el nombre del archivo exportado. En caso de haber error, retorna -1</returns>
        public JsonResult ExportaraTexto(int tpriecodi, string periodo, string nombreArchivo)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append(NameController);
            metodo.Append(".ExportaraTexto(int tpriecodi, string periodo, string nombreArchivo) - tpriecodi = ");
            metodo.Append(tpriecodi);
            metodo.Append(" , periodo = ");
            metodo.Append(periodo);
            metodo.Append(" , nombreArchivo = ");
            metodo.Append(nombreArchivo);

            try
            {
                int mes = Int32.Parse(periodo.Substring(0, 2));
                int anio = Int32.Parse(periodo.Substring(3));
                StringBuilder nombreArchivoFinal = new StringBuilder();
                if (tpriecodi < 10)
                {
                    nombreArchivoFinal.Append("0");
                }
                nombreArchivoFinal.Append(tpriecodi);
                nombreArchivoFinal.Append("_");
                nombreArchivoFinal.Append(nombreArchivo);
                nombreArchivoFinal.Append(anio.ToString().Substring(2));
                if (mes < 10)
                {
                    nombreArchivoFinal.Append("0");
                }
                nombreArchivoFinal.Append(mes.ToString());

                string rutaArchivo = ConfigurationManager.AppSettings[ConstantesSioSein.RutaReportePRIE].ToString();

                Log.Info("Exportación a un archivo de texto");
                string reporte = this._servicioSiosein.ExportarReporteaTexto(tpriecodi, periodo, rutaArchivo, nombreArchivoFinal.ToString());

                return Json(reporte);
            }
            catch (Exception e)
            {
                metodo.Append(" , e.Message: ");
                metodo.Append(e.Message);
                Log.Error(metodo.ToString());
                return Json("-1");
            }
        }

        /// <summary>
        /// Descargar archivo
        /// </summary>
        /// <param name="tipo">Tipo de archivo</param>
        /// <param name="nombreArchivo">Nombre del archivo a descargar</param>
        /// <returns>retorna el archivo descargado</returns>
        public virtual ActionResult AbrirArchivo(int tipo, string nombreArchivo)
        {
            StringBuilder rutaNombreArchivo = new StringBuilder();
            rutaNombreArchivo.Append(ConfigurationManager.AppSettings[ConstantesSioSein.RutaReportePRIE].ToString());
            rutaNombreArchivo.Append(nombreArchivo);

            StringBuilder rutaNombreArchivoDescarga = new StringBuilder();
            rutaNombreArchivoDescarga.Append(nombreArchivo);

            return File(rutaNombreArchivo.ToString(), Constantes.AppExcel, rutaNombreArchivoDescarga.ToString());
        }

        /// <summary>
        /// Devuelve el valor decimal de la propiedad H#
        /// </summary>
        /// <param name="item">MeMedicion48DTO</param>
        /// <param name="i">índice</param>
        /// <returns>retorna el valor decimal de la propiedad H#</returns>
        private static decimal ObtenerValor(MeMedicion48DTO item, int i)
        {
            decimal valor = 0;
            if (item.GetType().GetProperty("H" + i.ToString()) != null &&
                item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null) != null)
            {
                valor = (decimal)item.GetType().GetProperty("H" + i.ToString()).GetValue(item, null);
            }
            return valor;
        }

        #endregion

        /// <summary>
        /// Historial verificacion de tabla
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tpriecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarHistorialVerificacion(string periodo, int tpriecodi)
        {
            SioseinModel model = new SioseinModel();
            try
            {
                int imes = Int32.Parse(periodo.Substring(0, 2));
                int ianho = Int32.Parse(periodo.Substring(3));
                DateTime fechaProceso = new DateTime(ianho, imes, 1);

                model.ListaHistorialVerificacion = _servicioSiosein.GetByCriteriaSioCabeceradet(fechaProceso, tpriecodi);
                model.Resultado = "1";
            }
            catch (Exception e)
            {
                model.Resultado = "-1";
                model.Mensaje = e.ToString();
                Log.Error(NameController, e);
            }

            return Json(model);
        }

    }
}