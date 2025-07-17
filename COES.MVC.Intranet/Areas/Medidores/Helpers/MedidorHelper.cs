using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Helpers
{
    public class MedidorHelper
    {
        /// <summary>
        /// Permite generar el archivo excel de máxima demanda diaria
        /// </summary>
        /// <param name="entity"></param>
        public static void GenerarReporteMaximaDemandaDiaria(MaximaDemanda entity, string fechaInicio, string fechaFin)
        {
            try
            {
                String file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteMaximaDemandaDiaria;
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
                        ws.Cells[5, 2].Value = "MÁXIMA DEMANDA DIARIA";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[7, 2].Value = "DESDE:";
                        ws.Cells[7, 3].Value = fechaInicio;
                        ws.Cells[8, 2].Value = "HASTA:";
                        ws.Cells[8, 3].Value = fechaFin;

                        ws.Cells[10, 2].Value = "EMPRESA";
                        ws.Cells[10, 3].Value = "TIPO DE GENERACIÓN";
                        ws.Cells[10, 4].Value = "CENTRAL";
                        ws.Cells[10, 5].Value = "GRUPO";

                        for (var i = 1; i <= entity.ndiasXMes; i++)
                        {
                            ws.Cells[10, 5 + i].Value = i.ToString();
                        }

                        rg = ws.Cells[10, 2, 10, 5 + entity.ndiasXMes];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 11;
                        decimal valorMD = 0;

                        foreach (var reg in entity.ListaDemandaDia)
                        {
                            ws.Cells[row, 2].Value = reg.Empresanomb;
                            ws.Cells[row, 3].Value = reg.Tipogeneracion;
                            ws.Cells[row, 4].Value = reg.Centralnomb;
                            ws.Cells[row, 5].Value = reg.Gruponomb;

                            for (var i = 0; i < entity.ndiasXMes; i++)
                            {
                                if (reg.valores.Count > i)
                                {
                                    valorMD = reg.valores[i];
                                }
                                else
                                {
                                    valorMD = 0;
                                }

                                ws.Cells[row, 6 + i].Value = valorMD;
                            }

                            rg = ws.Cells[row, 2, row, 5 + entity.ndiasXMes];

                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            row++;
                        }

                        ws.Column(2).Width = 20;

                        for (int t = 7; t <= 5 + entity.ndiasXMes; t++)
                        {
                            ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        foreach (var reg in entity.ListaDemandaDiaTotalResumen)
                        {
                            switch (reg.Gruponomb)
                            {
                                case "TOTAL":
                                    ws.Cells[row, 2].Value = reg.Empresanomb;
                                    ws.Cells[row, 3].Value = reg.Tipogeneracion;
                                    ws.Cells[row, 4].Value = reg.Centralnomb;
                                    ws.Cells[row, 5].Value = reg.Gruponomb;
                                    for (var i = 0; i < entity.ndiasXMes; i++)
                                    {
                                        ws.Cells[row, 6 + i].Value = reg.valores[i];
                                    }
                                    break;
                                case "IMPORTACIÓN":
                                case "EXPORTACIÓN":
                                    ws.Cells[row, 2].Value = "L-2280 (Zorritos - Machala)";
                                    ws.Cells[row, 3].Value = "ECUADOR";
                                    ws.Cells[row, 4].Value = reg.Centralnomb;
                                    ws.Cells[row, 4, row, 5].Merge = true;

                                    for (var i = 0; i < entity.ndiasXMes; i++)
                                    {
                                        ws.Cells[row, 6 + i].Value = reg.valores[i];
                                    }

                                    break;

                                case "HORA":
                                    ws.Cells[row, 2].Value = reg.Empresanomb;
                                    ws.Cells[row, 3].Value = reg.Tipogeneracion;
                                    ws.Cells[row, 4].Value = reg.Centralnomb;
                                    ws.Cells[row, 5].Value = reg.Gruponomb;
                                    for (var i = 0; i < entity.ndiasXMes; i++)
                                    {
                                        ws.Cells[row, 6 + i].Value = reg.horamin[i];
                                    }
                                    break;

                            }

                            row++;
                        }

                        rg = ws.Cells[row - 4, 2, row - 1, 5 + entity.ndiasXMes];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3493D1"));

                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.View.FreezePanes(11, 6);
                        rg = ws.Cells[1, 3, row + 2, 5 + entity.ndiasXMes];
                        rg.AutoFitColumns();

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

        /// <summary>
        /// Permite generar el reporte de máxima demanda en HFP y HP
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public static void GenerarReporteMaximaDemandaHFPHP(MaximaDemanda entity, string fechaInicio, string fechaFin)
        {
            try
            {
                String file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteMaxinaDemandaHFPHP;
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
                        ws.Cells[5, 2].Value = "MÁXIMA DEMANDA HFP Y HP";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[7, 2].Value = "DESDE:";
                        ws.Cells[7, 3].Value = fechaInicio;
                        ws.Cells[8, 2].Value = "HASTA:";
                        ws.Cells[8, 3].Value = fechaFin;


                        ws.Cells[11, 2].Value = "FECHA";
                        ws.Cells[11, 3].Value = "HFP";
                        ws.Cells[11, 8].Value = "HP";

                        ws.Cells[12, 3].Value = "HORA";
                        ws.Cells[12, 4].Value = "TOTAL";
                        ws.Cells[12, 5].Value = "IMPORTACIÓN";
                        ws.Cells[12, 6].Value = "EXPORTACIÓN";
                        ws.Cells[12, 7].Value = "DEMANDA SEIN";
                        ws.Cells[12, 8].Value = "HORA";
                        ws.Cells[12, 9].Value = "TOTAL";
                        ws.Cells[12, 10].Value = "IMPORTACIÓN";
                        ws.Cells[12, 11].Value = "EXPORTACIÓN";
                        ws.Cells[12, 12].Value = "DEMANDA SEIN";

                        ws.Cells[13, 3].Value = "HH:MM";
                        ws.Cells[13, 4].Value = "MW";
                        ws.Cells[13, 5].Value = "MW";
                        ws.Cells[13, 6].Value = "MW";
                        ws.Cells[13, 7].Value = "MW";
                        ws.Cells[13, 8].Value = "HH:MM";
                        ws.Cells[13, 9].Value = "MW";
                        ws.Cells[13, 10].Value = "MW";
                        ws.Cells[13, 11].Value = "MW";
                        ws.Cells[13, 12].Value = "MW";


                        rg = ws.Cells[11, 2, 13, 2];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rg = ws.Cells[11, 3, 11, 7];
                        rg.Merge = true;

                        rg = ws.Cells[11, 8, 11, 12];
                        rg.Merge = true;

                        rg = ws.Cells[11, 2, 13, 12];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        int row = 14;
                        int index = 0;

                        decimal valorImportacionHFP = 0;
                        decimal valorExportacionHFP = 0;
                        decimal demandaHFP = 0;
                        decimal valorImportacionHP = 0;
                        decimal valorExportacionHP = 0;
                        decimal demandaHP = 0;
                        decimal negativo = -1;

                        foreach (var reg in entity.ListaDemandaDia_HFP_HP)
                        {
                            valorImportacionHFP = 0;
                            valorExportacionHFP = 0;
                            valorImportacionHP = 0;
                            valorExportacionHP = 0;
                            if (reg.ValorHFPInter < 0)
                            {
                                valorImportacionHFP = reg.ValorHFPInter * negativo * 4;
                            }
                            else
                            {
                                valorExportacionHFP = reg.ValorHFPInter * 4;
                            }
                            demandaHFP = reg.ValorHFP + valorExportacionHFP - valorImportacionHFP;

                            if (reg.ValorHPInter < 0)
                            {
                                valorImportacionHP = reg.ValorHPInter * negativo * 4;
                            }
                            else
                            {
                                valorExportacionHP = reg.ValorHPInter * 4;
                            }
                            demandaHP = reg.ValorHP + valorExportacionHP - valorImportacionHP;

                            ws.Cells[row, 2].Value = reg.Medifecha.ToString(Constantes.FormatoFecha);
                            ws.Cells[row, 3].Value = reg.MedifechaHFP;
                            ws.Cells[row, 4].Value = demandaHFP;
                            ws.Cells[row, 5].Value = valorImportacionHFP;
                            ws.Cells[row, 6].Value = valorExportacionHFP;
                            ws.Cells[row, 7].Value = reg.ValorHFP;
                            ws.Cells[row, 8].Value = reg.MedifechaHP;
                            ws.Cells[row, 9].Value = demandaHP;
                            ws.Cells[row, 10].Value = valorImportacionHP;
                            ws.Cells[row, 11].Value = valorExportacionHP;
                            ws.Cells[row, 12].Value = reg.ValorHP;

                            rg = ws.Cells[row, 2, row, 12];

                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            if (index == entity.IndexHFP)
                            {
                                rg = ws.Cells[row, 3, row, 7];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4E8FF"));
                            }
                            if (index == entity.IndexHP)
                            {
                                rg = ws.Cells[row, 8, row, 12];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4E8FF"));
                            }

                            row++;
                            index++;
                        }

                        ws.Column(2).Width = 20;

                        for (int t = 3; t <= 12; t++)
                        {
                            ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        rg = ws.Cells[1, 3, row + 2, 12];
                        rg.AutoFitColumns();


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

        /// <summary>
        /// Permite generar el reporte del ranking de la demanda en excel
        /// </summary>
        /// <param name="demandaDiaria"></param>
        /// <param name="datosMD"></param>
        /// <param name="listaOrdenada"></param>
        /// <param name="produccionEnergia"></param>
        /// <param name="fc"></param>
        /// <param name="evolucion"></param>
        /// <param name="diagramaCarga"></param>
        public static void GenerarReporteRankingDemanda(List<DemandadiaDTO> demandaDiaria, DemandadiaDTO datosMD, List<DemandadiaDTO> listaOrdenada,
            decimal produccionEnergia, decimal fc, EntidadSerieMedicionEvolucion evolucion, EntidadSerieMedicionEvolucion diagramaCarga)
        {
            try
            {
                String file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteRankingDemanda;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    #region Primera Hoja

                    ExcelWorksheet wsMD = xlPackage.Workbook.Worksheets.Add("MD Mensual");

                    if (wsMD != null)
                    {
                        wsMD.Cells[5, 2].Value = "MÁXIMA DEMANDA MENSUAL";

                        ExcelRange rg = wsMD.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        wsMD.Cells[7, 2].Value = "Máxima Demanda Mensual:";
                        wsMD.Cells[7, 3].Value = datosMD.ValorMD;
                        wsMD.Cells[8, 2].Value = "Fecha:";
                        wsMD.Cells[8, 3].Value = datosMD.FechaMD;
                        wsMD.Cells[9, 2].Value = "Hora:";
                        wsMD.Cells[9, 3].Value = datosMD.HoraMD;


                        wsMD.Cells[11, 2].Value = "FECHA";
                        wsMD.Cells[11, 3].Value = "HFP";
                        wsMD.Cells[11, 8].Value = "HP";

                        wsMD.Cells[12, 3].Value = "HORA";
                        wsMD.Cells[12, 4].Value = "TOTAL";
                        wsMD.Cells[12, 5].Value = "IMPORTACIÓN";
                        wsMD.Cells[12, 6].Value = "EXPORTACIÓN";
                        wsMD.Cells[12, 7].Value = "DEMANDA SEIN";
                        wsMD.Cells[12, 8].Value = "HORA";
                        wsMD.Cells[12, 9].Value = "TOTAL";
                        wsMD.Cells[12, 10].Value = "IMPORTACIÓN";
                        wsMD.Cells[12, 11].Value = "EXPORTACIÓN";
                        wsMD.Cells[12, 12].Value = "DEMANDA SEIN";

                        wsMD.Cells[13, 3].Value = "HH:MM";
                        wsMD.Cells[13, 4].Value = "MW";
                        wsMD.Cells[13, 5].Value = "MW";
                        wsMD.Cells[13, 6].Value = "MW";
                        wsMD.Cells[13, 7].Value = "MW";
                        wsMD.Cells[13, 8].Value = "HH:MM";
                        wsMD.Cells[13, 9].Value = "MW";
                        wsMD.Cells[13, 10].Value = "MW";
                        wsMD.Cells[13, 11].Value = "MW";
                        wsMD.Cells[13, 12].Value = "MW";


                        rg = wsMD.Cells[11, 2, 13, 2];
                        rg.Merge = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rg = wsMD.Cells[11, 3, 11, 7];
                        rg.Merge = true;

                        rg = wsMD.Cells[11, 8, 11, 12];
                        rg.Merge = true;

                        rg = wsMD.Cells[11, 2, 13, 12];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        int row = 14;
                        int index = 0;

                        decimal valorImportacionHFP = 0;
                        decimal valorExportacionHFP = 0;
                        decimal demandaHFP = 0;
                        decimal valorImportacionHP = 0;
                        decimal valorExportacionHP = 0;
                        decimal demandaHP = 0;
                        decimal negativo = -1;

                        foreach (var reg in demandaDiaria)
                        {
                            valorImportacionHFP = 0;
                            valorExportacionHFP = 0;
                            valorImportacionHP = 0;
                            valorExportacionHP = 0;
                            if (reg.ValorHFPInter < 0)
                            {
                                valorImportacionHFP = @reg.ValorHFPInter * negativo * 4;
                            }
                            else
                            {
                                valorExportacionHFP = @reg.ValorHFPInter * 4;
                            }
                            demandaHFP = reg.ValorHFP + valorExportacionHFP - valorImportacionHFP;

                            if (reg.ValorHPInter < 0)
                            {
                                valorImportacionHP = @reg.ValorHPInter * negativo * 4;
                            }
                            else
                            {
                                valorExportacionHP = @reg.ValorHPInter * 4;
                            }
                            demandaHP = reg.ValorHP + valorExportacionHP - valorImportacionHP;

                            wsMD.Cells[row, 2].Value = reg.Medifecha.ToString(Constantes.FormatoFecha);
                            wsMD.Cells[row, 3].Value = reg.MedifechaHFP;
                            wsMD.Cells[row, 4].Value = demandaHFP;
                            wsMD.Cells[row, 5].Value = valorImportacionHFP;
                            wsMD.Cells[row, 6].Value = valorExportacionHFP;
                            wsMD.Cells[row, 7].Value = reg.ValorHFP;
                            wsMD.Cells[row, 8].Value = reg.MedifechaHP;
                            wsMD.Cells[row, 9].Value = demandaHP;
                            wsMD.Cells[row, 10].Value = valorImportacionHP;
                            wsMD.Cells[row, 11].Value = valorExportacionHP;
                            wsMD.Cells[row, 12].Value = reg.ValorHP;

                            rg = wsMD.Cells[row, 2, row, 12];

                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            if (index == datosMD.IndiceMDHFP)
                            {
                                rg = wsMD.Cells[row, 3, row, 7];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4E8FF"));
                            }
                            if (index == datosMD.IndiceMDHP)
                            {
                                rg = wsMD.Cells[row, 8, row, 12];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4E8FF"));
                            }

                            row++;
                            index++;
                        }

                        wsMD.Column(2).Width = 20;

                        for (int t = 3; t <= 12; t++)
                        {
                            wsMD.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        rg = wsMD.Cells[1, 3, row + 2, 12];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsMD.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }



                    #endregion

                    #region Segunda parte

                    ExcelWorksheet wsOrden = xlPackage.Workbook.Worksheets.Add("Ordenamiento MD");

                    if (wsOrden != null)
                    {
                        wsOrden.Cells[5, 2].Value = "ORDENAMIENTO MÁXIMA DEMANDA";

                        ExcelRange rg = wsOrden.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        wsOrden.Cells[7, 2].Value = "Producción de Energía (MWh)";
                        wsOrden.Cells[7, 3].Value = produccionEnergia;
                        wsOrden.Cells[8, 2].Value = "Factor carga";
                        wsOrden.Cells[8, 3].Value = fc;

                        wsOrden.Cells[10, 2].Value = "N° de Registos/MES";
                        wsOrden.Cells[10, 3].Value = "Fecha/Hora";
                        wsOrden.Cells[10, 4].Value = "Total (MW)";
                        wsOrden.Cells[10, 5].Value = "Importación (MW)";
                        wsOrden.Cells[10, 6].Value = "Exportación (MW)";
                        wsOrden.Cells[10, 7].Value = "Máxima Demanda (MW)";

                        rg = wsOrden.Cells[10, 2, 10, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        int row = 11;
                        decimal valorExportacion = 0;
                        decimal valorImportacion = 0;
                        foreach (var reg in listaOrdenada)
                        {
                            valorExportacion = 0;
                            valorImportacion = 0;

                            if (reg.ValorInter < 0)
                            {
                                valorImportacion = reg.ValorInter;
                            }
                            else
                            {
                                valorExportacion = reg.ValorInter;
                            }

                            wsOrden.Cells[row, 2].Value = (row - 10);
                            wsOrden.Cells[row, 3].Value = reg.StrMediFecha;
                            wsOrden.Cells[row, 4].Value = reg.ValorGeneracion;
                            wsOrden.Cells[row, 5].Value = valorImportacion;
                            wsOrden.Cells[row, 6].Value = valorExportacion;
                            wsOrden.Cells[row, 7].Value = reg.Valor;

                            rg = wsOrden.Cells[row, 2, row, 7];

                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            row++;
                        }

                        wsOrden.Column(2).Width = 20;

                        for (int t = 3; t <= 7; t++)
                        {
                            wsOrden.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        rg = wsOrden.Cells[1, 3, row + 2, 7];
                        rg.AutoFitColumns();

                        var chart = wsOrden.Drawings.AddChart("crtExtensionsSize", eChartType.Area);
                        chart.SetPosition(180, 820);
                        chart.SetSize(1000, 500);
                        chart.Series.Add(wsOrden.Cells[11, 4, row, 4], wsOrden.Cells[11, 2, row, 2]);
                        chart.Legend.Position = eLegendPosition.Bottom;
                        chart.Legend.Add();
                        chart.YAxis.Title.Text = "Potencia (MW)";
                        chart.YAxis.Font.Size = 9;
                        chart.XAxis.Title.Text = "Tiempo";



                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsOrden.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    #endregion

                    #region Tercera parte

                    ExcelWorksheet wsEvolucion = xlPackage.Workbook.Worksheets.Add("Evolución diagramas");

                    if (wsEvolucion != null)
                    {
                        wsEvolucion.Cells[5, 2].Value = "EVOLUCIÓN DE LOS DIAGRAMAS DE CARGA";

                        ExcelRange rg = wsEvolucion.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        wsEvolucion.Cells[7, 2].Value = "Máxima Demanda Mensual:";
                        wsEvolucion.Cells[7, 3].Value = datosMD.ValorMD;
                        wsEvolucion.Cells[8, 2].Value = "Fecha:";
                        wsEvolucion.Cells[8, 3].Value = datosMD.FechaMD;
                        wsEvolucion.Cells[9, 2].Value = "Hora:";
                        wsEvolucion.Cells[9, 3].Value = datosMD.HoraMD;

                        wsEvolucion.Cells[11, 2].Value = "HORA";

                        int column = 3;

                        foreach (SerieMedicionEvolucion item in evolucion.ListaSerie)
                        {
                            wsEvolucion.Cells[11, column].Value = item.SerieName;
                            column++;
                        }

                        rg = wsEvolucion.Cells[11, 2, 11, column - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        int row = 12;
                        column = 3;

                        DateTime fecha = DateTime.ParseExact(datosMD.FechaMD, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                        for (int i = 1; i <= 96; i++)
                        {
                            wsEvolucion.Cells[row, column - 1].Value = fecha.AddMinutes(i * 15).ToString(Constantes.FormatoHoraMinuto);
                            row++;
                        }

                        foreach (SerieMedicionEvolucion item in evolucion.ListaSerie)
                        {
                            row = 12;
                            foreach (decimal valor in item.ListaValores)
                            {
                                wsEvolucion.Cells[row, column].Value = valor;
                                row++;
                            }
                            column++;
                        }

                        rg = wsEvolucion.Cells[11, 2, row - 1, column - 1];

                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        wsEvolucion.Column(2).Width = 30;

                        for (int t = 3; t <= 4; t++)
                        {
                            wsEvolucion.Column(t).Style.Numberformat.Format = "#,##0.000";
                        }

                        rg = wsEvolucion.Cells[1, 3, row + 2, 6];
                        rg.AutoFitColumns();

                        var chart = wsEvolucion.Drawings.AddChart("chartEvolucion", eChartType.AreaStacked);
                        chart.SetPosition(200, 890);
                        chart.SetSize(1000, 600);

                        column = 3;
                        int header = 0;
                        foreach (SerieMedicionEvolucion item in evolucion.ListaSerie)
                        {
                            chart.Series.Add(wsEvolucion.Cells[12, column, 107, column], wsEvolucion.Cells[12, 2, 107, 2]);
                            chart.Series[header].Header = item.SerieName;
                            header++;
                            column++;
                        }

                        chart.Legend.Position = eLegendPosition.Bottom;
                        chart.Legend.Add();
                        chart.Title.Text = "DESPACHO EN EL DÍA DE MÁXIMA DEMANDA POR TIPO DE RECURSO ENERGÉTICO " + evolucion.Titulo;
                        chart.YAxis.Title.Text = "Potencia (MW)";
                        chart.YAxis.Font.Size = 11;
                        chart.XAxis.Title.Text = "Tiempo";

                        row = 111;

                        wsEvolucion.Cells[row, 2].Value = "Fecha Máxima Demanda";
                        wsEvolucion.Cells[row, 3].Value = diagramaCarga.FechaMaximaDemanda;
                        wsEvolucion.Cells[row + 1, 2].Value = "Hora Máxima Demanda";
                        wsEvolucion.Cells[row + 1, 3].Value = DateTime.ParseExact(diagramaCarga.FechaMaximaDemanda, Constantes.FormatoFecha,
                            CultureInfo.InvariantCulture).AddMinutes(diagramaCarga.IndiceMaximaDemanda * 15).ToString(Constantes.FormatoHoraMinuto);
                        wsEvolucion.Cells[row + 2, 2].Value = "Valor Máxima Demanda";
                        wsEvolucion.Cells[row + 2, 3].Value = diagramaCarga.ValorMaximaDemanda;
                        wsEvolucion.Cells[row + 3, 2].Value = "Fecha Mínima Demanda";
                        wsEvolucion.Cells[row + 3, 3].Value = diagramaCarga.FechaMinimaDemanda;
                        wsEvolucion.Cells[row + 4, 2].Value = "Hora Mínima Demanda";
                        wsEvolucion.Cells[row + 4, 3].Value = DateTime.ParseExact(diagramaCarga.FechaMinimaDemanda, Constantes.FormatoFecha,
                            CultureInfo.InvariantCulture).AddMinutes(diagramaCarga.IndiceMinimaDemanda * 15).ToString(Constantes.FormatoHoraMinuto);
                        wsEvolucion.Cells[row + 5, 2].Value = "Valor Mínima Demanda";
                        wsEvolucion.Cells[row + 5, 3].Value = diagramaCarga.ValorMinimaDemanda;

                        wsEvolucion.Cells[118, 2].Value = "Hora";
                        wsEvolucion.Cells[118, 3].Value = "Máxima Demanda";
                        wsEvolucion.Cells[118, 4].Value = "Mínima Demanda";

                        rg = wsEvolucion.Cells[118, 2, 118, 4];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                        row = 119;
                        column = 3;

                        fecha = DateTime.ParseExact(datosMD.FechaMD, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                        for (int i = 1; i <= 96; i++)
                        {
                            wsEvolucion.Cells[row, column - 1].Value = fecha.AddMinutes(i * 15).ToString(Constantes.FormatoHoraMinuto);
                            row++;
                        }

                        foreach (SerieMedicionEvolucion item in diagramaCarga.ListaSerie)
                        {
                            row = 119;
                            foreach (decimal valor in item.ListaValores)
                            {
                                wsEvolucion.Cells[row, column].Value = valor;
                                row++;
                            }
                            column++;
                        }

                        rg = wsEvolucion.Cells[119, 2, row - 1, column - 1];

                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        var chartMaximo = wsEvolucion.Drawings.AddChart("chartMaximaMinima", eChartType.Area);
                        chartMaximo.SetPosition(2340, 480);
                        chartMaximo.SetSize(1000, 600);

                        column = 3;
                        header = 0;
                        foreach (SerieMedicionEvolucion item in diagramaCarga.ListaSerie)
                        {
                            chartMaximo.Series.Add(wsEvolucion.Cells[119, column, 214, column], wsEvolucion.Cells[119, 2, 214, 2]);
                            chartMaximo.Series[header].Header = item.SerieName;
                            header++;
                            column++;
                        }

                        chartMaximo.Legend.Position = eLegendPosition.Bottom;
                        chartMaximo.Legend.Add();
                        chartMaximo.Title.Text = "DIAGRAMA DE CARGA DEL SEIN MÁXIMA Y MÍNIMA DEMANDA ACUMULADA A " + diagramaCarga.Titulo;
                        chartMaximo.YAxis.Title.Text = "Potencia (MW)";
                        chartMaximo.YAxis.Font.Size = 11;
                        chartMaximo.XAxis.Title.Text = "Tiempo";

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = wsEvolucion.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    #endregion

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite generar el reporte del diagrama de carga
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mes"></param>
        public static void GenerarReporteDuracionCarga(List<SerieDuracionCarga> list, string fechaDesde, string fechaHasta)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteDuracionCarga;
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
                        ws.Cells[5, 2].Value = "DURACIÓN DE CARGA";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[6, 2].Value = "Desde:";
                        ws.Cells[6, 3].Value = fechaDesde;
                        ws.Cells[6, 4].Value = "Hasta:";
                        ws.Cells[6, 5].Value = fechaHasta;

                        int row = 8;
                        int column = 2;


                        if (list.Count > 0)
                        {
                            ws.Cells[row, column].Value = "NRO";
                            column++;
                            foreach (SerieDuracionCarga item in list)
                            {
                                ws.Cells[row, column].Value = item.SerieName;
                                column++;
                            }

                            rg = ws.Cells[row, 2, row, column - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                            int index = 1;
                            column = 3;

                            foreach (SerieDuracionCarga item in list)
                            {
                                row = 9;
                                foreach (decimal valor in item.ListaValores)
                                {
                                    if (column == 3)
                                    {
                                        ws.Cells[row, column - 1].Value = index;
                                    }

                                    ws.Cells[row, column].Value = valor;
                                    row++;
                                    index++;
                                }
                                column++;
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


                            var chart = ws.Drawings.AddChart("chartMaximaMinima", eChartType.AreaStacked);
                            chart.SetPosition(240, 750);
                            chart.SetSize(1000, 600);

                            column = 3;
                            int header = 0;
                            foreach (SerieDuracionCarga item in list)
                            {
                                chart.Series.Add(ws.Cells[9, column, row - 1, column], ws.Cells[9, 2, row - 1, 2]);
                                chart.Series[header].Header = item.SerieName;
                                header++;
                                column++;
                            }

                            chart.Legend.Position = eLegendPosition.Bottom;
                            chart.Legend.Add();
                            chart.Title.Text = "DIAGRAMA DE DURACIÓN DE CARGA MENSUAL";
                            chart.YAxis.Title.Text = "Potencia (MW)";
                            chart.YAxis.Font.Size = 11;
                            chart.XAxis.Title.Text = "Tiempo";


                            for (int t = 3; t <= column; t++)
                            {
                                ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                            }

                        }


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

        /// <summary>
        /// Permite armar la tabla del reporte de máxima demanda consolidado mensual
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerReporteConsolidado(List<MedicionReporteDTO> list, List<ItemReporteMedicionDTO> resultadoTotal,
            int opcion)
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 3;

            StringBuilder str = new StringBuilder();

            var listFechas = list.Select(x => new { x.Anio, x.Mes, x.FechaMaximaDemanda, x.Indice }).
                OrderBy(x => x.FechaMaximaDemanda).Distinct().ToList();

            #region Cabecera

            str.Append("<table class='tabla-formulario'>");
            str.Append("  <thead>");
            str.Append("    <tr>");
            str.Append("       <th rowspan='3'>Empresa</th>");
            str.Append("       <th rowspan='3'>Tipo de Generación</th>");
            str.Append("       <th rowspan='3'>Central</th>");
            str.Append("       <th rowspan='3'>Unidad</th>");
            str.Append(string.Format("       <th colspan='{0}'>Máxima Demanda Mensual (MW)</th>", listFechas.Count));
            str.Append("    </tr>");
            str.Append("    <tr style='background-color:#50A2D8'>");

            foreach (var item in listFechas)
            {
                str.Append(string.Format("       <th>{0}</th>", COES.Base.Tools.Util.ObtenerNombreMesAbrev(item.Mes) + " " + item.Anio));
            }

            str.Append("    </tr>");
            str.Append("    <tr>");

            foreach (var item in listFechas)
            {
                string fecha = item.FechaMaximaDemanda.AddMinutes(item.Indice * 15).ToString(Constantes.FormatoFechaHora);
                str.Append(string.Format("       <th>{0}</th>", fecha));
            }

            str.Append("    </tr>");
            str.Append("  </thead>");

            #endregion

            str.Append("  <tbody>");

            List<ReporteConsolidado> resultado = ObtenerEstructura(list);

            foreach (ReporteConsolidado item in resultado)
            {
                str.Append("    <tr>");
                string style = "style='text-align:right'";
                if (item.IndEmpresa == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesEmpresa, item.RowSpanEmpresa));
                }
                if (item.IndTotalEmpresa == 1)
                {
                    str.Append(string.Format("      <td colspan='4' style='background-color:#BFDEF0;text-align:right; font-weight:bold' >Total: {0}</td>", item.DesEmpresa));
                    style = "style='background-color:#BFDEF0; text-align:right'";
                }
                if (item.IndTipoGeneracion == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesTipoGeneracion, item.RowSpanTipoGeneracion));
                }
                if (item.IndTotalTipoGeneracion == 1)
                {
                    str.Append(string.Format("      <td colspan='3' style='background-color:#EEF5FD;text-align:right; font-weight:bold'>Total: {0}</td>", item.DesTipoGeneracion));
                    style = "style='background-color:#EEF5FD; text-align:right'";
                }
                if (item.IndCentral == 1)
                {
                    str.Append(string.Format("      <td rowspan='{1}'>{0}</td>", item.DesCentral, item.RowSpanCentral));
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
            string stilo = "style='text-align:right;'";
            str.Append("    <tr><td colspan='" + listFechas.Count + 4 + "'><div style='clear:both; height:15px'></div></td></tr>");
            str.Append("  <tr><td rowspan='2' colspan='4' style='text-align:center;font:bold;' >INTERCAMBIOS INTERNACIONALES DE ELECTRICIDAD</td>");
            stilo = "style='background-color:#2980B9; text-align:center; color:#ffffff'";
            str.Append("      <td colspan='" + listFechas.Count + "' " + stilo + ">Máxima Demanda Mensual(MW)</td></tr><tr>");
            foreach (var item in listFechas)
            {
                str.Append(string.Format("       <td style='background-color:#50A2D8;color:#ffffff;text-align:center;' >{0}</td>", COES.Base.Tools.Util.ObtenerNombreMesAbrev(item.Mes) + " " + item.Anio));
            }
            str.Append("    </tr>");

            str.Append("    <tr><td COLSPAN='2' " + stilo + ">ENLACE INTERNACIONAL</td><td " + stilo + ">PAÍS</td><td " + stilo + ">TIPO DE INTERCAMBIO</td>");

            foreach (var item in listFechas)
            {
                string fecha = item.FechaMaximaDemanda.AddMinutes(item.Indice * 15).ToString(Constantes.FormatoFechaHora);
                str.Append(string.Format("       <td " + stilo + ">{0}</td>", fecha));
            }
            str.Append("  </tr>");

            str.Append("  <tr><td colspan='2'>L-2280 (Zorritos - Machala)</td><td>ECUADOR</td><td>IMPORTACIÓN</td>");
            stilo = "style='text-align:right;'";
            foreach (var item in resultadoTotal)
            {
                decimal import = 0M;
                if (item.Interconexion < 0)
                    import = item.Interconexion * -1;
                str.Append(string.Format("       <td " + stilo + ">{0}</td>", import.ToString("N", nfi)));
            }
            str.Append("  </tr>");
            str.Append("  <tr><td colspan='2'>L-2280 (Zorritos - Machala)</td><td>ECUADOR</td><td>EXPORTACIÓN</td>");
            foreach (var item in resultadoTotal)
            {
                decimal export = 0M;
                if (item.Interconexion > 0)
                    export = item.Interconexion;
                str.Append(string.Format("       <td " + stilo + ">{0}</td>", export.ToString("N", nfi)));
            }
            str.Append("  </tr>");
            stilo = "style='background-color:#2980B9; text-align:right; color:#ffffff'";
            str.Append("  <tr><td colspan='4' " + stilo + ">TOTAL</td>");
            foreach (var item in resultadoTotal)
            {
                decimal total = 0M;
                total = item.MaximaDemanda;
                str.Append(string.Format("       <td " + stilo + ">{0}</td>", total.ToString("N", nfi)));
            }
            str.Append("  </tr>");

            str.Append("  </tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite obtener el reporte de consolidado mensual en excel
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteExcelConsolidado(List<MedicionReporteDTO> list, List<ItemReporteMedicionDTO> resultadoTotal)
        {
            try
            {
                String file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteConsolidoMensual;
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
                        ws.Cells[5, 2].Value = "CONSOLIDADO MAXIMA DEMANDA MENSUAL";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;


                        var listFechas = list.Select(x => new { x.Anio, x.Mes, x.FechaMaximaDemanda, x.Indice }).
                                OrderBy(x => x.FechaMaximaDemanda).Distinct().ToList();


                        ws.Cells[7, 2].Value = "EMPRESA";
                        ws.Cells[7, 2, 9, 2].Merge = true;
                        ws.Cells[7, 3].Value = "TIPO DE GENERACIÓN";
                        ws.Cells[7, 3, 9, 3].Merge = true;
                        ws.Cells[7, 4].Value = "CENTRAL";
                        ws.Cells[7, 4, 9, 4].Merge = true;
                        ws.Cells[7, 5].Value = "UNIDAD";
                        ws.Cells[7, 5, 9, 5].Merge = true;
                        ws.Cells[7, 6].Value = "MÁXIMA DEMANDA MENSUAL (MW)";
                        ws.Cells[7, 6, 7, 6 + listFechas.Count - 1].Merge = true;

                        int column = 6;

                        foreach (var item in listFechas)
                        {
                            string fecha = item.FechaMaximaDemanda.AddMinutes(item.Indice * 15).ToString(Constantes.FormatoFechaHora);
                            ws.Cells[8, column].Value = COES.Base.Tools.Util.ObtenerNombreMesAbrev(item.Mes) + " " + item.Anio;
                            ws.Cells[9, column].Value = fecha;
                            column++;
                        }

                        rg = ws.Cells[7, 2, 9, column - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        int row = 10;
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
                        row++;
                        ws.Cells[row, 2].Value = "INTERCAMBIOS INTERNACIONALES DE ELECTRICIDAD";
                        ws.Cells[row, 2, row + 1, 5].Merge = true;
                        ws.Cells[row, 6].Value = "Máxima Demanda Mensual(MW)";
                        ws.Cells[row, 6, row, 5 + listFechas.Count].Merge = true;
                        rg = ws.Cells[row, 2, row + 2, 5 + listFechas.Count];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        row++;
                        for (var i = 0; i < listFechas.Count; i++)
                        {
                            ws.Cells[row, 6 + i].Value = COES.Base.Tools.Util.ObtenerNombreMesAbrev(listFechas[i].Mes) + " " + listFechas[i].Anio;
                        }
                        row++;
                        ws.Cells[row, 2].Value = "ENLACE INTERNACIONAL";
                        ws.Cells[row, 2, row, 3].Merge = true;
                        ws.Cells[row, 4].Value = "PAÍS";
                        ws.Cells[row, 5].Value = "TIPO DE INTERCAMBIO";
                        for (var i = 0; i < listFechas.Count; i++)
                        {
                            string fecha = listFechas[i].FechaMaximaDemanda.AddMinutes(listFechas[i].Indice * 15).ToString(Constantes.FormatoFechaHora);
                            ws.Cells[row, 6 + i].Value = fecha;
                        }
                        row++;
                        ws.Cells[row, 2].Value = "L-2280 (Zorritos - Machala)";
                        ws.Cells[row + 1, 2].Value = "L-2280 (Zorritos - Machala)";
                        ws.Cells[row, 2, row, 3].Merge = true;
                        ws.Cells[row + 1, 2, row + 1, 3].Merge = true;
                        ws.Cells[row, 4].Value = "ECUADOR";
                        ws.Cells[row + 1, 4].Value = "ECUADOR";
                        ws.Cells[row, 5].Value = "IMPORTACIÓN";
                        ws.Cells[row + 1, 5].Value = "EXPORTACIÓN";
                        ws.Cells[row + 2, 2].Value = "TOTAL:";
                        ws.Cells[row + 2, 2, row + 2, 5].Merge = true;
                        for (var i = 0; i < resultadoTotal.Count; i++)
                        {
                            decimal import = 0M;
                            decimal export = 0M;
                            if (resultadoTotal[i].Interconexion < 0)
                            {
                                import = resultadoTotal[i].Interconexion * -1;
                            }
                            else
                            {
                                export = resultadoTotal[i].Interconexion;
                            }
                            ws.Cells[row, 6 + i].Value = import;
                            ws.Cells[row + 1, 6 + i].Value = export;
                            ws.Cells[row + 2, 6 + i].Value = resultadoTotal[i].MaximaDemanda;
                        }
                        row = row + 2;
                        rg = ws.Cells[row, 2, row, 5 + listFechas.Count];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);

                        rg = ws.Cells[10, 2, row, column - 1];

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

                        rg = ws.Cells[1, 2, row, columnFinal];
                        rg.AutoFitColumns();

                        ws.View.FreezePanes(10, 3);

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
                            itemResultado.EmprCodi = itemEmpresa.EmprCodi;
                            itemResultado.TgenerCodi = itemTipoGeneracion.Tgenercodi;
                            itemResultado.IndTotalCentral = 0;
                            itemResultado.IndTotalTipoGeneracion = 0;
                            itemResultado.IndTotalEmpresa = 0;

                            List<decimal> valores = new List<decimal>();

                            foreach (var itemFecha in listFecha)
                            {
                                var entity = list.Where(x => x.EmprCodi == itemEmpresa.EmprCodi && x.EquiCodi == itemUnidad.EquiCodi && x.Anio == itemFecha.Anio && x.Mes == itemFecha.Mes).FirstOrDefault();

                                if (entity != null)
                                {
                                    valores.Add(entity.ValorMD);
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
        /// Permite generar el reporte de validación de medidores
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteValidacionMedidores(List<ReporteValidacionMedidor> list, string fechaInicio,
            string fechaFin, ItemReporteMedicionDTO datosMDDespacho, ItemReporteMedicionDTO datosMDMedidor)
        {
            try
            {
                String file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteValidacionMedidores;
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
                        ws.Cells[5, 2].Value = "REPORTE DE VALIDACIÓN DE REGISTROS DE MEDIDORES Y SCADA";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[7, 2 - 1].Value = "DESDE:";
                        ws.Cells[7, 3 - 1].Value = fechaInicio;
                        ws.Cells[8, 2 - 1].Value = "HASTA:";
                        ws.Cells[8, 3 - 1].Value = fechaFin;

                        ws.Cells[10, 2].Value = "EMPRESA";
                        ws.Cells[10, 3].Value = "GRUPO";
                        ws.Cells[10, 4].Value = "REGISTROS MEDIDORES \n(MWh)";
                        ws.Cells[10, 5].Value = "REGISTROS SCADA \n(MWh)";
                        ws.Cells[10, 6].Value = "DESVIACIÓN \n(%)";

                        if (datosMDMedidor != null)
                            ws.Cells[10, 7].Value = "MD MEDIDORES (MW) \n" + datosMDMedidor.FechaMaximaDemanda.ToString(Constantes.FormatoFecha) + " \n(" + datosMDMedidor.HoraMaximaDemanda + ")";
                        else
                            ws.Cells[10, 7].Value = "MD MEDIDORES (MW) ";
                        if (datosMDDespacho != null)
                            ws.Cells[10, 8].Value = "MD SCADA (MW) \n" + datosMDDespacho.FechaMaximaDemanda.ToString(Constantes.FormatoFecha) + " \n(" + datosMDDespacho.HoraMaximaDemanda + ")";
                        else
                            ws.Cells[10, 8].Value = "MD SCADA (MW) ";

                        ws.Cells[10, 9].Value = "DESVIACIÓN MD \n(%)";

                        rg = ws.Cells[10, 2, 10, 9];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        rg.Style.WrapText = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        ws.Cells[10, 4, 10, 4].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ef8808"));
                        ws.Cells[10, 7, 10, 7].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ef8808"));
                        ws.Cells[10, 5, 10, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#89a539"));
                        ws.Cells[10, 8, 10, 8].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#89a539"));

                        ws.Row(10).Height = 42.0;

                        //Filas data
                        int row = 11;

                        foreach (var reg in list)
                        {
                            ws.Cells[row, 2].Value = reg.DesEmpresa;
                            ws.Cells[row, 3].Value = reg.DesGrupo;
                            ws.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[row, 4].Value = reg.ValorMedidor;
                            ws.Cells[row, 5].Value = reg.ValorDespacho;
                            if (reg.IndMuestra == Constantes.SI)
                            {
                                ws.Cells[row, 6].Value = reg.Desviacion;
                                rg = ws.Cells[row, 6, row, 6];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                if (reg.Desviacion < 0)
                                {
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87ceeb"));
                                }
                                if (reg.Desviacion >= 0 && reg.Desviacion < Convert.ToDecimal(5))
                                {
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffffbf"));
                                }
                                if(reg.Desviacion == Convert.ToDecimal(5))
                                {
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87ceeb"));
                                }
                                if(reg.Desviacion > Convert.ToDecimal(5))
                                {
                                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffff00"));
                                }
                            }

                            if (reg.IndColor == Constantes.SI)
                            {
                                rg = ws.Cells[row, 6, row, 6];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(reg.Color));
                            }
                            ws.Cells[row, 7].Value = reg.MDMedidor;
                            ws.Cells[row, 8].Value = reg.MDDespacho;
                            ws.Cells[row, 9].Value = reg.MDDesviacion;
                            rg = ws.Cells[row, 9, row, 9];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            if (reg.MDDesviacion < 0)
                            {
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87ceeb"));
                            }
                            if (reg.MDDesviacion >= 0 && reg.MDDesviacion < Convert.ToDecimal(5))
                            {
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffffbf"));
                            }
                            if (reg.MDDesviacion == Convert.ToDecimal(5))
                            {
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87ceeb"));
                            }
                            if (reg.MDDesviacion > Convert.ToDecimal(5))
                            {
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffff00"));
                            }

                            row++;
                        }

                        rg = ws.Cells[11, 2, row - 1, 9];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //Fila totales
                        decimal totalDespacho = list.Sum(x => x.ValorDespacho);
                        decimal totalMedidor = list.Sum(x => x.ValorMedidor);
                        decimal totalMDDespacho = list.Sum(x => x.MDDespacho);
                        decimal totalMDMedidor = list.Sum(x => x.MDMedidor);

                        ws.Cells[row, 3].Value = "TOTALES:";
                        ws.Cells[row, 4].Value = totalMedidor;
                        ws.Cells[row, 5].Value = totalDespacho;
                        ws.Cells[row, 7].Value = totalMDMedidor;
                        ws.Cells[row, 8].Value = totalMDDespacho;

                        rg = ws.Cells[row, 2, row, 9];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Row(row).Height = 22;

                        for (int t = 3; t <= 9; t++)
                        {
                            ws.Column(t).Style.Numberformat.Format = "###,###,##0.000";
                            ws.Column(t).Width = 20.0;
                        }

                        ws.Column(2).Width = 45;
                        ws.Column(3).Width = 14;

                        ws.View.FreezePanes(10 + 1, 3 + 1);
                        ws.View.ShowGridLines = false;

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

        /// <summary>
        /// Permite generar el reporte de stock de combustible
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        public static void GenerarReporteStockCombustible(List<MeMedicion1DTO> list, string fechaDesde, string fechaHasta)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteStockCombustible;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("STOCK");

                    if (ws != null)
                    {
                        ws.Cells[5, 2].Value = "STOCK DE COMBUSTIBLE";

                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[6, 2].Value = "Desde:";
                        ws.Cells[6, 3].Value = fechaDesde;
                        ws.Cells[6, 4].Value = "Hasta:";
                        ws.Cells[6, 5].Value = fechaHasta;

                        int row = 8;
                        int column = 2;


                        if (list.Count > 0)
                        {

                            ws.Cells[row, column].Value = "EMPRESA";
                            ws.Cells[row, column + 1].Value = "CENTRAL";
                            ws.Cells[row, column + 2].Value = "FECHA";
                            ws.Cells[row, column + 3].Value = "TIPO COMBUSTIBLE";
                            ws.Cells[row, column + 4].Value = "STOCK";
                            ws.Cells[row, column + 5].Value = "NOTA";
                            ws.Cells[row, column + 6].Value = "USUARIO";
                            ws.Cells[row, column + 7].Value = "FECHA CARGA";

                            rg = ws.Cells[row, 2, row, column + 7];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                            row++;

                            foreach (MeMedicion1DTO item in list)
                            {
                                if (item.IndInformo == Constantes.SI)
                                {
                                    ws.Cells[row, column].Value = item.Emprnomb;
                                    ws.Cells[row, column + 1].Value = item.Gruponomb;
                                    ws.Cells[row, column + 2].Value = item.Medifecha;
                                    ws.Cells[row, column + 3].Value = item.Tipoinfodesc;
                                    ws.Cells[row, column + 4].Value = item.H1;
                                    ws.Cells[row, column + 5].Value = item.Nota;
                                    ws.Cells[row, column + 6].Value = item.Lastuser;
                                    ws.Cells[row, column + 7].Value = item.Lastdate;
                                }
                                else
                                {
                                    ws.Cells[row, column].Value = item.Emprnomb;
                                    ws.Cells[row, column + 1].Value = item.Gruponomb;
                                    ws.Cells[row, column + 2].Value = item.Medifecha;
                                    ws.Cells[row, column + 3].Value = item.Tipoinfodesc;
                                    ws.Cells[row, column + 4].Value = "No informó";
                                    ws.Cells[row, column + 5].Value = "";
                                }

                                row++;
                            }

                            rg = ws.Cells[9, 2, row - 1, column + 7];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            ws.Column(column + 4).Style.Numberformat.Format = "#,###.00";
                            ws.Column(column + 2).Style.Numberformat.Format = "dd/mm/yyyy";
                            ws.Column(column + 7).Style.Numberformat.Format = "dd/mm/yyyy";

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        }


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

        /// <summary>
        /// Permite generar el formato vertical del CMG
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteVerticalCMgRealPorArea(List<ReporteCMGRealDTO> list, string filename, string fechaDesde, string fechaHasta)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CMG_REAL_AREA");

                    if (ws != null)
                    {
                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[6, 2].Value = "Desde:";
                        ws.Cells[6, 3].Value = fechaDesde;
                        ws.Cells[6, 4].Value = "Hasta:";
                        ws.Cells[6, 5].Value = fechaHasta;

                        int row = 8;
                        int column = 2;

                        if (list.Count > 0)
                        {
                            ws.Cells[row, column].Value = "FECHA HORA";
                            ws.Cells[row, column + 1].Value = "CMG REAL CENTRO";
                            ws.Cells[row, column + 2].Value = "CMG REAL NORTE";
                            ws.Cells[row, column + 3].Value = "CMG REAL SUR";
                            rg = ws.Cells[row, 2, row, column + 3];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                            row++;

                            foreach (ReporteCMGRealDTO item in list)
                            {
                                ws.Cells[row, column].Value = item.Fecha.ToString(Constantes.FormatoFechaHora);
                                ws.Cells[row, column + 1].Value = item.Centro;
                                ws.Cells[row, column + 2].Value = item.Norte;
                                ws.Cells[row, column + 3].Value = item.Sur;
                                row++;
                            }

                            rg = ws.Cells[9, 2, row - 1, column + 3];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            //ws.Column(column + 4).Style.Numberformat.Format = "#,###.00";
                            //ws.Column(column + 2).Style.Numberformat.Format = "dd/mm/yyyy";                            

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        }

                        ws.Cells[5, 2].Value = "EVOLUCIÓN MEDIO HORARIO DE LOS COSTOS MARGINALES REALES POR ÁREAS OPERATIVAS DEL SEIN (S/./MWh)";

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

        /// <summary>
        /// Permite generar el formato horizontal del CMG
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteHorizontalCMgRealPorArea(List<MeMedicion48DTO> list, string filename, string fechaDesde,
            string fechaHasta, DateTime fecha)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("CMG_REAL_AREA");

                    if (ws != null)
                    {
                        ExcelRange rg = ws.Cells[5, 2, 5, 2];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        ws.Cells[6, 2].Value = "Desde:";
                        ws.Cells[6, 3].Value = fechaDesde;
                        ws.Cells[6, 4].Value = "Hasta:";
                        ws.Cells[6, 5].Value = fechaHasta;

                        int row = 8;
                        int column = 2;

                        if (list.Count > 0)
                        {
                            ws.Cells[row, column].Value = "ÁREA OPERATIVA";
                            ws.Cells[row, column + 1].Value = "CÓDIGO";
                            ws.Cells[row, column + 2].Value = "DÍA";
                            ws.Cells[row, column + 3].Value = "MES";
                            ws.Cells[row, column + 4].Value = "AÑO";

                            for (int i = 1; i <= 48; i++)
                            {
                                DateTime fechaTitulo = (i < 48) ? fecha.AddMinutes(i * 30) : fecha.AddMinutes(i * 30).AddSeconds(-1);
                                ws.Cells[row, column + 4 + i].Value = fechaTitulo.ToString(Constantes.FormatoHoraMinuto);
                            }

                            rg = ws.Cells[row, 2, row, column + 4 + 48];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);

                            List<MeMedicion48DTO> ordenado = list.OrderBy(x => x.Ptomedicodi).ThenBy(x => x.Medifecha).ToList();

                            row++;

                            foreach (MeMedicion48DTO item in list)
                            {
                                ws.Cells[row, column].Value = item.Descripcion;
                                ws.Cells[row, column + 1].Value = item.Ptomedicodi;
                                ws.Cells[row, column + 2].Value = item.Medifecha.ToString(Constantes.FormatoFecha);
                                ws.Cells[row, column + 3].Value = item.Mes;
                                ws.Cells[row, column + 4].Value = item.Medifecha.Year;

                                for (int i = 1; i <= 48; i++)
                                {
                                    decimal valor = (decimal)item.GetType().GetProperty(Constantes.CaracterH + i).GetValue(item, null);
                                    ws.Cells[row, column + 4 + i].Value = valor;
                                }

                                row++;
                            }

                            rg = ws.Cells[9, 2, row - 1, column + 4 + 48];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            //ws.Column(column + 4).Style.Numberformat.Format = "#,###.00";
                            //ws.Column(column + 2).Style.Numberformat.Format = "dd/mm/yyyy";                            

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        }

                        ws.Cells[5, 2].Value = "EVOLUCIÓN MEDIO HORARIO DE LOS COSTOS MARGINALES REALES POR ÁREAS OPERATIVAS DEL SEIN (S/./MWh)";

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
    }

    /// <summary>
    /// Permite obtener la estructura del reporte
    /// </summary>
    public class ReporteConsolidado
    {
        public int EmprCodi { get; set; }
        public int TgenerCodi { get; set; }
        public string DesEmpresa { get; set; }
        public int RowSpanEmpresa { get; set; }
        public int IndEmpresa { get; set; }
        public string DesTipoGeneracion { get; set; }
        public int RowSpanTipoGeneracion { get; set; }
        public int IndTipoGeneracion { get; set; }
        public string DesCentral { get; set; }
        public int RowSpanCentral { get; set; }
        public int IndCentral { get; set; }
        public string DesUnidad { get; set; }
        public string DesFuenteEnergia { get; set; }
        public List<decimal> Valores { get; set; }
        public int IndTotalEmpresa { get; set; }
        public int IndTotalTipoGeneracion { get; set; }
        public int IndTotalCentral { get; set; }
        public int IndTotalGeneralTG { get; set; }
        public int IndTotalGeneral { get; set; }
    }

    /// <summary>
    /// Permite obtener la cantidad de elementos del reporte
    /// </summary>
    public class SubReporteConsolidado
    {
        public int EmprCodi { get; set; }
        public int TgenerCodi { get; set; }
        public string Central { get; set; }
        public int Cantidad { get; set; }
        public List<decimal> Valores { get; set; }
        public int IndicadorEmpresa { get; set; }
        public int IndicadorTiepoGeneracion { get; set; }
    }


}