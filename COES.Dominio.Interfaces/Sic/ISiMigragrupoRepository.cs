using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAGRUPO
    /// </summary>
    public interface ISiMigragrupoRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(SiMigragrupoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigragrupoDTO entity);
        void Delete(int miggrucodi);
        SiMigragrupoDTO GetById(int miggrucodi);
        List<SiMigragrupoDTO> List();
        List<SiMigragrupoDTO> GetByCriteria();
    }
}
