using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_EVALUACION_ENERGIAUNIDAD
    /// </summary>
    public interface IRerEvaluacionEnergiaUnidadRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerEvaluacionEnergiaUnidadDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerEvaluacionEnergiaUnidadDTO entity);
        void Delete(int rerEeuCodi);
        RerEvaluacionEnergiaUnidadDTO GetById(int rereeucodi);
        List<RerEvaluacionEnergiaUnidadDTO> List();
        List<RerEvaluacionEnergiaUnidadDTO> GetByCriteria(int reresecodi, int rerevacodi);
    }
}
