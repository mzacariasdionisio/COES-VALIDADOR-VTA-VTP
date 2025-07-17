using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Linq;
//using WSIC2010.WScoes;
using WScoes;
using WSIC2010.Util;

namespace WSIC2010
{
    public partial class w_eve_mantto : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        CManttoRegistro ManttoRegistro;

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

                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Administrador_Mantenimientos))
                {
                    ButtonEditManttos.Enabled = true;
                    ButtonEditManttos.Visible = true;
                }

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Mantenimientos) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        CheckBoxEmpresas.Text += " " + in_app.Ls_emprcodi[0];
                        //ManttoServiceClient service = new ManttoServiceClient();                    
                        ManttoService service = new ManttoService();
                        Dictionary<int, string> H_MRegistros;
                        if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                            H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-180), DateTime.Today.AddMonths(5), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
                        else
                            H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(5), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
                        ListBoxRegistros.DataSource = H_MRegistros;
                        ListBoxRegistros.DataTextField = "Value";
                        ListBoxRegistros.DataValueField = "Key";
                        ListBoxRegistros.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (ListBoxRegistros.SelectedIndex >= 0)
            {               
                int li_registro = Convert.ToInt32(ListBoxRegistros.SelectedItem.Value);
                if (li_registro > 0)
                {
                    ManttoService service = new ManttoService();
                    
                    Session["i_regcodi"] = li_registro;
                    

                    CManttoRegistro man = service.GetManttoRegistro(li_registro);
                    Session["d_fechaLimite"] = nf_GetCalculaFecha(man.RegCodi);

                    /**/
                    string ls_fecha = Convert.ToString(Session["d_fechaLimite"]).ToString();

                    if (String.IsNullOrEmpty(Convert.ToString(Session["d_fechaLimite"]).ToString()))
                    {
                        if (CheckBoxEmpresas.Checked)
                            Session["b_empresa"] = true;
                        else
                            Session["b_empresa"] = false;
                    }
                    else
                    {
                        Session["b_empresa"] = true;
                        Util.Alert.Show("No se puede visualizar los mantenimientos");
                    }

                    
                    Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");

                    //Response.Redirect("~/WebForm/mantto/w_eve_lista_manttos.aspx");

                    //Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx?id=" + li_registro + "&empresa=" + CheckBoxEmpresas.Checked); 
                }
                else
                {
                    LabelMensaje.Text = "ERROR SelectedValue!";
                    LabelMensaje.ForeColor = Color.Red;
                }                    
            }
            else
            {
                LabelMensaje.Text = "Tiene que seleccionar un item!";
                LabelMensaje.ForeColor = Color.Red;
            }
        }

        private string nf_GetCalculaFecha(int pi_mancodi)
        {
            ManttoService service = new ManttoService();
            ManttoRegistro = service.GetManttoRegistro(pi_mancodi);
            DateTime ldt_fechaLimite = ManttoRegistro.FechaLimiteFinal;
            DateTime ldt_fecha = new DateTime(2013, 1, 1);
            TimeSpan ts;
            double pd_limiteHora = 0;
            int pi_numeroDias = 0;
            if (ManttoRegistro.EvenClaseCodi == 1)
            {
                pd_limiteHora = 1;
                pi_numeroDias = -1;
            }
            else if (ManttoRegistro.EvenClaseCodi == 2)
            {
                //pd_limiteHora = 10.1;
                pd_limiteHora = 9;
                pi_numeroDias = 1;
            }
            else if (ManttoRegistro.EvenClaseCodi == 3)
            {
                //pd_limiteHora = 14.1;
                pd_limiteHora = 0.1;
                pi_numeroDias = 4;
            }
            else if (ManttoRegistro.EvenClaseCodi == 4)
            {
                pd_limiteHora = 24;
                pi_numeroDias = 21; //Para limite 13/08/2013
            }
            else if (ManttoRegistro.EvenClaseCodi == 5)
            {
                pd_limiteHora = 24;
                pi_numeroDias = 103;
            }

            switch (ManttoRegistro.EvenClaseCodi)
            {
                case 1:
                    ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                    ts = ts.Subtract(new TimeSpan(-1, 0, 0, 0));
                    if (ts.TotalHours >= 0)
                    {
                        return ManttoRegistro.FechaInicial.AddDays(-pi_numeroDias).AddHours(pd_limiteHora).AddSeconds(-20).ToString("MMM d yyyy HH:mm:ss");
                    }
                    else
                    {
                        return String.Empty;
                    }
                case 2:
                    ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                    ts = ts.Subtract(new TimeSpan(pi_numeroDias, 0, 0, 0));
                    if (ts.TotalHours >= 0)
                    {
                        return ManttoRegistro.FechaInicial.AddDays(-pi_numeroDias).AddHours(pd_limiteHora).AddSeconds(-20).ToString("MMM d yyyy HH:mm:ss");
                    }
                    else
                    {
                        return String.Empty;
                    }
                case 3:
                    if (ManttoRegistro.FechaInicial.ToString("dd/MM/yyyy").Equals("03/08/2013"))
                    {
                        pi_numeroDias = 8;
                        pd_limiteHora = 0.1;
                    }
                    ts = ManttoRegistro.FechaInicial.AddHours(pd_limiteHora) - DateTime.Now;
                    ts = ts.Subtract(new TimeSpan(pi_numeroDias, 0, 0, 0));
                    if (ts.TotalHours >= 0)
                    {
                        return ManttoRegistro.FechaInicial.AddDays(-pi_numeroDias).AddHours(pd_limiteHora).AddSeconds(-20).ToString("MMM d yyyy HH:mm:ss");
                    }
                    else
                    {
                        return String.Empty;
                    }
                case 4:
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(9).AddHours(24) - DateTime.Now; //10/09/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(8).AddHours(24) - DateTime.Now; //09/07/13
                    //ts = ldt_fecha.AddMonths(6).AddDays(9).AddHours(24) - DateTime.Now;
                    if (ts.TotalHours >= 0)
                    {
                        //return pdt_fechaInicial.AddDays(-pi_numeroDias).AddHours(pd_limiteHora).ToString("MMM d yyyy HH:mm:ss");
                        return ldt_fechaLimite.ToString("MMM d yyyy HH:mm:ss");
                    }
                    else
                    {
                        return String.Empty;
                    }
                case 5:
                    ts = ldt_fechaLimite - DateTime.Now;
                    //ts = ldt_fecha.AddMonths(8).AddDays(19).AddHours(24) - DateTime.Now;//06/09/13
                    if (ts.TotalHours >= 0)
                    {
                        //return pdt_fechaInicial.AddDays(-pi_numeroDias).AddHours(pd_limiteHora).ToString("MMM d yyyy HH:mm:ss");
                        return ldt_fechaLimite.ToString("MMM d yyyy HH:mm:ss");
                    }
                    else
                    {
                        return String.Empty;
                    }
                default:
                    return String.Empty;
            }
        }

        protected void ButtonNew_Click(object sender, EventArgs e)
        {            
            Response.Redirect("~/WebForm/mantto/w_eve_newreg_mantto.aspx");         
        }

      

        protected void ButtonIDCC_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_newreg_mantto.aspx");  
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_uploads.aspx");
        }

        protected void dl_TipoPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManttoService service = new ManttoService();
            Dictionary<int, string> H_MRegistros;
            if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-180), DateTime.Today.AddMonths(5), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
            else
            {
                if (dl_TipoPrograma.SelectedValue.Equals("5"))
                {
                    H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(20), DateTime.Today.AddMonths(5), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
                }
                else
                {
                    H_MRegistros = service.H_GetManttoRegistrosXTipo(DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(5), Convert.ToInt32(dl_TipoPrograma.SelectedItem.Value));
                }
            }
            ListBoxRegistros.DataSource = H_MRegistros;
            ListBoxRegistros.DataTextField = "Value";
            ListBoxRegistros.DataValueField = "Key";
            ListBoxRegistros.DataBind();
        }

        protected void ButtonEditManttos_Click(object sender, EventArgs e)
        {
            if (ListBoxRegistros.SelectedIndex >= 0)
            {
                int li_registro = Convert.ToInt32(ListBoxRegistros.SelectedItem.Value);
                if (li_registro > 0)
                {
                    Session["i_regcodi"] = li_registro;
                    Response.Redirect("~/WebForm/mantto/w_eve_editreg_mantto.aspx");
                }
                else
                {
                    LabelMensaje.Text = "ERROR SelectedValue!";
                    LabelMensaje.ForeColor = Color.Red;
                }
            }
            else
            {
                LabelMensaje.Text = "Tiene que seleccionar un item!";
                LabelMensaje.ForeColor = Color.Red;
            }
        }
    }
}