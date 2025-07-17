using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using ExcelLibrary.SpreadSheet;
using WSIC2010.Util;
using System.Diagnostics;

namespace WSIC2010.Demanda
{
    public partial class w_me_carga : System.Web.UI.Page
    {
        string ls_basePath = @"d:\data\demanda\";
        string ls_path = String.Empty;
        string ls_nombreExcel = String.Empty;
        string ls_strConn = String.Empty;
        //bool lb_nombre = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties=" + (char)34 + "Excel 8.0;IMEX=1;" + (char)34; //\"Excel 8.0;HDR=No;IMEX=1\"";
            string strConn = String.Empty;
            ListBox1.Items.Clear();
            ListBox1.DataSource = null;
            ListBox1.DataBind();

            if (FileUpload1.HasFile)
            {
                string fileName = Server.HtmlEncode(FileUpload1.FileName);
                string extension = System.IO.Path.GetExtension(fileName);

                if (extension.Equals(".xls"))
                {
                    ls_basePath += fileName.Substring(0, fileName.Length - 4) + DateTime.Now.ToString("_Hmmss") + ".xls";
                    FileUpload1.SaveAs(ls_basePath);

                    ////strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + ls_basePath + "';Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
                    //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + ls_basePath + "';Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\""; //Esta cadena sirve para ver solo numeros en las hojas.
                    ////ps_conex_string = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @ls_path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\"";

                    //DataSet output = new DataSet();
                    //string xlWorksheet = "HISTORICO";
                    //using (OleDbConnection conn = new OleDbConnection(strConn))
                    //{
                    //    conn.Open();

                    //    using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + xlWorksheet + "$]", conn))
                    //    {
                    //        cmd.CommandType = CommandType.Text;

                    //        DataTable outputTable = new DataTable("HISTORICOS");
                    //        output.Tables.Add(outputTable);
                    //        new OleDbDataAdapter(cmd).Fill(outputTable);

                    //        cmd.Dispose();
                    //    }
                    //}

                    //string ls_file = as_path;
                    //DataTable dt = new DataTable();
                    Stopwatch stopw = new Stopwatch();
                    stopw.Start();
                    ListBox1.Items.Add("Proceso de Carga Iniciado!");
                    Workbook wb = Workbook.Load(ls_basePath);
                    string ls_cabecera = String.Empty;
                    List<Pronostico_Demanda> AL_demandas = new List<Pronostico_Demanda>();
                    double ld_valor;

                    foreach (var item in wb.Worksheets)
                    {
                        int li_contador = 0;
                        if (IsValidName(item.Name))
                        {
                            DateTime dtime = item.Cells[14, 2].DateTimeValue;
                            ListBox1.Items.Add(dtime.ToString("dd/MM/yyyy H:mm:ss") + " hoja: " + item.Name);
                            do
                            {
                                ls_cabecera = item.Cells[0, li_contador].StringValue;
                                Pronostico_Demanda demanda;

                                if (Convert.ToInt32(ls_cabecera) > 0)
                                {
                                    demanda = new Pronostico_Demanda();
                                    demanda.Lugar = item.Cells[16, li_contador].StringValue;
                                    demanda.FuenteInfo = item.Cells[17, li_contador].StringValue;
                                    demanda.TensionBarra = item.Cells[18, li_contador].StringValue;
                                    demanda.Descripcion = item.Cells[19, li_contador].StringValue;
                                    demanda.CodigoBarra = item.Cells[20, li_contador].StringValue;
                                    double[] ld_array_valores = new double[96];

                                    for (int i = 21; i < 117; i++)
                                    {
                                        if (double.TryParse(item.Cells[i, li_contador].StringValue, out ld_valor))
                                        {
                                            if (ld_valor >= 0)
                                            {
                                                ld_array_valores[i - 21] = ld_valor;
                                            }
                                            else
                                            {
                                                ListBox1.Items.Add("Error en la columna " + Util.ExcelUtil.GetExcelColumnName(li_contador + 1) + (i + 1) + ": Valor negativo.");
                                            }
                                        }
                                        else
                                        {
                                            ListBox1.Items.Add("Error en la columna " + Util.ExcelUtil.GetExcelColumnName(li_contador + 1) + (i + 1) + ": Valor inválido.");
                                        }
                                    }

                                    demanda.ld_array_demanda96 = ld_array_valores;

                                    AL_demandas.Add(demanda);
                                    //ListBox1.Items.Add("Se agregó la columna " + Util.ExcelUtil.GetExcelColumnName(li_contador + 1));
                                }

                                li_contador++;

                            } while (!String.IsNullOrEmpty(item.Cells[0, li_contador].StringValue));
                        }

                    }

                    stopw.Stop();
                    ListBox1.Items.Add("Proceso de Carga Finalizó en " + stopw.Elapsed.Minutes + " minutos");
                    //return dt;
                }
                else
                {
                    ListBox1.Items.Add("El archivo " + fileName + " , presenta una extension inválida");
                }
            }
            else
            {
                ListBox1.Items.Add("No se seleccionó archivo");
            }
        }

        private static bool IsValidName(string ps_wsName)
        {
            if ((ps_wsName.ToUpper().Trim() == "HISTORICO") || (ps_wsName.ToUpper().Trim() == "PREVISTO"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private Pronostico_Demanda nf_get_puntoMedicion()
        //{
            
        //}
    }
}