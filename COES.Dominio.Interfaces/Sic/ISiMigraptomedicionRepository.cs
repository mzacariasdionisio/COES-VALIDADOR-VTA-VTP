using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAPTOMEDICION
    /// </summary>
    public interface ISiMigraptomedicionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(SiMigraPtomedicionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraPtomedicionDTO entity);
        void Delete(int mgpmedcodi);
        SiMigraPtomedicionDTO GetById(int mgpmedcodi);
        List<SiMigraPtomedicionDTO> List();
        List<SiMigraPtomedicionDTO> GetByCriteria();
    }
}
