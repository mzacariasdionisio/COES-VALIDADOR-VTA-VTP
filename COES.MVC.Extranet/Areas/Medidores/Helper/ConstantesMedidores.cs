using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.Medidores.Helper
{
    public class ConstantesMedidores
    {

        /// <summary>
        /// Constantes para manejo del archivo excel
        /// </summary>
        public const string ERR_TOTALREGISTRO = "TOTAL REGISTRO";
        public const string ERR_OUTPACTIVA = "EXCEDE POTENCIA INSTALADA";
        public const string ERR_BLANCOS = "FALTA VALOR";
        public const string ERR_MAXBLANCOS = "LIMITE DE BLANCOS";
        public const string ERR_NEGATIVO = "VALOR NEGATIVO";
        public const string ERR_NONUMERO = "REGISTRO NO IDENTIFICADO";
        public const string ERR_SOLAR = "FUERA DEL RANGO SOLAR";
        public const string ERR_PIN = "PIN ERROR";
        public const string ERR_NOERROR = "0";
        public const string ERR_PTOMEDICION = "PTO MEDICION NO EXISTE";
        public const string ERR_FECHA = "ERROR EN FECHA";
        public const string ERR_SECFECHA = "ERROR EN FECHA2";
        public const string ERR_GENERAL = "ERROR GENERAL";
        public const string ERR_FPLAZO = "FUERA DE PLAZO";
        public const string ERR_CRITICO = "ERROR CRITICO: ";
        public const string ERR_IMPORT_EXPORT = "ERROR IMPORTACION Y EXPORTACION A LA VEZ";
        public const int FILMAX = 96 * 30 + 21;
        public const int INICOL = 1;
        public const int FECHACOL = 1;
        public const int INIFIL = 22;
        public const int LIMPOTACT = 1000;
        public const int MAXBLANCOS = 1000;
        public const int SOLAR_RANGO_MIN = 4 * 60 + 15;
        public const int SOLAR_RANGO_MAX = 19 * 60 + 0;
        public const short COLPIN = 100;
        public const short rowHeightExcel = 15;
        public const short RESULTADOVALIDAOK = 3;
        public const string CargaInterconexionesCorrecto = "Los datos se cargaron correctamente.";
        public const string ValidacionDiario = "Fuera de plazo. Plazo permitido: hasta las 09:00 horas.";
        public const string ValidacionDiaPasado = "Fuera de plazo. La carga esta permitida al día siguiente de la interconexión, hasta de las 09:00 horas.";
        public const string CumplimientoEnPlazo = "En Plazo";
        public const string CumplimientoFueraDePlazo = "Fuera de Plazo";
        public const string HojaFormatoExcel = "FORMATO";
        public const int EnPlazo = 3;
        public const int FueraPlazo = 5;
        /// <summary>
        ///  Constantes de Limites
        /// </summary>

        public const int MW = 0;
        public const int MVAR = 1;
        public const int PTOSOLARGRUPO = 36;
        public const int PTOSOLARCENTRAL = 37;


        public const string ESTRECHAZADO = "R";
        public const string ESTPROCESADO = "P";
        public const decimal PRECISION = 0.01M;

        public const short LECTURA = 1;
        public const string FOLDERPUBLICACION = "Publicacion";
        public const string FOLDERREPOSITORIO = "Repositorio";

        public const short TIPOARCHIVO = 11;
        public const string MESNOMBRE = "ENERO,FEBRERO,MARZO,ABRIL,MAYO,JUNIO,JULIO,AGOSTO,SETIEMBRE,OCTUBRE,NOVIEMBRE,DICIEMBRE";
        public const int DIAPUBLICACIONFIN = 10;
        public const int DIAPUBLICACIONINI = 24;
        public const int DURACIONFORMATO = 22;
        public const int DIAENVIOINI = 2;
        //public const int DIAENVIOFIN = 22;
        public const short HORAENVIOFIN = 12;
        public const short DIAENVIOFIN = 2;
        //public const 
        //// Constante entidad ENVIO
        public const byte ENVIO_ENVIADO = 1;
        public const byte ENVIO_PROCESADO = 2;
        public const byte ENVIO_APROBADO = 3;
        public const byte ENVIO_RECHAZADO = 4;
        public const byte ENVIO_FUERAPLAZO = 5;

        public const string FROM_EMAIL = "webapp@coes.org.pe";
        public const string FROM_NAME = "Declaración Jurada de Medidores de Generaciòn";
        public const int IdFormato = 2;
        public const int IdEmpresa = 12;
        public const string FolderUpload = "Areas\\Medidores\\Uploads\\";
        public const string FolderReporte = "Areas\\Medidores\\Reportes\\";
        public static string ObtenerValorConfig(string llave)
        {
            NameValueCollection appSettings = WebConfigurationManager.AppSettings as NameValueCollection;
            string valor = appSettings[llave];
            return valor;
        }


    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class NombreArchivoInterconexiones
    {
        public const string FormatoInterconexion = "FormatoInterconexion.xlsx";
        public const string PlantillaFormatoInterconexion = "PlantillaInterconexion.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";

        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
        public const string NombreFileUploadInterconexion = "Interconexion-Ecuador";
        public const string ExtensionFileUploadInterconexion = "xlsx";
        public const int IdArchivoInterconexion = 12;


    }


    public class ParametrosEnvioInterconexiones
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
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Declaración Jurada de Medidores de Interconexiòn";
        public const string ToMail = "ToMailInterconexion";
    }

    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesionInterconexiones
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
        public const string SesionMedidores = "SesionMedidores";
        public const string SesionEnPlazo = "SesionEnPlazo";
    }

}