using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_LOGMIGRA
    /// </summary>
    public interface ISiLogmigraRepository
    {
        //Modified
        void Save(SiLogmigraDTO entity, IDbConnection cnn, DbTransaction tran);

        void Update(SiLogmigraDTO entity);
        void Delete(int migracodi, int logcodi);
        SiLogmigraDTO GetById(int migracodi, int logcodi);
        List<SiLogmigraDTO> List();
        List<SiLogmigraDTO> GetByCriteria();
    }
}
