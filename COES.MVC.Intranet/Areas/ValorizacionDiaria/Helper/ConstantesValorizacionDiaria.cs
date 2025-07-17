using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Helper
{
    public class ConstantesValorizacionDiarias
    {
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Declaración Jurada de Valorización Diaria";
        public const string ToMail = "ToMailValorizacionDiaria";
        public const string FolderUpload = "Areas\\ValorizacionDiaria\\Uploads\\";
        public const string FolderReporte = "Areas\\ValorizacionDiaria\\Reportes\\";
        public const string NombreArchivoLogo = "coes.png";
        public const int IdModulo = 3;
        public const int IdOrigenValorizacionDiaria = 16; // TODO Colocar id de origen de Valorización Diaria
        public const int Caudal = 11;
        public const int EjectutadoTR = 66;
        public const int BandaTR = 3;
    }
    public class MensajesValorizacionDiaria
    {
        public const string CargaValorizacionDiariaCorrecto = "Los datos se cargaron correctamente.";
        public const string MensajeFueraPlazo = "Fuera de plazo.";
        public const string MensajeEnvioExito = "El envío se grabo exitosamente";
    }

    public class ParametrosLecturaValorizacionDiaria
    {
        public const int PeriodoDiario = 1;
        public const int PeriodoSemanal = 2;
        public const int PeriodoMensual = 3;
        public const int PeriodoAnual = 4;
        public const int PeriodoMensualSemana = 5;
        public const int TiempoReal = 0;
        public const int Ejecutado = 1;
        public const int Programado = 2;
    }
    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionDemanda
    {
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
    }

    /// <summary>
    /// Mensajes de pantalla
    /// </summary>
    public class MensajesDemanda
    {
        public const string MensajeEnvioExito = "El envío se realizó correctamente";
    }

    public class NombreArchivoValorizacionDiaria
    {
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaFormatoProgDiario = "PlantillaProgDiario.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
        public const string NombreFileUploadValorizacionDiaria = "ValorizacionDiaria-";
        public const string ExtensionFileUploadValorizacionDiaria = "xlsx";
    }

    /// <summary>
    /// Nombre de archivos
    /// </summary>
    public class NombreArchivoDemanda
    {
        public const string ExtensionFileUploadHidrologia = "xlsx";
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaFormatoProgDiario = "PlantillaProgDiario.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
    }


    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesionValorizacionDiaria
    {
        public const string SesionUsuario = "SesionUsuario";
        public const string SesionIdOpcion = "SesionIdOpcion";
        public const string SesionOpciones = "SesionOpciones";
        public const string SesionIndPermiso = "SesionIndPermiso";
        public const string SesionNodo = "SesionNodo";
        public const string SesionPermiso = "SesionPermiso";
        public const string SesionEmpresa = "SesionEmpresa";
        public const string SesionFormato = "SesionFormato";
        public const string UltimoPerfil = "UltimoPerfil";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionListaProcesar = "SesionListaProcesar";
        public const string SesionListaErrores = "SesionListaErrores";
        public const string SesionFileName = "FileName";
        public const string SesionValidacionDatos = "SesionValidacionDatos";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFechaProceso = "SesionFechaProceso";
        public const string SesionFechaProcesoFin = "SesionFechaProcesoFin";
        public const string SesionTipoInformacion = "SesionTipoInformacion";
        public const int BandaTR = 3;
    }
}