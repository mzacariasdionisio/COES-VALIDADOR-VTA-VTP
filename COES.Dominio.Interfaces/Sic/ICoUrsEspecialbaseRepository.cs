using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_URS_ESPECIALBASE
    /// </summary>
    public interface ICoUrsEspecialbaseRepository
    {
        int Save(CoUrsEspecialbaseDTO entity);
        void Update(CoUrsEspecialbaseDTO entity);
        void Delete(int couebacodi);
        CoUrsEspecialbaseDTO GetById(int couebacodi);
        List<CoUrsEspecialbaseDTO> List();
        List<CoUrsEspecialbaseDTO> GetByCriteria();
    }
}
