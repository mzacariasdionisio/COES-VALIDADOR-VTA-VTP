using COES.MVC.Intranet.Areas.PotenciaFirme.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PotenciaFirme;
using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Controllers
{
    public class UnidadGeneracionController : BaseController
    {
        private readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();
        private readonly INDAppServicio _indAppServicio = new INDAppServicio();

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

        public UnidadGeneracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la vista principal de Operación Comercial
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexOperacionComercial(int? pericodi)
        {
            var model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual();
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);
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
        /// Carga Listado de Opetación Comercial en Formato JSON
        /// </summary>
        /// <param name="pericodi">Código de periodo</param>
        /// <param name="famcodi">Código de Familia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaOperacionComercial(int pericodi, int tipo)
        {
            var model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi);

                model.Resultado = _indAppServicio.GenerarReporteHtmlOperacionComercial(regPeriodo.FechaIni, regPeriodo.FechaFin, tipo);
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
        /// Muestra la vista principal de Potencia Efectiva
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPotenciaEfectiva(int? pericodi)
        {
            var model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual();
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);
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
        /// Carga Lista de Potencia Efectiva  en Formato JSON
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaPotenciaEfectiva(int pericodi, int tipo)
        {
            var model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi);

                int aplicativo = ConstantesIndisponibilidades.AppPF;
                model.Resultado = _indAppServicio.GenerarReporteHtmlPotenciaEfectiva(aplicativo, regPeriodo.FechaIni, regPeriodo.FechaFin, tipo);
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
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(anio);
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