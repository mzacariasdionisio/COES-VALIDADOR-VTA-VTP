using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class EquivalenciaController : BaseController
    {
        ValidacionReporteAppServicio servicio = new ValidacionReporteAppServicio();

        #region Declaración de variables
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
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

        public EquivalenciaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            EquivalenciaModel model = new EquivalenciaModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresasTienenCentralGen();
            return View(model);
        }

        /// <summary>
        /// Muestra el listado de puntos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int idEmpresa)
        {
            EquivalenciaModel model = new EquivalenciaModel();
            model.ListaDespacho = this.servicio.ObtenerPuntosMedicion(2, idEmpresa);
            model.ListaMedicion = this.servicio.ObtenerPuntosMedicion(1, idEmpresa);

            return PartialView(model);
        }

        /// <summary>
        /// Permite relacionar ambos puntos de medicion
        /// </summary>
        /// <param name="idMedicion"></param>
        /// <param name="idDespacho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int idMedicion, int idDespacho)
        {
            try
            {
                int resultado = this.servicio.RelacionarPuntos(idMedicion, idDespacho, User.Identity.Name);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar las relaciones existentes
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult Relaciones(int? idEmpresa)
        {
            EquivalenciaModel model = new EquivalenciaModel();
            model.ListaRelaciones = this.servicio.ListarRelaciones(idEmpresa);

            return PartialView(model);
        }

        /// <summary>
        /// Permite quitar la relacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.DeleteWbMedidoresValidacion(id);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
