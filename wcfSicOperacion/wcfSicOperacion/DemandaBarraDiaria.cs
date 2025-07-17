// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.DemandaBarraDiaria
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class DemandaBarraDiaria
  {
    private DateTime ldt_fecha_eje;
    private DateTime ldt_fecha_pron;
    private double ld_DemEjecutada;
    private double ld_DemPronosticada;
    private double ld_DemScada = double.NaN;
    private int li_MayorTolerancia = -1;
    private bool lb_IsNullDemScada = true;

    [DataMember]
    public DateTime Fecha
    {
      get => this.ldt_fecha_eje;
      set => this.ldt_fecha_eje = value;
    }

    [DataMember]
    public DateTime FechaPronostico
    {
      get => this.ldt_fecha_pron;
      set => this.ldt_fecha_pron = value;
    }

    [DataMember]
    public double ValorEjecutado
    {
      get => this.ld_DemEjecutada;
      set => this.ld_DemEjecutada = value;
    }

    [DataMember]
    public double ValorPronostico
    {
      get => this.ld_DemPronosticada;
      set => this.ld_DemPronosticada = value;
    }

    [DataMember]
    public double ValorScada
    {
      get => this.ld_DemScada;
      set => this.ld_DemScada = value;
    }

    [DataMember]
    public int Calidad
    {
      get => this.li_MayorTolerancia;
      set => this.li_MayorTolerancia = value;
    }

    [DataMember]
    public bool ExisteValorScada
    {
      get => this.lb_IsNullDemScada;
      set => this.lb_IsNullDemScada = value;
    }
  }
}
