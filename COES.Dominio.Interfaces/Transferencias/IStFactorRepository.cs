using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_FACTOR
    /// </summary>
    public interface IStFactorRepository
    {
        int Save(StFactorDTO entity);
        void Update(StFactorDTO entity);
        void Delete(int strecacodi);
        void DeleteVersion(int strecacodi);
        StFactorDTO GetById(int stfactcodi);
        List<StFactorDTO> List();
        List<StFactorDTO> GetByCriteria(int strecacodi);
        List<StFactorDTO> ListByStFactorVersion(int strecacodi);
        StFactorDTO GetBySisTrans(int sistrncodi);
    }
}
