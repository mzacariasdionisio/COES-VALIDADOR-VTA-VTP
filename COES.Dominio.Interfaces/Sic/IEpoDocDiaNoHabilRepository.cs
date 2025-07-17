using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DOC_DIA_ESP
    /// </summary>
    public interface IEpoDocDiaNoHabilRepository
    {
        List<EpoDocDiaNoHabilDTO> List();
        DateTime ObtenerDiasNoHabiles(DateTime fInicio, int Dias);
        DateTime ObtenerDiasNoHabSinFS(DateTime fInicio, int Dias);
    }
}
