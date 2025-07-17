using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Intranet.Areas.Ensayo.Helper
{
    public class EnsayoHelper
    {
        public static string BodyMailNuevoEnsayo(string usuario)
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
            strHtml.Append("        <p>Declaración Jurada de Ensayos de Potencia</p>");
            strHtml.Append("        <p>Estimado " + usuario + "</p>");
            strHtml.Append("        <p>El presente correo indica que ha enviado solicitud de un Ensayo de Potencia el cual se hara efectivo " +
                " dentro de un plazo de 72 horas. </p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información –SGI</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        public static string BodyMailAutorizaEnsayo(string usuario, string fecha)
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
            strHtml.Append("        <p>Declaración Jurada de Ensayos de Potencia</p>");
            strHtml.Append("        <p>Estimado " + usuario + "</p>");
            strHtml.Append("        <p>El presente correo indica que se ha autorizado su solicitud de un Ensayo de Potencia el cual se hara efectivo " +
                " el día : " + fecha + " </p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información –SGI</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }
    }
}