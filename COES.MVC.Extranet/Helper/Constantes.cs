using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Helper
{
    /// <summary>
    /// Constantes generales del aplicativo
    /// </summary>
    public class Constantes
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const string FormatoFechaFull = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoMesAnio = "MM yyyy";
        public const string FormatoOnlyHora = "HH:mm";
        public const int PageSize = 20;
        public const int NroPageShow = 10;               
        public const string HojaReporteExcel = "REPORTE";
        public const string HojaFormatoExcel = "FORMATO";
        public const string NombreLogoCoes = "coes.png";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppZip = "application/zip";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string PaginaLogin = "/WebForm/Account/Login.aspx";
        public const string PaginaAccesoDenegado = "/home/autorizacion";
        public const string DefaultControler = "Home";
        public const string LoginAction = "Login";
        public const string DefaultAction = "default";        
        public const string SI = "S";
        public const string NO = "N";
        public const string FormatoWord = "WORD";
        public const string FormatoPDF = "PDF";
        public const char CaracterComa = ',';
        public const string AperturaSerie = "[";
        public const string CierreSerie = "]";
        public const char CaracterCero = '0';
        public const char CaracterPunto = '.';
        public const char CaracterArroba = '@';
        public const string TextoCentral = "CENTRAL";
        public const string TextoMW = "MW";
        public const string CaracterH = "H";
        public const string ExtensionGif = "gif";
        public const string ExtensionJpg = "jpg";
        public const string ExtensionPng = "png";
        public const int EstacionHidrologica = 43;
        public const string MensajeEmpresaNoVigente = "La empresa se encuentra No Vigente";
        public const string MensajeSesionExpirado = "Su sesión ha caducado. Vuelva a iniciar sesión";
        public const string MensajeAccesoNoPermitido = "No tiene Acceso a está opción";
        public const string MensajeModuloNoPermitido = "Usted no tiene permisos para acceder a este módulo";
        public const string MensajePermisoNoValido = "Usted no tiene permisos para realizar esta acción";
        public const string MensajeFormularioNoValido = "La información enviada no es válida.";
        
        public const string AgrupacionVTP = "AGRVTP";
        public const string AgrupacionVTA = "AGRVTA";

        public const string IndDel = "0";
        public const string EstadoRegistrado = "Registrado";
        public const string EstadoEnviado = "Enviado";
        public const string EstadoObservado = "Observado";
        public const string EstadoAbsuelto = "Absuelto";
        public const string CumplimientoInicial = "";
        public const int VersionInicial = 0;
        public readonly static string FileSystemSco = ConfigurationManager.AppSettings["FileSystemSco"].ToString();
        public readonly static string InitialUrl = ConfigurationManager.AppSettings["InitialUrl"].ToString();

        #region GMM
        public const int formatoEnergiaTrimestral = 102;
        public const int tipoEnergia = 3;
        public const int tipoDemanda = 20;
        public const int formatoEnergiaMensual = 102;
        #endregion
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
        public const string SesionUsuario = "SesionUsuarioExtranet";
        public const string SesionIdOpcion = "SesionIdOpcionExtranet";
        public const string SesionOpciones = "SesionOpcionesExtranet";
        public const string SesionIndPermiso = "SesionIndPermisoExtranet";
        public const string SesionNodo = "SesionNodoExtranet";
        public const string SesionPermiso = "SesionPermisoExtranet";
        public const string SesionEmpresa = "SesionEmpresaExtranet";
        public const string UltimoPerfil = "UltimoPerfilExtranet";
        public const string SesionNombreArchivo = "SesionNombreArchivoExtranet";
        public const string SesionListaProcesar = "SesionListaProcesarExtranet";
        public const string SesionFileName = "FileNameExtranet";
        public const string SesionValidacionDatos = "SesionValidacionDatosExtranet";
        public const string ListaEquipos = "ListaEquiposExtranet";
        public const string ListaEmpresa = "ListaEmpresaExtranet";
        public const string SesionIdEnvio = "SesionIdEnvioExtranet";
        public const string SesionFechaProceso = "SesionFechaProcesoExtranet";
        public const string SesionListaErrores = "SesionListaErroresExtranet";
        public const string UserRepresentante = "UserRepresentanteExtranet";
        public const string SesionPuntosMedicion = "SesionPuntosMedicionExtranet";
        //Assetec - COES.Api.Seguridad
        public const string SesionTokenApiSeguridad = "SesionTokenApiSeguridad";
        public const string SesionTokenRefreshApiSeguridad = "SesionTokenRefreshApiSeguridad";
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

    /// <summary>
    /// Contiene las rutas de los directorios utilizados
    /// </summary>
    public class RutaDirectorio
    {
        public const string ReporteGeneracionRER = "ReporteGeneracionRER";
        public const string RutaCargaFile = "RutaCargaFile";
        public const string RutaCargaInformeEvento = "Areas/Eventos/Reporte/";
        public const string RutaExportacionInformeEvento = "RutaExportacionInformeEvento";
        public const string RutaCargaEvento = "RutaCargaEvento";
        public const string FileLogoEvento = "Logos/";
        public const string ReporteEnsayo = "ReporteEnsayos";
        public const string ReporteExcel = "ReporteExcel";
        public const string ReporteHidrologia = "ReporteHidrologia";
        public const string ReporteDemandaBarras = "ReporteDemandaBarras";
        public const string ReporteInterconexion = "ReporteInterconexion";
        public const string InitialUrl = "InitialUrl";
        public const string BaseUrlOtherApps = "BaseUrlOtherApps";
        public const string ReporteRechazoCarga = "ReporteRechazoCarga";
        public const string ReporteValorizacionDiaria = "ReporteValorizacionDiaria";
        public const string RutaCargaSolicitudEvento = "Areas/Eventos/Documentos/";
        public const string PathLogo = @"Content\Images\logocoes_black.png";
    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class NombreArchivo 
    {
        public const string FormatoGeneracionRER = "FormatoGeneracionRER.xlsx";
        public const string PlantillaFormatoGeneracionRERSemanal = "PlantillaGeneracionRERSemanal.xlsx";
        public const string PlantillaFormatoGeneracionRERDiario = "PlantillaGeneracionRERDiario.xlsx";
        public const string FormatoInformePDF = "INFORME.pdf";
        public const string FormatoInformeWORD = "INFORME.docx";
        public const string LogoEmpresa = "LOGOEMPRESA_{0}.{1}";
        public const string FormatoCargaInterrupcion = "CargaImportacion.xlsx";
    }

    /// <summary>
    /// Contiene los mensajes que se mostrarán en pantalla
    /// </summary>
    public class MensajesApp
    {
        public const string ValidacionRERDiario = "Fuera de plazo. Plazo permitido: antes de las 09:00 tolerancia 5 minutos.";
        public const string ValidacionRERSemanal = "Fuera de plazo. Plazo permitido: antes de las 14:30 tolerancia 15 minutos del día martes.";
        public const string FueraPlazoRER = "Los datos se cargaron en fuera de plazo.";
        public const string CargaGeneracionRERCorrecto = "Los datos se cargaron correctamente.";
        public const string MensajeFueraPlazo = "Fuera de plazo.";
    }
    
    /// <summary>
    /// Modulos del sistema
    /// </summary>
    public class Modulos
    {
        public const int AppHidrologia = 3;
        public const int AppMedidoresDistribucion = 4;
        public const int AppDemandaCP = 5;
        public const int AppInterconexiones = 6;
        public const int AppDemandaMaxTransferencia = 12;
    }
}