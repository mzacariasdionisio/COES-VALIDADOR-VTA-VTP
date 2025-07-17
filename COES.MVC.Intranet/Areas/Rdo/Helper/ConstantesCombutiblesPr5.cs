using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Rdo.Helper
{
    public class ConstantesIntranet
    {
        public const int TipoReporteStock = 1; //Tipo de reporte stock de combustible
        public const int TipoReporteConsumo = 2; //Tipo de reporte consumo de combustible
        public const string StrCtralIntCoes = "-1,0,1,2,3";
        public const string StrCtralIntNoCoes = "10";
        public const int NroColumnasComnsumo = 8;
        public const int NroFilHeadStock = 2;
        public const int NroFilHeadConsumo = 2;
        public const int FilasHead = 2;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
        public const int FilaExcelData = 13;
        public const int RowTitulo = 6;
        public const int RowCodigo = 1;
        public const int RowArea = 4;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
        public const int UnidadDisponibilidad = 13;
        public const int UnidadQuemaGas = 13;
        public const int NColumnaDisp = 5;
        public const int NColumnaQuema = 5;
        public const int NroPageShow = 10;
        public const string FolderReporte = "Areas\\StockCombustibles\\Reporte\\";
        public const string NombreReporteArchivosEnviados = "RptArchivosEnviados.xlsx";
        public const string NombreArchivoEnvio = "ArchivoEnvio.xlsx";
        public const string NombreArchivoCumplimiento = "RptArchivoCumplimiento.xlsx";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string NroPaginasReporte = "NroPaginasReporte";
        public const string ListaFechas = "ListaFechas";
        public const string ListaMedicionxIntervalo = "ListaMedicionxIntervalo";
        public const string ListaMedicion24 = "ListaMedicion24";
        public const int YacimientoGasCodi = 1;
        public const int LectCodiDisponibilidad = 240;

        public const string FolderReporteRDO = "Areas\\RDO\\Reportes\\";
    }

    /// <summary>
    /// Contiene los nombres de los archivos
    /// </summary>
    public class StockConsumoArchivo
    {
        public const string RptExcelStock = "RptExcelStock.xlsx";
        public const string RptExcelAcumulado = "RptExcelAcumulado.xlsx";
        public const string RptExcelAcumuladoDet = "RptExcelAcumuladoDet.xlsx";
        public const string RptExcelConsumo = "RptExcelConsumo.xlsx";
        public const string RptExcelHistorico = "RptExcelHistorico.xlsx";
        public const string RptExcelGraficoStock = "RptExcelGraficoStock.xlsx";
        public const string RptExcelGraficoConsumo = "RptExcelGraficoConsumo.xlsx";
        public const string RptExcelGraficoPresTemp = "RptExcelGraficoPresTemp.xlsx";
        public const string RptExcelDisponibilidad = "RptExcelDisponibilidad.xlsx";
        public const string RptExcelQuema = "RptExcelQuema.xlsx";
        public const string ArchivoEnvio = "ArchivoEnvio.xlsx";
    }
}