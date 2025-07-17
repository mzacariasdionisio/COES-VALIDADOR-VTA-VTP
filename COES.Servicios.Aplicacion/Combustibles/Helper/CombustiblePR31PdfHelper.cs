using COES.Dominio.DTO.Sic;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Combustibles.Helper
{
    public class CombustiblePR31PdfHelper
    {
        /// <summary>
        /// Genera el reporte pdf de cargos
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="idEnvio"></param>
        /// <param name="estado"></param>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static Document GenerarReporteCargo(string ruta, string rutaLogo, string nameFile, List<CbLogenvioDTO> lstCargoEnvios)
        {
            Document pdfDoc = new Document();
            CbLogenvioDTO cargoEnvio = lstCargoEnvios.Any() ? lstCargoEnvios.First() : null;

            #region Encabezado            
            string rutapdf = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutapdf);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutapdf);
            }

            FileStream file = new FileStream(rutapdf, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            System.Drawing.Color cCelda = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
            System.Drawing.Color cBorde = System.Drawing.ColorTranslator.FromHtml("#9A9A9A");
            //BaseColor colorCelda = new BaseColor(cCelda);
            //BaseColor colorBorde = new BaseColor(cBorde);

            pdfDoc.Open();

            PdfPTable table0 = new PdfPTable(3);

            PdfPCell pdfCell1 = null;
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(rutaLogo);
            imgLogo.ScalePercent(0.5f);
            imgLogo.Alignment = Element.ALIGN_LEFT;
            pdfCell1 = new PdfPCell(imgLogo, true);
            pdfCell1.Border = Rectangle.NO_BORDER;
            pdfCell1.PaddingTop = 1f;
            pdfCell1.PaddingBottom = 25f;
            table0.AddCell(pdfCell1);
            pdfCell1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell1.Border = Rectangle.NO_BORDER;
            table0.AddCell(pdfCell1);
            pdfCell1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell1.Border = Rectangle.NO_BORDER;
            table0.AddCell(pdfCell1);
            pdfDoc.Add(table0);

            Paragraph titulo = new Paragraph("CARGO DE RECEPCIÓN DOCUMENTO", new Font(Font.FontFamily.TIMES_ROMAN, 12f, Font.BOLD, BaseColor.BLACK));
            titulo.Alignment = Element.ALIGN_CENTER;

            pdfDoc.Add(titulo);

            #endregion

            #region Cuerpo

            //------Agrega la tabla de articulos de promocion
            PdfPTable table = new PdfPTable(3);
            //actual width of table in points
            table.TotalWidth = 500f;
            //fix the absolute width of the table
            table.LockedWidth = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 8f, 1f, 8f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 5f;

            PdfPCell cell = new PdfPCell(new Phrase("APLICATIVO EXTRANET : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Costo Combustible - PR31 (Gaseoso) ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("CÓDIGO DE ENVÍO : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.Cbenvcodi.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("ESTADO : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.Estenvnomb.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("EMPRESA : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.Emprnomb, new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("FECHA DE RECEPCIÓN : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.LogenvfecrecepcionDesc, new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("USUARIO DE RECEPCIÓN : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.Logenvusurecepcion, new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("FECHA DE LECTURA : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.LogenvfeclecturaDesc, new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("USUARIO DE LECTURA : ", new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 8f, Font.BOLD, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cargoEnvio.Logenvusulectura, new Font(Font.FontFamily.TIMES_ROMAN, 9f, Font.NORMAL, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);

            pdfDoc.Add(table);

            #endregion

            pdfDoc.Close();

            return pdfDoc;
        }

    }
}
