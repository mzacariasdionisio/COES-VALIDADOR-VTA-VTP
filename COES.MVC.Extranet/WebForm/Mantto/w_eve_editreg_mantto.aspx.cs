using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;

namespace WSIC2010.Mantto
{
    public partial class w_eve_editreg_mantto : System.Web.UI.Page
    {
        int ii_regcodi = -1;

        CManttoRegistro ManRegistro;
        n_app in_app;

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
                string ls_userRoles = String.Empty;
                Dictionary<int, string> ld_horarios = new Dictionary<int,string>();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Administrador_Mantenimientos))
                {
                    if (!IsPostBack)
                    {
                        if (Session["i_regcodi"] != null)
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
                            

                            ii_regcodi = (int)Session["i_regcodi"];
                            ManttoService manservice = new ManttoService();
                            ManRegistro = manservice.GetManttoRegistro(ii_regcodi);

                            //Asigna tipo de programa
                            DropDownListEvenClase.SelectedIndex = ManRegistro.EvenClaseCodi - 1;
                            DropDownListEvenClase.DataBind();

                            //Asigna fecha de inicio
                            if (ManRegistro.FechaInicial != null)
                            {
                                TextBoxFechaInicio.Text = ManRegistro.FechaInicial.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                TextBoxFechaInicio.Text = "NO DEFINIDO";
                            }

                            //DateTime ldt_fechaLimite = manservice.GetFechaLimitePrograma(ManRegistro.FechaInicial, ManRegistro.EvenClaseCodi);

                            if (ManRegistro.FechaLimiteFinal > (new DateTime(2013, 1, 1)))
                            {
                                //TextBoxFechaLimite.Text = ldt_fechaLimite.ToString("dd/MM/yyyy");
                                TextBoxFechaLimite.Text = ManRegistro.FechaLimiteFinal.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                TextBoxFechaLimite.Text = String.Empty;
                            }
                            

                            for (int i = 0; i < 24; i++)
                            {
                                ld_horarios.Add(i, i.ToString());
                            }

                            ddl_hora.DataSource = ld_horarios;
                            ddl_hora.DataTextField = "value";
                            ddl_hora.DataValueField = "key";

                            if (ManRegistro.FechaLimiteFinal > (new DateTime(2013, 1, 1)))
                                ddl_hora.SelectedIndex = Convert.ToInt32(ManRegistro.FechaLimiteFinal.ToString("HH"));

                            ddl_hora.DataBind();

                            for (int i1 = 24; i1 < 60; i1++)
                            {
                                ld_horarios.Add(i1, i1.ToString());
                            }

                            ddl_min.DataSource = ld_horarios;
                            ddl_min.DataTextField = "value";
                            ddl_min.DataValueField = "key";

                            if (ManRegistro.FechaLimiteFinal > (new DateTime(2013, 1, 1)))
                                ddl_min.SelectedIndex = Convert.ToInt32(ManRegistro.FechaLimiteFinal.ToString("mm"));

                            ddl_min.DataBind();

                            ddl_seg.DataSource = ld_horarios;
                            ddl_seg.DataTextField = "value";
                            ddl_seg.DataValueField = "key";

                            if (ManRegistro.FechaLimiteFinal > (new DateTime(2013, 1, 1)))
                                ddl_seg.SelectedIndex = Convert.ToInt32(ManRegistro.FechaLimiteFinal.ToString("ss"));

                            ddl_seg.DataBind();
                        }
                        else
                        {
                            Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void ButtonCrearFechaLim_Click(object sender, EventArgs e)
        {
            if(EPDate.IsDate(TextBoxFechaLimite.Text))
            {
                if (Session["i_regcodi"] != null)
                {
                    ii_regcodi = (int)Session["i_regcodi"];
                    ManttoService manservice = new ManttoService();
                    ManRegistro = manservice.GetManttoRegistro(ii_regcodi);
                    ManttoService service = new ManttoService();

                    DateTime ldt_fecha = EPDate.ToDate(TextBoxFechaLimite.Text);

                    DateTime ldt_fechaLimite = new DateTime(ldt_fecha.Year, ldt_fecha.Month, ldt_fecha.Day, ddl_hora.SelectedIndex, ddl_min.SelectedIndex, ddl_seg.SelectedIndex);

                    int li_resuldato = service.CreateFechaLim(ii_regcodi, ldt_fechaLimite, in_app.is_UserLogin + ":" + in_app.is_PC_IPs);

                    if (li_resuldato >= 1)
                    {
                        LabelMensaje.Text = "Se insertó fecha límite";
                        Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx", true);
                    }
                    else
                    {
                        LabelMensaje.Text = "Error al momento de insertar fecha límite";
                    }

                }
                else
                {
                    Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx", true);
                }

            }
            else
	        {
                LabelMensaje.Text = "Ingresé una fecha válida para la fecha límite";
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