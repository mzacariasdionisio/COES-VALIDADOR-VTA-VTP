using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Hidrologia.Helper
{
	public class HidrologiaHelper
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
								   <td class='tdcelda'>{6}</td>
								   <td class='tdcelda'>{7}</td>
					               <td class='tdcelda'>{8}</td>
					               <td class='tdcelda'>{9}</td>	
								   <td class='tdcelda'>{10}</td>
				               </tr>";

			public const string HtmlGrupo = @" <tr>
					               <td class='tdcelda'>{0}</td>
					               <td class='tdcelda'>{1}</td>
					               <td class='tdcelda'>{2}</td>
					               <td class='tdcelda'>{3}</td>
					               <td class='tdcelda'>{4}</td>
					               <td class='tdcelda'>{5}</td>							
								   <td class='tdcelda'>{6}</td>
								   <td class='tdcelda'>{7}</td>
					               <td class='tdcelda'>{8}</td>
					               <td class='tdcelda'>{9}</td>	
								   <td class='tdcelda'>{10}</td>
								   <td class='tdcelda'>{11}</td>
					               <td class='tdcelda'>{12}</td>
					               <td class='tdcelda'>{13}</td>
					               <td class='tdcelda'>{14}</td>
					               <td class='tdcelda'>{15}</td>							
								   <td class='tdcelda'>{16}</td>
								   <td class='tdcelda'>{17}</td>
				               </tr>";

			public const string HtmlCuerpo = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Informe mediciones</title>
                            <style type='text/css'>
                            body[
	                            background-color:#EEF0F2;
	                            top:0;
	                            left:0;
	                            margin:0;
	                            font-family:Arial, Helvetica, sans-serif;
	                            font-size:11px;
	                            color:#333333;
                            ]
                            .content[
	                            width:100%;
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
                                font-size:10px;
                            ]

                            </style>
                            </head>

                            <body>
                            <table class='content'>
	                            <tr>
		                            <td class='titulo'>Informe de cambios en Mediciones</td>
	                            </tr>
	                            <tr>
		                            <td class='subtitulo'>Cambios en Puntos de Medición</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Barra</td>
					                            <td>Ele</td>
					                            <td>Descripcion</td>
					                            <td>Equipo</td>
					                            <td>Grupo</td>
												<td>Empresa</td>
					                            <td>Tipo Punto</td>
												<td>Lectura</td>
												<td>Estado</td>
												<td>Usuario</td>
												<td>Fecha Modificación</td>
				                            </tr>
				                            {0}				                          
			                            </table>
                                        <br />
                                        <br />
		                            </td>
	                            </tr>	
								<tr>
		                            <td class='subtitulo'>Cambios en Grupos de Despacho</td>
	                            </tr>
	                            <tr>
		                            <td>
			                            <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                            <tr class='trtitulo'>
					                            <td>Código</td>
					                            <td>Grupo</td>
					                            <td>Abrev</td>
                                                <td>Padre</td>
					                            <td>Empresa</td>
					                            <td>Categoria</td>
												<td>F.Energía</td>
					                            <td>Tipo Grupo</td>
					                            <td>Generador RER</td>
                                                <td>Cód.Osinergmin</td>
					                            <td>Integrante</td>
					                            <td>Cogeneración</td>
												<td>NodoEnergetico</td>
					                            <td>ReservaFría</td>
                                                <td>Activo</td>
					                            <td>Estado</td>
												<td>Usuario Mod.</td>
												<td>Fecha Mod.</td>
				                            </tr>
				                            {1}				                           
			                            </table>
		                            </td>
	                            </tr>
                            </table>

                            </body>
                            </html>
                            ";

		}
	}

}
