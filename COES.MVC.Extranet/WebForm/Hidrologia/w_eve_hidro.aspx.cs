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
using fwapp;

namespace WSIC2010
{
    public partial class w_eve_hidro : System.Web.UI.Page
    {
        n_app in_app;
        int i_regcodi = 0;
        DateTime idt_fechini;
        DateTime idt_fechfin;
        int li_logpsecuen = 0;
        int li_hidrocod = 0;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Hidrologia/w_eve_hidro.aspx";
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
                        /*
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
                        */
                        //CalendarExtender1.SelectedDate = DateTime.Today.AddDays(1);
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
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
            string savePath = @"d:\data\hidro\";
            string ls_version="1.0";


            ListBox1.Items.Clear();
            UploadStatusLabel.Text += "";

            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.

            DateTime ldt_fh = new DateTime(1999, 1, 1);
            DateTime ldt_fhfile = new DateTime(1999, 1, 1);

            //DateTime ldt_Fecha_inicial = DateTime.Now;            
            DateTime ldt_Fecha_inicial = EPDate.ToDate(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));		
            double DT1;
            TimeSpan Tspan;  

            UploadStatusLabel.Text = "";

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

                    ldt_fhfile = f_get_filename2date(fileName);

                    if (ldt_fhfile.Year < 2000)
                    {
                        UploadStatusLabel.Text = "Error en nombre de archivo. No se puede continuar";
                        return;
                    }


                    //string ls_filename_old = "1_" + fileName;
                    string ls_filename_old = fileName;
                    //File.Move(savePath + fileName, savePath + ls_ruta_final);
                    //savePath += ls_ruta_final;
                    
                    
                    
                     //string UpdateQuery2 = "'" + savePath.Replace("\\","/") + fileName + "','" + txtda.Text + "'";
                    //verificando fecha de archivo

                    

                    ExtService srv_ext= new ExtService();
                    //srv_idcc.f_set_insertIdccFile("EXT_ARCHIVO", ref fileName, "EARARCHTAMMB,EARARCHVER,EARARCHRUTA,USERCODE,EARIP", (new FileInfo(savePath + fileName).Length / (1024 * 1024.0)).ToString() + "," + ls_version + "," + savePath.Replace("\\", "/") + "," + in_app.ii_UserCode + "," + in_app.is_PC_IPs, in_app.is_UserLogin);

                    //int li_idcccod= srv_idcc.f_set_insertIdccFile("EXT_ARCHIVO", ref fileName, ",EARARCHVER,EARARCHRUTA,USERCODE,EARIP", ",'" + ls_version + "','" + savePath.Replace("\\", "/") + "','" + in_app.ii_UserCode + "','" + in_app.is_PC_IPs+"'", in_app.is_UserLogin);

                    //File.Move(savePath + ls_filename_old, savePath + fileName);
                    //savePath += fileName;
                    
                    // Append the name of the file to upload to the path.
                    //savePath += fileName;

                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileUpload1.SaveAs(savePath + fileName);

                    //double LONG = new FileInfo(savePath + fileName).Length;

                    //int 
                    li_hidrocod = srv_ext.f_set_insertFile("EXT_ARCHIVO",4, ref fileName, "EARARCHTAMMB,EARARCHVER,EARARCHRUTA,USERCODE,EARIP,ESTENVCODI", (new FileInfo(savePath + fileName).Length / (1024 * 1024.0)).ToString() + ",'" + ls_version + "','" + savePath.Replace("\\", "/") + "'," + in_app.ii_UserCode + ",'" + in_app.is_PC_IPs + "',1", in_app.is_UserLogin);

                    if (li_hidrocod > 0)
                    {
                        
                    }
                    File.Move(savePath + ls_filename_old, savePath + fileName);
                    savePath += fileName;


                    
                    // Notify the user that their file was successfully uploaded.
                    UploadStatusLabel.Text = "Procesando archivo...";

                    
                    f_set_tra_logpro_actualizar(li_hidrocod, 1, "Inicio de Procesado. Archivo : " + fileName, srv_ext);
                    

                    //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Password=\"\";User ID=Admin;Data Source=" + savePath + ";Mode=Share Deny Write;Extended Properties=\"HDR=YES;\";Jet OLEDB:Engine Type=37";
                    
                    bool lb_valida=true;
                    //ldt_fh = f_set_generacion("Anexo1_Diario", savePath, "ME_MEDICION48", "12", ldt_fhfile, ls_version, srv_ext, ref lb_valida); //TABLA FINAL: ME_MEDICION48


                    switch (Convert.ToInt32(ddlx_archivo_item.SelectedValue))
                    {

                        case 2: //HIDRO-ANEXO1-PDIARIO
                            ldt_fh = f_get_validafecha_version("Anexo1_Diario", savePath, EPDate.ToDate(ldt_fhfile.ToString("dd-MM-yyyy")), "1", srv_ext, ref lb_valida, 1, 1, 3, 3);
                            break;
                        case 3: //HIDRO-ANEXO1-PDIARIO
                            ldt_fh = f_get_validafecha_version("Anexo1_Semanal", savePath, EPDate.ToDate(ldt_fhfile.ToString("dd-MM-yyyy")), "1", srv_ext, ref lb_valida, 1, 1, 3, 3);
                            break;
                        case 4: //HIDRO-ANEXO2-EJECUTADO-EMBALSE
                            ldt_fh = f_get_validafecha_version("Anexo2", savePath, EPDate.ToDate(ldt_fhfile.ToString("dd-MM-yyyy")), "1", srv_ext, ref lb_valida, 1, 1, 3, 2);
                            break;
                        case 5: //HIDRO-ANEXO3-EJECUTADO-EMBALSE
                            ldt_fh = f_get_validafecha_version("Anexo3", savePath, EPDate.ToDate(ldt_fhfile.ToString("dd-MM-yyyy")), "1", srv_ext, ref lb_valida, 1, 1, 3, 2);
                            break;

                    }


                    
                    
                    
                    //fecha idcc                    
                    srv_ext.f_set_consulta1("update EXT_ARCHIVO set earfecha=" + EPDate.SQLDateOracleString(ldt_fh) + " where EARCODI=" + li_hidrocod);

                    if (!lb_valida)
                    {
                        srv_ext.f_set_consulta1("update EXT_ARCHIVO set ESTENVCODI=4 where EARCODI=" + li_hidrocod);
                        ListBox1.Items.Add("*** ENVIO RECHAZADO ***");
                        UploadStatusLabel.Text = "*** ENVIO RECHAZADO ***";

                        //Tspan = DateTime.Now - ldt_Fecha_inicial;
                        Tspan = EPDate.ToDate(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) - EPDate.ToDate(ldt_Fecha_inicial.ToString(("yyyy-MM-dd hh:mm:ss")));
                        DT1 = Math.Abs(Tspan.TotalSeconds);
                        DT1 = Convert.ToInt32(DT1);
                        f_set_tra_logpro_actualizar(li_hidrocod, 4, "Tiempo transcurrido: " + DT1.ToString() + " s. Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), srv_ext);
                        return;
                    }


                    if ((ldt_fhfile.Year < 2005) || (ldt_fh.Year < 2005))
                    {
                        UploadStatusLabel.Text += "Fecha no compatible.No se puede continuar (1)";
                        f_set_tra_logpro_actualizar(li_hidrocod, 15, "Fecha en hoja Excel no compatible", srv_ext);

                        srv_ext.f_set_consulta1("update EXT_ARCHIVO set ESTENVCODI=4 where EARCODI=" + li_hidrocod);

                        //Tspan = DateTime.Now - ldt_Fecha_inicial;
                        Tspan = EPDate.ToDate(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) - EPDate.ToDate(ldt_Fecha_inicial.ToString(("yyyy-MM-dd hh:mm:ss")));
                        DT1 = Math.Abs(Tspan.TotalSeconds);
                        DT1 = Convert.ToInt32(DT1);
                        f_set_tra_logpro_actualizar(li_hidrocod, 4, "Tiempo transcurrido: " + DT1.ToString() + " s. Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), srv_ext);
                        return;
                    }

                    switch (Convert.ToInt32(ddlx_archivo_item.SelectedValue))
                    {

                        case 2: //HIDRO-ANEXO1-PDIARIO
                            f_set_ax1diario("Anexo1_Diario", 2, savePath, "me_medicion1", 4, srv_ext, ref lb_valida, in_app.is_UserLogin, ldt_fh, 3,true);// "09:00:00", "12:00:00");
                            break;
                        case 3: //HIDRO-ANEXO1-PSEMANAL
                            //f_set_ax1diario("Anexo1_Semanal", 3, savePath, "me_medicion1", 3, srv_ext, ref lb_valida, in_app.is_UserLogin, ldt_fh, 10,"14:00:00","23:59:59");
                            f_set_ax1diario("Anexo1_Semanal", 3, savePath, "me_medicion1", 3, srv_ext, ref lb_valida, in_app.is_UserLogin, ldt_fh,10,false);// "09:00:00", "12:00:00");
                            break;
                        case 4: //HIDRO-ANEXO2-EJECUTADO-CAUDAL
                            f_set_ax2embalse("Anexo2", 4, savePath, "me_medicion24", 6, ldt_fh, srv_ext, ref lb_valida, in_app.is_UserLogin, true,10,33);
                            break;
                        case 5: //HIDRO-ANEXO3-EJECUTADO-EMBALSE
                            f_set_ax2embalse("Anexo3", 5, savePath, "me_medicion24", 6, ldt_fh, srv_ext, ref lb_valida, in_app.is_UserLogin, false,7,30);
                            break;                            

                    }
                    
                    /*
                    f_set_hop("HOPERACION", savePath, "EVE_HORAOPERACIONGRUPO", "7", ldt_fh, srv_ext, ref lb_valida); //TABLA FINAL: EVE_HORAOPERACIONGRUPO
                    f_set_combustible_consumo("COMBUSTIBLES", savePath, "PR_CONSUMOCOMBGRUPO", "7", ldt_fh, srv_ext, ref lb_valida); //TABLA FINAL: PR_CONSUMOCOMBGRUPO
                    f_set_combustible_stock("COMBUSTIBLES", savePath, "PR_CONSUMOCOMB", "7", ldt_fh, srv_ext, ref lb_valida); //TABLA FINAL: PR_CONSUMOCOMB
                    f_set_hidrologia_reporte("HIDROLOGIA", savePath, "ME_MEDICION48", "12", ldt_fh, srv_ext, ref lb_valida);//TABLA FINAL: ME_MEDICION48
                    f_set_hidrologia_vertim("HIDROLOGIA", savePath, "EVE_IEODCUADRO", "7", ldt_fh, srv_ext, ref lb_valida,207);//TABLA FINAL: EVE_IEODCUADRO
                    f_set_ivdf("IVDF", savePath, "F_IVDF", "7", ldt_fh, srv_ext, ref lb_valida);//TABLA FINAL: F_IVDF (nueva)
                    */

                    if (!lb_valida)
                    {
                        srv_ext.f_set_consulta1("update EXT_ARCHIVO set ESTENVCODI=4 where EARCODI=" + li_hidrocod);
                        ListBox1.Items.Add("*** ENVIO RECHAZADO ***");
                        UploadStatusLabel.Text = "*** ENVIO RECHAZADO ***";

                        //Tspan = DateTime.Now - ldt_Fecha_inicial;
                        Tspan = EPDate.ToDate(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) - EPDate.ToDate(ldt_Fecha_inicial.ToString(("yyyy-MM-dd hh:mm:ss")));
                        DT1 = Math.Abs(Tspan.TotalSeconds);
                        DT1 = Convert.ToInt32(DT1);
                        f_set_tra_logpro_actualizar(li_hidrocod, 4, "Tiempo transcurrido: " + DT1.ToString() + " s. Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), srv_ext);
                    }
                    else
                    {
                        srv_ext.f_set_consulta1("update EXT_ARCHIVO set ESTENVCODI=3 where EARCODI=" + li_hidrocod);
                        ListBox1.Items.Add("*** ENVIO ACEPTADO ***");
                        UploadStatusLabel.Text = "*** ENVIO ACEPTADO ***";

                        //Tspan = DateTime.Now - ldt_Fecha_inicial;
                        Tspan = EPDate.ToDate(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) - EPDate.ToDate(ldt_Fecha_inicial.ToString(("yyyy-MM-dd hh:mm:ss")));
                        DT1 = Math.Abs(Tspan.TotalSeconds);
                        DT1 = Convert.ToInt32(DT1);
                        f_set_tra_logpro_actualizar(li_hidrocod, 4, "Tiempo transcurrido: " + DT1.ToString() + " s. Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), srv_ext);
                    }


                    return;

                }
                else
                {
                    // Notify the user why their file was not uploaded.
                    UploadStatusLabel.Text = "EL archivo no se cargo porque no tiene la extension .xls";
                }
            }
            else
            {
                UploadStatusLabel.Text = "Se requiere cargar el archivo";
            }
        }

        public void f_set_tra_logpro_actualizar(int pi_earcodi,int pi_mencodi, string ps_detalle,  ExtService srv_idcc)
        {
            string ls_sql;

            li_logpsecuen++;
            ls_sql = "insert into ext_logpro (EARCODI, MENCODI, LOGPSECUEN, LOGPFECHOR, LOGPDETMEN)";
            ls_sql += " values ";
            ls_sql += "(" + pi_earcodi + "," + pi_mencodi.ToString() + "," + li_logpsecuen.ToString() + ",sysdate,'" + ps_detalle + "')";

            srv_idcc.f_set_consulta1(ls_sql);

        }

        private DateTime f_get_filename2date(string ps_cad)
        {
            string ls_cad = ps_cad;
            DateTime ldt_fecha;
            string ls_fecha;


            try
            {
                int li_idx = ls_cad.LastIndexOf("_");
                int li_idx2 = ls_cad.LastIndexOf(".");

                if (li_idx < 0)
                    return new DateTime(1999, 1, 1);

                ls_fecha = ls_cad.Substring(li_idx + 1, li_idx2 - li_idx - 1);
                ls_fecha = ls_fecha.Substring(0, 4) + "-" + ls_fecha.Substring(4, 2) + "-" + ls_fecha.Substring(6, 2);

                //ldt_fecha = Convert.ToDateTime(ls_fecha);

                ldt_fecha = EPDate.ToDate(ls_fecha);
                return ldt_fecha;

                //if (ldt_fecha.Year < 2000)
                //{
                //    return new DateTime(1999, 1, 1);
                //}


                //return new DateTime(1999, 1, 1);
            }
            catch
            {
                return new DateTime(1999, 1, 1);
            }
        }




        private DateTime f_get_validafecha_version(string ps_hoja, string ps_path, DateTime pdt_fhfile, string ps_version, ExtService srv_idcc, ref bool pb_valida, int pi_verfila, int pi_vercol, int pi_fechfila, int pi_fechcol)
        {
            DataTable outputTable;
                        
            pi_verfila--;
            pi_vercol--;
            pi_fechfila--;
            pi_fechcol--;

            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja, srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DateTime ldt_fh = new DateTime(2000, 1, 1);


            DataSet output = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    #region lectura matriz
                    conn.Open();

                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                    cmd.CommandType = CommandType.Text;

                    //DataTable outputTable = new DataTable(ps_hoja);
                    outputTable = new DataTable(ps_hoja);

                    output.Tables.Add(outputTable);
                    new OleDbDataAdapter(cmd).Fill(outputTable);





                }
                catch
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": Error en nombre de hoja. Descargue la version actualizada del menu Formatos/Hidrología. No se puede continuar (1)");
                    f_set_tra_logpro_actualizar(li_hidrocod, 14, "Error en nombre de hoja", srv_idcc);
                    pb_valida = false;
                    return new DateTime(2000, 1, 1);
                }


                string ls_version;

                DataRow dr;// = outputTable.Rows[pi_fila];
                DataRow drf;// = outputTable.Rows[pi_fila];

                //verificacion de version
                dr = outputTable.Rows[pi_verfila];
                drf = outputTable.Rows[pi_fechfila];

                //verificacion de version
                #region version
                //revision de version
                if ((dr[pi_vercol] != DBNull.Value) && (dr[pi_vercol].ToString() != ""))
                {
                    ls_version = dr[pi_vercol].ToString();
                    if (ls_version != ps_version)
                    {
                        ListBox1.Items.Add( "Version no compatible. Descargue la version actualizada del menu Formatos/Hidrología. No se puede continuar (1)");
                        f_set_tra_logpro_actualizar(li_hidrocod, 14, "Version no compatible", srv_idcc);
                        pb_valida = false;
                        return new DateTime(2000, 1, 1);
                    }
                }
                else
                {
                    ListBox1.Items.Add("Version no compatible. Descargue la version actualizada del menu Formatos/Hidrología. No se puede continuar (2)");
                    f_set_tra_logpro_actualizar(li_hidrocod, 14, "Version no compatible", srv_idcc);
                    pb_valida = false;
                    return new DateTime(2000, 1, 1);
                }

                #endregion

                //verificacion de fecha
                #region fecha

                //Util.Alert.Show(outputTable.Rows[pi_fechfila][pi_fechcol].);
                //int li_fechaInt = Convert.ToInt32(outputTable.Rows[2][3]);
                //ldt_fh = EPDate.ExcelSerialDateToDMY(li_fechaInt);

                ldt_fh = EPDate.ToDateMMDDYYYY(drf[pi_fechcol].ToString());
                
                pdt_fhfile = EPDate.ToDateMMDDYYYY(pdt_fhfile.ToString());


                //Util.Alert.Show("pdt_fhfile " + pdt_fhfile.ToString("dd-MM-yyyy") + " ldt_fh " + ldt_fh.ToString("dd-MM-yyyy"));

                if (ldt_fh != pdt_fhfile)
                {
                    //ListBox1.Items.Add("Fecha de nombre de archivo difiere de Fecha en hoja Excel. No se puede continuar (3)" + "pdt_fhfile :" + pdt_fhfile.ToString() +"ldt_fh :" + ldt_fh.ToString());
                    ListBox1.Items.Add("Fecha de nombre de archivo difiere de Fecha en hoja Excel. No se puede continuar (3)");
                    f_set_tra_logpro_actualizar(li_hidrocod, 15, "Fecha de nombre de archivo difiere de Fecha en hoja Excel", srv_idcc);
                    pb_valida = false;
                    return new DateTime(2000, 1, 1);
                }

                #endregion

                #endregion





            }

            return ldt_fh;
        }

        private DateTime f_set_generacion(string ps_hoja, string ps_path, string ps_tabla_gen, string ps_lectcodi, DateTime pdt_fhfile, string ps_version, ExtService srv_idcc, ref bool pb_valida)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: "+ps_hoja, srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DateTime ldt_fh = new DateTime(2000, 1, 1);


            DataSet output = new DataSet();
            //WScoes.ManttoServiceClient Mservicio = new WScoes.ManttoServiceClient();
            //ManttoService servicio = new ManttoService();

            //UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                
                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);
                                
                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;
                int li_col = 0;
                string ls_cad = "";

                string[] arr_cod1 = new string[256];
                string[] arr_cod2 = new string[256];
              
                string[] ls_campo = new string[100];
                string[,] ls_valores = new string[48,100];
                                
                int li_filabd=0;
                                
                
                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;
                string ls_version;

                string ls_colerror = "";
                                 
                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";

                    if (li_xlsfila == 1)
                    {

                      
                        //codigos
                        li_col = 0;

                        try
                        {

                            while ((dr[li_col] != DBNull.Value) && (dr[li_col].ToString()!=""))
                            {

                                arr_cod1[li_col] = dr[li_col].ToString();
                                ls_cad +=  dr[li_col].ToString()+" , ";
                                li_col++;
                            }
                        }
                        catch { }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue; 
                        

                    }


                    if (li_xlsfila == 2)
                    {
                        #region version
                        //revision de version
                        if ((dr[0] != DBNull.Value) && (dr[0].ToString() != ""))
                        {
                            ls_version = dr[0].ToString();
                            if (ls_version != ps_version)
                            {
                                UploadStatusLabel.Text = "Version no compatible. Descargue la version actualizada del menu Formatos/IDCC. No se puede continuar (1)";
                                f_set_tra_logpro_actualizar(li_hidrocod, 14, "Version no compatible", srv_idcc);
                                pb_valida = false;
                                return new DateTime(2000, 1, 1);
                            }
                        }
                        else
                        {
                            UploadStatusLabel.Text = "Version no compatible. Descargue la version actualizada del menu Formatos/IDCC. No se puede continuar (2)";
                            f_set_tra_logpro_actualizar(li_hidrocod, 14, "Version no compatible", srv_idcc);
                            pb_valida = false;
                            return new DateTime(2000, 1, 1);
                        }

                        #endregion


                        //tipoinfocodi                        
                        for (int li_j = 0; li_j < li_col; li_j++)
                        {


                            if (dr[li_j] != DBNull.Value)
                            {
                                arr_cod2[li_j] = dr[li_j].ToString();
                            }
                            else
                            {
                                arr_cod2[li_j] = "-1";
                            }

                            ls_cad += arr_cod2[li_j] + " , ";

                        }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue;

                    }


                    //fecha
                    if(li_xlsfila==7)
                    {                        
                        ldt_fh = EPDate.ToDateMMDDYYYY(dr[1].ToString());

                        if (ldt_fh != pdt_fhfile)
                        {
                            UploadStatusLabel.Text = "Fecha de nombre de archivo difiere de Fecha en hoja Excel. No se puede continuar";
                            f_set_tra_logpro_actualizar(li_hidrocod, 15, "Fecha de nombre de archivo difiere de Fecha en hoja Excel", srv_idcc);
                            pb_valida = false;
                            return new DateTime(2000, 1, 1);
                        }
                    }

                    if (li_xlsfila < 14)
                        continue;


                    if (li_xlsfila <= 61)
                    {
                        
                        for (int li_j = 0; li_j < li_col; li_j++)
                        {
                            
                            ls_valores[li_filabd,li_j] = dr[li_j].ToString();
                            ls_cad += ls_valores[li_filabd, li_j] + " , ";
                        }

                        li_filabd++;
                    }
                    else
                        break;


                    //ListBox1.Items.Add(ls_cad);
                    //UploadStatusLabel.Text += ls_cad + "\r\n";

                    continue;



                }

                #endregion


                #region crea registro
                                


                //UploadStatusLabel.Text = "";
                double ld_suma;
                double ld_valor;
                int li_error;
                int li_filreg = 0;
                

                for (int li_i = 1; li_i < li_col; li_i++)
                {
                    ls_cad = "";
                    ld_suma = 0;
                    li_error = 0;

                    if (arr_cod1[li_i].ToString() == "-1")
                        continue;

                    //arr_cod2[li_i]

                    ls_cad += ps_lectcodi + "," + ldt_fh.ToString("yyyy-MM-dd") + "," + arr_cod2[li_i] + "," + arr_cod1[li_i] + ",";

                    ls_sql_del = "delete from " + ps_tabla_gen + " where LECTCODI=" + ps_lectcodi + " and MEDIFECHA=" + EPDate.SQLDateOracleString(ldt_fh) + " and TIPOINFOCODI=" + arr_cod2[li_i] + " and PTOMEDICODI=" + arr_cod1[li_i];
                    ls_sql_ins = "insert into "+ps_tabla_gen +"(";
                    

                    ls_sql_campos = "LECTCODI,MEDIFECHA,TIPOINFOCODI,PTOMEDICODI";
                    ls_sql_valores = ps_lectcodi + "," + EPDate.SQLDateOracleString(ldt_fh) + "," + arr_cod2[li_i] + "," + arr_cod1[li_i];
                    

                    for (int li_j = 0; li_j < 48; li_j++)
                    {
                        try
                        {
                            ls_sql_campos += "," + "H" + (li_j + 1).ToString();

                            ld_valor = Convert.ToDouble(ls_valores[li_j, li_i].ToString());
                            ls_cad += ls_valores[li_j, li_i].ToString() + ",";
                            ld_suma += ld_valor;

                            if (ld_valor == 0)
                            {
                                li_error++;
                            }
                            
                            ls_sql_valores += "," + ld_valor.ToString();
                        }
                        catch
                        {
                            ls_cad += "null,";
                            ls_sql_valores += "," + "null";
                            li_error++;
                        }
                    }

                    ls_sql_ins += ls_sql_campos + ",MEDITOTAL)" + " values (" + ls_sql_valores + "," + ld_suma + ")";

                    if (li_error < 48)
                    {
                        srv_idcc.f_set_consulta1(ls_sql_del);

                        if (srv_idcc.f_set_consulta1(ls_sql_ins) < 0)
                            ls_colerror += li_i + " ";
                        else
                            li_filreg++;

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";
                    }




                }

                if (ls_colerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).", srv_idcc);
                    
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " Columnas: " + ls_colerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " Columnas: " + ls_colerror, srv_idcc);
                    pb_valida = false;
                }
                

                #endregion

            }

            return ldt_fh;
        }



        private void f_set_ax1diario(string ps_hoja,int pi_eaicodi, string ps_path, string ps_tabla_hid, int pi_lectcodi, ExtService srv_ext, ref bool pb_valida, string ps_lastuser, DateTime pdt_fh, int pi_salto, bool lb_pdiario)
        {
            //diario: hasta las 9 permite cargar y calcula indicador
            //diario: hasta las 12 carga pero estado=rechazado

            //diario: hasta las 14 permite cargar y calcula indicador. dia lunes reporte
            //diario: hasta las 24 carga pero estado=rechazado. dia lunes reporte


            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja, srv_ext);

            //Util.Alert.Show("pdt_fh" + pdt_fh.ToString("yyyy-MM-dd hh:mm:ss") + " hora actual" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            //if ((DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " " + ps_hora2).AddDays(1)) || (DateTime.Now.ToString("yyyy-MM-dd") != pdt_fh.AddDays(1).ToString("yyyy-MM-dd")))
            //if ((lb_pdiario && ((DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 12:00:00").AddDays(1)) || (DateTime.Now.ToString("yyyy-MM-dd") != pdt_fh.AddDays(1).ToString("yyyy-MM-dd"))))
            /*Temporalmente para pruebas*/
            if ((lb_pdiario && (DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 12:00:00")))
                || (!lb_pdiario && ((DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 14:00:00").AddDays(1)) || (DateTime.Now.ToString("yyyy-MM-dd") != pdt_fh.AddDays(1).ToString("yyyy-MM-dd")))))
            if (
                (lb_pdiario && ((DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 12:00:00"))))
                ||
                (!lb_pdiario && (pdt_fh.DayOfWeek != DayOfWeek.Monday || (DateTime.Now > Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 23:59:59"))))
                )
            {
                ListBox1.Items.Add("Hoja " + ps_hoja + ": Envio fuera de plazo");
                f_set_tra_logpro_actualizar(li_hidrocod, 16, "Error en hoja " + ps_hoja + ". Envio fuera de plazo. No se procesara", srv_ext);
                pb_valida = false;
                return;
            }


            //if (Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " " + ps_hora1).AddDays(1) < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " " + ps_hora2).AddDays(1))
            //if ((lb_pdiario &&(Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 09:00:00").AddDays(1) < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 12:00:00").AddDays(1)))
            /*Temporalmente para pruebas*/
            if (
                (lb_pdiario && (Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 09:00:00") < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 12:00:00")))
                ||
                (!lb_pdiario && (pdt_fh.DayOfWeek == DayOfWeek.Monday && (Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 14:00:00") < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fh.ToString("yyyy-MM-dd") + " 23:59:59"))))
                )
            {
                ListBox1.Items.Add("Hoja " + ps_hoja + ": Envio fuera de plazo");
                f_set_tra_logpro_actualizar(li_hidrocod, 16, "Error en hoja " + ps_hoja + ". Se cargaran dato a la base de datos", srv_ext);
                pb_valida = false;
                //return;
            }

            
            //ExtService srv_ext = new ExtService();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DataSet output = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlscol = 2;

                DateTime ldt_fh = new DateTime(2000, 1, 1);

                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql;
                int li_tipoinfocodi = 11;// m3/s

                //codigos
                DataRow drc = outputTable.Rows[0];
                //fecha                
                DataRow drf = outputTable.Rows[9];
                //valores
                DataRow drv = outputTable.Rows[10];

                int li_puntosprog = 0;
                int li_puntosejec = 0;
                string ls_filerror = "";
                int li_numreg;


                while (Convert.ToInt32( drc.ItemArray.GetLength(0).ToString() )> li_xlscol && (drc[li_xlscol] != DBNull.Value) && (drc[li_xlscol].ToString() != ""))
                {
                    string ls_ptomedicodi = drc[li_xlscol].ToString();
                    li_puntosprog += pi_salto;

                    //for (int li_i = li_xlscol; li_i <= li_xlscol + 2; li_i++)
                    for (int li_i = li_xlscol; li_i < li_xlscol + (pi_salto); li_i++)
                    {



                        if ((drf[li_i] != DBNull.Value) && (drf[li_i].ToString() != ""))
                        {
                            //WSIC2010.Util.Alert.Show(drf[li_i].ToString());
                            DateTime ls_fecha = new DateTime(2013, 1,1);

                            try
                            {
                                ls_fecha = EPDate.ToDateMMDDYYYY(drf[li_i].ToString());
                            }
                            catch (Exception)
                            {
                                WSIC2010.Util.Alert.Show(drf[li_i].ToString());
                            }


                            //ls_sql_del

                            ls_sql = "select count(*) as cuenta from me_ptomedicion where ptomedicodi=" + ls_ptomedicodi + " and emprcodi in (-999," + in_app.is_Empresas + ")";
                            //li_numreg= srv_idcc.f_set_consulta1(ls_sql);                            
                            li_numreg = srv_ext.f_get_consulta_escalar(ls_sql);
                            

                            if (li_numreg > 0)
                            {
                                ls_sql_del = "delete from " + ps_tabla_hid + " where lectcodi=" + pi_lectcodi + " and medifecha=" + EPDate.SQLDateOracleString(ls_fecha) + " and tipoinfocodi=" + li_tipoinfocodi + " and ptomedicodi=" + ls_ptomedicodi;
                                srv_ext.f_set_consulta1(ls_sql_del);
                                //UploadStatusLabel.Text += ls_sql_del + "\r\n";
                                //ListBox1.Items.Add(ls_sql_del);

                                if ((drv[li_i] != DBNull.Value) && (drv[li_i].ToString() != "") && (ls_fecha.Year > 2005))
                                {

                                    string ls_valor = drv[li_i].ToString();
                                    double ld_valor;
                                    try
                                    {
                                        ld_valor = Convert.ToDouble(ls_valor);
                                    }
                                    catch
                                    {
                                        ld_valor = -1;
                                    }

                                    if (ld_valor >= 0)
                                    {
                                        ls_sql_ins = "insert into " + ps_tabla_hid;
                                        ls_sql_ins += "(LECTCODI,MEDIFECHA,TIPOINFOCODI,PTOMEDICODI,H1,LASTUSER,LASTDATE) values ";
                                        ls_sql_ins += "(" + pi_lectcodi + "," + EPDate.SQLDateOracleString(ls_fecha) + "," + li_tipoinfocodi + "," + ls_ptomedicodi + "," + ls_valor + ", '" + ps_lastuser + "',sysdate)";
                                        //UploadStatusLabel.Text += ls_sql_ins + "\r\n";
                                        //ListBox1.Items.Add(ls_sql_ins);

                                        if (srv_ext.f_set_consulta1(ls_sql_ins) < 0)
                                            ls_filerror += (li_i + 1) + ", ";
                                        else
                                            li_puntosejec++;
                                    }
                                    else
                                    {
                                        ls_filerror += (li_i + 1) + ", ";
                                    }



                                }

                            }
                            else
                            {
                                ls_filerror += (li_i + 1) + "(EMP), ";
                            }
                            //srv_idcc.f_set_consulta1(ls_sql_ins);

                        }
                    }

                    li_xlscol += pi_salto;
                }


                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";

                //string ls_equicodi_old = "";
                //string ls_filerror = "";
                //int li_filreg = 0;

                double ld_porc = 0;

                if (li_puntosprog != 0)
                {
                    ld_porc = (100.0 * li_puntosejec) / (li_puntosprog * 1.0);
                }


                if (ls_filerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": OK. (" + li_puntosejec + " registros creados). (" + Math.Round(ld_porc, 2) + " %)");
                    srv_ext.f_set_ext_ratio_actualizar(li_hidrocod, pi_eaicodi, li_puntosprog, li_puntosejec);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + ": OK. (" + li_puntosejec + " registros creados).", srv_ext);
                }
                else
                {
                    if (ls_filerror.Length > 200)
                    {
                        ls_filerror = ls_filerror.Substring(0, 200);
                    }
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + ". Columnas: " + ls_filerror);
                    srv_ext.f_set_ext_ratio_actualizar(li_hidrocod, pi_eaicodi, li_puntosprog, li_puntosejec);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " Columnas: " + ls_filerror, srv_ext);
                    pb_valida = false;
                }




                #endregion

            }

        }

        private void f_set_ax2embalse(string ps_hoja, int pi_eaicodi, string ps_path, string ps_tabla_gen, int pi_lectcodi, DateTime pdt_fhfile, ExtService srv_ext, ref bool pb_valida, string ps_lastuser, bool pb_anexo2, int pi_limite1, int pi_limite2)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja, srv_ext);

            //if (DateTime.Now.ToString("yyyy-MM-dd") != pdt_fhfile.AddDays(1).ToString("yyyy-MM-dd") || (DateTime.Now > Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 08:00:00").AddDays(1)))
            //if (DateTime.Now.ToString("yyyy-MM-dd") != pdt_fhfile.AddDays(1).ToString("yyyy-MM-dd") || (DateTime.Now > Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 08:00:00").AddDays(1)))
            //Para pruebas
            //Util.Alert.Show(DateTime.Now.ToString("yyyy-MM-dd") + "<br>" + pdt_fhfile.ToString("yyyy-MM-dd") + "<br>" + EPDate.ToDate(pdt_fhfile.ToString("yyyy-MM-dd") + " 18:00:00").AddDays(1).ToString("yyyy-MM-dd"));
            if (DateTime.Now.ToString("yyyy-MM-dd") != Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd")).AddDays(1).ToString("yyyy-MM-dd") || (DateTime.Now > Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 18:00:00").AddDays(1)))
            {
                ListBox1.Items.Add("Hoja " + ps_hoja + ": Envio fuera de plazo");
                f_set_tra_logpro_actualizar(li_hidrocod, 16, "Error en hoja " + ps_hoja + ". Envio fuera de plazo. No se procesara", srv_ext);
                pb_valida = false;
                return;
            }

            //Para pruebas
            if (Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 07:00:00").AddDays(1) < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 08:00:00").AddDays(1))
                if (Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 17:00:00").AddDays(1) < DateTime.Now && DateTime.Now <= Convert.ToDateTime(pdt_fhfile.ToString("yyyy-MM-dd") + " 18:00:00").AddDays(1))
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": Envio fuera de plazo pero se cargaran los datos a la base de datos");
                    f_set_tra_logpro_actualizar(li_hidrocod, 16, "Error plazo en hora " + ps_hoja + ". Se cargaran dato a la base de datos", srv_ext);
                    pb_valida = false;
                    //return;
                }
            


            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DateTime ldt_fh = pdt_fhfile;// new DateTime(2000, 1, 1);

            DataSet output = new DataSet();
            //WScoes.ManttoServiceClient Mservicio = new WScoes.ManttoServiceClient();
            //ManttoService servicio = new ManttoService();

            //UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;
                int li_col = 0;
                string ls_cad = "";

                string[] arr_cod1 = new string[256];
                string[] arr_cod2 = new string[256];

                string[] ls_campo = new string[256];
                string[,] ls_valores = new string[24, 256];

                int li_filabd = 0;


                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;
                string ls_version;

                string ls_colerror = "";

                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";

                    if (li_xlsfila == 1)
                    {


                        //codigos
                        li_col = 0;

                        try
                        {

                            while ((dr[li_col] != DBNull.Value) && (dr[li_col].ToString() != ""))
                            {

                                arr_cod1[li_col] = dr[li_col].ToString();
                                ls_cad += dr[li_col].ToString() + " , ";

                                //msnm: 40
                                //Hm3: 14
                                //m3/s: 11
                                //1 2 3 4 5
                                //6 7 8 9 10

                                //if (li_col%5)
                                    //arr_cod2

                                //TIPOINFOCODI

                                if (pb_anexo2)
                                {
                                    switch (li_col % 5)
                                    {
                                        case 0:
                                            arr_cod2[li_col + 1] = "40"; //msnm
                                            break;
                                        case 1:
                                            arr_cod2[li_col + 1] = "14"; //Hm3
                                            break;
                                        case 2:
                                            arr_cod2[li_col + 1] = "11"; //m3/s
                                            break;
                                        case 3:
                                            arr_cod2[li_col + 1] = "11"; //m3/s
                                            break;
                                        case 4:
                                            arr_cod2[li_col + 1] = "11"; //m3/s
                                            break;

                                    }
                                }
                                else
                                {
                                    arr_cod2[li_col + 1] = "11"; //m3/s
                                }


                                li_col++;
                            }
                        }
                        catch { }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue;


                    }

                    //if (li_xlsfila < 10)
                    if (li_xlsfila < pi_limite1)
                        continue;

                    //if (li_xlsfila > 33)
                    if (li_xlsfila > pi_limite2)
                        break;

                    //cargado de datos a matriz
                    //if (li_xlsfila <= 33)
                    if (li_xlsfila <= pi_limite2)
                    {

                        for (int li_j = 0; li_j < li_col; li_j++)
                        {
                            try
                            {
                                ls_valores[li_filabd, li_j] = dr[li_j].ToString();
                            }
                            catch
                            {
                                ls_valores[li_filabd, li_j] = null;
                            }

                            try
                            {
                                ls_cad += ls_valores[li_filabd, li_j] + " , ";
                            }
                            catch
                            {
                                ls_cad += "null" + " , ";
                            }
                        }

                        li_filabd++;
                    }
                    else
                        break;


                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";
                double ld_suma;
                double ld_valor;
                int li_error;
                int li_filreg = 0;
                int li_numreg;
                string ls_sql;


                for (int li_i = 1; li_i < li_col; li_i++)
                {
                    ls_cad = "";
                    ld_suma = 0;
                    li_error = 0;

                    if (arr_cod1[li_i].ToString() == "-1")
                        continue;

                    //arr_cod2[li_i]

                    ls_sql = "select count(*) as cuenta from me_ptomedicion where ptomedicodi=" + arr_cod1[li_i] + " and emprcodi in (-999," + in_app.is_Empresas + ")";

                    li_numreg = srv_ext.f_get_consulta_escalar(ls_sql);                    

                    if (li_numreg > 0)
                    {

                        ls_cad += pi_lectcodi + "," + ldt_fh.ToString("yyyy-MM-dd") + "," + arr_cod2[li_i] + "," + arr_cod1[li_i] + ",";

                        ls_sql_del = "delete from " + ps_tabla_gen + " where LECTCODI=" + pi_lectcodi + " and MEDIFECHA=" + EPDate.SQLDateOracleString(ldt_fh) + " and TIPOINFOCODI=" + arr_cod2[li_i] + " and PTOMEDICODI=" + arr_cod1[li_i];
                        ls_sql_ins = "insert into " + ps_tabla_gen + "(";


                        ls_sql_campos = "LECTCODI,MEDIFECHA,TIPOINFOCODI,PTOMEDICODI";
                        ls_sql_valores = pi_lectcodi + "," + EPDate.SQLDateOracleString(ldt_fh) + "," + arr_cod2[li_i] + "," + arr_cod1[li_i];


                        for (int li_j = 0; li_j < 24; li_j++)
                        {
                            try
                            {
                                ls_sql_campos += "," + "H" + (li_j + 1).ToString();

                                ld_valor = Convert.ToDouble(ls_valores[li_j, li_i].ToString());
                                ls_cad += ls_valores[li_j, li_i].ToString() + ",";
                                ld_suma += ld_valor;

                                if (ld_valor == 0)
                                {
                                    li_error++;
                                }
                                else
                                {
                                    if (ld_valor < 0)
                                        ls_colerror += li_i + " ";
                                }

                                ls_sql_valores += "," + ld_valor.ToString();
                            }
                            catch
                            {
                                ls_cad += "null,";
                                ls_sql_valores += "," + "null";
                                li_error++;
                            }
                        }

                        //ls_sql_ins += ls_sql_campos + ",MEDITOTAL)" + " values (" + ls_sql_valores + "," + ld_suma/24.0 + ")";
                        ls_sql_ins += ls_sql_campos + ",LASTUSER,LASTDATE)" + " values (" + ls_sql_valores + ",'" + ps_lastuser + "',SYSDATE)";

                        if (li_error < 24)
                        {

                            srv_ext.f_set_consulta1(ls_sql_del);

                            if (srv_ext.f_set_consulta1(ls_sql_ins) < 0)
                                ls_colerror += li_i + " ";
                            else
                                li_filreg++;

                            //ListBox1.Items.Add(ls_cad);
                            //ListBox1.Items.Add(ls_sql_del);
                            //ListBox1.Items.Add(ls_sql_ins);
                            //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";
                        }


                    }
                    else
                    {
                        ls_colerror += (li_i + 1) + "(EMP), ";
                    }

                }
                //777
                if (ls_colerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).", srv_ext);

                    if (li_filreg > 0)
                    {
                        srv_ext.f_set_ext_ratio_actualizar(li_hidrocod, pi_eaicodi, 1, 1);
                    }
                    else
                    {
                        srv_ext.f_set_ext_ratio_actualizar(li_hidrocod, pi_eaicodi, 0, 1);
                        pb_valida = false;
                    }

                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " Columnas: " + ls_colerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " Columnas: " + ls_colerror, srv_ext);
                    srv_ext.f_set_ext_ratio_actualizar(li_hidrocod, pi_eaicodi, 0, 1);
                    pb_valida = false;
                }


                #endregion

            }

            return;// ldt_fh;
        }



        private void f_set_combustible_consumo(string ps_hoja, string ps_path, string ps_tabla_ccomb, string ps_evenclasecodi, DateTime pdt_fh, ExtService srv_idcc, ref bool pb_valida)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja+" (consumo)", srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DataSet output = new DataSet();

            UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;

                string ls_cad = "";

                string[] arr_cod1 = new string[256]; //equicodi
                string[] arr_cod2 = new string[256]; //combcodi
                string[] arr_cod3 = new string[256]; //tipoinfocodi
                int[] arr_filax = new int[256]; 


                string[] ls_campo = new string[100];

                double[] ld_consumo = new double[100]; //entrada de servicio
                

                int li_filabd = -1;
                
                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;


                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";


                    if (li_xlsfila < 13)
                        continue;

                    if ((dr[0] != DBNull.Value) && (dr[0].ToString() != ""))
                    {
                        if ((dr[1] != DBNull.Value) && (dr[1].ToString() != ""))
                            if ((dr[2] != DBNull.Value) && (dr[2].ToString() != ""))
                                if ((dr[6] != DBNull.Value) && (dr[6].ToString() != ""))
                                {
                                    try
                                    {
                                        if (Convert.ToDouble(dr[6].ToString()) == 0)
                                            continue;
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                    li_filabd++;
                                    arr_cod1[li_filabd] = dr[0].ToString();
                                    arr_cod2[li_filabd] = dr[1].ToString();
                                    arr_cod3[li_filabd] = dr[2].ToString();
                                    arr_filax[li_filabd] = li_xlsfila;
                                    try
                                    {
                                        ld_consumo[li_filabd] = Convert.ToDouble(dr[6].ToString());
                                        ls_cad = arr_cod1[li_filabd] + " " + arr_cod2[li_filabd] + " " + arr_cod3[li_filabd] + ld_consumo[li_filabd];                                       
                                    }
                                    catch
                                    {
                                        li_filabd--;
                                    }
                                }

                        


                    }
                    else
                    {
                        break;
                    }

                                           
                        
                        
                    
                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";

                string ls_equicodi_old = "";
                string ls_filerror = "";
                int li_filreg = 0;

                for (int li_i = 0; li_i <= li_filabd; li_i++)
                {
                    ls_cad = "";


                    if (ld_consumo[li_i] > 0)
                    {

                        ls_sql_del = "delete from " + ps_tabla_ccomb + " where EVENCLASECODI=" + ps_evenclasecodi + " and MEDIFECHA=" + EPDate.SQLDateOracleString(pdt_fh) + " and GRUPOCODI=" + arr_cod1[li_i] + " and COMBCODI=" + arr_cod2[li_i] + " and TIPOINFOCODI=" + arr_cod3[li_i];
                        srv_idcc.f_set_consulta1(ls_sql_del);

                        ls_sql_ins = "insert into " + ps_tabla_ccomb;

                        ls_sql_campos = "(EVENCLASECODI,MEDIFECHA,GRUPOCODI,COMBCODI,TIPOINFOCODI,CCVCONSUMO)";

                        ls_sql_valores = ps_evenclasecodi + "," + EPDate.SQLDateOracleString(pdt_fh) + "," + arr_cod1[li_i] + "," + arr_cod2[li_i] + "," + arr_cod3[li_i] + "," + ld_consumo[li_i];

                        ls_sql_ins += ls_sql_campos + " values (" + ls_sql_valores + ")";
                        ls_equicodi_old = arr_cod2[li_i];
                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";

                        if (srv_idcc.f_set_consulta1(ls_sql_ins) < 0)
                            ls_filerror += arr_filax[li_i] + " ";
                        else
                            li_filreg++;

                    }
                    else
                    {
                        ListBox1.Items.Add("Hoja " + ps_hoja + "(Consumo): Valor negativo o cero en fila " + arr_filax[li_i] + ".");
                        ls_filerror += arr_filax[li_i] + " ";
                    }

                }

                if (ls_filerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + " (Consumo): OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + "(Consumo): OK. (" + li_filreg + " registros creados).", srv_idcc);
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " (Consumo). Filas: " + ls_filerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + "(Consumo). Filas: " + ls_filerror, srv_idcc);
                    pb_valida = false;
                }
            


                #endregion

            }

        }


        private void f_set_combustible_stock(string ps_hoja, string ps_path, string ps_tabla_cstock, string ps_evenclasecodi, DateTime pdt_fh, ExtService srv_idcc, ref bool pb_valida)
        {

            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja+" (stock)", srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DataSet output = new DataSet();

            UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;

                string ls_cad = "";

                string[] arr_cod1 = new string[256]; //equicodi
                string[] arr_cod2 = new string[256]; //combcodi
                string[] arr_cod3 = new string[256]; //tipoinfocodi
                int[] arr_filax = new int[256]; //columna excel
                int li_filax_old = -1;


                string[] ls_campo = new string[100];

                double[] ld_inicio = new double[100]; //inicio
                double[] ld_fin = new double[100]; //fin
                string[] ls_recepcion = new string[100]; //recepcion
                string[] ls_consumo = new string[100]; //consumo


                int li_filabd = -1;

                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;

                string ls_filerror = "";

                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";


                    if (li_xlsfila < 14)
                        continue;

                    

                    if ((dr[10] != DBNull.Value) && (dr[10].ToString() != ""))
                    {
                        if ((dr[11] != DBNull.Value) && (dr[11].ToString() != ""))
                            if ((dr[12] != DBNull.Value) && (dr[12].ToString() != ""))
                            {
                                if (((dr[15] != DBNull.Value) && (dr[15].ToString() != "")) && ((dr[16] != DBNull.Value) && (dr[16].ToString() != "")))
                                {
                                    try
                                    {
                                        if (Convert.ToDouble(dr[15].ToString()) == 0)
                                            continue;
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                    li_filabd++;
                                    arr_cod1[li_filabd] = dr[10].ToString();
                                    arr_cod2[li_filabd] = dr[11].ToString();
                                    arr_cod3[li_filabd] = dr[12].ToString();

                                    try
                                    {
                                        ld_inicio[li_filabd] = Convert.ToDouble(dr[15].ToString());
                                        ld_fin[li_filabd] = Convert.ToDouble(dr[16].ToString());

                                        if (ld_inicio[li_filabd] < 0)
                                        {
                                            if (li_xlsfila != li_filax_old)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): Valor negativo o cero en inicio (Fila" + li_xlsfila + ").");
                                                ls_filerror += li_xlsfila + " ";
                                                li_filabd--;
                                                continue;
                                            }
                                        }

                                        if (ld_fin[li_filabd] < 0)
                                        {
                                            if (li_xlsfila != li_filax_old)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): Valor negativo o cero en final (Fila" + li_xlsfila + ").");
                                                ls_filerror += li_xlsfila + " ";
                                                li_filabd--;
                                                continue;
                                            }
                                        }

                                    }
                                    catch
                                    {
                                        li_filabd--;
                                        continue;
                                    }

                                    try
                                    {
                                        ls_recepcion[li_filabd] = Convert.ToDouble(dr[17].ToString()).ToString();

                                        if (Convert.ToDouble(ls_recepcion[li_filabd]) < 0)
                                        {
                                            if (li_xlsfila != li_filax_old)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): Valor negativo o cero en recepcion (Fila " + li_xlsfila + ").");
                                                ls_filerror += li_xlsfila + " ";
                                                li_filabd--;
                                                continue;
                                            }
                                        }

                                    }
                                    catch
                                    {
                                        ls_recepcion[li_filabd] = "null";
                                    }

                                    try
                                    {
                                        ls_consumo[li_filabd] = Convert.ToDouble(dr[18].ToString()).ToString();

                                        if (Convert.ToDouble(ls_consumo[li_filabd]) < 0)
                                        {
                                            if (li_xlsfila != li_filax_old)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): Valor negativo o cero en consumo (Fila" + li_xlsfila + ").");
                                                ls_filerror += li_xlsfila + " ";
                                                li_filabd--;
                                                continue;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        ls_consumo[li_filabd] = "null";
                                    }


                                    //ls_cad = arr_cod1[li_filabd] + " " + arr_cod2[li_filabd] + " " + arr_cod3[li_filabd] + " " + ld_inicio[li_filabd] + " " + ld_fin[li_filabd] + " " + ls_recepcion[li_filabd] + " " + ls_consumo[li_filabd];

                                }
                                else
                                {
                                    if ((((dr[15] == DBNull.Value) || (dr[15].ToString() == "")) && ((dr[16] == DBNull.Value) && (dr[16].ToString() == ""))))
                                    {
                                    }
                                    else
                                    {
                                        ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): Dato requerido inicio y fin (Fila" + li_xlsfila + ").");
                                        ls_filerror += li_xlsfila + " ";
                                        continue;
                                    }
                                }
                            }
                            




                    }
                    else
                    {
                        break;
                    }


                    li_filax_old = li_xlsfila;

                    
                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";
                
                                
                int li_filreg = 0;

                for (int li_i = 0; li_i <= li_filabd; li_i++)
                {
                    ls_cad = "";

                    ls_sql_del = "delete from " + ps_tabla_cstock + " where EVENCLASECODI=" + ps_evenclasecodi + " and MEDIFECHA=" + EPDate.SQLDateOracleString(pdt_fh) + " and GRUPOCODI=" + arr_cod1[li_i] + " and COMBCODI=" + arr_cod2[li_i] + " and TIPOINFOCODI=" + arr_cod3[li_i];
                    srv_idcc.f_set_consulta1(ls_sql_del);

                    ls_sql_ins = "insert into " + ps_tabla_cstock;

                    ls_sql_campos = "(EVENCLASECODI,MEDIFECHA,GRUPOCODI,COMBCODI,TIPOINFOCODI,CCVINICIO,CCVFIN,CCVRECEPC,CCVCONSUMO)";

                    ls_sql_valores = ps_evenclasecodi + "," + EPDate.SQLDateOracleString(pdt_fh) + "," + arr_cod1[li_i] + "," + arr_cod2[li_i] + "," + arr_cod3[li_i] + "," + ld_inicio[li_i] + "," + ld_fin[li_i] + "," + ls_recepcion[li_i] + "," + ls_consumo[li_i];

                    ls_sql_ins += ls_sql_campos + " values (" + ls_sql_valores + ")";

                    if (srv_idcc.f_set_consulta1(ls_sql_ins) < 0)
                        ls_filerror += (14+li_i) + " ";
                    else
                        li_filreg++;

                    //ls_equicodi_old = arr_cod2[li_i];
                    //ListBox1.Items.Add(ls_cad);
                    //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";


                }

                if (ls_filerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + "(Stock): OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + "(Stock): OK. (" + li_filreg + " registros creados).", srv_idcc);
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " (Stock). Filas: " + ls_filerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " (Stock). Filas: " + ls_filerror, srv_idcc);
                    pb_valida = false;
                }


                #endregion

            }

        }

        private void f_set_hidrologia_reporte(string ps_hoja, string ps_path, string ps_tabla_gen, string ps_lectcodi, DateTime pdt_fh, ExtService srv_idcc, ref bool pb_valida)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja+" (reporte)", srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            //DateTime ldt_fh = new DateTime(2000, 1, 1);


            DataSet output = new DataSet();

            UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;
                int li_col = 0;
                string ls_cad = "";

                string[] arr_cod1 = new string[256];
                string[] arr_cod2 = new string[256];

                string[] ls_campo = new string[100];
                string[,] ls_valores = new string[48, 100];

                int li_filabd = 0;


                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;

                string ls_colerror = "";


                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";

                    if (li_xlsfila == 1)
                    {
                        //codigos
                        li_col = 2;

                        try
                        {

                            while ((dr[li_col] != DBNull.Value) && (dr[li_col].ToString() != ""))
                            {
                                
                                arr_cod1[li_col-2] = dr[li_col].ToString();
                                ls_cad += dr[li_col].ToString() + " , ";
                                li_col++;                                
                            }
                        }
                        catch { }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue;


                    }


                    if (li_xlsfila == 2)
                    {
                        //tipoinfocodi                        
                        for (int li_j = 2; li_j < li_col; li_j++)
                        {


                            if (dr[li_j] != DBNull.Value)
                            {
                                arr_cod2[li_j-2] = dr[li_j].ToString();
                            }
                            else
                            {
                                arr_cod2[li_j] = "-1";
                            }

                            ls_cad += arr_cod2[li_j] + " , ";

                        }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue;

                    }
                    

                    if (li_xlsfila < 12)
                        continue;


                    if (li_xlsfila <= 59)
                    {

                        for (int li_j = 2; li_j < li_col; li_j++)
                        {
                            try
                            {
                                ls_valores[li_filabd, li_j - 2] = Convert.ToDouble(dr[li_j].ToString()).ToString();

                                if (Convert.ToDouble(ls_valores[li_filabd, li_j - 2]) < 0)
                                {
                                    li_filabd--;
                                    ls_colerror += (li_j + 1) + " ";
                                    break;

                                }
                            }
                            catch
                            {
                                ls_valores[li_filabd, li_j - 2] = "null";
                            }
                                
                            ls_cad += ls_valores[li_filabd, li_j-2] + " , ";
                        }

                        li_filabd++;
                    }
                    else
                        break;


                    //ListBox1.Items.Add(ls_cad);
                    //UploadStatusLabel.Text += ls_cad + "\r\n";

                    continue;



                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";
                double ld_suma;
                double ld_valor;
                int li_error;
                
                int li_filreg = 0;

                for (int li_i = 2; li_i < li_col; li_i++)
                {
                    ls_cad = "";
                    ld_suma = 0; 
                    li_error = 0;

                    //if (arr_cod1[li_i-2].ToString() == "-1")
                    //    continue;

                    //arr_cod2[li_i]

                    //ls_cad += ps_lectcodi + "," + pdt_fh.ToString("yyyy-MM-dd") + "," + arr_cod2[li_i-2] + "," + arr_cod1[li_i-2] + ",";

                    ls_sql_del = "delete from " + ps_tabla_gen + " where LECTCODI=" + ps_lectcodi + " and MEDIFECHA=" + EPDate.SQLDateOracleString(pdt_fh) + " and TIPOINFOCODI=" + arr_cod2[li_i-2] + " and PTOMEDICODI=" + arr_cod1[li_i-2];
                    ls_sql_ins = "insert into " + ps_tabla_gen + "(";

                    ls_sql_campos = "LECTCODI,MEDIFECHA,TIPOINFOCODI,PTOMEDICODI";
                    ls_sql_valores = ps_lectcodi + "," + EPDate.SQLDateOracleString(pdt_fh) + "," + arr_cod2[li_i-2] + "," + arr_cod1[li_i-2];


                    for (int li_j = 0; li_j < 48; li_j++)
                    {
                        try
                        {
                            ls_sql_campos += "," + "H" + (li_j + 1).ToString();

                            ld_valor = Convert.ToDouble(ls_valores[li_j, li_i-2].ToString());
                            //ls_cad += ls_valores[li_j, li_i-2].ToString() + ",";
                            ld_suma += ld_valor;


                            ls_sql_valores += "," + ld_valor.ToString();
                        }
                        catch
                        {
                            //ls_cad += "null,";
                            ls_sql_valores += "," + "null";
                            li_error++;
                        }
                    }

                    //ingresa registro
                    if (li_error < 48)
                    {
                        srv_idcc.f_set_consulta1(ls_sql_del);
                        
                        ls_sql_ins += ls_sql_campos + ") values (" + ls_sql_valores + ")";
                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";
                        if (srv_idcc.f_set_consulta1(ls_sql_ins) < 0)
                            ls_colerror += (li_i+1) + " ";
                        else
                            li_filreg++;

                    }






                }


                if (ls_colerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + " (Reporte): OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + " (Reporte): OK. (" + li_filreg + " registros creados).", srv_idcc);
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " (Reporte).(Reporte) Columnas: " + ls_colerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " (Reporte). Columnas: " + ls_colerror, srv_idcc);
                    pb_valida = false;
                }



                #endregion

            }
                        
        }

        private void f_set_hidrologia_vertim(string ps_hoja, string ps_path, string ps_tabla_cstock, string ps_evenclasecodi, DateTime pdt_fh, ExtService srv_idcc, ref bool pb_valida, int pi_subcausacodi)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja+" (vertimiento)", srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

            DataSet output = new DataSet();

            UploadStatusLabel.Text = "";
            

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;

                string ls_cad = "";

                string[] arr_cod1 = new string[256]; //equicodi
                string[] arr_cod2 = new string[256]; //combcodi
                string[] arr_cod3 = new string[256]; //tipoinfocodi


                string[] ls_campo = new string[100];

                double[] ld_vertim = new double[100]; //inicio
                DateTime[] ldt_fhini = new DateTime[100]; //inicio
                DateTime[] ldt_fhfin = new DateTime[100]; //final

                double[] ld_fin = new double[100]; //fin
                string[] ls_recepcion = new string[100]; //recepcion
                string[] ls_consumo = new string[100]; //consumo
                int[] li_filax = new int[100]; //fila excel


                int li_filabd = -1;

                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;

                string ls_filerror = "";
                int li_filax_old = -1;

                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";


                    if (li_xlsfila < 12)
                        continue;

                    if ((dr[9] != DBNull.Value) && (dr[9].ToString() != ""))                    
                    {
                        for (int li_i = 1; li_i <= 2; li_i++)
                        {
                            if ((dr[8 + 3 * li_i] != DBNull.Value) && (dr[8 + 3 * li_i].ToString() != ""))
                                if ((dr[8 + 3 * li_i + 1] != DBNull.Value) && (dr[8 + 3 * li_i + 1].ToString() != ""))
                                    if ((dr[8 + 3 * li_i + 2] != DBNull.Value) && (dr[8 + 3 * li_i + 2].ToString() != ""))
                                    {
                                        try
                                        {
                                            if (Convert.ToDouble(dr[8 + 3 * li_i + 2].ToString()) <= 0)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Vertimiento): Valor negativo o cero en vertimiento (Fila" + li_xlsfila + "). Bloque:"+li_i);
                                                if (li_filax_old != li_xlsfila)
                                                {
                                                    ls_filerror += li_xlsfila + " ";
                                                    li_filax_old = li_xlsfila;
                                                }
                                                continue;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }

                                        li_filabd++;
                                        arr_cod1[li_filabd] = dr[9].ToString();
                                        li_filax[li_filabd] = li_xlsfila;

                                        try
                                        {
                                            ld_vertim[li_filabd] = Convert.ToDouble(dr[8 + 3 * li_i + 2].ToString());

                                            ldt_fhini[li_filabd] = EPDate.ToDate(pdt_fh.ToString("yyyy-MM-dd") + " " + dr[8 + 3 * li_i].ToString());

                                            if ((dr[8 + 3 * li_i + 1].ToString() == "24:00") || (dr[8 + 3 * li_i + 1].ToString() == "00:00"))
                                            {
                                                ldt_fhfin[li_filabd] = EPDate.ToDate(pdt_fh.AddDays(1).ToString("yyyy-MM-dd"));
                                            }
                                            else
                                                ldt_fhfin[li_filabd] = EPDate.ToDate(pdt_fh.ToString("yyyy-MM-dd") + " " + dr[8 + 3 * li_i + 1].ToString());

                                            if (ld_vertim[li_filabd] <= 0)
                                            {
                                                ListBox1.Items.Add("Hoja " + ps_hoja + "(Vertimiento): Valor negativo o cero en vertimiento (Fila" + li_xlsfila + "). Bloque:" + li_i);                                                
                                                li_filabd--;
                                                if (li_filax_old != li_xlsfila)
                                                {
                                                    ls_filerror += li_xlsfila + " ";
                                                }
                                                continue;

                                            }
                                        }
                                        catch
                                        {
                                            li_filabd--;
                                        }



                                        ls_cad = arr_cod1[li_filabd] + " " + ldt_fhini[li_filabd].ToString() + " " + ldt_fhfin[li_filabd].ToString() + " " + ld_vertim[li_filabd];
                                        li_filax_old = li_xlsfila;
                                    }
                        }



                    }
                    else
                    {
                        break;
                    }





                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";

                string ls_equicodi_old = "";
                int li_filreg = 0;
                li_filax_old = -1;

                for (int li_i = 0; li_i <= li_filabd; li_i++)
                {
                    ls_cad = "";

                    ls_sql_del = "";
                    if (ls_equicodi_old != arr_cod1[li_i])
                    {
                        ls_sql_del = "delete from " + ps_tabla_cstock + " where EVENCLASECODI=" + ps_evenclasecodi + " and ICHORINI>=TO_DATE('" + pdt_fh.ToString("yyyy-MM-dd") + " 00:00:00','yyyy-mm-dd hh24:mi:ss')" + " and ICHORINI<TO_DATE('" + pdt_fh.AddDays(1).ToString("yyyy-MM-dd") + "','yyyy-mm-dd')" + " and EQUICODI=" + arr_cod1[li_i] + " AND SUBCAUSACODI=" + pi_subcausacodi;
                        srv_idcc.f_set_consulta1(ls_sql_del);

                    }

                    ls_sql_ins = "insert into " + ps_tabla_cstock;

                    ls_sql_campos = "(EVENCLASECODI, EQUICODI, ICHORINI, ICHORFIN, ICVALOR1, lastuser, lastdate,SUBCAUSACODI,ICCODI) ";

                    ls_sql_valores = ps_evenclasecodi + "," + arr_cod1[li_i] + ",to_date('" + ldt_fhini[li_i].ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')" + ",to_date('" + ldt_fhfin[li_i].ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')" + "," + ld_vertim[li_i];

                    ls_sql_ins += ls_sql_campos + " values (" + ls_sql_valores;//+ ")";
                    ls_equicodi_old = arr_cod1[li_i];
                    //ListBox1.Items.Add(ls_cad);
                    //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";

                    if (srv_idcc.f_set_conslconfig("EVE_IEODCUADRO", ls_sql_ins + ",'" + in_app.is_UserLogin + "',sysdate," + pi_subcausacodi + ",", ")") < 0)
                    {
                        if(li_filax_old!=li_filax[li_i] )
                        ls_filerror += li_filax[li_i] + " ";
                    }
                    else
                        li_filreg++;


                    li_filax_old = li_filax[li_i];

                }

                if (ls_filerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + " (Vertimiento): OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + " (Vertimiento): OK. (" + li_filreg + " registros creados).", srv_idcc);
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " (Vertimiento). Filas: " + ls_filerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + " (Vertimiento). Filas: " + ls_filerror, srv_idcc);
                    pb_valida = false;
                }

                #endregion

            }

        }

        private void f_set_ivdf(string ps_hoja, string ps_path, string ps_tabla_gen, string ps_lectcodi, DateTime pdt_fh, ExtService srv_idcc, ref bool pb_valida)
        {
            f_set_tra_logpro_actualizar(li_hidrocod, 2, "Revision de hoja: " + ps_hoja, srv_idcc);

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ps_path + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;HDR=No" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";
            
            DataSet output = new DataSet();

            UploadStatusLabel.Text = "";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {

                #region lectura matriz
                conn.Open();


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + ps_hoja + "$]", conn);
                cmd.CommandType = CommandType.Text;

                DataTable outputTable = new DataTable(ps_hoja);

                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);

                int li_xlsfila = 0;
                int li_col = 0;
                string ls_cad = "";

                string[] arr_cod1 = new string[256];
                string[] arr_cod2 = new string[256];

                string[] ls_campo = new string[100];

                //DateTime[] ldt_fh1 = new DateTime[100];
                string[,] ls_valores = new string[48, 100];

                int li_filabd = 0;


                string ls_sql_del;
                string ls_sql_ins;
                string ls_sql_campos;
                string ls_sql_valores;

                //DateTime ldt_fh = pdt_fh;

                foreach (DataRow dr in outputTable.Rows)
                {

                    li_xlsfila++;
                    ls_cad = "";

                    if (li_xlsfila == 1)
                    {
                        //codigos
                        li_col = 1;

                        try
                        {

                            while ((dr[li_col] != DBNull.Value) && (dr[li_col].ToString() != ""))
                            {
                                arr_cod1[li_col - 1] = dr[li_col].ToString();
                                ls_cad += dr[li_col].ToString() + " , ";
                                li_col++;
                            }
                        }
                        catch { }

                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_cad + "\r\n";
                        continue;


                    }


                    if (li_xlsfila < 12)
                        continue;


                    if (li_xlsfila <= 15)
                    {

                        for (int li_j = 1; li_j < li_col; li_j++)
                        {
                            //ldt_fh1[li_filabd] = ldt_fh;
                            try
                            {                                
                                ls_valores[li_filabd, li_j - 1] = Convert.ToDouble(dr[li_j].ToString()).ToString();
                            }
                            catch
                            {
                                ls_valores[li_filabd, li_j - 1] = "null";
                            }

                            ls_cad += ls_valores[li_filabd, li_j - 1] + " , ";
                        }

                        li_filabd++;
                        //ldt_fh=ldt_fh.AddHours(8);
                    }
                    else
                        break;


                    //ListBox1.Items.Add(ls_cad);
                    //UploadStatusLabel.Text += ls_cad + "\r\n";

                    continue;



                }

                #endregion


                #region crea registro



                //UploadStatusLabel.Text = "";
                double ld_suma;
                double ld_valor;
                int li_error;
                string ls_colerror = "";
                int li_filreg = 0;

                for (int li_i = 1; li_i < li_col; li_i++)
                {
                    ls_cad = "";                   
                    li_error = 0;

                    //if (arr_cod1[li_i-2].ToString() == "-1")
                    //    continue;

                    //arr_cod2[li_i]


                    ls_sql_del = "delete from " + ps_tabla_gen + " where EVENCLASECODI=" + ps_lectcodi + " and FECHA=" + EPDate.SQLDateOracleString(pdt_fh) + " and GPSCODI=" + arr_cod1[li_i - 1];
                    ls_sql_ins = "insert into " + ps_tabla_gen + "(";

                    ls_sql_campos = "EVENCLASECODI,FECHA,GPSCODI";
                    ls_sql_valores = ps_lectcodi + "," + EPDate.SQLDateOracleString(pdt_fh) + "," + arr_cod1[li_i - 1] ;


                    for (int li_j = 1; li_j < 4; li_j++)
                    {
                        ls_cad += ps_lectcodi + "," +  "," + arr_cod1[li_i - 1] + "," + ls_valores[li_i , li_j];

                        try
                        {
                            ls_sql_campos += "," + "DEVSEC" + (8*li_j).ToString();

                            ld_valor = Convert.ToDouble(ls_valores[li_j, li_i - 1].ToString());
                            ls_cad += ls_valores[li_j, li_i - 1].ToString() + ",";
                            ls_sql_valores += "," + ld_valor.ToString();
                        }
                        catch
                        {
                            ls_cad += "null,";
                            ls_sql_valores += "," + "null";
                            li_error++;
                        }
                    }

                    //ingresa registro
                    if (li_error < 3)
                    {
                        ls_sql_ins += ls_sql_campos + ") values (" + ls_sql_valores + ")";

                        srv_idcc.f_set_consulta1(ls_sql_del);

                        if (srv_idcc.f_set_consulta1(ls_sql_ins) < 0)
                            ls_colerror += li_i + " ";
                        else
                            li_filreg++;
                        //ListBox1.Items.Add(ls_cad);
                        //UploadStatusLabel.Text += ls_sql_del + "\r\n" + ls_sql_ins + "\r\n";
                    }
                    else
                    {
                        ls_colerror += (li_i + 1) + " ";
                    }
                    

                }

                if (ls_colerror == "")
                {
                    ListBox1.Items.Add("Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).");
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Hoja " + ps_hoja + ": OK. (" + li_filreg + " registros creados).", srv_idcc);
                }
                else
                {
                    ListBox1.Items.Add("Error en hoja " + ps_hoja + " Columnas: " + ls_colerror);
                    f_set_tra_logpro_actualizar(li_hidrocod, 3, "Error en hoja " + ps_hoja + ". Columnas: " + ls_colerror, srv_idcc);
                    pb_valida = false;
                }



                #endregion

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
                        dtini = EPDate.ToDateMMDDYYYY(a_dr["F7"].ToString());
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
                        dtfin = EPDate.ToDateMMDDYYYY(a_dr["F8"].ToString());
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
                        dtini = EPDate.ToDateMMDDYYYY(a_dr["F7"].ToString());
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
                        dtfin = EPDate.ToDateMMDDYYYY(a_dr["F8"].ToString());
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
                
    }


}