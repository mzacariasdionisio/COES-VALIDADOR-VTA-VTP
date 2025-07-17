// Decompiled with JetBrains decompiler
// Type: Sicoes.Master.SicMasterFWmng
// Assembly: SicoesMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A64E3E23-7326-418E-8E1E-0D32A806EDE4
// Assembly location: C:\d\wsSICOES\SicoesMaster.dll

using DataAccessLayer;
using System;

namespace Sicoes.Master
{
  internal class SicMasterFWmng
  {
    private OracleDataAccessX ln_conex_ora;

    public SicMasterFWmng(ref OracleDataAccessX an_conex_ora) => this.ln_conex_ora = an_conex_ora;

    public long get_fw_counter(string stNombTabla)
    {
      try
      {
        this.ln_conex_ora.CreateParameters(1);
        this.ln_conex_ora.AddParameters((object) stNombTabla, DataDbTypeX.String);
        long num1 = -1;
        string commandText = "select maxcount from FW_COUNTER where tablename=:1";
        this.ln_conex_ora.BeginTransaction();
        object obj = this.ln_conex_ora.ExecuteScalar(commandText);
        long num2;
        if (obj != null)
        {
          num2 = 1L;
          num1 = (long) Convert.ToInt32(obj) + 1L;
        }
        else
          num2 = -1L;
        long num3;
        if (num2 > 0L)
        {
          this.ln_conex_ora.CreateParameters(1);
          this.ln_conex_ora.AddParameters((object) stNombTabla, DataDbTypeX.String);
          num3 = this.ln_conex_ora.ExecuteNonQuery("update FW_COUNTER set maxcount = maxcount + 1 where tablename=:1") <= 0 ? -1L : 1L;
        }
        else
          num3 = -1L;
        if (num3 == -1L)
        {
          this.ln_conex_ora.RollbackTransaction();
          num1 = num3;
        }
        else
          this.ln_conex_ora.CommitTransaction();
        return num1;
      }
      catch (Exception ex)
      {
        this.ln_conex_ora.RollbackTransaction();
        return -1;
      }
    }
  }
}
