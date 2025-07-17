using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_REVISION
    /// </summary>
    public interface IRerRevisionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        int Save(RerRevisionDTO entity);
        void Save(RerRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateEstado(int rerrevcodi, string rerrevestado, string rerrevusumodificacion, IDbConnection conn, DbTransaction tran);
        RerRevisionDTO GetById(int rerrevcodi);
        List<RerRevisionDTO> GetByCriteria(int ipericodi);
        List<RerRevisionDTO> ListPeriodosConUltimaRevision(int idPlazoEntregaEdi);
        int GetCantidadRevisionesTipoRevision(int ipericodi);
    }
}
