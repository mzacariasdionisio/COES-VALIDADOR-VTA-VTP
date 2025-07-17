using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Transferencias;
using System.Xaml;

namespace COES.Servicios.Aplicacion.CPPA.Helper
{
    /// <summary>
    /// Crea un archivo Excel
    /// </summary>
    public static class ExcelDocumentCPPA
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExcelDocumentCPPA));
        private static readonly HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
        private static readonly HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        private static readonly System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());


        /// <summary>
        /// Genera el reporte en la ruta especificada
        /// </summary>
        /// <param name="ListaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="rutaArchivo">Ruta y nombre del archivo donde se generará el reporte</param>
        /// <param name="mostrarLogoyTitulo">Determina si se muestra el logo o no</param>
        public static void ExportarReporte(List<CpaExcelHoja> ListaExcelHoja, string rutaArchivo, bool mostrarLogoyTitulo = true)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append("ExcelDocumentRer.ExportarReporte(List<CpaExcelHoja> ListaExcelHoja, string rutaArchivo) - ListaExcelHoja = ");
            metodo.Append(ListaExcelHoja);
            metodo.Append(", rutaArchivo = ");
            metodo.Append(rutaArchivo);

            try
            {
                FileInfo newFile = new FileInfo(rutaArchivo);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaArchivo);
                }

                using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                {
                    foreach (CpaExcelHoja excelHoja in ListaExcelHoja)
                    {
                        ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(excelHoja.NombreHoja);
                        EscribirHoja(ws, excelHoja, mostrarLogoyTitulo);
                    }
                    excelPackage.Save();
                }
            }
            catch (FileNotFoundException e1)
            {
                metodo.Append(", e1.Message: ");
                metodo.Append(e1.Message);
                Log.Error(metodo.ToString(), e1);
                throw new FileNotFoundException(e1.Message, e1);
            }
            catch (Exception e2)
            {
                metodo.Append(", e2.Message: ");
                metodo.Append(e2.Message);
                Log.Error(metodo.ToString(), e2);
                throw;
            }
        }


        /// <summary>
        /// Crea el contenido de la hoja Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="excelHoja"></param>
        /// <param name="mostrarLogoyTitulo"></param>
        private static void EscribirHoja(ExcelWorksheet ws, CpaExcelHoja excelHoja, bool mostrarLogoyTitulo)
        {
            if (ws != null && excelHoja != null)
            {
                int row = 1;
                int col = 1;

                //Titulo de la tabla
                //------------------------------------------------------------------------------------------------
                if (mostrarLogoyTitulo)
                {
                    row = 2;
                    col = 2;
                    row = EscribirLogoyTitulos(ws, excelHoja, row, (col + 1));
                    row += 2;
                }
                int rowInicial = row;
                //int colInicial = col;

                //Cabecera de la tabla
                //------------------------------------------------------------------------------------------------
                row = EscribirCabeceraoPie(excelHoja.ListaCabeceras, ws, row, col, out int numColumnas1);

                //Cuerpo de la tabla
                //------------------------------------------------------------------------------------------------
                row = EscribirCuerpo(excelHoja.CuerpoOculto, true, ws, row, col, numColumnas1);
                row = EscribirCuerpo(excelHoja.Cuerpo, false, ws, row, col, numColumnas1);

                //Pie de la tabla
                //------------------------------------------------------------------------------------------------
                row = EscribirCabeceraoPie(excelHoja.ListaPies, ws, row, col, out int numColumnas2);
                int rowFinal = row;

                //Ancho de columnas
                //------------------------------------------------------------------------------------------------
                AplicarAnchoColumna(excelHoja, ws, rowInicial, rowFinal, col);
            }
        }


        /// <summary>
        /// Escribe el modelo según el caso: cabecera o pie, en el archivo excel.
        /// Nota: En caso el parámetro excelHoja sea nulo, no tomará en cuenta los parámetros row y col para colocar los títulos
        /// Si no, centrará la imagen del logo
        /// </summary>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="excelHoja">CpaExcelHoja</param>
        /// <param name="row">Fila donde inicia</param>
        /// <param name="col">Columna donde inicia</param>/// 
        /// <returns>Devuelve la última fila</returns>
        private static int EscribirLogoyTitulos(ExcelWorksheet ws, CpaExcelHoja excelHoja, int row, int col)
        {
            ExcelPicture picture = null;
            lock (img)
            {
                picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
            }

            if (excelHoja != null)
            {
                picture.SetPosition(20, 60);
                picture.SetSize(135, 45);
                int rowInicial = row;
                ws.Cells[row, col].Value = excelHoja.Titulo; row++;
                ws.Cells[row, col].Value = excelHoja.Subtitulo1;
                
                if (!string.IsNullOrWhiteSpace(excelHoja.Subtitulo2))
                {
                    row++;
                    ws.Cells[row, col].Value = excelHoja.Subtitulo2;
                }

                if (excelHoja.ListaEstilosTitulos != null)
                {
                    for (int r = 0; r <= row - rowInicial; r++)
                    {
                        SetearEstiloCelda(ws.Cells[rowInicial + r, col], excelHoja.ListaEstilosTitulos[r]);
                    }
                }
            }
            else
            {
                picture.SetPosition(20, 250);
                picture.SetSize(180, 60);
            }

            return row;
        }


        /// <summary>
        /// Escribe el modelo según el caso: cabecera o pie, en el archivo excel
        /// </summary>
        /// <param name="listaModelos">Array de una lista de objeto: CpaExcelModelo</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila donde inicia el pintado del modelo</param>
        /// <param name="col">Columna donde inicia el pintado del modelo</param>
        /// <param name="numColumnas">Mayor número de columna de la cabecera</param>
        /// <returns>Devuelve el número de filas agregadas</returns>
        private static int EscribirCabeceraoPie(List<CpaExcelModelo>[] listaModelos, ExcelWorksheet ws, int row, int col, out int numColumnas)
        {
            int rowUltimo = row;
            int colInicio = col;
            numColumnas = 0;
            bool esListaModelosMayoraCero = (listaModelos != null && listaModelos.Length > 0);

            if (esListaModelosMayoraCero)
            {
                Dictionary<int, List<int>> colUsadasPorFila = new Dictionary<int, List<int>>();
                foreach (List<CpaExcelModelo> listaModelo in listaModelos)
                {
                    bool esListaModeloMayoraCero = (listaModelo != null && listaModelo.Count > 0);
                    int numeroColumnasModelo = 0;

                    if (esListaModeloMayoraCero)
                    {
                        col = colInicio;
                        int mayorNumLineasTexto = 1; //Mayor número de líneas de texto
                        foreach (CpaExcelModelo modelo in listaModelo)
                        {
                            //Aumenta el valor de la "columna inicial", si ya esta siendo usada
                            if (colUsadasPorFila.ContainsKey(row))
                            {
                                while (colUsadasPorFila[row].Contains(col))
                                {
                                    col += 1;
                                }
                            }

                            //Aquí se coloca el nombre/etiqueta para la celda
                            ws.Cells[row, col].Value = modelo.Nombre;

                            bool esNumColumnasMayoraUno = (modelo.NumColumnas > 1);
                            bool esNumFilasMayoraUno = (modelo.NumFilas > 1);
                            bool esNumColumnasIgualaUno = (modelo.NumColumnas == 1);
                            bool esNumFilasIgualaUno = (modelo.NumFilas == 1);

                            if (esNumColumnasMayoraUno && esNumFilasMayoraUno)
                            {
                                AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col + (modelo.NumColumnas - 1), true, true, ConstantesCPPA.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesCPPA.SumaColumnas);
                            }
                            else if (esNumColumnasMayoraUno || esNumFilasMayoraUno)
                            {
                                if (esNumColumnasMayoraUno)
                                {
                                    AplicarEstiloaRangoCelda(ws, row, col, row, col + (modelo.NumColumnas - 1), true, true, ConstantesCPPA.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesCPPA.SumaColumnas);
                                }
                                else if (esNumFilasMayoraUno)
                                {
                                    AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col, true, true, ConstantesCPPA.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesCPPA.SumaFilas);
                                }
                            }
                            else if (esNumColumnasIgualaUno || esNumFilasIgualaUno)
                            {
                                AplicarEstiloaRangoCelda(ws, row, col, row, col, false, true, ConstantesCPPA.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesCPPA.SumaFilas);
                            }
                            col += modelo.NumColumnas;
                            numeroColumnasModelo += modelo.NumColumnas;

                            if ((row + modelo.NumFilas) > rowUltimo)
                            {
                                rowUltimo = (row + modelo.NumFilas);
                            }

                            mayorNumLineasTexto = ObtenerMayorNumLineasTexto(modelo.Nombre, mayorNumLineasTexto);
                        }

                        //Aplicar nueva altura a la fila en caso exista más de una línea de texto
                        if (mayorNumLineasTexto > 1)
                        {
                            ws.Row(row).Height = mayorNumLineasTexto * ws.DefaultRowHeight;
                        }

                        row += 1;
                    }

                    if (numeroColumnasModelo > numColumnas)
                    {
                        numColumnas = numeroColumnasModelo;
                    }
                }

                return rowUltimo;
            }
            else
            {
                return rowUltimo;
            }
        }


        /// <summary>
        /// Escribe el cuerpo de datos
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <param name="ocultar"></param>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="numColumnas"></param>
        /// <returns></returns>
        public static int EscribirCuerpo(CpaExcelCuerpo cuerpo, bool ocultar, ExcelWorksheet ws, int row, int col, int numColumnas)
        {
            int rowUltimo = row;
            bool esListaRegistroMayoraCero = (cuerpo != null && cuerpo.ListaRegistros != null && cuerpo.ListaRegistros.Length > 0);

            if (esListaRegistroMayoraCero)
            {
                for (int i = 0; i < cuerpo.ListaRegistros.Count(); i++) //Filas
                {
                    for (int j = 0; j < cuerpo.ListaRegistros[0].Count; j++) //Columnas
                    {
                        string cell_value = WebUtility.HtmlDecode(cuerpo.ListaRegistros[i][j]);
                        try
                        {
                            bool justDrawValue = (
                                string.IsNullOrWhiteSpace(cell_value) ||
                                cuerpo.ListaTipo == null ||
                                cuerpo.ListaTipo[j] == null ||
                                cuerpo.ListaTipo[j] == ConstantesCPPA.TipoColumnaString
                                );
                            if (justDrawValue)
                            {
                                ws.Cells[row + i, col + j].Value = cell_value;
                            }
                            else
                            {
                                switch (cuerpo.ListaTipo[j])
                                {
                                    case ConstantesCPPA.TipoColumnaInteger:
                                        ws.Cells[row + i, col + j].Value = Int32.Parse(cell_value);
                                        break;
                                    case ConstantesCPPA.TipoColumnaDouble:
                                        ws.Cells[row + i, col + j].Value = Double.Parse(cell_value);
                                        break;
                                    default:
                                        ws.Cells[row + i, col + j].Value = cell_value;
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ws.Cells[row + i, col + j].Value = cell_value;
                        }
                    }
                }

                AplicarEstiloaRegistros(cuerpo, ocultar, ws, row, col, numColumnas);
                row += cuerpo.ListaRegistros.Count();
                rowUltimo = row;
            }

            return rowUltimo;
        }


        /// <summary>
        /// Aplica estilo a todos los registros
        /// </summary>
        /// <param name="cuerpo">CpaExcelCuerpo</param>
        /// <param name="ocultar">Determina si oculta el cuerpo</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila inicial de rango de celda</param>
        /// <param name="col">Columna inicial de rango de celda</param>
        /// <param name="numColumnasCabecera">Número de columnas de la cabecera</param>
        private static void AplicarEstiloaRegistros(CpaExcelCuerpo cuerpo, bool ocultar, ExcelWorksheet ws, int row, int col, int numColumnasCabecera)
        {
            //Estilo por Default
            int numColumnas = cuerpo.ListaRegistros[0].Count < numColumnasCabecera ? numColumnasCabecera : cuerpo.ListaRegistros[0].Count;
            AplicarEstiloaRangoCelda(ws, row, col, row + cuerpo.ListaRegistros.Count() - 1, col + numColumnas - 1, false, true, ConstantesCPPA.SeccionCuerpo, ExcelHorizontalAlignment.Left);

            AplicarEstiloPersonalizado(ws, row, col, cuerpo, ocultar);
        }


        /// <summary>
        /// Agregar elementos al diccionario "columnas usadas por fila", 
        /// con la finalidad de saber en que columna empezar a pintar la fila
        /// </summary>
        /// <param name="colUsadasPorFila">Dictionary</param>
        /// <param name="row">Fila inicial del modelo (cabecera o pie)</param>
        /// <param name="col">Cantidad de columnas del modelo (cabecera o pie) </param>
        /// <param name="modelo">CpaExcelModelo</param>
        /// <param name="suma">determina el tipo de suma que se desea realizar (suma de columnas, suma de filas)</param>
        private static void AgregarColumnasUsadasPorFila(ref Dictionary<int, List<int>> colUsadasPorFila, int row, int col, CpaExcelModelo modelo, string suma)
        {
            for (int r = row; r < (row + modelo.NumFilas); r++)
            {
                if (colUsadasPorFila.ContainsKey(r))
                {
                    if (suma == ConstantesCPPA.SumaColumnas)
                    {
                        for (int c = col; c < col + modelo.NumColumnas; c++)
                        {
                            colUsadasPorFila[r].Add(c);
                        }
                    }
                    else if (suma == ConstantesCPPA.SumaFilas)
                    {
                        colUsadasPorFila[r].Add(col);
                    }
                }
                else
                {
                    colUsadasPorFila.Add(r, Enumerable.Range(col, modelo.NumColumnas).ToList());
                }
            }
        }


        /// <summary>
        /// Aplica estilo a donde se especifique
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colEstilo"></param>
        /// <param name="cantidadRegistros"></param>
        /// <param name="listaEstilo"></param>
        private static void AplicarEstilo(ExcelWorksheet ws, int row, int col, int colEstilo, int cantidadRegistros, List<CpaExcelEstilo> listaEstilo)
        {
            ExcelRange rangoCelda;
            if (listaEstilo != null && listaEstilo.Count > 0)
            {
                for (int i = 0; i < listaEstilo.Count; i++)
                {
                    if (listaEstilo[i] != null)
                    {
                        int j = (colEstilo > -1) ? colEstilo : i;
                        if (listaEstilo[i].ListaRangoFilas != null && listaEstilo[i].ListaRangoFilas.Count > 0)
                        {
                            foreach (string rangoFila in listaEstilo[i].ListaRangoFilas)
                            {
                                string[] aRangoFila = rangoFila.Split(',').ToArray();
                                rangoCelda = ws.Cells[row + int.Parse(aRangoFila[0]), col + j, row + int.Parse(aRangoFila[1]), col + j];
                                SetearEstiloCelda(rangoCelda, listaEstilo[i]);
                            }
                        }
                        else
                        {
                            rangoCelda = ws.Cells[row, col + j, row + cantidadRegistros - 1, col + j];
                            SetearEstiloCelda(rangoCelda, listaEstilo[i]);
                        }

                        if (listaEstilo[i].ListaEstilo != null && listaEstilo[i].ListaEstilo.Count > 0)
                        {
                            AplicarEstilo(ws, row, col, i, cantidadRegistros, listaEstilo[i].ListaEstilo);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Aplica estilo a un rango de celdas, en base a la sección
        /// </summary>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="rowInicial">Fila inicial de rango de celda</param>
        /// <param name="colInicial">Columna inicial de rango de celda</param>
        /// <param name="rowFinal">Fila final de rango de celda</param>
        /// <param name="colFinal">Columna final de rango de celda</param>
        /// <param name="merge">Determina si mezcla un rango de celdas</param>
        /// <param name="wrapText">Determina si el rango de celda es considerado como texto</param>
        /// <param name="seccion">Tipo de configuración de estilos</param>
        /// <param name="alineaHorizontal">Alineamiento horizontal</param>
        private static void AplicarEstiloaRangoCelda(ExcelWorksheet ws, int rowInicial, int colInicial, int rowFinal, int colFinal, bool merge, bool wrapText, int seccion, ExcelHorizontalAlignment alineaHorizontal = ExcelHorizontalAlignment.General)
        {
            ws.Cells[rowInicial, colInicial, rowFinal, colFinal].Merge = merge;
            ExcelRange rangoCelda = ws.Cells[rowInicial, colInicial, rowFinal, colFinal];

            Color fillBackgroundColor = new Color();
            Color fontColor = new Color();
            Color colorBorder = new Color();
            bool fontBold = false;
            int fontSize = 10;

            if (seccion == ConstantesCPPA.SeccionCuerpo)
            {
                fillBackgroundColor = ColorTranslator.FromHtml("#FFFFFF");
                fontColor = ColorTranslator.FromHtml("#245C86");
                colorBorder = ColorTranslator.FromHtml("#245C86");
                fontBold = false;
            }
            else if (seccion == ConstantesCPPA.SeccionCabeceraoPie)
            {
                fillBackgroundColor = ColorTranslator.FromHtml("#2980B9");
                fontColor = Color.White;
                colorBorder = ColorTranslator.FromHtml("#DADAD9");
                fontBold = true;
            }
            else if (seccion == ConstantesCPPA.SeccionTitulo)
            {
                fillBackgroundColor = ColorTranslator.FromHtml("#FFFFFF");
                fontColor = Color.Black;
                colorBorder = Color.Black;
                fontBold = true;
            }
            //INICIO: NO CAMBIAR EL ORDEN
            rangoCelda.Style.HorizontalAlignment = alineaHorizontal;
            rangoCelda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rangoCelda.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rangoCelda.Style.Fill.BackgroundColor.SetColor(fillBackgroundColor);
            rangoCelda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rangoCelda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rangoCelda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rangoCelda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rangoCelda.Style.Border.Bottom.Color.SetColor(colorBorder);
            rangoCelda.Style.Border.Left.Color.SetColor(colorBorder);
            rangoCelda.Style.Border.Right.Color.SetColor(colorBorder);
            rangoCelda.Style.Border.Top.Color.SetColor(colorBorder);
            rangoCelda.Style.Font.Color.SetColor(fontColor);
            rangoCelda.Style.Font.Size = fontSize;
            rangoCelda.Style.Font.Bold = fontBold;
            rangoCelda.Style.WrapText = wrapText;
            //FIN: NO CAMBIAR EL ORDEN

            //INICIO: NO BORRAR ESTE CODIGO
            //
            //rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
            //rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
            //rango.Style.Font.Size = 10;
            //rango.Style.Font.Bold = false;
            //rango.Style.WrapText = true;

            //string colorborder = "#245C86";
            //rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            //rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            //rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            //rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            //rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //
            //FIN: NO BORRAR ESTE CODIGO
        }


        /// <summary>
        /// Función para convertir un color a formato hexadecimal 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static string ColorToHex(Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }


        /// <summary>
        /// Aplica estilo personalizado a un rango de celdas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="cuerpo"></param>
        /// <param name="ocultar"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private static void AplicarEstiloPersonalizado(ExcelWorksheet ws, int row, int col, CpaExcelCuerpo cuerpo, bool ocultar)
        {
            ExcelRange rangoCelda;
            bool existeListaAlineaHorizontal = (cuerpo.ListaAlineaHorizontal != null && cuerpo.ListaAlineaHorizontal.Count > 0);
            if (existeListaAlineaHorizontal)
            {
                for (int i = 0; i < cuerpo.ListaAlineaHorizontal.Count; i++)
                {
                    rangoCelda = ws.Cells[row, col + i, row + cuerpo.ListaRegistros.Count() - 1, col + i];
                    if (cuerpo.ListaAlineaHorizontal[i] == ConstantesCPPA.AlineaColumnaIzquierda)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesCPPA.AlineaColumnaDerecha)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesCPPA.AlineaColumnaCentro)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesCPPA.AlineaColumnaJustificada)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                    }
                    else
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                }
            }

            AplicarEstilo(ws, row, col, -1, cuerpo.ListaRegistros.Count(), cuerpo.ListaEstilo);

            if (ocultar)
            {
                for (int r = row; r <= row + cuerpo.ListaRegistros.Count() - 1; r++)
                {
                    ws.Row(r).Hidden = true;
                }
            }
        }


        /// <summary>
        /// Aplicar el ancho de columnas en el archivo excel
        /// </summary>
        /// <param name="excelHoja">CpaExcelHoja</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="rowInicial">Final inicial</param>
        /// <param name="rowFinal">Final final</param>
        /// <param name="col">Columna donde inicia la aplicación del ancho de columna</param>
        private static void AplicarAnchoColumna(CpaExcelHoja excelHoja, ExcelWorksheet ws, int rowInicial, int rowFinal, int col)
        {
            bool esListaAnchoColumnaContadorMayoraCero = (excelHoja.ListaAnchoColumna != null && excelHoja.ListaAnchoColumna.Count > 0);

            if (esListaAnchoColumnaContadorMayoraCero)
            {
                for (int j = 0; j < excelHoja.ListaAnchoColumna.Count; j++)
                {
                    if (excelHoja.ListaAnchoColumna[j] == -1)
                    {
                        ExcelRange rangoCelda = ws.Cells[rowInicial, col + j, rowFinal, col + j];
                        rangoCelda.AutoFitColumns();
                    }
                    else
                    {
                        ws.Column(col + j).Width = excelHoja.ListaAnchoColumna[j];
                    }

                }
            }
        }


        /// <summary>
        /// Obtiene alineamiento horizontal en base a la nomenclatura del excel
        /// </summary>
        /// <param name="alineaHorizontal">Alineamiento horizontal</param>
        /// <returns>Retorna un alineamiento horizontal en base a la nomenclatura del excel</returns>
        private static ExcelHorizontalAlignment ObtenerAlineaHorizontal(string alineaHorizontal)
        {
            bool esAlineaHorizontalNulo = (alineaHorizontal == null);
            bool esAlineaHorizontalVacio = (alineaHorizontal == "");
            bool esAlineaHorizontalGeneral = (alineaHorizontal == "general");

            if (esAlineaHorizontalNulo || esAlineaHorizontalVacio || esAlineaHorizontalGeneral)
            {
                return ExcelHorizontalAlignment.General;
            }
            else if (alineaHorizontal == "left")
            {
                return ExcelHorizontalAlignment.Left;
            }
            else if (alineaHorizontal == "center")
            {
                return ExcelHorizontalAlignment.Center;
            }
            else if (alineaHorizontal == "centerContinuous")
            {
                return ExcelHorizontalAlignment.CenterContinuous;
            }
            else if (alineaHorizontal == "right")
            {
                return ExcelHorizontalAlignment.Right;
            }
            else if (alineaHorizontal == "fill")
            {
                return ExcelHorizontalAlignment.Fill;
            }
            else if (alineaHorizontal == "distributed")
            {
                return ExcelHorizontalAlignment.Distributed;
            }
            else if (alineaHorizontal == "justify")
            {
                return ExcelHorizontalAlignment.Justify;
            }
            else
            {
                return ExcelHorizontalAlignment.General;
            }
        }


        /// <summary>
        /// Asigna estilo a la celda con respecto a aplicar negita al texto, color de texto y color de fondo
        /// </summary>
        /// <param name="rango">Rango de la celda</param>
        /// <param name="excelEstilo">Clase con los datos del color</param>
        /// <returns>Devuelve el rango de la celda con el contenido asignado</returns>
        public static void SetearEstiloCelda(ExcelRange rango, CpaExcelEstilo excelEstilo)
        {
            if (!string.IsNullOrWhiteSpace(excelEstilo.NumberformatFormat))
            {
                rango.Style.Numberformat.Format = excelEstilo.NumberformatFormat;
            }

            if (excelEstilo.FontBold != null)
            {
                rango.Style.Font.Bold = excelEstilo.FontBold.Value; //true
            }

            if (excelEstilo.FontColor != null)
            {
                rango.Style.Font.Color.SetColor(excelEstilo.FontColor.Value); //Color.White
            }

            if (!string.IsNullOrWhiteSpace(excelEstilo.FillBackgroundColor))
            {
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(excelEstilo.FillBackgroundColor)); //ColorTranslator.FromHtml("#2980B9")
            }

            if (!string.IsNullOrWhiteSpace(excelEstilo.BorderColor))
            {
                Color colorBorder = ColorTranslator.FromHtml(excelEstilo.BorderColor);
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(colorBorder);
                rango.Style.Border.Right.Color.SetColor(colorBorder);
                rango.Style.Border.Top.Color.SetColor(colorBorder);
                rango.Style.Border.Bottom.Color.SetColor(colorBorder);
            }
            if (excelEstilo.FontSize != null)
            {
                rango.Style.Font.Size = (float)excelEstilo.FontSize;
            }
        }


        /// <summary>
        /// Obtiene el mayor número de líneas de texto con respecto a la comparación de: 
        /// i) las líneas de texto ingresado, 
        /// ii) con el actual mayor número de líneas de texto. 
        /// Donde una nueva línea empieza con \n
        /// </summary>
        /// <param name="texto">Texto a evaluar</param>
        /// <param name="actualMayorNumLineasTexto">Actual mayor número de líneas de texto</param>
        /// <returns>Devuelve el mayor número de lineas de texto, con respecto a la comparación realizada</returns>
        private static int ObtenerMayorNumLineasTexto(string texto, int actualMayorNumLineasTexto)
        {
            bool esTextoNulooVacio = (texto == null || texto == "");
            if (!esTextoNulooVacio)
            {
                int numLineasTexto = texto.Split('\n').Length;
                if (numLineasTexto > actualMayorNumLineasTexto)
                {
                    actualMayorNumLineasTexto = numLineasTexto;
                }
            }
            return actualMayorNumLineasTexto;
        }

        #region CU03

        public static void GenerarArchivoExcel(CPPAFormatoExcel formato, string rutaArchivo)
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
                        //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
                                if (header.Columnas > 1)
                                {
                                    ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                }
                                i += header.Columnas;
                            }

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido[0].Count() + 1];
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
                                //6,2
                                ws.Cells[index, i].Value = header.Etiqueta;
                                if (header.Columnas > 1)
                                {
                                    ws.Cells[index, i, index, i + (header.Columnas - 1)].Merge = true;
                                }
                                i += header.Columnas;
                            }
                            //ws.Cells[5, 2, 6, 2].Merge = true;
                            //ws.Cells[5, 2].Value = "Central";

                            //Aplica estilos a la fila de la cabecera
                            rg = ws.Cells[index, 2, index, formato.Contenido[0].Count() + 1];
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
                            //rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
        #endregion



        /// <summary>
        /// Permite generar el archivo de exportación de la tabla CPA_TOTAL_DEMANDADET
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="cpatdanio">Año del calculo del total de transmisores</param>
        /// <param name="NombTipo">Tipo</param>
        /// <param name="NombMes">Nombre Mes</param>
        /// <param name="IdAjuste">Id de Ajuste</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="ListaTotalDemandaDet">Lista de registros de CpaTotalDemandaDetDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoTotalDemanda(string fileName, 
                                                      int cpatdanio,
                                                      string NombTipo,
                                                      string NombMes,
                                                      string IdAjuste,
                                                      string NombRevision,
                                                      List<CpaTotalDemandaDetDTO> ListaTotalDemandaDet)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 1;

                    // TITULO
                    ExcelRange rg = ws.Cells[index, 2, index + 1, 6];
                    ws.Cells[index, 3].Value = "Totales presupuesto Anual " + cpatdanio;
                    ws.Cells[index + 1, 3].Value = "Participación" + " " + NombTipo;
                    rg.Style.Font.Bold = true;

                    // CABECERAS DE TABLA
                    index += 4;

                    rg = ws.Cells[index, 2, index, 6];

                    ws.Cells[index, 2].Value = cpatdanio + "-" + IdAjuste + "-" + NombRevision;
                    ws.Cells[index + 1, 2].Value = NombMes + "-" + cpatdanio;

                    ws.Cells[index, 3].Value = "Energía";
                    ws.Cells[index, 3, index, 4].Merge = true;

                    ws.Cells[index, 5].Value = "Potencia";
                    ws.Cells[index, 5, index, 6].Merge = true;

                    rg.Style.Font.Bold = true;
                    rg.Style.Font.Size = 11;
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);

                    string colorborder = "#245C86";
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));

                    index += 1;

                    ws.Cells[index, 3].Value = "MWh";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.000000000";
                    ws.Cells[index, 4].Value = "Miles S./";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.000";

                    ws.Cells[index, 5].Value = "MW";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.000000000";
                    ws.Cells[index, 6].Value = "Miles S./";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.000";

                    rg = ws.Cells[index, 2, index, 6];
                    rg.Style.Font.Bold = true;
                    rg.Style.WrapText = true;
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);

                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));

                    index++;
                    foreach (var item in ListaTotalDemandaDet)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = (item.Cpatddtotenemwh != null) ? item.Cpatddtotenemwh : null;
                        ws.Cells[index, 4].Value = (item.Cpatddtotenesoles != null) ? item.Cpatddtotenesoles : null;
                        ws.Cells[index, 5].Value = (item.Cpatddtotpotmw != null) ? item.Cpatddtotpotmw : null;
                        ws.Cells[index, 6].Value = (item.Cpatddtotpotsoles != null) ? item.Cpatddtotpotsoles : null;

                        // Border por celda
                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);

                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);

                        index++;
                    }

                    rg = ws.Cells[5, 2, 6, 2];
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                    rg.Style.Font.Color.SetColor(Color.Black);

                    // Fijar panel
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 20;

                    // Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                xlPackage.Save();
            }
        }


        /// <summary>
        /// Permite generar el archivo de exportación de la tabla CPA_TOTAL_TRANSMISORESDET 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="cpattanio">Año del calculo del total de transmisores</param>
        /// <param name="IdAjuste">Id de Ajuste</param>
        /// <param name="NombRevision">Nombre de la Versión de Recálculo de Potencia</param>
        /// <param name="ListaTotalTransmisoresDet">Lista de registros de CpaTotalTransmisoresDetDTO</param>
        /// <returns></returns>
        public static void GenerarFormatoTotalTransmisores(string fileName, 
                                                           int cpattanio,
                                                           string IdAjuste,
                                                           string NombRevision,
                                                           List<CpaTotalTransmisoresDetDTO> ListaTotalTransmisoresDet)
        {
            FileInfo newFile = new FileInfo(fileName);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int index = 1;

                    // TITULO
                    ws.Cells[index, 3].Value = "Totales presupuesto Anual " + cpattanio;
                    ws.Cells[index + 1, 3].Value = "Participación: Transmisor";
                    ws.Cells[index + 2, 3].Value = cpattanio + "-" + IdAjuste + "-" + NombRevision;

                    ExcelRange rg = ws.Cells[index, 3, index + 2, 3];
                    rg.Style.Font.Size = 11;
                    rg.Style.Font.Bold = true;

                    // CABECERA DE TABLA
                    index += 4;
                    ws.Cells[index, 2].Value = "EMPRESA";
                    ws.Cells[index, 3].Value = "Ene-" + (cpattanio - 1).ToString().Substring(2,2);
                    ws.Column(3).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 4].Value = "Feb-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(4).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 5].Value = "Mar-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(5).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 6].Value = "Abr-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(6).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 7].Value = "May-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(7).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 8].Value = "Jun-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(8).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 9].Value = "Jul-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(9).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 10].Value = "Ago-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(10).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 11].Value = "Sep-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(11).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 12].Value = "Oct-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(12).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 13].Value = "Nov-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(13).Style.Numberformat.Format = "#,##0.0000000000";
                    ws.Cells[index, 14].Value = "Dic-" + (cpattanio - 1).ToString().Substring(2, 2);
                    ws.Column(14).Style.Numberformat.Format = "#,##0.0000000000";

                    rg = ws.Cells[index, 2, index, 14];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 1);

                    index++;
                    foreach (var item in ListaTotalTransmisoresDet)
                    {
                        ws.Cells[index, 2].Value = item.Emprnomb.ToString();
                        ws.Cells[index, 3].Value = (item.Cpattdtotmes01 != null) ? item.Cpattdtotmes01 : null;
                        ws.Cells[index, 4].Value = (item.Cpattdtotmes02 != null) ? item.Cpattdtotmes02 : null;
                        ws.Cells[index, 5].Value = (item.Cpattdtotmes03 != null) ? item.Cpattdtotmes03 : null;
                        ws.Cells[index, 6].Value = (item.Cpattdtotmes04 != null) ? item.Cpattdtotmes04 : null;
                        ws.Cells[index, 7].Value = (item.Cpattdtotmes05 != null) ? item.Cpattdtotmes05 : null;
                        ws.Cells[index, 8].Value = (item.Cpattdtotmes06 != null) ? item.Cpattdtotmes06 : null;
                        ws.Cells[index, 9].Value = (item.Cpattdtotmes07 != null) ? item.Cpattdtotmes07 : null;
                        ws.Cells[index, 10].Value = (item.Cpattdtotmes08 != null) ? item.Cpattdtotmes08 : null;
                        ws.Cells[index, 11].Value = (item.Cpattdtotmes09 != null) ? item.Cpattdtotmes09 : null;
                        ws.Cells[index, 12].Value = (item.Cpattdtotmes10 != null) ? item.Cpattdtotmes10 : null;
                        ws.Cells[index, 13].Value = (item.Cpattdtotmes11 != null) ? item.Cpattdtotmes11 : null;
                        ws.Cells[index, 14].Value = (item.Cpattdtotmes12 != null) ? item.Cpattdtotmes12 : null;

                        // Border por celda
                        rg = ws.Cells[index, 2, index, 14];
                        rg.Style.WrapText = true;
                        rg = ObtenerEstiloCelda(rg, 0);

                        // Color de la columna de empresas
                        rg = ws.Cells[index, 2, index, 2];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);

                        index++;
                    }

                    // Fijar panel
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 40;
                    ws.Column(7).Width = 40;
                    ws.Column(8).Width = 40;
                    ws.Column(9).Width = 40;
                    ws.Column(10).Width = 40;
                    ws.Column(11).Width = 40;
                    ws.Column(12).Width = 40;
                    ws.Column(13).Width = 40;
                    ws.Column(14).Width = 40;

                    // Insertar el logo
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                    picture.SetPosition(10, 10);
                    picture.SetSize(135, 45);
                }

                xlPackage.Save();
            }
        }

    }
}
