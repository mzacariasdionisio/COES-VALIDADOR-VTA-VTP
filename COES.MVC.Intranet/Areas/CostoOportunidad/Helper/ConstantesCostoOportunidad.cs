using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Helper
{
    public class ConstantesCOportunidad
    {
        public static int origlectcodi = 2;
        public int IdLectProgramado = 4;
        public int IdLectReporgramado = 5;
        public const int NrofilasCabecera = 1;
        public const int NroColCabecera = 1;
        public const int NroColumasMatriz = 6;
        public const int BandaMaxima = 1;
        public const int BandaMinima = 2;
        public const int NBloquesMattrizReservas = 48;
        public const string ListaReservaProg = "ListaReservaProg";        
        public const string ListaReservaEjec = "ListaReservaEjec";
        public const string ListaCruceReserva = "ListaCruceReserva";
        public const string ListaDespacho = "ListaDespacho";
        public const string ListaDespachoSin = "ListaDespachoSin";
        public const string Directorio = "Areas\\CostoOportunidad\\Reporte\\";
        public const string RptExcel = "RptCostoOportunidad.xls";        
        public const string PathArchivoDat = "Areas/CostoOportunidad/Temporal/";
        public const string PathArchivoCsv = "Areas/CostoOportunidad/Temporal/";
        public const string NombreArchivosDAT = "maxrsh.dat,maxrst.dat,minrsh.dat,minrst.dat";
        public const string NombreArchDat = "ARCHDAT";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionContadorArchivos = "SesionContadorArchivos";
        public const string SesionMatrizExcel = "MatrizExcel";       

        #region Mejoras Costos de Oportunidad

        public const string RutaExportacion = "Areas/CostoOportunidad/Reporte/";
        public const string ArchivoReporte = "CostoOportunidad.xlsx";
        public const string ArchivoReprogramas = "Reprogramas.xlsx";
        public const string ArchivoReporteInsumo = "InsumosCostoOportunidad.xlsx";
        public const string ArchivoReporteResultado = "ResultadoCostosOportunidad.xlsx";
        public const string ArchivoReporteCostoOportunidad = "InformacionCostoDeOportunidad.xlsx";

        #endregion

        public const string NombreArchivoSenial = "DATOS_SENIALES_MANUAL.xlsx";
    }

    public class ConstantesDespachoDiario
    {
        public const int IdFormatoDespachoDiario = 62;
        public const int LectCodiDespachoDiario = 85;
        public const int FilaExcelData = 13;
        public const int NroFilasDataCSV = 48;
        public const int NrofilasCabeceraCSV = 1;
        public const string NomCarpetaProgramados = "PDO";
        public const string NomCarpetaReProgramados = "RDO";
        public const string TipoArchivo = ".csv";
        public const string CsvGerHidrocp = "gerhidcp.csv";
        public const string CsvGerTermocp = "gertercp.csv";
        public const string CsvResagHidrocp = "resaghcp.csv";
        public const string CsvResagTermocp = "resagtcp.csv";
        public const string DatMaxRsHidro = "maxrsh.dat";
        public const string DatMaxRsTermo = "maxrst.dat";
        public const string DatMinRsHidro = "minrsh.dat";
        public const string DatMinRsTermo = "minrst.dat";
        public const string SR = "_SR/";
        public const string RSF = "RSF/";
    }

    /// <summary>
    /// Datos de sesion
    /// </summary>
    public class DatosSesionDespachoDiario
    {
        public const string SesionFormato = "SesionFormato";
        public const string SesionIdEnvio = "SesionIdEnvio";
        public const string SesionFileName = "SesionFileName";
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string SesionMatrizExcelColores = "MatrizExcelColores";
        public const string SesionListaPtos = "ListaPtos";
    }
}