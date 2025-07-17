using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSIC2010
{
    public partial class w_me_upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //INC 2024-001668 - Deshabilitar el acceso a la vista
            Response.Redirect("~/Error/Error404?");
        }

        protected void ButtonBarras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/Uploads/w_eve_formato_demanda.aspx");
        }

        protected void ButtonIDCC_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/Uploads/w_me_idcc.aspx");
        }

        protected void ButtonHidro_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/Uploads/w_me_hidro.aspx");
            Response.Redirect("~/WebForm/Uploads/w_eve_formato_hidro.aspx");
        }

        protected void ButtonCCOCarga_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_ccocarga.aspx");
        }

        protected void ButtonDemandaHistorica_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_dem_cargahistorica.aspx");
        }

        
    }
}