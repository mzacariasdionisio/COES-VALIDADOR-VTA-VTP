using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010.Admin
{
    public partial class page_error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Label label1 = new Label();
                label1.Text = "<div><br /><span class='style11'>SICOES</span><br /><p class='style21'>C&oacute;digo no existe.</p</div>";
                var t = (ContentPlaceHolder)Master.FindControl("MainContent");
                t.Controls.Add(label1);
            }
        }
    }
}