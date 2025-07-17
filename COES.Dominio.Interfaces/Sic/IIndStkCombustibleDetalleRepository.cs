using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_STKCOMBUSTIBLE_DETALLE
    /// </summary>
    public interface IIndStkCombustibleDetalleRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        IndStkCombustibleDetalleDTO GetById(int stkdetcodi);
        void Save(IndStkCombustibleDetalleDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateDays(int stkdetcodi, string setupdates, string stkdetusumodificacion, DateTime stkdetfecmodificacion, IDbConnection conn, DbTransaction tran);
        List<IndStkCombustibleDetalleDTO> GetByCriteria(int stkcmtcodi, string stkdettipo);
        List<IndStkCombustibleDetalleDTO> GetByPeriod(int ipericodi, string emprcodi, string stkdettipo);
    }

}
