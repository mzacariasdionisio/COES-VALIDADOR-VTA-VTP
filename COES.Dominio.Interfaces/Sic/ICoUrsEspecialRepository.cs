using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_URS_ESPECIAL
    /// </summary>
    public interface ICoUrsEspecialRepository
    {
        int Save(CoUrsEspecialDTO entity);
        void Update(CoUrsEspecialDTO entity);
        void Delete(int courescodi);
        CoUrsEspecialDTO GetById(int courescodi);
        List<CoUrsEspecialDTO> List();
        List<CoUrsEspecialDTO> GetByCriteria(int idVersion);
    }
}
