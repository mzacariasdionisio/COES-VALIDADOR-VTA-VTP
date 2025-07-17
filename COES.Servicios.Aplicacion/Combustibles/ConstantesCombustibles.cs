using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Combustibles
{
    /// <summary>
    /// Constantes de aplicativo PR31
    /// </summary>
    public class ConstantesCombustibles
    {
        public const int ModcodiPr31Extranet = 44;
        public const int ModcodiPr31ExtranetGaseoso = 47;

        //web.config
        public const string KeyFlagPR31HoraSistemaManual = "FlagPR31HoraSistemaManual";
        public const string KeyFlagPR31UsarMedidores = "FlagPR31UsarMedidores";
        public const string KeyFlagPR31ValidarCentralExistente = "FlagPR31ValidarCentralExistente";
        public const string KeyFlagPR31HoraSistemaManualMinPosterior = "FlagPR31HoraSistemaManualMinPosterior";
        public const string KeyFlagPR31MinutosAutoguardado = "FlagPR31MinutosAutoguardado";

        public const int PlantcodiNotificacionPr31 = 109;
        public const string RemitenteSgi = "Sub Dirección de Gestión de la Información";
        public const string AnexoSgi = " – Anexo: 621 / 633";
        public const int TipoCorreoNuevo = 1;
        public const int TipoCorreoSubsanacionObs = 2;
        public const int TipoCorreoCancelar = 3;
        public const int TipoCorreoObservacion = 4;
        public const int TipoCorreoAprobado = 5;
        public const int TipoCorreoAmplSubsanarObs = 6;
        public const int TipoCorreoHabilitar106 = 7;
        public const int TipoCorreoCulminacionPlazo = 8;
        public const int TipoCorreoDesaprobado = 9;
        public const int TipoCorreorRecordatorioSolicitudCoes = 10;
        public const int TipoCorreorRecordatorioSubsanacionObsCoes = 11;
        public const int TipoCorreorRecordatorioSubsanacionObsAgente = 12; 

        public const string MetodoPR31RecordatorioCOES = "PR31RecordatorioCOES";
        public const string MetodoPR31RecordatorioAgente = "PR31RecordatorioAgente";
        public const string MetodoPR31CulminacionPlazoAgente = "PR31CulminacionPlazoAgente";

        public const string MetodoPR31GasRecordatorioCOES_E = "PR31GasRecordatorioCOES_E";
        public const string MetodoPR31GasRecordatorioCOES_N = "PR31GasRecordatorioCOES_N";
        public const string MetodoPR31GasCulminacionPlazoAgente_E = "PR31GasCulminacionPlazoAgente_E";
        public const string MetodoPR31GasCulminacionPlazoAgente_N = "PR31GasCulminacionPlazoAgente_N";
        public const string MetodoPR31GasRecordatorioAgente_E = "PR31GasRecordatorioAgente_E";
        public const string MetodoPR31GasRecordatorioAgente_N = "PR31GasRecordatorioAgente_N";
        public const string MetodoPR31GasIncumplimiento_E = "PR31GasIncumplimiento_E";

        //public const para  en control de archivos  Archivos (upload y download)";
        public const string FolderRaizPR31Gaseoso = "Extranet/PR31Gaseoso/";
        public const string SubcarpetaSolicitud = "Solicitud_Agente";

        public const string MensajesFile = "Mensajes\\";
        public const string FolderPR31 = "PR31\\";
        public const string CarpetaEnvio = "Envio";
        public const string CarpetaTemporal = "Temporal";

        public const string FolderPR31Gas = "PR31Gas\\";
        //reportes

        public const string NombreReporteEnvios = "RptEnvios.xlsx";
        public const string NombreReporteFormularioEnvios = "RptFormularioEnvio.xlsx";
        public const string HojaReporteExcel = "REPORTE";
        public const string FolderReporte = "Areas\\Combustibles\\Reporte\\";
        public const int PosRowIni = 6;
        public const int PosColIni = 2;

        //Estado de combustible
        public const int EstcomcodiLiquido = 1;
        public const int EstcomcodiSolido = 2;
        public const int EstcomcodiGas = 3;

        public const int DiasLiquidoRptaSolicitud = 5;
        public const int DiasLiquidoSubsanarObs = 7;

        public const int DiasSolidoRptaSolicitud = 3;
        public const int DiasSolidoSubsanarObs = 30;

        public static List<int> ListadoEstadoPr31Extranet = new List<int>() { 1, 3, 4, };
        public const int EstadoSolicitud = 1;
        public const int EstadoAprobado = 3;
        public const int EstadoDesaprobado = 4; //el coes rechaza
        //public const int EstadoFueraPlazo = 5;
        public const int EstadoObservado = 6; //el coes observa
        public const int EstadoSubsanacionObs = 7; //el agente subsana
        public const int EstadoCancelado = 8;//el agente cancela
        public const int EstadoNotificado = 9;//notificado

        public const string EstadoRegActivo = "A";
        public const string EstadoRegHistorico = "H";
        public const int EstadoArchivoActivo = 1;
        public const int EstadoArchivoInactivo = 0;

        //Parametros de costos base de datos
        public const string TipoPcomb = "1";
        public const string TipoMerm = "2";
        public const string TipoCSegFleM = "3";
        public const string TipoCAduana = "4";
        public const string TipoImp = "5";
        public const string TipoCEmbFleT = "6";
        public const string TipoCTransp = "8";
        public const string TipoTMecan = "9";
        public const string TipoTQuim = "10";
        public const string TipoCFinanc = "7";
        public const string TipoCTotal = "11";

        public const string ParametrosGenerales = "10,18,19,21,249,248,1";
        public const string FormuladatTcambio = "TCambio";
        public const int ConcepcodiTcambio = 1;

        //SECCION BASE DE DATOS
        public const string BDCostoD2 = "521,522,523,524,510";
        public const string BDCostoR6 = "526,527,528,529,511";
        public const string BDCostoR500 = "533,534,535,536,512";
        public const string BDCostoCarbon = "218,219,223,224,193,222,62,231";

        public const int CostoUnitD2 = 521;
        public const int CostoUnitR6 = 526;
        public const int CostoUnitR500 = 533;
        public const int CostoUnitCarbon = 218;

        public const int CTranspD2 = 522;
        public const int CTranspR6 = 527;
        public const int CTranspR500 = 534;
        public const int CTranspTCarb = 224;
        public const int CSegurFletesCarb = 219;

        public const int TMecanD2 = 523;
        public const int TMecanR6 = 528;
        public const int TMecanR500 = 535;
        public const int CAduaDesCarb = 223;

        public const int TQuimD2 = 524;
        public const int TQuimR6 = 529;
        public const int TQuimR500 = 536;

        public const int ImpD2 = 510;
        public const int ImpR6 = 511;
        public const int ImpR500 = 512;
        public const int ImpCarbon = 231;

        //seccion RESULTADOS
        public const string TotalCostoD2 = "521,522,523,524,510,525,513,531";
        public const string TotalCostoR6 = "526,527,528,529,530,511,514,532";
        public const string TotalCostoR500 = "533,534,535,536,537,512,515,538";
        public const string TotalCostoCarbon = "218,235,219,223,231,224,220,221";

        public const int MermaD2 = 513;
        public const int MermaR6 = 514;
        public const int MermaR500 = 515;
        public const int MermaCarbon = 235;

        public const int CFinancD2 = 525;
        public const int CFinancR6 = 530;
        public const int CFinancR500 = 537;
        public const int CFinancCarbon = 220;

        public const int CTotalD2 = 531;
        public const int CTotalR6 = 532;
        public const int CTotalR500 = 538;
        public const int CTotalCarbon = 221;

        //UNIDADES
        public const string SolesXDolar = "S//USD";

        public const string Soles = "S";
        public const string Dolar = "D";
        public const string SolesDesc = "S/";
        public const string DolarDesc = "USD";

        public const string Litro = "l";
        public const string LitroDesc = "litro";
        public const string Kilo = "kg";
        public const string Tonelada = "t";
        public const string ToneladaDesc = "tonelada";

        public const string SolesXLitro = "S//l";
        public const string DolaresXLitro = "USD/l";
        public const string SolesXKilo = "S//kg";
        public const string DolaresXKilo = "USD/kg";
        public const string KjXKilo = "kJ/kg";

        public const string SesionEmpresas = "SesionEmpresas";

        public const string SeparadorHandson = ";";

        public enum Interfaz
        {
            Extranet = 1,
            Intranet = 2,
        }

        public enum Exportacion
        {
            NingunComb = 0,
            SoloCombLiquidos = 1,
            SoloCombSolidos = 2,
            AmbosComb = 3,
        }

        public const string ConcepcodiPrecioComb = "644,645";
        public enum OpcionParametro
        {
            Nuevo = 3,
            Editar = 2
        }

        //Estado de CENTRAL POR FENERG
        public enum EstadoParametro
        {
            Activo = 1,
            Inactivo = 0
        }

        //Item de resultados para Solidos y Liquidos
        public const int ResultadosItemSolido = 9;
        public const int ResultadosItemLiquido = 5;

        #region CCGAS.PR31
        public const string UsuarioSistema = "SISTEMA";

        public const int PorDefecto = -1;

        public const int ConcepcodiCostoCombustibleGaseoso = 689;

        public const int DiasGaseosoSubsanarObs = 3;

        public const string CombustiblesLiquidosYSolidos = "1,2";
        public const string CombustiblesGaseosos = "3";
        public const int EstadoAprobadoParcialmente = 10;
        public const int EstadoSolicitudAsignacion = 11;
        public const int EstadoAsignado = 12;
        public const int EstadoUsuAmpliado = 13;
        public const int EstadoUsuIncumplimiento_CE = 14;
        public const int EstadoUsuFinPlazoSubs = 15;
        public const int EstadoUsuRecorInfoRecibida = 16;
        public const int EstadoUsuRecorAgentePlazoSubsanar = 17;
        public const int EstadoUsuRecorCoesRevisarSubs = 18;

        public const string CCombcodisGaseosos = "168,169,170,171";
        public const int CCombcodiPrecioUSuministro = 168;
        public const int CCombcodiPrecioUTransporte = 169;
        public const int CCombcodiPrecioUDistribucion = 170;
        public const int CCombcodiCostoGasNatural = 171;
        public const int CCombcodiCostoCombustibleGaseoso = 192;
        public const int CCombcodiEmpresaGeneradora = 115;
        public const int CCombcodiNombreCentral = 116;
        public const int CCombcodiTipoComb = 117;
        public const int CCombcodiPCSs = 119;
        public const int CCombcodiPCSt = 120;
        public const int CCombcodiPCSd = 121;
        public const int CCombcodiPCSdg = 122;
        public const int CCombcodiFechaSuministro = 118;
        public const int CCombcodiNumColSeccion2 = 177;
        public const int CCombcodiNumColSeccion3 = 182;
        public const int CCombcodiNumColSeccion4 = 187;
        public const int CCombcodiTipoOpcionSeccion2= 193;
        public const int CCombcodiTipoOpcionSeccion3 = 194;
        public const int CCombcodiTipoOpcionSeccion4 = 195;
        public const int CCombcodiUltMesSeccion2 = 200;
        public const int CCombcodiUltMesSeccion3 = 201;
        public const int CCombcodiUltMesSeccion4 = 202;

        public const int CCombcodiInfSust = 118; //196
        public const int CCombcodiInfSustObs = 197;
        public const int CCombcodiInfSustSub = 198;
        public const int CCombcodiInfSustRpta = 199;

        public const int TipoCorreoAprobadoParcial = 13;
        public const int TipoCorreoAsignado = 14;
        public const int TipoCorreoIncumplimientoInfo_CE = 15;

        public const int EnvioPorIncumplimiento = -100;
        
        public const int TipoPlantillaNotificacion = 1;
        public const int TipoPlantillaRecordatorio = 2;

        public const string CentralExistente = "E";
        public const string CentralNueva = "N";
        public const string OpcionNuevaFormato3 = "F3";
        public const string OpcionNuevaSolicitoAsignacion = "SA";

        public const int EstadoCeldaEditable = 1;
        public const int EstadoCeldaSoloLectura = 0;
        public const int EstadoCeldaNoEditable = -1;
        public const int EstadoHandsonReadonly = -2;
        public const string TextoNoAplica = "No Aplica";
        public const int NumDecFormulaDefault = 4;
        public const string TextoConfidencial = "(*)";

        public const string CodigosPlantillaNotificacionExistentes = "133,135,137,139,141,143,145,147,149,152";
        public const string CodigosPlantillaNotificacionNuevas = "134,136,138,140,142,144,146,148,150,151,153";
        public const string CodigosPlantillaRecordatorioExistentes = "156,158,160";
        public const string CodigosPlantillaRecordatorioNuevas = "157,159,161";
       
        //variables similares al de configuracionPlantilla.js
        public const int VariableAsunto = 0;
        public const int VariableContenido = 1;
        public const int VariableCC = 2;
        public const int VariablePara = 3;

        public const int EsNotificacion = 1;
        public const int EsRecordatorio = 2;

        public const int NotificacionIncumplimientoEntregaInformacionFormato3_CE = 133;
        public const int NotificacionIncumplimientoEntregaInformacionFormato3_CN = 134;
        public const int NotificacionRegistroSolicitud_CE = 135;
        public const int NotificacionRegistroSolicitud_CN = 136;
        public const int NotificacionCancelarEnvio_CE = 137;
        public const int NotificacionCancelarEnvio_CN = 138;
        public const int NotificacionObservacionEnvío_CE = 139;
        public const int NotificacionObservacionEnvío_CN = 140;
        public const int NotificacionSubsanacionEnvío_CE = 141;
        public const int NotificacionSubsanacionEnvío_CN = 142;
        public const int NotificacionCulminacionPlazoSubsanacion_CE = 143;
        public const int NotificacionCulminacionPlazoSubsanacion_CN = 144;
        public const int NotificacionAprobacionEnvío_CE = 145;
        public const int NotificacionAprobacionEnvío_CN = 146;
        public const int NotificacionAprobacionParcialEnvío_CE = 147;
        public const int NotificacionAprobacionParcialEnvío_CN = 148;
        public const int NotificacionDesaprobacionEnvío_CE = 149;
        public const int NotificacionDesaprobacionEnvío_CN = 150;
        public const int NotificacionAsignacionEnvío_CN = 151;
        public const int NotificacionAmpliacionPlazoEnvio_CE = 152;
        public const int NotificacionAmpliacionPlazoEnvio_CN = 153;

        public const int RecordatorioRevisarEvaluarInformaciónRecibida_CE = 156;
        public const int RecordatorioRevisarEvaluarInformaciónRecibida_CN = 157;
        public const int RecordatorioInformarVencimientoPlazoSubsanacion_CE = 158;
        public const int RecordatorioInformarVencimientoPlazoSubsanacion_CN = 159;
        public const int RecordatorioRevisarEvaluarSubsanacionPresentadas_CE = 160;
        public const int RecordatorioRevisarEvaluarSubsanacionPresentadas_CN = 161;

        public const int ParametroRevisarEvaluarInformaciónRecibida_CE = 33;
        public const int ParametroRevisarEvaluarInformaciónRecibida_CN = 34;
        public const int ParametroInformarVencimientoPlazoSubsanacion_CE = 31;
        public const int ParametroInformarVencimientoPlazoSubsanacion_CN = 32;
        public const int ParametroRevisarEvaluarSubsanacionPresentadas_CE = 35;
        public const int ParametroRevisarEvaluarSubsanacionPresentadas_CN = 36;
        

        public const string ValMesActual = "{MES_ACTUAL}";
        public const string DscMesActual = "Mes Actual";
        public const string ValMesActualOSiguiente = "{MES_ACTUAL_O_SIGUIENTE}";
        public const string DscMesActualOSiguiente = "Mes Actual o Siguiente";
        public const string ValMesSiguiente = "{MES_SIGUIENTE}";
        public const string DscMesSiguiente = "Mes Siguiente";
        public const string ValAnio = "{ANIO}";
        public const string DscAnio = "Anio";
        public const string ValListaCentrales = "{LISTA_CENTRALES}";
        public const string DscListaCentrales = "Lista de Centrales Totales";
        public const string ValDestinatariosPara = "{DESTINATARIO}";
        public const string DscDestinatariosPara = "Destinatario";
        public const string ValEmpresa = "{EMPRESA}";
        public const string DscEmpresa = "Empresa";
        public const string ValMesVigencia = "{MES_VIGENCIA}";
        public const string DscMesVigencia = "Mes de Vigencia";
        public const string ValCostoCombAsignado = "{COSTO_COMB_ASIGNADO}";
        public const string DscCostoCombAsignado = "Costo de Combustible Asignado";
        public const string ValIdEnvio = "{ID_ENVIO}";
        public const string DscIdEnvio = "Código de envío";
        public const string ValTipoCombustible = "{TIPO_COMBUSTIBLE}";
        public const string DscTipoCombustible = "Tipo de Combustible";
        public const string ValFechaSolicitud = "{FECHA_SOLICITUD}";
        public const string DscFechaSolicitud = "Fecha de Solicitud";
        public const string ValEstado = "{ESTADO}";
        public const string DscEstado = "Estado";
        public const string ValFechaCancelacion = "{FECHA_CANCELACION}";
        public const string DscFechaCancelacion = "Fecha de Cancelacion";        
        public const string ValFechaRevision = "{FECHA_REVISION}";
        public const string DscFechaRevision = "Fecha de Revision";
        public const string ValFechaMaxRpta = "{FECHA_MAX_RESPUESTA}";
        public const string DscFechaMaxRpta = "Fecha maxima de respuesta";
        public const string ValFechaSubsanacion = "{FECHA_DE_SUBSANACION}";
        public const string DscFechaSubsanacion = "Fecha de Subsanacion";
        public const string ValFechaPlazoMaxSubsanacion = "{FECHA_PLAZO_MAX_DE_SUBSANACION}";
        public const string DscFechaPlazoMaxSubsanacion = "Fecha Plazo maximo de subsanacion";
        public const string ValFechaAprobacion = "{FECHA_DE_APROBACION}";
        public const string DscFechaAprobacion = "Fecha de Aprobacion";
        public const string ValFechaDesaprobacion = "{FECHA_DE_DESAPROBACION}";
        public const string DscFechaDesaprobacion = "Fecha de Desaprobacion";
        public const string ValRespuestaCoes = "{RESPUESTA_COES}";
        public const string DscRespuestaCoes = "Respuesta de COES";
        public const string ValFechaAsignacion = "{FECHA_DE_ASIGNACION}";
        public const string DscFechaAsignacion = "Fecha de Asignacion";
        public const string ValFechaInicioPlazo = "{FECHA_INICIO_DE_PLAZO}";
        public const string DscFechaInicioPlazo = "Fecha de inicio de plazo";
        public const string ValFechaFinPlazo = "{FECHA_FIN_DE_PLAZO}";
        public const string DscFechaFinPlazo = "Fecha de fin de plazo";
        public const string ValListaCentralesAprobadas = "{LISTA_CENTRALES_APROBADAS}";
        public const string DscListaCentralesAprobadas = "Lista de Centrales Aprobadas";
        public const string ValListaCentralesDesaprobadas = "{LISTA_CENTRALES_DESAPROBADAS}";
        public const string DscListaCentralesDesaprobadas = "Lista de Centrales Desaprobadas";
        public const string ValOtrosAgentes = "{OTROS_AGENTES_DE_EMPRESA}";
        public const string DscOtrosAgentes = "Otros agentes de la empresa";
        public const string ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}";
        public const string DscAgenteUltimo = "Agente que realizo el ultimo evento";
        public const string ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}";
        public const string DscTodosAgentes = "Todos los agentes de la empresa";
        public const string ValHorasCulminacion = "{VALOR_DE_HORAS_CULMINACION}";//"{PLAZO_CULMINACION_EN_HORAS}"
        public const string DscHorasCulminacion = "Valor de Horas Culminacion";
        public const string ValDiasRecepcion = "{VALOR_DE_DIAS_RECEPCION}";//"{DIAS_DE_RECEPCION}"
        public const string DscDiasRecepcion = "Valor de Dias Recepcion";

        //Alerta automática
        public const int PrcscodiRecordatorioRevisarEvaluarInformaciónRecibida_CE = 37;
        public const int PrcscodiRecordatorioRevisarEvaluarInformaciónRecibida_CN = 38;
        public const int PrcscodiRecordatorioInformarVencimientoPlazoSubsanacion_CE = 39;
        public const int PrcscodiRecordatorioInformarVencimientoPlazoSubsanacion_CN = 40;
        public const int PrcscodiRecordatorioRevisarEvaluarSubsanacionPresentadas_CE = 41;
        public const int PrcscodiRecordatorioRevisarEvaluarSubsanacionPresentadas_CN = 42;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_CE = 44;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_CN = 45;
        public const int PrcscodiNotificacionIncumplimientoEntregaInformacionFormato3_CE = 46;

        #region Iteracion 2
        public const int AccionGuardar = 1;
        public const int AccionActualizar = 2;

        public const int OrigenCRCalculo = 1;
        public const int OrigenCRAsignacion = 2;
        public const int OrigenCRCopiado = 3;

        public const int ConceptocodiCostoCombustible = 191;
        public const int ConceptocodiPCI_CCGas = 561;

        public const int EnvioTemporalPrevioConErrorConexion = 0;

        public const int TipoExistente = 1;
        public const int TipoAsignado = 2;
        public const int TipoIncumplimiento = 3;


        public const int PlantillaCorreosEnviados = 162;
        public const string SubcarpetaArchivoAdjuntado = "EnvioCorreo";

        public const int GuardadoTemporal = 1;
        public const int GuardadoOficial = 2;

        public const string EstadoDesuso = "X";
        public const string EstadoConError = "E";

        public const int ReporteCV_S = 1;
        public const int ReporteCV_USD = 2;
        public const int ReporteCCG_PCI = 3;
        public const int ReportePU_CG_PCI = 4;
        public const int ReportePU_CG_F3 = 5;
        public const int ReportePC = 6;

        public const int ArchivosTotales = 1;
        public const int ArchivosNoConfidencial = 2;
        public const int ArchivosConfidencial = 3;

        //public const string ModuloArchivosXEnvio = "formato3EInformesSustentatorios";
        public const string ModuloArchivosXEnvio = "F3";
        public const string NombreArchivosZip = "Archivos";

        //public const string ModuloArchivosF3IS = "F3IS";
        public const string ModuloArchivosParticipanteGen = "ArchivosXEnvios";

        public const string EstadoCentralAprobado = "A";

        //public const string ConcepcodiReporteCV = "119,120,121,122,168,169,170";
        public const int Concepcodi15PCSSuministro = 119;
        public const int Concepcodi16PCSTransporte = 120;
        public const int Concepcodi17PCSDistribucion = 121;
        public const int Concepcodi18PCSGncDistribucion = 122;

        public const int Concepcodi46PCSGncDistribucion = 157;
        public const int Concepcodi413PCSGncDistribucion = 164;
        public const int Concepcodi48PCSGncDistribucion = 159;

        public const int Concepcodi51PUSuministro = 168;
        public const int Concepcodi52PUTransporte = 169;
        public const int Concepcodi53PUDistribucion = 170;

        public const int ConceptocodiPCI = 192;

        public const int TipoReporteCVCuadro1 = 1;
        public const int TipoReporteCVCuadro2 = 2;
        public const int TipoReporteCVCuadro3 = 3;
        public const int TipoReporteCVCVC = 4;

        public const int Cuadro_1_CentralTermoeléctrica = 1;
        public const int Cuadro_1_TituloPrecioUnitario = 2;
        public const int Cuadro_1_PUSuministro = 3;
        public const int Cuadro_1_PUTransporte = 4;
        public const int Cuadro_1_PUDistribución = 5;
        public const int Cuadro_1_UnidadPUSuministro = 6;
        public const int Cuadro_1_UnidadPUTransporte = 7;
        public const int Cuadro_1_UnidadPUDistribución = 8;

        public const int Cuadro_2_CentralTermoeléctrica = 9;
        public const int Cuadro_2_TituloPCS = 10;
        public const int Cuadro_2_TituloPCI = 11;
        public const int Cuadro_2_PCSSuministro = 12;
        public const int Cuadro_2_PCSTransporte = 13;
        public const int Cuadro_2_PCSDistribución = 14;
        public const int Cuadro_2_PCISuministroTransporteDistribución = 15;
        public const int Cuadro_2_UnidadPCSSuministro = 16;
        public const int Cuadro_2_UnidadPCSTransporte = 17;
        public const int Cuadro_2_UnidadPCSDistribución = 18;
        public const int Cuadro_2_UnidadPCISuministroTransporteDistribución = 19;

        public const int Cuadro_3_CentralTermoeléctrica = 20;
        public const int Cuadro_3_TituloPrecioUnitario = 21;
        public const int Cuadro_3_PUSuministro = 22;
        public const int Cuadro_3_PUTransporte = 23;
        public const int Cuadro_3_PUDistribución = 24;
        public const int Cuadro_3_TituloCostoCombustible = 25;
        public const int Cuadro_3_UnidadPUSuministro = 26;
        public const int Cuadro_3_UnidadPUTransporte = 27;
        public const int Cuadro_3_UnidadPUDistribución = 28;
        public const int Cuadro_3_UnidadCostoCombustible = 29;

        #endregion

        #endregion
    }

    public class HandsonCombustible
    {
        public int Cbenvcodi { get; set; }
        public int Estcomcodi { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Estenvcodi { get; set; }

        public string Observacion { get; set; }
        public string Fecfinrptasolicitud { get; set; }
        public string Fecfinsubsanarobs { get; set; }
        public string Fecfinampliacion { get; set; }
        public string FecCostoVigenteDesde { get; set; }

        public string Unidad { get; set; }
        public string UnidadDesc { get; set; }
        //public decimal Tipocambio { get; set; }
        public decimal StockFinalDeclarado { get; set; }

        public bool Readonly { get; set; }
        public bool Editable { get; set; }
        public bool EsExtranet { get; set; }
        public bool EsEditableItem106 { get; set; }
        public bool HabilitarDesaprobar { get; set; }

        public List<SeccionCombustible> ListaSeccion { get; set; } = new List<SeccionCombustible>();
        public List<SeccionCombustible> ListaSeccionDocumento { get; set; } = new List<SeccionCombustible>();
        public List<CeldaErrorCombustible> ListaErrores { get; set; } = new List<CeldaErrorCombustible>();

        public int PosRowIni { get; set; }
        public int PosColIni { get; set; }

        public string RangoIniValidoRecepcion { get; set; }
        public string RangoFinValidoRecepcion { get; set; }

    }

    public class SeccionCombustible
    {
        public CbConceptocombDTO Seccion { get; set; }
        public List<CbConceptocombDTO> ListaItem { get; set; } = new List<CbConceptocombDTO>();
        public List<CbArchivoenvioDTO> ListaArchivo { get; set; } = new List<CbArchivoenvioDTO>();
        public CbObsDTO[] ListaObs { get; set; }
        public bool EsSeccionSoloLectura { get; set; }
    }

    public class CeldaErrorCombustible
    {
        public string Tab { get; set; } = string.Empty;
        public string Celda { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public int ColorError { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int TipoError { get; set; }
    }

    #region CCGAS.PR31    
    public class VariableCorreo
    {
        public string Valor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class PR31FormGasCentral
    {
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public string Cbcentestado { get; set; }

        public string Central { get; set; }
        public string TipoCentral { get; set; }
        public string NameSheet { get; set; }

        public string MesVigencia { get; set; }
        public string Emprnomb { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }

        public int FilaIni { get; set; }
        public HandsonModel Handson { get; set; }
        public List<CeldaErrorCombustible> ListaErrores { get; set; } = new List<CeldaErrorCombustible>();
        public bool EsEditable { get; set; }
        public bool EsEditableObs { get; set; }
        public bool IncluirObservacion { get; set; }

        public bool FlagConsultoEnergiaMesAnterior { get; set; }
        public decimal EnergiaMesAnterior { get; set; }

        public int Estenvcodi { get; set; }

        public int Cbftcodi { get; set; }
        public CbFichaItemDTO[] ArrayItem { get; set; }
        public CbFichaItemDTO[] ArrayItemObs { get; set; }
        public int NumMaxColData { get; set; }
    }

    public class PR31FormGasSustento
    {
        public int FilaIni { get; set; }
        public HandsonModel Handson { get; set; }
        public bool Readonly { get; set; }
        public bool EsEditable { get; set; }
        public bool EsEditableObs { get; set; }
        public bool IncluirObservacion { get; set; }
        public int Estenvcodi { get; set; }

        public SeccionCombustible SeccionCombustible { get; set; } = new SeccionCombustible();
    }

    #region Iteracion_2

    public class ElementosXCentral
    {
        public int codEnvio { get; set; }

        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaDesc { get; set; }
        public string TipoCombustible { get; set; }

        //parametros
        public decimal? CCombGas_SI { get; set; }
        public decimal? Cvnc { get; set; }
        public decimal? Cvc { get; set; }
        public decimal? Cv { get; set; }

        public List<decimal?> LstCCombGas_SI { get; set; }

        public decimal? CCombGas_SIPorcentaje { get; set; }
        public decimal? CvncPorcentaje { get; set; }
        public decimal? CvcPorcentaje { get; set; }
        public decimal? CvPorcentaje { get; set; }

        public List<ItemXmes> ListaItems { get; set; }
        public decimal? SuministroPorcentaje { get; set; }
        public decimal? TransportePorcentaje { get; set; }
        public decimal? DistribucionPorcentaje { get; set; }
        public decimal? SumTransDistPorcentaje { get; set; }

        public string FormulaCCombGas_SI { get; set; }
        public string FormulaCvnc { get; set; }
        public string FormulaCvc { get; set; }
        public string FormulaCv { get; set; }
    }

    public class ItemXmes
    {
        public decimal? Suministro { get; set; }
        public decimal? Transporte { get; set; }
        public decimal? Distribucion { get; set; }
        public decimal? SumTransDist { get; set; }
    }

    public class CentralC3
    {
        public int Cbcentcodi { get; set; }
        public int TipoDato { get; set; }
        public int? Orden { get; set; }
        public int UsadoEnLaExtranet { get; set; }
    }

    public class CumplimientoEmpresa
    {
        public int Emprcodi { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string TipoCentral { get; set; }
        public string TipoCombustible { get; set; }
        public string MesVigencia { get; set; }
        public int IDEnvio { get; set; }
        public DateTime Cbenvfechaperiodo { get; set; }
        public List<CumplimientoAccion> ListaAcciones { get; set; }
    }

    public class CumplimientoAccion
    {
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public string Usuario { get; set; }
        public string FechaHora { get; set; }
        public string Condicion { get; set; }
    }

    public class GrupoDato
    {
        public string Valor { get; set; }
        public int Cbcentcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Conceptocodi { get; set; }
        public int CComcodiTemporal { get; set; }
    }

    #endregion

    #endregion
}
