using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Compensacion.Helper
{
    public class ConstantesCompensacion
    {

        public const string FormatoFecha = "dd/MM/yyyy";
        public readonly static string EnlaceLogoCoes = ConfigurationManager.AppSettings["LogoCoes"].ToString();
        public const string NombreLogo = "LogoCOES";
        public const int FilaExcelData = 14;
        public const int ResolucionCuartoHora = 15;
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string MensajeEnvioExito = "El envío se realizó correctamente";
        public const string ExtensionFileUploadHidrologia = "xlsx";
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaFormatoProgDiario = "PlantillaProgDiario.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
        public const string RutaCarga = "Areas/Compensacion/Reportes/";
        public const string PathLogo = @"Content\Images\logocoes.png";
    }
}
