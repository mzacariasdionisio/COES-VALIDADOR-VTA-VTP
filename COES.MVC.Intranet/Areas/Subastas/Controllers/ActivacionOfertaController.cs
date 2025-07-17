using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class ActivacionOfertaController : BaseController
    {
        readonly SubastasAppServicio subastasServicio = new SubastasAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ActivacionOfertaController));
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



        #endregion

        #region Pantalla principal

        /// <summary>
        /// Ventana principal
        /// </summary>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ActivacionOfertaModel model = new ActivacionOfertaModel();

            DateTime maniana = DateTime.Now.AddDays(1);
            model.DiaSiguiente = (maniana).ToString(ConstantesAppServicio.FormatoFecha);
            model.ParamPotenciaUrsMinAuto = subastasServicio.ObtenerParametroPotenciaUrsMinAuto(maniana).Formuladat;

            return View(model);
        }


        #endregion

        #region Activacion Oferta por defecto

        /// <summary>
        /// Guarda los datos de la activacion de oferta por defecto
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="datosAGuardar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarActivacionOferta(string fechaOferta, DatoActivacionOferta datosAGuardar)
        {
            ActivacionOfertaModel model = new ActivacionOfertaModel();

            try
            {
                int accion = 0;
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                UserDTO userlogin = (UserDTO)Session[DatosSesion.SesionUsuario];
                int usercode = userlogin.UserCode;
                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaManania = DateTime.Now.AddDays(1).Date;
                var valor = fechaDeOferta.CompareTo(fechaManania);
                if (valor == 0)
                {
                    accion = subastasServicio.GuardarActivacionOferta(fechaDeOferta, datosAGuardar, base.UserName, usercode, out string strLstURSSinActivacion);
                    model.Resultado = accion.ToString();
                    if (strLstURSSinActivacion != "")
                    {
                        model.Resultado = strLstURSSinActivacion;
                    }
                }
                else
                {
                    throw new ArgumentException("Solo esta permitido realizar activaciones para el dia de mañana.");
                }

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Carga los datos de la activacion
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosActivacion(string fechaOferta)
        {
            ActivacionOfertaModel model = new ActivacionOfertaModel();

            try
            {                
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                DateTime hoy = DateTime.Now.Date;

                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                DatoActivacionOferta data = subastasServicio.ObtenerDatosActivacionOferta(fechaDeOferta, null);
                model.DataActivacionOferta = data;
                model.HistorialActivaciones = subastasServicio.ObtenerHistorialActivaciones(fechaDeOferta);

                //Obtengo los motivos a enlistar segun fecha de oferta
                List<SmaMaestroMotivoDTO> lstMotivosTotales = new List<SmaMaestroMotivoDTO>();
                int resultadoFecha = DateTime.Compare(hoy.Date, fechaDeOferta.Date);

                if (resultadoFecha < 0) //fechaDeOferta de oferta mañana
                {
                    lstMotivosTotales = subastasServicio.ListarMotivosActivacion(ConstantesSubasta.EstadoActivo);
                    model.ListaMotivosActivacionSubir = lstMotivosTotales;
                    model.ListaMotivosActivacionBajar = lstMotivosTotales;
                }
                else //fechaOferta hoy o anterior
                {

                    lstMotivosTotales = subastasServicio.ListarMotivosActivacion(ConstantesSubasta.Todos.ToString());
                    List<int> lstMotivoSubir = data.IdsMotivosSubir != null ? data.IdsMotivosSubir : new List<int>();
                    List<int> lstMotivoBajar = data.IdsMotivosBajar != null ? data.IdsMotivosBajar : new List<int>();
                    model.ListaMotivosActivacionSubir = lstMotivoSubir.Any() ? lstMotivosTotales.Where(x => lstMotivoSubir.Contains(x.Smammcodi)).ToList() : lstMotivosTotales;
                    model.ListaMotivosActivacionBajar = lstMotivoSubir.Any() ? lstMotivosTotales.Where(x => lstMotivoBajar.Contains(x.Smammcodi)).ToList() : lstMotivosTotales;
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
        /// Carga los datos de la activacion dsede el historial de activaciones
        /// </summary>
        /// <param name="smapaccodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarDatosHistorial(int smapaccodi)
        {
            ActivacionOfertaModel model = new ActivacionOfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DatoActivacionOferta data = subastasServicio.ObtenerDatosActivacionOferta(new DateTime(), smapaccodi);
                model.DataActivacionOferta = data;
                List<int> lstMotivoSubir = data.IdsMotivosSubir;
                List<int> lstMotivoBajar = data.IdsMotivosBajar;

                List<SmaMaestroMotivoDTO> lstMotivosTotales = subastasServicio.ListarMotivosActivacion(ConstantesSubasta.Todos.ToString());

                model.ListaMotivosActivacionSubir = lstMotivosTotales.Where(x => lstMotivoSubir.Contains(x.Smammcodi)).ToList();
                model.ListaMotivosActivacionBajar = lstMotivosTotales.Where(x => lstMotivoBajar.Contains(x.Smammcodi)).ToList();

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

        #region Indisponibilidad temporal URS
        /// <summary>
        /// Guarda informacion de indisponibilidad temporal urs
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="datosAGuardar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarIndisponibilidadTemporaUrs(string fechaOferta, List<SmaIndisponibilidadTempDetDTO> datosAGuardar)
        {
            ActivacionOfertaModel model = new ActivacionOfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaManania = DateTime.Now.AddDays(1).Date;
                var valor = fechaDeOferta.CompareTo(fechaManania);
                if (valor == 0)
                {
                    subastasServicio.GuardarIndisponibilidadTemporaUrs(fechaDeOferta, datosAGuardar, base.UserName);
                }
                else
                {
                    throw new ArgumentException("Solo esta permitido guardar información de indisponibilidad temporal de URS para el dia de mañana.");
                }

                model.IndisponibilidadCab = subastasServicio.ObtenerIndisponibilidadCabPorFecha(fechaDeOferta);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Carga datos de indisponibilidad temporal para cierta fecha
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <returns></returns>
        public JsonResult CargarDatosIndisponibilidad(string fechaOferta)
        {
            ActivacionOfertaModel model = new ActivacionOfertaModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                DateTime hoy = DateTime.Now.Date;
                DateTime maniana = hoy.AddDays(1).Date;

                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.ParamPotenciaUrsMinAuto = subastasServicio.ObtenerParametroPotenciaUrsMinAuto(fechaDeOferta).Formuladat;

                List<SmaIndisponibilidadTempDetDTO> lstIndisponibilidadT = subastasServicio.ObtenerDatosIndisponibilidadTemporal(fechaDeOferta, out bool mostrarEnWeb);

                model.IndisponibilidadCab = subastasServicio.ObtenerIndisponibilidadCabPorFecha(fechaDeOferta);
                model.MostrarTablaIndisponibilidad = mostrarEnWeb;
                model.ListaIndisponibilidadTemporal = lstIndisponibilidadT;


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