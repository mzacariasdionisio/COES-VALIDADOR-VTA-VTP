using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;
using System.Data.OleDb;

namespace WSIC2010.Mantto
{
    public partial class w_eve_ccocarga : System.Web.UI.Page
    {
        n_app in_app;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/mantto/w_eve_ccocarga.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Administrador_Mantenimientos))
                {
                    if (!IsPostBack)
                    {
                        nf_set_Meses();
                        nf_set_Anios();

                        DDLMes.SelectedIndex = DDLMes.Items.IndexOf(DDLMes.Items.FindByValue(DateTime.Now.Month.ToString()));
                        DDLAnio.SelectedIndex = DDLAnio.Items.IndexOf(DDLAnio.Items.FindByValue(DateTime.Now.Year.ToString()));

                    }
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }
        }

        private void nf_set_Meses()
        {
            //string[] ls_array_Meses = new string[]
            //{"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"};

            //int li_mesInicial = DateTime.Now.Month;

            //switch (li_mesInicial)
            //{
            //    case 1:
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[10], "11"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[11], "12"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[0], "1"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[1], "2"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[2], "3"));
            //        break;
            //    case 2:
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[11], "12"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[0], "1"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[1], "2"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[2], "3"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[3], "4"));
            //        break;
            //    case 3:
            //    case 4:
            //    case 5:
            //    case 6:
            //    case 7:
            //    case 8:
            //    case 9:
            //    case 10:
            //        for (int i = li_mesInicial - 2; i <= li_mesInicial + 2; i++)
            //        {
            //            DDLMes.Items.Add(new ListItem(ls_array_Meses[i - 1], i.ToString()));
            //        }
            //        break;
            //    case 11:
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[8], "9"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[9], "10"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[10], "11"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[11], "12"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[0], "1"));
            //        break;
            //    case 12:
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[9], "10"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[10], "11"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[11], "12"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[0], "1"));
            //        DDLMes.Items.Add(new ListItem(ls_array_Meses[1], "2"));
            //        break;
            //    default:
            //        break;
            //}
            
            //DDLMes.DataBind();

            Dictionary<int, string> lDictionary_Meses = new Dictionary<int, string>();
            lDictionary_Meses.Add(1, "Enero");
            lDictionary_Meses.Add(2, "Febrero");
            lDictionary_Meses.Add(3, "Marzo");
            lDictionary_Meses.Add(4, "Abril");
            lDictionary_Meses.Add(5, "Mayo");
            lDictionary_Meses.Add(6, "Junio");
            lDictionary_Meses.Add(7, "Julio");
            lDictionary_Meses.Add(8, "Agosto");
            lDictionary_Meses.Add(9, "Setiembre");
            lDictionary_Meses.Add(10, "Octubre");
            lDictionary_Meses.Add(11, "Noviembre");
            lDictionary_Meses.Add(12, "Diciembre");
            DDLMes.DataSource = lDictionary_Meses;
            DDLMes.DataValueField = "key";
            DDLMes.DataTextField = "value";
            DDLMes.DataBind();

        }

        private void nf_set_Anios()
        {
            int li_anio_base = DateTime.Now.AddMonths(-2).Year;
            int li_anio_fin = DateTime.Now.AddMonths(+2).Year;
            if (li_anio_base != li_anio_fin)
            {
                for (int i = li_anio_base; i <= li_anio_fin; i++)
                {
                    DDLAnio.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            else
            {
                DDLAnio.Items.Add(new ListItem(li_anio_base.ToString(), li_anio_base.ToString()));
            }
            

            DDLAnio.DataBind();
        }

        protected void btn_carga_Click(object sender, EventArgs e)
        {
            String savePath = @"d:\data\ccocarga\";
            int li_xlsfila = 0;
            lbox_error.DataSource = null;
            lbox_error.Items.Clear();
            lbox_error.DataBind();

            try
            {
                if (fu_carga.HasFile)
                {
                    string fileName = Server.HtmlEncode(fu_carga.FileName);
                    string extension = System.IO.Path.GetExtension(fileName);
                    string strConn = String.Empty;

                    //if (extension == ".xls" || extension == ".xlsx")
                    if (extension == ".xls")
                    {
                        //if (extension == ".xls")
                        //{
                            savePath += fileName.Substring(0, fileName.Length - 4) + DateTime.Now.ToString("_Hmmss") + extension;
                            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";
                        //}

                        //else if (extension == ".xlsx")
                        //{
                        //    savePath += fileName.Substring(0, fileName.Length - 5) + DateTime.Now.ToString("_Hmmss") + extension;
                        //    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + savePath + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\"";
                        //}
                        
                        fu_carga.SaveAs(savePath);

                        DataSet output = new DataSet();

                        //List<int> L_equipos = Mservicio.L_GetCodiEquipos();
                        using (OleDbConnection conn = new OleDbConnection(strConn))
                        {
                            conn.Open();

                            OleDbCommand cmd = new OleDbCommand("SELECT F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15,F16,F17 FROM [Resultados$]", conn);
                            cmd.CommandType = CommandType.Text;

                            DataTable outputTable = new DataTable("Resultados");
                            output.Tables.Add(outputTable);
                            new OleDbDataAdapter(cmd).Fill(outputTable);
                            btn_carga.Enabled = true;
                            string ls_mesactual = DDLAnio.Text + DDLMes.Text.PadLeft(2, '0');
                            DateTime ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(1);
                            string ls_mesinicio = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                            ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(12);
                            string ls_mesfin = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                            int li_equicodi, li_embalse, li_resultado, li_tipoComb;
                            double ld_valor;
                            string ls_valor, ls_bloque, ls_grupoTipo, ls_codigoComb;
                            string ls_cadena = String.Empty;
                            int li_retorno = 0;

                            string ls_mesprogramado, ls_comando;
                            int li_commit = 0;

                            string s1 = outputTable.Rows[4][5].ToString();
                            string s2 = outputTable.Rows[0][0].ToString();
                            if (outputTable.Rows[0][0].ToString() == "0" && outputTable.Rows[4][5].ToString() == DDLAnio.Text)
                            {
                                li_xlsfila = 1;

                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_POPE WHERE MESACTUAL ='" + ls_mesactual + "'");
                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_PONS WHERE MESACTUAL ='" + ls_mesactual + "'");
                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_POCM WHERE MESACTUAL ='" + ls_mesactual + "'");
                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_POCV WHERE MESACTUAL ='" + ls_mesactual + "'");
                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_POLJ WHERE MESACTUAL ='" + ls_mesactual + "'");
                                in_app.iL_data[0].nf_ExecuteNonQuery("DELETE FROM REP_POCC WHERE MESACTUAL ='" + ls_mesactual + "'");

                                in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");

                                int li_reportId;

                                do
                                {
                                    li_reportId = Convert.ToInt32(outputTable.Rows[li_xlsfila][0]);

                                    if (li_reportId > 0)
                                    {
                                        switch (li_reportId)
                                        {
                                            case 110:
                                                li_equicodi = Convert.ToInt32(outputTable.Rows[li_xlsfila][1]);
                                                ls_grupoTipo = Convert.ToString(outputTable.Rows[li_xlsfila][2]).Trim().ToUpper();
                                                ls_codigoComb = Convert.ToString(outputTable.Rows[li_xlsfila][3]).Trim();
                                                if (li_equicodi > 0)
                                                {
                                                    if (ls_grupoTipo.Equals("H") || ls_grupoTipo.Equals("T") || ls_grupoTipo.Equals("E") || ls_grupoTipo.Equals("S"))
                                                    {
                                                        for (int i = 1; i < 13; i++)
                                                        {
                                                            ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(i);
                                                            ls_mesprogramado = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                                                            ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][i + 4]);
                                                            if (double.TryParse(ls_valor, out ld_valor))
                                                            {
                                                                ld_valor = Math.Truncate(ld_valor * 1000) / 1000;
                                                                ls_comando = "INSERT INTO REP_POPE (MESACTUAL, MESINICIO, MESFIN, MESPROGRAMADO,CODIGOGRUPO,VALORPROGRAMADO, TIPOGRUPO, CODIGOCOMB) ";
                                                                ls_comando += "VALUES ('" + ls_mesactual + "','" + ls_mesinicio + "','" + ls_mesfin + "','" + ls_mesprogramado + "','" + li_equicodi.ToString().PadLeft(5, '0') + "'," + ld_valor.ToString() + ",'" + ls_grupoTipo + "','" + ls_codigoComb + "')";
                                                                //int li_retorno = in_app.iL_data[0].nf_ExecuteNonQuery(ls_comando);
                                                                li_retorno = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);
                                                                li_commit++;

                                                                if (li_retorno == -1)
                                                                {
                                                                    lbox_error.Items.Add(ls_cadena);
                                                                }

                                                                if (li_commit % 100 == 0)
	                                                            {
                                                                    in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");
	                                                            } 
                                                                
                                                            }
                                                            else
                                                            {
                                                                lbox_error.Items.Add("Error en el valor programado en la fila " + (li_xlsfila + 2) + " columna " + Util.ExcelUtil.GetExcelColumnName(i + 5));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en el codigo del tipo de grupo en la fila " + (li_xlsfila + 2) + " columna C");
                                                    }
                                                }
                                                else
                                                {
                                                    lbox_error.Items.Add("Error en el codigo Osinerg en la fila " + (li_xlsfila + 2) + " columna B");
                                                }
                                                li_reportId = li_equicodi = 0;
                                                ls_grupoTipo = String.Empty;
                                                break;

                                            case 111:
                                                for (int i = 1; i < 13; i++)
                                                {
                                                    ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(i);
                                                    ls_mesprogramado = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                                                    ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][i + 4]);
                                                    if (double.TryParse(ls_valor, out ld_valor))
                                                    {
                                                        ls_comando = "INSERT INTO REP_PONS (MESACTUAL, MESINICIO, MESFIN, MESPROGRAMADO, VALORDEFICIT) ";
                                                        ls_comando += "VALUES ('" + ls_mesactual + "','" + ls_mesinicio + "','" + ls_mesfin + "','" + ls_mesprogramado + "'," + Math.Round(ld_valor, 3).ToString() + ")";
                                                        li_retorno = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);
                                                        li_commit++;

                                                        if (li_retorno == -1)
                                                        {
                                                            lbox_error.Items.Add(ls_cadena);
                                                        }

                                                        if (li_commit % 100 == 0)
                                                        {
                                                            in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");
                                                        } 
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en el valor programado en la fila " + (li_xlsfila + 2) + " columna " + Util.ExcelUtil.GetExcelColumnName(i + 5));
                                                    }
                                                }
                                                li_reportId = 0;
                                                break;
                                            case 112:
                                                ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][2]);
                                                if (int.TryParse(ls_valor, out li_equicodi))
                                                {
                                                    ls_bloque = Convert.ToString(outputTable.Rows[li_xlsfila][1]);
                                                    if (li_equicodi > 0 && !String.IsNullOrEmpty(ls_bloque))
                                                    {
                                                        for (int i = 1; i < 13; i++)
                                                        {
                                                            ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(i);
                                                            ls_mesprogramado = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                                                            ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][i + 4]);
                                                            if (double.TryParse(ls_valor, out ld_valor))
                                                            {
                                                                ls_comando = "INSERT INTO REP_POCM (MESACTUAL, MESINICIO, MESFIN, MESPROGRAMADO,CODIGOBARRA,CODIGOBLOQUEHORARIO,VALORPROGRAMADO) ";
                                                                ls_comando += "VALUES ('" + ls_mesactual + "','" + ls_mesinicio + "','" + ls_mesfin + "','" + ls_mesprogramado + "','" + li_equicodi.ToString().PadLeft(5, '0') + "','" + ls_bloque + "'," + Math.Round(ld_valor, 3).ToString() + ")";
                                                                li_retorno = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);
                                                                li_commit++;

                                                                if (li_retorno == -1)
                                                                {
                                                                    lbox_error.Items.Add(ls_cadena);
                                                                }

                                                                if (li_commit % 100 == 0)
                                                                {
                                                                    in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");
                                                                } 
                                                            }
                                                            else
                                                            {
                                                                lbox_error.Items.Add("Error en el valor programado en la fila " + (li_xlsfila + 2) + " columna " + Util.ExcelUtil.GetExcelColumnName(i + 5));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en el codigo Osinerg en la fila " + (li_xlsfila + 2) + " columna C");
                                                    }
                                                }
                                                else
                                                {
                                                    lbox_error.Items.Add("Error en el codigo Osinerg en la fila " + (li_xlsfila + 2) + " columna C");
                                                }
                                                li_reportId = 0;
                                                break;
                                            case 114:
                                                ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][2]);
                                                if (int.TryParse(ls_valor, out li_embalse))
                                                {
                                                    ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][1]);
                                                    if (int.TryParse(ls_valor, out li_resultado))
                                                    {
                                                        for (int i = 1; i < 13; i++)
                                                        {
                                                            ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(i);
                                                            ls_mesprogramado = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                                                            ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][i + 4]);
                                                            if (double.TryParse(ls_valor, out ld_valor))
                                                            {
                                                                ls_comando = "INSERT INTO REP_POLJ(MESACTUAL, MESINICIO, MESFIN, MESPROGRAMADO, CODIGOEMBALSE, CODIGORESULTADOS, VALORPROGRAMADO) ";
                                                                ls_comando += "VALUES ('" + ls_mesactual + "','" + ls_mesinicio + "','" + ls_mesfin + "','" + ls_mesprogramado + "','" + li_embalse.ToString().PadLeft(3, '0') + "','" + li_resultado.ToString().PadLeft(2, '0') + "'," + Math.Round(ld_valor, 3).ToString() + ")";
                                                                li_retorno = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);
                                                                li_commit++;

                                                                if (li_retorno == -1)
                                                                {
                                                                    lbox_error.Items.Add(ls_cadena);
                                                                }

                                                                if (li_commit % 100 == 0)
                                                                {
                                                                    in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");
                                                                } 
                                                            }
                                                            else
                                                            {
                                                                lbox_error.Items.Add("Error en el valor programado en la fila " + (li_xlsfila + 2) + " columna " + Util.ExcelUtil.GetExcelColumnName(i + 5));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en el codigo de resultado en la fila " + (li_xlsfila + 2) + "columna B");
                                                    }
                                                }
                                                else
                                                {
                                                    lbox_error.Items.Add("Error en el codigo de embalse en la fila " + (li_xlsfila + 2) + " columna C");
                                                }
                                                li_reportId = li_embalse = li_resultado = 0;
                                                break;
                                            case 115:
                                                ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][1]);
                                                if (int.TryParse(ls_valor, out li_equicodi))
                                                {
                                                    ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][2]);
                                                    if (int.TryParse(ls_valor, out li_tipoComb))
                                                    {
                                                        for (int i = 1; i < 13; i++)
                                                        {
                                                            ldt_fecha = new DateTime(Convert.ToInt32(DDLAnio.Text), Convert.ToInt32(DDLMes.Text), 1).AddMonths(i);
                                                            ls_mesprogramado = ldt_fecha.Year.ToString() + ldt_fecha.Month.ToString().PadLeft(2, '0');
                                                            ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][i + 4]);
                                                            if (double.TryParse(ls_valor, out ld_valor))
                                                            {
                                                                ls_comando = "INSERT INTO REP_POCC(MESACTUAL, MESINICIO, MESFIN, MESPROGRAMADO, CODIGOGRUPO,CODIGOTIPOCOMBUSTIBLE,VALORPROGRAMADO) ";
                                                                ls_comando += "VALUES ('" + ls_mesactual + "','" + ls_mesinicio + "','" + ls_mesfin + "','" + ls_mesprogramado + "','" + li_equicodi.ToString().PadLeft(5, '0') + "','" + li_tipoComb.ToString().PadLeft(2, '0') + "'," + ld_valor + ")";
                                                                li_retorno = in_app.iL_data[0].nf_ExecuteNonQueryWithMessage(ls_comando, out ls_cadena);
                                                                li_commit++;

                                                                if (li_retorno == -1)
                                                                {
                                                                    lbox_error.Items.Add(ls_cadena);
                                                                }

                                                                if (li_commit % 100 == 0)
                                                                {
                                                                    in_app.iL_data[0].nf_ExecuteNonQuery("COMMIT");
                                                                } 
                                                            }
                                                            else
                                                            {
                                                                lbox_error.Items.Add("Error en el valor programado en la fila " + (li_xlsfila + 2) + " columna " + Util.ExcelUtil.GetExcelColumnName(i + 5));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en el codigo combustible en la fila " + (li_xlsfila + 2) + " columna C");
                                                    }
                                                }
                                                else
                                                {
                                                    lbox_error.Items.Add("Error en el codigo Osinerg en la fila " + (li_xlsfila + 2) + " columna B");
                                                }
                                                li_reportId = li_equicodi = li_tipoComb = 0;
                                                break;
                                            default:
                                                li_reportId = 0;
                                                break;
                                        }
                                    }

                                    li_xlsfila++;

                                } while (!String.IsNullOrEmpty(outputTable.Rows[li_xlsfila][0].ToString()));

                                if (lbox_error.Items.Count.Equals(0))
                                {
                                    lbox_error.Items.Add("Los datos se cargaron exitosamente. Se procesaron " + (li_xlsfila + 2) + " registros.");
                                }
                            }

                        }
                    }
                    else
                    {
                        //lbox_error.Items.Add("Your file was not uploaded because it does not have a .xls or .xlsx extension.");
                        lbox_error.Items.Add("Your file was not uploaded because it does not have a .xls extension.");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Alert.Show("Error: " + ex.Message + li_xlsfila);
            }

        }


    }
}