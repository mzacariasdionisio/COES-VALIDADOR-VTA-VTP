// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Entidad.WbGrupoDTO
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using System;
using System.Collections.Generic;

namespace COES.Storage.App.Metadata.Entidad
{
  public class WbGrupoDTO
  {
    public string Lastuser { get; set; }

    public DateTime? Lastdate { get; set; }

    public string Gruponame { get; set; }

    public int Grupocodi { get; set; }

    public List<FwUserDTO> ListaUsuarios { get; set; }
  }
}
