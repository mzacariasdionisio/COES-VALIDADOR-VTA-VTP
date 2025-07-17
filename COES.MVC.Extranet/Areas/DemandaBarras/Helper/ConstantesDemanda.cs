using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.DemandaBarras.Helper
{
    public class ConstantesDemanda
    {
        public const int FilaExcelData = 16;
        public const int ResolucionCuartoHora = 15;        
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
}