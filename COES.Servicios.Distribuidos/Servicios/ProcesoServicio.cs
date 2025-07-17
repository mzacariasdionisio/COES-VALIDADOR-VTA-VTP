using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Subastas;
using COES.Servicios.Distribuidos.Contratos;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using log4net;
using COES.Servicios.Aplicacion.CortoPlazo;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.SGDoc;
using COES.Servicios.Aplicacion.Equipamiento;

using COES.Servicios.Aplicacion.Coordinacion;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.RegistroObservacion;
using COES.Servicios.Aplicacion.Recomendacion;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.GestionEoEpo;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Despacho.Helper;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Configuration;
using COES.Servicios.Aplicacion.Despacho;
using System.Linq;
using COES.Servicios.Aplicacion.Helper;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class ProcesoServicio : IProcesoServicio
    {
        TareaProgramadaAppServicio logic = new TareaProgramadaAppServicio();
        AnalisisFallasAppServicio servAF = new AnalisisFallasAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ProcesoServicio));
        public DateTime FechaInicio { get; set; }
        public string[][] Data { get; set; }
        public DateTime FechaFin { get; set; }
        public List<string> ListaHoras { get; set; }
        public string TipoEstimador { get; set; }
        public bool FlagMD { get; set; }
        public string Usuario { get; set; }
        public string[][] DataTIE { get; set; }
        public int Barra { get; set; }
        public DateTime FechaProceso { get; set; }
        public int VersionModelo { get; set; }
        public string Correlativos { get; set; }

        public ProcesoServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Permite ejecutar proceso programado
        /// </summary>
        /// <param name="nombreMetodo"></param>
        public void EjecutarProcesoTarea()
        {
            try
            {
                log.Info("EjecutarProcesoTarea [Tarea automática]");
                List<SiProcesoDTO> listTask = this.logic.ObtenerTareasProgramadas(DateTime.Now);
                foreach (SiProcesoDTO item in listTask)
                {
                    switch (item.Prcsmetodo)
                    {
                        case "EnviarCorreoAlmacenamientoCombustibleCco":
                            {
                                log.Info("EnviarCorreoAlmacenamientoCombustibleCco");
                                CorreoAppServicio correoAppServicio = new CorreoAppServicio();
                                correoAppServicio.EnviarCorreoAlmacenamientoCombustibleCco();
                                break;
                            }
                        case "NotificacionPR16":
                            {
                                log.Info("NotificacionPR16");
                                DemandaMaximaAppServicio dremisionAppServicio = new DemandaMaximaAppServicio();
                                dremisionAppServicio.Notificar((int)item.Modcodi);
                                break;
                            }
                        #region Subastas RSF
                        case ConstantesSubasta.MetodoDesencriptarOfertaDiariaSubasta:
                            {
                                log.Info(item.Prcsmetodo);
                                SubastasAppServicio subastaAppServicio = new SubastasAppServicio();
                                subastaAppServicio.EjecutarProcesoAutomatico();
                                break;
                            }
                        case ConstantesSubasta.MetodoOfertaRSF_RecordatorioSinOfDefectoAnioSig:
                            {
                                log.Info(item.Prcsmetodo);
                                SubastasAppServicio subastaAppServicio = new SubastasAppServicio();
                                subastaAppServicio.EjecutarProcesoAutomaticoOfertaRSF(item.Prcscodi);
                                break;
                            }
                        case ConstantesSubasta.MetodoOfertaRSF_AutogeneracionOfDefectoAnioSig:
                            {
                                log.Info(item.Prcsmetodo);
                                SubastasAppServicio subastaAppServicio = new SubastasAppServicio();
                                subastaAppServicio.EjecutarProcesoAutomaticoOfertaRSF(item.Prcscodi);
                                break;
                            }
                        #endregion
                        case "EnviarNotificacionDemandaBarras":
                            {
                                log.Info("EnviarNotificacionDemandaBarras");
                                COES.Servicios.Aplicacion.DemandaBarras.NotificacionAppServicio notificacionAppServicio = new Aplicacion.DemandaBarras.NotificacionAppServicio();
                                notificacionAppServicio.NotificacionCargaDatos();
                                break;
                            }
                        case "SincronizacionMaestroAlpha":
                            {
                                log.Info("SincronizacionMaestroAlpha");
                                SincronizaMaestroAppServicio sincMaestroAppServicio = new SincronizaMaestroAppServicio();
                                sincMaestroAppServicio.IniciarSincronizacionTotal();
                                break;
                            }
                        case "ObtenerDirectorioNCP":
                            {
                                log.Info("ObtenerDirectorioNCP");
                                COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalAppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalAppServicio();
                                servicio.ObtenerDirectorioNCP();
                                break;
                            }
                        case "EnviarNotificacionAppMovil":
                            {
                                log.Info("EnviarNotificacionAppMovil");
                                PortalAppServicio servicio = new PortalAppServicio();
                                servicio.EnviarNotificacion();
                                break;
                            }
                        case "ImportarEstadisticas":
                            {
                                log.Info("ImportarEstadisticas");
                                SGDocAppServicio servicio = new SGDocAppServicio();
                                SgdEstadisticasDTO filtro = new SgdEstadisticasDTO();
                                filtro.ListaTipoAtencion = "1,3,5";
                                filtro.FilterFechaInicio = DateTime.Today.AddDays(-70);
                                filtro.FilterFechaFin = DateTime.Today;
                                filtro.Sgdeusucreacion = "WS-AUTO";
                                filtro.Sgdeusumodificacion = "WS-AUTO";
                                servicio.Import(filtro);
                                break;
                            }
                        //case "NotificacionEquipamiento":
                        //    {
                        //        log.Info("NotificacionEquipamiento");
                        //        NotificacionCambioEquipos servicio = new NotificacionCambioEquipos();
                        //        servicio.Procesar(5);
                        //        break;
                        //    }
                        case "NotificacionMediciones":
                            {
                                log.Info("NotificacionMediciones");
                                NotificacionCambioPtoMedicion serv_ptomed = new NotificacionCambioPtoMedicion();
                                serv_ptomed.NotificarCambio(30);
                                break;
                            }
                        case "NotificacionCurvaEnsayo":
                            {
                                log.Info("NotificacionCurvaEnsayo");
                                NotificacionCambioEquipos servicio = new NotificacionCambioEquipos();
                                servicio.ProcesarNotificacionCurvaEnsayo(5);
                                break;
                            }
                        case "CalcularMaximaDemandaMensual":
                            {
                                log.Info("CalcularMaximaDemandaMensual");
                                (new PortalAppServicio()).CalcularMaximaDemandaMensual(DateTime.Now);
                                break;
                            }
                        case "CalculoCrecimientoAnualEnergia":
                            {
                                log.Info("CalculoCrecimientoAnualEnergia");
                                (new PortalAppServicio()).ProcesarGeneracionAcumulada();
                                break;
                            }
                        case "EjecutarIndicadores":
                            {
                                log.Info("EjecutarIndicadores");
                                EjecucionIndicadoresAppServicio indicador = new EjecucionIndicadoresAppServicio();
                                indicador.Procesar();
                                break;
                            }
                        case "ObtenerGeneracionEMS":
                            {
                                log.Info("ObtenerGeneracionEMS");
                                SupervisionDemandaAppServicio servicio = new SupervisionDemandaAppServicio();
                                servicio.ObtenerGeneracionEMS();
                                break;
                            }
                        case "JobPronDema":
                            {
                                log.Info("JobPronDema");
                                //(new PronosticoDemandaAppServicio()).JobProdem();
                                break;
                            }
                        case "RegistroAutomaticoObservacionSeniales":
                            {
                                log.Info("RegistroAutomaticoObservacionSeniales");
                                (new RegistroObservacionAppServicio()).ProcesarRegistroAutomaticoSeniales();
                                break;
                            }
                        case "NotificacionSenialesObservadas":
                            {
                                log.Info("NotificacionSenialesObservadas");
                                (new RegistroObservacionAppServicio()).ProcesarNotificacionAutomatica();
                                break;
                            }
                        case "AseguramientoOperacionAlarma":
                            {
                                log.Info("AseguramientoOperacionAlarma");
                                (new SeguimientoRecomendacionAppServicio()).EnvioAlarma();
                                break;
                            }
                        case "CalcularValorizacionDiaria":
                            {
                                log.Info("CalcularValorizacionDiaria");
                                (new Aplicacion.ValorizacionDiaria.ValorizacionDiariaAppServicio()).ValorizacionDiariaAutomatica();
                                break;
                            }
                        case "AvisoVencimientoEoEpo":
                            {
                                log.Info("AvisoVencimientoEoEpo");
                                GestionEoEpoAppServicio gestioneoepo = new GestionEoEpoAppServicio();
                                gestioneoepo.Procesar();
                                break;
                            }
                        case ConstantesExtranetCTAF.PrcsmetodoAlertaDiariaSolicitudesPendientes:
                            {
                                log.Info(ConstantesExtranetCTAF.PrcsmetodoAlertaDiariaSolicitudesPendientes);
                                (new AnalisisFallasAppServicio()).NotificarSolicitudesPendientes();
                                break;
                            }
                        case "NotificacionDocumentosCOES":
                            {
                                log.Info("NotificacionDocumentosCOES");
                                (new TramiteVirtualAppServicio()).EnviarNotificacionAgente();
                                break;
                            }
                        case "AlmacenamientoInformacionBI":
                            {
                                log.Info("AlmacenamientoInformacionBI");
                                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                                generalAppServicio.AlmacenamientoInformacionBI();
                                break;
                            }
                        case "NotificacionReportesDiarios":
                            {
                                log.Info("NotificacionReportesDiarios");
                                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                                generalAppServicio.NotificacionReportesDiarios();
                                break;
                            }
                        case "NotificacionProcedimientoTecnico":
                            {
                                log.Info("NotificacionProcedimientoTecnico");
                                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                                generalAppServicio.notificacionProcedimientoTecnico();
                                break;
                            }
                        #region PR31 Líquidos y Sólidos
                        case ConstantesCombustibles.MetodoPR31CulminacionPlazoAgente:
                            {
                                log.Info(ConstantesCombustibles.MetodoPR31CulminacionPlazoAgente);
                                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(1);
                                break;
                            }
                        case ConstantesCombustibles.MetodoPR31RecordatorioCOES:
                            {
                                log.Info(ConstantesCombustibles.MetodoPR31RecordatorioCOES);
                                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(2);
                                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(4);
                                break;
                            }
                        case ConstantesCombustibles.MetodoPR31RecordatorioAgente:
                            {
                                log.Info(ConstantesCombustibles.MetodoPR31RecordatorioAgente);
                                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(3);
                                break;
                            }
                        #endregion
                        #region PR31 Gaseosos
                        case ConstantesCombustibles.MetodoPR31GasRecordatorioCOES_E:
                        case ConstantesCombustibles.MetodoPR31GasRecordatorioCOES_N:
                        case ConstantesCombustibles.MetodoPR31GasCulminacionPlazoAgente_E:
                        case ConstantesCombustibles.MetodoPR31GasCulminacionPlazoAgente_N:
                        case ConstantesCombustibles.MetodoPR31GasRecordatorioAgente_E:
                        case ConstantesCombustibles.MetodoPR31GasRecordatorioAgente_N:
                        case ConstantesCombustibles.MetodoPR31GasIncumplimiento_E:
                            {
                                log.Info("MetodoPR31Gas");
                                try
                                {
                                    var servCostoComb = new CombustibleAppServicio();
                                    int plantillaCorreo = servCostoComb.ObtenerPlantillaCorreoSegunProceso(item.Prcscodi);
                                    Dictionary<int, string> lstUsuarioPorEmpresa = ListarEmpresaCorreoPR31Gaseosos();

                                    servCostoComb.EjecutarRecordatoriosManualmente(plantillaCorreo, lstUsuarioPorEmpresa);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("EjecutarProcesoTarea - PR31 Gaseosos", ex);
                                }
                                break;
                            }
                        #endregion

                        #region Ficha Tecnica 2 - 2024 
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Modif:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Modif:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Modif:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Modif:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Modif:
                        case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_ModifBaja:

                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Conexion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Integracion:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_OpComercial:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Modif:
                        case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_ModifBaja:
                            {
                                log.Info("MetodoFT2NoticiacionCulminacionPlazoRevisarAreasSubsanacion");
                                try
                                {
                                    var servicioFT = new FichaTecnicaAppServicio();
                                    int plantillaCorreo = servicioFT.ObtenerPlantillaCorreoSegunProceso(item.Prcscodi, out int ftetcodi);                                    
                                    servicioFT.EjecutarRecordatoriosManualmente(plantillaCorreo, ftetcodi);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("EjecutarProcesoAutomaticoNotificaciones - Ficha Tecnica");
                                }
                                break;
                            }
                        #endregion

                        case ConstantesCortoPlazo.MetodoCargaCmgProgramadoYupana:
                            {
                                log.Info(ConstantesCortoPlazo.MetodoCargaCmgProgramadoYupana);
                                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                                (new CortoPlazoAppServicio()).CargarCostosMarginalesProgramadosYupana(fecha, fecha, "admin-carga");
                                break;
                            }
                        #region Mejoras EoEpo
                        case "NotificacionVigenciaEoEpo":
                            {
                                log.Info("NotificacionVigenciaEoEpo");
                                #region Mejoras EO-EPO-II
                                GestionEoEpoAppServicio ProcesosEoEpo = new GestionEoEpoAppServicio();
                                ProcesosEoEpo.ProcesosEoEpo();
                                break;
                                #endregion
                            }
                        #endregion
                        #region ASSETEC
                        //COES.Servicios.Aplicacion\PronosticoDemanda\PronosticoDemandaAppServicio.cs
                        case "ObtenerDatosRawSco":
                            {
                                log.Info("ObtenerDatosRawSco");
                                (new PronosticoDemandaAppServicio()).ObtenerDatosRawSco();
                                break;
                            }
                        //COES.Servicios.Aplicacion\DemandaPO\DemandaPOAppServicio.cs
                        case "ObtenerDatosRawIEOD1Dia":
                            {
                                log.Info("ObtenerDatosRawIEOD1Dia");
                                (new DemandaPOAppServicio()).ObtenerDatosRawIEOD1Dia();
                                break;
                            }
                        case "FiltradoInformacionTNA":
                            {
                                log.Info("FiltradoInformacionTNA");
                                (new DemandaPOAppServicio()).FiltradoInformacionTNA(DateTime.Now, "mes", -1);
                                break;
                            }
                        #endregion
                        #region Intervenciones

                        case ConstantesIntervencionesAppServicio.PrcsmetodoAlertaIntervencionesProgNoEjec:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAlertaIntervencionesProgNoEjec);
                                (new IntervencionesAppServicio()).EnviarCorreoValidacionIntervencionProgramadaNoEjec();
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoAnularReversion:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAnularReversion);
                                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAnularReversion();
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgDiario:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgDiario);
                                (new IntervencionesAppServicio()).EjecutarProcesoRecordatorioInclExclDiario(DateTime.Today);
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgSemanal:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgSemanal);
                                (new IntervencionesAppServicio()).EjecutarProcesoRecordatorioInclExclSemanal(DateTime.Today);
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoAprobarProgDiario:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAprobarProgDiario);
                                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAprobacionDiario(DateTime.Today);
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoAprobarProgSemanal:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAprobarProgSemanal);
                                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAprobacionSemanal(DateTime.Today);
                                break;
                            }

                        case ConstantesIntervencionesAppServicio.PrcsmetodoGenerarVersion:
                            {
                                log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoGenerarVersion);
                                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoGenerarVersiones(DateTime.Today);
                                break;
                            }

                        #endregion
                        case "ProcesarCna":
                            {
                                log.Info("ProcesarCna");
                                int numSemana = EPDate.f_numerosemana(DateTime.Now) - 1;
                                string semanaperiodo = DateTime.Now.Year.ToString() + numSemana.ToString();
                                DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, semanaperiodo, string.Empty, string.Empty).AddDays(2);
                                DateTime fechaFinSemana = fechaIniSemana.AddDays(6);
                                (new TransferenciasAppServicio()).ProcesarCna(fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha), fechaFinSemana.ToString(ConstantesAppServicio.FormatoFecha));
                                (new TransferenciasAppServicio()).NotificacionCna(ConstantesAppServicio.PlantillacorreoCna, "sistemas");
                                break;
                            }
                        case "EnviarCorreoAnalisisFallaAlertaCitacion":
                            {
                                log.Info("EnviarCorreoAnalisisFallaAlertaCitacion");
                                var fecha = DateTime.Now;
                                servAF.GenerarAlertasCitacion(fecha, Config.CorreoConstantes.CodigoCorreoAnalisisFallaAlertaCitacion);
                                break;
                            }
                        case "EnviarCorreoAlertaElaboracionInformeCtaf":
                            {
                                log.Info("EnviarCorreoAlertaElaboracionInformeCtaf");
                                var fecha = DateTime.Now;
                                servAF.GenerarAlertasElaboracionInformeCtaf(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeCtaf);
                                servAF.GenerarAlertasElaboracionInformeCtafMasDosDiasHabiles(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles);
                                break;
                            }
                        case "EnviarCorreoAlertaElaboracionInformeTecnico":
                            {
                                log.Info("EnviarCorreoAlertaElaboracionInformeTecnico");
                                var fecha = DateTime.Now;
                                servAF.GenerarAlertasElaboracionInformeTecnico(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnico);
                                servAF.GenerarAlertasElaboracionInformeTecnicoMasDiasHabiles(fecha,2, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles);
                                servAF.GenerarAlertasElaboracionInformeTecnicoMasDiasHabiles(fecha,5, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles);
                                break;
                            }
                        case "EnviarCorreoAlertaElaboracionInformeTecnicoSemanal":
                            {
                                log.Info("EnviarCorreoAlertaElaboracionInformeTecnicoSemanal");
                                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                                {
                                    var fecha = DateTime.Now;
                                    var fechaFin = DateTime.Now.AddDays(4);
                                    servAF.GenerarAlertasElaboracionInformeTecnicoSemanal(fecha.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoSemanal);
                                }
                                break;
                            }
                        case "NotificacionIncumplimientoIEOD":
                            {
                                log.Info("NotificacionIncumplimientoIEOD");
                                DateTime fechaEnvio = DateTime.Now.AddDays(-1);
                                (new StockCombustiblesAppServicio()).EnviarCorreoNotificacion(fechaEnvio);
                                break;
                            }
                        case "EnviarCorreoAlertaDatosFrecuencia":
                            {
                                log.Info("EnviarCorreoAlertaDatosFrecuencia");
                                var fecha = DateTime.Now;
                                servAF.GenerarAlertasDatosFrecuencia(fecha, Config.CorreoConstantes.CodigoCorreoAlertaDatosFrecuencia);
                                servAF.GenerarAlertasEventosFrecuencia(fecha, Config.CorreoConstantes.CodigoCorreoAlertaEventosFrecuencia);
                                servAF.GenerarAlertasRepSegFaltantes(fecha, Config.CorreoConstantes.CodigoCorreoAlertaReporteSegundosFaltantes);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("EjecutarProcesoTarea", ex);
            }
        }

        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public void EjecutarCostosMarginales(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb)
        {
            CalculoCostosMarginales cm = new CalculoCostosMarginales();
            cm.EjecutarProcesoCM(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, "automatico", 0);
        }

        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public void EjecutarCostosMarginalesAlterno(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb, string rutaNCP,
            bool flagMD, int idEscenario, string usuario, string tipoEstimador, int tipo, int version)
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarCM(fecha, tipoEstimador, indicadorPSSE, reproceso, indicadorNCP, flagWeb,
                rutaNCP, flagMD, idEscenario, usuario, tipo, version);

        }

        /// <summary>
        /// Validación de datos antes del proceso de calculo CM
        /// </summary>
        /// <param name="fecha"></param>
        public void ValidacionProcesoCostosMarginales(DateTime fecha)
        {
            CostoMarginalTnaAppServicio servicio = new CostoMarginalTnaAppServicio();
            servicio.ValidacionProceso(fecha);
        }

        /// <summary>
        /// Permite obtener las validaciones de costos marginales
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public COES.Servicios.Aplicacion.CortoPlazo.Helper.ResultadoValidacion ObtenerAlertasCostosMarginales(DateTime fechaProceso)
        {
            CostoMarginalTnaAppServicio servicio = new CostoMarginalTnaAppServicio();
            return servicio.ObtenerAlertasCostosMarginales(fechaProceso);
        }

        /// <summary>
        /// Permite obtener las alertas para el informe de fallas
        /// </summary>
        /// <returns></returns>
        public List<EveInformefallaDTO> ObtenerAlertaInformeFallas()
        {
            Aplicacion.Informefalla.InformefallaAppServicio servicio = new Aplicacion.Informefalla.InformefallaAppServicio();
            return servicio.ObtenerAlertaInformeFallas();
        }

        /// <summary>
        /// Reprocesar costos marginales masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="valores"></param>
        public int EjecutarReprocesoMasivo(DateTime fechaInicio, DateTime fechaFin, List<string> horas, bool flagMD, string usuario, string tipoEstimador,
            int version)
        {
            try
            {
                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;
                this.ListaHoras = horas;
                this.TipoEstimador = tipoEstimador;
                this.FlagMD = flagMD;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoAsincrono);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoAsincrono()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoCM(this.FechaInicio, this.FechaFin, this.ListaHoras, this.FlagMD, this.Usuario, this.TipoEstimador, this.VersionModelo);
           
        }

        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public int EjecutarPotenciaRemunerable(int pfpericodi, int pfrecacodi, int indrecacodiant, int recpotcodi, string usuario)
        {
            PotenciaFirmeRemunerableAppServicio cm = new PotenciaFirmeRemunerableAppServicio();

            int reporteCodiGenerado = cm.CalcularReportePFR(pfpericodi, pfrecacodi, indrecacodiant, recpotcodi, usuario);
            return reporteCodiGenerado;
        }

        /// <summary>
        /// Reprocesar costos marginales masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="valores"></param>
        public int EjecutarReprocesoMasivoModificado(string[][] datos, string usuario, int version)
        {
            try
            {
                this.Data = datos;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoAsincronoModificado);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoAsincronoModificado()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoCMModificado(this.Data, this.Usuario, this.VersionModelo);
        }


        public int EjecutarReprocesoTIE(string[][] datos, string usuario, int barra, DateTime fechaProceso, int version) 
        {
            try
            {
                this.DataTIE = datos;
                this.FechaProceso = fechaProceso;
                this.Barra = barra;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoTIE);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoTIE()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoTIE(this.DataTIE, this.Usuario, this.FechaProceso, this.Barra, this.VersionModelo);
        }

        public int EjecutarReprocesoVA(string horas, string usuario, DateTime fechaProceso, int version)
        {
            try
            {
                this.Correlativos = horas;
                this.FechaProceso = fechaProceso;          
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoVA);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoVA()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoVA(this.Correlativos, this.Usuario, this.FechaProceso, this.VersionModelo);
        }

        #region PR31 Gaseosos

        /// <summary>
        /// Mapa empresa y correos
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> ListarEmpresaCorreoPR31Gaseosos()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

            Dictionary<int, string> lstUsuarioPorEmpresa = new Dictionary<int, string>();

            List<CbCentralxfenergDTO> listaCentralTermicasTipoExistentesNuevas = (new CombustibleAppServicio()).ObtenerListadoCentralesTermicas(true).Where(x => x.Cbcxfeexistente == 1 || x.Cbcxfenuevo == 1).ToList();
            List<int> lstEmprcodisCentralesExistentesNuevas = listaCentralTermicasTipoExistentesNuevas.Select(x => x.Emprcodi).Distinct().ToList();

            foreach (var emprcodi in lstEmprcodisCentralesExistentesNuevas)
            {
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(emprcodi, "", seguridad);

                lstUsuarioPorEmpresa.Add(emprcodi, otrosUsuariosEmpresa);
            }

            return lstUsuarioPorEmpresa;
        }

        /// <summary>
        /// correos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="ususolicitud"></param>
        /// <param name="seguridad"></param>
        /// <returns></returns>
        private string ObtenerCCcorreosAgente(int idEmpresa, string ususolicitud, SeguridadServicio.SeguridadServicioClient seguridad)
        {
            ususolicitud = (ususolicitud ?? "").ToLower().Trim();

            var listaCorreo = ObtenerCorreosGeneradorModuloPr31(idEmpresa, seguridad);
            listaCorreo = listaCorreo.Where(x => x != ususolicitud).OrderBy(x => x).ToList();

            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="seguridad"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloPr31(int idEmpresa, SeguridadServicio.SeguridadServicioClient seguridad)
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

                var regPr31 = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesCombustibles.ModcodiPr31ExtranetGaseoso);
                if (regPr31 != null && regPr31.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }

        #endregion
    }
}
