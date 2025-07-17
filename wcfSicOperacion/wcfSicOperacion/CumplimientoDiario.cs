// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.CumplimientoDiario
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class CumplimientoDiario
  {
    private DateTime ldt_fecha;
    private string ls_env_diario;
    private string ls_env_semanal;

    [DataMember(Order = 1)]
    public DateTime Fecha
    {
      get => this.ldt_fecha;
      set => this.ldt_fecha = value;
    }

    [DataMember(Order = 2)]
    public string Diario
    {
      get => this.ls_env_diario;
      set => this.ls_env_diario = value;
    }

    [DataMember(Order = 3)]
    public string Semanal
    {
      get => this.ls_env_semanal;
      set => this.ls_env_semanal = value;
    }
  }
}
