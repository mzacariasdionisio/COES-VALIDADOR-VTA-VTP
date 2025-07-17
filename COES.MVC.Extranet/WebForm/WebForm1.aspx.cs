using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;

namespace COES.MVC.Extranet.WebForm
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ManttoService service = new ManttoService();
            AjaxControlToolkit.CascadingDropDownNameValue[] lit = service.GetAreasPorEmpresa("4", "Area");
        }
    }
}