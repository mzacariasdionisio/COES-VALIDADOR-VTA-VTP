using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ConsumoCombustible.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.ConsumoCombustible;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ConsumoCombustible.Controllers
{
    public class ConfiguracionController : BaseController
    {
        private readonly ConsumoCombustibleAppServicio servComb = new ConsumoCombustibleAppServicio();

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

        public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Tipo de Combustible

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexTipoCombustible()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            model.ListaMedida = servComb.ListSiTipoinformacions();

            return View(model);
        }

        /// <summary>
        /// Listado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TipoCombustibleListado()
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = this.servComb.GenerarTablaHtmlListadoTipoCombustible(url);
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
        /// Datos por registro
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TipoCombustibleDatos(int fenergcodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                model.FuenteEnergia = servComb.GetByIdSiFuenteenergia(fenergcodi);
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
        /// Actualizar
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <param name="osinergcodi"></param>
        /// <param name="tinfcoes"></param>
        /// <param name="tinfosi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TipoCombustibleActualizar(int fenergcodi, string osinergcodi, int tinfcoes, int tinfosi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                osinergcodi = !string.IsNullOrEmpty(osinergcodi) ? osinergcodi.ToString() : string.Empty;

                if (fenergcodi <= 0)
                    throw new ArgumentException("Debe seleccionar tipo de combustible");

                if (string.IsNullOrEmpty(osinergcodi))
                    throw new ArgumentException("Debe ingresar código osinergmin");

                if (tinfcoes <= 0)
                    throw new ArgumentException("Debe seleccionar Unidad COES");

                if (tinfosi <= 0)
                    throw new ArgumentException("Debe seleccionar Unidad Osinergmin");

                servComb.ActualizarSiFuenteEnergia(fenergcodi, osinergcodi, tinfcoes, tinfosi);

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

        #region Factor de Conversión

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFactorConversion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.ListaMedida = servComb.ListSiTipoinformacions();

            return View(model);
        }

        /// <summary>
        /// Listado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FactorConversionListado()
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = this.servComb.GenerarTablaHtmlListadoFactorConversion(url);
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
        /// Datos por registro
        /// </summary>
        /// <param name="tconvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FactorConversionDatos(int tconvcodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                model.FactorConversion = servComb.GetByIdSiFactorconversion(tconvcodi);
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
        /// Actualizar
        /// </summary>
        /// <param name="tconvcodi"></param>
        /// <param name="tconvfactor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FactorConversionActualizar(int tconvcodi, decimal tconvfactor)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (tconvcodi <= 0)
                    throw new ArgumentException("Debe seleccionar Factor de Conversion");

                if (tconvfactor <= 0)
                    throw new ArgumentException("Debe ingresar valor de factor de conversión");

                servComb.ActualizarFactorConversion(tconvcodi, tconvfactor);

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
        /// Guardar
        /// </summary>
        /// <param name="tinfdestino"></param>
        /// <param name="tinforigen"></param>
        /// <param name="tconvfactor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FactorConversionGuardar(int tinfdestino, int tinforigen, decimal tconvfactor)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (tinfdestino <= 0)
                    throw new ArgumentException("Debe seleccionar Unidad COES");

                if (tinforigen <= 0)
                    throw new ArgumentException("Debe seleccionar Unidad Osinergmin");

                if (tconvfactor <= 0)
                    throw new ArgumentException("Debe ingresar valor de factor de conversión");

                SiFactorconversionDTO reg = new SiFactorconversionDTO();
                reg.Tinfdestino = tinfdestino;
                reg.Tinforigen = tinforigen;
                reg.Tconvfactor = tconvfactor;

                servComb.GuardarFactorConversion(reg);

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
