using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ValorizacionDiaria.Helper
{
    public class ValorizacionDiariaCorreoHelper
    {
        public static int EnviarCorreo(DateTime fecha, string email, bool estado)
        {
            int resultado = 0;
            if (email.Trim().Length > 0)
            {
                List<string> listaCorreo = new List<string>();
                listaCorreo.Add(email);

                List<string> listaCC = new List<string>();
                listaCC.Add("webapp@coes.org.pe");

                string msg = MailProcesoValorizacion(fecha, estado);
                string asunto = "Proceso de valorizacion Diaria";

                try
                {
                    Base.Tools.Util.SendEmail(listaCorreo, listaCC, asunto, msg);
                }
                catch (Exception ex)
                {
                    resultado = 1;
                }
            }

            return resultado;
        }

        public static string MailProcesoValorizacion(DateTime fecha, bool estado)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<html>");
            strHtml.Append("    <head><STYLE TYPE='text/css'>");
            strHtml.Append("    body {font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}");
            strHtml.Append("    .mail {width:500px;border-spacing:0;border-collapse:collapse;}");
            strHtml.Append("    .mail thead th {text-align: center;background: #417AA7;color:#ffffff}");
            strHtml.Append("    .mail tr td {border:1px solid #dddddd;}");
            strHtml.Append("    </style></head>");
            strHtml.Append("    <body>");
            if (estado)
            {
                strHtml.Append("        <P>Se ha Ejecutado satisfactoriamente el Proceso de Valorización Diaria para la fecha : " + fecha.ToString(ConstantesBase.FormatoFecha) + "</P>");
            }
            else
            {
                strHtml.Append("        <P>No se ha Ejecutado el Proceso de Valorización Diaria para la fecha : " + fecha.ToString(ConstantesBase.FormatoFecha) + "</P>");
            }
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }
    }
}
