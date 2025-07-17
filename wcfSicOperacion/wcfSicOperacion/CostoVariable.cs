// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.CostoVariable
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using Sicoes.Operacion;
using System;
using System.Data;

namespace wcfSicOperacion
{
  public class CostoVariable : wcfBase, ICostoVariable
  {
    public DataTable ObtenerCostosVariables(DateTime af_rep)
    {
      CostosVariables costosVariables = new CostosVariables(this.ls_dns_sic);
      costosVariables.nf_open_source();
      DataTable dataTable1 = costosVariables.ObtenerCV_Vigente(af_rep);
      int ai_repcodi = -1;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        ai_repcodi = Convert.ToInt32(row["REPCODI"]);
      DataTable dataTable2 = costosVariables.OtenerCV_Detalle(ai_repcodi);
      costosVariables.nf_close_source();
      return dataTable2;
    }

    public DataTable ObtenerHistorico(int ai_grupo_cod)
    {
      int ai_ventana = 365;
      DateTime today = DateTime.Today;
      CostosVariables costosVariables = new CostosVariables(this.ls_dns_sic);
      costosVariables.nf_open_source();
      DataTable dataTable = costosVariables.ObtenerCV_Evolucion(today, ai_grupo_cod, ai_ventana);
      costosVariables.nf_close_source();
      return dataTable;
    }

    public DataSet ObtenerHistoricoDesc(int ai_grupo_cod)
    {
      int ai_ventana = 365;
      DateTime today = DateTime.Today;
      CostosVariables costosVariables = new CostosVariables(this.ls_dns_sic);
      costosVariables.nf_open_source();
      DataTable dataTable1 = costosVariables.ObtenerCV_Evolucion(today, ai_grupo_cod, ai_ventana);
      DataTable dataTable2 = costosVariables.ObtenerGrupoDesc(ai_grupo_cod);
      costosVariables.nf_close_source();
      return new DataSet("DS_CV")
      {
        Tables = {
          dataTable2,
          dataTable1
        }
      };
    }
  }
}
