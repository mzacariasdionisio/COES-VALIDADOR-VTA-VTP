// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.FwUserHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class FwUserHelper : HelperBase
  {
    public string Usercode = "USERCODE";
    public string Userlogin = "USERLOGIN";
    public string Username = "USERNAME";
    public string Grupocodi = "GRUPOCODI";

    public FwUserHelper()
      : base(Consultas.FwUserSql)
    {
    }

    public FwUserDTO Create(IDataReader dr)
    {
      FwUserDTO fwUserDto = new FwUserDTO();
      int ordinal1 = dr.GetOrdinal(this.Usercode);
      if (!dr.IsDBNull(ordinal1))
        fwUserDto.Usercode = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Userlogin);
      if (!dr.IsDBNull(ordinal2))
        fwUserDto.Userlogin = dr.GetString(ordinal2);
      int ordinal3 = dr.GetOrdinal(this.Username);
      if (!dr.IsDBNull(ordinal3))
        fwUserDto.Username = dr.GetString(ordinal3);
      return fwUserDto;
    }
  }
}
