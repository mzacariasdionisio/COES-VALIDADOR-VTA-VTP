using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSIC2010.Util;

namespace WSIC2010.Account
{
    public partial class RestorePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn01_Click(object sender, EventArgs e)
        {
            n_app in_app = new n_app();
            WScoes.AdminService admin = new WScoes.AdminService();
            string ls_dominio_coes = "@coes.org.pe";
            string ls_login = this.TextBoxUserLogin.Text.Trim();
            string ls_login_inicial = ls_login;

            if (ls_login.EndsWith("@coes.org.pe"))
            {
                ls_login = ls_login.Replace(ls_dominio_coes, String.Empty);
            }

            DataSet i_ds = new DataSet("dslogin");
            int li_counter, li_resultado;
            li_counter = li_resultado = 0;

            if (Session["counter"] != null && !String.IsNullOrEmpty(Session["counter"].ToString()))
            {
                li_counter = Convert.ToInt32(Session["counter"]);
            }

            if (!String.IsNullOrEmpty(ls_login_inicial) && StringHelper.IsValidEmail(ls_login_inicial) && (li_counter <= 3))
            {
                in_app.Fill(0, i_ds, "fw_user", "select uservalidate, usercheck, usercode, areacode, username, empresas from fw_user where trim(userlogin) = '" + ls_login.ToString().Trim() + "' and userstate = 'A'");

                if (i_ds.Tables["fw_user"].Rows.Count > 0)
                {
                    DataRow dr = i_ds.Tables["fw_user"].Rows[0];

                    string ls_username = Convert.ToString(dr["username"].ToString());
                    int li_usercode = Convert.ToInt32(dr["usercode"].ToString());
                    int li_areacode = Convert.ToInt32(dr["areacode"].ToString());
                    int li_usercheck = Convert.ToInt32(dr["usercheck"].ToString());
                    string ls_userpass = StringHelper.nf_get_random_str(6);
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

                    int li_uservalidate = li_valor;
                    //string ls

                    li_resultado = admin.SetPassword(li_usercode, li_uservalidate);

                    if (li_resultado > 0)
                    {
                        //li_resultado = admin.EnviaCorreoAlta(ls_username, 0, ls_login_inicial, ls_userpass, "EXTRANET COES: NUEVA CLAVE EXTRANET COES", true);
                        //li_resultado = admin.EnviaCorreoAlta(ls_username, 0, "aita", ls_userpass, "EXTRANET COES: NUEVA CLAVE EXTRANET COES", true); //Resetear los passwords locales

                        string mensaje = "Estimado {0}: <br /><br />";
                        mensaje+= "Sus datos de inicio de sesión son los siguientes: <br /><br />";
                        mensaje+= "Usuario: {1} <br />";
                        mensaje+= "Clave: {2} <br /><br />";                        
                        mensaje+= "<strong>COES SINAC</strong>";

                        string cuerpo = string.Format(mensaje, ls_username, ls_login, ls_userpass);

                        COES.Base.Tools.Util.SendEmail(ls_login_inicial, "EXTRANET COES: NUEVA CLAVE EXTRANET COES", cuerpo);
                    }

                    labelmessage.Text = "Se envió correo con los nuevos accesos";
                    Session.Clear();
                }
                else
                {
                    labelmessage.Text = "Usuario no registrado";
                    li_counter ++;
                    Session["counter"] = li_counter;
                }
            }
            else
            {
                if (li_counter >= 3)
                {
                    labelmessage.Text = "DESHABILITADO !. Se ingresó varios intentos";
                }
                else
                {
                    labelmessage.Text = "Ingrese login registrado y/o v&aacute;lido";
                }
                
                li_counter++;
                Session["counter"] = li_counter;
            }
        }
    }
}