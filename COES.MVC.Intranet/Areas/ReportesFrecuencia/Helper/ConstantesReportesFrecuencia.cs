using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper
{
    public class ConstantesReportesFrecuencia
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoMes = "MM yyyy";
        public const int FormatoPotenciaMax = 93;
        public const int CabeceraPotenciaMax = 41;
        public const string FormatoFechaCorto = "yyyy-MM-dd";
        public const string NombreExcel = "Consumos no autorizados";
        public const string HojaFormatoExcel = "CNA";
        public const int ColExcelData = 2;
        public const string ReporteCna = "ReporteCna";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string FolderReporte = "Areas\\ReportesFrecuencia\\Reporte\\";
        public const int Plantillacorreo = 165;
        public const int AccesoEmpresa = 13;

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual de Usuario - Automatización Reportes de Frecuencia.pdf";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizAutomatizacionModuloManual = "Reportes de Frecuencia\\";

        //constantes para fileServer
        public const string FolderRaizReportesFrecuenciaModuloManual = "Reportes de Frecuencia\\";
    }

    public class ConstantesGestionCodigosVTEAVTP
    {
        public const int IdRolCodigoVTEA = 80;
        public const int IdRolCodigoVTP = 96;

        public const string Activo = "ACT";
        public const string Baja = "BAJ";
        public const string Rechazado = "REC";
        public const string Pendiente = "PAP";
        public const string PendienteAprobacionVTP = "PVT";
        public const string SolicitudBaja = "SBJ";

    }

    public class FormatoReportesFrecuencia
    {
        public const string RepositorioEnsayo = "RepositorioEnsayo";
        public const string ArchFormato01 = "ArchFormato01";
        public const string NombreReporteSegundosFaltantes = "RptSegundosFaltantes.xlsx";
        public const string NombreReporteMilisegundos = "RptMilisegundos.xlsx";
        public const string PlantillaExcelReporteSegundosFaltantes = "PlantillaReporteSegundosFalntantes.xlsx";
        public const int NroFormatos = 10;

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual de Usuario - Automatización Reportes de Frecuencia.pdf";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //constantes para fileServer
        public const string FolderRaizReportesFrecuenciaModuloManual = "Reportes de Frecuencia\\";
    }
}