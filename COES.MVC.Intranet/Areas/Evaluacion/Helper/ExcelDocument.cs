using System.Collections.Generic;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Evaluacion.Helper
{
    public class ExcelDocument
    {
        public static string GenerarReporte(List<EprEquipoDTO> listaEquipos)
        {

            string ruta = ConfigurationManager.AppSettings[ConstantesEvaluacion.RutaReportes].ToString();

            var archivoExcel = "ReporteReactores" + ".xlsx";

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

                ws.Cells[2, 3].Value = "EVALUACIÓN DE CUMPLIMIENTO DE LOS RMC";
                ws.Cells[2, 3].Style.Font.Bold = true;
                ws.Cells[2, 3, 2, 10].Merge = true;
                ws.Cells[2, 3, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 3].Value = "EVENTO CTAF:";
                ws.Cells[4, 3].Style.Font.Bold = true;
                ws.Cells[4, 3, 4, 4].Merge = true;
                
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
                ws.Cells[8, 9, 8, 10].Merge = true;
                ws.Cells[8, 12, 8, 13].Merge = true;

                ws.Cells[8, 6, 9, 6].Merge = true;
                ws.Cells[8, 7, 9, 7].Merge = true;

                ws.Row(6).Height = 21;
                ws.Row(7).Height = 21;

                foreach (var item in listaEquipos)
                {
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 3].Style.WrapText = true;
                    ws.Cells[contFila, 14].Style.WrapText = true;
                    ws.Cells[contFila, 14].Style.Font.Bold = true;
                    contFila++;
                }

                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 15;

                if (listaEquipos.Count > 0)
                {
                    rg1 = ws.Cells[10, 3, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 1);
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
                rango.Style.Font.Size = 8;
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
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }
    }
}