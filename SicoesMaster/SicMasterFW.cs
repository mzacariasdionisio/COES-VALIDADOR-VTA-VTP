// Decompiled with JetBrains decompiler
// Type: Sicoes.Master.SicMasterFW
// Assembly: SicoesMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A64E3E23-7326-418E-8E1E-0D32A806EDE4
// Assembly location: C:\d\wsSICOES\SicoesMaster.dll

using DataAccessLayer;

namespace Sicoes.Master
{
  public class SicMasterFW : OracleCBaseX
  {
    public SicMasterFW(ref OracleDataAccessX an_conex_ora) => this.ln_conex_ora = an_conex_ora;

    public long get_fw_counter(string as_table_name) => new SicMasterFWmng(ref this.ln_conex_ora).get_fw_counter(as_table_name);
  }
}
