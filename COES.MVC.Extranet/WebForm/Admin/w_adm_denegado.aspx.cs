using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010.Admin
{
    public partial class w_adm_denegado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                Label label1 = new Label();
                label1.Text = "<div><br /><span class='style11'>SICOES</span><br /><p class='style21'>No tiene acceso a este m&oacute;dulo</p</div>";
                var t = (ContentPlaceHolder)Master.FindControl("MainContent");
                t.Controls.Add(label1);
            }
            else
            {
                Response.Redirect("~/WebForm/Account/Login.aspx", true);
            }
        }
    }
}