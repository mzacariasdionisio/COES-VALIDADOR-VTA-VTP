using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_SOLICITUDEDI
    /// </summary>
    public interface IRerSolicitudEdiRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void LogicalDelete(int rersedcodi, string rersedusumodificacion);
        RerSolicitudEdiDTO GetById(int rersedcodi);
        RerSolicitudEdiDTO GetByIdView(int rersedcodi);
        List<RerSolicitudEdiDTO> List();
        List<RerSolicitudEdiDTO> GetByCriteria(int emprcodi, int ipericodi);
        List<RerSolicitudEdiDTO> ListByPeriodo(int ipericodi);
    }

}
