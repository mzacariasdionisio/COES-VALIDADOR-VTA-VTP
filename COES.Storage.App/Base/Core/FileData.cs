// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Base.Core.FileData
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System;
using System.Collections.Generic;

namespace COES.Storage.App.Base.Core
{
  public class FileData
  {
    public string FileUrl { get; set; }

    public string FileName { get; set; }

    public string FileType { get; set; }

    public string FileSize { get; set; }

    public string Icono { get; set; }

    public string Extension { get; set; }

    public string TipoLibreria { get; set; }

    public string LastUser { get; set; }

    public DateTime LastDate { get; set; }

    public string TipoFolder { get; set; }

    public List<FileMetadato> Metadatos { get; set; }

    public int Blobcodi { get; set; }

    public string FirstUser { get; set; }

    public DateTime FirstDate { get; set; }

    public object Columnorder { get; set; }

    public string IndMain { get; set; }

    public int Padrecodi { get; set; }

    public string FileHide { get; set; }

    public string Metadata { get; set; }

    public int? TreePadre { get; set; }
  }
}
