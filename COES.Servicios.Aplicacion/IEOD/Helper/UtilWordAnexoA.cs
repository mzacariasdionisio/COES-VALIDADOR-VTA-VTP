using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using Newtonsoft.Json;
using Novacode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COES.Servicios.Aplicacion.IEOD.Helper
{
    public class UtilWordAnexoA
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UtilWordAnexoA));

        public static FontFamily FontDoc = new FontFamily("Arial");
        public static Color ColorTituloDoc = ColorTranslator.FromHtml("#000080");
        public static Color ColorTituloSeccion = ColorTranslator.FromHtml("#FF0000");
        public static Color ColorNegro = ColorTranslator.FromHtml("#000000");
        public static Color ColorEnlace = ColorTranslator.FromHtml("#0563C1");
        public static int HeighImagePX = 356;
        public static int WidthImagePX = 534;

        /// <summary>
        /// Generar archivo Word
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GenerarWordAnexoA(ModelWordAnexoA model)
        {
            string rutaArchivoTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + "area.png";
            string rutaArchivoDefault = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + "default.png";

            using (DocX document = DocX.Create(model.RutaNombreArchivo))
            {
                string diaSemana = EPDate.f_NombreDiaSemana(model.Fecha.DayOfWeek).ToLower();
                string textoparrafo = null;

                #region Header y Footer

                document.MarginLeft = 76.0f;
                document.MarginRight = 76.0f;

                //Cabecera
                document.AddHeaders();

                //Novacode.Image logo = document.AddImage(pathLogo);
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header_first = document.Headers.odd;

                Table header_first_table = header_first.InsertTable(1, 2);
                header_first_table.Design = TableDesign.TableNormal;
                header_first_table.AutoFit = AutoFit.ColumnWidth;

                header_first_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;
                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[0].Width = 350;

                header_first_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.right;
                header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[1].Width = 300;

                //primera fila

                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Top;
                Paragraph cabDerecha = header_first.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                cabDerecha.Append("Subdirección de Gestión de la Información").Font(FontDoc).Bold().FontSize(9);
                cabDerecha.AppendLine();
                cabDerecha.Append("IEOD No. " + model.NumeroIeod + "/" + model.Fecha.Year).Font(FontDoc).Bold().FontSize(9);
                cabDerecha.AppendLine();
                cabDerecha.Append(model.Fecha.ToString(ConstantesAppServicio.FormatoFecha)).Font(FontDoc).Bold().FontSize(9);
                cabDerecha.AppendLine();

                //Pie de página
                document.AddFooters();

                // Get the odd and even Footer for this document.
                Footer footer_odd = document.Footers.odd;

                Table footer_table = footer_odd.InsertTable(1, 2);
                footer_table.Design = TableDesign.TableNormal;
                footer_table.AutoFit = AutoFit.ColumnWidth;
                footer_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;
                footer_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                footer_table.Rows[0].Cells[0].Width = 380;
                footer_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.right;
                footer_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                footer_table.Rows[0].Cells[1].Width = 300;

                Paragraph codigo_acta = footer_odd.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                codigo_acta.Append(string.Format("SEMANA {0}", EPDate.f_numerosemana(model.Fecha))).Font(FontDoc).Bold().FontSize(8);

                Paragraph numero_pagina = footer_odd.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                numero_pagina.Append("Página ").Font(FontDoc).Bold().FontSize(8);
                numero_pagina.AppendPageNumber(PageNumberFormat.normal);
                numero_pagina.Append(" de ");
                numero_pagina.AppendPageCount(PageNumberFormat.normal);

                #endregion

                #region Título ANEXO A

                Paragraph p = document.InsertParagraph().Font(FontDoc);
                p.Append("INFORME DE EVALUACIÓN DE LA OPERACIÓN DIARIA").Font(FontDoc).Bold().FontSize(14).Color(ColorTituloDoc).UnderlineStyle(UnderlineStyle.singleLine);
                p.Alignment = Alignment.center;

                string dayname = model.Fecha.ToString("dddd", new CultureInfo("es-PE")).ToUpper();
                string nombreMes = Base.Tools.Util.ObtenerNombreMes(model.Fecha.Month).ToUpper();
                nombreMes = nombreMes.Length > 1 ? nombreMes.Substring(0, 1).ToUpper() + nombreMes.Substring(1, nombreMes.Length - 1) : nombreMes;
                string fechaTitulo = dayname.ToUpper() + " " + model.Fecha.Day + " DE " + nombreMes + " DE " + model.Fecha.Year;

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Font(FontDoc).Append(fechaTitulo).FontSize(13).Font(FontDoc).Bold().Color(ColorTituloDoc).UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p.Alignment = Alignment.center;

                #endregion
                
                #region "1. EVALUACIÓN TÉCNICA"
                
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1. ").FontSize(12).Font(FontDoc).Bold().Color(ColorTituloSeccion);
                AddSubtitulo(ref p, "EVALUACIÓN TÉCNICA", FontDoc, true);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("DESPACHOS DEL COES SINAC").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                #region 1.1.1.EVOLUCIÓN HORARIA DE LA DEMANDA TOTAL

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1.1. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("EVOLUCIÓN HORARIA DE LA DEMANDA TOTAL").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Se muestra la evolución de la demanda en el día:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #region Gráfico de Lineas DEMANDA EJECUTADA Y PROGRAMADA DEL COES

                //gráfico               
                var lineasObject = GetGraficoHighchartDemEjecyProg(model.GraficoDemandaEjecyProg, model.IncluyeEcuador);
                await AgregarImagenHighchart(document, "1.1.1", lineasObject, rutaArchivoTemporal, rutaArchivoDefault);

                #endregion

                string strProgEmitido = string.Format("Emitido a las {0} h. del día {1}", model.FechaProgramaEmitido.ToString(ConstantesHorasOperacion.FormatoOnlyHora), model.FechaProgramaEmitido.ToString("dd.MM.yyyy"));
                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append("").Font(FontDoc);
                p.Append("Programa: ").FontSize(11).Font(FontDoc).Bold().Font(FontDoc);
                p.Append(strProgEmitido).Font(FontDoc);
                p.AppendLine();

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Reprogramas:").FontSize(11).Font(FontDoc).Bold().AppendLine().Font(FontDoc);

                //Tabla Reprogramas
                if (model.ListaReprogramas.Count > 0)
                {
                    TablaWordListaReprogramas(document, model.ListaReprogramas);
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Ninguna.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                p.AppendLine("");
                Paragraph paFacC = document.InsertParagraph();
                paFacC.AppendLine();

                //parrafo 1
                string txtComparacion;
                if (model.PorcVariacionFactorCarga == 0)
                {
                    txtComparacion = "igual";
                }
                else
                {
                    if (model.PorcVariacionFactorCarga > 0)
                        txtComparacion = string.Format("{0}% mayor", model.PorcVariacionFactorCarga);
                    else
                        txtComparacion = string.Format("{0}% menor", Math.Abs(model.PorcVariacionFactorCarga));
                }

                textoparrafo = string.Format("El factor de carga del SEIN obtenido del diagrama ejecutado fue {0}, siendo este valor {1} que el {2} de la semana pasada.",
                                            model.FactorCargaHoy, txtComparacion, diaSemana);
                var bulletedListPFacC = document.AddList(textoparrafo, 0, ListItemType.Bulleted);

                //parrafo 2
                string txtComparacionVelHoy;
                if (model.PorcVCrecimientoVsAyer == 0) { txtComparacionVelHoy = ", siendo este valor igual"; }
                else
              if (model.PorcVCrecimientoVsAyer > 0)
                    txtComparacionVelHoy = ", siendo este valor " + model.PorcVCrecimientoVsAyer + "% mayor";
                else
                    txtComparacionVelHoy = ", siendo este valor " + Math.Abs(model.PorcVCrecimientoVsAyer) + "% menor";

                string txtComparacionVelHace7d;
                if (model.PorcVCrecimientoVsHace7d == 0) { txtComparacionVelHace7d = "e igual"; }
                else
              if (model.PorcVCrecimientoVsHace7d > 0)
                    txtComparacionVelHace7d = "y " + model.PorcVCrecimientoVsHace7d + "% mayor";
                else
                    txtComparacionVelHace7d = "y " + Math.Abs(model.PorcVCrecimientoVsHace7d) + "% menor";

                List<Paragraph> lstPFacC = bulletedListPFacC.Items;
                textoparrafo = string.Format("La velocidad de crecimiento de la carga al entrar a la hora punta dentro del periodo de {0} a {1} h aproximadamente, fue {2} MW/min{3} que el día de ayer {4} que el {5} de la semana pasada.",
                                             model.HrPeriodo01, model.HrPeriodo02, model.VelCrecimiento, txtComparacionVelHoy, txtComparacionVelHace7d, diaSemana);
                document.AddListItem(bulletedListPFacC, textoparrafo, 0, ListItemType.Bulleted);

                Paragraph pSbPE = document.InsertParagraph();
                lstPFacC.ElementAt(0).Alignment = Alignment.both;
                pSbPE.InsertParagraphBeforeSelf(lstPFacC.ElementAt(0).Font(FontDoc).FontSize(11));

                Paragraph pSbPE2 = document.InsertParagraph();
                lstPFacC.ElementAt(1).Alignment = Alignment.both;
                pSbPE2.InsertParagraphBeforeSelf(lstPFacC.ElementAt(1).Font(FontDoc).FontSize(11));
                paFacC.AppendLine();

                //Tabla Crecimiento por areas
                TablaWordVelocidadCrecimiento(document, model);

                //parrafo 3
                Paragraph paMinDem = document.InsertParagraph();
                paMinDem.AppendLine();

                textoparrafo = string.Format("La mínima demanda del SEIN fue {0} MW y ocurrió a las {1} h, mientras que la mayor demanda alcanzó un valor de {2} MW a las {3} h.",
                                            model.MinDemanda, model.HoraMinDemanda, model.MaxDemanda, model.HoraMaxDemanda);
                var bulletedListMinDem = document.AddList(textoparrafo, 0, ListItemType.Bulleted);

                List<Paragraph> lstPMinDem = bulletedListMinDem.Items;
                Paragraph pMinDem = document.InsertParagraph();
                lstPMinDem.ElementAt(0).Alignment = Alignment.both;
                pMinDem.InsertParagraphBeforeSelf(lstPMinDem.ElementAt(0).Font(FontDoc).FontSize(11));
                paMinDem.AppendLine();

                #endregion

                #region 1.1.2. EVOLUCIÓN HORARIA DE LA DEMANDA POR ÁREA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1.2. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("EVOLUCIÓN HORARIA DE LA DEMANDA POR ÁREA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                //gráfico
                var jsonDataLineasDPA = GetGraficoHighchartDemandaArea(model.GraficoDemandaxAreas);
                await AgregarImagenHighchart(document, "1.1.2", jsonDataLineasDPA, rutaArchivoTemporal, rutaArchivoDefault);

                //Párrafo 1
                string descMaximaDemanda = "";
                #region Escribir combinaciones de horas de demandas
                if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSein && model.HoraMaxDemSeinCentro == model.HoraMaxDemSein && model.HoraMaxDemSeinSur == model.HoraMaxDemSein) // Todas =
                {
                    descMaximaDemanda += "Las máximas demandas de las áreas Norte, Centro y Sur se presentaron a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h).";
                }
                else
                {
                    if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSein && model.HoraMaxDemSeinCentro == model.HoraMaxDemSein) // Norte = Centro = SEIN
                    {
                        descMaximaDemanda += "Las máximas demandas de las áreas Norte y Centro se presentaron a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                        descMaximaDemanda += "mientras que la máxima demanda del área Sur ocurrió a las " + model.HoraMaxDemSeinSur + " h.";
                    }
                    else
                    {
                        if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSein && model.HoraMaxDemSeinSur == model.HoraMaxDemSein) // Norte = Sur = SEIN
                        {
                            descMaximaDemanda += "Las máximas demandas de las áreas Norte y Sur se presentaron a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                            descMaximaDemanda += "mientras que la máxima demanda del área Centro ocurrió a las " + model.HoraMaxDemSeinCentro + " h.";
                        }
                        else
                        {
                            if (model.HoraMaxDemSeinCentro == model.HoraMaxDemSein && model.HoraMaxDemSeinSur == model.HoraMaxDemSein) // Centro = Sur = SEIN
                            {
                                descMaximaDemanda += "Las máximas demandas de las áreas Centro y Sur se presentaron a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                                descMaximaDemanda += "mientras que la máxima demanda del área Norte ocurrió a las " + model.HoraMaxDemSeinNorte + " h.";
                            }
                            else
                            {
                                if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSein) // Norte = SEIN
                                {
                                    descMaximaDemanda += "La máxima demanda del área Norte se presentó a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                                    if (model.HoraMaxDemSeinCentro == model.HoraMaxDemSeinSur) // Centro = Sur
                                        descMaximaDemanda += "mientras que las máximas demandas de las áreas Centro y Sur ocurrieron a las " + model.HoraMaxDemSeinCentro + " h.";
                                    else
                                        descMaximaDemanda += "mientras que las máximas demandas de las áreas Centro y Sur ocurrieron a las " + model.HoraMaxDemSeinCentro + " h y " + model.HoraMaxDemSeinSur + " h, respectivamente.";
                                }
                                else
                                {
                                    if (model.HoraMaxDemSeinCentro == model.HoraMaxDemSein) // Centro = SEIN
                                    {
                                        descMaximaDemanda += "La máxima demanda del área Centro se presentó a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                                        if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSeinSur) // Norte = Sur
                                            descMaximaDemanda += "mientras que las máximas demandas de las áreas Norte y Sur ocurrieron a las " + model.HoraMaxDemSeinNorte + " h.";
                                        else
                                            descMaximaDemanda += "mientras que las máximas demandas de las áreas Norte y Sur ocurrieron a las " + model.HoraMaxDemSeinNorte + " h y " + model.HoraMaxDemSeinSur + " h, respectivamente.";
                                    }
                                    else
                                    {
                                        if (model.HoraMaxDemSeinSur == model.HoraMaxDemSein) // Sur = SEIN
                                        {
                                            descMaximaDemanda += "La máxima demanda del área Sur se presentó a la misma hora que la máxima demanda del SEIN (" + model.HoraMaxDemSein + " h), ";
                                            if (model.HoraMaxDemSeinNorte == model.HoraMaxDemSeinCentro) // Norte = Centro
                                                descMaximaDemanda += "mientras que las máximas demandas de las áreas Norte y Centro ocurrieron a las " + model.HoraMaxDemSeinNorte + " h.";
                                            else
                                                descMaximaDemanda += "mientras que las máximas demandas de las áreas Norte y Centro ocurrieron a las " + model.HoraMaxDemSeinNorte + " h y " + model.HoraMaxDemSeinCentro + " h, respectivamente.";
                                        }
                                        else
                                        {
                                            descMaximaDemanda += "La máxima demanda del SEIN se presentó a las " + model.HoraMaxDemSein + " h., mientras que las máximas demandas de las áreas Norte, Centro y Sur ocurrieron a las " + model.HoraMaxDemSeinNorte + " h, " + model.HoraMaxDemSeinCentro + " h y " + model.HoraMaxDemSeinSur + " h, respectivamente.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                textoparrafo = descMaximaDemanda;
                var bulletedListEvolH = document.AddList(textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 2
                textoparrafo = string.Format("La contribución del área Centro a la máxima demanda del SEIN fue {0} %, mientras que las áreas Norte y Sur contribuyeron con {1} % y {2} % respectivamente.",
                                                model.PorcDemSeinCentro, model.PorcDemSeinNorte, model.PorcDemSeinSur);
                document.AddListItem(bulletedListEvolH, textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 3
                textoparrafo = string.Format("Los factores de carga fueron: Área Norte: {0}, Área Centro: {1}, Área Sur: {2}.",
                                                model.FCargaNorte, model.FCargaCentro, model.FCargaSur);
                document.AddListItem(bulletedListEvolH, textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 4
                textoparrafo = string.Format("La velocidad de crecimiento de carga de las áreas Centro, Norte y Sur fueron {0}, {1} y {2} MW/min respectivamente.",
                                                model.VelCrecCentro, model.VelCrecNorte, model.VelCrecSur);
                document.AddListItem(bulletedListEvolH, textoparrafo, 0, ListItemType.Bulleted);

                List<Paragraph> lstEvolH = bulletedListEvolH.Items;
                Paragraph pEvolH = document.InsertParagraph();
                lstEvolH.ElementAt(0).Alignment = Alignment.both;
                pEvolH.InsertParagraphBeforeSelf(lstEvolH.ElementAt(0).Font(FontDoc).FontSize(11));

                Paragraph pEvolH1 = document.InsertParagraph();
                lstEvolH.ElementAt(1).Alignment = Alignment.both;
                pEvolH1.InsertParagraphBeforeSelf(lstEvolH.ElementAt(1).Font(FontDoc).FontSize(11));

                Paragraph pEvolH2 = document.InsertParagraph();
                lstEvolH.ElementAt(1).Alignment = Alignment.both;
                pEvolH2.InsertParagraphBeforeSelf(lstEvolH.ElementAt(2).Font(FontDoc).FontSize(11));

                Paragraph pEvolH3 = document.InsertParagraph();
                lstEvolH.ElementAt(1).Alignment = Alignment.both;
                pEvolH3.InsertParagraphBeforeSelf(lstEvolH.ElementAt(3).Font(FontDoc).FontSize(11));
                pEvolH.AppendLine();

                #endregion

                #region 1.1.3. EVOLUCIÓN HORARIA DE LAS CARGAS MAS IMPORTANTES

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1.3. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("EVOLUCIÓN HORARIA DE LAS CARGAS MAS IMPORTANTES").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine();

                //Gráfico 01
                var ghChart1 = GetGraficoHighchartGUlibre(model.GraficoMultilineas);
                await AgregarImagenHighchart(document, "1.1.3_1", ghChart1, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //Gráfico 02
                var ghChart2 = GetGraficoHighchartGUlibre(model.GraficoMultilineas02);
                await AgregarImagenHighchart(document, "1.1.3_2", ghChart2, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //Gráfico 03
                var ghChart3 = GetGraficoHighchartGUlibre(model.GraficoMultilineas03);
                await AgregarImagenHighchart(document, "1.1.3_3", ghChart3, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //Gráfico 04
                var ghChart4 = GetGraficoHighchartGUlibre(model.GraficoMultilineas04);
                await AgregarImagenHighchart(document, "1.1.3_4", ghChart4, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #region 1.1.4. RECURSOS ENERGÉTICOS Y DIAGRAMA DE DURACIÓN DE CARGA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1.4. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("RECURSOS ENERGÉTICOS Y DIAGRAMA DE DURACIÓN DE CARGA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p.Append("Se presenta el siguiente gráfico:").Font(FontDoc);
                p.AppendLine();

                #region Gráfico 1.1.4. RECURSOS ENERGÉTICOS Y DIAGRAMA DE DURACIÓN DE CARGA

                var ghChartRecursoEnerg = GetGraficoHighchartDiagramaDuracion(model.GraficoRecEnergeticos);
                await AgregarImagenHighchart(document, "1.1.4", ghChartRecursoEnerg, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                #endregion

                p.Append("Del diagrama se observa que:").Font(FontDoc);
                p.AppendLine();
                Paragraph paEnerg = document.InsertParagraph();
                paEnerg.AppendLine();

                //Párrafo 1
                string txtIncluyeEcu = "";
                if (model.IncluyeEcuador) txtIncluyeEcu = " (Incluye exportación a Ecuador)";
                textoparrafo = string.Format("La energía total producida{1} fue {0} MWh.", model.EnegíaTotProduccion, txtIncluyeEcu);
                var bulletedListEnerg = document.AddList(textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 2
                textoparrafo = string.Format("La energía proporcionada por las centrales de pasada y regulación fueron {0} y {1} MWh respectivamente.",
                                                model.EnerCtralPasada, model.EnerCtralRegulacion);
                document.AddListItem(bulletedListEnerg, textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 3
                textoparrafo = string.Format("La energía térmica de tipo diesel fue de {0} MWh ({1} %). La energía generada con residual fue de {2} MWh ({3} %). La energía generada con gas fue de {4} MWh ({5} %), con carbón fue de {6} MWh ({7} %), bagazo y biogás fueron de {8} MWh ({9} %), la energía eólica fue de {10} MWh ({11} %) y la energía generada por las centrales solares fueron de {12} MWh ({13} %).",
                                                model.EnerTerDiesel, model.PorcEnerTerDiesel, model.EnergiaResidual, model.PorcEnergiaResidual, model.EnerGas, model.PorcEnerGas, model.EnerCarbon, model.PorcEnerCarbon, model.EnerBagBio, model.PorcEnerBagBio, model.EnerEolica, model.PorcEnerEolica, model.EnerSolar, model.PorcEnerSolar);
                document.AddListItem(bulletedListEnerg, textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 4
                textoparrafo = string.Format("Durante {0} horas la demanda fue mayor al {1} % de la máxima demanda ({2} % del tiempo total).", model.HorasDemanda, model.PorcMayorMaxDemanda, model.PorcTiempoTotal);
                document.AddListItem(bulletedListEnerg, textoparrafo, 0, ListItemType.Bulleted);

                //Párrafo 5
                textoparrafo = string.Format("Durante {0} horas la demanda estuvo entre el {1} y {2} % de la máxima demanda ({3} % del tiempo total).", model.HorasDemanda2, model.PorcRangoMaxDemanda1, model.PorcRangoMaxDemanda2, model.PorcTiempoTotal2);
                document.AddListItem(bulletedListEnerg, textoparrafo, 0, ListItemType.Bulleted);

                List<Paragraph> lstEnerg = bulletedListEnerg.Items;
                Paragraph pEnerg = document.InsertParagraph();
                lstEnerg.ElementAt(0).Alignment = Alignment.both;
                pEnerg.InsertParagraphBeforeSelf(lstEnerg.ElementAt(0).Font(FontDoc).FontSize(11));
                Paragraph pEnerg1 = document.InsertParagraph();
                lstEnerg.ElementAt(1).Alignment = Alignment.both;
                pEnerg1.InsertParagraphBeforeSelf(lstEnerg.ElementAt(1).Font(FontDoc).FontSize(11));
                Paragraph pEnerg2 = document.InsertParagraph();
                lstEnerg.ElementAt(1).Alignment = Alignment.both;
                pEnerg2.InsertParagraphBeforeSelf(lstEnerg.ElementAt(2).Font(FontDoc).FontSize(11));
                Paragraph pEnerg3 = document.InsertParagraph();
                lstEnerg.ElementAt(1).Alignment = Alignment.both;
                pEnerg3.InsertParagraphBeforeSelf(lstEnerg.ElementAt(3).Font(FontDoc).FontSize(11));
                Paragraph pEnerg4 = document.InsertParagraph();
                lstEnerg.ElementAt(1).Alignment = Alignment.both;
                pEnerg4.InsertParagraphBeforeSelf(lstEnerg.ElementAt(4).Font(FontDoc).FontSize(11));
                pEnerg4.AppendLine();

                #endregion

                #region 1.2. EVOLUCIÓN DIARIA DE LA PRODUCCIÓN DE ENERGÍA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.2. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("EVOLUCIÓN DIARIA DE LA PRODUCCIÓN DE ENERGÍA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Se muestra el siguiente gráfico:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                //gráfico 
                var ghChart12 = GetGraficoHighchartEvolucionEnergia(model.GraficoEvolDiario, model.TextoMaxEnergiaAnio, model.EnerEjecutadaAnio);
                await AgregarImagenHighchart(document, "1.2", ghChart12, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                textoparrafo = String.Format("Nota: La energía ejecutada fue {0} MWh", model.EnerEjecutada);
                p.Append(textoparrafo).FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                string textoEnergiaHoy = "La energía ejecutada fue igual que la programada";
                if (model.DifEnerEjecProg != 0)
                {
                    string textoDiffE = model.DifEnerEjecProg < 0 ? "menor que la programada" : "mayor que la programada";
                    textoEnergiaHoy = string.Format("La energía ejecutada fue {0}MWh ({1}%) {2}", Math.Abs(model.DifEnerEjecProg), Math.Abs(model.PorcenerEjecutada), textoDiffE);
                }

                string textoEnergia7dAntes = "e igual que la producida el";
                if (model.DifEnerEjec7D != 0)
                {
                    string textoDiffE = model.DifEnerEjec7D < 0 ? "menor que la producida el" : "mayor que la producida el";
                    textoEnergia7dAntes = string.Format("y {0}MWh ({1}%) {2}", Math.Abs(model.DifEnerEjec7D), Math.Abs(model.PorcEnerEjec7D), textoDiffE);
                }

                textoparrafo = string.Format("{0} {1} {2} de la semana pasada.",
                                           textoEnergiaHoy, textoEnergia7dAntes, diaSemana);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append(textoparrafo).FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("La producción de energía proviene de datos puntuales (instantáneos) de potencia cada 30 minutos por lo que deben ser considerados referenciales. Los valores definitivos se Informan al culminar el mes y corresponderán a registro de medidores.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #region 1.3. MÁXIMA GENERACIÓN INSTANTÁNEA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.3. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("MÁXIMA GENERACIÓN INSTANTÁNEA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                //tabla                
                TablaWordMaxGenInstant(document, model.ListMaxGenInstant);
                p = document.InsertParagraph();

                //gráfico
                var ghChart13 = GetGraficoHighchartEvolucionMaximaDemanda(model.GraficoMaximaDemandaDiario, model.TextoMDAnio, model.MDEjecutadaAnio);
                await AgregarImagenHighchart(document, "1.3", ghChart13, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                textoparrafo = string.Format("Nota: La máxima generación instantánea fue {0} MW", model.MaxGenInst);
                p.Append(textoparrafo).FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);

                string textoDifMD = "no varió con";
                if (model.DismMaxDemanda != 0)
                {
                    textoDifMD = model.DismMaxDemanda < 0 ? "disminuyó" : "aumentó";
                }

                textoparrafo = string.Format("La máxima demanda del SEIN {3} {0} MW ({1} %) respecto al {2} de la semana pasada.",
                                   Math.Abs(model.DismMaxDemanda), Math.Abs(model.PorcDismMaxDemanda), diaSemana, textoDifMD);
                p.Append(textoparrafo).FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("La máxima demanda es a nivel de generación, y proviene de datos puntuales(instantáneos) de potencia cada 30 minutos por lo que deben ser considerados referenciales. Los valores definitivos se informan al culminar el mes y corresponderán a registro de medidores.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #endregion 

                #region 1.4. PRINCIPALES EVENTOS (FALLAS, INTERRUPCIONES Y RACIONAMIENTO

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.4. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("PRINCIPALES EVENTOS (FALLAS, INTERRUPCIONES Y RACIONAMIENTO").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListaEvento.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se describen los siguientes eventos:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //Tabla
                    TablaWordEvento(document, model.ListaEvento, model.ListaInterrup);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No existe información.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.5. MANTENIMIENTOS PROGRAMADOS Y EJECUTADOS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.5. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("MANTENIMIENTOS PROGRAMADOS Y EJECUTADOS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListMantProgEjecutados.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("En el siguiente cuadro se resumen las principales desviaciones al programa diario de mantenimiento:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //tabla                
                    TablaWordMantProgEjec(document, model.ListMantProgEjecutados);

                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("MC: Mantenimiento correctivo MP: Mantenimiento preventivo").FontSize(8).Font(FontDoc).Italic().AppendLine().Append(" ").Font(FontDoc);
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No existe información.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.6. OPERACIÓN DE EQUIPOS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.6. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("OPERACIÓN DE EQUIPOS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #region 1.6.1. OPERACIÓN DE CALDEROS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.6.1. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("OPERACIÓN DE CALDEROS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);


                //Tabla
                if (model.ListobjOpCaldero.Count > 0)
                {
                    TablaWordOpeCaldero(document, model.ListobjOpCaldero);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No existe información.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.6.2. OPERACIÓN A CARGA MÍNIMA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.6.2. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("OPERACIÓN A CARGA MÍNIMA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListobjOpCargaMin.Count > 0)
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Las unidades generadoras que operaron a carga mínima fueron:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //Tabla
                    TablaWordOpeCargaMin(document, model.ListobjOpCargaMin);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No existe información.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.7. RESERVA NO SINCRONIZADA DEL COES

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.7. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("RESERVA NO SINCRONIZADA DEL COES").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("En el siguiente gráfico se muestra la evolución horaria de la reserva no sincronizada de las unidades térmicas del SEIN (reserva fría).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                //Gráfico
                var jsonDataRFria = GetGraficoHighchartReservaFria(model.GraficoReservaSinc);
                await AgregarImagenHighchart(document, "1.7_1", jsonDataRFria, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Nota: En el cálculo de la Reserva fría no están incluidas las unidades térmicas con un tiempo de sincronización mayor a 6 horas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                //Gráfico
                var jsonDataLineasREGNC = GetGraficoHighchartREficiente(model.GraficoEficGNyCarb);
                await AgregarImagenHighchart(document, "1.7_2", jsonDataLineasREGNC, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Nota: Corresponde a las unidades de Generación no despachadas pero disponibles con combustible Gas Natural y Carbón.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #region 1.8. REGULACION DE TENSIÓN
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.8. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("REGULACION DE TENSIÓN").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListobjOpRegTension.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Las líneas de transmisión que salieron de servicio por falta de equipos de compensación reactiva para regular tensión fueron:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //Tabla                
                    TablaWordRegulacionTension(document, model.ListobjOpRegTension);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No se encontraron Unidades de generación ni Líneas de transmisión que regularon la tensión del SEIN.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.9. PRUEBAS DE UNIDADES 

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.9. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("PRUEBAS DE UNIDADES").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                #region 1.9.A. ALEATORIAS DE DISPONIBILIDAD

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("A. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("ALEATORIAS DE DISPONIBILIDAD").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListobjAleatoriasDisp.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se realizaron las siguientes pruebas:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //tabla
                    TablaWordPruebasEquipos(document, model.ListobjAleatoriasDisp);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No existe información.").FontSize(11).Font(FontDoc).AppendLine().Font(FontDoc);
                }

                #endregion

                #region 1.9.B. POR REQUERIMIENTOS PROPIOS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("B. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("POR REQUERIMIENTOS PROPIOS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListaReqPropio.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se realizaron las siguientes pruebas:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //tabla
                    TablaWordPruebasEquipos(document, model.ListaReqPropio);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No se realizaron pruebas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                    p = document.InsertParagraph().Font(FontDoc);
                }

                if (model.ListaReqPropioNoTermo.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se realizaron las siguientes pruebas no termoeléctricas:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //tabla
                    TablaWordPruebasEquipos(document, model.ListaReqPropioNoTermo, true);
                    p = document.InsertParagraph();
                }

                #endregion

                #region 1.9.C. A SOLICITUD DE TERCEROS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("C. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("A SOLICITUD DE TERCEROS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListaReqSoliTercero.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se realizaron las siguientes pruebas:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //tabla
                    TablaWordPruebasEquipos(document, model.ListaReqSoliTercero);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No se realizaron pruebas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                    p = document.InsertParagraph().Font(FontDoc);
                }

                #endregion

                #endregion

                #region 1.10. SISTEMAS AISLADOS

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.10. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("SISTEMAS AISLADOS").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                if (model.ListaSisAislado.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se presentaron los siguientes sistemas aislados:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //Tabla                
                    TablaWordSistemaAislado(document, model.ListaSisAislado);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No hubo sistemas aislados.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                }

                #endregion

                #region 1.11. CONGESTIÓN

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.11. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("CONGESTIÓN").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Mediante Resolución Dirección Ejecutiva N°523-2022-D/COES se establecieron los límites de transmisión de potencia hacia el Área Operativa Sur, vigente desde el miércoles 01 de junio de 2022.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);

                Hyperlink lnk = document.AddHyperlink("https://www.coes.org.pe/Portal/MarcoNormativo/DecisionesEjecutivasNotasTecnicas", new Uri("https://www.coes.org.pe/Portal/MarcoNormativo/DecisionesEjecutivasNotasTecnicas"));
                p.AppendHyperlink(lnk).FontSize(11).Font(FontDoc).UnderlineStyle(UnderlineStyle.singleLine).Color(ColorEnlace).AppendLine().Append(" ").Font(FontDoc);

                if (model.ListaCongestion.Any())
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Se presenta el siguiente cuadro:").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                    //Tabla                
                    TablaWordCongestion(document, model.ListaCongestion);
                    p = document.InsertParagraph();
                }
                else
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("No hubo congestión.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                }

                #endregion

                #region 1.12. CALIDAD DE PRODUCTO (FRECUENCIA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.12. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("CALIDAD DE PRODUCTO (FRECUENCIA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);

                //Tabla frecuencia min-max
                p = document.InsertParagraph();
                p.Append(model.GpsNombre).FontSize(11).Font(FontDoc);

                TablaWordFrecuenciaMinMax(document, model.ListaFrecuenciaMinMax);
                p = document.InsertParagraph();

                //Tabla                
                TablaWordPeriodoVariacion(document, model.ListaPeriodoRevision);
                p = document.InsertParagraph();

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("(*) Estos valores de frecuencia son calculados según lo dispuesto por la NTCSE para verificar la tolerancia de las variaciones súbitas (1 minuto).").FontSize(8).Font(FontDoc).Italic().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("(**) Estos valores de frecuencia son calculados según lo dispuesto por la NTCSE para verificar la tolerancia de las variaciones sostenidas (15 minutos).").FontSize(8).Font(FontDoc).Italic().AppendLine().Append(" ").Font(FontDoc);

                //tabla
                TablaWordIndicadorCalidad(document, model.ListaIndicadorCalidad);
                p = document.InsertParagraph();

                //gráfico 01         
                var jsonDataLineasFrecSein = GetGraficoHighchartFrecSein(model.GraficoFrecSein);
                await AgregarImagenHighchart(document, "1.12_1", jsonDataLineasFrecSein, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //gráfico 02
                var jsonDataFrecCampana = GetGraficoHighchartFrecuenciaCampana(model.GraficoCampanaFrec);
                await AgregarImagenHighchart(document, "1.12_2", jsonDataFrecCampana, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //tabla
                TablaWordFrecuenciaRango(document, model.ListaFrecRango);
                p = document.InsertParagraph();

                //tabla
                TablaWordFrecuenciaUmbral(document, model.ListaFrecDebajo, true);
                p = document.InsertParagraph();

                //tabla
                TablaWordFrecuenciaUmbral(document, model.ListaFrecEncima, false);
                p = document.InsertParagraph();

                #endregion

                #region 1.13. FLUJOS POR LAS INTERCONEXIONES(MW)

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.13. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("FLUJOS POR LAS INTERCONEXIONES(MW)").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);


                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Se muestra la evolución horaria de los flujos por las interconexiones.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                //gráfico 01
                var ghChart131 = GetGraficoHighchartFlujoInterconexion(model.GraficoInterc01, false);
                await AgregarImagenHighchart(document, "1.13_1", ghChart131, rutaArchivoTemporal, rutaArchivoDefault);

                //gráfico 02
                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                var ghChart132 = GetGraficoHighchartFlujoInterconexion(model.GraficoInterc02, false);
                await AgregarImagenHighchart(document, "1.13_2", ghChart132, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                //gráfico 03
                //var ghChart133 = GetGraficoHighchartFlujoInterconexion(model.GraficoInterc03, true);
                //await AgregarImagenHighchart(document, "1.13_3", ghChart133, rutaArchivoTemporal, rutaArchivoDefault);

                //p = document.InsertParagraph().Font(FontDoc);
                //p.Append("Medido en las SS.EE.Tintaya y Puno").Italic().FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc).Italic();
                //p.Alignment = Alignment.center;

                #endregion

                #region 1.14. Interconexiones Internacionales

                if (model.IncluyeEcuador)
                {
                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("1.14. ").FontSize(11).Font(FontDoc).Bold();
                    p.Append("Interconexiones Internacionales").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                    p.AppendLine().Append(" ").Font(FontDoc);

                    if (model.ListaInterconexionesInternacional.Any())
                    {
                        //tabla
                        TablaWordInterconexiones(document, model.ListaInterconexionesInternacional);
                        p = document.InsertParagraph();
                    }
                    else
                    {
                        p = document.InsertParagraph().Font(FontDoc);
                        p.Append("No se presenta interconexiones internacionales.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);
                        p = document.InsertParagraph().Font(FontDoc);
                    }

                    //gráfico
                    var ghChart114 = GetGraficoHighchartInterconexionInternacional(model.GraficoInterconexionesInternacional);
                    await AgregarImagenHighchart(document, "1.14", ghChart114, rutaArchivoTemporal, rutaArchivoDefault);

                    p = document.InsertParagraph().Font(FontDoc);
                    p.Append("Medido en la S.E. Zorritos").Italic().FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc).Italic();
                    p.Alignment = Alignment.center;

                }

                #endregion

                #endregion

                #region "2. EVALUACIÓN ECONÓMICA"

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("2. ").FontSize(11).Font(FontDoc).Bold().Color(ColorTituloSeccion);
                AddSubtitulo(ref p, "EVALUACIÓN ECONÓMICA", FontDoc, true);

                #region 2.1. COSTO TOTAL DE LA OPERACIÓN POR DÍA

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("2.1. ").FontSize(11).Font(FontDoc).Bold();
                p.Append("COSTO TOTAL DE LA OPERACIÓN POR DÍA").FontSize(11).Font(FontDoc).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                p.AppendLine().Append(" ").Font(FontDoc);
                p = document.InsertParagraph().Font(FontDoc);
                textoparrafo = model.MensajeCosto;
                p.Append(textoparrafo).FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                //Gráfico
                var ghChart21 = GetGraficoHighchartCostoTotal(model.GraficoCosto);
                await AgregarImagenHighchart(document, "2.1", ghChart21, rutaArchivoTemporal, rutaArchivoDefault);

                p = document.InsertParagraph().Font(FontDoc);
                p.AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #endregion

                #region "3. OBSERVACIONES"

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("3. ").FontSize(11).Font(FontDoc).Bold(); //.Color(ColorTituloSeccion);
                AddSubtitulo(ref p, "OBSERVACIONES", FontDoc, false);


                p = document.InsertParagraph().Font(FontDoc);
                p.Append("3.1 Los datos utilizados en los numerales 1.1.1, 1.1.2, 1.1.3, 1.1.4, 1.2, 1.3, 1.8.3.B y 1.12 provienen de datos instantáneos del sistema SCADA de las Empresas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("3.2 El informe incluye los siguientes anexos: Generación ejecutada activa y reactiva, desviación del despacho de las centrales, información hidrológica, compromisos y transferencias por RPF, demandas por áreas operativas, stock y consumo de combustibles sólidos, líquidos y gaseosos. Además los reportes de Horas de operación de las unidades térmicas y de mantenimientos ejecutados.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #endregion

                #region "4. ANEXOS"

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("4. ").FontSize(11).Font(FontDoc).Bold(); //.Color(ColorTituloSeccion);
                AddSubtitulo(ref p, "ANEXOS", FontDoc, false);
                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 1: Resumen de la operación").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.1 RESUMEN - Resumen de generación de energía eléctrica por Empresas Integrantes del COES.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.2 G_AREAS - Generación por tipo de generación por áreas operativas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.3 GENERACION RER -Generación eléctrica de las centrales RER(MW).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.4 COGENERACION - Centrales de Cogeneración del SEIN.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.5 TIPO_RECURSO – Reporte de potencia generada por tipo de recurso.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.6 DESPACHO_EJECUTADO - Potencia activa ejecutada de las unidades de generación del SEIN(MW).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.7 PROGRAMADO - Programación diaria.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.8 REPROGRAMADO - Reprogramación diaria.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.9 DESVIACIÓN - Desviación del despacho de las centrales de generación COES(MW).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.10 REACTIVA - Potencia reactiva ejecutada de las unidades de generación del COES(MVAR).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.11 DEMANDA_UL - Potencia de los grandes Usuarios Libres (MW).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.12 DEMANDA_AREAS - Reporte de la demanda por áreas y sub - áreas operativas (MW).").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.13 FLUJOS - Potencia activa líneas de transmisión del SEIN.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.14 INTERCONEXIONES - Interconexión entre sistemas operativos del SEIN.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.15 STOCK_COMB - Reporte de stock de combustibles.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.16 ENERGÍA_PRIMARIA – Reporte de fuente de Energía Primaria de las unidades RER.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.17 CALOR_ÚTIL – Registro de Calor Útil.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.18 STOCK_COMB - Reporte de stock de combustibles.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.19 CONSUMO_COMB - Reporte de consumo de combustibles.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.20 PRESIÓN_GAS - Presiones de gas natural de las centrales termoeléctricas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.21 TEMP_AMB - Temperatura Ambiente de las centrales termoeléctricas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.22 DISPONIBILIDAD_GAS – Reporte de disponibilidad de Gas Natural.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.23 TRANSGRESIONES – Reporte de GPS.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("1.24 RESTRIC_OPE - Restricciones Operativas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 2: Hidrología de la operación").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("2.1 Principales caudales de los principales afluentes y volúmenes de embalses y reservorios.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("2.2 Vertimientos de Embalses.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("2.3 Descarga de Lagunas.").FontSize(11).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 3: Asignación de reserva Primaria y secundaria del SEIN.").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 4: Horas de operación de las unidades térmicas del SEIN.").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 5: Mantenimientos ejecutados del SEIN.").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Anexo 6: Costo Marginal de Corto Plazo del SEIN.").FontSize(11).Font(FontDoc).Bold().AppendLine().Append(" ").Font(FontDoc);

                //tabla
                TablaWordElaboracion(document, model);
                p = document.InsertParagraph().Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                var textolinea = string.Format("Hora de emisión del informe: {0} h.", DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora));
                p.Append(textolinea).FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                textolinea = string.Format("Fecha: {0}", DateTime.Now.ToString("dd.MM.yyyy"));
                p.Append(textolinea).FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("Difusión: SGI, SEV, SPR, SME, SCO, SNP, SPL, DP, DO, CC - INTEGRANTES.").FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("NOTA1: Las siglas utilizadas en el presente documento están de acuerdo a la 'Base Metodológica para la Aplicación de la Norma Técnica de Calidad de los Servicios Eléctricos'.").FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                p = document.InsertParagraph().Font(FontDoc);
                p.Append("NOTA2: Para el cálculo de la demanda del COES, se considera solo la generación de los Integrantes del COES.").FontSize(9).Font(FontDoc).AppendLine().Append(" ").Font(FontDoc);

                #endregion

                document.Save();
            }

            return 1;
        }

        /// <summary>
        /// Get imagen desde Highcharts
        /// </summary>
        /// <param name="options"></param>
        /// <param name="rutaImagen"></param> 
        /// <returns></returns>
        private async Task<string> ObtenerLinkImagenDesdeJsonString(string idGrafico, object options, string rutaImagen, string rutaArchivoDefault)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            string json = new JavaScriptSerializer().Serialize(new
            {
                infile = JsonConvert.SerializeObject(options, jsonSerializerSettings)
            });

            HttpWebResponse httpResponse = null;
            System.IO.Stream responseStream = null;
            try
            {
                /*if (idGrafico != "1.12_2")
                    return rutaArchivoDefault;*/

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://export.highcharts.com/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.UserAgent = "GestionAnexoA";
                httpWebRequest.Referer = "https://coes.org.pe";

                /*Debug.WriteLine("Gráfico: ");
                Debug.WriteLine(json);*/

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                if (System.IO.File.Exists(rutaImagen))
                    System.IO.File.Delete(rutaImagen);

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                responseStream = httpResponse.GetResponseStream();
                Bitmap chartPNG = new Bitmap(responseStream);
                chartPNG.Save(rutaImagen);
            }
            catch (Exception ex)
            {
                Logger.Error("Gráfico: " + idGrafico + ". JSON: " + json, ex);

                rutaImagen = rutaArchivoDefault;
            }
            finally
            {
                try
                {
                    if (responseStream != null) responseStream.Close();
                    if (httpResponse != null) httpResponse.Close();
                }
                catch (Exception e) { }
            }

            return rutaImagen;
        }

        #region Tablas y gráficos

        /// <summary>
        /// Configura los textos para los subtitulos
        /// </summary>
        /// <param name="p"></param>
        /// <param name="descripcion"></param>
        private static void AddSubtitulo(ref Paragraph p, string descripcion, FontFamily fontDoc, bool tieneColorRojo)
        {
            var color = tieneColorRojo ? ColorTituloSeccion : ColorNegro;
            p.Append(descripcion).FontSize(12).Font(fontDoc).Bold().Color(color).UnderlineStyle(UnderlineStyle.singleLine);
            p.AppendLine().Append(" ").Font(fontDoc);
        }

        private async Task<int> AgregarImagenHighchart(DocX document, string idGrafico, object objetoHighchart, string rutaImagen, string rutaArchivoDefault)
        {
            string rutaImagenHighchart = rutaArchivoDefault;
            for (int i = 0; i < 5; i++)
            {
                //consultar cada 10 segundos a Highcharts para evitar bloqueos del servidor
                await Task.Delay(1000 * 15);

                rutaImagenHighchart = await ObtenerLinkImagenDesdeJsonString(idGrafico, objetoHighchart, rutaImagen, rutaArchivoDefault);

                //si la llamada al servidor Highchart trae error ((400) Bad Request) entonces volver a tratar de generar la imagen despues de un tiempo de espera
                if (rutaImagenHighchart == rutaArchivoDefault)
                {
                    await Task.Delay(1000 * 30);
                }
                else 
                {
                    //si la imagen es correcta entonces continuar la generación del word
                    break;
                }
            }

            Novacode.Image imageLinea = document.AddImage(rutaImagenHighchart);
            Table tablaGraficos = document.InsertTable(1, 1);
            tablaGraficos.AutoFit = AutoFit.Contents;
            tablaGraficos.Design = TableDesign.TableGrid;
            tablaGraficos.Alignment = Alignment.center;

            tablaGraficos.Rows[0].Cells[0].Paragraphs[0].AppendPicture(imageLinea.CreatePicture(HeighImagePX, WidthImagePX));
            tablaGraficos.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            tablaGraficos.Rows[0].Cells[0].Paragraphs[0].Append(" \n ");

            return 1;
        }

        #region 1.1.1. EVOLUCIÓN HORARIA DE LA DEMANDA TOTAL

        private static void TablaWordListaReprogramas(DocX document, List<EveMailsDTO> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("Hora", 65, 10,10, Alignment.center),
                        new WordCelda("Reprograma", 180, 10,10, Alignment.center),
                        new WordCelda("Motivo", 395, 10,10, Alignment.left)
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Hora);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Reprograma);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Mailreprogcausa);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        private static void TablaWordVelocidadCrecimiento(DocX document, ModelWordAnexoA model)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("", 200, 10,10, Alignment.center),
                        new WordCelda("SEIN", 110, 10,10, Alignment.center),
                        new WordCelda("NORTE",110 , 10,10, Alignment.center),
                        new WordCelda("SUR", 110, 10,10, Alignment.center),
                        new WordCelda("CENTRO", 110, 10,10, Alignment.center),
                    };
            List<WordCelda> listaFila = new List<WordCelda>
                    {
                        new WordCelda("Pendiente (MW/min)", 200, 10,10, Alignment.center),
                        new WordCelda("Hora de Toma de Carga", 200, 10,10, Alignment.center),
                    };

            int nroRowData = 2;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            tabla.Rows[1].Cells[1].Paragraphs[0].Append(model.LsPendienteMaxSEINhoy.ToString());
            tabla.Rows[2].Cells[1].Paragraphs[0].Append(model.LsHoraPendMaxSEIN);

            tabla.Rows[1].Cells[2].Paragraphs[0].Append(model.LsPendienteMaxNorte.ToString());
            tabla.Rows[2].Cells[2].Paragraphs[0].Append(model.LsHoraPendMaxNorte);

            tabla.Rows[1].Cells[3].Paragraphs[0].Append(model.LsPendienteMaxSur.ToString());
            tabla.Rows[2].Cells[3].Paragraphs[0].Append(model.LsHoraPendMaxSur);

            tabla.Rows[1].Cells[4].Paragraphs[0].Append(model.LsPendienteMaxCentro.ToString());
            tabla.Rows[2].Cells[4].Paragraphs[0].Append(model.LsHoraPendMaxCentro);

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, 2, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera y primera columna
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc);
            UtilWord.FormatearColumna1TablaWord(ref tabla, listaFila, FontDoc);
        }

        private static ExpandoObject GetGraficoHighchartDemEjecyProg(GraficoWeb grafico, bool incluyeEcuador)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "line";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleY = new ExpandoObject();
            dynamic titleX = new ExpandoObject();
            titleY.text = "MW";
            titleX.text = "Hora";

            dynamic labelsX = new ExpandoObject();
            //labelsX.step = 4;
            labelsX.rotation = -90; // para mostrar etiquetas verticales
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.title = titleX;
            xAxis.labels = labelsX;
            xAxis.gridLineWidth = 1;

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value:,.0f}";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.labels = labelsY;
            yAxis.gridLineWidth = 1;
            yAxis.min = grafico.YaxixMin;
            yAxis.max = grafico.YaxixMax;
            yAxis.startOnTick = false;
            yAxis.endOnTick = false;

            dynamic dataLabels = new ExpandoObject();
            dataLabels.enabled = true;
            dataLabels.y = -10;
            dataLabels.format = "{point.y} MW";
            dynamic plotSerie = new ExpandoObject();
            plotSerie.dataLabels = dataLabels;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.series = plotSerie;
            dynamic legend = new ExpandoObject();
            legend.symbolWidth = 30;

            //bool incluyeEcuador = grafico.SeriesName.Count == 4;
            List<ExpandoObject> series = new List<ExpandoObject>();
            for (int i = 0; i < grafico.SeriesName.Count; i++)
            {
                List<ExpandoObject> listaPunto = new List<ExpandoObject>();

                string color = "";
                string dashStyle = "";
                int lineWidth = 2;
                dynamic markerXGraf = new ExpandoObject();
                if (incluyeEcuador)
                {
                    switch (i)
                    {
                        case 0: //ejecutado con Ecuador
                            markerXGraf.symbol = "square";
                            markerXGraf.radius = 2;
                            color = "#000000"; //negro
                            break;
                        case 1: //ejecutado sin Ecuador
                            markerXGraf.enabled = false;
                            color = "#098409"; //verde
                            break;
                        case 2: //programa
                            markerXGraf.symbol = "circle";
                            markerXGraf.radius = 2;
                            color = "#002BFF"; //azul
                            break;
                        case 3: //reprograma 1
                            markerXGraf.enabled = false;
                            color = "#ffa500"; //naranja
                            dashStyle = "longdash";
                            break;
                        case 4: //reprograma 2
                            markerXGraf.enabled = false;
                            color = "#FF0000"; //rojo
                            dashStyle = "longdash";
                            break;
                        case 5: //reprograma 3
                            markerXGraf.enabled = false;
                            color = "#c100ff"; //morado
                            break;
                        case 6: //reprograma 4
                            markerXGraf.enabled = false;
                            color = "#098409"; //verde
                            break;
                        case 7: //reprograma 5
                            markerXGraf.enabled = false;
                            color = "#5c77ff"; //azul2
                            break;
                        default:
                            markerXGraf.enabled = false;
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0: //ejecutado
                            markerXGraf.enabled = false;
                            color = "#098409"; //verde
                            break;
                        case 1: //programa
                            markerXGraf.enabled = false;
                            color = "#0C67FF"; //azul
                            break;
                        case 2: //reprograma 1
                            markerXGraf.enabled = false;
                            color = "#F4A70E"; //naranja
                            dashStyle = "longdash";
                            break;
                        case 3: //reprograma 2
                            markerXGraf.enabled = false;
                            color = "#FF1F1F"; //rojo
                            dashStyle = "longdash";
                            break;
                        case 4: //reprograma 3
                            markerXGraf.enabled = false;
                            color = "#AB44A0"; //morado
                            break;
                        case 5: //reprograma 4
                            markerXGraf.enabled = false;
                            color = "#63B246"; //verde
                            break;
                        case 6: //reprograma 5
                            markerXGraf.enabled = false;
                            color = "#295163"; //azul2
                            break;
                        default:
                            markerXGraf.enabled = false;
                            break;
                    }
                }

                for (var k = 0; k < 48; k++)
                {
                    dynamic dataLabelsPunto = new ExpandoObject();
                    dataLabelsPunto.enabled = grafico.SeriesDataVisible[i][k];
                    dynamic punto = new ExpandoObject();
                    if (grafico.SeriesData[i][k] != null) punto.y = Math.Round(grafico.SeriesData[i][k].Value, 1);
                    punto.dataLabels = dataLabelsPunto;
                    listaPunto.Add(punto);
                }

                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.SeriesName[i];
                objSerie.marker = markerXGraf;
                objSerie.data = listaPunto;
                objSerie.color = color;
                objSerie.lineWidth = lineWidth;
                objSerie.dashStyle = dashStyle;
                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.legend = legend;
            highchart.series = series;

            return highchart;
        }

        #endregion

        #region 1.1.2. EVOLUCIÓN HORARIA DE LA DEMANDA POR ÁREA

        private static ExpandoObject GetGraficoHighchartDemandaArea(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 4;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.labels = labelsX;

            dynamic style1 = new ExpandoObject();
            style1.color = "blue";
            dynamic title1 = new ExpandoObject();
            title1.text = grafico.Series[0].YAxisTitle;
            title1.style = style1;
            title1.align = "high";
            title1.rotation = 0;
            title1.y = -15;
            title1.offset = -100;
            dynamic labels1 = new ExpandoObject();
            labels1.format = "{value:,.0f}";
            labels1.style = style1;
            dynamic yAxis1 = new ExpandoObject();
            yAxis1.title = title1;
            yAxis1.labels = labels1;
            yAxis1.gridLineWidth = 1;

            dynamic style2 = new ExpandoObject();
            style2.color = "red";
            dynamic title2 = new ExpandoObject();
            title2.text = grafico.Series[2].YAxisTitle;
            title2.style = style2;
            title2.align = "high";
            title2.rotation = 0;
            title2.y = -15;
            title2.offset = -70;
            dynamic labels2 = new ExpandoObject();
            labels2.format = "{value:,.0f}";
            labels2.style = style2;
            dynamic yAxis2 = new ExpandoObject();
            yAxis2.title = title2;
            yAxis2.labels = labels2;
            yAxis2.opposite = true;
            yAxis2.gridLineWidth = 1;

            List<ExpandoObject> yAxis = new List<ExpandoObject> { yAxis1, yAxis2 };

            dynamic legend = new ExpandoObject();
            legend.align = "center";
            legend.verticalAlign = "bottom";
            legend.layout = "horizontal";
            legend.floating = false;

            dynamic hover = new ExpandoObject();
            hover.lineWidth = 5;
            dynamic states = new ExpandoObject();
            states.hover = hover;
            dynamic marker = new ExpandoObject();
            marker.enabled = false;
            dynamic spline = new ExpandoObject();
            spline.lineWidth = 1;
            spline.marker = marker;
            spline.states = states;

            dynamic plotOptions = new ExpandoObject();
            plotOptions.spline = spline;

            List<ExpandoObject> series = new List<ExpandoObject>();

            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.type = grafico.Series[i].Type;
                objSerie.yAxis = grafico.Series[i].YAxis;
                objSerie.data = grafico.SeriesData[i].ToArray();
                objSerie.color = grafico.Series[i].Color;

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.legend = legend;
            highchart.plotOptions = plotOptions;
            highchart.series = series;

            return highchart;
        }

        #endregion

        #region 1.1.3 EVOLUCIÓN HORARIA DE LAS CARGAS MAS IMPORTANTES

        private static ExpandoObject GetGraficoHighchartGUlibre(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.zoomType = "xy";
            chart.height = 975; //0.75
            chart.width = 1300; //1.0

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic labels1 = new ExpandoObject();
            labels1.rotation = -90;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.labels = labels1;

            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            dynamic labels = new ExpandoObject();
            labels.format = "{value}";

            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.labels = labels;

            dynamic legend = new ExpandoObject();
            legend.align = "center";
            legend.verticalAlign = "bottom";
            legend.layout = "horizontal";

            dynamic hover = new ExpandoObject();
            hover.lineWidth = 5;
            dynamic states = new ExpandoObject();
            states.hover = hover;
            dynamic marker = new ExpandoObject();
            marker.enabled = true;
            dynamic spline = new ExpandoObject();
            spline.lineWidth = 1;
            spline.marker = marker;
            spline.states = states;

            dynamic plotOptions = new ExpandoObject();
            plotOptions.spline = spline;

            List<ExpandoObject> series = new List<ExpandoObject>();

            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.type = grafico.Series[i].Type;
                objSerie.yAxis = grafico.Series[i].YAxis;
                objSerie.data = grafico.SeriesData[i].ToArray();

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.legend = legend;
            highchart.plotOptions = plotOptions;
            highchart.series = series;

            return highchart;
        }

        #endregion

        #region 1.1.4.RECURSOS ENERGÉTICOS Y DIAGRAMA DE DURACIÓN DE CARGA

        private static ExpandoObject GetGraficoHighchartDiagramaDuracion(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "area";
            chart.height = 500;
            chart.width = 900;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleX = new ExpandoObject();
            titleX.text = "MEDIAS HORAS";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.SeriesName.ToArray();
            xAxis.title = titleX;

            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            dynamic labels = new ExpandoObject();
            labels.format = "{value}";

            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.labels = labels;
            yAxis.lineWidth = 0.2m;

            dynamic legend = new ExpandoObject();
            legend.align = "center";
            legend.verticalAlign = "bottom";
            legend.layout = "horizontal";

            dynamic marker = new ExpandoObject();
            marker.enabled = false;
            dynamic area = new ExpandoObject();
            area.stacking = "normal";
            area.lineColor = "#ffffff";
            area.lineWidth = 0.1;
            area.marker = marker;

            dynamic plotOptions = new ExpandoObject();
            plotOptions.area = area;

            List<ExpandoObject> series = new List<ExpandoObject>();

            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.data = grafico.Series[i].Data.Select(z => z.Y).ToArray();

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.legend = legend;
            highchart.plotOptions = plotOptions;
            highchart.series = series;

            return highchart;
        }

        #endregion

        #region 1.2. EVOLUCIÓN DIARIA DE LA PRODUCCIÓN DE ENERGÍA

        private static ExpandoObject GetGraficoHighchartEvolucionEnergia(GraficoWeb grafico, string textoAnio, decimal valorAnio)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleX = new ExpandoObject();
            titleX.text = grafico.XAxisTitle;
            titleX.align = "high";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.title = titleX;

            dynamic labelPlotLine1 = new ExpandoObject();
            labelPlotLine1.text = textoAnio;
            List<ExpandoObject> plotLines = new List<ExpandoObject>();
            dynamic plotLine1 = new ExpandoObject();
            plotLine1.value = valorAnio + 20;
            plotLine1.color = "#FF0000";
            plotLine1.width = 0;
            plotLine1.zIndex = 4;
            plotLine1.label = labelPlotLine1;
            plotLines.Add(plotLine1);

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            titleY.align = "high";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.labels = labelsY;
            yAxis.plotLines = plotLines;

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic dataLabels = new ExpandoObject();
            dataLabels.enabled = true;
            dataLabels.y = -10;
            dataLabels.format = "{point.y} GWh";
            dynamic plotSerie = new ExpandoObject();
            plotSerie.dataLabels = dataLabels;
            dynamic column = new ExpandoObject();
            column.pointPadding = 0.2;
            column.borderWidth = 0;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;
            plotOptions.series = plotSerie;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                var serie = grafico.Series[i];

                List<ExpandoObject> dataSerie = new List<ExpandoObject>();
                for (var j = 0; j < grafico.SerieDataS[i].Count(); j++)
                {
                    var auxSerie = grafico.SerieDataS[i][j];

                    dynamic dataLabelsPunto = new ExpandoObject();
                    dataLabelsPunto.enabled = grafico.SeriesDataVisible[i][j];

                    dynamic punto = new ExpandoObject();
                    punto.y = auxSerie.Y;
                    punto.name = auxSerie.Name;
                    punto.dataLabels = dataLabelsPunto;
                    dataSerie.Add(punto);
                }

                dynamic objSerie = new ExpandoObject();
                objSerie.name = serie.Name;
                objSerie.type = serie.Type;
                objSerie.color = serie.Color;
                objSerie.data = dataSerie;

                if (i == 2) //linea del año
                {
                    dynamic markerXGraf = new ExpandoObject();
                    markerXGraf.enabled = false;
                    objSerie.marker = markerXGraf;
                }

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        #endregion        

        #region 1.3. MÁXIMA GENERACIÓN INSTANTÁNEA 

        private static void TablaWordMaxGenInstant(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("DÍA", 100, 10,10, Alignment.center),
                        new WordCelda("EJECUTADO", 100, 10,10, Alignment.center),
                        new WordCelda("", 100, 10,10, Alignment.center),
                        new WordCelda("PROGRAMADO",100 , 10,10, Alignment.center),
                        new WordCelda("",100 , 10,10, Alignment.center),
                        new WordCelda("DESVIACIÓN \n(%)", 140, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("", 100, 10,10, Alignment.center),
                        new WordCelda("MW", 100, 10,10, Alignment.center),
                        new WordCelda("HORA",100 , 10,10, Alignment.center),
                        new WordCelda("MW", 100, 10,10, Alignment.center),
                        new WordCelda("HORA",100 , 10,10, Alignment.center),
                        new WordCelda("", 140, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;
            Table tabla = document.InsertTable(nroRowData + 2, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 2;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Dia);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.EjeMW);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.EjeHora);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.ProgMW);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.ProgHora);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Desv);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, 4, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc);

            tabla.MergeCellsInColumn(0, 0, 1);
            tabla.MergeCellsInColumn(5, 0, 1);
            tabla.Rows[0].MergeCells(1, 2);
            tabla.Rows[0].MergeCells(2, 3);
        }

        private static ExpandoObject GetGraficoHighchartEvolucionMaximaDemanda(GraficoWeb grafico, string textoMD, decimal valorMD)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleX = new ExpandoObject();
            titleX.text = grafico.XAxisTitle;
            titleX.align = "high";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.title = titleX;

            dynamic labelPlotLine1 = new ExpandoObject();
            labelPlotLine1.text = textoMD;
            List<ExpandoObject> plotLines = new List<ExpandoObject>();
            dynamic plotLine1 = new ExpandoObject();
            plotLine1.value = valorMD + 20;
            plotLine1.color = "#FF0000";
            plotLine1.width = 0;
            plotLine1.zIndex = 4;
            plotLine1.label = labelPlotLine1;
            plotLines.Add(plotLine1);

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            titleY.align = "high";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.labels = labelsY;
            yAxis.plotLines = plotLines;

            dynamic legend = new ExpandoObject();
            legend.enabled = false;

            dynamic dataLabels = new ExpandoObject();
            dataLabels.enabled = true;
            dataLabels.y = -10;
            dataLabels.format = "{point.y} MW";
            dynamic plotSerie = new ExpandoObject();
            plotSerie.dataLabels = dataLabels;
            dynamic column = new ExpandoObject();
            column.pointPadding = 0.2;
            column.borderWidth = 0;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;
            plotOptions.series = plotSerie;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                var serie = grafico.Series[i];

                List<ExpandoObject> dataSerie = new List<ExpandoObject>();
                for (var j = 0; j < grafico.SerieDataS[i].Count(); j++)
                {
                    var auxSerie = grafico.SerieDataS[i][j];

                    dynamic dataLabelsPunto = new ExpandoObject();
                    dataLabelsPunto.enabled = grafico.SeriesDataVisible[i][j];

                    dynamic punto = new ExpandoObject();
                    punto.y = auxSerie.Y;
                    punto.name = auxSerie.Name;
                    punto.dataLabels = dataLabelsPunto;
                    dataSerie.Add(punto);
                }

                dynamic objSerie = new ExpandoObject();
                objSerie.name = serie.Name;
                objSerie.type = serie.Type;
                objSerie.color = serie.Color;
                objSerie.data = dataSerie;

                if (i == 1) //linea del año
                {
                    dynamic markerXGraf = new ExpandoObject();
                    markerXGraf.enabled = false;
                    objSerie.marker = markerXGraf;
                }

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        #endregion

        #region 1.4. PRINCIPALES EVENTOS (FALLAS, INTERRUPCIONES Y RACIONAMIENTO)

        private static void TablaWordEvento(DocX document, List<EventoDTO> listaEvento, List<EveInterrupcionDTO> listaInterrup)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("HORA", 60, 8,8, Alignment.center),
                        new WordCelda("EMP.", 80, 8,8, Alignment.center),
                        new WordCelda("EVENTO", 370, 8,8, Alignment.left),
                        new WordCelda("OBSERVACIÓN", 130, 8,8, Alignment.left)
                    };

            List<WordCelda> listaColumnaSuministro = new List<WordCelda>
                    {
                        new WordCelda("SUMINISTRO", 170, 8,8, Alignment.center),
                        new WordCelda("POTENCIA\n (MW)", 90, 8,8, Alignment.center),
                        new WordCelda("TIEMPO\n (min)", 70, 8,8, Alignment.center),
                    };

            int nroRowData = listaEvento.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 1;
            foreach (var entity in listaEvento)
            {
                var lstEveInte = listaInterrup.Where(x => x.Evencodi == Convert.ToInt32(entity.EVENCODI)).ToList();

                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.EVENINI.Value.ToString(ConstantesAppServicio.FormatoHora));
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.EMPRABREV);

                tabla.Rows[index].Cells[2].Paragraphs[0].Append(""); //EVENASUNTO, EVENDESC
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.EVENASUNTO).Bold();
                tabla.Rows[index].Cells[2].Paragraphs[0].Append("\n\n");
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.EVENDESC);

                if (lstEveInte.Any())
                {
                    tabla.Rows[index].Cells[2].Paragraphs[0].Append("\n");

                    int nroRowDataSum = lstEveInte.Count + 1; //se adiciona el total
                    int nroColumnSum = listaColumnaSuministro.Count;

                    Table tablaSum = tabla.Rows[index].Cells[2].InsertTable(nroRowDataSum + 1, nroColumnSum);
                    tablaSum.AutoFit = AutoFit.ColumnWidth;
                    tablaSum.Design = TableDesign.TableGrid;
                    tablaSum.Alignment = Alignment.center;

                    UtilWord.FormatearFilaTablaWord(ref tablaSum, 0, listaColumnaSuministro, FontDoc); //tabla maximo 320px
                    int indexSum = 1;
                    foreach (var entitySum in lstEveInte)
                    {
                        tablaSum.Rows[indexSum].Cells[0].Paragraphs[0].Append(entitySum.PtoInterrupNomb);
                        tablaSum.Rows[indexSum].Cells[1].Paragraphs[0].Append(entitySum.Interrmw.GetValueOrDefault(0).ToString());
                        tablaSum.Rows[indexSum].Cells[2].Paragraphs[0].Append(entitySum.Interrminu.GetValueOrDefault(0).ToString());

                        indexSum++;
                    }
                    decimal total = lstEveInte.Sum(x => x.Interrmw.GetValueOrDefault(0));
                    tablaSum.Rows[indexSum].Cells[0].Paragraphs[0].Append("TOTAL RECHAZADO").Bold();
                    tablaSum.Rows[indexSum].Cells[1].Paragraphs[0].Append(total.ToString());

                    UtilWord.BodyTablaWord(ref tablaSum, 0, 0, nroRowDataSum, nroColumnSum - 1, listaColumnaSuministro, FontDoc);

                }
                tabla.Rows[index].Cells[2].InsertParagraph();

                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.EVENCOMENTARIOS);

                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.5. MANTENIMIENTOS PROGRAMADOS Y EJECUTADOS

        private static void TablaWordMantProgEjec(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("EMP", 55, 8,8, Alignment.center),
                        new WordCelda("UBICACIÓN", 100, 8,8, Alignment.center),
                        new WordCelda("EQUIPO",85 , 8,8, Alignment.center),
                        new WordCelda("HORA", 75, 8,8, Alignment.center),
                        new WordCelda("", 70, 8,8, Alignment.center),
                        new WordCelda("TIPO", 45, 8,8, Alignment.center),
                        new WordCelda("MOTIVO", 230, 8,8, Alignment.left),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("", 55, 8,8, Alignment.center),
                        new WordCelda("", 100, 8,8, Alignment.center),
                        new WordCelda("", 85, 8,8, Alignment.center),
                        new WordCelda("PROG.", 75, 8,8, Alignment.center),
                        new WordCelda("EJEC.", 70, 8,8, Alignment.center),
                        new WordCelda("", 45, 8,8, Alignment.center),
                        new WordCelda("", 210, 8,8, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;
            Table tabla = document.InsertTable(nroRowData + 2, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 2;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Empresa);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Prog);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Ejec);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Tipo);
                tabla.Rows[index].Cells[6].Paragraphs[0].Append(entity.Motivo);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData + 1, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc);

            tabla.MergeCellsInColumn(6, 0, 1);
            tabla.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;

            tabla.Rows[0].MergeCells(3, 4); //hora
            tabla.Rows[0].Cells[3].RemoveParagraphAt(1);

            tabla.MergeCellsInColumn(0, 0, 1);
            tabla.MergeCellsInColumn(1, 0, 1);
            tabla.MergeCellsInColumn(2, 0, 1);
            tabla.MergeCellsInColumn(4, 0, 1);
            tabla.MergeCellsInColumn(5, 0, 1);
        }

        #endregion

        #region 1.6.1. OPERACIÓN DE CALDEROS

        private static void TablaWordOpeCaldero(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("EMPRESA", 95, 10,10, Alignment.center),
                        new WordCelda("UBICACIÓN", 330, 10,10, Alignment.left),
                        new WordCelda("EQUIPO", 95, 10,10, Alignment.center),
                        new WordCelda("INICIO", 60, 10,10, Alignment.center),
                        new WordCelda("FINAL", 60, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Empresa);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Final);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.6.2. OPERACIÓN A CARGA MÍNIMA

        private static void TablaWordOpeCargaMin(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("EMPRESA", 95, 10,10, Alignment.center),
                        new WordCelda("UBICACIÓN", 330, 10,10, Alignment.left),
                        new WordCelda("EQUIPO", 95, 10,10, Alignment.center),
                        new WordCelda("INICIO", 60, 10,10, Alignment.center),
                        new WordCelda("FINAL", 60, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Empresa);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Final);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.7. RESERVA NO SINCRONIZADA DEL COES

        private static ExpandoObject GetGraficoHighchartReservaFria(GraficoWeb grafico)
        {
            /*dynamic chart1 = new ExpandoObject();
            return chart1;*/
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 4;
            dynamic titleX = new ExpandoObject();
            titleX.text = grafico.XAxisTitle;
            titleX.align = "high";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.SeriesName.ToArray();
            xAxis.crosshair = true;
            xAxis.title = titleX;
            xAxis.labels = labelsX;

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            titleY.align = "high";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.labels = labelsY;

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic dataLabels = new ExpandoObject();
            dataLabels.enabled = false;
            dataLabels.y = -10;
            dataLabels.format = "{point.y} MW";
            dynamic plotSerie = new ExpandoObject();
            plotSerie.dataLabels = dataLabels;
            plotSerie.pointWidth = 5;
            dynamic column = new ExpandoObject();
            column.pointPadding = 0.2;
            column.borderWidth = 0;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;
            plotOptions.series = plotSerie;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                var serie = grafico.Series[i];

                dynamic objSerie = new ExpandoObject();
                objSerie.name = serie.Name;
                objSerie.type = serie.Type;
                objSerie.color = serie.Color;
                objSerie.data = grafico.Series[i].Data.Select(z => z.Y).ToArray();

                if (i == 2) //reserva fria minima
                {
                    dynamic markerXGraf = new ExpandoObject();
                    markerXGraf.enabled = false;
                    objSerie.marker = markerXGraf;
                }
                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        private static ExpandoObject GetGraficoHighchartREficiente(GraficoWeb grafico)
        {
            /*dynamic chart1 = new ExpandoObject();
            return chart1;*/
            dynamic chart = new ExpandoObject();
            chart.type = "area";
            chart.height = 400;
            chart.width = 600;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic title1 = new ExpandoObject();
            title1.text = "";
            title1.enabled = true;
            dynamic subtitle = new ExpandoObject();
            subtitle.title = title1;

            dynamic labels1 = new ExpandoObject();
            labels1.rotation = -90;
            labels1.step = 4;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.SeriesName.ToArray();
            xAxis.labels = labels1;

            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.max = grafico.YaxixMax;
            yAxis.min = grafico.YaxixMin;
            yAxis.lineWidth = 0.2m;

            dynamic marker = new ExpandoObject();
            marker.enabled = false;
            dynamic area = new ExpandoObject();
            area.stacking = "normal";
            area.lineColor = "#ffffff";
            area.lineWidth = 0.1;
            area.marker = marker;

            dynamic plotOptions = new ExpandoObject();
            plotOptions.area = area;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.data = grafico.Series[i].Data.Select(z => z.Y).ToArray();
                objSerie.color = grafico.Series[i].Color;

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.subtitle = subtitle;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;

            return highchart;
        }

        #endregion

        #region 1.8. REGULACION DE TENSIÓN

        private static void TablaWordRegulacionTension(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("EMP", 75, 10,10, Alignment.center),
                        new WordCelda("UBICACIÓN", 150, 10,10, Alignment.center),
                        new WordCelda("EQUIPO", 95, 10,10, Alignment.center),
                        new WordCelda("INICIO", 60, 10,10, Alignment.center),
                        new WordCelda("FINAL", 60, 10,10, Alignment.center),
                        new WordCelda("MOTIVO", 200, 10,10, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Empresa);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Final);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Motivo);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.9.A PRUEBAS DE UNIDADES - A. ALEATORIAS DE DISPONIBILIDAD y B. POR REQUERIMIENTOS PROPIOS

        private static void TablaWordPruebasEquipos(DocX document, List<InfSGIDatosTablas> listaData, bool tieneColDescripcion = false)
        {
            string txtUltimaCol = "OBSERVACIÓN";
            if (tieneColDescripcion) txtUltimaCol = "DESCRIPCIÓN";

            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("EMP.", 70, 10,10, Alignment.center),
                        new WordCelda("UBICACIÓN", 160, 10,10, Alignment.center),
                        new WordCelda("EQUIPO", 100, 10,10, Alignment.center),
                        new WordCelda("INICIO", 60, 10,10, Alignment.center),
                        new WordCelda("FINAL", 60, 10,10, Alignment.center),
                        new WordCelda(txtUltimaCol, 190, 10,10, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Empresa);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Final);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Observacion);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.10. SISTEMAS AISLADOS

        private static void TablaWordSistemaAislado(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("UBICACIÓN", 100, 8,8, Alignment.center),
                        new WordCelda("INSTALACIÓN\n CAUSANTE", 100, 8,8, Alignment.center),
                        new WordCelda("INICIO", 60, 8,8, Alignment.center),
                        new WordCelda("FINAL", 60, 8,8, Alignment.center),
                        new WordCelda("OPERACIÓN \nDE CENTRALES", 90, 8,8, Alignment.center),
                        new WordCelda("MOTIVO", 115, 8,8, Alignment.left),
                        new WordCelda("SUBSISTEMA\n AISLADO",115 , 8,8, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Final);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.OperacionCentrales);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Motivo);
                tabla.Rows[index].Cells[6].Paragraphs[0].Append(entity.SubsistemaAislado);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.11. CONGESTIÓN

        private static void TablaWordCongestion(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("UBICACIÓN", 100, 8,8, Alignment.center),
                        new WordCelda("INSTALACIÓN\n DE \nTRANSMISIÓN \nAFECTADA", 100, 8,8, Alignment.center),
                        new WordCelda("INICIO", 60, 8,8, Alignment.center),
                        new WordCelda("FINAL", 60, 8,8, Alignment.center),
                        new WordCelda("UNIDADES \nGENERADORAS \nLIMITADAS", 160, 8,8, Alignment.left),
                        new WordCelda("OBSERVACIONES", 160, 8,8, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Ubicacion);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Final);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.UnidadLimitada);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Observacion);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #region 1.12. CALIDAD DE PRODUCTO (FRECUENCIA)

        private static void TablaWordPeriodoVariacion(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("PERIODO DE VARIACIONES SUBITAS (1 min)", 100, 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                        new WordCelda("", 100, 10,10, Alignment.center),
                        new WordCelda("",60 , 10,10, Alignment.center),
                        new WordCelda("PERIODO DE VARIACIONES SOSTENIDAS (15 min)",100 , 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                        new WordCelda("",100 , 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("Frecuencia Mínima(*)", 100, 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                        new WordCelda("Frecuencia Máxima(*)", 100, 10,10, Alignment.center),
                        new WordCelda("",60 , 10,10, Alignment.center),
                        new WordCelda("Frecuencia Mínima(**)",100 , 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                        new WordCelda("Frecuencia Máxima(**)",100 , 10,10, Alignment.center),
                        new WordCelda("", 60, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;
            Table tabla = document.InsertTable(nroRowData + 2, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 2;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Celda1);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Celda2);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Celda3);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Celda4);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Celda5);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Celda6);
                tabla.Rows[index].Cells[6].Paragraphs[0].Append(entity.Celda7);
                tabla.Rows[index].Cells[7].Paragraphs[0].Append(entity.Celda8);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, 3, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc);

            tabla.Rows[0].MergeCells(4, 7);
            tabla.Rows[0].MergeCells(0, 3);
            tabla.Rows[1].MergeCells(6, 7);
            tabla.Rows[1].MergeCells(4, 5);
            tabla.Rows[1].MergeCells(2, 3);
            tabla.Rows[1].MergeCells(0, 1);

            tabla.Rows[1].Cells[3].RemoveParagraphAt(1); //Frecuencia Máxima(**)
            tabla.Rows[1].Cells[2].RemoveParagraphAt(1);
            tabla.Rows[1].Cells[1].RemoveParagraphAt(1);
            tabla.Rows[1].Cells[0].RemoveParagraphAt(1); //Frecuencia Mínima(*)

            tabla.Rows[0].Cells[1].RemoveParagraphAt(3); //PERIODO DE VARIACIONES SOSTENIDAS (15 min)
            tabla.Rows[0].Cells[1].RemoveParagraphAt(2);
            tabla.Rows[0].Cells[1].RemoveParagraphAt(1);

            tabla.Rows[0].Cells[0].RemoveParagraphAt(3); //PERIODO DE VARIACIONES SUBITAS (1 min)
            tabla.Rows[0].Cells[0].RemoveParagraphAt(2);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(1);
        }

        private static void TablaWordIndicadorCalidad(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("INDICADOR DE CALIDAD", 110, 10,10, Alignment.left),
                        new WordCelda("PERIODO / HORA", 150, 10,10, Alignment.center),
                        new WordCelda("VALOR", 70, 10,10, Alignment.center),
                        new WordCelda("N° TRANSGRES. ACUMULADAS - MES",130 , 10,10, Alignment.center),
                        new WordCelda("TOLERANCIA NTCSE",90 , 10,10, Alignment.center),
                        new WordCelda("", 90, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("", 110, 10,10, Alignment.left),
                        new WordCelda("", 150, 10,10, Alignment.center),
                        new WordCelda("", 70, 10,10, Alignment.center),
                        new WordCelda("",130 , 10,10, Alignment.center),
                        new WordCelda("Hz",90 , 10,10, Alignment.center),
                        new WordCelda("", 90, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna3 = new List<WordCelda>
                    {
                        new WordCelda("", 110, 10,10, Alignment.left),
                        new WordCelda("", 150, 10,10, Alignment.center),
                        new WordCelda("", 70, 10,10, Alignment.center),
                        new WordCelda("",130 , 10,10, Alignment.center),
                        new WordCelda("Máx.",90 , 10,10, Alignment.center),
                        new WordCelda("Min.", 90, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;
            Table tabla = document.InsertTable(nroRowData + 3, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 3;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Celda1);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Celda2);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Celda3);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Celda4);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Celda5);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Celda6);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, 4, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc);
            UtilWord.FormatearFilaTablaWord(ref tabla, 2, listaColumna3, FontDoc);

            tabla.Rows[0].MergeCells(4, 5);
            tabla.Rows[1].MergeCells(4, 5);
            tabla.MergeCellsInColumn(3, 0, 2);
            tabla.MergeCellsInColumn(2, 0, 2);
            tabla.MergeCellsInColumn(1, 0, 2);
            tabla.MergeCellsInColumn(0, 0, 2);

            tabla.Rows[0].Cells[4].RemoveParagraphAt(1); //TOLERANCIA NTCSE
            tabla.Rows[1].Cells[4].RemoveParagraphAt(1); //Hz
        }

        private static ExpandoObject GetGraficoHighchartFrecSein(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.type = grafico.Type;
            chart.zoomType = "xy";
            chart.height = 400;
            chart.width = 600;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 8;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.labels = labelsX;

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value:,.1f}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            dynamic labelPlotLine1 = new ExpandoObject();
            labelPlotLine1.text = "Valor Min. Sostenida = 59.64 Hz";
            dynamic labelPlotLine2 = new ExpandoObject();
            labelPlotLine2.text = "Valor Max. Sostenida = 60.36 Hz";
            List<ExpandoObject> plotLines = new List<ExpandoObject>();
            dynamic plotLine1 = new ExpandoObject();
            plotLine1.value = ConstantesPR5ReportesServicio.FrecuenciaMinSostenida;
            plotLine1.color = "#FF0000";
            plotLine1.width = 2;
            plotLine1.label = labelPlotLine1;
            dynamic plotLine2 = new ExpandoObject();
            plotLine2.value = ConstantesPR5ReportesServicio.FrecuenciaMaxSostenida;
            plotLine2.color = "#FF0000";
            plotLine2.width = 2;
            plotLine2.label = labelPlotLine2;
            plotLines.Add(plotLine1);
            plotLines.Add(plotLine2);
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.plotLines = plotLines;
            yAxis.max = 60.8;
            yAxis.min = 59.2;
            yAxis.labels = labelsY;
            yAxis.startOnTick = false;
            yAxis.endOnTick = false;

            dynamic marker = new ExpandoObject();
            marker.enabled = false;
            dynamic line = new ExpandoObject();
            line.lineWidth = 2;
            line.marker = marker;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.line = line;

            dynamic legend = new ExpandoObject();
            legend.enabled = false;

            List<ExpandoObject> series = new List<ExpandoObject>();
            dynamic objSerie = new ExpandoObject();
            objSerie.name = grafico.SerieData[0].Name;
            objSerie.data = grafico.SerieData[0].Data;

            series.Add(objSerie);

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        private static ExpandoObject GetGraficoHighchartFrecuenciaCampana(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 2;
            dynamic titleX = new ExpandoObject();
            titleX.text = grafico.XAxisTitle;
            titleX.align = "high";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.title = titleX;
            xAxis.labels = labelsX;

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            titleY.align = "high";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.labels = labelsY;
            yAxis.max = grafico.YaxixMax;
            yAxis.tickPositions = grafico.SeriesYAxis.ToArray();

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic dataLabels = new ExpandoObject();
            dataLabels.enabled = false;
            dataLabels.y = -10;
            dataLabels.format = "{point.y}";
            dynamic plotSerie = new ExpandoObject();
            plotSerie.dataLabels = dataLabels;
            plotSerie.pointWidth = 5;
            dynamic column = new ExpandoObject();
            column.pointPadding = 0.2;
            column.borderWidth = 0;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;
            plotOptions.series = plotSerie;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                var serie = grafico.Series[i];

                dynamic objSerie = new ExpandoObject();
                objSerie.name = serie.Name;
                objSerie.type = serie.Type;
                objSerie.color = serie.Color;
                objSerie.data = grafico.SerieDataS[i].Select(z => z.Y).ToArray();

                if (i == 3)
                {
                    objSerie.zIndex = -1;
                    objSerie.showInLegend = false;
                }

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        private static void TablaWordFrecuenciaRango(DocX document, List<FLecturaDTO> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("Tiempo en que la frecuencia estuvo entre:", 128, 10,10, Alignment.center),
                        new WordCelda("", 128, 10,10, Alignment.center),
                        new WordCelda("", 128, 10,10, Alignment.center),
                        new WordCelda("", 128, 10,10, Alignment.center),
                        new WordCelda("", 128, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("Rango de Frecuencia", 128, 10,10, Alignment.center),
                        new WordCelda("", 128, 10,10, Alignment.center),
                        new WordCelda("MIN", 128, 10,10, Alignment.center),
                        new WordCelda("MED", 128, 10,10, Alignment.center),
                        new WordCelda("MAX", 128, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;

            Table tabla = document.InsertTable(nroRowData + 2, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 2;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.TextoRangoIni);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.TextoRangoFin);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.TextoMin);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.TextoMed);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.TextoMax);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData + 1, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc); //tabla maximo 640px

            tabla.Rows[0].MergeCells(0, 4);
            tabla.Rows[1].MergeCells(0, 1);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(4); //Tiempo en que la frecuencia estuvo entre:
            tabla.Rows[0].Cells[0].RemoveParagraphAt(3);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(2);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(1);
            tabla.Rows[1].Cells[0].RemoveParagraphAt(1); //Rango de Frecuencia
        }

        private static void TablaWordFrecuenciaUmbral(DocX document, List<FLecturaDTO> listaData, bool esDebajo)
        {
            string titulo = esDebajo ? "Veces que la frecuencia disminuyó por debajo de:" : "Veces que la frecuencia aumentó por encima de:";

            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda(titulo, 160, 10,10, Alignment.center),
                        new WordCelda("", 160, 10,10, Alignment.center),
                        new WordCelda("", 160, 10,10, Alignment.center),
                        new WordCelda("", 160, 10,10, Alignment.center),
                    };
            List<WordCelda> listaColumna2 = new List<WordCelda>
                    {
                        new WordCelda("Umbral de Frecuencia", 160, 10,10, Alignment.center),
                        new WordCelda("MIN", 160, 10,10, Alignment.center),
                        new WordCelda("MED", 160, 10,10, Alignment.center),
                        new WordCelda("MAX", 160, 10,10, Alignment.center),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;

            Table tabla = document.InsertTable(nroRowData + 2, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 2;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.TextoUmbral);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.TextoMin);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.TextoMed);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.TextoMax);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData + 1, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px
            UtilWord.FormatearFilaTablaWord(ref tabla, 1, listaColumna2, FontDoc); //tabla maximo 640px

            tabla.Rows[0].MergeCells(0, 3);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(3);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(2);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(1);
        }

        private static void TablaWordFrecuenciaMinMax(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna1 = new List<WordCelda>
                    {
                        new WordCelda("Frecuencia Mínima", 100, 10,10, Alignment.center),
                        new WordCelda("", 100, 10,10, Alignment.center),
                        new WordCelda("Frecuencia Máxima", 100, 10,10, Alignment.center),
                        new WordCelda("", 100, 10,10, Alignment.center)
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna1.Count;
            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //llenar datos
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.Celda1.Trim());
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.Celda2.Trim());
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Celda3.Trim());
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Celda4.Trim());
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna1, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna1, FontDoc); //tabla maximo 640px

            tabla.Rows[0].MergeCells(2, 3);
            tabla.Rows[0].MergeCells(0, 1);
            tabla.Rows[0].Cells[1].RemoveParagraphAt(1);
            tabla.Rows[0].Cells[0].RemoveParagraphAt(1);
        }

        #endregion

        #region 1.13. FLUJOS POR LAS INTERCONEXIONES (MW)

        private static ExpandoObject GetGraficoHighchartFlujoInterconexion(GraficoWeb grafico, bool esEnlaceSOSE)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 4;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.SeriesName.ToArray();
            xAxis.labels = labelsX;

            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic column = new ExpandoObject();
            column.stacking = true;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;

            if (esEnlaceSOSE)
            {
                dynamic labelPlotLine1 = new ExpandoObject();
                labelPlotLine1.text = "Flujo hacia el Sur Oeste";
                labelPlotLine1.align = "left";
                dynamic labelPlotLine2 = new ExpandoObject();
                labelPlotLine2.text = "Flujo hacia el Sur Este";
                labelPlotLine2.align = "right";
                List<ExpandoObject> plotLines = new List<ExpandoObject>();
                dynamic plotLine1 = new ExpandoObject();
                plotLine1.value = grafico.YaxixMax + 3;
                plotLine1.width = 0;
                plotLine1.label = labelPlotLine1;
                dynamic plotLine2 = new ExpandoObject();
                plotLine2.value = grafico.YaxixMin - 3;
                plotLine2.width = 0;
                plotLine2.label = labelPlotLine2;
                plotLines.Add(plotLine1);
                plotLines.Add(plotLine2);

                yAxis.plotLines = plotLines;

                legend.enabled = false;
            }

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.data = grafico.Series[i].Data.Select(x => x.Y).ToArray();
                objSerie.color = grafico.Series[i].Color;

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        #endregion

        #region 1.14. Interconexiones Internacionales

        private static void TablaWordInterconexiones(DocX document, List<InfSGIDatosTablas> listaData)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("País Importador", 95, 10,10, Alignment.center),
                        new WordCelda("País Exportador", 95, 10,10, Alignment.center),
                        new WordCelda("Equipo", 60, 10,10, Alignment.center),
                        new WordCelda("Inicio", 60, 10,10, Alignment.center),
                        new WordCelda("Final", 60, 10,10, Alignment.center),
                        new WordCelda("Observación", 270, 10,10, Alignment.left),
                    };

            int nroRowData = listaData.Count;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            int index = 1;
            foreach (var entity in listaData)
            {
                tabla.Rows[index].Cells[0].Paragraphs[0].Append(entity.PaisImp);
                tabla.Rows[index].Cells[1].Paragraphs[0].Append(entity.PaisExp);
                tabla.Rows[index].Cells[2].Paragraphs[0].Append(entity.Equipo);
                tabla.Rows[index].Cells[3].Paragraphs[0].Append(entity.Inicio);
                tabla.Rows[index].Cells[4].Paragraphs[0].Append(entity.Final);
                tabla.Rows[index].Cells[5].Paragraphs[0].Append(entity.Observacion);
                index++;
            }

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        private static ExpandoObject GetGraficoHighchartInterconexionInternacional(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;

            dynamic labelsX = new ExpandoObject();
            labelsX.step = 4;
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.SeriesName.ToArray();
            xAxis.labels = labelsX;

            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.min = grafico.YaxixMin;
            yAxis.max = grafico.YaxixMax;

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic column = new ExpandoObject();
            column.stacking = true;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;

            dynamic labelPlotLine1 = new ExpandoObject();
            labelPlotLine1.text = "Flujo hacia el Ecuador";
            labelPlotLine1.align = "right";
            dynamic labelPlotLine2 = new ExpandoObject();
            labelPlotLine2.text = "Flujo hacia el Perú";
            labelPlotLine2.align = "left";
            dynamic labelPlotLine3 = new ExpandoObject();
            labelPlotLine3.text = "Límite de Transporte = 70MW";

            List<ExpandoObject> plotLines = new List<ExpandoObject>();
            dynamic plotLine1 = new ExpandoObject();
            plotLine1.value = grafico.YaxixMax + 3; //exportación
            plotLine1.width = 0;
            plotLine1.label = labelPlotLine1;
            dynamic plotLine2 = new ExpandoObject();
            plotLine2.value = grafico.YaxixMin - 3; //importacion
            plotLine2.width = 0;
            plotLine2.label = labelPlotLine2;
            dynamic plotLine3 = new ExpandoObject();
            plotLine3.value = ConstantesPR5ReportesServicio.LimiteTransporteTIE;
            plotLine3.color = "#FF0000";
            plotLine3.width = 2;
            plotLine3.label = labelPlotLine3;

            plotLines.Add(plotLine1);
            plotLines.Add(plotLine2);
            plotLines.Add(plotLine3);

            yAxis.plotLines = plotLines;

            legend.enabled = false;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                dynamic objSerie = new ExpandoObject();
                objSerie.name = grafico.Series[i].Name;
                objSerie.data = grafico.Series[i].Data.Select(x => x.Y).ToArray();
                objSerie.color = grafico.Series[i].Color;

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        #endregion

        #region 2.1. Costo total de la operación

        private static ExpandoObject GetGraficoHighchartCostoTotal(GraficoWeb grafico)
        {
            dynamic chart = new ExpandoObject();
            chart.type = "column";
            chart.zoomType = "xy";
            chart.height = 450;
            chart.width = 800;

            dynamic title = new ExpandoObject();
            title.text = grafico.TitleText;
            dynamic styleTitle = new ExpandoObject();
            styleTitle.color = "#0000FF";
            styleTitle.fontWeight = "bold";
            title.style = styleTitle;

            dynamic titleX = new ExpandoObject();
            titleX.text = grafico.XAxisTitle;
            titleX.align = "high";
            dynamic xAxis = new ExpandoObject();
            xAxis.categories = grafico.XAxisCategories.ToArray();
            xAxis.crosshair = true;
            xAxis.title = titleX;

            dynamic labelsY = new ExpandoObject();
            labelsY.format = "{value}";
            dynamic titleY = new ExpandoObject();
            titleY.text = grafico.YaxixTitle;
            titleY.align = "high";
            dynamic yAxis = new ExpandoObject();
            yAxis.title = titleY;
            yAxis.lineWidth = 0.2m;
            yAxis.labels = labelsY;

            dynamic legend = new ExpandoObject();
            legend.enabled = true;

            dynamic column = new ExpandoObject();
            column.pointPadding = 0.2;
            column.borderWidth = 0;
            dynamic plotOptions = new ExpandoObject();
            plotOptions.column = column;

            List<ExpandoObject> series = new List<ExpandoObject>();
            for (var i = 0; i < grafico.Series.Count; i++)
            {
                var serie = grafico.Series[i];

                List<ExpandoObject> dataSerie = new List<ExpandoObject>();
                for (var j = 0; j < grafico.SerieDataS[i].Count(); j++)
                {
                    var auxSerie = grafico.SerieDataS[i][j];

                    dynamic aux = new ExpandoObject();
                    aux.y = auxSerie.Y;
                    aux.name = auxSerie.Name;

                    dataSerie.Add(aux);
                }

                dynamic dataLabels = new ExpandoObject();
                dataLabels.enabled = true;
                dataLabels.y = 0;
                dataLabels.align = "right";
                dataLabels.format = "{point.name}";

                dynamic objSerie = new ExpandoObject();
                objSerie.name = serie.Name;
                objSerie.type = serie.Type;
                objSerie.color = serie.Color;
                objSerie.data = dataSerie;
                objSerie.dataLabels = dataLabels;

                series.Add(objSerie);
            }

            //salida
            dynamic highchart = new ExpandoObject();
            highchart.chart = chart;
            highchart.title = title;
            highchart.xAxis = xAxis;
            highchart.yAxis = yAxis;
            highchart.plotOptions = plotOptions;
            highchart.series = series;
            highchart.legend = legend;

            return highchart;
        }

        #endregion

        #region 4. Anexos

        private static void TablaWordElaboracion(DocX document, ModelWordAnexoA model)
        {
            List<WordCelda> listaColumna = new List<WordCelda>
                    {
                        new WordCelda("ELABORADO POR:", 300, 10,10, Alignment.center),
                        new WordCelda("APROBADO POR:", 300, 10,10, Alignment.center),
                    };

            int nroRowData = 2;
            int nroColumn = listaColumna.Count;

            Table tabla = document.InsertTable(nroRowData + 1, nroColumn);
            tabla.AutoFit = AutoFit.ColumnWidth;
            tabla.Design = TableDesign.TableGrid;
            tabla.Alignment = Alignment.center;

            //formatear cabecera
            tabla.Rows[1].Cells[0].Paragraphs[0].Append(model.Elaboradopor);
            tabla.Rows[1].Cells[1].Paragraphs[0].Append(model.Aprobadopor);
            tabla.Rows[2].Cells[0].Paragraphs[0].Append(model.Cargoelaboradopor).FontSize(8);
            tabla.Rows[2].Cells[1].Paragraphs[0].Append(model.CargoAprobadopor).FontSize(8);

            //formatear cuerpo
            UtilWord.BodyTablaWord(ref tabla, 0, 0, nroRowData, nroColumn - 1, listaColumna, FontDoc);
            //formatear cabecera
            UtilWord.FormatearFilaTablaWord(ref tabla, 0, listaColumna, FontDoc); //tabla maximo 640px
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Clase que almacena todos los datos para el inform word Anexo A
    /// </summary>
    public class ModelWordAnexoA
    {
        public DateTime Fecha { get; set; }
        public string RutaNombreArchivo { get; set; }
        public string NumeroIeod { get; set; }
        public bool IncluyeEcuador { get; set; }

        public List<MeMedicion48DTO> DemandaNorte { get; set; }
        public List<MeMedicion48DTO> DemandaCentro { get; set; }
        public List<MeMedicion48DTO> DemandaSur { get; set; }

        //variable 1.1.1. EVOLUCIÓN HORARIA DE LA DEMANDA TOTAL
        public List<EveMailsDTO> ListaReprogramas { get; set; }
        public DateTime FechaProgramaEmitido { get; set; }
        public Decimal FactorCargaHoy { get; set; }
        public Decimal PorcVariacionFactorCarga { get; set; }
        public GraficoWeb GraficoDemandaEjecyProg { get; set; }
        public string HrPeriodo01 { get; set; }
        public string HrPeriodo02 { get; set; }
        public decimal VelCrecimiento { get; set; }
        public decimal PorcVCrecimientoVsAyer { get; set; }
        public decimal PorcVCrecimientoVsHace7d { get; set; }

        public decimal LsPendienteMaxSEINhoy { get; set; }
        public string LsHoraPendMaxSEIN { get; set; }
        public decimal LsPendienteMaxNorte { get; set; }
        public string LsHoraPendMaxNorte { get; set; }
        public decimal LsPendienteMaxSur { get; set; }
        public string LsHoraPendMaxSur { get; set; }
        public decimal LsPendienteMaxCentro { get; set; }
        public string LsHoraPendMaxCentro { get; set; }

        public decimal MinDemanda { get; set; }
        public string HoraMinDemanda { get; set; }
        public decimal MaxDemanda { get; set; }
        public string HoraMaxDemanda { get; set; }

        //1.1.2. EVOLUCIÓN HORARIA DE LA DEMANDA POR ÁREA
        public GraficoWeb GraficoDemandaxAreas { get; set; }
        public string HoraMaxDemSein { get; set; }
        public string HoraMaxDemSeinNorte { get; set; }
        public string HoraMaxDemSeinCentro { get; set; }
        public string HoraMaxDemSeinSur { get; set; }
        public decimal PorcDemSeinCentro { get; set; }
        public decimal PorcDemSeinNorte { get; set; }
        public decimal PorcDemSeinSur { get; set; }
        public decimal FCargaNorte { get; set; }
        public decimal FCargaCentro { get; set; }
        public decimal FCargaSur { get; set; }
        public decimal VelCrecNorte { get; set; }
        public decimal VelCrecCentro { get; set; }
        public decimal VelCrecSur { get; set; }

        //1.1.3.EVOLUCIÓN HORARIA DE LAS CARGAS MAS IMPORTANTES
        public GraficoWeb GraficoMultilineas { get; set; }
        public GraficoWeb GraficoMultilineas02 { get; set; }
        public GraficoWeb GraficoMultilineas03 { get; set; }
        public GraficoWeb GraficoMultilineas04 { get; set; }

        //1.1.4. RECURSOS ENERGÉTICOS Y DIAGRAMA DE DURACIÓN DE CARGA
        public GraficoWeb GraficoRecEnergeticos { get; set; }
        public decimal EnegíaTotProduccion { get; set; }
        public decimal EnerCtralPasada { get; set; }
        public decimal EnerCtralRegulacion { get; set; }
        public decimal EnerTerDiesel { get; set; }
        public decimal PorcEnerTerDiesel { get; set; }
        public decimal EnergiaResidual { get; set; }
        public decimal PorcEnergiaResidual { get; set; }
        public decimal EnerGas { get; set; }
        public decimal PorcEnerGas { get; set; }
        public decimal EnerCarbon { get; set; }
        public decimal PorcEnerCarbon { get; set; }
        public decimal EnerBagBio { get; set; }
        public decimal PorcEnerBagBio { get; set; }
        public decimal EnerEolica { get; set; }
        public decimal PorcEnerEolica { get; set; }
        public decimal EnerSolar { get; set; }
        public decimal PorcEnerSolar { get; set; }
        public decimal HorasDemanda { get; set; }
        public decimal PorcMayorMaxDemanda { get; set; }
        public decimal PorcTiempoTotal { get; set; }
        public decimal HorasDemanda2 { get; set; }
        public decimal PorcRangoMaxDemanda1 { get; set; }
        public decimal PorcRangoMaxDemanda2 { get; set; }
        public decimal PorcTiempoTotal2 { get; set; }

        //1.2. EVOLUCIÓN DIARIA DE LA PRODUCCIÓN DE ENERGÍA
        public GraficoWeb GraficoEvolDiario { get; set; }
        public decimal EnerEjecutada { get; set; }
        public decimal DifEnerEjecProg { get; set; }
        public decimal PorcenerEjecutada { get; set; }
        public decimal DifEnerEjec7D { get; set; }
        public decimal PorcEnerEjec7D { get; set; }
        public string TextoMaxEnergiaAnio { get; set; }
        public decimal EnerEjecutadaAnio { get; set; }

        //1.3. MÁXIMA GENERACIÓN INSTANTÁNEA
        public decimal MaxGenInst { get; set; }
        public decimal DismMaxDemanda { get; set; }
        public decimal PorcDismMaxDemanda { get; set; }
        public List<InfSGIDatosTablas> ListMaxGenInstant { get; set; }
        public GraficoWeb GraficoMaximaDemandaDiario { get; set; }
        public string TextoMDAnio { get; set; }
        public decimal MDEjecutadaAnio { get; set; }

        //1.4. PRINCIPALES EVENTOS(FALLAS, INTERRUPCIONES Y RACIONAMIENTO)
        public List<EventoDTO> ListaEvento { get; set; }
        public List<EveInterrupcionDTO> ListaInterrup { get; set; }

        //1.5. MANTENIMIENTOS PROGRAMADOS Y EJECUTADOS
        public List<InfSGIDatosTablas> ListMantProgEjecutados { get; set; }

        //1.6.1. OPERACIÓN DE CALDEROS
        public List<InfSGIDatosTablas> ListobjOpCaldero { get; set; }

        //1.6.2. OPERACIÓN A CARGA MÍNIMA
        public List<InfSGIDatosTablas> ListobjOpCargaMin { get; set; }

        //1.7 RESERVA NO SINCRONIZADA DEL COES
        public GraficoWeb GraficoEficGNyCarb { get; set; }
        public GraficoWeb GraficoReservaSinc { get; set; }

        //1.8. REGULACION DE TENSIÓN
        public List<InfSGIDatosTablas> ListobjOpRegTension { get; set; }

        //1.9.A. ALEATORIAS DE DISPONIBILIDAD
        public List<InfSGIDatosTablas> ListobjAleatoriasDisp { get; set; }

        //1.9.B. POR REQUERIMIENTOS PROPIOS
        public List<InfSGIDatosTablas> ListaReqPropio { get; set; }
        public List<InfSGIDatosTablas> ListaReqPropioNoTermo { get; set; }

        //1.9.C. A SOLICITUD DE TERCEROS
        public List<InfSGIDatosTablas> ListaReqSoliTercero { get; set; }

        //1.10. SISTEMAS AISLADOS
        public List<InfSGIDatosTablas> ListaSisAislado { get; set; }

        //1.11. CONGESTIÓN
        public List<InfSGIDatosTablas> ListaCongestion { get; set; }

        // 1.12. CALIDAD DE PRODUCTO (FRECUENCIA)
        public GraficoWeb GraficoFrecSein { get; set; }
        public List<InfSGIDatosTablas> ListaPeriodoRevision { get; set; }
        public List<InfSGIDatosTablas> ListaIndicadorCalidad { get; set; }
        public List<InfSGIDatosTablas> ListaFrecuenciaMinMax { get; set; }
        public GraficoWeb GraficoCampanaFrec { get; set; }
        public List<FLecturaDTO> ListaFrecRango { get; set; }
        public List<FLecturaDTO> ListaFrecDebajo { get; set; }
        public List<FLecturaDTO> ListaFrecEncima { get; set; }
        public string GpsNombre { get; set; }

        //1.13 FLUJOS POR LAS INTERCONEXIONES (MW)
        public GraficoWeb GraficoInterc01 { get; set; }
        public GraficoWeb GraficoInterc02 { get; set; }
        public GraficoWeb GraficoInterc03 { get; set; }

        //1.14. Interconexiones Internacionales
        public List<InfSGIDatosTablas> ListaInterconexionesInternacional { get; set; }
        public GraficoWeb GraficoInterconexionesInternacional { get; set; }

        //2.1. COSTO TOTAL DE LA OPERACIÓN POR DÍA
        public string MensajeCosto { get; set; }
        public GraficoWeb GraficoCosto { get; set; }

        //Anexo 
        public string Elaboradopor { get; set; }
        public string Aprobadopor { get; set; }
        public string Cargoelaboradopor { get; set; }
        public string CargoAprobadopor { get; set; }
    }

    public class InfSGIDatosTablas
    {
        public string Empresa { get; set; }
        public string Ubicacion { get; set; }
        public string Equipo { get; set; }
        public string Inicio { get; set; }
        public string Final { get; set; }
        public string Prog { get; set; }
        public string Ejec { get; set; }

        public string UnidadLimitada { get; set; }
        public string OperacionCentrales { get; set; }
        public string Motivo { get; set; }
        public string SubsistemaAislado { get; set; }

        public string Observacion { get; set; }
        public string Tipo { get; set; }
        public string PaisImp { get; set; }
        public string PaisExp { get; set; }

        //maxima demanda
        public string Dia { get; set; }
        public string EjeMW { get; set; }
        public string EjeHora { get; set; }
        public string ProgMW { get; set; }
        public string ProgHora { get; set; }
        public string Desv { get; set; }

        //frecuencia
        public string Celda1 { get; set; }
        public string Celda2 { get; set; }
        public string Celda3 { get; set; }
        public string Celda4 { get; set; }
        public string Celda5 { get; set; }
        public string Celda6 { get; set; }
        public string Celda7 { get; set; }
        public string Celda8 { get; set; }
    }

}
