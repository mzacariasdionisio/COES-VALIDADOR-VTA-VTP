// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbGrupoHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbGrupoHelper : HelperBase
  {
    public string Lastuser = "LASTUSER";
    public string Lastdate = "LASTDATE";
    public string Gruponame = "GRUPONAME";
    public string Grupocodi = "GRUPOCODI";

    public WbGrupoHelper()
      : base(Consultas.WbGrupoSql)
    {
    }

    public WbGrupoDTO Create(IDataReader dr)
    {
      WbGrupoDTO wbGrupoDto = new WbGrupoDTO();
      int ordinal1 = dr.GetOrdinal(this.Lastuser);
      if (!dr.IsDBNull(ordinal1))
        wbGrupoDto.Lastuser = dr.GetString(ordinal1);
      int ordinal2 = dr.GetOrdinal(this.Lastdate);
      if (!dr.IsDBNull(ordinal2))
        wbGrupoDto.Lastdate = new DateTime?(dr.GetDateTime(ordinal2));
      int ordinal3 = dr.GetOrdinal(this.Gruponame);
      if (!dr.IsDBNull(ordinal3))
        wbGrupoDto.Gruponame = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Grupocodi);
      if (!dr.IsDBNull(ordinal4))
        wbGrupoDto.Grupocodi = Convert.ToInt32(dr.GetValue(ordinal4));
      return wbGrupoDto;
    }

    public string SqlDeleteGrupoBlob => this.GetSqlXml("DeleteGrupoBlob");

    public string SqlDeleteGrupoUsuario => this.GetSqlXml("DeleteGrupoUsuario");
  }
}
