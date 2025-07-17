using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace COES.MVC.Extranet.Areas.PMPO.Helper
{
    /// <summary>
    ///  Clase ayuda para construir PDF's.
    /// </summary>
    internal class PDFHelper
    {
        #region [ Fields ]

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
        public static Font tableHeaderFont = new Font(BaseFont, 12, Font.BOLD, WhiteColor);
        public static Font tableFont = new Font(BaseFont, 10, Font.NORMAL, FontColor);
        public static Font tableWhiteFont = new Font(BaseFont, 10, Font.NORMAL, WhiteColor);

        #endregion

        #region FooterTable

        /// <summary>
        /// Escribe en número de página.
        /// </summary>
        public class FooterTable : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                Paragraph footer = new Paragraph(string.Format("{0}", writer.CurrentPageNumber), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                footer.Alignment = Element.ALIGN_CENTER;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = writer.PageSize.Width;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = PdfPCell.NO_BORDER;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, 0, 30, writer.DirectContent);
            }
        }

        #endregion

        #region CreateTableHeaderCell

        /// <summary>
        /// Método para crear las celdas de la cabecera del PDF
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static PdfPCell CreateTableHeaderCell(string text)
        {
            return CreateCell(text, tableHeaderFont, BlueColor, greyColor, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE);
        }

        #endregion

        #region CreateTableCell

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

        #endregion

        #region CreateTableCell

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

        #endregion

        #region CreateCell

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
                PaddingLeft = 6f,
                PaddingRight = 6f,
                PaddingTop = 6f,
                PaddingBottom = 8f,
            };

            return pdfCell;
        }

        #endregion

        #region CreateCell

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
                PaddingLeft = 6f,
                PaddingRight = 6f,
                PaddingTop = 6f,
                PaddingBottom = 8f,
            };

            return pdfCell;
        }

        #endregion
    }
}
