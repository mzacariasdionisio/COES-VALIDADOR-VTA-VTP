// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbColumnitemRepository
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
  public class WbColumnitemRepository : RepositoryBase
  {
    private WbColumnitemHelper helper = new WbColumnitemHelper();

    public WbColumnitemRepository(string strConn)
      : base(strConn)
    {
    }

    public void Save(WbColumnitemDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Itemcodi, DbType.Int32, (object) entity.Itemcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Itemvalue, DbType.String, (object) entity.Itemvalue);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Update(WbColumnitemDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Itemcodi, DbType.Int32, (object) entity.Itemcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Itemvalue, DbType.String, (object) entity.Itemvalue);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbColumnitemDTO GetById(int itemcodi, int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Itemcodi, DbType.Int32, (object) itemcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      WbColumnitemDTO wbColumnitemDto = (WbColumnitemDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbColumnitemDto = this.helper.Create(dr);
      }
      return wbColumnitemDto;
    }

    public System.Collections.Generic.List<WbColumnitemDTO> List()
    {
      System.Collections.Generic.List<WbColumnitemDTO> wbColumnitemDtoList = new System.Collections.Generic.List<WbColumnitemDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbColumnitemDtoList.Add(this.helper.Create(dr));
      }
      return wbColumnitemDtoList;
    }

    public System.Collections.Generic.List<WbColumnitemDTO> GetByCriteria(
      int idColumna)
    {
      System.Collections.Generic.List<WbColumnitemDTO> wbColumnitemDtoList = new System.Collections.Generic.List<WbColumnitemDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) idColumna);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
          wbColumnitemDtoList.Add(this.helper.Create(dr));
      }
      return wbColumnitemDtoList;
    }
  }
}
