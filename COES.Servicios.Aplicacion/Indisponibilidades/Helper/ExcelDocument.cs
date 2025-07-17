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

namespace COES.Servicios.Aplicacion.Indisponibilidades.Helper
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

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C0B99D"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = false;

                string colorborder = "#000000";

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

            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = false;

                string colorborder = "#000000";

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

            if (seccion == 4)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
            return rango;
        }
        public static void GenerarCuadroA1A2Excel(IndFormatoExcel formato, string rutaArchivo)
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
                if (formato.HabilitadoCuadro1)
                {
                    #region Cuadro A1
                    ExcelWorksheet wsCuadro1 = xlPackage.Workbook.Worksheets.Add(formato.NombreLibroCuadroA1);
                    if (wsCuadro1 != null)
                    {
                        int index = 2;
                        //Titulo de la tabla
                        //------------------------------------------------------------------------------------------------
                        wsCuadro1.Cells[index, 6].Value = formato.TituloCuadroA1;
                        ExcelRange rg = wsCuadro1.Cells[index, 5, index + 1, 7];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        for (int i = 7; i < 38; i++)
                        {
                            wsCuadro1.Column(i).Style.Numberformat.Format = "#,##0.####################";
                        }

                        wsCuadro1.Cells["H17"].Style.Numberformat.Format = "dd/mm/yyyy";
                        wsCuadro1.Cells["I17"].Style.Numberformat.Format = "dd/mm/yyyy";

                        #region Datos generales
                        //Datos generales
                        //----------------------------------------------------------------------------------------
                        index += 4;
                        for (int i = 0; i < formato.DatosGenerales.Count(); i++)//Controla el llenado vertical
                        {
                            for (int j = 0; j < formato.DatosGenerales[0].Count(); j++)//Controla el llenado horizontal
                            {
                                wsCuadro1.Cells[index + i, j + 4].Value = formato.DatosGenerales[i][j];
                                //Aplica el ancho de la columna
                                //wsCuadro1.Column(j + 2).Width = 50;
                            }

                            //Aplica el borde por fila
                            rg = wsCuadro1.Cells[index + i, 4, index + i, formato.DatosGenerales[0].Count() + 3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //Aplica el borde por fila
                            rg = wsCuadro1.Cells[index + i, formato.DatosGenerales[0].Count() + 3, index + i, formato.DatosGenerales[0].Count() + 3];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 3);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        #endregion

                        #region Cuadro CDU
                        //Cabeceras
                        //------------------------------------------------------------------------------------------------
                        index += 9;

                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader1 != null)
                        {
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader1)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, 9];//el 4 debe ser reemplazado por la longitud del contenido
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }
                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader2 != null)
                        {
                            index += 1;
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader2)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, 9];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }

                        //Contenido de la tabla CDU
                        index += 1;
                        for (int i = 0; i < formato.Transportada.Count(); i++)//Controla el llenado horizontal
                        {
                            for (int j = 0; j < formato.Transportada[0].Count(); j++)//Controla el llenado vertical
                            {
                                try
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = formato.Transportada[i][j];
                                }
                                catch
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = 0;
                                }
                            }

                            //Aplica el borde por columna
                            rg = wsCuadro1.Cells[index, i + 2, index + formato.Transportada[0].Count() - 1, i + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 3);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                        #endregion

                        #region Cuadro CRD
                        //Cabeceras
                        //------------------------------------------------------------------------------------------------
                        index += 5 + formato.NumeroRegistros;

                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader3 != null)
                        {
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader3)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, formato.Reservada.Count + 1];//el 4 debe ser reemplazado por la longitud del contenido
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }
                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader4 != null)
                        {
                            index += 1;
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader4)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, formato.Reservada.Count + 1];//el 4 debe ser reemplazado por la longitud del contenido
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }
                        //3ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader5 != null)
                        {
                            index += 1;
                            int i = 6;//Columna de inicio de la tabla
                            int f = 0;//flag para hacer merge con la celda superior
                            foreach (IndExcelHeader header in formato.NestedHeader5)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                if (f == 0)
                                {
                                    wsCuadro1.Cells[index - 1, i, index, i].Merge = true;
                                }
                                i += header.Columnas;
                                f += 1;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, formato.Reservada.Count + 1];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }

                        //Contenido de la tabla CRD
                        index += 1;
                        for (int i = 0; i < formato.Reservada.Count(); i++)//Controla el llenado horizontal
                        {
                            for (int j = 0; j < formato.Reservada[0].Count(); j++)//Controla el llenado vertical
                            {
                                try
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = formato.Reservada[i][j];
                                }
                                catch
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = 0;
                                }
                            }

                            //Aplica el borde por columna
                            rg = wsCuadro1.Cells[index, i + 2, index + formato.Reservada[0].Count() - 1, i + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 3);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //Aplica el borde por columna
                            rg = wsCuadro1.Cells[index + 3, i + 2, index + 3, i + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                        #endregion

                        #region Cuadro CCD
                        //Cabecera de la tabla CCD
                        //------------------------------------------------------------------------------------------------
                        index += 8 + formato.NumeroRegistros;

                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader6 != null)
                        {
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader6)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, 9];//el 4 debe ser reemplazado por la longitud del contenido
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }
                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader7 != null)
                        {
                            index += 1;
                            int i = 6;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader7)
                            {
                                wsCuadro1.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro1.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro1.Cells[index, 6, index, 9];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }

                        //Contenido de la tabla CDU
                        index += 1;

                        wsCuadro1.Cells["H" + index].Style.Numberformat.Format = "dd/mm/yyyy";
                        wsCuadro1.Cells["I" + index].Style.Numberformat.Format = "dd/mm/yyyy";

                        for (int i = 0; i < formato.Contratada.Count(); i++)//Controla el llenado horizontal
                        {
                            for (int j = 0; j < formato.Contratada[0].Count(); j++)//Controla el llenado vertical
                            {
                                try
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = formato.Contratada[i][j];
                                }
                                catch
                                {
                                    wsCuadro1.Cells[index + j, i + 2].Value = 0;
                                }
                            }

                            //Aplica el borde por columna
                            rg = wsCuadro1.Cells[index, i + 2, index + formato.Contratada[0].Count() - 1, i + 2];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 3);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        for (int i = 3; i < formato.Reservada.Count() + 2; i++)
                        {
                            wsCuadro1.Column(i).Width = 30;
                        }
                        wsCuadro1.Column(6).Width = 110;
                        #endregion

                        #region Datos ocultos para validar el doc. excel
                        wsCuadro1.Column(2).Hidden = true;
                        wsCuadro1.Column(3).Hidden = true;
                        wsCuadro1.Column(4).Hidden = true;
                        wsCuadro1.Column(5).Hidden = true;
                        wsCuadro1.Row(6).Hidden = true;
                        wsCuadro1.Row(7).Hidden = true;
                        wsCuadro1.Row(8).Hidden = true;
                        wsCuadro1.Row(9).Hidden = true;
                        wsCuadro1.Row(10).Hidden = true;
                        wsCuadro1.Row(11).Hidden = true;
                        wsCuadro1.Row(12).Hidden = true;
                        #endregion
                    }
                    #endregion
                }

                if (formato.HabilitadoCuadro2)
                {
                    #region Cuadro A2
                    ExcelWorksheet wsCuadro2 = xlPackage.Workbook.Worksheets.Add(formato.NombreLibroCuadroA2);
                    if (wsCuadro2 != null)
                    {
                        int index = 2;
                        //Titulo de la tabla
                        //------------------------------------------------------------------------------------------------
                        wsCuadro2.Cells[index, 3].Value = formato.TituloCuadroA2;
                        //wsCuadro1.Cells[index + 1, 3].Value = formato.Subtitulo1;
                        ExcelRange rg = wsCuadro2.Cells[index, 3, index + 1, 5];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        wsCuadro2.Column(4).Style.Numberformat.Format = "#,##0.####################";
                        for (int i = 9; i < 100; i++)
                        {
                            wsCuadro2.Cells["D" + i].Style.Numberformat.Format = "dd/mm/yyyy";
                        }
                        #region Cabeceras
                        index += 4;
                        //1ra fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader8 != null)
                        {
                            int i = 3;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader8)
                            {
                                wsCuadro2.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro2.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro2.Cells[index, 3, index, 8];//el 4 debe ser reemplazado por la longitud del contenido
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }
                        //2da fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader9 != null)
                        {
                            index += 1;
                            int i = 3;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader9)
                            {
                                wsCuadro2.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro2.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro2.Cells[index, 3, index, 8];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }

                        //3era fila anidada
                        //------------------------------------------------------------------------------------------------
                        if (formato.NestedHeader10 != null)
                        {
                            index += 1;
                            int i = 3;//Columna de inicio de la tabla
                            foreach (IndExcelHeader header in formato.NestedHeader10)
                            {
                                wsCuadro2.Cells[index, i].Value = header.Etiqueta;
                                wsCuadro2.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = wsCuadro2.Cells[index, 3, index, 8];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 2);
                        }

                        wsCuadro2.Cells[7, 5, 8, 5].Merge = true;
                        wsCuadro2.Cells[7, 6, 8, 6].Merge = true;
                        wsCuadro2.Cells[7, 7, 8, 7].Merge = true;
                        wsCuadro2.Cells[7, 8, 8, 8].Merge = true;
                        #endregion

                        #region Contenido
                        if (formato.Cuadro2CTG.Count() > 0)
                        {
                            index += 1;
                            for (int i = 0; i < formato.Cuadro2CTG.Count(); i++)//Controla el llenado horizontal
                            {
                                for (int j = 0; j < formato.Cuadro2CTG[0].Count(); j++)//Controla el llenado vertical
                                {
                                    try
                                    {
                                        wsCuadro2.Cells[index + j, i + 3].Value = formato.Cuadro2CTG[i][j];
                                    }
                                    catch
                                    {
                                        wsCuadro2.Cells[index + j, i + 3].Value = 0;
                                    }
                                }

                                //Aplica el borde por columna
                                rg = wsCuadro2.Cells[index, i + 3, index + formato.Cuadro2CTG[0].Count() - 1, i + 3];
                                rg.Style.WrapText = true;
                                rg = ObtenerEstiloCelda(rg, 3);
                                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }
                        #endregion
                        for (int i = 3; i < 9; i++)
                        {
                            //Aplica el borde por columna
                            rg = wsCuadro2.Cells[9, i, 28, i];
                            rg.Style.WrapText = true;
                            rg = ObtenerEstiloCelda(rg, 3);
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            wsCuadro2.Column(i).Width = 30;
                        }
                        wsCuadro2.Row(7).Height = 30;
                        //for (int i = 9; i < 100; i++)
                        //{
                        //    wsCuadro2.Cells["D" + i].Style.Numberformat.Format = "dd/mm/yyyy";
                        //}
                        //wsCuadro2.Column(4).Style.Numberformat.Format = "0";
                        //wsCuadro2.Column(4).Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    #endregion
                }

                xlPackage.Save();
            }
        }

        public static void GenerarArchivoCumplimientoExcel(IndFormatoExcel formato, string rutaArchivo)
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
                    ExcelRange rg = ws.Cells[index, 3, index + 1, formato.Cabecera2.Count()];
                    rg.Style.Font.Size = 16;
                    rg.Style.Font.Bold = true;

                    //Cabecera de la tabla1
                    //------------------------------------------------------------------------------------------------
                    index += 3;
                    for (int i = 0; i < formato.Cabecera1.Count(); i++)
                    {
                        ws.Cells[index, i + 2].Value = formato.Cabecera1[i];
                        ws.Cells[index, i + 2, index + 1, i + 2].Merge = true;
                    }

                    rg = ws.Cells[index, 2, index, formato.Cabecera1.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index + 1, 2, index + 1, formato.Cabecera1.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Contenido de la tabla1
                    //------------------------------------------------------------------------------------------------
                    index += 2;
                    for (int i = 0; i < formato.Contenido1.Count(); i++)//Controla el llenado vertical
                    {
                        ws.Cells[index, i + 2].Value = formato.Contenido1[i];
                        //Aplica el ancho de la columna
                        ws.Column(i + 2).Width = formato.AnchoColumnas1[i];

                        //Aplica el borde por fila
                        rg = ws.Cells[index, 2, index, formato.Contenido1.Count() + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 4);
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Cabecera de la tabla2
                    //------------------------------------------------------------------------------------------------
                    index += 3;
                    for (int i = 0; i < formato.Cabecera2.Count(); i++)
                    {
                        ws.Cells[index, i + 2].Value = formato.Cabecera2[i];
                        ws.Cells[index, i + 2, index + 1, i + 2].Merge = true;
                    }

                    rg = ws.Cells[index, 2, index, formato.Cabecera2.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    rg = ws.Cells[index + 1, 2, index + 1, formato.Cabecera2.Count() + 1];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    //Contenido de la tabla2
                    //------------------------------------------------------------------------------------------------
                    index += 2;
                    for (int i = 0; i < formato.Contenido2.Count(); i++)//Controla el llenado vertical
                    {
                        ws.Cells[index, i + 2].Value = formato.Contenido2[i];
                        //Aplica el ancho de la columna
                        ws.Column(i + 2).Width = formato.AnchoColumnas2[i];

                        //Aplica el borde por fila
                        rg = ws.Cells[index, 2, index, formato.Contenido2.Count() + 1];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 4);
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
    }
}
