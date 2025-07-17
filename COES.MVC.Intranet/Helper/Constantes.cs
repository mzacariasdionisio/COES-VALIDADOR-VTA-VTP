using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Helper
{
    /// <summary>
    /// Constantes utilizadas en la aplicacion
    /// </summary>
    public class Constantes
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaYYYYMMDD = "yyyy-MM-dd";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoHoraMinuto = "HH:mm";
        public const string FormatoMesAnio = "MM yyyy";
        public const string FormatoMesAnio2 = "yyyy-MM";
        public const string FormatoMesAnio3 = "MM/yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoFechaFullMs = "dd/MM/yyyy HH:mm:ss.fff";
        public const string FormatoFechaAnioMesDia = "yyyyMMdd";
        public const string FormatoMes = "MMMM";
        public const string FormatoOnlyHora = "HH:mm";
        public const string HoraInicio = "00:00:00";
        public const string FormatoNumero = "#,###.00";
        public const string FormatoDecimal = "###.00";
        public const int PageSize = 20;
        public const int PageSizeEvento = 50;
        public const int PageSizeMedidores = 200;
        public const int NroPageShow = 10;
        public const string HojaReporteExcel = "REPORTE";
        public const string HojaFormatoExcel = "FORMATO";
        public const string NombreReportePerturbacionWord = "Perturbacion.docx";
        public const string NombreReportePerturbacionPdf = "Perturbacion.pdf";
        public const string NombreReportePerfilScadaExcel = "Perfiles.xlsx";
        public const string NombrePlantillaImportacionPerfilExcel = "PlantillaImportacion.xlsx";
        public const string NombreFormatoImportacionPerfilExcel = "Importacion.xlsx";
        public const string NombreReporteRSF = "rsf.xlsx";
        public const string NombreReporteRSF30 = "rsf30.xlsx";
        public const string NombreReporteRSFGeneral = "rsf_reporte.xlsx";
        public const string PlantillaReportePerfilScadaExcel = "Plantilla.xlsx";
        public const string NombreCSVServicioRPF = "CumplimientoRPF.csv";
        public const string NombreLogoCoes = "coes.png";
        public const string PlantillaExcel = "Plantilla.xlsx";
        public const string PlantillaExcelRSF30 = "PlantillaRsf30.xlsx";
        public const string PlantillaExcelReporteRSF = "PlantillaReporteRSF.xlsx";
        public const string PlantillaCumplimiento = "PlantillaRPF.xlsx";
        public const string PlantillaCumplimientoFalla = "PlantillaRPFFalla.xlsx";
        public const string PlantillaPotencia = "PlantillaPotenciaRPF.xlsx";
        public const string NombreReporteCargaRPF = "Cumplimiento.xlsx";
        public const string NombreReporteCumplimientoRPF = "Evaluacion.xlsx";
        public const string NombreReporteCumplimientoRPFFalla = "EvaluacionFalla.xlsx";
        public const string ArchivoPotencia = "Potencia.xlsx";
        public const string ArchivoCmVsTarifa = "CmVsTarifa.xlsx";
        public const string ArchivoIncrementoReduccion = "IncrementoReduccion.xlsx";
        public const string ArchivoImportacionPerfiles = "ImportacionPerfil.xlsx";
        public const string ArchivoDesviacion = "Desviacion.xlsx";
        public const string NombreReporteRPFWord = "CumplimientoRPF.docx";
        public const string NombreReporteRPFFallaWord = "CumplimientoFallaRPF.docx";
        public const string NombreChartRPF = "ChartRPF.jpg";
        public const string NombreChartFallaRPF = "ChartFallaRPF.jpg";
        public const string NombreReporteRPFPotencia = "PotenciaMaximaRPF.xlsx";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppXML = "application/XML";
        public const string PaginaLogin = "home/login";
        public const string PaginaAccesoDenegado = "home/autorizacion";
        public const string DefaultControler = "Home";
        public const string LoginAction = "Login";
        public const string DefaultAction = "default";
        public const string RutaCarga = "Uploads/";
        public const int IdAplicacion = 1;
        public const string SI = "S";
        public const string NO = "N";
        public const string TextoSI = "Si";
        public const string TextoNO = "No";
        public const string TextoNoRango = "No rango";
        public const string TextoFPIncorrecto = "P o F en 0";
        public const string TextoNoEnvio = "No envío";
        public const string TextoNoSospechoso = "No sospechoso";
        public const string TextoRSFA = "RSF - A";
        public const string FormatoWord = "WORD";
        public const string FormatoPDF = "PDF";
        public const string AppZip = "application/zip";
        public const string FileZip = "Comprimido.zip";
        public const string AppGen = "application/dat";
        public const char CaracterComa = ',';
        public const char CaracterAnd = '&';
        public const char CaracterArroba = '@';
        public const string Coma = ",";
        public const string AperturaSerie = "[";
        public const string CierreSerie = "]";
        public const string EspacioBlanco = " ";
        public const char CaracterGuion = '-';
        public const string CaracterPorcentaje = "%";
        public const string CaracterCero = "0";
        public const char CaracterDosPuntos = ':';
        public const string CaracterH = "H";
        public const string CaracterComillaSimple = "'";
        public const char CaracterRaya = '_';
        public const char CaracterPunto = '.';
        public const char CaracterIgual = '=';
        public const string LogError = "INTRANET.COES";
        public const string NodoPrincipal = "Principal <span>/</span>";
        public const string SeparacionMapa = "<span>/</span>";
        public const string SufijoImagenUser = "@coes.org.pe.jpg";
        public const string UsuarioAnonimo = "usuario.anonimo";
        public const string ClaveAnonimo = "sgoCOES2014";
        public const string TextoCentral = "CENTRAL";
        public const string TextoMW = "MW";
        public const string ParametroDefecto = "-1";
        public const string Opero = "OPERÓ";
        public const string NoOpero = "NO OPERÓ";
        public const string Indeterminado = "INDETERMINADO";
        public const string ExtensionGif = "gif";
        public const string ExtensionJpg = "jpg";
        public const string ExtensionPng = "png";
        public const string ColorPlomo = "#CCCCCC";
        public const string NombreReporteMantenimiento = "RptMantenimiento.xlsx";
        public const string NombreReporteMantenimiento01 = "RptMantenimiento_01.xlsx";
        public const string NombreReporteMantenimiento02 = "RptMantenimiento_02.xlsx";
        public const string NombreReporteMantenimiento03 = "RptMantenimiento_03.xlsx";
        public const string NombreReporteMantenimiento04 = "RptMantenimiento_04.xlsx";
        public const string NombreReporteMantenimiento05 = "RptMantenimiento_05.xlsx";
        public const string NombreReporteMantenimiento06 = "RptMantenimiento_06.xlsx";
        public const string PlantillaExcelMantenimiento = "PlantillaMantenimiento.xlsx";
        public const int IdModulo = 3;
        public const int AccionEditar = 0;
        public const int AccionNuevo = 1;
        public const string EstadoActivo = "A";
        public const string EstadoEliminado = "E";
        public const string EstadoBaja = "B";
        public const string PlantillaCostosVariablesRepCv = "PlantillaCostosVariablesRepcv.xlsx";
        public const string PlantillaReporteCostosVariablesRepCv = "PlantillaReporteCostosVariables.xlsx";
        public const string PlantillaListadoEquipos = "PlantillaListaEquipos.xlsx";
        public const string PlantillaListadoPropiedadesEquipos = "PlantillaListaPropiedades.xlsx";
        public const string PlantillaReportePropiedadesEquipos = "PlantillaListaPropiedadesEquipo.xlsx";

        public const int PageSizeDemandaMaxima = 30;
        public const string RutaCargaIntranet = "IntranetTest/";
        public const string RutaCargaIntercambio = "IntercambioOsinergmin/";
        public const string InfografiaPortal = "INFOGRAFIA_{0}.";
        public const string MensajeSesionExpirado = "Su sesión ha caducado. Vuelva a iniciar sesión";
        public const string MensajeAccesoNoPermitido = "No tiene Acceso a está opción";
        public const string MensajeModuloNoPermitido = "Usted no tiene permisos para acceder a este módulo.";
        public const string MensajeOpcionNoPermitido = "Usted no tiene permisos para acceder a esta opción.";
        public const string MensajePermisoNoValido = "Usted no tiene permisos para realizar esta acción.";
        public const string MensajeFormularioNoValido = "La información enviada no es válida.";
        public const string FormatoF = "dd_MM_yyyy";
        public const int EstacionHidrologica = 43;

        //GMME
        public const int tipoEnergia = 3;
        public const int tipoDemanda = 20;
        public const int formatoEnergiaTrimestral = 102;
        public const int formatoEnergiaMensual = 102;
        public const int formatoDemandaTrimestral = 118;
        public const int formatoDemandaMensual = 118;
        public const int formatoEnergiaEntregaTrimestral = 129;
        public const int formatoEnergiaEntregaMensual = 129;
        public readonly static string FileSystemSco = ConfigurationManager.AppSettings["FileSystemSco"].ToString();
        public readonly static string FileSystemPortal = ConfigurationManager.AppSettings["FileSystemPortal"].ToString();
        public readonly static string FileSystemSev = ConfigurationManager.AppSettings["FileSystemSev"].ToString();
        public readonly static string FechaFinSem1 = ConfigurationManager.AppSettings["FechaFinSem1"].ToString();
        public readonly static string FechaInicioSem2 = ConfigurationManager.AppSettings["FechaInicioSem2"].ToString();
        public readonly static string FechaFinSem2 = ConfigurationManager.AppSettings["FechaFinSem2"].ToString();
        public readonly static int UserRecomendaciones = Convert.ToInt32(ConfigurationManager.AppSettings["UserRecomendaciones"]);
        public readonly static int CriticidadRec = Convert.ToInt32(ConfigurationManager.AppSettings["CriticidadRec"]);
        public readonly static int EstadoRec = Convert.ToInt32(ConfigurationManager.AppSettings["EstadoRec"]);
        public readonly static int PlazoNotiRec = Convert.ToInt32(ConfigurationManager.AppSettings["PlazoNotiRec"]);
        public readonly static int IdPlantilla = Convert.ToInt32(ConfigurationManager.AppSettings["IdPlantilla"]);
        public readonly static string CarpetaDeFirmas = ConfigurationManager.AppSettings["CarpetaDeFirmas"].ToString();
        public const int PageSizeRecomendacion = 330;

        //Mejoras Ficha Técnica
        public const string UrlFileAppPortal = "UrlFileAppPortal";
        public const string UrlFileAppInformacionOperativa = "UrlFileAppInformacionOperativa";
        public const string UrlFileAppFichaTecnica = "UrlFileAppFichaTecnica";
        public const string wsIndSupervision = "wsIndSupervision";

        public readonly static int Limctaf = Convert.ToInt32(ConfigurationManager.AppSettings["Limctaf"]);
        public readonly static int Limit = Convert.ToInt32(ConfigurationManager.AppSettings["Limit"]);

        public readonly static int CodigoCorreoAnalisisFallaAlertaCitacion = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAnalisisFallaAlertaCitacion"]);
        public readonly static int CodigoCorreoAlertaElaboracionInformeCtaf = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtaf"]);
        public readonly static int CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles"]);
        public readonly static int CodigoCorreoAlertaElaboracionInformeTecnico = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnico"]);
        public readonly static int CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles"]);
        public readonly static int CodigoCorreoAlertaElaboracionInformeTecnicoSemanal = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoSemanal"]);

        public const string IndDel = "0";
        public const string IndDelEliminado = "1";
        public const string EstadoRegistrado = "Registrado";
        public const string EstadoEnviado = "Enviado";
        public const string EstadoObservado = "Observado";
        public const string EstadoCerrado = "Cerrado";
        public const string Activo = "1";
        public const string Cerrado = "0";
        public const string Todos = "T";

        //Parametria
        public const int EstadoPeriodo = 1;
    }

    /// <summary>
    /// Constantes para los nombres de los parametros query string
    /// </summary>
    public class RequestParameter
    {
        public const string EventoId = "id";
        public const string Indicador = "ind";
        public const string PerfilId = "id";
    }

    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesion
    {
        public const string SesionPerturbacion = "SesionPerturbacion";
        public const string SesionUsuario = "SesionUsuarioIntranet";
        public const string SesionIdOpcion = "SesionIdOpcion";
        public const string SesionOpciones = "SesionOpcionesIntranet";
        public const string SesionIndPermiso = "SesionIndPermiso";
        public const string SesionNodo = "SesionNodo";
        public const string SesionPermiso = "SesionPermiso";
        public const string SesionEmpresa = "SesionEmpresa";
        public const string ListaFormulas = "ListaFormulas";
        public const string ListaScada = "ListaScada";
        public const string ListaTotal = "ListaTotal";
        public const string EntidadScada = "EntidadScada";
        public const string ListaGrabado = "ListaGrabado";
        public const string UltimoPerfil = "UltimoPerfil";
        public const string ListaServicio = "ListaServicio";
        public const string ListaEvaluacion = "ListaEvaluacion";
        public const string ListaConfiguracion = "ListaConfiguracion";
        public const string ListaRangoNoEncontrado = "ListaRangoNoEncontrado";
        public const string ListaNoCargaron = "ListaNoCargaron";
        public const string ListaNoIncluir = "ListaNoIncluir";
        public const string ListaPotencia = "ListaPotencia";
        public const string ListaGrafico = "ListaGrafico";
        public const string ListaDatosFalla = "ListaDatosFalla";
        public const string ListaFrecuenciaFalla = "ListaFrecuenciaFalla";
        public const string ListaFrecuenciaFallaTotal = "ListaFrecuenciaFallaTotal";
        public const string ListaPotenciaFalla = "ListaPotenciaFalla";
        public const string ListaEquipos = "ListaEquipos";
        public const string IndicadorEvaluacion = "IndicadorEvaluacion";
        public const string NumeroSegundos = "NumeroSegundos";
        public const string ListaPuntoFormula = "ListaPuntoFormula";
        public const string FechaProceso = "FechaProceso";
        public const string IndicadorFuente = "IndicadorFuente";
        public const string DatosImportados = "DatosImportados";
        public const string ListaGraficoMaximo = "ListaGraficoMaximo";
        public const string ListaGraficoMinimo = "ListaGraficoMinimo";
        public const string NombrePlantillaImportacion = "NombrePlantillaImportacion";
        public const string ListaIncorrecto = "ListaIncorrecto";
        public const string SesionMapa = "SesionMapa";
        public const string InicioSesion = "InicioSesion";
        public const string ListaFuenteEnergia = "ListaFuenteEnergia";
        public const string ReporteEmpresas = "ReporteEmpresas";
        public const string ListaTipoGeneracion = "ListaTipoGeneracion";
        public const string ListaReporteConsistencia = "ListaReporteConsistencia";
        public const string ListaManttoEmpresa = "ListaManttoEmpresa";
        public const string ListaManttosTotal = "ListaManttosTotal";
        public const string ValorMaximaDemanda = "ValorMaximaDemanda";
        public const string ListaOrdenadaDemanda = "ListaOrdenadaDemanda";
        public const string FechaMaximaDemanda = "FechaMaximaDemanda";
        public const string HoraMaximaDemanda = "HoraMaximaDemanda";
        public const string ListaDiagramaCarga = "ListaDiagramaCarga";
        public const string ListaSerieDiagramaCarga = "ListaSerieDiagramaCarga";
        public const string ListaValidacionMedidores = "ListaValidacionMedidores";
        public const string FechaHoraMD96 = "FechaHoraMD96";
        public const string FechaHoraMD48 = "FechaHoraMD48";
        public const string ListaEmpresa = "ListaEmpresa";
        public const string FechaConsultaInicio = "FechaConsultaInicio";
        public const string FechaConsultaFin = "FechaConsultaFin";
        public const string FileInfografia = "FileInfografia";
        public const string FiltroInformeFalla = "FiltroInformeFalla";
        public const string SesionListaProcesar = "SesionListaProcesar";
        public const string SesionFileName = "FileName";
        public const string SesionFechaProceso = "SesionFechaProceso";

        public const string NroRevisiones = "NroRevisiones";
    }

    /// <summary>
    /// Contiene las rutas de los directorios utilizados
    /// </summary>
    public class RutaDirectorio
    {
        public const string RutaCargaFileTransferencia = "RutaCargaFileTransferencia";
        public const string ReportePropiedades = "ReportePropiedades";
        public const string ReporteEvento = "ReporteEvento";
        public const string ReportePerfiles = "ReportePerfiles";
        public const string ReporteServicioRPF = "ReporteServicioRPF";
        public const string DirectorioReporteCircular = "DirectorioReporteCircular";
        public const string ArchivoReporteCircularZip = "ArchivoReporteCircularZip";
        public const string ReporteEnvioCorreo = "ReporteEnvioCorreo";
        public const string RutaCargaInformeEvento = "Areas/Eventos/Reporte/";
        public const string RutaExportacionInformeEvento = "RutaExportacionInformeEvento";
        public const string RutaCargaEvento = "RutaCargaEvento";
        public const string FileLogoEvento = "Logos/";
        public const string ReporteExcel = "ReporteExcel";
        public const string ReporteInterconexion = "ReporteInterconexion";
        public const string ReporteDemandaBarras = "ReporteDemandaBarras";
        public const string ReporteGestionEoEpo = "ReporteGestionEoEpo";
        public const string RutaCargaFile = "RutaCargaFile";
        public const string RolRepresentanteLegal = "RolRepresentanteLegal";
        public const string ReporteDespacho = "ReporteDespacho";
        public const string RutaExportacionAdmin = "Areas/Admin/Reporte/";
        public const string RutaExportacionDespacho = "Areas/Despacho/Reporte/";
        public const string RutaExportacionEventos = "Areas/Eventos/Reporte/";
        public const string RutaExportacionWeb = "Areas/Web/Reporte/";
        public const string RutaExportacionHidrologia = "Areas/Hidrologia/Reporte/";
        public const string DirectorioVolumenCombustible = "Areas\\StockCombustibles\\Reporte\\";
        public const string RutaDefecto = "Principal/";
        public const string PathLogo = @"Content\Images\logocoes.png";
        public const string PathLogo30 = @"Content\Images\logocoes30.png";
        public const string PathLogoIntervenciones = @"Content\Images\logocoesintervenciones.png";
        public const string RutaServicioRpf = "Areas/ServicioRPF/Reporte/";
        public const string RutaCargaInformeTiempoReal = "Areas/TiempoReal/Reporte/";
        public const string ReporteEquipamiento = "ReporteEquipamiento";
        public const string RutaInfografia = "RutaInfografia";
        public const string RutaInformeReservaNodo = "Areas/ReservaFriaNodoEnergetico/Reporte/";
        public const string RutaExportacionCostoMarginal = "Areas/CortoPlazo/Reporte/";
        public const string RutaReprocesoCostosMarginales = "PathReprocesoCostosMarginales";
        public const string RutaReprocesoCostosMarginalesTNA = "PathReprocesoCostosMarginalesTNA";
        public const string DirectorioReportePR5 = "Areas\\PR5Reportes\\Reporte\\";
        public const string DirectorioIndisponibilidad = "Areas\\Indisponibilidades\\Reporte\\";
        public const string ReporteRechazoCarga = "ReporteRechazoCarga";
        public const string DirectorioReporteEquivalencia = "Areas\\IEOD\\Reporte\\";
        public const string DirectorioDocumentoIEOD = "Areas\\IEOD\\Documentos\\";
        public const string ReporteEnsayo = "ReporteEnsayos";
        public const string DirectorioDespacho = "Areas\\Despacho\\Reporte\\";
        public const string InitialUrl = "InitialUrl";

        public const string DevolucionAporteArchivos = "DevolucionAporteArchivos";

        public const string RutaFacturas = "RutaFacturas";
        public const string RutaDevoluciones = "RutaDevoluciones";
        //FIT
        public const string ReporteValorizacionDiaria = "ReporteValorizacionDiaria";
        /// Constante Yupana
        public const string DirectorioReporteYupana = "Areas\\Yupana\\Reporte\\";
        //Potencia firme
        public const string RutaCargaFilePotenciaFirme = "Areas/PotenciaFirme/Reporte/";
        //firma coes intervenciones
        public const string PathFirmaIntervenciones = @"Content\Images\modulo_intervenciones_sello_subdirector.png";
        public const string PathFirmaIntervencionesBlanco = @"Content\Images\modulo_intervenciones_sello_subdirector_blanco.png";
    }

    /// <summary>
    /// Contiene los nombres de propiedades de clases
    /// </summary>
    public class EntidadPropiedad
    {
        public const string PtoMediCodi = "PtoMediCodi";
        public const string PtoNomb = "PtoDescripcion";
        public const string Areacodi = "Areacodi";
        public const string AreaNomb = "AreaNomb";
        public const string EmprCodi = "EMPRCODI";
        public const string EmprNomb = "EMPRNOMB";
        public const string EquiCodi = "EquiCodi";
        public const string EquiNomb = "EquiNomb";
        public const string Emprcodi = "Emprcodi";
        public const string Emprnomb = "Emprnomb";
        public const string Equicodi = "Equicodi";
        public const string Equinomb = "Equinomb";
        public const string Tipoptomedicodi = "Tipoptomedicodi";
        public const string Tipoptomedinomb = "Tipoptomedinomb";
        public const string FamCodi = "FamCodi";
        public const string FamNomb = "FamNomb";
    }

    public class EntidadComite
    {
        public const string ComiteListaCodi = "COMITELISTACODI";
        public const string ComiteListaName = "COMITELISTANAME";
    }

    /// <summary>
    /// Contiene los nombres de los items del reporte de perturbacion
    /// </summary>
    public class TipoItemPerturbacion
    {
        public const string ItemFecha = "RP_FECHA";
        public const string ItemHora = "RP_HORA";
        public const string ItemEquipo = "RP_EQUIPO";
        public const string ItemEmpresa = "RP_EMPRESA";
        public const string ItemCausa = "RP_CAUSA";
        public const string ItemDescripcion = "RP_DESCRIPCION";
        public const string ItemSecuencia = "RP_SECUENCIA";
        public const string ItemActuacion = "RP_ACTUACION";
        public const string ItemAnalisis = "RP_ANALISIS";
        public const string ItemConclusion = "RP_CONCLUSION";
        public const string ItemRecomendacion = "RP_RECOMENDACION";
        public const string ItemOportunidad = "RP_OPORTUNIDAD";
        public const string ItemAcuerdo = "RP_ACUERDO";
        public const string ItemPlazo = "RP_PLAZO";
    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class NombreArchivo
    {
        public const string PlantillaGeneracionRERDiario = "PlantillaGeneracionRERDiario.xlsx";
        public const string PlantillaGeneracionRERSemanal = "PlantillaGeneracionRERSemanal.xlsx";
        public const string PlantillaConsistenciaRPF = "PlantillaConsistencia.xlsx";
        public const string ReporteGeneracionRER = "ReporteRER.xlsx";
        public const string ReporteCumplimientoRER = "CumplimientoRER.xlsx";
        public const string ReporteMedidoresHorizontal = "ReporteMedidores_Horizontal.xlsx";
        public const string ReporteMedidoresVertical = "ReporteMedidores_Vertical.xlsx";
        public const string ReporteMedidoresCSV = "ReporteMedidores.csv";
        public const string ReporteMedidoresGeneracion = "ReporteMedidoresGeneracion.xlsx";
        public const string ReporteConsistenciaRPF = "ReporteConsistenciaRPF.xlsx";
        public const string ReporteFrecuenciasRPF = "FrecuenciasRPF.xlsx";
        public const string PlantillaFrecuenciaRPF = "PlantillaFrecuencia.xlsx";
        public const string ReporteEvento = "Eventos.xlsx";
        public const string ReporteMaximaDemandaDiaria = "MaximaDemandaDiaria.xlsx";
        public const string ReporteMaxinaDemandaHFPHP = "MaximaDemandaHFPHP.xlsx";
        public const string ReporteRankingDemanda = "RankingDemanda.xlsx";
        public const string ReporteDuracionCarga = "DuracionCarga.xlsx";
        public const string ReporteConsolidoMensual = "ConsolidadoMensual.xlsx";
        public const string ReporteValidacionMedidores = "ValidacionDatos.xlsx";
        public const string ReporteStockCombustible = "StockCombustible.xlsx";
        public const string FormatoInformePDF = "INFORME.pdf";
        public const string FormatoInformeWORD = "INFORME.docx";
        public const string LogoEmpresa = "LOGOEMPRESA_{0}.{1}";
        public const string FormatoCargaInterrupcion = "CargaInterrupcion.xlsx";
        public const string RangoHorasRPF = "RangoHoras.xlsx";
        public const string ReporteInterrupciones = "Interrupciones.xlsx";
        public const string ReporteVerticalCMgReal = "ReporteVerticalCMgReal.xlsx";
        public const string ReporteHorizontalCMgReal = "ReporteHorizontalCMgReal.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string ReporteCostosVariablesRepCv = "CostosVariables.xlsx";
        public const string ReporteOficialCostosVariablesRepCv = "ReporteCostosVariables.xlsx";
        public const string ReporteOfertasDiarias = "ReporteOfertas.xlsx";
        public const string ReporteReservaSecundaria = "ReporteReservaSecundaria.xlsx";
        public const string ReporteComparacionRPF = "ComparativoRPF.xlsx";
        public const string ReporteRpfMedioHorario = "RPFMedioHorario.xlsx";
        public const string ReporteEnvíoCorreo = "EnvíoCorreo.xlsx";
        public const string ReporteXMLDespachoHidro = "UnitScheduledLoad_Hydro.xml";
        public const string ReporteXMLDespachoTermo = "UnitScheduledLoad_Thermal.xml";
        public const string ReporteXMLValorAgua = "watervcp.xml";
        public const string ReporteXMLPActivaHidro = "resaghcp.xml";
        public const string ReporteXMLPActivaTermo = "resagtcp.xml";
        public const string ReporteXMLCCOMB = "CCOMB.xml";
        public const string ReporteXMLCVC = "CVC.xml";
        public const string ReporteXMLCVNC = "CVNC.xml";
        public const string ReporteXMLMantoHydro = "Hydro.xml";
        public const string ReporteXMLMantoThermo = "Thermo.xml";
        public const string ReporteXMLMantoBanco = "Banco.xml";
        public const string ReporteXMLMantoReactor = "Reactor.xml";
        public const string ReporteXMLMantoCS = "CS.xml";
        public const string ReporteXMLMantoSVC = "SVC.xml";
        public const string ReporteFrecuencia = "ReporteFrecuencia.xlsx";
        public const string ReporteCircular = "ReporteCircular.xlsx";
        public const string ReporteCircularCSV = "ReporteCircular.csv";
        public const string ReporteCircularZIP = "ReporteCircular.zip";
        public const string ReporteSCADA = "ReporteSCADA.xlsx";
        public const string ReporteEquipos = "Equipos.xlsx";
        public const string ReportePropiedad = "ReportePropiedad.xlsx";
        public const string ReporteAuditoriaCambio = "ReporteAuditoriaCambio.xlsx";

        public const string ReporteOperaciones = "OperacionesVarias.xlsx";
        public const string ProgramaCCOMB = "ResultingFuelCost";
        public const string ProgramaCVC = "ResultingFuelCost";
        public const string ProgramaCVNC = "IncrementalMaintenanceCost";
        public const string ReporteNodoEnergetico = "NodoEnergetico.xlsx";
        public const string ReporteReservaFria = "ReservaFria.xlsx";
        public const string ArchivoRawCM = "psse_reproceso.raw";
        public const string PlantillaEvaFrecuenciaXLS = "Eva_Frecuencia.xlsm";
        public const string ReporteEvaFrecuenciaXLS = "Eva_Frecuencia_{0}.xlsm";

        public const string ReporteSeguimientoRec = "Seguimiento.xlsx";
        public const string ReporteSeguimientoRecReporte = "SeguimientoReporte.xlsx";
        public const string ReporteDatosYupana = "ReporteYUPANA.xlsx";

        public const string ReporteURSCalificadas = "ReporteURSCalif.xlsx";
        public const string ReporteURSBase = "ReporteURSBase.xlsx";
        public const string ReporteResumenYupana = "ReporteResumenYupana.xlsx";
        public const string ReporteYupanaDescarga = "Hoja_Resumen_{0}.xlsx";
        public const string ReporteIndicadoresCTAF = "IndicadoresCTAF.xlsx";
        public const string ReporteCriteriosCTAF = "CriteriosCTAF.xlsx";
    }

    /// <summary>
    /// Tipos de eventos
    /// </summary>
    public class TipoEventos
    {
        public const int EventoPruebas = 6;
    }

    /// <summary>
    /// Textos utilizados en pantalla
    /// </summary>
    public class TextosPantalla
    {
        public const string Todos = "-TODOS-";
        public const string Hora = "Hora";
        public const string Frecuencia = "Frecuencia";
        public const string Potencia = "Potencia";
    }

    /// <summary>
    /// Puntos de medicion
    /// </summary>
    public class PuntoMedicion
    {
        public const int CodRef = -1;
        public const int TipoInfoCodi = 0;
        public const string EstadoActivo = "A";
    }

    /// <summary>
    /// Modulos del sistema
    /// </summary>
    public class Modulos
    {
        public const int AppHidrologia = 3;
        public const int AppMedidoresDistribucion = 4;
        public const int AppDemandaCP = 5;
    }

    /// <summary>
    /// Contiene los nombres de atributos del webconfig
    /// </summary>
    public class DatosConfiguracion
    {
        public const string MailFrom = "MailFrom";
        public const string RolInterconexion = "RolInterconexion";
        public const string RolIEOD = "RolIEOD";
        public const string RolPr16 = "RolPr16";
        public const string IdAplicacionExtranet = "IdAplicacionExtranet";
    }
}