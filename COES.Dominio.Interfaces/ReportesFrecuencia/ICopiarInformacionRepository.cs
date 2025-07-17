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
    public interface ICopiarInformacionRepository
    {
        CopiarInformacionDTO GetById(int IdCopia);
        List<CopiarInformacionDTO> GetListaCopiarInformacion(string FechaInicial, string FechaFinal, string CodEquipoOrigen, string CodEquipoDestino);
        CopiarInformacionDTO SaveUpdate(CopiarInformacionDTO entity);
        CopiarInformacionDTO Eliminar(CopiarInformacionDTO entity);
    }
}
