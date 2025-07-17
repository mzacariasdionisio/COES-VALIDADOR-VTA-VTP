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
    public partial class w_eve_add_mantto : System.Web.UI.Page
    {
        int ii_regcodi = -1;
        bool ib_empresa = false;
        int ii_evenclasecodi = -1;

        CManttoRegistro ManRegistro;
        n_app in_app;        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                n_app in_app = (n_app)Session["in_app"];
                if (Session["i_regcodi"] != null)
                {
                    CascadingDropDown1.ContextKey = in_app.is_Empresas;
                    ii_regcodi = (int)Session["i_regcodi"];

                    ManttoService manservice = new ManttoService();
                    ManRegistro = manservice.GetManttoRegistro(ii_regcodi);

                    //Asigna tipo de programa
                    ii_evenclasecodi = ManRegistro.EvenClaseCodi;

                    if (Session["i_ddlEmpresa"] != null)
                    {
                        CascadingDropDown1.SelectedValue = Convert.ToString(Session["i_ddlEmpresa"]);
                        try
                        {
                            if (Session["i_ddlArea"] != null)
                                CascadingDropDown2.SelectedValue = Convert.ToString(Session["i_ddlArea"]);
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    ii_regcodi = 22;
                    Response.Redirect("~/WebForm/mantto/w_eve_mantto.aspx");
                }
                
            }

        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxEquiCodi.Text = ddlEquipo.SelectedValue; 
            LabelEquipoElegido.Text = ddlEquipo.SelectedItem.Text;           
        }

        public int EQUICODI()
        {
            return Convert.ToInt32(TextBoxEquiCodi.Text);           
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["i_ddlEmpresa"] = ddlEmpresa.SelectedValue;
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["i_ddlArea"] = ddlArea.SelectedValue;
        }

        protected void ButtonBorrarArchivo_Click(object sender, EventArgs e)
        {
            if (ListBoxArchivosCargados.DataSource != null)
            {
                if (ListBoxArchivosCargados.SelectedIndex >= 0)
                {
                    string ls_mancodi = "X";
                    //Label lbl = (ListViewManttoList.Items[ListViewManttoList.EditItem.DataItemIndex].FindControl("mancodiLabel")) as Label;
                    //ls_mancodi = lbl.Text;


                    ManttoService service = new ManttoService();
                    int li_temp = service.DeleteReferenciaArchivoMantto(Convert.ToInt32(ls_mancodi), Convert.ToInt32(ListBoxArchivosCargados.SelectedValue), in_app.is_UserLogin + ":" + in_app.is_PC_IPs);
                    ActualizarListaArchivos(service, Convert.ToInt32(ls_mancodi));
                    LabelMensaje.Text = "Archivo registro borrado! : [" + ListBoxArchivosCargados.SelectedValue.ToString() + "]";
                }
                else
                {
                    LabelMensaje.Text = "Debe seleccionar el archivo! ";
                }
            }
        }

        void ActualizarListaArchivos(ManttoService i_service, int ai_mancodi)
        {
            DataTable ln_table;
            ln_table = i_service.GetArchivosMantto(Convert.ToInt32(ai_mancodi));
            ListBoxArchivosCargados.DataSource = ln_table;
            ListBoxArchivosCargados.DataValueField = "NUMARCHIVO";
            ListBoxArchivosCargados.DataTextField = "DESCARCHIVO";
            ListBoxArchivosCargados.DataBind();
        }

        protected void ButtonAbrirArchivo_Click(object sender, EventArgs e)
        {
            if (ListBoxArchivosCargados.DataSource != null)
            {
                if (ListBoxArchivosCargados.SelectedIndex >= 0)
                {
                    DataTable dt = new DataTable();

                    if (ListBoxArchivosCargados.DataSource != null)
                    {

                        dt = (DataTable)ListBoxArchivosCargados.DataSource;
                        LabelMensaje.Text = dt.Rows[ListBoxArchivosCargados.SelectedIndex]["NOMBARCHIVO"].ToString();
                        string strUrl = System.Configuration.ConfigurationManager.AppSettings["DocumenDir"] + dt.Rows[ListBoxArchivosCargados.SelectedIndex]["NOMBARCHIVO"].ToString().Trim();

                        Response.Redirect(strUrl);
                    }
                }
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            string ls_mancodi = "X";
            //Label lbl = (ListViewManttoList.Items[ListViewManttoList.EditItem.DataItemIndex].FindControl("mancodiLabel")) as Label;
            //ls_mancodi = lbl.Text;

            if (!EPDate.IsDate(tBoxInicio.Value))
            {
                LabelMensaje.Text = "Fecha Inicio Erronea";
                return;
            }

            DateTime dt = EPDate.ToDateTime(tBoxInicio.Value);
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
            if (FileUpload1 != null && FileUpload1.HasFile)
            {
                // Get the name of the file to upload.
                string fileName = ls_mancodi + "_" + Server.HtmlEncode(FileUpload1.FileName);
                //string extension = System.IO.Path.GetExtension(fileName);
                savePath += fileName;
                FileUpload1.SaveAs(savePath);

                if (!String.IsNullOrEmpty(TextBoxDescArchivo.Text))
                {
                    TextBoxDescArchivo.Text = Server.HtmlEncode(FileUpload1.FileName);
                }

                // Notify the user that their file was successfully uploaded.
                LabelMensaje.Text = "Tu archivo ha sido cargado satisfactoriamente.";

                string UpdateQuery1 = "NOMBARCHIVO,DESCARCHIVO";
                string UpdateQuery2 = "'" + ls_webpath.Replace("\\", "/") + fileName + "','" + TextBoxDescArchivo.Text + "'";

                //if (ListBoxArchivosCargados.DataSource != null)
                //{
                    //int li_counter = listboxAC.Items.Count;
                    ManttoService service = new ManttoService();
                    //int li_temp = service.InsertReferenciaArchivoMantto(Convert.ToInt32(ls_mancodi), UpdateQuery1, UpdateQuery2, in_app.is_UserLogin + ":" + in_app.is_PC_IPs);
                    //ActualizarListaArchivos(service, Convert.ToInt32(ls_mancodi));
                    //if (li_temp > 0)
                    //{
                        //DataTable ln_table;
                        //ln_table = service.GetArchivosMantto(service, Convert.ToInt32(ls_mancodi));
                        //listboxAC.DataSource = ln_table;
                        //listboxAC.DataValueField = "NUMARCHIVO";
                        //listboxAC.DataTextField = "DESCARCHIVO"; 
                        //listboxAC.DataBind();
                        TextBoxDescArchivo.Text = "";
                        LabelMensaje.Text = "Informacion cargada!";
                    //}
                    //else
                    //{
                    //    LabelMensaje.Text = "Error en actualizacion";
                    //}
                //}
            }
            else
            {
                // Notify the user why their file was not uploaded.
                LabelMensaje.Text = "Tu archivo NO ha podido ser cargado";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_lista_manttos.aspx", true);
        }
    }
}