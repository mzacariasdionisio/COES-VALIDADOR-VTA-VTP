using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_PARAMETRO
    /// </summary>
    public interface ICpParametroRepository
    {
        CpParametroDTO GetById(int paramcodi, int topcodi);
        List<CpParametroDTO> List();
        List<CpParametroDTO> GetByCriteria(int topcodi);
        void CopiarParametroAEscenario(int topcodiOrigen, int topcodiDestino);
    }
}
