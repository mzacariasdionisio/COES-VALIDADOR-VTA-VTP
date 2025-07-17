using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.IEOD
{
    /// <summary>
    /// Clase que genera los reportes html y hojas excel
    /// </summary>
    public class UtilAnexoAPR5
    {
        #region Anexo A

        #region INFORMACIÓN GENERAL

        // 3.13.2.1.	Reporte de Eventos: fallas, interrupciones, restricciones y otros de carácter operativo.
        #region REPORTE_EVENTOS

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaInterrup"></param>
        /// <param name="listaInterrupVersion"></param>
        /// <returns></returns>
        public static string ReporteEventosHtml(DateTime fechaInicio, DateTime fechaFin, List<EventoDTO> data, List<EventoDTO> dataVersion,
                                List<EveInterrupcionDTO> listaInterrup, List<EveInterrupcionDTO> listaInterrupVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strHtmlInte = new StringBuilder();
            List<EveInterrupcionDTO> lstEveInte;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            string cabecer = string.Empty, cabecerDet = string.Empty;

            cabecerDet += "<table>";
            cabecerDet += "<thead>";
            cabecerDet += "<tr>";
            cabecerDet += "<th style='width:70px;'>PTO. SUMINISTRO</th>";
            cabecerDet += "<th style='width:10px;'>POTENCIA MW</th>";
            cabecerDet += "<th style='width:10px;'>TIEMPO</th>";
            cabecerDet += "</tr>";
            cabecerDet += "</thead>";
            cabecerDet += "<tbody>";

            cabecer += "<tr>";
            cabecer += "<th style='width:70px;' rowspan='2'>TIPO</th>";
            cabecer += "<th style='width:70px;' rowspan='2'>EMPRESA</th>";
            cabecer += "<th style='width:70px;' rowspan='2'>UBICACIÓN</th>";
            cabecer += "<th style='width:70px;' rowspan='2'>EQUIPO</th>";
            cabecer += "<th style='width:20px;' rowspan='2'>INICIO</th>";
            cabecer += "<th style='width:20px;' rowspan='2'>FINAL</th>";
            cabecer += "<th style='width:200px;' rowspan='2'>DESCRIPCIÓN</th>";
            cabecer += "<th style='width:200px;' rowspan='2'>OBSERVACIÓN</th>";
            cabecer += "</tr>";

            strHtml.Append("<table id='TablaEvento' border='0' class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append(cabecer);
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (data.Count > 0)
            {
                foreach (var obj in data)
                {
                    EventoDTO datVersi = dataVersion.Find(x => x.EVENCODI == obj.EVENCODI);

                    lstEveInte = listaInterrup.Where(x => x.Evencodi == Convert.ToInt32(obj.EVENCODI)).ToList();
                    strHtmlInte.Clear();
                    if (lstEveInte.Count > 0)
                    {
                        strHtmlInte.Append(cabecerDet);
                        decimal sum = 0;
                        foreach (var item in lstEveInte)
                        {
                            strHtmlInte.Append("<tr>");
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.PtoInterrupNomb);
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.Interrmw);
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.Interrminu);
                            strHtmlInte.Append("</tr>");
                            sum = sum + Convert.ToDecimal(item.Interrmw);
                        }
                        strHtmlInte.Append("<tr>");
                        strHtmlInte.AppendFormat("<td><b>{0}</b></td>", "TOTAL RECHAZADO");
                        strHtmlInte.AppendFormat("<td>{0}</td>", sum);
                        strHtmlInte.AppendFormat("<td>{0}</td>", "");
                        strHtmlInte.Append("</tr>");
                        strHtmlInte.Append("</tbody>");
                        strHtmlInte.Append("</table>");
                    }
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td>{0}</td>", obj.TIPOEVENABREV);
                    strHtml.AppendFormat("<td>{0}</td>", obj.EMPRNOMB);
                    strHtml.AppendFormat("<td>{0}</td>", obj.AREADESC);
                    strHtml.AppendFormat("<td>{0}</td>", obj.EQUIABREV);
                    strHtml.AppendFormat("<td>{0}</td>", obj.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    strHtml.AppendFormat("<td>{0}</td>", obj.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));

                    var _descrip = obj.EVENASUNTO;
                    string _bground = string.Empty;
                    if (datVersi != null) { if (_descrip != datVersi.EVENASUNTO) { _descrip = datVersi.EVENASUNTO; _bground = "lightgreen"; } }

                    var _descrip2 = obj.EVENDESC;
                    if (datVersi != null) { if (_descrip2 != datVersi.EVENDESC) { _descrip2 = datVersi.EVENDESC; _bground = "lightgreen"; } }

                    strHtml.AppendFormat("<td style='padding-left:5px;padding-right:5px;padding-bottom:5px;background:" + _bground + "'><b>{0}</b><br><p align='left'>{1}</p><br>{2}</td>", _descrip, _descrip2, strHtmlInte.ToString());

                    _descrip = obj.EVENCOMENTARIOS;
                    _bground = string.Empty;
                    if (datVersi != null) { if (_descrip != datVersi.EVENCOMENTARIOS) { _descrip = datVersi.EVENCOMENTARIOS; _bground = "lightgreen"; } }
                    strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", _descrip);

                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<tr><td colspan = '8'>No existen registros</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            #region Nuevos Registros

            foreach (var arr in data)
            {
                var dat = dataVersion.Find(x => x.EVENCODI == arr.EVENCODI);
                if (dat != null) { dataVersion.Remove(dat); }
            }

            if (dataVersion.Count > 0)
            {
                strHtml.Append("<div style='clear:both; height:30px'></div>");
                strHtml.Append("<table border='0' class='pretty tabla-icono'>");
                strHtml.Append("<thead>");
                strHtml.Append("<tr><th colspan='7'>Nuevos Eventos Generados despues de Generar IEOD</th></tr>");
                strHtml.Append(cabecer);
                strHtml.Append("</thead>");

                foreach (var list in dataVersion)
                {
                    lstEveInte = listaInterrup.Where(x => x.Evencodi == Convert.ToInt32(list.EVENCODI)).ToList();
                    strHtmlInte.Clear();
                    if (lstEveInte.Count > 0)
                    {
                        strHtmlInte.Append(cabecerDet);
                        decimal sum = 0;
                        foreach (var item in lstEveInte)
                        {
                            strHtmlInte.Append("<tr>");
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.PtoInterrupNomb);
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.Interrmw);
                            strHtmlInte.AppendFormat("<td>{0}</td>", item.Interrminu);
                            strHtmlInte.Append("</tr>");
                            sum = sum + Convert.ToDecimal(item.Interrmw);
                        }
                        strHtmlInte.Append("<tr>");
                        strHtmlInte.AppendFormat("<td><b>{0}</b></td>", "TOTAL RECHAZADO");
                        strHtmlInte.AppendFormat("<td>{0}</td>", sum);
                        strHtmlInte.AppendFormat("<td>{0}</td>", "");
                        strHtmlInte.Append("</tr>");
                        strHtmlInte.Append("</tbody>");
                        strHtmlInte.Append("</table>");
                    }
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td>{0}</td>", list.TIPOEVENABREV);
                    strHtml.AppendFormat("<td>{0}</td>", list.EMPRNOMB);
                    strHtml.AppendFormat("<td>{0}</td>", list.AREADESC);
                    strHtml.AppendFormat("<td>{0}</td>", list.EQUIABREV);
                    strHtml.AppendFormat("<td>{0}</td>", list.EVENINI);
                    strHtml.AppendFormat("<td>{0}</td>", list.EVENFIN);
                    strHtml.AppendFormat("<td><b>{0}</b><br><p align='left'>{1}</p><br>{2}</td>",
                        list.EVENASUNTO, list.EVENDESC, strHtmlInte.ToString());
                    strHtml.AppendFormat("<td>{0}</td>", list.EVENCOMENTARIOS);
                    strHtml.Append("</tr>");
                }
            }
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaEq"></param>
        /// <param name="listaEqVersion"></param>
        public static void GeneraRptEventos(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2,
                List<EventoDTO> lista, List<EventoDTO> listaVersion, List<EqEquipoDTO> listaEq, List<EqEquipoDTO> listaEqVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniTipoEvento = colIniNombreReporte;
            int colIniEmpresa = colIniTipoEvento + 1;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniTipoEq = colIniUbicacion + 1;
            int colIniEquipo = colIniTipoEq + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniDescripcion = colIniFinal + 1;
            int colIniMWIndisp = colIniDescripcion + 1;
            int colIniInterrup = colIniMWIndisp + 1;
            int colIniTension = colIniInterrup + 1;

            ws.Cells[rowIniCabecera, colIniTipoEvento].Value = "TIPO DE EVENTO";
            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniTipoEq].Value = "TIPO DE EQUIPO";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniCabecera, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniCabecera, colIniDescripcion].Value = "DESCRIPCIÓN";
            ws.Cells[rowIniCabecera, colIniMWIndisp].Value = "MW INDISP.";
            ws.Cells[rowIniCabecera, colIniInterrup].Value = "INTERRUPCIÓN (SI/NO)";
            ws.Cells[rowIniCabecera, colIniTension].Value = "TENSIÓN DE FALLA (kV)";

            //Nombre Reporte
            int colFinNombreReporte = colIniTension;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "INFORMACIÓN DE EVENTOS PRINCIPALES EN SUS INSTALACIONES";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniTipoEvento, rowIniCabecera, colIniTension, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    List<EqEquipoDTO> lstEq = listaEq.Where(x => x.Evencodi == list.EVENCODI).ToList();

                    if (lstEq.Count > 0)
                    {
                        foreach (var reg in lstEq)
                        {
                            ws.Cells[row, colIniTipoEvento].Value = list.TIPOEVENABREV;
                            ws.Cells[row, colIniEmpresa].Value = reg.EMPRNOMB;
                            ws.Cells[row, colIniUbicacion].Value = reg.AREANOMB;
                            ws.Cells[row, colIniTipoEq].Value = reg.Famnomb;
                            ws.Cells[row, colIniEquipo].Value = reg.Equiabrev;
                            ws.Cells[row, colIniInicio].Value = list.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                            ws.Cells[row, colIniFinal].Value = list.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                            ws.Cells[row, colIniDescripcion].Value = list.EVENASUNTO;
                            ws.Cells[row, colIniMWIndisp].Value = list.MWINTERRUMPIDOS > 0 ? (decimal?)list.MWINTERRUMPIDOS : null;
                            ws.Cells[row, colIniInterrup].Value = list.EVENINTERRUP;
                            ws.Cells[row, colIniTension].Value = list.TIPOEVENCODI == ConstantesPR5ReportesServicio.TipoevencodiFalla ? reg.Equitension : null;
                        }
                    }
                    else
                    {
                        ws.Cells[row, colIniTipoEvento].Value = list.TIPOEVENABREV;
                        ws.Cells[row, colIniEmpresa].Value = list.EMPRNOMB;
                        ws.Cells[row, colIniUbicacion].Value = list.AREANOMB;
                        ws.Cells[row, colIniTipoEq].Value = list.FAMNOMB;
                        ws.Cells[row, colIniEquipo].Value = list.EQUIABREV;
                        ws.Cells[row, colIniInicio].Value = list.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        ws.Cells[row, colIniFinal].Value = list.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        ws.Cells[row, colIniDescripcion].Value = list.EVENASUNTO;
                        ws.Cells[row, colIniMWIndisp].Value = list.MWINTERRUMPIDOS > 0 ? (decimal?)list.MWINTERRUMPIDOS : null;
                        ws.Cells[row, colIniInterrup].Value = list.EVENINTERRUP;
                        ws.Cells[row, colIniTension].Value = list.TIPOEVENCODI == ConstantesPR5ReportesServicio.TipoevencodiFalla ? list.EQUITENSION : null;
                    }

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniTipoEvento, rowFinData, colIniTension, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniTipoEvento, rowFinData, colIniTipoEvento, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniInicio, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniMWIndisp, rowFinData, colIniTension, "Centro");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniTipoEvento, rowFinData, colIniTension, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniTipoEvento, rowFinData, colIniTension, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniDescripcion, rowFinData, colIniDescripcion);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniTipoEvento, rowFinData, colIniTension);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniTipoEvento).Width = 18;
            ws.Column(colIniEmpresa).Width = 50;
            ws.Column(colIniUbicacion).Width = 30;
            ws.Column(colIniTipoEq).Width = 35;
            ws.Column(colIniEquipo).Width = 13;
            ws.Column(colIniInicio).Width = 21;
            ws.Column(colIniFinal).Width = 21;
            ws.Column(colIniDescripcion).Width = 100;
            ws.Column(colIniMWIndisp).Width = 18;
            ws.Column(colIniInterrup).Width = 18;
            ws.Column(colIniTension).Width = 18;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.2.	Reporte de las principales restricciones operativas y mantenimiento de las Unidades de Generación y de los equipos del Sistema de Transmisión.
        #region REPORTE_RESTRICCIONES_OPERATIVAS

        /// <summary>
        /// Genera vista del reporte de restricciones operativas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteRestriccionesOperativasHtml(DateTime fechaInicio, DateTime fechaFin, List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            string cabecer = string.Empty;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tablaRestric' class='pretty tabla-icono' style='table-layout: fixed; width: 2200px'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:250px;'>Empresa</th>");
            strHtml.Append("<th style='width:250px;'>Ubicación</th>");
            strHtml.Append("<th style='width:100px;'>T.Eq.</th>");
            strHtml.Append("<th style='width:100px;'>Equipo</th>");
            strHtml.Append("<th style='width:120px;'>Inicio</th>");
            strHtml.Append("<th style='width:120px;'>Final</th>");
            strHtml.Append("<th style='width:1100px;'>Descripción</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                var datVersi = dataVersion.Find(x => x.Iccodi == list.Iccodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Famabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Equiabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Ichorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                strHtml.AppendFormat("<td>{0}</td>", list.Ichorfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));

                var descrip = list.Icdescrip1;
                string _bground = string.Empty;
                if (datVersi != null) { if (list.Icdescrip1 != datVersi.Icdescrip1) { descrip = datVersi.Icdescrip1; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='padding-left: 6px; text-align: left !important; background:" + _bground + "'>{0}</td>", descrip);

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista del reporte de restricciones operativas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteMantenimentosHtml(DateTime fechaInicio, DateTime fechaFin, List<EveManttoDTO> data, List<EveManttoDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            string cabecer = string.Empty;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tablaMantto' class='pretty tabla-icono' style='table-layout: fixed; width: 2200px'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:150px;'>Empresa</th>");
            strHtml.Append("<th style='width:150px;'>Ubicación</th>");
            strHtml.Append("<th style='width:100px;'>Equipo</th>");
            strHtml.Append("<th style='width:100px;'>Inicio</th>");
            strHtml.Append("<th style='width:100px;'>Final</th>");
            strHtml.Append("<th style='width:400px;'>Descripción</th>");
            strHtml.Append("<th style='width: 70px;'>MW Indisp.</th>");
            strHtml.Append("<th style='width: 70px;'>Progr.</th>");
            strHtml.Append("<th style='width: 70px;'>Dispon</th>");
            strHtml.Append("<th style='width: 70px;'>Interrupc.</th>");
            strHtml.Append("<th style='width: 70px;'>Tipo</th>");
            strHtml.Append("<th style='width: 70px;'>CodEq</th>");
            strHtml.Append("<th style='width: 70px;'>TipoEq_Osinerg</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                var datVersi = dataVersion.Find(x => x.Manttocodi == list.Manttocodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Equiabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                strHtml.AppendFormat("<td>{0}</td>", list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));

                var descrip = list.Evendescrip;
                string _bground = string.Empty;
                if (datVersi != null) { if (list.Evendescrip != datVersi.Evendescrip) { descrip = datVersi.Evendescrip; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);

                strHtml.AppendFormat("<td>{0}</td>", list.Evenmwindisp);
                strHtml.AppendFormat("<td>{0}</td>", list.Eventipoprog);
                strHtml.AppendFormat("<td>{0}</td>", list.Evenindispo);
                strHtml.AppendFormat("<td>{0}</td>", list.Eveninterrup);
                strHtml.AppendFormat("<td>{0}</td>", list.Tipoevenabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Equicodi);
                strHtml.AppendFormat("<td>{0}</td>", list.Osigrupocodi);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            #region Nuevos Registros

            foreach (var arr in data)
            {
                var dat = dataVersion.Find(x => x.Manttocodi == arr.Manttocodi);
                if (dat != null) { dataVersion.Remove(dat); }
            }

            if (dataVersion.Count > 0)
            {
                strHtml.Append("<div style='clear:both; height:30px'></div>");
                strHtml.Append("<table id='tablaMantto2' class='pretty tabla-icono'>");
                strHtml.Append("<thead>");
                strHtml.Append("<tr><th colspan='7'>Nuevos Eventos Generados despues de Generar IEOD</th></tr>");
                strHtml.Append(cabecer);
                strHtml.Append("</thead>");

                foreach (var list in dataVersion)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equiabrev);
                    strHtml.AppendFormat("<td>{0}</td>", list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    strHtml.AppendFormat("<td>{0}</td>", list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    strHtml.AppendFormat("<td>{0}</td>", list.Evendescrip);
                    strHtml.AppendFormat("<td>{0}</td>", list.Evenmwindisp);
                    strHtml.AppendFormat("<td>{0}</td>", list.Eventipoprog);
                    strHtml.AppendFormat("<td>{0}</td>", list.Evenindispo);
                    strHtml.AppendFormat("<td>{0}</td>", list.Eveninterrup);
                    strHtml.AppendFormat("<td>{0}</td>", list.Tipoevenabrev);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equicodi);
                    strHtml.AppendFormat("<td>{0}</td>", list.Osigrupocodi);
                    strHtml.Append("</tr>");
                }

                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
            }

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptManttoEjecutado(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveManttoDTO> data, List<EveManttoDTO> dataVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniEmpresa = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniDescripcion = colIniFinal + 1;
            int colIniMWIndisp = colIniDescripcion + 1;
            int colIniProgr = colIniMWIndisp + 1;
            int colIniDispon = colIniProgr + 1;
            int colIniInterrup = colIniDispon + 1;
            int colIniTipo = colIniInterrup + 1;
            int colIniCodEq = colIniTipo + 1;
            int colIniTipoOsig = colIniCodEq + 1;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "Ubicación";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "Equipo";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "Inicio";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "Final";
            ws.Cells[rowIniEmpresa, colIniDescripcion].Value = "Descripción";
            ws.Cells[rowIniEmpresa, colIniMWIndisp].Value = "MW Indisp.";
            ws.Cells[rowIniEmpresa, colIniProgr].Value = "Prog.";
            ws.Cells[rowIniEmpresa, colIniDispon].Value = "Dispon";
            ws.Cells[rowIniEmpresa, colIniInterrup].Value = "Interrupc.";
            ws.Cells[rowIniEmpresa, colIniTipo].Value = "Tipo";
            ws.Cells[rowIniEmpresa, colIniCodEq].Value = "CodEq";
            ws.Cells[rowIniEmpresa, colIniTipoOsig].Value = "TipoEq_Osinerg";

            //Nombre Reporte
            int colFinNombreReporte = colIniDescripcion;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Mantenimiento Ejecutado";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniEmpresa + 1;
            #region cuerpo MANTENIMIENTO EJECUTADO

            List<EveManttoDTO> listaManttoEjec = data;
            if (listaManttoEjec.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in listaManttoEjec)
                {
                    ws.Cells[row, colIniEmpresa].Value = ReemplazarCaracteres(list.Emprnomb);
                    ws.Cells[row, colIniUbicacion].Value = ReemplazarCaracteres(list.Areanomb);
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    ws.Cells[row, colIniFinal].Value = list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    ws.Cells[row, colIniDescripcion].Value = ReemplazarCaracteres(list.Evendescrip);
                    ws.Cells[row, colIniMWIndisp].Value = list.Evenmwindisp;
                    ws.Cells[row, colIniProgr].Value = list.Eventipoprog;
                    ws.Cells[row, colIniDispon].Value = list.Evenindispo;
                    ws.Cells[row, colIniInterrup].Value = list.Eveninterrup;
                    ws.Cells[row, colIniTipo].Value = list.Tipoevenabrev;
                    ws.Cells[row, colIniCodEq].Value = list.Equicodi;
                    ws.Cells[row, colIniTipoOsig].Value = list.Osigrupocodi;

                    rowFinData = row;
                    row++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniInicio, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniMWIndisp, rowFinData, colIniMWIndisp, "Derecha");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniCodEq, rowFinData, colIniCodEq, "Derecha");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniDescripcion, rowFinData, colIniDescripcion);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig);

                #endregion
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniEmpresa).Height = 20;

            ws.Column(colIniEmpresa).Width = 30;
            ws.Column(colIniUbicacion).Width = 35;
            ws.Column(colIniEquipo).Width = 25;
            ws.Column(colIniInicio).Width = 16;
            ws.Column(colIniFinal).Width = 16;
            ws.Column(colIniDescripcion).Width = 70;
            ws.Column(colIniMWIndisp).Width = 13;
            ws.Column(colIniProgr).Width = 8;
            ws.Column(colIniDispon).Width = 8;
            ws.Column(colIniInterrup).Width = 11;
            ws.Column(colIniTipo).Width = 13;
            ws.Column(colIniCodEq).Width = 8;
            ws.Column(colIniTipoOsig).Width = 17;

            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptRestriccionesOperativas(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniEmpresa = rowIniNombreReporte + 1;
            int colIniFecha = colIniNombreReporte;
            int colIniInicio = colIniFecha + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniEmpresa = colIniFinal + 1;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniTeq = colIniUbicacion + 1;
            int colIniEquipo = colIniTeq + 1;
            int colIniDescripcion = colIniEquipo + 1;

            ws.Cells[rowIniEmpresa, colIniFecha].Value = "FECHA";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "HORA\nINICIO";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "HORA\nFINAL";
            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniEmpresa, colIniTeq].Value = "T.Eq.";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniEmpresa, colIniDescripcion].Value = "DESCRIPCIÓN";

            //Nombre Reporte
            int colFinNombreReporte = colIniDescripcion;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "RESTRICCIONES OPERATIVAS";

            #region Formato Cabecera

            UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniEmpresa, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, "Arial", 11);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte, "Arial", 11);
            UtilExcel.CeldasExcelWrapText(ws, rowIniEmpresa, colIniInicio, rowIniEmpresa, colIniFinal);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniEmpresa + 1;
            #region cuerpo

            if (data.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in data)
                {
                    string horaFinal = list.Ichorfin.Value.ToString(ConstantesAppServicio.FormatoHora);

                    ws.Cells[row, colIniFecha].Value = list.Ichorini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[row, colIniInicio].Value = list.Ichorini.Value.ToString(ConstantesAppServicio.FormatoHora);
                    ws.Cells[row, colIniFinal].Value = (horaFinal == "00:00" && list.Ichorfin.Value.Date == list.Ichorini.Value.Date.AddDays(1) ? "24:00" : horaFinal);
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniTeq].Value = list.Famabrev;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniDescripcion].Value = list.Icdescrip1;

                    rowFinData = row;
                    row++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFecha, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniDescripcion, "Izquierda");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniFecha, rowFinData, colIniDescripcion, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniFecha, rowFinData, colIniDescripcion, "Arial", 10);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniFecha, rowFinData, colIniDescripcion);

                #endregion
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniEmpresa).Height = 40;

            ws.Column(colIniFecha).Width = 13;
            ws.Column(colIniInicio).Width = 13;
            ws.Column(colIniFinal).Width = 13;
            ws.Column(colIniEmpresa).Width = 30;
            ws.Column(colIniUbicacion).Width = 35;
            ws.Column(colIniEquipo).Width = 25;
            ws.Column(colIniDescripcion).Width = 250;

            ws.View.FreezePanes(rowIniEmpresa + 1, 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// GetDescripcion2FechasMantto
        /// </summary>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <returns></returns>
        public static string GetDescripcion2FechasMantto(DateTime fini, DateTime ffin)
        {
            return string.Format("{0} a {1}", fini.ToString(ConstantesAppServicio.FormatoHora), ffin.ToString(ConstantesAppServicio.FormatoHora));
        }

        #endregion

        // 3.13.2.3.	Reporte de ingreso a operación comercial de unidades o centrales de generación, así como de la conexión e integración al SEIN de instalaciones de transmisión.
        #region  REPORTE_INGRESO_OPERACION_CI_SEIN

        /// <summary>
        /// Genera vista html de ingreso conexion integrción
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ReporteIngresoConexionIntegracionHtml(List<EveEventoEquipoDTO> data, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<div id='tab-container' class='tab-container'>");
            strHtml.Append("<ul id='tabss' class='etabs'>");
            strHtml.Append("<li id='tab_1' class='tab'><a href='#operacion'>Ing. Op Comercial</a></li>");
            strHtml.Append("<li id='tab_2' class='tab'><a href='#integrados'>Equipos Recien integrados</a></li>");
            strHtml.Append("<li id='tab_3' class='tab'><a href='#repotenciados'>Equipos Repotenciados</a></li>");
            strHtml.Append("<li id='tab_4' class='tab'><a href='#reubicados'>Equipos Reubicados</a></li></ul>");
            strHtml.Append("<div class='panel-container'>");
            strHtml.AppendFormat("<div id='operacion'>{0}</div>", ListaIngresoOperacionHtml(data, fechaInicio, fechaFin));
            strHtml.AppendFormat("<div id='integrados'>{0}</div>", ListaEquiposConexionSEINHtml(data, fechaInicio, fechaFin));
            strHtml.AppendFormat("<div id='repotenciados'>{0}</div>", ListaEquiposRepotenciadosHtml(data, fechaInicio, fechaFin));
            strHtml.AppendFormat("<div id='reubicados'>{0}</div>", ListaEquiposReubicadosHtml(data, fechaInicio, fechaFin));
            strHtml.Append("</div>");
            strHtml.Append("</div>");
            strHtml.Append("<script> $('#tab-container').easytabs({ animate: false });</script>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Genere vista html de ingreso a operación comercial
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ListaIngresoOperacionHtml(List<EveEventoEquipoDTO> data, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder strHtml = new StringBuilder();
            var listado = data.Where(x => x.Subcausacodi == ConstantesPR5ReportesServicio.SubCausaEquipoOperacionIng).OrderBy(x => x.Eeqfechaini).ThenBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equinomb).ToList();

            #region cabecera
            //*****************************CABECERA DE LA TABLA *******************************//

            strHtml.Append("<H3>INGRESO A OPERACIÓN COMERCIAL</H3>");
            strHtml.Append("<table class='pretty tabla-icono'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");

            strHtml.Append("<th style='width:70px;' rowspan='2'>Empresa</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>Tipo de Equipo</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>Ubicación</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>Equipo</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>Fecha y hora de Ingreso a <br>Operación Comercial</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #endregion
            if (listado.Count() > 0)
            {
                #region cuerpo
                foreach (var list in listado)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Famnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equinomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    strHtml.Append("</tr>");
                }
                #endregion
            }
            else
            {
                strHtml.Append("</tr><td colspan='6'>¡No existen datos para mostrar!</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// vista html de reporte equipos conexion a SEIN
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ListaEquiposConexionSEINHtml(List<EveEventoEquipoDTO> data, DateTime fechaInicio, DateTime fechaFin)
        {

            StringBuilder strHtml = new StringBuilder();
            string codigos = ConstantesPR5ReportesServicio.SubCausaEquiposIntegradosPrimeraconex;
            int[] result = new int[2];

            result = codigos.Split(',').Select(x => int.Parse(x)).ToArray();
            var listado = data.Where(x => result.Contains((int)x.Subcausacodi)).ToList();
            #region cabecera
            //*****************************CABECERA DE LA TABLA *******************************//

            strHtml.Append("<H3>EQUIPOS RECIÉN INTEGRADOS Y CON PRIMERA</H3>");
            strHtml.Append("<table class='pretty tabla-icono'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");

            strHtml.Append("<th style='width:70px;' rowspan='2'>EMPRESA</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>EQUIPO</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>FECHA  Y HORA DE INGRESO <br>EN OPERACION COMERCIAL</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>FECHA  Y HORA  <br> DE INTEGRACIÓN</th>");


            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");
            #endregion
            if (listado.Count() > 0)
            {
                #region cuerpo
                //*****************************CUERPO DE LA TABLA*************************************//



                foreach (var list in listado)
                {
                    strHtml.Append("<tr>");

                    strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equinomb);
                    if (list.Subcausacodi == 348) // primera conexion
                    {
                        strHtml.AppendFormat("<td>{0}</td>", list.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFecha));
                        strHtml.AppendFormat("<td>{0}</td>", "");
                    }
                    else //Recien integrado
                    {
                        strHtml.AppendFormat("<td>{0}</td>", "");
                        strHtml.AppendFormat("<td>{0}</td>", list.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFecha));
                    }
                    strHtml.Append("</tr>");

                }

                #endregion
            }
            else
            {
                strHtml.Append("</tr><td colspan = '5'>¡No existen datos para mostrar!</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// genere vista html de reporte equipos repotenciados
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ListaEquiposRepotenciadosHtml(List<EveEventoEquipoDTO> data, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder strHtml = new StringBuilder();
            var listado = data.Where(x => x.Subcausacodi == ConstantesPR5ReportesServicio.SubCausaEquipRepotenciados).ToList();
            #region cabecera
            //*****************************CABECERA DE LA TABLA *******************************//

            strHtml.Append("<H3>EQUIPOS REPOTENCIADOS</H3>");
            strHtml.Append("<table class='pretty tabla-icono'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");

            strHtml.Append("<th style='width:70px;' rowspan='2'>EMPRESA</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>EQUIPO</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>CAPACIDAD <br> ANTERIOR <br> (MW)</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>CAPACIDAD <br> ACTUAL <br> (MW)</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>FECHA Y HORA <br>DE EQUIPOS <br>REPOTENCIADOS</th>");

            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            #endregion
            if (listado.Count() > 0)
            {
                #region cuerpo
                //*****************************CUERPO DE LA TABLA*************************************//

                foreach (var list in listado)
                {
                    strHtml.Append("<tr>");

                    strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equinomb);
                    strHtml.AppendFormat("<td>{0}</td>", "");
                    strHtml.AppendFormat("<td>{0}</td>", "");
                    strHtml.AppendFormat("<td>{0}</td>", list.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFecha));

                    strHtml.Append("</tr>");

                }

                #endregion
            }
            else
            {
                strHtml.Append("</tr><td colspan = '5'>¡No existen datos para mostrar!</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// genera vista html de reporte equipos reubicados
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ListaEquiposReubicadosHtml(List<EveEventoEquipoDTO> data, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder strHtml = new StringBuilder();
            var listado = data.Where(x => x.Subcausacodi == ConstantesPR5ReportesServicio.SubCausaEquipReubicados).ToList();
            #region cabecera
            //*****************************CABECERA DE LA TABLA *******************************//

            strHtml.Append("<H3>EQUIPOS REUBICADOS</H3>");
            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>EMPRESA</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>EQUIPO</th>");
            strHtml.Append("<th style='width:70px;' rowspan='1'>DESDE</th>");
            strHtml.Append("<th style='width:70px;' rowspan='1'>HACIA</th>");
            strHtml.Append("<th style='width:70px;' rowspan='2'>FECHA Y HORA <br> EQUIPOS</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;' rowspan='1'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:70px;' rowspan='1'>UBICACIÓN(B)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            #endregion
            if (listado.Count() > 0)
            {
                #region cuerpo
                //*****************************CUERPO DE LA TABLA*************************************//

                foreach (var list in listado)
                {
                    strHtml.Append("<tr>");

                    strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Equinomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                    strHtml.AppendFormat("<td>{0}</td>", list.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFecha));

                    strHtml.Append("</tr>");
                }
                #endregion
            }
            else
            {
                strHtml.Append("</tr><td colspan = '5'>¡No existen datos para mostrar!</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptIngresoConexionIntegracion(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveEventoEquipoDTO> lista, List<EveEventoEquipoDTO> listaVersion)
        {
            lista = lista.Where(x => x.Subcausacodi == ConstantesPR5ReportesServicio.SubCausaEquipoOperacionIng).OrderBy(x => x.Eeqfechaini).ThenBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equinomb).ToList();

            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniTipoEq = colIniEmpresa + 1;
            int colIniUbicacion = colIniTipoEq + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;

            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniCabecera, colIniTipoEq].Value = "TIPO DE EQUIPO";
            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "Fecha y hora de Ingreso a Operación Comercial";

            //Nombre Reporte
            int colFinNombreReporte = colIniInicio;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "INGRESO A OPERACIÓN COMERCIAL";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniEmpresa, rowIniCabecera, colIniInicio, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var reg in lista)
                {
                    ws.Cells[row, colIniEmpresa].Value = reg.Emprnomb;
                    ws.Cells[row, colIniTipoEq].Value = reg.Famnomb;
                    ws.Cells[row, colIniUbicacion].Value = reg.Areanomb;
                    ws.Cells[row, colIniEquipo].Value = reg.Equinomb;
                    ws.Cells[row, colIniInicio].Value = reg.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFechaFull2);

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniEquipo, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniInicio, rowFinData, colIniInicio, "Centro");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniInicio, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colIniInicio, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniEmpresa, rowFinData, colIniInicio);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniEmpresa, rowFinData, colIniInicio);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniEmpresa).Width = 50;
            ws.Column(colIniTipoEq).Width = 35;
            ws.Column(colIniUbicacion).Width = 36;
            ws.Column(colIniEquipo).Width = 30;
            ws.Column(colIniInicio).Width = 30;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 100;
        }

        #endregion

        #endregion

        #region PRODUCCIÓN Y DEMANDA

        // 3.13.2.4.	Despacho registrado cada 30 minutos de las Unidades de Generación de los Integrantes del COES, asimismo, se incluye las Unidades de Generación con potencia superior a 5 MW conectadas al SEIN de empresas no Integrantes del COES (MW, MVAr).        
        #region DESPACHO_REGISTRADO

        ///
        public static string ReporteDespachoRegistradoHtml(List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto, int idPotencia, DateTime fechaIni, DateTime fechaFin,
                            List<MeMedicion48DTO> dataVersion, int tipoData48, List<string> listaMensaje)
        {
            switch (tipoData48)
            {
                case ConstantesPR5ReportesServicio.TipoData48PR5GruposDespacho:
                    return ReporteDespachoRegistradoHtml(true, data, listaPto, idPotencia, fechaIni, fechaFin, dataVersion, listaMensaje);
                case ConstantesPR5ReportesServicio.TipoData48PR5UnidadesGeneracion: //Extranet agentes
                    return ReporteDespachoRegistradoHtml(false, data, listaPto, idPotencia, fechaIni, fechaFin, dataVersion, listaMensaje);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Generación del reporte de Potencia en versión web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaPto"></param>
        /// <param name="idPotencia"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaMensaje"></param>
        /// <returns></returns>
        public static string ReporteDespachoRegistradoHtml(bool esGrupoDespacho, List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto, int idPotencia,
                                DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> dataVersion, List<string> listaMensaje)
        {
            List<MePtomedicionDTO> listaPtoOrdenado = new List<MePtomedicionDTO>();
            StringBuilder strHtml = new StringBuilder();

            //agregar mensajes de validación
            if (listaMensaje.Any())
            {
                strHtml.Append("<ul>");
                foreach (var item in listaMensaje)
                {
                    strHtml.AppendFormat("<li>{0}</li>", item);

                }
                strHtml.Append("</ul>");
            }

            string cabecer = string.Empty;

            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            string tipoinf = (idPotencia == 1) ? "MW" : "MVAR";
            string cabeceratipo = "<tr><th style='width: 101px;'>FECHA / HORA</th>";

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * (500 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            #region cabecera
            var listaTipoEmpresa = listaPto.Select(x => x.Grupointegrante).ToList().Distinct().ToList().OrderByDescending(x => x).ToList();

            //punto de medición, area operativa y tipo de generacion
            cabecer += "<tr>";
            cabecer += "<th style='width: 110px;'>CÓDIGO PUNTO</th>";

            if (listaTipoEmpresa.Count == 0)
            {
                strHtml.Append("<tr><th style='width: 400px;'>FECHA / HORA</th></tr><thead><tbody><tr><td>sin datos</td></tr></tbody>");
                return strHtml.ToString();
            }

            string cabecerGrupo = "<tr>";
            cabecerGrupo += "<th style='width: 110px;'>CÓDIGO GRUPO</th>";

            string cabecerAreaOp = "<tr>";
            cabecerAreaOp += "<th style='width: 110px;'>ÁREA</th>";

            string cabecerTipoGen = "<tr>";
            cabecerTipoGen += "<th style=''>TIPO GEN.</th>";

            string cabecerEquipo = "<tr>";
            cabecerEquipo += "<th style=''>EQUIPO</th>";

            for (int i = 0; i < listaTipoEmpresa.Count; i++)
            {
                var tipoEmpresa = listaTipoEmpresa[i];
                var classTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? "emp_coes" : "emp_no_coes";
                var descTotalTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? "TOTAL COES" : "TOTAL NO COES";

                var listaPtoDespacho = listaPto.Where(x => x.Grupointegrante == tipoEmpresa).ToList();

                for (int k = 0; k < listaPtoDespacho.Count; k++)
                {
                    var grupocodi = listaPtoDespacho[k].Grupocodi;
                    var gruponomb = listaPtoDespacho[k].Gruponomb;
                    var ptomedicodi = listaPtoDespacho[k].Ptomedicodi;
                    string areaOperativa = listaPtoDespacho[k].AreaOperativa;
                    string tipoGen = listaPtoDespacho[k].Tgenernomb;
                    string equinomb = listaPtoDespacho[k].Equinomb;

                    cabecer += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px' class='{1}'>{0}</th>", ptomedicodi, classTipoEmpresa);
                    cabecerGrupo += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px' class='{1}'>{0}</th>", grupocodi, classTipoEmpresa);
                    cabecerAreaOp += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px;{2}' class='{1}'>{0}</th>", areaOperativa, classTipoEmpresa, string.IsNullOrEmpty(areaOperativa) ? "background-color: #f54444 !important; color: white !important;" : string.Empty);
                    cabecerTipoGen += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px' class='{1}'>{0}</th>", tipoGen, classTipoEmpresa);
                    cabecerEquipo += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px' class='{1}'>{0}</th>", equinomb, classTipoEmpresa);
                }

                int totalFilaAgrup = esGrupoDespacho ? 6 : 7;
                cabecer += string.Format("<th style='width: 110px; word-wrap: break-word; white-space: normal' rowspan='{2}' class='{1}'>{0}</th>", descTotalTipoEmpresa, classTipoEmpresa, totalFilaAgrup);
                if (i < listaTipoEmpresa.Count - 1)
                {
                    cabecer += "<th class='separacion_tipo_empresa'></th>";
                    cabecerGrupo += "<th class='separacion_tipo_empresa'></th>";
                    cabecerAreaOp += "<th class='separacion_tipo_empresa'></th>";
                    cabecerTipoGen += "<th class='separacion_tipo_empresa'></th>";
                    cabecerEquipo += "<th class='separacion_tipo_empresa'></th>";
                }
            }

            cabecer += cabecerAreaOp + "</tr>";
            cabecer += cabecerTipoGen + "</tr>";

            //empresa
            cabecer += "<tr>";
            cabecer += "<th style='width: 110px;'>EMPRESA</th>";
            for (int i = 0; i < listaTipoEmpresa.Count; i++)
            {
                var tipoEmpresa = listaTipoEmpresa[i];
                var classTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? "emp_coes" : "emp_no_coes";
                var listaPtoDespacho = listaPto.Where(x => x.Grupointegrante == tipoEmpresa).ToList();

                for (int j = 0; j < listaPtoDespacho.Count; j++)
                {
                    var emprcodi = listaPtoDespacho[j].Emprcodi;
                    var empresa = listaPtoDespacho[j].Emprnomb;

                    cabecer += string.Format("<th style='word-wrap: break-word; white-space: normal;min-width:150px' class='{1}'>{0}</th>", empresa, classTipoEmpresa);
                }

                if (i < listaTipoEmpresa.Count - 1)
                {
                    cabecer += "<th class='separacion_tipo_empresa'></th>";
                }
            }
            cabecer += "</tr>";

            //Grupo despacho
            cabecer += "<tr>";
            cabecer += "<th>GRUPO</th>";
            for (int i = 0; i < listaTipoEmpresa.Count; i++)
            {
                var tipoEmpresa = listaTipoEmpresa[i];
                var classTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? "emp_coes" : "emp_no_coes";

                var listaPtoDespacho = listaPto.Where(x => x.Grupointegrante == tipoEmpresa).ToList();

                for (int k = 0; k < listaPtoDespacho.Count; k++)
                {
                    var grupocodi = listaPtoDespacho[k].Grupocodi;
                    var gruponomb = listaPtoDespacho[k].Gruponomb;
                    var ptomedicodi = listaPtoDespacho[k].Ptomedicodi;
                    var emprcodi = listaPtoDespacho[k].Emprcodi;
                    var empresa = listaPtoDespacho[k].Emprnomb;

                    cabecer += string.Format("<th style='word-wrap: break-word; white-space: normal;width: 150px' class='{1}'>{0}</th>", gruponomb, classTipoEmpresa);
                    cabeceratipo = cabeceratipo + string.Format("<th class='{1}'>{0}</th>", tipoinf, classTipoEmpresa);

                    listaPtoOrdenado.Add(listaPtoDespacho[k]);
                }

                cabeceratipo += string.Format("<th class='{1}'>{0}</th>", tipoinf, classTipoEmpresa);
                if (i < listaTipoEmpresa.Count - 1)
                {
                    cabecer += "<th class='separacion_tipo_empresa'></th>";
                    cabeceratipo += "<th class='separacion_tipo_empresa'></th>";
                }
            }
            cabecer += "</tr>";
            if (!esGrupoDespacho) cabecer += cabecerEquipo + "</tr>";
            cabecer += cabecerGrupo + "</tr>";
            cabeceratipo = cabeceratipo + "</tr>";
            cabecer += cabeceratipo;
            strHtml.Append(cabecer);
            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            List<MeMedicion48DTO> listaAcumulado = new List<MeMedicion48DTO>();
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                for (int i = 0; i < listaTipoEmpresa.Count; i++)
                {
                    MeMedicion48DTO m48 = new MeMedicion48DTO();
                    m48.Grupointegrante = listaTipoEmpresa[i];
                    m48.Medifecha = day;
                    listaAcumulado.Add(m48);
                }
            }

            decimal? valor;
            decimal total;

            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);
                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    for (int i = 0; i < listaTipoEmpresa.Count; i++)
                    {
                        decimal? valorVersi = null;
                        var tipoEmpresa = listaTipoEmpresa[i];
                        var regAcum = listaAcumulado.Where(x => x.Grupointegrante == tipoEmpresa && x.Medifecha == day).First();
                        total = ((decimal?)regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regAcum, null)).GetValueOrDefault(0);

                        var listaDataTipoEmpresa = listaPtoOrdenado.Where(x => x.Grupointegrante == tipoEmpresa).ToList();
                        foreach (var pto48 in listaDataTipoEmpresa)
                        {
                            MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Emprcodi == pto48.Emprcodi && x.Grupointegrante == tipoEmpresa);

                            var datVersi = dataVersion.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Emprcodi == pto48.Emprcodi && x.Grupointegrante == tipoEmpresa);
                            string _bground = string.Empty, descrip = string.Empty;

                            valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                            if (datVersi != null)
                            {
                                valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(datVersi, null);
                            }
                            if (valor != null)
                            {
                                descrip = ((decimal)valor).ToString("N", nfi);
                                if (valorVersi != null)
                                {
                                    if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                                }
                                total += valor.GetValueOrDefault(0);
                            }
                            if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; total += valorVersi.Value; }
                            strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);
                        }
                        strHtml.AppendFormat("<td>{0}</td>", total.ToString("N", nfi));

                        if (i < listaTipoEmpresa.Count - 1)
                        {
                            strHtml.Append("<td class='separacion_tipo_empresa'></td>");
                        }

                        regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regAcum, total);
                    }


                    strHtml.Append("</tr>");

                    horas = horas.AddMinutes(30);
                }
            }

            //EJECUTADO
            foreach (var m48 in listaAcumulado)
            {
                valor = 0;
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor != null ? valor.Value : 0;
                }

                m48.Meditotal = total;
            }

            strHtml.Append("<tr>");
            strHtml.Append("<td class='total_potencia_activa_ejecutado'>EJEC</td>");


            for (int i = 0; i < listaTipoEmpresa.Count; i++)
            {
                var tipoEmpresa = listaTipoEmpresa[i];
                var regAcum = listaAcumulado.Where(x => x.Grupointegrante == tipoEmpresa).ToList();

                var listaDataTipoEmpresa = listaPtoOrdenado.Where(x => x.Grupointegrante == tipoEmpresa).ToList();
                foreach (var pto48 in listaDataTipoEmpresa)
                {
                    List<MeMedicion48DTO> lista48 = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Grupointegrante == tipoEmpresa).ToList();
                    decimal totalPto = lista48.Sum(x => x.Meditotal.GetValueOrDefault(0));
                    strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", (totalPto / 2).ToString("N", nfi));
                }
                var totalTipoEmpresa = regAcum.Sum(x => x.Meditotal.GetValueOrDefault(0));
                strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", (totalTipoEmpresa / 2).ToString("N", nfi));

                if (i < listaTipoEmpresa.Count - 1)
                {
                    strHtml.Append("<td class='separacion_tipo_empresa'></td>");
                }
            }
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Generación del reporte de Potencia en versión web
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        /// <param name="tipoData48"></param>
        public static void GeneraRptDespachoRegistrado(ExcelWorksheet ws, int rowIni, int colIni, bool flagVisiblePtomedicodi, int tipoinfocodi, DateTime fecha1, DateTime fecha2
            , List<MeMedicion48DTO> lista, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion, int tipoData48)
        {
            switch (tipoData48)
            {
                case ConstantesPR5ReportesServicio.TipoData48PR5GruposDespacho:
                    GeneraRptDespachoRegistrado(ws, rowIni, colIni, true, flagVisiblePtomedicodi, tipoinfocodi, fecha1, fecha2, lista, listaVersion, listaPto, listaPtoVersion);
                    break;
                case ConstantesPR5ReportesServicio.TipoData48PR5UnidadesGeneracion:
                    GeneraRptDespachoRegistrado(ws, rowIni, colIni, false, flagVisiblePtomedicodi, tipoinfocodi, fecha1, fecha2, lista, listaVersion, listaPto, listaPtoVersion);
                    break;
            }
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        public static void GeneraRptDespachoRegistrado(ExcelWorksheet ws, int rowIni, int colIni, bool esGrupoDespacho, bool flagVisiblePtomedicodi, int tipoinfocodi, DateTime fecha1, DateTime fecha2
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            List<MePtomedicionDTO> listaPtoOrdenada = new List<MePtomedicionDTO>();
            List<MeMedicion48DTO> listaAcumulado = new List<MeMedicion48DTO>();

            string tipoinf = (tipoinfocodi == 1) ? "MW" : "MVAR";
            int row = rowIni + 1;
            int col = colIni;

            #region cabecera

            int rowIniTipoEmpresa = rowIni;
            // Fila Hora - Empresa - Total

            int colIniFecha = col;
            int rowIniFecha = row;
            int rowFinFecha = rowIniFecha + 5 - 1;
            if (!esGrupoDespacho) rowFinFecha = rowFinFecha + 1;
            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int colIniHora = colIniFecha + 1;
            int rowIniHora = rowIniFecha;
            int rowFinHora = rowFinFecha;
            ws.Cells[rowIniHora, colIniHora].Value = "HORA";
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Merge = true;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.WrapText = true;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int rowIniPto = row;
            int colIniPto = colIniHora + 1;
            int colFinPto = colIniPto;

            int rowIniArea = rowIniPto + 1;
            int colIniArea = colIniHora + 1;
            int colFinArea = colIniArea;

            int rowIniGen = rowIniArea + 1;
            int colIniGen = colIniHora + 1;
            int colFinGen = colIniGen;

            int rowIniEmp = rowIniGen + 1;
            int colIniEmp = colIniHora + 1;
            int colFinEmp = colIniEmp;

            int rowIniGrupoDespacho = rowIniEmp + 1;
            int colIniGrupoDespacho = colIniHora + 1;
            int colFinGrupoDespacho = colIniGrupoDespacho;

            int rowIniEq = rowIniGrupoDespacho + (esGrupoDespacho ? 0 : 1);
            int colIniEq = colIniHora + 1;
            int colFinEq = colIniEq;

            var listaTipoEmpresa = listaPto.Select(x => x.Grupointegrante).ToList().Distinct().ToList().OrderByDescending(x => x).ToList();

            for (int t = 0; t < listaTipoEmpresa.Count; t++)
            {
                int colIniTipoEmpresa = colIniEmp;

                var tipoEmpresa = listaTipoEmpresa[t];
                var classTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? ConstantesPR5ReportesServicio.ColorInfSGI : "#8EA9DB";
                var colorTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? Color.White : Color.Black;
                var colorBorder = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? Color.White : Color.Blue;
                var descTotalTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? "TOTAL COES" : "TOTAL NO COES";
                var descTipoEmpresa = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? ((tipoinfocodi == 1) ? "POTENCIA ACTIVA EJECUTADA DE LAS UNIDADES DE GENERACIÓN DEL SEIN (MW)" : "POTENCIA REACTIVA EJECUTADA DE LAS UNIDADES DE GENERACIÓN DEL COES (MVAR)") : "CENTRALES NO INTEGRANTES COES";

                //Grupos Despacho
                var listaGrupoDespacho = listaPto.Where(x => x.Grupointegrante == tipoEmpresa).ToList();

                for (int k = 0; k < listaGrupoDespacho.Count; k++)
                {
                    var thGrupoDespacho = listaGrupoDespacho[k];

                    colFinGrupoDespacho = colIniGrupoDespacho;

                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Value = thGrupoDespacho.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Style.Font.Size = 9;
                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Value = thGrupoDespacho.Gruponomb.Trim();
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.Font.Size = 8;
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.WrapText = true;
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowIniPto, colIniGrupoDespacho].Value = (thGrupoDespacho.Ptomedicodi > 0 ? thGrupoDespacho.Ptomedicodi : thGrupoDespacho.Grupocodi);
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Style.Font.Size = 8;
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Style.WrapText = true;
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowIniArea, colIniGrupoDespacho].Value = (thGrupoDespacho.AreaOperativa ?? "");
                    ws.Cells[rowIniArea, colIniGrupoDespacho].Style.Font.Size = 8;
                    ws.Cells[rowIniArea, colIniGrupoDespacho].Style.WrapText = true;
                    ws.Cells[rowIniArea, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniArea, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniArea, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowIniGen, colIniGrupoDespacho].Value = (thGrupoDespacho.Tgenernomb ?? "");
                    ws.Cells[rowIniGen, colIniGrupoDespacho].Style.Font.Size = 8;
                    ws.Cells[rowIniGen, colIniGrupoDespacho].Style.WrapText = true;
                    ws.Cells[rowIniGen, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniGen, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniGen, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    if (!esGrupoDespacho)
                    {
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Value = thGrupoDespacho.Equinomb;
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Style.Font.Size = 8;
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Style.WrapText = true;
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEq, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    colIniGrupoDespacho = colFinGrupoDespacho + 1;

                    listaPtoOrdenada.Add(thGrupoDespacho);
                }

                int colIniTotal = colFinGrupoDespacho + 1;
                int colFinTotal = colIniTotal;
                int rowIniTotal = rowIniPto;
                int rowFinTotal = rowIniEq;

                //Descripcion Total - medida
                ws.Cells[rowIniTotal, colIniTotal].Value = descTotalTipoEmpresa;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Merge = true;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.WrapText = true;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowFinTotal, colIniTotal].Value = tipoinf;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Merge = true;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.WrapText = true;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //Nombre Tipo empresa
                colIniTipoEmpresa = t == 0 ? colIniTipoEmpresa - 2 : colIniTipoEmpresa;
                int colFinTipoEmpresa = colFinTotal;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa].Value = descTipoEmpresa;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa].Style.Font.Size = 18;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniTipoEmpresa, colFinTipoEmpresa].Merge = true;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniTipoEmpresa, colFinTipoEmpresa].Style.WrapText = true;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniTipoEmpresa, colFinTipoEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniTipoEmpresa, colFinTipoEmpresa].Style.HorizontalAlignment = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoEmpresa ? ExcelHorizontalAlignment.Left : ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniTipoEmpresa, colFinTipoEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                using (var range = ws.Cells[rowIniTipoEmpresa, colIniTipoEmpresa, rowIniEq, colFinTipoEmpresa])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(colorTipoEmpresa);
                }

                if (t < listaTipoEmpresa.Count - 1)
                {
                    colIniEmp = colFinTotal + 2;
                    colIniGrupoDespacho = colFinTotal + 2;
                    colIniTipoEmpresa = colFinTotal + 2;
                }
            }

            #endregion

            int rowIniData = rowIniEq + 1;
            row = rowIniData;

            #region cuerpo

            for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
            {
                for (int i = 0; i < listaTipoEmpresa.Count; i++)
                {
                    MeMedicion48DTO m48 = new MeMedicion48DTO();
                    m48.Grupointegrante = listaTipoEmpresa[i];
                    m48.Medifecha = day;
                    listaAcumulado.Add(m48);
                }
            }

            decimal? valor;
            decimal total;
            int numDia = 0;

            int colData = colIniHora;
            for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
            {
                numDia++;

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    ws.Row(row).Height = 24;

                    //Fecha
                    ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                    ws.Cells[row, colIniFecha].Style.Font.Size = 10;
                    ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);

                    colData = colIniHora;
                    //Hora
                    ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    ws.Cells[row, colIniHora].Style.Font.Bold = true;
                    ws.Cells[row, colIniHora].Style.Font.Size = 10;
                    ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

                    colData++;
                    for (int i = 0; i < listaTipoEmpresa.Count; i++)
                    {
                        var tipoEmpresa = listaTipoEmpresa[i];
                        var regAcum = listaAcumulado.Where(x => x.Grupointegrante == tipoEmpresa && x.Medifecha == day).First();
                        total = ((decimal?)regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regAcum, null)).GetValueOrDefault(0);

                        var listaDataTipoEmpresa = listaPtoOrdenada.Where(x => x.Grupointegrante == tipoEmpresa).ToList();
                        foreach (var pto48 in listaDataTipoEmpresa)
                        {
                            MeMedicion48DTO m48 = data.Find(x => x.Grupointegrante == tipoEmpresa && x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Emprcodi == pto48.Emprcodi);
                            valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                            ws.Cells[row, colData].Value = valor;
                            colData++;

                            total += valor.GetValueOrDefault(0);
                        }

                        regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regAcum, total);
                        ws.Cells[row, colData].Value = total;
                        colData++;

                        if (i < listaTipoEmpresa.Count - 1)
                        {
                            colData++;
                        }
                    }

                    horas = horas.AddMinutes(30);
                    row++;
                }
            }

            //EJECUTADO
            foreach (var m48 in listaAcumulado)
            {
                valor = 0;
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor != null ? valor.Value : 0;
                }

                m48.Meditotal = total;
            }

            int rowEjec = row;
            ws.Cells[rowEjec, colIniFecha].Value = "EJEC";
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Merge = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Bold = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Size = 12;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.WrapText = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

            //Formatear data
            colData = colIniHora + 1;
            for (int i = 0; i < listaTipoEmpresa.Count; i++)
            {
                int colIniEjecTipoEmpresa = i == 0 ? colData - 2 : colData;
                var tipoEmpresa = listaTipoEmpresa[i];
                var regAcum = listaAcumulado.Where(x => x.Grupointegrante == tipoEmpresa).ToList();
                var totalTipoEmpresa = regAcum.Sum(x => x.Meditotal.GetValueOrDefault(0));

                var listaDataTipoEmpresa = listaPtoOrdenada.Where(x => x.Grupointegrante == tipoEmpresa).ToList();
                foreach (var pto48 in listaDataTipoEmpresa)
                {
                    List<MeMedicion48DTO> lista48 = data.Where(x => x.Grupointegrante == tipoEmpresa && x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi).ToList();
                    decimal totalPto = lista48.Sum(x => x.Meditotal.GetValueOrDefault(0));

                    ws.Cells[row, colData].Value = totalPto / 2;
                    ws.Cells[row, colData].Style.Font.Bold = true;
                    ws.Cells[row, colData].Style.Font.Size = 12;
                    ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6DCE4"));
                    ws.Cells[row, colData].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    ws.Cells[row, colData].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                    ws.Cells[row, colData].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    ws.Cells[row, colData].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                    ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colData].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[row, colData].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[row, colData].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[row, colData].Style.Border.Right.Color.SetColor(Color.Blue);
                    colData++;
                }

                ws.Cells[row, colData].Value = totalTipoEmpresa / 2;
                ws.Cells[row, colData].Style.Font.Bold = true;
                ws.Cells[row, colData].Style.Font.Size = 12;
                ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells[row, colData].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, colData].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                ws.Cells[row, colData].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, colData].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, colData].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[row, colData].Style.Border.Left.Color.SetColor(Color.Blue);
                ws.Cells[row, colData].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[row, colData].Style.Border.Right.Color.SetColor(Color.Blue);
                colData++;

                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 48, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Numberformat.Format = "#,##0.00";
                    range.Style.Font.Size = 10;
                }
                using (var range = ws.Cells[rowIniData + numDia * 48, colIniHora + 1, rowIniData + numDia * 48, colData - 1])
                {
                    range.Style.Font.Size = 12;
                }

                //mostrar lineas horas
                for (int c = colIniEjecTipoEmpresa; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                        ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                    }
                }

                if (i < listaTipoEmpresa.Count - 1)
                {
                    colData++;
                }
            }

            //Formato de Filas y columnas
            for (int columna = colIniHora + 1; columna < colData; columna++)
                ws.Column(columna).Width = 20;

            ws.Column(colIniFecha).Width = 11;
            ws.Column(colIniHora).Width = 9;
            ws.Row(rowIniTipoEmpresa).Height = 30;
            ws.Row(rowIniPto).Height = 22;
            ws.Row(rowIniEmp).Height = 40;
            ws.Row(rowIniGrupoDespacho).Height = 40;
            ws.Row(rowIniEq).Height = 40;
            ws.Row(rowEjec).Height = 25;

            if (!flagVisiblePtomedicodi)
            {
                ws.Row(rowIniPto).Hidden = true;
                ws.Row(rowIniArea).Hidden = true;
                ws.Row(rowIniGen).Hidden = true;
            }

            #endregion

            ws.View.FreezePanes(rowFinFecha + 1, colIniHora + 1);
            ws.View.ZoomScale = 70;
        }

        /// <summary>
        /// CalcularFactordeCarga
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal CalcularFactordeCarga(MeMedicion48DTO obj)
        {
            decimal factorCarga = 0;
            decimal totalEjecutado = 0;

            for (int i = 1; i <= 48; i++)
            {
                totalEjecutado = totalEjecutado + ((decimal?)obj.GetType().GetProperty("H" + i).GetValue(obj, null)).GetValueOrDefault();
            }

            totalEjecutado = totalEjecutado / 2;
            // Hallar el máximo
            decimal maxEjecutadoHora = 0;
            for (int i = 1; i <= 48; i++)
            {
                var valor = ((decimal?)obj.GetType().GetProperty("H" + i).GetValue(obj, null)).GetValueOrDefault(0);
                if (valor > maxEjecutadoHora)
                { maxEjecutadoHora = valor; }
            }
            // Hallar factor de carga
            if (maxEjecutadoHora > 0)
                factorCarga = (totalEjecutado / maxEjecutadoHora) / 24.0m;

            return factorCarga;
        }

        /// <summary>
        /// CalcularPendienteMax
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="obj"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="pendienteMax"></param>
        /// <param name="fechaHoraMax"></param>
        public static void CalcularPendienteMax(DateTime fecha, MeMedicion48DTO obj, List<SiParametroValorDTO> listaBloqueHorario, out decimal pendienteMax, out DateTime fechaHoraMax)
        {
            //total cada 30min
            decimal[] arrayTotal = new decimal[48];
            for (int i = 1; i <= 48; i++)
            {
                arrayTotal[i - 1] = ((decimal?)obj.GetType().GetProperty("H" + i).GetValue(obj, null)).GetValueOrDefault(0);
            }

            //Pendiente cada 30min
            decimal[] arrayPendiente = new decimal[48];
            MeMedicion48DTO objPendiente = new MeMedicion48DTO() { Medifecha = fecha};
            for (int i = 0; i < 48 - 1; i++)
            {
                arrayPendiente[i] = (arrayTotal[i + 1] - arrayTotal[i]) / 30.0m;
                objPendiente.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i + 1)).SetValue(objPendiente, arrayPendiente[i]);
            }

            //Valor máximo de la pendiente en hora punta
            MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoHoraPunta, fecha, new List<MeMedicion48DTO>() { objPendiente }, null, listaBloqueHorario,
                                                            out decimal valorResultado, out int hResultado, out DateTime fechaHora);

            //salidas
            pendienteMax = valorResultado;
            fechaHoraMax = fecha.Date.AddMinutes(hResultado * 30);
        }

        /// <summary>
        /// GetGraficoDemandaEjecyProg
        /// </summary>
        /// <param name="listaDemanda48"></param>
        /// <param name="listaReprogramas"></param>
        /// <param name="incluyeEcuador"></param>
        /// <param name="listaHDemanda"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoDemandaEjecyProg(List<MeMedicion48DTO> listaDemanda48, List<MeMedicion48DTO> listaReprogramas, bool incluyeEcuador, List<int> listaHDemanda)
        {
            List<string> listaSerie = new List<string>();
            List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();
            string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H","I","J", "K", "L", "M",
                                "N", "O", "p", "R", "S", "T", "U", "V", "W", "X", "Y","Z"};

            if (incluyeEcuador)
            {
                //etiquetas
                listaSerie.Add("EJECUTADO (CON ECUADOR)");
                listaSerie.Add("EJECUTADO (SIN ECUADOR)");
                listaSerie.Add("PROGRAMA (SIN ECUADOR)");

                int contador = 1;
                foreach (var item in listaReprogramas)
                {
                    string letra = letras[contador - 1];
                    listaSerie.Add("REPROGRAMA " + letra);
                    contador++;
                }

                //data
                listaData.Add(listaDemanda48[1]);
                listaData.Add(listaDemanda48[0]);
                listaData.Add(listaDemanda48[3]);
                listaData.AddRange(listaReprogramas);
            }
            else
            {
                //etiquetas
                listaSerie.Add("EJECUTADO");
                listaSerie.Add("PROGRAMA");

                int contador = 1;
                foreach (var item in listaReprogramas)
                {
                    string letra = letras[contador - 1];
                    listaSerie.Add("REPROGRAMA " + letra);
                    contador++;
                }

                //data
                listaData.Add(listaDemanda48[1]);
                listaData.Add(listaDemanda48[3]);
                listaData.AddRange(listaReprogramas);
            }

            GraficoWeb grafico = new GraficoWeb();
            grafico.TitleText = "DEMANDA EJECUTADA Y PROGRAMADA DEL COES";
            grafico.XAxisCategories = ListarMediaHora48();
            grafico.SeriesName = listaSerie;
            grafico.SeriesData = new decimal?[listaData.Count][];
            grafico.SeriesDataVisible = new bool[listaData.Count][];

            List<decimal> listaHTodo = new List<decimal>();
            for (var p = 0; p < listaData.Count(); p++)
            {
                var obj48 = listaData[p];
                grafico.SeriesData[p] = new decimal?[48];
                grafico.SeriesDataVisible[p] = new bool[48];

                for (var i = 1; i <= 48; i++)
                {
                    var val = (decimal?)obj48.GetType().GetProperty("H" + i).GetValue(obj48, null);
                    grafico.SeriesData[p][i - 1] = val;
                    if (val > 0) listaHTodo.Add(val.Value);
                }

                //cuando es ejecutado, el valor de MW es visible en el gráfico
                if (p == 0)
                {
                    for (int m = 0; m < listaHDemanda.Count(); m++)
                    {
                        int posM = listaHDemanda[m] - 1;
                        grafico.SeriesDataVisible[p][posM] = true;
                    }
                }
            }

            if (listaHTodo.Any())
            {
                decimal max = listaHTodo.Max(x => x);
                decimal min = listaHTodo.Min(x => x);

                grafico.YaxixMax = max + 200;
                grafico.YaxixMin = min - 200;
            }

            return grafico;
        }

        #endregion

        // 3.13.2.5.	Reporte de la demanda por áreas (MW).
        #region REPORTE_DEMANDA_POR_AREA

        /// <summary>
        /// Genera vista html del reporte de Demanada por area
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="dataVersion"></param>
        /// <param name="idArea"></param>
        /// <param name="areas_"></param>
        /// <param name="subareas_"></param>
        /// <returns></returns>
        public static string ReporteDemandaPorAreaYSubareaHtml(string url, List<MeMedicion48DTO> listaData, DateTime fechaInicio, List<MeMedicion48DTO> dataVersion, string idArea,
                List<MeReporptomedDTO> areas_, List<MeReporptomedDTO> subareas_)
        {
            int[] areas = idArea.Split(',').Select(x => int.Parse(x)).ToArray();

            StringBuilder strHtml = new StringBuilder();
            string cabecer = string.Empty;

            NumberFormatInfo nfi = GenerarNumberFormatInfo2();

            #region cabecera

            areas_ = areas_.Where(x => areas.Contains(x.Ptomedicodi)).ToList();
            subareas_ = subareas_.Where(x => areas.Contains(x.Ptomedicodi)).ToList();

            cabecer += "<tr>";
            cabecer += "<th style='width:110px;' rowspan='2'>FECHA HORA</th>";
            cabecer += "<th style='width:70px;' rowspan='1' colspan='2'>DEMANDA</th>";
            if (areas_.Count > 0)
            {
                string btnConfigReporte = string.Format("<a href='#' onclick='verReporte({0})' title='Ver puntos de medición del Reporte'><img src='{1}Content/Images/file.png' style='padding-top:7px; padding-right: 15px;'></a>", ConstantesPR5ReportesServicio.ReporcodiDemandaAreas, url);

                cabecer += string.Format("<th style='width:70px;' rowspan='1' colspan='{0}'>ÁREA {1}</th>", areas_.Count, btnConfigReporte);
            }
            if (subareas_.Count > 0)
            {
                string btnConfigReporte = string.Format("<a href='#' onclick='verReporte({0})' title='Ver puntos de medición del Reporte'><img src='{1}Content/Images/file.png' style='padding-top:7px; padding-right: 15px;'></a>", ConstantesPR5ReportesServicio.ReporcodiDemandaSubareas, url);

                cabecer += string.Format("<th style='width:70px;' rowspan='1' colspan='{0}'>SUBAREAS {1}</th>", subareas_.Count, btnConfigReporte);
            }
            cabecer += "</tr>";

            cabecer += "<tr>";
            cabecer += "<th style='width:70px;' rowspan='1'>INTERCAMBIOS <br> INTERNACIONALES</th>";
            cabecer += "<th style='width:70px;' rowspan='1'>SEIN</th>";
            foreach (var a in areas_.OrderBy(x => x.Ptomedicodi).OrderBy(x => x.Repptoorden).ToList())
            {
                string btnConfigCalculado = string.Format("<a href='#' onclick='verPuntoCalculado({0})' title='Ver puntos de medición del Calculado'><img src='{1}Content/Images/file.png' style='padding-top:7px; padding-right: 15px;'></a>", a.Ptomedicodi, url);

                cabecer += string.Format("<th style='width:70px;' rowspan='1'>{0} {1}</th>", a.Repptonomb, btnConfigCalculado);
            }
            foreach (var a in subareas_.OrderBy(x => x.Ptomedicodi))
            {
                string btnConfigCalculado = string.Format("<a href='#' onclick='verPuntoCalculado({0})' title='Ver puntos de medición del Calculado'><img src='{1}Content/Images/file.png' style='padding-top:7px; padding-right: 15px;'></a>", a.Ptomedicodi, url);

                cabecer += string.Format("<th style='width:70px;' rowspan='1'>{0} {1}</th>", a.Repptonomb, btnConfigCalculado);
            }
            cabecer += "</tr>";
            #endregion

            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append(cabecer);
            strHtml.Append("</thead>");


            strHtml.Append("<tbody>");
            #region cuerpo
            if (listaData.Count() > 0)
            {
                decimal? valorVersi = null;
                DateTime horas = fechaInicio.AddMinutes(30);
                decimal? valor;

                //data 48
                for (int h = 1; h <= 48; h++)
                {
                    //hora
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class=''>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    //data
                    for (int i = 0; i < listaData.Count; i++)
                    {
                        string _bground = string.Empty, descrip = string.Empty;
                        var m48 = listaData[i];
                        var datVersi = dataVersion.Count > 0 ? dataVersion[i] : null;

                        if (m48 != null)
                        {
                            valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                            if (datVersi != null)
                            {
                                valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(datVersi, null);
                            }
                            if (valor != null)
                            {
                                descrip = ((decimal)valor).ToString("N", nfi);
                                if (valorVersi != null)
                                {
                                    if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                                }
                            }
                            if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }

                            string style = "background: " + _bground;
                            if (i == 0)//Intercambio Ecuador
                            {
                                if (valor != null && valor != 0)
                                {
                                    style = valor > 0 ? "background-color: #4f81bd !important; color: white;" : "background-color: #c0504d !important; color: white;";
                                }
                            }
                            strHtml.AppendFormat("<td style='{1};'>{0}</td>", descrip, style);
                        }
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(30);
                }

                //Máximo
                strHtml.Append("<tr>");
                strHtml.Append("<td class='tdbody_reporte'>MÁXIMO</td>");
                foreach (var reg in listaData)
                {
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", ((decimal)reg.Maximo).ToString("N", nfi));
                }
                strHtml.Append("</tr>");

                //Mínimo
                strHtml.Append("<tr>");
                strHtml.Append("<td class='tdbody_reporte'>MÍNIMO</td>");
                foreach (var reg in listaData)
                {
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", ((decimal)reg.Minimo).ToString("N", nfi));
                }
                strHtml.Append("</tr>");

                //Promedio
                strHtml.Append("<tr>");
                strHtml.Append("<td class='tdbody_reporte'>PROMEDIO</td>");
                foreach (var reg in listaData)
                {
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", ((decimal)reg.Promedio).ToString("N", nfi));
                }
                strHtml.Append("</tr>");

                //ENERGIA MWh
                strHtml.Append("<tr>");
                strHtml.Append("<td class='tdbody_reporte'>ENERGÍA MWh</td>");
                foreach (var reg in listaData)
                {
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", ((decimal)reg.Meditotal / 2).ToString("N", nfi));
                }
                strHtml.Append("</tr>");
            }
            else
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td colspan='{0}'>¡No existen datos para mostrar!</td>", 3 + areas_.Count + subareas_.Count);
                strHtml.Append("</tr>");
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// GetGraficoDemandaPorArea
        /// </summary>
        /// <param name="esGraficoAnexoA"></param>
        /// <param name="listaAreaySubarea"></param>
        /// <param name="objRpt"></param>
        /// <param name="listaConfGraf"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoDemandaPorArea(bool esGraficoAnexoA, List<MeMedicion48DTO> listaAreaySubarea, MeReporteDTO objRpt, List<MeReporteGraficoDTO> listaConfGraf)
        {
            var lista = listaAreaySubarea.Where(x => x.Reporcodi == ConstantesPR5ReportesServicio.ReporcodiDemandaAreas).ToList();

            var grafico = new GraficoWeb();
            grafico.TitleText = objRpt.Repornombre;
            if (esGraficoAnexoA) grafico.TitleText = "DEMANDA POR ÁREAS SEIN";

            //Eje X
            grafico.XAxisCategories = ListarMediaHora48();

            int columnSerie = 48;
            int cc = 0;
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesData = new decimal?[listaConfGraf.Count][];
            foreach (var dat in listaConfGraf)
            {
                grafico.Series.Add(new RegistroSerie());
                grafico.Series[cc].Name = dat.Repgrname;
                grafico.Series[cc].Type = dat.Repgrtype;
                grafico.Series[cc].Color = dat.Repgrcolor;
                grafico.Series[cc].YAxis = (int)dat.Repgryaxis;
                grafico.Series[cc].YAxisTitle = (dat.Repgryaxis != 0 ? "MW ÁREA CENTRO" : "MW ÁREA NORTE Y SUR");

                var d = lista.Find(c => c.Ptomedicodi == dat.Ptomedicodi);
                grafico.SeriesData[cc] = new decimal?[columnSerie];
                for (int h = 1; h <= columnSerie; h++)
                {
                    grafico.SeriesData[cc][h - 1] = (d != null) ? (decimal?)d.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(d, null) : 0;
                }

                cc++;
            }

            return grafico;
        }

        /// <summary>
        /// Generar reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol1"></param>
        /// <param name="ncol2"></param>
        /// <param name="ncol3"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="ultimaFila"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="areas_"></param>
        /// <param name="subareas_"></param>
        public static void GeneraRptDemandaPorAreaSEIN(ExcelWorksheet ws, int filaIniTitulo, int coluIniTitulo, DateTime fecha1, DateTime fecha2, ref int nfil, ref int ncol1, ref int ncol2, ref int ncol3, int tipoGrafico, out int ultimaFila
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MeReporptomedDTO> areas_, List<MeReporptomedDTO> subareas_)
        {
            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = "REPORTE DE LA DEMANDA POR ÁREAS Y SUBAREAS OPERATIVAS (MW)";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int filaIniFechaHora = filaIniTitulo + 1;
            int coluIniFechaHora = coluIniTitulo;

            int ultimaFilaData = 0;
            ultimaFila = 0;
            int ultimaColu = 0;

            ncol1 = 0; ncol2 = 0; ncol3 = 0;

            //Cabecera MeReporte
            areas_ = areas_.OrderBy(x => x.Ptomedicodi).OrderBy(x => x.Repptoorden).ToList();
            subareas_ = subareas_.OrderBy(x => x.Ptomedicodi).ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            ws.Cells[filaIniFechaHora, coluIniFechaHora].Value = "FECHA HORA";
            ws.Cells[filaIniFechaHora, coluIniFechaHora + 1].Value = "DEMANDA";
            ws.Cells[filaIniFechaHora, coluIniFechaHora + 3].Value = "AREA";
            ws.Cells[filaIniFechaHora, coluIniFechaHora + 6].Value = "SUBAREAS";
            ws.Cells[filaIniFechaHora + 1, coluIniFechaHora + 1].Value = "INTERCAMBIOS INTERNACIONALES";
            ws.Cells[filaIniFechaHora + 1, coluIniFechaHora + 2].Value = "SEIN";

            for (int i = 0; i < areas_.Count; i++)
            {
                ws.Cells[filaIniFechaHora + 1, coluIniFechaHora + 3 + i].Value = areas_[i].Ptomedibarranomb;
            }
            for (int i = 0; i < subareas_.Count; i++)
            {
                ws.Cells[filaIniFechaHora + 1, coluIniFechaHora + 3 + areas_.Count + i].Value = subareas_[i].Ptomedibarranomb;
            }
            ultimaColu = coluIniFechaHora + 3 + areas_.Count + subareas_.Count - 1;

            #region Formato Cabecera

            ws.Row(filaIniFechaHora).Height = 23;
            ws.Row(filaIniFechaHora + 1).Height = 31;

            UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, coluIniFechaHora);
            UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, coluIniFechaHora + 1, filaIniFechaHora, coluIniFechaHora + 2);
            UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, coluIniFechaHora + 3, filaIniFechaHora, coluIniFechaHora + 3 + areas_.Count - 1);
            UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, coluIniFechaHora + 3 + areas_.Count, filaIniFechaHora, coluIniFechaHora + 3 + areas_.Count + subareas_.Count - 1);

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu, "Arial", 10);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu);
            UtilExcel.CeldasExcelWrapText(ws, filaIniFechaHora + 1, coluIniFechaHora + 1, filaIniFechaHora + 1, ultimaColu);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);
            UtilExcel.CeldasExcelColorTexto(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColu, "#FFFFFF");

            #endregion

            #endregion

            #region cuerpo

            for (int h = 1; h <= 48; h++)
            {
                int rowH = filaIniFechaHora + 1 + h;
                ws.Cells[rowH, coluIniFechaHora].Value = fecha1.AddMinutes(h * 30).ToString(ConstantesBase.FormatoHoraMinuto);

                for (int i = 0; i < data.Count; i++)
                {
                    int colH = coluIniFechaHora + 1 + i;
                    var m48 = data[i];
                    decimal? valor = null;
                    if (m48 != null)
                    {
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        ws.Cells[rowH, colH].Value = valor;

                        if (i == 0)//Intercambio Ecuador
                        {
                            if (valor != null && valor != 0)
                            {
                                string colorstyle = valor > 0 ? "#4f81bd" : " #c0504d";
                                UtilExcel.CeldasExcelColorFondo(ws, rowH, colH, rowH, colH, colorstyle);
                                UtilExcel.CeldasExcelColorTexto(ws, rowH, colH, rowH, colH, "#FFFFFF");
                            }
                        }
                    }
                }

                ws.Row(rowH).Height = 11;
            }

            ultimaFilaData = filaIniFechaHora + 1 + 48;

            #region Formato Cuerpo

            int filaIniData = filaIniFechaHora + 2;
            int coluIniData = coluIniFechaHora;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFilaData, ultimaColu, "Arial", 8);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFilaData, coluIniData + 2, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, ultimaFilaData, ultimaColu, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData, ultimaFilaData, coluIniData);
            UtilExcel.BorderCeldas4(ws, filaIniData, coluIniData, ultimaFilaData, ultimaColu, Color.Black);
            ws.Cells[filaIniData, coluIniData, ultimaFilaData, ultimaColu].Style.Numberformat.Format = "#,##0.00";

            #endregion

            #endregion

            #region Formato Totales
            //Máximo
            ws.Cells[ultimaFilaData + 1, coluIniData].Value = "MÁXIMO";
            ws.Row(ultimaFilaData + 1).Height = 17;
            int col = 0;
            foreach (var reg in data)
            {
                ws.Cells[ultimaFilaData + 1, coluIniData + 1 + col].Value = ((decimal)reg.Maximo);
                ws.Cells[ultimaFilaData + 1, coluIniData + 1 + col].Style.Numberformat.Format = "#,##0.00";
                col++;
            }

            //Mínimo
            ws.Cells[ultimaFilaData + 2, coluIniData].Value = "MÍNIMO";
            ws.Row(ultimaFilaData + 2).Height = 17;
            col = 0;
            foreach (var reg in data)
            {
                ws.Cells[ultimaFilaData + 2, coluIniData + 1 + col].Value = ((decimal)reg.Minimo);
                ws.Cells[ultimaFilaData + 2, coluIniData + 1 + col].Style.Numberformat.Format = "#,##0.00";
                col++;
            }

            //Promedio
            ws.Cells[ultimaFilaData + 3, coluIniData].Value = "PROMEDIO";
            ws.Row(ultimaFilaData + 3).Height = 17;
            col = 0;
            foreach (var reg in data)
            {
                ws.Cells[ultimaFilaData + 3, coluIniData + 1 + col].Value = ((decimal)reg.Promedio);
                ws.Cells[ultimaFilaData + 3, coluIniData + 1 + col].Style.Numberformat.Format = "#,##0.00";
                col++;
            }

            //ENERGIA MWh
            ws.Cells[ultimaFilaData + 4, coluIniData].Value = "ENERGÍA";
            ws.Row(ultimaFilaData + 4).Height = 17;
            col = 0;
            foreach (var reg in data)
            {
                ws.Cells[ultimaFilaData + 4, coluIniData + 1 + col].Value = ((decimal)reg.Meditotal / 2);
                ws.Cells[ultimaFilaData + 4, coluIniData + 1 + col].Style.Numberformat.Format = "#,##0.00";
                col++;
            }
            ultimaFila = ultimaFilaData + 4;

            #region Formato Totales

            UtilExcel.CeldasExcelColorFondo(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelColorTexto(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu, "Arial", 10);
            UtilExcel.CeldasExcelEnNegrita(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, ultimaFilaData + 1, coluIniData, ultimaFila, coluIniData + 2, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu, "Centro");
            UtilExcel.BorderCeldas4(ws, ultimaFilaData + 1, coluIniData, ultimaFila, ultimaColu, Color.Black);
            UtilExcel.BorderCeldas4_1(ws, ultimaFilaData + 4, coluIniData, ultimaFila, ultimaColu, Color.White);

            #endregion

            #endregion

            #region Datos Grafico
            nfil = filaIniData; // fila inicial de los datos
            ncol1 = coluIniData; // columna inicial de los datos                
            ncol2 = areas_.Count; // numero de áreas
            #endregion

            for (int c = coluIniFechaHora; c <= ultimaColu; c++)
            {
                ws.Column(c).Width = 22;
            }
            ws.Column(coluIniFechaHora + 1).Width = 20;
            ws.Column(coluIniFechaHora + 2).Width = 18;

            ws.Cells[ultimaFila + 2, coluIniData].Value = "LEYENDA:";
            ws.Cells[ultimaFila + 4, coluIniData + 2].Value = "Potencia recibida del Ecuador";
            ws.Cells[ultimaFila + 5, coluIniData + 2].Value = "Potencia entregada al Ecuador";

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 2, coluIniData, ultimaFila + 3, coluIniData, "Arial", 12);
            UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 2, coluIniData, ultimaFila + 3, coluIniData);

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 4, coluIniData + 3, ultimaFila + 5, coluIniData + 3, "Arial", 10);
            UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 4, coluIniData + 3, ultimaFila + 5, coluIniData + 3);

            UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 4, coluIniData, ultimaFila + 4, coluIniData + 1, "#c0504d");
            UtilExcel.BorderCeldas(ws, ultimaFila + 4, coluIniData, ultimaFila + 4, coluIniData + 1);
            UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 5, coluIniData, ultimaFila + 5, coluIniData + 1, "#4f81bd");
            UtilExcel.BorderCeldas(ws, ultimaFila + 5, coluIniData, ultimaFila + 5, coluIniData + 1);

            ws.View.FreezePanes(filaIniFechaHora + 2, coluIniFechaHora + 1);

            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaDatos"></param>
        /// <param name="coluDatos"></param>
        /// <param name="numAreas"></param>
        /// <param name="yAxisTitleIzq"></param>
        /// <param name="yAxisTitleDer"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="iniFor"></param>
        /// <param name="filaInicioGrafico"></param>
        public static void AddGraficoDemandaPorAreaSEIN(ExcelWorksheet ws, int filaDatos, int coluDatos, int numAreas, string yAxisTitleIzq, string yAxisTitleDer, string titulo, int rep, int iniFor, int filaInicioGrafico)
        {
            string nameGraf = string.Empty;
            switch (rep)
            {
                case 1: nameGraf = "grafDemandaPorAreaSEIN1"; break;
            }

            var LineaChart = ws.Drawings.AddChart(nameGraf, eChartType.Line) as ExcelLineChart;
            LineaChart.SetPosition(filaInicioGrafico, 0, 3, 0);
            LineaChart.SetSize(1200, 600);

            for (int i = 0; i < numAreas; i++)
            {
                if (i != 1) //si no es CENTRO
                {
                    var ran1 = ws.Cells[filaDatos, coluDatos + 3 + i, filaDatos + 48 - 1, coluDatos + 3 + i];
                    var ran2 = ws.Cells[filaDatos, coluDatos, filaDatos + 48 - 1, coluDatos];

                    var serie = (ExcelChartSerie)LineaChart.Series.Add(ran1, ran2);
                    serie.Header = (ws.Cells[filaDatos - 1, coluDatos + 3 + i].Value != null) ? ws.Cells[filaDatos - 1, coluDatos + 3 + i].Value.ToString() : "";
                }
                else // si es CENTRO
                {
                    var LineaChart2 = LineaChart.PlotArea.ChartTypes.Add(eChartType.Line);
                    var ran1 = ws.Cells[filaDatos, coluDatos + 3 + i, filaDatos + 48 - 1, coluDatos + 3 + i];
                    var ran2 = ws.Cells[filaDatos, coluDatos, filaDatos + 48 - 1, coluDatos];

                    var serie = (ExcelChartSerie)LineaChart2.Series.Add(ran1, ran2);
                    serie.Header = (ws.Cells[filaDatos - 1, coluDatos + 3 + i].Value != null) ? ws.Cells[filaDatos - 1, coluDatos + 3 + i].Value.ToString() : "";
                    LineaChart2.UseSecondaryAxis = true; //Flip the axes

                    LineaChart2.YAxis.Title.Text = yAxisTitleDer;
                    LineaChart2.YAxis.Title.Font.Color = Color.DarkRed;
                    LineaChart2.YAxis.Font.Color = Color.DarkRed;
                    LineaChart2.YAxis.Font.Bold = true;
                    LineaChart2.YAxis.Title.Font.Size = 10;
                    LineaChart2.YAxis.Title.Font.Bold = true;
                    LineaChart2.Series[0].Fill.Color = Color.Orange;
                }
            }
            //aqui me quede falta cambiar color a las lineas del grafico


            LineaChart.Title.Text = titulo;
            LineaChart.Title.Font.Bold = true;
            LineaChart.DataLabel.ShowLeaderLines = true;
            LineaChart.YAxis.Title.Text = yAxisTitleIzq;
            LineaChart.YAxis.Title.Font.Color = Color.DarkBlue;
            LineaChart.YAxis.Font.Color = Color.DarkBlue;
            LineaChart.YAxis.Font.Bold = true;
            LineaChart.YAxis.Title.Font.Size = 10;
            LineaChart.YAxis.Title.Font.Bold = true;

            LineaChart.Legend.Position = eLegendPosition.Bottom;
            LineaChart.Legend.Font.Size = 16;
        }

        #endregion

        // 3.13.2.6.	Reporte de Demanda de Grandes Usuarios (MW).
        #region REPORTE_DEMANDA_GRANDES_USUARIOS

        /// <summary>
        /// genera vista html del preporte de Demanda Grandes Usuarios
        /// </summary>
        /// <param name="listaDataXEmprArea"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaEmpresaArea"></param>
        /// <returns></returns>
        public static string ReporteDemandaGrandesUsuariosHtml(List<MeMedicion48DTO> listaDataXEmprArea, List<MeMedicion48DTO> dataVersion, List<SiEmpresaDTO> listaEmpresaArea)
        {
            StringBuilder strHtml = new StringBuilder();
            string cabecer = string.Empty;

            NumberFormatInfo nfi2 = GenerarNumberFormatInfo3();
            nfi2.NumberGroupSeparator = " ";
            nfi2.NumberDecimalDigits = 2;
            nfi2.NumberDecimalSeparator = ",";

            NumberFormatInfo nfi1 = GenerarNumberFormatInfo3();
            nfi1.NumberGroupSeparator = " ";
            nfi1.NumberDecimalDigits = 1;
            nfi1.NumberDecimalSeparator = ",";

            //set la lista de areas a usar
            List<MeReporteDTO> listaReporte = UtilSemanalPR5.GetListaReporteUL(false);

            //
            var total = listaEmpresaArea.Count * 80;
            strHtml.Append("<div id='div_reporte' style='height: auto; margin: 0 auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px'>", total);

            strHtml.Append("<thead>");
            #region cabecera

            //Área
            cabecer += "<tr>";
            cabecer += "<th style='width: 70px' rowspan='2'>FECHA HORA</th>";
            foreach (var area in listaReporte)
            {
                int totalPto = listaEmpresaArea.Where(x => x.AreaOperativa == area.AreaOperativa).Count();
                cabecer += string.Format("<th rowspan='1' class='grande_usuario_libre_area_{2}' colspan='{1}'>{0}</th>", area.Repornombre, totalPto, area.AreaOperativa);
            }
            cabecer += "</tr>";

            //Punto calculados
            cabecer += "<tr>";
            foreach (var area in listaReporte)
            {
                var listaPto = listaEmpresaArea.Where(x => x.AreaOperativa == area.AreaOperativa).ToList();
                foreach (var pto in listaPto)
                {
                    cabecer += string.Format("<th style='word-wrap: break-word;white-space: normal;' rowspan='1' class='grande_usuario_libre_area_{1}'>{0}</th>", pto.Emprnomb, area.AreaOperativa);
                }
            }
            cabecer += "</tr>";
            #endregion
            strHtml.Append(cabecer);
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo
            if (listaEmpresaArea.Count() > 0)
            {
                decimal? valorVersi = null;
                DateTime horas = DateTime.Now.Date;
                decimal? valor;

                //data 48
                for (int h = 1; h <= 48; h++)
                {
                    //hora
                    horas = horas.AddMinutes(30);
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:HH:mm}</td>", horas);

                    //data
                    for (int i = 0; i < listaEmpresaArea.Count; i++)
                    {
                        string _bground = string.Empty, descrip = string.Empty;

                        var objPto = listaEmpresaArea[i];
                        MeMedicion48DTO m48 = listaDataXEmprArea.Find(x => x.Emprcodi == objPto.Emprcodi && x.AreaOperativa == objPto.AreaOperativa) ?? new MeMedicion48DTO();
                        var datVersi = dataVersion.Find(x => x.Ptomedicodi == m48.Ptomedicodi);
                        if (m48 != null)
                        {
                            valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                            if (datVersi != null)
                            {
                                valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(datVersi, null);
                            }
                            if (valor != null)
                            {
                                descrip = ((decimal)valor).ToString("N", nfi1);
                                if (valorVersi != null)
                                {
                                    if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi1); _bground = "lightgreen"; }
                                }
                            }
                            if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi1); _bground = "lightgreen"; }
                            strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);
                        }
                    }

                    strHtml.Append("</tr>");
                }

                //ENERGIA MWh
                strHtml.Append("<tr>");
                strHtml.Append("<td class='tdbody_reporte'>ENERGÍA MWh</td>");
                foreach (var objPto in listaEmpresaArea)
                {
                    MeMedicion48DTO m48 = listaDataXEmprArea.Find(x => x.Emprcodi == objPto.Emprcodi && x.AreaOperativa == objPto.AreaOperativa) ?? new MeMedicion48DTO();

                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", (m48.Meditotal.GetValueOrDefault(0) / 2).ToString("N", nfi2));
                }
                strHtml.Append("</tr>");
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el listado del excel del reporte Demanda Grandes Usuarios
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol1"></param>
        /// <param name="ncol2"></param>
        /// <param name="ncol3"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="ListaSeries"></param>
        /// <param name="listaDataXEmprArea"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaEmpresaArea"></param>
        public static void GeneraRptDemandaGrandesUsuarios(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, ref int nfil, ref int ncol1, ref int ncol2, ref int ncol3, int tipoGrafico, ref List<int> ListaSeries
            , List<MeMedicion48DTO> listaDataXEmprArea, List<MeMedicion48DTO> dataVersion, List<SiEmpresaDTO> listaEmpresaArea)
        {
            List<MeReporteDTO> listaReporte = UtilSemanalPR5.GetListaReporteUL(false);

            int numS = 0;
            int ultimaFila = 0;

            if (listaEmpresaArea.Count > 0)
            {
                var colorCeldaDatos = ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI);
                var colorFondo1 = ColorTranslator.FromHtml("#D9E1F2");
                var colorFondo2 = ColorTranslator.FromHtml("#ACB9CA");

                #region cabecera

                int filaIniFechaHora = 6;
                int coluIniFechaHora = 2;

                int ultimaColumna = 0;

                //titulo de Excel
                ws.Cells[filaIniFechaHora - 1, coluIniFechaHora].Value = "POTENCIA DE LOS GRANDES USUARIOS LIBRES (MW)";

                //Área
                ws.Cells[filaIniFechaHora, coluIniFechaHora].Value = "FECHA HORA";
                int inicio = 0, final = 0;

                List<int> ultimos = new List<int>();
                foreach (var area in listaReporte)
                {
                    int totalPto = listaEmpresaArea.Where(x => x.AreaOperativa == area.AreaOperativa).Count();
                    if (inicio == 0)
                        inicio = coluIniFechaHora + 1;
                    final = inicio + totalPto - 1;
                    ultimos.Add(final);
                    ws.Cells[filaIniFechaHora, inicio].Value = area.Repornombre;
                    UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, inicio, filaIniFechaHora, final);
                    numS = final - inicio + 1;
                    ListaSeries.Add(numS);

                    inicio = final + 1;
                }

                //Punto calculados
                int col = 0;
                foreach (var area in listaReporte)
                {
                    var listaPto = listaEmpresaArea.Where(x => x.AreaOperativa == area.AreaOperativa).ToList();

                    foreach (var pto in listaPto)
                    {
                        ws.Cells[filaIniFechaHora + 1, coluIniFechaHora + 1 + col].Value = pto.Emprnomb;
                        col++;
                    }
                }
                ultimaColumna = coluIniFechaHora + listaEmpresaArea.Count();

                #region Formato Cabecera
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora - 1, coluIniFechaHora, filaIniFechaHora - 1, coluIniFechaHora, "Arial", 18);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniFechaHora - 1, coluIniFechaHora, filaIniFechaHora - 1, coluIniFechaHora);

                //Fecha - Hora
                UtilExcel.CeldasExcelAgrupar(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, coluIniFechaHora);
                UtilExcel.CeldasExcelColorTexto(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, coluIniFechaHora, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, coluIniFechaHora, ConstantesPR5ReportesServicio.ColorInfSGI);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, coluIniFechaHora, "Arial", 8);

                //Areas
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora, coluIniFechaHora + 1, filaIniFechaHora, ultimaColumna, "Arial", 14);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora + 1, coluIniFechaHora + 1, filaIniFechaHora + 1, ultimaColumna, "Arial", 8);
                int inicio1 = coluIniFechaHora + 1;
                int b = 1;
                foreach (var item in ultimos) //colores en fondo y texto
                {
                    if (b % 2 == 1)
                    {
                        UtilExcel.CeldasExcelColorFondoYBorder(ws, filaIniFechaHora, inicio1, filaIniFechaHora + 1, item, colorFondo1, colorCeldaDatos);
                    }
                    else
                    {
                        UtilExcel.CeldasExcelColorFondoYBorder2(ws, filaIniFechaHora, inicio1, filaIniFechaHora + 1, item, colorCeldaDatos, Color.White, colorCeldaDatos, Color.White);
                    }

                    inicio1 = item + 1;
                    b++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniFechaHora, coluIniFechaHora, filaIniFechaHora + 1, ultimaColumna);
                for (int i = coluIniFechaHora + 1; i <= ultimaColumna; i++)
                {
                    UtilExcel.CeldasExcelWrapText(ws, filaIniFechaHora + 1, i);
                }

                //
                ws.Row(filaIniFechaHora).Height = 33.75;
                ws.Row(filaIniFechaHora + 1).Height = 48.75;

                ws.Column(1).Width = 2.5;
                ws.Column(coluIniFechaHora).Width = 11.52;
                for (int i = coluIniFechaHora + 1; i <= ultimaColumna; i++)
                {
                    ws.Column(i).Width = 14.56;
                }


                #endregion

                #endregion

                #region cuerpo

                for (int h = 1; h <= 48; h++)
                {
                    ws.Cells[h + filaIniFechaHora + 1, coluIniFechaHora].Value = fecha1.AddMinutes(h * 30).ToString("HH:mm");
                }
                for (int i = 0; i < listaEmpresaArea.Count; i++)
                {
                    var objPto = listaEmpresaArea[i];
                    MeMedicion48DTO m48 = listaDataXEmprArea.Find(x => x.Emprcodi == objPto.Emprcodi && x.AreaOperativa == objPto.AreaOperativa) ?? new MeMedicion48DTO();
                    for (int h = 1; h <= 48; h++)
                    {
                        var valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        ws.Cells[h + filaIniFechaHora + 1, i + coluIniFechaHora + 1].Value = valor;
                        ws.Cells[h + filaIniFechaHora + 1, i + coluIniFechaHora + 1].Style.Numberformat.Format = "#,##0.0";
                    }
                }

                // TOTALES MWh
                ws.Cells[49 + filaIniFechaHora + 1, coluIniFechaHora].Value = "MWh";

                int coluTotales = 0;
                foreach (var objPto in listaEmpresaArea)
                {
                    MeMedicion48DTO m48 = listaDataXEmprArea.Find(x => x.Emprcodi == objPto.Emprcodi && x.AreaOperativa == objPto.AreaOperativa) ?? new MeMedicion48DTO();

                    int rowIni = 49 + filaIniFechaHora + 1;
                    int colIni = coluIniFechaHora + 1 + coluTotales;
                    ws.Cells[rowIni, colIni].Value = m48.Meditotal.GetValueOrDefault(0) / 2;
                    ws.Cells[rowIni, colIni].Style.Numberformat.Format = "#,##0.00";
                    coluTotales++;
                }

                ultimaFila = 49 + filaIniFechaHora + 1;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, ultimaColumna, "Arial", 12);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, coluIniFechaHora);

                UtilExcel.CeldasExcelWrapText(ws, 49 + filaIniFechaHora + 1, coluIniFechaHora);
                UtilExcel.CeldasExcelEnNegrita(ws, 49 + filaIniFechaHora + 1, coluIniFechaHora, ultimaFila, ultimaColumna);

                UtilExcel.BorderCeldas4(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, ultimaColumna, colorCeldaDatos);
                UtilExcel.BorderCeldas6(ws, filaIniFechaHora + 2, coluIniFechaHora, ultimaFila, ultimaColumna, Color.White, colorCeldaDatos);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, ultimaFila, coluIniFechaHora, ultimaFila, ultimaColumna, colorFondo1, colorCeldaDatos);

                ws.Row(ultimaFila).Height = 31.40;
                #endregion


                nfil = ultimaFila;
                ncol2 = filaIniFechaHora;
                ncol3 = coluIniFechaHora;

                #endregion

                #region Notas
                ws.Cells[ultimaFila + 3, coluIniFechaHora].Value = "Nota :";
                ws.Cells[ultimaFila + 3 + 1, coluIniFechaHora].Value = "Información proporcionada por la empresas";
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 3, coluIniFechaHora, ultimaFila + 3 + 1, coluIniFechaHora, "Arial", 12);
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 3, coluIniFechaHora, ultimaFila + 3, coluIniFechaHora);
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 3 + 1, coluIniFechaHora, ultimaFila + 3 + 1, coluIniFechaHora);

                UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 3 + 3, coluIniFechaHora, ultimaFila + 3 + 3, coluIniFechaHora, "#808080");
                UtilExcel.CeldasExcelBordear(ws, ultimaFila + 3 + 3, coluIniFechaHora, ultimaFila + 3 + 3, coluIniFechaHora);
                ws.Cells[ultimaFila + 3 + 3, coluIniFechaHora + 1].Value = "DATOS ESTIMADOS";
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 3 + 3, coluIniFechaHora + 1, ultimaFila + 3 + 3, coluIniFechaHora + 1, "Arial", 10);
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 3 + 3, coluIniFechaHora + 1, ultimaFila + 3 + 3, coluIniFechaHora + 1);

                UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 3 + 5, coluIniFechaHora, ultimaFila + 3 + 5, coluIniFechaHora, "#B4C6E7");
                UtilExcel.CeldasExcelBordear(ws, ultimaFila + 3 + 5, coluIniFechaHora, ultimaFila + 3 + 5, coluIniFechaHora);
                ws.Cells[ultimaFila + 3 + 5, coluIniFechaHora + 1].Value = "SIN DATOS";
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 3 + 5, coluIniFechaHora + 1, ultimaFila + 3 + 5, coluIniFechaHora + 1, "Arial", 10);
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila + 3 + 5, coluIniFechaHora + 1, ultimaFila + 3 + 5, coluIniFechaHora + 1);

                #endregion

                #region Escala Y freezer
                ws.View.FreezePanes(8, 3);
                ws.View.ZoomScale = 70;
                #endregion
            }

        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaSeries"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        public static void AddGraficoLineasDemandaGrandesUsuarios(ExcelWorksheet ws, List<int> listaSeries, string xAxisTitle, string yAxisTitle, string titulo, int rep,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie)
        {

            string nameGraf = string.Empty;
            int numSeries = 0;
            switch (rep)
            {
                case 1:
                    nameGraf = "grafGrandesUsuariosNor";
                    numSeries = listaSeries[0];
                    break;
                case 2:
                    nameGraf = "grafGrandesUsuariosCen1";
                    numSeries = listaSeries[1] / 2 + 1;
                    break;
                case 3:
                    nameGraf = "grafGrandesUsuariosCen2";
                    if (listaSeries[1] % 2 == 0)
                    {
                        numSeries = listaSeries[1] / 2 - 1;
                    }
                    else
                    {
                        numSeries = listaSeries[1] / 2 - 1 + 1;
                    }
                    break;
                case 4:
                    nameGraf = "grafGrandesUsuariosSur";
                    numSeries = listaSeries[2];
                    break;
            }

            var LineChart = ws.Drawings.AddChart(nameGraf, eChartType.Line);
            LineChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);

            LineChart.SetSize(1200, 720);

            for (int sn = 0; sn < numSeries; sn++)
            {
                var ran1 = ws.Cells[filaIniSerie, coluIniSerie + sn, filaIniSerie + 47, coluIniSerie + sn];
                var ran2 = ws.Cells[filaIniXSerie, coluIniXSerie, filaIniXSerie + 47, coluIniXSerie];

                var serie = (ExcelChartSerie)LineChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[filaIniSerie - 1, coluIniSerie + sn].Value.ToString();
            }


            LineChart.Title.Text = titulo;
            LineChart.Title.Font.Bold = true;
            LineChart.YAxis.Title.Text = yAxisTitle;

            LineChart.Legend.Position = eLegendPosition.Bottom;
            LineChart.Legend.Font.Size = 8;
            LineChart.Axis[0].DisplayUnit = 5.0;
        }

        /// <summary>
        /// ListarGraficoDemandaGrandesUsuarios
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaEmpresaArea"></param>
        /// <returns></returns>
        public static List<GraficoWeb> ListarGraficoDemandaGrandesUsuarios(List<MeMedicion48DTO> lista, List<SiEmpresaDTO> listaEmpresaArea)
        {
            List<GraficoWeb> listaGrafico = new List<GraficoWeb>();

            for (var tipoGrafico = 1; tipoGrafico <= 4; tipoGrafico++)
            {
                GraficoWeb grafico = new GraficoWeb();

                string titulo = "";
                List<SiEmpresaDTO> listaRptPtosGraf = new List<SiEmpresaDTO>();
                switch (tipoGrafico)
                {
                    case 1: //area norte
                        titulo = @"GRANDES USUARIOS AREA NORTE";
                        listaRptPtosGraf = listaEmpresaArea.Where(x => x.AreaOperativa == "NORTE").ToList();
                        break;
                    case 2: //area centro 1
                        titulo = @"GRANDES USUARIOS AREA CENTRO (1)";
                        listaRptPtosGraf = listaEmpresaArea.Where(x => x.AreaOperativa == "CENTRO").ToList();
                        if (listaRptPtosGraf.Count > 0)
                        {
                            int total = listaRptPtosGraf.Count;
                            listaRptPtosGraf = listaRptPtosGraf.GetRange(0, total / 2 + 1);
                        }
                        break;
                    case 3:
                        titulo = @"GRANDES USUARIOS AREA CENTRO (2)";
                        listaRptPtosGraf = listaEmpresaArea.Where(x => x.AreaOperativa == "CENTRO").ToList();
                        if (listaRptPtosGraf.Count > 0)
                        {
                            int total = listaRptPtosGraf.Count;
                            if (total % 2 == 0)
                                listaRptPtosGraf = listaRptPtosGraf.GetRange(total / 2 + 1, total / 2 - 1);
                            else
                                listaRptPtosGraf = listaRptPtosGraf.GetRange(total / 2 + 1, total / 2 - 1 + 1);
                        }
                        break;
                    case 4:
                        titulo = @"GRANDES USUARIOS AREA SUR";
                        listaRptPtosGraf = listaEmpresaArea.Where(x => x.AreaOperativa == "SUR").ToList();
                        break;
                }

                grafico.TitleText = titulo;
                grafico.SeriesType = new List<string>();
                grafico.SeriesName = new List<string>();
                grafico.YAxixTitle = new List<string>();
                grafico.SerieDataS = new DatosSerie[listaRptPtosGraf.Count][];
                grafico.YaxixTitle = "MW";

                //Eje X
                grafico.XAxisCategories = new List<string>();
                DateTime horas = DateTime.Now.Date;
                for (int h = 1; h <= 48; h++)
                {
                    grafico.XAxisCategories.Add(horas.ToString(ConstantesAppServicio.FormatoOnlyHora));
                    horas = horas.AddMinutes(30);
                }

                grafico.Series = new List<RegistroSerie>();
                grafico.SeriesData = new decimal?[listaRptPtosGraf.Count][];
                for (int i = 0; i < listaRptPtosGraf.Count; i++)
                {
                    var pto = listaRptPtosGraf[i];
                    grafico.Series.Add(new RegistroSerie());
                    grafico.Series[i].Name = pto.Emprnomb;
                    grafico.Series[i].Type = "line";
                    grafico.Series[i].YAxis = 0;

                    grafico.SeriesData[i] = new decimal?[48];

                    var objData = lista.Find(x => x.Emprcodi == pto.Emprcodi && x.AreaOperativa == pto.AreaOperativa);
                    for (int h = 1; h <= 48; h++)
                    {
                        grafico.SeriesData[i][h - 1] = (decimal?)objData.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objData, null);
                    }
                }

                listaGrafico.Add(grafico);
            }

            return listaGrafico;
        }

        #endregion

        // 3.13.2.7.	Recursos energéticos y diagrama de duración de demanda del SEIN.
        #region REPORTE_RECURSOS_ENERGETICOS_DEMANDASEIN

        /// <summary>
        /// Genera vista html de reporte potencia agrupado por tipo de recurso
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ListaReportePotenciaXTipoRecursoHtml(List<MeMedicion48DTO> Result, List<MeMedicion48DTO> dataVersion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            if (Result.Count > 0)
            {
                //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

                strHtml.Append("<table class='pretty tabla-icono'>");
                strHtml.Append("<thead>");
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<th colspan='{0}' class='titulo_tipo_recurso'>REPORTE POTENCIA GENERADA POR TIPO DE RECURSO</th>", Result.Count + 2);
                strHtml.Append("</tr>");
                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:70px;height: 30px;'>HORA</th>");
                foreach (var item in Result)
                {
                    strHtml.Append("<th>" + item.Ctgdetnomb + "</th>");
                }
                strHtml.Append("<th>TOTAL</th>");
                strHtml.Append("</tr>");
                strHtml.Append("</thead>");
                // ****************************************  CUERPO DE LA TABLA ******************************************************         
                strHtml.Append("<tbody>");
                decimal? valorVersi = null;
                DateTime horas = DateTime.Now.Date;
                int j = 1;
                decimal? valor;
                decimal acumuladoH = 0;
                for (int i = 0; i < 48; i++)
                {
                    acumuladoH = 0;
                    horas = horas.AddMinutes(30);
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='width:70px;'>{0:HH:mm}</td>", horas);

                    foreach (var item in Result)
                    {
                        string _bground = string.Empty, descrip = string.Empty;
                        MeMedicion48DTO datVersi = null;

                        descrip = string.Empty;
                        valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(item, null);
                        if (datVersi != null)
                        {
                            valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(datVersi, null);
                        }
                        if (valor != null)
                        {
                            descrip = ((decimal)valor).ToString("N", nfi);
                            if (valorVersi != null)
                            {
                                if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                            }
                            acumuladoH += decimal.Parse(descrip.Replace(",", ".").Replace(" ", ""));
                        }
                        if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; acumuladoH += valorVersi.Value; }
                        strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", (descrip.Equals(string.Empty) ? "0,000" : descrip));
                    }

                    strHtml.AppendFormat("<td>{0}</td>", (acumuladoH).ToString("N", nfi));

                    j++;
                    strHtml.Append("</tr>");
                }

                decimal acumuladoTotal = 0;
                foreach (var item in Result)
                {
                    decimal? acumulado = null;
                    for (int i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                        if (valor != null)
                        {
                            acumulado = acumulado == null ? valor : acumulado + valor;
                        }
                    }

                    item.Meditotal = acumulado.GetValueOrDefault(0) / (decimal)2;
                }

                foreach (var item in Result)
                {
                    acumuladoTotal += item.Meditotal.GetValueOrDefault(0);
                }

                //totales
                strHtml.Append("<tr>");
                strHtml.Append("<td class='totalTipoRecurso'>TOTAL MWh</td>");
                foreach (var item in Result)
                {
                    if (item.Meditotal != null)
                        strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", ((decimal)item.Meditotal).ToString("N", nfi));
                    else
                        strHtml.AppendFormat("<td class='totalTipoRecurso'>0,000</td>");
                }
                strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", (acumuladoTotal).ToString("N", nfi));
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<td class='totalTipoRecurso'>%</td>");
                foreach (var item in Result)
                {
                    decimal porcentaje = 0;
                    if (acumuladoTotal > 0)
                    {
                        porcentaje = item.Meditotal.GetValueOrDefault(0) * 100 / acumuladoTotal;
                        strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", ((decimal)porcentaje).ToString("N", nfi));
                    }
                    else
                    {
                        strHtml.AppendFormat("<td class='totalTipoRecurso'>0,000</td>");
                    }
                }
                strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", (100).ToString("N", nfi));
                strHtml.Append("</tr>");

                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte potwncia por tipo de recurso hirologico
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ListaReportePotenciaXTipoHidroHtml(List<MeMedicion48DTO> data)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<div id='reporteHidro' style=' overflow: auto; width: 1000px; height: auto; margin: 0 auto;'>");
            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");

            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:350px;' rowspan='1'>EMPRESA</th>");
            strHtml.Append("<th style='width:350px;' rowspan='1'>CENTRAL</th>");
            strHtml.Append("<th style='width:150px;' rowspan='1'>HIDROELECTRICA <br/> DE PASADA</th>");
            strHtml.Append("<th style='width:150px;' rowspan='1'>HIDROELECTRICA <br/> DE REGULACION</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            #region cuerpo_reporte
            // **************  CUERPO DE LA TABLA **************//
            var listaEmpresa = data.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);
            var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            strHtml.Append("<tbody>");
            if (listaEmprcodi.Count > 0)
            {
                decimal totalPasada = 0;
                decimal totalRegulacion = 0;

                foreach (var empcodi in listaEmprcodi)
                {
                    //empresa
                    string nomEmpresa = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                    int cantPtoPorEmpresa = data.Where(x => x.Emprcodi == empcodi).Select(y => y.Equipadre).Distinct().Count();
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td rowspan='" + cantPtoPorEmpresa + "' class='td_empresa'>{0}</td>", nomEmpresa);

                    //central
                    var listaCentral = data.Where(x => x.Emprcodi == empcodi).Select(y => new { y.Equipadre, y.Central }).Distinct().ToList().OrderBy(c => c.Central);
                    var listaCentralcodi = listaCentral.Select(x => x.Equipadre).ToList();
                    for (int u = 0; u < listaCentralcodi.Count; u++)
                    {
                        if (u != 0)
                        {
                            strHtml.Append("</tr>");
                            strHtml.Append("<tr>");
                        }

                        var centralcodi = listaCentralcodi[u];
                        var nomCentral = data.Where(x => x.Emprcodi == empcodi && x.Equipadre == centralcodi).First().Central;
                        int cantPtoPorCentral = 1;
                        strHtml.AppendFormat("<td rowspan='" + cantPtoPorCentral + "' class='td_central'>{0}</td>", nomCentral);

                        //Pasada / Regulacion
                        decimal? valor = null, acumulado = null;
                        var listaData = data.Where(x => x.Equipadre == centralcodi && x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaPasada).ToList();
                        for (int i = 1; i <= 48; i++)
                        {
                            foreach (var item in listaData)
                            {
                                valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                                if (valor != null)
                                {
                                    acumulado = acumulado == null ? valor : acumulado.GetValueOrDefault(0) + valor;
                                }
                            }
                        }

                        if (acumulado != null)
                        {
                            totalPasada += (decimal)acumulado / 2;
                            strHtml.AppendFormat("<td>{0}</td>", ((decimal)acumulado / 2).ToString("N", nfi));
                        }
                        else
                        {
                            strHtml.AppendFormat("<td></td>");
                        }

                        // Regulacion
                        valor = null;
                        acumulado = null;
                        listaData = data.Where(x => x.Equipadre == centralcodi && x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaRegulacion).ToList();
                        for (int i = 1; i <= 48; i++)
                        {
                            foreach (var item in listaData)
                            {
                                valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                                if (valor != null)
                                {
                                    acumulado = acumulado == null ? valor : acumulado.GetValueOrDefault(0) + valor;
                                }
                            }
                        }

                        if (acumulado != null)
                        {
                            totalRegulacion += (decimal)acumulado / 2;
                            strHtml.AppendFormat("<td>{0}</td>", ((decimal)acumulado / 2).ToString("N", nfi));
                        }
                        else
                            strHtml.AppendFormat("<td></td>");
                    }
                    strHtml.Append("</tr>");
                }

                //totales
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td colspan='2' class='totalTipoRecursoHidro'>TOTAL MWh</td>");
                strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", (totalPasada).ToString("N", nfi));
                strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", (totalRegulacion).ToString("N", nfi));
                strHtml.Append("</tr>");
            }
            else
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td colspan='4'>NO SE ENCONTRO DATOS</td>");
                strHtml.Append("</tr>");
            }
            #endregion

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// GetGraficoDiagramaCarga
        /// </summary>
        /// <param name="listaReporteAgrupCtg"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoDiagramaCarga(List<MeMedicion48DTO> listaReporteAgrupCtg)
        {
            decimal? valor;
            List<MeMedicion48DTO> listaReporte = listaReporteAgrupCtg.OrderBy(x => x.Orden).ToList();

            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            for (int i = 0; i < listaReporte.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].Ctgdetnomb;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].Fenercolor;
                List<DatosSerie> listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    valor = (decimal?)listaReporte[i].GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(listaReporte[i], null);
                    listadata.Add(new DatosSerie() { Y = valor });
                }
                regSerie.Data = listadata;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = " DIAGRAMA DE CARGA POR TIPO DE RECURSO";
            if (listaReporte.Count > 0)
            {
                grafico.YaxixTitle = "MW";
                grafico.XAxisCategories = new List<string>();
                grafico.SeriesType = new List<string>();
                grafico.SeriesYAxis = new List<int>();
            }

            return grafico;
        }

        /// <summary>
        /// OrdenarListaM48DiagramaDuracion
        /// </summary>
        /// <param name="listaReporteAgrupCtg"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> OrdenarListaM48DiagramaDuracion(List<MeMedicion48DTO> listaReporteAgrupCtg)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            //armar total por media hora

            var reg48Totalizado = (from t in listaReporteAgrupCtg
                                   group t by new { t.Medifecha, }
                                      into destino
                                   select new MeMedicion48DTO()
                                   {
                                       Medifecha = destino.Key.Medifecha,
                                       H1 = destino.Sum(t => t.H1),
                                       H2 = destino.Sum(t => t.H2),
                                       H3 = destino.Sum(t => t.H3),
                                       H4 = destino.Sum(t => t.H4),
                                       H5 = destino.Sum(t => t.H5),
                                       H6 = destino.Sum(t => t.H6),
                                       H7 = destino.Sum(t => t.H7),
                                       H8 = destino.Sum(t => t.H8),
                                       H9 = destino.Sum(t => t.H9),
                                       H10 = destino.Sum(t => t.H10),
                                       H11 = destino.Sum(t => t.H11),
                                       H12 = destino.Sum(t => t.H12),
                                       H13 = destino.Sum(t => t.H13),
                                       H14 = destino.Sum(t => t.H14),
                                       H15 = destino.Sum(t => t.H15),
                                       H16 = destino.Sum(t => t.H16),
                                       H17 = destino.Sum(t => t.H17),
                                       H18 = destino.Sum(t => t.H18),
                                       H19 = destino.Sum(t => t.H19),
                                       H20 = destino.Sum(t => t.H20),
                                       H21 = destino.Sum(t => t.H21),
                                       H22 = destino.Sum(t => t.H22),
                                       H23 = destino.Sum(t => t.H23),
                                       H24 = destino.Sum(t => t.H24),
                                       H25 = destino.Sum(t => t.H25),
                                       H26 = destino.Sum(t => t.H26),
                                       H27 = destino.Sum(t => t.H27),
                                       H28 = destino.Sum(t => t.H28),
                                       H29 = destino.Sum(t => t.H29),
                                       H30 = destino.Sum(t => t.H30),
                                       H31 = destino.Sum(t => t.H31),
                                       H32 = destino.Sum(t => t.H32),
                                       H33 = destino.Sum(t => t.H33),
                                       H34 = destino.Sum(t => t.H34),
                                       H35 = destino.Sum(t => t.H35),
                                       H36 = destino.Sum(t => t.H36),
                                       H37 = destino.Sum(t => t.H37),
                                       H38 = destino.Sum(t => t.H38),
                                       H39 = destino.Sum(t => t.H39),
                                       H40 = destino.Sum(t => t.H40),
                                       H41 = destino.Sum(t => t.H41),
                                       H42 = destino.Sum(t => t.H42),
                                       H43 = destino.Sum(t => t.H43),
                                       H44 = destino.Sum(t => t.H44),
                                       H45 = destino.Sum(t => t.H45),
                                       H46 = destino.Sum(t => t.H46),
                                       H47 = destino.Sum(t => t.H47),
                                       H48 = destino.Sum(t => t.H48),
                                       Meditotal = destino.Sum(t => t.Meditotal)
                                   }).ToList().FirstOrDefault() ?? new MeMedicion48DTO();


            List<GenericoDTO> listaDuracion = new List<GenericoDTO>();
            for (int j = 1; j <= 48; j++)
            {
                decimal? valor = (decimal?)reg48Totalizado.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(reg48Totalizado, null);
                listaDuracion.Add(new GenericoDTO() { Entero2 = j, Decimal1 = valor });
            }

            listaDuracion = listaDuracion.OrderByDescending(x => x.Decimal1).ToList();

            //cambiar orden de los datos
            foreach (var reg48Agrup in listaReporteAgrupCtg)
            {
                MeMedicion48DTO reg48Dur = new MeMedicion48DTO()
                {
                    Medifecha = reg48Agrup.Medifecha,
                    Fenergcodi = reg48Agrup.Fenergcodi,
                    Ctgdetcodi = reg48Agrup.Ctgdetcodi,
                    Fenercolor = reg48Agrup.Fenercolor,
                    Ctgdetnomb = reg48Agrup.Ctgdetnomb,
                };

                decimal total = 0;
                for (int i = 1; i <= 48; i++)
                {
                    int posDuracion = listaDuracion[i - 1].Entero2.Value;
                    decimal? valorDuracion = (decimal?)reg48Agrup.GetType().GetProperty(ConstantesAppServicio.CaracterH + posDuracion).GetValue(reg48Agrup, null);

                    reg48Dur.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(reg48Dur, valorDuracion);
                    total += valorDuracion.GetValueOrDefault(0);
                }
                reg48Dur.Meditotal = total;

                listaFinal.Add(reg48Dur);
            }

            //Camisea, Malacas y Aguaytia
            MeMedicion48DTO reg48DurGasTmp = listaFinal.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas);
            MeMedicion48DTO reg48DurGas = null;
            if (reg48DurGasTmp != null)
            {
                reg48DurGas = new MeMedicion48DTO() { Medifecha = reg48DurGasTmp.Medifecha, Ctgdetnomb = "GAS", Fenergcodi = reg48DurGasTmp.Fenergcodi, Fenercolor = reg48DurGasTmp.Fenercolor };
                SetMeditotalXLista(reg48DurGas, listaFinal.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas).ToList());
            }

            //nuevo final
            listaFinal = listaFinal.Where(x => x.Fenergcodi != ConstantesPR5ReportesServicio.FenergcodiGas).ToList();
            if (reg48DurGas != null) listaFinal.Add(reg48DurGas);

            return listaFinal;
        }

        /// <summary>
        /// GetGraficoDiagramaDuracion
        /// </summary>
        /// <param name="listaReporteAgrupCtg"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoDiagramaDuracion(List<MeMedicion48DTO> listaReporteAgrupCtg)
        {
            List<MeMedicion48DTO> listaReporte = listaReporteAgrupCtg.OrderByDescending(x => x.Meditotal).ToList();

            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            for (int i = 1; i <= 48; i++)
            {
                grafico.SeriesName.Add((i - 1).ToString());
            }

            for (int i = 0; i < listaReporte.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].Ctgdetnomb;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].Fenercolor;
                List<DatosSerie> listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    decimal? valor = (decimal?)listaReporte[i].GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(listaReporte[i], null);
                    listadata.Add(new DatosSerie() { Y = valor });
                }
                regSerie.Data = listadata;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = "DIAGRAMA DE DURACIÓN DE CARGA DEL COES";
            if (listaReporte.Count > 0)
            {
                grafico.YaxixTitle = "MW";
                grafico.XAxisCategories = new List<string>();
                grafico.SeriesType = new List<string>();
                grafico.SeriesYAxis = new List<int>();
            }

            return grafico;
        }

        /// <summary>
        /// Genera vista html de reporte potencia por tipo de recurso
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="Result"></param>
        /// <param name="listaGrupoData"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaTipoGen"></param>
        /// <returns></returns>
        public static string ListaReporteGeneracionElectricaCentralesRERHtml(DateTime fechaIni, List<MeMedicion48DTO> Result, List<PrGrupoDTO> listaGrupoData,
                                            List<MeMedicion48DTO> dataVersion, List<SiTipogeneracionDTO> listaTipoGen)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();

            StringBuilder strHtml = new StringBuilder();

            if (Result.Count == 0) //mostrar cabecera del reporte cuando no exista dataos
            {
                strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
                strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px'>", 500);
                strHtml.Append("<thead>");
                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:70px;' rowspan=2>HORA</th>");
                strHtml.Append("<th class='titulo_rer' rowspan=2>GENERACIÓN ELÉCTRICA DE LAS CENTRALES RER (MW)</th>");
                strHtml.Append("<th style='width:70px;'>TOTAL<br/>RER</th>");
                strHtml.Append("</tr>");
                strHtml.Append("<tr>");
                strHtml.Append("<th>MW</th>");
                strHtml.Append("</tr>");
                strHtml.Append("</thead>");
                strHtml.Append("<tbody>");
                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
                strHtml.Append("</div>");

                return strHtml.ToString();
            }

            //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

            var total = 200 + Result.Count * 124;
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px'>", total);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;' rowspan=3>HORA</th>");
            strHtml.AppendFormat("<th colspan='{0}' class='titulo_rer'>GENERACIÓN ELÉCTRICA DE LAS CENTRALES RER (MW)</th>", Result.Count);
            strHtml.Append("<th style='width:70px;' rowspan=2>TOTAL<br/>RER</th>");
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            //empresas
            var listaEmpr = listaGrupoData.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            foreach (var item in listaEmpr)
            {
                var listaDataXEmpr = listaGrupoData.Where(x => x.Emprcodi == item.Emprcodi).ToList();
                strHtml.AppendFormat("<th  style='word-wrap: break-word; white-space: normal;min-width:150px;' colspan='{0}' class='empresa_rer'>" + item.Emprnomb + "</th>", listaDataXEmpr.Count);
            }
            strHtml.Append("</tr>");

            //centrales
            strHtml.Append("<tr>");
            foreach (var grupo in listaGrupoData)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px;' class='grupo_rer'>" + grupo.Gruponomb + "</th>");
            }
            strHtml.Append("<th>MW</th>");
            strHtml.Append("</tr>");


            strHtml.Append("</thead>");
            // ****************************************  CUERPO DE LA TABLA ******************************************************         
            strHtml.Append("<tbody>");
            decimal? valorVersi = null;
            DateTime horas = DateTime.Now.Date;
            decimal? valor;
            decimal acumuladoH = 0;
            for (int h = 1; h <= 48; h++)
            {
                acumuladoH = 0;
                horas = horas.AddMinutes(30);
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='tdbody_reporte_fecha'>{0:HH:mm}</td>", horas);

                string _bground = string.Empty, descrip = string.Empty;

                foreach (var grupo in listaGrupoData)
                {
                    MeMedicion48DTO item = Result.Find(x => x.Medifecha == fechaIni && x.Grupocodi == grupo.Grupocodi);
                    MeMedicion48DTO datVersi = null;

                    descrip = string.Empty;
                    valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(item, null);
                    if (datVersi != null)
                    {
                        valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(datVersi, null);
                    }
                    if (valor.GetValueOrDefault(0) > 0)
                    {
                        descrip = ((decimal)valor).ToString("N", nfi);
                        if (valorVersi != null)
                        {
                            if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                        }
                        acumuladoH += valor.GetValueOrDefault(0);
                    }
                    if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; acumuladoH += valorVersi.Value; }
                    strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);
                }

                strHtml.AppendFormat("<td>{0}</td>", (acumuladoH).ToString("N", nfi));

                strHtml.Append("</tr>");
            }

            decimal acumuladoTotal = 0;
            foreach (var item in Result)
            {
                decimal? acumulado = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                    if (valor != null)
                    {
                        acumulado = acumulado == null ? valor : acumulado + valor;
                    }
                }

                item.Meditotal = acumulado.GetValueOrDefault(0) / (decimal)2;
            }

            foreach (var item in Result)
            {
                acumuladoTotal += item.Meditotal.GetValueOrDefault(0);
            }

            //totales
            strHtml.Append("<tr>");
            strHtml.Append("<td class='totalTipoRecurso'>EJEC <br/> MWh</td>");

            foreach (var grupo in listaGrupoData)
            {
                MeMedicion48DTO item = Result.Find(x => x.Medifecha == fechaIni && x.Grupocodi == grupo.Grupocodi);
                if (item.Meditotal != null)
                    strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", ((decimal)item.Meditotal).ToString("N", nfi));
                else
                    strHtml.AppendFormat("<td class='totalTipoRecurso'>0,000</td>");
            }
            strHtml.AppendFormat("<td class='totalTipoRecurso'>{0}</td>", (acumuladoTotal).ToString("N", nfi));
            strHtml.Append("</tr>");

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol1"></param>
        /// <param name="ncol2"></param>
        /// <param name="ncol3"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="numRecursos"></param>
        /// <param name="lista"></param>
        /// <param name="lista2"></param>
        /// <param name="listaVersion1"></param>
        /// <param name="listaVersion2"></param>
        public static void GeneraRptPotenciaXTipoRecursoTotal(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, ref int nfil, ref int ncol1, ref int ncol2, ref int ncol3, int tipoGrafico, out int numRecursos
                    , List<MeMedicion48DTO> lista, List<MeMedicion48DTO> lista2, List<MeMedicion48DTO> listaVersion1, List<MeMedicion48DTO> listaVersion2)
        {
            int filaTab1;
            int coluFinalTab1;
            int filaFinalTab1;
            int coluTab1;

            GeneraRptPotenciaXTipoRecurso(ws, fecha1, fecha2, lista, listaVersion1, out filaTab1, out coluTab1, out filaFinalTab1, out coluFinalTab1, out numRecursos);

            GeneraRptPotenciaXTipoHidro(ws, lista2, filaTab1, coluFinalTab1 + 5);

            nfil = filaFinalTab1;
            ncol1 = filaTab1;
            ncol2 = coluTab1;

            #region Escala Y freezer
            ws.View.FreezePanes(7, 3);
            ws.View.ZoomScale = 70;
            #endregion

        }

        /// <summary>
        /// Genera el listado en excel del reporte Potencia por tipo recursos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion1"></param>
        /// <param name="filaTab1"></param>
        /// <param name="coluTab1"></param>
        /// <param name="filaFinalTab1"></param>
        /// <param name="coluFinalTab1"></param>
        /// <param name="numRecursos"></param>
        public static void GeneraRptPotenciaXTipoRecurso(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, List<MeMedicion48DTO> lista, List<MeMedicion48DTO> listaVersion1,
                                                out int filaTab1, out int coluTab1, out int filaFinalTab1, out int coluFinalTab1, out int numRecursos)
        {

            numRecursos = 0;
            List<MeMedicion48DTO> Result = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> dataVersion = new List<MeMedicion48DTO>();

            Result = lista;
            dataVersion = listaVersion1;
            filaTab1 = 0;
            coluTab1 = 0;
            filaFinalTab1 = 0;
            coluFinalTab1 = 0;

            int filaIniHora = 6;
            int coluIniHora = 2;
            ws.Cells[filaIniHora - 1, coluIniHora].Value = "REPORTE POTENCIA GENERADA POR TIPO DE RECURSO";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora - 1, coluIniHora, filaIniHora - 1, coluIniHora, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora - 1, coluIniHora, filaIniHora - 1, coluIniHora);

            int ultimaColumna1 = 0;
            int ultimaFila1 = 0;

            coluTab1 = coluIniHora;
            #region Cabecera

            //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

            ws.Cells[filaIniHora, coluIniHora].Value = "HORA";
            int col = 0;
            foreach (var item in Result)
            {
                ws.Cells[filaIniHora, coluIniHora + 1 + col].Value = "\n" + item.Ctgdetnomb.TrimEnd() + "\n";
                col++;
            }
            numRecursos = col - 1;
            ws.Cells[filaIniHora, coluIniHora + 1 + col].Value = "TOTAL";

            ultimaColumna1 = coluIniHora + 1 + col;

            #region Formato Cabecera

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1, "Centro");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1, "Arial", 8);
            UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora, coluIniHora, filaIniHora, ultimaColumna1);
            ws.Column(coluIniHora).Width = 17;
            int m = 0;
            foreach (var item in Result)
            {
                UtilExcel.CeldasExcelWrapText(ws, filaIniHora, coluIniHora + 1 + m);
                ws.Column(coluIniHora + 1 + m).Width = 17.5;
                m++;
            }

            ws.Column(coluIniHora + 1 + m).Width = 17;
            #endregion

            #endregion

            #region Cuerpo
            // ****************************************  CUERPO DE LA TABLA ******************************************************      
            decimal? valorVersi = null;
            DateTime horas = DateTime.Now.Date;
            int j = 1;
            decimal? valor;
            decimal acumuladoH = 0;
            for (int i = 0; i < 48; i++)
            {
                acumuladoH = 0;
                horas = horas.AddMinutes(30);
                ws.Cells[filaIniHora + 1 + j - 1, coluIniHora].Value = string.Format("{0:HH:mm}", horas);

                decimal descrip = 0;
                int colR = 0;
                foreach (var item in Result)
                {
                    MeMedicion48DTO datVersi = null;
                    descrip = 0.00m;
                    valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(item, null);
                    if (datVersi != null)
                    {
                        valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(datVersi, null);
                    }
                    if (valor != null)
                    {
                        descrip = ((decimal)valor);
                        if (valorVersi != null)
                        {
                            if (valorVersi != valor)
                            {
                                descrip = ((decimal)valorVersi);
                            }
                        }
                    }
                    if (valorVersi != null && valor == null)
                    {
                        descrip = ((decimal)valorVersi);
                    }
                    ws.Cells[filaIniHora + 1 + i, coluIniHora + 1 + colR].Value = descrip;
                    acumuladoH = acumuladoH + descrip;
                    ws.Cells[filaIniHora + 1 + i, coluIniHora + 1 + colR].Style.Numberformat.Format = "#,##0.00";
                    colR++;
                }
                ws.Cells[filaIniHora + 1 + i, coluIniHora + 1 + colR].Value = acumuladoH;
                ws.Cells[filaIniHora + 1 + i, coluIniHora + 1 + colR].Style.Numberformat.Format = "#,##0.00";


                j++;
            }



            decimal acumuladoTotal = 0;
            foreach (var item in Result)
            {
                decimal? acumulado = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                    if (valor != null)
                    {
                        acumulado = acumulado == null ? valor : acumulado + valor;
                    }
                }

                item.Meditotal = acumulado.GetValueOrDefault(0) / (decimal)2;
            }

            foreach (var item in Result)
            {
                acumuladoTotal += item.Meditotal.GetValueOrDefault(0);
            }

            #region Formato Cuerpo

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 1, coluIniHora, filaIniHora + 48, coluIniHora, "Arial", 10);

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 1, coluIniHora + 1, filaIniHora + 48, ultimaColumna1, "Arial", 8);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniHora + 1, coluIniHora, filaIniHora + 48, ultimaColumna1, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 1, coluIniHora, filaIniHora + 48, coluIniHora);

            UtilExcel.BorderCeldas3(ws, filaIniHora + 1, coluIniHora, filaIniHora + 8, ultimaColumna1);
            UtilExcel.BorderCeldas3(ws, filaIniHora + 9, coluIniHora, filaIniHora + 16, ultimaColumna1);
            UtilExcel.BorderCeldas3(ws, filaIniHora + 17, coluIniHora, filaIniHora + 24, ultimaColumna1);
            UtilExcel.BorderCeldas3(ws, filaIniHora + 25, coluIniHora, filaIniHora + 32, ultimaColumna1);
            UtilExcel.BorderCeldas3(ws, filaIniHora + 33, coluIniHora, filaIniHora + 40, ultimaColumna1);
            UtilExcel.BorderCeldas3(ws, filaIniHora + 41, coluIniHora, filaIniHora + 48, ultimaColumna1);

            #endregion

            #endregion

            #region Totales

            //totales

            ws.Cells[filaIniHora + 1 + 48, coluIniHora].Value = "TOTAL MWh";
            int colTot = 0;
            foreach (var item in Result)
            {
                if (item.Meditotal != null)
                    ws.Cells[filaIniHora + 1 + 48, coluIniHora + 1 + colTot].Value = (decimal)item.Meditotal;
                else
                    ws.Cells[filaIniHora + 1 + 48, coluIniHora + 1 + colTot].Value = 0.000;
                ws.Cells[filaIniHora + 1 + 48, coluIniHora + 1 + colTot].Style.Numberformat.Format = "#,##0.00";
                colTot++;
            }
            ws.Cells[filaIniHora + 1 + 48, coluIniHora + 1 + colTot].Value = acumuladoTotal;
            ws.Cells[filaIniHora + 1 + 48, coluIniHora + 1 + colTot].Style.Numberformat.Format = "#,##0.00";
            ws.Cells[filaIniHora + 1 + 49, coluIniHora].Value = "%";
            int colPorc = 0;
            foreach (var item in Result)
            {
                decimal porcentaje = 0;
                if (acumuladoTotal > 0)
                {
                    porcentaje = item.Meditotal.GetValueOrDefault(0) * 100 / acumuladoTotal;
                    ws.Cells[filaIniHora + 1 + 49, coluIniHora + 1 + colPorc].Value = (decimal)porcentaje;
                }
                else
                {
                    ws.Cells[filaIniHora + 1 + 49, coluIniHora + 1 + colPorc].Value = 0.000;
                }
                ws.Cells[filaIniHora + 1 + 49, coluIniHora + 1 + colPorc].Style.Numberformat.Format = "#,##0.00";
                colPorc++;
            }
            ws.Cells[filaIniHora + 1 + 49, coluIniHora + 1 + colPorc].Value = "100.00";
            ultimaFila1 = filaIniHora + 1 + 49;
            #region Formato Totales

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 49, coluIniHora, filaIniHora + 50, coluIniHora, "Arial", 10);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 49, coluIniHora + 1, ultimaFila1, ultimaColumna1, "Arial", 9);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniHora + 49, coluIniHora, ultimaFila1, ultimaColumna1, "Centro");
            UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 49, coluIniHora, ultimaFila1, ultimaColumna1, "#DCE6F1");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 49, coluIniHora, ultimaFila1, ultimaColumna1);
            UtilExcel.BorderCeldas2(ws, filaIniHora + 49, coluIniHora, ultimaFila1, ultimaColumna1);
            #endregion

            #endregion

            filaTab1 = filaIniHora;
            coluFinalTab1 = ultimaColumna1;
            filaFinalTab1 = ultimaFila1;
        }

        /// <summary>
        /// GeneraRptPotenciaXTipoHidro
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="data"></param>
        /// <param name="filaInicial"></param>
        /// <param name="ColumnaInicial"></param>
        public static void GeneraRptPotenciaXTipoHidro(ExcelWorksheet ws, List<MeMedicion48DTO> data, int filaInicial, int ColumnaInicial)
        {
            int filaIniEmpresa = filaInicial;
            int coluIniEmpresa = ColumnaInicial;

            int ultimaColumna2 = 0;
            int ultimaFila2 = 0;

            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            ws.Cells[filaIniEmpresa, coluIniEmpresa].Value = "EMPRESA";
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = "CENTRAL";
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 2].Value = "\n HIDROELECTRICA DE PASADA \n";
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 3].Value = "\n HIDROELECTRICA DE REGULACION \n";

            #region Formato Cabecera
            ws.Column(coluIniEmpresa).Width = 58;
            ws.Column(coluIniEmpresa + 1).Width = 25;
            ws.Column(coluIniEmpresa + 2).Width = 22;
            ws.Column(coluIniEmpresa + 3).Width = 22;
            ultimaColumna2 = coluIniEmpresa + 3;

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2, "Centro");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2, "Arial", 9);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2);
            UtilExcel.CeldasExcelColorFondo(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelColorTexto(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2, "#FFFFFF");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa, ultimaColumna2);

            for (int i = 0; i < 4; i++)
                UtilExcel.CeldasExcelWrapText(ws, filaIniEmpresa, coluIniEmpresa + i);

            #endregion

            #endregion

            #region cuerpo_reporte
            // **************  CUERPO DE LA TABLA **************//
            var listaEmpresa = data.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);
            var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            if (listaEmprcodi.Count > 0)
            {
                decimal totalPasada = 0;
                decimal totalRegulacion = 0;

                int ultima = 0;
                int n = 0;


                foreach (var empcodi in listaEmprcodi)
                {
                    int iFila = 0;
                    int uFila = 0;
                    //empresa
                    string nomEmpresa = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                    ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa].Value = nomEmpresa;
                    iFila = filaIniEmpresa + 1 + ultima;
                    //central
                    var listaCentral = data.Where(x => x.Emprcodi == empcodi).Select(y => new { y.Equipadre, y.Central }).Distinct().ToList().OrderBy(c => c.Central);
                    var listaCentralcodi = listaCentral.Select(x => x.Equipadre).ToList();
                    for (int centralN = 0; centralN < listaCentralcodi.Count; centralN++)
                    {
                        var centralcodi = listaCentralcodi[centralN];
                        var nomCentral = data.Where(x => x.Emprcodi == empcodi && x.Equipadre == centralcodi).First().Central;
                        ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 1].Value = nomCentral;

                        //Pasada / Regulacion
                        decimal? valor = null, acumulado = null;
                        var listaData = data.Where(x => x.Equipadre == centralcodi && x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaPasada).ToList();
                        for (int i = 1; i <= 48; i++)
                        {
                            foreach (var item in listaData)
                            {
                                valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                                if (valor != null)
                                {
                                    acumulado = acumulado == null ? valor : acumulado.GetValueOrDefault(0) + valor;
                                }
                            }
                        }

                        if (acumulado != null)
                        {
                            totalPasada += (decimal)acumulado / 2;
                            ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 2].Value = (decimal)acumulado / 2;
                        }
                        else
                        {
                            ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 2].Value = "";
                        }
                        ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 2].Style.Numberformat.Format = "#,##0.0";

                        // Regulacion
                        valor = null;
                        acumulado = null;
                        listaData = data.Where(x => x.Equipadre == centralcodi && x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoAguaRegulacion).ToList();
                        for (int i = 1; i <= 48; i++)
                        {
                            foreach (var item in listaData)
                            {
                                valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                                if (valor != null)
                                {
                                    acumulado = acumulado == null ? valor : acumulado.GetValueOrDefault(0) + valor;
                                }
                            }
                        }

                        if (acumulado != null)
                        {
                            totalRegulacion += (decimal)acumulado / 2;
                            ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 3].Value = (decimal)acumulado / 2;
                        }
                        else
                            ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 3].Value = "";
                        ws.Cells[filaIniEmpresa + 1 + ultima + centralN, coluIniEmpresa + 3].Style.Numberformat.Format = "#,##0.0";
                        n = centralN;
                    }
                    ultima = ultima + n + 1;
                    uFila = filaIniEmpresa + 1 + ultima - 1;
                    UtilExcel.CeldasExcelAgrupar(ws, iFila, coluIniEmpresa, uFila, coluIniEmpresa);
                }


                ultimaFila2 = filaIniEmpresa + 1 + ultima;
                ////totales
                ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa].Value = "TOTAL MWh";
                ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa + 2].Value = totalPasada;
                ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa + 2].Style.Numberformat.Format = "#,##0.0";
                ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa + 3].Value = totalRegulacion;
                ws.Cells[filaIniEmpresa + 1 + ultima, coluIniEmpresa + 3].Style.Numberformat.Format = "#,##0.0";

            }
            else
            {

            }

            #region Formato Cuerpo
            if (filaIniEmpresa + 1 > ultimaFila2 - 1) ultimaFila2 = filaIniEmpresa + 2;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa + 1, coluIniEmpresa, ultimaFila2 - 1, coluIniEmpresa, "Arial", 9);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa + 1, coluIniEmpresa + 1, ultimaFila2 - 1, coluIniEmpresa + 1, "Arial", 8);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa + 1, coluIniEmpresa + 2, ultimaFila2 - 1, ultimaColumna2, "Arial", 10);
            UtilExcel.CeldasExcelColorTexto(ws, filaIniEmpresa + 1, coluIniEmpresa, ultimaFila2 - 1, coluIniEmpresa + 1, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, filaIniEmpresa + 1, coluIniEmpresa, ultimaFila2 - 1, coluIniEmpresa + 1, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniEmpresa + 1, coluIniEmpresa, ultimaFila2 - 1, ultimaColumna2, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniEmpresa + 1, coluIniEmpresa + 2, ultimaFila2 - 1, ultimaColumna2, "Centro");

            UtilExcel.CeldasExcelColorFondo(ws, ultimaFila2, coluIniEmpresa, ultimaFila2, ultimaColumna2, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelColorTexto(ws, ultimaFila2, coluIniEmpresa, ultimaFila2, ultimaColumna2, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila2, coluIniEmpresa, ultimaFila2, coluIniEmpresa, "Arial", 8);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila2, coluIniEmpresa + 1, ultimaFila2, ultimaColumna2, "Arial", 10);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, ultimaFila2, coluIniEmpresa, ultimaFila2, ultimaColumna2, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila2, coluIniEmpresa, ultimaFila2, ultimaColumna2);
            #endregion


            #endregion

            UtilExcel.BorderCeldas2(ws, filaIniEmpresa, coluIniEmpresa, ultimaFila2, ultimaColumna2);
        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>        
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        /// <param name="numRecursos"></param>
        public static void AddGraficoPotenciaXTipoRecursoTotal(ExcelWorksheet ws, string xAxisTitle, string yAxisTitle, string titulo, int rep,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie, int numRecursos)
        {

            string nameGraf = string.Empty;
            int numSeries = 0;
            switch (rep)
            {
                case 1: nameGraf = "grafPotenciaXTipoRecursoTotal1"; numSeries = numRecursos; break;
            }
            var AreaChart = ws.Drawings.AddChart(nameGraf, eChartType.AreaStacked);
            AreaChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);
            AreaChart.SetSize(1000, 450);

            for (int sn = 0; sn < numSeries; sn++)
            {
                var ran1 = ws.Cells[filaIniSerie, coluIniSerie + sn, filaIniSerie + 47, coluIniSerie + sn];
                var ran2 = ws.Cells[filaIniXSerie, coluIniXSerie, filaIniXSerie + 47, coluIniXSerie];

                var serie = (ExcelChartSerie)AreaChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[filaIniSerie - 1, coluIniSerie + sn].Value.ToString();
            }


            AreaChart.Title.Text = titulo;
            AreaChart.Title.Font.Bold = true;
            AreaChart.YAxis.Title.Text = yAxisTitle;

            AreaChart.Legend.Position = eLegendPosition.Bottom;


        }

        /// <summary>
        ///  Genera grafico tipo Pie en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        /// <param name="numRecursos"></param>
        public static void AddGraficoPiePotenciaXTipoRecursoTotal(ExcelWorksheet ws, string xAxisTitle, string yAxisTitle, string titulo, int rep,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie, int numRecursos)
        {
            if (filaIniSerie >= 0)
            {
                string nameGraf = string.Empty;
                int numSeries = 0;
                switch (rep)
                {
                    case 1: nameGraf = "grafPotenciaXTipoRecursoTotal2"; numSeries = numRecursos; break;
                }
                var PieChart = ws.Drawings.AddChart(nameGraf, eChartType.PieExploded3D) as ExcelPieChart;
                PieChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);

                PieChart.SetSize(1100, 450);


                var ran1 = ws.Cells[filaIniSerie, coluIniSerie, filaIniSerie, coluIniSerie + numRecursos - 1]; //serie
                var ran2 = ws.Cells[filaIniXSerie, coluIniXSerie, filaIniXSerie, coluIniXSerie + numRecursos - 1]; //xserie

                var serie = (ExcelChartSerie)PieChart.Series.Add(ran1, ran2);


                PieChart.Title.Text = titulo;
                PieChart.Title.Font.Bold = true;

                PieChart.View3D.RotX = 30;
                PieChart.View3D.RotY = 100;

                PieChart.DataLabel.ShowCategory = true;
                PieChart.DataLabel.ShowPercent = true;
                PieChart.DataLabel.ShowLeaderLines = true;
                PieChart.DataLabel.ShowLegendKey = true;
                PieChart.DataLabel.Separator = ":";
                PieChart.Legend.Remove();
            }
        }

        /// <summary>
        /// GeneraRptGeneracionElectricaCentralesRERTotal
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="titulo"></param>
        /// <param name="total"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol1"></param>
        /// <param name="nfil2"></param>
        /// <param name="ncol2"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="numCentrales"></param>
        /// <param name="listaY"></param>
        /// <param name="lista"></param>
        /// <param name="lista2"></param>
        /// <param name="listaVersion1"></param>
        /// <param name="listaVersion2"></param>
        /// <param name="listaTipoGen"></param>
        /// <param name="listaGrupoData"></param>
        public static void GeneraRptGeneracionElectricaCentralesRERTotal(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, string titulo, string total,
                ref int nfil, ref int ncol1, ref int nfil2, ref int ncol2, int tipoGrafico, out int numCentrales, out List<MeMedicion48DTO> listaY
            , List<MeMedicion48DTO> lista, List<MeMedicion48DTO> lista2, List<MeMedicion48DTO> listaVersion1, List<MeMedicion48DTO> listaVersion2, List<SiTipogeneracionDTO> listaTipoGen, List<PrGrupoDTO> listaGrupoData)
        {
            listaY = new List<MeMedicion48DTO>();
            int filaTab1;
            int coluTab1;
            int filaFinalTab1;

            GeneraRptGeneracionElectricaCentralesRER(ws, fecha1, fecha2, titulo, total, lista, listaVersion1, listaTipoGen, listaGrupoData, out filaTab1, out coluTab1, out filaFinalTab1, out numCentrales);

            nfil = filaTab1;
            nfil2 = filaFinalTab1;
            ncol1 = coluTab1;
            ncol2 = numCentrales;
            listaY = lista2;
        }

        /// <summary>
        /// GeneraRptGeneracionElectricaCentralesRER
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="titulo"></param>
        /// <param name="total"></param>
        /// <param name="Result"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaTipoGen"></param>
        /// <param name="listaGrupoData"></param>
        /// <param name="filaTab1"></param>
        /// <param name="coluTab1"></param>
        /// <param name="filaFinalTab1"></param>
        /// <param name="numCentrales"></param>
        public static void GeneraRptGeneracionElectricaCentralesRER(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, string titulo, string total, List<MeMedicion48DTO> Result, List<MeMedicion48DTO> dataVersion
                , List<SiTipogeneracionDTO> listaTipoGen, List<PrGrupoDTO> listaGrupoData, out int filaTab1, out int coluTab1, out int filaFinalTab1, out int numCentrales)
        {
            int filaIniCabecera = 5;
            int coluIniCabecera = 2;

            int ultimaColumna = 0;
            int antepenultimaFila = 0;
            int ultimaFila = 0;

            if (Result.Count > 0)
            {

                #region Cabecera

                //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

                ws.Cells[filaIniCabecera, coluIniCabecera].Value = titulo;
                ws.Cells[filaIniCabecera + 1, coluIniCabecera].Value = "HORA";
                int espacio = 0;
                int inicio = 0;
                int final = 0;

                var listaEmpr = listaGrupoData.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
                foreach (var item in listaEmpr)
                {
                    inicio = coluIniCabecera + 1 + espacio;
                    var listaDataXEmpr = listaGrupoData.Where(x => x.Emprcodi == item.Emprcodi).ToList();
                    ws.Cells[filaIniCabecera + 1, coluIniCabecera + 1 + espacio].Value = item.Emprnomb;

                    espacio = espacio + listaDataXEmpr.Count;
                    final = coluIniCabecera + 1 + espacio - 1;
                    UtilExcel.CeldasExcelWrapText(ws, filaIniCabecera + 1, inicio);
                    UtilExcel.CeldasExcelAgrupar(ws, filaIniCabecera + 1, inicio, filaIniCabecera + 1, final);
                }
                ultimaColumna = coluIniCabecera + 1 + espacio;
                ws.Cells[filaIniCabecera + 1, coluIniCabecera + 1 + espacio].Value = total;
                int col = 0;
                foreach (var grupo in listaGrupoData)
                {
                    ws.Cells[filaIniCabecera + 2, coluIniCabecera + 1 + col].Value = (grupo.Gruponomb != null ? grupo.Gruponomb.Trim() : string.Empty);
                    col++;
                }
                ws.Cells[filaIniCabecera + 2, ultimaColumna].Value = "MW";

                #region Formato Cabecera

                ws.Column(coluIniCabecera).Width = 15;
                for (int i = coluIniCabecera + 1; i <= ultimaColumna; i++)
                {
                    ws.Column(i).Width = 20;
                }
                ws.Row(filaIniCabecera + 1).Height = 44;

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera, ultimaColumna, "Arial", 16);
                UtilExcel.CeldasExcelAgrupar(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera, ultimaColumna);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera, ultimaColumna, "Izquierda");

                UtilExcel.CeldasExcelAgrupar(ws, filaIniCabecera + 1, coluIniCabecera, filaIniCabecera + 2, coluIniCabecera);

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCabecera + 1, coluIniCabecera, filaIniCabecera + 2, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCabecera + 1, coluIniCabecera, filaIniCabecera + 2, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCabecera + 1, coluIniCabecera, filaIniCabecera + 2, ultimaColumna, "Arial", 8);

                ws.Cells[filaIniCabecera + 1, coluIniCabecera, filaIniCabecera + 1, ultimaColumna].Style.WrapText = true;
                ws.Cells[filaIniCabecera + 2, coluIniCabecera, filaIniCabecera + 2, ultimaColumna].Style.WrapText = true;

                UtilExcel.CeldasExcelColorTexto(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera + 2, ultimaColumna, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera + 2, ultimaColumna, ConstantesPR5ReportesServicio.ColorInfSGI);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera + 2, ultimaColumna);

                UtilExcel.BorderCeldas2(ws, filaIniCabecera, coluIniCabecera, filaIniCabecera + 2, ultimaColumna);

                #endregion

                #endregion

                #region Cuerpo

                // ****************************************  CUERPO DE LA TABLA ******************************************************     

                decimal? valorVersi = null;
                DateTime horas = DateTime.Now.Date;
                int j = 1;
                decimal? valor;
                decimal acumuladoH = 0;
                for (int i = 0; i < 48; i++)
                {
                    acumuladoH = 0;
                    horas = horas.AddMinutes(30);
                    ws.Cells[filaIniCabecera + 3 + i, coluIniCabecera].Value = string.Format("{0:H:mm}", horas);

                    decimal? descrip;
                    int colR = 0;

                    foreach (var grupo in listaGrupoData)
                    {
                        MeMedicion48DTO item = Result.Find(x => x.Medifecha == fecha1 && x.Grupocodi == grupo.Grupocodi);
                        MeMedicion48DTO datVersi = null;
                        descrip = null;
                        valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(item, null);
                        if (datVersi != null)
                        {
                            valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(datVersi, null);
                        }
                        if (valor != null)
                        {
                            descrip = ((decimal)valor);
                            if (valorVersi != null)
                            {
                                if (valorVersi != valor)
                                {
                                    descrip = ((decimal)valorVersi);
                                }
                            }
                        }
                        if (valorVersi != null && valor == null)
                        {
                            descrip = ((decimal)valorVersi);
                            acumuladoH += valorVersi.Value;
                        }
                        if (descrip.GetValueOrDefault(0) != 0) ws.Cells[filaIniCabecera + 3 + i, coluIniCabecera + 1 + colR].Value = descrip;
                        acumuladoH = acumuladoH + descrip.GetValueOrDefault(0);
                        ws.Cells[filaIniCabecera + 3 + i, coluIniCabecera + 1 + colR].Style.Numberformat.Format = "#,##0.00";
                        colR++;

                    }

                    ws.Cells[filaIniCabecera + 3 + i, ultimaColumna].Value = acumuladoH;
                    ws.Cells[filaIniCabecera + 3 + i, ultimaColumna].Style.Numberformat.Format = "#,##0.00";

                    j++;
                    antepenultimaFila = filaIniCabecera + 3 + i;
                }
                ultimaFila = antepenultimaFila + 1;
                decimal acumuladoTotal = 0;
                foreach (var item in Result)
                {
                    decimal? acumulado = null;
                    for (int i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                        if (valor != null)
                        {
                            acumulado = acumulado == null ? valor : acumulado + valor;
                        }
                    }

                    item.Meditotal = acumulado.GetValueOrDefault(0) / (decimal)2;
                }

                foreach (var item in Result)
                {
                    acumuladoTotal += item.Meditotal.GetValueOrDefault(0);
                }

                //totales

                ws.Cells[ultimaFila, coluIniCabecera].Value = "EJEC MWh";
                int colTot = 0;
                foreach (var grupo in listaGrupoData)
                {
                    MeMedicion48DTO item = Result.Find(x => x.Medifecha == fecha1 && x.Grupocodi == grupo.Grupocodi);
                    if (item.Meditotal != null)
                        ws.Cells[ultimaFila, coluIniCabecera + 1 + colTot].Value = ((decimal)item.Meditotal);
                    else
                        ws.Cells[ultimaFila, coluIniCabecera + 1 + colTot].Value = 0.000;
                    ws.Cells[ultimaFila, coluIniCabecera + 1 + colTot].Style.Numberformat.Format = "#,##0.00";
                    colTot++;
                }
                ws.Cells[ultimaFila, ultimaColumna].Value = acumuladoTotal;
                ws.Cells[ultimaFila, ultimaColumna].Style.Numberformat.Format = "#,##0.00";

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCabecera + 3, coluIniCabecera, antepenultimaFila, ultimaColumna, "Arial", 10);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCabecera + 3, coluIniCabecera, antepenultimaFila, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCabecera + 3, coluIniCabecera, antepenultimaFila, coluIniCabecera);
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCabecera + 3, ultimaColumna, antepenultimaFila, ultimaColumna, "#D9E1F2");

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna, "Arial", 12);
                ws.Cells[ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna].Style.WrapText = true;
                UtilExcel.CeldasExcelEnNegrita(ws, ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelColorFondo(ws, ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna, "#D6DCE4");

                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 1, coluIniCabecera, filaIniCabecera + 2 + 8, ultimaColumna);
                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 9, coluIniCabecera, filaIniCabecera + 2 + 16, ultimaColumna);
                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 17, coluIniCabecera, filaIniCabecera + 2 + 24, ultimaColumna);
                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 25, coluIniCabecera, filaIniCabecera + 2 + 32, ultimaColumna);
                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 33, coluIniCabecera, filaIniCabecera + 2 + 40, ultimaColumna);
                UtilExcel.BorderCeldas3(ws, filaIniCabecera + 2 + 41, coluIniCabecera, filaIniCabecera + 2 + 48, ultimaColumna);

                UtilExcel.BorderCeldas2(ws, ultimaFila, coluIniCabecera, ultimaFila, ultimaColumna);
                #endregion

                #endregion

            }
            filaTab1 = filaIniCabecera;
            coluTab1 = coluIniCabecera;
            filaFinalTab1 = ultimaFila;
            numCentrales = ultimaColumna - coluIniCabecera - 1;

            ws.View.FreezePanes(8, 3);

            ws.View.ZoomScale = 70;
        }

        /// <summary>
        /// Genera grafico tipo Pie en archivo excel del reporte Gneracion Electrica de centrales RER
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        /// <param name="numCentrales"></param>
        public static void AddGraficoPieGeneracionElectricaCentralesRER(ExcelWorksheet ws, List<MeMedicion48DTO> listaReporte, string xAxisTitle, string yAxisTitle, string titulo,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie, int numCentrales)
        {
            int filaIniData = 70;
            int coluIniData = 12;

            decimal totalRER;
            CrearTablaGeneracionRERXTipoGeneracion(ws, listaReporte, filaIniData, coluIniData, out totalRER);


            string nameGraf = string.Empty;
            int numSeries = listaReporte.Count;

            nameGraf = "grafPieGeneracionElectricaRER";

            var PieChart = ws.Drawings.AddChart(nameGraf, eChartType.PieExploded3D) as ExcelPieChart;
            PieChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);

            PieChart.SetSize(800, 580);

            var ran1 = ws.Cells[filaIniData, coluIniData + 1, filaIniData + numSeries - 1, coluIniData + 1]; //serie
            var ran2 = ws.Cells[filaIniData, coluIniData, filaIniData + numSeries - 1, coluIniData]; //xserie

            var serie = (ExcelChartSerie)PieChart.Series.Add(ran1, ran2);


            PieChart.Title.Text = titulo + "\n\n" + "TOTAL RER : " + Decimal.Round(totalRER, 3);
            PieChart.Title.Font.Bold = true;

            PieChart.View3D.RotX = 30;
            PieChart.View3D.RotY = 100;

            PieChart.DataLabel.Font.Size = 11;
            PieChart.DataLabel.Font.Bold = true;
            PieChart.DataLabel.ShowCategory = true;
            PieChart.DataLabel.ShowPercent = true;
            PieChart.DataLabel.ShowLeaderLines = true;
            PieChart.Legend.Remove();
        }

        /// <summary>
        /// Genera una tabla en el Excel con los datos que usaremos en el grafico GENERACION RER POR TIPO DE GENERACION DEL SEIN
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaReporte"></param>
        /// <param name="filaIniTabla"></param>
        /// <param name="coluIniTabla"></param>
        /// <param name="totalRER"></param>
        public static void CrearTablaGeneracionRERXTipoGeneracion(ExcelWorksheet ws, List<MeMedicion48DTO> listaReporte, int filaIniTabla, int coluIniTabla, out decimal totalRER)
        {
            listaReporte = listaReporte.OrderBy(x => x.Meditotal).ToList();
            for (int i = 0; i < listaReporte.Count; i++)
            {
                decimal valor = 0;
                ws.Cells[filaIniTabla + i, coluIniTabla].Value = listaReporte[i].Tgenernomb;
                ws.Cells[filaIniTabla + i, coluIniTabla + 3].Value = listaReporte[i].Tgenercolor;
                valor += ((decimal)listaReporte[i].Meditotal / 2);
                ws.Cells[filaIniTabla + i, coluIniTabla + 1].Value = valor;
            }
            decimal total = 0;
            for (int i = 0; i < listaReporte.Count; i++)
            {
                total = total + (decimal)ws.Cells[filaIniTabla + i, coluIniTabla + 1].Value;
            }

            //asignar porcentaje
            for (int i = 0; i < listaReporte.Count; i++)
            {
                if (total != 0)
                    ws.Cells[filaIniTabla + i, coluIniTabla + 2].Value = (decimal)ws.Cells[filaIniTabla + i, coluIniTabla + 1].Value / total * 100;
            }
            totalRER = total;
        }

        /// <summary>
        /// Genera grafico tipo Barras en archivo excel del reporte Gneracion Electrica de centrales RER
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        /// <param name="numCentrales"></param>
        public static void AddGraficoGeneracionElectricaCentralesRER(ExcelWorksheet ws, string xAxisTitle, string yAxisTitle, string titulo,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie, int numCentrales)
        {

            string nameGraf = string.Empty;
            nameGraf = "grafGenElecCentralesRER";

            var BarChart = ws.Drawings.AddChart(nameGraf, eChartType.BarClustered) as ExcelBarChart;
            BarChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);

            BarChart.SetSize(800, 1400);

            var ran1 = ws.Cells[filaIniSerie, coluIniSerie, filaIniSerie, coluIniSerie + numCentrales - 1]; //serie
            var ran2 = ws.Cells[filaIniXSerie, coluIniXSerie, filaIniXSerie, coluIniXSerie + numCentrales - 1]; //xserie

            var serie = (ExcelBarChartSerie)BarChart.Series.Add(ran1, ran2);
            serie.DataLabel.ShowValue = true;
            serie.DataLabel.Font.Bold = true;
            serie.DataLabel.Position = eLabelPosition.InEnd;

            BarChart.Title.Text = titulo;
            BarChart.Title.Font.Bold = true;
            BarChart.Series[0].Fill.Color = Color.OrangeRed;

            BarChart.YAxis.Title.Text = yAxisTitle;
            BarChart.YAxis.Title.Font.Size = 10;
            BarChart.YAxis.Title.Font.Bold = true;

            BarChart.XAxis.Title.Text = xAxisTitle;
            BarChart.XAxis.Title.Font.Size = 10;
            BarChart.XAxis.Title.Font.Bold = true;

            BarChart.Legend.Remove();
        }

        #endregion

        // 3.13.2.8.	Evolución de la producción de energía diaria.
        #region REPORTE_PRODUCCION_ENERGIA_DIARIA

        /// <summary>
        /// Genera vista html del reporte de la produccion de energia diaria
        /// </summary>
        /// <param name="tipoIntegrante"></param>
        /// <param name="data"></param>
        /// <param name="fechaIniFiltro"></param>
        /// <param name="fechaFinFiltro"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteProduccionEnergiaDiariaHtml(string tipoIntegrante, List<MePtomedicionDTO> listaPto48, List<MeMedicion48DTO> data, DateTime fechaIniFiltro, DateTime fechaFinFiltro, List<MeMedicion48DTO> dataVersion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();

            StringBuilder strHtml = new StringBuilder(), strTrDia = new StringBuilder();

            DateTime fIniMesFiltro = new DateTime(fechaIniFiltro.Year, fechaIniFiltro.Month, 1);
            DateTime fFinMesFiltro = fechaFinFiltro.AddMonths(1).AddDays(-1);
            int totalDias = Convert.ToInt32(fFinMesFiltro.Subtract(fIniMesFiltro).TotalDays) + 1;

            string titulo = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoIntegrante ? "EVOLUCIÓN DE LA PRODUCCIÓN DE LA ENERGÍA DIARIA DE LOS INTEGRANTES DEL COES (MWh)" : "EVOLUCIÓN DE LA PRODUCCIÓN DE LA ENERGÍA DIARIA DE LOS NO INTEGRANTES DEL COES (MWh)";
            string strTotal = ConstantesPR5ReportesServicio.EmpresacoesSi == tipoIntegrante ? "TOTAL INTEGRANTES COES (MWh)" : "TOTAL NO COES (MWh)";
            var anchodias = (totalDias * (70 + 20)) + 230 + 230 + 150;

            strHtml.AppendFormat("<h3>{0}</h3>", titulo);
            strHtml.AppendFormat("<div class='freeze_table' id='resultado_{0}' style='height: auto;'>", tipoIntegrante);
            strHtml.AppendFormat("<table id='reporte_{1}' class='pretty tabla-icono tablaProdGen' style='table-layout: fixed; width: {0}px'>", anchodias, tipoIntegrante);

            strHtml.Append("<thead>");
            #region cabecera

            //meses
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:230px;' rowspan='2'>EMPRESA</th>");
            strHtml.Append("<th style='width:150px;' rowspan='2'>CENTRAL</th>");
            strHtml.Append("<th style='width:150px;' rowspan='2'>UNIDAD</th>");
            strHtml.Append("<th style='width:110px;' rowspan='2'>TIPO DE <br> GENERACIÓN</th>");

            DateTime fechaTmp = fechaIniFiltro;

            do
            {
                DateTime fIniMes = fechaTmp;
                DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                int totalDiaMes = Convert.ToInt32(fFinMes.Subtract(fIniMes).TotalDays) + 1;
                string nombreMes = EPDate.f_NombreMes(fIniMes.Month);

                string classMes = "prodgen_mes_" + (fIniMes.Month % 2);
                strHtml.AppendFormat("<th class='{3}' style='word-wrap: break-word; white-space: normal;width:{0}px;' colspan='{1}'>{2}</th>", ((totalDiaMes + 1) * 90), totalDiaMes + 1, nombreMes, classMes);

                for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                {
                    strTrDia.AppendFormat("<th class='{1}' style='word-wrap: break-word; white-space: normal;width90px;'>{0}</th>", f.Day, classMes);
                }
                strTrDia.AppendFormat("<th class='{0}' style='word-wrap: break-word; white-space: normal;width:90px;'>TOTAL</th>", classMes);

                fechaTmp = fFinMes.AddDays(1);
            } while (fechaTmp < fechaFinFiltro);

            strHtml.Append("</tr>");

            //días
            strHtml.Append("<tr>");
            strHtml.Append(strTrDia);
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            if (listaPto48.Count() > 0)
            {
                foreach (var obj in listaPto48)
                {
                    var dataXPto = data.Where(x => x.Ptomedicodi == obj.Ptomedicodi && x.Emprcodi == obj.Emprcodi).ToList();

                    strHtml.Append("<tr>");

                    strHtml.AppendFormat("<td class='unidad'>{0}</td>", obj.Emprnomb);
                    strHtml.AppendFormat("<td class='unidad'>{0}</td>", obj.Central);
                    strHtml.AppendFormat("<td class='unidad'>{0}</td>", obj.Equinomb);
                    strHtml.AppendFormat("<td class='tgener'>{0}</td>", obj.Tgenernomb);

                    fechaTmp = fechaIniFiltro;

                    do
                    {
                        DateTime fIniMes = fechaTmp;
                        DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                        fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                        var dataXUnidadXMes = dataXPto.Where(x => x.Medifecha >= fIniMes && x.Medifecha <= fFinMes).ToList();

                        for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                        {
                            var item = dataXUnidadXMes.Find(x => x.Medifecha == f);

                            string valorDiaXUnidad = (item != null && item.Meditotal != null) ? item.Meditotal.Value.ToString("N", nfi) : string.Empty;
                            strHtml.AppendFormat("<td class='valor_num'>{0}</td>", valorDiaXUnidad);
                        }

                        decimal? totalXUnidadXMes = dataXUnidadXMes.Sum(x => x.Meditotal);
                        string valorMesXUnidad = (totalXUnidadXMes != null) ? totalXUnidadXMes.Value.ToString("N", nfi) : string.Empty;

                        strHtml.AppendFormat("<td class='valor_num'>{0}</td>", valorMesXUnidad);

                        fechaTmp = fFinMes.AddDays(1);
                    } while (fechaTmp < fechaFinFiltro);

                    strHtml.Append("</tr>");
                }
            }

            //Totales
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<td class='total'>{0}</td>", strTotal);
            strHtml.Append("<td class='total'></td>");
            strHtml.Append("<td class='total'></td>");
            strHtml.Append("<td class='total'></td>");

            fechaTmp = fechaIniFiltro;
            do
            {
                DateTime fIniMes = fechaTmp;
                DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                var dataRangoDia = data.Where(x => x.Medifecha >= fIniMes && x.Medifecha <= fFinMes).ToList();
                var totalxDia = dataRangoDia.GroupBy(x => new { x.Medifecha }).Select(x => new MeMedicion48DTO
                {
                    Meditotal = x.Sum(p => p.Meditotal),
                    Medifecha = x.Key.Medifecha
                }).OrderBy(x => x.Medifecha).ToList();

                for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                {
                    decimal valor = 0;
                    var regTotalXDia = totalxDia.Find(x => x.Medifecha == f);
                    if (regTotalXDia != null)
                        valor = regTotalXDia.Meditotal.GetValueOrDefault(0);

                    strHtml.AppendFormat("<td class='total'>{0}</td>", valor.ToString("N", nfi));
                }

                decimal? totalXMes = dataRangoDia.Sum(x => x.Meditotal);
                string valorMes = (totalXMes != null) ? totalXMes.Value.ToString("N", nfi) : string.Empty;

                strHtml.AppendFormat("<td class='total'>{0}</td>", valorMes);

                fechaTmp = fFinMes.AddDays(1);
            } while (fechaTmp < fechaFinFiltro);

            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// LLama alas funciones que armarán el Reporte Evolución de la producción de energía diaria.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="listaDC"></param>
        /// <param name="listaDNC"></param>
        /// <param name="listaVC"></param>
        /// <param name="listaVNC"></param>
        public static void GeneraRptTodoEvoProdEnDiaria(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2,
            List<MeMedicion48DTO> listaDC, List<MeMedicion48DTO> listaDNC, List<MePtomedicionDTO> listaPto48Coes, List<MePtomedicionDTO> listaPto48NoCoes, List<MeMedicion48DTO> listaVC, List<MeMedicion48DTO> listaVNC)
        {
            DateTime fIniMesFiltro = new DateTime(fecha1.Year, fecha1.Month, 1);
            DateTime fFinMesFiltro = fecha2.AddMonths(1).AddDays(-1);
            int totalDias = Convert.ToInt32(fFinMesFiltro.Subtract(fIniMesFiltro).TotalDays) + 1;

            int filaFinData = rowIni;
            #region tabla1

            ws.View.FreezePanes((rowIni + 2) + 1, (colIni + 3) + 1);

            //para los integrantes de COES
            GenerarExcelEvoProdEnDiariaXGrupoIntegrante(ws, fecha1, fecha2, "EVOLUCIÓN DE LA PRODUCCIÓN DE LA ENERGÍA DIARIA DE LOS INTEGRANTES DEL COES (MWh)", "TOTAL INTEGRANTES COES (MWh)",
                listaPto48Coes, listaDC, listaVC, rowIni, colIni, ref filaFinData);

            #endregion

            #region  tabla2

            //para los NO integrantes de COES
            GenerarExcelEvoProdEnDiariaXGrupoIntegrante(ws, fecha1, fecha2, "EVOLUCIÓN DE LA PRODUCCIÓN DE LA ENERGÍA DIARIA DE LOS NO INTEGRANTES DEL COES (MWh)", "TOTAL NO COES (MWh)",
                listaPto48NoCoes, listaDNC, listaVNC, filaFinData + 3, colIni, ref filaFinData);

            #endregion       

            GenerarGraficoExcelEvoProdEnDiaria(ws, listaDC, listaVC, filaFinData + 3, colIni + 5);

            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// Genera el listado (tabla) del Reporte Evolución de la producción de energía diaria.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fechaIniFiltro"></param>
        /// <param name="fechaFinFiltro"></param>
        /// <param name="titulo"></param>
        /// <param name="descTotal"></param>
        /// <param name="listaDC"></param>
        /// <param name="listaVC"></param>
        /// <param name="filaTitulo1"></param>
        /// <param name="coluTitulo1"></param>
        /// <param name="filaFinData"></param>
        public static void GenerarExcelEvoProdEnDiariaXGrupoIntegrante(ExcelWorksheet ws, DateTime fechaIniFiltro, DateTime fechaFinFiltro, string titulo, string descTotal,
             List<MePtomedicionDTO> listaPto48, List<MeMedicion48DTO> listaDC, List<MeMedicion48DTO> listaVC, int filaTitulo1, int coluTitulo1, ref int filaFinData)
        {
            List<MeMedicion48DTO> listaVersion = listaVC;
            List<MeMedicion48DTO> data = listaDC;

            DateTime fIniMesFiltro = new DateTime(fechaIniFiltro.Year, fechaIniFiltro.Month, 1);
            DateTime fFinMesFiltro = fechaFinFiltro.AddMonths(1).AddDays(-1);
            int totalDias = Convert.ToInt32(fechaFinFiltro.Subtract(fechaIniFiltro).TotalDays) + 1;

            int filaEmpresas = filaTitulo1 + 1;
            int coluEmpresas = coluTitulo1;

            int filaCentral = filaEmpresas;
            int coluCentral = coluEmpresas + 1;

            int filaUnidad = filaCentral;
            int coluUniidad = coluCentral + 1;

            int filaTipo = filaUnidad;
            int coluTipo = coluUniidad + 1;

            ws.Cells[filaTitulo1, coluTitulo1].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaTitulo1, coluTitulo1, filaTitulo1, coluTitulo1, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, filaTitulo1, coluTitulo1, filaTitulo1, coluTitulo1);

            ws.Cells[filaEmpresas, coluEmpresas, filaEmpresas + 1, coluEmpresas].Merge = true;
            ws.Cells[filaCentral, coluCentral, filaCentral + 1, coluCentral].Merge = true;
            ws.Cells[filaUnidad, coluUniidad, filaUnidad + 1, coluUniidad].Merge = true;
            ws.Cells[filaTipo, coluTipo, filaTipo + 1, coluTipo].Merge = true;

            ws.Cells[filaEmpresas, coluEmpresas].Value = "EMPRESA";
            ws.Cells[filaCentral, coluCentral].Value = "CENTRAL";
            ws.Cells[filaUnidad, coluUniidad].Value = "UNIDAD";
            ws.Cells[filaTipo, coluTipo].Value = "TIPO";
            int filaMes = filaTipo;
            int colDiaIni = coluTipo + 1;
            int colDia = colDiaIni;
            int filaDia = filaMes + 1;

            DateTime fechaTmp = fechaIniFiltro;

            do
            {
                DateTime fIniMes = fechaTmp;
                DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                int totalDiaMes = Convert.ToInt32(fFinMes.Subtract(fIniMes).TotalDays) + 1;
                string nombreMes = EPDate.f_NombreMes(fIniMes.Month);

                ws.Cells[filaMes, colDia].Value = nombreMes;
                string colorMes = (fIniMes.Month % 2 == 0) ? "#5b39ad" : ConstantesPR5ReportesServicio.ColorInfSGI;
                ws.Cells[filaMes, colDia, filaMes, colDia + totalDiaMes].Merge = true;
                UtilExcel.CeldasExcelColorFondoYBorder(ws, filaEmpresas, colDia, filaDia, colDia + totalDiaMes, ColorTranslator.FromHtml(colorMes), Color.White);

                for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                {
                    ws.Cells[filaDia, colDia].Value = f.Day;
                    ws.Column(colDia).Width = 13;
                    colDia++;
                }
                ws.Cells[filaDia, colDia].Value = "TOTAL";
                ws.Column(colDia).Width = 21;
                colDia++;

                fechaTmp = fFinMes.AddDays(1);
            } while (fechaTmp < fechaFinFiltro);

            int colDiaFin = colDia > colDiaIni ? colDia - 1 : colDiaIni;

            #region Formato Cabecera

            UtilExcel.CeldasExcelColorTexto(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaEmpresas, coluEmpresas, filaDia, colDiaFin);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, filaEmpresas, coluEmpresas, filaDia, coluTipo, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #region cuerpo

            int rowFilaData = filaEmpresas + 2;
            int filaX = 0;
            int filaFin = rowFilaData - 1;
            int j = 0;
            fechaTmp = fechaIniFiltro;

            if (listaPto48.Count() > 0)
            {
                foreach (var obj in listaPto48)
                {
                    var dataXPto = data.Where(x => x.Ptomedicodi == obj.Ptomedicodi && x.Emprcodi == obj.Emprcodi).ToList();

                    ws.Cells[rowFilaData + filaX, coluEmpresas].Value = obj.Emprnomb;
                    ws.Cells[rowFilaData + filaX, coluCentral].Value = obj.Central;
                    ws.Cells[rowFilaData + filaX, coluUniidad].Value = obj.Equinomb;
                    ws.Cells[rowFilaData + filaX, coluTipo].Value = obj.Tgenernomb;

                    j = 1;
                    fechaTmp = fechaIniFiltro;

                    do
                    {
                        DateTime fIniMes = fechaTmp;
                        DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                        fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                        var dataXUnidadXMes = dataXPto.Where(x => x.Medifecha >= fIniMes && x.Medifecha <= fFinMes).ToList();

                        for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                        {
                            var item = dataXUnidadXMes.Find(x => x.Medifecha == f);

                            decimal? valorDiaXUnidad = (item != null && item.Meditotal != null) ? item.Meditotal : null;
                            ws.Cells[rowFilaData + filaX, coluTipo + j].Value = valorDiaXUnidad;
                            ws.Cells[rowFilaData + filaX, coluTipo + j].Style.Numberformat.Format = "#,##0.00";
                            j++;
                        }

                        decimal? totalXUnidadXMes = dataXUnidadXMes.Sum(x => x.Meditotal);

                        ws.Cells[rowFilaData + filaX, coluTipo + j].Value = totalXUnidadXMes;
                        ws.Cells[rowFilaData + filaX, coluTipo + j].Style.Numberformat.Format = "#,##0.00";

                        j++;
                        fechaTmp = fFinMes.AddDays(1);
                    } while (fechaTmp < fechaFinFiltro);

                    filaX++;
                }

                filaFin = rowFilaData + filaX - 1;
                #region Formato cuerpo

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowFilaData, coluEmpresas, filaFin, colDiaFin, "Arial", 11);

                UtilExcel.CeldasExcelWrapText(ws, rowFilaData, coluEmpresas, filaFin, colDiaFin);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowFilaData, coluEmpresas, filaFin, coluUniidad, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowFilaData, coluTipo, filaFin, coluTipo, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowFilaData, coluTipo + 1, filaFin, colDiaFin, "Derecha");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowFilaData, coluEmpresas, filaFin, colDiaFin, "Centro");

                #endregion
            }

            //Totales
            j = 1;
            int filaTotal = filaFin + 1;
            ws.Cells[filaTotal, coluEmpresas].Value = descTotal;
            ws.Cells[filaTotal, coluEmpresas, filaTotal, coluTipo].Merge = true;

            fechaTmp = fechaIniFiltro;
            do
            {
                DateTime fIniMes = fechaTmp;
                DateTime fFinMes = new DateTime(fechaTmp.Year, fechaTmp.Month, 1).AddMonths(1).AddDays(-1);
                fFinMes = fFinMes > fechaFinFiltro ? fechaFinFiltro : fFinMes;

                var dataRangoDia = data.Where(x => x.Medifecha >= fIniMes && x.Medifecha <= fFinMes).ToList();
                var totalxDia = dataRangoDia.GroupBy(x => new { x.Medifecha }).Select(x => new MeMedicion48DTO
                {
                    Meditotal = x.Sum(p => p.Meditotal),
                    Medifecha = x.Key.Medifecha
                }).OrderBy(x => x.Medifecha).ToList();

                for (var f = fIniMes; f <= fFinMes; f = f.AddDays(1))
                {
                    decimal valor = 0;
                    var regTotalXDia = totalxDia.Find(x => x.Medifecha == f);
                    if (regTotalXDia != null)
                        valor = regTotalXDia.Meditotal.GetValueOrDefault(0);

                    ws.Cells[filaTotal, coluTipo + j].Value = valor;
                    ws.Cells[filaTotal, coluTipo + j].Style.Numberformat.Format = "#,##0.00";
                    j++;
                }

                decimal? totalXMes = dataRangoDia.Sum(x => x.Meditotal);
                ws.Cells[filaTotal, coluTipo + j].Value = totalXMes;
                ws.Cells[filaTotal, coluTipo + j].Style.Numberformat.Format = "#,##0.00";
                j++;

                fechaTmp = fFinMes.AddDays(1);
            } while (fechaTmp < fechaFinFiltro);

            #region Formato cuerpo

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaTotal, coluEmpresas, filaTotal, colDiaFin, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, filaTotal, coluEmpresas, filaTotal, colDiaFin);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaTotal, coluEmpresas, filaTotal, coluTipo, "Izquierda");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaTotal, coluTipo + 1, filaTotal, colDiaFin, "Derecha");
            UtilExcel.CeldasExcelEnNegrita(ws, filaTotal, coluEmpresas, filaTotal, colDiaFin);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, filaTotal, coluEmpresas, filaTotal, coluTipo, Color.FromArgb(184, 226, 251), Color.Black);

            UtilExcel.BorderCeldas2(ws, rowFilaData, coluEmpresas, filaTotal, colDiaFin);

            #endregion

            filaFinData = filaEmpresas + filaX + 2;

            #endregion

            //tamaños de las celdas
            ws.Row(filaEmpresas).Height = 23;
            ws.Row(filaDia).Height = 23;

            ws.Column(coluEmpresas).Width = 55;
            ws.Column(coluCentral).Width = 35;
            ws.Column(coluUniidad).Width = 35;
            ws.Column(coluTipo).Width = 30;
        }

        /// <summary>
        /// Genera el gráfico excel del Reporte Evolución de la producción de energía diaria.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaDC"></param>
        /// <param name="listaVC"></param>
        /// <param name="filaIniListado"></param>
        /// <param name="coluIniListado"></param>
        public static void GenerarGraficoExcelEvoProdEnDiaria(ExcelWorksheet ws, List<MeMedicion48DTO> listaDC, List<MeMedicion48DTO> listaVC, int filaIniListado, int coluIniListado)
        {
            List<MeMedicion48DTO> listaVersion = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            listaVersion = listaVC;
            lista = listaDC;

            if (listaVersion.Count > 0)
            {
                var temp = lista;
                var tempVersi = listaVersion;

                lista = tempVersi;
                listaVersion = temp;
            }

            var totalesxempresa = lista.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new MeMedicion48DTO
            {
                Meditotal = x.Sum(p => p.Meditotal),
                Emprnomb = x.Key.Emprnomb.TrimEnd(),
                Emprcodi = x.Key.Emprcodi

            }).OrderByDescending(x => x.Meditotal).ToList();


            int filaIniData = filaIniListado + 2;
            int coluIniData = coluIniListado + 2;

            int numEmpresas = totalesxempresa.Count;
            for (int empr = 0; empr < numEmpresas; empr++)
            {
                ws.Cells[filaIniData + empr, coluIniData].Value = totalesxempresa[empr].Emprnomb;
                ws.Cells[filaIniData + empr, coluIniData + 1].Value = totalesxempresa[empr].Meditotal;
            }
            UtilExcel.CeldasExcelColorTexto(ws, filaIniData, coluIniData, filaIniData + numEmpresas, coluIniData + 1, "#FFFFFF"); //"ocultamos" tabla
            UtilExcel.CeldasExcelColorFondo(ws, filaIniData, coluIniData, filaIniData + numEmpresas, coluIniData + 1, "#FFFFFF"); //"ocultamos" tabla

            string nameGraf = string.Empty;


            nameGraf = "grafProdEnergiaDiaria";


            var ChartBar = ws.Drawings.AddChart(nameGraf, eChartType.BarClustered) as ExcelBarChart;
            ChartBar.SetPosition(filaIniListado + 6, 0, 2, 0);

            ChartBar.SetSize(900, 1400);


            var ran1 = ws.Cells[filaIniData, coluIniData + 1, filaIniData + numEmpresas - 1, coluIniData + 1];
            var ran2 = ws.Cells[filaIniData, coluIniData, filaIniData + numEmpresas - 1, coluIniData];

            var serie = (ExcelChartSerie)ChartBar.Series.Add(ran1, ran2);


            ChartBar.Title.Text = "GENERACIÓN DE ENERGÍA EJECUTADA POR EMPRESAS INTEGRANTES COES (MWh)";
            ChartBar.DataLabel.ShowLeaderLines = true;
            ChartBar.YAxis.Title.Text = "MWh";
            ChartBar.YAxis.Title.Font.Size = 10;
            ChartBar.YAxis.Title.Font.Bold = true;
            ChartBar.XAxis.Orientation = eAxisOrientation.MaxMin;
            ChartBar.VaryColors = true;
            ChartBar.Legend.Remove();
        }

        #endregion

        // 3.13.2.9.	Máxima generación instantánea del SEIN (MW).
        #region REPORTE_MAX_GENERACION_DEL_SEIN

        /// <summary>
        /// Genera vista html de reporte generacion del SEIN
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteMaxGeneracionInstSEINHtml(string url, List<MeMedicion48DTO> data, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            if (data.Count() > 0)
            {
                string[] listaIdTipoGeneracion = ConstantesPR5ReportesServicio.TipoFteEnergiaTodos.Split(',');
                List<SiTipogeneracionDTO> listaTipoGen = FactorySic.GetSiTipogeneracionRepository().List().Where(x => listaIdTipoGeneracion.Contains(x.Tgenercodi.ToString())).ToList().OrderBy(x => x.Tgenercodi).ToList();

                int padding = 20;
                int anchoTotal = (100 + padding) + 20 * (200 + padding);

                strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
                strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);

                strHtml.Append("<thead>");
                #region cabecera
                //***************************      CABECERA DE LA TABLA         ***********************************//

                //Fila 1
                strHtml.Append("<tr>");
                strHtml.Append("<th rowspan='4' style='width: 110px;'>HORA</th>");
                strHtml.Append("<th rowspan='1' colspan='16'>RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN</th>");
                strHtml.Append("<th rowspan='4'>HORA</th>");
                strHtml.Append("<th rowspan='1' colspan='5' style='background: #1F497D !important;'>GENERACIÓN POR TIPO DE GENERACIÓN</th>");
                strHtml.Append("<th rowspan='4'>HORA</th>");
                strHtml.Append("<th rowspan='1' colspan='5' style='background: #31869B !important;'>GENERACIÓN POR TIPO DE GENERACIÓN</th>");
                strHtml.Append("</tr>");

                //Fila 2
                var listaArea = data.Select(x => new { x.Areanomb, x.Areacodi }).Where(x => x.Areacodi != 0).Distinct().OrderBy(x => x.Areacodi).ToList();
                strHtml.Append("<tr>");
                foreach (var area in listaArea)
                {
                    strHtml.AppendFormat("<th rowspan='1' colspan='4' style='background: #B8CCE4 !important; color: black;'>{0}</th>", area.Areanomb);
                }
                strHtml.Append("<th rowspan='1' colspan='4'>GENERACIÓN INTEGRANTES COES</th>");
                strHtml.Append("<th rowspan='1' colspan='5' style='background: #1F497D !important;'>INTEGRANTES COES</th>");
                strHtml.Append("<th rowspan='1' colspan='5' style='background: #31869B !important;'>INTEGRANTES COES RER</th>");
                strHtml.Append("</tr>");

                //Fila 3
                strHtml.Append("<tr>");
                for (int i = 1; i <= 12; i++)
                {
                    MeMedicion48DTO m48pto = data.Find(x => x.Orden == i);
                    string codPto = m48pto.Ptomedicodi > 0 ? m48pto.Ptomedicodi.ToString() : "";
                    string btnConfigCalculado = string.Format("<a href='#' onclick='verPuntoCalculado({0})' title='Ver puntos de medición del Calculado'><img src='{1}Content/Images/file.png' style='padding-top:7px; padding-right: 15px;'></a>", codPto, url);
                    strHtml.AppendFormat("<th style='background: #B8CCE4 !important; color: black; word-wrap: break-word; white-space: normal;width: 150px'>{0} {1}</th>", codPto, btnConfigCalculado);
                }

                foreach (var area in listaArea)
                {
                    strHtml.AppendFormat("<th rowspan='2'>Generación COES  <br/> {0}</th>", area.Areanomb);
                }
                strHtml.Append("<th rowspan='2'>TOTAL GENERACIÓN COES</th>");

                foreach (var tipogen in listaTipoGen)
                {
                    strHtml.AppendFormat("<th rowspan='2' style='background: #1F497D !important;  word-wrap: break-word; white-space: normal;width: 150px'>{0}</th>", tipogen.Tgenernomb);
                }
                strHtml.Append("<th rowspan='2' style='background: #1F497D !important;'>TOTAL GENERACIÓN COES</th>");

                foreach (var tipogen in listaTipoGen)
                {
                    strHtml.AppendFormat("<th rowspan='2' style='background: #31869B !important; word-wrap: break-word; white-space: normal;width: 150px'>{0}</th>", tipogen.Tgenernomb);
                }
                strHtml.Append("<th rowspan='2' style='background: #31869B !important;'>TOTAL RER</th>");
                strHtml.Append("</tr>");

                //Fila 4
                strHtml.Append("<tr>");
                foreach (var area in listaArea)
                {
                    foreach (var tipogen in listaTipoGen)
                    {
                        strHtml.AppendFormat("<th rowspan='1' style='background: #B8CCE4 !important; color: black; word-wrap: break-word; white-space: normal;width: 150px'>GENERACIÓN <br/> {0} <br> {1} </th>", tipogen.Tgenernomb, area.Areanomb);
                    }
                }
                strHtml.Append("</tr>");

                #endregion
                strHtml.Append("</thead>");

                strHtml.Append("<tbody>");
                #region cuerpo
                //***************************      CUERPO DE LA TABLA         ***********************************//
                NumberFormatInfo nfi2 = GenerarNumberFormatInfo3();
                nfi2.NumberGroupSeparator = " ";
                nfi2.NumberDecimalDigits = 2;
                nfi2.NumberDecimalSeparator = ",";

                DateTime horas = DateTime.Now.Date;
                MeMedicion48DTO m48 = null;
                decimal? valor;
                decimal? valorVersi = null;
                int totalData = data.Count;

                for (int h = 1; h <= 48; h++)
                {
                    horas = horas.AddMinutes(30);

                    strHtml.Append("<tr>");

                    for (int i = 1; i <= totalData; i++)
                    {
                        string _bground = string.Empty, descrip = string.Empty;
                        string border = h % 8 == 0 ? "border-bottom-width: medium" : string.Empty;
                        if (i == 1 || i == 17 || i == 22)
                        {
                            strHtml.AppendFormat("<td class='tdbody_reporte'>{0:HH:mm}</td>", horas);
                        }

                        m48 = data.Find(x => x.Orden == i);
                        var datVersi = dataVersion.Count > 0 ? dataVersion[i - 1] : null;
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        if (datVersi != null)
                        {
                            valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(datVersi, null);
                        }
                        if (valor != null)
                        {
                            descrip = ((decimal)valor).ToString("N", nfi2);
                            if (valorVersi != null)
                            {
                                if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi2); _bground = "lightgreen"; }
                            }
                        }
                        if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi2); _bground = "lightgreen"; }
                        strHtml.AppendFormat("<td style='{1};background:" + _bground + "'>{0}</td>", descrip, border);
                    }

                    strHtml.Append("</tr>");
                }

                //total
                strHtml.Append("<tr>");

                for (int i = 1; i <= totalData; i++)
                {
                    if (i == 1 || i == 17 || i == 22)
                    {
                        strHtml.Append("<td class='tdbody_reporte'>TOTAL MWh</td>");
                    }

                    m48 = data.Find(x => x.Orden == i);
                    var datVersi = dataVersion.Count > 0 ? dataVersion[i - 1] : null;
                    if (datVersi != null)
                    {
                        strHtml.AppendFormat("<td>{0}</td>", ((decimal)datVersi.Meditotal.Value / 2).ToString("N", nfi2));
                    }
                    else
                    {
                        strHtml.AppendFormat("<td>{0}</td>", ((decimal)m48.Meditotal.Value / 2).ToString("N", nfi2));
                    }
                }

                strHtml.Append("</tr>");

                #endregion
                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
                strHtml.Append("</div>");
            }
            else
            {
                strHtml.Append("¡No existen datos para mostrar!");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="version"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol1"></param>
        /// <param name="ncol2"></param>
        /// <param name="ncol3"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptGeneracionSEIN(ExcelWorksheet ws, string version, DateTime fecha1, DateTime fecha2, ref int nfil, ref int ncol1, ref int ncol2, ref int ncol3, int tipoGrafico
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion)
        {
            //set la lista de areas a usar
            if (data.Count > 0)
            {
                string[] listaIdTipoGeneracion = ConstantesPR5ReportesServicio.TipoFteEnergiaTodos.Split(',');
                List<SiTipogeneracionDTO> listaTipoGen = FactorySic.GetSiTipogeneracionRepository().List().Where(x => listaIdTipoGeneracion.Contains(x.Tgenercodi.ToString())).ToList().OrderBy(x => x.Tgenercodi).ToList();

                var listaArea = data.Select(x => new { x.Areanomb, x.Areacodi }).Where(x => x.Areacodi != 0).Distinct().OrderBy(x => x.Areacodi).ToList();

                #region cabecera
                //***************************      CABECERA DE LA TABLA         ***********************************//

                int filaIniHora = 5;
                int coluIniHora = 2;

                int contCab = 0, contC = 0;
                ws.Cells[filaIniHora, coluIniHora].Value = "HORA";
                ws.Cells[filaIniHora, coluIniHora + 1, filaIniHora, coluIniHora + 16].Value = "RESUMEN DE GENERACIÓN POR ÁREAS DEL SEIN";
                ws.Cells[filaIniHora, coluIniHora + 1, filaIniHora, coluIniHora + 16].Merge = true;
                ws.Cells[filaIniHora, coluIniHora + 1, filaIniHora, coluIniHora + 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[filaIniHora, coluIniHora + 17].Value = "HORA";
                ws.Cells[filaIniHora, coluIniHora + 18, filaIniHora, coluIniHora + 22].Value = "GENERACIÓN POR TIPO DE GENERACIÓN";
                ws.Cells[filaIniHora, coluIniHora + 18, filaIniHora, coluIniHora + 22].Merge = true;
                ws.Cells[filaIniHora, coluIniHora + 18, filaIniHora, coluIniHora + 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[filaIniHora, coluIniHora + 23].Value = "HORA";
                ws.Cells[filaIniHora, coluIniHora + 24, filaIniHora, coluIniHora + 28].Value = "GENERACIÓN POR TIPO DE GENERACIÓN";
                ws.Cells[filaIniHora, coluIniHora + 24, filaIniHora, coluIniHora + 28].Merge = true;
                ws.Cells[filaIniHora, coluIniHora + 24, filaIniHora, coluIniHora + 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                foreach (var area in listaArea) // no ce su
                {
                    ws.Cells[filaIniHora + 1, coluIniHora + 1 + contC, filaIniHora + 1, listaTipoGen.Count + 2 + contC].Value = area.Areanomb;
                    ws.Cells[filaIniHora + 1, coluIniHora + 1 + contC, filaIniHora + 1, listaTipoGen.Count + 2 + contC].Merge = true;
                    ws.Cells[filaIniHora + 1, coluIniHora + 1 + contC, filaIniHora + 1, listaTipoGen.Count + 2 + contC].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    contC += listaTipoGen.Count;

                }
                ws.Cells[filaIniHora + 1, contC + coluIniHora + 1, filaIniHora + 1, contC + coluIniHora + 4].Value = "GENERACIÓN INTEGRANTES COES";
                ws.Cells[filaIniHora + 1, contC + coluIniHora + 1, filaIniHora + 1, contC + coluIniHora + 4].Merge = true;
                ws.Cells[filaIniHora + 1, contC + coluIniHora + 1, filaIniHora + 1, contC + coluIniHora + 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[filaIniHora + 1, contC + 1 + coluIniHora + 5, filaIniHora + 1, contC + coluIniHora + 10].Value = "INTEGRANTES COES";
                ws.Cells[filaIniHora + 1, contC + 1 + coluIniHora + 5, filaIniHora + 1, contC + coluIniHora + 10].Merge = true;
                ws.Cells[filaIniHora + 1, contC + 1 + coluIniHora + 5, filaIniHora + 1, contC + coluIniHora + 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[filaIniHora + 1, contC + 2 + coluIniHora + 10, filaIniHora + 1, contC + coluIniHora + 16].Value = "INTEGRANTES COES RER";
                ws.Cells[filaIniHora + 1, contC + 2 + coluIniHora + 10, filaIniHora + 1, contC + coluIniHora + 16].Merge = true;
                ws.Cells[filaIniHora + 1, contC + 2 + coluIniHora + 10, filaIniHora + 1, contC + coluIniHora + 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                foreach (var area in listaArea) // generacion
                {
                    for (int i = 0; i < listaTipoGen.Count; i++)
                    {
                        ws.Cells[filaIniHora + 2, i + coluIniHora + 1 + contCab].Value = "GENERACIÓN " + listaTipoGen[i].Tgenernomb + " " + area.Areanomb;
                    }
                    contCab = contCab + listaTipoGen.Count;
                }
                for (int i = 0; i < listaArea.Count; i++) //generacion coes
                {
                    ws.Cells[filaIniHora + 2, i + coluIniHora + 1 + (listaArea.Count * listaTipoGen.Count)].Value = "GENERACIÓN COES " + listaArea[i].Areanomb;
                }
                ws.Cells[filaIniHora + 2, coluIniHora + 1 + (listaArea.Count * listaTipoGen.Count) + listaArea.Count].Value = "TOTAL GENERACION";

                for (int i = 0; i < listaTipoGen.Count; i++)
                {
                    ws.Cells[filaIniHora + 2, i + coluIniHora + 3 + (listaArea.Count * listaTipoGen.Count) + listaArea.Count].Value = listaTipoGen[i].Tgenernomb;
                }
                ws.Cells[filaIniHora + 2, coluIniHora + 4 + (listaArea.Count * listaTipoGen.Count) + (listaArea.Count * 2)].Value = "TOTAL GENERACION";

                for (int i = 0; i < listaTipoGen.Count; i++)
                {
                    ws.Cells[filaIniHora + 2, i + coluIniHora + 6 + (listaArea.Count * listaTipoGen.Count) + (listaArea.Count * 2)].Value = listaTipoGen[i].Tgenernomb + " RER";
                }
                ws.Cells[filaIniHora + 2, coluIniHora + 6 + (listaArea.Count * listaTipoGen.Count) + (listaArea.Count * 2) + listaTipoGen.Count].Value = "TOTAL RER";


                #region Formato Cabecera

                UtilExcel.CeldasExcelAgrupar(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora); //hora1
                UtilExcel.CeldasExcelAgrupar(ws, filaIniHora, coluIniHora + 17, filaIniHora + 2, coluIniHora + 17); //hora2
                UtilExcel.CeldasExcelAgrupar(ws, filaIniHora, coluIniHora + 23, filaIniHora + 2, coluIniHora + 23); //hora3

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora + 28, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora + 28, "Centro");

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora, "Arial", 14); //hora1
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora + 17, filaIniHora + 2, coluIniHora + 17, "Arial", 14); //hora2
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora + 23, filaIniHora + 2, coluIniHora + 23, "Arial", 14);  //hora3
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora, "#ACB9CA"); //h1
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora + 17, filaIniHora + 2, coluIniHora + 17, "#ACB9CA"); //h2
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora + 23, filaIniHora + 2, coluIniHora + 23, "#ACB9CA"); //h3
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora, "#FFFFFF");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora + 17, filaIniHora + 2, coluIniHora + 17, "#FFFFFF");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora + 23, filaIniHora + 2, coluIniHora + 23, "#FFFFFF");

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora + 1, filaIniHora + 1, coluIniHora + 16, "Arial", 16);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora + 18, filaIniHora + 1, coluIniHora + 22, "Arial", 16);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora, coluIniHora + 24, filaIniHora + 1, coluIniHora + 28, "Arial", 16);


                for (int col = coluIniHora; col < coluIniHora + 28; col++)
                {
                    UtilExcel.CeldasExcelWrapText(ws, filaIniHora + 2, col + 1);
                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 2, col + 1, filaIniHora + 2, col + 1, "Arial", 8);
                }

                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora + 1, filaIniHora, coluIniHora + 16, ConstantesPR5ReportesServicio.ColorInfSGI); // color RESUMEN
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora + 1, filaIniHora, coluIniHora + 16, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 1, coluIniHora + 1, filaIniHora + 2, coluIniHora + 12, "#B4C6E7"); // color NO CE SU
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 1, coluIniHora + 13, filaIniHora + 2, coluIniHora + 16, ConstantesPR5ReportesServicio.ColorInfSGI);
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora + 1, coluIniHora + 13, filaIniHora + 2, coluIniHora + 16, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora + 18, filaIniHora + 2, coluIniHora + 22, "#44546A");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora + 18, filaIniHora + 2, coluIniHora + 22, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora, coluIniHora + 24, filaIniHora + 2, coluIniHora + 28, "#2F75B5");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniHora, coluIniHora + 24, filaIniHora + 2, coluIniHora + 28, "#FFFFFF");

                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora, coluIniHora, filaIniHora + 2, coluIniHora + 28);

                for (int z = coluIniHora; z <= coluIniHora + 28; z++) { ws.Column(z).Width = 16; }

                #endregion

                #endregion

                #region cuerpo

                //***************************      CUERPO DE LA TABLA         ***********************************//


                MeMedicion48DTO m48 = null;
                decimal? valor;

                // CUERPO TABLA
                for (int hora = 1; hora <= 48; hora++) //horas
                {
                    ws.Cells[filaIniHora + 2 + hora, coluIniHora].Value = fecha1.AddMinutes(hora * 30).ToString("HH:mm");
                    ws.Cells[filaIniHora + 2 + hora, coluIniHora + 17].Value = fecha1.AddMinutes(hora * 30).ToString("HH:mm");
                    ws.Cells[filaIniHora + 2 + hora, coluIniHora + 23].Value = fecha1.AddMinutes(hora * 30).ToString("HH:mm");
                }


                int av = 0;
                for (int coluData = 0; coluData < data.Count; coluData++) //data
                {
                    if (coluData == 16 || coluData == 21)
                    {
                        av++;
                    }

                    m48 = data[coluData];
                    for (int h = 1; h <= 48; h++)
                    {
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        ws.Cells[filaIniHora + 2 + h, coluData + av + coluIniHora + 1].Value = (valor != null) ? valor : 0;
                        ws.Cells[filaIniHora + 2 + h, coluData + av + coluIniHora + 1].Style.Numberformat.Format = "#,##0.00";
                    }
                }


                // TOTALES 

                ws.Cells[filaIniHora + 51, coluIniHora].Value = "TOTAL MWh";
                ws.Cells[filaIniHora + 51, coluIniHora + 17].Value = "TOTAL MWh";
                ws.Cells[filaIniHora + 51, coluIniHora + 23].Value = "TOTAL MWh";


                int av1 = 0;
                for (int coluData = 0; coluData < data.Count; coluData++) //data
                {
                    if (coluData == 16 || coluData == 21)
                    {
                        av1++;
                    }

                    m48 = data[coluData];

                    valor = ((decimal)m48.Meditotal.Value / 2);
                    ws.Cells[filaIniHora + 51, coluData + av1 + coluIniHora + 1].Value = (valor != null) ? valor : 0;
                    ws.Cells[filaIniHora + 51, coluData + av1 + coluIniHora + 1].Style.Numberformat.Format = "#,##0.00";

                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 3, coluIniHora + 1, filaIniHora + 50, coluIniHora + 28, "Arial", 8);

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniHora + 3, coluIniHora, filaIniHora + 51, coluIniHora + 28, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 3, coluIniHora, filaIniHora + 50, coluIniHora, "Arial", 10);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 3, coluIniHora + 17, filaIniHora + 50, coluIniHora + 17, "Arial", 10);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniHora + 3, coluIniHora + 23, filaIniHora + 50, coluIniHora + 23, "Arial", 10);

                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora, filaIniHora + 50, coluIniHora);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora + 17, filaIniHora + 50, coluIniHora + 17);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora + 23, filaIniHora + 50, coluIniHora + 23);

                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora + 16, filaIniHora + 50, coluIniHora + 16);
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 3, coluIniHora + 16, filaIniHora + 50, coluIniHora + 16, "#B4C6E7 ");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora + 22, filaIniHora + 50, coluIniHora + 22);
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 3, coluIniHora + 22, filaIniHora + 50, coluIniHora + 22, "#B4C6E7 ");
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 3, coluIniHora + 28, filaIniHora + 50, coluIniHora + 28);
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 3, coluIniHora + 28, filaIniHora + 50, coluIniHora + 28, "#B4C6E7 ");

                UtilExcel.CeldasExcelEnNegrita(ws, filaIniHora + 51, coluIniHora, filaIniHora + 51, coluIniHora + 28);
                UtilExcel.CeldasExcelColorFondo(ws, filaIniHora + 51, coluIniHora, filaIniHora + 51, coluIniHora + 28, "#B4C6E7 ");

                #endregion

                #endregion

                #region grafico
                nfil = 51;
                ncol2 = filaIniHora;
                ncol3 = coluIniHora;
                #endregion

                UtilExcel.BorderCeldas2(ws, filaIniHora, coluIniHora, filaIniHora + 51, coluIniHora + 28);

                ws.View.FreezePanes(8, 3);
            }
        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="posFilaGraf"></param>
        /// <param name="posColuGraf"></param>
        /// <param name="filaIniSerie"></param>
        /// <param name="coluIniSerie"></param>
        /// <param name="filaIniXSerie"></param>
        /// <param name="coluIniXSerie"></param>
        /// <param name="numSeries"></param>
        public static void AddGraficoGeneracionSEIN(ExcelWorksheet ws, string xAxisTitle, string yAxisTitle, string titulo, int rep,
            int posFilaGraf, int posColuGraf, int filaIniSerie, int coluIniSerie, int filaIniXSerie, int coluIniXSerie, int numSeries)
        {
            string nameGraf = string.Empty;
            switch (rep)
            {
                case 1: nameGraf = "grafGeneracionSEIN1"; numSeries = 3; break;
                case 2: nameGraf = "grafGeneracionSEIN2"; numSeries = 4; break;
                case 3: nameGraf = "grafGeneracionSEIN3"; numSeries = 4; break;
                case 4: nameGraf = "grafCogeneracion"; break;
            }
            //Set top left corner to row 1 column 2
            var AreaChart = ws.Drawings.AddChart(nameGraf, eChartType.AreaStacked);
            AreaChart.SetPosition(posFilaGraf, 0, posColuGraf, 0);

            AreaChart.SetSize(720, 380);

            for (int sn = 0; sn < numSeries; sn++)
            {
                var ran1 = ws.Cells[filaIniSerie, coluIniSerie + sn, filaIniSerie + 47, coluIniSerie + sn];
                var ran2 = ws.Cells[filaIniXSerie, coluIniXSerie, filaIniXSerie + 47, coluIniXSerie];

                var serie = (ExcelChartSerie)AreaChart.Series.Add(ran1, ran2);
                var celda = ws.Cells[filaIniSerie - 1, coluIniSerie + sn].Value;
                serie.Header = celda != null ? celda.ToString() : "";
            }


            AreaChart.Title.Text = titulo;
            AreaChart.Title.Font.Bold = true;
            AreaChart.YAxis.Title.Text = yAxisTitle;

            AreaChart.Legend.Position = eLegendPosition.Bottom;
        }

        #endregion

        #endregion

        #region INFORMACIÓN DE LAS UNIDADES DE GENERACIÓN

        // 3.13.2.10.	Horas de orden de arranque y parada, así como las horas de ingreso y salida de las Unidades de Generación del SEIN.
        #region REPORTE_HORAS_ORDEN_APISGENERACIONSEIN

        /// <summary>
        /// Genera vista html del reporte Horas orden APIS
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteHorasOrdenAPISHtml(List<EveHoraoperacionDTO> data, DateTime fechaInicio, DateTime fechaFin, List<EveHoraoperacionDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            List<EqFamiliaDTO> listaTipoCentral = data.GroupBy(x => new { FamCodi = x.Famcodi, FamNomb = x.Famnomb }).Select(x => new EqFamiliaDTO() { Famcodi = x.Key.FamCodi, Famnomb = x.Key.FamNomb }).ToList();
            bool tieneVarioTC = listaTipoCentral.Count >= 2;
            string descModoGrupo = listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null && listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación - Grupo" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null
                ? "" : string.Empty;

            #region cabecera

            strHtml.Append("<table id='tablaHOP' class='pretty tabla-adicional' width='100%'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>Empresa</th>");
            if (tieneVarioTC)
            {
                strHtml.Append("<th style=''>Tipo Central</th>");
            }
            strHtml.Append("<th style=''>Central</th>");
            strHtml.Append("<th style=''>Grupo</th>");
            if (descModoGrupo != string.Empty)
            {
                strHtml.AppendFormat("<th style=''>{0}</th>", descModoGrupo);
            }
            strHtml.Append("<th style=''>Inicio</th>");
            strHtml.Append("<th style=''>Final</th>");
            strHtml.Append("<th style=''>O. Arranque</th>");
            strHtml.Append("<th style=''>O. Parada</th>");
            strHtml.Append("<th style=''>Tipo Operación</th>");
            strHtml.Append("<th style=''>Ensayo de <br/>Potencia Efectiva</th>");
            strHtml.Append("<th style=''>Ensayo de <br/>Potencia Mínima</th>");
            strHtml.Append("<th style=''>Sistema</th>");
            strHtml.Append("<th style=''>Lim. Transm.</th>");
            strHtml.Append("<th style=''>Causa</th>");
            strHtml.Append("<th style=''>Observación</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var obj in data)
            {
                var datVersi = dataVersion.Find(x => x.Hophorini == obj.Hophorini);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Emprnomb);
                if (tieneVarioTC)
                {
                    strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Famnomb.Trim());
                }
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.PadreNombre);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Equiabrev);
                if (descModoGrupo != string.Empty)
                {
                    strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.EquipoNombre);
                }
                strHtml.AppendFormat("<td>{0}</td>", obj.HophoriniDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorfinDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorordarranqDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorparadaDesc);
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", obj.Subcausadesc);
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", obj.HopensayopeDesc);
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", obj.HopensayopminDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopsaisladoDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HoplimtransDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopcausacodiDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Hopdesc);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptHorasOrdenAPIS
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="esReporteCalificacion"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptHorasOrdenAPIS(ExcelWorksheet ws, string titulo, bool esReporteCalificacion, int filaIniTitulo, int coluIniTitulo,
                        DateTime fecha1, DateTime fecha2, List<EveHoraoperacionDTO> lista, List<EveHoraoperacionDTO> listaVersion)
        {
            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int row = filaIniTitulo + 1;
            int col = coluIniTitulo;
            int rowCampos = row;

            #region cabecera

            List<EqFamiliaDTO> listaTipoCentral = lista.GroupBy(x => new { FamCodi = x.Famcodi, FamNomb = x.Famnomb }).
                                        Select(x => new EqFamiliaDTO() { Famcodi = x.Key.FamCodi, Famnomb = x.Key.FamNomb }).ToList();
            bool tieneVarioTC = listaTipoCentral.Count >= 2;
            string descModoGrupo = listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null && listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación - Grupo" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null
                ? "" : string.Empty;

            int colIniEmpresa = col;
            int colIniTipoCentral = colIniEmpresa + 1;
            if (!tieneVarioTC)
            {
                colIniTipoCentral = colIniEmpresa;
            }
            int colIniCentral = colIniTipoCentral + 1;
            int colIniEquipo = colIniCentral + 1;
            int colIniModo = colIniEquipo + 1;
            if (descModoGrupo == string.Empty)
            {
                colIniModo = colIniCentral;
            }
            int colIniEnParalelo = colIniModo + 1;
            int colIniFueraParalelo = colIniEnParalelo + 1;
            int colIniOArranque = colIniFueraParalelo + 1;
            int colIniOParada = colIniOArranque + 1;
            int colIniTipoOp = colIniOParada + 1;
            int colIniEnsayo = colIniTipoOp + 1;
            int colIniEnsayoMin = colIniEnsayo + 1;
            int colIniSistema = colIniEnsayoMin + 1;
            int colIniLimTransm = colIniSistema + 1;
            int colIniCausa = colIniLimTransm + 1;
            int colIniObs = colIniCausa + 1;

            if (tieneVarioTC)
            {
                ws.Cells[row, colIniTipoCentral].Value = "Tipo Central";
            }
            ws.Cells[row, colIniEmpresa].Value = "Empresa";
            ws.Cells[row, colIniCentral].Value = "Central";
            ws.Cells[row, colIniEquipo].Value = "Grupo";
            if (descModoGrupo != string.Empty)
            {
                ws.Cells[row, colIniModo].Value = descModoGrupo;
            }
            ws.Cells[row, colIniEnParalelo].Value = "Inicio";
            ws.Cells[row, colIniFueraParalelo].Value = "Final";
            ws.Cells[row, colIniOArranque].Value = "O. Arranque";
            ws.Cells[row, colIniOParada].Value = "O. Parada";
            ws.Cells[row, colIniTipoOp].Value = "Tipo Operación";
            ws.Cells[row, colIniEnsayo].Value = "Ensayo de \nPotencia Efectiva";
            ws.Cells[row, colIniEnsayoMin].Value = "Ensayo de \nPotencia Mínima";
            ws.Cells[row, colIniSistema].Value = "Sistema";
            ws.Cells[row, colIniLimTransm].Value = "Lim. Transm.";
            ws.Cells[row, colIniCausa].Value = "Causa";
            ws.Cells[row, colIniObs].Value = "Observación";

            int colIni = colIniEmpresa;
            int colFin = colIniObs;

            #region Formato Cabecera

            UtilExcel.CeldasExcelColorTexto(ws, row, colIni, row, colFin, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, row, colIni, row, colFin, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, row, colIni, row, colFin, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, row, colIni, row, colFin);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, row, colIni, row, colFin, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, row, colIni, row, colFin, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, row, colIni, row, colFin);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, row, colIni, row, colFin, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region cuerpo
            if (lista.Count > 0)
            {
                row++;
                int rowIniData = row;
                foreach (var reg in lista)
                {
                    if (tieneVarioTC)
                    {
                        ws.Cells[row, colIniTipoCentral].Value = reg.Famnomb.Trim();
                    }

                    ws.Cells[row, colIniEmpresa].Value = reg.Emprnomb.Trim();
                    ws.Cells[row, colIniCentral].Value = reg.PadreNombre.Trim();
                    ws.Cells[row, colIniEquipo].Value = reg.Equiabrev;
                    if (descModoGrupo != string.Empty)
                    {
                        ws.Cells[row, colIniModo].Value = reg.EquipoNombre.Trim();
                    }

                    ws.Cells[row, colIniEnParalelo].Value = reg.HophoriniDesc;
                    ws.Cells[row, colIniFueraParalelo].Value = reg.HophorfinDesc;
                    ws.Cells[row, colIniOArranque].Value = reg.HophorordarranqDesc;
                    ws.Cells[row, colIniOParada].Value = reg.HophorparadaDesc;
                    ws.Cells[row, colIniTipoOp].Value = reg.Subcausadesc;
                    if (esReporteCalificacion)
                    {
                        ws.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniTipoOp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniTipoOp].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(reg.Subcausacolor));
                        ws.Cells[row, colIniTipoOp].Style.Font.Color.SetColor(Color.Black);
                    }
                    ws.Cells[row, colIniEnsayo].Value = reg.HopensayopeDesc;
                    ws.Cells[row, colIniEnsayoMin].Value = reg.HopensayopminDesc;
                    ws.Cells[row, colIniSistema].Value = reg.HopsaisladoDesc;
                    ws.Cells[row, colIniLimTransm].Value = reg.HoplimtransDesc;
                    ws.Cells[row, colIniCausa].Value = reg.HopcausacodiDesc;
                    ws.Cells[row, colIniObs].Value = reg.Hopdesc;
                    row++;
                }
                int rowFinData = row > rowIniData ? row - 1 : rowIniData;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIni, rowFinData, colFin, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniCentral, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniModo, rowFinData, colIniModo, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniCausa, rowFinData, colIniObs, "Izquierda");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIni, rowFinData, colFin, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIni, rowFinData, colFin, "Arial", 10);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIni, rowFinData, colFin);
                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIni, rowFinData, colFin);
                if (esReporteCalificacion)
                    UtilExcel.CeldasExcelEnNegrita(ws, rowIniData, colIniTipoOp, rowFinData, colIniTipoOp);
            }
            #endregion

            if (tieneVarioTC)
            {
                ws.Column(colIniTipoCentral).Width = 30;
            }
            ws.Column(colIniEmpresa).Width = 37;
            ws.Column(colIniCentral).Width = 45;
            ws.Column(colIniEquipo).Width = 13;
            if (descModoGrupo != string.Empty)
            {
                ws.Column(colIniModo).Width = 45;
            }
            ws.Column(colIniEnParalelo).Width = 20;
            ws.Column(colIniFueraParalelo).Width = 20;
            ws.Column(colIniOArranque).Width = 20;
            ws.Column(colIniOParada).Width = 20;
            ws.Column(colIniTipoOp).Width = 30;

            ws.Column(colIniEnsayo).Width = 19;
            ws.Column(colIniEnsayoMin).Width = 19;
            ws.Column(colIniSistema).Width = 12;
            ws.Column(colIniLimTransm).Width = 12;
            ws.Column(colIniCausa).Width = 15;
            ws.Column(colIniObs).Width = 75;

            #endregion

            ws.View.FreezePanes(rowCampos + 1, colIniFueraParalelo + 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// GetHoraWordHoraOperacion
        /// </summary>
        /// <param name="fechaHora"></param>
        /// <param name="esHoraFin"></param>
        /// <returns></returns>
        public static string GetHoraWordHoraOperacion(DateTime fechaHora, bool esHoraFin)
        {
            if (esHoraFin && fechaHora.Hour == 0 && fechaHora.Minute == 0)
            {
                return "24:00";
            }

            return fechaHora.ToString(ConstantesAppServicio.FormatoHora);
        }

        #endregion

        // 3.13.2.11.	Hora de inicio y fin de las Indisponibilidades de las Unidades de Generación del SEIN y su respectivo motivo.
        #region REPORTE_HORA_INICIO_FIN_DISPONIBILIDAD

        /// <summary>
        /// Lista de hora de indisponibilidad
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteHoraIndisponibilidadesHtml(List<EveManttoDTO> data, DateTime fechaInicio, DateTime fechaFin, List<EveManttoDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table id='tablaMantto' class='pretty tabla-icono' style='table-layout: fixed; width: 2200px'>");
            strHtml.Append("<thead>");
            #region cabecera

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:150px;'>Empresa</th>");
            strHtml.Append("<th style='width:150px;'>Ubicación</th>");
            strHtml.Append("<th style='width:100px;'>Equipo</th>");
            strHtml.Append("<th style='width:100px;'>Inicio</th>");
            strHtml.Append("<th style='width:100px;'>Final</th>");
            strHtml.Append("<th style='width:400px;'>Descripción</th>");
            strHtml.Append("<th style='width: 70px;'>MW Indisp.</th>");
            strHtml.Append("<th style='width: 70px;'>Progr.</th>");
            strHtml.Append("<th style='width: 70px;'>Dispon</th>");
            strHtml.Append("<th style='width: 70px;'>Interrupc.</th>");
            strHtml.Append("<th style='width: 70px;'>Tipo</th>");
            strHtml.Append("<th style='width: 70px;'>CodEq</th>");
            strHtml.Append("<th style='width: 70px;'>TipoEq_Osinerg</th>");
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            foreach (var list in data)
            {
                var datVersi = dataVersion.Find(x => x.Manttocodi == list.Manttocodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Areanomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Equiabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                strHtml.AppendFormat("<td>{0}</td>", list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));

                var descrip = list.Evendescrip;
                string _bground = string.Empty;
                if (datVersi != null) { if (list.Evendescrip != datVersi.Evendescrip) { descrip = datVersi.Evendescrip; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);

                strHtml.AppendFormat("<td>{0}</td>", list.Evenmwindisp);
                strHtml.AppendFormat("<td>{0}</td>", list.Eventipoprog);
                strHtml.AppendFormat("<td>{0}</td>", list.Evenindispo);
                strHtml.AppendFormat("<td>{0}</td>", list.Eveninterrup);
                strHtml.AppendFormat("<td>{0}</td>", list.Tipoevenabrev);
                strHtml.AppendFormat("<td>{0}</td>", list.Equicodi);
                strHtml.AppendFormat("<td>{0}</td>", list.Osigrupocodi);
                strHtml.Append("</tr>");
            }

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el listado excel para el reporte  Hora Inicio Fin Indisponibilidades
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptExcelListadoHoraIndisponibilidades(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveManttoDTO> data, List<EveManttoDTO> dataVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniEmpresa = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniDescripcion = colIniFinal + 1;
            int colIniMWIndisp = colIniDescripcion + 1;
            int colIniProgr = colIniMWIndisp + 1;
            int colIniDispon = colIniProgr + 1;
            int colIniInterrup = colIniDispon + 1;
            int colIniTipo = colIniInterrup + 1;
            int colIniCodEq = colIniTipo + 1;
            int colIniTipoOsig = colIniCodEq + 1;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "Ubicación";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "Equipo";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "Inicio";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "Final";
            ws.Cells[rowIniEmpresa, colIniDescripcion].Value = "Descripción";
            ws.Cells[rowIniEmpresa, colIniMWIndisp].Value = "MW Indisp.";
            ws.Cells[rowIniEmpresa, colIniProgr].Value = "Prog.";
            ws.Cells[rowIniEmpresa, colIniDispon].Value = "Dispon";
            ws.Cells[rowIniEmpresa, colIniInterrup].Value = "Interrupc.";
            ws.Cells[rowIniEmpresa, colIniTipo].Value = "Tipo";
            ws.Cells[rowIniEmpresa, colIniCodEq].Value = "CodEq";
            ws.Cells[rowIniEmpresa, colIniTipoOsig].Value = "TipoEq_Osinerg";

            //Nombre Reporte
            int colFinNombreReporte = colIniDescripcion;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Hora Inicio y Fin de Indisponibilidades de las Unidades de Generación SEIN";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            UtilExcel.CeldasExcelColorTexto(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTipoOsig, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniEmpresa + 1;
            #region cuerpo MANTENIMIENTO EJECUTADO

            if (data.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in data)
                {
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    ws.Cells[row, colIniFinal].Value = list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    ws.Cells[row, colIniDescripcion].Value = list.Evendescrip;
                    ws.Cells[row, colIniMWIndisp].Value = list.Evenmwindisp;
                    ws.Cells[row, colIniProgr].Value = list.Eventipoprog;
                    ws.Cells[row, colIniDispon].Value = list.Evenindispo;
                    ws.Cells[row, colIniInterrup].Value = list.Eveninterrup;
                    ws.Cells[row, colIniTipo].Value = list.Tipoevenabrev;
                    ws.Cells[row, colIniCodEq].Value = list.Equicodi;
                    ws.Cells[row, colIniTipoOsig].Value = list.Osigrupocodi;

                    rowFinData = row;
                    row++;
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniInicio, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniMWIndisp, rowFinData, colIniMWIndisp, "Derecha");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniCodEq, rowFinData, colIniCodEq, "Derecha");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniDescripcion, rowFinData, colIniDescripcion);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniEmpresa, rowFinData, colIniTipoOsig);

                #endregion
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniEmpresa).Height = 20;

            ws.Column(colIniEmpresa).Width = 30;
            ws.Column(colIniUbicacion).Width = 35;
            ws.Column(colIniEquipo).Width = 25;
            ws.Column(colIniInicio).Width = 16;
            ws.Column(colIniFinal).Width = 16;
            ws.Column(colIniDescripcion).Width = 70;
            ws.Column(colIniMWIndisp).Width = 13;
            ws.Column(colIniProgr).Width = 8;
            ws.Column(colIniDispon).Width = 8;
            ws.Column(colIniInterrup).Width = 11;
            ws.Column(colIniTipo).Width = 13;
            ws.Column(colIniCodEq).Width = 8;
            ws.Column(colIniTipoOsig).Width = 17;

            ws.View.FreezePanes(rowIniEmpresa + 1, 1);

            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.12.	Reserva Fría del sistema.
        #region REPORTE_RESERVA_FRIA_SISTEMA

        /// <summary>
        /// Generación del reporte de ReservaFria en versión web
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="filtroRF"></param>
        /// <param name="listaPto"></param>
        /// <param name="totalData"></param>
        /// <returns></returns>
        public static string ReporteReservaFriaHtml(DateTime fechaInicial, string filtroRF, List<MePtomedicionDTO> listaPto, List<MeMedicion48DTO> totalData)
        {
            if (listaPto.Count == 0)
            {
                return "Sin datos";
            }

            List<int> listaFiltroRF = filtroRF.Split(',').Select(x => int.Parse(x)).ToList();
            int totalFiltroRf = listaFiltroRF.Count; //al menos un filtro debe existir

            //
            StringBuilder strHtml = new StringBuilder();

            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * totalFiltroRf * (60 + padding) + 4 * (60 + padding); //hora,unidades,totales

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            #region cabecera

            //fila 1 
            strHtml.Append("<tr>");
            strHtml.Append("<th rowspan='5' style='width: 80px;'>HORA</th>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th colspan='{1}' style='width: 180px;' >{0}</th>", item.Ptomedicodi, totalFiltroRf);
            }

            strHtml.Append("<th colspan='4' rowspan='3'>TOTAL</th>");
            strHtml.Append("</tr>");

            //fila 2 
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th colspan='{1}' style='width: 180px;' >{0}</th>", item.Emprnomb, totalFiltroRf);
            }
            strHtml.Append("</tr>");

            //fila 3
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th colspan='{1}' style='width: 180px;' >{0}</th>", item.Central, totalFiltroRf);
            }
            strHtml.Append("</tr>");

            //fila 4
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th colspan='{1}' style='width: 180px;' >{0}</th>", item.Equinomb, totalFiltroRf);
            }
            strHtml.AppendFormat("<th rowspan='2' style='width: 60px; background:{0}' >RESERVA FRÍA <br>TOTAL</th>", ConstantesPR5ReportesServicio.TipoReservaFriaTotalColor);
            strHtml.AppendFormat("<th rowspan='2' style='width: 60px; background:{0}' >RESERVA FRÍA</th>", ConstantesPR5ReportesServicio.TipoReservaFriaRapidaColor);
            strHtml.AppendFormat("<th rowspan='2' style='width: 60px; background:{0}' >RESERVA FRÍA <br>MÍNIMA (PR-12)</th>", ConstantesPR5ReportesServicio.TipoReservaFriaMinimaColor);
            strHtml.AppendFormat("<th rowspan='2' style='width: 60px; background:{0}' >INDISPONIBILIDAD</th>", ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidadColor);

            strHtml.Append("</tr>");

            //fila 5
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                var rfrapXPto = item.ListaReservaFria.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaRapida).First();
                string tiempoSinc = rfrapXPto.SincronizacionMin != null ? rfrapXPto.SincronizacionTiempo : "--:--:--";

                if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaTotal))
                    strHtml.AppendFormat("<th style='width: 60px; background:{0}' >RF</th>", ConstantesPR5ReportesServicio.TipoReservaFriaTotalColor);
                if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaRapida))
                    strHtml.AppendFormat("<th style='width: 60px; background:{0}' >RF ráp <br/> {1}</th>", ConstantesPR5ReportesServicio.TipoReservaFriaRapidaColor, tiempoSinc);
                if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaMinima))
                    strHtml.AppendFormat("<th style='width: 60px; background:{0}' >RF mín</th>", ConstantesPR5ReportesServicio.TipoReservaFriaMinimaColor);
                if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad))
                    strHtml.AppendFormat("<th style='width: 60px; background:{0}' >Indisp</th>", ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidadColor);
            }
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            DateTime horas = DateTime.Now.Date;
            decimal? rftotal = null;
            decimal? rfrap = null;
            decimal? rfmin = null;
            decimal? rfindisp = null;

            var totalRftXPto = totalData.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaTotal).First();
            var totalFfrapXPto = totalData.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaRapida).First();
            var totalFfminXPto = totalData.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaMinima).First();
            var totalRfindispXPto = totalData.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad).First();

            for (int h = 1; h <= 48; h++)
            {
                strHtml.Append("<tr>");

                //Hora
                horas = horas.AddMinutes(30);
                strHtml.AppendFormat("<td class='tdbody_reporte' style='width: 80px'>{0:HH:mm}</td>", horas);

                //Valores
                foreach (var pto in listaPto)
                {
                    var rftXPto = pto.ListaReservaFria.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaTotal).First();
                    var rfrapXPto = pto.ListaReservaFria.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaRapida).First();
                    var rfminXPto = pto.ListaReservaFria.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaMinima).First();
                    var rfindispXPto = pto.ListaReservaFria.Where(x => x.TipoReservaFria == ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad).First();

                    rftotal = (decimal?)rftXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(rftXPto, null);
                    rfrap = (decimal?)rfrapXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(rfrapXPto, null);
                    rfmin = (decimal?)rfminXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(rfminXPto, null);
                    rfindisp = (decimal?)rfindispXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(rfindispXPto, null);

                    if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaTotal))
                        strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rftotal != null ? ((decimal)rftotal).ToString("N", nfi) : string.Empty);
                    if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaRapida))
                        strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfrap != null ? ((decimal)rfrap).ToString("N", nfi) : string.Empty);
                    if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaMinima))
                        strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfmin != null ? ((decimal)rfmin).ToString("N", nfi) : string.Empty);
                    if (listaFiltroRF.Contains(ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad))
                        strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfindisp != null ? ((decimal)rfindisp).ToString("N", nfi) : string.Empty);
                }

                //Totales
                rftotal = (decimal?)totalRftXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(totalRftXPto, null);
                rfrap = (decimal?)totalFfrapXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(totalFfrapXPto, null);
                rfmin = (decimal?)totalFfminXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(totalFfminXPto, null);
                rfindisp = (decimal?)totalRfindispXPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(totalRfindispXPto, null);

                strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rftotal != null ? ((decimal)rftotal).ToString("N", nfi) : string.Empty);
                strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfrap != null ? ((decimal)rfrap).ToString("N", nfi) : string.Empty);
                strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfmin != null ? ((decimal)rfmin).ToString("N", nfi) : string.Empty);
                strHtml.AppendFormat("<td style='width: 60px'>{0}</td>", rfindisp != null ? ((decimal)rfindisp).ToString("N", nfi) : string.Empty);

                strHtml.Append("</tr>");
            }

            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Exportacion de Reporte Reserva Fria
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha"></param>
        /// <param name="regCDespachoXDia"></param>
        /// <param name="filtroRF"></param>
        public static void GeneraRptReservaFria(ExcelWorksheet ws, DateTime fecha, CDespachoDiario regCDespachoXDia, int filtroRF)
        {
            int tipoinfocodi = ConstantesAppServicio.TipoinfocodiRsvFria;

            UtilCdispatch.HojaExcelDiaCdispatch(ws, false, fecha, "EJECUTADO", tipoinfocodi, filtroRF, regCDespachoXDia);

            ws.View.ZoomScale = 85;
        }

        /// <summary>
        /// GetGraficoRFria
        /// </summary>
        /// <param name="tituloGrafico"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoRFria(string tituloGrafico, decimal? ymin, decimal? ymax, List<MeMedicion48DTO> listaReporte)
        {
            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaReporte.Count][];
            grafico.YaxixMax = ymax;
            grafico.YaxixMin = ymin;

            DateTime horas = DateTime.Today;
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            decimal? valor;
            RegistroSerie regSerie;
            List<DatosSerie> listadata;
            for (int i = 0; i < listaReporte.Count; i++)
            {
                if (string.IsNullOrEmpty(listaReporte[i].NombreSerie)) listaReporte[i].NombreSerie = listaReporte[i].Gruponomb;

                regSerie = new RegistroSerie();
                regSerie.Name = listaReporte[i].NombreSerie;
                regSerie.Type = "area";
                regSerie.Color = listaReporte[i].ColorSerie;

                listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    valor = (decimal?)listaReporte[i].GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(listaReporte[i], null);
                    listadata.Add(new DatosSerie() { Y = valor });
                }
                regSerie.Data = listadata;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = tituloGrafico;
            if (listaReporte.Count > 0)
            {
                grafico.YaxixTitle = "MW";
                grafico.XAxisCategories = new List<string>();
                grafico.SeriesType = new List<string>();
                grafico.SeriesYAxis = new List<int>();
            }

            return grafico;
        }

        #endregion

        // 3.13.2.13.	Caudales en los principales afluentes a las Centrales Hidroeléctricas.
        #region REPORTE_CAUDALES_CENTRAL_HIDROELECTRICA

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteCaudalesCentralHidroelectricaHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteCaudalesCentralHidroelectricaHtml(List<MeMedicion24DTO> data, List<MeReporptomedDTO> listaCabecera, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion24DTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            DateTime fecha = fechaInicio;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            int padding = 20;
            int anchoTotal = (100 + padding) + (listaCabecera.Count * (110 + padding));
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");

            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");

            strHtml.Append("<th style='width:100px;'>EMPRESA</th>");
            foreach (var p in listaCabecera)
            {
                var numVeces =
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Emprnomb + "</th>"); //empresa
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;'>CUENCA</th>");
            foreach (var p in listaCabecera)
            {
                //cuenca
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Central + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;'>INSTALACIÓN</th>");
            foreach (var p in listaCabecera)
            {
                //instalacion
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Famnomb + "</th>");//instalacion
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;'>EQUIPO</th>");
            foreach (var p in listaCabecera)
            {
                //equipo
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Equinomb + "</th>");//equipo
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;'>TIPO</th>");
            foreach (var p in listaCabecera)
            {
                //tipo
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Tipoptomedinomb + "</th>");//tipo
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:100px;'>FECHA</th>");//fecha
            foreach (var p in listaCabecera)
            {
                strHtml.Append("<th  style='width:110px;overflow-wrap: break-word; white-space: normal;' rowspan='1'>" + p.Tipoinfoabrev + "</th>");//tipo info
            }
            strHtml.Append("</tr>");


            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            decimal? valor;
            for (int i = 1; i <= 24; i++)
            {
                for (int k = 1; k <= 2; k++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", fechaInicio.ToString(ConstantesAppServicio.FormatoOnlyHora));
                    foreach (var p in listaCabecera)
                    {
                        decimal? valorVersi = null;
                        var reg = data.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);

                        if (reg != null)
                        {
                            var datVersi = dataVersion.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);
                            string _bground = string.Empty, descrip = "--";

                            valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);
                            if (datVersi != null)
                            {
                                valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datVersi, null);
                            }

                            if (valor != null)
                            {
                                descrip = ((decimal)valor).ToString("N", nfi);
                                if (valorVersi != null)
                                {
                                    if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                                }
                            }
                            if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                            strHtml.AppendFormat("<td style='width:110px; background:" + _bground + "'>{0}</td>", descrip);
                        }
                        else { strHtml.Append("<td style='width:110px;'></td>"); }
                    }
                    strHtml.Append("</tr>");
                    fechaInicio = fechaInicio.AddMinutes(30);
                }
            }

            #endregion


            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el listado (tabla) del excel  Caudales centrales hidroelectricas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fecha12"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptExcelListadoCaudalesCentralHidroelectrica(ExcelWorksheet ws, List<MeReporptomedDTO> listaCabecera, DateTime fechaInicio, DateTime fecha12
            , List<MeMedicion24DTO> data, List<MeMedicion24DTO> dataVersion)
        {
            DateTime fecha = fechaInicio;
            int filaIniEmpresa = 6;
            int coluIniEmpresa = 1;

            int ultimaFila = 0;
            int ultimaColu = 0;

            if (listaCabecera.Count > 0)
            {
                ws.Cells[filaIniEmpresa - 1, coluIniEmpresa].Value = "Caudales de las Centrales Hidroelectricas ";

                #region Cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa].Value = "EMPRESA";
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa].Value = "CUENCA";
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa].Value = "INSTALACIÓN";
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa].Value = "EQUIPO";
                ws.Cells[filaIniEmpresa + 4, coluIniEmpresa].Value = "TIPO";
                ws.Cells[filaIniEmpresa + 5, coluIniEmpresa].Value = "FECHA";

                int grupEmpr = 0;
                int colIni1 = 0;
                int colFin1 = 0;
                foreach (var p in listaCabecera.GroupBy(x => x.Emprnomb))
                {
                    colIni1 = coluIniEmpresa + 1 + grupEmpr;
                    ws.Cells[filaIniEmpresa, coluIniEmpresa + 1 + grupEmpr].Value = p.Key;
                    grupEmpr = grupEmpr + p.Count();
                    colFin1 = coluIniEmpresa + 1 + grupEmpr - 1;
                    UtilExcel.CeldasExcelAgrupar(ws, filaIniEmpresa, colIni1, filaIniEmpresa, colFin1);
                    UtilExcel.BorderCeldas5(ws, filaIniEmpresa, colIni1, filaIniEmpresa + 5 + 48, colFin1);
                }

                int c = 0;
                foreach (var p in listaCabecera)
                {
                    ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 1 + c].Value = p.Central;
                    ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 1 + c].Value = p.Famnomb;
                    ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 1 + c].Value = p.Equinomb;
                    ws.Cells[filaIniEmpresa + 4, coluIniEmpresa + 1 + c].Value = p.Tipoptomedinomb;
                    ws.Cells[filaIniEmpresa + 5, coluIniEmpresa + 1 + c].Value = p.Tipoinfoabrev;
                    c++;
                }

                ultimaColu = coluIniEmpresa + 1 + listaCabecera.Count() - 1;

                #region Formato Cabecera

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa - 1, coluIniEmpresa, filaIniEmpresa - 1, coluIniEmpresa, "Arial", 18);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniEmpresa - 1, coluIniEmpresa, filaIniEmpresa - 1, coluIniEmpresa);

                UtilExcel.SetFormatoCelda(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa + 5, ultimaColu, "Centro", "Centro", "#FFFFFF", ConstantesPR5ReportesServicio.ColorInfSGI, "Arial", 10, true, true);
                UtilExcel.BorderCeldas2(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa + 5, ultimaColu);

                ws.Column(coluIniEmpresa).Width = 16;
                for (int i = coluIniEmpresa + 1; i <= ultimaColu; i++)
                {
                    ws.Column(i).Width = 18;
                }
                ws.Row(filaIniEmpresa).Height = 40;

                #endregion

                #endregion

                #region cuerpo

                int filaIniData = filaIniEmpresa + 5 + 1;
                int coluIniData = coluIniEmpresa;

                ultimaFila = filaIniData + 48 - 1;
                //***************************      CUERPO DE LA TABLA         ***********************************//               

                decimal? valor;
                int filaX = 0;
                for (int i = 1; i <= 24; i++)
                {
                    for (int k = 1; k <= 2; k++)
                    {
                        ws.Cells[filaIniData + filaX, coluIniData].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoOnlyHora);

                        int colX = 0;
                        foreach (var p in listaCabecera)
                        {
                            decimal? valorVersi = null;
                            var reg = data.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);

                            if (reg != null)
                            {
                                var datVersi = dataVersion.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);
                                string descrip = "--";
                                decimal val = -99999999999999.9m; //solo para que en excel tenga un formato adecuado

                                valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);

                                if (datVersi != null)
                                {
                                    valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datVersi, null);
                                }

                                if (valor != null)
                                {
                                    val = valor.Value;
                                    if (valorVersi != null)
                                    {
                                        if (valorVersi != valor) { val = valorVersi.Value; }
                                    }

                                }

                                if (valorVersi != null && valor == null)
                                {
                                    val = valorVersi.Value;
                                }
                                if (val != -99999999999999.9m)
                                {
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Value = val;
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Style.Numberformat.Format = "#,###0.000";
                                }
                                else
                                {
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Value = descrip;
                                }
                            }
                            else
                            {
                                ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Value = "";
                            }
                            colX++;
                        }
                        fechaInicio = fechaInicio.AddMinutes(30);
                        filaX++;
                    }
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Arial", 9);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
                UtilExcel.BorderCeldas5(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
                #endregion

                #endregion

                #region Fórmulas

                ws.Cells[ultimaFila + 1, coluIniEmpresa].Value = "MAX";
                ws.Cells[ultimaFila + 2, coluIniEmpresa].Value = "MIN";
                ws.Cells[ultimaFila + 3, coluIniEmpresa].Value = "PROM";
                ws.Cells[ultimaFila + 4, coluIniEmpresa].Value = "INICIO";
                ws.Cells[ultimaFila + 5, coluIniEmpresa].Value = "FINAL";

                for (var col = coluIniData + 1; col <= ultimaColu; col++)
                {
                    string letraCol = UtilExcel.GetExcelColumnName(col);
                    ws.Cells[ultimaFila + 1, col].Formula = string.Format("=MIN({0}{1}:{0}{2})", letraCol, filaIniData, ultimaFila);
                    ws.Cells[ultimaFila + 2, col].Formula = string.Format("=MAX({0}{1}:{0}{2})", letraCol, filaIniData, ultimaFila);
                    ws.Cells[ultimaFila + 3, col].Formula = string.Format("=AVERAGE({0}{1}:{0}{2})", letraCol, filaIniData, ultimaFila);
                    ws.Cells[ultimaFila + 4, col].Formula = string.Format("={0}{1}", letraCol, filaIniData);
                    ws.Cells[ultimaFila + 5, col].Formula = string.Format("={0}{1}", letraCol, ultimaFila);
                }

                UtilExcel.CeldasExcelFormatoNumero(ws, ultimaFila + 1, coluIniData + 1, ultimaFila + 5, ultimaColu, 3);

                #region Formato Fórmulas
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, ultimaFila + 1, coluIniEmpresa, ultimaFila + 5, ultimaColu, "Arial", 9);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, ultimaFila + 1, coluIniEmpresa, ultimaFila + 5, ultimaColu, "Centro");
                UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 1, coluIniEmpresa, ultimaFila + 5, ultimaColu, "#D9E1F2");
                UtilExcel.BorderCeldas5(ws, ultimaFila + 1, coluIniEmpresa, ultimaFila + 3, ultimaColu);

                #endregion

                #endregion
                ws.Cells[ultimaFila + 7, coluIniEmpresa + 2].Value = "Valores Estimados";
                UtilExcel.CeldasExcelColorFondo(ws, ultimaFila + 7, coluIniEmpresa + 1, ultimaFila + 7, coluIniEmpresa + 1, "#D9E1F2");
                ws.View.FreezePanes(filaIniData, coluIniData + 1);

            }

            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.14.	Volúmenes horarios y caudales horarios de descarga de los embalses asociados a las Centrales Hidroeléctricas.
        #region REPORTE_HORARIOS_CAUDAL_VOLUMEN_CENTRAL_HIDROELECTRICA

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteHorariosCaudalVolumenCentralHidroelectricaHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>        
        public static string ListarReporteHorariosVolumenEmbalseCentralHidroelectricaHtml(List<MeMedicion24DTO> data, List<MeReporptomedDTO> listaCabecera, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion24DTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            DateTime fecha = fechaInicio;

            //***************************      CABECERA DE LA TABLA         ***********************************//
            if (data.Count > 0)
            {
                #region cabecera
                strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
                strHtml.Append("<thead>");
                strHtml.Append("<tr>");
                //strHtml.Append("<th style='width:170px;' rowspan='3' >FECHA </th>");
                strHtml.AppendFormat("<th style='width:170px;' rowspan='3' >FECHA <br> {0}</th>", fechaInicio.ToString(ConstantesAppServicio.FormatoFecha));
                var listaEmpresas = listaCabecera.Select(x => x.Emprnomb).Distinct().ToList();
                foreach (var p in listaEmpresas)
                {
                    var cantidadEmpresas = listaCabecera.Where(x => x.Emprnomb == p).Count();
                    strHtml.Append("<th  style='width:70px;' colspan='" + cantidadEmpresas + "' rowspan='1'>" + p + "</th>");

                }
                strHtml.Append("</tr>");
                strHtml.Append("<tr>");
                foreach (var p in listaCabecera)
                {
                    strHtml.Append("<th  style='width:70px;' rowspan='1'>" + p.Ptomedielenomb + "</th>");
                }
                strHtml.Append("</tr>");
                strHtml.Append("<tr>");
                foreach (var p in listaCabecera)
                {
                    strHtml.Append("<th  style='width:70px;' rowspan='1'>" + p.Tipoinfoabrev + "</th>");
                }
                strHtml.Append("</tr>");


                strHtml.Append("</thead>");

                #endregion

                #region cuerpo

                //***************************      CUERPO DE LA TABLA         ***********************************//
                strHtml.Append("<tbody>");

                decimal? valor;
                for (int i = 1; i <= 24; i++)
                {
                    for (int k = 1; k <= 2; k++)
                    {
                        strHtml.Append("<tr>");
                        strHtml.AppendFormat("<td>{0}</td>", fechaInicio.ToString(ConstantesAppServicio.FormatoOnlyHora));
                        foreach (var p in listaCabecera)
                        {
                            decimal? valorVersi = null;
                            var reg = data.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);

                            if (reg != null)
                            {
                                var datVersi = dataVersion.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);
                                string _bground = string.Empty, descrip = "--";

                                valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);
                                if (datVersi != null)
                                {
                                    valorVersi = (decimal?)datVersi.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datVersi, null);
                                }

                                if (valor != null)
                                {
                                    descrip = ((decimal)valor).ToString("N", nfi);
                                    if (valorVersi != null)
                                    {
                                        if (valorVersi != valor) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                                    }
                                    //strHtml.AppendFormat("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                }
                                if (valorVersi != null && valor == null) { descrip = ((decimal)valorVersi).ToString("N", nfi); _bground = "lightgreen"; }
                                strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);
                                //else { strHtml.AppendFormat("<td>--</td>")); }
                            }
                            else { strHtml.Append("<td></td>"); }
                        }
                        strHtml.Append("</tr>");
                        fechaInicio = fechaInicio.AddMinutes(30);
                    }
                }

                #endregion
            }
            else
            {
                int ncol = listaCabecera.Count;
                strHtml.Append("<tr><td style='text-align:center'>No existen registros.</td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera Listado (tabla) del excel de Volumenes Horarios de embalses
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fecha2"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptVolumenHorarioCentralesHidroelectrica(ExcelWorksheet ws, DateTime fechaInicio, DateTime fecha2, List<MeReporptomedDTO> listaPto, List<MeReporptomedDTO> listaPtoVersion, List<MeMedicion24DTO> data, List<MeMedicion24DTO> dataVersion)
        {
            DateTime fecha = fechaInicio;
            int filaIniEmpresa = 5;
            int coluIniEmpresa = 1;

            int ultimaFila = 0;
            int ultimaColu = 0;

            if (listaPto.Count() > 0)
            {
                #region Cabecera
                ws.Cells[filaIniEmpresa, coluIniEmpresa].Value = "EMPRESA";
                ws.Cells[filaIniEmpresa + 1, coluIniEmpresa].Value = "CUENCA";
                ws.Cells[filaIniEmpresa + 2, coluIniEmpresa].Value = "INSTALACIÓN";
                ws.Cells[filaIniEmpresa + 3, coluIniEmpresa].Value = "EQUIPO";
                ws.Cells[filaIniEmpresa + 4, coluIniEmpresa].Value = "TIPO";
                ws.Cells[filaIniEmpresa + 5, coluIniEmpresa].Value = "FECHA";

                int c = 0;
                int tamListaCabecera = listaPto.Count();
                foreach (var p in listaPto)
                {
                    ws.Cells[filaIniEmpresa, coluIniEmpresa + 1 + c].Value = p.Emprnomb;
                    ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 1 + c].Value = p.Central;
                    ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 1 + c].Value = p.Famnomb;
                    ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 1 + c].Value = p.Equinomb;
                    ws.Cells[filaIniEmpresa + 4, coluIniEmpresa + 1 + c].Value = p.Tipoptomedinomb;
                    ws.Cells[filaIniEmpresa + 5, coluIniEmpresa + 1 + c].Value = p.Tipoinfoabrev;
                    c++;
                }

                ultimaColu = coluIniEmpresa + 1 + tamListaCabecera - 1;

                #region Formato Cabecera

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniEmpresa - 1, coluIniEmpresa, filaIniEmpresa - 1, coluIniEmpresa, "Arial", 18);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniEmpresa - 1, coluIniEmpresa, filaIniEmpresa - 1, coluIniEmpresa);

                UtilExcel.SetFormatoCelda(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa + 5, ultimaColu, "Centro", "Centro", "#FFFFFF", ConstantesPR5ReportesServicio.ColorInfSGI, "Arial", 10, true, true);
                UtilExcel.BorderCeldas2(ws, filaIniEmpresa, coluIniEmpresa, filaIniEmpresa + 5, ultimaColu);

                ws.Column(coluIniEmpresa).Width = 16;
                for (int i = coluIniEmpresa + 1; i <= ultimaColu; i++)
                {
                    ws.Column(i).Width = 18;
                }
                ws.Row(filaIniEmpresa).Height = 40;

                #endregion

                #endregion

                #region cuerpo

                int filaIniData = filaIniEmpresa + 5 + 1;
                int coluIniData = coluIniEmpresa;

                ultimaFila = filaIniData + 48 - 1;
                //***************************      CUERPO DE LA TABLA         ***********************************//

                //strHtml.Append("<tbody>");
                decimal? valor;
                int filaX = 0;
                for (int i = 1; i <= 24; i++)
                {
                    for (int k = 1; k <= 2; k++)
                    {
                        ws.Cells[filaIniData + filaX, coluIniData].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        int colX = 0;
                        foreach (var p in listaPto)
                        {
                            var reg = data.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);

                            if (reg != null)
                            {
                                var datVersi = dataVersion.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fecha && x.Tipoinfocodi == p.Tipoinfocodi);

                                valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);

                                if (valor != null)
                                {
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Value = (decimal)valor;
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Style.Numberformat.Format = "#,###0.000";
                                }
                                else
                                {
                                    ws.Cells[filaIniData + filaX, coluIniData + 1 + colX].Value = "--";
                                }
                            }
                            colX++;
                        }
                        fechaInicio = fechaInicio.AddMinutes(30);
                        filaX++;
                    }
                }

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Arial", 9);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
                UtilExcel.BorderCeldas5(ws, filaIniData, coluIniData, ultimaFila, coluIniData);

                int col = 1;
                foreach (var p in listaPto)
                {
                    UtilExcel.BorderCeldas5(ws, filaIniData, coluIniData + col, ultimaFila, coluIniData + col);
                    col++;
                }

                #endregion

                #endregion
                ws.View.FreezePanes(filaIniData, coluIniData + 1);
            }

            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// Genera el listado (tabla) del excel del reporte de descargas Lagunas de las centrales hidrolectricas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="titulo"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptDescargaLagunasCentralesHidroelectrica(ExcelWorksheet ws, int filaIniTitulo, int coluIniTitulo, string titulo, DateTime fecha1, DateTime fecha2, List<MeMedicionxintervaloDTO> data, List<MeMedicionxintervaloDTO> dataVersion)
        {
            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int filaIniFecha = filaIniTitulo + 1;
            int coluIniFecha = coluIniTitulo;

            int ultimaFila = 0;
            int ultimaColumna = 0;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            ws.Cells[filaIniFecha, coluIniFecha].Value = "FECHA";
            ws.Cells[filaIniFecha, coluIniFecha + 1].Value = "EMPRESA";
            ws.Cells[filaIniFecha, coluIniFecha + 2].Value = "EMBALSE";
            ws.Cells[filaIniFecha, coluIniFecha + 3].Value = "INICIO";
            ws.Cells[filaIniFecha, coluIniFecha + 4].Value = "FINAL";
            ws.Cells[filaIniFecha, coluIniFecha + 5].Value = "CAUDAL PROMEDIO \n (m3/s)";
            ws.Cells[filaIniFecha, coluIniFecha + 6].Value = "OBSERVACIÓN";

            ultimaColumna = coluIniFecha + 6;

            #region Formato Cabecera
            UtilExcel.CeldasExcelColorTexto(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna, "#FFFFFF");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna, "Centro");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna, "Arial", 11);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna);
            UtilExcel.CeldasExcelWrapText(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColumna, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);
            ws.Column(coluIniFecha).Width = 18;
            ws.Column(coluIniFecha + 1).Width = 32;
            ws.Column(coluIniFecha + 2).Width = 50;
            ws.Column(coluIniFecha + 3).Width = 14;
            ws.Column(coluIniFecha + 4).Width = 14;
            ws.Column(coluIniFecha + 5).Width = 27;
            ws.Column(coluIniFecha + 6).Width = 50;
            #endregion

            #endregion

            #region cuerpo
            if (data.Count > 0)
            {

                //***************************      CUERPO DE LA TABLA         ***********************************//                
                int filaX = 0;
                foreach (var list in data)
                {
                    var datVersi = dataVersion.Find(x => x.Medintcodi == list.Medintcodi);


                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha].Value = list.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 1].Value = list.Emprnomb;
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 2].Value = list.Equinomb;
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 3].Value = list.Medintfechaini.ToString(ConstantesAppServicio.FormatoHHmmss);
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 4].Value = list.Medintfechafin.ToString(ConstantesAppServicio.FormatoHHmmss);

                    var _descrip = list.Medinth1;
                    if (datVersi != null) { if (_descrip != datVersi.Medinth1) { _descrip = datVersi.Medinth1; } }
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 5].Value = _descrip;
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[filaIniFecha + 1 + filaX, coluIniFecha + 6].Value = list.Medintdescrip;

                    filaX++;
                }
                ultimaFila = filaIniFecha + 1 + filaX - 1;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFecha + 1, coluIniFecha, ultimaFila, ultimaColumna, "Arial", 10);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFecha + 1, coluIniFecha, ultimaFila, ultimaColumna, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFecha + 1, coluIniFecha + 1, ultimaFila, coluIniFecha + 2, "Izquierda");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFecha + 1, coluIniFecha, ultimaFila, ultimaColumna, "Centro");

                #endregion

                UtilExcel.BorderCeldas2(ws, filaIniFecha + 1, coluIniFecha, ultimaFila, ultimaColumna);
                UtilExcel.CeldasExcelWrapText(ws, filaIniFecha + 1, coluIniFecha, ultimaFila, ultimaColumna);

                ws.View.FreezePanes(filaIniFecha + 1, 1);
                ws.View.ZoomScale = 80;
            }
            #endregion
        }

        #endregion

        // 3.13.2.15.	Vertimientos en los embalses y/o presas en período y volumen.
        #region REPORTE_VERTIMIENTOS_PERIODO_VOLUMEN

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteVertimientosPeriodoVolumenHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteVertimientosPeriodoVolumenHtml(List<MeMedicionxintervaloDTO> data, DateTime fechaInicio, DateTime fechaFin, List<MeMedicionxintervaloDTO> dataVersion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EMBALSE</th>");
            strHtml.Append("<th>INICIO</th>");
            strHtml.Append("<th>FINAL</th>");
            strHtml.Append("<th>CAUDAL PROMEDIO <br/> (m3/s)</th>");
            strHtml.Append("<th>OBSERVACIÓN</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                var datVersi = dataVersion.Find(x => x.Medintcodi == list.Medintcodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", list.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha));
                strHtml.AppendFormat("<td>{0}</td>", list.Emprnomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Equinomb);
                strHtml.AppendFormat("<td>{0}</td>", list.Medintfechaini.ToString(ConstantesAppServicio.FormatoHHmmss));
                strHtml.AppendFormat("<td>{0}</td>", list.Medintfechafin.ToString(ConstantesAppServicio.FormatoHHmmss));

                decimal? _descrip = list.Medinth1;
                string _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Medinth1) { _descrip = datVersi.Medinth1; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", _descrip.GetValueOrDefault(0).ToString("N", nfi));

                strHtml.AppendFormat("<td>{0}</td>", list.Medintdescrip);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        #endregion

        // 3.13.2.16.	Volúmenes o cantidad de combustible almacenado a las 24:00 h de las Centrales Térmicas.
        #region REPORTE_ListarReporteCantidadCombustibleCentralTermicaHtml

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteCantidadCombustibleCentralTermicaHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteCantidadCombustibleCentralTermicaHtml(List<MeMedicionxintervaloDTO> data, DateTime fechaInicio, DateTime fechaFin, List<MeMedicionxintervaloDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed;' >");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>FECHA</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>EMPRESA</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>CENTRAL</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>TIPO DE <br> COMBUSTIBLE</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>INICIO</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>FINAL <br> DECLARADO</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>RECEPCIÓN</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>UNIDADES</th>");
            strHtml.Append("<th style='width:100px;' rowspan='1'>OBSERVACIONES</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                List<MeMedicionxintervaloDTO> dataXDia = data.Where(x => x.Medintfechaini.Date == day).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Fenergnomb).ToList();
                foreach (var item in dataXDia)
                {
                    var datVersi = dataVersion.Find(x => x.Medintcodi == item.Medintcodi);

                    //k++;
                    strHtml.Append("<tr>");
                    strHtml.Append("<td >" + item.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                    strHtml.Append("<td >" + item.Emprnomb + "</td>");
                    strHtml.Append("<td >" + item.Equinomb + "</td>");
                    strHtml.AppendFormat("<td><div class='symbol' style='background-color:{0}'></div><div class='serieName'>{1}</div></td>", item.Fenercolor, item.Fenergnomb);

                    //IMPRIRME VALOR STOCK INICIO
                    strHtml.AppendFormat("<td>{0}</td>", (item.Medinth1.GetValueOrDefault(0)).ToString("N", nfi));

                    //IMPRIME VALOR FINAL DECLARADO
                    strHtml.AppendFormat("<td>{0}</td>", (item.H1Fin.GetValueOrDefault(0)).ToString("N", nfi));

                    //IMPRIME VALOR RECEPCIÓN
                    decimal? valorRecep = item.H1Recep.GetValueOrDefault(0);

                    if (valorRecep > 0)
                    {// Pinta celda de rojo si existe valor de recepción
                        strHtml.AppendFormat("<td style='background-color:#F08080'>{0}</td>", ((decimal)valorRecep).ToString("N", nfi));
                    }
                    else { strHtml.AppendFormat("<td>{0}</td>", ((decimal)valorRecep).ToString("N", nfi)); }

                    strHtml.Append("<td >" + item.Tipoinfoabrev + "</td>");
                    strHtml.Append("<td >" + item.Medintdescrip + "</td>");
                    strHtml.Append("</tr>");
                }
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptCantidadCombustibleCentralTermica
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nfilIni"></param>
        /// <param name="ncolIni"></param>
        /// <param name="nfilFin"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptCantidadCombustibleCentralTermica(ExcelWorksheet ws, ref int nfilIni, ref int ncolIni, ref int nfilFin, DateTime fecha1, DateTime fecha2, List<MeMedicionxintervaloDTO> lista, List<MeMedicionxintervaloDTO> listaVersion)
        {
            int filaIniTitulo = 5;
            int coluIniTitulo = 2;

            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = "Reporte de Stock de Combustibles";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int row = filaIniTitulo + 1;
            int col = coluIniTitulo;


            #region cabecera
            int colIniFecha = col;
            int rowIniFecha = row;
            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";

            int colIniEmpresa = colIniFecha + 1;
            int rowIniEmpresa = rowIniFecha;
            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "EMPRESA";

            int colIniCentral = colIniEmpresa + 1;
            int rowIniCentral = rowIniFecha;
            ws.Cells[rowIniCentral, colIniCentral].Value = "CENTRAL";

            int colIniTipoComb = colIniCentral + 1;
            int rowIniTipoComb = rowIniFecha;
            ws.Cells[rowIniTipoComb, colIniTipoComb].Value = "TIPO DE COMBUSTIBLE";

            int colIniInicio = colIniTipoComb + 1;
            int rowIniInicio = rowIniFecha;
            ws.Cells[rowIniInicio, colIniInicio].Value = "INICIO";

            int colIniFinalDecl = colIniInicio + 1;
            int rowIniFinalDecl = rowIniFecha;
            ws.Cells[rowIniFinalDecl, colIniFinalDecl].Value = "FINAL DECLARADO";

            int colIniRecep = colIniFinalDecl + 1;
            int rowIniRecep = rowIniFecha;
            ws.Cells[rowIniRecep, colIniRecep].Value = "RECEPCIÓN";

            int colIniUnidades = colIniRecep + 1;
            int rowIniUnidades = rowIniFecha;
            ws.Cells[rowIniUnidades, colIniUnidades].Value = "UNIDADES";

            int colIniObservacion = colIniUnidades + 1;
            int rowIniObservacion = rowIniFecha;
            ws.Cells[rowIniObservacion, colIniObservacion].Value = "OBSERVACIONES";

            #region Formato Cabecera

            //Fecha - Hora
            UtilExcel.CeldasExcelColorTexto(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniObservacion, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region cuerpo
            if (lista.Count > 0)
            {
                int rowIniData = rowIniFecha + 1;
                row = rowIniData;

                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    var listaXDia = lista.Where(x => x.Medintfechaini.Date == day).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Fenergnomb).ToList();
                    foreach (var list in listaXDia)
                    {
                        ws.Cells[row, colIniFecha].Value = list.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                        ws.Cells[row, colIniCentral].Value = list.Equinomb;
                        ws.Cells[row, colIniTipoComb].Value = list.Fenergnomb;
                        ws.Cells[row, colIniInicio].Value = list.Medinth1;
                        ws.Cells[row, colIniFinalDecl].Value = list.H1Fin;
                        ws.Cells[row, colIniRecep].Value = list.H1Recep;
                        ws.Cells[row, colIniUnidades].Value = list.Tipoinfoabrev;
                        ws.Cells[row, colIniObservacion].Value = list.Medintdescrip;

                        row++;
                    }
                }

                int rowFinData = rowIniData + lista.Count() - 1;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFecha, rowFinData, colIniObservacion, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniFecha, rowFinData, colIniObservacion, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniFecha, rowFinData, colIniObservacion, "Arial", 10);

                UtilExcel.CeldasExcelEnNegrita(ws, rowIniData, colIniFecha, rowFinData, colIniFecha);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniFecha, rowFinData, colIniObservacion);

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniInicio, rowFinData, colIniRecep, "Derecha");
                ws.Cells[rowIniData, colIniInicio, rowFinData, colIniRecep].Style.Numberformat.Format = "#,##0.000";

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniObservacion, rowFinData, colIniObservacion, "Izquierda");

                #endregion

                #region grafico

                nfilIni = rowIniData;
                nfilFin = rowFinData;
                ncolIni = colIniFecha;

                #endregion
            }

            #endregion

            ws.Column(colIniFecha).Width = 11;
            ws.Row(rowIniFecha).Height = 27;

            ws.Column(colIniEmpresa).Width = 60;
            ws.Column(colIniCentral).Width = 45;
            ws.Column(colIniTipoComb).Width = 27;
            ws.Column(colIniInicio).Width = 20;
            ws.Column(colIniFinalDecl).Width = 20;
            ws.Column(colIniRecep).Width = 20;
            ws.Column(colIniUnidades).Width = 12;
            ws.Column(colIniObservacion).Width = 50;

            ws.View.FreezePanes(rowIniFecha + 1, colIniFecha + 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.17.	Volúmenes o cantidad diaria de combustible consumido (asociado a la generación) por cada Unidad de Generación termoeléctrica.
        #region REPORTE_COMBUSTIBLE_CONSUMIDO_UNIDAD_TERMOELECTRICA

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteCombustibleConsumidoUnidadTermoelectricaHtml
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaVersion"></param>
        /// <returns></returns>
        public static string ReporteCombustibleConsumidoUnidadTermoelectricaHtml(List<MeMedicionxintervaloDTO> lista, DateTime fechaInicio, DateTime fechaFin, List<MeMedicionxintervaloDTO> listaVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            List<DateTime> listaFechas = new List<DateTime>();
            for (DateTime fecha = fechaInicio.Date; fecha <= fechaFin.Date; fecha = fecha.AddDays(1))
            {
                listaFechas.Add(fecha);
            }

            List<MeMedicionxintervaloDTO> listaCabeceraM = lista.GroupBy(x => new
            {
                x.Ptomedicodi,
                x.Emprnomb,
                x.Ptomedibarranomb,
                x.Tipoinfoabrev,
                x.Tptomedicodi,
                x.Fenergnomb,
                x.Fenercolor,
                x.Equinomb,
                x.Emprcoes,
                x.Ptomedielenomb,
                x.Equipadre,
                x.Equipopadre
            })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Fenercolor = y.Key.Fenercolor,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equipopadre).ThenBy(x => x.Equinomb).ThenBy(x => x.Fenergnomb).ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            int padding = 20;
            int anchoTotal = (850 + 5 * padding) + (listaFechas.Count * (120 + padding));

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:210px;'>Empresa</th>");
            strHtml.Append("<th style='width:280px;'>Central</th>");
            strHtml.Append("<th style='width:150px;'>Medidor</th>");
            strHtml.Append("<th style='width:125px;'>Tipo</th>");
            strHtml.Append("<th style='width:80px;'>Unidad</th>");
            for (int i = 0; i < listaFechas.Count; i++)
            {
                strHtml.Append("<th style='width:140px;'>" + fechaInicio.AddDays(i).ToString(ConstantesAppServicio.FormatoDiaMes) + "</th>");
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var reg in listaCabeceraM)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td style='width:200px;'>" + reg.Emprnomb + "</td>");
                strHtml.Append("<td style='width:260px;'>" + reg.Equipopadre + "</td>");
                strHtml.Append("<td style='width:120px;'>" + reg.Equinomb + "</td>");
                strHtml.AppendFormat("<td style='width:125px;'><div class='symbol' style='background-color:{0}'></div><div class='serieName' style='padding-top: 4px;'>{1}</div></td>", reg.Fenercolor, reg.Fenergnomb);
                strHtml.Append("<td style='width:50px;'>" + reg.Tipoinfoabrev + "</td>");

                for (var day = fechaInicio.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    var fil = lista.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == day);
                    var datVersi = listaVersion.Find(x => x.Medintcodi == fil.Medintcodi);

                    if (fil != null)
                    {
                        decimal? _descrip = fil.Medinth1;
                        string _bground = string.Empty;
                        if (datVersi != null) { if (_descrip != datVersi.Medinth1) { _descrip = datVersi.Medinth1; _bground = "lightgreen"; } }
                        strHtml.AppendFormat("<td style='width:140px; background:" + _bground + "'>{0}</td>", _descrip != null ? _descrip.Value.ToString("N", nfi) : "--");
                    }
                    else
                        strHtml.Append("<td style='width:140px'>--</td>");
                }

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptCombustibleConsumidoUnidadTermoelectrica
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="titulo"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptCombustibleConsumidoUnidadTermoelectrica(ExcelWorksheet ws, ref int filaIniTitulo, ref int coluIniTitulo, string titulo, int tipoReporte, DateTime fecha1, DateTime fecha2, List<MeMedicionxintervaloDTO> lista, List<MeMedicionxintervaloDTO> listaVersion)
        {
            List<DateTime> listaFechas = new List<DateTime>();
            for (DateTime fecha = fecha1.Date; fecha <= fecha2.Date; fecha = fecha.AddDays(1))
            {
                listaFechas.Add(fecha);
            }

            List<MeMedicionxintervaloDTO> listaCabeceraM = lista.GroupBy(x => new
            {
                x.Ptomedicodi,
                x.Emprnomb,
                x.Ptomedibarranomb,
                x.Tipoinfoabrev,
                x.Tptomedicodi,
                x.Fenergnomb,
                x.Equinomb,
                x.Emprcoes,
                x.Ptomedielenomb,
                x.Equipadre,
                x.Equipopadre
            })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equipopadre).ThenBy(x => x.Equinomb).ThenBy(x => x.Fenergnomb).ToList();

            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int row = filaIniTitulo + 1;
            int col = coluIniTitulo;


            #region cabecera
            int colIniTituloDesc = col;
            int rowIniTituloDesc = row;
            ws.Cells[rowIniTituloDesc, colIniTituloDesc].Value = tipoReporte == 1 ? "CONSUMO DE COMBUSTIBLES - LÍQUIDO, SÓLIDO, GASEOSO" : "CONSUMO DE COMBUSTIBLES - GASEOSO";

            int colIniEmpresa = colIniTituloDesc;
            int rowIniEmpresa = rowIniTituloDesc + 1;
            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "EMPRESA";

            int colIniCentral = colIniEmpresa + 1;
            int rowIniCentral = rowIniEmpresa;
            ws.Cells[rowIniCentral, colIniCentral].Value = "CENTRAL";

            int colIniMedidor = colIniCentral + 1;
            int rowIniMedidor = rowIniEmpresa;
            ws.Cells[rowIniMedidor, colIniMedidor].Value = "MEDIDOR";

            int colIniTipoComb = colIniMedidor + 1;
            int rowIniTipoComb = rowIniEmpresa;
            ws.Cells[rowIniTipoComb, colIniTipoComb].Value = "TIPO DE COMBUSTIBLE";

            int colIniUnidades = colIniTipoComb + 1;
            int rowIniUnidades = rowIniEmpresa;
            ws.Cells[rowIniUnidades, colIniUnidades].Value = "UNIDAD";

            int rowIniFechas = rowIniEmpresa;
            int colIniFechas = colIniUnidades + 1;
            for (int i = 0; i < listaFechas.Count; i++)
            {
                ws.Cells[rowIniFechas, colIniFechas + i].Value = listaFechas[i].ToString(ConstantesAppServicio.FormatoDiaMes);
            }
            int colFinFechas = colIniFechas + listaFechas.Count - 1;

            int colFinTituloDesc = colFinFechas;

            #region Formato Cabecera

            UtilExcel.CeldasExcelAgrupar(ws, rowIniTituloDesc, colIniTituloDesc, rowIniTituloDesc, colFinTituloDesc);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniTituloDesc, colIniTituloDesc, rowIniEmpresa, colFinTituloDesc, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region cuerpo

            int rowIniData = rowIniFechas + 1;
            int rowFinData = rowIniData + 1;
            if (lista.Count > 0)
            {
                row = rowIniData;

                foreach (var reg in listaCabeceraM)
                {
                    ws.Cells[row, colIniEmpresa].Value = reg.Emprnomb;
                    ws.Cells[row, colIniCentral].Value = reg.Equipopadre;
                    ws.Cells[row, colIniMedidor].Value = reg.Equinomb;
                    ws.Cells[row, colIniTipoComb].Value = reg.Fenergnomb;
                    ws.Cells[row, colIniUnidades].Value = reg.Tipoinfoabrev;

                    int i = 0;
                    for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                    {
                        decimal? valor;
                        var fil = lista.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == day);
                        if (fil != null)
                        {
                            valor = fil.Medinth1;
                            if (valor != null)
                            {
                                ws.Cells[row, colIniFechas + i].Value = valor;
                            }
                            else
                                ws.Cells[row, colIniFechas + i].Value = "--";
                        }
                        else
                            ws.Cells[row, colIniFechas + i].Value = "--";

                        i++;
                    }
                    row++;
                }

                rowFinData = rowIniData + listaCabeceraM.Count() - 1;

                #region Formato Cuerpo
                //UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniMedidor, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniUnidades, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colFinFechas, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colFinFechas, "Arial", 10);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniEmpresa, rowFinData, colFinFechas);
                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniEmpresa, rowFinData, colFinFechas);

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFechas, rowFinData, colFinFechas, "Derecha");
                ws.Cells[rowIniData, colIniFechas, rowFinData, colFinFechas].Style.Numberformat.Format = "#,##0.000";

                #endregion
            }

            #endregion

            ws.Row(rowIniTituloDesc).Height = 27;
            ws.Row(rowIniEmpresa).Height = 27;

            ws.Column(colIniEmpresa).Width = 53;
            ws.Column(colIniCentral).Width = 45;
            ws.Column(colIniMedidor).Width = 30;
            ws.Column(colIniTipoComb).Width = 27;
            ws.Column(colIniUnidades).Width = 9;
            int colCont = 0;
            for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
            {
                ws.Column(colIniFechas + colCont).Width = 15;
                colCont++;
            }
            ws.View.FreezePanes(rowIniEmpresa + 1, colIniUnidades + 1);
            ws.View.ZoomScale = 80;

            //
            filaIniTitulo = rowFinData;
        }

        #endregion

        // 3.13.2.18.	Volúmenes diarios de gas natural consumido (asociado a la generación) y presión horaria del gas natural al ingreso (en el lado de alta presión) de cada Unidad de Generación termoeléctrica a gas natural.
        #region REPORTE_CONSUMO_Y_PRESION_DIARIO_UNIDAD_TERMOELECTRICA

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte consumo
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteConsumoDiarioUnidadTermoelectricaHtml(List<MeMedicionxintervaloDTO> listaReporte, List<MeMedicionxintervaloDTO> dataVersion)
        {
            string idsEstado = "3"; //liquido, solido, gaseoso

            var listaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();

            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            StringBuilder strHtml = new StringBuilder();
            List<MeMedicionxintervaloDTO> listaCabeceraM = listaReporte.GroupBy(x => new
            {
                x.Ptomedicodi,
                x.Emprnomb,
                x.Ptomedibarranomb,
                x.Tipoinfoabrev,
                x.Tptomedicodi,
                x.Fenergnomb,
                x.Fenercolor,
                x.Equinomb,
                x.Emprcoes,
                x.Ptomedielenomb,
                x.Equipadre,
                x.Equipopadre
            })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Fenercolor = y.Key.Fenercolor,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equipopadre).ThenBy(x => x.Equinomb).ThenBy(x => x.Fenergnomb).ToList();

            int padding = 20;
            int anchoTotal = (850 + 5 * padding) + (listaFechas.Count * (120 + padding));

            strHtml.AppendFormat("<div class='title-seccion'>{0}</div>", StockCombustiblesAppServicio.GeneraTituloListado("CONSUMO DE COMBUSTIBLES - ", idsEstado));

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:210px;'>Empresa</th>");
            strHtml.Append("<th style='width:280px;'>Central</th>");
            strHtml.Append("<th style='width:150px;'>Medidor</th>");
            strHtml.Append("<th style='width:110px;'>Tipo</th>");
            strHtml.Append("<th style='width:80px;'>Unidad</th>");
            foreach (var reg in listaFechas)
            {
                strHtml.Append(string.Format("<th style='width:140px'>{0}</th>", reg.ToString(ConstantesStockCombustibles.FormatoDiaMes)));

            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            foreach (var reg in listaCabeceraM)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td style='width:200px;'>" + reg.Emprnomb + "</td>");
                strHtml.Append("<td style='width:260px;'>" + reg.Equipopadre + "</td>");
                strHtml.Append("<td style='width:120px;'>" + reg.Equinomb + "</td>");

                strHtml.AppendFormat("<td style='width:90px;'><div class='symbol' style='background-color:{0}'></div><div class='serieName' style='padding-top: 4px;'>{1}</div></td>", reg.Fenercolor, reg.Fenergnomb);
                strHtml.Append("<td style='width:50px;'>" + reg.Tipoinfoabrev + "</td>");

                foreach (var reg2 in listaFechas)
                {
                    decimal? valor;
                    string strValor = "--";
                    var fil = listaReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg2);
                    if (fil != null)
                    {
                        valor = fil.Medinth1;
                        if (valor != null)
                            strValor = valor.Value.ToString("N", nfi);
                    }
                    strHtml.AppendFormat("<td style='width:140px'>{0}</td>", strValor);
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte presion gas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public static string ReportePresionDiarioUnidadTermoelectricaHtml(List<MeMedicion24DTO> listaReporte, DateTime fechaIni, DateTime fechaFin, List<MeMedicion24DTO> dataVersion, int idParametro)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            StringBuilder strHtml = new StringBuilder();

            List<MeMedicion24DTO> listaCabeceraM24 = new List<MeMedicion24DTO>();

            if (idParametro == 1) // Presión de Gas
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprcodi = y.First().Emprcodi,
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedielenomb).ToList();
            }
            else //Temperatura Ambiente
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprcodi = y.First().Emprcodi,
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();
            }
            int nCol = listaCabeceraM24.Count;
            if (idParametro == 1) //Presión de Gas
                strHtml.Append("<div class='title-seccion'>HISTÓRICO PRESIÓN DE GAS</div>");
            else //Temperatura Ambiente 
                strHtml.Append("<div class='title-seccion'>HISTÓRICO TEMPERATURA AMBIENTE(ºC)</div>");

            int padding = 20;
            int anchoTotal = (100 + padding) + (nCol * (110 + padding));

            strHtml.Append("<div id='resultado'>");
            strHtml.AppendFormat("<table  class='pretty tabla-icono' id='reporte' style='table-layout: fixed; width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px'></th>");

            if (listaReporte.Count > 0)
            {
                if (idParametro == 1) //Presión de Gas
                    strHtml.Append("<th colspan = '" + nCol + "'>PRESIÓN GAS (kPa)</th>");
                else //Temperatura Ambiente
                    strHtml.Append("<th colspan = '" + nCol + "'>TEMPERATURA AMBIENTE(ºC)</th>");
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px'></th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append(string.Format("<th style='width:200px;overflow-wrap: break-word; white-space: normal;'>{0}</th>", reg.Emprnomb));
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px'>HORA</th>");
            foreach (var reg in listaCabeceraM24)
            {
                if (idParametro == 1)//Presión de Gas
                    strHtml.Append(string.Format("<th style='width:200px;overflow-wrap: break-word; white-space: normal;'>{0}</th>", reg.Ptomedielenomb));
                else //Temperatura Ambiente
                    strHtml.Append(string.Format("<th style='width:200px;overflow-wrap: break-word; white-space: normal;'>{0}</th>", reg.Equinomb));
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day;
                for (int h = 1; h <= 24; h++)
                {
                    var fechaMin = horas.ToString(ConstantesStockCombustibles.FormatoFechaHora);
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                    foreach (var reg in listaCabeceraM24)
                    {
                        MeMedicion24DTO p = listaReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == day && x.Emprcodi == reg.Emprcodi);

                        decimal? valor = p != null ? (decimal?)p.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(p, null) : null;
                        if (valor != null)
                            strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                    }

                    strHtml.Append("</tr>");
                    horas = horas.AddMinutes(60);
                }
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Muestra los datos de consulta para el listado de La Presión de Gas ó Temperatura Ambiente
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="xFil"></param>
        /// <param name="xCol"></param>
        /// <param name="listaReporte"></param>
        /// <param name="idParametro"></param>
        public static void ConfiguracionHojaExcelPresTemp(ExcelWorksheet ws, DateTime fechaIni, DateTime fechaFin, ref int xFil, ref int xCol, List<MeMedicion24DTO> listaReporte, int idParametro)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            List<DateTime> listaFechas = new List<DateTime>();
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                listaFechas.Add(day);
            }

            var titulo = string.Empty;

            List<MeMedicion24DTO> listaCabeceraM24 = new List<MeMedicion24DTO>();
            if (idParametro == 1) // Presión de Gas
            {
                titulo = "PRESIÓN GAS(kPa)";
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Gruponomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Gruponomb = y.Key.Gruponomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedielenomb).ToList();
            }
            else //Temperatura Ambiente
            {
                titulo = "TEMPERATURA AMBIENTE (ºC)";
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();
            }

            int nfil = listaReporte.Count;
            int ncol = listaCabeceraM24.Count;

            #region Cabecera

            int colIniHora = xCol;
            int rowIniTitulo = xFil + 1;
            int colIniTitulo = colIniHora + 1;
            int colFinTitulo = colIniTitulo + nfil - 1;
            int rowIniEmpresa = rowIniTitulo + 1;
            int rowIniPto = rowIniEmpresa + 1;

            ws.Cells[rowIniTitulo, colIniHora].Value = "HORA";
            ws.Cells[rowIniTitulo, colIniHora, rowIniPto, colIniHora].Merge = true;

            if (colFinTitulo >= colIniTitulo)
            {
                ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
                ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Merge = true;
            }

            int colTituloTmp = colIniTitulo;
            foreach (var reg in listaCabeceraM24)
            {
                if (idParametro == 1) // Presión de Gas
                {
                    ws.Cells[rowIniEmpresa, colTituloTmp].Value = reg.Emprnomb;
                    ws.Cells[rowIniPto, colTituloTmp].Value = reg.Ptomedielenomb;
                }
                else //Temperatura Ambiente
                {
                    ws.Cells[rowIniEmpresa, colTituloTmp].Value = reg.Emprnomb;
                    ws.Cells[rowIniPto, colTituloTmp].Value = reg.Equinomb;
                }
                colTituloTmp++;
            }

            #region Formato Cabecera

            UtilExcel.CeldasExcelColorTexto(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo);
            UtilExcel.CeldasExcelWrapText(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniTitulo, colIniHora, rowIniPto, colFinTitulo, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region Cuerpo
            //Cuerpo
            int nBloques = 24;
            int resolucion = 60;
            int rowIniData = rowIniPto + 1;
            int row = rowIniData;
            for (int j = 0; j < listaFechas.Count; j++)
            {
                for (int k = 1; k <= nBloques; k++)
                {
                    int col = colIniHora + 1;
                    ws.Cells[row, colIniHora].Value = listaFechas[j].AddMinutes((k - 1) * resolucion).ToString(ConstantesAppServicio.FormatoFechaHora);
                    foreach (var reg in listaCabeceraM24)
                    {
                        decimal? valor;
                        var p = listaReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == listaFechas[j]);
                        if (p != null)
                        {
                            valor = (decimal?)p.GetType().GetProperty("H" + k).GetValue(p, null);
                            if (valor != null)
                                ws.Cells[row, col].Value = valor;
                            else
                                ws.Cells[row, col].Value = "--";
                        }
                        col++;
                    }
                    row++;
                }
            }
            int rowFinData = row - 1;

            #region Formato Cuerpo
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniHora, rowFinData, colIniHora, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniHora, rowFinData, colFinTitulo, "Centro");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniHora, rowFinData, colFinTitulo, "Arial", 10);

            UtilExcel.CeldasExcelEnNegrita(ws, rowIniData, colIniHora, rowFinData, colIniHora);
            UtilExcel.BorderCeldas2(ws, rowIniData, colIniHora, rowFinData, colFinTitulo);

            if (colFinTitulo >= colIniTitulo)
            {
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniTitulo, rowFinData, colFinTitulo, "Derecha");
                ws.Cells[rowIniData, colIniHora, rowFinData, colFinTitulo].Style.Numberformat.Format = "#,##0.000";
            }

            #endregion

            #endregion

            ws.Row(rowIniTitulo).Height = 27;
            ws.Row(rowIniEmpresa).Height = 40;
            ws.Row(rowIniPto).Height = 40;

            ws.Column(colIniHora).Width = 20;
            for (int i = colIniTitulo; i <= colFinTitulo; i++)
            {
                ws.Column(i).Width = 23;
            }

            ws.View.ZoomScale = 80;
        }

        #endregion

        #region Disponibilidad de Gas

        /// <summary>
        /// GeneraRptDisponibilidadGasUnidadTermoelectrica
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="titulo"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="listaData"></param>
        public static void GeneraRptDisponibilidadGasUnidadTermoelectrica(ExcelWorksheet ws, ref int filaIniTitulo, ref int coluIniTitulo, string titulo,
            DateTime fecha1, DateTime fecha2, List<MeMedicionxintervaloDTO> listaData)
        {
            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int row = filaIniTitulo + 1;
            int col = coluIniTitulo;

            #region cabecera

            int colFecha = col;
            int colEmpresa = colFecha + 1;
            int colGaseoducto = colEmpresa + 1;
            int colVolumen = colGaseoducto + 1;
            int colIni = colVolumen + 1;
            int colFin = colIni + 1;
            int colObs = colFin + 1;
            int rowIniFecha = row + 1;

            ws.Cells[rowIniFecha, colFecha].Value = "FECHA";
            ws.Cells[rowIniFecha, colEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniFecha, colGaseoducto].Value = "GASEODUCTO";
            ws.Cells[rowIniFecha, colVolumen].Value = "VOLUMEN DE GAS (Mm3)";
            ws.Cells[rowIniFecha, colIni].Value = "INICIAL";
            ws.Cells[rowIniFecha, colFin].Value = "FINAL";
            ws.Cells[rowIniFecha, colObs].Value = "OBSERVACIONES";

            #region Formato Cabecera

            UtilExcel.CeldasExcelWrapText(ws, rowIniFecha, colFecha, rowIniFecha, colObs);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniFecha, colFecha, rowIniFecha, colObs, "#FFFFFF");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colFecha, rowIniFecha, colObs, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniFecha, colFecha, rowIniFecha, colObs, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colFecha, rowIniFecha, colObs);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniFecha, colFecha, rowIniFecha, colObs, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region cuerpo

            int rowIniData = rowIniFecha + 1;
            int rowFinData = rowIniData + 1;
            if (listaData.Count > 0)
            {
                row = rowIniData;

                foreach (var reg in listaData)
                {
                    ws.Cells[row, colFecha].Value = reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoFecha);
                    ws.Cells[row, colEmpresa].Value = reg.Emprnomb;
                    ws.Cells[row, colGaseoducto].Value = reg.Equinomb;
                    ws.Cells[row, colVolumen].Value = reg.Medinth1;
                    ws.Cells[row, colIni].Value = string.Format("{0} 06:00", reg.Medintfechaini.ToString(ConstantesStockCombustibles.FormatoFecha));
                    ws.Cells[row, colFin].Value = string.Format("{0} 06:00", reg.Medintfechaini.AddDays(1).ToString(ConstantesStockCombustibles.FormatoFecha));
                    ws.Cells[row, colObs].Value = reg.Medintdescrip;

                    row++;
                }

                rowFinData = rowIniData + listaData.Count() - 1;

                #region Formato Cuerpo

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colFecha, rowFinData, colFecha, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIni, rowFinData, colFin, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colFecha, rowFinData, colObs, "Centro");

                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colFecha, rowFinData, colObs, "Arial", 10);

                UtilExcel.BorderCeldas2(ws, rowIniData, colFecha, rowFinData, colObs);
                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colObs, rowFinData, colObs);

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colVolumen, rowFinData, colVolumen, "Derecha");
                ws.Cells[rowIniData, colVolumen, rowFinData, colVolumen].Style.Numberformat.Format = "#,##0.000";

                #endregion
            }

            #endregion

            ws.Row(rowIniFecha).Height = 45;

            ws.Column(colFecha).Width = 15;
            ws.Column(colEmpresa).Width = 35;
            ws.Column(colGaseoducto).Width = 45;
            ws.Column(colVolumen).Width = 17;
            ws.Column(colIni).Width = 18;
            ws.Column(colFin).Width = 18;
            ws.Column(colObs).Width = 40;

            ws.View.FreezePanes(rowIniFecha + 1, colGaseoducto + 1);
            ws.View.ZoomScale = 80;

            //
            filaIniTitulo = rowFinData;
        }

        #endregion

        // 3.13.2.19.	Reporte cada 30 minutos de la fuente de energía primaria de las unidades RER solar, geotérmica y biomasa. En caso de las Centrales Eólicas, la velocidad del viento registrada cada 30 minutos.
        #region REPORTE_REGISTRO_ENERGIA_30_UNIDADES

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteRegistoEnergia30UnidadesHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaPto"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteRegistroEnergia30UnidadesHtml(List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto, DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> dataVersion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * (500 + padding);
            List<MePtomedicionDTO> lOrdenado = new List<MePtomedicionDTO>();

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            #region cabecera

            // Equipos
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 110px' rowspan='6'> Día / Hora</th>");
            List<SiEmpresaDTO> listaEmpresa = listaPto.GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi.Value, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ToList();
            foreach (var obj in listaEmpresa)
            {
                List<PrGrupoDTO> listaCentrales = listaPto.Where(x => x.Emprcodi == obj.Emprcodi).GroupBy(x => new { x.Grupopadre, x.Grupocentral })
                    .Select(x => new PrGrupoDTO() { Grupopadre = x.Key.Grupopadre, Central = x.Key.Grupocentral })
                    .OrderBy(x => x.Central).ToList();

                foreach (var central in listaCentrales)
                {
                    List<MePtomedicionDTO> listaGrupo = listaPto.Where(x => x.Emprcodi == obj.Emprcodi && x.Grupopadre == central.Grupopadre).OrderBy(x => x.Equiabrev).ToList();
                    foreach (var grupo in listaGrupo)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px'>{0}</th>", grupo.Ptomedicodi);
                    }
                }
            }
            strHtml.Append("</tr>");

            // Empresas
            strHtml.Append("<tr>");
            foreach (var obj in listaEmpresa)
            {
                List<MePtomedicionDTO> listaPtoXEmpresa = listaPto.Where(x => x.Emprcodi == obj.Emprcodi).ToList();
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px' colspan='{1}'>{0}</th>", obj.Emprnomb.ToString(), listaPtoXEmpresa.Count);
            }
            strHtml.Append("</tr>");

            // Centrales
            strHtml.Append("<tr>");
            foreach (var obj in listaEmpresa)
            {
                List<PrGrupoDTO> listaCentrales = listaPto.Where(x => x.Emprcodi == obj.Emprcodi).GroupBy(x => new { x.Grupopadre, x.Grupocentral })
                    .Select(x => new PrGrupoDTO() { Grupopadre = x.Key.Grupopadre, Central = x.Key.Grupocentral })
                    .OrderBy(x => x.Central).ToList();

                foreach (var central in listaCentrales)
                {
                    List<MePtomedicionDTO> listaPtoXCentral = listaPto.Where(x => x.Emprcodi == obj.Emprcodi && x.Grupopadre == central.Grupopadre).OrderBy(x => x.Equiabrev).ToList();

                    strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px' colspan='{1}'>{0}</th>", central.Central, listaPtoXCentral.Count);
                }
            }
            strHtml.Append("</tr>");

            // Equipos
            strHtml.Append("<tr>");
            foreach (var obj in listaEmpresa)
            {
                List<PrGrupoDTO> listaCentrales = listaPto.Where(x => x.Emprcodi == obj.Emprcodi).GroupBy(x => new { x.Grupopadre, x.Grupocentral })
                    .Select(x => new PrGrupoDTO() { Grupopadre = x.Key.Grupopadre, Central = x.Key.Grupocentral })
                    .OrderBy(x => x.Central).ToList();

                foreach (var central in listaCentrales)
                {
                    List<MePtomedicionDTO> listaGrupo = listaPto.Where(x => x.Emprcodi == obj.Emprcodi && x.Grupopadre == central.Grupopadre).OrderBy(x => x.Equiabrev).ToList();
                    foreach (var grupo in listaGrupo)
                    {
                        strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px'>{0}</th>", grupo.Equiabrev);

                        lOrdenado.Add(grupo);
                    }
                }
            }
            strHtml.Append("</tr>");

            // Tipo de punto de medicion
            strHtml.Append("<tr>");
            foreach (var obj in lOrdenado)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 150px'>{0}</th>", obj.Tipoptomedinomb);
            }
            strHtml.Append("</tr>");

            // Infocodi
            strHtml.Append("<tr>");
            foreach (var obj in lOrdenado)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word;white-space: normal;'>{0}</th>", obj.Tipoinfoabrev);
            }
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            // Día - Hora
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    foreach (var pto in lOrdenado)
                    {
                        MeMedicion48DTO regpotActiva = data.Find(x => x.Emprcodi == pto.Emprcodi && x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                        decimal? pot = null;
                        if (regpotActiva != null)
                        {
                            pot = (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null);
                        }

                        strHtml.AppendFormat("<td>{0}</td>", pot != null ? pot.Value.ToString("N", nfi) : string.Empty);
                    }

                    strHtml.Append("</tr>");

                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de fuente de Energia Primaria
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        public static void GeneraRptRegistroEnergia30Unidades(ExcelWorksheet ws, string nombreSheet, int rowIni, int colIni, bool flagVisiblePtomedicodi, DateTime fecha1, DateTime fecha2, List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto)
        {
            List<MePtomedicionDTO> lOrdenado = new List<MePtomedicionDTO>();

            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaPto.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                // Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Merge = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.WrapText = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniPto = row;
                int colIniPto = colIniHora + 1;
                int colFinPto = colIniPto;

                int rowIniEmp = rowIniPto + 1;
                int colIniEmp = colIniHora + 1;
                int colFinEmp = colIniEmp;

                int rowIniEquipo = rowIniEmp + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo = colIniEmp;

                List<SiEmpresaDTO> listaEmpresa = listaPto.GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                    .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi.Value, Emprnomb = x.Key.Emprnomb })
                    .OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ToList();

                int colIniNombreReporte = colIniEmp;
                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    int totalEqXemp = listaPto.Where(x => x.Emprcodi == thEmp.Emprcodi).ToList().Select(y => new { y.Equicodi, y.Equiabrev }).Distinct().ToList().OrderBy(c => c.Equiabrev).Count();

                    colFinEmp = colIniEmp + totalEqXemp - 1;
                    ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 9;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    colIniEmp = colFinEmp + 1;

                    //Central
                    List<PrGrupoDTO> listaCentrales = listaPto.Where(x => x.Emprcodi == thEmp.Emprcodi).GroupBy(x => new { x.Grupopadre, x.Grupocentral })
                                                                .Select(x => new PrGrupoDTO() { Grupopadre = x.Key.Grupopadre, Central = x.Key.Grupocentral })
                                                                .OrderBy(x => x.Central).ToList();

                    for (int j = 0; j < listaCentrales.Count; j++)
                    {
                        var thCentral = listaCentrales[j];
                        List<MePtomedicionDTO> listaGrupo = listaPto.Where(x => x.Grupopadre == thCentral.Grupopadre).OrderBy(x => x.Equiabrev).ToList();

                        for (int k = 0; k < listaGrupo.Count; k++)
                        {
                            var thUnidad = listaGrupo[k];

                            colFinEquipo = colIniEquipo;
                            ws.Cells[rowIniEquipo, colIniEquipo].Value = thUnidad.Ptomedielenomb.Trim();
                            ws.Cells[rowIniEquipo, colIniEquipo].Style.Font.Size = 8;
                            ws.Cells[rowIniEquipo, colIniEquipo].Style.WrapText = true;
                            ws.Cells[rowIniEquipo, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniEquipo, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniEquipo, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Cells[rowIniPto, colIniEquipo].Value = thUnidad.Ptomedicodi;
                            ws.Cells[rowIniPto, colIniEquipo].Style.Font.Size = 8;
                            ws.Cells[rowIniPto, colIniEquipo].Style.WrapText = true;
                            ws.Cells[rowIniPto, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[rowIniPto, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniPto, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            colIniEquipo = colFinEquipo + 1;

                            var reg = listaPto.Where(x => x.Equicodi == thUnidad.Equicodi).First();
                            lOrdenado.Add(reg);

                        }
                    }
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 2;
                int colFinNombreReporte = colFinEmp;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreSheet;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Style.Font.Size = 18;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.WrapText = true;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var colorBorder = Color.White;
                var classTipoEmpresa = ConstantesPR5ReportesServicio.ColorInfSGI;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniEquipo, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }


                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo

                decimal? valor;
                int numDia = 0;

                int colData = colIniHora;
                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        ws.Row(row).Height = 24;

                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                        ws.Cells[row, colIniFecha].Style.Font.Size = 10;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);

                        colData = colIniHora;
                        //Hora
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        ws.Cells[row, colIniHora].Style.Font.Bold = true;
                        ws.Cells[row, colIniHora].Style.Font.Size = 10;
                        ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

                        colData++;
                        foreach (var pto in lOrdenado)
                        {
                            MeMedicion48DTO regpotActiva = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                            valor = regpotActiva != null ? (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null) : null;
                            ws.Cells[row, colData].Value = valor;
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 48, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10;
                    range.Style.Numberformat.Format = "#,##0.0";
                }

                //mostrar lineas horas
                for (int c = colIniHora - 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                        ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                    }
                }

                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 20;

                ws.Column(colIniFecha).Width = 11;
                ws.Column(colIniHora).Width = 9;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniPto).Height = 22;
                ws.Row(rowIniEmp).Height = 40;
                ws.Row(rowIniEquipo).Height = 57;

                if (!flagVisiblePtomedicodi) ws.Row(rowIniPto).Hidden = true;

                ws.View.FreezePanes(8, 3);
                ws.View.ZoomScale = 100;

                #endregion
            }

            ws.View.ZoomScale = 85;
        }

        #endregion

        // 3.13.2.20.	En caso sea una Central de Cogeneración Calificada, deberá remitir información sobre la producción del Calor Útil de sus Unidades de Generación o el Calor Útil recibido del proceso industrial asociado, en MW
        #region REPORTE_CALOR_UTIL_GENERACION_PROCESO

        /// <summary>
        /// Metodo disenio de la tabla cabecera y detalle Reporte ListarReporteCalorUtilGeneracionProcesoHtml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaPto"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteCalorUtilGeneracionProcesoHtml(List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto, DateTime fechaIni, DateTime fechaFin,
                            List<MeMedicion48DTO> dataVersion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            if (listaPto.Count == 0)
                return string.Empty;

            StringBuilder strHtml = new StringBuilder();

            //***************************************************** CABECERA DE LA TABLA  ***************************************************************************************************

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");

            //Punto de medición
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:110px;' rowspan=5>HORA</th>");
            var listaEmpr = listaPto.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            foreach (var g in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer'>" + g.Ptomedicodi + "</th>");
            }
            strHtml.Append("<th rowspan='4' class='empresa_rer'>TOTAL CALOR ÚTIL</th>");
            strHtml.Append("</tr>");

            //Empresas
            strHtml.Append("<tr>");
            foreach (var item in listaEmpr)
            {
                var listaDataXEmpr = listaPto.Where(x => x.Emprcodi == item.Emprcodi).ToList();
                strHtml.AppendFormat("<th colspan='{0}' class='empresa_rer'>" + item.Emprnomb + "</th>", listaDataXEmpr.Count);
            }
            strHtml.Append("</tr>");

            //Centrales
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer'>" + item.Gruponomb + "</th>");
            }
            strHtml.Append("</tr>");

            //Equipo
            strHtml.Append("<tr>");
            foreach (var item in listaPto)
            {
                strHtml.AppendFormat("<th class='grupo_rer'>" + item.Ptomedidesc + "</th>");
            }
            strHtml.Append("</tr>");

            //Tipoinfocodi
            strHtml.Append("<tr>");
            for (int i = 0; i < listaPto.Count + 1; i++)
            {
                strHtml.AppendFormat("<th class='grupo_rer'>MW</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            // ****************************************  CUERPO DE LA TABLA ******************************************************         
            strHtml.Append("<tbody>");

            List<MeMedicion48DTO> listaAcumulado = new List<MeMedicion48DTO>();
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                MeMedicion48DTO m48 = new MeMedicion48DTO();
                m48.Medifecha = day;
                listaAcumulado.Add(m48);
            }
            decimal? valor;
            decimal total;

            // Día - Hora
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    var regAcum = listaAcumulado.Find(x => x.Medifecha == day);
                    total = ((decimal?)regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regAcum, null)).GetValueOrDefault(0);

                    foreach (var pto in listaPto)
                    {
                        MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                        valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                        if (valor != null)
                        {
                            total += valor.GetValueOrDefault(0);
                        }
                        strHtml.AppendFormat("<td>{0}</td>", valor != null ? valor.Value.ToString("N", nfi) : string.Empty);
                    }
                    strHtml.AppendFormat("<td>{0}</td>", total.ToString("N", nfi));

                    regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regAcum, total);
                    strHtml.Append("</tr>");

                    horas = horas.AddMinutes(30);
                }
            }

            //EJECUTADO
            foreach (var m48 in listaAcumulado)
            {
                valor = 0;
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor != null ? valor.Value : 0;
                }

                m48.Meditotal = total;
            }

            strHtml.Append("<tr>");
            strHtml.Append("<td class='total_potencia_activa_ejecutado'>EJEC MWh</td>");

            foreach (var pto48 in listaPto)
            {
                List<MeMedicion48DTO> lista48 = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi).ToList();
                decimal totalPto = lista48.Sum(x => x.Meditotal.GetValueOrDefault(0));
                strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", (totalPto / 2).ToString("N", nfi));
            }
            var totalAcum = listaAcumulado.Sum(x => x.Meditotal.GetValueOrDefault(0));
            strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", (totalAcum / 2).ToString("N", nfi));

            strHtml.Append("</tr>");

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        public static void GeneraRptCalorUtilGeneracionProceso(ExcelWorksheet ws, string nombreSheet, int rowIni, int colIni, bool flagVisiblePtomedicodi, DateTime fechaIni, DateTime fechaFin
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaPto.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                // Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 3 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Merge = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.WrapText = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniPto = row;
                int colIniPto = colIniHora + 1;
                int colFinPto = colIniPto;

                int rowIniEmp = rowIniPto + 1;
                int colIniEmp = colIniHora + 1;
                int colFinEmp = colIniEmp;

                int rowIniEquipo = rowIniEmp + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo = colIniEmp;

                var listaEmpresa = listaPto.GroupBy(x => new { x.Emprcodi, x.Emprnomb }).Select(x => new { x.Key.Emprcodi, x.Key.Emprnomb }).OrderBy(x => x.Emprnomb).ToList();

                int colIniNombreReporte = colIniEmp;
                for (int i = 0; i < listaEmpresa.Count; i++)
                {
                    //Empresa
                    var thEmp = listaEmpresa[i];
                    int totalEqXemp = listaPto.Where(x => x.Emprcodi == thEmp.Emprcodi).Count();

                    colFinEmp = colIniEmp + totalEqXemp - 1;
                    ws.Cells[rowIniEmp, colIniEmp].Value = thEmp.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Font.Size = 9;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    colIniEmp = colFinEmp + 1;

                    List<MePtomedicionDTO> listaPtoXEmp = listaPto.Where(x => x.Emprcodi == thEmp.Emprcodi).ToList();

                    for (int k = 0; k < listaPtoXEmp.Count; k++)
                    {
                        var thUnidad = listaPtoXEmp[k];

                        colFinEquipo = colIniEquipo;
                        ws.Cells[rowIniEquipo, colIniEquipo].Value = thUnidad.Ptomedidesc.Trim();
                        ws.Cells[rowIniEquipo, colIniEquipo].Style.Font.Size = 8;
                        ws.Cells[rowIniEquipo, colIniEquipo].Style.WrapText = true;
                        ws.Cells[rowIniEquipo, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniEquipo, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniEquipo, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        ws.Cells[rowIniPto, colIniEquipo].Value = thUnidad.Ptomedicodi;
                        ws.Cells[rowIniPto, colIniEquipo].Style.Font.Size = 8;
                        ws.Cells[rowIniPto, colIniEquipo].Style.WrapText = true;
                        ws.Cells[rowIniPto, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[rowIniPto, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowIniPto, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colIniEquipo = colFinEquipo + 1;

                    }
                }

                int colIniTotal = colFinEmp + 1;
                int colFinTotal = colIniTotal;
                int rowIniTotal = rowIniPto;
                int rowFinTotal = rowIniEquipo;

                //Descripcion Total - medida
                ws.Cells[rowIniTotal, colIniTotal].Value = "TOTAL CALOR ÚTIL";
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Merge = true;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.WrapText = true;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTotal, colIniTotal, rowFinTotal - 1, colFinTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowFinTotal, colIniTotal].Value = "MW";
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Merge = true;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.WrapText = true;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowFinTotal, colIniTotal, rowFinTotal, colFinTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 2;
                int colFinNombreReporte = colFinTotal;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreSheet;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Style.Font.Size = 18;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.WrapText = true;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var colorBorder = Color.White;
                var classTipoEmpresa = ConstantesPR5ReportesServicio.ColorInfSGI;
                using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniEquipo, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorBorder);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorBorder);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorBorder);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorBorder);
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                    range.Style.Font.Color.SetColor(Color.White);
                }


                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo
                List<MeMedicion48DTO> listaAcumulado = new List<MeMedicion48DTO>();
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    MeMedicion48DTO m48 = new MeMedicion48DTO();
                    m48.Medifecha = day;
                    listaAcumulado.Add(m48);
                }
                decimal? valor;
                decimal total;
                int numDia = 0;

                int colData = colIniHora;
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        ws.Row(row).Height = 24;

                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                        ws.Cells[row, colIniFecha].Style.Font.Size = 10;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);

                        colData = colIniHora;
                        //Hora
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        ws.Cells[row, colIniHora].Style.Font.Bold = true;
                        ws.Cells[row, colIniHora].Style.Font.Size = 10;
                        ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

                        colData++;

                        var regAcum = listaAcumulado.Where(x => x.Medifecha == day).First();
                        total = ((decimal?)regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regAcum, null)).GetValueOrDefault(0);
                        foreach (var pto in listaPto)
                        {
                            MeMedicion48DTO regpotActiva = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                            valor = regpotActiva != null ? (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null) : null;
                            ws.Cells[row, colData].Value = valor;
                            colData++;
                            total += valor.GetValueOrDefault(0);
                        }

                        regAcum.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regAcum, total);
                        ws.Cells[row, colData].Value = total;
                        colData++;

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                //EJECUTADO
                foreach (var m48 in listaAcumulado)
                {
                    valor = 0;
                    total = 0;
                    for (int h = 1; h <= 48; h++)
                    {
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        total += valor != null ? valor.Value : 0;
                    }

                    m48.Meditotal = total;
                }

                int rowEjec = row;
                ws.Cells[rowEjec, colIniFecha].Value = "EJEC MWh";
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Merge = true;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Bold = true;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Size = 12;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.WrapText = true;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);
                //Formatear data
                colData = colIniHora + 1;

                var totalAcum = listaAcumulado.Sum(x => x.Meditotal.GetValueOrDefault(0));
                foreach (var pto48 in listaPto)
                {
                    List<MeMedicion48DTO> lista48 = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi).ToList();
                    decimal totalPto = lista48.Sum(x => x.Meditotal.GetValueOrDefault(0));

                    ws.Cells[row, colData].Value = totalPto / 2;
                    ws.Cells[row, colData].Style.Font.Bold = true;
                    ws.Cells[row, colData].Style.Font.Size = 12;
                    ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6DCE4"));
                    ws.Cells[row, colData].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    ws.Cells[row, colData].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                    ws.Cells[row, colData].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    ws.Cells[row, colData].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                    ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colData].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[row, colData].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[row, colData].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[row, colData].Style.Border.Right.Color.SetColor(Color.Blue);
                    colData++;
                }

                ws.Cells[row, colData].Value = totalAcum / 2;
                ws.Cells[row, colData].Style.Font.Bold = true;
                ws.Cells[row, colData].Style.Font.Size = 12;
                ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells[row, colData].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, colData].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                ws.Cells[row, colData].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, colData].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, colData].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[row, colData].Style.Border.Left.Color.SetColor(Color.Blue);
                ws.Cells[row, colData].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[row, colData].Style.Border.Right.Color.SetColor(Color.Blue);
                colData++;

                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 48, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Numberformat.Format = "#,##0.0";
                    range.Style.Font.Size = 10;
                }
                using (var range = ws.Cells[rowIniData + numDia * 48, colIniHora + 1, rowIniData + numDia * 48, colData - 1])
                {
                    range.Style.Font.Size = 12;
                }

                //mostrar lineas horas
                for (int c = colIniHora - 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                        ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                    }
                }

                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 20;

                ws.Column(colIniFecha).Width = 11;
                ws.Column(colIniHora).Width = 9;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniPto).Height = 22;
                ws.Row(rowIniEmp).Height = 40;
                ws.Row(rowIniEquipo).Height = 57;

                if (!flagVisiblePtomedicodi) ws.Row(rowIniPto).Hidden = true;

                ws.View.FreezePanes(8, 3);
                ws.View.ZoomScale = 100;

                #endregion
            }

            ws.View.ZoomScale = 85;
        }

        #endregion

        #endregion

        #region SISTEMA DE TRANSMISIÓN

        // 3.13.2.21.	Registro cada 30 minutos del flujo (MW y MVAr) por las líneas de transmisión y transformadores de potencia definidos por el COES.
        #region REPORTE_REPORTE_PA_LINEAS_TRANSMISION

        /// <summary>
        /// ReporteFlujoPotenciaActivaTransmisionSEINHtml
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idPotencia"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        /// <returns></returns>
        public static string ReporteFlujoPotenciaActivaTransmisionSEINHtml(DateTime fechaIni, DateTime fechaFin, int idPotencia, List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            List<MePtomedicionDTO> lOrdenado = new List<MePtomedicionDTO>();
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            int totalPto = listaPto.Count();

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");

            #region cabecera

            int n = 0;

            //Punto
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>CÓDIGO</th>");
            foreach (var punto in listaPto)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", punto.Ptomedicodi);
                n++;
            }
            strHtml.Append("</tr>");

            //Scada
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>CANAL SCADA</th>");
            foreach (var punto in listaPto)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", punto.Canales);
                n++;
            }
            strHtml.Append("</tr>");

            //Subestacion
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px'>SS.EE</th>");
            List<MePtomedicionDTO> listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPto);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPto[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Subestacion != regPto.Subestacion);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px' colspan={1}>{0}</th>", regPto.Subestacion, numElemSig);
            }
            strHtml.Append("</tr>");

            //Equipo
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>EQUIPO</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", punto.Repptonomb, back_color);
            }
            strHtml.Append("</tr>");

            string tipoinf = (idPotencia == 1) ? "MW" : "MVAR";

            //Parámetro
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>HORA / PARÁMETRO</th>");
            foreach (var punto in listaPto)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px'>{0}</th>", tipoinf);
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            //00:30 - 00:00
            DateTime horas = fechaIni.Date;
            decimal? valor;
            int? tipo;
            for (int h = 1; h <= 48; h++)
            {
                horas = horas.AddMinutes(30);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                foreach (var punto in listaPto)
                {
                    MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                    valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                    tipo = m48 != null ? (int?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).GetValue(m48, null) : null;

                    string backColor = "background-color: " + CargaDatosUtil.ColorCeldaMeMedicion(tipo ?? 0);

                    if (m48 != null)
                    {
                        if (valor != null) { strHtml.AppendFormat("<td style='{1}'>{0}</td>", valor.Value.ToString("N", nfi), backColor); }
                        else { strHtml.AppendFormat("<td style='{0}'></td>", backColor); }
                    }
                    else
                    {
                        strHtml.AppendFormat("<td></td>");
                    }
                }

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<tfoot>");
            //Cálculo - Máximo
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>MÁXIMO</td>");

            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Maximo;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Cálculo - Mínimo
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>MÍNIMO</td>");
            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Minimo;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Cálculo - Promedio
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>PROMEDIO</td>");
            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Promedio;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Area operativa
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>ÁREA OPERATIVA</td>");
            foreach (var reg in listaPto)
            {
                strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", reg.AreaOperativa);
            }
            strHtml.Append("</tr>");

            //Empresa
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>EMPRESA</td>");
            listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPto);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPto[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }

                strHtml.AppendFormat("<td colspan={1} class='tdbody_reporte'>{0}</td>", regPto.Emprnomb, numElemSig);
            }
            strHtml.Append("</tr>");

            #endregion

            strHtml.Append("</tfoot>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        public static void GeneraRptFlujoPotenciaActivaTransmisionSEIN(ExcelWorksheet ws, string nombreSheet, int rowIni, int colIni, bool flagVisiblePtomedicodi, DateTime fecha1, DateTime fecha2
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaPto.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                // Fila Hora - Empresa - Total

                int colIniPto = col;
                int colFinPto = colIniPto + 1;
                int rowIniPto = row;
                int rowFinPto = rowIniPto;
                ws.Cells[rowIniPto, colIniPto].Value = "CÓDIGO";
                ws.Cells[rowIniPto, colIniPto, rowFinPto, colFinPto].Merge = true;

                int rowIniScada = rowFinPto + 1;
                int rowFinScada = rowIniScada;
                ws.Cells[rowIniScada, colIniPto].Value = "CANAL SCADA";
                ws.Cells[rowIniScada, colIniPto, rowFinScada, colFinPto].Merge = true;

                int colIniSSEE = colIniPto;
                int colFinSSEE = colIniSSEE + 1;
                int rowIniSSEE = rowFinScada + 1;
                int rowFinSSEE = rowIniSSEE;
                ws.Cells[rowIniSSEE, colIniSSEE].Value = "SS.EE.";
                ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;

                int colIniFecha = colIniPto;
                int rowIniFecha = rowFinSSEE + 1;
                int rowFinFecha = rowIniFecha;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";

                int rowIniEquipo = rowFinSSEE + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo = colIniEquipo;
                colIniSSEE = colFinSSEE + 1;

                int colIniNombreReporte = colIniEquipo;

                int totalPto = listaPto.Count();
                //Punto
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    ws.Cells[rowIniPto, colIniEquipo + i].Value = punto.Ptomedicodi;
                    ws.Cells[rowIniScada, colIniEquipo + i].Value = punto.Canales;

                    ws.Cells[rowIniEquipo, colIniEquipo + i].Value = punto.Repptonomb;
                    colFinEquipo++;
                }

                //SSEE
                List<MePtomedicionDTO> listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Subestacion != regPto.Subestacion);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinSSEE = colIniSSEE + numElemSig - 1;
                    ws.Cells[rowIniSSEE, colIniSSEE].Value = regPto.Subestacion.Trim();
                    ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;
                    colIniSSEE = colFinSSEE + 1;
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 2;
                int colFinNombreReporte = colFinEquipo - 1;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreSheet;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniFecha, rowIniFecha, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniEquipo, rowIniFecha, colFinNombreReporte, "Arial", 9);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte, "Izquierda");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];

                    string backColor = (!string.IsNullOrEmpty(punto.Colorcelda)) ? punto.Colorcelda : "#538DD5";
                    UtilExcel.CeldasExcelColorFondo(ws, rowIniEquipo, colIniEquipo + i, rowIniEquipo, colIniEquipo + i, backColor);
                }

                #endregion

                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo

                var colorCeldaDatos = ColorTranslator.FromHtml("#1F497D");
                decimal? valor;
                int? tipo;
                int numDia = 0;

                int colData = colIniHora;
                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        ws.Row(row).Height = 18;

                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                        ws.Cells[row, colIniFecha].Style.Font.Size = 12;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniFecha].Style.Font.Color.SetColor(Color.White);

                        colData = colIniHora;
                        //Hora
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        ws.Cells[row, colIniHora].Style.Font.Bold = true;
                        ws.Cells[row, colIniHora].Style.Font.Size = 12;
                        ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniHora].Style.Font.Color.SetColor(Color.White);

                        colData++;
                        foreach (var pto in listaPto)
                        {
                            MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);

                            valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                            tipo = m48 != null ? (int?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).GetValue(m48, null) : null;
                            string backColor = CargaDatosUtil.ColorCeldaMeMedicion(tipo ?? 0);

                            ws.Cells[row, colData].Value = valor;
                            ws.Cells[row, colData].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                            UtilExcel.CeldasExcelColorFondo(ws, row, colData, row, colData, backColor);
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                int rowFinData = rowIniData + numDia * 48 - 1;
                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowFinData, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                //mostrar lineas horas
                for (int c = colIniHora + 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        //ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(colorCeldaDatos);

                        //ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(colorCeldaDatos);

                        //ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(colorCeldaDatos);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(colorCeldaDatos);
                    }
                }

                #endregion

                #region footer

                int colIniMax = colIniPto;
                int rowIniMax = rowFinData + 1;
                ws.Cells[rowIniMax, colIniMax].Value = "MÁXIMO";
                ws.Cells[rowIniMax, colIniPto, rowIniMax, colFinPto].Merge = true;

                int colIniMin = colIniPto;
                int rowIniMin = rowIniMax + 1;
                ws.Cells[rowIniMin, colIniMin].Value = "MÍNIMO";
                ws.Cells[rowIniMin, colIniPto, rowIniMin, colFinPto].Merge = true;

                int colIniProm = colIniPto;
                int rowIniProm = rowIniMin + 1;
                ws.Cells[rowIniProm, colIniProm].Value = "PROMEDIO";
                ws.Cells[rowIniProm, colIniProm, rowIniProm, colFinPto].Merge = true;

                int colIniArea = colIniPto;
                int rowIniArea = rowIniProm + 1;
                ws.Cells[rowIniArea, colIniArea].Value = "ÁREA OPERATIVA";
                ws.Cells[rowIniArea, colIniArea, rowIniArea, colFinPto].Merge = true;

                int colIniEmpr = colIniPto;
                int rowIniEmpr = rowIniArea + 1;
                ws.Cells[rowIniEmpr, colIniEmpr].Value = "EMPRESA";
                ws.Cells[rowIniEmpr, colIniPto, rowIniEmpr, colFinPto].Merge = true;

                int rowIniEmp = rowIniEmpr;
                int colIniEmp = colIniHora + 1;
                int colFinEmp = colIniEmp;

                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                    //Cálculo - Máximo
                    valor = m48?.Maximo;
                    ws.Cells[rowIniMax, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Mínimo
                    valor = m48?.Minimo;
                    ws.Cells[rowIniMin, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Promedio
                    valor = m48?.Promedio;
                    ws.Cells[rowIniProm, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Area operativa
                    ws.Cells[rowIniArea, colIniEquipo + i].Value = punto.AreaOperativa;
                }

                //Empresa
                listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinEmp = colIniEmp + numElemSig - 1;
                    ws.Cells[rowIniEmp, colIniEmp].Value = regPto.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    colIniEmp = colFinEmp + 1;
                }

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniFecha, rowIniEmp, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniEquipo, rowIniEmp, colFinNombreReporte, "Arial", 9);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniMax, colIniMax, rowIniEmp, colIniHora, ColorTranslator.FromHtml("#538DD5"), Color.White);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniArea, colIniNombreReporte, rowIniEmp, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                #endregion

                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 20;

                ws.Column(colIniFecha).Width = 15;
                ws.Column(colIniHora).Width = 10;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniPto).Height = 20;
                ws.Row(rowIniEmp).Height = 27;
                ws.Row(rowIniSSEE).Height = 27;
                ws.Row(rowIniEquipo).Height = 27;

                if (!flagVisiblePtomedicodi)
                {
                    ws.Row(rowIniPto).Hidden = true;
                    ws.Row(rowIniScada).Hidden = true;
                }
                ws.View.FreezePanes(rowIniFecha + 1, colIniHora + 1);
                ws.View.ZoomScale = 70;

                #endregion

                CargaDatosUtil.AgregarSeccionNota(ConstantesPR5ReportesServicio.IdReporteFlujo, ws, rowIniEmpr + 2, 1);
            }
        }

        #endregion

        #region REPORTE_INTERCONEXIONES

        /// <summary>
        /// GeneraRptInterconexionesSEIN
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="listaData"></param>
        /// <param name="listaPto"></param>
        public static void GeneraRptInterconexionesSEIN(ExcelWorksheet ws, string nombreSheet, int rowIni, int colIni, bool flagVisiblePtomedicodi, DateTime fecha1, DateTime fecha2,
                List<MeMedicion48DTO> listaData, List<MePtomedicionDTO> listaPto)
        {
            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaPto.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                // Fila Hora - Empresa - Total

                int colIniPto = col;
                int colFinPto = colIniPto + 1;
                int rowIniPto = row;
                int rowFinPto = rowIniPto;
                ws.Cells[rowIniPto, colIniPto].Value = "CÓDIGO";
                ws.Cells[rowIniPto, colIniPto, rowFinPto, colFinPto].Merge = true;

                int colIniSSEE = colIniPto;
                int colFinSSEE = colIniSSEE + 1;
                int rowIniSSEE = rowFinPto + 1;
                int rowFinSSEE = rowIniSSEE;
                ws.Cells[rowIniSSEE, colIniSSEE].Value = "SS.EE.";
                ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;

                int colIniFecha = colIniPto;
                int rowIniFecha = rowFinSSEE + 1;
                int rowFinFecha = rowIniFecha;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";

                int rowIniEquipo = rowFinSSEE + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo = colIniEquipo;
                colIniSSEE = colFinSSEE + 1;

                int colIniNombreReporte = colIniEquipo;

                int totalPto = listaPto.Count();
                //Punto
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    if (punto.Ptomedicodi > 0) ws.Cells[rowIniPto, colIniEquipo + i].Value = punto.Ptomedicodi;

                    ws.Cells[rowIniEquipo, colIniEquipo + i].Value = (punto.Subestacion != null) ? punto.Subestacion.Replace("SE ", "") + " \n " + punto.Equinomb : punto.Repptonomb;
                    colFinEquipo++;
                }

                //SSEE
                List<MePtomedicionDTO> listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Ptomedidesc != regPto.Ptomedidesc);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinSSEE = colIniSSEE + numElemSig - 1;
                    ws.Cells[rowIniSSEE, colIniSSEE].Value = regPto.Ptomedidesc.Trim();
                    ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;
                    colIniSSEE = colFinSSEE + 1;
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 2;
                int colFinNombreReporte = colFinEquipo - 1;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreSheet;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniFecha, rowIniFecha, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniEquipo, rowIniFecha, colFinNombreReporte, "Arial", 9);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte, "Izquierda");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                #endregion

                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo

                var colorCeldaDatos = ColorTranslator.FromHtml("#1F497D");
                decimal? valor;
                int numDia = 0;

                int colData = colIniHora;
                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        ws.Row(row).Height = 18;

                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                        ws.Cells[row, colIniFecha].Style.Font.Size = 12;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniFecha].Style.Font.Color.SetColor(Color.White);

                        colData = colIniHora;
                        //Hora
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        ws.Cells[row, colIniHora].Style.Font.Bold = true;
                        ws.Cells[row, colIniHora].Style.Font.Size = 12;
                        ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniHora].Style.Font.Color.SetColor(Color.White);

                        colData++;
                        foreach (var pto in listaPto)
                        {
                            MeMedicion48DTO regpotActiva = listaData.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);
                            valor = regpotActiva != null ? (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null) : null;
                            ws.Cells[row, colData].Value = valor;
                            ws.Cells[row, colData].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(Color.White);
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                int rowFinData = rowIniData + numDia * 48 - 1;
                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowFinData, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                //mostrar lineas horas
                for (int c = colIniHora + 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(colorCeldaDatos);

                        ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(colorCeldaDatos);

                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(colorCeldaDatos);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(colorCeldaDatos);
                    }
                }

                #endregion

                #region footer

                int colIniMax = colIniPto;
                int rowIniMax = rowFinData + 1;
                ws.Cells[rowIniMax, colIniMax].Value = "MÁXIMO";
                ws.Cells[rowIniMax, colIniPto, rowIniMax, colFinPto].Merge = true;

                int colIniMin = colIniPto;
                int rowIniMin = rowIniMax + 1;
                ws.Cells[rowIniMin, colIniMin].Value = "MÍNIMO";
                ws.Cells[rowIniMin, colIniPto, rowIniMin, colFinPto].Merge = true;

                int colIniProm = colIniPto;
                int rowIniProm = rowIniMin + 1;
                ws.Cells[rowIniProm, colIniProm].Value = "PROMEDIO";
                ws.Cells[rowIniProm, colIniProm, rowIniProm, colFinPto].Merge = true;

                int colIniEmpr = colIniPto;
                int rowIniEmpr = rowIniProm + 1;
                ws.Cells[rowIniEmpr, colIniEmpr].Value = "EMPRESA";
                ws.Cells[rowIniEmpr, colIniPto, rowIniEmpr, colFinPto].Merge = true;

                int rowIniEmp = rowIniEmpr;
                int colIniEmp = colIniHora + 1;
                int colFinEmp = colIniEmp;

                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    MeMedicion48DTO m48 = listaData.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                    //Cálculo - Máximo
                    valor = m48?.Maximo;
                    ws.Cells[rowIniMax, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Mínimo
                    valor = m48?.Minimo;
                    ws.Cells[rowIniMin, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Promedio
                    valor = m48?.Promedio;
                    ws.Cells[rowIniProm, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";
                }

                //Empresa
                listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinEmp = colIniEmp + numElemSig - 1;
                    ws.Cells[rowIniEmp, colIniEmp].Value = regPto.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    colIniEmp = colFinEmp + 1;
                }

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniFecha, rowIniEmp, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniEquipo, rowIniEmp, colFinNombreReporte, "Arial", 9);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniMax, colIniMax, rowIniEmp, colIniHora, ColorTranslator.FromHtml("#538DD5"), Color.White);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniEmp, colIniNombreReporte, rowIniEmp, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                #endregion

                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 20;

                ws.Column(colIniFecha).Width = 15;
                ws.Column(colIniHora).Width = 10;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniPto).Height = 20;
                ws.Row(rowIniEmp).Height = 27;
                ws.Row(rowIniSSEE).Height = 27;
                ws.Row(rowIniEquipo).Height = 27;

                if (!flagVisiblePtomedicodi) ws.Row(rowIniPto).Hidden = true;

                ws.View.FreezePanes(rowIniFecha + 1, colIniHora + 1);
                ws.View.ZoomScale = 70;

                #endregion
            }
        }

        /// <summary>
        /// AgregarGraficoRptInterconexionesSEIN
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreGrafico"></param>
        /// <param name="tituloGrafico"></param>
        /// <param name="colIniGrafico"></param>
        /// <param name="filaIniHora"></param>
        /// <param name="filFinHora"></param>
        /// <param name="colIniHora"></param>
        /// <param name="filaNombrePto"></param>
        /// <param name="colIniData"></param>
        /// <param name="colFinData"></param>
        public static void AgregarGraficoRptInterconexionesSEIN(ExcelWorksheet ws, string nombreGrafico, string tituloGrafico, int colIniGrafico,
                        int filaIniHora, int filFinHora, int colIniHora, int filaNombrePto, int colIniData, int colFinData)
        {
            var areaChart = ws.Drawings.AddChart(nombreGrafico, eChartType.ColumnStacked);
            areaChart.SetPosition(filFinHora + 8, 0, colIniGrafico, 0);
            areaChart.SetSize(1100, 520);

            for (int col = colIniData; col <= colFinData; col++)
            {
                var ran1 = ws.Cells[filaIniHora, col, filFinHora, col];
                var ran2 = ws.Cells[filaIniHora, colIniHora, filFinHora, colIniHora]; //horas

                var serie = (ExcelChartSerie)areaChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[filaNombrePto, col].Value.ToString();
            }

            areaChart.Title.Text = tituloGrafico;
            areaChart.Title.Font.Bold = true;
            areaChart.YAxis.Title.Text = "";
            areaChart.Legend.Position = eLegendPosition.Bottom;
        }

        /// <summary>
        /// GetGraficoInterconexionWord
        /// </summary>
        /// <param name="tituloGrafico"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoInterconexionWord(string tituloGrafico, List<MePtomedicionDTO> listaPto, List<MeMedicion48DTO> listaData)
        {
            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaPto.Count][];

            DateTime horas = DateTime.Today;
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            decimal? valor;
            RegistroSerie regSerie;
            List<DatosSerie> listadata;
            List<decimal> listaValor = new List<decimal>();
            for (int i = 0; i < listaPto.Count; i++)
            {
                var obj48XPto = listaData.FirstOrDefault(x => x.Ptomedicodi == listaPto[i].Ptomedicodi) ?? new MeMedicion48DTO();

                regSerie = new RegistroSerie();
                regSerie.Name = listaPto[i].Repptonomb;
                regSerie.Type = "column";

                listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    valor = (decimal?)obj48XPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(obj48XPto, null);
                    listadata.Add(new DatosSerie() { Y = valor });
                    if (valor != null) listaValor.Add(valor.Value);
                }
                regSerie.Data = listadata;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = tituloGrafico;
            grafico.YaxixTitle = "MW";
            grafico.XAxisCategories = new List<string>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesYAxis = new List<int>();

            if (listaValor.Count() > 0)
            {
                grafico.YaxixMin = listaValor.Min(x => x);
                grafico.YaxixMax = listaValor.Max(x => x);
            }

            return grafico;
        }

        #endregion

        // 3.13.2.23.	Registro cada 30 minutos de la tensión de las Barras del SEIN definidas por el COES.
        #region REPORTE_TENSION_BARRAS_SEIN

        /// <summary>
        /// genera vista html de reporte tensión de barras
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        /// <returns></returns>
        public static string ReporteTensionBarrasSeinHtml(DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            int totalPto = listaPto.Count();

            int padding = 20;
            int anchoTotal = (100 + padding) + listaPto.Count * (150 + padding);

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");

            #region Cabecera

            int n = 0;

            //Punto
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>CÓDIGO</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", punto.Ptomedicodi, back_color);
                n++;
            }
            strHtml.Append("</tr>");

            //Scada
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>CANAL SCADA</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", punto.Canales, back_color);
                n++;
            }
            strHtml.Append("</tr>");

            //Empresa
            List<MePtomedicionDTO> listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPto);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPto[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }

            }

            //Subestacion
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px'>SS.EE</th>");
            listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPto);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPto[i];
                string back_color = (!string.IsNullOrEmpty(regPto.Colorcelda)) ? "background-color: " + regPto.Colorcelda : "";
                int nSigSSEE = listaSig.FindIndex(x => x.Subestacion != regPto.Subestacion);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }

                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {2}' colspan={1}>{0}</th>", regPto.Subestacion, numElemSig, back_color);
            }
            strHtml.Append("</tr>");

            //Tension
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>TENSIÓN</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", punto.NivelTension, back_color);
            }
            strHtml.Append("</tr>");

            //Equipo
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>EQUIPO</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", punto.Repptonomb, back_color);
            }
            strHtml.Append("</tr>");

            //Parámetro
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 100px;'>HORA / PARÁMETRO</th>");
            foreach (var punto in listaPto)
            {
                string back_color = (!string.IsNullOrEmpty(punto.Colorcelda)) ? "background-color: " + punto.Colorcelda : "";
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: 100px; {1}'>{0}</th>", "kV", back_color);
            }
            strHtml.Append("</tr>");

            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo
            decimal? valor = null;
            int? tipo;
            // Día - Hora
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                    foreach (var punto in listaPto)
                    {
                        MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                        valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                        tipo = m48 != null ? (int?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).GetValue(m48, null) : null;

                        string backColor = "background-color: " + CargaDatosUtil.ColorCeldaMeMedicion(tipo ?? 0);

                        if (m48 != null)
                        {
                            if (valor != null) { strHtml.AppendFormat("<td style='{1}'>{0}</td>", valor.Value.ToString("N", nfi), backColor); }
                            else { strHtml.AppendFormat("<td style='{0}'></td>", backColor); }
                        }
                        else
                        {
                            strHtml.AppendFormat("<td></td>");
                        }
                    }

                    strHtml.Append("</tr>");

                    horas = horas.AddMinutes(30);
                }
            }

            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("<tfoot>");
            //Cálculo - Máximo
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>MÁXIMO</td>");

            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Maximo;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Cálculo - Mínimo
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>MÍNIMO</td>");
            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Minimo;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Cálculo - Promedio
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>PROMEDIO</td>");
            foreach (var punto in listaPto)
            {
                MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                valor = m48?.Promedio;
                if (valor != null) { strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", valor.Value.ToString("N", nfi)); }
                else { strHtml.AppendFormat("<td class='tdbody_reporte'></td>"); }
            }
            strHtml.Append("</tr>");

            //Area operativa
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>ÁREA OPERATIVA</td>");
            foreach (var reg in listaPto)
            {
                strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", reg.AreaOperativa);
            }
            strHtml.Append("</tr>");

            //Empresa
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte'>EMPRESA</td>");
            listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPto);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPto[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }

                strHtml.AppendFormat("<td colspan={1} class='tdbody_reporte'>{0}</td>", regPto.Emprnomb, numElemSig);
            }
            strHtml.Append("</tr>");



            strHtml.Append("</tfoot>");

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de fuente de Energia Primaria
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreSheet"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        public static void GeneraRptTensionBarrasSein(ExcelWorksheet ws, string nombreSheet, int rowIni, int colIni, bool flagVisiblePtomedicodi, DateTime fecha1, DateTime fecha2
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaPto.Count > 0)
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                // Fila Hora - Empresa - Total

                int colIniPto = col;
                int colFinPto = colIniPto + 1;
                int rowIniPto = row;
                int rowFinPto = rowIniPto;
                ws.Cells[rowIniPto, colIniPto].Value = "CÓDIGO";
                ws.Cells[rowIniPto, colIniPto, rowFinPto, colFinPto].Merge = true;

                int rowIniScada = rowFinPto + 1;
                int rowFinScada = rowIniScada;
                ws.Cells[rowIniScada, colIniPto].Value = "CANAL SCADA";
                ws.Cells[rowIniScada, colIniPto, rowFinScada, colFinPto].Merge = true;

                int colIniSSEE = colIniPto;
                int colFinSSEE = colIniSSEE + 1;
                int rowIniSSEE = rowFinScada + 1;
                int rowFinSSEE = rowIniSSEE;
                ws.Cells[rowIniSSEE, colIniSSEE].Value = "SS.EE.";
                ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;

                int colIniTension = colIniPto;
                int colFinTension = colIniTension + 1;
                int rowIniTension = rowIniSSEE + 1;
                int rowFinTension = rowIniTension;
                ws.Cells[rowIniTension, colIniTension].Value = "TENSIÓN";
                ws.Cells[rowIniTension, colIniTension, rowFinTension, colFinTension].Merge = true;

                int colIniFecha = colIniPto;
                int rowIniFecha = rowFinTension + 1;
                int rowFinFecha = rowIniFecha;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";

                int rowIniEmp = rowIniPto + 1;
                int colIniEmp = colIniHora + 1;
                int colFinEmp = colIniEmp;

                int rowIniEquipo = rowFinTension + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo = colIniEmp;
                colIniSSEE = colFinSSEE + 1;

                int colIniNombreReporte = colIniEmp;

                int totalPto = listaPto.Count();

                //Punto
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    ws.Cells[rowIniPto, colIniEquipo + i].Value = punto.Ptomedicodi;
                    ws.Cells[rowIniScada, colIniEquipo + i].Value = punto.Canales;

                    ws.Cells[rowIniEquipo, colIniEquipo + i].Value = punto.Repptonomb;

                    ws.Cells[rowIniTension, colIniEquipo + i].Value = punto.NivelTension.ToString();
                }

                //Empresa
                List<MePtomedicionDTO> listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinEmp = colIniEmp + numElemSig - 1;
                    colIniEmp = colFinEmp + 1;
                }

                //SSEE
                listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Subestacion != regPto.Subestacion);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinSSEE = colIniSSEE + numElemSig - 1;
                    ws.Cells[rowIniSSEE, colIniSSEE].Value = regPto.Subestacion.Trim();
                    ws.Cells[rowIniSSEE, colIniSSEE, rowFinSSEE, colFinSSEE].Merge = true;
                    colIniSSEE = colFinSSEE + 1;
                }

                //Nombre Reporte
                colIniNombreReporte = colIniNombreReporte - 2;
                int colFinNombreReporte = colFinEmp;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreSheet;
                ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniNombreReporte, colIniNombreReporte, rowIniFecha, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniFecha, rowIniFecha, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniEquipo, rowIniFecha, colFinNombreReporte, "Arial", 9);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte, "Izquierda");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniNombreReporte, colIniFecha, rowIniFecha, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];

                    string backColor = (!string.IsNullOrEmpty(punto.Colorcelda)) ? punto.Colorcelda : "#538DD5";
                    UtilExcel.CeldasExcelColorFondo(ws, rowIniPto, colIniEquipo + i, rowIniFecha, colIniEquipo + i, backColor);
                }

                #endregion

                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo

                var colorCeldaDatos = ColorTranslator.FromHtml("#1F497D");
                decimal? valor;
                int? tipo;
                int numDia = 0;

                int colData = colIniHora;
                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    for (int h = 1; h <= 48; h++)
                    {
                        ws.Row(row).Height = 18;

                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                        ws.Cells[row, colIniFecha].Style.Font.Size = 12;
                        ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniFecha].Style.Font.Color.SetColor(Color.White);

                        colData = colIniHora;
                        //Hora
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        ws.Cells[row, colIniHora].Style.Font.Bold = true;
                        ws.Cells[row, colIniHora].Style.Font.Size = 12;
                        ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4F81BD"));
                        ws.Cells[row, colIniHora].Style.Font.Color.SetColor(Color.White);

                        colData++;
                        foreach (var pto in listaPto)
                        {
                            MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == day);

                            valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                            tipo = m48 != null ? (int?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).GetValue(m48, null) : null;
                            string backColor = CargaDatosUtil.ColorCeldaMeMedicion(tipo ?? 0);

                            ws.Cells[row, colData].Value = valor;
                            ws.Cells[row, colData].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                            UtilExcel.CeldasExcelColorFondo(ws, row, colData, row, colData, backColor);
                            colData++;
                        }

                        horas = horas.AddMinutes(30);
                        row++;
                    }
                }

                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 48 - 1, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10;
                    range.Style.Numberformat.Format = "#,##0.00";
                }

                //mostrar lineas horas
                for (int c = colIniHora + 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                    {
                        //ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(colorCeldaDatos);

                        //ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(colorCeldaDatos);

                        //ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(colorCeldaDatos);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(colorCeldaDatos);
                    }
                }

                int rowFinData = rowIniData + numDia * 48 - 1;
                int colIniMax = colIniPto;
                int rowIniMax = rowFinData + 1;
                ws.Cells[rowIniMax, colIniMax].Value = "MÁXIMO";
                ws.Cells[rowIniMax, colIniPto, rowIniMax, colFinPto].Merge = true;

                int colIniMin = colIniPto;
                int rowIniMin = rowIniMax + 1;
                ws.Cells[rowIniMin, colIniMin].Value = "MÍNIMO";
                ws.Cells[rowIniMin, colIniPto, rowIniMin, colFinPto].Merge = true;

                int colIniProm = colIniPto;
                int rowIniProm = rowIniMin + 1;
                ws.Cells[rowIniProm, colIniProm].Value = "PROMEDIO";
                ws.Cells[rowIniProm, colIniProm, rowIniProm, colFinPto].Merge = true;

                int colIniArea = colIniPto;
                int rowIniArea = rowIniProm + 1;
                ws.Cells[rowIniArea, colIniArea].Value = "ÁREA OPERATIVA";
                ws.Cells[rowIniArea, colIniArea, rowIniArea, colFinPto].Merge = true;

                int colIniEmpr = colIniPto;
                int rowIniEmpr = rowIniArea + 1;
                ws.Cells[rowIniEmpr, colIniEmpr].Value = "EMPRESA";
                ws.Cells[rowIniEmpr, colIniPto, rowIniEmpr, colFinPto].Merge = true;

                rowIniEmp = rowIniEmpr;
                colIniEmp = colIniHora + 1;
                colFinEmp = colIniEmp;

                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO punto = listaPto[i];
                    MeMedicion48DTO m48 = data.Find(x => x.Ptomedicodi == punto.Ptomedicodi);

                    //Cálculo - Máximo
                    valor = m48?.Maximo;
                    ws.Cells[rowIniMax, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMax, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Mínimo
                    valor = m48?.Minimo;
                    ws.Cells[rowIniMin, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniMin, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Cálculo - Promedio
                    valor = m48?.Promedio;
                    ws.Cells[rowIniProm, colIniEquipo + i].Value = valor;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Border.BorderAround(ExcelBorderStyle.Dotted, colorCeldaDatos);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[rowIniProm, colIniEquipo + i].Style.Numberformat.Format = "#,##0.00";

                    //Area operativa
                    ws.Cells[rowIniArea, colIniEquipo + i].Value = punto.AreaOperativa;
                }

                //Empresa
                listaSig = new List<MePtomedicionDTO>();
                listaSig.AddRange(listaPto);
                for (int i = 0; i < totalPto; i++)
                {
                    MePtomedicionDTO regPto = listaPto[i];
                    int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                    int numElemSig = 1;

                    if (nSigSSEE > -1)
                    {
                        listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                        i += (nSigSSEE - 1);

                        numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                    }
                    else
                    {
                        //si no existe un elemento distinto terminar la iteracion
                        i = totalPto;
                        numElemSig = listaSig.Count();
                    }

                    colFinEmp = colIniEmp + numElemSig - 1;
                    ws.Cells[rowIniEmp, colIniEmp].Value = regPto.Emprnomb.Trim();
                    ws.Cells[rowIniEmp, colIniEmp, rowIniEmp, colFinEmp].Merge = true;
                    colIniEmp = colFinEmp + 1;
                }

                #region Formato Cabecera

                //Fecha - Hora
                UtilExcel.CeldasExcelWrapText(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorTexto(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "#538DD5");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniFecha, rowIniEmp, colIniHora, "Arial", 11);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniMax, colIniEquipo, rowIniEmp, colFinNombreReporte, "Arial", 9);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniMax, colIniNombreReporte, rowIniEmp, colFinNombreReporte);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniMax, colIniMax, rowIniEmp, colIniHora, ColorTranslator.FromHtml("#538DD5"), Color.White);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniArea, colIniNombreReporte, rowIniEmp, colFinNombreReporte, ColorTranslator.FromHtml("#538DD5"), Color.White);

                #endregion


                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 20;

                ws.Column(colIniFecha).Width = 15;
                ws.Column(colIniHora).Width = 10;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniPto).Height = 20;
                ws.Row(rowIniEmp).Height = 27;
                ws.Row(rowIniSSEE).Height = 27;
                ws.Row(rowIniEquipo).Height = 27;

                if (!flagVisiblePtomedicodi)
                {
                    ws.Row(rowIniPto).Hidden = true;
                    ws.Row(rowIniScada).Hidden = true;
                }
                ws.View.FreezePanes(10, 3);
                ws.View.ZoomScale = 70;

                #endregion

                CargaDatosUtil.AgregarSeccionNota(ConstantesPR5ReportesServicio.IdReporteTension, ws, rowIniEmpr + 2, 1);
            }
        }

        #endregion

        // 3.13.2.24.	Reporte de sobrecarga de equipos mayores a 100 kV. De presentarse sobrecarga en equipos menores a 100 kV hasta los 60 kV, que ocasione acciones correctivas en la Operación en Tiempo Real, se incluirá dicha sobrecarga en el reporte respectivo.
        #region REPORTE_SOBRECARGA_EQUIPO

        /// <summary>
        /// genera vista html de reporte sobrecarga de equipos
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteSobrecargaEquipoHtml(List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: 100%;' >");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;'>EMPRESA</th>");
            strHtml.Append("<th style='width:70px;'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:70px;'>T. EQUIPO</th>");
            strHtml.Append("<th style='width:70px;'>EQUIPO</th>");
            strHtml.Append("<th style='width:70px;'>INICIO</th>");
            strHtml.Append("<th style='width:70px;'>FINAL</th>");
            strHtml.Append("<th style='width:70px;'>OBSERVACIÓN</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var reg in data)
            {
                var datVersi = dataVersion.Find(x => x.Iccodi == reg.Iccodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", reg.Emprnomb);
                strHtml.AppendFormat("<td>{0}</td>", reg.Areanomb);
                strHtml.AppendFormat("<td>{0}</td>", reg.Famabrev);
                strHtml.AppendFormat("<td>{0}</td>", reg.Equiabrev);
                strHtml.AppendFormat("<td>{0}</td>", reg.HoraIni);
                strHtml.AppendFormat("<td>{0}</td>", reg.HoraFin);

                var _descrip = reg.Icdescrip1;
                string _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip1) { _descrip = datVersi.Icdescrip1; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", _descrip);
                //strHtml.AppendFormat("<td>{0}</td>", reg.Icdescrip1));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptSobreCargaEquipo
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptSobreCargaEquipo(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveIeodcuadroDTO> lista, List<EveIeodcuadroDTO> listaVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniTipoeq = colIniUbicacion + 1;
            int colIniEquipo = colIniTipoeq + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniObs = colIniFinal + 1;
            int colFinNombreReporte = colIniObs;

            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniTipoeq].Value = "T. EQUIPO";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniCabecera, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniCabecera, colIniObs].Value = "OBSERVACIÓN";

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Reporte de Sobrecarga de Equipos Mayores a 100 kV";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniTipoeq].Value = list.Famabrev;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.HoraIni;
                    ws.Cells[row, colIniFinal].Value = list.HoraFin;
                    ws.Cells[row, colIniObs].Value = list.Icdescrip1;

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniUbicacion, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniTipoeq, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniObs, rowFinData, colIniObs, "Izquierda");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniEmpresa).Width = 32;
            ws.Column(colIniUbicacion).Width = 30;
            ws.Column(colIniTipoeq).Width = 25;
            ws.Column(colIniEquipo).Width = 18;
            ws.Column(colIniInicio).Width = 23;
            ws.Column(colIniFinal).Width = 23;
            ws.Column(colIniObs).Width = 68;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.25.	Reporte de líneas desconectadas por Regulación de Tensión.
        #region REPORTE_LINEAS_DESCONECTAS_POR_TENSION

        /// <summary>
        /// genera vista html de reporte lineas desconectadad por tension
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteLineasDesconectadasPorTensionHtml(List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: 100%;' >");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:185px;'>EMPRESA</th>");
            strHtml.Append("<th style='width:150px;'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:70px;'>EQUIPO</th>");
            strHtml.Append("<th style='width:100px;'>INICIO</th>");
            strHtml.Append("<th style='width:100px;'>FINAL</th>");
            strHtml.Append("<th style='width:265px;'>MOTIVO</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var reg in data)
            {
                var datVersi = dataVersion.Find(x => x.Iccodi == reg.Iccodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='width:185px;text-align: left; padding-left: 5px;'>{0}</td>", reg.Emprnomb);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", reg.Areanomb);
                strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", reg.Equiabrev);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraIni);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraFin);
                strHtml.AppendFormat("<td style='width:265px;text-align: left; padding-left: 5px;'>{0}</td>", reg.Icdescrip1);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptLineasDesconectadasPorTension
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptLineasDesconectadasPorTension(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveIeodcuadroDTO> lista, List<EveIeodcuadroDTO> listaVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniObs = colIniFinal + 1;
            int colFinNombreReporte = colIniObs;

            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniCabecera, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniCabecera, colIniObs].Value = "MOTIVO";

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Reporte de líneas desconectadas por Regulación de Tensión ";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.HoraIni;
                    ws.Cells[row, colIniFinal].Value = list.HoraFin;
                    ws.Cells[row, colIniObs].Value = list.Icdescrip1;

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniUbicacion, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEquipo, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniObs, rowFinData, colIniObs, "Izquierda");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniEmpresa).Width = 44;
            ws.Column(colIniUbicacion).Width = 45;
            ws.Column(colIniEquipo).Width = 18;
            ws.Column(colIniInicio).Width = 23;
            ws.Column(colIniFinal).Width = 23;
            ws.Column(colIniObs).Width = 105;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.26.	Reporte de Sistemas Aislados Temporales
        #region REPORTE_SISTEMAS_AISLADOS_TEMPORALES

        /// <summary>
        /// ReporteSistemasAisladosTemporalesHtml
        /// </summary>
        /// <param name="flagIncluirGps"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteSistemasAisladosTemporalesHtml(bool flagIncluirGps, string url, List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: 100%;' >");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:150px;'>Ubicación</th>");
            strHtml.Append("<th style='width:150px;'>Empresa <br/>causante</th>");
            strHtml.Append("<th style='width:70px;'>Instalación <br/>causante</th>");
            strHtml.Append("<th style='width:100px;'>Inicio</th>");
            strHtml.Append("<th style='width:100px;'>Final</th>");
            strHtml.Append("<th style='width:150px;'>Operación de <br/>centrales</th>");
            strHtml.Append("<th style='width:300px;'>Motivo</th>");
            strHtml.Append("<th style='width:150px;'>Subsistema <br/>aislado</th>");
            if (flagIncluirGps)
            {
                strHtml.Append("<th style='width:70px;'>GPS Principal</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var reg in data)
            {
                var datVersi = dataVersion.Find(x => x.Iccodi == reg.Iccodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", reg.Areanomb);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", reg.Emprnomb);
                strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", reg.Equiabrev);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraIni);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraFin);

                var _descrip = reg.Icdescrip1;
                string _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip1) { _descrip = datVersi.Icdescrip1; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='width:150px; text-align: left; padding-left: 5px; background:" + _bground + "'>{0}</td>", _descrip);

                _descrip = reg.Icdescrip2;
                _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip2) { _descrip = datVersi.Icdescrip2; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='width:300px; text-align: left; padding-left: 5px; background:" + _bground + "'>{0}</td>", _descrip);

                _descrip = reg.Icdescrip3;
                _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip3) { _descrip = datVersi.Icdescrip3; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='width:150px; text-align: left; padding-left: 5px; background:" + _bground + "'>{0}</td>", _descrip);

                if (flagIncluirGps)
                {
                    int? gpscodiPpal = reg.Gpscodi;
                    string gpsnombre = reg.Gpsnombre;
                    if (gpscodiPpal > 0)
                    {
                        strHtml.Append("<td style='width:70px;text-align:center'>");
                        strHtml.Append(gpsnombre + " <br/>");
                        strHtml.Append("<a href='#' onclick='fnClickReporteVariacion(\"" + gpscodiPpal + "\")' title='Variaciones sostenidas y súbitas de frecuencia'><img src='" + url + "Content/Images/btn-open.png' style='padding-top:7px; padding-right: 15px;'></a>");
                        strHtml.Append("<a href='#' onclick='fnClickFrecuencia(\"" + gpscodiPpal + "\")' title='Frecuencia Instantánea'><img src='" + url + "Content/Images/ContextMenu/grafico.png' style='padding-top:7px'></a>");
                        strHtml.Append("</td>");
                    }
                    else
                    {
                        strHtml.Append("<td style='width:70px;text-align:center'></td>");
                    }
                }

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptSistemasAisladosTemporales
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptSistemasAisladosTemporales(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveIeodcuadroDTO> lista, List<EveIeodcuadroDTO> listaVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Reporte de Sistemas Aislados Temporales";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            int rowIniCabecera = rowIniNombreReporte + 1;

            GeneraRptSistemasAisladosTemporalesByLista(ws, rowIniNombreReporte, colIniNombreReporte, false, lista);

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// GeneraRptSistemasAisladosTemporalesByLista
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIniNombreReporte"></param>
        /// <param name="colIniNombreReporte"></param>
        /// <param name="flagIncluirGps"></param>
        /// <param name="lista"></param>
        public static void GeneraRptSistemasAisladosTemporalesByLista(ExcelWorksheet ws, int rowIniNombreReporte, int colIniNombreReporte, bool flagIncluirGps, List<EveIeodcuadroDTO> lista)
        {
            int rowIniCabecera = rowIniNombreReporte + 1;

            int colIniUbicacion = colIniNombreReporte;
            int colIniEmpresa = colIniUbicacion + 1;
            int colIniEquipo = colIniEmpresa + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniOpCentrales = colIniFinal + 1;
            int colIniMotivo = colIniOpCentrales + 1;
            int colIniSubAislado = colIniMotivo + 1;
            int colIniGps = colIniSubAislado;
            if (flagIncluirGps)
                colIniGps = colIniSubAislado + 1;
            int colFinNombreReporte = colIniGps;

            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA \nCAUSANTE";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "INSTALACIÓN CAUSANTE";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniCabecera, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniCabecera, colIniOpCentrales].Value = "OPERACIÓN DE \nCENTRALES";
            ws.Cells[rowIniCabecera, colIniMotivo].Value = "MOTIVO";
            ws.Cells[rowIniCabecera, colIniSubAislado].Value = "SUBSISTEMA \nAISLADO";
            if (flagIncluirGps)
                ws.Cells[rowIniCabecera, colIniGps].Value = "GPS";

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            int row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.HoraIni;
                    ws.Cells[row, colIniFinal].Value = list.HoraFin;
                    ws.Cells[row, colIniOpCentrales].Value = list.Icdescrip1;
                    ws.Cells[row, colIniMotivo].Value = list.Icdescrip2;
                    ws.Cells[row, colIniSubAislado].Value = list.Icdescrip3;
                    if (flagIncluirGps)
                        ws.Cells[row, colIniGps].Value = list.ListaNombreGps;

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUbicacion, rowFinData, colIniEmpresa, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEquipo, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniOpCentrales, rowFinData, colIniSubAislado, "Izquierda");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniUbicacion).Width = 21;
            ws.Column(colIniEmpresa).Width = 32;
            ws.Column(colIniEquipo).Width = 18;
            ws.Column(colIniInicio).Width = 18;
            ws.Column(colIniFinal).Width = 18;
            ws.Column(colIniOpCentrales).Width = 37;
            ws.Column(colIniMotivo).Width = 68;
            ws.Column(colIniSubAislado).Width = 34;
            if (flagIncluirGps)
                ws.Column(colIniGps).Width = 34;
        }

        #endregion

        #endregion

        #region INFORMACIÓN DEL PRODUCTO

        // 3.13.2.27.	Reporte de las variaciones sostenidas y súbitas de frecuencia en el SEIN.
        #region REPORTE_VARIACIONES_SOSTENIDAS_SUBITAS

        /// <summary>
        /// Reporte de Variaciones de Frecuencia SEIN - HTML
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="url"></param>
        /// <param name="listaGPS"></param>
        /// <returns></returns>
        public static string ReporteVariacionesFrecuenciaSEINHtml(List<FIndicadorDTO> listaData, string url, List<MeGpsDTO> listaGPS)
        {
            StringBuilder strHtml = new StringBuilder();

            if (listaGPS.Count() > 0)
            {
                foreach (var g in listaGPS)
                {
                    var gpsnomb = g.Nombre;
                    var data = listaData.Where(x => x.Gps == g.Gpscodi).ToList();
                    strHtml.Append("<table>");
                    strHtml.Append("<tr>");
                    strHtml.Append("<td>");
                    strHtml.Append("<table style='width: 800px;margin: 0 auto;'>");
                    strHtml.Append("<tr class='fila_grafico_distribucion'>");
                    strHtml.Append("<td style='width:30px;text-align:right'>");
                    strHtml.Append("<a href='#' onclick='fnClickFrecuencia(\"" + g.Gpscodi + "\")' title='Frecuencia Instantánea'><img src='" + url + "Content/Images/ContextMenu/grafico.png' style='padding-top:7px'></a>");
                    strHtml.Append("</td>");
                    strHtml.Append("</tr>");
                    strHtml.Append("</table>");
                    strHtml.Append("</td>");
                    strHtml.Append("</tr>");

                    strHtml.Append("<tr>");
                    strHtml.Append("<td>");
                    strHtml.Append("<table class='pretty tabla-icono' style='width: 800px;margin: 0 auto;'>");

                    strHtml.Append("<thead>");
                    #region cabecera
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th style='width:70px;' colspan='8'>{0}</th>", gpsnomb);
                    strHtml.Append("</tr>");

                    strHtml.Append("<tr>");
                    strHtml.Append("<th style='width:70px;' rowspan='2'>Indicador de Calidad</th>");
                    strHtml.Append("<th style='width:70px;' colspan='2'>FRECUENCIA MÍNIMA</th>");
                    strHtml.Append("<th style='width:70px;' colspan='2'>FRECUENCIA MÁXIMA</th>");
                    strHtml.Append("<th style='width:70px;' colspan='3'>TRANSGRESIONES</th>");
                    strHtml.Append("</tr>");

                    strHtml.Append("<tr>");
                    strHtml.Append("<th style='width:70px;' >HORA</th>");
                    strHtml.Append("<th style='width:70px;' >Hz</th>");
                    strHtml.Append("<th style='width:70px;' >HORA</th>");
                    strHtml.Append("<th style='width:70px;' >Hz</th>");
                    strHtml.Append("<th style='width:70px;' >HORA</th>");
                    strHtml.Append("<th style='width:70px;' >Hz</th>");
                    strHtml.Append("<th style='width:70px;' >ACUMULADAS MES</th>");
                    strHtml.Append("</tr>");
                    #endregion
                    strHtml.Append("</thead>");

                    strHtml.Append("<tbody>");
                    #region cuerpo
                    //***************************      CUERPO DE LA TABLA         ***********************************//

                    FIndicadorDTO varSostenida, varSubita;
                    //Variaciones sostenidad de frecuencia
                    varSostenida = data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSostenida);
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style='width:70px;'>Variaciones Sostenidas de Frecuencia</td>");
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSostenida.HoraFrecMin);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSostenida.ValorFrecMinDesc);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSostenida.HoraFrecMax);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSostenida.ValorFrecMaxDesc);
                    strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", (varSostenida.HoraTransgr ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:60px;'>{0}</td>", (varSostenida.IndicValorTransgr ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:120px;'>{0}</td>", varSostenida.AcumuladoTransgr);
                    strHtml.Append("</tr>");

                    //Variaciones súbitas de frecuencia
                    varSubita = data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSubita);
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style='width:70px;'>Variaciones Súbitas de Frecuencia</td>");
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSubita.HoraFrecMin);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSubita.ValorFrecMinDesc);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSubita.HoraFrecMax);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", varSubita.ValorFrecMaxDesc);
                    strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", (varSubita.HoraTransgr ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:60px;'>{0}</td>", (varSubita.IndicValorTransgr ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:120px;'>{0}</td>", varSubita.AcumuladoTransgr);
                    strHtml.Append("</tr>");

                    //Variaciones de frecuencia
                    var minima = data.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMinima);
                    var maxima = data.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMaxima);
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style='width:70px;'>Variaciones Frecuencia</td>");
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", (minima.HoraFrecMin ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", (minima.ValorFrecMinDesc ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", (maxima.HoraFrecMax ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", (maxima.ValorFrecMaxDesc ?? "").Replace("\n", "<br>"));
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", string.Empty);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", string.Empty);
                    strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", string.Empty);
                    strHtml.Append("</tr>");

                    #endregion
                    strHtml.Append("</tbody>");

                    strHtml.Append("</table>");
                    strHtml.Append("</td>");
                    strHtml.Append("</tr>");
                    strHtml.Append("</table>");

                    strHtml.Append("<br/><br/>");
                }
            }
            else
            {
                strHtml.Append("¡No existen datos para mostrar!");
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptVariacionesFrecuenciaSEIN
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaGPS"></param>
        public static void GeneraRptVariacionesFrecuenciaSEIN(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2,
                                        List<FIndicadorDTO> lista, List<MeGpsDTO> listaGPS)
        {
            int row = rowIni;

            #region Cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;
            int rowIniCabecera = rowIniNombreReporte;

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Reporte de GPS";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #endregion
            row = rowIniCabecera + 1;

            #region Cuerpo
            if (listaGPS.Count > 0)
            {
                GeneraRptVariacionesFrecuenciaSEINByObj(ws, row, colIniNombreReporte, fecha1, fecha2, lista, listaGPS);
            }

            #endregion

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// GeneraRptVariacionesFrecuenciaSEINByObj
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="colIniNombreReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaGPS"></param>
        public static void GeneraRptVariacionesFrecuenciaSEINByObj(ExcelWorksheet ws, int row, int colIniNombreReporte, DateTime fecha1, DateTime fecha2,
                                        List<FIndicadorDTO> lista, List<MeGpsDTO> listaGPS)
        {
            int colGps = colIniNombreReporte;
            int colFrecMinHora = colGps + 1;
            int colFrecMinHz = colFrecMinHora + 1;
            int colFrecMaxHora = colFrecMinHz + 1;
            int colFrecMaxHz = colFrecMaxHora + 1;
            int colTransgHora = colFrecMaxHz + 1;
            int colTransgHz = colTransgHora + 1;
            int colTransAcumMes = colTransgHz + 1;

            foreach (var g in listaGPS)
            {
                int filGps = row;
                int filIndCal1 = filGps + 1;
                int filIndCal2 = filIndCal1 + 1;
                int filVarSostenida = filIndCal2 + 1;
                int filVarSubita = filVarSostenida + 1;
                int filVarFrec = filVarSubita + 1;

                var gpsnomb = g.Nombre;
                var data = lista.Where(x => x.Gps == g.Gpscodi).ToList();

                ws.Cells[filGps, colGps].Value = gpsnomb;
                ws.Cells[filIndCal1, colGps].Value = "Indicador de Calidad";
                ws.Cells[filIndCal1, colFrecMinHora].Value = "Frecuencia mínima";
                ws.Cells[filIndCal2, colFrecMinHora].Value = "Hora";
                ws.Cells[filIndCal2, colFrecMinHz].Value = "Hz";
                ws.Cells[filIndCal1, colFrecMaxHora].Value = "Frecuencia máxima";
                ws.Cells[filIndCal2, colFrecMaxHora].Value = "Hora";
                ws.Cells[filIndCal2, colFrecMaxHz].Value = "Hz";
                ws.Cells[filIndCal1, colTransgHora].Value = "Transgresiones";
                ws.Cells[filIndCal2, colTransgHora].Value = "Hora";
                ws.Cells[filIndCal2, colTransgHz].Value = "Hz";
                ws.Cells[filIndCal2, colTransAcumMes].Value = "Acumuladas mes";

                UtilExcel.CeldasExcelAgrupar(ws, filGps, colGps, filGps, colTransAcumMes);
                UtilExcel.CeldasExcelAgrupar(ws, filIndCal1, colGps, filIndCal2, colGps);
                UtilExcel.CeldasExcelAgrupar(ws, filIndCal1, colFrecMinHora, filIndCal1, colFrecMinHz);
                UtilExcel.CeldasExcelAgrupar(ws, filIndCal1, colFrecMaxHora, filIndCal1, colFrecMaxHz);
                UtilExcel.CeldasExcelAgrupar(ws, filIndCal1, colTransgHora, filIndCal1, colTransAcumMes);

                UtilExcel.CeldasExcelColorTexto(ws, filGps, colGps, filIndCal2, colTransAcumMes, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filGps, colGps, filIndCal2, colTransAcumMes, ConstantesPR5ReportesServicio.ColorInfSGI);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filGps, colGps, filIndCal2, colTransAcumMes, "Arial", 11);

                UtilExcel.CeldasExcelWrapText(ws, filGps, colGps, filIndCal2, colTransAcumMes);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filGps, colGps, filIndCal2, colTransAcumMes, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filGps, colGps, filIndCal2, colTransAcumMes, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, filGps, colGps, filIndCal2, colTransAcumMes);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, filGps, colGps, filIndCal2, colTransAcumMes, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

                //datos
                for (int i = 1; i <= 3; i++)
                {
                    FIndicadorDTO varIndicador = i == 2 ? data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSubita) : data.Find(x => x.Indiccodi == ConstantesIndicador.VariacionSostenida);

                    FIndicadorDTO indicadorMin = new FIndicadorDTO();
                    FIndicadorDTO indicadorMax = new FIndicadorDTO();
                    if (i == 3)
                    {
                        indicadorMin = data.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMinima);
                        indicadorMax = data.Find(x => x.Indiccodi == ConstantesIndicador.FrecuenciaMaxima);
                    }


                    string texto_ = i == 2 ? "Variaciones Súbitas de Frecuencia" : i== 1? "Variaciones Sostenidas de Frecuencia" : "Variaciones Frecuencia";
                    int filVar = i == 1 ? filVarSostenida : i == 2 ? filVarSubita : filVarFrec;

                    ws.Cells[filVar, colGps].Value = texto_;
                    ws.Cells[filVar, colFrecMinHora].Value = i < 3? varIndicador.HoraFrecMin : indicadorMin.HoraFrecMin;
                    ws.Cells[filVar, colFrecMinHz].Value = i < 3 ? varIndicador.ValorFrecMinDesc : indicadorMin.ValorFrecMinDesc;
                    ws.Cells[filVar, colFrecMaxHora].Value = i < 3 ? varIndicador.HoraFrecMax : indicadorMax.HoraFrecMax;
                    ws.Cells[filVar, colFrecMaxHz].Value = i < 3 ? varIndicador.ValorFrecMaxDesc : indicadorMax.ValorFrecMaxDesc;
                    ws.Cells[filVar, colTransgHora].Value = i != 3? varIndicador.HoraTransgr: "";
                    ws.Cells[filVar, colTransgHz].Value = i != 3 ? varIndicador.IndicValorTransgr: "";
                    ws.Cells[filVar, colTransAcumMes].Value = i != 3 ? varIndicador.AcumuladoTransgr.ToString(): "";
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filVarSostenida, colGps, filVarFrec, colTransAcumMes, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filVarSostenida, colGps, filVarFrec, colTransAcumMes, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filVarSostenida, colGps, filVarFrec, colTransAcumMes, "Arial", 10);
                UtilExcel.CeldasExcelWrapText(ws, filVarSostenida, colGps, filVarFrec, colTransAcumMes);
                UtilExcel.BorderCeldas2(ws, filVarSostenida, colGps, filVarFrec, colTransAcumMes);

                ws.Row(filGps).Height = 30;
                ws.Row(filIndCal1).Height = 20;
                ws.Row(filIndCal2).Height = 20;

                row = filVarFrec + 3;
            }

            ws.Column(colGps).Width = 28;
            ws.Column(colFrecMinHora).Width = 23;
            ws.Column(colFrecMinHz).Width = 23;
            ws.Column(colFrecMaxHora).Width = 23;
            ws.Column(colFrecMaxHz).Width = 23;
            ws.Column(colTransgHora).Width = 23;
            ws.Column(colTransgHz).Width = 23;
            ws.Column(colTransAcumMes).Width = 23;
        }

        /// <summary>
        /// Reporte de Variaciones de Frecuencia SEIN - HTML
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public static string ReporteRangoFrecuenciaHtml(List<FLecturaDTO> listaReporte)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' style='width: auto;margin: 0 auto;'>");

            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='5'>Tiempo en que la frecuencia estuvo entre:</th>");
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;' colspan='2'>Rango de frecuencia</th>");
            strHtml.Append("<th style='width:70px;'>MIN</th>");
            strHtml.Append("<th style='width:70px;'>MED</th>");
            strHtml.Append("<th style='width:70px;'>MAX</th>");
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            foreach (var item in listaReporte)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", item.TextoRangoIni);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoRangoFin);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMin);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMed);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMax);
                strHtml.Append("</tr>");
            }

            #endregion

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// ReporteUmbralFrecuenciaHtml
        /// </summary>
        /// <param name="esDebajo"></param>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public static string ReporteUmbralFrecuenciaHtml(bool esDebajo, List<FLecturaDTO> listaReporte)
        {
            string titulo = esDebajo ? "Veces que la frecuencia disminuyó por debajo de:" : "Veces que la frecuencia aumentó por encima de:";

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' style='width: auto;margin: 0 auto;'>");

            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", titulo);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:70px;'>Umbral de frecuencia</th>");
            strHtml.Append("<th style='width:70px;'>MIN</th>");
            strHtml.Append("<th style='width:70px;'>MED</th>");
            strHtml.Append("<th style='width:70px;'>MAX</th>");
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            foreach (var item in listaReporte)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", item.TextoUmbral);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMin);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMed);
                strHtml.AppendFormat("<td>{0}</td>", item.TextoMax);
                strHtml.Append("</tr>");
            }

            #endregion

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// GetGraficoFrecuenciaWord
        /// </summary>
        /// <param name="objFrec"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoFrecuenciaWord(MeMedicion96DTO objFrec)
        {
            GraficoWeb grafico = new GraficoWeb
            {
                Type = "line",
                YaxixTitle = "HZ",
                XAxisTitle = "HORAS",
                XAxisCategories = ListarCuartoHora96(),
                SerieData = new DatosSerie[1],
                TitleText = "FRECUENCIA DEL SEIN"
            };

            grafico.SerieData[0] = new DatosSerie { Name = "HOY", Data = new decimal?[96] };

            decimal? valorH = null;
            for (int k = 1; k <= 96; k++)
            {
                valorH = (decimal?)objFrec.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(objFrec, null);

                grafico.SerieData[0].Data[k - 1] = valorH;
            }

            return grafico;
        }

        public static GraficoWeb GraficoDistribucionFrecuencia(bool esYAxisPorcentaje, List<FLecturaDTO> listaSerieFrecuencia)
        {
            int totalElemento = listaSerieFrecuencia.Count;

            //Grafico
            var grafico = new GraficoWeb();
            grafico.TitleText = "DISTRIBUCIÓN DE LA FRECUENCIA INSTANTÁNEA";
            //
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();

            //eje y 
            grafico.YAxixTitle = new List<string>();
            grafico.YaxixTitle = "";
            grafico.SerieDataS = new DatosSerie[4][];
            if (esYAxisPorcentaje)
            {
                grafico.YaxixMin = 0;
                grafico.YaxixMax = 70;
                grafico.SeriesYAxis = new List<int>() { 0, 10, 20, 30, 40, 50, 60, 70 };
            }
            else
            {
                grafico.YaxixMin = 0;
                grafico.YaxixMax = 13;

                List<decimal> listaMax = new List<decimal>();
                listaMax.AddRange(listaSerieFrecuencia.Select(x => x.Minima_Porc).ToList());
                listaMax.AddRange(listaSerieFrecuencia.Select(x => x.Media_Porc).ToList());
                listaMax.AddRange(listaSerieFrecuencia.Select(x => x.Maxima_Porc).ToList());
                if (listaMax.Any()) grafico.YaxixMax = (int)(listaMax.Max() + 2);
                grafico.SeriesYAxis = new List<int>() { };
                for(int i = 0;  i<= grafico.YaxixMax; i++)
                {
                    grafico.SeriesYAxis.Add(i);
                }
            }

            //Eje X
            grafico.XAxisTitle = "Hz";
            grafico.XAxisCategories = listaSerieFrecuencia.Select(x => x.TextoUmbral).ToList();

            //DATA SERIES
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesData = new decimal?[4][];

            //
            grafico.Series.Add(new RegistroSerie());
            grafico.Series[0].Name = "en MIN demanda";
            grafico.Series[0].Type = "spline";
            grafico.Series[0].Color = "#0000FF";
            grafico.SerieDataS[0] = new DatosSerie[totalElemento];
            for (int h = 0; h < totalElemento; h++)
            {
                grafico.SerieDataS[0][h] = new DatosSerie();
                grafico.SerieDataS[0][h].Y = listaSerieFrecuencia[h].Minima_Porc;
            }

            //
            grafico.Series.Add(new RegistroSerie());
            grafico.Series[1].Name = "en MED demanda";
            grafico.Series[1].Type = "spline";
            grafico.Series[1].Color = "#FF0086";
            grafico.SerieDataS[1] = new DatosSerie[totalElemento];
            for (int h = 0; h < totalElemento; h++)
            {
                grafico.SerieDataS[1][h] = new DatosSerie();
                grafico.SerieDataS[1][h].Y = listaSerieFrecuencia[h].Media_Porc;
            }

            //
            grafico.Series.Add(new RegistroSerie());
            grafico.Series[2].Name = "en MAX demanda";
            grafico.Series[2].Type = "spline";
            grafico.Series[2].Color = "#0F870F";
            grafico.SerieDataS[2] = new DatosSerie[totalElemento];
            for (int h = 0; h < totalElemento; h++)
            {
                grafico.SerieDataS[2][h] = new DatosSerie();
                grafico.SerieDataS[2][h].Y = listaSerieFrecuencia[h].Maxima_Porc;
            }

            //
            grafico.Series.Add(new RegistroSerie());
            grafico.Series[3].Name = "";
            grafico.Series[3].Type = "area";
            grafico.Series[3].Color = "#D2EDF7";
            grafico.SerieDataS[3] = new DatosSerie[totalElemento];
            for (int h = 0; h < totalElemento; h++)
            {
                grafico.SerieDataS[3][h] = new DatosSerie();
                if (h >= 4 && h <= 8)
                {
                    if (esYAxisPorcentaje)
                    {
                        grafico.SerieDataS[3][h].Y = 70.0m;
                    }
                }
            }
            return grafico;
        }

        public static void ListarDistribucionNormalFrec(int tipoDemanda, List<FLecturaDTO> listaDataFLectPeriodo, ref List<FLecturaDTO> listarSerieGraficoFrec)
        {
            //datos de entrada
            double promedio = (double)listaDataFLectPeriodo.Average(x => x.Frecuencia);
            double desviacionEstandar = MathHelper.GetDesviacionEstandar(listaDataFLectPeriodo.Select(x => (double)x.Frecuencia).ToList());

            //distribucion normal (función de densidad normal (acumulado = FALSO))
            foreach (var objFrec in listarSerieGraficoFrec)
            {
                //variables
                double x = (double)objFrec.Frecuencia;
                double div = Math.Pow(x - promedio, 2) / (2 * Math.Pow(desviacionEstandar, 2));
                double a = 1.0 / (desviacionEstandar * Math.Sqrt(2.0 * Math.PI));
                double b = Math.Pow(Math.E, -1.0 * div);

                //resultado
                decimal densidad = 0;
                try
                {
                    densidad = (decimal)(a * b);
                    densidad = Math.Round(densidad, 5);
                }
                catch (Exception)
                {
                    //error de conversión de double muy pequeño a decimal
                }

                switch (tipoDemanda)
                {
                    case 1: //maxima
                        objFrec.Maxima_Porc = densidad;
                        break;
                    case 2: //minima
                        objFrec.Minima_Porc = densidad;
                        break;
                    case 3: //media
                        objFrec.Media_Porc = densidad;
                        break;
                }
            }
        }

        public static List<FLecturaDTO> ListarSerieGraficoFrec(decimal min, decimal max)
        {
            decimal val1 = Math.Abs(60 - min);
            decimal val2 = Math.Abs(60 - max);
            decimal diffMax = Math.Round(Math.Max(val1, val2), 3);

            int frec = (int)(diffMax * 1000.0m);
            int ntramos = frec / 50 - 4; //cada tramo es 0.05
            ntramos = (ntramos / 2) * 2 + 1; //numero impar para mostrar 60.000
            if (ntramos < 5) ntramos = 5;
            if (ntramos > 11) ntramos = 11;

            List<FLecturaDTO> lista = new List<FLecturaDTO>();
            lista.Add(new FLecturaDTO() { Frecuencia = 60.0m, XValor = 0 });

            for (int i = 1; i < ntramos; i++)
            {
                lista.Add(new FLecturaDTO() { Frecuencia = 60.0m + i * 0.05m, XValor = 0 });
                lista.Add(new FLecturaDTO() { Frecuencia = 60.0m - i * 0.05m, XValor = 0 });
            }

            //formato
            foreach (var objFrec in lista)
            {
                objFrec.TextoUmbral = objFrec.Frecuencia.ToString("00.000");
                objFrec.Linf = objFrec.Frecuencia - 0.05m;
                objFrec.Lsup = objFrec.Frecuencia + 0.05m;
            }

            return lista.OrderBy(x => x.Frecuencia).ToList();
        }

        public static void GetFrecuenciaXRangoYFecha(bool esSostenida, List<FLecturaDTO> listaData, List<GenericoDTO> listaRangoHora, 
                            out string horaMin, out string horaMax, out string frecMin, out string frecMax)
        {
            horaMin = "---";
            horaMax = "---";
            frecMin = "---";
            frecMax = "---";
            List<DateTime> listaFecha = new List<DateTime>();
            foreach (var item in listaRangoHora)
            {
                if (item.ValorFecha1 != null) listaFecha.Add(item.ValorFecha1.Value);
                if (item.ValorFecha2 != null) listaFecha.Add(item.ValorFecha2.Value);
            }

            //cálculo
            if (listaFecha.Any() && listaData.Any())
            {
                DateTime fIni = listaFecha.Min();
                DateTime fFin = listaFecha.Max();
                var rangoFrec = listaData.Where(x => x.Fechahora >= fIni && x.Fechahora <= fFin && x.Frecuencia > 0).ToList();
                if (esSostenida)
                {
                    rangoFrec = rangoFrec.Where(x => x.Frecuencia > 60.36m || x.Frecuencia < 59.64m).ToList();
                }
                else
                {
                    rangoFrec = rangoFrec.Where(x => x.Frecuencia > 61.0m || x.Frecuencia < 59.0m).ToList();
                }

                if (rangoFrec.Any())
                {
                    var frecMinDecimal = rangoFrec.Min(x => x.Frecuencia);
                    var frecMaxDecimal = rangoFrec.Max(x => x.Frecuencia);
                    var frecMinDate = rangoFrec.FirstOrDefault(x => x.Frecuencia == frecMinDecimal) ?? new FLecturaDTO();
                    var frecMaxDate = rangoFrec.FindLast(x => x.Frecuencia == frecMaxDecimal) ?? new FLecturaDTO();

                    frecMin = (frecMinDecimal).ToString("00.000");
                    frecMax = (frecMaxDecimal).ToString("00.000");
                    horaMin = frecMinDate.Fechahora.ToString(ConstantesAppServicio.FormatoHHmmss);
                    horaMax = frecMaxDate.Fechahora.ToString(ConstantesAppServicio.FormatoHHmmss);
                }
            }
        }

        #endregion

        // 3.13.2.28.	Reporte de Sistemas Aislados Temporales y sus variaciones sostenidas y súbitas de frecuencia.
        #region REPORTE_SISTEMAS_AISLADOS_TEMPORALES_Y_VARIACIONES_SOSTENIDAS_SUBITAS

        /// <summary>
        /// ReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitasHtml
        /// </summary>
        /// <param name="listaGPS"></param>
        /// <returns></returns>
        public static string ReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitasHtml(List<InfSGIAisladosTempGPS> listaGPS)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");

            foreach (var regGPS in listaGPS)
            {
                strHtml.AppendFormat("<table id='reporte_{0}' class='pretty tabla-icono tbl_sis_ais' style='width: 100%; margin-bottom: 25px;' >", regGPS.Gpscodi);

                #region cabecera

                strHtml.Append("<thead>");

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<th style='width:150px;' colspan='6'>{0}</th>", regGPS.Gpsnombre);
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:150px;' rowspan='3'>INDICADOR DE CALIDAD</th>");
                strHtml.Append("<th style='width:150px;' rowspan='3'>PERIODO / HORA</th>");
                strHtml.Append("<th style='width:150px;' rowspan='3'>VALOR</th>");
                strHtml.Append("<th style='width:150px;' rowspan='3'>N° TRANSGRESIONES <BR/> ACUMULADAS - MES</th>");
                strHtml.Append("<th style='width:150px;' colspan='2'>TOLERANCIA NTCSE</th>");
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:150px;' colspan='2'>Hz</th>");
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<th style='width:150px;'>MAX.</th>");
                strHtml.Append("<th style='width:150px;'>MIN.</th>");
                strHtml.Append("</tr>");

                strHtml.Append("</thead>");

                #endregion

                #region cuerpo

                strHtml.Append("<tbody>");

                strHtml.Append("<tr>");
                strHtml.Append("<td style='width:150px;'>Variaciones Sostenidas de Frecuencia</td>");
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSosFrecPeriodo.Replace("\n", "<br>"));
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSosFrecValor.Replace("\n", "<br>"));
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSosFrecNTransg);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSosFrecMax);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSosFrecMin);
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<td style='width:150px;'>Variaciones Súbitas de Frecuencia</td>");
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSubFrecPeriodo.Replace("\n", "<br>"));
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSubFrecValor.Replace("\n", "<br>"));
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSubFrecNTransg);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSubFrecMax);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", regGPS.VarSubFrecMin);
                strHtml.Append("</tr>");

                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
                #endregion
            }

            strHtml.Append("</div");

            return strHtml.ToString();
        }

        /// <summary>
        /// GeneraRptSistemasAisladosTemporalesYVariacionesSostenidasSubitas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        public static void GeneraRptSistemasAisladosTemporalesYVariacionesSostenidasSubitas(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2,
                        List<InfSGIAisladosTempGPS> lista)
        {
            int row = rowIni;

            #region Cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;
            int rowIniCabecera = rowIniNombreReporte;

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Reporte de Sistemas Aislados Temporales y sus variaciones sostenidas y súbitas de frecuencia";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #endregion
            row = rowIniCabecera + 1;

            #region Cuerpo
            if (lista.Count > 0)
            {
                GeneraRptTablaSistemasAisladosTemporalesYVariacionesSostenidasSubitas(ws, row, colIniNombreReporte, fecha1, fecha2, lista);
            }

            #endregion

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// GeneraRptTablaSistemasAisladosTemporalesYVariacionesSostenidasSubitas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="colIniNombreReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="listaGPS"></param>
        public static void GeneraRptTablaSistemasAisladosTemporalesYVariacionesSostenidasSubitas(ExcelWorksheet ws, int row, int colIniNombreReporte, DateTime fecha1, DateTime fecha2,
                    List<InfSGIAisladosTempGPS> listaGPS)
        {
            int colGps = colIniNombreReporte;
            int colPeriodoHora = colGps + 1;
            int colValor = colPeriodoHora + 1;
            int colNTransg = colValor + 1;
            int colHzMax = colNTransg + 1;
            int colHzMin = colHzMax + 1;

            foreach (var regGPS in listaGPS)
            {
                int filGps = row;
                int filTol = filGps + 1;
                int filHz = filTol + 1;
                int filMaxMin = filHz + 1;

                ws.Cells[filGps, colGps].Value = regGPS.Gpsnombre;
                ws.Cells[filTol, colGps].Value = "INDICADOR DE CALIDAD";
                ws.Cells[filTol, colPeriodoHora].Value = "PERIODO / HORA";
                ws.Cells[filTol, colValor].Value = "VALOR";
                ws.Cells[filTol, colNTransg].Value = "N° TRANSGRESIONES\nACUMULADAS - MES";
                ws.Cells[filTol, colHzMax].Value = "TOLERANCIA NTCSE";

                ws.Cells[filHz, colHzMax].Value = "Hz";

                ws.Cells[filMaxMin, colHzMax].Value = "MAX.";
                ws.Cells[filMaxMin, colHzMin].Value = "MMIN.";

                UtilExcel.CeldasExcelAgrupar(ws, filGps, colGps, filGps, colHzMin);
                UtilExcel.CeldasExcelAgrupar(ws, filTol, colHzMax, filTol, colHzMin);
                UtilExcel.CeldasExcelAgrupar(ws, filHz, colHzMax, filHz, colHzMin);

                UtilExcel.CeldasExcelAgrupar(ws, filTol, colGps, filMaxMin, colGps);
                UtilExcel.CeldasExcelAgrupar(ws, filTol, colPeriodoHora, filMaxMin, colPeriodoHora);
                UtilExcel.CeldasExcelAgrupar(ws, filTol, colValor, filMaxMin, colValor);
                UtilExcel.CeldasExcelAgrupar(ws, filTol, colNTransg, filMaxMin, colNTransg);


                UtilExcel.CeldasExcelColorTexto(ws, filGps, colGps, filMaxMin, colHzMin, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filGps, colGps, filMaxMin, colHzMin, ConstantesPR5ReportesServicio.ColorInfSGI);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filGps, colGps, filMaxMin, colHzMin, "Arial", 11);

                UtilExcel.CeldasExcelWrapText(ws, filGps, colGps, filMaxMin, colHzMin);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filGps, colGps, filMaxMin, colHzMin, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filGps, colGps, filMaxMin, colHzMin, "Centro");
                UtilExcel.CeldasExcelEnNegrita(ws, filGps, colGps, filMaxMin, colHzMin);
                UtilExcel.CeldasExcelColorFondoYBorder(ws, filGps, colGps, filMaxMin, colHzMin, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

                //datos
                int filaSos = filMaxMin + 1;
                int filaSub = filaSos + 1;
                ws.Cells[filaSos, colGps].Value = "Variaciones Sostenidas de Frecuencia";
                ws.Cells[filaSos, colPeriodoHora].Value = regGPS.VarSosFrecPeriodo;
                ws.Cells[filaSos, colValor].Value = regGPS.VarSosFrecValor;
                ws.Cells[filaSos, colNTransg].Value = regGPS.VarSosFrecNTransg;
                ws.Cells[filaSos, colHzMax].Value = regGPS.VarSosFrecMax;
                ws.Cells[filaSos, colHzMin].Value = regGPS.VarSosFrecMin;

                ws.Cells[filaSub, colGps].Value = "Variaciones Súbitas de Frecuencia\t";
                ws.Cells[filaSub, colPeriodoHora].Value = regGPS.VarSubFrecPeriodo;
                ws.Cells[filaSub, colValor].Value = regGPS.VarSubFrecValor;
                ws.Cells[filaSub, colNTransg].Value = regGPS.VarSubFrecNTransg;
                ws.Cells[filaSub, colHzMax].Value = regGPS.VarSubFrecMax;
                ws.Cells[filaSub, colHzMin].Value = regGPS.VarSubFrecMin;

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaSos, colGps, filaSub, colHzMin, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaSos, colGps, filaSub, colHzMin, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaSos, colGps, filaSub, colHzMin, "Arial", 10);
                UtilExcel.CeldasExcelWrapText(ws, filaSos, colGps, filaSub, colHzMin);
                UtilExcel.BorderCeldas2(ws, filaSos, colGps, filaSub, colHzMin);

                ws.Row(filGps).Height = 30;

                row = filaSub + 3;
            }

            ws.Column(colGps).Width = 38;
            ws.Column(colPeriodoHora).Width = 23;
            ws.Column(colValor).Width = 23;
            ws.Column(colNTransg).Width = 23;
            ws.Column(colHzMax).Width = 23;
            ws.Column(colHzMin).Width = 23;
        }

        #endregion

        #endregion

        #region DESVIACIONES CON RESPECTO AL PDO

        // 3.13.2.30.	Desviaciones de la demanda respecto a su pronóstico
        #region REPORTE_DESVIACIONES_DEMANDA_PRONOSTICO

        /// <summary>
        /// Genera vista html de reporte desviacion de la demanda de acuerdo a su pronostico
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteDesviacionDemandaPronosticoHtml(List<MeMedicion48DTO> data, DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo1();
            NumberFormatInfo nfi3 = GenerarNumberFormatInfo3();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            strHtml.Append("<table border='1' style='width:500px' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;' >FECHA</th>");
            strHtml.Append("<th style='width:70px;' >EJECUTADA</th>");
            strHtml.Append("<th style='width:70px;' >PROG. DIARIA </th>");
            strHtml.Append("<th style='width:70px;' >DESVIACIÓN(%)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");
            if (data.Count > 0)
            {
                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    //Buscamos Demanda Ejecutada
                    var objEjecutado = data.Find(x => x.Medifecha == day && x.Lectcodi != ConstantesPR5ReportesServicio.LectCodiProgDiaria);
                    //Buscamos Demanda Proyectada Diaria
                    var objProgDiario = data.Find(x => x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectCodiProgDiaria);

                    for (int k = 1; k <= 48; k++)
                    {
                        strHtml.Append("<tr>");

                        decimal? valor1 = null, valor2 = null;

                        //Fecha - hora
                        string fechaMin = (day.AddMinutes(k * 30)).ToString(ConstantesAppServicio.FormatoFechaFull);
                        strHtml.AppendFormat("<td class='tdbody_reporte'>{0}</td>", fechaMin);

                        //Ejecutado
                        if (objEjecutado != null)
                        {
                            valor1 = (decimal?)objEjecutado.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(objEjecutado, null);

                            string str1 = valor1 != null ? ((decimal)valor1).ToString("N", nfi) : string.Empty;
                            strHtml.AppendFormat("<td>{0}</td>", str1);
                        }
                        else { strHtml.Append("<td></td>"); }

                        //Programado
                        if (objProgDiario != null)
                        {
                            valor2 = (decimal?)objProgDiario.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(objProgDiario, null);

                            string str2 = valor2 != null ? ((decimal)valor2).ToString("N", nfi) : string.Empty;
                            strHtml.AppendFormat("<td>{0}</td>", str2);
                        }
                        else { strHtml.Append("<td></td>"); }

                        //Desviación
                        decimal? valorDesv;
                        if (valor1 != null && (decimal)valor1 != 0)
                        {
                            valorDesv = (valor2.GetValueOrDefault(0) - valor1.Value) / valor1.Value;
                            strHtml.AppendFormat("<td>{0} %</td>", ((decimal)valorDesv * 100).ToString("N", nfi3));
                        }
                        else { strHtml.Append("<td></td>"); }

                        strHtml.Append("</tr>");
                    }
                }
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Exportacion Excel de RptDesviacionDemandaPronostico
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nfilIni"></param>
        /// <param name="ncolIni"></param>
        /// <param name="nfilFin"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptDesviacionDemandaPronostico(ExcelWorksheet ws, ref int nfilIni, ref int ncolIni, ref int nfilFin, int tipoGrafico, DateTime fecha1, DateTime fecha2, List<MeMedicion48DTO> lista, List<MeMedicion48DTO> listaVersion)
        {
            int filaIniTitulo = 5;
            int coluIniTitulo = 2;

            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = "Desviaciones de la demanda respecto a su pronóstico";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int row = filaIniTitulo + 1;
            int col = coluIniTitulo;

            #region cabecera
            int colIniFecha = col;
            int rowIniFecha = row;
            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";

            int colIniHora = colIniFecha + 1;
            int rowIniHora = rowIniFecha;
            ws.Cells[rowIniHora, colIniHora].Value = "HORA";

            int colIniEjec = colIniHora + 1;
            int rowIniEjec = rowIniFecha;
            ws.Cells[rowIniEjec, colIniEjec].Value = "EJECUTADA";

            int colIniProg = colIniEjec + 1;
            int rowIniProg = rowIniFecha;
            ws.Cells[rowIniProg, colIniProg].Value = "PROG. DIARA";

            int colIniDesv = colIniProg + 1;
            int rowIniDesv = rowIniFecha;
            ws.Cells[rowIniDesv, colIniDesv].Value = "DESVIACIÓN(%)";


            #region Formato Cabecera

            //Fecha - Hora
            UtilExcel.CeldasExcelColorTexto(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, "#FFFFFF");
            //UtilExcel.CeldasExcelColorFondo(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniDesv, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            #region cuerpo
            if (lista.Count > 0)
            {
                int rowIniData = rowIniFecha + 1;
                row = rowIniData;

                int numDia = 0;

                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    numDia++;

                    //HORA
                    DateTime horas = day.AddMinutes(30);

                    //Buscamos Demanda Ejecutada
                    var objEjecutado = lista.Find(x => x.Medifecha == day && x.Lectcodi != ConstantesPR5ReportesServicio.LectCodiProgDiaria);
                    //Buscamos Demanda Proyectada Diaria
                    var objProgDiario = lista.Find(x => x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectCodiProgDiaria);

                    for (int k = 1; k <= 48; k++)
                    {
                        decimal? valor1 = null, valor2 = null;

                        //Fecha - hora
                        //Fecha
                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);

                        //Ejecutado
                        if (objEjecutado != null)
                        {
                            valor1 = (decimal?)objEjecutado.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(objEjecutado, null);
                            ws.Cells[row, colIniEjec].Value = valor1;
                        }

                        //Programado
                        if (objProgDiario != null)
                        {
                            valor2 = (decimal?)objProgDiario.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(objProgDiario, null);
                            ws.Cells[row, colIniProg].Value = valor2;
                        }

                        //Desviación
                        decimal? valorDesv;
                        if (valor1 != null && (decimal)valor1 != 0)
                        {
                            valorDesv = (valor2.GetValueOrDefault(0) - valor1.Value) / valor1.Value;
                            ws.Cells[row, colIniDesv].Value = valorDesv;
                        }
                        row++;
                        horas = horas.AddMinutes(30);
                    }
                }

                int rowFinData = rowIniData + 48 * numDia - 1;

                #region Formato Cuerpo
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFecha, rowFinData, colIniDesv, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniFecha, rowFinData, colIniDesv, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniFecha, rowFinData, colIniDesv, "Arial", 10);

                UtilExcel.CeldasExcelEnNegrita(ws, rowIniData, colIniFecha, rowFinData, colIniHora);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniFecha, rowFinData, colIniDesv);

                ws.Cells[rowIniData, colIniEjec, rowFinData, colIniProg].Style.Numberformat.Format = "#,##0.0";
                ws.Cells[rowIniData, colIniDesv, rowFinData, colIniDesv].Style.Numberformat.Format = ConstantesPR5ReportesServicio.FormatoNumero3DigitoPorcentaje;

                #endregion

                #region grafico

                nfilIni = rowIniData;
                nfilFin = rowFinData;
                ncolIni = colIniHora;

                #endregion
            }

            #endregion

            ws.Column(colIniFecha).Width = 11;
            ws.Column(colIniHora).Width = 9;
            ws.Row(rowIniFecha).Height = 27;

            ws.Column(colIniEjec).Width = 18;
            ws.Column(colIniProg).Width = 18;
            ws.Column(colIniDesv).Width = 18;

            ws.View.FreezePanes(rowIniFecha + 1, colIniFecha + 1);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// Genera grafico tipo Linea en archivo excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        public static void AddGraficoDesviacionDemandaPronostico(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, string xAxisTitle, string yAxisTitle, string titulo)
        {
            if (rowFin > 0)
            {
                string nameGraf = string.Empty;
                //Set top left corner to row 1 column 2
                var LineaChart = ws.Drawings.AddChart(nameGraf, eChartType.Line) as ExcelLineChart;
                LineaChart.SetPosition(rowIni, 0, colIni + 4, 0);

                LineaChart.SetSize(1200, 600);

                for (int i = 1; i <= 2; i++)
                {
                    var ran1 = ws.Cells[rowIni, colIni + i, rowFin, colIni + i];
                    var ran2 = ws.Cells[rowIni, colIni, rowFin, colIni];

                    var serie = (ExcelChartSerie)LineaChart.Series.Add(ran1, ran2);
                    serie.Header = (ws.Cells[rowIni - 1, colIni + i].Value != null) ? ws.Cells[rowIni - 1, colIni + i].Value.ToString() : "";
                }
                LineaChart.Title.Text = titulo;
                LineaChart.DataLabel.ShowLeaderLines = true;
                LineaChart.YAxis.Title.Text = yAxisTitle;

                LineaChart.Legend.Position = eLegendPosition.Bottom;
            }
        }

        #endregion

        // 3.13.2.31.	Desviaciones de la producción de las Unidades de Generación
        #region REPORTE_DESVIACIONES_PRODUCCION_UG

        /// <summary>
        /// Genera vista html de reporte desviacion de la producción UG
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaPto"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteDesviacionesProduccionUGHtml(List<MeMedicion48DTO> data, List<MePtomedicionDTO> listaPto, DateTime fechaInicio, List<MeMedicion48DTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo2();

            int padding = 20;
            int ancho = 56;
            int anchoTotal = (100 + padding) + (listaPto.Count * 4 * (ancho + padding));

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px;' >", anchoTotal);
            strHtml.Append("<thead>");

            #region
            if (data.Count == 0)
            {
                strHtml.Append("<tr><th style='width: 400px;'>FECHA / HORA</th></tr><thead><tbody><tr><td>sin datos</td></tr></tbody>");
                return strHtml.ToString();
            }

            strHtml.Append("<tr>");

            strHtml.Append("<th rowspan='3' style='width: 100px;'>FECHA / HORA</th>");

            // *************  NOMBRE DE EMPRESAS  *****************************************************************************    
            foreach (var reg in listaPto)
            {
                var emprcodi = reg.Emprcodi;

                strHtml.AppendFormat("<th colspan ='{0}' style='word-wrap: break-word; white-space: normal;width: {1}px' >" + reg.Emprnomb + "</th>", 4, 4 * ancho);
            }
            strHtml.Append("</tr>");

            // ************  NOMBRE DE UNIDADES  *********************************************************************************
            strHtml.Append("<tr>");
            foreach (var reg in listaPto)
            {
                var emprcodi = reg.Emprcodi;
                var empresa = reg.Emprnomb;

                var grupocodi = reg.Grupocodi;
                var gruponomb = reg.Gruponomb;
                var ptomedicodi = reg.Ptomedicodi;

                strHtml.AppendFormat("<th colspan ='{0}' style='word-wrap: break-word; white-space: normal;width: {1}px'>" + gruponomb + "</th>", 4, ancho * 4);
            }
            strHtml.Append("</tr>");


            // ************  PARAMETRO  *********************************************************************************
            strHtml.Append("<tr>");
            foreach (var reg in listaPto)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: {0}px'>EJE</th>", ancho);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: {0}px'>REPROG</th>", ancho);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: {0}px'>PROG</th>", ancho);
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal;width: {0}px'>EJE-<br/>PROG</th>", ancho);
            }
            strHtml.Append("</tr>");
            #endregion

            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            #region Cuerpo
            fechaInicio = fechaInicio.Date;
            DateTime horas = fechaInicio.Date.AddMinutes(30);
            for (int k = 1; k <= 48; k++)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='tdbody_reporte'>{0:dd/MM/yyyy HH:mm}</td>", horas);

                foreach (var pto48 in listaPto)
                {
                    MeMedicion48DTO m48Ejec = data.Find(x => x.Emprcodi == pto48.Emprcodi && x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);
                    MeMedicion48DTO m48Prog = data.Find(x => x.Emprcodi == pto48.Emprcodi && x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);
                    MeMedicion48DTO m48Reprog = data.Find(x => x.Emprcodi == pto48.Emprcodi && x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoReprogramado);

                    decimal? valorEjec = null, valorProg = null; decimal diferencia = 0;
                    //Imprime Columna Despacho Ejecutado 
                    if (m48Ejec != null)
                    {
                        valorEjec = (decimal?)m48Ejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(m48Ejec, null);
                        string str1 = valorEjec != null ? ((decimal)valorEjec).ToString("N", nfi) : string.Empty;
                        strHtml.AppendFormat("<td>{0}</td>", str1);
                    }
                    else { strHtml.AppendFormat("<td></td>"); }

                    //Imprime Columna Reprogramado
                    if (m48Reprog != null)
                    {
                        decimal? valor1 = (decimal?)m48Reprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(m48Reprog, null);
                        string str1 = valor1 != null ? ((decimal)valor1).ToString("N", nfi) : string.Empty;
                        strHtml.AppendFormat("<td>{0}</td>", str1);
                    }
                    else { strHtml.AppendFormat("<td></td>"); }

                    //Imprime Columna Programado
                    if (m48Prog != null)
                    {
                        valorProg = (decimal?)m48Prog.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(m48Prog, null);
                        string str1 = valorProg != null ? ((decimal)valorProg).ToString("N", nfi) : string.Empty;
                        strHtml.AppendFormat("<td>{0}</td>", str1);
                    }
                    else { strHtml.AppendFormat("<td></td>"); }

                    //Imprime Columna Eje-Prog
                    diferencia = valorEjec.GetValueOrDefault(0) - valorProg.GetValueOrDefault(0);
                    strHtml.AppendFormat("<td>{0}</td>", (diferencia).ToString("N", nfi));

                }
                strHtml.Append("</tr>");

                horas = horas.AddMinutes(30);
            }

            //Total MWh
            strHtml.Append("<tr>");
            strHtml.Append("<td class='total_potencia_activa_ejecutado'>EJEC (MWh)</td>");
            foreach (var pto48 in listaPto)
            {
                MeMedicion48DTO m48Ejec = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);
                MeMedicion48DTO m48Prog = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);
                MeMedicion48DTO m48Reprog = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Medifecha == fechaInicio && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoReprogramado);

                decimal valorEjec = m48Ejec != null ? m48Ejec.Meditotal.GetValueOrDefault(0) / 2 : 0.0m;
                decimal valorProg = m48Prog != null ? m48Prog.Meditotal.GetValueOrDefault(0) / 2 : 0.0m;
                decimal valorReprog = m48Reprog != null ? m48Reprog.Meditotal.GetValueOrDefault(0) / 2 : 0.0m;

                strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", valorEjec.ToString("N", nfi));
                strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", valorReprog.ToString("N", nfi));
                strHtml.AppendFormat("<td class='total_potencia_activa_ejecutado'>{0}</td>", valorProg.ToString("N", nfi));
                strHtml.Append("<td class='total_potencia_activa_ejecutado'></td>");
            }
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        public static void GeneraRptDesviacionesProduccionUG(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2,
            List<MePtomedicionDTO> listaPto48, List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion)
        {
            List<MeMedicion48DTO> listaAcumulado = new List<MeMedicion48DTO>();

            int row = rowIni + 1;
            int col = colIni;

            #region cabecera

            int rowIniTitulo = rowIni;
            int colIniTitulo = col;
            int colFinTitulo = colIniTitulo;

            // Fila Hora - Empresa - Total

            int colIniFecha = col;
            int rowIniFecha = row;
            int rowFinFecha = rowIniFecha + 3 - 1;
            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int colIniHora = colIniFecha + 1;
            int rowIniHora = rowIniFecha;
            int rowFinHora = rowFinFecha;
            ws.Cells[rowIniHora, colIniHora].Value = "HORA";
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Merge = true;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.WrapText = true;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int rowIniEmp = row;
            int colIniEmp = colIniHora + 1;
            int colFinEmp = colIniEmp;

            int rowIniGrupoDespacho = rowIniEmp + 1;
            int colIniGrupoDespacho = colIniHora + 1;
            int colFinGrupoDespacho = colIniGrupoDespacho;

            int rowIniTipo = rowIniGrupoDespacho + 1;

            var colorBorder = Color.White;
            var descTitulo = "DESVIACIÓN DEL DESPACHO DE LAS CENTRALES DE GENERACIÓN COES (MW)";

            //Grupos Despacho
            for (int k = 0; k < listaPto48.Count; k++)
            {
                var thGrupoDespacho = listaPto48[k];

                colFinGrupoDespacho = colIniGrupoDespacho;

                ws.Cells[rowIniEmp, colIniGrupoDespacho].Value = thGrupoDespacho.Emprnomb.Trim();
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Style.Font.Size = 9;
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Merge = true;
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Style.WrapText = true;
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniEmp, colIniGrupoDespacho, rowIniEmp, colIniGrupoDespacho + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Value = thGrupoDespacho.Gruponomb.Trim();
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Style.Font.Size = 8;
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho + 3].Merge = true;
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho + 3].Style.WrapText = true;
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniTipo, colIniGrupoDespacho].Value = "EJE";
                ws.Cells[rowIniTipo, colIniGrupoDespacho].Style.Font.Size = 8;
                ws.Cells[rowIniTipo, colIniGrupoDespacho].Style.WrapText = true;
                ws.Cells[rowIniTipo, colIniGrupoDespacho].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTipo, colIniGrupoDespacho].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTipo, colIniGrupoDespacho].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Value = "REPROG";
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Style.Font.Size = 8;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Style.WrapText = true;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Value = "PROG";
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Style.Font.Size = 8;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Style.WrapText = true;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Value = "EJE - PROG";
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Style.Font.Size = 8;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Style.WrapText = true;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniTipo, colIniGrupoDespacho + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                colFinTitulo = colFinGrupoDespacho + 3;
                colIniGrupoDespacho = colFinTitulo + 1;
            }

            //Nombre Tipo empresa
            ws.Cells[rowIniTitulo, colIniTitulo].Value = descTitulo;
            ws.Cells[rowIniTitulo, colIniTitulo].Style.Font.Size = 18;
            ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Merge = true;
            ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Style.WrapText = true;
            ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            using (var range = ws.Cells[rowIniTitulo, colIniTitulo, rowIniTipo, colFinTitulo])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(colorBorder);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(colorBorder);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(colorBorder);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(colorBorder);
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
                range.Style.Font.Color.SetColor(Color.White);
            }

            #endregion

            int rowIniData = rowIniTipo + 1;
            row = rowIniData;

            #region cuerpo

            int numDia = 0;

            int colData = colIniHora;
            for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
            {
                numDia++;

                //HORA
                DateTime horas = day.AddMinutes(30);

                for (int h = 1; h <= 48; h++)
                {
                    ws.Row(row).Height = 24;

                    //Fecha
                    ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[row, colIniFecha].Style.Font.Bold = true;
                    ws.Cells[row, colIniFecha].Style.Font.Size = 10;
                    ws.Cells[row, colIniFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, colIniFecha].Style.Fill.BackgroundColor.SetColor(Color.White);

                    colData = colIniHora;
                    //Hora
                    ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);
                    ws.Cells[row, colIniHora].Style.Font.Bold = true;
                    ws.Cells[row, colIniHora].Style.Font.Size = 10;
                    ws.Cells[row, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

                    colData++;

                    foreach (var pto48 in listaPto48)
                    {
                        MeMedicion48DTO m48Ejec = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);
                        MeMedicion48DTO m48Prog = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);
                        MeMedicion48DTO m48Reprog = data.Find(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Medifecha == day && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoReprogramado);

                        decimal? valorEjec = null, valorReprog = null, valorProg = null, diferencia;

                        //Ejecutado
                        if (m48Ejec != null)
                        {
                            valorEjec = (decimal?)m48Ejec.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48Ejec, null);
                            ws.Cells[row, colData].Value = valorEjec;
                        }
                        colData++;

                        //Reprogramado
                        if (m48Reprog != null)
                        {
                            valorReprog = (decimal?)m48Reprog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48Reprog, null);
                            ws.Cells[row, colData].Value = valorReprog;
                        }
                        colData++;

                        //Programado
                        if (m48Prog != null)
                        {
                            valorProg = (decimal?)m48Prog.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48Prog, null);
                            ws.Cells[row, colData].Value = valorProg;
                        }
                        colData++;

                        //Eje-Prog
                        diferencia = valorEjec.GetValueOrDefault(0) - valorProg.GetValueOrDefault(0);
                        ws.Cells[row, colData].Value = diferencia;
                        colData++;
                    }

                    horas = horas.AddMinutes(30);
                    row++;
                }
            }

            //EJEC Total
            decimal? valor, total;
            foreach (var m48 in data)
            {
                valor = 0;
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor != null ? valor.Value : 0;
                }

                m48.Meditotal = total;
            }

            int rowEjec = row;
            ws.Cells[rowEjec, colIniFecha].Value = "EJEC (MWh)";
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Merge = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Bold = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Font.Size = 12;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.WrapText = true;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowEjec, colIniFecha, rowEjec, colIniHora].Style.Fill.BackgroundColor.SetColor(Color.White);

            //Formatear data
            colData = colIniHora + 1;

            foreach (var pto48 in listaPto48)
            {
                List<MeMedicion48DTO> lista48Ejec = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto).ToList();
                List<MeMedicion48DTO> lista48Reprog = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoReprogramado).ToList();
                List<MeMedicion48DTO> lista48Prog = data.Where(x => x.Ptomedicodi == pto48.Ptomedicodi && x.Emprcodi == pto48.Emprcodi && x.Lectcodi == ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario).ToList();

                decimal totalPtoEjec = lista48Ejec.Sum(x => x.Meditotal.GetValueOrDefault(0));
                decimal totalPtoReprog = lista48Reprog.Sum(x => x.Meditotal.GetValueOrDefault(0));
                decimal totalPtoProg = lista48Prog.Sum(x => x.Meditotal.GetValueOrDefault(0));

                BorderCeldaProgUG(ws, row, colData, totalPtoEjec / 2);
                colData++;
                BorderCeldaProgUG(ws, row, colData, totalPtoReprog / 2);
                colData++;
                BorderCeldaProgUG(ws, row, colData, totalPtoProg / 2);
                colData++;
                BorderCeldaProgUG(ws, row, colData, null);
                colData++;
            }
            colData--;
            if (colIniHora + 1 > colData) colData = colIniHora + 1;
            using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 48, colData])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Numberformat.Format = "#,##0.00";
                range.Style.Font.Size = 10;
            }
            using (var range = ws.Cells[rowIniData + numDia * 48, colIniHora + 1, rowIniData + numDia * 48, colData])
            {
                range.Style.Font.Size = 12;
            }

            //mostrar lineas horas data
            for (int c = colIniTitulo; c <= colData; c++)
            {
                for (int f = rowIniData; f < rowIniData + numDia * 48; f += 8)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                    ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                }
            }
            //Formato de Filas y columnas
            for (int columna = colIniHora + 1; columna <= colData; columna++)
                ws.Column(columna).Width = 12;

            ws.Column(colIniFecha).Width = 11;
            ws.Column(colIniHora).Width = 9;
            ws.Row(rowIniTitulo).Height = 30;
            ws.Row(rowIniEmp).Height = 20;
            ws.Row(rowIniGrupoDespacho).Height = 40;
            ws.Row(rowIniTipo).Height = 20;
            ws.Row(rowEjec).Height = 25;

            #endregion

            ws.View.FreezePanes(rowFinFecha + 1, colIniHora + 1);
            ws.View.ZoomScale = 70;
        }

        /// <summary>
        /// BorderCeldaProgUG
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="colData"></param>
        /// <param name="valor"></param>
        public static void BorderCeldaProgUG(ExcelWorksheet ws, int row, int colData, decimal? valor)
        {
            ws.Cells[row, colData].Value = valor;
            ws.Cells[row, colData].Style.Font.Bold = true;
            ws.Cells[row, colData].Style.Font.Size = 12;
            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[row, colData].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6DCE4"));
            ws.Cells[row, colData].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            ws.Cells[row, colData].Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
            ws.Cells[row, colData].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            ws.Cells[row, colData].Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI));
            ws.Cells[row, colData].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[row, colData].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, colData].Style.Border.Left.Color.SetColor(Color.Blue);
            ws.Cells[row, colData].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, colData].Style.Border.Right.Color.SetColor(Color.Blue);
        }

        #endregion

        #endregion

        #region INFORMACIÓN DEL MERCADO DE CORTO PLAZO

        // 3.13.2.32.	Costos Marginales de Corto Plazo cada 30 minutos en las Barras del SEIN.
        #region REPORTE_COSTO_MARGINALES_CORTO_PLAZO

        /// <summary>
        /// ReporteCostoMarginalesCPHtml
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="tipoInfo"></param>
        /// <param name="data"></param>
        /// <param name="listaBarra"></param>
        /// <returns></returns>
        public static string ReporteCostoMarginalesCPHtml(DateTime fecha1, int tipoInfo, List<SiCostomarginalDTO> data, List<BarraDTO> listaBarra)
        {
            StringBuilder strHtml = new StringBuilder();
            string cabecer = string.Empty;

            NumberFormatInfo nfi2 = GenerarNumberFormatInfo3();
            nfi2.NumberGroupSeparator = " ";
            nfi2.NumberDecimalDigits = 2;
            nfi2.NumberDecimalSeparator = ",";

            var total = listaBarra.Count * 80;
            strHtml.AppendFormat("<div id='div_reporte_{0}' style='height: auto; margin: 0 auto;'>", tipoInfo);
            strHtml.AppendFormat("<table id='reporte_{1}' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px'>", total, tipoInfo);

            strHtml.Append("<thead>");
            #region cabecera

            //Área
            cabecer += "<tr>";
            cabecer += "<th style=''>FECHA HORA</th>";
            foreach (var barra in listaBarra)
            {
                cabecer += string.Format("<th style='word-wrap: break-word;white-space: normal;'>{0}</th>", barra.BarrNombre);
            }
            cabecer += "</tr>";
            #endregion
            strHtml.Append(cabecer);
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            #region cuerpo
            if (listaBarra.Count() > 0)
            {
                DateTime horas = fecha1;

                //data 48
                for (int h = 1; h <= 48; h++)
                {
                    //hora
                    horas = horas.AddMinutes(30);
                    if (h == 48) horas = horas.AddMinutes(-1);
                    List<SiCostomarginalDTO> listaXFechaHora = data.Where(x => x.Cmgrfecha == horas).ToList();

                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td class='tdbody_reporte'>{0:HH:mm}</td>", horas);

                    //data
                    for (int i = 0; i < listaBarra.Count; i++)
                    {
                        string _bground = string.Empty, descrip = string.Empty;
                        decimal? valor = null;

                        var objSiCmg = listaXFechaHora.Find(x => x.Barrcodi == listaBarra[i].BarrCodi);
                        if (objSiCmg != null)
                        {
                            switch (tipoInfo)
                            {
                                case 1: valor = objSiCmg.Cmgrtotal; break;
                                case 2: valor = objSiCmg.Cmgrenergia; break;
                                case 3: valor = objSiCmg.Cmgrcongestion; break;
                            }
                            descrip = (valor.GetValueOrDefault(0)).ToString("N", nfi2);
                        }
                        strHtml.AppendFormat("<td style='background:" + _bground + "'>{0}</td>", descrip);
                    }

                    strHtml.Append("</tr>");
                }
            }
            #endregion
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="tipoInfo"></param>
        /// <param name="data"></param>
        /// <param name="listaBarra"></param>
        public static void GeneraRptCostoMarginalesCP(ExcelWorksheet ws, DateTime fecha1, int tipoInfo, List<SiCostomarginalDTO> data, List<BarraDTO> listaBarra)
        {
            #region cabecera

            int filIniFechaHora = 3;
            int colIniFechaHora = 2;
            int colIniReporte = colIniFechaHora + 1;
            int colFinReporte = colIniFechaHora + listaBarra.Count();

            ws.Cells[filIniFechaHora, colIniFechaHora].Value = "S/./MWh";
            for (int i = 0; i < listaBarra.Count; i++)
            {
                ws.Cells[filIniFechaHora, colIniReporte + i].Value = listaBarra[i].BarrNombre;
                ws.Column(colIniReporte + i).Width = 19;
            }

            UtilExcel.SetFormatoCelda(ws, filIniFechaHora, colIniFechaHora, filIniFechaHora, colFinReporte, "Centro", "Centro", "#FFFFFF", ConstantesPR5ReportesServicio.ColorInfSGI, "Arial", 11, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, filIniFechaHora, colIniFechaHora, filIniFechaHora, colFinReporte, "#FFFFFF", true, true);

            #endregion

            #region cuerpo

            int rowIni = filIniFechaHora + 1;
            for (int h = 1; h <= 48; h++)
            {
                rowIni = filIniFechaHora + h;

                DateTime fechaHora = fecha1.AddMinutes(h * 30);
                if (h == 48) fechaHora = fechaHora.AddMinutes(-1);
                List<SiCostomarginalDTO> listaXFechaHora = data.Where(x => x.Cmgrfecha == fechaHora).ToList();

                ws.Cells[rowIni, colIniFechaHora].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                ws.Cells[rowIni, colIniFechaHora].Value = fechaHora;

                for (int i = 0; i < listaBarra.Count; i++)
                {
                    decimal? valor = null;

                    var objSiCmg = listaXFechaHora.Find(x => x.Barrcodi == listaBarra[i].BarrCodi);
                    if (objSiCmg != null)
                    {
                        switch (tipoInfo)
                        {
                            case 1: valor = objSiCmg.Cmgrtotal; break;
                            case 2: valor = objSiCmg.Cmgrenergia; break;
                            case 3: valor = objSiCmg.Cmgrcongestion; break;
                        }
                    }

                    ws.Cells[rowIni, colIniReporte + i].Value = valor.GetValueOrDefault(0);
                    ws.Cells[rowIni, colIniReporte + i].Style.Numberformat.Format = "#,##0.00";
                }
            }

            rowIni = filIniFechaHora + 1;
            int rowFin = filIniFechaHora + 48;

            UtilExcel.SetFormatoCelda(ws, rowIni, colIniFechaHora, rowFin, colFinReporte, "Centro", "Centro", "#FFFFFF", ConstantesPR5ReportesServicio.ColorInfSGI, "Arial", 11, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIni, colIniFechaHora, rowFin, colFinReporte, "#FFFFFF", true, true);

            if (colFinReporte >= colIniFechaHora + 1)
            {
                UtilExcel.SetFormatoCelda(ws, rowIni, colIniFechaHora + 1, rowFin, colFinReporte, "Centro", "Centro", "#000000", "#FFFFFF", "Arial", 11, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIni, colIniFechaHora, rowFin, colFinReporte, "#000000", true, true);
            }

            ws.Column(colIniFechaHora).Width = 25;
            //ws.Row(filIniFechaHora).Height = 33;

            #endregion

            #region Escala Y freezer
            ws.View.FreezePanes(filIniFechaHora + 1, colIniFechaHora + 1);
            ws.View.ZoomScale = 70;
            #endregion
        }

        #endregion

        // 3.13.2.33.	Costo total de operación ejecutada.
        #region COSTO TOTAL OPERACION EJECUTADA

        /// <summary>
        /// GeneraRptCostoTotalOperacionEjecutada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptCostoTotalOperacionEjecutada(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2, ref int row, ref int col, List<MeMedicion1DTO> lista, List<MeMedicion1DTO> listaVersion)
        {
            List<MeMedicion1DTO> listaEjec = lista.Where(x => x.Lectcodi == ConstantesPR5ReportesServicio.LectCostoOperacionEjec).ToList();
            List<MeMedicion1DTO> listaProg = lista.Where(x => x.Lectcodi == ConstantesPR5ReportesServicio.LectCostoOperacionProg).ToList();

            List<DateTime> listaFecha = lista.Select(x => x.Medifecha.Date).Distinct().OrderBy(x => x.Date).ToList();
            int totalDia = listaFecha.Count();
            decimal diferenciaCost = 0;
            //Mensaje
            var ejecModelFinal = listaEjec.Find(x => x.Medifecha == listaFecha[7]);
            var ejecModelInicio = listaEjec.Find(x => x.Medifecha == listaFecha[0]);
            string mensaje = string.Empty;
            if (ejecModelFinal.H1 != null && ejecModelInicio.H1 != null)
            {
                if (ejecModelFinal.H1 > ejecModelInicio.H1) diferenciaCost = ejecModelFinal.H1.Value - ejecModelInicio.H1.Value;
                else diferenciaCost = ejecModelInicio.H1.Value - ejecModelFinal.H1.Value;

                var porcentaje = Math.Round((diferenciaCost / ejecModelInicio.H1.Value) * 100, 2);
                mensaje = "El costo total de la operación ejecutado fue S/. " + Math.Round(ejecModelFinal.H1.Value) +
                    " y resultó S/. " + Math.Round(diferenciaCost) + " (" + porcentaje + "% )" + " menor que el correspondiente al " + ejecModelFinal.Medifecha.ToString("dddd",
                        new CultureInfo("es-ES")) + " de la semana pasada.";
            }

            #region cabecera

            int filaMensaje = 6;
            int filaTitDias = filaMensaje + 1;
            int coluTitDias = 2;

            int filaTitCostoEjecutado = filaTitDias;
            int coluTitCostoEjecutado = coluTitDias + 1;
            int coluTitCostoProgramado = coluTitCostoEjecutado + 1;
            int coluTitPorcentaje = coluTitCostoProgramado + 1;

            int filaIniData = filaTitDias + 1;
            int coluIniData = coluTitDias + 1;

            ws.Cells[filaMensaje, coluTitDias].Value = mensaje;

            ws.Cells[filaTitCostoEjecutado, coluTitDias].Value = "Fecha";
            ws.Cells[filaTitCostoEjecutado, coluTitCostoEjecutado].Value = "Costo Total Ejecutado";
            ws.Cells[filaTitCostoEjecutado, coluTitCostoProgramado].Value = "Costo Total Programado";
            ws.Cells[filaTitCostoEjecutado, coluTitPorcentaje].Value = "%";

            #region Formato Cabecera

            UtilExcel.CeldasExcelWrapText(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje);
            UtilExcel.CeldasExcelColorTexto(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje, "#FFFFFF");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, filaTitCostoEjecutado, coluTitDias, filaTitCostoEjecutado, coluTitPorcentaje, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            row = filaTitDias;
            col = coluTitDias;

            #endregion

            #region cuerpo

            int fila = 0;
            decimal porcentajeXDia = 0.00m;

            for (int h = 0; h < totalDia; h++)
            {
                DateTime fecha = listaFecha[h];
                ws.Cells[filaTitDias + 1 + fila, coluTitDias].Value = EPDate.f_NombreDiaSemanaCorto(fecha.DayOfWeek) + " " + fecha.Day;

                MeMedicion1DTO ejec = listaEjec.Find(x => x.Medifecha == listaFecha[h]);
                MeMedicion1DTO prog = listaProg.Find(x => x.Medifecha == listaFecha[h]);
                porcentajeXDia = prog.H1 > 0 ? (ejec.H1.GetValueOrDefault(0) / prog.H1.GetValueOrDefault(0) * 1) - 1 : 0;

                ws.Cells[filaIniData + fila, coluTitCostoEjecutado].Value = ejec.H1;
                ws.Cells[filaIniData + fila, coluTitCostoProgramado].Value = prog.H1;
                ws.Cells[filaIniData + fila, coluTitPorcentaje].Value = porcentajeXDia;
                ws.Cells[filaIniData + fila, coluTitPorcentaje].Style.Numberformat.Format = "0.0%";
                fila++;
            }

            int filaIni = filaTitCostoEjecutado + 1;
            int filaFin = filaIni + 7;

            #region Formato Cuerpo
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIni, coluTitDias, filaFin, coluTitPorcentaje, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIni, coluTitDias, filaFin, coluTitPorcentaje, "Centro");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIni, coluTitDias, filaFin, coluTitPorcentaje, "Arial", 10);

            UtilExcel.CeldasExcelEnNegrita(ws, filaIni, coluTitDias, filaFin, coluTitDias);

            UtilExcel.BorderCeldas2(ws, filaIni, coluTitDias, filaFin, coluTitPorcentaje);

            ws.Cells[filaIni, coluTitCostoEjecutado, filaFin, coluTitCostoProgramado].Style.Numberformat.Format = "#,##0.0";
            ws.Cells[filaIni, coluTitPorcentaje, filaFin, coluTitPorcentaje].Style.Numberformat.Format = ConstantesPR5ReportesServicio.FormatoNumero2DigitoPorcentaje;

            #endregion

            #endregion

            ws.Row(filaTitCostoEjecutado).Height = 33;
            ws.Column(coluTitDias).Width = 8;
            ws.Column(coluTitCostoEjecutado).Width = 18;
            ws.Column(coluTitCostoProgramado).Width = 18;
            ws.Column(coluTitPorcentaje).Width = 10;
        }

        /// <summary>
        /// AddGraficoCostoTotalOperacionEjecutada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="titulo"></param>
        /// <param name="rep"></param>
        /// <param name="iniFor"></param>
        public static void AddGraficoCostoTotalOperacionEjecutada(ExcelWorksheet ws, int row, int col, string xAxisTitle, string yAxisTitle, string titulo, int rep, int iniFor)
        {
            string nameGraf = string.Empty;
            int position_ = 0;
            switch (rep)
            {
                case 1: nameGraf = "grafCostoTotOperacionEjecutada"; position_ = row; break;
            }
            //Set top left corner to row 1 column 2
            var BiColumnaChart = ws.Drawings.AddChart(nameGraf, eChartType.ColumnClustered) as ExcelBarChart;
            BiColumnaChart.SetPosition(position_, 0, col + 5, 0);

            BiColumnaChart.SetSize(800, 400);

            int filaTitDias = row;
            int coluTitDias = col;

            int filaTitCostoEjecutado = filaTitDias;
            int coluTitCostoEjecutado = coluTitDias + 1;

            int filaTitCostoProgramado = filaTitDias;
            int coluTitCostoProgramado = coluTitCostoEjecutado + 1;

            int filaTitPorcentaje = filaTitDias;
            int coluTitPorcentaje = coluTitCostoProgramado + 1;

            int filaIniData = filaTitDias + 1;
            int coluIniData = coluTitDias + 1;

            var ranEjecutado = ws.Cells[filaTitCostoEjecutado + 1, coluTitCostoEjecutado, filaTitCostoEjecutado + 1 + 7, coluTitCostoEjecutado];
            var ranProgramado = ws.Cells[filaTitCostoProgramado + 1, coluTitCostoProgramado, filaTitCostoProgramado + 1 + 7, coluTitCostoProgramado];
            var ranFechas = ws.Cells[filaTitDias + 1, coluTitDias, filaTitDias + 1 + 7, coluTitDias];
            var ranPorcentajes = ws.Cells[filaTitPorcentaje + 1, coluTitPorcentaje, filaTitPorcentaje + 1 + 7, coluTitPorcentaje];

            //var serie = (ExcelChartSerie)BiColumnaChart.Series.Add(ranEjecutado, ranProgramado);
            //var seriePorcentaje = (ExcelBarChartSerie)BiColumnaChart.Series.Add(ranPorcentajes, ranPorcentajes);  // EJECUTADO  
            var serieEjecutado = (ExcelBarChartSerie)BiColumnaChart.Series.Add(ranEjecutado, ranFechas);  // EJECUTADO            
            var serieProgramado = (ExcelBarChartSerie)BiColumnaChart.Series.Add(ranProgramado, ranPorcentajes); // PROGRAMADO
            //var serieProgramado2 = (ExcelBarChartSerie)BiColumnaChart.Series.Add(ranFechas, ranPorcentajes); // PROGRAMADO

            serieProgramado.DataLabel.ShowCategory = true;
            serieProgramado.DataLabel.Position = eLabelPosition.Center;

            //-- quedamos en esta funcion, borrar esto para continuar --

            //seriePorcentaje.DataLabel.ShowValue = true;

            //serieEjecutado.DataLabel.ShowValue = true;
            //serieEjecutado.DataLabel.Position = eLabelPosition.OutEnd;
            //serieEjecutado.DataLabel.

            //serieProgramado.DataLabel. = "";
            //serieProgramado.DataLabel.ShowValue = true;

            serieEjecutado.Header = ws.Cells[filaTitCostoEjecutado, coluTitCostoEjecutado].Value.ToString();
            serieProgramado.Header = ws.Cells[filaTitCostoProgramado, coluTitCostoProgramado].Value.ToString();

            BiColumnaChart.Title.Text = titulo;
            //BiColumnaChart.DataLabel.ShowLeaderLines = true;
            BiColumnaChart.YAxis.Title.Text = yAxisTitle;

            BiColumnaChart.Legend.Position = eLegendPosition.Bottom;
        }

        #endregion

        // 3.13.2.34. Calificación de la Operacion de las Unidades de Generación
        #region REPORTE CALIFICACION OPERACION UNIDADES

        /// <summary>
        /// Genera vista html del reporte Horas orden APIS
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteCalificacionHoraOperacionHtml(List<EveHoraoperacionDTO> data, DateTime fechaInicio, DateTime fechaFin, List<EveHoraoperacionDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            List<EqFamiliaDTO> listaTipoCentral = data.GroupBy(x => new { FamCodi = x.Famcodi, FamNomb = x.Famnomb }).Select(x => new EqFamiliaDTO() { Famcodi = x.Key.FamCodi, Famnomb = x.Key.FamNomb }).ToList();
            bool tieneVarioTC = listaTipoCentral.Count >= 2;
            string descModoGrupo = listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null && listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación - Grupo" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null
                ? "" : string.Empty;

            #region cabecera

            strHtml.Append("<table id='tablaHOP' class='pretty tabla-adicional' width='100%'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>Empresa</th>");
            if (tieneVarioTC)
            {
                strHtml.Append("<th style=''>Tipo Central</th>");
            }
            strHtml.Append("<th style=''>Central</th>");
            strHtml.Append("<th style=''>Grupo</th>");
            if (descModoGrupo != string.Empty)
            {
                strHtml.AppendFormat("<th style=''>{0}</th>", descModoGrupo);
            }
            strHtml.Append("<th style=''>Inicio</th>");
            strHtml.Append("<th style=''>Final</th>");
            strHtml.Append("<th style=''>O. Arranque</th>");
            strHtml.Append("<th style=''>O. Parada</th>");
            strHtml.Append("<th style=''>Tipo Operación</th>");
            strHtml.Append("<th style=''>Ensayo de <br/>Potencia Efectiva</th>");
            strHtml.Append("<th style=''>Ensayo de <br/>Potencia Mínima</th>");
            strHtml.Append("<th style=''>Sistema</th>");
            strHtml.Append("<th style=''>Lim. Transm.</th>");
            strHtml.Append("<th style=''>Causa</th>");
            strHtml.Append("<th style=''>Observación</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var obj in data)
            {
                var datVersi = dataVersion.Find(x => x.Hophorini == obj.Hophorini);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='text-align: left;height: 25px;'>{0}</td>", obj.Emprnomb);
                if (tieneVarioTC)
                {
                    strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Famnomb.Trim());
                }
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.PadreNombre);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Equiabrev);
                if (descModoGrupo != string.Empty)
                {
                    strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.EquipoNombre);
                }
                strHtml.AppendFormat("<td>{0}</td>", obj.HophoriniDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorfinDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorordarranqDesc);
                strHtml.AppendFormat("<td>{0}</td>", obj.HophorparadaDesc);
                strHtml.AppendFormat("<td style='background-color:{1};color: black;text-align: center;font-weight: bold;'>{0}</td>", obj.Subcausadesc, obj.Subcausacolor);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopensayopeDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopensayopminDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopsaisladoDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HoplimtransDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.HopcausacodiDesc);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", obj.Hopdesc);
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            #endregion

            return strHtml.ToString();
        }

        #endregion

        // 3.13.2.35.	Registro de las congestiones del Sistema de Transmisión.
        #region REPORTE REGISTRO CONGESTIONES ST

        /// <summary>
        /// Reporte Registro Congestiones ST
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <returns></returns>
        public static string ReporteRegistroCongestionesSTHtml(List<EveIeodcuadroDTO> data, List<EveIeodcuadroDTO> dataVersion)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: 100%;' >");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:150px;'>Ubicación</th>");
            strHtml.Append("<th style='width:150px;'>Empresa <br/>afectada</th>");
            strHtml.Append("<th style='width:70px;'>Instalación de <br/>transmisión <br/>afectada</th>");
            strHtml.Append("<th style='width:100px;'>Inicio</th>");
            strHtml.Append("<th style='width:100px;'>Final</th>");
            strHtml.Append("<th style='width:150px;'>Unidades <br/>generadores <br/>limitadas</th>");
            strHtml.Append("<th style='width:300px;'>Observaciones</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            foreach (var reg in data)
            {
                var datVersi = dataVersion.Find(x => x.Iccodi == reg.Iccodi);

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='width:150px; text-align: left; padding-left: 5px;'>{0}</td>", reg.Areanomb);
                strHtml.AppendFormat("<td style='width:150px;'>{0}</td>", reg.Emprnomb);
                strHtml.AppendFormat("<td style='width:70px;'>{0}</td>", reg.Equiabrev);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraIni);
                strHtml.AppendFormat("<td style='width:100px;'>{0}</td>", reg.HoraFin);

                var _descrip = reg.Icdescrip1;
                string _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip1) { _descrip = datVersi.Icdescrip1; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='width:150px; text-align: left; padding-left: 5px; background:" + _bground + "'>{0}</td>", _descrip);
                //strHtml.AppendFormat("<td>{0}</td>", reg.Icdescrip1));

                _descrip = reg.Icdescrip2;
                _bground = string.Empty;
                if (datVersi != null) { if (_descrip != datVersi.Icdescrip2) { _descrip = datVersi.Icdescrip2; _bground = "lightgreen"; } }
                strHtml.AppendFormat("<td style='width:300px; text-align: left; padding-left: 5px; background:" + _bground + "'>{0}</td>", _descrip);
                //strHtml.AppendFormat("<td>{0}</td>", reg.Icdescrip2));

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptRegistroCongestionesST(ExcelWorksheet ws, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<EveIeodcuadroDTO> lista, List<EveIeodcuadroDTO> listaVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniUbicacion = colIniNombreReporte;
            int colIniEmpresa = colIniUbicacion + 1;
            int colIniEquipo = colIniEmpresa + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniUnidLim = colIniFinal + 1;
            int colIniObs = colIniUnidLim + 1;
            int colFinNombreReporte = colIniObs;

            ws.Cells[rowIniCabecera, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniCabecera, colIniEmpresa].Value = "EMPRESA AFECTADA";
            ws.Cells[rowIniCabecera, colIniEquipo].Value = "INSTALACIÓN DE TRANSMISIÓN AFECTADA";
            ws.Cells[rowIniCabecera, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniCabecera, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniCabecera, colIniUnidLim].Value = "UNIDADES GENERADORAS LIMITADAS";
            ws.Cells[rowIniCabecera, colIniObs].Value = "OBSERVACIONES";

            //Nombre Reporte
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = "Registro de las congestiones del Sistema de Transmisión";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ConstantesPR5ReportesServicio.ColorInfSGI);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniNombreReporte, rowIniCabecera, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    ws.Cells[row, colIniUbicacion].Value = list.Areanomb;
                    ws.Cells[row, colIniEmpresa].Value = list.Emprnomb;
                    ws.Cells[row, colIniEquipo].Value = list.Equiabrev;
                    ws.Cells[row, colIniInicio].Value = list.HoraIni;
                    ws.Cells[row, colIniFinal].Value = list.HoraFin;
                    ws.Cells[row, colIniUnidLim].Value = list.Icdescrip1;
                    ws.Cells[row, colIniObs].Value = list.Icdescrip2;

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUbicacion, rowFinData, colIniEmpresa, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEquipo, rowFinData, colIniFinal, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUnidLim, rowFinData, colIniObs, "Izquierda");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 53;

            ws.Column(colIniUbicacion).Width = 37;
            ws.Column(colIniEmpresa).Width = 32;
            ws.Column(colIniEquipo).Width = 20;
            ws.Column(colIniInicio).Width = 23;
            ws.Column(colIniFinal).Width = 23;
            ws.Column(colIniUnidLim).Width = 48;
            ws.Column(colIniObs).Width = 68;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        // 3.13.2.36.	Registro de asignación de la RRPF y RRSF
        #region REGISTRO ASIGNACION RRPF Y RRSF

        /// <summary>
        /// ReporteAsignacionRRPFyRRSHtml
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        /// <returns></returns>
        public static string ReporteAsignacionRRPFyRRSHtml(DateTime fecha, string[][] lista, string[][] listaVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            string[] cabecera = lista[0];
            string[] titulos = lista[1];
            int numCol = cabecera.Length;
            int numFil = lista.Length;

            int padding = 20;
            int anchoTotal = (420 + (4 * padding)) + (numCol * (70 + padding));

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='width: {0}px;' >", anchoTotal);

            strHtml.Append("<thead>");
            #region cabecera
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:60px;'>{0}</th>", cabecera[0]);
            strHtml.AppendFormat("<th style='width:110px;'>{0}</th>", cabecera[2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", cabecera[3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", cabecera[4]);
            for (int i = 5; i < numCol; i += 3)
            {
                strHtml.AppendFormat("<th style='word-wrap: break-word; white-space: normal' colspan=3>{0}</th>", cabecera[i]);
            }

            strHtml.Append("</tr>");
            #endregion
            #region titulo
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width:60px;'>{0}</th>", titulos[0]);
            strHtml.AppendFormat("<th style='width:110px;'>{0}</th>", titulos[2]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", titulos[3]);
            strHtml.AppendFormat("<th style='width:100px;'>{0}</th>", titulos[4]);
            for (int i = 5; i < numCol; i++)
            {
                strHtml.AppendFormat("<th style='width:70px; word-wrap: break-word; white-space: normal'>{0}</th>", titulos[i]);
            }
            strHtml.Append("</tr>");
            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            var central = "";
            for (int m = 2; m < numFil; m++)
            {
                central = lista[m - 1][2];
                var style = "";
                var styleCell = "";
                if (central != lista[m][2])
                {
                    style = "background-color: rgb(255, 219, 164);";
                    styleCell = style;
                }
                else
                {
                    style = "background-color: rgb(215, 239, 239);";
                    styleCell = "background-color: #FFFFFF";
                }
                strHtml.Append("<tr>");
                for (int n = 0; n < numCol; n++)
                {
                    if ((m == numFil - 2) && n < 5) //comentarios y total
                    {
                        if (n == 0)
                        {
                            strHtml.AppendFormat("<th style='width:60px; text-align: right;padding-right: 10px;background: #7AAD47;color: #ffffff;cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:150px; text-align: right;padding-right: 10px;background: #7AAD47;color: #ffffff;cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:110px; text-align: right;padding-right: 10px;background: #7AAD47;color: #ffffff;cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:100px; text-align: right;padding-right: 10px;background: #7AAD47;color: #ffffff;cursor: pointer;'>{0}</th>", lista[m][n]);
                        }
                    }
                    else if (m == numFil - 1 && n < 5)
                    {
                        if (n == 0)
                        {
                            strHtml.AppendFormat("<th style='width:60px; text-align: right;padding-right: 10px;background: rgb(255, 235, 156);color: rgb(173, 101, 0);cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:150px; text-align: right;padding-right: 10px;background: rgb(255, 235, 156);color: rgb(173, 101, 0);cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:110px; text-align: right;padding-right: 10px;background: rgb(255, 235, 156);color: rgb(173, 101, 0);cursor: pointer;'></th>");
                            strHtml.AppendFormat("<th style='width:100px; text-align: right;padding-right: 10px;background: rgb(255, 235, 156);color: rgb(173, 101, 0);cursor: pointer;'>{0}</th>", lista[m][n]);
                        }
                    }
                    else
                    {
                        //formateo de columnas
                        if (n < 5)
                        {
                            if (n == 0) strHtml.AppendFormat("<td style='width:60px; text-align: left;padding-left: 10px;{1}'>{0}</td>", lista[m][n], style);
                            if (n == 2) strHtml.AppendFormat("<td style='width:150px; text-align: left;padding-left: 10px;{1}'>{0}</td>", lista[m][n], style);
                            if (n == 3) strHtml.AppendFormat("<td style='width:110px; text-align: left;padding-left: 10px;{1}'>{0}</td>", lista[m][n], style);
                            if (n == 4) strHtml.AppendFormat("<td style='width:100px; text-align: left;padding-left: 10px;{1}'>{0}</td>", lista[m][n], style);
                        }
                        else
                        {
                            if (m == numFil - 1) //comentarios
                            {
                                strHtml.AppendFormat("<td style='width:81px; text-align: left;padding-left: 10px; word-wrap: break-word; white-space: normal; background-color: rgb(235, 235, 235)'>{0}</td>", lista[m][n]);
                            }
                            else if (m == numFil - 2)
                            {
                                strHtml.AppendFormat("<td style='width:81px; text-align: right;padding-right: 10px;'>{0}</td>", lista[m][n]);
                            }
                            else
                            {
                                strHtml.AppendFormat("<td style='width:81px; text-align: right;padding-right: 10px;{1}'>{0}</td>", lista[m][n], styleCell);
                            }
                        }
                    }
                }

                //saber si tiene registros de horas en la primera fila
                if (numCol == 4)
                {
                    strHtml.Append("<td style='width:70px; text-align: right;padding-right: 10px;'></td>");
                }

                strHtml.Append("</tr>");
            }
            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        /// <param name="magnitudRpf"></param>
        /// <param name="magnitudRpfVersion"></param>
        public static void GeneraRptAsignacionRRPFyRRSF(ExcelWorksheet ws, int filaIniTitulo, int coluIniTitulo, DateTime fecha1, DateTime fecha2, string[][] lista, string[][] listaVersion, decimal? magnitudRpf, decimal? magnitudRpfVersion)
        {
            string titulo = "Regulación primaria (RPF) y secundaria (RSF) de frecuencia";
            List<int> indices = new List<int>();

            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            int filPorcentaje = filaIniTitulo + 1;
            int filTexto = filPorcentaje + 1;
            int filaIniTabla = filTexto + 1;

            string strMagnitudRPF = magnitudRpf != null ? String.Format("{0:0.00}", magnitudRpf * 100) + "%" : string.Empty;
            ws.Cells[filPorcentaje, coluIniTitulo].Value = string.Format("El porcentaje de magnitud de Reserva Rotante para la RPF a todas las centrales del SEIN según el PR-21 es: {0}", strMagnitudRPF);
            ws.Cells[filTexto, coluIniTitulo].Value = string.Format("Las centrales que realizaron la Regulación Secundaria de Frecuencia fueron:");

            if (lista.Count() > 0)
            {
                string[] cabecera = lista[0];
                int numCol = cabecera.Length;
                int numFil = lista.Length;
                RsfAppServicio.GenerarTablaDatosRsf(ws, lista, numFil - 4, filaIniTabla, coluIniTitulo, 0);

                //aplicar estilos de Anexo A
                UtilExcel.SetFormatoCelda(ws, filaIniTabla, coluIniTitulo, filaIniTabla + 1, coluIniTitulo + numCol - 1, "Centro", "Centro", "#FFFFFF", ConstantesPR5ReportesServicio.ColorInfSGI, "Arial", 10, true);

                ws.View.FreezePanes(filaIniTabla + 2, (coluIniTitulo + 4 - 1) + 2);
            }

            ws.View.ZoomScale = 70;
        }

        #endregion

        #endregion

        #region INFORMACIÓN SOBRE INTERCAMBIOS DE ELECTRICIDAD

        // 3.13.2.37.	Registro de los flujos (MW y MVAr) cada 30 minutos de los enlaces internacionales.
        #region REPORTE REGISTRO FLUJOS ENLACES INTERNACIONALES

        /// <summary>
        /// Genera vista html del reporte de >Registro flujos enlaces internacionales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="data"></param>
        /// <param name="dataVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaHojaPtoVersion"></param>
        /// <returns></returns>
        public static string ReporteRegistroFlujosEIHtml(DateTime fechaInicio, List<MeMedicion48DTO> data, List<MeMedicion48DTO> dataVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaHojaPtoVersion)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = GenerarNumberFormatInfo3();

            #region CABECERA
            strHtml.Append("<table class='pretty tabla-icono' style='width:auto;' id='tabla'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th>SS.EE <br>{0}</th>", "");
            foreach (var reg in listaPto)
            {
                strHtml.AppendFormat("<th>{0}</th>", reg.Ptomedielenomb);
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 180px'>HORA<br>{0}</th>", "");
            foreach (var reg in listaPto)
            {
                strHtml.AppendFormat("<th style='width: 180px'>{0}<br/>{1}</th>", reg.Equinomb, reg.Tipoinfoabrev);
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region CUERPO
            strHtml.Append("<tbody>");

            for (int k = 1; k <= 48; k++)
            {
                var fecha = fechaInicio.AddMinutes(30 * k).ToString(ConstantesAppServicio.FormatoFechaHora);
                strHtml.AppendFormat("<tr><td class='tdbody_reporte'>{0}</td>", fecha);

                foreach (var reg in listaPto)
                {
                    var pto = data.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == fechaInicio && x.Tipoinfocodi == reg.Tipoinfocodi);
                    if (pto != null)
                    {
                        decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(pto, null);
                        if (valor != null)
                        {
                            strHtml.AppendFormat("<td>{0}</td>", ((decimal)valor).ToString("N", nfi));
                        }
                        else
                        {
                            strHtml.AppendFormat("<td></td>");
                        }
                    }
                    else
                    {
                        strHtml.AppendFormat("<td></td>");
                    }
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            #endregion

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniTitulo"></param>
        /// <param name="coluIniTitulo"></param>
        /// <param name="flagVisiblePtomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="data"></param>
        /// <param name="listaVersion"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoVersion"></param>
        public static void GeneraRptRegistroFlujosEI(ExcelWorksheet ws, int filaIniTitulo, int coluIniTitulo, bool flagVisiblePtomedicodi, DateTime fecha1, DateTime fecha2
            , List<MeMedicion48DTO> data, List<MeMedicion48DTO> listaVersion, List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoVersion)
        {
            #region Título

            ws.Cells[filaIniTitulo, coluIniTitulo].Value = "Registro de los Flujos Enlaces Internacionales";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo, "Arial", 18);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniTitulo, coluIniTitulo, filaIniTitulo, coluIniTitulo);

            #endregion

            #region cabecera

            int rowIniSSEE = filaIniTitulo + 1;
            int colIniSSEE = coluIniTitulo;
            int colFinSSEE = colIniSSEE + 1;
            int rowIniFecha = rowIniSSEE + 1;
            int colIniFecha = colIniSSEE;
            int colIniHora = colIniSSEE + 1;
            int colIniMW = colIniHora + 1;
            int colIniMVar = colIniMW + 1;

            ws.Cells[rowIniSSEE, colIniSSEE].Value = "SS.EE.";
            ws.Cells[rowIniSSEE, colIniSSEE, rowIniSSEE, colFinSSEE].Merge = true;
            ws.Cells[rowIniSSEE, colIniSSEE, rowIniSSEE, colFinSSEE].Style.WrapText = true;

            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
            ws.Cells[rowIniFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            ws.Cells[rowIniFecha, colIniHora].Value = "HORA";
            ws.Cells[rowIniFecha, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowIniFecha, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            foreach (var reg in listaPto)
            {
                ws.Cells[rowIniSSEE, colIniMW].Value = reg.Ptomedielenomb;
                ws.Cells[rowIniSSEE, colIniMW, rowIniSSEE, colIniMVar].Merge = true;
                ws.Cells[rowIniSSEE, colIniMW, rowIniSSEE, colIniMVar].Style.WrapText = true;

                ws.Cells[rowIniFecha, colIniMW].Value = reg.Equinomb + "\r\n" + "MW";
                ws.Cells[rowIniFecha, colIniMW].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniMVar].Value = reg.Equinomb + "\r\n" + "MVar";
                ws.Cells[rowIniFecha, colIniMVar].Style.WrapText = true;
            }

            int sizeFontHeader = 11;
            var colorHeader = ColorTranslator.FromHtml("#4F81BD");
            var colorBody = Color.White;

            using (ExcelRange range = ws.Cells[rowIniSSEE, colIniSSEE, rowIniFecha, colIniMVar])
            {
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Font.Size = sizeFontHeader;
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(Color.White);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(colorHeader);
            }

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//

            for (int i = 1; i <= 48; i++)
            {
                var rowCurrent = rowIniFecha + i;

                var wsCellFeccha = ws.Cells[rowCurrent, colIniFecha];
                wsCellFeccha.Value = fecha1.ToString(ConstantesAppServicio.FormatoFecha);
                wsCellFeccha.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                wsCellFeccha.Style.Font.Size = sizeFontHeader - 1;
                wsCellFeccha.Style.Fill.BackgroundColor.SetColor(colorHeader);
                wsCellFeccha.Style.Font.Color.SetColor(colorBody);
                wsCellFeccha.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsCellFeccha.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsCellFeccha.Style.Border.BorderAround(ExcelBorderStyle.Hair, ColorTranslator.FromHtml("#1F497D"));

                wsCellFeccha = ws.Cells[rowCurrent, colIniHora];
                wsCellFeccha.Value = fecha1.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoOnlyHora);
                wsCellFeccha.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                wsCellFeccha.Style.Font.Size = sizeFontHeader - 1;
                wsCellFeccha.Style.Fill.BackgroundColor.SetColor(colorHeader);
                wsCellFeccha.Style.Font.Color.SetColor(colorBody);
                wsCellFeccha.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsCellFeccha.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsCellFeccha.Style.Border.BorderAround(ExcelBorderStyle.Hair, ColorTranslator.FromHtml("#1F497D"));

                foreach (var reg in listaPto)
                {
                    var pto = data.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == fecha1 && x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW);

                    if (pto != null)
                    {
                        decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);
                        if (valor != null)
                        {
                            ws.Cells[rowCurrent, colIniMW].Value = valor;
                        }
                    }

                    //
                    pto = data.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == fecha1 && x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMVAR);
                    if (pto != null)
                    {
                        decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);
                        if (valor != null)
                        {
                            ws.Cells[rowCurrent, colIniMVar].Value = valor;
                        }
                    }

                    using (ExcelRange range = ws.Cells[rowCurrent, colIniMW, rowCurrent, colIniMVar])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Hair, ColorTranslator.FromHtml("#1F497D"));
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Font.Size = sizeFontHeader - 1;
                        range.Style.Fill.BackgroundColor.SetColor(colorBody);
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                }
            }
            #endregion

            ws.Column(colIniFecha).Width = 12;
            ws.Column(colIniHora).Width = 12;
            ws.Column(colIniMW).Width = 20;
            ws.Column(colIniMVar).Width = 20;

            ws.Row(rowIniFecha).Height = 36;

            //if (!flagVisiblePtomedicodi) ws.Row(rowIniPto).Hidden = true;

            ws.View.FreezePanes(rowIniFecha + 1, colIniHora + 1);

            ws.View.ZoomScale = 85;
        }

        /// <summary>
        /// GetGraficoInterconexionInternacionalWord
        /// </summary>
        /// <param name="tituloGrafico"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public static GraficoWeb GetGraficoInterconexionInternacionalWord(string tituloGrafico, List<MePtomedicionDTO> listaPto, List<MeMedicion48DTO> listaData)
        {
            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaPto.Count][];

            DateTime horas = DateTime.Today;
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            decimal? valor;
            RegistroSerie regSerie;
            List<DatosSerie> listadata;
            List<decimal> listaValor = new List<decimal>();
            for (int i = 0; i < listaPto.Count; i++)
            {
                var obj48XPto = listaData.FirstOrDefault(x => x.Ptomedicodi == listaPto[i].Ptomedicodi) ?? new MeMedicion48DTO();

                regSerie = new RegistroSerie();
                regSerie.Name = listaPto[i].Repptonomb;
                regSerie.Type = "column";

                listadata = new List<DatosSerie>();
                for (int j = 1; j <= 48; j++)
                {
                    valor = (decimal?)obj48XPto.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(obj48XPto, null);
                    listadata.Add(new DatosSerie() { Y = valor });
                    if (valor != null) listaValor.Add(valor.Value);
                }
                regSerie.Data = listadata;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = tituloGrafico;
            grafico.YaxixTitle = "MW";
            grafico.XAxisCategories = new List<string>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesYAxis = new List<int>();

            if (listaValor.Count() > 0)
            {
                grafico.YaxixMin = listaValor.Min(x => x);
                grafico.YaxixMax = listaValor.Max(x => x);
            }

            if (grafico.YaxixMax < 70) grafico.YaxixMax = 80;
            grafico.YaxixMin = grafico.YaxixMin - 40;

            return grafico;
        }

        #endregion

        #endregion

        #region NOTAS

        #region OBSERVACION

        /// <summary>
        /// Genera vista html de reporte de eventos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="lista"></param>
        /// <param name="listaVersion"></param>
        public static void GeneraRptObservaciones(ExcelWorksheet ws, string titulo, int rowIni, int colIni, DateTime fecha1, DateTime fecha2, List<SiNotaDTO> lista, List<SiNotaDTO> listaVersion)
        {
            int row = rowIni;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = colIni;

            int rowIniCabecera = rowIniNombreReporte + 1;
            int colIniNro = colIniNombreReporte;
            int colIniNota = colIniNro + 1;

            ws.Cells[rowIniCabecera, colIniNro].Value = "N° ";
            ws.Cells[rowIniCabecera, colIniNota].Value = titulo;

            //Nombre Reporte
            int colFinNombreReporte = colIniNota;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = string.Empty;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte, "Arial", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colIniNombreReporte);

            #region Formato Cabecera

            //UtilExcel.CeldasExcelAgrupar(ws, rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte);
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte, "Arial", 11);

            UtilExcel.CeldasExcelWrapText(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte);
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniCabecera, colIniNro, rowIniCabecera, colFinNombreReporte, ColorTranslator.FromHtml(ConstantesPR5ReportesServicio.ColorInfSGI), Color.White);

            #endregion

            #endregion

            row = rowIniCabecera + 1;
            #region cuerpo

            if (lista.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in lista)
                {
                    ws.Row(row).Height = 45;

                    ws.Cells[row, colIniNro].Value = list.Sinotaorden;
                    ws.Cells[row, colIniNota].Value = list.Sinotadesc;

                    rowFinData = row;
                    row++;
                }

                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniNro, rowFinData, colIniNro, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniNota, rowFinData, colIniNota, "Izquierda");

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniNro, rowFinData, colIniNota, "Arriba");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniNro, rowFinData, colIniNota, "Arial", 10);

                UtilExcel.CeldasExcelWrapText(ws, rowIniData, colIniNro, rowFinData, colIniNota);

                UtilExcel.BorderCeldas2(ws, rowIniData, colIniNro, rowFinData, colIniNota);
            }
            #endregion

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniCabecera).Height = 35;

            ws.Column(colIniNro).Width = 18;
            ws.Column(colIniNota).Width = 100;

            ws.View.FreezePanes(rowIniCabecera + 1, 1);
            ws.View.ZoomScale = 80;
        }

        #endregion

        #endregion

        #endregion

        #region Exportación de Archivo Excel - Anexo A

        /// <summary>
        /// ExcelCabGeneral
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="rowIni"></param>
        /// <param name="colFin"></param>
        public static void ExcelCabGeneral(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, DateTime fecha1, DateTime fecha2, int rowIni, int colFin)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            //Fecha
            ws.Cells[rowIni + 2, colFin].Value = GetDescripcionFiltroFechaAnexoA(fecha1, fecha2);
            ws.Cells[rowIni + 2, colFin].Style.Font.SetFromFont(new Font("Arial", 14));
            ws.Cells[rowIni + 2, colFin, rowIni + 2, colFin + 5].Merge = true;

            ws.Row(rowIni + 2).Height = 69;
            ws.Row(rowIni + 3).Height = 0;
            ws.Column(1).Width = 2;
            ws.Column(2).Width = 2;
            ws.Column(3).Width = 2;
            ws.Column(4).Width = 2;
        }

        /// <summary>
        /// ExcelFooterGeneral
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="rowIniHeader"></param>
        /// <param name="colIniHeader"></param>
        /// <param name="incluirLogo"></param>
        public static void ExcelFooterGeneral(ref ExcelWorksheet ws, ExcelPackage xlPackage, string pathLogo, int rowIniHeader, int colIniHeader, bool incluirLogo = true)
        {
            if (ws != null)
            {
                if (incluirLogo)
                {
                    //Logo
                    UtilExcel.AddImageLocalAlto4Filas(ws, 1, 0, pathLogo); //AddImage(ws, pathLogo, rowIniHeader, colIniHeader);
                }

                //No mostrar lineas
                ws.View.ShowGridLines = false;

                //Todo el excel con Font Arial
                var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Name = "Arial";

                xlPackage.Save();
            }
        }

        #endregion

        #region Generación de Archivo Excel - Útil

        /// <summary>
        /// Descripción del filtro de fecha
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public static string GetDescripcionFiltroFechaAnexoA(DateTime fecha1, DateTime fecha2)
        {
            if (fecha1.Date == fecha2.Date)
                return fecha2.ToString("dddd", new CultureInfo("es-PE")) + ", " + fecha2.Day + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")) + " del " + fecha2.Year;
            else
                return fecha1.ToString("dddd", new CultureInfo("es-PE")) + ", " + fecha1.Day + " de " + fecha1.ToString("MMMM", new CultureInfo("es-Pe")) + " del " + fecha1.Year
                    + " - " +
                    fecha2.ToString("dddd", new CultureInfo("es-PE")) + ", " + fecha2.Day + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")) + " del " + fecha2.Year;
        }

        /// <summary>
        /// Descripción del filtro de fecha
        /// </summary>
        /// <param name="fecha1">Fecha Inicio de la semana 1</param>
        /// <param name="fecha2">Fecha fin de la semana 2</param>
        /// <returns></returns>
        public static string GetDescripcionFiltroFechaInformeSemanal(DateTime fecha1, DateTime fecha2)
        {
            string descDia = GetDescripcionPeriodoSemanal(fecha1, fecha2);
            string descSemOp = GetNumeroSemanaPeriodoSemanal(ConstantesPR5ReportesServicio.ReptipcodiInformeSemanal, fecha1, fecha2);

            return "Semana Operativa <b>" + descSemOp + "</b>: del " + descDia; //PERIODO: SEMANA OPERATIVA 36: DEL 02 AL 08 DE SETIEMBRE            
        }

        /// <summary>
        /// Descripción del filtro de fecha
        /// </summary>
        /// <param name="fecha1">Fecha Inicio</param>
        /// <param name="fecha2">Fecha fin</param>
        /// <returns></returns>
        public static string GetDescripcionFiltroFechaEjecutivoSemanal(DateTime fecha1, DateTime fecha2)
        {
            string descDia = GetDescripcionPeriodoSemanal(fecha1, fecha2);
            string descSemOp = GetNumeroSemanaPeriodoSemanal(ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal, fecha1, fecha2);

            return "Semana N° <b>" + descSemOp + "</b>: del " + descDia; //PERIODO: SEMANA OPERATIVA 36-(37): DEL 02 AL 08 DE SETIEMBRE            
        }

        /// <summary>
        /// Obtener la siguiente estructura: 26 de agosto al 02 de setiembre de 2017 
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public static string GetDescripcionPeriodoSemanal(DateTime fecha1, DateTime fecha2)
        {
            string descDia;
            if (fecha1.Year != fecha2.Year)
            {
                descDia = fecha1.Day.ToString("00") + " de " + fecha1.ToString("MMMM", new CultureInfo("es-Pe")) + " de " + fecha1.Year
                    + " al " + fecha2.Day.ToString("00") + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")) + " de " + fecha2.Year;
            }
            else
            {
                string anio1 = fecha1.Year.ToString();
                string anio2 = fecha2.Year.ToString();
                anio1 = string.Empty;
                anio2 = " de " + anio2;

                descDia = fecha1.Month != fecha2.Month ? fecha1.Day.ToString("00") + " de " + fecha1.ToString("MMMM", new CultureInfo("es-Pe")) + anio1
                                                      + " al " + fecha2.Day.ToString("00") + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe"))
                                             : fecha1.Day.ToString("00") + " al " + fecha2.Day.ToString("00") + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")) + anio2;
            }

            return descDia;
        }

        /// <summary>
        /// Obtener la siguiente estructura: sábado 26 de agosto al sábado 02 de setiembre de 2017 
        /// </summary>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public static string GetDescripcionPeriodoSemanalConNombreDia(DateTime fecha1, DateTime fecha2)
        {
            string anio1 = fecha1.Year.ToString();
            string anio2 = fecha2.Year.ToString();
            anio1 = anio1 != anio2 ? " de " + anio1 : string.Empty;
            anio2 = " de " + anio2;

            string descDia = fecha1.Month != fecha2.Month ? fecha1.ToString("dddd", new CultureInfo("es-PE")) + " " + fecha1.Day.ToString("00") + " de " + fecha1.ToString("MMMM", new CultureInfo("es-Pe")).ToLower() + anio1
                                                        + " al " + fecha2.ToString("dddd", new CultureInfo("es-PE")) + " " + fecha2.Day.ToString("00") + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")).ToLower() + anio2
                                               : fecha1.ToString("dddd", new CultureInfo("es-PE")) + " " + fecha1.Day.ToString("00") + " al " + fecha2.ToString("dddd", new CultureInfo("es-PE")) + " " + fecha2.Day.ToString("00") + " de " + fecha2.ToString("MMMM", new CultureInfo("es-Pe")).ToLower() + anio2;

            return descDia;
        }

        /// <summary>
        /// GetNumeroSemanaPeriodoSemanal
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public static string GetNumeroSemanaPeriodoSemanal(int tipoReporte, DateTime fecha1, DateTime fecha2)
        {
            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(fecha1);
            Tuple<int, int> anioSemFin = EPDate.f_numerosemana_y_anho(fecha2);

            string strSemIni = anioSemIni.Item1.ToString("00");
            string strSemFin = anioSemFin.Item1.ToString("00");

            string descSemOp;
            if (ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal == tipoReporte)
            {
                if (anioSemIni.Item2 == anioSemFin.Item2) //compara años
                {
                    //compara semanas
                    descSemOp = strSemIni != strSemFin ? strSemIni + "(" + strSemFin + ")" : strSemIni;
                    descSemOp += "-" + anioSemIni.Item2;
                }
                else
                {
                    descSemOp = strSemIni + "(" + anioSemIni.Item2 + ")" + " - " + strSemFin + " (" + anioSemFin.Item2 + ")";
                }
            }
            else
            {
                descSemOp = fecha1.Date.AddDays(6) == fecha2.Date ? (strSemIni + "-" + anioSemIni.Item2)  //el filtro es la misma semana
                              : (strSemIni + "-" + anioSemIni.Item2) + " - " + (strSemFin + "-" + anioSemFin.Item2);
            }

            return descSemOp;
        }

        /// <summary>
        /// GetNumeroSemanaPeriodoSemanalSinAnio
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <returns></returns>
        public static string GetNumeroSemanaPeriodoSemanalSinAnio(int tipoReporte, DateTime fecha1, DateTime fecha2)
        {
            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(fecha1);
            Tuple<int, int> anioSemFin = EPDate.f_numerosemana_y_anho(fecha2);

            string strSemIni = anioSemIni.Item1.ToString("00");
            string strSemFin = anioSemFin.Item1.ToString("00");

            string descSemOp;
            if (ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal == tipoReporte)
            {
                //compara semanas
                descSemOp = strSemIni != strSemFin ? strSemIni + " - (" + strSemFin + ")" : strSemIni;
            }
            else
            {
                descSemOp = fecha1.Date.AddDays(6) == fecha2.Date ? strSemIni   //el filtro es la misma semana
                              : strSemIni + " - " + strSemFin;
            }

            return descSemOp;
        }

        /// <summary>
        /// Devuelve solo el reporte que se requiere (usado en exportaciones individuales)
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="idSheet"></param>
        /// <param name="numSheetExportados"></param>
        public static void EscogerSheetAExportar(ExcelPackage xlPackage, int idSheet, int numSheetExportados)
        {
            int numSheetTotales = xlPackage.Workbook.Worksheets.Count;
            //Elimino hasta llegar al sheet a exportar
            for (int i = 1; i < idSheet; i++)
            {
                xlPackage.Workbook.Worksheets.Delete(1);
                numSheetTotales--;
            }

            //Elimino los siguientes 
            for (int i = 1; i <= numSheetTotales - numSheetExportados; i++)
            {
                xlPackage.Workbook.Worksheets.Delete(1 + numSheetExportados);
            }
        }

        /// <summary>
        /// Devuelve solo el reporte que se requiere (usado en exportaciones individuales)
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="idSheet"></param>
        /// <param name="numSheetExportados"></param>
        public static void EscogerSheetAExportarEventos(ExcelPackage xlPackage, int idSheet, int numSheetExportados)
        {
            int numSheetTotales = xlPackage.Workbook.Worksheets.Count;
            //Elimino hasta llegar al sheet a exportar
            for (int i = 1; i < idSheet; i++)
            {
                xlPackage.Workbook.Worksheets.Delete(1);
                numSheetTotales--;
            }


        }

        /// <summary>
        /// Setea el titulo del Todo el reporte informe semanal
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="objFecha"></param>
        public static void IngresarEncabezadoGeneral(ExcelWorksheet ws, FechasPR5 objFecha)
        {
            int tipoDoc = objFecha.TipoReporte;
            string tipoVistaReporte = objFecha.TipoVistaReporte;

            string tituloGeneral = "";
            //if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual) tituloGeneral = "TITULO INFORME SEMANAL - INDIVIDUAL";
            //if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal) tituloGeneral = "TITULO INFORME SEMANAL - GRUPAL";

            if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal)
            {
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual) tituloGeneral = "";
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal) tituloGeneral = "";
            }
            if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiInformeSemanal)
            {
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual) tituloGeneral = "";
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal) tituloGeneral = "";
            }
            if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiInformeMensual)
            {
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual) tituloGeneral = "";
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal) tituloGeneral = "";
            }
            if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiInformeAnual)
            {
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual) tituloGeneral = "";
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal) tituloGeneral = "";
            }

            ws.Cells[1, 4].Value = tituloGeneral;
        }

        /// <summary>
        /// Selecciona el texto de la anotacion segun el item del reporte, el orden de la anotacion y el tipo de la exportacion
        /// </summary>
        /// <param name="itemReporte"></param>
        /// <param name="ordenAnotacion"></param>
        /// <param name="tipoVistaReporte"></param>
        /// <param name="tipoDoc"></param>
        /// <returns></returns>
        public static string EscogerAnotacion(string itemReporte, int ordenAnotacion, string tipoVistaReporte, int tipoDoc)
        {
            string texto = "";
            if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiInformeSemanal)
            {
                if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual)
                {

                    switch (itemReporte)
                    {
                        case "1.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_1p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_1p1;
                            break;
                        case "1.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_1p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_1p2;
                            break;
                        case "2.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_2p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_2p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_2p1;
                            break;
                        case "2.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_2p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_2p2;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_2p2;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_2p2;
                            if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.I_Grafico4_Reporte_2p2;
                            break;
                        case "2.3":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_2p3;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_2p3;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_2p3;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_2p3;
                            break;
                        case "2.4":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_2p4;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_2p4;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_2p4;
                            break;
                        case "2.5":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_2p5;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_2p5;
                            break;
                        case "3.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_3p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_3p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_3p1;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_3p1;
                            if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.I_Grafico4_Reporte_3p1;
                            break;
                        case "3.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_3p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_3p2;
                            break;
                        case "3.3":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_3p3;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_3p3;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_3p3;
                            break;
                        case "4.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_4p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_4p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_4p1;
                            break;
                        case "4.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_4p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_4p2;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_4p2;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico4_Reporte_4p2;
                            break;
                        case "4.3":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_4p3;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_4p3;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_4p3;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_4p3;
                            break;
                        case "5.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_5p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_5p2;
                            break;
                        case "5.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_5p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_5p2;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_5p2;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico4_Reporte_5p2;
                            if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.I_Grafico5_Reporte_5p2;
                            if (ordenAnotacion == 6) texto = NotasPieExcelInformeSemanal.I_Grafico6_Reporte_5p2;
                            if (ordenAnotacion == 7) texto = NotasPieExcelInformeSemanal.I_Grafico7_Reporte_5p2;
                            if (ordenAnotacion == 8) texto = NotasPieExcelInformeSemanal.I_Grafico8_Reporte_5p2;
                            if (ordenAnotacion == 9) texto = NotasPieExcelInformeSemanal.I_Grafico9_Reporte_5p2;
                            if (ordenAnotacion == 10) texto = NotasPieExcelInformeSemanal.I_Grafico10_Reporte_5p2;
                            if (ordenAnotacion == 11) texto = NotasPieExcelInformeSemanal.I_Grafico11_Reporte_5p2;
                            if (ordenAnotacion == 12) texto = NotasPieExcelInformeSemanal.I_Grafico12_Reporte_5p2;
                            if (ordenAnotacion == 13) texto = NotasPieExcelInformeSemanal.I_Grafico13_Reporte_5p2;
                            break;
                        case "5.3":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_5p3;
                            break;
                        case "5.4":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_5p4;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_5p4;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_5p4;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico4_Reporte_5p4;
                            if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.I_Grafico5_Reporte_5p4;
                            if (ordenAnotacion == 6) texto = NotasPieExcelInformeSemanal.I_Grafico6_Reporte_5p4;
                            if (ordenAnotacion == 7) texto = NotasPieExcelInformeSemanal.I_Grafico7_Reporte_5p4;
                            if (ordenAnotacion == 8) texto = NotasPieExcelInformeSemanal.I_Grafico8_Reporte_5p4;
                            break;
                        case "6.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_6p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_6p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_6p1;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_6p1;
                            break;
                        case "7.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_7p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_7p1;
                            break;
                        case "8.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_8p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_8p1;
                            break;
                        case "8.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_8p2;
                            break;
                        case "9.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_9p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_9p1;
                            break;
                        case "9.2":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_9p2;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_9p2;
                            break;
                        case "9.3":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_9p3;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_9p3;
                            break;
                        case "10.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_10p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_10p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_10p1;
                            break;
                        case "11.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_11p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_11p1;
                            break;
                        case "13.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_13p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_13p1;
                            break;
                        case "14.1":
                            if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.I_Cuadro1_Reporte_14p1;
                            if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.I_Grafico1_Reporte_14p1;
                            if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.I_Grafico2_Reporte_14p1;
                            if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.I_Grafico3_Reporte_14p1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal)
                    {
                        switch (itemReporte)
                        {
                            case "1.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_1p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_1p1;
                                break;
                            case "1.2":
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_1p2;
                                break;
                            case "2.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_2p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_2p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_2p1;
                                break;
                            case "2.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_2p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_2p2;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_2p2;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_2p2;
                                if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.G_Grafico4_Reporte_2p2;
                                break;
                            case "2.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_2p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_2p3;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_2p3;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_2p3;
                                break;
                            case "2.4":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_2p4;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_2p4;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_2p4;
                                break;
                            case "2.5":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_2p5;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_2p5;
                                break;
                            case "3.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_3p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_3p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_3p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_3p1;
                                if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.G_Grafico4_Reporte_3p1;
                                break;
                            case "3.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_3p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_3p2;
                                break;
                            case "3.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_3p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_3p3;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_3p3;
                                break;
                            case "4.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_4p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_4p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_4p1;
                                break;
                            case "4.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_4p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_4p2;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_4p2;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico4_Reporte_4p2;
                                break;
                            case "4.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_4p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_4p3;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_4p3;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_4p3;
                                break;
                            case "5.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_5p1;
                                break;
                            case "5.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_5p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_5p2;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_5p2;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico4_Reporte_5p2;
                                if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.G_Grafico5_Reporte_5p2;
                                if (ordenAnotacion == 6) texto = NotasPieExcelInformeSemanal.G_Grafico6_Reporte_5p2;
                                if (ordenAnotacion == 7) texto = NotasPieExcelInformeSemanal.G_Grafico7_Reporte_5p2;
                                if (ordenAnotacion == 8) texto = NotasPieExcelInformeSemanal.G_Grafico8_Reporte_5p2;
                                if (ordenAnotacion == 9) texto = NotasPieExcelInformeSemanal.G_Grafico9_Reporte_5p2;
                                if (ordenAnotacion == 10) texto = NotasPieExcelInformeSemanal.G_Grafico10_Reporte_5p2;
                                if (ordenAnotacion == 11) texto = NotasPieExcelInformeSemanal.G_Grafico11_Reporte_5p2;
                                if (ordenAnotacion == 12) texto = NotasPieExcelInformeSemanal.G_Grafico12_Reporte_5p2;
                                if (ordenAnotacion == 13) texto = NotasPieExcelInformeSemanal.G_Grafico13_Reporte_5p2;
                                break;
                            case "5.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_5p3;
                                break;
                            case "5.4":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_5p4;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_5p4;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_5p4;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico4_Reporte_5p4;
                                if (ordenAnotacion == 5) texto = NotasPieExcelInformeSemanal.G_Grafico5_Reporte_5p4;
                                if (ordenAnotacion == 6) texto = NotasPieExcelInformeSemanal.G_Grafico6_Reporte_5p4;
                                if (ordenAnotacion == 7) texto = NotasPieExcelInformeSemanal.G_Grafico7_Reporte_5p4;
                                if (ordenAnotacion == 8) texto = NotasPieExcelInformeSemanal.G_Grafico8_Reporte_5p4;
                                break;
                            case "6.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_6p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_6p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_6p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_6p1;
                                break;
                            case "7.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_7p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_7p1;
                                break;
                            case "8.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_8p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_8p1;
                                break;
                            case "8.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_8p2;
                                break;
                            case "9.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_9p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_9p1;
                                break;
                            case "9.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_9p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_9p2;
                                break;
                            case "9.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_9p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_9p3;
                                break;
                            case "10.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_10p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_10p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_10p1;
                                break;
                            case "11.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_11p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_11p1;
                                break;
                            case "13.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_13p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_13p1;
                                break;
                            case "14.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelInformeSemanal.G_Cuadro1_Reporte_14p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelInformeSemanal.G_Grafico1_Reporte_14p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelInformeSemanal.G_Grafico2_Reporte_14p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelInformeSemanal.G_Grafico3_Reporte_14p1;
                                break;
                            default:
                                break;
                        }
                    }

                }
            }
            else
            {
                if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiEjecutivoSemanal)
                {
                    if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual)
                    {
                        switch (itemReporte)
                        {
                            case "1.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_1p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_1p1;
                                break;
                            case "1.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_1p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_1p2;
                                break;


                            case "2.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_2p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_2p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_2p1;
                                break;

                            case "2.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_2p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_2p2;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_2p2;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_2p2;
                                break;

                            case "2.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_2p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_2p3;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_2p3;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_2p3;
                                break;
                            case "2.4":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_2p4;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_2p4;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_2p4;
                                break;
                            case "2.5":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_2p5;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_2p5;
                                break;


                            case "3.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_3p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_3p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_3p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_3p1;
                                if (ordenAnotacion == 5) texto = NotasPieExcelEjecutivoSemanal.I_Grafico4_Reporte_3p1;
                                break;

                            case "3.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_3p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_3p2;
                                break;

                            case "3.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_3p3;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_3p3;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_3p3;
                                break;


                            case "4.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_4p1;
                                break;

                            case "4.2":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_4p2;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_4p2;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_4p2;
                                break;

                            case "4.3":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_4p3;
                                break;

                            case "4.4":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_4p4;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_4p4;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_4p4;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico4_Reporte_4p4;
                                break;

                            case "5.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_5p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_5p1;
                                break;


                            case "6.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_6p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_6p1;
                                break;

                            case "7.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_7p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_7p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_7p1;
                                break;

                            case "8.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_8p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_8p1;
                                break;

                            case "9.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_9p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_9p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_9p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_9p1;
                                break;


                            case "10.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_10p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_10p1;
                                break;

                            case "11.1":
                                if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.I_Cuadro1_Reporte_11p1;
                                if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.I_Grafico1_Reporte_11p1;
                                if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.I_Grafico2_Reporte_11p1;
                                if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.I_Grafico3_Reporte_11p1;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal)
                        {
                            switch (itemReporte)
                            {
                                case "1.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_1p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_1p1;
                                    break;
                                case "1.2":
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_1p2;
                                    break;

                                case "2.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_2p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_2p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_2p1;
                                    break;

                                case "2.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_2p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_2p2;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_2p2;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_2p2;
                                    break;

                                case "2.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_2p3;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_2p3;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_2p3;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_2p3;
                                    break;
                                case "2.4":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_2p4;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_2p4;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_2p4;
                                    break;
                                case "2.5":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_2p5;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_2p5;
                                    break;


                                case "3.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_3p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_3p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_3p1;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_3p1;
                                    if (ordenAnotacion == 5) texto = NotasPieExcelEjecutivoSemanal.G_Grafico4_Reporte_3p1;
                                    break;

                                case "3.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_3p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_3p2;
                                    break;

                                case "3.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_3p3;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_3p3;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_3p3;
                                    break;


                                case "4.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_4p1;
                                    break;

                                case "4.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_4p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_4p2;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_4p2;
                                    break;

                                case "4.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_4p3;
                                    break;

                                case "4.4":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_4p4;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_4p4;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_4p4;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico4_Reporte_4p4;
                                    break;

                                case "5.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_5p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_5p1;
                                    break;


                                case "6.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_6p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_6p1;
                                    break;

                                case "7.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_7p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_7p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_7p1;
                                    break;

                                case "8.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_8p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_8p1;
                                    break;

                                case "9.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_9p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_9p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_9p1;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_9p1;
                                    break;


                                case "10.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_10p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_10p1;
                                    break;

                                case "11.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelEjecutivoSemanal.G_Cuadro1_Reporte_11p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelEjecutivoSemanal.G_Grafico1_Reporte_11p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelEjecutivoSemanal.G_Grafico2_Reporte_11p1;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelEjecutivoSemanal.G_Grafico3_Reporte_11p1;
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
                else
                {
                    if (tipoDoc == ConstantesPR5ReportesServicio.ReptipcodiInformeMensual)
                    {
                        if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaIndividual)
                        {

                            switch (itemReporte)
                            {
                                case "1.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_1p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_1p1;
                                    break;
                                case "1.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_1p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_1p2;
                                    break;
                                case "1.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_1p3;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_1p3;
                                    break;
                                case "2.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_2p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_2p1;
                                    break;
                                case "2.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_2p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_2p2;
                                    break;
                                case "2.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_2p3;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_2p3;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_2p3;
                                    break;
                                case "2.4":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_2p4;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_2p4;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_2p4;
                                    break;
                                case "2.5":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_2p5;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_2p5;
                                    break;
                                case "3.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_3p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_3p1;
                                    break;
                                case "3.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_3p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_3p2;
                                    break;

                                case "4.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_4p1;
                                    break;

                                case "4.2":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_4p2;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_4p2;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Grafico3_Reporte_4p2;
                                    break;

                                case "4.3":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_4p3;

                                    break;

                                case "4.4":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_4p4;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_4p4;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Grafico3_Reporte_4p4;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.I_Grafico4_Reporte_4p4;
                                    break;

                                case "5.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_5p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_5p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Cuadro2_Reporte_5p1;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_5p1;
                                    if (ordenAnotacion == 5) texto = NotasPieExcelInformeMensual.I_Cuadro3_Reporte_5p1;
                                    if (ordenAnotacion == 6) texto = NotasPieExcelInformeMensual.I_Grafico3_Reporte_5p1;
                                    break;

                                case "6.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_6p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_6p1;
                                    break;

                                case "7.1":
                                    if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.I_Cuadro1_Reporte_7p1;
                                    if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.I_Grafico1_Reporte_7p1;
                                    if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.I_Grafico2_Reporte_7p1;
                                    if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.I_Grafico3_Reporte_7p1;
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            if (tipoVistaReporte == ConstantesPR5ReportesServicio.TipoVistaGrupal)
                            {
                                switch (itemReporte)
                                {
                                    case "1.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_1p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_1p1;
                                        break;
                                    case "1.2":
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_1p2;
                                        break;
                                    case "1.3":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_1p3;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_1p3;
                                        break;
                                    case "2.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_2p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_2p1;
                                        break;
                                    case "2.2":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_2p2;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_2p2;
                                        break;
                                    case "2.3":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_2p3;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_2p3;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_2p3;
                                        break;
                                    case "2.4":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_2p4;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_2p4;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_2p4;
                                        break;
                                    case "2.5":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_2p5;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_2p5;
                                        break;
                                    case "3.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_3p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_3p1;
                                        break;
                                    case "3.2":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_3p2;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_3p2;
                                        break;

                                    case "4.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_4p1;
                                        break;
                                    case "4.2":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_4p2;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_4p2;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Grafico3_Reporte_4p2;
                                        break;
                                    case "4.3":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_4p3;
                                        break;
                                    case "4.4":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_4p4;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_4p4;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Grafico3_Reporte_4p4;
                                        if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.G_Grafico4_Reporte_4p4;
                                        break;

                                    case "5.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_5p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_5p1;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Cuadro2_Reporte_5p1;
                                        if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_5p1;
                                        if (ordenAnotacion == 5) texto = NotasPieExcelInformeMensual.G_Cuadro3_Reporte_5p1;
                                        if (ordenAnotacion == 6) texto = NotasPieExcelInformeMensual.G_Grafico3_Reporte_5p1;
                                        break;


                                    case "6.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_6p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_6p1;
                                        break;

                                    case "7.1":
                                        if (ordenAnotacion == 1) texto = NotasPieExcelInformeMensual.G_Cuadro1_Reporte_7p1;
                                        if (ordenAnotacion == 2) texto = NotasPieExcelInformeMensual.G_Grafico1_Reporte_7p1;
                                        if (ordenAnotacion == 3) texto = NotasPieExcelInformeMensual.G_Grafico2_Reporte_7p1;
                                        if (ordenAnotacion == 4) texto = NotasPieExcelInformeMensual.G_Grafico3_Reporte_7p1;
                                        break;

                                    default:
                                        break;
                                }
                            }

                        }
                    }
                }
            }

            return texto;
        }

        /// <summary>
        /// Devuelve el numero de semanas al que pertenece el rango de fechas que se le da por parametro
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        public static void ObtenerSemanasEnRango(DateTime fechaInicial, DateTime fechaFinal, out string n1, out string n2)
        {
            var semIni = EPDate.f_numerosemana_y_anho(fechaInicial);
            var semFin = EPDate.f_numerosemana_y_anho(fechaFinal);

            int nSemIni = semIni.Item1;
            int anioSemIni = semIni.Item2;
            int nSemFin = semFin.Item1;
            int anioSemFin = semFin.Item2;

            if (nSemIni == nSemFin && anioSemIni == anioSemFin)
            {
                n1 = nSemIni.ToString();
                n2 = anioSemIni.ToString();
            }
            else
            {
                if (anioSemIni == anioSemFin)
                {
                    n1 = nSemIni + "(" + nSemFin + ")";
                    n2 = anioSemFin.ToString();
                }
                else
                {
                    n1 = nSemIni + "(" + anioSemIni + ")";
                    n2 = nSemFin + "(" + anioSemFin + ")";
                }
            }
        }

        /// <summary>
        /// Quita todas las series del un ExcelChart
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static void EliminarAllSeriesDelGrafico(ExcelChart chart)
        {
            int numSeries = chart.Series.Count;
            for (int i = 0; i < numSeries; i++)
            {
                chart.Series.Delete(0);
            }
        }

        /// <summary>
        /// Quita todas las series del un ExcelChart
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static void EliminarTodasSeriesMenosUnaDelGrafico(ExcelChart chart)
        {
            int numSeries = chart.Series.Count;
            for (int i = 0; i < numSeries - 1; i++)
            {
                chart.Series.Delete(0);
            }
        }

        /// <summary>
        /// Devuelve la anotacion igual al parametro ingresado
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string CompletarAnotacion(string texto)
        {
            return texto;
        }

        /// <summary>
        /// Adiciona 1 parametro a la anotación previa
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public static string CompletarAnotacion(string texto, object param1)
        {
            return string.Format(texto, param1);
        }

        /// <summary>
        /// Adiciona los 2 parametros a la anotación previa
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static string CompletarAnotacion(string texto, object param1, object param2)
        {
            return string.Format(texto, param1, param2);
        }

        /// <summary>
        /// Adiciona los 3 parametros a la anotación previa
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <returns></returns>
        public static string CompletarAnotacion(string texto, object param1, object param2, object param3)
        {
            return string.Format(texto, param1, param2, param3);
        }

        /// <summary>
        /// Adiciona los 4 parametros a la anotación previa
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <returns></returns>
        public static string CompletarAnotacion(string texto, object param1, object param2, object param3, object param4)
        {
            return string.Format(texto, param1, param2, param3, param4);
        }

        /// <summary>
        /// formato para los subitulos en el reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        public static void FormatoSubtituloExcel(ExcelWorksheet ws, int rowIni, int colIni)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.SetFromFont(new Font("Calibri", 12));
                r1.Style.Font.Italic = false;
                r1.Style.Font.Bold = true;
            }
        }

        #endregion

        #region Generación de Versión IEOD

        /// <summary>
        /// Genera listado del menu
        /// </summary>
        /// <param name="listaItems"></param>
        /// <returns></returns>
        public static string ListaMenuHtml(List<SiMenureporteDTO> listaItems)
        {
            List<SiMenureporteDTO> listCat = listaItems.Where(z => z.Repcatecodi == -1).OrderBy(c => c.Reporden).ToList();
            List<SiMenureporteDTO> listMenu = listaItems.Where(z => z.Repstado == 1).OrderBy(c => c.Repcodi).ToList();

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table id='myTable' class='display' cellspacing='0' width='100%' heigth='100%'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='display:none'>Name</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            for (int x = 0; x < listCat.Count; x++)
            {
                var eject = listMenu.Where(c => c.Repcatecodi == listCat[x].Repcodi).OrderBy(m => m.Reporden).ToList();
                if (eject.Count > 0)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<th style='text-align:left'>" + listCat[x].Repdescripcion + "</th>");
                    strHtml.Append("</tr>");
                }

                foreach (var list in eject)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td id='repor_" + list.Repcodi + "' class='item_pr5' style='text-align:left; padding-left: 24px;'><a style='color:black' href='#' onclick='fnClick(\"" + list.Repabrev + "\")'>" + list.Repdescripcion + "</a></td>");
                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Genera vista Html del menu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ListaAdmReporteHtml(List<SiMenureporteDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='rep_pr5'>");

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:100px;'>DESCRIPCION</th>");
            strHtml.Append("<th style='width:100px;'>CATEGORIA</th>");
            strHtml.Append("<th style='width:100px;'>MODIFICACION</th>");
            strHtml.Append("<th style='width:100px;'>ESTADO</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='text-align:left'>{0}</td>", list.Repdescripcion);
                strHtml.AppendFormat("<td style='text-align:left'>{0}</td>", FactorySic.GetSiMenureporteRepository().GetById((int)list.Repcatecodi).Repdescripcion);
                strHtml.AppendFormat("<td>{0}</td>", list.Repfecmodificacion);
                strHtml.Append("<td><input type='checkbox' onclick='updItem(" + list.Repcodi + ");' " + ((list.Repstado.Equals(1)) ? "checked" : "") + " />" + ((list.Repstado.Equals(1)) ? "ACTIVO" : "INACTIVO") + "</td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        #endregion

        #region UTIL

        /// <summary>
        /// Flag que indica si el item tiene 2 filtros de fecha
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public static bool TieneFiltroDobleFechaAnexoA(int reporcodi)
        {
            switch (reporcodi)
            {
                case ConstantesAnexoAPR5.IndexReporteDemandaPorArea:
                case ConstantesAnexoAPR5.IndexReporteDemandaGrandesUsuarios:
                case ConstantesAnexoAPR5.IndexReporteRecursosEnergeticosDemandaSEIN:
                //case ConstantesAnexoAPR5.IndexReporteProduccionEnergiaDiaria:
                case ConstantesAnexoAPR5.IndexReporteGeneracionDelSEIN:

                case ConstantesAnexoAPR5.IndexReporteReservaFriaSistema:
                case ConstantesAnexoAPR5.IndexReporteCaudalesCentralHidroelectrica:
                case ConstantesAnexoAPR5.IndexReporteHorariosCaudalVolumenCentralHidroelectrica:
                case ConstantesAnexoAPR5.IndexReporteVertimientosPeriodoVolumen:
                //case ConstantesAnexoAPR5.IndexReporteConsumoYPresionDiarioUnidadTermoelectrica:
                //case ConstantesAnexoAPR5.IndexReporteRegistroEnergiaPrimaria30Unidades:
                //case ConstantesAnexoAPR5.IndexReporteCalorUtilGeneracionProceso:

                case ConstantesAnexoAPR5.IndexReportePALineasTransmision:
                case ConstantesAnexoAPR5.IndexReporteTensionBarrasSEIN:

                case ConstantesAnexoAPR5.IndexReporteVariacionesSostenidasSubitas:
                case ConstantesAnexoAPR5.IndexReporteSistemasAisladosTemporalesYVariacionesSostenidasSubitas:

                //case ConstantesAnexoAPR5.IndexReporteDesviacionesDemandaPronostico:
                case ConstantesAnexoAPR5.IndexReporteDesviacionesProduccionUG:

                case ConstantesAnexoAPR5.IndexReporteCostoMarginalesCortoPlazo:

                case ConstantesAnexoAPR5.IndexReporteCostoTotalOperacionEjecutada:
                case ConstantesAnexoAPR5.IndexReporteAsignacionRRPFyRRSF:

                case ConstantesAnexoAPR5.IndexReporteRegistroFlujosEnlacesInternacionales:

                case ConstantesAnexoAPR5.IndexObservacion:
                case ConstantesAnexoAPR5.IndexRecomendacionConclusion:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Variacion 
        /// </summary>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <returns></returns>
        public static decimal CalcularVariacionO100(decimal valor1, decimal valor2)
        {
            if (valor2 != 0)
                return ((valor1 - valor2) / valor2) * 100;

            return 100;
        }

        /// <summary>
        /// Participacion
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valorTotal"></param>
        /// <returns></returns>
        public static decimal CalcularParticipacionO0(decimal valor, decimal valorTotal)
        {
            if (valorTotal != 0)
                return (valor / valorTotal) * 100.0m;

            return 0;
        }

        /// <summary>
        /// formatea numero decimal a 3 decimales
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo3()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// formatea numero decimal a 2 decimales
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo2()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// formatea numero decimal a 1 decimales
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo1()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// formatea numero decimal a 0 decimales
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo0()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 0;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Determina si en las fechas existe un bisiesto
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static bool ExisteBisiestoEnRango(DateTime fechaIni, DateTime fechaFin)
        {
            if (DateTime.IsLeapYear(fechaIni.Year))
            {
                DateTime fbis = new DateTime(fechaIni.Year, 2, 29);
                if (fechaIni <= fbis && fbis <= fechaFin)
                {
                    return true;
                }
            }

            if (DateTime.IsLeapYear(fechaFin.Year))
            {
                DateTime fbis = new DateTime(fechaFin.Year, 2, 29);
                if (fechaIni <= fbis && fbis <= fechaFin)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Listar todas las fechas bisiestas en un rango determinado
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static List<DateTime> ListarFechasBisiestoEnRango(DateTime fechaIni, DateTime fechaFin)
        {
            List<DateTime> l = new List<DateTime>();
            int anioIni = fechaIni.Year;
            int anioFin = fechaFin.Year;

            for (var i = anioIni; i <= anioFin; i++)
            {
                if (DateTime.IsLeapYear(i))
                    l.Add(new DateTime(i, 2, 29));
            }

            return l;
        }

        /// <summary>
        /// Listar empresas a partir de data de m48
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="listaEmpresaBD"></param>
        /// <returns></returns>
        public static List<SiEmpresaDTO> ListarEmpresaFromM48(List<MeMedicion48DTO> listaData, List<SiEmpresaDTO> listaEmpresaBD)
        {
            var lista = listaData.GroupBy(x => new { x.Emprnomb, x.Emprcodi })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi }).ToList();

            foreach (var reg in lista)
            {
                reg.Emprnomb = (listaEmpresaBD.Find(x => x.Emprcodi == reg.Emprcodi)?.Emprnomb) ?? "";
                reg.Emprabrev = (listaEmpresaBD.Find(x => x.Emprcodi == reg.Emprcodi)?.Emprabrev) ?? "";
            }

            return lista;
        }

        /// <summary>
        /// Listar empresas a partir de data de m96
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="listaEmpresaBD"></param>
        /// <returns></returns>
        public static List<SiEmpresaDTO> ListarEmpresaFromM96(List<MeMedicion96DTO> listaData, List<SiEmpresaDTO> listaEmpresaBD)
        {
            var lista = listaData.GroupBy(x => new { x.Emprnomb, x.Emprcodi })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi }).ToList();

            foreach (var reg in lista)
            {
                reg.Emprnomb = (listaEmpresaBD.Find(x => x.Emprcodi == reg.Emprcodi)?.Emprnomb) ?? "";
                reg.Emprabrev = (listaEmpresaBD.Find(x => x.Emprcodi == reg.Emprcodi)?.Emprabrev) ?? "";
            }

            return lista;
        }

        /// <summary>
        /// Listar los puntos de medición de una data de medición 48
        /// </summary>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public static List<MePtomedicionDTO> ListarPtoMedicionFromM48(List<MeMedicion48DTO> listaData)
        {
            return listaData.GroupBy(x => new
            {
                x.Emprcodi,
                x.Equicodi,
                x.Equipadre,
                x.Grupocodi,
                x.Ptomedicodi,
                x.Grupointegrante
            })
                    .Select(x => new MePtomedicionDTO()
                    {
                        Grupointegrante = x.First().Grupointegrante ?? "N",
                        Tipogenerrer = x.First().Tipogenerrer,
                        Grupotipocogen = x.First().Grupotipocogen,
                        Emprcodi = x.Key.Emprcodi,
                        Emprnomb = x.First().Emprnomb,
                        Emprorden = x.First().Emprorden,
                        Equicodi = x.Key.Equicodi,
                        Equinomb = x.First().Equinomb,
                        Equiabrev = x.First().Equiabrev,
                        Equipadre = x.First().Equipadre,
                        Central = x.First().Central,
                        Grupocodi = x.Key.Grupocodi,
                        Gruponomb = x.First().Gruponomb,
                        Grupoorden = x.First().Grupoorden,
                        Ptomedicodi = x.Key.Ptomedicodi,
                        Ptomedibarranomb = x.First().Ptomedinomb,
                        Ptomedielenomb = x.First().Ptomedinomb,
                        AreaOperativa = x.First().AreaOperativa,
                        Tgenernomb = x.First().Tgenernomb,
                        Fenergcodi = x.First().Fenergcodi,
                        Tipoptomedicodi = x.First().Tipoptomedicodi
                    }).ToList();
        }

        /// <summary>
        /// Obtener empresas ordenadas segun orden
        /// </summary>
        /// <param name="listaPto"></param>
        /// <returns></returns>
        public static List<SiEmpresaDTO> ListarEmpresaFromPto(List<MePtomedicionDTO> listaPto)
        {
            return listaPto.OrderBy(x => x.Tipogenerrer).ThenBy(x => x.Grupotipocogen).ThenBy(x => x.Emprorden).ThenBy(x => x.Grupoorden)
                .GroupBy(x => new { x.Emprnomb, x.Emprcodi })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi.GetValueOrDefault(0), Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Listar grupos segun orden despacho
        /// </summary>
        /// <param name="listaPto"></param>
        /// <returns></returns>
        public static List<PrGrupoDTO> ListarGrupoFromPto(List<MePtomedicionDTO> listaPto)
        {
            return listaPto.OrderBy(x => x.Tipogenerrer).ThenBy(x => x.Grupoorden)
                .GroupBy(x => new { x.Grupocodi, x.Emprcodi, x.Gruponomb, x.Ptomedicodi })
                .Select(x => new PrGrupoDTO()
                {
                    Emprcodi = x.Key.Emprcodi.GetValueOrDefault(0),
                    Gruponomb = x.Key.Gruponomb,
                    Grupocodi = x.Key.Grupocodi.GetValueOrDefault(-1),
                    Ptomedicodi = x.Key.Ptomedicodi,
                    AreaOperativa = x.First().AreaOperativa,
                    Tgenernomb = x.First().Tgenernomb
                }).ToList();
        }

        /// <summary>
        /// Listar puntos desde hojapto
        /// </summary>
        /// <param name="listaHoja"></param>
        /// <returns></returns>
        public static List<MePtomedicionDTO> ListarPtoMedicionFromHojapto(List<MeHojaptomedDTO> listaHoja)
        {
            return listaHoja.GroupBy(x => new { x.Ptomedicodi, x.Emprcodi })
                    .Select(x => new MePtomedicionDTO()
                    {
                        Ptomedicodi = x.First().Ptomedicodi,
                        Grupocodi = x.First().Grupocodi,
                        Equinomb = x.First().Equinomb,
                        Equiabrev = x.First().Equiabrev,
                        Emprcodi = x.Key.Emprcodi,
                        Emprnomb = x.First().Emprnomb,
                        Emprabrev = x.First().Emprabrev,
                        Central = x.First().Equipopadre,
                        Equipadre = x.First().Equipadre,
                        Ptomedidesc = x.First().Ptomedidesc,
                        Ptomedibarranomb = x.First().Ptomedibarranomb,
                        Ptomedielenomb = x.First().PtoMediEleNomb,
                        Areacodi = x.First().Areacodi,
                        Areanomb = x.First().Areanomb,
                        AreaOperativa = x.First().AreaOperativa,
                        Tipoptomedicodi = x.First().Tptomedicodi
                    }).ToList();
        }

        /// <summary>
        /// Convertir MeReporptomed a PtoMedicion
        /// </summary>
        /// <param name="listaRptmed"></param>
        /// <returns></returns>
        public static List<MePtomedicionDTO> ListarPtoMedicionFromMeReporptomed(List<MeReporptomedDTO> listaRptmed)
        {
            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
            foreach (var reg in listaRptmed)
            {
                listaPto.Add(new MePtomedicionDTO()
                {
                    Ptomedidesc = reg.Ptomedidesc,
                    Ptomedielenomb = reg.Ptomedielenomb,
                    Equinomb = reg.Equinomb,
                    Tipoinfoabrev = reg.Tipoinfoabrev,
                    Ptomedicodi = reg.Ptomedicodi,
                    Tipoinfocodi = reg.Tipoinfocodi,
                });
            }

            return listaPto;
        }

        /// <summary>
        /// Listar centrales de una data de medicion 48
        /// </summary>
        /// <param name="lista48"></param>
        /// <param name="listaTgeneracion"></param>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        public static List<EqEquipoDTO> ListarCentralesFromM48(List<MeMedicion48DTO> lista48, List<SiTipogeneracionDTO> listaTgeneracion, List<EqEquipoDTO> listaEquipo)
        {
            var lista = lista48.GroupBy(x => new { x.Equipadre })
                .Select(x => new EqEquipoDTO()
                {
                    Equipadre = x.Key.Equipadre,
                    Tgenercodi = x.First().Tgenercodi,
                }).ToList();

            foreach (var reg in lista)
            {
                reg.Tgenernomb = (listaTgeneracion.Find(x => x.Tgenercodi == reg.Tgenercodi)?.Tgenernomb) ?? "";
                reg.Central = (listaEquipo.Find(x => x.Equicodi == reg.Equipadre)?.Equinomb) ?? "";
            }

            return lista.OrderBy(x => x.Central).ToList();
        }

        /// <summary>
        /// Listar centrales de una data de medicion 96
        /// </summary>
        /// <param name="lista96"></param>
        /// <param name="listaTgeneracion"></param>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        public static List<EqEquipoDTO> ListarCentralesFromM96(List<MeMedicion96DTO> lista96, List<SiTipogeneracionDTO> listaTgeneracion, List<EqEquipoDTO> listaEquipo)
        {
            var lista = lista96.GroupBy(x => new { x.Equipadre })
                .Select(x => new EqEquipoDTO()
                {
                    Equipadre = x.Key.Equipadre,
                    Tgenercodi = x.First().Tgenercodi,
                }).ToList();

            foreach (var reg in lista)
            {
                reg.Tgenernomb = (listaTgeneracion.Find(x => x.Tgenercodi == reg.Tgenercodi)?.Tgenernomb) ?? "";
                reg.Central = (listaEquipo.Find(x => x.Equicodi == reg.Equipadre)?.Equinomb) ?? "";
            }

            return lista.OrderBy(x => x.Central).ToList();
        }

        /// <summary>
        /// generar detalle cada media hora
        /// </summary>
        /// <param name="regTotal"></param>
        /// <param name="lista"></param>
        public static void SetMeditotalXLista(MeMedicion48DTO regTotal, List<MeMedicion48DTO> lista)
        {
            decimal totalReg = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal total = ((decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regTotal, null)).GetValueOrDefault(0);

                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor.GetValueOrDefault(0);
                }

                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, total);

                totalReg += total;
            }

            regTotal.Meditotal = totalReg;
        }

        /// <summary>
        /// SetMeditotalXListaResta
        /// </summary>
        /// <param name="regTotal"></param>
        /// <param name="lista"></param>
        public static void SetMeditotalXListaResta(MeMedicion48DTO regTotal, List<MeMedicion48DTO> lista)
        {
            decimal totalReg = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal total = ((decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regTotal, null)).GetValueOrDefault(0);

                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total -= valor.GetValueOrDefault(0);
                }

                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, total);

                totalReg += total;
            }

            regTotal.Meditotal = totalReg;
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="variacion"></param>
        /// <param name="nfi"></param>
        /// <returns></returns>
        public static string ImprimirVariacionHtml(decimal? variacion, NumberFormatInfo nfi)
        {
            if (variacion != null)
                return variacion.Value.ToString("N", nfi) + "%";

            return string.Empty;
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="variacion"></param>
        /// <param name="nfi"></param>
        /// <param name="numDecimal"></param>
        /// <returns></returns>
        public static string ImprimirVariacionYRedondeoHtml(decimal? variacion, NumberFormatInfo nfi, int numDecimal)
        {
            if (variacion != null)
            {
                decimal valor = Math.Round(variacion.Value, numDecimal);
                return valor.ToString("N", nfi) + "%";
            }

            return string.Empty;
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="meditotal"></param>
        /// <param name="nfi"></param>
        /// <returns></returns>
        public static string ImprimirValorTotalHtml(decimal? meditotal, NumberFormatInfo nfi)
        {
            return meditotal.GetValueOrDefault(0).ToString("N", nfi);
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="meditotal"></param>
        /// <param name="nfi"></param>
        /// <param name="numDecimal"></param>
        /// <returns></returns>
        public static string ImprimirValorTotalYRedondeoHtml(decimal? meditotal, NumberFormatInfo nfi, int numDecimal)
        {
            decimal valor = Math.Round(meditotal.GetValueOrDefault(0), numDecimal);
            return valor.ToString("N", nfi);
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="meditotal"></param>
        /// <param name="nfi"></param>
        /// <returns></returns>
        public static string ImprimirValorTotalOcultar0Html(decimal? meditotal, NumberFormatInfo nfi)
        {
            return meditotal.GetValueOrDefault(0) != 0 ? ImprimirValorTotalHtml(meditotal, nfi) : string.Empty;
        }

        /// <summary>
        /// Imprimir variación HTML
        /// </summary>
        /// <param name="meditotal"></param>
        /// <param name="nfi"></param>
        /// <returns></returns>
        public static string ImprimirValorTotalSinOcultar0Html(decimal? meditotal, NumberFormatInfo nfi)
        {
            return ImprimirValorTotalHtml(meditotal, nfi);
        }

        /// <summary>
        /// ListarMediaHora48
        /// </summary>
        /// <returns></returns>
        public static List<string> ListarMediaHora48()
        {
            DateTime horas = DateTime.Today;

            List<string> listaHora = new List<string>();
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                listaHora.Add(horas.ToString(ConstantesAppServicio.FormatoOnlyHora));
            }
            return listaHora;
        }

        /// <summary>
        /// ListarCuartoHora96
        /// </summary>
        /// <returns></returns>
        public static List<string> ListarCuartoHora96()
        {
            DateTime horas = DateTime.Today;

            List<string> listaHora = new List<string>();
            for (int i = 0; i < 96; i++)
            {
                horas = horas.AddMinutes(15);
                listaHora.Add(horas.ToString(ConstantesAppServicio.FormatoOnlyHora));
            }
            return listaHora;
        }

        #endregion

        /// Reemplazar caracteres especiales
        /// <returns></returns>
        public static string ReemplazarCaracteres(string texto)
        {
            String stAux;

            stAux = texto.Replace('Á', 'A');
            stAux = stAux.Replace('É', 'E');
            stAux = stAux.Replace('Í', 'I');
            stAux = stAux.Replace('Ó', 'O');
            stAux = stAux.Replace('Ú', 'U');
            stAux = stAux.Replace('á', 'a');
            stAux = stAux.Replace('é', 'e');
            stAux = stAux.Replace('í', 'i');
            stAux = stAux.Replace('ó', 'o');
            stAux = stAux.Replace('ú', 'u');
            stAux = stAux.Replace('ñ', '?');
            stAux = stAux.Replace('Ñ', '?');
            stAux = stAux.Replace('"', ' ');
            stAux = stAux.Replace('´', ' ');
            stAux = stAux.Replace("'", " ");

            return stAux.ToString();
        }

    }
}