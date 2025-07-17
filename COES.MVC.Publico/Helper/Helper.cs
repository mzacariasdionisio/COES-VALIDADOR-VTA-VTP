using COES.MVC.Publico.SeguridadServicio;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Servicio;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;

namespace COES.MVC.Publico.Helper
{
    public class Helper
    {
        /// <summary>
        /// Verifica si el usuario contiene el rol para acceso a la aplicación
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="rolCode"></param>
        /// <returns></returns>
        public static bool ValidarAccesoAdministrador(int userCode, int rolCode)
        {
            SeguridadServicioClient cliente = new SeguridadServicioClient();
            List<RolDTO> listRol = cliente.ObtenerRolPorUsuario(userCode).ToList();

            if (listRol.Where(x => x.RolCode == rolCode).Count() > 0) return true;
            return false;
        }

        /// <summary>
        /// Permite obtener el formulario dinámicamente
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerFormulario(List<WbBlobcolumnDTO> list)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<table class='form-busqueda' cellspacing='0' cellpadding='0'>");

            foreach (WbBlobcolumnDTO item in list)
            {
                str.Append("<tr>");

                str.Append(string.Format("<td class='form-label-busqueda'>{0}</td>", item.Columnshow));
                str.Append("<td class='form-control-busqueda'>");

                if (item.Typecodi == TiposDeDato.Numero)
                {
                    str.Append(string.Format("<input type='text' id='campo{0}' />", item.Columncodi));
                }
                if (item.Typecodi == TiposDeDato.Entero)
                {
                    str.Append(string.Format("<input type='text' id='campo{0}'  />", item.Columncodi));
                }
                if (item.Typecodi == TiposDeDato.Fecha)
                {
                    str.Append(string.Format("<input type='text' id='campo{0}' class='formulario-item-fecha' />", item.Columncodi));
                }
                if (item.Typecodi == TiposDeDato.ListaDesplegable)
                {
                    str.Append(string.Format("<select id='campo{0}'>", item.Columncodi));
                    str.Append("<option value=''>-SELECCIONE-</option>");

                    foreach (WbColumnitemDTO option in item.ListaItems)
                    {
                        str.Append(string.Format("<option value='{0}'>{0}</option>", option.Itemvalue));
                    }

                    str.Append("</select>");
                }

                if (item.Typecodi == TiposDeDato.Texto)
                {
                    str.Append(string.Format("<input type='text' id='campo{0}' />", item.Columncodi));
                }
                if (item.Typecodi == TiposDeDato.TextoLargo)
                {
                    str.Append(string.Format("<textarea id='campo{0}'></textarea> ", item.Columncodi));
                }
                str.Append("</td>");

                str.Append("</tr>");
            }

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerNewMenuSalaPrensa(List<WbMenuDTO> list, string nodos)
        {
            int idPadre = 1;
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<div class=\"row\">");
            strHtml.AppendLine("");
            List<WbMenuDTO> listItem = list.Where(x => x.Padrecodi == idPadre).ToList();
            int contador = 0;
            foreach (WbMenuDTO item in listItem)
            {
                    strHtml.Append("<div class=\"col-12 col-md-6 col-lg-3\">");
                    strHtml.AppendLine("");
               
                List<WbMenuDTO> listHijos = list.Where(x => x.Padrecodi == item.Menucodi).ToList();
                if (listHijos.Count > 0)
                {
                    strHtml.Append("<div class=\"sitemap-items pb-3\">");
                    strHtml.AppendLine("");
                    strHtml.AppendFormat("<h3 class=\"coes-box--title pb-2 text-uppercase\">{0}</h3>", item.Menutitle);
                    strHtml.AppendLine("");
                    strHtml.Append("<ul class=\"ps-3\">");
                    foreach (var itemHijos in listHijos)
                    {
                        if (itemHijos.Menuurl != "#")
                        {
                            strHtml.AppendFormat("<li><a href='../{0}'>{1}</a></li>", itemHijos.Menuurl, itemHijos.Menutitle);
                            strHtml.AppendLine("");
                        }
                        else
                        {
                            strHtml.Append("<li>");
                            strHtml.AppendLine("");
                            strHtml.Append(ObtenerNewSubMenu(itemHijos, list, "   ", nodos));
                            strHtml.Append("</li>");
                            strHtml.AppendLine("");
                        }
                    }
                    strHtml.Append("</ul>");
                    strHtml.AppendLine("");
                    strHtml.Append("</div>");
                    strHtml.AppendLine("");
                }
            
               
                    strHtml.Append("</div>");
                    strHtml.AppendLine("");
               
                contador++;
            }

            strHtml.Append("</div>");
            strHtml.AppendLine("");
            return strHtml.ToString();
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static string ObtenerNewSubMenu(WbMenuDTO itemHijos, List<WbMenuDTO> listGeneral, string pad, string nodos)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            List<WbMenuDTO> listHijos = listGeneral.Where(x => x.Padrecodi == itemHijos.Menucodi).ToList();
            strHtml.AppendFormat("<a class=\"coes-collapse-item\" data-bs-toggle=\"collapse\" href=\"#collapseItem{0}\" role=\"button\" aria-expanded=\"false\" aria-controls=\"collapseExample\">{1}</a>", itemHijos.Menucodi, itemHijos.Menutitle);
            strHtml.AppendLine("");
            strHtml.AppendFormat("<div class=\"collapse\" id=\"collapseItem{0}\">", itemHijos.Menucodi);
            strHtml.AppendLine("");
            strHtml.Append("<ul class='pt-2'>");
            strHtml.AppendLine("");
            foreach (WbMenuDTO itemSubHijos in listHijos)
            {
                strHtml.AppendFormat("<li><a href='../{0}'>{1}</a></li>", itemSubHijos.Menuurl, itemSubHijos.Menutitle);
                strHtml.AppendLine("");
              
                contador++;
            }
            strHtml.Append("</ul>");
            strHtml.AppendLine("");
            strHtml.Append("</div>");
            strHtml.AppendLine("");

            return strHtml.ToString();
        }


        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerTreeOpciones(List<WbMenuDTO> list, string nodos)
        {
            int idPadre = 1;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("[\n");

            List<WbMenuDTO> listItem = list.Where(x => x.Padrecodi == idPadre).ToList();
            int contador = 0;
            foreach (WbMenuDTO item in listItem)
            {
                List<WbMenuDTO> listHijos = list.Where(x => x.Padrecodi == item.Menucodi).ToList();
                if (listHijos.Count > 0)
                {
                    item.Menuname = "0$" + item.Menuurl;

                    strHtml.Append("   {'key': '" + item.Menuname + "', 'title': '" + item.Menutitle +
                        "' , 'expanded' : 'true', selected : " + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + ", 'children':[\n");
                    strHtml.Append(ObtenerSubMenu(listHijos, list, "   ", nodos));
                    if (contador < listItem.Count - 1) strHtml.Append("   ]},\n");
                    else strHtml.Append("   ]}\n");
                }
                else
                {
                    item.Menuname = "1$" + item.Menuurl;
                    if (item.Menutype == "E") item.Menuname = "2$" + item.Menuurl;

                    if (contador < listItem.Count - 1)
                        strHtml.Append("   {'key': '" + item.Menuname + "', 'title': '" +
                            item.Menutitle + "', 'icon': 'application.png' , selected : " + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.Menuname + "', 'title': '" +
                            item.Menutitle + "', 'icon': 'application.png',  selected : " + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + "}\n");
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
        static string ObtenerSubMenu(List<WbMenuDTO> list, List<WbMenuDTO> listGeneral, string pad, string nodos)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            foreach (WbMenuDTO item in list)
            {
                List<WbMenuDTO> listHijos = listGeneral.Where(x => x.Padrecodi == item.Menucodi).ToList();

                if (listHijos.Count > 0)
                {
                    item.Menuname = "0$" + item.Menuurl;
                    strHtml.Append(pad + "    {'key': '" + item.Menuname + "', selected :" + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + ", 'title': '" +
                        item.Menudesc + "', 'children':[\n");
                    strHtml.Append(ObtenerSubMenu(listHijos, listGeneral, pad + "  ", nodos));
                    if (contador < list.Count - 1) strHtml.Append(pad + "    ]},\n");
                    else strHtml.Append(pad + "    ]}\n");
                }
                else
                {
                    item.Menuname = "1$" + item.Menuurl;
                    if (item.Menutype == "E") item.Menuname = "2$" + item.Menuurl;
                    if (contador < list.Count - 1)
                        strHtml.Append("   {'key': '" + item.Menuname + "', 'title': '" +
                           item.Menutitle + "' , 'icon': 'application.png', selected : " + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + "},\n");
                    else
                        strHtml.Append("   {'key': '" + item.Menuname + "', 'title': '" +
                            item.Menutitle + "', 'icon': 'application.png', selected : " + ObtieneSeleccionNodo(item.Menucodi, item.Selected, nodos) + "}\n");
                }
                contador++;
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite verificar la selección de un nodo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        /// <param name="nodos"></param>
        /// <returns></returns>
        static string ObtieneSeleccionNodo(int id, int selected, string nodos)
        {
            string[] nodes = nodos.Split(',');

            if (nodes.Contains(id.ToString()) || selected > 0)
            {
                return "true";
            }
            return "false";
        }

        /// <summary>
        /// Permite armar el menu del portal
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerMenuPrincipal(List<WbMenuDTO> list, string siteRoot)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<ul class='navbar-nav justify-content-between'>");
            str.AppendLine("");
            foreach (var item in list.Where(x => x.Padrecodi == 1))
            {
                str.Append("    <li class='nav-item dropdown'>");
                str.AppendLine("");
                str.AppendFormat("        <a class='nav-link dropdown-toggle' id='navbarDropdownMenuLink' role='button' data-bs-toggle='dropdown' aria-expanded='false' href='#'>{0}</a>", item.Menutitle);
                str.AppendLine("");
                var subList = list.Where(x => x.Padrecodi == item.Menucodi);

                if (subList.Count() > 0)
                {
                    //
                    //str.Append("        <div class='sub-nav'>");
                    str.AppendLine("");


                    var columns = subList.Select(x => (int)x.Menucolumn).Distinct().OrderBy(x => x);
                    str.Append("            <ul class='dropdown-menu' aria-labelledby='navbarDropdownMenuLink'>");
                    str.AppendLine("");
                    foreach (var i in columns)
                    {
                        var groups = subList.Where(x => x.Menucolumn == i).OrderBy(x => x.Menuorden).ToList();
                        foreach (var group in groups)
                        {
                            if (group.Menuurl != "#")
                            {
                                if (group.Menutype == "I")
                                {
                                    str.AppendFormat("                <li class='nav-item'><a class='nav-link bold' href='{0}'>{1}</a></li>", siteRoot + group.Menuurl, group.Menutitle);
                                    str.AppendLine("");
                                }
                                else
                                {
                                    str.AppendFormat("                <li class='nav-item'><a class='nav-link bold' href='{0}' target='_blank'>{1}</a></li>", group.Menuurl, group.Menutitle);
                                    str.AppendLine("");
                                }
                            }
                            else
                            {
                                str.AppendFormat("                <li class='nav-item'><a class='dropdown-item'>{0}</a></li>", group.Menutitle);
                                
                                str.AppendLine("");
                            }

                            var hijos = list.Where(x => x.Padrecodi == group.Menucodi).ToList();
                            if (hijos.Count > 0)
                            {
                                str.Append("                <li class='nav-item'>");
                                str.AppendLine("");
                                str.Append("                    <ul>");
                                str.AppendLine("");
                                foreach (var itemHijo in hijos)
                                {
                                    var subhijos = list.Where(x => x.Padrecodi == itemHijo.Menucodi).ToList();

                                    if (subhijos.Count > 0)
                                    {
                                        if (itemHijo.Menuurl != "#")
                                        {

                                            if (itemHijo.Menutype == "I")
                                            {
                                                str.AppendFormat("                <li><a href='{0}'><strong>{1}</strong></a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                str.AppendLine("");
                                            }
                                            else
                                            {
                                                str.AppendFormat("                <li><a  href='{0}' target='_blank'><strong>{1}</strong></a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                str.AppendLine("");
                                            }
                                        }
                                        else
                                        {
                                            str.AppendFormat("                <li class='nav-item'><a class='dropdown-item'>{0}</a></li>", itemHijo.Menutitle);
                                        }

                                        str.Append(ObtenerMenuPrincipalRecursivo(subhijos, list, siteRoot));
                                    }
                                    else
                                    {

                                        if (itemHijo.Menuurl != "#")
                                        {
                                            if (itemHijo.Menutype == "I")
                                            {
                                                str.AppendFormat("                <li class='nav-item'><a class='nav-link' href='{0}'>{1}</a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                str.AppendLine("");
                                            }
                                            else
                                            {
                                                str.AppendFormat("                <li class='nav-item'><a class='nav-link' href='{0}' target='_blank'>{1}</a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                str.AppendLine("");
                                            }
                                        }
                                    }
                                }
                                str.Append("                    </ul>");
                                str.AppendLine("");
                                str.Append("                </li>");
                                str.AppendLine("");
                            }
                        }
                    }
                    str.Append("            </ul>");
                    str.AppendLine("");
                    //str.Append("        </div>");

                }

                //
                str.AppendLine("");
                str.Append("    </li>");
                str.AppendLine("");
            }

            str.Append("</ul>");
            str.AppendLine("");
            return str.ToString();
        }
        public static string ObtenerMenuPrincipalDesktop(List<WbMenuDTO> list, string siteRoot)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<div class='menu'>");
            str.Append("<ul class='menu-vertical'>");
            str.AppendLine("");
            foreach (var item in list.Where(x => x.Padrecodi == 1))
            {
                str.Append("    <li>");
                str.AppendLine("");
                str.AppendFormat("        <a href='#' class='menu__item' id='item_1'>{0}<div class='icon-menu-principal'></div></a>", item.Menutitle);
                str.AppendLine("");
                var subList = list.Where(x => x.Padrecodi == item.Menucodi);
                int cantidadItems = 0;
                int cantidadItemsSeccion02 = 0;

                if (subList.Count() > 0)
                {
                    //
                    //str.Append("        <div class='sub-nav'>");
                    str.AppendLine("");


                    var columns = subList.Select(x => (int)x.Menucolumn).Distinct().OrderBy(x => x);
                   
                    str.Append("            <ul class='submenu-contenedor " + (columns.Count() == 1 ? "columns-1" : "columns-2") + "'>");
                    str.AppendLine("");
                    str.Append("    <div class='submenu-body'>");
                    str.AppendLine("");
                  
                    foreach (var i in columns)
                    {
                    
                        var groups = subList.Where(x => x.Menucolumn == i).OrderBy(x => x.Menuorden).ToList();

                        if (i == 1)
                        {
                            str.Append("    <div class='submenu-seccion-01'>");
                            str.AppendLine("");
                            foreach (var group in groups)
                            {

                                cantidadItems++;
                                if (group.Menuurl != "#")
                                {
                                    if (group.Menutype == "I")
                                    {
                                        str.AppendFormat("                <li><a class='titulo-submenu' href='{0}'>{1}</a></li>", siteRoot + group.Menuurl, group.Menutitle);
                                        str.AppendLine("");
                                    }
                                    else
                                    {
                                        str.AppendFormat("                <li><a class='titulo-submenu' href='{0}' target='_blank'>{1}</a></li>", group.Menuurl, group.Menutitle);
                                        str.AppendLine("");
                                    }
                                }
                                else
                                {
                                    str.AppendFormat("                <li><a href='#' class='titulo-submenu' >{0}</a></li>", group.Menutitle);

                                    str.AppendLine("");
                                }

                                var hijos = list.Where(x => x.Padrecodi == group.Menucodi).ToList();
                                if (hijos.Count > 0)
                                {
                                    //str.Append("                <li class='nav-item'>");
                                    //str.AppendLine("");
                                    str.Append("                    <ul>");
                                    str.AppendLine("");
                                    foreach (var itemHijo in hijos)
                                    {
                                        cantidadItems++;

                                        var subhijos = list.Where(x => x.Padrecodi == itemHijo.Menucodi).ToList();

                                        if (subhijos.Count > 0)
                                        {
                                            if (itemHijo.Menuurl != "#")
                                            {

                                                if (itemHijo.Menutype == "I")
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}'><strong>{1}</strong></a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                                else
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}' target='_blank'><strong>{1}</strong></a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                            }
                                            else
                                            {
                                                str.AppendFormat("                <li ><a class='titulo-submenu'>{0}</a></li>", itemHijo.Menutitle);
                                            }

                                            str.Append(ObtenerMenuPrincipalRecursivoDesktop(subhijos, list, siteRoot));
                                        }
                                        else
                                        {

                                            if (itemHijo.Menuurl != "#")
                                            {
                                                if (itemHijo.Menutype == "I")
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}'>{1}</a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                                else
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}' target='_blank'>{1}</a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                            }
                                        }
                                    }
                                    str.Append("                    </ul>");
                                    str.AppendLine("");
                                    //str.Append("                </li>");
                                    //str.AppendLine("");
                                }


                            }
                            str.Append("    </div>");
                            str.AppendLine("");
                        }

                        if (i == 2)
                        {
                            str.Append("    <div class='submenu-seccion-02'>");
                            str.AppendLine("");
                            foreach (var group in groups)
                            {

                                cantidadItems++;
                                if (group.Menuurl != "#")
                                {
                                    if (group.Menutype == "I")
                                    {
                                        str.AppendFormat("                <li><a class='titulo-submenu' href='{0}'>{1}</a></li>", siteRoot + group.Menuurl, group.Menutitle);
                                        str.AppendLine("");
                                    }
                                    else
                                    {
                                        str.AppendFormat("                <li><a class='titulo-submenu' href='{0}' target='_blank'>{1}</a></li>", group.Menuurl, group.Menutitle);
                                        str.AppendLine("");
                                    }
                                }
                                else
                                {
                                    str.AppendFormat("                <li><a href='#' class='titulo-submenu' >{0}</a></li>", group.Menutitle);

                                    str.AppendLine("");
                                }

                                var hijos = list.Where(x => x.Padrecodi == group.Menucodi).ToList();
                                if (hijos.Count > 0)
                                {
                                    //str.Append("                <li class='nav-item'>");
                                    //str.AppendLine("");
                                    str.Append("                    <ul>");
                                    str.AppendLine("");
                                    foreach (var itemHijo in hijos)
                                    {
                                        cantidadItems++;

                                        var subhijos = list.Where(x => x.Padrecodi == itemHijo.Menucodi).ToList();

                                        if (subhijos.Count > 0)
                                        {
                                            if (itemHijo.Menuurl != "#")
                                            {

                                                if (itemHijo.Menutype == "I")
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}'><strong>{1}</strong></a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                                else
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}' target='_blank'><strong>{1}</strong></a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                            }
                                            else
                                            {
                                                str.AppendFormat("                <li ><a >{0}</a></li>", itemHijo.Menutitle);
                                            }

                                            str.Append(ObtenerMenuPrincipalRecursivo(subhijos, list, siteRoot));
                                        }
                                        else
                                        {

                                            if (itemHijo.Menuurl != "#")
                                            {
                                                if (itemHijo.Menutype == "I")
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}'>{1}</a></li>", siteRoot + itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                                else
                                                {
                                                    str.AppendFormat("                <li ><a  href='{0}' target='_blank'>{1}</a></li>", itemHijo.Menuurl, itemHijo.Menutitle);
                                                    str.AppendLine("");
                                                }
                                            }
                                        }
                                    }
                                    str.Append("                    </ul>");
                                    str.AppendLine("");
                                    //str.Append("                </li>");
                                    //str.AppendLine("");
                                }


                            }
                            str.Append("    </div>");
                            str.AppendLine("");
                        }
                    }
                   
                
                    str.Append("            </ul>");
                    str.AppendLine("");
                    //str.Append("        </div>");

                }

                //
                str.AppendLine("");
                str.Append("    </li>");
                str.AppendLine("");
            }

            str.Append("</ul>");
            str.Append("</div>");
            str.AppendLine("");
            return str.ToString();
        }
        /// <summary>
        /// Permite armar el menu en modo recursivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listGeneral"></param>
        /// <param name="siteRoot"></param>
        /// <returns></returns>
        private static string ObtenerMenuPrincipalRecursivoDesktop(List<WbMenuDTO> list, List<WbMenuDTO> listGeneral, string siteRoot)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("                <li>");
            strHtml.AppendLine("");
            strHtml.Append("                    <ul>");
            strHtml.AppendLine("");

            foreach (WbMenuDTO item in list)
            {
                List<WbMenuDTO> listHijos = listGeneral.Where(x => x.Padrecodi == item.Menucodi).ToList();

                if (listHijos.Count > 0)
                {
                    if (item.Menuurl != "#")
                    {
                        if (item.Menutype == "I")
                        {
                            strHtml.AppendFormat("                      <li ><a  href='{0}'><strong>{1}</strong></a></li>", siteRoot + item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                        else
                        {
                            strHtml.AppendFormat("                      <li ><a  href='{0}' target='_blank'><strong>{1}</strong></a></li>", item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                    }
                    else
                    {
                        strHtml.AppendFormat("                <li ><a >{0}</a></li>", item.Menutitle);
                    }

                    strHtml.Append(ObtenerMenuPrincipalRecursivoDesktop(listHijos, listGeneral, siteRoot));
                }
                else
                {
                    if (item.Menuurl != "#")
                    {
                        if (item.Menutype == "I")
                        {
                            strHtml.AppendFormat("                      <li ><a  href='{0}'>{1}</a></li>", siteRoot + item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                        else
                        {
                            strHtml.AppendFormat("                      <li ><a href='{0}' target='_blank'>{1}</a></li>", item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                    }
                    else
                    {
                        strHtml.AppendFormat("                <li ><a >{0}</a></li>", item.Menutitle);
                    }
                }
            }

            strHtml.Append("                    </ul>");
            strHtml.AppendLine("");
            strHtml.Append("                </li>");
            strHtml.AppendLine("");
            return strHtml.ToString();
        }

        /// <summary>
        /// Permite armar el menu en modo recursivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listGeneral"></param>
        /// <param name="siteRoot"></param>
        /// <returns></returns>
        private static string ObtenerMenuPrincipalRecursivo(List<WbMenuDTO> list, List<WbMenuDTO> listGeneral, string siteRoot)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("                <li class='nav-item'>");
            strHtml.AppendLine("");
            strHtml.Append("                    <ul>");
            strHtml.AppendLine("");

            foreach (WbMenuDTO item in list)
            {
                List<WbMenuDTO> listHijos = listGeneral.Where(x => x.Padrecodi == item.Menucodi).ToList();

                if (listHijos.Count > 0)
                {
                    if (item.Menuurl != "#")
                    {
                        if (item.Menutype == "I")
                        {
                            strHtml.AppendFormat("                      <li class='nav-item'><a class='nav-link' href='{0}'><strong>{1}</strong></a></li>", siteRoot + item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                        else
                        {
                            strHtml.AppendFormat("                      <li class='nav-item'><a class='nav-link' href='{0}' target='_blank'><strong>{1}</strong></a></li>", item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                    }
                    else
                    {
                        strHtml.AppendFormat("                <li class='nav-item'><a class='dropdown-item'>{0}</a></li>", item.Menutitle);
                    }

                    strHtml.Append(ObtenerMenuPrincipalRecursivo(listHijos, listGeneral, siteRoot));
                }
                else
                {
                    if (item.Menuurl != "#")
                    {
                        if (item.Menutype == "I")
                        {
                            strHtml.AppendFormat("                      <li class='nav-item'><a class='nav-link' href='{0}'>{1}</a></li>", siteRoot + item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                        else
                        {
                            strHtml.AppendFormat("                      <li class='nav-item'><a class='nav-link' href='{0}' target='_blank'>{1}</a></li>", item.Menuurl, item.Menutitle);
                            strHtml.AppendLine("");
                        }
                    }
                    else
                    {
                        strHtml.AppendFormat("                <li class='nav-item'><a class='dropdown-item'>{0}</a></li>", item.Menutitle);
                    }
                }
            }

            strHtml.Append("                    </ul>");
            strHtml.AppendLine("");
            strHtml.Append("                </li>");
            strHtml.AppendLine("");
            return strHtml.ToString();
        }
    }
}