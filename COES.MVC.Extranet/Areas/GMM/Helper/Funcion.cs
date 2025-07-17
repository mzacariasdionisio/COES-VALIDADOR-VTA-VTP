using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.GMM.Helper
{
    public class Funcion
    {
        //CONSTANTES
        public const int iGrabar = 1;
        public const int iEditar = 2;
        public const int iNuevo = 3;
        public const int iEliminar = 5;
        public const decimal dValorMax = 0.3135M;
        public const double dLimiteMaxEnergia = 350;
        public const int CODIEMPR_SINCONTRATO = -1001;
        public const string NOMBEMPR_SINCONTRATO = "RETIRO SIN CONTRATO";
        public const string MensajeSoles = "Importes Expresados en Nuevos Soles (S/.)";
        public const string ReporteDirectorio = "ReporteTransferencia";
        public const string RutaReporte = "Areas/Transferencias/Reporte/";
        public const string HojaReporteExcel = "REPORTE";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppTxt = "application/txt";
        public const decimal dPorcentajePotenciaMaxima = 0.1M;
        public const int CodigoOrigenLecturaSICLI = 19;
        public const int CodigoOrigenLecturaML = 32;
        
        //GMME
        public const string NombreRptInsumo = "Insumos.xlsx";
        public const string NombreRpt1 = "Resumen.xlsx";
        public const string NombreRpt2 = "Cuadro1.xlsx";
        public const string NombreRpt3 = "Cuadro2.xlsx";
        public const string NombreRpt4 = "Cuadro3.xlsx";
        public const string NombreRpt5 = "Cuadro4.xlsx";
        public const string NombreRpt6 = "Cuadro5.xlsx";
        public const int formatoEnergiaTrimestral = 102;

    }
}

