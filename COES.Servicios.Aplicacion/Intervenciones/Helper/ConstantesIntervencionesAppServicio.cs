using System;
using System.Collections.Generic;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Constantes para el aplicativo de intervenciones
    /// </summary>
    public class ConstantesIntervencionesAppServicio
    {
        #region Modulo Registro
        // Entorno       
        public const int AmbienteExtranet = 1;
        public const int AmbienteIntranet = 2;

        // Formatos de Archivos
        public const string HojaReporteExcel = "REPORTE";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppTxt = "application/txt";

        // Para el Excel: cantidad Maxima de Rows and Columns
        public const int MaxRows = 1048576;
        public const int MaxColumns = 16384;

        // Escenarios o modos de operación de la aplicación
        public const int iEscenarioMantenimiento = 1; // MANTENIMIENTO;
        public const int iEscenarioConsulta = 2; // CONSULTAS;
        public const int iEscenarioReporte = 3; // PARA LOS DEMAS REPORTES;
        public const int iEscenarioConsultaCruzadas = 4; // CONSULTA CRUZADAS;
        public const int iEscenarioReporteIntervencionesMayores = 5; // SOLO PARA REPORTE DE INTERVENCIONES MAYORES;

        //Areas usuario
        public const int AreacodiSPR = 7;

        // Constante para el repositorio CSV       
        public const string SeparadorCampo = ",";
        public const string SeparadorCampoCSV = ";";

        // Indicadores
        public const string sSi = "S";
        public const string sNo = "N";

        public const string sES = "E";
        public const string sFS = "F";

        public const string sGN = "G";
        public const string sLN = "L";
        public const string FlagIndispES = "E/S";
        public const string FlagIndispFS = "F/S";

        public const int iSi = 1;
        public const int iNo = 0;

        public const int AlertaEmsNO = 0;
        public const int AlertaEmsSI = 1;

        // Filtro TODOS
        public const string sFiltroTodos = "0";
        public const string FiltroEmpresaTodos = "0";
        public const string FiltroEquipoTodos = "0";
        public const string FiltroTipoIntervencionTodos = "0";
        public const string FiltroUbicacionTodos = "0";
        public const string FiltroTipoEquipoTodos = "0";
        public const string FiltroIndispoTodos = "0";

        //Programacion de solo Lectura
        public const int ProgSoloLectura = 1;

        // Estados       
        public const string sEstadoEnProceso = "4,5,6,7,8,9";

        public const int InEstadoConforme = 1; //Antes del pase a PROD, los que estaban EN PROCESO DE EVALUACIÓN se consideran CONFORME
        public const int InEstadoAprobado = 2;
        public const int InEstadoRechazado = 3;

        public const int InEstadoPendienteEnProcesoEvaluacion = 6; //Antes del pase a PROD era estadocodi 1
        public const int InEstadoPendienteConformidadPorCorte = 4;
        public const int InEstadoPendienteConformidadPorInforme = 5;
        public const int InEstadoPendienteCanceladoPorAgente = 7;
        public const int InEstadoPendienteCanceladoPorCOES = 8;
        public const int InEstadoPendienteOtro = 9;

        public const string ProcesoEvaluacion = "Pendiente - Proceso de Evaluación";
        public const string CanceladoPorAgente = "Pendiente - Cancelado Por Agente";
        public const string CanceladoPorCOES = "Pendiente - Cancelado Por COES";
        public const string Pendiente = "Pendiente";
        public const string ConformidadInforme = "Pendiente - Conformidad por Informe";
        public const string ConformidadCorte = "Pendiente - Conformidad por Corte";
        public const string Otros = "Pendiente - Otros";
        public const string Rechazado = "Rechazado";
        public const string Conforme = "Conforme";

        public const int AgenteLeyo = 1;
        public const int AgenteNoLeyo = 0;


        public const int FlagTodo = -1;
        public const int FlagActivoNoEliminado = 0;
        public const int FlagSiEliminado = 1;

        public const int FlagAprobado = 1;
        public const int FlagNoAprobado = 1;

        public const int FlagProcesadoReversion = 2;

        //Estado de reversion
        public const int SinAprobarReversion = 0;
        public const int AprobadoReversion = 1;

        // Clases de Programación
        public const int iClassProgNodefinido = -1;
        public const int iClassProgProgramado = 1;
        public const int iClassProgReprogramado = 2;
        public const int iClassProgForzadoImprevisto = 3;

        // Tipos de Programación
        public const int TipoProgramacionEjecutado = 1;
        public const int TipoProgramacionProgramadoDiario = 2;
        public const int TipoProgramacionProgramadoSemanal = 3;
        public const int TipoProgramacionProgramadoMensual = 4;
        public const int TipoProgramacionProgramadoAnual = 5;

        public const int TipoProgramacionPM7 = 1000;
        public const int FormatoEjecutadoExtranet = 103;
        public const int FormatoEjecutadoIntranet = 104;
        public const int FormatoProgramadoDiario = 105;
        public const int FormatoProgramadoSemanal = 106;
        public const int FormatoProgramadoMensual = 107;
        public const int FormatoProgramadoAnual = 108;

        // Nombre de Tipo de Intervención
        public const string NombPreventivo = "PREVENTIVO";// 1-PREVENTIVO
        public const string NombCorrectivo = "CORRECTIVO";// 2-CORRECTIVO
        public const string NombPreventivo2 = "MANTENIMIENTO PREVENTIVO";// 1-PREVENTIVO
        public const string NombCorrectivo2 = "MANTENIMIENTO CORRECTIVO";// 2-CORRECTIVO
        public const string NombAmpliacionMejoras = "AMPLIACIONES/MEJORAS";// 3-AMPLIACIONES/MEJORAS
        public const string NombAmpliaciones = "AMPLIACIONES";// 3-AMPLIACIONES/MEJORAS
        public const string NombEvento = "EVENTO";// 4-EVENTO
        public const string NombEventos = "EVENTOS";// 4-EVENTO
        public const string NombPruebas = "PRUEBAS";// 6-PRUEBAS
        public const string NombSeguridadPersonas = "SEGURIDAD DE LAS PERSONAS";// 12-SEGURIDAD DE LAS PERSONAS
        public const string NombEnergizacion = "ENERGIZACION DE NUEVOS EQUIPOS O INSTALACIONES";// 10-ENERGIZACIÓN DE NUEVOS EQUIPOS O INSTALACIONES
        public const string NombOtros = "OTROS";// 9-OTROS

        //Código Tipo de Intervención
        public const int CodPreventivo = 1;// 1-PREVENTIVO
        public const int CodCorrectivo = 2;// 2-CORRECTIVO
        public const int CodAmpliacionMejoras = 3;// 3-AMPLIACIONES/MEJORAS
        public const int CodEvento = 4;// 4-EVENTO
        public const int CodPruebas = 6;// 6-PRUEBAS
        public const int CodSeguridadPersonas = 12;// 12-SEGURIDAD DE LAS PERSONAS
        public const int CodEnergizacion = 10;// 10-ENERGIZACIÓN DE NUEVOS EQUIPOS O INSTALACIONES
        public const int CodOtros = 9;// 9-OTROS

        // Nombre de Clase de programación
        public const string ClaseProgramado = "PROGRAMADO";// 1 - PROGRAMADO
        public const string ClaseReprogramado = "REPROGRAMADO";// 2 - REPROGRAMADO
        public const string ClaseForzadoImprevisto = "FORZADO IMPREVISTO";// 3 - FORZADO IMPREVISTO
        public const string ClaseForzadoImprevisto2 = "FORZADO/IMPREVISTO";// 3 - FORZADO/IMPREVISTO

        //Campo No definido
        public const int NoDefinido = -1;

        //Acciones
        public const int AccionVer = 1;
        public const int AccionEditar = 2;
        public const int AccionNuevo = 3;

        // Acciones al importar
        public const string AccionReemplazar = "1";
        public const string AccionAdicionar = "2";

        // Tipos de Programaciónes Para Intervenciones Cruzadas
        public const int TipoProgramacionEjecutadoDiario = 7; // 6
        public const int TipoProgramacionEjecutadoMensual = 8; // 7
        public const int TipoProgramacionProgramadoAnualMensual = 9; // 8
        public const int TipoProgramacionProgramadoAnualMensualSemanal = 10; // 9
        public const int TipoProgramacionProgramadoMensualSemanalDiario = 11;
        public const int TipoProgramacionProgramadoSemanalDiario = 12;

        //criterio para obtener intervenciones anuales
        public const int CriterioAnualMasReciente = 1;
        public const int CriterioAprobadoMasReciente = 2;

        // Versiones de PAI
        public const int ivPAI1 = 1;
        public const int ivPAI2 = 2;

        // Actividades del Sistema
        public const string sCreacion = "Creación";
        public const string sModificacion = "Modificacion";
        public const string sEliminacion = "Eliminacion";
        public const string sAprobacionRechazo = "Aprobacion/Rechazo";
        public const string sProcesamiento = "Procesamiento";
        public const string sCopiar = "Copia";
        public const string sImportar = "Importar";
        public const string sMensaje = "Mensaje";

        //Flags que se utilizan para utilizar el aplicativo en modo de pruebas
        public readonly static bool FlagGrabarMantto = ConfigurationManager.AppSettings["FlagGrabarEveMantto"].ToString() == "S";
        public readonly static bool FlagCompletarDatosAplicativos = ConfigurationManager.AppSettings["FlagCompletarDatosAplicativos"].ToString() == "S";
        public const string KeyFlagEnviarNotificacionManual = "FlagEnviarNotificacionManual";
        public const string KeyFileSystemPortal = "FileSystemPortal";

        public const int ModcodiIntervenciones = 1; //Mantenimientos

        // Correlativo de la tabla: SI_FUENTEDATOS
        public readonly static int FdatcodiIntervenciones = 10;

        // Correlativo de la tabla: SI_TIPOMENSAJE
        public readonly static int TMsgcodiTodos = -1; //Todos
        public readonly static int TMsgcodiMensajes = 7; //Mensajes
        public readonly static int TMsgcodiNoEjecutados = 8; //Mensajes
        public readonly static int TMsgcodiAlertaHo = 9; //ALERTA HORAS DE OPERACIÓN
        public readonly static int TMsgcodiSustentoExclIncl = 10; //INTERVENCIONES - RECORDATORIO SUSTENTO INCLUSION / EXCLUSION

        // Correlativo de la tabla: SI_ESTADOMENSAJE
        public readonly static int EstMsgcodiEnProceso = 1;

        // Valor del tipo de mensaje
        public const int MsgTipoEnviado = 1;
        public const int MsgTipoRecibido = 2;
        public const int MsgTipoNoNecesitaRpta = 3;

        //manejo de sesion
        public const string IdsEmpresasTotal = "EMPRESAS_TOTAL_INTERVENCIONES";
        public const string IdsEmpresasSeleccionado = "EMPRESAS_SELECTED_INTERVENCIONES";

        //public const para  en control de archivos  Archivos (upload y download)";
        public const string sNombreCarpetaTemporal = "Temporal";

        //Nombre del  modulo que se maneja
        public const string sModuloIntervencion = "Informe_Agente";
        public const string sModuloMensaje = "Mensajes";

        public const string ModuloProcedimientoManiobras = "ProcedimientoManiobra";
        public const string ModuloArchivosXIntervenciones = "ArchivosXIntervencion";
        public const string sModuloTemporalMensaje = "TM";
        public const string ModuloManualUsuario = "Manual";

        public const string ModuloFactorF1F2 = "FactoresF1F2";

        //Nombre de la carpeta para generar Zip
        public const string NombreArchivosZip = "Archivos";
        public const string NombreArchivoVistaPrevia = "VistaPrevia.pdf";


        #endregion

        #region Modulo Reportes       
        //constantes para fileServer
        //public const string FolderRaizIntervenciones = "Intervenciones\\";
        public const string Plantilla = "Plantilla\\";

        public const string Reportes = "Reportes\\";
        public const string RutaReportes = "Areas\\Intervenciones\\Reporte\\";

        public const string NombreReporteExcelIntervencionesProcesadas = "IntervencionesProcesadas.xlsx";
        public const string NombreReporteExcelIntervencionesCruzadas = "ReporteIntervencionesCruzadas.xlsx";
        public const string NombreReporteExcelIntervencionesCruzadasIndisp = "ReporteIntervencionesCruzadasIndisponible.xlsx";

        public const string NombreReporteExcelListadoIntervencionesMayores = "ListadoIntervencionesMayores.xlsx";
        public const string NombreReporteExcelAnexo2 = "ANEXO2-PROGRAMA_DE_MANTENIMIENTO_MAYOR.xlsx";
        public const string NombreReporteExcelAnexo3 = "ANEXO3-SISTEMAS_AISLADOS.xlsx";
        public const string NombreReporteExcelAnexo4 = "ANEXO4-INTERRUPCIONES_DE_SUMINISTRO.xlsx";
        public const string NombreReporteExcelAnexo5ES = "ANEXO5-LISTADO_PAM_(EN SERVICIO).xlsx";
        public const string NombreReporteExcelAnexo5FS = "ANEXO5-LISTADO_PAM_(FUERA DE SERVICIO).xlsx";
        public const string NombreReporteExcelAnexo6Generacion = "GENERACION_PAI.xlsx";
        public const string NombreReporteExcelAnexo6Transmision = "TRANSMISION_PAI.xlsx";
        public const string NombreReporteExcelIntervencionesImportantes = "IntervencionesImportantes.xlsx";
        public const string NombreReporteExcelConexionesProvisionales = "ConexionesProvisionales.xlsx";
        public const string NombreReporteExcelSistemasAislados = "SistemasAislados.xlsx";
        public const string NombreReporteExcelInterrupcionRestriccionSuministros = "InterrupcionRestriccionSuministros.xlsx";
        public const string NombreReporteExcelOSINERGMINProc257d = "OSINERGMIN_Proc257d.xlsx";
        public const string NombreReporteExcelIntervenciones = "Intervenciones.xlsx";
        public const string NombreReporteExcelIntervencionesOSINERGMING = "Intervenciones.xls";
        public const string NombreReporteExcelF1F2ProgramadosEjecutados = "F1F2ProgramadosEjecutados.xlsx";
        public const string NombreReporteExcelF1F2Indices = "F1F2Indices.xlsx";
        public const string NombreReporteExcelMensajes = "Mensajes.xlsx";
        public const string NombreReporteExcelLogActividadesSistema = "LogActividadesSistema.xlsx";

        public const string NombreReporteExcelntervencionesMayores = "ReporteIntervencionesMayores.xlsx";

        public const string InformeProgramaDiario = "SPR-IPDO-{0}-{1} INFORME DEL PROGRAMA DIARIO DE OPERACIÓN DEL SEIN.docx";
        public const string InformeProgramaDiarioIPDI = "SPR-IPDI-{0}-{1} PROGRAMA DIARIO DE INTERVENCIONES.docx";
        public const string InformeProgramaDiarioOperaciones = "InformeProgramaDiarioOperaciones.docx";
        public const string InformeProgramaSemanalOperaciones = "InformeProgramaSemanalOperaciones.docx";
        public const string InformeProgramaSemanal = "SPR-IPSO-{0}-{1} PROGRAMA SEMANAL DE OPERACION.docx";
        public const string InformeProgramaSemanalIPSI = "SPR-IPSI-{0}-{1} PROGRAMA SEMANAL DE INTERVENCIONES.docx";
        public const string InformeProgramaMensual = " SPR-IPMI-{0}-{1}_PROGRAMA MENSUAL DE INTERVENCIONES DE {2}_{1}.docx";
        public const string InformeProgramaAnual = "SPR-IT-IPAI-S{0}-{1}.docx";

        // Nombre de la plantilla para la exportacion de registros de intervenciones
        //public const string NombrePlantillaExcelManttoXlsm = "Formato_Mantto_v3_7_79.xlsm";
        public const string NombrePlantillaExcelManttoXlsm = "Mantto_v3_8_01.xlsm";
        public const string NombrePlantillaExcelManttoXls = "Mantto_v3_8_01.xls";

        // Nombre de la plantilla para la exportacion de registros del reporte de intervenciones para agentes
        public const string NombrePlantillaExcelManttoAgentesXls = "Formato_Mantto_v3_7_79.xlsm";

        public const int iRptIntervencioneImportantes = 1; // Reporte de Intervenciones Importantes
        public const int iRptIntervencioneMayores = 2; // Reporte de Intervenciones Mayores
        public const int iRptConexionesProvisionales = 3; // Reporte de Conexiones Provisionales
        public const int iRptSistemasAislados = 4; // Reporte de Sistemas Aislados
        public const int iRptInterrupcionRestriccionSuministros = 5; // Reporte de Interrupción Restricción Suministros
        public const int iRptOSINERGMIN = 6; // Reporte para OSINERGMIN
        public const int iRptIntervenciones = 7; // Reporte de Intervenciones
        public const int iRptAgentes = 8; // Reporte de Agentes 

        public const int iInfProgramaDiarioOpe = 6; // Informe Programa Diario Operaciones
        public const int iInfProgSemanalOpe = 7; // Informe Programa Semanal Operaciones

        public const int iListado = 1; // Listado de la grilla
        public const int iAnexo2 = 2; // Anexo N°2 Programa de Mantenimientos Mayores
        public const int iAnexo3 = 3; // Anexo N°3 Sistemas Aislados
        public const int iAnexo4 = 4; // Anexo N°4 Reduccion y/o Restriccion de Suministro
        public const int iAnexo5ES = 5; // Anexo N°5 En Servicio
        public const int iAnexo5FS = 6; // Anexo N°5 Fuera de Servicio
        public const int iAnexo6Generacion = 7; // Anexo N°6 Generación
        public const int iAnexo6Transmision = 8; // Anexo N°6 Transmisión       
        #endregion

        //estados de programación de intervenciones
        public const string Recepcion = "Recepción de información";
        public const string Proceso = "En proceso";
        public const string Aprobado = "Aprobado";
        public const int ProgEstadoRecepcion = 1;
        public const int ProgEstadoProceso = 2;
        public const int ProgEstadoAprobado = 3;

        public const string Abierto = "Abierto";
        public const string Cerrado = "Cerrado";
        public const int EstadoAbierto = 4;
        public const int EstadoCerrado = 5;

        public const int FlagAdminCreacion = 0;
        public const int FlagAdminModificacion = 1;

        public const int FlagAgenteCreacion = 5;
        public const int FlagAgenteModificacion = 6;

        //constantes mantenimientos - eventos
        public const int CateevencodiManttoEjecutado = 1;
        public const int CateevencodiEventoOcurrido = 2;

        //Notificacion de correo
        public const int PlantcodiAlertMant = 63;
        public const int PlantcodiAlertaHoraOperacion = 62;
        public const int PlantcodiSustentoExclInclProgDiario = 64;
        public const int PlantcodiSustentoExclInclProgSemanal = 65;
        public const int PlantcodiAbrevEmpresa = 66;
        public const int Plantcodiconfiguracionnotificacion = 67;
        public const int PlantcodiAlertaAprobacionAutomatica = 68;
        public const int PlantcodiNotificacionComunicacionApp = 69;

        public const int RolAdministradorEmpresa = 133;

        //Proceso automático
        public const int PrcscodiAlertaIntervencionesProgNoEjec = 22;
        public const string PrcsmetodoAlertaIntervencionesProgNoEjec = "EnviarCorreoAlertaAutomatico";
        public const int PrcscodiSustentoExclInclProgDiario = 51;
        public const string PrcsmetodoSustentoExclInclProgDiario = "IntervencionRecordatorioSustentoDiario";
        public const int PrcscodiSustentoExclInclProgSemanal = 52;
        public const string PrcsmetodoSustentoExclInclProgSemanal = "IntervencionRecordatorioSustentoSemanal";

        //Proceso automático para anular reversiones
        public const int PrcscodiAnularReversion = 34;
        public const string PrcsmetodoAnularReversion = "ProcesoAnularReversion";

        //Tipo de comparaciones
        public const int ComparacionRptActividades = 1;
        public const int ComparacionNotifExclIncl = 2;

        public const int ParametroDefecto = 0;

        //Variables correo
        public const string VariableTodosAgentesEmpresa = "{TODOS_AGENTES_DE_EMPRESA}";
        public const string VariableDescripcionPlan = "{DESCRIPCION_PLAN}";
        public const string VariableTablaExcl = "{TABLA_DATOS_INTERVENCION_EXCLUSION}";
        public const string VariableTablaIncl = "{TABLA_DATOS_INTERVENCION_INCLUSION}";
        public const string VariableCorreoHorizonte = "{CORREO_HORIZONTE}";
        public const string VariableAdminModuloEmpresa = "{ADMIN_MODULO_EMPRESA}";
        public const string VariableCorreoUsuarioModif = "{CORREO_USUARIO_MODIF}";
        public const string VariableCodigoEmpresa = "{CODIGO_EMPRESA}";
        public const string VariableNombreEmpresa = "{NOMBRE_EMPRESA}";
        public const string VariableAbrevAntes = "{ABREV_ANTES}";
        public const string VariableAbrevAhora = "{ABREV_AHORA}";
        public const string VariableUsuarioModif = "{USUARIO_MODIF}";
        public const string VariableFechaModif = "{FECHA_MODIF}";
        public const string VariableFechaReporte = "{FECHA_REPORTE}";
        public const string VariableReporte = "{REPORTE}";
        public const string VariableConfiguracionNotificacion = "{CONFIGURACION_NOTIFICACION}";

        //Variables correo error aprobación automática
        public const string VariableNombreProgramacion = "{NOMBRE_PROGRAMACION}";
        public const string VariableListaErrores = "{LISTA_ERRORES}";

        //Variables correo notificación de comunicación
        public const string VariableComunicacionPara = "{COMUNICACION_PARA}";
        public const string VariableComunicacionCC = "{COMUNICACION_CC}";
        public const string VariableComunicacionBCC = "{COMUNICACION_BCC}";
        public const string VariableComunicacionAsunto = "{COMUNICACION_ASUNTO}";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_de_Usuario_Intranet_Intervenciones_v6.2.pdf";
        public const string ArchivoManualUsuarioExtranet = "Manual_de_Usuario_Extranet_Intervenciones.pdf";
        public const string ModuloManualUsuarioNuevo = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizIntervenciones = "Intervenciones\\";

        // Tipos de Programaciónes Para Intervenciones Cruzadas
        public const int CodigoParametroDiario = 20;
        public const int CodigoParametroSemanal = 21;
        public const int CodigoParametroMensual = 22;
        public const int CodigoParametroAnual = 23;

        //Propiedad (EQ_PROPEQUI)
        public const int PropiedadPotenciaIndisponible = 1965;

        //dias
        public const int DiaNormal = 0;
        public const int DiaSabado = 1;
        public const int DiaDomingoFeriado = 2;
        public const int TotalSegundosDia = 24 * 60 * 60;
        public const int CriterioProximoConsecutivo = 1;
        public const int CriterioProximoContinuo = 2;
        public const int CriterioProximoConsecutivoRangoHora = 3;

        //accion cruzada
        public const int AccionCruzadaAgregar = 1;
        public const int AccionCruzadaSobreescribir = 2;

        //Actualiza Reporte
        public const int RpteProgDiarioIPDO = 1;
        public const int RpteProgDiarioIPDI = 2;
        public const int RpteProgSemanalIPSO = 3;
        public const int RpteProgSemanalIPSI = 4;
        public const int RpteProgMensualIPMI = 5;
        public const int RpteProgAnualIPAI = 6;

        //Cuadro informes word
        public const string RolElaboracion = "E";
        public const string RolRevision = "R";
        public const string RolAprobacion = "A";

        public const string ValFechaLarga = "{FECHA_LARGA}";
        public const string ValNroSemOperativa = "{NRO_SEM_OPE}";
        public const string ValSemAnio = "{SEM_ANIO}";
        public const string ValDiaAnioAnio = "{DIAANIO_ANIO}";
        public const string ValPorcReservPrim = "{%RESERV_PRIM}";
        public const string ValPrecMaxResevSec = "{PREC_MAX_RSEC}";
        public const string ValVolumenTotal = "{VOL_TOTAL}";
        public const string ValDiaIniSemOper = "{DIA_INISEMOPE}";
        public const string ValSemIniALSemFin = "{SEM_DEL_AL}";
        public const string ValMesAnioSemOperativa = "{MES_ANIOSEMOPE}";
        public const string ValDiaMesAnio = "{DIA_MES_ANIO}";
        public const string ValDiaAnio = "{DIA_ANIO}";

        //Formatos, lectura
        public const int IdReporteQdLagunas = 98;
        public const int IdReporteQtotalLagunas = 99;
        public const int IdLecturaQnQdLaguna = 277;
        public const int IdlecturaProgramaCortoPlazo = 67;
        public const int SrestConsumoCombustible = 119;

        //hojas de reportes
        public const int HojaEjecutado = 1;
        public const int HojaProgramadoMensual = 2;
        public const int HojaProgramadoSemanal = 3;
        public const int HojaProgramadoDiario = 4;
        public const int HojaEjecutadoMenorSiMensual = 5;
        public const int HojaEjecutadoMenorNoMensual = 6;

        public const int TipoInsumoIntervencion = 1;
        public const int TipoInsumoFactorF1F2 = 2;

        public const string PlantillaExcelF1F2Dashboard = "PLT_F1F2_DASHBOARD.xlsx";
        public const string PlantillaExcelF1F2Reporte = "PLT_F1F2_REPORTE.xlsx";

        //Menú intervenciones
        public const int MenuOpcionCodeIntervenciones = 913; //antes era Registro

        //Estados plantilla
        public const string Activo = "A";
        public const string Historico = "H";
        public const int TipoPltSustPdInclCorr = 1; //Plantilla programa diario - Inclusión - Generación y líneas - Correctivo	
        public const int TipoPltSustPdInclPrevEnergSeg = 2; //Plantilla programa diario - Inclusión - Generación y líneas - Preventivo, energización de nuevos equipos y seguridad de las personas	
        public const int TipoPltSustPdInclPrueba = 3; //Plantilla programa diario - Inclusión - Generación y líneas - Pruebas	
        public const int TipoPltSustPdExclCorrPrev = 4; //Plantilla programa diario - Exclusión - Generación y líneas - Correctivo y preventido	
        public const int TipoPltSustPsInclCorr = 5; //Plantilla programa semanal - Inclusión - Generación y líneas - Correctivo	
        public const int TipoPltSustPsInclPrevEnergSeg = 6; //Plantilla programa semanal - Inclusión - Generación y líneas - Preventivo, energización de nuevos equipos y seguridad de las personas	
        public const int TipoPltSustPsInclPrueba = 7; //Plantilla programa semanal - Inclusión - Generación y líneas - Pruebas	
        public const int TipoPltSustPsExclCorrPrev = 8; //Plantilla programa semanal - Exclusión - Generación y líneas - Correctivo y preventido	

        public const int FlagTieneSustento = 1;

        public const string ArchivoExportacionConfiguracionNotificacion = "ConfiguracionNotificacionMensajes.xlsx";
        public const string TextoSi = "Si";
        public const string TextoNo = "No";
        public const string ValidacionNotificacion = "No puede quitar la selección del tipo {0} y la empresa {1} ya que ningún usuario más tiene configurado dicha notificación.";

        //Proceso automático de aprobación automática
        public const int PrcscodiAprobarProgDiario = 113;
        public const int PrcscodiAprobarProgSemanal = 114;
        public const string PrcsmetodoAprobarProgDiario = "ProcesoAprobarProgramaDiaro";
        public const string PrcsmetodoAprobarProgSemanal = "ProcesoAprobarProgramaSemanal";

        // SI_PARAMETRO_VALOR PARA APROBACIÓN AUTOMÁTICA
        public const int IdParametroAprobacionDiario = 80;
        public const int IdParametroAprobacionSemanal = 81;

        //Tipo de equipo
        public const int FamcodiLinea = 8;
        public const int FamcodiCelda = 6;

        //Factores F1 y F2
        public const int ModuloFactores = 1;
        public const int ModuloFactoresSPR = 2;
        public const string PlantillaExcelF1F2DashboardSPR = "PLT_F1F2_DASHBOARDSPR.xlsx";

        // SI_PARAMETRO_VALOR PARA APROBACIÓN AUTOMÁTICA
        public const int IdParametroPorcentajeSimilitud = 82;

        //Proceso automático de generar versión
        public const int PrcscodiGenerarVersion = 58;
        public const string PrcsmetodoGenerarVersion = "ProcesoGenerarVersion";
    }

    public class IntervencionGridExcel
    {
        /// <summary>
        /// Model para el manejo de grillas tipo excel
        /// </summary>

        public string[] Headers { get; set; }       //colHeaders : Setea verdadero o falso para activar o desactivar los encabezados de las columnas por defecto ( A, B , C ). También puede definir una matriz [' One' , ' Dos ', ' Tres ' , ...] o una función para definir las cabeceras. Si una función se establece el índice de la columna se pasa como parámetro.
        public int[] Widths { get; set; }           //colWidths : Define el ancho de la columna en píxeles. Acepta número, cadena ( que se convertirá en número) , matriz de números (si desea definir el ancho de columna por separado para cada columna ) o una función (si desea ajustar el ancho de columna dinámicamente en cada render )
        public object[] Columnas { get; set; }      //columns : Define las propiedades de las celdas y y los datos para ciertas columnas . Aviso: El uso de esta opción establece un número fijo de columnas ( Opciones startCols , minCols , maxCols serán ignoradas ) .
        public string[][] Data { get; set; }        //data : Fuente de datos inicial que se une a la red de datos por cuadrícula de referencia (datos de edición altera la fuente de datos . Ver Entendimiento vinculante como referencia
        public string[][] DataCodigo { get; set; }        //dataCodigo : Codigo de las filas, no se mezcla con el objeto "Data" porque hace que se descuadre la presentación del handsontable

        public object[] ListaColumnasColor { get; set; }
        public object[] ListaCeldasColor { get; set; }

        public int FixedColumnsLeft { get; set; }   //Permite especificar el número de columnas fijas ( congelado ) en el lado izquierdo de la tabla
        public int FixedRowsTop { get; set; }       //Permite especificar el número de filas fijos ( congelado ) en la parte superior de la tabla
        public List<IntervencionColumnaDia> ListaFecha { get; set; }
        public List<IntervencionCeldaEq> ListaEq { get; set; }

        public const string TipoTexto = "text";
        public const string TipoNumerico = "numeric";
        public const string TipoFecha = "date";
        public const string TipoLista = "dropdown";
        public const string TipoAutocompletar = "autocomplete";
        public int IdTipoProgramacion { get; set; }
    }

    public class FilaMacroIntervencion
    {
        public int Row { get; set; }
        public int NumItem { get; set; }

        public string StrItem { get; set; }
        public string StrCodSeg { get; set; }
        public string StrEmpresa { get; set; }
        public string StrUbicacion { get; set; }
        public string StrTipoEquipo { get; set; }
        public string StrEquinomb { get; set; }
        public string StrEquicodi { get; set; }
        public string StrInicio { get; set; }
        public string StrFin { get; set; }
        public string StrDescripcion { get; set; }
        public string StrDescripcion2 { get; set; }
        public string StrMwIndisp { get; set; }
        public string StrDispon { get; set; }
        public string StrInterrupc { get; set; }
        public string StrSistAislado { get; set; }
        public string StrConexionProv { get; set; }
        public string StrTipo { get; set; }
        public string StrProgr { get; set; }

        public int Equicodi { get; set; }
        public int Areacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Famcodi { get; set; }
        public int Operadoremprcodi { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MwIndisp { get; set; }
        public int Tipoevencodi { get; set; }
        public int Claprocodi { get; set; }
    }

    /// <summary>
    /// Clase para almacenar los elementos seleccionados en excel web
    /// </summary>
    public class IntervencionCopiaGrid
    {
        public string StrFecha { get; set; }
        public string StrListaInterCodi { get; set; }
        public DateTime? Fecha { get; set; }
        public List<int> ListaInterCodi { get; set; }
        public int Contador { get; set; }
    }

    public class IntervencionColumnaDia
    {
        public DateTime Dia { get; set; }
        public string DiaDesc { get; set; }
        public int TipoDia { get; set; }

        public decimal MWIndispXDia { get; set; }
        public List<string> ListaMediaHora { get; set; }
        public List<IntervencionFilaEq> ListaEquipo { get; set; }
    }

    public class IntervencionFilaEq
    {
        public string EmprNomb { get; set; } // Nombre de Empresa
        public string AreaNomb { get; set; } // Nombre de Area (Nombre de Ubicación)
        public string EquiNomb { get; set; } // Nombre de Equipo

        public bool[] ListaTieneIndisp { get; set; } = new bool[48];
        public decimal?[] ListaMWIndisp { get; set; } = new decimal?[48];
    }

    public class IntervencionCeldaEq
    {
        public int Emprcodi { get; set; }  // Id de Empresa
        public int Areacodi { get; set; } // Id de Area (Ubicación del Equipo)
        public int Equicodi { get; set; } // Id de Equipo
        public string EmprNomb { get; set; } // Nombre de Empresa
        public string AreaNomb { get; set; } // Nombre de Area (Nombre de Ubicación)
        public string FamAbrev { get; set; } // Nombre de Familia (Nombre de Tipo de Equipo)
        public string EquiNomb { get; set; } // Nombre de Equipo

        public List<IntervencionCeldaEqDia> ListaEqDia { get; set; }

        public decimal HorasIndispXEq { get; set; }
        public int SegundosIndispXEq { get; set; }
        public bool TieneDiaCompletoIndisp { get; set; }
    }

    public class IntervencionCeldaEqDia
    {
        public DateTime Dia { get; set; }
        public List<IntervencionCeldaDato> ListaDato { get; set; }
        public string DescripcionDia { get; set; }
        public string HtmlDia { get; set; }

        public decimal MWIndispXDia { get; set; }
        public decimal HorasIndispXDia { get; set; }
        public bool[] ListaTieneIndisp { get; set; } = new bool[48];
        public decimal?[] ListaMWIndisp { get; set; } = new decimal?[48];

        public decimal MWRsvaDispXDia { get; set; }
        public int SegIndispXDia { get; set; }
        public string ComentarioDispXDia { get; set; }
    }

    public class IntervencionCeldaDato
    {
        public int Intercodi { get; set; } // Id de Intervención
        public DateTime Interfechaini { get; set; }     // Fecha Hora de Inicio de la Intervención
        public DateTime Interfechafin { get; set; }     // Fecha Hora de Fin de la Intervención
        public string Interindispo { get; set; }
        public int? Interflagsustento { get; set; }

        public string Interdescrip { get; set; }  // Descripción de la Intervención (Mantenimiento)
        public string CeldaClase { get; set; }
        public string Title { get; set; }
        public string CeldaHoraIni { get; set; }
        public string CeldaHoraFin { get; set; }
        public string CeldaHorizonte { get; set; }
        public string CeldaDesc { get; set; }

        public bool EsContinuoFraccionado { get; set; } //Intervención única de varios días que se muestra fraccionado en la vista web (color azul)
        public bool EsConsecutivoRangoHora { get; set; } //Varias intervenciones que tienen el mismo equipo, tipo de intervención, descripción y consecutivo (color verde)
        public bool TieneArchivo { get; set; }
        public bool TieneNota { get; set; }

        public string VistoBueno { get; set; } //Si estado de la intervencion es conforme tiene valor = "VBª"

        //para filtro
        public string Tipoevenabrev { get; set; } // Nombre Abreviado de Tipo de Evento
        public string EstadoRegistro { get; set; } // estado
        public string EmprNomb { get; set; } // Nombre de Empresa
        public string Operadornomb { get; set; } //Nombre del operador del equipo
        public string AreaNomb { get; set; } // Nombre de Area (Nombre de Ubicación)
        public string Famabrev { get; set; } // Nombre Abreviado de la familia o tipo de equipo
        public string Equiabrev { get; set; } // Nombre abreviado del equipo
        public string InterfechainiDesc { get; set; } // Cadena de Fecha y hora de Inicio Formateada
        public string InterfechafinDesc { get; set; } // Cadena de Fecha y hora de Fin Formateada
        public string InterindispoDesc { get; set; }   // Flag de Indisp.
        public string InterinterrupDesc { get; set; }  // Flag de Interrupción
        public string InterconexionprovDesc { get; set; } // Campo indicador  que indica si tiene conexión provisional
        public string IntersistemaaisladoDesc { get; set; } // Campo indicador  que indica si el sistema está aislado
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }

    public class IntervencionFila
    {
        public int Intercodi { get; set; } // Id de Intervención
        public int Progrcodi { get; set; }
        public int Equicodi { get; set; } // Id de Equipo

        public string Evenclasedesc { get; set; }
        public int Emprcodi { get; set; } // Cod Empresa
        public string EmprNomb { get; set; } // Nombre de Empresa
        public string EmprAbrev { get; set; } // Abreviatura de Empresa
        public string Operadornomb { get; set; } //Nombre del operador del equipo
        public string AreaNomb { get; set; } // Nombre de Area (Nombre de Ubicación)
        public string Tipoevenabrev { get; set; } // Nombre Abreviado de Tipo de Evento
        public string Famabrev { get; set; } // Nombre Abreviado de la familia o tipo de equipo
        public string Equiabrev { get; set; } // Nombre abreviado del equipo
        public string InterfechainiDesc { get; set; } // Cadena de Fecha y hora de Inicio Formateada
        public string InterfechafinDesc { get; set; } // Cadena de Fecha y hora de Fin Formateada
        public decimal Intermwindispo { get; set; } // MW Indisp.
        public string InterindispoDesc { get; set; }   // Flag de Indisp.
        public string InterinterrupDesc { get; set; }  // Flag de Interrupción
        public string Interdescrip { get; set; }  // Descripción de la Intervención (Mantenimiento)
        public string Interisfiles { get; set; }  // Flag tiene archivos.
        public string Intercodsegempr { get; set; }    // Código de seguimiento de empresa para registro masivo de la intervención (Para controlar la trazabilidad de las intervenciones en el horizonte establecido (Programación))
        public string InterconexionprovDesc { get; set; } // Campo indicador  que indica si tiene conexión provisional
        public string IntersistemaaisladoDesc { get; set; } // Campo indicador  que indica si el sistema está aislado
        public string Interjustifaprobrechaz { get; set; }  // Justificación u observación
        public string Internota { get; set; }  // Nota
        public int Interflagsustento { get; set; }  // Flag sustento

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public string UltimaModificacionUsuarioAgrupDesc { get; set; }

        public bool EstaFraccionado { get; set; }
        public bool EsConsecutivoRangoHora { get; set; }
        public bool ChkMensaje { get; set; }
        public bool ChkAprobacion { get; set; } //Flag que indica que tiene check de seleccion del listado
        public int TipoComunicacion { get; set; } //Tipo de comunicacion. 1: Ninguna, 2: TodoLeido, 3: AlgunNoLeido

        public string EstadoRegistro { get; set; }
        public string ClaseProgramacion { get; set; }
        public int Estadocodi { get; set; }  // Id de estado del registro de la intervención
        public int Interdeleted { get; set; }  // Flag indicador de registro borrado
        public int Interfuentestado { get; set; } //campo adicional para ver los estados de modificación
        public int Interprocesado { get; set; }    // Flag de registro procesado.

        //manejo de fecha hora para ejecutados
        public string IniFecha { get; set; }
        //public string IniHora { get; set; }
        //public string IniMinuto { get; set; }
        public string FinFecha { get; set; }
        //public string FinHora { get; set; }
        //public string FinMinuto { get; set; }
        public string IniHoraMinuto { get; set; }
        public string FinHoraMinuto { get; set; }

        #region Alertas Intervenciones

        public bool TieneAlertaHoraOperacion { get; set; }
        public bool TieneAlertaScada { get; set; }
        public bool TieneAlertaEms { get; set; }
        public bool TieneAlertaIDCC { get; set; }
        public bool TieneAlertaPR21 { get; set; }
        public bool TieneAlertaMedidores { get; set; }
        public bool TieneAlertaEstadoPendiente { get; set; }
        public int AlertaNoEjecutado { get; set; }



        #endregion
    }

    public class IntervencionRangoAnual
    {
        public int Progrcodi { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Incluido { get; set; }
    }

    public class IntervencionInputWeb
    {
        public int ProgrcodiReal { get; set; }
        public int Progrcodi { get; set; }
        public int TipoProgramacion { get; set; }

        public string InterFechaIni { get; set; }
        public string InterFechaFin { get; set; }

        public string Emprcodi { get; set; }
        public string TipoEvenCodi { get; set; }
        public string EstadoCodi { get; set; }
        public string AreaCodi { get; set; }
        public string FamCodi { get; set; }
        public string Equicodi { get; set; }

        public string InterIndispo { get; set; }

        public string Descripcion { get; set; }
        public string EstadoEliminado { get; set; }
        public string EstadoFiles { get; set; }
        public string EstadoMensaje { get; set; }
        public string EstadoNota { get; set; }
        public string TieneValidaciones { get; set; }

        public string CheckIntercodi { get; set; }

        //tablas gen,trans
        public string TipoReporte { get; set; }

        //cruzadas
        public string Subcausa { get; set; }
        public string ClaseProgramacion { get; set; }
        public string TipoGrupoEquipo { get; set; }
        public string HorasIndispo { get; set; }
        public string Maniobras { get; set; }
    }

    public class IntervencionFiltro
    {
        public int ProgrcodiReal { get; set; }
        public int Progrcodi { get; set; }
        public int Evenclasecodi { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        public List<int> ListaTipoevencodi { get; set; } = new List<int>();
        public List<int> ListaAreacodi { get; set; } = new List<int>();
        public List<int> ListaFamcodi { get; set; } = new List<int>();
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public List<int> ListaEmprcodi { get; set; } = new List<int>();
        public List<string> ListaInterIndispo { get; set; } = new List<string>();
        public List<int> ListaEstadocodi { get; set; } = new List<int>();
        public List<int> ListaIntercodiSel { get; set; } = new List<int>();
        public string Descripcion { get; set; }
        public List<int> ListaSubcausacodi { get; set; } = new List<int>();
        public List<int> ListaClaprocodi { get; set; } = new List<int>();

        public string StrIdsEmpresa { get; set; }
        public string StrIdsAreas { get; set; }
        public string StrIdsEquipos { get; set; }
        public string StrIdsTipoIntervencion { get; set; }
        public string StrIdsFamilias { get; set; }
        public string StrIdsDisponibilidad { get; set; }
        public string StrIdsEstados { get; set; }
        public string StrDescripcion { get; set; }

        public bool CheckOcultarEliminado { get; set; }
        public bool CheckMostrarConArchivo { get; set; }
        public bool CheckMostrarConMensaje { get; set; }
        public bool CheckMostrarConNota { get; set; }

        public string TipoGrupoFamilia { get; set; }
        public string FlagEqManiobra { get; set; }
        public string StrIdsSubcausa { get; set; }
        public string StrIdsClaprog { get; set; }

        public int TipoReporte { get; set; }
        public bool EsReporteExcel { get; set; }
        public bool AgruparIntervencion { get; set; }
        public int HoraIndisp { get; set; }
    }

    public class ReporteTIITRCuadro7
    {
        public int Item { get; set; }
        public string NombreAgente { get; set; }
        public string EnlaceIccp { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string FechaIniDesc { get; set; }
        public string FechaFinDesc { get; set; }
        public string ExcluirDesc { get; set; }
        public string Senial { get; set; }
        public string Motivo { get; set; }
        public string Ubicacion { get; set; }
        public string Equipo { get; set; }
        public int Equicodi { get; set; }

        public DateTime FechaIniExclusion { get; set; }
        public DateTime FechaFinExclusion { get; set; }
        public List<int> ListaCanalcodi { get; set; }
    }

    public class INNotificacionEmpresa
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public string EmprabrevNew { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime Emprfecmodificacion { get; set; }
    }

    public class ResultadoNotificacion
    {
        public int Result { get; set; }
        public List<string> ListaValidacion { get; set; }
    }

    public class CuadroResponsablesInforme
    {
        public string Elaboracion { get; set; }
        public string Revision { get; set; }
        public string Aprobacion { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
