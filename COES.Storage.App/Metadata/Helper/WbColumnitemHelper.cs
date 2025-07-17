// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbColumnitemHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbColumnitemHelper : HelperBase
  {
    public string Itemcodi = "ITEMCODI";
    public string Columncodi = "COLUMNCODI";
    public string Itemvalue = "ITEMVALUE";

    public WbColumnitemHelper()
      : base(Consultas.WbColumnitemSql)
    {
    }

    public WbColumnitemDTO Create(IDataReader dr)
    {
      WbColumnitemDTO wbColumnitemDto = new WbColumnitemDTO();
      int ordinal1 = dr.GetOrdinal(this.Itemcodi);
      if (!dr.IsDBNull(ordinal1))
        wbColumnitemDto.Itemcodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Columncodi);
      if (!dr.IsDBNull(ordinal2))
        wbColumnitemDto.Columncodi = Convert.ToInt32(dr.GetValue(ordinal2));
      int ordinal3 = dr.GetOrdinal(this.Itemvalue);
      if (!dr.IsDBNull(ordinal3))
        wbColumnitemDto.Itemvalue = dr.GetString(ordinal3);
      return wbColumnitemDto;
    }
  }
}
