using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WScoes;
using System.Drawing;
using WSIC2010.Util;
using fwapp;

namespace WSIC2010.Admin
{
    public partial class w_adm_listarUsuarios : System.Web.UI.Page
    {
        n_app in_app;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Admin/w_adm_listarUsuarios.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();
                string ls_userRoles = String.Empty;

                if (admin.IsAdmin(in_app.ii_UserCode, out ls_userRoles))
                {
                    if (!IsPostBack)
                    {
                        Dictionary<int, string> Empresas = admin.GetEmpresas();

                        Dictionary<int, string> EmpresasOrdenadas = new Dictionary<int, string>();

                        foreach (KeyValuePair<int, string> item in Empresas.OrderBy(key => key.Value))
                        {
                            EmpresasOrdenadas.Add(item.Key, item.Value);
                        }

                        DDLEmpresa.DataSource = EmpresasOrdenadas;
                        DDLEmpresa.DataValueField = "key";
                        DDLEmpresa.DataTextField = "value";
                        DDLEmpresa.DataBind();
                        DDLEmpresa.SelectedIndex = 0;
                        gView.DataSource = admin.ListarUsuarios(Convert.ToInt32(DDLEstado.SelectedValue), Convert.ToInt32(DDLEmpresa.SelectedValue), Convert.ToInt32(ddlModulo.SelectedValue), new DateTime(2000, 01, 01), DateTime.Now.AddDays(1));
                        gView.DataBind();    
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
                
            }
        }

        private DataTable filtrar()
        {
            try
            {
                AdminService admin = new AdminService();
                DateTime ldt_fechaInicio = new DateTime(2000, 1, 1);
                if (!String.IsNullOrEmpty(tBoxInicio.Value))
                {
                    ldt_fechaInicio = EPDate.ToDate(tBoxInicio.Value);
                }
                DateTime ldt_fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);;
                if (!String.IsNullOrEmpty(tBoxFin.Value))
                {
                    ldt_fechaFin = EPDate.ToDate(tBoxFin.Value);
                }

                return admin.ListarUsuarios(Convert.ToInt32(DDLEstado.SelectedValue), Convert.ToInt32(DDLEmpresa.SelectedValue), Convert.ToInt32(ddlModulo.SelectedValue), ldt_fechaInicio, ldt_fechaFin.AddDays(1));
            }
            catch (Exception ex)
            {
                Label1.Text = "ERROR: No se pudo obtener los datos. Mas detalles: " + ex.Message;
                return null;
            }
        }

        private void ClearGridView(GridView gv)
        {
            gView.DataSource = null;
            gView.DataBind();
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            //bool lb_valido = true;
            //if (!string.isnullorempty(tboxinicio.value))
            //{
                
            //}
            gView.DataSource = filtrar();
            gView.DataBind();
        }

        protected void gView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gView.DataSource = filtrar();
            gView.PageIndex = e.NewPageIndex;
            gView.DataSource = filtrar();
            gView.DataBind(); 
        }

        protected void gView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowIndex == 0 )
                //{
                //    e.Row.Font.Bold = true;
                //    //e.Row.ForeColor = Color.FromName("#ffffff");
                //    //e.Row.BackColor = Color.FromName("#0000FF");
                //    //e.Row.Cells[0].Text = "HORA";
                //    /*e.Row.Cells[1].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                //    e.Row.Cells[2].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["02-EJECUTADO "]).ToString("ddd, MM/dd/yyyy");*/
                //    //e.Row.Cells[1].Text = Convert.ToString(((DataRowView)e.Row.DataItem)[1]);
                //    //e.Row.Cells[2].Text = Convert.ToString(((DataRowView)e.Row.DataItem)[2]);
                //}
                ///*else */if (e.Row.RowIndex != -1)
                //{
                //    /*e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("0.00");
                //    e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["02-EJECUTADO "]).ToString("0.00");*/
                //    //e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[1]).ToString("0.00");
                //    //e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[2]).ToString("0.00");
                //    //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                //    switch (e.Row.Cells[3].Text)
                //    {
                //        case "A":
                //            e.Row.Cells[3].Text = "Activo";
                //            break;
                //        case "B":
                //            e.Row.Cells[3].Text = "Baja";
                //            break;
                //        case "":
                //            e.Row.Cells[3].Text = "Pendiente";
                //            break;
                //        default:
                //            break;
                //    }
                    //switch (e.Row.Cells[4].Text)
                    //{
                    //    case "userForRegister":
                    //        e.Row.Cells[4].Text = String.Empty;
                    //        break;
                    //    default:
                    //        e.Row.Cells[4].Text = "ND";
                    //        break;
                    //}
                

                //    e.Row.Cells[4].Style.Add("text-align", "left");
                //}

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Font.Bold = true;
                }

                string ls_estado = "";
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ls_estado = DataBinder.Eval(e.Row.DataItem, "userstate").ToString();
                    //var main_content = Master.FindControl("MainContent");
                    //var bt_habilita = (Button)main_content.FindControl("habil_usuario");
                    //var gview = (GridView)main_content.FindControl("gView");
                    //Button bt_habilita = new Button() { ID = "bt_habil", CommandName = "Habilitar", CommandArgument = Convert.ToString(e.Row.DataItemIndex)};
                    //switch (ls_estado)
                    //{
                    //    case "A":
                    //        e.Row.Cells[3].Text = "Activo";
                            
                    //        //bt_habilita.Text = "Deshabilitar";
                    //        //e.Row.Cells[5].Controls.Add(bt_habilita);
                    //        break;
                    //    case "B":
                    //        e.Row.Cells[3].Text = "Baja";
                    //        //bt_habilita.Text = "Habilitar";
                    //        //e.Row.Cells[5].Controls.Add(bt_habilita);
                    //        break;
                    //    //case "":
                    //    case "P":
                    //        e.Row.Cells[3].Text = "Pendiente";
                    //        //bt_habilita.Text = "Habilitar";
                    //        //e.Row.Cells[5].Controls.Add(bt_habilita);
                    //        break;
                    //    default:
                    //        break;
                    //}


                    //Seteando align en left para columna Áreas
                    e.Row.Cells[4].Style.Add("text-align", "left");
                }

            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }

        protected void gView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Habilitar")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = gView.Rows[index];

                UsuarioExterno user = new UsuarioExterno()
                {
                    Codigo = Convert.ToInt32(row.Cells[0].Text),
                    Email = row.Cells[1].Text,
                    Nombre = row.Cells[2].Text,
                    Estado = new EstadoXModulo(Convert.ToInt32(ddlModulo.SelectedValue), DDLEstado.SelectedItem.Text),
                    AreaNombre = row.Cells[5].Text,
                    MotivoContacto = row.Cells[6].Text
                };

                Session["userSelected"] = user;

                Response.Redirect("~/WebForm/Admin/w_adm_activarUser.aspx", true);

            }

        }

        public string GetEstado(Object objEstado)
        {
            if (objEstado.ToString() == "A") 
            {
                
                return "Deshabilitar"; 
            }
            else 
            {
                return "Habilitar"; 
            }
        }

        public string GetUrlImage(Object objEstado)
        {
            if (objEstado.ToString() == "A") 
            {
                //return "~/images/icon-delete.gif";
                return "~/webform/images/WLIST16x16.png";
                //return "Deshabilitar"; 
            }
            else 
            {
                //return "Habilitar";
                //return "~/images/green_check_icon.png";
                return "~/webform/images/WLIST16x16.png";
            }
        }

        public string GetModulo(Object objCodigo)
        {
            int li_codigo = 0;
            int li_rolcode = 0;
            int li_userrolcheck, li_userrolvalidate;
            li_userrolcheck = li_userrolvalidate = 0;
            string ls_cadena_roles = String.Empty;
            string ls_rol = String.Empty;
            using(DataSet li_ds = new DataSet("dsroles"))
            {
                if (!String.IsNullOrEmpty(objCodigo.ToString()))
                {
                    if (Int32.TryParse(objCodigo.ToString(), out li_codigo))
                    {
                        string ls_comando = "SELECT ur.usercode, ur.rolcode, ur.userrolcheck, ur.userrolvalidate ";
                        ls_comando += "FROM FW_USERROL ur ";
                        ls_comando += "WHERE ur.rolcode IN (48, 49, 50, 54, 60) ";
                        ls_comando += "AND ur.usercode = " + li_codigo;
                        in_app.iL_data[0].Fill(li_ds, "FW_ROL", ls_comando);

                        ls_cadena_roles += "<ul>";

                        foreach (DataRow dr in li_ds.Tables[0].Rows)
                        {
                            if (Int32.TryParse(dr["ROLCODE"].ToString(), out li_rolcode))
                            {
                                li_userrolcheck = Int32.TryParse(dr["userrolcheck"].ToString(), out li_userrolcheck) ? li_userrolcheck : 0;
                                li_userrolvalidate = Int32.TryParse(dr["userrolvalidate"].ToString(), out li_userrolvalidate) ? li_userrolvalidate : 0;

                                if (li_userrolcheck == 1 && li_userrolvalidate == 1)
                                    ls_rol = "(A)";
                                else
                                    ls_rol = "(P)";

                                switch (li_rolcode)
                                {
                                    case 48:
                                        ls_cadena_roles += "<li>MANTTO" + ls_rol + "</li>";
                                        break;
                                    case 49:
                                        ls_cadena_roles += "<li>DEMANDA" + ls_rol + "</li>";
                                        break;
                                    case 50:
                                        ls_cadena_roles += "<li>HIDROLOGIA" + ls_rol + "</li>";
                                        break;
                                    case 54:
                                        ls_cadena_roles += "<li>RECLAMO" + ls_rol + "</li>";
                                        break;
                                    case 60:
                                        ls_cadena_roles += "<li>MEDIDORES" + ls_rol + "</li>";
                                        break;
                                    default:
                                        ls_cadena_roles += "<li>NO DEFINIDA</li>";
                                        break;
                                }
                            }

                            if (li_rolcode == 45)
                            {
                                break;
                            }
                           
                        }

                        ls_cadena_roles += "</ul>";
                    }
                }
            }

            return ls_cadena_roles;
        }
    }
}