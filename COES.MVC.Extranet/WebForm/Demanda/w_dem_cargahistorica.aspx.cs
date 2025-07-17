using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WScoes;
using System.Data;
using System.Data.OleDb;

namespace WSIC2010.Demanda
{
    public partial class w_dem_cargahistorica : System.Web.UI.Page
    {
        n_app in_app;
        int[] li_array_codigoAreas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["in_app"] == null)
            {
                Session["ReturnPage"] = "~/WebForm/Demanda/w_dem_cargahistorica.aspx";
                Response.Redirect("~/WebForm/Account/Login.aspx");
            }
            else
            {
                in_app = (n_app)Session["in_app"];
                AdminService admin = new AdminService();

                if (admin.IsUserModulo(in_app.ii_UserCode, (int)Role.Usuario_Demanda) || li_array_codigoAreas.Contains(in_app.ii_AreaCode))
                {
                    fu_carga.Enabled = true;
                    btn_carga.Enabled = true;
                }
                else
                {
                    Response.Redirect("~/WebForm/Admin/w_adm_denegado.aspx", true);
                }

            }
        }

        protected void btn_carga_Click(object sender, EventArgs e)
        {
            String savePath = @"d:\data\demandaHistorica\";
            int li_xlsfila = 1;
            int li_xlscolumna = 0;
            int li_xlsfila_max;
            int li_valor = 0;
            double ld_valor = 0;
            string ls_valor = String.Empty;
            lbox_error.DataSource = null;
            lbox_error.Items.Clear();
            lbox_error.DataBind();
            IDemandaService demanda = new DemandaService();
            

            try
            {
                if (fu_carga.HasFile)
                {
                    string fileName = Server.HtmlEncode(fu_carga.FileName);
                    string extension = System.IO.Path.GetExtension(fileName);
                    string strConn = String.Empty;
                    List<CEquipoDemanda> AL_Equipos = new List<CEquipoDemanda>();

                    if (extension == ".xls")
                    {
                        savePath += fileName.Substring(0, fileName.Length - 4) + DateTime.Now.ToString("_Hmmss") + extension;
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties=" + (char)34 + "Excel 8.0;HDR=No;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";

                        fu_carga.SaveAs(savePath);

                        DataSet output = new DataSet();

                        //List<int> L_equipos = Mservicio.L_GetCodiEquipos();
                        using (OleDbConnection conn = new OleDbConnection(strConn))
                        {
                            conn.Open();

                            OleDbCommand cmd = new OleDbCommand("SELECT F1,F2,F3,F4,F5,F6,F7,F8,F9 FROM [Sheet1$]", conn);
                            cmd.CommandType = CommandType.Text;

                            DataTable outputTable = new DataTable("Equipo_Barra_PtoMedicodi");
                            output.Tables.Add(outputTable);
                            new OleDbDataAdapter(cmd).Fill(outputTable);
                            btn_carga.Enabled = true;

                            string ls_sql = String.Empty;
                            DataSet i_ds = new DataSet("dsEquipo");
                            li_xlscolumna = 0;
                            li_xlsfila_max = outputTable.Rows.Count;
                            

                            while (li_xlsfila < li_xlsfila_max)
                            {
                                ls_valor = Convert.ToString(outputTable.Rows[li_xlsfila][li_xlscolumna]);

                                if (Int32.TryParse(ls_valor, out li_valor))
	                            {
                                    CEquipoDemanda equipoXLS = new CEquipoDemanda();
                                    equipoXLS.ii_equicodi = li_valor;
                                    CEquipoDemanda equipoBD = demanda.GetEquipo(li_valor);

                                    if (!String.IsNullOrEmpty(outputTable.Rows[li_xlsfila][li_xlscolumna + 1].ToString()) && !String.IsNullOrEmpty(outputTable.Rows[li_xlsfila][li_xlscolumna + 2].ToString()))
                                    {
                                        equipoXLS.is_equinomb = outputTable.Rows[li_xlsfila][li_xlscolumna + 1].ToString();
                                        equipoXLS.is_equiabrev = outputTable.Rows[li_xlsfila][li_xlscolumna + 2].ToString();

                                        if (Int32.TryParse(outputTable.Rows[li_xlsfila][li_xlscolumna + 3].ToString(), out li_valor))
                                        {
                                            equipoXLS.ii_famcodi = li_valor;

                                            if (double.TryParse(outputTable.Rows[li_xlsfila][li_xlscolumna + 4].ToString(), out ld_valor))
                                            {
                                                equipoXLS.id_equitension = ld_valor;

                                                if (Int32.TryParse(outputTable.Rows[li_xlsfila][li_xlscolumna + 5].ToString(), out li_valor))
                                                {
                                                    equipoXLS.ii_areacodi = li_valor;

                                                    if (Int32.TryParse(outputTable.Rows[li_xlsfila][li_xlscolumna + 6].ToString(), out li_valor))
                                                    {
                                                        equipoXLS.ii_emprcodi = li_valor;

                                                        //Comparar y validar
                                                        if (!nf_get_bool_diferencias(equipoBD, equipoXLS, li_xlsfila))
                                                        {
                                                            AL_Equipos.Add(equipoXLS);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lbox_error.Items.Add("Error en código de Empresa " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(6) + (li_xlsfila + 1));
                                                    }
                                                }
                                                else
                                                {
                                                    lbox_error.Items.Add("Error en código de Area " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(5) + (li_xlsfila + 1));
                                                }
                                            }
                                            else
                                            {
                                                lbox_error.Items.Add("Error en tipo de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(4) + (li_xlsfila + 1));
                                            }
                                        }
                                        else
                                        {
                                            
                                        }
                                        
                                    }
                                    else
                                    {
                                        lbox_error.Items.Add("Error en Nombre o Abrev. de Equipo " + " Celdas : " + Util.ExcelUtil.GetExcelColumnName(2) + (li_xlsfila + 1) + "ó" + Util.ExcelUtil.GetExcelColumnName(3) + (li_xlsfila + 1));
                                    }
                                    
	                            }
                                else
                                {
                                    lbox_error.Items.Add("Error en el código de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(1) + (li_xlsfila + 1));
                                }

                                li_xlsfila++;
                            }
                        }
                    }

                    foreach (CEquipoDemanda demandaEquipo in AL_Equipos)
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Alert.Show("Error: En la fila: " + (li_xlsfila + 1) + ", " + ex.Message );
            }

            
        }

        private bool nf_get_bool_diferencias(CEquipoDemanda equipoBD, CEquipoDemanda equipoXLS, int li_xlsfila)
        {
            bool lb_existe = false;
            if (equipoBD.ii_equicodi != equipoXLS.ii_equicodi)
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinto código de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(1) + (li_xlsfila + 1));
            }
            if (!equipoBD.is_equinomb.Equals(equipoXLS.is_equinomb))
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinto nombre de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(2) + (li_xlsfila + 1));
            }
            if (!equipoBD.is_equiabrev.Equals(equipoXLS.is_equiabrev))
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinta abreviatura de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(3) + (li_xlsfila + 1));
            }
            if (equipoBD.ii_famcodi != equipoXLS.ii_famcodi)
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinto tipo de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(4) + (li_xlsfila + 1));
            }
            if (equipoBD.id_equitension != equipoXLS.id_equitension)
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinta tensión de Equipo " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(5) + (li_xlsfila + 1));
            }
            if (equipoBD.ii_areacodi != equipoXLS.ii_areacodi)
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinto código de Área " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(6) + (li_xlsfila + 1));
            }
            if (equipoBD.ii_emprcodi != equipoXLS.ii_emprcodi)
            {
                lb_existe = true;
                lbox_error.Items.Add("Distinto código de Empresa " + " Celda : " + Util.ExcelUtil.GetExcelColumnName(7) + (li_xlsfila + 1));
            }

            return lb_existe;
        }
    }
}