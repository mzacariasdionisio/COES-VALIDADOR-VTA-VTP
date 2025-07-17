
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class RegistroIntegrantesHelper
    {
        /// <summary>
        /// Genera el cuerpo del mensaje de correo de solictud.
        /// </summary>
        /// <param name="representante">representante legal</param>
        /// <param name="empresa">empresa</param>
        /// <param name="solicitud">solicitud</param>      
        /// <param name="observacion">observación</param>
        /// <returns></returns>
        public static string Solicitud_BodyMailDenegado(string representante, string empresa, string solicitud, string observacion)
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
            strHtml.Append("        <p>Estimado Sr(a)" + representante + "</p>");
            strHtml.Append("        <p><b>Empresa: " + empresa + "</b></p>");
            strHtml.Append("        <p>El COES ha denegado la solicitud: " + solicitud + ", favor " +
                " de revisar la observación y si considera necesario volver a realizar la solicitud vía sistema.</p>");
            strHtml.Append("    <P>Observación:&nbsp;" + observacion + "</P>");
            strHtml.Append("    <P>Cualquier inquietud, sírvase en escribirnos al correo electrónico: amontalva@coes.org.pe</P>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p><p>T: +51 611 8585 – Anexo:  620</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el cuerpo del mensaje de correo de solictud.
        /// </summary>
        /// <param name="representante">representante legal</param>
        /// <param name="empresa">empresa</param>
        /// <param name="enlace">solicitud</param>  
        /// <returns></returns>
        public static string Solicitud_BodyMailAceptado(string representante, string empresa, string solicitud)
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
            strHtml.Append("        <p>Estimado Sr(a) " + representante + "</p>");
            strHtml.Append("        <p>Empresa: " + empresa + "</p>");
            strHtml.Append("        <p>Como resultado de la revisión de los documentos presentados a través de la Extranet del COES, su solicitud de:  " + solicitud + "</p>");
            strHtml.Append("        <p>Deberá de presentar a través de la plataforma de trámite documentario virtual del COES, en un plazo "+
            "no mayor de tres (3) días hábiles su solicitud aprobada, dirigida a la Dirección Ejecutiva del COES, adjuntando "+
            "todos los documentos digitalizados(PDF).</p>");
            strHtml.Append("        <p>Cualquier inquietud, sírvase en escribirnos al correo electrónico: amontalva@coes.org.pe</p>");
            strHtml.Append(" <p>Atentamente,</p><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p><p>T: +51 611 8585 – Anexo:  620</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el cuerpo del mensaje de correo de solictud.
        /// </summary>
        /// <param name="representante">representante legal</param>
        /// <param name="empresa">empresa</param>
        /// <param name="observacion">observación</param>
        /// <returns></returns>
        public static string Revision_BodyMailDenegado(string representante, string empresa, string enlace)
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
            strHtml.Append("        <p>Estimado Sr(a) " + representante + "</p>");
            strHtml.Append("        <p><b>Representante del trámite para la empresa " + empresa + "</b></p>");
            strHtml.Append("        <p>Como resultado de la revisión de la documentación presentada a través del formulario web, se identificó algunas observaciones, otorgándole un plazo máximo de siete (07) días calendarios para la subsanación correspondiente.</p>");
            strHtml.Append("        <p> El levantamiento de lo observado lo podrá realizar a través del enlace web:&nbsp;</p>");
            strHtml.Append("<a href='" + enlace + "'>Observaciones del registro</a>");
            strHtml.Append(" <p>Cualquier inquietud, sírvase en escribirnos al correo electrónico  registrointegrantes@coes.org.pe</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el cuerpo del mensaje de correo de solictud.
        /// </summary>
        /// <param name="representante">representante legal</param>
        /// <param name="empresa">empresa</param>        
        /// <returns></returns>
        public static string Revision_BodyMailAceptado(string representante, string empresa)
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
            strHtml.Append("        <p>Estimado Sr(a)" + representante + "</p>");
            strHtml.Append("        <p><b>Responsable del trámite para la empresa " + empresa + "</b></p>");
            strHtml.Append("        <p>Como resultado de la revisión de los documentos presentados por el portal web(pre inscripción), se le comunica que no"+
            "tiene observaciones; por lo tanto, deberá de presentar a través de la plataforma de trámite virtual, en un plazo "+
            "máximo de cinco(5) días hábiles, su solicitud de registro de integrante del COES mediante carta dirigida a la "+
            "Dirección Ejecutiva, adjuntando los documentos digitalizados (PDF) aprobados como lo establece como requisito el "+
            "Procedimiento Administrativo N° 16A</p>");
            strHtml.Append(" <p>Cualquier inquietud, sírvase en escribirnos al correo electrónico: registrointegrantes@coes.org.pe</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el cuerpo del mensaje de correo de enviar credenciales de la Extranet a los Representantes Legales
        /// </summary>
        /// <param name="representante">representante legal</param>
        /// <param name="empresa">empresa</param>     
        /// <param name="usuario">usuario extranet</param>
        /// <param name="contraseña">contraseña usuario extranet</param>
        /// <returns></returns>
        public static string Revision_BodyMailExtranet(string representante, string empresa, string usuario, string contraseña)
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
            strHtml.Append("        <p>Estimado Sr(a)" + representante + "</p>");
            strHtml.Append("        <p><b>Empresa: " + empresa + "</b></p>");
            strHtml.Append("        <p>En conformidad al proceso de inscripción como integrante del COES de acuerdo al Procedimiento Administrativo N°16A “Registro de Integrantes del COES, se les remite sus credenciales para el acceso en la plataforma <a href='http://wwww.coes.org.pe/extranet'>Extranet</a>:</p>");
            strHtml.Append("        <p>Usuario:&nbsp;" + usuario + "</p>");
            strHtml.Append("        <p>Contraseña:&nbsp; " + contraseña + "</p>");
            strHtml.Append("        <p>Cualquier inquietud, sírvase en escribirnos al correo electrónico: registrointegrantes@coes.org.pe</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Dirección Ejecutiva</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");



            return strHtml.ToString();
        }

        /// <summary>
        /// Permite obtener el cuerpo del correo para informar al personal COES
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public static string Solicitud_BodyMailAgente(string empresa, string solicitud)
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
            strHtml.Append("        <p>Estimados:  <br /><br /></p>");
            strHtml.Append("        <p>El Representante Legal de la empresa: " + empresa + " ");
            strHtml.Append("        ha registrado una solicitud de tipo   " + solicitud + " haciendo uso de la Extranet COES. <br /> Por favor revisar en la Intranet SGOCOES para atender dicha solicitud.</p>");            
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el cuerpo del mensaje de correo de solictud.
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="empresa">empresa</param>        
        /// <param name="correlativo">correlativo generado</param>
        /// <returns></returns>
        public static string Registro_BodyMailAceptado(string responsableTramite, string empresa, string correlativo)
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
            strHtml.Append("        <p>Estimado Sr(a) " + responsableTramite + "</p>");
            strHtml.Append("        <p><b>Responsable del trámite para la empresa " + empresa + "</b></p>");
            strHtml.Append("        <p>Se ha registrado su solicitud para la revisión de los documentos presentados a través del formulario web.</p>");
            strHtml.Append("        <p>Cabe mencionar, que el proceso de revisión de los documentos presentados tiene un plazo máximo de diez(10) días calendarios,");
            strHtml.Append(" en el caso que, el Agente no cumpliera con adjuntar correctamente los documentos solicitados, el COES enviará a través de correo electrónico ");
            strHtml.Append(" un enlace del formulario web, a fin de que subsane lo observado, otorgándole un plazo máximo de siete(07) días calendarios.</p>");


            strHtml.Append(" <p>Cualquier inquietud, sírvase en escribirnos al correo electrónico registrointegrantes@coes.org.pe</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }


        /// <summary>
        /// Genera el cuerpo del mensaje de correo de evento de alta de contacto
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="empresa">empresa</param>        
        /// <returns></returns>
        public static string Contacto_BodyMailAlta(string contacto, string empresa)
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
            strHtml.Append("        <p>Alta de contacto</p>");
            strHtml.Append("        <p><b>Empresa " + empresa + "</b></p>");
            strHtml.Append("        <p>Se ha agregado un contacto con nombre: " + contacto + "</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }



        /// <summary>
        /// Genera el cuerpo del mensaje de correo de evento de alta de contacto
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="empresa">empresa</param>        
        /// <returns></returns>
        public static string Contacto_BodyMailEdicion(string contacto, string empresa)
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
            strHtml.Append("        <p>Alta de contacto</p>");
            strHtml.Append("        <p><b>Empresa " + empresa + "</b></p>");
            strHtml.Append("        <p>Se ha editado el contacto con nombre: " + contacto + "</p>");
            strHtml.Append(" <p>Atentamente,</P><p>Sub Dirección de Gestión de Información</p><p>COES SINAC</p>");
            strHtml.Append("    </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        public static string Codifica(string cadena)
        {
            return BitConverter.ToString(Encoding.ASCII.GetBytes(Encriptacion.Encripta(cadena))).Replace("-", "");
        }

        public static string DeCodificar(string codificado)
        {
            var arr = new String[codificado.Length / 2];
            var j = 0;
            for (var i = 0; i < codificado.Length; i += 2)
            {
                arr[j] = codificado.Substring(i, 2);
                j++;
            }

            var array = new byte[arr.Length];
            for (var i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);

            var nuevoOrigen = System.Text.Encoding.ASCII.GetString(array);
            var desencriptado = Encriptacion.Desencripta(nuevoOrigen);
            return desencriptado;
        }



    }
}
