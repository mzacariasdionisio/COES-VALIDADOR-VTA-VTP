using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Helper
{
    /// <summary>
    /// Metodo de ayuda para el comparativo RPF y Ejecutado
    /// </summary>
    public class ComparativoHelper
    {
        /// <summary>
        /// Permite obtener el Comparativo
        /// </summary>
        /// <param name="listRPF"></param>
        /// <param name="despacho"></param>
        /// <returns></returns>
        public static List<ComparativoItemModel> ObtenerComparativo(DateTime fecha, List<ServicioCloud.Medicion> listRPF, MeMedicion48DTO despacho)
        {
            List<ComparativoItemModel> entitys = new List<ComparativoItemModel>();

            if (listRPF.Count == 48 && despacho != null)
            {
                for (int i = 1; i <= 48; i++)
                {
                    ComparativoItemModel entity = new ComparativoItemModel();
                    entity.Hora = fecha.AddMinutes((i) * 30).ToString(Constantes.FormatoHoraMinuto);
                    entity.ValorRPF = listRPF[i - 1].H0;
                    entity.ValorDespacho = Convert.ToDecimal(despacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(despacho, null));
                    entity.Desviacion = (entity.ValorRPF != 0) ? (entity.ValorDespacho - entity.ValorRPF) / entity.ValorRPF : 0;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void ExportarComparativo(List<ComparativoItemModel> list, string path, string filename, string fecha)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COMPARATIVO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "COMPARATIVO SERVICIO RPF - DESPACHO EJECUTADO";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "HORA";
                        ws.Cells[index, 3].Value = "SERVICIO RPF";
                        ws.Cells[index, 4].Value = "DESPACHO EJECUTADO";
                        ws.Cells[index, 5].Value = "DESVIACIÓN";                        

                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ComparativoItemModel item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            ws.Cells[index, 3].Value = item.ValorRPF;
                            ws.Cells[index, 4].Value = item.ValorDespacho;
                            ws.Cells[index, 5].Value = (item.ValorRPF != 0) ? (item.ValorDespacho - item.ValorRPF) * 100 / item.ValorRPF : 0;
                            
                            rg = ws.Cells[index, 2, index, 5];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            rg = ws.Cells[index, 5, index, 5];
                            rg.Style.Numberformat.Format = "#0\\.00%";

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 5];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        
                        var lineChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.LineMarkers) as ExcelLineChart;
                        lineChart.SetPosition(5, 0, 6, 0);
                        lineChart.SetSize(650, 400);
                        lineChart.Series.Add(ExcelRange.GetAddress(6, 3, index - 1, 3), ExcelRange.GetAddress(6, 2, index - 1, 2));
                        lineChart.Series.Add(ExcelRange.GetAddress(6, 4, index - 1, 4), ExcelRange.GetAddress(6, 2, index - 1, 2));
                        lineChart.Series[0].Header = "Servicio RPF";
                        lineChart.Series[1].Header = "Despacho Ejecutado";

                        lineChart.Title.Text = "Comparativo RPF - Despacho";   
                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 5];
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
        /// Permite exportar los datos de RPF
        /// </summary>
        /// <param name="list"></param>
        /// <param name="relacion"></param>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void ExportarDatosRPF(List<ServicioCloud.Medicion> list, List<MePtorelacionDTO> relacion, string fecha, string path, string filename)
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
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DATOS RPF");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "DATOS RPF - MEDIO HORARIO";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;
                        ws.Cells[index + 1, 2].Value = "HORA";

                        var centrales = relacion.Select(x => new { x.Equicodi, x.Equinomb, x.Emprnomb }).Distinct().ToList();

                        int columna = 3;
                        foreach (var central in centrales)
                        {
                            ws.Cells[index, columna].Value = central.Emprnomb.Trim();
                            ws.Cells[index + 1, columna].Value = central.Equinomb.Trim();
                            columna++;
                        }

                        DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        
                        index = 7;
                        for (int i = 1; i <= 48; i++)
                        {
                            ws.Cells[index, 2].Value = fechaConsulta.AddMinutes(i * 30).ToString(Constantes.FormatoOnlyHora);
                            rg = ws.Cells[index, 2, index, 2];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                            index++;
                        }

                        rg = ws.Cells[5, 2, 6, columna - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        
                        columna = 3;
                        foreach (var central in centrales)
                        {
                            List<int> puntos = relacion.Where(x => x.Equicodi == central.Equicodi).Select(x => (int)x.Ptomedicodi).ToList();
                            List<ServicioCloud.Medicion> datos = list.Where(x => puntos.Any(y => x.PTOMEDICODI == y)).ToList();
                                                        
                            index = 7;    
                            for (int i = 1; i <= 48; i++)
                            {
                                decimal valor = datos.Sum(x => (decimal)x.GetType().GetProperty(Constantes.CaracterH + i).GetValue(x));                                
                                ws.Cells[index, columna].Value = valor;
                                index++;
                            }

                            rg = ws.Cells[7, columna, index - 1, columna];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                            columna++;
                        }
                       
                        rg = ws.Cells[5, 2, index - 1, columna -1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                                               
                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, columna];
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