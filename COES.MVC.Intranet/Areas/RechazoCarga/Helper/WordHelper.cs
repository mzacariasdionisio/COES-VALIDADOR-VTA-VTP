using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using Novacode;
using System.Drawing;
using Formato = COES.Framework.Base.Tools.ExtensionMethod;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Helper
{
    public class WordHelper
    {
        public static byte[] GenerarReporteTotalDatos(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];            

            using (DocX document = DocX.Create("ReporteTotalDatos.doc"))
            {
                document.MarginLeft = 114.0f;
                document.MarginRight = 114.0f;

                Paragraph p = document.InsertParagraph();

                p.Append("INTERRUPCION POR RECHAZO CARGA - TOTAL DE DATOS").FontSize(12).Font(new FontFamily("Calibri")).Bold()             
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 2;
                
                Table table = document.InsertTable(rows, 7);
                table.Design = TableDesign.TableGrid;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 3.11M, 3.11M, 2.93M, 1.62M, 1.66M, 1.66M, 1.76M };
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[5].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[6].VerticalAlignment = VerticalAlignment.Center;                                                  

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("SUBESTACIÓN").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("POTENCIA (MW)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[4].Paragraphs[0].Append("INICIO ").FontSize(8).Font(new FontFamily("Calibri")).Bold()
                    .Append("(HH:MM:SS)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[5].Paragraphs[0].Append("FINAL ").FontSize(8).Font(new FontFamily("Calibri")).Bold()
                    .Append("(HH:MM:SS)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[6].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(8).Font(new FontFamily("Calibri")).Bold();

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);
                CentrarCellTableWord(table.Rows[0].Cells[6]);


               

                //table.Rows[0].Cells[0].Width = 20;
                //table.Rows[0].Cells[1].Width = 20;
                //table.Rows[0].Cells[2].Width = 18;
                //table.Rows[0].Cells[3].Width = 9;
                //table.Rows[0].Cells[4].Width = 8;
                //table.Rows[0].Cells[5].Width = 8;
                //table.Rows[0].Cells[6].Width = 10;

                for (int i = 0; i < listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[0]);
                    

                    table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i].Suministrador).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[1]);                    

                    table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(listaCuadroProgUsuario[i].Subestacion).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[2]);
                    

                    table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcejeucargarechazada, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[3]);
                   

                    table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(listaCuadroProgUsuario[i].Rccuadhoriniejec).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[4]);
                   
                    table.Rows[i + 1].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[5].Paragraphs[0].Append(listaCuadroProgUsuario[i].Rccuadhorfinejec).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[5]);
                   

                    table.Rows[i + 1].Cells[6].Width = Convert.ToDouble(anchoColumnas[6]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[6].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Duracion,2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[6]);
                    

                }

                cont = 0;
                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[rows - 1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                table.Rows[rows - 1].MergeCells(0, 2);
                table.Rows[rows - 1].Cells[0].Paragraphs.Last().Remove(false);
                table.Rows[rows - 1].Cells[0].Paragraphs.Last().Remove(false);
                table.Rows[rows - 1].Cells[0].Paragraphs[0].Append("TOTAL").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[rows - 1].Cells[1].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario.Sum(param => param.Rcejeucargarechazada), 2)).FontSize(8).Font(new FontFamily("Calibri")).Bold();

                CentrarCellTableWord(table.Rows[rows - 1].Cells[0]);
                CentrarCellTableWord(table.Rows[rows - 1].Cells[1]);

                table.Rows[rows - 1].MergeCells(2, 4);
                table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);

                Novacode.Border border = new Border();
                border.Tcbs = Novacode.BorderStyle.Tcbs_none;
                table.Rows[rows - 1].Cells[2].SetBorder(TableCellBorderType.Bottom, border);
                table.Rows[rows - 1].Cells[2].SetBorder(TableCellBorderType.Right, border);

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteEvaluacionCumplimiento(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteEvaluacionCumplkimiento.doc"))
            {
                document.MarginLeft = 25.0f;
                document.MarginRight = 25.0f;
                //document.PageLayout.Orientation = Orientation.Landscape;
                Paragraph p = document.InsertParagraph();

                p.Append("EVALUACIÓN DE CUMPLIMIENTO DE LOS RMC").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 4;

                Table table = document.InsertTable(rows, 12);
                table.Design = TableDesign.TableGrid;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 2.11M, 2.26M, 1.4M, 1.6M, 1.6M, 1.45M, 1.6M, 1.6M, 1.45M, 1.6M, 1.6M, 2.06M};
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[2].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[3].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                //table.Rows[0].Cells[0].Width = 15;
                //table.Rows[0].Cells[1].Width = 15;
                //table.Rows[0].Cells[2].Width = 8;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 8;
                //table.Rows[0].Cells[5].Width = 8;
                //table.Rows[0].Cells[6].Width = 8;
                //table.Rows[0].Cells[7].Width = 8;
                //table.Rows[0].Cells[8].Width = 8;
                //table.Rows[0].Cells[9].Width = 8;
                //table.Rows[0].Cells[10].Width = 8;
                //table.Rows[0].Cells[11].Width = 8;               

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[1].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[2].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[3].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);
                CentrarCellTableWord(table.Rows[0].Cells[6]);
                CentrarCellTableWord(table.Rows[0].Cells[7]);
                CentrarCellTableWord(table.Rows[0].Cells[8]);
                CentrarCellTableWord(table.Rows[0].Cells[9]);
                CentrarCellTableWord(table.Rows[0].Cells[10]);
                CentrarCellTableWord(table.Rows[0].Cells[11]);

                CentrarCellTableWord(table.Rows[1].Cells[0]);
                CentrarCellTableWord(table.Rows[1].Cells[1]);
                CentrarCellTableWord(table.Rows[1].Cells[2]);
                CentrarCellTableWord(table.Rows[1].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[4]);
                CentrarCellTableWord(table.Rows[1].Cells[5]);
                CentrarCellTableWord(table.Rows[1].Cells[6]);
                CentrarCellTableWord(table.Rows[1].Cells[7]);
                CentrarCellTableWord(table.Rows[1].Cells[8]);
                CentrarCellTableWord(table.Rows[1].Cells[9]);
                CentrarCellTableWord(table.Rows[1].Cells[10]);
                CentrarCellTableWord(table.Rows[1].Cells[11]);

                CentrarCellTableWord(table.Rows[2].Cells[0]);
                CentrarCellTableWord(table.Rows[2].Cells[1]);
                CentrarCellTableWord(table.Rows[2].Cells[2]);
                CentrarCellTableWord(table.Rows[2].Cells[3]);
                CentrarCellTableWord(table.Rows[2].Cells[4]);
                CentrarCellTableWord(table.Rows[2].Cells[5]);
                CentrarCellTableWord(table.Rows[2].Cells[6]);
                CentrarCellTableWord(table.Rows[2].Cells[7]);
                CentrarCellTableWord(table.Rows[2].Cells[8]);
                CentrarCellTableWord(table.Rows[2].Cells[9]);
                CentrarCellTableWord(table.Rows[2].Cells[10]);
                CentrarCellTableWord(table.Rows[2].Cells[11]);

                CentrarCellTableWord(table.Rows[3].Cells[0]);
                CentrarCellTableWord(table.Rows[3].Cells[1]);
                CentrarCellTableWord(table.Rows[3].Cells[2]);
                CentrarCellTableWord(table.Rows[3].Cells[3]);
                CentrarCellTableWord(table.Rows[3].Cells[4]);
                CentrarCellTableWord(table.Rows[3].Cells[5]);
                CentrarCellTableWord(table.Rows[3].Cells[6]);
                CentrarCellTableWord(table.Rows[3].Cells[7]);
                CentrarCellTableWord(table.Rows[3].Cells[8]);
                CentrarCellTableWord(table.Rows[3].Cells[9]);
                CentrarCellTableWord(table.Rows[3].Cells[10]);
                CentrarCellTableWord(table.Rows[3].Cells[11]);

                table.MergeCellsInColumn(11, 0, 3);
                table.Rows[0].Cells[11].Paragraphs[0].Append("EVALUACIÓN DE RMC EN ENERGÍA").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.MergeCellsInColumn(5, 1, 3);
                table.MergeCellsInColumn(8, 0, 3);

                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[0].MergeCells(2, 4);
                table.Rows[0].Cells[2].Paragraphs[0].Append("COORDINADO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].MergeCells(3, 5);
                table.Rows[0].Cells[3].Paragraphs[0].Append("POTENCIA PROMEDIO                      (PREVIO AL RMC)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].MergeCells(4, 6);
                table.Rows[0].Cells[4].Paragraphs[0].Append("EVALUACIÓN PROMEDIO            POTENCIA EJECUTADO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                //table.Rows[0].Cells[5].Paragraphs[0].Append("EVALUACIÓN DE RMC EN ENERGÍA").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[1].Cells[2].Paragraphs[0].Append("RECHAZO DE CARGA (MW)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[3].Paragraphs[0].Append("HORA").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[5].Paragraphs[0].Append("POTENCIA(MW)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[6].Paragraphs[0].Append("INTERVALO DE MEDICIÓN").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[8].Paragraphs[0].Append("POTENCIA(MW)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[9].Paragraphs[0].Append("INTERVALO DE MEDICIÓN").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[1].MergeCells(3, 4);
                table.Rows[1].MergeCells(5, 6);
                table.Rows[1].MergeCells(7, 8);

                table.Rows[2].Cells[3].Paragraphs[0].Append("INICIO HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[4].Paragraphs[0].Append("FINAL HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[6].Paragraphs[0].Append("HORA").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[9].Paragraphs[0].Append("HORA").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[2].MergeCells(6, 7);
                table.Rows[2].MergeCells(8, 9);

                table.Rows[3].Cells[6].Paragraphs[0].Append("INICIO HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[7].Paragraphs[0].Append("FINAL HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[9].Paragraphs[0].Append("INICIO HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[10].Paragraphs[0].Append("FINAL HH:MM:SS DD.MM.YY").FontSize(7).Font(new FontFamily("Calibri")).Bold();


                table.MergeCellsInColumn(0, 0, 3);
                table.MergeCellsInColumn(1, 0, 3);
               
                table.MergeCellsInColumn(2, 1, 3);
                //table.MergeCellsInColumn(5, 1, 3);
                //table.MergeCellsInColumn(8, 1, 3);

                table.MergeCellsInColumn(3, 2, 3);
                table.MergeCellsInColumn(4, 2, 3);

                table.Rows[0].Cells[2].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[2].Paragraphs.Last().Remove(false);

                table.Rows[0].Cells[3].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[3].Paragraphs.Last().Remove(false);

                table.Rows[0].Cells[4].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[4].Paragraphs.Last().Remove(false);

                table.Rows[1].Cells[3].Paragraphs.Last().Remove(false);
                table.Rows[1].Cells[5].Paragraphs.Last().Remove(false);
                table.Rows[1].Cells[7].Paragraphs.Last().Remove(false);

                table.Rows[2].Cells[6].Paragraphs.Last().Remove(false);
                table.Rows[2].Cells[8].Paragraphs.Last().Remove(false);

                //table.Rows[0].Cells[6].Width = 8;

                //var total = decimal.Zero;

                for (int i = 0; i < listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 4].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i].Empresa).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[0]);

                    table.Rows[i + 4].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i].Suministrador).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[1]);

                    table.Rows[i + 4].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[2].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcproucargarechazarcoord,2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[2]);

                    table.Rows[i + 4].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechoriniRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[3]);

                    table.Rows[i + 4].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[4].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechorfinRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[4]);

                    table.Rows[i + 4].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[5].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].RcreevpotenciaprompreviaRep,2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[5]);

                    table.Rows[i + 4].Cells[6].Width = Convert.ToDouble(anchoColumnas[6]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[6].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechoriniPrevioRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[6]);

                    table.Rows[i + 4].Cells[7].Width = Convert.ToDouble(anchoColumnas[7]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[7].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechorfinPrevioRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[7]);

                    table.Rows[i + 4].Cells[8].Width = Convert.ToDouble(anchoColumnas[8]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[8].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcejeucargarechazada,2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[8]);

                    table.Rows[i + 4].Cells[9].Width = Convert.ToDouble(anchoColumnas[9]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[9].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorinicioRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[9]);

                    table.Rows[i + 4].Cells[10].Width = Convert.ToDouble(anchoColumnas[10]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[10].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorfinRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[10]);

                    table.Rows[i + 4].Cells[11].Width = Convert.ToDouble(anchoColumnas[11]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[11].Paragraphs[0].Append(listaCuadroProgUsuario[i].Evaluacion.Replace("CUMPLIO", "CUMPLIÓ")).FontSize(7).Font(new FontFamily("Calibri")).Bold();
                    CentrarCellTableWord(table.Rows[i + 4].Cells[11]);
                    
                }

              

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteInterInformeTecnico(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteEvaluacionCumplkimiento.doc"))
            {
                document.MarginLeft = 50.0f;
                document.MarginRight = 50.0f;
                //document.PageLayout.Orientation = Orientation.Landscape;
                Paragraph p = document.InsertParagraph();

                p.Append("INTERRUPCIONES DE SUMINISTRO POR RMC PARA EL INFORME TÉCNICO").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 4;

                Table table = document.InsertTable(rows, 10);
                table.Design = TableDesign.TableGrid;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 1.64M, 2.28M, 1.91M, 1.6M, 1.84M, 1.84M, 1.6M, 1.84M, 1.84M, 1.54M};
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[2].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }
                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[3].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                //table.Rows[0].Cells[0].Width = 15;
                //table.Rows[0].Cells[1].Width = 15;
                //table.Rows[0].Cells[2].Width = 15;
                //table.Rows[0].Cells[3].Width = 7;
                //table.Rows[0].Cells[4].Width = 7;
                //table.Rows[0].Cells[5].Width = 7;
                //table.Rows[0].Cells[6].Width = 7;
                //table.Rows[0].Cells[7].Width = 7;
                //table.Rows[0].Cells[8].Width = 7;
                //table.Rows[0].Cells[9].Width = 7;
                //table.Rows[0].Cells[10].Width = 8;
                //table.Rows[0].Cells[11].Width = 8;

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[0].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[0].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[1].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[1].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[1].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[2].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[2].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[2].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[2].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                table.Rows[3].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[7].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[8].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[3].Cells[9].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[3].Cells[10].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                //table.Rows[3].Cells[11].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);
                CentrarCellTableWord(table.Rows[0].Cells[6]);
                CentrarCellTableWord(table.Rows[0].Cells[7]);
                CentrarCellTableWord(table.Rows[0].Cells[8]);
                CentrarCellTableWord(table.Rows[0].Cells[9]);
                //CentrarCellTableWord(table.Rows[0].Cells[10]);
                //CentrarCellTableWord(table.Rows[0].Cells[11]);

                CentrarCellTableWord(table.Rows[1].Cells[0]);
                CentrarCellTableWord(table.Rows[1].Cells[1]);
                CentrarCellTableWord(table.Rows[1].Cells[2]);
                CentrarCellTableWord(table.Rows[1].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[4]);
                CentrarCellTableWord(table.Rows[1].Cells[5]);
                CentrarCellTableWord(table.Rows[1].Cells[6]);
                CentrarCellTableWord(table.Rows[1].Cells[7]);
                CentrarCellTableWord(table.Rows[1].Cells[8]);
                CentrarCellTableWord(table.Rows[1].Cells[9]);
                //CentrarCellTableWord(table.Rows[1].Cells[10]);
                //CentrarCellTableWord(table.Rows[1].Cells[11]);

                CentrarCellTableWord(table.Rows[2].Cells[0]);
                CentrarCellTableWord(table.Rows[2].Cells[1]);
                CentrarCellTableWord(table.Rows[2].Cells[2]);
                CentrarCellTableWord(table.Rows[2].Cells[3]);
                CentrarCellTableWord(table.Rows[2].Cells[4]);
                CentrarCellTableWord(table.Rows[2].Cells[5]);
                CentrarCellTableWord(table.Rows[2].Cells[6]);
                CentrarCellTableWord(table.Rows[2].Cells[7]);
                CentrarCellTableWord(table.Rows[2].Cells[8]);
                CentrarCellTableWord(table.Rows[2].Cells[9]);
                //CentrarCellTableWord(table.Rows[2].Cells[10]);
                //CentrarCellTableWord(table.Rows[2].Cells[11]);

                CentrarCellTableWord(table.Rows[3].Cells[0]);
                CentrarCellTableWord(table.Rows[3].Cells[1]);
                CentrarCellTableWord(table.Rows[3].Cells[2]);
                CentrarCellTableWord(table.Rows[3].Cells[3]);
                CentrarCellTableWord(table.Rows[3].Cells[4]);
                CentrarCellTableWord(table.Rows[3].Cells[5]);
                CentrarCellTableWord(table.Rows[3].Cells[6]);
                CentrarCellTableWord(table.Rows[3].Cells[7]);
                CentrarCellTableWord(table.Rows[3].Cells[8]);
                CentrarCellTableWord(table.Rows[3].Cells[9]);
                //CentrarCellTableWord(table.Rows[3].Cells[10]);
                //CentrarCellTableWord(table.Rows[3].Cells[11]);

                table.MergeCellsInColumn(3, 1, 3);
                //table.Rows[0].Cells[11].Paragraphs[0].Append("EVALUACIÓN DE RMC EN ENERGÍA").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.MergeCellsInColumn(6, 1, 3);
                table.MergeCellsInColumn(9, 1, 3);

                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("SUBESTACION").FontSize(7).Font(new FontFamily("Calibri")).Bold();


                table.Rows[0].MergeCells(3, 5);
                table.Rows[0].Cells[3].Paragraphs[0].Append("COORDINADO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].MergeCells(4, 7);
                table.Rows[0].Cells[4].Paragraphs[0].Append("EJECUTADO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                //table.Rows[0].MergeCells(4, 6);
                //table.Rows[0].Cells[4].Paragraphs[0].Append("EVALUACIÓN PROMEDIO POTENCIA EJECUTADO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                //table.Rows[0].Cells[5].Paragraphs[0].Append("EVALUACIÓN DE RMC EN ENERGÍA").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[1].Cells[3].Paragraphs[0].Append("RECHAZO DE CARGA (MW)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[4].Paragraphs[0].Append("HORA").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[6].Paragraphs[0].Append("RECHAZO DE CARGA (MW)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[7].Paragraphs[0].Append("HORA").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[9].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                //table.Rows[1].Cells[9].Paragraphs[0].Append("INTERVALO DE MEDICIÓN").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[1].MergeCells(4, 5);
                table.Rows[1].MergeCells(6, 7);
                //table.Rows[1].MergeCells(7, 8);

                table.Rows[2].Cells[4].Paragraphs[0].Append("INICIO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[5].Paragraphs[0].Append("FINAL").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[7].Paragraphs[0].Append("INICIO").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[2].Cells[8].Paragraphs[0].Append("FINAL").FontSize(7).Font(new FontFamily("Calibri")).Bold();

                table.Rows[3].Cells[4].Paragraphs[0].Append("(HH:MM:SS) DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[5].Paragraphs[0].Append("(HH:MM:SS) DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[7].Paragraphs[0].Append("(HH:MM:SS) DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[3].Cells[8].Paragraphs[0].Append("(HH:MM:SS) DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold();


                table.MergeCellsInColumn(0, 0, 3);
                table.MergeCellsInColumn(1, 0, 3);
                table.MergeCellsInColumn(2, 0, 3);              

                table.Rows[0].Cells[3].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[3].Paragraphs.Last().Remove(false);

                table.Rows[0].Cells[4].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[4].Paragraphs.Last().Remove(false);
                table.Rows[0].Cells[4].Paragraphs.Last().Remove(false);               

                table.Rows[1].Cells[4].Paragraphs.Last().Remove(false);
                table.Rows[1].Cells[6].Paragraphs.Last().Remove(false);
              

                //var total = decimal.Zero;

                for (int i = 0; i < listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 4].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i].Empresa).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[0]);

                    table.Rows[i + 4].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i].Suministrador).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[1]);

                    table.Rows[i + 4].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[2].Paragraphs[0].Append(listaCuadroProgUsuario[i].Subestacion).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[2]);

                    table.Rows[i + 4].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[3].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcproucargarechazarcoord, 2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[3]);

                    table.Rows[i + 4].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[4].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechoriniRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[4]);

                    table.Rows[i + 4].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[5].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechorfinRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[5]);

                    table.Rows[i + 4].Cells[6].Width = Convert.ToDouble(anchoColumnas[6]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[6].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcejeucargarechazada, 2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[6]);

                    table.Rows[i + 4].Cells[7].Width = Convert.ToDouble(anchoColumnas[7]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[7].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorinicioRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[7]);

                    table.Rows[i + 4].Cells[8].Width = Convert.ToDouble(anchoColumnas[8]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[8].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorfinRep).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[8]);
                    
                    table.Rows[i + 4].Cells[9].Width = Convert.ToDouble(anchoColumnas[9]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 4].Cells[9].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Duracion, 2)).FontSize(7).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 4].Cells[9]);
                   
                }



                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteDemorasEjecucionRMC(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteDemorasEjecucion.doc"))
            {

                Paragraph p = document.InsertParagraph();

                p.Append("DEMORAS EN LA EJECUCIÓN DEL RMC").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 2;

                Table table = document.InsertTable(rows, 5);
                table.Design = TableDesign.TableGrid;
                //table.AutoFit = AutoFit.Contents;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 3.25M, 3.25M, 2.5M, 2.5M, 2.08M };
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                cont = 0;
                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;      
                               
                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("COORDINADO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("EJECUTADO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[2].Paragraphs[0].Append("INICIO  HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[3].Paragraphs[0].Append("INICIO  HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[4].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(8).Font(new FontFamily("Calibri")).Bold();

                table.MergeCellsInColumn(0, 0, 1);
                table.MergeCellsInColumn(1, 0, 1);
                table.MergeCellsInColumn(4, 0, 1);

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[2]);
                CentrarCellTableWord(table.Rows[1].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);

                //table.Rows[0].Cells[0].Width = 16;
                //table.Rows[0].Cells[1].Width = 16;
                //table.Rows[0].Cells[2].Width = 8;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 7;               


                for (int i = 1; i <= listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[0]);

                    table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Suministrador).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[1]);

                    table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Rccuadhorinicoord).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[2]);

                    table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Rccuadhoriniejec).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[3]);

                    table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i - 1].Duracion, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[4]);
                }
                              
                //table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                //table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteDemorasReestablecimiento(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteDemorasReestablecimiento.doc"))
            {

                Paragraph p = document.InsertParagraph();

                p.Append("DEMORAS PARA EL REESTABLECIMIENTO").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 2;

                Table table = document.InsertTable(rows, 5);
                table.Design = TableDesign.TableGrid;
                //table.AutoFit = AutoFit.Contents;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 3.25M, 3.25M, 2.5M, 2.5M, 2.08M };
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                cont = 0;
                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;

                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("COORDINADO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("EJECUTADO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[2].Paragraphs[0].Append("FINAL   HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[3].Paragraphs[0].Append("FINAL   HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[4].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(8).Font(new FontFamily("Calibri")).Bold();

                table.MergeCellsInColumn(0, 0, 1);
                table.MergeCellsInColumn(1, 0, 1);
                table.MergeCellsInColumn(4, 0, 1);

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[2]);
                CentrarCellTableWord(table.Rows[1].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);

                //table.Rows[0].Cells[0].Width = 16;
                //table.Rows[0].Cells[1].Width = 16;
                //table.Rows[0].Cells[2].Width = 8;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 7;


                for (int i = 1; i <= listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[0]);

                    table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Suministrador).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[1]);

                    table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Rccuadhorfincoord).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[2]);

                    table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Rccuadhorfinejec).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[3]);

                    table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i - 1].Duracion, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[4]);
                }

                //table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                //table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteInterrupcionesMenores(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteInterrupcionesMenores.doc"))
            {

                Paragraph p = document.InsertParagraph();

                p.Append("INTERRUPCIONES MENORES 3 MINUTOS").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count > 0 ? listaCuadroProgUsuario.Count + 3 : 2;

                Table table = document.InsertTable(rows, 6);
                table.Design = TableDesign.TableGrid;
                //table.AutoFit = AutoFit.Contents;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 3.25M, 3.74M, 2.01M, 2.5M, 2.5M, 1.74M };
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[5].VerticalAlignment = VerticalAlignment.Center;

                table.Rows[0].Cells[0].Paragraphs[0].Append("USUARIO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRADOR").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("POTENCIA (MW)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("EJECUTADO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[3].Paragraphs[0].Append("INICIO  HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[1].Cells[4].Paragraphs[0].Append("FINAL   HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[5].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(8).Font(new FontFamily("Calibri")).Bold();

                table.MergeCellsInColumn(0, 0, 1);
                table.MergeCellsInColumn(1, 0, 1);
                table.MergeCellsInColumn(2, 0, 1);
                table.MergeCellsInColumn(5, 0, 1);

                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[1].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[3]);
                CentrarCellTableWord(table.Rows[1].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);

                //table.Rows[0].Cells[0].Width = 16;
                //table.Rows[0].Cells[1].Width = 16;
                //table.Rows[0].Cells[2].Width = 10;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 8;
                //table.Rows[0].Cells[4].Width = 7;

                table.Rows[0].MergeCells(3, 4);
                table.Rows[0].Cells[3].Paragraphs.Last().Remove(false);

                if (listaCuadroProgUsuario.Count > 0)
                {
                    for (int i = 1; i <= listaCuadroProgUsuario.Count; i++)
                    {
                        table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[0]);

                        table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].Suministrador).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[1]);

                        table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[2].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i - 1].Rcejeucargarechazada, 3)).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[2]);

                        table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].RcejeufechorinicioRep).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[3]);

                        table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[4].Paragraphs[0].Append(listaCuadroProgUsuario[i - 1].RcejeufechorfinRep).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[4]);

                        table.Rows[i + 1].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                        table.Rows[i + 1].Cells[5].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i - 1].Duracion, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                        CentrarCellTableWord(table.Rows[i + 1].Cells[5]);
                    }                    

                    cont = 0;

                    foreach (var ancho in anchoColumnas)
                    {
                        //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                        table.Rows[rows - 1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                        cont++;
                    }

                    table.Rows[rows - 1].MergeCells(0, 1);
                    table.Rows[rows - 1].Cells[0].Paragraphs.Last().Remove(false);
                    table.Rows[rows - 1].Cells[0].Paragraphs.Last().Remove(false);
                    table.Rows[rows - 1].Cells[0].Paragraphs[0].Append("TOTAL").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                    table.Rows[rows - 1].Cells[1].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario.Sum(param => param.Rcejeucargarechazada), 3)).FontSize(8).Font(new FontFamily("Calibri")).Bold();

                    CentrarCellTableWord(table.Rows[rows - 1].Cells[0]);
                    CentrarCellTableWord(table.Rows[rows - 1].Cells[1]);

                    table.Rows[rows - 1].MergeCells(2, 4);
                    //table.Rows[rows - 1].Cells.RemoveRange(2, 1);
                    table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                    table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);

                    Novacode.Border border = new Border();
                    border.Tcbs = Novacode.BorderStyle.Tcbs_none;
                    table.Rows[rows - 1].Cells[2].SetBorder(TableCellBorderType.Bottom, border);
                    table.Rows[rows - 1].Cells[2].SetBorder(TableCellBorderType.Right, border);
                }
               

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteInterrupcionesSuministroDecision(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteInterrupcioneSuministroDecision.doc"))
            {

                Paragraph p = document.InsertParagraph();

                p.Append("INTERRUPCIONES DE SUMINISTRO POR RMC PARA LA DECISIÓN").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count + 1;

                Table table = document.InsertTable(rows, 6);
                table.Design = TableDesign.TableGrid;
                //table.AutoFit = AutoFit.Contents;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 1.21M, 8.26M, 1.59M, 1.78M, 1.78M, 1.78M };
                var contCol = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[contCol].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    contCol++;
                }

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[5].VerticalAlignment = VerticalAlignment.Center;

                table.Rows[0].Cells[0].Paragraphs[0].Append("N°").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("SUMINISTRO").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("POTENCIA (MW)").FontSize(8).Font(new FontFamily("Calibri")).Bold();                
                //table.Rows[0].Cells[3].Paragraphs[0].Append("INICIO HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                //table.Rows[0].Cells[4].Paragraphs[0].Append("FINAL HH:MM:SS DD.MM.YYYY").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("INICIO ").FontSize(8).Font(new FontFamily("Calibri")).Bold().
                    Append("HH:MM:SS DD.MM.YYYY").FontSize(Convert.ToDouble("7")).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[4].Paragraphs[0].Append("FINAL ").FontSize(8).Font(new FontFamily("Calibri")).Bold().
                    Append("HH:MM:SS DD.MM.YYYY").FontSize(Convert.ToDouble("7")).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[5].Paragraphs[0].Append("DURACIÓN (MIN)").FontSize(8).Font(new FontFamily("Calibri")).Bold();

              
                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");              
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);               
                CentrarCellTableWord(table.Rows[0].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);

                //table.Rows[0].Cells[0].Width = 5;
                //table.Rows[0].Cells[1].Width = 16;
                //table.Rows[0].Cells[2].Width = 8;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 8;
                //table.Rows[0].Cells[5].Width = 7;

                var cont = 1;
                for (int i = 0; i < listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(cont.ToString()).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[0]);

                    table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(listaCuadroProgUsuario[i].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[1]);

                    table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcejeucargarechazada, 3)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[2]);

                    table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorinicioRep).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[3]);

                    table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechorfinRep).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[4]);

                    table.Rows[i + 1].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[5].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Duracion, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[5]);

                    cont++;
                }              

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteInterrupcionesResarcimiento(List<RcaCuadroProgUsuarioDTO> listaCuadroProgUsuario, string eventoCTAF)
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteInterrupcioneResarcimiento.doc"))
            {
                document.MarginLeft = 54.0f;
                document.MarginRight = 54.0f;

                Paragraph p = document.InsertParagraph();

                p.Append("INTERRUPCIONES POR RMC PARA EL RESARCIMIENTO").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;

                Paragraph p1 = document.InsertParagraph();
                p1.Append("EVENTO CTAF: ").FontSize(12).Font(new FontFamily("Calibri")).Bold().Append(eventoCTAF).FontSize(12).Font(new FontFamily("Calibri"));
                p1.Alignment = Alignment.left;

                document.InsertParagraph("\n");

                var rows = listaCuadroProgUsuario.Count > 0 ? listaCuadroProgUsuario.Count + 2 : 1;
                                
                Table table = document.InsertTable(rows, 8);
                table.Design = TableDesign.TableGrid;
                //table.AutoFit = AutoFit.ColumnWidth;
                table.AutoFit = AutoFit.ColumnWidth;

                var anchoColumnas = new decimal[] { 5.49M, 2.4M, 1.78M, 1.78M, 1.72M, 1.68M, 2.4M, 1.3M };
                var cont = 0;

                foreach (var ancho in anchoColumnas)
                {
                    //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                    table.Rows[0].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                    cont++;
                }


                Novacode.Formatting textRed = new Novacode.Formatting();
                textRed.FontColor = Color.Red;
                //textRed.FontFamily.si
                textRed.FontFamily = new FontFamily("Calibri");

                table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[4].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[5].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[0].Cells[6].VerticalAlignment = VerticalAlignment.Center;

                table.Rows[0].Cells[0].Paragraphs[0].Append("SUMINISTROS AFECTADOS").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("POTENCIA INTERRUMPIDA (MW)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("\n(A)").FontSize(8).Font(new FontFamily("Calibri")).Bold().Color(Color.Red);
                //table.Rows[0].Cells[1].Paragraphs[0].InsertText("\n(A)", false, textRed);
                table.Rows[0].Cells[2].Paragraphs[0].Append("HORA INICIO ").FontSize(8).Font(new FontFamily("Calibri")).Bold()
                    .Append("HH:MM:SS DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold(); ;
                table.Rows[0].Cells[3].Paragraphs[0].Append("HORA FINAL ").FontSize(8).Font(new FontFamily("Calibri")).Bold()
                    .Append("HH:MM:SS DD.MM.YYYY").FontSize(7).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[4].Paragraphs[0].Append("TIEMPO DURACIÓN (MINUTOS)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[5].Paragraphs[0].Append("TIEMPO DURACIÓN (HORAS)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[5].Paragraphs[0].Append("\n(B)").FontSize(8).Font(new FontFamily("Calibri")).Bold().Color(Color.Red);
                table.Rows[0].Cells[6].Paragraphs[0].Append("ENERGÍA NO SUMINISTRADA (MWH)").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                table.Rows[0].Cells[6].Paragraphs[0].Append("\n(AXB)").FontSize(8).Font(new FontFamily("Calibri")).Bold().Color(Color.Red);


                table.Rows[0].Cells[0].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[1].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[2].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[3].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[4].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[5].FillColor = ColorTranslator.FromHtml("#BFBFBF");
                table.Rows[0].Cells[6].FillColor = ColorTranslator.FromHtml("#BFBFBF");

                CentrarCellTableWord(table.Rows[0].Cells[0]);
                CentrarCellTableWord(table.Rows[0].Cells[1]);
                CentrarCellTableWord(table.Rows[0].Cells[2]);
                CentrarCellTableWord(table.Rows[0].Cells[3]);
                CentrarCellTableWord(table.Rows[0].Cells[4]);
                CentrarCellTableWord(table.Rows[0].Cells[5]);
                CentrarCellTableWord(table.Rows[0].Cells[6]);

                table.Rows[0].Cells[0].MarginLeft = 0;
                table.Rows[0].Cells[0].MarginRight = 0;

                //table.Rows[0].Cells[0].Width = 20;
                //table.Rows[0].Cells[1].Width = 8;
                //table.Rows[0].Cells[2].Width = 8;
                //table.Rows[0].Cells[3].Width = 8;
                //table.Rows[0].Cells[4].Width = 8;
                //table.Rows[0].Cells[5].Width = 8;
                //table.Rows[0].Cells[6].Width = 8;

                var total = decimal.Zero;

                Novacode.Border border = new Border();
                border.Tcbs = Novacode.BorderStyle.Tcbs_none;

                for (int i = 0; i < listaCuadroProgUsuario.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Width = Convert.ToDouble(anchoColumnas[0]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(listaCuadroProgUsuario[i].Empresa).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[0]);

                    table.Rows[i + 1].Cells[1].Width = Convert.ToDouble(anchoColumnas[1]) / Convert.ToDouble(FactorAnchoColumWord);                    
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Rcejeucargarechazada,3)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[1]);

                    table.Rows[i + 1].Cells[2].Width = Convert.ToDouble(anchoColumnas[2]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(listaCuadroProgUsuario[i].RcejeufechorinicioRep).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[2]);

                    table.Rows[i + 1].Cells[3].Width = Convert.ToDouble(anchoColumnas[3]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(listaCuadroProgUsuario[i].RccuadfechorfinRep).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[3]);

                    table.Rows[i + 1].Cells[4].Width = Convert.ToDouble(anchoColumnas[4]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(Formato.FormatoDecimal(listaCuadroProgUsuario[i].Duracion,2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[4]);

                    var duracionHoras = decimal.Round(listaCuadroProgUsuario[i].Duracion / 60, 2);

                    table.Rows[i + 1].Cells[5].Width = Convert.ToDouble(anchoColumnas[5]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[5].Paragraphs[0].Append(Formato.FormatoDecimal(duracionHoras, 2)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[5]);

                    table.Rows[i + 1].Cells[6].Width = Convert.ToDouble(anchoColumnas[6]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[6].Paragraphs[0].Append(Formato.FormatoDecimal(duracionHoras * listaCuadroProgUsuario[i].Rcejeucargarechazada, 3)).FontSize(8).Font(new FontFamily("Calibri"));
                    CentrarCellTableWord(table.Rows[i + 1].Cells[6]);

                    table.Rows[i + 1].Cells[7].Width = Convert.ToDouble(anchoColumnas[7]) / Convert.ToDouble(FactorAnchoColumWord);
                    table.Rows[i + 1].Cells[7].SetBorder(TableCellBorderType.Right, border);

                    total = total + duracionHoras * listaCuadroProgUsuario[i].Rcejeucargarechazada;
                }

                if(listaCuadroProgUsuario.Count > 0)
                {
                    cont = 0;
                    foreach (var ancho in anchoColumnas)
                    {
                        //Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                        table.Rows[rows - 1].Cells[cont].Width = Convert.ToDouble(ancho) / Convert.ToDouble(FactorAnchoColumWord);
                        cont++;
                    }


                    var totalMW = Formato.FormatoDecimal(listaCuadroProgUsuario.Sum(param => param.Rcejeucargarechazada), 3);
                    table.Rows[rows - 1].Cells[0].Paragraphs[0].Append("TOTAL (MW)--->").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                    table.Rows[rows - 1].Cells[1].Paragraphs[0].Append(totalMW).FontSize(8).Font(new FontFamily("Calibri")).Bold();

                    DerechaCellTableWord(table.Rows[rows - 1].Cells[0]);
                    CentrarCellTableWord(table.Rows[rows - 1].Cells[1]);

                    table.Rows[rows - 1].MergeCells(2, 4);
                    table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                    table.Rows[rows - 1].Cells[2].Paragraphs.Last().Remove(false);
                    table.Rows[rows - 1].Cells[2].SetBorder(TableCellBorderType.Bottom, border);

                    var totalFormato = Formato.FormatoDecimal(total, 3);
                    table.Rows[rows - 1].Cells[3].Paragraphs[0].Append("ENSF ---->").FontSize(8).Font(new FontFamily("Calibri")).Bold();
                    //table.Rows[rows - 1].Cells[4].Paragraphs[0].Append(total.ToString("###,##0.#0")).FontSize(8).Font(new FontFamily("Calibri")).Bold();
                    table.Rows[rows - 1].Cells[4].Paragraphs[0].Append(totalFormato).FontSize(8).Font(new FontFamily("Calibri")).Bold();
                    CentrarCellTableWord(table.Rows[rows - 1].Cells[4]);

                    table.Rows[rows - 1].Cells[5].Paragraphs[0].Append("MWH").FontSize(8).Font(new FontFamily("Calibri")).Bold();

                    table.MergeCellsInColumn(7, 0, rows - 2);

                    table.Rows[0].Cells[7].SetBorder(TableCellBorderType.Top, border);
                    table.Rows[0].Cells[7].SetBorder(TableCellBorderType.Right, border);

                }
                else
                {
                    table.Rows[0].Cells[7].SetBorder(TableCellBorderType.Top, border);
                    table.Rows[0].Cells[7].SetBorder(TableCellBorderType.Right, border);
                    table.Rows[0].Cells[7].SetBorder(TableCellBorderType.Bottom, border);
                    
                }


                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        public static byte[] GenerarReporteBlanco()
        {

            byte[] bytes = new byte[0];

            //var archivoExcel = "ReporteTotalDatos.xlsx";

            using (DocX document = DocX.Create("ReporteTotalDatos.doc"))
            {

                Paragraph p = document.InsertParagraph();

                p.Append("").FontSize(12).Font(new FontFamily("Calibri")).Bold()
                    .AppendLine()
                ;
                p.Alignment = Alignment.center;               

                MemoryStream ms = new MemoryStream();
                document.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
        }

        /// <summary>
        /// Centrar celda table Word
        /// </summary>
        /// <param name="cell"></param>
        private static void CentrarCellTableWord(Cell cell)
        {
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Paragraphs[0].Alignment = Alignment.center;
        }

        private static void DerechaCellTableWord(Cell cell)
        {
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Paragraphs[0].Alignment = Alignment.right;
        }


        

        public const decimal FactorAnchoColumWord = 0.02651m; // 0.0368m;
        public const decimal FactorAltoFilaWord = 0.02651m;
    }
}