// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbColumntypeRepository
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Metadata.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Metadata.Data
{
  public class WbColumntypeRepository : RepositoryBase
  {
    private WbColumntypeHelper helper = new WbColumntypeHelper();

    public WbColumntypeRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbColumntypeDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typename, DbType.String, (object) entity.Typename);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typeunique, DbType.String, (object) entity.Typeunique);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typemaxcount, DbType.Int32, (object) entity.Typemaxcount);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbColumntypeDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) entity.Typecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typename, DbType.String, (object) entity.Typename);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typeunique, DbType.String, (object) entity.Typeunique);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typemaxcount, DbType.Int32, (object) entity.Typemaxcount);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int typecodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) typecodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbColumntypeDTO GetById(int typecodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) typecodi);
      WbColumntypeDTO wbColumntypeDto = (WbColumntypeDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbColumntypeDto = this.helper.Create(dr);
      }
      return wbColumntypeDto;
    }

    public System.Collections.Generic.List<WbColumntypeDTO> List()
    {
      System.Collections.Generic.List<WbColumntypeDTO> wbColumntypeDtoList = new System.Collections.Generic.List<WbColumntypeDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbColumntypeDtoList.Add(this.helper.Create(dr));
      }
      return wbColumntypeDtoList;
    }

    public System.Collections.Generic.List<WbColumntypeDTO> GetByCriteria(
      System.Collections.Generic.List<int> ids)
    {
      System.Collections.Generic.List<WbColumntypeDTO> wbColumntypeDtoList = new System.Collections.Generic.List<WbColumntypeDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(string.Format(this.helper.SqlGetByCriteria, (object) string.Join<int>(",", (IEnumerable<int>) ids)))))
      {
        while (dr.Read())
          wbColumntypeDtoList.Add(this.helper.Create(dr));
      }
      return wbColumntypeDtoList;
    }
  }
}
