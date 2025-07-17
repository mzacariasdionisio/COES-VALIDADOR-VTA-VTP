using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Eventos.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
{
    public class ExcelGrafico
    {

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
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(2); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(2);//Two pixel space for better alignment
                picture.SetSize(90, 40);
            }
        }

        public static void ConfiguraEncabezadoHojaExcel(ExcelWorksheet ws, string titulo)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();

            UtilExcel.SetearFondoBlanco(ws);
            ws.Cells[1, 3].Value = titulo;
            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";

            //Borde cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            //Ancho de Columnas y Filas
            ws.Row(1).Height = 30;
            ws.Row(2).Height = 10;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 23;
            ws.Column(3).Width = 12;
            //Imagen
            AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);


        }
        //public static void ConfiguraEncabezadoHojaExcel2(ExcelWorksheet ws, string titulo)
        //{
        //    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
        //    UtilExcel.SetearFondoBlanco(ws);
        //    ws.Cells[1, 4].Value = titulo;
        //    var font = ws.Cells[1, 4].Style.Font;
        //    font.Size = 16;
        //    font.Bold = true;
        //    font.Name = "Calibri";
        //    var fontTabla = ws.Cells[3, 2, 3, 3].Style.Font;
        //    fontTabla.Size = 8;
        //    fontTabla.Name = "Calibri";
        //    fontTabla.Bold = true;
        //    ws.Cells[3, 2].Value = "FECHA:";
        //    ws.Cells[3, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
        //    AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);
        //}

        public static void AddGraficoPie(ExcelWorksheet ws, int row)
        {
            var pieChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.PieExploded3D) as ExcelPieChart;
            //Set top left corner to row 1 column 2
            pieChart.SetPosition(3, 0, 4, 0);
            pieChart.SetSize(800, 600);
            pieChart.Series.Add(ExcelRange.GetAddress(7, 3, row, 3), ExcelRange.GetAddress(7, 2, row, 2));

            //pieChart.Title.Text = "Mantenimientos Ejecutados";
            //Set datalabels and remove the legend
            pieChart.DataLabel.ShowCategory = true;
            pieChart.DataLabel.ShowPercent = true;
            pieChart.DataLabel.ShowLeaderLines = true;
            pieChart.Legend.Remove();
        }

        public static void AddGraficoColumn(ExcelWorksheet ws, int rowini, int colini, int rows, int cols)
        {
            var colChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.ColumnStacked);
            //Set top left corner to row 1 column 2
            colChart.SetPosition(rowini + 1 + rows, 0, 1, 0);
            colChart.SetSize(1000, 400);
            List<ExcelChartSerie> series = new List<ExcelChartSerie>();
            for (var i = 0; i < rows; i++)
            {

                series.Add(colChart.Series.Add(ExcelRange.GetAddress(rowini + i, colini, rowini + i, colini + cols), ExcelRange.GetAddress(rowini - 1, colini, rowini - 1, colini + cols)));
                series[i].Header = ws.Cells[rowini + i, colini - 1].Value.ToString();

            }
            //pieChart.Title.Text = "Mantenimientos Ejecutados";
            //Set datalabels and remove the legend
            //pieChart.DataLabel.ShowCategory = true;
            //pieChart.DataLabel.ShowPercent = true;
            //pieChart.DataLabel.ShowLeaderLines = true;
            //colChart.Legend.Remove();
        }

        public static void AddGraficoBar(ExcelWorksheet ws, int rowini, int colini, int rows, int cols)
        {
            var colChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.BarStacked);
            //Set top left corner to row 1 column 2
            colChart.SetPosition(rowini + 1 + rows, 0, 1, 0);
            colChart.SetSize(1000, 400);
            List<ExcelChartSerie> series = new List<ExcelChartSerie>();
            for (var i = 0; i < rows; i++)
            {

                series.Add(colChart.Series.Add(ExcelRange.GetAddress(rowini + i, colini, rowini + i, colini + cols), ExcelRange.GetAddress(rowini - 1, colini, rowini - 1, colini + cols)));
                series[i].Header = ws.Cells[rowini + i, colini - 1].Value.ToString();

            }
        }

        public static void ConfiguracionHojaMantenimiento(ExcelWorksheet ws)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();

            int fil = 6;
            var fill = ws.Cells[fil, 2, fil, 17].Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            //Borde cabecera de Tabla Listado
            var border = ws.Cells[fil, 2, fil, 17].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //Borde cabecera de Tabla Fecha
            var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;

            UtilExcel.SetearFondoBlanco(ws);

            using (ExcelRange r = ws.Cells["B6:Q6"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

            }

            ws.Cells[fil, 2].Value = "MANTENIMIENTO"; ws.Cells[6, 3].Value = "TIPO EMPRESA";
            ws.Cells[fil, 4].Value = "EMPRESA"; ws.Cells[6, 5].Value = "UBICACIÓN";
            ws.Cells[fil, 6].Value = "TIPO EQUIPO"; ws.Cells[6, 7].Value = "EQUIPO";
            ws.Cells[fil, 8].Value = "INICIO"; ws.Cells[6, 9].Value = "FINAL";
            ws.Cells[fil, 10].Value = "DESCRIPCION"; ws.Cells[6, 11].Value = "PROG";
            ws.Cells[fil, 12].Value = "INTERRUPCIÓN"; ws.Cells[6, 13].Value = "INDISPONIBILIDAD";
            ws.Cells[fil, 14].Value = "TENSIÓN"; ws.Cells[6, 15].Value = "TIPO MANTENIMIENTO";
            ws.Cells[fil, 16].Value = "CodEq"; ws.Cells[6, 17].Value = "TipoEq_Osinerg";

            ws.Row(1).Height = 30;
            ws.Row(2).Height = 10;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20; ws.Column(3).Width = 20; ws.Column(4).Width = 30; ws.Column(5).Width = 28;
            ws.Column(6).Width = 25; ws.Column(7).Width = 15; ws.Column(8).Width = 18; ws.Column(9).Width = 18;
            ws.Column(10).Width = 38; ws.Column(11).Width = 18; ws.Column(12).Width = 15; ws.Column(13).Width = 15;
            ws.Column(14).Width = 15; ws.Column(15).Width = 20; ws.Column(16).Width = 10; ws.Column(17).Width = 20;
            ws.View.FreezePanes(7, 1);
            AddImage(ws, 1, 0, ruta + Constantes.NombreLogoCoes);

        }

        public static void AplicarFormatoFila(ExcelWorksheet ws, int row, int col, int ncol)
        {
            var border = ws.Cells[row, col, row, col + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[row, col, row, col + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            //fontTabla.Color.SetColor(Color.FromArgb(51, 102, 255));

        }

        public static void GernerarArchivoMantenimiento(List<EveManttoDTO> listaManttos, string fecIni, string fecFin)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaExcelMantenimiento);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento);
            }
            int row = 7;
            int column = 2;
            var tipoManto = -1;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                foreach (var reg in listaManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        ConfiguracionHojaMantenimiento(ws);
                        row = 7;
                        ws.Cells[1, 3].Value = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = fecIni;
                        ws.Cells[4, 3].Value = fecFin;
                        var font = ws.Cells[1, 3].Style.Font;
                        font.Size = 16;
                        font.Bold = true;
                        font.Name = "Calibri";

                    }
                    tipoManto = (int)reg.Evenclasecodi;

                    ws.Cells[row, column].Value = reg.Evenclasedesc;
                    ws.Cells[row, column + 1].Value = reg.Tipoemprdesc.ToString();
                    ws.Cells[row, column + 2].Value = reg.Emprnomb.ToString();
                    ws.Cells[row, column + 3].Value = reg.Areanomb;
                    ws.Cells[row, column + 4].Value = reg.Famnomb;
                    ws.Cells[row, column + 5].Value = reg.Equiabrev;
                    ws.Cells[row, column + 6].Value = ((DateTime)reg.Evenini).ToString(Constantes.FormatoFechaHora);
                    ws.Cells[row, column + 7].Value = ((DateTime)reg.Evenfin).ToString(Constantes.FormatoFechaHora);
                    ws.Cells[row, column + 8].Value = reg.Evendescrip;
                    var tipoProg = string.Empty;
                    switch (reg.Eventipoprog)
                    {
                        case "P":
                            tipoProg = "PROGRAMADO";
                            break;
                        case "R":
                            tipoProg = "REPROGRAMADO";
                            break;
                        case "N":
                            tipoProg = "NO PROGRAMADO";
                            break;
                        case "F":
                            tipoProg = "FORZADO";
                            break;
                        default:
                            tipoProg = reg.Eventipoprog;
                            break;
                    }
                    ws.Cells[row, column + 9].Value = tipoProg;
                    ws.Cells[row, column + 10].Value = reg.Eveninterrup;
                    var indisponibilidad = string.Empty;
                    switch (reg.Evenindispo)
                    {
                        case "F":
                            indisponibilidad = "F/S";
                            break;
                        case "E":
                            indisponibilidad = "E/S";
                            break;
                        default:
                            indisponibilidad = reg.Evenindispo;
                            break;
                    }
                    ws.Cells[row, column + 11].Value = indisponibilidad;
                    ws.Cells[row, column + 12].Value = reg.Equitension.ToString();

                    ws.Cells[row, column + 13].Value = reg.Tipoevenabrev;
                    ws.Cells[row, column + 14].Value = reg.Equicodi.ToString();
                    ws.Cells[row, column + 15].Value = reg.Osigrupocodi.ToString();
                    var border = ws.Cells[row, 2, row, 17].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 17].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";

                    row++;
                }

                xlPackage.Save();
            }
        }

        //genera graficos de mantenimiento empresas.  Grafico 01
        public static void GenerarArchivoMantoEmpresa(MantenimientoModel model)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento01);
            int row = 7;
            var tipoManto = -1;
            string titulo = "";

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento01);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1)
                            AddGraficoPie(ws, row - 1);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;

                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;

                        ws.Cells[row - 1, 2].Value = "EMPRESA";
                        ws.Cells[row - 1, 3].Value = "TOTAL";

                        //Borde cabecera de Tabla Listado
                        var border = ws.Cells[row - 1, 2, row - 1, 3].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, 3])
                        {
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

                        }

                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    ws.Cells[row, 2].Value = reg.Emprnomb;
                    ws.Cells[row, 3].Value = reg.Porcentajemantto;
                    AplicarFormatoFila(ws, row, 2, 1);
                    row++;
                }
                AddGraficoPie(ws, row - 1);
                xlPackage.Save();
            }
        }
        //genera graficos de mantenimento equipos. Grafico 02
        public static void GenerarArchivoMantoEquipo(MantenimientoModel model)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento02);
            int row = 7;

            var tipoManto = -1;
            string titulo = "";

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento02);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1)
                            AddGraficoPie(ws, row - 1);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;
                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;
                        //Borde cabecera de Tabla Listado
                        var border = ws.Cells[row - 1, 2, row - 1, 3].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, 3])
                        {
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

                        }

                        ws.Cells[row - 1, 2].Value = "TIPO EQUIPO";
                        ws.Cells[row - 1, 3].Value = "TOTAL";
                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    ws.Cells[row, 2].Value = reg.Famnomb;
                    ws.Cells[row, 3].Value = reg.Porcentajemantto;
                    AplicarFormatoFila(ws, row, 2, 1);
                    row++;
                }
                AddGraficoPie(ws, row - 1);
                xlPackage.Save();
            }

        }
        // grafico 03
        /// <summary>
        /// Lista que contiene el orden de las columnas para generar el grafico Pareto
        /// </summary>
        /// <returns></returns>
        public static List<int> PatronOrden(int[][] matriz)
        {

            List<int> totales = new List<int>();
            List<int> orden = new List<int>();
            int piv = 0;
            for (var z = 0; z < matriz.Length; z++)
            {
                orden.Add(z);
                totales.Add(0);
            }
            for (int i = 0; i < matriz.Length; i++)
            {
                for (var j = 0; j < matriz[i].Length; j++)
                    totales[i] += matriz[i][j];
            }
            for (var i = 0; i < totales.Count(); i++)
                for (var j = 0; j < totales.Count() - 1 - i; j++)
                {
                    if (totales[j] < totales[j + 1])
                    {
                        piv = totales[j + 1];
                        totales[j + 1] = totales[j];
                        totales[j] = piv;
                        piv = orden[j + 1];
                        orden[j + 1] = orden[j];
                        orden[j] = piv;
                    }
                }

            return orden;
        }

        public static void OrdenarMatriz(List<int> orden, int[][] matriz)
        {
            int[] matrizaux;

            int nseries = matriz[0].Length;
            for (int i = 0; i < nseries; i++)
            {
                matrizaux = new int[matriz.Length];
                for (var z = 0; z < matrizaux.Length; z++)
                    matrizaux[z] = matriz[z][i];

                for (var j = 0; j < matriz.Length; j++)
                    matriz[j][i] = matrizaux[orden[j]];
            }
        }

        public static void OrdenarAxis(List<int> orden, List<string> axisx)
        {
            List<string> axisxaux = new List<string>();
            foreach (var reg in axisx)
                axisxaux.Add(reg);
            for (var i = 0; i < axisx.Count; i++)
                axisx[i] = axisxaux[orden[i]];

        }


        public static void AddBodyMantoEmpresaEquipo1(ExcelWorksheet ws, List<string> empresas, List<string> mantos,
            List<ReporteManttoDTO> valores)
        {
            int row = 7;
            int col = 2;
            int[][] matriz;
            int i = empresas.Count();
            //Borde cabecera de Tabla Listado
            var border = ws.Cells[row - 1, col, row - 1, col + i].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[row - 1, col, row - 1, col + i])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

            }

            AplicarFormatoFila(ws, row - 1, 3, i - 1);
            int j = mantos.Count();
            matriz = new int[i][];

            for (var z = 0; z < i; z++)
            {
                matriz[z] = new int[j];
                for (var k = 0; k < j; k++)
                {
                    if (z == 0) ws.Cells[row + k, 2].Value = mantos[k];
                    matriz[z][k] = 0;
                }
            }

            ExcelHelper.SetFormatCells(ws, row, 2, row - 1 + j, 2, 8, "Calibri", Color.Black);
            foreach (var v in valores)
            {
                var posi = empresas.IndexOf(v.Emprabrev);
                var posj = mantos.IndexOf(v.Tipoevendesc);
                matriz[posi][posj] = v.Subtotal;
            }
            var orden = PatronOrden(matriz);
            OrdenarMatriz(orden, matriz);
            OrdenarAxis(orden, empresas);

            for (var z = 0; z < i; z++)
            {
                //Escribir abreviatura de empresas en la cabecera de la tabla de datos en el excel
                ws.Cells[row - 1, 3 + z].Value = empresas[z];
                for (var k = 0; k < j; k++)
                {
                    //Escribir tabla de datos en el excel
                    ws.Cells[row + k, 3 + z].Value = matriz[z][k];
                    AplicarFormatoFila(ws, row + k, 3, i - 1);
                }
            }
            AddGraficoColumn(ws, row, 3, j, empresas.Count() - 1);
            empresas.Clear();
            mantos.Clear();
            valores.Clear();
        }
        public static void GenerarArchivoMantoEmpresaEquipo1(MantenimientoModel model)
        {

            int row = 7;
            List<string> empresas = new List<string>();
            List<string> mantos = new List<string>();
            List<ReporteManttoDTO> valores = new List<ReporteManttoDTO>();
            var tipoManto = -1;
            string titulo = "";
            // Definicion del Archivo excel
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento03);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento03);
            }
            //
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                //Encontramos todas las empresas
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1) AddBodyMantoEmpresaEquipo1(ws, empresas, mantos, valores);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;

                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;
                        ws.Cells[row - 1, 2].Value = "TIPO MANTO/EMPRESAS";

                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    var algo = empresas.Find(x => x == reg.Emprabrev);
                    if (empresas.Find(x => x == reg.Emprabrev) == null) empresas.Add(reg.Emprabrev);
                    if (mantos.Find(x => x == reg.Tipoevendesc) == null) mantos.Add(reg.Tipoevendesc);
                    valores.Add(new ReporteManttoDTO()
                    {
                        Emprabrev = reg.Emprabrev,
                        Tipoevendesc = reg.Tipoevendesc,
                        Subtotal = reg.Subtotal
                    });
                    row++;
                }
                if (tipoManto != -1) AddBodyMantoEmpresaEquipo1(ws, empresas, mantos, valores);
                xlPackage.Save();
            }

        }

        // grafico 4
        public static void AddBodyMantoEmpresaEquipo2(ExcelWorksheet ws, List<string> empresas, List<string> tipoequipos,
                    List<ReporteManttoDTO> valores)
        {
            int row = 7;
            int col = 2;
            int[][] matriz;
            int i = empresas.Count();
            //Borde cabecera de Tabla Listado
            var border = ws.Cells[row - 1, col, row - 1, col + i].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[row - 1, col, row - 1, col + i])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

            }

            AplicarFormatoFila(ws, row - 1, 3, i - 1);
            int j = tipoequipos.Count();
            matriz = new int[i][];

            for (var z = 0; z < i; z++)
            {
                //ws.Cells[row-1, 3 + z].Value = empresas[z];
                matriz[z] = new int[j];
                for (var k = 0; k < j; k++)
                {
                    if (z == 0) ws.Cells[row + k, 2].Value = tipoequipos[k];
                    matriz[z][k] = 0;
                }
            }

            ExcelHelper.SetFormatCells(ws, row, 2, row - 1 + j, 2, 8, "Calibri", Color.Black);
            foreach (var v in valores)
            {
                var posi = empresas.IndexOf(v.Emprabrev);
                var posj = tipoequipos.IndexOf(v.Famnomb);
                matriz[posi][posj] = v.Subtotal;
            }
            var orden = PatronOrden(matriz);
            OrdenarMatriz(orden, matriz);
            OrdenarAxis(orden, empresas);
            for (var z = 0; z < i; z++)
            {
                ws.Cells[row - 1, 3 + z].Value = empresas[z];
                for (var k = 0; k < j; k++)
                {
                    ws.Cells[row + k, 3 + z].Value = matriz[z][k];
                    AplicarFormatoFila(ws, row + k, 3, i - 1);
                }
            }
            AddGraficoColumn(ws, row, 3, j, empresas.Count() - 1);
            empresas.Clear();
            tipoequipos.Clear();
            valores.Clear();
        }

        public static void GenerarArchivoMantoEmpresaEquipo2(MantenimientoModel model)
        {
            // Definicion del Archivo excel
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento04);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento04);
            }
            //  
            var tipoManto = -1;
            int row = 7;
            List<string> empresas = new List<string>();
            List<string> tipoequipos = new List<string>();
            List<ReporteManttoDTO> valores = new List<ReporteManttoDTO>();
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                //Encontramos todas las empresas
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1) AddBodyMantoEmpresaEquipo2(ws, empresas, tipoequipos, valores);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        string titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;
                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;
                        ws.Cells[row - 1, 2].Value = "TIPO EQUIPO/EMPRESAS";

                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    var algo = empresas.Find(x => x == reg.Emprabrev);
                    if (empresas.Find(x => x == reg.Emprabrev) == null) empresas.Add(reg.Emprabrev);
                    if (tipoequipos.Find(x => x == reg.Famnomb) == null) tipoequipos.Add(reg.Famnomb);
                    valores.Add(new ReporteManttoDTO()
                    {
                        Emprabrev = reg.Emprabrev,
                        Famnomb = reg.Famnomb,
                        Subtotal = reg.Subtotal
                    });
                    row++;
                }
                if (tipoManto != -1) AddBodyMantoEmpresaEquipo2(ws, empresas, tipoequipos, valores);
                xlPackage.Save();
            }
        }

        // grafica 5
        public static void AddBodyMantoEquipo2(ExcelWorksheet ws, List<string> tipoequipos, List<string> mantos,
                    List<ReporteManttoDTO> valores)
        {
            int row = 7;
            int col = 2;
            int[][] matriz;
            int i = tipoequipos.Count();
            //Borde cabecera de Tabla Listado
            var border = ws.Cells[row - 1, col, row - 1, col + i].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[row - 1, col, row - 1, col + i])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

            }
            AplicarFormatoFila(ws, row - 1, 3, i - 1);

            int j = mantos.Count();
            matriz = new int[i][];

            for (var z = 0; z < i; z++)
            {
                matriz[z] = new int[j];
                for (var k = 0; k < j; k++)
                {
                    if (z == 0) ws.Cells[row + k, 2].Value = mantos[k];
                    matriz[z][k] = 0;
                }
            }

            ExcelHelper.SetFormatCells(ws, row, 2, row - 1 + j, 2, 8, "Calibri", Color.Black);
            foreach (var v in valores)
            {
                var posi = tipoequipos.IndexOf(v.Famnomb);
                var posj = mantos.IndexOf(v.Tipoevendesc);
                matriz[posi][posj] = v.Subtotal;
            }
            var orden = PatronOrden(matriz);
            OrdenarMatriz(orden, matriz);
            OrdenarAxis(orden, tipoequipos);

            for (var z = 0; z < i; z++)
            {
                ws.Cells[row - 1, 3 + z].Value = tipoequipos[z];
                for (var k = 0; k < j; k++)
                {
                    ws.Cells[row + k, 3 + z].Value = matriz[z][k];
                    AplicarFormatoFila(ws, row + k, 3, i - 1);
                }
            }
            AddGraficoBar(ws, row, 3, j, tipoequipos.Count() - 1);
            tipoequipos.Clear();
            mantos.Clear();
            valores.Clear();
        }

        public static void GenerarArchivoMantoEquipo2(MantenimientoModel model)
        {
            // Definicion del Archivo excel
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento05);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento05);
            }
            //  
            var tipoManto = -1;
            int row = 7;
            List<string> tipoequipos = new List<string>();
            List<string> mantos = new List<string>();
            List<ReporteManttoDTO> valores = new List<ReporteManttoDTO>();
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                //Encontramos todas las empresas
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1) AddBodyMantoEquipo2(ws, tipoequipos, mantos, valores);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        string titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;

                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;
                        ws.Cells[row - 1, 2].Value = "TIPO MANTO/EMPRESAS";

                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    if (tipoequipos.Find(x => x == reg.Famnomb) == null) tipoequipos.Add(reg.Famnomb);
                    if (mantos.Find(x => x == reg.Tipoevendesc) == null) mantos.Add(reg.Tipoevendesc);
                    valores.Add(new ReporteManttoDTO()
                    {
                        Famnomb = reg.Famnomb,
                        Tipoevendesc = reg.Tipoevendesc,
                        Subtotal = reg.Subtotal
                    });
                    row++;
                }
                if (tipoManto != -1) AddBodyMantoEquipo2(ws, tipoequipos, mantos, valores);
                xlPackage.Save();
            }
        }

        // grafica 6

        public static void AddBodyMantoTipoMantoTipoEmpresa(ExcelWorksheet ws, List<string> mantos,
                    List<string> tipoempresas, List<ReporteManttoDTO> valores)
        {
            int row = 7;
            int col = 2;
            int[][] matriz;
            int i = mantos.Count();
            //Borde cabecera de Tabla Listado
            var border = ws.Cells[row - 1, col, row - 1, col + i].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[row - 1, col, row - 1, col + i])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

            }
            AplicarFormatoFila(ws, row - 1, 3, i - 1);
            int j = tipoempresas.Count();
            matriz = new int[i][];

            for (var z = 0; z < i; z++)
            {
                matriz[z] = new int[j];
                for (var k = 0; k < j; k++)
                {
                    if (z == 0) ws.Cells[row + k, 2].Value = tipoempresas[k];
                    matriz[z][k] = 0;
                }
            }

            ExcelHelper.SetFormatCells(ws, row, 2, row - 1 + j, 2, 8, "Calibri", Color.Black);
            foreach (var v in valores)
            {
                var posi = mantos.IndexOf(v.Tipoevendesc);
                var posj = tipoempresas.IndexOf(v.Tipoemprdesc);
                matriz[posi][posj] = v.Subtotal;
            }
            var orden = PatronOrden(matriz);
            OrdenarMatriz(orden, matriz);
            OrdenarAxis(orden, mantos);
            for (var z = 0; z < i; z++)
            {
                ws.Cells[row - 1, 3 + z].Value = mantos[z];
                for (var k = 0; k < j; k++)
                {
                    ws.Cells[row + k, 3 + z].Value = matriz[z][k];
                    AplicarFormatoFila(ws, row + k, 3, i - 1);
                }
            }
            AddGraficoColumn(ws, row, 3, j, mantos.Count() - 1);
            tipoempresas.Clear();
            mantos.Clear();
            valores.Clear();
        }

        public static void GenerarArchivoTipoMantoTipoEmpresa(MantenimientoModel model)
        {
            // Definicion del Archivo excel
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento].ToString();
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento06);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteMantenimiento06);
            }
            //  
            var tipoManto = -1;
            int row = 7;
            List<string> mantos = new List<string>();
            List<string> tipoempresas = new List<string>();
            List<ReporteManttoDTO> valores = new List<ReporteManttoDTO>();
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                //Encontramos todas las empresas
                foreach (var reg in model.ListarReporteManttos)
                {
                    if (tipoManto != reg.Evenclasecodi)
                    {
                        if (tipoManto != -1) AddBodyMantoTipoMantoTipoEmpresa(ws, mantos, tipoempresas, valores);

                        ws = xlPackage.Workbook.Worksheets.Add(reg.Evenclasedesc);
                        ws = xlPackage.Workbook.Worksheets[reg.Evenclasedesc];
                        row = 7;
                        string titulo = "REPORTE DE MANTENIMIENTOS " + reg.Evenclasedesc;
                        ConfiguraEncabezadoHojaExcel(ws, titulo);
                        ws.Cells[3, 2].Value = "Fecha Inicio:";
                        ws.Cells[4, 2].Value = "Fecha Fin:";
                        ws.Cells[3, 3].Value = model.FechaInicio;
                        ws.Cells[4, 3].Value = model.FechaFin;
                        ws.Cells[row - 1, 2].Value = "TIPO MANTO/TIPO EMPRESA";

                    }
                    tipoManto = (int)reg.Evenclasecodi;
                    if (mantos.Find(x => x == reg.Tipoevendesc) == null) mantos.Add(reg.Tipoevendesc);
                    if (tipoempresas.Find(x => x == reg.Tipoemprdesc) == null) tipoempresas.Add(reg.Tipoemprdesc);

                    valores.Add(new ReporteManttoDTO()
                    {
                        Tipoemprdesc = reg.Tipoemprdesc,
                        Tipoevendesc = reg.Tipoevendesc,
                        Subtotal = reg.Subtotal
                    });
                    row++;
                }
                if (tipoManto != -1) AddBodyMantoTipoMantoTipoEmpresa(ws, mantos, tipoempresas, valores);
                xlPackage.Save();
            }
        }

    }

    public class UtilExcel
    {

        public static void SetearFondoBlanco(ExcelWorksheet ws)
        {
            //Setear la hoja a fondo blanco
            using (ExcelRange r = ws.Cells["A1:BA300"])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.White);
                r.Style.Font.Color.SetColor(Color.Black);
            }
        }
    }
}