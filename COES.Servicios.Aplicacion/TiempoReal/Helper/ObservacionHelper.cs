using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using COES.Dominio.DTO.Scada;

namespace COES.Servicios.Aplicacion.TiempoReal.Helper
{
    public class ObservacionHelper
    {
        /// <summary>
        /// Lista de estados posibles
        /// </summary>
        public static string[] Estados = { "Pendiente", "Levantado", "Confirmado" };


        /// <summary>
        /// Codificacion de los estados
        /// </summary>
        public static string[] CodigoEstados = { "P", "L", "C" };

        /// <summary>
        /// Permite obtener el codigo de estado
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public static string ObtenerCodigoEstado(string estado)
        {
            if (!string.IsNullOrEmpty(estado))
            {
                int index = Array.IndexOf(Estados, estado);
                return CodigoEstados[index];
            }
            else
            {
                return CodigoEstados[0];
            }
        }

        /// <summary>
        /// Permite obtener la descripcion del estado
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public static string ObtenerDescripcionEstado(string estado)
        {
            int index = Array.IndexOf(CodigoEstados, estado);
            return Estados[index];
        }

        /// <summary>
        /// Permite obtener lista de estados
        /// </summary>
        /// <returns></returns>
        public static string[] ObtenerListaEstados(int agente)
        {
            if (agente == 1) return Estados.Take(2).ToArray();
            return Estados;
        }

        /// <summary>
        /// Permite estado global de la observación
        /// </summary>
        /// <param name="estados"></param>
        /// <returns></returns>
        public static string ObtenerEstadoGlobal(List<string> estados)
        {
            foreach (string estado in estados)
            {
                if (estado == CodigoEstados[0] || estado == CodigoEstados[1])
                {
                    return CodigoEstados[0];
                }
            }

            return CodigoEstados[2];
        }

        /// <summary>
        /// Permite verificar el estado pendiente
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public static string VerificarEdicion(string estado)
        {
            if (estado == CodigoEstados[2])
            {
                return ConstantesAppServicio.NO;
            }
            return ConstantesAppServicio.SI;
        }

        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="elmentos"></param>
        /// <returns></returns>
        public static string ObtenerCorreoNotificacion(List<TrObservacionItemDTO> listadoItems, int indicador)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' cellspacing = '0' cellpadding='2'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr style='background-color:#2980B9; color:#fff'>");
            strHtml.Append("<td>Zona</td>");
            strHtml.Append("<td>Nombre</td>");
            strHtml.Append("<td>ICCP</td>");
            strHtml.Append("<td>Estado</td>");
            strHtml.Append("<td>Comentario COES</td>");
            strHtml.Append("<td>Comentario Agente</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            foreach (TrObservacionItemDTO item in listadoItems)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", item.Zonanomb);
                strHtml.AppendFormat("<td>{0}</td>", item.Canalnomb);
                strHtml.AppendFormat("<td>{0}</td>", item.Canaliccp);
                strHtml.AppendFormat("<td>{0}</td>", item.Descestado);
                strHtml.AppendFormat("<td>{0}</td>", item.Obsitecomentario);
                strHtml.AppendFormat("<td>{0}</td>", (!string.IsNullOrEmpty(item.Obsitecomentarioagente)) ? item.Obsitecomentarioagente : "&nbsp;");

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            string texto = string.Empty;
            if (indicador == 0)
            {
                texto = "Se han identificado observaciones en las señales ICCP listadas a continuación:";
            }
            else if (indicador == 1)
            {
                texto = "Aún existen señales ICCP con observaciones pendientes de levantar:";
            }
            else if (indicador == 2)
            {
                texto = "Las señales ICCP observadas y que fueron revisadas por le empresa son las siguientes:";
            }

            string mensaje = @"
                <html>
                    <head>       
                        <style type='text/css'>
                        <!--
                        body
                        [
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-size:12px;
	                        top:0;
	                        left:0;
	                        background-color:#ffffff;	
                        ]
                        .celdacon
                        [
	                        color:#333333;
                            width:200px;
	                        font-size:11px;
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-weight:bold;
	                        line-height:25px;
	                        padding-left:20px;
                        ]                
                        -->
                        </style>
                    </head>
                    <body>
                        <table>
                            <tr>
                                <td><img src='http://www.coes.org.pe/wcoes/fondo_emailapp.png'></td>
                            </tr>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td><strong>Estimados Ingenieros:</strong> <br /><br /></td>			       
		                                </tr>
		                                <tr>			        
			                                <td>
                                                {1}
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	
                                    {0}
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            return String.Format(mensaje, strHtml.ToString(), texto);
        }

        /// <summary>
        /// Permite obtener el asunto del correo electronio
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerAsuntoNotificacion(string empresa, int indicador)
        {
            if (indicador == 0)
                return string.Format("Observaciones en señales ICCP de la empresa ", empresa);
            else if (indicador == 1)
                return string.Format("Aún existen señales ICCP pendientes de levantar de la empresa: ", empresa);
            else if (indicador == 2)
                return string.Format("Levantamiento de observaciones en señales ICCP de la empresa  ", empresa);


            return string.Empty;
        }

        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="elmentos"></param>
        /// <returns></returns>
        public static string ObtenerCorreoNotificacionAutomatico(List<TrObservacionItemDTO> listadoItems, string empresa)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' cellspacing = '0' cellpadding='2'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr style='background-color:#2980B9; color:#fff'>");
            strHtml.Append("<td>Zona</td>");
            strHtml.Append("<td>Nombre</td>");
            strHtml.Append("<td>ICCP</td>");
            strHtml.Append("<td>Estado</td>");
            strHtml.Append("<td>Fecha Empresa</td>");
            strHtml.Append("<td>Fecha COES</td>");
            strHtml.Append("<td>Comentario COES</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            foreach (TrObservacionItemDTO item in listadoItems)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", item.Zonanomb);
                strHtml.AppendFormat("<td>{0}</td>", item.Canalnomb);
                strHtml.AppendFormat("<td>{0}</td>", item.Canaliccp);
                strHtml.AppendFormat("<td>{0}</td>", "Pendiente");
                strHtml.AppendFormat("<td>{0}</td>", item.FechaEmpresa);
                strHtml.AppendFormat("<td>{0}</td>", item.FechaCoes);
                strHtml.AppendFormat("<td>{0}</td>", item.Obsitecomentario);

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            string mensaje = @"
                <html>
                    <head>       
                        <style type='text/css'>
                        <!--
                        body
                        [
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-size:12px;
	                        top:0;
	                        left:0;
	                        background-color:#ffffff;	
                        ]
                        .celdacon
                        [
	                        color:#333333;
                            width:200px;
	                        font-size:11px;
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-weight:bold;
	                        line-height:25px;
	                        padding-left:20px;
                        ]                
                        -->
                        </style>
                    </head>
                    <body>
                        <table>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td>Empresa <strong>&nbsp; {0}</strong> <br /><br />
                                                Al {1}, se han detectado señales que no cumplen con el ítem 4.2.3 de la &quot;Norma Técnica para el Intercambio de Información en Tiempo Real para la Operación del SEIN&quot;, 
                                                las cuales se listan a continuación. Por favor levantar cada una de las observaciones.
                                                <br />
                                                <br />
                                                <strong>Listado de Señales</strong>
                                                <br />
                                                <br />
                                            </td>			       
		                                </tr>
		                                <tr>			        
			                                <td>
                                                {2}
                                                <br />
                                                <br />
                                                Para poder realizar el levantamiento de observaciones, por favor ingresa al siguiente enlace <a href = 'http://www.coes.org.pe/extranet'> Extranet COES </a>
                                                <br />
                                                Puede descargar desde <a href = 'https://www.coes.org.pe/extranet/manuales/Manual_usuario_extranet_observaciones_se%C3%B1ales_iccp.pdf'> aquí </a> el Manual de Usuario del aplicativo.
                                                <br />
                                                Saludos cordiales,
                                                <br />      
                                                COES SINAC

                                                <br />
                                                <br />
                                                <strong>Nota:</strong> Este mensaje ha sido generado automáticamente.
                                                
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	                          
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            string fecha = String.Format("{0:MM/dd/yyyy}", DateTime.Now);

            return String.Format(mensaje, empresa, fecha, strHtml.ToString());
        }

        /// <summary>
        /// Permite obtener el asunto del correo electronio
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerAsuntoNotificacionAutomatico(string empresa)
        {
            return string.Format("Notificación para el levantamiento de observaciones en las señales ICCP - {0}", empresa);
        }


        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="elmentos"></param>
        /// <returns></returns>
        public static string ObtenerCorreoNotificacionIndiceDisponibilidad(string valor, string empresa)
        {

            string mensaje = @"
                <html>
                    <head>       
                        <style type='text/css'>
                        <!--
                        body
                        [
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-size:12px;
	                        top:0;
	                        left:0;
	                        background-color:#ffffff;	
                        ]
                        .celdacon
                        [
	                        color:#333333;
                            width:200px;
	                        font-size:11px;
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-weight:bold;
	                        line-height:25px;
	                        padding-left:20px;
                        ]                
                        -->
                        </style>
                    </head>
                    <body>
                        <table>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td>Empresa <strong>&nbsp; {0}</strong> <br /><br />
                                                Al {1}, se emite la siguiente información:
                                                <br />
                                                <br />
                                                Indice de disponibilidad porcentual:&nbsp; 
                                                <strong>{2}</strong>
                                                <br />
                                                <br />
                                            </td>			       
		                                </tr>
		                                <tr>			        
			                                <td>                                              
                                                <br />
                                                <br />
                                                Esta infomación también la puede consultar en la <a href = 'http://www.coes.org.pe/extranet'> Extranet COES </a>.
                                                <br />
                                                <br />
                                                Saludos cordiales,
                                                <br />      
                                                COES SINAC
                                                
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	                          
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            string fecha = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

            return String.Format(mensaje, empresa, fecha, valor);
        }

        /// <summary>
        /// Permite obtener el asunto del correo electronio
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerAsuntoNotificacionIndiceDisponibilidad(string empresa)
        {
            return string.Format("Notificación del Índice de disponibilidad porcentual - {0}", empresa);
        }

        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="elmentos"></param>
        /// <returns></returns>
        public static string ObtenerCorreoNotificacionCongelamientoSeñales(string empresa)
        {

            string mensaje = @"
                <html>
                    <head>       
                        <style type='text/css'>
                        <!--
                        body
                        [
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-size:12px;
	                        top:0;
	                        left:0;
	                        background-color:#ffffff;	
                        ]
                        .celdacon
                        [
	                        color:#333333;
                            width:200px;
	                        font-size:11px;
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-weight:bold;
	                        line-height:25px;
	                        padding-left:20px;
                        ]                
                        -->
                        </style>
                    </head>
                    <body>
                        <table>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td>Empresa <strong>&nbsp; {0}</strong> <br /><br />
                                                Al {1}, se notifica el siguiente evento:
                                                <br />
                                                <br />                                                
                                                <strong>Congelamiento de Señales ICCP, por favor revise.</strong>
                                                <br />
                                                <br />
                                            </td>			       
		                                </tr>
		                                <tr>			        
			                                <td>                                                                                                
                                                <br />
                                                <br />
                                                Saludos cordiales,
                                                <br />      
                                                COES SINAC
                                                
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	                          
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            string fecha = String.Format("{0:MM/dd/yyyy}", DateTime.Now);

            return String.Format(mensaje, empresa, fecha);
        }

        /// <summary>
        /// Permite obtener el asunto del correo electronio
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerAsuntoNotificacionCongelamientoSeñales(string empresa)
        {
            return string.Format("Notificación de Congelamiento de Señales - {0} ", empresa);
        }


        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerCorreoNotificacionCaidaEnlace(string empresa, string fechaDesconexion)
        {

            string mensaje = @"
                <html>
                    <head>       
                        <style type='text/css'>
                        <!--
                        body
                        [
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-size:12px;
	                        top:0;
	                        left:0;
	                        background-color:#ffffff;	
                        ]
                        .celdacon
                        [
	                        color:#333333;
                            width:200px;
	                        font-size:11px;
	                        font-family:Arial, Helvetica, sans-serif;
	                        font-weight:bold;
	                        line-height:25px;
	                        padding-left:20px;
                        ]                
                        -->
                        </style>
                    </head>
                    <body>
                        <table>
                            <tr>
                                <td>
	                                <table cellspacing='0' border='0' width='100%'>		
		                                <tr>
			                                <td>Empresa <strong>&nbsp; {0}</strong> <br /><br />
                                                Existe una observación por la caída en los enlaces de transmisión con el COES, por favor su atención para el levantamiento de la observación.
                                                <br />                                                
                                                La caida del enlace se produjo a las: {1}.
                                            </td>			       
		                                </tr>
		                                <tr>			        
			                                <td>                                              
                                                <br />
                                                <br />
                                                Para poder realizar el levantamiento de observaciones, por favor ingresa al siguiente enlace <a href = 'http://www.coes.org.pe/extranet'> Extranet COES </a>
                                                <br />
                                                <br />
                                                PD. Puede descargar desde <a href = 'https://www.coes.org.pe/extranet/manuales/Manual_usuario_extranet_observaciones_se%C3%B1ales_iccp.pdf'> aquí </a> el manual de usuario del aplicativo.
                                                <br />
                                                <br />

                                                Saludos cordiales,
                                                <br />      
                                                COES SINAC
                                                
                                            </td>
		                                </tr>      
	                                </table>	        
	                                <br/>	                          
	                            </td>
	                        </tr>
                        </table>
                    </body>
                </html>";

            return String.Format(mensaje, empresa, fechaDesconexion);
        }

        /// <summary>
        /// Permite obtener el asunto del correo electronio
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static string ObtenerAsuntoNotificacionCaidaEnlace(string empresa)
        {
            return string.Format("Notificación de Caida de Enlace - {0}", empresa);
        }
    }
}
