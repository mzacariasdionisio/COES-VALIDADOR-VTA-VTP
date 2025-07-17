// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbGrupousuarioRepository
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Metadata.Helper;
using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Metadata.Data
{
  public class WbGrupousuarioRepository : RepositoryBase
  {
    private WbGrupousuarioHelper helper = new WbGrupousuarioHelper();

    public WbGrupousuarioRepository(string strConn)
      : base(strConn)
    {
    }

    public void Save(WbGrupousuarioDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercode, DbType.Int32, (object) entity.Usercode);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) entity.Grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Update(WbGrupousuarioDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercode, DbType.Int32, (object) entity.Usercode);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) entity.Grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int grupocodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbGrupousuarioDTO GetById(int usercode, int grupocodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercode, DbType.Int32, (object) usercode);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) grupocodi);
      WbGrupousuarioDTO wbGrupousuarioDto = (WbGrupousuarioDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbGrupousuarioDto = this.helper.Create(dr);
      }
      return wbGrupousuarioDto;
    }

    public System.Collections.Generic.List<WbGrupousuarioDTO> List()
    {
      System.Collections.Generic.List<WbGrupousuarioDTO> wbGrupousuarioDtoList = new System.Collections.Generic.List<WbGrupousuarioDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbGrupousuarioDtoList.Add(this.helper.Create(dr));
      }
      return wbGrupousuarioDtoList;
    }

    public System.Collections.Generic.List<WbGrupousuarioDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbGrupousuarioDTO> wbGrupousuarioDtoList = new System.Collections.Generic.List<WbGrupousuarioDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbGrupousuarioDtoList.Add(this.helper.Create(dr));
      }
      return wbGrupousuarioDtoList;
    }
  }
}
