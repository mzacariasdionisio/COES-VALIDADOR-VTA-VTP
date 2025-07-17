using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper
{
    public class WordDocument
    {
        /// <summary>
        /// Permite generar el reporte de cumplimiento en formato Word
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCumplimiento(List<ServicioRpfDTO> list, string path, DateTime fecha)
        {
            Grafico grafico = new Grafico();

            using (DocX document = DocX.Create(path + Constantes.NombreReporteRPFWord))
            {
                Novacode.Image logo = document.AddImage(path + Constantes.NombreLogoCoes);
                document.AddHeaders();
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header = document.Headers.odd;
                header.RemoveParagraph(header.Paragraphs[0]);

                Table tbHeader = header.InsertTable(2, 3);
                tbHeader.Design = TableDesign.TableGrid;
                tbHeader.AutoFit = AutoFit.ColumnWidth;


                Paragraph txtHeader = header.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                txtHeader.AppendPicture(logo.CreatePicture());
                txtHeader.Alignment = Alignment.center;

                tbHeader.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                tbHeader.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                tbHeader.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                tbHeader.Rows[0].Cells[0].Width = 170;
                tbHeader.MergeCellsInColumn(0, 0, 1);

                Paragraph cabecera = header.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                cabecera.Append("REPORTE DE CUMPLIMIENTO RPF");
                cabecera.Alignment = Alignment.center;
                cabecera.FontSize(13);
                cabecera.Bold();

                tbHeader.Rows[0].Cells[1].Width = 300;
                tbHeader.MergeCellsInColumn(1, 0, 1);

                tbHeader.Rows[0].Cells[2].Width = 100;

                cabecera = header.Tables[0].Rows[0].Cells[2].Paragraphs[0];
                cabecera.Append(String.Format("{0}", fecha.ToString("dd/MM/yyyy")));
                cabecera.Alignment = Alignment.center;

                cabecera = header.Tables[0].Rows[1].Cells[2].Paragraphs[0];
                cabecera.AppendPageNumber(PageNumberFormat.normal);
                cabecera.Alignment = Alignment.center;

                tbHeader.Alignment = Alignment.center;

                header.InsertParagraph("");

                document.MarginTop = 56.8f;
                document.MarginBottom = 84.06f;
                document.MarginRight = 92.02f;

                Paragraph titulo = document.InsertParagraph();
                titulo.Append("REPORTE DE CUMPLIMIENTO RPF\n");
                titulo.FontSize(14);
                titulo.Alignment = Alignment.center;
                titulo.Bold();

                Table reporte = document.InsertTable(list.Count + 1, 8);

                reporte.AutoFit = AutoFit.Window;

                reporte.Rows[0].Cells[0].Paragraphs[0].Append("EMPRESA");
                reporte.Rows[0].Cells[1].Paragraphs[0].Append("CENTRAL");
                reporte.Rows[0].Cells[2].Paragraphs[0].Append("UNIDAD");
                reporte.Rows[0].Cells[3].Paragraphs[0].Append("PTO MEDICIÓN");
                reporte.Rows[0].Cells[4].Paragraphs[0].Append("HORA INICIO");
                reporte.Rows[0].Cells[5].Paragraphs[0].Append("HORA FIN");
                reporte.Rows[0].Cells[6].Paragraphs[0].Append("PORCENTAJE");
                reporte.Rows[0].Cells[7].Paragraphs[0].Append("CUMPLIMIENTO");

                reporte.Design = TableDesign.TableGrid;

                reporte.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[7].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[7].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[0].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[1].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[2].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[3].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[4].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[5].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[6].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[7].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[0].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[1].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[2].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[3].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[4].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[5].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[6].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[7].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[0].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[1].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[2].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[3].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[4].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[5].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[6].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[7].Paragraphs[0].FontSize(8);

                int index = 1;
                foreach (ServicioRpfDTO item in list)
                {
                    reporte.Rows[index].Cells[0].Paragraphs[0].Append(item.EMPRNOMB);
                    reporte.Rows[index].Cells[1].Paragraphs[0].Append(item.EQUINOMB);
                    reporte.Rows[index].Cells[2].Paragraphs[0].Append(item.EQUIABREV);
                    reporte.Rows[index].Cells[3].Paragraphs[0].Append(item.PTOMEDICODI.ToString());
                    reporte.Rows[index].Cells[4].Paragraphs[0].Append(item.HORAINICIO);
                    reporte.Rows[index].Cells[5].Paragraphs[0].Append(item.HORAFIN);
                    reporte.Rows[index].Cells[6].Paragraphs[0].Append(item.PORCENTAJE.ToString(Constantes.FormatoNumero) + Constantes.CaracterPorcentaje);
                    reporte.Rows[index].Cells[7].Paragraphs[0].Append(item.INDCUMPLIMIENTO);

                    reporte.Rows[index].Cells[0].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[1].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[2].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[3].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[4].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[5].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[6].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[7].Paragraphs[0].FontSize(8);

                    index++;
                }

                document.InsertSectionPageBreak();
                try
                {
                    int numeral = 1;
                    foreach (ServicioRpfDTO item in list)
                    {
                        if (item.ListaSerie != null && item.ListaSuperior != null && item.ListaInferior != null && item.ListaPuntos != null)
                        {
                            Paragraph parrafo = document.InsertParagraph();
                            parrafo.Append(numeral.ToString() + ". " + item.EMPRNOMB.Trim() + " " + item.EQUINOMB.Trim() + " - " +
                                item.EQUIABREV.Trim() + " (" + item.PORCENTAJE.ToString(Constantes.FormatoNumero) + "%)\n");
                            parrafo.Bold();

                            grafico.GenerarGrafico(item.ListaSerie, item.ListaSuperior, item.ListaInferior, item.ListaPuntos, path);

                            Novacode.Image chart = document.AddImage(path + Constantes.NombreChartRPF);

                            Paragraph celda = document.InsertParagraph();
                            celda.AppendPicture(chart.CreatePicture());
                            celda.Alignment = Alignment.center;

                            Paragraph separacion = document.InsertParagraph();
                            separacion.Append("\n\n");

                            numeral++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

                document.Save();
            }
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento en formato Word
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCumplimientoFalla(List<ServicioRpfDTO> list, string path, DateTime fecha, DateTime fechaProceso)
        {
            Grafico grafico = new Grafico();

            using (DocX document = DocX.Create(path + Constantes.NombreReporteRPFFallaWord))
            {
                Novacode.Image logo = document.AddImage(path + Constantes.NombreLogoCoes);
                document.AddHeaders();
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header = document.Headers.odd;
                header.RemoveParagraph(header.Paragraphs[0]);

                Table tbHeader = header.InsertTable(2, 3);
                tbHeader.Design = TableDesign.TableGrid;
                tbHeader.AutoFit = AutoFit.ColumnWidth;

                Paragraph txtHeader = header.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                txtHeader.AppendPicture(logo.CreatePicture());
                txtHeader.Alignment = Alignment.center;

                tbHeader.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                tbHeader.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                tbHeader.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                tbHeader.Rows[0].Cells[0].Width = 170;
                tbHeader.MergeCellsInColumn(0, 0, 1);

                Paragraph cabecera = header.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                cabecera.Append("REPORTE DE CUMPLIMIENTO ANTE FALLA");
                cabecera.Alignment = Alignment.center;
                cabecera.FontSize(13);
                cabecera.Bold();

                tbHeader.Rows[0].Cells[1].Width = 300;
                tbHeader.MergeCellsInColumn(1, 0, 1);

                tbHeader.Rows[0].Cells[2].Width = 100;

                cabecera = header.Tables[0].Rows[0].Cells[2].Paragraphs[0];
                cabecera.Append(String.Format("{0}", fecha.ToString("dd/MM/yyyy")));
                cabecera.Alignment = Alignment.center;

                cabecera = header.Tables[0].Rows[1].Cells[2].Paragraphs[0];
                cabecera.AppendPageNumber(PageNumberFormat.normal);
                cabecera.Alignment = Alignment.center;

                tbHeader.Alignment = Alignment.center;

                header.InsertParagraph("");

                document.MarginTop = 56.8f;
                document.MarginBottom = 84.06f;
                document.MarginRight = 92.02f;

                Paragraph titulo = document.InsertParagraph();
                titulo.Append("REPORTE DE CUMPLIMIENTO ANTE FALLAS\n");
                titulo.FontSize(14);
                titulo.Alignment = Alignment.center;
                titulo.Bold();

                Table reporte = document.InsertTable(list.Count + 1, 6);

                reporte.AutoFit = AutoFit.Window;

                reporte.Rows[0].Cells[0].Paragraphs[0].Append("EMPRESA");
                reporte.Rows[0].Cells[1].Paragraphs[0].Append("CENTRAL");
                reporte.Rows[0].Cells[2].Paragraphs[0].Append("UNIDAD");
                reporte.Rows[0].Cells[3].Paragraphs[0].Append("PTO MEDICIÓN");
                reporte.Rows[0].Cells[4].Paragraphs[0].Append("PORCENTAJE");
                reporte.Rows[0].Cells[5].Paragraphs[0].Append("RA");

                reporte.Design = TableDesign.TableGrid;

                reporte.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                reporte.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                reporte.Rows[0].Cells[0].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[1].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[2].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[3].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[4].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[5].Paragraphs[0].Color(System.Drawing.Color.White);
                reporte.Rows[0].Cells[0].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[1].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[2].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[3].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[4].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[5].Paragraphs[0].Bold();
                reporte.Rows[0].Cells[0].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[1].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[2].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[3].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[4].Paragraphs[0].FontSize(8);
                reporte.Rows[0].Cells[5].Paragraphs[0].FontSize(8);

                int index = 1;
                foreach (ServicioRpfDTO item in list)
                {
                    reporte.Rows[index].Cells[0].Paragraphs[0].Append(item.EMPRNOMB);
                    reporte.Rows[index].Cells[1].Paragraphs[0].Append(item.EQUINOMB);
                    reporte.Rows[index].Cells[2].Paragraphs[0].Append(item.EQUIABREV);
                    reporte.Rows[index].Cells[3].Paragraphs[0].Append(item.PTOMEDICODI.ToString());
                    reporte.Rows[index].Cells[4].Paragraphs[0].Append(item.PORCENTAJE.ToString(Constantes.FormatoNumero) + Constantes.CaracterPorcentaje);
                    reporte.Rows[index].Cells[5].Paragraphs[0].Append(item.ValorRA.ToString("0.0000"));

                    reporte.Rows[index].Cells[0].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[1].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[2].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[3].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[4].Paragraphs[0].FontSize(8);
                    reporte.Rows[index].Cells[5].Paragraphs[0].FontSize(8);

                    index++;
                }

                document.InsertSectionPageBreak();
                int numeral = 1;

                try
                {

                    foreach (ServicioRpfDTO item in list)
                    {
                        if (item.ListaArea !=null && item.ListaPotencia !=null && item.ListaFrecuencia != null && item.ListaSanJuan !=null)
                        {
                            Paragraph parrafo = document.InsertParagraph();
                            parrafo.Append(numeral.ToString() + ". " + item.EMPRNOMB.Trim() + " " + item.EQUINOMB.Trim() + " - " +
                                item.EQUIABREV.Trim() + " (" + item.PORCENTAJE.ToString(Constantes.FormatoNumero) + "%)\n");
                            parrafo.Bold();

                            grafico.GenerarGraficoFalla(item.ListaArea, item.ListaPotencia, item.ListaFrecuencia, path, numeral, fechaProceso, item.ValorRA, item.ListaSanJuan);

                            Novacode.Image chart = document.AddImage(path + numeral +  Constantes.NombreChartFallaRPF);

                            Paragraph celda = document.InsertParagraph();
                            celda.AppendPicture(chart.CreatePicture());
                            celda.Alignment = Alignment.center;

                            Paragraph separacion = document.InsertParagraph();
                            separacion.Append("\n\n");


                            numeral++;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message, ex);
                }

                document.Save();
            }
        }
    }
}