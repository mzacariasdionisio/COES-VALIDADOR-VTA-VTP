using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using COES.Dominio.DTO.Sic;
using System.IO;
using System.Configuration;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.Eventos.Helper
{
    public class PdfDocument
    {

        /// <summary>
        /// Permite generar el reporte de pertubación en formato PDF
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="evento"></param>
        /// <param name="idEvento"></param>
        /// <param name="path"></param>
        public void GenerarReporteInforme(EveEventoDTO evento, List<EveInformeItemDTO> listItem, int idInforme, string path, string fileName,
            int indicadorLogo, string pathLogo, string empresa)
        {
            #region Encabezado

            Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 80f, 50f);
            string rutapdf = path + fileName;

            if (File.Exists(rutapdf))
            {
                File.Delete(rutapdf);
            }

            FileStream file = new FileStream(rutapdf, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);
            pdfWriter.PageEvent = new ITextEvents(indicadorLogo, pathLogo, empresa);
            System.Drawing.Color cCelda = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
            System.Drawing.Color cBorde = System.Drawing.ColorTranslator.FromHtml("#9A9A9A");
            BaseColor colorCelda = new BaseColor(cCelda);
            BaseColor colorBorde = new BaseColor(cBorde);

            pdfDoc.Open();
            Paragraph titulo = new Paragraph("INFORME TÉCNICO", new Font(Font.FontFamily.TIMES_ROMAN, 10f, Font.BOLD, BaseColor.BLACK));
            titulo.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(titulo);

            #endregion

            #region Seccion 1 al 3

            //------Agrega la tabla de articulos de promocion
            PdfPTable table = new PdfPTable(2);
            //actual width of table in points
            table.TotalWidth = 500f;
            //fix the absolute width of the table
            table.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 2f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 5f;

            PdfPCell cell = new PdfPCell(new Phrase("1. EVENTO", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(evento.Tipoevenabrev, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("2. FECHA", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DateTime)evento.Evenini).ToString(ConstantesAppServicio.FormatoFecha), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("3. HORA", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DateTime)evento.Evenini).ToString(ConstantesAppServicio.FormatoOnlyHora), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            pdfDoc.Add(table);

            #endregion

            #region Seccion 4

            string texto = string.Empty;
            EveInformeItemDTO item = listItem.Where(x => x.Itemnumber == 4).FirstOrDefault();

            if (item != null)
            {
                texto = item.Descomentario;
            }

            pdfDoc.Add(new Phrase("4. DESCRIPCION DEL EVENTO", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            pdfDoc.Add(new Paragraph(texto, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            pdfDoc.Add(new Paragraph(""));

            #endregion

            #region Seccion 5.1
            //5
            pdfDoc.Add(new Phrase("5. CONDICIONES OPERATIVAS PREVIAS", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            pdfDoc.Add(new Paragraph(""));
            //5.1
            pdfDoc.Add(new Phrase("a. GENERACIÖN", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listGeneracion = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 1).ToList();

            PdfPTable tableItem = new PdfPTable(6);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 1.5f, 1f, 1.5f, 1.5f, 1.5f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            PdfPCell cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Subestación", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Unidad", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Activa(MW)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Reactiva(MVAR)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Observaciones", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);


            if (listGeneracion.Count > 0)
            {
                int indice = 1;
                foreach (EveInformeItemDTO entity in listGeneracion)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Areanomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Equinomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potactiva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potreactiva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Desobservacion, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 5.2

            //5.2
            pdfDoc.Add(new Phrase("b. FLUJO DE POTENCIAS EN LAS LÍNEAS", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listLineas = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 2).ToList();

            tableItem = new PdfPTable(7);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Código L.T.", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Subestaciones", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.Colspan = 2;
            cellItem.Rowspan = 1;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Activa (MW)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Reactiva (MVAR)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Observaciones", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("De", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("A", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);


            if (listLineas.Count > 0)
            {
                int indice = 1;
                foreach (EveInformeItemDTO entity in listLineas)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Equinomb + " " + entity.Equicodi, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase(entity.Equinomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Subestacionde == null ? "" : entity.Subestacionde.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Subestacionhasta == null ? "" : entity.Subestacionhasta.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Potactiva == null ? "" : entity.Potactiva.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potreactiva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 5.3

            //5.3          
            pdfDoc.Add(new Phrase("c. FLUJO DE POTENCIAS EN LOS TRANSFORMADORES", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listTransformadores = listItem.Where(x => x.Itemnumber == 5 && x.Subitemnumber == 3).ToList();

            tableItem = new PdfPTable(7);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Subestación", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Código", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Activa (MW)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia Reactiva (MVAR)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Nivel de Tensión", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Observaciones", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);


            if (listTransformadores.Count > 0)
            {
                int indice = 1;
                foreach (EveInformeItemDTO entity in listTransformadores)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Areanomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Areanomb == null ? "" : entity.Areanomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Equinomb + " " + entity.Equicodi, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Equinomb == null ? "" : entity.Equinomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potactiva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potreactiva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Niveltension.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Desobservacion, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 6

            //6
            pdfDoc.Add(new Phrase("6. SECUENCIA CRONOLÓGICA", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listSecuencia = listItem.Where(x => x.Itemnumber == 6).ToList();

            tableItem = new PdfPTable(2);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 1.5f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("Hora", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Descripción del evento", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            if (listSecuencia.Count > 0)
            {
                foreach (EveInformeItemDTO entity in listSecuencia)
                {
                    cellItem = new PdfPCell(new Phrase(entity.Itemhora, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 7

            //7
            pdfDoc.Add(new Phrase("7. ACTUACIÓN DE LAS PROTECCIONES", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listActuacion = listItem.Where(x => x.Itemnumber == 7).ToList();

            tableItem = new PdfPTable(5);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1.5f, 1f, 1.5f, 1.5f, 1f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("Subestación", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Equipo", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Señalización", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("INT", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("A/C", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            if (listActuacion.Count > 0)
            {
                foreach (EveInformeItemDTO entity in listActuacion)
                {
                    //cellItem = new PdfPCell(new Phrase(entity.Areanomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Areanomb == null ? "" : entity.Areanomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Equinomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Equinomb == null ? "" : entity.Equinomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Senializacion, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Senializacion == null ? "" : entity.Senializacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Internomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Internomb == null ? "" : entity.Internomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Ac, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Ac == null ? "" : entity.Ac.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 8

            //8
            pdfDoc.Add(new Phrase("8. CONTADOR DE INTERRUPTORES Y PARARRAYOS", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listcontador = listItem.Where(x => x.Itemnumber == 8).ToList();

            tableItem = new PdfPTable(9);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 2f, 2f, 2f, 1f, 1f, 1f, 1f, 1f, 1f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("Subestación", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Celda", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Código Interruptor", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Antes", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 3;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Después", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 3;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("R", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("S", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("T", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("R", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("S", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("T", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);


            if (listcontador.Count > 0)
            {
                foreach (EveInformeItemDTO entity in listcontador)
                {
                    //cellItem = new PdfPCell(new Phrase(entity.Areanomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Areanomb == null ? "" : entity.Areanomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);


                    //cellItem = new PdfPCell(new Phrase(entity.Equinomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Equinomb == null ? "" : entity.Equinomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Internomb, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Internomb == null ? "" : entity.Internomb.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Ra.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Ra == null ? "" : entity.Ra.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Sa.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Sa == null ? "" : entity.Sa.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Ta.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Ta == null ? "" : entity.Ta.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Rd.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Rd == null ? "" : entity.Rd.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Sd.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Sd == null ? "" : entity.Sd.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Td.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Td == null ? "" : entity.Td.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 9

            texto = string.Empty;
            EveInformeItemDTO itemAnalisis = listItem.Where(x => x.Itemnumber == 9).FirstOrDefault();

            if (itemAnalisis != null)
            {
                texto = itemAnalisis.Descomentario;
            }

            pdfDoc.Add(new Phrase("9. ANÁLISIS DEL EVENTO", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            pdfDoc.Add(new Paragraph(texto, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            pdfDoc.Add(new Paragraph(""));

            #endregion

            #region Seccion 10

            pdfDoc.Add(new Phrase("10. SUMINISTROS INTERRUMPIDOS", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listSuministros = listItem.Where(x => x.Itemnumber == 10).ToList();

            tableItem = new PdfPTable(7);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Suministro", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Potencia MW", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Tiempo de desconexión (min)", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 3;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Protección", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 2;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Inicio", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Final", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Duración", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            cellItem.Rowspan = 1;
            cellItem.Colspan = 1;
            tableItem.AddCell(cellItem);


            if (listSuministros.Count > 0)
            {
                int indice = 1;
                foreach (EveInformeItemDTO entity in listSuministros)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Sumininistro == null ? "" : entity.Sumininistro.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase(entity.Potenciamw.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((((DateTime)entity.Intinicio).ToString("dd/MM/yyyy HH:mm:ss")), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((((DateTime)entity.Intfin).ToString("dd/MM/yyyy HH:mm:ss")), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    entity.Duracion = 0;

                    if (entity.Intinicio != null && entity.Intfin != null)
                    {
                        TimeSpan duracion = ((DateTime)entity.Intfin) - ((DateTime)entity.Intinicio);
                        entity.Duracion = (int)duracion.TotalMinutes;
                    }

                    cellItem = new PdfPCell(new Phrase(entity.Duracion.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Proteccion, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Proteccion == null ? "" : entity.Proteccion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    cellItem.Colspan = 1;
                    cellItem.Rowspan = 1;
                    tableItem.AddCell(cellItem);

                    indice++;

                }
            }

            pdfDoc.Add(tableItem);


            #endregion

            #region Seccion 11

            pdfDoc.Add(new Phrase("11. CONCLUSIONES", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listConclusion = listItem.Where(x => x.Itemnumber == 11).ToList();

            tableItem = new PdfPTable(2);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 2f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Conclusión", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            if (listConclusion.Count > 0)
            {
                int indice = 1;

                foreach (EveInformeItemDTO entity in listConclusion)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 12

            pdfDoc.Add(new Phrase("12. ACCIONES EJECUTADAS", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listAcciones = listItem.Where(x => x.Itemnumber == 12).ToList();

            tableItem = new PdfPTable(2);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 2f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Acción ejecutada", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            if (listAcciones.Count > 0)
            {
                int indice = 1;

                foreach (EveInformeItemDTO entity in listAcciones)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    //cellItem = new PdfPCell(new Phrase(entity.Desobservacion, new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            #region Seccion 13


            pdfDoc.Add(new Phrase("13. OBSERVACIONES Y RECOMENDACIONES", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            List<EveInformeItemDTO> listObservacion = listItem.Where(x => x.Itemnumber == 13).ToList();

            tableItem = new PdfPTable(2);
            //actual width of table in points
            tableItem.TotalWidth = 500f;
            //fix the absolute width of the table
            tableItem.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            widths = new float[] { 1f, 2f };
            tableItem.SetWidths(widths);
            tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            tableItem.SpacingBefore = 5f;
            tableItem.SpacingAfter = 5f;

            cellItem = new PdfPCell(new Phrase("N°", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            cellItem = new PdfPCell(new Phrase("Observación / Recomendación", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
            cellItem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellItem.BorderColor = colorBorde;
            cellItem.BackgroundColor = colorCelda;
            tableItem.AddCell(cellItem);

            if (listObservacion.Count > 0)
            {
                int indice = 1;

                foreach (EveInformeItemDTO entity in listObservacion)
                {
                    cellItem = new PdfPCell(new Phrase(indice.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    cellItem = new PdfPCell(new Phrase((entity.Desobservacion == null ? "" : entity.Desobservacion.ToString()), new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.NORMAL, BaseColor.BLACK)));
                    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellItem.BorderColor = colorBorde;
                    tableItem.AddCell(cellItem);

                    indice++;
                }
            }

            pdfDoc.Add(tableItem);

            #endregion

            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el reporte de pertubación en formato PDF
        /// </summary>
        /// <param name="LstEvento"></param>
        /// <param name="oAnalisisFallaDTO"></param>
        public void GenerarReporteCitacionPDF(List<EventoDTO> LstEvento, AnalisisFallaDTO oAnalisisFallaDTO, string path )
        {
            bool presencial = false;
            string fileName = string.Empty;

            if (LstEvento.Count > 0)
            {
                if (LstEvento[0].EVENTIPOFALLA == "P")
                {
                    fileName = "CITACION P " + oAnalisisFallaDTO.AFECORR + "_" + oAnalisisFallaDTO.AFEANIO + ".pdf";
                    presencial = true;
                }
                else
                {
                    fileName = "CITACION NP " + oAnalisisFallaDTO.AFECORR + "_" + oAnalisisFallaDTO.AFEANIO + ".pdf";
                    presencial = false;
                }

            }

            string pathLogo = ConfigurationManager.AppSettings["RutaInformesCtaf"].ToString();
            string imagenLogo = pathLogo + "logocoes.png";

            Document pdfDoc = new Document(PageSize.A4, 60f, 60f, 90f, 50f);

            string rutapdf = path + fileName;

            if (File.Exists(rutapdf))
            {
                File.Delete(rutapdf);
            }

            FileStream file = new FileStream(rutapdf, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);
            
            pdfWriter.PageEvent = new EventosTitulos(1, imagenLogo, String.Format("COMITÉ TÉCNICO\n DE ANÁLISIS DE FALLAS \n AÑO {0} (CT-AF) ", DateTime.Now.Year.ToString()));
            System.Drawing.Color cCelda = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
            System.Drawing.Color cBorde = System.Drawing.ColorTranslator.FromHtml("#9A9A9A");
            BaseColor colorCelda = new BaseColor(cCelda);
            BaseColor colorBorde = new BaseColor(cBorde);
            
            pdfDoc.Open();
            Paragraph bSalto = new Paragraph("\n");
            pdfDoc.Add(bSalto);
            Paragraph bTitulo = new Paragraph(("San Isidro, " + DateTime.Now.Day.ToString("00") + " de " + f_get_nombre_mes(DateTime.Now.Month) + " de " + DateTime.Now.Year.ToString() + "\r\n"), new Font(FontFactory.GetFont("Calibri", 12f, BaseColor.BLACK)));
            bTitulo.Alignment = Element.ALIGN_LEFT;

            Paragraph titulo = new Paragraph(String.Format("CITACIÓN N°{0}-{1}", oAnalisisFallaDTO.AFECORR, oAnalisisFallaDTO.AFEANIO), new Font(FontFactory.GetFont("Calibri", 12f, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
            titulo.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(bTitulo);
            pdfDoc.Add(titulo);
            pdfDoc.Add(bSalto);

            string pa1 = string.Empty;
            pa1 = "Se hace de conocimiento a todos los representantes al Comité de Técnico de Análisis de Fallas - Año " + DateTime.Now.Year.ToString() + " (CT-AF), ";
            if(presencial)
                pa1 += " a la reunión que se llevará a cabo en la sede del COES-SINAC, para tratar la agenda que se indica a continuación:";
            else
                pa1 += "sobre el análisis que se realizará, respecto al evento que se indica a continuación:";

            Paragraph p1 = new Paragraph((pa1), new Font(FontFactory.GetFont("Calibri", 12f, BaseColor.BLACK)));
            p1.Alignment = Element.ALIGN_BOTTOM;

            Paragraph p2 = new Paragraph(("AGENDA:"), new Font(FontFactory.GetFont("Calibri", 12f, Font.UNDERLINE,BaseColor.BLACK))); 
            p2.Alignment = Element.ALIGN_LEFT;

            pdfDoc.Add(p1);
            pdfDoc.Add(bSalto);
            pdfDoc.Add(p2);
            pdfDoc.Add(bSalto);
            PdfPTable table = new PdfPTable(4);
            if (LstEvento.Count > 0)
            {               
                iTextSharp.text.Font baseFontCabTable = new iTextSharp.text.Font(FontFactory.GetFont("Calibri", 12f, Font.BOLD, BaseColor.BLACK));
                iTextSharp.text.Font baseFontBodyTable = new iTextSharp.text.Font(FontFactory.GetFont("Calibri", 12f, BaseColor.BLACK));
                iTextSharp.text.Font baseFontBodyTableSubrayado = new iTextSharp.text.Font(FontFactory.GetFont("Calibri", 12f, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK));

                PdfPCell pdfCell1 = new PdfPCell(new Phrase("CÓDIGO", baseFontCabTable));
                PdfPCell pdfCell2 = new PdfPCell(new Phrase("FECHA Y HORA", baseFontCabTable));
                PdfPCell pdfCell3 = new PdfPCell(new Phrase("DESCRIPCIÓN DEL EVENTO", baseFontCabTable));
                PdfPCell pdfCell4 = new PdfPCell(new Phrase("EMPRESAS INVOLUCRADAS*", baseFontCabTable));

                float[] anchoDeColumnas = new float[] { 150f, 150f, 200f, 200f };
                
                pdfCell1.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                
                pdfCell2.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                
                pdfCell3.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                
                pdfCell4.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell4.BackgroundColor = BaseColor.LIGHT_GRAY;

                table.SetWidths(anchoDeColumnas);
                table.AddCell(pdfCell1);
                table.AddCell(pdfCell2);
                table.AddCell(pdfCell3);
                table.AddCell(pdfCell4);

                for (int i = 0; i < LstEvento.Count; i++)
                {
                    PdfPCell cellCod = new PdfPCell(new Phrase(LstEvento[i].CODIGO.ToString(), baseFontBodyTable));
                    PdfPCell cellFechSCO = new PdfPCell(new Phrase(LstEvento[i].FechasEventosSco.ToString(), baseFontBodyTable));
                    PdfPCell cellAsunto = new PdfPCell(new Phrase(LstEvento[i].EVENASUNTO.ToString(), baseFontBodyTable));
                    PdfPCell cellEmpInv = new PdfPCell(new Phrase(LstEvento[i].EmpresaInvolucrada.ToString(), baseFontBodyTable));

                    table.AddCell(cellCod);
                    table.AddCell(cellFechSCO);
                    table.AddCell(cellAsunto);
                    table.AddCell(cellEmpInv);
                }
                pdfDoc.Add(table);

                if (presencial)
                {
                    Paragraph p3 = new Paragraph(("* Las empresas involucradas derivan de un análisis preliminar realizado por el COES, por lo que su identificación es Referencial. Es responsabilidad de cada empresa evaluar el evento a fin de determinar su participación en el mismo como empresa involucrada y determinar su asistencia a la reunión del CT-AF."), baseFontBodyTable);
                    p3.Alignment = Element.ALIGN_BOTTOM;

                    Paragraph p3_1 = new Paragraph(("La asistencia a la reunión de las empresas mencionadas como involucradas en el evento es indispensable."), baseFontBodyTable);
                    p3_1.Alignment = Element.ALIGN_BOTTOM;

                    Paragraph p4 = new Paragraph(("FECHA DE LA REUNIÓN:\r\n"), baseFontBodyTableSubrayado);
                    p4.Alignment = Element.ALIGN_LEFT;

                    var fecha = f_get_nombre_dia(Convert.ToDateTime(oAnalisisFallaDTO.AFEREUFECHAPROG).DayOfWeek) + " " + oAnalisisFallaDTO.AFEREUFECHAPROG.Value.ToString("dd.MM.yyyy") + ", a las " + oAnalisisFallaDTO.AFEREUHORAPROG + " h.";

                    Paragraph p5 = new Paragraph(fecha, baseFontBodyTable);
                    p5.Alignment = Element.ALIGN_LEFT;

                    Paragraph p6 = new Paragraph(("LUGAR:\r\n"),baseFontBodyTableSubrayado);
                    p6.Alignment = Element.ALIGN_LEFT;

                    Paragraph p7 = new Paragraph(("Sala de reuniones: 201"), baseFontBodyTable);
                    p7.Alignment = Element.ALIGN_LEFT;

                    Paragraph p8 = new Paragraph(("Sede del COES: C. Manuel Roaud y Paz Soldán 364 San Isidro - Lima."), baseFontBodyTable);
                    p8.Alignment = Element.ALIGN_LEFT;

                    pdfDoc.Add(p3);
                    pdfDoc.Add(p3_1);
                    pdfDoc.Add(p4);
                    pdfDoc.Add(p5);
                    pdfDoc.Add(p6);
                    pdfDoc.Add(p7);
                    pdfDoc.Add(p8);
                }
                else
                {
                    Paragraph p3 = new Paragraph(("* Las empresas involucradas derivan de un análisis preliminar realizado por el COES, por lo que su identificación es Referencial. Es responsabilidad de cada empresa evaluar el evento a fin de determinar su participación en el mismo como empresa involucrada."), baseFontBodyTable);
                    p3.Alignment = Element.ALIGN_BOTTOM;

                    var _p3_1 = new Chunk("Se considera por conveniente que el presente evento sea analizado en forma ", baseFontBodyTable);
                    var _p3_1_1 = new Chunk("no presencial", baseFontCabTable);
                    var _p3_1_2 = new Chunk(", coordinando vía correo electrónico y / o vía telefónica con las empresas involucradas.\r\n", baseFontBodyTable);

                    Paragraph p3_1 = new Paragraph(_p3_1);
                    p3_1.Add(_p3_1_1);
                    p3_1.Add(_p3_1_2);
                    p3_1.Alignment = Element.ALIGN_BOTTOM;

                    Paragraph p4 = new Paragraph(("Asimismo, la información correspondiente será publicada en el portal web del COES.\r\n"), baseFontBodyTable);
                    p4.Alignment = Element.ALIGN_LEFT;

                    Paragraph p5 = new Paragraph(("FECHA DE ENVÍO DEL INFORME CTAF:\r\n"), baseFontBodyTableSubrayado);
                    p5.Alignment = Element.ALIGN_LEFT;

                    var fecha = f_get_nombre_dia(Convert.ToDateTime(oAnalisisFallaDTO.AFEREUFECHAPROG).DayOfWeek) + " " + oAnalisisFallaDTO.AFEREUFECHAPROG.Value.ToString("dd.MM.yyyy");

                    Paragraph p5_1 = new Paragraph(fecha, baseFontBodyTable);
                    p5_1.Alignment = Element.ALIGN_LEFT;

                    pdfDoc.Add(p3);
                    pdfDoc.Add(p3_1);
                    pdfDoc.Add(p4);
                    pdfDoc.Add(p5);
                    pdfDoc.Add(p5_1);
                }
                Paragraph p9 = new Paragraph(("\r\nAtentamente,"), baseFontBodyTable);
                p9.Alignment = Element.ALIGN_LEFT;
                pdfDoc.Add(p9);

                string pathFirma = ConfigurationManager.AppSettings["RutaInformesCtaf"].ToString();
                string imagenfirma = pathFirma + "firma_sierra.png";

                iTextSharp.text.Image imgFirma = iTextSharp.text.Image.GetInstance(imagenfirma);
                imgFirma.ScalePercent(0.5f);
                imgFirma.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(imgFirma);

                Paragraph p9_1 = new Paragraph(("\t\t\t\t\t\tIng. Eleazar Sierra"), baseFontBodyTable);
                p9_1.Alignment = Element.ALIGN_CENTER;

                Paragraph p10 = new Paragraph(("\t\t\t\t\t\tCoordinador CT-AF"), baseFontBodyTable);
                p10.Alignment = Element.ALIGN_CENTER;

                Paragraph p11 = new Paragraph(("\t\t\t\t\t\t    COES-SINAC"), baseFontBodyTable);
                p11.Alignment = Element.ALIGN_CENTER;

                pdfDoc.Add(p9_1);
                pdfDoc.Add(p10);
                pdfDoc.Add(p11);
            }

            
            pdfDoc.Close();
        }

        private string f_get_nombre_mes(int pi_mes)
        {

            switch (pi_mes)
            {
                case 1:
                    return "enero";
                case 2:
                    return "febrero";
                case 3:
                    return "marzo";
                case 4:
                    return "abril";
                case 5:
                    return "mayo";
                case 6:
                    return "junio";
                case 7:
                    return "julio";
                case 8:
                    return "agosto";
                case 9:
                    return "setiembre";
                case 10:
                    return "octubre";
                case 11:
                    return "noviembre";
                case 12:
                    return "diciembre";
                default:
                    return "";
            }
        }

        private string f_get_nombre_dia(DayOfWeek ps_dia)
        {

            switch (ps_dia)
            {
                case DayOfWeek.Sunday:
                    return "DOMINGO";
                case DayOfWeek.Monday:
                    return "LUNES";
                case DayOfWeek.Tuesday:
                    return "MARTES";
                case DayOfWeek.Wednesday:
                    return "MIÉRCOLES";
                case DayOfWeek.Thursday:
                    return "JUEVES";
                case DayOfWeek.Friday:
                    return "VIERNES";
                case DayOfWeek.Saturday:
                    return "SÁBADO";
                default:
                    return "";
            }


        }
    }

    /// <summary>
    /// Clase para manejo del header and footer del pdf
    /// </summary>
    public class ITextEvents : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now;
        private string _header;
        public int IndicadorLogo { get; set; }
        public string PathLogo { get; set; }
        public string Empresa { get; set; }

        public ITextEvents()
        {

        }

        public ITextEvents(int indicadorLogo, string pathLogo, string empresa)
        {
            this.IndicadorLogo = indicadorLogo;
            this.PathLogo = pathLogo;
            this.Empresa = empresa;
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        /// <summary>
        /// Evento de Apertura del documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        /// <summary>
        /// Agrega el header y footer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL,
                iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD,
                iTextSharp.text.BaseColor.BLACK);

            PdfPTable pdfTab = new PdfPTable(3);
            String text = writer.PageNumber + " de ";

            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetBottom(30));
            }

            string ruta = string.Empty;

            if (IndicadorLogo == 0)
            {
                ruta = PathLogo;
            }
            else if (IndicadorLogo == 1)
            {
                string path = ConfigurationManager.AppSettings["RutaExportacionInformeEvento"].ToString();
                ruta = path + "coes.png";
            }

            PdfPCell pdfCell1 = null;

            if (IndicadorLogo == 0 || IndicadorLogo == 1)
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(ruta);
                imgLogo.ScalePercent(0.5f);
                imgLogo.Alignment = Element.ALIGN_LEFT;
                pdfCell1 = new PdfPCell(imgLogo, true);
                pdfCell1.FixedHeight = 30f;
                pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfCell1.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell1.Border = Rectangle.NO_BORDER;
            }
            else
            {
                pdfCell1 = new PdfPCell(new Phrase("NO EXISTE LOGO"));
                //this.Empresa
            }

            PdfPCell pdfCell2 = new PdfPCell(new Phrase("INFORMA: \n" + this.Empresa));
            PdfPCell pdfCell3 = new PdfPCell(new Phrase("INFORME TÉCNICO \n" + DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha), baseFontBig));

            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell3.VerticalAlignment = Element.ALIGN_TOP;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;

            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);

            pdfTab.TotalWidth = 500f;
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            cb.MoveTo(50, document.PageSize.Height - 70);
            cb.LineTo(document.PageSize.Width - 50, document.PageSize.Height - 70);
            cb.Stroke();

            cb.MoveTo(50, document.PageSize.GetBottom(40));
            cb.LineTo(document.PageSize.Width - 50, document.PageSize.GetBottom(40));
            cb.Stroke();
        }

        /// <summary>
        /// Evento que permite cerrar el documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }

    public class EventosTitulos : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now;
        private string _header;
        public int IndicadorLogo { get; set; }
        public string PathLogo { get; set; }
        public string Cabecera { get; set; }

        public EventosTitulos()
        {

        }

        public EventosTitulos(int indicadorLogo, string pathLogo, string cabecera)
        {
            this.IndicadorLogo = indicadorLogo;
            this.PathLogo = pathLogo;
            this.Cabecera = cabecera;
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        /// <summary>
        /// Evento de Apertura del documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        /// <summary>
        /// Agrega el header y footer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL,
                iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(FontFactory.GetFont("Calibri", 10f, Font.BOLD, BaseColor.BLACK));

            PdfPTable pdfTab = new PdfPTable(2);
            String text = writer.PageNumber + " de ";

            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetBottom(30));
            }

            string ruta = string.Empty;

            if (IndicadorLogo == 0)
            {
                ruta = PathLogo;
            }
            else if (IndicadorLogo == 1)
            {
                string path = ConfigurationManager.AppSettings["RutaInformesCtaf"].ToString();
                ruta = path + "logocoes.png";
            }

            PdfPCell pdfCell1 = null;

            if (IndicadorLogo == 0 || IndicadorLogo == 1)
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(ruta);
                imgLogo.ScalePercent(0.5f);
                imgLogo.Alignment = Element.ALIGN_LEFT;
                pdfCell1 = new PdfPCell(imgLogo, true);
                pdfCell1.FixedHeight = 60f;
                pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfCell1.VerticalAlignment = Element.ALIGN_CENTER;
                pdfCell1.Border = Rectangle.NO_BORDER;
            }
            else
            {
                pdfCell1 = new PdfPCell(new Phrase("NO EXISTE LOGO"));
                //this.Empresa
            }
            PdfPCell pdfCell3 = new PdfPCell(new Phrase(String.Format("COMITÉ TÉCNICO\n DE ANÁLISIS DE FALLAS \n AÑO {0} (CT-AF) ", DateTime.Now.Year.ToString()), baseFontBig));

            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_TOP;

            pdfCell1.Border = 0;
            pdfCell3.Border = 0;

            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell3);

            pdfTab.TotalWidth = 500f;
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            cb.MoveTo(50, document.PageSize.Height - 70);
            cb.Stroke();

            cb.MoveTo(50, document.PageSize.GetBottom(40));
            cb.Stroke();
        }

        /// <summary>
        /// Evento que permite cerrar el documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }

    
}