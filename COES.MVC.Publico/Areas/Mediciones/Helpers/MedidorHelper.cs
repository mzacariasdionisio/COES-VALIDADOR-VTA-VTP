using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Medidores.Helpers;
using COES.MVC.Publico.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace COES.MVC.Publico.Areas.Mediciones.Helpers
{
    public class MedidorHelper
    {
        /// <summary>
        /// Permite armar la tabla del reporte de máxima demanda consolidado mensual
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerReporteConsolidado(List<MedicionReporteDTO> list, int opcion)
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 3;

            StringBuilder str = new StringBuilder();

            var listFechas = list.Select(x => new { x.Anio, x.Mes }).
                OrderBy(x => x.Anio).ThenBy(x => x.Mes).Distinct().ToList();

            #region Cabecera

            str.Append("<table id='tabla' class='tabla-formulario' cellspacing='0'>");
            str.Append("  <tbody>");
            str.Append("    <tr>");
            str.Append("       <td rowspan='2' style='background-color:#2980B9;text-align:center; font-weight:bold; color:#ffffff'>Empresa</td>");
            str.Append("       <td rowspan='2' style='background-color:#2980B9;text-align:center; font-weight:bold; color:#ffffff'>Tipo de Generación</td>");
            str.Append("       <td rowspan='2' style='background-color:#2980B9;text-align:center; font-weight:bold; color:#ffffff'>Central</td>");
            str.Append("       <td rowspan='2' style='background-color:#2980B9;text-align:center; font-weight:bold; color:#ffffff'>Unidad</td>");
            str.Append(string.Format("       <td colspan='{0}' style='background-color:#2980B9;text-align:center; font-weight:bold; color:#ffffff'>Acumulado Mensual (MW)</td>", listFechas.Count));
            str.Append("    </tr>");
            str.Append("    <tr style='background-color:#50A2D8'>");

            foreach (var item in listFechas)
            {
                str.Append(string.Format("       <td style='text-align:center; font-weight:bold; color:#ffffff'>{0}</td>", COES.Base.Tools.Util.ObtenerNombreMesAbrev(item.Mes) + " " + item.Anio));
            }

            str.Append("    </tr>");            
            //str.Append("  </thead>");

            #endregion

            //str.Append("  <tbody>");

            List<ReporteConsolidado> resultado = ObtenerEstructura(list);

            foreach (ReporteConsolidado item in resultado)
            {
                str.Append("    <tr>");
                string style = "style='text-align:right'";
                if (item.IndEmpresa == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesEmpresa.Trim(), item.RowSpanEmpresa));
                }
                if (item.IndTotalEmpresa == 1)
                {
                    str.Append(string.Format("      <td colspan='4' style='background-color:#BFDEF0;text-align:right; font-weight:bold' >Total: {0}</td>", item.DesEmpresa));
                    style = "style='background-color:#BFDEF0; text-align:right'";
                }
                if (item.IndTipoGeneracion == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesTipoGeneracion.Trim(), item.RowSpanTipoGeneracion));
                }
                if (item.IndTotalTipoGeneracion == 1)
                {
                    str.Append(string.Format("      <td colspan='3' style='background-color:#EEF5FD;text-align:right; font-weight:bold'>Total: {0}</td>", item.DesTipoGeneracion));
                    style = "style='background-color:#EEF5FD; text-align:right'";
                }
                if (item.IndCentral == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesCentral.Trim(), item.RowSpanCentral));
                }

                if (item.IndTotalGeneralTG == 1)
                {
                    str.Append(string.Format("      <td colspan='4' style='background-color:#3594D2;text-align:right; font-weight:bold; color:#ffffff'>TOTAL: {0}</td>", item.DesTipoGeneracion));
                    style = "style='background-color:#3594D2; text-align:right; color:#ffffff'";
                }

                if (item.IndTotalGeneral == 1)
                {
                    str.Append("      <td colspan='4' style='background-color:#2980B9;text-align:right; font-weight:bold; color:#ffffff'>TOTAL COES</td>");
                    style = "style='background-color:#2980B9; text-align:right; color:#ffffff'";
                }

                if (item.IndTotalEmpresa != 1 && item.IndTotalTipoGeneracion != 1 && item.IndTotalGeneralTG != 1 && item.IndTotalGeneral != 1)
                {
                    str.Append(string.Format("      <td>{0}</td>", item.DesUnidad));
                }

                foreach (decimal valor in item.Valores)
                {
                    str.Append(string.Format("      <td {1}>{0}</td>", valor.ToString("N", nfi), style));
                }

                str.Append("    </tr>");
            }

            str.Append("  </tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite obtener la estructura del reporte
        /// </summary>
        /// <param name="list"></param>
        private static List<ReporteConsolidado> ObtenerEstructura(List<MedicionReporteDTO> list)
        {
            List<ReporteConsolidado> resultado = new List<ReporteConsolidado>();
            List<SubReporteConsolidado> subResultado = new List<SubReporteConsolidado>();

            #region Obtención de datos

            var listEmpresas = list.Select(x => new { x.EmprCodi, x.Emprnomb }).Distinct().ToList();
            var listFecha = list.Select(x => new { x.Anio, x.Mes }).Distinct().ToList();

            var listEquipos = list.Select(x => new
            {
                x.EmprCodi,
                x.Central,
                x.EquiCodi,
                x.Unidad,
                x.Tgenernomb,
                x.Tgenercodi,
                x.Fenergcodi,
                x.Fenergnomb
            }).Distinct().ToList();


            foreach (var itemEmpresa in listEmpresas)
            {
                var listTipoGeneracion = listEquipos.Where(x => x.EmprCodi == itemEmpresa.EmprCodi).
                    Select(x => new { x.Tgenercodi, x.Tgenernomb }).Distinct().ToList();

                foreach (var itemTipoGeneracion in listTipoGeneracion)
                {
                    var listCentral = listEquipos.Where(x => x.EmprCodi == itemEmpresa.EmprCodi && x.Tgenercodi == itemTipoGeneracion.Tgenercodi).Select(
                        x => new { x.Central }).Distinct().ToList();

                    foreach (var itemCentral in listCentral)
                    {
                        var listaUnidad = listEquipos.Where(x => x.EmprCodi == itemEmpresa.EmprCodi && x.Tgenercodi == itemTipoGeneracion.Tgenercodi &&
                            x.Central == itemCentral.Central).Select(x => new { x.EquiCodi, x.Unidad, x.Fenergnomb }).Distinct().ToList();

                        foreach (var itemUnidad in listaUnidad)
                        {
                            ReporteConsolidado itemResultado = new ReporteConsolidado();
                            itemResultado.DesEmpresa = itemEmpresa.Emprnomb;
                            itemResultado.DesTipoGeneracion = itemTipoGeneracion.Tgenernomb;
                            itemResultado.DesCentral = itemCentral.Central;
                            itemResultado.DesUnidad = itemUnidad.Unidad;
                            itemResultado.DesFuenteEnergia = itemUnidad.Fenergnomb;
                            itemResultado.EmprCodi = itemEmpresa.EmprCodi;
                            itemResultado.TgenerCodi = itemTipoGeneracion.Tgenercodi;
                            itemResultado.IndTotalCentral = 0;
                            itemResultado.IndTotalTipoGeneracion = 0;
                            itemResultado.IndTotalEmpresa = 0;

                            List<decimal> valores = new List<decimal>();

                            foreach (var itemFecha in listFecha)
                            {
                                var entity = list.Where(x => x.EquiCodi == itemUnidad.EquiCodi && x.Anio == itemFecha.Anio && x.Mes == itemFecha.Mes).FirstOrDefault();

                                if (entity != null)
                                {
                                    valores.Add(entity.ValorAcumulado);
                                }
                            }

                            itemResultado.Valores = valores;
                            resultado.Add(itemResultado);
                        }

                        SubReporteConsolidado subReporte = new SubReporteConsolidado();
                        subReporte.Cantidad = listaUnidad.Count;
                        subReporte.Central = itemCentral.Central;
                        subReporte.EmprCodi = itemEmpresa.EmprCodi;
                        subReporte.TgenerCodi = itemTipoGeneracion.Tgenercodi;
                        subResultado.Add(subReporte);
                    }
                }
            }

            #endregion

            List<int> subListEmpresa = new List<int>();
            List<SubReporteConsolidado> subListTipoGeneracion = new List<SubReporteConsolidado>();
            List<SubReporteConsolidado> subListCentral = new List<SubReporteConsolidado>();

            foreach (ReporteConsolidado item in resultado)
            {
                if (!subListEmpresa.Contains(item.EmprCodi))
                {
                    item.RowSpanEmpresa = subResultado.Where(x => x.EmprCodi == item.EmprCodi).Sum(x => x.Cantidad)
                        + subResultado.Where(x => x.EmprCodi == item.EmprCodi).Select(x => x.TgenerCodi).Distinct().Count();
                    item.IndEmpresa = 1;
                    subListEmpresa.Add(item.EmprCodi);
                }
                else
                {
                    item.RowSpanEmpresa = 0;
                    item.IndEmpresa = 0;
                }

                int count = subListTipoGeneracion.Where(x => x.EmprCodi == item.EmprCodi && x.TgenerCodi == item.TgenerCodi).Count();

                if (count == 0)
                {
                    item.RowSpanTipoGeneracion = subResultado.Where(x => x.EmprCodi == item.EmprCodi && x.TgenerCodi ==
                        item.TgenerCodi).Sum(x => x.Cantidad);
                    item.IndTipoGeneracion = 1;

                    SubReporteConsolidado subItemReporteConsolidado = new SubReporteConsolidado();
                    subItemReporteConsolidado.EmprCodi = item.EmprCodi;
                    subItemReporteConsolidado.TgenerCodi = item.TgenerCodi;
                    subListTipoGeneracion.Add(subItemReporteConsolidado);
                }
                else
                {
                    item.RowSpanTipoGeneracion = 0;
                    item.IndTipoGeneracion = 0;
                }

                int countCentral = subListCentral.Where(x => x.EmprCodi == item.EmprCodi && x.TgenerCodi == item.TgenerCodi &&
                    x.Central == item.DesCentral).Count();

                if (countCentral == 0)
                {
                    item.RowSpanCentral = subResultado.Where(x => x.EmprCodi == item.EmprCodi && x.TgenerCodi ==
                        item.TgenerCodi && x.Central == item.DesCentral).Sum(x => x.Cantidad);
                    item.IndCentral = 1;

                    SubReporteConsolidado subItemReporteConsolidado = new SubReporteConsolidado();
                    subItemReporteConsolidado.EmprCodi = item.EmprCodi;
                    subItemReporteConsolidado.TgenerCodi = item.TgenerCodi;
                    subItemReporteConsolidado.Central = item.DesCentral;
                    subListCentral.Add(subItemReporteConsolidado);
                }
                else
                {
                    item.RowSpanCentral = 0;
                    item.IndCentral = 0;
                }
            }

            List<ReporteConsolidado> final = new List<ReporteConsolidado>();

            foreach (var itemEmpresa in listEmpresas)
            {
                var listTipoGeneracion = resultado.Where(x => x.EmprCodi == itemEmpresa.EmprCodi).Select(x =>
                    new { x.TgenerCodi, x.DesTipoGeneracion }).Distinct().ToList();
                List<decimal> totalEmpresa = new List<decimal>();

                int contadorEmpresa = 0;
                foreach (var itemTipoGeneracion in listTipoGeneracion)
                {
                    List<decimal> totalTipoGeneracion = new List<decimal>();
                    List<List<decimal>> lista = resultado.Where(x => x.EmprCodi == itemEmpresa.EmprCodi && x.TgenerCodi ==
                        itemTipoGeneracion.TgenerCodi).Select(x => x.Valores).ToList();

                    int contadorTipoGeneracion = 0;
                    foreach (List<decimal> subList in lista)
                    {
                        if (contadorTipoGeneracion == 0)
                        {
                            totalTipoGeneracion = new List<decimal>(subList);
                        }
                        else
                        {
                            for (int i = 0; i < subList.Count; i++)
                            {
                                totalTipoGeneracion[i] = totalTipoGeneracion[i] + subList[i];
                            }
                        }
                        contadorTipoGeneracion++;
                    }

                    if (contadorEmpresa == 0)
                    {
                        totalEmpresa = new List<decimal>(totalTipoGeneracion);
                    }
                    else
                    {
                        for (int i = 0; i < totalTipoGeneracion.Count; i++)
                        {
                            totalEmpresa[i] = totalEmpresa[i] + totalTipoGeneracion[i];
                        }
                    }

                    contadorEmpresa++;

                    List<ReporteConsolidado> subListTG = resultado.Where(x => x.EmprCodi == itemEmpresa.EmprCodi && x.TgenerCodi ==
                        itemTipoGeneracion.TgenerCodi).ToList();
                    final.AddRange(subListTG);

                    ReporteConsolidado subTotalTG = new ReporteConsolidado();
                    subTotalTG.TgenerCodi = itemTipoGeneracion.TgenerCodi;
                    subTotalTG.DesTipoGeneracion = itemTipoGeneracion.DesTipoGeneracion;
                    subTotalTG.Valores = totalTipoGeneracion;
                    subTotalTG.IndTotalTipoGeneracion = 1;
                    final.Add(subTotalTG);
                }

                ReporteConsolidado subTotalEmpresa = new ReporteConsolidado();
                subTotalEmpresa.EmprCodi = itemEmpresa.EmprCodi;
                subTotalEmpresa.DesEmpresa = itemEmpresa.Emprnomb;
                subTotalEmpresa.IndTotalEmpresa = 1;
                subTotalEmpresa.Valores = totalEmpresa;
                final.Add(subTotalEmpresa);
            }

            List<ReporteConsolidado> arreglo = final.Where(x => x.IndTotalTipoGeneracion == 1).ToList();
            var subListTipo = arreglo.Select(x => new { x.TgenerCodi, x.DesTipoGeneracion }).Distinct().ToList();

            int contadorTotal = 0;
            List<decimal> valoresTotal = new List<decimal>();
            foreach (var item in subListTipo)
            {
                List<decimal> valoresTipo = new List<decimal>();

                List<List<decimal>> lista = arreglo.Where(x => x.TgenerCodi ==
                       item.TgenerCodi).Select(x => x.Valores).ToList();

                int contadorTipo = 0;
                foreach (List<decimal> subList in lista)
                {
                    if (contadorTipo == 0)
                    {
                        valoresTipo = new List<decimal>(subList);
                    }
                    else
                    {
                        for (int i = 0; i < subList.Count; i++)
                        {
                            valoresTipo[i] = valoresTipo[i] + subList[i];
                        }
                    }
                    contadorTipo++;
                }

                ReporteConsolidado itemTipoGeneracion = new ReporteConsolidado();
                itemTipoGeneracion.TgenerCodi = item.TgenerCodi;
                itemTipoGeneracion.DesTipoGeneracion = item.DesTipoGeneracion;
                itemTipoGeneracion.Valores = valoresTipo;
                itemTipoGeneracion.IndTotalGeneralTG = 1;
                final.Add(itemTipoGeneracion);

                if (contadorTotal == 0)
                {
                    valoresTotal = new List<decimal>(valoresTipo);
                }
                else
                {
                    for (int i = 0; i < valoresTipo.Count; i++)
                    {
                        valoresTotal[i] = valoresTotal[i] + valoresTipo[i];
                    }
                }
                contadorTotal++;
            }

            ReporteConsolidado itemGeneral = new ReporteConsolidado();
            itemGeneral.Valores = valoresTotal;
            itemGeneral.IndTotalGeneral = 1;
            final.Add(itemGeneral);

            return final;
        }

        /// <summary>
        /// Permite obtener el reporte de consolidado mensual en excel
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteExcelConsolidado(List<MedicionReporteDTO> list)
        {
            try
            {
                String file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + NombreArchivo.ReporteEjecutadoAcumuladoMensual;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        ws.Cells[5, 2].Value = "DESPACHO EJECUTADO ACUMULADO MENSUAL";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        var listFechas = list.Select(x => new { x.Anio, x.Mes }).
                                OrderBy(x => x.Anio).ThenBy(x => x.Mes).Distinct().ToList();
                        
                        ws.Cells[7, 2].Value = "EMPRESA";
                        ws.Cells[7, 2, 8, 2].Merge = true;
                        ws.Cells[7, 3].Value = "TIPO DE GENERACIÓN";
                        ws.Cells[7, 3, 8, 3].Merge = true;
                        ws.Cells[7, 4].Value = "CENTRAL";
                        ws.Cells[7, 4, 8, 4].Merge = true;
                        ws.Cells[7, 5].Value = "UNIDAD";
                        ws.Cells[7, 5, 8, 5].Merge = true;
                        ws.Cells[7, 6].Value = "ACUMULADO MENSUAL (MW)";
                        ws.Cells[7, 6, 7, 6 + listFechas.Count - 1].Merge = true;

                        int column = 6;

                        foreach (var item in listFechas)
                        {                            
                            ws.Cells[8, column].Value = COES.Base.Tools.Util.ObtenerNombreMesAbrev(item.Mes) + " " + item.Anio;                         
                            column++;
                        }

                        rg = ws.Cells[7, 2, 8, column - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        int row = 9;
                        int columnFinal = column;

                        List<ReporteConsolidado> resultado = ObtenerEstructura(list);

                        foreach (ReporteConsolidado item in resultado)
                        {

                            if (item.IndEmpresa == 1)
                            {
                                ws.Cells[row, 2].Value = item.DesEmpresa;
                                ws.Cells[row, 2, row + item.RowSpanEmpresa - 1, 2].Merge = true;
                            }

                            if (item.IndTotalEmpresa == 1)
                            {
                                ws.Cells[row, 2].Value = "TOTAL: " + item.DesEmpresa;
                                ws.Cells[row, 2, row, 5].Merge = true;

                                rg = ws.Cells[row, 2, row, columnFinal - 1];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFDEF0"));
                            }

                            if (item.IndTipoGeneracion == 1)
                            {
                                ws.Cells[row, 3].Value = item.DesTipoGeneracion;
                                ws.Cells[row, 3, row + item.RowSpanTipoGeneracion - 1, 3].Merge = true;
                            }

                            if (item.IndTotalTipoGeneracion == 1)
                            {
                                ws.Cells[row, 3].Value = "TOTAL: " + item.DesTipoGeneracion;
                                ws.Cells[row, 3, row, 5].Merge = true;

                                rg = ws.Cells[row, 3, row, columnFinal - 1];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EEF5FD"));
                            }

                            if (item.IndCentral == 1)
                            {
                                ws.Cells[row, 4].Value = item.DesCentral;
                                ws.Cells[row, 4, row + item.RowSpanCentral - 1, 4].Merge = true;
                            }

                            if (item.IndTotalGeneralTG == 1)
                            {
                                ws.Cells[row, 2].Value = "TOTAL: " + item.DesTipoGeneracion;
                                ws.Cells[row, 2, row, 5].Merge = true;

                                rg = ws.Cells[row, 2, row, columnFinal - 1];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3594D2"));
                                rg.Style.Font.Color.SetColor(Color.White);
                            }

                            if (item.IndTotalGeneral == 1)
                            {
                                ws.Cells[row, 2].Value = "TOTAL COES ";
                                ws.Cells[row, 2, row, 5].Merge = true;

                                rg = ws.Cells[row, 2, row, columnFinal - 1];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                rg.Style.Font.Color.SetColor(Color.White);
                            }

                            if (item.IndTotalEmpresa != 1 && item.IndTotalTipoGeneracion != 1 && item.IndTotalGeneralTG != 1 && item.IndTotalGeneral != 1)
                            {
                                ws.Cells[row, 5].Value = item.DesUnidad;
                            }

                            column = 6;
                            foreach (decimal valor in item.Valores)
                            {
                                ws.Cells[row, column].Value = valor;
                                column++;
                            }
                            row++;
                        }

                        rg = ws.Cells[9, 2, row - 1, column - 1];

                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        
                        for (int t = 6; t <= 6 + columnFinal; t++)
                        {
                            ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        rg = ws.Cells[1, 2, row - 6 , columnFinal];
                        rg.AutoFitColumns();
                        ws.View.FreezePanes(9, 3);

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
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
    }
}