using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class ParametroController : BaseController
    {
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();
        ServiceReferenceYupanaContinuo.YupanaContinuoServicioClient cliente = new ServiceReferenceYupanaContinuo.YupanaContinuoServicioClient();

        private bool UsarCliente = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.UsarWebServiceYupanaContinuo] == "S";

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

        public ParametroController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SimulacionModel model = new SimulacionModel();
            model.ListaHora = yupanaServicio.ListarHoras(DateTime.Now.Hour);
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        [HttpPost]
        public JsonResult SimularEjecucionManual(string fecha, int hora)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string usuario = base.UserName;

                //Validación
                yupanaServicio.ValidarPrevioSimulacion(ConstantesYupanaContinuo.SimulacionManual, fecha, hora, out DateTime fechaHora, out int topcodi);

                if (UsarCliente)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => cliente.EjecutarYupanaContinuoManual(fecha, hora, usuario));
                }
                else
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => yupanaServicio.EjecutarSimulacionManualXFechaYHora(fecha, hora, usuario));
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

        [HttpPost]
        public JsonResult SimularEjecucionAutomatica(string fecha, int hora)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Validación
                yupanaServicio.ValidarPrevioSimulacion(ConstantesYupanaContinuo.SimulacionAutomatica, fecha, hora, out DateTime fechaHora, out int topcodi);

                if (UsarCliente)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => cliente.EjecutarYupanaContinuoAutomatico(fecha, hora));
                }
                else
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => yupanaServicio.EjecutarSimulacionAutomaticaXFechaYHora(fecha, hora));
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
        /// Guarda la informacion de los insumos usados en la simulación del escenario elegido
        /// </summary>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public JsonResult FinalizarEjecucionGams()
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (UsarCliente)
                {
                    cliente.TerminarEjecucionGams();
                }
                else
                {
                    yupanaServicio.FinalizarEjecucionArbol();
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
    }
}