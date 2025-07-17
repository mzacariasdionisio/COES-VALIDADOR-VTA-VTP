using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IInformacionFrecuenciaRepository
    {
        List<InformacionFrecuenciaDTO> GetReporteFrecuenciaDesviacion();

        List<InformacionFrecuenciaDTO> GetReporteEventosFrecuencia();
    }
}
