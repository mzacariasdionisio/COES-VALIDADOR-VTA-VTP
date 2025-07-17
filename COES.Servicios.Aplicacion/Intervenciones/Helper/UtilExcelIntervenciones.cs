using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UtilExcelIntervenciones
    {
        #region Formato de Sustento

        /// <summary>
        /// Generar excel rerpote sustento
        /// </summary>
        /// <param name="regIn"></param>
        /// <param name="regSt"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="listaPathImagen"></param>
        public static void GenerarExcelReporteSustento(InIntervencionDTO regIn, InSustentoDTO regSt, string path, string fileName, List<InArchivoDTO> listaPathImagen)
        {
            //generar excel
            string file = path + fileName;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Sustento");
                HojaExcelReporteSustento(ws, regIn, regSt, listaPathImagen);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Generar hoja excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="regIn"></param>
        /// <param name="regSt"></param>
        /// <param name="listaPathImagen"></param>
        private static void HojaExcelReporteSustento(ExcelWorksheet ws, InIntervencionDTO regIn, InSustentoDTO regSt, List<InArchivoDTO> listaPathImagen)
        {
            int col = 2;
            int row = 1;

            #region filtro
            int rowTitulo = row;
            ws.Cells[rowTitulo, col].Value = (regSt.FlagTieneInclusion ? "Informe de Inclusión" : "Informe de Exclusión") + " - " + regIn.Nomprogramacion;
            ws.Cells[rowTitulo, col].Style.Font.Bold = true;
            UtilExcel.CeldasExcelAgrupar(ws, rowTitulo, col, rowTitulo, col + 1);
            UtilExcel.SetFormatoCelda(ws, rowTitulo, col, rowTitulo, col + 1, "Centro", "Centro", "#FFFFFF", "#2980B9", "Calibri", 11, true);

            row += 2;

            int rowIniEmpresa = row;
            int rowIniEquipo = rowIniEmpresa + 1;
            int rowIniDescripcion = rowIniEquipo + 1;
            int rowIniFechaIni = rowIniDescripcion + 1;
            int rowIniFechaFin = rowIniFechaIni + 1;
            int rowIniTipoIntervencion = rowIniFechaFin + 1;
            int colIniLabel = col;
            int colIniValor = col + 1;

            ws.Cells[rowIniEmpresa, colIniLabel].Value = "Empresa:";
            ws.Cells[rowIniEmpresa, colIniValor].Value = regIn.EmprNomb;

            ws.Cells[rowIniEquipo, colIniLabel].Value = "Equipo:";
            ws.Cells[rowIniEquipo, colIniValor].Value = regIn.AreaNomb + " " + regIn.Equiabrev;

            ws.Cells[rowIniDescripcion, colIniLabel].Value = "Descripción:";
            ws.Cells[rowIniDescripcion, colIniValor].Value = regIn.Interdescrip;

            ws.Cells[rowIniFechaIni, colIniLabel].Value = "Fecha de inicio:";
            ws.Cells[rowIniFechaIni, colIniValor].Value = regIn.InterfechainiDesc;

            ws.Cells[rowIniFechaFin, colIniLabel].Value = "Fecha de fin:";
            ws.Cells[rowIniFechaFin, colIniValor].Value = regIn.InterfechafinDesc;

            ws.Cells[rowIniTipoIntervencion, colIniLabel].Value = "Tipo de Intervención:";
            ws.Cells[rowIniTipoIntervencion, colIniValor].Value = regIn.TipoEvenDesc;

            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniEmpresa, colIniLabel, rowIniTipoIntervencion, colIniValor, "#000000", true, true);
            UtilExcel.CeldasExcelWrapText(ws, rowIniEmpresa, colIniLabel, rowIniTipoIntervencion, colIniValor);
            UtilExcel.SetFormatoCelda(ws, rowIniEmpresa, colIniLabel, rowIniTipoIntervencion, colIniLabel, "Centro", "Izquierda", "#FFFFFF", "#2980B9", "Calibri", 11, true);

            ws.Row(rowIniTipoIntervencion + 1).Height = 30;

            row = rowIniTipoIntervencion + 2;

            #endregion

            #region cabecera

            int rowIniCabecera = row;
            int colIniReq = col;
            int colIniRpta = colIniReq + 1;

            ws.Cells[rowIniCabecera, colIniReq].Value = "Requisito";
            ws.Cells[rowIniCabecera, colIniRpta].Value = "Respuesta";

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colIniReq, rowIniCabecera, colIniRpta, "Centro", "Centro", "#FFFFFF", "#2980B9", "Calibri", 11, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniCabecera, colIniReq, rowIniCabecera, colIniRpta, "#000000", true, true);

            row = rowIniCabecera;
            #endregion

            row += 1;

            #region cuerpo

            foreach (var item in regSt.ListaItem)
            {
                ws.Cells[row, colIniReq].Value = item.Inpstidesc;
                ws.Cells[row, colIniRpta].Value = item.Instdrpta;
                UtilExcel.CeldasExcelWrapText(ws, row, colIniReq, row, colIniRpta);
                UtilExcel.BorderCeldasLineaDelgada(ws, row, colIniReq, row, colIniRpta, "#000000", true, true);

                row++;
            }
            #endregion

            if (listaPathImagen != null)
            {
                //row += 3;

                foreach (var item in listaPathImagen)
                {
                    //maximo 500 alto
                    int alto = 500;
                    int ancho = 1000;
                    if (item.Alto > 0 && item.Ancho > 0)
                    {
                        decimal factor = 500.0m / item.Alto;
                        alto = (int)(factor * item.Alto);
                        ancho = (int)(factor * item.Ancho);
                    }
                    UtilExcel.AddImageLocal(ws, colIniReq - 1, row, item.PathArchivo, ancho, alto);
                    row += 25;
                }
            }

            //ancho de columnas
            ws.Column(1).Width = 3;
            ws.Column(colIniReq).Width = 35;
            ws.Column(colIniRpta).Width = 60;
        }

        #endregion

        #region Upload sustento

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<InFilaSustento> ImportSustentoToDataTable(string filePath)
        {
            List<InFilaSustento> listaMacro = new List<InFilaSustento>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexReq = 2; //2
            int indexRpta = indexReq + 1;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 11;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sReq = string.Empty;
                    if (worksheet.Cells[row, indexReq].Value != null) sReq = worksheet.Cells[row, indexReq].Value.ToString();

                    var sRpta = string.Empty;
                    if (worksheet.Cells[row, indexRpta].Value != null) sRpta = worksheet.Cells[row, indexRpta].Value.ToString();

                    sReq = (sReq ?? "").Trim().ToUpper();
                    sRpta = (sRpta ?? "").Trim();

                    if (string.IsNullOrEmpty(sReq))
                    {
                        continue;
                    }

                    var regMantto = new InFilaSustento()
                    {
                        Row = row,
                        Requisito = sReq,
                        Respuesta = sRpta,
                    };

                    listaMacro.Add(regMantto);
                }
            }

            return listaMacro;
        }


        #endregion
    }


    public class InFilaSustento
    {
        public int Row { get; set; }
        public string Requisito { get; set; }
        public string Respuesta { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
