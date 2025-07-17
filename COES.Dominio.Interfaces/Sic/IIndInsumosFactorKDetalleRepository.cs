using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_INSUMOS_FACTORK_DETALLE
    /// </summary>
    public interface IIndInsumosFactorKDetalleRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        IndInsumosFactorKDetalleDTO GetById(int infkdtcodi);
        IndInsumosFactorKDetalleDTO GetByCriteria(int insfckcodi, int infkdttipo);
        void Save(IndInsumosFactorKDetalleDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateDays(int infkdtcodi, string setupdates, string infkdtusumodificacion, DateTime infkdtfecmodificacion, IDbConnection conn, DbTransaction tran);
        void DeleteByCriteria(string insfckcodi, string infkdttipo, IDbConnection conn, DbTransaction tran);
        List<IndInsumosFactorKDetalleDTO> GetByInsumosFactorK(int ipericodi, int emprcodi, int equicodicentral, int equicodiunidad, int grupocodi, int famcodi);
        List<IndInsumosFactorKDetalleDTO> GetByPeriodo(int ipericodi);
    }
}
