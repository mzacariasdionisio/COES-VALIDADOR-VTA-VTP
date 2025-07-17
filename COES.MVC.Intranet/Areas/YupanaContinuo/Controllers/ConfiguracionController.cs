using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class ConfiguracionController : BaseController
    {
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();

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

        public ActionResult IndexCaudal()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ConfiguracionModel model = new ConfiguracionModel();
            model.Tyupcodi = ConstantesYupanaContinuo.TipoConfiguracionCaudal;
            model.Yupcfgcodi = yupanaServicio.GetConfiguracionBaseXTipo(model.Tyupcodi);

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        public ActionResult IndexRER()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ConfiguracionModel model = new ConfiguracionModel();
            model.Tyupcodi = ConstantesYupanaContinuo.TipoConfiguracionRer;
            model.Yupcfgcodi = yupanaServicio.GetConfiguracionBaseXTipo(model.Tyupcodi);

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Listado Reporte Configuracion
        /// </summary>
        /// <param name="yupcfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoReporteConfiguracion(int yupcfgcodi)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                model.ListaConfiguracion = yupanaServicio.ListarReporteConfiguracionXTipo(yupcfgcodi);
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Listado Filtro Configuracion
        /// </summary>
        /// <param name="tyupcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoFiltroConfiguracion(int tyupcodi)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                yupanaServicio.ListarFiltroConfiguracionRecurso(tyupcodi, ConstantesYupanaContinuo.TopologiaBase, out List<CpRecursoDTO> listaRecurso, out List<MePtomedicionDTO> listaPto);
                model.ListaRecurso = listaRecurso;
                model.ListaPto = listaPto;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar Configuracion
        /// </summary>
        /// <param name="yupcfgcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="strConf"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarConfiguracion(int yupcfgcodi, int recurcodi, string strConf)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<CpYupconCfgdetDTO> listaConf =!string.IsNullOrEmpty(strConf) ? serializer.Deserialize<List<CpYupconCfgdetDTO>>(strConf) : new List<CpYupconCfgdetDTO>();

                yupanaServicio.GuardarConfiguracionXRecurso(yupcfgcodi, recurcodi, listaConf, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    }
}