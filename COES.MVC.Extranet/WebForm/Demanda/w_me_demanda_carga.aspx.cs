using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using ExcelLibrary.SpreadSheet;
using WSIC2010.Util;
using System.Globalization;
using WScoes;
using wsExtranet;
using System.Configuration;
using System.IO;
using System.Text;
using fwapp;
using COES.Servicios.Aplicacion.WLib;

namespace WSIC2010.Demanda
{
    public partial class w_me_demanda_carga : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        Stopwatch stopw = new Stopwatch();
        DateTime ldt_expectedDate;
        WLibAppServicio servicio = new WLibAppServicio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Demanda/w_me_demanda_carga.aspx";
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

                        ddl_tipoPrograma.Items.Add(new ListItem("PDO (Programa Diario de Operación)", "1"));

                        //if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                        //if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now.Date))
                        //if(true)
                        //{
                        ddl_tipoPrograma.Items.Add(new ListItem("PSO (Programa Semanal de Operación)", "2"));
                        //}

                        //TextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        tBoxFecha.Attributes["value"] = DateTime.Now.ToString("dd/MM/yyyy");

                        DDLNumSemana.DataSource = Util.CargaDDLSemanas.LlenaSemanas(EPDate.XWeekNumber_Entire4DayWeekRule(DateTime.Now.AddDays(7).Date));
                        DDLNumSemana.DataValueField = "Key";
                        DDLNumSemana.DataTextField = "Value";
                        DDLNumSemana.DataBind();

                        if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                        {
                            //TextBox1.Enabled = true;
                            tBoxFecha.Disabled = false;
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
            ListBox1.Items.Clear();
            ListBox1.DataSource = null;
            ListBox1.DataBind();

            string ls_basePath = @"d:\data\demanda\";
            string ls_path = String.Empty;
            string ls_nombreExcel = String.Empty;
            string ls_strConn = String.Empty;
            bool lb_nombre = false;

            List<Pronostico_Demanda> AL_demandas_Total = new List<Pronostico_Demanda>();

            #region Carga Archivos
            try
            {
                //if (DateTime.TryParseExact(TextBox1.Text, "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out ldt_expectedDate))
                if (DateTime.TryParseExact(Convert.ToString(tBoxFecha.Attributes["value"]), "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out ldt_expectedDate))
                {
                    if (FileUpload1.HasFile)
                    {
                        string fileName = Server.HtmlEncode(FileUpload1.FileName); /* Nombre del archivo a subir*/
                        int sizeFile = FileUpload1.PostedFile.ContentLength; /* Tamanio del archivo*/
                        string ls_extension = System.IO.Path.GetExtension(fileName); /* Extension del Archivo*/
                        string ls_formatoNombre = "";

                        if (ls_extension.Equals(".xls"))
                        {
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, ddl_tipoPrograma.SelectedValue, fileName.Substring(0, (fileName.Length - 4)), ls_extension, out ls_formatoNombre, ldt_expectedDate.ToString("dd/MM/yyyy"));
                        }
                        else if (ls_extension.Equals(".xlsx"))
                        {
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, ddl_tipoPrograma.SelectedValue, fileName.Substring(0, (fileName.Length - 5)), ls_extension, out ls_formatoNombre, ldt_expectedDate.ToString("dd/MM/yyyy"));
                        }

                        //Restringir extension de archivos
                        if (Util.Extension.IsExcel(ls_extension, fileName, out ls_nombreExcel, ls_basePath, out ls_strConn) && lb_nombre)
                        {
                            ls_path = ls_basePath + ls_nombreExcel;

                            //Guardar en el servidor
                            FileUpload1.SaveAs(ls_path);

                            //Instaciamos seguridad 
                            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);

                            //Instaciamos WS Demanda
                            IDemandaService demandaService = new DemandaService();
                            int pi_codigoTipoEmpresa = 0;
                            DataTable ln_data = demandaService.nf_get_empresa_detalles(Convert.ToInt32(DDLEmpresa.SelectedValue));
                            if (nf_get_data(ln_data))
	                        {
		                        pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());
	                        }

                            double fileMB = Util.SizeFile.SizeOfFile(sizeFile);
                            string cadena = ls_path.Substring(16);

                            if (ddl_tipoPrograma.SelectedValue.Equals("1"))
                            {
                                
                                if (fileName.Contains("DIARIO"))
                                {
                                    if (DateTime.Now.Date >= ldt_expectedDate.Date)
                                    {
                                        int pi_valor_pdo = 0;
                                        if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, 16, 1))
                                        {
                                            // 1 valor por defecto (Enviado)
                                            //pi_valor_pdo = wsExtranet.AgregarEnvArchivo(2, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])), 1, "N", ls_credencial);
                                            pi_valor_pdo = demandaService.nf_set_insert_archivo_envio(2, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));
                                        }
                                        else
                                        {
                                            //pi_valor_pdo = wsExtranet.AgregarEnvArchivo(2, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])), 5, "N", ls_credencial);
                                            pi_valor_pdo = demandaService.nf_set_insert_archivo_envio(2, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])), 5, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));
                                        }

                                        if (pi_valor_pdo >= 1)
                                        {
                                            if (pi_codigoTipoEmpresa > 0)
                                            {
                                                LoadProgramaDataXLS(ls_path, pi_valor_pdo, Convert.ToInt32(ddl_tipoPrograma.SelectedValue), pi_codigoTipoEmpresa, ls_extension);
                                            }
                                            else
                                            {
                                                ListBox1.Items.Add("ERROR: Existen problemas al obtener el código de tipo de empresa");
                                            }

                                        }
                                        else
                                        {
                                            ListBox1.Items.Add("ERROR: Existen problemas al conectarse al servidor. Contactese con el Administrador");
                                        }
                                    }
                                    else
                                    {
                                        ListBox1.Items.Add("La fecha debe ser menor a la fecha actual");
                                    }
                                }
                                else
                                {
                                    ListBox1.Items.Add(new ListItem("El documento no corresponde con el Programa Diario de Operación (DIARIO)", "Error"));
                                }
                                
                            }
                            else if (ddl_tipoPrograma.SelectedValue == "2")
                            {
                                if (fileName.Contains("SEMANAL"))
                                {
                                    // 1 valor por defecto (Enviado)
                                    //int pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                    int pi_valor_pso = 0;
                                    if (Util.DiferenciaFecha.HoraValida(EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)), 16, 2))
                                        //pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])).AddDays(-4), 1, "N", ls_credencial);
                                        pi_valor_pso = demandaService.nf_set_insert_archivo_envio(3, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])).AddDays(-4), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));
                                    else
                                        //pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])).AddDays(-4), 5, "N", ls_credencial);
                                        pi_valor_pso = demandaService.nf_set_insert_archivo_envio(3, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"])).AddDays(-4), 5, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));

                                    if (pi_valor_pso >= 1)
                                    {
                                        if (pi_codigoTipoEmpresa > 0)
	                                    {
                                            LoadProgramaDataXLS(ls_path, pi_valor_pso, Convert.ToInt32(ddl_tipoPrograma.SelectedValue), pi_codigoTipoEmpresa, ls_extension);
	                                    }
                                        else
	                                    {
                                            ListBox1.Items.Add("ERROR: Existen problemas al obtener el código de tipo de empresa");
	                                    }
                                    }
                                    else
                                    {
                                        ListBox1.Items.Add(new ListItem("ERROR: Existen problemas al conectarse al servidor. Contactese con el Administrador", "Error"));
                                    }
                                }
                                else
                                {
                                    ListBox1.Items.Add(new ListItem("El documento " + fileName  + "No corresponde con el Programa Semanal de Operación (SEMANAL)", "Error"));
                                }
                            }
                        }
                        else
                        {
                            ListBox1.Items.Add(new ListItem("ERROR: El archivo " + fileName + " no corresponde con el formato: " + ls_formatoNombre, "Error"));
                        }

                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: No se ha seleccionado archivo", "Error"));
                    }
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: Formato incorrecto de fecha. Ingrese fecha en formato dd/mm/aaaa.", "Error"));
                }
            }
            catch (Exception ex)
            {
                ListBox1.Items.Add(new ListItem("ERROR: En la carga del archivo: " + ex.Message, "Error"));
            }

            #endregion

        }

        private bool nf_get_data(DataTable ln_data)
        {
            bool lb_tiene_data = true;
            if ((ln_data != null) && (ln_data.Rows.Count > 0))
            {
                return lb_tiene_data;
            }
            else
            {
                return lb_tiene_data;
            }
        }

        private void LoadProgramaDataXLS(string ps_path, int pi_earcodi, int pi_tipoPrograma, int pi_codigoTipoEmpresa, string ps_extension)
        {
            stopw.Start();
            ListBox1.Items.Add("Proceso de verificación iniciado a las " + DateTime.Now.ToString("H:mm:ss") + " del día " + DateTime.Now.ToString("dd/MM/yyyy"));
            List<Pronostico_Demanda> AL_demandas_Total = new List<Pronostico_Demanda>();
            DateTime ldt_fechaInicioSem = new DateTime(2000, 1, 1);
            DataTable dt_ptosMedicionxEmpresa = nf_get_puntosMedicionxEmpresa();
            DateTime ldt_fecha = new DateTime(2000, 1, 1);


            //Workbook wb = Workbook.Load(ps_path); //XLS file
            DemandaService demandaServicio = new DemandaService();
            //DataSet ds = demandaServicio.nf_get_FileDemandaExcel(ps_path, ps_extension);
            DataSet ds = ExcelReader.nf_get_excel_to_ds(ps_path, ps_extension);

            bool lb_fechavalida = true;

            //foreach (var worksheet in wb.Worksheets) //XLS sheets
            foreach (DataTable dt in ds.Tables)
            {
                //if (IsValidName(worksheet.Name))
                if (IsValidName(dt.TableName))
                {
                    //ListBox1.Items.Add("Accediendo a la hoja: '" + worksheet.Name + "'");
                    ListBox1.Items.Add("Accediendo a la hoja: '" + dt.TableName + "'");
                    //var cells = worksheet.Cells;
                    DataRowCollection drows = dt.Rows;

                    //Valida fecha en XLS file celda C16
                    //ldt_fecha = nf_get_fechaValida(Convert.ToInt32(cells[15, 2].Value), worksheet.Name, out lb_fechavalida);
                    if (ps_extension == ".xls" || ps_extension == ".XLS")
                        ldt_fecha = nf_get_fechaValida(Convert.ToInt32(dt.Rows[15]["C"].ToString()), dt.TableName, out lb_fechavalida);
                    else if (ps_extension == ".xlsx" || ps_extension == ".XLSX")
                        ldt_fecha = nf_get_fechaValida(dt.Rows[15]["C"].ToString(), dt.TableName, out lb_fechavalida);

                    List<Pronostico_Demanda> AL_demandas = new List<Pronostico_Demanda>();
                    //Recorre columnas XLS file
                    //for (int li_columna = 0; li_columna <= cells.LastColIndex; li_columna++)
                    for (int li_columna = 0; li_columna < dt.Columns.Count; li_columna++)
                    {
                    //    if (cells[0, li_columna].StringValue.Equals("1")) //XLS headers
                    if (dt.Rows[0][li_columna].ToString().Equals("1")) //XLS headers
                    {
                            if (lb_fechavalida)
                            {
                    //          if (ValidateDemandaXLS(dt_ptosMedicionxEmpresa, ldt_fecha, li_columna, pi_tipoPrograma, pi_codigoTipoEmpresa, cells, worksheet.Name, AL_demandas))
                                if (ValidateDemandaXLS(dt_ptosMedicionxEmpresa, ldt_fecha, li_columna, pi_tipoPrograma, pi_codigoTipoEmpresa, dt, dt.TableName, AL_demandas))
                                {
                                    foreach (var demanda in AL_demandas)
                                    {
                                        AL_demandas_Total.Add(demanda);
                                        AL_demandas = new List<Pronostico_Demanda>();
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    //ListBox1.Items.Add(new ListItem("ERROR: Nombre de Hoja Inválido '" + worksheet.Name + "'", "Error"));
                    ListBox1.Items.Add(new ListItem("ERROR: Nombre de Hoja Inválido '" + dt.TableName + "'", "Error"));
                }
            }

            bool lb_errores = false;

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Value.Trim().TrimEnd().ToUpper().Equals("ERROR"))
                {
                    lb_errores = true;
                    btn_exporta.Enabled = true; btn_exporta.Visible = true;
                }
            }

            stopw.Stop();
            ListBox1.Items.Add("Proceso de verificación finalizó en " + Math.Truncate(stopw.Elapsed.TotalSeconds * 1000) / 1000 + " segundos");
            stopw.Start();
            ListBox1.Items.Add("Proceso de carga iniciado desde los " + Math.Truncate(stopw.Elapsed.TotalSeconds * 1000) / 1000 + " segundos");

            if (!lb_errores)
            {

                /* Grabamos a base de datos*/
                #region Conexion Base Datos

                nf_set_SaveDemandas(AL_demandas_Total, pi_earcodi);

                #endregion

            }
            else
            {
                ListBox1.Items.Add("NO SE PUEDE CARGAR INFORMACIÓN: Existen errores en la carga ");
            }
        }

        private void nf_set_SaveDemandas(List<Pronostico_Demanda> AL_demandas_Total, int pi_earcodi)
        {
            DataAccessLayer.OracleDataAccessX ln_conex_ora = new DataAccessLayer.OracleDataAccessX();
            wcfSicOperacion.Demanda ln_demanda = new wcfSicOperacion.Demanda();
            IDemandaService demandaService = new DemandaService();
            string ls_dns_sic = ConfigurationManager.ConnectionStrings["SICOES"].ToString();
            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
            ln_conex_ora.CreateConnection(ls_dns_sic);
            ln_conex_ora.Open();

            //Inicio de la transaccion

            int li_realizado = 0;
            int li_valor = pi_earcodi;

            string ls_comando = String.Empty;
            string ls_cadena = String.Empty;
            string ls_cadenaCodigos = String.Empty;
            double ld_meditotal = 0;

            for (int i = 0; i < AL_demandas_Total.Count; i++)
            {
                //Para los PDO 
                if ((AL_demandas_Total[i].Codigo > 0) && (AL_demandas_Total[i].LectoCodi.Equals(45)) && i % 3 == 0)
                {
                    //li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 2, AL_demandas_Total[i].Codigo, EPDate.ToDate(TextBox1.Text), AL_demandas_Total[i].FuenteInfo, ref ln_conex_ora);
                    //if (li_realizado > 0)
                    //{
                        //li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                    li_realizado = nf_get_add_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0));
                    //}
                }

                //Para los PSO 
                if ((AL_demandas_Total[i].Codigo > 0) && (AL_demandas_Total[i].LectoCodi.Equals(47)) && i % 7 == 0)
                {
                    //li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 3, AL_demandas_Total[i].Codigo, EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1), AL_demandas_Total[i].FuenteInfo, ref ln_conex_ora);
                    //if (li_realizado > 0)
                    //{
                        //li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                    li_realizado = nf_get_add_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0));
                    //}
                }

                if (li_realizado >= 0)
                {
                    /*Borrando datos*/
                    ls_comando = " DELETE FROM ME_MEDICION48";
                    ls_comando += " WHERE TIPOINFOCODI = " + AL_demandas_Total[i].TipoInfo;
                    ls_comando += " AND LECTCODI = " + AL_demandas_Total[i].LectoCodi;
                    ls_comando += " AND MEDIFECHA = TO_DATE('" + AL_demandas_Total[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
                    ls_comando += " AND PTOMEDICODI = " + AL_demandas_Total[i].Codigo;
                    li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                    if (!String.IsNullOrEmpty(ls_cadena))
                    {
                        ListBox1.Items.Add("Error de Base de Datos. Detalles: " + ls_cadena);
                    }

                    /*Insertando Datos*/
                    if (li_realizado >= 0)
                    {
                        string ls_userlogin = (in_app.is_UserLogin.Length > 19) ? in_app.is_UserLogin.Substring(0, 19) : in_app.is_UserLogin;
                        ls_comando = "INSERT INTO ME_MEDICION48 (MEDIFECHA, LECTCODI, TIPOINFOCODI, PTOMEDICODI,LASTDATE, LASTUSER,";
                        ls_comando += "H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,";
                        ls_comando += "H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,MEDITOTAL) ";
                        ls_comando += "VALUES (TO_DATE('" + AL_demandas_Total[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')," + AL_demandas_Total[i].LectoCodi + ",";
                        ls_comando += AL_demandas_Total[i].TipoInfo + "," + AL_demandas_Total[i].Codigo + ",sysdate,'" + ls_userlogin + "'";
                        ls_comando += nf_get_string_Valores(AL_demandas_Total[i].ld_array_demanda48, out ld_meditotal);
                        ls_comando += "," + ld_meditotal;
                        ls_comando += ")";
                        li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                        if (!String.IsNullOrEmpty(ls_cadena))
                        {
                            ListBox1.Items.Add("Error de Base de Datos. Detalles: " + ls_cadena);
                        }
                    }
                }

                if (li_realizado < 0)
                    break;

            }

            int li_fuera_plazo = 0;
            //Actualizar estado envio 
            if (li_realizado > 0)
            {
                if (ddl_tipoPrograma.SelectedValue.Equals("1")) //Diario
                {
                    if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, 16, 1))
                    {
                        li_realizado = demandaService.nf_upd_env_estado(li_valor, 2, in_app.is_UserLogin);// 2 valor por defecto (Procesado)
                    }
                    else
                    {
                        li_realizado = demandaService.nf_upd_env_estado(li_valor, 5, in_app.is_UserLogin);// 5 (Fuera de Plazo)
                        ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA FUERA DE PLAZO");
                        li_fuera_plazo = 1;
                    }
                }
                else if (ddl_tipoPrograma.SelectedValue.Equals("2")) //Semanal
                {
                    if (Util.DiferenciaFecha.HoraValida(EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)), 16, 2))
                    {
                        li_realizado = demandaService.nf_upd_env_estado(li_valor, 2, in_app.is_UserLogin);// 2 valor por defecto (Procesado)
                    }
                    else
                    {
                        li_realizado = demandaService.nf_upd_env_estado(li_valor, 5, in_app.is_UserLogin);// 5 (Fuera de Plazo)
                        ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA FUERA DE PLAZO");
                        li_fuera_plazo = 1;
                    }
                }
                
            }

            if (li_realizado > 0)
            {
                demandaService.nf_upd_env_copiado(li_valor, true, in_app.is_UserLogin);
            }

            if (li_realizado <= 0)
            {
                ListBox1.Items.Add("SE PRESENTÓ PROBLEMAS AL MOMENTO DE ENVIAR LA INFORMACIÓN");
            }
            else
            {
                if (li_fuera_plazo < 1)
                {
                    ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA EXITÓSAMENTE"); 
                }
                

                if (stopw.Elapsed.TotalSeconds <= 60)
                {
                    ListBox1.Items.Add("Proceso de carga finalizó en " + Math.Truncate(stopw.Elapsed.TotalSeconds * 1000) / 1000 + " segundos");
                }
                else
                {
                    ListBox1.Items.Add("Proceso de carga finalizó en " + Math.Truncate(stopw.Elapsed.TotalMinutes * 1000) / 1000 + " minutos");
                }
            }

            ln_conex_ora.Close();

            stopw.Stop();

        }

        private int nf_get_add_ratio(int ai_earcodi, int ai_eaicodi, int ai_ninformado, int ai_ntotal, double ad_ratio)
        {
            int li_codigo = in_app.iL_data[0].nf_GetMaxCodi();
            string ls_sql = String.Empty;
            ls_sql = "insert into EXT_RATIO (ERATCODI, EARCODI, EAICODI, ERATTOTINF, ERATENVINF,ERATRATIO,LASTDATE) values ( ";
            ls_sql += li_codigo + ", ";
            ls_sql += ai_earcodi + ", ";
            ls_sql += ai_eaicodi + ", ";
            ls_sql += ai_ninformado + ", ";
            ls_sql += ai_ntotal + ", ";
            ls_sql += ad_ratio + ", sysdate)";
            return in_app.iL_data[0].nf_ExecuteNonQuery(ls_sql);
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

        private DateTime nf_get_fechaValida(int ai_fechaNumero, string as_nombreHoja, out bool ab_fechavalida)
        {
            DateTime ldt_fecha;
            ldt_fecha = EPDate.ExcelSerialDateToDMY(ai_fechaNumero);
            ab_fechavalida = false;

            if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
            {
                if (ldt_fecha.ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"])))
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la fecha seleccionada.", "Error"));
                    ab_fechavalida = false;
                }
            }
            else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
            {
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1);
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]))).AddDays(1);
                DateTime ldt_fechaSabado = EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]));
                DateTime fecha = EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);
                if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")) && ldt_fecha.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la semana correspondiente ó con un día Martes.", "Error"));
                    ab_fechavalida = false;
                }
            }

            return ldt_fecha;
        }

        private DateTime nf_get_fechaValida(string ls_fecha, string as_nombreHoja, out bool ab_fechavalida)
        {
            DateTime ldt_fecha = new DateTime(2000, 1, 1);
            bool lb_fechavalida = EPDate.IsDate(ls_fecha);

            if (lb_fechavalida)
            {
                ldt_fecha = EPDate.ToDate(ls_fecha);
            }
            else
            {
                ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " debe tener formato dd/MM/aaaa.", "Error"));
                ab_fechavalida = false;
                return ldt_fecha;
            }

            ab_fechavalida = false;

            if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
            {
                if (ldt_fecha.ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"])))
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la fecha seleccionada.", "Error"));
                    ab_fechavalida = false;
                }
            }
            else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
            {
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1);
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]))).AddDays(1);
                DateTime ldt_fechaSabado = EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]));
                DateTime fecha = EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);
                if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")) && ldt_fecha.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la semana correspondiente ó con un día Martes.", "Error"));
                    ab_fechavalida = false;
                }
            }

            return ldt_fecha;
        }

        private DataTable nf_get_puntosMedicionxEmpresa()
        {
            COES.MVC.Extranet.wsDemanda.DemandaClient wsdemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();
            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
            DataTable dataBarras = wsdemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);

            if (dataBarras.Rows.Count > 0)
            {
                return dataBarras;
            }
            else
            {
                ListBox1.Items.Add(new ListItem("ERROR: No se puede obtener códigos por empresa", "Error"));
                return null;
            }
            
        }

        private static bool IsValidName(string ps_wsName)
        {
            if ((ps_wsName.ToUpper() == "HISTORICO-MW") || (ps_wsName.ToUpper() == "HISTÓRICO-MW") || 
                (ps_wsName.ToUpper() == "HISTORICO-MVAR") || (ps_wsName.ToUpper() == "HISTÓRICO-MVAR") ||
                (ps_wsName.ToUpper() == "PREVISTO-MW"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void DDLNumSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox1.Text = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
            tBoxFecha.Attributes["value"] = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
        }

        protected void ddl_tipoPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox1.Text = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");
            tBoxFecha.Attributes["value"] = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");

            if (ddl_tipoPrograma.SelectedItem.Value == "1")
            {
                //if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                //{
                    //TextBox1.Enabled = true;
                    tBoxFecha.Disabled = false;
                //}
                DDLNumSemana.Enabled = false;
                DDLNumSemana.Visible = false;
                lblNumSem.Enabled = false;
                lblNumSem.Visible = false;
            }
            else if (ddl_tipoPrograma.SelectedItem.Value == "2")
            {
                //if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                //{
                    //TextBox1.Enabled = true;
                    //tBoxFecha.Disabled = false;
                tBoxFecha.Attributes["value"] = String.Empty;
                tBoxFecha.Disabled = true;
                //CalendarExtender1.Enabled = false;
                DDLNumSemana.Enabled = true;
                DDLNumSemana.Visible = true;
                lblNumSem.Enabled = true;
                lblNumSem.Visible = true;


                DDLNumSemana.Text = Convert.ToString(EPDate.XWeekNumber_Entire4DayWeekRule(DateTime.Now.AddDays(7).Date)).PadLeft(2, '0');
                tBoxFecha.Attributes["value"] = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
                //}
            }
        }

        private bool ValidateDemandaXLS(DataTable pdt_ptosMedicionxEmpresa, DateTime pdt_fecha, int pi_columna, int pi_codigoTipoPrograma, int pi_codigoTipoEmpresa, DataTable dt, string ps_sheetName, List<Pronostico_Demanda> demandas)
        {
            bool lb_valida = true;
            int pi_codigoPuntoMedicion = 0;
            int pi_codigoLectura = 0;
            int pi_tipoInformacion = 0;

            if (Int32.TryParse(dt.Rows[1][pi_columna].ToString(), out pi_codigoPuntoMedicion))
            {
                if (pi_codigoPuntoMedicion > 0)
                {
                    if (Util.Columnas.GetExistingCode(pi_codigoPuntoMedicion, pdt_ptosMedicionxEmpresa))
                    {
                        if (Int32.TryParse(dt.Rows[2][pi_columna].ToString(), out pi_codigoLectura))
                        {
                            if (pi_codigoTipoPrograma == 1) //PDO
                            {
                                if (ps_sheetName.ToUpper().Equals("HISTÓRICO-MW") || ps_sheetName.ToUpper().Equals("HISTORICO-MW"))
                                {
                                    if (pi_codigoLectura == 45)
                                    {
                                        if (Int32.TryParse(dt.Rows[3][pi_columna].ToString(), out pi_tipoInformacion))
                                        {
                                            if (pi_tipoInformacion == 20)
                                            {
                                                //Valores Validos para HISTÓRICO-MW - PDO
                                                nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                     pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                            }
                                            else
                                            {
                                                f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información. Código Erróneo: " + pi_tipoInformacion);
                                                lb_valida = false;
                                            }
                                        }
                                        else
                                        {
                                            f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información");
                                            lb_valida = false;
                                        }
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 3, pi_columna, "En el código de lectura");
                                        lb_valida = false;
                                    }
                                }
                                else if (ps_sheetName.ToUpper().Equals("HISTÓRICO-MVAR") || ps_sheetName.ToUpper().Equals("HISTORICO-MVAR"))
                                {
                                    if (pi_codigoLectura == 45)
                                    {
                                        if (Int32.TryParse(dt.Rows[3][pi_columna].ToString(), out pi_tipoInformacion))
                                        {
                                            if (pi_tipoInformacion == 48)
                                            {
                                                //Valores Validos para HISTÓRICO-MVAR - PDO
                                                nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                     pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                            }
                                            else
                                            {
                                                f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información. Código Erróneo: " + pi_tipoInformacion);
                                                lb_valida = false;
                                            }
                                        }
                                        else
                                        {
                                            f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información");
                                            lb_valida = false;
                                        }
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 3, pi_columna, "En el código de lectura");
                                        lb_valida = false;
                                    }
                                }
                                else if (ps_sheetName.ToUpper().Equals("PREVISTO-MW"))
                                {
                                    if (pi_codigoLectura == 46)
                                    {
                                        if (Int32.TryParse(dt.Rows[3][pi_columna].ToString(), out pi_tipoInformacion))
                                        {
                                            if (pi_tipoInformacion == 20)
                                            {
                                                //Valores Validos para PREVISTO-MW - PDO
                                                nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                     pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                            }
                                            else
                                            {
                                                f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información. Código Erróneo: " + pi_tipoInformacion);
                                                lb_valida = false;
                                            }
                                        }
                                        else
                                        {
                                            f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información");
                                            lb_valida = false;
                                        }
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 3, pi_columna, "En el código de lectura");
                                        lb_valida = false;
                                    }
                                }
                            }
                            else if (pi_codigoTipoPrograma == 2) //PSO 
                            {
                                if (ps_sheetName.ToUpper().Equals("PREVISTO-MW"))
                                {
                                    if (pi_codigoLectura == 47)
                                    {
                                        if (Int32.TryParse(dt.Rows[3][pi_columna].ToString(), out pi_tipoInformacion))
                                        {
                                            if (pi_tipoInformacion == 20)
                                            {
                                                //Valores Validos para PREVISTO-MW - PSO
                                                nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                     pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                            }
                                            else
                                            {
                                                f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información. Código Erróneo: " + pi_tipoInformacion);
                                                lb_valida = false;
                                            }
                                        }
                                        else
                                        {
                                            f_set_linea_listBox(ps_sheetName, 4, pi_columna, "En el tipo de información");
                                            lb_valida = false;
                                        }
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 3, pi_columna, "En el código de lectura");
                                        lb_valida = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, 3, pi_columna, "En el código de lectura");
                            lb_valida = false;
                        }
                    }
                    else
                    {
                        f_set_linea_listBox(ps_sheetName, 2, pi_columna, "En el código del punto de medición (" + pi_codigoPuntoMedicion +") no pertenece a la empresa seleccionada ");
                        lb_valida = false;
                    }
                }
                else
                {
                    f_set_linea_listBox(ps_sheetName, 2, pi_columna, "En el código del punto de medición: " + pi_codigoPuntoMedicion);
                    lb_valida = false;
                }
            }
            else
            {
                f_set_linea_listBox(ps_sheetName, 2, pi_columna, "En el código del punto de medición");
                lb_valida = false;
            }

            return lb_valida;
        }

        private void nf_valida_valores_48(DateTime pdt_fecha, string ps_sheetName, int pi_codigoPuntoMedicion, int pi_codigoLectura, int pi_tipoInformacion, int pi_columna, int pi_codigoTipoEmpresa, int pi_codigoTipoPrograma, List<Pronostico_Demanda> demandas, DataTable dt, out bool pb_valida)
        {
            double ld_valor = 0;
            pb_valida = true;
            Pronostico_Demanda demanda = new Pronostico_Demanda();

            if (pi_codigoTipoPrograma == 1) //PDO
            {
                if (pi_codigoTipoEmpresa == 2) //DISTR
                {
                    demanda.ld_array_demanda48 = new double[48];
                    for (int i = 22; i < 70; i++)
                    {
                        if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                        {
                            if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                            {
                                demanda.ld_array_demanda48[i - 22] = Math.Round(ld_valor, 5);
                            }
                            else
                            {
                                f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Valor no válido : '" + dt.Rows[i][pi_columna].ToString() + "'");
                                pb_valida = false;
                            }
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Celda vacía"); 
                            pb_valida = false;
                        }
                    }

                    if (pb_valida) 
                    {
                        demanda.Codigo = pi_codigoPuntoMedicion;
                        demanda.LectoCodi = pi_codigoLectura;
                        demanda.TipoInfo = pi_tipoInformacion;

                        if (pi_codigoLectura == 45)
                        {
                            demanda.Fecha = pdt_fecha.AddDays(-1);
                        }
                        else if (pi_codigoLectura == 46)
                        {
                            demanda.Fecha = pdt_fecha.AddDays(+1);
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, 16, pi_columna, "Fecha inválida" + pdt_fecha.ToString("dd/MM/yyyy"));
                            pb_valida = false;
                        }

                        demandas.Add(demanda);

                    }
                }
                else if (pi_codigoTipoEmpresa == 4) //ULIBRE
                {
                    demanda.ld_array_demanda48 = new double[48];
                    for (int i = 27; i < 75; i++)
                    {
                        if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                        {
                            if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                            {
                                demanda.ld_array_demanda48[i - 27] = Math.Round(ld_valor, 5);
                            }
                            else
                            {
                                f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Valor no válido : '" + dt.Rows[i][pi_columna].ToString() + "'");
                                pb_valida = false;
                            }
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Celda vacía");
                            pb_valida = false;
                        }
                    }

                    if (pb_valida)
                    {
                        demanda.Codigo = pi_codigoPuntoMedicion;
                        demanda.LectoCodi = pi_codigoLectura;
                        demanda.TipoInfo = pi_tipoInformacion;

                        if (pi_codigoLectura == 45)
                        {
                            demanda.Fecha = pdt_fecha.AddDays(-1);
                        }
                        else if (pi_codigoLectura == 46)
                        {
                            demanda.Fecha = pdt_fecha.AddDays(+1);
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, 16, pi_columna, "Fecha inválida" + pdt_fecha.ToString("dd/MM/yyyy"));
                            pb_valida = false;
                        }

                        demandas.Add(demanda);
                    }
                }
                else
                {
                    f_set_linea_listBox(ps_sheetName, -1, "No es empresa de distribución ó cliente libre");
                    pb_valida = false;
                }
            }
            else if (pi_codigoTipoPrograma == 2) //PSO
            {
                double[] ld_array_valores = new double[48];
                DateTime ldt_fechaInicioSem = EPDate.f_fechafinsemana(pdt_fecha);

                if (pi_codigoTipoEmpresa == 2) //DISTR
                {
                    for (int i = 22; i < 358; i++)
                    {
                        if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                        {
                            if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                            {
                                ld_array_valores[(i - 22) % 48] = Math.Round(ld_valor, 5);

                                if ((i - 22) % 48 == 47)
                                {
                                    demanda.Codigo = pi_codigoPuntoMedicion;
                                    demanda.LectoCodi = pi_codigoLectura;
                                    demanda.TipoInfo = pi_tipoInformacion;

                                    if (pi_codigoLectura == 47)
                                    {
                                        demanda.Fecha = ldt_fechaInicioSem.AddDays((int)i / 48);
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 16, pi_columna, "Fecha inválida" + pdt_fecha.ToString("dd/MM/yyyy"));
                                        pb_valida = false;
                                    }

                                    demanda.ld_array_demanda48 = new double[48];
                                    demanda.ld_array_demanda48 = ld_array_valores;
                                    ld_array_valores = new double[48];
                                    demandas.Add(demanda);
                                    demanda = new Pronostico_Demanda();
                                }
                            }
                            else
                            {
                                f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Valor no válido : '" + dt.Rows[i][pi_columna].ToString() + "'");
                                pb_valida = false;
                            }
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Celda vacía");
                            pb_valida = false;
                        }
                    }
                }
                else if (pi_codigoTipoEmpresa == 4) //ULIBRE
                {
                    for (int i = 27; i < 363; i++)
                    {
                        if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                        {
                            if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                            {
                                ld_array_valores[(i - 27) % 48] = Math.Round(ld_valor, 5);

                                if ((i - 27) % 48 == 47)
                                {
                                    demanda.Codigo = pi_codigoPuntoMedicion;
                                    demanda.LectoCodi = pi_codigoLectura;
                                    demanda.TipoInfo = pi_tipoInformacion;

                                    if (pi_codigoLectura == 47)
                                    {
                                        demanda.Fecha = ldt_fechaInicioSem.AddDays((int)i / 48);
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 16, pi_columna, "Fecha inválida" + pdt_fecha.ToString("dd/MM/yyyy"));
                                        pb_valida = false;
                                    }

                                    demanda.ld_array_demanda48 = new double[48];
                                    demanda.ld_array_demanda48 = ld_array_valores;
                                    ld_array_valores = new double[48];
                                    demandas.Add(demanda);
                                    demanda = new Pronostico_Demanda();
                                }
                            }
                            else
                            {
                                f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Valor no válido : '" + dt.Rows[i][pi_columna].ToString() + "'");
                                pb_valida = false;
                            }
                        }
                        else
                        {
                            f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Celda vacía");
                            pb_valida = false;
                        }
                    }
                }
                else
                {
                    f_set_linea_listBox(ps_sheetName, -1, "No es empresa de distribución ó cliente libre");
                    pb_valida = false;
                }
            }
            else
            {
                f_set_linea_listBox(ps_sheetName, -1, "En el tipo de programa");
                pb_valida = false;
            }
        }

        private void f_set_linea_listBox(string ps_nameSheet, int pi_fila, string ps_mensaje)
        {
            string ls_mensaje = String.Empty;
            if (pi_fila != -1)
            {
                
            }
            else
            {
                ls_mensaje = "Error : Hoja " + ps_nameSheet + " " + ps_mensaje;
            }
            
            //ListBox1.Items.Add(ls_mensaje);
            ListBox1.Items.Add(new ListItem(ls_mensaje, "Error"));
        }

        private void f_set_linea_listBox(string ps_nameSheet,int pi_fila, int pi_columna, string ps_mensaje)
        {
            string ls_mensaje = "Error : Hoja " + ps_nameSheet + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + pi_fila.ToString() + ", " + ps_mensaje;
            ListBox1.Items.Add(new ListItem(ls_mensaje, "Error"));
            //ListBox1.Items.Add(ls_mensaje);
        }

        protected void btn_exporta_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();

            foreach (ListItem item in ListBox1.Items)
            {
                sb.AppendLine(item.ToString());
            }


            Response.BufferOutput = true;
            Response.Clear();
            Response.ClearHeaders();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", "attachment; filename=log_Carga" + Convert.ToString(tBoxFecha.Attributes["value"]).Replace("/", ".") + ".txt");
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();


        }
    }
}