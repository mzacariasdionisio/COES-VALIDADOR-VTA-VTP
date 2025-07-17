using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.IO;
using wsExtranet;
using System.Globalization;
using System.Configuration;
using fwapp;

namespace WSIC2010.Demanda
{
    public partial class w_me_demanda : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1,2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Demanda/w_me_demanda.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {

                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                        string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                        string[] array_Empresas = in_app.Ls_emprcodi.ToArray();

                        DataTable dtEmpresas = wsDemanda.EmpresasRepxUsuario(array_Empresas, ls_credencial);
                        DDLEmpresa.DataSource = dtEmpresas;
                        DDLEmpresa.DataValueField = dtEmpresas.Columns[0].ToString();
                        DDLEmpresa.DataTextField = dtEmpresas.Columns[1].ToString();
                        DDLEmpresa.DataBind();

                        DDL1.Items.Add(new ListItem("PDO (Programa Diario de Operación)", "1"));

                        //if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                        //if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now.Date))
                        //if(true)
                        //{
                        DDL1.Items.Add(new ListItem("PSO (Programa Semanal de Operación)", "2"));
                        //}

                        TextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        DDLNumSemana.DataSource = Util.CargaDDLSemanas.LlenaSemanas(EPDate.XWeekNumber_Entire4DayWeekRule(DateTime.Now.AddDays(7).Date));
                        DDLNumSemana.DataValueField = "Key";
                        DDLNumSemana.DataTextField = "Value";
                        DDLNumSemana.DataBind();

                        if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                        {
                            TextBox1.Enabled = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }
        }

        protected void UploadButon_Click(object sender, EventArgs e)
        {

            Button2.Visible = false;
            Session["Demandas"] = null;

            string ls_basePath = @"d:\data\demanda\";
            string ls_path = String.Empty;
            string ls_nombreExcel = String.Empty;
            string ls_strConn = String.Empty;
            bool lb_nombre = false;

            #region Carga del archivo
            try
            {
                DateTime ldt_expectedDate;
                if (DateTime.TryParseExact(TextBox1.Text, "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out ldt_expectedDate))
                {
                    if (FileUpload1.HasFile)
                    {
                        string fileName = Server.HtmlEncode(FileUpload1.FileName); /* Nombre del archivo a subir*/
                        int sizeFile = FileUpload1.PostedFile.ContentLength; /* Tamanio del archivo*/
                        string extension = System.IO.Path.GetExtension(fileName); /* Extension del Archivo*/
                        string ls_formatoNombre = "";

                        if (extension.Equals(".xls"))
                        {
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, DDL1.SelectedValue, fileName.Substring(0, (fileName.Length - 4)), extension, out ls_formatoNombre, DateTime.Now.ToString("dd/MM/yyyy"));//TextBox1.Text);
                        }
                        else if (extension.Equals(".xlsx"))
                        {
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, DDL1.SelectedValue, fileName.Substring(0, (fileName.Length - 5)), extension, out ls_formatoNombre, DateTime.Now.ToString("dd/MM/yyyy"));//TextBox1.Text);
                        }

                       

                        //Restringir extension de archivos
                        //if ((extension == ".xls") && lb_nombre)
                        if (Util.Extension.IsExcel(extension, fileName, out ls_nombreExcel, ls_basePath, out ls_strConn) && lb_nombre)
                        {
                            //ls_path = ls_basePath + fileName.Substring(0, (fileName.Length - 4)) + "-" + DateTime.Now.ToString("hhmmss") + ".xls";
                            ls_path = ls_basePath + ls_nombreExcel;

                            //Guardar en el servidor
                            FileUpload1.SaveAs(ls_path);

                            //Instaciamos seguridad 
                            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                            IDemandaService demandaService = new DemandaService();
                            //wsExtranet1.IExtranet wsExtranet = new wsExtranet1.ExtranetClient();
                            double fileMB = Util.SizeFile.SizeOfFile(sizeFile);
                            string cadena = ls_path.Substring(16);

                            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ls_path + ";Extended Properties=" + (char)34 + "Excel 8.0;HDR=No;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";
                            DataSet output = new DataSet();

                            if (DDL1.SelectedValue.Equals("1"))
                            {
                                if (Util.DiferenciaFecha.HoraValida(DateTime.Now, 23))
                                {
                                    if (fileName.Contains("PDO"))
                                    {
                                        //int pi_valor_pdo = wsExtranet.AgregarEnvArchivo(2, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                        int pi_valor_pdo = demandaService.nf_set_insert_archivo_envio(2, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(TextBox1.Attributes["value"])), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));

                                        if (pi_valor_pdo >= 1)
                                        {
                                            //Almacenamos el valor en una variable session
                                            Session["ai_earcodi"] = pi_valor_pdo;

                                            LoadPDOData(output, ls_strConn);

                                            //wsExtranet.ActualizarEstadoEnv(pi_valor_pdo, 2, in_app.is_UserLogin, ls_credencial);if (li_realizado >= 1)

                                            ////wsExtranet.ActualizarCopiaEnv(int ai_earcodi, bool ab_copiado, string as_lastuser, string as_credencil_key);
                                            demandaService.nf_upd_env_copiado(pi_valor_pdo, true, in_app.is_UserLogin);
                                        }
                                        else
                                        {
                                            StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Existen problemas al conectarse al servidor. " +
                                                                 "Contactese con el Administrador </p>";
                                        }
                                        

                                    }
                                    else
                                    {
                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>El documento <strong>" + 
                                                                 "</strong> No corresponde con el Programa Diario de Operaci&oacute;n (PDO)</p>";
                                    }
                                }
                                else
                                {
                                    StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>El envio de informaci&oacute;n del PDO se har&aacute; hasta las <strong>8 horas de cada d&iacute;a</strong></p>";
                                }
                            }
                            else if (DDL1.SelectedValue == "2")
                            {
                                if ((Util.DiferenciaFecha.HoraValida(DateTime.Now, 23)))
                                {
                                    if (fileName.Contains("PSO"))
                                    {
                                        //wsExtranet.AgregarEnvArchivo(int ai_etacod,int ai_emprcodi, string as_arch_nomb, double ad_arch_tam, string as_arch_vers, string as_arch_ruta, int ai_usercode, string as_user_ip, string as_last_user, DateTime ad_fecha, int ai_estad, string as_copiado, string as_credencial_key)
                                        //int pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                        int pi_valor_pso = demandaService.nf_set_insert_archivo_envio(3, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(TextBox1.Attributes["value"])), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));


                                        if (pi_valor_pso >= 1)
                                        {
                                            //Almacenamos el valor en una variable session
                                            Session["ai_earcodi"] = pi_valor_pso;

                                            LoadPSOData(output, ls_strConn);

                                            //wsExtranet.ActualizarEstadoEnv(pi_valor_pso, 3, in_app.is_UserLogin, ls_credencial);
                                            ////wsExtranet.ActualizarCopiaEnv(int ai_earcodi, bool ab_copiado, string as_lastuser, string as_credencil_key);
                                            demandaService.nf_upd_env_copiado(pi_valor_pso, true, in_app.is_UserLogin);
                                        }
                                        else
                                        {
                                            StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Existen problemas al conectarse al servidor. " +
                                                                 "Contactese con el Administrador </p>";
                                        }
                                    }
                                    else
                                    {
                                        //Delete temporary Excel file from the Server path
                                        //BorraArchivo(ls_path);
                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>El documento <strong>" + fileName +
                                                                 "</strong> No corresponde con el Programa Semanal de Operaci&oacute;n (PSO)</p>";
                                    }
                                }
                                else
                                {
                                    StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>El envio de informaci&oacute;n del PSO se har&aacute; hasta las <strong>8 horas de cada d&iacute;a martes</strong></p>";
                                }
                            }
                            else
                            {
                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Seleccione un tipo de programa</p>";
                            }

                        }
                        else
                        {
                            StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Seleccionar archivo en formato Excel (.xls) &oacute; (.xlsx)<br/><ul><li>El archivo <b>" + fileName + "</b> no posee el formato solicitado</li></ul></p>";
                            if (!lb_nombre)
                            {
                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: En el nombre de archivo <br/><ul style='color:#00F;'><li>El archivo <b>" + fileName + "</b> no posee el nombre <b>" + ls_formatoNombre + "</b></li></ul></p>";
                            }
                        }

                    }
                    else
                    {
                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: No se ha seleccionado archivo</a>";
                    }
                }
                else
                {
                    StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: Formato incorrecto de fecha. Ingrese fecha en formato dd/mm/aaaa.</a>";
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "<p style='color:#00F;margin-left:15px;'>ERROR: al cargar el archivo: <ul><li>" + ex.Message + "</li></ul></p>";
            }

            #endregion
        }

        private void LoadPSOData(DataSet ds_output, string ps_strConn)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(ps_strConn))
                {
                    conn.Open();
                    string[] ls_array_hojas = Util.Columnas.GetHojasExcel(conn);
                    string[] ls_array_filas;
                    bool lb_error = false;
                    DateTime ldt_fecha;
                    double ld_dato;
                    StatusLabel.Text = "";
                    List<Util.Pronostico_Demanda> ALPronosticosTotal = new List<Util.Pronostico_Demanda>();
                    DateTime ldt_fechaInicioSabado;

                    COES.MVC.Extranet.wsDemanda.DemandaClient wsdemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
                    Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                    string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                    DataTable dataBarras = wsdemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);

                    try
                    {
                        for (int vi = 0; vi < ls_array_hojas.Length; vi++)
                        {
                            //Llenamos la tabla de datos "oTable"
                            OleDbCommand cmd = new OleDbCommand("SELECT F2,F3,F4,F5,F6,F7,F8,F9 FROM [" + ls_array_hojas[vi] + "$]", conn);
                            cmd.CommandType = CommandType.Text;
                            DataTable oTable = new DataTable("PROG" + vi);
                            ds_output.Tables.Add(oTable);
                            new OleDbDataAdapter(cmd).Fill(oTable);

                            int li_columns = oTable.Columns.Count;
                            int li_rows = oTable.Rows.Count;

                            ls_array_filas = new string[96];

                            if (oTable.Rows[0][0] != DBNull.Value && Convert.ToInt32(oTable.Rows[0][0]).Equals(1))
                            {
                                if (oTable.Rows[15][0] != DBNull.Value)
                                {
                                    //Recuperando la fecha de reporte del doc
                                    int li_fechaInt = Convert.ToInt32(oTable.Rows[15][0]);
                                    ldt_fecha = EPDate.ExcelSerialDateToDMY(li_fechaInt);
                                    //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(Convert.ToDateTime(TextBox1.Text)).AddDays(1); //Aki nos quedamos
                                    DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1);
                                    DateTime fecha = EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);

                                    //if (ldt_fecha.ToString("dd/MM/yyyy").Equals(TextBox1.Text))
                                    if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")) && ldt_fecha.DayOfWeek == DayOfWeek.Tuesday)
                                    {
                                        List<Util.Pronostico_Demanda> ALPronosticos = new List<Util.Pronostico_Demanda>();
                                        //inicializacion de variables
                                        int li_col = 1;
                                        bool lb_cabecera = false;
                                        bool lb_errorLectoCodi = false;
                                        //DateTime ldt_fechaInicioSem = EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);
                                        //DateTime ldt_fechaInicioSem = EPDate.f_fechainiciosemana(ldt_fecha);
                                        DateTime ldt_fechaInicioSem =  EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);


                                        while (!lb_error && li_col < li_columns)
                                        {
                                            //validamos las cabeceras con el valor de 1
                                            if (Convert.ToInt32(oTable.Rows[0][li_col]).Equals(1))
                                            {
                                                Util.Pronostico_Demanda pronostico = new Util.Pronostico_Demanda();

                                                //validamos el codigo y el tipo de demanda
                                                if ((oTable.Rows[1][li_col] != DBNull.Value) &&
                                                    (oTable.Rows[2][li_col] != DBNull.Value))
                                                {
                                                    bool lb_existe = Util.Columnas.GetExistingCode(Convert.ToInt32(oTable.Rows[1][li_col]), dataBarras);

                                                    if (lb_existe)
                                                    {
                                                        pronostico.Codigo = Convert.ToInt32(oTable.Rows[1][li_col].ToString());
                                                        pronostico.LectoCodi = Convert.ToInt32(oTable.Rows[2][li_col].ToString());

                                                        if (pronostico.LectoCodi.Equals(47))
                                                        {
                                                            lb_errorLectoCodi = false;
                                                        }
                                                        else
                                                        {
                                                            lb_errorLectoCodi = true;
                                                            lb_error = true;
                                                            break;
                                                        }

                                                        string ls_fuente = String.Empty;
                                                        if (li_col.Equals(1))
                                                        {
                                                            pronostico.Lugar = Convert.ToString(oTable.Rows[18][1].ToString());
                                                            pronostico.Carga = Convert.ToString(oTable.Rows[19][1].ToString());
                                                            pronostico.Descripcion = Convert.ToString(oTable.Rows[20][1].ToString());
                                                            pronostico.CodigoBarra = Convert.ToString(oTable.Rows[21][1].ToString());
                                                            pronostico.TensionBarra = Convert.ToString(oTable.Rows[22][1].ToString());
                                                            ls_fuente = Convert.ToString(oTable.Rows[23][li_col].ToString().TrimStart().TrimEnd()).ToUpper();

                                                            if (ls_fuente.Equals("M") || ls_fuente.Equals("S"))
                                                            {
                                                                pronostico.FuenteInfo = ls_fuente;
                                                            }
                                                            else
                                                            {
                                                                lb_error = true;
                                                                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Falta asignar tipo de fuente <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                    Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (23 + 1) + "</b> de la hoja " + ls_array_hojas[vi] +
                                                                                    ".Se debe asignar 'S' para DATO SCADA y 'M' para DATO MEDIDOR.</li></ul></p>";
                                                                break;
                                                            }
                                                        }

                                                        if (pronostico.LectoCodi.Equals(47))
                                                        {
                                                            //pronostico.Fecha = ldt_fecha.AddDays(li_col - 1 + 4);
                                                            pronostico.Fecha = ldt_fechaInicioSem.AddDays(li_col - 1);
                                                        }

                                                        pronostico.ld_array_demanda96 = new double[96];

                                                        //completamos los datos en la tabla
                                                        double ld_number;
                                                        for (int li_row = 27; li_row < 123; li_row++)
                                                        {
                                                            //if (oTable.Rows[li_row][li_col] != DBNull.Value)
                                                            if (double.TryParse(Convert.ToString(oTable.Rows[li_row][li_col]), out ld_number))
                                                            {
                                                                //if (!(oTable.Rows[li_row][li_col] is string) || li_col.Equals(1))
                                                                //{
                                                                //ld_dato = Convert.ToDouble(oTable.Rows[li_row][li_col]);
                                                                //if (ld_dato >= 0)
                                                                if (ld_number >= 0)
                                                                {
                                                                    //ls_array_filas[li_row - 27] += "<td>" + ld_dato.ToString("0.00") + "</td>";
                                                                    //pronostico.ld_array_demanda96[li_row - 27] = ld_dato;
                                                                    ls_array_filas[li_row - 27] += "<td>" + ld_number.ToString("0.00") + "</td>";
                                                                    pronostico.ld_array_demanda96[li_row - 27] = ld_number;
                                                                }
                                                                else
                                                                {
                                                                    lb_error = true;
                                                                    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Dato incorrecto <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                             Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (li_row + 1) + "</b> de la hoja " + ls_array_hojas[vi] + " existen registros con valores negativos. Revisar y efectuar opci&oacute;n de carga nuevamente.</li></ul></p>";
                                                                    break;
                                                                }

                                                                //}
                                                                //else
                                                                //{
                                                                //    lb_error = true;
                                                                //    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>Error de formato en la celda " + Util.ExcelUtil.GetExcelColumnName(li_col + 2) +
                                                                //        ":" + (li_row + 1) + " de la hoja " + ls_array_hojas[vi] + "</p>";
                                                                //    break;
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                lb_error = true;
                                                                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Dato incorrecto <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                         Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (li_row + 1) + "</b> de la hoja " + ls_array_hojas[vi] + " existen registros en blanco o con formato err&oacute;neo. Revisar y efectuar opci&oacute;n de carga nuevamente.</li></ul></p>";
                                                                break;
                                                            }
                                                        }
                                                        //fin del for
                                                        ALPronosticos.Add(pronostico);
                                                    }
                                                    else
                                                    {
                                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se cambio formato original</p>";
                                                        lb_cabecera = true;
                                                        break;
                                                    }
                                                }
                                                //Si no tiene codigo de barra, ni tipo de demanda
                                                else
                                                {
                                                    StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR:Agregar c&oacute;digos de Barra y Tipo de Demanda</p>";
                                                    lb_cabecera = true;
                                                    break;
                                                }
                                            }
                                            //Si no tiene codigo de barra, ni tipo de demanda
                                            else if (Convert.ToInt32(oTable.Rows[0][li_col]) != 0)
                                            {
                                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se ha modificado el formato original. Por favor vuelva a descargarlo.";
                                                lb_cabecera = true;
                                                break;
                                            }

                                            li_col++;
                                        }
                                        //fin del while

                                        foreach (Util.Pronostico_Demanda pronostico in ALPronosticos)
                                        {
                                            ALPronosticosTotal.Add(pronostico);
                                        }

                                        //Creamos las tablas para visualizacion del PSO
                                        StringBuilder sb2 = new StringBuilder("");
                                        if (!lb_error && !lb_cabecera && ALPronosticos.Count > 0)
                                        {
                                            //StatusLabel.Text += "<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>";
                                            sb2.Append("<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>");
                                            //Cabeceras de Descripcion
                                            //StatusLabel.Text += "<tr><th align='center'></th>";
                                            sb2.Append("<tr><td align='center' class='left' style='border-left:1px solid #C8C8C8;'>LUGAR</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                    //StatusLabel.Text += "<th colspan='7'>" + ALPronosticos[i].Descripcion + "</th>";
                                                    sb2.Append("<td colspan='7' class='left'>" + ALPronosticos[i].Lugar + "</td>");
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            //StatusLabel.Text += "<tr><th align='center'></th>";
                                            sb2.Append("<tr><td align='center' class='left' style='border-left:1px solid #C8C8C8;'>CARGA</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                    //StatusLabel.Text += "<th colspan='7'>" + ALPronosticos[i].Descripcion + "</th>";
                                                    sb2.Append("<td colspan='7' class='left'>" + ALPronosticos[i].Carga + "</td>");
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            //StatusLabel.Text += "<tr><th align='center'></th>";
                                            sb2.Append("<tr><td align='center' class='left' style='border-left:1px solid #C8C8C8;'>BARRA</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                    //StatusLabel.Text += "<th colspan='7'>" + ALPronosticos[i].Descripcion + "</th>";
                                                    sb2.Append("<td colspan='7' class='left'>" + ALPronosticos[i].Descripcion + "</td>");
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            sb2.Append("</tr>");

                                            sb2.Append("<tr><td align='center' class='left' style='border-left:1px solid #C8C8C8;'>COD. BARRA</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                    //StatusLabel.Text += "<th colspan='7'>" + ALPronosticos[i].Descripcion + "</th>";
                                                    sb2.Append("<td colspan='7' class='left'>" + ALPronosticos[i].CodigoBarra + "</td>");
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            sb2.Append("</tr>");

                                            sb2.Append("<tr><td align='center' class='left' style='border-left:1px solid #C8C8C8;'>TENSI&Oacute;N</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                    //StatusLabel.Text += "<th colspan='7'>" + ALPronosticos[i].Descripcion + "</th>";
                                                    sb2.Append("<td colspan='7' class='left'>" + ALPronosticos[i].TensionBarra + "</td>");
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            sb2.Append("</tr>");

                                            sb2.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>FUENTE DE INFORMACI&Oacute;N</td>");
                                            for (int i = 0; i < ALPronosticos.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    if (ALPronosticos[i].FuenteInfo.Equals("M"))
                                                    {
                                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                                        sb2.Append("<td colspan='7' class='left'>" + "MEDIDOR (M)" + "</td>");
                                                    }
                                                    else if (ALPronosticos[i].FuenteInfo.Equals("S"))
                                                    {
                                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                                        sb2.Append("<td colspan='7' class='left'>" + "SCADA (S)" + "</td>");
                                                    }
                                                }
                                            }
                                            //StatusLabel.Text += "</tr>";
                                            sb2.Append("</tr>");

                                            //Cabeceras de Pronostico
                                            /*StatusLabel.Text += "<tr><th align='center'>" + ls_array_hojas[vi].Substring(1, ls_array_hojas[vi].Length - 3) + "</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th>" +
                                                    "<th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th></tr>" + "<tr><th>HORA</th><th>" +
                                                    ldt_fecha.Date.AddDays(0).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(1).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fecha.Date.AddDays(2).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(3).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fecha.Date.AddDays(4).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fecha.Date.AddDays(5).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fecha.Date.AddDays(6).ToString("dd/MM/yyyy") + "</th></tr>";*/
                                            sb2.Append("<tr><th align='center'></th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th>" +
                                                    "<th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th><th>Pron&oacute;stico (MW)</th></tr>" + "<tr><th>HORA</th><th>" +
                                                    ldt_fechaInicioSem.Date.AddDays(0).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fechaInicioSem.Date.AddDays(1).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fechaInicioSem.Date.AddDays(2).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fechaInicioSem.Date.AddDays(3).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fechaInicioSem.Date.AddDays(4).ToString("dd/MM/yyyy") + "</th><th>" + ldt_fechaInicioSem.Date.AddDays(5).ToString("dd/MM/yyyy") + "</th><th>" +
                                                    ldt_fechaInicioSem.Date.AddDays(6).ToString("dd/MM/yyyy") + "</th></tr>");
                                            for (int k = 0; k < 96; k++)
                                            {
                                                //StatusLabel.Text += "<tr><td>" + Util.ExcelUtil.GetTime(k + 1) + "</td>" + ls_array_filas[k] + "</tr>";
                                                sb2.Append("<tr><td>" + Util.ExcelUtil.GetTime(k + 1) + "</td>" + ls_array_filas[k] + "</tr>");
                                            }
                                            //StatusLabel.Text += "</table>";
                                            sb2.Append("</table>");

                                            StatusLabel.Text += sb2.ToString();

                                            Session["Demandas"] = ALPronosticosTotal;
                                            Button2.Visible = true;
                                        }
                                        else if (ALPronosticosTotal.Count == 0)
                                        {
                                            StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La empresa seleccionada <strong>NO PRESENTA</strong> los c&oacute;digos de barras del formato. Por favor descargue el formato original</p>";

                                        }
                                        else
                                        {
                                            if (!lb_error)
                                            {
                                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La empresa seleccionada <strong>NO PRESENTA</strong> los c&oacute;digos de barras del formato. Por favor descargue el formato original</p>";
                                            }
                                            if (lb_errorLectoCodi)
                                            {
                                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se ha cambiado el formato original. Por favor descarguelo desde la extranet.</p>";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La fecha de la celda B:16 en la hoja " + ls_array_hojas[vi] +
                                                           "<strong> NO COINCIDE</strong> con la fecha seleccionada, o no corresponde a un día Martes</p>";
                                        break;
                                    }
                                }
                                else
                                {
                                    StatusLabel.Text = "<p style='color:#00F'><b>ERROR:</b> No se asign&oacute; fecha en la celda <strong><em>B:16</em></strong> de la hoja <strong>" +
                                                         ls_array_hojas[vi] + "</strong></p>";
                                    break;
                                }
                            }
                            else
                            {
                                StatusLabel.Text = "<p style='color:#00F'><b>ERROR:</b> Se ha cambiado el formato. Por favor descargue el original</p>";
                                break;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se ha modificado el formato original. Por favor vuelva a descargarlo desde la extranet. M&aacutes detalles: <br/><ul style='color:#00F;margin-left:15px'><li>" + ex.Message + "</li></ul></p>";
                    }
                    finally
                    {
                        conn.Close();
                    }

                }//fin del using
            }
            catch (Exception exception)
            {
                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>ERROR : " + exception.Message + "</p>";
            }
        }

        private void LoadPDOData(DataSet ds_output, string ps_strConn)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(ps_strConn))
                {
                    conn.Open();
                    string[] ls_array_hojas = Util.Columnas.GetHojasExcel(conn);
                    string[] ls_array_filas = new string[96];
                    bool lb_error = false;
                    DateTime ldt_fecha;
                    double ld_dato;
                    StatusLabel.Text = "";
                    List<Util.Pronostico_Demanda> ALDemandasTotal = new List<Util.Pronostico_Demanda>();

                    bool lb_cabecera = false;
                    bool lb_errorLectoCodi = false;

                    COES.MVC.Extranet.wsDemanda.DemandaClient wsdemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
                    Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                    string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                    DataTable dataBarras = wsdemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);

                    try
                    {
                        for (int vi = ls_array_hojas.Length - 1; vi >= 0; vi--)
                        {
                            //Llenamos la tabla de datos "oTable"
                            OleDbCommand cmd = new OleDbCommand("SELECT F2,F3,F4,F5,F6,F7,F8,F9 FROM [" + ls_array_hojas[vi] + "$]", conn);
                            cmd.CommandType = CommandType.Text;
                            DataTable oTable = new DataTable("PROG" + vi);
                            ds_output.Tables.Add(oTable);
                            new OleDbDataAdapter(cmd).Fill(oTable);

                            int li_columns = oTable.Columns.Count;
                            int li_rows = oTable.Rows.Count;

                            //ls_array_filas = new string[96];

                            if (oTable.Rows[0][0] != DBNull.Value && Convert.ToInt32(oTable.Rows[0][0]).Equals(1))
                            {
                                if (oTable.Rows[15][0] != DBNull.Value)
                                {
                                    //Recuperando la fecha de reporte del doc
                                    int li_fechaInt = Convert.ToInt32(oTable.Rows[15][0]);
                                    ldt_fecha = EPDate.ExcelSerialDateToDMY(li_fechaInt);

                                    if (ldt_fecha.ToString("dd/MM/yyyy").Equals(TextBox1.Text))
                                    {
                                        List<Util.Pronostico_Demanda> ALDemandas = new List<Util.Pronostico_Demanda>();
                                        //inicializacion de variables
                                        int li_col = 1;

                                        while (!lb_error && li_col < li_columns)
                                        {
                                            //validamos las cabeceras con el valor de 1
                                            if (Convert.ToInt32(oTable.Rows[0][li_col]).Equals(1))
                                            {
                                                Util.Pronostico_Demanda demanda = new Util.Pronostico_Demanda();

                                                //validamos el codigo y el tipo de demanda
                                                if ((oTable.Rows[1][li_col] != DBNull.Value) &&
                                                    (oTable.Rows[2][li_col] != DBNull.Value))
                                                {
                                                    bool lb_existe = Util.Columnas.GetExistingCode(Convert.ToInt32(oTable.Rows[1][li_col]), dataBarras);

                                                    if (lb_existe)
                                                    {
                                                        demanda.Codigo = Convert.ToInt32(oTable.Rows[1][li_col].ToString());
                                                        demanda.LectoCodi = Convert.ToInt32(oTable.Rows[2][li_col].ToString());

                                                        if (demanda.LectoCodi.Equals(45) || demanda.LectoCodi.Equals(46))
                                                        {
                                                            lb_errorLectoCodi = false;
                                                        }
                                                        else
                                                        {
                                                            lb_errorLectoCodi = true;
                                                            lb_error = true;
                                                            break;
                                                        }

                                                        string ls_fuente = String.Empty;
                                                        if ((li_col).Equals(1))
                                                        {
                                                            demanda.Lugar = Convert.ToString(oTable.Rows[18][li_col].ToString());
                                                            demanda.Carga = Convert.ToString(oTable.Rows[19][li_col].ToString());
                                                            demanda.Descripcion = Convert.ToString(oTable.Rows[20][li_col].ToString());
                                                            demanda.CodigoBarra = Convert.ToString(oTable.Rows[21][li_col].ToString());
                                                            demanda.TensionBarra = Convert.ToString(oTable.Rows[22][li_col].ToString());
                                                            ls_fuente = Convert.ToString(oTable.Rows[23][li_col].ToString().TrimStart().TrimEnd()).ToUpper();

                                                            if (ls_fuente.Equals("M") || ls_fuente.Equals("S"))
                                                            {
                                                                demanda.FuenteInfo = ls_fuente;
                                                            }
                                                            else
                                                            {
                                                                lb_error = true;
                                                                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Falta asignar tipo de fuente <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                                 Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (23 + 1) + "</b> de la hoja " + ls_array_hojas[vi] + ". Se debe asignar 'S' para DATO SCADA y 'M' para DATO MEDIDOR.</li></ul></p>";
                                                                break;
                                                            }

                                                        }

                                                        if (demanda.LectoCodi.Equals(45))
                                                        {
                                                            demanda.Fecha = ldt_fecha.AddDays(-1);
                                                        }
                                                        else if (demanda.LectoCodi.Equals(46))
                                                        {
                                                            demanda.Fecha = ldt_fecha.AddDays(1);
                                                        }

                                                        demanda.ld_array_demanda96 = new double[96];

                                                        //completamos los datos en la tabla
                                                        double ld_number;
                                                        for (int li_row = 27; li_row < 123; li_row++)
                                                        {
                                                            //if (oTable.Rows[li_row][li_col] != DBNull.Value && double.TryParse(Convert.ToString(oTable.Rows[li_row][li_col]), out ld_number))
                                                            if (double.TryParse(Convert.ToString(oTable.Rows[li_row][li_col]), out ld_number))
                                                            {
                                                                //if (!(oTable.Rows[li_row][li_col] is string) || li_col .Equals(1))
                                                                //{
                                                                //ld_dato = Convert.ToDouble(oTable.Rows[li_row][li_col]);
                                                                //if (ld_dato >= 0)
                                                                if (ld_number >= 0)
                                                                {
                                                                    //ls_array_filas[li_row - 27] += "<td>" + ld_dato.ToString("0.00") + "</td>";
                                                                    //demanda.ld_array_demanda96[li_row - 27] = ld_dato;
                                                                    ls_array_filas[li_row - 27] += "<td>" + ld_number.ToString("0.00") + "</td>";
                                                                    demanda.ld_array_demanda96[li_row - 27] = ld_number;
                                                                }
                                                                else
                                                                {
                                                                    lb_error = true;
                                                                    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Dato incorrecto <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                             Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (li_row + 1) + "</b> de la hoja " + ls_array_hojas[vi] + " existen registros con valores negativos. Revisar y efectuar opci&oacute;n de carga nuevamente.</li></ul></p>";
                                                                    break;
                                                                }

                                                                //}
                                                                //else
                                                                //{
                                                                //    lb_error = true;
                                                                //    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>Error de formato en la celda " + Util.ExcelUtil.GetExcelColumnName(li_col + 2) +
                                                                //        ":" + (li_row + 1) + " de la hoja " + ls_array_hojas[vi] + "</p>";
                                                                //    break;
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                lb_error = true;
                                                                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + "ERROR: Dato incorrecto <ul style='color:#00F;margin-left:20px'><li>En la celda <b>" +
                                                                                         Util.ExcelUtil.GetExcelColumnName(li_col + 2) + ":" + (li_row + 1) + "</b> de la hoja " + ls_array_hojas[vi] + " existen registros en blanco o con formato err&oacute;neo. Revisar y efectuar opci&oacute;n de carga nuevamente.</li></ul></p>";
                                                                break;
                                                            }
                                                        }
                                                        //fin del for
                                                        ALDemandas.Add(demanda);
                                                    }
                                                    else
                                                    {
                                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se cambio formato original</p>";
                                                        lb_cabecera = true;
                                                        break;
                                                    }
                                                }
                                                //Si no tiene codigo de barra, ni tipo de demanda
                                                else
                                                {
                                                    StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Agregar c&oacute;digos de Barra y Tipo de Demanda</p>";
                                                    lb_cabecera = true;
                                                    break;
                                                }
                                            }
                                            //Si no tiene codigo de barra, ni tipo de demanda
                                            else if (Convert.ToInt32(oTable.Rows[0][li_col]) != 0)
                                            {
                                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se han agregado columnas al formato. Por favor descargue el formato original";
                                                lb_cabecera = true;
                                                break;
                                            }

                                            li_col++;
                                        }
                                        //fin del while

                                        foreach (Util.Pronostico_Demanda demanda in ALDemandas)
                                        {
                                            ALDemandasTotal.Add(demanda);
                                        }

                                    }
                                    else
                                    {
                                        lb_error = true;
                                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La fecha en la celda B:16 en la hoja " + ls_array_hojas[vi] +
                                                           "<strong> NO COINCIDE</strong> con la fecha seleccionada.</p>";
                                        break;
                                    }
                                }
                                else
                                {
                                    StatusLabel.Text = "<p style='color:#00F'><b>ERROR:</b> No se asign&oacute; fecha en la celda <strong><em>B:16</em></strong> de la hoja <strong>" +
                                                         ls_array_hojas[vi] + "</strong></p>";
                                    break;
                                }
                            }
                            else
                            {
                                StatusLabel.Text = "<p style='color:#00F'><b>ERROR:</b> Se ha cambiado el formato. Por favor descargue el original</p>";
                                break;
                            }


                        }
                        //fin del for de las hojas

                        //Creamos las tablas para visualizacion del PDO
                        StringBuilder sb1 = new StringBuilder("");
                        if (!lb_error && !lb_cabecera && ALDemandasTotal.Count > 0)
                        {
                            //StatusLabel.Text += "<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>";
                            sb1.Append("<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>");
                            //Cabeceras de Descripcion
                            //StatusLabel.Text += "<tr><th align='center'></th>";

                            //GridView gv3;
                            //wsDemanda.DemandaClient wsdemanda = new wsDemanda.DemandaClient();
                            //Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                            //string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                            /*
                            String ls_cadena_html;
                            StringWriter sr = new StringWriter();
                            HtmlTextWriter htm = new HtmlTextWriter(sr);
                            */
                            //DataTable dt = new DataTable();

                            //for (int i = 0; i < ALDemandasTotal.Count; i++)
                            //{
                            //gv3 = new GridView();
                            //if (ALDemandasTotal[i].LectoCodi == 45)
                            //{
                            //DataTable dt_Desc = wsdemanda.PuntoMedicionBarraDesc(ALDemandasTotal[i].Codigo, ls_credencial);

                            /*foreach (DataRow row in dt_Desc.Rows)
                            {
                                sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>" + row[0].ToString() + "</td>");
                                sb1.Append("<td colspan='2' class='left'>" + row[1].ToString() + "</td>");
                                sb1.Append("</tr>");
                            }*/
                            /*
                            gv3.DataSource = wsdemanda.PuntoMedicionBarraDesc(ALDemandasTotal[i].Codigo, ls_credencial);
                            gv3.DataBind();
                            gv3.RenderControl(htm);
                            htm.Flush();
                            ls_cadena_html = sr.ToString();
                            */
                            /*
                            DataTable dt_Desc = wsdemanda.PuntoMedicionBarraDesc(ALDemandasTotal[i].Codigo, ls_credencial);
                            AgregaColumna(dt, dt_Desc);
                            */


                            //}

                            //}

                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>LUGAR</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                    sb1.Append("<td colspan='2' class='left'>" + ALDemandasTotal[i].Lugar + "</td>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>CARGA</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                    sb1.Append("<td colspan='2' class='left'>" + ALDemandasTotal[i].Carga + "</td>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>BARRA</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                    sb1.Append("<td colspan='2' class='left'>" + ALDemandasTotal[i].Descripcion + "</td>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>COD. BARRA</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                    sb1.Append("<td colspan='2' class='left'>" + ALDemandasTotal[i].CodigoBarra + "</td>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>TENSI&Oacute;N</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                    sb1.Append("<td colspan='2' class='left'>" + ALDemandasTotal[i].TensionBarra + "</td>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");


                            //sb1.Append("<table CELLSPACING='3' CELLPADDING='3' BORDER='1' class='sample'>");
                            sb1.Append("<tr><td class='left' style='border-left:1px solid #C8C8C8;'>FUENTE</td>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if ((ALDemandasTotal[i].LectoCodi == 45) && (ALDemandasTotal[i].FuenteInfo != null))
                                {
                                    if (ALDemandasTotal[i].FuenteInfo.Equals("M"))
                                    {
                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                        sb1.Append("<td colspan='2' class='left'>" + "MEDIDOR (M)" + "</td>");
                                    }
                                    else if (ALDemandasTotal[i].FuenteInfo.Equals("S"))
                                    {
                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                        sb1.Append("<td colspan='2' class='left'>" + "SCADA (S)" + "</td>");
                                    }
                                }
                                else if (ALDemandasTotal[i].LectoCodi == 46 && (ALDemandasTotal[i].FuenteInfo != null))
                                {
                                    if (ALDemandasTotal[i].FuenteInfo.Equals("M"))
                                    {
                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                        sb1.Append("<td colspan='2' class='left'>" + "MEDIDOR (M)" + "</td>");
                                    }
                                    else if (ALDemandasTotal[i].FuenteInfo.Equals("S"))
                                    {
                                        //StatusLabel.Text += "<th colspan='2'>" + ALDemandas[i].Descripcion + "</th>";
                                        sb1.Append("<td colspan='2' class='left'>" + "SCADA (S)" + "</td>");
                                    }
                                }
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            //Cabeceras de Demanda y Pronostico
                            //StatusLabel.Text += "<tr><th align='center'></th>";
                            sb1.Append("<tr><th align='center' style='border-left:1px solid #C8C8C8;'></th>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                if (ALDemandasTotal[i].LectoCodi == 45)
                                    //StatusLabel.Text += "<th>" + "DEMANDA (MW)" + "</th>";
                                    sb1.Append("<th>" + "DEMANDA (MW)" + "</th>");
                                else if (ALDemandasTotal[i].LectoCodi == 46)
                                    //StatusLabel.Text += "<th>" + "PRON&Oacute;STICO (MW)" + "</th>";
                                    sb1.Append("<th>" + "PRON&Oacute;STICO (MW)" + "</th>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            //StatusLabel.Text += "<tr><th align='center'>HORA</th>";
                            sb1.Append("<tr><th align='center' style='border-left:1px solid #C8C8C8;'>HORA</th>");
                            for (int i = 0; i < ALDemandasTotal.Count; i++)
                            {
                                //StatusLabel.Text += "<th>" + ALDemandas[i].Fecha.Date.ToString("MM/dd/yyyy") + "</th>";
                                sb1.Append("<th>" + ALDemandasTotal[i].Fecha.Date.ToString("dd/MM/yyyy") + "</th>");
                            }
                            //StatusLabel.Text += "</tr>";
                            sb1.Append("</tr>");

                            for (int k = 0; k < 96; k++)
                            {
                                //StatusLabel.Text += "<tr><td>" + Util.ExcelUtil.GetTime(k + 1) + "</td>" + ls_array_filas[k] + "</tr>";
                                sb1.Append("<tr><td>" + Util.ExcelUtil.GetTime(k + 1) + "</td>" + ls_array_filas[k] + "</tr>");
                            }
                            //StatusLabel.Text += "</table>";
                            sb1.Append("</table>");

                            StatusLabel.Text = sb1.ToString();


                            Session["Demandas"] = ALDemandasTotal;
                            Button2.Visible = true;
                        }
                        else if ((ALDemandasTotal.Count == 0) && !lb_error)
                        {
                            StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La empresa seleccionada <strong>NO PRESENTA</strong> los c&oacute;digos de barras del formato. Por favor descargue el formato original</p>";
                        }
                        else
                        {
                            if (!lb_error)
                            {
                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'><b>ERROR:</b> La empresa seleccionada <strong>NO PRESENTA</strong> los c&oacute;digos de barras del formato. Por favor descargue el formato original</p>";
                            }
                            if (lb_errorLectoCodi)
                            {
                                StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se ha cambiado el formato original. Por favor descarguelo desde la extranet.</p>";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "<p style='color:#00F;margin-left:15px'>ERROR: Se han agregado columnas. Por favor descargue el formato original. <br/><ul style='color:#00F;margin-left:15px'><li>" + ex.Message + "</li></ul></p>";
                    }
                    finally
                    {
                        conn.Close();
                    }

                }//fin del using
            }
            catch (Exception exception)
            {
                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>ERROR : " + exception.Message + "</p>";
            }
        }
        /*
        private void AgregaColumna(DataTable dt1, DataTable dt2)
        {
            DataTable dt =  new DataTable();
            DataRow dr;
            if (dt1.Rows.Count.Equals(0))
            {
                dt = dt2;
            }
            else
            {
                foreach (DataRow drow in dt2.Rows)
                {
                    for (int i = 1; i < dt2.Columns.Count; i++ )
                    {
                        dt1.Columns.Add(dt2.Columns[i]);
                    }

                    Data
                }
            }
        }
        */
        protected void Button2_Click(object sender, EventArgs e)
        {
            DataAccessLayer.OracleDataAccessX ln_conex_ora;
            ln_conex_ora = new DataAccessLayer.OracleDataAccessX();
            try
            {
                wcfSicOperacion.Demanda ln_demanda = new wcfSicOperacion.Demanda();
                IDemandaService demandaService = new DemandaService();
                string ls_dns_sic = ConfigurationManager.ConnectionStrings["SICOES"].ToString();
                List<Util.Pronostico_Demanda> Demandas = (List<Util.Pronostico_Demanda>)Session["Demandas"];
                /*wsDemanda.IDemanda wsDemanda = new wsDemanda.DemandaClient();*/
                Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);


                ln_conex_ora.CreateConnection(ls_dns_sic);
                ln_conex_ora.Open();

                //Inicio de la transaccion

                //ln_conex_ora.BeginTransaction();

                //StatusLabel.Text = "<ul>";
                int li_realizado = 0;
                int li_valor = Convert.ToInt32(Session["ai_earcodi"]);

                for (int i = 0; i < Demandas.Count; i++)
                {
                    //StatusLabel.Text += "<li>" + demanda.Codigo + "</li>";
                    //realizado = wsDemanda.AgregarDemanda48(demanda.Fecha, demanda.LectoCodi, demanda.InfoCodi, demanda.Codigo, demanda.ld_array_demanda96, ls_credencial); 

                    //Para los PDO 
                    if ((!Demandas[i].Codigo.Equals(0)) && (Demandas[i].LectoCodi.Equals(45)) && (Demandas[i].FuenteInfo != null) && i % 2 == 0)
                    {
                        li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 2, Demandas[i].Codigo, EPDate.ToDate(TextBox1.Text), Demandas[i].FuenteInfo, ref ln_conex_ora);
                        if (li_realizado > 0)
                        {
                            li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                        }
                    }

                    //Para los PSO 
                    if ((!Demandas[i].Codigo.Equals(0)) && (Demandas[i].LectoCodi.Equals(47)) && (Demandas[i].FuenteInfo != null) && i % 7 == 0)
                    {
                        li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 3, Demandas[i].Codigo, EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1), Demandas[i].FuenteInfo, ref ln_conex_ora);
                        if (li_realizado > 0)
                        {
                            li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                        }
                    }

                    string ls_comando = String.Empty;
                    string ls_cadena = String.Empty;
                    string ls_cadenaCodigos = String.Empty;
                    double ld_meditotal = 0;

                    if (li_realizado >= 0)
                    {
                        //li_realizado = ln_demanda.AgregarDemanda96(Demandas[i].Fecha, Demandas[i].LectoCodi, Demandas[i].InfoCodi, Demandas[i].Codigo, Demandas[i].ld_array_demanda96, ref ln_conex_ora, ls_credencial);
                        /*Borrando datos*/
                        ls_comando = " DELETE FROM ME_MEDICION96";
                        ls_comando += " WHERE TIPOINFOCODI = " + Demandas[i].InfoCodi;
                        ls_comando += " AND LECTCODI = " + Demandas[i].LectoCodi;
                        ls_comando += " AND MEDIFECHA = TO_DATE('" + Demandas[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
                        ls_comando += " AND PTOMEDICODI = " + Demandas[i].Codigo;
                        li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                        if (!String.IsNullOrEmpty(ls_cadena))
                        {
                            StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + ls_cadena + "</p>";
                        }

                        /*Insertando Datos*/
                        if (li_realizado >= 0)
                        {
                            ls_comando = "INSERT INTO ME_MEDICION96 (MEDIFECHA, LECTCODI, TIPOINFOCODI, PTOMEDICODI,";
                            ls_comando += "H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,";
                            ls_comando += "H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,H49,H50,H51,H52,H53,H54,H55,H56,H57,H58,H59,H60,";
                            ls_comando += "H61,H62,H63,H64,H65,H66,H67,H68,H69,H70,H71,H72,H73,H74,H75,H76,H77,H78,H79,H80,H81,H82,H83,H84,H85,H86,H87,H88,H89,H90,H91,H92,H93,H94,H95,H96,MEDITOTAL) ";
                            ls_comando += "VALUES (TO_DATE('" + Demandas[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')," + Demandas[i].LectoCodi + "," + Demandas[i].InfoCodi + "," + Demandas[i].Codigo;
                            ls_comando += nf_get_string_Valores(Demandas[i].ld_array_demanda96, out ld_meditotal);
                            ls_comando += "," + ld_meditotal;
                            ls_comando += ")";
                            li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                            if (!String.IsNullOrEmpty(ls_cadena))
                            {
                                StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>" + ls_cadena +"</p>";
                            }
                        }
                    }


                    if (li_realizado < 0)
                        break;
                }

                if (li_realizado >= 0)
                {
                    //wsExtranet.ActualizarEstadoEnv(pi_valor_pso, 3, in_app.is_UserLogin, ls_credencial);
                    //li_realizado = extranet.ActualizarEstadoEnv(li_valor, 2, in_app.is_UserLogin, ref ln_conex_ora, ls_credencial);// 2 es para procesado
                    li_realizado = demandaService.nf_upd_env_estado(li_valor, 1, in_app.is_UserLogin);// 1 valor por defecto (Enviado)
                    //li_realizado = extranet.ActualizarEstadoEnv(li_valor, 2, in_app.is_UserLogin, ls_credencial); 
                    //li_realizado = extra.ActualizarEstadoEnv(li_valor, 2, in_app.is_UserLogin, in_app.is_UserName);

                    if (li_realizado >= 0)
                    {
                        //ln_conex_ora.CommitTransaction();
                    }
                    else
                    {
                        //ln_conex_ora.RollbackTransaction();
                    }

                }
                else
                {
                    //ln_conex_ora.RollbackTransaction();
                }


                //StatusLabel.Text = "</ul><p style='color:#00F;margin-left:20px'>Se envi&oacute; los datos exitosamente</p>";
                if (li_realizado <= 0)
                {
                    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>Se presento problemas al momento de enviar los datos</p>";
                    Button2.Visible = false;
                }
                else
                {
                    StatusLabel.Text = "<p style='color:#00F;margin-left:20px'>Los datos se enviaron exitosamente</p>";
                    Button2.Visible = false;
                }

            }
            catch (Exception ex)
            {
                StatusLabel.Text = "ERROR: " + ex.Message;
            }
            finally
            {
                ln_conex_ora.Close();
            }

        }

        private string nf_get_string_Valores(double[] ad_arrayValores, out double ad_meditotal)
        {
            string ls_cadenaValores = String.Empty;
            ad_meditotal = 0;
            for (int i = 0; i < ad_arrayValores.Length; i++)
            {
                ad_meditotal += ad_arrayValores[i];
                ls_cadenaValores += "," + ad_arrayValores[i];
            }

            return ls_cadenaValores;
        }

        protected void DDL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox1.Text = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");

            if (DDL1.SelectedItem.Value == "1")
            {
                if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    TextBox1.Enabled = true;
                }
                //CalendarExtender1.Enabled = false;
                DDLNumSemana.Enabled = false;
                DDLNumSemana.Visible = false;
                lblNumSem.Enabled = false;
                lblNumSem.Visible = false;
            }
            else if (DDL1.SelectedItem.Value == "2")
            {
                //TextBox1.Text = EPDate.f_fechafinsemana(DateTime.Now).AddDays(1).ToString("dd/MM/yyyy");
                //TextBox1.Text = String.Empty;
                //CalendarExtender1.Enabled = false;
                if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                      TextBox1.Enabled = true;
                //    DDLNumSemana.Enabled = true;
                //    DDLNumSemana.Visible = true;
                //    lblNumSem.Enabled = true;
                //    lblNumSem.Visible = true;
                }
                //DDLNumSemana.Text = Convert.ToString(EPDate.XWeekNumber_Entire4DayWeekRule(EPDate.f_fechafinsemana(DateTime.Now).AddDays(1))).PadLeft(2, '0');
                //TextBox1.Text = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
            }
            
        }

        protected void DDLNumSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox1.Text = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
        }

    }
}