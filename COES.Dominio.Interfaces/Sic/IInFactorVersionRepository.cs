using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_FACTOR_VERSION
    /// </summary>
    public interface IInFactorVersionRepository
    {
        int GetMaxId();
        int Save(InFactorVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InFactorVersionDTO entity);
        void UpdateByFecha(DateTime fechaPeriodo, int modulo, int horizonte, IDbConnection conn, DbTransaction tran);
        void Delete(int infvercodi);
        InFactorVersionDTO GetById(int infvercodi);
        List<InFactorVersionDTO> List();
        List<InFactorVersionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        List<InFactorVersionDTO> GetByFecha(DateTime fechaPeriodo, int modulo);
    }
}
