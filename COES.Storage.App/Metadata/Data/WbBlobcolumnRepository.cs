// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbBlobcolumnRepository
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
  public class WbBlobcolumnRepository : RepositoryBase
  {
    private WbBlobcolumnHelper helper = new WbBlobcolumnHelper();

    public WbBlobcolumnRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbBlobcolumnDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) entity.Typecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnstate, DbType.String, (object) entity.Columnstate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnunique, DbType.String, (object) entity.Columnunique);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnname, DbType.String, (object) entity.Columnname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnalign, DbType.String, (object) entity.Columnalign);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnshow, DbType.String, (object) entity.Columnshow);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbBlobcolumnDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) entity.Typecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnstate, DbType.String, (object) entity.Columnstate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnunique, DbType.String, (object) entity.Columnunique);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnname, DbType.String, (object) entity.Columnname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnalign, DbType.String, (object) entity.Columnalign);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columnshow, DbType.String, (object) entity.Columnshow);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) entity.Columncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbBlobcolumnDTO GetById(int columncodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Columncodi, DbType.Int32, (object) columncodi);
      WbBlobcolumnDTO wbBlobcolumnDto = (WbBlobcolumnDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBlobcolumnDto = this.helper.Create(dr);
      }
      return wbBlobcolumnDto;
    }

    public System.Collections.Generic.List<WbBlobcolumnDTO> List()
    {
      System.Collections.Generic.List<WbBlobcolumnDTO> wbBlobcolumnDtoList = new System.Collections.Generic.List<WbBlobcolumnDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
        {
          WbBlobcolumnDTO wbBlobcolumnDto = this.helper.Create(dr);
          int ordinal = dr.GetOrdinal(this.helper.Columntype);
          if (!dr.IsDBNull(ordinal))
            wbBlobcolumnDto.Columntype = dr.GetString(ordinal);
          wbBlobcolumnDtoList.Add(wbBlobcolumnDto);
        }
      }
      return wbBlobcolumnDtoList;
    }

    public System.Collections.Generic.List<WbBlobcolumnDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbBlobcolumnDTO> wbBlobcolumnDtoList = new System.Collections.Generic.List<WbBlobcolumnDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbBlobcolumnDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobcolumnDtoList;
    }

    public int ObtenerCantidadPorTipo(int typecodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerCantidadPorTipo);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Typecodi, DbType.Int32, (object) typecodi);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public System.Collections.Generic.List<WbBlobcolumnDTO> ObtenerColumnasPorLibreria(
      int idLibreria)
    {
      System.Collections.Generic.List<WbBlobcolumnDTO> wbBlobcolumnDtoList = new System.Collections.Generic.List<WbBlobcolumnDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerColumnasPorLibreria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) idLibreria);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
        {
          WbBlobcolumnDTO wbBlobcolumnDto = this.helper.Create(dr);
          int ordinal1 = dr.GetOrdinal(this.helper.Columnorder);
          if (!dr.IsDBNull(ordinal1))
            wbBlobcolumnDto.Columnorder = Convert.ToInt32(dr.GetValue(ordinal1));
          int ordinal2 = dr.GetOrdinal(this.helper.Columnvisible);
          if (!dr.IsDBNull(ordinal2))
            wbBlobcolumnDto.Columnvisible = dr.GetString(ordinal2);
          int ordinal3 = dr.GetOrdinal(this.helper.Columntype);
          if (!dr.IsDBNull(ordinal3))
            wbBlobcolumnDto.Columntype = dr.GetString(ordinal3);
          int ordinal4 = dr.GetOrdinal(this.helper.Columnbusqueda);
          if (!dr.IsDBNull(ordinal4))
            wbBlobcolumnDto.Columnbusqueda = dr.GetString(ordinal4);
          wbBlobcolumnDtoList.Add(wbBlobcolumnDto);
        }
      }
      return wbBlobcolumnDtoList;
    }

    public System.Collections.Generic.List<WbBlobcolumnDTO> ObtenerColumnasVista(
      int idConfig)
    {
      System.Collections.Generic.List<WbBlobcolumnDTO> wbBlobcolumnDtoList = new System.Collections.Generic.List<WbBlobcolumnDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerColumnasVista);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) idConfig);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
        {
          WbBlobcolumnDTO wbBlobcolumnDto = this.helper.Create(dr);
          int ordinal1 = dr.GetOrdinal(this.helper.Columnorder);
          if (!dr.IsDBNull(ordinal1))
            wbBlobcolumnDto.Columnorder = Convert.ToInt32(dr.GetValue(ordinal1));
          int ordinal2 = dr.GetOrdinal(this.helper.Columnbusqueda);
          if (!dr.IsDBNull(ordinal2))
            wbBlobcolumnDto.Columnbusqueda = dr.GetString(ordinal2);
          wbBlobcolumnDtoList.Add(wbBlobcolumnDto);
        }
      }
      return wbBlobcolumnDtoList;
    }
  }
}
