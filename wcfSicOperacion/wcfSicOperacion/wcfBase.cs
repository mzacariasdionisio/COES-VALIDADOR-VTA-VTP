// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.wcfBase
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

namespace wcfSicOperacion
{
  public class wcfBase
  {
    public string ls_dns_sic;
    public string ls_dns_scada;

    public wcfBase()
    {
      this.ls_dns_sic = "User ID=sic;Password=S1C03$2018;Data Source=SICCOESR;";
      this.ls_dns_scada = "User ID=trcoes;Password=74123;Data Source=TRCOESR;";
    }
  }
}
