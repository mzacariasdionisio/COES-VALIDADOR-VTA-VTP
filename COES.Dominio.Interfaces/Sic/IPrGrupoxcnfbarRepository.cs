using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPOXCNFBAR
    /// </summary>
    public interface IPrGrupoxcnfbarRepository
    {
        int Save(PrGrupoxcnfbarDTO entity);
        void Update(PrGrupoxcnfbarDTO entity);
        void Delete(PrGrupoxcnfbarDTO entity);
        PrGrupoxcnfbarDTO GetById(int grcnfbcodi);
        PrGrupoxcnfbarDTO GetByGrupocodi(int grupocodi);
        List<PrGrupoxcnfbarDTO> List();
        List<PrGrupoxcnfbarDTO> GetByCriteria();
    }
}
