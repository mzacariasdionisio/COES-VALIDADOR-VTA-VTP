// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbBannerHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbBannerHelper : HelperBase
  {
    public string Banncodi = "BANNCODI";
    public string Banntitulo = "BANNTITULO";
    public string Bannlink = "BANNLINK";
    public string Bannimage = "BANNIMAGE";
    public string Banndescrip = "BANNDESCRIP";
    public string Bannorden = "BANNORDEN";
    public string Bannestado = "BANNESTADO";
    public string Bannlastdate = "BANNLASTDATE";
    public string Bannlastuser = "BANNLASTUSER";

    public WbBannerHelper()
      : base(Consultas.WbBannerSql)
    {
    }

    public WbBannerDTO Create(IDataReader dr)
    {
      WbBannerDTO wbBannerDto = new WbBannerDTO();
      int ordinal1 = dr.GetOrdinal(this.Banncodi);
      if (!dr.IsDBNull(ordinal1))
        wbBannerDto.Banncodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Banntitulo);
      if (!dr.IsDBNull(ordinal2))
        wbBannerDto.Banntitulo = dr.GetString(ordinal2);
      int ordinal3 = dr.GetOrdinal(this.Bannlink);
      if (!dr.IsDBNull(ordinal3))
        wbBannerDto.Bannlink = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Bannimage);
      if (!dr.IsDBNull(ordinal4))
        wbBannerDto.Bannimage = dr.GetString(ordinal4);
      int ordinal5 = dr.GetOrdinal(this.Banndescrip);
      if (!dr.IsDBNull(ordinal5))
        wbBannerDto.Banndescrip = dr.GetString(ordinal5);
      int ordinal6 = dr.GetOrdinal(this.Bannorden);
      if (!dr.IsDBNull(ordinal6))
        wbBannerDto.Bannorden = new int?(Convert.ToInt32(dr.GetValue(ordinal6)));
      int ordinal7 = dr.GetOrdinal(this.Bannestado);
      if (!dr.IsDBNull(ordinal7))
        wbBannerDto.Bannestado = dr.GetString(ordinal7);
      int ordinal8 = dr.GetOrdinal(this.Bannlastdate);
      if (!dr.IsDBNull(ordinal8))
        wbBannerDto.Bannlastdate = new DateTime?(dr.GetDateTime(ordinal8));
      int ordinal9 = dr.GetOrdinal(this.Bannlastuser);
      if (!dr.IsDBNull(ordinal9))
        wbBannerDto.Bannlastuser = dr.GetString(ordinal9);
      return wbBannerDto;
    }

    public string SqlActualizarOrden => this.GetSqlXml("ActualizarOrden");
  }
}
