// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbBlobcolumnHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbBlobcolumnHelper : HelperBase
  {
    public string Columncodi = "COLUMNCODI";
    public string Typecodi = "TYPECODI";
    public string Columnstate = "COLUMNSTATE";
    public string Columnunique = "COLUMNUNIQUE";
    public string Columnname = "COLUMNNAME";
    public string Columntype = "COLUMNTYPE";
    public string Configcodi = "CONFIGCODI";
    public string Columnorder = "COLUMNORDER";
    public string Columnvisible = "COLUMNVISIBLE";
    public string Columnbusqueda = "COLUMNBUSQUEDA";
    public string Columnalign = "COLUMNALIGN";
    public string Columnshow = "COLUMNSHOW";

    public WbBlobcolumnHelper()
      : base(Consultas.WbBlobcolumnSql)
    {
    }

    public WbBlobcolumnDTO Create(IDataReader dr)
    {
      WbBlobcolumnDTO wbBlobcolumnDto = new WbBlobcolumnDTO();
      int ordinal1 = dr.GetOrdinal(this.Columncodi);
      if (!dr.IsDBNull(ordinal1))
        wbBlobcolumnDto.Columncodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Typecodi);
      if (!dr.IsDBNull(ordinal2))
        wbBlobcolumnDto.Typecodi = new int?(Convert.ToInt32(dr.GetValue(ordinal2)));
      int ordinal3 = dr.GetOrdinal(this.Columnstate);
      if (!dr.IsDBNull(ordinal3))
        wbBlobcolumnDto.Columnstate = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Columnunique);
      if (!dr.IsDBNull(ordinal4))
        wbBlobcolumnDto.Columnunique = dr.GetString(ordinal4);
      int ordinal5 = dr.GetOrdinal(this.Columnname);
      if (!dr.IsDBNull(ordinal5))
        wbBlobcolumnDto.Columnname = dr.GetString(ordinal5);
      int ordinal6 = dr.GetOrdinal(this.Columnalign);
      if (!dr.IsDBNull(ordinal6))
        wbBlobcolumnDto.Columnalign = dr.GetString(ordinal6);
      int ordinal7 = dr.GetOrdinal(this.Columnshow);
      if (!dr.IsDBNull(ordinal7))
        wbBlobcolumnDto.Columnshow = dr.GetString(ordinal7);
      return wbBlobcolumnDto;
    }

    public string SqlObtenerCantidadPorTipo => this.GetSqlXml("ObtenerCantidadPorTipo");

    public string SqlObtenerColumnasPorLibreria => this.GetSqlXml("ObtenerColumnasPorLibreria");

    public string SqlObtenerColumnasVista => this.GetSqlXml("ObtenerColumnasVista");
  }
}
