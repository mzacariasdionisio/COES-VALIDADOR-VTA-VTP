using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Dominio.DTO.Sic;
using System.Configuration;
using System.IO;
using System.Net;
using OfficeOpenXml.Drawing;

namespace COES.MVC.Publico.Helper
{
    public class ExcelHelper
    {
        //The correct method to convert width to pixel is:
        //Pixel =Truncate(((256 * {width} + Truncate(128/{Maximum DigitWidth}))/256)*{Maximum Digit Width})
        //The correct method to convert pixel to width is:
        //1. use the formula =Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
        // to convert pixel to character number.
        //2. use the formula width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
        // to convert the character number to width.

        // Escala
        public const int MTU_PER_PIXEL = 9525;
        public static int ColumnWidth2Pixel(ExcelWorksheet ws, double excelColumnWidth)
        {
            //The correct method to convert width to pixel is:
            //Pixel =Truncate(((256 * {width} + Truncate(128/{Maximum DigitWidth}))/256)*{Maximum Digit Width})
            //get the maximum digit width
            decimal mdw = ws.Workbook.MaxFontWidth;
            //convert width to pixel
            decimal pixels = decimal.Truncate(((256 * (decimal)excelColumnWidth + decimal.Truncate(128 / mdw)) / 256) * mdw);
            //double columnWidthInTwips = (double)(pixels * (1440f / 96f));
            return Convert.ToInt32(pixels);
        }
        public static double Pixel2ColumnWidth(ExcelWorksheet ws, int pixels)
        {
            //The correct method to convert pixel to width is:
            //1. use the formula =Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
            // to convert pixel to character number.
            //2. use the formula width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
            // to convert the character number to width.
            //get the maximum digit width
            decimal mdw = ws.Workbook.MaxFontWidth;
            //convert pixel to character number
            decimal numChars = decimal.Truncate(decimal.Add((pixels - 5) / mdw * 100, (decimal)0.5)) / 100;
            //convert the character number to width
            decimal excelColumnWidth = decimal.Truncate((decimal.Add(numChars * mdw, 5)) / mdw * 256) / 256;
            return Convert.ToDouble(excelColumnWidth);
        }
        public static int RowHeight2Pixel(double excelRowHeight)
        {
            //convert height to pixel
            decimal pixels = decimal.Truncate((decimal)(excelRowHeight / 0.75));
            return Convert.ToInt32(pixels);
        }
        public static double Pixel2RowHeight(int pixels)
        {
            //convert height to pixel
            double excelRowHeight = pixels * 0.75;
            return excelRowHeight;
        }
        public static int MTU2Pixel(int mtus)
        {
            //convert MTU to pixel
            decimal pixels = decimal.Truncate(mtus / MTU_PER_PIXEL);
            return Convert.ToInt32(pixels);
        }
        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        // Formato

        public static void SetFormatCells(ExcelWorksheet ws,int fromX, int fromY, int toX, int toY,int sizeFont,
            string nameFont, Color color)
        {
            var border = ws.Cells[fromX, fromY, toX, toY].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[fromX, fromY, toX, toY].Style.Font;
            fontTabla.Size = sizeFont;
            fontTabla.Name = nameFont;
            fontTabla.Color.SetColor(color);        
        }

        public static void GenerarReporteEvento(List<EventoDTO> list, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + NombreArchivo.ReporteEvento;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EVENTO FALLAS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "CONSULTA DE FALLAS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "FECHA DESDE: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd'/'MM'/'yyyy");
                        ws.Cells[6, 2].Value = "FECHA HASTA: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd'/'MM'/'yyyy");

                        rg = ws.Cells[5, 2, 6, 2];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#BBDF8D"));

                        ws.Cells[8, 2].Value = "CIER";
                        ws.Cells[8, 3].Value = "Tensión (KV)";
                        ws.Cells[8, 4].Value = "Tipo de Empresa";
                        ws.Cells[8, 5].Value = "Empresa";
                        ws.Cells[8, 6].Value = "Ubicación";
                        ws.Cells[8, 7].Value = "Familia";
                        ws.Cells[8, 8].Value = "Equipo";
                        ws.Cells[8, 9].Value = "Inicio";
                        ws.Cells[8, 10].Value = "Final";
                        ws.Cells[8, 11].Value = "Interrupción (MW)";
                        ws.Cells[8, 12].Value = "Disminución (MW)";
                        ws.Cells[8, 13].Value = "Duración (Minutos)";
                        ws.Cells[8, 14].Value = "Energía No Suministrada (MWh)";
                        ws.Cells[8, 15].Value = "Descripción";

                        rg = ws.Cells[8, 2, 8, 15];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (EventoDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.CAUSAEVENABREV;
                            ws.Cells[index, 3].Value = item.EQUITENSION;
                            ws.Cells[index, 4].Value = item.TIPOEMPRDESC;
                            ws.Cells[index, 5].Value = item.EMPRNOMB;
                            ws.Cells[index, 6].Value = item.TAREAABREV + " " + item.AREANOMB;
                            ws.Cells[index, 7].Value = item.FAMNOMB;
                            ws.Cells[index, 8].Value = item.EQUIABREV;
                            ws.Cells[index, 9].Value = (item.EVENINI != null) ? ((DateTime)item.EVENINI).ToString("dd'/'MM'/'yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 10].Value = (item.EVENFIN != null) ? ((DateTime)item.EVENFIN).ToString("dd'/'MM'/'yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 11].Value = item.INTERRUPCIONMW;
                            ws.Cells[index, 12].Value = item.DISMINUCIONMW;
                            TimeSpan t = item.EVENFIN.Value - item.EVENINI.Value;
                            ws.Cells[index, 13].Value = 1440 * t.Days + 60 * t.Hours + t.Minutes;
                            ws.Cells[index, 14].Value = item.ENERGIAINTERRUMPIDA;
                            ws.Cells[index, 15].Value = item.EVENASUNTO;

                            rg = ws.Cells[index, 2, index, 15];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, 15];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, 15];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}