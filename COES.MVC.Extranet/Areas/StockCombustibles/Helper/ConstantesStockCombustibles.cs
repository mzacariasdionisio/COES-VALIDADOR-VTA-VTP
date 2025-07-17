using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.StockCombustibles.Helper
{
    public class ConstantesCombustible
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
        public const string FolderUpload = "Areas\\StockCombustibles\\Uploads\\";
        public const string ExtensionFile = "xlsx";
        public const int PlantillaEnvio = 1;
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const int PtoMedUti5 = 42435;
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