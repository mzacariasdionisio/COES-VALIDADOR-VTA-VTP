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

namespace COES.Servicios.Aplicacion.PrimasRER.Helper
{
    /// <summary>
    /// Crea un archivo Excel
    /// </summary>
    public static class ExcelDocumentPrimasRER
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExcelDocumentPrimasRER));
        private static readonly HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
        private static readonly HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        private static readonly System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());


        /// <summary>
        /// Genera el reporte en la ruta especificada
        /// </summary>
        /// <param name="ListaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="rutaArchivo">Ruta y nombre del archivo donde se generará el reporte</param>
        /// <param name="mostrarLogoyTitulo">Determina si se muestra el logo o no</param>
        public static void ExportarReporte(List<RerExcelHoja> ListaExcelHoja, string rutaArchivo, bool mostrarLogoyTitulo = true)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append("ExcelDocumentRer.ExportarReporte(List<RerExcelHoja> ListaExcelHoja, string rutaArchivo) - ListaExcelHoja = ");
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
                    foreach (RerExcelHoja excelHoja in ListaExcelHoja)
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
                Log.Error(metodo.ToString());
                throw new FileNotFoundException(e1.Message, e1);
            }
            catch (Exception e2)
            {
                metodo.Append(", e2.Message: ");
                metodo.Append(e2.Message);
                Log.Error(metodo.ToString());
                throw;
            }
        }


        /// <summary>
        /// Crea el contenido de la hoja Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="excelHoja"></param>
        /// <param name="mostrarLogoyTitulo"></param>
        private static void EscribirHoja(ExcelWorksheet ws, RerExcelHoja excelHoja, bool mostrarLogoyTitulo)
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
        /// <param name="excelHoja">RerExcelHoja</param>
        /// <param name="row">Fila donde inicia</param>
        /// <param name="col">Columna donde inicia</param>/// 
        /// <returns>Devuelve la última fila</returns>
        private static int EscribirLogoyTitulos(ExcelWorksheet ws, RerExcelHoja excelHoja, int row, int col)
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
                ws.Cells[row, col].Value = excelHoja.Titulo; row++;
                ws.Cells[row, col].Value = excelHoja.Subtitulo1; 
                if (!string.IsNullOrWhiteSpace(excelHoja.Subtitulo2))
                {
                    row++;
                    ws.Cells[row, col].Value = excelHoja.Subtitulo2;
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
        /// <param name="listaModelos">Array de una lista de objeto: RerExcelModelo</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila donde inicia el pintado del modelo</param>
        /// <param name="col">Columna donde inicia el pintado del modelo</param>
        /// <param name="numColumnas">Mayor número de columna de la cabecera</param>
        /// <returns>Devuelve el número de filas agregadas</returns>
        private static int EscribirCabeceraoPie(List<RerExcelModelo>[] listaModelos, ExcelWorksheet ws, int row, int col, out int numColumnas)
        {
            int rowUltimo = row;
            int colInicio = col;
            numColumnas = 0;
            bool esListaModelosMayoraCero = (listaModelos != null && listaModelos.Length > 0);

            if (esListaModelosMayoraCero)
            {
                Dictionary<int, List<int>> colUsadasPorFila = new Dictionary<int, List<int>>();
                foreach (List<RerExcelModelo> listaModelo in listaModelos)
                {
                    bool esListaModeloMayoraCero = (listaModelo != null && listaModelo.Count > 0);
                    int numeroColumnasModelo = 0;

                    if (esListaModeloMayoraCero)
                    {
                        col = colInicio;
                        int mayorNumLineasTexto = 1; //Mayor número de líneas de texto
                        foreach (RerExcelModelo modelo in listaModelo)
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
                                AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col + (modelo.NumColumnas - 1), true, true, ConstantesPrimasRER.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesPrimasRER.SumaColumnas);
                            }
                            else if (esNumColumnasMayoraUno || esNumFilasMayoraUno)
                            {
                                if (esNumColumnasMayoraUno)
                                {
                                    AplicarEstiloaRangoCelda(ws, row, col, row, col + (modelo.NumColumnas - 1), true, true, ConstantesPrimasRER.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesPrimasRER.SumaColumnas);
                                }
                                else if (esNumFilasMayoraUno)
                                {
                                    AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col, true, true, ConstantesPrimasRER.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesPrimasRER.SumaFilas);
                                }
                            }
                            else if (esNumColumnasIgualaUno || esNumFilasIgualaUno)
                            {
                                AplicarEstiloaRangoCelda(ws, row, col, row, col, false, true, ConstantesPrimasRER.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesPrimasRER.SumaFilas);
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
        public static int EscribirCuerpo(RerExcelCuerpo cuerpo, bool ocultar, ExcelWorksheet ws, int row, int col, int numColumnas)
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
                                cuerpo.ListaTipo[j] == ConstantesPrimasRER.TipoColumnaString
                                );
                            if (justDrawValue)
                            {
                                ws.Cells[row + i, col + j].Value = cell_value;
                            }
                            else
                            {
                                switch (cuerpo.ListaTipo[j])
                                {
                                    case ConstantesPrimasRER.TipoColumnaInteger:
                                        ws.Cells[row + i, col + j].Value = Int32.Parse(cell_value);
                                        break;
                                    case ConstantesPrimasRER.TipoColumnaDouble:
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
        /// <param name="cuerpo">RerExcelCuerpo</param>
        /// <param name="ocultar">Determina si oculta el cuerpo</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila inicial de rango de celda</param>
        /// <param name="col">Columna inicial de rango de celda</param>
        /// <param name="numColumnasCabecera">Número de columnas de la cabecera</param>
        private static void AplicarEstiloaRegistros(RerExcelCuerpo cuerpo, bool ocultar, ExcelWorksheet ws, int row, int col, int numColumnasCabecera)
        {
            //Estilo por Default
            int numColumnas = cuerpo.ListaRegistros[0].Count < numColumnasCabecera ? numColumnasCabecera : cuerpo.ListaRegistros[0].Count;
            AplicarEstiloaRangoCelda(ws, row, col, row + cuerpo.ListaRegistros.Count() - 1, col + numColumnas - 1, false, true, ConstantesPrimasRER.SeccionCuerpo, ExcelHorizontalAlignment.Left);

            AplicarEstiloPersonalizado(ws, row, col, cuerpo, ocultar);
        }


        /// <summary>
        /// Agregar elementos al diccionario "columnas usadas por fila", 
        /// con la finalidad de saber en que columna empezar a pintar la fila
        /// </summary>
        /// <param name="colUsadasPorFila">Dictionary</param>
        /// <param name="row">Fila inicial del modelo (cabecera o pie)</param>
        /// <param name="col">Cantidad de columnas del modelo (cabecera o pie) </param>
        /// <param name="modelo">RerExcelModelo</param>
        /// <param name="suma">determina el tipo de suma que se desea realizar (suma de columnas, suma de filas)</param>
        private static void AgregarColumnasUsadasPorFila(ref Dictionary<int, List<int>> colUsadasPorFila, int row, int col, RerExcelModelo modelo, string suma)
        {
            for (int r = row; r < (row + modelo.NumFilas); r++)
            {
                if (colUsadasPorFila.ContainsKey(r))
                {
                    if (suma == ConstantesPrimasRER.SumaColumnas)
                    {
                        for (int c = col; c < col + modelo.NumColumnas; c++)
                        {
                            colUsadasPorFila[r].Add(c);
                        }
                    }
                    else if (suma == ConstantesPrimasRER.SumaFilas)
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
        private static void AplicarEstilo(ExcelWorksheet ws, int row, int col, int colEstilo, int cantidadRegistros, List<RerExcelEstilo> listaEstilo)
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

            if (seccion == ConstantesPrimasRER.SeccionCuerpo)
            {
                fillBackgroundColor = ColorTranslator.FromHtml("#FFFFFF");
                fontColor = ColorTranslator.FromHtml("#245C86");
                colorBorder = ColorTranslator.FromHtml("#245C86");
                fontBold = false;
            }
            else if (seccion == ConstantesPrimasRER.SeccionCabeceraoPie)
            {
                fillBackgroundColor = ColorTranslator.FromHtml("#2980B9");
                fontColor = Color.White;
                colorBorder = ColorTranslator.FromHtml("#DADAD9");
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
            rangoCelda.Style.Font.Size = 10;
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
        private static void AplicarEstiloPersonalizado(ExcelWorksheet ws, int row, int col, RerExcelCuerpo cuerpo, bool ocultar)
        {
            ExcelRange rangoCelda;
            bool existeListaAlineaHorizontal = (cuerpo.ListaAlineaHorizontal != null && cuerpo.ListaAlineaHorizontal.Count > 0);
            if (existeListaAlineaHorizontal)
            {
                for (int i = 0; i < cuerpo.ListaAlineaHorizontal.Count; i++)
                {
                    rangoCelda = ws.Cells[row, col + i, row + cuerpo.ListaRegistros.Count() - 1, col + i];
                    if (cuerpo.ListaAlineaHorizontal[i] == ConstantesPrimasRER.AlineaColumnaIzquierda)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesPrimasRER.AlineaColumnaDerecha)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesPrimasRER.AlineaColumnaCentro)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else if (cuerpo.ListaAlineaHorizontal[i] == ConstantesPrimasRER.AlineaColumnaJustificada)
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
        /// <param name="excelHoja">RerExcelHoja</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="rowInicial">Final inicial</param>
        /// <param name="rowFinal">Final final</param>
        /// <param name="col">Columna donde inicia la aplicación del ancho de columna</param>
        private static void AplicarAnchoColumna(RerExcelHoja excelHoja, ExcelWorksheet ws, int rowInicial, int rowFinal, int col)
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
        public static void SetearEstiloCelda(ExcelRange rango, RerExcelEstilo excelEstilo)
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
    
    }
}
