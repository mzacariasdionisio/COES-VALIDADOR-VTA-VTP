using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010.Account
{
    public partial class Registrado : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                n_app in_app = (n_app)Session["in_app"];
                labelUsuario.Text = in_app.is_UserName;
                labelUsuario.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                if (Session["REG_USUARIO"] != null)
                {
                    Dictionary<string, string> Usuarios = (Dictionary<string, string>)Session["REG_USUARIO"];
                    foreach (var usuario in Usuarios)
                    {
                        labelUsuario.Text = usuario.Value + ", ";
                        //Usuarios[Convert.ToInt32(dr["USERCODE"])] = dr["USERMAIL"].ToString();
                    }
                    
                }
                else
                {
                    Response.Redirect("~/WebForm/Account/Register.aspx");
                }
            }

        }
    }
}