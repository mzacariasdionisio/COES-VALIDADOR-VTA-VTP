using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_SUBRESTRICCION
    /// </summary>
    public interface ICpSubrestriccionRepository
    {

        CpSubrestriccionDTO GetById(int srestriccodi, int topcodi);
        List<CpSubrestriccionDTO> List();
        List<CpSubrestriccionDTO> GetByCriteria(short restriccodi);

    }
}
