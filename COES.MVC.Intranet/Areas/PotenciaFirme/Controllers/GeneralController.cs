using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PotenciaFirme.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Controllers
{
    public class GeneralController : BaseController
    {
        private readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();

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
            this.pfServicio.CrearIndPeriodoAutomatico();

            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            DateTime fechaPeriodo = pfServicio.GetPeriodoActual(); // primer dia del mes anterior

            model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();
            model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(fechaPeriodo.Year);

            model.IdPeriodo = model.ListaPeriodo.First().Pfpericodi;
            model.ListaRecalculo = new List<PfRecalculoDTO>();
            if (model.ListaPeriodo.Any())
            {
                model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                model.IdRecalculo = regRecalculo != null ? regRecalculo.Pfrecacodi : 0;
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
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FechaIni = string.Empty;
                model.FechaFin = string.Empty;
                model.ListaRecalculo = new List<PfRecalculoDTO>();

                if (pfpericodi > 0)
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(pfpericodi);
                    PfPeriodoDTO regPeriodo = pfServicio.GetByIdPfPeriodo(pfpericodi);
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