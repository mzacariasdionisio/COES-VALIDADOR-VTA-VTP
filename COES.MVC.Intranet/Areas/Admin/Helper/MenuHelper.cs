using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Admin.Helper
{
    public class MenuHelper
    {
        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ObtenerTreeOpciones(List<OptionDTO> list, string nodos)
        {
            int idPadre = 1;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("[\n");

            List<OptionDTO> listItem = list.Where(x => x.PadreCodi == idPadre).ToList();
            int contador = 0;
            foreach (OptionDTO item in listItem)
            {
                List<OptionDTO> listHijos = list.Where(x => x.PadreCodi == item.OptionCode).ToList();
                if (listHijos.Count > 0)
                {
                    strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" + item.OptionName +
                        "' , 'expanded' : 'true', selected : " + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + ", 'children':[\n");
                    strHtml.Append(ObtenerSubMenu(listHijos, list, "   ", nodos));
                    if (contador < listItem.Count - 1) strHtml.Append("   ]},\n");
                    else strHtml.Append("   ]}\n");
                }
                else
                {
                    if (contador < listItem.Count - 1)
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "' , selected : " + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "',  selected : " + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + "}\n");
                }
                contador++;
            }

            strHtml.Append("]");
            return strHtml.ToString();
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ObtenerSubMenu(List<OptionDTO> list, List<OptionDTO> listGeneral, string pad, string nodos)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            foreach (OptionDTO item in list)
            {
                List<OptionDTO> listHijos = listGeneral.Where(x => x.PadreCodi == item.OptionCode).ToList();

                if (listHijos.Count > 0)
                {
                    strHtml.Append(pad + "    {'key': '" + item.OptionCode + "' , selected :" + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + ", 'title': '" +
                        item.OptionName + "', 'children':[\n");
                    strHtml.Append(this.ObtenerSubMenu(listHijos, listGeneral, pad + "  ", nodos));
                    if (contador < list.Count - 1) strHtml.Append(pad + "    ]},\n");
                    else strHtml.Append(pad + "    ]}\n");
                }
                else
                {
                    if (contador < list.Count - 1)
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                           item.OptionName + "' , selected : " + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.OptionCode + "', 'title': '" +
                            item.OptionName + "',  selected : " + this.ObtieneSeleccionNodo(item.OptionCode, item.Selected, nodos) + "}\n");
                }
                contador++;
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite verificar la selección de un nodo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        /// <param name="nodos"></param>
        /// <returns></returns>
        private string ObtieneSeleccionNodo(int id, int selected, string nodos)
        {
            string[] nodes = nodos.Split(',');

            if (nodes.Contains(id.ToString()) || selected > 0)
            {
                return "true";
            }
            return "false";
        }


        /// <summary>
        /// Permite obtener las descripciones para los nodos
        /// </summary>
        /// <param name="list"></param>
        public List<OptionDTO> ObtenerReporteExcel(List<OptionDTO> list)
        {
            List<OptionDTO> result = new List<OptionDTO>();
            int idPadre = 1;

            List<OptionDTO> listItem = list.Where(x => x.PadreCodi == idPadre).ToList();
            int contador = 0;
            foreach (OptionDTO item in listItem)
            {
                item.DesIcon = Constantes.SI;
                result.Add(item);

                List<OptionDTO> listHijos = list.Where(x => x.PadreCodi == item.OptionCode).ToList();
                if (listHijos.Count > 0)
                {                   
                    result.AddRange(ObtenerReporteExcel(listHijos, list, "      "));
                }
                
                contador++;
            }

            return result;
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<OptionDTO>  ObtenerReporteExcel(List<OptionDTO> list, List<OptionDTO> listGeneral, string padding)
        {
            List<OptionDTO> result = new List<OptionDTO>();
            int contador = 0;
            foreach (OptionDTO item in list)
            {

                item.DesIcon = Constantes.SI;
                item.OptionName = padding + item.OptionName;
                result.Add(item);

                List<OptionDTO> listHijos = listGeneral.Where(x => x.PadreCodi == item.OptionCode).ToList();

                if (listHijos.Count > 0)
                {                    
                    result.AddRange(this.ObtenerReporteExcel(listHijos, listGeneral, padding + "    "));
                }
               
                contador++;
            }

            return result;
        }
        public static void GenerarReporteExcelSinOpcion(List<OptionDTO> list, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteUso;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                newFile.Delete();
                newFile = new FileInfo(file);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("USUARIOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE ACCESOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "NRO";
                        ws.Cells[index, 3].Value = "OPCIÓN";
                        ws.Cells[index, 4].Value = "CANTIDAD DE ACCESOS";
                        ws.Cells[index, 5].Value = "ÚLTIMO USUARIO";
                        ws.Cells[index, 6].Value = "ÚLTIMO FECHA";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        int contador = 0;
                            foreach (OptionDTO item in list)
                            {
                                ws.Cells[index, 2].Value = (index - 5).ToString();
                                ws.Cells[index, 3].Value = item.OptionName;

                                if (item.DesIcon == Constantes.SI)
                                {
                                    List<LogOptionDTO> listEstadistica = (new SeguridadServicioClient()).ObtenerEstadisticaPorOpcion(item.OptionCode, fechaInicio, fechaFin)
                                        .OrderByDescending(x => x.Fecha).ToList();

                                ws.Cells[index, 4].Value = listEstadistica.Count();
                                    ws.Cells[index, 5].Value = (listEstadistica.Count > 0) ? listEstadistica[0].UserLogin : string.Empty;
                                    ws.Cells[index, 6].Value = (listEstadistica.Count > 0) ? listEstadistica[0].Fecha.ToString("dd/MM/yyyy HH:mm") : string.Empty;

                                }
                                else
                                {
                                    ws.Cells[index, 4].Value = string.Empty;
                                    ws.Cells[index, 5].Value = string.Empty;
                                    ws.Cells[index, 6].Value = string.Empty;
                                }
                                contador++;
                                index++;
                            }
                        


                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

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
        public static void GenerarReporteExcel(List<LogOptionDTO> list, DateTime fechaInicio, DateTime fechaFin,int idOpcion)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteUso;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("USUARIOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE ACCESOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "NRO";
                        ws.Cells[index, 3].Value = "OPCIÓN";
                        ws.Cells[index, 4].Value = "USUARIO";
                        ws.Cells[index, 5].Value = "FECHA";

                        rg = ws.Cells[index, 2, index, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        int contador = 0;
                        foreach (LogOptionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = (index - 5).ToString();
                            ws.Cells[index, 3].Value = item.OptionName;

                                List<LogOptionDTO> listEstadistica = (new SeguridadServicioClient()).ObtenerEstadisticaPorOpcion(idOpcion, fechaInicio, fechaFin)
                                    .OrderByDescending(x => x.Fecha).ToList();

                                ws.Cells[index, 4].Value = (listEstadistica.Count > 0) ? listEstadistica[contador].UserLogin : string.Empty;
                                ws.Cells[index, 5].Value = (listEstadistica.Count > 0) ? listEstadistica[contador].Fecha.ToString("dd/MM/yyyy HH:mm") : string.Empty;

                            contador++;
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
        public void ArmarReporteExcel(List<OptionDTO> list, string file) 
        {
            try
            {               
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("USUARIOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE ACCESOS";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "NRO";
                        ws.Cells[index, 3].Value = "OPCIÓN";
                        ws.Cells[index, 4].Value = "CANTIDAD DE ACCESOS";
                        ws.Cells[index, 5].Value = "ÚLTIMO USUARIO";
                        ws.Cells[index, 6].Value = "ÚLTIMO FECHA";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        DateTime fechaInicio = DateTime.Now.AddDays(-360);
                        DateTime fechaFin = DateTime.Now;


                        index = 6;
                        foreach (OptionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = (index - 5).ToString();
                            ws.Cells[index, 3].Value = item.OptionName;
                            
                            if (item.DesIcon == Constantes.SI)
                            {
                                List<LogOptionDTO> listEstadistica = (new SeguridadServicioClient()).ObtenerEstadisticaPorOpcion(item.OptionCode, fechaInicio, fechaFin)
                                    .OrderByDescending(x=>x.Fecha).ToList();
                                ws.Cells[index, 4].Value = listEstadistica.Count();
                                ws.Cells[index, 5].Value = (listEstadistica.Count > 0) ? listEstadistica[0].UserLogin : string.Empty;
                                ws.Cells[index, 6].Value = (listEstadistica.Count > 0) ? listEstadistica[0].Fecha.ToString("dd/MM/yyyy HH:mm") : string.Empty;

                            }
                            else
                            {
                                ws.Cells[index, 4].Value = string.Empty;
                                ws.Cells[index, 5].Value = string.Empty;
                                ws.Cells[index, 6].Value = string.Empty;
                            }
                                                        
                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

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
    }
}