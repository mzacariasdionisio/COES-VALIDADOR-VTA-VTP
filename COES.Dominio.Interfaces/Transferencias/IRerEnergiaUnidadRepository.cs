using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_ENERGIAUNIDAD
    /// </summary>
    public interface IRerEnergiaUnidadRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerEnergiaUnidadDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerEnergiaUnidadDTO entity);
        void Delete(int rersedcodi, IDbConnection conn, DbTransaction tran);
        RerEnergiaUnidadDTO GetById(int rereucodi);
        List<RerEnergiaUnidadDTO> List(int rersedcodi);
        List<RerEnergiaUnidadDTO> ListByPeriodo(int ipericodi);
    }
}
