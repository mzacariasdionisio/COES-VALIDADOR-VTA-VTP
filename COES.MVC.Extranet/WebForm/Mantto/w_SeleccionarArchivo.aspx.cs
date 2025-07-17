using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data;
using WScoes;
using WSIC2010.Util;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace WSIC2010
{
    public partial class w_SeleccionarArchivo : System.Web.UI.Page
    {
        n_app in_app;
        int i_regcodi = 0;
        DateTime idt_fechini;
        DateTime idt_fechfin;
        int ii_evenclasecodi = 0;
        string ls_mensaje = String.Empty;
        int li_resultado = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/mantto/w_eve_mantto.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];

                if (Session["i_regcodi"] != null)
                {
                    i_regcodi = Convert.ToInt32(Session["i_regcodi"]);
                    idt_fechini = (DateTime)Session["dt_FechaInicial"];
                    idt_fechfin = (DateTime)Session["dt_FechaFinal"];
                    ii_evenclasecodi = Convert.ToInt32(Session["ii_evenclasecodi"]);
                }
                else
                {
                    UploadStatusLabel.Text = "Codigo registro mantto no definido";
                    Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
                }
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            // Specify the path on the server to
            // save the uploaded file to.
            String savePath = @"d:\data\mantto\";

            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            if (FileUpload1.HasFile && FileUpload1.FileName.ToUpper().EndsWith("XLS"))
            {
                // Get the name of the file to upload.
                //string fileName = Server.HtmlEncode(FileUpload1.FileName);
                string fileName = FileUpload1.FileName;

                //Delete whitespace in filename
                fileName = fileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                                   .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U")
                                   .Replace(" ", "_").Substring(0, fileName.Length - 4) + "_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".xls";

                if (fileName.Length > 56) fileName = fileName.Substring(0, 56) + ".xls";

                // Get the extension of the uploaded file.
                string extension = System.IO.Path.GetExtension(fileName);

                // Allow only files with .doc or .xls extensions
                // to be uploaded.
                if (extension == ".xls")
                {
                    // Append the name of the file to upload to the path.
                    string ls_pathFile = savePath + fileName;

                    int sizeFile = FileUpload1.PostedFile.ContentLength;
                    double fileMB = Util.SizeFile.SizeOfFile(sizeFile);



                    FileUpload1.SaveAs(ls_pathFile);

                    DataSet output = new DataSet();
                    ManttoService servicio = new ManttoService();
                    string ls_query1 = String.Empty;
                    string ls_query2 = String.Empty;
                    DateTime dt_fechaIni = new DateTime(2000, 1, 1, 0, 0, 0);
                    DateTime dt_fechaFin = new DateTime(2000, 1, 1, 0, 0, 0);
                    List<ManttoByQuery> AL_ManttosQuerys = new List<ManttoByQuery>();

                    if (ii_evenclasecodi > 0)
                    {
                        int li_codigo_tipo_archivo = ii_evenclasecodi + 5;

                        li_resultado = servicio.Create_EnvioArchivo(li_codigo_tipo_archivo, -1, fileName, fileMB, "1.0", savePath, in_app.ii_UserCode, in_app.is_PC_IPs, in_app.is_UserLogin, idt_fechini, 1, "N");
                    }

                    if (li_resultado < 1)
                    {
                        Alert.Show("Existe un error al momento de guardar el archivo");
                        return;
                    }

                    DataTable outputTable = new DataTable("MANTTOS");
                    outputTable = CreateDataSet(ls_pathFile).Tables[0];
                    UploadButton.Visible = false;
                    bool lb_inicio = false;
                    int li_valor = 0;
                    int li_xlsfila = 0;

                    if (outputTable != null && outputTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in outputTable.Rows)
                        {
                            li_xlsfila++;
                            if (lb_inicio)
                            {
                                if (dr[0].ToString().Trim() != String.Empty)
                                {
                                    int li_equicodi = -1;
                                    string ls_mensaje = String.Empty;
                                    if (int.TryParse(dr[3].ToString(), out li_equicodi))
                                    {
                                        if (li_equicodi > 0)
                                        {
                                            if (servicio.GetEquipoValido(in_app.is_Empresas, li_equicodi, out ls_mensaje))
                                            {
                                                if (!ValidaRegistro(dr, li_xlsfila, idt_fechini, idt_fechfin, out dt_fechaIni, out dt_fechaFin, out ls_query1, out ls_query2))
                                                {
                                                    f_set_linea_texto(li_xlsfila, -1, " Equipo : " + dr[1].ToString() + "-" + dr[2].ToString());
                                                }
                                                else
                                                {
                                                    AL_ManttosQuerys.Add(new ManttoByQuery()
                                                    {
                                                        FechaHoraInicial = dt_fechaIni,
                                                        FechaHoraFinal = dt_fechaFin,
                                                        UpdateQuery1 = ls_query1,
                                                        UpdateQuery2 = ls_query2
                                                    }
                                                        );
                                                }
                                            }
                                            else
                                            {
                                                f_set_linea_texto(li_xlsfila - 1, 5, ls_mensaje);
                                            }
                                        }
                                        else
                                        {
                                            f_set_linea_texto(li_xlsfila - 1, 5, "El código del equipo ingresado es inválido.");
                                        }
                                    }
                                    else
                                    {
                                        f_set_linea_texto(li_xlsfila - 1, 5, "El código del equipo ingresado es inválido.");
                                    }
                                }
                            }
                            else
                                if (dr[0].ToString().Trim() == "EMPRESA")
                                    lb_inicio = true;
                                //fin else if
                                else
                                    if (int.TryParse(dr[5].ToString(), out li_valor) && li_xlsfila == 3)
                                    {
                                        if (ii_evenclasecodi != li_valor)
                                        {
                                            switch (ii_evenclasecodi)
                                            {
                                                case 1:
                                                    f_set_linea_texto(2, 6, "Intentó cargar un Programa de Mantenimiento Ejecutado, pero en el archivo excel seleccionó " + nf_get_TipoPrograma(li_valor));
                                                    break;
                                                case 2:
                                                    f_set_linea_texto(2, 6, "Intentó cargar un Programa de Mantenimiento Diario, pero en el archivo excel seleccionó " + nf_get_TipoPrograma(li_valor));
                                                    break;
                                                case 3:
                                                    f_set_linea_texto(2, 6, "Intentó cargar un Programa de Mantenimiento Semanal, pero en el archivo excel seleccionó " + nf_get_TipoPrograma(li_valor));
                                                    break;
                                                case 4:
                                                    f_set_linea_texto(2, 6, "Intentó cargar un Programa de Mantenimiento Mensual, pero en el archivo excel seleccionó " + nf_get_TipoPrograma(li_valor));
                                                    break;
                                                case 5:
                                                    f_set_linea_texto(2, 6, "Intentó cargar un Programa de Mantenimiento Anual, pero en el archivo excel seleccionó " + nf_get_TipoPrograma(li_valor));
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                            //fin else if
                        }
                    }
                    else
                    {
                        f_set_linea_texto(10, -1, "No existen registros a cargar en archivo en blanco");
                    }

                    if (ListBox1.Items.Count == 0)
                    {
                        //ManttoServiceClient service = new ManttoServiceClient();

                        ManttoService service = new ManttoService();
                        //int li_temp = service.InsertMantto(i_regcodi, UpdateQuery1, UpdateQuery2, in_app.is_UserLogin + " " + in_app.is_PC_IPs);
                        int li_temp = 0;
                        int li_counter = 0;

                        if (AL_ManttosQuerys.Count > 0)
                        {
                            //Eliminamos los manttos programados anuales por usuario antes de crear los nuevos
                        }

                        foreach (ManttoByQuery item in AL_ManttosQuerys)
                        {
                            li_temp = service.InsertMantto(i_regcodi, item.FechaHoraInicial, item.FechaHoraFinal, item.UpdateQuery1 + ",CREATED", item.UpdateQuery2 + ",SYSDATE", in_app.is_UserLogin + " " + in_app.is_PC_IPs);
                            li_counter++;

                            if (li_temp == 0)
                            {
                                Alert.Show("ERROR en el Servicio, Contáctese con el administrador");
                                break;
                            }
                        }


                        if (li_temp > 0)
                        {
                            UploadStatusLabel.Text = "Informacion actualizada";
                            //Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
                            //Alert.Show("Se cargaron exitósamente " + li_counter + " registros.");
                            ButtonRegresar.Visible = false; ButtonRegresar.Enabled = false;
                            ButtonListado.Visible = true; ButtonListado.Enabled = true;
                            UploadStatusLabel.Text += "<br/>Se cargarón exitósamente " + li_counter + " registros desde el archivo.";
                        }
                        else
                        {
                            UploadStatusLabel.Text = "Error en actualización !!!";
                        }

                    }

                }
                else
                {
                    // Notify the user why their file was not uploaded.
                    UploadStatusLabel.Text = "El archivo no puede ser subido debido a que no posee la extensión .xls ";

                }
            }
            else
            {
                UploadStatusLabel.Text = "Tipo de archivo inválido o no se ha seleccionado un archivo a subir";
            }
        }

        public static DataSet CreateDataSet(String filePath)
        {
            //bool lb_encontrado = false;
            DataSet ds = new DataSet();
            string ls_value = String.Empty;

            if (filePath.ToUpper().EndsWith(".XLS"))
            {
                HSSFWorkbook hssfworkbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                    Sheet sheet = hssfworkbook.GetSheetAt(0);
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                    DataTable dt = new DataTable();
                    for (int j = 2; j < 14; j++)
                    {
                        dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
                    }

                    while (rows.MoveNext())
                    {
                        Row row = (HSSFRow)rows.Current;
                        DataRow dr = dt.NewRow();

                        for (int i = 2; i < 14; i++)
                        {
                            Cell cell = row.GetCell(i);


                            if (cell == null)
                            {
                                dr[i - 2] = null;
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    //if (DateUtil.IsCellDateFormatted(cell))
                                    //{
                                    //    DateTime ldt_fecha = DateUtil.GetJavaDate(cell.NumericCellValue);
                                    //    dr[i - 1] = ldt_fecha.ToString("dd/MM/yyyy HH:mm:ss");
                                    //}
                                    //else
                                    //{
                                    ls_value = cell.NumericCellValue.ToString();
                                    dr[i - 2] = ls_value;
                                }
                                else
                                {
                                    dr[i - 2] = cell.ToString();
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }

                //if (!lb_encontrado)
                //{
                //    throw new Exception("No se encuentra hoja con nombre 'MANTTOS'");
                //}

            }
            else
            {
                throw new Exception("Ingresar documento con extension .XLS");
            }
            return ds;
        }


        private string nf_get_TipoPrograma(int pi_codigoPrograma)
        {
            switch (pi_codigoPrograma)
            {
                case 1:
                    return "Programa de Mantenimiento Ejecutado";
                case 2:
                    return "Programa de Mantenimiento Diario";
                case 3:
                    return "Programa de Mantenimiento Semanal";
                case 4:
                    return "Programa de Mantenimiento Mensual";
                case 5:
                    return "Programa de Mantenimiento Anual";
                case 6:
                    return "Programa de Mantenimiento Ajuste Diario";
                default:
                    return String.Empty;
            }
        }

        bool ValidaRegistro(DataRow a_dr, int a_linea, DateTime idt_fechini, DateTime idt_fechfin, out DateTime adt_fechaIni, out DateTime adt_fechaFin, out string as_query1, out string as_query2)
        {
            double ld_valor = 0;
            as_query1 = as_query2 = String.Empty;
            adt_fechaIni = new DateTime(2000, 1, 1);
            adt_fechaFin = new DateTime(2000, 1, 1);

            try
            {

                string UpdateQuery1 = "EQUICODI";
                string UpdateQuery2 = a_dr[3].ToString();

                if (a_dr[6] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENDESCRIP";
                    if (a_dr[6].ToString() != String.Empty)
                    {
                        if (a_dr[6].ToString().Length < 300)
                            UpdateQuery2 += ",'" + a_dr[6].ToString() + "'";
                        else
                            f_set_linea_texto(a_linea - 1, 8, "Se excedió longitud del campo DESCRIPCIÓN (máximo 300 caracteres). '" + a_dr[6].ToString() + "'");
                    }
                    else
                    {
                        //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(9) + " Ingresé DESCRIPCIÓN VÁLIDA.");
                        f_set_linea_texto(a_linea - 1, 8, "Ingresé DESCRIPCIÓN VÁLIDA. '" + a_dr[6].ToString() + "'");
                    }
                }
                else
                {
                    //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(9) + " Ingresé DESCRIPCIÓN.");
                    f_set_linea_texto(a_linea - 1, 8, "Ingresé DESCRIPCIÓN.");
                }

                if (a_dr[4] != DBNull.Value)
                {
                    try
                    {
                        ld_valor = Convert.ToDouble(a_dr[4]);
                        if (ld_valor > 0)
                        {
                            adt_fechaIni = EPDate.FromExcelSerialDate(ld_valor);
                        }
                        else
                        {
                            //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(7) + " Formato Fecha Erroneo :" + a_dr[5].ToString());
                            f_set_linea_texto(a_linea - 1, 6, " Formato Fecha Erróneo (dd/MM/yyyy hh:mm):'" + a_dr[4].ToString() + "'");
                        }
                        //UpdateQuery1 += ",EVENINI";
                        //UpdateQuery2 += "," + EPDate.SQLDateTimeOracleString(dtini);
                    }
                    catch (Exception ex)
                    {
                        f_set_linea_texto(a_linea - 1, 6, " Formato Fecha Erróneo (dd/MM/yyyy hh:mm):'" + a_dr[4].ToString() + "'");
                    }
                }
                else
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(7) + " Ingresé FECHA INICIAL.");
                    f_set_linea_texto(a_linea - 1, 6, " Ingresé FECHA INICIAL.");
                }


                if (a_dr[5] != DBNull.Value)
                {
                    try
                    {
                        ld_valor = Convert.ToDouble(a_dr[5]);
                        if (ld_valor > 0)
                        {
                            adt_fechaFin = EPDate.FromExcelSerialDate(ld_valor);
                        }
                        else
                        {
                            ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(8) + " Formato Fecha Erroneo :" + a_dr[5].ToString());
                            f_set_linea_texto(a_linea - 1, 7, " Formato Fecha Erróneo (dd/MM/yyyy hh:mm):'" + a_dr[5].ToString() + "'");
                        }

                        //UpdateQuery1 += ",EVENFIN";
                        //UpdateQuery2 += "," + EPDate.SQLDateTimeOracleString(dtfin);
                    }
                    catch (Exception ex)
                    {
                        f_set_linea_texto(a_linea - 1, 7, " Formato Fecha Erróneo (dd/MM/yyyy hh:mm):'" + a_dr[5].ToString() + "'");
                    }
                }
                else
                {
                    ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(8) + " Ingresé FECHA FINAL.");
                    f_set_linea_texto(a_linea - 1, 7, " Ingresé FECHA FINAL.");
                }

                //verifica que el rango de fechas es valido
                if (adt_fechaIni >= adt_fechaFin )
                {
                    f_set_linea_texto(a_linea - 1, 6, "Error Fecha Inicial > = Fecha Final " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss") +" >= " + adt_fechaFin.ToString("dd-MM-yyyy hh:mm:ss"));
                }

                if (adt_fechaIni < idt_fechini || adt_fechaFin > idt_fechfin.AddDays(1))
                {
                    if (adt_fechaIni < idt_fechini)
                        //ListBox1.Items.Add("Registro en fila " + a_linea + ", Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss"));
                        f_set_linea_texto(a_linea - 1, 6, "Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss"));

                    if (adt_fechaFin < idt_fechini)
                        //ListBox1.Items.Add("Registro en fila " + a_linea + ", Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss"));
                        f_set_linea_texto(a_linea - 1, 7, "Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaFin.ToString("dd-MM-yyyy hh:mm:ss"));

                    if (adt_fechaIni > idt_fechfin.AddDays(1))
                        //ListBox1.Items.Add("Registro en fila " + a_linea + ", Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss"));
                        f_set_linea_texto(a_linea - 1, 6, "Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaIni.ToString("dd-MM-yyyy hh:mm:ss"));

                    if (adt_fechaFin > idt_fechfin.AddDays(1))
                        //ListBox1.Items.Add("Registro en fila " + a_linea + ", Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaFin.ToString("dd-MM-yyyy hh:mm:ss"));
                        f_set_linea_texto(a_linea - 1, 7, "Fecha fuera de rango (" + idt_fechini.ToString("dd/MM/yyyy") + "-" + idt_fechfin.ToString("dd/MM/yyyy") + "): " + adt_fechaFin.ToString("dd-MM-yyyy hh:mm:ss"));
                }

                if (a_dr[10] != DBNull.Value)
                {
                    UpdateQuery1 += ",TIPOEVENCODI";
                    try
                    {
                        string tec = a_dr[10].ToString().Substring(0, 3).ToUpper();
                        string s_tipoevencodi = String.Empty;
                        if (tec == "PRE" || tec == "COR" || tec == "AMP" || tec == "EVE" || tec == "PRU")
                        {
                            if (tec == "PRE")
                                s_tipoevencodi = "1";
                            if (tec == "COR")
                                s_tipoevencodi = "2";
                            if (tec == "AMP")
                                s_tipoevencodi = "3";
                            if (tec == "EVE")
                                s_tipoevencodi = "4";
                            if (tec == "PRU")
                                s_tipoevencodi = "6";

                            UpdateQuery2 += "," + s_tipoevencodi + " ";
                        }
                        else
                        {
                            //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(13) + ": NO SE INGRESÓ TIPOS DE EVENTO VÁLIDO.");
                            f_set_linea_texto(a_linea - 1, 12, "NO SE INGRESÓ TIPO DE EVENTO VÁLIDO. '" + a_dr[10].ToString() + "'");
                        }
                    }
                    catch (Exception)
                    {
                        //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(13) + " observado EVENTO NO VÁLIDO.");
                        f_set_linea_texto(a_linea - 1, 12, "observado EVENTO NO VÁLIDO. '" + a_dr[10].ToString() + "'");
                    }

                }
                else
                {
                    //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(13) + " Ingresé TIPO DE EVENTO.");
                    f_set_linea_texto(a_linea - 1, 12, " Ingresé TIPO DE EVENTO.");
                }


                if (a_dr[11] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENTIPOPROG";
                    string s_eventipoprog = a_dr[11].ToString().Substring(0, 1).ToUpper();
                    try
                    {
                        if ((s_eventipoprog == "P") || (s_eventipoprog == "R") || (s_eventipoprog == "F"))
                        {
                            UpdateQuery2 += ",'" + s_eventipoprog + "'";
                        }
                        else
                        {
                            //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(14) +" : NO SE INGRESÓ PROGRAMACIÓN VÁLIDA.");
                            f_set_linea_texto(a_linea - 1, 13, " NO SE INGRESÓ PROGRAMACIÓN VÁLIDA.");
                        }

                    }
                    catch
                    {
                        //ListBox1.Items.Add("Registro en fila " + a_linea + "columna " + Util.ExcelUtil.GetExcelColumnName(14) + " observado PROGRAMACIÓN NO VÁLIDA.");
                        f_set_linea_texto(a_linea - 1, 13, " observado PROGRAMACIÓN NO VÁLIDA. '" + a_dr[11].ToString() + "'");
                    }

                }
                else
                {
                    //ListBox1.Items.Add("Registro en fila " + a_linea + "columna " + Util.ExcelUtil.GetExcelColumnName(14) + " Ingresé TIPO DE PROGRAMACIÓN.");
                    f_set_linea_texto(a_linea - 1, 13, " Ingresé TIPO DE PROGRAMACIÓN.");
                }

                if (a_dr[8] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENINDISPO";
                    string tec = a_dr[8].ToString().ToUpper().Trim(); ;
                    string s_evenindispo = String.Empty;
                    try
                    {
                        if (tec == "F/S" || tec == "E/S")
                        {
                            s_evenindispo = tec.Substring(0, 1);

                            UpdateQuery2 += ",'" + s_evenindispo + "'";
                        }
                        else
                        {
                            //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(11) + ": NO SE INGRESÓ INDISPONIBILIDAD VÁLIDA.");
                            f_set_linea_texto(a_linea - 1, 10, " NO SE INGRESÓ INDISPONIBILIDAD VÁLIDA. '" + a_dr[8].ToString() + "'");
                        }
                    }
                    catch
                    {
                        //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(11) + " observado EVENINDISPO");
                        f_set_linea_texto(a_linea - 1, 10, " observado EVENINDISPO.");
                    }
                }
                else
                {
                    //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(11) + " Ingresé INDISPONIBILIDAD.");
                    f_set_linea_texto(a_linea - 1, 10, "Ingresé INDISPONIBILIDAD.");
                }

                if (a_dr[9] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENINTERRUP";
                    string tec = a_dr[9].ToString().ToUpper().Trim(); ;
                    string s_eveninterrup = String.Empty;

                    try
                    {
                        if (tec == "NO" || tec == "SI")
                        {
                            s_eveninterrup = tec.Substring(0, 1);
                            UpdateQuery2 += ",'" + s_eveninterrup + "'";
                        }
                        else
                        {
                            //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(12) + ": NO SE INGRESÓ INDISPONIBILIDAD VÁLIDA.");
                            f_set_linea_texto(a_linea - 1, 11, " NO SE INGRESÓ INDISPONIBILIDAD VÁLIDA. '" + a_dr[9].ToString() + "'");
                        }

                    }
                    catch
                    {
                        //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(12) + " observado EVENINTERRUP");
                        f_set_linea_texto(a_linea - 1, 11, " observado EVENINTERRUP");
                    }

                }
                else
                {
                    //ListBox1.Items.Add("Registro en fila " + a_linea + " columna " + Util.ExcelUtil.GetExcelColumnName(12) + " Ingresé INTERRUPCIÓN.");
                    f_set_linea_texto(a_linea - 1, 11, " Ingresé INTERRUPCIÓN.");
                }


                if (a_dr[7] != DBNull.Value)
                {
                    UpdateQuery1 += ",EVENMWINDISP";
                    string mwindisp = a_dr[7].ToString().ToUpper().Trim();
                    double d_eveninterrup = 0;

                    if (Double.TryParse(mwindisp, out d_eveninterrup))
                    {
                        UpdateQuery2 += "," + d_eveninterrup.ToString();
                    }
                    else
                    {
                        f_set_linea_texto(a_linea - 1, 9, " NO SE INGRESÓ MW INIDISPONIBLES VÁLIDO (NÚMERICO). '" + a_dr[7].ToString() + "'");
                    }
                }
                else
                {
                    f_set_linea_texto(a_linea - 1, 9, " Ingresé MW INDISPONIBLES.");
                }


                as_query1 = UpdateQuery1;
                as_query2 = UpdateQuery2;

            }
            catch
            {
                return false;
            }
            return true;
        }

        private void f_set_linea_texto(int pi_row, int pi_col, string ps_texto)
        {
            ls_mensaje = String.Empty;
            if (pi_col != -1)
            {
                ls_mensaje += "ERROR: Celda " + ExcelUtil.GetExcelColumnName(pi_col + 1) + (pi_row + 1).ToString() + ", " + ps_texto;
            }
            else
            {
                ls_mensaje += "ERROR: Fila " + (pi_row + 1).ToString() + ", " + ps_texto;
            }

            ListBox1.Items.Add(ls_mensaje);
        }

        protected void ButtonRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
        }

        protected void ButtonListado_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebForm/mantto/w_eve_mantto_list.aspx");
        }
    }
}