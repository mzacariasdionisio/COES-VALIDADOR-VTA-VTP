using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Despacho.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Intranet.Areas.Despacho.Helper
{
    public class GrupoDespachoHelper
    {
        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerArbolGrupo(List<PrGrupoDTO> list)
        {
            int idPadre = -1;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("[\n");

            List<PrGrupoDTO> listItem = list.Where(x => x.Grupopadre == idPadre).ToList();
            int contador = 0;
            foreach (PrGrupoDTO item in listItem)
            {
                string icono = ObtenerIcono(item.Grupotipo, (int)item.Catecodi);
                string clave = item.Grupocodi + "," + item.Catecodi;

                List<PrGrupoDTO> listHijos = list.Where(x => x.Grupopadre == item.Grupocodi).ToList();
                if (listHijos.Count > 0)
                {
                    strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "',  'title': '" + item.Gruponomb +
                        "' , 'expanded' : 'true', 'children':[\n");
                    strHtml.Append(ObtenerSubArbolGrupo(listHijos, list, "   "));
                    if (contador < listItem.Count - 1) strHtml.Append("   ]},\n");
                    else strHtml.Append("   ]}\n");
                }
                else
                {
                    if (contador < listItem.Count - 1)
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                            item.Gruponomb + "' },\n");
                    else
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "','title': '" +
                            item.Gruponomb + "'}\n");
                }
                contador++;
            }

            strHtml.Append("]");
            return strHtml.ToString();
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ObtenerSubArbolGrupo(List<PrGrupoDTO> list, List<PrGrupoDTO> listGeneral, string pad)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            foreach (PrGrupoDTO item in list)
            {
                string icono = ObtenerIcono(item.Grupotipo, (int)item.Catecodi);
                string clave = item.Grupocodi + "," + item.Catecodi;

                List<PrGrupoDTO> listHijos = listGeneral.Where(x => x.Grupopadre == item.Grupocodi).ToList();

                if (listHijos.Count > 0)
                {
                    strHtml.Append(pad + "    {'key': '" + clave + "' , 'icon': '" + icono + "', 'title': '" +
                        item.Gruponomb + "', 'children':[\n");
                    strHtml.Append(ObtenerSubArbolGrupo(listHijos, listGeneral, pad + "  "));
                    if (contador < list.Count - 1) strHtml.Append(pad + "    ]},\n");
                    else strHtml.Append(pad + "    ]}\n");
                }
                else
                {
                    if (contador < list.Count - 1)
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                           item.Gruponomb + "' },\n");
                    else
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                            item.Gruponomb + "' }\n");
                }
                contador++;
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite obtener el íconpo
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        private static string ObtenerIcono(string tipo, int categoria)
        {
            
            if (tipo == ConstantesDespacho.TipoTermico)
            {
                if (categoria == ConstantesDespacho.CategoriaCentralTermica)
                {
                    return "termica.png";
                }
                else if (categoria == ConstantesDespacho.CategoriaGrupoTermico)
                {
                    return "grupotermico.png";
                }
                else if (categoria == ConstantesDespacho.CategoriaModoTermico)
                {
                    return "modotermico.png";
                }
            }


            return "termica.png";
        }
    }
}