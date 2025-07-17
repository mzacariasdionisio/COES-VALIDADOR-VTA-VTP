using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_INSUMO_LOG
    /// </summary>
    public interface IIndInsumoLogRepository
    {
        int GetMaxId();
        int Save(IndInsumoLogDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(IndInsumoLogDTO entity);
        void Delete(int ilogcodi);
        IndInsumoLogDTO GetById(int ilogcodi);
        List<IndInsumoLogDTO> List();
        List<IndInsumoLogDTO> GetByCriteria();
    }
}
