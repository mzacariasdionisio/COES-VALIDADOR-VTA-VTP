using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Titularidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Titularidad.Controllers
{
    public class ConfiguracionController : BaseController
    {
        TitularidadAppServicio servTitEmp = new TitularidadAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TransferenciaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
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

        public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// Página Principal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            TransferenciaModel model = new TransferenciaModel();
            model.ListaTipoOperacion = servTitEmp.ListSiTipomigraoperacions();

            return View(model);
        }

        /// <summary>
        /// Listado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoQuery(string tipoOp, int flagStr, int flagActivo)
        {
            TransferenciaModel model = new TransferenciaModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");

                model.ListaTipoOperacion = servTitEmp.ListSiTipomigraoperacions();
                model.ListaParametro = servTitEmp.GetByCriteriaSiMigraParametros();
                model.ListaPlantilla = servTitEmp.GetByCriteriaSiMigraqueryplantillas();

                model.Resultado = this.servTitEmp.GenerarReporteListadoQuery(url, tipoOp, flagStr, flagActivo);
                model.Resultado2 = this.servTitEmp.GenerarReporteListadoPlantilla(url);
                model.Resultado3 = this.servTitEmp.GenerarReporteListadoParametro(url);
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

        #region CRUD Querybase

        /// <summary>
        /// Obtener QUERY
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerQuery(int id)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Query = servTitEmp.GetByIdSiMigraquerybase(id);
                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar query
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarQuery(string strQuery)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                SiMigraquerybaseDTO obj = strQuery != null ? serializer.Deserialize<SiMigraquerybaseDTO>(strQuery) : new SiMigraquerybaseDTO();
                obj.Miqubausucreacion = base.UserName;

                servTitEmp.GuardarQuery(obj);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region CRUD plantilla

        /// <summary>
        /// Obtener plantilla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPlantilla(int id)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Plantilla = servTitEmp.GetByIdSiMigraqueryplantilla(id);
                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar cambio plantilla
        /// </summary>
        /// <param name="strPlantilla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPlantilla(string strPlantilla)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                SiMigraqueryplantillaDTO obj = strPlantilla != null ? serializer.Deserialize<SiMigraqueryplantillaDTO>(strPlantilla) : new SiMigraqueryplantillaDTO();
                obj.Miqplausucreacion = base.UserName;

                servTitEmp.GuardarPlantilla(obj);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region CRUD parametro

        /// <summary>
        /// Obtener plantilla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerParametro(int id)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Parametro = servTitEmp.GetByIdSiMigraparametro(id);
                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar cambio parametro
        /// </summary>
        /// <param name="strParametro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarParametro(string strParametro)
        {
            var model = new TransferenciaModel();

            try
            {
                this.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                SiMigraParametroDTO obj = strParametro != null ? serializer.Deserialize<SiMigraParametroDTO>(strParametro) : new SiMigraParametroDTO();
                obj.Migparusucreacion = base.UserName;

                servTitEmp.GuardarParametro(obj);

                model.Resultado = ConstantesAppServicio.ParametroOK;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion
    }
}