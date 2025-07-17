using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using System.Net;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Helper
{
    public class ExcelDocument
    {

        /// <summary>
        /// Permite generar el reporte en excel de Seguimiento de recomendaciones
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteSeguimiento(List<SrmRecomendacionDTO> list, DateTime fechaDesde, DateTime fechaHasta, int indicador)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteSeguimientoRec;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Seguimiento");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "SEGUIMIENTO DE RECOMENDACIONES" + (indicador == 0 ? " (pendientes de asignar recomendación)" : "");

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "INICIO: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd/MM/yyyy");
                        ws.Cells[6, 2].Value = "FIN: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd/MM/yyyy");

                        rg = ws.Cells[5, 3, 6, 3];
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

                        ws.Cells[8, 2].Value = "Tipo";
                        ws.Cells[8, 3].Value = "Empresa";
                        ws.Cells[8, 4].Value = "Ubicación";
                        ws.Cells[8, 5].Value = "Equipo";
                        ws.Cells[8, 6].Value = "Inicio";
                        ws.Cells[8, 7].Value = "Final";
                        ws.Cells[8, 8].Value = "Descripción";
                        int lastcolumn = 8;
                        if (indicador == 1)
                        {
                            ws.Cells[8, 9].Value = "Fecha recomendación";
                            ws.Cells[8, 10].Value = "Plazo atención";
                            ws.Cells[8, 11].Value = "Equipo";
                            ws.Cells[8, 12].Value = "Estado";
                            ws.Cells[8, 13].Value = "Criticidad";
                            ws.Cells[8, 14].Value = "Plazo";
                            lastcolumn = 14;
                        }
                        

                        rg = ws.Cells[8, 2, 8, lastcolumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (SrmRecomendacionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Tipo;
                            ws.Cells[index, 3].Value = item.Emprnomb;
                            ws.Cells[index, 4].Value = item.Areanomb;
                            ws.Cells[index, 5].Value = item.Equiabrev;
                            ws.Cells[index, 6].Value = (item.Evenini != null) ? ((DateTime)item.Evenini).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 7].Value = (item.Evenfin != null) ? ((DateTime)item.Evenfin).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 8].Value = item.EvenAsunto;

                            if(indicador==1){
                                ws.Cells[index, 9].Value = (item.Srmrecfecharecomend != null) ? ((DateTime)item.Srmrecfecharecomend).ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[index, 10].Value = (item.Srmrecfechavencim != null) ? ((DateTime)item.Srmrecfechavencim).ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[index, 11].Value = (item.Equinomb != null) ? item.Equinomb : string.Empty;


                            if (item.Srmstdcolor != null)
                            {
                                ExcelRange rg2 = ws.Cells[index, 12, index, 12];
                                //rg2 = ws.Cells[index, index, 12, 12];
                                rg2.Style.Font.Size = 10;
                                rg2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#" + item.Srmstdcolor));
                                //rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#" + item.Srmstdcolor));
                                ws.Cells[index, 12].Value = item.Srmstddescrip;
                            }

                            if (item.Srmcrtcolor != null)
                            {
                                ExcelRange rg3 = ws.Cells[index, 13, index, 13];
                                //rg3 = ws.Cells[index, index, 13, 13];
                                rg3.Style.Font.Size = 10;
                                rg3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg3.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#" + item.Srmcrtcolor));
                                //rg3.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#" + item.Srmcrtcolor));
                                ws.Cells[index, 13].Value = item.Srmcrtdescrip;
                            }

                            ws.Cells[index, 14].Value = (item.Srmrecdianotifplazo != null) ? item.Srmrecdianotifplazo.ToString() : string.Empty;
                            }
                                                        
                            rg = ws.Cells[index, 2, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, lastcolumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        //ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, lastcolumn];
                        rg.AutoFitColumns();

                        try
                        {
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
                        catch { }

                    }


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el reporte en excel de Seguimiento de recomendaciones. Listado
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteSeguimientoListado(List<SrmRecomendacionDTO> list, DateTime fechaDesde, DateTime fechaHasta, int indicador)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteSeguimientoRecReporte;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "LISTADO DE RECOMENDACIONES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "INICIO: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd/MM/yyyy");
                        ws.Cells[6, 2].Value = "FIN: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd/MM/yyyy");

                        rg = ws.Cells[5, 3, 6, 3];
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

                        ws.Cells[8, 2].Value = "Empresa";
                        ws.Cells[8, 3].Value = "Subestación";
                        ws.Cells[8, 4].Value = "Equipo";
                        ws.Cells[8, 5].Value = "Título Recomendación";
                        ws.Cells[8, 6].Value = "Responsable";
                        ws.Cells[8, 7].Value = "Fecha vencimiento";
                            ws.Cells[8, 8].Value = "Plazo (días)";
                           
                            ws.Cells[8, 9].Value = "Criticidad";
                         ws.Cells[8, 10].Value = "Estado";
                          int  lastcolumn = 10;
                        


                        rg = ws.Cells[8, 2, 8, lastcolumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (SrmRecomendacionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprnomb;
                            ws.Cells[index, 3].Value = item.Areanomb;
                            ws.Cells[index, 4].Value = item.Equiabrev;
                            ws.Cells[index, 5].Value = (item.Srmrectitulo != null) ? (item.Srmrectitulo ): string.Empty; ;
                            ws.Cells[index, 6].Value = (item.Username != null) ? (item.Username ): string.Empty;
                            ws.Cells[index, 7].Value = (item.Srmrecfechavencim != null) ? ((DateTime)item.Srmrecfechavencim).ToString("dd/MM/yyyy") : string.Empty;
                            ws.Cells[index, 8].Value = (item.Srmrecdianotifplazo != null) ? (item.Srmrecdianotifplazo.ToString()) : string.Empty; 


                            
                            ExcelRange rg3 = ws.Cells[index, 9, index, 9];
                            ExcelRange rg2 = ws.Cells[index, 10, index, 10];

                            if (item.Srmcrtcolor != null)
                            {
                                rg3.Style.Font.Size = 10;
                                rg3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg3.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#" + item.Srmcrtcolor));
                                ws.Cells[index, 9].Value = item.Srmcrtdescrip;
                            }

                            if (item.Srmstdcolor != null)
                            {
                                rg2.Style.Font.Size = 10;
                                rg2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#" + item.Srmstdcolor));
                                ws.Cells[index, 10].Value = item.Srmstddescrip;
                            }

                            rg = ws.Cells[index, 2, index, lastcolumn];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, lastcolumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        rg = ws.Cells[7, 3, index, lastcolumn];
                        rg.AutoFitColumns();

                        try
                        {
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
                        catch { }

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