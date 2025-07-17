using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Configura los encabezados y titulos del reporte excel
        /// </summary>
        /// <param name="ws"></param>
        public static void ConfiguracionHojaExcel(ExcelWorksheet ws, List<string> listaFechas)
        {

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
            int numCeldaFinal = listaFechas.Count + 3;
            var fill = ws.Cells[6, 2, 6, numCeldaFinal].Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);
            var border = ws.Cells[6, 2, 6, numCeldaFinal].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            using (ExcelRange r = ws.Cells[6, 2, 6, numCeldaFinal])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[6, 2].Value = "EQUIPO";
            ws.Row(1).Height = 30;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 20;

            int numCeldaInicio = 3;
            foreach (var fecha in listaFechas)
            {
                ws.Cells[6, numCeldaInicio].Value = fecha;
                ws.Column(numCeldaInicio).Width = 15;
                numCeldaInicio++;
            }
            ws.Cells[6, numCeldaInicio].Value = "TOTAL";
            ws.Column(numCeldaInicio).Width = 15;


        }
        /// <summary>
        /// Genera listado de Reporte de segundos faltantes de archivo excel
        /// </summary>
        /// <param name="model"></param>
        public static void GernerarArchivoReporteSegundosFaltantes(ReporteSegundosFaltantesModel model)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
            FileInfo template = new FileInfo(ruta + FormatoReportesFrecuencia.PlantillaExcelReporteSegundosFaltantes);
            FileInfo newFile = new FileInfo(ruta + FormatoReportesFrecuencia.NombreReporteSegundosFaltantes);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + FormatoReportesFrecuencia.NombreReporteSegundosFaltantes);
            }
            int row = 7;
            int column = 2;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Cells[1, 3].Value = "REPORTE DE SEGUNDOS FALTANTES ";
                ws.Cells[2, 2].Value = "FECHA INICIO:";
                ws.Cells[2, 3].Value = model.FechaIni;
                ws.Cells[3, 2].Value = "FECHA FINAL:";
                ws.Cells[3, 3].Value = model.FechaFin;
                ws.Cells[4, 2].Value = "FECHA REPORTE:";
                ws.Cells[4, 3].Value = DateTime.Now.ToString(Constantes.FormatoFechaHora);
                var font = ws.Cells[1, 3].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                ConfiguracionHojaExcel(ws, model.ListaFechas);

                int numCeldaFinal = model.ListaFechas.Count + 3;

                foreach (var reg in model.ListaEquipos)
                {

                    ws.Cells[row, column].Value = reg.NombreEquipo;
                    int columInicio = 3;
                    foreach (var fecha in model.ListaFechas)
                    {
                        var aux = model.ListaReporte.Where(x => x.GPSCodi == reg.GPSCodi && x.FechaHora == fecha).ToList();
                        var difSeg = 86400;
                        if (aux.Count > 0)
                        {
                            var primerElemento = aux.First();
                            difSeg = 86400 - primerElemento.Num;
                        }
                        ws.Cells[row, columInicio].Value = difSeg;
                        columInicio++;
                    }

                    var border = ws.Cells[row, 2, row, numCeldaFinal].Style.Border;                  
                    ws.Cells[row, 2, row, numCeldaFinal].Style.WrapText = true;
                    ws.Cells[row, 2, row, numCeldaFinal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //ws.Cells[row, column + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //ws.Cells[row, column + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, numCeldaFinal].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";

                    var auxEquipo = model.ListaReporte.Where(x => x.GPSCodi ==reg.GPSCodi).ToList();
                    int intNumDias = model.ListaFechas.Count;
                    int totalEquipo = 86400 * intNumDias;
                    if (auxEquipo.Count > 0)
                    {
                        foreach (var itemAux in auxEquipo)
                        {
                            totalEquipo = totalEquipo - itemAux.Num;
                        }
                    }

                    ws.Cells[row, numCeldaFinal].Value = totalEquipo;
                    row++;
                }

                ws.Cells[row, 2].Value = "TOTAL GENERAL";
                int columInicioReporte = 3;
                foreach (var fecha in model.ListaFechas)
                {
                    var aux = model.ListaReporte.Where(x => x.FechaHora == fecha).ToList();
                    int intNumEquipos = model.ListaEquipos.Count;
                    int totalFecha = 86400 * intNumEquipos;
                    if (aux.Count > 0)
                    {
                        foreach (var itemAux in aux)
                        {
                            totalFecha = totalFecha - itemAux.Num;
                        }
                    }

                    ws.Cells[row, columInicioReporte].Value = totalFecha;
                    columInicioReporte++;
                }

                var auxTotal = model.ListaReporte.ToList();
                int intTotalNumEquipos = model.ListaEquipos.Count;
                int intTotalNumDias = model.ListaFechas.Count;

                int totalGeneral = 86400 * intTotalNumEquipos * intTotalNumDias;
                if (auxTotal.Count > 0)
                {
                    foreach (var itemAux in auxTotal)
                    {
                        totalGeneral = totalGeneral - itemAux.Num;
                    }
                }
                ws.Cells[row, columInicioReporte].Value = totalGeneral;

                var borderFinal = ws.Cells[row, 2, row, numCeldaFinal].Style.Border;
                ws.Cells[row, 2, row, numCeldaFinal].Style.WrapText = true;
                ws.Cells[row, 2, row, numCeldaFinal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                borderFinal.Bottom.Style = borderFinal.Top.Style = borderFinal.Left.Style = borderFinal.Right.Style = ExcelBorderStyle.Thin;
                var fontTablaFinal = ws.Cells[row, 2, row, numCeldaFinal].Style.Font;
                fontTablaFinal.Size = 8;
                fontTablaFinal.Name = "Calibri";

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera listado de Reporte de segundos faltantes de archivo excel
        /// </summary>
        /// <param name="model"></param>
        public static void GenerarArchivoReporteMilisegundos(ExtraerFrecuenciaModel model)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
            FileInfo template = new FileInfo(ruta + FormatoReportesFrecuencia.PlantillaExcelReporteSegundosFaltantes);
            FileInfo newFile = new FileInfo(ruta + FormatoReportesFrecuencia.NombreReporteMilisegundos);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + FormatoReportesFrecuencia.NombreReporteMilisegundos);
            }
            int row = 9;
            int column = 2;
            

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Column(2).Width = 32;
                ws.Cells[1, 3].Value = "REPORTE DE MILISEGUNDOS ";
                ws.Cells[3, 2].Value = "EQUIPO GPS:";
                ws.Cells[3, 3].Value = model.Entidad.GPSNombre;
                ws.Cells[4, 2].Value = "FECHA HORA INICIO:";
                ws.Cells[4, 3].Value = model.Entidad.FechaHoraInicioString;
                ws.Cells[5, 2].Value = "FECHA HORA FIN:";
                ws.Cells[5, 3].Value = model.Entidad.FechaHoraFinString;
                ws.Cells[6, 2].Value = "FECHA REPORTE:";
                ws.Cells[6, 3].Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var font = ws.Cells[1, 6].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";
                //ConfiguracionHojaExcel(ws, model.ListaFechas);

                int numCeldaFinal = model.ListaMilisegundos.Count + 3;

                ws.Cells[8, 2].Value = "FECHA HORA";
                ws.Cells[8, 3].Value = "FRECUENCIA";
                ws.Cells[8, 4].Value = "TENSION";
                var borderInicio = ws.Cells[8, 2, 8, 4].Style.Border;
                ws.Cells[8, 2, 8, 4].Style.WrapText = true;
                ws.Cells[8, 2, 8, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //ws.Cells[6, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                borderInicio.Bottom.Style = borderInicio.Top.Style = borderInicio.Left.Style = borderInicio.Right.Style = ExcelBorderStyle.Thin;
                var fontTablaInicio = ws.Cells[8, 2, 8, 4].Style.Font;
                fontTablaInicio.Size = 8;
                fontTablaInicio.Name = "Calibri";

                foreach (var reg in model.ListaMilisegundos)
                {

                    ws.Cells[row, column].Value = " " + reg.FechaHoraString + "." + reg.Miliseg.ToString("D3");
                    ws.Cells[row, column + 1].Value = reg.Frecuencia;
                    ws.Cells[row, column + 2].Value = reg.Tension;


                    var border = ws.Cells[row, 2, row, 4].Style.Border;
                    ws.Cells[row, 2, row, 4].Style.WrapText = true;
                    ws.Cells[row, 2, row, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 4].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";

                    row++;
                }

                xlPackage.Save();
            }
        }



    }
}