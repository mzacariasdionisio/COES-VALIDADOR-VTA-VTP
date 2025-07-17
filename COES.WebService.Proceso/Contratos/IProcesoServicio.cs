using COES.Dominio.DTO.Sic;
using COES.WebService.Proceso.Config;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace COES.WebService.Proceso.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IProcesoServicio
    {

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "ObtenerTareasProgramadasXBloque/?bloque={bloque}")]
        List<SiProcesoDTO> ObtenerTareasProgramadasXBloque(int bloque);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAlmacenamientoCombustibleCco/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAlmacenamientoCombustibleCco(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionPR16/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionPR16(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DesencriptarOfertaDiariaSubasta/?prcscodi={prcscodi}")]
        ResultadoProceso DesencriptarOfertaDiariaSubasta(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarNotificacionDemandaBarras/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarNotificacionDemandaBarras(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SincronizacionMaestroAlpha/?prcscodi={prcscodi}")]
        ResultadoProceso SincronizacionMaestroAlpha(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ObtenerDirectorioNCP/?prcscodi={prcscodi}")]
        ResultadoProceso ObtenerDirectorioNCP(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarNotificacionAppMovil/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarNotificacionAppMovil(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ImportarEstadisticas/?prcscodi={prcscodi}")]
        ResultadoProceso ImportarEstadisticas(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionEquipamiento/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionEquipamiento(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionMediciones/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionMediciones(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionCurvaEnsayo/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionCurvaEnsayo(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CalcularMaximaDemandaMensual/?prcscodi={prcscodi}")]
        ResultadoProceso CalcularMaximaDemandaMensual(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CalculoCrecimientoAnualEnergia/?prcscodi={prcscodi}")]
        ResultadoProceso CalculoCrecimientoAnualEnergia(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EjecutarIndicadores/?prcscodi={prcscodi}")]
        ResultadoProceso EjecutarIndicadores(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ObtenerGeneracionEMS/?prcscodi={prcscodi}")]
        ResultadoProceso ObtenerGeneracionEMS(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "JobPronDema/?prcscodi={prcscodi}")]
        ResultadoProceso JobPronDema(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "RegistroAutomaticoObservacionSeniales/?prcscodi={prcscodi}")]
        ResultadoProceso RegistroAutomaticoObservacionSeniales(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionSenialesObservadas/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionSenialesObservadas(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AseguramientoOperacionAlarma/?prcscodi={prcscodi}")]
        ResultadoProceso AseguramientoOperacionAlarma(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CalcularValorizacionDiaria/?prcscodi={prcscodi}")]
        ResultadoProceso CalcularValorizacionDiaria(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AvisoVencimientoEoEpo/?prcscodi={prcscodi}")]
        ResultadoProceso AvisoVencimientoEoEpo(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAutomaticoDiarioSolicitudes/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAutomaticoDiarioSolicitudes(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionDocumentosCOES/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionDocumentosCOES(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AlmacenamientoInformacionBI/?prcscodi={prcscodi}")]
        ResultadoProceso AlmacenamientoInformacionBI(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionReportesDiarios/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionReportesDiarios(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionProcedimientoTecnico/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionProcedimientoTecnico(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31CulminacionPlazoAgente/?prcscodi={prcscodi}")]
        ResultadoProceso PR31CulminacionPlazoAgente(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31RecordatorioCOES/?prcscodi={prcscodi}")]
        ResultadoProceso PR31RecordatorioCOES(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31RecordatorioAgente/?prcscodi={prcscodi}")]
        ResultadoProceso PR31RecordatorioAgente(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasRecordatorioCOES_E/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasRecordatorioCOES_E(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasRecordatorioCOES_N/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasRecordatorioCOES_N(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasCulminacionPlazoAgente_E/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasCulminacionPlazoAgente_E(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasCulminacionPlazoAgente_N/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasCulminacionPlazoAgente_N(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasRecordatorioAgente_E/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasRecordatorioAgente_E(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasRecordatorioAgente_N/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasRecordatorioAgente_N(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PR31GasIncumplimiento_E/?prcscodi={prcscodi}")]
        ResultadoProceso PR31GasIncumplimiento_E(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotificacionCulminacionPlazoSubsanar_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotificacionCulminacionPlazoSubsanar_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotificacionCulminacionPlazoSubsanar_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotificacionCulminacionPlazoSubsanar_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotificacionCulminacionPlazoSubsanar_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotificacionCulminacionPlazoSubsanar_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSolicitudAreas_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSolicitud_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2RecordatorioVencPlazoRevisarSubsanacionAreas_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Conexion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Conexion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Integracion/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Integracion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_OpComercial/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_OpComercial(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Modif/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Modif(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_ModifBaja/?prcscodi={prcscodi}")]
        ResultadoProceso FT2NotifCulminacionPlazoRevisarAreasSubsanacion_ModifBaja(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CargarCostosMarginalesProgramadosYupana/?prcscodi={prcscodi}")]
        ResultadoProceso CargarCostosMarginalesProgramadosYupana(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionVigenciaEoEpo/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionVigenciaEoEpo(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ObtenerDatosRawSco/?prcscodi={prcscodi}")]
        ResultadoProceso ObtenerDatosRawSco(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ObtenerDatosRawCada5Mninutos/?prcscodi={prcscodi}")]
        ResultadoProceso ObtenerDatosRawCada5Mninutos(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ObtenerDatosRawIEOD1Dia/?prcscodi={prcscodi}")]
        ResultadoProceso ObtenerDatosRawIEOD1Dia(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FiltradoInformacionTNA/?prcscodi={prcscodi}")]
        ResultadoProceso FiltradoInformacionTNA(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAlertaAutomatico/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAlertaAutomatico(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ProcesoAnularReversion/?prcscodi={prcscodi}")]
        ResultadoProceso ProcesoAnularReversion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "IntervencionRecordatorioSustentoDiario/?prcscodi={prcscodi}")]
        ResultadoProceso IntervencionRecordatorioSustentoDiario(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "IntervencionRecordatorioSustentoSemanal/?prcscodi={prcscodi}")]
        ResultadoProceso IntervencionRecordatorioSustentoSemanal(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ProcesarCna/?prcscodi={prcscodi}")]
        ResultadoProceso ProcesarCna(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAnalisisFallaAlertaCitacion/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAnalisisFallaAlertaCitacion(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAlertaElaboracionInformeCtaf/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAlertaElaboracionInformeCtaf(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAlertaElaboracionInformeTecnico/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAlertaElaboracionInformeTecnico(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EnviarCorreoAlertaElaboracionInformeTecnicoSemanal/?prcscodi={prcscodi}")]
        ResultadoProceso EnviarCorreoAlertaElaboracionInformeTecnicoSemanal(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NotificacionIncumplimientoIEOD/?prcscodi={prcscodi}")]
        ResultadoProceso NotificacionIncumplimientoIEOD(int prcscodi);

        [OperationContract]
        List<EveInformefallaDTO> ObtenerAlertaInformeFallas();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ProcesoAprobarProgramaDiaro/?prcscodi={prcscodi}")]
        ResultadoProceso ProcesoAprobarProgramaDiaro(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ProcesoAprobarProgramaSemanal/?prcscodi={prcscodi}")]
        ResultadoProceso ProcesoAprobarProgramaSemanal(int prcscodi);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ProcesoGenerarVersiones/?prcscodi={prcscodi}")]
        ResultadoProceso ProcesoGenerarVersiones(int prcscodi);
    }
}