// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbGrupousuarioHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbGrupousuarioHelper : HelperBase
  {
    public string Usercode = "USERCODE";
    public string Grupocodi = "GRUPOCODI";

    public WbGrupousuarioHelper()
      : base(Consultas.WbGrupousuarioSql)
    {
    }

    public WbGrupousuarioDTO Create(IDataReader dr)
    {
      WbGrupousuarioDTO wbGrupousuarioDto = new WbGrupousuarioDTO();
      int ordinal1 = dr.GetOrdinal(this.Usercode);
      if (!dr.IsDBNull(ordinal1))
        wbGrupousuarioDto.Usercode = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Grupocodi);
      if (!dr.IsDBNull(ordinal2))
        wbGrupousuarioDto.Grupocodi = Convert.ToInt32(dr.GetValue(ordinal2));
      return wbGrupousuarioDto;
    }
  }
}
