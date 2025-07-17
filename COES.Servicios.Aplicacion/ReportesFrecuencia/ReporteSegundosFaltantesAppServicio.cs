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
    public class ReporteSegundosFaltantesAppServicio : AppServicioBase
    {
        public List<ReporteSegundosFaltantesDTO> GetReporteSegundosFaltantes(ReporteSegundosFaltantesParam param)
        {
            List<ReporteSegundosFaltantesDTO> listaReporte = new List<ReporteSegundosFaltantesDTO>();
            var resultado = FactoryReportesFrecuencia.GetReporteSegundosFaltantesRepository().GetReporteSegundosFaltantes(param);
            foreach (var item in resultado.ToList())
            {
                listaReporte.Add(new ReporteSegundosFaltantesDTO
                {
                    GPSCodi = item.GPSCodi,
                    FechaHora = item.FechaHora,
                    Num = item.Num
                });
            }
            return listaReporte;
        }

        public List<ReporteSegundosFaltantesDTO> GetReporteTotalSegundosFaltantes(ReporteSegundosFaltantesParam param)
        {
            List<ReporteSegundosFaltantesDTO> listaReporte = new List<ReporteSegundosFaltantesDTO>();
            var resultado = FactoryReportesFrecuencia.GetReporteSegundosFaltantesRepository().GetReporteTotalSegundosFaltantes(param);
            foreach (var item in resultado.ToList())
            {
                listaReporte.Add(new ReporteSegundosFaltantesDTO
                {
                    GPSCodi = item.GPSCodi,
                    FechaHora = item.FechaHora,
                    Num = item.Num
                });
            }
            return listaReporte;
        }

    }
}
