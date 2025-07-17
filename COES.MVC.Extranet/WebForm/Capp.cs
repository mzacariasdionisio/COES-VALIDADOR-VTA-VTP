using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using WScoes;
using System.Configuration;

namespace WSIC2010
{
    public class n_app:fwapp.n_fw_app
    {
        public n_app()
        {
            iL_data[0] = new fwapp.n_fw_data(fwapp.DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES_OLEDB"].ToString());
            iL_data[1] = new fwapp.n_fw_data(fwapp.DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA_OLEDB"].ToString());
        }

        public string is_EmpresasNombres()
        {
            string nombres ="";
            foreach(string s_empresa in this.Ls_emprcodi)
            {
                ManttoService service = new ManttoService();
                nombres+= service.GetEmpresaNombre(Convert.ToInt32(s_empresa)) +" ";
            }

            return nombres;
        }

        public void Fill(int ai_IdConnection, DataSet a_dataset, string as_tablename, string as_SelecCommand)
        {
            if (ai_IdConnection == 0)
            {                
                //ManttoServiceClient service = new ManttoServiceClient();
                ManttoService service = new ManttoService();
                int ii_key = service.Register("sic", 47896);

                string ls_commmand = "";
                as_SelecCommand = as_SelecCommand.Trim();
                int li_indexof = as_SelecCommand.IndexOf(' ');
                if (li_indexof >= 0 && li_indexof < as_SelecCommand.Length)
                {
                    ls_commmand = as_SelecCommand.Substring(0, li_indexof);

                    if (ls_commmand.Trim().ToUpper() == "SELECT")
                        ls_commmand = "SELECT|" + as_SelecCommand.Substring(li_indexof + 1).Trim();

                    DataTable aa = service.GetDTData(ls_commmand, ii_key);
                    a_dataset.Tables.Add(aa);
                    aa.TableName = as_tablename;
                }
            }
            else
            {
                iL_data[ai_IdConnection].Fill(a_dataset, as_tablename, as_SelecCommand);
            }
        }
    }
    public class Cutils
    {
        public static void ExportDataTableToCSV(DataTable table, string filename)
        {

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            context.Response.End();
        }
    }
}