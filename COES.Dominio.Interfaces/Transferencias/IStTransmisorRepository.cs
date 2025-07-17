using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_TRANSMISOR
    /// </summary>
    public interface IStTransmisorRepository
    {
        int Save(StTransmisorDTO entity);
        void Update(StTransmisorDTO entity);
        void Delete(int stranscodi);
        void DeleteVersion(int strecacodi);
        StTransmisorDTO GetById(int stranscodi);
        List<StTransmisorDTO> List(int strecacodi);
        List<StTransmisorDTO> ListByStTransmisorVersion(int strecacodi);
        List<StTransmisorDTO> GetByCriteria();
    }
}
