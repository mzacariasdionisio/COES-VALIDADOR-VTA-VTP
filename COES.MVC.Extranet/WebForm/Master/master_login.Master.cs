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
    public partial class master_login : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                //LinkButtonLogin.Visible = false;
                LinkButtonLogout.Visible = true;
                n_app i_app = (n_app)Session["in_app"];
                LabelUserName.Text = "Bienvenido: " + i_app.is_UserName;

                

            }
            else
            {
                //LinkButtonLogin.Visible = true;
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