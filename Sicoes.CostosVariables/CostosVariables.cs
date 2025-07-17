// Decompiled with JetBrains decompiler
// Type: Sicoes.Operacion.CostosVariables
// Assembly: Sicoes.CostosVariables, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9779532A-A698-4A0A-ACEE-4B44B585FC64
// Assembly location: C:\d\wsSICOES\Sicoes.CostosVariables.dll

using DataAccessLayer;
using System;
using System.Data;

namespace Sicoes.Operacion
{
  public class CostosVariables : OracleCBaseX
  {
    private string ls_sql;

    public CostosVariables(ref OracleDataAccessX an_conex_ora)
      : base(an_conex_ora)
    {
    }

    public CostosVariables(string as_dns)
      : base(as_dns)
    {
    }

    public DataTable OtenerCV_Detalle(int ai_repcodi)
    {
      this.ls_sql = "\r\n            SELECT\r\n               case\r\n                 when GR.GRUPOPADRE > 0 THEN \r\n                   (select TRIM(EA.EMPRNOMB) from si_empresa EA where EA.emprcodi = ( select GX.Emprcodi from pr_grupo GX where GX.Grupocodi = GR.Grupopadre ) )\r\n                 else  ( select TRIM(EB.emprnomb) from si_empresa EB where EB.emprcodi = GR.Emprcodi)\r\n                 end as EMPRESA,       \r\n               GR.GRUPONOMB  AS GRUPO,\r\n               ES.ESCENOMB   AS ESCENARIO,\r\n               CV.PE,\r\n               CV.EFICBTUKWH,\r\n               CV.EFICTERM,\r\n               CV.CCOMB,\r\n               CV.CVC,\r\n               CV.CVNC,\r\n               CV.CVC + CV.CVNC AS CVAR,\r\n               CV.Grupocodi \r\n            FROM PR_CVARIABLES CV, PR_GRUPO GR, PR_ESCENARIO ES\r\n              WHERE CV.GRUPOCODI = GR.GRUPOCODI\r\n               AND CV.ESCECODI = ES.ESCECODI\r\n               AND REPCODI = :1\r\n            ORDER BY 1 asc,2 asc";
      this.ln_conex_ora.AddParameters((object) ai_repcodi, DataDbTypeX.Int);
      return this.ln_conex_ora.ExecuteDataTable(this.ls_sql, "DT_DV_DET");
    }

    public DataTable ObtenerCV_Vigente(DateTime dt_finicial)
    {
      this.ls_sql = "select * from pr_repcv d where d.repfechaem = (select max(c.repfechaem) from pr_repcv c where c.repfecha <= :1  and c.deleted!='S'  and c.reptipo = 'D')";
      this.ln_conex_ora.AddParameters((object) dt_finicial.Date, DataDbTypeX.Date);
      return this.ln_conex_ora.ExecuteDataTable(this.ls_sql, "DT_CV");
    }

    public DataTable ObtenerCV_Evolucion(
      DateTime adt_ffin,
      int ai_grupocodi,
      int ai_ventana)
    {
      DateTime dateTime = adt_ffin.AddDays((double) -ai_ventana);
      this.ls_sql = "\r\n            select d.repfecha, d.repcodi, e.cvc,e.cvnc,e.cvc + e.cvnc as total\r\n              from pr_repcv d, pr_cvariables e\r\n             where d.repfechaem = (select max(c.repfechaem)\r\n                                     from pr_repcv c\r\n                                    where c.repfecha <= d.repfecha\r\n                                      and c.deleted != 'S'\r\n                                      and c.reptipo = 'D')\r\n               and d.repfecha between :1 and :2\r\n               and d.repcodi = e.repcodi\r\n               and d.deleted != 'S'\r\n               and d.reptipo = 'D'\r\n               and e.grupocodi = :3\r\n               order by d.repfecha desc";
      this.ln_conex_ora.AddParameters((object) dateTime, DataDbTypeX.Date);
      this.ln_conex_ora.AddParameters((object) adt_ffin, DataDbTypeX.Date);
      this.ln_conex_ora.AddParameters((object) ai_grupocodi, DataDbTypeX.Int);
      return this.ln_conex_ora.ExecuteDataTable(this.ls_sql, "DT_CV_EVOLUCION");
    }

    public DataTable ObtenerGrupoDesc(int ai_grupo_cod)
    {
      this.ls_sql = "select * from pr_grupo g where g.grupocodi = :1";
      this.ln_conex_ora.AddParameters((object) ai_grupo_cod, DataDbTypeX.Int);
      return this.ln_conex_ora.ExecuteDataTable(this.ls_sql, "DT_GRUPO");
    }
  }
}
