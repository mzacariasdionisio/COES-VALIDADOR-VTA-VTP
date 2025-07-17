// Decompiled with JetBrains decompiler
// Type: DataAccessLayer.OracleCBaseX
// Assembly: DataAccessLayerx, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null
// MVID: 2126538F-FA75-4966-A163-9657B4D8E1BC
// Assembly location: C:\d\DataAccessLayerx.dll

using Basex;
using System;
using System.Data;

namespace DataAccessLayer
{
  public class OracleCBaseX : Base
  {
    public OracleDataAccessX ln_conex_ora;
    private string ls_dns_db;

    public OracleCBaseX()
    {
      this.ls_dns_db = "";
      this.ln_conex_ora = (OracleDataAccessX) null;
    }

    public OracleCBaseX(OracleDataAccessX an_conex) => this.ln_conex_ora = an_conex;

    public OracleCBaseX(string as_dns_db)
    {
      this.ls_dns_db = as_dns_db;
      this.ln_conex_ora = (OracleDataAccessX) null;
    }

    public int nf_open_source()
    {
      try
      {
        if (this.ln_conex_ora != null)
          return this.ln_conex_ora.Connection.State == ConnectionState.Closed ? (this.ln_conex_ora.Open() ? 0 : -1) : (this.ln_conex_ora.Connection.State == ConnectionState.Broken && !this.ln_conex_ora.Open() ? -1 : 0);
        this.ln_conex_ora = new OracleDataAccessX();
        this.ln_conex_ora.CreateConnection(this.ls_dns_db);
        return this.ln_conex_ora.Open() ? 0 : -1;
      }
      catch (Exception ex)
      {
        this.MsgError = ex.Message;
        this.CodigoError = -1;
        return -1;
      }
    }

    public void nf_close_source() => this.ln_conex_ora.Close();

    public DataTable nf_get_tabla(DataSet ads_data) => ads_data != null && ads_data.Tables.Count > 0 ? ads_data.Tables[0] : (DataTable) null;
  }
}
