using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_EVALUACION_SOLICITUDEDI
    /// </summary>
    public interface IRerEvaluacionSolicitudEdiRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerEvaluacionSolicitudEdiDTO entity);
        void UpdateFields(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateFieldsForResults(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateEnergias(RerEvaluacionSolicitudEdiDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int reresecodi);
        RerEvaluacionSolicitudEdiDTO GetById(int reresecodi);
        List<RerEvaluacionSolicitudEdiDTO> List();
        List<RerEvaluacionSolicitudEdiDTO> GetByCriteria(int rerevacodi, int rersedcodi);
        List<RerEvaluacionSolicitudEdiDTO> GetByEvaluacionByEliminadoByEstado(string rerevacodi, string rereseeliminado, string rereseresestado);
    }
}
