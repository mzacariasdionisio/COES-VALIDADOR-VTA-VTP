using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_NIVEL_TENSION
    /// </summary>
    public interface IReNivelTensionRepository
    {
        int Save(ReNivelTensionDTO entity);
        void Update(ReNivelTensionDTO entity);
        void Delete(int rentcodi);
        ReNivelTensionDTO GetById(int rentcodi);
        List<ReNivelTensionDTO> List();
        List<ReNivelTensionDTO> GetByCriteria();
    }
}
