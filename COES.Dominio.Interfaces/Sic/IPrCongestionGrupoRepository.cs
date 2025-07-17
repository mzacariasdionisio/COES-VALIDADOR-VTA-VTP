using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CONGESTION_GRUPO
    /// </summary>
    public interface IPrCongestionGrupoRepository
    {
        int Save(PrCongestionGrupoDTO entity);
        void Update(PrCongestionGrupoDTO entity);
        void Delete(int congrpcodi);
        PrCongestionGrupoDTO GetById(int congrpcodi);
        List<PrCongestionGrupoDTO> List();
        List<PrCongestionGrupoDTO> GetByCriteria(int congescodi);
    }
}
