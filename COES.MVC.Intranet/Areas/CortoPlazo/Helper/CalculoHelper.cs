using COES.Dominio.DTO.Sic;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
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

namespace COES.MVC.Intranet.Areas.CortoPlazo.Helper
{
    public class CalculoHelper
    {
        /// <summary>
        /// Convierte una lista de datos en una Matriz Excel Web
        /// </summary>
        /// <param name="data">datos en una lista</param>
        /// <param name="colHead">Cabecera</param>
        /// <param name="nCol">Número de columnas</param>
        /// <param name="filHead">Fila</param>
        /// <param name="nFil">Número de filas</param>
        /// <returns></returns>
        public static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var colTot = nCol + colHead;
            var inicio = (colTot) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[colTot];

            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == colTot)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[colTot];
                }
                arreglo[fila][col] = valor;
                col++;
            }

            return arreglo;
        }

        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">Matriz de datos</param>
        /// <param name="columna">Número de columnas</param>
        /// <returns></returns>
        public static string[][] PintarCeldas(string[][] data, int columna)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[columna];

                for (int j = 0; j < columna; j++)
                {
                    data[i][j] = string.Empty;
                }
            }
            return data;
        }

        /// <summary>
        /// Permite generar el reporte de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listGeneracion"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void GenerarReporteCM(List<CmCostomarginalDTO> list,List<CmGeneracionEmsDTO> listGeneracion, string path, string filename)
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

                        ws.Cells[index, 2].Value = "HORA";
                        ws.Cells[index, 3].Value = "NODO EMD";
                        ws.Cells[index, 4].Value = "NOMBRE BARRA";
                        ws.Cells[index, 5].Value = "DEMANDA";
                        ws.Cells[index, 6].Value = "ENERGÍA";
                        ws.Cells[index, 7].Value = "CONGESTIÓN";
                        ws.Cells[index, 8].Value = "TOTAL";

                        rg = ws.Cells[index, 2, index, 8];
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
                            ws.Cells[index, 5].Value = item.Cmgndemanda;
                            ws.Cells[index, 6].Value = item.Cmgnenergia;
                            ws.Cells[index, 7].Value = item.Cmgncongestion;
                            ws.Cells[index, 8].Value = item.Cmgntotal;                            

                            rg = ws.Cells[index, 2, index, 8];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 8];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 8];
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

                    ExcelWorksheet wsgen = xlPackage.Workbook.Worksheets.Add("GENERACION-EMS");
                    if (wsgen != null)
                    {
                        wsgen.Cells[2, 3].Value = "GENERACION EMS";

                        ExcelRange rg = wsgen.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        wsgen.Cells[index, 2].Value = "HORA";
                        wsgen.Cells[index, 3].Value = "EMPRESA";
                        wsgen.Cells[index, 4].Value = "NOMBRE EQUIPO";
                        wsgen.Cells[index, 5].Value = "ABREVIATURA EQUIPO";
                        wsgen.Cells[index, 6].Value = "GENERACIÓN";

                        rg = wsgen.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        index = 6;
                        foreach (CmGeneracionEmsDTO item in listGeneracion)
                        {
                            wsgen.Cells[index, 2].Value = item.Genemsfecha.ToString("dd/MM/yyyy HH:mm");
                            wsgen.Cells[index, 3].Value = item.Emprnomb;
                            wsgen.Cells[index, 4].Value = item.Equinomb;
                            wsgen.Cells[index, 5].Value = item.Equiabrev;
                            wsgen.Cells[index, 6].Value = item.Genemsgeneracion;

                            rg = wsgen.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = wsgen.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        wsgen.Column(2).Width = 30;

                        rg = wsgen.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();


                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsgen.Drawings.AddPicture("Logo", img);
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
        public static void GenerarReporteMasivoCM(List<CmCostomarginalDTO> list, List<CmGeneracionEmsDTO> listadoGeneracion, string path, string filename)
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
                        ws.Cells[index, 5].Value = "DEMANDA";
                        ws.Cells[index, 6].Value = "ENERGÍA";
                        ws.Cells[index, 7].Value = "CONGESTIÓN";
                        ws.Cells[index, 8].Value = "TOTAL";
                        ws.Cells[index, 9].Value = "OPERATIVO";
                        ws.Cells[index, 10].Value = "ESCENARIO YUPANA";
                        ws.Cells[index, 11].Value = "CÓDIGO ESCENARIO YUPANA";

                        rg = ws.Cells[index, 2, index, 11];
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
                            ws.Cells[index, 5].Value = item.Cmgndemanda;
                            ws.Cells[index, 6].Value = item.Cmgnenergia;
                            ws.Cells[index, 7].Value = item.Cmgncongestion;
                            ws.Cells[index, 8].Value = item.Cmgntotal;
                            ws.Cells[index, 9].Value = item.Cmgnoperativo;
                            ws.Cells[index, 10].Value = item.Topnombre;
                            ws.Cells[index, 11].Value = item.Topcodi;

                            rg = ws.Cells[index, 2, index, 11];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 11];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 11];
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

                    ExcelWorksheet wsgen = xlPackage.Workbook.Worksheets.Add("GENERACION-EMS");
                    if (wsgen != null)
                    {
                        wsgen.Cells[2, 3].Value = "GENERACION EMS";

                        ExcelRange rg = wsgen.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        wsgen.Cells[index, 2].Value = "HORA";
                        wsgen.Cells[index, 3].Value = "EMPRESA";
                        wsgen.Cells[index, 4].Value = "NOMBRE EQUIPO";
                        wsgen.Cells[index, 5].Value = "ABREVIATURA EQUIPO";
                        wsgen.Cells[index, 6].Value = "GENERACIÓN";

                        rg = wsgen.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        index = 6;
                        foreach (CmGeneracionEmsDTO item in listadoGeneracion)
                        {
                            wsgen.Cells[index, 2].Value = item.Genemsfecha.ToString("dd/MM/yyyy HH:mm");
                            wsgen.Cells[index, 3].Value = item.Emprnomb;
                            wsgen.Cells[index, 4].Value = item.Equinomb;
                            wsgen.Cells[index, 5].Value = item.Equiabrev;
                            wsgen.Cells[index, 6].Value = item.Genemsgeneracion;

                            rg = wsgen.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = wsgen.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        wsgen.Column(2).Width = 30;

                        rg = wsgen.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();


                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsgen.Drawings.AddPicture("Logo", img);
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
        public static void GenerarReporteMasivoCMWeb(List<CmCostomarginalDTO> list, string path, string filename)
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
                        ws.Cells[index, 5].Value = "DEMANDA";
                        ws.Cells[index, 6].Value = "ENERGÍA";
                        ws.Cells[index, 7].Value = "CONGESTIÓN";
                        ws.Cells[index, 8].Value = "TOTAL";
                        ws.Cells[index, 9].Value = "CORRELATIVO";

                        rg = ws.Cells[index, 2, index, 9];
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
                            ws.Cells[index, 5].Value = item.Cmgndemanda;
                            ws.Cells[index, 6].Value = item.Cmgnenergia;
                            ws.Cells[index, 7].Value = item.Cmgncongestion;
                            ws.Cells[index, 8].Value = item.Cmgntotal;
                            ws.Cells[index, 9].Value = item.Cmgncorrelativo;

                            rg = ws.Cells[index, 2, index, 9];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 9];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 9];
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