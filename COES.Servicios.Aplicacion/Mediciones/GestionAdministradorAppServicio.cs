using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class GestionAdministradorAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReporteMedidoresAppServicio));

        #region Cumplimiento
        /// <summary>
        /// Genera el View de reportes de Cumplimiento.
        /// </summary>
        /// <returns></returns>
        public string GeneraViewCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, int idPeriodo)
        {
            StringBuilder strHtml = new StringBuilder();
            var listaEmpresa = sEmpresas.Split(',');
            var empresas = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato).Where(x => listaEmpresa.Contains(x.Emprcodi.ToString()));
            var listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(sEmpresas, idFormato, fInicio, fFin);
            var listaFecha = GetListaFecha(fInicio, fFin, idPeriodo);
            int cont = listaFecha.Count;

            strHtml.Append("<table border='1' width:'100%' class='pretty tabla-icono cell-border' cellspacing='0'  id='tabla'>");
            ///Cabecera de Reporte
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th width='300px'>EMPRESAS</th>");
            strHtml.Append("<th width='100px'>RUC</th>");
            string htmlCabecera = GetHtmlCabecera(fInicio, fFin, idPeriodo);
            strHtml.Append(htmlCabecera);
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            /// Fin de cabecera de Reporte
            strHtml.Append("<tbody>");
            string colorFondo = string.Empty;
            foreach (var emp in empresas)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + emp.Emprnomb + "</td>");
                strHtml.Append("<td>" + emp.Emprruc + "</td>");
                for (int i = 0; i < cont; i++)
                {
                    colorFondo = "style='background-color:orange;color:white'";
                    var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == listaFecha[i].Date);
                    if (find != null)
                    {
                        if (find.Envioplazo == "P")
                            colorFondo = "style='background-color:SteelBlue;color:white'";
                        strHtml.Append("<td " + colorFondo + " title='" + ((DateTime)find.Enviofecha).ToString("hh:mm:ss")
                            + "'>" + ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFechaHora) + "</td>");
                    }
                    else
                        strHtml.Append("<td >--</td>");
                }
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();

        }

        /// <summary>
        /// Devuelve string html de la cebecera de reportes de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public string GetHtmlCabecera(DateTime fInicio, DateTime fFin, int idPeriodo)
        {
            StringBuilder strHtml = new StringBuilder();
            switch (idPeriodo)
            {
                case 1:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(1))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                    }
                    break;
                case 2:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(7))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                    }
                    break;
                case 3:
                case 5:
                    for (var f = fInicio; f <= fFin; f = f.AddMonths(1))
                    {
                        strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                    }
                    break;
            }
            return strHtml.ToString();
        }


        /// <summary>
        /// Lista de cumplimiento de envios de hidrologia,
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="nombreFormato"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, string nombreFormato, int idPeriodo, string rutaArchivo,
            string rutaLogo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            var listaEmpresa = sEmpresas.Split(',');
            var empresas = FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato).Where(x => listaEmpresa.Contains(x.Emprcodi.ToString()));
            var listaEnvio = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(sEmpresas, idFormato, fInicio, fFin);
            var listaFecha = GetListaFecha(fInicio, fFin, idPeriodo);
            int cont = listaFecha.Count;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Cumplimiento");
                ws = xlPackage.Workbook.Worksheets["Cumplimiento"];
                AddImage(ws, 1, 0, rutaLogo);
                var fontTabla = ws.Cells[3, 2].Style.Font;
                fontTabla.Size = 14;
                fontTabla.Bold = true;
                ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
                ws.Cells[3, 3].Value = nombreFormato;
                GetExcelCabecera(ws, listaFecha, idPeriodo);
                int filLeyenda = empresas.Count() + 6;
                ws.Cells[filLeyenda + 1, 3].Value = "LEYENDA:";
                ws.Cells[filLeyenda + 2, 3].Value = "EN PLAZO";
                ws.Cells[filLeyenda + 3, 3].Value = "FUERA DE PLAZO";
                var borderLeyenda = ws.Cells[filLeyenda + 1, 3, filLeyenda + 3, 3].Style.Border;
                borderLeyenda.Bottom.Style = borderLeyenda.Top.Style = borderLeyenda.Left.Style = borderLeyenda.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[filLeyenda + 2, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[filLeyenda + 2, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                ws.Cells[filLeyenda + 3, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[filLeyenda + 3, 3].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                ws.Cells[filLeyenda + 2, 3].Style.Font.Color.SetColor(Color.White);
                ws.Cells[filLeyenda + 3, 3].Style.Font.Color.SetColor(Color.White);

                int fila = 6;
                int col = 2;
                int contador = 1;
                string colorFondo = string.Empty;
                foreach (var emp in empresas)
                {
                    ws.Cells[fila, col].Value = contador;
                    ws.Cells[fila, col+1].Value = emp.Emprnomb;
                    ws.Cells[fila , col + 2].Value = emp.Emprruc;

                    for (int i = 0; i < cont; i++)
                    {
                        colorFondo = "style='background-color:orange;color:white'";
                        var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == listaFecha[i].Date);
                        if (find != null)
                        {
                            ws.Cells[fila, col + i + 3].Value = ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFechaHora);
                            if (find.Envioplazo == "P")
                            {
                                ws.Cells[fila, col + i +3].StyleID = ws.Cells[filLeyenda + 2, 3].StyleID;
                            }
                            else
                            {
                                ws.Cells[fila, col + i + 3].StyleID = ws.Cells[filLeyenda + 3, 3].StyleID;
                            }
                        }
                    }
                    fila++;
                    contador++;
                }
                // borde de region de datos
                var borderReg = ws.Cells[5, 2, fila - 1, cont + 4].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[5, 2, 5, cont + 4])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));
                }
                using (ExcelRange r = ws.Cells[6, 3, fila - 1, 3])
                {
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(173, 216, 230));
                }
                //ws.Column(col).AutoFit();
                ExcelRange rg = ws.Cells[6, 1, 1 + fila, cont + 4];
                rg.AutoFitColumns();

                xlPackage.Save();
            }


        }

        private List<DateTime> GetListaFecha(DateTime fInicio, DateTime fFin, int idPeriodo)
        {
            List<DateTime> lista = new List<DateTime>();

            switch (idPeriodo)
            {
                case 1:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(1))
                    {
                        lista.Add(f);
                    }
                    break;
                case 2:

                    break;
                case 3:
                case 5:
                    for (var f = fInicio; f <= fFin; f = f.AddMonths(1))
                    {
                        lista.Add(f);
                    }

                    break;
            }

            return lista;
        }

        /// <summary>
        /// Obtiene Cabecera de reporte excel de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        private void GetExcelCabecera(ExcelWorksheet ws, List<DateTime> lista, int idPeriodo)
        {

            int col = 2;
            
            ws.Cells[5, col].Value = "N°";
            ws.Cells[5, col+1].Value = "Empresa/Fecha";
            ws.Cells[5, col+2].Value = "RUC";
            col++;
            for (var i = 0; i < lista.Count; i++)
            {
                DateTime f = lista[i];
                ws.Cells[5, col+2].Value = GetNombrePeriodo(f, idPeriodo);
                ws.Cells[5, col+2].AutoFitColumns();
                col++;
            }
        }

        #endregion

        #region Listado de Envio
        /// <summary>
        /// Genera Reporte Excevl de Envios formatos de Combustibles
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelEnvio(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, string rutaArchivo,
            string rutaLogo)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            var lista = FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, ConstantesAppServicio.ParametroDefecto,
                idsFormato, idsEstado, fechaIni, fechaFin);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Envío");
                ws = xlPackage.Workbook.Worksheets["Envío"];
                AddImage(ws, 1, 0, rutaLogo);
                ws.Cells[1, 3].Value = "REPORTE HISTORICO DE ENVÍOS";
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
                ws.Cells[3, 3].Value = DateTime.Now.ToString(ConstantesBase.FormatoFechaHora);
                var fill = ws.Cells[5, 2, 5, 13].Style;
                fill.Fill.PatternType = ExcelFillStyle.Solid;
                fill.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                fill.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                fill.Border.Bottom.Style = fill.Border.Top.Style = fill.Border.Left.Style = fill.Border.Right.Style = ExcelBorderStyle.Thin;
                var border = ws.Cells[5, 2, 5, 13].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[5, 2].Value = "N°";
                ws.Cells[5, 3].Value = "ID ENVÍO";
                ws.Cells[5, 4].Value = "FECHA PERIODO";
                ws.Cells[5, 5].Value = "EMPRESA";
                ws.Cells[5, 6].Value = "RUC";
                ws.Cells[5, 7].Value = "ESTADO";
                ws.Cells[5, 8].Value = "CUMPLIMIENTO";
                ws.Cells[5, 9].Value = "FECHA ENVÍO";
                ws.Cells[5, 10].Value = "FORMATO";
                ws.Cells[5, 11].Value = "USUARIO";
                ws.Cells[5, 12].Value = "CORREO";
                ws.Cells[5, 13].Value = "TELÉFONO";

                ws.Column(1).Width = 5;
                ws.Column(2).Width = 5;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 30;
                ws.Column(6).Width = 30;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 30;
                ws.Column(11).Width = 30;
                ws.Column(12).Width = 30;
                ws.Column(13).Width = 20;

                int row = 6;
                int column = 2;
                int contador = 1;
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        ws.Cells[row, column].Value = contador;
                        ws.Cells[row, column + 1].Value = reg.Enviocodi;
                        ws.Cells[row, column + 2].Value = reg.FechaPeriodo;
                        ws.Cells[row, column + 3].Value = reg.Emprnomb;
                        ws.Cells[row, column + 4].Value = reg.RucEmpresa;
                        ws.Cells[row, column + 5].Value = reg.Estenvnombre;
                        var eplazo = "";
                        if (reg.Envioplazo == "F")
                        {
                            eplazo = "Fuera de Plazo";
                        }
                        else
                        {
                            eplazo = "En Plazo";
                        }
                        ws.Cells[row, column + 6].Value = eplazo;
                        DateTime fechaenvio = (DateTime)reg.Enviofecha;
                        ws.Cells[row, column + 7].Value = fechaenvio.ToString(ConstantesBase.FormatoFechaHora); ;
                        ws.Cells[row, column + 8].Value = reg.Formatnombre;
                        ws.Cells[row, column + 9].Value = reg.Username;
                        ws.Cells[row, column + 10].Value = reg.Lastuser;
                        ws.Cells[row, column + 11].Value = reg.Usertlf;
                        border = ws.Cells[row, 2, row, 13].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        fontTabla = ws.Cells[row, 2, row, 13].Style.Font;
                        fontTabla.Size = 8;
                        fontTabla.Name = "Calibri";
                        row++;
                        contador++;
                    }
                }
                //ws.Column(column + 11).AutoFit();
                xlPackage.Save();
            }
        }
        #endregion

        #region util
        /// <summary>
        /// Inserta Imagen COES en Archivo Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filePath"></param>
        private void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(2); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(2);//Two pixel space for better alignment
                picture.SetSize(100, 40);

            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        /// <summary>
        /// Devuelve el nombre del periodo segun sea diario, semanal o mensual
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string GetNombrePeriodo(DateTime fecha, int periodo)
        {
            string fechaPeriodo = string.Empty;
            switch (periodo)
            {
                case 1:
                    fechaPeriodo = fecha.ToString(ConstantesBase.FormatoFecha);
                    break;
                case 2:
                    fechaPeriodo = fecha.Year.ToString() + " Sem " + EPDate.f_numerosemana(fecha);
                    break;
                case 3:
                case 5:
                    fechaPeriodo = fecha.Year.ToString() + " " + EPDate.f_NombreMes(fecha.Month);
                    break;
            }
            return fechaPeriodo;
        }
        #endregion
    }
}
