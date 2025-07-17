using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using WScoes;

namespace WSIC2010.Mantto
{
    public partial class w_eve_cargarcco : System.Web.UI.Page
    {
        n_app in_app;
        int i_regcodi = 0;
        DateTime idt_fechini;
        DateTime idt_fechfin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/mantto/w_eve_cargarcco.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                if(!in_app.ib_IsIntranet)
                    Response.Redirect("~/About.aspx");

                if (!IsPostBack)
                {

                    Dictionary<int, string> H_evenclase = new Dictionary<int, string>();
                    if (Session["H_EVE_EVENCLASE"] == null)
                    {
                        DataSet i_ds = new DataSet("dslogin");
                       // n_app in_app = (n_app)Session["in_app"];
                        string ls_comando = @" SELECT * FROM EVE_EVENCLASE ORDER BY EVENCLASECODI";
                        in_app.Fill(0, i_ds, "EVE_EVENCLASE", ls_comando);
                        //in_app.iL_data[0].Fill(i_ds, "EVE_EVENCLASE", ls_comando);
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
                    DropDownListEvenClase.DataBind();

                    DropDownListEvenClase.SelectedIndex = 1;
                    //CalendarExtender1.SelectedDate = DateTime.Today.AddDays(1);
                }

                //if (Session["i_regcodi"] != null)
                //{
                //    i_regcodi = Convert.ToInt32(Session["i_regcodi"]);
                //    idt_fechini = (DateTime)Session["dt_FechaInicial"];
                //    idt_fechfin = (DateTime)Session["dt_FechaFinal"];
                //}
                //else
                //{
                //    UploadStatusLabel.Text = "Codigo registro mantto no definido";
                //    Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
                //}


            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            // Specify the path on the server to
            // save the uploaded file to.
            String savePath = @"d:\data\mantto\";

            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            if (FileUpload1.HasFile)
            {
                // Get the name of the file to upload.
                string fileName = Server.HtmlEncode(FileUpload1.FileName);

                // Get the extension of the uploaded file.
                string extension = System.IO.Path.GetExtension(fileName);

                // Allow only files with .doc or .xls extensions
                // to be uploaded.
                if (extension == ".xls")
                {
                    // Append the name of the file to upload to the path.
                    savePath += fileName;

                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileUpload1.SaveAs(savePath);

                    // Notify the user that their file was successfully uploaded.
                    UploadStatusLabel.Text = "Your file was uploaded successfully.";

                    //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Password=\"\";User ID=Admin;Data Source=" + savePath + ";Mode=Share Deny Write;Extended Properties=\"HDR=YES;\";Jet OLEDB:Engine Type=37";


                    string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

                    DataSet output = new DataSet();
                    //WScoes.ManttoServiceClient Mservicio = new WScoes.ManttoServiceClient();
                    ManttoService servicio = new ManttoService();

                    //List<int> L_equipos = Mservicio.L_GetCodiEquipos();
                    using (OleDbConnection conn = new OleDbConnection(strConn))
                    {
                        conn.Open();

                        //DataTable schemaTable = conn.GetOleDbSchemaTable(
                        //  OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        //foreach (DataRow schemaRow in schemaTable.Rows)
                        //{
                        //    string sheet = schemaRow["TABLE_NAME"].ToString();

                        OleDbCommand cmd = new OleDbCommand("SELECT F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14 FROM [MANTTOS$]", conn);
                        cmd.CommandType = CommandType.Text;

                        DataTable outputTable = new DataTable("MANTTOS");
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        //GridView1.DataSource = outputTable;
                        //GridView1.DataBind();
                      
                        bool lb_iniciar = false;
                        int li_xlsfila = 0;
                        bool lb_sinerrores = true;

                        foreach (DataRow dr in outputTable.Rows)
                        {
                            li_xlsfila++;
                            if (lb_iniciar)
                            {
                                int li_equicodi = -1;
                                if (int.TryParse(dr["F6"].ToString(), out li_equicodi))
                                {
                                    if (li_equicodi > 0)
                                    {
                                        if (!VerificarRegistro(dr, li_xlsfila))
                                        {
                                            //ListBox1.Items.Add("Error al cargar registro # " + li_xlsfila + " : " + dr["F4"].ToString() + "-" + dr["F5"].ToString());
                                            lb_sinerrores = false;
                                        }
                                    }
                                }
                            }
                            else
                                if (dr["F2"].ToString().Trim() == "ITEM")
                                    lb_iniciar = true;
                        }

                        lb_iniciar = false;
                        li_xlsfila = 0;
                        if (lb_sinerrores)
                        {
                            foreach (DataRow dr in outputTable.Rows)
                            {
                                li_xlsfila++;
                                if (lb_iniciar)
                                {
                                    int li_equicodi = -1;
                                    if (int.TryParse(dr["F6"].ToString(), out li_equicodi))
                                    {
                                        if (li_equicodi > 0)
                                        {
                                            if (!InsertarRegistro(dr, li_xlsfila))
                                            {
                                                ListBox1.Items.Add("Error al cargar registro # " + li_xlsfila + " : " + dr["F4"].ToString() + "-" + dr["F5"].ToString());
                                            }
                                        }
                                    }
                                }
                                else
                                    if (dr["F2"].ToString().Trim() == "ITEM")
                                        lb_iniciar = true;
                            }
                        }
                        else
                        {
                            UploadStatusLabel.Text = "Archivo no cargado, corrija e intente de nuevo!";
                        }
                    }
                    if (ListBox1.Items.Count == 0)
                        Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
                }
                else
                {
                    // Notify the user why their file was not uploaded.
                    UploadStatusLabel.Text = "Your file was not uploaded because it does not have a .xls extension.";
                }
            }
        }

        bool VerificarRegistro(DataRow a_dr, int a_linea)
        {
            bool lb_registrobueno = true;
            try
            {              

                if (a_dr["F9"] == DBNull.Value)
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " Formato Fecha Erroneo :" + a_dr["F7"].ToString());
                    return false;
                }

                DateTime dtini = new DateTime(2000, 1, 1);
                if (a_dr["F7"] != DBNull.Value)
                {                    
                    if (EPDate.IsDate(a_dr["F7"].ToString()))
                    {
                        dtini = EPDate.ToDateTime(a_dr["F7"].ToString());
                    }
                    else
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " Formato Fecha Erroneo :" + a_dr["F7"].ToString());
                        lb_registrobueno = false;
                    }                 
                }

                DateTime dtfin = new DateTime(2000, 1, 1);
                if (a_dr["F8"] != DBNull.Value)
                {

                    if (EPDate.IsDate(a_dr["F8"].ToString()))
                    {
                        dtfin = EPDate.ToDateTime(a_dr["F8"].ToString());
                    }
                    else
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " Formato Fecha Erroneo :" + a_dr["F8"].ToString());
                        lb_registrobueno = false;
                    }                    
                }

                //verifica que el rango de fechas es valido

                if (dtini < idt_fechini || dtfin > idt_fechfin.AddDays(1))
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " fecha fuera de rango: " + dtini.ToString() + "-" + dtfin.ToString());
                    lb_registrobueno = false;
                }

                if (dtini >= dtfin )
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " fecha final es mayor o igual al inicial : " + dtini.ToString() + "-" + dtfin.ToString());
                    lb_registrobueno = false;
                }

                if (a_dr["F13"] != DBNull.Value)
                {
                    //UpdateQuery1 += ",TIPOEVENCODI";
                    string tec = a_dr["F13"].ToString().Substring(0, 3).ToUpper();
                    string s_tipoevencodi = "1";
                    //if (tec == "PRE")
                    //    s_tipoevencodi = "1";
                    if (tec == "COR")
                        s_tipoevencodi = "2";
                    if (tec == "AMP")
                        s_tipoevencodi = "3";
                    if (tec == "PRU")
                        s_tipoevencodi = "6";
                    //UpdateQuery2 += "," + s_tipoevencodi + " ";
                }


                if (a_dr["F14"] != DBNull.Value)
                {
                   // UpdateQuery1 += ",EVENTIPOPROG";
                    string tec = "P";
                    try
                    {
                        tec = a_dr["F14"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENTIPOPROG");
                    }
                    //UpdateQuery2 += ",'" + tec + "'";
                }

                if (a_dr["F11"] != DBNull.Value)
                {
                    //UpdateQuery1 += ",EVENINDISPO";
                    string tec = "F";
                    try
                    {
                        tec = a_dr["F11"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENINDISPO");
                    }
                    //UpdateQuery2 += ",'" + tec + "'";
                }

                if (a_dr["F12"] != DBNull.Value)
                {
                    //UpdateQuery1 += ",EVENINTERRUP";
                    string tec = "N";
                    try
                    {
                        tec = a_dr["F12"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENINTERRUP");
                        lb_registrobueno = false;
                    }
                    //UpdateQuery2 += ",'" + tec + "'";
                }

                //UpdateQuery1 += ",EVENMWINDISP";
                if (a_dr["F10"] != DBNull.Value)
                {
                    //UpdateQuery2 += "," + a_dr["F10"].ToString();
                }
                else
                {
                    //UpdateQuery2 += ",0";
                }
               
            }
            catch
            {
                return false;
            }
            return lb_registrobueno;
        }

        bool InsertarRegistro(DataRow a_dr, int a_linea)
        {
            try
            {
                string UpdateQuery1 = "EQUICODI";
                string UpdateQuery2 = a_dr["F6"].ToString();

                if (a_dr["F9"] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENDESCRIP";
                    UpdateQuery2 += ",'" + a_dr["F9"].ToString() + "'";
                }

                DateTime dtini = new DateTime(2000, 1, 1);
                if (a_dr["F7"] != DBNull.Value)
                {
                    //DateTime l_dt;
                    if (EPDate.IsDate(a_dr["F7"].ToString()))
                    {
                        dtini = EPDate.ToDateTime(a_dr["F7"].ToString());
                    }
                    else
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " Formato Fecha Erroneo :" + a_dr["F7"].ToString());
                        return false;
                    }
                    UpdateQuery1 += ",EVENINI";
                    UpdateQuery2 += "," + EPDate.SQLDateTimeOracleString(dtini);
                }

                DateTime dtfin = new DateTime(2000, 1, 1);
                if (a_dr["F8"] != DBNull.Value)
                {

                    if (EPDate.IsDate(a_dr["F8"].ToString()))
                    {
                        dtfin = EPDate.ToDateTime(a_dr["F8"].ToString());
                    }
                    else
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " Formato Fecha Erroneo :" + a_dr["F8"].ToString());
                        return false;
                    }

                    UpdateQuery1 += ",EVENFIN";
                    UpdateQuery2 += "," + EPDate.SQLDateTimeOracleString(dtfin);
                }

                //verifica que el rango de fechas es valido

                if (dtini < idt_fechini || dtfin > idt_fechfin.AddDays(1))
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " fecha fuera de rango: " + dtini.ToString() + "-" + dtfin.ToString());
                    return false;
                }

                if (dtini >= dtfin)
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " fecha final es mayor o igual al inicial : " + dtini.ToString() + "-" + dtfin.ToString());
                    return false;
                }

                if (a_dr["F13"] != DBNull.Value)
                {
                    UpdateQuery1 += ",TIPOEVENCODI";
                    string tec = a_dr["F13"].ToString().Substring(0, 3).ToUpper();
                    string s_tipoevencodi = "1";
                    //if (tec == "PRE")
                    //    s_tipoevencodi = "1";
                    if (tec == "COR")
                        s_tipoevencodi = "2";
                    if (tec == "AMP")
                        s_tipoevencodi = "3";
                    if (tec == "PRU")
                        s_tipoevencodi = "6";
                    UpdateQuery2 += "," + s_tipoevencodi + " ";
                }


                if (a_dr["F14"] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENTIPOPROG";
                    string tec = "P";
                    try
                    {
                        tec = a_dr["F14"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENTIPOPROG");
                    }
                    UpdateQuery2 += ",'" + tec + "'";
                }

                if (a_dr["F11"] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENINDISPO";
                    string tec = "F";
                    try
                    {
                        tec = a_dr["F11"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENINDISPO");
                    }
                    UpdateQuery2 += ",'" + tec + "'";
                }

                if (a_dr["F12"] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENINTERRUP";
                    string tec = "N";
                    try
                    {
                        tec = a_dr["F12"].ToString().Substring(0, 1).ToUpper();
                    }
                    catch
                    {
                        ListBox1.Items.Add("Registro en fila " + a_linea + " observado EVENINTERRUP");
                    }
                    UpdateQuery2 += ",'" + tec + "'";
                }

                UpdateQuery1 += ",EVENMWINDISP";
                if (a_dr["F10"] != DBNull.Value)
                {
                    UpdateQuery2 += "," + a_dr["F10"].ToString();
                }
                else
                {
                    UpdateQuery2 += ",0";
                }

                ManttoService service = new ManttoService();

                int li_temp = 1;// service.InsertMantto(i_regcodi, dtini, dtfin, UpdateQuery1, UpdateQuery2, in_app.is_UserLogin + " " + in_app.is_PC_IPs);
                ListBox1.Items.Add("InsertMantto: " + UpdateQuery1 + "::" + UpdateQuery2);

                if (li_temp > 0)
                {
                    UploadStatusLabel.Text = "Informacion actualizada";
                }
                else
                {
                    UploadStatusLabel.Text = "Error en actualizacion";
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected void ButtonVerificar_Click(object sender, EventArgs e)
        {

        }

       
    }
}