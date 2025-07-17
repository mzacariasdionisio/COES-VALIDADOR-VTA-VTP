using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class ReporteController : BaseController
    {
        readonly INDAppServicio indServicio = new INDAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public ReporteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Mostrar vista de cuadro
        /// </summary>
        /// <param name="idCuadro"></param>
        /// <returns></returns>
        public ActionResult Cuadro(int idCuadro, int? pericodi, int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.IdCuadro = idCuadro;

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdCuadro = idCuadro;
                model.Cuadro = indServicio.GetByIdIndCuadro(idCuadro);

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = indServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.MesActual = regPeriodo.FechaIni.Month;
                model.ListaAnio = indServicio.ListaAnio(regPeriodo.FechaIni).ToList();
                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<IndRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Irecacodi : recacodi.GetValueOrDefault(0);
                }
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

        #region Listar versiones - Procesar Cuadro - Editar Handson

        /// <summary>
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(anio);
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
        /// Listar recalculo por periodo en formato JSON
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoListado(int ipericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(ipericodi);
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
        /// Obtener datos de Recalculo por código de recalculo
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoDatos(int irecacodi, int idCuadro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IndRecalculo = indServicio.GetByIdIndRecalculo(irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                model.FechaIni = model.IndRecalculo.IrecafechainiDesc;
                model.FechaFin = model.IndRecalculo.IrecafechafinDesc;
                model.NumeroDias = model.IndRecalculo.Irecafechafin.Day; //IND.PR25.2022

                if (idCuadro == ConstantesIndisponibilidades.ReportePR25Cuadro2)
                {
                    indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro3FactorK, out int rptcodi3, out int numVersion, out string strComent3);

                    model.IdReporte = rptcodi3;
                    model.Mensaje = rptcodi3 > 0 ? "Versión " + numVersion + " aprobado." : strComent3;
                }

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
        /// Listar versiones por recalculo y cuadro
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int cuadro, int irecacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = this.indServicio.GenerarHtmlListadoVersion(url, model.TienePermisoEditar, cuadro, irecacodi);
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
        /// Listado verificacion data aplicativo
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="tiempo"></param>
        /// <param name="medicionorigen"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CuadroAplicativoVerificarData(int cuadro, int irecacodi, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaVerificacion = this.indServicio.ListarVerificacionCuadro(cuadro, irecacodi, famcodi);
                //Inicio: IND.PR25.2022
                if (cuadro == ConstantesIndisponibilidades.ReportePR25Cuadro3FactorK)
                {
                    model.Resultado = this.indServicio.GenerarHtmlVerificacionCuadro3FactorK(model.ListaVerificacion);
                }
                else
                {
                    model.Resultado = this.indServicio.GenerarHtmlVerificacion(model.ListaVerificacion);
                }
                //Fin: IND.PR25.2022
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Procesar el Cuadro Aplicativo
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="tiempo"></param>
        /// <param name="medicionorigen"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CuadroAplicativoGuardar(int cuadro, int irecacodi, string tiempo, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                switch (cuadro)
                {
                    case ConstantesIndisponibilidades.ReportePR25Cuadro3FactorK:
                        model.IdReporte = this.indServicio.ProcesarCuadro3(irecacodi, User.Identity.Name);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25Cuadro2:
                        model.IdReporte = this.indServicio.ProcesarCuadro2(irecacodi, tiempo, User.Identity.Name);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25Cuadro5:
                        model.IdReporte = this.indServicio.ProcesarCuadro5(irecacodi, User.Identity.Name);
                        break;
                    default:
                        model.IdReporte = this.indServicio.ProcesarCuadro(cuadro, irecacodi, tiempo, famcodi, User.Identity.Name);
                        break;
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
        /// Guardar cambios
        /// </summary>
        /// <param name="icuacodi"></param>
        /// <param name="irecacodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="irptcodi"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCambiosHandson(int icuacodi, int irecacodi, int famcodi, int irptcodi, List<IndReporteTotalDTO> listaTotData, List<IndReporteDetDTO> listaData)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                listaData = listaData != null ? listaData : new List<IndReporteDetDTO>();
                SetFechaIndReporteDet(icuacodi, listaData);

                listaTotData = listaTotData != null ? listaTotData : new List<IndReporteTotalDTO>();

                switch (icuacodi)
                {
                    case ConstantesIndisponibilidades.ReportePR25Cuadro3FactorK:
                        model.IdReporte = indServicio.GuardarCambiosHandsonCuadro3(icuacodi, irecacodi, User.Identity.Name, irptcodi, listaTotData);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25Cuadro2:
                        model.IdReporte = indServicio.GuardarCambiosHandsonCuadro2(icuacodi, irecacodi, famcodi, User.Identity.Name, irptcodi, listaData);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25Cuadro5:
                        model.IdReporte = indServicio.GuardarCambiosHandsonCuadro5(icuacodi, irecacodi, User.Identity.Name, irptcodi, listaData);
                        break;
                    default:
                        model.IdReporte = indServicio.GuardarCambiosHandsonCuadro1(icuacodi, irecacodi, famcodi, User.Identity.Name, irptcodi, listaData);
                        break;
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
        /// Aprovar version de Reporte
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarVersion(int irptcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                this.indServicio.AprobarVersionReporte(irptcodi, User.Identity.Name);
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
        /// Obtener datos de reporte por codigo
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReporteVersionDatos(int irptcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IndReporte = indServicio.GetByIdIndReporte(irptcodi);
                model.Cuadro = indServicio.GetByIdIndCuadro(model.IndReporte.Icuacodi);
                model.IndRecalculo = indServicio.GetByIdIndRecalculo(model.IndReporte.Irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
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

        private void SetFechaIndReporteDet(int icuacodi, List<IndReporteDetDTO> lista)
        {
            switch (icuacodi)
            {
                case ConstantesIndisponibilidades.ReportePR25Cuadro1:
                case ConstantesIndisponibilidades.ReportePR25Cuadro2:
                case ConstantesIndisponibilidades.ReportePR25Cuadro4:
                    foreach (var reg in lista)
                    {
                        reg.Idethoraini = null;
                        reg.Idethorafin = null;

                        if (!string.IsNullOrEmpty(reg.IdethorainiDesc))
                        {
                            reg.Idethoraini = DateTime.ParseExact(reg.IdethorainiDesc, ConstantesAppServicio.FormatoFechaYMD + " "+ ConstantesAppServicio.FormatoHora, CultureInfo.InvariantCulture);
                        }

                        if (!string.IsNullOrEmpty(reg.IdethorafinDesc))
                        {
                            reg.Idethorafin = DateTime.ParseExact(reg.IdethorafinDesc, ConstantesAppServicio.FormatoFechaYMD + " " + ConstantesAppServicio.FormatoHora, CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case ConstantesIndisponibilidades.ReportePR25Cuadro5:
                    foreach (var reg in lista)
                    {
                        reg.Idethoraini = null;
                        reg.Idethorafin = null;

                        if (!string.IsNullOrEmpty(reg.IdethorainiDesc))
                        {
                            reg.Idethoraini = DateTime.ParseExact(reg.IdethorainiDesc, ConstantesAppServicio.FormatoFechaYMD, CultureInfo.InvariantCulture);
                        }
                    }
                    break;
            }

        }

        #endregion

        #region Visualizar Versión del Cuadro 1, 2 y 4

        public ActionResult ViewVersionCuadro(int irptcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdReporte = irptcodi;
                model.IndReporte = indServicio.GetByIdIndReporte(irptcodi);
                model.Cuadro = indServicio.GetByIdIndCuadro(model.IndReporte.Icuacodi);
                model.IndRecalculo = indServicio.GetByIdIndRecalculo(model.IndReporte.Irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                model.IdCuadro = model.Cuadro.Icuacodi;
                model.Famcodi = model.Cuadro.Famcodi;
                model.FechaIni = model.IndRecalculo.IrecafechainiDesc;
                model.FechaFin = model.IndRecalculo.IrecafechafinDesc;

                indServicio.ListarFiltroXVersionReporte(irptcodi, model.Famcodi, ConstantesAppServicio.SI, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral);
                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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

        [HttpPost]
        public JsonResult ViewCargarExcelWeb(int irptcodi, string centralIntegrante, string empresa, string central, int famcodi, string strFechaIni, string strFechaFin)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                indServicio.GenerarWebXVersionReporte(irptcodi, centralIntegrante, empresa, central, famcodi, fechaini, fechafin
                        , out HandsonModel handsonProg, out HandsonModel handsonFort, out string htmlReporteConsolidado, out string htmlCambios);
                model.HandsonFort = handsonFort;
                model.HandsonProg = handsonProg;
                model.RptHtmlCambios = htmlCambios;
                model.Resultado = htmlReporteConsolidado;
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

        #region Visualizar Versión del Cuadro 3

        public ActionResult ViewVersionCuadro3(int irptcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdReporte = irptcodi;
                model.IndReporte = indServicio.GetByIdIndReporte(irptcodi);
                model.Cuadro = indServicio.GetByIdIndCuadro(model.IndReporte.Icuacodi);
                model.IndRecalculo = indServicio.GetByIdIndRecalculo(model.IndReporte.Irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                model.IdCuadro = model.Cuadro.Icuacodi;
                model.Famcodi = model.Cuadro.Famcodi;
                model.FechaIni = model.IndRecalculo.IrecafechainiDesc;
                model.FechaFin = model.IndRecalculo.IrecafechafinDesc;

                indServicio.ListarFiltroXVersionReporte(irptcodi, model.Famcodi, ConstantesAppServicio.SI, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral);
                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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

        [HttpPost]
        public JsonResult ViewCargarExcelWebFactorK(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebCuadro3(irptcodi, empresa, central, out HandsonModel handsonK, out string htmlCambios);
                model.HandsonK = handsonK;
                model.RptHtmlCambios = htmlCambios;
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

        #endregion

        #region Visualizar Versión del Cuadro 5

        public ActionResult ViewVersionCuadro5(int irptcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdReporte = irptcodi;
                model.IndReporte = indServicio.GetByIdIndReporte(irptcodi);
                model.Cuadro = indServicio.GetByIdIndCuadro(model.IndReporte.Icuacodi);
                model.IndRecalculo = indServicio.GetByIdIndRecalculo(model.IndReporte.Irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                model.IdCuadro = model.Cuadro.Icuacodi;
                model.Famcodi = model.Cuadro.Famcodi;
                model.FechaIni = model.IndRecalculo.IrecafechainiDesc;
                model.FechaFin = model.IndRecalculo.IrecafechafinDesc;

                indServicio.ListarFiltroXVersionReporte(irptcodi, model.Famcodi, ConstantesAppServicio.SI, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral);
                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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

        [HttpPost]
        public JsonResult ViewCargarExcelWebCuadro5(int irptcodi, string empresa, string central, string strFechaIni, string strFechaFin)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                indServicio.GenerarWebReporte5(irptcodi, empresa, central, fechaini, fechafin, out HandsonModel handson, out string htmlCambios, out List<string> listaMensajeNota);
                model.HandsonDisp = handson;
                model.RptHtmlCambios = htmlCambios;
                model.Resultado = "1";
                model.ListaNota = listaMensajeNota;
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

        #region Métodos comunes (Filtros y Exportación excel)

        /// <summary>
        /// Cargar los filtros comunes para todos los cuadros
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <param name="centralIntegrante"></param>
        /// <param name="empresa"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ViewCargarFiltros(int irptcodi, string centralIntegrante, string empresa, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.ListarFiltroXVersionReporte(irptcodi, famcodi, centralIntegrante, empresa, ConstantesAppServicio.ParametroDefecto, out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral);
                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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
        /// Generar archivo excel del cuadro
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <param name="centralIntegrante"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="famcodi"></param>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelCuadro(int irptcodi, string flagDescargar, string centralIntegrante, string empresa, string central, int famcodi, string strFechaIni, string strFechaFin)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (flagDescargar == ConstantesAppServicio.SI)
                {
                    var regVersion = indServicio.GetByIdIndReporte(irptcodi);
                    var regCuadro = indServicio.GetByIdIndCuadro(regVersion.Icuacodi);

                    centralIntegrante = ConstantesAppServicio.SI;
                    empresa = ConstantesAppServicio.ParametroDefecto;
                    central = ConstantesAppServicio.ParametroDefecto;
                    famcodi = regCuadro.Famcodi;
                    strFechaIni = regVersion.IrecafechainiDesc;
                    strFechaFin = regVersion.IrecafechafinDesc;
                }

                DateTime fechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                indServicio.GenerarArchivoExcelXCuadroVersion(ruta, irptcodi, centralIntegrante, empresa, central, famcodi, fechaini, fechafin, out string nameFile);

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

        [HttpGet]
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

        #region Carga histórica

        /// <summary>
        /// Mostrar vista de cuadro
        /// </summary>
        /// <param name="idCuadro"></param>
        /// <returns></returns>
        public ActionResult CargaHistorica(int? pericodi, int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);


                DateTime fechaNuevo = this.indServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;

                DateTime fechaPeriodo = indServicio.GetPeriodoActual();
                model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = indServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<IndRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Irecacodi : recacodi.GetValueOrDefault(0);
                }
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
        /// Upload 
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadExcelHistorico(FormCollection formCollection, int recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                if (stremExcel != null && recacodi > 0)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    var file = Request.Files[0];
                    model.NombreArchivoUpload = file.FileName;

                    if (model.NombreArchivoUpload.ToUpper().Contains(".XLSX"))
                    {
                        string fileRandom = System.IO.Path.GetRandomFileName();
                        string fileName = ruta + fileRandom + "." + "xlsx";

                        file.SaveAs(fileName);

                        this.indServicio.ListarDatoExcel(fileName, recacodi, out string htmlVal
                                        , out List<IndReporteTotalDTO> listaTotTermo, out string rptTermo
                                        , out List<IndReporteTotalDTO> listaTotHidro, out string rptHidro);

                        model.RptHtml1 = rptTermo;
                        model.RptHtml2 = rptHidro;

                        model.RptHtml3 = htmlVal;

                        model.ListaTotTermo = listaTotTermo;
                        model.ListaTotHidro = listaTotHidro;

                        model.Resultado = "1";
                    }
                    else
                    {
                        throw new ArgumentException("Archivo no válido. La extensión del archivo debe terminar en .XLSX");
                    }
                }
                else
                {
                    throw new ArgumentException("No existe archivo o no ha seleccionado recálculo.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
            }

            return Json(model);
        }

        /// <summary>
        /// Procesar el Cuadro Aplicativo
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="tiempo"></param>
        /// <param name="medicionorigen"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarHistoricoPR25(IndisponibilidadesModel indModel)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                this.indServicio.ProcesarHistorico(indModel.IdRecalculo, indModel.ListaTotTermo, indModel.ListaTotHidro, User.Identity.Name);

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
        /// Listar versiones por recalculo y cuadro
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListadoHistorico(int irecacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = this.indServicio.GenerarHtmlListadoVersionHistorico(url, irecacodi);
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

        #region "Compensación de pruebas aleatorias"

        #endregion

        #region IND.PR25.2022
        /// <summary>
        /// Generar archivo excel del nuevo cálculo del factor K desde Cuadro N°3,
        /// para las empresas/centrales que están registradas en Identificación de Empresas y
        /// que su gaseoducto sea mayor a 0.
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelCalculo(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarArchivoExcelReporteCalculoFactorK(irptcodi, empresa, central, out string nameFile);

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

        #endregion
    }
}