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
using System.Text;
using System.Web.UI.HtmlControls;
using fwapp;

namespace WSIC2010
{
    public partial class w_me_listarcargas : System.Web.UI.Page
    {
        n_app in_app;
        string ls_sql;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Hidrologia/Consultas/w_me_listarcargas.aspx";
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

                        txt_fechaini.Text = DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");
                        txt_fechafin.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        //EMPRESA
                        Dictionary<int, string> H_evenclase = new Dictionary<int, string>();
                        if (Session["SI_EMPRESA0"] == null)
                        {
                            DataSet i_ds = new DataSet("dslogin");
                            string ls_sql = "SELECT EMPRNOMB, EMPRCODI FROM SI_EMPRESA WHERE EMPRSEIN='S' AND EMPRCOES='S' AND EMPRCODI>0 AND EMPRCODI IN (" + in_app.is_Empresas + ") ORDER BY EMPRCODI ";
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
                            string ls_sql = "SELECT ETACODI,ETADESCRIP FROM EXT_TIPO_ARCHIVO WHERE ETAACTIVO='S' ORDER BY 1 ";
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





                    }

                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }


            }
        }

        protected void b_consultar_Click(object sender, EventArgs e)
        {
            DateTime ld_fechaini;
            DateTime ld_fechafin;

            ListBox1.Items.Clear();

            try
            {
                ld_fechaini = EPDate.ToDate(txt_fechaini.Text);//Convert.ToDateTime(txt_fechaini.Text);
            }
            catch
            {
                //txt_fechaini.Text=DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");
                //ld_fechaini = Convert.ToDateTime(txt_fechaini.Text);
                ld_fechafin = DateTime.Now;
                ListBox1.Items.Add("Fecha inicial no valida");
                return;
            }


            try
            {
                ld_fechafin = EPDate.ToDate(txt_fechafin.Text);//Convert.ToDateTime(txt_fechafin.Text);
            }
            catch
            {
                ld_fechafin = DateTime.Now;
                ListBox1.Items.Add("Fecha final no valida");
                return;
            }

            TimeSpan ts1 = ld_fechafin - ld_fechaini;

            if (ts1.TotalDays > 60)
            {
                ListBox1.Items.Add("Limite de Fechas de consulta excedido.");
                gv.DataSource = null;
                gv.DataBind();
                return;
            }

            ExtService srv_ext = new ExtService();




            int li_formato = 0;

            try
            {
                li_formato = Convert.ToInt32(ddlb_formato.SelectedItem.Value.ToString());
            }
            catch
            {
                li_formato = 0;
            }


            gv.DataSource = srv_ext.nf_GetListarCargas(Convert.ToInt32(ddlb_empresa.SelectedItem.Value.ToString()), Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString()), li_formato, ld_fechaini, ld_fechafin);
            //i_ds.Tables["ME_LISTARCARGAS"];
            gv.DataBind();

            ListBox1.Items.Add("Nro. de registro(s) : " + gv.Rows.Count);
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //cambio color
            //e.Row.Cells[1].BackColor= new rgb
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

        protected void b_excel_Click(object sender, EventArgs e)
        {
            //para exportar sin un aspxgridview exporter
            //se debe inicializar varias clases
            try
            {
                DateTime ld_fechaini;
                DateTime ld_fechafin;

                ListBox1.Items.Clear();

                try
                {
                    ld_fechaini = EPDate.ToDate(txt_fechaini.Text);//Convert.ToDateTime(txt_fechaini.Text);
                }
                catch
                {
                    //txt_fechaini.Text=DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");
                    //ld_fechaini = Convert.ToDateTime(txt_fechaini.Text);
                    ld_fechafin = DateTime.Now;
                    ListBox1.Items.Add("Fecha inicial no valida");
                    return;
                }


                try
                {
                    ld_fechafin = EPDate.ToDate(txt_fechafin.Text);//Convert.ToDateTime(txt_fechafin.Text);
                }
                catch
                {
                    ld_fechafin = DateTime.Now;
                    ListBox1.Items.Add("Fecha final no valida");
                    return;
                }

                ExtService srv_ext = new ExtService();

                int li_formato = 0;

                try
                {
                    li_formato = Convert.ToInt32(ddlb_formato.SelectedItem.Value.ToString());
                }
                catch
                {
                    li_formato = 0;
                }
                DataTable dt = srv_ext.nf_GetListarCargas(Convert.ToInt32(ddlb_empresa.SelectedItem.Value.ToString()), Convert.ToInt32(ddlb_informacion.SelectedItem.Value.ToString()), li_formato, ld_fechaini, ld_fechafin);
                string attachment = "attachment; filename=ListarCargas.xls";
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }


}