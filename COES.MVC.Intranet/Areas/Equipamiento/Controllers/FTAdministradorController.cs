using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTAdministradorController : BaseController
    {
        readonly FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        readonly DespachoAppServicio _appDespacho = new DespachoAppServicio();

        private int IdOpcionEnvioFormato = ConstantesFichaTecnica.IdoptionAdminFicha;

        #region Declaracion de variables

        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTAdministradorController));
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

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesFormato.SesionNombreArchivo] != null) ?
                    Session[ConstantesFormato.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesFormato.SesionNombreArchivo] = value; }
        }

        #endregion

        #region Pantalla principal

        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAdministradorModel model = new FTAdministradorModel();
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesFichaTecnica.EstadoSolicitud : carpeta.Value;

            model.ListaEmpresas = servFictec.ListarEmpresasExtranetFT();
            model.NumeroEmpresas = model.ListaEmpresas.Count();
            model.ListaEtapas = servFictec.ListFtExtEtapas();
            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Devuelve el listado de envios segun carpeta
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BloqueEnvios(string empresas, int ftetcodi, int idEstado, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                model.ListadoEnvios = servFictec.ObtenerListadoEnviosEtapa(empresas, idEstado, fechaInicio, fechaFin, ftetcodi.ToString());

                model.HtmlCarpeta = servFictec.GenerarHtmlCarpeta(empresas, idEstado, fechaInicio, fechaFin, ftetcodi);
                model.UsarFechaSistemaManual = ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTHoraSistemaManual] == "S";
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
        /// /Genera el archivo excel de listado de envíos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string empresas, int ftetcodi, int idEstado, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_EnvíoFichaTécnica_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) +
                                                string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servFictec.GenerarExportacionEnvios(ruta, pathLogo, empresas, fechaInicio, fechaFin, idEstado, ftetcodi, nameFile);
                model.Resultado = nameFile;
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
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar(string file_name)
        {
            base.ValidarSesionUsuario();

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file_name;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }

        /// <summary>
        /// Valores por defecto para ampliación, fecha inicio revisión (solicitud / subsanación)
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosEnvio(int ftenvcodi)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                FtExtEnvioDTO envio = servFictec.GetByIdFtExtEnvio(ftenvcodi);

                model.Ftenvcodi = ftenvcodi;

                //para ampliación de plazo
                if (envio.Ftenvfecsistema != null)
                {
                    model.FechaPlazo = (envio.Ftenvfecsistema.Value).ToString(Constantes.FormatoFecha);
                    decimal horas = (decimal)(((DateTime)envio.Ftenvfecsistema).Hour);
                    decimal minutos = ((decimal)((DateTime)envio.Ftenvfecsistema).Minute) / 60;
                    model.HoraPlazo = (int)((horas + minutos) * 2);  //value de Hora actual en 30min
                }
                else
                {
                    DateTime fechaUltimaModif2 = envio.Ftenvfecmodificacion ?? envio.Ftenvfecsolicitud;
                    model.FechaPlazo = fechaUltimaModif2.ToString(Constantes.FormatoFecha);
                    model.HoraPlazo = DateTime.Now.Hour * 2 + 1;  //value de Hora actual en 30min
                }

                //si tiene subsanacion (automatica o de forma manual) obtener la primera
                DateTime fechaUltimaModif = envio.Ftenvfecmodificacion ?? envio.Ftenvfecsolicitud;
                var listaLogEnvio = servFictec.GetByCriteriaFtExtEnvioLogs(ftenvcodi).OrderBy(x => x.Ftelogfeccreacion).ToList();
                var objPrimerSubs = listaLogEnvio.FirstOrDefault(x => x.Estenvcodi == ConstantesFichaTecnica.EstadoSubsanacionObs);
                if (objPrimerSubs != null) fechaUltimaModif = objPrimerSubs.Ftelogfeccreacion;

                model.FechaInicio = fechaUltimaModif.ToString(Constantes.FormatoFecha);


                //para habilitar revisión
                DateTime? fechaRev = null;
                if (envio.Estenvcodi == ConstantesFichaTecnica.EstadoSolicitud) fechaRev = envio.Ftenvfecinirev1;
                if (envio.Estenvcodi == ConstantesFichaTecnica.EstadoSubsanacionObs) fechaRev = envio.Ftenvfecinirev2;

                model.FechaPlazoRevision = fechaRev != null ? fechaRev.Value.ToString(ConstantesAppServicio.FormatoFecha) : ""; //fecha default

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
        /// Guarda la fecha del sistema para Envio
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="fechaoriginal"></param>
        /// <param name="fechasistema"></param>
        /// <param name="horasistema"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFechaSistema(int ftenvcodi, string fechaoriginal, string fechasistema, int horasistema)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = servFictec.ObtenerFechaSistemaFT(ftenvcodi);
                DateTime dateFechaSistema = DateTime.ParseExact(fechasistema, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaConHora = dateFechaSistema.AddMinutes((horasistema - 1) * 30);

                int result = DateTime.Compare(fechaActual, fechaConHora);

                if (result >= 0)
                {
                    throw new ArgumentException("La fecha de sistema debe ser posterior a " + fechaActual.ToString(ConstantesAppServicio.FormatoFechaFull));
                }

                servFictec.GuardarFechaSistema(ftenvcodi, base.UserName, fechaConHora);
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
        /// 
        /// </summary>
        /// <param name="Ftenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosFinPlazo(int Ftenvcodi)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                FtExtEnvioDTO envio = servFictec.GetByIdFtExtEnvio(Ftenvcodi);
                List<FtExtEnvioEqDTO> listaEquipoEnvio = servFictec.GetByCriteriaFtExtEnvioEqs(envio.VersionActual.Ftevercodi.ToString());

                model.Ftenvcodi = Ftenvcodi;
                if (envio.Ftenvfecsistema != null)
                {
                    var fechaSistema = (DateTime)envio.Ftenvfecsistema;
                    model.FechaInicio = fechaSistema.ToString(Constantes.FormatoFecha);
                    model.FechaPlazo = fechaSistema.AddHours(24).ToString(Constantes.FormatoFecha);
                    decimal horas = (decimal)(((DateTime)envio.Ftenvfecsistema).Hour);
                    decimal minutos = ((decimal)((DateTime)envio.Ftenvfecsistema).Minute) / 60;
                    model.HoraPlazo = (int)((horas + minutos) * 2);  //value de Hora actual en 30min
                }
                else
                {
                    var fechaSolicitud = (DateTime)envio.Ftenvfecsolicitud;
                    model.FechaInicio = fechaSolicitud.ToString(Constantes.FormatoFecha);
                    model.FechaPlazo = fechaSolicitud.AddHours(24).ToString(Constantes.FormatoFecha);
                    model.HoraPlazo = DateTime.Now.Hour * 2 + 1;  //value de Hora actual en 30min
                }
                model.Ftenvcodi = envio.Ftenvcodi;
                model.Emprnomb = envio.Emprnomb;
                model.Ftprynombre = envio.Ftprynombre;

                if (envio.Ftetcodi == ConstantesFichaTecnica.EtapaModificacion
                    && envio.Ftenvtipoformato == ConstantesFichaTecnica.FormatoBajaModoOperacion)
                {
                    model.CodigoModo = listaEquipoEnvio.First().Grupocodi.Value;
                    PrGrupoDTO objGrupo = servFictec.GetByIdPrGrupo(model.CodigoModo);
                    model.ModoOperacion = objGrupo;
                }
                else
                {
                    PrGrupoDTO objGrupo = new PrGrupoDTO();
                    model.ModoOperacion = objGrupo;
                }

                model.NombreEquipos = string.Join(", ", listaEquipoEnvio.Select(x => string.Format("{0} ({1})", x.Nombreelemento ?? "", x.Areaelemento ?? "")));

                model.Ftetcodi = envio.Ftetcodi; //id de la etapa
                model.Ftetnombre = envio.Ftetnombre; //etapa nombre
                model.EnvioTipoFormato = envio.Ftenvtipoformato; //tipo de formato

                model.FechaSubsanacion = envio.FtenvfecmodificacionDesc;//fecha de subsanacion
                model.PlazoFinSubsanar = envio.FtenvfecfinsubsanarobsDesc; //fecha max subsana

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
        /// Guarda la fecha de la ampliacion
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="fechaoriginal"></param>
        /// <param name="fechasistema"></param>
        /// <param name="horasistema"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAmpliarFecha(int ftenvcodi, string fechaFinPlazo, int horaFinPlazo)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = servFictec.ObtenerFechaSistemaFT(ftenvcodi);

                DateTime dateFechaFinPlazo = DateTime.ParseExact(fechaFinPlazo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaConHoraFinPlazo = dateFechaFinPlazo.AddMinutes((horaFinPlazo - 1) * 30);

                int result = DateTime.Compare(fechaActual, fechaConHoraFinPlazo);

                if (result >= 0)
                {
                    throw new ArgumentException("El Fin de plazo ingresado está fuera del rango permitido  (posterior al " + fechaActual.ToString(ConstantesAppServicio.FormatoFechaFull) + ")");
                }

                //actualizar la fecha de amplificacion
                servFictec.GuardarFechaAmplificar(ftenvcodi, base.UserName, fechaConHoraFinPlazo);

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
        /// Grabar habilitar plazo para inicio de revisión (solicitud o subsanación de observaciones)
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="fechaFinPlazo"></param>
        /// <param name="horaFinPlazo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFechaInicioRevision(int ftenvcodi, string fechaIniPlazo)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime dateFechaIniPlazo = DateTime.ParseExact(fechaIniPlazo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //actualizar la fecha de ini rev1 o rev2
                servFictec.GuardarFechaInicioPlazoRevision(ftenvcodi, base.UserName, dateFechaIniPlazo);

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
        /// Check para permitir agregar / eliminar equipos en operación comercial
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HabilitarEdicionEquipos(int ftenvcodi, string flag)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);


                servFictec.ActualizarEdiciónEquipos(ftenvcodi, flag);
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

        #region Envío de Formato Intranet para Conexión, Integración y Modificación de Ficha Técnica 

        /// <summary>
        /// Muestra la ventana principal para envios en etapas Conexión, Integración y Modificación de Ficha Técnica  (Sin dar baja)
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="accion"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="codigoEtapa"></param>
        /// <param name="codigoProyecto"></param>
        /// <param name="codigoEquipos"></param>
        /// <returns></returns>
        public ActionResult EnvioFormato(int codigoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            FTAdministradorModel model = new FTAdministradorModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            //solo lectura
            string tipo = objEnvio.TipoAccionIntranet != 0 ? (objEnvio.TipoAccionIntranet == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";
            model.TipoOpcion = tipo;
            model.IdEnvio = codigoEnvio;
            model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvio, ConstantesFichaTecnica.INTRANET);

            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.Ftprycodi = objEnvio.Ftprycodi ?? 0;
            model.Ftprynombre = objEnvio.Ftprynombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = objEnvio.Estenvcodi;
            model.CarpetaEstadoDesc = objEnvio.Estenvnomb;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvio, ConstantesFichaTecnica.INTRANET);
            model.EsFTModificada = servFictec.VerificarSiParametrosFueronModificados(model.IdVersion);
            model.MsgCancelacion = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvio.Ftenvobs != null ? objEnvio.Ftenvobs.Trim() : "") : "";

            #region Acciones en envios

            //Obtenemos dias observacion segun etapa            
            model.PlazoFinSubsanar = servFictec.ObtenerPlazoDiasParaSubsanarSegunEtapa(objEnvio);

            model.AgenteUltimoEvento = objEnvio.AgenteUltimoEvento;
            model.OtrosAgentesEmpresaDifUsuarioUltEvento = ObtenerStrcorreosAgentePorEmpresaDistintosA(objEnvio.Emprcodi, objEnvio.AgenteUltimoEvento);

            DateTime fechaSistemaEnvio = servFictec.ObtenerFechaSistemaFT(codigoEnvio);
            model.strFechaSistema = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFecha)) : "";

            model.HtmlListadoCVA = servFictec.GenerarHtmlListaCostosVariableFT("A");
            model.HtmlListadoCVAP = servFictec.GenerarHtmlListaCostosVariableFT("AP");
            model.UsarFechaSistemaManual = ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTHoraSistemaManual] == "S";
            model.FechaSistemaFull = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFechaFull2)) : "";
            model.EnlaceSistemaIntranet = servFictec.ObtenerEnlaceSistemaIntranetFT(codigoEnvio, objEnvio.Ftetcodi, objEnvio.Ftenvtipoformato);

            model.FlagFaltaHabilitarPlazo = servFictec.ValidarEnvioNoTienePlazoRevision(objEnvio);
            DateTime fecD = servFictec.ObtenerDiasHabiles(fechaSistemaEnvio, 2);
            model.FechaDerivacion = fecD != null ? fecD.ToString(ConstantesAppServicio.FormatoFecha) : "";
            model.FlagVersionDerivada = servFictec.VerificarDerivacionDeVersion(objEnvio);

            #endregion

            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarEqConexIntegModifXEnvio(int codigoEnvio, int versionEnvio)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);
                model.LstFteeqcodis = string.Join(",", model.ListaEnvioEq.Select(x => x.Fteeqcodi));
                model.LstEnviosEqNombres = model.ListaEnvioEq.Any() ? string.Join(",", model.ListaEnvioEq.Select(x => x.Nombreelemento.Trim())) : "";

                model.ValidacionEnvio = servFictec.ObtenerListadoErroresIntranet(codigoEnvio, versionEnvio);

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
        /// Obtiene la informacion para armar la estructura izquierda de la tabla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ObtenerDatosFT(int fteeqcodi, string areasUsuario)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ReporteDatoXEq = servFictec.ObtenerFichaTreeXEnvioEq(fteeqcodi, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionParametrosAFT(fteeqcodi, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionAreasFT = servFictec.ObtenerDatosRevisionAreasCOESFT(fteeqcodi, areasUsuario);
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
        public JsonResult GenerarFormatoConexIntegModif(int codigoEnvio, int versionEnvio, string areasUsuario, string codigoEquipos = "")
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servFictec.GenerarFormatoConexIntegModifXEnvio(ruta, pathLogo, codigoEnvio, codigoEquipos, null, ConstantesFichaTecnica.INTRANET, versionEnvio, areasUsuario, out string fileName); //SOS

                model.Resultado = fileName;
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
        public JsonResult LeerFileUpExcelFormatoConexIntegModif(int codigoEnvio, int estado, int versionEnvio)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                List<FTReporteExcel> listaEqEnv = servFictec.ListarLecturaExcelFormatoConexIntegModif(codigoEnvio, this.NombreFile, ConstantesFichaTecnica.INTRANET, estado, versionEnvio, "", 0, "", out string mensajes);
                if (mensajes != "")
                    throw new ArgumentException(mensajes);

                model.ListaRevisionImportacion = listaEqEnv;

                //realizar autoguardado
                bool flagHuboAutoguardado = servFictec.RealizarAutoguardadoVersionTemporalIntranet(codigoEnvio, versionEnvio, listaEqEnv, base.UserName, false);

                //volver a obtener los errores y lista de autoguardados
                model.ValidacionEnvio = servFictec.ObtenerListadoErroresIntranet(codigoEnvio, versionEnvio);

                ToolsFormato.BorrarArchivo(this.NombreFile);

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

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFormatoConexIntegModif()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesFormato.ExtensionFile;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerListadoParametrosModificadosAP(int ftenvcodi, string fitcfgcodiAprobados, string fitcfgcodiDenegados)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                FtExtEnvioDTO regEnvio = servFictec.GetByIdFtExtEnvio(ftenvcodi);
                int codigoVersionUltimo = regEnvio.VersionTemporalIntranet.Ftevercodi;
                int codigoVersionAnterior = regEnvio.FtevercodiTemporalFTVigente;

                model.HtmlParametrosModifAprobados = servFictec.ObtenerHtmlTablaParametrosModificadosAP(ftenvcodi, codigoVersionUltimo, codigoVersionAnterior, fitcfgcodiAprobados, ConstantesFichaTecnica.TipoParametrosModificadosAprobados);
                model.HtmlParametrosModifDenegados = servFictec.ObtenerHtmlTablaParametrosModificadosAP(ftenvcodi, codigoVersionUltimo, codigoVersionAnterior, fitcfgcodiDenegados, ConstantesFichaTecnica.TipoParametrosModificadosDenegados);
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

        #region Envío de Formato Intranet para Operacion Comercial

        /// <summary>
        /// muestra la ventana principal para envios en operacion comercial
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="accion"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="codigoEtapa"></param>
        /// <param name="codigoProyecto"></param>
        /// <param name="codigoEquipos"></param>
        /// <returns></returns>
        public ActionResult EnvioFormatoOperacionComercial(int codigoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            FTAdministradorModel model = new FTAdministradorModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            //solo lectura
            string tipo = objEnvio.TipoAccionIntranet != 0 ? (objEnvio.TipoAccionIntranet == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";
            model.TipoOpcion = tipo;
            model.IdEnvio = codigoEnvio;
            model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvio, ConstantesFichaTecnica.INTRANET);

            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.Ftprycodi = objEnvio.Ftprycodi ?? 0;
            model.Ftprynombre = objEnvio.Ftprynombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = objEnvio.Estenvcodi;
            model.CarpetaEstadoDesc = objEnvio.Estenvnomb;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvio, ConstantesFichaTecnica.INTRANET);
            model.MsgCancelacion = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvio.Ftenvobs != null ? objEnvio.Ftenvobs.Trim() : "") : "";

            #region Acciones en envios

            //Obtenemos dias observacion segun etapa
            model.PlazoFinSubsanar = servFictec.ObtenerPlazoDiasParaSubsanarSegunEtapa(objEnvio);

            model.AgenteUltimoEvento = objEnvio.AgenteUltimoEvento;
            model.OtrosAgentesEmpresaDifUsuarioUltEvento = ObtenerStrcorreosAgentePorEmpresaDistintosA(objEnvio.Emprcodi, objEnvio.AgenteUltimoEvento);

            DateTime fechaSistemaEnvio = servFictec.ObtenerFechaSistemaFT(codigoEnvio);
            model.strFechaSistema = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFecha)) : "";
            model.UsarFechaSistemaManual = ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTHoraSistemaManual] == "S";
            model.FechaSistemaFull = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFechaFull2)) : "";
            model.EnlaceSistemaIntranet = servFictec.ObtenerEnlaceSistemaIntranetFT(codigoEnvio, objEnvio.Ftetcodi, objEnvio.Ftenvtipoformato);

            model.FlagFaltaHabilitarPlazo = servFictec.ValidarEnvioNoTienePlazoRevision(objEnvio);
            DateTime fecD = servFictec.ObtenerDiasHabiles(fechaSistemaEnvio, 2);
            model.FechaDerivacion = fecD != null ? fecD.ToString(ConstantesAppServicio.FormatoFecha) : "";
            model.FlagVersionDerivada = servFictec.VerificarDerivacionDeVersion(objEnvio);
            #endregion

            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            return View(model);
        }

        /// <summary>
        /// Lista los archivos de cada requisito
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="codigoEtapa"></param>
        /// <param name="codigoProyecto"></param>
        /// <param name="codigoEquipos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRequisitoXEnvio(int codigoEnvio, int versionEnvio, string areasUsuario)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

                model.Evento = servFictec.GetByIdFtExtEvento(objEnvio.Ftevcodi ?? 0);
                model.ListaReqEvento = servFictec.ListarRequisitoXEnvioVersion(objEnvio.Ftevcodi ?? 0, versionEnvio, ConstantesFichaTecnica.INTRANET);
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);
                model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionContenidoReq(versionEnvio, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionAreasContenido = servFictec.ObtenerDatosRevisionAreasContenidoCOESFT(versionEnvio, areasUsuario);

                model.ValidacionEnvio = servFictec.ObtenerListadoErroresIntranet(codigoEnvio, versionEnvio);
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
        public JsonResult GenerarFormatoRevisionContenidoOC(int codigoEnvio, string areasUsuario)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio); //tipo formato: operación comercial o dar de baja
                servFictec.GenerarFormatoRevisionContenido(ruta, pathLogo, codigoEnvio, objEnvioAct.Ftenvtipoformato, areasUsuario, 0, out string fileName);

                model.Resultado = fileName;
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

        #region Envío de Formato Extranet para Dar de baja a modo de operación

        /// <summary>
        /// Ventana principal para envios en dar de baja
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="accion"></param>
        /// <param name="codigoModoBaja"></param>
        /// <returns></returns>
        public ActionResult EnvioFormatoBajaModoOperacion(int codigoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            FTAdministradorModel model = new FTAdministradorModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            //solo lectura
            string tipo = objEnvio.TipoAccionIntranet != 0 ? (objEnvio.TipoAccionIntranet == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";
            model.TipoOpcion = tipo;
            model.IdEnvio = codigoEnvio;
            model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvio, ConstantesFichaTecnica.INTRANET);

            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = objEnvio.Estenvcodi;
            model.CarpetaEstadoDesc = objEnvio.Estenvnomb;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvio, ConstantesFichaTecnica.INTRANET);
            model.MsgCancelacion = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvio.Ftenvobs != null ? objEnvio.Ftenvobs.Trim() : "") : "";

            #region Acciones en envios

            //Obtenemos dias observacion segun etapa
            model.PlazoFinSubsanar = servFictec.ObtenerPlazoDiasParaSubsanarSegunEtapa(objEnvio);

            model.AgenteUltimoEvento = objEnvio.AgenteUltimoEvento;
            model.OtrosAgentesEmpresaDifUsuarioUltEvento = ObtenerStrcorreosAgentePorEmpresaDistintosA(objEnvio.Emprcodi, objEnvio.AgenteUltimoEvento);

            DateTime fechaSistemaEnvio = servFictec.ObtenerFechaSistemaFT(codigoEnvio);
            model.strFechaSistema = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFecha)) : "";
            model.UsarFechaSistemaManual = ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTHoraSistemaManual] == "S";
            model.FechaSistemaFull = fechaSistemaEnvio != null ? (fechaSistemaEnvio.ToString(ConstantesAppServicio.FormatoFechaFull2)) : "";
            model.EnlaceSistemaIntranet = servFictec.ObtenerEnlaceSistemaIntranetFT(codigoEnvio, objEnvio.Ftetcodi, objEnvio.Ftenvtipoformato);

            model.FlagFaltaHabilitarPlazo = servFictec.ValidarEnvioNoTienePlazoRevision(objEnvio);
            DateTime fecD = servFictec.ObtenerDiasHabiles(fechaSistemaEnvio, 2);
            model.FechaDerivacion = fecD != null ? fecD.ToString(ConstantesAppServicio.FormatoFecha) : "";
            model.FlagVersionDerivada = servFictec.VerificarDerivacionDeVersion(objEnvio);
            #endregion

            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            return View(model);
        }

        #endregion

        #region Envío - Guardado manual del administrador

        [HttpPost]
        public JsonResult GuardarDatosFT(FTReporteExcel modelWeb)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                //guardar cambios
                bool flagHuboAutoguardado = servFictec.RealizarAutoguardadoVersionTemporalIntranet(modelWeb.Ftenvcodi, modelWeb.Ftevercodi, new List<FTReporteExcel>() { modelWeb }, base.UserName, true);

                //volver a obtener los errores y lista de autoguardados                
                model.ValidacionEnvio = servFictec.ObtenerListadoErroresIntranet(modelWeb.Ftenvcodi, modelWeb.Ftevercodi);

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

        #region Envío - Documentos

        [HttpPost]
        public JsonResult VistaPreviaArchivoEnvio(int idEnvio, int idVersion, int idElemento, string fileName, string tipoArchivo)
        {
            FTArchivoModel model = new FTArchivoModel();

            try
            {
                base.ValidarSesionUsuario();

                string pathSesionID = servFictec.GetPathSubcarpeta(ConstantesFichaTecnica.SubcarpetaSolicitud) + servFictec.GetSubcarpetaEnvio(idEnvio, idVersion, idElemento, tipoArchivo) + @"/";
                string pathDestino = ConstantesFichaTecnica.RutaReportes;
                FileServer.CopiarFileAlterFinalOrigen(pathSesionID, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                string url = pathDestino + fileName;

                model.Resultado = url;
                model.Detalle = fileName;
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

        /// <summary>
        /// Graba los archivos directamente en la carpeta del id seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTemporal(int idEnvio, int idVersion, int idElemento, string tipoArchivo)
        {
            FTArchivoModel model = new FTArchivoModel();

            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    DateTime fechaAhora = DateTime.Now;
                    servFictec.UploadArchivoEnvioTemporal(idEnvio, idVersion, idElemento, tipoArchivo, file.FileName,
                                                                file.InputStream, fechaAhora, out string fileNamefisico);

                    return Json(new { success = true, nuevonombre = fileNamefisico }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(new { success = model.Resultado == "1", response = model }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Descargar el archivo que está en el FileServer
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idConcepto"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(int idEnvio, int idVersion, int idElemento, string tipoArchivo, string fileName, string fileNameOriginal, bool esArchivoConf = false)
        {
            if (esArchivoConf)
            {
                bool esAdminFT = base.TienePermisosConfidencialFT(Acciones.Confidencial);

                if (esAdminFT)
                {
                    byte[] buffer = servFictec.GetBufferArchivoEnvioFinal(idEnvio, idVersion, idElemento, tipoArchivo, fileName);

                    var archivo = File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOriginal);

                    return archivo;
                }
                else
                {
                    //descargar .txt cuando no tiene permisos
                    string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                    return Redirect(urlArchivoSinPermiso);
                }
            }
            else
            {
                byte[] buffer = servFictec.GetBufferArchivoEnvioFinal(idEnvio, idVersion, idElemento, tipoArchivo, fileName);

                var archivo = File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOriginal);

                return archivo;
            }
        }

        #endregion

        #region Acciones sobre envio: Observar, Aprobar, Aprobar parcial, Denegar, Derivar

        /// <summary>
        /// Guarda la observacion de envio para cualquier etapa
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="data"></param>
        /// <param name="fecmaxrpta"></param>
        /// <returns></returns>
        public JsonResult GrabarObservacionFT(int ftenvcodi, string fecmaxrpta)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);
                if (string.IsNullOrEmpty(fecmaxrpta)) throw new ArgumentException("No ingresó la fecha máxima de respuesta.");

                DateTime fechaMaxR = DateTime.ParseExact(fecmaxrpta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //Observar envio y enviar notificación
                FtExtEnvioDTO regEnvio = servFictec.ObservarEnvioFT(ftenvcodi, fechaMaxR, base.UserName);

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
        /// Guarda la Denegacion de envio para cualquier etapa
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="data"></param>
        /// <param name="fecmaxrpta"></param>
        /// <returns></returns>
        public JsonResult GrabarDenegacionFT(int ftenvcodi, string fecVigencia, string mensaje, string ccAgentes)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime? fechaNull = null;
                DateTime? fechaVigencia = fecVigencia != "" ? DateTime.ParseExact(fecVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : fechaNull;

                //Actualizar estado de envio
                servFictec.DenegarEnvioFT(ftenvcodi, fechaVigencia, mensaje, ccAgentes, base.UserName);

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
        /// Aprueba un envío
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="data"></param>
        /// <param name="fecVigencia"></param>
        /// <param name="enlaceSI"></param>
        /// <param name="enlaceCarta"></param>
        /// <param name="enlaceOtro"></param>
        /// <param name="idCV"></param>
        /// <param name="ccAgentes"></param>
        /// <param name="hayParamVacios"></param>
        /// <param name="opcionReemplazo"></param>
        /// <returns></returns>
        public JsonResult GrabarAprobacionFT(int ftenvcodi, string fecVigencia, string enlaceSI, string enlaceCarta,
            string enlaceOtro, string idCV, string ccAgentes, string opcionReemplazo)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);
                if (string.IsNullOrEmpty(fecVigencia)) throw new ArgumentException("No ingresó la fecha vigencia.");

                DateTime fechaVigencia = DateTime.ParseExact(fecVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                FtExtEnvioDTO env = servFictec.GetByIdFtExtEnvio(ftenvcodi);

                PrRepcvDTO oRepCv = new PrRepcvDTO();
                List<FtExtEnvioDatoDTO> lstTotalParametros = new List<FtExtEnvioDatoDTO>();

                //Obtenemos los parametros antes de aprobar
                if (env.Ftenvtipoformato == ConstantesFichaTecnica.FormatoConexIntegModif)
                {
                    //valido que la fecha vigencia sea igual a la del CV elegido
                    if (idCV != "")
                    {
                        int repcodi = Int32.Parse(idCV);
                        oRepCv = _appDespacho.GetByIdPrRepcv(repcodi);

                        int result = DateTime.Compare(fechaVigencia, oRepCv.Repfecha.Date);

                        if (result != 0)
                        {
                            throw new ArgumentException("La fecha vigencia debe coincidir con la fecha del evento elegido.");
                        }
                    }

                    lstTotalParametros = servFictec.ListarParametrosEnvioFT(ftenvcodi, env.Ftenvtipocasoesp, env.FtevercodiTemporalIntranet, env.Ftetcodi, "");
                }

                //Obtenemos los parametros antes de aprobar
                if (env.Ftenvtipoformato == ConstantesFichaTecnica.FormatoOperacionComercial)
                {
                    lstTotalParametros = servFictec.ListarParametrosEnvioOpComercial(ftenvcodi, env.Ftenvtipocasoesp, env.FtevercodiTemporalIntranet, env.Ftetcodi);
                }

                //guardo los enlaces del popup aprobar y generar enlaces de los archivos y sustentos
                FTParametroGuardar objParametrosGuardar = servFictec.ListarPropiedadesYConceptosAActualizar(lstTotalParametros, opcionReemplazo, fechaVigencia, enlaceSI, enlaceCarta, enlaceOtro, base.UserName);
                objParametrosGuardar.Ftenvcodi = ftenvcodi;
                string msjValEnlace = servFictec.ObtenerMensajeValidacionEnlaceSustento(objParametrosGuardar);
                if (!string.IsNullOrEmpty(msjValEnlace))
                {
                    throw new ArgumentException("La longitud combinada de los enlaces de sustento supera los 400 caracteres: " + msjValEnlace);
                }

                //Actualizar estado de envio
                FtExtEnvioDTO regEnvio = servFictec.AprobarEnvioFT(ftenvcodi, fechaVigencia, ccAgentes, enlaceSI, enlaceCarta, enlaceOtro, base.UserName);
                servFictec.ListarCambiosEtapaIntegracionFTCasoEspecial1(ftenvcodi, regEnvio.VersionOficialActual.Ftevercodi, env.Ftenvtipoformato, "0", true);

                //datos en eq_propequi, pr_grupodat y actualizar Costos variables
                if (lstTotalParametros.Any())
                {
                    //Copiar archivos a otras carpetas para descarga directa desde el fileapp
                    servFictec.GenerarZipYCopiarAFileServerFileappFT(ftenvcodi, regEnvio.VersionOficialActual.Ftevercodi, lstTotalParametros);

                    //guardar en PR_GRUPODAT y EQ_PROPEQUI
                    servFictec.ActualizarPropiedadesYConceptos(objParametrosGuardar);

                    //Genero CV en BD
                    if (idCV != "")
                    {
                        var lsCostosVariables = new List<PrCvariablesDTO>();
                        _appDespacho.GenerarCostosVariables(oRepCv, ref lsCostosVariables, true);
                    }
                }

                //Cambio estados a los equipos segun etapa
                servFictec.ActualizarEstadosEquiposAlAprobar(ftenvcodi, base.UserName);

                //Actualizar equipos en etapas
                servFictec.ActualizarEquiposEtapas(ftenvcodi, base.UserName);

                //crear versión temporal que puede ser denegado en este caso especial
                if (regEnvio.Ftetcodi == ConstantesFichaTecnica.EtapaConexion)
                {
                    servFictec.CrearVersionTrabajoFromVersionBD(ftenvcodi, regEnvio.VersionOficialActual.Ftevercodi,
                                                            regEnvio.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporal, regEnvio.Ftenvtipoformato, "SISTEMA");
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
        /// Aprueba parcialmente un envio
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="data"></param>
        /// <param name="fecVigencia"></param>
        /// <param name="enlaceSI"></param>
        /// <param name="enlaceCarta"></param>
        /// <param name="enlaceOtro"></param>
        /// <param name="idCV"></param>
        /// <param name="ccAgentes"></param>
        /// <param name="hayParamVacios"></param>
        /// <param name="opcionReemplazo"></param>
        /// <returns></returns>
        public JsonResult GrabarAprobacionParcialFT(int ftenvcodi, string fecVigencia, string enlaceSI, string enlaceCarta,
            string enlaceOtro, string idCV, string ccAgentes, string opcionReemplazo, string fitcfgcodiAprobados, string fitcfgcodiDenegados)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);
                if (string.IsNullOrEmpty(fecVigencia)) throw new ArgumentException("No ingresó la fecha vigencia.");

                DateTime fechaVigencia = DateTime.ParseExact(fecVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                FtExtEnvioDTO env = servFictec.GetByIdFtExtEnvio(ftenvcodi);
                int etapa = env.Ftetcodi;
                int tipoFormato = env.Ftenvtipoformato;

                //valido que la fecha vigencia sea igual a la del CV elegido
                PrRepcvDTO oRepCv = new PrRepcvDTO();
                if (tipoFormato == ConstantesFichaTecnica.FormatoConexIntegModif && idCV != "")
                {
                    int repcodi = Int32.Parse(idCV);
                    oRepCv = _appDespacho.GetByIdPrRepcv(repcodi);

                    int result = DateTime.Compare(fechaVigencia, oRepCv.Repfecha.Date);

                    if (result != 0)
                    {
                        throw new ArgumentException("La fecha vigencia debe coincidir con la fecha del evento elegido.");
                    }
                }

                List<FtExtEnvioDatoDTO> lstTotalParametros = new List<FtExtEnvioDatoDTO>();
                if (tipoFormato == ConstantesFichaTecnica.FormatoConexIntegModif)
                {
                    //Obtenemos los parametros antes de aprobar (Solo de equipos con todos los items aprobados)
                    lstTotalParametros = servFictec.ListarParametrosEnvioFT(ftenvcodi, env.Ftenvtipocasoesp, env.VersionActual.Ftevercodi, etapa, fitcfgcodiAprobados);
                }

                //Obtenemos los parametros antes de aprobar
                if (env.Ftenvtipoformato == ConstantesFichaTecnica.FormatoOperacionComercial)
                {
                    lstTotalParametros = servFictec.ListarParametrosEnvioOpComercial(ftenvcodi, env.Ftenvtipocasoesp, env.FtevercodiTemporalIntranet, env.Ftetcodi);
                }
                FTParametroGuardar objParametrosGuardar = servFictec.ListarPropiedadesYConceptosAActualizar(lstTotalParametros, opcionReemplazo, fechaVigencia, enlaceSI, enlaceCarta, enlaceOtro, base.UserName);
                string msjValEnlace = servFictec.ObtenerMensajeValidacionEnlaceSustento(objParametrosGuardar);
                if (!string.IsNullOrEmpty(msjValEnlace))
                {
                    throw new ArgumentException("La longitud combinada de los enlaces de sustento supera los 400 caracteres: " + msjValEnlace);
                }

                //Actualizar estado de envio
                FtExtEnvioDTO regEnvio = servFictec.AprobarParcialmenteEnvioFT(ftenvcodi, fechaVigencia, ccAgentes,
                                                        enlaceSI, enlaceCarta, enlaceOtro, fitcfgcodiAprobados, fitcfgcodiDenegados, base.UserName);
                servFictec.ListarCambiosEtapaIntegracionFTCasoEspecial1(ftenvcodi, regEnvio.VersionOficialActual.Ftevercodi, env.Ftenvtipoformato, fitcfgcodiAprobados, true);

                if (tipoFormato == ConstantesFichaTecnica.FormatoConexIntegModif)
                {
                    //Copiar archivos a otras carpetas para descarga directa desde el fileapp
                    servFictec.GenerarZipYCopiarAFileServerFileappFT(ftenvcodi, regEnvio.VersionOficialActual.Ftevercodi, lstTotalParametros);

                    //guardar en PR_GRUPODAT y EQ_PROPEQUI
                    servFictec.ActualizarPropiedadesYConceptos(objParametrosGuardar);

                    //Genero CV en BD
                    if (idCV != "")
                    {
                        var lsCostosVariables = new List<PrCvariablesDTO>();
                        _appDespacho.GenerarCostosVariables(oRepCv, ref lsCostosVariables, true);
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
        /// Deriva la version de cierto envio
        /// </summary>
        /// <param name="ftenvcodi"></param>
        /// <param name="fecmaxrpta"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult GrabarDerivacionFT(int ftenvcodi, string fecmaxrpta)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormato);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);
                if (string.IsNullOrEmpty(fecmaxrpta)) throw new ArgumentException("No ingresó la fecha máxima de respuesta.");

                fecmaxrpta = fecmaxrpta + " 23:59:59";
                DateTime fechaMaxRpta = DateTime.ParseExact(fecmaxrpta, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);

                FtExtEnvioDTO envio = servFictec.GetByIdFtExtEnvio(ftenvcodi);

                int flagDerivacion = servFictec.VerificarDerivacionDeVersion(envio);

                if (flagDerivacion == 1)
                    throw new ArgumentException("Esta versión del envío ya presenta derivación.");

                string msgValidacion = servFictec.ValidarFilasConAreasAsignados(envio);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                servFictec.DerivarEnvioFT(envio, fechaMaxRpta, base.UserName);
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
        /// correos agentes en string por empresa diferentes a ciero usuario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="ususolicitud"></param>
        /// <returns></returns>
        private string ObtenerStrcorreosAgentePorEmpresaDistintosA(int idEmpresa, string ususolicitud)
        {
            ususolicitud = (ususolicitud ?? "").ToLower().Trim();

            var listaCorreo = ObtenerCorreosGeneradorModuloFT(idEmpresa);
            listaCorreo = listaCorreo.Where(x => x != ususolicitud).OrderBy(x => x).ToList();

            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloFT(int idEmpresa)
        {
            List<string> listaCorreo = new List<string>();

            //modulos extranet
            var listaModuloExtr = seguridad.ListarModulos().Where(x => (x.RolName.StartsWith("Usuario Extranet") || x.RolName.StartsWith("Extranet")) && x.ModEstado.Equals(ConstantesAppServicio.Activo)).OrderBy(x => x.ModNombre).ToList();

            //considerar solo a los usuarios activos de la empresa
            var listaUsuarios = seguridad.ListarUsuariosPorEmpresa(idEmpresa).Where(x => x.UserState == ConstantesAppServicio.Activo).ToList();
            foreach (var regUsuario in listaUsuarios)
            {
                var listaModuloXUsu = seguridad.ObtenerModulosPorUsuarioSelecion(regUsuario.UserCode).ToList();

                //modulos que tiene el usuario en extranet
                var listaModuloXUsuExt = listaModuloXUsu.Where(x => listaModuloExtr.Any(y => y.ModCodi == x.ModCodi)).ToList();

                var regFT = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesFichaTecnica.ModcodiFichaTecnicaExtranet);
                if (regFT != null && regFT.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }

        #endregion

        #region Descarga comprimida de archivos de sustento

        /// <summary>
        /// en intranet se usa el nombre de usuario y no el correo
        /// </summary>
        /// <returns></returns>
        private string GetCurrentCarpetaSesion()
        {
            return base.UserName;
        }

        /// <summary>
        /// generar zip de archivos no confidenciales
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="ftitcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarFmtExtSustento(int idEnvio, int ftitcodi, int codEquipo)
        {
            base.ValidarSesionJsonResult();
            servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, ftitcodi, codEquipo, ConstantesFichaTecnica.STipoArchivoSustentoDato, false, out string nameFile);

            //descargar archivo comprimido
            return ExportarZip(nameFile);
        }

        public virtual ActionResult DescargarFmtExtSustentoConf(int idEnvio, int ftitcodi, int codEquipo)
        {
            bool esAdminFT = base.TienePermisosConfidencialFT(Acciones.Confidencial);

            if (esAdminFT)
            {
                //TODO generar zip de archivos confidenciales
                servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, ftitcodi, codEquipo, ConstantesFichaTecnica.STipoArchivoSustentoDato, true, out string nameFile);

                //descargar archivo comprimido
                return ExportarZip(nameFile);
            }
            else
            {
                //descargar .txt cuando no tiene permisos
                string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                return Redirect(urlArchivoSinPermiso);
            }
        }

        public virtual ActionResult DescargarOpComSustento(int idEnvio, int idReq)
        {
            //TODO generar zip de archivos no confidenciales
            servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, idReq, 0, ConstantesFichaTecnica.STipoArchivoRequisito, false, out string nameFile);

            //descargar archivo comprimido
            return ExportarZip(nameFile);
        }

        public virtual ActionResult DescargarOpComSustentoConf(int idEnvio, int idReq)
        {
            bool esAdminFT = base.TienePermisosConfidencialFT(Acciones.Confidencial);

            if (esAdminFT)
            {
                //TODO generar zip de archivos confidenciales
                servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, idReq, 0, ConstantesFichaTecnica.STipoArchivoRequisito, true, out string nameFile);

                //descargar archivo comprimido
                return ExportarZip(nameFile);
            }
            else
            {
                //descargar .txt cuando no tiene permisos
                string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                return Redirect(urlArchivoSinPermiso);
            }
        }

        public virtual ActionResult DescargarBajaMopSustento(int idEnvio, int idReq)
        {
            //TODO generar zip de archivos no confidenciales
            servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, idReq, 0, ConstantesFichaTecnica.STipoArchivoRequisito, false, out string nameFile);

            //descargar archivo comprimido
            return ExportarZip(nameFile);
        }

        public virtual ActionResult DescargarBajaMopSustentoConf(int idEnvio, int idReq)
        {
            bool esAdminFT = base.TienePermisosConfidencialFT(Acciones.Confidencial);

            if (esAdminFT)
            {
                //TODO generar zip de archivos confidenciales
                servFictec.ExportarSustentos(GetCurrentCarpetaSesion(), idEnvio, idReq, 0, ConstantesFichaTecnica.STipoArchivoRequisito, true, out string nameFile);

                //descargar archivo comprimido
                return ExportarZip(nameFile);
            }
            else
            {
                //descargar .txt cuando no tiene permisos
                string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                return Redirect(urlArchivoSinPermiso);
            }
        }

        public virtual FileResult ExportarZip(string nameFile)
        {
            string nombreArchivo = nameFile;
            string modulo = ConstantesFichaTecnica.ModuloArchivosSustentoXEnvio;
            string pathDestino = ConstantesFichaTecnica.FolderRaizExtranetFT + "Temporal_" + modulo + @"/" + ConstantesFichaTecnica.NombreArchivosZip;
            string pathAlternativo = servFictec.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        #endregion

        #region Descarga comprimida de archivos de valor

        /// <summary>
        /// generar zip de archivos no confidenciales
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="ftitcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarFmtExtValor(int idEnvio, int ftitcodi, int codEquipo)
        {
            base.ValidarSesionJsonResult();
            servFictec.ExportarValor(GetCurrentCarpetaSesion(), idEnvio, ftitcodi, codEquipo, ConstantesFichaTecnica.STipoArchivoValorDato, false, out string nameFile);

            //descargar archivo comprimido
            return ExportarValorZip(nameFile);
        }

        public virtual ActionResult DescargarFmtExtValorConf(int idEnvio, int ftitcodi, int codEquipo)
        {
            bool esAdminFT = base.TienePermisosConfidencialFT(Acciones.Confidencial);

            if (esAdminFT)
            {
                //TODO generar zip de archivos confidenciales
                servFictec.ExportarValor(GetCurrentCarpetaSesion(), idEnvio, ftitcodi, codEquipo, ConstantesFichaTecnica.STipoArchivoValorDato, true, out string nameFile);

                //descargar archivo comprimido
                return ExportarValorZip(nameFile);
            }
            else
            {
                //descargar .txt cuando no tiene permisos
                string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                return Redirect(urlArchivoSinPermiso);
            }
        }

        public virtual FileResult ExportarValorZip(string nameFile)
        {
            string nombreArchivo = nameFile;
            string modulo = ConstantesFichaTecnica.ModuloArchivosValorXEnvido;
            string pathDestino = ConstantesFichaTecnica.FolderRaizExtranetFT + "Temporal_" + modulo + @"/" + ConstantesFichaTecnica.NombreArchivosZip;
            string pathAlternativo = servFictec.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        #endregion
    }
}