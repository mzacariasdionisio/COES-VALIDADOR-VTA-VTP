using COES.Dominio.DTO.ReportesFrecuencia;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IReporteSegundosFaltantesRepository
    {
        List<ReporteSegundosFaltantesDTO> GetReporteSegundosFaltantes(ReporteSegundosFaltantesParam param);

        List<ReporteSegundosFaltantesDTO> GetReporteTotalSegundosFaltantes(ReporteSegundosFaltantesParam param);
    }
}