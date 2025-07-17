using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Subastas;
using COES.WebService.Proceso.Contratos;
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
using COES.Servicios.Aplicacion.Factory;
using COES.WebService.Proceso.Config;

namespace COES.WebService.Proceso.Servicios
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

        public ProcesoServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /*fin tem*/

        /// <summary>
        /// ObtenerTareasProgramadas
        /// </summary>
        /// <param name="nombreMetodo"></param>
        public List<SiProcesoDTO> ObtenerTareasProgramadasXBloque(int bloque)
        {
            log.Info("EjecutarProcesoTarea [Tarea automática]");
            List<SiProcesoDTO> listTask = this.logic.ObtenerTareasProgramadasXBloque(DateTime.Now, DateTime.Now.Hour, DateTime.Now.Minute, bloque);
            return listTask;
        }

        /// <summary>
        /// EnviarCorreoAlmacenamientoCombustibleCco
        /// </summary>
        public ResultadoProceso EnviarCorreoAlmacenamientoCombustibleCco(int prcscodi)
        {
            log.Info("EnviarCorreoAlmacenamientoCombustibleCco");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                CorreoAppServicio correoAppServicio = new CorreoAppServicio();
                correoAppServicio.EnviarCorreoAlmacenamientoCombustibleCco();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionPR16
        /// </summary>
        public ResultadoProceso NotificacionPR16(int prcscodi)
        {
            log.Info("NotificacionPR16");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                SiProcesoDTO entidad = FactorySic.GetSiProcesoRepository().GetById(prcscodi);
                DemandaMaximaAppServicio dremisionAppServicio = new DemandaMaximaAppServicio();
                dremisionAppServicio.Notificar((int)entidad.Modcodi);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// DesencriptarOfertaDiariaSubasta
        /// </summary>
        public ResultadoProceso DesencriptarOfertaDiariaSubasta(int prcscodi)
        {
            log.Info("DesencriptarOfertaDiariaSubasta");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                SubastasAppServicio subastaAppServicio = new SubastasAppServicio();
                subastaAppServicio.EjecutarProcesoAutomatico();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// EnviarNotificacionDemandaBarras
        /// </summary>
        public ResultadoProceso EnviarNotificacionDemandaBarras(int prcscodi)
        {
            log.Info("EnviarNotificacionDemandaBarras");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                COES.Servicios.Aplicacion.DemandaBarras.NotificacionAppServicio notificacionAppServicio = new COES.Servicios.Aplicacion.DemandaBarras.NotificacionAppServicio();
                notificacionAppServicio.NotificacionCargaDatos();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// SincronizacionMaestroAlpha
        /// </summary>
        public ResultadoProceso SincronizacionMaestroAlpha(int prcscodi)
        {
            log.Info("SincronizacionMaestroAlpha");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                SincronizaMaestroAppServicio sincMaestroAppServicio = new SincronizaMaestroAppServicio();
                sincMaestroAppServicio.IniciarSincronizacionTotal();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// ObtenerDirectorioNCP
        /// </summary>
        public ResultadoProceso ObtenerDirectorioNCP(int prcscodi)
        {
            log.Info("ObtenerDirectorioNCP");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalAppServicio servicio = new COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalAppServicio();
                servicio.ObtenerDirectorioNCP();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// EnviarNotificacionAppMovil
        /// </summary>
        public ResultadoProceso EnviarNotificacionAppMovil(int prcscodi)
        {
            log.Info("EnviarNotificacionAppMovil");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                PortalAppServicio servicio = new PortalAppServicio();
                servicio.EnviarNotificacion();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// ImportarEstadisticas
        /// </summary>
        public ResultadoProceso ImportarEstadisticas(int prcscodi)
        {
            log.Info("ImportarEstadisticas");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                SGDocAppServicio servicio = new SGDocAppServicio();
                SgdEstadisticasDTO filtro = new SgdEstadisticasDTO();
                filtro.ListaTipoAtencion = "1,3,5";
                filtro.FilterFechaInicio = DateTime.Today.AddDays(-70);
                filtro.FilterFechaFin = DateTime.Today;
                filtro.Sgdeusucreacion = "WS-AUTO";
                filtro.Sgdeusumodificacion = "WS-AUTO";
                servicio.Import(filtro);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionEquipamiento
        /// </summary>
        public ResultadoProceso NotificacionEquipamiento(int prcscodi)
        {
            log.Info("NotificacionEquipamiento");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                NotificacionCambioEquipos servicio = new NotificacionCambioEquipos();
                servicio.Procesar(5);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionMediciones
        /// </summary>
        public ResultadoProceso NotificacionMediciones(int prcscodi)
        {
            log.Info("NotificacionMediciones");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                NotificacionCambioPtoMedicion serv_ptomed = new NotificacionCambioPtoMedicion();
                serv_ptomed.NotificarCambio(30);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionCurvaEnsayo
        /// </summary>
        public ResultadoProceso NotificacionCurvaEnsayo(int prcscodi)
        {
            log.Info("NotificacionCurvaEnsayo");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                NotificacionCambioEquipos servicio = new NotificacionCambioEquipos();
                servicio.ProcesarNotificacionCurvaEnsayo(5);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// CalcularMaximaDemandaMensual
        /// </summary>
        public ResultadoProceso CalcularMaximaDemandaMensual(int prcscodi)
        {
            log.Info("CalcularMaximaDemandaMensual");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new PortalAppServicio()).CalcularMaximaDemandaMensual(DateTime.Now);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// CalculoCrecimientoAnualEnergia
        /// </summary>
        public ResultadoProceso CalculoCrecimientoAnualEnergia(int prcscodi)
        {
            log.Info("CalculoCrecimientoAnualEnergia");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new PortalAppServicio()).ProcesarGeneracionAcumulada();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// EjecutarIndicadores
        /// </summary>
        public ResultadoProceso EjecutarIndicadores(int prcscodi)
        {
            log.Info("EjecutarIndicadores");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                EjecucionIndicadoresAppServicio indicador = new EjecucionIndicadoresAppServicio();
                indicador.Procesar();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// ObtenerGeneracionEMS
        /// </summary>
        public ResultadoProceso ObtenerGeneracionEMS(int prcscodi)
        {
            log.Info("ObtenerGeneracionEMS");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                SupervisionDemandaAppServicio servicio = new SupervisionDemandaAppServicio();
                servicio.ObtenerGeneracionEMS();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// JobPronDema
        /// </summary>
        public ResultadoProceso JobPronDema(int prcscodi)
        {
            log.Info("JobPronDema");
            return new ResultadoProceso { Resultado = "E", Mensaje = "" };
            //(new PronosticoDemandaAppServicio()).JobProdem();
        }

        /// <summary>
        /// RegistroAutomaticoObservacionSeniales
        /// </summary>
        public ResultadoProceso RegistroAutomaticoObservacionSeniales(int prcscodi)
        {
            log.Info("RegistroAutomaticoObservacionSeniales");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new RegistroObservacionAppServicio()).ProcesarRegistroAutomaticoSeniales();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionSenialesObservadas
        /// </summary>
        public ResultadoProceso NotificacionSenialesObservadas(int prcscodi)
        {
            log.Info("NotificacionSenialesObservadas");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new RegistroObservacionAppServicio()).ProcesarNotificacionAutomatica();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// AseguramientoOperacionAlarma
        /// </summary>
        public ResultadoProceso AseguramientoOperacionAlarma(int prcscodi)
        {
            log.Info("AseguramientoOperacionAlarma");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new SeguimientoRecomendacionAppServicio()).EnvioAlarma();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// CalcularValorizacionDiaria
        /// </summary>
        public ResultadoProceso CalcularValorizacionDiaria(int prcscodi)
        {
            log.Info("CalcularValorizacionDiaria");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new COES.Servicios.Aplicacion.ValorizacionDiaria.ValorizacionDiariaAppServicio()).ValorizacionDiariaAutomatica();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// AvisoVencimientoEoEpo
        /// </summary>
        public ResultadoProceso AvisoVencimientoEoEpo(int prcscodi)
        {
            log.Info("AvisoVencimientoEoEpo");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                GestionEoEpoAppServicio gestioneoepo = new GestionEoEpoAppServicio();
                gestioneoepo.Procesar();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// EnviarCorreoAutomaticoDiarioSolicitudes
        /// </summary>
        public ResultadoProceso EnviarCorreoAutomaticoDiarioSolicitudes(int prcscodi)
        {
            log.Info(ConstantesExtranetCTAF.PrcsmetodoAlertaDiariaSolicitudesPendientes);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new AnalisisFallasAppServicio()).NotificarSolicitudesPendientes();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionDocumentosCOES
        /// </summary>
        public ResultadoProceso NotificacionDocumentosCOES(int prcscodi)
        {
            log.Info("NotificacionDocumentosCOES");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new TramiteVirtualAppServicio()).EnviarNotificacionAgente();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// AlmacenamientoInformacionBI
        /// </summary>
        public ResultadoProceso AlmacenamientoInformacionBI(int prcscodi)
        {
            log.Info("AlmacenamientoInformacionBI");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                generalAppServicio.AlmacenamientoInformacionBI();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionReportesDiarios
        /// </summary>
        public ResultadoProceso NotificacionReportesDiarios(int prcscodi)
        {
            log.Info("NotificacionReportesDiarios");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                generalAppServicio.NotificacionReportesDiarios();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        /// <summary>
        /// NotificacionProcedimientoTecnico
        /// </summary>
        public ResultadoProceso NotificacionProcedimientoTecnico(int prcscodi)
        {
            log.Info("NotificacionProcedimientoTecnico");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                COES.Servicios.Aplicacion.General.GeneralAppServicio generalAppServicio = new COES.Servicios.Aplicacion.General.GeneralAppServicio();
                generalAppServicio.notificacionProcedimientoTecnico();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #region PR31 Líquidos y Sólidos

        public ResultadoProceso PR31CulminacionPlazoAgente(int prcscodi)
        {
            log.Info(ConstantesCombustibles.MetodoPR31CulminacionPlazoAgente);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(1);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso PR31RecordatorioCOES(int prcscodi)
        {
            log.Info(ConstantesCombustibles.MetodoPR31RecordatorioCOES);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(2);
                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(4);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso PR31RecordatorioAgente(int prcscodi)
        {
            log.Info(ConstantesCombustibles.MetodoPR31RecordatorioAgente);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new CombustibleAppServicio()).EjecutarProcesoAutomaticoPR31(3);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #endregion

        #region PR31 Gaseosos
        public ResultadoProceso PR31GasRecordatorioCOES_E(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasRecordatorioCOES_N(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasCulminacionPlazoAgente_E(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasCulminacionPlazoAgente_N(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasRecordatorioAgente_E(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasRecordatorioAgente_N(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso PR31GasIncumplimiento_E(int prcscodi)
        {
            return this.EjecutarRecordatoriosPR31Gas(prcscodi);
        }

        public ResultadoProceso EjecutarRecordatoriosPR31Gas(int prcscodi)
        {
            log.Info("MetodoPR31Gas");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                var servCostoComb = new CombustibleAppServicio();
                //int plantillaCorreo = servCostoComb.ObtenerPlantillaCorreoSegunProceso(item.Prcscodi);
                int plantillaCorreo = servCostoComb.ObtenerPlantillaCorreoSegunProceso(prcscodi);
                Dictionary<int, string> lstUsuarioPorEmpresa = ListarEmpresaCorreoPR31Gaseosos();

                servCostoComb.EjecutarRecordatoriosManualmente(plantillaCorreo, lstUsuarioPorEmpresa);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error("EjecutarProcesoTarea - PR31 Gaseosos", ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #endregion

        #region Ficha Tecnica 2 - 2024 

        public ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Conexion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Integracion(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_OpComercial(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Modif(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_ModifBaja(int prcscodi)
        {
            return this.EjecutarRecordatoriosFT2(prcscodi);
        }

        public ResultadoProceso EjecutarRecordatoriosFT2(int prcscodi)
        {
            log.Info("MetodoFT2Noticiacion");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                var servicioFT = new FichaTecnicaAppServicio();
                int plantillaCorreo = servicioFT.ObtenerPlantillaCorreoSegunProceso(prcscodi, out int ftetcodi);
                servicioFT.EjecutarRecordatoriosManualmente(plantillaCorreo, ftetcodi);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error("EjecutarProcesoAutomaticoNotificaciones - Ficha Tecnica", ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// CargarCostosMarginalesProgramadosYupana
        /// </summary>
        public ResultadoProceso CargarCostosMarginalesProgramadosYupana(int prcscodi)
        {
            log.Info(ConstantesCortoPlazo.MetodoCargaCmgProgramadoYupana);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                (new CortoPlazoAppServicio()).CargarCostosMarginalesProgramadosYupana(fecha, fecha, "admin-carga");

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #region Mejoras EoEpo
        /// <summary>
        /// NotificacionVigenciaEoEpo
        /// </summary>
        public ResultadoProceso NotificacionVigenciaEoEpo(int prcscodi)
        {
            log.Info("NotificacionVigenciaEoEpo");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                GestionEoEpoAppServicio ProcesosEoEpo = new GestionEoEpoAppServicio();
                ProcesosEoEpo.ProcesosEoEpo();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #endregion

        #region ASSETEC

        public ResultadoProceso ObtenerDatosRawSco(int prcscodi)
        {
            log.Info("ObtenerDatosRawSco");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new PronosticoDemandaAppServicio()).ObtenerDatosRawSco();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso ObtenerDatosRawCada5Mninutos(int prcscodi)
        {
            return null;
        }

        public ResultadoProceso ObtenerDatosRawIEOD1Dia(int prcscodi)
        {
            log.Info("ObtenerDatosRawIEOD1Dia");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new DemandaPOAppServicio()).ObtenerDatosRawIEOD1Dia();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso FiltradoInformacionTNA(int prcscodi)
        {
            log.Info("FiltradoInformacionTNA");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new DemandaPOAppServicio()).FiltradoInformacionTNA(DateTime.Now, "mes", -1);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        #endregion

        #region Intervenciones

        public ResultadoProceso EnviarCorreoAlertaAutomatico(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAlertaIntervencionesProgNoEjec);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EnviarCorreoValidacionIntervencionProgramadaNoEjec();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso ProcesoAnularReversion(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoAnularReversion);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAnularReversion();

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso IntervencionRecordatorioSustentoDiario(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgDiario);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoRecordatorioInclExclDiario(DateTime.Today);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso IntervencionRecordatorioSustentoSemanal(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcsmetodoSustentoExclInclProgSemanal);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoRecordatorioInclExclSemanal(DateTime.Today);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso ProcesoAprobarProgramaDiaro(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcscodiAprobarProgDiario);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAprobacionDiario(DateTime.Today);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso ProcesoAprobarProgramaSemanal(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcscodiAprobarProgSemanal);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoAprobacionSemanal(DateTime.Today);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }

        public ResultadoProceso ProcesoGenerarVersiones(int prcscodi)
        {
            log.Info(ConstantesIntervencionesAppServicio.PrcscodiGenerarVersion);
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                (new IntervencionesAppServicio()).EjecutarProcesoAutomaticoGenerarVersiones(DateTime.Today);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        #endregion

        public ResultadoProceso ProcesarCna(int prcscodi)
        {
            log.Info("ProcesarCna");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                int numSemana = EPDate.f_numerosemana(DateTime.Now) - 1;
                string semanaperiodo = DateTime.Now.Year.ToString() + numSemana.ToString();
                DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, semanaperiodo, string.Empty, string.Empty).AddDays(2);
                DateTime fechaFinSemana = fechaIniSemana.AddDays(6);
                (new TransferenciasAppServicio()).ProcesarCna(fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha), fechaFinSemana.ToString(ConstantesAppServicio.FormatoFecha));
                (new TransferenciasAppServicio()).NotificacionCna(ConstantesAppServicio.PlantillacorreoCna, "sistemas");

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        public ResultadoProceso EnviarCorreoAnalisisFallaAlertaCitacion(int prcscodi)
        {
            log.Info("EnviarCorreoAnalisisFallaAlertaCitacion");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                var fecha = DateTime.Now;
                servAF.GenerarAlertasCitacion(fecha, Config.CorreoConstantes.CodigoCorreoAnalisisFallaAlertaCitacion);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        public ResultadoProceso EnviarCorreoAlertaElaboracionInformeCtaf(int prcscodi)
        {
            log.Info("EnviarCorreoAlertaElaboracionInformeCtaf");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                var fecha = DateTime.Now;
                servAF.GenerarAlertasElaboracionInformeCtaf(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeCtaf);
                servAF.GenerarAlertasElaboracionInformeCtafMasDosDiasHabiles(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        public ResultadoProceso EnviarCorreoAlertaElaboracionInformeTecnico(int prcscodi)
        {
            log.Info("EnviarCorreoAlertaElaboracionInformeTecnico");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                var fecha = DateTime.Now;
                servAF.GenerarAlertasElaboracionInformeTecnico(fecha, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnico);
                servAF.GenerarAlertasElaboracionInformeTecnicoMasDiasHabiles(fecha, 2, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles);
                servAF.GenerarAlertasElaboracionInformeTecnicoMasDiasHabiles(fecha, 5, Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        public ResultadoProceso EnviarCorreoAlertaElaboracionInformeTecnicoSemanal(int prcscodi)
        {
            log.Info("EnviarCorreoAlertaElaboracionInformeTecnicoSemanal");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    var fecha = DateTime.Now;
                    var fechaFin = DateTime.Now.AddDays(4);
                    servAF.GenerarAlertasElaboracionInformeTecnicoSemanal(fecha.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), Config.CorreoConstantes.CodigoCorreoAlertaElaboracionInformeTecnicoSemanal);
                }

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }
        public ResultadoProceso NotificacionIncumplimientoIEOD(int prcscodi)
        {
            log.Info("NotificacionIncumplimientoIEOD");
            //Agregar log proceso
            int prcslgcodi = AgregarInicioLog(prcscodi);
            try
            {
                DateTime fechaEnvio = DateTime.Now.AddDays(-1);
                (new StockCombustiblesAppServicio()).EnviarCorreoNotificacion(fechaEnvio);

                //Actualizar log exitoso
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "E", "");
            }
            catch (Exception ex)
            {
                log.Error(ConstantesAppServicio.LogError, ex);
                //Actualizar log fallido
                return ActualizarFinLog(prcslgcodi, DateTime.Now, "F", ex.Message);
            }
        }


        /// <summary>
        /// Permite obtener las alertas para el informe de fallas
        /// </summary>
        /// <returns></returns>
        public List<EveInformefallaDTO> ObtenerAlertaInformeFallas()
        {
            COES.Servicios.Aplicacion.Informefalla.InformefallaAppServicio servicio = new COES.Servicios.Aplicacion.Informefalla.InformefallaAppServicio();
            return servicio.ObtenerAlertaInformeFallas();
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

                var regPr31 = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesCombustibles.ModcodiPr31Extranet);
                if (regPr31 != null && regPr31.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }

        #endregion

        #region Utilidad

        public int AgregarInicioLog(int prcscodi)
        {
            SiProcesoLogDTO log = new SiProcesoLogDTO();
            log.Prcscodi = prcscodi;
            log.Prcslgfecha = DateTime.Today;
            log.Prcslginicio = DateTime.Now;
            log.Prcslgestado = "P";

            int logcodi = FactorySic.GetSiProcesoLogRepository().Save(log);

            return logcodi;
        }

        public ResultadoProceso ActualizarFinLog(int prcslgcodi, DateTime fechaFin, string estado, string mensaje)
        {
            ResultadoProceso result = new ResultadoProceso();
            SiProcesoLogDTO log = FactorySic.GetSiProcesoLogRepository().GetById(prcslgcodi);

            if (log != null)
            {
                log.Prcslgfin = DateTime.Now;
                log.Prcslgestado = estado;

                FactorySic.GetSiProcesoLogRepository().Update(log);
            }

            result.Resultado = estado;
            result.Mensaje = mensaje;

            return result;
        }

        #endregion
    }
}
