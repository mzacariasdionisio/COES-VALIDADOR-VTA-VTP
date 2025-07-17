using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Text;

namespace COES.Servicios.Aplicacion.Helper
{
    /// <summary>
    /// Utileria del TextSharp
    /// </summary>
    public static class UtilTextSharpPdf
    {
        /// <summary>
        /// Inicializa las variables para la configuración del PDF
        /// </summary>
        public static BaseColor BlueColor = new BaseColor(41, 128, 185);
        public static BaseColor FontColor = new BaseColor(51, 88, 115);
        public static BaseColor WhiteColor = new BaseColor(255, 255, 255);
        public static BaseColor greyColor = new BaseColor(221, 221, 221);
        public static BaseColor greenColor = new BaseColor(89, 202, 75);
        public static BaseColor redColor = new BaseColor(255, 102, 102);

        public static BaseFont BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        public static Font tableHeaderFont = new Font(BaseFont, 8, Font.NORMAL, WhiteColor);
        public static Font tableFont = new Font(BaseFont, 8, Font.NORMAL, FontColor);
        public static Font tableWhiteFont = new Font(BaseFont, 8, Font.NORMAL, WhiteColor);

        /// <summary>
        /// Método para crear las celdas de la cabecera del PDF
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static PdfPCell CreateTableHeaderCell(string text)
        {
            return CreateCell(text, tableHeaderFont, BlueColor, greyColor, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE);
        }

        /// <summary>
        /// Método para crear la celdas del PDF
        /// </summary>
        /// <param name="text"></param>
        /// <param name="horizontalAlignment"></param>
        /// <returns></returns>
        public static PdfPCell CreateTableCell(string text, int horizontalAlignment)
        {
            return CreateCell(text, tableFont, WhiteColor, greyColor, horizontalAlignment, PdfPCell.ALIGN_MIDDLE);
        }

        /// <summary>
        /// Método para crear la celdas del PDF
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="horizontalAlignment"></param>
        /// <returns></returns>
        public static PdfPCell CreateTableCell(string text, Font font, BaseColor backgroundColor, int horizontalAlignment)
        {
            return CreateCell(text, font, backgroundColor, greyColor, horizontalAlignment, PdfPCell.ALIGN_MIDDLE);
        }

        /// <summary>
        /// Método para crear la celdas del PDF
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAligment"></param>
        /// <returns></returns>
        public static PdfPCell CreateCell(string text, Font font, BaseColor backgroundColor, BaseColor borderColor, int horizontalAlignment, int verticalAligment)
        {
            PdfPCell pdfCell = new PdfPCell(new Phrase(text, font))
            {
                BackgroundColor = backgroundColor,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAligment,
                BorderColor = borderColor,
                PaddingLeft = 1f,
                PaddingRight = 1f,
                PaddingTop = 2f,
                PaddingBottom = 2f,
            };

            return pdfCell;
        }

        /// <summary>
        /// Método para crear la celdas del PDF
        /// </summary>
        /// <param name="image"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAligment"></param>
        /// <returns></returns>
        public static PdfPCell CreateCell(Image image, BaseColor backgroundColor, BaseColor borderColor, int horizontalAlignment, int verticalAligment)
        {
            PdfPCell pdfCell = new PdfPCell(image)
            {
                BackgroundColor = backgroundColor,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAligment,
                BorderColor = borderColor,
                PaddingLeft = 3f,
                PaddingRight = 3f,
                PaddingTop = 3f,
                PaddingBottom = 4f,
            };

            return pdfCell;
        }


    }

    public class FooterTextSharpPdf : PdfPageEventHelper
    {
        public int TotalPagina = 0;

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (TotalPagina > 0)
            {
                int totalPaginas = TotalPagina;
                string Page = (totalPaginas != 0) ? string.Format("{0} de {1}", writer.CurrentPageNumber, totalPaginas) : string.Format("{0}", writer.CurrentPageNumber);
                Paragraph footer2 = new Paragraph(Page, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                footer2.Alignment = Element.ALIGN_RIGHT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = writer.PageSize.Width;
                PdfPCell cell = new PdfPCell(footer2);
                cell.Border = PdfPCell.NO_BORDER;
                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                cell.PaddingRight = 20f;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, 0, 30, writer.DirectContent);
            }
        }
    }
}
