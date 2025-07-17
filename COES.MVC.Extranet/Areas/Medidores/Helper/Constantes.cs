using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Helper
{
    public class Constantes
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const int PageSize = 20;
        public const int NroPageShow = 10;
               
        public const string HojaReporteExcel = "REPORTE";
        public const string HojaFormatoExcel = "FORMATO";
        public const string NombreLogoCoes = "coes.png";

        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";

        public const string PaginaLogin = "/WebForm/Account/Login.aspx";
        public const string PaginaAccesoDenegado = "home/autorizacion";
        public const string DefaultControler = "Home";
        public const string LoginAction = "Login";
        public const string DefaultAction = "default";
        public const int IdAplicacion = 10;
        public const string SI = "S";
        public const string NO = "N";

        public const string FormatoWord = "WORD";
        public const string FormatoPDF = "PDF";

        public const char CaracterComa = ',';
        public const string AperturaSerie = "[";
        public const string CierreSerie = "]";
        public const char CaracterCero = '0';

        public const string TextoCentral = "CENTRAL";
        public const string TextoMW = "MW";
        public const string CaracterH = "H";

        public const int EnPlazo = 3;
        public const int FueraPlazo = 5;
        public const int FormatoCodigo = 2;
        public const string CumplimientoEnPlazo = "En Plazo";
        public const string CumplimientoFueraDePlazo = "Fuera de Plazo";
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
        public const string SesionUsuario = "SesionUsuario";
        public const string SesionIdOpcion = "SesionIdOpcion";
        public const string SesionOpciones = "SesionOpciones";
        public const string SesionIndPermiso = "SesionIndPermiso";
        public const string SesionNodo = "SesionNodo";
        public const string SesionPermiso = "SesionPermiso";
        public const string SesionEmpresa = "SesionEmpresa";
        public const string UltimoPerfil = "UltimoPerfil";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionListaProcesar = "SesionListaProcesar";
        public const string SesionListaErrores = "SesionListaErrores";
        public const string SesionListaPeriodo = "SesionListaPeriodo";
        public const string SesionFileName = "FileName";
        public const string SesionValidacionDatos = "SesionValidacionDatos";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFechaProceso = "SesionFechaProceso";
        public const string SesionMedidores= "SesionMedidores";
    }

    /// <summary>
    /// Contiene las rutas de los directorios utilizados
    /// </summary>
    public class RutaDirectorio
    {
        public const string RutaCargaFileInterconexion = "RutaCargaFileInterconexion";
        public const string ReporteInterconexion = "ReporteInterconexion";
    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class NombreArchivo 
    {
        public const string FormatoInterconexion = "FormatoInterconexion.xlsx";
        public const string PlantillaFormatoInterconexion = "PlantillaInterconexion.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";

        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
        public const string NombreFileUploadInterconexion = "Interconexion-";
        public const string ExtensionFileUploadInterconexion = "xlsx";
        public const int IdArchivoInterconexion = 12;
    }

    /// <summary>
    /// Contiene los mensajes que se mostrarán en pantalla
    /// </summary>
    public class MensajesApp
    {
        public const string ValidacionDiario = "Fuera de plazo. Plazo permitido: hasta las 09:00 horas.";
        public const string ValidacionDiaPasado = "Fuera de plazo. La carga esta permitida al día siguiente de la interconexión, hasta de las 09:00 horas.";
        public const string FueraPlazoRER = "Los datos se cargaron en fuera de plazo.";
        public const string CargaGeneracionRERCorrecto = "Los datos se cargaron correctamente.";
        public const string MensajeFueraPlazo = "Fuera de plazo.";
    }

    public class ParametrosEnvio
    {
        public const byte EnvioEnviado = 1;
        public const byte EnvioProcesado = 2;
        public const byte EnvioRechazado = 4;
        public const byte EnvioAprobado = 3;
        public const byte EnvioFueraPlazo = 5;
        public const string MailBody = "Estimados Ingenieros  RED DE ENRGÍA DEL PERÚ\n\n Por medio del presente, se le comunica que se registró {0} en el portal" +
            " extranet la información de Intercambios Internacionales de Electricidad de su representada en atención a lo " + 
            " dispuesto en el Procedimiento Técnico N°43 - /“Intercambios Internacionales de Electricidad en Marco de la Decisión" +
            " 757 de la CAN ” el que se detalla a seguir:";
        public const string From_Email = "webapp@coes.org.pe";
        public const string From_Name = "Declaración Jurada de Medidores de Interconexiòn";
        public const string To_Mail = "ToMailInterconexion";
    }

}