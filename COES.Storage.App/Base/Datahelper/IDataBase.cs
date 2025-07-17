// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Base.Datahelper.IDataBase
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Base.Datahelper
{
  public interface IDataBase
  {
    DbCommand GetStoredProcCommand(string storedProcedureName);

    DbCommand GetSqlStringCommand(string query);

    void AddInParameter(DbCommand command, string name, DbType dbType, object value);

    IDataReader ExecuteReader(DbCommand command);

    void AddOutParameter(DbCommand command, string name, DbType dbType, int size);

    object GetParameterValue(DbCommand command, string name);

    object ExecuteScalar(DbCommand command);

    int ExecuteNonQuery(DbCommand command);

    void LoadDataSet(DbCommand command, DataSet dataSet, string tableName);
  }
}
