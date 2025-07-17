using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_COMBUSTIBLE
    /// </summary>
    public interface IPrCombustibleRepository
    {
        int Save(CombustibleDTO entity);
        void Update(CombustibleDTO entity);
        void Delete(int combcodi);
        CombustibleDTO GetById(int combcodi);
        List<CombustibleDTO> List();
        List<CombustibleDTO> GetByCriteria();
    }
}
