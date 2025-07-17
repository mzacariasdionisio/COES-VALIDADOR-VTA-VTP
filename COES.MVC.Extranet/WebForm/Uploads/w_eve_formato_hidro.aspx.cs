using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WSIC2010.Uploads
{
    public partial class w_eve_formato_hidro : System.Web.UI.Page
    {
        COES.MVC.Extranet.srGestor.CMSManagerClient gcCOES;
        static string BASE_PATH = "/Company Home/Contenido Web/Formatos/Extranet/Hidrologia/";
        string ls_option_selected = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gcCOES = new COES.MVC.Extranet.srGestor.CMSManagerClient();
                DataTable dt_Spaces = gcCOES.GetDataByPath(BASE_PATH);

                gView1.DataSource = dt_Spaces;
                gView1.DataBind();
            }
        }
    }
}