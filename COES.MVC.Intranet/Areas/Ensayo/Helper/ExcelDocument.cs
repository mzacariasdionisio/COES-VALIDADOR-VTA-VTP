using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace COES.MVC.Intranet.Areas.Ensayo.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Configura los encabezadois y titulos del reporte excel
        /// </summary>
        /// <param name="ws"></param>
        public static void ConfiguracionHojaExcel(ExcelWorksheet ws)
        {

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnsayo.FolderReporte;

            var fill = ws.Cells[4, 2, 4, 8].Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);
            var border = ws.Cells[4, 2, 4, 8].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:H4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[4, 2].Value = "CODIGO"; ws.Cells[4, 3].Value = "EMPRESA";
            ws.Cells[4, 4].Value = "CENTRAL"; ws.Cells[4, 5].Value = "UNIDAD";
            ws.Cells[4, 6].Value = "MODO DE OPERACIÓN"; ws.Cells[4, 7].Value = "FECHA";
            ws.Cells[4, 8].Value = "ESTADO";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 10; ws.Column(3).Width = 30; ws.Column(4).Width = 30; ws.Column(5).Width = 45;
            ws.Column(6).Width = 45; ws.Column(7).Width = 25; ws.Column(8).Width = 25; ws.Column(9).Width = 18;
            ws.Column(10).Width = 18; ws.Column(11).Width = 15; ws.Column(12).Width = 15; ws.Column(13).Width = 15;
            ws.Column(14).Width = 20; ws.Column(15).Width = 10; ws.Column(16).Width = 20;

        }
        /// <summary>
        /// Genera listado de ensayos de archivo excel
        /// </summary>
        /// <param name="list"></param>
        public static void GernerarArchivoEnsayos(List<EnEnsayoDTO> list)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnsayo.FolderReporte;
            FileInfo template = new FileInfo(ruta + FormatoEnsayo.PlantillaExcelEnsayos);
            FileInfo newFile = new FileInfo(ruta + FormatoEnsayo.NombreReporte);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + FormatoEnsayo.NombreReporte);
            }
            int row = 5;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("ENSAYOS");
                ws = xlPackage.Workbook.Worksheets["ENSAYOS"];
                ws.Cells[1, 3].Value = "REPORTE DE ENSAYOS ";
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws);

                foreach (var reg in list)
                {

                    ws.Cells[row, column].Value = reg.Ensayocodi;
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Equinomb;
                    ws.Cells[row, column + 3].Value = reg.Unidadnomb;
                    ws.Cells[row, column + 4].Value = reg.Ensayomodoper;
                    ws.Cells[row, column + 5].Value = ((DateTime)reg.Ensayofecha).ToString(Constantes.FormatoFechaHora);
                    ws.Cells[row, column + 6].Value = reg.Estadonombre;

                    var border = ws.Cells[row, 2, row, 8].Style.Border;
                    ws.Cells[row, 2, row, 8].Style.WrapText = true;
                    ws.Cells[row, 2, row, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, column + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, column + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 8].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";

                    row++;
                }

                xlPackage.Save();
            }
        }

    }


}