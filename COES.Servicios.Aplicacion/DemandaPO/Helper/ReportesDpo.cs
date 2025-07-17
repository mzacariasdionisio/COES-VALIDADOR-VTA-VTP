using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;

namespace COES.Servicios.Aplicacion.DPODemanda.Helper
{
    class ReportesDpo
    {
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

        public static void GenerarArchivoExcelConLibros(List<PrnFormatoExcel> ListFormato, string rutaArchivo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);

            //Valida la existencia de un archivo con el mismo nombre
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                foreach (PrnFormatoExcel formato in ListFormato)
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(formato.NombreLibro);
                    if (ws != null)
                    {
                        int index = 2;
                        //Titulo de la tabla
                        //------------------------------------------------------------------------------------------------
                        ws.Cells[index, 3].Value = formato.Titulo;
                        ws.Cells[index + 1, 3].Value = formato.Subtitulo1;
                        ExcelRange rg = ws.Cells[index, 3, index + 1, formato.Contenido[0].Count()];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //Cabecera de la tabla
                        //------------------------------------------------------------------------------------------------
                        index += 3;
                        for (int i = 0; i < formato.Cabecera.Count(); i++)
                        {
                            ws.Cells[index, i + 2].Value = formato.Cabecera[i];
                            ws.Cells[index, i + 2, index + 1, i + 2].Merge = true;
                        }

                        rg = ws.Cells[index, 2, index, formato.Cabecera.Count() + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        rg = ws.Cells[index + 1, 2, index + 1, formato.Cabecera.Count() + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 1);

                        //Contenido de la tabla
                        //------------------------------------------------------------------------------------------------
                        index += 2;

                        for (int i = 0; i < formato.Contenido.Count(); i++)//Controla el llenado horizontal
                        {
                            for (int j = 0; j < formato.Contenido[0].Count(); j++)//Controla el llenado vertical
                            {
                                try
                                {
                                    ws.Cells[index + j, i + 2].Value = formato.Contenido[i][j];
                                }
                                catch
                                {
                                    ws.Cells[index + j, i + 2].Value = 0;
                                }
                            }

                            //Aplica el ancho de la columna
                            ws.Column(i + 2).Width = formato.AnchoColumnas[i];

                            //Aplica el borde por columna
                            rg = ws.Cells[index, i + 2, index + formato.Contenido[0].Count() - 1, i + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 0);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(20, 60);
                        picture.SetSize(135, 45);
                    }
                }

                xlPackage.Save();
            }
        }

        public static void GenerarArchivoExcel(PrnFormatoExcel formato, string rutaArchivo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                if (ws != null)
                {
                    int index = 2;
                    //Titulo de la tabla
                    //------------------------------------------------------------------------------------------------
                    ws.Cells[index, 3].Value = formato.Titulo;
                    ws.Cells[index + 1, 3].Value = formato.Subtitulo1;
                    ExcelRange rg = ws.Cells[index, 3, index + 1, formato.Cabecera.Count()];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Cabecera de la tabla
                    //------------------------------------------------------------------------------------------------
                    index += 3;
                    for (int i = 0; i < formato.Cabecera.Count(); i++)
                    {
                        ws.Cells[index, i + 2].Value = formato.Cabecera[i];
                        ws.Cells[index, i + 2, index + 1, i + 2].Merge = true;
                    }

                    rg = ws.Cells[index, 2, index, formato.Cabecera.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index + 1, 2, index + 1, formato.Cabecera.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Contenido de la tabla
                    //------------------------------------------------------------------------------------------------
                    index += 2;
                    for (int i = 0; i < formato.Contenido.Count(); i++)//Controla el llenado vertical
                    {
                        for (int j = 0; j < formato.Contenido[0].Count(); j++)//Controla el llenado horizontal
                        {
                            ws.Cells[index + i, j + 2].Value = formato.Contenido[i][j];
                            //Aplica el ancho de la columna
                            ws.Column(j + 2).Width = formato.AnchoColumnas[j];
                        }

                        //Aplica el borde por fila
                        rg = ws.Cells[index + i, 2, index + i, formato.Contenido[0].Count() + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(20, 60);
                    picture.SetSize(135, 45);
                }
                xlPackage.Save();
            }
        }

        public static void DemVegReporteSimple(
            List<DemVegCol> datos, List<string> intervalos,
            string nombreArchivo, string rutaArchivo, string tipoCabecera)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreArchivo);
                if (ws != null)
                {
                    //Cabecera de la tabla                
                    int indiceVertical = 1;

                    
                    List<string> cabecera = new List<string>();

                    cabecera = datos.Select(x => x.GetType()
                        .GetProperty(tipoCabecera)
                        .GetValue(x).ToString()).ToList();

                    cabecera.Insert(0, "");

                    for (int i = 0; i < cabecera.Count; i++)
                    {
                        ws.Cells[indiceVertical, i + 1].Value = cabecera[i];
                    }
                    //Contenido de la tabla
                    indiceVertical += 1;

                    //. Llenado de intervalos
                    for (int i = 0; i < intervalos.Count; i++)
                    {
                        ws.Cells[indiceVertical + i, 1].Value = intervalos[i];
                    }

                    if (datos.Count != 0)
                    {
                        //. Llenado de datos
                        int columnas = datos.Count;
                        int filas = datos[0].Valores.Count;

                        for (int i = 0; i < columnas; i++)
                        {
                            for (int j = 0; j < filas; j++)
                            {
                                ws.Cells[indiceVertical + j, i + 2].Value = datos[i].Valores[j];
                            }
                        }
                    }
                }
                xlPackage.Save();
            }
        }
    }
}
