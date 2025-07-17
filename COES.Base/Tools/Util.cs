using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Text.RegularExpressions;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.Base.Tools
{
    public class Util
    {
        /// <summary>
        /// Permite obtener el nombre del mes en base al número
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ObtenerNombreMesAbrev(int index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case 1:
                    resultado = "Ene";
                    break;
                case 2:
                    resultado = "Feb";
                    break;
                case 3:
                    resultado = "Mar";
                    break;
                case 4:
                    resultado = "Abr";
                    break;
                case 5:
                    resultado = "May";
                    break;
                case 6:
                    resultado = "Jun";
                    break;
                case 7:
                    resultado = "Jul";
                    break;
                case 8:
                    resultado = "Ago";
                    break;
                case 9:
                    resultado = "Sep";
                    break;
                case 10:
                    resultado = "Oct";
                    break;
                case 11:
                    resultado = "Nov";
                    break;
                case 12:
                    resultado = "Dic";
                    break;
                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el nombre del mes
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ObtenerNombreMes(int index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case 1:
                    resultado = "ENERO";
                    break;
                case 2:
                    resultado = "FEBRERO";
                    break;
                case 3:
                    resultado = "MARZO";
                    break;
                case 4:
                    resultado = "ABRIL";
                    break;
                case 5:
                    resultado = "MAYO";
                    break;
                case 6:
                    resultado = "JUNIO";
                    break;
                case 7:
                    resultado = "JULIO";
                    break;
                case 8:
                    resultado = "AGOSTO";
                    break;
                case 9:
                    resultado = "SETIEMBRE";
                    break;
                case 10:
                    resultado = "OCTUBRE";
                    break;
                case 11:
                    resultado = "NOVIEMBRE";
                    break;
                case 12:
                    resultado = "DICIEMBRE";
                    break;
                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Permite enviar correos
        /// </summary>s
        /// <param name="mails"></param>
        /// <param name="subject"></param>
        /// <param name="file"></param>
        /// <param name="mensaje"></param>
        /// <param name="remitente"></param>
        public static void SendEmail(string mailTo, string subject, string mensaje)
        {
            try
            {
                MailMessage correo = new MailMessage();
                string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
                correo.From = new MailAddress(remitente);
                correo.To.Add(mailTo);
                correo.Bcc.Add("webapp@coes.org.pe");
                correo.Subject = subject;
                correo.Body = mensaje;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
                //TODO. Las 2 lineas siguientes comentadas
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                    ConfigurationManager.AppSettings["PasswordSMTP"]);
                if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite enviar correos
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="subject"></param>
        /// <param name="file"></param>
        /// <param name="mensaje"></param>
        /// <param name="remitente"></param>
        public static void SendEmailSincronizacionXmls(List<string> mailsTo, string mailCc, string subject, string mensaje, string absolutePath)
        {
            try
            {

                Attachment archivo = new Attachment(absolutePath);

                MailMessage correo = new MailMessage();
                string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
                correo.From = new MailAddress(remitente);

                foreach (string mailTo in mailsTo)
                    correo.To.Add(mailTo);

                if (!string.IsNullOrEmpty(mailCc))
                    correo.CC.Add(mailCc);

                correo.Bcc.Add("webapp@coes.org.pe");
                correo.Subject = subject;
                correo.Body = mensaje;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                correo.Attachments.Add(archivo);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
                //TODO. Las 2 lineas siguientes comentadas
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                    ConfigurationManager.AppSettings["PasswordSMTP"]);
                if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite enviar correos
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="subject"></param>
        /// <param name="file"></param>
        /// <param name="mensaje"></param>
        /// <param name="remitente"></param>
        public static void SendEmail(string mailTo, string mailCc, string subject, string mensaje)
        {
            try
            {
                MailMessage correo = new MailMessage();
                string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
                correo.From = new MailAddress(remitente);
                correo.To.Add(mailTo);

                if (!string.IsNullOrEmpty(mailCc))
                    correo.CC.Add(mailCc);

                correo.Bcc.Add("webapp@coes.org.pe");
                correo.Subject = subject;
                correo.Body = mensaje;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
                //TODO. Las 2 lineas siguientes comentadas
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                    ConfigurationManager.AppSettings["PasswordSMTP"]);
                if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite enviar correo con copias
        /// </summary>
        /// <param name="mailsTo"></param>
        /// <param name="mailsCc"></param>
        /// <param name="subject"></param>
        /// <param name="mensaje"></param>
        public static void SendEmail(List<string> mailsTo, List<string> mailsCc, string subject, string mensaje)
        {
            MailMessage correo = new MailMessage();
            string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
            correo.From = new MailAddress(remitente);

            foreach (string mailTo in mailsTo)
                correo.To.Add(mailTo);

            if(mailsCc != null)
                foreach (string mailCc in mailsCc)
                    correo.CC.Add(mailCc);

            correo.Bcc.Add("webapp@coes.org.pe");
            correo.Subject = subject;
            correo.Body = mensaje;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
            //TODO. Las 2 lineas siguientes comentadas
            //smtp.Port = 587;
            //smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                ConfigurationManager.AppSettings["PasswordSMTP"]);
            if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

            smtp.Send(correo);
        }
        /// <summary>
        /// Permite enviar correo con copias oculta
        /// </summary>
        /// <param name="mailsTo"></param>
        /// <param name="mailsCc"></param>
        /// <param name="mailsBcc"></param>
        /// <param name="subject"></param>
        /// <param name="mensaje"></param>
        public static void SendEmail(List<string> mailsTo, List<string> mailsCc, List<string> mailsBcc, string subject, string mensaje, string sfrom)
        {
            MailMessage correo = new MailMessage();

            string remitente = string.Empty;

            if (!string.IsNullOrEmpty(sfrom))
            {
                remitente = sfrom.Trim();
            }
            else
            {
                remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
            }

            foreach (string mailTo in mailsTo)
                correo.To.Add(mailTo);
            foreach (string mailCc in mailsCc)
                correo.CC.Add(mailCc);
            foreach (string mailBcc in mailsBcc)
                correo.Bcc.Add(mailBcc);

            correo.From = new MailAddress(remitente);
            correo.Subject = subject;
            correo.Body = mensaje;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
            //TODO. Las 2 lineas siguientes comentadas
            //smtp.Port = 587;
            //smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                ConfigurationManager.AppSettings["PasswordSMTP"]);
            if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

            smtp.Send(correo);
        }
        /// <summary>
        /// Permite enviar correos incluyendo archivos adjuntos
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="subject"></param>
        /// <param name="file"></param>
        /// <param name="mensaje"></param>
        /// <param name="remitente"></param>
        public static void SendEmail(List<String> mails, string subject, string mensaje, string remitente,
            List<String> mailscc, List<String> files)
        {
            try
            {

                using (MailMessage correo = new MailMessage())
                {
                    correo.From = new MailAddress(remitente);
                    int contador = 0;
                    int contadorcc = 0;

                    foreach (string mail in mails)
                    {
                        if (!string.IsNullOrEmpty(mail))
                        {
                            correo.To.Add(mail);
                            contador++;
                        }
                    }

                    if (mailscc != null)
                    {
                        foreach (string mailcc in mailscc)
                        {
                            if (!string.IsNullOrEmpty(mailcc))
                            {
                                correo.CC.Add(mailcc);
                                contadorcc++;
                            }
                        }
                    }

                    if (contador > 0)
                    {
                        correo.Subject = subject;
                        correo.Body = mensaje;
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;
                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
                        //TODO. Las 2 lineas siguientes comentadas
                        //smtp.Port = 587;
                        //smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                            ConfigurationManager.AppSettings["PasswordSMTP"]);

                        //smtp.EnableSsl = true;


                        foreach (string file in files)
                        {
                            string archivo = file;
                            Attachment data = new Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet);
                            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file);
                            correo.Attachments.Add(data);
                            //data.Dispose();
                        }
                        if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                        smtp.Send(correo);

                    }
                }

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void SendEmail(List<string> mailsTo, List<string> mailsCc, List<string> mailsBcc, string subject, string mensaje, string sfrom, List<String> files)
        {
            try
            {
                using (MailMessage correo = new MailMessage())
                {
                    string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
                    if (sfrom.Trim().Length == 0)
                        correo.From = new MailAddress(remitente);
                    else
                    {
                        correo.From = new MailAddress(sfrom);
                    }

                    foreach (string mailTo in mailsTo)
                    {
                        if (mailTo.Contains("@"))
                            correo.To.Add(mailTo);
                    }
                    foreach (string mailCc in mailsCc)
                    {
                        if (mailCc.Contains("@"))
                            correo.CC.Add(mailCc);
                    }
                    foreach (string mailBcc in mailsBcc)
                    {
                        if (mailBcc.Contains("@"))
                            correo.Bcc.Add(mailBcc);
                    }

                    foreach (string file in files)
                    {
                        string archivo = file;
                        Attachment data = new Attachment(archivo, System.Net.Mime.MediaTypeNames.Application.Octet);
                        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(file);
                        correo.Attachments.Add(data);
                    }
                    correo.Subject = subject;
                    correo.Body = mensaje;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
                    //TODO. Las 2 lineas siguientes comentadas
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;

                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                        ConfigurationManager.AppSettings["PasswordSMTP"]);
                    if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                    smtp.Send(correo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public static void SendEmailNotificacion(List<string> mailsTo, string subject, string mensaje, string sfrom, string file, string fileName)
        {
            
            MailMessage correo = new MailMessage();
            string remitente = ConfigurationManager.AppSettings["MailFrom"].ToString();
            if (sfrom.Trim().Length == 0)
                correo.From = new MailAddress(remitente);
            else
            {
                correo.From = new MailAddress(sfrom);
            }

            foreach (string mailTo in mailsTo)
            {
                if (mailTo.Contains("@"))
                    correo.To.Add(mailTo);
            }

            string archivo = file;
            Attachment data = new Attachment(archivo, System.Net.Mime.MediaTypeNames.Application.Octet);
            data.Name = fileName;
            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            correo.Attachments.Add(data);

            correo.Subject = subject;
            correo.Body = mensaje;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["EmailServer"];
            //TODO. Las 2 lineas siguientes comentadas
            // smtp.Port = 587;
            //smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                ConfigurationManager.AppSettings["PasswordSMTP"]);
            if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

            smtp.Send(correo);
        }


        /// <summary>
        /// Permite remover las tildes de un texto
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static String RemoverTildes(String texto)
        {
            String stAux;

            stAux = texto.Replace('Á', 'A');
            stAux = stAux.Replace('É', 'E');
            stAux = stAux.Replace('Í', 'I');
            stAux = stAux.Replace('Ó', 'O');
            stAux = stAux.Replace('Ú', 'U');
            stAux = stAux.Replace('á', 'a');
            stAux = stAux.Replace('é', 'e');
            stAux = stAux.Replace('í', 'i');
            stAux = stAux.Replace('ó', 'o');
            stAux = stAux.Replace('ú', 'u');

            return stAux;
        }

        public static string ObtenerImagenExtension(string extension)
        {
            string image = string.Empty;
            switch (extension)
            {
                case "XLS":
                case "XLSX":
                    image = "excel.png";
                    break;
                case "DOC":
                case "DOCX":
                    image = "doc.png";
                    break;
                case "JPG":
                    image = "jpg.png";
                    break;
                case "PDF":
                    image = "pdf.png";
                    break;
            }
            return image;
        }

        public static int ObtenerNroSemanasxAnho(DateTime fecha, int dayOfWeek)
        {
            int nroSemana = 0;
            switch (dayOfWeek)
            {
                case 0:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
                    break;
                case 6:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
                    break;
            }
            return nroSemana;
        }

        public static int GenerarNroSemana(DateTime fecha, int dayOfWeek)
        {
            return ObtenerNroSemanasxAnho(fecha, dayOfWeek);
        }

        /// <summary>
        /// Valida el correo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidarEmail(string email)
        {
            //bool flag = true;
            //Regex regex = new Regex(@"/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/");
            //Match match = regex.Match(email);
            //if (!match.Success)
            //{
            //    flag = false;
            //}

            //return flag;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean EsNumero(string numString)
        {
            Boolean isNumber = false;
            long number1 = 0;
            bool canConvert = long.TryParse(numString, out number1);
            if (canConvert == true)
                isNumber = true;
            else
            {
                byte number2 = 0;
                canConvert = byte.TryParse(numString, out number2);
                if (canConvert == true)
                    isNumber = true;
                else
                {
                    double number3 = 0;

                    canConvert = double.TryParse(numString, out number3);
                    if (canConvert == true)
                        isNumber = true;

                }
            }
            return isNumber;
        }

        public static DateTime GetFechaYearMonth(string stFecha)
        {
            DateTime fecha = DateTime.MinValue;

            string stMes = stFecha.Substring(5, 3);
            switch (stMes)
            {
                case "Ene":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 1, 1);
                    break;
                case "Feb":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 2, 1);
                    break;
                case "Mar":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 3, 1);
                    break;
                case "Abr":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 4, 1);
                    break;
                case "May":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 5, 1);
                    break;
                case "Jun":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 6, 1);
                    break;
                case "Jul":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 7, 1);
                    break;
                case "Ago":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 8, 1);
                    break;
                case "Set":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 9, 1);
                    break;
                case "Oct":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 10, 1);
                    break;
                case "Nov":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 11, 1);
                    break;
                case "Dic":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 12, 1);
                    break;
            }
            return fecha;
        }

        public static int TotalSemanasEnAnho(int year, int dayOfWeek)
        {
            int nroSemana = 0;
            DateTime fecha = new DateTime(year, 12, 28);
            switch (dayOfWeek)
            {
                case 0:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
                    break;
                case 6:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
                    break;
            }
            return nroSemana;

        }

        /// <summary>
        /// funcion que devuelve la fecha del primer dia de la semana ingresado como parametro y año respectivamente
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <param name="opc"></param>
        /// <returns></returns>
        public static DateTime GenerarFecha(int year, int weekOfYear, int opc)
        {   // opc (0:Programado, 1: Cronológico)         
            DayOfWeek dayOfWeek = 0;
            if (opc == 0)
                dayOfWeek = DayOfWeek.Saturday;
            else
                dayOfWeek = DayOfWeek.Sunday;

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = dayOfWeek - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, CalendarWeekRule.FirstFourDayWeek, dayOfWeek);
            var weekNum = weekOfYear; if (firstWeek <= 1) { weekNum -= 1; }
            var result = firstMonday.AddDays(weekNum * 7);
            return result.AddDays(0);
        }

        /// <summary>
        /// funcion que devuelve la fecha del primer dia de la fecha recibida  en formaro MM-YYYY
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public static DateTime FormatFecha(String fechaInicial)
        {
            DateTime dfecha = DateTime.MinValue;
            int mes = Int32.Parse(fechaInicial.Substring(0, 2));
            int anho = Int32.Parse(fechaInicial.Substring(3, 4));
            dfecha = new DateTime(anho, mes, 1);

            //if (fechaInicial != null)
            //{
            //    dfecha = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //}           
            return dfecha;
        }
        /// <summary>
        /// Permite obtener el nombre del ícono que se mostrará
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ObtenerIcono(string tipo, string extension)
        {
            string icono = string.Empty;

            if (tipo == ConstantesBase.TipoFile)
            {
                switch (extension.ToLower())
                {
                    case ".pdf":
                        {
                            icono = "pdficon.png";
                            break;
                        }
                    case ".docx":
                        {
                            icono = "docicon.png";
                            break;
                        }
                    case ".doc":
                        {
                            icono = "docicon.png";
                            break;
                        }
                    case ".xlsx":
                        {
                            icono = "xlsicon.png";
                            break;
                        }
                    case ".xls":
                        {
                            icono = "xlsicon.png";
                            break;
                        }
                    case ".pptx":
                        {
                            icono = "ppticon.png";
                            break;
                        }
                    case ".ppt":
                        {
                            icono = "ppticon.png";
                            break;
                        }
                    case ".zip":
                        {
                            icono = "zipicon.gif";
                            break;
                        }
                    case ".rar":
                        {
                            icono = "zipicon.gif";
                            break;
                        }
                    case ".png":
                        {
                            icono = "imgicon.gif";
                            break;
                        }
                    case ".jpg":
                        {
                            icono = "imgicon.gif";
                            break;
                        }
                    case ".gif":
                        {
                            icono = "imgicon.gif";
                            break;
                        }
                    case ".csv":
                        {
                            icono = "csvicon.png";
                            break;
                        }
                    default:
                        {
                            icono = "defaulticon.png";
                            break;
                        }
                }
            }
            else
            {
                icono = "foldericon.gif";
            }
            return icono;
        }

        /// <summary>
        /// Convierte un texto a pascal case
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToPascalCase(string texto)
        {
            if (texto.Length > 1)
            {
                return texto[0].ToString().ToUpper() + texto.Substring(1, texto.Length - 1).ToLower();
            }
            else
            {
                return texto.ToUpper();
            }
        }


       

        #region INTERVENCIONES
        /// <summary>
        /// Permite enviar correos incluyendo archivos adjuntos (STREAM)
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="subject"></param>
        /// <param name="file"></param>
        /// <param name="mensaje"></param>
        /// <param name="remitente"></param>
        /// <param name="mailscc"></param>
        /// <param name="filesDescargados"></param>
        public static void SendEmailAttachDispose(List<String> mails, 
                                                  string subject, 
                                                  string mensaje, 
                                                  string remitente,
                                                  List<String> mailscc, 
                                                  List<Tuple<Stream, string>> filesDescargados)
        {
            try
            {

                MailMessage correo = new MailMessage();

                correo.From = new MailAddress(remitente);
                int contador = 0;
                int contadorcc = 0;

                foreach (string mail in mails)
                {
                    if (!string.IsNullOrEmpty(mail))
                    {
                        correo.To.Add(mail);
                        contador++;
                    }
                }

                if (mailscc != null)
                {
                    foreach (string mailcc in mailscc)
                    {
                        if (!string.IsNullOrEmpty(mailcc))
                        {
                            correo.CC.Add(mailcc);
                            contadorcc++;
                        }
                    }
                }

                if (contador > 0)
                {
                    correo.Subject = subject;
                    correo.Body = mensaje;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["EmailServer"];

                    //// ----------------------------------------------------------------------------------------
                    //// SOLO FUNCIONA COMO UNA CUENTA LIBRE Y NO CORPORATIVA - SE COMENTA CADA VEZ QUE SE PASA A
                    //// TEST O PRODUCCIÓN
                    //// ----------------------------------------------------------------------------------------
                    //smtp.EnableSsl = true;

                    //smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserNameSMTP"],
                    //    ConfigurationManager.AppSettings["PasswordSMTP"]);
                    //// ----------------------------------------------------------------------------------------

                    foreach (var documento in filesDescargados)
                    {

                        string archivo = documento.Item2; //nombre
                        Attachment data = new Attachment(documento.Item1, archivo, System.Net.Mime.MediaTypeNames.Application.Octet);
                        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                        //disposition.CreationDate = System.IO.File.GetCreationTime();
                        correo.Attachments.Add(data);
                    }
                    if (ConfigurationManager.AppSettings["EnableSslSMTP"] == "S") smtp.EnableSsl = true;

                    smtp.Send(correo);
                    correo.Attachments.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

    }
}
