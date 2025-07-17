using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Configuration;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Helper
{
    public class ExcelHelper
    {
        public static string GenerarReporteTotalDatos(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteTotalDatos" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";
                string colorborder = "#000000";

                var contFila = 6;
                //var filainicio = 6;

                ws.Cells[2, 3].Value = "INTERRUPCION POR RECHAZO CARGA - TOTAL DE DATOS";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;

                ws.Cells[4, 4].Value = eventoCTAF;
                ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "SUBESTACIÓN";
                ws.Cells[6, 6].Value = "POTENCIA (MW)";                       
                ws.Cells[6, 7].Value = "INICIO (HH:MM:SS)";
                ws.Cells[6, 8].Value = "FINAL (HH:MM:SS)";
                ws.Cells[6, 9].Value = "DURACIÓN (MIN)";
             
                ExcelRange rg1 = ws.Cells[6, 3, 6, 9];
                ObtenerEstiloCelda(rg1, 0);

                
                var total = decimal.Zero;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 5].Value = item.Subestacion;
                    ws.Cells[contFila, 6].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 6].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 7].Value = item.Rccuadhoriniejec;

                    ws.Cells[contFila, 8].Value = item.Rccuadhorfinejec;
                    ws.Cells[contFila, 9].Value = item.Duracion;
                    ws.Cells[contFila, 9].Style.Numberformat.Format = @"0.00";

                    total += item.Rcejeucargarechazada;
                    
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(3.11M); //16.43p;
                ws.Column(4).Width = CallcularAnchoCentimetros(3.11M); //16.43p;
                ws.Column(5).Width = CallcularAnchoCentimetros(2.93M); //9.7;
                ws.Column(6).Width = CallcularAnchoCentimetros(1.57M); //7.5;
                ws.Column(7).Width = CallcularAnchoCentimetros(1.64M); //8.8;
                ws.Column(8).Width = CallcularAnchoCentimetros(1.64M); //8.8;
                ws.Column(9).Width = CallcularAnchoCentimetros(1.76M); //9;

                if(listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[7, 3, contFila, 9];
                    ObtenerEstiloCelda(rg1, 1);

                    ws.Cells[contFila + 1, 3].Value = "TOTAL";
                    ws.Cells[contFila + 1, 3, contFila + 1, 5].Merge = true;
                    ws.Cells[contFila + 1, 3, contFila + 1, 5].Style.Font.Bold = true;

                    //ws.Cells[contFila, 1, contFila, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[contFila + 1, 6].Value = decimal.Round(total, 2);
                    ws.Cells[contFila + 1, 6].Style.Font.Bold = true;
                    ws.Cells[contFila + 1, 6].Style.Numberformat.Format = @"0.00";
                    //ws.Cells[contFila + 1, 3, contFila + 1, 6].Style.Font.Size = 8;

                    rg1 = ws.Cells[contFila + 1, 3, contFila + 1, 6];

                    rg1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rg1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg1.Style.Font.Size = 8;
                }
                

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteCumplimientoRMC(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteCumplimientoRMC" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 10;
                //var contItem = 1;


                ws.Cells[2, 3].Value = "EVALUACIÓN DE CUMPLIMIENTO DE LOS RMC";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 10].Merge = true;
                ws.Cells[2, 3, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;
                ws.Cells[4, 3, 4, 4].Merge = true;

                ws.Cells[4, 5].Value = eventoCTAF;
                //ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "COORDINADO";
                ws.Cells[6, 8].Value = "POTENCIA PROMEDIO                (PREVIO AL RMC)";
                ws.Cells[6, 11].Value = "EVALUACIÓN PROMEDIO     POTENCIA EJECUTADO";
                ws.Cells[6, 14].Value = "EVALUACIÓN DE RMC EN ENERGÍA";

                ws.Cells[7, 5].Value = "RECHAZO DE CARGA (MW)";
                ws.Cells[7, 6].Value = "HORA";
                ws.Cells[7, 8].Value = "POTENCIA (MW)";
                ws.Cells[7, 9].Value = "INTERVALO DE MEDICIÓN";
                ws.Cells[7, 11].Value = "POTENCIA (MW)";
                ws.Cells[7, 12].Value = "INTERVALO DE MEDICIÓN";

                ws.Cells[8, 6].Value = "INICIO HH:MM:SS DD.MM.YY";
                ws.Cells[8, 7].Value = "FINAL HH:MM:SS DD.MM.YY";
                ws.Cells[8, 9].Value = "HORA";
                ws.Cells[8, 12].Value = "HORA";

                ws.Cells[9, 9].Value = "INICIO HH:MM:SS DD.MM.YY";
                ws.Cells[9, 10].Value = "FINAL HH:MM:SS DD.MM.YY";
                ws.Cells[9, 12].Value = "INICIO HH:MM:SS DD.MM.YY";
                ws.Cells[9, 13].Value = "FINAL HH:MM:SS DD.MM.YY";



                ExcelRange rg1 = ws.Cells[6, 3, 9, 14];
                ObtenerEstiloCelda(rg1, 2);

                ws.Cells[6, 3, 9, 3].Merge = true;
                ws.Cells[6, 4, 9, 4].Merge = true;
                ws.Cells[6, 5, 6, 7].Merge = true;
                ws.Cells[6, 8, 6, 10].Merge = true;
                ws.Cells[6, 11, 6, 13].Merge = true;
                ws.Cells[6, 14, 9, 14].Merge = true;

                ws.Cells[7, 6, 7, 7].Merge = true;
                ws.Cells[7, 9, 7, 10].Merge = true;
                ws.Cells[7, 12, 7, 13].Merge = true;
                ws.Cells[7, 5, 9, 5].Merge = true;
                ws.Cells[7, 8, 9, 8].Merge = true;
                ws.Cells[7, 11, 9, 11].Merge = true;
                //ws.Cells[2, 6, 4, 6].Merge = true;
                //ws.Cells[2, 7, 2, 8].Merge = true;
                ws.Cells[8, 9, 8, 10].Merge = true;
                ws.Cells[8, 12, 8, 13].Merge = true;
                //ws.Cells[2, 10, 2, 11].Merge = true;

                ws.Cells[8, 6, 9, 6].Merge = true;
                ws.Cells[8, 7, 9, 7].Merge = true;
                //ws.Cells[3, 5, 4, 5].Merge = true;
                //ws.Cells[3, 7, 3, 8].Merge = true;
                //ws.Cells[3, 10, 3, 11].Merge = true;

                //ws.Cells[1, 6, 1, 8].Style.WrapText = true;
                //ws.Cells[1, 9, 1, 11].Style.WrapText = true;
                ws.Row(6).Height = 21;
                ws.Row(7).Height = 21;
                //var total = decimal.Zero;

                foreach (var item in listaCuadroProgUsuario)
                {
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 4].Style.WrapText = true;
                    ws.Cells[contFila, 5].Value = item.Rcproucargarechazarcoord;
                    ws.Cells[contFila, 5].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 6].Value = item.RccuadfechoriniRep;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = item.RccuadfechorfinRep;
                    ws.Cells[contFila, 7].Style.WrapText = true;
                    ws.Cells[contFila, 8].Value = item.RcreevpotenciaprompreviaRep;
                    ws.Cells[contFila, 8].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 9].Value = item.RccuadfechoriniPrevioRep;
                    ws.Cells[contFila, 9].Style.WrapText = true;
                    ws.Cells[contFila, 10].Value = item.RccuadfechorfinPrevioRep;
                    ws.Cells[contFila, 10].Style.WrapText = true;
                    ws.Cells[contFila, 11].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 11].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 12].Value = item.RcejeufechorinicioRep;
                    ws.Cells[contFila, 12].Style.WrapText = true;
                    ws.Cells[contFila, 13].Value = item.RcejeufechorfinRep;
                    ws.Cells[contFila, 13].Style.WrapText = true;
                    ws.Cells[contFila, 14].Value = item.Evaluacion.Replace("CUMPLIO","CUMPLIÓ");
                    ws.Cells[contFila, 14].Style.WrapText = true;
                    ws.Cells[contFila, 14].Style.Font.Bold = true;
                    contFila++;
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(2.11M);
                ws.Column(4).Width = CallcularAnchoCentimetros(2.26M);
                ws.Column(5).Width = CallcularAnchoCentimetros(1.3M); 
                ws.Column(6).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(7).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(8).Width = CallcularAnchoCentimetros(1.37M);
                ws.Column(9).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(10).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(11).Width = CallcularAnchoCentimetros(1.37M); 
                ws.Column(12).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(13).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(14).Width = CallcularAnchoCentimetros(2.06M);

                if(listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[10, 3, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 1);
                }
                         

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteInterrupcionesSuministroInformeTecnico(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteInterrupcionesSuministroInfTec" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 10;
                //var contItem = 1;

                ws.Cells[2, 3].Value = "INTERRUPCIONES DE SUMINISTRO POR RMC PARA EL INFORME TÉCNICO";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 10].Merge = true;
                ws.Cells[2, 3, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;
                ws.Cells[4, 3, 4, 4].Merge = true;

                ws.Cells[4, 5].Value = eventoCTAF;
                //ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "SUBESTACIÓN";
                ws.Cells[6, 6].Value = "COORDINADO";
                ws.Cells[6, 9].Value = "EJECUTADO";               

                ws.Cells[7, 6].Value = "RECHAZO DE CARGA (MW)";
                ws.Cells[7, 7].Value = "HORA";
                //ws.Cells[2, 6].Value = "POTENCIA (MW)";
                ws.Cells[7, 9].Value = "RECHAZO DE CARGA (MW)";
                ws.Cells[7, 10].Value = "HORA";
                ws.Cells[7, 12].Value = "DURACIÓN (MIN)";

                ws.Cells[8, 7].Value = "INICIO";
                ws.Cells[8, 8].Value = "FINAL";
                //ws.Cells[3, 7].Value = "HORA";
                ws.Cells[8, 10].Value = "INICIO";
                ws.Cells[8, 11].Value = "FINAL";

                ws.Cells[9, 7].Value = "(HH:MM:SS) DD.MM.YYYY";
                ws.Cells[9, 8].Value = "(HH:MM:SS) DD.MM.YYYY";
                ws.Cells[9, 10].Value = "(HH:MM:SS) DD.MM.YYYY";
                ws.Cells[9, 11].Value = "(HH:MM:SS) DD.MM.YYYY";



                ExcelRange rg1 = ws.Cells[6, 3, 9, 12];
                ObtenerEstiloCelda(rg1, 0);

                ws.Cells[6, 3, 9, 3].Merge = true;
                ws.Cells[6, 4, 9, 4].Merge = true;
                ws.Cells[6, 5, 9, 5].Merge = true;
                ws.Cells[6, 6, 6, 8].Merge = true;
                ws.Cells[6, 9, 6, 12].Merge = true;
                //ws.Cells[1, 10, 1, 12].Merge = true;
                //ws.Cells[1, 13, 4, 13].Merge = true;

                ws.Cells[7, 6, 9, 6].Merge = true;
                //ws.Cells[7, 6, 7, 8].Merge = true;
                ws.Cells[7, 9, 9, 9].Merge = true;
                ws.Cells[7, 10, 7, 11].Merge = true;
                ws.Cells[7, 12, 9, 12].Merge = true;
                //ws.Cells[2, 10, 2, 11].Merge = true;
               
                ws.Row(9).Height = 22;
                
                foreach (var item in listaCuadroProgUsuario)
                {
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 4].Style.WrapText = true;
                    ws.Cells[contFila, 5].Value = item.Subestacion;
                    ws.Cells[contFila, 5].Style.WrapText = true;
                    ws.Cells[contFila, 6].Value = item.Rcproucargarechazarcoord;
                    ws.Cells[contFila, 6].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 7].Value = item.RccuadfechoriniRep;
                    ws.Cells[contFila, 7].Style.WrapText = true;
                    ws.Cells[contFila, 8].Value = item.RccuadfechorfinRep;
                    ws.Cells[contFila, 8].Style.WrapText = true;
                    ws.Cells[contFila, 9].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 9].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 10].Value = item.RcejeufechorinicioRep;
                    ws.Cells[contFila, 10].Style.WrapText = true;
                    ws.Cells[contFila, 11].Value = item.RcejeufechorfinRep;
                    ws.Cells[contFila, 11].Style.WrapText = true;
                    ws.Cells[contFila, 12].Value = item.Duracion;
                    ws.Cells[contFila, 12].Style.Numberformat.Format = @"0.00";

                    //total += item.Rcejeucargarechazada;
                    contFila++;
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(1.64M);
                ws.Column(4).Width = CallcularAnchoCentimetros(2.28M);
                ws.Column(5).Width = CallcularAnchoCentimetros(1.91M);
                ws.Column(6).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(7).Width = CallcularAnchoCentimetros(1.84M);
                ws.Column(8).Width = CallcularAnchoCentimetros(1.84M);
                ws.Column(9).Width = CallcularAnchoCentimetros(1.6M);
                ws.Column(10).Width = CallcularAnchoCentimetros(1.84M);
                ws.Column(11).Width = CallcularAnchoCentimetros(1.84M);
                ws.Column(12).Width = CallcularAnchoCentimetros(1.54M);

                if(listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[10, 3, contFila - 1, 12];
                    ObtenerEstiloCelda(rg1, 1);
                }

                

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteDemorasEjecucionRMC( List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {
                        
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteDemorasEjecucionRMC" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";
                string colorborder = "#000000";

                var contFila = 7;
                //var filainicio = 6;

                ws.Cells[2, 3].Value = "DEMORAS EN LA EJECUCIÓN DEL RMC";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;

                ws.Cells[4, 4].Value = eventoCTAF;
                ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "COORDINADO";
                ws.Cells[6, 6].Value = "EJECUTADO";
                ws.Cells[6, 7].Value = "DURACIÓN              (MIN)";

                ws.Cells[7, 5].Value = "INICIO        HH:MM:SS     DD.MM.YYYY";
                ws.Cells[7, 6].Value = "INICIO        HH:MM:SS     DD.MM.YYYY";
               
               
                ExcelRange rg1 = ws.Cells[6, 3, 7, 7];
                ObtenerEstiloCelda(rg1, 0);
                ws.Cells[6, 3, 7, 3].Merge = true;
                ws.Cells[6, 4, 7, 4].Merge = true;
                ws.Cells[6, 7, 7, 7].Merge = true;

                //ws.Cells[7, 5, 9, 5].Merge = true;
                //ws.Cells[7, 6, 9, 6].Merge = true;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 5].Value = item.Rccuadhorinicoord;
                    ws.Cells[contFila, 5].Style.WrapText = true;
                    ws.Cells[contFila, 6].Value = item.Rccuadhoriniejec;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = item.Duracion;
                    ws.Cells[contFila, 7].Style.Numberformat.Format = @"0.00";
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(3.25M);// 23;
                ws.Column(4).Width = CallcularAnchoCentimetros(3.25M);//23;
                ws.Column(5).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(6).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(7).Width = CallcularAnchoCentimetros(2.08M);//15;

                if (listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[8, 3, contFila, 7];
                    ObtenerEstiloCelda(rg1, 1);
                }                    

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteDemorasReestablecimiento(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteDemorasReestablecimiento" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 7;
                //var filainicio = 6;

                ws.Cells[2, 3].Value = "DEMORAS REESTABLECIMIENTO";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;

                ws.Cells[4, 4].Value = eventoCTAF;
                ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "COORDINADO";
                ws.Cells[6, 6].Value = "EJECUTADO";
                ws.Cells[6, 7].Value = "DURACIÓN              (MIN)";

                ws.Cells[7, 5].Value = "FINAL         HH:MM:SS     DD.MM.YYYY";
                ws.Cells[7, 6].Value = "FINAL         HH:MM:SS     DD.MM.YYYY";


                ExcelRange rg1 = ws.Cells[6, 3, 7, 7];
                ObtenerEstiloCelda(rg1, 0);
                ws.Cells[6, 3, 7, 3].Merge = true;
                ws.Cells[6, 4, 7, 4].Merge = true;
                ws.Cells[6, 7, 7, 7].Merge = true;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 5].Value = item.Rccuadhorfincoord;
                    ws.Cells[contFila, 5].Style.WrapText = true;
                    ws.Cells[contFila, 6].Value = item.Rccuadhorfinejec;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = item.Duracion;
                    ws.Cells[contFila, 7].Style.Numberformat.Format = @"0.00";

                }

                ws.Column(3).Width = CallcularAnchoCentimetros(3.25M);// 23;
                ws.Column(4).Width = CallcularAnchoCentimetros(3.25M);//23;
                ws.Column(5).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(6).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(7).Width = CallcularAnchoCentimetros(2.08M);//15;

                if (listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[8, 3, contFila, 7];
                    ObtenerEstiloCelda(rg1, 1);
                }

                    

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteInterrupcionesMenores(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteInterrupcionesMenores" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 7;
                //var filainicio = 6;

                ws.Cells[2, 3].Value = "INTERRUPCIONES MENORES 3 MINUTOS";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;

                ws.Cells[4, 4].Value = eventoCTAF;
                ws.Cells[4, 4, 4, 5].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "USUARIO";
                ws.Cells[6, 4].Value = "SUMINISTRADOR";
                ws.Cells[6, 5].Value = "POTENCIA (MW)";
                ws.Cells[6, 6].Value = "EJECUTADO";
                ws.Cells[6, 8].Value = "DURACIÓN              (MIN)";

                ws.Cells[7, 6].Value = "INICIO                       HH:MM:SS     DD.MM.YYYY";
                ws.Cells[7, 7].Value = "FINAL                        HH:MM:SS     DD.MM.YYYY";

                ExcelRange rg1 = ws.Cells[6, 3, 7, 8];
                ObtenerEstiloCelda(rg1, 0);
                ws.Cells[6, 3, 7, 3].Merge = true;
                ws.Cells[6, 4, 7, 4].Merge = true;
                ws.Cells[6, 5, 7, 5].Merge = true;
                ws.Cells[6, 8, 7, 8].Merge = true;

                ws.Cells[6, 6, 6, 7].Merge = true;
                //ws.Cells[2, 5, 4, 5].Merge = true;

                var total = decimal.Zero;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Suministrador;
                    ws.Cells[contFila, 5].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 5].Style.Numberformat.Format = @"0.000";
                    ws.Cells[contFila, 6].Value = item.RcejeufechorinicioRep;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = item.RcejeufechorfinRep;
                    ws.Cells[contFila, 7].Style.WrapText = true;
                    ws.Cells[contFila, 8].Value = item.Duracion;
                    ws.Cells[contFila, 8].Style.Numberformat.Format = @"0.00";

                    total += item.Rcejeucargarechazada;
                    
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(3.25M);//23;
                ws.Column(4).Width = CallcularAnchoCentimetros(3.74M);//26.5;
                ws.Column(5).Width = CallcularAnchoCentimetros(2.01M);//14;
                ws.Column(6).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(7).Width = CallcularAnchoCentimetros(2.5M);//18;
                ws.Column(8).Width = CallcularAnchoCentimetros(1.74M);//15;

                if(listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[8, 3, contFila, 8];
                    ObtenerEstiloCelda(rg1, 1);

                    ws.Cells[contFila + 1, 3, contFila + 1, 3].Value = "TOTAL";
                    ws.Cells[contFila + 1, 3, contFila + 1, 4].Merge = true;
                    ws.Cells[contFila + 1, 5].Value = total;
                    ws.Cells[contFila + 1, 5].Style.Numberformat.Format = @"0.000";

                    rg1 = ws.Cells[contFila + 1, 3, contFila + 1, 5];
                    string colorborder = "#000000";
                    rg1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rg1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg1.Style.Font.Size = 8;
                    rg1.Style.Font.Bold = true;
                }

               

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteInterrupcionesSuministroDecision(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteInterrupcionesSuministro" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 6;
                //var filainicio = 6;

                ws.Cells[2, 3].Value = "INTERRUPCIONES DE SUMINISTRO POR RMC PARA LA DECISIÓN";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 4].Value = "EVENTO CTAF:";
                ws.Cells[4, 4].Style.Font.Bold = true;

                ws.Cells[4, 5].Value = eventoCTAF;
                ws.Cells[4, 5, 4, 6].Merge = true;
                //var contItem = 1;

                ws.Cells[6, 3].Value = "N°";
                ws.Cells[6, 4].Value = "SUMINISTRO";
                ws.Cells[6, 5].Value = "POTENCIA (MW)";
                ws.Cells[6, 6].Value = "INICIO     HH:MM:SS     DD.MM.YYYY";
                ws.Cells[6, 7].Value = "FINAL      HH:MM:SS     DD.MM.YYYY";
                ws.Cells[6, 8].Value = "DURACIÓN              (MIN)";

                //ws.Cells[7, 6].Value = "HH:MM:SS     DD.MM.YYYY";
                //ws.Cells[7, 7].Value = "HH:MM:SS     DD.MM.YYYY";               

                ExcelRange rg1 = ws.Cells[6, 3, 6, 8];
                ObtenerEstiloCelda(rg1, 0);

                //ws.Cells[6, 3, 7, 3].Merge = true;
                //ws.Cells[6, 4, 7, 4].Merge = true;
                //ws.Cells[6, 5, 7, 5].Merge = true;
                //ws.Cells[6, 8, 7, 8].Merge = true;

                //ws.Cells[7, 6].Style.Font.Size = 7;
                //ws.Cells[7, 7].Style.Font.Size = 7;
                //ws.Cells[6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.None;
                //ws.Cells[6, 7].Style.Border.Bottom.Style = ExcelBorderStyle.None;

                var contItem = 1;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = contItem;
                    ws.Cells[contFila, 4].Value = item.Empresa;
                    ws.Cells[contFila, 4].Style.WrapText = true;
                    ws.Cells[contFila, 5].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 5].Style.Numberformat.Format = @"0.000";
                    ws.Cells[contFila, 6].Value = item.RcejeufechorinicioRep;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = item.RccuadfechorfinRep;
                    ws.Cells[contFila, 7].Style.WrapText = true;
                    ws.Cells[contFila, 8].Value = item.Duracion;
                    ws.Cells[contFila, 8].Style.Numberformat.Format = @"0.00";

                    contItem++;
                    
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(1.21M); //8.5;
                ws.Column(4).Width = CallcularAnchoCentimetros(5.26M); //58;
                ws.Column(5).Width = CallcularAnchoCentimetros(1.59M); //8.7;
                ws.Column(6).Width = CallcularAnchoCentimetros(1.82M); // 8.8;
                ws.Column(7).Width = CallcularAnchoCentimetros(1.82M); //8.8;
                ws.Column(8).Width = CallcularAnchoCentimetros(1.78M); //8.9;

                if (listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[7, 3, contFila, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }                                  

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteInterrupcionesResarcimiento(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var archivoExcel = "ReporteInterrupcionesResarcimiento" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                ws.Cells.Style.Font.Name = "Calibri";
                string colorborder = "#000000";

                var contFila = 7;
                var filainicio = 6;
                //var columnaInicio = 2;

                ws.Cells[2, 3].Value = "INTERRUPCIONES POR RMC PARA EL RESARCIMIENTO";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 9].Merge = true;
                ws.Cells[2, 3, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;

                ws.Cells[4, 4].Value = eventoCTAF;
                ws.Cells[4, 4, 4, 5].Merge = true;

                ws.Cells[filainicio, 3].Value = "SUMINISTROS AFECTADOS";
                ws.Cells[filainicio, 4].Value = "POTENCIA INTERRUMPIDA (MW)";               
                ws.Cells[filainicio, 5].Value = "HORA      INICIO                  HH:MM:SS     DD.MM.YYYY";
                ws.Cells[filainicio, 6].Value = "HORA      FINAL                 HH:MM:SS     DD.MM.YYYY";                
                ws.Cells[filainicio, 7].Value = "TIEMPO DURACIÓN   (MINUTOS)";
                ws.Cells[filainicio, 8].Value = "TIEMPO DURACIÓN   (HORAS)";
                ws.Cells[filainicio, 9].Value = "ENERGÍA NO SUMINISTRADA (MWH)";

                ws.Cells[filainicio + 1, 4].Value = "(A)";
                ws.Cells[filainicio + 1, 8].Value = "(B)";
                ws.Cells[filainicio + 1, 9].Value = "(AXB)";

                ExcelRange rg1 = ws.Cells[filainicio, 3, filainicio + 1, 9];
                ObtenerEstiloCelda(rg1, 3);

                rg1.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(colorborder));

                ws.Cells[filainicio, 3, filainicio + 1, 3].Merge = true;
                ws.Cells[filainicio, 5, filainicio + 1, 5].Merge = true;
                ws.Cells[filainicio, 6, filainicio + 1, 6].Merge = true;

                ws.Cells[filainicio, 7, filainicio + 1, 7].Merge = true;
                //ws.Cells[2, 4, 4, 4].Merge = true;
                //var totalPotencia = decimal.Zero; ws.Cells[2, 2].Value = "(A)";
                ws.Cells[filainicio + 1, 4].Style.Font.Color.SetColor(Color.Red);
                ws.Cells[filainicio + 1, 8].Style.Font.Color.SetColor(Color.Red);
                ws.Cells[filainicio + 1, 9].Style.Font.Color.SetColor(Color.Red);

                //ws.Cells[2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                //ws.Cells[2, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

                //ws.Cells[2, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[2, 6].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                //ws.Cells[2, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

                //ws.Cells[2, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[2, 7].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                //ws.Cells[2, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

                var totalEnergia = decimal.Zero;

                foreach (var item in listaCuadroProgUsuario)
                {
                    contFila++;

                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 4].Value = item.Rcejeucargarechazada;
                    ws.Cells[contFila, 4].Style.Numberformat.Format = @"0.000";
                    ws.Cells[contFila, 5].Value = item.RcejeufechorinicioRep;
                    ws.Cells[contFila, 5].Style.WrapText = true;
                    ws.Cells[contFila, 6].Value = item.RccuadfechorfinRep;//item.RcejeufechorfinRep;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = decimal.Round(item.Duracion, 2);
                    ws.Cells[contFila, 7].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 8].Value = decimal.Round(item.Duracion / 60, 2);
                    ws.Cells[contFila, 8].Style.Numberformat.Format = @"0.00";
                    ws.Cells[contFila, 9].Value = item.Rcejeucargarechazada * decimal.Round(item.Duracion / 60, 2);
                    ws.Cells[contFila, 9].Style.Numberformat.Format = @"0.000";

                    totalEnergia = totalEnergia + (item.Rcejeucargarechazada * decimal.Round(item.Duracion / 60, 2));

                    
                }

                ws.Column(3).Width = CallcularAnchoCentimetros(5.49M);
                ws.Column(4).Width = CallcularAnchoCentimetros(2.11M); //2.11
                ws.Column(5).Width = CallcularAnchoCentimetros(1.82M); //1.64
                ws.Column(6).Width = CallcularAnchoCentimetros(1.82M); //1.66
                ws.Column(7).Width = CallcularAnchoCentimetros(1.58M);
                ws.Column(8).Width = CallcularAnchoCentimetros(1.64M); //1.64
                ws.Column(9).Width = CallcularAnchoCentimetros(2.2M); //2.1
                ws.Column(10).Width = CallcularAnchoCentimetros(1.6M);
                //ws.Column(9).Width = 10;
                //contFila = contFila - 1;
                

                if(listaCuadroProgUsuario.Count > 0)
                {
                    rg1 = ws.Cells[filainicio + 2, 3, contFila, 9];
                    ObtenerEstiloCelda(rg1, 1);
                    //Totales
                    ws.Cells[contFila + 1, 3].Value = "TOTAL (MW)--->";
                    ws.Cells[contFila + 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[contFila + 1, 4].Value = listaCuadroProgUsuario.Sum(param => param.Rcejeucargarechazada);
                    ws.Cells[contFila + 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[contFila + 1, 4].Style.Numberformat.Format = @"0.000";

                    using (ExcelRange Rng = ws.Cells[contFila + 1, 8])
                    {
                        //How to add multi style text in excel cell text  
                        ExcelRichTextCollection RichTxtCollection = Rng.RichText;
                        ExcelRichText RichText = RichTxtCollection.Add("ENS");
                        RichText.Size = 8;
                        RichText.FontName = "Calibri";
                        RichText = RichTxtCollection.Add("F");
                        RichText.Size = 8;
                        RichText.FontName = "Calibri";
                        //RichText = RichTxtCollection.Insert(1, "2");                     
                        RichText.VerticalAlign = ExcelVerticalAlignmentFont.Subscript;
                        RichText = RichTxtCollection.Add(" ---->");
                        RichText.Size = 8;
                        RichText.FontName = "Calibri";

                    }
                    //ws.Cells[contFila + 1, 8].Value = "ENSF ---->";
                    ws.Cells[contFila + 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[contFila + 1, 9].Value = decimal.Round(totalEnergia, 3);
                    ws.Cells[contFila + 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[contFila + 1, 9].Style.Numberformat.Format = @"0.000";

                    ws.Cells[contFila + 1, 10].Value = "MWH";
                    ws.Cells[contFila + 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[contFila + 1, 10].Style.Font.Bold = true;


                    rg1 = ws.Cells[contFila + 1, 3, contFila + 1, 4];

                    rg1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg1.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    //rg1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rg1 = ws.Cells[contFila + 1, 8, contFila + 1, 10];

                    rg1.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    rg1.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    rg1.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    rg1.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg1.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    rg1.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    //rg1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rg1 = ws.Cells[contFila + 1, 3, contFila + 1, 10];
                    rg1.Style.Font.Size = 8;

                }



                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                //rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 8;
                //rango.Style.Font.Bold = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 8;//float.Parse("8", System.Globalization.CultureInfo.InvariantCulture);
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                //rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                //rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        private static decimal AnchoCentimetros(decimal ancho)
        {
            var nuevoAncho = decimal.Zero;

            if(ancho > 0)
            {

            }

            return nuevoAncho;
        }

        private static double CallcularAnchoCentimetros(decimal valor)
        {
            decimal factor = 5.384M;//5.05984251968488M;

            return Convert.ToDouble(factor * valor);
        }

        public const decimal FactorAnchoColumExcel = 5.384M;
        public const decimal FactorAltoFilaExcel = 29M;
    }
}