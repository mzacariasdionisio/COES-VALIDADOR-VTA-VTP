using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace COES.Servicios.Aplicacion.IEOD.Helper
{
    /// <summary>
    /// Clase utilitaria
    /// </summary>
    public class CargaDatosUtil
    {
        //colores de celdas 
        public const string ColorSinDatos = "#B4C6E7";
        public const string ColorDatoEstimado = "#D9D9D9";
        public const string ColorEquipos = "#305496";
        public const string ColorFeriado = "#22B14C";
        public const string ColorNormal = "#FFFFFF";

        //color de lectura excel
        public const string RGBColorSinDatos = "FFB4C6E7";
        public const string RGBColorDatoEstimado = "FFD9D9D9";
        public const string RGBColorEquipos = "FF305496";

        //diferenciar fuentes de datos
        public const int TipoInfoMed48 = 0;
        public const int TipoInfoScada = 1;

        /// <summary>
        /// Permite formatear las celdas cabeceras
        /// </summary>
        /// <param name="rg"></param>
        public static void FormatearCeldaTitulo(ExcelRange rg)
        {
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4C97C3"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;
        }

        /// <summary>
        /// Permite obtener los colores
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ObtenerCodigoColor(string color)
        {
            int idColor = 0;
            switch (color)
            {
                case RGBColorSinDatos:
                    idColor = -1;
                    break;
                case RGBColorDatoEstimado:
                    idColor = 1;
                    break;
                case RGBColorEquipos:
                    idColor = 2;
                    break;
                default:
                    idColor = 3;
                    break;

            }
            return idColor;
        }

        /// <summary>
        /// Formato segun tipo de celda
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="tipo"></param>
        public static void FormatearCeldaItem(ExcelRange rg, int tipo)
        {
            string color = ColorCeldaMeMedicion(tipo);

            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
        }

        public static string ColorCeldaMeMedicion(int tipo)
        {
            return (tipo == -1) ? ColorSinDatos : (tipo == 1) ? ColorDatoEstimado : (tipo == 2) ? ColorEquipos : ColorNormal;
        }

        /// <summary>
        /// Formatear hoja 
        /// </summary>
        /// <param name="rg"></param>
        public static void FormatearCeldaGeneral(ExcelRange rg)
        {
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            rg.Style.Font.Size = 10;
        }

        public static void AgregarSeccionNota(int reporcodi, ExcelWorksheet ws, int rowIni, int colIni)
        {
            if (ConstantesPR5ReportesServicio.IdReporteFlujo == reporcodi || ConstantesPR5ReportesServicio.IdReporteFlujoIDCOS == reporcodi)
            {
                ws.Cells[rowIni, colIni].Value = "Nota: Información proviene de datos instantáneos de potencia cada 30 minutos (Valores SCADA)";
                UtilExcel.CeldasExcelEnNegrita(ws, rowIni, colIni, rowIni, colIni);

                int fila1 = rowIni + 1;
                int fila2 = fila1 + 1;
                int fila3 = fila2 + 1;

                UtilExcel.CeldasExcelColorFondo(ws, fila1, colIni, fila1, colIni, ColorDatoEstimado);
                ws.Cells[fila1, colIni + 1].Value = "Datos estimados";

                UtilExcel.CeldasExcelColorFondo(ws, fila2, colIni, fila2, colIni, ColorSinDatos);
                ws.Cells[fila2, colIni + 1].Value = "Sin datos";

                UtilExcel.CeldasExcelColorFondo(ws, fila3, colIni, fila3, colIni, ColorEquipos);
                ws.Cells[fila3, colIni + 1].Value = "Equipos intervienen en demanda por áreas y congestiones";

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, fila1, colIni, fila3, colIni, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, fila1, colIni, fila3, colIni, "Centro");
                UtilExcel.BorderCeldasLineaDelgada(ws, fila1, colIni, fila3, colIni, "#9F9F9F", true, true);
            }

            if (ConstantesPR5ReportesServicio.IdReporteTension == reporcodi)
            {
                ws.Cells[rowIni, colIni].Value = "Nota: Información proviene de datos instantáneos de tensión cada 30 minutos (Valores SCADA)";
                UtilExcel.CeldasExcelEnNegrita(ws, rowIni, colIni, rowIni, colIni);

                int fila1 = rowIni + 1;
                int fila2 = fila1 + 1;

                UtilExcel.CeldasExcelColorFondo(ws, fila1, colIni, fila1, colIni, ColorDatoEstimado);
                ws.Cells[fila1, colIni + 1].Value = "Datos estimados";

                UtilExcel.CeldasExcelColorFondo(ws, fila2, colIni, fila2, colIni, ColorSinDatos);
                ws.Cells[fila2, colIni + 1].Value = "Sin datos";

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, fila1, colIni, fila2, colIni, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, fila1, colIni, fila2, colIni, "Centro");
                UtilExcel.BorderCeldasLineaDelgada(ws, fila1, colIni, fila2, colIni, "#9F9F9F", true, true);
            }
        }

        /// <summary>
        /// Permite obtener las fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="diaSemana"></param>
        /// <returns></returns>
        public static List<DateTime> ObtenerFechasPorDiaSemana(DateTime fechaInicio, DateTime fechaFin, int diaSemana)
        {
            List<DateTime> result = new List<DateTime>();

            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

            for (int i = 0; i <= dias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);

                if (((int)fecha.DayOfWeek) == diaSemana)
                {
                    result.Add(fecha);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Estructura para respuesta
    /// </summary>
    public class EstructuraRespuesta
    {
        public List<InformacionCelda> ListaMerge { get; set; }
        public List<InformacionCelda> ListaInformacionCelda { get; set; }
        public List<InformacionCelda> ListaFeriados { get; set; }
        public List<List<string>> Data { get; set; }
        public int Result { get; set; }
        public int? ExisteDatos { get; set; }
        public List<string> ListaColorColumna { get; set; }
    }

    /// <summary>
    /// Informacion de las celdas de datos
    /// </summary>
    public class InformacionCelda
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Colspan { get; set; }
        public int Tipo { get; set; }
    }
}
