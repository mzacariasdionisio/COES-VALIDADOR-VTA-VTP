using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using WScoes;
using fwapp;

namespace WSIC2010.Demanda
{
    public partial class w_me_listarDemanda : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        IDemandaService demandaServ;
        int pi_codigoTipoEmpresa = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                //Session["ReturnPage"] = "~/WebForm/mantto/w_eve_mantto.aspx";
                Session["ReturnPage"] = "~/WebForm/Demanda/w_me_demandaBarras_list.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                //Response.Redirect("~/WebForm/Demanda/w_me_demandaBarras_list.aspx");
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {

                    if (!IsPostBack)
                    {
                        //CheckBoxEmpresas.Text += " " + in_app.Ls_emprcodi[0];
                        ////ManttoServiceClient service = new ManttoServiceClient();                    
                        //ManttoService service = new ManttoService();
                        //Dictionary<int, string> H_MRegistros;
                        //if (in_app.is_PC_IPs.Length >= 3 && (in_app.is_PC_IPs.Substring(0, 3) == "192" || in_app.is_PC_IPs.Substring(0, 3) == "127" || in_app.is_PC_IPs.Substring(0, 3) == "::1"))
                        //    H_MRegistros = service.H_GetManttoRegistros(DateTime.Today.AddDays(-180), DateTime.Today.AddMonths(3));
                        //else
                        //    H_MRegistros = service.H_GetManttoRegistros(DateTime.Today.AddDays(-7), DateTime.Today.AddMonths(3));
                        //ListBoxRegistros.DataSource = H_MRegistros;
                        //ListBoxRegistros.DataTextField = "Value";
                        //ListBoxRegistros.DataValueField = "Key";
                        //ListBoxRegistros.DataBind();

                        this.Page.UICulture = "es";
                        this.Page.Culture = "es-MX";

                        //RangeValidator1.MinimumValue = new DateTime(1600, 01, 01).ToString("dd/MM/yyyy");
                        //RangeValidator1.MaximumValue = DateTime.Today.AddDays(7).ToString("dd/MM/yyyy");

                        string[] array_Empresas = in_app.Ls_emprcodi.ToArray();
                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                        string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                        //DataTable dtEmpresas = new DataTable();
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
                            else if(pi_codigoTipoEmpresa == 4)
                            {
                                lbl_equipo.Text = "Cliente Libre:";
                            }
                        }

                        //DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
                        DataTable dtBarras = demandaServ.nf_get_puntos_medicion_x_empresa(Convert.ToInt32(DDLEmpresa.SelectedValue), pi_codigoTipoEmpresa);

                        if (nf_get_data(dtBarras))
                        {
                            DDLBarras.DataSource = dtBarras;
                            DDLBarras.DataValueField = dtBarras.Columns[0].ToString();
                            DDLBarras.DataTextField = dtBarras.Columns[1].ToString();
                            DDLBarras.DataBind();

                            tBoxFecha.Attributes["value"] = DateTime.Now.ToString("dd/MM/yyyy");

                            DDLNumSemana.DataSource = Util.CargaDDLSemanas.LlenaSemanas(53);
                            DDLNumSemana.DataValueField = "Key";
                            DDLNumSemana.DataTextField = "Value";
                            DDLNumSemana.DataBind();
                        }
                        else 
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('La empresa indicada no presenta barras');", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }
        }

        protected void btnListar_Click(object sender, EventArgs e)
        {
            ClearGridView(GridView1, GridView2);
            demandaServ = new DemandaService();
            try
            {
                Label1.Text = "";
                bool valido = true;
                COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
                GridView1.Columns.Clear();
                GridView2.Columns.Clear();
                Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                DataTable dt_fechaEnvios = new DataTable("fecha_envio");
                DateTime ldt_fechaEnvio = new DateTime(2000, 1, 1);

                DateTime ldt_fecha = EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]));

                DataTable ln_data = demandaServ.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
                if (nf_get_data(ln_data))
                {
                    pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());
                }

                if (ldt_fecha.CompareTo(DateTime.Now) >= -1)
                {

                    if (DDLTipoInfo.SelectedValue == "1")
                    {
                        dt_fechaEnvios = wsDemanda.ListarEnviosxEmpresas(ldt_fecha, ldt_fecha, Convert.ToInt32(DDLEmpresa.SelectedItem.Value), -1, ls_credencial);
                        DataTable dt_Demandas = demandaServ.nf_DemandaBarraDiario48FHora(ldt_fecha, 45, 46, 20, Convert.ToInt32(DDLBarras.SelectedValue));
                        //Util.Alert.Show(ldt_fecha.ToString() + "-" + DateTime.Now.ToString());

                        if (!nf_get_data(dt_Demandas))
                        {
                            valido = false;
                            Label1.Text = "<p style='color:#00F;margin-left:15px;'>No hay datos para este Programa Diario en la fecha seleccionada</p>";
                            
                        }

                        if (valido && !String.IsNullOrEmpty(ObtienefechaEnvio(dt_fechaEnvios, 2)))
                        {
                            HeaderGridView(GridView1, dt_Demandas);
                            //DataTable dt_Desc = wsDemanda.PuntoMedicionBarraDesc(Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                            DataTable dt_Desc = demandaServ.nf_get_PuntoMedicion(Convert.ToInt32(DDLBarras.SelectedValue), Convert.ToInt32(DDLEmpresa.SelectedValue), pi_codigoTipoEmpresa);
                            //DataTable dt_Fuente = wsDemanda.ObtenerDemandaFuente(2, ldt_fecha, Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                            //ldt_fechaEnvio = EPDate.ToDate(ObtienefechaEnvio(dt_fechaEnvios, 2));
                            ldt_fechaEnvio = EPDate.ToDateMMDDYYYY(ObtienefechaEnvio(dt_fechaEnvios, 2));
                            //AgregaFilaDataTable(dt_Desc, ldt_fechaEnvio);
                            GridView2.DataSource = dt_Desc;
                            GridView2.DataBind();
                            GridView1.DataSource = dt_Demandas;
                            GridView1.DataBind();
                        }
                    }
                    else if (DDLTipoInfo.SelectedValue == "2")
                    {
                        //if (ldt_fecha.DayOfWeek.Equals(DayOfWeek.Saturday))
                        //{
                            //DataTable dt_Pronosticos = wsDemanda.ObtenerDemandaBarraSemanal96Fhora(EPDate.f_fechainiciosemana(ldt_fecha), 47, 20, Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                            dt_fechaEnvios = wsDemanda.ListarEnviosxEmpresas(ldt_fecha.AddDays(-7), ldt_fecha, Convert.ToInt32(DDLEmpresa.SelectedItem.Value), -1, ls_credencial);
                            //DataTable dt_Pronosticos = wsDemanda.ObtenerDemandaBarraSemanal96Fhora(ldt_fecha, 47, 20, Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                            DataTable dt_Pronosticos = demandaServ.nf_DemandaBarraSemanal48FHora(ldt_fecha, 47, 20, Convert.ToInt32(DDLBarras.SelectedValue));
                            //Util.Alert.Show(ldt_fecha.ToString() + "-" + DateTime.Now.ToString());


                            if (!nf_get_data(dt_Pronosticos))
                            {
                                valido = false;
                                Label1.Text = "<p style='color:#00F;margin-left:15px;'>No hay datos para el Programa Semanal en la fecha seleccionada</p>";
                                
                            }

                            if (valido && !String.IsNullOrEmpty(ObtienefechaEnvio(dt_fechaEnvios, 3)))
                            {
                                HeaderGridView(GridView1, dt_Pronosticos);
                                //DataTable dt_Desc = wsDemanda.PuntoMedicionBarraDesc(Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                                DataTable dt_Desc = demandaServ.nf_get_PuntoMedicion(Convert.ToInt32(DDLBarras.SelectedValue), Convert.ToInt32(DDLEmpresa.SelectedValue), pi_codigoTipoEmpresa);
                                //DataTable dt_Fuente = wsDemanda.ObtenerDemandaFuente(3, ldt_fecha, Convert.ToInt32(DDLBarras.SelectedValue), ls_credencial);
                                //ldt_fechaEnvio = EPDate.ToDate(ObtienefechaEnvio(dt_fechaEnvios, 3));
                                ldt_fechaEnvio = EPDate.ToDateMMDDYYYY(ObtienefechaEnvio(dt_fechaEnvios, 3));
                                //AgregaFilaDataTable(dt_Desc, ldt_fechaEnvio);
                                GridView2.DataSource = dt_Desc;
                                GridView2.DataBind();
                                GridView1.DataSource = dt_Pronosticos;
                                GridView1.DataBind();
                            }
                        //}
                        //else
                        //{
                        //    Label1.Text = "<p style='color:#00F;margin-left:15px;'>Error en la fecha ingresada, Seleccione un s&aacute;bado como fecha de consulta para el Programa Semanal</p>";
                        //}
                   
                    }
                }
                else
                {
                    Label1.Text = "<p style='color:#00F;margin-left:15px;'>Error en la fecha ingresada, Seleccione otra fecha valida</p>";
                }
            }
            catch (Exception exc)
            {
                GridView1.Columns.Clear();
                Label1.Text = "<p style='color:#00F;margin-left:20px'>ERROR: <ul style='color:#00F;margin-left:20px'><li>" + exc.Message + "</li></ul></p>";
            }

        }

        private string ObtienefechaEnvio(DataTable pdt_fechaEnvios, int pi_tipoinf)
        {
            string ls_fechaEnvio = String.Empty;
            if (pdt_fechaEnvios.Rows.Count > 0)
            {
                foreach (DataRow drow in pdt_fechaEnvios.Rows)
                {
                    if (drow[8].ToString().Equals(pi_tipoinf.ToString()))
                    {
                        ls_fechaEnvio =  drow[7].ToString();
                    }
                }

                return ls_fechaEnvio;
            }
            else
            {
                return null;
            }
            
        }

        //private void AgregaFilaDataTable(DataTable dt1, DataTable dt2, DateTime adt_fechaEnvio)
        //{
        //    DataRow dr = dt1.NewRow();
        //    dr[0] = "FUENTE";
        //    foreach (DataRow drow in dt2.Rows)
        //    {
        //        dr[1] = drow[0].ToString();
        //    }

        //    dt1.Rows.Add(dr);

        //    DataRow dr1 = dt1.NewRow();

        //    dr1[0] = "FECHA ENVIO";
        //    dr1[1] = adt_fechaEnvio.ToString("dd/MM/yyyy H:mm:ss");

        //    dt1.Rows.Add(dr1);
        //}

        private void AgregaFilaDataTable(DataTable dt1, DateTime adt_fechaEnvio)
        //private void AgregaFilaDataTable(DataTable dt1, string sdt_fechaEnvio)
        {
            DataRow dr1 = dt1.NewRow();

            dr1[0] = "FECHA ENVIO";
            dr1[1] = adt_fechaEnvio.ToString("dd/MM/yyyy H:mm:ss");

            dt1.Rows.Add(dr1);
        }

        private void ClearGridView(GridView gView1, GridView gView2)
        {
            gView1.DataSource = null;
            gView1.DataBind();
            gView2.DataSource = null;
            gView2.DataBind();
        }

        private void HeaderGridView(GridView gView, DataTable dt)
        {
            int i = 0;
            string[] headersPDO = new string[] { "HISTÓRICO (MW)", "PREVISTO (MW)" };
            //Dictionary<int, string> ld_headersPDO = new Dictionary<int, string>();
            //ld_headersPDO.Add(45, "HISTÓRICO (MW)");
            //ld_headersPDO.Add(46, "PREVISTO (MW)");
            string[] headersPSO = new string[] { "PREVISTO (MW)" };
            foreach (DataColumn col in dt.Columns)
            {
                //Declare the bound field and allocate memory for the bound field.
                BoundField bfield = new BoundField();

                if (col.Ordinal == 0)
                {
                    //Initalize the DataField value.
                    bfield.DataField = col.ColumnName;
                    //Initialize the HeaderText field value.
                    bfield.HeaderText = col.ColumnName;
                }
                else if ((dt.Columns.Count <= 3) && (col.Ordinal > 0))
                {
                    bfield.DataField = col.ColumnName;
                    //bfield.HeaderText = headersPDO[i-1];
                    //bfield.HeaderText = headersPDO[i - 1] + " : " + col.ColumnName;
                    bfield.HeaderText = (col.ColumnName.StartsWith("45") ? headersPDO[0] : headersPDO[1]) + " : " + col.ColumnName;
                }
                else if ((dt.Columns.Count == 8) && (col.Ordinal > 0))
                {
                    bfield.DataField = col.ColumnName;
                    //bfield.HeaderText = headersPDO[1];
                    bfield.HeaderText = headersPSO[0] + " : " + col.ColumnName;
                }
                //Add the newly created bound field to the GridView.
                gView.Columns.Add(bfield);
                i++;
            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (DDLTipoInfo.SelectedValue == "1")
                {
                    //if (e.Row.RowIndex == 0)
                    //{
                    //    e.Row.Font.Bold = true;
                    //    e.Row.ForeColor = Color.FromName("#ffffff");
                    //    e.Row.BackColor = Color.FromName("#4a70aa");
                    //    e.Row.Cells[0].Text = "HORA";
                    //    /*e.Row.Cells[1].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[2].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["02-EJECUTADO "]).ToString("ddd, MM/dd/yyyy");*/
                    //    e.Row.Cells[1].Text = Convert.ToString(((DataRowView)e.Row.DataItem)[1]);
                    //    e.Row.Cells[2].Text = Convert.ToString(((DataRowView)e.Row.DataItem)[2]);
                    //}
                    //else if (e.Row.RowIndex != -1)
                    if (e.Row.RowIndex != -1)
                    {
                        /*e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["02-EJECUTADO "]).ToString("0.00");*/
                        if (e.Row.Cells.Count == 2)
                            e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[1]).ToString("0.00");

                        if (e.Row.Cells.Count == 3)
                        {
                            e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[1]).ToString("0.00");
                            e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[2]).ToString("0.00");
                        }
                    }
                }
                else if (DDLTipoInfo.SelectedValue == "2")
                {
                    //if (e.Row.RowIndex == 0)
                    //{
                    //    e.Row.Font.Bold = true;
                    //    e.Row.ForeColor = Color.FromName("#ffffff");
                    //    e.Row.BackColor = Color.FromName("#4a70aa");
                    //    e.Row.Cells[0].Text = "HORA";
                    //    e.Row.Cells[1].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[2].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["02-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[3].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["03-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[4].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["04-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[5].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["05-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[6].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["06-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //    e.Row.Cells[7].Text = Convert.ToDateTime(((DataRowView)e.Row.DataItem)["07-REPROGRAMACION DIARIA"]).ToString("ddd, MM/dd/yyyy");
                    //}
                    //else if (e.Row.RowIndex != -1)
                    if (e.Row.RowIndex != -1)
                    {
                        /*e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["01-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["02-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[3].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["03-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[4].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["04-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[5].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["05-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[6].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["06-REPROGRAMACION DIARIA"]).ToString("0.00");
                        e.Row.Cells[7].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)["07-REPROGRAMACION DIARIA"]).ToString("0.00");*/
                        e.Row.Cells[1].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[1]).ToString("0.00");
                        e.Row.Cells[2].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[2]).ToString("0.00");
                        e.Row.Cells[3].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[3]).ToString("0.00");
                        e.Row.Cells[4].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[4]).ToString("0.00");
                        e.Row.Cells[5].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[5]).ToString("0.00");
                        e.Row.Cells[6].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[6]).ToString("0.00");
                        e.Row.Cells[7].Text = Convert.ToDouble(((DataRowView)e.Row.DataItem)[7]).ToString("0.00");
                    }
                }
            }
            catch(Exception ex)
            {
            //    GridView1.DataSource = null;
                Label1.Text = ex.Message;
            }
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

            //DataTable dtBarras = wsDemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
            DataTable dtBarras = demandaServ.nf_get_puntos_medicion_x_empresa(Convert.ToInt32(DDLEmpresa.SelectedValue), pi_codigoTipoEmpresa);
            DDLBarras.DataSource = dtBarras;
            DDLBarras.DataValueField = dtBarras.Columns[0].ToString();
            DDLBarras.DataTextField = dtBarras.Columns[1].ToString();
            DDLBarras.DataBind();
        }

        protected void DDLTipoInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int li_tipoPrograma = Int32.Parse(DDLTipoInfo.SelectedValue);
            if (li_tipoPrograma.Equals(1))
            {
                tBoxFecha.Disabled = false;
                tBoxFecha.Attributes["value"] = DateTime.Now.ToString("dd/MM/yyyy");
                //CalendarExtender1.Enabled = true;
                DDLNumSemana.Enabled = false;
                lblNumSem.Enabled = false;
                DDLNumSemana.Visible = false;
                lblNumSem.Visible = false;

            }
            else if (li_tipoPrograma.Equals(2))
            {

                tBoxFecha.Attributes["value"] = String.Empty;
                tBoxFecha.Disabled = true;
                //CalendarExtender1.Enabled = false;
                DDLNumSemana.Enabled = true;
                DDLNumSemana.Visible = true;
                lblNumSem.Enabled = true;
                lblNumSem.Visible = true;
                DDLNumSemana.Text = Convert.ToString(EPDate.XWeekNumber_Entire4DayWeekRule(DateTime.Now.Date)).PadLeft(2, '0');
                tBoxFecha.Attributes["value"] = EPDate.f_fechafinsemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).AddDays(1).ToString("dd/MM/yyyy");
            }
        }

        protected void DDLNumSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            tBoxFecha.Attributes["value"] = EPDate.f_fechafinsemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).AddDays(1).ToString("dd/MM/yyyy");
        }

        protected bool nf_get_data(DataTable adt_data)
        {
            if (adt_data != null && adt_data.Rows.Count > 0)
	        {
                return true;
	        }

            return false;
        }

    }
}