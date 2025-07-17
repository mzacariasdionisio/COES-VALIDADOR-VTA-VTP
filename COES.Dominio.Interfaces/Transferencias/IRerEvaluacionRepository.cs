using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_EVALUACION
    /// </summary>
    public interface IRerEvaluacionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerEvaluacionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerEvaluacionDTO entity);
        void UpdateEstado(int rerevacodi, string rerevaestado, string rerevausumodificacion, IDbConnection conn, DbTransaction tran);
        void UpdateEstadoAGenerado(int rerrevcodi, string rerevausumodificacion, IDbConnection conn, DbTransaction tran);
        void Delete(int rerevacodi);
        RerEvaluacionDTO GetById(int rerevacodi);
        List<RerEvaluacionDTO> List();
        List<RerEvaluacionDTO> GetByCriteria(int rerrevcodi);
        int GetNextNumVersion(int rerrevcodi);
        RerEvaluacionDTO GetByRevisionAndLastNumVersion(int rerrevcodi);
        List<RerEvaluacionDTO> GetUltimaByEstadoEvaluacionByAnioTarifario(string rerevaestado, int anio);
        int GetCantidadEvaluacionValidado(int rerrevcodi);
    }
}
