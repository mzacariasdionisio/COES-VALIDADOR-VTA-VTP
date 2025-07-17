using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace SgoCoesDesktopReporteHidrologia
{
    internal class Program
    {
        private static string FormatoFechaCorta = "dd/MM/yyyy";
        private static string FormatoFechaExtendido = "yyyy-MM-dd HH:mm:ss";
        public static string ReporteFechaDesde;
        public static string ReporteFechaHasta;

        static void Main(string[] args)
        {
            string tipo = "S"; //S => Semanal - M => Mensual. Por defecto, al no pasar ningún parámetro se procesa como semanal

            if(args.Length > 0)
            {
                tipo = args[0].ToUpper();
            }

            string tmp = ConfigurationManager.AppSettings["DirectorioTemporal"].ToString();
            string fileName = tmp + Guid.NewGuid().ToString().Replace("-", "");
            string fileNameFull = fileName + ".xlsx";

            #region PROCESANDO REPORTE
            crearExcel(fileNameFull, tipo);
            enviarCorreo(fileNameFull);
            eliminarExcel(fileNameFull);
            #endregion
        }

        private static void enviarCorreo(string fileNameFull)
        {
            using (MailMessage correo = new MailMessage())
            {
                correo.Subject = String.Format("REPORTE HIDROLÓGIA SGOCOES DEKTOP - DESDE {0} HASTA {1}", ReporteFechaDesde, ReporteFechaHasta);
                correo.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                foreach (string mailTo in ConfigurationManager.AppSettings["MailTo"].ToString().Split(';').ToList())
                {
                    correo.To.Add(new MailAddress(mailTo));
                }
                foreach (string mailBcc in ConfigurationManager.AppSettings["MailBcc"].ToString().Split(';').ToList())
                {
                    correo.Bcc.Add(new MailAddress(mailBcc));
                }
                Attachment data = new Attachment(fileNameFull, System.Net.Mime.MediaTypeNames.Application.Octet);
                System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(fileNameFull);
                correo.Attachments.Add(data);
                new SmtpClient()
                {
                    Host = ConfigurationManager.AppSettings["EmailServer"].ToString()
                }.Send(correo);
            }
        }

        private static void eliminarExcel(string fileNameFull)
        {
            if (File.Exists(fileNameFull))
            {
                File.Delete(fileNameFull);
            }
        }

        private static void crearExcel(string fileNameFull, string tipo)
        {
            XLWorkbook wbook = new XLWorkbook();

            IXLWorksheet ws = wbook.AddWorksheet("DATA");

            #region CABECERA
            ws.Cell("A1").Value = "COES-SINAC";
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("B3").Value = "Hidrología";
            ws.Cell("B3").Style.Font.Bold = true;
            ws.Cell("B4").Value = String.Format("Desde: {0}  Hasta: {1}", ReporteFechaDesde, ReporteFechaHasta);
            ws.Cell("A6").Value = "Fecha";
            ws.Cell("A6").Style.Font.Bold = true;
            ws.Cell("B6").Value = "Elemento Hidrológico";
            ws.Cell("B6").Style.Font.Bold = true;
            ws.Cell("C6").Value = "Unidad";
            ws.Cell("C6").Style.Font.Bold = true;
            ws.Cell("D6").Value = "Valor";
            ws.Cell("D6").Style.Font.Bold = true;

            ws.Column("A").AdjustToContents();
            ws.Column("B").AdjustToContents();
            ws.Column("C").AdjustToContents();
            ws.Column("D").AdjustToContents();
            #endregion

            #region CUERPO
            int pos = 7;
            foreach (ReporteData item in getData(tipo))
            {
                ws.Cell("A" + pos).Value = item.MediFecha;
                ws.Cell("B" + pos).Value = item.PtoMedicionDesc;
                ws.Cell("C" + pos).Value = item.TipoInfoDesc;
                ws.Cell("D" + pos).Value = item.Valor;
                pos++;
            }
            #endregion

            wbook.SaveAs(fileNameFull);
        }

        private static List<ReporteData> getData(string tipo)
        {
            DateTime fechaActual = tipo == "S" ? DateTime.Now.Date.AddDays(-7) : DateTime.Now.Date.AddMonths(-1);
            DateTime fechainicio = tipo == "S" ? fechaActual.AddDays(-(int)(fechaActual.DayOfWeek == DayOfWeek.Sunday ? fechaActual.DayOfWeek : fechaActual.AddDays(1).DayOfWeek)) : new DateTime(fechaActual.Year, fechaActual.Month, 1);
            DateTime FechaFinal = tipo == "S" ? fechainicio.AddDays(7).AddSeconds(-1) : fechainicio.AddMonths(1).AddDays(-1);
            List<ReporteData> reporteDataList = new List<ReporteData>();
            string query = "SELECT\r\n\tMEPN.PTOMEDICODI,\r\n\tMEPN.PTOMEDIDESC,\r\n\tSI.TIPOINFODESC,\r\n\tto_char(ME1.MEDIFECHA, 'DD/MM/YYYY') MEDIFECHA_DDMMYYYY,\r\n\tME1.H1,\r\n\tME1.LASTUSER,\r\n\tME1.LASTDATE\r\nFROM\r\n\tME_MEDICION1 ME1\r\nINNER JOIN ME_PTOMEDICION MEPN ON\r\n\tMEPN.PTOMEDICODI = ME1.PTOMEDICODI\r\nINNER JOIN SI_TIPOINFORMACION SI ON\r\n\tSI.TIPOINFOCODI = ME1.TIPOINFOCODI\r\nWHERE\r\n\tMEDIFECHA >= to_date('{0}', 'yyyy-mm-dd hh24:mi:ss')\r\n\tAND MEDIFECHA <= to_date('{1}', 'yyyy-mm-dd hh24:mi:ss')\r\n\tAND (\r\n\t\tMEPN.PTOMEDIDESC LIKE '%CAHUA%'\r\n\t\tOR \r\n\t\tMEPN.PTOMEDIDESC LIKE '%EDEGEL%'\r\n\t\tOR \r\n\t\tMEPN.PTOMEDIDESC LIKE '%EGEMSA%'\r\n\t\tOR \r\n\t\tMEPN.PTOMEDIDESC LIKE '%EGENOR%'\r\n\t\tOR \r\n\t\tMEPN.PTOMEDIDESC LIKE '%EGESUR%'\r\n\t\tOR\r\n\t\tMEPN.PTOMEDIDESC LIKE '%ELECTROANDES%'\r\n\t\tOR\r\n\t\tMEPN.PTOMEDIDESC LIKE '%ELECTROPERU%'\r\n\t\tOR\r\n\t\tMEPN.PTOMEDIDESC LIKE '%SAN GABAN%'\r\n\t\tOR\r\n\t\tMEPN.PTOMEDIDESC LIKE '%EGASA%' \r\n\t)\r\nORDER BY\r\n\tMEPN.PTOMEDIDESC ASC,\r\n\tME1.MEDIFECHA ASC";
            query = string.Format(query, fechainicio.ToString(FormatoFechaExtendido), FechaFinal.ToString(FormatoFechaExtendido));
            Console.WriteLine(query);
            ReporteFechaDesde = fechainicio.ToString(FormatoFechaCorta);
            ReporteFechaHasta = FechaFinal.ToString(FormatoFechaCorta);
            string connectionStringsSicoes = ConfigurationManager.AppSettings["database-sicoes"];

            using (OracleConnection con = new OracleConnection(connectionStringsSicoes))
            {
                con.Open();
                using (OracleCommand command = new OracleCommand(query, con))
                {
                    using (OracleDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            ReporteData reporteData = new ReporteData();
                            reporteData.PtoMedicionCodi = Convert.ToInt32(dr["PTOMEDICODI"]);
                            reporteData.PtoMedicionDesc = Convert.ToString(dr["PTOMEDIDESC"]);
                            reporteData.TipoInfoDesc = Convert.ToString(dr["TIPOINFODESC"]);
                            reporteData.MediFecha = Convert.ToString(dr["MEDIFECHA_DDMMYYYY"]);
                            reporteData.Valor = Convert.ToDouble(dr["H1"]);
                            reporteDataList.Add(reporteData);
                            Console.WriteLine(reporteData.getInfo()); 
                        }
                    }
                }
                con.Close();
            }

            return reporteDataList;
        } 

    }
}
