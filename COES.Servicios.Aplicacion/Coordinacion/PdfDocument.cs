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
using COES.Servicios.Aplicacion.CambioTurno;


namespace COES.Servicios.Aplicacion.Coordinacion
{
    public class PdfDocument
    {


        /// <summary>
        /// 0: Celdas
        /// 1: Titulos
        /// 2: Subtitulos
        /// 3: Comentarios
        /// 4: agrupaciones      
        /// 5: bloque cabecera
        /// 6: datos
        /// 7: Contenido de datos
        /// 8: Primera de fila titulo
        /// 9: segunda fila titulo
        /// 10: tercera fila titulo
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tipo"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <returns></returns>
        public PdfPCell ObtieneCelda(string texto, int tipo, int colspan, int rowspan)
        {
            Font fuente = null;

            if (tipo == 0)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.NORMAL,
                    new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1C91AE")));
            }
            if (tipo == 1)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD, BaseColor.WHITE);
            }
            if (tipo == 2)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD,
                    new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1C91AE")));
            }
            if (tipo == 3)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.NORMAL,
                                       new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EA9140")));
            }
            if (tipo == 4)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD,
                                       new BaseColor(System.Drawing.ColorTranslator.FromHtml("#AD6500")));
            }
            if (tipo == 5)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD, BaseColor.WHITE);
            }
            if (tipo == 6)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD,
                                           new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FA7D00")));
            }
            if (tipo == 7)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD,
                                           new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1C91AE")));
            }
            if (tipo == 8) 
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.WHITE);
            }
            if (tipo == 9)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD, BaseColor.BLACK);
            }
            if (tipo == 10)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD, BaseColor.BLACK);
            }
            if (tipo == 11)
            {
                fuente = new Font(Font.FontFamily.HELVETICA, 7f, Font.NORMAL,
                    new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1C91AE")));
            }


            if (string.IsNullOrEmpty(texto)) texto = " ";


            PdfPCell celda = new PdfPCell(new Phrase(texto, fuente));           
            celda.Colspan = colspan;
            celda.Rowspan = rowspan;
            celda.VerticalAlignment = Element.ALIGN_MIDDLE;

            if (tipo == 0)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#E7E7E7"));
            }
            if (tipo == 1)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1991B5"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
            }
            if (tipo == 2)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D7EFEF"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
            }
            if (tipo == 3)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
            }
            if (tipo == 5)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            }
            if (tipo == 4)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFEB9C"));
            }           
            if (tipo == 6)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            }
            if (tipo == 7)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#DADAD9"));
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            }
            if (tipo == 8)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#1991B5"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            }
            if (tipo == 9)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            }
            if(tipo == 10)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            }
            if (tipo == 11)
            {
                celda.Border = Rectangle.BOX;
                celda.BorderWidth = 1;
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EAFFEA"));
                celda.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
               
            }

            return celda;
        }


        /// <summary>
        /// Permite generar el reporte de pertubación en formato PDF
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="evento"></param>
        /// <param name="idEvento"></param>
        /// <param name="path"></param>
        public void GenerarReporteInforme(SiCambioTurnoDTO entity, string responsable, string path, string fileName)
        {
            #region Encabezado

            Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50f, 50f);
            string rutapdf = path + fileName;

            if (File.Exists(rutapdf))
            {
                File.Delete(rutapdf);
            }

            FileStream file = new FileStream(rutapdf, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);



            pdfDoc.Open();

            //Paragraph titulo = new Paragraph("ENTREGA DE TURNO DEL CENTRO COORDINADOR DE LA OPERACIÓN", new Font(Font.FontFamily.TIMES_ROMAN, 10f, Font.BOLD, BaseColor.BLACK));
            //titulo.Alignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(titulo);

            //Paragraph subTitulo = new Paragraph("DIRECCIÓN DE OPERACIONES");
            //subTitulo.Alignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(subTitulo);

            //subTitulo = new Paragraph("SUB DIRECCIÓN DE COORDINACIÓN");
            //subTitulo.Alignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(subTitulo);


            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widths = new float[] { 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 0f;

            string pathLogo = ConfigurationManager.AppSettings["RutaExportacionInformeEvento"].ToString() + "coes.png";
            iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
            imgLogo.ScalePercent(0.5f);
            imgLogo.Alignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(imgLogo, true);
            cell.FixedHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = 10;
            table.AddCell(cell);
            table.AddCell(this.ObtieneCelda(" ", 5, 10, 1));

            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("ENTREGA DE TURNO DEL CENTRO COORDINADOR DE LA OPERACIÓN", 8, 7, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));

            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("DIRECCIÓN DE OPERACIONES", 9, 5, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));

            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("SUB DIRECCIÓN DE COORDINACIÓN", 10, 5, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));
            table.AddCell(this.ObtieneCelda("", 5, 1, 1));

            table.AddCell(this.ObtieneCelda(" ", 5, 10, 1));

            string fecha = (entity.Fecturno != null) ? ((DateTime)entity.Fecturno).ToString("dd/MM/yyyy") : string.Empty;
            table.AddCell(this.ObtieneCelda("COORDINADOR RESPONSABLE:", 6, 2, 1));
            table.AddCell(this.ObtieneCelda(responsable, 7, 4, 1));
            table.AddCell(this.ObtieneCelda("FECHA:", 6, 1, 1));
            table.AddCell(this.ObtieneCelda(fecha, 7, 1, 1));
            table.AddCell(this.ObtieneCelda("TURNO:", 6, 1, 1));
            table.AddCell(this.ObtieneCelda("TURNO " + entity.Turno, 7, 1, 1));

            table.AddCell(this.ObtieneCelda("1. DESPACHO", 1, 10, 1));
            table.AddCell(this.ObtieneCelda("CENTRAL MARGINAL", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("RSF-AUTOMÁTICA SEIN", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("RSF-MANUAL SEIN", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("RSF-SISTEMAS AISLADOS", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("URS", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("MAGNITUD", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("URS", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("MAGNITUD", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("URS", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("MAGNITUD", 2, 1, 1));

            SiCambioTurnoSeccionDTO seccion11 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion11).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion11.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Despcentromarginal, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Despursautomatica, 0, 2, 1));
                table.AddCell(this.ObtieneCelda((item.Despmagautomatica != null) ? item.Despmagautomatica.ToString() : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Despursmanual, 0, 2, 1));
                table.AddCell(this.ObtieneCelda((item.Despmagmanual != null) ? item.Despmagmanual.ToString() : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Despcentralaislado, 0, 2, 1));
                table.AddCell(this.ObtieneCelda((item.Despmagaislado != null) ? item.Despmagaislado.ToString() : string.Empty, 0, 1, 1));
            }

            table.AddCell(this.ObtieneCelda("REPROGRAMAS", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("HORA", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("MOTIVO PRINCIPAL", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("ATR PUBLICADO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("PREMISAS IMPORTANTES", 2, 4, 1));

            SiCambioTurnoSeccionDTO seccion12 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion12).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion12.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Despreprogramas, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Desphorareprog, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Despmotivorepro, 0, 3, 1));
                table.AddCell(this.ObtieneCelda(item.Desparchivoatr, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Desppremisasreprog, 0, 4, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion12.Descomentario)) ? "Comentarios adicionales" : seccion12.Descomentario,
                3, 10, 2));


            table.AddCell(this.ObtieneCelda("2.  MANTENIMIENTOS RELEVANTES", 1, 10, 1));
            table.AddCell(this.ObtieneCelda("EQUIPO", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("TIPO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("HORA CONEXÓN", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA", 2, 6, 2));
            table.AddCell(this.ObtieneCelda("PROG", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("CORREC", 2, 1, 1));

            SiCambioTurnoSeccionDTO seccion21 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion21).FirstOrDefault();

            List<SiCambioTurnoSubseccionDTO> list21 = seccion21.ListItems.OrderBy(x => x.Manhoraconex).ToList(); ;
            foreach (SiCambioTurnoSubseccionDTO item in list21)
            {
                if (item.Manhoraconex == "00:00")
                {
                    table.AddCell(this.ObtieneCelda(item.Manequipo, 11, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "P") ? "X" : "", 11, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "C") ? "X" : "", 11, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manhoraconex, 11, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manconsideraciones, 11, 6, 1));
                }
                else
                {
                    table.AddCell(this.ObtieneCelda(item.Manequipo, 0, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "P") ? "X" : "", 0, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "C") ? "X" : "", 0, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manhoraconex, 0, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manconsideraciones, 0, 6, 1));
                }
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion21.Descomentario)) ? "Comentarios adicionales" : seccion21.Descomentario,
                3, 10, 2));

            table.AddCell(this.ObtieneCelda("EQUIPO", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("TIPO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("HORA CONEXÓN", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("CONSIDERACIONES IMPORTANTES PARA LA MANIOBRA", 2, 6, 2));
            table.AddCell(this.ObtieneCelda("PROG", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("CORREC", 2, 1, 1));

            SiCambioTurnoSeccionDTO seccion22 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion22).FirstOrDefault();

            List<SiCambioTurnoSubseccionDTO> list22 = seccion22.ListItems.OrderBy(x => x.Manhoraconex).ToList();
            foreach (SiCambioTurnoSubseccionDTO item in list22)
            {
                if (item.Manhoraconex == "00:00")
                {
                    table.AddCell(this.ObtieneCelda(item.Manequipo, 11, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "P") ? "X" : "", 11, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "C") ? "X" : "", 11, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manhoraconex, 11, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manconsideraciones, 11, 6, 1));
                }
                else
                {
                    table.AddCell(this.ObtieneCelda(item.Manequipo, 0, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "P") ? "X" : "", 0, 1, 1));
                    table.AddCell(this.ObtieneCelda((item.Mantipo == "C") ? "X" : "", 0, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manhoraconex, 0, 1, 1));
                    table.AddCell(this.ObtieneCelda(item.Manconsideraciones, 0, 6, 1));
                }
            }

            table.AddCell(this.ObtieneCelda("3.  SUMINISTRO DE ENERGIA", 1, 10, 1));
            table.AddCell(this.ObtieneCelda("SUBESTACIÓN", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("MOTIVO DEL CORTE", 2, 4, 1));
            table.AddCell(this.ObtieneCelda("HORA INICIO", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("REPOSICIÓN", 2, 1, 2));
            table.AddCell(this.ObtieneCelda("CONSIDERACIONES IMPORTANTES", 2, 3, 2));
            table.AddCell(this.ObtieneCelda("Deficit/Sobrecarga", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("FALLA", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("TENSIÓN", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("MANTTO", 2, 1, 1));

            SiCambioTurnoSeccionDTO seccion31 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion31).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion31.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Sumsubestacion, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Summotivocorte == "D") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Summotivocorte == "F") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Summotivocorte == "T") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Summotivocorte == "M") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Sumhorainicio, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Sumreposicion, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Sumconsideraciones, 0, 3, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion31.Descomentario)) ? "Comentarios adicionales" : seccion31.Descomentario,
                3, 10, 2));

            table.AddCell(this.ObtieneCelda("4.  OTROS ASPECTOS RELEVANTES PARA LA OPERACIÓN", 1, 10, 1));
            table.AddCell(this.ObtieneCelda("Regulación de tensión", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("OPERACIÓN DE CENTRALES", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("SUBESTACIÓN", 2, 4, 1));
            table.AddCell(this.ObtieneCelda("HORA FIN APROX", 2, 3, 1));

            SiCambioTurnoSeccionDTO seccion41 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion41).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion41.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Regopecentral, 0, 3, 1));
                table.AddCell(this.ObtieneCelda(item.Regcentralsubestacion, 0, 4, 1));
                table.AddCell(this.ObtieneCelda(item.Regcentralhorafin, 0, 3, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion41.Descomentario)) ? "Comentarios adicionales" : seccion41.Descomentario,
                3, 10, 2));

            table.AddCell(this.ObtieneCelda("Lineas desconectadas", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("LÍNEA", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("SUBESTACIÓN", 2, 4, 1));
            table.AddCell(this.ObtieneCelda("HORA FIN APROX", 2, 3, 1));

            SiCambioTurnoSeccionDTO seccion42 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion42).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion42.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Reglineas, 0, 3, 1));
                table.AddCell(this.ObtieneCelda(item.Reglineasubestacion, 0, 4, 1));
                table.AddCell(this.ObtieneCelda(item.Reglineahorafin, 0, 3, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion42.Descomentario)) ? "Comentarios adicionales" : seccion42.Descomentario,
               3, 10, 2));

            table.AddCell(this.ObtieneCelda("Gestión de Mantenimientos fuera del PDO", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("EQUIPO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("ACEPTADO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("RECHAZADO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("DETALLE (descripción, fecha, hora inicio, hora fin) (motivo de rechazo)", 2, 7, 1));

            SiCambioTurnoSeccionDTO seccion43 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion43).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion43.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Gesequipo, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Gesaceptado == "A") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda((item.Gesaceptado == "R") ? "X" : string.Empty, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Gesdetalle, 0, 7, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion43.Descomentario)) ? "Comentarios adicionales" : seccion43.Descomentario,
             3, 10, 2));

            table.AddCell(this.ObtieneCelda("Eventos Importantes", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("EQUIPO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("HORA INICIO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("REPOSICIÓN", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("RESUMEN (hora falla, carga interrumpida,reposición del equipo)", 2, 6, 1));


            SiCambioTurnoSeccionDTO seccion44 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion44).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion44.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Eveequipo, 0, 2, 1));
                table.AddCell(this.ObtieneCelda(item.Evehorainicio, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Evereposicion, 0, 1, 1));
                table.AddCell(this.ObtieneCelda(item.Everesumen, 0, 6, 1));
            }

            table.AddCell(this.ObtieneCelda((string.IsNullOrEmpty(seccion44.Descomentario)) ? "Comentarios adicionales" : seccion44.Descomentario,
             3, 10, 2));

            table.AddCell(this.ObtieneCelda("Informes Finales de Falla próximos a vencer (Tipo N1 dentro de las 24h)", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("EQUIPO", 2, 3, 1));
            table.AddCell(this.ObtieneCelda("ENVIADO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("PENDIENTE", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("PLAZO (h)", 2, 3, 1));

            SiCambioTurnoSeccionDTO seccion45 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion45).FirstOrDefault();

            foreach (SiCambioTurnoSubseccionDTO item in seccion45.ListItems)
            {
                table.AddCell(this.ObtieneCelda(item.Infequipo, 0, 3, 1));
                table.AddCell(this.ObtieneCelda((item.Infestado == "E") ? "X" : string.Empty, 0, 2, 1));
                table.AddCell(this.ObtieneCelda((item.Infestado == "P") ? "X" : string.Empty, 0, 2, 1));
                table.AddCell(this.ObtieneCelda(item.Infplazo, 0, 3, 1));
            }


            //-Inicio cambios

            table.AddCell(this.ObtieneCelda("5.  VISUALIZACIÓN DE SORTEO DE PRUEBAS ALEATORIAS", 1, 10, 1));
            table.AddCell(this.ObtieneCelda("FECHA DE SORTEO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("SORTEO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("RESULTADO", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("GENERADOR", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("PRUEBA", 2, 2, 1));

            if (entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion51).Count() > 0)
            {
                SiCambioTurnoSeccionDTO seccion51 = entity.ListaSeccion.Where(x => x.Nroseccion == SubSeccionesCambio.Seccion51).FirstOrDefault();

                foreach (SiCambioTurnoSubseccionDTO item in seccion51.ListItems)
                {
                    table.AddCell(this.ObtieneCelda(item.Pafecha, 0, 2, 1));
                    table.AddCell(this.ObtieneCelda(item.Pasorteo, 0, 2, 1));
                    table.AddCell(this.ObtieneCelda(item.Paresultado, 0, 2, 1));
                    table.AddCell(this.ObtieneCelda(item.Pagenerador, 0, 2, 1));
                    table.AddCell(this.ObtieneCelda(item.Paprueba, 0, 2, 1));
                }

            }

            //-Fin cambios


            table.AddCell(this.ObtieneCelda("Otros", 4, 10, 1));
            table.AddCell(this.ObtieneCelda("CASOS SIN RESERVA", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("EMS OPERATIVO", 2, 6, 1));
            table.AddCell(this.ObtieneCelda("HORA DE ENTREGA DE TURNO", 2, 2, 2));

            table.AddCell(this.ObtieneCelda("Se entrega todos los casos SIN RESERVA", 2, 2, 1));
            table.AddCell(this.ObtieneCelda("SI", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("NO", 2, 1, 1));
            table.AddCell(this.ObtieneCelda("OBSERVACIONES", 2, 4, 1));

            table.AddCell(this.ObtieneCelda(entity.CasoSinReserva, 0, 2, 1));
            table.AddCell(this.ObtieneCelda((entity.Emsoperativo == ConstantesAppServicio.SI) ? "X" : "", 0, 1, 1));
            table.AddCell(this.ObtieneCelda((entity.Emsoperativo == ConstantesAppServicio.NO) ? "X" : "", 0, 1, 1));
            table.AddCell(this.ObtieneCelda(entity.Emsobservaciones, 0, 4, 1));
            table.AddCell(this.ObtieneCelda(entity.Horaentregaturno, 0, 2, 1));


            table.AddCell(this.ObtieneCelda("", 0, 10, 1));
            table.AddCell(this.ObtieneCelda(entity.Coordinadorrecibe, 7, 2, 1));
            table.AddCell(this.ObtieneCelda(entity.Especialistarecibe, 7, 4, 1));
            table.AddCell(this.ObtieneCelda(entity.Analistarecibe, 7, 4, 1));
            table.AddCell(this.ObtieneCelda("COORDINADOR QUE RECIBE EL TURNO", 6, 2, 1));
            table.AddCell(this.ObtieneCelda("ESPECIALISTA QUE RECIBE EL TURNO", 6, 4, 1));
            table.AddCell(this.ObtieneCelda("ANALISTA QUE RECIBE EL TURNO", 6, 4, 1));

            pdfDoc.Add(table);


            #endregion

            pdfDoc.Close();
        }
    }
}

