// Decompiled with JetBrains decompiler
// Type: Sicoes.Master.SicMasterSI
// Assembly: SicoesMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A64E3E23-7326-418E-8E1E-0D32A806EDE4
// Assembly location: C:\d\wsSICOES\SicoesMaster.dll

using DataAccessLayer;
using System.Data;

namespace Sicoes.Master
{
  public class SicMasterSI : OracleCBaseX
  {
    public SicMasterSI(ref OracleDataAccessX an_conex_ora)
      : base(an_conex_ora)
    {
    }

    public SicMasterSI(string as_dns)
      : base(as_dns)
    {
    }

    public DataTable nf_get_empresas(int ai_tipo)
    {
      string str = " select t.emprcodi,trim(t.emprnomb) as Empresa,t.tipoemprcodi,trim(t.emprrazsocial) as RazonSocial ";
      string commandText;
      if (ai_tipo < 0)
      {
        commandText = str + "  from si_empresa t  order by 2 asc ";
      }
      else
      {
        commandText = str + "  from si_empresa t where t.tipoemprcodi = :1 order by 2 asc ";
        this.ln_conex_ora.AddParameters((object) ai_tipo, DataDbTypeX.Int);
      }
      return this.nf_get_tabla(this.ln_conex_ora.ExecuteDataSet(commandText, "DS_EMPRESAS"));
    }

    public DataTable nf_get_empresas(string as_tipos)
    {
      string str = " select t.emprcodi,trim(t.emprnomb) as Empresa,t.tipoemprcodi,trim(t.emprrazsocial) as RazonSocial ";
      return this.nf_get_tabla(this.ln_conex_ora.ExecuteDataSet(!(as_tipos == "") ? str + "  from si_empresa t where t.tipoemprcodi in (" + as_tipos + ")  order by 2 asc " : str + "  from si_empresa t  order by 2 asc ", "DS_EMPRESAS"));
    }

    public DataTable nf_get_tipo_emp() => this.nf_get_tabla(this.ln_conex_ora.ExecuteDataSet("select tipoemprcodi,tipoemprdesc,tipoemprabrev from SI_TIPOEMPRESA order by 2 asc", "DS_EMPRESAS"));

    public DataTable nf_get_tipo_emp(string as_filter) => this.nf_get_tabla(this.ln_conex_ora.ExecuteDataSet("select tipoemprcodi,tipoemprdesc,tipoemprabrev from SI_TIPOEMPRESA  where tipoemprcodi not in ( " + as_filter + " ) order by 2 asc", "DS_EMPRESAS"));
  }
}
