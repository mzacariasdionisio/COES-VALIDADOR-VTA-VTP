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
using ExcelLibrary.SpreadSheet;
using System.Diagnostics;
using WSIC2010.Util;
using System.Configuration;
using fwapp;

namespace WSIC2010.Demanda
{
    public partial class w_me_demanda_new : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1,2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        Stopwatch stopw = new Stopwatch();

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
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, ddl_tipoPrograma.SelectedValue, fileName.Substring(0, (fileName.Length - 4)), extension, out ls_formatoNombre, DateTime.Now.ToString("dd/MM/yyyy"));//TextBox1.Text);
                        }
                        else if (extension.Equals(".xlsx"))
                        {
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue, DDLNumSemana.SelectedValue, ddl_tipoPrograma.SelectedValue, fileName.Substring(0, (fileName.Length - 5)), extension, out ls_formatoNombre, DateTime.Now.ToString("dd/MM/yyyy"));//TextBox1.Text);
                        }

                        //Restringir extension de archivos
                        if (Util.Extension.IsExcel(extension, fileName, out ls_nombreExcel, ls_basePath, out ls_strConn) && lb_nombre)
                        {
                            ls_path = ls_basePath + ls_nombreExcel;

                            //Guardar en el servidor
                            FileUpload1.SaveAs(ls_path);

                            //Instaciamos seguridad 
                            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                            IDemandaService demandaServ = new DemandaService();
                            double fileMB = Util.SizeFile.SizeOfFile(sizeFile);
                            string cadena = ls_path.Substring(16);

                            DataSet output = new DataSet();

                            if (ddl_tipoPrograma.SelectedValue.Equals("1"))
                            {
                                if (Util.DiferenciaFecha.HoraValida(DateTime.Now, 23))
                                {
                                    if (fileName.Contains("PDO"))
                                    {
                                        //int pi_valor_pdo = wsExtranet.AgregarEnvArchivo(2, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                        int pi_valor_pdo = demandaServ.nf_set_insert_archivo_envio(2, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(TextBox1.Attributes["value"])), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));

                                        if (pi_valor_pdo >= 1)
                                        {
                                            LoadProgramaData(ls_path, pi_valor_pdo);
                                        }
                                        else
                                        {
                                            ListBox1.Items.Add("ERROR: Existen problemas al conectarse al servidor. Contactese con el Administrador");
                                        }
                                    }
                                    else
                                    {
                                        ListBox1.Items.Add(new ListItem("El documento no corresponde con el Programa Diario de Operación (PDO)", "Error"));
                                    }
                                }
                                else
                                {
                                    ListBox1.Items.Add("El envio de información del PDO se hará hasta las 8 horas de cada día");
                                }
                            }
                            else if (ddl_tipoPrograma.SelectedValue == "2")
                            {
                                if ((Util.DiferenciaFecha.HoraValida(DateTime.Now, 23)))
                                {
                                    if (fileName.Contains("PSO"))
                                    {
                                        //int pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                        int pi_valor_pso = demandaServ.nf_set_insert_archivo_envio(3, cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(Convert.ToString(TextBox1.Attributes["value"])), 1, "N", Convert.ToInt32(DDLEmpresa.SelectedValue));
                                        if (pi_valor_pso >= 1)
                                        {
                                            LoadProgramaData(ls_path, pi_valor_pso);
                                        }
                                        else
                                        {
                                            ListBox1.Items.Add(new ListItem("ERROR: Existen problemas al conectarse al servidor. Contactese con el Administrador", "Error"));
                                        }
                                    }
                                    else
                                    {
                                        ListBox1.Items.Add(new ListItem("El documento " + fileName  + "No corresponde con el Programa Semanal de Operación (PSO)", "Error"));
                                    }
                                }
                                else
                                {
                                    ListBox1.Items.Add("El envio de información del PSO se hará hasta las 8 horas de cada día martes");
                                }
                            }
                        }
                        else
                        {
                            ListBox1.Items.Add(new ListItem("ERROR: El archivo " + fileName + " no corresponde con el tipo de programa o no corresponde a un archivo Excel", "Error"));
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

        private void LoadProgramaData(string as_path, int ai_earcodi)
        {
            stopw.Start();
            ListBox1.Items.Add("Proceso de verificación iniciado a las " + DateTime.Now.ToString("H:mm:ss") + " del día " + DateTime.Now.ToString("dd/MM/yyyy"));
            List<Pronostico_Demanda> AL_demandas = new List<Pronostico_Demanda>();
            DateTime ldt_fechaInicioSem = new DateTime(2000, 1, 1);
            DataTable dt_barrasxEmpresa = nf_get_barrasxEmpresa();
            Workbook wb = Workbook.Load(as_path);
            string ls_cabecera = String.Empty;
            DateTime ldt_fecha = new DateTime(2000, 1, 1);

            foreach (var item in wb.Worksheets)
            {
                int li_contador = 0;

                while (!String.IsNullOrEmpty(item.Cells[0, li_contador].StringValue))
                {
                    ls_cabecera = item.Cells[0, li_contador].StringValue;
                    Pronostico_Demanda demanda;
                    bool lb_fechavalida = true;

                    if (Convert.ToInt32(ls_cabecera) > 0)
                    {
                        if (li_contador == 1)
                        {
                            ldt_fecha = nf_get_fechaValida(Convert.ToInt32(item.Cells[15, li_contador].Value), item.Name, out lb_fechavalida);
                            ListBox1.Items.Add("Accediendo a la hoja: " + item.Name);
                            li_contador++;
                            continue;
                        }

                        if (lb_fechavalida)
                        {
                            demanda = new Pronostico_Demanda();
                            if (ValidaPtoMedicion(item, li_contador, demanda, ldt_fecha, dt_barrasxEmpresa))
                            {
                                AL_demandas.Add(demanda);
                            }
                        }
                    }

                    li_contador++;
                }

            }

            bool lb_errores = false;

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Value.Trim().TrimEnd().ToUpper().Equals("ERROR"))
                {
                    lb_errores = true;
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

                nf_set_SaveDemandas(AL_demandas, ai_earcodi);

                #endregion

            }
            else
            {
                ListBox1.Items.Add("NO SE PUEDE CARGAR INFORMACIÓN: Existen errores en la carga ");
            }
            
            
        }

        private void nf_set_SaveDemandas(List<Pronostico_Demanda> al_demandas, int ai_earcodi)
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
            int li_valor = ai_earcodi;

            string ls_comando = String.Empty;
            string ls_cadena = String.Empty;
            string ls_cadenaCodigos = String.Empty;
            double ld_meditotal = 0;

            for (int i = 0; i < al_demandas.Count; i++)
            {
                //Para los PDO 
                if ((!al_demandas[i].Codigo.Equals(0)) && (al_demandas[i].LectoCodi.Equals(45)) && (al_demandas[i].FuenteInfo != null) && i % 2 == 0)
                {
                    li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 2, al_demandas[i].Codigo, EPDate.ToDate(TextBox1.Text), al_demandas[i].FuenteInfo, ref ln_conex_ora);
                    if (li_realizado > 0)
                    {
                        li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                    }
                }

                //Para los PSO 
                if ((!al_demandas[i].Codigo.Equals(0)) && (al_demandas[i].LectoCodi.Equals(47)) && (al_demandas[i].FuenteInfo != null) && i % 7 == 0)
                {
                    li_realizado = ln_demanda.AgregarDemandaFuente(li_valor, 3, al_demandas[i].Codigo, EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1), al_demandas[i].FuenteInfo, ref ln_conex_ora);
                    if (li_realizado > 0)
                    {
                        li_realizado = ln_demanda.Agregar_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0), ref ln_conex_ora);
                    }
                }


                if (li_realizado >= 0)
                {
                    /*Borrando datos*/
                    ls_comando = " DELETE FROM ME_MEDICION96";
                    ls_comando += " WHERE TIPOINFOCODI = " + al_demandas[i].InfoCodi;
                    ls_comando += " AND LECTCODI = " + al_demandas[i].LectoCodi;
                    ls_comando += " AND MEDIFECHA = TO_DATE('" + al_demandas[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
                    ls_comando += " AND PTOMEDICODI = " + al_demandas[i].Codigo;
                    li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                    if (!String.IsNullOrEmpty(ls_cadena))
                    {
                        ListBox1.Items.Add(new ListItem(ls_cadena, "Error"));
                    }

                    /*Insertando Datos*/
                    if (li_realizado >= 0)
                    {
                        ls_comando = "INSERT INTO ME_MEDICION96 (MEDIFECHA, LECTCODI, TIPOINFOCODI, PTOMEDICODI,";
                        ls_comando += "H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,";
                        ls_comando += "H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,H49,H50,H51,H52,H53,H54,H55,H56,H57,H58,H59,H60,";
                        ls_comando += "H61,H62,H63,H64,H65,H66,H67,H68,H69,H70,H71,H72,H73,H74,H75,H76,H77,H78,H79,H80,H81,H82,H83,H84,H85,H86,H87,H88,H89,H90,H91,H92,H93,H94,H95,H96,MEDITOTAL) ";
                        ls_comando += "VALUES (TO_DATE('" + al_demandas[i].Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')," + al_demandas[i].LectoCodi + "," + al_demandas[i].InfoCodi + "," + al_demandas[i].Codigo;
                        ls_comando += nf_get_string_Valores(al_demandas[i].ld_array_demanda96, out ld_meditotal);
                        ls_comando += "," + ld_meditotal;
                        ls_comando += ")";
                        li_realizado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);

                        if (!String.IsNullOrEmpty(ls_cadena))
                        {
                            ListBox1.Items.Add(new ListItem(ls_cadena, "Error"));
                        }
                    }
                }

                if (li_realizado < 0)
                    break;

            }

            if (li_realizado > 0)
            {
                li_realizado = demandaService.nf_upd_env_estado(li_valor, 1, in_app.is_UserLogin);// 1 valor por defecto (Enviado)
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
                ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA EXITÓSAMENTE");

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

        private bool ValidaPtoMedicion(Worksheet item, int li_contador, Util.Pronostico_Demanda ad_demanda, DateTime adt_fecha, DataTable adt_barras)
        {
            bool lb_valido = true; ;

            if (Util.Columnas.GetExistingCode(Convert.ToInt32(item.Cells[1, li_contador].StringValue), adt_barras))
            {
                ad_demanda.Codigo = Convert.ToInt32(item.Cells[1, li_contador].StringValue);
                int ai_tipoPrograma;
                string ls_fuenteInformacion;
                if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
                {
                    if (int.TryParse(item.Cells[2, li_contador].StringValue, out ai_tipoPrograma) && nf_get_bool_Programa(Convert.ToInt32(ddl_tipoPrograma.SelectedItem.Value), Convert.ToInt32(item.Cells[2, li_contador].StringValue)))
                    {
                        switch (ai_tipoPrograma)
                        {
                            case 45:
                                ad_demanda.LectoCodi = ai_tipoPrograma;
                                ad_demanda.Fecha = adt_fecha.AddDays(-1);
                                break;
                            case 46:
                                ad_demanda.LectoCodi = ai_tipoPrograma;
                                ad_demanda.Fecha = adt_fecha.AddDays(1);
                                break;
                            default:
                                ad_demanda.LectoCodi = 0;
                                ad_demanda.Fecha = new DateTime(2000, 1, 1);
                                break;
                        }
                    }
                    else
                    {
                        lb_valido = false;
                        ListBox1.Items.Add(new ListItem("ERROR: En la hoja " + item.Name + ", Se cambió formato original. Código Tipo de Programa inválido", "Error"));
                    }
                }
                else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
	            {
                    if (int.TryParse(item.Cells[2, li_contador].StringValue, out ai_tipoPrograma) && nf_get_bool_Programa(Convert.ToInt32(ddl_tipoPrograma.SelectedItem.Value), Convert.ToInt32(item.Cells[2, li_contador].StringValue)))
                    {
                        ad_demanda.LectoCodi = ai_tipoPrograma;
                        ad_demanda.Fecha = EPDate.f_fechafinsemana(adt_fecha).AddDays(li_contador - 1);
                    }
                    else
                    {
                        lb_valido = false;
                        ListBox1.Items.Add(new ListItem("ERROR: En la hoja " + item.Name + ", Se cambió formato original. Código Tipo de Programa inválido", "Error"));
                    }
	            }
                
                ls_fuenteInformacion = item.Cells[23, 2].StringValue.TrimStart().TrimEnd();


                if (ls_fuenteInformacion.Equals("M") || ls_fuenteInformacion.Equals("S"))
                {
                    ad_demanda.FuenteInfo = ls_fuenteInformacion;

                    ad_demanda.Lugar = item.Cells[18, 2].StringValue;
                    ad_demanda.Carga = item.Cells[19, 2].StringValue;
                    ad_demanda.Descripcion = item.Cells[20, 2].StringValue;
                    ad_demanda.CodigoBarra = item.Cells[21, 2].StringValue;
                    ad_demanda.TensionBarra = item.Cells[22, 2].StringValue;
                }
                else
                {
                    lb_valido = false;
                    if (li_contador == 2)
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: En la hoja " + item.Name + " Se debe asignar 'S' para DATO SCADA y 'M' para DATO MEDIDOR (Mayúsculas).", "Error"));
                    }
                    
                }

                ad_demanda.ld_array_demanda96 = nf_get_arrayDatos(item.Cells, li_contador, item.Name);
            }
            else
            {
                lb_valido = false;
                ListBox1.Items.Add(new ListItem("ERROR: En la hoja " + item.Name + ", Se cambio formato original. Codigo Barra no existe", "Error"));
                //Send Mail
            }

            return lb_valido;
        }

        private bool nf_get_bool_Programa(int ai_tipoPrograma, int ai_valor)
        {
            bool lb_retorno = false;

            if (ai_tipoPrograma == 1)
            {
                if ((ai_valor == 45)||(ai_valor == 46))
                {
                    return true;
                }
            }
            else if (ai_tipoPrograma == 2)
            {
                if (ai_valor == 47)
                {
                    return true;
                }
            }

            return lb_retorno;
        }

        private DataTable nf_get_barrasxEmpresa()
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

        private DateTime nf_get_fechaValida(int ai_fechaNumero, string as_nombreHoja, out bool ab_fechavalida)
        {
            DateTime ldt_fecha;
            ldt_fecha = EPDate.ExcelSerialDateToDMY(ai_fechaNumero);
            ab_fechavalida = false;

            if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
            {
                if (ldt_fecha.ToString("dd/MM/yyyy").Equals(TextBox1.Text))
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda B:16 en la hoja " + as_nombreHoja + " no coincide con la fecha seleccionada.", "Error"));
                    ab_fechavalida = false;
                }
            }
            else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
            {
                DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1);
                DateTime fecha = EPDate.f_fechafinsemana(ldt_fecha).AddDays(1);
                if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")) && ldt_fecha.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda B:16 en la hoja " + as_nombreHoja + " no coincide con la semana correspondiente ó con un día Martes.", "Error"));
                    ab_fechavalida = false;
                }
            }

            return ldt_fecha;
        }

        private double[] nf_get_arrayDatos(CellCollection ascc_cells, int ai_contador, string as_nameSheet)
        {
            double[] ld_array_valores = new double[96];
            double ld_valor;

            for (int i = 27; i < 122; i++)
            {
                if (double.TryParse(ascc_cells[i, ai_contador].StringValue, out ld_valor))
                {
                    if (ld_valor >= 0)
                    {
                        ld_array_valores[i - 27] = Math.Truncate(ld_valor * 1000) / 1000;
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: Valor negativo, hoja " + as_nameSheet + " celda " + Util.ExcelUtil.GetExcelColumnName(ai_contador + 1) + (i + 1), "Error"));
                    }
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: Valor inválido, hoja " + as_nameSheet + " celda " + Util.ExcelUtil.GetExcelColumnName(ai_contador + 1) + (i + 1), "Error"));
                }
            }

            return ld_array_valores;
        }

        protected void ddl_tipoPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox1.Items.Clear();
            ListBox1.DataSource = null;
            ListBox1.DataBind();
            TextBox1.Text = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");

            if (ddl_tipoPrograma.SelectedItem.Value == "1")
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
            else if (ddl_tipoPrograma.SelectedItem.Value == "2")
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