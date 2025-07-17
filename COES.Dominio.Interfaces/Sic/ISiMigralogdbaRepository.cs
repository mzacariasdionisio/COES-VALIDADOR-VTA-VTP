using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRALOGDBA
    /// </summary>
    public interface ISiMigralogdbaRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int Save(SiMigralogdbaDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigralogdbaDTO entity);
        void Delete(int migdbacodi);
        SiMigralogdbaDTO GetById(int migdbacodi);
        List<SiMigralogdbaDTO> List();
        List<SiMigralogdbaDTO> GetByCriteria();

        int SaveTransferencia(SiMigralogdbaDTO entity, IDbConnection conn, DbTransaction tran);
        int GetMaxId();

        List<SiMigralogdbaDTO> ListLogDbaByMigracion(int migracodi);
    }
}
