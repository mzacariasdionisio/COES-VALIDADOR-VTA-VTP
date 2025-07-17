using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.RDO.Models;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.ServiceReferenceMail;
using COES.Servicios.Aplicacion.FormatoMedicion;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using COES.Servicios.Aplicacion.StockCombustibles;

namespace COES.MVC.Extranet.Areas.RDO.Helper
{
    public class Tools
    {

        /// <summary>
        /// Define la matriz que sera reflejada en el Handson para Stock y Consumo de Combustibles
        /// </summary>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        /// <returns></returns>
        public static string[][] ObtenerMatrizListaExcelData(int nfil, int ncol, int filadicional)
        {
            nfil = nfil + filadicional + 1; // filas dicionales para el encabezado

            string[][] lista = new string[nfil][];
            for (int i = 0; i < nfil; i++)
                lista[i] = new string[ncol];
            return lista;
        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public static List<bool> InicializaListaFilaReadOnly(int filHead, int filData)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }
            for (int i = 0; i < filData; i++)
            {
                lista.Add(false);
            }
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
        public static string[][] ObtenerListaExcelDataPG(DisponibilidadCombustibleModel model, List<MeMedicion24DTO> lista24, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
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

        /// <summary>
        /// Verifica id de formato en el archivo excel que se requiere cargar en el grid web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public static int VerificarIdsFormato(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                string valorEmp = string.Empty;
                if (ws.Cells[1, ConstantesDisponibilidadCombustible.ColEmpresa].Value != null)
                    valorEmp = ws.Cells[1, ConstantesDisponibilidadCombustible.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                string valorFormato = string.Empty;
                if (ws.Cells[1, ConstantesDisponibilidadCombustible.ColFormato].Value != null)
                    valorFormato = ws.Cells[1, ConstantesDisponibilidadCombustible.ColFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
                if (idEmpresaArchivo != idEmpresa)
                {
                    retorno = -1;
                }
                if (idFormatoEmpresa != idFormato)
                {
                    retorno = -2;
                }
            }
            retorno = 1;
            return retorno;
        }

        /// <summary>
        /// Borra archivo fisico
        /// </summary>
        /// <param name="archivo"></param>
        public static void BorrarArchivo(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil; i++)
            {

                matriz[i + rowsHead] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i + rowsHead][j + colsHead] = string.Empty;
                }
            }
            return matriz;
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] LeerExcelFile(string file, List<MeHojaptomedDTO> listaPto, string fecha)
        {
            FileInfo fileInfo = new FileInfo(file);
            double numero = 0;
            int nFil = 27;
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

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil - 3; i++)
                {
                    for (int j = 0; j < nCol - 1; j++)
                    {
                        string valor = (ws.Cells[i + 3 + ConstantesDisponibilidadCombustible.FilaExcelData, j + 1 + 1].Value != null) ?
                           ws.Cells[i + 3 + ConstantesDisponibilidadCombustible.FilaExcelData, j + 1 + 1].Value.ToString() : string.Empty;

                        if (esNumero(valor))
                        {
                            double.TryParse(valor, out numero);
                            valor = numero.ToString("0.###########################################");
                        }
                        lista[i + 3][j + 1] = valor;

                    }
                }
            }

            for (var i = 1; i < 25; i++) //Recorre las 24 horas del dia
            {
                string hora = ("0" + (i - 1).ToString()).Substring(("0" + (i - 1).ToString()).Length - 2, 2) + ":00";
                lista[i + 2][0] = fecha + " " + hora;

            }
            return lista;
        }


        /// <summary>
        /// Genera archivo excel del formato Consumo y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static string GenerarFileExcelConsumo(DisponibilidadCombustibleModel model)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())//, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                ws.View.ShowGridLines = false;
                ws.Cells[ParamFormatoCombustible.RowArea, 1].Value = model.Formato.Areaname;
                ws.Cells[ParamFormatoCombustible.RowTitulo, 1].Value = model.Formato.Formatnombre;
                ws.Cells[1, ParamFormatoCombustible.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParamFormatoCombustible.ColFormato].Value = model.Formato.Formatcodi.ToString();

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
                row = ConstantesDisponibilidadCombustible.FilaExcelData;
                int totColumnas = grillaBD.Length;

                for (int i = 0; i < totColumnas; i++)
                {
                    var fila = grillaBD[i];
                    for (int j = 0; j < fila.Length; j++)
                    {
                        decimal valueDecimal;
                        bool isNum = decimal.TryParse(fila[j], out valueDecimal);
                        if (isNum)
                        {
                            ws.Cells[row + i, 1 + j].Value = valueDecimal;
                        }
                        else
                        {
                            ws.Cells[row + i, 1 + j].Value = fila[j];
                        }
                    }

                }

                ws.Cells[row + totColumnas + 1, 1].Value = "NOTA: El consumo de combustible a cargar deberá corresponder a las períodos de operación en que las unidades de generación acoplaron al sistema";

                int nfilStock = model.Formato.ListaHoja[1].ListaPtos.Count() / 2;
                int nfilConsumo = model.Formato.ListaHoja[0].ListaPtos.Count();

                //CONSUMO DE COMBUSTIBLE
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
                    ws.Cells[nfilConsumo + 2 + row + 2, 9].StyleID = ws.Cells[row + 1, 1].StyleID;

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
                        if (ws.Cells[nfilConsumo + 3 + i + row + 2, 7].Value != null)
                        {
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 7].Formula = ws.Cells[nfilConsumo + 3 + i + row + 2, 7].Value.ToString();
                            ws.Cells[nfilConsumo + 3 + i + row + 2, 7].Calculate();
                        }
                        //ws.Cells[nfilConsumo + 3 + i + row + 2, 7].StyleID = ws.Cells[row + 2, 6].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 8].StyleID = ws.Cells[row + 2, 5].StyleID;
                        ws.Cells[nfilConsumo + 3 + i + row + 2, 9].StyleID = ws.Cells[row + 2, 5].StyleID;
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

                rg = ws.Cells[1, 1, nfilConsumo + nfilStock + 5 + row, 9];
                rg.AutoFitColumns();

                //HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                //picture.From.Column = 0;
                //picture.From.Row = 0;
                //picture.To.Column = 1;
                //picture.To.Row = 1;
                //picture.SetSize(120, 60);

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));

            }
            return fileExcel;
        }

        /// <summary>
        /// Genera Archivo excel del formato Presion de Gas y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static string GenerarFileExcelPresion(DisponibilidadCombustibleModel model)
        {
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
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
                row = ConstantesDisponibilidadCombustible.FilaExcelData;
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

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }
            return fileExcel;
        }

        /// <summary>
        /// Funcion que verifica si una cadena es numero(byte,entero o decimal)
        /// </summary>
        /// <param name="numString"></param>
        /// <returns></returns>
        private static Boolean esNumero(string numString)
        {
            Boolean isNumber = false;
            long number1 = 0;
            bool canConvert = long.TryParse(numString, out number1);
            if (canConvert == true)
                isNumber = true;
            else
            {
                byte number2 = 0;
                canConvert = byte.TryParse(numString, out number2);
                if (canConvert == true)
                    isNumber = true;
                else
                {
                    double number3 = 0;

                    canConvert = double.TryParse(numString, out number3);
                    if (canConvert == true)
                        isNumber = true;

                }
            }
            return isNumber;
        }

        /// <summary>
        /// Convierte cadena numerica sin formato a cadena numerica con formato
        /// </summary>
        /// <param name="stNumero"></param>
        /// <returns></returns>
        public static string StrNumeroFormato(string stNumero)
        {
            //quitar el caracter final #, el CAMENVDATOS debe terminar en un número
            stNumero = stNumero != null && stNumero.Length > 0 && stNumero[stNumero.Length - 1].ToString() == "#" ? stNumero.Substring(0, stNumero.Length - 1) : stNumero;

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
        /// Carga Matriz de datos con informacion de envio por fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcelDisponibilidad(string[][] data, List<MeMedicionxintervaloDTO> medicion, int col)
        {
            int i = 1;
            data[0] = new string[col];
            data[0][0] = "Gaseoducto";
            data[0][1] = "Fecha Hora";
            //data[0][2] = "";// "Hora (HH24:MM:SS)";
            data[0][2] = "Volumen Gas (Mm3)";
            data[0][3] = "Estado";
            data[0][4] = "Observacion";
            data[0][5] = "";
            data[0][6] = "Empresa";

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
                data[i][6] = reg.Emprcodi.ToString();
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
            data[0][6] = "Empresa";

            foreach (var reg in medicion)
            {
                data[i] = new string[col];
                data[i][0] = reg.Ptomedicodi.ToString();
                data[i][1] = reg.Medestcodi.ToString();
                data[i][2] = reg.Medintfechaini.ToString(Constantes.FormatoHora);
                data[i][3] = reg.Medinth1.ToString();
                data[i][6] = reg.Emprcodi.ToString();
                i++;
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

        /// <summary>
        /// Obtiene NOmbre columna en excel
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
