using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace COES.MVC.Publico.Areas.Operacion.Helper
{
    public class OperacionHelper
    {
        public const string ReporteCostosMarginales = "CostosMarginales.xlsx";

        /// <summary>
        /// Permite generar el reporte en excel
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReportePrecioCombustible(List<PrGrupodatDTO> list)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + NombreArchivo.ReportePrecioCombustible;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PRECIO COMB.");

                    if (ws != null)
                    {
                        ws.Cells[2, 4].Value = "PRECIOS DE COMBUSTIBLES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;
                                                
                        ws.Cells[5, 2].Value = "Nro";
                        ws.Cells[5, 3].Value = "Empresa";
                        ws.Cells[5, 4].Value = "Central";
                        ws.Cells[5, 5].Value = "Tipo de Combustible";
                        ws.Cells[5, 6].Value = "Unidad de Medida";
                        ws.Cells[5, 7].Value = "Precio de Combustible";
                        ws.Cells[5, 8].Value = "Fecha de Vigencia";

                        rg = ws.Cells[5, 2, 5, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 6;
                        int contador = 1;
                        foreach (PrGrupodatDTO item in list)
                        {
                            ws.Cells[index, 2].Value = contador;
                            ws.Cells[index, 3].Value = item.Emprnomb;
                            ws.Cells[index, 4].Value = item.Centralnomb;
                            ws.Cells[index, 5].Value = item.Tipocombustible;
                            ws.Cells[index, 6].Value = item.ConcepUni;
                            ws.Cells[index, 7].Value = item.Valor.ToString("#,##0.00");
                            ws.Cells[index, 8].Value = (item.Fechadat != null) ? ((DateTime)item.Fechadat).ToString(Constantes.FormatoFecha) : string.Empty;

                            rg = ws.Cells[index, 2, index, 8];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                            contador ++;
                        }

                        if (list.Count > 0)
                        {
                            rg = ws.Cells[6, 2, index - 1, 8];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        }

                        //ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 8];
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


        /// <summary>
        /// Permite generar el reporte en excel de CMgN
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCostosMarginales(List<CmCostomarginalDTO> list, string path, string fileName)
        {
            try
            {
                string file = path + fileName;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Costos Marginales Nodales");

                    if (ws != null)
                    {
                        ws.Cells[2, 4].Value = "COSTOS MARGINALES NODALES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "Hora";
                        ws.Cells[5, 3].Value = "Barra";
                        ws.Cells[5, 4].Value = "CM Energía (S/. / MWh)";
                        ws.Cells[5, 5].Value = "CM Congestión (S/. / MWh)";
                        ws.Cells[5, 6].Value = "CM Total (S/. / MWh)";
                        
                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 6;
                        int contador = 1;
                        foreach (CmCostomarginalDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Cmgnfecha.ToString("dd/MM/yyyy HH:mm");
                            ws.Cells[index, 3].Value = item.Cnfbarnombre;
                            ws.Cells[index, 4].Value = item.Cmgnenergia;
                            ws.Cells[index, 5].Value = item.Cmgncongestion;
                            ws.Cells[index, 6].Value = item.Cmgntotal;                           

                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                            contador++;
                        }

                        if (list.Count > 0)
                        {
                            rg = ws.Cells[6, 2, index - 1, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        }

                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 6];
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

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite generar el reporte de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void GenerarReporteMasivoCM(List<CmCostomarginalDTO> list, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "COSTOS MARGINALES NODALES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "FECHA HORA";
                        ws.Cells[index, 3].Value = "NODO EMD";
                        ws.Cells[index, 4].Value = "NOMBRE BARRA";
                        ws.Cells[index, 5].Value = "ENERGÍA (S/. / MWh)";
                        ws.Cells[index, 6].Value = "CONGESTIÓN (S/. / MWh)";
                        ws.Cells[index, 7].Value = "TOTAL (S/. / MWh)";

                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (CmCostomarginalDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Cmgnfecha.ToString("dd/MM/yyyy HH:mm");
                            ws.Cells[index, 3].Value = item.Cnfbarnodo;
                            ws.Cells[index, 4].Value = item.Cnfbarnombre;
                            ws.Cells[index, 5].Value = item.Cmgnenergia;
                            ws.Cells[index, 6].Value = item.Cmgncongestion;
                            ws.Cells[index, 7].Value = item.Cmgntotal;

                            rg = ws.Cells[index, 2, index, 7];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 7];
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