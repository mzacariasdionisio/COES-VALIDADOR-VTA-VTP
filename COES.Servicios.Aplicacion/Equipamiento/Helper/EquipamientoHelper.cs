using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Equipamiento.Helper
{
    public class EquipamientoHelper
    {
        public struct Html
        {

            public const string HtmlEquipo = @" <tr>
					               <td class='tdcelda'>{0}</td>
					               <td class='tdcelda'>{1}</td>
					               <td class='tdcelda'>{2}</td>
					               <td class='tdcelda'>{3}</td>
					               <td class='tdcelda'>{4}</td>
					               <td class='tdcelda'>{5}</td>							
				               </tr>";
            public const string HtmlPropiedad = @"<tr>
					                 <td class='tdcelda'>{0}</td>
					                 <td class='tdcelda'>{1}</td>
					                 <td class='tdcelda'>{2}</td>
					                 <td class='tdcelda'>{3}</td>
					                 <td class='tdcelda'>{4}</td>
					                 <td class='tdcelda'>{5}</td>	
                                     <td class='tdcelda'>{6}</td>	
				                 </tr>";
            public const string HtmlGrupo = @"<tr>
					              <td class='tdcelda'>{0}</td>
					              <td class='tdcelda'>{1}</td>
					              <td class='tdcelda'>{2}</td>
					              <td class='tdcelda'>{3}</td>
					              <td class='tdcelda'>{4}</td>	
                                  <td class='tdcelda'>{5}</td>	
				             </tr>";

            public const string HtmlCuerpo = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Informe equipamiento</title>
                            <style type='text/css'>
                            body[
	                            background-color:#EEF0F2;
	                            top:0;
	                            left:0;
	                            margin:0;
	                            font-family:Arial, Helvetica, sans-serif;
	                            font-size:12px;
	                            color:#333333;
                            ]
                            .content[
	                            width:80%;
	                            margin:auto;
                            ]

                            .titulo[
	                            font-size:16px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-align:center;
	                            padding:20px;
	                            text-transform:uppercase;
                            ]

                            .subtitulo[
	                            font-size:13px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-transform:uppercase;		
                            ]

                            .table[
	
	                            margin-bottom:20px;
                            ]

                            .trtitulo[
	                            background-color:#506DBE;
	                            color:#fff;
	                            font-weight:bold;
	                            text-align:center;
	                            line-height:20px;
	                            font-size:10px;
	                            text-transform:uppercase;
                            ]

                            .tdcelda[
	                            background-color:#fff;
	                            text-align:center;
	                            border:1px solid #DBDCDD;
	                            border-top:1px none;
	                            line-height:18px;
                                font-size:11px;
                            ]

                            </style>
                            </head>

                            <body>
                            <table class='content'>
	                            <tr>
		                            <td class='titulo'>Informe de cambios en Equipamiento</td>
	                            </tr>
	                            <tr>
		                            <td class='subtitulo'>Cambios en equipamiento</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Empresa</td>
					                            <td>Equipo</td>
					                            <td>Abreviatura</td>
					                            <td>Familia</td>
					                            <td>Usuario</td>
					                            <td>Fecha</td>		
				                            </tr>
				                            {0}				                          
			                            </table>
                                        <br />
                                        <br />
		                            </td>
	                            </tr>
	                            <tr>
		                            <td class='subtitulo'>Cambios en Propiedades de Equipos</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Empresa</td>
					                            <td>Propiedad</td>
					                            <td>Equipo</td>
					                            <td>Valor</td>
                                                <td>Vigencia a partir del</td>
					                            <td>Usuario</td>
					                            <td>Fecha</td>		
				                            </tr>	
                                            {1}
			                            </table>
                                        <br />
                                        <br />
		                            </td>
	                            </tr>
	                            <tr>
		                            <td class='subtitulo'>Cambios en fórmulas</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Grupo</td>
					                            <td>Concepto</td>
					                            <td>Fórmula</td>
                                                <td>Vigencia a partir del</td>
					                            <td>Usuario</td>
					                            <td>Fecha</td>
				                            </tr>
				                            {2}				                           
			                            </table>
		                            </td>
	                            </tr>
                            </table>

                            </body>
                            </html>
                            ";

            public const string HtmlCuerpoCurva = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Informe equipamiento</title>
                            <style type='text/css'>
                            body[
	                            background-color:#EEF0F2;
	                            top:0;
	                            left:0;
	                            margin:0;
	                            font-family:Arial, Helvetica, sans-serif;
	                            font-size:12px;
	                            color:#333333;
                            ]
                            .content[
	                            width:80%;
	                            margin:auto;
                            ]

                            .titulo[
	                            font-size:16px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-align:center;
	                            padding:20px;
	                            text-transform:uppercase;
                            ]

                            .subtitulo[
	                            font-size:13px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-transform:uppercase;		
                            ]

                            .table[
	
	                            margin-bottom:20px;
                            ]

                            .trtitulo[
	                            background-color:#506DBE;
	                            color:#fff;
	                            font-weight:bold;
	                            text-align:center;
	                            line-height:20px;
	                            font-size:10px;
	                            text-transform:uppercase;
                            ]

                            .tdcelda[
	                            background-color:#fff;
	                            text-align:center;
	                            border:1px solid #DBDCDD;
	                            border-top:1px none;
	                            line-height:18px;
                                font-size:11px;
                            ]

                            </style>
                            </head>

                            <body>
                            <table class='content'>
	                            <tr>
		                            <td class='titulo'>Informe de cambios en la Curva de Ensayo de Potencia</td>
	                            </tr>	                          
	                            <tr>
		                            <td class='subtitulo'>Los siguientes puntos de las coordenadas de las curvas de ensayo de potencia sufrieron cambios:</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Grupo</td>
					                            <td>Concepto</td>
					                            <td>Fórmula</td>
					                            <td>Usuario</td>
					                            <td>Fecha</td>
				                            </tr>
				                            {0}				                           
			                            </table>
		                            </td>
	                            </tr>
                            </table>

                            </body>
                            </html>
                            ";
        }

        /// <summary>
        /// Estilo Css para las filas de equipos
        /// </summary>
        /// <param name="sEstado"></param>
        /// <returns></returns>
        public static string EstiloEstado(string sEstado)
        {
            string estiloEstado = string.Empty;
            sEstado = sEstado != null ? sEstado.ToUpper().Trim() : string.Empty;
            switch (sEstado)
            {
                case "A":
                    break;
                case "P":
                    estiloEstado = "background-color: #ffff00;";
                    break;
                case "F":
                    estiloEstado = "background-color: #5b6ff9;color:#FFFFFF";
                    break;
                case "B":
                    estiloEstado = "background-color: #FFDDDD;";
                    break;
                case "X":
                    estiloEstado = "background-color: #A4A4A4;color:#FFFFFF";
                    break;
            }
            return estiloEstado;
        }

        public static string EstadoDescripcion(string areaestado)
        {
            string estadoDescripcion;
            switch (areaestado)
            {
                case "A":
                    estadoDescripcion = "Activo";
                    break;
                case "B":
                    estadoDescripcion = "Baja";
                    break;
                case "P":
                    estadoDescripcion = "Proyecto";
                    break;
                case "X":
                    estadoDescripcion = "Anulado";
                    break;
                case "F":
                    estadoDescripcion = "Fuera de COES";
                    break;
                default:
                    estadoDescripcion = "";
                    break;
            }
            return estadoDescripcion;
        }

        public static string SiNoDescripcion(string sValor)
        {
            string siNoDesc;
            switch (sValor.ToUpperInvariant().Trim())
            {
                case "S":
                    siNoDesc = "SI";
                    break;
                case "N":
                    siNoDesc = "NO";
                    break;
                default:
                    siNoDesc = "";
                    break;
            }
            return siNoDesc;
        }

        public static string ValorArchivoUrl(string sValor)
        {
            if (!string.IsNullOrEmpty(sValor))
                if (sValor.ToLowerInvariant().Contains("http"))
                {
                    return "Archivo";
                }
            return sValor;
        }
        public static string ConvertirEnLink(string Valor)
        {
            string link = string.Empty;
            if (string.IsNullOrEmpty(Valor) || string.IsNullOrWhiteSpace(Valor))
                return link;
            if (Valor.ToUpperInvariant().Trim().StartsWith("HTTP"))
            {
                link = "<a href='" + Valor + "' target='_blank'>Ver Archivo</a>";
                link = link.Replace("'", "\"");
                return link;
            }
            else
            {
                return Valor;
            }
            //HtmlString anchor = new HtmlString(link);
        }

    }

}
