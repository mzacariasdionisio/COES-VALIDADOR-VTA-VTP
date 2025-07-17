using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CONCEPTO
    /// </summary>
    public interface ISiConceptoRepository
    {
        int Save(SiConceptoDTO entity);
        void Update(SiConceptoDTO entity);
        void Delete(int consiscodi);
        SiConceptoDTO GetById(int consiscodi);
        List<SiConceptoDTO> List();
        List<SiConceptoDTO> GetByCriteria();
    }
}
