using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_FACTOR_VERSION_MMAYOR
    /// </summary>
    public interface IInFactorVersionMmayorRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(InFactorVersionMmayorDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InFactorVersionMmayorDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int infmmcodi);
        InFactorVersionMmayorDTO GetById(int infmmcodi);
        List<InFactorVersionMmayorDTO> List();
        List<InFactorVersionMmayorDTO> GetByCriteria(int infvercodi, string infmmhoja);
        List<SiEmpresaDTO> GetEmpresaByID(int infvercodi);
    }
}
