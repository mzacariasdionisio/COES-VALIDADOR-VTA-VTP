using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSIC2010.Util;

namespace WSIC2010.Account
{
    public partial class Confirmacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null && Session["REG_EMPRESA"] != null)
            {
                UsuarioExterno userExterno = (UsuarioExterno)Session["Usuario"];
                Dictionary<int, string> empresas = (Dictionary<int, string>)Session["REG_EMPRESA"];
                Nombre.Text = userExterno.Nombre;
                Apellido.Text = userExterno.Apellido;
                Empresa.Text = empresas[userExterno.Empresa];
                Email.Text = userExterno.Email;
                Phone.Text = userExterno.Telefono;

            }
            else
            {
                Response.Redirect("~/WebForm/Account/Register.aspx");
            }

        }
    }
}