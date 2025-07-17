// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.FwUserRepository
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
  public class FwUserRepository : RepositoryBase
  {
    private FwUserHelper helper = new FwUserHelper();

    public FwUserRepository(string strConn)
      : base(strConn)
    {
    }

    public FwUserDTO GetById(int userCode)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Usercode, DbType.Int32, (object) userCode);
      FwUserDTO fwUserDto = (FwUserDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          fwUserDto = this.helper.Create(dr);
      }
      return fwUserDto;
    }

    public System.Collections.Generic.List<FwUserDTO> List()
    {
      System.Collections.Generic.List<FwUserDTO> fwUserDtoList = new System.Collections.Generic.List<FwUserDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          fwUserDtoList.Add(this.helper.Create(dr));
      }
      return fwUserDtoList;
    }

    public System.Collections.Generic.List<FwUserDTO> GetByCriteria(int grupoCodi)
    {
      System.Collections.Generic.List<FwUserDTO> fwUserDtoList = new System.Collections.Generic.List<FwUserDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Grupocodi, DbType.Int32, (object) grupoCodi);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
          fwUserDtoList.Add(this.helper.Create(dr));
      }
      return fwUserDtoList;
    }
  }
}
