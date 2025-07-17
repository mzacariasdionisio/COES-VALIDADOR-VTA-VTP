using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_CONCEPTO
    /// </summary>
    public interface IPfrConceptoRepository
    {
        int Save(PfrConceptoDTO entity);
        void Update(PfrConceptoDTO entity);
        void Delete(int pfrcnpcodi);
        PfrConceptoDTO GetById(int pfrcnpcodi);
        List<PfrConceptoDTO> List();
        List<PfrConceptoDTO> GetByCriteria();
    }
}
