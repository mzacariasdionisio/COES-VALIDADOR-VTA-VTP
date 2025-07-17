using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class LimitePotElectController : BaseController
    {
        private readonly INDAppServicio indServicio;
        public LimitePotElectController()
        {
            indServicio = new INDAppServicio();
            log4net.Config.XmlConfigurator.Configure();
        }

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

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Vista principal Límite de Potencia Eléctrica
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = indServicio.GetPeriodoActual();
                model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = indServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);
                model.FechaIni = regPeriodo.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = regPeriodo.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                indServicio.ListarFiltroUnidad(ConstantesHorasOperacion.IdTipoTermica, regPeriodo.FechaIni, regPeriodo.FechaFin, -1, -1
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad);
                model.ListaEmpresa = listaEmpresa;
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
        /// Listar periodos por año
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(anio);
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
        /// Listar Limite de potencia el. por periodo
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LimPotListado(int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                IndPeriodoDTO regPeriodo = indServicio.GetByIdIndPeriodo(pericodi);
                DateTime fechaIni = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                model.FechaIni = fechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                string url = Url.Content("~/");
                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.Resultado = indServicio.GenerarHtmlListadoLimPotencia(fechaIni, fechaFin, model.TienePermisoEditar, url);
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
        /// Registrar Limite de potencia electrica
        /// </summary>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <param name="nombre"></param>
        /// <param name="strJsonData"></param>
        /// <param name="mw"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LimPotGuardar(string strFechaIni, string strFechaFin, string nombre, string strJsonData, decimal? mw)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<IndPotlimUnidadDTO> llimUnidades = !string.IsNullOrEmpty(strJsonData) ? serialize.Deserialize<List<IndPotlimUnidadDTO>>(strJsonData) : new List<IndPotlimUnidadDTO>();

                //Validaciones
                if (string.IsNullOrEmpty(nombre?.Trim())) throw new ArgumentException("Debe ingresar nombre de sistema de transmisión eléctrica.");
                if (mw.GetValueOrDefault(0) <= 0) throw new ArgumentException("Debe ingresar Capacidad(MW) mayor que cero.");
                if (!llimUnidades.Any()) throw new ArgumentException("Debe seleccionar una o más unidades de generación.");

                decimal valorSumUnid = llimUnidades.Sum(x => x.Equlimpotefectiva ?? 0);
                if (valorSumUnid <= mw) throw new ArgumentException("La Capacidad(MW) debe ser menor que la suma de las potencias efectivas de las unidades de generación.");

                var fechaini = DateTime.ParseExact(strFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                var fechafin = DateTime.ParseExact(strFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaini.Month != fechafin.Month) throw new ArgumentException("La fecha de inicio y fin deben comprenderse en el mismo mes");

                //Guardar


                var indPotlim = new IndPotlimDTO()
                {
                    Potlimfechaini = fechaini,
                    Potlimfechafin = fechafin,
                    Potlimnombre = nombre,
                    Potlimmw = mw,
                    IndPotlimUnidades = llimUnidades,
                    Potlimusucreacion = base.UserName,
                    Potlimfeccreacion = DateTime.Now
                };

                indServicio.LimPotGuardar(indPotlim);

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
        /// Actualizar Limite de potencia electrica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mw"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LimPotEditar(int id, decimal mw)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                var indPotlim = new IndPotlimDTO()
                {
                    Potlimcodi = id,
                    Potlimmw = mw,
                    Potlimusumodificacion = base.UserName,
                    Potlimfecmodificacion = DateTime.Now
                };

                indServicio.LimPotEditar(indPotlim);

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
        /// Dar de baja Limite de potencia por códido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LimPotDarBaja(int id)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                indServicio.LimPotDarBaja(id);

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
        /// Listar central Op Comercial por empresa y periodo
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltros(int emprcodi, int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                IndPeriodoDTO regPeriodo = indServicio.GetByIdIndPeriodo(pericodi);

                indServicio.ListarFiltroUnidad(ConstantesHorasOperacion.IdTipoTermica, regPeriodo.FechaIni, regPeriodo.FechaFin, emprcodi, -1
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad);

                model.ListaCentral = listaCentral;

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
        /// Cargar Lista modo de operación por periodo,empresa y central
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equipadre"></param>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarModo(int emprcodi, int equipadre, int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                IndPeriodoDTO regPeriodo = indServicio.GetByIdIndPeriodo(pericodi);

                indServicio.ListarFiltroUnidad(ConstantesHorasOperacion.IdTipoTermica, regPeriodo.FechaIni, regPeriodo.FechaFin, emprcodi, equipadre
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad);

                List<ListaSelect> listaModo = listaUnidad
                                            .Select(x => new ListaSelect() { codigo = (x.Grupocodi) + "," + x.Pe + "," + x.Equicodi, text = x.UnidadnombPR25 })
                                            .ToList();

                model.ListaModoOperacion = listaModo;

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