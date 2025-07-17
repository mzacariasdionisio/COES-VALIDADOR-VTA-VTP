using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using Novacode;
using COES.Dominio.DTO.Sic;
using System.IO;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using System.Drawing;
using COES.Servicios.Aplicacion.RegistroIntegrantes;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Helper
{
    public class WordDocument
    {
        /// <summary>
        /// Permite generar carta en formato WORD
        /// </summary>
        /// <param name="empresa"></param>        
        public byte[] GenerarCarta(CartaModel model)
        {
            #region Generacion del Documento Word

            byte[] bytes = new byte[0];

            using (DocX Report = DocX.Create(string.Format("Report-{0}.doc", DateTime.Now.Ticks)))
            {

                Paragraph p = Report.InsertParagraph();

                p.AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .Append("San Isidro, " + model.FechaRegistro).FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .AppendLine()
                ;

                p.Append(model.NroDocumento).FontSize(11).Bold().Font(new FontFamily("Arial")).UnderlineStyle(UnderlineStyle.singleLine)
                    .AppendLine()
                    .AppendLine()
                ;

                p.Append("Señor").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .Append(model.Nombre).Bold().FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                ;

                p.Append(model.Cargo).FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .Append(model.Empresa).Bold().FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                ;

                if (model.Direccion != "")
                {
                    p.Append(model.Direccion + ",").FontSize(11).Font(new FontFamily("Arial"))
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                    ;
                }

                //- Inicio de modificación

                Table tabla = Report.InsertTable(3, 2);
                tabla.AutoFit = AutoFit.Contents;
                tabla.Design = TableDesign.None;
                
                tabla.Rows[0].Cells[0].Paragraphs[0].Append("Asunto:");
                tabla.Rows[0].Cells[1].Paragraphs[0].Append("INSCRIPCIÓN DE LA EMPRESA " + model.Empresa + ", EN EL REGISTRO DE INTEGRANTES DEL COES");
                tabla.Rows[2].Cells[0].Paragraphs[0].Append("Ref.:");
                tabla.Rows[2].Cells[1].Paragraphs[0].Append("Solicitud de inscripción en el Registro de Integrantes del COES recibida el " + model.FechaRegistro);

                tabla.Rows[0].Cells[0].Paragraphs[0].FontSize(10);
                tabla.Rows[0].Cells[1].Paragraphs[0].FontSize(11);
                tabla.Rows[2].Cells[0].Paragraphs[0].FontSize(10);
                tabla.Rows[2].Cells[1].Paragraphs[0].FontSize(11);
                tabla.Rows[0].Cells[1].Paragraphs[0].Bold();
                tabla.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.both;
                tabla.Rows[2].Cells[1].Paragraphs[0].Alignment = Alignment.both;


                //p.Append("Asunto:").FontSize(11).Font(new FontFamily("Arial")).UnderlineStyle(UnderlineStyle.singleLine)
                //;

                //p.Append("     ").FontSize(11).Font(new FontFamily("Arial"));
                //;

                //p.Append("INSCRIPCIÓN DE LA EMPRESA ").Bold().FontSize(11).Font(new FontFamily("Arial"))
                //    .Append(model.Empresa + ", EN EL REGISTRO DE INTEGRANTES DEL COES").Bold().FontSize(11).Font(new FontFamily("Arial"))
                //    .AppendLine()
                //    .AppendLine()
                //;

                //p.Append("Ref.:").FontSize(11).Font(new FontFamily("Arial")).UnderlineStyle(UnderlineStyle.singleLine)
                //;

                //p.Append("     ").FontSize(11).Font(new FontFamily("Arial"));
                //;

                //p.Append("Solicitud de inscripción en el Registro de Integrantes del COES recibida el ").FontSize(11).Font(new FontFamily("Arial"))
                //    .Append(model.FechaSolicitud).FontSize(11).Font(new FontFamily("Arial"))
                //    .AppendLine()
                //    .AppendLine()
                //;

                //-FIn de modificación

                Paragraph p1 = Report.InsertParagraph();

                p1.AppendLine();
                p1.Append("De mi consideración:").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                ;
                
                Paragraph n = Report.InsertParagraph();

                n.Append("Me dirijo a usted en atención a su solicitud, recibida mediante la comunicación de la referencia.").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .AppendLine()
                    .Append("Al respecto, le informamos que, encontrándose su representada dentro de los alcances del artículo 3 del Decreto Supremo Nº 027-2008-EM, que aprueba el Reglamento del Comité de Operación Económica del Sistema (COES), ").FontSize(11).Font(new FontFamily("Arial"))
                ;

                n.Append(model.Empresa).Bold().FontSize(11).Font(new FontFamily("Arial"))
                 ;

                n.Append(", ha sido formalmente inscrita como Integrante Obligatorio en dicho Registro a partir del día ").FontSize(11).Font(new FontFamily("Arial"))
                   .Append(model.FechaRegistro).FontSize(11).Font(new FontFamily("Arial"))
                   .Append(" con la Ficha de Registro Nº").FontSize(11).Font(new FontFamily("Arial"))
                   .Append(model.NroRegistro).FontSize(11).Font(new FontFamily("Arial"))
                   .Append(", teniendo como representante legal ante el COES al ").FontSize(11).Font(new FontFamily("Arial"))
               ;

                n.Append("Sr. " + model.Nombre + ".").Bold().FontSize(11).Font(new FontFamily("Arial"))
                   .AppendLine()
                   .AppendLine()
                ;

                n.Append("Adjuntamos a la presente el formulario de su empresa con los datos que incorporaron al Sistema de Información del COES mediante su Portal de Internet. En caso de requerirse alguna modificación, agradeceremos utilizar el Usuario y Contraseña remitidos a la dirección electrónica que figura en el formulario y enviar al COES el formulario corregido y firmado, así como cualquier nueva documentación de sustento, de ser pertinente.").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .AppendLine()
                ;

                n.Alignment = Alignment.both;

                Paragraph m = Report.InsertParagraph();

                m.Append("Sin otro particular, hago propicia la ocasión para saludarlo.").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .AppendLine();

                Paragraph m1 = Report.InsertParagraph();
                m1.Alignment = Alignment.center;
                    m1.Append("Atentamente,").FontSize(11).Font(new FontFamily("Arial"))
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
               
                 
                ;

                Report.AddFooters();
                Report.InsertParagraph().Append("Adj.: Lo indicado.").FontSize(8).Bold().FontSize(11).Font(new FontFamily("Arial"));
                Report.InsertParagraph().Append("C.c.: P, DO, DP, DAD, DJR, SPR, SCO, STR, SEV, SGI, SNP, SPL, DTI.").FontSize(8).Bold().FontSize(11).Font(new FontFamily("Arial"));

                MemoryStream ms = new MemoryStream();
                Report.SaveAs(ms);

                bytes = ms.GetBuffer();

            }
            return bytes;
            #endregion
        }      

        /// <summary>
        /// Permite generar el reporte de representantes legales en formato WORD
        /// </summary>
        /// <param name="Lista"></param>        
        /// <param name="path"></param>
        public void GenerarReporteRepresentantesLegales(List<SiEmpresaDTO> Lista, string path)
        {
            #region Generacion del Documento Word

            using (DocX document = DocX.Create(path + ConstantesRegistroIntegrantes.NombreReporteEnviosWord))
            {



                Novacode.Image logo = document.AddImage(path + Constantes.NombreLogoCoes);
                document.AddHeaders();
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;
                Header header_first = document.Headers.odd;

                #region Company Logo in Header in Table

                Table header_first_table = header_first.InsertTable(2, 3);
                header_first_table.Design = TableDesign.TableGrid;
                header_first_table.AutoFit = AutoFit.Contents;


                Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                upperRightParagraph.AppendPicture(logo.CreatePicture());
                upperRightParagraph.Alignment = Alignment.center;

                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                header_first_table.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                header_first_table.MergeCellsInColumn(0, 0, 1);
                header_first.InsertParagraph("\n");

                Paragraph cabecera = header_first.Tables[0].Rows[0].Cells[1].Paragraphs[0];
                cabecera.Append("REPORTE DE REPRESENTANTES LEGALES");
                cabecera.Alignment = Alignment.center;
                cabecera.FontSize(13);
                cabecera.Bold();

                cabecera = header_first.Tables[0].Rows[0].Cells[2].Paragraphs[0];
                cabecera.Append(String.Format("Fecha de Emisión:\n {0}", DateTime.Now.ToString("dd/MM/yyyy")));
                cabecera.Alignment = Alignment.center;

                cabecera = header_first.Tables[0].Rows[1].Cells[1].Paragraphs[0];
                cabecera.Append("Registro de Integrantes");
                cabecera.Alignment = Alignment.center;

                cabecera = header_first.Tables[0].Rows[1].Cells[2].Paragraphs[0];
                cabecera.AppendPageNumber(PageNumberFormat.normal);
                cabecera.Alignment = Alignment.center;

                #endregion


                if (Lista.Count > 0)
                {
                    Table secuencia = document.InsertTable(Lista.Count + 1, 5);

                    secuencia.AutoFit = AutoFit.Window;
                    secuencia.Design = TableDesign.TableGrid;

                    secuencia.Rows[0].Cells[0].Paragraphs[0].Append("Razón Social");
                    secuencia.Rows[0].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[0].Paragraphs[0].Bold();

                    secuencia.Rows[0].Cells[1].Paragraphs[0].Append("Ruc");
                    secuencia.Rows[0].Cells[1].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[1].Paragraphs[0].Bold();

                    secuencia.Rows[0].Cells[2].Paragraphs[0].Append("Tipo");
                    secuencia.Rows[0].Cells[2].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[2].Paragraphs[0].Bold();

                    secuencia.Rows[0].Cells[3].Paragraphs[0].Append("Nombres");
                    secuencia.Rows[0].Cells[3].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[3].Paragraphs[0].Bold();

                    secuencia.Rows[0].Cells[4].Paragraphs[0].Append("Correo Electrónico");
                    secuencia.Rows[0].Cells[4].FillColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
                    secuencia.Rows[0].Cells[4].Paragraphs[0].Alignment = Alignment.center;
                    secuencia.Rows[0].Cells[4].Paragraphs[0].Bold();

                    int index = 1;
                    foreach (SiEmpresaDTO entity in Lista)
                    {
                        secuencia.Rows[index].Cells[0].Paragraphs[0].Append(entity.Emprrazsocial);
                        secuencia.Rows[index].Cells[1].Paragraphs[0].Append(entity.Emprruc);
                        secuencia.Rows[index].Cells[2].Paragraphs[0].Append(entity.RpteTipRepresentanteLegal);
                        secuencia.Rows[index].Cells[3].Paragraphs[0].Append(entity.RpteNombres);
                        secuencia.Rows[index].Cells[4].Paragraphs[0].Append(entity.RpteCorreoElectronico);
                        index++;
                    }
                }

                document.Save();
            }

            #endregion
        }



    }
}