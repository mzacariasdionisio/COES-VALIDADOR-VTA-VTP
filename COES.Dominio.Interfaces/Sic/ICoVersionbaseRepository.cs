using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_VERSIONBASE
    /// </summary>
    public interface ICoVersionbaseRepository
    {
        int Save(CoVersionbaseDTO entity);
        void Update(CoVersionbaseDTO entity);
        void Delete(int covebacodi);
        CoVersionbaseDTO GetById(int covebacodi);
        List<CoVersionbaseDTO> List();
        List<CoVersionbaseDTO> GetByCriteria();
    }
}
