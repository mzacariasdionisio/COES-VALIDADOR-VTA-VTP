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
using COES.Servicios.Aplicacion.DemandaBarras;
using COES.Dominio.DTO.Sic;

namespace WSIC2010.Demanda
{
    public partial class w_me_carga_ult : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        Stopwatch stopw = new Stopwatch();
        DateTime ldt_expectedDate;

       
        public int NroHorasValidacionDiario
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hfPlazoDiario.Value))
                {
                    return Convert.ToInt32(this.hfPlazoDiario.Value);
                }
                return 0;
            }
            set
            {
                this.hfPlazoDiario.Value = value.ToString();
            }
        }

        public int NroHorasValidacionSemanal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hfPlazoSemanal.Value))
                {
                    return Convert.ToInt32(this.hfPlazoSemanal.Value);
                }
                return 0;
            }
            set
            {
                this.hfPlazoSemanal.Value = value.ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/demanda/w_me_carga_ult.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();
                DateTime ldt_fecha = new DateTime(2013, 1, 1);

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    if (!IsPostBack)
                    {

                        COES.MVC.Extranet.wsDemanda.DemandaClient wsDemanda = new COES.MVC.Extranet.wsDemanda.DemandaClient();

                        Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
                        string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
                        //string[] array_Empresas = in_app.Ls_emprcodi.ToArray();

                        string[] array_Empresas = (new COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient()).ObtenerEmpresasActivasPorUsuario(User.Identity.Name).
                        Select(x => x.EMPRCODI.ToString()).ToArray();

                        DataTable dtEmpresas = wsDemanda.EmpresasRepxUsuario(array_Empresas, ls_credencial);

                        DataView dv = dtEmpresas.DefaultView;
                        dv.Sort = "emprnomb asc";
                        DataTable sortedDT = dv.ToTable();

                        DDLEmpresa.DataSource = sortedDT;
                        DDLEmpresa.DataValueField = dtEmpresas.Columns[0].ToString();
                        DDLEmpresa.DataTextField = dtEmpresas.Columns[1].ToString();
                        DDLEmpresa.DataBind();

                        ddl_tipoPrograma.Items.Add(new ListItem("Pronóstico de Demanda Diaria", "1"));

                        //if (DateTime.Now.DayOfWeek != DayOfWeek.Wednesday) //Excluye dias miercoles
                        //{
                            ddl_tipoPrograma.Items.Add(new ListItem("Pronóstico Demanda Semanal", "2"));
                        //}

                        tBoxFecha.Attributes["value"] = DateTime.Now.ToString("dd/MM/yyyy");

                        DDLNumSemana.DataSource = Util.CargaDDLSemanas.LlenaSemanasTemporal(DateTime.Now.Date, out ldt_fecha);
                        DDLNumSemana.DataValueField = "Key";
                        DDLNumSemana.DataTextField = "Value";
                        DDLNumSemana.DataBind();

                        if (li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                        {
                            tBoxFecha.Disabled = false;
                        }

                        this.CargarDatosPlazo();
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

            List<Pronostico_Demanda2> AL_demandas_Total = new List<Pronostico_Demanda2>();

            #region Carga Archivos
            try
            {
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
                            lb_nombre = Util.Valida.NombreArchivo(DDLEmpresa.SelectedValue,DDLNumSemana.SelectedValue, ddl_tipoPrograma.SelectedValue, fileName.Substring(0, (fileName.Length - 4)), ls_extension, out ls_formatoNombre, ldt_expectedDate.ToString("dd/MM/yyyy"));
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
                                if (Convert.ToInt32(DDLEmpresa.SelectedValue) == 11772)
                                    pi_codigoTipoEmpresa = 4;
                                else
                                    pi_codigoTipoEmpresa = Convert.ToInt32(ln_data.Rows[0]["TIPOEMPRCODI"].ToString());
                            }

                            double fileMB = Util.SizeFile.SizeOfFile(sizeFile);
                            string cadena = ls_path.Substring(16);

                            if (ddl_tipoPrograma.SelectedValue.Equals("1"))
                            {
                                int pi_valor_pdo = 0;
                                if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, this.NroHorasValidacionDiario, 1))
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
                            else if (ddl_tipoPrograma.SelectedValue == "2")
                            {
                                // 1 valor por defecto (Enviado)
                                //int pi_valor_pso = wsExtranet.AgregarEnvArchivo(3, Convert.ToInt32(DDLEmpresa.SelectedValue), cadena, fileMB, "1.0", ls_basePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, EPDate.ToDate(TextBox1.Text), 1, "N", ls_credencial);
                                int pi_valor_pso = 0;
                                if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now, this.NroHorasValidacionSemanal))
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
            catch (FormatException fe)
            {
                ListBox1.Items.Add(new ListItem("ERROR: Formato incorrecto de fecha en la hoja. Ingrese fecha en formato dd/mm/aaaa.", "Error"));
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
            //ListBox1.Items.Add("Proceso de verificación iniciado a las " + DateTime.Now.ToString("H:mm:ss") + " del día " + DateTime.Now.ToString("dd/MM/yyyy"));
            ListBox1.Items.Add("Proceso de verificación iniciado. (" + pi_earcodi.ToString() + ")");
            List<Pronostico_Demanda2> AL_demandas_Total = new List<Pronostico_Demanda2>();
            DateTime ldt_fechaInicioSem = new DateTime(2000, 1, 1);
            //DataTable dt_ptosMedicionxEmpresa = nf_get_puntosMedicionxEmpresa();
            DataTable dt_ptosMedicionxEmpresa = nf_get_puntosMedicionxEmpresa(pi_codigoTipoEmpresa);
            DateTime ldt_fecha = new DateTime(2000, 1, 1);


            //Workbook wb = Workbook.Load(ps_path); //XLS file
            DemandaService demandaServicio = new DemandaService();
            DataSet ds = new DataSet(); //= demandaServicio.nf_get_FileDemandaExcel(ps_path, ps_extension);
            bool lb_archivocorrupto = false;

            try
            {
                ds = ExcelReader.nf_get_excel_to_ds(ps_path, ps_extension);
            }
            catch (Exception)
            {
                ListBox1.Items.Add(new ListItem("Error archivo corrupto o en la extensión del archivo", "Error"));
                lb_archivocorrupto = true;
                stopw.Stop();
            }



            bool lb_fechavalida = false;
            bool lb_hojaValida = true;
            bool lb_errores = false;

            foreach (DataTable dt in ds.Tables)
            {
                if (!IsValidName(dt.TableName))
                {
                    ListBox1.Items.Add(new ListItem("ERROR: Nombre de Hoja Inválido '" + dt.TableName + "'", "Error"));
                }
            }

            if (!nf_get_valida_errores())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    int li_fechanumero = 0;
                    if (IsValidName(dt.TableName))
                    {
                        ListBox1.Items.Add("Accediendo a la hoja: '" + dt.TableName + "'");
                        DataRowCollection drows = dt.Rows;
                        int li_filaFecha = 0;
                        if (pi_codigoTipoEmpresa == 2)
                            li_filaFecha = 15;
                        else if (pi_codigoTipoEmpresa == 4)
                            li_filaFecha = 16;

                        if (pi_tipoPrograma == 1)
                        {
                            try
                            {
                                //Valida fecha en XLS file celda C16
                                if (pi_codigoTipoEmpresa == 2)//DISTR
                                {
                                    if (!Int32.TryParse(drows[li_filaFecha - 1][2].ToString(), out li_fechanumero))
                                        ldt_fecha = nf_get_fechaValida(EPDate.ToDateMMDDYYYY(drows[li_filaFecha - 1][2].ToString()).ToString("dd/MM/yyyy"), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                    else
                                        ldt_fecha = nf_get_fechaValida(drows[li_filaFecha - 1][2].ToString(), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                }
                                else if (pi_codigoTipoEmpresa == 4)//ULIBRE
                                {
                                    if (!Int32.TryParse(drows[li_filaFecha - 1][2].ToString(), out li_fechanumero))
                                        ldt_fecha = nf_get_fechaValida(EPDate.ToDateMMDDYYYY(drows[li_filaFecha - 1][2].ToString()).ToString("dd/MM/yyyy"), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                    else
                                        ldt_fecha = nf_get_fechaValida(drows[li_filaFecha - 1][2].ToString(), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                }

                                if (lb_fechavalida)
                                {
                                    //if ((dt.TableName.ToUpper() == "HISTORICO-MW") || (dt.TableName.ToUpper() == "HISTÓRICO-MW"))
                                    //{
                                    //if ((ldt_fecha.Date <= DateTime.Now.Date))
                                    //{
                                    //Valores Validos para HISTÓRICO-MW - PDO
                                    //lb_hojaValida = true;
                                    //}

                                    //else
                                    //{
                                    //ListBox1.Items.Add(new ListItem("LA INFORMACIÓN HISTÓRICA NO SERÁ CARGADA EN DÍAS FUTUROS"));
                                    //lb_hojaValida = false;
                                    //}
                                    //}
                                    //else if(dt.TableName.ToUpper() == "PREVISTO-MW")
                                    if (dt.TableName.ToUpper() == "PREVISTO-MW")
                                    {
                                        if ((ldt_fecha > DateTime.Now.Date))
                                        {
                                            //Valores Validos para PREVISTO-MW - PDO
                                            lb_hojaValida = true;
                                        }
                                        else
                                        {
                                            ListBox1.Items.Add(new ListItem("LA INFORMACIÓN PREVISTA NO SERÁ CARGADA EN EL MISMO DÍA O ANTERIOR"));
                                            lb_hojaValida = false;
                                        }
                                    }
                                }
                            }
                            catch (FormatException fe)
                            {

                                ListBox1.Items.Add(new ListItem("Formato de Fecha en la celda C" + li_filaFecha.ToString() + " ('" + drows[li_filaFecha - 1][2].ToString() + "') de la Hoja '" + dt.TableName + "', Inválido. Ingresar dd/mm/aa", "Error"));
                            }
                        }
                        else if (pi_tipoPrograma == 2)
                        {
                            try
                            {
                                //Valida fecha en XLS file celda C16
                                if (pi_codigoTipoEmpresa == 2)//DISTR
                                {
                                    if (!Int32.TryParse(drows[li_filaFecha - 1][2].ToString(), out li_fechanumero))
                                        ldt_fecha = nf_get_fechaValida(EPDate.ToDateMMDDYYYY(drows[li_filaFecha - 1][2].ToString()).ToString("dd/MM/yyyy"), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                    else
                                        ldt_fecha = nf_get_fechaValida(drows[li_filaFecha - 1][2].ToString(), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);

                                }
                                else if (pi_codigoTipoEmpresa == 4)//ULIBRE
                                {
                                    if (!Int32.TryParse(drows[li_filaFecha - 1][2].ToString(), out li_fechanumero))
                                        ldt_fecha = nf_get_fechaValida(EPDate.ToDateMMDDYYYY(drows[li_filaFecha - 1][2].ToString()).ToString("dd/MM/yyyy"), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                    else
                                        ldt_fecha = nf_get_fechaValida(drows[li_filaFecha - 1][2].ToString(), dt.TableName, out lb_fechavalida, pi_codigoTipoEmpresa);
                                }
                            }
                            catch (FormatException fe)
                            {
                                ListBox1.Items.Add(new ListItem("Formato de Fecha en la celda C" + li_filaFecha.ToString() + " (" + drows[li_filaFecha - 1][2].ToString() + ") de la Hoja '" + dt.TableName + "', Inválido. Ingresar dd/mm/aa", "Error"));
                            }
                        }


                        List<Pronostico_Demanda2> AL_demandas = new List<Pronostico_Demanda2>();
                        //Recorre columnas XLS file
                        if (lb_hojaValida)
                        {

                            for (int li_columna = 0; li_columna < dt.Columns.Count; li_columna++)
                            {
                                if (dt.Rows[0][li_columna].ToString().Equals("1")) //XLS headers
                                {
                                    if (lb_fechavalida)
                                    {
                                        if (ValidateDemandaXLS(dt_ptosMedicionxEmpresa, ldt_fecha, li_columna, pi_tipoPrograma, pi_codigoTipoEmpresa, dt, dt.TableName, AL_demandas))
                                        {
                                            foreach (var demanda in AL_demandas)
                                            {
                                                AL_demandas_Total.Add(demanda);
                                                AL_demandas = new List<Pronostico_Demanda2>();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: Nombre de Hoja Inválido '" + dt.TableName + "'", "Error"));
                    }
                }
            }

            lb_errores = nf_get_valida_errores();

            if (!lb_archivocorrupto)
            {
                stopw.Stop();
                ListBox1.Items.Add("Proceso de verificación finalizó en " + Math.Truncate(stopw.Elapsed.TotalSeconds * 1000) / 1000 + " segundos");
                stopw.Start();
                ListBox1.Items.Add("Proceso de carga iniciado desde los " + Math.Truncate(stopw.Elapsed.TotalSeconds * 1000) / 1000 + " segundos");
            }

            if (!lb_errores)
            {

                /* Grabamos a base de datos*/
                #region Conexion Base Datos
                if (AL_demandas_Total.Count > 0)
                    nf_set_SaveDemandas(AL_demandas_Total, pi_earcodi);
                else
                    ListBox1.Items.Add(new ListItem("NO SE REPORTA INFORMACIÓN ALGUNA. NO SE CARGA A BASE DE DATOS"));

                #endregion

            }
            else
            {
                ListBox1.Items.Add("NO SE PUEDE CARGAR INFORMACIÓN: Existen errores en la carga ");
            }

            //Seteando el largo
            ListBox1.Rows = ListBox1.Items.Count;
        }

        private bool nf_get_valida_errores()
        {
            bool pb_errores = false;

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Value.Trim().TrimEnd().ToUpper().Equals("ERROR"))
                {
                    pb_errores = true;
                    btn_exporta.Enabled = true; btn_exporta.Visible = true;
                    break;
                }
            }

            return pb_errores;
        }

        private void nf_set_SaveDemandas(List<Pronostico_Demanda2> AL_demandas_Total, int pi_earcodi)
        {
            DataAccessLayer.OracleDataAccessX ln_conex_ora = new DataAccessLayer.OracleDataAccessX();
            wcfSicOperacion.Demanda ln_demanda = new wcfSicOperacion.Demanda();
            IDemandaService demandaService = new DemandaService();
            string ls_dns_sic = ConfigurationManager.ConnectionStrings["SICOES"].ToString();
            Seguridad.Autenticacion.Credencial ln_credencial = new Seguridad.Autenticacion.Credencial();
            string ls_credencial = ln_credencial.nf_get_enc_security_credencial1(in_app.is_UserLogin, in_app.is_UserName);
            ln_conex_ora.CreateConnection(ls_dns_sic);
            ln_conex_ora.Open();
            DemandaService demandaServ = new DemandaService();

            //Inicio de la transaccion

            int li_realizado = 0;
            int li_valor = pi_earcodi;

            string ls_comando = String.Empty;
            string ls_cadena = String.Empty;
            string ls_cadenaCodigos = String.Empty;
            DataSet ds_resultado;

            //Agregando los ratios
            //Para los PDO 
            if (((AL_demandas_Total.Where(t => t.LectoCodi == 45).ToList().Count > 0) || (AL_demandas_Total.Where(t => t.LectoCodi == 46).ToList().Count > 0)))
            {
                li_realizado = nf_get_add_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0));
            }

            //Para los PSO 
            if ((AL_demandas_Total.Where(t => t.LectoCodi == 47).ToList().Count > 0))
            {
                li_realizado = nf_get_add_ratio(li_valor, 6, 1, 1, Convert.ToDouble(1 / 1 * 100.0));
            }

            for (int i = 0; i < AL_demandas_Total.Count; i++)
            {
                ds_resultado = new DataSet();

                if (li_realizado >= 0)
                {
                    /*Consulta de Datos*/
                    li_realizado = nf_get_consulta_medicion48(ds_resultado, AL_demandas_Total[i]);


                    /*Ingresando datos PDO*/
                    if (AL_demandas_Total.Where(t => t.LectoCodi == 45).ToList().Count > 0 || AL_demandas_Total.Where(t => t.LectoCodi == 46).ToList().Count > 0)
                    {
                        if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, this.NroHorasValidacionDiario, 1))
                        {
                            if (ds_resultado.Tables["MEDICION48"] != null && ds_resultado.Tables["MEDICION48"].Rows.Count >= 0)
                            {
                                if (ds_resultado.Tables["MEDICION48"].Rows.Count == 0)
                                {
                                    nf_set_inserta_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                                else
                                {
                                    nf_set_actualiza_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                            }
                        }
                        else
                        {
                            //if (AL_demandas_Total[i].LectoCodi == 45)
                            //{
                            if (ds_resultado.Tables["MEDICION48"] != null && ds_resultado.Tables["MEDICION48"].Rows.Count >= 0)
                            {
                                if (ds_resultado.Tables["MEDICION48"].Rows.Count == 0)
                                {
                                    nf_set_inserta_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                                else
                                {
                                    nf_set_actualiza_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                            }
                            //}
                        }
                    }
                    else if (AL_demandas_Total.Where(t => t.LectoCodi == 47).ToList().Count > 0)
                    {
                        if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now, this.NroHorasValidacionSemanal))
                        {
                            if (ds_resultado.Tables["MEDICION48"] != null && ds_resultado.Tables["MEDICION48"].Rows.Count >= 0)
                            {
                                if (ds_resultado.Tables["MEDICION48"].Rows.Count == 0)
                                {
                                    nf_set_inserta_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                                else
                                {
                                    nf_set_actualiza_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                            }
                        }
                        else
                        {
                            if (ds_resultado.Tables["MEDICION48"] != null && ds_resultado.Tables["MEDICION48"].Rows.Count >= 0)
                            {
                                if (ds_resultado.Tables["MEDICION48"].Rows.Count == 0)
                                {
                                    nf_set_inserta_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                                else
                                {
                                    nf_set_actualiza_medicion48(ds_resultado, AL_demandas_Total[i]);
                                }
                            }
                        }
                    }


                    if (!String.IsNullOrEmpty(ls_cadena))
                    {
                        ListBox1.Items.Add("Error de Base de Datos. Detalles: " + ls_cadena);
                    }

                    /*Insertando Datos*/
                    if (li_realizado >= 0)
                    {


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
            int li_inserta = 0;
            DateTime ldt_fecha_envio = DateTime.Now;
            //string ls_en_plazo = "F";
            int li_en_plazo = 2; //FUERA DE PLAZO POR DEFECTO: omision de carga


            if (ddl_tipoPrograma.SelectedValue.Equals("1"))
            {
                if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, this.NroHorasValidacionDiario, 1))
                    //ls_en_plazo = "P";
                    li_en_plazo = 2;
            }
            else if (ddl_tipoPrograma.SelectedValue.Equals("2"))
            {
                if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now, this.NroHorasValidacionSemanal))
                    //ls_en_plazo = "P";
                    li_en_plazo = 2;
            }


            if ((AL_demandas_Total.Where(t => t.LectoCodi == 45).ToList().Count > 0) && (AL_demandas_Total.Where(t => t.LectoCodi == 46).ToList().Count > 0))
            {
                ListBox1.Items.Add("LA INFORMACIÓN HISTÓRICA HA SIDO CARGADA");
                ListBox1.Items.Add("LA INFORMACIÓN PREVISTA HA SIDO CARGADA");
                li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 2, 3, li_en_plazo, ldt_fecha_envio, ldt_expectedDate, in_app.is_UserLogin);
            }
            else if (AL_demandas_Total.Where(t => t.LectoCodi == 45).ToList().Count > 0)
            {
                ListBox1.Items.Add("LA INFORMACIÓN HISTÓRICA HA SIDO CARGADA");
                li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 2, 1, li_en_plazo, ldt_fecha_envio, ldt_expectedDate, in_app.is_UserLogin);
            }
            else if (AL_demandas_Total.Where(t => t.LectoCodi == 46).ToList().Count > 0)
            {
                ListBox1.Items.Add("LA INFORMACIÓN PREVISTA HA SIDO CARGADA");
                li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 2, 2, li_en_plazo, ldt_fecha_envio, ldt_expectedDate, in_app.is_UserLogin);
            }
            else if (AL_demandas_Total.Where(t => t.LectoCodi == 47).ToList().Count > 0)
            {
                ListBox1.Items.Add("LA INFORMACIÓN PREVISTA HA SIDO CARGADA");
                li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 3, 2, li_en_plazo, ldt_fecha_envio, ldt_expectedDate, in_app.is_UserLogin);
            }

            //foreach (Pronostico_Demanda2 pronostico in AL_demandas_Total)
            //{
            //    if (pronostico.LectoCodi == 45)
            //    {
            //        if (!lb_info_historica)
            //        {
            //            ListBox1.Items.Add("LA INFORMACIÓN HISTORICA HA SIDO CARGADA");
            //            li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 2, 1, "F", DateTime.Now.Date, ldt_expectedDate, in_app.is_UserLogin);
            //            lb_info_historica = true;
            //        }
            //    }
            //    if (pronostico.LectoCodi == 46)
            //    {
            //        if (!lb_info_prevista_diaria)
            //        {
            //            ListBox1.Items.Add("LA INFORMACIÓN PREVISTA HA SIDO CARGADA");
            //            li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 2, 2, "F", DateTime.Now.Date, ldt_expectedDate, in_app.is_UserLogin);
            //            lb_info_prevista_diaria = true;
            //        }
            //    }
            //    if (pronostico.LectoCodi == 47)
            //    {
            //        if (!lb_info_prevista_semanal)
            //        {
            //            ListBox1.Items.Add("LA INFORMACIÓN PREVISTA HA SIDO CARGADA");
            //            li_inserta = demandaServ.nf_set_insert_envio(pi_earcodi, 3, 2, "F", DateTime.Now.Date, ldt_expectedDate, in_app.is_UserLogin);
            //            lb_info_prevista_semanal = true;
            //        }
            //    }
            //}

            //Actualizar estado envio 
            if (li_realizado > 0)
            {
                if (ddl_tipoPrograma.SelectedValue.Equals("1")) //Diario
                {
                    if (Util.DiferenciaFecha.HoraValida(ldt_expectedDate, this.NroHorasValidacionDiario, 1))
                    {
                        //li_valor = earcodi
                        li_realizado = demandaServ.nf_upd_env_estado(li_valor, 2, in_app.is_UserLogin);// 2 valor por defecto (Procesado)
                    }
                    else
                    {
                        //li_valor = earcodi
                        li_realizado = demandaServ.nf_upd_env_estado(li_valor, 5, in_app.is_UserLogin);// 5 (Fuera de Plazo)
                        ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA FUERA DE PLAZO a las " + ldt_fecha_envio.ToString("H:mm:ss") + " del día " + ldt_fecha_envio.ToString("dd/MM/yyyy"));
                        li_fuera_plazo = 1;
                    }
                }
                else if (ddl_tipoPrograma.SelectedValue.Equals("2")) //Semanal
                {
                    if(true)
                    //if (Util.DiferenciaFecha.RangoDiasValidoPSO(DateTime.Now, this.NroHorasValidacionSemanal))
                    {
                        //li_valor = earcodi
                        li_realizado = demandaServ.nf_upd_env_estado(li_valor, 2, in_app.is_UserLogin);// 2 valor por defecto (Procesado)
                    }
                    else
                    {
                        //li_valor = earcodi
                        li_realizado = demandaServ.nf_upd_env_estado(li_valor, 5, in_app.is_UserLogin);// 5 (Fuera de Plazo)
                        ListBox1.Items.Add("LA INFORMACIÓN FUE ENVIADA FUERA DE PLAZO a las " + ldt_fecha_envio.ToString("H:mm:ss") + " del día " + ldt_fecha_envio.ToString("dd/MM/yyyy"));
                        li_fuera_plazo = 1;
                    }
                }

            }

            if (li_realizado > 0)
            {
                demandaServ.nf_upd_env_copiado(li_valor, true, in_app.is_UserLogin);
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

            /*Nuevo*/
            if (li_fuera_plazo == 1)
            {
                Session["earcodi"] = li_valor;
                Session["listaErrores"] = ListBox1.Items;
                Response.Redirect("~/WebForm/Demanda/w_dem_inf_fuera_plazo.aspx", false);
                //Response.AddHeader("REFRESH", "5;URL=w_dem_inf_fuera_plazo.aspx");  
            }
        }


        private int nf_get_consulta_medicion48(DataSet ds, Pronostico_Demanda2 demanda)
        {
            String ls_string = String.Empty;
            int li_resultado = 0;
            try
            {
                ls_string = " SELECT TIPOINFOCODI, LECTCODI, MEDIFECHA, PTOMEDICODI FROM ME_MEDICION48 ";
                ls_string += " WHERE TIPOINFOCODI = " + demanda.TipoInfo;
                ls_string += " AND LECTCODI = " + demanda.LectoCodi;
                ls_string += " AND MEDIFECHA = TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
                ls_string += " AND PTOMEDICODI = " + demanda.Codigo;
                li_resultado = in_app.iL_data[0].Fill(ds, "MEDICION48", ls_string);

                li_resultado = 1;

                return li_resultado;
            }
            catch (Exception)
            {
                return li_resultado;
            }
        }

        private int nf_set_inserta_medicion48(DataSet ds, Pronostico_Demanda2 demanda)
        {
            String ls_string = String.Empty;
            String ls_cadena = String.Empty;
            int li_resultado = 0;
            double ld_meditotal = 0;
            string ls_userlogin = (in_app.is_UserLogin.Length > 19) ? in_app.is_UserLogin.Substring(0, 19) : in_app.is_UserLogin;

            if (ds.Tables["MEDICION48"] != null && ds.Tables["MEDICION48"].Rows.Count >= 0)
            {
                if (ds.Tables["MEDICION48"].Rows.Count == 0)
                {
                    ls_string = "INSERT INTO ME_MEDICION48 (MEDIFECHA, LECTCODI, TIPOINFOCODI, PTOMEDICODI,LASTDATE, LASTUSER,";
                    ls_string += "H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,";
                    ls_string += "H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,MEDITOTAL) ";
                    ls_string += "VALUES (TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')," + demanda.LectoCodi + ",";
                    ls_string += demanda.TipoInfo + "," + demanda.Codigo + ",sysdate,'" + ls_userlogin + "'";
                    ls_string += nf_get_string_Valores(demanda.ld_array_demanda48, out ld_meditotal);
                    ls_string += "," + ld_meditotal;
                    ls_string += ")";
                    li_resultado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_string, out ls_cadena);

                    return li_resultado;
                }
                else
                {
                    ls_string = " DELETE FROM ME_MEDICION48";
                    ls_string += " WHERE TIPOINFOCODI = " + demanda.TipoInfo;
                    ls_string += " AND LECTCODI = " + demanda.LectoCodi;
                    ls_string += " AND MEDIFECHA = TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
                    ls_string += " AND PTOMEDICODI = " + demanda.Codigo;
                    li_resultado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_string, out ls_cadena);

                    if (li_resultado > 0)
                    {
                        ls_string = "INSERT INTO ME_MEDICION48 (MEDIFECHA, LECTCODI, TIPOINFOCODI, PTOMEDICODI,LASTDATE, LASTUSER,";
                        ls_string += "H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,";
                        ls_string += "H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,MEDITOTAL) ";
                        ls_string += "VALUES (TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')," + demanda.LectoCodi + ",";
                        ls_string += demanda.TipoInfo + "," + demanda.Codigo + ",sysdate,'" + ls_userlogin + "'";
                        ls_string += nf_get_string_Valores(demanda.ld_array_demanda48, out ld_meditotal);
                        ls_string += "," + ld_meditotal;
                        ls_string += ")";
                        li_resultado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_string, out ls_cadena);

                        return li_resultado;
                    }
                    else
                    {
                        return li_resultado;
                    }


                }
            }
            else
            {
                return li_resultado;
            }
        }

        private int nf_set_actualiza_medicion48(DataSet ds, Pronostico_Demanda2 demanda)
        {
            String ls_string = String.Empty;
            String ls_cadena = String.Empty;
            int li_resultado = 0;
            double ld_meditotal = 0;
            string ls_userlogin = (in_app.is_UserLogin.Length > 19) ? in_app.is_UserLogin.Substring(0, 19) : in_app.is_UserLogin;

            ls_string = "UPDATE ME_MEDICION48 SET MEDIFECHA = TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY'), LECTCODI = " + demanda.LectoCodi + " "; ;
            ls_string += ", TIPOINFOCODI = " + demanda.TipoInfo + ", PTOMEDICODI = " + demanda.Codigo + ", LASTDATE = sysdate, LASTUSER = '" + ls_userlogin + "' ";
            ls_string += nf_set_update_string_Valores(demanda.ld_array_demanda48, out ld_meditotal);
            ls_string += ", MEDITOTAL = " + ld_meditotal;
            ls_string += " WHERE TIPOINFOCODI = " + demanda.TipoInfo;
            ls_string += " AND LECTCODI = " + demanda.LectoCodi;
            ls_string += " AND MEDIFECHA = TO_DATE('" + demanda.Fecha.ToString("dd/MM/yy") + "', 'DD/MM/YY')";
            ls_string += " AND PTOMEDICODI = " + demanda.Codigo;
            li_resultado = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_string, out ls_cadena);

            return li_resultado;
        }

        private int nf_get_add_ratio(int ai_earcodi, int ai_eaicodi, int ai_ninformado, int ai_ntotal, double ad_ratio)
        {
            try
            {
                int li_resultado = 0;
                //int li_codigo = in_app.iL_data[0].nf_GetMaxCodi("EXT_RATIO", "ERATCODI") + 1;
                int li_codigo = in_app.iL_data[0].nf_get_next_key("EXT_RATIO");
                string ls_sql = String.Empty;
                ls_sql = "insert into EXT_RATIO (ERATCODI, EARCODI, EAICODI, ERATTOTINF, ERATENVINF,ERATRATIO,LASTDATE) values ( ";
                ls_sql += li_codigo + ", ";
                ls_sql += ai_earcodi + ", ";
                ls_sql += ai_eaicodi + ", ";
                ls_sql += ai_ninformado + ", ";
                ls_sql += ai_ntotal + ", ";
                ls_sql += ad_ratio + ", sysdate)";
                li_resultado = in_app.iL_data[0].nf_ExecuteNonQuery(ls_sql);

                return li_resultado;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private string nf_get_string_Valores(Valor[] ad_arrayValores, out double ad_meditotal)
        {
            string ls_cadenaValores = String.Empty;
            ad_meditotal = 0;
            for (int i = 0; i < ad_arrayValores.Length; i++)
            {
                ad_meditotal += ad_arrayValores[i].ld_valor ?? 0;
                ls_cadenaValores += "," + ad_arrayValores[i].ld_valor ?? ",";
            }

            ad_meditotal = Math.Round(ad_meditotal, 4);
            return ls_cadenaValores;
        }

        private string nf_set_update_string_Valores(Valor[] ad_arrayValores, out double ad_meditotal)
        {
            string ls_cadenaValores = String.Empty;
            ad_meditotal = 0;
            for (int i = 0; i < ad_arrayValores.Length; i++)
            {
                ad_meditotal += ad_arrayValores[i].ld_valor ?? 0;
                ls_cadenaValores += "," + " H" + (i + 1).ToString() + " = " + (ad_arrayValores[i].ld_valor ?? 0).ToString();
            }

            ad_meditotal = Math.Round(ad_meditotal, 4);
            return ls_cadenaValores;
        }

        private DateTime nf_get_fechaValida(int ai_fechaNumero, string as_nombreHoja, out bool ab_fechavalida, int ai_codigoTipoEmpresa)
        {
            DateTime ldt_fecha;
            ldt_fecha = EPDate.ExcelSerialDateToDMY(ai_fechaNumero);
            ab_fechavalida = false;
            string ps_celdaFecha = String.Empty;

            if (ai_codigoTipoEmpresa == 2)
                ps_celdaFecha = "C:15";
            else if (ai_codigoTipoEmpresa == 4)
                ps_celdaFecha = "C:16";


            if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
            {
                if ((as_nombreHoja.ToUpper() == "HISTORICO-MW") || (as_nombreHoja.ToUpper() == "HISTÓRICO-MW"))
                {
                    if (ldt_fecha.AddDays(1).ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"]))) //Se aumenta un dia a la fecha del archivo hoja 'HISTORICO-MW'
                    {
                        ab_fechavalida = true;
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda " + ps_celdaFecha + " en la hoja " + as_nombreHoja + " no coincide con la fecha " + ldt_fecha.ToString("dd/MM/yy"), "Error"));
                        ab_fechavalida = false;
                    }
                }
                else if ((as_nombreHoja.ToUpper() == "PREVISTO-MW"))
                {
                    if (ldt_fecha.AddDays(-1).ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"]))) //Se resta un dia a la fecha del archivo hoja 'PREVISTO-MW'
                    {
                        ab_fechavalida = true;
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda " + ps_celdaFecha + " en la hoja " + as_nombreHoja + " no coincide con la fecha " + ldt_fecha.ToString("dd/MM/yy"), "Error"));
                        ab_fechavalida = false;
                    }
                }
            }
            else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
            {
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(TextBox1.Text)).AddDays(1);
                //DateTime ldt_fechaSabado = EPDate.f_fechafinsemana(EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]))).AddDays(1);
                DateTime ldt_fechaSabado = EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]));
                DateTime fecha = ldt_fecha;
                if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")))
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la fecha de inicio de la semana operativa (sábado).", "Error"));
                    ab_fechavalida = false;
                }
            }

            return ldt_fecha;
        }

        private DateTime nf_get_fechaValida(string ls_fecha, string as_nombreHoja, out bool ab_fechavalida, int ai_codigoTipoEmpresa)
        {
            int li_fechaNumero = 0;
            DateTime ldt_fecha = new DateTime(2000, 1, 1);

            string ps_celdaFecha = String.Empty;

            if (ai_codigoTipoEmpresa == 2)
                ps_celdaFecha = "C:15";
            else if (ai_codigoTipoEmpresa == 4)
                ps_celdaFecha = "C:16";

            if (Int32.TryParse(ls_fecha, out li_fechaNumero))
            {
                ldt_fecha = EPDate.ExcelSerialDateToDMY(li_fechaNumero);
                ab_fechavalida = true;
            }
            else
            {
                if (EPDate.IsDate(ls_fecha))
                {
                    ldt_fecha = EPDate.ToDate(ls_fecha);
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda " + ps_celdaFecha + "'" + ls_fecha + "' en la hoja " + as_nombreHoja + " debe tener formato dd/MM/aaaa.", "Error"));
                    ab_fechavalida = false;
                    return ldt_fecha;
                }
            }

            ab_fechavalida = false;

            if (ddl_tipoPrograma.SelectedItem.Value.Equals("1"))
            {
                if ((as_nombreHoja.ToUpper() == "HISTORICO-MW") || (as_nombreHoja.ToUpper() == "HISTÓRICO-MW"))
                {
                    if (ldt_fecha.AddDays(1).ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"]))) //Se aumenta un dia a la fecha del archivo hoja 'HISTORICO-MW'
                    {
                        ab_fechavalida = true;
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda " + ps_celdaFecha + "'" + ls_fecha + "' en la hoja " + as_nombreHoja + " no coincide el día anterior a la fecha seleccionada ", "Error")); // + ldt_fecha.ToString("dd/MM/yy"), "Error"));
                        ab_fechavalida = false;
                    }
                }
                else if ((as_nombreHoja.ToUpper() == "PREVISTO-MW"))
                {
                    if (ldt_fecha.AddDays(-1).ToString("dd/MM/yyyy").Equals(Convert.ToString(tBoxFecha.Attributes["value"]))) //Se resta un dia a la fecha del archivo hoja 'PREVISTO-MW'
                    {
                        ab_fechavalida = true;
                    }
                    else
                    {
                        ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda " + ps_celdaFecha + "'" + ls_fecha + "' en la hoja " + as_nombreHoja + " no coincide el día posterior a la fecha seleccionada ", "Error")); // + ldt_fecha.ToString("dd/MM/yy"), "Error"));
                        ab_fechavalida = false;
                    }
                }
            }
            else if (ddl_tipoPrograma.SelectedItem.Value.Equals("2"))
            {
                DateTime ldt_fechaSabado = EPDate.ToDate(Convert.ToString(tBoxFecha.Attributes["value"]));
                DateTime fecha = ldt_fecha;
                if (fecha.ToString("dd/MM/yyyy").Equals(ldt_fechaSabado.ToString("dd/MM/yyyy")))
                {
                    ab_fechavalida = true;
                }
                else
                {
                    ListBox1.Items.Add(new ListItem("ERROR: La fecha en la celda C:16 en la hoja " + as_nombreHoja + " no coincide con la fecha de inicio de semana operativa (sábado).", "Error"));
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

            if ((dataBarras != null) && (dataBarras.Rows.Count > 0))
            {
                return dataBarras;
            }
            else
            {
                ListBox1.Items.Add(new ListItem("ERROR: No se puede obtener códigos de puntos de medición por empresa", "Error"));
                return null;
            }

        }

        private DataTable nf_get_puntosMedicionxEmpresa(int pi_tipo_empresa_codigo)
        {
            DemandaService demandaServ = new DemandaService();
            //DataTable dataBarras = wsdemanda.PuntoMedicionBarraxEmp(Convert.ToInt32(DDLEmpresa.SelectedValue), ls_credencial);
            DataTable dataBarras = demandaServ.nf_get_puntos_medicion_x_empresa(Convert.ToInt32(DDLEmpresa.SelectedValue), pi_tipo_empresa_codigo);

            if ((dataBarras != null) && (dataBarras.Rows.Count > 0))
            {
                return dataBarras;
            }
            else
            {
                ListBox1.Items.Add(new ListItem("ERROR: No se puede obtener códigos de puntos de medición por empresa", "Error"));
                return null;
            }

        }

        private static bool IsValidName(string ps_wsName)
        {
            if ((ps_wsName.ToUpper() == "HISTORICO-MW") || (ps_wsName.ToUpper() == "HISTÓRICO-MW") || (ps_wsName.ToUpper() == "PREVISTO-MW"))
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
            if (Convert.ToInt32(DDLNumSemana.SelectedValue) == 1 || Convert.ToInt32(DDLNumSemana.SelectedValue) == 2)
                tBoxFecha.Attributes["value"] = EPDate.f_fechainiciosemana(DateTime.Now.Year + 1, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");
            else
                tBoxFecha.Attributes["value"] = EPDate.f_fechainiciosemana(DateTime.Now.Year, Convert.ToInt32(DDLNumSemana.SelectedValue)).ToString("dd/MM/yyyy");

        }

        protected void ddl_tipoPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            //limpiar list box
            ListBox1.DataSource = null;
            ListBox1.DataBind();

            //TextBox1.Text = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");
            tBoxFecha.Attributes["value"] = EPDate.ToDate(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy");
            DateTime ldt_fecha = new DateTime(2013, 1, 1);

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



                DDLNumSemana.Text = CargaDDLSemanas.LlenaSemanasTemporal(DateTime.Now.Date, out ldt_fecha).First().Key;               
                tBoxFecha.Attributes["value"] = ldt_fecha.ToString("dd/MM/yyyy");
                //}
            }
        }

        private bool ValidateDemandaXLS(DataTable pdt_ptosMedicionxEmpresa, DateTime pdt_fecha, int pi_columna, int pi_codigoTipoPrograma, int pi_codigoTipoEmpresa, DataTable dt, string ps_sheetName, List<Pronostico_Demanda2> demandas)
        {
            bool lb_valida = true;
            int pi_codigoPuntoMedicion = 0;
            int pi_codigoLectura = 0;
            int pi_tipoInformacion = 20;

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
                                        //if ((pdt_fecha.Date <= DateTime.Now.Date))
                                        //{
                                        //Valores Validos para HISTÓRICO-MW - PDO
                                        nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                        //}
                                        //else
                                        //{
                                        //ListBox1.Items.Add(new ListItem("LA INFORMACIÓN HISTÓRICA NO SERÁ CARGADA EN DÍAS FUTUROS"));
                                        //lb_valida = false;
                                        //}    
                                    }
                                    else
                                    {
                                        f_set_linea_listBox(ps_sheetName, 3, pi_columna, "Código de Lectura no corresponde.");
                                        lb_valida = false;
                                    }
                                }
                                else if (ps_sheetName.ToUpper().Equals("PREVISTO-MW"))
                                {
                                    if (pi_codigoLectura == 46)
                                    {
                                        //if ((pdt_fecha > DateTime.Now.Date))
                                        //{
                                        //Valores Validos para PREVISTO-MW - PDO
                                        nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
                                        //}
                                        //else
                                        //{
                                        //ListBox1.Items.Add(new ListItem("LA INFORMACIÓN PREVISTA NO SERÁ CARGADA EN EL MISMO DÍA O ANTERIOR"));
                                        //lb_valida = false;
                                        //}

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
                                        //Valores Validos para PREVISTO-MW - PSO
                                        nf_valida_valores_48(pdt_fecha, ps_sheetName, pi_codigoPuntoMedicion, pi_codigoLectura, pi_tipoInformacion,
                                                                pi_columna, pi_codigoTipoEmpresa, pi_codigoTipoPrograma, demandas, dt, out lb_valida);
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
                        f_set_linea_listBox(ps_sheetName, 2, pi_columna, "En el código del punto de medición (" + pi_codigoPuntoMedicion + ") no pertenece a la empresa seleccionada ");
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

        private void nf_valida_valores_48(DateTime pdt_fecha, string ps_sheetName, int pi_codigoPuntoMedicion, int pi_codigoLectura, int pi_tipoInformacion, int pi_columna, int pi_codigoTipoEmpresa, int pi_codigoTipoPrograma, List<Pronostico_Demanda2> demandas, DataTable dt, out bool pb_valida)
        {
            double ld_valor = 0;
            pb_valida = true;
            Pronostico_Demanda2 demanda = new Pronostico_Demanda2();
            List<string> ld_listaErrores;
            int li_recorre_LImINF = 0;
            int li_recorre_LImSUP = 0;

            if (pi_codigoTipoPrograma == 1) //PDO
            {
                if (pi_codigoTipoEmpresa == 2 || pi_codigoTipoEmpresa == 4)
                {
                    if (pi_codigoTipoEmpresa == 2) //DISTR
                    {
                        li_recorre_LImINF = 22; li_recorre_LImSUP = 70;
                    }
                    else //ULIBRE
                    {
                        li_recorre_LImINF = 27; li_recorre_LImSUP = 75;
                    }

                    demanda.ld_array_demanda48 = new Valor[48];
                    ld_listaErrores = new List<string>();

                    for (int i = li_recorre_LImINF; i < li_recorre_LImSUP; i++)
                    {
                        try
                        {
                            if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                            {
                                if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                                {
                                    demanda.ld_array_demanda48[i - li_recorre_LImINF] = new Valor() { ld_valor = Math.Round(ld_valor, 5) };
                                }
                                else
                                {
                                    f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Valor no válido : '" + dt.Rows[i][pi_columna].ToString() + "'");
                                    pb_valida = false;
                                }
                            }
                            else
                            {
                                ld_listaErrores.Add("Error : Hoja " + ps_sheetName + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + (i + 1).ToString() + ", Celda vacía");
                                pb_valida = false;
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            ld_listaErrores.Add("Error : Hoja " + ps_sheetName + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + (i + 1).ToString() + ", Celda vacía");
                            pb_valida = false;

                        }
                    }

                    if (ld_listaErrores.Count < li_recorre_LImSUP - li_recorre_LImINF)
                    {
                        foreach (var item in ld_listaErrores)
                        {
                            ListBox1.Items.Add(new ListItem(item, "Error"));
                        }
                    }
                    else
                    {
                        ListBox1.Items.Add("NO SE REPORTA INFORMACIÓN EN LA COLUMNA '" + ExcelUtil.GetExcelColumnName(pi_columna + 1) + "' DE LA HOJA " + ps_sheetName);
                    }

                    if (pb_valida)
                    {
                        demanda.Codigo = pi_codigoPuntoMedicion;
                        demanda.LectoCodi = pi_codigoLectura;
                        demanda.TipoInfo = pi_tipoInformacion;

                        if (pi_codigoLectura == 45)
                        {
                            demanda.Fecha = pdt_fecha;
                        }
                        else if (pi_codigoLectura == 46)
                        {
                            demanda.Fecha = pdt_fecha;
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
                Valor[] ld_array_valores = new Valor[48];
                DateTime ldt_fechaInicioSem = pdt_fecha;
                ld_listaErrores = new List<string>();

                if (pi_codigoTipoEmpresa == 2 || pi_codigoTipoEmpresa == 4)
                {
                    if (pi_codigoTipoEmpresa == 2) //DISTR
                    {
                        li_recorre_LImINF = 22; li_recorre_LImSUP = 358;
                    }
                    else //ULIBRE
                    {
                        li_recorre_LImINF = 27; li_recorre_LImSUP = 363;
                    }

                    demanda.ld_array_demanda48 = new Valor[48];
                    ld_listaErrores = new List<string>();

                    for (int i = li_recorre_LImINF; i < li_recorre_LImSUP; i++)
                    {
                        try
                        {
                            if (dt.Rows[i][pi_columna] != null && dt.Rows[i][pi_columna].ToString() != String.Empty)
                            {
                                if (double.TryParse(dt.Rows[i][pi_columna].ToString(), out ld_valor))
                                {
                                    ld_array_valores[(i - li_recorre_LImINF) % 48] = new Valor() { ld_valor = Math.Round(ld_valor, 5) };

                                    if ((i - li_recorre_LImINF) % 48 == 47)
                                    {
                                        demanda.Codigo = pi_codigoPuntoMedicion;
                                        demanda.LectoCodi = pi_codigoLectura;
                                        demanda.TipoInfo = pi_tipoInformacion;

                                        if (pi_codigoLectura == 47)
                                        {
                                            demanda.Fecha = ldt_fechaInicioSem.AddDays(((int)i / 48) - 1);
                                        }
                                        else
                                        {
                                            f_set_linea_listBox(ps_sheetName, 15, pi_columna, "Fecha inválida" + pdt_fecha.ToString("dd/MM/yyyy"));
                                            pb_valida = false;
                                        }

                                        demanda.ld_array_demanda48 = new Valor[48];
                                        demanda.ld_array_demanda48 = ld_array_valores;
                                        ld_array_valores = new Valor[48];
                                        demandas.Add(demanda);
                                        demanda = new Pronostico_Demanda2();
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
                                //f_set_linea_listBox(ps_sheetName, (i + 1), pi_columna, "Celda vacía");
                                ld_listaErrores.Add("Error : Hoja " + ps_sheetName + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + (i + 1).ToString() + ", Celda vacía");
                                pb_valida = false;
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            //ListBox1.Items.Add(new ListItem("Error : Hoja " + ps_sheetName + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + (i + 1).ToString() + ", Celda vacía", "Error"));
                            //pb_valida = false;
                            ld_listaErrores.Add("Error : Hoja " + ps_sheetName + " Celda " + ExcelUtil.GetExcelColumnName(pi_columna + 1).ToString() + (i + 1).ToString() + ", Celda vacía");
                            pb_valida = false;
                        }

                    }

                    if (ld_listaErrores.Count < li_recorre_LImSUP - li_recorre_LImINF)
                    {
                        foreach (var item in ld_listaErrores)
                        {
                            ListBox1.Items.Add(new ListItem(item, "Error"));
                        }
                    }
                    else
                    {
                        ListBox1.Items.Add("NO SE REPORTA INFORMACIÓN EN LA COLUMNA '" + ExcelUtil.GetExcelColumnName(pi_columna + 1) + "' DE LA HOJA " + ps_sheetName);
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

        private void f_set_linea_listBox(string ps_nameSheet, int pi_fila, int pi_columna, string ps_mensaje)
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

        protected void DDLEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //limpiar list box
            ListBox1.DataSource = null;
            ListBox1.DataBind();
            this.CargarDatosPlazo();
        }

        /// <summary>
        /// Permite cargar los plazos permitidos
        /// </summary>
        protected void CargarDatosPlazo()
        {
            NotificacionAppServicio notificacion = new NotificacionAppServicio();
          
            if (!string.IsNullOrEmpty(this.DDLEmpresa.SelectedValue))
            {
                int idEmpresa = Convert.ToInt32(this.DDLEmpresa.SelectedValue);
                int plazoDiario = 0;
                int plazoSemanal = 0;
                notificacion.ObtenerPlazoFormato(idEmpresa, DateTime.Now, out plazoDiario, out plazoSemanal);
                this.NroHorasValidacionDiario = plazoDiario;
                this.NroHorasValidacionSemanal = plazoSemanal;
            }
        }
    }
}