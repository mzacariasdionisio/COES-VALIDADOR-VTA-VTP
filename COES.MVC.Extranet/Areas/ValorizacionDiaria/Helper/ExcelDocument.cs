using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.ValorizacionDiaria.Models;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria.Helper
{
    public class ExcelDocument
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
                picture.From.ColumnOff = ExcelHelper.Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = ExcelHelper.Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }
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

        #region Monto Por Energia
        /// <summary>
        /// Monto por Energia (ME)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        //configuracion de la hoja de excel de Monto por Energia
        public static void ConfiguracionHojaExcelME(ExcelWorksheet ws, List<VtdMontoPorEnergiaDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 6].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 6].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha"; ws.Cells[5, 3].Value = "Participante"; ws.Cells[5, 4].Value = "Montos de Retiro"; ws.Cells[5, 5].Value = "Montos de Entrega"; ws.Cells[5, 6].Value = "Montos de Energia";
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Valdretiro;
                    ws.Cells[row, column + 3].Value = reg.Valdentrega;
                    ws.Cells[row, column + 4].Value = reg.Valdretiro - reg.Valdentrega;
                    border = ws.Cells[row, 2, row, 6].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 6].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }

        public static void GenerarArchivoMontoEnergia(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelMontoPorEnergia);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorEnergia);
            List<VtdMontoPorEnergiaDTO> list = model.MontoPorEnergia;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorEnergia);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Monto Por Energia");
                ws = xlPackage.Workbook.Worksheets["RPTE Monto Por Energia"];
                string titulo = "Monto Por Energía";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelME(ws, list);
                xlPackage.Save();
            }
        }
        #endregion

        #region Monto Por Capacidad
        /// <summary>
        /// Monto por Capacidad (MC)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        //configuracion de la hoja de excel de Monto por Capacidad
        public static void ConfiguracionHojaExcelMC(ExcelWorksheet ws, List<VtdMontoPorCapacidadDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 9].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 9].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha"; ws.Cells[5, 3].Value = "Participante"; ws.Cells[5, 4].Value = "Margen Reserva"; ws.Cells[5, 5].Value = "Precio Potencia"; ws.Cells[5, 6].Value = "PFirmeRemun(KW)";
            ws.Cells[5, 7].Value = "PFirmeRemun/(1+mr)(KW)"; ws.Cells[5, 8].Value = "Demanda Cliente"; ws.Cells[5, 9].Value = "Monto Por Capacidad";
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 30;
            ws.Column(9).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Valomr;
                    ws.Cells[row, column + 3].Value = reg.Valopreciopotencia;
                    ws.Cells[row, column + 4].Value = reg.Valdpfirremun;
                    ws.Cells[row, column + 5].Value = reg.Valdpfirremun / (1 + reg.Valomr);
                    ws.Cells[row, column + 6].Value = reg.Valddemandacoincidente;
                    ws.Cells[row, column + 7].Value = (((reg.Valddemandacoincidente - reg.Valdpfirremun) / (1 + reg.Valomr)) * reg.Valopreciopotencia);
                    border = ws.Cells[row, 2, row, 9].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 9].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }
        public static void GenerarArchivoMontoCapacidad(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelMontoPoCapacidad);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorCapacidad);
            List<VtdMontoPorCapacidadDTO> list = model.MontoPorCapacidad;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorCapacidad);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Monto Por Capacidad");
                ws = xlPackage.Workbook.Worksheets["RPTE Monto Por Capacidad"];
                string titulo = "Monto Por Capacidad";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelMC(ws, list);
                xlPackage.Save();
            }
        }
        #endregion

        #region Monto Por Peaje
        /// <summary>
        /// Monto por Peaje (MP)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        //configuracion de la hoja de excel de Monto por Peaje
        public static void ConfiguracionHojaExcelMP(ExcelWorksheet ws, List<VtdMontoPorPeajeDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 6].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 6].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha"; ws.Cells[5, 3].Value = "Participante"; ws.Cells[5, 4].Value = "Demanda Cliente"; ws.Cells[5, 5].Value = "Monto Por Peaje unitario"; ws.Cells[5, 6].Value = "Monto Por Peaje";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Valddemandacoincidente;
                    ws.Cells[row, column + 3].Value = reg.Valdpeajeuni;
                    ws.Cells[row, column + 4].Value = (reg.Valddemandacoincidente * reg.Valdpeajeuni);
                    border = ws.Cells[row, 2, row, 6].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 6].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }
        public static void GenerarArchivoMontoPeaje(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelMontoPorPeaje);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorPeaje);
            List<VtdMontoPorPeajeDTO> list = model.MontoPorPeaje;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorPeaje);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Monto Por Peaje");
                ws = xlPackage.Workbook.Worksheets["RPTE Monto Por Peaje"];
                string titulo = "Monto Por Peaje por Conexión en SPT y Peaje por Trans. en el SGT";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelMP(ws, list);
                xlPackage.Save();
            }
        }
        #endregion

        #region Monto Por Exceso
        /// <summary>
        /// Monto Por Exceso de Consumo (MEx)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        //configuracion de la hoja de excel de Monto por Exceso de Consumo
        public static void ConfiguracionHojaExcelMEx(ExcelWorksheet ws, List<VtdMontoPorExcesoDTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 10].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 10].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha"; ws.Cells[5, 3].Value = "Participante"; ws.Cells[5, 4].Value = "Frec Total S/./dia";
            ws.Cells[5, 5].Value = "Otros Equipos S/./dia"; ws.Cells[5, 6].Value = "Costos Fuera Banda S/./dia";
            ws.Cells[5, 7].Value = "Comp. Term. RT S/./dia"; ws.Cells[5, 8].Value = "Cargos por Consumo en Exceso S/./dia";
            ws.Cells[5, 9].Value = "Aportes Adicionales S/./dia"; ws.Cells[5, 10].Value = "Total S/./dia";
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 30;
            ws.Column(6).Width = 30;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 35;
            ws.Column(9).Width = 30;
            ws.Column(10).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Valofrectotal;
                    ws.Cells[row, column + 3].Value = reg.Valootrosequipos;
                    ws.Cells[row, column + 4].Value = reg.Valocostofuerabanda;
                    ws.Cells[row, column + 5].Value = reg.Valocomptermrt;
                    ws.Cells[row, column + 6].Value = reg.Valdcargoconsumo;
                    ws.Cells[row, column + 7].Value = reg.Valdaportesadicional;
                    ws.Cells[row, column + 8].Value = (reg.Valdcargoconsumo + reg.Valdaportesadicional);
                    border = ws.Cells[row, 2, row, 10].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 10].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }
        public static void GenerarArchivoMontoExceso(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelMontoPorExceso);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorExceso);
            List<VtdMontoPorExcesoDTO> list = model.MontoPorExceso;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoPorExceso);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Monto Por Exceso");
                ws = xlPackage.Workbook.Worksheets["RPTE Monto Por Exceso"];
                string titulo = "Monto Por Exceso de Consumo de Energia Reactiva";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelMEx(ws, list);
                xlPackage.Save();
            }
        }
        #endregion

        #region Monto por SCeIO
        /// <summary>
        /// Monto por SCeIO
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param> 
        //configuracion de la hoja de excel de ServiciosComplementarios e InflexibilidadesOperativas
        public static void ConfiguracionHojaExcelSCeIO(ExcelWorksheet ws, List<VtdMontoSCeIODTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 19].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 19].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha";
            ws.Cells[5, 3].Value = "Participante";
            ws.Cells[5, 4].Value = "Fpgm";
            ws.Cells[5, 5].Value = "% perdida";
            ws.Cells[5, 6].Value = "Factor P";
            ws.Cells[5, 7].Value = "MCio S/.";
            ws.Cells[5, 8].Value = "PDsc S/.";
            ws.Cells[5, 9].Value = "CO S/.";
            ws.Cells[5, 10].Value = "RA";
            ws.Cells[5, 11].Value = "RA Subida";
            ws.Cells[5, 12].Value = "RA Bajada";
            ws.Cells[5, 13].Value = "Demanda MW COES";
            ws.Cells[5, 14].Value = "Factor de Reparto";
            ws.Cells[5, 15].Value = "Comp. Costo Ope S/.";
            ws.Cells[5, 16].Value = "Of max";
            ws.Cells[5, 17].Value = "Of max baj";
            ws.Cells[5, 18].Value = "PAGOio S/.";
            ws.Cells[5, 19].Value = "PAGOsc S/.";
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
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
            ws.Column(14).Width = 30;
            ws.Column(15).Width = 30;
            ws.Column(16).Width = 30;
            ws.Column(17).Width = 30;
            ws.Column(18).Width = 30;
            ws.Column(19).Width = 30;
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = reg.Valdfpgm;
                    ws.Cells[row, column + 3].Value = reg.Valoporcentajeperdida;
                    ws.Cells[row, column + 4].Value = reg.Valdfactorp;
                    ws.Cells[row, column + 5].Value = reg.Valdmcio;
                    ws.Cells[row, column + 6].Value = reg.Valdpdsc;
                    ws.Cells[row, column + 7].Value = reg.Valoco;
                    ws.Cells[row, column + 8].Value = reg.Valora;
                    ws.Cells[row, column + 9].Value = reg.ValoraSub;
                    ws.Cells[row, column + 10].Value = reg.ValoraBaj;
                    ws.Cells[row, column + 11].Value = reg.Valodemandacoes;
                    ws.Cells[row, column + 12].Value = reg.Valofactorreparto;
                    ws.Cells[row, column + 13].Value = reg.Valocompcostosoper;
                    ws.Cells[row, column + 14].Value = reg.Valoofmax;
                    ws.Cells[row, column + 15].Value = reg.ValoofmaxBaj;
                    ws.Cells[row, column + 16].Value = reg.Valdpagoio;
                    ws.Cells[row, column + 17].Value = reg.Valdpagosc;
                    border = ws.Cells[row, 2, row, 19].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 19].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }
        public static void GenerarArchivoMontoSCeIO(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelMontoSCeIO);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoSCeIO);
            List<VtdMontoSCeIODTO> list = model.MontoSCeIO;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteMontoSCeIO);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Monto SCeIO");
                ws = xlPackage.Workbook.Worksheets["RPTE Monto SCeIO"];
                string titulo = "Monto Por Servicios Complementarios e Inflexibilidades Operativas";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelSCeIO(ws, list);
                xlPackage.Save();
            }
        }
        #endregion

        #region Valorizacion Diaria
        /// <summary>
        /// Valorizacion Diaria (VD)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        //configuracion de la hoja de excel de Valorizacion Diaria
        public static void ConfiguracionHojaExcelVD(ExcelWorksheet ws, List<ValorizacionDiariaDTO> lista, List<ValorizacionDiariaDTO> listames)
        {
            var fill = ws.Cells[5, 2, 5, 8].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 8].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            var fill2 = ws.Cells[5, 10, 5, 14].Style;
            fill2.Fill.PatternType = ExcelFillStyle.Solid;
            fill2.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill2.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill2.Border.Bottom.Style = fill2.Border.Top.Style = fill2.Border.Left.Style = fill2.Border.Right.Style = ExcelBorderStyle.Thin;

            var border2 = ws.Cells[5, 10, 5, 14].Style.Border;
            border2.Bottom.Style = border2.Top.Style = border2.Left.Style = border2.Right.Style = ExcelBorderStyle.Thin;
            //
            ws.Cells[5, 2].Value = "Fecha";
            ws.Cells[5, 3].Value = "Participante";
            ws.Cells[5, 4].Value = "Por Energia Activa (S/./dia)";
            ws.Cells[5, 5].Value = "Por Servicios Complementarios (S/./dia)";
            ws.Cells[5, 6].Value = "Por Inflexibilidades Operativas (S/./dia)";
            ws.Cells[5, 7].Value = "Por Exceso de Consumo de Energia Reactiva (S/./dia)";
            ws.Cells[5, 8].Value = "Monto total (S/.)";
            //
            ws.Cells[5, 10].Value = "Mes";
            ws.Cells[5, 11].Value = "Participante";
            ws.Cells[5, 12].Value = "Por Capacidad (S/./Mensual)";
            ws.Cells[5, 13].Value = "Por Peaje de Conexión (S/./Mensual)";
            ws.Cells[5, 14].Value = "Monto Total (S/.)";
            //
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 30;
            ws.Column(5).Width = 40;
            ws.Column(6).Width = 40;
            ws.Column(7).Width = 50;
            ws.Column(8).Width = 30;

            ws.Column(10).Width = 30;
            ws.Column(11).Width = 30;
            ws.Column(12).Width = 30;
            ws.Column(13).Width = 30;
            ws.Column(14).Width = 30;


            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Valofecha.ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.Emprnomb;
                    ws.Cells[row, column + 2].Value = (reg.Valdretiro - reg.Valdentrega);
                    ws.Cells[row, column + 3].Value = reg.Valdpagosc;
                    ws.Cells[row, column + 4].Value = reg.Valdpagoio;
                    ws.Cells[row, column + 5].Value = (reg.Valdcargoconsumo + reg.Valdaportesadicional);
                    ws.Cells[row, column + 6].Value = ((reg.Valdretiro - reg.Valdentrega) + reg.Valdpagosc + reg.Valdpagoio + (reg.Valdcargoconsumo + reg.Valdaportesadicional));
                    border = ws.Cells[row, 2, row, 8].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 8].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }

            int row2 = 6;
            int column2 = 10;
            if (listames != null)
            {
                foreach (var item in listames)
                {
                    ws.Cells[row2, column2].Value = item.Valofecha.ToString(Constantes.FormatoMesAnio);
                    ws.Cells[row2, column2 + 1].Value = item.Emprnomb;
                    ws.Cells[row2, column2 + 2].Value = ((item.Valddemandacoincidente - item.Valdpfirremun) / (1 + item.Valomr)) * item.Valopreciopotencia;
                    ws.Cells[row2, column2 + 3].Value = (item.Valddemandacoincidente * item.Valdpeajeuni);
                    ws.Cells[row2, column2 + 4].Value = ((((item.Valddemandacoincidente - item.Valdpfirremun) / (1 + item.Valomr)) * item.Valopreciopotencia) + (item.Valddemandacoincidente * item.Valdpeajeuni));
                    border = ws.Cells[row2, 10, row2, 14].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row2, 10, row2, 14].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row2++;
                }

            }
        }
        public static void GenerarArchivoVD(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelValorizacionDiaria);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteValorizacionDiaria);
            List<ValorizacionDiariaDTO> list = model.ValorizacionDiaria;
            List<ValorizacionDiariaDTO> listmes = model.ValorizacionDiariaMes;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteValorizacionDiaria);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Valorizacion Diaria");
                ws = xlPackage.Workbook.Worksheets["RPTE Valorizacion Diaria"];
                string titulo = "Reporte Valorizacion Diaria";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelVD(ws, list, listmes);
                xlPackage.Save();
            }
        }
        #endregion

        #region Reporte Informacion Prevista Remitida por el Participante
        public static void ConfiguracionHojaExcelInformacionPRP(ExcelWorksheet ws, List<MeMedicion96DTO> lista)
        {
            var fill = ws.Cells[5, 2, 5, 100].Style;
            fill.Fill.PatternType = ExcelFillStyle.Solid;
            fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
            var border = ws.Cells[5, 2, 5, 100].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 2].Value = "Fecha";
            ws.Cells[5, 3].Value = "Participante";
            ws.Cells[5, 4].Value = "Barra";
            ws.Cells[5, 5].Value = "00:15";
            ws.Cells[5, 6].Value = "00:30";
            ws.Cells[5, 7].Value = "00:45";
            ws.Cells[5, 8].Value = "01:00";
            ws.Cells[5, 9].Value = "01:15";
            ws.Cells[5, 10].Value = "01:30";
            ws.Cells[5, 11].Value = "01:45";
            ws.Cells[5, 12].Value = "02:00";
            ws.Cells[5, 13].Value = "02:15";
            ws.Cells[5, 14].Value = "02:30";
            ws.Cells[5, 15].Value = "02:45";
            ws.Cells[5, 16].Value = "03:00";
            ws.Cells[5, 17].Value = "03:15";
            ws.Cells[5, 18].Value = "03:30";
            ws.Cells[5, 19].Value = "03:45";
            ws.Cells[5, 20].Value = "04:00";
            ws.Cells[5, 21].Value = "04:15";
            ws.Cells[5, 22].Value = "04:30";
            ws.Cells[5, 23].Value = "04:45";
            ws.Cells[5, 24].Value = "05:00";
            ws.Cells[5, 25].Value = "05:15";
            ws.Cells[5, 26].Value = "05:30";
            ws.Cells[5, 27].Value = "05:45";
            ws.Cells[5, 28].Value = "06:00";
            ws.Cells[5, 29].Value = "06:15";
            ws.Cells[5, 30].Value = "06:30";
            ws.Cells[5, 31].Value = "06:45";
            ws.Cells[5, 32].Value = "07:00";
            ws.Cells[5, 33].Value = "07:15";
            ws.Cells[5, 34].Value = "07:30";
            ws.Cells[5, 35].Value = "07:45";
            ws.Cells[5, 36].Value = "08:00";
            ws.Cells[5, 37].Value = "08:15";
            ws.Cells[5, 38].Value = "08:30";
            ws.Cells[5, 39].Value = "08:45";
            ws.Cells[5, 40].Value = "09:00";
            ws.Cells[5, 41].Value = "09:15";
            ws.Cells[5, 42].Value = "09:30";
            ws.Cells[5, 43].Value = "09:45";
            ws.Cells[5, 44].Value = "10:00";
            ws.Cells[5, 45].Value = "10:15";
            ws.Cells[5, 46].Value = "10:30";
            ws.Cells[5, 47].Value = "10:45";
            ws.Cells[5, 48].Value = "11:00";
            ws.Cells[5, 49].Value = "11:15";
            ws.Cells[5, 50].Value = "11:30";
            ws.Cells[5, 51].Value = "11:45";
            ws.Cells[5, 52].Value = "12:00";
            ws.Cells[5, 53].Value = "12:15";
            ws.Cells[5, 54].Value = "12:30";
            ws.Cells[5, 55].Value = "12:45";
            ws.Cells[5, 56].Value = "13:00";
            ws.Cells[5, 57].Value = "13:15";
            ws.Cells[5, 58].Value = "13:30";
            ws.Cells[5, 59].Value = "13:45";
            ws.Cells[5, 60].Value = "14:00";
            ws.Cells[5, 61].Value = "14:15";
            ws.Cells[5, 62].Value = "14:30";
            ws.Cells[5, 63].Value = "14:45";
            ws.Cells[5, 64].Value = "15:00";
            ws.Cells[5, 65].Value = "15:15";
            ws.Cells[5, 66].Value = "15:30";
            ws.Cells[5, 67].Value = "15:45";
            ws.Cells[5, 68].Value = "16:00";
            ws.Cells[5, 69].Value = "16:15";
            ws.Cells[5, 70].Value = "16:30";
            ws.Cells[5, 71].Value = "16:45";
            ws.Cells[5, 72].Value = "17:00";
            ws.Cells[5, 73].Value = "17:15";
            ws.Cells[5, 74].Value = "17:30";
            ws.Cells[5, 75].Value = "17:45";
            ws.Cells[5, 76].Value = "18:00";
            ws.Cells[5, 77].Value = "18:15";
            ws.Cells[5, 78].Value = "18:30";
            ws.Cells[5, 79].Value = "18:45";
            ws.Cells[5, 80].Value = "19:00";
            ws.Cells[5, 81].Value = "19:15";
            ws.Cells[5, 82].Value = "19:30";
            ws.Cells[5, 83].Value = "19:45";
            ws.Cells[5, 84].Value = "20:00";
            ws.Cells[5, 85].Value = "20:15";
            ws.Cells[5, 86].Value = "20:30";
            ws.Cells[5, 87].Value = "20:45";
            ws.Cells[5, 88].Value = "21:00";
            ws.Cells[5, 89].Value = "21:15";
            ws.Cells[5, 90].Value = "21:30";
            ws.Cells[5, 91].Value = "21:45";
            ws.Cells[5, 92].Value = "22:00";
            ws.Cells[5, 93].Value = "22:15";
            ws.Cells[5, 94].Value = "22:30";
            ws.Cells[5, 95].Value = "22:45";
            ws.Cells[5, 96].Value = "23:00";
            ws.Cells[5, 97].Value = "23:15";
            ws.Cells[5, 98].Value = "23:30";
            ws.Cells[5, 99].Value = "23:45";
            ws.Cells[5, 100].Value = "00:00";
            ws.Column(1).Width = 5;
            for (int i = 2; i <= 100; i++)
            {
                ws.Column(i).Width = 30;
            }
            int row = 6;
            int column = 2;
            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = ((DateTime)reg.Medifecha).ToString(Constantes.FormatoFecha);
                    ws.Cells[row, column + 1].Value = reg.ClienteNomb;
                    ws.Cells[row, column + 2].Value = reg.BarrNomb;
                    ws.Cells[row, column + 3].Value = reg.H1;
                    ws.Cells[row, column + 4].Value = reg.H2;
                    ws.Cells[row, column + 5].Value = reg.H3;
                    ws.Cells[row, column + 6].Value = reg.H4;
                    ws.Cells[row, column + 7].Value = reg.H5;
                    ws.Cells[row, column + 8].Value = reg.H6;
                    ws.Cells[row, column + 9].Value = reg.H7;
                    ws.Cells[row, column + 10].Value = reg.H8;
                    ws.Cells[row, column + 11].Value = reg.H9;
                    ws.Cells[row, column + 12].Value = reg.H10;
                    ws.Cells[row, column + 13].Value = reg.H11;
                    ws.Cells[row, column + 14].Value = reg.H12;
                    ws.Cells[row, column + 15].Value = reg.H13;
                    ws.Cells[row, column + 16].Value = reg.H14;
                    ws.Cells[row, column + 17].Value = reg.H15;
                    ws.Cells[row, column + 18].Value = reg.H16;
                    ws.Cells[row, column + 19].Value = reg.H17;
                    ws.Cells[row, column + 20].Value = reg.H18;
                    ws.Cells[row, column + 21].Value = reg.H19;
                    ws.Cells[row, column + 22].Value = reg.H20;
                    ws.Cells[row, column + 23].Value = reg.H21;
                    ws.Cells[row, column + 24].Value = reg.H22;
                    ws.Cells[row, column + 25].Value = reg.H23;
                    ws.Cells[row, column + 26].Value = reg.H24;
                    ws.Cells[row, column + 27].Value = reg.H25;
                    ws.Cells[row, column + 28].Value = reg.H26;
                    ws.Cells[row, column + 29].Value = reg.H27;
                    ws.Cells[row, column + 30].Value = reg.H28;
                    ws.Cells[row, column + 31].Value = reg.H29;
                    ws.Cells[row, column + 32].Value = reg.H30;
                    ws.Cells[row, column + 33].Value = reg.H31;
                    ws.Cells[row, column + 34].Value = reg.H32;
                    ws.Cells[row, column + 35].Value = reg.H33;
                    ws.Cells[row, column + 36].Value = reg.H34;
                    ws.Cells[row, column + 37].Value = reg.H35;
                    ws.Cells[row, column + 38].Value = reg.H36;
                    ws.Cells[row, column + 39].Value = reg.H37;
                    ws.Cells[row, column + 40].Value = reg.H38;
                    ws.Cells[row, column + 41].Value = reg.H39;
                    ws.Cells[row, column + 42].Value = reg.H40;
                    ws.Cells[row, column + 43].Value = reg.H41;
                    ws.Cells[row, column + 44].Value = reg.H41;
                    ws.Cells[row, column + 45].Value = reg.H43;
                    ws.Cells[row, column + 46].Value = reg.H44;
                    ws.Cells[row, column + 47].Value = reg.H45;
                    ws.Cells[row, column + 48].Value = reg.H46;
                    ws.Cells[row, column + 49].Value = reg.H47;
                    ws.Cells[row, column + 50].Value = reg.H48;
                    ws.Cells[row, column + 51].Value = reg.H49;
                    ws.Cells[row, column + 52].Value = reg.H50;
                    ws.Cells[row, column + 53].Value = reg.H51;
                    ws.Cells[row, column + 54].Value = reg.H52;
                    ws.Cells[row, column + 55].Value = reg.H53;
                    ws.Cells[row, column + 56].Value = reg.H54;
                    ws.Cells[row, column + 57].Value = reg.H55;
                    ws.Cells[row, column + 58].Value = reg.H56;
                    ws.Cells[row, column + 59].Value = reg.H57;
                    ws.Cells[row, column + 60].Value = reg.H58;
                    ws.Cells[row, column + 61].Value = reg.H59;
                    ws.Cells[row, column + 62].Value = reg.H60;
                    ws.Cells[row, column + 63].Value = reg.H61;
                    ws.Cells[row, column + 64].Value = reg.H62;
                    ws.Cells[row, column + 65].Value = reg.H63;
                    ws.Cells[row, column + 66].Value = reg.H64;
                    ws.Cells[row, column + 67].Value = reg.H65;
                    ws.Cells[row, column + 68].Value = reg.H66;
                    ws.Cells[row, column + 69].Value = reg.H67;
                    ws.Cells[row, column + 70].Value = reg.H68;
                    ws.Cells[row, column + 71].Value = reg.H69;
                    ws.Cells[row, column + 72].Value = reg.H70;
                    ws.Cells[row, column + 73].Value = reg.H71;
                    ws.Cells[row, column + 74].Value = reg.H72;
                    ws.Cells[row, column + 75].Value = reg.H73;
                    ws.Cells[row, column + 76].Value = reg.H74;
                    ws.Cells[row, column + 77].Value = reg.H75;
                    ws.Cells[row, column + 78].Value = reg.H76;
                    ws.Cells[row, column + 79].Value = reg.H77;
                    ws.Cells[row, column + 80].Value = reg.H78;
                    ws.Cells[row, column + 81].Value = reg.H79;
                    ws.Cells[row, column + 82].Value = reg.H80;
                    ws.Cells[row, column + 83].Value = reg.H81;
                    ws.Cells[row, column + 84].Value = reg.H82;
                    ws.Cells[row, column + 85].Value = reg.H83;
                    ws.Cells[row, column + 86].Value = reg.H84;
                    ws.Cells[row, column + 87].Value = reg.H85;
                    ws.Cells[row, column + 88].Value = reg.H86;
                    ws.Cells[row, column + 89].Value = reg.H87;
                    ws.Cells[row, column + 90].Value = reg.H88;
                    ws.Cells[row, column + 91].Value = reg.H89;
                    ws.Cells[row, column + 92].Value = reg.H90;
                    ws.Cells[row, column + 93].Value = reg.H91;
                    ws.Cells[row, column + 94].Value = reg.H92;
                    ws.Cells[row, column + 95].Value = reg.H93;
                    ws.Cells[row, column + 96].Value = reg.H94;
                    ws.Cells[row, column + 97].Value = reg.H95;
                    ws.Cells[row, column + 98].Value = reg.H96;
                    border = ws.Cells[row, 2, row, 100].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    var fontTabla = ws.Cells[row, 2, row, 100].Style.Font;
                    fontTabla.Size = 8;
                    fontTabla.Name = "Calibri";
                    row++;
                }
            }
        }
        public static void GenerarArchivoInformacionPRP(ConsultasModel model, string ruta)
        {
            FileInfo template = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.PlantillaExcelReporteInformacionPrevista);
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista);
            List<MeMedicion96DTO> list = model.InformacionPrevista;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("RPTE Informacion Prevista");
                ws = xlPackage.Workbook.Worksheets["RPTE Informacion Prevista"];
                string titulo = "Reporte Informacion Prevista Remitida por el Participante";
                ConfigEncabezadoExcel(ws, titulo, ruta);
                ConfiguracionHojaExcelInformacionPRP(ws, list);
                xlPackage.Save();
            }
        }

        #endregion

        #region Informacion Prevista Remitida por el Participante

        /// <summary>
        /// Genera Archivo Excel en servidor del formato solicitado
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcel(FormatoModel model, string ruta)
        {
            //string fileTemplate = NombreArchivoHidro.PlantillaFormatoProgDiario;
            //FileInfo template = new FileInfo(ruta + fileTemplate);
            //FileInfo newFile = new FileInfo(ruta + NombreArchivo.FormatoProgDiario);
            string rutaLogo = ruta + Constantes.NombreLogoCoes;
            FileInfo newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Servicios.Aplicacion.ValorizacionDiaria.Helper.ConstantesValorizacionDiaria.NombreReporteInformacionPrevista);
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
                ws.Cells[1, ParamFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
                ws.Cells[1, ParamFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

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
                    case 6:
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

                for (var i = 0; i <= model.ListaHojaPto.Count; i++)
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
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
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
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
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
            //if (idEnvio > 0)
            //{
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
                               // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                            //model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][1] = model.Formato.FechaInicio.AddDays(7*z).ToString(Constantes.FormatoFecha);

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
                            if (tipoLectura == 1)
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
                    int semana = COES.Framework.Base.Tools.EPDate.f_numerosemana(fechaInicio.AddDays(horizonte * 7));
                    if (semana == 53)
                    {
                    }
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == 1)
                    {
                        resultado = fechaInicio.AddDays((horizonte + 1) * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        var fresultado = fechaInicio.AddDays((horizonte + 1) * 7);

                        if (semana >= 52)
                        {
                            resultado = (fresultado.AddDays(-7).Year).ToString() + " Sem:" + stSemana;
                        }
                        else
                        {
                            resultado = fresultado.Year.ToString() + " Sem:" + stSemana;
                        }

                    }
                    break;
                case ParametrosFormato.PeriodoDiario:
                    if (resolucion == ParametrosFormato.ResolucionHora)
                        resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice - 1)).ToString(Constantes.FormatoFechaHora);
                    else if (resolucion == ParametrosFormato.ResolucionCuartoHora)
                        resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(Constantes.FormatoFechaHora);
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

        #endregion
    }
}