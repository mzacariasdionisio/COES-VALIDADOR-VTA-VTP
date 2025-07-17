using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_EVENTO_LOG
    /// </summary>
    public interface IEveEventoLogRepository
    {
        int Save(EveEventoLogDTO entity);
        void Update(EveEventoLogDTO entity);
        void Delete(int evelogcodi);
        EveEventoLogDTO GetById(int evelogcodi);
        List<EveEventoLogDTO> List(int idEvento);
        List<EveEventoLogDTO> GetByCriteria();
    }
}
