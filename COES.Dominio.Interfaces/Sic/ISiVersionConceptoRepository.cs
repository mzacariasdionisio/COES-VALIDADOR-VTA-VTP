using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_VERSION_CONCEPTO
    /// </summary>
    public interface ISiVersionConceptoRepository
    {
        int Save(SiVersionConceptoDTO entity);
        void Update(SiVersionConceptoDTO entity);
        void Delete(int vercnpcodi);
        SiVersionConceptoDTO GetById(int vercnpcodi);
        List<SiVersionConceptoDTO> List();
        List<SiVersionConceptoDTO> GetByCriteria();
    }
}
