using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010
{
    public partial class w_reclamos : System.Web.UI.Page
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
                    //this.CargarDatos();
                }
            }
        }

        //protected void CargarDatos()
        //{
        //    n_app dao = (n_app)Session["in_app"];
        //    string rutareclamos = ConfigurationManager.AppSettings["RutaReclamos"];
        //    string page = String.Format("<iframe src='{0}?user={1}' width='100%' height='1000' scrolling='auto' style='border:1px none'></iframe>", rutareclamos, dao.is_UserLogin);

        //    this.lblContenido.Text = page;
        //}
    }
}