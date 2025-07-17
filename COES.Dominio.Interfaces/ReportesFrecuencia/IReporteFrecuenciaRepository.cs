using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;


namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IReporteFrecuenciaRepository
    {
        List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuencias(ReporteFrecuenciaParam param);
        List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuenciasMinuto(ReporteFrecuenciaParam param);
        List<int> ObtenerGPSPorRangoFechas(ReporteFrecuenciaParam param);
        object ObtenerSQL(string query, string Tipo);
        List<EquipoGPSDTO> ObtenerGPSs(ReporteFrecuenciaParam param);
        System.Data.DataSet Reportes(int idGPS, string gps, DateTime fechaIni, DateTime fechaFin, string Tipo);
        int Indicadores(int idGPS, string gps, DateTime fecha);
        System.Data.DataSet TransgresionMensual(string inicio, string fin);
    }
}
