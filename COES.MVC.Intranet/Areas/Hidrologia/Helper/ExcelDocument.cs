using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System.Globalization;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Net;
using COES.Dominio.DTO.Scada;
using System.Text;
using System.Xml;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System.Data;
using System.Data.Common;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;
using DevExpress.XtraSpreadsheet.Model;
using static iTextSharp.text.pdf.AcroFields;
using static OfficeOpenXml.ExcelErrorValue;

namespace COES.MVC.Intranet.Areas.Hidrologia.Helper
{
    public class ExcelDocument
    {
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
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }

        /// <summary>
        /// Genera encabezados para reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="ruta"></param>
        public static void ConfigEncabezadoExcel(ExcelWorksheet ws, string titulo, string ruta)
        {
            AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 3, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "Fecha:";
            ws.Cells[3, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);

        }

        /// <summary>
        /// Genera encabezados para reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="ruta"></param>
        public static void ConfiguraEncabezadoHojaExcel(ExcelWorksheet ws, string titulo, string fInicio, string fFin, string ruta)
        {
            AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            //Borde, font cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 3, 5].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 3, 5].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "Fecha Inicio:";
            ws.Cells[3, 4].Value = "Fecha Fin:";
            ws.Cells[3, 3].Value = fInicio;
            ws.Cells[3, 5].Value = fFin;
        }

        /// <summary>
        /// Genera encabezados para reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoRpte"></param>
        /// <param name="ruta"></param>
        public static void ConfiguraEncabezadoHojaExcel2(ExcelWorksheet ws, string titulo, string fechaIni, string fechaFin, int tipoRpte, string ruta)
        {
            AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);
            ws.Cells[1, 4].Value = titulo;
            var font = ws.Cells[1, 4].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            if (tipoRpte == 0) // horas
            {
                ws.Cells[3, 2].Value = "FECHA INICIO:";
                ws.Cells[3, 3].Value = fechaIni;
                ws.Cells[4, 2].Value = "FECHA FIN:";
                ws.Cells[4, 3].Value = fechaFin;

            }
            else
            {
                var borderFecha2 = ws.Cells[4, 2, 4, 3].Style.Border;
                borderFecha2.Bottom.Style = borderFecha2.Top.Style = borderFecha2.Left.Style = borderFecha2.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla2 = ws.Cells[4, 2, 4, 3].Style.Font;
                fontTabla2.Size = 8;
                fontTabla2.Name = "Calibri";
                fontTabla2.Bold = true;
                if (tipoRpte == 1) // dias
                {
                    ws.Cells[3, 2].Value = "FECHA INICIO:";
                    ws.Cells[3, 3].Value = fechaIni;
                    ws.Cells[4, 2].Value = "FECHA FIN:";
                    ws.Cells[4, 3].Value = fechaFin;
                }
                if (tipoRpte == 2 || tipoRpte == 3) // Semanal programado/cronológico
                {
                    ws.Cells[3, 2].Value = "SEMANA INICIO:";
                    ws.Cells[4, 2].Value = "SEMANA FIN:";
                    if (tipoRpte == 2) // sem programado
                    {
                        ws.Cells[3, 3].Value = "SEM - " +
                                            COES.Base.Tools.Util.GenerarNroSemana(DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture), FirstDayOfWeek.Saturday)
                                            + " - " + DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;

                        ws.Cells[4, 3].Value = "SEM - " +
                                            COES.Base.Tools.Util.GenerarNroSemana(DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture), FirstDayOfWeek.Saturday)
                                            + " - " + DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                    }
                    else
                    {
                        ws.Cells[3, 3].Value = "SEM - " +
                                            COES.Base.Tools.Util.GenerarNroSemana(DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture), FirstDayOfWeek.Sunday)
                                            + " - " + DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;

                        ws.Cells[4, 3].Value = "SEM - " +
                                            COES.Base.Tools.Util.GenerarNroSemana(DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture), FirstDayOfWeek.Sunday)
                                             + " - " + DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                    }
                }
                if (tipoRpte == 4) // Mensual
                {
                    ws.Cells[3, 2].Value = "MES INICIO:";
                    ws.Cells[3, 3].Value = COES.Base.Tools.Util.ObtenerNombreMesAbrev(DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Month) + " - " +
                                            DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                    ws.Cells[4, 2].Value = "MES FIN:";
                    ws.Cells[4, 3].Value = COES.Base.Tools.Util.ObtenerNombreMesAbrev(DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Month) + " - " +
                                            DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                }
                if (tipoRpte == 5) // Anual
                {
                    ws.Cells[3, 2].Value = "AÑO INICIO:";
                    ws.Cells[3, 3].Value = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                    ws.Cells[4, 2].Value = "AÑO FIN:";
                    ws.Cells[4, 3].Value = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture).Year;
                }

            }

        }

        /// <summary>
        /// Genera grafico tipo linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        public static void AddGraficoLineas(ExcelWorksheet ws, int row, int col, string xAxisTitle, string yAxisTitle, string titulo)
        {
            var LineaChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.Line) as ExcelLineChart;
            //Set top left corner to row 1 column 2
            LineaChart.SetPosition(5, 0, col + 3, 0);
            LineaChart.SetSize(1200, 600);

            for (int i = 0; i < col; i++)
            {

                var ran1 = ws.Cells[7, 3 + i, row + 6, 3 + i];
                var ran2 = ws.Cells[7, 2, row + 6, 2];

                var serie = (ExcelChartSerie)LineaChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[6, 3 + i].Value.ToString();
            }
            LineaChart.Title.Text = titulo;
            //LineaChart.DataLabel.ShowSeriesName = true;
            //LineaChart.DataLabel.ShowLegendKey = true;
            LineaChart.DataLabel.ShowLeaderLines = true;
            //LineaChart.DataLabel.ShowCategory = true;
            //LineaChart.XAxis.Title.Text = xAxisTitle;
            LineaChart.YAxis.Title.Text = yAxisTitle;

            LineaChart.Legend.Position = eLegendPosition.Bottom;


        }

        /// <summary>
        /// Configura la cabecera del resporte en excel para resolucion diaria, semanal o mesual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="cadena"></param>
        /// <param name="ruta"></param>
        /// AQUÍ
        public static void ConfiguracionHojaExcelM1(ExcelWorksheet ws, List<MeMedicion1DTO> listaCabecera, string cadena, string ruta)
        {
            int ncol = listaCabecera.Count;
            //var fill = ws.Cells[7, 2, 8, ncol + 2].Style;
            //fill.Fill.PatternType = ExcelFillStyle.Solid;
            //fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            //fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            //fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[4, 2, 11, ncol + 2].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            using (ExcelRange r = ws.Cells[4, 2, 11, ncol + 2])
            {
                //r.Merge = true;
                //r.Style.Font.SetFromFont(new Font("Arial", 22, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            var fontTabla = ws.Cells[4, 2, 11, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            ws.Cells[4, 2].Value = "PUNTO DE MEDICIÓN";
            ws.Cells[5, 2].Value = "CÓDIGO";
            ws.Cells[6, 2].Value = "CUENCA";
            ws.Cells[7, 2].Value = "EMPRESA";
            ws.Cells[8, 2].Value = "RECURSO";
            ws.Cells[9, 2].Value = "EQUIPO";
            ws.Cells[10, 2].Value = "TIPO";
            ws.Cells[11, 2].Value = "FECHA-HORA/UNIDAD";

            int col = 3;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 25;
            foreach (var reg in listaCabecera)
            {
                ws.Cells[4, col].Value = reg.Ptomedibarranomb;
                ws.Cells[5, col].Value = reg.Ptomedicodi;
                ws.Cells[6, col].Value = reg.Cuenca;
                ws.Cells[7, col].Value = reg.Emprnomb;
                ws.Cells[8, col].Value = reg.Famabrev;
                if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica) //Estacion Hidrologica
                    ws.Cells[9, col].Value = reg.Ptomedibarranomb;
                else
                    ws.Cells[9, col].Value = reg.Equinomb;

                ws.Cells[10, col].Value = reg.Tipoptomedinomb;

                ws.Cells[11, col].Value = reg.Tipoinfoabrev;
                ws.Column(col).Width = 20;

                col++;
            }
        }

        /// <summary>
        /// Mustra los datos de consulta Tabla MeMedidcion24 Diario, Semanal, Mensual, Año
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public static void ConfiguracionHojaExcelM24(ExcelWorksheet ws, List<MeMedicion24DTO> lista, int rbDetalleRpte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();
            List<DateTime> listaFechas = lista.Select(x => x.Medifecha).Distinct().ToList();
            int nBloques = listaFechas.Count;
            List<MeMedicion24DTO> listaCabeceraM24 = lista.GroupBy(x => new { x.Ptomedicodi, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Cuenca, x.Emprcodi, x.Emprnomb, x.Famabrev })
                                .Select(y => new MeMedicion24DTO()
                                {
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                    Equinomb = y.Key.Equinomb,
                                    Famcodi = y.Key.Famcodi,
                                    Cuenca = y.Key.Cuenca,
                                    Emprnomb = y.Key.Emprnomb,
                                    Emprcodi = y.Key.Emprcodi,
                                    Famabrev = y.Key.Famabrev
                                }
                                ).ToList();

            int ncol = listaCabeceraM24.Count;
            using (ExcelRange r = ws.Cells[5, 2, 11, ncol + 2])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            ///PRUEBA 1
            ws.Cells[4, 2].Value = "PUNTO DE MEDICIÓN";
            ws.Cells[5, 2].Value = "CÓDIGO";
            ws.Cells[6, 2].Value = "CUENCA";
            ws.Cells[7, 2].Value = "EMPRESA";
            ws.Cells[8, 2].Value = "RECURSO";
            ws.Cells[9, 2].Value = "EQUIPO";
            ws.Cells[10, 2].Value = "TIPO";
            ws.Cells[11, 2].Value = "FECHA-HORA/UNIDAD";


            int col = 3;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;

            var fontTabla = ws.Cells[4, 2, 11 + nBloques, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";


            //  IMPRIME ENCABEZADO...
            foreach (var reg in listaCabeceraM24)
            {
                ws.Cells[4, col].Value = reg.Ptomedibarranomb;
                ws.Cells[5, col].Value = reg.Ptomedicodi;
                ws.Cells[6, col].Value = reg.Cuenca;
                ws.Cells[7, col].Value = reg.Emprnomb;
                ws.Cells[8, col].Value = reg.Famabrev;
                if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica) //Estacion Hidrologica
                    ws.Cells[9, col].Value = reg.Ptomedibarranomb;
                else
                    ws.Cells[9, col].Value = reg.Equinomb;

                ws.Cells[10, col].Value = reg.Tipoptomedinomb;
                ws.Cells[11, col].Value = reg.Tipoinfoabrev;
                ws.Column(col).Width = 20;
                col++;
            }

            int row = 12;
            int column = 3;
            if (lista.Count > 0)
            {
                DateTime fant = new DateTime();
                DateTime f = new DateTime();

                //for (int k = 1; k <= nBloques; k++)
                int k = 1;
                foreach (var lst in lista)
                {
                    f = lst.Medifecha;
                    if (f != fant)
                    {
                        if (rbDetalleRpte == 1) // Diario
                        {
                            string fecha = listaFechas[k - 1].ToString(Constantes.FormatoFecha);
                            ws.Cells[row + k - 1, 2].Value = fecha;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                        }
                        if (rbDetalleRpte == 2) // Semanal programado
                        {
                            string semProg = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(listaFechas[k - 1], FirstDayOfWeek.Saturday);
                            ws.Cells[row + k - 1, 2].Value = semProg;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                        }
                        if (rbDetalleRpte == 3) //semanal cronologico
                        {
                            string semCron = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(listaFechas[k - 1], FirstDayOfWeek.Sunday);
                            ws.Cells[row + k - 1, 2].Value = semCron;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                        }
                        if (rbDetalleRpte == 4) // Mensual
                        {
                            string mmYY = listaFechas[k - 1].Year + "-" + COES.Base.Tools.Util.ObtenerNombreMes(listaFechas[k - 1].Month);
                            ws.Cells[row + k - 1, 2].Value = mmYY;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                        }
                        if (rbDetalleRpte == 5) //Anual
                        {
                            string year = "Año - " + listaFechas[k - 1].Year;
                            ws.Cells[row + k - 1, 2].Value = year;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                        }

                        int z = 0;
                        foreach (var p in listaCabeceraM24)
                        {
                            var reg = lista.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg.GetType().GetProperty("Meditotal").GetValue(reg, null);
                                if (valor != null)
                                    ws.Cells[row + k - 1, column + z].Value = valor;
                                else
                                {
                                    valor = 0;
                                    ws.Cells[row + k - 1, column + z].Value = valor;
                                }
                            }
                            else
                            {
                                decimal valor = 0;
                                ws.Cells[row + k - 1, column + z].Value = valor;
                            }

                            z++;
                        }
                        k++;
                    }//end del if
                    fant = f;

                }//******foreach
            }
            else
            {
                // NO HAY REGISTROS.....
            }
            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[12, 3, 12 + nBloques, 2 + ncol])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var border = ws.Cells[4, 2, 11 + nBloques, ncol + 2].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta Tabla MeMedidcion24 por horas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        /// ultimo
        public static void ConfiguracionHojaExcelM24Horas(ExcelWorksheet ws, List<MeMedicion24DTO> lista, List<MeMedicion24DTO> listaCabeceraM24, int rbDetalleRpte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();
            int ncol = listaCabeceraM24.Count;
            using (ExcelRange r = ws.Cells[4, 2, 11, ncol + 2])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[4, 2].Value = "PUNTO DE MEDICIÓN";
            ws.Cells[5, 2].Value = "CÓDIGO";
            ws.Cells[6, 2].Value = "CUENCA";
            ws.Cells[7, 2].Value = "EMPRESA";
            ws.Cells[8, 2].Value = "RECURSO";
            ws.Cells[9, 2].Value = "EQUIPO";
            ws.Cells[10, 2].Value = "TIPO";
            ws.Cells[11, 2].Value = "FECHA-HORA/UNIDAD";
            int col = 3;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            foreach (var reg in listaCabeceraM24)
            {
                ws.Cells[4, col].Value = reg.Ptomedibarranomb;
                ws.Cells[5, col].Value = reg.Ptomedicodi;
                ws.Cells[6, col].Value = reg.Cuenca;
                ws.Cells[7, col].Value = reg.Emprnomb;
                ws.Cells[8, col].Value = reg.Famabrev;
                if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica) //Estacion Hidrologica
                    ws.Cells[9, col].Value = reg.Ptomedibarranomb;
                else
                    ws.Cells[9, col].Value = reg.Equinomb;

                ws.Cells[10, col].Value = reg.Tipoptomedinomb;
                ws.Cells[11, col].Value = reg.Tipoinfoabrev;
                ws.Column(col).Width = 20;
                col++;
            }

            int nBloques = 24;
            int row = 12;
            int column = 3;

            if (lista.Count > 0)
            {
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var lst in lista)
                {
                    f = lst.Medifecha;
                    if (f != fant)
                    {
                        for (int k = 1; k <= nBloques; k++)
                        {
                            string hora = ("0" + (k - 1).ToString()).Substring(("0" + (k - 1).ToString()).Length - 2, 2) + ":00";
                            ws.Cells[row + k - 1, 2].Value = f.ToString(Constantes.FormatoFecha) + " - " + hora;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[row + k - 2, 2].StyleID;
                            int z = 0;
                            foreach (var p in listaCabeceraM24)
                            {
                                var reg = lista.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                                if (reg != null)
                                {
                                    decimal? valor;
                                    valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                                    if (valor != null)
                                        ws.Cells[row + k - 1, column + z].Value = valor;
                                    else
                                    {
                                        valor = 0;
                                        //ws.Cells[row + k - 1, column + z].Value = valor;
                                    }
                                    z++;
                                }
                                else
                                {
                                    decimal valor = 0;
                                    //ws.Cells[row + k - 1, column + z].Value = valor;
                                    z++;
                                }
                            }
                        }
                        row = row + 24;
                    }
                    fant = f;
                }
            }
            else
            {
                //No existen registros);
            }


            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[12, 3, row, 2 + ncol])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[4, 2, 11 + row, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[5, 2, row - 1, ncol + 2].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Muestra los datos de consulta Tabla MeMedidcion24 por horas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public static void ConfiguracionHojaExcelM24TR(ExcelWorksheet ws, List<MeMedicion24DTO> lista, List<MeReporptomedDTO> listaCabeceraM24, int rbDetalleRpte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();
            int ncol = listaCabeceraM24.Count;

            using (ExcelRange r = ws.Cells[5, 2, 10, ncol + 2])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
            }

            using (ExcelRange r = ws.Cells[11, 2, 11, 2])
            {
                r.Style.Font.Color.SetColor(Color.Black);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 206, 250));
            }

            ws.Cells[4, 2].Value = "PUNTO DE MEDICIÓN";
            ws.Cells[5, 2].Value = "CÓDIGO";
            ws.Cells[6, 2].Value = "CUENCA";
            ws.Cells[7, 2].Value = "EMPRESA";
            ws.Cells[8, 2].Value = "PTO MEDICIÓN/EMBALSE/CENTRAL";
            ws.Cells[9, 2].Value = "DESCRIPCIÓN";
            ws.Cells[10, 2].Value = "FECHA-HORA/UNIDAD";
            int col = 3;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            foreach (var reg in listaCabeceraM24)
            {
                ws.Cells[5, col].Value = reg.Ptomedicodi;
                ws.Cells[6, col].Value = reg.Cuenca;
                ws.Cells[7, col].Value = reg.Emprnomb;
                ws.Cells[8, col].Value = reg.Ptomedielenomb;
                ws.Cells[9, col].Value = reg.Ptomedidesc;
                ws.Cells[10, col].Value = reg.Tipoinfoabrev;
                ws.Column(col).Width = 20;
                col++;
            }

            int nBloques = 24;
            int row = 11;
            int column = 3;

            if (lista.Count > 0)
            {
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var lst in lista)
                {
                    f = lst.Medifecha;
                    if (f != fant)
                    {
                        for (int k = 1; k <= nBloques; k++)
                        {
                            string hora = ("0" + (k - 1).ToString()).Substring(("0" + (k - 1).ToString()).Length - 2, 2) + ":00";
                            ws.Cells[row + k - 1, 2].Value = f.ToString(Constantes.FormatoFecha) + " - " + hora;
                            ws.Cells[row + k - 1, 2].StyleID = ws.Cells[11, 2].StyleID;
                            int z = 0;
                            foreach (var p in listaCabeceraM24)
                            {
                                var reg = lista.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                                if (reg != null)
                                {
                                    decimal? valor;
                                    valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                                    if (valor != null)
                                        ws.Cells[row + k - 1, column + z].Value = valor;
                                    else
                                    {
                                        valor = 0;
                                        //ws.Cells[row + k - 1, column + z].Value = valor;
                                    }
                                    z++;
                                }
                                else
                                {
                                    decimal valor = 0;
                                    //ws.Cells[row + k - 1, column + z].Value = valor;
                                    z++;
                                }
                            }
                        }
                        row = row + 24;
                    }
                    fant = f;
                }
            }
            else
            {
                //No existen registros);
            }


            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[12, 3, row, 2 + ncol])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[4, 2, 11 + row, 2 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[5, 2, row - 1, ncol + 2].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Configuracion de archivo excel para reporte afluentes
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public static void ConfiguracionHojaExcel3(ExcelWorksheet ws, List<MeMedicion1DTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();
            List<MeMedicion1DTO> listaCabecera = lista.GroupBy(x => new { x.Ptomedicodi, x.Ptomedinomb, x.Tipoinfoabrev })
                     .Select(y => new MeMedicion1DTO()
                     {
                         Ptomedicodi = y.Key.Ptomedicodi,
                         Ptomedinomb = y.Key.Ptomedinomb,
                         Tipoinfoabrev = y.Key.Tipoinfoabrev
                     }
                     ).ToList();
            int ncol = listaCabecera.Count;
            var fill = ws.Cells[7, 2, 7, ncol + 3].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[7, 2, 7, ncol + 3].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


            using (ExcelRange r = ws.Cells[6, 2, 6, ncol + 3])
            {
                //r.Merge = true;
                //r.Style.Font.SetFromFont(new Font("Arial", 22, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[6, 3].Value = "AFLUENTES"; ws.Cells[7, 2].Value = "AÑO"; ws.Cells[7, 3].Value = "SEMANA";
            int col = 4;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            foreach (var reg in listaCabecera)
            {
                ws.Cells[6, col].Value = reg.Ptomedinomb;
                ws.Cells[7, col].Value = reg.Tipoinfoabrev;
                ws.Column(col).Width = 20;
                col++;
            }
            int row = 8;
            int column = 2;

            DateTime fechaInicio = lista.Min(x => x.Medifecha);
            int nSem = COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday);
            if (lista.Count > 0)
            {
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var reg in lista)
                {
                    f = reg.Medifecha;
                    if (f != fant)
                    {
                        var anho = f.Year.ToString();
                        var mes = f.Month;
                        ws.Cells[row, column].Value = anho;
                        ws.Cells[row, column + 1].Value = nSem;
                        ++nSem;
                        int z = 1;
                        foreach (var p in listaCabecera)
                        {
                            var reg2 = lista.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg2 != null)
                            {
                                decimal valor = (decimal)reg2.H1;
                                ws.Cells[row, column + 1 + z].Value = string.Format("{0}", valor.ToString("N", nfi));
                                z++;
                            }
                            else
                            {
                                ws.Cells[row, column + z + 1].Value = "";
                                z++;
                            }
                        }
                        z--;
                        border = ws.Cells[row, 2, row, 3 + z].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        var fontTabla = ws.Cells[row, 2, row, 3 + z].Style.Font;
                        fontTabla.Size = 8;
                        fontTabla.Name = "Calibri";
                        row++;
                    }
                    fant = f;
                }
            }

        }

        //Configuracion de area para reporte Topologia
        public static void ConfiguracionHojaExcel4(ExcelWorksheet ws, List<EqEquipoDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 4].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 4].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "EMPRESA"; ws.Cells[5, 3].Value = "TIPO"; ws.Cells[5, 4].Value = "NOMBRE";
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Emprnomb;
                    ws.Cells[row, column + 1].Value = reg.Famnomb;
                    ws.Cells[row, column + 2].Value = reg.Equinomb;
                    border = ws.Cells[row, 2, row, 4].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 4].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }

        //Configuracion de area para reporte Archivos enviados
        public static void ConfiguracionHojaExcel5(ExcelWorksheet ws, List<MeEnvioDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 13].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 13].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            ws.Cells[5, 2].Value = "ID ENVIO";
            ws.Cells[5, 3].Value = "PERIODO";
            ws.Cells[5, 4].Value = "FECHA PERIODO";
            ws.Cells[5, 5].Value = "EMPRESA";
            ws.Cells[5, 6].Value = "ESTADO";
            ws.Cells[5, 7].Value = "CUMPLIMIENTO";
            ws.Cells[5, 8].Value = "FECHA ENVIO";
            ws.Cells[5, 9].Value = "LECTURA";
            ws.Cells[5, 10].Value = "FORMATO";
            ws.Cells[5, 11].Value = "USUARIO";
            ws.Cells[5, 12].Value = "CORREO";
            ws.Cells[5, 13].Value = "TELEFONO";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 30;
            ws.Column(9).Width = 30;
            ws.Column(10).Width = 30;
            ws.Column(11).Width = 30;
            ws.Column(12).Width = 30;
            ws.Column(13).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Enviocodi;
                    ws.Cells[row, column + 1].Value = reg.Periodo;
                    ws.Cells[row, column + 2].Value = reg.FechaPeriodo;
                    ws.Cells[row, column + 3].Value = reg.Emprnomb;
                    ws.Cells[row, column + 4].Value = reg.Estenvnombre;
                    var eplazo = "";
                    if (reg.Envioplazo == "F")
                    {
                        eplazo = "Fuera de Plazo";
                    }
                    else
                    {
                        eplazo = "En Plazo";
                    }
                    ws.Cells[row, column + 5].Value = eplazo;
                    DateTime fechaenvio = (DateTime)reg.Enviofecha;
                    ws.Cells[row, column + 6].Value = fechaenvio.ToString(Constantes.FormatoFechaHora); ;
                    ws.Cells[row, column + 7].Value = reg.Lectnomb;
                    ws.Cells[row, column + 8].Value = reg.Formatnombre;
                    ws.Cells[row, column + 9].Value = reg.Username;
                    ws.Cells[row, column + 10].Value = reg.Lastuser;
                    ws.Cells[row, column + 11].Value = reg.Usertlf;

                    border = ws.Cells[row, 2, row, 13].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 13].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        /// <summary>
        /// Genera archivo excel de reporte EJECUTADO HISTORICO/TR - Diario, Semanal, Mensual, Anual
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarArchivoHidrologiaM24(HidrologiaModel model, int rbDetalleRpte, string ruta)
        {
            List<MeMedicion24DTO> list = model.ListaMedicion24horas;
            FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia01);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia01);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];
                string titulo = string.Empty;
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas)
                    titulo = model.TituloReporteXLS + "- HORAS";
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Diario)
                    titulo = model.TituloReporteXLS + "- DIARIO";
                if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalProgramado)
                    titulo = model.TituloReporteXLS + "- SEMANAL PROGRAMADO";
                if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalCronologico)
                    titulo = model.TituloReporteXLS + "- SEMANAL CRONOLÓGICO";
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Mensual)
                    titulo = model.TituloReporteXLS + "- MENSUAL";
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Anual)
                    titulo = model.TituloReporteXLS + "- ANUAL";

                ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin, ruta);
                if ((rbDetalleRpte == ConstantesRbDetalleRpte.Horas))
                    ConfiguracionHojaExcelM24Horas(ws, list, model.ListaMedicion24Cabecera, rbDetalleRpte);
                else
                    ConfiguracionHojaExcelM24(ws, list, rbDetalleRpte);
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Genera archivo excel de reporte de teimpo real - Diario
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="ruta"></param>
        public static void GenerarArchivoHidrologiaM24TR(HidrologiaModel model, List<MeReporptomedDTO> listaCabecera, int tipoReporte, string ruta)
        {

            if (model.FechaRepInicio == model.FechaRepFinal)
            {
                List<MeMedicion24DTO> list = model.ListaMedicion24horas;
                FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
                FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia01);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia01);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = null;
                    //Hoja Horas
                    ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                    ws = xlPackage.Workbook.Worksheets[model.SheetName];

                    string titulo = model.TituloReporteXLS;
                    ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin, ruta);
                    ConfiguracionHojaExcelM24TR(ws, list, listaCabecera, ConstantesRbDetalleRpte.Horas);
                    // ConfiguracionHojaExcelM24TR(ws, list, listaCabecera, tipoReporte);
                    xlPackage.Save();
                }
            }

            else
            {
                List<MeMedicion24DTO> list = model.ListaMedicion24horas;
                FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
                FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia001);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia001);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = null;
                    //Hoja Horas
                    ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                    ws = xlPackage.Workbook.Worksheets[model.SheetName];

                    string titulo = model.TituloReporteXLS;
                    ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin, ruta);
                    ConfiguracionHojaExcelM24TR(ws, list, listaCabecera, ConstantesRbDetalleRpte.Horas);
                    // ConfiguracionHojaExcelM24TR(ws, list, listaCabecera, tipoReporte);
                    xlPackage.Save();
                }

            }
        }
        //Genera archivo excel de reporte M1
        //aquí
        public static void GenerarArchivoHidrologiaM1(HidrologiaModel model, int rbDetalleRpte, string ruta)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();
            List<MeMedicion1DTO> list = model.ListaMedicion1;
            List<MeMedicion1DTO> listaCabecera = list.GroupBy(x => new { x.Ptomedicodi, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Equinomb, x.Tipoptomedinomb, x.Cuenca, x.Emprnomb, x.Emprcodi, x.Famabrev })
                     .Select(y => new MeMedicion1DTO()
                     {
                         Ptomedicodi = y.Key.Ptomedicodi,
                         Ptomedibarranomb = y.Key.Ptomedibarranomb,
                         Tipoinfoabrev = y.Key.Tipoinfoabrev,
                         Equinomb = y.Key.Equinomb,
                         Tipoptomedinomb = y.Key.Tipoptomedinomb,
                         Cuenca = y.Key.Cuenca,
                         Emprnomb = y.Key.Emprnomb,
                         Emprcodi = y.Key.Emprcodi,
                         Famabrev = y.Key.Famabrev
                     }
                     ).ToList();
            int ncol = listaCabecera.Count;
            int nfil = 0;

            FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia03);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia03);
            }
            int row = 12;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];
                string titulo = model.TituloReporteXLS;
                ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin, ruta);

                ConfiguracionHojaExcelM1(ws, listaCabecera, "FECHA", ruta);

                if (list.Count > 0)
                {
                    DateTime fant = new DateTime();
                    DateTime f = new DateTime();
                    foreach (var reg in list)
                    {
                        f = reg.Medifecha;
                        if (f != fant)
                        {

                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Diario) //Diario    
                                ws.Cells[row, column].Value = f.ToString(Constantes.FormatoFecha);
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalProgramado) // Semanal Programado
                                ws.Cells[row, column].Value = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Saturday) + " - " + f.Year;
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalCronologico) //Semanal Cronológico
                                ws.Cells[row, column].Value = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Sunday) + " - " + f.Year;
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Mensual) //Menual
                                ws.Cells[row, column].Value = f.Year + "-" + COES.Base.Tools.Util.ObtenerNombreMes(f.Month);
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Anual)// ANual
                                ws.Cells[row, column].Value = "Año - " + f.Year;
                            ws.Cells[row, column].StyleID = ws.Cells[row - 1, column].StyleID;
                            int z = 1;
                            foreach (var p in listaCabecera)
                            {
                                var reg2 = list.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                                if (reg2 != null)
                                {
                                    decimal? valor;
                                    valor = (decimal?)reg2.H1;
                                    if (valor != null)
                                        ws.Cells[row, column + z].Value = valor;
                                    else
                                    {
                                        valor = 0;
                                        ws.Cells[row, column + z].Value = valor;
                                    }
                                    z++;
                                }
                                else
                                {
                                    decimal valor = 0;
                                    ws.Cells[row, column + z].Value = valor;
                                    z++;
                                }
                            }
                            z--;
                            var border = ws.Cells[row, 2, row, 2 + z].Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            var fontTabla = ws.Cells[row, 2, row, 2 + z].Style.Font;
                            fontTabla.Size = 8;
                            fontTabla.Name = "Calibri";
                            row++;
                            nfil++;
                        }
                        fant = f;
                    }
                }// end del if
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[12, 3, 12 + nfil, 2 + ncol])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                xlPackage.Save();
            }// end del using

        }

        //genera archivo de grafico Programado Diario, Ejecutado historico
        public static void GenerarArchivoGrafM24(HidrologiaModel model, string ruta)
        {

            List<String> listCategoriaGrafico = model.Grafico.XAxisCategories; // Lista de horas, dias, meses, semanas, años ordenados para la categoria del grafico

            var series = model.Grafico.Series; // lista de valores para las series del grafico
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia01);
            int nfil = 0;
            if (series != null)
                nfil = series[0].Data.Count;
            int ncol = series.Count();
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia01);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];

                //string titulo = model.TituloReporteXLS;
                string titulo = model.Grafico.TitleText;
                switch (model.RbTipoReporte)
                {
                    case ConstantesRbDetalleRpte.Horas: //horas
                        titulo = titulo + " - HORAS";
                        break;
                    case ConstantesRbDetalleRpte.Diario: // dias
                        titulo = titulo + " - DIAS ";
                        break;
                    case ConstantesRbDetalleRpte.SemanalProgramado: // Semanal programado
                        titulo = titulo + " - SEMANAL PROGRAMADO";
                        break;
                    case ConstantesRbDetalleRpte.SemanalCronologico: // semanal cronológico
                        titulo = titulo + " - SEMANAL CRONOLÓGICO";
                        break;
                    case ConstantesRbDetalleRpte.Mensual: // meses
                        titulo = titulo + " - MESES";
                        break;
                    case ConstantesRbDetalleRpte.Anual: // años
                        titulo = titulo + " - AÑOS";
                        break;

                }
                ConfiguraEncabezadoHojaExcel2(ws, titulo, model.FechaInicio, model.FechaFin, model.RbTipoReporte, ruta);
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 20;
                ws.Cells[row - 1, 2].Value = "FECHA";

                //Borde cabecera de Tabla Listado
                var border = ws.Cells[row - 1, 2, row - 1, ncol + 2].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, ncol + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.Font.Size = 8;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    r.Style.WrapText = true;

                }

                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                int x = 0;
                int y = 0;

                foreach (var reg in series)
                {
                    ws.Cells[row - 1, x + 3].Value = reg.Name;
                    foreach (var reg2 in reg.Data)
                    {
                        if (x == 0)
                        {
                            ws.Cells[row + y, 2].Value = reg2.X.ToString(Constantes.FormatoFechaHora);
                        }
                        ws.Cells[y + row, x + 3].Value = reg2.Y;
                        y++;
                    }
                    x++;
                    y = 0;
                }
                string xAxisTitle = model.Grafico.XAxisTitle;
                string yAxisTitle = model.Grafico.YaxixTitle;
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                xlPackage.Save();
            }

        }

        //genera archivo de grafico Programado Mensual QN
        public static void GenerarArchivoGrafM1(HidrologiaModel model, string ruta)
        {
            List<String> listCategoriaGrafico = model.Grafico.XAxisCategories; // Lista fechas ordenados para la categoria del grafico
            List<String> listSerieName = new List<String>(); //Lista de nombres de las series del grafico(Ptos de medicion)
            decimal?[][] listSerieData = model.Grafico.SeriesData; // lista de valores para las series del grafico

            // Obtener Lista de nombres de las series del grafico.
            var listaGrupoMedicion = model.ListaMedicion1.GroupBy(x => x.Ptomedicodi).Select(group => group.First()).ToList();
            int z = 0;
            foreach (var reg in listaGrupoMedicion)
            {
                listSerieName.Add(reg.Ptomedibarranomb);
                z++;
            }

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia04);
            int nfil = listCategoriaGrafico.Count;
            int ncol = listSerieName.Count;
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia04);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];

                string titulo = model.Grafico.TitleText;
                switch (model.RbTipoReporte)
                {

                    case ConstantesRbDetalleRpte.Diario: // dias
                        titulo = titulo + " - DIAS ";
                        break;
                    case ConstantesRbDetalleRpte.SemanalProgramado: // Semanal programado
                        titulo = titulo + " - SEMANAL PROGRAMADO";
                        break;
                    case ConstantesRbDetalleRpte.SemanalCronologico: // semanal cronológico
                        titulo = titulo + " - SEMANAL CRONOLÓGICO";
                        break;
                    case ConstantesRbDetalleRpte.Mensual: // meses
                        titulo = titulo + " - MESES";
                        break;
                    case ConstantesRbDetalleRpte.Anual: // años
                        titulo = titulo + " - AÑOS";
                        break;

                }
                ConfiguraEncabezadoHojaExcel2(ws, titulo, model.FechaInicio, model.FechaFin, model.RbTipoReporte, ruta);
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 20;
                ws.Cells[row - 1, 2].Value = "AFLUENTES";
                for (int i = 0; i < ncol; i++)
                {
                    ws.Cells[row - 1, i + 3].Value = listSerieName[i];
                }
                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listCategoriaGrafico[i];
                }

                //Borde cabecera de Tabla Listado
                var border = ws.Cells[row - 1, 2, row - 1, ncol + 2].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, ncol + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    r.Style.WrapText = true;
                    r.Style.Font.Size = 8;

                }
                for (int i = 0; i < ncol; i++) //inserta los datos 
                    for (int j = 0; j < nfil; j++)
                    {
                        ws.Cells[j + 7, i + 3].Value = listSerieData[i][j];
                    }
                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";
                string xAxisTitle = model.Grafico.XAxisTitle;
                string yAxisTitle = model.Grafico.YaxixTitle;
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                xlPackage.Save();
            }
        }

        //genera archivo de grafico tiempo real - volumenes
        public static void GenerarArchivoGrafTR(HidrologiaModel model, string ruta)
        {
            List<String> listCategoriaGrafico = model.Grafico.XAxisCategories; // Lista de horas, dias, meses, semanas, años ordenados para la categoria del grafico
            List<String> listSerieName = model.Grafico.SeriesName; //Lista de nombres de las series del grafico(Ptos de medicion)
            decimal?[][] listSerieData = model.Grafico.SeriesData; // lista de valores para las series del grafico
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia01);
            int nfil = listCategoriaGrafico.Count;
            int ncol = listSerieName.Count;
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreRptGraficoHidrologia01);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];

                //string titulo = model.TituloReporteXLS;
                string titulo = model.Grafico.TitleText;

                ConfiguraEncabezadoHojaExcel2(ws, titulo, model.FechaInicio, model.FechaFin, 0, ruta);
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 20;
                ws.Cells[row - 1, 2].Value = "FECHA";
                for (int i = 0; i < ncol; i++)
                {
                    ws.Cells[row - 1, i + 3].Value = listSerieName[i];
                }
                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listCategoriaGrafico[i];
                }

                //Borde cabecera de Tabla Listado
                var border = ws.Cells[row - 1, 2, row - 1, ncol + 2].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, ncol + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

                }

                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";
                for (int i = 0; i < ncol; i++) //inserta los datos 
                    for (int j = 0; j < nfil; j++)
                    {
                        ws.Cells[j + row, i + 3].Value = listSerieData[i][j];
                    }
                string xAxisTitle = model.Grafico.XAxisTitle;
                string yAxisTitle = model.Grafico.YaxixTitle;
                AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                xlPackage.Save();
            }
        }

        // Genera archivo excel de listado topología
        public static void GenerarArchivoTopologia(HidrologiaModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteTopologia);
            List<EqEquipoDTO> list = model.ListaRecursosCuenca;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteTopologia);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE TOPOLOGIA");
                ws = xlPackage.Workbook.Worksheets["RPTE TOPOLOGIA"];
                string titulo = "RECURSO ORIGEN";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcel4(ws, list);
                xlPackage.Save();
            }
        }

        //Genera archivo excel de listado de archivos enviados
        public static void GenerarArchivosEnviados(HidrologiaModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteArchivosEnviados);
            List<MeEnvioDTO> list = model.ListaEnvio;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteArchivosEnviados);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE ARCHENVIADOS");
                ws = xlPackage.Workbook.Worksheets["RPTE ARCHENVIADOS"];
                string titulo = "REPORTE HISTORICO DE ARCHIVOS ENVIADOS";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcel5(ws, list);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera  archivo de reporte de cumplimiento
        /// </summary>
        /// <param name="ruta"></param>
        public static void GeneraArchivoReporteCumplimiento(string ruta)
        {
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreArchivoCumplimiento);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreArchivoCumplimiento);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Cumplimiento");
                ws = xlPackage.Workbook.Worksheets["Cumplimiento"];
                string titulo = "REPORTE DE CUMPLIMIENTO";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                //ConfiguracionHojaExcel5(ws, list);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera archivo excel de reporte de teimpo real - Diario
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="ruta"></param>
        public static void GenerarArchivoHidrologiaDescargaVert(HidrologiaModel model, int tipoReporte, string ruta)
        {
            List<MeMedicionxintervaloDTO> list = model.ListaDescargaVert;
            //FileInfo template = new FileInfo(ruta + ConstantesHidrologia.PlantillaExcelHidrologia);
            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia05);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreReporteHidrologia05);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets[model.SheetName];

                string titulo = model.TituloReporteXLS;
                ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin, ruta);
                ConfiguracionHojaExcelDescargaVert(ws, list);
                xlPackage.Save();
            }
        }



        /// <summary>
        /// Configuracion de formatos para numeros al sistema internacional
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo2()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Muestra los datos de consulta Tabla MeMedicioxIntervalo para reporte Descarga de Lagunas y Vertimiento de Embalses
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        public static void ConfiguracionHojaExcelDescargaVert(ExcelWorksheet ws, List<MeMedicionxintervaloDTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();

            using (ExcelRange r = ws.Cells[6, 2, 6, 11])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[6, 2].Value = "FECHA";
            ws.Cells[6, 3].Value = "EMPRESA";
            ws.Cells[6, 4].Value = "EMBALSE";
            ws.Cells[6, 5].Value = "INICIO";
            ws.Cells[6, 6].Value = "FINAL";
            ws.Cells[6, 7].Value = "VALOR";
            ws.Cells[6, 8].Value = "UNIDAD";
            ws.Cells[6, 9].Value = "USUARIO";
            ws.Cells[6, 10].Value = "FECHA MODIFICACIÓN";
            ws.Cells[6, 11].Value = "OBSERVACIÓN";
            int col = 2;
            int fil = 7;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;
            for (int i = 3; i <= 11; i++)
            {
                ws.Column(i).Width = 15;
            }
            foreach (var reg in lista)
            {
                var fechaMin = reg.Medintfechaini.ToString(Constantes.FormatoFecha);

                decimal? valor;
                valor = (decimal?)reg.Medinth1;
                if (valor != null & valor != 0)
                {
                    ws.Cells[fil, col].Value = fechaMin;
                    ws.Cells[fil, col + 1].Value = reg.Emprnomb;
                    ws.Cells[fil, col + 2].Value = reg.Equinomb;
                    var horaIni = reg.Medintfechaini.ToString(Constantes.FormatoHora);
                    ws.Cells[fil, col + 3].Value = horaIni;
                    var horaFin = reg.Medintfechafin.ToString(Constantes.FormatoHora);
                    ws.Cells[fil, col + 4].Value = horaFin;

                    ws.Cells[fil, col + 5].Value = valor;
                    using (var range = ws.Cells[fil, col + 5, fil, col + 5])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                    }

                    ws.Cells[fil, col + 6].Value = reg.Tipoinfoabrev;
                    ws.Cells[fil, col + 7].Value = reg.Medintusumodificacion;
                    var fechaModif = ((DateTime)reg.Medintfecmodificacion).ToString(Constantes.FormatoFecha);
                    ws.Cells[fil, col + 8].Value = fechaModif;
                    ws.Cells[fil, col + 9].Value = reg.Medintdescrip;
                    fil++;
                }
            }

            ////////////// Formato de Celdas Valores

            var fontTabla = ws.Cells[6, 2, fil - 1, 11].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, fil - 1, 11].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Permite exportar el formato de puntos de medicion
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="origenLectura">Código de origen de lectura</param>
        public static void ExportarPuntosMedicion(List<MePtomedicionDTO> list, string path, string filename, string origenLectura, int tipoPunto)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PUNTOS");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "PUNTOS DE MEDICIÓN";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;



                        if (origenLectura == "19")
                        {
                            ws.Cells[index, 2].Value = "CODIGO";
                            ws.Cells[index, 3].Value = "EMPRESA";
                            ws.Cells[index, 4].Value = "CÓDIGO CL";
                            ws.Cells[index, 5].Value = "TENSIÓN";
                            ws.Cells[index, 6].Value = "PUNTO DE MEDICIÓN";
                            ws.Cells[index, 7].Value = "TIPO PUNTO";
                            ws.Cells[index, 8].Value = "UBICACIÓN";
                            ws.Cells[index, 9].Value = "TIPO EQUIPO";
                            ws.Cells[index, 10].Value = "EQUIPO";
                            ws.Cells[index, 11].Value = "ORIGEN LECTURA";
                            ws.Cells[index, 12].Value = "USUARIO";
                            ws.Cells[index, 13].Value = "FECHA";

                            rg = ws.Cells[index, 2, index, 13];
                        }
                        else
                        {
                            ws.Cells[index, 2].Value = "CODIGO";
                            ws.Cells[index, 3].Value = "EMPRESA";
                            ws.Cells[index, 4].Value = "PUNTO DE MEDICIÓN";

                            if (tipoPunto != 3)
                            {
                                ws.Cells[index, 5].Value = "TIPO PUNTO";
                                ws.Cells[index, 6].Value = "UBICACIÓN";
                                ws.Cells[index, 7].Value = "TIPO EQUIPO";
                                ws.Cells[index, 8].Value = "EQUIPO";
                                ws.Cells[index, 9].Value = "ORIGEN LECTURA";
                                ws.Cells[index, 10].Value = "USUARIO";
                                ws.Cells[index, 11].Value = "FECHA";

                                rg = ws.Cells[index, 2, index, 11];
                            }
                            else
                            {
                                ws.Cells[index, 5].Value = "CLIENTE";
                                ws.Cells[index, 6].Value = "BARRA";
                                ws.Cells[index, 7].Value = "USUARIO";
                                ws.Cells[index, 8].Value = "FECHA";

                                rg = ws.Cells[index, 2, index, 8];
                            }
                        }


                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (MePtomedicionDTO item in list)
                        {
                            if (origenLectura == "19")
                            {
                                var arrNombre = item.Ptomedielenomb.Split('-');
                                ws.Cells[index, 2].Value = item.Ptomedicodi;
                                ws.Cells[index, 3].Value = item.Emprnomb;
                                ws.Cells[index, 4].Value = arrNombre.Count() > 0 ? arrNombre[0] : item.Ptomedielenomb;
                                ws.Cells[index, 5].Value = item.NivelTension;
                                ws.Cells[index, 6].Value = item.Ptomedielenomb;
                                ws.Cells[index, 7].Value = item.Tipoptomedinomb;
                                ws.Cells[index, 8].Value = item.DesUbicacion;
                                ws.Cells[index, 9].Value = item.Famnomb;
                                ws.Cells[index, 10].Value = item.Equinomb;
                                ws.Cells[index, 11].Value = item.Origlectnombre;
                                ws.Cells[index, 12].Value = item.Lastuser;
                                ws.Cells[index, 13].Value = item.Lastdate;

                                rg = ws.Cells[index, 2, index, 13];
                            }
                            else
                            {
                                ws.Cells[index, 2].Value = item.Ptomedicodi;
                                ws.Cells[index, 3].Value = item.Emprnomb;
                                ws.Cells[index, 4].Value = item.Ptomedielenomb;

                                if (tipoPunto != 3)
                                {
                                    ws.Cells[index, 5].Value = item.Tipoptomedinomb;
                                    ws.Cells[index, 6].Value = item.DesUbicacion;
                                    ws.Cells[index, 7].Value = item.Famnomb;
                                    ws.Cells[index, 8].Value = item.Equinomb;
                                    ws.Cells[index, 9].Value = item.Origlectnombre;
                                    ws.Cells[index, 10].Value = item.Lastuser;
                                    ws.Cells[index, 11].Value = item.Lastdate;

                                    rg = ws.Cells[index, 2, index, 11];
                                }
                                else
                                {
                                    ws.Cells[index, 5].Value = item.ClientNomb;
                                    ws.Cells[index, 6].Value = item.BarrNomb;
                                    ws.Cells[index, 7].Value = item.Lastuser;
                                    ws.Cells[index, 8].Value = item.Lastdate;

                                    rg = ws.Cells[index, 2, index, 8];
                                }
                            }


                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }


                        if (origenLectura == "19")
                        {
                            rg = ws.Cells[5, 2, index - 1, 13];
                        }
                        else
                        {
                            rg = ws.Cells[5, 2, index - 1, 11];
                        }

                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 30;
                        ws.Column(6).Width = 30;
                        ws.Column(7).Width = 30;
                        ws.Column(8).Width = 30;
                        ws.Column(9).Width = 30;
                        ws.Column(10).Width = 30;
                        ws.Column(11).Width = 30;

                        if (origenLectura == "19")
                        {
                            ws.Column(12).Width = 30;
                            ws.Column(13).Width = 30;
                        }

                        rg = ws.Cells[5, 3, index, 7];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void GenerarArchivoGraficoAnual(List<GraficoSeries> list, int tipoptomedicion, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoAnual);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoAnual);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico Anual");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                if (tipoptomedicion == 89) //CAUDAL EVAPORADO
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                else if (tipoptomedicion == 8) //CAUDAL NATURAL ESTIMADO
                {
                    ws.Cells[2, 1].Value = "RIO";
                }
                else //VOLUMEN UTIL
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }

                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 16])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "AÑO";
                ws.Cells[5, 2].Value = "ENE";
                ws.Cells[5, 3].Value = "FEB";
                ws.Cells[5, 4].Value = "MAR";
                ws.Cells[5, 5].Value = "ABR";
                ws.Cells[5, 6].Value = "MAY";
                ws.Cells[5, 7].Value = "JUN";
                ws.Cells[5, 8].Value = "JUL";
                ws.Cells[5, 9].Value = "AGO";
                ws.Cells[5, 10].Value = "SEP";
                ws.Cells[5, 11].Value = "OCT";
                ws.Cells[5, 12].Value = "NOV";
                ws.Cells[5, 13].Value = "DIC";
                ws.Cells[5, 14].Value = "Maximo";
                ws.Cells[5, 15].Value = "Minimo";
                ws.Cells[5, 16].Value = "Promedio";

                int row = 6;
                foreach (var item in list)
                {
                    ws.Cells[row, 1].Value = item.Anio;
                    ws.Cells[row, 2].Value = item.M1;
                    ws.Cells[row, 3].Value = item.M2;
                    ws.Cells[row, 4].Value = item.M3;
                    ws.Cells[row, 5].Value = item.M4;
                    ws.Cells[row, 6].Value = item.M5;
                    ws.Cells[row, 7].Value = item.M6;
                    ws.Cells[row, 8].Value = item.M7;
                    ws.Cells[row, 9].Value = item.M8;
                    ws.Cells[row, 10].Value = item.M9;
                    ws.Cells[row, 11].Value = item.M10;
                    ws.Cells[row, 12].Value = item.M11;
                    ws.Cells[row, 13].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    ws.Cells[row, 14].Value = valores.Max();
                    ws.Cells[row, 15].Value = valores.Min();
                    ws.Cells[row, 16].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));

                    row++;
                }

                // Agregar filas para Maximo, Minimo, Promedio
                ws.Cells[row, 1].Value = "Maximo";
                ws.Cells[row + 1, 1].Value = "Minimo";
                ws.Cells[row + 2, 1].Value = "Promedio";

                for (int col = 2; col <= 13; col++)
                {
                    var valoresColumna = new List<double>();
                    for (int fila = 6; fila < row; fila++)
                    {
                        valoresColumna.Add(Convert.ToDouble(ws.Cells[fila, col].Value));
                    }

                    ws.Cells[row, col].Value = valoresColumna.Max();
                    ws.Cells[row + 1, col].Value = valoresColumna.Min();
                    ws.Cells[row + 2, col].Value = Convert.ToDouble(String.Format("{0:#,0.000}", valoresColumna.Average()));
                }

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 2, 16].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 2, 16].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 16; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }

        public static void GenerarArchivoGraficoAnualDesvStandar(List<GraficoSeries> list, int tipoptomedicion, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoAnual);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoAnual);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico Anual");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                if (tipoptomedicion == 89) //CAUDAL EVAPORADO
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                else if (tipoptomedicion == 8) //CAUDAL NATURAL ESTIMADO
                {
                    ws.Cells[2, 1].Value = "RIO";
                }
                else //VOLUMEN UTIL
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }

                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 17])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "AÑO";
                ws.Cells[5, 2].Value = "ENE";
                ws.Cells[5, 3].Value = "FEB";
                ws.Cells[5, 4].Value = "MAR";
                ws.Cells[5, 5].Value = "ABR";
                ws.Cells[5, 6].Value = "MAY";
                ws.Cells[5, 7].Value = "JUN";
                ws.Cells[5, 8].Value = "JUL";
                ws.Cells[5, 9].Value = "AGO";
                ws.Cells[5, 10].Value = "SEP";
                ws.Cells[5, 11].Value = "OCT";
                ws.Cells[5, 12].Value = "NOV";
                ws.Cells[5, 13].Value = "DIC";
                ws.Cells[5, 14].Value = "Máximo";
                ws.Cells[5, 15].Value = "Mínimo";
                ws.Cells[5, 16].Value = "Promedio";
                ws.Cells[5, 17].Value = "Desv.Standar";

                int row = 6;
                foreach (var item in list)
                {
                    ws.Cells[row, 1].Value = item.Anio;
                    ws.Cells[row, 2].Value = item.M1;
                    ws.Cells[row, 3].Value = item.M2;
                    ws.Cells[row, 4].Value = item.M3;
                    ws.Cells[row, 5].Value = item.M4;
                    ws.Cells[row, 6].Value = item.M5;
                    ws.Cells[row, 7].Value = item.M6;
                    ws.Cells[row, 8].Value = item.M7;
                    ws.Cells[row, 9].Value = item.M8;
                    ws.Cells[row, 10].Value = item.M9;
                    ws.Cells[row, 11].Value = item.M10;
                    ws.Cells[row, 12].Value = item.M11;
                    ws.Cells[row, 13].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    List<decimal?> valoresList = new List<decimal?>();
                    foreach (var itemValor in valores)
                    {
                        if (itemValor != null)
                        {
                            valoresList.Add(itemValor);
                        }
                    }
                    ws.Cells[row, 14].Value = valores.Max();
                    ws.Cells[row, 15].Value = valores.Min();
                    ws.Cells[row, 16].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));
                    ws.Cells[row, 17].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", standardDesviation(valoresList)));

                    row++;
                }

                // Agregar filas para Maximo, Minimo, Promedio
                ws.Cells[row, 1].Value = "Máximo";
                ws.Cells[row + 1, 1].Value = "Mínimo";
                ws.Cells[row + 2, 1].Value = "Promedio";
                ws.Cells[row + 3, 1].Value = "Desv.Standar";

                for (int col = 2; col <= 13; col++)
                {
                    var valoresColumna = new List<double>();
                    for (int fila = 6; fila < row; fila++)
                    {
                        valoresColumna.Add(Convert.ToDouble(ws.Cells[fila, col].Value));
                    }
                    var valoresColumnaDesvStandar = new List<decimal?>();
                    for (int fila = 6; fila < row; fila++)
                    {
                        valoresColumnaDesvStandar.Add(Convert.ToDecimal(ws.Cells[fila, col].Value));
                    }

                    ws.Cells[row, col].Value = valoresColumna.Max();
                    ws.Cells[row + 1, col].Value = valoresColumna.Min();
                    ws.Cells[row + 2, col].Value = Convert.ToDouble(String.Format("{0:#,0.000}", valoresColumna.Average()));
                    ws.Cells[row + 3, col].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", standardDesviation(valoresColumnaDesvStandar)));
                }

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 3, 17].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 3, 17].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 17; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }

        public static double standardDesviation(List<decimal?> valores)
        {
            decimal? average = valores.Average();
            decimal? sum = Convert.ToDecimal(valores.Sum(d => Math.Pow(Convert.ToDouble(d - average), 2)));
            return Math.Sqrt((Convert.ToDouble(sum)) / valores.Count());
        }

        public static void GenerarArchivoGraficoMensual(List<GraficoSeries> list, int tipoptomedicion, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoMensual);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoMensual);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico Mensual");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                if (tipoptomedicion == 89) //CAUDAL EVAPORADO
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                else if (tipoptomedicion == 8) //CAUDAL NATURAL ESTIMADO
                {
                    ws.Cells[2, 1].Value = "RIO";
                }
                else //VOLUMEN UTIL
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 16])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "AÑO";
                ws.Cells[5, 2].Value = "ENE";
                ws.Cells[5, 3].Value = "FEB";
                ws.Cells[5, 4].Value = "MAR";
                ws.Cells[5, 5].Value = "ABR";
                ws.Cells[5, 6].Value = "MAY";
                ws.Cells[5, 7].Value = "JUN";
                ws.Cells[5, 8].Value = "JUL";
                ws.Cells[5, 9].Value = "AGO";
                ws.Cells[5, 10].Value = "SEP";
                ws.Cells[5, 11].Value = "OCT";
                ws.Cells[5, 12].Value = "NOV";
                ws.Cells[5, 13].Value = "DIC";
                ws.Cells[5, 14].Value = "Maximo";
                ws.Cells[5, 15].Value = "Minimo";
                ws.Cells[5, 16].Value = "Promedio";

                int row = 6;
                foreach (var item in list)
                {
                    ws.Cells[row, 1].Value = item.Anio;
                    ws.Cells[row, 2].Value = item.M1;
                    ws.Cells[row, 3].Value = item.M2;
                    ws.Cells[row, 4].Value = item.M3;
                    ws.Cells[row, 5].Value = item.M4;
                    ws.Cells[row, 6].Value = item.M5;
                    ws.Cells[row, 7].Value = item.M6;
                    ws.Cells[row, 8].Value = item.M7;
                    ws.Cells[row, 9].Value = item.M8;
                    ws.Cells[row, 10].Value = item.M9;
                    ws.Cells[row, 11].Value = item.M10;
                    ws.Cells[row, 12].Value = item.M11;
                    ws.Cells[row, 13].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    ws.Cells[row, 14].Value = valores.Max();
                    ws.Cells[row, 15].Value = valores.Min();
                    ws.Cells[row, 16].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));

                    row++;
                }

                // Agregar filas para Maximo, Minimo, Promedio
                ws.Cells[row, 1].Value = "Maximo";
                ws.Cells[row + 1, 1].Value = "Minimo";
                ws.Cells[row + 2, 1].Value = "Promedio";

                for (int col = 2; col <= 13; col++)
                {
                    var valoresColumna = new List<double>();
                    for (int fila = 6; fila < row; fila++)
                    {
                        valoresColumna.Add(Convert.ToDouble(ws.Cells[fila, col].Value));
                    }

                    ws.Cells[row, col].Value = valoresColumna.Max();
                    ws.Cells[row + 1, col].Value = valoresColumna.Min();
                    ws.Cells[row + 2, col].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valoresColumna.Average()));
                }

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 2, 16].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 2, 16].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 16; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }


        public static void GenerarArchivoGraficoComparativaVolumen(List<GraficoSeries> list, string cuencaNombre, string valorEmbalse, string ptomedicionNombre, int tipoptomedicion)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaVolumen);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaVolumen);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico Volumen");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                if (tipoptomedicion == 89) //CAUDAL EVAPORADO
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                else if (tipoptomedicion == 8) //CAUDAL NATURAL ESTIMADO
                {
                    ws.Cells[2, 1].Value = "RIO";
                }
                else //VOLUMEN UTIL
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }

                //ws.Cells[2, 1].Value = "EMBALSE";
                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 16])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "AÑO";
                ws.Cells[5, 2].Value = "ENE";
                ws.Cells[5, 3].Value = "FEB";
                ws.Cells[5, 4].Value = "MAR";
                ws.Cells[5, 5].Value = "ABR";
                ws.Cells[5, 6].Value = "MAY";
                ws.Cells[5, 7].Value = "JUN";
                ws.Cells[5, 8].Value = "JUL";
                ws.Cells[5, 9].Value = "AGO";
                ws.Cells[5, 10].Value = "SEP";
                ws.Cells[5, 11].Value = "OCT";
                ws.Cells[5, 12].Value = "NOV";
                ws.Cells[5, 13].Value = "DIC";
                ws.Cells[5, 14].Value = "Maximo";
                ws.Cells[5, 15].Value = "Minimo";
                ws.Cells[5, 16].Value = "Promedio";

                int row = 6;
                foreach (var item in list)
                {
                    ws.Cells[row, 1].Value = item.Anio;
                    ws.Cells[row, 2].Value = item.M1;
                    ws.Cells[row, 3].Value = item.M2;
                    ws.Cells[row, 4].Value = item.M3;
                    ws.Cells[row, 5].Value = item.M4;
                    ws.Cells[row, 6].Value = item.M5;
                    ws.Cells[row, 7].Value = item.M6;
                    ws.Cells[row, 8].Value = item.M7;
                    ws.Cells[row, 9].Value = item.M8;
                    ws.Cells[row, 10].Value = item.M9;
                    ws.Cells[row, 11].Value = item.M10;
                    ws.Cells[row, 12].Value = item.M11;
                    ws.Cells[row, 13].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    ws.Cells[row, 14].Value = valores.Max();
                    ws.Cells[row, 15].Value = valores.Min();
                    ws.Cells[row, 16].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));

                    row++;
                }

                // Agregar filas para Maximo, Minimo, Promedio
                ws.Cells[row, 1].Value = "Maximo";
                ws.Cells[row + 1, 1].Value = "Minimo";
                ws.Cells[row + 2, 1].Value = "Promedio";

                for (int col = 2; col <= 13; col++)
                {
                    var valoresColumna = new List<double>();
                    for (int fila = 6; fila < row; fila++)
                    {
                        valoresColumna.Add(Convert.ToDouble(ws.Cells[fila, col].Value));
                    }

                    ws.Cells[row, col].Value = valoresColumna.Max();
                    ws.Cells[row + 1, col].Value = valoresColumna.Min();
                    ws.Cells[row + 2, col].Value = Convert.ToDecimal((String.Format("{0:#,0.000}", valoresColumna.Average())));
                }

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 2, 16].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 2, 16].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 16; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }
        public static void GenerarArchivoGraficoComparativaNaturalEvaporada(List<GraficoSeries> list, List<GraficoSeries> listTotal, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaNaturalEvaporada);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaNaturalEvaporada);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico NaturalEvaporada");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                ws.Cells[2, 1].Value = "EMBALSE";
                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 16])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "AÑO";
                ws.Cells[5, 2].Value = "ENE";
                ws.Cells[5, 3].Value = "FEB";
                ws.Cells[5, 4].Value = "MAR";
                ws.Cells[5, 5].Value = "ABR";
                ws.Cells[5, 6].Value = "MAY";
                ws.Cells[5, 7].Value = "JUN";
                ws.Cells[5, 8].Value = "JUL";
                ws.Cells[5, 9].Value = "AGO";
                ws.Cells[5, 10].Value = "SEP";
                ws.Cells[5, 11].Value = "OCT";
                ws.Cells[5, 12].Value = "NOV";
                ws.Cells[5, 13].Value = "DIC";
                ws.Cells[5, 14].Value = "Maximo";
                ws.Cells[5, 15].Value = "Minimo";
                ws.Cells[5, 16].Value = "Promedio";

                int row = 6;
                foreach (var item in list)
                {
                    ws.Cells[row, 1].Value = item.Anio;
                    ws.Cells[row, 2].Value = item.M1;
                    ws.Cells[row, 3].Value = item.M2;
                    ws.Cells[row, 4].Value = item.M3;
                    ws.Cells[row, 5].Value = item.M4;
                    ws.Cells[row, 6].Value = item.M5;
                    ws.Cells[row, 7].Value = item.M6;
                    ws.Cells[row, 8].Value = item.M7;
                    ws.Cells[row, 9].Value = item.M8;
                    ws.Cells[row, 10].Value = item.M9;
                    ws.Cells[row, 11].Value = item.M10;
                    ws.Cells[row, 12].Value = item.M11;
                    ws.Cells[row, 13].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    ws.Cells[row, 14].Value = valores.Max();
                    ws.Cells[row, 15].Value = valores.Min();
                    ws.Cells[row, 16].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));

                    row++;
                }

                // Agregar filas para Maximo, Minimo, Promedio
                ws.Cells[row, 1].Value = "Maximo";
                ws.Cells[row + 1, 1].Value = "Minimo";
                ws.Cells[row + 2, 1].Value = "Promedio";

                var wsTotal = package.Workbook.Worksheets.Add("GraficoNaturalEvaporadaTotal");
                int rowTotal = 1;
                foreach (var item in listTotal)
                {
                    wsTotal.Cells[rowTotal, 1].Value = item.Anio;
                    wsTotal.Cells[rowTotal, 2].Value = item.M1;
                    wsTotal.Cells[rowTotal, 3].Value = item.M2;
                    wsTotal.Cells[rowTotal, 4].Value = item.M3;
                    wsTotal.Cells[rowTotal, 5].Value = item.M4;
                    wsTotal.Cells[rowTotal, 6].Value = item.M5;
                    wsTotal.Cells[rowTotal, 7].Value = item.M6;
                    wsTotal.Cells[rowTotal, 8].Value = item.M7;
                    wsTotal.Cells[rowTotal, 9].Value = item.M8;
                    wsTotal.Cells[rowTotal, 10].Value = item.M9;
                    wsTotal.Cells[rowTotal, 11].Value = item.M10;
                    wsTotal.Cells[rowTotal, 12].Value = item.M11;
                    wsTotal.Cells[rowTotal, 13].Value = item.M12;
                    rowTotal++;
                }

                for (int col = 2; col <= 13; col++)
                {
                    var valoresColumna = new List<double>();
                    for (int fila = 1; fila <= listTotal.Count; fila++)
                    {
                        valoresColumna.Add(Convert.ToDouble(wsTotal.Cells[fila, col].Value));
                    }

                    ws.Cells[row, col].Value = valoresColumna.Max();
                    ws.Cells[row + 1, col].Value = valoresColumna.Min();
                    ws.Cells[row + 2, col].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valoresColumna.Average()));
                }

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 2, 16].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 2, 16].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 16; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }


        public static void GenerarArchivoGraficoComparativaLineaTendencia(List<GraficoSeries> list, string cuencaNombre, string valorEmbalse, string ptomedicionNombre, int tipoptomedicion)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            List<int> ptoMedList = new List<int>();

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaLineaTendencia);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreGraficoComparativaLineaTendencia);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Gráfico LineaTendencia");
                // Agregar datos adicionales con formato
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[1, 2, 1, 3].Merge = true; // Fusionar celdas para CUENCA
                ws.Cells[1, 2].Value = ":" + cuencaNombre;

                //ws.Cells[2, 1].Value = "EMBALSE";
                if (tipoptomedicion == 89) //CAUDAL EVAPORADO
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                else if (tipoptomedicion == 8) //CAUDAL NATURAL ESTIMADO
                {
                    ws.Cells[2, 1].Value = "RIO";
                }
                else //VOLUMEN UTIL
                {
                    ws.Cells[2, 1].Value = "EMBALSE";
                }
                ws.Cells[2, 2, 2, 3].Merge = true; // Fusionar celdas para EMBALSE
                ws.Cells[2, 2].Value = ":" + valorEmbalse;

                ws.Cells[3, 1].Value = "NOMBRE DE PUNTO DE MEDICIÓN";
                ws.Cells[3, 2, 3, 3].Merge = true; // Fusionar celdas para NOMBRE DE PUNTO DE MEDICIÓN
                ws.Cells[3, 2].Value = ":" + ptomedicionNombre;

                // Fusionar celdas para el cuadro
                ws.Cells[1, 1, 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Ajustar alineación y fuente
                ws.Cells[1, 1, 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[1, 1, 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, 3, 3].Style.Font.Size = 12;
                ws.Cells[1, 1, 3, 3].Style.Font.Bold = true;

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 25;


                // Configurar encabezados de la tabla
                using (ExcelRange r = ws.Cells[5, 1, 5, 17])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                ws.Cells[5, 1].Value = "PTO.MED.";
                ws.Cells[5, 2].Value = "AÑO";
                ws.Cells[5, 3].Value = "ENE";
                ws.Cells[5, 4].Value = "FEB";
                ws.Cells[5, 5].Value = "MAR";
                ws.Cells[5, 6].Value = "ABR";
                ws.Cells[5, 7].Value = "MAY";
                ws.Cells[5, 8].Value = "JUN";
                ws.Cells[5, 9].Value = "JUL";
                ws.Cells[5, 10].Value = "AGO";
                ws.Cells[5, 11].Value = "SEP";
                ws.Cells[5, 12].Value = "OCT";
                ws.Cells[5, 13].Value = "NOV";
                ws.Cells[5, 14].Value = "DIC";
                ws.Cells[5, 15].Value = "Maximo";
                ws.Cells[5, 16].Value = "Minimo";
                ws.Cells[5, 17].Value = "Promedio";

                int row = 6;
                int numIndice = 1;
                foreach (var item in list)
                {
                    
                    ws.Cells[row, 1].Value = item.Ptomedielenomb;
                    ws.Cells[row, 2].Value = item.Anio;
                    ws.Cells[row, 3].Value = item.M1;
                    ws.Cells[row, 4].Value = item.M2;
                    ws.Cells[row, 5].Value = item.M3;
                    ws.Cells[row, 6].Value = item.M4;
                    ws.Cells[row, 7].Value = item.M5;
                    ws.Cells[row, 8].Value = item.M6;
                    ws.Cells[row, 9].Value = item.M7;
                    ws.Cells[row, 10].Value = item.M8;
                    ws.Cells[row, 11].Value = item.M9;
                    ws.Cells[row, 12].Value = item.M10;
                    ws.Cells[row, 13].Value = item.M11;
                    ws.Cells[row, 14].Value = item.M12;

                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    ws.Cells[row, 15].Value = valores.Max();
                    ws.Cells[row, 16].Value = valores.Min();
                    ws.Cells[row, 17].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));

                    row++;
                    
                    if (item.Anio==2023) //Ultimo Anio
                    {
                        // Agregar filas para Maximo, Minimo, Promedio
                        
                        for (int col = 2; col <= 13; col++)
                        {
                            var valoresColumna = new List<double>();
                            for (int fila = 6; fila < row; fila++)
                            {
                                valoresColumna.Add(Convert.ToDouble(ws.Cells[fila, col].Value));
                            }

                            ws.Cells[row, col].Value = valoresColumna.Max();
                            ws.Cells[row + 1, col].Value = valoresColumna.Min();
                            ws.Cells[row + 2, col].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valoresColumna.Average()));
                        }
                        ws.Cells[row, 1].Value = "Maximo";
                        row++;
                        ws.Cells[row, 1].Value = "Minimo";
                        row++;
                        ws.Cells[row, 1].Value = "Promedio";
                        row++;
                    }

                }

                

                // Formato de celdas
                var fontTabla = ws.Cells[5, 1, row + 2, 17].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                var border = ws.Cells[5, 1, row + 2, 17].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Column(1).Width = 25;
                for (int i = 2; i <= 17; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }

        }


        public static void GenerarArchivoTablaVertical(List<TablaVertical> listaTablaVertical, string fechaInicioStr, string fechaFinStr, int tptomedicodi)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreTablaVertical);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreTablaVertical);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Hoja1");

                // Encabezados
                ws.Cells[1, 1].Value = "CUENCA";
                ws.Cells[2, 1].Value = "EMPRESA";
                ws.Cells[3, 1].Value = "PUNTO DE MEDICIÓN";
                ws.Cells[4, 1].Value = "AÑO-MES / UNIDAD";

                // Convertir las cadenas de fecha a DateTime
                DateTime fechaInicio = DateTime.ParseExact(fechaInicioStr, "MM/yyyy", null);
                DateTime fechaFin = DateTime.ParseExact(fechaFinStr, "MM/yyyy", null);

                // Configurar la cultura en español
                CultureInfo culturaEspañol = new CultureInfo("es-ES");

                // Crear lista dinámica de meses en la primera columna
                int filaActual = 5; // Inicia en la fila 5
                DateTime fechaActual = fechaInicio;

                while (fechaActual <= fechaFin)
                {
                    ws.Cells[filaActual, 1].Value = fechaActual.ToString("yyyy-MMMM", culturaEspañol).ToUpper(); // Formato "AÑO-MES" en español
                    fechaActual = fechaActual.AddMonths(1);
                    filaActual++;
                }

                // Diccionario para rastrear las columnas ya usadas por cada Ptomedicodi
                var columnasPorPtoMedico = new Dictionary<int, int>();

                foreach (var item in listaTablaVertical)
                {
                    int columna;

                    // Verificar si el Ptomedicodi ya tiene una columna asignada
                    if (!columnasPorPtoMedico.TryGetValue(item.Ptomedicodi, out columna))
                    {
                        // Si no existe, asignamos una nueva columna
                        columna = columnasPorPtoMedico.Count + 2; // Comenzamos en la columna 2
                        columnasPorPtoMedico[item.Ptomedicodi] = columna;

                        // Colocamos los encabezados para la nueva columna
                        ws.Cells[1, columna].Value = item.Caudal;//Caudal
                        ws.Cells[2, columna].Value = item.Emprnomb;
                        ws.Cells[3, columna].Value = item.Ptomedielenomb;
                        if (tptomedicodi==7)
                        {
                            ws.Cells[4, columna].Value = "hm3";// Unidad
                        } else
                        {
                            ws.Cells[4, columna].Value = "m3/s";// Unidad
                        }
                       
                    }

                    // Calcular la fila inicial para el mes y año correspondiente al item
                    DateTime fechaItem = new DateTime(item.Anio, 1, 1);
                    int diferenciaMeses = ((item.Anio - fechaInicio.Year) * 12) + (fechaItem.Month - fechaInicio.Month);
                    int fila = 5 + diferenciaMeses;

                    // Llenar los valores de cada mes desde el mes correspondiente
                    if (fila >= 5 && fila < filaActual)
                    {
                        ws.Cells[fila, columna].Value = item.M1;
                        ws.Cells[fila + 1, columna].Value = item.M2;
                        ws.Cells[fila + 2, columna].Value = item.M3;
                        ws.Cells[fila + 3, columna].Value = item.M4;
                        ws.Cells[fila + 4, columna].Value = item.M5;
                        ws.Cells[fila + 5, columna].Value = item.M6;
                        ws.Cells[fila + 6, columna].Value = item.M7;
                        ws.Cells[fila + 7, columna].Value = item.M8;
                        ws.Cells[fila + 8, columna].Value = item.M9;
                        ws.Cells[fila + 9, columna].Value = item.M10;
                        ws.Cells[fila + 10, columna].Value = item.M11;
                        ws.Cells[fila + 11, columna].Value = item.M12;
                    }
                }

                // Formato y ajustes
                int totalColumnas = columnasPorPtoMedico.Count + 1;
                ws.Cells[1, 1, 4, totalColumnas].Style.Font.Bold = true;
                ws.Cells[1, 1, filaActual - 1, totalColumnas].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                // Aplicar formato de fondo azul y texto blanco a las tres primeras filas
                using (ExcelRange rng = ws.Cells[1, 1, 4, totalColumnas])
                {
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#305496"));
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                // Aplicar formato de fondo azul y texto blanco a la primera columna
                using (ExcelRange rng = ws.Cells[1, 1, filaActual - 1, 1])
                {
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#305496"));
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                // Ajustar ancho de columnas
                ws.Column(1).Width = 25;
                for (int i = 2; i <= totalColumnas; i++)
                {
                    ws.Column(i).Width = 15;
                }

                package.Save();
            }
        }




        public static void GenerarArchivoMatricesMensuales(List<TablaVertical> listaTablaVertical, List<MePtomedicionDTO> listaPuntoMedicion, int tptomedicodi)
        {
            string strTipoPuntoMed = string.Empty;
            string strEmbalseRio = string.Empty;
            if (tptomedicodi == 89) //CAUDAL EVAPORADO
            {
                strTipoPuntoMed = "CAUDAL EVAPORADO";
                strEmbalseRio = "EMBALSE";
            }
            else if (tptomedicodi == 8) //CAUDAL NATURAL ESTIMADO
            {
                strTipoPuntoMed = "CAUDAL NATURAL";
                strEmbalseRio = "RIO";
            }
            else //VOLUMEN UTIL
            {
                strTipoPuntoMed = "VOLUMEN UTIL";
                strEmbalseRio = "EMBALSE";
            }

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreMatricesMensuales);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreMatricesMensuales);
            }

            int numHojas = 0;
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                foreach (var itemPtoMedicion in listaPuntoMedicion)
                {
                    int CodPtoMed = itemPtoMedicion.Ptomedicodi;
                    List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == CodPtoMed).ToList();
                    int numRegistros = listaTablaVerticalPtoMed.Count();
                    if (numRegistros>0)
                    {
                        numHojas++;
                        var ws = package.Workbook.Worksheets.Add(itemPtoMedicion.Ptomedielenomb);

                        string imagePath = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderImageHidro + "logocoes.png";

                        var image = Image.FromFile(imagePath);
                        var picture = ws.Drawings.AddPicture("logo", image);
                        picture.SetPosition(0, 0, 1, 0);
                        picture.SetSize(150, 80); // Ajusta el tamaño de la imagen si es necesario
                                                    // Encabezado principal centrado y combinado
                        ws.Cells[1, 5].Value = strTipoPuntoMed + " " + itemPtoMedicion.Ptomedidesc;
                        ws.Cells[1, 5, 1, 10].Merge = true;
                        ws.Cells[1, 5, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Encabezados fijos
                        ws.Cells[6, 2].Value = "CUENCA:";
                        ws.Cells[6, 4].Value = itemPtoMedicion.EquiPadrenomb;

                        ws.Cells[7, 2].Value = "EMPRESA:";
                        ws.Cells[7, 4].Value = itemPtoMedicion.Emprnomb;

                        ws.Cells[8, 2].Value = strEmbalseRio + ":";
                        ws.Cells[8, 4].Value = itemPtoMedicion.Ptomedidesc;

                        ws.Cells[9, 2].Value = "PUNTO DE MEDICIÓN:";
                        ws.Cells[9, 4].Value = itemPtoMedicion.Ptomedielenomb;

                        ws.Cells[6, 7].Value = "ESTACIÓN:";
                        ws.Cells[6, 9].Value = itemPtoMedicion.Ptomedibarranomb;

                        ws.Cells[7, 7].Value = "COORDENADA X:";
                        ws.Cells[7, 9].Value = itemPtoMedicion.CoordenadaX;

                        ws.Cells[8, 7].Value = "COORDENADA Y:";
                        ws.Cells[8, 9].Value = itemPtoMedicion.CoordenadaY;

                        ws.Cells[9, 7].Value = "ALTITUD";
                        ws.Cells[9, 9].Value = itemPtoMedicion.Altitud +  " m.s.n.m";

                        // Encabezado de datos
                        ws.Cells[12, 2].Value = "AÑO";
                        ws.Cells[12, 3].Value = "ENE";
                        ws.Cells[12, 4].Value = "FEB";
                        ws.Cells[12, 5].Value = "MAR";
                        ws.Cells[12, 6].Value = "ABR";
                        ws.Cells[12, 7].Value = "MAY";
                        ws.Cells[12, 8].Value = "JUN";
                        ws.Cells[12, 9].Value = "JUL";
                        ws.Cells[12, 10].Value = "AGO";
                        ws.Cells[12, 11].Value = "SET";
                        ws.Cells[12, 12].Value = "OCT";
                        ws.Cells[12, 13].Value = "NOV";
                        ws.Cells[12, 14].Value = "DIC";
                        ws.Cells[12, 15].Value = "PROM";

                        // Llenar datos dinámicos
                        int currentRow = 13;
                        foreach (var tablaPtoMed in listaTablaVerticalPtoMed)
                        {
                            ws.Cells[currentRow, 2].Value = tablaPtoMed.Anio;
                            ws.Cells[currentRow, 3].Value = tablaPtoMed.M1;
                            ws.Cells[currentRow, 4].Value = tablaPtoMed.M2;
                            ws.Cells[currentRow, 5].Value = tablaPtoMed.M3;
                            ws.Cells[currentRow, 6].Value = tablaPtoMed.M4;
                            ws.Cells[currentRow, 7].Value = tablaPtoMed.M5;
                            ws.Cells[currentRow, 8].Value = tablaPtoMed.M6;
                            ws.Cells[currentRow, 9].Value = tablaPtoMed.M7;
                            ws.Cells[currentRow, 10].Value = tablaPtoMed.M8;
                            ws.Cells[currentRow, 11].Value = tablaPtoMed.M9;
                            ws.Cells[currentRow, 12].Value = tablaPtoMed.M10;
                            ws.Cells[currentRow, 13].Value = tablaPtoMed.M11;
                            ws.Cells[currentRow, 14].Value = tablaPtoMed.M12;
                           
                            var valores = new decimal?[] { tablaPtoMed.M1, tablaPtoMed.M2, tablaPtoMed.M3, tablaPtoMed.M4, tablaPtoMed.M5, tablaPtoMed.M6, tablaPtoMed.M7, tablaPtoMed.M8, tablaPtoMed.M9, tablaPtoMed.M10, tablaPtoMed.M11, tablaPtoMed.M12 };
                            ws.Cells[currentRow, 15].Value = Convert.ToDecimal(String.Format("{0:#,0.000}", valores.Average()));
                            currentRow++;
                        }

                        // Ajustar formato de celdas
                        ws.Cells[6, 2, 9, 4].Style.Font.Bold = true;
                        ws.Cells[6, 7, 9, 9].Style.Font.Bold = true;
                        ws.Cells[12, 2, currentRow - 1, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        ws.Cells[12, 15, currentRow - 1, 15].Style.Font.Bold = true;


                        // Ajustar ancho de columnas
                        ws.Column(2).Width = 25;
                        for (int i = 2; i <= 15; i++)
                        {
                            ws.Column(i).Width = 15;
                        }
                        
                    }
                }
                if (numHojas==0)
                {
                    var ws = package.Workbook.Worksheets.Add("HOJA1");
                }
                package.Save();


            }
        }

        public static void GenerarArchivoModeloPerseo(List<TablaVertical> listaTablaVertical, List<MePtomedicionDTO> listaPuntoMedicion, int tptomedicodi)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            FileInfo newFile = new FileInfo(ruta + ConstantesHidrologia.NombreModeloPerseo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + ConstantesHidrologia.NombreModeloPerseo);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var ws = package.Workbook.Worksheets.Add("Hoja1");

                // Insertar los encabezados en la tercera fila
                if (listaPuntoMedicion.Count>0)
                {
                    ws.Cells[3, 2].Value = listaPuntoMedicion[0].EquiPadrenomb;

                    // Insertar los títulos de las columnas en la cuarta fila
                    ws.Cells[4, 1].Value = "CODIGO";
                    ws.Cells[4, 2].Value = "AÑO";
                    ws.Cells[4, 3].Value = "ENE";
                    ws.Cells[4, 4].Value = "FEB";
                    ws.Cells[4, 5].Value = "MAR";
                    ws.Cells[4, 6].Value = "ABR";
                    ws.Cells[4, 7].Value = "MAY";
                    ws.Cells[4, 8].Value = "JUN";
                    ws.Cells[4, 9].Value = "JUL";
                    ws.Cells[4, 10].Value = "AGO";
                    ws.Cells[4, 11].Value = "SET";
                    ws.Cells[4, 12].Value = "OCT";
                    ws.Cells[4, 13].Value = "NOV";
                    ws.Cells[4, 14].Value = "DIC";

                    int currentRow = 5;

                    foreach (var itemPtoMedicion in listaPuntoMedicion)
                    {
                        int CodPtoMed = itemPtoMedicion.Ptomedicodi;
                        List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == CodPtoMed).ToList();
                        int numRegistros = listaTablaVerticalPtoMed.Count();
                        if (numRegistros > 0)
                        {
                            // Insertar la descripción del punto de medición en la fila debajo de "AÑO"
                            ws.Cells[currentRow, 2].Value = itemPtoMedicion.Ptomedidesc;
                            ws.Cells[currentRow, 2, currentRow, 14].Merge = true;
                            ws.Cells[currentRow, 2, currentRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[currentRow, 1, currentRow, 14].Style.Font.Bold = true;

                            // Llenar datos dinámicos
                            currentRow++;
                            foreach (var tablaPtoMed in listaTablaVerticalPtoMed)
                            {
                                ws.Cells[currentRow, 1].Value = itemPtoMedicion.Ptomedielenomb;
                                ws.Cells[currentRow, 2].Value = tablaPtoMed.Anio;
                                ws.Cells[currentRow, 3].Value = tablaPtoMed.M1;
                                ws.Cells[currentRow, 4].Value = tablaPtoMed.M2;
                                ws.Cells[currentRow, 5].Value = tablaPtoMed.M3;
                                ws.Cells[currentRow, 6].Value = tablaPtoMed.M4;
                                ws.Cells[currentRow, 7].Value = tablaPtoMed.M5;
                                ws.Cells[currentRow, 8].Value = tablaPtoMed.M6;
                                ws.Cells[currentRow, 9].Value = tablaPtoMed.M7;
                                ws.Cells[currentRow, 10].Value = tablaPtoMed.M8;
                                ws.Cells[currentRow, 11].Value = tablaPtoMed.M9;
                                ws.Cells[currentRow, 12].Value = tablaPtoMed.M10;
                                ws.Cells[currentRow, 13].Value = tablaPtoMed.M11;
                                ws.Cells[currentRow, 14].Value = tablaPtoMed.M12;
                                currentRow++;
                            }

                            // Ajustar formato de celdas
                            ws.Cells[3, 1, 3, 14].Style.Font.Bold = true;
                            ws.Cells[4, 1, 4, 14].Style.Font.Bold = true;
                            ws.Cells[3, 1, currentRow - 1, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                            // Ajustar el ancho de las columnas
                            for (int i = 1; i <= 14; i++)
                            {
                                ws.Column(i).AutoFit();
                            }

                        }
                    }
                }
                

                package.Save();
            }
        }

    }
}