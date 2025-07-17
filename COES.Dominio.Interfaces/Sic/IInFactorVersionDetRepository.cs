using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_FACTOR_VERSION_DET
    /// </summary>
    public interface IInFactorVersionDetRepository
    {
        int GetMaxId();
        int Save(InFactorVersionDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InFactorVersionDetDTO entity);
        void Delete(int infvdtcodi);
        InFactorVersionDetDTO GetById(int infvdtcodi);
        List<InFactorVersionDetDTO> List();
        List<InFactorVersionDetDTO> ListxInfvercodi(int infvercodi);
        List<InFactorVersionDetDTO> GetByCriteria();
    }
}
