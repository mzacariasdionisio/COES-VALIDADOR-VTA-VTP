// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.CumplimientoPorcEmp
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class CumplimientoPorcEmp
  {
    private string ls_empresa;
    private DateTime ldt_fini;
    private DateTime ldt_ffin;
    private double ld_val_sem;
    private double ld_val_men;
    private double ld_val_anual;

    [DataMember]
    public string Empresa
    {
      get => this.ls_empresa;
      set => this.ls_empresa = value;
    }

    [DataMember]
    public DateTime FechaInicial
    {
      get => this.ldt_fini;
      set => this.ldt_fini = value;
    }

    [DataMember]
    public DateTime FechaFinal
    {
      get => this.ldt_ffin;
      set => this.ldt_ffin = value;
    }

    [DataMember]
    public double ValorSem
    {
      get => this.ld_val_sem;
      set => this.ld_val_sem = value;
    }

    [DataMember]
    public double ValorMen
    {
      get => this.ld_val_men;
      set => this.ld_val_men = value;
    }

    [DataMember]
    public double ValorAnual
    {
      get => this.ld_val_anual;
      set => this.ld_val_anual = value;
    }
  }
}
