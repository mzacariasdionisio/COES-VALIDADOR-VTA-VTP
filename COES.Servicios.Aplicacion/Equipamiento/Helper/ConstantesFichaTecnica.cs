using System.Configuration;

namespace COES.Servicios.Aplicacion.Equipamiento.Helper
{
    public class ConstantesFichaTecnica
    {
        public const int TipoRaiz = -1;
        public const int TipoAgrupamiento = 1;
        public const int TipoPropiedad = 0;

        public const string OrientacionVertical = "V";
        public const string OrientacionHorizontal = "H";

        public const int EstadoActivo = 1;
        public const int EstadoInactivo = 0;

        public const int OrigenTipoNinguno = 0;
        public const int OrigenTipoEquipo = 1;
        public const int OrigenCategoriaGrupo = 2;
        public const int OrigenFichaTecnica = 3;
        public const string OrigenTipoNingunoDesc = "";
        public const string OrigenTipoEquipoDesc = "Tipo de Equipo";
        public const string OrigenCategoriaGrupoDesc = "Categoría de Grupo";
        public const string TipoEquipoPropiedadDesc = "Propiedad de Equipo";
        public const string TipoConceptoDesc = "Concepto de Grupo";
        public const string TipoFichaPropiedadDesc = "Propiedad de Ficha Técnica";

        public const int FichaMaestraPrincipal = 1;
        public const int FichaMaestraNoPrincipal = 0;

        public const int PropEmpresa = 1;
        public const int PropCentral = 2;
        public const int PropNombreEquipo = 3;
        public const int PropInicioOpComercialEquipo = 4;
        public const int PropCodigoEquipo = 5;
        public const int PropNumModosOp = 6;
        public const int PropNombreModoOp = 7;
        public const int PropCodigoModoOp = 8;
        public const int PropInicioOpComercialModoOp = 9;

        public const string ColorCategoriaGrupo = "#69c3ff";
        public const string ColorCategoriaCentral = "#fffd8d";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_usuario_Ficha_Técnica.rar";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizFichaTecnicaModuloManual = "Ficha Técnica\\";

        //Excel
        public const string NombreArchivoMasivo = "Reporte_FichaTécnica_";
        public const string NombreArchivo = "FichaTécnica_";
        public const string FormatoFechaHoraExcel = "yyyy-MM-dd_HH-mm";

        public const string ColorIntranetAgrupRaiz = "#92CDDC";
        public const string ColorIntranetAgrupHijo = "#f9ede4";
        public const string ColorPortalAgrupRaiz = "#ffff00";
        public const string ColorPortalAgrupHijo = "#FFFFFF";
        public const string ColorOrdenAgrup = "#00B0F0";
        public const string ColorTextoAgrup = "#000000";

        //Categorias
        public const int CatecodiCentralHidraulica = 6;
        public const int CatecodiGrupoHidraulico = 5;

        public const int RolAdministradorFichaTecnica = 280;
        public const int RolUsuarioExtranetFichaTecnica = 296;

        public const int ValorSi = 1;
        public const int ValorNo = 0;

        //constantes para fileServer
        public const string FolderRaizFichaTecnica = "FichaTecnica/";
        public const string Plantilla = "Plantilla\\";

        public const string HojaPlantillaExcel = "PLANTILLA";
        public const string NombrePlantillaExcelParametros = "Plantilla_Parametros_GruposMop.xlsx";
        public const string RutaReportes = "Areas/Equipamiento/Reporte/";

        // Constante para el repositorio CSV
        public const string AppCSV = "application/CSV";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string SeparadorCampo = ",";
        public const string SeparadorCampoCSV = ";";

        //Extranet
        public const string FolderRaizExtranetFT = "Extranet/FichaTecnicaEtapa2/";
        public const string SubcarpetaSolicitud = "Solicitud_Agente";
        public const string SubcarpetaNoConfidencial = "Archivo_Aprobado_NoConfidencial";
        public const string SubcarpetaConfidencial = "Archivo_Aprobado_Confidencial";
        public const string MensajesFile = "Mensajes\\";
        public const string CarpetaEnvio = "Envio";
        public const string CarpetaTemporal = "Temporal";
        public const string SubcarpetaArchivoAdjuntado = "EnvioCorreo";

        public const string ModuloArchivosSustentoXEnvio = "sustentos"; //sustentos
        public const string ModuloArchivosValorXEnvido = "valor"; // archivos valor
        public const string NombreArchivosZip = "Archivos";

        public const int EsNotificacion = 1;
        public const int EsRecordatorio = 2;

        public const string UsuarioSistema = "SISTEMA";

        public const string ColorVerdeClaro = "#CBFBE6";
        public const int AccionNuevo = 1;
        public const int AccionEditar = 2;
        public const int AccionEliminar = 3;
        public const int AccionVer = 4;

        public const int AccionGuardar = 1;
        public const int AccionActualizar = 2;

        public const string EstadoStrActivo = "A";
        public const string EstadoStrEliminado = "X";
        public const string EstadoStrBaja = "B";
        public const string EstadoStrProyecto = "P";
        public const string EstadoStrVigente = "V";

        //autoguardado
        public const int GuardadoOficial = 1;
        public const int GuardadoTemporal = 2;
        public const int GuardadoLogRespaldo = 3;
        //public const int NoGuardadoExtranet = 4;
        public const int GuardadoTemporalFTVigente = 5;
        public const int OperacionExistosa = 1;
        public const int OperacionConError = 0;
        public const string RealizadoPorSistema = "S";
        public const string RealizadoPorManual = "M";
        public const int ConexionSiCOES = 1;
        public const int ConexionNoCOES = 0;
        public const int OpcionVisualFormActualBD = 10;
        public const int OpcionVisualFormLimpiar = 11;
        public const int OpcionVisualFormCambioVersion = 12;

        public const int CasoEspecial1 = 1;
        public const int CasoEspecial2 = 2;

        public const string EstadoDesuso = "X";
        public const string EstadoConError = "E";
        public const int EquiposModificadosFT = 1;

        //etapas
        public const int EtapaConexion = 1;
        public const int EtapaIntegracion = 2;
        public const int EtapaOperacionComercial = 3;
        public const int EtapaModificacion = 4;

        //formatos de carga Extranet
        public const int FormatoConexIntegModif = 1;
        public const int FormatoOperacionComercial = 2;
        public const int FormatoBajaModoOperacion = 3;

        public const int PorDefecto = -1;

        public const int EstadoSolicitud = 1;
        public const int EstadoAprobado = 3;
        public const int EstadoDesaprobado = 4; //el coes rechaza        
        public const int EstadoObservado = 6; //el coes observa
        public const int EstadoSubsanacionObs = 7; //el agente subsana
        public const int EstadoCancelado = 8;//el agente cancela        
        public const int EstadoNotificado = 9;//notificado
        public const int EstadoAprobadoParcialmente = 10;

        //public const int TipoCorreoNuevo = 1;
        //public const int TipoCorreoSubsanacionObs = 2;
        //public const int TipoCorreoCancelar = 3;
        public const int TipoCorreoNotificacionAutomatica = 4;

        public const int TipoArchivoAgenteValorDato = 1;
        public const int TipoArchivoAgenteSustentoDato = 2;
        public const int TipoArchivoAgenteRequisito = 3;
        public const int TipoArchivoAgenteModo = 4;

        public const int TipoArchivoRevisionObsCOES = 5;
        public const int TipoArchivoRevisionRptaAgente = 6;
        public const int TipoArchivoRevisionRptaCOES = 7;

        public const int TipoArchivoRevAreaSolicitado = 8;
        public const int TipoArchivoRevAreaSubsanado = 9;

        public const string STipoArchivoValorDato = "A_VD";
        public const string STipoArchivoSustentoDato = "A_SD";
        public const string STipoArchivoRequisito = "A_R";

        public const string STipoArchivoRevisionObsCOES = "REV_OC";
        public const string STipoArchivoRevisionRptaAgente = "REV_RA";
        public const string STipoArchivoRevisionRptaCOES = "REV_RC";

        //public const string STipoArchivoAreaRevision = "AAR";
        public const string STipoArchivoAreaRevision = "REV_AREA";

        public const string VAR_CODIGO_ENVIO = "{CODIGO_ENVIO}";  //
        public const string VAR_NOMBRE_EMPRESA_ENVIO = "{NOMBRE_EMPRESA_ENVIO}"; //
        public const string VAR_NOMBRE_PROYECTO = "{NOMBRE_PROYECTO}"; //
        public const string VAR_NOMBRE_EQUIPO_PROYECTO = "{NOMBRE_EQUIPO_PROYECTO}"; //
        public const string VAR_NOMBRE_ETAPA = "{NOMBRE_ETAPA}"; //
        public const string VAR_FECHA_SOLICITUD = "{FECHA_SOLICITUD}"; //
        public const string VAR_FECHA_CANCELACION = "{FECHA_CANCELACION}"; //
        public const string VAR_CORREO_USUARIO_SOLICITUD = "{CORREO_USUARIO_SOLICITUD}"; //
        public const string VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT}"; //
        public const string VAR_NOMBRE_EQUIPOS = "{NOMBRE_EQUIPOS}"; //

        public const string VAR_FECHA_REVISION = "{FECHA_REVISION}";//
        public const string VAR_FECHA_MAX_RPTA = "{FECHA_MAXIMA_RESPUESTA}";//
        public const string VAR_EQUIPOS_MODIFICADOS = "{EQUIPOS_MODIFICADOS}";//
        public const string VAR_CORREO_USUARIO_ULTIMO_EVENTO = "{CORREO_USUARIO_ULTIMO_EVENTO}";//
        public const string VAR_CORREOS_CC_AGENTES = "{CORREOS_CC_AGENTES}";//
        public const string VAR_FECHA_DENEGACION = "{FECHA_DENEGACION}";//
        public const string VAR_MENSAJE_AL_AGENTE = "{MENSAJE_AL_AGENTE}";//
        public const string VAR_FECHA_APROBACION = "{FECHA_APROBACION}";//

        public const string VAR_FECHA_SUBSANACION_OBS = "{FECHA_SUBSANACION_OBS}";//

        public const string VAR_FECHA_INICIO_DE_PLAZO = "{FECHA_INICIO_DE_PLAZO}";//
        public const string VAR_FECHA_FINAL_DE_PLAZO = "{FECHA_FINAL_DE_PLAZO}";//

        public const string VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO}";//
        public const string VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO}";//
        public const string VAR_FECHA_APROBACION_PARCIAL = "{FECHA_APROBACION_PARCIAL}";//
        public const string VAR_CORREO_USUARIO_LT_OTRO_AGENTE = "{CORREO_USUARIO_LT_OTRO_AGENTE}"; //

        public const string VAR_MODO_OPERACION_BAJA = "{MODO_OPERACION_BAJA}"; //

        public const string VAR_NUMERO_DIAS_RECEPCION_SOLICITUD = "{NUMERO_DIAS_RECEPCION_SOLICITUD}"; //
        public const string VAR_NUMERO_DIAS_RECEPCION_SUBSANACION = "{NUMERO_DIAS_RECEPCION_SUBSANACION}"; //

        public const string VAR_CORREOS_ADMIN_FT = "{CORREOS_ADMIN_FT}"; //
        public const string VAR_CORREOS_AREAS_COES_SOLICITUD = "{CORREOS_AREAS_COES_SOLICITUD}";//
        public const string VAR_FECHA_MAX_RPTA_DERIVACION = "{FECHA_MAXIMA_RESPUESTA_DERIVACION}";//
        public const string VAR_CORREOS_AREAS_COES_SUBSANADO = "{CORREOS_AREAS_COES_SUBSANADO}";//

        public const string VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION = "{CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION}";//
        public const string VAR_NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION = "{NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION}";//
        public const string VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS = "{NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS}";//

        public const string VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION = "{CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION}";//
        public const string VAR_NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION = "{NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION}";//

        public const string VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR = "{CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR}";//
        public const string VAR_NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR = "{NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR}";//

        public const string VAR_FECHA_CARTA_SOLICITUD = "{FECHA_CARTA_SOLICITUD}";//
        public const string VAR_FECHA_CARTA_SUBSANACION = "{FECHA_CARTA_SUBSANACION}";//

        public const string Si = "S";
        public const string No = "N";

        //ambientes
        public const int INTRANET = 1;
        public const int EXTRANET = 2;
        public const int PORTAL = 3;

        //proceso de revision
        public const string OpcionConforme = "OK";
        public const string OpcionNoSubsanado = "NS";
        public const string OpcionSubsanado = "S";
        public const string OpcionObservado = "O";
        public const string OpcionDenegado = "D";

        public const string ColorRojo = "#F90505";
        public const string ColorAzul = "#0544F9";
        public const string ColorVerde = "#2BA205";
        public const string ColorNaranja = "#FC9D02";
        public const string ColorVioleta = "#1502FC";
        public const int FamiliaCentralTermica = 5;

        public const string ColorValorNoEditable = "#D3D3D3";
        public const string ColorBloqueado = "#F9EDE4";

        //asignación area a baja modo operación
        public const int EventoBajaMO = -1; // evento Modo Operacíón
        public const int RequisitoBajaMO = -1; // requisito Modo Operacíón

        public const int TipoOcultoPortal = 1;
        public const int TipoOcultoExtranet = 2;
        public const int TipoOcultoIntranet = 3;
        public const string EstadoValidos = "A,P,F";

        public const string TipoProgramaDiario = "D";
        public const string TipoProgramaSemanal = "S";
        public const string TipoProgramaTodos = "-1";
        public const string TipoProgramaDiarioDesc = "Diario";
        public const string TipoProgramaSemanalDesc = "Semanal";

        public const string KeyFlagFTHoraSistemaManual = "FlagFTHoraSistemaManual";
        public const string KeyFlagFTHoraSistemaManualMinPosterior = "FlagFTHoraSistemaManualMinPosterior";
        public const string KeyFlagFTMinutosAutoguardado = "FlagFTMinutosAutoguardado";

        public const int TipoParametrosModificadosAprobados = 1;
        public const int TipoParametrosModificadosDenegados = 2;

        //fileapp
        public const string KeyUrlIntranet = "UrlIntranet";
        public const string KeyUrlExtranet = "UrlExtranet";
        public const string KeyUrlFileAppFichaTecnica = "UrlFileAppFichaTecnica";
        public const string KeyFileServerFileAppFichaTecnica = "FileServerFileAppFichaTecnica";
        public const string KeyFileServerExtranetppFichaTecnica = "FileServerExtranetFichaTecnica";
        public const string ClaveOcultaPermiteDescargaConfidencial = "B3dyEzqwle4ff4a";

        //constantes para fileServer Intranet Corporativa
        public const string KeyFileServerIntranetCorporativa = "FileServerIntranetCorporativa";
        public const string FolderDirectorio = "Directorio\\";
        public const string FotoThumbnail = "THUMB_";
        public const string ImagenDefectoDirectorioThumbnail = "thumb_persona.png";

        public const int FichaMaestraPortal = 1;
        public const int FichaMaestraExtranet = 2;
        public const int FichaMaestraIntranet = 3;

        public const string FichaMaestraPortalDesc = "Portal Web FT";
        public const string FichaMaestraExtranetDesc = "Extranet FT";
        public const string FichaMaestraIntranetDesc = "Total FT (Intranet)";

        public const int IdAreaAdminFT = 1;

        public const string SesionRelacionEmpresaCorreo = "SesionRelacionEmpresaCorreo";

        public const string EstadoStrPendiente = "P";
        public const string EstadoStrAtendido = "A";
        public const int EstadoPendiente = 1;
        public const int EstadoAtendido = 2;

        public const int TipoCheckComentario = 1;
        public const int TipoCheckSustento = 2;
        public const int TipoCheckFecha = 3;
        public const string ArchivoConfidencialSinPermiso = "ArchivoConfidencial.txt";
        public const string ArchivoNoDisponible = "ArchivoNoDisponible.txt";

        public const string CondicionEnPlazo = "P";
        public const string CondicionFueraPlazo = "F";
        public const string CondicionNoAtendido = "NA";

        public const int PlazoDiasRevisarSolicitudConexion = 5;
        public const int PlazoDiasRevisarSolicitudIntegracion = 10;
        public const int PlazoDiasRevisarSolicitudOperacionComercial = 10;
        public const int PlazoDiasRevisarSolicitudModificacion = 15;

        public const int PlazoDiasRevisarSubsanadoConexion = 5;
        public const int PlazoDiasRevisarSubsanadoIntegracion = 5;
        public const int PlazoDiasRevisarSubsanadoOperacionComercial = 5;
        public const int PlazoDiasRevisarSubsanadoModificacion = 15;

        public const int ModuloVisualizacion = 1;
        public const int ModuloRevisionDerivacion = 2;
        public const int ModuloEnviosHistorico = 3;
        public const int ModuloReporteHistoricoFT = 4;

        public const string MensajeLogDerivacionArea = "Area derivada para revision de envio.";
        public const string CarpetaAreasRCSolicitud = "Solicitud";
        public const string CarpetaAreasRCSubsanacíon = "Subsanación de Obs.";

        public const int ModcodiFichaTecnicaExtranet = 49; //agentes

        public const int IdOptionModuloAreas = 1410;

        public const int IdoptionAdminFicha = 1363; // Panatalla Envios Administrador Ficha Técnica 
        //public const int IdoptionVisualizarFTVEIntranet = 1383; // Ficha Técnica Extranet (EN INTRANET) 
        //public const int IdoptionVisualizarFTVIntranet = 1361; //Visualizar ficha técnica vigente 
        //public const int IdoptionConfigurarFTVIntranet = 1386; // Configurar Visualización Ficha Técnica 

        // public const int RolUsuarioIntranetAreas_SoloConsulta = 76;
        public const int RolUsuarioIntranetAreas_PermisoTotal = 73;
        public const int RolUsuarioIntranetAreas_SoloNoConfidenciales = 74;

        #region Plantilla de correo

        public const int PlantcodiNotificacionCambiosConfiguracionFT = 123;
        public const int PlantcodiNotificacionCambiosFMOficial = 124;
        public const int PlantcodiNotificacionCambiosEquiposVisualizacionPortalWeb = 125;
        public const int PlantcodiNotificacionCambiosEquiposVisualizacionExtranet = 180;

        public const int RecordatorioVencimientoPlazoSolicitudConexion = 229;
        public const int RecordatorioVencimientoPlazoSolicitudIntegracion = 230;
        public const int RecordatorioVencimientoPlazoSolicitudOperacionComercial = 231;
        public const int RecordatorioVencimientoPlazoSolicitudModifTecnica = 232;
        public const int RecordatorioVencimientoPlazoSolicitudModifTecnicaBaja = 233;

        public const int RecordatorioVencimientoPlazoSubsanacionConexion = 234;
        public const int RecordatorioVencimientoPlazoSubsanacionIntegracion = 235;
        public const int RecordatorioVencimientoPlazoSubsanacionOperacionComercial = 236;
        public const int RecordatorioVencimientoPlazoSubsanacionModifTecnica = 237;
        public const int RecordatorioVencimientoPlazoSubsanacionModifTecnicaBaja = 238;

        public const int RecordatorioVencimientoPlazoSolicitudAreasConexion = 252;
        public const int RecordatorioVencimientoPlazoSolicitudAreasIntegracion = 253;
        public const int RecordatorioVencimientoPlazoSolicitudAreasOperacionComercial = 254;
        public const int RecordatorioVencimientoPlazoSolicitudAreasModifTecnica = 255;
        public const int RecordatorioVencimientoPlazoSolicitudAreasModifTecnicaBaja = 256;

        public const int PlantillaCorreosEnviados = 282;
        public const int PlantcodiDerivacionSolicitudConexion = 283;
        public const int PlantcodiDerivacionSolicitudIntegracion = 284;
        public const int PlantcodiDerivacionSolicitudOpComercial = 285;
        public const int PlantcodiDerivacionSolicitudMFT = 286;
        public const int PlantcodiDerivacionSolicitudMFTDB = 287;
        public const int PlantcodiDerivacionSubsanacionConexion = 288;
        public const int PlantcodiDerivacionSubsanacionIntegracion = 289;
        public const int PlantcodiDerivacionSubsanacionOpComercial = 290;
        public const int PlantcodiDerivacionSubsanacionMFT = 291;
        public const int PlantcodiDerivacionSubsanacionMFTDB = 292;

        public const int PlantcodiNotificacionCambiosEquiposVisualizacionIntranet = 251;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoSubsanarObservacionesConexion = 221;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoSubsanarObservacionesIntegracion = 222;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoSubsanarObservacionesOpComercial = 223;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoSubsanarObservacionesModifTecnica = 224;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoSubsanarObservacionesModifTecnicaBaja = 225;

        public const int PlantcodiRevisionAreasSolicitudConexion = 257;
        public const int PlantcodiRevisionAreasSolicitudIntegracion = 258;
        public const int PlantcodiRevisionAreasSolicitudOpComercial = 259;
        public const int PlantcodiRevisionAreasSolicitudMFT = 260;
        public const int PlantcodiRevisionAreasSolicitudMFTDB = 261;

        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSolicitudAreasConexion = 262;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSolicitudAreasIntegracion = 263;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSolicitudAreasOpComercial = 264;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSolicitudAreasModifTecnica = 265;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSolicitudAreasModifTecnicaBaja = 266;

        public const int RecordatorioVencimientoPlazoSubsanacionAreasConexion = 267;
        public const int RecordatorioVencimientoPlazoSubsanacionAreasIntegracion = 268;
        public const int RecordatorioVencimientoPlazoSubsanacionAreasOperacionComercial = 269;
        public const int RecordatorioVencimientoPlazoSubsanacionAreasModifTecnica = 270;
        public const int RecordatorioVencimientoPlazoSubsanacionAreasModifTecnicaBaja = 271;

        public const int PlantcodiRevisionAreasSubsanacionConexion = 272;
        public const int PlantcodiRevisionAreasSubsanacionIntegracion = 273;
        public const int PlantcodiRevisionAreasSubsanacionOpComercial = 274;
        public const int PlantcodiRevisionAreasSubsanacionMFT = 275;
        public const int PlantcodiRevisionAreasSubsanacionMFTDB = 276;

        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSubsanacionAreasConexion = 277;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSubsanacionAreasIntegracion = 278;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSubsanacionAreasOpComercial = 279;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSubsanacionAreasModifTecnica = 280;
        public const int PlantcodiNotificacionCambiosCulminacionPlazoRevisarSubsanacionAreasModifTecnicaBaja = 281;

        // datos adicionales de plantilla correo
        public const int ParametroVencimientoPlazoSolicitudConexion = 40;
        public const int ParametroVencimientoPlazoSolicitudIntegracion = 41;
        public const int ParametroVencimientoPlazoSolicitudOperacionComercial = 42;
        public const int ParametroVencimientoPlazoSolicitudModifTecnica = 43;
        public const int ParametroVencimientoPlazoSolicitudModifTecnicaBaja = 44;

        public const int ParametroVencimientoPlazoSubsanacionConexion = 45;
        public const int ParametroVencimientoPlazoSubsanacionIntegracion = 46;
        public const int ParametroVencimientoPlazoSubsanacionOperacionComercial = 47;
        public const int ParametroVencimientoPlazoSubsanacionModifTecnica = 48;
        public const int ParametroVencimientoPlazoSubsanacionModifTecnicaBaja = 49;

        public const int ParametroVencimientoPlazoSolicitudAreasConexion = 70;
        public const int ParametroVencimientoPlazoSolicitudAreasIntegracion = 71;
        public const int ParametroVencimientoPlazoSolicitudAreasOperacionComercial = 72;
        public const int ParametroVencimientoPlazoSolicitudAreasModifTecnica = 73;
        public const int ParametroVencimientoPlazoSolicitudAreasModifTecnicaBaja = 74;

        public const int ParametroVencimientoPlazoSubsanacionAreasConexion = 75;
        public const int ParametroVencimientoPlazoSubsanacionAreasIntegracion = 76;
        public const int ParametroVencimientoPlazoSubsanacionAreasOperacionComercial = 77;
        public const int ParametroVencimientoPlazoSubsanacionAreasModifTecnica = 78;
        public const int ParametroVencimientoPlazoSubsanacionAreasModifTecnicaBaja = 79;

        #endregion

        #region Proceso automático

        public const string MetodoFT2NotificacionCulminacionPlazoSubsanar_Conexion = "FT2NotificacionCulminacionPlazoSubsanar_Conexion";//prcscodi: 60
        public const string MetodoFT2NotificacionCulminacionPlazoSubsanar_Integracion = "FT2NotificacionCulminacionPlazoSubsanar_Integracion";//prcscodi: 61
        public const string MetodoFT2NotificacionCulminacionPlazoSubsanar_OpComercial = "FT2NotificacionCulminacionPlazoSubsanar_OpComercial";//prcscodi: 62
        public const string MetodoFT2NotificacionCulminacionPlazoSubsanar_Modif = "FT2NotificacionCulminacionPlazoSubsanar_Modif";//prcscodi: 63
        public const string MetodoFT2NotificacionCulminacionPlazoSubsanar_ModifBaja = "FT2NotificacionCulminacionPlazoSubsanar_ModifBaja";//prcscodi: 64

        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion";//prcscodi: 65
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion";//prcscodi: 66
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial = "FT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial";//prcscodi: 67
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Modif = "FT2RecordatorioVencPlazoParaRevisarSolicitud_Modif";//prcscodi: 68
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja = "FT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja";//prcscodi: 69

        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion";//prcscodi: 70
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion";//prcscodi: 71
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial";//prcscodi: 72
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif";//prcscodi: 73
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja = "FT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja";//prcscodi: 74

        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Conexion = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Conexion";//prcscodi: 75
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Integracion = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Integracion";//prcscodi: 76
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_OpComercial = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_OpComercial";//prcscodi: 77
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Modif = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_Modif";//prcscodi: 78
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_ModifBaja = "FT2RecordatorioVencPlazoRevisarSolicitudAreas_ModifBaja";//prcscodi: 79

        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Conexion = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Conexion";//prcscodi: 80
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Integracion = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Integracion";//prcscodi: 81
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_OpComercial = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_OpComercial";//prcscodi: 82
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Modif = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_Modif";//prcscodi: 83
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_ModifBaja = "FT2NotifCulminacionPlazoRevisarAreasSolicitud_ModifBaja";//prcscodi: 84

        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Conexion = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Conexion";//prcscodi: 85
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Integracion = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Integracion";//prcscodi: 86
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_OpComercial = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_OpComercial";//prcscodi: 87
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Modif = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_Modif";//prcscodi: 88
        public const string MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_ModifBaja = "FT2RecordatorioVencPlazoRevisarSubsanacionAreas_ModifBaja";//prcscodi: 89

        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Conexion = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Conexion";//prcscodi: 90
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Integracion = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Integracion";//prcscodi: 91
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_OpComercial = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_OpComercial";//prcscodi: 92
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Modif = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_Modif";//prcscodi: 93
        public const string MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_ModifBaja = "FT2NotifCulminacionPlazoRevisarAreasSubsanacion_ModifBaja";//prcscodi: 94

        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_Conexion = 60;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_Integracion = 61;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_OperacionComercial = 62;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_Modif = 63;
        public const int PrcscodiNotificacionCulminacionPlazoSubsanacion_ModifBaja = 64;

        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitud_Conexion = 65;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitud_Integracion = 66;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitud_OpComercial = 67;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitud_Modif = 68;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitud_ModifBaja = 69;

        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacion_Conexion = 70;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacion_Integracion = 71;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacion_OpComercial = 72;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacion_Modif = 73;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja = 74;

        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitudAreas_Conexion = 75;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitudAreas_Integracion = 76;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitudAreas_OpComercial = 77;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitudAreas_Modif = 78;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSolicitudAreas_ModifBaja = 79;

        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSolicitud_Conexion = 80;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSolicitud_Integracion = 81;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSolicitud_OpComercial = 82;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSolicitud_Modif = 83;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSolicitud_ModifBaja = 84;

        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacionAreas_Conexion = 85;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacionAreas_Integracion = 86;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacionAreas_OpComercial = 87;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacionAreas_Modif = 88;
        public const int PrcscodiRecordatorioVencPlazoParaRevisarSubsanacionAreas_ModifBaja = 89;

        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSubsanacion_Conexion = 90;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSubsanacion_Integracion = 91;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSubsanacion_OpComercial = 92;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSubsanacion_Modif = 93;
        public const int PrcscodiNotificacionCulminacionPlazoRevisarAreasSubsanacion_ModifBaja = 94;

        #endregion

    }

    public class UserCorreo
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Imagen { get; set; }
    }
}