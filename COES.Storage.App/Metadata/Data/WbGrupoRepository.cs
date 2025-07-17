// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbGrupoRepository
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
  public class WbGrupoRepository : RepositoryBase
  {
    private WbGrupoHelper helper = new WbGrupoHelper();

    public WbGrupoRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbGrupoDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Gruponame, DbType.String, (object) entity.Gruponame);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Lastuser, DbType.String, (object) entity.Lastuser);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbGrupoDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Gruponame, DbType.String, (object) entity.Gruponame);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Lastuser, DbType.String, (object) entity.Lastuser);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) entity.Grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int grupocodi)
    {
      DbCommand sqlStringCommand1 = this.dbProvider.GetSqlStringCommand(this.helper.SqlDeleteGrupoBlob);
      this.dbProvider.AddInParameter(sqlStringCommand1, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand1);
      DbCommand sqlStringCommand2 = this.dbProvider.GetSqlStringCommand(this.helper.SqlDeleteGrupoUsuario);
      this.dbProvider.AddInParameter(sqlStringCommand2, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand2);
      DbCommand sqlStringCommand3 = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand3, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand3);
    }

    public WbGrupoDTO GetById(int grupocodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      WbGrupoDTO wbGrupoDto = (WbGrupoDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbGrupoDto = this.helper.Create(dr);
      }
      return wbGrupoDto;
    }

    public System.Collections.Generic.List<WbGrupoDTO> List()
    {
      System.Collections.Generic.List<WbGrupoDTO> wbGrupoDtoList = new System.Collections.Generic.List<WbGrupoDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbGrupoDtoList.Add(this.helper.Create(dr));
      }
      return wbGrupoDtoList;
    }

    public System.Collections.Generic.List<WbGrupoDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbGrupoDTO> wbGrupoDtoList = new System.Collections.Generic.List<WbGrupoDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbGrupoDtoList.Add(this.helper.Create(dr));
      }
      return wbGrupoDtoList;
    }
  }
}
