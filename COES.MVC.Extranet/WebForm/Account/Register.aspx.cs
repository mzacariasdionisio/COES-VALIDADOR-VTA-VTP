using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSIC2010.Util;
using WScoes;
using System.Configuration;

namespace WSIC2010.Account
{
    public partial class Register : System.Web.UI.Page
    {

        string ls_mailAdmExtranet = "aita@coes.org.pe";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] != null)
            {
                Response.Redirect("~/WebForm/Account/Registrado.aspx");
            }
            else
            {
                if (!IsPostBack)
                {

                    Dictionary<int, string> Empresas = new Dictionary<int, string>();
                    Dictionary<int, string> Roles = new Dictionary<int, string>();
                    if (Session["REG_EMPRESA"] == null)
                    {
                        n_app Clase = new n_app();
                        DataSet i_ds = new DataSet("dsregister");
                        // n_app in_app = (n_app)Session["in_app"];
                        string ls_comando = @" SELECT * FROM FW_COMPANY WHERE COMPCODE NOT IN (0, -1, 1, 4) ORDER BY COMPNAME";
                        Clase.Fill(0, i_ds, "FW_COMPANY", ls_comando);
                        //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
                        foreach (DataRow dr in i_ds.Tables["FW_COMPANY"].Rows)
                        {
                            Empresas[Convert.ToInt32(dr["COMPCODE"])] = dr["COMPNAME"].ToString();
                        }
                        Session["REG_EMPRESA"] = Empresas;
                        Session["REG_APP"] = Clase;
                    }
                    else
                    {
                        Empresas = (Dictionary<int, string>)Session["REG_EMPRESA"];
                    }

                    DropDownListEmpresa.DataSource = Empresas;
                    DropDownListEmpresa.DataTextField = "value";
                    DropDownListEmpresa.DataValueField = "key";
                    DropDownListEmpresa.DataBind();

                    DropDownListEmpresa.SelectedIndex = 1;
                    Roles.Add(48, "MANTENIMIENTOS");
                    Roles.Add(49, "DEMANDA BARRAS");
                    Roles.Add(50, "HIDROLOGÍA");
                    Roles.Add(54, "RECLAMOS");
                    Roles.Add(60, "MEDIDORES GENERACIÓN");
                    Roles.Add(65, "CUMPLIMIENTO RPF");
                    Roles.Add(74, "TRANFERENCIAS");


                    foreach (KeyValuePair<int,string> item in Roles)
                    {
                        ListItem li = new ListItem(item.Value, item.Key.ToString());
                        li.Selected = false;
                        li.Attributes.Add("class", "item_checkbox");
                        chBoxModulos.Items.Add(li);
                    }

                    chBoxModulos.DataBind();
                }
            }
        }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            int li_resultado = 0, li_user_codi = 0;

            if (Session["REG_APP"] != null)
            {
                UsuarioExterno usuarioExterno = new UsuarioExterno()
                {
                    Nombre = UserName.Text.Trim(),
                    Apellido = UserSurName.Text.TrimEnd(),
                    Empresa = Convert.ToInt32(DropDownListEmpresa.SelectedValue),
                    Email = Email.Text.Trim().ToLower(),
                    Telefono = Phone.Text.TrimEnd(),
                    MotivoContacto = MotivoContacto.Text,
                    AreaLaboral = Area.Text,
                    Cargo = Cargo.Text
                };

                Dictionary<string, string> Usuarios = new Dictionary<string, string>();
                n_app Clase = (n_app)Session["REG_APP"];
                DataSet i_ds = new DataSet("dsusuario");
                string ls_comando = @" SELECT * FROM FW_USER U WHERE U.USERLOGIN = '" + usuarioExterno.Email.Trim().ToLower() + "'";
                Clase.Fill(0, i_ds, "FW_USER", ls_comando);
                foreach (DataRow dr in i_ds.Tables["FW_USER"].Rows)
                {
                    Usuarios[dr["USERCODE"].ToString()] = dr["USERNAME"].ToString();
                }
                Session["REG_USUARIO"] = Usuarios;

                if (Usuarios.Count >= 1)
                {
                    Response.Redirect("~/WebForm/Account/Registrado.aspx", true);
                }
                else
                {
                    Session["Usuario"] = usuarioExterno;
                    /*
                     *
                     * Aqui debemos enviar a base de datos INSERT INTO WB_USER
                     * Se envia noticacion
                     */

                    AdminService admin = new AdminService();
                    string ls_motivoContacto;
                    string ls_areaLaboral = usuarioExterno.AreaLaboral.Length > 49 ? usuarioExterno.AreaLaboral.Substring(0, 48) : usuarioExterno.AreaLaboral;
                    string ls_cargoUsuario = usuarioExterno.Cargo.Length > 49 ? usuarioExterno.Cargo.Substring(0, 48) : usuarioExterno.Cargo;
                    int cnt = 0;

                    ls_motivoContacto = usuarioExterno.MotivoContacto.Length > 299 ? usuarioExterno.MotivoContacto.Substring(0, 297) : usuarioExterno.MotivoContacto;

                    for (int i = 0; i < chBoxModulos.Items.Count; i++)
                    {
                        if (chBoxModulos.Items[i].Selected)
                        {
                            cnt++;
                        }
                    }

                    if (cnt > 0)
                    {
                        if (StringHelper.IsValidEmail(Email.Text.Trim()))
                        {
                            li_user_codi = admin.InsertUsuario(UserName.Text.TrimStart().TrimEnd() + " " + UserSurName.Text.TrimStart().TrimEnd(), Convert.ToInt32(DropDownListEmpresa.SelectedValue), Email.Text.Trim().ToLower(), Phone.Text.TrimStart().TrimEnd(), ls_motivoContacto, ls_cargoUsuario, ls_areaLaboral);

                            if (li_user_codi > 0)
                            {
                                li_resultado = 1;
                            }
                            else
                            {
                                Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                return;
                            }

                            if (chBoxModulos.Items[0].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admManttos"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Mantenimientos", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Mantenimientos </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "48");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - MANTENIMIENTO");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[1].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admDemanda"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Demanda en Barras", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Mantenimientos </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "49");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - DEMANDA EN BARRAS");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[2].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admHidrologia"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Hidrología", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Hidrología </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "50");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - HIDROLOGIA");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[3].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admReclamos"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Reclamos", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Reclamos </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "54");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - RECLAMOS");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[4].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admMedidores"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Medidores", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Medidores </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "60");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - MEDIDORES");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[5].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admCumplimientoRPF"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Cumplimiento RPF", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente a Cumplimiento RPF </p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "65");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - CUMPLIMIENTO RPF");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            if (chBoxModulos.Items[6].Selected && li_resultado > 0)
                            {
                                try
                                {
                                    ls_mailAdmExtranet = ConfigurationManager.AppSettings["admTransferencia"].ToString();
                                }
                                catch (Exception)
                                {
                                    admin.EnviaCorreoLog("webapp@coes.org.pe", "Error Extranet COES - Correo Administrador Transferencia", "<p>No se puede recuperar correo desde el web config, asegurese que se encuentre la key correspondiente al módulo Transferencias</p>", true);
                                }

                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "79");
                                if (li_resultado > 0)
                                {
                                    li_resultado = admin.EnviaCorreoToAdministrator(ls_mailAdmExtranet, UserName.Text, UserSurName.Text, Email.Text.Trim(), DropDownListEmpresa.SelectedItem.Text, MotivoContacto.Text, "SOLICITUD USUARIO EXTRANET - TRANSFERENCIAS");
                                }
                                else
                                {
                                    Util.Alert.Show("Existen Problemas al intentar registrar usuario. Contacte al administrador");
                                    return;
                                }

                            }
                            //Rol de Usuario Extranet
                            if (li_resultado > 0)
                                li_resultado = admin.AsignaRol(li_user_codi.ToString(), "userForRegister", "45");


                            if (li_resultado > 0)
                            {
                                //li_resultado = admin.EnviaCorreoRegistro(UserName.Text + " " + UserSurName.Text, Email.Text.Trim(), "REGISTRO EXTRANET: SE HA ENVIADO SU SOLICITUD ");
                                li_resultado = admin.SetEmpresasByCompcode(li_user_codi, usuarioExterno.Empresa, usuarioExterno.Email);
                            }
                        }
                    }
                    else
                    {
                        Util.Alert.Show("Seleccione un módulo para acceder al momento de registrarse");
                    }


                    if (li_resultado > 0)
                    {
                        Response.Redirect("~/WebForm/Account/Confirmacion.aspx");
                    }
                    else
                    {
                        Util.Alert.Show("Error en el ingreso de datos ó en la conexión a los servicios");
                    }
                    
                }
            }

        }

    }
}
