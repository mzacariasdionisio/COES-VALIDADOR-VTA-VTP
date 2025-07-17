// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Base.Datahelper.DbProvider
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Base.Datahelper
{
  public class DbProvider : IDataBase
  {
    private readonly Database dataBase;

    public DbProvider(string dbName) => this.dataBase = DatabaseFactory.CreateDatabase(dbName);

    public DbCommand GetStoredProcCommand(string storedProcedureName) => this.dataBase.GetStoredProcCommand(storedProcedureName);

    public DbCommand GetSqlStringCommand(string query) => this.dataBase.GetSqlStringCommand(query);

    public void AddInParameter(DbCommand command, string name, DbType dbType, object value) => this.dataBase.AddInParameter(command, name, dbType, value);

    public void AddOutParameter(DbCommand command, string name, DbType dbType, int size) => this.dataBase.AddOutParameter(command, name, dbType, size);

    public IDataReader ExecuteReader(DbCommand command) => this.dataBase.ExecuteReader(command);

    public object GetParameterValue(DbCommand command, string name) => this.dataBase.GetParameterValue(command, name);

    public object ExecuteScalar(DbCommand command) => this.dataBase.ExecuteScalar(command);

    public int ExecuteNonQuery(DbCommand command) => this.dataBase.ExecuteNonQuery(command);

    public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName) => this.dataBase.LoadDataSet(command, dataSet, tableName);
  }
}
