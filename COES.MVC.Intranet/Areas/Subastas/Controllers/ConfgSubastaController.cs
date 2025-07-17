using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class ConfgSubastaController : BaseController
    {
        readonly SubastasAppServicio servicio = new SubastasAppServicio();

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
        public ConfgSubastaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la Ventana Principal
        /// </summary>
        public ActionResult Default()
        {
            ProcesoModel model = new ProcesoModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);

                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                //historico de cambios
                model.ListProceso = servicio.ListSmaParamProcesos();

                //vigente
                SmaParamProcesoDTO reg = servicio.GetParamValidoEnvioyProcesoAutomatico();

                ViewData["inicio"] = reg.Papohorainicio;
                ViewData["fin"] = reg.Papohorafin;
                ViewData["ncp"] = reg.Papohoraenvioncp;
                ViewData["maxdias"] = reg.Papomaxdiasoferta;

                //mes defecto
                model.FechaMesSig = DateTime.Today.AddMonths(1).ToString(ConstantesAppServicio.FormatoMes);
                model.FechaMesActual = DateTime.Today.ToString(ConstantesAppServicio.FormatoMes);
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

        #region Horarios Subasta

        /// <summary>
        /// Registra Prametros de Proceso
        /// </summary>
        public JsonResult MantenimientoParametro(FormCollection collection)
        {
            ProcesoModel model = new ProcesoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string accion = Convert.ToString(collection["accion"]);

                SmaParamProcesoDTO dto = new SmaParamProcesoDTO();
                dto.Papohorainicio = Convert.ToString(collection["inicio"]);
                dto.Papohorafin = Convert.ToString(collection["fin"]);
                dto.Papohoraenvioncp = Convert.ToString(collection["envioncp"]);
                dto.Papomaxdiasoferta = Convert.ToInt32(collection["maxdias"]);

                dto.Papocodi = 0;
                dto.Papousucreacion = User.Identity.Name;

                Log.Info("Registrando Parametro - SaveSmaParamProceso");

                this.servicio.RegistrarHoraMaxCargaDatosyProcesoAutomatico(dto);
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

        #region Ampliación de plazo Oferta por Defecto

        [HttpPost]
        public JsonResult ListadoAmpliacion()
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                this.ValidarSesionJsonResult();

                model.ListaAmpliacion = servicio.ListarAmpliacionPlazoOfDefecto();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ObtenerValorDefectoAmpliacion(string mesProceso)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                this.ValidarSesionJsonResult();

                var objFechaMes = DateTime.ParseExact(mesProceso, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                var dia1MesActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                servicio.ObtenerPlazoValidoCargaExtranet(ConstantesSubasta.OfertipoDefecto, objFechaMes, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo, out int maxDiaDiaria);
                model.PlazoDefectoDia = fechaHoraFinPlazo.ToString(ConstantesAppServicio.FormatoFecha);
                model.PlazoDefectoHora = fechaHoraFinPlazo.ToString(ConstantesAppServicio.FormatoHora);

                if (objFechaMes == dia1MesActual)
                {
                    model.NuevoPlazoDefectoDia = fechaHoraFinPlazo.ToString(ConstantesAppServicio.FormatoFecha);
                }
                else
                {
                    model.NuevoPlazoDefectoDia = fechaHoraFinPlazo.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
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

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GuardarAmpliacion(int smaapcodi, string mesProceso, string fechaFinPlazo, string estado)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                this.ValidarSesionJsonResult();

                var objFechaMes = DateTime.ParseExact(mesProceso, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                var objFechaDia = DateTime.ParseExact(fechaFinPlazo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture); //dd/mm/yyyy hh:mm

                SmaAmpliacionPlazoDTO obj = new SmaAmpliacionPlazoDTO()
                {
                    Smaapcodi = smaapcodi,
                    Smaapaniomes = objFechaMes,
                    Smaapnuevoplazo = objFechaDia,
                    Smaapestado = estado,
                };

                servicio.GuardarAmpliacionPlazo(obj, base.UserName);

                model.Resultado = "1";
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

        #region Desencriptar Ofertas

        /// <summary>
        /// Muestra la Ventana Principal
        /// </summary>
        public ActionResult IndexProcesarOferta()
        {
            ProcesoModel model = new ProcesoModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajeOpcionNoPermitido);

                model.Fecha = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);

                #region
                model.Fecha2 = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
                string fechaDespliegue = ConfigurationManager.AppSettings[ConstantesSubasta.KeyFechaDespliegue].ToString();
                model.TieneOpcionEspecialHabilitado = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha) == fechaDespliegue;
                #endregion
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
        /// Método para desencriptar ofertas segun el dia y el tipo de oferta
        /// </summary>
        /// <param name="strFechaOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarOferta(string strFechaOferta, int tipoOferta)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaOferta = DateTime.ParseExact(strFechaOferta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.ProcesarDesencriptacionXFechaYTipoOferta(fechaOferta, tipoOferta, User.Identity.Name);

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
        /// Método para desencriptar ofertas segun el dia y el tipo de oferta
        /// </summary>
        /// <param name="strFechaOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarOfertaInicial(string strMaxFechaOferta)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string fechaDespliegue = ConfigurationManager.AppSettings[ConstantesSubasta.KeyFechaDespliegue].ToString();
                bool tieneOpcionEspecialHabilitado = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha) == fechaDespliegue;

                if (!tieneOpcionEspecialHabilitado)
                    throw new Exception("La opción ya no está disponible.");

                DateTime fechaMaxOferta = DateTime.ParseExact(strMaxFechaOferta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.ProcesarDesencriptacionEspecialInicial(fechaMaxOferta);

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
        /// Muestra la Ventana Principal
        /// </summary>
        public ActionResult OfertaSimetrica()
        {
            SmaOfertaSimetricaHorarioDTO model = new SmaOfertaSimetricaHorarioDTO();

            try
            {
                Boolean isPost = string.IsNullOrEmpty(Request.Params["btnRegistrar"]);

                if (isPost == false)
                {
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime HorarioInicio = DateTime.ParseExact(Request.Params["FechaConsultaIni"], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime HorarioFin = DateTime.ParseExact(Request.Params["FechaConsultaFin"], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    servicio.CrearSmaOfertaSimetricaHorario(Request.Params["FechaConsultaIni"], Request.Params["FechaConsultaFin"]);
                }

                Boolean isGetRevertirEstado = string.IsNullOrEmpty(Request.Params["idRevertirEstado"]);

                if (isGetRevertirEstado == false)
                {
                    int estado = Request.Params["estado"] == "0" ? 1 : 0;
                    servicio.RevertirEstadoSmaOfertaSimetricaHorario(Request.Params["idRevertirEstado"], estado);
                }


                model.HorarioInicio = DateTime.Now;
                model.HorarioFin = DateTime.Now;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                throw new ArgumentException("___" + Request.Params["FechaConsultaIni"] + "___" + Request.Params["FechaConsultaFin"] + "_____<hr>" + ex.Message);
            }

            ///List<SmaOfertaSimetricaHorarioDTO> registros = servicio.ListSmaOfertaSimetricaHorario();
            ViewBag.registros = servicio.ListSmaOfertaSimetricaHorario();
            return View(model);
        }


        #endregion

    }
}