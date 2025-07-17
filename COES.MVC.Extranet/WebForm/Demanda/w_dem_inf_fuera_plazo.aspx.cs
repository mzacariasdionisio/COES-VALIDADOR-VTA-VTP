using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;

namespace WSIC2010.Demanda
{
    public partial class w_dem_inf_fuera_plazo : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        int li_earcodi = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/demanda/w_dem_inf_fuera_plazo.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if ((admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode)) && Session["earcodi"] != null)
                {
                    if (!IsPostBack)
                    {

                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();

                        if (!Int32.TryParse(Session["earcodi"].ToString(), out li_earcodi))
                        {
                            Response.Redirect("~/Account/Login.aspx");
                        }
                        else
                        {
                            if (Session["listaErrores"] != null)
                            {
                                lBox.Visible = true;
                                foreach (ListItem item in (ListItemCollection)Session["listaErrores"])
                                {
                                    lBox.Items.Add(item);
                                }
                            }
                        }

                    }
                }

                else
                {
                    Response.Redirect("~/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void btn_enviar_Click(object sender, EventArgs e)
        {
            int li_realizado = 0;
            IDemandaService demandaService = new DemandaService();
            DemandaService demandaServ = new DemandaService();
            if (Session["earcodi"] != null)
            {
                if (Int32.TryParse(Session["earcodi"].ToString(), out li_earcodi))
                {
                    if (rb_list.SelectedValue == "3" || rb_list.SelectedValue == "4")
                    {
                        li_realizado = demandaServ.nf_upd_env_estado(li_earcodi, 2, in_app.is_UserLogin);// 2 valor por defecto (Procesado)
                        if(li_realizado == 1)
                            li_realizado = demandaServ.nf_upd_env_estado_envio(li_earcodi, Convert.ToInt32(rb_list.SelectedValue), DateTime.Now, in_app.is_UserLogin, tbox_descipcion.Text.TrimStart().TrimEnd());
                    }
                }
                else
                    Response.Redirect("~/Account/Login.aspx");

                if (li_realizado > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Se guardó la información alcanzada');document.location.href='./w_me_carga_ult.aspx';", true);
                }
            }
            else
                Response.Redirect("~/Account/Login.aspx");
        }
    }
}