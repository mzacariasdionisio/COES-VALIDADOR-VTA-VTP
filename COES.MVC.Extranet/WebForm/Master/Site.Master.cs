using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                LinkButtonLogin.Visible = false;
                LinkButtonLogout.Visible = true;
                n_app i_app = (n_app)Session["in_app"];
                LabelUserName.Text = "Bienvenido " + i_app.is_UserName;

                /*Nuevo*/
                foreach (MenuItem item in NavigationMenu.Items)
                {
                    //if (item.Value.Equals("Demanda"))
                    //{
                    //    item.ChildItems.Add(new MenuItem() { Text= "Listado", NavigateUrl= "~/WebForm/Demanda/w_me_demandaBarras_list.aspx"});
                    //    item.ChildItems.Add(new MenuItem() { Text = "Listado Hist&oacute;rico", NavigateUrl = "~/WebForm/Demanda/w_dem_historica.aspx" });
                    //    item.ChildItems.Add(new MenuItem() { Text = "Carga", NavigateUrl = "~/WebForm/Demanda/w_me_demanda.aspx" });
                    //}

                    if (item.Value.Equals("Operacion"))
                    {
                        item.ChildItems.Add(new MenuItem() { Text = "Mantenimiento", NavigateUrl = "~/WebForm/mantto/w_eve_mantto.aspx" });
                        item.ChildItems.Add(new MenuItem() { Text = "Stock de Combustible", NavigateUrl = "~/WebForm/w_op_combustible.aspx" });
                    }

                    if (item.Value.Equals("Consulta"))
                    {
                        item.ChildItems.Add(new MenuItem() { Text = "Listar Cargas", NavigateUrl = "~/WebForm/Hidrologia/Consultas/w_me_listarcargas.aspx", Value = "ListarCargas" });
                        item.ChildItems.Add(new MenuItem() { Text = "Estad&iacute;stica Cumplimiento", NavigateUrl = "~/WebForm/Hidrologia/Consultas/w_me_ratio.aspx", Value = "EstadisticaCumplimiento" });
                    }

                    if (item.Value.Equals("Administracion"))
                    {
                        item.ChildItems.Add(new MenuItem() { Text = "Listar Usuarios", NavigateUrl = "~/WebForm/Admin/w_adm_listarUsuarios.aspx", Value = "ListarUsuarios" });
                    }
                }

            }
            else
            {
                LinkButtonLogin.Visible = true;
                LinkButtonLogout.Visible = false;
            }
        }

        protected void LinkButtonLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/Account/Login.aspx");
        }

        protected void LinkButtonLogout_Click(object sender, EventArgs e)
        {
            Session["in_app"] = null;
            FormsAuthentication.SetAuthCookie(null, false);
            Session[DatosSesion.SesionUsuario] = null;
            //Response.Redirect("~/WebForm/Default.aspx");
            Response.Redirect("~/WebForm/Account/Login.aspx");
        }

       
    }
}
