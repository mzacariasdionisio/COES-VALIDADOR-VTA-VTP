using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Campanias.Helper
{
    public class ConstantesCampanias
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoMes = "MM yyyy";
        public const int FormatoPotenciaMax = 93;
        public const int CabeceraPotenciaMax = 41;
        public const string FormatoFechaCorto = "yyyy-MM-dd";
        public const string NombreExcel = "Consumos no autorizados";
        public const string HojaFormatoExcel = "CNA";
        public const int ColExcelData = 2;
        public const string AppExcel = "application/vnd.ms-excel";
        public const string FolderFichas = "Areas\\Campanias\\Fichas\\";
        public const string FolderReporte = "Reporte\\";
        public const string FolderTemp = "Temp\\";
        public const string FolderZip = "Temp.zip";
        public const string FolderFichasGeneracionSubtipo = "Generacion\\Subtipo\\";
        public const string FolderFichasGeneracion = "Generacion\\";
        public const string FolderFichasEmpresa = "Empresa\\";
        public const string FolderFichasGeneracionCHidro = "Hidroelectrica\\";
        public const string FolderFichasGeneracionCTermo = "Termoelectrica\\";
        public const string FolderFichasGeneracionCEolica = "Eolica\\";
        public const string FolderFichasGeneracionCSolar = "Solar\\";
        public const string FolderFichasGeneracionCBiom = "Biomasa\\";
        public const string FolderFichasGeneracionSubestaciones = "Subestaciones\\";
        public const string FolderFichasGeneracionLineas = "Lineas\\";
        public const string FolderFichasGeneracionDistribuida = "GeneracionDistribuida\\";
        public const string FolderFichasHidrogenoVerde = "HidrogenoVerde\\";
        public const string FolderFichasITC = "ITC\\";
        public const string FolderFichasTransmision = "Transmision\\";
        public const string FolderFichasDemanda = "Demanda\\";
        public const string FolderArchivos = "Archivos\\";
        public const string FolderReporteItc = "itc\\";
        public const string FolderEmpresa = "empresa\\";
        public const string FolderReportePronosticos = "pronosticos\\";
        public const string FolderReporteProyectos = "proyectos\\";
        public const string FolderTempReporte = "TempReporte\\";
        public const string FolderZipReporte = "TempReporte.zip";

        //Manual de usuario
        public const string ArchivoManualUsuarioIntranet = "Manual_Usuario_Intranet_v1.0.pdf";
        public const string ModuloManualUsuario = "Manuales de Usuario\\";

        //Constantes para fileServer
        public const string FolderRaizCampaniasModuloManual = "Plataforma de Campañas\\";
    }


    public class FormatoArchivosExcelCampanias
    {
        public const string NombreFichaExcelGeneracionCentralHidro = "G1-FichasProyecto_CentralHidro.xlsx";
        public const string NombreFichaExcelGeneracionCentralTermo = "G2-FichasProyecto_CentralTermo.xlsx";
        public const string NombreFichaExcelGeneracionCentralEolica = "G3-FichasProyecto_CentralEolica.xlsx";
        public const string NombreFichaExcelGeneracionCentralSolar = "G4-FichasProyecto_CentralSolar.xlsx";
        public const string NombreFichaExcelGeneracionCentralBiom = "G5-FichasProyecto_CentralBiomasa.xlsx";
        public const string NombreFichaExcelGeneracionSubestaciones = "G6-FichasProyecto_Subestaciones.xlsx";
        public const string NombreFichaExcelGeneracionCentralLineas = "G7-FichasProyecto_Lineas.xlsx";
        public const string NombreFichaExcelTransmisionLinea = "T1-FichasProyecto_Lineas.xlsx";
        public const string NombreFichaExcelTransmisionSubestacion = "T2-FichasProyecto_Subestaciones.xlsx";
        public const string NombreFichaExcelTransmisionCronograma = "T3-FichasProyecto_Cronograma.xlsx";
        public const string NombreFichaExcelDemanda = "D1-Demanda_Grandes_Proyectos.xlsx";
        public const string NombreFichaExcelITCDemanda = "FichasITC-DemandaEDE.xlsx";
        public const string NombreFichaExcelITCSistemaElectricoParametros = "FichaITC - Sistema Electrico y Parametros.xlsx";
        public const string NombreFichaExcelGeneracionDistribuida = "GD-Ficha de Proyecto Generacion Distribuida.xlsx";
        public const string NombreFichaExcelHidrogenoVerde = "H2V-Ficha de Proyecto Hidrogeno Verde.xlsx";
        public const string NombrePlanTransmision = "PlanTransmision.xlsx";
        public const string NombrePlanTransmisionNew = "ReportePlanTransmision.xlsx";
        public const string NombreReporteItcDemanda = "ReporteITC-DemandaEDE.xlsx";
        public const string NombreReporteItcSistema = "ReporteITC-SistemaElectricoyParametros.xlsx";
        public const string NombreReporteDemanda = "ReporteDemanda.xlsx";
        public const string NombreReporteProyCronograma = "Formato_CronogramaDeEjecucion_generacion.xlsx";
        public const string NombreReporteProyBiomasa = "Reporte_Biomasa.xlsx";
        public const string NombreReporteProyLinea = "Reporte_LineaTransmision.xlsx";
        public const string NombreReporteProyTransform = "Reporte_Transformador_y_EquiposDeCompensacion.xlsx";
        public const string NombreReporteProyEolica = "Reportes_Eolica.xlsx";
        public const string NombreReporteProyGenDistrib = "Reportes_GenDistribuida.xlsx";
        public const string NombreReporteProyHidrogeno = "Reportes_Hidrogeno.xlsx";
        public const string NombreReporteProySolar = "Reportes_Solar.xlsx";
        public const string NombreReporteProyTermica = "Reportes_Termica.xlsx";
        public const string NombreReporteProyHidroel = "ReportesHidroelectrica.xlsx";
        public const string NombreReporteEmpresa = "Empresa.xlsx";
    }

}