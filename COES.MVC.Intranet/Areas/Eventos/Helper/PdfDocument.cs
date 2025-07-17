using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
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
        public void GenerarReportePerturbacion(List<InformePerturbacionDTO> Lista, EventoDTO evento, int idEvento, string path)
        {

            ////Creando el documento PDF
            //Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 40f, 20f);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //Font times = new Font(bfTimes, 8, Font.NORMAL, Color.BLACK);

            //string rutapdf = path + Constantes.NombreReportePerturbacionPdf;
            //FileStream file = new FileStream(rutapdf, FileMode.Create);

            //PdfWriter.GetInstance(pdfDoc, file);

            //System.Drawing.Color cCelda = System.Drawing.ColorTranslator.FromHtml("#4674A6");
            //System.Drawing.Color cBorde = System.Drawing.ColorTranslator.FromHtml("#A9C8FA");

            //Color colorCelda = new Color(cCelda);
            //Color colorBorde = new Color(cBorde);

            ////Indica la Cabecera del Documento

            //string ruta = path + Constantes.NombreLogoCoes;
            //iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(ruta);
            //imgLogo.ScalePercent(0.5f);
            //imgLogo.Alignment = Element.ALIGN_LEFT;

            //PdfPTable tableCab = new PdfPTable(3);
            ////actual width of table in points
            //tableCab.TotalWidth = 500f;
            //float[] widthsheader = new float[] { 1f, 1.5f, 1f };
            ////fix the absolute width of the table
            //tableCab.LockedWidth = true;
            //tableCab.SetWidths(widthsheader);
            //tableCab.SpacingBefore = 30f;

            //PdfPCell pdfcellImage = new PdfPCell(imgLogo, true);
            //pdfcellImage.FixedHeight = 30f;
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_LEFT;
            //pdfcellImage.VerticalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //pdfcellImage = new PdfPCell(new Phrase("REPORTE DE PERTURBACIONES DEL\n SEIN", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //pdfcellImage = new PdfPCell(new Phrase(String.Format("Fecha de Emisión:\n {0}", DateTime.Now.ToString("dd/MM/yyyy")), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //pdfcellImage = new PdfPCell(new Phrase("", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //pdfcellImage = new PdfPCell(new Phrase("IAO – " + idEvento + "-" + DateTime.Now.Year, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //pdfcellImage = new PdfPCell(new Phrase("", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //pdfcellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfcellImage.Border = Rectangle.NO_BORDER;
            //tableCab.AddCell(pdfcellImage);

            //Phrase phHeader = new Phrase();
            //phHeader.Add(tableCab);

            //HeaderFooter header = new HeaderFooter(phHeader, false);
            //header.Border = Rectangle.NO_BORDER;
            //header.Alignment = Element.ALIGN_CENTER;
            //header.Bottom = 30f;
            //pdfDoc.Header = header;

            ////Indica el Numero de Página
            //HeaderFooter pie = new HeaderFooter(new Phrase("Pagina:", times), true);
            //pie.BorderWidthTop = 1;
            //pie.BorderWidthBottom = 0;
            //pdfDoc.Footer = pie;

            //pdfDoc.Open();

            //pdfDoc.Add(new Paragraph(" "));
       
            ////------Agrega la tabla de articulos de promocion
            //PdfPTable table = new PdfPTable(2);
            ////actual width of table in points
            //table.TotalWidth = 500f;
            ////fix the absolute width of the table
            //table.LockedWidth = true;
            ////relative col widths in proportions - 1/3 and 2/3
            //float[] widths = new float[] { 1f, 2f };
            //table.SetWidths(widths);
            //table.HorizontalAlignment = Element.ALIGN_LEFT;
            ////leave a gap before and after the table
            //table.SpacingBefore = 40f;
            //table.SpacingAfter = 20f;

            ////Creacion de la tabla de Articulos a regalar en el PDF

            //PdfPCell cell = new PdfPCell(new Phrase("1. FECHA", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = colorCelda;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);
            //string fecha = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString("dd/MM/yyyy") : string.Empty;
            //cell = new PdfPCell(new Phrase(fecha, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);

            //string time = (evento.EVENINI != null) ? ((DateTime)evento.EVENINI).ToString("HH:mm:ss") : string.Empty;
            //cell = new PdfPCell(new Phrase("2. HORA DE INICIO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(time, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);

            //cell = new PdfPCell(new Phrase("3. EQUIPO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(evento.EQUIABREV, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);

            //cell = new PdfPCell(new Phrase("4. PROPIETARIO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(evento.EMPRNOMB, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);


            //cell = new PdfPCell(new Phrase("5. CAUSA DE LA PERTURBACIÓN", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //string texto = string.Empty;
            //InformePerturbacionDTO item = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemCausa).FirstOrDefault();

            //if (item != null)
            //{
            //    texto = item.SUBCAUSADESC;
            //    cell = new PdfPCell(new Phrase(texto, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.BorderColor = colorBorde;
            //    table.AddCell(cell);
            //}
            
            //pdfDoc.Add(table);
            
            ///*LLENANDO LOS DATOS DEL VEHICULO*/
            
            ////------Agrega la tabla de articulos de promocion
            //table = new PdfPTable(1);
            ////actual width of table in points
            //table.TotalWidth = 500f;
            ////fix the absolute width of the table
            //table.LockedWidth = true;
            ////relative col widths in proportions - 1/3 and 2/3
            //widths = new float[] { 1f };
            //table.SetWidths(widths);
            //table.HorizontalAlignment = Element.ALIGN_LEFT;
            ////leave a gap before and after the table
            //table.SpacingBefore = 5f;
            //table.SpacingAfter = 20f;


            //InformePerturbacionDTO descripcion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemDescripcion).FirstOrDefault();

            //if (descripcion != null)
            //{
            //    texto = descripcion.ITEMDESCRIPCION;
            //}


            //cell = new PdfPCell(new Phrase("6. DESCRIPCIÓN", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);

            //cell = new PdfPCell(new Phrase(texto, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //table.AddCell(cell);

            //List<InformePerturbacionDTO> listaSecuencia = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemSecuencia).ToList();

            //cell = new PdfPCell(new Phrase("7. SECUENCIA DE EVENTOS", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);

            //if (listaSecuencia.Count > 0)
            //{
            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("Hora(hh:mm:ss)", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    foreach (InformePerturbacionDTO entity in listaSecuencia)
            //    {
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMTIME, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);

            //}

            //List<InformePerturbacionDTO> listaActuacion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemActuacion).ToList();
            //int indice = 8;
            //int row = 10;

            //if (listaActuacion.Count > 0)
            //{
            //    cell = new PdfPCell(new Phrase("8. ACTUACIÓN DE LAS PROTECCIONES", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.BorderColor = colorBorde;
            //    cell.BackgroundColor = colorCelda;
            //    table.AddCell(cell);


            //    PdfPTable tableItem = new PdfPTable(5);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 1f, 1f, 1f, 1f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("Subestación", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Equipo", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Señalización", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Interruptor", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("A/C", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    foreach (InformePerturbacionDTO entity in listaActuacion)
            //    {
            //        cellItem = new PdfPCell(new Phrase(entity.SUBESTACION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.EQUINOMB, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMSENALIZACION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.INTERRUPTORNOMB, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMAC, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}

            //cell = new PdfPCell(new Phrase(string.Format("{0}. ANÁLISIS DEL EVENTO", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;

            //InformePerturbacionDTO analisis = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemAnalisis).FirstOrDefault();

            //if (analisis != null)
            //{


            //    cell = new PdfPCell(new Phrase(analisis.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}
            //row++;


            //cell = new PdfPCell(new Phrase(string.Format("{0}. CONCLUSIONES", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;


            //List<InformePerturbacionDTO> listaConclusion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemConclusion).OrderBy(x => x.ITEMORDEN).ToList();

            //if (listaConclusion.Count > 0)
            //{

            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("NRO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    int index = 1;
            //    foreach (InformePerturbacionDTO entity in listaConclusion)
            //    {
            //        cellItem = new PdfPCell(new Phrase(index.ToString(), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        index++;
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);

            //}

            //row++;



            //cell = new PdfPCell(new Phrase(string.Format("{0}. RECOMENDACIONES", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;


            //List<InformePerturbacionDTO> listaRecomendacion = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemRecomendacion).OrderBy(x => x.ITEMORDEN).ToList();

            //if (listaRecomendacion.Count > 0)
            //{

            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("NRO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    int index = 1;
            //    foreach (InformePerturbacionDTO entity in listaRecomendacion)
            //    {
            //        cellItem = new PdfPCell(new Phrase(index.ToString(), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        index++;
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}

            //row++;


            //cell = new PdfPCell(new Phrase(string.Format("{0}. OPORTUNIDADES DE MEJORA", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;


            //List<InformePerturbacionDTO> listaOportunidad = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemOportunidad).OrderBy(x => x.ITEMORDEN).ToList();

            //if (listaOportunidad.Count > 0)
            //{

            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("NRO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    int index = 1;
            //    foreach (InformePerturbacionDTO entity in listaOportunidad)
            //    {
            //        cellItem = new PdfPCell(new Phrase(index.ToString(), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        index++;
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}

            //row++;


            //cell = new PdfPCell(new Phrase(string.Format("{0}. ACUERDOS", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;


            //List<InformePerturbacionDTO> listaAcuerdo = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemAcuerdo).OrderBy(x => x.ITEMORDEN).ToList();

            //if (listaAcuerdo.Count > 0)
            //{
            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("NRO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    int index = 1;
            //    foreach (InformePerturbacionDTO entity in listaAcuerdo)
            //    {
            //        cellItem = new PdfPCell(new Phrase(index.ToString(), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        index++;
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}

            //row++;

            //cell = new PdfPCell(new Phrase(string.Format("{0}. PLAZOS DE ATENCIÓN PARA LAS OPORTUNIDADES DE MEJORA", indice), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.WHITE)));
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = colorBorde;
            //cell.BackgroundColor = colorCelda;
            //table.AddCell(cell);
            //row++;
            //indice++;


            //List<InformePerturbacionDTO> listaPlazo = Lista.Where(x => x.ITEMTIPO == TipoItemPerturbacion.ItemPlazo).OrderBy(x => x.ITEMORDEN).ToList();

            //if (listaPlazo.Count > 0)
            //{
            //    PdfPTable tableItem = new PdfPTable(2);
            //    //actual width of table in points
            //    tableItem.TotalWidth = 400f;
            //    //fix the absolute width of the table
            //    tableItem.LockedWidth = true;
            //    //relative col widths in proportions - 1/3 and 2/3
            //    widths = new float[] { 1f, 2f };
            //    tableItem.SetWidths(widths);
            //    tableItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    //leave a gap before and after the table
            //    tableItem.SpacingBefore = 20f;
            //    tableItem.SpacingAfter = 20f;

            //    PdfPCell cellItem = new PdfPCell(new Phrase("NRO", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);
            //    cellItem = new PdfPCell(new Phrase("Descripción", new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //    cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellItem.BorderColor = colorBorde;
            //    tableItem.AddCell(cellItem);

            //    int index = 1;
            //    foreach (InformePerturbacionDTO entity in listaPlazo)
            //    {
            //        cellItem = new PdfPCell(new Phrase(index.ToString(), new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        cellItem = new PdfPCell(new Phrase(entity.ITEMDESCRIPCION, new Font(Font.TIMES_ROMAN, 8f, Font.NORMAL, Color.BLACK)));
            //        cellItem.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cellItem.BorderColor = colorBorde;
            //        tableItem.AddCell(cellItem);
            //        index++;
            //    }

            //    cell = new PdfPCell(tableItem);
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.BorderColor = colorBorde;
            //    cell.Padding = 5;
            //    table.AddCell(cell);
            //}

            //row++;

            //pdfDoc.Add(table);
            //pdfDoc.Close();
        }
    }
}