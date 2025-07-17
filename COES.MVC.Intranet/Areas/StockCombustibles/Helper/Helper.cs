using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.StockCombustibles;
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

namespace COES.MVC.Intranet.Areas.StockCombustibles.Helper
{
    public class Util
    {

        /// <summary>
        /// Obtiene la matriz con el tamaño para los datos de las centrales de stock de combustibles
        /// </summary>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        /// <returns></returns>
        public static string[][] ObtenerMatrizListaExcelData(int nfil, int ncol, int filadicional)
        {
            nfil = nfil + filadicional; // filas dicionales para el encabezado
            string[][] lista = new string[nfil][];
            for (int i = 0; i < nfil; i++)
                lista[i] = new string[ncol];
            return lista;
        }

        /// <summary>
        /// Carga Informacion de Presion de Gas en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista24"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public static string[][] ObtenerListaExcelDataPG(StockCombustibleFormatoModel model, List<MeMedicion24DTO> lista24,
            List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
        {
            int nFil = 27;
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = ""; // r.Next(10).ToString(); 
            lista[0][0] = "PUNTO DE MEDICIÓN";
            lista[1][0] = "DESCRIPCIÓN";
            lista[2][0] = "FECHA/UNIDAD";
            for (int i = 1; i < nCol; i++)
            {
                lista[0][i] = listaPto[i - 1].Equinomb; //listaPto[i - 1].Equipopadre + " " +
                switch (listaPto[i - 1].Tipoinfocodi)
                {
                    case ConstantesStockCombustibles.IdTipoInfoPresionDeGas:
                        lista[1][i] = "EN EL LADO DE ALTA PRESIÓN";
                        break;
                    case ConstantesStockCombustibles.IdTipoInfoTemperaturaAmbiente:
                        lista[1][i] = "TEMPERATURA AMBIENTE";
                        break;
                }

                lista[2][i] = "(" + listaPto[i - 1].Tipoinfoabrev + ")";
            }
            int idPto = 0;

            ///

            if (idEnvio > 0)
            {
                model.ListaCambios = new List<CeldaCambios>();
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            var find = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi);
                            if (find != null)
                            {
                                var col = PosListaPunto(model.ListaHojaPto, find.Hojaptoorden) + 1;
                                var row = model.FilasCabecera + int.Parse(cambios[i]) - 1;
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
                        }
                    }
                }
            }

            ///
            for (var z = 0; z < nCol; z++)
            {
                if (z > 0)
                {
                    idPto = listaPto[z - 1].Ptomedicodi;
                }
                for (int k = 1; k <= 24; k++)
                {
                    if (z == 0)
                    {
                        string hora = ("0" + (k - 1).ToString()).Substring(("0" + (k - 1).ToString()).Length - 2, 2) + ":00";
                        lista[2 + k][0] = fecha + " " + hora;
                    }
                    else
                    {
                        var find = lista24.Find(x => x.Ptomedicodi == idPto);
                        if (find != null)
                        {
                            decimal? valor = (decimal?)find.GetType().GetProperty("H" + k).GetValue(find, null);
                            if (valor != null)
                                lista[2 + k][z] = valor.ToString();

                        }
                    }
                }
            }

            return lista;
        }

        public static List<TipoInformacion> ObtenerListaCentrales()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "SANTA ROSA" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 4, NombreTipoInfo = "VENTANILLA" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Genera Lista Tipo Integrante para Filtros de Reporte
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion2> ObtenerListaTipoAgente()
        {
            List<TipoInformacion2> lista = new List<TipoInformacion2>();
            var elemento = new TipoInformacion2() { IdTipoInfo = 'S', NombreTipoInfo = "INTEGRANTE" };
            lista.Add(elemento);
            elemento = new TipoInformacion2() { IdTipoInfo = 'N', NombreTipoInfo = "NO INTEGRANTE" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Genera Lista Central Integrante para Filtros de Reporte
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerCentralIntegrante()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "COES" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "NO COES" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Genera Lista Estado Fisico del Combustible para Filtros de Reporte
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaEstFisCombustible(int tipoReporte)
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "LÍQUIDO (m3)" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "SÓLIDO (kg)" };
            lista.Add(elemento);
            if (tipoReporte == 2) // Consumo de combustibles
            {
                elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "GASEOSO (m3)" };
                lista.Add(elemento);
            }
            return lista;

        }

        /// <summary>
        /// Genera Listade parametros para Filtros de Reporte
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaParametros()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "PRESIÓN DE GAS (Kpa)" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "TEMPERATURA AMBIENTE (ºC)" };
            lista.Add(elemento);
            return lista;

        }

        /// <summary>
        /// Setea los parametros del DTO Formato a partir de la fecha de proceso
        /// </summary>
        /// <param name="formato"></param>
        public static void GetSizeFormato(COES.Dominio.DTO.Sic.MeFormatoDTO formato)
        {

            formato.FechaInicio = formato.FechaProceso;
            formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
            formato.RowPorDia = 24;
            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
            if (formato.Formatdiaplazo == 0)
            {
                formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
            }
            else
            {
                formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
            }


        }

        /// <summary>
        /// Carga Matriz de datos con informacion de envio por fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcelDisponibilidad(string[][] data, List<COES.Dominio.DTO.Sic.MeMedicionxintervaloDTO> medicion, int col)
        {
            int i = 1;
            data[0] = new string[col];
            data[0][0] = "Gaseoducto";
            data[0][1] = "Fecha Hora";
            //data[0][2] = "";// "Hora (HH24:MM:SS)";
            data[0][2] = "Volumen Gas (Mm3)";
            data[0][3] = "Estado";
            data[0][4] = "Observacion";

            foreach (var reg in medicion)
            {
                data[i] = new string[col];
                data[i][0] = reg.Ptomedicodi.ToString();
                data[i][1] = reg.Medintfechafin.ToString(ConstantesBase.FormatoFechaExtendido);
                //data[i][2] = "";// reg.Medintfechaini.ToString(Constantes.FormatoHora);
                data[i][2] = reg.Medinth1.ToString();
                data[i][3] = reg.Medestcodi.ToString();

                if (reg.Medintdescrip != null)
                {
                    data[i][4] = reg.Medintdescrip.ToString();
                }
                else { data[i][4] = ""; }


                i++;
            }

        }

        /// <summary>
        /// Carga Matriz de datos de Quema de Gas con informacion de envio
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcelQuemaGas(string[][] data, List<MeMedicionxintervaloDTO> medicion, int col)
        {
            int i = 1;
            data[0] = new string[col];
            data[0][0] = "Central";
            data[0][1] = "Tipo";
            data[0][2] = "Hora (HH24:MM:SS)";
            data[0][3] = "Volumen Gas (Mm3)";
            data[0][4] = "Observaciones";

            foreach (var reg in medicion)
            {
                data[i] = new string[col];
                data[i][0] = reg.Ptomedicodi.ToString();
                data[i][1] = reg.Medestcodi.ToString();
                data[i][2] = reg.Medintfechaini.ToString(Constantes.FormatoHora);
                data[i][3] = reg.Medinth1.ToString();
                data[i][4] = reg.Medintdescrip;
                i++;
            }

        }

        /// <summary>
        /// Convierte cadena numerica sin formato a cadena numerica con formato
        /// </summary>
        /// <param name="stNumero"></param>
        /// <returns></returns>
        public static string StrNumeroFormato(string stNumero)
        {
            string strNumero = string.Empty;
            decimal dato;
            decimal? numero = null;
            if (decimal.TryParse(stNumero, out dato))
                numero = dato;
            if (numero != null)
                strNumero = ((decimal)numero).ToString("0.00");
            return strNumero;
        }

        /// <summary>
        /// Genera archivo excel del formato Consumo y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcelConsumo(StockCombustibleFormatoModel model, string rutaNombreArchivo)
        {
            string fileExcel = string.Empty;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))//, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                ws.Cells[ConstantesIntranet.RowArea, 1].Value = model.Formato.Areaname;
                ws.Cells[ConstantesIntranet.RowTitulo, 1].Value = model.Formato.Formatnombre;
                ws.Cells[1, ConstantesIntranet.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ConstantesIntranet.ColFormato].Value = model.Formato.Formatcodi.ToString();

                int row = 8;
                int column = 2;
                ws.Cells[row, column - 1].Value = "Empresa";
                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column - 1].Value = "Año";
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                ws.Cells[row + 2, column - 1].Value = "Mes";
                ws.Cells[row + 2, column].Value = model.Mes;
                ws.Cells[row + 3, column - 1].Value = "Día";
                ws.Cells[row + 3, column].Value = model.Dia.ToString();

                ExcelRange rg = ws.Cells[row, 1, row + 3, 2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                var grillaBD = model.Handson.ListaExcelData;
                ///Imprimimos Centrales
                row = ConstantesIntranet.FilaExcelData;
                int totColumnas = grillaBD.Length;

                for (int i = 0; i < totColumnas; i++)
                {
                    var fila = grillaBD[i];
                    for (int j = 0; j < fila.Length; j++)
                        ws.Cells[row + i, 1 + j].Value = fila[j];

                }
                int nfilStock = model.Formato.ListaHoja[1].ListaPtos.Count() / 2;
                int nfilConsumo = model.Formato.ListaHoja[0].ListaPtos.Count();

                rg = ws.Cells[row, 1, row, 6];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                rg = ws.Cells[row + 1, 1, row + 1, 6];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87CEEB"));
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);


                rg = ws.Cells[row + 2, 1, row + 3, 6];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Color.SetColor(Color.Black);

                rg = ws.Cells[row + 2, 1, row + 2, 3];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#B0E0E6"));
                rg.Style.Font.Bold = true;

                rg = ws.Cells[row + 2, 4, row + 2, 5];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(Color.White);

                rg = ws.Cells[row + 2, 6, row + 2, 6];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1E90FF"));
                rg.Style.Font.Color.SetColor(Color.White);



                for (int i = 0; i < nfilConsumo; i++)
                {
                    ws.Cells[i + row + 2, 1].StyleID = ws.Cells[row + 2, 1].StyleID;
                    ws.Cells[i + row + 2, 2].StyleID = ws.Cells[row + 2, 1].StyleID;
                    ws.Cells[i + row + 2, 3].StyleID = ws.Cells[row + 2, 1].StyleID;
                    ws.Cells[i + row + 2, 4].StyleID = ws.Cells[row + 2, 4].StyleID;
                    ws.Cells[i + row + 2, 5].StyleID = ws.Cells[row + 2, 5].StyleID;
                    ws.Cells[i + row + 2, 5].Style.Numberformat.Format = @"0.000";
                    ws.Cells[i + row + 2, 6].StyleID = ws.Cells[row + 2, 6].StyleID;
                    if (ws.Cells[i + row + 2, 6].Value != null)
                        ws.Cells[i + row + 2, 6].Formula = ws.Cells[i + row + 2, 6].Value.ToString();
                    ws.Cells[i + row + 2, 6].Calculate();
                    ws.Cells[i + row + 2, 6].Style.Numberformat.Format = @"0.000";
                }

                if (nfilStock > 0)
                {
                    ws.Cells[nfilConsumo + 1 + row + 2, 1].StyleID = ws.Cells[row, 1].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 1].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 2].StyleID = ws.Cells[row + 1, 2].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 3].StyleID = ws.Cells[row + 1, 3].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 4].StyleID = ws.Cells[row + 1, 4].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 5].StyleID = ws.Cells[row + 1, 5].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 6].StyleID = ws.Cells[row + 1, 6].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 7].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[nfilConsumo + 2 + row + 2, 8].StyleID = ws.Cells[row + 1, 1].StyleID;

                    for (int i = 0; i < nfilStock; i++)
                    {
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 1].StyleID = ws.Cells[row + 2, 1].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 2].StyleID = ws.Cells[row + 2, 2].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 3].StyleID = ws.Cells[row + 2, 5].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 4].StyleID = ws.Cells[row + 2, 5].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 5].StyleID = ws.Cells[row + 2, 6].StyleID;
                        if (ws.Cells[nfilConsumo + 3 + i + row + 2, 5].Value != null)
                        {
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 5].Formula = ws.Cells[nfilConsumo + 3 + i + row + 2, 5].Value.ToString();
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 5].Calculate();
                        }
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 6].StyleID = ws.Cells[row + 2, 6].StyleID;
                        if (ws.Cells[nfilConsumo + 3 + i + row + 2, 6].Value != null)
                        {
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 6].Formula = ws.Cells[nfilConsumo + 3 + i + row + 2, 6].Value.ToString();
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 6].Calculate();
                        }
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 7].StyleID = ws.Cells[row + 2, 6].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 8].StyleID = ws.Cells[row + 2, 5].StyleID;
                    }

                }

                /////////////////////// Celdas Merge /////////////////////
                int totMerge = model.Handson.ListaMerge.Count();
                int k = 0;
                foreach (var reg in model.Handson.ListaMerge)
                {
                    k++;
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                rg = ws.Cells[1, 1, nfilConsumo + nfilStock + 5 + row, 8];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                //fileExcel = System.IO.Path.GetTempFileName();
                //xlPackage.SaveAs(new FileInfo(fileExcel));
                xlPackage.Save();

            }
            return;
        }

        /// <summary>
        /// Genera Archivo excel del formato Presion de Gas y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcelPresion(StockCombustibleFormatoModel model, string rutaNombreArchivo)
        {
            string fileExcel = string.Empty;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);

                int row = 8;
                int column = 2;
                ws.Cells[row, column - 1].Value = "Empresa";
                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column - 1].Value = "Año";
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                ws.Cells[row + 2, column - 1].Value = "Mes";
                ws.Cells[row + 2, column].Value = model.Mes;
                ws.Cells[row + 3, column - 1].Value = "Día";
                ws.Cells[row + 3, column].Value = model.Dia.ToString();

                ExcelRange rg = ws.Cells[row, 1, row + 3, 2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                ///Imprimimos cabecera de puntos de medicion
                row = ConstantesIntranet.FilaExcelData;
                int totColumnas = model.ListaHojaPto.Count;

                for (var i = 0; i <= totColumnas; i++)
                {
                    for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                        if (canConvert)
                            ws.Cells[row + j, i + 1].Value = valor;
                        else
                            ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                        {
                            ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////

                using (var range = ws.Cells[row, 1, row + model.Formato.Formatrows - 1, 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87CEEB"));
                    range.Style.Font.Color.SetColor(Color.White);
                }


                using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }

                rg = ws.Cells[1, 1, 50, 10 + 1];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                xlPackage.Save();
            }
            return;
        }

        /// <summary>
        /// Genera Archivo excel del formato Disponibilidad de Gas y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcelDisponibilidad(StockCombustibleFormatoModel model, string rutaNombreArchivo)
        {
            string fileExcel = string.Empty;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))//, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                ws.Cells[ConstantesIntranet.RowArea, 1].Value = model.Formato.Areaname;
                ws.Cells[ConstantesIntranet.RowTitulo, 1].Value = model.Formato.Formatnombre;
                ws.Cells[1, ConstantesIntranet.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ConstantesIntranet.ColFormato].Value = model.Formato.Formatcodi.ToString();

                int row = 8;
                int column = 2;
                ws.Cells[row, column - 1].Value = "Empresa";
                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column - 1].Value = "Año";
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                ws.Cells[row + 2, column - 1].Value = "Mes";
                ws.Cells[row + 2, column].Value = model.Mes;
                ws.Cells[row + 3, column - 1].Value = "Día";
                ws.Cells[row + 3, column].Value = model.Dia.ToString();

                ExcelRange rg = ws.Cells[row, 1, row + 3, 2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                var grillaBD = model.Handson.ListaExcelData;
                ///Imprimimos Centrales
                row = ConstantesIntranet.FilaExcelData;
                int totColumnas = grillaBD.Length;

                for (int i = 0; i < totColumnas; i++)
                {
                    var fila = grillaBD[i];
                    for (int j = 0; j < fila.Length - 1; j++)
                    {
                        if (i == 0)
                            ws.Cells[row + i, 1 + j].Value = fila[j];
                        else
                        {
                            if (j > 0)
                                ws.Cells[row + i, 1 + j].Value = fila[j];
                            else
                                ws.Cells[row + i, 1 + j].Value = model.Formato.ListaHoja[0].ListaPtos[i - 1].Equinomb;
                        }
                    }

                }

                int nfilConsumo = model.Formato.ListaHoja[0].ListaPtos.Count() - 1;

                rg = ws.Cells[row, 1, row, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                rg = ws.Cells[row + 1, 1, row + 1, 4];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                for (int i = 0; i < nfilConsumo; i++)
                {
                    ws.Cells[i + row + 2, 1].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 2].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 3].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 4].StyleID = ws.Cells[row + 1, 4].StyleID;
                    ws.Cells[i + row + 2, 4].Style.Numberformat.Format = @"0.000";

                }
                rg = ws.Cells[1, 1, nfilConsumo + row, 8];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                //fileExcel = System.IO.Path.GetTempFileName();
                //xlPackage.SaveAs(new FileInfo(fileExcel));
                xlPackage.Save();

            }
        }

        /// <summary>
        /// Genera Archivo excel del formato Quema de Gas y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcelQuemaGas(StockCombustibleFormatoModel model, string rutaNombreArchivo)
        {
            string fileExcel = string.Empty;
            FileInfo newFile = new FileInfo(rutaNombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))//, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                ws.Cells[ConstantesIntranet.RowArea, 1].Value = model.Formato.Areaname;
                ws.Cells[ConstantesIntranet.RowTitulo, 1].Value = model.Formato.Formatnombre;
                ws.Cells[1, ConstantesIntranet.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ConstantesIntranet.ColFormato].Value = model.Formato.Formatcodi.ToString();

                int row = 8;
                int column = 2;
                ws.Cells[row, column - 1].Value = "Empresa";
                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column - 1].Value = "Año";
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                ws.Cells[row + 2, column - 1].Value = "Mes";
                ws.Cells[row + 2, column].Value = model.Mes;
                ws.Cells[row + 3, column - 1].Value = "Día";
                ws.Cells[row + 3, column].Value = model.Dia.ToString();

                ExcelRange rg = ws.Cells[row, 1, row + 3, 2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                var grillaBD = model.Handson.ListaExcelData;
                ///Imprimimos Centrales
                row = ConstantesIntranet.FilaExcelData;
                int totColumnas = grillaBD.Length;

                for (int i = 0; i < totColumnas; i++)
                {
                    var fila = grillaBD[i];
                    for (int j = 0; j < fila.Length; j++)
                    {
                        if (i == 0)
                            ws.Cells[row + i, 1 + j].Value = fila[j];
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                    ws.Cells[row + i, 1 + j].Value = model.Formato.ListaHoja[0].ListaPtos[i - 1].Equinomb;
                                    break;
                                case 1:
                                    ws.Cells[row + i, 1 + j].Value = model.Handson.ListaSourceDropDown[0][int.Parse(fila[j])];
                                    break;
                                default:
                                    ws.Cells[row + i, 1 + j].Value = fila[j];
                                    break;
                            }

                        }
                    }

                }

                int nfilConsumo = model.Formato.ListaHoja[0].ListaPtos.Count() - 1;

                rg = ws.Cells[row, 1, row, 5];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                rg = ws.Cells[row + 1, 1, row + 1, 5];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                for (int i = 0; i < nfilConsumo; i++)
                {
                    ws.Cells[i + row + 2, 1].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 2].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 3].StyleID = ws.Cells[row + 1, 1].StyleID;
                    ws.Cells[i + row + 2, 4].StyleID = ws.Cells[row + 1, 4].StyleID;
                    ws.Cells[i + row + 2, 5].StyleID = ws.Cells[row + 1, 5].StyleID;
                    ws.Cells[i + row + 2, 4].Style.Numberformat.Format = @"0.000";

                }
                rg = ws.Cells[1, 1, nfilConsumo + row, 8];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                //fileExcel = System.IO.Path.GetTempFileName();
                //xlPackage.SaveAs(new FileInfo(fileExcel));
                xlPackage.Save();

            }
        }
        /// <summary>
        /// Ubica la posicion de una columna en el grid
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public static int PosListaPunto(List<MeHojaptomedDTO> lista, int orden)
        {
            int pos = -1;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Hojaptoorden == orden)
                    pos = i;
            }
            return pos;
        }

    }
}
