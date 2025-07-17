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
    public class Reporte7Controller : BaseController
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

        public Reporte7Controller()
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
        public ActionResult Cuadro7(int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.IdCuadro = ConstantesIndisponibilidades.ReportePR25Cuadro7;

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = this.indServicio.GetPeriodoActual().AddYears(-1).AddMonths(-1);
                model.AnioIni = fechaPeriodo.Year.ToString();
                model.AnioFin = fechaPeriodo.Year.ToString();

                fechaPeriodo = this.indServicio.GetPeriodoActual().AddMonths(-1);
                model.MesIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoMes);
                model.MesFin = fechaPeriodo.ToString(ConstantesAppServicio.FormatoMes);
                model.Horizonte = ConstantesIndisponibilidades.HorizonteVariableAnual;

                if (recacodi > 0)
                {
                    var regRecalculo = indServicio.GetByIdIndRecalculo(recacodi.Value);
                    var regPeriodo = indServicio.GetByIdIndPeriodo(regRecalculo.Ipericodi);
                    if (regPeriodo.Iperihorizonte == ConstantesIndisponibilidades.HorizonteVariableAnual)
                    {
                        model.AnioIni = regPeriodo.Iperianio.ToString();
                        model.AnioFin = regPeriodo.Iperianiofin.ToString();
                    }
                    else
                    {
                        var fecha1 = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                        var fecha2 = new DateTime(regPeriodo.Iperianiofin, regPeriodo.Iperimesfin, 1);
                        model.MesIni = fecha1.ToString(ConstantesAppServicio.FormatoMes);
                        model.MesFin = fecha2.ToString(ConstantesAppServicio.FormatoMes);
                        model.Horizonte = ConstantesIndisponibilidades.HorizonteVariableMensual;
                    }
                }


                model.Cuadro = this.indServicio.GetByIdIndCuadro(ConstantesIndisponibilidades.ReportePR25Cuadro7);
                model.ListaRecalculo = new List<IndRecalculoDTO>();
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
        /// Listar versiones por recalculo y cuadro
        /// </summary>
        /// <param name="cuadro"></param>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(string horizonte, int anioIni, int anioFin, string mesIni, string mesFin)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdCuadro = ConstantesIndisponibilidades.ReportePR25Cuadro7;
                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                string url = Url.Content("~/");

                GetFechasCuadro7(horizonte, anioIni, anioFin, mesIni, mesFin, out DateTime fechaIni, out DateTime fechaFin);

                int irecacodi = indServicio.GetIrecacodiCuadro7XMes(horizonte, fechaIni, fechaFin);

                model.Resultado = this.indServicio.GenerarHtmlListadoVersion(url, model.TienePermisoEditar, model.IdCuadro, irecacodi);
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
        public JsonResult CuadroAplicativoGuardar(string horizonte, int anioIni, int anioFin, string mesIni, string mesFin)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                GetFechasCuadro7(horizonte, anioIni, anioFin, mesIni, mesFin, out DateTime fechaIni, out DateTime fechaFin);

                model.IdReporte = this.indServicio.ProcesarCuadro7(horizonte, fechaIni, fechaFin, User.Identity.Name);

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
        /// Obtener fecha para cuadro 7
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        private void GetFechasCuadro7(string horizonte, int anioIni, int anioFin, string mesIni, string mesFin, out DateTime fechaIni, out DateTime fechaFin)
        {
            fechaIni = this.indServicio.GetPeriodoActual();

            if (horizonte == ConstantesIndisponibilidades.HorizonteVariableAnual)
            {
                fechaIni = new DateTime(anioIni, 1, 1);
                fechaFin = new DateTime(anioFin, 12, 31);
            }
            else
            {
                fechaIni = DateTime.ParseExact(mesIni, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                fechaFin = DateTime.ParseExact(mesFin, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddDays(-1);
            }
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

        #region Visualizar Versión del Cuadro 7

        public ActionResult ViewVersionCuadro7(int irptcodi)
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
                model.Famcodi = ConstantesHorasOperacion.IdTipoTermica;
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
        public JsonResult ViewCargarExcelWeb7(int irptcodi, string empresa, string central, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                indServicio.GenerarWebXVersionReporte7(irptcodi, empresa, central, famcodi, out HandsonModel handson7);

                model.HandsonTotal = handson7;
                model.Resultado = "1";
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
        public JsonResult GenerarArchivoExcelCuadro(int irptcodi, string flagDescargar, string empresa, string central, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (flagDescargar == ConstantesAppServicio.SI)
                {
                    var regVersion = indServicio.GetByIdIndReporte(irptcodi);
                    var regCuadro = indServicio.GetByIdIndCuadro(regVersion.Icuacodi);

                    empresa = ConstantesAppServicio.ParametroDefecto;
                    central = ConstantesAppServicio.ParametroDefecto;
                    famcodi = ConstantesHorasOperacion.IdTipoTermica;
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                indServicio.GenerarArchivoExcelXCuadroVersion(ruta, irptcodi, ConstantesAppServicio.SI, empresa, central, famcodi, DateTime.MinValue, DateTime.MinValue, out string nameFile);

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

    }
}