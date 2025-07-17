using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_COMPARATIVO_CAB
    /// </summary>
    public interface IRerComparativoCabRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        RerComparativoCabDTO GetById(int rerccbcodi);
        void Save(RerComparativoCabDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerComparativoCabDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int rerccbcodi);
        List<RerComparativoCabDTO> List();
        List<RerComparativoCabDTO> GetByCriteria(int rerevacodi);
    }
}
