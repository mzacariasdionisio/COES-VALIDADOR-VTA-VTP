using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ENVIODET
    /// </summary>
    public interface IMeEnviodetRepository
    {
        int Save(MeEnviodetDTO entity);
        void Update(MeEnviodetDTO entity);
        void Delete(int enviocodi, int fdatpkcodi);
        MeEnviodetDTO GetById(int enviocodi);
        List<MeEnviodetDTO> List();
        List<MeEnviodetDTO> GetByCriteria(int enviocodi);

        #region INTERVENCIONES

        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(MeEnviodetDTO entity, IDbConnection conn, DbTransaction tran);
        MeEnviodetDTO GetByInterCodi(int intercodi);  // Envdetfpkcodi
        void EliminarFisicoPorIntervencionId(int fdatpkcodi, IDbConnection conn, DbTransaction tran);
        int ObtenerEnvDetCodi(int IdIntervencion);
        string ObtenerMsgCodi(int id);

        #endregion
    }
}
