using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class DispCalorUtilController : BaseController
    {
        readonly INDAppServicio indServicio = new INDAppServicio();

        private readonly INDAppServicio _indAppServicio;

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

        public DispCalorUtilController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult IndexCalorUtil(int? pericodi, int? recacodi, int idCuadro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

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
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Irecacodi : recacodi.GetValueOrDefault(0);
                }

                DateTime fechaInicial = regPeriodo.FechaIni;
                DateTime fechaFinal = regPeriodo.FechaFin;
                model.FechaIni = fechaInicial.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFinal.ToString(ConstantesAppServicio.FormatoFecha);
                model.ListaHoraIni = indServicio.ListarCuartoHora(1);
                model.ListaHoraFin = indServicio.ListarCuartoHora(96);

                model.ListaEmpresa = indServicio.ListarEmpresaCoGeneracion(fechaInicial, fechaFinal);
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
        /// Listado de calor util por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstCalorUtil(string strFechaIni, int hIni, string strFechaFin, int hFin, int recacodi, int verscodi, int empresas)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.Resultado = indServicio.GenerarHtmlDisponibilidadCalorUtil(verscodi, recacodi, fechaini, hIni, fechafin, hFin, empresas, out int versionMostrada, out List<EqEquipoDTO> listaCentrall);
                model.IdReporte = versionMostrada;
                model.ListaCentral = listaCentrall;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar disponibilidad de calor util
        /// </summary>
        /// <param name="lstCu"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDisponibilidadCalorUtil(List<PfDispcalorutilDTO> lstCu, int recacodi, string strFechaIni, int hIni, string strFechaFin, int hFin, int empresas)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                int hIni_ = hIni * 15;
                int hFin_ = hFin * 15;

                DateTime fechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                fechaini = fechaini.AddMinutes(hIni_);
                fechafin = fechafin.AddMinutes(hFin_);

                lstCu = lstCu != null ? lstCu : new List<PfDispcalorutilDTO>();

                indServicio.EditarDisponibilidadCalorUtil(recacodi, lstCu, fechaini, fechafin, empresas, base.UserName);

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

        [HttpPost]
        public JsonResult CargarLIstaCalorUtilIndsipo(List<PfDispcalorutilDTO> lstCu)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                lstCu = lstCu != null ? lstCu : new List<PfDispcalorutilDTO>();

                model.Resultado = indServicio.ObtenerCentralesCalorUtilNoDisponibles(lstCu);
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
        /// Muestra las versiones de cierto insumo
        /// </summary>
        /// <param name="recurso"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = indServicio.GenerarHtmlListadoVersionCu(url, model.TienePermisoEditar, recacodi);

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
        /// Obtiene los recálculos para un periodo seleccionado
        /// </summary>
        /// <param name="indpericodi"></param>
        /// <returns></returns>
        public JsonResult CargarRevisiones(int indpericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FechaIni = string.Empty;
                model.FechaFin = string.Empty;
                model.ListaRecalculo = new List<IndRecalculoDTO>();

                if (indpericodi > 0)
                {
                    model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(indpericodi);
                    IndPeriodoDTO regPeriodo = indServicio.GetByIdIndPeriodo(indpericodi);
                    model.FechaIni = regPeriodo.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    model.FechaFin = regPeriodo.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
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
    }
}