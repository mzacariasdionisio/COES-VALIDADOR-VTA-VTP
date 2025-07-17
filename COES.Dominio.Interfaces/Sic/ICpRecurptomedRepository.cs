using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_RECURPTOMED
    /// </summary>
    public interface ICpRecurptomedRepository
    {

        void CrearCopia(int topcodi1, int topcodi2);
        List<CpRecurptomedDTO> ListByTopcodi(int topcodi);
    }
}
