using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using COES.Servicios.Aplicacion.Despacho;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Despacho.Helper;

namespace COES.MVC.Intranet.Areas.Despacho.Helper
{
    public class ExcelDocumentCostoIncremental
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
            var border = ws.Cells[4, 2, 4, 7].Style.Border;

            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells["B4:Q4"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r.AutoFitColumns();
            }
            ws.Cells[4, 2].Value = "EMPRESA";
            ws.Cells[4, 3].Value = "GRUPO MODO OPERACIÓN";
            ws.Cells[4, 4].Value = "CEC kJ/kWh";
            ws.Cells[4, 5].Value = "Pe MW";
            ws.Cells[4, 6].Value = "RENDIMIENTO KWH/Uni";
            ws.Cells[4, 7].Value = "PRECIO S/./ Uni";
            ws.Cells[4, 8].Value = "CVNC S/./ KWh";
            ws.Cells[4, 9].Value = "CVC S/./ KWh";
            ws.Cells[4, 10].Value = "CV S/./ KWh";
            ws.Cells[4, 11].Value = "Tramo 1 MW";
            ws.Cells[4, 12].Value = "Cincrem.-1 S/./ MWh";
            ws.Cells[4, 13].Value = "Tramo 2 MW";
            ws.Cells[4, 14].Value = "Cincrem.-2 S/./ MWh";
            ws.Cells[4, 15].Value = "Tramo 3 MW";
            ws.Cells[4, 16].Value = "Cincrem.-3 S/./ MWh";
            ws.Cells[4, 17].Value = "Tipo de Combustible";

            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
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

        public static void GernerarArchivoEnvios(List<EntidadReporteModel> list, string ruta, string titulo, string fecha)
        {

            FileInfo newFile = new FileInfo(ruta + ConstantesDespacho.NombreReporteEnvios);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesDespacho.NombreReporteEnvios);
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
                ws.Cells[2, 3].Value = fecha;
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, ruta);

                foreach (var reg in list)
                {

                    ws.Cells[row, column].Value = reg.Empresa == null ? "" : reg.Empresa.ToString();
                    ws.Cells[row, column].AutoFitColumns();
                    ws.Cells[row, column + 1].Value = reg.GrupoModoOperacion == null ? "" : reg.GrupoModoOperacion.ToString();
                    ws.Cells[row, column + 1].AutoFitColumns();
                    ws.Cells[row, column + 2].Value = reg.CEC == null ? "" : reg.CEC.ToString();
                    ws.Cells[row, column + 2].AutoFitColumns();
                    ws.Cells[row, column + 3].Value = reg.Pe == null ? "" : reg.Pe.ToString();
                    ws.Cells[row, column + 3].AutoFitColumns();
                    ws.Cells[row, column + 4].Value = reg.Rendimiento == null ? "" : reg.Rendimiento.ToString();
                    ws.Cells[row, column + 4].AutoFitColumns();
                    ws.Cells[row, column + 5].Value = reg.Precio == null ? "" : reg.Precio.ToString();
                    ws.Cells[row, column + 5].AutoFitColumns();
                    ws.Cells[row, column + 6].Value = reg.CVNC == null ? "" : reg.CVNC.ToString();
                    ws.Cells[row, column + 6].AutoFitColumns();
                    ws.Cells[row, column + 7].Value = reg.CVC == null ? "" : reg.CVC.ToString();
                    ws.Cells[row, column + 7].AutoFitColumns();
                    ws.Cells[row, column + 8].Value = reg.CV == null ? "" : reg.CV.ToString();
                    ws.Cells[row, column + 8].AutoFitColumns();
                    ws.Cells[row, column + 9].Value = reg.Tramo1 == null ? "" : reg.Tramo1.ToString();
                    ws.Cells[row, column + 9].AutoFitColumns();
                    ws.Cells[row, column + 10].Value = reg.Cincrem1 == null ? "" : reg.Cincrem1.ToString();
                    ws.Cells[row, column + 10].AutoFitColumns();
                    ws.Cells[row, column + 11].Value = reg.Tramo2 == null ? "" : reg.Tramo2.ToString();
                    ws.Cells[row, column + 11].AutoFitColumns();
                    ws.Cells[row, column + 12].Value = reg.Cincrem2 == null ? "" : reg.Cincrem2.ToString();
                    ws.Cells[row, column + 12].AutoFitColumns();
                    ws.Cells[row, column + 13].Value = reg.Tramo3 == null ? "" : reg.Tramo3.ToString();
                    ws.Cells[row, column + 13].AutoFitColumns();
                    ws.Cells[row, column + 14].Value = reg.Cincrem3 == null ? "" : reg.Cincrem3.ToString();
                    ws.Cells[row, column + 14].AutoFitColumns();
                    ws.Cells[row, column + 15].Value = reg.TipoCombustible == null ? "" : reg.TipoCombustible.ToString();
                    ws.Cells[row, column + 15].AutoFitColumns();



                    var border = ws.Cells[row, 2, row, 17].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 17].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }

                ExcelRange rg = ws.Cells[4, 2, row, 17];
                rg.AutoFitColumns();

                xlPackage.Save();
            }
        }
    }

}