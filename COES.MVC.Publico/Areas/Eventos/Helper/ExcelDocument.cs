using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Publico.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Eventos.Models;
using System.Net;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Publico.Areas.Evento.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Permite generar el reporte en excel
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteEvento(List<EventoDTO> list, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteEvento;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EVENTOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "CONSULTA DE EVENTOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[5, 2].Value = "FECHA DESDE: ";
                        ws.Cells[5, 3].Value = fechaDesde.ToString("dd/MM/yyyy");
                        ws.Cells[6, 2].Value = "FECHA HASTA: ";
                        ws.Cells[6, 3].Value = fechaHasta.ToString("dd/MM/yyyy");

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

                        ws.Cells[8, 2].Value = "Empresa";
                        ws.Cells[8, 3].Value = "Ubicación";
                        ws.Cells[8, 4].Value = "Familia";
                        ws.Cells[8, 5].Value = "Equipo";
                        ws.Cells[8, 6].Value = "Inicio";
                        ws.Cells[8, 7].Value = "Final";                        
                        ws.Cells[8, 8].Value = "Descripción";
                        
                        rg = ws.Cells[8, 2, 8, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int index = 9;
                        foreach (EventoDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.EMPRNOMB;
                            ws.Cells[index, 3].Value = item.TAREAABREV + " " + item.AREANOMB;
                            ws.Cells[index, 4].Value = item.FAMABREV;
                            ws.Cells[index, 5].Value = item.EQUIABREV;
                            ws.Cells[index, 6].Value = (item.EVENINI != null) ? ((DateTime)item.EVENINI).ToString("dd/MM/yyyy HH:mm") : string.Empty;
                            ws.Cells[index, 7].Value = (item.EVENFIN != null) ? ((DateTime)item.EVENFIN).ToString("dd/MM/yyyy HH:mm") : string.Empty;                          
                            ws.Cells[index, 8].Value = item.EVENASUNTO;                            

                            rg = ws.Cells[index, 2, index, 8];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[8, 2, index - 1, 8];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[7, 3, index, 8];
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

    /// <summary>
    /// Clase para manejar las horas de la exportación
    /// </summary>
    public class HoraExcel
    {
        public int Hora { get; set; }
        public int Minuto { get; set; }
        public decimal Valor { get; set; }
        public int IdEquipo { get; set; }

        public List<HoraExcel> ListaHoras()
        {
            List<HoraExcel> list = new List<HoraExcel>();
            list.Add(new HoraExcel { Hora = 0, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 1, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 1, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 2, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 2, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 3, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 3, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 4, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 4, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 5, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 5, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 6, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 6, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 7, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 7, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 8, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 8, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 9, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 9, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 10, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 10, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 11, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 11, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 12, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 12, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 13, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 13, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 14, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 14, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 15, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 15, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 16, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 16, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 17, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 17, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 18, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 18, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 19, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 19, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 20, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 20, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 21, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 21, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 22, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 22, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 23, Minuto = 0 });
            list.Add(new HoraExcel { Hora = 23, Minuto = 30 });
            list.Add(new HoraExcel { Hora = 0, Minuto = 0 });     

            return list;
        
        }
    }
}