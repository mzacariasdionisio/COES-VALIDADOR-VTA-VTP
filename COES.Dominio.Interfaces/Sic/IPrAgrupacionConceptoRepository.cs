using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_AGRUPACIONCONCEPTO
    /// </summary>
    public interface IPrAgrupacionConceptoRepository
    {
        int Save(PrAgrupacionConceptoDTO entity);
        void Update(PrAgrupacionConceptoDTO entity);
        void Delete(int agrconcodi);
        PrAgrupacionConceptoDTO GetById(int agrconcodi);
        List<PrAgrupacionConceptoDTO> List();
        List<PrAgrupacionConceptoDTO> GetByCriteria(int agrupcodi);
    }
}
