using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using WScoes;
using System.IO;

namespace WSIC2010
{
    public partial class w_me_log : System.Web.UI.Page
    {
        n_app in_app;
        string ls_sql;
        int li_earcodi;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Hidrologia/w_eve_hidro.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                //if (!in_app.ib_IsIntranet)
                //    Response.Redirect("~/About.aspx");
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Hidrologia) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        try
                        {
                            li_earcodi = Convert.ToInt32(Request.QueryString["id"]);
                            f_get_datos(li_earcodi);
                        }
                        catch
                        {
                            li_earcodi = 0;
                            return;
                        }

                        //ListBox1.Items.Add(li_earcodi.ToString());
                        return;


                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }



            }
        }

        private void f_get_datos(int pi_earcodi)
        {
            ListBox1.Items.Clear();


            ExtService srv_ext = new ExtService();

            gv.DataSource = srv_ext.nf_GetListarLog(pi_earcodi);
            gv.DataBind();

            ListBox1.Items.Add("Nro. de registro(s) : " + gv.Rows.Count);

        }



        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //cambio color
            //e.Row.Cells[1].BackColor= new rgb
        }




                
    }


}