using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WSIC2010
{
    public partial class w_medidores : System.Web.UI.Page
    {
        string sesionApp = "in_app";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[sesionApp] == null)
            {
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    this.CargarDatos();
                }
            }
        }

        protected void CargarDatos()
        {
            n_app dao = (n_app)Session["in_app"];
            string controlador = Request["controlador"];
            string accion = Request["accion"];

            string page = String.Format("<iframe  id='frameContenido' src='" + ConfigurationManager.AppSettings["AppMedidores"].ToString()
                + "{1}/{2}/?user={0}' width='100%' height='1000' scrolling='auto' style='border:1px none'></iframe>",
                dao.is_UserLogin, controlador, accion);

            this.lblContenido.Text = page;
        }
    }
}