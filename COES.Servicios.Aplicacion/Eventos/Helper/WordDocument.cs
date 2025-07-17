using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.Eventos.Helper
{
    /// <summary>
    /// DTO para generar el reporte de decision ctaf 
    /// </summary>
    public class InfoDecisionCTAF
    {
        /// <summary>
        /// Propiedad Analisis de Falla
        /// </summary>
        public AnalisisFallaDTO AnalisisFalla { get; set; }
        /// <summary>
        /// Propiedad Evento CTAF
        /// </summary>
        public EventoDTO EventoCTAF { get; set; }
        /// <summary>
        /// Propiedad Eventos SCO
        /// </summary>
        public List<EventoDTO> EventosSCO { get; set; }
        /// <summary>
        /// Propiedad Equipo
        /// </summary>
        public EquipoDTO Equipo { get; set; }
        /// <summary>
        /// Propiedad Informes Finales
        /// </summary>
        public List<EveInformesScoDTO> InformesFinales { get; set; }
        /// <summary>
        /// Propiedad Tabla Reportes
        /// </summary>
        public TablaReporte TablaReporte { get; set; }
        /// <summary>
        /// Propiedad Reporte Interrupciones
        /// </summary>
        public TablaReporte TablaDecision { get; set; }
        /// <summary>
        /// Propiedad Reporte Interrupciones
        /// </summary>
        public TablaReporte TablaResarcimiento { get; set; }
        public AnalisisFallasAppServicio AnalisisFallasAppServicio { get; set; }
    }

    /// <summary>
    /// Constantes para el modulo de eventos
    /// </summary>
    public class WordDocument
    {
        /// <summary>
        /// Permite generar el reporte de perturbación en formato WORD
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="evento"></param>
        /// <param name="path"></param>
        public void GenerarReporteInforme(EveEventoDTO evento, List<EveInformeItemDTO> listItem, int idInforme, string path, string fileName,
            int indicadorLogo, string pathLogo, string empresa)
        {
            try
            {
                #region Generacion del Documento Word

                using (DocX document = DocX.Create(path + fileName))
                {
                    Novacode.Image logo = null;

                    if (indicadorLogo == 1)
                    {
                        logo = document.AddImage(path + "coes.png");
                    }
                    else if (indicadorLogo == 0)
                    {
                        logo = document.AddImage(pathLogo);
                    }

                    document.AddHeaders();
                    document.DifferentFirstPage = false;
                    document.DifferentOddAndEvenPages = false;
                    Header header = document.Headers.odd;

                    #region Cabecera

                    Table tbHeader = header.InsertTable(1, 3);
                    tbHeader.Design = TableDesign.None;
                    tbHeader.AutoFit = AutoFit.Window;

                    Paragraph upperRightParagraph = header.Tables[0].Rows[0].Cells[0].Paragraphs[0];

                    if (indicadorLogo == 0 || indicadorLogo == 1)
                    {
                        upperRightParagraph.AppendPicture(logo.CreatePicture());
                    }
                    else
                    {
                        upperRightParagraph.Append("NO EXISTE LOGO");
                    }

                    upperRightParagraph.Alignment = Alignment.center;
                    tbHeader.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    tbHeader.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                    tbHeader.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                    Paragraph cEmpresa = header.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                    cEmpresa.Append("INFORMA: \n" + empresa);
                    cEmpresa.Alignment = Alignment.center;
                    cEmpresa.FontSize(13);
                    cEmpresa.Bold();


                    Paragraph cabecera = header.Tables[0].Rows[0].Cells[2].Paragraphs[0];
                    cabecera.Append("INFORME TÉCNICO \n" + DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha));
                    cabecera.Alignment = Alignment.center;
                    cabecera.FontSize(13);
                    cabecera.Bold();
                    header.InsertParagraph("");


                    #endregion

                    string color = "#DDDDDD";

                    document.InsertParagraph("");
                    Paragraph parrafo = document.InsertParagraph("INFORME TÉCNICO");
                    parrafo.FontSize(11);
                    parrafo.Alignment = Alignment.center;
                    parrafo.Bold();
                    document.InsertParagraph("");


                    #region Seccion 1 al 3

                    Table tabla = document.InsertTable(3, 2);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.None;


                    parrafo = tabla.Rows[0].Cells[0].Paragraphs[0];
                    parrafo.Append("1. EVENTO");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    parrafo = tabla.Rows[0].Cells[1].Paragraphs[0];
                    parrafo.Append(evento.Tipoevenabrev);
                    parrafo.FontSize(10);

                    parrafo = tabla.Rows[1].Cells[0].Paragraphs[0];
                    parrafo.Append("2. FECHA");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    parrafo = tabla.Rows[1].Cells[1].Paragraphs[0];
                    parrafo.Append(((DateTime)evento.Evenini).ToString(ConstantesAppServicio.FormatoFecha));
                    parrafo.FontSize(10);

                    parrafo = tabla.Rows[2].Cells[0].Paragraphs[0];
                    parrafo.Append("3. HORA");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    parrafo = tabla.Rows[2].Cells[1].Paragraphs[0];
                    parrafo.Append(((DateTime)evento.Evenini).ToString(ConstantesAppServicio.FormatoOnlyHora));
                    parrafo.FontSize(10);

                    document.InsertParagraph("");
                    #endregion

                    #region Seccion 4

                    parrafo = document.InsertParagraph("4. DESCRIPCION DEL EVENTO");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    string texto = string.Empty;
                    EveInformeItemDTO item = listItem.Where(x => x.Itemnumber == 4).FirstOrDefault();

                    if (item != null)
                    {
                        texto = item.Descomentario;
                    }

                    parrafo = document.InsertParagraph(texto);
                    parrafo.FontSize(10);

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 5.1

                    parrafo = document.InsertParagraph("5. CONDICIONES OPERATIVAS PREVIAS");
                    parrafo.Bold();
                    parrafo.FontSize(10);
                    document.InsertParagraph("");
                    parrafo = document.InsertParagraph("a. GENERACIÖN");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listGeneracion = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 1).ToList();

                    tabla = document.InsertTable(listGeneracion.Count + 1, 6);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;

                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Subestación");
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Unidad");
                    tabla.Rows[0].Cells[3].Paragraphs[0].Append("Potencia Activa(MW)");
                    tabla.Rows[0].Cells[4].Paragraphs[0].Append("Potencia Reactiva(MVAR)");
                    tabla.Rows[0].Cells[5].Paragraphs[0].Append("Observaciones");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[5].Paragraphs[0].FontSize(10);

                    int indice = 1;
                    foreach (EveInformeItemDTO entity in listGeneracion)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice.ToString()));
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Equinomb);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append(entity.Potactiva.ToString());
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append(entity.Potreactiva.ToString());
                        tabla.Rows[indice].Cells[5].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[5].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 5.2

                    parrafo = document.InsertParagraph("b. FLUJO DE POTENCIAS EN LAS LÍNEAS");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listLineas = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 2).ToList();

                    tabla = document.InsertTable(listLineas.Count + 2, 7);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.MergeCellsInColumn(0, 0, 1);
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Código L.T.");
                    tabla.MergeCellsInColumn(1, 0, 1);
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Subestaciones");

                    tabla.Rows[0].Cells[4].Paragraphs[0].Append("Potencia Activa (MW)");
                    tabla.MergeCellsInColumn(4, 0, 1);
                    tabla.Rows[0].Cells[5].Paragraphs[0].Append("Potencia Reactiva (MVAR)");
                    tabla.MergeCellsInColumn(5, 0, 1);
                    tabla.Rows[0].Cells[6].Paragraphs[0].Append("Observaciones");
                    tabla.MergeCellsInColumn(6, 0, 1);

                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[6].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[5].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[6].Paragraphs[0].FontSize(10);

                    tabla.Rows[0].MergeCells(2, 3);

                    tabla.Rows[1].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[2].Paragraphs[0].Append("De");
                    tabla.Rows[1].Cells[3].Paragraphs[0].Append("A");
                    tabla.Rows[1].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[3].Paragraphs[0].Bold();

                    tabla.Rows[1].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[3].Paragraphs[0].FontSize(10);


                    indice = 2;
                    foreach (EveInformeItemDTO entity in listLineas)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice - 1).ToString());
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Equinomb + " " + entity.Equicodi);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Subestacionde);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append(entity.Subestacionhasta);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append(entity.Potactiva.ToString());
                        tabla.Rows[indice].Cells[5].Paragraphs[0].Append(entity.Potreactiva.ToString());
                        tabla.Rows[indice].Cells[6].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[5].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[6].Paragraphs[0].FontSize(10);

                        indice++;
                    }


                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 5.3

                    parrafo = document.InsertParagraph("c. FLUJO DE POTENCIAS EN LOS TRANSFORMADORES");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listTransformador = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 3).ToList();

                    tabla = document.InsertTable(listTransformador.Count + 1, 7);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Subestación");
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Código");
                    tabla.Rows[0].Cells[3].Paragraphs[0].Append("Potencia Activa(MW)");
                    tabla.Rows[0].Cells[4].Paragraphs[0].Append("Potencia Reactiva(MVAR)");
                    tabla.Rows[0].Cells[5].Paragraphs[0].Append("Nivel de Tensión");
                    tabla.Rows[0].Cells[6].Paragraphs[0].Append("Observaciones");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[6].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[5].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[6].Paragraphs[0].FontSize(10);

                    indice = 1;
                    foreach (EveInformeItemDTO entity in listTransformador)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice.ToString()));
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Areanomb);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Equinomb + " " + entity.Equicodi);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append(entity.Potactiva.ToString());
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append(entity.Potreactiva.ToString());
                        tabla.Rows[indice].Cells[5].Paragraphs[0].Append(entity.Niveltension.ToString());
                        tabla.Rows[indice].Cells[6].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[5].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[6].Paragraphs[0].FontSize(10);

                        indice++;
                    }


                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 6

                    parrafo = document.InsertParagraph("6. SECUENCIA CRONOLÓGICA");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listSecuencia = listItem.Where(x => x.Itemnumber == 6).ToList();

                    tabla = document.InsertTable(listSecuencia.Count + 1, 2);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("Hora");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Descripción del evento");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);


                    indice = 1;
                    foreach (EveInformeItemDTO entity in listSecuencia)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append(entity.Itemhora);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 7

                    parrafo = document.InsertParagraph("7. ACTUACIÓN DE LAS PROTECCIONES");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listActuacion = listItem.Where(x => x.Itemnumber == 7).ToList();

                    tabla = document.InsertTable(listActuacion.Count + 1, 5);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("Subestación");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Equipo");
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Señalización");
                    tabla.Rows[0].Cells[3].Paragraphs[0].Append("INT");
                    tabla.Rows[0].Cells[4].Paragraphs[0].Append("A/C");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);



                    indice = 1;
                    foreach (EveInformeItemDTO entity in listActuacion)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append(entity.Areanomb);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Equinomb);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Senializacion);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append(entity.Internomb);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append(entity.Ac);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 8

                    parrafo = document.InsertParagraph("8. CONTADOR DE INTERRUPTORES Y PARARRAYOS");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listContador = listItem.Where(x => x.Itemnumber == 8).ToList();

                    tabla = document.InsertTable(listContador.Count + 2, 9);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[7].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[8].FillColor = System.Drawing.ColorTranslator.FromHtml(color);

                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[7].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[8].Paragraphs[0].Alignment = Alignment.center;

                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("Subestación");
                    tabla.MergeCellsInColumn(0, 0, 1);
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Celda");
                    tabla.MergeCellsInColumn(1, 0, 1);
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Código Interruptor");
                    tabla.MergeCellsInColumn(2, 0, 1);
                    tabla.Rows[0].Cells[3].Paragraphs[0].Append("Antes");
                    tabla.Rows[0].Cells[6].Paragraphs[0].Append("Después");

                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[6].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[7].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[8].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[5].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[6].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[7].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[8].Paragraphs[0].FontSize(10);

                    tabla.Rows[0].MergeCells(3, 5);
                    tabla.Rows[0].MergeCells(4, 6);

                    tabla.Rows[1].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[7].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[8].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[6].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[7].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[8].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[3].Paragraphs[0].Append("R");
                    tabla.Rows[1].Cells[4].Paragraphs[0].Append("S");
                    tabla.Rows[1].Cells[5].Paragraphs[0].Append("T");
                    tabla.Rows[1].Cells[6].Paragraphs[0].Append("R");
                    tabla.Rows[1].Cells[7].Paragraphs[0].Append("S");
                    tabla.Rows[1].Cells[8].Paragraphs[0].Append("T");
                    tabla.Rows[1].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[6].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[7].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[8].Paragraphs[0].Bold();

                    tabla.Rows[1].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[5].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[6].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[7].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[8].Paragraphs[0].FontSize(10);


                    indice = 1;
                    foreach (EveInformeItemDTO entity in listContador)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append(entity.Areanomb);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Equinomb);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Internomb);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append(entity.Ra.ToString());
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append(entity.Sa.ToString());
                        tabla.Rows[indice].Cells[5].Paragraphs[0].Append(entity.Ta.ToString());
                        tabla.Rows[indice].Cells[6].Paragraphs[0].Append(entity.Rd.ToString());
                        tabla.Rows[indice].Cells[7].Paragraphs[0].Append(entity.Sd.ToString());
                        tabla.Rows[indice].Cells[8].Paragraphs[0].Append(entity.Td.ToString());

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[5].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[6].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[7].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[8].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 9

                    parrafo = document.InsertParagraph("9. ANÁLISIS DEL EVENTO");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    texto = string.Empty;
                    EveInformeItemDTO itemAnalisis = listItem.Where(x => x.Itemnumber == 9).FirstOrDefault();

                    if (itemAnalisis != null)
                    {
                        texto = itemAnalisis.Descomentario;
                    }

                    parrafo = document.InsertParagraph(texto);

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 10

                    parrafo = document.InsertParagraph("10. SUMINISTROS INTERRUMPIDOS");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listSuministro = listItem.Where(x => x.Itemnumber == 10).ToList();

                    tabla = document.InsertTable(listSuministro.Count + 2, 7);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[6].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[6].Paragraphs[0].Alignment = Alignment.center;


                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.MergeCellsInColumn(0, 0, 1);
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Suministro");
                    tabla.MergeCellsInColumn(1, 0, 1);
                    tabla.Rows[0].Cells[2].Paragraphs[0].Append("Potencia MW");
                    tabla.MergeCellsInColumn(2, 0, 1);
                    tabla.Rows[0].Cells[4].Paragraphs[0].Append("Tiempo de desconexión (min)");
                    tabla.Rows[0].Cells[6].Paragraphs[0].Append("Protección");
                    tabla.MergeCellsInColumn(6, 0, 1);

                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[2].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[5].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[6].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[2].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[5].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[6].Paragraphs[0].FontSize(10);

                    tabla.Rows[0].MergeCells(3, 5);

                    tabla.Rows[1].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[5].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[1].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[5].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[1].Cells[3].Paragraphs[0].Append("Inicio");
                    tabla.Rows[1].Cells[4].Paragraphs[0].Append("Final");
                    tabla.Rows[1].Cells[5].Paragraphs[0].Append("Duración");
                    tabla.Rows[1].Cells[3].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[4].Paragraphs[0].Bold();
                    tabla.Rows[1].Cells[5].Paragraphs[0].Bold();

                    tabla.Rows[1].Cells[3].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[4].Paragraphs[0].FontSize(10);
                    tabla.Rows[1].Cells[5].Paragraphs[0].FontSize(10);


                    indice = 2;
                    foreach (EveInformeItemDTO entity in listSuministro)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice - 1).ToString());
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Sumininistro);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].Append(entity.Potenciamw.ToString());
                        tabla.Rows[indice].Cells[3].Paragraphs[0].Append((((DateTime)entity.Intinicio).ToString("dd/MM/yyyy HH:mm:ss")));
                        tabla.Rows[indice].Cells[4].Paragraphs[0].Append((((DateTime)entity.Intfin).ToString("dd/MM/yyyy HH:mm:ss")));

                        entity.Duracion = 0;

                        if (entity.Intinicio != null && entity.Intfin != null)
                        {
                            TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);

                            entity.Duracion = (int)duracion.TotalMinutes;
                            if (entity.Duracion < 0) entity.Duracion = 0;
                        }

                        tabla.Rows[indice].Cells[5].Paragraphs[0].Append(entity.Duracion.ToString());
                        tabla.Rows[indice].Cells[6].Paragraphs[0].Append(entity.Proteccion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[2].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[3].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[4].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[5].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[6].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 11

                    parrafo = document.InsertParagraph("11. CONCLUSIONES");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listConclusion = listItem.Where(x => x.Itemnumber == 11).ToList();

                    tabla = document.InsertTable(listConclusion.Count + 1, 2);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Conclusión");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);


                    indice = 1;
                    foreach (EveInformeItemDTO entity in listConclusion)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice.ToString()));
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);

                        indice++;
                    }


                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 12

                    parrafo = document.InsertParagraph("12. ACCIONES EJECUTADAS");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listAcciones = listItem.Where(x => x.Itemnumber == 12).ToList();

                    tabla = document.InsertTable(listAcciones.Count + 1, 2);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Acción ejecutada");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);

                    indice = 1;
                    foreach (EveInformeItemDTO entity in listAcciones)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice.ToString()));
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.InsertParagraph("");

                    #region Seccion 13

                    parrafo = document.InsertParagraph("13. OBSERVACIONES Y RECOMENDACIONES");
                    parrafo.Bold();
                    parrafo.FontSize(10);

                    List<EveInformeItemDTO> listObservacion = listItem.Where(x => x.Itemnumber == 13).ToList();

                    tabla = document.InsertTable(listObservacion.Count + 1, 2);
                    tabla.AutoFit = AutoFit.Window;
                    tabla.Design = TableDesign.TableGrid;

                    tabla.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    tabla.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    tabla.Rows[0].Cells[0].Paragraphs[0].Append("N°");
                    tabla.Rows[0].Cells[1].Paragraphs[0].Append("Observación / Recomendación");
                    tabla.Rows[0].Cells[0].Paragraphs[0].Bold();
                    tabla.Rows[0].Cells[1].Paragraphs[0].Bold();

                    tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                    tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(10);

                    indice = 1;
                    foreach (EveInformeItemDTO entity in listObservacion)
                    {
                        tabla.Rows[indice].Cells[0].Paragraphs[0].Append((indice.ToString()));
                        tabla.Rows[indice].Cells[1].Paragraphs[0].Append(entity.Desobservacion);

                        tabla.Rows[indice].Cells[0].Paragraphs[0].FontSize(10);
                        tabla.Rows[indice].Cells[1].Paragraphs[0].FontSize(10);

                        indice++;
                    }

                    #endregion

                    document.Save();
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite generar el reporte de decision ctaf en formato word
        /// retorna un valor tipo byte[]
        /// </summary>
        public GenerarDecisionCtafFDTO DecisionCTAF(InfoDecisionCTAF infoDecision, string logoPath, string fileName, string imagenLinea)
        {
            var response = new byte[0];

            string reporteName = $"D_{infoDecision.AnalisisFalla.CodigoEvento}";
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion;

            using (var documentoWord = DocX.Create(reporteName))
            {
                documentoWord.DifferentOddAndEvenPages = true;
                documentoWord.DifferentFirstPage = true;
                documentoWord.PageWidth = 780;

                #region HEADER
                documentoWord.AddHeaders();
                Header headerFirst = documentoWord.Headers.first;
                Novacode.Image logo = documentoWord.AddImage(logoPath);
                var tempLogo = logo.CreatePicture();
                tempLogo.Width = 155;
                tempLogo.Height = 70;
                headerFirst.InsertParagraph().AppendPicture(tempLogo);
                headerFirst.InsertParagraph();
                #endregion

                #region FOOTER
                documentoWord.AddFooters();
                Footer footer = documentoWord.Footers.first;
                Novacode.Image logoLinea = documentoWord.AddImage(imagenLinea);
                var tempLogoLinea = logoLinea.CreatePicture();
                tempLogoLinea.Width = 591;
                footer.InsertParagraph().AppendPicture(tempLogoLinea);

                Table footer_table = footer.InsertTable(1, 2);
                footer_table.Design = TableDesign.TableNormal;
                footer_table.AutoFit = AutoFit.Window;

                footer_table.Rows[0].Cells[0].Paragraphs.First().Append(infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.ToString("dd/MM/yyyy"));
                footer_table.Rows[0].Cells[1].Paragraphs.First().Append("Pág. ").Alignment = Alignment.right;
                footer_table.Rows[0].Cells[1].Paragraphs.First().AppendPageNumber(PageNumberFormat.normal);
                footer_table.Rows[0].Cells[1].Paragraphs.First().Append(" de ").Alignment = Alignment.right;
                footer_table.Rows[0].Cells[1].Paragraphs.First().AppendPageCount(PageNumberFormat.normal);
                #endregion

                #region TITULO
                var parrafoTitulo = documentoWord.InsertParagraph();
                AddFormattedText(parrafoTitulo, $"DECISIÓN DE LA DIRECCIÓN EJECUTIVA DEL COES\n\nRESPECTO DE LAS TRANSGRESIONES A LA NTCSE\n\nPOR EL EVENTO {infoDecision.AnalisisFalla.CodigoEvento}\n\n", true, "Calibri", 11, "center", false);
                #endregion

                #region SUMILLA
                Table tableSumilla = documentoWord.AddTable(1, 2);
                tableSumilla.Design = TableDesign.TableNormal;
                tableSumilla.AutoFit = AutoFit.Contents;

                AddFormattedText(tableSumilla.Rows[0].Cells[0].Paragraphs.First(), "Sumilla:", false, "Calibri", 11, string.Empty, true);
                tableSumilla.Rows[0].Cells[0].Width = 80;

                string fechasEventosSCO = string.Empty;

                var tempEventosOrdenados = infoDecision.EventosSCO.OrderBy(x => x.EVENINI).ToList();

                for (int i = 0; i < tempEventosOrdenados.Count; i++)
                {
                    var tempEventoSCO = tempEventosOrdenados[i];

                    if (tempEventoSCO.EVENINI.HasValue)
                    {
                        var agregarAdicion = i == tempEventosOrdenados.Count - 1 ? " y " : ", ";
                        agregarAdicion = i == 0 ? "" : agregarAdicion;
                        var hora = tempEventoSCO.EVENINI.Value.ToString("HH:mm:ss ");
                        var tempDiaEvento = tempEventoSCO.EVENINI.Value.ToString($" dd.MM.yyyy");
                        fechasEventosSCO = string.Concat(fechasEventosSCO, agregarAdicion, hora, "h del", tempDiaEvento);
                    }
                }

                AddFormattedText(tableSumilla.Rows[0].Cells[1].Paragraphs.First(), $"Asignación de responsabilidad por interrupción de suministros en el SEIN, ocurrida a las {fechasEventosSCO}.", false, "Calibri", 11, "both", false);
                tableSumilla.Rows[0].Cells[1].Width = 520;
                tableSumilla.Rows[0].Cells[1].InsertParagraph();

                documentoWord.InsertTable(tableSumilla);
                #endregion SUMILLA

                #region FECHA EMISION DESICION
                var tempDiaFechaEmisionDecision = infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.ToString("dd");
                var mes = COES.Base.Tools.Util.ObtenerNombreMes(infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.Month).ToLower();
                var parrafoFechaEmisionDecisionText = $"Lima, {tempDiaFechaEmisionDecision} de {mes} de {infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.ToString("yyyy")}";
                var parrafoFechaEmisionDecision = documentoWord.InsertParagraph();
                AddFormattedText(parrafoFechaEmisionDecision, parrafoFechaEmisionDecisionText, false, "Calibri", 11, "both", false);
                #endregion

                documentoWord.InsertParagraph();

                var parrafoDireccionEjecutivaDelCOES = documentoWord.InsertParagraph();
                AddFormattedText(parrafoDireccionEjecutivaDelCOES, "LA DIRECCIÓN EJECUTIVA DEL COES:", true, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                int anchoTableContenido = 550;
                int anchoTablaSeccion = 48;
                int anchoTablaSeccionDos = 40;
                int anchoTablaSeccionTres = 50;
                int anchoTablaSeccionTresContenido = 500;

                #region ANTECEDENTES
                Table tableAntecedente = documentoWord.InsertTable(7, 2);
                tableAntecedente.Design = TableDesign.TableNormal;
                tableAntecedente.AutoFit = AutoFit.ColumnWidth;

                var antecedentesFechasEventosSCO = string.Empty;
                var antecedentesHorasEventosSCO = string.Empty;

                var tempEventosOrdenadosAntecedentes = infoDecision.EventosSCO.OrderBy(x => x.EVENINI).ToList();

                for (int i = 0; i < tempEventosOrdenadosAntecedentes.Count; i++)
                {
                    var tempEventoSCO = tempEventosOrdenadosAntecedentes[i];

                    if (tempEventoSCO.EVENINI.HasValue)
                    {
                        var agregarAdicion = i == tempEventosOrdenadosAntecedentes.Count - 1 ? " y " : ", ";
                        agregarAdicion = i == 0 ? "" : agregarAdicion;
                        var tempDiaEvento = tempEventoSCO.EVENINI.Value.ToString($"dd.MM.yyyy");
                        var tempHoraEvento = tempEventoSCO.EVENINI.Value.ToString("HH:mm:ss");
                        antecedentesFechasEventosSCO = string.Concat(antecedentesFechasEventosSCO, agregarAdicion, tempDiaEvento);
                        antecedentesHorasEventosSCO = string.Concat(antecedentesHorasEventosSCO, agregarAdicion, tempHoraEvento, " h");
                    }
                }

                var antecedentesDateTimeEventosSCO = $"{antecedentesFechasEventosSCO}, a las {antecedentesHorasEventosSCO}";

                var antecedentesEmpresasInformeFinal = string.Empty;
                var tempListaEmpresas = infoDecision.InformesFinales.Select(x => x.Emprnomb).Distinct().ToList();

                for (int i = 0; i < tempListaEmpresas.Count; i++)
                {
                    var tempNombreEmpresa = tempListaEmpresas[i];
                    var tempAgregarAdicionInformeFinal = i == tempListaEmpresas.Count - 1 ? " y " : ", ";
                    tempAgregarAdicionInformeFinal = i == 0 ? "" : tempAgregarAdicionInformeFinal;
                    antecedentesEmpresasInformeFinal = string.Concat(antecedentesEmpresasInformeFinal, tempAgregarAdicionInformeFinal, tempNombreEmpresa);
                }

                var tempDiaFechaEmisionInformeCTAF = infoDecision.AnalisisFalla.AFEREUFECHAPROG.Value.ToString("dd");
                var tempMesFechaEmisionInformeCTAF = COES.Base.Tools.Util.ObtenerNombreMes(infoDecision.AnalisisFalla.AFEREUFECHAPROG.Value.Month).ToLower();
                var fechaEmisionInformeCTAF = $"{tempDiaFechaEmisionInformeCTAF} de {tempMesFechaEmisionInformeCTAF} de {infoDecision.AnalisisFalla.AFEREUFECHAPROG.Value.ToString("yyyy")}";

                var codigoInformeTecnico = infoDecision.AnalisisFalla.CodigoEvento.Replace("EV", "IT");
                var tempFechaInformTecnicoAntecedente = infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.ToString("dd.MM.yyyy");

                string tempAntecedentesFechasEventosSCO = string.Empty;

                for (int i = 0; i < tempEventosOrdenadosAntecedentes.Count; i++)
                {
                    var tempEventoSCO = tempEventosOrdenadosAntecedentes[i];

                    if (tempEventoSCO.EVENINI.HasValue)
                    {
                        var agregarAdicion = i == tempEventosOrdenadosAntecedentes.Count - 1 ? " y " : ", ";
                        agregarAdicion = i == 0 ? "" : agregarAdicion;
                        var tempAntecedenteHora = tempEventoSCO.EVENINI.Value.ToString(" HH:mm:ss");
                        var tempAntecedenteDiaEvento = tempEventoSCO.EVENINI.Value.ToString($"dd.MM.yyyy ");
                        tempAntecedentesFechasEventosSCO = string.Concat(tempAntecedentesFechasEventosSCO, agregarAdicion, tempAntecedenteDiaEvento, "a las", tempAntecedenteHora, " h");
                    }
                }

                AddFormattedText(tableAntecedente.Rows[0].Cells[0].Paragraphs.First(), "I.", true, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[0].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[0].Cells[1].Paragraphs.First(), "ANTECEDENTES", true, "Calibri", 11, "both", false);
                tableAntecedente.Rows[0].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[0].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[1].Cells[0].Paragraphs.First(), "1.1", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[1].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[1].Cells[1].Paragraphs.First(), $"Con fechas {antecedentesDateTimeEventosSCO} respectivamente, se produjeron las interrupciones de suministros por un total de XX,XX MW y XX,XX MW, debido a la {infoDecision.EventoCTAF.EVENASUNTO}, equipo de titularidad de la empresa {infoDecision.Equipo.EMPRENOMB}, en adelante “Evento”.", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[1].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[1].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[2].Cells[0].Paragraphs.First(), "1.2", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[2].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[2].Cells[1].Paragraphs.First(), "Con relación a los hechos antes descritos, el COES procedió a efectuar el análisis de asignación de responsabilidad, de acuerdo con el Procedimiento Técnico N° 40 “Procedimiento para la Aplicación del Numeral 3.5 de la NTCSE” (en adelante, “PR-40”).", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[2].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[2].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[3].Cells[0].Paragraphs.First(), "1.3", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[3].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[3].Cells[1].Paragraphs.First(), $"Conforme al numeral 8.2.7 de la Norma Técnica para la Coordinación de la Operación en Tiempo Real de los Sistemas Interconectados, aprobado por Resolución Directoral N° 014-2005-EM-DGE (en adelante, “NTCOTRSI”), las Empresas Involucradas {antecedentesEmpresasInformeFinal} remitieron al COES sus Informes Finales de Perturbaciones (“IFP/A”) así como sus cuadros de interrupción de suministros. Asimismo, sobre la base de tales Informes, el Coordinador del COES emitió el Informe Final de Perturbaciones del COES (“IFP/C”).", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[3].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[3].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[4].Cells[0].Paragraphs.First(), "1.4", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[4].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[4].Cells[1].Paragraphs.First(), $"En fecha {fechaEmisionInformeCTAF}, dentro del proceso de análisis de los eventos que ocasionan transgresiones a la Norma Técnica de Calidad de los Servicios Eléctricos, aprobada por Decreto Supremo N° 020-97-EM (en adelante, “NTCSE”), el Comité Técnico de Análisis de Fallas (en adelante, “CT-AF”) se reunió en forma no presencial  a fin de analizar los hechos ocurridos en el referido evento, teniendo como producto el Informe Técnico del CT-AF, elaborado dentro del plazo de 20 días hábiles otorgado por el Numeral 9.2.c del PR-40.", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[4].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[4].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[5].Cells[0].Paragraphs.First(), "1.5", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[5].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[5].Cells[1].Paragraphs.First(), $"La Subdirección de Evaluación de la Dirección de Operaciones del COES (“SEV”), con toda la información recibida, realizó un análisis detallado del origen de la falla y las implicancias derivadas de ella, así como los hechos vinculados a los eventos posteriores. Como parte del proceso, la SEV elaboró el Informe Técnico COES/D/DO/SEV/{codigoInformeTecnico} (“Informe Técnico”), de fecha {tempFechaInformTecnicoAntecedente}, con el análisis detallado sobre el Evento ocurrido el {tempAntecedentesFechasEventosSCO}, de conformidad a lo establecido en el Numeral 9.3.a del PR-40.", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[5].Cells[1].Width = anchoTableContenido;
                tableAntecedente.Rows[5].Cells[1].InsertParagraph();

                AddFormattedText(tableAntecedente.Rows[6].Cells[0].Paragraphs.First(), "1.6", false, "Calibri", 11, string.Empty, false);
                tableAntecedente.Rows[6].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAntecedente.Rows[6].Cells[1].Paragraphs.First(), "El Informe Técnico, que forma parte de la presente Decisión, desarrolla a detalle el análisis técnico de los hechos ocurridos en el Evento, sobre la base de las evidencias y pruebas recopiladas, así como la mejor información disponible, por lo que constituye el documento base para la identificación de las Empresas Involucradas en las transgresiones a los indicadores de calidad de la NTCSE.", false, "Calibri", 11, "both", false);
                tableAntecedente.Rows[6].Cells[1].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region HEADER PAGE 2
                Header headerOdd = documentoWord.Headers.odd;
                Table tableHeader = headerOdd.InsertTable(1, 2);
                tableHeader.Design = TableDesign.TableNormal;
                tableHeader.AutoFit = AutoFit.Window;

                tableHeader.Rows[0].Cells[0].Paragraphs.First().AppendPicture(tempLogo);
                AddFormattedText(tableHeader.Rows[0].Cells[1].Paragraphs.First(), $"Decisión de la Dirección Ejecutiva del COES\nrespecto de Transgresiones a la NTCSE\npor el {infoDecision.AnalisisFalla.CodigoEvento}", false, "Calibri", 9, "right", false);

                headerOdd.InsertParagraph();
                #endregion

                #region FOOTER PAGE 2
                var footerOdd = documentoWord.Footers.odd;

                footerOdd.InsertParagraph().AppendPicture(tempLogoLinea);

                Table tableFooter = footerOdd.InsertTable(1, 2);
                tableFooter.Design = TableDesign.TableNormal;
                tableFooter.AutoFit = AutoFit.Window;

                tableFooter.Rows[0].Cells[0].Paragraphs.First().Append(infoDecision.AnalisisFalla.AFEITFECHAELAB.Value.ToString("dd/MM/yyyy"));
                tableFooter.Rows[0].Cells[1].Paragraphs.First().Append("Pág. ").Alignment = Alignment.right;
                tableFooter.Rows[0].Cells[1].Paragraphs.First().AppendPageNumber(PageNumberFormat.normal);
                tableFooter.Rows[0].Cells[1].Paragraphs.First().Append(" de ").Alignment = Alignment.right;
                tableFooter.Rows[0].Cells[1].Paragraphs.First().AppendPageCount(PageNumberFormat.normal);

                footerOdd.InsertParagraph();
                #endregion

                #region ANALISIS

                #region ANALISIS II
                Table tableAnalisisPrincipal = documentoWord.InsertTable(1, 2);
                tableAnalisisPrincipal.Design = TableDesign.TableNormal;
                tableAnalisisPrincipal.AutoFit = AutoFit.ColumnWidth;

                AddFormattedText(tableAnalisisPrincipal.Rows[0].Cells[0].Paragraphs.First(), "II.", true, "Calibri", 11, "both", false);
                tableAnalisisPrincipal.Rows[0].Cells[0].Width = anchoTablaSeccion;
                AddFormattedText(tableAnalisisPrincipal.Rows[0].Cells[1].Paragraphs.First(), "ANÁLISIS", true, "Calibri", 11, "both", false);
                tableAnalisisPrincipal.Rows[0].Cells[1].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region TITULO SECCION 2.1
                Table tableAnalisisSeccion1 = documentoWord.InsertTable(1, 3);
                tableAnalisisSeccion1.Design = TableDesign.TableNormal;
                tableAnalisisSeccion1.AutoFit = AutoFit.ColumnWidth;

                tableAnalisisSeccion1.Rows[0].Cells[0].Width = anchoTablaSeccion;
                AddFormattedText(tableAnalisisSeccion1.Rows[0].Cells[1].Paragraphs.First(), "2.1", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion1.Rows[0].Cells[1].Width = anchoTablaSeccionDos;
                AddFormattedText(tableAnalisisSeccion1.Rows[0].Cells[2].Paragraphs.First(), "Competencia para emitir pronunciamiento", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion1.Rows[0].Cells[2].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION 2.1 CONTENIDO PRIMERA PARTE
                Table tableAnalisis21PrimerContenido = documentoWord.InsertTable(3, 2);
                tableAnalisis21PrimerContenido.Design = TableDesign.TableNormal;
                tableAnalisis21PrimerContenido.AutoFit = AutoFit.ColumnWidth;

                for (int i = 0; i < 3; i++)
                {
                    tableAnalisis21PrimerContenido.Rows[i].Cells[0].Width = anchoTablaSeccion;
                    tableAnalisis21PrimerContenido.Rows[i].Cells[1].Width = anchoTableContenido;
                }

                AddFormattedText(tableAnalisis21PrimerContenido.Rows[0].Cells[0].Paragraphs.First(), string.Empty, true, "Calibri", 11, string.Empty, false);
                AddFormattedText(tableAnalisis21PrimerContenido.Rows[0].Cells[1].Paragraphs.First(), "De acuerdo con el literal i) del Artículo 14º de la Ley para Asegurar el Desarrollo Eficiente de la Generación Eléctrica, aprobada por Ley N° 28832 y el numeral 3.5 de la NTCSE, en casos de transgresiones a la calidad del producto y/o suministro, el COES está obligado a asignar las responsabilidades que correspondan, así como a calcular las compensaciones derivadas de las mismas.", false, "Calibri", 11, "both", false);
                tableAnalisis21PrimerContenido.Rows[0].Cells[1].InsertParagraph();
                
                AddFormattedText(tableAnalisis21PrimerContenido.Rows[1].Cells[0].Paragraphs.First(), string.Empty, true, "Calibri", 11, string.Empty, false);
                AddFormattedText(tableAnalisis21PrimerContenido.Rows[1].Cells[1].Paragraphs.First(), "Asimismo, los numerales 5.1.6 y 11.2 del PR - 40 indican que el COES emitirá la Decisión de Asignación de Responsabilidad por Transgresión a la NTCSE y el cálculo preliminar de los Resarcimientos correspondientes, conforme a lo establecido en el inciso b) del numeral 3.5 de la NTCSE como se detalla a continuación:", false, "Calibri", 11, "both", false);
                tableAnalisis21PrimerContenido.Rows[1].Cells[1].InsertParagraph();

                AddFormattedText(tableAnalisis21PrimerContenido.Rows[2].Cells[0].Paragraphs.First(), string.Empty, true, "Calibri", 11, string.Empty, false);
                AddFormattedText(tableAnalisis21PrimerContenido.Rows[2].Cells[1].Paragraphs.First(), "PR-40", true, "Calibri", 11, "both", true);

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION PR40
                int anchoTablaPR40Seccion = 60;
                int anchoTablaPR40Contenido = 490;

                var tablaPR40 = documentoWord.InsertTable(2, 3);
                tablaPR40.Design = TableDesign.TableNormal;
                tablaPR40.AutoFit = AutoFit.ColumnWidth;

                tablaPR40.Rows[0].Cells[0].Width = anchoTablaSeccion;

                AddFormattedTextCursiva(tablaPR40.Rows[0].Cells[1].Paragraphs.First(), "“5.1.6", false, "Calibri", 11, "both", false);
                tablaPR40.Rows[0].Cells[1].Width = anchoTablaPR40Seccion;

                AddFormattedTextCursiva(tablaPR40.Rows[0].Cells[2].Paragraphs.First(), "Efectuar los cálculos de Resarcimientos correspondientes con la información que los Agentes alcancen y la mejor información disponible.”", false, "Calibri", 11, "both", false);
                tablaPR40.Rows[0].Cells[2].InsertParagraph();
                tablaPR40.Rows[0].Cells[2].Width = anchoTablaPR40Contenido;

                tablaPR40.Rows[1].Cells[0].Width = anchoTablaSeccion;

                AddFormattedTextCursiva(tablaPR40.Rows[1].Cells[1].Paragraphs.First(), "“11.2.", false, "Calibri", 11, "both", false);
                tablaPR40.Rows[1].Cells[1].Width = anchoTablaPR40Seccion;

                AddFormattedTextCursiva(tablaPR40.Rows[1].Cells[2].Paragraphs.First(), "Calidad del Suministro”", false, "Calibri", 11, "both", false);
                AddFormattedTextCursiva(tablaPR40.Rows[1].Cells[2].InsertParagraph(), "(…) Reporte 1 del COES: El COES emitirá la Decisión de Asignación de Responsabilidad por Transgresiones a la NTCSE y el cálculo preliminar de Resarcimientos, conforme a lo establecido en el inciso b) del Numeral 3.5 de la NTCSE. (…)”", false, "Calibri", 11, "both", false);
                tablaPR40.Rows[1].Cells[2].Width = anchoTablaPR40Contenido;

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION 2.1 CONTENIDO SEGUNDA PARTE
                Table tableAnalisis21SegundoContenido = documentoWord.InsertTable(1, 2);
                tableAnalisis21SegundoContenido.Design = TableDesign.TableNormal;
                tableAnalisis21SegundoContenido.AutoFit = AutoFit.ColumnWidth;

                AddFormattedText(tableAnalisis21SegundoContenido.Rows[0].Cells[0].Paragraphs.First(), string.Empty, true, "Calibri", 11, string.Empty, false);
                tableAnalisis21SegundoContenido.Rows[0].Cells[0].Width = anchoTablaSeccion;

                AddFormattedText(tableAnalisis21SegundoContenido.Rows[0].Cells[1].Paragraphs.First(), "NTCSE", true, "Calibri", 11, "both", true);
                tableAnalisis21SegundoContenido.Rows[0].Cells[1].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION NTCSE
                int anchoTablaNTCSE = 60;
                int anchoTablaNTCSEContenido = 490;

                var tablaNTCSE = documentoWord.InsertTable(1, 3);
                tablaNTCSE.Design = TableDesign.TableNormal;
                tablaNTCSE.AutoFit = AutoFit.ColumnWidth;

                tablaNTCSE.Rows[0].Cells[0].Width = anchoTablaSeccion;

                AddFormattedTextCursiva(tablaNTCSE.Rows[0].Cells[1].Paragraphs.First(), "“b)", false, "Calibri", 11, "both", false);
                tablaNTCSE.Rows[0].Cells[1].Width = anchoTablaNTCSE;

                AddFormattedTextCursiva(tablaNTCSE.Rows[0].Cells[2].Paragraphs.First(), "Dentro de los diez (10) días hábiles de recibido el Informe del Comité Técnico, el COES deberá emitir la decisión debidamente sustentada con un Informe Técnico y los fundamentos legales correspondientes. De ser el caso, la decisión contendrá la asignación de responsabilidades y el cálculo preliminar de las compensaciones correspondientes. El COES remitirá copia de su decisión a la Autoridad y a los Agentes involucrados.”", false, "Calibri", 11, "both", false);
                tablaNTCSE.Rows[0].Cells[2].Width = anchoTablaNTCSEContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region TITULO SECCION 2.2
                Table tableAnalisisSeccion2 = documentoWord.InsertTable(1, 3);
                tableAnalisisSeccion2.Design = TableDesign.TableNormal;
                tableAnalisisSeccion2.AutoFit = AutoFit.ColumnWidth;

                tableAnalisisSeccion2.Rows[0].Cells[0].Width = anchoTablaSeccion;
                AddFormattedText(tableAnalisisSeccion2.Rows[0].Cells[1].Paragraphs.First(), "2.2", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion2.Rows[0].Cells[1].Width = anchoTablaSeccionDos;
                AddFormattedText(tableAnalisisSeccion2.Rows[0].Cells[2].Paragraphs.First(), "Asignación de Responsabilidad", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion2.Rows[0].Cells[2].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION 2.2 CONTENIDO
                int tempTableAnalisisFilas = 3 + infoDecision.EventosSCO.Count;
                Table tableAnalisis = documentoWord.InsertTable(tempTableAnalisisFilas, 3);
                tableAnalisis.Design = TableDesign.TableNormal;
                tableAnalisis.AutoFit = AutoFit.ColumnWidth;

                for (int i = 0; i < tempTableAnalisisFilas; i++)
                {
                    tableAnalisis.Rows[i].Cells[0].Width = anchoTablaSeccion;
                    tableAnalisis.Rows[i].Cells[1].Width = anchoTablaSeccionTres;
                    tableAnalisis.Rows[i].Cells[2].Width = anchoTablaSeccionTresContenido;
                }

                AddFormattedText(tableAnalisis.Rows[0].Cells[1].Paragraphs.First(), "2.2.1", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisis.Rows[0].Cells[2].Paragraphs.First(), "De conformidad con lo dispuesto en el literal b) del Artículo 31º de la LCE , los concesionarios de generación, transmisión y distribución están obligados a conservar y mantener sus instalaciones en condiciones adecuadas para su operación eficiente. En concordancia con ello, el numeral 1.4.3 de la NTCOTRSI establece que son los propios Agentes los responsables de la seguridad de las personas y de sus instalaciones.", false, "Calibri", 11, "both", false);
                tableAnalisis.Rows[0].Cells[2].InsertParagraph();

                AddFormattedText(tableAnalisis.Rows[1].Cells[1].Paragraphs.First(), "2.2.2", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisis.Rows[1].Cells[2].Paragraphs.First(), "Conforme a lo establecido en el numeral 6.1.2 de la NTCSE, se considera interrupción a toda falta de suministro eléctrico en un punto de entrega, cuya duración sea igual o mayor a tres (03) minutos.", false, "Calibri", 11, "both", false);
                tableAnalisis.Rows[1].Cells[2].InsertParagraph();

                var tempEventosSCO = infoDecision.EventosSCO.OrderBy(x => x.EVENINI).ToList();

                for (int i = 0; i < tempEventosSCO.Count; i++)
                {
                    var tempEventoSCO = tempEventosSCO[i];
                    AddFormattedText(tableAnalisis.Rows[2 + i].Cells[1].Paragraphs.First(), $"2.2.{3 + i}", false, "Calibri", 11, "both", false);
                    AddFormattedText(tableAnalisis.Rows[2 + i].Cells[2].Paragraphs.First(), $"En el evento del {tempEventoSCO.EVENINI.Value.ToString($"dd.MM.yyyy")} a las {tempEventoSCO.EVENINI.Value.ToString($"HH:mm:ss")} h, debido a la {infoDecision.EventoCTAF.EVENASUNTO}, de titularidad de la empresa {infoDecision.Equipo.EMPRENOMB}, por falla monofásica en la fase “R” debido a descargas atmosféricas, se originó la interrupción de suministro de las SS.EE. Aguaytía, Parque Industrial, Yarinacocha y Pucallpa EUC con un total de XX,XX MW.", false, "Calibri", 11, "both", false);
                    tableAnalisis.Rows[2 + i].Cells[2].InsertParagraph();
                }

                AddFormattedText(tableAnalisis.Rows[2 + infoDecision.EventosSCO.Count].Cells[1].Paragraphs.First(), $"2.2.{3 + infoDecision.EventosSCO.Count}", false, "Calibri", 11, string.Empty, false);
                AddFormattedText(tableAnalisis.Rows[2 + infoDecision.EventosSCO.Count].Cells[2].Paragraphs.First(), "En ese sentido, al amparo de la normativa descrita en la presente Decisión y de acuerdo con el análisis del Informe Técnico, resulta responsable la empresa XXXXX por las referidas interrupciones de suministro.", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();
                #endregion

                #region TITULO SECCION 2.3
                Table tableAnalisisSeccion3 = documentoWord.InsertTable(1, 3);
                tableAnalisisSeccion3.Design = TableDesign.TableNormal;
                tableAnalisisSeccion3.AutoFit = AutoFit.ColumnWidth;

                tableAnalisisSeccion3.Rows[0].Cells[0].Width = anchoTablaSeccion;
                AddFormattedText(tableAnalisisSeccion3.Rows[0].Cells[1].Paragraphs.First(), "2.3", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion3.Rows[0].Cells[1].Width = anchoTablaSeccionDos;
                AddFormattedText(tableAnalisisSeccion3.Rows[0].Cells[2].Paragraphs.First(), "Resarcimientos", true, "Calibri", 11, "both", false);
                tableAnalisisSeccion3.Rows[0].Cells[2].Width = anchoTableContenido;

                documentoWord.InsertParagraph();
                #endregion

                #region SECCION 2.3 CONTENIDO
                Table tableAnalisisParte2 = documentoWord.InsertTable(4, 3);
                tableAnalisisParte2.Design = TableDesign.TableNormal;
                tableAnalisisParte2.AutoFit = AutoFit.ColumnWidth;

                for (int i = 0; i < 4; i++)
                {
                    tableAnalisisParte2.Rows[i].Cells[0].Width = anchoTablaSeccion;
                    tableAnalisisParte2.Rows[i].Cells[1].Width = anchoTablaSeccionTres;
                    tableAnalisisParte2.Rows[i].Cells[2].Width = anchoTablaSeccionTresContenido;
                }

                AddFormattedText(tableAnalisisParte2.Rows[0].Cells[1].Paragraphs.First(), "2.3.1", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisisParte2.Rows[0].Cells[2].Paragraphs.First(), "El literal b) del Numeral 3.5 de la NTCSE indica que la decisión contendrá, de ser el caso, la asignación de responsabilidades y el cálculo preliminar de compensaciones correspondientes.", false, "Calibri", 11, "both", false);
                tableAnalisisParte2.Rows[0].Cells[2].InsertParagraph();

                AddFormattedText(tableAnalisisParte2.Rows[1].Cells[1].Paragraphs.First(), "2.3.2", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisisParte2.Rows[1].Cells[2].Paragraphs.First(), "Asimismo, el Numeral 4.18 del PR-40 establece que los Resarcimientos son el “Monto a pagar por el (los) responsable (s) a los Suministradores como consecuencia de la asignación de responsabilidad efectuada por el COES, correspondiente a las Compensaciones pagadas conforme a lo señalado en la NTCSE y su Base Metodológica.”", false, "Calibri", 11, "both", false);
                tableAnalisisParte2.Rows[1].Cells[2].InsertParagraph();

                AddFormattedText(tableAnalisisParte2.Rows[2].Cells[1].Paragraphs.First(), "2.3.3", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisisParte2.Rows[2].Cells[2].Paragraphs.First(), "Por su parte, los numerales 5.1.6 y 11.2 del PR-40 indican que el COES emitirá la Decisión de Asignación de Responsabilidad por Transgresión a la NTCSE y el cálculo preliminar de los Resarcimientos correspondientes, conforme a lo establecido en el inciso b) del Numeral 3.5 de la NTCSE.", false, "Calibri", 11, "both", false);
                tableAnalisisParte2.Rows[2].Cells[2].InsertParagraph();

                AddFormattedText(tableAnalisisParte2.Rows[3].Cells[1].Paragraphs.First(), "2.3.4", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnalisisParte2.Rows[3].Cells[2].Paragraphs.First(), "En cumplimiento de lo establecido en la normativa precedente, se incluye el cálculo preliminar de resarcimientos, el que se muestra en el Anexo de esta Decisión.", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();
                #endregion

                #endregion
                
                documentoWord.InsertParagraph("Sobre la base del análisis efectuado en los numerales precedentes y de conformidad al Informe Técnico que forma parte integrante de la presente Decisión, la Dirección Ejecutiva del COES decide lo siguiente:").Font(new FontFamily("Calibri")).FontSize(11).Alignment = Alignment.both;
                documentoWord.InsertParagraph();

                #region DECIDE
                var parrafoDECIDE = documentoWord.InsertParagraph();
                AddFormattedText(parrafoDECIDE, "DECIDE:", true, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                var parrafoPrimero = documentoWord.InsertParagraph();
                AddFormattedText(parrafoPrimero, "PRIMERO:", true, "Calibri", 11, "both", false);
                AddFormattedText(parrafoPrimero, " Asignar responsabilidad a la empresa XXXXX por las transgresiones a la NTCSE, en lo referente a interrupción de suministros, que se muestran a continuación.", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                var codigosEventosDecision = infoDecision.TablaDecision.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();

                List<EventoDTO> tempEventosDecisionSCOOrdenados = new List<EventoDTO>();

                foreach (var item in codigosEventosDecision)
                {
                    var tempEventoSCO = infoDecision.EventosSCO.FirstOrDefault(x => x.EVENCODI.ToString() == item.codigo);

                    if (tempEventoSCO != null)
                    {
                        tempEventosDecisionSCOOrdenados.Add(tempEventoSCO);
                    }
                }

                tempEventosDecisionSCOOrdenados = tempEventosDecisionSCOOrdenados.OrderBy(x => x.EVENINI).ToList();

                if (codigosEventosDecision.Any())
                {
                    Table secuencia_4;
                    var registrosTotales = infoDecision.TablaDecision.ListaRegistros;

                    for (int i = 0; i < tempEventosDecisionSCOOrdenados.Count; i++)
                    {
                        var item = tempEventosDecisionSCOOrdenados[i];

                        var registros = registrosTotales.Where(x => x.codigo == item.EVENCODI.ToString() || x.codigo == null).OrderByDescending(c => c.codigo).ToList();
                        
                        string eventFecha = string.Empty;

                        if (item.EVENINI.HasValue)
                        {
                            eventFecha = item.EVENINI.Value.ToString("dd.MM.yyyy");
                        }

                        var parrafoTituloTabla = documentoWord.InsertParagraph();
                        AddFormattedText(parrafoTituloTabla, $"Evento del {eventFecha}", true, "Calibri", 11, "center", false);

                        var Tabla = infoDecision.TablaDecision;
                        Tabla.ListaRegistros = registros;
                        var numFilas = registros.Count;
                        var numColumnas = infoDecision.TablaDecision.CabeceraColumnas.Count;
                        secuencia_4 = documentoWord.InsertTable(numFilas + 1, numColumnas);

                        infoDecision.AnalisisFallasAppServicio.GenerarRptWord(ref secuencia_4, infoDecision.TablaDecision, 15, fdatcodi);

                        documentoWord.InsertParagraph();
                    }
                } 
                else
                {
                    documentoWord.InsertParagraph();
                    documentoWord.InsertParagraph("No registrado.").FontSize(11).Font(new FontFamily("Calibri"));
                }

                documentoWord.InsertParagraph();

                var parrafoSegundo = documentoWord.InsertParagraph();
                
                AddFormattedText(parrafoSegundo, "SEGUNDO:", true, "Calibri", 11, "both", false);
                AddFormattedText(parrafoSegundo, $" Incorporar el Informe Técnico COES/D/DO/SEV/{codigoInformeTecnico}, como parte integrante de la presente Decisión.", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                var parrafoNotifiquese = documentoWord.InsertParagraph();

                AddFormattedText(parrafoNotifiquese, "Notifíquese.", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph().InsertPageBreakAfterSelf();

                #endregion

                #region TITULO ANEXO
                var parrafoTituloAnexo = documentoWord.InsertParagraph();
                AddFormattedText(parrafoTituloAnexo, $"ANEXO\n\nCÁLCULO PRELIMINAR DE RESARCIMIENTO\n\n", true, "Calibri", 11, "center", false);
                #endregion

                #region CONTENIDO ANEXO
                var parrafoAnexoContenido = documentoWord.InsertParagraph();
                AddFormattedText(parrafoAnexoContenido, "De acuerdo con el numeral 11.2 del PR-40, se adjunta el cálculo de Resarcimiento, el cual es de carácter preliminar y que debe ser validado por las empresas afectadas y sus respectivos suministradores.", false, "Calibri", 11, "both", false);
                documentoWord.InsertParagraph();
                documentoWord.InsertParagraph();

                var parrafoAnexoContenido2 = documentoWord.InsertParagraph();
                AddFormattedText(parrafoAnexoContenido2, "La fórmula aplicada en este caso es la N°14 de la NTCSE:", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();
                documentoWord.InsertParagraph();

                var parrafoAnexoTituloMedio = documentoWord.InsertParagraph();
                AddFormattedTextCursiva(parrafoAnexoTituloMedio, "Compensaciones por Interrupciones = e. E. ENS\n\n", true, "Calibri", 11, "center", false);

                var parrafoAnexoDonde = documentoWord.InsertParagraph();
                AddFormattedText(parrafoAnexoDonde, "Dónde:", false, "Calibri", 11, "both", false);

                var parrafoAnexoDonde2 = documentoWord.InsertParagraph();
                AddFormattedText(parrafoAnexoDonde2, "e: Es la compensación unitaria por incumplimiento en la Calidad de Suministro, cuyo valor en la actualidad es ", false, "Calibri", 11, "both", false);
                AddFormattedText(parrafoAnexoDonde2, "350 U$/MWh.", true, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                Table tableAnexoDonde = documentoWord.AddTable(2,2);
                tableAnexoDonde.Design = TableDesign.TableNormal;
                tableAnexoDonde.AutoFit = AutoFit.Contents;
                int anchoTableAnexoDonde = 20;

                AddFormattedText(tableAnexoDonde.Rows[0].Cells[0].Paragraphs.First(), "E:", false, "Calibri", 11, "both", false);
                tableAnexoDonde.Rows[0].Cells[0].Width = anchoTableAnexoDonde;
                AddFormattedText(tableAnexoDonde.Rows[0].Cells[1].Paragraphs.First(), "Es el factor que toma en consideración la magnitud de los indicadores de calidad de suministro y está definido en función del Número de Interrupciones por Cliente por Semestre (N) y la Duración Total Acumulada de Interrupciones (D).", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnexoDonde.Rows[0].Cells[1].Paragraphs.First(), " Para este cálculo preliminar se considerará un valor igual a 1.", true, "Calibri", 11, "both", false);
                tableAnexoDonde.Rows[0].Cells[1].InsertParagraph();

                AddFormattedText(tableAnexoDonde.Rows[1].Cells[0].Paragraphs.First(), "ENS:", false, "Calibri", 11, "both", false);
                tableAnexoDonde.Rows[1].Cells[0].Width = anchoTableAnexoDonde;
                AddFormattedText(tableAnexoDonde.Rows[1].Cells[1].Paragraphs.First(), "Es la energía teóricamente no suministrada a un cliente.", false, "Calibri", 11, "both", false);
                AddFormattedText(tableAnexoDonde.Rows[1].Cells[1].Paragraphs.First(), " Para este cálculo preliminar se considerará como la multiplicación de la potencia suministrada en el momento en que se produjo la interrupción (MW) por la duración (tiempo en horas) individual de la interrupción.\n", true, "Calibri", 11, "both", false);

                documentoWord.InsertTable(tableAnexoDonde);

                var parrafoLuegoAnexo = documentoWord.InsertParagraph();
                AddFormattedText(parrafoLuegoAnexo, "Luego aplicando la formula N°14, anteriormente mostrada, se obtiene:", false, "Calibri", 11, "both", false);

                documentoWord.InsertParagraph();

                var codigosResarcimientoEventos = infoDecision.TablaResarcimiento.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();

                List<EventoDTO> tempEventosSCOOrdenados = new List<EventoDTO>();

                foreach (var item in codigosResarcimientoEventos)
                {
                    var tempEventoSCO = infoDecision.EventosSCO.FirstOrDefault(x => x.EVENCODI.ToString() == item.codigo);
                    
                    if (tempEventoSCO != null)
                    {
                        tempEventosSCOOrdenados.Add(tempEventoSCO);
                    }
                }

                tempEventosSCOOrdenados = tempEventosSCOOrdenados.OrderBy(x => x.EVENINI).ToList();
                
                if (tempEventosSCOOrdenados.Count > 0)
                {
                    for (int i = 0; i < tempEventosSCOOrdenados.Count; i++)
                    {
                        var item = tempEventosSCOOrdenados[i];

                        var registros = infoDecision.TablaResarcimiento.ListaRegistros.Where(x => x.codigo == item.EVENCODI.ToString() || x.codigo == null).OrderByDescending(c => c.codigo).ToList();
                        var tempRegistroTotal = registros[registros.Count - 1];
                        var tempTotal = tempRegistroTotal.ListaCelda[6].Valor?.ToString("0.000");

                        var tempAnexoEventFecha = infoDecision.EventosSCO.Find(x => x.EVENCODI.ToString() == item.EVENCODI.ToString()).EVENINI;

                        var parrafoAnexoEventoIteracion = documentoWord.InsertParagraph();
                        AddFormattedText(parrafoAnexoEventoIteracion, $"Para el evento del {tempAnexoEventFecha.Value.ToString("dd.MM.yyyy")} a las {tempAnexoEventFecha.Value.ToString("HH:mm:ss")} h:", true, "Calibri", 11, "both", true);

                        documentoWord.InsertParagraph();

                        var parrafoAnexoEventoIteracion2 = documentoWord.InsertParagraph();
                        AddFormattedText(parrafoAnexoEventoIteracion2, $"Compensaciones por Interrupción de suministros XXXXX = 350 x 1 x {tempTotal}", false, "Calibri", 11, "both", false);

                        documentoWord.InsertParagraph();

                        var parrafoAnexoEventoIteracion3 = documentoWord.InsertParagraph();
                        AddFormattedTextCursiva(parrafoAnexoEventoIteracion3, "Compensaciones por Interrupción de suministros XXXXXX = XXXXX,XX U$", true, "Calibri", 11, "both", false);

                        documentoWord.InsertParagraph();
                    }
                } 
                else
                {
                    documentoWord.InsertParagraph("No existen registros.").FontSize(11).Font(new FontFamily("Calibri"));
                    documentoWord.InsertParagraph();
                }
                
                #endregion

                documentoWord.InsertParagraph().InsertPageBreakAfterSelf();

                if (tempEventosSCOOrdenados.Count > 0)
                {
                    FontFamily fontDoc = new FontFamily("Calibri");

                    Table secuencia_5;
                    var registrosTotales = infoDecision.TablaResarcimiento.ListaRegistros;

                    var codigosEventosRearcimiento = infoDecision.TablaResarcimiento.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();

                    for (int i = 0; i < tempEventosSCOOrdenados.Count; i++)
                    {
                        var item = tempEventosSCOOrdenados[i];

                        var tempCuadroEventFecha = infoDecision.EventosSCO.Find(x => x.EVENCODI.ToString() == item.EVENCODI.ToString()).EVENINI;

                        var parrafoCuadroIteracion = documentoWord.InsertParagraph();
                        AddFormattedText(parrafoCuadroIteracion, $"\nCuadro N° {i + 1}", true, "Calibri", 11, "center", false);
                        AddFormattedText(parrafoCuadroIteracion, $": Interrupciones de Suministro Producidas por el evento del {tempCuadroEventFecha.Value.ToString("dd.MM.yyyy")}\n", false, "Calibri", 11, "center", false);

                        var parrafoCuadroIteracionX = documentoWord.InsertParagraph();
                        AddFormattedText(parrafoCuadroIteracionX, "XXXXXX", true, "Calibri", 11, "center", false);
                        documentoWord.InsertParagraph();

                        var registrosN = registrosTotales.Where(x => x.codigo == item.EVENCODI.ToString() || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                        var Tabla = infoDecision.TablaResarcimiento;
                        Tabla.ListaRegistros = registrosN;
                        var numFilas = registrosN.Count;
                        var numColumnas = infoDecision.TablaResarcimiento.CabeceraColumnas.Count;
                        secuencia_5 = documentoWord.InsertTable(numFilas + 1, numColumnas);

                        infoDecision.AnalisisFallasAppServicio.GenerarRptWord(ref secuencia_5, infoDecision.TablaResarcimiento, 16, fdatcodi);
                    }
                } 
                else
                {
                    documentoWord.InsertParagraph("No registrado.").FontSize(11).Font(new FontFamily("Calibri"));
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    documentoWord.SaveAs(stream);
                    response = stream.GetBuffer();

                    FileServer.CreateFolder(null, null, fileName);
                    documentoWord.SaveAs(fileName + reporteName);
                }
            }

            return new GenerarDecisionCtafFDTO()
            {
                WordContent = response,
                FileName = $"Formato_D_{infoDecision.AnalisisFalla.CodigoEvento}.docx"
            };
        }

        private void AddFormattedTextCursiva(Paragraph p, string text, bool isBold, string fontFamily, int fontSize, string align, bool underLine)
        {
            AddFormattedText(p, text, isBold, fontFamily, fontSize, align, underLine);
            p.Italic();
        }

        private void AddFormattedText(Paragraph p, string text, bool isBold, string fontFamily, int fontSize, string align, bool underLine)
        {
            p.Append(text);

            if (isBold)
            {
                p.Bold();
            }

            if (!string.IsNullOrEmpty(fontFamily))
            {
                p.Font(new FontFamily(fontFamily));
            }

            if (fontSize > 0)
            {
                p.FontSize(fontSize);
            }

            if (!string.IsNullOrEmpty(align) && align == "center")
            {
                p.Alignment = Alignment.center;
            }

            if (!string.IsNullOrEmpty(align) && align == "right")
            {
                p.Alignment = Alignment.right;
            }

            if (!string.IsNullOrEmpty(align) && align == "both")
            {
                p.Alignment = Alignment.both;
            }

            if (underLine)
            {
                p.UnderlineStyle(UnderlineStyle.singleLine);
            }
        }
    }
}