// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.CumplimientoDiarioxEmp
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class CumplimientoDiarioxEmp
  {
    private int li_correlativo;
    private string ls_empresa;
    private string ls_potencia;
    private string ls_integrante;
    private int li_nenvios_pro30;
    private int li_env_plazo;
    private int li_env_fplazo;
    private int li_nenvio;
    private double ld_porc_cumpli;

    [DataMember(Order = 1)]
    public int Orden
    {
      get => this.li_correlativo;
      set => this.li_correlativo = value;
    }

    [DataMember(Order = 2)]
    public string Empresa
    {
      get => this.ls_empresa;
      set => this.ls_empresa = value;
    }

    [DataMember(Order = 3)]
    public string Potencia
    {
      get => this.ls_potencia;
      set => this.ls_potencia = value;
    }

    [DataMember(Order = 4)]
    public string EsIntegrante
    {
      get => this.ls_integrante;
      set => this.ls_integrante = value;
    }

    [DataMember(Order = 5)]
    public int EnvPr30
    {
      get => this.li_nenvios_pro30;
      set => this.li_nenvios_pro30 = value;
    }

    [DataMember(Order = 6)]
    public int EnvPlazo
    {
      get => this.li_env_plazo;
      set => this.li_env_plazo = value;
    }

    [DataMember(Order = 7)]
    public int EnvFPlazo
    {
      get => this.li_env_fplazo;
      set => this.li_env_fplazo = value;
    }

    [DataMember(Order = 8)]
    public int NoEnviados
    {
      get => this.li_nenvio;
      set => this.li_nenvio = value;
    }

    [DataMember(Order = 9)]
    public double PorCumplimiento
    {
      get => this.ld_porc_cumpli;
      set => this.ld_porc_cumpli = value;
    }
  }
}
