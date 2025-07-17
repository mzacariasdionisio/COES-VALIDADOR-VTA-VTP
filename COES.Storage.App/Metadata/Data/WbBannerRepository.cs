// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbBannerRepository
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
  public class WbBannerRepository : RepositoryBase
  {
    private WbBannerHelper helper = new WbBannerHelper();

    public WbBannerRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbBannerDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      entity.Bannimage = string.Format(entity.Bannimage, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banncodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banntitulo, DbType.String, (object) entity.Banntitulo);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlink, DbType.String, (object) entity.Bannlink);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannimage, DbType.String, (object) entity.Bannimage);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banndescrip, DbType.String, (object) entity.Banndescrip);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannorden, DbType.Int32, (object) entity.Bannorden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannestado, DbType.String, (object) entity.Bannestado);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlastdate, DbType.DateTime, (object) entity.Bannlastdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlastuser, DbType.String, (object) entity.Bannlastuser);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbBannerDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banntitulo, DbType.String, (object) entity.Banntitulo);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlink, DbType.String, (object) entity.Bannlink);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banndescrip, DbType.String, (object) entity.Banndescrip);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannestado, DbType.String, (object) entity.Bannestado);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlastdate, DbType.DateTime, (object) entity.Bannlastdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannlastuser, DbType.String, (object) entity.Bannlastuser);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banncodi, DbType.Int32, (object) entity.Banncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int banncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banncodi, DbType.Int32, (object) banncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarOrden(WbBannerDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarOrden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bannorden, DbType.Int32, (object) entity.Bannorden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banncodi, DbType.Int32, (object) entity.Banncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbBannerDTO GetById(int banncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Banncodi, DbType.Int32, (object) banncodi);
      WbBannerDTO wbBannerDto = (WbBannerDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBannerDto = this.helper.Create(dr);
      }
      return wbBannerDto;
    }

    public System.Collections.Generic.List<WbBannerDTO> List()
    {
      System.Collections.Generic.List<WbBannerDTO> wbBannerDtoList = new System.Collections.Generic.List<WbBannerDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbBannerDtoList.Add(this.helper.Create(dr));
      }
      return wbBannerDtoList;
    }

    public System.Collections.Generic.List<WbBannerDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbBannerDTO> wbBannerDtoList = new System.Collections.Generic.List<WbBannerDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbBannerDtoList.Add(this.helper.Create(dr));
      }
      return wbBannerDtoList;
    }
  }
}
