using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
//using WSIC2010.WScoes;
using WScoes;
using System.Web;
using System.Drawing;
using WSIC2010.Util;


namespace WSIC2010
{
    public partial class w_eve_mantto_list : System.Web.UI.Page
    {
        int ii_regcodi = -1;
        bool ib_empresa = false;
        int ii_evenclasecodi = -1;

        CManttoRegistro ManRegistro;
        n_app in_app;
        //DateTime idt_FechaIni = new DateTime(2000, 1, 1, 0, 0, 0);
        //DateTime idt_FechaFin = new DateTime(2000, 1, 1, 0, 0, 0);
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ReturnPage"] = "~/WebForm/mantto/w_eve_mantto.aspx";
            if (Session["in_app"] == null)
            {
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Mantenimientos) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {

                    //if (Request.QueryString["qs_codi"] != null)
                    //{
                    //    if (!String.IsNullOrEmpty(Request.QueryString["qs_codi"].ToString()))
                    //    {
                    //        Session["i_regcodi"] = Convert.ToInt32(Request.QueryString["qs_codi"].ToString());
                    //    }
                    //}

                    //if (Request.QueryString["qs_empresa"] != null)
                    //{
                    //    if (!String.IsNullOrEmpty(Request.QueryString["qs_empresa"].ToString()))
                    //    {
                    //        Session["b_empresa"] = Convert.ToBoolean(Request.QueryString["qs_empresa"].ToString());
                    //    }
                    //}

                    //if (Request.QueryString["qs_fechalimite"] != null)
                    //{
                    //    if (!String.IsNullOrEmpty(Request.QueryString["qs_fechalimite"].ToString()))
                    //    {
                    //        Session["d_fechaLimite"] = Convert.ToString(Request.QueryString["qs_fechalimite"].ToString());
                    //    }
                    //}


                    if (Session["i_regcodi"] != null)
                    //if (!String.IsNullOrEmpty(Request.QueryString["id"].ToString()) && !String.IsNullOrEmpty(Request.QueryString["empresa"].ToString()))
                    {
                        ii_regcodi = (int)Session["i_regcodi"];
                        ib_empresa = (bool)Session["b_empresa"];
                        //ii_regcodi = Convert.ToInt32(Request.QueryString["id"].ToString());
                        //ib_empresa = Convert.ToBoolean(Request.QueryString["empresa"].ToString());
                        if (Session["i_ddlEmpresa"] == null)
                            Session["i_ddlEmpresa"] = in_app.Ls_emprcodi[0].Trim();

                        // ManttoServiceClient manservice = new ManttoServiceClient();
                        ManttoService manservice = new ManttoService();
                        //if (ib_empresa)
                        //    ManRegistro = manservice.GetManttoRegistro(in_app.is_Empresas, ii_regcodi);
                        //else
                        ManRegistro = manservice.GetManttoRegistro(ii_regcodi);

                        //Asigna tipo de programa

                        ii_evenclasecodi = ManRegistro.EvenClaseCodi;
                        Session["ii_evenclasecodi"] = ii_evenclasecodi;

                        LabelTituloMantto.Text = ManRegistro.RegistroNombre + " -> Empresa: " + in_app.is_EmpresasNombres();//.is_Empresas;
                        // "Mantenimiento " + ManRegistro.EvenClaseDesc;
                        //idt_FechaIni = (DateTime)Session["dt_FechaIni"];
                        //idt_FechaFin = (DateTime)Session["dt_FechaFin"];

                        if (Session["d_fechaLimite"] != null && !String.IsNullOrEmpty(Session["d_fechaLimite"].ToString()))
                            hdfecha.Value = EPDate.ToDate(Session["d_fechaLimite"].ToString()).ToString("MMM d yyyy HH:mm:ss");

                    }
                    else
                    {
                        ii_regcodi = 22;
                        Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx");
                    }

                    if (!IsPostBack)
                    {
                        //DataSet i_ds = new DataSet("dslogin");
                        //n_app in_app = (n_app)Session["in_app"];


                        //string ls_comando = "SELECT * from MAN_MANTTO WHERE evenclasecodi=" + 2;
                        //in_app.Fill(0, i_ds, "MAN_MANTTO", ls_comando);               

                        List<DateTime> L_Items = new List<DateTime>();
                        L_Items.Add(new DateTime(2000, 1, 1, 0, 0, 0));

                        RadioButtonListDias.Items.Add("Todos");

                        for (DateTime dtemp = ManRegistro.FechaInicial; dtemp <= ManRegistro.FechaFinal; dtemp = dtemp.AddDays(1))
                        {
                            RadioButtonListDias.Items.Add(dtemp.ToString("dd MMM"));
                            L_Items.Add(dtemp);
                        }
                        if (RadioButtonListDias.Items.Count > 2)
                            RadioButtonListDias.SelectedIndex = 1;
                        else
                            RadioButtonListDias.SelectedIndex = 0;//si es solo un dia 
                        Session["L_Items"] = L_Items;
                        CargarListaMantto();
                        Visibility_for_Controls();
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }

        }

        private string nf_get_calculaFecha(int ps_tipoPrograma, DateTime pdt_fechaInicial)
        {
            switch (ps_tipoPrograma)
            {
                case 1:
                    return DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                case 2:
                    return pdt_fechaInicial.ToString("dd/MM/yyyy") + " 09:00:00";
                case 3:
                    return EPDate.f_fechainiciosemana(pdt_fechaInicial).AddDays(-5).ToString("dd/MM/yyyy") + " 14:00:00";
                default:
                    return String.Empty;
            }
        }

        protected void ListViewManttoList_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataSet i_ds = new DataSet("dslogin");
                //n_app in_app = (n_app)Session["in_app"];


                //string ls_comando = "SELECT * from MAN_MANTTO WHERE evenclasecodi=" + 2;
                //in_app.Fill(0, i_ds, "MAN_MANTTO", ls_comando);               

                CargarListaMantto();

            }
        }

        public void CargarListaMantto()
        {
            //ManttoServiceClient mservice = new ManttoServiceClient();


            ManttoService mservice = new ManttoService();
            DataTable ln_table;
            if (ib_empresa)
            {
                ln_table = mservice.GetMantto(in_app.is_Empresas, ii_regcodi);
            }
            else
                ln_table = mservice.GetMantto("0", ii_regcodi);

            //DateTime dt_min= DateTime.Today.AddYears(2);
            //DateTime dt_max = DateTime.Today.AddYears(-2); 
            foreach (DataRow dr in ln_table.Rows)
            {
                dr["LASTUSER"] = EPString.EP_GetFirstString(dr["LASTUSER"].ToString());
            }

            //if (dt_min > ((DateTime)dr["EVENINI"])) dt_min = ((DateTime)dr["EVENINI"]);
            //if (dt_max < ((DateTime)dr["EVENINI"])) dt_max = ((DateTime)dr["EVENINI"]);
            //}

            //List<DateTime> L_Items = new List<DateTime>();
            //L_Items.Add(new DateTime(2000,1,1,0,0,0));
            //RadioButtonListDias.Items.Add("Todos");
            //for (DateTime dtemp = dt_min.Date; dtemp <= dt_max.Date; dtemp = dtemp.AddDays(1))
            //{            
            //    RadioButtonListDias.Items.Add(dtemp.ToString("dd MMM"));
            //    L_Items.Add(dtemp);
            //}

            List<DateTime> L_Items = new List<DateTime>();
            if (Session["L_Items"] != null && ((List<DateTime>)Session["L_Items"]).Count > 2 && RadioButtonListDias.SelectedIndex > 0)
            {
                L_Items = (List<DateTime>)Session["L_Items"];
                DataView dv_table = new DataView(ln_table, string.Format("[EVENINI] >= #{0:yyyy-MM-dd}# AND [EVENINI] < #{1:yyyy-MM-dd}#", L_Items[RadioButtonListDias.SelectedIndex], L_Items[RadioButtonListDias.SelectedIndex].AddDays(1)), "", DataViewRowState.CurrentRows);
                ListViewManttoList.DataSource = dv_table;
                LabelMensaje.Text += " (Cargado " + dv_table.Count + " registros)";
            }
            else
            {
                ListViewManttoList.DataSource = ln_table; // llena tabla de mantenimientos
                LabelMensaje.Text += " (Cargado " + ln_table.Rows.Count + " registros)";
            }

            ListViewManttoList.DataBind();
        }

        public void CargarMantto(int ai_mancodi)
        {
            ManttoService mservice = new ManttoService();
            DataTable ln_table = mservice.GetMantto(ai_mancodi);
            ListViewManttoList.DataSource = ln_table;
            ListViewManttoList.DataBind();
        }

        void Visibility_for_Controls()
        {
            if (ListViewManttoList.EditIndex != -1 || ListViewManttoList.InsertItemPosition == InsertItemPosition.FirstItem)
            {

                if (ListViewManttoList.FindControl("NewButton") != null)
                    ((Button)ListViewManttoList.FindControl("NewButton")).Visible = false;
                if (ListViewManttoList.FindControl("ImportButton") != null)
                    ((Button)ListViewManttoList.FindControl("ImportButton")).Visible = false;

                if (ListViewManttoList.FindControl("GenerateReport") != null)
                    ((Button)ListViewManttoList.FindControl("GenerateReport")).Visible = false;

                RadioButtonListDias.Visible = false;
            }
            else
            {
                if (ListViewManttoList.FindControl("NewButton") != null)
                    ((Button)ListViewManttoList.FindControl("NewButton")).Visible = true;
                if (ListViewManttoList.FindControl("ImportButton") != null)
                    ((Button)ListViewManttoList.FindControl("ImportButton")).Visible = true;

                if (ListViewManttoList.FindControl("GenerateReport") != null)
                    ((Button)ListViewManttoList.FindControl("GenerateReport")).Visible = true;

                if (RadioButtonListDias.Items.Count > 2)
                    RadioButtonListDias.Visible = true;
                else
                    RadioButtonListDias.Visible = false;
            }
        }

        protected void ListViewManttoList_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            bool l_editmode = true;
            if (Session["b_editmode"] != null)
                if (!(bool)Session["b_editmode"])
                    l_editmode = false;


            LabelMensaje.Text = "_ItemEditing ";
            //((Button)ListViewManttoList.FindControl("NewButton")).Visible = false;           
            //Label labelmancodi = (ListViewManttoList.Items[e.NewEditIndex].FindControl("mancodiLabel")) as Label; 
            //int i_mancodi = Convert.ToInt32(labelmancodi.Text);
            ListViewManttoList.EditIndex = e.NewEditIndex;
            //CargarMantto(i_mancodi);
            CargarListaMantto();


            if (ListViewManttoList.EditItem != null)
            {

                Dictionary<string, string> H_Disponibilidad = new Dictionary<string, string>();
                H_Disponibilidad.Add("-1", "No Definido");/*Nuevo*/
                H_Disponibilidad.Add("E", "En Servicio");
                H_Disponibilidad.Add("F", "Fuera de Servicio");
                DropDownList ddlist_temp = (DropDownList)ListViewManttoList.EditItem.FindControl("evenindispoDropDownList");
                //DropDownList ddlist_temp = (DropDownList)ListViewManttoList.Items[0].FindControl("evenindispoDropDownList");               
                ddlist_temp.DataSource = H_Disponibilidad;
                ddlist_temp.DataTextField = "Value";
                ddlist_temp.DataValueField = "Key";
                Label txt = (ListViewManttoList.EditItem.FindControl("Labelevenindispo")) as Label;
                //Label txt = (ListViewManttoList.Items[0].FindControl("Labelevenindispo")) as Label;
                ddlist_temp.SelectedValue = txt.Text;
                ddlist_temp.DataBind();
                DropDownList ddlist_interrup = (DropDownList)ListViewManttoList.EditItem.FindControl("eveninterrupDropDownList");
                //DropDownList ddlist_interrup = (DropDownList)ListViewManttoList.Items[0].FindControl("eveninterrupDropDownList");
                ddlist_interrup.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                ddlist_interrup.Items.Add(new ListItem("SI", "S"));
                ddlist_interrup.Items.Add(new ListItem("NO", "N"));
                txt = (ListViewManttoList.EditItem.FindControl("Labeleveninterrup")) as Label;
                //txt = (ListViewManttoList.Items[0].FindControl("Labeleveninterrup")) as Label;                
                ddlist_interrup.DataBind();
                try
                {
                    ddlist_interrup.SelectedValue = txt.Text;
                }
                catch
                { }
                DropDownList ddlist_tipoevencodi = (DropDownList)ListViewManttoList.EditItem.FindControl("TipoEvenCodiDropDownList");
                //DropDownList ddlist_tipoevencodi = (DropDownList)ListViewManttoList.Items[0].FindControl("TipoEvenCodiDropDownList");
                ddlist_tipoevencodi.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                ddlist_tipoevencodi.Items.Add(new ListItem("PREVENTIVO", "1"));
                ddlist_tipoevencodi.Items.Add(new ListItem("CORRECTIVO", "2"));
                ddlist_tipoevencodi.Items.Add(new ListItem("AMPLIACION/MEJORAS", "3"));
                ddlist_tipoevencodi.Items.Add(new ListItem("EVENTO", "4"));/*Nuevo*/
                ddlist_tipoevencodi.Items.Add(new ListItem("PRUEBAS", "6"));
                txt = (ListViewManttoList.EditItem.FindControl("LabelTipoEvenCodi")) as Label;
                //txt = (ListViewManttoList.Items[0].FindControl("LabelTipoEvenCodi")) as Label;
                ddlist_tipoevencodi.DataBind();
                try
                {
                    ddlist_tipoevencodi.SelectedValue = txt.Text;
                }
                catch
                { }

                DropDownList ddlist_EvenTipoProg = (DropDownList)ListViewManttoList.EditItem.FindControl("EvenTipoProgDropDownList");
                //DropDownList ddlist_EvenTipoProg = (DropDownList)ListViewManttoList.Items[0].FindControl("EvenTipoProgDropDownList");
                ddlist_EvenTipoProg.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                ddlist_EvenTipoProg.Items.Add(new ListItem("PROGRAMADO", "P"));
                ddlist_EvenTipoProg.Items.Add(new ListItem("REPROGRAMADO", "R"));
                ddlist_EvenTipoProg.Items.Add(new ListItem("FORZADO/IMPREVISTO", "F"));
                txt = (ListViewManttoList.EditItem.FindControl("LabelEvenTipoProg")) as Label;
                //txt = (ListViewManttoList.Items[0].FindControl("LabelEvenTipoProg")) as Label;
                ddlist_EvenTipoProg.DataBind();
                try
                {
                    ddlist_EvenTipoProg.SelectedValue = txt.Text;
                }
                catch
                { }

                ManttoService mservice = new ManttoService();
                DataTable ln_table;
                Label labelmancodi = (ListViewManttoList.Items[e.NewEditIndex].FindControl("mancodiLabel")) as Label;
                int i_mancodi = Convert.ToInt32(labelmancodi.Text);

                ln_table = mservice.GetArchivosMantto(i_mancodi);

                ListBox listboxAC = (ListBox)ListViewManttoList.EditItem.FindControl("ListBoxArchivosCargados");
                if (listboxAC != null)
                {
                    listboxAC.DataSource = ln_table;

                    Session["listboxAC_table"] = ln_table;
                    //istboxAC.DataMember = "DESCARCHIVO";                   
                    listboxAC.DataTextField = "DESCARCHIVO";
                    listboxAC.DataValueField = "NUMARCHIVO";
                    listboxAC.DataBind();
                }
                if (!l_editmode)
                {
                    ddlist_temp.Enabled = false;
                    ddlist_interrup.Enabled = false;
                    ddlist_tipoevencodi.Enabled = false;
                    ddlist_EvenTipoProg.Enabled = false;
                    ((TextBox)ListViewManttoList.EditItem.FindControl("eveniniTextBox")).Enabled = false;

                    ((System.Web.UI.WebControls.Image)ListViewManttoList.EditItem.FindControl("eveniniImage")).Visible = false;
                    ((System.Web.UI.WebControls.Image)ListViewManttoList.EditItem.FindControl("evenfinImage")).Visible = false;


                    ((TextBox)ListViewManttoList.EditItem.FindControl("evenfinTextBox")).Enabled = false;
                    ((TextBox)ListViewManttoList.EditItem.FindControl("evenmwindispTextBox")).Enabled = false;
                    ((TextBox)ListViewManttoList.EditItem.FindControl("evendescripTextBox")).Enabled = false;
                    ((TextBox)ListViewManttoList.EditItem.FindControl("evenobsrvTextBox")).Enabled = false;
                    //if (ListViewManttoList.FindControl("UpdateButton") != null)
                    ((Button)ListViewManttoList.EditItem.FindControl("UpdateButton")).Visible = false;
                    //if (ListViewManttoList.FindControl("CancelButton") != null)
                    ((Button)ListViewManttoList.EditItem.FindControl("CancelButton")).Visible = false;
                    ((Button)ListViewManttoList.EditItem.FindControl("ButtonBorrarArchivo")).Visible = false;
                    ((Label)ListViewManttoList.EditItem.FindControl("LabelUploadText")).Visible = false;
                    ((Label)ListViewManttoList.EditItem.FindControl("Label1")).Visible = false;
                    ((Label)ListViewManttoList.EditItem.FindControl("Label2")).Visible = false;
                    ((TextBox)ListViewManttoList.EditItem.FindControl("TextBoxDescArchivo")).Visible = false;
                    ((FileUpload)ListViewManttoList.EditItem.FindControl("FileUpload1")).Visible = false;
                    ((Button)ListViewManttoList.EditItem.FindControl("UploadButton")).Visible = false;

                }
            }
            Visibility_for_Controls();
        }

        protected void ListViewManttoList_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            LabelMensaje.Text = "_ItemUpdating ";

            //System.Data.DataRowView rowView = ListViewManttoList.Items[e.ItemIndex].DataItem as System.Data.DataRowView;
            //string s_emprcodi = rowView["MANCODI"].ToString().Trim();

            int li_mancodi = -1;
            Label lbl = (ListViewManttoList.Items[e.ItemIndex].FindControl("mancodiLabel")) as Label;
            li_mancodi = Convert.ToInt32(lbl.Text);

            string s_evendescrip = "";
            TextBox txt = (ListViewManttoList.Items[e.ItemIndex].FindControl("evendescripTextBox")) as TextBox;
            if (txt != null && !String.IsNullOrEmpty(txt.Text))
            {
                s_evendescrip = txt.Text.Replace("'", "");
            }
            else
            {
                string myScript = @"alert('No se agregó descripción');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_evenobsrv = "";
            txt = (ListViewManttoList.Items[e.ItemIndex].FindControl("evenobsrvTextBox")) as TextBox;
            if (txt != null)
            {
                s_evenobsrv = txt.Text.Replace("'", "");
            }

            DateTime dtini = new DateTime(2020, 1, 1);
            txt = (ListViewManttoList.Items[e.ItemIndex].FindControl("eveniniTextBox")) as TextBox;
            if (txt != null)
            {
                if (EPDate.IsDate(txt.Text))
                {
                    DateTime ldt_fechahora = EPDate.ToDateTime(txt.Text);
                    lbl = ListViewManttoList.Items[e.ItemIndex].FindControl("LabeleveniniWarning") as Label;
                    if (lbl != null)
                    {
                        if (ldt_fechahora < ManRegistro.FechaInicial || ldt_fechahora >= ManRegistro.FechaFinal.AddDays(1))
                        {
                            lbl.Text = "Fecha Hora Inicio fuera de rango";
                            return;
                        }
                        else
                        {
                            lbl.Text = "";
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    lbl = ListViewManttoList.Items[e.ItemIndex].FindControl("LabeleveniniWarning") as Label;
                    lbl.Text = "Formato incorrecto: Fecha Hora Inicio ";
                    return;
                }

                //    if (UpdateQuery.Length > 0)
                //        UpdateQuery += ", ";
                //    UpdateQuery += " EVENINI = TO_DATE('" + txt.Text + "','DD-MM-YYYY HH24:MI') ";

                dtini = EPDate.ToDateTime(txt.Text);
            }
            else
            {
                return;
            }

            DateTime dtfin = new DateTime(2000, 1, 1);
            txt = (ListViewManttoList.Items[e.ItemIndex].FindControl("evenfinTextBox")) as TextBox;
            if (txt != null)
            {
                if (EPDate.IsDate(txt.Text))
                {
                    DateTime ldt_fechahora = EPDate.ToDateTime(txt.Text);
                    lbl = ListViewManttoList.Items[e.ItemIndex].FindControl("LabelevenfinWarning") as Label;

                    if (ldt_fechahora < ManRegistro.FechaInicial || ldt_fechahora > ManRegistro.FechaFinal.AddDays(1))
                    {
                        lbl.Text = "Fecha Hora Final fuera de rango";
                        return;
                    }
                    else
                    {
                        lbl.Text = "";
                    }
                }
                else
                {
                    lbl = ListViewManttoList.Items[e.ItemIndex].FindControl("LabelevenfinWarning") as Label;
                    lbl.Text = "Formato incorrecto: Fecha Hora Final ";
                }

                //if (UpdateQuery.Length > 0)
                //    UpdateQuery += ", ";
                //UpdateQuery += " EVENFIN = TO_DATE('" + txt.Text + "','DD-MM-YYYY  HH24:MI') ";

                dtfin = EPDate.ToDateTime(txt.Text);
            }
            else
            {
                lbl.Text = "ERROR: Fecha Hora Final ";
                return;
            }

            if (dtfin <= dtini)
            {
                string myScript = @"alert('La Fecha Final debe ser mayor que la Fecha Inicial');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_evenindispo = "";
            DropDownList ddlist_temp = (DropDownList)ListViewManttoList.EditItem.FindControl("evenindispoDropDownList");
            if (ddlist_temp != null && ddlist_temp.SelectedItem.Value != "-1")
            {
                s_evenindispo = ddlist_temp.SelectedValue;
            }
            else
            {
                string myScript = @"alert('No se seleccionó INDISPONIBILIDAD');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_tipoevencodi = "";
            DropDownList ddlist_tipoevencodi = (DropDownList)ListViewManttoList.EditItem.FindControl("TipoEvenCodiDropDownList");
            if (ddlist_tipoevencodi != null && ddlist_tipoevencodi.SelectedItem.Value != "-1")
            {
                s_tipoevencodi = ddlist_tipoevencodi.SelectedValue.ToString();
            }
            else
            {
                string myScript = @"alert('No se seleccionó TIPO DE EVENTO');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_eventipoprog = "";
            DropDownList ddlist_EvenTipoProg = (DropDownList)ListViewManttoList.EditItem.FindControl("EvenTipoProgDropDownList");
            if (ddlist_EvenTipoProg != null && ddlist_EvenTipoProg.SelectedItem.Value != "-1")
            {
                s_eventipoprog = ddlist_EvenTipoProg.SelectedValue;
            }
            else
            {
                string myScript = @"alert('No se seleccionó SITUACIÓN');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_eveninterrup = "";
            DropDownList ddlist_interrup = (DropDownList)ListViewManttoList.EditItem.FindControl("eveninterrupDropDownList");
            if (ddlist_interrup != null && ddlist_interrup.SelectedItem.Value != "-1")
            {
                s_eveninterrup = ddlist_interrup.SelectedValue;
            }
            else
            {
                string myScript = @"alert('No se seleccionó INTERRUPCIÓN');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string s_evenmwindisp = "";
            txt = (ListViewManttoList.Items[e.ItemIndex].FindControl("evenmwindispTextBox")) as TextBox;
            if (txt != null)
            {
                //if (txt.Text.Trim().Length > 0 && Char.IsNumber(txt.Text.Trim(), 0))
                double ld_evenmwindisp = 0;
                if (!String.IsNullOrEmpty(txt.Text.Trim()) && double.TryParse(txt.Text.Trim(), out ld_evenmwindisp))
                {
                    s_evenmwindisp = ld_evenmwindisp.ToString();
                }
                else
                {
                    s_evenmwindisp = "0";
                }
            }

            //ManttoServiceClient mservice = new ManttoServiceClient();
            ManttoService mservice = new ManttoService();
            string ls_temp_texto = String.Empty;
            ls_temp_texto = (s_evendescrip.Length >= 296) ? s_evendescrip.Substring(0, 296) + "..." : s_evendescrip;

            //if (mservice.UpdateMantto(li_mancodi, dtini, dtfin, s_evendescrip, s_evenobsrv, s_evenindispo, s_tipoevencodi, s_eventipoprog, s_eveninterrup, s_evenmwindisp, in_app.is_UserLogin + " " + in_app.is_PC_IPs) > 0)
            if (mservice.UpdateMantto(li_mancodi, dtini, dtfin, ls_temp_texto, s_evenobsrv, s_evenindispo, s_tipoevencodi, s_eventipoprog, s_eveninterrup, s_evenmwindisp, in_app.is_UserLogin + " " + in_app.is_PC_IPs) > 0)
            {
                LabelMensaje.Text = "Informacion actualizada";
                ListViewManttoList.EditIndex = -1;

                CargarListaMantto();

                //((Button)ListViewManttoList.FindControl("NewButton")).Visible = true;
                Visibility_for_Controls();
            }
            else
            {
                LabelMensaje.Text = "Error en datos ingresados";
            }
        }

        protected void ListViewManttoList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            int li_mancodi = -1;
            LabelMensaje.Text = "_ItemDeleting ";
            Label lbl = (ListViewManttoList.Items[e.ItemIndex].FindControl("mancodiLabel")) as Label;
            if (lbl != null)
            {
                li_mancodi = Convert.ToInt32(lbl.Text);
                //ManttoServiceClient mservice = new ManttoServiceClient();
                ManttoService service = new ManttoService();
                if (service.DeleteMantto(li_mancodi, in_app.is_UserLogin + " " + in_app.is_PC_IPs) > 0)
                {
                    LabelMensaje.Text = "Registro borrado";
                }
            }
            else
            {
                LabelMensaje.Text = "ELSE _ItemDeleting ";
            }
            ListViewManttoList.EditIndex = -1;
            CargarListaMantto();
        }

        private void CloseInsert()
        {
            ListViewManttoList.InsertItemPosition = InsertItemPosition.None;



            //if (ListViewManttoList.FindControl("NewButton") != null)
            //    ((Button)ListViewManttoList.FindControl("NewButton")).Visible = true;
            //if (ListViewManttoList.FindControl("ImportButton") != null)
            //    ((Button)ListViewManttoList.FindControl("ImportButton")).Visible = true;            
            //if (ListViewManttoList.FindControl("GenerateReport") != null)
            //    ((Button)ListViewManttoList.FindControl("GenerateReport")).Visible = true;

            //RadioButtonListDias.Visible = true;           
            Visibility_for_Controls();


        }

        protected void ListViewManttoList_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            //LabelMensaje.Text = "_ItemInserting ";
            ListViewManttoList.EditIndex = -1;

            SeleccionarEquipo se = e.Item.FindControl("SeleccionarEquipo1") as SeleccionarEquipo;
            int li_equicodi = se.EQUICODI();

            string UpdateQuery1;
            string UpdateQuery2;

            bool lb_errores = false;

            if (li_equicodi > 0)
            {
                UpdateQuery1 = "EQUICODI";
                UpdateQuery2 = li_equicodi.ToString();
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se seleccionó equipo');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            string ls_temp_texto = String.Empty;
            TextBox txt = (e.Item.FindControl("evendescripTextBox")) as TextBox;
            if (txt != null && !String.IsNullOrEmpty(txt.Text))
            {
                ls_temp_texto = (txt.Text.Replace("'", "").Length >= 296) ? txt.Text.Replace("'", "").Substring(0, 296) + "..." : txt.Text.Replace("'", "");

                UpdateQuery1 += ",EVENDESCRIP";
                //UpdateQuery2 += ",'" + txt.Text.Replace("'", "") + "'";
                UpdateQuery2 += ",'" + ls_temp_texto + "'";
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se agregó descripción');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            //txt = (e.Item.FindControl("evenobsrvTextBox")) as TextBox;
            //if (txt != null)
            //{
            //    UpdateQuery1 += ",EVENOBSRV";
            //    UpdateQuery2 += ",'" + txt.Text.Replace("'","") + "'";
            //}

            DateTime dtini = new DateTime(2020, 1, 1);
            txt = e.Item.FindControl("eveniniTextBox") as TextBox;
            if (txt != null)
            {
                if (EPDate.IsDate(txt.Text))
                {
                    DateTime ldt_fechahora = EPDate.ToDateTime(txt.Text);
                    Label lbl = e.Item.FindControl("LabeleveniniWarning") as Label;

                    if (ldt_fechahora < ManRegistro.FechaInicial || ldt_fechahora >= ManRegistro.FechaFinal.AddDays(1))
                    {
                        lb_errores = true;
                        lbl.Text = "Fecha Hora Inicio fuera de rango";
                        return;
                    }
                    else
                    {
                        lbl.Text = "";
                    }
                }
                else
                {
                    lb_errores = true;
                    Label lbl = e.Item.FindControl("LabeleveniniWarning") as Label;
                    lbl.Text = "Formato incorrecto: Fecha Hora Inicio ";
                    return;
                }
                //    UpdateQuery1 += ",EVENINI";
                //    UpdateQuery2 += ",TO_DATE('" + txt.Text + "','DD-MM-YYYY HH24:MI')";
                dtini = EPDate.ToDateTime(txt.Text);
            }



            txt = e.Item.FindControl("evenfinTextBox") as TextBox;
            DateTime dtfin = new DateTime(2000, 1, 1);
            if (txt != null)
            {
                if (EPDate.IsDate(txt.Text))
                {
                    DateTime ldt_fechahora = EPDate.ToDateTime(txt.Text);
                    Label lbl = e.Item.FindControl("LabelevenfinWarning") as Label;

                    if (ldt_fechahora < ManRegistro.FechaInicial || ldt_fechahora > ManRegistro.FechaFinal.AddDays(1))
                    {
                        lb_errores = true;
                        lbl.Text = "Fecha Hora Final fuera de rango";
                        return;
                    }
                    else
                    {
                        lbl.Text = "";
                    }
                }
                else
                {
                    lb_errores = true;
                    Label lbl = e.Item.FindControl("LabelevenfinWarning") as Label;
                    lbl.Text = "Formato incorrecto: Fecha Hora Final ";
                }

                //    UpdateQuery1 += ",EVENFIN";
                //    UpdateQuery2 += ",TO_DATE('" + txt.Text + "','DD-MM-YYYY  HH24:MI')";
                dtfin = EPDate.ToDateTime(txt.Text);
            }

            //Agregamos validación para que la fecha inicial sea menor que la fecha final
            if (dtfin <= dtini)
            {
                lb_errores = true;
                string myScript = @"alert('La Fecha Final debe ser mayor que la Fecha Inicial');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }


            DropDownList ddTipoEvenCodi_temp = (DropDownList)e.Item.FindControl("TipoEvenCodiDropDownList");
            if (ddTipoEvenCodi_temp != null && ddTipoEvenCodi_temp.SelectedItem.Value != "-1")
            {
                UpdateQuery1 += ",TIPOEVENCODI";
                UpdateQuery2 += "," + ddTipoEvenCodi_temp.SelectedValue + " ";
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se seleccionó TIPO DE EVENTO');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }


            DropDownList ddEvenTipoProg_temp = (DropDownList)e.Item.FindControl("EvenTipoProgDropDownList");
            if (ddEvenTipoProg_temp != null && ddEvenTipoProg_temp.SelectedItem.Value != "-1")
            {
                UpdateQuery1 += ",EVENTIPOPROG";
                UpdateQuery2 += ",'" + ddEvenTipoProg_temp.SelectedValue + "'";
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se seleccionó SITUACIÓN');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            DropDownList ddlist_temp = (DropDownList)e.Item.FindControl("evenindispoDropDownList");
            if (ddlist_temp != null && ddlist_temp.SelectedItem.Value != "-1")
            {
                UpdateQuery1 += ",EVENINDISPO";
                UpdateQuery2 += ",'" + ddlist_temp.SelectedValue + "'";
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se seleccionó INDISPONIBILIDAD');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            DropDownList ddlist_interrup = (DropDownList)e.Item.FindControl("eveninterrupDropDownList");
            if (ddlist_interrup != null && ddlist_interrup.SelectedItem.Value != "-1")
            {
                UpdateQuery1 += ",EVENINTERRUP";
                UpdateQuery2 += ",'" + ddlist_interrup.SelectedValue + "' ";
            }
            else
            {
                lb_errores = true;
                string myScript = @"alert('No se seleccionó INTERRUPCIÓN');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", myScript, true);
                return;
            }

            txt = (e.Item.FindControl("evenmwindispTextBox")) as TextBox;
            if (txt != null && txt.Text.Length > 0 && Char.IsNumber(txt.Text.Trim(), 0))
            {
                UpdateQuery1 += ",EVENMWINDISP";
                if (txt.Text.Trim().Length > 0)
                    UpdateQuery2 += "," + txt.Text;
                else
                    UpdateQuery2 += ",0";
            }

            if (!lb_errores)
            {
                UpdateQuery1 += ",CREATED";
                UpdateQuery2 += ",sysdate";
            }

            ManttoService service = new ManttoService();
            int li_temp = service.InsertMantto(ii_regcodi, dtini, dtfin, UpdateQuery1, UpdateQuery2, in_app.is_UserLogin + " " + in_app.is_PC_IPs);

            if (li_temp > 0)
            {
                LabelMensaje.Text = "Informacion actualizada";
                CloseInsert();
                CargarListaMantto();
            }
            else
            {
                LabelMensaje.Text = "Error en actualizacion";
            }
        }

        protected void ListViewManttoList_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            if (e.CancelMode == ListViewCancelMode.CancelingInsert)
            {
                //CloseInsert();
            }
            else
            {
                ListViewManttoList.EditIndex = -1;
            }

            CloseInsert();
            LabelMensaje.Text = "";
            //((Button)ListViewManttoList.FindControl("NewButton")).Visible = true;
            CargarListaMantto();

        }

        protected void ListViewManttoList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "UndoDelete":
                    if (!Util.DiferenciaFecha.HoraInValida(ManRegistro.RegCodi))
                    {
                        int li_mancodi = -1;
                        Label lbl = (e.Item.FindControl("mancodiLabel")) as Label;
                        if (lbl != null)
                        {
                            li_mancodi = Convert.ToInt32(lbl.Text);
                            //ManttoServiceClient mservice = new ManttoServiceClient();
                            ManttoService service = new ManttoService();
                            if (service.UndoDeleteMantto(li_mancodi, in_app.is_UserLogin + " " + in_app.is_PC_IPs) > 0)
                            {
                                LabelMensaje.Text = "Registro previamente borrado ha sido recuperado";
                            }
                        }
                        else
                        {
                            LabelMensaje.Text = "ELSE _UndoDelete ";
                        }
                        ListViewManttoList.EditIndex = -1;
                        CargarListaMantto();
                    }
                    else
                    {
                        Util.Alert.Show("El tiempo para la carga del presente mantenimiento ha finalizado");
                    }
                    break;
                case "Edit":
                    if (e.CommandArgument == "View")
                    {
                        Session["b_editmode"] = false;
                        LabelMensaje.Text = "Ver mantenimiento";
                    }
                    //Label labelmancodi = (e.Item.FindControl("mancodiLabel")) as Label;
                    //int i_mancodi = Convert.ToInt32(labelmancodi.Text);                                
                    //CargarMantto(i_mancodi);
                    //ListViewManttoList.SelectedIndex = 0;

                    break;
                case "Import":
                    if (!Util.DiferenciaFecha.HoraInValida(ManRegistro.RegCodi))
                    {
                        Session["dt_FechaInicial"] = ManRegistro.FechaInicial;
                        Session["dt_FechaFinal"] = ManRegistro.FechaFinal;
                        Response.Redirect("~/WebForm/mantto/w_SeleccionarArchivo.aspx");
                        //break;
                    }
                    else
                    {
                        Util.Alert.Show("El tiempo para la carga del presente mantenimiento ha finalizado");
                    }
                    return;
                case "CopyEveMantto":
                    if (!Util.DiferenciaFecha.HoraInValida(ManRegistro.RegCodi))
                    {

                        ManttoService mservice = new ManttoService();

                        int li_nroregistroprocesados;
                        int li_evenclasecodiOrigen = -1;
                        if (in_app.Ls_emprcodi[0] != null && in_app.Ls_emprcodi[0].Trim() != "0")
                        {
                            switch (ii_evenclasecodi)
                            {
                                case 1:
                                    li_evenclasecodiOrigen = 2;
                                    LabelMensaje.Text += "Importado del programa diario";
                                    break;
                                case 2:
                                    LabelMensaje.Text += "Importado del programa semanal";
                                    li_evenclasecodiOrigen = 3;
                                    break;
                                case 3:
                                    LabelMensaje.Text += "Importado del programa mensual";
                                    li_evenclasecodiOrigen = 4;
                                    break;
                                case 4:
                                    LabelMensaje.Text += "Importado del programa anual";
                                    li_evenclasecodiOrigen = 5;
                                    break;
                                case 5:
                                    LabelMensaje.Text += "No existe nivel superior de programa de mantto.";
                                    break;
                                case 6:
                                    li_evenclasecodiOrigen = 2;
                                    LabelMensaje.Text += "Importado del programa diario";
                                    break;
                            }
                            li_nroregistroprocesados = mservice.CopyMantto(in_app.is_Empresas, ii_regcodi, li_evenclasecodiOrigen, ManRegistro.FechaInicial, ManRegistro.FechaFinal, in_app.is_UserLogin + " " + in_app.is_PC_IPs);
                            if (li_nroregistroprocesados > 0)
                            {
                                LabelMensaje.Text += "CARGADO " + li_nroregistroprocesados + " registros.";
                            }
                            Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
                        }
                    }
                    else
                    {
                        Util.Alert.Show("El tiempo para la carga del presente mantenimiento ha finalizado");
                    }
                    break;
                case "ReportXLS":
                    {

                        string filename = ManRegistro.RegistroNombre.Replace('/', '_').Replace(' ', '_');
                        ManttoService mservice = new ManttoService();

                        DataTable ln_table;
                        if (ib_empresa)
                        {
                            ln_table = mservice.GetManttoPrint(in_app.is_Empresas, ii_regcodi);
                        }
                        else
                            ln_table = mservice.GetManttoPrint("0", ii_regcodi);


                        HttpContext context = HttpContext.Current;
                        context.Response.Clear();
                        //foreach (DataColumn column in ln_table.Columns)
                        //{
                        //    context.Response.Write(column.ColumnName + ",");
                        //}
                        for (int i = 0; i < 8; i++)
                            context.Response.Write(Environment.NewLine);

                        context.Response.Write("\"\",ITEM,EMPRESA,UBICACION,EQUIPO,Cod.,INICIO,FINAL,DESCRIPCION,MW INDISP.,Dispon,Interrupc.,TIPO,PROGR.");
                        context.Response.Write(Environment.NewLine);
                        int j = 0;
                        foreach (DataRow row in ln_table.Rows)
                        {
                            context.Response.Write("\"\"," + ++j + ",");
                            for (int i = 0; i < ln_table.Columns.Count; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        try
                                        {
                                            context.Response.Write(row[i].ToString().Replace("\n", " ").Replace("\r", " ").Replace(",", " ").Replace("-", " ")
                                                                                    .Replace("ó", "o").Replace("Ó", "O")
                                                                                    .Replace("í", "i").Replace("Í", "I")
                                                                                    .Replace("ú", "u").Replace("Ú", "U")
                                                                                    .Replace("á", "a").Replace("Á", "A")
                                                                                    .Replace("é", "e").Replace("É", "E")
                                                                                    .Replace("Ñ", "N").Replace("ñ", "n").TrimEnd() + ",");
                                        }
                                        catch
                                        {
                                            context.Response.Write(",");
                                        }
                                        break;
                                    case 1:
                                        try
                                        {
                                            context.Response.Write(row[i].ToString().Replace("\n", " ").Replace("\r", " ").Replace(",", " ").Replace("-", " ")
                                                                                    .Replace("ó", "o").Replace("Ó", "O")
                                                                                    .Replace("í", "i").Replace("Í", "I")
                                                                                    .Replace("ú", "u").Replace("Ú", "U")
                                                                                    .Replace("á", "a").Replace("Á", "A")
                                                                                    .Replace("é", "e").Replace("É", "E")
                                                                                    .Replace("Ñ", "N").Replace("ñ", "n").TrimEnd() + ",");
                                        }
                                        catch
                                        {
                                            context.Response.Write(",");
                                        }
                                        break;
                                    case 6://Desc
                                        try
                                        {
                                            context.Response.Write(row[i].ToString().Replace("\n", " ").Replace("\r", " ").Replace(",", " ").Replace("-", " ").Replace("°", "o")
                                                                                    .Replace("ó", "o").Replace("Ó", "O")
                                                                                    .Replace("í", "i").Replace("Í", "I")
                                                                                    .Replace("ú", "u").Replace("Ú", "U")
                                                                                    .Replace("á", "a").Replace("Á", "A")
                                                                                    .Replace("é", "e").Replace("É", "E")
                                                                                    .Replace("Ñ", "N").Replace("ñ", "n").TrimEnd().TrimStart().ToUpper() + ",");
                                        }
                                        catch
                                        {
                                            context.Response.Write(",");
                                        }
                                        break;
                                    case 8: //Dispon
                                        try
                                        {
                                            if (row[i].ToString().Trim() == "E")
                                                context.Response.Write("E/S,");
                                            else
                                                context.Response.Write("F/S,");
                                        }
                                        catch
                                        {
                                            context.Response.Write("NODEF,");
                                        }
                                        break;
                                    case 9: //Interrup
                                        try
                                        {
                                            if (row[i].ToString().Trim() == "S")
                                                context.Response.Write("SI,");
                                            else
                                                context.Response.Write("NO,");
                                        }
                                        catch
                                        {
                                            context.Response.Write("NODEF,");
                                        }
                                        break;
                                    case 10: //TIPO
                                        try
                                        {
                                            switch (row[i].ToString().Substring(0, 3))
                                            {
                                                case "PRE":
                                                    context.Response.Write("PREVENTIVO,");
                                                    break;
                                                case "COR":
                                                    context.Response.Write("CORRECTIVO,");
                                                    break;
                                                case "AMP":
                                                    context.Response.Write("AMPLIACION,");
                                                    break;
                                                case "PRU":
                                                    context.Response.Write("PRUEBAS,");
                                                    break;
                                                case "EVE":
                                                    context.Response.Write("EVENTO,");
                                                    break;
                                            }
                                        }
                                        catch
                                        {
                                            context.Response.Write("NO DEFINIDO,");
                                        }
                                        break;
                                    case 11: //PROG
                                        try
                                        {
                                            switch (row[i].ToString().Substring(0, 1))
                                            {
                                                case "P":
                                                    context.Response.Write("PROGRAMADO,");
                                                    break;
                                                case "F":
                                                    context.Response.Write("FORZADO/IMPREVISTO,");
                                                    break;
                                                case "R":
                                                    context.Response.Write("REPROGRAMADO,");
                                                    break;
                                            }
                                        }
                                        catch
                                        {
                                            context.Response.Write("NO DEFINIDO,");
                                        }
                                        break;
                                    default:
                                        context.Response.Write(row[i].ToString().Replace(",", string.Empty).Replace('[', '(').Replace(']', ')') + ",");
                                        break;
                                }

                            }
                            context.Response.Write(Environment.NewLine);
                        }
                        context.Response.ContentType = "text/csv";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
                        context.Response.End();

                        //Cutils.ExportDataTableToCSV(dtable2, "ProduccionSemana_a" + ai_year.ToString() + "s" + ai_week.ToString().Trim().PadLeft(2, '0') +
                        //    "_" + adt_fechainicial.Year.ToString() + adt_fechainicial.Month.ToString().Trim().PadLeft(2, '0') + adt_fechainicial.Day.ToString().Trim().PadLeft(2, '0') +
                        //    "_" + adt_fechafinal.AddDays(-1).Year.ToString() + adt_fechafinal.AddDays(-1).Month.ToString().Trim().PadLeft(2, '0') + adt_fechafinal.AddDays(-1).Day.ToString().Trim().PadLeft(2, '0'));

                        return;
                    }
                case "Insert":
                    {
                        //LabelMensaje.Text += "_ItemCommand[CommandName=Insert] ";                                         
                        break;
                    }
                case "Update":
                    {
                        //LabelMensaje.Text += "_ItemCommand[CommandName=Update] ";

                        break;
                    }
                case "Delete":
                    {
                        //LabelMensaje.Text += "_ItemCommand[CommandName=Delete] ";

                        break;
                    }
                //case "Copy":
                //    {
                //        ListViewManttoList.EditIndex = -1;
                //        ((Button)ListViewManttoList.FindControl("NewButton")).Visible = false;
                //        ((Button)ListViewManttoList.FindControl("NewCopyButton")).Visible = false;                        
                //        ListViewManttoList.InsertItemPosition = InsertItemPosition.FirstItem;
                //        LabelMensaje.Text += "_ItemCommand[CommandName=Copy] ";  
                //        ListViewManttoList.DataBind();
                //        break;                     
                //    }
                case "New":

                    //if (ii_evenclasecodi != 2 || DiferenciaFecha.HoraValida(DateTime.Now, 10))
                    //{
                    if (!Util.DiferenciaFecha.HoraInValida(ManRegistro.RegCodi))
                    {
                        ListViewManttoList.InsertItemPosition = InsertItemPosition.FirstItem;
                        // CargarLista();
                        ListViewManttoList.DataBind();
                        LabelMensaje.Text += "_ItemCommand[CommandName=New]";
                        Dictionary<string, string> H_Disponibilidad = new Dictionary<string, string>();
                        H_Disponibilidad.Add("-1", "No Definido");/*Nuevo*/
                        H_Disponibilidad.Add("E", "En Servicio");
                        H_Disponibilidad.Add("F", "Fuera de Servicio");

                        DropDownList ddlist_temp = (DropDownList)ListViewManttoList.InsertItem.FindControl("evenindispoDropDownList");
                        ddlist_temp.DataSource = H_Disponibilidad;
                        ddlist_temp.DataTextField = "Value";
                        ddlist_temp.DataValueField = "Key";
                        //Label txt = (ListViewManttoList.InsertItem.FindControl("Labelevenindispo")) as Label;
                        //ddlist_temp.SelectedValue = "F";/*Nuevo*/
                        ddlist_temp.DataBind();

                        DropDownList ddlist_interrup = (DropDownList)ListViewManttoList.InsertItem.FindControl("eveninterrupDropDownList");
                        ddlist_interrup.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                        ddlist_interrup.Items.Add(new ListItem("SI", "S"));
                        ddlist_interrup.Items.Add(new ListItem("NO", "N"));
                        //txt = (ListViewManttoList.InsertItem.FindControl("Labeleveninterrup")) as Label;
                        //ddlist_interrup.SelectedValue = "N";/*Nuevo*/
                        ddlist_interrup.DataBind();

                        TextBox txtEvenIni = (ListViewManttoList.InsertItem.FindControl("eveniniTextBox")) as TextBox;
                        if (txtEvenIni != null)
                            txtEvenIni.Text = ManRegistro.FechaInicial.ToString("dd/MM/yyyy HH:mm");
                        TextBox txtEvenFin = (ListViewManttoList.InsertItem.FindControl("evenfinTextBox")) as TextBox;
                        if (txtEvenFin != null)
                            txtEvenFin.Text = ManRegistro.FechaInicial.AddDays(1).ToString("dd/MM/yyyy HH:mm");

                        DropDownList ddlist_tipoevencodi = (DropDownList)ListViewManttoList.InsertItem.FindControl("TipoEvenCodiDropDownList");
                        ddlist_tipoevencodi.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                        ddlist_tipoevencodi.Items.Add(new ListItem("PREVENTIVO", "1"));
                        ddlist_tipoevencodi.Items.Add(new ListItem("CORRECTIVO", "2"));
                        ddlist_tipoevencodi.Items.Add(new ListItem("AMPLIACION/MEJORAS", "3"));
                        ddlist_tipoevencodi.Items.Add(new ListItem("EVENTO", "4"));/*Nuevo*/
                        ddlist_tipoevencodi.Items.Add(new ListItem("PRUEBAS", "6"));
                        //ddlist_tipoevencodi.SelectedValue = "2"; //correctivo/*Nuevo*/
                        //txt = (ListViewManttoList.InsertItem.FindControl("LabelTipoEvenCodi")) as Label;
                        ddlist_tipoevencodi.DataBind();

                        //ddlist_tipoevencodi.SelectedValue = txt.Text;
                        DropDownList ddlist_EvenTipoProg = (DropDownList)ListViewManttoList.InsertItem.FindControl("EvenTipoProgDropDownList");
                        ddlist_EvenTipoProg.Items.Add(new ListItem("NO DEFINIDO", "-1"));/*Nuevo*/
                        ddlist_EvenTipoProg.Items.Add(new ListItem("PROGRAMADO", "P"));
                        ddlist_EvenTipoProg.Items.Add(new ListItem("REPROGRAMADO", "R"));
                        ddlist_EvenTipoProg.Items.Add(new ListItem("FORZADO/IMPREVISTO", "F"));
                        //ddlist_EvenTipoProg.SelectedValue = "F";/*Nuevo*/
                        //txt = (ListViewManttoList.EditItem.FindControl("LabelEvenTipoProg")) as Label;
                        ddlist_EvenTipoProg.DataBind();
                        //try
                        //{
                        //    ddlist_EvenTipoProg.SelectedValue ="P";
                        //}
                        //catch
                        //{ }
                        Visibility_for_Controls();



                        //<asp:Button ID="NewButton" runat="server" CommandName="New" Text="Nuevo"/>   
                        //<asp:Button ID="ImportButton" runat="server"  CommandName="Import" Text="Importar de Formato XLS"/>             
                        //<asp:Button ID="CopyEveManttoButton" runat="server"  CommandName="CopyEveMantto" Text="Copiar de Mantto Aprob."/>             
                        //<asp:Button ID="GenerateReport" runat="server" CommandName="ReportXLS" Text="Reporte XLS"/>

                        //}
                        //else
                        //{
                        //    Alert.Show("el Límite de carga para el programa diario es hasta las 10 horas del día.");
                        //}
                    }
                    else
                    {
                        Util.Alert.Show("El tiempo para la carga del presente mantenimiento ha finalizado");
                    }

                    break;
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            string ls_mancodi = "X";
            Label lbl = (ListViewManttoList.Items[ListViewManttoList.EditItem.DataItemIndex].FindControl("mancodiLabel")) as Label;
            ls_mancodi = lbl.Text;

            TextBox txt = (ListViewManttoList.Items[ListViewManttoList.EditItem.DataItemIndex].FindControl("eveniniTextBox")) as TextBox;
            if (!EPDate.IsDate(txt.Text))
            {
                LabelMensaje.Text = "Fecha Inicio Erronea";
                return;
            }

            DateTime dt = EPDate.ToDateTime(txt.Text);
            if (dt.Year != ManRegistro.FechaInicial.Year)
            {
                LabelMensaje.Text = "Fecha Inicio fuera de rango";
                return;
            }

            string ls_anho = dt.Year.ToString();
            string ls_webpath = ls_anho + @"\" + ManRegistro.EvenClaseAbrev + @"\";
            // Specify the path on the server to
            // save the uploaded file to.
            String savePath = @"D:\data\MANTENIMIENTOS\" + ls_webpath;

            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            //DropDownList ddlist_temp = (DropDownList)ListViewManttoList.EditItem.FindControl("evenindispoDropDownList");
            FileUpload fileupload = (FileUpload)ListViewManttoList.EditItem.FindControl("FileUpload1");
            if (fileupload != null && fileupload.HasFile)
            {
                // Get the name of the file to upload.
                //string fileName = ls_mancodi + "_" + Server.HtmlEncode(fileupload.FileName);
                string fileName = ls_mancodi + "_" + Server.HtmlEncode(fileupload.FileName.ToLower().Replace(' ', '_').Replace('á', 'a')
                                                                                                    .Replace('é', 'e').Replace('í', 'i')
                                                                                                    .Replace('ó', 'o').Replace('ú', 'u'));
                string extension = System.IO.Path.GetExtension(fileName);
                if (extension.ToUpper() == ".MSG")
                {
                    LabelMensaje.Text = "Archivo Formato MSG, no permitido!, se sugiere cargar como archivo .zip";
                    return;
                }

                savePath += fileName;
                fileupload.SaveAs(savePath);

                TextBox txtda = ListViewManttoList.EditItem.FindControl("TextBoxDescArchivo") as TextBox;
                if (txtda == null)
                {
                    txtda.Text = Server.HtmlEncode(fileupload.FileName);
                    //LabelMensaje.Text = "Falta definir descripcion de archivo ";
                    //return;
                }
                if (txtda.Text == "")
                    txtda.Text = Server.HtmlEncode(fileupload.FileName);
                // Notify the user that their file was successfully uploaded.
                LabelMensaje.Text = "Tu archivo ha sido cargado satisfactoriamente.";
                string UpdateQuery1 = "NOMBARCHIVO,DESCARCHIVO";
                string UpdateQuery2 = "'" + ls_webpath.Replace("\\", "/") + fileName + "','" + txtda.Text + "'";

                ListBox listboxAC = (ListBox)ListViewManttoList.EditItem.FindControl("ListBoxArchivosCargados");
                if (listboxAC != null)
                {
                    //int li_counter = listboxAC.Items.Count;
                    ManttoService service = new ManttoService();
                    int li_temp = service.InsertReferenciaArchivoMantto(Convert.ToInt32(ls_mancodi), UpdateQuery1, UpdateQuery2, in_app.is_UserLogin + ":" + in_app.is_PC_IPs);
                    ActualizarListaArchivos_EditItem(listboxAC, service, Convert.ToInt32(ls_mancodi));
                    if (li_temp > 0)
                    {
                        //DataTable ln_table;
                        //ln_table = service.GetArchivosMantto(service, Convert.ToInt32(ls_mancodi));
                        //listboxAC.DataSource = ln_table;
                        //listboxAC.DataValueField = "NUMARCHIVO";
                        //listboxAC.DataTextField = "DESCARCHIVO"; 
                        //listboxAC.DataBind();
                        txtda.Text = "";
                        LabelMensaje.Text = "Informacion cargada!";
                    }
                    else
                    {
                        LabelMensaje.Text = "Error en actualizacion";
                    }
                }
            }
            else
            {
                // Notify the user why their file was not uploaded.
                LabelMensaje.Text = "Tu archivo NO ha podido ser cargado";
            }
        }

        void ActualizarListaArchivos_EditItem(ListBox listboxAC, ManttoService i_service, int ai_mancodi)
        {
            DataTable ln_table;
            ln_table = i_service.GetArchivosMantto(Convert.ToInt32(ai_mancodi));
            Session["listboxAC_table"] = ln_table;
            listboxAC.DataSource = ln_table;
            listboxAC.DataValueField = "NUMARCHIVO";
            listboxAC.DataTextField = "DESCARCHIVO";
            listboxAC.DataBind();
        }

        protected void ButtonBorrarArchivo_Click(object sender, EventArgs e)
        {
            ListBox listboxAC = (ListBox)ListViewManttoList.EditItem.FindControl("ListBoxArchivosCargados");
            if (listboxAC != null)
            {
                if (listboxAC.SelectedIndex >= 0)
                {
                    string ls_mancodi = "X";
                    Label lbl = (ListViewManttoList.Items[ListViewManttoList.EditItem.DataItemIndex].FindControl("mancodiLabel")) as Label;
                    ls_mancodi = lbl.Text;


                    ManttoService service = new ManttoService();
                    int li_temp = service.DeleteReferenciaArchivoMantto(Convert.ToInt32(ls_mancodi), Convert.ToInt32(listboxAC.SelectedValue), in_app.is_UserLogin + ":" + in_app.is_PC_IPs);
                    //listboxAC.SelectedItem.Attributes[""]
                    ActualizarListaArchivos_EditItem(listboxAC, service, Convert.ToInt32(ls_mancodi));
                    LabelMensaje.Text = "Archivo registro borrado! : [" + listboxAC.SelectedValue.ToString() + "]";
                }
                else
                {
                    LabelMensaje.Text = "Debe seleccionar el archivo! ";
                }
            }
        }

        protected void ButtonAbrirArchivo_Click(object sender, EventArgs e)
        {
            ListBox listboxAC = (ListBox)ListViewManttoList.EditItem.FindControl("ListBoxArchivosCargados");
            if (listboxAC != null)
            {
                if (listboxAC.SelectedIndex >= 0)
                {
                    DataTable dt = new DataTable();

                    if (Session["listboxAC_table"] != null)
                    {

                        dt = (DataTable)Session["listboxAC_table"];
                        LabelMensaje.Text = dt.Rows[listboxAC.SelectedIndex]["NOMBARCHIVO"].ToString();
                        string strUrl = System.Configuration.ConfigurationManager.AppSettings["DocumenDir"] + dt.Rows[listboxAC.SelectedIndex]["NOMBARCHIVO"].ToString().Trim();

                        Response.Redirect(strUrl);
                        //Response.Redirect(strUrl + dt.Rows[listboxAC.SelectedIndex]["NOMBARCHIVO"].ToString(), "_blank", "menubar=0,width=600,height=400");
                        //Response.WriteFile(@strUrl);
                        //Response.End(); 
                    }
                }
            }
        }

        protected void ListViewManttoList_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            LabelMensaje.Text = "_ListViewManttoList_ItemCreated";
        }

        protected void ListViewManttoList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                if (ListViewManttoList.EditIndex >= 0 && ListViewManttoList.EditIndex != e.Item.DataItemIndex)
                {
                    e.Item.Visible = false;
                    return;
                }
                // Display the e-mail address in italics.
                //Label EmprCodiLabel = (Label)e.Item.FindControl("emprcodiLabel");
                System.Data.DataRowView rowView = e.Item.DataItem as System.Data.DataRowView;
                string s_tipoevenabrev = rowView["TIPOEVENABREV"].ToString().Trim();
                string s_emprcodi = rowView["EMPRCODI"].ToString().Trim();
                string s_procesado = rowView["EVENPROCESADO"].ToString().Trim();
                string s_eliminado = rowView["DELETED"].ToString().Trim();
                string s_isfiles = rowView["ISFILES"].ToString().Trim();
                DateTime dt_inicial = (DateTime)rowView["EVENINI"];
                DateTime dt_final = (DateTime)rowView["EVENFIN"];

                string s_lastdate = String.Empty;
                string s_created = String.Empty;
                if (rowView["CREATED"] != null && rowView["LASTDATE"] != null)
                {
                    s_created = rowView["CREATED"].ToString();
                    s_lastdate = rowView["LASTDATE"].ToString();
                }





                int i_manttocodi = Convert.ToInt32(rowView["MANTTOCODI"]);
                Label dlLabelItem;
                Label dlLabelTipoEven;

                //dlLabelItem = (Label)e.Item.FindControl("eveniniLabel");
                //dlLabelItem.Font.Bold = true;
                //string s_etiqueta = "";

                List<string> empresas = EPString.EP_GetListStringSeparate(in_app.is_Empresas, ',');
                ImageButton boton;
                if (!empresas.Contains(s_emprcodi) || s_procesado == "1" || s_eliminado == "1")
                {
                    boton = (ImageButton)e.Item.FindControl("DeleteButton");
                    if (boton != null) boton.Visible = false;
                    boton = (ImageButton)e.Item.FindControl("EditButton");
                    if (boton != null) boton.Visible = false;
                    if (s_eliminado == "1")
                    {
                        boton = (ImageButton)e.Item.FindControl("ViewButton");
                        if (boton != null) boton.Visible = false;
                    }
                }
                else
                {
                    boton = (ImageButton)e.Item.FindControl("ViewButton");
                    if (boton != null) boton.Visible = false;
                }

                ImageButton boton3 = (ImageButton)e.Item.FindControl("UndoButton");
                if (boton3 != null)
                {
                    //si esta borrando/cancelado y es de la misma empresa y esta borrado menos de 3 horas puede deshacer! 
                    if (s_eliminado == "1" && empresas.Contains(s_emprcodi) && ((DateTime)rowView["LASTDATE"]).AddHours(3) > DateTime.Now)
                        boton3.Visible = true;
                    else
                        boton3.Visible = false;
                }

                ImageButton boton2 = (ImageButton)e.Item.FindControl("ObsButton");
                if (boton2 != null)
                    if (rowView["EVENOBSRV"].ToString().Trim().Length == 0 || rowView["EVENOBSRV"].ToString().Trim() == rowView["EVENDESCRIP"].ToString().Trim())
                    {
                        boton2.Visible = false;
                    }
                    else
                    {
                        boton2.ToolTip = "Obs: " + rowView["EVENOBSRV"].ToString().Trim();
                    }

                ImageButton boton4 = (ImageButton)e.Item.FindControl("AttachButton");
                if (boton4 != null)
                {
                    if (s_isfiles == "N")
                        boton4.Visible = false;
                }

                if (s_tipoevenabrev.Equals("AMPL."))
                {
                    dlLabelTipoEven = (Label)e.Item.FindControl("TIPOEVENABREVLabel");
                    if (dlLabelTipoEven != null)
                    {
                        dlLabelTipoEven.Font.Bold = true;
                        dlLabelTipoEven.ForeColor = Color.Red;
                    }
                }


                if (s_procesado == "1" || s_eliminado == "1" || i_manttocodi == -1)
                {
                    List<Label> L_ControlesRegistro = new List<Label>();
                    // L_ControlesRegistro.Add((Label)e.Item.FindControl("dlLabelItem"));
                    //L_ControlesRegistro.Add((Label)e.Item.FindControl("TIPOEVENABREVLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("emprnombLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("areanombLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("famabrevLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("equiabrevLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("eveniniLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenfinLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenmwindispLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenindispoLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("eveninterrupLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evendescripLabel"));
                    //L_ControlesRegistro.Add((Label)e.Item.FindControl("lastdateLabel"));
                    //L_ControlesRegistro.Add((Label)e.Item.FindControl("lastuserLabel"));

                    if (i_manttocodi == -1)
                    {
                        foreach (Label label in L_ControlesRegistro)
                        {
                            if (label != null)
                            {
                                label.ForeColor = Color.MediumBlue;//.DarkGreen;// .Sienna;
                            }
                        }
                    }


                    if (s_procesado == "1")
                    {
                        dlLabelItem = (Label)e.Item.FindControl("dlLabelItem");
                        if (dlLabelItem != null)
                        {
                            dlLabelItem.Text = "Proc.";
                            dlLabelItem.ForeColor = Color.RoyalBlue;//.MediumBlue;                        
                        }
                    }
                    if (s_eliminado == "1")
                    {
                        //Label dlLabelItem = (Label)e.Item.FindControl("dlLabelItem");
                        //dlLabelItem.Text = "CANC";


                        foreach (Label label in L_ControlesRegistro)
                        {
                            if (label != null)
                            {
                                label.Font.Strikeout = true;
                                label.ForeColor = Color.Gray;
                            }
                        }
                        if (i_manttocodi != -1)
                        {
                            dlLabelItem = (Label)e.Item.FindControl("dlLabelItem");
                            dlLabelItem.Text = "Canc.";
                            dlLabelItem.ForeColor = Color.DarkRed;
                        }

                    }


                }
                if (ii_evenclasecodi != 5 && dt_inicial.Date.AddDays(1) < dt_final)
                {
                    if (e.Item.FindControl("eveniniLabel") != null)
                        ((Label)e.Item.FindControl("eveniniLabel")).ForeColor = Color.Red;
                    if (e.Item.FindControl("evenfinLabel") != null)
                        ((Label)e.Item.FindControl("evenfinLabel")).ForeColor = Color.Red;
                }

                if (s_lastdate != s_created && !String.IsNullOrEmpty(s_created))
                {
                    List<Label> L_ControlesRegistro = new List<Label>();
                    // L_ControlesRegistro.Add((Label)e.Item.FindControl("dlLabelItem"));
                    //L_ControlesRegistro.Add((Label)e.Item.FindControl("TIPOEVENABREVLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("emprnombLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("areanombLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("famabrevLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("equiabrevLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("eveniniLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenfinLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenmwindispLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evenindispoLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("eveninterrupLabel"));
                    L_ControlesRegistro.Add((Label)e.Item.FindControl("evendescripLabel"));

                    foreach (Label label in L_ControlesRegistro)
                    {
                        if (label != null)
                        {
                            label.ForeColor = Color.Green;//.DarkGreen;// .Sienna;
                        }
                    }
                }

            }
            //Button ddlist_temp = (Button)ListViewManttoList. .FindControl("evenindispoDropDownList");
        }

        protected void RadioButtonListDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelMensaje.Text = "";
            CargarListaMantto();
        }

    }

}