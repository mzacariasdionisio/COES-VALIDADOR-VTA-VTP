using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using WScoes;
using System.Data;
using WSIC2010.Util;

namespace WSIC2010.Mantto
{
    public partial class w_eve_lista_manttos : System.Web.UI.Page
    {
        int ii_regcodi = -1;
        bool ib_empresa = false;
        int ii_evenclasecodi = -1;
        CManttoRegistro ManRegistro;
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ReturnPage"] = "~/WebForm/mantto/w_eve_lista_manttos.aspx";
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
                    if (Session["i_regcodi"] != null)
                    {
                        ii_regcodi = (int)Session["i_regcodi"];
                        ib_empresa = (bool)Session["b_empresa"];
                        if (Session["i_ddlEmpresa"] == null)
                            Session["i_ddlEmpresa"] = in_app.Ls_emprcodi[0].Trim();

                        ManttoService manservice = new ManttoService();
                        ManRegistro = manservice.GetManttoRegistro(ii_regcodi);

                        if (!IsPostBack)
                        {

                            TextBoxFecha.Text = ManRegistro.FechaInicial.ToString("dd/MM/yyyy");

                            /*Seteando los limites al calendario*/
                            hdfecha1.Value = EPDate.ToDate(ManRegistro.FechaInicial.ToString("dd/MM/yyyy")).ToString("MMM d yyyy HH:mm:ss");//Limite Inicial
                            hdfecha2.Value = EPDate.ToDate(ManRegistro.FechaFinal.ToString("dd/MM/yyyy")).ToString("MMM d yyyy HH:mm:ss");//Limite Final

                            /*Seteando limites al contador*/
                            if (Session["d_fechaLimite"] != null && !String.IsNullOrEmpty(Session["d_fechaLimite"].ToString()))
                                hdfecha.Value = EPDate.ToDate(Session["d_fechaLimite"].ToString()).ToString("MMM d yyyy HH:mm:ss");

                            /*Validar estado del mantenimiento para agregar*/
                            if (Util.DiferenciaFecha.HoraInValida(ManRegistro.RegCodi))
                            {
                                btnNuevo.Enabled = false;
                                btnImportarXLS.Enabled = false;
                                btnCopiarManttosAprob.Enabled = false;
                            }
                            //else
                            //{
                            //    Util.Alert.Show("El tiempo para la carga del presente mantenimiento ha finalizado");
                            //}

                            ii_evenclasecodi = ManRegistro.EvenClaseCodi;
                            Session["ii_evenclasecodi"] = ii_evenclasecodi;

                            LabelTituloMantto.Text = ManRegistro.RegistroNombre + " -> Empresa: " + in_app.is_EmpresasNombres();

                            DataTable ln_table;
                            ManttoService mservice = new ManttoService();

                            if (ib_empresa)
                            {
                                ln_table = mservice.GetMantto(in_app.is_Empresas, ii_regcodi, ManRegistro.FechaInicial, ManRegistro.FechaInicial.AddDays(1));
                            }
                            else
                                ln_table = mservice.GetMantto("0", ii_regcodi, ManRegistro.FechaInicial, ManRegistro.FechaInicial.AddDays(1));

                            //gView.PageSize = 50;

                            gView.DataSource = ln_table;
                            gView.DataBind();
                            LabelMensaje.Text = "Cantidad de registros:  " + ln_table.Rows.Count;
                        }

                    }
                    else
                    {
                        ii_regcodi = 22;
                        Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }
            }
        }

        protected void TextBoxFecha_TextChanged(object sender, EventArgs e)
        {
            DataTable ln_table;
            ManttoService mservice = new ManttoService();
            DateTime ldt_fecha = new DateTime(2013, 1, 1);

            this.nf_clear_grid();

            LabelTituloMantto.Text = ManRegistro.RegistroNombre + " -> Empresa: " + in_app.is_EmpresasNombres();

            if (EPDate.IsDate(TextBoxFecha.Text))
            {
                ldt_fecha = EPDate.ToDate(TextBoxFecha.Text);

                if (ib_empresa)
                {
                    ln_table = mservice.GetMantto(in_app.is_Empresas, ii_regcodi, ldt_fecha, ldt_fecha.AddDays(1));
                }
                else
                    ln_table = mservice.GetMantto("0", ii_regcodi, ldt_fecha, ldt_fecha.AddDays(1));

                gView.DataSource = ln_table;
                gView.DataBind();
                LabelMensaje.Text = "Cantidad de registros: " + ln_table.Rows.Count;
            }
            else
            {
                LabelMensaje.Text = "<b>Ingresé fecha válida</b>";
            }
            
        }

        private DataTable filtrar()
        {
            try
            {
                DataTable ln_table = new DataTable("Manttos");
                ManttoService mservice = new ManttoService();
                DateTime ldt_fecha = new DateTime(2013, 1, 1);
                this.nf_clear_grid();
                LabelTituloMantto.Text = ManRegistro.RegistroNombre + " -> Empresa: " + in_app.is_EmpresasNombres();

                if (EPDate.IsDate(TextBoxFecha.Text))
                {
                    ldt_fecha = EPDate.ToDate(TextBoxFecha.Text);

                    if (ib_empresa)
                    {
                        ln_table = mservice.GetMantto(in_app.is_Empresas, ii_regcodi, ldt_fecha, ldt_fecha.AddDays(1));
                    }
                    else
                        ln_table = mservice.GetMantto("0", ii_regcodi, ldt_fecha, ldt_fecha.AddDays(1));
                }

                return ln_table;
            }
            catch (Exception ex)
            {
                LabelMensaje.Text = "ERROR: No se pudo obtener los datos. Mas detalles: " + ex.Message;
                return null;
            }
        }

        protected void gView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gView.PageIndex = e.NewPageIndex;
            gView.DataSource = filtrar();
            gView.DataBind();
        }

        private void nf_clear_grid()
        {
            gView.DataSource = null;
            gView.DataBind();
        }

        protected void gView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Font.Bold = true;
                }

                string ls_tipomantto = String.Empty;
                string ls_manttocodi = String.Empty;
                string ls_deleted = String.Empty;
                string ls_mancodi = String.Empty;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ls_mancodi = DataBinder.Eval(e.Row.DataItem, "mancodi").ToString();
                    e.Row.Attributes.Add("onclick", "javascript:onGridViewRowSelected_eve(" + ls_mancodi + ")");
                    e.Row.Attributes.Add("style", "cursor:hand;cursor:pointer");
                    e.Row.Attributes.Add("onmouseover", "javascript:onMouseOver_eve(this)");
                    e.Row.Attributes.Add("onmouseout", "javascript:onMouseOut_eve(this)");
                    ls_tipomantto = DataBinder.Eval(e.Row.DataItem, "tipoevencodi").ToString();

                    switch (ls_tipomantto)
                    {
                        case "1":
                            e.Row.Cells[0].Text = "PREV.";
                            break;
                        case "2":
                            e.Row.Cells[0].Text = "CORR.";
                            break;
                        case "3":
                            e.Row.Cells[0].Text = "AMPL.";
                            break;
                        case "4":
                            e.Row.Cells[0].Text = "EVEN.";
                            break;
                        case "6":
                            e.Row.Cells[0].Text = "PRUE.";
                            break;
                        default:
                            break;
                    }

                    ls_manttocodi = DataBinder.Eval(e.Row.DataItem, "manttocodi").ToString();
                    /*Manttos nuevos*/
                    if (ls_manttocodi.Equals("-1"))
                    {
                        e.Row.Cells[0].ForeColor = Color.MediumBlue;
                        e.Row.Cells[0].Font.Bold = true;
                    }


                    /*Para manttos cancelados*/
                    ls_deleted = DataBinder.Eval(e.Row.DataItem, "deleted").ToString();

                    if (ls_deleted.Equals("1"))
                    {
                        e.Row.Cells[0].Style.Add("color", "red");
                        e.Row.Cells[0].Style.Add("text-decoration", "line-through");
                        e.Row.Cells[0].Font.Bold = true;
                        //e.Row.Cells[0].Style.Add("background-color", "red");
                        e.Row.Cells[0].ToolTip = "Cancenlado";
                    }

                    //e.Row.Cells[1].Style.Add("text-align", "right");

                }
            }
            catch (Exception ex)
            {
                LabelMensaje.Text = ex.Message;
            }
        }


        public static string GetString(object ao_user)
        {
            string ls_user;
            ls_user = Convert.ToString(ao_user);
            string ls_lastUser;
            ls_lastUser = EPString.EP_GetFirstString(ls_user);
            return ls_lastUser;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_add_mantto.aspx");
        }

        protected void btnImportarXLS_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_SeleccionarArchivo.aspx");
        }


        protected void btnExportarXLS_Click(object sender, EventArgs e)
        {
            int li_cont = 0;
            string ls_fechaInicial, ls_fechaFinal, ls_tipo, ls_programa, ls_disponib, ls_interrup;
            ls_fechaInicial = ls_fechaFinal = ls_tipo = ls_programa = ls_disponib = ls_interrup = String.Empty;
            DateTime ldt_fecha = new DateTime(2013, 1, 1);
            double ld_valor = 0;

            DataTable dt_data = new DataTable("Datos");
            DataTable dt_dataToExport = new DataTable("DatosExportar");
            dt_dataToExport.Columns.Add(new DataColumn("ITEM", typeof(System.Int32)));
            dt_dataToExport.Columns.Add(new DataColumn("TIPO MANTTO", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("EMPRESA", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("UBICACIÓN", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("FAMILIA", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("EQUIPO", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("INICIO", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("FINAL", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("MW INDISP", typeof(System.Double)));
            dt_dataToExport.Columns.Add(new DataColumn("DISPON", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("INTERRUP", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("PROGRAMA", typeof(System.String)));
            dt_dataToExport.Columns.Add(new DataColumn("DESCRIPCIÓN", typeof(System.String)));

            ManttoService mservice = new ManttoService();

            if (ib_empresa)
            {
                dt_data = mservice.GetMantto(in_app.is_Empresas, ii_regcodi);
            }
            else
            {
                dt_data = mservice.GetMantto("0", ii_regcodi);
            }

            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_data.Rows)
                {
                    li_cont++;
                    ls_fechaInicial = DateTime.TryParse(dr["EVENINI"].ToString(), out ldt_fecha) ? ldt_fecha.ToString("dd/MM/yyyy HH:mm") : String.Empty;
                    ls_fechaFinal = DateTime.TryParse(dr["EVENFIN"].ToString(), out ldt_fecha) ? ldt_fecha.ToString("dd/MM/yyyy HH:mm") : String.Empty;
                    ls_tipo = nf_get_tipoMantto(dr["TIPOEVENABREV"].ToString());
                    ls_disponib = nf_get_indisponibilidad(dr["EVENINDISPO"].ToString());
                    ls_interrup = nf_get_interrupcion(dr["EVENINTERRUP"].ToString());
                    ls_programa = nf_get_tipoPrograma(dr["EVENTIPOPROG"].ToString());
                    ld_valor = Double.TryParse(dr["EVENMWINDISP"].ToString(), out ld_valor) ? ld_valor : 0;

                    dt_dataToExport.Rows.Add(li_cont, ls_tipo, dr["EMPRNOMB"].ToString(), dr["AREANOMB"].ToString(), 
                                             dr["FAMABREV"].ToString(), dr["EQUIABREV"].ToString(), ls_fechaInicial, 
                                             ls_fechaFinal, ld_valor, ls_disponib, ls_interrup, ls_programa, dr["EVENDESCRIP"].ToString());
                }


                string ls_cadena = ManRegistro.RegistroNombre.Replace(" ", String.Empty)
                                                             .Replace(".", String.Empty)
                                                             .Replace("-", String.Empty);

                byte[] lb_array_xls = UtilsExcel.CrearExcel2003(dt_dataToExport, ls_cadena);

                WriteFileStream(lb_array_xls, ls_cadena + ".xls", "application/vnd.ms-excel");
            }

        }

        private string nf_get_tipoPrograma(string ps_tipoPrograma)
        {
            switch (ps_tipoPrograma.Trim())
            {
                case "P":
                    return "PROGRAMADO";
                case "F":
                    return "FORZADO/IMPREVISTO";
                case "R":
                    return "REPROGRAMADO";
                default:
                    return "NO DEFINIDO";
            }
        }

        private string nf_get_interrupcion(string ps_interrupcion)
        {
            switch (ps_interrupcion.Trim())
            {
                case "S":
                    return "SI";
                case "N":
                    return "NO";
                default:
                    return "NO DEFINIDO";
            }
        }

        private string nf_get_indisponibilidad(string ps_indisponibilidad)
        {
            switch (ps_indisponibilidad.Trim())
            {
                case "E":
                    return "E/S";
                case "F":
                    return "F/S";
                default:
                    return "NO DEFINIDO";
            }
        }

        private string nf_get_tipoMantto(string ps_tipoMantto)
        {
            switch (ps_tipoMantto.Trim())
            {
                case "PREV.":
                    return "PREVENTIVO";
                case "CORR.":
                    return "CORRECTIVO";
                case "AMPL.":
                    return "AMPLIACION";
                case "PRUE.":
                    return "PRUEBAS";
                case "EVE.":
                    return "EVENTO";
                default:
                    return "NO DEFINIDO";
            }
        }

        static void WriteFileStream(byte[] ab_array, string as_fileNameWithExtension, string as_contentType)
        {
            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.Response.Clear();
                page.Response.ClearContent();
                page.Response.ClearHeaders();
                page.Response.ContentType = as_contentType;
                page.Response.Charset = String.Empty;
                page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + as_fileNameWithExtension);
                page.Response.OutputStream.Write(ab_array, 0, ab_array.Length);
                page.Response.OutputStream.Flush();
                page.Response.OutputStream.Close();
                page.Response.End();
            }

        }

    }
}