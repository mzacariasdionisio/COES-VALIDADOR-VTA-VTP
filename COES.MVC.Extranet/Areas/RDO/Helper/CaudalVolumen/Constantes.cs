using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.RDO.Helper
{
    public class ConstantesCaudalVolumen
    {
        public const string FolderReporte = "Areas\\RDO\\Reportes\\";
        
        public const string FromEmail = "webapp@coes.org.pe";
        public const string FromName = "Caudales y Volúmenes";
        public const string ToMail = "ToMailHidrologia";
        public const string FolderUpload = "Areas\\RDO\\Uploads\\";
        
        public const string NombreArchivoLogo = "coes.png";
        public const int IdModulo = 3;
        public const int IdOrigen = 16;
        public const int Caudal = 11;
        public const int EjectutadoTR = 66;
        public const int BandaTR = 3;
        public const int IdLectura = 239;
        
    }
    public class NombreArchivoCaudalVolumen
    {
        public const string ExtensionFileUpload = "xlsx";
    }
    public class MensajesCaudalVolumen
    {
        public const string MensajeFueraPlazo = "Fuera de plazo.";
        public const string MensajeEnvioExito = "El envío se grabo exitosamente";
    }
    /// <summary>
    /// Constantes para los datos de sesion
    /// </summary>
    public class DatosSesionCaudalVolumen
    {
        public const string SesionFormato = "SesionFormato";
    }
    /// <summary>
    /// Contiene los nombres de propiedades de clases
    /// </summary>
    public class EntidadPropiedadCaudalVolumen
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
    public class ParametrosLectura
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