using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class HorasOperacionController : BaseController
    {
        readonly HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        readonly IEODAppServicio servicio = new IEODAppServicio();
        readonly EventoAppServicio servEvento = new EventoAppServicio();
        readonly EventosAppServicio servicioEvento = new EventosAppServicio();

        #region Declaración de variables

        private readonly int IdOpcionIndexCoordinador = 1022;

        readonly SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        /// <summary>
        /// Eventos/Horas de Operación/
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCoordinador()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            HOReporteModel model = new HOReporteModel();
            this.CargarFiltrosBusqueda(model, ConstantesHorasOperacion.IdTipoTermica.ToString());

            model.IdTipoCentral = ConstantesHorasOperacion.IdTipoTermica;
            model.IdEmpresa = Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos);
            model.IdCentralSelect = Int32.Parse(ConstantesHorasOperacion.ParamCentralSeleccione);
            model.IdEquipo = Int32.Parse(ConstantesHorasOperacion.ParamModoSeleccione);
            model.ListaTipoDesglose = HorasOperacionAppServicio.ListarTipoDesglose();
            model.HoraFinDefecto = ConstantesHorasOperacion.HoraFinDefecto;
            model.ValorUmbral = (new ParametroAppServicio()).ObtenerValorVigente(ConstantesHorasOperacion.IdParametroUmbral);

            model.TienePermisoAdministrador = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador);

            return View(model);
        }

        #region Pestaña "Listado" y "Gráfico"

        /// <summary>
        /// Generación de Listado
        /// </summary>
        /// <param name="sfecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaReporteHopEms(string sfecha, int idEmpresa, int? idEnvio, string sFecha2, int flagHoraTR, string horaCI)
        {
            HOReporteModel jsModel = new HOReporteModel();

            bool consultarOtros = true;

            try
            {
                base.ValidarSesionJsonResult();
                jsModel.TienePermisoAdministrador = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador);

                horaCI = ConstantesHorasOperacion.FlagFiltroTR == flagHoraTR ? null : horaCI;

                string horaMin = !string.IsNullOrEmpty(horaCI) ? horaCI : DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora);
                DateTime fechaCI = DateTime.ParseExact(sfecha + " " + horaMin, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaIni = fecha;
                DateTime fechaFin = fechaIni.AddDays(1);

                #region Valores default para los Formularios de registro / edición

                jsModel.HoraMinutoActual = DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora);
                jsModel.HoraMinutoConsulta = fechaCI.ToString(ConstantesAppServicio.FormatoOnlyHora);

                jsModel.Fecha = sfecha;
                jsModel.FechaAnterior = fecha.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
                jsModel.FechaSiguiente = fecha.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);

                jsModel.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();
                jsModel.ListaMotOpForzada = HorasOperacionAppServicio.ListarMotivoOperacionForzada();
                jsModel.ListaTipoDesglose = HorasOperacionAppServicio.ListarTipoDesglose();

                jsModel.IdTipoCentral = ConstantesHorasOperacion.IdTipoTermica;
                jsModel.IdEmpresa = Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos);
                jsModel.IdCentralSelect = Int32.Parse(ConstantesHorasOperacion.ParamCentralSeleccione);
                jsModel.IdEquipo = Int32.Parse(ConstantesHorasOperacion.ParamModoSeleccione);

                jsModel.ListaFechaArranque = new List<string>
                {
                    jsModel.Fecha,
                    fecha.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha)
                };
                if ((jsModel.Fechahorordarranq != null) && (!jsModel.ListaFechaArranque.Contains(jsModel.Fechahorordarranq)))
                    jsModel.ListaFechaArranque.Add(jsModel.Fechahorordarranq);
                if (jsModel.Hophorordarranq == "") jsModel.Hophorordarranq = null;
                if (jsModel.Hophorparada == "") jsModel.Hophorparada = null;

                #endregion

                //data maestra
                List<PrGrupoDTO> listaModosOperacion = this.servHO.ListarModoOperacionXCentralYEmpresa(-2, idEmpresa);
                this.servHO.AsignarDatosModoOperacion(listaModosOperacion, fechaCI.Date);

                List<EqEquipoDTO> listaUnidades = new List<EqEquipoDTO>();
                foreach (var itemModo in listaModosOperacion)
                {
                    for (int i = 0; i < itemModo.ListaEquicodi.Count; i++)
                    {
                        listaUnidades.Add(new EqEquipoDTO() { Equicodi = itemModo.ListaEquicodi[i], Equiabrev = itemModo.ListaEquiabrev[i] });
                    }
                }

                jsModel.ListaUnidades = listaUnidades.GroupBy(x => x.Equicodi).Select(x => new EqEquipoDTO() { Equicodi = x.Key, Equiabrev = x.First().Equiabrev }).ToList();
                jsModel.ListaUnidXModoOP = this.servHO.ListarUnidadesWithModoOperacionXCentralYEmpresa(-2, idEmpresa.ToString());
                jsModel.ListaEmpresas = this.servHO.ListarEmpresasHorasOperacionByTipoCentral(true, null, ConstantesHorasOperacion.IdTipoTermica.ToString());

                jsModel.ListaCentrales = this.servHO.ListarCentralesXEmpresaGener(idEmpresa)
                                    .Where(x => x.Famcodipadre == ConstantesHorasOperacion.IdTipoTermica)
                                    .GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                                    .Select(y => new EqEquipoDTO() { Equicodi = y.Key.Codipadre, Equinomb = y.Key.Nombrecentral, Emprcodi = y.First().Emprcodi, Emprnomb = y.First().Emprnomb })
                                    .OrderBy(x => x.Equinomb)
                                    .ToList();

                List<ReporteCostoIncrementalDTO> listaReporteCostosIncr = (new GrupoDespachoAppServicio()).ListarTodosCI(new List<int>(), fecha);

                //validaciones con otros aplicativos
                this.servHO.ListarReporteHopEms(fecha, idEmpresa.ToString(), fechaCI, idEnvio, consultarOtros, out List<EveHoraoperacionDTO> listaHOHoyValidacion
                                            , out List<ResultadoValidacionAplicativo> listaValEmsUniNoReg, out List<int> listaHopcodiValInterFS
                                            , out List<MeMedicion48DTO> listaEms
                                            , out List<ResultadoValidacionAplicativo> listaValScadaUniNoReg);

                //Horas de operación
                List<EveHoraoperacionDTO> listaHorasOperacion = this.servHO.ListarHorasOperacionMejoras2023(fechaIni, fechaFin,
                                                            idEmpresa.ToString(), ConstantesHorasOperacion.ParamCentralTodos);
                listaHorasOperacion = this.servHO.ListarHopByEnvio(listaHorasOperacion, idEnvio); //historial de envio seleccionado

                List<EveHoraoperacionDTO> listaHorasOperacionAnt = this.servHO.ListarHorasOperacionMejoras2023(fechaIni.AddDays(-1), fechaFin.AddDays(-1),
                                                            idEmpresa.ToString(), ConstantesHorasOperacion.ParamCentralTodos);

                List<EveHoEquiporelDTO> listaDesgloseRango = this.servHO.GetByCriteriaEveHoEquiporelGroupByHoPadre(fecha, fecha);

                servHO.SetearPuedeOffPuedeOnXModo(fechaCI, listaModosOperacion, listaHorasOperacion, listaHorasOperacionAnt);
                servHO.AsignarDatosAHoraOperacion(listaHorasOperacion, listaModosOperacion, listaDesgloseRango, listaHOHoyValidacion);

                //Gráfico: Ordenamiento de los modos de operación por costo incremental
                List<PrGrupoDTO> listaModosOperacionCI = this.servHO.OrdenarListXCostoIncremental(fechaCI, listaModosOperacion, listaHorasOperacion, listaReporteCostosIncr);

                //Listado y Gráfico: Alertas Programado y pruebas aleatorias                
                ObtenerHorasProgramadas(fechaCI, listaHorasOperacion, listaModosOperacion, listaModosOperacionCI, out List<HOPAlerta> listaAlerta,
                                                out List<HorasProgramadasDTO> listaHorasProgramadas, out List<PrGrupoDTO> listaModoProgramada);

                //Listado y Gráfico: Unidades en operación no registradas                
                jsModel.ListaHOPUnidadesNoRegistradasEMS = listaValEmsUniNoReg;
                jsModel.ListaHOPUnidadesNoRegistradasScada = listaValScadaUniNoReg;
                jsModel.ListaUnidadesNoRegistradasEMS = this.servHO.ListarUnidadesNoRegistradas(listaValEmsUniNoReg);
                jsModel.ListaUnidadesNoRegistradasScada = this.servHO.ListarUnidadesNoRegistradas(listaValScadaUniNoReg);

                //Lista de envios
                int fdatcodi = ConstantesIEOD.FdatcodiPadreHOP;
                jsModel.ListaEnvios = servicio.GetByCriteriaMeEnvios(ConstantesHorasOperacion.IdEmpresaTodos, 0, fecha).Where(x => x.Fdatcodi == fdatcodi).ToList();
                if (idEnvio > 0) jsModel.ListaEnviodet = servicio.GetByCriteriaMeEnviodets(idEnvio.Value);

                //Pestaña Costo CT
                List<PrGrupoDTO> listaModosOperacionCT = this.servHO.ListarReporteHopCT(listaReporteCostosIncr,
                                                                    listaHorasOperacion, fechaCI, listaModoProgramada, listaModosOperacion);

                //Pestaña Log de Correo
                DateTime fecha2 = DateTime.ParseExact(sFecha2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string resultaLogCorreo = string.Empty;

                //salidas
                jsModel.ListaModosOperacion = listaModosOperacion.OrderBy(x => x.Gruponomb).ToList();

                jsModel.ListaHorasOperacion = listaHorasOperacion;
                jsModel.ListaHorasOperacionAnt = listaHorasOperacionAnt;
                jsModel.ListaHorasProgramadas = listaHorasProgramadas;

                jsModel.ListaModosOperacionCI = listaModosOperacionCI;
                jsModel.ListaModosOperacionCT = listaModosOperacionCT;

                jsModel.ListaModosOperacionProgramada = listaModoProgramada;

                jsModel.TotalValInterFS = listaHopcodiValInterFS.Count();
                jsModel.ListaAlerta = listaAlerta;

                jsModel.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = -1;
                jsModel.Mensaje = ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            var json = Json(jsModel);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        #endregion

        #region Alertas (Horas Programadas, EMS, Scada, Intervenciones y Costos Incrementales)

        /// <summary>
        /// Interfaz para mostrar la alerta ems por Hora de operacion
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaEmsXHOP(string sfecha, int? idEnvio, int idEmpresa, int hopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();

            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaValidacionHorasOperacionEms = this.servHO.ListarAlertaEmsEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), hopcodipadre.ToString(), true);
                model.HoraOperacion = model.ListaValidacionHorasOperacionEms.First().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Interfaz para mostrar la alerta ems por Modo de operacion
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="listaHopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaEmsXModo(string sfecha, int? idEnvio, int idEmpresa, string listaHopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();

            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaHorasOperacion = new List<EveHoraoperacionDTO>();
                model.ListaValidacionHorasOperacionEms = this.servHO.ListarAlertaEmsEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), listaHopcodipadre, true);

                List<string> listacodi = listaHopcodipadre.Split(',').ToList();
                foreach (var reg in listacodi)
                {
                    var hop = model.ListaValidacionHorasOperacionEms.Where(x => x.Hopcodipadre.ToString() == reg).FirstOrDefault();
                    if (hop != null)
                    {
                        model.ListaHorasOperacion.Add(hop.HoraOperacion);
                    }
                }
                model.HoraOperacion = model.ListaValidacionHorasOperacionEms.FirstOrDefault().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Interfaz para mostrar la alerta Scada por Hora de operacion
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaScadaXHOP(string sfecha, int? idEnvio, int idEmpresa, int hopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaValidacionHorasOperacionScada = this.servHO.ListarAlertaScadaEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), hopcodipadre.ToString(), true);
                model.HoraOperacion = model.ListaValidacionHorasOperacionScada.First().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Interfaz para mostrar la alerta ems por Modo de operacion
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="listaHopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaScadaXModo(string sfecha, int? idEnvio, int idEmpresa, string listaHopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaHorasOperacion = new List<EveHoraoperacionDTO>();
                model.ListaValidacionHorasOperacionScada = this.servHO.ListarAlertaScadaEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), listaHopcodipadre, true);

                List<string> listacodi = listaHopcodipadre.Split(',').ToList();
                foreach (var reg in listacodi)
                {
                    var hop = model.ListaValidacionHorasOperacionScada.Where(x => x.Hopcodipadre.ToString() == reg).FirstOrDefault();
                    if (hop != null)
                    {
                        model.ListaHorasOperacion.Add(hop.HoraOperacion);
                    }
                }
                model.HoraOperacion = model.ListaValidacionHorasOperacionScada.FirstOrDefault().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Interfaz para mostrar la alerta intervencion por Hora de operacion
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaIntervencionXHOP(string sfecha, int? idEnvio, int idEmpresa, int hopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaValidacionHorasOperacionIntervencion = this.servHO.ListarAlertaIntervencionEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), hopcodipadre.ToString(), true);
                model.HoraOperacion = model.ListaValidacionHorasOperacionIntervencion.First().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Interfaz para mostrar la alerta intervencion por modo
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public JsonResult ListarAlertaIntervencionXModo(string sfecha, int? idEnvio, int idEmpresa, string listaHopcodipadre)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaHorasOperacion = new List<EveHoraoperacionDTO>();
                model.ListaValidacionHorasOperacionIntervencion = this.servHO.ListarAlertaIntervencionEquipoNoOperaronByListaHo(fecha, idEnvio, idEmpresa.ToString(), listaHopcodipadre, true);

                List<string> listacodi = listaHopcodipadre.Split(',').ToList();
                foreach (var reg in listacodi)
                {
                    var hop = model.ListaValidacionHorasOperacionIntervencion.Where(x => x.Hopcodipadre.ToString() == reg).FirstOrDefault();
                    if (hop != null)
                    {
                        model.ListaHorasOperacion.Add(hop.HoraOperacion);
                    }
                }
                model.HoraOperacion = model.ListaValidacionHorasOperacionIntervencion.First().HoraOperacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return json;
        }

        /// <summary>
        /// Máximo 2 alertas de Horas de Operación
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="flagHoraTR"></param>
        /// <param name="horaCI"></param>
        /// <param name="alertasOcultas"></param>
        /// <returns></returns>
        public JsonResult MostrarAlertas(string sfecha, int flagHoraTR, string horaCI, string alertasOcultas)
        {
            HOReporteModel jsModel = new HOReporteModel();

            try
            {
                horaCI = ConstantesHorasOperacion.FlagFiltroTR == flagHoraTR ? null : horaCI;

                string horaMin = !string.IsNullOrEmpty(horaCI) ? horaCI : DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora);
                DateTime fechaCI = DateTime.ParseExact(sfecha + " " + horaMin, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                List<EveHoraoperacionDTO> ListaHorasOperacion = new List<EveHoraoperacionDTO>();
                List<PrGrupoDTO> ListaModosOperacion = new List<PrGrupoDTO>();
                List<PrGrupoDTO> ListaModosOperacionCI = new List<PrGrupoDTO>();

                if (Session["ListaHorasOperacion"] != null) ListaHorasOperacion = (List<EveHoraoperacionDTO>)Session["ListaHorasOperacion"];
                if (Session["ListaModosOperacion"] != null) ListaModosOperacion = (List<PrGrupoDTO>)Session["ListaModosOperacion"];
                if (Session["ListaModosOperacionCI"] != null) ListaModosOperacionCI = (List<PrGrupoDTO>)Session["ListaModosOperacionCI"];

                List<HOPAlerta> alertas = new List<HOPAlerta>();
                if (ListaHorasOperacion != null)
                {
                    ObtenerHorasProgramadas(fechaCI, ListaHorasOperacion, ListaModosOperacion, ListaModosOperacionCI, out alertas,
                                            out List<HorasProgramadasDTO> listaHorasProgramadas, out List<PrGrupoDTO> listaModoProgramada);

                    var aAlertasOcultas = alertasOcultas.Split(',');

                    alertas = alertas.Where(p => !alertasOcultas.Contains(p.CodAlerta)).ToList();
                }
                jsModel.ListaAlerta = alertas;

                jsModel.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                jsModel.Resultado = -1;
                jsModel.Mensaje = ex.Message;
                jsModel.Detalle = ex.StackTrace;
            }

            return Json(jsModel);
        }

        private void ObtenerHorasProgramadas(DateTime fechaCI, List<EveHoraoperacionDTO> listaHorasOperacion
                                    , List<PrGrupoDTO> listaModosOperacion
                                    , List<PrGrupoDTO> ListaModosOperacionCI
                                    , out List<HOPAlerta> alertas,
                                    out List<HorasProgramadasDTO> listaHorasProgramadas, out List<PrGrupoDTO> listaModoProgramada
                                )
        {
            Session["ListaHorasOperacion"] = listaHorasOperacion;
            Session["ListaModosOperacion"] = listaModosOperacion;
            Session["ListaModosOperacionCI"] = ListaModosOperacionCI;

            McpAppServicio mcpAppServicio = new McpAppServicio();

            //obtener los rangos de bloques amarillos yupana que no se cruzan con los ejecutados de horas de operación
            mcpAppServicio.ObtenerHorasProgramadas(fechaCI, listaHorasOperacion, listaModosOperacion, out listaHorasProgramadas, out listaModoProgramada);

            alertas = servHO.ListarAlertaHoXDia(fechaCI, listaHorasOperacion, listaHorasProgramadas, listaModosOperacion);
        }

        #endregion

        #region Pestaña "Detalle"

        /// <summary>
        /// Guarda registro de envío de horas de operación
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult RegistrarEnvioHorasOperacion(string data, int idEmpresa, string fecha, int flagConfirmarInterv)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            int fdatcodi = ConstantesIEOD.FdatcodiPadreHOP;

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                List<EveHoraoperacionDTO> listaData = serializer.Deserialize<List<EveHoraoperacionDTO>>(data);

                //////// Definicion de Variables ////////////////
                DateTime fechaHO = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaCI = DateTime.Now;
                DateTime fechaEnvio = DateTime.Now;
                string usuarioEnvio = base.UserName;

                bool consultarOtros = true;
                if (ConstantesHorasOperacion.FlagConfirmarValidacion != flagConfirmarInterv)
                {
                    //cuando cambia de calificacion
                    var listaTipoOperacion = this.servHO.ListarTipoOperacionHO();
                    foreach (var item in listaData)
                    {
                        var objTipoOp = listaTipoOperacion.Find(x => x.Subcausacodi == item.Subcausacodi);
                        if (objTipoOp != null) item.Subcausadesc = objTipoOp.Subcausadesc;
                    }

                    //obtener todas las horas de operacion del dia
                    var listaHoSistema = this.servHO.ListarHorasOperacionMejoras2023(fechaHO, fechaHO.AddDays(1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos);
                    if (listaData.Any())
                    {
                        //edicion multiple, solo se envia todas las horas de operación de ese modo
                        List<int> lgrupocodiEdicion = listaData.Select(x => x.Grupocodi ?? 0).ToList();
                        listaHoSistema = listaHoSistema.Where(x => !lgrupocodiEdicion.Contains(x.Grupocodi ?? 0)).ToList();

                        listaHoSistema.AddRange(listaData);
                    }

                    //////////Validar Centrales con costos incrementales mas caros por bajar ///////////////// 
                    this.servHO.VerificarCentralesMasCaras(listaHoSistema, fechaHO, fechaCI, out List<EveHoraoperacionDTO> listaHoOut2, out List<ResultadoValidacionAplicativo> listaValCentralCaras);
                    model.ListaHorasOperacionCostoIncremental = listaHoOut2;
                    model.ListaValidacionHorasOperacionCostoIncremental = listaValCentralCaras;

                    //////////Validar con Intervenciones /////////////////       
                    this.servHO.VerificarIntervencionFS(listaData, fechaHO, consultarOtros, out List<EveHoraoperacionDTO> listaHoOut1, out List<ResultadoValidacionAplicativo> listaValInterv);
                    model.ListaHorasOperacionIntervencion = listaHoOut1;
                    model.ListaValidacionHorasOperacionIntervencion = listaValInterv;

                    // enviar flag si existe validaciones
                    if (listaValInterv.Count() > 0 || listaValCentralCaras.Count() > 0)
                    {
                        model.Resultado = 2;
                        return Json(model);
                    }
                }

                ///////////////Guardar Envio//////////////////////////                
                MeEnvioDTO envio = new MeEnvioDTO
                {
                    Formatcodi = ConstantesHorasOperacion.IdFormato,
                    Fdatcodi = fdatcodi,
                    Archcodi = 0,
                    Emprcodi = idEmpresa,
                    Estenvcodi = ParametrosEnvio.EnvioEnviado,
                    Enviofecha = fechaEnvio,
                    Enviofechaperiodo = fechaHO,
                    Lastdate = fechaEnvio,
                    Lastuser = usuarioEnvio,
                    Userlogin = usuarioEnvio
                };

                int idEnvio = servicio.SaveMeEnvio(envio);

                ///////////////////////////////////////////////////////
                /////////Guardar Horas de Operación////////////////////
                List<int> listCodHop = new List<int>(), listCodHopElim = new List<int>();
                var listaHorasOperacionBDAntes = this.servHO.GetListaHorasOperacionByCriteria(idEmpresa, ConstantesHorasOperacion.ParamTipoOperacionTodos, fechaHO, fechaHO.AddDays(1), 5, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));
                this.servHO.GuardarHorasdeOperacionAdministrador(listaData, idEmpresa, listaHorasOperacionBDAntes, ref listCodHop, ref listCodHopElim, base.UserName, fechaEnvio);
                var listaHorasOperacionBDDespues = this.servHO.GetListaHorasOperacionByCriteria(idEmpresa, ConstantesHorasOperacion.ParamTipoOperacionTodos, fechaHO, fechaHO.AddDays(1), 5, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));

                //*****

                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = -1;
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                servicio.UpdateMeEnvio(envio);

                /////////Grabar Detalle de envios////////////////////
                listCodHop = listCodHop.Select(o => o).Distinct().ToList();
                foreach (var entity in listCodHop)
                {
                    servicio.SaveMeEnviodet(new MeEnviodetDTO
                    {
                        Enviocodi = idEnvio,
                        Envdetfpkcodi = entity,
                        Envdetusucreacion = usuarioEnvio,
                        Envdetfeccreacion = fechaEnvio
                    });
                }
                //////////////////////////////////////////////////////

                /// Enviar notificacion
                this.servHO.EnviarCorreoNotificacionHayCambioHorasOperacion(listCodHopElim, listaHorasOperacionBDDespues, listaHorasOperacionBDAntes, fechaEnvio, fechaHO, idEmpresa, idEnvio);

                /// Enviar notificación Intervenciones
                if (ConstantesHorasOperacion.FlagConfirmarValIntervenciones == flagConfirmarInterv)
                {
                    //actualizamos la lista de horas de operacion con sus unidades con data de BD
                    listaData = this.servHO.ListarHorasOperacionByCriteria(fechaHO, fechaHO.AddDays(1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos.ToString(), ConstantesHorasOperacion.TipoListadoSoloTermico);

                    this.servHO.VerificarIntervencionFS(listaData, fechaHO, consultarOtros, out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValInterv);
                    if (listaValInterv.Count() > 0)
                    {
                        var arrayUsuario = this.servSeguridad.ListarUsuarios();
                        List<UsuarioParametro> listaUsuario = arrayUsuario.Select(x => this.ConvertirUsuarioServicio(x)).ToList();

                        foreach (var reg in listaValInterv)
                        {
                            UsuarioParametro usuario = listaUsuario.Find(x => x.UserLogin == reg.UltimaModificacionUsuarioDesc);
                            reg.UltimaModificacionUsuarioCorreo = usuario != null ? usuario.UserEmail : string.Empty;
                        }

                        this.servHO.EnviarCorreoNotificacionIntervencionesFS(listaHoOut, listaValInterv, usuarioEnvio, fechaEnvio, fechaHO, idEmpresa, idEnvio);
                    }
                }

                //////////////////////////////////////////////////////

                model.IdEnvio = idEnvio;
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// validar cruce
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ValidarRegistrarEnvioHorasOperacion(string dataForm, string dataListado, string fecha)
        {
            HorasOperacionModel model = new HorasOperacionModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaHO = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<EveHoraoperacionDTO> listaHOweb = new List<EveHoraoperacionDTO>();
                if (!string.IsNullOrEmpty(dataForm)) listaHOweb = serializer.Deserialize<List<EveHoraoperacionDTO>>(dataForm);

                List<EveHoraoperacionDTO> listaHOBD = new List<EveHoraoperacionDTO>();
                if (!string.IsNullOrEmpty(dataListado)) listaHOBD = serializer.Deserialize<List<EveHoraoperacionDTO>>(dataListado);

                List<ValidacionHoraOperacion> listaValCruce = servHO.ListarValidacionCruce(listaHOweb, listaHOBD);

                model.Resultado = 1;
                model.ListaValCruce = listaValCruce;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Convertir objetos de Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listaArea"></param>
        /// <returns></returns>
        private UsuarioParametro ConvertirUsuarioServicio(UserDTO user)
        {
            UsuarioParametro u = new UsuarioParametro
            {
                UserCode = user.UserCode,
                UsernName = user.UsernName,
                UserLogin = user.UserLogin,
                AreaCode = user.AreaCode.GetValueOrDefault(0),
                UserEmail = user.UserEmail
            };

            return u;
        }

        #region Listado de Equipos: Lineas de Transmision, Trafo, Trafo 3D y para Bitacora

        /// <summary>
        /// View Busqueda Linea de transmision
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipo(int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            if (filtroFamilia == 0)
            {
                model.ListaEmpresas = this.servicio.ListarEmpresasxTipoEquipos(this.ListarFamiliaByFiltro(0, 0)).ToList();
                model.ListaFamilia = this.servicio.ListarFamilia().Where(x =>
                    x.Famcodi == ConstantesHorasOperacion.FamLinea ||
                    x.Famcodi == ConstantesHorasOperacion.FamTrafo ||
                    x.Famcodi == ConstantesHorasOperacion.FamTrafo3D).ToList();
            }
            else
            {
                model.ListaEmpresas = this.servicio.GetListaCriteria("-1").Where(x => x.Emprcodi != 0 && x.Emprcodi != -1).ToList();
                model.ListaFamilia = this.servicio.ListarFamilia();
            }
            model.FiltroFamilia = filtroFamilia.Value;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoResultado(int idEmpresa, int idFamilia, string filtro, int nroPagina, int? idArea = 0, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            var listaLinea = this.servEvento.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

            foreach (var reg in listaLinea)
            {
                EqEquipoDTO eq = new EqEquipoDTO
                {
                    Emprnomb = reg.EMPRENOMB,
                    Areanomb = reg.AREANOMB,
                    Equicodi = reg.EQUICODI,
                    Equinomb = reg.EQUIABREV,
                    Equiabrev = reg.EQUIABREV,
                    Famabrev = reg.FAMABREV,
                    Emprcodi = reg.EMPRCODI
                };

                listaEquipo.Add(eq);
            }

            model.ListaLineasCongestion = listaEquipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoPaginado(int idEmpresa, int idFamilia, string filtro, int? idArea = 0, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel
            {
                IndicadorPagina = false
            };
            int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.NroPageShow;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoArea(int idEmpresa, int idFamilia, int? filtroFamilia = 0)
        {
            HorasOperacionModel model = new HorasOperacionModel
            {
                ListaArea = this.servEvento.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para obtener las familias permitidas. 
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"> -1: filtrar todas las familias, 0: filtrar solo para lineas de tranmision </param>
        /// <returns></returns>
        private string ListarFamiliaByFiltro(int idFamilia, int? filtroFamilia = 0)
        {
            if (filtroFamilia == 0)
            {
                return idFamilia == 0 ? ConstantesHorasOperacion.FamLinea.ToString() + ConstantesAppServicio.CaracterComa
                    + ConstantesHorasOperacion.FamTrafo.ToString() + ConstantesAppServicio.CaracterComa + ConstantesHorasOperacion.FamTrafo3D.ToString() : idFamilia.ToString();
            }

            return idFamilia.ToString();
        }

        #endregion

        #region Bitacora

        /// <summary>
        /// Muestra la pantalla para el registro de eventos tipo bitacora
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Bitacora(int? id = 0)
        {
            RegistroModel model = new RegistroModel
            {
                ListaTipoEvento = this.servicioEvento.ListarTipoEvento(),
                ListaEmpresa = this.servicioEvento.ListarEmpresas(),
                ListaTipoOperacion = this.servicioEvento.ObtenerCausaEvento(ConstantesHorasOperacion.IdTipoEventoBitacora)
            };

            if (id == 0)
            {
                model.IdTipoEvento = ConstantesHorasOperacion.IdTipoEventoBitacora;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(ConstantesHorasOperacion.IdTipoEventoBitacora);
                model.Entidad = new EveEventoDTO();
                model.HoraInicial = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
                model.HoraFinal = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
            }
            else if (id != 0)
            {
                model.Entidad = this.servicioEvento.GetByIdEveEvento((int)id);

                if (model.Entidad.Equicodi != null)
                {
                    EqEquipoDTO equipo = (new EquipamientoAppServicio()).ObtenerDetalleEquipo((int)model.Entidad.Equicodi);
                    model.Entidad.Equiabrev = equipo.TAREAABREV + " " + equipo.AREANOMB + " - " + equipo.Equiabrev;
                }

                model.IdTipoEvento = (int)model.Entidad.Tipoevencodi;

                model.IdTipoOperacion = (model.Entidad.Subcausacodiop != null) ? (int)model.Entidad.Subcausacodiop : -1;
                model.ListaSubCausaEvento = this.servicioEvento.ObtenerCausaEvento(model.IdTipoEvento);
                model.HoraInicial = (model.Entidad.Evenini != null) ?
                    ((DateTime)model.Entidad.Evenini).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                model.HoraFinal = (model.Entidad.Evenfin != null) ?
                    ((DateTime)model.Entidad.Evenfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            }

            return PartialView(model);
        }

        #endregion

        #endregion

        #region Pestaña "Costos C.T."

        /// <summary>
        /// >Exportar reporte de coastos CT
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaModosOperacionCT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteCostosCT(string fecha, List<ReporteExcelCT> listaModosOperacionCT)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servHO.GenerarExcelReporteCostosCT(listaModosOperacionCT, path, fecha, out string fileName);

                model.Resultado = 1;
                model.NombreArchivo = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Abrir archivo exportado
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            base.ValidarSesionJsonResult();
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(path, Constantes.AppExcel, file);
        }

        #endregion

        #region Pestaña "Correo"

        /// <summary>
        /// Permite visualizar el contenido del correo enviado
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerContenidoCorreo(int idCorreo)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                SiCorreoDTO entity = (new CorreoAppServicio()).GetByIdSiCorreo(idCorreo);
                model.Mensaje = entity.Corrcontenido;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        #endregion

        #region Continuar Día

        /// <summary>
        /// Registrar nuevo dia
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="strHopcodiPermitido"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarNuevoDia(string sfecha, string strHopcodiPermitido)
        {
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador)) throw new Exception(Constantes.MensajePermisoNoValido);

                //
                List<int> listaHopcodiPermitido = string.IsNullOrEmpty(strHopcodiPermitido) ? new List<int>() : strHopcodiPermitido.Split(',').Select(x => int.Parse(x)).ToList();
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime fechaIni = fecha;
                DateTime fechaFin = fechaIni.AddDays(1);
                List<int> listCodHop = new List<int>(), listCodHopElim = new List<int>();

                //
                var listaHorasOperacionBDAntes = this.servHO.GetListaHorasOperacionByCriteria(Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos), ConstantesHorasOperacion.ParamTipoOperacionTodos, fecha, fecha.AddDays(1), ConstantesHorasOperacion.IdTipoTermica, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));
                int resultado = this.servHO.RegistrarIniciarDia(fechaIni, fechaFin, base.UserName, listaHopcodiPermitido, ref listCodHop);
                var listaHorasOperacionBDDespues = this.servHO.GetListaHorasOperacionByCriteria(Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos), ConstantesHorasOperacion.ParamTipoOperacionTodos, fecha, fecha.AddDays(1), ConstantesHorasOperacion.IdTipoTermica, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));

                //
                string[] datos = new string[3];
                switch (resultado)
                {
                    case ConstantesHorasOperacion.TipoNuevoDiaExitoso:

                        ///////////////Guardar Envio//////////////////////////                
                        MeEnvioDTO envio = new MeEnvioDTO
                        {
                            Formatcodi = ConstantesHorasOperacion.IdFormato,
                            Fdatcodi = ConstantesIEOD.FdatcodiPadreHOP,
                            Archcodi = 0,
                            Emprcodi = ConstantesHorasOperacion.IdEmpresaTodos,
                            Estenvcodi = ParametrosEnvio.EnvioAprobado,
                            Cfgenvcodi = -1,

                            Enviofecha = DateTime.Now,
                            Enviofechaperiodo = fecha,

                            Lastdate = DateTime.Now,
                            Lastuser = base.UserName,
                            Userlogin = base.UserName
                        };

                        int idEnvio = servicio.SaveMeEnvio(envio);

                        /////////Grabar Detalle de envios////////////////////

                        listCodHop = listCodHop.Select(o => o).Distinct().ToList();
                        foreach (var entity in listCodHop)
                        {
                            servicio.SaveMeEnviodet(new MeEnviodetDTO
                            {
                                Enviocodi = idEnvio,
                                Envdetfpkcodi = entity,
                                Envdetusucreacion = base.UserName,
                                Envdetfeccreacion = DateTime.Now
                            });
                        }

                        //////////////////////////////////////////////////////

                        /// Enviar notificacion
                        this.servHO.EnviarCorreoNotificacionHayCambioHorasOperacion(listCodHopElim, listaHorasOperacionBDDespues, listaHorasOperacionBDAntes, DateTime.Now, fecha, 0, idEnvio);

                        //////////////////////////////////////////////////////

                        datos[0] = "1";
                        datos[1] = "Se registró correctamente las nuevas Horas de Operación";
                        break;
                    case ConstantesHorasOperacion.TipoNuevoDiaNoHayDataAnterior:
                        datos[0] = "2";
                        datos[1] = "No hay Horas de Operación del día anterior que finalicen a las 24:00:00";
                        break;
                    case ConstantesHorasOperacion.TipoNuevoDiaRegistroPrevioExistente:
                        datos[0] = "2";
                        datos[1] = "Las Horas de Operación tomadas del día anterior ya han sido registrados previamente y/o existe cruce de Horas de Operación";
                        break;
                }

                var json = Json(datos);
                json.MaxJsonLength = Int32.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Listar los modos de operación que tienen horas de operación con orden de parada
        /// </summary>
        /// <param name="sfecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarModoOrdParadaContinuarNuevoDia(string sfecha)
        {
            HOReporteModel model = new HOReporteModel();
            try
            {
                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaIni = fecha;
                DateTime fechaFin = fechaIni.AddDays(1);

                //
                model.ListaHorasOperacion = this.servHO.ListarHoOrdParCondContinuarNuevoDia(fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Finalizar dia
        /// </summary>
        /// <param name="sfecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FinalizarDia(string sfecha)
        {
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecha = DateTime.ParseExact(sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime fechaIni = fecha;
                DateTime fechaFin = fechaIni.AddDays(1);
                List<int> listCodHop = new List<int>(), listCodHopElim = new List<int>();

                //
                var listaHorasOperacionBDAntes = this.servHO.GetListaHorasOperacionByCriteria(Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos), ConstantesHorasOperacion.ParamTipoOperacionTodos, fecha, fecha.AddDays(1), ConstantesHorasOperacion.IdTipoTermica, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));
                int resultado = this.servHO.GuardarFinalizarDia(fechaIni, fechaFin, base.UserName, ref listCodHop);
                var listaHorasOperacionBDDespues = this.servHO.GetListaHorasOperacionByCriteria(Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos), ConstantesHorasOperacion.ParamTipoOperacionTodos, fecha, fecha.AddDays(1), ConstantesHorasOperacion.IdTipoTermica, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));

                //
                string[] datos = new string[3];
                switch (resultado)
                {
                    case ConstantesHorasOperacion.TipoFinalizarDiaExitoso:
                        ///////////////Guardar Envio//////////////////////////                
                        MeEnvioDTO envio = new MeEnvioDTO
                        {
                            Formatcodi = ConstantesHorasOperacion.IdFormato,
                            Fdatcodi = ConstantesIEOD.FdatcodiPadreHOP,
                            Archcodi = 0,
                            Emprcodi = ConstantesHorasOperacion.IdEmpresaTodos,
                            Estenvcodi = ParametrosEnvio.EnvioAprobado,
                            Cfgenvcodi = -1,

                            Enviofecha = DateTime.Now,
                            Enviofechaperiodo = fecha,

                            Lastdate = DateTime.Now,
                            Lastuser = base.UserName,
                            Userlogin = base.UserName
                        };

                        int idEnvio = servicio.SaveMeEnvio(envio);

                        /////////Grabar Detalle de envios////////////////////

                        listCodHop = listCodHop.Select(o => o).Distinct().ToList();
                        foreach (var entity in listCodHop)
                        {
                            servicio.SaveMeEnviodet(new MeEnviodetDTO
                            {
                                Enviocodi = idEnvio,
                                Envdetfpkcodi = entity,
                                Envdetusucreacion = base.UserName,
                                Envdetfeccreacion = DateTime.Now
                            });
                        }
                        //////////////////////////////////////////////////////

                        /// Enviar notificacion
                        this.servHO.EnviarCorreoNotificacionHayCambioHorasOperacion(listCodHopElim, listaHorasOperacionBDDespues, listaHorasOperacionBDAntes, DateTime.Now, fecha, 0, idEnvio);

                        //////////////////////////////////////////////////////

                        datos[0] = "1";
                        datos[1] = "Se finalizó correctamente las Horas de Operación";
                        break;
                    case ConstantesHorasOperacion.TipoFinalizarDiaNoHayData:
                        datos[0] = "2";
                        datos[1] = "No existen Horas de Operación cuyo FIN REGISTRO sea " + ConstantesHorasOperacion.HoraFinDefecto + " y no tengan Orden de Parada";
                        break;
                    case ConstantesHorasOperacion.TipoFinalizarDiaNoExisteRegCierre:
                        datos[0] = "2";
                        datos[1] = "Las Horas de Operación ya han sido finalizadas previamente y/o existe cruce de Horas de Operación";
                        break;
                }

                var json = Json(datos);
                json.MaxJsonLength = Int32.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

        #region Manual de usuario

        /// <summary>
        /// //Mostrar del manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult MostrarManual()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioDocumentoIEOD + "Manual_Usuario_Horas_Operacion.pdf";
                return File(fullPath, Constantes.AppPdf, "Manual_Usuario_Horas_Operacion.pdf");
            }
            return null;
        }

        #endregion

        #region Filtro de empresas SEIN

        /// <summary>
        /// Actualiza la informacion necesaria para cargar los filtros de busqueda en la vista principal
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosBusqueda(HOReporteModel model, string tipoCentrales)
        {
            base.ValidarSesionUsuario();

            model.ListaEmpresas = this.servHO.ListarEmpresasHorasOperacionByTipoCentral(true, null, tipoCentrales);
            model.ListaEmpresas = model.ListaEmpresas.Count > 0 ? model.ListaEmpresas : new List<SiEmpresaDTO>() { new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No Existe" } };

            if (model.Fecha == null)
            {
                model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
                model.HoraMinutoActual = DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora);
            }
        }

        #endregion

        /// <summary>
        /// IEOD/Contenido del IEOD/Reporte Horas de Operación/
        /// </summary>
        /// <returns></returns>
        public ActionResult Reporte()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            HorasOperacionModel model = new HorasOperacionModel
            {
                ListaTipoCentral = this.servHO.ListarTipoCentralHOP(),
                Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha),
                FechaFin = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha),
                ListaTipoOperacion = this.servHO.ListarTipoOperacionHO()
            };
            return View(model);
        }

        #region Reporte Histórico - IEOD

        /// <summary>
        /// Genera  View de Listado de Horas de Operación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sIdTipoOperacion"></param>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaReporte(int idEmpresa, string sIdTipoOperacion, string sFechaIni, string sFechaFin, int idTipoCentral, int idCentral, bool checkCompensar, string idFiltroEnsayoPe, string idFiltroEnsayoPMin)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            DateTime fechaInicial = sFechaIni != null ? DateTime.ParseExact(sFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFinal = sFechaFin != null ? DateTime.ParseExact(sFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            fechaFinal = fechaFinal.AddDays(1);

            try
            {
                TimeSpan ts = fechaFinal.Subtract(fechaInicial);

                if (ts.TotalDays > 92)
                {
                    throw new Exception("El lapso de tiempo no puede ser mayor a 3 meses.");
                }

                string url = Url.Content("~/");
                model.Reporte = this.servHO.ListarReporteHOPHtml(url, idEmpresa, sIdTipoOperacion, fechaInicial, fechaFinal, idTipoCentral, idCentral, checkCompensar, idFiltroEnsayoPe, idFiltroEnsayoPMin);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Listar Empresa por Tipo de central
        /// </summary>
        /// <param name="tipoGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaEmpresaXTipoCentral(int tipoCentral)
        {
            HOReporteModel model = new HOReporteModel();
            this.CargarFiltrosBusqueda(model, ConstantesHorasOperacion.CodFamilias);
            string listaFamcodi = tipoCentral == ConstantesHorasOperacion.IdTipoCentralTodos ? ConstantesHorasOperacion.CodFamilias : tipoCentral.ToString();
            List<SiEmpresaDTO> listaEmprXFam = this.servicio.ListarEmpresasxTipoEquipos(listaFamcodi);
            var emprcodis = listaEmprXFam.Select(x => x.Emprcodi).Distinct().ToList();
            model.ListaEmpresas = model.ListaEmpresas.Where(x => emprcodis.Contains(x.Emprcodi)).OrderBy(x => x.Emprnomb).ToList();

            return Json(model);
        }

        /// <summary>
        /// Genera View de ventana para edición de horas de operación
        /// </summary>
        /// <param name="hopcodi"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="central"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="subCausacodi"></param>
        /// <returns></returns>
        public PartialViewResult PopUpEditarHorasOperacion(int hopcodi, int idEmpresa, int idTipoCentral, int central)
        {
            HorasOperacionModel model = new HorasOperacionModel();

            var entity = this.servHO.GetByIdEveHoraoperacion(hopcodi);
            model.HoraOperacion = entity;
            model.Hopcodi = hopcodi;
            model.IdEmpresa = idEmpresa;
            model.IdTipoCentral = idTipoCentral;
            model.IdCentralSelect = central;

            model.Fecha = ((DateTime)(entity.Hophorini)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaIni = entity.Hophorini != null ? entity.Hophorini.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            model.FechaFin = entity.Hophorfin != null ? entity.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            model.Hophorordarranq = entity.Hophorordarranq != null ? entity.Hophorordarranq.Value.ToString(ConstantesAppServicio.FormatoOnlyHora) : string.Empty;
            model.HoraIni = entity.Hophorini != null ? entity.Hophorini.Value.ToString(ConstantesAppServicio.FormatoOnlyHora) : string.Empty;
            model.Hophorparada = entity.Hophorparada != null ? entity.Hophorparada.Value.ToString(ConstantesAppServicio.FormatoOnlyHora) : string.Empty;
            model.HoraFin = entity.Hophorfin != null ? entity.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoOnlyHora) : string.Empty;

            model.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();
            model.IdTipoOperSelect = entity.Subcausacodi.Value;

            entity.Emprnomb = this.servicio.GetByIdEmpresa(idEmpresa).Emprnomb;
            entity.PadreNombre = this.servicio.GetEquipo(central).Equinomb;
            switch (idTipoCentral)
            {
                case ConstantesHorasOperacion.IdTipoSolar:
                case ConstantesHorasOperacion.IdTipoEolica:
                    entity.PadreNombre = this.servicio.GetEquipo(entity.Equicodi.Value).Equinomb;
                    break;
                case ConstantesHorasOperacion.IdTipoHidraulica:
                    model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaGrupo;
                    entity.EquipoNombre = this.servicio.GetEquipo(entity.Equicodi.Value).Equinomb;
                    break;
                case ConstantesHorasOperacion.IdTipoTermica:
                    model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaModo;
                    entity.EquipoNombre = this.servicio.GetByIdPrGrupo(entity.Grupocodi.Value).Gruponomb;
                    break;
            }

            model.OpfueraServ = entity.Hopfalla == ConstantesHorasOperacion.HopFalla ? 1 : 0;

            return PartialView(model);
        }

        /// <summary>
        /// Exportar reporte de Horas de Operación
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelReporteHOP(int idEmpresa, string sIdTipoOperacion, string sFechaIni, string sFechaFin, int idTipoCentral, int idCentral, bool checkCompensar, string idFiltroEnsayoPe, string idFiltroEnsayoPMin)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                DateTime fechaInicial = sFechaIni != null ? DateTime.ParseExact(sFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                DateTime fechaFinal = sFechaFin != null ? DateTime.ParseExact(sFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

                TimeSpan ts = fechaFinal.Subtract(fechaInicial);

                if (ts.TotalDays > 92)
                {
                    throw new Exception("El lapso de tiempo no puede ser mayor a 3 meses.");
                }

                List<EveHoraoperacionDTO> lista = this.servHO.GetListaHorasOperacionByCriteria(idEmpresa, sIdTipoOperacion, fechaInicial, fechaFinal.AddDays(1), idTipoCentral, idCentral, idFiltroEnsayoPe);
                if (idFiltroEnsayoPe != ConstantesAppServicio.ParametroDefecto) lista = lista.Where(x => x.Hopensayope == idFiltroEnsayoPe).ToList();
                if (idFiltroEnsayoPMin != ConstantesAppServicio.ParametroDefecto) lista = lista.Where(x => x.Hopensayopmin == idFiltroEnsayoPMin).ToList();
                lista = this.servHO.GetListaHorasOperacionByCriteriaWithDesglose(fechaInicial, fechaFinal, lista);
                ruta = this.servHO.GenerarFileExcelReporteHOP(lista, fechaInicial, fechaFinal, checkCompensar, true, true);
                datos[0] = ruta;
                datos[1] = ConstantesHorasOperacion.NombreArchivoHOP;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Permite descargar el archivo de 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelReporte()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// Exportación de Excel para OSINERGMIN
        /// </summary>
        /// <param name="sFechaIni"></param>
        /// <param name="sFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelReporteHOPOsinergmin(string sFechaIni, string sFechaFin, bool checkCompensar)
        {
            string[] datos = new string[2];
            try
            {
                DateTime fechaInicial = sFechaIni != null ? DateTime.ParseExact(sFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                DateTime fechaFinal = sFechaFin != null ? DateTime.ParseExact(sFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                fechaFinal = fechaFinal.AddDays(1);

                string ruta = this.servHO.GenerarFileExcelReporteHOPOsinergmin(fechaInicial, fechaFinal, checkCompensar);
                datos[0] = ruta;
                datos[1] = ConstantesHorasOperacion.NombreArchivoHOP;

                var jsonResult = Json(datos);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        #endregion

    }
}
