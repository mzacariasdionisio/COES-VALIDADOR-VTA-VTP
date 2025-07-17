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
    public class ReporteFactoresController : BaseController
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

        public ReporteFactoresController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="idCuadro"></param>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        public ActionResult Factor(int idCuadro, int? pericodi, int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.IdCuadro = idCuadro;

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdCuadro = idCuadro;

                DateTime fechaNuevo = this.indServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;

                model.Cuadro = indServicio.GetByIdIndCuadro(idCuadro);

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
                    model.ListaRecalculo = model.ListaRecalculo.Where(x => x.Irecatipo != ConstantesIndisponibilidades.TipoRecalculoPrimeraQuincena).ToList();

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

        #region Listar versiones y procesar Cuadro

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
                model.ListaRecalculo = model.ListaRecalculo.Where(x => x.Irecatipo != ConstantesIndisponibilidades.TipoRecalculoPrimeraQuincena).ToList();
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

                if (idCuadro == ConstantesIndisponibilidades.ReportePR25FactorFortTermico)
                {
                    DateTime fechaFinHistorico = indServicio.GetPeriodoMaxHistTermo();
                    if (model.IndPeriodo.FechaIni > fechaFinHistorico)
                    {
                        indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro1, out int rptcodi1, out int numVersion1, out string strComent1);
                        indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro2, out int rptcodi2, out int numVersion2, out string strComent2);

                        model.UsarAplicativo = 1;
                        model.IdReporte = rptcodi1;
                        model.IdReporte2 = rptcodi2;

                        model.Mensaje = rptcodi1 > 0 ? "Versión " + numVersion1 + " aprobado." : strComent1;
                        model.Mensaje2 = rptcodi2 > 0 ? "Versión " + numVersion2 + " aprobado." : strComent2;
                    }

                    indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25DisponibilidadCalorUtil, out int rptcodi3, out int numVersion, out string strComent);
                    model.IdReporte3 = rptcodi3;
                    model.Mensaje3 = rptcodi3 > 0 ? "Versión " + numVersion + " aprobado." : strComent;
                }

                if (idCuadro == ConstantesIndisponibilidades.ReportePR25FactorProgTermico)
                {
                    DateTime fechaFinHistorico = indServicio.GetPeriodoMaxHistTermo();
                    if (model.IndPeriodo.FechaIni > fechaFinHistorico)
                    {
                        indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro1, out int rptcodi1, out int numVersion1, out string strComent1);
                        indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro2, out int rptcodi2, out int numVersion2, out string strComent2);

                        model.UsarAplicativo = 1;
                        model.IdReporte = rptcodi1;
                        model.IdReporte2 = rptcodi2;

                        model.Mensaje = rptcodi1 > 0 ? "Versión " + numVersion1 + " aprobado." : strComent1;
                        model.Mensaje2 = rptcodi2 > 0 ? "Versión " + numVersion2 + " aprobado." : strComent2;
                    }

                    indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25DisponibilidadCalorUtil, out int rptcodi3, out int numVersion, out string strComent);
                    model.IdReporte3 = rptcodi3;
                    model.Mensaje3 = rptcodi3 > 0 ? "Versión " + numVersion + " aprobado." : strComent;
                }

                if (idCuadro == ConstantesIndisponibilidades.ReportePR25FactorProgHidro)
                {
                    DateTime fechaFinHistorico = indServicio.GetPeriodoMaxHistHidro();
                    if (model.IndPeriodo.FechaIni > fechaFinHistorico)
                    {
                        indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro4, out int rptcodi, out int numVersion, out string strComent);

                        model.UsarAplicativo = 1;
                        model.IdReporte = rptcodi;
                        model.Mensaje = rptcodi > 0 ? "Versión " + numVersion + " aprobado." : strComent;
                    }
                }

                if (idCuadro == ConstantesIndisponibilidades.ReportePR25FactorPresencia)
                {
                    indServicio.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25Cuadro5, out int rptcodi, out int numVersion, out string strComent);

                    model.IdReporte = rptcodi;
                    model.Mensaje = rptcodi > 0 ? "Versión " + numVersion + " aprobado." : strComent;
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
        public JsonResult FactorAplicativoVerificarData(int cuadro, int irecacodi, int idReporteCuadro1, int idReporteCuadro2, int idReporteCuadro14, int idReporteCuadro4, int idReporteCuadro5)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaVerificacion = this.indServicio.ListarVerificacionFactor(cuadro, irecacodi, idReporteCuadro1, idReporteCuadro2, idReporteCuadro14, idReporteCuadro4, idReporteCuadro5
                                                                                    , out bool flagContinuar);
                model.Resultado = this.indServicio.GenerarHtmlVerificacion(model.ListaVerificacion);
                model.FlagContinuar = flagContinuar;
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
        public JsonResult FactorAplicativoGuardar(int cuadro, int irecacodi, int idReporteCuadro1, int idReporteCuadro2, int idReporteCuadro14, int idReporteCuadro4, int idReporteCuadro5)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                switch (cuadro)
                {
                    case ConstantesIndisponibilidades.ReportePR25FactorFortTermico:
                        model.IdReporte = this.indServicio.ProcesarFactorFortuitaMensualTermica(irecacodi, idReporteCuadro1, idReporteCuadro2, idReporteCuadro14, base.UserName);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25FactorProgTermico:
                        model.IdReporte = this.indServicio.ProcesarFactorProgramadoTermico(irecacodi, idReporteCuadro1, idReporteCuadro2, idReporteCuadro14, base.UserName);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25FactorProgHidro:
                        model.IdReporte = this.indServicio.ProcesarFactorProgramadoHidro(irecacodi, idReporteCuadro4, base.UserName);
                        break;
                    case ConstantesIndisponibilidades.ReportePR25FactorPresencia:
                        model.IdReporte = this.indServicio.ProcesarFactorPresencia(irecacodi, idReporteCuadro5, base.UserName);
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

        #endregion

        #region Visualizar Versión - Factor Fortuito mensual Termico

        public ActionResult ViewFactorFortTermico(int irptcodi)
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
        public JsonResult ViewCargarExcelWebFactorFortTermico(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebReporteFortTermico(irptcodi, empresa, central, out HandsonModel handson);
                model.HandsonFF = handson;
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

        #region Visualizar Versión - Factor Programado mensual y anual Termico

        public ActionResult ViewFactorProgTermico(int irptcodi)
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
        public JsonResult ViewCargarExcelWebFactorProgTermico(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebReporteProgramadoTermo(irptcodi, empresa, central, out HandsonModel handson);
                model.HandsonFF = handson;
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

        #region Visualizar Versión - Factor Programado Anual y Mensual Hidro

        public ActionResult ViewFactorProgramadoHidro(int irptcodi)
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
        public JsonResult ViewCargarExcelWebFactorProgramadoHidro(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebReporteProgramadoHidro(irptcodi, empresa, central, out HandsonModel handson);
                model.HandsonFP = handson;
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

        #region Visualizar Versión - Factor Presencia Hidráulica

        public ActionResult ViewFactorPresencia(int irptcodi)
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
        public JsonResult ViewCargarExcelWebFactorPresencia(int irptcodi, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebReporteFP(irptcodi, empresa, central, out HandsonModel handson);
                model.HandsonFP = handson;
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
        public JsonResult GenerarArchivoExcelCuadro(int irptcodi, string flagDescargar, string empresa, string central)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (flagDescargar == ConstantesAppServicio.SI)
                {
                    empresa = ConstantesAppServicio.ParametroDefecto;
                    central = ConstantesAppServicio.ParametroDefecto;
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                indServicio.GenerarArchivoExcelFactoresXCuadroVersion(ruta, irptcodi, empresa, central, out string nameFile);

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

        ///// <summary>
        ///// Guarda los cambio del cuadro 1 y 4 
        ///// </summary>
        ///// <param name="icuacodi"></param>
        ///// <param name="irecacodi"></param>
        ///// <param name="famcodi"></param>
        ///// <param name="irptcodi"></param>
        ///// <param name="listaData"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GuardarCambiosHandson(int icuacodi, int irecacodi, int famcodi, int irptcodi, List<IndReporteDetDTO> listaData)
        //{
        //    IndisponibilidadesModel model = new IndisponibilidadesModel();

        //    try
        //    {
        //        base.ValidarSesionJsonResult();
        //        model.IdRecalculo = indServicio.GuardarCambiosHandson(icuacodi, irecacodi, famcodi, User.Identity.Name, irptcodi, listaData);

        //        model.Resultado = "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(NameController, ex);
        //        model.Resultado = "-1";
        //        model.Mensaje = ex.Message;
        //        model.Detalle = ex.StackTrace;
        //    }

        //    return Json(model);


        //}

        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
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

    }
}