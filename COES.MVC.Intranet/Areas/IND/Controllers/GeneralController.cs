using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class GeneralController : BaseController
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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public GeneralController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// Generación de Cuadros
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //realizar proceso 
            this.indServicio.CrearIndPeriodoAutomatico();

            DateTime fechaPeriodo = indServicio.GetPeriodoActual(); // primer dia del mes anterior

            model.ListaCuadro = indServicio.GetByCriteriaIndCuadros();

            model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);

            var regPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month);
            model.IdPeriodo = regPeriodo.Ipericodi;
            model.AnioActual = regPeriodo.FechaIni.Year;
            model.ListaRecalculo = new List<IndRecalculoDTO>();
            if (model.ListaPeriodo.Any())
            {
                model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo);
                var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                model.IdRecalculo = regRecalculo != null ? regRecalculo.Irecacodi : 0;
            }

            return View(model);
        }

        /// <summary>
        /// Factores de Indisponibilidad
        /// </summary>
        /// <returns></returns>
        public ActionResult Factores()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //realizar proceso 
            this.indServicio.CrearIndPeriodoAutomatico();

            DateTime fechaPeriodo = indServicio.GetPeriodoActual(); // primer dia del mes anterior

            model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);

            var regPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month);
            model.IdPeriodo = regPeriodo.Ipericodi;
            model.AnioActual = regPeriodo.FechaIni.Year;
            model.ListaRecalculo = new List<IndRecalculoDTO>();
            if (model.ListaPeriodo.Any())
            {
                model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo, 1);

                var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                model.IdRecalculo = regRecalculo != null ? regRecalculo.Irecacodi : 0;
            }

            return View(model);
        }

        /// <summary>
        /// Cuadro 7
        /// </summary>
        /// <returns></returns>
        public ActionResult Cuadro7()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //realizar proceso 
            this.indServicio.CrearIndPeriodoAutomatico();

            DateTime fechaPeriodo = indServicio.GetPeriodoActual(); // primer dia del mes anterior

            model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);

            var regPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month);
            model.IdPeriodo = regPeriodo.Ipericodi;
            model.AnioActual = regPeriodo.FechaIni.Year;
            model.ListaRecalculo = new List<IndRecalculoDTO>();
            if (model.ListaPeriodo.Any())
            {
                model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo);
                var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                model.IdRecalculo = regRecalculo != null ? regRecalculo.Irecacodi : 0;
            }

            return View(model);
        }

        /// <summary>
        /// Listar recalculo por periodo en formato JSON
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoListado(int ipericodi, int? omitirQuincenal)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(ipericodi, omitirQuincenal);
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

                DateTime fechaPeriodo = indServicio.GetPeriodoActual(); // primer dia del mes anterior
                if (fechaPeriodo.Year == anio)
                {
                    model.IdPeriodo = model.ListaPeriodo.Find(x => x.Iperimes == fechaPeriodo.Month).Ipericodi;
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
    }
}