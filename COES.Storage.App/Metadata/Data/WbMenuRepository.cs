// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbMenuRepository
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
  public class WbMenuRepository : RepositoryBase
  {
    private WbMenuHelper helper = new WbMenuHelper();

    public WbMenuRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbMenuDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menudesc, DbType.String, (object) entity.Menudesc);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menutitle, DbType.String, (object) entity.Menutitle);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuorden, DbType.Int32, (object) entity.Menuorden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucolumn, DbType.Int32, (object) entity.Menucolumn);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menutype, DbType.String, (object) entity.Menutype);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuestado, DbType.String, (object) entity.Menuestado);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuurl, DbType.String, (object) entity.Menuurl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) entity.Padrecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Nronivel, DbType.Int32, (object) entity.Nronivel);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuname, DbType.String, (object) entity.Menuname);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbMenuDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menudesc, DbType.String, (object) entity.Menudesc);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menutitle, DbType.String, (object) entity.Menutitle);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuorden, DbType.Int32, (object) entity.Menuorden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucolumn, DbType.Int32, (object) entity.Menucolumn);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menutype, DbType.String, (object) entity.Menutype);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuestado, DbType.String, (object) entity.Menuestado);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuurl, DbType.String, (object) entity.Menuurl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) entity.Padrecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Nronivel, DbType.Int32, (object) entity.Nronivel);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuname, DbType.String, (object) entity.Menuname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucodi, DbType.Int32, (object) entity.Menucodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int menucodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucodi, DbType.Int32, (object) menucodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbMenuDTO GetById(int menucodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucodi, DbType.Int32, (object) menucodi);
      WbMenuDTO wbMenuDto = (WbMenuDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbMenuDto = this.helper.Create(dr);
      }
      return wbMenuDto;
    }

    public System.Collections.Generic.List<WbMenuDTO> List()
    {
      System.Collections.Generic.List<WbMenuDTO> wbMenuDtoList = new System.Collections.Generic.List<WbMenuDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbMenuDtoList.Add(this.helper.Create(dr));
      }
      return wbMenuDtoList;
    }

    public System.Collections.Generic.List<WbMenuDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbMenuDTO> wbMenuDtoList = new System.Collections.Generic.List<WbMenuDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbMenuDtoList.Add(this.helper.Create(dr));
      }
      return wbMenuDtoList;
    }

    public void ActualizarNodoOpcion(int opcionId, int padreId, int nroOrden)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarNodoOpcion);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) padreId);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menuorden, DbType.Int32, (object) nroOrden);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Menucodi, DbType.Int32, (object) opcionId);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public int ObtenerNroItemPorPadre(int idPadre)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerNroItemPorPadre);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) idPadre);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 1;
    }
  }
}
