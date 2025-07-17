using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Helper
{
    public class Constantes
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaISO = "yyyy-MM-dd";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoHoraMinuto = "HH:mm";
        public const string FormatoMesAnio = "MM yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm:ss";
        public const string HoraInicio = "00:00:00";
        public const string FormatoNumero = "#,###.00";
        public const string FormatoDecimal = "###.00";
        public const int PageSize = 20;
        public const int PageSizeBusqueda = 30;
        public const int PageSizeEvento = 50;
        public const int PageSizeMedidores = 200;
        public const int NroPageShow = 10;
        public const int RolCargaArchivos = 96;
        public const int RolDirectorioImpugnacion = 103;
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string FileZip = "Comprimido.zip";
        public const string AppZip = "application/zip";
        public const string PaginaLogin = "home/login";
        public const string PaginaAccesoDenegado = "home/autorizacion";
        public const string DefaultControler = "Home";
        public const string ActionAutorizacion = "autorizacion";
        public const string ControladorHome = "home";
        public const string LoginAction = "Login";
        public const string DefaultAction = "default";
        public const string RutaCarga = "Uploads/";
        public const int IdAplicacion = 1;
        public const string SI = "S";
        public const string NO = "N";
        public const string FormatoWord = "WORD";
        public const string FormatoPDF = "PDF";
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
        public const string CaracterPunto = ".";
        public const char CaracterDosPuntos = ':';
        public const string CaracterH = "H";
        public const string CaracterComillaSimple = "'";
        public const string LogError = "Publico.COES";
        public const string NodoPrincipal = "Principal <span>/</span>";
        public const string SeparacionMapa = "<span>/</span>";
        public const string NombreLogoCoes = "coes-logo.png";
        public const string TextoMW = "MW";
        public const string ParametroDefecto = "-1";
        public const string NombreReporteMantenimiento = "RptMantenimiento.xlsx";
        public const string NombreReporteMantenimiento01 = "RptMantenimiento_01.xlsx";
        public const string NombreReporteMantenimiento02 = "RptMantenimiento_02.xlsx";
        public const string NombreReporteMantenimiento03 = "RptMantenimiento_03.xlsx";
        public const string NombreReporteMantenimiento04 = "RptMantenimiento_04.xlsx";
        public const string NombreReporteMantenimiento05 = "RptMantenimiento_05.xlsx";
        public const string NombreReporteMantenimiento06 = "RptMantenimiento_06.xlsx";
        public const string PlantillaExcelMantenimiento = "PlantillaMantenimiento.xlsx";
        public const string EmailOportunidades = "EmailOportunidades";
        public const string EmailWebApp = "webapp@coes.org.pe";
        public const string SubjectOportunidades = "CV - Oportunidades de Trabajo - Portal Web COES";
        public const string SubjectOportunidadesRespuesta = "Oportunidades de Trabajo COES - Envío exitoso";
        public const string OrdenamientoAscendente = "A";
        public const string OrdenamientoDescendente = "D";
        public const string VisorISSUU = "I";
        public const string VisorPDF = "V";
        public const string VisorEspecial = "C";
        public const string TextoInicial = "Inicial";
        public const string RequestPath = "path";
        public const string EstadoActivo = "A";
        public const string InfografiaPortal = "INFOGRAFIA_{0}.";
        public const string FormatoF = "dd_MM_yyyy";
    }



    /// <summary>
    /// Constantes para los nombres de los parametros query string
    /// </summary>
    public class RequestParameter
    {
        public const string Indicador = "ind";
        public const string RequestUser = "user";
    }

    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesion
    {
        public const string SesionUsuario = "SesionUsuario";
        public const string SesionIdOpcion = "SesionIdOpcion";
        public const string SesionOpciones = "SesionOpciones";
        public const string SesionIndPermiso = "SesionIndPermiso";
        public const string SesionNodo = "SesionNodo";
        public const string SesionPermiso = "SesionPermiso";
        public const string SesionEmpresa = "SesionEmpresa";
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
        public const string ListaFrecuencia = "ListaFrecuencia";
    }

    /// <summary>
    /// Contiene las rutas de los directorios utilizados
    /// </summary>
    public class RutaDirectorio
    {
        public const string ReporteEvento = "ReporteEvento";
        public const string ReporteMediciones = "ReporteMediciones";
        public const string PronosticoTempoRealDirectorio = "PronosticoTempoRealDirectorio";
        public const string PronosticoTempoRealArchivoData = "PronosticoTempoRealArchivoData";
        public const string PronosticoTempoRealArchivoFecha = "PronosticoTempoRealArchivoFecha";
        public const string DirectorioImpugnaciones = "Directorio e Impugnaciones/Administrador/Impugnaciones/";
        public const string DirectorioEventos = "Directorio e Impugnaciones/Administrador/Eventos/";
        public const string ReporteTransferencias = "Areas/Operacion/Reporte/";
        public const string DirectorioNotasTecnicas = "Marco Normativo/Administrador/";
        public const string PathLogo = @"Content\Images\logocoes.png";

    }


    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class NombreArchivo
    {
        public const string ReporteMedidoresHorizontal = "ReporteMedidores_Horizontal.xlsx";
        public const string ReporteMedidoresVertical = "ReporteMedidores_Vertical.xlsx";
        public const string ReporteMedidoresCSV = "ReporteMedidores.csv";
        public const string ReporteMedidoresGeneracion = "ReporteMedidoresGeneracion.xlsx";
        public const string ReporteMaximaDemandaDiaria = "MaximaDemandaDiaria.xlsx";
        public const string ReporteMaxinaDemandaHFPHP = "MaximaDemandaHFPHP.xlsx";
        public const string ReporteRankingDemanda = "RankingDemanda.xlsx";
        public const string ReporteDuracionCarga = "DuracionCarga.xlsx";
        public const string ReporteConsolidoMensual = "ConsolidadoMensual.xlsx";
        public const string ReporteValidacionMedidores = "ValidacionDatos.xlsx";
        public const string ReporteEjecutadoDiario = "EjecutadoDiario.xlsx";
        public const string ReporteEjecutadoAcumuladoMensual = "EjecutadoAcumulado.xlsx";
        public const string ReporteDemandaBarras = "DemandaBarras.xlsx";
        public const string ReporteEvento = "Eventos.xlsx";
        public const string ReportePrecioCombustible = "PrecioCombustible.xlsx";
        public const string ReporteGeneracionPortal = "GeneracionCOES.xlsx";
        public const string ReporteMedicionPortal = "DemandaCOES.xlsx";
        public const string ReporteHidrologiaPortal = "HidrologiaCOES.xlsx";
        public const string ReporteCostoMarginalNodal = "CostosMarginalesNodales.xlsx";
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
        public const string Tipoptomedicodi = "Tipoptomedicodi";
        public const string Tipoptomedinomb = "Tipoptomedinomb";
    }
}