// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbMenuHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbMenuHelper : HelperBase
  {
    public string Menucodi = "MENUCODI";
    public string Menudesc = "MENUDESC";
    public string Menutitle = "MENUTITLE";
    public string Menuorden = "MENUORDEN";
    public string Menucolumn = "MENUCOLUMN";
    public string Menutype = "MENUTYPE";
    public string Menuestado = "MENUESTADO";
    public string Menuurl = "MENUURL";
    public string Padrecodi = "PADRECODI";
    public string Nronivel = "NRONIVEL";
    public string Menuname = "MENUNAME";

    public WbMenuHelper()
      : base(Consultas.WbMenuSql)
    {
    }

    public WbMenuDTO Create(IDataReader dr)
    {
      WbMenuDTO wbMenuDto = new WbMenuDTO();
      int ordinal1 = dr.GetOrdinal(this.Menucodi);
      if (!dr.IsDBNull(ordinal1))
        wbMenuDto.Menucodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Menudesc);
      if (!dr.IsDBNull(ordinal2))
        wbMenuDto.Menudesc = dr.GetString(ordinal2);
      int ordinal3 = dr.GetOrdinal(this.Menutitle);
      if (!dr.IsDBNull(ordinal3))
        wbMenuDto.Menutitle = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Menuorden);
      if (!dr.IsDBNull(ordinal4))
        wbMenuDto.Menuorden = new int?(Convert.ToInt32(dr.GetValue(ordinal4)));
      int ordinal5 = dr.GetOrdinal(this.Menucolumn);
      if (!dr.IsDBNull(ordinal5))
        wbMenuDto.Menucolumn = new int?(Convert.ToInt32(dr.GetValue(ordinal5)));
      int ordinal6 = dr.GetOrdinal(this.Menutype);
      if (!dr.IsDBNull(ordinal6))
        wbMenuDto.Menutype = dr.GetString(ordinal6);
      int ordinal7 = dr.GetOrdinal(this.Menuestado);
      if (!dr.IsDBNull(ordinal7))
        wbMenuDto.Menuestado = dr.GetString(ordinal7);
      int ordinal8 = dr.GetOrdinal(this.Menuurl);
      if (!dr.IsDBNull(ordinal8))
        wbMenuDto.Menuurl = dr.GetString(ordinal8);
      int ordinal9 = dr.GetOrdinal(this.Padrecodi);
      if (!dr.IsDBNull(ordinal9))
        wbMenuDto.Padrecodi = new int?(Convert.ToInt32(dr.GetValue(ordinal9)));
      int ordinal10 = dr.GetOrdinal(this.Nronivel);
      if (!dr.IsDBNull(ordinal10))
        wbMenuDto.Nronivel = new int?(Convert.ToInt32(dr.GetValue(ordinal10)));
      int ordinal11 = dr.GetOrdinal(this.Menuname);
      if (!dr.IsDBNull(ordinal11))
        wbMenuDto.Menuname = dr.GetString(ordinal11);
      return wbMenuDto;
    }

    public string SqlActualizarNodoOpcion => this.GetSqlXml("ActualizarNodoOpcion");

    public string SqlObtenerNroItemPorPadre => this.GetSqlXml("ObtenerNroItemPorPadre");
  }
}
