// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbGrupoblobHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbGrupoblobHelper : HelperBase
  {
    public string Grupocodi = "GRUPOCODI";
    public string Blobcodi = "BLOBCODI";

    public WbGrupoblobHelper()
      : base(Consultas.WbGrupoblobSql)
    {
    }

    public WbGrupoblobDTO Create(IDataReader dr)
    {
      WbGrupoblobDTO wbGrupoblobDto = new WbGrupoblobDTO();
      int ordinal1 = dr.GetOrdinal(this.Grupocodi);
      if (!dr.IsDBNull(ordinal1))
        wbGrupoblobDto.Grupocodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Blobcodi);
      if (!dr.IsDBNull(ordinal2))
        wbGrupoblobDto.Blobcodi = Convert.ToInt32(dr.GetValue(ordinal2));
      return wbGrupoblobDto;
    }
  }
}
