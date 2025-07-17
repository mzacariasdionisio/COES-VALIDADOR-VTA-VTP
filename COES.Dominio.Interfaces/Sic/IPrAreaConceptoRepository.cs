using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_AREACONCEPTO
    /// </summary>
    public interface IPrAreaConceptoRepository
    {
        int Save(PrAreaConceptoDTO entity);
        void Update(PrAreaConceptoDTO entity);
        void Delete(int arconcodi);
        PrAreaConceptoDTO GetById(int arconcodi);
        List<PrAreaConceptoDTO> List();
        List<PrAreaConceptoDTO> GetByCriteria(int concepcodi, string arconactivo);
        List<int> ListarConcepcodiRegistrados();
    }
}
