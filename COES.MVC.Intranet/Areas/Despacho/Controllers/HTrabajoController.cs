using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class HTrabajoController : BaseController
    {
        private readonly HTrabajoAppServicio htrabajoServicio = new HTrabajoAppServicio();

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

        public HTrabajoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            HTrabajoModel model = new HTrabajoModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        #region Configuración de centrales

        /// <summary>
        /// listado de configuración
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoConfiguracionRER()
        {
            HTrabajoModel model = new HTrabajoModel();

            try
            {
                model.ListaConfiguracionRER = htrabajoServicio.ListarConfiguracionCentralesRER();
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
        /// obtener entidad
        /// </summary>
        /// <param name="htcentcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerHtCentralCfg(int htcentcodi)
        {
            HTrabajoModel model = new HTrabajoModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaCentrales = htrabajoServicio.ListarCentralesConfigReqHT();
                model.Entidad = new HtCentralCfgDTO();

                if (htcentcodi == 0)
                {
                    model.AccionNuevo = true;
                }
                else
                {
                    model.Entidad = htrabajoServicio.GetByIdHtCentralCfg(htcentcodi);
                    model.ListaConfiguracion = htrabajoServicio.GetByCriteriaHtCentralCfgdets(htcentcodi);
                    model.AccionEditar = true;
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
        /// Permite obtener punto o canal según tipo
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerElemento(int codigo, int tipo)
        {
            HTrabajoModel model = new HTrabajoModel();

            try
            {
                if (tipo == ConstantesDespacho.TipoHtrabajo)
                {
                    var ptomedicon = htrabajoServicio.GetByIdEquipo(codigo);
                    if (ptomedicon != null)
                    {
                        model.Elemento = new ItemElemento();
                        model.Elemento.Codigo = ptomedicon.Ptomedicodi;
                        model.Elemento.Descripcion = ptomedicon.Ptomedielenomb;
                    }
                    else
                    {
                        throw new ArgumentException("Error: EL código no existe.");
                    }
                }
                else
                {
                    var canal = htrabajoServicio.GetByIdCanal(codigo);
                    if (canal != null)
                    {
                        model.Elemento = new ItemElemento();
                        model.Elemento.Codigo = canal.Canalcodi;
                        model.Elemento.Descripcion = canal.Canalnomb;
                    }
                    else
                    {
                        throw new ArgumentException("Error: EL código no existe.");
                    }
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
        /// Permite guardar y editar una configuración
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="idCentralCfg"></param>
        /// <param name="tipo"></param>
        /// <param name="strConf"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarConfiguracion(int equicodi, int idCentralCfg, int tipo, string strConf)
        {
            HTrabajoModel model = new HTrabajoModel();

            try
            {
                base.ValidarSesionJsonResult();

                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<HtCentralCfgdetDTO> listaConf = !string.IsNullOrEmpty(strConf) ? serializer.Deserialize<List<HtCentralCfgdetDTO>>(strConf) : new List<HtCentralCfgdetDTO>();

                htrabajoServicio.GuardarConfiguracion(equicodi, idCentralCfg, tipo, listaConf, base.UserName);

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
        /// Permite dar de baja la configuración y sus detalles
        /// </summary>
        /// <param name="htcentcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarConfiguracion(int htcentcodi)
        {
            HTrabajoModel model = new HTrabajoModel();

            try
            {
                base.ValidarSesionJsonResult();

                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                htrabajoServicio.EliminarConfiguracion(htcentcodi, base.UserName);

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

        public ActionResult IndexEjecucion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            HTrabajoModel model = new HTrabajoModel();
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFechaWS);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        #region Carga de archivos

        [HttpPost]
        public async Task<JsonResult> EjecucionManual(string fecha, int h)
        {
            HTrabajoModel model = new HTrabajoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int resultado = await CallWebServiceHtrabajo(fecha, h);

                model.Resultado = resultado.ToString();
                if (resultado == 0) model.Mensaje = "No se realizó la carga de datos.";
                if (resultado == -1) model.Mensaje = "No se pudo conectar al servidor COES.";
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

        private async Task<int> CallWebServiceHtrabajo(string fecha, int hMediaHora)
        {
            //obtener hora a procesar
            var strFecha = fecha.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
            DateTime fechaHora = new DateTime(strFecha[2], strFecha[1], strFecha[0], 0, 0, 0);
            int h = Convert.ToInt32(hMediaHora);
            DateTime fechaMediaHora = fechaHora.AddMinutes(h * 30);

            try
            {
                string logEjecucion = string.Format("[Carga de archivos RER] El usuario {0} ha iniciado la opción Ejecución manual para el periodo {1}."
                                            , base.UserName, fechaMediaHora.ToString(ConstantesAppServicio.FormatoFechaFull));
                Log.Info(logEjecucion);

                string baseUrl = ConfigurationManager.AppSettings[ConstantesDespacho.KeyHtrabajoRERUrlWS];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage resProceso = await client.GetAsync(string.Format("HtrabajoServicio.svc/SubirArchivoFTPPronosticoRER/{0}/{1}", fecha, hMediaHora));

                    if (resProceso.IsSuccessStatusCode)
                    {
                        var response = resProceso.Content.ReadAsStringAsync().Result;
                        return response.Contains("1") ? 1 : 0;
                    }
                    else
                    {
                        Log.Error("[Carga de archivos RER] Error al conectarse al Servidor web COES. " + "\n" + resProceso.ReasonPhrase + "\n");
                        htrabajoServicio.EnviarNotificacionEnvio(5, null, fechaMediaHora);
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("CallWebServiceHtrabajo", ex);
                htrabajoServicio.EnviarNotificacionEnvio(5, null, fechaMediaHora);
                return -1;
            }
        }

        #endregion

    }
}