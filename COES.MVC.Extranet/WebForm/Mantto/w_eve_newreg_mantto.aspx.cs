using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Globalization;
//using WSIC2010.WScoes;
using WScoes;
using WSIC2010.Util;

namespace WSIC2010
{
    public partial class w_eve_newreg_mantto : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

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
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Mantenimientos) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        Dictionary<int, string> H_evenclase = new Dictionary<int, string>();
                        string ls_comando;
                        if (Session["H_EVE_EVENCLASE"] == null)
                        {
                            DataSet i_ds = new DataSet("dslogin");
                            ls_comando = @" SELECT * FROM EVE_EVENCLASE ORDER BY EVENCLASECODI";

                            in_app.Fill(0, i_ds, "EVE_EVENCLASE", ls_comando);
                            foreach (DataRow dr in i_ds.Tables["EVE_EVENCLASE"].Rows)
                            {
                                H_evenclase[Convert.ToInt32(dr["EVENCLASECODI"])] = dr["EVENCLASEDESC"].ToString();
                            }
                            Session["H_EVE_EVENCLASE"] = H_evenclase;

                        }
                        else
                        {
                            H_evenclase = (Dictionary<int, string>)Session["H_EVE_EVENCLASE"];
                        }

                        DropDownListEvenClase.DataSource = H_evenclase;
                        DropDownListEvenClase.DataTextField = "value";
                        DropDownListEvenClase.DataValueField = "key";
                        DropDownListEvenClase.DataBind();

                        DropDownListEvenClase.SelectedIndex = 1;
                        CalendarExtender1.SelectedDate = DateTime.Today.AddDays(1);
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }

            
        }



        protected void ButtonCrear_Click(object sender, EventArgs e)
        {
            ManttoService service = null;
            if (!EPDate.IsDate(TextBoxFechaInicio.Text))
            {
                LabelError.Text = "Formato de Fecha Erroneo, deberia ser dd/mm/aaaa.";
                return;
            }
            DateTime ldt_fecha = EPDate.ToDate(TextBoxFechaInicio.Text);
            if (ldt_fecha > DateTime.Today.AddMonths(5) || ldt_fecha < DateTime.Today.AddDays(-7))
            {
                LabelMensaje.Text = "Fecha fuera de rango.";
                return;
            }

            service = new ManttoService();


            if (DiferenciaFecha.HoraValida(ldt_fecha, 10, DropDownListEvenClase.SelectedItem.Value) && DropDownListEvenClase.SelectedItem.Value.Equals("2"))
            {
                LabelMensaje.Text = "Creación Programa Diario fuera de plazo.";
                return;
            }

            if (DiferenciaFecha.HoraValida(ldt_fecha, 14, DropDownListEvenClase.SelectedItem.Value) && DropDownListEvenClase.SelectedItem.Value.Equals("3"))
            {
                LabelMensaje.Text = "Creación Programa Semanal fuera de plazo.";
                return;
            }

            if (!service.ValidateRegMan(ldt_fecha, Convert.ToInt32(DropDownListEvenClase.SelectedItem.Value)))
            {
                LabelMensaje.Text = "Ya existe opción en el listado de registros.";
                return;
            }

            if (Session["in_app"] != null)
            {
                n_app in_app = (n_app)Session["in_app"];
                //ManttoServiceClient service = new ManttoServiceClient();
                service = new ManttoService();
                
                int li_key = service.CreateRegMan(EPDate.ToDate(TextBoxFechaInicio.Text), TextBoxDescripcion.Text, Convert.ToInt32(DropDownListEvenClase.SelectedValue), in_app.is_UserLogin + ":" + in_app.is_PC_IPs);
                if (li_key == -2)
	            {
                    LabelMensaje.Text = "Fecha no es inicio de semana operativa";
                    return;
	            }
                
                if (li_key > 0)
                {
                    LabelMensaje.Text ="Error en generacion de registro.";
                }
                
                if (Session["ReturnPage"] != null)
                {
                    //Response.Redirect(Convert.ToString(Session["ReturnPage"]));
                    Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx");
                }
                else
                {
                    Response.Redirect("~/WebForm/Default.aspx");
                }
                //string elmer;
                //elmer.PadRight(12)
            }
        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {

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

    }
}