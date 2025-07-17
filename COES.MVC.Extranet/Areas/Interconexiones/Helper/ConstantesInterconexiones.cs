using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Interconexiones.Helper
{
    public class ConstantesInterconexiones
    {
        public const int FilaExcelData = 16;
        public const int ResolucionCuartoHora = 15;
        public const int ptoInterconexion = 5020;
        public const int IdEmpresaInterconexion = 12;
        public const int IdFormato = 50;
        public const int IdLectura = 1;
        public const string FolderUpload = "Areas\\Interconexiones\\Uploads\\";
        public const string FolderReporte = "Areas\\Interconexiones\\Reportes\\";
        public const int EnPlazo = 3;
        public const int FueraPlazo = 5;
        public const string CumplimientoEnPlazo = "En Plazo";
        public const string CumplimientoFueraDePlazo = "Fuera de Plazo";
        public const int IdHoja = 34;
        public const int IdHojaMedidorPrincipal = 21;
        public const int IdHojaMedidorSecundario = 22;
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionInterconexiones
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
    public class NombreArchivoInterconexiones
    {
        public const string ExtensionFileUploadHidrologia = "xlsx";
        public const string FormatoProgDiario = "FormatoProgDiario.xlsx";
        public const string PlantillaFormatoInter = "PlantillaInterconexiones.xlsx";
        public const string PlantillaReporteHistorico = "PlantillaReporteHistorico.xlsx";
        public const string PlantillaReporteEnvio = "PlantillaReporteEnvio.xlsx";
        public const string ReporteEnvio = "ReporteEnvio.xlsx";
        public const string ReporteHistorico = "ReporteHistorico.xlsx";
        public const string NombreReporteHistorico = "Historico.xlsx";
        public const string NombreReporteEnvio = "Envio.xlsx";
    }
}