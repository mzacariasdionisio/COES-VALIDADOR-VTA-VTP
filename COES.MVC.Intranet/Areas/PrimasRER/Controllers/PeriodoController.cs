using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PrimasRER;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using log4net;
using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class PeriodoController : BaseController
    {
        #region Declaración de variables
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly INDAppServicio indAppServicio = new INDAppServicio();
        private readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

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

        public PeriodoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        /// <summary>
        /// PrimasRER.2023
        /// Muestra vista de inicio de Periodp
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                //realizar proceso 
                this.indAppServicio.CrearIndPeriodoAutomatico();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
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
        /// PrimasRER.2023
        /// Listado de periodos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPeriodos()
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();
                int idPlazoEntregaEDI = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesPrimasRER.IdPlazoEntregaEDI]);
                model.Resultado = primasRERAppServicio.GenerarHtmlListadoPeriodos(idPlazoEntregaEDI, Url.Content("~/"));
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
        /// PrimasRER.2023
        /// Listar revisiones de un periodo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRevisiones(int ipericodi)
        {
            PrimasRERModel model = new PrimasRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.IndPeriodo = this.indAppServicio.GetByIdIndPeriodo(ipericodi);
                model.Resultado = primasRERAppServicio.GenerarHtmlListadoRevisiones(Url.Content("~/"), model.TienePermisoEditar, ipericodi);
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
        /// PrimasRER.2023
        /// Crea o actualiza una revisión
        /// </summary>
        /// <param name="rerRevision"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRevision(RerRevisionDTO rerRevision)
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                
                try
                { 
                    rerRevision.Rerrevfecha = DateTime.ParseExact(rerRevision.RerrevfechaDesc, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new Exception("La fecha de creación del periodo del reporte es inválida");
                }

                if (rerRevision.Rerrevcodi > 0)
                {
                    primasRERAppServicio.ActualizarRevision(rerRevision, User.Identity.Name.Trim());
                } 
                else
                {
                    primasRERAppServicio.CrearRevision(rerRevision, User.Identity.Name.Trim());
                }

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
        /// PrimasRER.2023
        /// Obtener una revisión
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRevision(int rerrevcodi)
        {
            PrimasRERModel model = new PrimasRERModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                model.RerRevision = primasRERAppServicio.ObtenerRevision(rerrevcodi);
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
    }
}