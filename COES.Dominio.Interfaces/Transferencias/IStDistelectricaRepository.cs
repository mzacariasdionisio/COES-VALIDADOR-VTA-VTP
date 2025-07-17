using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_DISTELECTRICA
    /// </summary>
    public interface IStDistelectricaRepository
    {
        int Save(StDistelectricaDTO entity);
        void Update(StDistelectricaDTO entity);
        void Delete(int strecacodi);
        StDistelectricaDTO GetById(int dstelecodi);
        List<StDistelectricaDTO> List();
        List<StDistelectricaDTO> GetByCriteria(int strecacodi);
        List<StDistelectricaDTO> GetByCriteriaVersion(int strecacodi);
    }
}
