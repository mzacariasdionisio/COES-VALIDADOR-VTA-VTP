using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010
{
    public partial class _Default : System.Web.UI.Page
    {
        n_app in_app;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Default.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");              
            }
            else
            {
                in_app = (n_app)Session["in_app"];
            }
        }
    }
}
