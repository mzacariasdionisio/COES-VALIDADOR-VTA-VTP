// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbBlobconfigRepository
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
  public class WbBlobconfigRepository : RepositoryBase
  {
    private WbBlobconfigHelper helper = new WbBlobconfigHelper();

    public WbBlobconfigRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbBlobconfigDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercreate, DbType.String, (object) entity.Usercreate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Datecreate, DbType.DateTime, (object) entity.Datecreate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Userupdate, DbType.String, (object) entity.Userupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Dateupdate, DbType.DateTime, (object) entity.Dateupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configname, DbType.String, (object) entity.Configname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configstate, DbType.String, (object) entity.Configstate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configdefault, DbType.String, (object) entity.Configdefault);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configorder, DbType.String, (object) entity.Configorder);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configespecial, DbType.String, (object) entity.Configespecial);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbBlobconfigDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercreate, DbType.String, (object) entity.Usercreate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Datecreate, DbType.DateTime, (object) entity.Datecreate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Userupdate, DbType.String, (object) entity.Userupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Dateupdate, DbType.DateTime, (object) entity.Dateupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configname, DbType.String, (object) entity.Configname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configstate, DbType.String, (object) entity.Configstate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configdefault, DbType.String, (object) entity.Configdefault);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configorder, DbType.String, (object) entity.Configorder);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configespecial, DbType.String, (object) entity.Configespecial);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) entity.Configcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int configcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) configcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbBlobconfigDTO GetById(int configcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) configcodi);
      WbBlobconfigDTO wbBlobconfigDto = (WbBlobconfigDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBlobconfigDto = this.helper.Create(dr);
      }
      return wbBlobconfigDto;
    }

    public System.Collections.Generic.List<WbBlobconfigDTO> List()
    {
      System.Collections.Generic.List<WbBlobconfigDTO> wbBlobconfigDtoList = new System.Collections.Generic.List<WbBlobconfigDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbBlobconfigDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobconfigDtoList;
    }

    public System.Collections.Generic.List<WbBlobconfigDTO> List(int idFuente)
    {
      System.Collections.Generic.List<WbBlobconfigDTO> wbBlobconfigDtoList = new System.Collections.Generic.List<WbBlobconfigDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlList);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blofuecodi, DbType.Int32, (object) idFuente);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
          wbBlobconfigDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobconfigDtoList;
    }

    public System.Collections.Generic.List<WbBlobconfigDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbBlobconfigDTO> wbBlobconfigDtoList = new System.Collections.Generic.List<WbBlobconfigDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbBlobconfigDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobconfigDtoList;
    }

    public int ValidarNombreNuevo(string configname)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlValidarNombreNuevo);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configname, DbType.String, (object) configname);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public int ValidarNombreEdicion(int idConfig, string configname)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlValidarNombreEdicion);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) idConfig);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configname, DbType.String, (object) configname);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public int ValidarEliminacionLibreria(int idConfig)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlValidarEliminacionLibreria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) idConfig);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }
  }
}
