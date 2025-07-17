using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRACIONLOG
    /// </summary>
    public interface ISiMigracionlogRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int Save(SiMigracionlogDTO entity);
        void Update(SiMigracionlogDTO entity);
        void Delete(int logmigcodi);
        SiMigracionlogDTO GetById(int logmigcodi);
        List<SiMigracionlogDTO> List();
        List<SiMigracionlogDTO> GetByCriteria();
        void EjecutarQuery(string query, IDbConnection conn, DbTransaction tran);

        int GetMaxId();
        int? CantRegistrosMigraQuery(int migracodi, int mqxtopcodi);
    }
}
