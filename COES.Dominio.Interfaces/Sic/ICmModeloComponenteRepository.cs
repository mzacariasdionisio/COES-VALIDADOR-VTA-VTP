using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_MODELO_COMPONENTE
    /// </summary>
    public interface ICmModeloComponenteRepository
    {
        int Save(CmModeloComponenteDTO entity);
        void Update(CmModeloComponenteDTO entity);
        void Delete(int modcomcodi);
        CmModeloComponenteDTO GetById(int modcomcodi);
        List<CmModeloComponenteDTO> List();
        List<CmModeloComponenteDTO> GetByCriteria(string modembcodi, string modcomcodis);
    }
}
