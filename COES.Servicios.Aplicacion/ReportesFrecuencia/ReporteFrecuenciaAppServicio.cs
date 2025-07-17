using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ReportesFrecuencia
{
    public class ReporteFrecuenciaAppServicio : AppServicioBase
    {
        public List<EquipoGPSReporteDTO> GetListaGPS()
        {
            List<EquipoGPSReporteDTO> listaGPS = new List<EquipoGPSReporteDTO>();
            var resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetListaEquipoGPS();
            foreach (var item in resultado.Where(x => x.GPSEstado == "A").ToList())
            {
                listaGPS.Add(new EquipoGPSReporteDTO
                {
                    Id = item.GPSCodi,
                    GPS = item.NombreEquipo,
                    Oficial = item.GPSOficial
                });
            }
            return listaGPS;
        }

        public List<int> GetGPSPorRangoFechas(ReporteFrecuenciaParam param)
        {
            List<int> Ids = new List<int>();
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().ObtenerGPSPorRangoFechas(param);
            return resultado;
        }

        public List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuencia(ReporteFrecuenciaParam param)
        {
            //List<ReporteFrecuenciaDescargaDTO> listaFrecuencias = new List<ReporteFrecuenciaDescargaDTO>();
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().ObtenerFrecuencias(param);

            return resultado;
        }

        public List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuenciaMinuto(ReporteFrecuenciaParam param)
        {
            //List<ReporteFrecuenciaDescargaDTO> listaFrecuencias = new List<ReporteFrecuenciaDescargaDTO>();
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().ObtenerFrecuenciasMinuto(param);

            return resultado;
        }

        public object ObtenerSQL(string query, string Tipo)
        {
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().ObtenerSQL(query, Tipo);
            return resultado;
        }

        public List<EquipoGPSDTO> ObtenerGPSs(ReporteFrecuenciaParam param)
        {
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().ObtenerGPSs(param);
            return resultado;
        }
        public System.Data.DataSet Reportes(int idGPS, string gps, DateTime fechaIni, DateTime fechaFin, string Tipo)
        {
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().Reportes(idGPS, gps, fechaIni, fechaFin, Tipo);
            return resultado;
        }
        public int Indicadores(int idGPS, string gps, DateTime fecha)
        {
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().Indicadores(idGPS, gps, fecha);
            return resultado;
        }
        public System.Data.DataSet TransgresionMensual(string inicio, string fin)
        {
            var resultado = FactoryReportesFrecuencia.GetReporteFrecuenciaRepository().TransgresionMensual(inicio, fin);
            return resultado;
        }
    }
}
