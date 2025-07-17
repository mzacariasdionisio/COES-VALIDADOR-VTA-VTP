using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Helper
{
    public class CostoOportunidadExcel
    {
        /// <summary>
        /// Permite realizar la exportación de los insumos
        /// </summary>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void ExportarInsumoProceso(ProcesoModel model, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet wsPrograma = xlPackage.Workbook.Worksheets.Add("PROGRAMADO CON RESERVA");
                    ExcelWorksheet wsProgamaSinR = xlPackage.Workbook.Worksheets.Add("PROGRAMADO SIN RESERVA");
                    ExcelWorksheet wsRaDownProg = xlPackage.Workbook.Worksheets.Add("RA DOWN PROGRAMADA");
                    ExcelWorksheet wsRaUpProg = xlPackage.Workbook.Worksheets.Add("RA UP PROGRAMADA");
                    ExcelWorksheet wsRaDownEjec = xlPackage.Workbook.Worksheets.Add("RA DOWN EJECUTADA");
                    ExcelWorksheet wsRaUpEjec = xlPackage.Workbook.Worksheets.Add("RA UP EJECUTADA");

                    AgregarHojaReporte(wsPrograma, "Programado con Reserva", model.DatosPrograma, 1);
                    AgregarHojaReporte(wsProgamaSinR, "Programado sin Reserva", model.DatosProgramaSinReserva, 1);
                    AgregarHojaReporte(wsRaDownProg, "RA Down Programada", model.DatosRAProgramadaDown, 2);
                    AgregarHojaReporte(wsRaUpProg, "RA Up Programada", model.DatosRAProgramadaUp, 2);
                    AgregarHojaReporte(wsRaDownEjec, "RA Down Ejecutada", model.DatosRAEjecutadaDown, 2);
                    AgregarHojaReporte(wsRaUpEjec, "RA Up Ejecutada", model.DatosRAEjecutadaUp, 2);
                    
                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite agregar una hoja al reporte
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="data"></param>
        /// <param name="indicador"></param>
        public static void AgregarHojaReporte(ExcelWorksheet ws, string titulo, string[][] data, int indicador)
        {
            if (ws != null)
            {
                ws.Cells[2, 3].Value = titulo;

                ExcelRange rg = ws.Cells[2, 3, 3, 3];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                int index = 5;
                int col = 2;
                int colmax = data[0].Length;
                int rowmax = data.Length - 1;

                for (int i = 0; i < rowmax; i++)
                {
                    for (int j = 0; j < colmax; j++)
                    {
                        if (i < indicador)
                        {
                            ws.Cells[index + i, col + j].Value = data[i][j].Trim();
                        }
                        else
                        {
                            if (j > 0)
                            {
                                ws.Cells[index + i, col + j].Value = (!string.IsNullOrEmpty(data[i][j])) ? (decimal?)Convert.ToDecimal(data[i][j]) : null;
                            }
                            else
                            {
                                ws.Cells[index + i, col + j].Value = data[i][j];
                            }
                        }
                    }
                }

                rg = ws.Cells[index, col, index + indicador - 1, colmax + 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[index, col, index + rowmax + indicador, colmax + 1];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));                

                for (int t = 3; t <= col + 1; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }

                rg = ws.Cells[index, col, rowmax, colmax + 1];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }

        /// <summary>
        /// Permite exportar el reporte de resultados
        /// </summary>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void ExportarResultadoProceso(ProcesoModel model, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet wsDespacho = xlPackage.Workbook.Worksheets.Add("DESPACHO CON RESERVA");
                    ExcelWorksheet wsDespachoSinR = xlPackage.Workbook.Worksheets.Add("DESPACHO SIN RESERVA");

                    AgregarHojaReporteReserva(wsDespacho, "Despacho con Reserva", model.DatosDespacho, model.ColoresDespacho, 2);
                    AgregarHojaReporteReserva(wsDespachoSinR, "Despacho sin Reserva", model.DatosDespachoSinR, model.ColoresDespachoSinR, 2);
                    //AgregarHojaReporte(wsDespachoSinR, "Despacho sin Reserva", model.DatosDespachoSinR, 2);                    
                    

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite agregar una hoja al reporte
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="data"></param>
        /// <param name="indicador"></param>
        public static void AgregarHojaReporteReserva(ExcelWorksheet ws, string titulo, string[][] data, int[][] colores, int indicador)
        {
            if (ws != null)
            {
                ws.Cells[2, 3].Value = titulo;

                ExcelRange rg = ws.Cells[2, 3, 3, 3];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                int index = 5;
                int col = 2;
                int colmax = data[0].Length;
                int rowmax = data.Length - 1;

                for (int i = 0; i < rowmax; i++)
                {
                    for (int j = 0; j < colmax; j++)
                    {
                        if (i < indicador)
                        {
                            ws.Cells[index + i, col + j].Value = data[i][j].Trim();
                        }
                        else
                        {
                            if (j > 0)
                            {
                                ws.Cells[index + i, col + j].Value = (!string.IsNullOrEmpty(data[i][j])) ? (decimal?)Convert.ToDecimal(data[i][j]) : null;
                            }
                            else
                            {
                                ws.Cells[index + i, col + j].Value = data[i][j];
                            }
                        }
                    }
                }

                rg = ws.Cells[index, col, index + indicador - 1, colmax + 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;


                for (int k = 0; k < colores.Length; k++)
                {
                    for (int m = 0; m < colores[k].Length; m++)
                    {
                        if (colores[k][m] == 1)
                        {
                            rg = ws.Cells[index + indicador + k, col + 1 + m, index + indicador + k, col + 1 + m];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffd569"));
                        }
                        if (colores[k][m] == 2)
                        {
                            rg = ws.Cells[index + indicador + k, col + 1 + m, index + indicador + k, col + 1 + m];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF69B4"));
                        }
                        if (colores[k][m] == 3)
                        {
                            rg = ws.Cells[index + indicador + k, col + 1 + m, index + indicador + k, col + 1 + m];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#B8F367"));
                        }
                    }
                }


                rg = ws.Cells[index, col, index + rowmax + indicador, colmax + 1];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                for (int t = 3; t <= col + 1; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }

                rg = ws.Cells[index, col, rowmax, colmax + 1];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }

    }
}