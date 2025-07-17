using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Publico.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using COES.Servicios.Aplicacion.RegistroIntegrantes;

namespace COES.MVC.Publico.Areas.RegistroIntegrante.Helper
{
    public class ExcelDocumentEmpresasPublicas
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>

        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, string ruta)
        {
            //AddImage(ws,1, 0,ruta + Constantes.NombreLogoCoes);
            var border = ws.Cells[4, 2, 4, 5].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:E4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "NRO.";
            ws.Cells[4, 3].Value = "RAZÓN SOCIAL";
            ws.Cells[4, 4].Value = "RUC";
            ws.Cells[4, 5].Value = "FECHA DE INGRESO";           

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
            ws.Column(3).Width = 40;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 20;
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoEnvios(List<SiEmpresaDTO> list, string ruta, string titulo)
        {

            FileInfo newFile = new FileInfo(ruta + "ReporteEmpresas.Xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + "ReporteEmpresas.Xlsx");
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = titulo;
                ws.Cells[2, 2].Value = "FECHA:";
                ws.Cells[2, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                int contador = 0;
                foreach (var reg in list)
                {
                    contador++;
                    ws.Cells[row, column].Value = contador;
                    ws.Cells[row, column].AutoFitColumns();
                    ws.Cells[row, column + 1].Value = reg.Emprrazsocial;
                    ws.Cells[row, column + 2].Value = reg.Emprruc;
                    ws.Cells[row, column + 3].Value = reg.Emprfecingreso == null ? "" : ((DateTime)reg.Emprfecingreso).ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 3].AutoFitColumns();
                 
                    var border = ws.Cells[row, 2, row, 5].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 5].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                xlPackage.Save();
            }
        }
    }

}