using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_CAUSAEVENTO
    /// </summary>
    public interface IEveCausaeventoRepository
    {
        int Save(EveCausaeventoDTO entity);
        void Update(EveCausaeventoDTO entity);
        void Delete(int causaevencodi);
        EveCausaeventoDTO GetById(int causaevencodi);
        List<EveCausaeventoDTO> List();
        List<EveCausaeventoDTO> GetByCriteria();
    }
}

