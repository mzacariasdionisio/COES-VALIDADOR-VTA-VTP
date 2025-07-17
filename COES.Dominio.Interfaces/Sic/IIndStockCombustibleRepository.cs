using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_STOCK_COMBUSTIBLE
    /// </summary>
    public interface IIndStockCombustibleRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        IndStockCombustibleDTO GetById(int stkcmtcodi);
        void Save(IndStockCombustibleDTO entity, IDbConnection conn, DbTransaction tran);
        List<IndStockCombustibleDTO> GetByCriteria(int ipericodi, int emprcodi, int equicodicentral, string equicodiunidad, int tipoinfocodi);
        List<IndStockCombustibleDTO> ListStockByAnioMes(int anio, int mes);
    }
}
