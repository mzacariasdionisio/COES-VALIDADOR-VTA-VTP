using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_PROPIEDAD
    /// </summary>
    public interface ICpPropiedadRepository
    {
        List<CpPropiedadDTO> List();
        List<CpPropiedadDTO> GetByCriteria(int catcodi);
    }
}
