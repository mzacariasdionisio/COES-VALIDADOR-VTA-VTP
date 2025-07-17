using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COES.MVC.Extranet.Areas.RDO.Helper
{
    public class ConstantesExtranetRDO
    {
        public const string LogError = "ERROR.APLICACION";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        public const int VerUltimoEnvio = 1;
        public const int NoVerUltimoEnvio = 0;
        public const string HojaFormatoExcel = "FORMATO";
        public const string CaracterEnter = "\n";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string ExtensionFileUpload = "xlsx";
        public const string RutaCargaFile = "RutaCargaFile";
        public const string MensajeEnvioExito = "El envío se realizó correctamente";
    }

    public class ConstantesDespachoGeneracion
    {
        public const string FolderUpload = "Areas\\RDO\\Uploads\\";
        public const string FolderReporte = "Areas\\RDO\\Reportes\\";
        public const string NombreArchivoLogo = "coes.png";
    }

    public class ConstantesDisponibilidadCombustible
    {
        public const int UnidadDisponibilidad = 13;
        public const int UnidadQuemaGas = 13;
        public const int NroColumnasComnsumo = 9;
        public const int NroFilHeadStock = 2;
        public const int NroFilHeadConsumo = 2;
        public const int FilasHead = 2;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
        public const int NColumnaDisp = 7;
        public const int NColumnaQuema = 7;
        public const int FilaExcelData = 13;
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
        public const string FolderUpload = "Areas\\RDO\\Uploads\\";
        public const string ExtensionFile = "xlsx";
        public const int PlantillaEnvio = 1;
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const int PtoMedUti5 = 42435;
        public const int IdModulo = 9;
        public const int IdFormatoDisponibilidad = 126;
        public const int LectCodiDisponibilidad = 240;
    }

    public class ParamFormatoCombustible
    {
        public const int RowTitulo = 6;
        public const int RowCodigo = 1;
        public const int RowArea = 4;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
    }
}