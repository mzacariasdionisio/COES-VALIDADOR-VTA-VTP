// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.Demanda
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using DataAccessLayer;
using Scada.Operacion;
using Sicoes.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace wcfSicOperacion
{
  public class Demanda : IDemanda
  {
    private string ls_dns_sic;
    private string ls_dns_scada;
    private string ls_version;

    public Demanda()
    {
      this.ls_dns_scada = "User ID=trcoes;Password=74123;Data Source=TRCOESR;";
      this.ls_dns_sic = "User ID=sic;Password=S1C03$2018;Data Source=SICCOESR;";
      this.ls_version = "1.0.0";
    }

    public DataTable EmpresasRepxUsuario(string[] as_arr_codigo_emp, string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable empresasRepxUsuario = demandaBarra.nf_get_EmpresasRepxUsuario(as_arr_codigo_emp);
      demandaBarra.nf_close_source();
      return empresasRepxUsuario;
    }

    public DataTable ListarEnviosxEmpresas(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      int ai_tipempr,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable listadoEnviosxEmpresas = demandaBarra.nf_get_ListadoEnviosxEmpresas(adt_f_ini, adt_f_fin, ai_emprcodi, ai_tipempr);
      demandaBarra.nf_close_source();
      return listadoEnviosxEmpresas;
    }

    public DataTable ListarEnviosxEmpresaHistorico(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_tipo_empr,
      int ai_tipo,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable empresasHistorico = demandaBarra.nf_get_ListadoEnviosxEmpresasHistorico(adt_f_ini, adt_f_fin, ai_tipo_empr, ai_tipo);
      demandaBarra.nf_close_source();
      return empresasHistorico;
    }

    public DataTable PuntoMedicionBarraxEmp(int ai_emprcodi, string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable medicionBarraxEmp = demandaBarra.nf_get_PtoMedicionBarraxEmp(ai_emprcodi);
      demandaBarra.nf_close_source();
      return medicionBarraxEmp;
    }

    public DataTable PuntoMedicionBarraDescxEmp(int ai_emprcodi, string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable medicionDescxEmp = demandaBarra.nf_get_PtoMedicionDescxEmp(ai_emprcodi);
      demandaBarra.nf_close_source();
      return medicionDescxEmp;
    }

    public DataTable PuntoMedicionBarraDesc(int ai_emprcodi, string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable ptoMedicionDesc = demandaBarra.nf_get_PtoMedicionDesc(ai_emprcodi);
      demandaBarra.nf_close_source();
      return ptoMedicionDesc;
    }

    public DataTable ObtenerDemandaFuente(
      int ai_etacodi,
      DateTime adt_fecha_rep,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable demandaFuente = demandaBarra.nf_get_DemandaFuente(ai_etacodi, adt_fecha_rep, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return demandaFuente;
    }

    public DataTable ObtenerDemandaBarraDiario48Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi_real,
      int ai_lectcodi_prog,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteDiario48FHora(adt_fecha_rep, ai_lectcodi_real, ai_lectcodi_prog, ai_tipoinf, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public DataTable ObtenerDemandaBarraDiario96Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi_real,
      int ai_lectcodi_prog,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteDiario96FHora(adt_fecha_rep, ai_lectcodi_real, ai_lectcodi_prog, ai_tipoinf, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public DataTable ObtenerDemandaBarraDiario96Fhora2(
      DateTime adt_fini,
      DateTime adt_ffin,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_etacodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      int num1 = 0;
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteDiario96FHora(adt_fini, adt_ffin, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, ai_etacodi);
      demandaBarra.nf_close_source();
      int num2 = 0;
      if (dataTable != null)
      {
        num2 = dataTable.Columns.Count;
        dataTable.Columns.Add("TOTAL");
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        double num3 = 0.0;
        if (num1 > 0)
        {
          for (int columnIndex = 1; columnIndex < num2; ++columnIndex)
          {
            if (!row.IsNull(columnIndex))
              num3 += Convert.ToDouble(row[columnIndex]);
          }
        }
        row["TOTAL"] = num1 != 0 ? (object) num3 : (object) "";
        ++num1;
      }
      return dataTable;
    }

    public DataTable ObtenerDemandaBarraSemanal48Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteSemanal48FHora(adt_fecha_rep, ai_lectcodi, ai_tipoinf, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public DataTable ObtenerDemandaBarraSemanal96Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteSemanal96FHora(adt_fecha_rep, ai_lectcodi, ai_tipoinf, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public DataTable DemandaBarraReporte48from48T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      int num1 = 0;
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporte48from48T(adt_fi, adt_ff, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, ai_emprecodi);
      demandaBarra.nf_close_source();
      int num2 = 0;
      if (dataTable != null)
      {
        num2 = dataTable.Columns.Count;
        dataTable.Columns.Add("TOTAL");
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        double num3 = 0.0;
        if (num1 > 0)
        {
          for (int columnIndex = 0; columnIndex < num2; ++columnIndex)
          {
            if (!row.IsNull(columnIndex))
              num3 += Convert.ToDouble(row[columnIndex]);
          }
        }
        row["TOTAL"] = num1 != 0 ? (object) num3 : (object) "";
        ++num1;
      }
      return dataTable;
    }

    public DataTable DemandaBarraReporte48from48T2(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      string as_emprecodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporte48from48T(adt_fi, adt_ff, ai_lectcodi, ai_tipoinf, as_emprecodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public DataTable DemandaBarraReporte48from96T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      int num1 = 0;
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporte48from96T(adt_fi, adt_ff, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, ai_emprecodi);
      demandaBarra.nf_close_source();
      int num2 = 0;
      if (dataTable != null)
      {
        num2 = dataTable.Columns.Count;
        dataTable.Columns.Add("TOTAL");
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        double num3 = 0.0;
        if (num1 > 0)
        {
          for (int columnIndex = 0; columnIndex < num2; ++columnIndex)
          {
            if (!row.IsNull(columnIndex))
              num3 += Convert.ToDouble(row[columnIndex]);
          }
        }
        row["TOTAL"] = num1 != 0 ? (object) num3 : (object) "";
        ++num1;
      }
      return dataTable;
    }

    public DataTable DemandaBarraReporte96from96T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      int num1 = 0;
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporte96from96T(adt_fi, adt_ff, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, ai_emprecodi);
      demandaBarra.nf_close_source();
      int num2 = 0;
      if (dataTable != null)
      {
        num2 = dataTable.Columns.Count;
        dataTable.Columns.Add("TOTAL");
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        double num3 = 0.0;
        if (num1 > 0)
        {
          for (int columnIndex = 0; columnIndex < num2; ++columnIndex)
          {
            if (!row.IsNull(columnIndex))
              num3 += Convert.ToDouble(row[columnIndex]);
          }
        }
        row["TOTAL"] = num1 != 0 ? (object) num3 : (object) "";
        ++num1;
      }
      return dataTable;
    }

    public DataTable DemandaBarraDiariaReportada96(
      DateTime adt_fecha_rep,
      int ai_lectcodi_real,
      int ai_lectcodi_prog,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_canalcodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      MedicionScada medicionScada = new MedicionScada(this.ls_dns_scada);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      medicionScada.nf_open_source();
      DateTime adt_fecha1 = adt_fecha_rep.AddDays(-1.0);
      DateTime adt_fecha2 = adt_fecha_rep.AddDays(1.0);
      DataTable dataTable = new DataTable("DT_DATOS_REP");
      try
      {
        DataTable demanda96_1 = demandaBarra.nf_get_Demanda96(adt_fecha1, ai_lectcodi_real, ai_tipoinf, ai_ptomedicodi, true, false);
        DataTable demanda96_2 = demandaBarra.nf_get_Demanda96(adt_fecha2, ai_lectcodi_real, ai_tipoinf, ai_ptomedicodi, true, false);
        DataTable medicionScada96 = medicionScada.nf_get_medicion_scada96(ai_canalcodi, adt_fecha1);
        demandaBarra.nf_close_source();
        medicionScada.nf_close_source();
        dataTable.Columns.Add("01-D. Real" + demanda96_1.Rows[0][0].ToString(), typeof (double));
        dataTable.Columns.Add("02-D. Scada" + medicionScada96.Rows[0][0].ToString(), typeof (double));
        dataTable.Columns.Add("03-D. Pronosticada" + demanda96_2.Rows[0][0].ToString(), typeof (double));
        dataTable.Columns.Add("C05", typeof (int));
        dataTable.Columns.Add("C06", typeof (int));
        for (int columnIndex = 1; columnIndex <= 96; ++columnIndex)
        {
          DemandaBarraDiaria demandaBarraDiaria = new DemandaBarraDiaria();
          DataRow row = dataTable.NewRow();
          demandaBarraDiaria.ValorEjecutado = demanda96_1.Rows.Count <= 0 ? double.NaN : (demanda96_1.Rows[0].IsNull(columnIndex) ? double.NaN : Convert.ToDouble(demanda96_1.Rows[0][columnIndex]));
          demandaBarraDiaria.ValorPronostico = demanda96_2.Rows.Count <= 0 ? double.NaN : (demanda96_2.Rows[0].IsNull(columnIndex) ? double.NaN : Convert.ToDouble(demanda96_2.Rows[0][columnIndex]));
          if (medicionScada96.Rows.Count > 0)
          {
            if (!medicionScada96.Rows[0].IsNull(columnIndex))
            {
              demandaBarraDiaria.ValorScada = Convert.ToDouble(medicionScada96.Rows[0][columnIndex]);
              demandaBarraDiaria.ExisteValorScada = true;
            }
            else
            {
              demandaBarraDiaria.ValorScada = double.NaN;
              demandaBarraDiaria.ExisteValorScada = false;
            }
          }
          else
            demandaBarraDiaria.ValorScada = double.NaN;
          demandaBarraDiaria.Calidad = !demandaBarraDiaria.ExisteValorScada ? -1 : (Math.Abs((demandaBarraDiaria.ValorScada - demandaBarraDiaria.ValorEjecutado) / demandaBarraDiaria.ValorScada) * 100.0 <= 5.0 ? 0 : 1);
          adt_fecha1 = adt_fecha1.AddMinutes(30.0);
          adt_fecha2 = adt_fecha2.AddMinutes(30.0);
          demandaBarraDiaria.Fecha = adt_fecha1;
          demandaBarraDiaria.FechaPronostico = adt_fecha2;
          row[0] = (object) demandaBarraDiaria.ValorEjecutado;
          row[1] = (object) demandaBarraDiaria.ValorScada;
          row[2] = (object) demandaBarraDiaria.ValorPronostico;
          row[3] = (object) demandaBarraDiaria.Calidad;
          row[4] = !demandaBarraDiaria.ExisteValorScada ? (object) 0 : (object) 1;
          dataTable.Rows.Add(row);
        }
      }
      catch (Exception ex)
      {
        return (DataTable) null;
      }
      return dataTable;
    }

    public DataTable DemandaBarraSemanalReportada96(
      DateTime adt_fecha_rep,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return (DataTable) null;
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_DemandaBarraReporteSemanal96(adt_fecha_rep, ai_lectcodi, ai_tipoinf, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return dataTable;
    }

    public int AgregarDemanda48(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores48,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_add_Demanda48(adt_fecha, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, arr_valores48);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int AgregarDemanda96(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores96,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_add_Demanda96(adt_fecha, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, arr_valores96);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int AgregarDemanda96(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores96,
      ref OracleDataAccessX an_conex,
      string as_credencial)
    {
      int num = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(ref an_conex);
      return Validador.nf_valida_credencial(as_credencial) ? demandaBarra.nf_add_Demanda96(adt_fecha, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, arr_valores96) : num;
    }

    public int AgregarDemanda96(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores96,
      ref OracleDataAccessX an_conex)
    {
      return new Sicoes.DemandaBarra.DemandaBarra(ref an_conex).nf_add_Demanda96(adt_fecha, ai_lectcodi, ai_tipoinf, ai_ptomedicodi, arr_valores96);
    }

    public int AgregarDemandaFuente(
      int ai_earcodi,
      int ai_etacodi,
      int ai_ptomedicodi,
      DateTime adt_fecha_rep,
      string as_fuente,
      ref OracleDataAccessX an_conex)
    {
      return new Sicoes.DemandaBarra.DemandaBarra(ref an_conex).nf_add_DemandaFuente(ai_earcodi, ai_etacodi, ai_ptomedicodi, adt_fecha_rep, as_fuente);
    }

    public int Agregar_ratio(
      int ai_earcodi,
      int ai_eaicodi,
      int ai_ninformado,
      int ai_ntotal,
      double ad_ratio,
      ref OracleDataAccessX an_conex)
    {
      return new Sicoes.DemandaBarra.DemandaBarra(ref an_conex).nf_add_ext_ratio(ai_earcodi, ai_eaicodi, ai_ninformado, ai_ntotal, ad_ratio);
    }

    public int EliminarDemanda96(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_ptomedicodi,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_del_Demanda96(adt_fecha, ai_lectcodi, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int EliminarDemanda96(
      DateTime adt_f_fini,
      DateTime adt_f_fin,
      int ai_lectcodi,
      int ai_ptomedicodi,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_del_Demanda96(adt_f_fini, adt_f_fin, ai_lectcodi, ai_ptomedicodi);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int EliminarDemanda96DiaExe(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_del_Demanda96xEmpDiaExe(adt_f_ini, adt_f_fin, ai_emprcodi);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int EliminarDemanda96DiaPro(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_del_Demanda96xEmpDiaPro(adt_f_ini, adt_f_fin, ai_emprcodi);
      demandaBarra.nf_close_source();
      return num2;
    }

    public int EliminarDemanda96SemPro(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial)
    {
      int num1 = -1;
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      if (!Validador.nf_valida_credencial(as_credencial))
        return num1;
      demandaBarra.nf_open_source();
      int num2 = demandaBarra.nf_del_Demanda96xEmpSemPro(adt_f_ini, adt_f_fin, ai_emprcodi);
      demandaBarra.nf_close_source();
      return num2;
    }

    public List<Cumplimiento> ReporteCumplimiento(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_tipo)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      List<Cumplimiento> cumplimientoList = new List<Cumplimiento>();
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_Cumplimiento(adt_ini, adt_fin, ai_tipo);
      int num;
      if (ai_tipo == 2)
      {
        num = adt_fin.Subtract(adt_ini).Days;
        if (num > 0)
          ++num;
      }
      else
        num = this.nf_numdias_rango(adt_ini, adt_fin, DayOfWeek.Saturday);
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        Cumplimiento cumplimiento = new Cumplimiento()
        {
          Inicio = adt_ini,
          Fin = adt_fin,
          Empresa = row["nombre"].ToString(),
          TipoEmpresa = row["tipoemprdesc"].ToString(),
          NCumplimiento = Convert.ToInt32(row["nenv"].ToString())
        };
        cumplimiento.NNoCumplimiento = num - cumplimiento.NCumplimiento;
        cumplimiento.Emprcodi = Convert.ToInt32(row["emprcodi"]);
        cumplimientoList.Add(cumplimiento);
      }
      demandaBarra.nf_close_source();
      return cumplimientoList;
    }

    public List<CumplimientoDetalle> ReporteCumplimientoDet(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi,
      int ai_tipo)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      List<CumplimientoDetalle> cumplimientoDetalleList = new List<CumplimientoDetalle>();
      demandaBarra.nf_open_source();
      DataTable dataTable = demandaBarra.nf_CumplimientoDetalle(adt_ini, adt_fin, ai_emprcodi, ai_tipo);
      int days = adt_fin.Subtract(adt_ini).Days;
      for (DateTime dateTime = adt_ini; dateTime <= adt_fin; dateTime = dateTime.AddDays(1.0))
      {
        CumplimientoDetalle cumplimientoDetalle = new CumplimientoDetalle();
        cumplimientoDetalle.Fecha = dateTime;
        if (ai_tipo == 2)
        {
          DataRow[] dataRowArray = dataTable.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}#", new object[1]
          {
            (object) dateTime
          }));
          cumplimientoDetalle.Descripcion = dataRowArray == null ? "No cumplio" : (dataRowArray.Length <= 0 ? "No cumplio" : "Cumplio");
          cumplimientoDetalleList.Add(cumplimientoDetalle);
        }
        else if (dateTime.DayOfWeek == DayOfWeek.Saturday)
        {
          DataRow[] dataRowArray = dataTable.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}#", new object[1]
          {
            (object) dateTime
          }));
          cumplimientoDetalle.Descripcion = dataRowArray == null ? "No cumplio" : (dataRowArray.Length <= 0 ? "No cumplio" : "Cumplio");
          cumplimientoDetalleList.Add(cumplimientoDetalle);
        }
      }
      demandaBarra.nf_close_source();
      return cumplimientoDetalleList;
    }

    public List<CumplimientoDiario> ReporteCumplimientoDiario(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      List<CumplimientoDiario> cumplimientoDiarioList = new List<CumplimientoDiario>();
      DateTime now1 = DateTime.Now;
      DateTime now2 = DateTime.Now;
      demandaBarra.nf_open_source();
      DataTable dataTable1 = demandaBarra.nf_Cumplimiento2(ai_emprcodi, adt_ini, adt_fin, 2);
      DataTable dataTable2 = demandaBarra.nf_Cumplimiento2(ai_emprcodi, adt_ini, adt_fin, 3);
      for (DateTime adt_fbusq = adt_ini; adt_fbusq <= adt_fin; adt_fbusq = adt_fbusq.AddDays(1.0))
      {
        CumplimientoDiario cumplimientoDiario = new CumplimientoDiario();
        cumplimientoDiario.Fecha = adt_fbusq;
        DateTime dateTime = new DateTime(adt_fbusq.Year, adt_fbusq.Month, adt_fbusq.Day, 8, 0, 0);
        DataRow[] dataRowArray = dataTable1.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}# ", new object[1]
        {
          (object) adt_fbusq
        }));
        cumplimientoDiario.Diario = dataRowArray == null ? "No envio" : (dataRowArray.Length <= 0 ? "No envio" : "Envio");
        DataRow[] ao_datos = dataTable2.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}#", new object[1]
        {
          (object) adt_fbusq
        }));
        cumplimientoDiario.Semanal = ao_datos == null ? "No envio" : (ao_datos.Length <= 0 ? "No envio" : (!this.nf_existe_envio_sem(ao_datos, adt_fbusq) ? "No envio" : "Envio"));
        cumplimientoDiarioList.Add(cumplimientoDiario);
      }
      demandaBarra.nf_close_source();
      return cumplimientoDiarioList;
    }

    public List<CumplimientoDiario> ReporteCumplimientoDiario2(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      List<CumplimientoDiario> cumplimientoDiarioList = new List<CumplimientoDiario>();
      DateTime now1 = DateTime.Now;
      DateTime now2 = DateTime.Now;
      demandaBarra.nf_open_source();
      DataTable dataTable1 = demandaBarra.nf_Cumplimiento2(ai_emprcodi, adt_ini, adt_fin, 2);
      DataTable dataTable2 = demandaBarra.nf_Cumplimiento2(ai_emprcodi, adt_ini, adt_fin, 3);
      for (DateTime adt_fbusq = adt_ini; adt_fbusq <= adt_fin; adt_fbusq = adt_fbusq.AddDays(1.0))
      {
        CumplimientoDiario cumplimientoDiario = new CumplimientoDiario();
        cumplimientoDiario.Fecha = adt_fbusq;
        DateTime dateTime = new DateTime(adt_fbusq.Year, adt_fbusq.Month, adt_fbusq.Day, 8, 0, 0);
        DataRow[] dataRowArray = dataTable1.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}# ", new object[1]
        {
          (object) adt_fbusq
        }));
        if (dataRowArray != null)
        {
          if (dataRowArray.Length > 0)
          {
            foreach (DataRow dataRow in dataRowArray)
            {
              if (dataRow["tipoenvio"].ToString() == "1" && dataRow["plazo"].ToString() == "P")
                cumplimientoDiario.Diario = "Envio";
              else if (dataRow["tipoenvio"].ToString() == "2" && dataRow["plazo"].ToString() == "P")
                cumplimientoDiario.Diario = "Envio";
              else if (dataRow["tipoenvio"].ToString() == "1" && dataRow["plazo"].ToString() == "F")
                cumplimientoDiario.Diario = "No Envio";
              else if (dataRow["tipoenvio"].ToString() == "2" && dataRow["plazo"].ToString() == "F")
                cumplimientoDiario.Diario = "Envio";
              else if (dataRow["tipoenvio"].ToString() == "3" && dataRow["plazo"].ToString() == "P")
                cumplimientoDiario.Diario = "Envio";
              else if (dataRow["tipoenvio"].ToString() == "3" && dataRow["plazo"].ToString() == "F")
                cumplimientoDiario.Diario = "No Envio";
            }
          }
          else
            cumplimientoDiario.Diario = "No envio";
        }
        else
          cumplimientoDiario.Diario = "No envio";
        DataRow[] ao_datos = dataTable2.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}#", new object[1]
        {
          (object) adt_fbusq
        }));
        cumplimientoDiario.Semanal = ao_datos == null ? "No envio" : (ao_datos.Length <= 0 ? "No envio" : (!this.nf_existe_envio_sem(ao_datos, adt_fbusq) ? "No envio" : "Envio"));
        cumplimientoDiarioList.Add(cumplimientoDiario);
      }
      demandaBarra.nf_close_source();
      return cumplimientoDiarioList;
    }

    private List<CumplimientoDiario> ReporteCumplimientoDiario(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi,
      ref OracleDataAccessX lo_conex)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(ref lo_conex);
      List<CumplimientoDiario> cumplimientoDiarioList = new List<CumplimientoDiario>();
      DateTime now1 = DateTime.Now;
      DateTime now2 = DateTime.Now;
      DataTable dataTable1 = demandaBarra.nf_Cumplimiento(ai_emprcodi, adt_ini, adt_fin, 2);
      DataTable dataTable2 = demandaBarra.nf_Cumplimiento(ai_emprcodi, adt_ini, adt_fin, 3);
      for (DateTime adt_fbusq = adt_ini; adt_fbusq <= adt_fin; adt_fbusq = adt_fbusq.AddDays(1.0))
      {
        CumplimientoDiario cumplimientoDiario = new CumplimientoDiario();
        cumplimientoDiario.Fecha = adt_fbusq;
        DateTime dateTime = new DateTime(adt_fbusq.Year, adt_fbusq.Month, adt_fbusq.Day, 8, 0, 0);
        DataRow[] dataRowArray = dataTable1.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}#", new object[1]
        {
          (object) adt_fbusq
        }));
        cumplimientoDiario.Diario = dataRowArray == null ? "No envio" : (dataRowArray.Length <= 0 ? "No envio" : "Envio");
        DataRow[] ao_datos = dataTable2.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha <= #{0}#", new object[1]
        {
          (object) adt_fbusq
        }));
        cumplimientoDiario.Semanal = ao_datos == null ? "No envio" : (ao_datos.Length <= 0 ? "No envio" : (!this.nf_existe_envio_sem(ao_datos, adt_fbusq) ? "No envio" : "Envio"));
        cumplimientoDiarioList.Add(cumplimientoDiario);
      }
      return cumplimientoDiarioList;
    }

    public List<CumplimientoPorcEmp> ReporteCumplimientoPorc(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_tipo_emp)
    {
      List<CumplimientoPorcEmp> cumplimientoPorcEmpList = new List<CumplimientoPorcEmp>();
      SicMasterSI sicMasterSi = new SicMasterSI(this.ls_dns_sic);
      sicMasterSi.nf_open_source();
      foreach (DataRow row in (InternalDataCollectionBase) sicMasterSi.nf_get_empresas(ai_tipo_emp).Rows)
      {
        CumplimientoPorcEmp cumplimientoPorcEmp = new CumplimientoPorcEmp();
        cumplimientoPorcEmp.Empresa = row[1].ToString();
        cumplimientoPorcEmp.FechaInicial = adt_ini;
        cumplimientoPorcEmp.FechaFinal = adt_fin;
        double ao_val_sem;
        double ao_val_men;
        this.nf_ratio_emp(this.ReporteCumplimientoDiario(adt_ini, adt_fin, Convert.ToInt32(row[0]), ref sicMasterSi.ln_conex_ora), out ao_val_sem, out ao_val_men);
        cumplimientoPorcEmp.ValorSem = ao_val_sem;
        cumplimientoPorcEmp.ValorMen = ao_val_men;
        cumplimientoPorcEmp.ValorAnual = 100.0;
        cumplimientoPorcEmpList.Add(cumplimientoPorcEmp);
      }
      sicMasterSi.nf_close_source();
      return cumplimientoPorcEmpList;
    }

    private bool nf_existe_envio_sem_old(DataRow[] ao_datos, DateTime adt_fbusq)
    {
      foreach (DataRow aoDato in ao_datos)
      {
        DateTime dateTime1 = Convert.ToDateTime(aoDato[1]);
        DateTime dateTime2 = Convert.ToDateTime(aoDato[2]);
        double totalSeconds = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 8, 0, 0).Subtract(dateTime2).TotalSeconds;
        if (1.0 >= 0.0 && (dateTime1.Date == adt_fbusq.Date || dateTime1.AddDays(7.0) >= dateTime1))
          return true;
      }
      return false;
    }

    private bool nf_existe_envio_sem(DataRow[] ao_datos, DateTime adt_fbusq)
    {
      foreach (DataRow aoDato in ao_datos)
      {
        DateTime dateTime1 = Convert.ToDateTime(aoDato[1]);
        DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 8, 0, 0);
        if (!(aoDato["plazo"].ToString() == "P") && (dateTime1.Date == adt_fbusq.Date || dateTime1.AddDays(7.0) >= dateTime1))
          return true;
      }
      return false;
    }

    private void nf_ratio_emp(
      List<CumplimientoDiario> an_cump,
      out double ao_val_sem,
      out double ao_val_men)
    {
      int num1 = 0;
      int num2 = 0;
      ao_val_sem = 0.0;
      ao_val_men = 0.0;
      try
      {
        int num3 = an_cump.Count<CumplimientoDiario>();
        foreach (CumplimientoDiario cumplimientoDiario in an_cump)
        {
          if (cumplimientoDiario.Diario == "Envio")
            ++num2;
          if (cumplimientoDiario.Semanal == "Envio")
            ++num1;
        }
        if (num3 <= 0)
          return;
        ao_val_sem = (double) (num1 / num3);
        ao_val_men = (double) (num2 / num3);
      }
      catch (Exception ex)
      {
      }
    }

    public List<CumplimientoDiarioxEmp> ReporteCumplimientoxEmpresa(
      DateTime adt_fini,
      DateTime adt_ffin,
      int ai_tipo,
      int ai_tipo_em,
      int ai_frec)
    {
      Sicoes.DemandaBarra.DemandaBarra demandaBarra = new Sicoes.DemandaBarra.DemandaBarra(this.ls_dns_sic);
      int num1 = -1;
      int num2 = -1;
      int num3 = 0;
      List<CumplimientoDiarioxEmp> cumplimientoDiarioxEmpList = new List<CumplimientoDiarioxEmp>();
      demandaBarra.nf_open_source();
      DataTable demandaBarraTipo = demandaBarra.nf_get_EmpresasDemandaBarraTipo(ai_tipo);
      int ndiasDbarra = demandaBarra.nf_get_ndias_dbarra(adt_fini, adt_ffin, ai_frec);
      foreach (DataRow row in (InternalDataCollectionBase) demandaBarraTipo.Rows)
      {
        DataTable dataTable = demandaBarra.nf_Cumplimiento2(Convert.ToInt32(row[0]), adt_fini, adt_ffin, ai_tipo);
        CumplimientoDiarioxEmp cumplimientoDiarioxEmp = new CumplimientoDiarioxEmp();
        cumplimientoDiarioxEmp.Empresa = row[1].ToString();
        cumplimientoDiarioxEmp.Potencia = "";
        cumplimientoDiarioxEmp.EsIntegrante = row[2].ToString();
        cumplimientoDiarioxEmp.EnvPr30 = ndiasDbarra;
        cumplimientoDiarioxEmp.EnvPlazo = 0;
        cumplimientoDiarioxEmp.EnvFPlazo = 0;
        cumplimientoDiarioxEmp.NoEnviados = 0;
        cumplimientoDiarioxEmp.Orden = num3++;
        for (DateTime dateTime = adt_fini; dateTime <= adt_ffin; dateTime = dateTime.AddDays(1.0))
        {
          DataRow[] dataRowArray = dataTable.Select(string.Format((IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat, "fecha = #{0}# ", new object[1]
          {
            (object) dateTime
          }));
          if (dataRowArray != null)
          {
            foreach (DataRow dataRow in dataRowArray)
            {
              if (dataRow["tipoenvio"].ToString() == "1" && dataRow["plazo"].ToString() == "P")
                num1 = 1;
              else if (dataRow["tipoenvio"].ToString() == "2" && dataRow["plazo"].ToString() == "P")
                num2 = 1;
              else if (dataRow["tipoenvio"].ToString() == "1" && dataRow["plazo"].ToString() == "F")
                num1 = 0;
              else if (dataRow["tipoenvio"].ToString() == "2" && dataRow["plazo"].ToString() == "F")
                num2 = 0;
              else if (dataRow["tipoenvio"].ToString() == "3" && dataRow["plazo"].ToString() == "P")
              {
                num1 = 1;
                num2 = 1;
              }
              else if (dataRow["tipoenvio"].ToString() == "3" && dataRow["plazo"].ToString() == "F")
              {
                num1 = 0;
                num2 = 0;
              }
            }
            if (ai_frec <= 0)
            {
              if (num1 == 1 && num2 == 1)
                ++cumplimientoDiarioxEmp.EnvPlazo;
              else if (num1 == 1 && num2 <= 0)
                ++cumplimientoDiarioxEmp.EnvFPlazo;
              else if (num1 <= 0 && num2 == 1)
                ++cumplimientoDiarioxEmp.EnvFPlazo;
              else
                ++cumplimientoDiarioxEmp.EnvFPlazo;
            }
            else
            {
              switch (num2)
              {
                case 0:
                  ++cumplimientoDiarioxEmp.EnvFPlazo;
                  continue;
                case 1:
                  ++cumplimientoDiarioxEmp.EnvPlazo;
                  continue;
                default:
                  ++cumplimientoDiarioxEmp.NoEnviados;
                  continue;
              }
            }
          }
          else
            ++cumplimientoDiarioxEmp.NoEnviados;
        }
        cumplimientoDiarioxEmp.PorCumplimiento = Math.Round((double) (cumplimientoDiarioxEmp.EnvPlazo / cumplimientoDiarioxEmp.EnvPr30) * 100.0, 4);
        cumplimientoDiarioxEmpList.Add(cumplimientoDiarioxEmp);
      }
      demandaBarra.nf_close_source();
      return cumplimientoDiarioxEmpList;
    }

    public string Version() => this.ls_version;

    private int nf_numdias_rango(DateTime adt_ini, DateTime adt_fin, DayOfWeek adw_dia)
    {
      int num = 0;
      for (DateTime dateTime = adt_ini; dateTime <= adt_fin; dateTime = dateTime.AddDays(1.0))
      {
        if (dateTime.DayOfWeek == adw_dia)
          ++num;
      }
      return num;
    }
  }
}
