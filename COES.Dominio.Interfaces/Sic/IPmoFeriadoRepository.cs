using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_FERIADO
    /// </summary>
    public interface IPmoFeriadoRepository
    {
        int Save(PmoFeriadoDTO entity);
        void Update(PmoFeriadoDTO entity);
        void Delete(int pmfrdocodi);
        PmoFeriadoDTO GetById(int pmfrdocodi);
        List<PmoFeriadoDTO> List();
        List<PmoFeriadoDTO> GetByCriteria(int pmanopcodi);
        int Save(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateBajaFeriados(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateAprobar(PmoFeriadoDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
