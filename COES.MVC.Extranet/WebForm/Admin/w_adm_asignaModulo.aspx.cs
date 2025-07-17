using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;

namespace WSIC2010.Admin
{
    public partial class w_adm_asignaModulo : System.Web.UI.Page
    {
        n_app in_app;
        AdminService admin;
        Dictionary<int, string> ld_rolesSolicitados;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Admin/w_adm_asignaModulo.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                admin = new AdminService();

                if (!IsPostBack)
                {
                    ld_rolesSolicitados = admin.GetRolesSolicitadosXUsuario(in_app.ii_UserCode);

                    if(!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Demanda)))
                        ddlModulos.Items.Add(new ListItem("DEMANDA", ((Int32)Role.Usuario_Demanda).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Mantenimientos)))
                        ddlModulos.Items.Add(new ListItem("MANTENIMIENTOS", ((Int32)Role.Usuario_Mantenimientos).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Hidrologia)))
                        ddlModulos.Items.Add(new ListItem("HIDROLOGIA", ((Int32)Role.Usuario_Hidrologia).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Reclamo)))
                        ddlModulos.Items.Add(new ListItem("RECLAMO", ((Int32)Role.Usuario_Reclamo).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Medidores)))
                        ddlModulos.Items.Add(new ListItem("MEDIDORES", ((Int32)Role.Usuario_Medidores).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_CumplimientoRPF)))
                        ddlModulos.Items.Add(new ListItem("CUMPLIMIENTO RPF", ((Int32)Role.Usuario_CumplimientoRPF).ToString()));
                    if (!ld_rolesSolicitados.Keys.Contains(Convert.ToInt32(Role.Usuario_Transferencias)))
                        ddlModulos.Items.Add(new ListItem("TRANSFERENCIAS", ((Int32)Role.Usuario_Transferencias).ToString()));

                    if (ddlModulos.Items.Count <= 0)
                    {
                        ddlModulos.Enabled = false;
                        lBox_modulosAsignados.Enabled = false;
                        bt_Agregar_Rol.Enabled = false;
                        bt_Eliminar_Rol.Enabled = false;
                        bt_Aceptar.Enabled = false;
                        string myScript = @"alert('No presenta roles por agregar');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                    }
                        
                    
                    ddlModulos.DataBind();
                }

            }
        }

        protected void bt_AgregarRol_Click(object sender, EventArgs e)
        {
            admin = new AdminService();
            if (ddlModulos.SelectedIndex != -1)
            {  
                if (!String.IsNullOrEmpty(ddlModulos.SelectedItem.Value.Trim()) && !NoAsignado(ddlModulos.SelectedItem.Value.Trim(), lBox_modulosAsignados.Items))
                {
                    lBox_modulosAsignados.Items.Add(new ListItem(ddlModulos.SelectedItem.Text, ddlModulos.SelectedItem.Value.Trim()));
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
                string myScript = @"alert('No se seleccionó rol solicitado');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }

        }

        private bool nf_get_roles(Dictionary<int, string> ld_modulosXAdmin, string ps_codigoModulo)
        {
            bool lb_valor = false;

            if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Mantenimientos)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Demanda)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Hidrologia)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Reclamo)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Medidores)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_CumplimientoRPF)))
                lb_valor = true;
            else if (ld_modulosXAdmin.Keys.Contains(Convert.ToInt32(Role.Usuario_Transferencias)))
                lb_valor = true;
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
                while (lBox_modulosAsignados.SelectedIndex != -1)
                {
                    ListItem mySelectedItem = (from ListItem li in lBox_modulosAsignados.Items where li.Selected == true select li).First();
                    lBox_modulosAsignados.Items.Remove(mySelectedItem);
                    if (lBox_modulosAsignados.Items.Count > 0)
                        lBox_modulosAsignados.Rows = lBox_modulosAsignados.Items.Count;
                };
            }
            else
            {
                string myScript = @"alert('No se seleccionó módulo a eliminar');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
            }
        }

        protected void bt_Aceptar_Click(object sender, EventArgs e)
        {
            admin = new AdminService();
            int li_valor, li_resultado;
            li_valor = li_resultado = 0;
            string ls_resultado = String.Empty;

            foreach (ListItem item in lBox_modulosAsignados.Items)
            {
                if (Int32.TryParse(item.Value, out li_valor) && li_resultado >= 0)
                {
                    li_resultado = admin.SetRoles(in_app.ii_UserCode, li_valor);
                }
                else
                {
                    ls_resultado += "Existen errores al momentos de agregar rol seleccionado: " +  item.Text + "<br />";
                }
            }

            if (ls_resultado.Equals(String.Empty))
                ls_resultado = "Los módulos solicitados se registraron satisfactoriamente";

            label_resultado.Text = ls_resultado;
        }
    }
}