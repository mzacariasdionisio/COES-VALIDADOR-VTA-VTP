using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Helper
{
    public class ToolsCostoOport
    {

        public static string[][] InicializaMatriz(int rowsHead, int nFil, int nCol)
        {

            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil; i++)
            {
                matriz[i + rowsHead] = new string[nCol];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i + rowsHead][j] = string.Empty;
                }
            }
            return matriz;

        }

        public static short[][] InicializaMatrizColor(int rowsHead, int nFil, int nCol)
        {

            short[][] matriz = new short[nFil + rowsHead][];
            for (int i = 0; i < nFil + rowsHead; i++)
            {
                matriz[i] = new short[nCol];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i][j] = 0;
                }
            }
            return matriz;

        }


        public static void SetColoresMatrizExcel(short[][] matriz, int xfila, int nfilas, int nCol, int value)
        {
            for (int i = xfila; i < nfilas; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i][j] = (short)value;
                }
            }

        }

        public static void CargaDatosArchivo(string[][] matriz, List<CoBandancpDTO> ListaDataArchivo, int rowsHead)
        {
            decimal? max = 0, min = 0;
            for (var i = 0; i < ListaDataArchivo.Count; i++ )
            {
                int j = 0;
                min = (ListaDataArchivo[i].Bandmin == null) ? 0 : ListaDataArchivo[i].Bandmin;
                max = (ListaDataArchivo[i].Bandmax == null) ? 0 : ListaDataArchivo[i].Bandmax;

                matriz[i + rowsHead][j++] = ListaDataArchivo[i].Gruponomb;
                matriz[i + rowsHead][j++] = ((decimal)min).ToString("00.000");// min
                matriz[i + rowsHead][j++] = ((decimal)max).ToString("00.000");//Max
                matriz[i + rowsHead][j++] = ListaDataArchivo[i].Bandusumodificacion;
                matriz[i + rowsHead][j++] = ((DateTime)ListaDataArchivo[i].Bandfecmodificacion).ToString(Constantes.FormatoFecha);
            }

        }

        public static void CargaDatosBandaNCP(string [][] data, List<CoBandancpDTO> lista, int nfilCabecera, int nCol ){

            for (int i = 0; i < lista.Count; i++)
            {
                data[i + nfilCabecera][0] = lista[i].Gruponomb;
                data[i + nfilCabecera][1] = ((decimal)lista[i].Bandmin).ToString("0.000");
                data[i + nfilCabecera][2] = ((decimal)lista[i].Bandmax).ToString("0.000");
                data[i + nfilCabecera][3] = lista[i].Bandusumodificacion;
                data[i + nfilCabecera][4] = ((DateTime)lista[i].Bandfecmodificacion).ToString(Constantes.FormatoFecha);
                data[i + nfilCabecera][5] = lista[i].Grupocodi.ToString();
            }

        }

        public static void GeneraCabecera(string [][] matriz, int ncol)
        {
           matriz[0] = new string[ncol];    
           matriz[0][0] = "Modo de Operación";                
           matriz[0][1] = "Banda Mínima";
           matriz[0][2] = "Banda Máxima";
           matriz[0][3] = "Usuario";
           matriz[0][4] = "Fecha Registro"; 
        }

        public static void GeneraCabeceraReserva(string[][] matriz, List<PrGrupoDTO> ListaModosOperacion, DateTime dfecha)
        {
            matriz[0] = new string[ListaModosOperacion.Count + 1];  
            matriz[0][0] = "FECHA - HORA";
            for (var i = 0; i < ListaModosOperacion.Count; i++)
            {
                matriz[0][i+1] = ListaModosOperacion[i].Gruponombncp;    
            }

            // imprime la fecha y hora en la primera columna
            for (var k = 1; k <= 48; k++)
            {
                matriz[k][0] = dfecha.AddMinutes(30 * (k)).ToString(Constantes.FormatoFechaHora);
            }
        }

        public static void BorrarArchivo(string path)
        {
            for (var i = 0; i < 4; i++) {

                string objARchivo = ConstantesCOportunidad.NombreArchDat + i;
                if (System.IO.File.Exists(path + objARchivo))
                {
                    try
                    {
                        System.IO.File.Delete(path + objARchivo);
                    }
                    catch (System.IO.IOException e)
                    {
                        return;
                    }
                }

            }
        }

        public static void CargaDatosMeMedicion48(string [][] data, List<MeMedicion48DTO> lista, List<PrGrupoDTO> ListaModosOperacion, DateTime dfecha, int nRowsCabecera)
        {
            for (var i= 0; i<ListaModosOperacion.Count; i++){

                var entity = lista.Find(x => x.Ptomedicodi == ListaModosOperacion[i].PtoMediCodi && x.Medifecha == dfecha);
                if (entity != null){

                    for (var j=1; j<=48; j++){

                        decimal? valor = (decimal?)entity.GetType().GetProperty("H" + j).GetValue(entity, null);
                         if (valor != null){
                             data[nRowsCabecera + j - 1][i + 1] = valor.ToString();
                            }
                    }                    
                }
            }       
        }

        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public List<CoDatoSenialDTO> LeerSenialesDesdeFormato(string file, out List<string> validaciones)
        {
            try
            {
                List<CoDatoSenialDTO> list = new List<CoDatoSenialDTO>();
                List<string> errores = new List<string>();

                int cantidad = 1000;

                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                    
                    for (int i = 2; i <= cantidad; i++)
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                        {
                            CoDatoSenialDTO item = new CoDatoSenialDTO();

                            item.Canalnomb = ws.Cells[i, 1].Value.ToString();

                            string fecha = (ws.Cells[i, 2].Value != null) ? ws.Cells[i, 2].Value.ToString() : string.Empty;
                            string valor = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                            DateTime fechaDato;
                            if(DateTime.TryParseExact(fecha, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDato))
                            {
                                item.Codasefechahora = fechaDato;
                            }
                            else
                            {
                                errores.Add("La fecha de la fila " + i + " no tiene el formato correcto.");
                            }

                            decimal valorDato = 0;

                            if (!string.IsNullOrEmpty(valor))
                            {
                                if(decimal.TryParse(valor, out valorDato))
                                {
                                    if (valorDato >= 0)
                                    {
                                        item.Codasevalor = valorDato;
                                    }
                                    else
                                    {
                                        errores.Add("El valor de la fila " + i + " debe ser mayor o igual a cero.");
                                    }
                                }
                                else
                                {
                                    errores.Add("El valor de la fila " + i + " no tiene formato correcto.");
                                }
                            }
                            else
                            {
                                errores.Add("El valor de la fila " + i + " no debe ser vacía.");
                            }


                            list.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                validaciones = errores;
                return list;
            }
            catch (Exception ex)
            {
                validaciones = new List<string>();
                throw new Exception(ex.Message, ex);
            }
        }
    }

    public class HelperDespachoDiario
    {
        /// <summary>
        /// Completa los parametros del DTO Formato
        /// </summary>
        /// <param name="formato"></param>
        public static void GetSizeFormato(COES.Dominio.DTO.Sic.MeFormatoDTO formato)
        {

            formato.FechaInicio = formato.FechaProceso;
            formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
            formato.RowPorDia = 48;
            if (formato.Formatdiaplazo == 0) //Informacion en Tiempo Real
            {
                formato.FechaPlazoIni = formato.FechaProceso;
                formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
            }
            else
            {
                formato.FechaPlazoIni = formato.FechaProceso.AddDays(-1);
                formato.FechaPlazo = formato.FechaProceso.AddDays(-1).AddMinutes(formato.Formatminplazo);
            }


        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public static List<bool> InicializaListaFilaReadOnly(int filHead, int filData, bool plazo, int horaini, int horafin)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }
            for (int i = 0; i < filData; i++)
            {
                if (plazo)
                {
                    if (horafin == 0)
                        lista.Add(false);
                    else
                    {
                        if ((i >= horaini) && (i < horafin))
                        {
                            lista.Add(false);
                        }
                        else
                            lista.Add(true);
                    }
                }
                else
                    lista.Add(true);
            }

            return lista;
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
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public static void ObtieneMatrizWebExcel(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
        {
            if (idEnvio > 0)
            {
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                            var horizon = ts.Days;
                            var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                            var row = model.FilasCabecera +
                                ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                            //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = row,
                                Col = col
                            });
                        }
                    }
                }
            }
            for (int k = 0; k < model.ListaHojaPto.Count; k++)
            {
                for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                {
                    DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

                    for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                    {
                        if (k == 0)
                        {
                            int jIni = 0;
                            if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                jIni = j - 1;
                            else
                                jIni = j;

                            model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                                // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                        }
                        if (reg != null)
                        {
                            decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            if (valor != null)
                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                        }
                    }
                }
            }

            //}
        }

        public static int ObtieneRowChange(int periodo, int resolucion, int indiceBloque, int rowPorDia, DateTime fechaCambio, DateTime fechaInicio)
        {
            int row = 0;
            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
            return row;
        }

        /// <summary>
        /// Devuelve la fecha del siguiente bloque
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public static DateTime GetNextFilaHorizonte(int periodo, int resolucion, int horizonte, DateTime fechaInicio)
        {
            DateTime resultado = DateTime.MinValue;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            resultado = fechaInicio.AddMonths(horizonte);
                            break;

                        default:
                            resultado = fechaInicio.AddDays(horizonte);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    resultado = fechaInicio.AddDays(horizonte * 7);
                    break;
                default:
                    resultado = fechaInicio.AddDays(horizonte);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre de la celda fechaa mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public static string ObtenerCeldaFecha(int periodo, int resolucion, int tipoLectura, int horizonte, int indice, DateTime fechaInicio)
        {
            string resultado = string.Empty;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            if (tipoLectura == ParametrosLectura.Ejecutado)
                                resultado = fechaInicio.Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(horizonte + 1);
                            else
                            {
                                resultado = fechaInicio.AddMonths(horizonte).Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(fechaInicio.AddMonths(horizonte).Month);
                            }
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(Constantes.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(fechaInicio, 6) + horizonte;
                    int semanaMax = COES.Base.Tools.Util.TotalSemanasEnAnho(fechaInicio.Year, 6);
                    semana = (semana > semanaMax) ? semana - semanaMax : semana;
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == ParametrosLectura.Ejecutado)
                    {

                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(Constantes.FormatoFechaHora);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Genera Archivo excel del formato y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static string GenerarFileExcelFormato(FormatoModel model)
        {
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);

                int row = 8;
                int column = 2;

                ws.Cells[row - 3, column - 1].Value = model.Formato.Formatnombre;

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
                row = ConstantesDespachoDiario.FilaExcelData;
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
        /// Busca un elemento de las lista de puntos por su nombre y devuelve el codigo del elemento
        /// </summary>
        /// <param name="nomPto"></param>
        /// <param name="ListaPtos"></param>
        /// <returns></returns>
        public static int GetCodigoCentral(string nomPto, List<PrGrupoDTO> ListaPtos)
        {
            int idPto = -1;
            foreach (var obj in ListaPtos)
            {
                //var str = (obj.Gruponombncp.Replace(" ", "")).Trim().ToUpper();
                var str = (obj.Gruponombncp).Trim().ToUpper();
                if ((obj.Gruponombncp).Trim().ToUpper() == nomPto.ToUpper())
                {
                    idPto = obj.PtoMediCodi;
                }
            }
            return idPto;
        }

    }

}