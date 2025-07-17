using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;
//using System.Collections.Generic;
//using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using COES.Servicios.Aplicacion.WLib;

namespace fwapp
{
   public enum DBType : int
   {
      SqlServer,
      OleDb,
      Odbc,
      Oracle,
      Informix,
      DB2
   }

   public class n_fw_data
   {
       //CONSTANTES DE CONEXION A BASE DE DATOS
       // public n_fw_app in_app = null;
       public string is_ConnectionString;
       DBType i_DBType = DBType.OleDb;
       public IDbConnection i_conn = null;
       public IDbCommand i_cmd = null;
       public IDataReader i_reader = null;
       public IDbDataAdapter i_adapter = null;

       WLibAppServicio servicio= new WLibAppServicio();

       public n_fw_data(DBType a_dbtype, string as_ConnectionString)
       {
           //in_app = an_app;
           i_DBType = a_dbtype;
           is_ConnectionString = as_ConnectionString;
           switch (i_DBType)
           {
               case DBType.SqlServer:
                   i_conn = new SqlConnection(is_ConnectionString);
                   break;
               case DBType.OleDb:
                   i_conn = new OleDbConnection(is_ConnectionString);
                   break;
               case DBType.Odbc:
                   //i_conn = new OdbcConnection(is_ConnectionString);
                   break;
               case DBType.Oracle:
                   i_conn = new OracleConnection(is_ConnectionString);
                   break;
               default:
                   break;
           }
       }

       public IDbConnection GetConnection(DBType a_connType, string a_connString)
       {
           IDbConnection idbConn = null;
           switch (a_connType)
           {
               case DBType.OleDb: // OleDb Data Provider
                   idbConn = new OleDbConnection(a_connString);
                   break;
               case DBType.SqlServer: // Sql Data Provider
                   idbConn = new SqlConnection(a_connString);
                   break;
               case DBType.Odbc: // ODBC Data Provider
                   //idbConn = new OdbcConnection(connString);
                   break;
               case DBType.Oracle:	// Add your custom data provider
                   idbConn = new OracleConnection(a_connString);
                   break;
               default:
                   break;
           }
           return idbConn;
       }

       public IDbDataAdapter GetDataAdapter(string a_sql)
       {
           IDbDataAdapter idbAdapter = null;
           switch (i_DBType)
           {
               case DBType.OleDb: // OleDb Data Provider
                   idbAdapter = new OleDbDataAdapter(a_sql, (OleDbConnection)this.i_conn);
                   break;
               case DBType.SqlServer: // Sql Data Provider
                   idbAdapter = new SqlDataAdapter(a_sql, (SqlConnection)this.i_conn);
                   break;
               case DBType.Odbc: // ODBC Data Provider
                   //idbAdapter = new OdbcDataAdapter(a_sql, this.i_conn); 
                   break;
               case DBType.Oracle: // Add your custom data provider
                   idbAdapter = new OracleDataAdapter(a_sql, (OracleConnection)this.i_conn);
                   break;
               default:
                   break;
           }
           return idbAdapter;
       }

       public int Fill(DataSet a_ds, string as_TableName, string as_commands)
       {
           switch (this.i_DBType)
           {
               case DBType.OleDb: // OleDb Data Provider
                   using (OleDbDataAdapter da = (OleDbDataAdapter)this.GetDataAdapter(as_commands))
                   {
                       return da.Fill(a_ds, as_TableName);
                   }
                   //break;
               case DBType.SqlServer: // Sql Data Provider
                   using (SqlDataAdapter da = (SqlDataAdapter)this.GetDataAdapter(as_commands))
                   {
                       return da.Fill(a_ds, as_TableName);
                       //((SqlDataAdapter)a_da).Fill(a_ds,as_names);
                   }
                   //break;
               case DBType.Odbc: // ODBC Data Provider
                   //idbAdapter = new OdbcDataAdapter(a_sql, this.i_conn); 
                   break;
               case DBType.Oracle: // Sql Data Provider
                   using (OracleDataAdapter da = (OracleDataAdapter)this.GetDataAdapter(as_commands))
                    {
                        // var t = typeof(Oracle.ManagedDataAccess.Client.OracleConnection);
                        // var myAssemblyName = AssemblyName.GetAssemblyName(t.Assembly.Location);
                        // return da.Fill(a_ds, as_TableName);
                        ////((SqlDataAdapter)a_da).Fill(a_ds,as_names);
                        ///

                        return da.Fill(a_ds, as_TableName);
                    }
                   //break;
               default:
                   break;
           }
           return -1;
       }

       public int Fill(DataSet a_ds, string as_TableName, string as_commands, string as_parameter)
       {
           switch (this.i_DBType)
           {
               case DBType.OleDb: // OleDb Data Provider
                   using (OleDbDataAdapter da = (OleDbDataAdapter)this.GetDataAdapter(as_commands))
                   {
                       return da.Fill(a_ds, as_TableName);
                   }
               //break;
               case DBType.SqlServer: // Sql Data Provider
                   using (SqlDataAdapter da = (SqlDataAdapter)this.GetDataAdapter(as_commands))
                   {
                       return da.Fill(a_ds, as_TableName);
                       //((SqlDataAdapter)a_da).Fill(a_ds,as_names);
                   }
               //break;
               case DBType.Odbc: // ODBC Data Provider
                   //idbAdapter = new OdbcDataAdapter(a_sql, this.i_conn); 
                   break;
               case DBType.Oracle: // Sql Data Provider
                   using (OracleDataAdapter da = (OracleDataAdapter)this.GetDataAdapter(as_commands))
                   {
                       da.SelectCommand.Parameters.Add(new OracleParameter() { ParameterName = "param", Value = as_parameter, OracleDbType = OracleDbType.Varchar2 });
                       return da.Fill(a_ds, as_TableName);
                       //((SqlDataAdapter)a_da).Fill(a_ds,as_names);
                   }
               //break;
               default:
                   break;
           }
           return -1;
       }

       //public bool nf_GetFirstDataRow(string as_SqlCommandText)
       //{
       //   bool lb_isopen = false;
       //   try
       //   {
       //      Fill(
       //   }
       //   catch (Exception ex)
       //   {
       //      MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
       //      return false;
       //   }
       //   return true;
       //}


       public int nf_ExecuteNonQuery(string as_SqlCommandText)
       {
           bool lb_isopen = false;
           int li_retorno = -1;
           try
           {
               IDbCommand cmd = i_conn.CreateCommand();
               cmd.CommandText = as_SqlCommandText;
               if (i_conn.State == ConnectionState.Open) lb_isopen = true;
               if (!lb_isopen) i_conn.Open();
               li_retorno = cmd.ExecuteNonQuery();
               cmd.Dispose();
               if (!lb_isopen) i_conn.Close();
           }
           catch (Exception)
           {
               //MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
               return -1;
           }
           return li_retorno;
       }

       public int nf_ExecuteNonQueryWithMessage(string as_SqlCommandText, out string ls_cadena)
       {
           bool lb_isopen = false;
           int li_retorno = -1;
           ls_cadena = String.Empty;
           try
           {
               IDbCommand cmd = i_conn.CreateCommand();
               cmd.CommandText = as_SqlCommandText;
               if (i_conn.State == ConnectionState.Open) lb_isopen = true;
               if (!lb_isopen) i_conn.Open();
               li_retorno = cmd.ExecuteNonQuery();
               cmd.Dispose();
               if (!lb_isopen) i_conn.Close();
           }
           catch (Exception ex)
           {
               //MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
               ls_cadena = ex.Message;
               return -1;
           }
           return li_retorno;
       }

       //public int nf_ExecuteNonQueryWithParameters(string as_SqlCommandText, out string ls_cadena)
       //{
       //    bool lb_isopen = false;
       //    int li_retorno = -1;
       //    ls_cadena = String.Empty;
       //    try
       //    {
       //        IDbCommand cmd = i_conn.CreateCommand();
       //        cmd.CommandText = " INSERT INTO ME_MEDICION_96 VALUES(";
       //        cmd.CommandText += ":MEDIFECHA_p, ";
       //        cmd.CommandText += ":TIPOINFOCODI_p, ";
       //        cmd.CommandText += ":PTOMEDICODI_p, ";
       //        cmd.CommandText += ":MEDITOTAL_p, ";
       //        for (int i = 1; i < 97; i++)
       //        {
       //            cmd.CommandText += ":H" + i + "_p, ";

       //            if (i == 96)
       //            {
       //                cmd.CommandText += ":H" + i + "_p)";
       //            }
       //        }


       //        if (i_conn.State == ConnectionState.Open) lb_isopen = true;
       //        if (!lb_isopen) i_conn.Open();
       //        li_retorno = cmd.ExecuteNonQuery();
       //        cmd.Dispose();
       //        if (!lb_isopen) i_conn.Close();
       //    }
       //    catch (Exception ex)
       //    {
       //        //MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
       //        ls_cadena = ex.Message;
       //        return -1;
       //    }
       //    return li_retorno;
       //}

       public int nf_ExecuteScalar_GetInteger(string as_SqlCommandText)
       {
           int li_temp = 0;
           bool lb_isopen = false;
           try
           {
               IDbCommand cmd = i_conn.CreateCommand();
               cmd.CommandText = as_SqlCommandText;
               if (i_conn.State == ConnectionState.Open) lb_isopen = true;
               if (!lb_isopen) i_conn.Open();
               li_temp = Convert.ToInt32(cmd.ExecuteScalar());
               if (!lb_isopen) i_conn.Close();
           }
           catch (Exception)
           {
               //MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
               li_temp = -1;
           }
           return li_temp;
       }

       public object nf_ExecuteScalar(string as_SqlCommandText)
       {
           object li_temp;
           bool lb_isopen = false;
           try
           {
               IDbCommand cmd = i_conn.CreateCommand();
               cmd.CommandText = as_SqlCommandText;
               if (i_conn.State == ConnectionState.Open) lb_isopen = true;
               if (!lb_isopen) i_conn.Open();
               li_temp = cmd.ExecuteScalar();
               if (!lb_isopen) i_conn.Close();
           }
           catch (Exception ex)
           {
               //MessageBox.Show("Error in " + as_SqlCommandText + " -> " + ex.Message);
               li_temp = null;
           }
           return li_temp;
       }

       public int nf_GetMaxCodi()
       {
            //return nf_ExecuteScalar_GetInteger("select max(" + as_pk + ") from " + as_table);
            return servicio.GetMaxCodi();
       }

       public DateTime Now()
       {
           bool lb_isopen = false;
           try
           {
               switch (this.i_DBType)
               {

                   case DBType.OleDb: // OleDb Data Provider
                       IDbCommand l_cmd = i_conn.CreateCommand();
                       l_cmd.CommandText = "SELECT SYSDATE FROM DUAL";
                       if (i_conn.State == ConnectionState.Open) lb_isopen = true;
                       if (!lb_isopen) i_conn.Open();
                       DateTime temphoy = (DateTime)l_cmd.ExecuteScalar();
                       if (!lb_isopen) i_conn.Close();
                       return temphoy;
                   case DBType.SqlServer: // Sql Data Provider
                       return DateTime.Today;
                   case DBType.Odbc: // ODBC Data Provider
                       return DateTime.Today;
                   case DBType.Oracle: // Sql Data Provider
                       return DateTime.Today;
               }
           }
           catch (Exception)
           {
               //MessageBox.Show("Error in n_fw_data.Now() -> " + ex.Message);
           }
           return new DateTime(1, 1, 1, 0, 0, 0);
       }

       public DateTime Today()
       {
           bool lb_isopen = false;
           try
           {
               switch (this.i_DBType)
               {
                   case DBType.OleDb: // OleDb Data Provider
                       IDbCommand l_cmd = i_conn.CreateCommand();
                       l_cmd.CommandText = "SELECT SYSDATE FROM DUAL";
                       if (i_conn.State == ConnectionState.Open) lb_isopen = true;
                       if (!lb_isopen) i_conn.Open();
                       DateTime temphoy = (DateTime)l_cmd.ExecuteScalar();
                       if (!lb_isopen) i_conn.Close();
                       return temphoy.Date;
                   case DBType.SqlServer: // Sql Data Provider
                       return DateTime.Today;
                   case DBType.Odbc: // ODBC Data Provider
                       return DateTime.Today;
                   case DBType.Oracle: // Sql Data Provider
                       return DateTime.Today;
               }
           }
           catch (Exception)
           {
               //MessageBox.Show("Error in n_fw_data.Today() -> " + ex.Message);
           }
           return new DateTime(1, 1, 1, 0, 0, 0);
       }

       public int nf_get_next_key(string as_table)
       {
           int ll_counter = 0;
           servicio.UpdateMaxCount(as_table.Trim());
           ll_counter = servicio.GetMaxCount(as_table);
           return ll_counter;
       }

       //public void nf_fwlog(string as_APP_10, string as_OBJECT_20, int ai_CODI, DateTime adt_DATE, string as_OPTION_5, string as_VALUE_20, int ai_CODI2)
       //{
       //    string ls_object = as_OBJECT_20;
       //    if (ls_object.Length > 20)
       //        ls_object = ls_object.Substring(0, 20);
       //    string ls_comando = "INSERT INTO FW_DATALOG (L_APP, L_OBJECT, L_CODI, L_DATE, L_OPTION, L_VALUE, L_CODI2, L_XUSER, L_XDATETIME) VALUES (";
       //    ls_comando += "'" + as_APP_10 + "','" + ls_object + "'," + ai_CODI + "," + EPDate.SQLDateTimeOracleString(adt_DATE) +
       //       " ,'" + as_OPTION_5 + "','" + as_VALUE_20 + "'," + ai_CODI2 + ",'";
       //    string slocal = in_app.is_PCComputerName + "/" + in_app.is_PCUserName;

       //    if (slocal.Length > 20)
       //        slocal = slocal.Substring(0, 20);

       //    ls_comando += slocal + "',sysdate)";

       //    try
       //    {
       //        nf_ExecuteNonQuery(ls_comando);
       //    }
       //    catch
       //    {
       //        ls_comando = "INSERT INTO FW_DATALOG (L_APP, L_OBJECT, L_CODI, L_DATE, L_OPTION, L_VALUE, L_CODI2, L_XUSER, L_XDATETIME) VALUES (";
       //        ls_comando += "'FALLA','FALLA',-1," + EPDate.SQLDateTimeOracleString(adt_DATE) +
       //           " ,'','',-1,'" + in_app.is_PCComputerName + "/" + in_app.is_PCUserName + "',sysdate)";
       //        nf_ExecuteNonQuery(ls_comando);
       //    }
       //}


       //		public IDbDataAdapter GetDataAdapter(DBType a_connType,string a_connString, string a_sql)
       //		{
       //			IDbDataAdapter idbAdapter=null;
       //			switch(a_connType) 
       //			{
       //				case DBType.OleDb: // OleDb Data Provider
       //					idbAdapter = new OleDbDataAdapter(a_sql, a_connString);
       //					break;
       //				case DBType.SqlServer: // Sql Data Provider
       //					idbAdapter = new SqlDataAdapter(a_sql, a_connString); 
       //					break;
       //				case DBType.Odbc: // ODBC Data Provider
       //					//idbAdapter = new OdbcDataAdapter(sql, connString); 
       //					break;
       //					// case 3: // Add your custom data provider
       //				default:
       //					break;
       //			}
       //			return idbAdapter;
       //		}
       //
       //		public IDbDataAdapter GetDataAdapter(string a_connString, string a_sql)
       //		{
       //			IDbDataAdapter idbAdapter=null;
       //			switch(this.i_DBType) 
       //			{
       //				case DBType.OleDb: // OleDb Data Provider
       //					idbAdapter = new OleDbDataAdapter(a_sql, a_connString);
       //					break;
       //				case DBType.SqlServer: // Sql Data Provider
       //					idbAdapter = new SqlDataAdapter(a_sql, a_connString); 
       //					break;
       //				case DBType.Odbc: // ODBC Data Provider
       //					//idbAdapter = new OdbcDataAdapter(a_sql, a_connString); 
       //					break;
       //					// case 3: // Add your custom data provider
       //				default:
       //					break;
       //			}
       //			return idbAdapter;
       //		}      

   }
}
