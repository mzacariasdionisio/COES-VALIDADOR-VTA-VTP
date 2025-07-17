// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbColumntypeHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbColumntypeHelper : HelperBase
  {
    public string Typecodi = "TYPECODI";
    public string Typename = "TYPENAME";
    public string Typeunique = "TYPEUNIQUE";
    public string Typemaxcount = "TYPEMAXCOUNT";

    public WbColumntypeHelper()
      : base(Consultas.WbColumntypeSql)
    {
    }

    public WbColumntypeDTO Create(IDataReader dr)
    {
      WbColumntypeDTO wbColumntypeDto = new WbColumntypeDTO();
      int ordinal1 = dr.GetOrdinal(this.Typecodi);
      if (!dr.IsDBNull(ordinal1))
        wbColumntypeDto.Typecodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Typename);
      if (!dr.IsDBNull(ordinal2))
        wbColumntypeDto.Typename = dr.GetString(ordinal2);
      int ordinal3 = dr.GetOrdinal(this.Typeunique);
      if (!dr.IsDBNull(ordinal3))
        wbColumntypeDto.Typeunique = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Typemaxcount);
      if (!dr.IsDBNull(ordinal4))
        wbColumntypeDto.Typemaxcount = new int?(Convert.ToInt32(dr.GetValue(ordinal4)));
      return wbColumntypeDto;
    }
  }
}
