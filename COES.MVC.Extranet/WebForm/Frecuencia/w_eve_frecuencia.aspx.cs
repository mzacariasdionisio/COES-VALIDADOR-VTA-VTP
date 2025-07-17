using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WScoes;

namespace WSIC2010
{
    public partial class w_eve_frecuencia : System.Web.UI.Page
    {        
        DataSet i_ds = new DataSet();
        DataTable dtable1;

        protected void Page_Load(object sender, EventArgs e)
        {            

            if (!IsPostBack)
            {
                //ManttoService service = new ManttoService();
                //n_app in_app = new n_app();//(n_app)Session["in_app"];
                //string ls_comando = @" SELECT max(fechahora) as maxfecha, min(fechahora) as minfecha from f_lectura";
                //in_app.Fill(0, i_ds, "F_MINMAX", ls_comando);
                //dtable1 = i_ds.Tables["F_MINMAX"];
                DateTime dtinicio = DateTime.Today;//((DateTime)dtable1.Rows[0]["maxfecha"]);
                DateTime dtfinal = DateTime.Today.AddYears(-1);//((DateTime)dtable1.Rows[0]["minfecha"]);
                int anhomax = dtinicio.Year;
                int anhomin = dtfinal.Year;

                for (int year = anhomax; year >= anhomin; year--)
                    DropDownListYears.Items.Add(Convert.ToString(year));

                for (int i = 1; i <= 31; i++)
                {
                    DropDownListDay.Items.Add(i.ToString().PadLeft(2, '0'));
                    //DropDownListDay2.Items.Add(i.ToString().PadLeft(2, '0'));
                }
                for (int i = 1; i <= 12; i++)
                {
                    DropDownListMonths.Items.Add(i.ToString().PadLeft(2, '0'));
                }

                DropDownListDay.SelectedValue = dtinicio.Day.ToString().PadLeft(2, '0');
                DropDownListMonths.SelectedIndex = dtinicio.Month - 1;
                DropDownListYears.SelectedValue = dtinicio.Year.ToString();
            }
        }

        protected void ButtonGenerarMedicion_Click(object sender, EventArgs e)
        {
            CargarFrecuencias(Convert.ToInt32(DropDownListYears.SelectedValue), Convert.ToInt32(DropDownListMonths.SelectedValue), Convert.ToInt32(DropDownListDay.SelectedValue));
        }

        void CargarFrecuencias(int ai_year, int ai_month, int ai_dia)
        {
            int li_diainicio, li_diafin;
            li_diainicio = ai_dia;
            li_diafin = ai_dia;


            string query = "* from f_lectura where gpscodi=1 and to_char(fechahora,'YYYYMMDD')= '" + ai_year.ToString() + ai_month.ToString().Trim().PadLeft(2, '0') + ai_dia.ToString().Trim().PadLeft(2, '0') + "'";
            query += " order by fechahora";

            n_app in_app = new n_app();
            string ls_comando = @" SELECT " + query;
            in_app.Fill(0, i_ds, "F_LECTURA", ls_comando);
            dtable1 = i_ds.Tables["F_LECTURA"];

            //dtable1 = in_EPservices.GetDTData("SELECT|" + query, ii_key);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();


            context.Response.Write(Environment.NewLine);
            context.Response.Write("Fecha Hora" + ",");
            for (int i = 0; i < 59; i++)
            {
                context.Response.Write("S" + i + ",");
            }
            context.Response.Write("S59");
            context.Response.Write(Environment.NewLine);           

            foreach (DataRow dr in dtable1.Rows)
            {
                int li_num = Convert.ToInt32(dr["num"]);
                if (li_num == 60)
                {
                    DateTime ldt_hora = (DateTime)dr["fechahora"];
                    context.Response.Write(ldt_hora.ToString("yyyy-MM-dd HH:mm:ss") + ",");
                    double ld_frecuencia = 0;
                    for (int i = 0; i < 60; i++)
                    {
                        ld_frecuencia = Convert.ToDouble(dr["h" + i]);
                        if (i < 59)
                            context.Response.Write(ld_frecuencia.ToString() + ",");
                        else
                            context.Response.Write(ld_frecuencia.ToString());
                    }
                    context.Response.Write(Environment.NewLine);
                }
            }

            context.Response.Write(Environment.NewLine);
            
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=Frecuencia_" + ai_year.ToString() + ai_month.ToString().Trim().PadLeft(2, '0') + li_diainicio.ToString().Trim().PadLeft(2, '0') + ".csv");
            context.Response.End();
        }
    }
}