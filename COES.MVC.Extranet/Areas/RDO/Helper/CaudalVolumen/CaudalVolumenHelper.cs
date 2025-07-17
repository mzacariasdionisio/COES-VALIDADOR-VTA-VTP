using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using COES.MVC.Extranet.ServiceReferenceMail;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.RDO.Helper;

namespace COES.MVC.Extranet.Areas.RDO.Helper
{
    public class CaudalVolumenHelper
    {
        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        ///         
        private static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
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

        /// <summary>
        /// Genera Archivo Excel en servidor del formato solicitado
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcel(FormatoModel model, string ruta, string horario)
        {
            string rutaLogo = ruta + ConstantesCaudalVolumen.NombreArchivoLogo;
            FileInfo newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + model.Formato.Formatnombre + ".xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaFormatoExcel];
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                ws = xlPackage.Workbook.Worksheets[Constantes.HojaFormatoExcel];
                AddImage(ws, 0, 0, rutaLogo);
                //Escribe  Nombre Area
                ws.Cells[3, 1].Value = model.Formato.Areaname;
                ws.Cells[5, 1].Value = model.Formato.Formatnombre;
                //Imprimimos Codigo Empresa y Codigo Formato
                ws.Cells[1, ParametrosFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParametrosFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 6;
                int column = 2;
                ws.Cells[row, 1].Value = "Empresa";
                ws.Cells[row + 1, 1].Value = "Año";

                using (var range = ws.Cells[row, 1, row + 4, 2])
                {
                    range.Style.Border.Bottom.Style = range.Style.Border.Left.Style = range.Style.Border.Right.Style = range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }

                ws.Cells[row, column].Value = model.Empresa;
                ws.Cells[row + 1, column].Value = model.Anho.ToString();
                switch (model.Formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        ws.Cells[row + 3, column - 1].Value = "Día";
                        ws.Cells[row + 3, column].Value = model.Dia.ToString();
                        row = row + 3;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 4, column - 1].Value = "Caudal";
                            ws.Cells[row + 4, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        ws.Cells[row + 2, column].Value = model.Semana.ToString();
                        row = row + 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case 3:
                    case 5:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = model.Mes;
                        row += 2;
                        if (model.ListaHojaPto[0].Famcodi == 43)
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;

                }

                ///Imprimimos cabecera de puntos de medicion
                row += 4;
                row = ParametrosFormato.FilaExcelData;
                int totColumnas = model.ListaHojaPto.Count;

                for (var i = 0; i <= model.ListaHojaPto.Count + 1; i++)
                {
                    for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = true;
                        
                        if (i <= model.ListaHojaPto.Count)
                            canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);

                        if (canConvert)
                        {
                            if(j == 0)
                                ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i-1];                               
                            else if(j == 1)
                                ws.Cells[row + j, i + 1].Value = "ESTADO";
                            else if (j == 2)
                                ws.Cells[row + j, i + 1].Value = "E/P";    
                            else if (j > 2)
                            {
                                if(i == model.ListaHojaPto.Count + 1)
                                {
                                    //string hora = model.Handson.ListaExcelData[j][0];
                                    //string hora2 = hora.Substring(11,2);
                                    if (Convert.ToInt32(model.Handson.ListaExcelData[j][0].Substring(11, 2)) <= Convert.ToInt32(horario) && model.Handson.ListaExcelData[j][0].Substring(11, 5) != horario + ":30" && j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1)
                                        ws.Cells[row + j, i + 1].Value = "E";
                                    else
                                        ws.Cells[row + j, i + 1].Value = "P";
                                }
                                else
                                    ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];
                            }
                           
                        }
                            
                        else
                            ws.Cells[row + j, i + 1].Value = model.Handson.ListaExcelData[j][i];

                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                        {
                            ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
                using (var range = ws.Cells[row, 2, row + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                }
                ////////////// Formato de Celdas Valores

                using (var range = ws.Cells[row + model.Formato.Formatrows, 2, row + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, model.ListaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }

                for (int x = 1; x <= model.Handson.ListaColWidth.Count; x++)
                {
                    ws.Column(x).Width = model.Handson.ListaColWidth[x - 1] / 5;
                }

                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in model.Handson.ListaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = coli + reg.colspan - 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Genera el Formato en Excel HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEnvio"></param>
        /// <param name="enPlazo"></param>
        /// <returns></returns>
        public static string GenerarFormatoHtml(FormatoModel model, int idEnvio, Boolean enPlazo)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("");
            strHtml.Append("<div id='tab-2' class='ui-tabs-panel ui-widget-content ui-corner-bottom tab-panel js-tab-contents' name='grid'>");

            strHtml.Append("    <nav class='tool-menu tool-menu--grid'>");


            if (idEnvio <= 0)
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-download-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-download'></i><p class='tool-menu__icon-label'>Formato</p></a></li>");
                if (enPlazo)
                {
                    strHtml.Append("            <li><a id='btnSelectExcel3' class='link--tool js-add-grid' href='javascript:;' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Agregar</p></a></li>");
                    strHtml.Append("            <li><a class='link--tool js-save-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-save'></i><p class='tool-menu__icon-label'>Grabar</p></a></li>");
                }
                strHtml.Append("            <li><a class='link--tool js-export-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Exportar</p></a></li>");
                if (model.ListaEnvios.Count > 0)
                    strHtml.Append("            <li><a id='" + model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi.ToString() + "'class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíos</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");

                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                strHtml.Append("            <li><a class='link--tool js-nonumero-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p id='idNonum' class='tool-menu__icon-label'>0</p></a></li>");
                strHtml.Append("        </ul>");
            }
            else
            {
                strHtml.Append("        <ul class='tool-menu__btn-list'>");
                foreach (var reg in model.ListaEnvios)
                {
                    strHtml.Append("            <li><a id='" + reg.Enviocodi.ToString() + "' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>" + reg.Enviocodi + "</p></a></li>");
                }
                strHtml.Append("            <li><a id='0' class='link--tool js-reenvio-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Envíar</p></a></li>");
                strHtml.Append("            <li><a class='link--tool js-exit-grid' href='#' original-title='ctrl/⌘-S'><i class='ploticon-copy'></i><p class='tool-menu__icon-label'>Salir</p></a></li>");
                strHtml.Append("        </ul>");
            }

            strHtml.Append("    </nav>");


            strHtml.Append("<div style='clear:both; height:20px'></div>");
            strHtml.Append("<table class='table-form-vertical'>");
            strHtml.Append("  <tr><td >" + model.Formato.Areaname + " </td>");
            strHtml.Append("  <td>" + model.Formato.Formatnombre + " </td>");
            strHtml.Append("  <td>Empresa:</td><td>" + model.Empresa + "</td>");
            strHtml.Append("  <td>Año:</td><td>" + model.Anho + "</td>");
            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td>");
                    strHtml.Append("  <td>Día:</td><td>" + model.Dia + "</td>");
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    strHtml.Append("  <td>Semana:</td><td>" + model.Semana + "</td>");
                    break;
                case ParametrosFormato.PeriodoMensual:
                case ParametrosFormato.PeriodoMensualSemana:
                    strHtml.Append("  <td>Mes:</td><td>" + model.Mes + "</td><tr>");
                    break;
            }
            strHtml.Append("</table></div>");


            return strHtml.ToString();
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public static void ObtieneMatrizWebExcel(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
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
                        model.ListaCambios.Add(new CeldaCambios()
                        {
                            Row = row,
                            Col = col
                        });
                    }
                }
                // }
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

                            model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][0] =
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
        }
        public static int ObtieneRowChange(int periodo, int resolucion, int indiceBloque, int rowPorDia, DateTime fechaCambio, DateTime fechaInicio)
        {
            int row = 0;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            row = ((fechaCambio.Year - fechaInicio.Year) * 12) + fechaCambio.Month - fechaInicio.Month;
                            break;
                        default:
                            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                            break;
                    }
                    break;
                default:
                    row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                    break;
            }

            return row;
        }

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
                    Tuple<int, int> semana_anho = COES.Framework.Base.Tools.EPDate.f_numerosemana_y_anho(fechaInicio.AddDays(horizonte * 7));
                    int semana = semana_anho.Item1;
                    int anio = semana_anho.Item2;
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    resultado = anio + " Sem:" + stSemana;
                    break;
                case ParametrosFormato.PeriodoDiario:
                    if (resolucion == ParametrosFormato.ResolucionHora)
                        resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice - 1)).ToString(Constantes.FormatoFechaHora);
                    else
                        resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice)).ToString(Constantes.FormatoFecha);
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice)).ToString(Constantes.FormatoFecha);
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice)).ToString(Constantes.FormatoFechaHora);
                    break;
            }

            return resultado;
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
        /// Carga la data recibida del excel web a una lista de Medidion96DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion96DTO> LeerExcelWeb96(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil, int checkBlanco)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            MeMedicion96DTO reg = new MeMedicion96DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 96) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion96DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Meditotal = 0;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.H1 = null;
                            else
                                reg.H1 = 0;
                        }
                    }
                    else
                    {
                        int indice = j % 96 + 1;
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                        }
                    }
                }
                lista.Add(reg);
            }

            #region Validación de Cantidad mínima de filas por cada Punto de medición

            foreach (var regPto in ptos)
            {
                MeHojaptomedDTO objPto = ptos.Find(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi);
                int numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0)
                {
                    int cantidadFilas = 0;
                    decimal? valorH;
                    var listaDataXPto = lista.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).ToList();
                    foreach (var regData in listaDataXPto)
                    {
                        for (int h = 1; h <= 96; h++)
                        {
                            valorH = (decimal?)regData.GetType().GetProperty("H" + h).GetValue(regData, null);
                            cantidadFilas += (valorH != null ? 1 : 0);
                        }
                    }

                    if (cantidadFilas < numFilasMinimo)
                    {
                        foreach (var regData in listaDataXPto)
                        {
                            for (int h = 1; h <= 96; h++)
                            {
                                regData.GetType().GetProperty("H" + h).SetValue(regData, null);
                            }
                        }
                    }
                }
            }

            #endregion

            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion48DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> LeerExcelWeb48(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil, int checkBlanco)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO reg = new MeMedicion48DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    if ((j % 48) == 0)
                    {
                        if (j != 0)
                            lista.Add(reg);
                        reg = new MeMedicion48DTO();
                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Lectcodi = idLectura;
                        reg.Meditotal = 0;
                        reg.Tipoinfocodi = (int)ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.H1 = valor;
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.H1 = null;
                            else
                                reg.H1 = 0;
                        }
                    }
                    else
                    {
                        int indice = j % 48 + 1;
                        stValor = matriz[j][i];
                        if (Util.EsNumero(stValor))
                        {
                            valor = decimal.Parse(stValor);
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                        }
                        else
                        {
                            if (checkBlanco == 0)
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                        }
                    }
                }
                lista.Add(reg);
            }

            #region Validación de Cantidad mínima de filas por cada Punto de medición

            foreach (var regPto in ptos)
            {
                MeHojaptomedDTO objPto = ptos.Find(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi);
                int numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0)
                {
                    int cantidadFilas = 0;
                    decimal? valorH;
                    var listaDataXPto = lista.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).ToList();
                    foreach (var regData in listaDataXPto)
                    {
                        for (int h = 1; h <= 48; h++)
                        {
                            valorH = (decimal?)regData.GetType().GetProperty("H" + h).GetValue(regData, null);
                            cantidadFilas += (valorH != null ? 1 : 0);
                        }
                    }

                    if (cantidadFilas < numFilasMinimo)
                    {
                        foreach (var regData in listaDataXPto)
                        {
                            for (int h = 1; h <= 48; h++)
                            {
                                regData.GetType().GetProperty("H" + h).SetValue(regData, null);
                            }
                        }
                    }
                }
            }

            #endregion

            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion24DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion24DTO> LeerExcelWeb24(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            var columnas = colHead + nCol;
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            try
            {
                for (var i = colHead; i < columnas; i++)
                {
                    for (var j = 0; j < nFil; j++)
                    {
                        //verificar inicio de dia
                        if ((j % 24) == 0)
                        {
                            if (j != 0)
                                lista.Add(reg);
                            reg = new MeMedicion24DTO();
                            reg.Ptomedicodi = ptos[i - colHead].Ptomedicodi;
                            reg.Lectcodi = idLectura;
                            reg.Tipoinfocodi = (int)ptos[i - colHead].Tipoinfocodi;
                            reg.Emprcodi = ptos[i - colHead].Emprcodi;
                            fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                            stValor = matriz[j][i];
                            if (Util.EsNumero(stValor))
                            {
                                //valor = decimal.Parse(stValor);
                                bool flag = decimal.TryParse(stValor, out valor);
                                reg.H1 = valor;
                            }
                            else
                                reg.H1 = null;
                        }
                        else
                        {
                            stValor = matriz[j][i];
                            int indice = j % 24 + 1;
                            if (Util.EsNumero(stValor))
                            {
                                //valor = decimal.Parse(stValor);
                                bool flag = decimal.TryParse(stValor, out valor);
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                            }
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);

                        }
                    }
                    lista.Add(reg);
                }
            }
            catch
            {
                var mensaje = "";
            }

            #region Validación de Cantidad mínima de filas por cada Punto de medición

            foreach (var regPto in ptos)
            {
                MeHojaptomedDTO objPto = ptos.Find(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi);
                int numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0)
                {
                    int cantidadFilas = 0;
                    decimal? valorH;
                    var listaDataXPto = lista.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).ToList();
                    foreach (var regData in listaDataXPto)
                    {
                        for (int h = 1; h <= 24; h++)
                        {
                            valorH = (decimal?)regData.GetType().GetProperty("H" + h).GetValue(regData, null);
                            cantidadFilas += (valorH != null ? 1 : 0);
                        }
                    }

                    if (cantidadFilas < numFilasMinimo)
                    {
                        foreach (var regData in listaDataXPto)
                        {
                            for (int h = 1; h <= 24; h++)
                            {
                                regData.GetType().GetProperty("H" + h).SetValue(regData, null);
                            }
                        }
                    }
                }
            }

            #endregion

            return lista;
        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion1DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion1DTO> LeerExcelWeb1(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int periodo, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            var columnas = colHead + nCol;
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
            MeMedicion1DTO reg = new MeMedicion1DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = colHead; i < columnas; i++)
            {
                for (var j = 0; j < nFil; j++)
                {
                    //verificar inicio de dia
                    reg = new MeMedicion1DTO();
                    reg.Ptomedicodi = ptos[i - colHead].Ptomedicodi;
                    reg.Lectcodi = idLectura;
                    reg.Tipoinfocodi = (int)ptos[i - colHead].Tipoinfocodi;
                    reg.Emprcodi = ptos[i - colHead].Emprcodi;
                    switch (periodo)
                    {
                        case 1:
                            if (matriz[j][0].Length > 12)
                                reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            else
                                reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            break;
                        case 2:
                            //reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            if (matriz[j][0].Length > 12)
                                reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            else
                                reg.Medifecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                            break;
                        //break;
                        case 5:
                            reg.Medifecha = EPDate.GetFechaSemana(matriz[j][0]);//DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            break;
                        case 3:
                            reg.Medifecha = EPDate.GetFechaYearMonth(matriz[j][0]);
                            break;
                    }

                    reg.Medifecha = new DateTime(reg.Medifecha.Year, reg.Medifecha.Month, reg.Medifecha.Day);
                    stValor = matriz[j][i];
                    if (Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.H1 = valor;
                    }
                    else
                        reg.H1 = null;
                    lista.Add(reg);
                }

            }

            #region Validación de Cantidad mínima de filas por cada Punto de medición

            foreach (var regPto in ptos)
            {
                MeHojaptomedDTO objPto = ptos.Find(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi);
                int numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0)
                {
                    int cantidadFilas = 0;
                    decimal? valorH;
                    var listaDataXPto = lista.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).ToList();
                    foreach (var regData in listaDataXPto)
                    {
                        for (int h = 1; h <= 1; h++)
                        {
                            valorH = (decimal?)regData.GetType().GetProperty("H" + h).GetValue(regData, null);
                            cantidadFilas += (valorH != null ? 1 : 0);
                        }
                    }

                    if (cantidadFilas < numFilasMinimo)
                    {
                        foreach (var regData in listaDataXPto)
                        {
                            for (int h = 1; h <= 1; h++)
                            {
                                regData.GetType().GetProperty("H" + h).SetValue(regData, null);
                            }
                        }
                    }
                }
            }

            #endregion

            return lista;
        }

        /// <summary>
        /// Convierte una lista de mediciones en una Matriz Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filHead"></param>
        /// <param name="nFil"></param>
        /// <returns></returns>
        private static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var colTot = nCol + colHead + 1 ;
            var inicio = (colTot) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[colTot];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == colTot)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[colTot];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        public static DateTime GetFechaFinPeriodo(DateTime fechaIni, int periodo, int horizonte)
        {
            DateTime fechaFin = DateTime.MinValue;
            switch (periodo)
            {
                case 1:
                    fechaFin = fechaIni.AddDays(horizonte - 1);
                    break;
                case 2:
                    fechaFin = fechaIni.AddDays(horizonte);
                    break;
                case 3:
                    fechaFin = fechaIni;
                    break;
                case 5:
                    int nMeses = horizonte / 30;
                    fechaFin = fechaIni.AddMonths(nMeses);
                    break;
            }
            return fechaFin;
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
        public static Boolean LeerExcelFile(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, string horario)
        {
            Boolean retorno = true;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil; i++)
                {
                    for (int j = 0; j < nCol; j++)
                    {
                        string _estado = (ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, nCol + 2].Value != null) ?
                           ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, nCol + 2].Value.ToString() : string.Empty;

                        string _hora = (ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, 1].Value != null) ?
                           ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, 1].Value.ToString().Substring(11, 2) : string.Empty;

                        string _horario = (ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, 1].Value != null) ?
                           ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, 1].Value.ToString().Substring(11, 5) : string.Empty;

                        if (_estado == "E")
                        {
                            if (Convert.ToInt32(horario) < Convert.ToInt32(_hora))
                            {
                                retorno = false;
                                break;
                                //throw new Exception("La hora registrada está fuera de rango de horario.");
                            }

                        }

                        string valor = (ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, j + colsHead + 1].Value.ToString() : string.Empty;
                        matriz[i + rowsHead][j + colsHead] = valor;
                    }
                }
            }
            return retorno;
        }

        public static Boolean LeerExcelFile2(string[][] matriz, string file, int rowsHead, int nFil, int colsHead, int nCol, List<bool> listaRead, List<MeMedicion24DTO> lista24, List<MeHojaptomedDTO> ptos)
        {
            Boolean retorno = false;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 0; i < nFil; i++)
                {
                    for (int j = 0; j < nCol; j++)
                    {
                        string valor = (ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, j + colsHead + 1].Value != null) ?
                            ws.Cells[i + rowsHead + ParametrosFormato.FilaExcelData, j + colsHead + 1].Value.ToString() : string.Empty;

                        if (listaRead[i + rowsHead] == false)
                        {
                            matriz[i + rowsHead][j + colsHead] = valor;
                        }
                        else
                        {
                            valor = "";

                            if (lista24.Count > 0)
                            {
                                foreach (var tmp in lista24)
                                {
                                    if (tmp.Ptomedicodi == ptos[j].Ptomedicodi)
                                    {
                                        valor = ((decimal?)tmp.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(tmp, null)).ToString();
                                    }
                                }


                            }
                            matriz[i + rowsHead][j + colsHead] = valor;
                        }
                    }
                }
            }
            return retorno;
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
                matriz[i] = new string[nCol + colsHead];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    matriz[i][j] = "";

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
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcel2(List<CabeceraRow> listaCabeceraRow, int rowsHead, int nFil, int colsHead, int nCol, string fecha)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            ///Cabecera
            matriz[0] = new string[nCol + colsHead];

            for (int i = 0; i < listaCabeceraRow.Count; i++)
            {
                matriz[0][i] = listaCabeceraRow[i].TituloRow;
            }

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
        /// Carga Matriz de datos con informacion de envio por fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcel2(string[][] data, List<MeMedicionxintervaloDTO> medicion, int col)
        {
            int i = 1;
            foreach (var reg in medicion)
            {

                data[i] = new string[col];
                data[i][0] = reg.Medintfechaini.ToString(Constantes.FormatoFecha);
                data[i][1] = reg.Ptomedicodi.ToString();
                data[i][2] = reg.Medintfechaini.ToString(Constantes.FormatoHora);
                data[i][3] = reg.Medintfechafin.ToString(Constantes.FormatoHora);
                data[i][4] = reg.Medinth1.ToString();
                data[i][5] = reg.Emprcodi.ToString();
                data[i][6] = string.IsNullOrEmpty(reg.Medintdescrip) ? "" : reg.Medintdescrip;
                i++;
            }

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

        public static void GetFechaActualEnvio(int periodo, int tipo, ref string mes, ref string fecha, ref int semana, ref int anho)
        {
            DateTime fechaActual = DateTime.Now;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    if (tipo == ParametrosLectura.Ejecutado)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosLectura.Programado)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosLectura.TiempoReal)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    var totalSemanasAnho = EPDate.TotalSemanasEnAnho(fechaActual.Year, 6);
                    semana = EPDate.f_numerosemana(fechaActual);
                    anho = fechaActual.Year;
                    if (tipo == ParametrosLectura.Ejecutado)
                    {

                        if (semana == 1)
                        {
                            semana = totalSemanasAnho;
                            anho = anho - 1;
                        }
                        else
                            semana--;
                    }
                    else
                    {

                        if (semana == totalSemanasAnho)
                        {
                            semana = 1;
                            anho++;
                        }
                        else
                            semana++;
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    if (tipo == ParametrosLectura.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    if (tipo == ParametrosLectura.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoAnual:
                    break;
            }
        }

        /// <summary>
        /// Borra el archivo del servidor
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
        /// Verifica Id de Empresa e ID Fromato en el archivo formato para cargar en la hoja web excel
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
                var valorEmp = ws.Cells[1, ParametrosFormato.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                var valorFormato = ws.Cells[1, ParametrosFormato.ColFormato].Value.ToString();
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
            return retorno;
        }

        public static int EnviarCorreo(string lectura, string formato, bool enPlazo, string empresa, DateTime fechaInicio, DateTime fechaFin, string areaCoes, string usuario, DateTime fechaEnvio, int idEnvio)
        {
            int resultado = 0;
            MailClient appMail = new MailClient();
            string msg = GenerarBodyMail(lectura, formato, enPlazo, empresa, fechaInicio, fechaFin, areaCoes, usuario, fechaEnvio, idEnvio);
            string toEmail = ConfigurationManager.AppSettings[ConstantesCaudalVolumen.ToMail];
            string ccEmail = string.Empty;
            if (usuario.IndexOf("@") != -1)
                ccEmail = usuario;
            appMail.EnviaCorreoFull(ConstantesCaudalVolumen.FromEmail, ConstantesCaudalVolumen.FromName, toEmail,
                ccEmail, "Notificacion de Envío de Información Caudales y Volúmenes", msg, true, "", null);
            return resultado;
        }

        public static string GenerarBodyMail(string lectura, string formato, bool enPlazo, string empresa, DateTime fechaInicio, DateTime fechaFin, string areaCoes, string usuario, DateTime fechaEnvio, int idEnvio)
        {
            StringBuilder strHtml = new StringBuilder();
            string stCumplimiento = string.Empty;
            string stCumplimientoMensaje = string.Empty;
            if (!enPlazo)
            {
                stCumplimiento = "Fuera de Plazo";
                stCumplimientoMensaje = "<strong>“Fuera de Plazo“</strong>";
            }
            else
            {
                stCumplimiento = "En Plazo";
            }
            strHtml.Append("<html>");
            strHtml.Append("    <head><STYLE TYPE='text/css'>");
            strHtml.Append("    body {font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}");
            strHtml.Append("    .mail {width:500px;border-spacing:0;border-collapse:collapse;}");
            strHtml.Append("    .mail thead th {text-align: center;background: #417AA7;color:#ffffff}");
            strHtml.Append("    .mail tr td {border:1px solid #dddddd;}");
            strHtml.Append("    </style></head>");
            strHtml.Append("    <body>");
            strHtml.Append("        <P>Estimado:</p><p>" + usuario + "</P>");
            strHtml.Append("        <P>Por medio del presente, se le comunica que se registró " + stCumplimientoMensaje + " en el portal extranet la información de Hidrología");
            strHtml.Append(" de su representada en atención a lo  dispuesto en el <strong>Procedimiento Técnico N° 41 - </strong> “ Información Hidrológica para la Operación del SEIN");
            strHtml.Append("  ” el que se detalla a seguir:");
            strHtml.Append(" <TABLE class='mail'>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Empresa:</TD><TD> " + empresa + " </TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Tipo de Lectura:</TD><TD> " + lectura + " </TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Formato:</TD><TD> " + formato + " </TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Fecha Inicio: </TD><TD>" + fechaInicio.ToString(ConstantesBase.FormatoFecha) + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Fecha Fin: </TD><TD>" + fechaFin.ToString(ConstantesBase.FormatoFecha) + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Fecha de Envío: </TD><TD>" + fechaEnvio.ToString(ConstantesBase.FormatoFechaExtendido) + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Código de Envío: </TD><TD>" + idEnvio + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Cumplimiento:</TD><TD> " + stCumplimiento + "</TD></TR>");
            strHtml.Append(" </Table>");
            strHtml.Append(" <p>Atentamente,</P><p>" + areaCoes + "</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();

        }

        /// <summary>
        /// Carga la data recibida del excel web a una lista de Medidion24DTO
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ptos"></param>
        /// <param name="idLectura"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="checkBlanco"></param>
        /// <returns></returns>
        public static List<MeMedicion24DTO> LeerExcelWeb24Ejecutados(List<string> data, List<MeHojaptomedDTO> ptos, int idLectura, int colHead, int nCol, int filaHead, int nFil)
        {
            var matriz = GetMatrizExcel(data, colHead, nCol, filaHead, nFil);
            var columnas = colHead + nCol;
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            string stEstado = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            try
            {
                for (var i = colHead; i < columnas; i++)
                {
                    for (var j = 0; j < nFil; j++)
                    {
                        //verificar inicio de dia
                        if ((j % 24) == 0)
                        {
                            if (j != 0)
                                lista.Add(reg);
                            reg = new MeMedicion24DTO();
                            reg.Ptomedicodi = ptos[i - colHead].Ptomedicodi;
                            reg.Tipoinfocodi = (int)ptos[i - colHead].Tipoinfocodi;
                            reg.Emprcodi = ptos[i - colHead].Emprcodi;                           
                            reg.Lectcodi = idLectura;
                            
                            fecha = DateTime.ParseExact(matriz[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);

                            stValor = matriz[j][i];
                            stEstado = matriz[j][columnas];
                            reg.E1 = stEstado == "true" ? "E" : "P";

                            if (Util.EsNumero(stValor))
                            {
                                bool flag = decimal.TryParse(stValor, out valor);
                                reg.H1 = valor;
                            }
                            else
                                reg.H1 = null;   
                        }
                        else
                        {
                            int indice = j % 24 + 1;
                            stValor = matriz[j][i];
                            stEstado = matriz[j][columnas];
                            reg.GetType().GetProperty("E" + indice.ToString()).SetValue(reg, stEstado == "true" ? "E" : "P");

                            if (Util.EsNumero(stValor))
                            {
                                //valor = decimal.Parse(stValor);
                                bool flag = decimal.TryParse(stValor, out valor);
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                            }
                            else
                                reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        }
                    }
                    lista.Add(reg);
                }
            }
            catch
            {
                var mensaje = "";
            }

            return lista;
        }

    }

}