using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSIC2010.Util;
using fwapp;
using WScoes;
using System.Web.Security;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.ServiceReferenceMail;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using COES.Servicios.Aplicacion.CPPA.Helper;
using System.Configuration;

namespace WSIC2010.Account
{
    public partial class Login : System.Web.UI.Page
    {

        SeguridadServicioClient servicio = new SeguridadServicioClient();
        private static readonly HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            wf_validarusuario();

            if (Session["in_app"] != null)
            {
                if (Session["ReturnPage"] != null)
                {
                    Response.Redirect(Convert.ToString(Session["ReturnPage"]));
                }
                else
                {
                    if (Request["op"] == "login")
                    {
                        Response.Redirect("~/home/info");
                    }
                    else 
                    {
                        Response.Redirect("~/home/default");
                    }
                }
            }
        }

        private void wf_validarusuario()
        {
            LogService logService = new LogService();
            DataSet i_ds = new DataSet("dslogin");
            n_app in_app = new n_app();
            string ls_login = this.TextBoxUserLogin.Text.Trim().ToLowerInvariant();
            string ls_password = this.TextBoxUserPassword.Text.Trim();
            string ls_ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (ls_login != "")
            {
                if (ls_password != "")
                {
                    bool flagRepresentante = false;

                    in_app.Fill(0, i_ds, "fw_user", "select uservalidate, usercheck, usercode, areacode, username, empresas, userstate, userlogin, useremail from fw_user where trim(userlogin) = '" + ls_login.ToString().Trim() + "'");

                    if (i_ds.Tables["fw_user"].Rows.Count > 0)
                    {
                        DataRow dr = i_ds.Tables["fw_user"].Rows[0];

                        if ((dr["userstate"] != DBNull.Value) && (dr["userstate"].ToString().Trim().Equals("A")))
                        {
                            int ll_uservalidate = Convert.ToInt32(dr["uservalidate"]);
                            int ll_usercheck = Convert.ToInt32(dr["usercheck"]);
                            int ll_usercode = Convert.ToInt32(dr["usercode"]);
                            int ll_areacode = Convert.ToInt32(dr["areacode"]);
                            string ls_name = dr["username"].ToString().Trim();
                            string ls_email = dr["useremail"].ToString().Trim();
                            int ll_temp1, ll_temp2;
                            ll_temp1 = StringHelper.f_getpass(ll_usercode + 1000 * ll_areacode, ls_login.Trim() + ls_password.Trim());
                            ll_temp2 = ll_uservalidate - (ll_usercheck - 10000) * 2;

                            if (ll_temp1 == ll_temp2)
                            {
                                in_app.ib_access = true;
                                in_app.is_UserLogin = ls_login;
                                in_app.ii_UserCode = ll_usercode;
                                in_app.is_UserEmail = ls_email;
                                in_app.is_UserName = ls_name;
                                in_app.ii_AreaCode = ll_areacode;
                                in_app.is_PC_IPs = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                                if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                                    in_app.ib_IsIntranet = true;
                                in_app.is_Empresas = dr["EMPRESAS"].ToString().Trim();
                                in_app.Ls_emprcodi = EPString.EP_GetListStringSeparate(in_app.is_Empresas, ',');
                                if (in_app.Ls_emprcodi.Count == 0)
                                    in_app.Ls_emprcodi.Add("-1");
                                in_app.is_AreaName = ((string)in_app.iL_data[0].nf_ExecuteScalar("select areaname from fw_area where areacode = " + ll_areacode.ToString())).Trim();
                                labelmessage.Text = "Usuario y clave Ok! -> " + ls_name + "-" + in_app.is_AreaName;
                                labelmessage.ForeColor = System.Drawing.Color.Blue;
                                logService.nf_add_log("EXTRANET", "login", "LOGIN", "válida", "FW_USER", null, null, in_app.is_PC_IPs, in_app.is_UserLogin, -1);
                                Session["in_app"] = in_app;

                                int resultado = 0;
                                UserDTO entidad = this.servicio.AutentificarUsuario(ls_login, ls_password, out resultado);
                                FormsAuthentication.SetAuthCookie(ls_login, false);
                                Session[DatosSesion.SesionUsuario] = entidad;
                                ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin = dr["userlogin"].ToString().Trim();
                                
                                //ASSETEC - TOKEN WEB API
                                try
                                {
                                    var url = ConfigurationManager.AppSettings["ApiSeguridad"] + ConstantesCPPA.urlApiSeguridadAuthenticate;
                                    var requestData = new
                                    {
                                        user = ls_login,
                                        password = ls_password
                                    };

                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                                    // Verificar si la solicitud fue exitosa
                                    if (response.IsSuccessStatusCode)
                                    {
                                        // Leer la respuesta como cadena de forma sincrónica
                                        string responseData = response.Content.ReadAsStringAsync().Result;
                                        var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseData);
                                        string accessToken = tokenResponse?.access_Token;
                                        string refreshToken = tokenResponse?.refresh_Token;
                                        Session[DatosSesion.SesionTokenApiSeguridad] = accessToken;
                                        Session[DatosSesion.SesionTokenRefreshApiSeguridad] = refreshToken;
                                    }
                                    else
                                    {
                                        Session[DatosSesion.SesionTokenApiSeguridad] = "-1";//El -1 significa que no se autentico           con el api seguridad
                                    }
                                }
                                catch (Exception e)
                                {
                                    Session[DatosSesion.SesionTokenApiSeguridad] = "-1";
                                }
                                //ASSETEC - TOKEN WEB API 
                                return;
                            }
                            else
                            {
                                logService.nf_add_log("EXTRANET", "login", "LOGIN", "inválida", "FW_USER", null, null, ls_ip, ls_login, -1);
                                this.labelmessage.Text = "Usuario y/o clave INVALIDO!";
                                this.HyperLink2.Enabled = true;
                                this.HyperLink2.Visible = true;
                            }
                        }
                        else
                        {
                            logService.nf_add_log("EXTRANET", "login", "LOGIN", "pendiente", "FW_USER", null, null, ls_ip, ls_login, -1);
                            this.labelmessage.Text = "Usuario pendiente de autorización de acceso";
                        }
                    }

                    else
                    {
                        //aca realizamos para loguearse con el representante legal
                        DataSet i_dsRep = new DataSet("dsloginrep");
                        string sql = string.Format(@"select usuario.username, usuario.userapellido, usuario.userlogin, 
                                        usuario.userpwd,
                                        empresa.emprcodi
                                        from 
                                        wb_user usuario 
                                        inner join si_registro_integ registro
                                        on usuario.regempcodi = registro.regempcodi
                                        inner join si_empresa empresa on registro.emprecodi = empresa.emprcodi
                                        where usuario.userreplegal = 'S' and usuario.userestado = 'A' 
                                        and usuario.userlogin = '{0}' and usuario.userpwd = '{1}'", ls_login, ls_password);

                        in_app.Fill(0, i_ds, "wb_user", sql);

                        if (i_ds.Tables["wb_user"].Rows.Count > 0)
                        {
                            DataRow dr = i_ds.Tables["wb_user"].Rows[0];
                            int resultado = 0;

                            UserDTO entidad = this.servicio.AutentificarUsuario("representantelegal", "123456", out resultado);
                            FormsAuthentication.SetAuthCookie("representantelegal", false);
                            string nombre = (dr["username"] != null) ? dr["username"].ToString() : string.Empty;
                            string apellido = (dr["userapellido"] != null) ? dr["userapellido"].ToString() : string.Empty;
                            int idEmpresa = (dr["emprcodi"] != null) ? Convert.ToInt32(dr["emprcodi"]) : -1;
                            entidad.UsernName = nombre + " " + apellido;
                            entidad.EmprCodi = (short?)idEmpresa;
                            Session[DatosSesion.SesionUsuario] = entidad;
                            Session[DatosSesion.UserRepresentante] = ls_login;

                            in_app.is_UserLogin = ls_login;
                            Session["in_app"] = in_app;

                            return;
                        }
                        else
                        {
                            logService.nf_add_log("EXTRANET", "login", "LOGIN", "unregistered", "FW_USER", null, null, ls_ip, ls_login, -1);
                            this.labelmessage.Text = "Usuario no registrado";
                        }
                    }
                }
                else this.labelmessage.Text = "Ingrese su clave.";
            }
            else this.labelmessage.Text = "Ingrese el usuario";
        }
    }
}