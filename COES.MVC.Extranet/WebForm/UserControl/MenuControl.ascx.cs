using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010.UserControl
{
    public partial class MenuControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Lista de opciones de la aplicación
        /// </summary>
        public List<OptionDTO> ListaOpciones = new List<OptionDTO>();


        private int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        /// <summary>
        /// Evento de carga del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                n_app dao = (n_app)Session["in_app"];
                this.ListaOpciones = (new COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient()).ObtenerOpcionPorUsuario(this.IdAplicacion,
                    dao.is_UserLogin).ToList();

                try
                {
                    if (((UserDTO)Session[DatosSesion.SesionUsuario]).EmprCodi == 10494) //Se asume el código de Osignermin => 10494
                    {
                        OptionDTO optionFormato = this.ListaOpciones.SingleOrDefault(x => x.OptionCode == 282);
                        if (optionFormato != null)
                        {
                            this.ListaOpciones.Remove(optionFormato);
                        }
                    }

                }
                catch (Exception ex) { }

                this.LlenarOpciones(1, null, this.nav);
            }
        }

        /// <summary>
        /// Carga en el treview, las opciones del sistema
        /// </summary>
        /// <param name="codigoPadre"></param>
        /// <param name="node"></param>
        /// <param name="tree"></param>
        public void LlenarOpciones(int codigoPadre, MenuItem node, Menu tree)
        {
            List<OptionDTO> treeItemsChild = this.ListaOpciones.Where(x => x.PadreCodi == codigoPadre).ToList();
            string urlbase = ConfigurationManager.AppSettings["InitialUrl"].ToString() + "/";

            n_app dao = (n_app)Session["in_app"];

            foreach (OptionDTO nodo in treeItemsChild)
            {
                MenuItem newNode = new MenuItem(nodo.OptionName, nodo.OptionCode.ToString());
                
                if (nodo.IndTipo == "W")
                    if (nodo.DesUrl != "#") newNode.NavigateUrl = urlbase + nodo.DesUrl; //hacer ajuste

                if (nodo.IndTipo == "F")
                {
                    if (nodo.DesArea != null)
                        nodo.DesController = nodo.DesArea + "/" + nodo.DesController;

                    if (nodo.DesUrl != "#") newNode.NavigateUrl = urlbase + nodo.DesController + "/" + nodo.DesAction;
                }

                if (nodo.OptionCode == 302)
                {
                    newNode.NavigateUrl = "https://www.coes.org.pe/pr21app?user=" + dao.ii_UserCode;
                    newNode.Target = "_blank";
                }

                if (nodo.OptionCode == 303)
                {
                    newNode.NavigateUrl = "https://www.coes.org.pe/pr21app/home/consulta?user=" + dao.ii_UserCode;
                    newNode.Target = "_blank";
                }

                if (nodo.OptionCode == 300)
                {
                    string url = HttpUtility.UrlEncode((new COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(dao.is_UserLogin));
                   newNode.NavigateUrl = "https://www.coes.org.pe/appMedidores/?user=" + url;
                    newNode.Target = "_blank";
                }

                if (nodo.OptionCode == 288)
                {
                    newNode.NavigateUrl = "http://sicoes.coes.org.pe/appsstsct";
                    newNode.Target = "_blank";
                }

                if (nodo.OptionCode == 775)
                {
                    newNode.NavigateUrl = "http://sicoes.coes.org.pe/webris/login.aspx";
                    newNode.Target = "_blank";
                }

                if (nodo.IndTipo == "E")
                {
                    newNode.NavigateUrl = nodo.DesUrl;
                    newNode.Target = "_blank";
                }

                if (tree != null) nav.Items.Add(newNode);
                else node.ChildItems.Add(newNode);

                this.LlenarOpciones(nodo.OptionCode, newNode, null);
            }
        }
    }
}