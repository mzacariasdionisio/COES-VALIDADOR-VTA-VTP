using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.PronosticoDemanda.Helper
{
    class ExcelDocument
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

        public static ExcelRange EstiloCeldaPorTipoInformacion(ExcelRange rango, string tipoInfo)
        {
            rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            rango.Style.Fill.PatternType = ExcelFillStyle.Solid;

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

            switch (tipoInfo)
            {
                case "D":
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                case "S":
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ABB9DB"));
                    break;
                case "P":
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CECECF"));
                    break;
                case "E":
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#B7DFD2"));
                    break;
                case "X":
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                default:
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                    break;
            }
            
            return rango;
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

                    rg = ws.Cells[index, 2, index, formato.Cabecera.Count() + 1 ];
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

                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader1 != null)
                        {
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader1)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader2 != null)
                        {
                            index += 1;
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader2)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //3ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader3 != null)
                        {
                            index += 1;
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader3)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }
                        
                        //Contenido de la tabla
                        //------------------------------------------------------------------------------------------------
                        index += 1;
                        
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

        public static void GenerarExcelConLibrosyEstilosPorCeldas(List<PrnFormatoExcel> ListFormato, string rutaArchivo)
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

                        //Leyenda
                        //------------------------------------------------------------------------------------------------
                        index += 2;

                        rg = ws.Cells[index, 2];
                        rg.Value = "Previsto Diario";
                        rg = EstiloCeldaPorTipoInformacion(rg, "D");

                        rg = ws.Cells[index, 3];
                        rg.Value = "Previsto Semanal";
                        rg = EstiloCeldaPorTipoInformacion(rg, "S");

                        rg = ws.Cells[index, 4];
                        rg.Value = "Patron Defecto";
                        rg = EstiloCeldaPorTipoInformacion(rg, "P");

                        rg = ws.Cells[index, 5];
                        rg.Value = "Ejecutado";
                        rg = EstiloCeldaPorTipoInformacion(rg, "E");
                        
                        //Cabecera de la tabla
                        //------------------------------------------------------------------------------------------------
                        index += 3;

                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader1 != null)
                        {
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader1)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader2 != null)
                        {
                            index += 1;
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader2)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //3ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader3 != null)
                        {
                            index += 1;
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader3)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //4ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader4 != null)
                        {
                            index += 1;
                            int i = 2;//Columna de inicio de la tabla
                            foreach (PrnExcelHeader header in formato.NestedHeader4)
                            {
                                ws.Cells[index, i].Value = header.Etiqueta;
                                ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido.Count() + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 1);
                        }

                        //Contenido de la tabla
                        //------------------------------------------------------------------------------------------------
                        index += 1;

                        for (int i = 0; i < formato.Contenido.Count(); i++)//Controla el llenado horizontal
                        {
                            for (int j = 0; j < formato.Contenido[0].Count(); j++)//Controla el llenado vertical
                            {
                                rg = ws.Cells[index + j, i + 2];
                                rg.Value = formato.Contenido[i][j];
                                rg = EstiloCeldaPorTipoInformacion(rg, formato.ColorByCells[i][j]);                                
                            }

                            //Aplica el ancho de la columna
                            ws.Column(i + 2).Width = formato.AnchoColumnas[i];

                            //Aplica el borde por columna
                            rg = ws.Cells[index, i + 2, index + formato.Contenido[0].Count() - 1, i + 2];
                            rg.Style.WrapText = true;
                            //rg = ObtenerEstiloCelda(rg, 0);
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

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Superávit" CU10.01
        /// </summary>
        public static void GenerarReportePrnMedicionEq(string fileName, DateTime fechaConsulta, List<PrnMedicioneqDTO> listaMedicion, List<PrnMedicioneqDTO> listaRpf, List<PrnMedicioneqDTO> listaDespacho)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                #region PRIMERA PESTAÑA - RESUMEN DE TODOS LOS CONCEPTOS
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DESPACHOEJECUTADO");
                if (ws != null)
                {
                    int index = 2;
                    int columna = 2;
                    //TITULO
                    ws.Cells[index, 3].Value = "DESPACHO EJECUTADO - " + fechaConsulta.ToString("yyyy/MM/dd");
                    //ws.Cells[index + 1, 3].Value = 'Otra fila';
                    ExcelRange rg = ws.Cells[index, 3, index + 1, 3];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Inicio de Fila donde se muestra todo
                    index += 3;
                    //-------------------------------------------------------------------------------------------------------------------------------------------
                    //Primeras dos columnas:
                    //Cabecera:
                    ws.Cells[index, columna].Value = "ÁREA"; //Area operativa
                    ws.Cells[index, columna, index + 1, columna].Merge = true;
                    ws.Column(columna).Width = 12;//Ancho
                    columna++;
                    ws.Cells[index, columna].Value = "CENTRAL"; //Central
                    ws.Cells[index, columna, index + 1, columna].Merge = true;
                    ws.Column(columna).Width = 40;//Ancho
                    columna++;
                    ws.Cells[index, columna].Value = "DIFER. MW"; //Suma absoluta por cada intervalo de las diferencias RSF vs Despacho Ejecutado
                    ws.Cells[index, columna, index + 1, columna].Merge = true;
                    ws.Column(columna).Width = 10;//Ancho
                    ws.Column(columna).Style.Numberformat.Format = "#,##0.00";
                    columna++;
                    ws.Cells[index, columna].Value = "#INTERV."; //Numero de Intervalos que presentan diferencias RSF vs Despacho Ejecutado
                    ws.Cells[index, columna, index + 1, columna].Merge = true;
                    ws.Column(columna).Width = 10;//Ancho
                    DateTime dFecha = fechaConsulta;
                    columna++;
                    for (int i = 1; i <= 48; i++)
                    {
                        dFecha = dFecha.AddMinutes(30);
                        ws.Cells[index, columna].Value = dFecha.ToString("HH:mm"); //"H" + i.ToString(); //Hi
                        ws.Cells[index + 1, columna].Value = "RPF"; //Hi
                        ws.Column(columna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(columna++).Width = 8;//Ancho
                        ws.Cells[index + 1, columna].Value = "DSPC"; //Hi
                        ws.Column(columna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(columna++).Width = 8;//Ancho
                        ws.Cells[index + 1, columna].Value = "PRNT"; //Hi
                        ws.Column(columna).Style.Numberformat.Format = "#,##0.00";
                        ws.Column(columna++).Width = 8;//Ancho
                        ws.Cells[index, columna - 3, index, columna - 1].Merge = true;
                    }
                    rg = ws.Cells[index, 2, index + 1, columna - 1];
                    //rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //while (dFecha <= dFecFin)
                    //{
                    //    ws.Cells[iFila++, 2].Value = dFecha.ToString("dd/MM/yyyy");
                    //    dFecha = dFecha.AddDays(1);
                    //}

                    //Filas:
                    int iFila = index + 2;
                    int iColumna = 2;
                    int iPosElem = 0;
                    foreach (PrnMedicioneqDTO dtoMedicion in listaMedicion)
                    {
                        columna = iColumna;
                        int iPrnmeqdepurar = dtoMedicion.Prnmeqdepurar;
                        if (dtoMedicion.Prnmeqdepurar < 0)
                        {
                            iPrnmeqdepurar *= -1;
                        }
                        ws.Cells[iFila, columna++].Value = dtoMedicion.Areanomb.ToString().Trim();//Area operativa
                        ws.Cells[iFila, columna++].Value = dtoMedicion.Equinomb.ToString().Trim();//Central
                        ws.Cells[iFila, columna++].Value = dtoMedicion.Prnmeqdejevsrpf;//DIFER. MW
                        ws.Cells[iFila, columna++].Value = iPrnmeqdepurar;//#INTERV.
                        for (int i = 1; i <= 48; i++)
                        {
                            //Hi
                            ws.Cells[iFila, columna++].Value = Convert.ToDecimal(listaRpf[iPosElem].GetType().GetProperty("H" + i).GetValue(listaRpf[iPosElem], null)); //RPF
                            ws.Cells[iFila, columna++].Value = Convert.ToDecimal(listaDespacho[iPosElem].GetType().GetProperty("H" + i).GetValue(listaDespacho[iPosElem], null)); //DSPC
                            ws.Cells[iFila, columna++].Value = Convert.ToDecimal(dtoMedicion.GetType().GetProperty("H" + i).GetValue(dtoMedicion, null));//PRNT
                        }
                        iFila++;
                        iPosElem++;
                    }
                    rg = ws.Cells[index + 2, 2, iFila - 1, columna - 1];
                    rg = ObtenerEstiloCelda(rg, 0);
                    rg = ws.Cells[index + 2, 4, iFila - 1, columna - 1];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }
                #endregion
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Devuelve la información leida de un archivo excel
        /// </summary>
        /// <returns></returns>
        public static List<string[]> LeerArchivoExcel(string fileName)
        {
            FileInfo newFile = new FileInfo(fileName);
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

            }

            return new List<string[]>();
        }
    }
    
}
