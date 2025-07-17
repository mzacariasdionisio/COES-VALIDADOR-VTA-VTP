using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
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
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.DemandaMaxima.Helper
{
    public class ExcelDocument
    {
        public static void GenerarFormatoExcelValidacion(string fileName, List<String> list)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Lista de Observaciones");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "OBSERVACIONES ENCONTRADAS";

                    ExcelRange rg = ws.Cells[index, 1, index, 1];
                    rg = ObtenerEstiloCeldaValidacion(rg, 1);

                    index++;
                    foreach (String item in list)
                    {
                        ws.Cells[index, 1].Value = item;
                        rg = ws.Cells[index, 1, index, 1];
                        rg = ObtenerEstiloCeldaValidacion(rg, 0);
                        index++;
                    }

                    ws.Column(1).Width = 120;

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesDemandaMaxima.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesDemandaMaxima.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static ExcelRange ObtenerEstiloCeldaValidacion(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;

                string colorborder = "#245C86";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }


        public static void GenerarFormatoExcelRepCumplimiento(string fileName, System.Data.IDataReader list, DateTime fechaInicio, int meses, string abreviatura)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte de Cumplimiento");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Ítem";
                    //ws.Cells[index, 2].Value = "Periodo";
                    //ws.Cells[index, 3].Value = "Cumplimiento";
                    ws.Cells[index, 2].Value = "Razón Social";
                    ws.Cells[index, 3].Value = "Código Cliente ";
                    ws.Cells[index, 4].Value = "RUC de la Empresa";                    
                    ws.Cells[index, 5].Value = "Fecha Ingreso";
                    //ws.Cells[index, 7].Value = "Fecha del primer envío";
                    //ws.Cells[index, 8].Value = "Fecha del último envío";
                    for (int i = 0; i <= meses; i++)
                    {
                        ws.Cells[index, 6 + i].Value = fechaInicio.AddMonths(i).ToString("MM yyyy");
                    }
                    ExcelRange rg = ws.Cells[index, 1, index, 6 + meses];
                    rg = ObtenerEstiloCeldaRepCumplimiento(rg, 1);

                    index++;
                    var item = 1;
                    using  (var dr = list)
                    {
                        while (dr.Read())
                        {
                            //Values
                            ws.Cells[index, 1].Value = item;
                            ws.Cells[index, 2].Value = dr["NOMBRE_EMPRESA"] != null ? dr["NOMBRE_EMPRESA"].ToString() : "";
                            ws.Cells[index, 3].Value = dr["EQUIABREV"]?.ToString() ?? "";
                            ws.Cells[index, 4].Value = dr["RUC_EMPRESA"] != null ? dr["RUC_EMPRESA"].ToString() : "";
                            ws.Cells[index, 5].Value = dr["EMPRFECINGRESO"] != null && !string.IsNullOrEmpty(dr["EMPRFECINGRESO"].ToString()) ?
                                                            Convert.ToDateTime(dr["EMPRFECINGRESO"]).ToString("dd/MM/yyyy") : "";

                            for (int i = 0; i <= meses; i++)
                            {
                                var cumplimiento = dr[fechaInicio.AddMonths(i).ToString("MMyyyy")].ToString();

                                if (cumplimiento == "5")
                                {
                                    ws.Cells[index, 6 + i].Value = "No determinado";
                                    ws.Cells[index, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid; 
                                    ws.Cells[index, 6 + i].Style.Fill.BackgroundColor.SetColor(colVerde);
                                }
                                else if (cumplimiento == "4")
                                {
                                    ws.Cells[index, 6 + i].Value = "No Cumplió";
                                    ws.Cells[index, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid; 
                                    ws.Cells[index, 6 + i].Style.Fill.BackgroundColor.SetColor(colRojo);
                                }
                                else if (cumplimiento == "3")
                                {
                                    ws.Cells[index, 6 + i].Value = "Inconsistente";
                                    ws.Cells[index, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid; 
                                    ws.Cells[index, 6 + i].Style.Fill.BackgroundColor.SetColor(colVerde);
                                }
                                else if (cumplimiento == "2")
                                {
                                    ws.Cells[index, 6 + i].Value = "Fuera del Plazo";
                                    ws.Cells[index, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid; 
                                    ws.Cells[index, 6 + i].Style.Fill.BackgroundColor.SetColor(colAmarillo);
                                }
                                else
                                {
                                    ws.Cells[index, 6 + i].Value = "Dentro del Plazo";
                                    ws.Cells[index, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid; 
                                    ws.Cells[index, 6 + i].Style.Fill.BackgroundColor.SetColor(colVerde);
                                }
                            }
                            index++;
                            item++;                            
                        }
                        

                        //Styles for PR16
                        //ws.Cells[index, 7].Style.Numberformat.Format = "dd/mm/yyyy h:mm";
                        //ws.Cells[index, 8].Style.Numberformat.Format = "dd/mm/yyyy h:mm";
                        //rg = ws.Cells[index, 1, index, 8];
                        //rg = ObtenerEstiloCeldaRepCumplimiento(rg, 0);
                        //ws.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;      
                        
                    }

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;

                    for (int i = 0; i <= meses; i++)
                    {
                        ws.Column(6 + i).Width = 20;
                    }

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesDemandaMaxima.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesDemandaMaxima.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static ExcelRange ObtenerEstiloCeldaRepCumplimiento(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;

                string colorborder = "#245C86";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        public static void GenerarFormatoExcel15min(string fileName, List<MeMedicion96DTO> list, string tipo)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte de Cumplimiento");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                Color colHP = System.Drawing.ColorTranslator.FromHtml("#FAD0D0");
                Color colHFP = System.Drawing.ColorTranslator.FromHtml("#BBADF5");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Ítem";
                    ws.Cells[index, 2].Value = "Periodo";
                    ws.Cells[index, 3].Value = "Fuente";
                    ws.Cells[index, 4].Value = "Cumplimiento";
                    ws.Cells[index, 5].Value = "Código Cliente";
                    ws.Cells[index, 6].Value = "Suministrador";
                    ws.Cells[index, 7].Value = "RUC de la Empresa";
                    ws.Cells[index, 8].Value = "Razón Social";
                    ws.Cells[index, 9].Value = "Subestación";
                    ws.Cells[index, 10].Value = "Tensión (KV)";
                    ws.Cells[index, 11].Value = "Fecha";

                    int v = 11;
                    for (int n = 1; n <= 96; n++)
                    {
                        v++;
                        string timeString = TimeSpan.FromMinutes(n * 15).ToString("hh':'mm");
                        ws.Cells[index, v].Value = timeString;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, 107];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (MeMedicion96DTO item in list)
                    {
                        //Values
                        ws.Cells[index, 1].Value = item.Item;
                        ws.Cells[index, 2].Value = item.Periodo;
                        ws.Cells[index, 3].Value = item.Fuente;
                        if (item.Cumplimiento == "5")
                        {
                            ws.Cells[index, 4].Value = "No determinado";
                        }
                        else if (item.Cumplimiento == "4")
                        {
                            ws.Cells[index, 4].Value = "No Cumplió";
                        }
                        else if (item.Cumplimiento == "3")
                        {
                            ws.Cells[index, 4].Value = "Inconsistente";
                        }
                        else if (item.Cumplimiento == "2")
                        {
                            ws.Cells[index, 4].Value = "Fuera del Plazo";
                        }
                        else
                        {
                            ws.Cells[index, 4].Value = "Dentro del Plazo";
                        }
                        ws.Cells[index, 5].Value = item.CodigoCliente;
                        ws.Cells[index, 6].Value = item.Suministrador;
                        ws.Cells[index, 7].Value = item.RucEmpresa;
                        ws.Cells[index, 8].Value = item.NombreEmpresa;
                        ws.Cells[index, 9].Value = item.Subestacion;
                        ws.Cells[index, 10].Value = item.Tension;
                        ws.Cells[index, 11].Value = item.FechaFila;

                        int n = 1;
                        for (int i = 12; i < 108; i++)//Mediciones del 1 - 96
                        {
                            ws.Cells[index, i].Value = item.GetType().GetProperty("H" + n).GetValue(item, null);
                            n++;
                        }

                        //Styles for PR16
                        ws.Cells[index, 11].Style.Numberformat.Format = ConstantesDemandaMaxima.FormatoFecha;
                        rg = ws.Cells[index, 1, index, 107];
                        rg = ObtenerEstiloCelda(rg, 0);

                        ws.Cells[index, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        if (item.Cumplimiento == "4")
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colRojo);
                        }
                        else if (item.Cumplimiento == "2")
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colAmarillo);
                        }
                        else//Si cumplio (1)
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colVerde);
                        }

                        if (item.HP != null)
                        {
                            string valHP = item.HP.Trim(new Char[] { 'H' });
                            int hp = Int32.Parse(valHP) + 11;
                            ws.Cells[index, hp].Style.Fill.BackgroundColor.SetColor(colHP);
                        }

                        if (item.HFP != null)
                        {
                            string valHFP = item.HFP.Trim(new Char[] { 'H' });
                            int hfp = Int32.Parse(valHFP) + 11;
                            ws.Cells[index, hfp].Style.Fill.BackgroundColor.SetColor(colHFP);
                        }
                        index++;
                    }

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 10;
                    ws.Column(3).Width = 10;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 16;
                    ws.Column(7).Width = 16;
                    ws.Column(8).Width = 25;
                    ws.Column(9).Width = 25;
                    ws.Column(10).Width = 25;
                    ws.Column(11).Width = 16;
                    ws.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int i = 12; i <= 107; i++)//Medicion 1 - 96
                    {
                        ws.Column(i).Width = 10;
                        ws.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    if (tipo == "2")//Si el Tipo de Empresa eds Distribucción No se muestran las siguientes columnas
                    {
                        ws.DeleteColumn(10);
                        ws.DeleteColumn(9);
                        ws.DeleteColumn(7);
                        ws.DeleteColumn(6);
                        ws.DeleteColumn(5);
                    }
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesDemandaMaxima.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesDemandaMaxima.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static void GenerarFormatoExcel30min(string fileName, List<MeMedicion48DTO> list, string tipo)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte de Cumplimiento");

                //Colores
                Color colRojo = System.Drawing.ColorTranslator.FromHtml("#F9C3C3");
                Color colAmarillo = System.Drawing.ColorTranslator.FromHtml("#FFDF75");
                Color colVerde = System.Drawing.ColorTranslator.FromHtml("#97F9B0");

                if (ws != null)
                {
                    int index = 6;

                    ws.Cells[index, 1].Value = "Ítem";
                    ws.Cells[index, 2].Value = "Periodo";
                    ws.Cells[index, 3].Value = "Fuente";
                    ws.Cells[index, 4].Value = "Cumplimiento";
                    ws.Cells[index, 5].Value = "Código Cliente";
                    ws.Cells[index, 6].Value = "Suministrador";
                    ws.Cells[index, 7].Value = "RUC de la Empresa";
                    ws.Cells[index, 8].Value = "Razón Social";
                    ws.Cells[index, 9].Value = "Subestación";
                    ws.Cells[index, 10].Value = "Tensión (KV)";
                    ws.Cells[index, 11].Value = "Fecha";

                    int v = 11;
                    for (int n = 1; n <= 48; n++)
                    {
                        v++;
                        string fromTimeString = TimeSpan.FromMinutes(n * 30).ToString("hh':'mm");
                        ws.Cells[index, v].Value = fromTimeString;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, 59];
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (MeMedicion48DTO item in list)
                    {
                        //Values
                        ws.Cells[index, 1].Value = item.Item;
                        ws.Cells[index, 2].Value = item.Periodo;
                        ws.Cells[index, 3].Value = item.Fuente;
                        if (item.Cumplimiento == "5")
                        {
                            ws.Cells[index, 4].Value = "No determinado";
                        }
                        else if (item.Cumplimiento == "4")
                        {
                            ws.Cells[index, 4].Value = "No Cumplió";
                        }
                        else if (item.Cumplimiento == "3")
                        {
                            ws.Cells[index, 4].Value = "Inconsistente";
                        }
                        else if (item.Cumplimiento == "2")
                        {
                            ws.Cells[index, 4].Value = "Fuera del Plazo";
                        }
                        else
                        {
                            ws.Cells[index, 4].Value = "Dentro del Plazo";
                        }
                        ws.Cells[index, 5].Value = item.CodigoCliente;
                        ws.Cells[index, 6].Value = item.Suministrador;
                        ws.Cells[index, 7].Value = item.RucEmpresa;
                        ws.Cells[index, 8].Value = item.NombreEmpresa;
                        ws.Cells[index, 9].Value = item.Subestacion;
                        ws.Cells[index, 10].Value = item.Tension;
                        ws.Cells[index, 11].Value = item.FechaFila;

                        int n = 1;
                        for (int i = 12; i < 60; i++)//Mediciones del 1 - 48
                        {
                            ws.Cells[index, i].Value = item.GetType().GetProperty("H" + n).GetValue(item, null);
                            n++;
                        }

                        //Styles for PR16
                        ws.Cells[index, 11].Style.Numberformat.Format = ConstantesDemandaMaxima.FormatoFecha;
                        rg = ws.Cells[index, 1, index, 59];
                        rg = ObtenerEstiloCelda(rg, 0);

                        ws.Cells[index, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        if (item.Cumplimiento == "4")
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colRojo);
                        }
                        else if (item.Cumplimiento == "2")
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colAmarillo);
                        }
                        else//Si cumplio con la remisión en el tiempo indicado. (1)
                        {
                            ws.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(colVerde);
                        }
                        index++;
                    }

                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 10;
                    ws.Column(3).Width = 10;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 16;
                    ws.Column(7).Width = 16;
                    ws.Column(8).Width = 25;
                    ws.Column(9).Width = 25;
                    ws.Column(10).Width = 25;
                    ws.Column(11).Width = 16;
                    ws.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int i = 12; i <= 59; i++)//Medicion 1 - 48
                    {
                        ws.Column(i).Width = 10;
                        ws.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    if (tipo == "2")
                    {
                        ws.DeleteColumn(10);
                        ws.DeleteColumn(9);
                        ws.DeleteColumn(7);
                        ws.DeleteColumn(6);
                        ws.DeleteColumn(5);
                    }
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesDemandaMaxima.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesDemandaMaxima.NombreLogo, img);
                    picture.SetPosition(20, 250);
                    picture.SetSize(180, 60);
                }

                xlPackage.Save();
            }
        }

        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#245C86";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

    }
}
