using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_CONCEPTO
    /// </summary>
    public interface ISpoConceptoRepository
    {
        int Save(SpoConceptoDTO entity);
        void Update(SpoConceptoDTO entity);
        void Delete(int sconcodi);
        SpoConceptoDTO GetById(int sconcodi);
        List<SpoConceptoDTO> List();
        List<SpoConceptoDTO> GetByCriteria(int numecodi);
    }
}
