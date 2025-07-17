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
    public partial class w_eve_details_mantto : System.Web.UI.Page
    {
        int ii_regcodi = -1;
        bool ib_empresa = false;
        int ii_evenclasecodi = -1;

        CManttoRegistro ManRegistro;
        n_app in_app;

        public int ii_man_codi;

        protected void Page_Load(object sender, EventArgs e)
        {
            string ls_param;
            ii_man_codi = -1;
            ls_param = Page.Request.QueryString.Get("qs_mancodi");

            if (ls_param == null)
            {
                Response.Write("No existe el código.");
            }
            else
            {
                ii_man_codi = Convert.ToInt32(ls_param);
                ManttoService mservice = new ManttoService();
                DataTable ln_table = mservice.GetMantto(ii_man_codi);

                if (ln_table.Rows.Count > 0)
                {
                    this.nf_llena_evento(ln_table);
                }
                else
                {
                    Response.Write("No existe el código.");
                }
            }

            
        }

        private void nf_llena_evento(DataTable adt_data)
        {
            DateTime ldt_date = new DateTime(2013, 1, 1);
            string ls_date = String.Empty;
            if (adt_data != null && adt_data.Rows.Count > 0)
            {
                foreach (DataRow dr in adt_data.Rows)
                {
                    lbl_Empresa.Text = dr["EMPRNOMB"].ToString().Trim();
                    lbl_area.Text = dr["AREANOMB"].ToString().Trim();
                    lbl_familia.Text = dr["FAMABREV"].ToString().Trim();
                    lbl_equipo.Text = dr["EQUIABREV"].ToString().Trim();


                    ddlTipoEven.SelectedValue = dr["TIPOEVENCODI"].ToString().Trim();
                    txtBoxInicio.Value = DateTime.TryParse(dr["EVENINI"].ToString().Trim(), out ldt_date) ? EPDate.ToDate(ldt_date.ToString("dd/MM/yyyy HH:mm")).ToString("dd/MM/yyyy HH:mm") 
                                                                                                 : String.Empty;
                    txtBoxFin.Value = DateTime.TryParse(dr["EVENFIN"].ToString().Trim(), out ldt_date) ? EPDate.ToDate(ldt_date.ToString("dd/MM/yyyy HH:mm")).ToString("dd/MM/yyyy HH:mm")
                                                                                                 : String.Empty;

                    ddlMWIndisp.Text = dr["EVENMWINDISP"].ToString().Trim();
                    ddlIndisp.SelectedValue = dr["EVENINDISPO"].ToString().Trim();
                    ddlInterrup.SelectedValue = dr["EVENINTERRUP"].ToString().Trim();
                    ddlTipoMantto.SelectedValue = dr["EVENTIPOPROG"].ToString().Trim();

                    txtBoxDescrip.Text = dr["EVENDESCRIP"].ToString().Trim();
                    txtBoxObserv.Text = dr["EVENOBSRV"].ToString().Trim();


                }
            }
            
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonAbrirArchivo_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonBorrarArchivo_Click(object sender, EventArgs e)
        {

        }
    }
}