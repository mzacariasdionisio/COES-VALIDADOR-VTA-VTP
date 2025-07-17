using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class ModeloEmbalseController : BaseController
    {
        private readonly HidrologiaAppServicio servicio = new HidrologiaAppServicio();

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

        public ModeloEmbalseController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ModeloEmbalseModel model = new ModeloEmbalseModel();
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Genera el reporte web del comparativo HO vs Despacho
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoEmbalse()
        {
            ModeloEmbalseModel model = new ModeloEmbalseModel();

            try
            {
                model.ListaEmbalse = servicio.GetByCriteriaCmModeloEmbalses(ConstantesAppServicio.ParametroDefecto);
                model.ConfiguracionDefault = servicio.ObtenerFiltroModeloEmbalse(DateTime.Today);

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
        /// ObtenerEmbalse
        /// </summary>
        /// <param name="modembcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEmbalse(int modembcodi)
        {
            ModeloEmbalseModel model = new ModeloEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ModeloEmbalse = servicio.ObtenerModeloEmbalseXId(modembcodi);

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
        /// GuardarEmbalse
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarEmbalse(CmModeloEmbalseDTO modelo)
        {
            ModeloEmbalseModel model = new ModeloEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();
                modelo.Modembfecvigencia = DateTime.ParseExact(modelo.ModembfecvigenciaDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.GuardarModeloEmbalse(modelo, base.UserName);

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
        /// EliminarEmbalse
        /// </summary>
        /// <param name="modembcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEmbalse(int modembcodi)
        {
            ModeloEmbalseModel model = new ModeloEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();
                servicio.EliminarModeloEmbalse(modembcodi, base.UserName);

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
        /// ListarHistorialXEmbalse
        /// </summary>
        /// <param name="modembcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarHistorialXEmbalse(int recurcodi)
        {
            ModeloEmbalseModel model = new ModeloEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListaEmbalse = servicio.ListHistorialCmModeloEmbalses(recurcodi);

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
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }


    }
}
