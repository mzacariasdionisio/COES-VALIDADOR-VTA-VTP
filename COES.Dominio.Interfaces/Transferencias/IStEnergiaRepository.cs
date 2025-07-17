using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_ENERGIA
    /// </summary>
    public interface IStEnergiaRepository
    {
        int Save(StEnergiaDTO entity);
        void Update(StEnergiaDTO entity);
        void Delete(int stenrgcodi);
        StEnergiaDTO GetById(int stenrgcodi);
        List<StEnergiaDTO> List();
        List<StEnergiaDTO> GetByCriteria(int strecacodi);
        List<StEnergiaDTO> ListByStEnergiaVersion(int strecacodi);
        StEnergiaDTO GetByCentralCodi(int strecacodi, int stcntgcodi);
    }
}
