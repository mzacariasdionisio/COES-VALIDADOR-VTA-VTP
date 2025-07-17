// Decompiled with JetBrains decompiler
// Type: DataAccessLayer.IDataAccessX
// Assembly: DataAccessLayerx, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null
// MVID: 2126538F-FA75-4966-A163-9657B4D8E1BC
// Assembly location: C:\d\DataAccessLayerx.dll

using System.Data;

namespace DataAccessLayer
{
  internal interface IDataAccessX
  {
    DataProvider2 ProviderType { get; set; }

    string ConnectionString { get; }

    IDbConnection Connection { get; }

    IDbTransaction Transaction { get; }

    IDataReader DataReader { get; }

    IDbCommand Command { get; }

    IDbDataParameter[] Parameters { get; }

    void CreateConnection(string ConnectionString);

    bool Open();

    void BeginTransaction();

    void CommitTransaction();

    void RollbackTransaction();

    void CreateParameters(int paramsCount);

    void CreateParameters(object[] param);

    void AddParameters(int index, string paramName, object objValue, object objDbType);

    void ClearParameters();

    IDataReader ExecuteReader(string commandText);

    IDataReader ExecuteReader(CommandType commandType, string commandText);

    DataSet ExecuteDataSet(string commandText, string DataSetName);

    DataSet ExecuteDataSet(CommandType commandType, string commandText, string DataSetName);

    object ExecuteScalar(string commandText);

    object ExecuteScalar(CommandType commandType, string commandText);

    int ExecuteNonQuery(CommandType commandType, string commandText);

    void CloseReader();

    void Close();

    void Dispose();
  }
}
