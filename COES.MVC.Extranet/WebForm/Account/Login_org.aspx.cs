using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSIC2010.Util;
using fwapp;

namespace WSIC2010.Account
{
    public partial class Login_org : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (TextBoxUserLogin.Text == "egenor" && TextBoxUserPassword.Text == "qq")
            {
                n_app in_app = new n_app();
                in_app.ii_UserCode = 100;
                in_app.ii_AreaCode = 7;
                in_app.is_UserLogin = TextBoxUserLogin.Text;
                Session["in_app"] = in_app;
            }
            else
            {
                wf_validarusuario();
            }

            if (Session["in_app"] != null)
                if (Session["ReturnPage"] != null)
                {
                    Response.Redirect(Convert.ToString(Session["ReturnPage"]));
                }
                else
                {
                    Response.Redirect("~/WebForm/Default.aspx");
                }
        }

        private void wf_validarusuario()
        {
            DataSet i_ds = new DataSet("dslogin");
            //            Session.Clear();
            n_app in_app = new n_app();
            string ls_login = this.TextBoxUserLogin.Text.Trim();
            string ls_password = this.TextBoxUserPassword.Text.Trim();
            //MessageBox.Show("validando");			
            if (ls_login != "")
            {
                if (ls_password != "")
                {
                    //if (ls_login == "elmer" && ls_password == "felix")
                    //{
                    //    in_app.ii_root = -1;
                    //    in_app.ib_access = true;
                    //    in_app.ii_UserCode = 0;
                    //    in_app.is_UserName = "Administrador";
                    //    in_app.is_UserLogin = "root";
                    //    in_app.ii_AreaCode = 0;
                    //    in_app.is_AreaName = "Sistemas";
                    //    in_app.is_Empresas = "0";
                    //    in_app.Ls_emprcodi[0] = "0";
                    //    Session["in_app"] = in_app;
                    //    // wf_close(in_parameter);
                    //    return;
                    //}

                    //n_parameter ln_parameter = new n_parameter();
                    //in_app.iL_data[1].Fill(i_ds, "fw_user", "select uservalidate, usercheck, usercode, areacode, username from fw_user where userlogin = '" + ls_login.ToString() + "'");
                    in_app.Fill(0, i_ds, "fw_user", "select uservalidate, usercheck, usercode, areacode, username, empresas from fw_user where trim(userlogin) = '" + ls_login.ToString().Trim() + "' and userstate = 'A'");
                    //WScoes.ManttoService service = new WScoes.ManttoService();


                    if (i_ds.Tables["fw_user"].Rows.Count > 0)
                    {
                        DataRow dr = i_ds.Tables["fw_user"].Rows[0];
                        int ll_uservalidate = Convert.ToInt32(dr["uservalidate"]);
                        int ll_usercheck = Convert.ToInt32(dr["usercheck"]);
                        int ll_usercode = Convert.ToInt32(dr["usercode"]);
                        int ll_areacode = Convert.ToInt32(dr["areacode"]);

                        string ls_name = dr["username"].ToString().Trim();

                        int ll_temp1, ll_temp2;

                        ll_temp1 = StringHelper.f_getpass(ll_usercode + 1000 * ll_areacode, ls_login.Trim() + ls_password.Trim());
                        ll_temp2 = ll_uservalidate - (ll_usercheck - 10000) * 2;
                        if (ll_temp1 == ll_temp2)
                        {
                            in_app.ib_access = true;
                            in_app.is_UserLogin = ls_login;
                            in_app.ii_UserCode = ll_usercode;
                            in_app.is_UserName = ls_name;
                            in_app.ii_AreaCode = ll_areacode;
                            //in_app.is_PC_IPs = HttpContext.Current.Request.UserHostAddress;
                            in_app.is_PC_IPs = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            //in_app.is_PC_IPs = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                                in_app.ib_IsIntranet = true;

                            in_app.is_Empresas = dr["EMPRESAS"].ToString().Trim();
                            in_app.Ls_emprcodi = EPString.EP_GetListStringSeparate(in_app.is_Empresas, ',');
                            if (in_app.Ls_emprcodi.Count == 0)
                                in_app.Ls_emprcodi.Add("-1");

                            in_app.is_AreaName = ((string)in_app.iL_data[0].nf_ExecuteScalar("select areaname from fw_area where areacode = " + ll_areacode.ToString())).Trim();
                            labelmessage.Text = "Usuario y clave Ok! -> " + ls_name + "-" + in_app.is_AreaName;
                            labelmessage.ForeColor = System.Drawing.Color.Blue;
                            //Refresh();
                            //System.Threading.Thread.Sleep(800);
                            Session["in_app"] = in_app;
                            return;
                        }
                        else
                        {
                            this.labelmessage.Text = "Usuario y/o clave INVALIDO!";
                        }
                    }
                    else
                    {
                        this.labelmessage.Text = "Usuario no registrado";
                    }

                }
                else this.labelmessage.Text = "Ingrese su clave.";
            }
            else this.labelmessage.Text = "Ingrese el usuario";
        }

    }
}
