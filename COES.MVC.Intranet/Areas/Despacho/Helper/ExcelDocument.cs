using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
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

namespace COES.MVC.Intranet.Areas.Despacho.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Reporte oficial de Costos Variables
        /// </summary>
        /// <param name="listaCostosvariables"></param>
        /// <param name="listaParametrosGenerales"></param>
        /// <param name="repC"></param>
        public static void GenerarReporteOficialCostosVariables(List<PrCvariablesDTO> listaCostosvariables, List<PrGrupodatDTO> listaParametrosGenerales, PrRepcvDTO repC)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDespacho].ToString();
            FileInfo newFile = new FileInfo(ruta + NombreArchivo.ReporteOficialCostosVariablesRepCv);
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaReporteCostosVariablesRepCv);
            ///////////////////Parámetros generales
            n_parameter l_PGenerales = new n_parameter();
            foreach (var drParam in listaParametrosGenerales)
            {
                string formulaDat = l_PGenerales.EvaluateFormula(drParam.Formuladat.Trim());
                l_PGenerales.SetData(drParam.Concepabrev.Trim(), drParam.Formuladat.Trim());
            }
            ///////////////////////////////////////
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivo.ReporteOficialCostosVariablesRepCv);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                ws.Cells[10, 2].Value = repC.Repnomb;
                ws.Cells[11, 4].Value = "SEMANA N° " + EPDate.f_numerosemana(repC.Repfecha).ToString() + " PARA EL DIA " + repC.Repfecha.ToString("dd/MM/yyyy HH:mm");
                int row = 15;
                foreach (var oCostosvariable in listaCostosvariables)
                {
                    if (oCostosvariable.TipoModo != "H")
                    {
                        ws.Cells[row, 2].Value = oCostosvariable.Emprnomb;
                        ws.Cells[row, 2].StyleID = ws.Cells[16, 2].StyleID;
                        ws.Cells[row, 3].Value = oCostosvariable.Gruponomb;
                        SetCeldaReporteHistorico(ws, row, 4, oCostosvariable.FormulaCecSi, oCostosvariable.CecSi);
                        SetCeldaReporteHistorico(ws, row, 5, oCostosvariable.FormulaPe, oCostosvariable.Pe);
                        SetCeldaReporteHistorico(ws, row, 6, oCostosvariable.FormulaRendSi, oCostosvariable.RendSi);
                        SetCeldaReporteHistorico(ws, row, 7, oCostosvariable.FormulaCcomb, oCostosvariable.Ccomb);
                        SetCeldaReporteHistorico(ws, row, 8, oCostosvariable.FormulaCvnc, oCostosvariable.Cvnc);
                        SetCeldaReporteHistorico(ws, row, 9, oCostosvariable.FormulaCvc, oCostosvariable.Cvc);
                        SetCeldaReporteHistorico(ws, row, 10, oCostosvariable.FormulaCv, oCostosvariable.Cv);
                        ws.Cells[row, 10].StyleID = ws.Cells[16, 10].StyleID;
                        row++;
                    }
                }
                ws.Cells[row - 1, 2].StyleID = ws.Cells[14, 2].StyleID;
                ws.Cells[row - 1, 3].StyleID = ws.Cells[14, 3].StyleID;
                ws.Cells[row - 1, 4].StyleID = ws.Cells[14, 4].StyleID;
                ws.Cells[row - 1, 5].StyleID = ws.Cells[14, 5].StyleID;
                ws.Cells[row - 1, 6].StyleID = ws.Cells[14, 6].StyleID;
                ws.Cells[row - 1, 7].StyleID = ws.Cells[14, 7].StyleID;
                ws.Cells[row - 1, 8].StyleID = ws.Cells[14, 8].StyleID;
                ws.Cells[row - 1, 9].StyleID = ws.Cells[14, 9].StyleID;
                ws.Cells[row - 1, 10].StyleID = ws.Cells[14, 10].StyleID;
                using (var range = ws.Cells[row - 1, 2, row - 1, 10])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                row = row + 2;

                //DATOS CAÑON DEL PATO
                ws.Cells[row, 2].Value = "EMPRESA";
                ws.Cells[row, 3].Value = "CENTRAL - MODO OPERACION";
                ws.Cells[row, 8].Value = "";
                ws.Cells[row, 9].Value = "COSTO";
                ws.Cells[row + 1, 9].Value = "SOLIDOS";
                ws.Cells[row, 10].Value = "COSTO O Y M";
                ws.Cells[row + 1, 10].Value = "S/./ KWh";
                ws.Cells[row, 2].StyleID = ws.Cells[12, 2].StyleID;
                ws.Cells[row, 3].StyleID = ws.Cells[12, 3].StyleID;
                ws.Cells[row, 8].StyleID = ws.Cells[12, 8].StyleID;
                ws.Cells[row, 9].StyleID = ws.Cells[12, 9].StyleID;
                ws.Cells[row + 1, 9].StyleID = ws.Cells[13, 9].StyleID;
                ws.Cells[row, 10].StyleID = ws.Cells[12, 10].StyleID;
                ws.Cells[row + 1, 10].StyleID = ws.Cells[13, 10].StyleID;

                ws.Cells[row, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, 10].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[row, 2, row, 10].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[row + 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws.Cells[row + 1, 10].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[row + 2, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws.Cells[row + 2, 10].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                ws.Cells[row + 2, 2, row + 2, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                ws.Cells[row + 3, 3].Style.Font.Bold = false;
                ws.Cells[row + 3, 6].Style.Font.Bold = false;

                row = row + 3;

                foreach (var oCostosvariable in listaCostosvariables)
                {
                    if (oCostosvariable.TipoModo == "H")
                    {
                        ws.Cells[row, 2].Value = oCostosvariable.Emprnomb;
                        ws.Cells[row, 2].StyleID = ws.Cells[16, 2].StyleID;
                        ws.Cells[row, 3].Value = oCostosvariable.Gruponomb;
                        SetCeldaReporteHistorico(ws, row, 4, oCostosvariable.FormulaCecSi, oCostosvariable.CecSi);
                        SetCeldaReporteHistorico(ws, row, 5, oCostosvariable.FormulaPe, oCostosvariable.Pe);
                        SetCeldaReporteHistorico(ws, row, 6, oCostosvariable.FormulaRendSi, oCostosvariable.RendSi);
                        SetCeldaReporteHistorico(ws, row, 7, oCostosvariable.FormulaCcomb, oCostosvariable.Ccomb);
                        SetCeldaReporteHistorico(ws, row, 8, oCostosvariable.FormulaCvnc, oCostosvariable.Cvnc);
                        SetCeldaReporteHistorico(ws, row, 9, oCostosvariable.FormulaCvc, oCostosvariable.Cvc);
                        SetCeldaReporteHistorico(ws, row, 10, oCostosvariable.FormulaCv, oCostosvariable.Cv);
                        ws.Cells[row, 10].StyleID = ws.Cells[16, 10].StyleID;
                        row++;
                    }
                }
        

                ws.Cells[row - 1, 2].StyleID = ws.Cells[14, 2].StyleID;
                ws.Cells[row - 1, 3].StyleID = ws.Cells[14, 3].StyleID;
                ws.Cells[row - 1, 4].StyleID = ws.Cells[14, 4].StyleID;
                ws.Cells[row - 1, 5].StyleID = ws.Cells[14, 5].StyleID;
                ws.Cells[row - 1, 6].StyleID = ws.Cells[14, 6].StyleID;
                ws.Cells[row - 1, 7].StyleID = ws.Cells[14, 7].StyleID;
                ws.Cells[row - 1, 8].StyleID = ws.Cells[14, 8].StyleID;
                ws.Cells[row - 1, 9].StyleID = ws.Cells[14, 9].StyleID;
                ws.Cells[row - 1, 10].StyleID = ws.Cells[14, 10].StyleID;
                using (var range = ws.Cells[row - 1, 2, row - 1, 10])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }

                //DATOS GENERALES
                row = row + 3;
                ws.Cells[row, 3].Value = "Tipo de Cambio";
                ws.Cells[row, 3].Style.Font.Bold = true;
                var oTipoCambio = listaParametrosGenerales.Single(t => t.Concepabrev.ToUpper().Equals(("TCambio").ToUpper()));
                if (oTipoCambio != null)
                    ws.Cells[row, 4].Value = l_PGenerales.GetEvaluate("TCambio");
                row++;
                ws.Cells[row, 3].Value = "Factor Actual.de Perdida de Energ. Marg.";
                ws.Cells[row, 3].Style.Font.Bold = true;
                var oFactor = listaParametrosGenerales.Single(t => t.Concepabrev.Contains("FAPEM"));
                if (oFactor != null)
                    ws.Cells[row, 4].Value = l_PGenerales.GetEvaluate("FAPEM");
                ws.Cells[row, 6].Value = "FECHA HORA:";
                ws.Cells[row, 6].Style.Font.Bold = true;
                ws.Cells[row, 7].Value = repC.Repfecha.ToString("dd/MM/yyyy");
                row++;
                ws.Cells[row, 3].Value = "Canon agua";
                ws.Cells[row, 3].Style.Font.Bold = true;
                var oCanon = listaParametrosGenerales.Single(t => t.Concepabrev.Equals("CANON"));
                if (oCanon != null)
                    ws.Cells[row, 4].Value = l_PGenerales.GetEvaluate("CANON");
                ws.Cells[row, 6].Value = "REPORTE:";
                ws.Cells[row, 6].Style.Font.Bold = true;
                ws.Cells[row, 7].Value = repC.Repnomb;
                row++;
                ws.Cells[row, 3].Value = "Costo de Racionamiento";
                ws.Cells[row, 3].Style.Font.Bold = true;
                var oCostoAgua = listaParametrosGenerales.Single(t => t.Concepabrev.Contains("COSTORAC"));
                if (oCostoAgua != null)
                    ws.Cells[row, 4].Value = l_PGenerales.GetEvaluate("COSTORAC");
                row++;
                ws.Cells[row, 3].Value = "Valor Agua";
                ws.Cells[row, 3].Style.Font.Bold = true;
                var oValorAgua = listaParametrosGenerales.Single(t => t.Concepabrev.Contains("VALORAGUA"));
                if (oValorAgua != null)
                    ws.Cells[row, 4].Value = l_PGenerales.GetEvaluate("VALORAGUA");
                ws.Cells[row, 6].Value = repC.Repdetalle;
                row++;
                ws.Cells[row, 3].Value = "CEC: Consumo Específico de Calor";
                ws.Cells[row, 3].Style.Font.Bold = true;

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Value o Formula
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fila"></param>
        /// <param name="col"></param>
        /// <param name="formula"></param>
        private static void SetCeldaReporteHistorico(ExcelWorksheet ws, int fila, int col, string formula, decimal? valor)
        {
            if (!string.IsNullOrEmpty(formula))
            {
                try
                {
                    ws.Cells[fila, col].FormulaR1C1 = formula;
                }
                catch (Exception )
                {
                    ws.Cells[fila, col].Value = formula;
                }
            }
            else
            {
                ws.Cells[fila, col].Value = valor;
            }
        }

        /// <summary>
        /// Permite generar el reporte de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listGeneracion"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void GenerarReporteCortoPlazo(List<CpMedicion48DTO> list, string path, string filename)
        {
            try
            {
                string file = path + filename;
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
                        ws.Cells[2, 3].Value = "DATOS EXPORTADOS YUPANA";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "RECURSO";
                        ws.Cells[index, 3].Value = "00:30";
                        ws.Cells[index, 4].Value = "01:00";
                        ws.Cells[index, 5].Value = "01:30";
                        ws.Cells[index, 6].Value = "02:00";
                        ws.Cells[index, 7].Value = "02:30";
                        ws.Cells[index, 8].Value = "03:00";
                        ws.Cells[index, 9].Value = "03:30";
                        ws.Cells[index, 10].Value = "04:00";
                        ws.Cells[index, 11].Value = "04:30";
                        ws.Cells[index, 12].Value = "05:00";
                        ws.Cells[index, 13].Value = "05:30";
                        ws.Cells[index, 14].Value = "06:00";
                        ws.Cells[index, 15].Value = "06:30";
                        ws.Cells[index, 16].Value = "07:00";
                        ws.Cells[index, 17].Value = "07:30";
                        ws.Cells[index, 18].Value = "08:00";
                        ws.Cells[index, 19].Value = "08:30";
                        ws.Cells[index, 20].Value = "09:00";
                        ws.Cells[index, 21].Value = "09:30";
                        ws.Cells[index, 22].Value = "10:00";
                        ws.Cells[index, 23].Value = "10:30";
                        ws.Cells[index, 24].Value = "11:00";
                        ws.Cells[index, 25].Value = "11:30";
                        ws.Cells[index, 26].Value = "12:00";
                        ws.Cells[index, 27].Value = "12:30";
                        ws.Cells[index, 28].Value = "13:00";
                        ws.Cells[index, 29].Value = "13:30";
                        ws.Cells[index, 30].Value = "14:00";
                        ws.Cells[index, 31].Value = "14:30"; 
                        ws.Cells[index, 32].Value = "15:00";
                        ws.Cells[index, 33].Value = "15:30";
                        ws.Cells[index, 34].Value = "16:00";
                        ws.Cells[index, 35].Value = "16:30";
                        ws.Cells[index, 36].Value = "17:00";
                        ws.Cells[index, 37].Value = "17:30";
                        ws.Cells[index, 38].Value = "18:00";
                        ws.Cells[index, 39].Value = "18:30";
                        ws.Cells[index, 40].Value = "19:00";
                        ws.Cells[index, 41].Value = "19:30";
                        ws.Cells[index, 42].Value = "20:00";
                        ws.Cells[index, 43].Value = "20:30";
                        ws.Cells[index, 44].Value = "21:00";
                        ws.Cells[index, 45].Value = "21:30";
                        ws.Cells[index, 46].Value = "22:00";
                        ws.Cells[index, 47].Value = "22:30";
                        ws.Cells[index, 48].Value = "23:00";
                        ws.Cells[index, 49].Value = "23:30";
                        ws.Cells[index, 50].Value = "00:00";
                                                
                        rg = ws.Cells[index, 2, index, 50];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (CpMedicion48DTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Recurnombre;

                            for (int i = 1; i <= 48; i++)
                            {
                                ws.Cells[index, 2 + i].Value = item.GetType().GetProperty("H" + i).GetValue(item);
                            }

                            rg = ws.Cells[index, 2, index, 50];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 50];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, 50];
                        rg.AutoFitColumns();

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
}