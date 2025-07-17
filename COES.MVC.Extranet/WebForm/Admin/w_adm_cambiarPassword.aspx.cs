using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using WSIC2010.Util;
using System.Data;

namespace WSIC2010.Admin
{
    public partial class w_adm_cambiarPassword : System.Web.UI.Page
    {
        n_app in_app;
        AdminService admin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Admin/w_adm_cambiarPassword.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                admin = new AdminService();

                //if (!IsPostBack)
                //{
                //    if (Session["userSelected"] != null)
                //    {
                //        UsuarioExterno usuario = (UsuarioExterno)Session["userSelected"];
                //        //label_user.Text = usuario.Nombre;
                //        //label_state.Text = usuario.Estado;
                //        //label_email.Text = usuario.Email;
                //    }
                //}
            }
        }

        protected void btn01_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbox1.Text.Trim()) && !String.IsNullOrEmpty(in_app.is_UserLogin))
            {
                DataSet i_ds = new DataSet("dslogin");
                string ls_login = in_app.is_UserLogin;
                in_app.Fill(0, i_ds, "fw_user", "select uservalidate, usercheck, usercode, areacode, username, empresas from fw_user where trim(userlogin) = '" + ls_login.ToString().Trim() + "' and userstate = 'A'");

                if (i_ds.Tables["fw_user"].Rows.Count > 0)
                {
                    DataRow dr = i_ds.Tables["fw_user"].Rows[0];
                    int li_uservalidate = Convert.ToInt32(dr["uservalidate"]);
                    int li_usercheck = Convert.ToInt32(dr["usercheck"]);
                    int li_usercode = Convert.ToInt32(dr["usercode"]);
                    int li_areacode = Convert.ToInt32(dr["areacode"]);

                    string ls_name = dr["username"].ToString().Trim();

                    int ll_temp1, ll_temp2;

                    ll_temp1 = StringHelper.f_getpass(li_usercode + 1000 * li_areacode, ls_login.Trim() + tbox1.Text.Trim());
                    ll_temp2 = li_uservalidate - (li_usercheck - 10000) * 2;
                    int li_resultado;

                    if (ll_temp1 == ll_temp2)
                    {
                        if (tbox2.Text.Trim().Equals(tbox3.Text.Trim()))
                        {
                            string ls_userpass = tbox2.Text.Trim();
                            bool lb_positivo = false;
                            int li_valor = 0;

                            while (!lb_positivo)
                            {
                                li_valor = StringHelper.f_getpass(li_usercode + 1000 * li_areacode, ls_login.Trim() + ls_userpass.Trim()) + 2 * (li_usercheck - 10000);
                                if (li_valor > 0)
                                {
                                    lb_positivo = true;
                                }
                            }

                            li_uservalidate = li_valor;
                            //string ls

                            li_resultado = admin.SetPassword(li_usercode, li_uservalidate);

                            if (li_resultado > 0)
                            {
                                //li_resultado = admin.EnviaCorreoAlta(in_app.is_UserName, in_app.ii_AreaCode,ls_login, ls_userpass, "EXTRANET COES: CAMBIO CLAVE SATISFACTORIO", false);
                                //li_resultado = admin.EnviaCorreoAlta(in_app.is_UserName, in_app.ii_AreaCode, "aita", ls_userpass, "EXTRANET COES: CAMBIO CLAVE SATISFACTORIO", false); --cambiar passwords locales
                                if (li_resultado > 0)
                                {
                                    lbl01.Text = "<p style='color:#00f;'>La contrase&ntilde;a fue cambiada con &eacute;xito</p>";
                                    return;
                                }
                                else
                                {
                                    lbl01.Text = "<p style='color:#00f;'>Error al momento de cambiar contrase&ntilde;s</p>";
                                    return;
                                }
                            }
                            else
                            {
                                lbl01.Text = "<p style='color:#00f;'>Error al momento de cambiar contrase&ntilde;as</p>";
                                return;
                            }
                        }
                        else
                        {
                            lbl01.Text = "<p style='color:#00f;'>Los campos asignados a la nueva contrase&ntilde;a no coinceden</p>";
                            return;
                        }
                    }
                    else
                    {
                        lbl01.Text = "<p style='color:#00f;'>Ingrese contrase&ntilde;a actual correcta</p>";
                        return;
                    }
                }
            }
            else
            {
                lbl01.Text = "<p style='color:#00f;'>Ingrese contrase&ntilde;a actual</p>";
                return;
            }
        }
    }
}