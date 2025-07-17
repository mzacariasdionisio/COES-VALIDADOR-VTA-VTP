using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;

namespace WSIC2010.Mantto
{
    public partial class w_eve_mantto_new : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/mantto/w_eve_mantto.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Mantenimientos) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Mantenimientos)) //Adm. Manttos
                        {
                            ButtonNew.Enabled = true;
                            ButtonNew.Visible = true;
                        }
                        CheckBoxEmpresas.Text += " " + in_app.Ls_emprcodi[0];
                        //ManttoServiceClient service = new ManttoServiceClient();                    
                        ManttoService service = new ManttoService();
                        DataTable H_MRegistros;
                        if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                            H_MRegistros = service.H_GetManttosRegistros(DateTime.Today.AddDays(-180), DateTime.Today.AddMonths(3));
                        else
                            H_MRegistros = service.H_GetManttosRegistros(DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(3));

                        gView.DataSource = H_MRegistros;
                        gView.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void gView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "hide";
                //e.Row.Cells[0].Attributes.Add("style", "display:none;");
                //e.Row.Attributes.Add("style", "cursor:pointer;");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onMouseOver", "this.style.cursor='hand';");
                //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ceedfc'");
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                //e.Row.Attributes.Add("style", "cursor:pointer;");
                //e.Row.Attributes.Add("onclick", "location='w_eve_mantto_list.aspx?id=" + e.Row.Cells[0].Text + "&empresa=" + CheckBoxEmpresas.Checked + "'");
                //e.Row.Attributes.Add("onclick", "location='w_eve_mantto_list.aspx");
                e.Row.Attributes.Add("onclick", "javascript:onGridViewRowSelected_eve(" + e.Row.Cells[0].Text + ",'" + e.Row.Cells[4].Text + "')");
                e.Row.Attributes.Add("style", "cursor:hand;cursor:pointer");
                e.Row.Attributes.Add("onmouseover", "javascript:onMouseOver_eve(this)");
                e.Row.Attributes.Add("onmouseout", "javascript:onMouseOut_eve(this)");

            }
        }

        protected void ButtonNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_newreg_mantto.aspx");       
        }

        //protected void gView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    GridViewRow row = gView.Rows[e.NewSelectedIndex];
        //    Session["i_regcodi"] = Convert.ToInt32(row.Cells[1].Text);
        //    Session["d_fechaLimite"] = nf_get_calculaFecha(Convert.ToInt32(row.Cells[2].Text), EPDate.ToDate(row.Cells[5].Text));
        //    //for (int i = 0; i < row.Cells.Count; i++ )
        //    //{
        //    //    string valor = row.Cells[i].Text;
        //    //}
        //    Session["b_empresa"] = CheckBoxEmpresas.Checked;

        //    Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");

        //}

        //protected void gView_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView drowView = (DataRowView)e.Row.DataItem;
        //        if (drowView != null)
        //        {
        //            if (drowView.Row.ItemArray[1].ToString() == "1")
        //            {
        //                //e.Row.BackColor = System.Drawing.Color.FromArgb(0xFFE5E5);
        //                e.Row.BackColor = System.Drawing.Color.FromArgb(0xFFFFE5);
        //                //e.Row.ForeColor = System.Drawing.Color.FromArgb(0xFFFFFF);
        //            }
        //            else if (drowView.Row.ItemArray[1].ToString() == "2")
        //            {
        //                //e.Row.BackColor = System.Drawing.Color.FromArgb(0x00CCFF);
        //                e.Row.BackColor = System.Drawing.Color.FromArgb(0xBAE3FF);
        //            }
        //            else if (drowView.Row.ItemArray[1].ToString() == "3")
        //            {
        //                //e.Row.BackColor = System.Drawing.Color.FromArgb(0x99FFCC);
        //                e.Row.BackColor = System.Drawing.Color.FromArgb(0xFFF0E0);
        //            }
        //            else if (drowView.Row.ItemArray[1].ToString() == "4")
        //            {
        //                //e.Row.BackColor = System.Drawing.Color.FromArgb(0xCCCCFF);
        //                e.Row.BackColor = System.Drawing.Color.FromArgb(0xCCEBEB);
        //            }
        //            else if (drowView.Row.ItemArray[1].ToString() == "5")
        //            {
        //                //e.Row.BackColor = System.Drawing.Color.FromArgb(0xCCCCFF);
        //                e.Row.BackColor = System.Drawing.Color.FromArgb(0xE0E0E0);
        //            }
        //        }
        //    }
        //}


        //protected string GetEnlace(object ao_fcodigo, object ao_ftipo)
        //{
        //    int li_fcodigo;
        //    bool lb_fempresa;
        //    li_fcodigo = Convert.ToInt32(ao_fcodigo);
        //    lb_fempresa = Convert.ToBoolean(ao_ftipo);

        //    Session["i_regcodi"] = li_fcodigo;
        //    Session["b_empresa"] = lb_fempresa;

        //    //Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");

        //    return "w_eve_mantto_list.aspx?id=" + li_fcodigo + "&empresa=" + CheckBoxEmpresas.Checked.ToString(); ;
        //}

        protected void dl_TipoPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManttoService service = new ManttoService();
            DataTable H_MRegistros;
            //if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                //H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-180), DateTime.Today.AddMonths(3), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
            //else
                //H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(3), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));

            //gView.DataSource = H_MRegistros;
            //gView.DataBind();
        }
    }
}