using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class HelperEvento
    {
        /// <summary>
        /// Permite obtener el mensaje del correo electrónico
        /// </summary>
        /// <returns></returns>
        public static string ObtenerMensajeEvento(int idEvento, string asunto, DateTime fechaEvento, string nombre)
        {                       
            string url = string.Format(ConfigurationManager.AppSettings[ConstantesEvento.RutaEnlaceExtranet], idEvento);

            string html = String.Format(@"
            <html>
            <head>       
            <style type='text/css'>            
            body
            [
	            font-family:Arial, Helvetica, sans-serif;
	            font-size:12px;
	            top:0;
	            left:0;	            	
            ]          
            .celda
            [
	            color:#4171A0;	           
	            font-family:Arial, Helvetica, sans-serif;
	            font-weight:bold;
	            line-height:20px;	            
            ]
            .celda a
            [
	            color:#FF6600;	            
	            font-family:Arial, Helvetica, sans-serif;	           
            ]            
            </style>
            </head>
            <body>
            <table width='700'>
                <tr>
                    <td style='text-align:center'><img src='{9}'></td>
                </tr>
                <tr>
                    <td class='celda'>
                        <br />
                        <br />
                        San Isidro, {0} de {1} del {2}<br /><br />

                        Señores integrantes del Sistema Interconectado Nacional:<br /><br />

                        Es grato dirigirme a ustedes para informarles que se produjo el Evento '{3}' a las {4} h del {5} y 
                        recordarles que los plazos definidos en los numerales 8.2.6 y 8.2.7 de la NTCOTRSI para el reporte de sus informes 
                        son 2,50 h para su informe preliminar y 60 h para su informe final de perturbaciones.<br /><br />

                        Para este fin, acceder a la Extranet del COES haciendo <a href='{7}'>clic aquí.</a><br /><br /><br />

                        Saludos cordiales<br />
                        <strong>{6}</strong><br />
                        <strong>CCO-COES</strong>
	                </td>
	            </tr>
            </table>
            </body>
            </html>
            ", DateTime.Now.Day, COES.Base.Tools.Util.ObtenerNombreMes(DateTime.Now.Month), DateTime.Now.Year, asunto,
             fechaEvento.ToString("HH:mm"), fechaEvento.ToString("dd.MM.yyyy"), nombre, url, ConfigurationManager.AppSettings["LogoCoes"].ToString());

            string mensaje = html.Replace('[', '{');
            mensaje = mensaje.Replace(']', '}');

            return mensaje;
        }

    }
}
