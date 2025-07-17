using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Helper
{
    public class InterconexionHelper
    {
        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarFormato(List<MeHojaptomedDTO> lista, string empresa, DateTime fecha,string ruta)
        {

            string fileTemplate = NombreArchivoInterconexiones.PlantillaFormatoInterconexion;
            FileInfo template = new FileInfo(ruta + fileTemplate);
            FileInfo newFile = new FileInfo(ruta + NombreArchivoInterconexiones.FormatoInterconexion);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoInterconexiones.FormatoInterconexion);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesMedidores.HojaFormatoExcel];
                ///Imprimimos cabecera de puntos de medicion
                int row = 1;
                int column = 2;
                foreach (var reg in lista)
                {
                    ws.Cells[row, column].Value = reg.Hojaptoorden;
                    column++;
                }
                /// Nombre de la Empresa
                row = 13;
                column = 3;
                ws.Cells[row, column].Value = empresa;
                /// Año
                row++;
                ws.Cells[row, column].Value = fecha.Year;
                //Mes
                row++;
                ws.Cells[row, column].Value = Util.ObtenerNombreMes(fecha.Month);
                //Día
                row++;
                ws.Cells[row, column].Value = fecha.Day;
                row = 22;
                column = 2;
                for (int i = 1; i <= 96; i++)
                {
                    ws.Cells[row, 1].Value = ((fecha.AddMinutes(i * 15))).ToString(Constantes.FormatoFechaHora);

                    for (int k = 2; k < column; k++)
                    {
                       ws.Cells[row, k].StyleID = ws.Cells[row, 2].StyleID;
                    }
                    row++;
                    }
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Listar empresas 
        /// </summary>
        /// <returns></returns>
        public static List<SiEmpresaDTO> ListarEmpresaInterconexion()
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();
            lista = new List<SiEmpresaDTO>(){
                new SiEmpresaDTO(){
                     Emprcodi= 12,
                     Emprnomb = "RED DE ENERGÍA DEL PERÚ"
                }
            };
            return lista;
        }


        /// <summary>
        /// Obtiene la lista de los periodos de medidores utilizados en un envio
        /// </summary>
        public static List<MePeriodomedidorDTO> GetListaPeriodoMedidor(List<string> periodos, int enviocodi, DateTime fecha)
        {
            List<MePeriodomedidorDTO> listaPeriodo = new List<MePeriodomedidorDTO>();
            MePeriodomedidorDTO periodo = new MePeriodomedidorDTO();
            int mediant = 0;
            int medidor = 0;
            DateTime periodoIni = DateTime.Now;
            DateTime periodoFin = DateTime.Now;
            for (var i = 0; i < 96; i++)
            {
                medidor = int.Parse(periodos[i]);
                if(mediant != medidor ) 
                {
                    if (i != 0)
                    {
                        periodoFin = fecha.AddMinutes(i * 15);
                        periodo.Permedifechafin = periodoFin;
                        periodo.Permedifechaini = periodoIni;
                        periodo.Medicodi = mediant;
                        periodo.Earcodi = enviocodi;
                        listaPeriodo.Add(periodo);
                        periodo = new MePeriodomedidorDTO();
                    }
                    periodoIni = fecha.AddMinutes((i + 1) * 15);
                    mediant = medidor;
                }
            }
            periodoFin = fecha.AddMinutes(96 * 15);
            periodo.Permedifechafin = periodoFin;
            periodo.Permedifechaini = periodoIni;
            periodo.Medicodi = mediant;
            periodo.Earcodi = enviocodi;
            listaPeriodo.Add(periodo);
            return listaPeriodo;
        }

        /// <summary>
        /// Genera el Texto del Correo
        /// </summary>
        /// <param name="fechaMedicion"></param>
        /// <param name="fechaEnvio"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idCumplimiento"></param>
        /// <param name="listaPeriodo"></param>
        /// <returns></returns>
        public static string GenerarBodyMail(DateTime fechaMedicion,DateTime fechaEnvio,int idEnvio,int idCumplimiento,List<MePeriodomedidorDTO> listaPeriodo)
        {
            StringBuilder strHtml = new StringBuilder();
            string stCumplimiento = string.Empty;
            string stCumplimientoMensaje = string.Empty;
            if (idCumplimiento == ConstantesMedicion.ENVIO_FUERAPLAZO)
            {
                stCumplimiento = "Fuera de Plazo";
                stCumplimientoMensaje = "<strong>“Fuera de Plazo“</strong>";
            }
            else
            {
                stCumplimiento = "En Plazo";
            }
            strHtml.Append("<html>");
            strHtml.Append("    <head><STYLE TYPE='text/css'>");
            strHtml.Append("    body {font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}");
            strHtml.Append("    .mail {width:500px;border-spacing:0;border-collapse:collapse;}");
            strHtml.Append("    .mail thead th {text-align: center;background: #417AA7;color:#ffffff}");            
            strHtml.Append("    .mail tr td {border:1px solid #dddddd;}");            
            strHtml.Append("    </style></head>");
            strHtml.Append("    <body>");
            strHtml.Append("        <P>Estimados Ingenieros:</p><p>RED DE ENERGÍA DEL PERÚ. S.A.</P>");
            strHtml.Append("        <P>Por medio del presente, se le comunica que se registró " + stCumplimientoMensaje + " en el portal extranet la información de Intercambios Internacionales");
            strHtml.Append(" de Electricidad de su representada en atención a lo  dispuesto en el <strong>Procedimiento Técnico N°43 - </strong> “Intercambios Internacionales");
            strHtml.Append(" de Electricidad en Marco de la Decisión  757 de la CAN ” el que se detalla a seguir:");
            strHtml.Append(" <TABLE class='mail'>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Empresa:</TD><TD> RED DE ENERGÍA DEL PERÚ. S.A.</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Enlace Internacional:<TD> L-2280 (Zorritos-Machala).</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Fecha de Medición: </TD><TD>" + fechaMedicion.ToString(ConstantesBase.FormatoFecha) + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Fecha de Envío: </TD><TD>" + fechaEnvio.ToString(ConstantesBase.FormatoFechaExtendido) + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Código de Envío: </TD><TD>" + idEnvio + "</TD></TR>");
            strHtml.Append(" <TR><TD style='background: #417AA7;color:#ffffff;'>Cumplimiento:</TD><TD> " + stCumplimiento + "</TD></TR>");
            strHtml.Append(" </Table>");
            strHtml.Append(" <p><strong>Medidores Utilizados:</strong></p>");
            strHtml.Append(" <TABLE class='mail'>");
            strHtml.Append(" <thead><tr><td style='background: #417AA7;color:#ffffff;'>Hora Inicio</td><td style='background: #417AA7;color:#ffffff;'>Hora Fin</td><td style='background: #417AA7;color:#ffffff;'>Medidor</td></thead><tbody>");
            foreach (var reg in listaPeriodo)
            {
                string nomMedidor = (reg.Medicodi == 1) ? "S.E. ZORRITOS (Principal)" : "S.E. ZORRITOS (Secundario)";
                strHtml.Append("<tr><td>" + ((DateTime)reg.Permedifechaini).ToString(ConstantesBase.FormatoFechaHora) + "</td><td>" +
                    ((DateTime)reg.Permedifechafin).ToString(ConstantesBase.FormatoFechaHora) + "</td><td>" + nomMedidor + "</td>");
            }
            strHtml.Append(" </tbody></Table>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información –SGI</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();

        }
        
    }
}