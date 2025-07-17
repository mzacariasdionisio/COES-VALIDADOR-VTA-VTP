using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CCC_VERSION
    /// </summary>
    public interface ICccVersionRepository
    {
        int Save(CccVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CccVersionDTO entity);
        void Delete(int cccvercodi);
        CccVersionDTO GetById(int cccvercodi);
        List<CccVersionDTO> List();
        List<CccVersionDTO> GetByCriteria(DateTime fecha, DateTime fechaFin, string horizonte);
    }
}
