using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_FACTOR_REL_MMAYOR_ARCHIVO
    /// </summary>
    public interface IInFactorRelMmayorArchivoRepository
    {
        int GetMaxId();
        int Save(InFactorRelMmayorArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InFactorRelMmayorArchivoDTO entity);
        void Delete(int irmarcodi);
        InFactorRelMmayorArchivoDTO GetById(int irmarcodi);
        List<InFactorRelMmayorArchivoDTO> List();
        List<InFactorRelMmayorArchivoDTO> GetByCriteria();
    }
}
