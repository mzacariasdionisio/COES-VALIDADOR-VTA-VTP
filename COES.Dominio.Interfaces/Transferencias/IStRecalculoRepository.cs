using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_RECALCULO
    /// </summary>
    public interface IStRecalculoRepository
    {
        int Save(StRecalculoDTO entity);
        void Update(StRecalculoDTO entity);
        void Delete(int strecacodi);
        StRecalculoDTO GetById(int strecacodi);
        List<StRecalculoDTO> ListByStPericodi(int stpericodi);
        List<StRecalculoDTO> List(int id);
        List<StRecalculoDTO> GetByCriteria();
        StRecalculoDTO GetByIdView(int stpericodi, int strecacodi);
    }
}
