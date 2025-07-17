using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System;

namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{
    public class GenerarWordResarcimiento
    {
        public void GenerarWordDemo()
        {
            // Ruta de la plantilla existente
            string templatePath = "D:\\Plantilla_Informe.docx";

            // Ruta para guardar el nuevo documento
            string outputPath = "D:\\DocumentoGenerado.docx";

            // Cargar la plantilla
            // Crear un servidor de documentos
            using (RichEditDocumentServer wordProcessor = new RichEditDocumentServer())
            {
                // Cargar la plantilla
                wordProcessor.LoadDocument(templatePath);

                // Acceder al documento
                Document document = wordProcessor.Document;

                // Insertar un título principal (Heading 1)
                DocumentPosition titlePosition = document.Range.End;
                DocumentRange titleRange = document.InsertText(titlePosition, "Título Principal\n");
                CharacterProperties titleProperties = document.BeginUpdateCharacters(titleRange);
                titleProperties.FontSize = 16;
                titleProperties.Bold = true;
                document.EndUpdateCharacters(titleProperties);

                ParagraphProperties titleParagraph = document.BeginUpdateParagraphs(titleRange);
                titleParagraph.Style = document.ParagraphStyles["Estilo2"]; // Estilo de nivel 1
                document.EndUpdateParagraphs(titleParagraph);

                // Insertar un subtítulo (Heading 2)
                DocumentPosition subtitlePosition = document.Range.End;
                DocumentRange subtitleRange = document.InsertText(subtitlePosition, "Subtítulo 1\n");
                CharacterProperties subtitleProperties = document.BeginUpdateCharacters(subtitleRange);
                subtitleProperties.FontSize = 14;
                subtitleProperties.Bold = true;
                document.EndUpdateCharacters(subtitleProperties);

                ParagraphProperties subtitleParagraph = document.BeginUpdateParagraphs(subtitleRange);
                subtitleParagraph.Style = document.ParagraphStyles["Estilo2"]; // Estilo de nivel 2
                document.EndUpdateParagraphs(subtitleParagraph);

                // Insertar contenido normal
                DocumentPosition contentPosition = document.Range.End;
                document.InsertText(contentPosition, "Este es el contenido debajo del subtítulo.\n");

                // Insertar otro título y subtítulo
                DocumentRange anotherTitleRange = document.InsertText(document.Range.End, "Otro Título\n");
                CharacterProperties anotherTitleProperties = document.BeginUpdateCharacters(anotherTitleRange);
                anotherTitleProperties.FontSize = 16;
                anotherTitleProperties.Bold = true;
                document.EndUpdateCharacters(anotherTitleProperties);

                ParagraphProperties anotherTitleParagraph = document.BeginUpdateParagraphs(anotherTitleRange);
                anotherTitleParagraph.Style = document.ParagraphStyles["Estilo2"]; // Estilo de nivel 1
                document.EndUpdateParagraphs(anotherTitleParagraph);

                DocumentRange anotherSubtitleRange = document.InsertText(document.Range.End, "Otro Subtítulo\n");
                CharacterProperties anotherSubtitleProperties = document.BeginUpdateCharacters(anotherSubtitleRange);
                anotherSubtitleProperties.FontSize = 14;
                anotherSubtitleProperties.Bold = true;
                document.EndUpdateCharacters(anotherSubtitleProperties);

                ParagraphProperties anotherSubtitleParagraph = document.BeginUpdateParagraphs(anotherSubtitleRange);
                anotherSubtitleParagraph.Style = document.ParagraphStyles["Estilo2"]; // Estilo de nivel 2
                document.EndUpdateParagraphs(anotherSubtitleParagraph);

                //- Reemplazamos texto en contenido principal
                document.ReplaceAll("{TextoReemplazar}", "Semestre 2024", SearchOptions.None);

                
                // Buscar y reemplazar texto dentro de las cajas de texto
                foreach (Shape shape in document.Shapes)
                {
                    if (shape.ShapeFormat.TextBox != null) // Verifica que sea una caja de texto
                    {
                        SubDocument textoCaja = shape.TextBox.Document; // Obtén el subdocumento de la caja de texto
                        textoCaja.ReplaceAll("{TextoReemplazar}", "Semestre 2024", SearchOptions.None);
                    }
                }


                foreach (Section seccion in document.Sections)
                {
                    // Acceder al encabezado (Header)
                    SubDocument encabezado = seccion.BeginUpdateHeader();
                   
                    encabezado.ReplaceAll("{PERIODOHEADER}", "Valor Reemplazo", SearchOptions.None);
                    
                    seccion.EndUpdateHeader(encabezado);
                    
                }



                // Insertar una tabla
                Table table = document.Tables.Create(document.Range.End, 30, 5); // 30 filas, 5 columnas

                // Formatear la tabla
                table.TableLayout = TableLayoutType.Fixed; // Ancho fijo
                table.BeginUpdate();

                // Configurar la primera fila como encabezado
                TableRow headerRow = table.Rows[0];               
                headerRow.RepeatAsHeaderRow = true;
                foreach (TableCell cell in headerRow.Cells)
                {
                    cell.PreferredWidthType = WidthType.Auto; // Ancho fijo de las celdas
                    cell.PreferredWidth = 100f; // 100 puntos por celda

                    // Agregar texto al encabezado
                    DocumentRange cellRange = cell.Range;
                    document.InsertText(cellRange.Start, "Encabezado ");

                    // Configurar alineación y estilo
                    ParagraphProperties paragraphProperties = document.BeginUpdateParagraphs(cellRange);
                    paragraphProperties.Alignment = ParagraphAlignment.Center;
                    document.EndUpdateParagraphs(paragraphProperties);

                    // Aplicar color de fondo
                    cell.BackgroundColor = System.Drawing.Color.LightGray;
                }

                // Agregar contenido a las filas
                for (int i = 1; i < table.Rows.Count; i++)
                {
                    TableRow row = table.Rows[i];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        TableCell cell = row.Cells[j];
                        DocumentRange cellRange = cell.Range;

                        // Agregar texto a la celda
                        document.InsertText(cellRange.Start, $"Fila {i + 1}, Columna {j + 1}");
                    }
                }

                table.EndUpdate();

                DocumentRange anotherSubtitleRange2 = document.InsertText(document.Range.End, "Otro Subtítulo 3\n");
                CharacterProperties anotherSubtitleProperties2 = document.BeginUpdateCharacters(anotherSubtitleRange2);
                anotherSubtitleProperties2.FontSize = 14;
                anotherSubtitleProperties2.Bold = true;
                document.EndUpdateCharacters(anotherSubtitleProperties2);

                ParagraphProperties anotherSubtitleParagraph2 = document.BeginUpdateParagraphs(anotherSubtitleRange2);
                anotherSubtitleParagraph2.Style = document.ParagraphStyles["Estilo2"]; // Estilo de nivel 2
                document.EndUpdateParagraphs(anotherSubtitleParagraph2);

                // Actualizar la tabla de contenido (TOC)
                //document.UpdateTableOfContents();

                // Guardar el documento
                wordProcessor.SaveDocument(outputPath, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

                Console.WriteLine($"Documento generado exitosamente en: {outputPath}");
            }
        }

    }
}
