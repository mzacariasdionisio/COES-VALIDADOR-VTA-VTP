using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using WScoes;

namespace WSIC2010
{
    public partial class w_procedimiento_21 : System.Web.UI.Page
    {
        string sesionApp = "in_app";
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[sesionApp] == null)
            {
                Session["ReturnPage"] = "~/WebForm/w_procedimiento_21.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_CumplimientoRPF) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        this.CargarDatos();
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void CargarDatos()
        {
            n_app dao = (n_app)Session["in_app"];
            string controlador = "home";
            string accion = Request["accion"];

            //en duro
            //string page = String.Format("<iframe  id='frameContenido' src='http://wwww.coes.org.pe/pr21app/{1}/{2}?user={0}' width='100%' height='900' scrolling='auto' style='border:1px none'></iframe>",
            //    dao.is_UserLogin, controlador, accion);
            
            string page = String.Format("<iframe  id='frameContenido' src='"+ System.Configuration.ConfigurationManager.AppSettings["PortalWeb"].ToString() + "pr21app/{1}/{2}?user={0}' width='100%' height='900' scrolling='auto' style='border:1px none'></iframe>",
                dao.is_UserLogin, controlador, accion);

            this.lblContenido.Text = page;
        }
    }
}