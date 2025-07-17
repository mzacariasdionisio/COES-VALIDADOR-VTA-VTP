using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.Hidrologia.Helper
{
    public class ConstantesHidrologia
    {
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Declaración Jurada de Hidrología";
        public const string ToMail = "ToMailHidrologia";
        public const int IdModulo = 3;
        public const int IdOrigenHidro = 16;
        public const int Caudal = 11;
        public const int EjectutadoTR = 66;
        public const int BandaTR = 3;
    }

    public class NombreArchivoHidro
    {
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaFormatoProgDiario = "PlantillaProgDiario.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
        public const string NombreFileUploadHidrologia = "Hidrologia-";
        public const string ExtensionFileUploadHidrologia = "xlsx";
    }

    public class MensajesHidro
    {
        public const string CargaHidrologiaCorrecto = "Los datos se cargaron correctamente.";
        public const string MensajeFueraPlazo = "Fuera de plazo.";
        public const string MensajeEnvioExito = "El envío se grabo exitosamente";
    }

    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesionHidro
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
    }

    /// <summary>
    /// Contiene los nombres de propiedades de clases
    /// </summary>
    public class EntidadPropiedadHidro
    {
        public const string PtoMediCodi = "PtoMediCodi";
        public const string PtoNomb = "PtoDescripcion";
        public const string Areacodi = "Areacodi";
        public const string AreaNomb = "AreaNomb";
        public const string Formatcodi = "Formatcodi";
        public const string Formatnombre = "Formatnombre";
        public const string EquiCodi = "EquiCodi";
        public const string EquiNomb = "EquiNomb";
        public const string Emprcodi = "Emprcodi";
        public const string Emprnomb = "Emprnomb";
        public const string Tipoptomedicodi = "Tipoptomedicodi";
        public const string Tipoptomedinomb = "Tipoptomedinomb";
        public const string Lectcodi = "Lectcodi";
        public const string Lectnomb = "Lectnomb";

    }

    public class ParametrosLecturaHidro
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
}