using COES.Dominio.DTO.Sic;
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

namespace COES.MVC.Intranet.Areas.DemandaBarras.Helper
{
    public class DemandaBarrasHelper
    {
        /// <summary>
        /// Permite generar el archivo de exportación del reporte de cumplimiento
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCumplimiento(List<MeEnvioDTO> list, string file)
        {
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }
            
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CUMPLIMIENTO");

                if (ws != null)
                {
                    ws.Cells[5, 2].Value = "REPORTE DE CUMPLIMIENTO";

                    ExcelRange rg = ws.Cells[5, 2, 5, 2];
                    rg.Style.Font.Size = 13;
                    rg.Style.Font.Bold = true;

                    int index = 7;
                    ws.Cells[index, 2].Value = "Empresa";
                    ws.Cells[index, 3].Value = "Cumplimiento";
                    ws.Cells[index, 4].Value = "Estado Envío";
                    ws.Cells[index, 5].Value = "Plazo";
                    ws.Cells[index, 6].Value = "Periodo";
                    ws.Cells[index, 7].Value = "Fecha Envío";
                    ws.Cells[index, 8].Value = "Usuario";

                    rg = ws.Cells[index, 2, index, 8];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    index++;

                    foreach (MeEnvioDTO item in list)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb;
                        ws.Cells[index, 3].Value = (item.Indcumplimiento == 1) ? "Si" : "No";
                        ws.Cells[index, 4].Value = (item.Estenvnombre != null) ? item.Estenvnombre : string.Empty;
                        ws.Cells[index, 5].Value = (item.Indcumplimiento == 1) ? (item.Envioplazo == "P") ? "En plazo" : "Fuera de Plazo" : "";
                        ws.Cells[index, 6].Value = (item.Enviofechaperiodo != null) ? ((DateTime)item.Enviofechaperiodo).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                        ws.Cells[index, 7].Value = (item.Enviofecha != null) ? ((DateTime)item.Enviofecha).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                        ws.Cells[index, 8].Value = (item.Lastuser != null) ? item.Lastuser : string.Empty;

                        rg = ws.Cells[index, 3, index, 3];                      
                        if (item.Indcumplimiento == 1)
                        {                            
                            rg.Style.Font.Color.SetColor(Color.Green);
                            rg = ws.Cells[index, 5, index, 5];

                            if (item.Envioplazo == "P")                            
                                rg.Style.Font.Color.SetColor(Color.Green);                            
                            else                            
                                rg.Style.Font.Color.SetColor(Color.Red);                            
                        }
                        else 
                        {
                            rg.Style.Font.Color.SetColor(Color.Red);
                        }
                                                
                        rg = ws.Cells[index, 2, index, 8];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        index++;
                    }
                                                           
                    rg = ws.Cells[1, 2, index + 2, 8];
                    rg.AutoFitColumns();
                    
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);

                }

                xlPackage.Save();
            }

        }

    }


    /// <summary>
    /// Almacena todas las constantes del modulo
    /// </summary>
    public class ConstantesDemandaBarras
    {
        public const string NombreExportacionCumplimiento = "CumplimientoMedidoresDistribucion.xlsx";
    }
}