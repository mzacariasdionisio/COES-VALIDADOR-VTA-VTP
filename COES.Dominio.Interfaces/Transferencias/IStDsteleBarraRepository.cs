using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_DSTELE_BARRA
    /// </summary>
    public interface IStDsteleBarraRepository
    {
        void Save(StDsteleBarraDTO entity);
        void Update(StDsteleBarraDTO entity);
        void Delete(int strecacodi);
        StDsteleBarraDTO GetById(int dstelecodi, int barrcodi);
        List<StDsteleBarraDTO> List();
        List<StDsteleBarraDTO> GetByCriteria();
        List<StDsteleBarraDTO> ListStDistEleBarrPorID(int id);
        StDsteleBarraDTO GetByCriterios(int strecacodi, int barr1, int barr2);
    }
}
