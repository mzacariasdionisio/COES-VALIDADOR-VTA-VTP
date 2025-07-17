using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_INTERCONEXION
    /// </summary>
    public interface IInInterconexionRepository
    {
        List<InInterconexionDTO> List();
        InInterconexionDTO GetById(int id);
    }
}
