using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using Novacode;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
{
    public class WordDocument
    {
        /// <summary>
        /// Permite generar el reporte de perturbación en formato WORD
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="evento"></param>
        /// <param name="path"></param>
        public void GenerarReportePerturbacion(List<InformePerturbacionDTO> Lista, EventoDTO evento, int idEvento, string path)
        {
            #region Generacion del Documento Word

            using (DocX document = DocX.Create(path + Constantes.NombreReportePerturbacionWord))
            {

                

                Novacode.Image logo = document.AddImage(path + Constantes.NombreLogoCoes);
                document.AddHeaders();
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;
                Header header_first = document.Headers.odd;

                #region Company Logo in Header in Table

                Table header_first_table = header_first.InsertTable(2, 3);
                header_first_table.Design = TableDesign.TableGrid;
                header_first_table.AutoFit = AutoFit.Contents;


                Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                upperRightParagraph.AppendPicture(logo.CreatePicture());
                upperRightParagraph.Alignment = Alignment.center;

                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                header_first_table.MergeCellsInColumn(0, 0, 1);
                header_first.InsertParagraph("\n");

                Paragraph cabecera = header_first.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                cabecera.Append("REPORTE DE PERTURBACIONES DEL\n SEIN");
                cabecera.Alignment = Alignment.center;
                cabecera.FontSize(13);
                cabecera.Bold();

                cabecera = header_first.Tables[0].Rows[0].Cells[2].Paragraphs[0];
                cabecera.Append(String.Format("Fecha de Emisión:\n {0}", DateTime.Now.ToString("dd/MM/yyyy")));
                cabecera.Alignment = Alignment.center;

                cabecera = header_first.Tables[0].Rows[1].Cells[1].Paragraphs[0];
                cabecera.Append("IAO – " + idEvento + "-" + DateTime.Now.Year);
                cabecera.Alignment = Alignment.center;


                //PageNumberFormat number = PageNumberFormat.normal;

                cabecera = header_first.Tables[0].Rows[1].Cells[2].Paragraphs[0];
                cabecera.AppendPageNumber(PageNumberFormat.normal);
                cabecera.Alignment = Alignment.center;

                #endregion

                Table tabla = document.InsertTable(24, 2);
                tabla.AutoFit = AutoFit.Window;
                tabla.Design = TableDesign.None;
                Paragraph parrafo = tabla.Rows[0].Cells[0].Paragraphs[0];
                parrafo.Append("1. FECHA");
                parrafo.Bold();

                Paragraph texto = tabla.Rows[0].Cells[1].Paragraphs[0];

                string fecha = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString("dd/MM/yyyy") : string.Empty;
                texto.Append(fecha);

                parrafo = tabla.Rows[1].Cells[0].Paragraphs[0];
                parrafo.Append("2. HORA DE INICIO");
                parrafo.Bold();
                texto = tabla.Rows[1].Cells[1].Paragraphs[0];

                string time = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString("HH:mm:ss") : string.Empty;
                texto.Append(time);

                parrafo = tabla.Rows[2].Cells[0].Paragraphs[0];
                parrafo.Append("3. EQUIPO");
                parrafo.Bold();
                texto = tabla.Rows[2].Cells[1].Paragraphs[0];
                texto.Append(evento.EQUIABREV);

                parrafo = tabla.Rows[3].Cells[0].Paragraphs[0];
                parrafo.Append("4. PROPIETARIO");
                parrafo.Bold();
                texto = tabla.Rows[3].Cells[1].Paragraphs[0];
                texto.Append(evento.EMPRNOMB);

                parrafo = tabla.Rows[4].Cells[0].Paragraphs[0];
                parrafo.Append("5. CAUSA DE LA PERTURBACIÓN\n");
                parrafo.Bold();

                InformePerturbacionDTO item = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemCausa).FirstOrDefault();

                if (item != null)
                {
                    texto = tabla.Rows[4].Cells[1].Paragraphs[0];
                    texto.Append(item.SUBCAUSADESC);
                }

                tabla.Rows[5].MergeCells(0, 1);
                parrafo = tabla.Rows[5].Cells[0].Paragraphs[0];
                parrafo.Append("6. DESCRIPCIÓN");
                parrafo.Bold();

                InformePerturbacionDTO descripcion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemDescripcion).FirstOrDefault();

                if (descripcion != null)
                {
                    tabla.Rows[6].MergeCells(0, 1);
                    texto = tabla.Rows[6].Cells[0].Paragraphs[0];
                    texto.Append(descripcion.ITEMDESCRIPCION);
                    texto.Alignment = Alignment.both;
                    
                }

                List<InformePerturbacionDTO> listaSecuencia = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemSecuencia).ToList();

                tabla.Rows[7].MergeCells(0, 1);
                parrafo = tabla.Rows[7].Cells[0].Paragraphs[0];
                parrafo.Append("7. SECUENCIA DE EVENTOS");
                parrafo.Bold();


                if (listaSecuencia.Count > 0)
                {
                    Table secuencia = tabla.Rows[8].Cells[0].InsertTable(listaSecuencia.Count + 1, 2);
                    secuencia.Rows[0].Cells[0].Paragraphs[0].Append("Hora(hh:mm:ss)");
                    secuencia.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");                   
                    secuencia.AutoFit = AutoFit.Window;
                    secuencia.Design = TableDesign.TableGrid;
                    secuencia.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[0].Paragraphs[0].Bold();
                    secuencia.Rows[0].Cells[1].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaSecuencia)
                    {
                        secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.ITEMTIME);
                        secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }

                    tabla.Rows[8].MergeCells(0, 1);
                }

                List<InformePerturbacionDTO> listaActuacion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemActuacion).ToList();
                int indice = 8;
                int row = 10;

                if (listaActuacion.Count > 0)
                {
                    parrafo = tabla.Rows[9].Cells[0].Paragraphs[0];
                    parrafo.Append("8. ACTUACIÓN DE LAS PROTECCIONES");
                    parrafo.Bold();

                    Table adicional = tabla.Rows[10].Cells[0].InsertTable(listaActuacion.Count + 1, 5);
                  
                    adicional.AutoFit = AutoFit.Window;

                    adicional.Rows[0].Cells[0].Paragraphs[0].Append("Subestación");
                    adicional.Rows[0].Cells[1].Paragraphs[0].Append("Equipo");
                    adicional.Rows[0].Cells[2].Paragraphs[0].Append("Señalización");
                    adicional.Rows[0].Cells[3].Paragraphs[0].Append("Interruptor");
                    adicional.Rows[0].Cells[4].Paragraphs[0].Append("A/C");


                    adicional.Design = TableDesign.TableGrid;
                    adicional.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    adicional.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    adicional.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    adicional.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    adicional.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    adicional.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    adicional.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    adicional.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    adicional.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    adicional.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    adicional.Rows[0].Cells[0].Paragraphs[0].Bold();
                    adicional.Rows[0].Cells[1].Paragraphs[0].Bold();
                    adicional.Rows[0].Cells[2].Paragraphs[0].Bold();
                    adicional.Rows[0].Cells[3].Paragraphs[0].Bold();
                    adicional.Rows[0].Cells[4].Paragraphs[0].Bold();





                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaActuacion)
                    {
                        adicional.Rows[index].Cells[0].Paragraphs[0].Append(entity.SUBESTACION);
                        adicional.Rows[index].Cells[1].Paragraphs[0].Append(entity.EQUINOMB);
                        adicional.Rows[index].Cells[2].Paragraphs[0].Append(entity.ITEMSENALIZACION);
                        adicional.Rows[index].Cells[3].Paragraphs[0].Append(entity.INTERRUPTORNOMB);
                        adicional.Rows[index].Cells[4].Paragraphs[0].Append(entity.ITEMAC);
                        index++;
                    }
                    tabla.Rows[10].MergeCells(0, 1);
                    indice++;
                    row++;
                }

                tabla.Rows[row].MergeCells(0, 1);
                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. ANÁLISIS DEL EVENTO", indice));
                parrafo.Bold();
                row++;
                indice++;

                InformePerturbacionDTO analisis = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemAnalisis).FirstOrDefault();

                if (analisis != null)
                {
                    tabla.Rows[row].MergeCells(0, 1);
                    texto = tabla.Rows[row].Cells[0].Paragraphs[0];                    
                    texto.Append(analisis.ITEMDESCRIPCION);
                    texto.Alignment = Alignment.both;
                }
                row++;

                tabla.Rows[row].MergeCells(0, 1);
                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. CONCLUSIONES", indice));
                parrafo.Bold();
                row++;
                indice++;

                List<InformePerturbacionDTO> listaConclusion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemConclusion).OrderBy(x => x.ITEMORDEN).ToList();

                if (listaConclusion.Count > 0)
                {
                    Table conclusion = tabla.Rows[row].Cells[0].InsertTable(listaConclusion.Count + 1, 2);
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Append("Nro.");
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");                    
                    conclusion.AutoFit = AutoFit.Window;


                    conclusion.Design = TableDesign.TableGrid;
                    conclusion.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Bold();
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Bold();




                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaConclusion)
                    {
                        conclusion.Rows[index].Cells[0].Paragraphs[0].Append(index.ToString());
                        conclusion.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }
                    tabla.Rows[row].MergeCells(0, 1);
                }

                row++;

                tabla.Rows[row].MergeCells(0, 1);
                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. RECOMENDACIONES", indice));
                parrafo.Bold();
                row++;
                indice++;

                List<InformePerturbacionDTO> listaRecomendacion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemRecomendacion).OrderBy(x => x.ITEMORDEN).ToList();

                if (listaRecomendacion.Count > 0)
                {
                    Table conclusion = tabla.Rows[row].Cells[0].InsertTable(listaRecomendacion.Count + 1, 2);
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Append("Nro.");
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");
                   
                    conclusion.AutoFit = AutoFit.Window;
                    conclusion.Design = TableDesign.TableGrid;
                    conclusion.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Bold();
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaRecomendacion)
                    {
                        conclusion.Rows[index].Cells[0].Paragraphs[0].Append(index.ToString());
                        conclusion.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }
                    tabla.Rows[row].MergeCells(0, 1);
                }

                row++;

                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. OPORTUNIDADES DE MEJORA", indice));
                parrafo.Bold();
                tabla.Rows[row].MergeCells(0, 1);
                row++;
                indice++;

                List<InformePerturbacionDTO> listaOportunidad = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemOportunidad).OrderBy(x => x.ITEMORDEN).ToList();

                if (listaOportunidad.Count > 0)
                {
                    Table conclusion = tabla.Rows[row].Cells[0].InsertTable(listaOportunidad.Count + 1, 2);
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Append("Nro.");
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");
                    
                    conclusion.AutoFit = AutoFit.Window;
                    conclusion.Design = TableDesign.TableGrid;
                    conclusion.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Bold();
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaOportunidad)
                    {
                        conclusion.Rows[index].Cells[0].Paragraphs[0].Append(index.ToString());
                        conclusion.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }
                    tabla.Rows[row].MergeCells(0, 1);
                }

                row++;

                tabla.Rows[row].MergeCells(0, 1);
                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. ACUERDOS", indice));
                parrafo.Bold();
                row++;
                indice++;

                List<InformePerturbacionDTO> listaAcuerdo = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemAcuerdo).OrderBy(x => x.ITEMORDEN).ToList();

                if (listaAcuerdo.Count > 0)
                {
                    Table conclusion = tabla.Rows[row].Cells[0].InsertTable(listaAcuerdo.Count + 1, 2);
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Append("Nro.");
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");
                  
                    conclusion.AutoFit = AutoFit.Window;
                    conclusion.Design = TableDesign.TableGrid;
                    conclusion.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Bold();
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaAcuerdo)
                    {
                        conclusion.Rows[index].Cells[0].Paragraphs[0].Append(index.ToString());
                        conclusion.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }
                    tabla.Rows[row].MergeCells(0, 1);
                }

                row++;

                tabla.Rows[row].MergeCells(0, 1);
                parrafo = tabla.Rows[row].Cells[0].Paragraphs[0];
                parrafo.Append(string.Format("{0}. PLAZOS DE ATENCIÓN PARA LAS OPORTUNIDADES DE MEJORA", indice));
                parrafo.Bold();
                indice++;

                List<InformePerturbacionDTO> listaPlazo = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemPlazo).OrderBy(x => x.ITEMORDEN).ToList();

                if (listaPlazo.Count > 0)
                {
                    Table conclusion = tabla.Rows[row].Cells[0].InsertTable(listaPlazo.Count + 1, 2);
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Append("Nro.");
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Append("Descripción");
                   
                    conclusion.AutoFit = AutoFit.Window;
                    conclusion.Design = TableDesign.TableGrid;
                    conclusion.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    conclusion.Rows[0].Cells[0].Paragraphs[0].Bold();
                    conclusion.Rows[0].Cells[1].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (InformePerturbacionDTO entity in listaPlazo)
                    {
                        conclusion.Rows[index].Cells[0].Paragraphs[0].Append(index.ToString());
                        conclusion.Rows[index].Cells[1].Paragraphs[0].Append(entity.ITEMDESCRIPCION);
                        index++;
                    }
                    tabla.Rows[row].MergeCells(0, 1);
                }

                row++;

                tabla.Alignment = Alignment.center;

                document.Save();
            }

            #endregion
        }
    }
}