using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using WSIC2010.Util;
using System.Data;
using System.Configuration;

namespace WSIC2010.Admin
{
    public partial class w_adm_activarUser : System.Web.UI.Page
    {
        n_app in_app;
        AdminService admin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Admin/w_adm_activarUser.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                admin = new AdminService();
                string ls_userRoles = String.Empty;

                if(admin.IsAdmin(in_app.ii_UserCode, out ls_userRoles))
                {
                    if (!IsPostBack)
                    {
                        if (Session["userSelected"] != null)
                        {
                            DateTime ldt_fecha = new DateTime(2000, 1, 1);
                            UsuarioExterno usuario = (UsuarioExterno)Session["userSelected"];
                            DataRow drow = admin.GetInfoUsuario(usuario.Codigo.ToString());

                            label_user.Text = usuario.Nombre;
                            /*Agregando estado*/
                            Dictionary<int, string> ld_rolesSolicitados = admin.GetRolesSolicitadosXUsuario(usuario.Codigo);
                            foreach (KeyValuePair<int, string> item in ld_rolesSolicitados)
                            {
                                lBox_modulosSolicitados.Items.Add(new ListItem(item.Value.ToUpper().Replace("USUARIO", String.Empty).Replace("EXTRANET -", String.Empty).Replace("userForRegister", String.Empty), item.Key.ToString()));
                            }

                            if(lBox_modulosSolicitados.Items.Count > 0)
                                lBox_modulosSolicitados.Rows = lBox_modulosSolicitados.Items.Count;

                            Dictionary<int, string> ld_rolesAsignados = admin.GetRolesAsignadosXUsuario(usuario.Codigo);
                            foreach (KeyValuePair<int, string> item in ld_rolesAsignados)
                            {
                                lBox_modulosAsignados.Items.Add(new ListItem(item.Value.ToUpper().Replace("USUARIO", String.Empty).Replace("EXTRANET -", String.Empty), item.Key.ToString()));
                            }

                            if (lBox_modulosAsignados.Items.Count > 0)
                                lBox_modulosAsignados.Rows = lBox_modulosAsignados.Items.Count;

                            label_email.Text = usuario.Email;
                            label_empresa.Text = usuario.AreaNombre;
                            label_fec_creacion.Text = DateTime.TryParse(drow["USERFCREACION"].ToString().Trim(), out ldt_fecha) ? ldt_fecha.ToString("dd/MM/yyyy H:mm") : "ND";
                            label_fec_activacion.Text = DateTime.TryParse(drow["USERFACTIVACION"].ToString().Trim(), out ldt_fecha) ? ldt_fecha.ToString("dd/MM/yyyy H:mm") : "ND";
                            label_fec_baja.Text = DateTime.TryParse(drow["USERFBAJA"].ToString().Trim(), out ldt_fecha) ? ldt_fecha.ToString("dd/MM/yyyy H:mm") : "ND";
                            label_telefono.Text = drow["USERTLF"].ToString().Trim();
                            label_motivo.Text = usuario.MotivoContacto;


                            ddlEmpresa.DataSource = admin.GetEmpresasSEIN();
                            ddlEmpresa.DataTextField = "value";
                            ddlEmpresa.DataValueField = "key";
                            ddlEmpresa.DataBind();

                            ddlEmpresa.SelectedIndex = 1;
                             

                            string[] array_empresas = admin.GetEmpresasxUsuario(Convert.ToInt32(usuario.Codigo)).Replace(" ", "").Split(new char[]{','});
                            Dictionary<string, string> ld_Empresas = new Dictionary<string, string>();

                            if (array_empresas.Length > 0 && !String.IsNullOrEmpty(array_empresas[0]))
                            {
                                for (int i = 0; i < array_empresas.Length; i++)
                                {
                                    ld_Empresas.Add(array_empresas[i], admin.GetEmpresasSEIN(array_empresas[i]));
                                }

                                lBoxEmpresas.DataSource = ld_Empresas;
                                lBoxEmpresas.DataTextField = "value";
                                lBoxEmpresas.DataValueField = "key";
                                if (lBoxEmpresas.Items.Count > 0)
                                    lBoxEmpresas.Rows = lBoxEmpresas.Items.Count;
                                lBoxEmpresas.DataBind();
                            }

                        }
                        else
                        {
                            Response.Redirect("~/WebForm/Admin/w_adm_listarUsuarios.aspx");
                        }

                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }
        }

        protected void bt_Aceptar_Click(object sender, EventArgs e)
        {
            if (lBoxEmpresas.Items.Count > 0 && lBox_modulosAsignados.Items.Count > 0)
            {
                UsuarioExterno usuario = (UsuarioExterno)Session["userSelected"];
                string ls_usercode = usuario.Codigo.ToString();
                string ls_userName = usuario.Nombre;
                string ls_estado = usuario.Estado.DescripcionEstado;
                int li_resultado = 0;
                string ls_empresas = String.Empty;
                string ls_nombre_empresas = String.Empty;
                string ls_nombre_modulos = String.Empty;
                string ls_nombre_modulos2 = String.Empty;
                List<int> list_codigos_empresas = new List<int>();
                List<int> list_codigos_roles = new List<int>();
                List<int> list_codigos_roles_to_reset = new List<int>();
                Dictionary<int, int> li_codigoRoles;
                string ls_mail_admin = ConfigurationManager.AppSettings["ListaAdmin"].ToString();

                if (!String.IsNullOrEmpty(ls_estado) && (!String.IsNullOrEmpty(ls_userName)))
                {
                    admin = new AdminService();
                    li_codigoRoles = admin.GetRolesAsigXUsuario(Convert.ToInt32(ls_usercode));
                    if (li_codigoRoles.Count == 0)
                    {
                        li_resultado = admin.ActualizaEstado(ls_usercode.Trim(), "Pendiente", in_app.is_UserLogin);// Actualizo Estado a Activo (A)
                        if (li_resultado > 0)
                        {
                            if (li_resultado > 0)
                            {
                                //Recupera Correo
                                DataRow drow = admin.GetInfoUsuario(ls_usercode);
                                int li_usercode = Convert.ToInt32(drow["usercode"].ToString());
                                int li_areacode = Convert.ToInt32(drow["areacode"].ToString());

                                for (int i = 0; i < lBoxEmpresas.Items.Count; i++)
                                {
                                    list_codigos_empresas.Add(Convert.ToInt32(lBoxEmpresas.Items[i].Value));
                                    ls_nombre_empresas += lBoxEmpresas.Items[i].Text + "<br />";
                                }
                                ls_empresas = String.Join(", ", list_codigos_empresas.Distinct().ToList());

                                for (int i = 0; i < lBox_modulosAsignados.Items.Count; i++)
                                {
                                    list_codigos_roles.Add(Convert.ToInt32(lBox_modulosAsignados.Items[i].Value));
                                    ls_nombre_modulos += lBox_modulosAsignados.Items[i].Text + "<br />";
                                    
                                    //Agregando roles
                                    string[] ls_array_cadena = lBox_modulosAsignados.Items[i].Text.Split(new char[] { '-' });
                                    if(ls_array_cadena != null && ls_array_cadena.Length > 0)
                                        ls_nombre_modulos2 += ls_array_cadena[0] + "<br />";
                                }

                                int li_result = 0;
                                string ls_userlogin = drow["userlogin"].ToString();
                                int li_usercheck = Convert.ToInt32(drow["usercheck"].ToString());
                                int li_uservalidate = Int32.TryParse(drow["uservalidate"].ToString(), out li_result) ? li_result : 0;
                                bool lb_enviado = false;
                                if(li_usercheck > 0 && li_uservalidate > 0)
                                    lb_enviado = true;
                                string ls_userpass = StringHelper.nf_get_random_str(6);
                                bool lb_positivo = false;
                                int li_valor = 0;

                                if (li_uservalidate == 0)
                                {
                                    while (!lb_positivo)
                                    {
                                        li_valor = StringHelper.f_getpass(li_usercode + 1000 * li_areacode, ls_userlogin.Trim() + ls_userpass.Trim()) + 2 * (li_usercheck - 10000);
                                        if (li_valor > 0)
                                        {
                                            lb_positivo = true;
                                        }
                                    }

                                    li_uservalidate = li_valor;

                                    li_resultado = admin.SetPassword(li_usercode, li_uservalidate);
                                }

                                //agregando los modulos
                                if (list_codigos_roles != null && list_codigos_roles.Count > 0)
                                { 
                                    foreach (int item in list_codigos_roles)
	                                {
                                        if (li_resultado > 0)
                                            li_resultado = admin.ActualizaRol(li_usercode, item, in_app.is_UserLogin);
                                        else
                                        {
                                            string myScript = @"alert('Error al agregar rol');";
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                        }
                                            
	                                }   
                                }

                                //agregando las empresas al usuario
                                if (li_resultado >= 0)
                                {
                                    li_resultado = admin.SetEmpresas(li_usercode, ls_empresas, in_app.is_UserLogin);
                                }
                                else
                                {
                                    string myScript = @"alert('Error al agregar empresa');";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                }

                                //Enviar el correo
                                if (li_valor >= 0)
                                {                                    
                                    if (!lb_enviado)
                                    {
                                        li_resultado = admin.EnviaCorreoAlta(ls_userName, ls_userlogin, ls_userpass, "EXTRANET COES: REGISTRO USUARIO SATISFACTORIO", "http://contenido.coes.org.pe/alfrescostruts/download.do?nodeId=d74862fd-979a-49a4-a238-0c00c31ef53b");
                                        //Agregando el modulo de reclamos
                                        if (li_resultado > 0)
                                            li_resultado = admin.SetRoles(li_usercode, (int)Role.Usuario_Reclamo);
                                        if (li_resultado > 0)
                                            li_resultado = admin.ActualizaRol(li_usercode, (int)Role.Usuario_Reclamo, in_app.is_UserLogin);
                                    }

                                    if (li_resultado > 0)
                                    {
                                        li_resultado = admin.EnviarNotificacionAdministrador(true, ls_userName, ls_mail_admin, ls_userlogin,ls_nombre_empresas, ls_nombre_modulos, "EXTRANET COES: HABILITACION DE ACCESO A MODULOS EXTRANET USUARIO: " + ls_userlogin);
                                        if (li_resultado > 0)
                                        {
                                            li_resultado = admin.EnviarNotificacionUsuario(true, ls_userName, ls_userlogin, ls_nombre_empresas, ls_nombre_modulos2, "EXTRANET COES: HABILITACION DE ACCESO A MODULOS EXTRANET");
                                            if (li_resultado > 0)
                                            {
                                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Se asigno rol al usuario');document.location.href='./w_adm_listarUsuarios.aspx';", true);
                                            }
                                            else
                                            {
                                                string myScript = @"alert('Error al enviar correo');";
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                            }
                                        }
                                        else
                                        {
                                            string myScript = @"alert('Error al enviar correo');";
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                Alert.Show("Error al momento de habilitar el usuario");
                            }
                        }
                    }
                    else //Para un usuario activo
                    {
                        DataRow drow = admin.GetInfoUsuario(ls_usercode);
                        int li_usercode = Convert.ToInt32(drow["usercode"].ToString());
                        int li_areacode = Convert.ToInt32(drow["areacode"].ToString());
                        string ls_userlogin = drow["userlogin"].ToString();

                        for (int i = 0; i < lBoxEmpresas.Items.Count; i++)
                        {
                            list_codigos_empresas.Add(Convert.ToInt32(lBoxEmpresas.Items[i].Value));
                            ls_nombre_empresas += lBoxEmpresas.Items[i].Text + "<br />";
                        }
                        ls_empresas = String.Join(",", list_codigos_empresas.Distinct().ToList());

                        //agregando los modulos
                        for (int i = 0; i < lBox_modulosAsignados.Items.Count; i++)
                        {
                            if (!li_codigoRoles.Keys.Contains(Convert.ToInt32(lBox_modulosAsignados.Items[i].Value)))
                                list_codigos_roles.Add(Convert.ToInt32(lBox_modulosAsignados.Items[i].Value));

                            ls_nombre_modulos += lBox_modulosAsignados.Items[i].Text + "<br />";

                            //Agregando roles
                            string[] ls_array_cadena = lBox_modulosAsignados.Items[i].Text.Split(new char[] { '-' });
                            if (ls_array_cadena != null && ls_array_cadena.Length > 0)
                                ls_nombre_modulos2 += ls_array_cadena[0] + "<br />";
                        }

                        
                        if (list_codigos_roles != null && list_codigos_roles.Count > 0)
                        {
                            foreach (int item in list_codigos_roles)
                            {
                                if (li_resultado >= 0)
                                    li_resultado = admin.ActualizaRol(li_usercode, item, in_app.is_UserLogin);
                                else
                                {
                                    string myScript = @"alert('Error al agregar rol');";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                }

                            }
                        }

                        //eliminando modulos
                        if (li_codigoRoles.Count > lBox_modulosAsignados.Items.Count)
                        {
                            int li_elemento = 0;
                            list_codigos_roles_to_reset = li_codigoRoles.Keys.ToList<int>();
                            for (int i = 0; i < li_codigoRoles.Count; i++)
                            {
                                for(int li = 0; li < lBox_modulosAsignados.Items.Count; li++) 
                                {
                                    li_elemento = Convert.ToInt32(lBox_modulosAsignados.Items[li].Value);
                                    if (li_codigoRoles.ContainsKey(li_elemento))
                                    {
                                        list_codigos_roles_to_reset.Remove(li_elemento);
                                    }
                                }
                            }


                            if (list_codigos_roles_to_reset != null && list_codigos_roles_to_reset.Count > 0)
                            {
                                foreach (int item in list_codigos_roles_to_reset)
                                {
                                    if (li_resultado >= 0)
                                        li_resultado = admin.ResetRol(li_usercode, item, in_app.is_UserLogin);
                                    else
                                    {
                                        string myScript = @"alert('Error al eliminar rol');";
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                    }

                                }
                            }
                        }


                        //agregando las empresas al usuario
                        if (li_resultado >= 0)
                        {
                            li_resultado = admin.SetEmpresas(li_usercode, ls_empresas, in_app.is_UserLogin);
                        }

                        if (li_resultado > 0)
                        {
                            li_resultado = admin.EnviarNotificacionAdministrador(false, ls_userName, ls_mail_admin, ls_userlogin, ls_nombre_empresas, ls_nombre_modulos, "EXTRANET COES: HABILITACION DE ACCESO A MODULOS EXTRANET USUARIO: " + ls_userlogin);
                            if(li_resultado > 0)
                            {
                                li_resultado = admin.EnviarNotificacionUsuario(false, ls_userName, ls_userlogin, ls_nombre_empresas, ls_nombre_modulos2, "EXTRANET COES: HABILITACION DE ACCESO A MODULOS EXTRANET");
                                if (li_resultado > 0)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Se guardaron los cambios efectuados');document.location.href='./w_adm_listarUsuarios.aspx';", true);
                                }
                                else
                                {
                                    string myScript = @"alert('Error al enviar correo');";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                                }
                            }
                            else
                            {
                                string myScript = @"alert('Error al enviar correo');";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                            }
                        }


                    }
                }
            }
            else if (lBoxEmpresas.Items.Count == 0)
            {
                Alert.Show("Se debe seleccionar al menos una empresa");
            }
            else if (lBox_modulosAsignados.Items.Count == 0)
            {
                UsuarioExterno usuario = (UsuarioExterno)Session["userSelected"];
                string ls_usercode = usuario.Codigo.ToString();
                int li_usercode = Convert.ToInt32(ls_usercode);
                string ls_userName = usuario.Nombre;
                string ls_estado = usuario.Estado.DescripcionEstado;
                int li_resultado = 0;
                string ls_empresas = String.Empty;
                string ls_nombre_empresas = String.Empty;
                string ls_nombre_modulos = String.Empty;
                string ls_nombre_modulos2 = String.Empty;
                List<int> list_codigos_empresas = new List<int>();
                List<int> list_codigos_roles = new List<int>();
                List<int> list_codigos_roles_to_reset = new List<int>();
                Dictionary<int, int> li_codigoRoles;
                string ls_mail_admin = ConfigurationManager.AppSettings["ListaAdmin"].ToString();

                //Deberiamos asignar estado Eliminado
                //li_resultado = admin.ActualizaEstado(ls_usercode.Trim(), "Activo", in_app.is_UserLogin);

                admin = new AdminService();
                li_codigoRoles = admin.GetRolesAsigXUsuario(Convert.ToInt32(ls_usercode));

                if (li_codigoRoles != null && li_codigoRoles.Count > 0)
                {
                    foreach (KeyValuePair<int, int> item in li_codigoRoles)
                    {
                        if (li_resultado >= 0)
                            li_resultado = admin.ResetRol(li_usercode, item.Key, in_app.is_UserLogin);
                        else
                        {
                            string myScript = @"alert('Error al eliminar rol');";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                        }

                    }
                }

                DataRow drow = admin.GetInfoUsuario(ls_usercode);
                string ls_userlogin = Convert.ToString(drow["userlogin"].ToString());
                

                //agregando las empresas al usuario
                if (li_resultado >= 0)
                {
                    li_resultado = admin.SetEmpresas(li_usercode, ls_empresas, in_app.is_UserLogin);
                }

                if (li_resultado > 0)
                {
                    li_resultado = admin.EnviarNotificacionAdministrador(false, ls_userName, ls_mail_admin, ls_userlogin, ls_nombre_empresas, ls_nombre_modulos, "EXTRANET COES: ELIMINACION DE MODULOS EXTRANET USUARIO: " + ls_userlogin);
                    if (li_resultado > 0)
                    {
                        li_resultado = admin.EnviarNotificacionUsuario(false, ls_userName, ls_userlogin, ls_nombre_empresas, ls_nombre_modulos2, "EXTRANET COES: HABILITACION DE ACCESO A MODULOS EXTRANET");
                        if (li_resultado > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Se guardaron los cambios efectuados');document.location.href='./w_adm_listarUsuarios.aspx';", true);
                        }
                        else
                        {
                            string myScript = @"alert('Error al enviar correo');";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                        }
                    }
                    else
                    {
                        string myScript = @"alert('Error al enviar correo');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                    }
                }
            }
            
        }

        protected void bt_Cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/Admin/w_adm_listarUsuarios.aspx");
        }

        protected void bt_Agregar_Click(object sender, EventArgs e)
        {
            int li_numero_maximo_empresas = 10;
            if (!String.IsNullOrEmpty(ddlEmpresa.SelectedItem.Value.Trim()) && !NoAsignado(ddlEmpresa.SelectedItem.Value.Trim(), lBoxEmpresas.Items))
            {
                if (lBoxEmpresas.Items.Count < li_numero_maximo_empresas + 1)
                {
                    lBoxEmpresas.Items.Add(new ListItem(ddlEmpresa.SelectedItem.Text, ddlEmpresa.SelectedItem.Value.Trim()));
                    if (lBoxEmpresas.Items.Count > 0)
                        lBoxEmpresas.Rows = lBoxEmpresas.Items.Count;
                    lBoxEmpresas.DataBind();
                }
                else
                {
                    string myScript = @"alert('Agregar como máximo " + li_numero_maximo_empresas.ToString() + " empresas');";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                }
            }
            else
            {
                string myScript = @"alert('Ya se agregó empresa seleccionada');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }
        }

        protected void bt_Eliminar_Click(object sender, EventArgs e)
        {
            if (lBoxEmpresas.SelectedIndex != -1)
            {
                while (lBoxEmpresas.SelectedIndex != -1)
                {
                    ListItem mySelectedItem = (from ListItem li in lBoxEmpresas.Items where li.Selected == true select li).First();
                    lBoxEmpresas.Items.Remove(mySelectedItem);
                    if (lBoxEmpresas.Items.Count > 0)
                        lBoxEmpresas.Rows = lBoxEmpresas.Items.Count;
                };
            }
            else
            {
                string myScript = @"alert('No se seleccionó empresa a eliminar');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }
            
        }

        private void ClearAll()
        {

        }

        private void EnableControls(bool ab_valor)
        {
            /*Deshabilitando*/
            ddlEmpresa.Enabled = ab_valor;
            lBox_modulosSolicitados.Enabled = ab_valor;
            lBoxEmpresas.Enabled = ab_valor;
            lBox_modulosSolicitados.Enabled = ab_valor;
            lBox_modulosAsignados.Enabled = ab_valor;
            bt_Agregar.Enabled = ab_valor;
            bt_Eliminar.Enabled = ab_valor;
            bt_Agregar_Rol.Enabled = ab_valor;
            bt_Eliminar_Rol.Enabled = ab_valor;
            bt_Aceptar.Enabled = ab_valor;
            bt_Cancelar.Enabled = ab_valor;
            

            /*Deshabilitando*/
            bt_Agregar.Visible = ab_valor;
            bt_Eliminar.Visible = ab_valor;
            bt_Agregar_Rol.Visible = ab_valor;
            bt_Eliminar_Rol.Visible = ab_valor;
            bt_Agregar_Rol.Visible = ab_valor;
            bt_Eliminar_Rol.Visible = ab_valor;
            bt_Aceptar.Visible = ab_valor;
            bt_Cancelar.Visible = ab_valor;

        }

        protected void bt_AgregarRol_Click(object sender, EventArgs e)
        {
            IAdminService admin = new AdminService();
            if (lBox_modulosSolicitados.SelectedIndex != -1)
            {
                if (nf_get_admin(admin.GetRolesAsigXAdmin(in_app.ii_UserCode), lBox_modulosSolicitados.SelectedItem.Value))
                {
                    if (!String.IsNullOrEmpty(lBox_modulosSolicitados.SelectedItem.Value.Trim()) && !NoAsignado(lBox_modulosSolicitados.SelectedItem.Value.Trim(), lBox_modulosAsignados.Items))
                    {
                        lBox_modulosAsignados.Items.Add(new ListItem(lBox_modulosSolicitados.SelectedItem.Text + " - " + in_app.is_UserLogin + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), lBox_modulosSolicitados.SelectedItem.Value.Trim()));
                        if (lBox_modulosAsignados.Items.Count > 0)
                            lBox_modulosAsignados.Rows = lBox_modulosAsignados.Items.Count;
                        lBox_modulosAsignados.DataBind();
                    }
                    else
                    {
                        string myScript = @"alert('Ya se agregó rol selecccionado');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                    }
                }
                else
	            {
                    string myScript = @"alert('No tiene privilegios para asignar modulo');";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
	            }
            }
            else
            {
                string myScript = @"alert('No se seleccionó rol solicitado');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }
            
        }

        private bool nf_get_admin(Dictionary<int, int> ld_modulosXAdmin, string ps_codigoModulo)
        {
            bool lb_valor = false;

            if(ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(ps_codigoModulo)))
                    lb_valor =  true;
            else
                    lb_valor = false;
            
            return lb_valor;
        }

        private bool NoAsignado(string as_value, ListItemCollection alc_items)
        {
            bool lb_existe = false;
            foreach (ListItem item in alc_items)
            {
                if (item.Value.Equals(as_value))
                {
                    lb_existe = true;
                }
            }

            return lb_existe;
        }

        protected void bt_EliminarRol_Click(object sender, EventArgs e)
        {
            if (lBox_modulosAsignados.SelectedIndex != -1)
            {
                if (nf_get_admin(admin.GetRolesAsigXAdmin(in_app.ii_UserCode), lBox_modulosAsignados.SelectedItem.Value))
                {
                    while (lBox_modulosAsignados.SelectedIndex != -1)
                    {
                        ListItem mySelectedItem = (from ListItem li in lBox_modulosAsignados.Items where li.Selected == true select li).First();
                        lBox_modulosAsignados.Items.Remove(mySelectedItem);
                        if (lBox_modulosAsignados.Items.Count > 0)
                            lBox_modulosAsignados.Rows = lBox_modulosAsignados.Items.Count;
                    };

                    if (lBox_modulosAsignados.Items.Count == 0)
                    {
                        string myScript = @"alert('Si el usuario no presenta módulos asociados se procederá a eliminar');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                    }
                }
                else
                {
                    string myScript = @"alert('No tiene privilegios para retirar modulo seleccionado');";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                }
            }
            else
            {
                string myScript = @"alert('No se seleccionó módulo a eliminar');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }
        }

    }
}