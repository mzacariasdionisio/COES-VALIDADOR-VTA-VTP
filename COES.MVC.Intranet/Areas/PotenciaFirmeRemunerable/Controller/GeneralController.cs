using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Controller
{
    public class GeneralController : BaseController
    {
        private readonly PotenciaFirmeRemunerableAppServicio pfrServicio = new PotenciaFirmeRemunerableAppServicio();

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

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            //realizar proceso 
            this.pfrServicio.CrearIndPeriodoAutomatico();

            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            DateTime fechaPeriodo = pfrServicio.GetPeriodoActual(); // primer dia del mes anterior

            model.ListaAnio = pfrServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = pfrServicio.GetByCriteriaPfrPeriodos(fechaPeriodo.Year);

            model.IdPeriodo = model.ListaPeriodo.First().Pfrpercodi;
            model.ListaRecalculo = new List<PfrRecalculoDTO>();
            if (model.ListaPeriodo.Any())
            {
                model.ListaRecalculo = pfrServicio.GetByCriteriaPfrRecalculos(model.IdPeriodo);
                var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                model.IdRecalculo = regRecalculo != null ? regRecalculo.Pfrreccodi : 0;
            }

            return View(model);
        }

        /// <summary>
        /// Obtiene los recálculos para un periodo seleccionado
        /// </summary>
        /// <param name="nomPeriodo"></param>
        /// <returns></returns>
        public JsonResult CargarRevisiones(int pfpericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FechaIni = string.Empty;
                model.FechaFin = string.Empty;
                model.ListaRecalculo = new List<PfrRecalculoDTO>();

                if (pfpericodi > 0)
                {
                    model.ListaRecalculo = pfrServicio.GetByCriteriaPfrRecalculos(pfpericodi);
                    PfrPeriodoDTO regPeriodo = pfrServicio.GetByIdPfrPeriodo(pfpericodi);
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
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = pfrServicio.GetByCriteriaPfrPeriodos(anio);
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