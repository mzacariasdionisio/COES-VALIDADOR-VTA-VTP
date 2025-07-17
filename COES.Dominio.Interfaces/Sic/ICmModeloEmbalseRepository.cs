using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_MODELO_EMBALSE
    /// </summary>
    public interface ICmModeloEmbalseRepository
    {
        int Save(CmModeloEmbalseDTO entity);
        void Update(CmModeloEmbalseDTO entity);
        void Delete(CmModeloEmbalseDTO entity);
        CmModeloEmbalseDTO GetById(int modembcodi);
        List<CmModeloEmbalseDTO> List();
        List<CmModeloEmbalseDTO> GetByCriteria(string estado, string recurcodis);
        List<CmModeloEmbalseDTO> ListHistorialXRecurso(int recurcodi);
    }
}
