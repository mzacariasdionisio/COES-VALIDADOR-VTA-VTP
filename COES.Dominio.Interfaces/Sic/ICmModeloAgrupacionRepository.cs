using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_MODELO_AGRUPACION
    /// </summary>
    public interface ICmModeloAgrupacionRepository
    {
        int Save(CmModeloAgrupacionDTO entity);
        void Update(CmModeloAgrupacionDTO entity);
        void Delete(int modagrcodi);
        CmModeloAgrupacionDTO GetById(int modagrcodi);
        List<CmModeloAgrupacionDTO> List();
        List<CmModeloAgrupacionDTO> GetByCriteria(int modembcodi);
    }
}
