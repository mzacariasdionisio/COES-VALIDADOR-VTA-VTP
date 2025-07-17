// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Entidad.WbBlobcolumnDTO
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System.Collections.Generic;

namespace COES.Storage.App.Metadata.Entidad
{
  public class WbBlobcolumnDTO
  {
    public int Columncodi { get; set; }

    public int? Typecodi { get; set; }

    public string Columnstate { get; set; }

    public string Columnunique { get; set; }

    public string Columnname { get; set; }

    public string Columntype { get; set; }

    public string Columnshow { get; set; }

    public int Columnorder { get; set; }

    public string Columnvisible { get; set; }

    public string Columnbusqueda { get; set; }

    public string Columnalign { get; set; }

    public List<WbColumnitemDTO> ListaItems { get; set; }

    public string Type { get; set; }

    public string[] Lista { get; set; }

    public string Dateformat { get; set; }

    public string Defaultdate { get; set; }

    public string Format { get; set; }

    public int Width { get; set; }

    public bool ReadOnly { get; set; }
  }
}
