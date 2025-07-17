// Decompiled with JetBrains decompiler
// Type: DataAccessLayer.OracleDataAccessX
// Assembly: DataAccessLayerx, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null
// MVID: 2126538F-FA75-4966-A163-9657B4D8E1BC
// Assembly location: C:\d\DataAccessLayerx.dll

using Basex;
using Oracle.DataAccess.Client;
using System;
using System.Collections;
using System.Data;

namespace DataAccessLayer
{
  public class OracleDataAccessX : Base, IDataAccessX
  {
    private OracleConnection ora_Connection;
    private OracleDataReader ora_dataReader;
    private OracleCommand ora_Command;
    private DataProvider2 ora_provider;
    private OracleTransaction ora_Transaction = (OracleTransaction) null;
    private OracleParameter[] ora_Parameters = (OracleParameter[]) null;
    private ArrayList ora_array_Param = (ArrayList) null;
    private string strConnection;

    public DataProvider2 ProviderType
    {
      get => this.ora_provider;
      set => this.ora_provider = value;
    }

    public IDbConnection Connection => (IDbConnection) this.ora_Connection;

    public IDataReader DataReader
    {
      get => (IDataReader) this.ora_dataReader;
      set => this.ora_dataReader = (OracleDataReader) value;
    }

    public string ConnectionString => this.strConnection;

    public IDbCommand Command => (IDbCommand) this.ora_Command;

    public IDbTransaction Transaction => (IDbTransaction) this.ora_Transaction;

    public IDbDataParameter[] Parameters => (IDbDataParameter[]) this.ora_Parameters;

    public OracleDataAccessX()
    {
      this.ora_provider = DataProvider2.Oracle;
      this.ora_array_Param = new ArrayList();
      this.strConnection = "";
    }

    ~OracleDataAccessX() => this.Dispose();

    public void CreateConnection(string CadenaConexion) => this.SetCadenaConnection(CadenaConexion);

    public void SetCadenaConnection(string as_cad_conexion) => this.strConnection = as_cad_conexion;

    public bool OpenSic()
    {
      this.SetCadenaConnection("User ID=sic;Password=S1C03$2018;Data Source=SICCOESR;");
      return this.Open();
    }

    public bool OpenScada()
    {
      this.SetCadenaConnection("User ID=trcoes;Password=74123;Data Source=SICOES11G;");
      return this.Open();
    }

    public bool Open()
    {
      try
      {
        if (this.ora_Connection != null)
        {
          if (this.ora_Connection.State == ConnectionState.Closed)
          {
            this.ora_Connection.Open();
            return true;
          }
          if (this.ora_Connection.State != ConnectionState.Broken)
            return false;
          this.ora_Connection.Open();
          return true;
        }
        this.ora_Connection = new OracleConnection(this.strConnection);
        this.ora_Connection.Open();
        return true;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(" Open: Error ora_Connection: " + ex.Message + "," + this.getultmsgerr());
        return false;
      }
    }

    public void Close()
    {
      try
      {
        if (this.ora_Connection.State == ConnectionState.Closed)
          return;
        this.ora_Connection.Close();
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
      }
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this.Close();
      this.ora_Command = (OracleCommand) null;
      this.ora_Transaction = (OracleTransaction) null;
      this.ora_Connection = (OracleConnection) null;
    }

    public void BeginTransaction()
    {
      if (this.ora_Transaction != null)
        return;
      this.ora_Transaction = this.ora_Connection.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    public void CommitTransaction()
    {
      if (this.ora_Transaction != null)
        this.ora_Transaction.Commit();
      this.ora_Transaction = (OracleTransaction) null;
    }

    public void RollbackTransaction()
    {
      if (this.ora_Transaction != null)
        this.ora_Transaction.Rollback();
      this.ora_Transaction = (OracleTransaction) null;
    }

    public void CreateParameters(int paramsCount) => this.ora_Parameters = this.GetNewParameters(paramsCount);

    public void CreateParameters(object[] param) => this.ora_Parameters = (OracleParameter[]) param;

    public void AddParameters(int index, string paramName, object objValue, object objDbType)
    {
      if (index >= this.ora_Parameters.Length)
        return;
      this.ora_Parameters[index].ParameterName = paramName;
      this.ora_Parameters[index].Value = objValue;
      this.ora_Parameters[index].OracleDbType = (OracleDbType) objDbType;
    }

    public void AddParameters(int index, object objValue, object objDbType)
    {
      if (index >= this.ora_Parameters.Length)
        return;
      this.ora_Parameters[index].ParameterName = "p" + index.ToString();
      this.ora_Parameters[index].Value = objValue;
      this.ora_Parameters[index].OracleDbType = (OracleDbType) objDbType;
    }

    public void AddParameters(object objValue, DataDbTypeX an_DbType)
    {
      OracleParameter oracleParameter = new OracleParameter();
      oracleParameter.ParameterName = "p" + this.ora_array_Param.Count.ToString();
      oracleParameter.Value = objValue;
      oracleParameter.OracleDbType = this.ConvertDbType(an_DbType);
      this.ora_array_Param.Add((object) oracleParameter);
    }

    public void ClearParameters()
    {
      if (this.ora_Parameters != null)
        this.ora_Parameters = (OracleParameter[]) null;
      if (this.ora_array_Param.Count <= 0)
        return;
      this.ora_array_Param.Clear();
    }

    public IDataReader ExecuteReader(CommandType commandType, string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      this.DataReader = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, commandType, commandText, this.ora_array_Param) >= 0 ? (IDataReader) command.ExecuteReader() : (IDataReader) null;
      command.Parameters.Clear();
      command.Dispose();
      return this.DataReader;
    }

    public IDataReader ExecuteReader(string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      this.DataReader = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, CommandType.Text, commandText, this.ora_array_Param) >= 0 ? (IDataReader) command.ExecuteReader() : (IDataReader) null;
      command.Parameters.Clear();
      command.Dispose();
      return this.DataReader;
    }

    public void CloseReader()
    {
      if (this.DataReader == null)
        return;
      this.DataReader.Close();
    }

    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      try
      {
        int num = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, commandType, commandText, this.ora_array_Param) >= 0 ? command.ExecuteNonQuery() : -1;
        command.Parameters.Clear();
        command.Dispose();
        return num;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
        command.Parameters.Clear();
        command.Dispose();
        return -1;
      }
    }

    public int ExecuteNonQuery(string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      try
      {
        int num = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, CommandType.Text, commandText, this.ora_array_Param) >= 0 ? command.ExecuteNonQuery() : -1;
        command.Parameters.Clear();
        command.Dispose();
        return num;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
        command.Parameters.Clear();
        command.Dispose();
        return -1;
      }
    }

    public object ExecuteScalar(CommandType commandType, string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      object obj = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, commandType, commandText, this.ora_array_Param) >= 0 ? command.ExecuteScalar() : (object) null;
      command.Parameters.Clear();
      command.Dispose();
      return obj;
    }

    public object ExecuteScalar(string commandText)
    {
      OracleCommand command = (OracleCommand) null;
      object obj = this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, CommandType.Text, commandText, this.ora_array_Param) >= 0 ? command.ExecuteScalar() : (object) null;
      command.Parameters.Clear();
      command.Dispose();
      return obj;
    }

    public DataSet ExecuteDataSet(CommandType commandType, string commandText, string NombDataSet)
    {
      OracleCommand command = (OracleCommand) null;
      try
      {
        DataSet dataSet;
        if (this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, commandType, commandText, this.ora_array_Param) < 0)
        {
          dataSet = (DataSet) null;
        }
        else
        {
          OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(command);
          dataSet = new DataSet(NombDataSet);
          try
          {
            oracleDataAdapter.Fill(dataSet, NombDataSet);
          }
          catch (Exception ex)
          {
            this.setultmsgerr(ex.Message);
          }
          oracleDataAdapter.Dispose();
        }
        command.Parameters.Clear();
        command.Dispose();
        return dataSet;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
        return (DataSet) null;
      }
    }

    public DataSet ExecuteDataSet(string commandText, string NombDataSet)
    {
      OracleCommand command = (OracleCommand) null;
      try
      {
        DataSet dataSet;
        if (this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, CommandType.Text, commandText, this.ora_array_Param) < 0)
        {
          dataSet = (DataSet) null;
        }
        else
        {
          OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(command);
          dataSet = new DataSet(NombDataSet);
          try
          {
            oracleDataAdapter.Fill(dataSet, NombDataSet);
          }
          catch (Exception ex)
          {
            this.setultmsgerr(ex.Message);
          }
          oracleDataAdapter.Dispose();
        }
        command.Parameters.Clear();
        command.Dispose();
        return dataSet;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
        return (DataSet) null;
      }
    }

    public DataTable ExecuteDataTable(string commandText, string NombDataTable)
    {
      OracleCommand command = (OracleCommand) null;
      try
      {
        DataTable dataTable;
        if (this.PrepareCommand(ref command, (OracleConnection) this.Connection, (OracleTransaction) this.Transaction, CommandType.Text, commandText, this.ora_array_Param) < 0)
        {
          dataTable = (DataTable) null;
        }
        else
        {
          OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(command);
          dataTable = new DataTable(NombDataTable);
          try
          {
            oracleDataAdapter.Fill(dataTable);
          }
          catch (Exception ex)
          {
            this.setultmsgerr(ex.Message);
          }
          oracleDataAdapter.Dispose();
        }
        command.Parameters.Clear();
        command.Dispose();
        return dataTable;
      }
      catch (Exception ex)
      {
        this.setultmsgerr(ex.Message);
        return (DataTable) null;
      }
    }

    private OracleParameter[] GetNewParameters(int paramsCount)
    {
      OracleParameter[] newParameters = new OracleParameter[paramsCount];
      for (int index = 0; index < paramsCount; ++index)
        newParameters[index] = new OracleParameter();
      return newParameters;
    }

    private int PrepareCommand(
      ref OracleCommand command,
      OracleConnection connection,
      OracleTransaction transaction,
      CommandType commandType,
      string commandText,
      OracleParameter[] commandParameters)
    {
      command = new OracleCommand();
      command.Connection = connection;
      command.CommandText = commandText;
      command.CommandType = commandType;
      int num = 0;
      if (commandParameters != null)
        num = this.AttachParameters(command, commandParameters);
      if (commandParameters != null && commandParameters.Length > 0)
        commandParameters = (OracleParameter[]) null;
      return num;
    }

    private int PrepareCommand(
      ref OracleCommand command,
      OracleConnection connection,
      OracleTransaction transaction,
      CommandType commandType,
      string commandText,
      ArrayList commandParameters)
    {
      command = new OracleCommand();
      command.Connection = connection;
      command.CommandText = commandText;
      command.CommandType = commandType;
      int num = 0;
      if (commandParameters != null)
        num = this.AttachParameters(command, commandParameters);
      if (commandParameters.Count > 0)
        commandParameters.Clear();
      return num;
    }

    private int AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
    {
      int num = 0;
      foreach (OracleParameter commandParameter in commandParameters)
      {
        num = this.ValidaParametroString(commandParameter);
        if (num >= 0)
        {
          if (commandParameter.Direction == ParameterDirection.InputOutput && commandParameter.Value == null)
            commandParameter.Value = (object) DBNull.Value;
          command.Parameters.Add(commandParameter);
        }
        else
        {
          num = -1;
          break;
        }
      }
      return num;
    }

    private int AttachParameters(OracleCommand command, ArrayList commandParameters)
    {
      int num = 0;
      foreach (OracleParameter commandParameter in commandParameters)
      {
        num = this.ValidaParametroString(commandParameter);
        if (num >= 0)
        {
          if (commandParameter.Direction == ParameterDirection.InputOutput && commandParameter.Value == null)
            commandParameter.Value = (object) DBNull.Value;
          command.Parameters.Add(commandParameter);
        }
        else
        {
          num = -1;
          break;
        }
      }
      return num;
    }

    private OracleDbType ConvertDbType(DataDbTypeX an_DbTypeX)
    {
      switch (an_DbTypeX)
      {
        case DataDbTypeX.Char:
          return OracleDbType.Char;
        case DataDbTypeX.Decimal:
          return OracleDbType.Decimal;
        case DataDbTypeX.Double:
          return OracleDbType.Double;
        case DataDbTypeX.Date:
          return OracleDbType.Date;
        case DataDbTypeX.Int:
          return OracleDbType.Int32;
        case DataDbTypeX.Int32:
          return OracleDbType.Int32;
        case DataDbTypeX.String:
          return OracleDbType.Varchar2;
        default:
          return OracleDbType.Varchar2;
      }
    }

    public DataTable nf_get_tabla(DataSet ads_data) => ads_data != null && ads_data.Tables.Count > 0 ? ads_data.Tables[0] : (DataTable) null;

    private int ValidaParametroString(OracleParameter ao_parametro)
    {
      try
      {
        switch (ao_parametro.OracleDbType)
        {
          case OracleDbType.Char:
          case OracleDbType.Clob:
          case OracleDbType.NClob:
          case OracleDbType.NChar:
          case OracleDbType.NVarchar2:
          case OracleDbType.Varchar2:
          case OracleDbType.XmlType:
            return this.AdvertenciaSQLInjection(ao_parametro.ToString()) ? -1 : 0;
          default:
            return 0;
        }
      }
      catch (Exception ex)
      {
        return 1;
      }
    }

    private bool AdvertenciaSQLInjection(string as_parametro)
    {
      string upper = as_parametro.ToUpper();
      return upper.Contains("DELETE") || upper.Contains("SELECT") || upper.Contains("INSERT") || upper.Contains("UPDATE");
    }
  }
}
