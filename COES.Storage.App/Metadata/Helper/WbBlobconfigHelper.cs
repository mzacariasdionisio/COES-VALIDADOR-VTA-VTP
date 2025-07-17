// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbBlobconfigHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbBlobconfigHelper : HelperBase
  {
    public string Configcodi = "CONFIGCODI";
    public string Usercreate = "USERCREATE";
    public string Datecreate = "DATECREATE";
    public string Userupdate = "USERUPDATE";
    public string Dateupdate = "DATEUPDATE";
    public string Configname = "CONFIGNAME";
    public string Configstate = "CONFIGSTATE";
    public string Configdefault = "CONFIGDEFAULT";
    public string Configorder = "CONFIGORDER";
    public string Configespecial = "CONFIGESPECIAL";
    public string Columncodi = "COLUMNCODI";
    public string Blofuecodi = "BLOFUECODI";

    public WbBlobconfigHelper()
      : base(Consultas.WbBlobconfigSql)
    {
    }

    public WbBlobconfigDTO Create(IDataReader dr)
    {
      WbBlobconfigDTO wbBlobconfigDto = new WbBlobconfigDTO();
      int ordinal1 = dr.GetOrdinal(this.Configcodi);
      if (!dr.IsDBNull(ordinal1))
        wbBlobconfigDto.Configcodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Usercreate);
      if (!dr.IsDBNull(ordinal2))
        wbBlobconfigDto.Usercreate = dr.GetString(ordinal2);
      int ordinal3 = dr.GetOrdinal(this.Datecreate);
      if (!dr.IsDBNull(ordinal3))
        wbBlobconfigDto.Datecreate = new DateTime?(dr.GetDateTime(ordinal3));
      int ordinal4 = dr.GetOrdinal(this.Userupdate);
      if (!dr.IsDBNull(ordinal4))
        wbBlobconfigDto.Userupdate = dr.GetString(ordinal4);
      int ordinal5 = dr.GetOrdinal(this.Dateupdate);
      if (!dr.IsDBNull(ordinal5))
        wbBlobconfigDto.Dateupdate = new DateTime?(dr.GetDateTime(ordinal5));
      int ordinal6 = dr.GetOrdinal(this.Configname);
      if (!dr.IsDBNull(ordinal6))
        wbBlobconfigDto.Configname = dr.GetString(ordinal6);
      int ordinal7 = dr.GetOrdinal(this.Configstate);
      if (!dr.IsDBNull(ordinal7))
        wbBlobconfigDto.Configstate = dr.GetString(ordinal7);
      int ordinal8 = dr.GetOrdinal(this.Configdefault);
      if (!dr.IsDBNull(ordinal8))
        wbBlobconfigDto.Configdefault = dr.GetString(ordinal8);
      int ordinal9 = dr.GetOrdinal(this.Configorder);
      if (!dr.IsDBNull(ordinal9))
        wbBlobconfigDto.Configorder = dr.GetString(ordinal9);
      int ordinal10 = dr.GetOrdinal(this.Configespecial);
      if (!dr.IsDBNull(ordinal10))
        wbBlobconfigDto.Configespecial = dr.GetString(ordinal10);
      int ordinal11 = dr.GetOrdinal(this.Columncodi);
      if (!dr.IsDBNull(ordinal11))
        wbBlobconfigDto.Columncodi = new int?(Convert.ToInt32(dr.GetValue(ordinal11)));
      return wbBlobconfigDto;
    }

    public string SqlValidarNombreNuevo => this.GetSqlXml("ValidarNombreNuevo");

    public string SqlValidarNombreEdicion => this.GetSqlXml("ValidarNombreEdicion");

    public string SqlValidarEliminacionLibreria => this.GetSqlXml("ValidarEliminacionLibreria");
  }
}
