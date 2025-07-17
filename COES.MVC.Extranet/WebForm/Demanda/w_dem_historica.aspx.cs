using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WScoes;
using fwapp;

namespace WSIC2010.Demanda
{
    public partial class w_dem_historica : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        IDemandaService demandaServ;
        int pi_codigoTipoEmpresa = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.UICulture = "es";
            this.Page.Culture = "es-MX";

            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Demanda/w_dem_historica.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {
                        string[] array_Empresas = in_app.Ls_emprcodi.ToArray();
                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                        string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                        DataTable dtEmpresas = wsDemanda.EmpresasRepxUsuario(array_Empresas, ls_credencial);
                        DDLEmpresa.DataSource = dtEmpresas;
                        DDLEmpresa.DataValueField = dtEmpresas.Columns[0].ToString();
                        DDLEmpresa.DataTextField = dtEmpresas.Columns[1].ToString();
                        DDLEmpresa.DataBind();

                        demandaServ = new DemandaService();
                        
                        DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
                        if (nf_get_data(ln_data))
                        {
                            pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());

                            if (pi_codigoTipoEmpresa == 2)
                            {
                                lbl_equipo.Text = "Equipo:";
                            }
                            else if (pi_codigoTipoEmpresa == 4)
                            {
                                lbl_equipo.Text = "Cliente Libre:";
                            }
                        }

                        DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
                        DDLBarras.DataSource = dtBarras;
                        DDLBarras.DataValueField = dtBarras.Columns[0].ToString();
                        DDLBarras.DataTextField = dtBarras.Columns[3].ToString();
                        DDLBarras.DataBind();

                        //Inicializo Fecha Inicio y Fin
                        //tBoxInicio.Attributes["value"] = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //tBoxFin.Attributes["value"] = EPDate.ToDate(Convert.ToString(tBoxInicio.Attributes["value"])).AddDays(7).Date.ToString("dd/MM/yyyy");

                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }

        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            demandaServ = new DemandaService();
            ClearGridView(gViewHistorico);
            DateTime ldt_fecha_inicio;
            DateTime ldt_fecha_fin;
            try
            {
                Label1.Text = "";
                bool lb_valido = true;
                COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
                gViewHistorico.Columns.Clear();
                Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                if (EPDate.IsDate(Convert.ToString(tBoxInicio.Attributes["value"])))
                {
                    ldt_fecha_inicio = EPDate.ToDate(Convert.ToString(tBoxInicio.Attributes["value"]));

                    if (EPDate.IsDate(Convert.ToString(tBoxFin.Attributes["value"])))
                    {

                        ldt_fecha_fin = EPDate.ToDate(Convert.ToString(tBoxFin.Attributes["value"]));

                        if (ldt_fecha_fin.Date.CompareTo(ldt_fecha_inicio.Date) > -1)
                        {
                            if (ldt_fecha_fin.Date.CompareTo(ldt_fecha_inicio.AddMonths(2).Date) <= -1)
                            {
                                //DataTable dt_DemHistorica = wsDemanda.ObtenerDemandaBarraDiario96Fhora2(ldt_fecha_inicio, ldt_fecha_fin, Convert.ToInt32(DDLLectura.SelectedValue), 20, Convert.ToInt32(DDLBarras.SelectedValue), 2, ls_credencial);
                                DataTable dt_DemHistorica = demandaServ.nf_DemandaBarraReporteDiario48FHora(ldt_fecha_inicio, ldt_fecha_fin, Convert.ToInt32(DDLLectura.SelectedItem.Value), 20, Convert.ToInt32(DDLBarras.SelectedValue));
                                //DataTable dt_fechaEnvios = wsDemanda.ListarEnviosxEmpresas(ldt_fecha_inicio, ldt_fecha_fin, Convert.ToInt32(DDListEmpresa.SelectedItem.Value), -1, ls_credencial);
                                //int li_columnas = dt_DemHistorica.Columns.Count;
                                DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
                                if (nf_get_data(ln_data))
                                {
                                    pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());
                                }

                                DataTable dt_Desc = demandaServ.nf_get_PuntoMedicion(Convert.ToInt32(DDLBarras.SelectedValue), Convert.ToInt32(DDLEmpresa.SelectedValue), pi_codigoTipoEmpresa);
                                int li_columnas = 0;

                                if (!nf_get_data(dt_DemHistorica))
                                {
                                    lb_valido = false;
                                    Label1.Text = "<p style='color:#00F;margin-left:15px;'>No hay datos para el Programa Diario Hist&oacute;rico en la fecha seleccionada</p>";
                                }
                                else
                                {
                                    li_columnas = dt_DemHistorica.Columns.Count;
                                }


                                if (lb_valido)
                                {
                                    dt_DemHistorica.Columns[li_columnas - 1].SetOrdinal(1);
                                    GridView2.DataSource = dt_Desc;
                                    GridView2.DataBind();
                                    gViewHistorico.DataSource = dt_DemHistorica;
                                    gViewHistorico.DataBind();
                                    //gViewHistorico.Columns["TOTAL"]
                                }
                            }
                            else
                            {
                                Label1.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: La Fecha Fin debe ser dos meses de la Fecha Inicio</p>";
                            }

                        }
                        else
                        {
                            Label1.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: La Fecha de inicio debe ser menor que la fecha final</p>";
                        }
                    }
                    else
                    {
                        Label1.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Definir Fecha Inicio y/o Fecha Fin</p>";
                    }
                }
                else
                {
                    Label1.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Definir Fecha Inicio y/o Fecha Fin</p>";
                }

            }
            catch (Exception ex)
            {
                gViewHistorico.Columns.Clear();
                Label1.Text = "<p style='color:#00F;margin-left:20px'>ERROR: <ul style='color:#00F;margin-left:20px'><li>" + ex.Message + "</li></ul></p>";
            }
        }

        private void ClearGridView(GridView gView)
        {
            gView.DataSource = null;
            gView.DataBind();
        }

        protected void DDListEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

            demandaServ = new DemandaService();
            int pi_codigoTipoEmpresa = 0;
            DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
            if (nf_get_data(ln_data))
            {
                pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());

                if (pi_codigoTipoEmpresa == 2)
                {
                    lbl_equipo.Text = "Equipo:";
                }
                else if (pi_codigoTipoEmpresa == 4)
                {
                    lbl_equipo.Text = "Cliente Libre:";
                }
            }

            DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
            DDLBarras.DataSource = dtBarras;
            DDLBarras.DataValueField = dtBarras.Columns[0].ToString();
            DDLBarras.DataTextField = dtBarras.Columns[3].ToString();
            DDLBarras.DataBind();
        }

        protected bool nf_get_data(DataTable adt_data)
        {
            if (adt_data != null && adt_data.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        protected void DDLEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

            demandaServ = new DemandaService();
            int pi_codigoTipoEmpresa = 0;
            DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
            if (nf_get_data(ln_data))
            {
                pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());

                if (pi_codigoTipoEmpresa == 2)
                {
                    lbl_equipo.Text = "Equipo:";
                }
                else if (pi_codigoTipoEmpresa == 4)
                {
                    lbl_equipo.Text = "Cliente Libre:";
                }
            }

            DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
            DDLBarras.DataSource = dtBarras;
            //DDLBarras.DataSource = resort(dtBarras, "PTOMEDICODI", "ASC");
            DDLBarras.DataValueField = dtBarras.Columns[0].ToString();
            DDLBarras.DataTextField = dtBarras.Columns[3].ToString();
            DDLBarras.DataBind();
        }

        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            DataTable dtOut = null;
            dt.DefaultView.Sort = colName + " " + direction;
            dtOut = dt.DefaultView.ToTable();
            return dtOut;
        }
    }
}