using COES.Servicios.Aplicacion.IEOD;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace COES.Servicios.Aplicacion.Helper
{
    /// <summary>
    /// Utileria del EEPlus
    /// </summary>
    public static class UtilExcel
    {
        /// <summary>
        /// get the Cell address from excel given a row and column number
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetCellAddress(int row, int col)
        {
            StringBuilder sb = new StringBuilder();

            do
            {
                col--;
                sb.Insert(0, (char)('A' + (col % 26)));
                col /= 26;
            } while (col > 0);
            sb.Append(row);
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        public static void BackgroundColor(ExcelRange range, Color color)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        public static void FontColor(ExcelRange range, Color color)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Font.Color.SetColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        public static void FontBold(ExcelRange range)
        {
            range.Style.Font.Bold = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        public static void SetFontBold(this ExcelRange range)
        {
            range.Style.Font.Bold = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        public static void SetFontColor(this ExcelRange range, Color color)
        {
            range.Style.Font.Color.SetColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="font"></param>
        public static void SetFont(this ExcelRange range, Font font)
        {
            range.Style.Font.SetFromFont(font);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        public static void SetBackgroundColor(this ExcelRange range, Color color)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        public static void SetAlignment(this ExcelRange range, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center)
        {
            range.Style.HorizontalAlignment = horizontalAlignment;
            range.Style.VerticalAlignment = verticalAlignment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        public static void Alignment(ExcelRange range, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center)
        {
            range.Style.HorizontalAlignment = horizontalAlignment;
            range.Style.VerticalAlignment = verticalAlignment;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="borderStyle"></param>
        /// <param name="borderColor"></param>
        public static void AllBorders(ExcelRange range, ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin, Color borderColor = default(Color))
        {
            if (Equals(borderColor, default(Color))) borderColor = Color.Black;

            range.Style.Border.Left.Style = borderStyle;
            range.Style.Border.Right.Style = borderStyle;
            range.Style.Border.Top.Style = borderStyle;
            range.Style.Border.Bottom.Style = borderStyle;

            range.Style.Border.Left.Color.SetColor(borderColor);
            range.Style.Border.Right.Color.SetColor(borderColor);
            range.Style.Border.Top.Color.SetColor(borderColor);
            range.Style.Border.Bottom.Color.SetColor(borderColor);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="borderStyle"></param>
        /// <param name="borderColor"></param>
        public static void BorderAround(ExcelRange range, ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin, Color borderColor = default(Color))
        {
            if (Equals(borderColor, default(Color))) borderColor = Color.Black;
            range.Style.Border.BorderAround(borderStyle, borderColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="alineacionVert"></param>
        /// <param name="alineacionHorz"></param>
        /// <param name="colorTexto"></param>
        /// <param name="colorFondo"></param>
        /// <param name="font"></param>
        /// <param name="tamanioLetra"></param>
        /// <param name="enNegrita"></param>
        /// <param name="wrap"></param>
        public static void SetFormatoCelda(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin
                                    , string alineacionVert, string alineacionHorz, string colorTexto, string colorFondo, string font, int tamanioLetra, bool enNegrita, bool? wrap = false)
        {
            if (wrap.GetValueOrDefault(false))
                UtilExcel.CeldasExcelWrapText(ws, filaIni, coluIni, filaFin, coluFin);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIni, coluIni, filaFin, coluFin, alineacionVert);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIni, coluIni, filaFin, coluFin, alineacionHorz);
            UtilExcel.CeldasExcelColorTexto(ws, filaIni, coluIni, filaFin, coluFin, colorTexto);
            UtilExcel.CeldasExcelColorFondo(ws, filaIni, coluIni, filaFin, coluFin, colorFondo);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIni, coluIni, filaFin, coluFin, font, tamanioLetra);
            if (enNegrita)
                UtilExcel.CeldasExcelEnNegrita(ws, filaIni, coluIni, filaFin, coluFin);
        }

        /// <summary>
        /// Alinear  verticalmente a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="alineacion"></param>
        /// <returns></returns>
        public static void CeldasExcelAlinearVerticalmente(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string alineacion)
        {
            var rg = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            switch (alineacion)
            {
                case "Arriba": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Top; break;
                case "Centro": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; break;
                case "Abajo": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom; break;
                case "Distribuido": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Distributed; break;
                case "Justificado": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Justify; break;
            }
        }
        
        /// <summary>
        /// Dar borde a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelBordear(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloqueABordear = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloqueABordear.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorBordeTablaRepEje));

        }

        /// <summary>
        /// Alinear  horizontalmente a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="alineacion"></param>
        /// <returns></returns>
        public static void CeldasExcelAlinearHorizontalmente(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string alineacion)
        {
            var rg = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            switch (alineacion)
            {
                case "General": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.General; break;
                case "Izquierda": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; break;
                case "Centro": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; break;
                case "CentroContinuo": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous; break;
                case "Derecha": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; break;
                case "Lleno": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Fill; break;
                case "Distribuido": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Distributed; break;
                case "Justificado": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify; break;
            }
        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="tipoLetra"></param>
        /// <param name="tamLetra"></param>
        /// <returns></returns>
        public static void CeldasExcelTipoYTamanioLetra(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string tipoLetra, int tamLetra)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Font.SetFromFont(new Font(tipoLetra, tamLetra));
        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorFondo(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            if (color != null)
            {
                if (color.Contains(","))
                {
                    bloque.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                    bloque.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(Int32.Parse(color)));
                }
                if (color.Contains("#"))
                {
                    bloque.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                    bloque.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
                }
            }
        }

        /// <summary>
        /// Color para la cabecera
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorFondoYBorder(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, Color colorFondo, Color colorBorder)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = ExcelFillStyle.Solid;
            bloque.Style.Fill.BackgroundColor.SetColor(colorFondo);

            var borderTabla = bloque.Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;

            for (var i = filaIni; i <= filaFin; i++)
            {
                for (var j = coluIni; j <= coluFin; j++)
                {
                    var borderCelda = ws.Cells[i, j].Style.Border;
                    borderCelda.Bottom.Color.SetColor(colorBorder);
                    borderCelda.Top.Color.SetColor(colorBorder);
                    borderCelda.Left.Color.SetColor(colorBorder);
                    borderCelda.Right.Color.SetColor(colorBorder);
                }
            }
        }

        /// <summary>
        /// Color para la cabecera 2
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorFondoYBorder2(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin
            , Color colorFondo, Color colorBorderInterno, Color colorBorderExterno, Color fontColor)
        {
            CeldasExcelColorFondoYBorder(ws, filaIni, coluIni, filaFin, coluFin, colorFondo, colorBorderInterno);

            ws.Cells[filaIni, coluIni, filaIni, coluFin].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[filaIni, coluIni, filaIni, coluFin].Style.Fill.BackgroundColor.SetColor(colorFondo);
            ws.Cells[filaIni, coluIni, filaIni, coluFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[filaIni, coluIni, filaIni, coluFin].Style.Border.Top.Color.SetColor(colorBorderExterno);

            ws.Cells[filaFin, coluIni, filaFin, coluFin].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[filaFin, coluIni, filaFin, coluFin].Style.Fill.BackgroundColor.SetColor(colorFondo);
            ws.Cells[filaFin, coluIni, filaFin, coluFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[filaFin, coluIni, filaFin, coluFin].Style.Border.Bottom.Color.SetColor(colorBorderExterno);
            /*
            //ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Fill.BackgroundColor.SetColor(colorFondo);
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Border.Left.Color.SetColor(colorBorderExterno);
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Border.Right.Color.SetColor(colorBorderExterno);*/

            ws.Cells[filaIni, coluIni, filaFin, coluFin].Style.Font.Color.SetColor(fontColor);
        }

        public static void CeldasExcelColorFondoYBorder3(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, Color colorBorder, string alineacion)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = ExcelFillStyle.Solid;

            var borderTabla = bloque.Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;

            for (var i = filaIni; i <= filaFin; i++)
            {
                for (var j = coluIni; j <= coluFin; j++)
                {
                    var borderCelda = ws.Cells[i, j].Style.Border;
                    switch (alineacion)
                    {
                        case "Arriba": borderCelda.Top.Color.SetColor(colorBorder); break;
                        case "Abajo": borderCelda.Bottom.Color.SetColor(colorBorder); break;
                        case "Izquierda": borderCelda.Left.Color.SetColor(colorBorder); break;
                        case "Derecha": borderCelda.Right.Color.SetColor(colorBorder); break;
                    }
                }
            }
        }

        public static void CeldasExcelColorFondoYBorderSoloUnLado(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color, string alineacion)
        {
            for (var i = filaIni; i <= filaFin; i++)
            {
                for (var j = coluIni; j <= coluFin; j++)
                {
                    ws.Cells[i, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    var borderCelda = ws.Cells[i, j].Style.Border;
                    var colorBorder = ColorTranslator.FromHtml(color);
                    switch (alineacion)
                    {
                        case "Arriba":
                            borderCelda.Top.Style = ExcelBorderStyle.Thin;
                            borderCelda.Top.Color.SetColor(colorBorder);
                            break;
                        case "Abajo":
                            borderCelda.Bottom.Style = ExcelBorderStyle.Thin;
                            borderCelda.Bottom.Color.SetColor(colorBorder);
                            break;
                        case "Izquierda":
                            borderCelda.Left.Style = ExcelBorderStyle.Thin;
                            borderCelda.Left.Color.SetColor(colorBorder);
                            break;
                        case "Derecha":
                            borderCelda.Right.Style = ExcelBorderStyle.Thin; 
                            borderCelda.Right.Color.SetColor(colorBorder);
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorTexto(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            if (color.Contains(","))
            {
                bloque.Style.Font.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                bloque.Style.Font.Color.SetColor(ColorTranslator.FromHtml(color));
            }

        }

        /// <summary>
        ///  Colocar en negrita a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelEnNegrita(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            if (filaIni <= filaFin)
            {
                var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
                bloque.Style.Font.Bold = true;
            }
        }

        /// <summary>
        ///  Dar Wrap a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <returns></returns>
        public static void CeldasExcelWrapText(ExcelWorksheet ws, int filaIni, int coluIni)
        {
            var bloque = ws.Cells[filaIni, coluIni];
            bloque.Style.WrapText = true;
        }

        /// <summary>
        /// Dar Wrap a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        public static void CeldasExcelWrapText(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.WrapText = true;
        }

        public static void CeldasExcelNoWrapText(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.WrapText = false;
        }

        /// <summary>
        /// Dar indent a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="indentacion"></param>
        public static void CeldasExcelIndentar(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, int indentacion)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Indent = indentacion;
        }

        /// <summary>
        /// Dar formato de decimales
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="numDecimales"></param>
        public static void CeldasExcelFormatoNumero(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, int numDecimales)
        {
            string strCeros = string.Empty;
            for (int i = 0; i < numDecimales; i++) strCeros += "0";

            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Numberformat.Format = "#,##0" + (strCeros != "" ? "." + strCeros : "");
        }

        /// <summary>
        /// Obtener la cantidad de decimales de una celda con formato de numero
        /// </summary>
        /// <param name="formatoNumber"></param>
        /// <param name="numDecMinimoDefault"></param>
        /// <returns></returns>
        public static int NumeroDecimalFromFormato(string formatoNumber, int numDecMinimoDefault) 
        {
            formatoNumber = (formatoNumber ?? "").Trim();

            if (formatoNumber.Contains(".")) {
                int posPunto = formatoNumber.LastIndexOf(".");
                int numDec = formatoNumber.Length - posPunto;
                return numDec >= numDecMinimoDefault ? numDec : numDecMinimoDefault;
            }

            return numDecMinimoDefault;
        }

        /// <summary>
        /// Dar formato de porcentaje
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="numDecimales"></param>
        public static void CeldasExcelFormatoPorcentaje(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, int numDecimales)
        {
            string strCeros = string.Empty;
            for (int i = 0; i < numDecimales; i++) strCeros += "0";

            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Numberformat.Format = "#,##0" + (strCeros != "" ? "." + strCeros : "") + "%";
        }

        /// <summary>
        /// Configura la imagen que va en el excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="img"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void AddImage(ExcelWorksheet ws, System.Drawing.Image img, int row, int col)
        {
            int rowImg = row - 1;
            int colImg = col - 1;
            rowImg = rowImg > 0 ? rowImg : 0;
            colImg = colImg > 0 ? colImg : 0;

            if (img != null)
            {
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Row = rowImg;
                picture.From.Column = colImg;
                picture.From.ColumnOff = Pixel2MTU(12);
                picture.From.RowOff = Pixel2MTU(10);
                picture.SetSize(165, 83);
            }
        }

        /// <summary>
        /// Inserta Imagen COES en Archivo Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="img"></param>
        public static void AddImageXY(ExcelWorksheet ws, int columnIndex, int rowIndex, System.Drawing.Image img)
        {
            ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
            picture.From.Column = columnIndex;
            picture.From.Row = rowIndex;
            picture.From.ColumnOff = Pixel2MTU(2); //Two pixel space for better alignment
            picture.From.RowOff = Pixel2MTU(2);//Two pixel space for better alignment
            picture.SetSize(150, 90);
        }

        /// <summary>
        /// Agregar imagen de carpeta local
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filePath"></param>
        public static void AddImageLocal(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);
            }
        }

        public static void AddImageLocalAlto4Filas(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(150, 90);
            }
        }

        /// <summary>
        /// Agregar imagen de carpeta local
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filePath"></param>
        /// <param name="PixelWidth"></param>
        /// <param name="PixelHeight"></param>
        public static void AddImageLocal(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath, int PixelWidth, int PixelHeight)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(PixelWidth, PixelHeight);
            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        private static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        /// <summary>
        /// Bordea el perimetro linea gruesa
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="color"></param>
        public static void BorderCeldasLineaGruesa(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, string color) //BORDES
        {
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(color));
        }

        /// <summary>
        /// Bordea el perimetro linea delgada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="color"></param>
        /// <param name="iterativo"></param>
        public static void BorderCeldasLineaDelgada(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, string color, bool iterativoCol = false, bool iterativoRow = false) //BORDES
        {
            if (!iterativoCol)
            {
                ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(color));

                ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));

                ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(color));

                ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(color));
            }
            else
            {
                for (int c = colIni; c <= colFin; c++)
                {
                    ws.Cells[rowIni, c, rowIni, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[rowIni, c, rowIni, c].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[rowFin, c, rowFin, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[rowFin, c, rowFin, c].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[rowIni, c, rowFin, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[rowIni, c, rowFin, c].Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[rowIni, c, rowFin, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[rowIni, c, rowFin, c].Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(color));
                }
            }

            if (iterativoRow)
            {
                for (int r = rowIni; r <= rowFin; r++)
                {
                    ws.Cells[r, colIni, r, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[r, colIni, r, colIni].Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[r, colFin, r, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[r, colFin, r, colFin].Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[r, colIni, r, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[r, colIni, r, colFin].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(color));

                    ws.Cells[r, colIni, r, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[r, colIni, r, colFin].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));
                }
            }
        }

        /// <summary>
        /// Bordea cada celda con linea continua
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="color"></param>
        public static void BorderCeldasThin(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, string color)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
            if (color.Contains(","))
            {
                borderTabla.Bottom.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Top.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Left.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Right.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                borderTabla.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Top.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Left.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Right.Color.SetColor(ColorTranslator.FromHtml(color));
            }
        }

        /// <summary>
        /// Bordea el perimetro linea delgada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="color"></param>
        public static void BorderCeldasLineaDiscontinua(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, string color) //BORDES
        {
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Dashed;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Dashed;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Dashed;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(color));

            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Dashed;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(color));
        }

        /// <summary>
        /// Bordea cada celda con linea discontinua
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="color"></param>
        public static void BorderCeldasHair(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, string color)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;

            if (color.Contains(","))
            {
                borderTabla.Bottom.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Top.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Left.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
                borderTabla.Right.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                borderTabla.Bottom.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Top.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Left.Color.SetColor(ColorTranslator.FromHtml(color));
                borderTabla.Right.Color.SetColor(ColorTranslator.FromHtml(color));
            }
        }

        /// <summary>
        /// Bordea cada celda con linea discontinua
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;
        }

        /// <summary>
        /// Bordea cada celda con linea continua
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas2(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Bordes Laterales, arriba y abajo
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas3(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        }

        /// <summary>
        /// Bordea celda con linea discontinua y color de borde
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas4(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, Color color)
        {
            //ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border.BorderAround(ExcelBorderStyle.Dotted, color);
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;
            borderTabla.Bottom.Color.SetColor(color);
            borderTabla.Top.Color.SetColor(color);
            borderTabla.Left.Color.SetColor(color);
            borderTabla.Right.Color.SetColor(color);
        }

        /// <summary>
        /// Bordea celda con linea thin y color de borde
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas4_1(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, Color color)
        {
            //ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border.BorderAround(ExcelBorderStyle.Dotted, color);
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
            borderTabla.Bottom.Color.SetColor(color);
            borderTabla.Top.Color.SetColor(color);
            borderTabla.Left.Color.SetColor(color);
            borderTabla.Right.Color.SetColor(color);
        }

        /// <summary>
        /// Bordea el perimetro linea gruesa
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas5(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin) //BORDES
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thick;
        }

        /// <summary>
        /// Bordea el perimetro linea delgada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas5_1(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin) //BORDES
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Bordea el rango pero no dentro
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldas6(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, Color colorFondo, Color colorCelda)
        {
            for (int c = colIni; c <= colFin; c++)
            {
                ws.Cells[rowIni, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIni, c].Style.Fill.BackgroundColor.SetColor(colorFondo);
                ws.Cells[rowIni, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, c].Style.Border.Top.Color.SetColor(colorCelda);

                ws.Cells[rowFin, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowFin, c].Style.Fill.BackgroundColor.SetColor(colorFondo);
                ws.Cells[rowFin, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowFin, c].Style.Border.Bottom.Color.SetColor(colorCelda);

                ws.Cells[rowIni, c, rowFin, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIni, c, rowFin, c].Style.Fill.BackgroundColor.SetColor(colorFondo);
                ws.Cells[rowIni, c, rowFin, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, c, rowFin, c].Style.Border.Left.Color.SetColor(colorCelda);
                ws.Cells[rowIni, c, rowFin, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[rowIni, c, rowFin, c].Style.Border.Right.Color.SetColor(colorCelda);
            }
        }

        /// <summary>
        /// Bordea cada celda con linea continua
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldasPunteado(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Dotted;
        }

        /// <summary>
        /// Bordes Laterales, arriba y abajo
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void BorderCeldasDobleSoloLateralDerecha(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Double;
        }

        /// <summary>
        /// Borra el contenido de las celdas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        public static void BorrarCeldasExcel(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            for (int f = filaIni; f <= filaFin; f++)
            {
                for (int c = coluIni; c <= coluFin; c++)
                {
                    ws.Cells[f, c].Value = null;
                }
            }
        }

        /// <summary>
        ///  Agrupar varias celdas en un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelAgrupar(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Merge = true;
        }

        /// <summary>
        /// Agregar comentario a excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="colData"></param>
        /// <param name="texto"></param>
        public static void AgregarComentarioExcel(ExcelWorksheet ws, int row, int colData, string texto)
        {
            if (!string.IsNullOrEmpty(texto) && !string.IsNullOrWhiteSpace(texto))
            {
                ExcelComment cmd = ws.Cells[row, colData].AddComment(texto, "COES");
                cmd.From.Column = colData; //Zero Index base  
                cmd.To.Column = colData + 7;
                cmd.From.Row = row;
                cmd.To.Row = row + 3;
                cmd.AutoFit = true;
            }
        }

        /// <summary>
        /// Agregar comentario a excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="colData"></param>
        /// <param name="rowIniComment"></param>
        /// <param name="texto"></param>
        public static void AgregarComentarioExcel2(ExcelWorksheet ws, int row, int colData, int rowIniComment, string texto)
        {
            if (!string.IsNullOrEmpty(texto) && !string.IsNullOrWhiteSpace(texto))
            {
                ExcelComment cmd = ws.Cells[row, colData].AddComment(texto, "COES");
                cmd.Visible = true;
                cmd.From.Column = colData; //Zero Index base  
                cmd.To.Column = colData + 7;
                cmd.From.Row = rowIniComment;
                cmd.To.Row = rowIniComment + 3;
                cmd.AutoFit = true;
                cmd.Visible = false;
            }
        }


        /// <summary>
        /// formato para las anotaciones  en el reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        public static void FormatoNota(ExcelWorksheet ws, int rowIni, int colIni)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.SetFromFont(new Font("Arial", 9));
                r1.Style.Font.Italic = true;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// formato para las anotaciones  en el reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="listaNota"></param>
        public static void FormatoNotaNegrita(ExcelWorksheet ws, int rowIni, int colIni, List<string> listaNota)
        {
            if (listaNota != null)
            {
                foreach (var regTexto in listaNota)
                {
                    FormatoNotaNegrita(ws, rowIni, colIni, regTexto);
                    rowIni++;
                }
            }
        }
        /// <summary>
        /// formato para las anotaciones  en el reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="listaNota"></param>
        public static void FormatoNotaNegrita(ExcelWorksheet ws, int rowIni, int colIni, string regTexto)
        {
            string texto = regTexto.Trim();
            int pos2Puntos = texto.IndexOf(":");

            string textHastaPuntos = pos2Puntos > 0 ? texto.Substring(0, pos2Puntos + 1) : string.Empty;
            string textDespuesPuntos = texto.Substring(pos2Puntos + 1, texto.Length - pos2Puntos - 1);

            ws.Cells[rowIni, colIni].Value = null;
            ws.Cells[rowIni, colIni].Style.Font.Color.SetColor(Color.Black);
            ws.Cells[rowIni, colIni].Style.Font.SetFromFont(new Font("Arial", 9));
            ws.Cells[rowIni, colIni].Style.Font.Italic = true;
            ws.Cells[rowIni, colIni].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowIni, colIni].IsRichText = true;
            ExcelRichTextCollection rtfCollection = ws.Cells[rowIni, colIni].RichText;
            ExcelRichText ert = null;

            if (pos2Puntos > 0)
            {
                ert = rtfCollection.Add(textHastaPuntos);
                ert.Bold = true;
            }
            ert = rtfCollection.Add(textDespuesPuntos);
            ert.Bold = false;

        }

        public static void OcultaTextoRegion(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            using (ExcelRange r = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r.Style.Font.Color.SetColor(Color.White);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="width"></param>
        public static void SetTrueColumnWidth(this ExcelColumn column, double width)
        {
            // Deduce what the column width would really get set to.
            var z = width >= (1 + 2 / 3)
                ? Math.Round((Math.Round(7 * (width - 1 / 256), 0) - 5) / 7, 2)
                : Math.Round((Math.Round(12 * (width - 1 / 256), 0) - Math.Round(5 * width, 0)) / 12, 2);

            // How far off? (will be less than 1)
            var errorAmt = width - z;

            // Calculate what amount to tack onto the original amount to result in the closest possible setting.
            var adj = width >= 1 + 2 / 3
                ? Math.Round(7 * errorAmt - 7 / 256, 0) / 7
                : Math.Round(12 * errorAmt - 12 / 256, 0) / 12 + (2 / 12);

            // Set width to a scaled-value that should result in the nearest possible value to the true desired setting.
            if (z > 0)
            {
                column.Width = width + adj;
                return;
            }

            column.Width = 0d;
        }

        public static void AnchoColumna(ExcelWorksheet ws, int z, decimal valor)
        {
            ws.Column(z).Width = decimal.ToDouble(valor);
        }

        /// <summary>
        /// Convertir numero a texto español
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NumeroATexto(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + NumeroATexto(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + NumeroATexto(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = NumeroATexto(Math.Truncate(value / 10) * 10) + " Y " + NumeroATexto(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + NumeroATexto(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = NumeroATexto(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = NumeroATexto(Math.Truncate(value / 100) * 100) + " " + NumeroATexto(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + NumeroATexto(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = NumeroATexto(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + NumeroATexto(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + NumeroATexto(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = NumeroATexto(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + NumeroATexto(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + NumeroATexto(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = NumeroATexto(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + NumeroATexto(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }

        /// <summary>
        /// Obtener letra de columna Excel
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
