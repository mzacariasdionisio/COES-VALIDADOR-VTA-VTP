using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_DISTELECTRICA_GENELE
    /// </summary>
    public interface IStDistelectricaGeneleRepository
    {
        int Save(StDistelectricaGeneleDTO entity);
        void Update(StDistelectricaGeneleDTO entity);
        void Delete(int strecacodi);
        StDistelectricaGeneleDTO GetById(int degelecodi);
        List<StDistelectricaGeneleDTO> List();
        List<StDistelectricaGeneleDTO> GetByCriteria(int strecacodi, int Stcompcodi);
        List<StDistelectricaGeneleDTO> GetByIdCriteriaStDistGeneReporte(int strecacodi);
        StDistelectricaGeneleDTO GetByIdCriteriaStDistGene(int stcntgcodi, int stcompcodi);
        
    }
}
