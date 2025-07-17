using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class RestriccionOperativaController : BaseController
    {
        private readonly INDAppServicio indServicio;

        public RestriccionOperativaController()
        {
            indServicio = new INDAppServicio();
            log4net.Config.XmlConfigurator.Configure();
        }

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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la vista principal de Indisponibilidad de Restricciones Operativas Ejecutadas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.ListaTipoEmpresa = this.indServicio.ListarTipoEmpresas();
                model.ListaEmpresa = this.indServicio.ListarEmpresasPorTipo(ConstantesAppServicio.ParametroDefecto);
                model.ListaFamilia = this.indServicio.ListarFamilia(ConstantesAppServicio.ParametroDefecto);

                DateTime fechaPeriodo = indServicio.GetPeriodoActual();
                model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();

                if (pericodi.GetValueOrDefault(0) <= 0)
                {
                    var listaPeriodo = indServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);
                    var regpertmp = listaPeriodo.Find(x => x.FechaIni == fechaPeriodo);
                    pericodi = regpertmp.Ipericodi;
                }

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = indServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);
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
        /// Listar Restricciones operativas con filtros
        /// </summary>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RestriccionListado(int pericodi, string empresas, string tiposEquipo)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                indServicio.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni, out DateTime fechaFin);

                string url = Url.Content("~/");

                model.Resultado = indServicio.GenerarHtmlListadoResticcionesOperativas(url, fechaIni, fechaFin, empresas, tiposEquipo);
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
        /// Ver información del evento
        /// </summary>
        /// <param name="iccodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VerDetalleRestriccion(int iccodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Ieodcuadro = this.indServicio.ObtenerIeodcuadro(iccodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Ver listado de cambios
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VerHistorialCambio(int iccodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaIndIeodcuadro = indServicio.ListarHistorialCambioRestriccion(iccodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Guardar Indisponibilidad de Restricciones Operativas Ejecutadas
        /// </summary>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="lstIndRestriccion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RestriccionGuardar(int pericodi, string empresas, string tiposEquipo, List<IndIeodcuadroDTO> lstIndRestriccion)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                indServicio.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni, out DateTime fechaFin);

                indServicio.GuardarRestriccionOperativa(fechaIni, fechaFin, empresas, tiposEquipo, lstIndRestriccion, base.UserName);

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
        /// Listar empresas por tipo de empresa
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmpresaListado(string tiposEmpresa)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaEmpresa = this.indServicio.ListarEmpresasPorTipo(tiposEmpresa).OrderBy(x => x.Emprnomb).ToList();

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
        /// Retorna Listado de Periodo por año en formato JSON
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