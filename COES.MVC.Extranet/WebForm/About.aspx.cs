using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            //if (Session["in_app"] == null)
            //{
            //    Response.Redirect("~/WebForm/Account/Login.aspx");
            //}
            //else
            //{
            //    if (!IsPostBack)
            //    {
            //        DataSet i_ds = new DataSet("dslogin");
            //        n_app in_app = (n_app)Session["in_app"];
            //        LabelUsuario.Text = in_app.is_UserName;
            //        LabelEmpresa.Text = in_app.is_AreaName;
            //    }
            //}
        }
    }
}
