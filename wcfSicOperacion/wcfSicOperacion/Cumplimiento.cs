// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.Cumplimiento
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Runtime.Serialization;

namespace wcfSicOperacion
{
  [DataContract]
  public class Cumplimiento
  {
    private DateTime ldt_ini;
    private DateTime ldt_fin;
    private string ls_nombre_emp;
    private string ls_tipo_emp;
    private int li_num_cumpli;
    private int li_num_nocumpli;
    private int li_emprcodi;

    [DataMember]
    public DateTime Inicio
    {
      get => this.ldt_ini;
      set => this.ldt_ini = value;
    }

    [DataMember]
    public DateTime Fin
    {
      get => this.ldt_fin;
      set => this.ldt_fin = value;
    }

    [DataMember]
    public string Empresa
    {
      get => this.ls_nombre_emp;
      set => this.ls_nombre_emp = value;
    }

    [DataMember]
    public string TipoEmpresa
    {
      get => this.ls_tipo_emp;
      set => this.ls_tipo_emp = value;
    }

    [DataMember]
    public int NCumplimiento
    {
      get => this.li_num_cumpli;
      set => this.li_num_cumpli = value;
    }

    [DataMember]
    public int NNoCumplimiento
    {
      get => this.li_num_nocumpli;
      set => this.li_num_nocumpli = value;
    }

    [DataMember]
    public int Emprcodi
    {
      get => this.li_emprcodi;
      set => this.li_emprcodi = value;
    }
  }
}
