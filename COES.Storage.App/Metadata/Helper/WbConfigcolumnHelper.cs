// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbConfigcolumnHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbConfigcolumnHelper : HelperBase
  {
    public string Configcodi = "CONFIGCODI";
    public string Columncodi = "COLUMNCODI";
    public string Columnorder = "COLUMNORDER";
    public string Columnvisible = "COLUMNVISIBLE";
    public string Columnbusqueda = "COLUMNBUSQUEDA";

    public WbConfigcolumnHelper()
      : base(Consultas.WbConfigcolumnSql)
    {
    }

    public WbConfigcolumnDTO Create(IDataReader dr)
    {
      WbConfigcolumnDTO wbConfigcolumnDto = new WbConfigcolumnDTO();
      int ordinal1 = dr.GetOrdinal(this.Configcodi);
      if (!dr.IsDBNull(ordinal1))
        wbConfigcolumnDto.Configcodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Columncodi);
      if (!dr.IsDBNull(ordinal2))
        wbConfigcolumnDto.Columncodi = Convert.ToInt32(dr.GetValue(ordinal2));
      int ordinal3 = dr.GetOrdinal(this.Columnorder);
      if (!dr.IsDBNull(ordinal3))
        wbConfigcolumnDto.Columnorder = new int?(Convert.ToInt32(dr.GetValue(ordinal3)));
      int ordinal4 = dr.GetOrdinal(this.Columnvisible);
      if (!dr.IsDBNull(ordinal4))
        wbConfigcolumnDto.Columnvisible = dr.GetString(ordinal4);
      int ordinal5 = dr.GetOrdinal(this.Columnbusqueda);
      if (!dr.IsDBNull(ordinal5))
        wbConfigcolumnDto.Columnbusqueda = dr.GetString(ordinal5);
      return wbConfigcolumnDto;
    }

    public string SqlValidarEliminacionColumna => this.GetSqlXml("ValidarEliminacionColumna");

    public string SqlEliminarColumnasLibreria => this.GetSqlXml("EliminarColumnasLibreria");
  }
}
