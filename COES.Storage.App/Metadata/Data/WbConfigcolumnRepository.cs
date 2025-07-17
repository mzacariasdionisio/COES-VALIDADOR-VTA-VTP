// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbConfigcolumnRepository
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Metadata.Helper;
using System;
using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Metadata.Data
{
  public class WbConfigcolumnRepository : RepositoryBase
  {
    private WbConfigcolumnHelper helper = new WbConfigcolumnHelper();

    public WbConfigcolumnRepository(string strConn)
      : base(strConn)
    {
    }

    public void Save(WbConfigcolumnDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) entity.Configcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnorder, DbType.Int32, (object) entity.Columnorder);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnvisible, DbType.String, (object) entity.Columnvisible);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnbusqueda, DbType.String, (object) entity.Columnbusqueda);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Update(WbConfigcolumnDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) entity.Configcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnorder, DbType.Int32, (object) entity.Columnorder);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnvisible, DbType.String, (object) entity.Columnvisible);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnbusqueda, DbType.String, (object) entity.Columnbusqueda);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbConfigcolumnDTO GetById(int configcodi, int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) configcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      WbConfigcolumnDTO wbConfigcolumnDto = (WbConfigcolumnDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbConfigcolumnDto = this.helper.Create(dr);
      }
      return wbConfigcolumnDto;
    }

    public System.Collections.Generic.List<WbConfigcolumnDTO> List()
    {
      System.Collections.Generic.List<WbConfigcolumnDTO> wbConfigcolumnDtoList = new System.Collections.Generic.List<WbConfigcolumnDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbConfigcolumnDtoList.Add(this.helper.Create(dr));
      }
      return wbConfigcolumnDtoList;
    }

    public System.Collections.Generic.List<WbConfigcolumnDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbConfigcolumnDTO> wbConfigcolumnDtoList = new System.Collections.Generic.List<WbConfigcolumnDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbConfigcolumnDtoList.Add(this.helper.Create(dr));
      }
      return wbConfigcolumnDtoList;
    }

    public int ValidarEliminacionColumna(int idColumna)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlValidarEliminacionColumna);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) idColumna);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public void EliminarColumnasLibreria(int idConfig)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlEliminarColumnasLibreria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) idConfig);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }
  }
}
