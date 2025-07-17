using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Campanias.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using COES.Dominio.DTO.Campania;
using COES.Servicios.Aplicacion.Campanias;
using System.Globalization;
using DevExpress.ClipboardSource.SpreadsheetML;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using DevExpress.XtraReports;
using static iTextSharp.text.pdf.AcroFields;
using static COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper.TablaHTML;
using static COES.Servicios.Aplicacion.PotenciaFirme.ConstantesPotenciaFirmeRemunerable;
using DevExpress.Xpo.DB;
using DevExpress.XtraRichEdit.Layout;
using COES.Framework.Base.Tools;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;

namespace COES.MVC.Intranet.Areas.Campanias.Helper
{
    public class ExcelReport
    {
        /// <summary>
        /// Genera Reporte
        /// </summary>
        /// <param name="model"></param>
        /// 
        private static string ConvertirAFormatoSeguro(object valor, int decimales=4)
        {
            if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()))
            {
                return ""; // Retorna vacío si el valor es null o una cadena vacía
            }

            if (decimal.TryParse(valor.ToString(), out decimal resultado))
            {
                return resultado.ToString($"F{decimales}"); // Convierte a string con los decimales fijos
            }

            return ""; // Si no es un número, retorna vacío
        }

        // Función para aplicar estilos a cualquier hoja
        public static void AplicarEstilosHoja(ExcelWorksheet ws, int filasCabecera = 1)
        {
            if (ws?.Dimension != null) // Evitar errores en hojas vacías o nulas
            {
                int totalFilas = ws.Dimension.End.Row;
                int totalColumnas = ws.Dimension.End.Column;

                int filaInicioDatos = filasCabecera + 1; // Datos comienzan después de las cabeceras

                if (filaInicioDatos <= totalFilas) // Evitar aplicar en hojas vacías o sin datos
                {
                    using (ExcelRange rango = ws.Cells[filaInicioDatos, 1, totalFilas, totalColumnas])
                    {
                        rango.Style.Font.Color.SetColor(Color.Black);
                        rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
            }
        }
        private static string Capitalize(string texto)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((texto ?? "").ToLower());
        }

        public static void AplicarFormatoFecha(ExcelWorksheet ws, int columna)
        {
            if (ws?.Dimension != null)
            {
                int filaInicio = ws.Dimension.Start.Row;
                int filaFin = ws.Dimension.End.Row;

                string rango = $"{ExcelCellAddress.GetColumnLetter(columna)}{filaInicio}:{ExcelCellAddress.GetColumnLetter(columna)}{filaFin}";
                ws.Cells[rango].Style.Numberformat.Format = "dd/MM/yyyy";
            }
        }
        public static string CombinarTexto(string principal, string otro)
        {
            return !string.IsNullOrWhiteSpace(otro)
                ? $"{principal} - {otro}"
                : principal;
        }

        private static string ConvertirFormatoRadio(object valor)
        {
            if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()))
            {
                return "";
            }

            string valorStr = valor.ToString().Trim(); 

            if (valorStr == "S")
            {
                return "Si";
            }
            else if (valorStr == "N")
            {
                return "No";
            }

            return valorStr;
        }

        public static void GenerarReporteItcDemanda(ItcDemandaReporteDTO itcDemanda, PeriodoDTO Periodo, string nombreTemp )
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantesCampanias.FolderFichas);
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteItc);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteItc, FormatoArchivosExcelCampanias.NombreReporteItcDemanda));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteItcDemanda));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteItcDemanda));
            }

            const int anioBase = 2025;

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    int anioPeriodo = Periodo.PeriFechaInicio.Year+1;
                    int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante-1;

                    if (itcDemanda.lista104 != null && itcDemanda.lista104.Any())
                    {
                        ExcelWorksheet ws104 = ObtenerHoja(xlPackage.Workbook, "F-104");
                        // Cantidad de decimales configurable
                        int cantidadDecimales = 4;
                        int filaInicial = 4; // Comienza desde la fila 4
                        int filaActual = filaInicial;

                        ProcesarLista(ws104, itcDemanda.lista104, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Anio;
                            ws.Cells[index, 4].Value = ConvertirAFormatoSeguro(item.MillonesolesPbi);
                            ws.Cells[index, 5].Value = ConvertirAFormatoSeguro(item.TasaCrecimientoPbi);
                            ws.Cells[index, 6].Value = ConvertirAFormatoSeguro(item.NroClientesLibres);
                            ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.NroClientesRegulados);
                            ws.Cells[index, 8].Value = ConvertirAFormatoSeguro(item.NroHabitantes);
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.TasaCrecimientoPoblacion);
                            ws.Cells[index, 10].Value = ConvertirAFormatoSeguro(item.MillonesClientesRegulados);
                            ws.Cells[index, 11].Value = ConvertirAFormatoSeguro(item.ClientesReguladoSelectr);
                            ws.Cells[index, 12].Value = ConvertirAFormatoSeguro(item.Usmwh, 2);
                            ws.Cells[index, 13].Value = ConvertirAFormatoSeguro(item.TasaCrecimientoEnergia);

                            filaActual++; // Avanzar a la siguiente fila
                        }, filaInicial);

                        AplicarEstilosHoja(ws104, 1);


                    }
                    if (itcDemanda.listaP011 != null && itcDemanda.listaP011.Any())
                    {
                        ExcelWorksheet ws = ObtenerHoja(xlPackage.Workbook, "F-P01.1");

                        var fechas = new HashSet<DateTime>(
                            itcDemanda.listaP011
                            .SelectMany(dto => dto.ListItcdf011Det)
                            .Where(d => !string.IsNullOrEmpty(d.FechaHora))
                            .Select(d => DateTime.ParseExact(d.FechaHora, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture))
                        ).OrderBy(f => f).ToList();

                        var columnas = itcDemanda.listaP011
                            .SelectMany(dto => dto.ListItcdf011Det
                                .Select(d => (dto.ProyCodi, d.BarraNro, dto.Empresa, dto.AreaDemanda)))
                            .Distinct()
                            .OrderBy(c => c.ProyCodi).ThenBy(c => c.BarraNro)
                            .ToList();

                        var detalleDict = itcDemanda.listaP011
                            .SelectMany(dto => dto.ListItcdf011Det
                                .Select(d => new { dto.ProyCodi, d.BarraNro, d.FechaHora, d.Kwval, d.Kvarval }))
                            .GroupBy(d => (d.ProyCodi, d.BarraNro, d.FechaHora))
                            .ToDictionary(g => g.Key, g => g.First());

                        int colBase = 2;

                        using (var rgCab = ws.Cells[1, 1, 1, 1 + columnas.Count * 2])
                        {
                            rgCab.Merge = true;
                            rgCab.Style.Font.Bold = true;
                            rgCab.Value = "REGISTRO HISTÓRICO DE LAS CARGAS";
                            rgCab.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        for (int i = 0; i < columnas.Count; i++)
                        {
                            int col = colBase + i * 2;
                            ws.Cells[2, col, 2, col + 1].Merge = true;
                            ws.Cells[2, col].Value = int.TryParse(columnas[i].AreaDemanda, out int area) ? area : 0; ;
                            ws.Cells[3, col, 3, col + 1].Merge = true;
                            ws.Cells[3, col].Value = columnas[i].Empresa;
                            ws.Cells[4, col, 4, col + 1].Merge = true;
                            ws.Cells[4, col].Value = $"BARRA[{columnas[i].BarraNro}]";
                            ws.Cells[5, col, 5, col + 1].Merge = true;
                            ws.Cells[5, col].Value = "KV";
                            ws.Cells[6, col].Value = "kW";
                            ws.Cells[6, col + 1].Value = "kVAR";
                        }

                        int fila = 7;
                        object[,] dataArray = new object[fechas.Count, columnas.Count * 2 + 1];

                        for (int i = 0; i < fechas.Count; i++)
                        {
                            DateTime fecha = fechas[i];
                            dataArray[i, 0] = fecha.ToString("dd/MM/yyyy HH:mm");

                            for (int j = 0; j < columnas.Count; j++)
                            {
                                var colInfo = columnas[j];
                                string fechaKey = fecha.ToString("dd/MM/yyyy HH:mm");

                                if (detalleDict.TryGetValue((colInfo.ProyCodi, colInfo.BarraNro, fechaKey), out var detalle))
                                {
                                    dataArray[i, j * 2 + 1] = detalle.Kwval.HasValue ? Math.Round((double)detalle.Kwval.Value, 4) : (object)null;
                                    dataArray[i, j * 2 + 2] = detalle.Kvarval.HasValue ? Math.Round((double)detalle.Kvarval.Value, 4) : (object)null;
                                }
                            }
                        }

                        int lastCol = 1 + columnas.Count * 2;
                        ws.Cells[fila, 1, fila + fechas.Count - 1, lastCol].Value = dataArray;

                        using (var rgCab = ws.Cells[2, 1, 3, lastCol])
                        {
                            rgCab.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        using (var rgCab = ws.Cells[4, 1, 6, lastCol])
                        {
                            rgCab.Style.Font.Bold = true;
                            rgCab.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgCab.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E2EFDA"));
                            rgCab.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            rgCab.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        using (var rgDatos = ws.Cells[fila, 1, fila + fechas.Count - 1, lastCol])
                        {
                            rgDatos.Style.Numberformat.Format = "0.0000";
                        }

                        using (var rgDatos = ws.Cells[1, 1, fila + fechas.Count - 1, lastCol])
                        {
                            rgDatos.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDatos.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDatos.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDatos.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                    }

                    if (itcDemanda.lista108 != null && itcDemanda.lista108.Any())
                    {
                        ExcelWorksheet ws108 = ObtenerHoja(xlPackage.Workbook, "F-108");
                     
                        int cantidadDecimales = 4;
                        int filaInicial = 4; 

                        var empresasAgrupadas = itcDemanda.lista108
                            .Where(x => !string.IsNullOrWhiteSpace(x.Empresa))
                            .GroupBy(item => item.Empresa.Trim().ToUpper())
                            .ToList();

                        foreach (var group in empresasAgrupadas)
                        {
                            string empresa = group.First().Empresa;
                            var listaPorEmpresa = group.ToList();

                            int menorAnio = listaPorEmpresa.Min(item => item.Anio);
                            int correlativo = 1;

                            Console.WriteLine($"Procesando empresa: {empresa} con {listaPorEmpresa.Count} registros");

                            ProcesarLista(ws108, listaPorEmpresa, (ws, item, index) =>
                            {
                                Console.WriteLine($"    Escribiendo fila {index} año {item.Anio}");
                                if (item.Anio == menorAnio)
                                {
                                    ws.Cells[index, 3].Value = "Hist.";
                                    correlativo = 1;
                                }
                                else
                                {
                                    ws.Cells[index, 3].Value = correlativo;
                                    correlativo++;
                                }

                                ws.Cells[index, 1].Value = item.AreaDemanda;
                                ws.Cells[index, 2].Value = empresa;
                                ws.Cells[index, 4].Value = item.Anio;
                                ws.Cells[index, 5].Value = ConvertirAFormatoSeguro(item.Atval);
                                ws.Cells[index, 6].Value = ConvertirAFormatoSeguro(item.Mtval);
                                ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.Btval);
                                ws.Cells[index, 8].Value = ConvertirAFormatoSeguro((item.Atval ?? 0) + (item.Mtval ?? 0) + (item.Btval ?? 0), cantidadDecimales);

                                // Aplicar bordes a la fila
                                var border = ws.Cells[index, 1, index, 8].Style.Border;
                                border.Top.Style = ExcelBorderStyle.Thin;
                                border.Bottom.Style = ExcelBorderStyle.Thin;
                                border.Left.Style = ExcelBorderStyle.Thin;
                                border.Right.Style = ExcelBorderStyle.Thin;

                            }, filaInicial);

                            // Aumentar fila para que la siguiente empresa no sobreescriba
                            filaInicial += listaPorEmpresa.Count;
                        }


                        AplicarEstilosHoja(ws108, filaInicial - 1);
                    }



                    if (itcDemanda.listaP012 != null && itcDemanda.listaP012.Any())
                    {
                        ExcelWorksheet wsp012 = ObtenerHoja(xlPackage.Workbook, "F-P01.2");

                        ProcesarLista(wsp012, itcDemanda.listaP012, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.CodigoSicli;
                            ws.Cells[index, 4].Value = item.NombreCliente;
                            ws.Cells[index, 5].Value = item.Subestacion;
                            ws.Cells[index, 6].Value = item.Barra;
                            ws.Cells[index, 7].Value = item.CodigoNivelTension;
                        }, 3);
                        AplicarEstilosHoja(wsp012, 2);
                    }

                    if (itcDemanda.listaP013 != null && itcDemanda.listaP013.Any())
                    {
                        ExcelWorksheet wsp013 = ObtenerHoja(xlPackage.Workbook, "F-P01.3");
                        
                        int cantidadDeAnios = (horizonteFin - anioPeriodo);

                        if (wsp013.Cells["E2:O2"].Merge)
                        {
                            wsp013.Cells["E2:O2"].Merge = false;
                        }

                        int indexDet1 = 5;
                        wsp013.Cells[1, 1, 1, (6 + cantidadDeAnios) - 1].Merge = true;

                        wsp013.Cells[2, indexDet1, 2, (indexDet1 + cantidadDeAnios)].Merge = true;
                        wsp013.Cells[2, indexDet1].Value = "POTENCIA SOLICITADA (MW) [0]";
                        wsp013.Cells[2, indexDet1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        wsp013.Cells[2, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        wsp013.Cells[2, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsp013.Cells[2, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        wsp013.Cells[2, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsp013.Cells[2, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        Color colorFondo = ColorTranslator.FromHtml("#E2EFDA");

                        for (int anio = anioPeriodo; anio <= horizonteFin; anio++)
                        {
                            wsp013.Cells[3, indexDet1].Value = anio;
                            wsp013.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            wsp013.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            wsp013.Cells[3, indexDet1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            wsp013.Cells[3, indexDet1].Style.Fill.BackgroundColor.SetColor(colorFondo);
                            wsp013.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            wsp013.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            wsp013.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            wsp013.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            indexDet1++;
                        }

                        ProcesarLista(wsp013, itcDemanda.listaP013, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.NombreCliente;
                            ws.Cells[index, 4].Value = item.TipoCarga;

                            InsertDetalleHojaDemanda(ws, item.ListItcdf013Det, index, 5, indexDet1);

                        }, 4);
                        AplicarEstilosHoja(wsp013,3);
                    }

                    if (itcDemanda.lista110 != null && itcDemanda.lista110.Any())
                    {
                        ExcelWorksheet ws110 = ObtenerHoja(xlPackage.Workbook, "F-110");

                        int cantidadDeAnios = ((horizonteFin + 10+1)-(Periodo.PeriFechaInicio.Year - 1)) ;

                        if (ws110.Cells["H2:Z2"].Merge)
                        {
                            ws110.Cells["H2:Z2"].Merge = false;
                        }
                        ws110.Cells[1, 1, 1, (8 + cantidadDeAnios) - 1].Merge = true;
                        int indexDet1 = 8;

                        ws110.Cells[2, indexDet1, 2, (indexDet1 + cantidadDeAnios) - 1].Merge = true;
                        ws110.Cells[2, indexDet1].Value = "MÁXIMA DEMANDA (MW)";
                        ws110.Cells[2, indexDet1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws110.Cells[2, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws110.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws110.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws110.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws110.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        Color colorFondo = ColorTranslator.FromHtml("#E2EFDA");

                        for (int anio = Periodo.PeriFechaInicio.Year-1; anio <= horizonteFin+10; anio++)
                        {
                            ws110.Cells[3, indexDet1].Value = anio;
                            ws110.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws110.Cells[3, indexDet1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws110.Cells[3, indexDet1].Style.Fill.BackgroundColor.SetColor(colorFondo);
                            ws110.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws110.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws110.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws110.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            indexDet1++;
                        }

                        ProcesarLista(ws110, itcDemanda.lista110, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Sistema;
                            ws.Cells[index, 4].Value = item.Subestacion;
                            ws.Cells[index, 5].Value = item.Tension;
                            ws.Cells[index, 6].Value = item.Barra;
                            ws.Cells[index, 7].Value = item.IdCarga;

                            InsertDetalleHojaDemanda(ws, item.ListItcdf110Det, index, 8, indexDet1);

                        }, 4);
                        AplicarEstilosHoja(ws110, 3);
                    }

                    if (itcDemanda.lista116 != null && itcDemanda.lista116.Any())
                    {
                        ExcelWorksheet ws116 = ObtenerHoja(xlPackage.Workbook, "F-116");

                        int cantidadDeAnios = ((horizonteFin + 10 + 1) - (Periodo.PeriFechaInicio.Year - 1));

                        if (ws116.Cells["I2:AA2"].Merge)
                        {
                            ws116.Cells["I2:AA2"].Merge = false;
                        }
                        ws116.Cells[1, 1, 1, (8 + cantidadDeAnios)].Merge = true;
                        int indexDet1 = 9;

                        ws116.Cells[2, indexDet1, 2, (indexDet1 + cantidadDeAnios) - 1].Merge = true;
                        ws116.Cells[2, indexDet1].Value = "MÁXIMA DEMANDA (MW)";
                        ws116.Cells[2, indexDet1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws116.Cells[2, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws116.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws116.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws116.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws116.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                        Color colorFondo = ColorTranslator.FromHtml("#E2EFDA");
                        for (int anio = Periodo.PeriFechaInicio.Year - 1; anio <= horizonteFin + 10; anio++)
                        {
                            ws116.Cells[3, indexDet1].Value = anio;
                            ws116.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws116.Cells[3, indexDet1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws116.Cells[3, indexDet1].Style.Fill.BackgroundColor.SetColor(colorFondo);
                            ws116.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws116.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws116.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws116.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            indexDet1++;
                        }

                        ProcesarLista(ws116, itcDemanda.lista116, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Sistema;
                            ws.Cells[index, 4].Value = item.Subestacion;
                            ws.Cells[index, 5].Value = item.Tension;
                            ws.Cells[index, 6].Value = item.Barra;
                            ws.Cells[index, 7].Value = item.NombreCliente;
                            ws.Cells[index, 8].Value = item.IdCarga;

                            InsertDetalleHojaDemanda(ws, item.ListItcdf116Det, index, 9, indexDet1);

                        }, 4);
                        AplicarEstilosHoja(ws116, 3);
                    }

                    if (itcDemanda.lista121 != null && itcDemanda.lista121.Any())
                    {
                        ExcelWorksheet ws121 = ObtenerHoja(xlPackage.Workbook, "F-121");

                        int cantidadDeAnios = ((horizonteFin + 10 + 1) - (Periodo.PeriFechaInicio.Year - 1));

                        if (ws121.Cells["H2:Z2"].Merge)
                        {
                            ws121.Cells["H2:Z2"].Merge = false;
                        }

                        int indexDet1 = 8;

                        ws121.Cells[1, 1, 1, (7 + cantidadDeAnios)].Merge = true;
                        ws121.Cells[2, indexDet1, 2, (indexDet1 + cantidadDeAnios) - 1].Merge = true;
                        ws121.Cells[2, indexDet1].Value = "MÁXIMA DEMANDA (MW)";
                        ws121.Cells[2, indexDet1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws121.Cells[2, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws121.Cells[2, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws121.Cells[2, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws121.Cells[2, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws121.Cells[2, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        Color colorFondo = ColorTranslator.FromHtml("#E2EFDA");
                        for (int anio = Periodo.PeriFechaInicio.Year - 1; anio <= horizonteFin + 10; anio++)
                        {
                            ws121.Cells[3, indexDet1].Value = anio;
                            ws121.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws121.Cells[3, indexDet1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws121.Cells[3, indexDet1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws121.Cells[3, indexDet1].Style.Fill.BackgroundColor.SetColor(colorFondo);
                            ws121.Cells[3, indexDet1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws121.Cells[3, indexDet1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws121.Cells[3, indexDet1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws121.Cells[3, indexDet1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            indexDet1++;
                        }

                        ProcesarLista(ws121, itcDemanda.lista121, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Sistema;
                            ws.Cells[index, 4].Value = item.Subestacion;
                            ws.Cells[index, 5].Value = item.Tension;
                            ws.Cells[index, 6].Value = item.Barra;
                            ws.Cells[index, 7].Value = item.IdCarga;

                            InsertDetalleHojaDemanda(ws, item.ListItcdf121Det, index, 8, indexDet1);

                        }, 4);
                        AplicarEstilosHoja(ws121, 2);
                    }

                    if (itcDemanda.lista123 != null && itcDemanda.lista123.Any())
                    {
                        ExcelWorksheet ws123 = ObtenerHoja(xlPackage.Workbook, "F-123");

                        // Obtener el año inicial (según el periodo de inicio)
                        int anioInicial = Periodo.PeriFechaInicio.Year + 4;

                        // Generar la lista de años con saltos de 4 años (5 períodos en total)
                        List<int> listaAniosF123 = new List<int> { anioInicial };
                        for (int i = 1; i < 4; i++)
                        {
                            listaAniosF123.Add(listaAniosF123[i - 1] + 5);
                        }
                        int totalColumnas = 5 + listaAniosF123.Count;

                        ws123.Cells[1, 1, 1, totalColumnas].Merge = true;
                        ws123.Cells[1, 1, 1, totalColumnas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws123.Cells[1, 1, 1, totalColumnas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        int colInicio = 6;
                        int colFinal = 5 + listaAniosF123.Count;

                        Color colorFondo = ColorTranslator.FromHtml("#E2EFDA");
                        // Hacer el merge una sola vez
                        ws123.Cells[2, colInicio, 2, colFinal].Merge = true;

                        // Aplicar bordes a cada celda individual dentro del rango
                        for (int col = colInicio; col <= colFinal; col++)
                        {
                            ws123.Cells[2, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[2, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[2, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[2, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            // Si quieres aplicar también el fondo:
                            ws123.Cells[2, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws123.Cells[2, col].Style.Fill.BackgroundColor.SetColor(colorFondo);
                        }

                      

                        // Insertar los años en la fila 3 desde la columna 6
                        for (int i = 0; i < listaAniosF123.Count; i++)
                        {
                            int col = 6 + i; // Columna donde irán los años
                            ws123.Cells[3, col].Value = listaAniosF123[i];

                            // Aplicar bordes y fondo de color

                            ws123.Cells[3, col].Style.Fill.PatternType = ExcelFillStyle.Solid; // Establecer el patrón
                            ws123.Cells[3, col].Style.Fill.BackgroundColor.SetColor(colorFondo); // Ahora sí aplicar el color

                            ws123.Cells[3, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[3, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[3, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws123.Cells[3, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        // Insertar los datos desde la fila 4 en adelante
                        ProcesarLista(ws123, itcDemanda.lista123, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.UtmEste;
                            ws.Cells[index, 4].Value = item.UtmNorte;
                            ws.Cells[index, 5].Value = item.UtmZona;

                            // Aplicar los valores desde la columna 6
                            for (int i = 0; i < listaAniosF123.Count; i++)
                            {
                                int col = 6 + i;
                                ws.Cells[index, col].Value = ConvertirAFormatoSeguro(
                                    item.GetType().GetProperty($"Anio{i + 1}")?.GetValue(item), 4);

                                // Aplicar borde
                                //ws.Cells[index, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[index, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Cells[index, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[index, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Cells[index, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }, 4);

                        AplicarEstilosHoja(ws123, 2);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte ITC Demanda: " + ex.Message, ex);
            }
        }

        public static void InsertDetalleHojaDemanda<T>(ExcelWorksheet ws, List<T> listaDetalle, int index, int iteraDetInicial, int iteraDetFin) where T : class
        {
            if (listaDetalle != null && listaDetalle.Any())
            {
                var detallesPorAnio = listaDetalle
                    .GroupBy(d => d.GetType().GetProperty("Anio").GetValue(d))
                    .ToDictionary(g => g.Key, g => g.First());

                for (int i = iteraDetInicial; i < iteraDetFin; i++)
                {
                    var valorAnioStr = ws.Cells[3, i].Value.ToString();

                    if (int.TryParse(valorAnioStr, out int valorAnio))
                    {
                        if (detallesPorAnio.TryGetValue(valorAnio, out var detalle))
                        {
                            ws.Cells[index, i].Value = ConvertirAFormatoSeguro(detalle.GetType().GetProperty("Valor").GetValue(detalle),4);
                            ws.Cells[index, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                }
            }

            DeleteCeldasColumnRestantesExcel(iteraDetFin, ws);
        }

        public static void GenerarReporteItcSist(ItcSistemaReporteDTO itcSistema, string nombreTemp)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantesCampanias.FolderFichas);
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteItc);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteItc, FormatoArchivosExcelCampanias.NombreReporteItcSistema));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteItcSistema));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteItcSistema));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (itcSistema.listaprm1 != null)
                    {
                        itcSistema.listaprm1 = itcSistema.listaprm1
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.Electroducto) ||
                            !string.IsNullOrWhiteSpace(x.Descripcion) ||
                            !string.IsNullOrWhiteSpace(x.Tipo) ||
                            x.Vn != null ||
                            x.Seccion != null ||
                            x.Ctr != null ||
                            x.R != null ||
                            x.X != null ||
                            x.B != null ||
                            x.Ro != null ||
                            x.Xo != null || 
                            x.Bo != null ||
                            x.Capacidad != null ||
                            x.Tmxop != null
                        )
                        .ToList();
                    }
                        if (itcSistema.listaprm1 != null && itcSistema.listaprm1.Any())
                    {
                        ExcelWorksheet wsprm1 = ObtenerHoja(xlPackage.Workbook, "PRM-1");

                        ProcesarLista(wsprm1, itcSistema.listaprm1, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value =  item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Electroducto;
                            ws.Cells[index, 4].Value = item.Descripcion;
                            ws.Cells[index, 5].Value = ConvertirAFormatoSeguro(item.Vn); 
                            ws.Cells[index, 6].Value = item.Tipo;
                            ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.Seccion);
                            ws.Cells[index, 8].Value = ConvertirAFormatoSeguro(item.Ctr);
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.R);
                            ws.Cells[index, 10].Value = ConvertirAFormatoSeguro(item.X);
                            ws.Cells[index, 11].Value = ConvertirAFormatoSeguro(item.B);
                            ws.Cells[index, 12].Value = ConvertirAFormatoSeguro(item.Ro);
                            ws.Cells[index, 13].Value = ConvertirAFormatoSeguro(item.Xo);
                            ws.Cells[index, 14].Value = ConvertirAFormatoSeguro(item.Bo);
                            ws.Cells[index, 15].Value = ConvertirAFormatoSeguro(item.Capacidad);
                            ws.Cells[index, 16].Value = ConvertirAFormatoSeguro(item.Tmxop);
                        }, 2);
                        AplicarEstilosHoja(wsprm1, 1);
                    }

                    if (itcSistema.listaprm2 != null)
                    {
                        itcSistema.listaprm2 = itcSistema.listaprm2
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.Grpcnx) ||
                            !string.IsNullOrWhiteSpace(x.Transformador) ||
                            !string.IsNullOrWhiteSpace(x.Taplado) ||
                            !string.IsNullOrWhiteSpace(x.Taptipo) ||
                            !string.IsNullOrWhiteSpace(x.Tipo) ||
                            !string.IsNullOrWhiteSpace(x.Pnp) ||
                            !string.IsNullOrWhiteSpace(x.Pns) ||
                            !string.IsNullOrWhiteSpace(x.Pnt) ||
                            x.Fases != null ||
                            x.Ndvn != null ||
                            x.Vnp != null ||
                            x.Vns != null ||
                            x.Vnt != null ||
                            x.Pcups != null ||
                            x.Pcust != null ||
                            x.Pcutp != null ||
                            x.Pfe != null ||
                            x.Tapcnt != null ||
                            x.Tapdv != null ||
                            x.Tapmax != null ||
                            x.Tapmin != null ||
                            x.Tccps != null ||
                            x.Tccst != null ||
                            x.Tcctp != null ||
                            x.Ivacio != null
                        )
                        .ToList();
                    }

                    if (itcSistema.listaprm2 != null && itcSistema.listaprm2.Any())
                    {
                        ExcelWorksheet wsprm2 = ObtenerHoja(xlPackage.Workbook, "PRM-2");

                        ProcesarLista(wsprm2, itcSistema.listaprm2, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Transformador;
                            ws.Cells[index, 4].Value = item.Tipo;
                            ws.Cells[index, 5].Value = item.Fases;
                            ws.Cells[index, 6].Value = item.Ndvn;
                            ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.Vnp);
                            ws.Cells[index, 8].Value = ConvertirAFormatoSeguro(item.Vns);
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.Vnt);
                            ws.Cells[index, 10].Value = item.Pnp;
                            ws.Cells[index, 11].Value = item.Pns;
                            ws.Cells[index, 12].Value = item.Pnt;
                            ws.Cells[index, 13].Value = ConvertirAFormatoSeguro(item.Tccps);
                            ws.Cells[index, 14].Value = ConvertirAFormatoSeguro(item.Tccst);
                            ws.Cells[index, 15].Value = ConvertirAFormatoSeguro(item.Tcctp);
                            ws.Cells[index, 16].Value = ConvertirAFormatoSeguro(item.Pcups);
                            ws.Cells[index, 17].Value = ConvertirAFormatoSeguro(item.Pcust);
                            ws.Cells[index, 18].Value = ConvertirAFormatoSeguro(item.Pcutp);
                            ws.Cells[index, 19].Value = ConvertirAFormatoSeguro(item.Pfe);
                            ws.Cells[index, 20].Value = ConvertirAFormatoSeguro(item.Ivacio);
                            ws.Cells[index, 21].Value = item.Grpcnx;
                            ws.Cells[index, 22].Value = item.Taptipo;
                            ws.Cells[index, 23].Value = item.Taplado;
                            ws.Cells[index, 24].Value = ConvertirAFormatoSeguro(item.Tapdv);
                            ws.Cells[index, 25].Value = item.Tapmin;
                            ws.Cells[index, 26].Value = item.Tapcnt;
                            ws.Cells[index, 27].Value = item.Tapmax;
                        }, 2);
                        AplicarEstilosHoja(wsprm2, 1);
                    }

                    if (itcSistema.listared1 != null && itcSistema.listared1.Any())
                    {
                        ExcelWorksheet wsred1 = ObtenerHoja(xlPackage.Workbook, "RED-1");

                        ProcesarLista(wsred1, itcSistema.listared1, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Barra;
                            ws.Cells[index, 4].Value = item.Vnpu;
                            ws.Cells[index, 5].Value = item.Vopu;
                            ws.Cells[index, 6].Value = item.Tipo;
                        }, 2);
                        AplicarEstilosHoja(wsred1, 1);
                    }

                    if (itcSistema.listared2 != null)
                    {
                        // Eliminar registros vacíos
                        itcSistema.listared2 = itcSistema.listared2
                            .Where(x =>

                                !string.IsNullOrWhiteSpace(x.Electroducto) ||
                                !string.IsNullOrWhiteSpace(x.BarraE) ||
                                !string.IsNullOrWhiteSpace(x.BarraR) ||
                                !string.IsNullOrWhiteSpace(x.Linea) ||
                                x.Longitud != null ||
                                x.Tramo != null ||
                                x.Nternas != null
                            )
                            .ToList();
                    }

                 

                    if (itcSistema.listared2 != null && itcSistema.listared2.Any())
                    {
                        ExcelWorksheet wsred2 = ObtenerHoja(xlPackage.Workbook, "RED-2");

                        ProcesarLista(wsred2, itcSistema.listared2, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.Linea;
                            ws.Cells[index, 4].Value = item.BarraE;
                            ws.Cells[index, 5].Value = item.BarraR;
                            ws.Cells[index, 6].Value = item.Nternas;
                            ws.Cells[index, 7].Value = item.Tramo;
                            ws.Cells[index, 8].Value = item.Electroducto;
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.Longitud);
                        }, 2);
                        AplicarEstilosHoja(wsred2, 1);
                    }

                    if (itcSistema.listared3 != null)
                    {
                        itcSistema.listared3 = itcSistema.listared3
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.IdCircuito) ||
                            !string.IsNullOrWhiteSpace(x.BarraP) ||
                            !string.IsNullOrWhiteSpace(x.BarraS) ||
                            !string.IsNullOrWhiteSpace(x.BarraT) ||
                            !string.IsNullOrWhiteSpace(x.CdgTrafo) ||
                            !string.IsNullOrWhiteSpace(x.OprTap) ||
                            x.PosTap != null
                        )
                        .ToList();
                    }

                    if (itcSistema.listared3 != null && itcSistema.listared3.Any())
                    {
                        ExcelWorksheet wsred3 = ObtenerHoja(xlPackage.Workbook, "RED-3");

                        ProcesarLista(wsred3, itcSistema.listared3, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.IdCircuito;
                            ws.Cells[index, 4].Value = item.BarraP;
                            ws.Cells[index, 5].Value = item.BarraS;
                            ws.Cells[index, 6].Value = item.BarraT;
                            ws.Cells[index, 7].Value = item.CdgTrafo;
                            ws.Cells[index, 8].Value = item.OprTap;
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.PosTap);
                        }, 2);
                        AplicarEstilosHoja(wsred3, 1);
                    }
                    if (itcSistema.listared4 != null)
                    {
                        itcSistema.listared4 = itcSistema.listared4
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.IdCmp) ||
                            !string.IsNullOrWhiteSpace(x.Barra) ||
                            !string.IsNullOrWhiteSpace(x.Tipo) ||
                            x.Vnkv != null ||
                            x.CapmVar != null ||
                            x.Npasos != null ||
                            x.PasoAct != null
                        )
                        .ToList();
                    }


                    if (itcSistema.listared4 != null && itcSistema.listared4.Any())
                    {
                        ExcelWorksheet wsred4 = ObtenerHoja(xlPackage.Workbook, "RED-4");

                        ProcesarLista(wsred4, itcSistema.listared4, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.IdCmp;
                            ws.Cells[index, 4].Value = item.Barra;
                            ws.Cells[index, 5].Value = item.Tipo;
                            ws.Cells[index, 6].Value = ConvertirAFormatoSeguro(item.Vnkv);
                            ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.CapmVar);
                            ws.Cells[index, 8].Value = item.Npasos;
                            ws.Cells[index, 9].Value = item.PasoAct;
                        }, 2);
                        AplicarEstilosHoja(wsred4, 1);
                    }
                    if (itcSistema.listared5 != null)
                    {
                        itcSistema.listared5 = itcSistema.listared5
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.CaiGen) ||
                            !string.IsNullOrWhiteSpace(x.IdGen) ||
                            !string.IsNullOrWhiteSpace(x.Barra) ||
                            x.PdMw != null ||
                            x.PnMw != null ||
                            x.QnMin != null ||
                            x.QnMa != null
                        )
                        .ToList();
                    }

                    if (itcSistema.listared5 != null && itcSistema.listared5.Any())
                    {
                        ExcelWorksheet wsred5 = ObtenerHoja(xlPackage.Workbook, "RED-5");

                        ProcesarLista(wsred5, itcSistema.listared5, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.AreaDemanda;
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.CaiGen;
                            ws.Cells[index, 4].Value = item.IdGen;
                            ws.Cells[index, 5].Value = item.Barra;
                            ws.Cells[index, 6].Value = ConvertirAFormatoSeguro(item.PdMw);
                            ws.Cells[index, 7].Value = ConvertirAFormatoSeguro(item.PnMw);
                            ws.Cells[index, 8].Value = ConvertirAFormatoSeguro(item.QnMin);
                            ws.Cells[index, 9].Value = ConvertirAFormatoSeguro(item.QnMa);
                        }, 2);
                        AplicarEstilosHoja(wsred5, 1);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte ITC Sistema: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteCentralesTermicas(CentralTermicaReporteDTO centralTermica, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyTermica));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyTermica));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyTermica));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (centralTermica.listaTermicaA != null && centralTermica.listaTermicaA.Any())
                    {
                        ExcelWorksheet wsTermA = ObtenerHoja(xlPackage.Workbook, "CC.TT.-A");

                        ProcesarLista(wsTermA, centralTermica.listaTermicaA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.Centralnombre;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = item.Propietario;
                            ws.Cells[index, 11].Value = item.Sociooperador;
                            ws.Cells[index, 12].Value = item.Socioinversionista;
                            ws.Cells[index, 13].Value = item.Tipoconcesionactual;
                            ws.Cells[index, 14].Value = item.Fechaconcesionactual;
                            ws.Cells[index, 15].Value = item.Potenciainstalada;
                            ws.Cells[index, 16].Value = item.Potenciamaxima;
                            ws.Cells[index, 17].Value = item.Potenciaminima;
                            ws.Cells[index, 18].Value = !string.IsNullOrWhiteSpace(item.CombustibletipoOtro) ? $"{item.Combustibletipo} - {item.CombustibletipoOtro}" : item.Combustibletipo;
                            ws.Cells[index, 19].Value = item.Podercalorificoinferior;
                            ws.Cells[index, 20].Value = item.Undpci;
                            ws.Cells[index, 21].Value = item.Podercalorificosuperior;
                            ws.Cells[index, 22].Value = item.Undpcs;
                            ws.Cells[index, 23].Value = item.Costocombustible;
                            ws.Cells[index, 24].Value = item.Undcomb;
                            ws.Cells[index, 25].Value = item.Costotratamientocombustible;
                            ws.Cells[index, 26].Value = item.Undtrtcomb;
                            ws.Cells[index, 27].Value = item.Costotransportecombustible;
                            ws.Cells[index, 28].Value = item.Undtrnspcomb;
                            ws.Cells[index, 29].Value = item.Costovariablenocombustible;
                            ws.Cells[index, 30].Value = item.Undvarncmb;
                            ws.Cells[index, 31].Value = item.Costoinversioninicial;
                            ws.Cells[index, 32].Value = item.Undinvinic;
                            ws.Cells[index, 33].Value = item.Rendimientoplantacondicion;
                            ws.Cells[index, 34].Value = item.Undrendcnd;
                            ws.Cells[index, 35].Value = item.Consespificacondicion;
                            ws.Cells[index, 36].Value = item.Undconscp;
                            ws.Cells[index, 37].Value = item.Tipomotortermico;
                            ws.Cells[index, 38].Value = item.Velnomrotacion;
                            ws.Cells[index, 39].Value = item.Potmotortermico;
                            if (!string.IsNullOrEmpty(item.Nummotorestermicos?.ToString()) && int.TryParse(item.Nummotorestermicos.ToString(), out int nummotorestermicos))
                            {
                                ws.Cells[index, 40].Value = nummotorestermicos;
                                ws.Cells[index, 40].Style.Numberformat.Format = "0";
                            }
                            ws.Cells[index, 41].Value = item.Potgenerador;
                            if (!string.IsNullOrEmpty(item.Numgeneradores?.ToString()) && int.TryParse(item.Numgeneradores.ToString(), out int numGeneradores))
                            {
                                ws.Cells[index, 42].Value = numGeneradores;
                                ws.Cells[index, 42].Style.Numberformat.Format = "0";
                            }
                            ws.Cells[index, 43].Value = item.Tensiongeneracion;
                            ws.Cells[index, 44].Value = item.Rendimientogenerador;


                            ws.Cells[index, 45].Value = item.Tensionkv;
                            ws.Cells[index, 46].Value = item.Longitudkm;

                            if (!string.IsNullOrEmpty(item.Numternas?.ToString()) && int.TryParse(item.Numternas.ToString(), out int numTernas))
                            {
                                ws.Cells[index, 47].Value = numTernas;
                                ws.Cells[index, 47].Style.Numberformat.Format = "0";
                            }
                            ws.Cells[index, 48].Value = item.Nombresubestacion;
                            ws.Cells[index, 49].Value = item.Perfil;
                            ws.Cells[index, 50].Value = item.Prefactibilidad;
                            ws.Cells[index, 51].Value = item.Factibilidad;
                            ws.Cells[index, 52].Value = item.Estudiodefinitivo;
                            ws.Cells[index, 53].Value = item.Eia;
                            ws.Cells[index, 54].Value = item.Fechainicioconstruccion;
                            if (!string.IsNullOrEmpty(item.Periodoconstruccion?.ToString()) && int.TryParse(item.Periodoconstruccion.ToString(), out int periodoConstruccion))
                            {
                                ws.Cells[index, 55].Value = periodoConstruccion;
                                ws.Cells[index, 55].Style.Numberformat.Format = "0";
                            }
                            ws.Cells[index, 56].Value = item.Fechaoperacioncomercial;
                            ws.Cells[index, 57].Value = ConvertirFormatoRadio(item.Condifencial);
                        }, 3);
                        AplicarEstilosHoja(wsTermA, 2);
                        AplicarFormatoFecha(wsTermA, 14);
                    }

                    if (centralTermica.listaTermicaB != null && centralTermica.listaTermicaB.Any())
                    {
                        ExcelWorksheet wsTermB = ObtenerHoja(xlPackage.Workbook, "CC.TT.-B");

                        ProcesarLista(wsTermB, centralTermica.listaTermicaB, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = item.Obrasregulacion;
                            ws.Cells[index, 13].Value = item.Administracion;
                            ws.Cells[index, 14].Value = item.Aduanas;
                            ws.Cells[index, 15].Value = item.Supervision;
                            ws.Cells[index, 16].Value = item.Gastosgestion;
                            ws.Cells[index, 17].Value = item.Imprevistos;
                            ws.Cells[index, 18].Value = item.Igv;
                            ws.Cells[index, 19].Value = item.Usoagua;
                            ws.Cells[index, 20].Value = item.Otrosgastos;
                            ws.Cells[index, 21].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 22].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 23].Value = item.Financiamientotipo;
                            ws.Cells[index, 24].Value = item.Financiamientoestado;
                            ws.Cells[index, 25].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 26].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 27].Value = item.Ventaenergia;
                            ws.Cells[index, 28].Value = item.Ejecucionobra;
                            ws.Cells[index, 29].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsTermB, 2);
                    }
                    var countLista = 1;
                    var existData = 0;
                    if (centralTermica.listaTermicaC != null && centralTermica.listaTermicaC.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - anioPeriodo) + 1;
                        countLista = cantidadDeAnios * listaparam.Count * 4;
                        existData = 1;
                        GenerarHojaReporteCronograma(croEg, centralTermica.listaTermicaC, anioPeriodo, horizonteFin, listaparam, 1);
                    }

                    if (centralTermica.listaTermicaC2 != null && centralTermica.listaTermicaC2.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;
                        existData = 1;
                        GenerarHojaReporteCronograma(croEg, centralTermica.listaTermicaC2, anioPeriodo, horizonteFin, listaparam, countLista + 3);
                    }

                    if(existData == 0)
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                        xlPackage.Workbook.Worksheets.Delete(croEg);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Central Térmicas: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteCentralesHidroelectricas(CentralHidroReporteDTO centralHidro, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstantesCampanias.FolderFichas);
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyHidroel));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyHidroel));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyHidroel));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (centralHidro.listaHidroA != null && centralHidro.listaHidroA.Any())
                    {
                        ExcelWorksheet wsHidroA = ObtenerHoja(xlPackage.Workbook, "Hidro-A");

                        ProcesarLista(wsHidroA, centralHidro.listaHidroA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.Centralnombre;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = item.Cuenca;
                            ws.Cells[index, 11].Value = item.Rio;
                            ws.Cells[index, 12].Value = item.Propietario;
                            ws.Cells[index, 13].Value = item.Sociooperador;
                            ws.Cells[index, 14].Value = item.Socioinversionista;
                            ws.Cells[index, 15].Value = item.Concesiontemporal;
                            ws.Cells[index, 16].Value = item.Fechaconcesiontemporal;
                            ws.Cells[index, 17].Value = item.Tipoconcesionactual;
                            ws.Cells[index, 18].Value = item.Fechaconcesionactual;
                            ws.Cells[index, 19].Value = ConvertirFormatoRadio(item.Estudiogeologico);
                            ws.Cells[index, 20].Value = item.Perfodiamantinas;
                            ws.Cells[index, 21].Value = item.Numcalicatas;
                            ws.Cells[index, 22].Value = ConvertirFormatoRadio(item.EstudioTopografico);
                            ws.Cells[index, 23].Value = item.Levantamientotopografico;
                            ws.Cells[index, 24].Value = item.Alturabruta;
                            ws.Cells[index, 25].Value = item.Alturaneta;
                            ws.Cells[index, 26].Value = item.Caudaldiseno;
                            ws.Cells[index, 27].Value = item.Potenciainstalada;
                            ws.Cells[index, 28].Value = item.Conduccionlongitud;
                            ws.Cells[index, 29].Value = item.Tunelarea;
                            ws.Cells[index, 30].Value = item.Tuneltipo;
                            ws.Cells[index, 31].Value = item.Tuberialongitud;
                            ws.Cells[index, 32].Value = item.Tuberiadiametro;
                            ws.Cells[index, 33].Value = item.Tuberiatipo;
                            ws.Cells[index, 34].Value = item.Maquinatipo;
                            ws.Cells[index, 35].Value = item.Maquinaaltitud;
                            ws.Cells[index, 36].Value = item.Regestacionalvbruto;
                            ws.Cells[index, 37].Value = item.Regestacionalvutil;
                            ws.Cells[index, 38].Value = item.Regestacionalhpresa;
                            ws.Cells[index, 39].Value = item.Reghorariavutil;
                            ws.Cells[index, 40].Value = item.Reghorariahpresa;
                            ws.Cells[index, 41].Value = item.Reghorariaubicacion;
                            ws.Cells[index, 42].Value = item.Energhorapunta;
                            ws.Cells[index, 43].Value = item.Energfuerapunta;
                            ws.Cells[index, 44].Value = item.Tipoturbina;
                            ws.Cells[index, 45].Value = item.Velnomrotacion;
                            ws.Cells[index, 46].Value = item.Potturbina;
                            ws.Cells[index, 47].Value = item.Numturbinas;
                            ws.Cells[index, 48].Value = item.Potgenerador;
                            ws.Cells[index, 49].Value = item.Numgeneradores;
                            ws.Cells[index, 50].Value = item.Tensiongeneracion;
                            ws.Cells[index, 51].Value = item.Rendimientogenerador;
                            ws.Cells[index, 52].Value = item.Tensionkv;
                            ws.Cells[index, 53].Value = item.Longitudkm;
                            ws.Cells[index, 54].Value = item.Numternas;
                            ws.Cells[index, 55].Value = item.Nombresubestacion;
                            ws.Cells[index, 56].Value = item.Perfil;
                            ws.Cells[index, 57].Value = item.Prefactibilidad;
                            ws.Cells[index, 58].Value = item.Factibilidad;
                            ws.Cells[index, 59].Value = item.Estudiodefinitivo;
                            ws.Cells[index, 60].Value = item.Eia;
                            ws.Cells[index, 61].Value = item.Fechainicioconstruccion;
                            ws.Cells[index, 62].Value = item.Periodoconstruccion;
                            ws.Cells[index, 63].Value = item.Fechaoperacioncomercial;
                            ws.Cells[index, 64].Value = ConvertirFormatoRadio(item.Condifencial);
                        }, 3);
                        AplicarEstilosHoja(wsHidroA, 2);
                        AplicarFormatoFecha(wsHidroA, 16);
                        AplicarFormatoFecha(wsHidroA, 18);
                    }

                    if (centralHidro.listaHidroB != null && centralHidro.listaHidroB.Any())
                    {
                        ExcelWorksheet wsHidroB = ObtenerHoja(xlPackage.Workbook, "Hidro-B");

                        ProcesarLista(wsHidroB, centralHidro.listaHidroB, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = item.Obrasregulacion;
                            ws.Cells[index, 13].Value = item.Administracion;
                            ws.Cells[index, 14].Value = item.Aduanas;
                            ws.Cells[index, 15].Value = item.Supervision;
                            ws.Cells[index, 16].Value = item.Gastosgestion;
                            ws.Cells[index, 17].Value = item.Imprevistos;
                            ws.Cells[index, 18].Value = item.Igv;
                            ws.Cells[index, 19].Value = item.Usoagua;
                            ws.Cells[index, 20].Value = item.Otrosgastos;
                            ws.Cells[index, 21].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 22].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 23].Value = item.Financiamientotipo;
                            ws.Cells[index, 24].Value = item.Financiamientoestado;
                            ws.Cells[index, 25].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 26].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 27].Value = item.Ventaenergia;
                            ws.Cells[index, 28].Value = item.Ejecucionobra;
                            ws.Cells[index, 29].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsHidroB, 2);
                    }

                    if (centralHidro.listaHidroC != null && centralHidro.listaHidroC.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteCronograma(croEg, centralHidro.listaHidroC, anioPeriodo, horizonteFin, listaparam, 1);
                    } else
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                        xlPackage.Workbook.Worksheets.Delete(croEg);
                    }

                    if (centralHidro.listaFichaD != null && centralHidro.listaFichaD.Any())
                    {
                        ExcelWorksheet wsFichaD = ObtenerHoja(xlPackage.Workbook, "Ficha-D");

                        // Diccionario de meses a columnas
                        Dictionary<string, int> columnasMeses = new Dictionary<string, int>
                        {
                            { "Enero", 6 }, { "Febrero", 7 }, { "Marzo", 8 }, { "Abril", 9 }, { "Mayo", 10 },
                            { "Junio", 11 }, { "Julio", 12 }, { "Agosto", 13 }, { "Setiembre", 14 },
                            { "Octubre", 15 }, { "Noviembre", 16 }, { "Diciembre", 17 }
                        };

                        // Agrupar datos y ordenar correctamente
                        var datosAgrupados = centralHidro.listaFichaD
                            .SelectMany(item => item.ListDetRegHojaD
                                .Select(detalle => new
                                {
                                    Empresa = item.Empresa,
                                    Proyecto = item.Proyecto,
                                    Cuenca = item.Cuenca,
                                    Caudal = item.Caudal,
                                    Año = detalle.Anio,
                                    Mes = detalle.Mes,
                                    Valor = ConvertirAFormatoSeguro(detalle.Valor, 4)
                                })
                            )
                            .GroupBy(d => new { d.Empresa, d.Proyecto, d.Cuenca, d.Caudal, d.Año }) // Agrupa por Empresa, Proyecto, Cuenca, Caudal y Año
                            .OrderBy(g => g.Key.Empresa)
                            .ThenBy(g => g.Key.Proyecto)
                            .ThenBy(g => g.Key.Cuenca)
                            .ThenBy(g => g.Key.Caudal)
                            .ThenBy(g => g.Key.Año) // Ordenar por Empresa > Proyecto > Cuenca > Caudal > Año
                            .ToList();

                        int fila = 2;

                        foreach (var grupo in datosAgrupados)
                        {
                            // Escribir datos generales
                            wsFichaD.Cells[fila, 1].Value = grupo.Key.Empresa;
                            wsFichaD.Cells[fila, 2].Value = grupo.Key.Proyecto;
                            wsFichaD.Cells[fila, 3].Value = grupo.Key.Cuenca;
                            wsFichaD.Cells[fila, 4].Value = grupo.Key.Caudal;
                            wsFichaD.Cells[fila, 5].Value = grupo.Key.Año; // Año en la columna 5

                            // Llenar los valores en las columnas de meses
                            foreach (var detalle in grupo)
                            {
                                if (columnasMeses.ContainsKey(detalle.Mes))
                                {
                                    wsFichaD.Cells[fila, columnasMeses[detalle.Mes]].Value = detalle.Valor;
                                }
                            }

                            fila++; // Pasar a la siguiente fila para el siguiente grupo
                        }


                        AplicarEstilosHoja(wsFichaD, 1);
                    }


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Central Hidroelectrica: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteCentralesSolares(CentralSolarReporteDTO centralSolar, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProySolar));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProySolar));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProySolar));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (centralSolar.listaSolarA != null && centralSolar.listaSolarA.Any())
                    {
                        ExcelWorksheet wsSolarA = ObtenerHoja(xlPackage.Workbook, "CC.Sol.-A");

                        ProcesarLista(wsSolarA, centralSolar.listaSolarA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.Centralnombre;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = $"{item.Propietario} - {item.Otro}";
                            ws.Cells[index, 11].Value = item.Sociooperador;
                            ws.Cells[index, 12].Value = item.Socioinversionista;
                            ws.Cells[index, 13].Value = item.Concesiontemporal;
                            ws.Cells[index, 14].Value = item.Fechaconcesiontem;
                            ws.Cells[index, 15].Value = item.Tipoconcesionact;
                            ws.Cells[index, 16].Value = item.Fechaconcesionact;
                            ws.Cells[index, 17].Value = item.Nomestacion;
                            ws.Cells[index, 18].Value = item.Serieradiacion;
                            ws.Cells[index, 19].Value = item.Potinstnom;
                            ws.Cells[index, 20].Value = item.Ntotalmodfv;
                            ws.Cells[index, 21].Value = item.Horutilequ;
                            ws.Cells[index, 22].Value = item.Eneestanual;
                            ws.Cells[index, 23].Value = item.Facplantaact;
                            ws.Cells[index, 24].Value = item.Tecnologia;
                            ws.Cells[index, 25].Value = item.Potenciapico;
                            ws.Cells[index, 26].Value = item.Nivelradsol;
                            ws.Cells[index, 27].Value = item.Seguidorsol;
                            ws.Cells[index, 28].Value = item.Volpunmax;
                            ws.Cells[index, 29].Value = item.Intpunmax;
                            ws.Cells[index, 30].Value = item.Modelo;
                            ws.Cells[index, 31].Value = item.Entpotmax;
                            ws.Cells[index, 32].Value = item.Salpotmax;
                            ws.Cells[index, 33].Value = item.Siscontro;
                            ws.Cells[index, 34].Value = item.Baterias;
                            ws.Cells[index, 35].Value = item.Enemaxbat;
                            ws.Cells[index, 36].Value = item.Potmaxbat;
                            ws.Cells[index, 37].Value = item.Eficargamax;
                            ws.Cells[index, 38].Value = item.Efidesbat;
                            ws.Cells[index, 39].Value = item.Timmaxreg;
                            ws.Cells[index, 40].Value = item.Rampascardes;
                            ws.Cells[index, 41].Value = item.Tension;
                            ws.Cells[index, 42].Value = item.Longitud;

                            ws.Cells[index, 43].Value = item.Numternas;
                            ws.Cells[index, 44].Value = item.Nombsubest;
                            ws.Cells[index, 45].Value = item.Perfil;
                            ws.Cells[index, 46].Value = item.Prefact;
                            ws.Cells[index, 47].Value = item.Factibilidad;
                            ws.Cells[index, 48].Value = item.Estdefinitivo;
                            ws.Cells[index, 49].Value = item.Eia;
                            ws.Cells[index, 50].Value = item.Fecinicioconst;
                            ws.Cells[index, 51].Value = item.Perconstruccion;
                            ws.Cells[index, 52].Value = item.Fecoperacioncom;
                            ws.Cells[index, 53].Value = ConvertirFormatoRadio(item.Confidencial);
                        }, 3);
                        AplicarEstilosHoja(wsSolarA, 1);
                        AplicarFormatoFecha(wsSolarA, 14);
                        AplicarFormatoFecha(wsSolarA, 16);
                    }

                    if (centralSolar.listaSolarB != null && centralSolar.listaSolarB.Any())
                    {
                        ExcelWorksheet wsSolarB = ObtenerHoja(xlPackage.Workbook, "CC.Sol.-B");

                        ProcesarLista(wsSolarB, centralSolar.listaSolarB, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = item.Administracion;
                            ws.Cells[index, 13].Value = item.Aduanas;
                            ws.Cells[index, 14].Value = item.Supervision;
                            ws.Cells[index, 15].Value = item.Gastosgestion;
                            ws.Cells[index, 16].Value = item.Imprevistos;
                            ws.Cells[index, 17].Value = item.Igv;
                            ws.Cells[index, 18].Value = item.Otrosgastos;
                            ws.Cells[index, 19].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 20].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 21].Value = item.Financiamientotipo;
                            ws.Cells[index, 22].Value = item.Financiamientoestado;
                            ws.Cells[index, 23].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 24].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 25].Value = item.Ventaenergia;
                            ws.Cells[index, 26].Value = item.Ejecucionobra;
                            ws.Cells[index, 27].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsSolarB, 1);
                    }
                    if (centralSolar.listaSolarC != null && centralSolar.listaSolarC.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteCronograma(croEg, centralSolar.listaSolarC, anioPeriodo, horizonteFin, listaparam, 1);
                    }
                    else
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                        xlPackage.Workbook.Worksheets.Delete(croEg);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Central Solar: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteCentralesEolicas(CentralEolicaReporteDTO centralEolica, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyEolica));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyEolica));

            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyEolica));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (centralEolica.listaEolicaA != null && centralEolica.listaEolicaA.Any())
                    {
                        ExcelWorksheet wsHEolA = ObtenerHoja(xlPackage.Workbook, "CC.Eol.-A");
                        int filaInicial = 3; // Comienza desde la fila3
                        int filaActual = filaInicial;
                        ProcesarLista(wsHEolA, centralEolica.listaEolicaA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.CentralNombre;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = !string.IsNullOrWhiteSpace(item.OtroPropietario) ? $"{item.Propietario} - {item.OtroPropietario}" : item.Propietario;
                            ws.Cells[index, 11].Value = item.SocioOperador;
                            ws.Cells[index, 12].Value = item.SocioInversionista;
                            ws.Cells[index, 13].Value = item.ConcesionTemporal;
                            ws.Cells[index, 14].Value = item.FechaConcesionTemporal;
                            ws.Cells[index, 15].Value = item.TipoConcesionActual;
                            ws.Cells[index, 16].Value = item.FechaConcesionActual;
                            ws.Cells[index, 17].Value = item.NombreEstacionMet;
                            ws.Cells[index, 18].Value = item.NumEstacionMet;
                            ws.Cells[index, 19].Value = item.SerieVelViento;
                            ws.Cells[index, 20].Value = item.PeriodoDisAnio;
                            ws.Cells[index, 21].Value = item.EstudioGeologico;
                            ws.Cells[index, 22].Value = item.PerfoDiamantinas;
                            ws.Cells[index, 23].Value = item.NumCalicatas;
                            ws.Cells[index, 24].Value = item.EstudioTopografico;
                            ws.Cells[index, 25].Value = item.LevantamientoTopografico;
                            ws.Cells[index, 26].Value = item.PotenciaInstalada;
                            ws.Cells[index, 27].Value = item.VelVientoInstalada;
                            ws.Cells[index, 28].Value = item.HorPotNominal;
                            ws.Cells[index, 29].Value = item.VelConexion;
                            ws.Cells[index, 30].Value = item.VelDesconexion;
                            ws.Cells[index, 31].Value = item.TipoContrCentral;
                            ws.Cells[index, 32].Value = item.TipoTurbina;
                            ws.Cells[index, 33].Value = item.EnergiaAnual;
                            ws.Cells[index, 34].Value = item.TipoParqEolico;
                            ws.Cells[index, 35].Value = item.TipoTecGenerador;
                            ws.Cells[index, 36].Value = item.NumPalTurbina;
                            ws.Cells[index, 37].Value = item.DiaRotor;
                            ws.Cells[index, 38].Value = item.LongPala;
                            ws.Cells[index, 39].Value = item.AlturaTorre;
                            ws.Cells[index, 40].Value = item.PotNomGenerador;
                            ws.Cells[index, 41].Value = item.NumUnidades;
                            ws.Cells[index, 42].Value = item.NumPolos;
                            ws.Cells[index, 43].Value = item.TensionGeneracion;
                            ws.Cells[index, 44].Value = item.Bess;
                            ws.Cells[index, 45].Value = item.EnergiaMaxBat;
                            ws.Cells[index, 46].Value = item.PotenciaMaxBat;
                            ws.Cells[index, 47].Value = item.EfiCargaBat;
                            ws.Cells[index, 48].Value = item.EfiDescargaBat;
                            ws.Cells[index, 49].Value = item.TiempoMaxRegulacion;
                            ws.Cells[index, 50].Value = item.RampaCargDescarg;
                            ws.Cells[index, 51].Value = item.NumTernas;
                            ws.Cells[index, 52].Value = item.NombreSubestacion;
                            ws.Cells[index, 53].Value = item.Perfil;
                            ws.Cells[index, 54].Value = item.Prefactibilidad;
                            ws.Cells[index, 55].Value = item.Factibilidad;
                            ws.Cells[index, 56].Value = item.EstudioDefinitivo;
                            ws.Cells[index, 57].Value = item.Eia;
                            ws.Cells[index, 58].Value = item.FechaInicioConstruccion;
                            ws.Cells[index, 59].Value = item.PeriodoConstruccion;
                            ws.Cells[index, 60].Value = item.FechaOperacionComercial;
                            ws.Cells[index, 61].Value = ConvertirFormatoRadio(item.Confidencial);
                        }, 3);
                        AplicarEstilosHoja(wsHEolA, 2);
                        AplicarFormatoFecha(wsHEolA, 14);
                        AplicarFormatoFecha(wsHEolA, 16);

                        // Crear o acceder a la hoja PerfilTurbina
                        ExcelWorksheet wsPerfilTurbina = ObtenerHoja(xlPackage.Workbook, "PerfilTurbina");

                        // 1. Obtener todos los Speed únicos no nulos, ordenados
                        var listaSpeedGlobal = centralEolica.listaEolicaA
                            .Where(x => x.RegHojaEolADetDTOs != null)
                            .SelectMany(x => x.RegHojaEolADetDTOs)
                            .Select(x => x.Speed)
                            .Where(x => x.HasValue)
                            .Select(x => x.Value)
                            .Distinct()
                            .OrderBy(s => s)
                            .ToList();

                        // 2. Recorrer empresas y registrar nombres y Turbi_#
                        int columnaEmpresa = 2;
                        int contadorTurbi = 1;
                        var empresasProcesadas = new List<(string nombre, Dictionary<decimal, decimal?> accionaPorSpeed, List<decimal?> accionaSinSpeed)>();

                        foreach (var item in centralEolica.listaEolicaA)
                        {
                            if (item.RegHojaEolADetDTOs != null && item.RegHojaEolADetDTOs.Any())
                            {
                                // Fila 2: nombre de empresa
                                wsPerfilTurbina.Cells[2, columnaEmpresa].Value = item.Empresa;
                                wsPerfilTurbina.Cells[3, columnaEmpresa].Value = item.NombreProyecto;
                                // Fila 3: identificador Turbi_#
                                wsPerfilTurbina.Cells[4, columnaEmpresa].Value = $"Turbi_{contadorTurbi++}";
                                // Aplicar negrita a toda la fila 4
                                wsPerfilTurbina.Cells[4, 1, 4, columnaEmpresa].Style.Font.Bold = true;


                                // Diccionario para Acciona con Speed no nulo
                                var accionaPorSpeed = new Dictionary<decimal, decimal?>();

                                // Lista para Acciona con Speed == null
                                var accionaSinSpeed = new List<decimal?>();

                                foreach (var detalle in item.RegHojaEolADetDTOs)
                                {
                                    if (detalle == null) continue;

                                    if (detalle.Speed.HasValue)
                                    {
                                        decimal key = detalle.Speed.Value;

                                        if (!accionaPorSpeed.ContainsKey(key))
                                        {
                                            accionaPorSpeed[key] = detalle.Acciona;
                                        }
                                    }
                                    else
                                    {
                                        accionaSinSpeed.Add(detalle.Acciona);
                                    }
                                }

                                empresasProcesadas.Add((item.Empresa, accionaPorSpeed, accionaSinSpeed));
                                columnaEmpresa++;
                            }
                        }

                        // 3. Encabezado columna A
                        wsPerfilTurbina.Cells[1, 1, 1, columnaEmpresa - 1].Merge = true;


                        // 4. Fila 5+: escribir los Speed ordenados en columna A
                        int filaBase =5;
                        for (int i = 0; i < listaSpeedGlobal.Count; i++)
                        {
                            wsPerfilTurbina.Cells[filaBase + i, 1].Value = listaSpeedGlobal[i].ToString("0.##");
                        }

                        // 5. Completar los valores de Acciona por empresa alineados con Speed
                        for (int i = 0; i < empresasProcesadas.Count; i++)
                        {
                            var empresa = empresasProcesadas[i];
                            int columna = 2 + i;

                            for (int j = 0; j < listaSpeedGlobal.Count; j++)
                            {
                                var speed = listaSpeedGlobal[j];

                                if (empresa.accionaPorSpeed.TryGetValue(speed, out var acciona))
                                {
                                    wsPerfilTurbina.Cells[filaBase + j, columna].Value = acciona;
                                }
                            }

                            // Si hay Accionas sin Speed, colócalos al final del Excel (una debajo de la última fila)
                            int filaFinal = filaBase + listaSpeedGlobal.Count;
                            foreach (var accionaSinSpeed in empresa.accionaSinSpeed)
                            {
                                wsPerfilTurbina.Cells[filaFinal++, columna].Value = accionaSinSpeed;
                            }
                        }
                        AplicarEstilosHoja(wsPerfilTurbina);



                    }

                    if (centralEolica.listaEolicaB != null && centralEolica.listaEolicaB.Any())
                    {
                        ExcelWorksheet wsEolB = ObtenerHoja(xlPackage.Workbook, "CC.Eol.-B");

                        ProcesarLista(wsEolB, centralEolica.listaEolicaB, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = item.Administracion;
                            ws.Cells[index, 13].Value = item.Aduanas;
                            ws.Cells[index, 14].Value = item.Supervision;
                            ws.Cells[index, 15].Value = item.Gastosgestion;
                            ws.Cells[index, 16].Value = item.Imprevistos;
                            ws.Cells[index, 17].Value = item.Igv;
                            ws.Cells[index, 18].Value = item.Otrosgastos;
                            ws.Cells[index, 19].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 20].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 21].Value = item.Financiamientotipo;
                            ws.Cells[index, 22].Value = item.Financiamientoestado;
                            ws.Cells[index, 23].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 24].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 25].Value = item.Ventaenergia;
                            ws.Cells[index, 26].Value = item.Ejecucionobra;
                            ws.Cells[index, 27].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsEolB, 2);
                    }
                    if (centralEolica.listaEolicaC != null && centralEolica.listaEolicaC.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteCronograma(croEg, centralEolica.listaEolicaC, anioPeriodo, horizonteFin, listaparam, 1);
                    }
                    else
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                        xlPackage.Workbook.Worksheets.Delete(croEg);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Central Eolica: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteCentralesBiomasa(CentralBiomasaReporteDTO centralBiomasa, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyBiomasa));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyBiomasa));
            
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyBiomasa));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (centralBiomasa.listaBioA != null && centralBiomasa.listaBioA.Any())
                    {
                        ExcelWorksheet wsBioA = ObtenerHoja(xlPackage.Workbook, "CC.BIO.-A");

                        ProcesarLista(wsBioA, centralBiomasa.listaBioA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.CentralNombre;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = !string.IsNullOrWhiteSpace(item.Otro) ? $"{item.Propietario} - {item.Otro}": item.Propietario;
                            ws.Cells[index, 11].Value = item.SocioOperador;
                            ws.Cells[index, 12].Value = item.SocioInversionista;
                            ws.Cells[index, 13].Value = item.ConTemporal;
                            ws.Cells[index, 14].Value = item.FecAdjudicacionTemp;
                            ws.Cells[index, 15].Value = item.TipoConActual;
                            ws.Cells[index, 16].Value = item.FecAdjudicacionAct;
                            ws.Cells[index, 17].Value = item.PotInstalada;
                            ws.Cells[index, 18].Value = item.PotMaxima;
                            ws.Cells[index, 19].Value = item.PotMinima;
                            ws.Cells[index, 20].Value = $"{item.TipoNomComb} - {item.OtroComb}";
                            ws.Cells[index, 21].Value = item.PoderCalorInf;
                            ws.Cells[index, 22].Value = item.CombPoderCalorInf;
                            ws.Cells[index, 23].Value = item.PoderCalorSup;
                            ws.Cells[index, 24].Value = item.CombPoderCalorSup;
                            ws.Cells[index, 25].Value = item.CostCombustible;
                            ws.Cells[index, 26].Value = item.CombCostoCombustible;
                            ws.Cells[index, 27].Value = item.CostTratamiento;
                            ws.Cells[index, 28].Value = item.CombCostTratamiento;
                            ws.Cells[index, 29].Value = item.CostTransporte;
                            ws.Cells[index, 30].Value = item.CombCostTransporte;
                            ws.Cells[index, 31].Value = item.CostoVariableNoComb;
                            ws.Cells[index, 32].Value = item.CombCostoVariableNoComb;
                            ws.Cells[index, 33].Value = item.CostInversion;
                            ws.Cells[index, 34].Value = item.CombCostoInversion;
                            ws.Cells[index, 35].Value = item.RendPlanta;
                            ws.Cells[index, 36].Value = item.CombRendPlanta;
                            ws.Cells[index, 37].Value = item.ConsEspec;
                            ws.Cells[index, 38].Value = item.CombConsEspec;
                            ws.Cells[index, 39].Value = item.TipoMotorTer;
                            ws.Cells[index, 40].Value = item.VelNomRotacion;
                            ws.Cells[index, 41].Value = item.PotEjeMotorTer;
                            ws.Cells[index, 42].Value = item.NumMotoresTer;
                            ws.Cells[index, 43].Value = item.PotNomGenerador;
                            ws.Cells[index, 44].Value = item.NumGeneradores;
                            ws.Cells[index, 45].Value = item.TenGeneracion;
                            ws.Cells[index, 46].Value = "-";
                            ws.Cells[index, 47].Value = item.Tension;
                            ws.Cells[index, 48].Value = item.Longitud;
                            ws.Cells[index, 49].Value = item.NumTernas;
                            ws.Cells[index, 50].Value = !string.IsNullOrWhiteSpace(item.OtroSubEstacion) ? item.OtroSubEstacion : item.NomSubEstacion;
                            ws.Cells[index, 51].Value = item.Perfil;
                            ws.Cells[index, 52].Value = item.Prefactibilidad;
                            ws.Cells[index, 53].Value = item.Factibilidad;
                            ws.Cells[index, 54].Value = item.EstDefinitivo;
                            ws.Cells[index, 55].Value = item.Eia;
                            ws.Cells[index, 56].Value = item.FecInicioConst;
                            ws.Cells[index, 57].Value = item.PeriodoConst;
                            ws.Cells[index, 58].Value = item.FecOperacionComer;
                            ws.Cells[index, 59].Value = ConvertirFormatoRadio(item.Condifencial);
                        }, 3);
                        AplicarEstilosHoja(wsBioA, 2);
                        AplicarFormatoFecha(wsBioA, 14);
                        AplicarFormatoFecha(wsBioA, 16);
                    }

                    if (centralBiomasa.listaBioB != null && centralBiomasa.listaBioB.Any())
                    {
                        ExcelWorksheet wsBioB = ObtenerHoja(xlPackage.Workbook, "CC.BIO.-B");

                        ProcesarLista(wsBioB, centralBiomasa.listaBioB, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = item.SubTipoProyecto;
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = item.Obrasregulacion;
                            ws.Cells[index, 13].Value = item.Administracion;
                            ws.Cells[index, 14].Value = item.Aduanas;
                            ws.Cells[index, 15].Value = item.Supervision;
                            ws.Cells[index, 16].Value = item.Gastosgestion;
                            ws.Cells[index, 17].Value = item.Imprevistos;
                            ws.Cells[index, 18].Value = item.Igv;
                            ws.Cells[index, 19].Value = item.Otrosgastos;
                            ws.Cells[index, 20].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 21].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 22].Value = item.Financiamientotipo;
                            ws.Cells[index, 23].Value = item.Financiamientoestado;
                            ws.Cells[index, 24].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 25].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 26].Value = item.Ventaenergia;
                            ws.Cells[index, 27].Value = item.Ejecucionobra;
                            ws.Cells[index, 28].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsBioB, 2);
                    }
                    if (centralBiomasa.listaBioC != null && centralBiomasa.listaBioC.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteCronograma(croEg, centralBiomasa.listaBioC, anioPeriodo, horizonteFin, listaparam, 1);
                    }
                    else
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                        xlPackage.Workbook.Worksheets.Delete(croEg);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Central Biomasa: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteLineasTransmision(LineasReporteDTO Lineas, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyLinea));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyLinea));
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyLinea));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (Lineas.ListaLineaA != null && Lineas.ListaLineaA.Any())
                    {
                        ExcelWorksheet wsLinA = ObtenerHoja(xlPackage.Workbook, "Datos Generales");

                        ProcesarLista(wsLinA, Lineas.ListaLineaA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreLinea;
                            ws.Cells[index, 3].Value = item.FecPuestaServ;
                            ws.Cells[index, 4].Value = (!string.IsNullOrEmpty(item.OtroSubInicio)) ? item.OtroSubInicio : item.SubInicio;
                            ws.Cells[index, 5].Value = (!string.IsNullOrEmpty(item.OtroSubFin)) ? item.OtroSubFin : item.SubFin;
                            ws.Cells[index, 6].Value = item.EmpPropietaria;
                            ws.Cells[index, 7].Value = item.NivTension;
                            ws.Cells[index, 8].Value = item.CapCorriente;
                            ws.Cells[index, 9].Value = item.CapCorrienteA;
                            ws.Cells[index, 10].Value = item.TpoSobreCarga;
                            ws.Cells[index, 11].Value = item.NumTemas;
                            ws.Cells[index, 12].Value = item.LongTotal;
                            ws.Cells[index, 13].Value = "-"; // archivos adjuntos
                            ws.Cells[index, 14].Value = item.LongVanoPromedio;
                            ws.Cells[index, 15].Value = item.TipMatSop;
                            ws.Cells[index, 16].Value = item.DesProtecPrincipal;
                            ws.Cells[index, 17].Value = item.DesProtecRespaldo;
                            ws.Cells[index, 18].Value = item.DesGenProyecto;
                            ws.Cells[index, 19].Value = ConvertirFormatoRadio(item.Condifencial);
                        }, 4);
                        AplicarEstilosHoja(wsLinA, 2);
                    }

                    if (Lineas.ListaLineaATramo != null && Lineas.ListaLineaATramo.Any())
                    {
                        ExcelWorksheet wsLinAT = ObtenerHoja(xlPackage.Workbook, "Linea de Transmisión (Tramos)");

                        ProcesarListaColum(wsLinAT, Lineas.ListaLineaATramo, (ws, item, index) =>
                        {
                            ws.Cells[2, index].Value = item.Empresa;
                            ws.Cells[3, index].Value = item.NombreLinea;
                            ws.Cells[4, index].Value =$"Tramo {item.Tramo}";
                            ws.Cells[5, index].Value = item.Tipo;
                            ws.Cells[6, index].Value = item.Longitud;
                            ws.Cells[7, index].Value = item.MatConductor;
                            ws.Cells[8, index].Value = item.SecConductor;
                            ws.Cells[9, index].Value = item.ConductorFase;
                            ws.Cells[10, index].Value = item.CapacidadTot;
                            ws.Cells[11, index].Value = item.CabGuarda;
                            ws.Cells[12, index].Value = item.ResistCabGuarda;
                            ws.Cells[13, index].Value = item.R;
                            ws.Cells[14, index].Value = item.X;
                            ws.Cells[15, index].Value = item.B;
                            ws.Cells[16, index].Value = item.G;
                            ws.Cells[17, index].Value = item.R0;
                            ws.Cells[18, index].Value = item.X0;
                            ws.Cells[19, index].Value = item.B0;
                            ws.Cells[20, index].Value = item.G0;
                        }, 2);
                        using (var range = wsLinAT.Cells[1, 2, 1, Lineas.ListaLineaATramo.Count + 1])
                        {
                            range.Value = "LÍNEAS DE TRANSMISIÓN";
                            range.Merge = true;
                            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                        }  
                        AplicarEstilosHoja(wsLinAT, 0);
                    }
                    if (Lineas.ListaLineaB != null && Lineas.ListaLineaB.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "Cronogram De Ejecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        GenerarHojaReporteCronogramaH(croEg, Lineas.ListaLineaB, anioPeriodo, horizonteFin, listaparam, 1);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Lineas: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteTransformadores(TransformReporteDTO Trans, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, List<DataCatalogoDTO> ListCatalogoTrans, List<DataCatalogoDTO> ListCatalogoEqui, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyTransform));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyTransform));
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyTransform));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (Trans.ListaT2Subest1 != null && Trans.ListaT2Subest1.Any())
                    {
                        ExcelWorksheet wsTransA = ObtenerHoja(xlPackage.Workbook, "Datos Generales");

                        ProcesarLista(wsTransA, Trans.ListaT2Subest1, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.EmpresaPropietaria;
                            ws.Cells[index, 2].Value = item.NombreSubestacion;
                            ws.Cells[index, 3].Value = item.FechaPuestaServicio;
                            ws.Cells[index, 4].Value = item.TipoProyecto;
                            ws.Cells[index, 5].Value = ConvertirFormatoRadio(item.Confidencial);
                            ws.Cells[index, 6].Value = !string.IsNullOrWhiteSpace(item.OtroSistemaBarras) ? $"{item.SistemaBarras} - {item.OtroSistemaBarras}" : item.SistemaBarras;
                        }, 4);
                        AplicarEstilosHoja(wsTransA, 2);
                    }

                    if (Trans.ListaT2Subest1Trans != null && Trans.ListaT2Subest1Trans.Any())
                    {
                        ExcelWorksheet wsSubes1E = ObtenerHoja(xlPackage.Workbook, "Transformadores");

                        GenerarHojaReporteSubestacion(wsSubes1E, Trans.ListaT2Subest1Trans, ListCatalogoTrans, 1, "Trafo");
                        using (var range = wsSubes1E.Cells[1, 1, 1, Trans.ListaT2Subest1Trans.Count + 2])
                        {
                            range.Value = "TRANSFORMADORES DE POTENCIA";
                            range.Merge = true;
                            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                        }
                        AplicarEstilosHoja(wsSubes1E, 0);
                    }

                    if (Trans.ListaT2Subest1Equi != null && Trans.ListaT2Subest1Equi.Any())
                    {
                        ExcelWorksheet wsSubes1E = ObtenerHoja(xlPackage.Workbook, "Equipos de Comp. Reactiva");

                        GenerarHojaReporteSubestacion(wsSubes1E, Trans.ListaT2Subest1Equi, ListCatalogoEqui, 1, "Equipo");
                        using (var range = wsSubes1E.Cells[1, 1, 1, Trans.ListaT2Subest1Equi.Count + 2])
                        {
                            range.Value = "EQUIPOS DE COMPENSACIÓN REACTIVA";
                            range.Merge = true;
                            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                        }
                        AplicarEstilosHoja(wsSubes1E, 0);
                    }
                    if (Trans.ListaT3Crono != null && Trans.ListaT3Crono.Any())
                    {
                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "Cronogram De Ejecución");

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        GenerarHojaReporteCronogramaH(croEg, Trans.ListaT3Crono, anioPeriodo, horizonteFin, listaparam, 1);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Transformadores: " + ex.Message, ex);
            }
        }

        private static void GenerarReporteDistribuidoPorReflexion(IEnumerable<object> lista, ExcelWorksheet ws, int filaInicio)
        {
            if (lista == null || !lista.Any()) return;

            int row = filaInicio;

            var datosAgrupados = lista
                .GroupBy(x => new {
                    Empresa = x.GetType().GetProperty("Empresa")?.GetValue(x)?.ToString(),
                    ProyCodi = x.GetType().GetProperty("ProyCodi")?.GetValue(x)?.ToString(),
                    Anio = Convert.ToInt32(x.GetType().GetProperty("Anio")?.GetValue(x))
                })
                .OrderBy(g => g.Key.Empresa)
                .ThenBy(g => g.Key.ProyCodi)
                .ThenBy(g => g.Key.Anio)
                .ToList();

            string[] meses = new[]
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
            };

            foreach (var grupo in datosAgrupados)
            {
                int rowInicioAnio = row;

                foreach (var mes in meses)
                {
                    var item = grupo.FirstOrDefault(x =>
                        x.GetType().GetProperty("Mes")?.GetValue(x)?.ToString() == mes);

                    ws.Cells[row, 2].Value = grupo.Key.Empresa;
                    ws.Cells[row, 3].Value = item != null
                    ? item.GetType().GetProperty("NombreProyecto")?.GetValue(item)?.ToString()
                    : grupo.FirstOrDefault()?.GetType().GetProperty("NombreProyecto")?.GetValue(grupo.FirstOrDefault())?.ToString();

                    ws.Cells[row, 4].Value = grupo.Key.Anio;
                    ws.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[row, 5].Value = mes;

                    string[] propiedades = {
                "DemandaEnergia", "DemandaHP", "DemandaHFP",
                "GeneracionEnergia", "GeneracionHP", "GeneracionHFP"
            };

                    for (int i = 0; i < propiedades.Length; i++)
                    {
                        var valor = item?.GetType().GetProperty(propiedades[i])?.GetValue(item);
                        ws.Cells[row, 6 + i].Value = valor;
                    }

                    for (int col = 2; col <= 11; col++)
                    {
                        ws.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        if (col >= 6)
                            ws.Cells[row, col].Style.Numberformat.Format = "#,##0.0000";
                    }

                    row++;
                }

                ws.Cells[rowInicioAnio, 4, row - 1, 4].Merge = true;
                ws.Cells[rowInicioAnio, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowInicioAnio, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        public static void GenerarReporteGeneracionDistribuida(GenDistribuidaReporteDTO GenDistribuida, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyGenDistrib));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyGenDistrib));
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyGenDistrib));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (GenDistribuida.ListaDistA != null && GenDistribuida.ListaDistA.Any())
                    {
                        ExcelWorksheet wsBioA = ObtenerHoja(xlPackage.Workbook, "CC.GD.-A");

                        ProcesarLista(wsBioA, GenDistribuida.ListaDistA, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = "-";
                            ws.Cells[index, 5].Value = item.DetalleProyecto;
                            ws.Cells[index, 6].Value = item.NombreUnidad;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 7].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 8].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 9].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 10].Value = item.Propietario;
                            ws.Cells[index, 11].Value = item.SocioOperador;
                            ws.Cells[index, 12].Value = item.SocioInversionista;
                            ws.Cells[index, 13].Value = CombinarTexto(item.ObjetivoProyecto, item.OtroObjetivo);
                            ws.Cells[index, 14].Value = item.IncluidoPlanTrans == "S" ? "Sí" : item.IncluidoPlanTrans == "N" ? "No" : ""; ;
                            ws.Cells[index, 15].Value = item.EstadoOperacion;
                            ws.Cells[index, 16].Value = item.CargaRedDistribucion == "S" ? "Sí" : item.CargaRedDistribucion == "N" ? "No" : "";
                            ws.Cells[index, 17].Value = item.ConexionTemporal == "S" ? "Si" : item.ConexionTemporal == "N" ? "No" : "";

                            // Validación y conversión de FechaAdjudicactem
                            ws.Cells[index, 18].Value = item.FechaAdjudicactem.HasValue
                                ? item.FechaAdjudicactem.Value.ToString("dd/MM/yyyy")
                                : "";
                            ws.Cells[index, 19].Value = item.TipoTecnologia;
                            ws.Cells[index, 20].Value = item.FechaAdjutitulo.HasValue
                                ? item.FechaAdjutitulo.Value.ToString("dd/MM/yyyy")
                                : "";

                            
                            ws.Cells[index, 21].Value = item.PotInstalada;
                            ws.Cells[index, 22].Value = item.RecursoUsada;
                            ws.Cells[index, 23].Value = item.Tecnologia;
                            ws.Cells[index, 24].Value = item.TecOtro;
                            ws.Cells[index, 25].Value = item.BarraConexion;
                            ws.Cells[index, 26].Value = item.NivelTension;
                            ws.Cells[index, 27].Value = item.IncluidoPlanTransGD == "S" ? "Sí" : item.IncluidoPlanTransGD == "N" ? "No" : "";
                            ws.Cells[index, 28].Value = item.NombreProyectoGD;

                            if (item.ubicacionDTO2 != null)
                            {
                                ws.Cells[index, 29].Value = item.ubicacionDTO2.Departamento;
                                ws.Cells[index, 30].Value = item.ubicacionDTO2.Provincia;
                                ws.Cells[index, 31].Value = item.ubicacionDTO2.Distrito;
                            }

                            ws.Cells[index, 32].Value = item.NomDistribuidorGD;
                            ws.Cells[index, 33].Value = item.PropietarioGD;
                            ws.Cells[index, 34].Value = item.SocioOperadorGD;
                            ws.Cells[index, 35].Value = item.SocioInversionistaGD;
                            ws.Cells[index, 36].Value = item.EstadoOperacionGD;
                            ws.Cells[index, 37].Value = item.CargaRedDistribucionGD == "S" ? "Sí" : item.CargaRedDistribucionGD == "N" ? "No" : "";
                            ws.Cells[index, 38].Value = item.BarraConexionGD;
                            ws.Cells[index, 39].Value = item.NivelTensionGD;
                            ws.Cells[index, 40].Value = "-";
                            ws.Cells[index, 41].Value = "-";
                            ws.Cells[index, 42].Value = item.Perfil;
                            ws.Cells[index, 43].Value = item.Prefactibilidad;
                            ws.Cells[index, 44].Value = item.Factibilidad;
                            ws.Cells[index, 45].Value = item.EstDefinitivo;
                            ws.Cells[index, 46].Value = item.Eia;
                            ws.Cells[index, 47].Value = item.FechaInicioConst;
                            ws.Cells[index, 48].Value = item.PeriodoConst;
                            ws.Cells[index, 49].Value = item.FechaOpeComercial;
                            ws.Cells[index, 50].Value = item.Confidencial == "S" ? "Sí" : item.Confidencial == "N" ? "No" : "";
                        }, 3);
                        AplicarEstilosHoja(wsBioA, 2);
                    }


                    // ListaDistB → Hoja "CC.GD-B"
                    if (GenDistribuida.ListaDistB != null && GenDistribuida.ListaDistB.Any())
                    {
                        var wsB = ObtenerHoja(xlPackage.Workbook, "CC.GD-B");
                        GenerarReporteDistribuidoPorReflexion(GenDistribuida.ListaDistB.Cast<object>(), wsB, 5);
                    }

                    // if (GenDistribuida.ListaDistC1 != null && GenDistribuida.ListaDistC1.Any())
                    // {
                    //     var wsC1 = ObtenerHoja(xlPackage.Workbook, "CC.GD-C-1");
                    //     GenerarReporteDistribuidoPorReflexion(GenDistribuida.ListaDistC1.Cast<object>(), wsC1, 6);
                    // }

                    // if (GenDistribuida.ListaDistC2 != null && GenDistribuida.ListaDistC2.Any())
                    // {
                    //     var wsC2 = ObtenerHoja(xlPackage.Workbook, "CC.GD-C-2");
                    //     GenerarReporteDistribuidoPorReflexion(GenDistribuida.ListaDistC2.Cast<object>(), wsC2, 6);
                    // }

                    if (GenDistribuida.ListaDistC != null && GenDistribuida.ListaDistC.Any())
                    {
                        var wsC = ObtenerHoja(xlPackage.Workbook, "CC.GD-C-1_2");
                        ProcesarLista(wsC, GenDistribuida.ListaDistC, (ws, item, index) =>
                        {
                            ws.Cells[index, 2].Value = item.Empresa;
                            ws.Cells[index, 3].Value = item.NombreProyecto;
                            ws.Cells[index, 4].Value = item.Escenario;
                            ws.Cells[index, 5].Value = item.Anio;
                            ws.Cells[index, 6].Value = item.DemandaEnergia;
                            ws.Cells[index, 7].Value = item.DemandaHP;
                            ws.Cells[index, 8].Value = item.DemandaHFP;
                            ws.Cells[index, 9].Value = item.GeneracionEnergia;
                            ws.Cells[index, 10].Value = item.GeneracionHP;
                            ws.Cells[index, 11].Value = item.GeneracionHFP;
                            ws.Cells[index, 6, index, 11].Style.Numberformat.Format = "#,##0.0000";
                        }, 7);
                        int totalFilas = wsC.Dimension.End.Row;
                        int totalColumnas = wsC.Dimension.End.Column;
                        using (ExcelRange rango = wsC.Cells[3, 2, totalFilas, totalColumnas])
                        {
                            rango.Style.Font.Color.SetColor(Color.Black);
                            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                    }

                    if (GenDistribuida.ListaDistD != null && GenDistribuida.ListaDistD.Any())
                    {
                        var horasFijas = new List<string>
    {
        "00:30","01:00","01:30","02:00","02:30","03:00","03:30","04:00","04:30","05:00",
        "05:30","06:00","06:30","07:00","07:30","08:00","08:30","09:00","09:30","10:00",
        "10:30","11:00","11:30","12:00","12:30","13:00","13:30","14:00","14:30","15:00",
        "15:30","16:00","16:30","17:00","17:30","18:00","18:30","19:00","19:30","20:00",
        "20:30","21:00","21:30","22:00","22:30","23:00","23:30","00:00"
    };

                        var datosAgrupados = GenDistribuida.ListaDistD
                            .Where(x => x.IndDel != "1")
                            .GroupBy(x => x.ProyCodi)
                            .ToList();

                        foreach (var nombreHoja in new[] { "CC.GD-D-1", "CC.GD-D-2" })
                        {
                            ExcelWorksheet wsD = ObtenerHoja(xlPackage.Workbook, nombreHoja);
                            int colIndex = 5;
                            int contador = 1;

                            foreach (var grupo in datosAgrupados)
                            {
                                var primerItem = grupo.FirstOrDefault();

                                // Fila 3: Empresa
                                wsD.Cells[3, colIndex].Value = primerItem?.Empresa ?? "";
                                wsD.Cells[3, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                wsD.Cells[3, colIndex].Style.Font.Bold = true;

                                // Fila 4: Proyecto
                                wsD.Cells[4, colIndex].Value = primerItem?.NombreProyecto ?? "";
                                wsD.Cells[4, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                string elvalor = "";
                                // Fila 5: G1, G2, etc.
                                if (nombreHoja == "CC.GD-D-1")
                                {
                                     elvalor = "D";
                                }
                                else if (nombreHoja == "CC.GD-D-2")
                                {
                                     elvalor = "G";
                                }
                                wsD.Cells[5, colIndex].Value = elvalor + contador;
                                wsD.Cells[5, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                contador++;

                                // Fila 6+: Horas y valores
                                for (int i = 0; i < horasFijas.Count; i++)
                                {
                                    // Columna D: Hora
                                    wsD.Cells[i + 6, 4].Value = horasFijas[i];

                                    var registro = grupo.FirstOrDefault(x => x.Hora == horasFijas[i]);
                                    if (registro != null)
                                    {
                                        if (nombreHoja == "CC.GD-D-1")
                                        {
                                            wsD.Cells[i + 6, colIndex].Value = registro.Demanda;
                                        }
                                        else if (nombreHoja == "CC.GD-D-2")
                                        {
                                            wsD.Cells[i + 6, colIndex].Value = registro.Generacion;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException("Nombre de hoja no válido: " + nombreHoja);
                                        }
                                    }

                                    wsD.Cells[i + 6, colIndex].Style.Numberformat.Format = "#,##0.0";
                                }
                                wsD.Column(colIndex).AutoFit();
                                if (wsD.Column(colIndex).Width > 25)
                                {
                                    wsD.Column(colIndex).Width = 25;
                                }
                                colIndex++;
                            }

                            // Aplicar bordes a toda la tabla
                            int filaInicio = 3;
                            int filaFin = 6 + horasFijas.Count - 1;
                            int colInicio = 4;
                            int colFin = colIndex - 1;

                            using (var range = wsD.Cells[filaInicio, colInicio, filaFin, colFin])
                            {
                                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                        }
                    }




                    if (GenDistribuida.ListaDistE != null && GenDistribuida.ListaDistE.Any())
                    {
                        ExcelWorksheet wsBioE = ObtenerHoja(xlPackage.Workbook, "CC.GD.-E");

                        ProcesarLista(wsBioE, GenDistribuida.ListaDistE, (ws, item, index) =>
                        {
                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.NombreProyecto;
                            ws.Cells[index, 3].Value = item.TipoProyecto;
                            ws.Cells[index, 4].Value = "-";
                            ws.Cells[index, 5].Value = item.Estudiofactibilidad;
                            ws.Cells[index, 6].Value = item.Investigacionescampo;
                            ws.Cells[index, 7].Value = item.Gestionesfinancieras;
                            ws.Cells[index, 8].Value = item.Disenospermisos;
                            ws.Cells[index, 9].Value = item.Obrasciviles;
                            ws.Cells[index, 10].Value = item.Equipamiento;
                            ws.Cells[index, 11].Value = item.Lineatransmision;
                            ws.Cells[index, 12].Value = "-";
                            ws.Cells[index, 13].Value = item.Administracion;
                            ws.Cells[index, 14].Value = item.Aduanas;
                            ws.Cells[index, 15].Value = item.Supervision;
                            ws.Cells[index, 16].Value = item.Gastosgestion;
                            ws.Cells[index, 17].Value = item.Imprevistos;
                            ws.Cells[index, 18].Value = item.Igv;
                            ws.Cells[index, 19].Value = "-";
                            ws.Cells[index, 20].Value = item.Otrosgastos;
                            ws.Cells[index, 21].Value = item.Inversiontotalsinigv;
                            ws.Cells[index, 22].Value = item.Inversiontotalconigv;
                            ws.Cells[index, 23].Value = item.Financiamientotipo;
                            ws.Cells[index, 24].Value = item.Financiamientoestado;
                            ws.Cells[index, 25].Value = item.Porcentajefinanciado;
                            ws.Cells[index, 26].Value = item.Concesiondefinitiva;
                            ws.Cells[index, 27].Value = item.Ventaenergia;
                            ws.Cells[index, 28].Value = item.Ejecucionobra;
                            ws.Cells[index, 29].Value = item.Contratosfinancieros;
                        }, 3);
                        AplicarEstilosHoja(wsBioE, 2);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Generación Distribuida: " + ex.Message, ex);
            }
        }

        public static void GenerarReporteHidrogenoVerde(CuestionarioH2VReporteDTO cuestionarioH2VReporte, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyHidrogeno));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyHidrogeno));
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyHidrogeno));
            }

            int index = 3;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = ObtenerHoja(xlPackage.Workbook, "CC.H2V.-A");

                if (cuestionarioH2VReporte.dataH2VA != null && cuestionarioH2VReporte.dataH2VA.Any())
                {
                    // Obtener cabeceras desde listaparam con CatCodi == 41, ordenados por ID (DataCatCodi)
                    var cabecerasPermisos = listaparam
                        .Where(p => p.CatCodi == 41)
                        //.OrderBy(p => p.DataCatCodi) // Ordenar por ID
                        .ToList();

                    int anchoMaximo = 100; // puedes ajustarlo si deseas otro límite

                    for (int i = 0; i < cabecerasPermisos.Count; i++)
                    {
                        var celda = ws.Cells[2, 30 + i];
                        celda.Value = i < cabecerasPermisos.Count ? cabecerasPermisos[i].DesDataCat : "";
                        celda.Style.Font.Bold = true;
                        celda.Style.WrapText = true;
                        celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Ajustar el ancho de la columna según el contenido, con un límite
                        int textoLength = celda.Value?.ToString().Length ?? 10;
                        double anchoCalculado = Math.Min(textoLength + 5, anchoMaximo);
                        ws.Column(30 + i).Width = anchoCalculado;

                        // ➕ Aplicar mismo estilo que los años
                        celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celda.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#156082"));
                        celda.Style.Font.Color.SetColor(Color.White);

                        //celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    // Paso 1: Obtener todos los años únicos de todos los proyectos
                    var todosLosAnios = cuestionarioH2VReporte.dataH2VA
                        .SelectMany(x => x.ListCH2VADet1DTOs ?? new List<CuestionarioH2VADet1DTO>())
                        .Where(x => x.Anio.HasValue)
                        .Select(x => x.Anio.Value)
                        .Distinct()
                        .OrderBy(x => x)
                        .ToList();

                    int colBase = 30+ cabecerasPermisos.Count;

                    // Paso 2: Escribir los años en la fila 2
                    for (int i = 0; i < todosLosAnios.Count; i++)
                    {
                        ws.Cells[2, colBase + i].Value = todosLosAnios[i];
                        ws.Cells[2, colBase + i].Style.Font.Bold = true;
                    }
                    if (todosLosAnios.Count > 0)
                    {
                        int colInicio = 30 + cabecerasPermisos.Count;
                        int colFin = colInicio + todosLosAnios.Count - 1;

                        // Fila 1 - texto fusionado
                        var celdaMerge = ws.Cells[1, colInicio, 1, colFin];
                        celdaMerge.Merge = true;
                        celdaMerge.Value = "Inversiones Estimadas/Periodos";
                        celdaMerge.Style.Font.Bold = true;
                        celdaMerge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        celdaMerge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        celdaMerge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celdaMerge.Style.Fill.BackgroundColor.SetColor(Color.White); // fondo blanco
                        celdaMerge.Style.Font.Color.SetColor(Color.Black); // texto negro

                        // Fila 2 - años
                        for (int i = 0; i < todosLosAnios.Count; i++)
                        {
                            var celda = ws.Cells[2, colInicio + i];
                            celda.Value = todosLosAnios[i];
                            celda.Style.Font.Bold = true;
                            celda.Style.Font.Color.SetColor(Color.White);
                            celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            celda.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#156082")); // fondo verde azulado oscuro

                            //// Aplicar bordes a cada celda
                            //celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            //celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            //celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }


                    foreach (var dto in cuestionarioH2VReporte.dataH2VA)
                    {
                        ws.Cells[index, 1].Value = dto.Empresa;
                        ws.Cells[index, 2].Value = dto.NombreProyecto;
                        ws.Cells[index, 3].Value = dto.TipoProyecto;
                        ws.Cells[index, 4].Value = "-"; // Subtipo Proyecto
                        ws.Cells[index, 5].Value = dto.DetalleProyecto;
                        ws.Cells[index, 6].Value = "-";

                        if (dto.ubicacionDTO != null)
                        {
                            ws.Cells[index, 7].Value = dto.ubicacionDTO.Departamento;
                            ws.Cells[index, 8].Value = dto.ubicacionDTO.Provincia;
                            ws.Cells[index, 9].Value = dto.ubicacionDTO.Distrito;
                        }

                        ws.Cells[index, 10].Value = "-";
                        ws.Cells[index, 11].Value = dto.SocioOperador;
                        ws.Cells[index, 12].Value = dto.SocioInversionista;
                        ws.Cells[index, 13].Value = dto.ActDesarrollar;
                        ws.Cells[index, 14].Value = dto.SituacionAct;
                        ws.Cells[index, 15].Value = CombinarTexto(dto.TipoElectrolizador, dto.OtroElectrolizador);

                        ws.Cells[index, 16].Value = dto.VidaUtil;
                        ws.Cells[index, 17].Value = dto.ProduccionAnual;
                        ws.Cells[index, 18].Value = CombinarTexto(dto.ObjetivoProyecto, dto.OtroObjetivo);
                        ws.Cells[index, 19].Value = CombinarTexto(dto.UsoEsperadoHidro, dto.OtroUsoEsperadoHidro);
                        ws.Cells[index, 20].Value = CombinarTexto(dto.MetodoTransH2, dto.OtroMetodoTransH2);

                        ws.Cells[index, 21].Value = dto.PoderCalorifico;
                        ws.Cells[index, 22].Value = dto.SubEstacionSein;
                        ws.Cells[index, 23].Value = dto.NivelTension;
                        ws.Cells[index, 24].Value = CombinarTexto(dto.TipoSuministro,dto.OtroSuministro);
                        ws.Cells[index, 25].Value = dto.CostoProduccion;
                        ws.Cells[index, 26].Value = dto.PrecioVenta;
                        ws.Cells[index, 27].Value = dto.Financiamiento;
                        ws.Cells[index, 28].Value = dto.FactFavorecenProy;
                        ws.Cells[index, 29].Value = dto.FactDesfavorecenProy;

                        // Si ya tienes los siguientes campos (booleans o texto), ponlos aquí
                        //ws.Cells[index, 30].Value = dto.Comentarios; // Estudio de Factibilidad
                                                                     // Columnas 30 a 37 - Permisos/documentos desde ListCH2VADet2DTOs (orden secuencial)
                        var det2Ordenado = (dto.ListCH2VADet2DTOs ?? new List<CuestionarioH2VADet2DTO>())
                                           .OrderBy(x => x.H2vaDet2Codi)
                                           .ToList();

                        for (int i = 0; i < 10; i++) // columnas 30 a 37
                        {
                            if (i >= det2Ordenado.Count)
                            {
                                ws.Cells[index, 30 + i].Value = ""; // por si no hay suficientes registros
                                continue;
                            }

                            var det = det2Ordenado[i];
                            var valores = new List<string>();
                            if (det.EnElaboracion == "true") valores.Add("EN ELABORACIÓN");
                            if (det.Presentado == "true") valores.Add("PRESENTADO");
                            if (det.EnTramite == "true") valores.Add("EN TRAMITE (EVALUACIÓN)");
                            if (det.Aprobado == "true") valores.Add("APROBADO - AUTORIZADO");
                            if (det.Firmado == "true") valores.Add("FIRMADO");

                            ws.Cells[index, 30 + i].Value = string.Join(" / ", valores);
                        }

                        // Ordenar por año (para consistencia en columnas)
                        // Paso 4: Llenar la inversión según el año y columna correspondiente
                        var det1 = dto.ListCH2VADet1DTOs ?? new List<CuestionarioH2VADet1DTO>();
                        foreach (var d in det1)
                        {
                            if (d.Anio.HasValue)
                            {
                                int colOffset = todosLosAnios.IndexOf(d.Anio.Value);
                                if (colOffset >= 0)
                                {
                                    ws.Cells[index, colBase + colOffset].Value = d.MontoInversion.HasValue ? Math.Round(d.MontoInversion.Value, 2) : (object)"";
                                    ws.Cells[index, colBase + colOffset].Style.Numberformat.Format = "#,##0.00";
                                }
                            }
                        }
                        for (int col = 1; col < colBase + todosLosAnios.Count; col++)
                        {
                            ws.Cells[index, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[index, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[index, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[index, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        index++;
                    }
                }


                if (cuestionarioH2VReporte.dataH2VB != null && cuestionarioH2VReporte.dataH2VB.Any())
                {
                    ExcelWorksheet wsB = ObtenerHoja(xlPackage.Workbook, "CC.H2V.-B");

                    int filaInicioB = 3;

                    foreach (var dto in cuestionarioH2VReporte.dataH2VB)
                    {
                        wsB.Cells[filaInicioB, 1].Value = dto.Empresa;
                        wsB.Cells[filaInicioB, 2].Value = dto.NombreProyecto;
                        wsB.Cells[filaInicioB, 3].Value = dto.TipoProyecto;
                        wsB.Cells[filaInicioB, 4].Value = "-"; // Subtipo Proyecto
                        wsB.Cells[filaInicioB, 5].Value = dto.DetalleProyecto;
                        wsB.Cells[filaInicioB, 6].Value = dto.NombreUnidad;

                        if (dto.ubicacionDTO != null)
                        {
                            wsB.Cells[filaInicioB, 7].Value = dto.ubicacionDTO.Departamento;
                            wsB.Cells[filaInicioB, 8].Value = dto.ubicacionDTO.Provincia;
                            wsB.Cells[filaInicioB, 9].Value = dto.ubicacionDTO.Distrito;
                        }

                        wsB.Cells[filaInicioB, 10].Value = dto.Propietario;
                        wsB.Cells[filaInicioB, 11].Value = dto.SocioOperador;
                        wsB.Cells[filaInicioB, 12].Value = dto.SocioInversionista;
                        wsB.Cells[filaInicioB, 13].Value = string.IsNullOrWhiteSpace(dto.ConcesionTemporal) ? "" :
                        dto.ConcesionTemporal.ToUpper() == "S" ? "Sí" : dto.ConcesionTemporal.ToUpper() == "N" ? "No" : "";


                        wsB.Cells[filaInicioB, 14].Value = dto.FechaConcesionTemporal?.ToString("dd/MM/yyyy");

                        wsB.Cells[filaInicioB, 15].Value = dto.TipoElectrolizador;
                        wsB.Cells[filaInicioB, 16].Value = dto.FechaTituloHabilitante?.ToString("dd/MM/yyyy");

                        wsB.Cells[filaInicioB, 17].Value = dto.Perfil;
                        wsB.Cells[filaInicioB, 18].Value = dto.Prefactibilidad;
                        wsB.Cells[filaInicioB, 19].Value = dto.Factibilidad;
                        wsB.Cells[filaInicioB, 20].Value = dto.EstudioDefinitivo;
                        wsB.Cells[filaInicioB, 21].Value = dto.EIA;
                        wsB.Cells[filaInicioB, 22].Value = dto.FechaInicioConstruccion;
                        wsB.Cells[filaInicioB, 23].Value = dto.PeriodoConstruccion;
                        wsB.Cells[filaInicioB, 24].Value = dto.FechaOperacionComercial;
                        wsB.Cells[filaInicioB, 25].Value = dto.PotenciaInstalada;
                        wsB.Cells[filaInicioB, 26].Value = dto.RecursoUsado;
                        wsB.Cells[filaInicioB, 27].Value = dto.Tecnologia;
                        wsB.Cells[filaInicioB, 28].Value = dto.BarraConexion;
                        wsB.Cells[filaInicioB, 29].Value = dto.NivelTension;
                        wsB.Cells[filaInicioB, 30].Value =
                        string.IsNullOrWhiteSpace(dto.Confidencial) ? "" :
                        dto.Confidencial.ToUpper() == "S" ? "Sí" :
                        dto.Confidencial.ToUpper() == "N" ? "No" : "";


                        // Aplicar bordes a la fila completa (1-30)
                        for (int col = 1; col <= 30; col++)
                        {
                            var cell = wsB.Cells[filaInicioB, col];
                            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        filaInicioB++;
                    }
                }

                if (cuestionarioH2VReporte.dataH2VC != null && cuestionarioH2VReporte.dataH2VC.Any())
                {
                    ExcelWorksheet wsC = ObtenerHoja(xlPackage.Workbook, "CC.H2V-C");

                    int row = 5;

                    var datosAgrupados = cuestionarioH2VReporte.dataH2VC
                        .Where(x => x.IndDel != "1")
                        .GroupBy(x => new { x.Empresa, x.ProyCodi, x.NombreProyecto, x.Anio })
                        .OrderBy(g => g.Key.Empresa)
                        .ThenBy(g => g.Key.ProyCodi)
                        .ThenBy(g => g.Key.Anio)
                        .ToList();

                    string[] meses = new string[]
                    {
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
                    };

                    foreach (var grupo in datosAgrupados)
                    {
                        int rowInicioAnio = row;
                        foreach (var mes in meses)
                        {
                            var item = grupo.FirstOrDefault(x => x.Mes == mes);

                            wsC.Cells[row, 2].Value = grupo.Key.Empresa;
                            wsC.Cells[row, 3].Value = grupo.Key.NombreProyecto;

                            // Año: se llena solo una vez, luego se mergea
                            wsC.Cells[row, 4].Value = grupo.Key.Anio;

                            wsC.Cells[row, 5].Value = mes;

                            if (item != null)
                            {
                                wsC.Cells[row, 6].Value = item.DemandaEnergia;
                                wsC.Cells[row, 7].Value = item.DemandaHP;
                                wsC.Cells[row, 8].Value = item.DemandaHFP;
                                wsC.Cells[row, 9].Value = item.GeneracionEnergia;
                                wsC.Cells[row, 10].Value = item.GeneracionHP;
                                wsC.Cells[row, 11].Value = item.GeneracionHFP;
                            }

                            for (int col = 2; col <= 11; col++)
                            {
                                wsC.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                                if (col >= 6)
                                {
                                    wsC.Cells[row, col].Style.Numberformat.Format = "#,##0.0000";
                                }
                            }

                            row++;
                        }

                        // MERGE año desde fila inicio hasta fila final (12 meses)
                        wsC.Cells[rowInicioAnio, 4, row - 1, 4].Merge = true;
                        wsC.Cells[rowInicioAnio, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsC.Cells[rowInicioAnio, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                if (cuestionarioH2VReporte.dataH2VE != null && cuestionarioH2VReporte.dataH2VE.Any())
                {
                    var horasFijas = new List<string>
    {
        "00:30","01:00","01:30","02:00","02:30","03:00","03:30","04:00","04:30","05:00",
        "05:30","06:00","06:30","07:00","07:30","08:00","08:30","09:00","09:30","10:00",
        "10:30","11:00","11:30","12:00","12:30","13:00","13:30","14:00","14:30","15:00",
        "15:30","16:00","16:30","17:00","17:30","18:00","18:30","19:00","19:30","20:00",
        "20:30","21:00","21:30","22:00","22:30","23:00","23:30","00:00"
    };

                    var datosAgrupados = cuestionarioH2VReporte.dataH2VE
                        .Where(x => x.IndDel != "1")
                        .GroupBy(x => x.ProyCodi)
                        .ToList();

                    foreach (var nombreHoja in new[] { "CC.H2V-E-1", "CC.H2V-E-2" })
                    {
                        ExcelWorksheet wsE = ObtenerHoja(xlPackage.Workbook, nombreHoja);
                        int colIndex = 5;
                        int contador = 1;

                        foreach (var grupo in datosAgrupados)
                        {
                            var primerItem = grupo.FirstOrDefault();

                            // Fila 3: Empresa (sin concatenar)
                            wsE.Cells[3, colIndex].Value = primerItem?.Empresa ?? "";
                            wsE.Cells[3, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            wsE.Cells[3, colIndex].Style.Font.Bold = true;

                            // Fila 4: Proyecto
                            wsE.Cells[4, colIndex].Value = primerItem?.NombreProyecto ?? "";
                            wsE.Cells[4, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            // Fila 5: G1, G2, etc.
                            if (nombreHoja == "CC.H2V-E-1")
                            {
                                wsE.Cells[5, colIndex].Value = "D" + contador;
                            }
                            else {
                                wsE.Cells[5, colIndex].Value = "G" + contador;
                            }
                            wsE.Cells[5, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            contador++;

                            // Fila 6+: Horas y valores
                            for (int i = 0; i < horasFijas.Count; i++)
                            {
                                // Columna D: Hora
                                wsE.Cells[i + 6, 4].Value = horasFijas[i];

                                var registro = grupo.FirstOrDefault(x => x.Hora == horasFijas[i]);
                                if (registro != null)
                                {
                                    if (nombreHoja == "CC.H2V-E-1")
                                    {
                                        wsE.Cells[i + 6, colIndex].Value = registro.ConsumoEnergetico;
                                    }
                                    else
                                    {
                                        wsE.Cells[i + 6, colIndex].Value = registro.ProduccionCentral;
                                    }
                                }

                                wsE.Cells[i + 6, colIndex].Style.Numberformat.Format = "#,##0.0";
                            }

                            colIndex += 1;
                        }

                        // Aplicar bordes a toda la tabla
                        int filaInicio = 3;
                        int filaFin = 6 + horasFijas.Count - 1;
                        int colInicio = 4;
                        int colFin = colIndex - 1;

                        using (var range = wsE.Cells[filaInicio, colInicio, filaFin, colFin])
                        {
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }
                if (cuestionarioH2VReporte.dataH2VF != null && cuestionarioH2VReporte.dataH2VF.Any())
                {
                    ExcelWorksheet wsF = ObtenerHoja(xlPackage.Workbook, "CC.H2V.-F");
                    int fila = 3;

                    foreach (var dto in cuestionarioH2VReporte.dataH2VF)
                    {
                        wsF.Cells[fila, 1].Value = dto.Empresa;
                        wsF.Cells[fila, 2].Value = dto.NombreProyecto;
                        wsF.Cells[fila, 3].Value = dto.TipoProyecto;
                        wsF.Cells[fila, 4].Value = "-"; // Subtipo Proyecto si aplica

                        wsF.Cells[fila, 5].Value = dto.EstudioFactibilidad;
                        wsF.Cells[fila, 6].Value = dto.InvestigacionesCampo;
                        wsF.Cells[fila, 7].Value = dto.GestionesFinancieras;
                        wsF.Cells[fila, 8].Value = dto.DisenosPermisos;

                        wsF.Cells[fila, 9].Value = dto.ObrasCiviles;
                        wsF.Cells[fila, 10].Value = dto.Equipamiento;
                        wsF.Cells[fila, 11].Value = dto.LineaTransmision;
                        wsF.Cells[fila, 12].Value = "-";

                        wsF.Cells[fila, 13].Value = dto.Administracion;
                        wsF.Cells[fila, 14].Value = dto.Aduanas;
                        wsF.Cells[fila, 15].Value = dto.Supervision;
                        wsF.Cells[fila, 16].Value = dto.GastosGestion;

                        wsF.Cells[fila, 17].Value = dto.Imprevistos;
                        wsF.Cells[fila, 18].Value = dto.Igv;
                        wsF.Cells[fila, 19].Value = "-";
                        wsF.Cells[fila, 20].Value = dto.OtrosGastos;

                        wsF.Cells[fila, 21].Value = dto.InversionTotalSinIgv;
                        wsF.Cells[fila, 22].Value = dto.InversionTotalConIgv;

                        wsF.Cells[fila, 23].Value = dto.FinanciamientoTipo;
                        wsF.Cells[fila, 24].Value = dto.FinanciamientoEstado;
                        wsF.Cells[fila, 25].Value = dto.PorcentajeFinanciado;

                        wsF.Cells[fila, 26].Value = dto.ConcesionDefinitiva;
                        wsF.Cells[fila, 27].Value = dto.VentaEnergia;
                        wsF.Cells[fila, 28].Value = dto.EjecucionObra;
                        wsF.Cells[fila, 29].Value = dto.ContratosFinancieros;

                        for (int col = 1; col <= 29; col++)
                        {
                            wsF.Cells[fila, col].Style.Numberformat.Format = "#,##0.00";
                            wsF.Cells[fila, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                        fila++;
                    }

                    using (var range = wsF.Cells[3, 1, fila - 1, 29])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }

                xlPackage.Save();
            }

        }
        // Función auxiliar para ordenar los meses
        public static int ObtenerNumeroMes(string mes)
        {
            if (string.IsNullOrEmpty(mes)) return 13;
            string[] meses = new[]
            {
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
    };
            for (int i = 0; i < meses.Length; i++)
            {
                if (mes.Equals(meses[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return i + 1;
                }
            }
            return 13; // Por defecto si no se encuentra
        }





        public static void GenerarReporteCronogramaEjecucion(ReporteModel reporte, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReporteProyectos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReporteProyectos, FormatoArchivosExcelCampanias.NombreReporteProyCronograma));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteProyCronograma));
            // Verificar si la plantilla existe
            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteProyCronograma));
            }

            int index = 5;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                xlPackage.Save();
            }
        }

        public static void GenerarReporteProyeccionDemanda(ProDemandaReporteDTO proDemanda, PeriodoDTO Periodo, List<DataCatalogoDTO> listaparam, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderReportePronosticos);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderReportePronosticos, FormatoArchivosExcelCampanias.NombreReporteDemanda));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteDemanda));

            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteDemanda));
            }

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    if (proDemanda.listaProDemandaA != null && proDemanda.listaProDemandaA.Any())
                    {

                        foreach (var item in proDemanda.listaProDemandaA)
                        {
                            if (item.ListaFormatoDet4A != null)
                            {
                                item.ListaFormatoDet4A = item.ListaFormatoDet4A.Where(detalle => detalle.Anio > 0).ToList();
                            }
                        }
                        ExcelWorksheet wsHEolA = ObtenerHoja(xlPackage.Workbook, "Datos Generales");

                        var todosLosAnios = proDemanda.listaProDemandaA
                            .SelectMany(item => item.ListaFormatoDet4A ?? new List<FormatoD1ADet4DTO>())
                            .Select(detalle => detalle.Anio)
                            .Distinct()
                            .OrderBy(anio => anio>0)
                            .ToList();

                        int columnaInicio = 41;
                        while (wsHEolA.Cells[3, columnaInicio].Text.Contains("INVERSION ESTIMADA"))
                        {
                            wsHEolA.DeleteColumn(columnaInicio);
                        }
                        
                        for (int i = 0; i < todosLosAnios.Count; i++)
                        {
                            wsHEolA.InsertColumn(columnaInicio + i, 1);
                            wsHEolA.Cells[3, columnaInicio + i].Value = todosLosAnios[i];
                            wsHEolA.Cells[3, columnaInicio + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            wsHEolA.Cells[3, columnaInicio + i].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#83E28E"));
                            wsHEolA.Cells[3, columnaInicio + i].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        }

                        int columnaBase = columnaInicio + todosLosAnios.Count;

                        ProcesarLista(wsHEolA, proDemanda.listaProDemandaA, (ws, item, index) =>
                        {
                            int columnaFinal = columnaBase;

                            ws.Cells[index, 1].Value = item.Empresa;
                            ws.Cells[index, 2].Value = item.Nombre;
                            ws.Cells[index, 3].Value = item.TipoCarga;

                            if (item.ubicacionDTO != null)
                            {
                                ws.Cells[index, 4].Value = item.ubicacionDTO.Departamento;
                                ws.Cells[index, 5].Value = item.ubicacionDTO.Provincia;
                                ws.Cells[index, 6].Value = item.ubicacionDTO.Distrito;
                            }

                            ws.Cells[index, 7].Value = item.ActDesarrollo;
                            ws.Cells[index, 8].Value = item.SituacionAct;
                            ws.Cells[index, 9].Value = item.Exploracion;
                            ws.Cells[index, 10].Value = item.EstudioPreFactibilidad;
                            ws.Cells[index, 11].Value = item.EstudioFactibilidad;
                            ws.Cells[index, 12].Value = item.EstudioImpAmb;
                            ws.Cells[index, 13].Value = item.Financiamiento1;
                            ws.Cells[index, 14].Value = item.Ingenieria;
                            ws.Cells[index, 15].Value = item.Construccion;
                            ws.Cells[index, 16].Value = item.PuestaMarchar;
                            ws.Cells[index, 17].Value = item.TipoExtraccionMin;
                            ws.Cells[index, 18].Value = item.MetalesExtraer;
                            ws.Cells[index, 19].Value = item.TipoYacimiento;
                            ws.Cells[index, 20].Value = item.VidaUtil;
                            ws.Cells[index, 21].Value = item.Reservas;
                            ws.Cells[index, 22].Value = item.EscalaProduccion;
                            ws.Cells[index, 23].Value = item.PlantaBeneficio;
                            ws.Cells[index, 24].Value = item.RecuperacionMet;
                            ws.Cells[index, 25].Value = item.LeyesConcentrado;
                            ws.Cells[index, 26].Value = item.CapacidadTrata;
                            ws.Cells[index, 27].Value = item.ProduccionAnual;
                            ws.Cells[index, 28].Value = item.ToneladaMetrica;
                            ws.Cells[index, 29].Value = item.Energia;
                            ws.Cells[index, 30].Value = item.Consumo;
                            ws.Cells[index, 31].Value = item.SubestacionCodi;
                            ws.Cells[index, 32].Value = item.NivelTension;
                            ws.Cells[index, 33].Value = item.EmpresaSuminicodi;
                            ws.Cells[index, 34].Value = item.FactorPotencia;
                            ws.Cells[index, 35].Value = item.Inductivo;
                            ws.Cells[index, 36].Value = item.Capacitivo;
                            ws.Cells[index, 37].Value = item.CostoProduccion;
                            ws.Cells[index, 38].Value = item.Metales;
                            ws.Cells[index, 39].Value = item.Precio;
                            ws.Cells[index, 40].Value = "Unidad";

                            if (item.ListaFormatoDet4A != null && item.ListaFormatoDet4A.Any())
                            {
                                var detallesDet4 = item.ListaFormatoDet4A.OrderBy(d => d.Anio).ToList();
                                foreach (var detalle in detallesDet4)
                                {
                                    int colIndex = columnaInicio + todosLosAnios.IndexOf(detalle.Anio);
                                    if (detalle.MontoInversion.HasValue)
                                    {
                                        ws.Cells[index, colIndex].Value = Math.Round(detalle.MontoInversion.Value, 2);
                                        ws.Cells[index, colIndex].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else
                                    {
                                        ws.Cells[index, colIndex].Value = ""; // o null
                                    }

                                }
                            }

                            ws.Cells[index, columnaFinal].Value = item.Financiamiento2;
                            ws.Cells[index, columnaFinal + 1].Value = item.FacFavEjecuProy;
                            ws.Cells[index, columnaFinal + 2].Value = item.FactDesEjecuProy;

                            if (item.ListaFormatoDet5A != null && item.ListaFormatoDet5A.Any())
                            {
                                var detallesOrdenados = item.ListaFormatoDet5A.OrderBy(d => d.FormatoD1ADet5Codi).ToList();
                                for (int i = 0; i < detallesOrdenados.Count; i++)
                                {
                                    var detalle = detallesOrdenados[i];
                                    var valores = new List<string>();
                                    if (detalle.EnElaboracion == "true") valores.Add("EN ELABORACIÓN");
                                    if (detalle.Presentado == "true") valores.Add("PRESENTADO");
                                    if (detalle.EnTramite == "true") valores.Add("EN TRAMITE (EVALUACIÓN)");
                                    if (detalle.Aprobado == "true") valores.Add("APROBADO - AUTORIZADO");
                                    if (detalle.Firmado == "true") valores.Add("FIRMADO");

                                    ws.Cells[index, columnaFinal + 3 + i].Value = string.Join(" / ", valores);
                                }
                            }
                             
                            int countDet5A = item.ListaFormatoDet5A != null ? item.ListaFormatoDet5A.Count : 0;
                            ws.Cells[index, columnaFinal + countDet5A + 3].Value = item.Comentarios;

                            ws.Cells[index, columnaFinal + countDet5A + 4].Value =item.Condifencial == "S" ? "Si" : item.Condifencial == "N" ? "No" : "";
                        }, 4);
                        //// Fusionar celdas y agregar texto "INVERSIÓN ESTIMADA"
                        int columnaFin = 41 + todosLosAnios.Count - 1;
                        wsHEolA.Cells[2, columnaInicio, 2, columnaFin].Merge = true;
                        wsHEolA.Cells[2, columnaInicio].Value = "INVERSIÓN ESTIMADA";
                        wsHEolA.Cells[2, columnaInicio, 2, columnaFin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHEolA.Cells[2, columnaInicio, 2, columnaFin].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsHEolA.Cells[2, columnaInicio, 3, columnaFin].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsHEolA.Cells[2, columnaInicio, 3, columnaFin].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#83E28E"));
                        wsHEolA.Cells[2, columnaInicio, 2, columnaFin].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                        AplicarEstilosHoja(wsHEolA, 3);
                    }

                    if (proDemanda.listaProDemandaA != null && proDemanda.listaProDemandaA.Any())
                    {
                        List<FormatoD1ADTO> listaProDemandaA = proDemanda.listaProDemandaA;

                        ExcelWorksheet wsDemanEA = ObtenerHoja(xlPackage.Workbook, "DemandaAnual");
 

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteVertical(wsDemanEA, listaProDemandaA);


                    }

                    if (proDemanda.listaProDemandaB != null && proDemanda.listaProDemandaB.Any())
                    {
                        List<FormatoD1BDTO> listaProDemandaB = proDemanda.listaProDemandaB;

                        var wsDemanEMes = ObtenerHoja(xlPackage.Workbook, "DemandaMensual");


                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;
                        int horizonteIni = anioPeriodo - Periodo.PeriHorizonteAtras;
                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporteMensualVertical(wsDemanEMes, proDemanda.listaProDemandaB);


                    }

                    if (proDemanda.listaProDemandaC != null && proDemanda.listaProDemandaC.Any())
                    {
                        List<FormatoD1CDTO> listaProDemandaC = proDemanda.listaProDemandaC;

                        ExcelWorksheet wsDC = ObtenerHoja(xlPackage.Workbook, "Demanda (DiagramaDeCarga)");
                        ExcelWorksheet wsGC = ObtenerHoja(xlPackage.Workbook, "Generación (DiagramaDeCarga)");
 

                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporte3(wsDC, listaProDemandaC, horizonteFin, cantidadDeAnios, "DEMANDA PU");
                        GenerarHojaReporte3(wsGC, listaProDemandaC, horizonteFin, cantidadDeAnios, "GENERACION PU");



                    }
                    if (proDemanda.listaProDemandaD != null && proDemanda.listaProDemandaD.Any())
                    {
                       

                        List<FormatoD1DDTO> listaProDemandaD = proDemanda.listaProDemandaD;

                        ExcelWorksheet croEg = ObtenerHoja(xlPackage.Workbook, "CronogramDeEjecución");
                         
                        int anioPeriodo = Periodo.PeriFechaInicio.Year;
                        int horizonteFin = anioPeriodo + Periodo.PeriHorizonteAdelante;

                        int cantidadDeAnios = (horizonteFin - 2011) + 1;

                        GenerarHojaReporte4(croEg, listaProDemandaD, anioPeriodo, horizonteFin, listaparam);
                        //GenerarHojaReporte3(wsGC, listaProDemandaC, horizonteFin, cantidadDeAnios, "GENERACION PU");



                    }


                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al generar el reporte Demanda - Pronóstico: " + ex.Message, ex);
            }
        }

        public static void GenerarHojaReporte(ExcelWorksheet wsExcel, List<FormatoD1ADTO> listaProDemandaA, int horizonteFin, int cantidadDeAnios, string tipo)
        {

            if (wsExcel.Cells["A4:A75"].Merge)
            {
                wsExcel.Cells["A4:A75"].Merge = false;
            }

            int indexDet1 = 4;
            int cantidadEmpresas = listaProDemandaA.Count;
            wsExcel.Cells[1, 3, 1, cantidadEmpresas + 2].Merge = true;

            var titulosPorTipo = new Dictionary<string, string>
            {
                { "DemandaEnergia", "ENERGÍA (GWh)" },
                { "DemandaHP", "POTENCIA HP (MW)" },
                { "DemandaFHP", "POTENCIA FHP (MW)" },
                { "GeneracionEnergia", "ENERGÍA (GWh)" },
                { "GeneracionHP", "POTENCIA HP (MW)" },
                { "GeneracionFHP", "POTENCIA FHP (MW)" }
            };

            if (titulosPorTipo.TryGetValue(tipo, out string titulo))
            {
                wsExcel.Cells[1, 3].Value = titulo;
            }


            wsExcel.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsExcel.Cells[1, 3].Style.Font.Bold = true;


            wsExcel.Cells[indexDet1, 1, (indexDet1 + cantidadDeAnios) - 1, 1].Merge = true;
            wsExcel.Cells[indexDet1, 1].Value = "ESCENARIO MEDIO O BASE";
            wsExcel.Cells[indexDet1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            wsExcel.Cells[indexDet1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            for (int anio = 2011; anio <= horizonteFin; anio++)
            {
                wsExcel.Cells[indexDet1, 2].Value = anio;
                wsExcel.Cells[indexDet1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                indexDet1++;
            }

            int indexDet2 = indexDet1;

            wsExcel.Cells[indexDet2, 1, (indexDet1 + cantidadDeAnios) - 1, 1].Merge = true;
            wsExcel.Cells[indexDet2, 1].Value = "ESCENARIO OPTIMISTA";
            wsExcel.Cells[indexDet2, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            wsExcel.Cells[indexDet2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            for (int anio = 2011; anio <= horizonteFin; anio++)
            {
                wsExcel.Cells[indexDet2, 2].Value = anio;
                wsExcel.Cells[indexDet2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                indexDet2++;
            }

            int indexDet3 = indexDet2;

            wsExcel.Cells[indexDet3, 1, (indexDet2 + cantidadDeAnios) - 1, 1].Merge = true;
            wsExcel.Cells[indexDet3, 1].Value = "ESCENARIO PESIMISTA";
            wsExcel.Cells[indexDet3, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            wsExcel.Cells[indexDet3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            for (int anio = 2011; anio <= horizonteFin; anio++)
            {
                wsExcel.Cells[indexDet3, 2].Value = anio;
                wsExcel.Cells[indexDet3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                indexDet3++;
            }

            ProcesarListaColum(wsExcel, listaProDemandaA, (ws, item, index) =>
            {
                ws.Cells[2, index].Value = item.Empresa;
                ws.Cells[3, index].Value = item.Nombre;

                if (item.ListaFormatoDet1A != null && item.ListaFormatoDet1A.Any())
                {
                    var detallesPorAnio = item.ListaFormatoDet1A
                        .GroupBy(d => d.Anio)
                        .ToDictionary(g => g.Key, g => g.First());

                    for (int i = 4; i < indexDet1; i++)
                    {
                        var valorAnioStr = ws.Cells[i, 2].Value.ToString();

                        if (int.TryParse(valorAnioStr, out int valorAnio))
                        {
                            if (detallesPorAnio.TryGetValue(valorAnio, out var detalle))
                            {
                                decimal? valor = null;

                                if (tipo == "DemandaEnergia")
                                    valor = detalle.DemandaEnergia;
                                else if (tipo == "DemandaHP")
                                    valor = detalle.DemandaHP;
                                else if (tipo == "DemandaFHP")
                                    valor = detalle.DemandaHFP;
                                else if (tipo == "GeneracionEnergia")
                                    valor = detalle.GeneracionEnergia;
                                else if (tipo == "GeneracionHP")
                                    valor = detalle.GeneracionHP;
                                else if (tipo == "GeneracionFHP")
                                    valor = detalle.GeneracionHFP;

                                ws.Cells[i, index].Value = valor.HasValue
                                    ? Math.Round(valor.Value, 4).ToString("F4")
                                    : "0.0000";

                                ws.Cells[i, index].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }
                    }
                }


                if (item.ListaFormatoDet2A != null && item.ListaFormatoDet2A.Any())
                {
                    var detallesPorAnio = item.ListaFormatoDet2A
                        .GroupBy(d => d.Anio)
                        .ToDictionary(g => g.Key, g => g.First());

                    for (int i = indexDet1; i < indexDet2; i++)
                    {
                        var valorAnioStr = ws.Cells[i, 2].Value.ToString();

                        if (int.TryParse(valorAnioStr, out int valorAnio))
                        {
                            if (detallesPorAnio.TryGetValue(valorAnio, out var detalle))
                            {
                                decimal? valor = null;

                                if (tipo == "DemandaEnergia")
                                    valor = detalle.DemandaEnergia;
                                else if (tipo == "DemandaHP")
                                    valor = detalle.DemandaHP;
                                else if (tipo == "DemandaFHP")
                                    valor = detalle.DemandaHFP;
                                else if (tipo == "GeneracionEnergia")
                                    valor = detalle.GeneracionEnergia;
                                else if (tipo == "GeneracionHP")
                                    valor = detalle.GeneracionHP;
                                else if (tipo == "GeneracionFHP")
                                    valor = detalle.GeneracionHFP;

                                ws.Cells[i, index].Value = valor.HasValue
                                    ? Math.Round(valor.Value, 4).ToString("F4")
                                    : "0.0000";

                                ws.Cells[i, index].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }
                    }
                }

                if (item.ListaFormatoDet3A != null && item.ListaFormatoDet3A.Any())
                {
                    var detallesPorAnio = item.ListaFormatoDet3A
                        .GroupBy(d => d.Anio)
                        .ToDictionary(g => g.Key, g => g.First());

                    for (int i = indexDet2; i < indexDet3; i++)
                    {
                        var valorAnioStr = ws.Cells[i, 2].Value.ToString();

                        if (int.TryParse(valorAnioStr, out int valorAnio))
                        {
                            if (detallesPorAnio.TryGetValue(valorAnio, out var detalle))
                            {
                                decimal? valor = null;

                                if (tipo == "DemandaEnergia")
                                    valor = detalle.DemandaEnergia;
                                else if (tipo == "DemandaHP")
                                    valor = detalle.DemandaHP;
                                else if (tipo == "DemandaFHP")
                                    valor = detalle.DemandaHFP;
                                else if (tipo == "GeneracionEnergia")
                                    valor = detalle.GeneracionEnergia;
                                else if (tipo == "GeneracionHP")
                                    valor = detalle.GeneracionHP;
                                else if (tipo == "GeneracionFHP")
                                    valor = detalle.GeneracionHFP;

                                ws.Cells[i, index].Value = valor.HasValue
                                    ? Math.Round(valor.Value, 4).ToString("F4")
                                    : "0.0000";

                                ws.Cells[i, index].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }
                    }
                }

            }, 3);

            DeleteCeldasRowRestantesExcel(indexDet3, wsExcel);
            // Fondo gris para la primera fila (título) y las dos primeras columnas
         
            int colInicio = 3; // columna C
            int colFin = colInicio + cantidadEmpresas - 1;
            for (int col = colInicio; col <= colFin; col++)
            {
                // Definir un ancho mínimo
                if (wsExcel.Column(col).Width < 20)
                    wsExcel.Column(col).Width = 20;

                // Estilos de alineación y ajuste de texto
                wsExcel.Cells[2, col].Style.WrapText = true;
                wsExcel.Cells[2, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[2, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                wsExcel.Cells[3, col].Style.WrapText = true;
                wsExcel.Cells[3, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[3, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }


            using (var rangeTitulo = wsExcel.Cells[1, 1, 1, wsExcel.Dimension.End.Column])
            {
                rangeTitulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangeTitulo.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            using (var col1 = wsExcel.Cells[1, 1, wsExcel.Dimension.End.Row, 1])
            {
                col1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                col1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            using (var col2 = wsExcel.Cells[1, 2, wsExcel.Dimension.End.Row, 2])
            {
                col2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                col2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Bordes a toda la tabla
            using (var fullRange = wsExcel.Cells[1, 1, wsExcel.Dimension.End.Row, wsExcel.Dimension.End.Column])
            {
                fullRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                fullRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                fullRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                fullRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }


        }
        public static void GenerarHojaReporteVertical(ExcelWorksheet wsExcel, List<FormatoD1ADTO> listaProDemandaA)
        {
            var headers = new[] { "Empresa propietaria", "Nombre de la Carga", "Tipo", "Parametro", "Escenario", "Año", "Magnitud" };

            for (int i = 0; i < headers.Length; i++)
            {
                wsExcel.Cells[1, i + 1].Value = headers[i];
                wsExcel.Cells[1, i + 1].Style.Font.Bold = true;
                wsExcel.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsExcel.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                wsExcel.Column(i + 1).AutoFit();
            }

            int currentRow = 2;

            foreach (var item in listaProDemandaA.OrderBy(p => p.ProyCodi))
            {
                string empresa = item.Empresa;
                string nombreCarga = item.Nombre ?? "(Sin nombre)"; // ← este es el Nombre de la Carga

                var escenarios = new List<(string nombre, IEnumerable<object> lista)>
        {
            ("base", item.ListaFormatoDet1A ?? new List<FormatoD1ADet1DTO>()),
            ("optimista", item.ListaFormatoDet2A ?? new List<FormatoD1ADet2DTO>()),
            ("pesimista", item.ListaFormatoDet3A ?? new List<FormatoD1ADet3DTO>())
        };

                var campos = new List<(string tipo, string parametro, string nombreCampo)>
        {
            ("demanda", "energia", "DemandaEnergia"),
            ("demanda", "potencia hp", "DemandaHP"),
            ("demanda", "potencia fhp", "DemandaHFP"),
            ("generacion", "energia", "GeneracionEnergia"),
            ("generacion", "potencia hp", "GeneracionHP"),
            ("generacion", "potencia fhp", "GeneracionFHP"),
            ("demanda neta", "potencia hp", "DemandaNetaHP"),
            ("demanda neta", "potencia fhp", "DemandaNetaHFP")
        };

                var ordenEscenario = new Dictionary<string, int> {
            { "base", 1 }, { "optimista", 2 }, { "pesimista", 3 }
        };

                foreach (var (escenario, lista) in escenarios.OrderBy(e => ordenEscenario[e.nombre]))
                {
                    var listaOrdenada = lista
                        .Where(d =>
                            d.GetType().GetProperty("Anio")?.GetValue(d) != null
                            && int.TryParse(d.GetType().GetProperty("Anio")?.GetValue(d).ToString(), out _)
                        )
                        .OrderBy(d => Convert.ToInt32(d.GetType().GetProperty("Anio")?.GetValue(d)))
                        .ToList();

                    foreach (var detalle in listaOrdenada)
                    {
                        var tipoDet = detalle.GetType();
                        var anio = (int?)tipoDet.GetProperty("Anio")?.GetValue(detalle);

                        if (!anio.HasValue) continue;

                        foreach (var (tipo, parametro, campo) in campos)
                        {
                            var valor = tipoDet.GetProperty(campo)?.GetValue(detalle) as decimal?;

                            if (valor.HasValue)
                            {
                                wsExcel.Cells[currentRow, 1].Value = empresa;
                                wsExcel.Cells[currentRow, 2].Value = nombreCarga;
                                wsExcel.Cells[currentRow, 3].Value = Capitalize(tipo);
                                wsExcel.Cells[currentRow, 4].Value = Capitalize(parametro);
                                wsExcel.Cells[currentRow, 5].Value = Capitalize(escenario);
                                wsExcel.Cells[currentRow, 6].Value = anio.Value;
                                wsExcel.Cells[currentRow, 7].Value = ConvertirAFormatoSeguro(valor);

                                currentRow++;
                            }
                        }

                        var demEnergia = tipoDet.GetProperty("DemandaEnergia")?.GetValue(detalle) as decimal?;
                        var genEnergia = tipoDet.GetProperty("GeneracionEnergia")?.GetValue(detalle) as decimal?;

                        if (demEnergia.HasValue && genEnergia.HasValue)
                        {
                            decimal valorNeto = demEnergia.Value - genEnergia.Value;

                            wsExcel.Cells[currentRow, 1].Value = empresa;
                            wsExcel.Cells[currentRow, 2].Value = nombreCarga;
                            wsExcel.Cells[currentRow, 3].Value = "demanda neta";
                            wsExcel.Cells[currentRow, 4].Value = "energia";
                            wsExcel.Cells[currentRow, 5].Value = escenario;
                            wsExcel.Cells[currentRow, 6].Value = anio.Value;
                            wsExcel.Cells[currentRow, 7].Value = ConvertirAFormatoSeguro(valorNeto);

                            currentRow++;
                        }
                    }
                }
            }

            using (var range = wsExcel.Cells[1, 1, currentRow - 1, 7])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }


        public static void GenerarHojaReporteMensualVertical(ExcelWorksheet wsExcel, List<FormatoD1BDTO> listaProDemandaB)
        {
            var headers = new[] { "Empresa propietaria", "Nombre de la Carga", "Tipo", "Parametro", "Escenario", "Año", "Mes", "Magnitud" };

            for (int i = 0; i < headers.Length; i++)
            {
                wsExcel.Cells[1, i + 1].Value = headers[i];
                wsExcel.Cells[1, i + 1].Style.Font.Bold = true;
                wsExcel.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsExcel.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                wsExcel.Column(i + 1).AutoFit();
            }

            int currentRow = 2;

            foreach (var item in listaProDemandaB.OrderBy(x => x.ProyCodi))
            {
                string empresa = item.Empresa;
                string nombreCarga = item.NombreCarga ?? "(Sin nombre)";

                if (item.ListaFormatoDet1B == null || !item.ListaFormatoDet1B.Any())
                    continue;

                var combinaciones = new List<(string tipo, string parametro, string campo)>
        {
            ("demanda", "energia", "DemandaEnergia"),
            ("demanda", "potencia hp", "DemandaHP"),
            ("demanda", "potencia fhp", "DemandaHFP"),
            ("generacion", "energia", "GeneracionEnergia"),
            ("generacion", "potencia hp", "GeneracionHP"),
            ("generacion", "potencia fhp", "GeneracionHFP"),
            ("demanda neta", "potencia hp", "DemandaNetaHP"),
            ("demanda neta", "potencia fhp", "DemandaNetaHFP")
        };

                var detallesOrdenados = item.ListaFormatoDet1B
                    .Where(d => !string.IsNullOrEmpty(d.Anio) && int.TryParse(d.Anio, out _))
                    .OrderBy(d => Convert.ToInt32(d.Anio))
                    .ThenBy(d => ObtenerIndiceMes(d.Mes))
                    .ToList();

                foreach (var detalle in detallesOrdenados)
                {
                    string anio = detalle.Anio;
                    string mesNumero = ObtenerNumeroMesTexto(detalle.Mes);

                    if (string.IsNullOrEmpty(mesNumero))
                        continue;

                    foreach (var (tipo, parametro, campo) in combinaciones)
                    {
                        var tipoDet = detalle.GetType();
                        var valor = tipoDet.GetProperty(campo)?.GetValue(detalle) as decimal?;

                        if (valor.HasValue)
                        {
                            wsExcel.Cells[currentRow, 1].Value = empresa;
                            wsExcel.Cells[currentRow, 2].Value = nombreCarga;
                            wsExcel.Cells[currentRow, 3].Value = Capitalize(tipo);
                            wsExcel.Cells[currentRow, 4].Value = Capitalize(parametro);
                            wsExcel.Cells[currentRow, 5].Value = "Base";
                            wsExcel.Cells[currentRow, 6].Value = anio;
                            wsExcel.Cells[currentRow, 7].Value = mesNumero;
                            wsExcel.Cells[currentRow, 8].Value = ConvertirAFormatoSeguro(valor);

                            currentRow++;
                        }
                    }

                    var demEnergia = detalle.GetType().GetProperty("DemandaEnergia")?.GetValue(detalle) as decimal?;
                    var genEnergia = detalle.GetType().GetProperty("GeneracionEnergia")?.GetValue(detalle) as decimal?;

                    if (demEnergia.HasValue && genEnergia.HasValue)
                    {
                        decimal valorNeto = demEnergia.Value - genEnergia.Value;

                        wsExcel.Cells[currentRow, 1].Value = empresa;
                        wsExcel.Cells[currentRow, 2].Value = nombreCarga;
                        wsExcel.Cells[currentRow, 3].Value = Capitalize("demanda neta");
                        wsExcel.Cells[currentRow, 4].Value = Capitalize("energia");
                        wsExcel.Cells[currentRow, 5].Value = "Base";
                        wsExcel.Cells[currentRow, 6].Value = anio;
                        wsExcel.Cells[currentRow, 7].Value = mesNumero;
                        wsExcel.Cells[currentRow, 8].Value = ConvertirAFormatoSeguro(valorNeto);

                        currentRow++;
                    }
                }
            }

            using (var range = wsExcel.Cells[1, 1, currentRow - 1, 8])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }
        private static int ObtenerIndiceMes(string mes)
        {
            if (string.IsNullOrWhiteSpace(mes)) return 0;

            mes = mes.Trim().ToLower();
            if (mes == "septiembre") mes = "setiembre";

            var meses = new[]
            {
        "enero", "febrero", "marzo", "abril", "mayo", "junio",
        "julio", "agosto", "setiembre", "octubre", "noviembre", "diciembre"
    };

            int indice = Array.IndexOf(meses, mes);
            return indice >= 0 ? (indice + 1) : 0;
        }

        private static string ObtenerNumeroMesTexto(string mes)
        {
            int numero = ObtenerIndiceMes(mes);
            return numero > 0 ? numero.ToString("D2") : "";
        }


        public static void GenerarHojaReporte2(ExcelWorksheet wsExcel, List<FormatoD1BDTO> listaProDemandaB,int horizonteIni,int horizonteFin, string tipo)
        {
            int indexFila = 2;

            wsExcel.Cells[1, 1].Value = "DEMANDA";
            int indexCol = 3;
            if (listaProDemandaB.Last() == listaProDemandaB[listaProDemandaB.Count - 1])
            {
                wsExcel.Cells[1, 1, 1, wsExcel.Dimension.End.Column].Merge = false;
                wsExcel.Cells[1, indexCol, 1, indexCol+listaProDemandaB.Count - 1].Merge = true;
                wsExcel.Cells[1, 1, 1, 2].Merge = true;
                using (var range = wsExcel.Cells[1, 1, 1, indexCol + listaProDemandaB.Count - 1])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    range.Style.Font.Bold = true;
                }
            }
            wsExcel.Cells[1, 3].Value = tipo;
            wsExcel.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            wsExcel.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            wsExcel.Cells[1, 2].Style.Font.Bold = true;

            wsExcel.Cells[2, 1].Value = "Empresa propietaria";
            wsExcel.Cells[3, 1].Value = "Nombre de la Carga";
            wsExcel.Cells[4, 1].Value = "Fecha de Ingreso";
            wsExcel.Cells[5, 1].Value = "Barra de Conexión";
            wsExcel.Cells[6, 1].Value = "Nivel de Tensión";

            // Aplicar color #D9D9D9 a las dos primeras columnas de la primera tabla
            using (var range = wsExcel.Cells[1, 1, 6, 2])
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
            }

           // int indexCol = 3;
            foreach (var proyecto in listaProDemandaB)
            {

                wsExcel.Cells[2, indexCol].Value = proyecto.Empresa;
                wsExcel.Column(indexCol).AutoFit();
                wsExcel.Cells[3, indexCol].Value = proyecto.NombreCarga;
                wsExcel.Cells[4, indexCol].Value = proyecto.FechaIngreso;
                wsExcel.Cells[5, indexCol].Value = proyecto.BarraConexion;
                wsExcel.Cells[6, indexCol].Value = proyecto.NivelTension;
                indexCol++;
            }
            //wsExcel.Cells[1,3,1,7].Merge = true;
            var todosLosAnios = Enumerable.Range(horizonteIni, horizonteFin - horizonteIni + 1).ToList();


            if (!todosLosAnios.Any()) return;

            var meses = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre" };

            indexFila = 7;
            foreach (var anio in todosLosAnios)
            {
                int inicioFilaAnio = indexFila;
                foreach (var mes in meses)
                {
                    wsExcel.Cells[indexFila, 1].Value = anio;
                    wsExcel.Cells[indexFila, 2].Value = mes;

                    // Aplicar color #D9D9D9 a la primera y segunda columna de la tabla de datos
                    using (var range = wsExcel.Cells[indexFila, 1, indexFila, 2])
                    {
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                    }

                    int indexColProyecto = 3;
                    foreach (var proyecto in listaProDemandaB)
                    {
                        var detalleMes = proyecto.ListaFormatoDet1B
                                                 .FirstOrDefault(d => d.Anio == anio.ToString() && d.Mes == mes);

                        if (detalleMes != null)
                        {
                            switch (tipo)
                            {
                                case "ENERGÍA (GWh)":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.DemandaEnergia ?? 0).ToString("F2");
                                    break;
                                case "POTENCIA HP (MW)":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.DemandaHP ?? 0).ToString("F2");
                                    break;
                                case "POTENCIA FHP (MW)":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.DemandaNetaHFP ?? 0).ToString("F2");
                                    break;
                                case "GeneracionEnergia":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.GeneracionEnergia ?? 0).ToString("F2");
                                    break;
                                case "GeneracionHP":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.GeneracionHP ?? 0).ToString("F2");
                                    break;
                                case "GeneracionFHP":
                                    wsExcel.Cells[indexFila, indexColProyecto].Value = (detalleMes.GeneracionHFP ?? 0).ToString("F2");
                                    break;
                            }

                            // Aplicar formato de texto a la celda
                            wsExcel.Cells[indexFila, indexColProyecto].Style.Numberformat.Format = "@";

                        }
                        indexColProyecto++;
                    }
                    indexFila++;
                }
                wsExcel.Cells[inicioFilaAnio, 1, indexFila - 1, 1].Merge = true;
                wsExcel.Cells[inicioFilaAnio, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                wsExcel.Cells[inicioFilaAnio, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Agregar bordes a la primera tabla
            using (var range = wsExcel.Cells[1, 1, indexFila - 1, indexCol - 1])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            // Agregar espacio de separación
            indexFila += 2;

            // Agregar tabla de totales por año
            // Guardar la posición de la fila inicial para el merge
            int filaTituloTotales = indexFila;
            // Merge y texto centrado en la celda
            var celdaTitulo = wsExcel.Cells[indexFila, 1, indexFila, listaProDemandaB.Count + 2];
            celdaTitulo.Merge = true;
            celdaTitulo.Value = "ENERGÍA TOTAL (GWh)";

            // Aplicar bordes
            celdaTitulo.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            celdaTitulo.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            celdaTitulo.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            celdaTitulo.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Aplicar fondo gris
            celdaTitulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
            celdaTitulo.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

            // Alinear y poner en negrita
            celdaTitulo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            celdaTitulo.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celdaTitulo.Style.Font.Bold = true;

            indexFila++;

            foreach (var anio in todosLosAnios)
            {
                wsExcel.Cells[indexFila, 1].Value = anio;
                int indexColProyecto = 3;
                //wsExcel.Cells[indexFila, 1, indexFila, 2].Merge = true;
                foreach (var proyecto in listaProDemandaB)
                {
                    var totalEnergia = proyecto.ListaFormatoDet1B
                                               .Where(d => d.Anio == anio.ToString())
                                               .Sum(d => d.DemandaEnergia ?? 0);
                    wsExcel.Cells[indexFila, indexColProyecto].Value = totalEnergia;
                    indexColProyecto++;
                }

                // Aplicar color #D9D9D9 a la primera columna de la tabla de totales
                wsExcel.Cells[indexFila, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                wsExcel.Cells[indexFila, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                wsExcel.Cells[indexFila, 1].Style.TextRotation = 0;
                indexFila++;
            }

            // Agregar bordes a la tabla de totales
            using (var range = wsExcel.Cells[indexFila - todosLosAnios.Count, 1, indexFila - 1, indexCol - 1])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
        }

        public static void GenerarHojaReporte3(ExcelWorksheet wsExcel, List<FormatoD1CDTO> listaProDemandaC, int horizonteFin, int cantidadDeAnios, string tipo)
        {
            int filaInicio = 1;

            List<string> horasOrdenadas = new List<string>
    {
        "00:30","01:00","01:30","02:00","02:30","03:00","03:30","04:00","04:30",
        "05:00","05:30","06:00","06:30","07:00","07:30","08:00","08:30","09:00",
        "09:30","10:00","10:30","11:00","11:30","12:00","12:30","13:00","13:30",
        "14:00","14:30","15:00","15:30","16:00","16:30","17:00","17:30","18:00",
        "18:30","19:00","19:30","20:00","20:30","21:00","21:30","22:00","22:30",
        "23:00","23:30","00:00"
    };

            int totalEmpresas = listaProDemandaC.Count;
            int colInicioEmpresas = 2;

            // A1 = DEMANDA o GENERACIÓN
            wsExcel.Cells[1, 1].Value = tipo == "DEMANDA PU" ? "DEMANDA" : "GENERACIÓN";

            // B1 en adelante = DIAGRAMA DE CARGA (p.u.) (fusionado)
            if (totalEmpresas > 0)
            {
                wsExcel.Cells[1, colInicioEmpresas, 1, colInicioEmpresas + totalEmpresas - 1].Merge = true;
                wsExcel.Cells[1, colInicioEmpresas].Value = "DIAGRAMA DE CARGA (p.u.)";
                wsExcel.Cells[1, colInicioEmpresas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[1, colInicioEmpresas].Style.Font.Bold = true;
            }

            // A2 = Empresa propietaria
            wsExcel.Cells[2, 1].Value = "Empresa propietaria";

            // A3 = Nombre de la Carga
            wsExcel.Cells[3, 1].Value = "Nombre de la Carga";

            // A partir de A4 = horas
            int filaHora = 4;
            foreach (var hora in horasOrdenadas)
            {
                wsExcel.Cells[filaHora, 1].Value = hora;
                filaHora++;
            }

            // Escribir los datos por empresa en columnas
            int col = colInicioEmpresas;

            foreach (var proyecto in listaProDemandaC)
            {
                wsExcel.Cells[2, col].Value = proyecto.Empresa;           // Fila 2: Empresa
                wsExcel.Cells[3, col].Value = proyecto.ProyCodi;          // Fila 3: Proyecto

                var datosPorHora = proyecto.ListaFormatoDe1CDet
                    .Where(d => !string.IsNullOrEmpty(d.Hora))
                    .GroupBy(d => d.Hora)
                    .ToDictionary(g => g.Key, g => g.First());

                int fila = 4;

                foreach (var hora in horasOrdenadas)
                {
                    if (datosPorHora.TryGetValue(hora, out var detalle))
                    {
                        decimal? valor = tipo == "DEMANDA PU" ? detalle.Demanda : detalle.Generacion;

                        if (valor.HasValue)
                        {
                            wsExcel.Cells[fila, col].Value = Math.Round(valor.Value, 1);
                            wsExcel.Cells[fila, col].Style.Numberformat.Format = "0.0";
                        }
                    }
                    fila++;
                }

                col++;
            }

            int colFinal = col - 1;
            int filaFinal = filaHora - 1;

            // Aplicar bordes a toda la tabla
            using (var range = wsExcel.Cells[1, 1, filaFinal, colFinal])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            // Fondo gris para la primera fila
            using (var range = wsExcel.Cells[1, 1, 1, colFinal])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Fondo gris para la primera columna (columna A)
            using (var range = wsExcel.Cells[1, 1, filaFinal, 1])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Autoajustar columnas
            for (int i = 1; i <= colFinal; i++)
            {
                wsExcel.Column(i).AutoFit();
            }
        }

        public static void GenerarHojaReporte4(ExcelWorksheet wsExcel, List<FormatoD1DDTO> lista, int anioInicio, int anioFin, List<DataCatalogoDTO> listaparam)
        {
            int fila = 1;
            int colEmpresaInicio = 4;
            // Obtener lista única de ProyCodi + Empresa + ProyNombre
            var proyectos = lista
                .Select(x => new { x.ProyCodi, x.Empresa, x.ProyNombre })
                .Distinct()
                .OrderBy(x => x.Empresa)
                .ThenBy(x => x.ProyCodi)
                .ToList();



            int totalColumnas = colEmpresaInicio + proyectos.Count - 1;

            // Título principal
            wsExcel.Cells[fila, 1].Value = "CRONOGRAMA DE EJECUCIÓN";
            wsExcel.Cells[fila, 1, fila, totalColumnas].Merge = true;
            wsExcel.Cells[fila, 1].Style.Font.Bold = true;
            wsExcel.Cells[fila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

            fila++;

            
            int filaEmpresa = fila;
            // Segunda fila - "Empresa propietaria" y nombre de la empresa
            wsExcel.Cells[fila, 1, fila, 3].Merge = true;
            wsExcel.Cells[fila, 1].Value = "Empresa propietaria";

            for (int i = 0; i < proyectos.Count; i++)
            {
                int col = colEmpresaInicio + i;
                wsExcel.Cells[fila, col].Value = proyectos[i].Empresa;
                wsExcel.Column(col).AutoFit();
            }
            fila++;

            int filaCarga = fila;
            // Tercera fila - "Nombre de la Carga" y ProyNombre (hacer merge con fila anterior)
            wsExcel.Cells[fila, 1, fila, 3].Merge = true;
            wsExcel.Cells[fila, 1].Value = "Nombre de la Carga";

            for (int i = 0; i < proyectos.Count; i++)
            {
                int col = colEmpresaInicio + i;
                wsExcel.Cells[fila, col, fila + 1, col].Merge = true;
                wsExcel.Cells[fila , col].Value = proyectos[i].ProyNombre;
                wsExcel.Cells[fila , col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[fila , col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            fila++;

            // Encabezado de columnas
            wsExcel.Cells[fila, 1].Value = "Actividad";
            wsExcel.Cells[fila, 2].Value = "Año";
            wsExcel.Cells[fila, 3].Value = "Trimestre";
           // wsExcel.Cells[fila+1, 1].Style.TextRotation = 90;
            fila++;

            // Lista de años reales desde anioInicio hasta anioFin
            var listaAnios = Enumerable.Range(anioInicio, anioFin - anioInicio + 1).ToList();

            // Obtener todas las actividades desde listaparam (no solo las que están en la data)
            var actividades = listaparam
                .Select(p => new {
                    Codigo = p.DataCatCodi,
                    Nombre = p.DesDataCat,
                    Valor = p.Valor
                })
                .Distinct()
                .OrderBy(x => x.Valor)
                .ToList();

            foreach (var actividad in actividades)
            {
                int inicioFilaActividad = fila;

                for (int i = 0; i < listaAnios.Count; i++)
                {
                    string anioReal = listaAnios[i].ToString();
                    int anioLogico = i + 1;
                    int inicioFilaAnio = fila;

                    for (int trimestre = 1; trimestre <= 4; trimestre++)
                    {
                        wsExcel.Cells[fila, 3].Value = trimestre;

                        foreach (var proyecto in proyectos)
                        {
                            var valor = lista.FirstOrDefault(x => x.DataCatCodi == actividad.Codigo && x.Anio == anioLogico.ToString() && x.Trimestre == trimestre && x.ProyCodi == proyecto.ProyCodi)?.Valor;
                            int col = colEmpresaInicio + proyectos.IndexOf(proyecto);

                            if (!string.IsNullOrEmpty(valor) && valor == "1")
                            {
                                wsExcel.Cells[fila, col].Value = 1;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                            }
                            else
                            {
                                wsExcel.Cells[fila, col].Value = 0;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            }
                        }

                        fila++;
                    }

                    wsExcel.Cells[inicioFilaAnio, 2, fila - 1, 2].Merge = true;
                    wsExcel.Cells[inicioFilaAnio, 2].Value = anioReal;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                wsExcel.Cells[inicioFilaActividad, 1, fila - 1, 1].Merge = true;
                wsExcel.Cells[inicioFilaActividad, 1].Value = actividad.Nombre;
                wsExcel.Cells[inicioFilaActividad, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.TextRotation = 90;
            }
            using (var rangoABC = wsExcel.Cells[1, 1, fila - 1, 3])
            {
                rangoABC.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangoABC.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
            }

            using (var range = wsExcel.Cells[1, 1, fila - 1, colEmpresaInicio + proyectos.Count - 1])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.AutoFitColumns();
            }
        }

        public static void GenerarReporteEmpresa(List<TransmisionProyectoDTO> reporte, string nombreTemp)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCampanias.FolderFichas;
            string pathNewFile = Path.Combine(ruta, nombreTemp, ConstantesCampanias.FolderEmpresa);
            FileInfo template = new FileInfo(Path.Combine(ruta, ConstantesCampanias.FolderReporte, ConstantesCampanias.FolderEmpresa, FormatoArchivosExcelCampanias.NombreReporteEmpresa));
            FileInfo newFile = new FileInfo(Path.Combine(pathNewFile, FormatoArchivosExcelCampanias.NombreReporteEmpresa));

            if (!template.Exists)
            {
                throw new FileNotFoundException("La plantilla no existe en la ruta especificada: " + template.FullName);
            }

            // Si el archivo ya existe, eliminarlo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(ruta, FormatoArchivosExcelCampanias.NombreReporteEmpresa));
            }

            int filaInicial = 5;
            int filaActual = filaInicial;
            int totalColumnas = 5; // Número de columnas en el reporte

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Reporte"];
                foreach (TransmisionProyectoDTO item in reporte)
                {
                    ws.Cells[filaActual, 1].Value = item.EmpresaNom;
                    ws.Cells[filaActual, 2].Value = item.TipoProyecto;
                    ws.Cells[filaActual, 3].Value = item.TipoSubProyecto;
                    ws.Cells[filaActual, 4].Value = item.Proynombre;
                    ws.Cells[filaActual, 5].Value = item.Planestado;
                    filaActual++;
                }
                AplicarEstilosHoja(ws, 1);
                xlPackage.Save();
            }
        }

        public static void GenerarHojaReporteCronograma<T>(ExcelWorksheet wsExcel, List<T> lista, int anioInicio, int anioFin, List<DataCatalogoDTO> listaparam, int filaIni)
        {
            int fila = filaIni;
            int colEmpresaInicio = 4;

            // Obtener las propiedades de T dinámicamente
            var type = typeof(T);
            var propProyCodi = type.GetProperty("ProyCodi");
            var propEmpresa = type.GetProperty("Empresa");
            var propProyNombre = type.GetProperty("ProyNombre");
            var propDataCatCodi = type.GetProperty("DataCatCodi") ?? type.GetProperty("Datacatcodi");
            var propAnio = type.GetProperty("Anio");
            var propTrimestre = type.GetProperty("Trimestre");
            var propValor = type.GetProperty("Valor");

            if (propProyCodi == null || propEmpresa == null || propProyNombre == null || propDataCatCodi == null || propAnio == null || propTrimestre == null || propValor == null)
            {
                throw new Exception($"El tipo {type.Name} no tiene todas las propiedades necesarias.");
            }

            // Obtener lista única de ProyCodi + Empresa + ProyNombre
            var proyectos = lista
                .Select(x => new
                {
                    ProyCodi = propProyCodi.GetValue(x)?.ToString(),
                    Empresa = propEmpresa.GetValue(x)?.ToString(),
                    ProyNombre = propProyNombre.GetValue(x)?.ToString()
                })
                .Where(x => x.ProyCodi != null && x.Empresa != null && x.ProyNombre != null)
                .Distinct()
                .OrderBy(x => x.Empresa)
                .ThenBy(x => x.ProyCodi)
                .ToList();

            int totalColumnas = colEmpresaInicio + proyectos.Count - 1;

            wsExcel.Cells[fila, 1].Value = "CRONOGRAMA DE EJECUCIÓN";
            wsExcel.Cells[fila, 1, fila, totalColumnas].Merge = true;
            wsExcel.Cells[fila, 1].Style.Font.Bold = true;
            wsExcel.Cells[fila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

            fila++;

            wsExcel.Cells[fila, 1, fila, 3].Merge = true;
            wsExcel.Cells[fila, 1].Value = "Empresa propietaria";

            for (int i = 0; i < proyectos.Count; i++)
            {
                int col = colEmpresaInicio + i;
                wsExcel.Cells[fila, col].Value = proyectos[i].Empresa;
                wsExcel.Column(col).AutoFit();
            }
            fila++;

            // Lista de años reales desde anioInicio hasta anioFin
            var listaAnios = Enumerable.Range(anioInicio, anioFin - anioInicio + 1).ToList();

            // Obtener todas las actividades desde listaparam
            var actividades = listaparam
                .Select(p => new
                {
                    Codigo = p.DataCatCodi,
                    Nombre = p.DesDataCat,
                    Valor = p.Valor,
                })
                .Distinct()
                .OrderBy(x => x.Valor)
                .ToList();

            foreach (var actividad in actividades)
            {
                int inicioFilaActividad = fila;

                for (int i = 0; i < listaAnios.Count; i++)
                {
                    string anioReal = listaAnios[i].ToString();
                    int anioLogico = i + 1;
                    int inicioFilaAnio = fila;

                    for (int trimestre = 1; trimestre <= 4; trimestre++)
                    {
                        wsExcel.Cells[fila, 3].Value = trimestre;
                        wsExcel.Cells[fila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (var proyecto in proyectos)
                        {
                            var valor = lista.FirstOrDefault(x =>
                                propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                                propAnio.GetValue(x)?.ToString() == anioLogico.ToString() &&
                                propTrimestre.GetValue(x)?.ToString() == trimestre.ToString() &&
                                propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                            )?.GetType().GetProperty("Valor")?.GetValue(lista.FirstOrDefault(x =>
                                propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                                propAnio.GetValue(x)?.ToString() == anioLogico.ToString() &&
                                propTrimestre.GetValue(x)?.ToString() == trimestre.ToString() &&
                                propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                            ))?.ToString();

                            int col = colEmpresaInicio + proyectos.IndexOf(proyecto);

                            if (!string.IsNullOrEmpty(valor) && valor == "1")
                            {
                                wsExcel.Cells[fila, col].Value = 1;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                                wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else
                            {
                                wsExcel.Cells[fila, col].Value = 0;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }

                        fila++;
                    }

                    wsExcel.Cells[inicioFilaAnio, 2, fila - 1, 2].Merge = true;
                    wsExcel.Cells[inicioFilaAnio, 2].Value = anioReal;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                wsExcel.Cells[inicioFilaActividad, 1, fila - 1, 1].Merge = true;
                wsExcel.Cells[inicioFilaActividad, 1].Value = actividad.Nombre;
                wsExcel.Cells[inicioFilaActividad, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.TextRotation = 90;
            }
            using (var rangoABC = wsExcel.Cells[1, 1, fila - 1, 3])
            {
                rangoABC.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangoABC.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
            }

            using (var range = wsExcel.Cells[1, 1, fila - 1, colEmpresaInicio + proyectos.Count - 1])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.AutoFitColumns();
            }
        }

        public static void GenerarHojaReporteCronogramaH<T>(ExcelWorksheet wsExcel, List<T> lista, int anioInicio, int anioFin, List<DataCatalogoDTO> listaparam, int filaIni)
        {
            int fila = filaIni;
            int colEmpresaInicio = 4;

            // Obtener las propiedades de T dinámicamente
            var type = typeof(T);
            var propProyCodi = type.GetProperty("ProyCodi");
            var propEmpresa = type.GetProperty("Empresa");
            var propProyNombre = type.GetProperty("ProyNombre");
            var propFecPuestaOpe = type.GetProperty("FecPuestaOpe");
            var propDataCatCodi = type.GetProperty("DataCatCodi") ?? type.GetProperty("Datacatcodi");
            var propAnio = type.GetProperty("Anio");
            var propTrimestre = type.GetProperty("Trimestre");
            var propValor = type.GetProperty("Valor");

            if (propProyCodi == null || propEmpresa == null || propProyNombre == null || propDataCatCodi == null || propAnio == null || propTrimestre == null || propValor == null || propFecPuestaOpe == null)
            {
                throw new Exception($"El tipo {type.Name} no tiene todas las propiedades necesarias.");
            }

            // Obtener lista única de ProyCodi + Empresa + ProyNombre
            var proyectos = lista
                .Select(x => new
                {
                    ProyCodi = propProyCodi.GetValue(x)?.ToString(),
                    Empresa = propEmpresa.GetValue(x)?.ToString(),
                    ProyNombre = propProyNombre.GetValue(x)?.ToString(),
                    FecPuestaOpe = propFecPuestaOpe.GetValue(x)?.ToString(),
                })
                .Where(x => x.ProyCodi != null && x.Empresa != null && x.ProyNombre != null)
                .Distinct()
                .OrderBy(x => x.Empresa)
                .ThenBy(x => x.ProyCodi)
                .ToList();

            int totalColumnas = colEmpresaInicio + proyectos.Count - 1;

            wsExcel.Cells[fila, 1].Value = "CRONOGRAMA DE EJECUCIÓN";
            wsExcel.Cells[fila, 1, fila, totalColumnas].Merge = true;
            wsExcel.Cells[fila, 1].Style.Font.Bold = true;
            wsExcel.Cells[fila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.PatternType = ExcelFillStyle.Solid;
            wsExcel.Cells[fila, 1, fila, totalColumnas].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));

            fila++;

            wsExcel.Cells[fila, 1, fila, 3].Merge = true;
            wsExcel.Cells[fila, 1].Value = "Empresa propietaria";

            for (int i = 0; i < proyectos.Count; i++)
            {
                int col = colEmpresaInicio + i;
                wsExcel.Cells[fila, col].Value = proyectos[i].Empresa;
                wsExcel.Column(col).AutoFit();
            }
            fila++;

            wsExcel.Cells[fila, 1, fila, 3].Merge = true;
            wsExcel.Cells[fila, 1].Value = "Fecha (mes/año) de Puesta en Operación Comercial";

            for (int i = 0; i < proyectos.Count; i++)
            {
                int col = colEmpresaInicio + i;
                DateTime fecha;
                if (DateTime.TryParse(proyectos[i].FecPuestaOpe.ToString(), out fecha))
                {
                    wsExcel.Cells[fila, col].Value = fecha;
                    wsExcel.Cells[fila, col].Style.Numberformat.Format = "MM/yyyy"; 
                    wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                wsExcel.Column(col).AutoFit();
            }
            fila++;

            // Lista de años reales desde anioInicio hasta anioFin
            var listaAnios = Enumerable.Range(anioInicio, anioFin - anioInicio + 1).ToList();

            // Obtener todas las actividades desde listaparam
            var actividades = listaparam
                .Select(p => new
                {
                    Codigo = p.DataCatCodi,
                    Nombre = p.DesDataCat,
                    Valor = p.Valor
                })
                .Distinct()
                .OrderBy(x => x.Valor)
                .ToList();

            foreach (var actividad in actividades)
            {
                int inicioFilaActividad = fila;
                string anioAntes = (Convert.ToInt32(listaAnios[0]) - 1).ToString();
                int anio = 0;
                int inicioFilaAntes = fila;

                wsExcel.Cells[fila, 2, fila, 3].Value = $"{anioAntes}  ó antes";
                wsExcel.Cells[fila, 2, fila, 3].Merge = true;
                wsExcel.Cells[fila, 2, fila, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[fila, 2, fila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                foreach (var proyecto in proyectos)
                {
                    var valor = lista.FirstOrDefault(x =>
                        propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                        propAnio.GetValue(x)?.ToString() == anio.ToString() &&
                        propTrimestre.GetValue(x)?.ToString() == anio.ToString() &&
                        propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                    )?.GetType().GetProperty("Valor")?.GetValue(lista.FirstOrDefault(x =>
                        propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                        propAnio.GetValue(x)?.ToString() == anio.ToString() &&
                        propTrimestre.GetValue(x)?.ToString() == anio.ToString() &&
                        propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                    ))?.ToString();

                    int col = colEmpresaInicio + proyectos.IndexOf(proyecto);

                    if (!string.IsNullOrEmpty(valor) && valor == "1")
                    {
                        wsExcel.Cells[fila, col].Value = 1;
                        wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                        wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else
                    {
                        wsExcel.Cells[fila, col].Value = 0;
                        wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                fila++;

                for (int i = 0; i < listaAnios.Count; i++)
                {
                    string anioReal = listaAnios[i].ToString();
                    int anioLogico = i + 1;
                    int inicioFilaAnio = fila;

                    for (int trimestre = 1; trimestre <= 4; trimestre++)
                    {
                        wsExcel.Cells[fila, 3].Value = trimestre;
                        wsExcel.Cells[fila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (var proyecto in proyectos)
                        {
                            var valor = lista.FirstOrDefault(x =>
                                propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                                propAnio.GetValue(x)?.ToString() == anioLogico.ToString() &&
                                propTrimestre.GetValue(x)?.ToString() == trimestre.ToString() &&
                                propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                            )?.GetType().GetProperty("Valor")?.GetValue(lista.FirstOrDefault(x =>
                                propDataCatCodi.GetValue(x)?.ToString() == actividad.Codigo.ToString() &&
                                propAnio.GetValue(x)?.ToString() == anioLogico.ToString() &&
                                propTrimestre.GetValue(x)?.ToString() == trimestre.ToString() &&
                                propProyCodi.GetValue(x)?.ToString() == proyecto.ProyCodi
                            ))?.ToString();

                            int col = colEmpresaInicio + proyectos.IndexOf(proyecto);

                            if (!string.IsNullOrEmpty(valor) && valor == "1")
                            {
                                wsExcel.Cells[fila, col].Value = 1;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9D9D9"));
                                wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else
                            {
                                wsExcel.Cells[fila, col].Value = 0;
                                wsExcel.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                wsExcel.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                wsExcel.Cells[fila, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }

                        fila++;
                    }

                    wsExcel.Cells[inicioFilaAnio, 2, fila - 1, 2].Merge = true;
                    wsExcel.Cells[inicioFilaAnio, 2].Value = anioReal;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsExcel.Cells[inicioFilaAnio, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                wsExcel.Cells[inicioFilaActividad, 1, fila - 1, 1].Merge = true;
                wsExcel.Cells[inicioFilaActividad, 1].Value = actividad.Nombre;
                wsExcel.Cells[inicioFilaActividad, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[inicioFilaActividad, 1].Style.TextRotation = 90;
            }
            using (var rangoABC = wsExcel.Cells[1, 1, fila - 1, 3])
            {
                rangoABC.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangoABC.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
            }

            using (var range = wsExcel.Cells[1, 1, fila - 1, colEmpresaInicio + proyectos.Count - 1])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.AutoFitColumns();
            }
        }

        public static void GenerarHojaReporteSubestacion<T>(ExcelWorksheet wsExcel, List<T> lista, List<DataCatalogoDTO> listaparam, int filaIni, string NameCol)
        {
            int fila = filaIni + 1;
            int filaData = filaIni + 4;
            int colEmpresa = 3;

            // Obtener lista de actividades únicas y ordenadas
            var actividades = listaparam
                .Select(p => new
                {
                    Codigo = p.DataCatCodi,
                    Nombre = p.DesDataCat,
                    NombreCort = p.DescortaDatacat,
                    CatCod = p.CatCodi
                })
                .Distinct()
                .Distinct()
                .OrderBy(x => x.CatCod)
                .ThenBy(x => x.Codigo)
                .ToList();

            int lastCadCodi = actividades[0].CatCod;
            int filaDataChange = 0;
            foreach (var actividad in actividades)
            {
                if (lastCadCodi != actividad.CatCod) {
                    filaDataChange = filaData;
                }
                wsExcel.Cells[filaData, 2].Value = !string.IsNullOrEmpty(actividad.NombreCort) ? $"{actividad.Nombre} ({actividad.NombreCort})" : actividad.Nombre;
                lastCadCodi = actividad.CatCod;
                filaData++;
            }

            if (filaDataChange == 0)
            {
                wsExcel.Cells[filaIni + 4, 1, filaData - 1, 1].Merge = true;
                wsExcel.Cells[filaIni + 4, 1, filaData - 1, 1].Value = "CARACTERISTICAS";
                wsExcel.Cells[filaIni + 4, 1, filaData - 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[filaIni + 4, 1, filaData - 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[filaIni + 4, 1, filaData - 1, 1].Style.TextRotation = 90;
            }
            else {
                wsExcel.Cells[filaIni + 4, 1, filaDataChange - 1, 1].Merge = true;
                wsExcel.Cells[filaIni + 4, 1, filaDataChange - 1, 1].Value = "CARACTERISTICAS";
                wsExcel.Cells[filaIni + 4, 1, filaDataChange - 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[filaIni + 4, 1, filaDataChange - 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[filaIni + 4, 1, filaDataChange - 1, 1].Style.TextRotation = 90;
                wsExcel.Cells[filaDataChange, 1, filaData - 1, 1].Merge = true;
                wsExcel.Cells[filaDataChange, 1, filaData - 1, 1].Value = "PRUEBAS";
                wsExcel.Cells[filaDataChange, 1, filaData - 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsExcel.Cells[filaDataChange, 1, filaData - 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsExcel.Cells[filaDataChange, 1, filaData - 1, 1].Style.TextRotation = 90;
            }

            using (var rangoABC = wsExcel.Cells[1, 1, filaData - 1, 2])
            {
                rangoABC.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangoABC.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9D9D9"));
            }

            filaData = filaIni + 4;

            foreach (var proyecto in lista)
            {
                fila = filaIni + 1;
                dynamic dynProyecto = proyecto;

                wsExcel.Cells[fila, colEmpresa].Value = dynProyecto.EmpresaPropietaria;
                fila++;
                wsExcel.Cells[fila, colEmpresa].Value = dynProyecto.NombreSubestacion;
                fila++;
                var numTrafo = dynProyecto.GetType().GetProperty("NumTrafo")?.GetValue(dynProyecto);
                var numEquipo = dynProyecto.GetType().GetProperty("NumEquipo")?.GetValue(dynProyecto);
                string numMostrar = numTrafo != null ? numTrafo.ToString() : (numEquipo ?? "").ToString();
                wsExcel.Cells[fila, colEmpresa].Value = $"{NameCol} {numMostrar}";
                fila++;

                string[] codigosCatalogo = dynProyecto.DataCatCodiGroup?.Split(',') ?? new string[0];
                string[] valoresProyecto = dynProyecto.ValorGroup?.Split(',') ?? new string[0];

                for (int i = 0; i < codigosCatalogo.Length; i++)
                {
                    string codigo = codigosCatalogo[i].Trim();

                    int index = actividades.FindIndex(a => a.Codigo.ToString() == codigo);

                    if (index != -1 && i < valoresProyecto.Length)
                    {
                        fila = filaData + index;
                        if (decimal.TryParse(valoresProyecto[i], out decimal valorNumerico))
                        {
                            wsExcel.Cells[fila, colEmpresa].Value = valorNumerico;
                        }
                        else
                        {
                            wsExcel.Cells[fila, colEmpresa].Value = valoresProyecto[i]; 
                        }
                    }
                }
                colEmpresa++;
            }
        }

        private static ExcelWorksheet ObtenerHoja(ExcelWorkbook workbook, string nombreHoja)
        {
            ExcelWorksheet ws = workbook.Worksheets[nombreHoja];
            if (ws == null)
            {
                throw new Exception($"La hoja '{nombreHoja}' no existe en la plantilla.");
            }
            return ws;
        }

        private static void ProcesarLista<T>(ExcelWorksheet ws, IEnumerable<T> lista, Action<ExcelWorksheet, T, int> procesarFila, int filaInicial)
        {
            int filaActual = filaInicial;
            foreach (T item in lista)
            {
                procesarFila(ws, item, filaActual);
                filaActual++;
            }
        }

        private static void ProcesarListaColum<T>(ExcelWorksheet ws, IEnumerable<T> lista, Action<ExcelWorksheet, T, int> procesarColumna, int columnaInicial)
        {
            int columnaActual = columnaInicial;
            foreach (T item in lista)
            {
                procesarColumna(ws, item, columnaActual);
                columnaActual++;
            }
        }

        private static void DeleteCeldasRowRestantesExcel(int filaInicio, ExcelWorksheet worksheet)
        {
            int numeroFilas = worksheet.Dimension.End.Row - filaInicio + 1;

            worksheet.DeleteRow(filaInicio, numeroFilas);
        }

        private static void DeleteCeldasColumnRestantesExcel(int columnInicio, ExcelWorksheet worksheet)
        {
            int numeroColumnas = worksheet.Dimension.End.Column - columnInicio + 1;

            worksheet.DeleteColumn(columnInicio, numeroColumnas);
        }


    }


}