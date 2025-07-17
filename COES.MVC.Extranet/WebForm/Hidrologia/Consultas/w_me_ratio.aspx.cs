using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using WScoes;
using System.IO;

namespace WSIC2010
{
    public partial class w_me_ratio : System.Web.UI.Page
    {
        n_app in_app;
        string ls_sql;
        int li_earcodi;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Hidrologia/Consultas/w_me_ratio.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                //if (!in_app.ib_IsIntranet)
                //    Response.Redirect("~/About.aspx");
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Hidrologia) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {

                        //txt_fechaini.Text = DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");
                        //txt_fechafin.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        //EMPRESA
                        Dictionary<int, string> H_evenclase = new Dictionary<int, string>();
                        if (Session["SI_EMPRESA0"] == null)
                        {
                            DataSet i_ds = new DataSet("dslogin");
                            //string ls_sql = "SELECT EMPRNOMB, EMPRCODI FROM SI_EMPRESA WHERE EMPRSEIN='S' AND EMPRCOES='S' AND EMPRCODI>0 AND EMPRCODI IN (" + in_app.is_Empresas + ") ORDER BY EMPRCODI ";
                            string ls_sql = "SELECT EMPRNOMB, EMPRCODI FROM SI_EMPRESA WHERE EMPRSEIN='S' AND EMPRCOES='S' AND EMPRCODI>0 ORDER BY EMPRCODI ";
                            in_app.Fill(0, i_ds, "SI_EMPRESA0", ls_sql);

                            foreach (DataRow dr in i_ds.Tables["SI_EMPRESA0"].Rows)
                            {
                                H_evenclase[Convert.ToInt32(dr["EMPRCODI"])] = dr["EMPRNOMB"].ToString();
                            }
                            Session["SI_EMPRESA0"] = H_evenclase;
                        }
                        else
                        {
                            H_evenclase = (Dictionary<int, string>)Session["SI_EMPRESA0"];
                        }

                        ddlb_empresa.DataSource = H_evenclase;
                        ddlb_empresa.DataTextField = "value";
                        ddlb_empresa.DataValueField = "key";
                        ddlb_empresa.DataBind();

                        ddlb_empresa.SelectedIndex = 0;

                        //TIPO ARCHIVO
                        Dictionary<int, string> H_tipoarchivo = new Dictionary<int, string>();
                        if (Session["EXT_TIPO_ARCHIVO"] == null)
                        {
                            DataSet i_ds = new DataSet("dslogin");
                            //string ls_sql = "SELECT ETACODI,ETADESCRIP FROM EXT_TIPO_ARCHIVO WHERE ETACODI IN (0,2,3,4) ORDER BY 1 ";
                            string ls_sql = "SELECT ETACODI,ETADESCRIP FROM EXT_TIPO_ARCHIVO WHERE ETAACTIVO='S' AND ETACODI>0 ORDER BY 1 ";
                            in_app.Fill(0, i_ds, "EXT_TIPO_ARCHIVO", ls_sql);
                            foreach (DataRow dr in i_ds.Tables["EXT_TIPO_ARCHIVO"].Rows)
                            {
                                H_tipoarchivo[Convert.ToInt32(dr["ETACODI"])] = dr["ETADESCRIP"].ToString();
                            }
                            Session["EXT_TIPO_ARCHIVO"] = H_tipoarchivo;
                        }
                        else
                        {
                            H_tipoarchivo = (Dictionary<int, string>)Session["EXT_TIPO_ARCHIVO"];
                        }

                        ddlb_informacion.DataSource = H_tipoarchivo;
                        ddlb_informacion.DataTextField = "value";
                        ddlb_informacion.DataValueField = "key";
                        ddlb_informacion.DataBind();

                        try
                        {
                            ddlb_informacion.SelectedIndex = 0;
                        }
                        catch { }


                        ddlb_informacion_SelectedIndexChanged(this, null);


                        //ddlb_anio
                        f_set_anio();


                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }


            }
        }

        private void f_get_datos(int pi_earcodi)
        {
            ListBox1.Items.Clear();


            ExtService srv_ext = new ExtService();

            gv.DataSource = srv_ext.nf_GetListarLog(pi_earcodi);
            gv.DataBind();

            ListBox1.Items.Add("Nro. de registro(s) : " + gv.Rows.Count);

        }



        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //cambio color
            //e.Row.Cells[1].BackColor= new rgb
        }

        protected void b_consultar_Click(object sender, EventArgs e)
        {
            try
            {
                ListBox1.Items.Clear();


                ExtService srv_ext = new ExtService();

                int li_formato = 0;



                //nf_GetListarRatio(int pi_emprcodi, int pi_etacodi,int pi_eaicodi,  DateTime pdt_fechaini, DateTime pdt_fechafin)//, ref System.Web.UI.WebControls.ListBox PLBox1)

                DateTime ldt_fechafin= Convert.ToDateTime(ddlb_anio.SelectedValue+"-09-30");

                if (DateTime.Now.Year.ToString() == ddlb_anio.SelectedValue)
                {
                    ldt_fechafin = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                }
                else
                {
                    ldt_fechafin = Convert.ToDateTime(ddlb_anio.SelectedValue + "-12-31");
                }



                try
                {
                    li_formato = Convert.ToInt32(ddlb_formato.SelectedItem.Value.ToString());
                    gv.DataSource = srv_ext.nf_GetListarRatio(Convert.ToInt32(ddlb_empresa.SelectedItem.Value.ToString()), Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString()), li_formato, Convert.ToDateTime(ddlb_anio.SelectedValue + "-01-01"), ldt_fechafin);//, ref ListBox1);//(/*.nf_GetListarLog(pi_earcodi);
                    gv.DataBind();

                }
                catch
                {
                    li_formato = 0;
                    gv.DataSource = null;
                    gv.DataBind();
                }


                

                ListBox1.Items.Add("Nro. de registro(s) : " + gv.Rows.Count);
            }
            catch
            {
                gv.DataSource = null;
                gv.DataBind();
            }

        }

        protected void ddlb_informacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int li_formato = Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString());

            //if (li_formato != 0)
            //{
            ls_sql = "select EAICODI,EAIDESCRIP from EXT_ARCHIVO_ITEM WHERE EAIVALIDO='S' AND ETACODI=" + li_formato + " ORDER BY 1 ";
            //}
            /*else
            {
                ls_sql = "select EAICODI,EAIDESCRIP from EXT_ARCHIVO_ITEM WHERE EAIVALIDO='S' ORDER BY 1 ";
            }
            */
            //TIPO FORMATO
            Dictionary<int, string> H_tipoformato = new Dictionary<int, string>();
            //if (Session["EXT_ARCHIVO_ITEM"] == null)
            //{
            DataSet i_ds = new DataSet("dslogin");

            in_app.Fill(0, i_ds, "EXT_ARCHIVO_ITEM", ls_sql);
            foreach (DataRow dr in i_ds.Tables["EXT_ARCHIVO_ITEM"].Rows)
            {
                H_tipoformato[Convert.ToInt32(dr["EAICODI"])] = dr["EAIDESCRIP"].ToString();
            }
            Session["EXT_ARCHIVO_ITEM"] = H_tipoformato;
            /*
            }
            else
            {
                H_tipoformato = (Dictionary<int, string>)Session["EXT_ARCHIVO_ITEM"];
            }
            */

            ddlb_formato.DataSource = H_tipoformato;
            ddlb_formato.DataTextField = "value";
            ddlb_formato.DataValueField = "key";
            ddlb_formato.DataBind();

            try
            {
                ddlb_formato.SelectedIndex = 0;
            }
            catch { }
        }

        protected void f_set_anio()
        {
            int li_formato = Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString());

            Dictionary<int, string> H_Anio = new Dictionary<int, string>();
            DataSet i_ds = new DataSet("dslogin");

            //in_app.Fill(0, i_ds, "EXT_ARCHIVO_ITEM", ls_sql);

            /*
            foreach (DataRow dr in i_ds.Tables["EXT_ARCHIVO_ITEM"].Rows)
            {
                H_tipoformato[Convert.ToInt32(dr["EAICODI"])] = dr["EAIDESCRIP"].ToString();
            }
            */

            int li_anio;
            li_anio = DateTime.Now.Year;

            for (int li_i = 0; li_i <= 5; li_i++)
            {
                H_Anio[li_anio] = li_anio.ToString();
                li_anio--;
            }

                

            Session["EXT_ANIO"] = H_Anio;

            ddlb_anio.DataSource = H_Anio;
            ddlb_anio.DataTextField = "value";
            ddlb_anio.DataValueField = "key";
            ddlb_anio.DataBind();

            try
            {
                ddlb_anio.SelectedIndex = 0;
            }
            catch { }
        }

        protected void Excel_Click(object sender, EventArgs e)
        {
            try
            {
                ListBox1.Items.Clear();


                ExtService srv_ext = new ExtService();

                int li_formato = 0;

                DateTime ldt_fechafin = Convert.ToDateTime(ddlb_anio.SelectedValue + "-09-30");

                if (DateTime.Now.Year.ToString() == ddlb_anio.SelectedValue)
                {
                    ldt_fechafin = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                }
                else
                {
                    ldt_fechafin = Convert.ToDateTime(ddlb_anio.SelectedValue + "-12-31");
                }


                try
                {
                    li_formato = Convert.ToInt32(ddlb_formato.SelectedItem.Value.ToString());
                    DataTable dt = srv_ext.nf_GetListarRatio(Convert.ToInt32(ddlb_empresa.SelectedItem.Value.ToString()), Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString()), li_formato, Convert.ToDateTime(ddlb_anio.SelectedValue + "-01-01"), ldt_fechafin);//, ref ListBox1);//(/*.nf_GetListarLog(pi_earcodi);
                    string attachment = "attachment; filename=EstadisticaCumplimiento.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");

                    int i;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                }
                catch
                {
                    li_formato = 0;
                    gv.DataSource = null;
                    gv.DataBind();
                }

            }
            catch
            {
                gv.DataSource = null;
                gv.DataBind();
            }
        }



                
    }


}