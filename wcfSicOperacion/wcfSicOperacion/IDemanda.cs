// Decompiled with JetBrains decompiler
// Type: wcfSicOperacion.IDemanda
// Assembly: wcfSicOperacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB6DF14C-22DB-4213-A671-4E83B266BC5E
// Assembly location: C:\d\wsSICOES\wcfSicOperacion.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace wcfSicOperacion
{
  [ServiceContract]
  public interface IDemanda
  {
    [OperationContract]
    DataTable EmpresasRepxUsuario(string[] as_arr_emp, string as_credencial);

    [OperationContract]
    DataTable PuntoMedicionBarraxEmp(int ai_emprcodi, string as_credencial);

    [OperationContract]
    DataTable PuntoMedicionBarraDescxEmp(int ai_emprcodi, string as_credencial);

    [OperationContract]
    DataTable PuntoMedicionBarraDesc(int ai_ptomedicodi, string as_credencial);

    [OperationContract]
    DataTable ObtenerDemandaFuente(
      int ai_etacodi,
      DateTime adt_fecha_rep,
      int ai_ptomedicodi,
      string as_credencial);

    [OperationContract]
    DataTable ListarEnviosxEmpresas(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      int ai_tipempr,
      string as_credencial);

    [OperationContract]
    DataTable ListarEnviosxEmpresaHistorico(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_tipo_empr,
      int ai_tipo,
      string as_credencial);

    [OperationContract]
    DataTable ObtenerDemandaBarraDiario96Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi_real,
      int ai_lectcodi_prog,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial);

    [OperationContract]
    DataTable ObtenerDemandaBarraDiario96Fhora2(
      DateTime adt_fini,
      DateTime adt_ffin,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_etacodi,
      string as_credencial);

    [OperationContract]
    DataTable ObtenerDemandaBarraSemanal96Fhora(
      DateTime adt_fecha_rep,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraDiariaReportada96(
      DateTime adt_fecha_rep,
      int ai_lectcodi_real,
      int ai_lectcodi_prog,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_canalcodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraSemanalReportada96(
      DateTime adt_fecha_rep,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraReporte48from48T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraReporte48from48T2(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      string ai_emprecodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraReporte48from96T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial);

    [OperationContract]
    DataTable DemandaBarraReporte96from96T(
      DateTime adt_fi,
      DateTime adt_ff,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      int ai_emprecodi,
      string as_credencial);

    [OperationContract]
    int AgregarDemanda48(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores48,
      string as_credencial);

    [OperationContract]
    int AgregarDemanda96(
      DateTime adt_fecha,
      int ai_lectcodi,
      int ai_tipoinf,
      int ai_ptomedicodi,
      double[] arr_valores96,
      string as_credencial);

    [OperationContract]
    int EliminarDemanda96DiaExe(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial);

    [OperationContract]
    int EliminarDemanda96DiaPro(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial);

    [OperationContract]
    int EliminarDemanda96SemPro(
      DateTime adt_f_ini,
      DateTime adt_f_fin,
      int ai_emprcodi,
      string as_credencial);

    [OperationContract]
    List<Cumplimiento> ReporteCumplimiento(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_tipo);

    [OperationContract]
    List<CumplimientoDetalle> ReporteCumplimientoDet(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi,
      int ai_tipo);

    [OperationContract]
    List<CumplimientoDiario> ReporteCumplimientoDiario(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi);

    [OperationContract]
    List<CumplimientoDiario> ReporteCumplimientoDiario2(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_emprcodi);

    [OperationContract]
    List<CumplimientoPorcEmp> ReporteCumplimientoPorc(
      DateTime adt_ini,
      DateTime adt_fin,
      int ai_tipo_emp);

    [OperationContract]
    List<CumplimientoDiarioxEmp> ReporteCumplimientoxEmpresa(
      DateTime adt_fini,
      DateTime adt_ffin,
      int ai_tipo,
      int ai_tipo_em,
      int ai_frec);

    [OperationContract]
    string Version();
  }
}
