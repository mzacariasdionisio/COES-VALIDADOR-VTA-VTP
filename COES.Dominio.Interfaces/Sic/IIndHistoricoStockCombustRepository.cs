using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_HISTORICO_STOCKCOMBUST
    /// </summary>
    public interface IIndHistoricoStockCombustRepository
    {
        int GetMaxId();
        void Save(IndHistoricoStockCombustDTO entity, IDbConnection conn, DbTransaction tran);
        List<IndHistoricoStockCombustDTO> GetByCriteria(int ipericodi);
    }
}
