// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.CumplimientoDetalle
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class CumplimientoDetalle
  {
    private DateTime ldt_fecha;
    private string ls_desc;

    [DataMember]
    public DateTime Fecha
    {
      get => this.ldt_fecha;
      set => this.ldt_fecha = value;
    }

    [DataMember]
    public string Descripcion
    {
      get => this.ls_desc;
      set => this.ls_desc = value;
    }
  }
}
