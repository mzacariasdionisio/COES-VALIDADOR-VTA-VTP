using System;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    /// <summary>
    /// clase para funcionalidades asociadas al DevExpress
    /// </summary>
    public class UtilDevExpressIntervenciones
    {
        /// <summary>
        /// Generar PDF desde HTML
        /// </summary>
        /// <param name="textoHtml"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void GenerarPDFdeHtml(string textoHtml, string path, string fileName)
        {
            string fullPath = path + fileName;
            var memStrem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textoHtml));

            RichEditDocumentServer srv = new RichEditDocumentServer();
            srv.Document.Sections[0].Page.Landscape = true;
            srv.Document.Unit = DevExpress.Office.DocumentUnit.Millimeter;
            srv.Document.Sections[0].Margins.Left = 0.2f;
            srv.Document.Sections[0].Margins.Right = 0.2f;
            srv.Document.Sections[0].Margins.Top = 0f;
            srv.Document.Sections[0].Margins.Bottom = 0f;

            srv.LoadDocument(memStrem, DevExpress.XtraRichEdit.DocumentFormat.Html);

            //verificar que archivo no exista
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            using (Stream str = System.IO.File.Create(fullPath))
            {
                srv.ExportToPdf(str);
            }
            //srv.Dispose();

            //return System.IO.File.ReadAllBytes(fullPath);
        }

        /// <summary>
        /// Generar pdf desde Word
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileNameWord"></param>
        /// <param name="fileNamePdf"></param>
        public static void GenerarPDFdeWord(string path, string fileNameWord, string fileNamePdf)
        {
            using (RichEditDocumentServer wordProcessor = new RichEditDocumentServer())
            {
                // Load a DOCX document.
                wordProcessor.LoadDocument(path + fileNameWord);

                // Specify export options.
                PdfExportOptions options = new PdfExportOptions();
                options.DocumentOptions.Author = "COES";
                options.Compressed = false;
                options.ImageQuality = PdfJpegImageQuality.Highest;

                // Export the document to a stream.
                using (FileStream pdfFileStream = new FileStream(path + fileNamePdf, FileMode.Create))
                {
                    wordProcessor.ExportToPdf(pdfFileStream, options);
                }
            }
        }

    }
}
