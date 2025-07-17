using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{
    class PdfDocument
    {
        /// <summary>
        /// CU09 - Permite generar el archivo de exportación de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEgresoMinfo">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <param name="pathLogo">Ruta del logo del COES</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoMinfo(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            Chunk c1 = new Chunk("INFORMACIÓN INGRESADA PARA VTP Y PEAJES" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre, FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph(p1);

            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco


            //float[] widths = new float[] { 29, 23, 16, 16, 16 };
            PdfPTable table = new PdfPTable(14);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.HeaderRows = 2;
            //Formato  de Texto y numero
            var TextFont = FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var NumberFont = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Background cell
            BaseColor colorbg = new BaseColor(36, 92, 134);


            //Cabecera 1
            PdfPCell cellcabe = new PdfPCell(new Phrase("", TextFont));
            cellcabe.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabe.Colspan = 5;
            cellcabe.Border = 0;
            table.AddCell(cellcabe);

            PdfPCell cellEP = new PdfPCell(new Phrase("PARA EGRESO POTENCIA", TextFont));
            cellEP.HorizontalAlignment = Element.ALIGN_CENTER;
            cellEP.Colspan = 2;
            cellEP.BackgroundColor = colorbg;
            table.AddCell(cellEP);

            PdfPCell cellPC = new PdfPCell(new Phrase("PARA PEAJE POR CONEXIÓN", TextFont));
            cellPC.HorizontalAlignment = Element.ALIGN_CENTER;
            cellPC.Colspan = 3;
            cellPC.BackgroundColor = colorbg;
            table.AddCell(cellPC);

            PdfPCell cellFCO = new PdfPCell(new Phrase("PARA FLUJO DE CARGA ÓPTIMO", TextFont));
            cellFCO.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFCO.Colspan = 3;
            cellFCO.BackgroundColor = colorbg;
            table.AddCell(cellFCO);

            PdfPCell cellcabe5 = new PdfPCell(new Phrase("", TextFont));
            cellcabe5.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabe5.Border = 0;
            table.AddCell(cellcabe5);

            //Cabecera 2

            PdfPCell cellcabeE = new PdfPCell(new Phrase("EMPRESA", TextFont));
            cellcabeE.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeE.BackgroundColor = colorbg;
            table.AddCell(cellcabeE);

            PdfPCell cellcabeCL = new PdfPCell(new Phrase("CLIENTE", TextFont));
            cellcabeCL.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeCL.BackgroundColor = colorbg;
            table.AddCell(cellcabeCL);

            PdfPCell cellcabeBA = new PdfPCell(new Phrase("BARRA", TextFont));
            cellcabeBA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeBA.BackgroundColor = colorbg;
            table.AddCell(cellcabeBA);

            PdfPCell cellcabeTU = new PdfPCell(new Phrase("TIPO USUARIO", TextFont));
            cellcabeTU.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeTU.BackgroundColor = colorbg;
            table.AddCell(cellcabeTU);

            PdfPCell cellcabeLI = new PdfPCell(new Phrase("LICITACIÓN", TextFont));
            cellcabeLI.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeLI.BackgroundColor = colorbg;
            table.AddCell(cellcabeLI);

            PdfPCell cellcabePP = new PdfPCell(new Phrase("PRECIO POTENCIA " + "\n" + "S/ /kW-mes", TextFont));
            cellcabePP.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePP.BackgroundColor = colorbg;
            table.AddCell(cellcabePP);

            PdfPCell cellcabePE = new PdfPCell(new Phrase("POTENCIA EGRESO " + "\n" + "KW", TextFont));
            cellcabePE.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePE.BackgroundColor = colorbg;
            table.AddCell(cellcabePE);

            PdfPCell cellcabePC = new PdfPCell(new Phrase("POTENCIA  " + "\n" + "CALCULADA (KW)", TextFont));
            cellcabePC.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePC.BackgroundColor = colorbg;
            table.AddCell(cellcabePC);

            PdfPCell cellcabePD = new PdfPCell(new Phrase("POTENCIA  " + "\n" + "DECLARADA (KW)", TextFont));
            cellcabePD.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePD.BackgroundColor = colorbg;
            table.AddCell(cellcabePD);

            PdfPCell cellcabePU = new PdfPCell(new Phrase("PEAJE UNITARIO  " + "\n" + "S/ /kW-mes", TextFont));
            cellcabePU.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePU.BackgroundColor = colorbg;
            table.AddCell(cellcabePU);

            PdfPCell cellcabeFCOBAR = new PdfPCell(new Phrase("FCO-BARRA ", TextFont));
            cellcabeFCOBAR.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeFCOBAR.BackgroundColor = colorbg;
            table.AddCell(cellcabeFCOBAR);

            PdfPCell cellcabeFCOPA = new PdfPCell(new Phrase("FCO-POTENCIA " + "\n" + "ACTIVA kW", TextFont));
            cellcabeFCOPA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeFCOPA.BackgroundColor = colorbg;
            table.AddCell(cellcabeFCOPA);

            PdfPCell cellcabeFCOPR = new PdfPCell(new Phrase("FCO-POTENCIA " + "\n" + "REACTIVA kW", TextFont));
            cellcabeFCOPR.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeFCOPR.BackgroundColor = colorbg;
            table.AddCell(cellcabeFCOPR);

            PdfPCell cellcabeCA = new PdfPCell(new Phrase("CALIDAD", TextFont));
            cellcabeCA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeCA.BackgroundColor = colorbg;
            table.AddCell(cellcabeCA);

            foreach (var item in ListaPeajeEgresoMinfo)
            {

                PdfPCell cellbodyE = new PdfPCell(new Phrase((item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty, NumberFont));
                cellbodyE.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyE);

                PdfPCell cellbodyCL = new PdfPCell(new Phrase((item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty, NumberFont));
                cellbodyCL.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyCL);

                PdfPCell cellbodyBA = new PdfPCell(new Phrase((item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty, NumberFont));
                cellbodyBA.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyBA);

                PdfPCell cellbodyTU = new PdfPCell(new Phrase((item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty, NumberFont));
                cellbodyTU.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyTU);

                PdfPCell cellbodyLI = new PdfPCell(new Phrase((item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty, NumberFont));
                cellbodyLI.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyLI);

                PdfPCell cellbodyPP = new PdfPCell(new Phrase(((item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPP.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPP);

                PdfPCell cellbodyPE = new PdfPCell(new Phrase(((item.Pegrmipoteegreso != null) ? item.Pegrmipoteegreso : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPE.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPE);

                PdfPCell cellbodyPC = new PdfPCell(new Phrase(((item.Pegrmipotecalculada != null) ? item.Pegrmipotecalculada : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPC.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPC);

                PdfPCell cellbodyPD = new PdfPCell(new Phrase(((item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPD.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPD);

                PdfPCell cellbodyPU = new PdfPCell(new Phrase(((item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPU.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPU);

                PdfPCell cellbodyFCOBAR = new PdfPCell(new Phrase((item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty, NumberFont));
                cellbodyFCOBAR.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyFCOBAR);

                PdfPCell cellbodyFCOPA = new PdfPCell(new Phrase(((item.Pegrmipoteactiva != null) ? item.Pegrmipoteactiva : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyFCOPA.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyFCOPA);

                PdfPCell cellbodyFCOPR = new PdfPCell(new Phrase(((item.Pegrmipotereactiva != null) ? item.Pegrmipotereactiva : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyFCOPR.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyFCOPR);

                PdfPCell cellbodyCA = new PdfPCell(new Phrase((item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty, NumberFont));
                cellbodyCA.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyCA);
            }

            pdfDoc.Add(table);

            pdfDoc.Close();
        }

        /// <summary>
        /// CU09 - Permite generar el archivo de exportación de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="entidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="listaPeajeEgresoMinfo">Lista de registros de VtpIngresoPotefrDetalleDTO</param>
        /// <param name="pathLogo">Ruta del logo del COES</param>
        /// <returns></returns>
        public static void GenerarFormatoPeajeEgresoMinfoNuevo(string fileName, VtpRecalculoPotenciaDTO entidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            Chunk c1 = new Chunk("INFORMACIÓN INGRESADA PARA VTP Y PEAJES" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(entidadRecalculoPotencia.Perinombre + "/" + entidadRecalculoPotencia.Recpotnombre, FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph(p1);

            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE);

            //float[] widths = new float[] { 29, 23, 16, 16, 16 };
            PdfPTable table = new PdfPTable(12);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.HeaderRows = 2;
            //Formato  de Texto y numero
            var TextFont = FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var NumberFont = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Background cell
            BaseColor colorbg = new BaseColor(36, 92, 134);


            //Cabecera 2
            PdfPCell cellcabeC = new PdfPCell(new Phrase("CÓDIGO", TextFont));
            cellcabeC.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeC.BackgroundColor = colorbg;
            table.AddCell(cellcabeC);

            PdfPCell cellcabeE = new PdfPCell(new Phrase("EMPRESA", TextFont));
            cellcabeE.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeE.BackgroundColor = colorbg;
            table.AddCell(cellcabeE);

            PdfPCell cellcabeCL = new PdfPCell(new Phrase("CLIENTE", TextFont));
            cellcabeCL.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeCL.BackgroundColor = colorbg;
            table.AddCell(cellcabeCL);

            PdfPCell cellcabeBA = new PdfPCell(new Phrase("BARRA", TextFont));
            cellcabeBA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeBA.BackgroundColor = colorbg;
            table.AddCell(cellcabeBA);

            PdfPCell cellcabeLI = new PdfPCell(new Phrase("LICITACIÓN", TextFont));
            cellcabeLI.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeLI.BackgroundColor = colorbg;
            table.AddCell(cellcabeLI);

            PdfPCell cellcabeTU = new PdfPCell(new Phrase("TIPO USUARIO", TextFont));
            cellcabeTU.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeTU.BackgroundColor = colorbg;
            table.AddCell(cellcabeTU);

            PdfPCell cellcabePP = new PdfPCell(new Phrase("PRECIO POTENCIA " + "\n" + "S/ /kW-mes", TextFont));
            cellcabePP.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePP.BackgroundColor = colorbg;
            table.AddCell(cellcabePP);

            PdfPCell cellcabePE = new PdfPCell(new Phrase("POTENCIA COINCIDENTE " + "\n" + "KW", TextFont));
            cellcabePE.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePE.BackgroundColor = colorbg;
            table.AddCell(cellcabePE);

            PdfPCell cellcabePD = new PdfPCell(new Phrase("POTENCIA  " + "\n" + "DECLARADA (KW)", TextFont));
            cellcabePD.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePD.BackgroundColor = colorbg;
            table.AddCell(cellcabePD);

            PdfPCell cellcabePU = new PdfPCell(new Phrase("PEAJE UNITARIO  " + "\n" + "S/ /kW-mes", TextFont));
            cellcabePU.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabePU.BackgroundColor = colorbg;
            table.AddCell(cellcabePU);

            PdfPCell cellcabeFP = new PdfPCell(new Phrase("FACTOR " + "\n" + "PERDIDA", TextFont));
            cellcabeFP.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeFP.BackgroundColor = colorbg;
            table.AddCell(cellcabeFP);

            PdfPCell cellcabeCA = new PdfPCell(new Phrase("CALIDAD", TextFont));
            cellcabeCA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcabeCA.BackgroundColor = colorbg;
            table.AddCell(cellcabeCA);

            foreach (var item in listaPeajeEgresoMinfo)
            {
                PdfPCell cellbodyC = new PdfPCell(new Phrase((item.Coregecodvtp != null) ? item.Coregecodvtp.ToString() : string.Empty, NumberFont));
                cellbodyC.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyC);

                PdfPCell cellbodyE = new PdfPCell(new Phrase((item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty, NumberFont));
                cellbodyE.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyE);

                PdfPCell cellbodyCL = new PdfPCell(new Phrase((item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty, NumberFont));
                cellbodyCL.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyCL);

                PdfPCell cellbodyBA = new PdfPCell(new Phrase((item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty, NumberFont));
                cellbodyBA.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyBA);

                PdfPCell cellbodyLI = new PdfPCell(new Phrase((item.Coregecodvtp != null) ? (item.Tipconnombre != null) ? item.Tipconnombre.ToString() : string.Empty : (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty, NumberFont));
                cellbodyLI.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyLI);

                PdfPCell cellbodyTU = new PdfPCell(new Phrase((item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty, NumberFont));
                cellbodyTU.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyTU);

                PdfPCell cellbodyPP = new PdfPCell(new Phrase(((item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPP.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPP);


                decimal? potencia = (item.Pegrdpotecoincidente == null) ? item.Pegrmipoteegreso : item.Pegrdpotecoincidente;
                PdfPCell cellbodyPC = new PdfPCell(new Phrase(((potencia != null) ? potencia : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPC.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPC);

                PdfPCell cellbodyPD = new PdfPCell(new Phrase(((item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPD.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPD);

                PdfPCell cellbodyPU = new PdfPCell(new Phrase(((item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyPU.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyPU);

                PdfPCell cellbodyFP = new PdfPCell(new Phrase(((item.Pegrdfacperdida != null) ? item.Pegrdfacperdida : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                cellbodyFP.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyFP);

                PdfPCell cellbodyCA = new PdfPCell(new Phrase((item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty, NumberFont));
                cellbodyCA.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellbodyCA);
            }

            pdfDoc.Add(table);

            pdfDoc.Close();
        }


        /// <summary>
        /// CU17 - Permite generar el archivo de exportación de la vista VTP_RETIRO_POTESC
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaRetiroPotenciaSC">Lista de registros de VtpRetiroPotescDTO</param>
        /// <returns></returns>
        internal static void GenerarReporteRetiroSC(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            Chunk c1 = new Chunk("LISTA DE RETIROS DE POTENCIA SIN CONTRATO" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + "/" + EntidadRecalculoPotencia.Recpotnombre, FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph(p1);

            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            Chunk sLinea1 = new Chunk("Se ha cargado la siguiente información:", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase pLinea1 = new Phrase(sLinea1);
            pdfDoc.Add(pLinea1);
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            float[] widths = new float[] { 29, 23, 16, 16, 16 };
            PdfPTable table = new PdfPTable(widths);
            table.WidthPercentage = 80;
            table.HorizontalAlignment = 0;

            //Formato  de Texto y numero
            var TextFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var NumberFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Background cell
            BaseColor colorbg = new BaseColor(36, 92, 134);

            PdfPCell cellcliente = new PdfPCell(new Phrase("CLIENTE", TextFont));
            cellcliente.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcliente.BackgroundColor = colorbg;
            table.AddCell(cellcliente);

            PdfPCell cellbarra = new PdfPCell(new Phrase("BARRA", TextFont));
            cellbarra.HorizontalAlignment = Element.ALIGN_CENTER;
            cellbarra.BackgroundColor = colorbg;
            table.AddCell(cellbarra);

            PdfPCell cellpp = new PdfPCell(new Phrase("PRECIO POTENCIA PPB ctm. S/KW-mes", TextFont));
            cellpp.HorizontalAlignment = Element.ALIGN_CENTER;
            cellpp.BackgroundColor = colorbg;
            table.AddCell(cellpp);

            PdfPCell cellpc = new PdfPCell(new Phrase("POTENCIA CONSUMIDA kW", TextFont));
            cellpc.HorizontalAlignment = Element.ALIGN_CENTER;
            cellpc.BackgroundColor = colorbg;
            table.AddCell(cellpc);

            PdfPCell cellva = new PdfPCell(new Phrase("VALORIZACIÓN S/", TextFont));
            cellva.HorizontalAlignment = Element.ALIGN_CENTER;
            cellva.BackgroundColor = colorbg;
            table.AddCell(cellva);

            decimal dTotalRpscpoteegreso = 0;
            decimal dTotalValorizacion = 0;

            foreach (var item in ListaRetiroPotenciaSC)
            {
                PdfPCell cliente = new PdfPCell(new Phrase(item.Emprnomb, NumberFont));
                cliente.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cliente);

                PdfPCell barra = new PdfPCell(new Phrase(item.Barrnombre, NumberFont));
                barra.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(barra);

                PdfPCell ppotencia = new PdfPCell(new Phrase(((item.Rpscprecioppb != null) ? item.Rpscprecioppb : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                ppotencia.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(ppotencia);
                //  table.AddCell(((item.Rpscpreciopote != null) ? item.Rpscpreciopote : Decimal.Zero).ToString()); //PPB

                PdfPCell pconsumida = new PdfPCell(new Phrase(((item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero).Value.ToString(formatNumber), NumberFont));
                pconsumida.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(pconsumida);

                //table.AddCell(((item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero).ToString());

                PdfPCell pvalorizacion = new PdfPCell(new Phrase((item.Rpscprecioppb * item.Rpscpoteegreso).Value.ToString(formatNumber), NumberFont));
                pvalorizacion.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(pvalorizacion);
                // table.AddCell((item.Rpscpreciopote * item.Rpscpoteegreso).ToString());

                dTotalRpscpoteegreso += Convert.ToDecimal(item.Rpscpoteegreso);
                dTotalValorizacion += Convert.ToDecimal(item.Rpscprecioppb) * Convert.ToDecimal(item.Rpscpoteegreso);
            }

            if (dTotalValorizacion > 0)
            {
                PdfPCell ptota0 = new PdfPCell(new Phrase("", TextFont));
                ptota0.Border = 0;
                table.AddCell(ptota0);

                PdfPCell ptotal = new PdfPCell(new Phrase("TOTAL", TextFont));
                ptotal.BackgroundColor = colorbg;
                ptotal.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(ptotal);

                PdfPCell ptotal2 = new PdfPCell(new Phrase("", TextFont));
                ptotal2.BackgroundColor = colorbg;
                ptotal2.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(ptotal2); //PPB

                PdfPCell pTotalRpscpoteegreso = new PdfPCell(new Phrase(dTotalRpscpoteegreso.ToString(formatNumber), NumberFont));
                pTotalRpscpoteegreso.HorizontalAlignment = Element.ALIGN_RIGHT;
                pTotalRpscpoteegreso.BackgroundColor = colorbg;
                table.AddCell(pTotalRpscpoteegreso);
                // table.AddCell(dTotalRpscpoteegreso.ToString());
                PdfPCell ptotalvalorizacion = new PdfPCell(new Phrase(dTotalValorizacion.ToString(formatNumber), NumberFont));
                ptotalvalorizacion.HorizontalAlignment = Element.ALIGN_RIGHT;
                ptotalvalorizacion.BackgroundColor = colorbg;
                table.AddCell(ptotalvalorizacion);
                //table.AddCell(dTotalValorizacion.ToString());                }
            }
            pdfDoc.Add(table);

            //Lista de empresas con FactorProporcion
            decimal dFactorProporcion = 0;
            //Listas complementarias
            List<VtpRetiroPotescDTO> ListaRetiroSC = (new TransfPotenciaAppServicio()).ListVtpRetiroPotenciaSCByEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            List<IngresoRetiroSCDTO> ListaFactorProporcion = (new IngresoRetiroSCAppServicio()).BuscarIngresoRetiroSC(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recacodi);

            pdfDoc.Add(Chunk.NEWLINE);
            pdfDoc.Add(Chunk.NEWLINE);
            pdfDoc.Add(Chunk.NEWLINE);

            int TotalColumnas = ListaFactorProporcion.Count + 2;
            PdfPTable table2 = new PdfPTable(TotalColumnas);
            table2.WidthPercentage = 100;

            //Cabecera
            PdfPCell cellCabeHidden = new PdfPCell(new Phrase("", TextFont));
            cellCabeHidden.Border = 0;
            table2.AddCell(cellCabeHidden);

            foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
            {
                if (dtoFactoProporcion.EmprNombre == null) dtoFactoProporcion.EmprNombre = "No existe empresa";
                PdfPCell cellCabe = new PdfPCell(new Phrase(dtoFactoProporcion.EmprNombre.ToString(), TextFont));
                cellCabe.BackgroundColor = colorbg;
                cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cellCabe);
                //   table2.AddCell(dtoFactoProporcion.EmprNombre.ToString());                  
            }
            PdfPCell cellCabetotal = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellCabetotal.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabetotal.BackgroundColor = colorbg;
            table2.AddCell(cellCabetotal);

            //Filas
            int iColumna = 3;
            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            //Por cada Factor de Proporción
            foreach (VtpRetiroPotescDTO dtoRetiroPoteSC in ListaRetiroPotenciaSC)
            {
                PdfPCell cell11 = new PdfPCell(new Phrase(dtoRetiroPoteSC.Emprnomb.ToString(), TextFont));
                cell11.BackgroundColor = colorbg;
                table2.AddCell(cell11);

                decimal dTotalRow = 0;
                iColumna = 3;
                foreach (IngresoRetiroSCDTO dtoFactoProporcion in ListaFactorProporcion)
                {
                    dFactorProporcion = dtoFactoProporcion.IngrscImporteVtp;
                    PdfPCell cell1 = new PdfPCell(new Phrase((dFactorProporcion * dTotalValorizacion).ToString(formatNumber), NumberFont));
                    cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(cell1);  //Pinta el total por Fila                        
                    dTotalRow += dFactorProporcion * Convert.ToDecimal(dTotalValorizacion);
                    dTotalColum[iColumna] += dFactorProporcion * Convert.ToDecimal(dTotalValorizacion);
                    iColumna++;
                }
                PdfPCell cell2 = new PdfPCell(new Phrase(dTotalRow.ToString(formatNumber), NumberFont));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cell2);  //Pinta el total por Fila
                dTotalColum[iColumna] += dTotalRow;
            }

            iColumna = 3;

            PdfPCell cellColumnTotal = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellColumnTotal.HorizontalAlignment = Element.ALIGN_CENTER;
            cellColumnTotal.BackgroundColor = colorbg;
            table2.AddCell(cellColumnTotal);

            for (int i = 0; i <= ListaFactorProporcion.Count(); i++)
            {
                PdfPCell cellTotal = new PdfPCell(new Phrase(dTotalColum[iColumna].ToString(formatNumber), NumberFont));
                cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cellTotal);
                // table2.AddCell( dTotalColum[iColumna].ToString());
                iColumna++;
            }

            pdfDoc.Add(table2);

            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Peajes a pagarse - CU18
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeEmpresaPago">Lista de registros de VtpPeajeEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReportePeajePagarse(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            Chunk c1 = new Chunk("COMPENSACIÓN POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN Y PEAJE DE TRANSMISIÓN DEL SISTEMA GARANTIZADO DE TRANSMISIÓN" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 11));
            Chunk c3 = new Chunk("A) COMPENSACIÓN A TRANSMISORAS" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Chunk c4 = new Chunk("PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            Phrase p2 = new Phrase();
            p2.Add(c2);
            p2.Add(c3);
            Phrase p3 = new Phrase();
            p3.Add(c4);
            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            Paragraph pSubTitulo = new Paragraph();
            pSubTitulo.Add(p2);
            pSubTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pSubTitulo);
            Paragraph pSubTitulo2 = new Paragraph();
            pSubTitulo2.Add(p3);
            pSubTitulo2.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pSubTitulo2);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var NumberFont = FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Background cell   
            BaseColor colorbg = new BaseColor(36, 92, 134);

            /////Declarando cantidad de columnas de tabla ///////////
            int iEmprcodiPagoContador = ListaPeajeEmpresaPago[0].Emprcodipeaje;
            int iNumEmpresaCobro = 0;
            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresa = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPagoContador, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            iNumEmpresaCobro = ListaPeajeEmpresa.Count();
            PdfPTable table = new PdfPTable(iNumEmpresaCobro + 2);
            //table.WidthPercentage = 100;
            ////////////////////////////
            table.HeaderRows = 1;

            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            int colum = 3;

            //CABCERA
            for (int i = 0; i < ListaPeajeEmpresaPago.Count(); i++)
            {
                if (i == 0)
                {
                    int iEmprcodiPago = ListaPeajeEmpresaPago[0].Emprcodipeaje;
                    List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                    PdfPCell cellCabe = new PdfPCell(new Phrase("EMPRESA", TextFont));
                    cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabe.BackgroundColor = colorbg;
                    table.AddCell(cellCabe);

                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                    {
                        string cabecera = "";
                        cabecera = (dtoEmpresaCobro.Emprnombcargo != null) ? dtoEmpresaCobro.Emprnombcargo.ToString().Trim() + "\n" : string.Empty + "\n";
                        int iPingcodi = dtoEmpresaCobro.Pingcodi;
                        VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                        if (dtoPeajeIngreso != null)
                        {
                            cabecera += dtoPeajeIngreso.Pingnombre + "\n";
                            cabecera += dtoPeajeIngreso.Pingtipo;
                        }
                        PdfPCell cellCabe2 = new PdfPCell(new Phrase(cabecera, TextFont));
                        cellCabe2.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellCabe2.BackgroundColor = colorbg;
                        table.AddCell(cellCabe2);

                        dTotalColum[colum] = 0; //Inicializando los valores
                        colum++;
                    }
                    PdfPCell cellCabeTotal = new PdfPCell(new Phrase("TOTAL", TextFont));
                    cellCabeTotal.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabeTotal.BackgroundColor = colorbg;
                    table.AddCell(cellCabeTotal);

                    dTotalColum[colum] = 0;
                }
                break;
            }

            foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
            {
                string empr = (dtoEmpresaPago.Emprnombpeaje != null) ? dtoEmpresaPago.Emprnombpeaje.ToString().Trim() : string.Empty;

                PdfPCell cellEmpr = new PdfPCell(new Phrase(empr, TextFont));
                cellEmpr.HorizontalAlignment = Element.ALIGN_CENTER;
                cellEmpr.BackgroundColor = colorbg;
                table.AddCell(cellEmpr);

                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                colum = 3;
                decimal dTotalRow = 0;
                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                {
                    PdfPCell cellEmpr2 = new PdfPCell(new Phrase(dtoEmpresaCobro.Pempagpeajepago.ToString(formatNumber), NumberFont));
                    cellEmpr2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellEmpr2);

                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago);
                    dTotalColum[colum] += Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago);
                    colum++;
                }

                PdfPCell cellEmprTotal = new PdfPCell(new Phrase(dTotalRow.ToString(formatNumber), NumberFont));
                cellEmprTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellEmprTotal);

                dTotalColum[colum] += dTotalRow;
            }

            PdfPCell cellFoter = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellFoter.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFoter.BackgroundColor = colorbg;

            table.AddCell(cellFoter);

            colum = 3;
            for (int i = 0; i <= iNumEmpresaCobro; i++)
            {
                PdfPCell cellFoter2 = new PdfPCell(new Phrase(dTotalColum[colum].ToString(formatNumber), TextFont));
                cellFoter2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellFoter2.BackgroundColor = colorbg;
                table.AddCell(cellFoter2);
                colum++;
            }
            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Ingresos Tarifarios - CU19
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoTarifarioPago">Lista de registros de VtpIngresoTarifarioDTO</param>
        /// <returns></returns>
        public static void GenerarReporteIngresoTarifario(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("COMPENSACIÓN POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN Y PEAJE DE TRANSMISIÓN DEL SISTEMA GARANTIZADO DE TRANSMISIÓN" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 11));
            Chunk c3 = new Chunk("PEAJE POR  CONEXIÓN Y TRANSMISIÓN QUE CORRESPONDE PAGAR (" + ConstantesTransfPotencia.MensajeSoles + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            Phrase p2 = new Phrase();
            p2.Add(c2);
            p2.Add(c3);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            Paragraph pSubTitulo = new Paragraph();
            pSubTitulo.Add(p2);
            pSubTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pSubTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";

            //Declarar tabla
            int iEmprcodiPagoContador = ListaIngresoTarifarioPago[0].Emprcodingpot;
            List<VtpIngresoTarifarioDTO> ListaEmpresasCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPagoContador, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            int iNumEmpresasCobro = ListaEmpresasCobro.Count();
            PdfPTable table = new PdfPTable(iNumEmpresasCobro + 2);
            table.HeaderRows = 1;
            //table.WidthPercentage = 100;
            ////////////////////////////             
            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            int colum = 3;

            for (int i = 0; i < ListaIngresoTarifarioPago.Count(); i++)
            {
                if (i == 0)
                {
                    int iEmprcodiPago = ListaIngresoTarifarioPago[0].Emprcodingpot;
                    List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                    PdfPCell cellCabe = new PdfPCell(new Phrase("EMPRESA", TextFont));
                    cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabe.BackgroundColor = colorbg;
                    table.AddCell(cellCabe);

                    foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                    {
                        string cabecera = "";
                        cabecera = (dtoIngresoTarifarioCobro.Emprnombping != null) ? dtoIngresoTarifarioCobro.Emprnombping.ToString().Trim() : string.Empty + "\n";
                        int iPingcodi = dtoIngresoTarifarioCobro.Pingcodi;
                        VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, iPingcodi);
                        if (dtoPeajeIngreso != null)
                        {
                            cabecera += dtoPeajeIngreso.Pingnombre + "\n";
                            cabecera += dtoPeajeIngreso.Pingtipo;
                        }
                        PdfPCell cellCabeNombre = new PdfPCell(new Phrase(cabecera, TextFont));
                        cellCabeNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellCabeNombre.BackgroundColor = colorbg;
                        table.AddCell(cellCabeNombre);
                        dTotalColum[colum] = 0; //Inicializando los valores
                        colum++;
                    }

                    PdfPCell cellCabeTotal = new PdfPCell(new Phrase("TOTAL", TextFont));
                    cellCabeTotal.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabeTotal.BackgroundColor = colorbg;
                    table.AddCell(cellCabeTotal);
                    dTotalColum[colum] = 0;
                }
                break;
            }

            foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
            {
                PdfPCell cellBody = new PdfPCell(new Phrase((dtoIngresoTarifarioPago.Emprnombingpot != null) ? dtoIngresoTarifarioPago.Emprnombingpot.ToString().Trim() : string.Empty, TextFont));
                cellBody.HorizontalAlignment = Element.ALIGN_LEFT;
                cellBody.BackgroundColor = colorbg;
                table.AddCell(cellBody);

                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                colum = 3;
                decimal dTotalRow = 0;
                foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                {
                    PdfPCell cellBodyDato = new PdfPCell(new Phrase(dtoIngresoTarifarioCobro.Ingtarimporte.ToString(formatNumber), NumberFont));
                    cellBodyDato.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellBodyDato);

                    dTotalRow += Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte);
                    dTotalColum[colum] += Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte);
                    colum++;
                }
                PdfPCell cellBodyTotal = new PdfPCell(new Phrase(dTotalRow.ToString(formatNumber), NumberFont));
                cellBodyTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBodyTotal);

                dTotalColum[colum] += dTotalRow;
                //Border por celda en la Fila                           
            }

            PdfPCell cellFooter = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFooter.BackgroundColor = colorbg;
            table.AddCell(cellFooter);

            colum = 3;
            for (int i = 0; i <= iNumEmpresasCobro; i++)
            {
                PdfPCell cellFooterDato = new PdfPCell(new Phrase(dTotalColum[colum].ToString(formatNumber), NumberFont));
                cellFooterDato.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellFooterDato);
                colum++;
            }
            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Peajes Recaudados - CU20
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaPeajeCargoEmpresa">Lista de registros de VtpPeajeCargoDTO</param>
        /// <returns></returns>
        public static void GenerarReportePeajeRecaudado(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeCargoDTO> ListaPeajeCargoEmpresa, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("COMPENSACIÓN POR  PEAJE  DE  CONEXIÓN AL SISTEMA PRINCIPAL DE TRANSMISIÓN Y PEAJE DE TRANSMISIÓN DEL SISTEMA GARANTIZADO DE TRANSMISIÓN" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 14));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c3 = new Chunk("PEAJES RECAUDADOS (" + ConstantesTransfPotencia.MensajeSoles + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Phrase p1 = new Phrase();
            p1.Add(c1);
            Phrase p2 = new Phrase();
            p2.Add(c2);
            p2.Add(c3);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);
            Paragraph pSubTitulo = new Paragraph();
            pSubTitulo.Add(p2);
            pSubTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pSubTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";

            //Declarar tabla
            int iEmprcodiContador = ListaPeajeCargoEmpresa[0].Emprcodi;
            //Lista de cargos donde pago = no
            List<VtpPeajeCargoDTO> ListaPeajeCargoEmpr = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodiContador, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            int iNumEmpresaCargo = ListaPeajeCargoEmpr.Count();
            PdfPTable table = new PdfPTable(iNumEmpresaCargo + 2);
            table.HeaderRows = 1;
            //table.WidthPercentage = 100;
            ////////////////////////////         

            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            int colum = 3;

            for (int i = 0; i < ListaPeajeCargoEmpresa.Count(); i++)
            {
                if (i == 0)
                {
                    int iEmprcodi = ListaPeajeCargoEmpresa[0].Emprcodi;
                    //Lista de cargos donde pago = no
                    List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(iEmprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                    PdfPCell cellCabe = new PdfPCell(new Phrase("EMPRESA", TextFont));
                    cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabe.BackgroundColor = colorbg;
                    table.AddCell(cellCabe);

                    foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                    {
                        string cabecera = "";
                        cabecera = (dtoCargo.Pingnombre != null) ? dtoCargo.Pingnombre.ToString().Trim() : string.Empty + "\n";

                        PdfPCell cellCabeNombre = new PdfPCell(new Phrase(cabecera, TextFont));
                        cellCabeNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellCabeNombre.BackgroundColor = colorbg;
                        table.AddCell(cellCabeNombre);

                        dTotalColum[colum] = 0; //Inicializando los valores
                        colum++;
                    }

                    PdfPCell cellCabeTotal = new PdfPCell(new Phrase("TOTAL", TextFont));
                    cellCabeTotal.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabeTotal.BackgroundColor = colorbg;
                    table.AddCell(cellCabeTotal);

                    dTotalColum[colum] = 0;
                }
                break;
            }

            foreach (VtpPeajeCargoDTO dtoCargoEmpresa in ListaPeajeCargoEmpresa)
            {

                PdfPCell cellBody = new PdfPCell(new Phrase((dtoCargoEmpresa.Emprnomb != null) ? dtoCargoEmpresa.Emprnomb.ToString().Trim() : string.Empty, TextFont));
                cellBody.HorizontalAlignment = Element.ALIGN_LEFT;
                cellBody.BackgroundColor = colorbg;
                table.AddCell(cellBody);

                List<VtpPeajeCargoDTO> ListaPeajeCargo = (new TransfPotenciaAppServicio()).ListVtpPeajeCargoPagoNo(dtoCargoEmpresa.Emprcodi, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                colum = 3;
                decimal dTotalRow = 0;
                foreach (VtpPeajeCargoDTO dtoCargo in ListaPeajeCargo)
                {
                    PdfPCell cellBodyDato = new PdfPCell(new Phrase(dtoCargo.Pecarpeajerecaudado.ToString(formatNumber), NumberFont));
                    cellBodyDato.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellBodyDato);

                    dTotalRow += Convert.ToDecimal(dtoCargo.Pecarpeajerecaudado);
                    dTotalColum[colum] += Convert.ToDecimal(dtoCargo.Pecarpeajerecaudado);
                    colum++;
                }

                PdfPCell cellBodyTotal = new PdfPCell(new Phrase(dTotalRow.ToString(formatNumber), NumberFont));
                cellBodyTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBodyTotal);
                dTotalColum[colum] += dTotalRow;//Pinta el total por Fila
                //Border por celda en la Fila                           
            }

            PdfPCell cellFooter = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFooter.BackgroundColor = colorbg;
            table.AddCell(cellFooter);
            colum = 3;
            for (int i = 0; i <= iNumEmpresaCargo; i++)
            {
                PdfPCell cellFooterDato = new PdfPCell(new Phrase(dTotalColum[colum].ToString(formatNumber), NumberFont));
                cellFooterDato.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellFooterDato);
                colum++;
            }
            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }

        public static void GenerarReportePotenciaValor(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("RESUMEN DE RETIROS DE POTENCIA POR GENERADOR" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));

            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Declarar tabla

            PdfPTable table = new PdfPTable(9);
            table.WidthPercentage = 100;
            table.HeaderRows = 2;
            ////////////////////////////         
            //decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            //int colum = 3;

            decimal dTotalPotenciaBilateral = 0;
            decimal dTotalPotenciaLicitacion = 0;
            decimal dTotalPotenciaSinContrato = 0;
            decimal dTotalPotencia = 0;
            decimal dTotalValorizacionBilateral = 0;
            decimal dTotalValorizacionLicitacion = 0;
            decimal dTotalValorizacionSinContrato = 0;
            decimal dTotalValorizacion = 0;

            PdfPCell cellCabe = new PdfPCell(new Phrase(" " + "\n" + " " + "\n" + " ", TextFont));
            cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe.Border = 0;
            table.AddCell(cellCabe);

            PdfPCell cellCabe2 = new PdfPCell(new Phrase("" + "\n" + "POTENCIA CONSUMIDA (kW)" + "\n" + " ", TextFont));
            cellCabe2.Colspan = 4;
            cellCabe2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe2.BackgroundColor = colorbg;
            table.AddCell(cellCabe2);

            PdfPCell cellCabe3 = new PdfPCell(new Phrase("" + "\n" + "VALORIZACIÓ DE CONSUMOS (S/)" + "\n" + " ", TextFont));
            cellCabe3.Colspan = 4;
            cellCabe3.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe3.BackgroundColor = colorbg;
            table.AddCell(cellCabe3);

            PdfPCell cellCabe11 = new PdfPCell(new Phrase("\n" + "EMPRESA" + "\n", TextFont));
            cellCabe11.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe11.BackgroundColor = colorbg;
            table.AddCell(cellCabe11);

            PdfPCell cellCabe12 = new PdfPCell(new Phrase("CLIENTES" + "\n" + "CONTRATO" + "\n" + "BILATERAL", TextFont));
            cellCabe12.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe12.BackgroundColor = colorbg;
            table.AddCell(cellCabe12);

            PdfPCell cellCabe13 = new PdfPCell(new Phrase("CLIENTES" + "\n" + "CONTRATO" + "\n" + "LICITACIONES", TextFont));
            cellCabe13.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe13.BackgroundColor = colorbg;
            table.AddCell(cellCabe13);

            PdfPCell cellCabe14 = new PdfPCell(new Phrase("" + "\n" + "SIN CONTRATO" + "\n" + "", TextFont));
            cellCabe14.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe14.BackgroundColor = colorbg;
            table.AddCell(cellCabe14);

            PdfPCell cellCabe15 = new PdfPCell(new Phrase("" + "\n" + "TOTAL" + "\n" + "", TextFont));
            cellCabe15.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe15.BackgroundColor = colorbg;
            table.AddCell(cellCabe15);

            PdfPCell cellCabe16 = new PdfPCell(new Phrase("CLIENTES" + "\n" + "CONTRATO" + "\n" + "BILATERAL", TextFont));
            cellCabe16.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe16.BackgroundColor = colorbg;
            table.AddCell(cellCabe16);

            PdfPCell cellCabe17 = new PdfPCell(new Phrase("CLIENTES" + "\n" + "CONTRATO" + "\n" + "LICITACIONES", TextFont));
            cellCabe17.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe17.BackgroundColor = colorbg;
            table.AddCell(cellCabe17);

            PdfPCell cellCabe18 = new PdfPCell(new Phrase("" + "\n" + "SIN CONTRATO" + "\n" + "", TextFont));
            cellCabe18.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe18.BackgroundColor = colorbg;
            table.AddCell(cellCabe18);

            PdfPCell cellCabe19 = new PdfPCell(new Phrase("" + "\n" + "TOTAL" + "\n" + "", TextFont));
            cellCabe19.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe19.BackgroundColor = colorbg;
            table.AddCell(cellCabe19);

            foreach (var item in ListaPeajeEgreso)
            {
                PdfPCell cellBody1 = new PdfPCell(new Phrase(item.Genemprnombre.ToString(), NumberFont));
                cellBody1.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellBody1);

                PdfPCell cellBody2 = new PdfPCell(new Phrase(item.Pegrmipotecalculada.Value.ToString(formatNumber), NumberFont));
                cellBody2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody2);
                dTotalPotenciaBilateral += Convert.ToDecimal(item.Pegrmipotecalculada);

                PdfPCell cellBody3 = new PdfPCell(new Phrase(item.Pegrmipotedeclarada.Value.ToString(formatNumber), NumberFont));
                cellBody3.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody3);
                dTotalPotenciaLicitacion += Convert.ToDecimal(item.Pegrmipotedeclarada);

                PdfPCell cellBody4 = new PdfPCell(new Phrase(item.Pegrmipreciopote.Value.ToString(formatNumber), NumberFont));
                cellBody4.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody4);
                dTotalPotenciaSinContrato += Convert.ToDecimal(item.Pegrmipreciopote);

                PdfPCell cellBody5 = new PdfPCell(new Phrase((item.Pegrmipotecalculada + item.Pegrmipotedeclarada + item.Pegrmipreciopote).Value.ToString(formatNumber), NumberFont));
                cellBody5.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody5);
                dTotalPotencia += Convert.ToDecimal(item.Pegrmipotecalculada) + Convert.ToDecimal(item.Pegrmipotedeclarada) + Convert.ToDecimal(item.Pegrmipreciopote);

                PdfPCell cellBody6 = new PdfPCell(new Phrase(item.Pegrmipoteegreso.Value.ToString(formatNumber), NumberFont));
                cellBody6.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody6);
                dTotalValorizacionBilateral += Convert.ToDecimal(item.Pegrmipoteegreso);

                PdfPCell cellBody7 = new PdfPCell(new Phrase(item.Pegrmipeajeunitario.Value.ToString(formatNumber), NumberFont));
                cellBody7.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody7);
                dTotalValorizacionLicitacion += Convert.ToDecimal(item.Pegrmipeajeunitario);

                PdfPCell cellBody8 = new PdfPCell(new Phrase(item.Pegrmipoteactiva.Value.ToString(formatNumber), NumberFont));
                cellBody8.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody8);
                dTotalValorizacionSinContrato += Convert.ToDecimal(item.Pegrmipoteactiva);

                PdfPCell cellBody9 = new PdfPCell(new Phrase((item.Pegrmipoteegreso + item.Pegrmipeajeunitario + item.Pegrmipoteactiva).Value.ToString(formatNumber), NumberFont));
                cellBody9.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody9);
                dTotalValorizacion += Convert.ToDecimal(item.Pegrmipoteegreso) + Convert.ToDecimal(item.Pegrmipeajeunitario) + Convert.ToDecimal(item.Pegrmipoteactiva);
            }

            if (dTotalValorizacion > 0)
            {
                PdfPCell cellBodyTotal1 = new PdfPCell(new Phrase("TOTAL", TextFont));
                cellBodyTotal1.HorizontalAlignment = Element.ALIGN_CENTER;
                cellBodyTotal1.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal1);

                PdfPCell cellBodyTotal2 = new PdfPCell(new Phrase(dTotalPotenciaBilateral.ToString(formatNumber), TextFont));
                cellBodyTotal2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal2.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal2);

                PdfPCell cellBodyTotal3 = new PdfPCell(new Phrase(dTotalPotenciaLicitacion.ToString(formatNumber), TextFont));
                cellBodyTotal3.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal3.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal3);

                PdfPCell cellBodyTotal4 = new PdfPCell(new Phrase(dTotalPotenciaSinContrato.ToString(formatNumber), TextFont));
                cellBodyTotal4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal4.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal4);

                PdfPCell cellBodyTotal5 = new PdfPCell(new Phrase(dTotalPotencia.ToString(formatNumber), TextFont));
                cellBodyTotal5.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal5.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal5);

                PdfPCell cellBodyTotal6 = new PdfPCell(new Phrase(dTotalValorizacionBilateral.ToString(formatNumber), TextFont));
                cellBodyTotal6.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal6.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal6);

                PdfPCell cellBodyTotal7 = new PdfPCell(new Phrase(dTotalValorizacionLicitacion.ToString(formatNumber), TextFont));
                cellBodyTotal7.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal7.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal7);

                PdfPCell cellBodyTotal8 = new PdfPCell(new Phrase(dTotalValorizacionSinContrato.ToString(formatNumber), TextFont));
                cellBodyTotal8.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal8.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal8);

                PdfPCell cellBodyTotal9 = new PdfPCell(new Phrase(dTotalValorizacion.ToString(formatNumber), TextFont));
                cellBodyTotal9.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellBodyTotal9.BackgroundColor = colorbg;
                table.AddCell(cellBodyTotal9);
            }

            //DETALLE DE PERDIDA
            decimal dMaximaDemanda = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotmaxidemamensual);
            if (dMaximaDemanda == 0) dMaximaDemanda = 1;
            decimal dServicioAuxiliar = Convert.ToDecimal(EntidadRecalculoPotencia.Recpotpreciodemaservauxiliares);

            PdfPCell cellBodyDetalle = new PdfPCell(new Phrase("Máxima Demanda a nivel de generación:", NumberFont));
            cellBodyDetalle.Colspan = 4;
            cellBodyDetalle.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellBodyDetalle);

            PdfPCell cellBodyDetalle2 = new PdfPCell(new Phrase(dMaximaDemanda.ToString(formatNumber), NumberFont));
            cellBodyDetalle2.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle2);

            PdfPCell cellBodyDetalle3 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle3.Colspan = 4;
            cellBodyDetalle3.Border = 0;
            cellBodyDetalle3.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle3);

            PdfPCell cellBodyDetalle11 = new PdfPCell(new Phrase("Demanda de Servicios Auxiliares de centrales de generación:", NumberFont));
            cellBodyDetalle11.Colspan = 4;
            cellBodyDetalle11.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellBodyDetalle11);

            PdfPCell cellBodyDetalle12 = new PdfPCell(new Phrase(dServicioAuxiliar.ToString(formatNumber), NumberFont));
            cellBodyDetalle12.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle12);

            PdfPCell cellBodyDetalle13 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle13.Colspan = 4;
            cellBodyDetalle13.Border = 0;
            cellBodyDetalle13.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle13);

            PdfPCell cellBodyDetalle21 = new PdfPCell(new Phrase("Potencia consumida por demanda de carácter no regulada sin contratos:", NumberFont));
            cellBodyDetalle21.Colspan = 4;
            cellBodyDetalle21.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellBodyDetalle21);

            PdfPCell cellBodyDetalle22 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle22.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle22);

            PdfPCell cellBodyDetalle23 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle23.Colspan = 4;
            cellBodyDetalle23.Border = 0;
            cellBodyDetalle23.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle23);

            PdfPCell cellBodyDetalle31 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle31.Colspan = 4;
            cellBodyDetalle31.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellBodyDetalle31);

            PdfPCell cellBodyDetalle32 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle32.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle32);

            PdfPCell cellBodyDetalle33 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle33.Colspan = 4;
            cellBodyDetalle33.Border = 0;
            cellBodyDetalle33.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle33);

            PdfPCell cellBodyDetalle41 = new PdfPCell(new Phrase("Pérdidas (%) :", NumberFont));
            cellBodyDetalle41.Colspan = 4;
            cellBodyDetalle41.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellBodyDetalle41);

            decimal dPerdida = (dMaximaDemanda - dTotalPotencia - dServicioAuxiliar) * 100 / dMaximaDemanda;
            PdfPCell cellBodyDetalle42 = new PdfPCell(new Phrase((dPerdida).ToString(formatNumber), NumberFont));
            cellBodyDetalle42.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle42);

            PdfPCell cellBodyDetalle43 = new PdfPCell(new Phrase("", NumberFont));
            cellBodyDetalle43.Colspan = 4;
            cellBodyDetalle43.Border = 0;
            cellBodyDetalle43.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellBodyDetalle43);

            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Egresos - CU22
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaEmpresaEgreso">Lista de registros de VtpPeajeSaldoTransmisionDTO</param>
        /// <returns></returns>
        public static void GenerarReporteEgresos(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpPagoEgresoDTO> ListaPagoEgreso, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("EGRESO POR COMPRA DE POTENCIA POR GENERADOR INTEGRANTE" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));

            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Declarar tabla

            PdfPTable table = new PdfPTable(4);
            table.HeaderRows = 1;
            table.WidthPercentage = 100;

            PdfPCell cellCabe = new PdfPCell(new Phrase(" " + "\n" + "EMPRESA " + "\n" + " ", TextFont));
            cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe.BackgroundColor = colorbg;
            table.AddCell(cellCabe);

            PdfPCell cellCabe2 = new PdfPCell(new Phrase("" + "\n" + "EGRESO POR COMPRA POTENCIA (S/)" + "\n" + " ", TextFont));
            cellCabe2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe2.BackgroundColor = colorbg;
            table.AddCell(cellCabe2);

            PdfPCell cellCabe3 = new PdfPCell(new Phrase("" + "\n" + "SALDO POR PEAJE (S/)" + "\n" + " ", TextFont));
            cellCabe3.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe3.BackgroundColor = colorbg;
            table.AddCell(cellCabe3);

            PdfPCell cellCabe4 = new PdfPCell(new Phrase("" + "\n" + "EGRESO TOTAL POR COMPRA DE POTENCIA (S/)" + "\n", TextFont));
            cellCabe4.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe4.BackgroundColor = colorbg;
            table.AddCell(cellCabe4);

            ///////////////////////////////
            decimal dEgreso = 0;
            decimal dSaldo = 0;
            decimal dTotalEgreso = 0;

            foreach (VtpPagoEgresoDTO dtoEgreso in ListaPagoEgreso)
            {   //Los atributos: Pstrnstotalrecaudacion, Pstrnstotalpago, Pstrnssaldotransmision son empleado para leer la información de la BD

                PdfPCell cellBody = new PdfPCell(new Phrase(dtoEgreso.Emprnomb, NumberFont));
                cellBody.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cellBody);

                PdfPCell cellBody2 = new PdfPCell(new Phrase(dtoEgreso.Pagegregreso.ToString(formatNumber), NumberFont));
                cellBody2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody2);
                dEgreso += dtoEgreso.Pagegregreso;

                PdfPCell cellBody3 = new PdfPCell(new Phrase(dtoEgreso.Pagegrsaldo.ToString(formatNumber), NumberFont));
                cellBody3.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody3);
                dSaldo += dtoEgreso.Pagegrsaldo;

                PdfPCell cellBody4 = new PdfPCell(new Phrase(dtoEgreso.Pagegrpagoegreso.ToString(formatNumber), NumberFont));
                cellBody4.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBody4);
                dTotalEgreso += dtoEgreso.Pagegrpagoegreso;
            }

            if (dEgreso > 0)
            {
                PdfPCell cellFooter = new PdfPCell(new Phrase("TOTAL", TextFont));
                cellFooter.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFooter.BackgroundColor = colorbg;
                table.AddCell(cellFooter);

                PdfPCell cellFooter2 = new PdfPCell(new Phrase(dEgreso.ToString(formatNumber), TextFont));
                cellFooter2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellFooter2.BackgroundColor = colorbg;
                table.AddCell(cellFooter2);

                PdfPCell cellFooter3 = new PdfPCell(new Phrase(dSaldo.ToString(formatNumber), TextFont));
                cellFooter3.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellFooter3.BackgroundColor = colorbg;
                table.AddCell(cellFooter3);

                PdfPCell cellFooter4 = new PdfPCell(new Phrase(dTotalEgreso.ToString(formatNumber), TextFont));
                cellFooter4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellFooter4.BackgroundColor = colorbg;
                table.AddCell(cellFooter4);
            }
            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Ingresos por Potencia - CU23
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaIngresoPotenciaEmpresa">Lista de registros de VtpIngresoPotUnidPromdDTO</param>
        /// <param name="ListaIngresoPotEFR">Lista de registros de VtpIngresoPotefrDTO</param>
        /// <returns></returns>
        public static void GenerarReporteIngresoPotencia(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa, List<VtpIngresoPotefrDTO> ListaIngresoPotEFR, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("INGRESOS POR POTENCIA POR GENERADOR INTEGRANTE Y CENTRAL GENERACIÓN" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));

            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Declarar tabla
            int contador = ListaIngresoPotEFR.Count;
            PdfPTable table = new PdfPTable(contador + 3);
            table.HeaderRows = 1;
            table.WidthPercentage = 100;

            PdfPCell cellCabe = new PdfPCell(new Phrase("" + "\n" + "EMPRESA " + "\n" + "", TextFont));
            cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe.BackgroundColor = colorbg;
            table.AddCell(cellCabe);

            PdfPCell cellCabe2 = new PdfPCell(new Phrase("" + "\n" + "CENTRAL /  UNIDAD" + "\n" + "", TextFont));
            cellCabe2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe2.BackgroundColor = colorbg;
            table.AddCell(cellCabe2);

            foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
            {
                PdfPCell cellCabeN = new PdfPCell(new Phrase("" + "\n" + "Periodo " + dtoIngresoPotEFR.Ipefrintervalo + "\n" + dtoIngresoPotEFR.Ipefrdescripcion, TextFont));
                cellCabeN.HorizontalAlignment = Element.ALIGN_CENTER;
                cellCabeN.BackgroundColor = colorbg;
                table.AddCell(cellCabeN);
            }

            PdfPCell cellCabe3 = new PdfPCell(new Phrase("" + "\n" + "PROMEDIO" + "\n" + "", TextFont));
            cellCabe3.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe3.BackgroundColor = colorbg;
            table.AddCell(cellCabe3);
            //BODY
            foreach (VtpIngresoPotUnidPromdDTO dtoIngresoPotencia in ListaIngresoPotenciaEmpresa)
            {
                PdfPCell cellBody1 = new PdfPCell(new Phrase(dtoIngresoPotencia.Emprnomb, NumberFont));
                cellBody1.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellBody1);

                PdfPCell cellBody2 = new PdfPCell(new Phrase(dtoIngresoPotencia.Equinomb, NumberFont));
                cellBody2.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellBody2);

                foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                {
                    List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).GetByCriteriaVtpIngresoPotUnidIntervl(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, dtoIngresoPotencia.Emprcodi, dtoIngresoPotencia.Equicodi, dtoIngresoPotEFR.Ipefrcodi);
                    foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                    {
                        PdfPCell cellBodyN = new PdfPCell(new Phrase(dtoIngresoIntervalo.Inpuinimporte.ToString(formatNumber), NumberFont));
                        cellBodyN.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cellBodyN);
                        break;
                    }
                }
                PdfPCell cellBodyProm = new PdfPCell(new Phrase(dtoIngresoPotencia.Inpuprimportepromd.ToString(formatNumber), NumberFont));
                cellBodyProm.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellBodyProm);
            }

            pdfDoc.Add(table);
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //SEGUNDO REPORTE
            List<VtpIngresoPotenciaDTO> ListaVtpIngresoPotencia = (new TransfPotenciaAppServicio()).ListVtpIngresoPotenciaEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            //Declarar tabla

            PdfPTable table2 = new PdfPTable(contador + 2);
            table2.WidthPercentage = 100;
            table2.HeaderRows = 1;
            //Cabecera
            table2.AddCell(cellCabe);
            foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
            {
                PdfPCell cellCabeN = new PdfPCell(new Phrase("" + "\n" + "Periodo " + dtoIngresoPotEFR.Ipefrintervalo + "\n" + dtoIngresoPotEFR.Ipefrdescripcion + "\n" + " ", TextFont));
                cellCabeN.HorizontalAlignment = Element.ALIGN_CENTER;
                cellCabeN.BackgroundColor = colorbg;
                table2.AddCell(cellCabeN);
            }
            table2.AddCell(cellCabe3);
            //Cuerpo
            foreach (VtpIngresoPotenciaDTO dtoIngresoPotencia in ListaVtpIngresoPotencia)
            {
                PdfPCell cellBody2 = new PdfPCell(new Phrase(dtoIngresoPotencia.Emprnomb, NumberFont));
                cellBody2.HorizontalAlignment = Element.ALIGN_LEFT;
                table2.AddCell(cellBody2);

                foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in ListaIngresoPotEFR)
                {
                    List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad = (new TransfPotenciaAppServicio()).ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi, (int)dtoIngresoPotencia.Emprcodi, dtoIngresoPotEFR.Ipefrcodi);
                    foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoIntervalo in ListaIngresoPotenciaUnidad)
                    {
                        PdfPCell cellBodyN = new PdfPCell(new Phrase(dtoIngresoIntervalo.Inpuinimporte.ToString(formatNumber), NumberFont));
                        cellBodyN.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table2.AddCell(cellBodyN);
                        break;
                    }

                }
                PdfPCell cellBodyProm = new PdfPCell(new Phrase(dtoIngresoPotencia.Potipimporte.Value.ToString(formatNumber), NumberFont));
                cellBodyProm.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cellBodyProm);
            }
            pdfDoc.Add(table2);
            // step 5
            pdfDoc.Close();
        }

        /// <summary>
        /// Permite generar el Reporte de Saldos y Matriz de Pagos - CU24
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculoPotencia">Entidad de VtpRecalculoPotenciaDTO</param>
        /// <param name="ListaEmpresaPago">Lista de registros de VtpEmpresaPagoDTO</param>
        /// <returns></returns>
        public static void GenerarReporteMatriz(string fileName, VtpRecalculoPotenciaDTO EntidadRecalculoPotencia, List<VtpEmpresaPagoDTO> ListaEmpresaPago, string pathLogo)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50f, 50f);
            FileStream file = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

            pdfDoc.Open();

            //Agregar Logo
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.Alignment = Element.ALIGN_RIGHT;
            imgLogo.Alignment = iTextSharp.text.Image.PARAGRAPH;
            pdfDoc.Add(imgLogo);

            //titulo
            Chunk c1 = new Chunk("VALORIZACIÓN DE LAS TRANSFERENCIA DE POTENCIA" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            Chunk c2 = new Chunk(EntidadRecalculoPotencia.Perinombre + " - " + EntidadRecalculoPotencia.Recpotnombre + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 11));
            Chunk c3 = new Chunk("MATRIZ DE TRANSFERENCIAS" + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));
            Chunk c4 = new Chunk("Importes Expresados en Nuevos Soles (" + ConstantesTransfPotencia.MensajeSoles + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10));

            Phrase p1 = new Phrase();
            p1.Add(c1);
            p1.Add(c2);
            p1.Add(c3);
            p1.Add(c4);

            Paragraph pTitulo = new Paragraph();
            pTitulo.Add(p1);
            pTitulo.Alignment = Element.ALIGN_RIGHT;
            pdfDoc.Add(pTitulo);

            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
            pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

            //Formato  de Texto
            var TextFont = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            BaseColor colorbg = new BaseColor(36, 92, 134); //Background cell      
            var NumberFont = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string formatNumber = "#,##0.00";
            //Declarar tabla
            int iEmprcodiPagoContador = ListaEmpresaPago[0].Emprcodipago;
            List<VtpEmpresaPagoDTO> ListaEmpresa = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(iEmprcodiPagoContador, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
            int iNumEmpresasCobro = ListaEmpresa.Count();
            PdfPTable table = new PdfPTable(iNumEmpresasCobro + 2);
            table.HeaderRows = 1;
            table.WidthPercentage = 100;

            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            int colum = 3;

            PdfPCell cellCabe = new PdfPCell(new Phrase("" + "\n" + "EMPRESA " + "\n" + " ", TextFont));
            cellCabe.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCabe.BackgroundColor = colorbg;
            table.AddCell(cellCabe);

            for (int i = 0; i < ListaEmpresaPago.Count(); i++)
            {
                if (i == 0)
                {
                    int iEmprcodiPago = ListaEmpresaPago[0].Emprcodipago;
                    List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                    foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                    {
                        PdfPCell cellCabeN = new PdfPCell(new Phrase("" + "\n" + ((dtoEmpresaCobro.Emprnombcobro != null) ? dtoEmpresaCobro.Emprnombcobro.ToString().Trim() : string.Empty), TextFont));
                        cellCabeN.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellCabeN.BackgroundColor = colorbg;
                        table.AddCell(cellCabeN);
                        dTotalColum[colum] = 0; //Inicializando los valores
                        colum++;
                    }
                    PdfPCell cellCabe3 = new PdfPCell(new Phrase("" + "\n" + "TOTAL", TextFont));
                    cellCabe3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellCabe3.BackgroundColor = colorbg;
                    table.AddCell(cellCabe3);
                    dTotalColum[colum] = 0;
                }
                break;
            }
            foreach (VtpEmpresaPagoDTO dtoEmpresaPago in ListaEmpresaPago)
            {
                PdfPCell cellbody = new PdfPCell(new Phrase(((dtoEmpresaPago.Emprnombpago != null) ? dtoEmpresaPago.Emprnombpago.ToString().Trim() : string.Empty), TextFont));
                cellbody.HorizontalAlignment = Element.ALIGN_LEFT;
                cellbody.BackgroundColor = colorbg;
                table.AddCell(cellbody);
                List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(dtoEmpresaPago.Emprcodipago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                colum = 3;
                decimal dTotalRow = 0;
                foreach (VtpEmpresaPagoDTO dtoEmpresaCobro in ListaEmpresaCobro)
                {
                    PdfPCell cellbodyN = new PdfPCell(new Phrase(dtoEmpresaCobro.Potepmonto.ToString(formatNumber), NumberFont));
                    cellbodyN.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellbodyN);
                    dTotalRow += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                    dTotalColum[colum] += Convert.ToDecimal(dtoEmpresaCobro.Potepmonto);
                    colum++;
                }
                PdfPCell cellbodyT = new PdfPCell(new Phrase(dTotalRow.ToString(formatNumber), NumberFont));
                cellbodyT.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellbodyT);
                dTotalColum[colum] += dTotalRow;
            }
            PdfPCell cellFooter = new PdfPCell(new Phrase("TOTAL", TextFont));
            cellFooter.HorizontalAlignment = Element.ALIGN_LEFT;
            cellFooter.BackgroundColor = colorbg;
            table.AddCell(cellFooter);

            colum = 3;
            for (int i = 0; i <= iNumEmpresasCobro; i++)
            {
                PdfPCell cellFooterDato = new PdfPCell(new Phrase(dTotalColum[colum].ToString(formatNumber), NumberFont));
                cellFooterDato.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellFooterDato);
                colum++;
            }
            pdfDoc.Add(table);
            // step 5
            pdfDoc.Close();
        }
    }
}
