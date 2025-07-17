using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Combustibles.Controllers
{
    public class ConfiguracionController : BaseController
    {
        private readonly CombustibleAppServicio _combustibleAppServicio;
        public ConfiguracionController()
        {
            _combustibleAppServicio = new CombustibleAppServicio();
        }

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GestionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        #region Principal

        /// <summary>
        /// Index para gestor de envio de combustibles líquidos y carbón
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) 
                return base.RedirectToLogin();

            return View();
        }

        /// <summary>
        /// Actualiza registros de central fuente de energia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarCentralXFenerg()
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                _combustibleAppServicio.ActualizarCentralesPorFuenteEnergia();

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
        /// Obtiene listado de centrales fuente de energia
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ObtenerListaCentralXFenerg()
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                string htmlListado = _combustibleAppServicio.GenerarHtmlListaConfiguracionParametros(url);
                model.HtmlListado = htmlListado;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Parámetros

        [HttpGet]
        public ActionResult DetalleParametro(int id)
        {
            var reg = _combustibleAppServicio.GetByIdCbCentralxfenerg(id);
            CombustibleModel model = new CombustibleModel
            {
                Centralxfenerg = reg
            };

            model.IdGrupo = reg.Grupocodi;
            model.IdAgrup = CombustibleAppServicio.GetAgrupcodiByFenergcodi(reg.Fenergcodi);
            model.FechaConsulta = DateTime.Today.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        [HttpPost]
        public JsonResult ObtenerListadoConceptocombsResultado(int cbcxfecodi)
        {
            CombustibleModel model = new CombustibleModel
            {
                Centralxfenerg = _combustibleAppServicio.GetByIdCbCentralxfenerg(cbcxfecodi)
            };
            var estcomcodi = CombustibleAppServicio.GetEstcomcodiByFenergcodi(model.Centralxfenerg.Fenergcodi);

            var listaItemsResultado = _combustibleAppServicio.ObtenerConceptocombsResultados(cbcxfecodi, estcomcodi, model.Centralxfenerg.Fenergcodi, model.Centralxfenerg.Grupocodi);
            model.HtmlListado = _combustibleAppServicio.GenerarHtmlRegistroConfiguracionParametros(listaItemsResultado); 

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarDatoCentralXFenerg(List<CbDatosxcentralxfenergDTO> lista)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                _combustibleAppServicio.GuardarDatosxcentralxfenerg(lista, User.Identity.Name);

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

        #region Automático

        /// <summary>
        /// Ejecutar proceso automatico
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarProcesoAutomatico(int tipo)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                _combustibleAppServicio.EjecutarProcesoAutomaticoPR31(tipo);

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

    }
}
