using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_URS_MODO_OPERACION
    /// </summary>
    public interface ISmaUrsModoOperacionRepository
    {               
        List<SmaUrsModoOperacionDTO> List();

        List<SmaUrsModoOperacionDTO> ListUrs(int usercode);

        List<SmaUrsModoOperacionDTO> ListInUrs(int usercode);

        List<SmaUrsModoOperacionDTO> ListMO(int urscodi);

        List<SmaUrsModoOperacionDTO> GetByCriteria();

    }
}
