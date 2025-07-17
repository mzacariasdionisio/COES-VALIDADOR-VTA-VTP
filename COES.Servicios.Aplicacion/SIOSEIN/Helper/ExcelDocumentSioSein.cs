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
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SIOSEIN;

namespace COES.Servicios.Aplicacion.SIOSEIN.Util
{
    public static class ExcelDocumentSioSein
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExcelDocumentSioSein));


        /// <summary>
        /// Genera el reporte en la ruta especificada
        /// </summary>
        /// <param name="ListaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="rutaArchivo">Ruta y nombre del archivo donde se generará el reporte</param>
        public static void ExportarReporte(List<SioExcelHoja> ListaExcelHoja, string rutaArchivo)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append("ExcelDocumentSioSein.ExportarReporte(List<SioExcelHoja> ListaExcelHoja, string rutaArchivo) - ListaExcelHoja = ");
            metodo.Append(ListaExcelHoja);
            metodo.Append(", rutaArchivo = ");
            metodo.Append(rutaArchivo);

            try
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
                    foreach (SioExcelHoja excelHoja in ListaExcelHoja)
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(excelHoja.NombreHoja);
                        if (ws != null)
                        {
                            int row = 2;
                            int col = 2;
                            ExcelRange rangoCelda;

                            //Titulo de la tabla
                            //------------------------------------------------------------------------------------------------
                            EscribirLogoyTitulos(ws, excelHoja, row, (col + 1));

                            //Cabecera de la tabla
                            //------------------------------------------------------------------------------------------------
                            row += 3;
                            int rowInicial = row;
                            row = EscribirCabeceraoPie(excelHoja.ListaCabeceras, ws, row);

                            //Cuerpo de la tabla
                            //------------------------------------------------------------------------------------------------
                            bool esListaRegistroMayoraCero = (excelHoja.Cuerpo != null && excelHoja.Cuerpo.ListaRegistros != null && excelHoja.Cuerpo.ListaRegistros.Length > 0);

                            if (esListaRegistroMayoraCero)
                            {
                                rangoCelda = ws.Cells[row, col + 1, row + 1, excelHoja.Cuerpo.ListaRegistros[0].Count()];
                                rangoCelda.Style.Font.Size = 16;
                                rangoCelda.Style.Font.Bold = true;

                                for (int i = 0; i < excelHoja.Cuerpo.ListaRegistros.Count(); i++)//Controla el llenado vertical
                                {
                                    for (int j = 0; j < excelHoja.Cuerpo.ListaRegistros[0].Count(); j++)//Controla el llenado horizontal
                                    {
                                        string cell_value = WebUtility.HtmlDecode(excelHoja.Cuerpo.ListaRegistros[i][j]);
                                        try
                                        {
                                            if (excelHoja.Cuerpo.ListaTipo[j] != null)
                                            {
                                                if (excelHoja.Cuerpo.ListaTipo[j] == ConstantesSioSein.TipoColumnaString)
                                                {
                                                    ws.Cells[row + i, col + j].Value = cell_value;
                                                }
                                                else if (excelHoja.Cuerpo.ListaTipo[j] == ConstantesSioSein.TipoColumnaInteger)
                                                {
                                                    ws.Cells[row + i, col + j].Value = Int32.Parse(cell_value);
                                                }
                                                else if (excelHoja.Cuerpo.ListaTipo[j] == ConstantesSioSein.TipoColumnaDouble)
                                                {
                                                    ws.Cells[row + i, col + j].Value = Double.Parse(cell_value);
                                                }
                                                else
                                                {
                                                    ws.Cells[row + i, col + j].Value = cell_value;
                                                }
                                            }
                                            else
                                            {
                                                ws.Cells[row + i, col + j].Value = cell_value;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            ws.Cells[row + i, col + j].Value = cell_value;
                                        }
                                    }

                                    AplicarEstiloaRangoCelda(ws, row + i, col, row + i, excelHoja.Cuerpo.ListaRegistros[0].Count() + 1, false, true, ConstantesSioSein.SeccionCuerpo, ExcelHorizontalAlignment.Left);
                                }

                                AplicarEstiloaRegistros(excelHoja, ws, row);
                                row += excelHoja.Cuerpo.ListaRegistros.Count();
                            }

                            //Pie de la tabla
                            //------------------------------------------------------------------------------------------------
                            row = EscribirCabeceraoPie(excelHoja.ListaPies, ws, row);
                            int rowFinal = row;

                            //Ancho de columnas
                            //------------------------------------------------------------------------------------------------
                            AplicarAnchoColumna(excelHoja, ws, rowInicial, rowFinal, col);
                        }
                    }

                    xlPackage.Save();
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
        /// Escribe el modelo según el caso: cabecera o pie, en el archivo excel.
        /// Nota: En caso el parámetro excelHoja sea nulo, no tomará en cuenta los parámetros row y col para colocar los títulos
        /// Si no, centrará la imagen del logo
        /// </summary>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="excelHoja">SioExcelHoja</param>
        /// <param name="row">Fila donde inicia</param>
        /// <param name="col">Columna donde inicia</param>/// 
        /// <returns>Devuelve el número de filas agregadas</returns>
        private static void EscribirLogoyTitulos(ExcelWorksheet ws, SioExcelHoja excelHoja, int row, int col)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);

            if (excelHoja != null)
            {
                picture.SetPosition(20, 60);
                picture.SetSize(135, 45);
                ws.Cells[row, col].Value = excelHoja.Titulo;
                ws.Cells[row + 1, col].Value = excelHoja.Subtitulo1;
            }
            else 
            {
                picture.SetPosition(20, 250);
                picture.SetSize(180, 60);
            }
        }


        /// <summary>
        /// Escribe el modelo según el caso: cabecera o pie, en el archivo excel
        /// </summary>
        /// <param name="listaModelos">Array de una lista de objeto: SioExcelModelo</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila donde inicia el pintado del modelo</param>
        /// <returns>Devuelve el número de filas agregadas</returns>
        private static int EscribirCabeceraoPie(List<SioExcelModelo>[] listaModelos, ExcelWorksheet ws, int row)
        {
            int rowUltimo = row;
            bool esListaModelosMayoraCero = (listaModelos != null && listaModelos.Length > 0);

            if (esListaModelosMayoraCero)
            {
                Dictionary<int, List<int>> colUsadasPorFila = new Dictionary<int, List<int>>();
                foreach (List<SioExcelModelo> listaModelo in listaModelos)
                {
                    bool esListaModeloMayoraCero = (listaModelo != null && listaModelo.Count > 0);
                    
                    if (esListaModeloMayoraCero)
                    {
                        int col = 2;    //Columna de inicio de la tabla
                        int mayorNumLineasTexto = 1; //Mayor número de líneas de texto
                        foreach (SioExcelModelo modelo in listaModelo)
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
                            //

                            bool esNumColumnasMayoraUno = (modelo.NumColumnas > 1);
                            bool esNumFilasMayoraUno = (modelo.NumFilas > 1);
                            bool esNumColumnasIgualaUno = (modelo.NumColumnas == 1);
                            bool esNumFilasIgualaUno = (modelo.NumFilas == 1);

                            if (esNumColumnasMayoraUno && esNumFilasMayoraUno)
                            {
                                //alturaFila = ObtenerMayorAltoFila(ws, row, col, row + (modelo.NumFilas - 1), col + (modelo.NumColumnas - 1), alturaFila);
                                AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col + (modelo.NumColumnas - 1), true, true, ConstantesSioSein.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesSioSein.SumaColumnas);
                            }
                            else if (esNumColumnasMayoraUno || esNumFilasMayoraUno)
                            {
                                if (esNumColumnasMayoraUno)
                                {
                                    //alturaFila = ObtenerMayorAltoFila(ws, row, col, row, col + (modelo.NumColumnas - 1), alturaFila);
                                    AplicarEstiloaRangoCelda(ws, row, col, row, col + (modelo.NumColumnas - 1), true, true, ConstantesSioSein.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesSioSein.SumaColumnas);
                                }
                                else if (esNumFilasMayoraUno)
                                {
                                    //alturaFila = ObtenerMayorAltoFila(ws, row, col, row + (modelo.NumFilas - 1), col, alturaFila);
                                    AplicarEstiloaRangoCelda(ws, row, col, row + (modelo.NumFilas - 1), col, true, true, ConstantesSioSein.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                    AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesSioSein.SumaFilas);
                                }
                            }
                            else if (esNumColumnasIgualaUno || esNumFilasIgualaUno)
                            {
                                //alturaFila = ObtenerMayorAltoFila(ws, row, col, row, col, alturaFila);
                                AplicarEstiloaRangoCelda(ws, row, col, row, col, false, true, ConstantesSioSein.SeccionCabeceraoPie, ObtenerAlineaHorizontal(modelo.AlineaHorizontal));
                                AgregarColumnasUsadasPorFila(ref colUsadasPorFila, row, col, modelo, ConstantesSioSein.SumaFilas);
                            }
                            col += modelo.NumColumnas;

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
                }
                return rowUltimo;
            }
            else 
            {
                return rowUltimo;
            }
        }


        /// <summary>
        /// Agregar elementos al diccionario "columnas usadas por fila", 
        /// con la finalidad de saber en que columna empezar a pintar la fila
        /// </summary>
        /// <param name="colUsadasPorFila">Dictionary</param>
        /// <param name="row">Fila inicial del modelo (cabecera o pie)</param>
        /// <param name="col">Cantidad de columnas del modelo (cabecera o pie) </param>
        /// <param name="modelo">SioExcelModelo</param>
        /// <param name="suma">determina el tipo de suma que se desea realizar (suma de columnas, suma de filas)</param>
        private static void AgregarColumnasUsadasPorFila(ref Dictionary<int, List<int>> colUsadasPorFila, int row, int col, SioExcelModelo modelo, string suma)
        {
            for (int r = row; r < (row + modelo.NumFilas); r++)
            {
                if (colUsadasPorFila.ContainsKey(r))
                {
                    if (suma == ConstantesSioSein.SumaColumnas)
                    {
                        for (int c = col; c < col + modelo.NumColumnas; c++)
                        {
                            colUsadasPorFila[r].Add(c);
                        }
                    }
                    else if (suma == ConstantesSioSein.SumaFilas)
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
        /// Aplica estilo a un rango de celdas
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
            ExcelRange rangoCelda;
            ws.Cells[rowInicial, colInicial, rowFinal, colFinal].Merge = merge;
            rangoCelda = ws.Cells[rowInicial, colInicial, rowFinal, colFinal];
            rangoCelda = ObtenerEstiloCelda(rangoCelda, seccion);
            rangoCelda.Style.WrapText = wrapText;
            rangoCelda.Style.HorizontalAlignment = alineaHorizontal;
        }


        /// <summary>
        /// Aplica estilo a todos los registros
        /// </summary>
        /// <param name="excelHoja">SioExcelHoja</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="row">Fila inicial de rango de celda</param>
        private static void AplicarEstiloaRegistros(SioExcelHoja excelHoja, ExcelWorksheet ws, int row)
        {
            if (excelHoja.Cuerpo.ListaAlineaHorizontal != null)
            {
                ExcelRange rangoCelda;
                for (int i = 0; i < excelHoja.Cuerpo.ListaAlineaHorizontal.Count; i++)
                {
                    rangoCelda = ws.Cells[row, i + 2, row + excelHoja.Cuerpo.ListaRegistros.Count(), i + 2];
                    if (excelHoja.Cuerpo.ListaAlineaHorizontal[i] == ConstantesSioSein.AlineaColumnaIzquierda)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else if (excelHoja.Cuerpo.ListaAlineaHorizontal[i] == ConstantesSioSein.AlineaColumnaDerecha)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if (excelHoja.Cuerpo.ListaAlineaHorizontal[i] == ConstantesSioSein.AlineaColumnaCentro)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else if (excelHoja.Cuerpo.ListaAlineaHorizontal[i] == ConstantesSioSein.AlineaColumnaJustificada)
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                    }
                    else
                    {
                        rangoCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                }
            }
        }


        /// <summary>
        /// Aplicar el ancho de columnas en el archivo excel
        /// </summary>
        /// <param name="excelHoja">SioExcelHoja</param>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="rowInicial">Final inicial</param>
        /// <param name="rowFinal">Final final</param>
        /// <param name="col">Columna donde inicia la aplicación del ancho de columna</param>
        private static void AplicarAnchoColumna(SioExcelHoja excelHoja, ExcelWorksheet ws, int rowInicial, int rowFinal, int col)
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
        /// Asigna estilo a la celda
        /// </summary>
        /// <param name="rango">Rango de la celda</param>
        /// <param name="seccion">Aplica un contenido a la celda según la sección, donde: 0 = Cuerpo, 1 = Cabecera o Pie</param>
        /// <returns>Devuelve el rango de la celda con el contenido asignado</returns>
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == ConstantesSioSein.SeccionCuerpo)
            {
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
            else if (seccion == ConstantesSioSein.SeccionCabeceraoPie)
            {
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


        /// <summary>
        /// Asigna un estilo a la celda en base al tipo de información
        /// </summary>
        /// <param name="rango">Rango de la celda</param>
        /// <param name="tipoInfo">Pinta la celda según el tipo de información</param>
        /// <returns>Devuelve el rango de la celda con el contenido asignado</returns>
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

        /// <summary>
        /// Genera el reporte con respecto al log de errores en la ruta especificada
        /// </summary>
        /// <param name="rutaNombreArchivo">Ruta y nombre del archivo donde se generará el reporte</param>
        /// <param name="listaLogErrores">Contenido del archivo a exportar</param>
        /// <param name="usuario"></param>
        public static void ExportarLogErrores(string rutaNombreArchivo, List<IioLogRemisionDTO> listaLogErrores, string usuario, DateTime fecha)
        {
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Se agrega una hoja al archivo excel
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Log de errores");

                //Definir el ancho de las columnas antes de colocar el logo
                ws.Column(1).Width = 10;
                ws.Column(2).Width = 100;

                EscribirLogoyTitulos(ws, null, 0, 0);

                //Armando tabla de identificador de usuario y fecha de modificación
                ws.Cells[6, 1].Value = "LOG DE ERRORES";

                ExcelRange rg = ws.Cells[6, 1, 6, 2];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 1].Value = "Usuario: " + usuario;
                ws.Cells[8, 1].Value = "Fecha y Hora: " + fecha.ToString(ConstantesAppServicio.FormatoFechaFull2);

                ExcelRange rg1 = ws.Cells[7, 1, 7, 2];
                ExcelRange rg2 = ws.Cells[8, 1, 8, 2];
                rg1.Merge = true;
                rg2.Merge = true;

                //Armando tabla de lista de errores
                int index = 10;

                ws.Cells[index, 1].Value = "Linea";
                ws.Cells[index, 2].Value = "Descripción del error";

                rg = ws.Cells[index, 1, index, 2];
                rg = ObtenerEstiloCelda(rg, 1);
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                index++;
                foreach (IioLogRemisionDTO item in listaLogErrores)
                {
                    ws.Cells[index, 1].Value = item.RlogNroLinea;
                    ws.Cells[index, 2].Value = item.RlogDescripcionError;
                    rg = ws.Cells[index, 1, index, 2];
                    rg = ObtenerEstiloCelda(rg, 0);
                    index++;
                }

                

                xlPackage.Save();
            }
        }
    }
}
