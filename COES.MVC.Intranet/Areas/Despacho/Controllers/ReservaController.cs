using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class ReservaController : BaseController
    {
        readonly MigracionesAppServicio servicio = new MigracionesAppServicio();

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


        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region RSF

        /// <summary>
        /// Pantalla principal
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRSF()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ReservaModel model = new ReservaModel();
            model.FechaConsulta = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        public JsonResult ListarRSF(string fechaConsulta)
        {
            ReservaModel model = new ReservaModel();
            try
            {
                DateTime fechConsulta = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaReserva = servicio.ListarReservaSecundaria(fechConsulta);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarReserva(string fechaVigencia, int opcion, Datos48Reserva datosAGuardar)
        {
            ReservaModel model = new ReservaModel();

            try
            {
                int accion = 0;
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string usuario = base.UserName;

                accion = servicio.GuardarReserva(fechaVig, datosAGuardar, usuario, opcion);

                model.Resultado = accion.ToString();
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDatosReserva(string fechaVigencia, string fechaConsulta)
        {
            ReservaModel model = new ReservaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);
                DateTime hoy = DateTime.Now.Date;

                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.Datos48Reserva = servicio.ObtenerDatosRSF(fechaVig, fecha);
                model.Entidad = model.Datos48Reserva.DatosSubir;// obtengo valores de RSF

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult EliminarRSF(string fechaVigencia)
        {
            ReservaModel model = new ReservaModel();
            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                string usuario = base.UserName;
                DateTime fecha = DateTime.Now;

                servicio.ActualizarEstadoBaja(fechaVig, ConstantesMigraciones.ReservaSecundaria, usuario);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
       
        #endregion

        #region RPF

        /// <summary>
        /// Pantalla principal
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRPF()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ReservaModel model = new ReservaModel();
            model.FechaConsulta = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        public JsonResult ListarRPF(string fechaConsulta)
        {
            ReservaModel model = new ReservaModel();
            try
            {
                DateTime fechConsulta = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaReserva = servicio.ListarReservaPrimaria(fechConsulta);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarRPF(string fechaVigencia, int opcion, PrReservaDTO dataGuardar)
        {
            ReservaModel model = new ReservaModel();

            try
            {
                int accion = 0;
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string usuario = base.UserName;

                accion = servicio.GuardarReservaPrimaria(fechaVig, dataGuardar, usuario, opcion);

                model.Resultado = accion.ToString();
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarDatosRPF(string fechaVigencia, string fechaConsulta)
        {
            ReservaModel model = new ReservaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);
                DateTime hoy = DateTime.Now.Date;

                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.Entidad = servicio.ObtenerDatosRPF(fechaVig, fecha);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult EliminarRPF(string fechaVigencia)
        {
            ReservaModel model = new ReservaModel();
            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaVig = DateTime.ParseExact(fechaVigencia, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                string usuario = base.UserName;
                DateTime fecha = DateTime.Now;

                servicio.ActualizarEstadoBaja(fechaVig, ConstantesMigraciones.ReservaPrimaria, usuario);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
        
        #endregion

    }
}