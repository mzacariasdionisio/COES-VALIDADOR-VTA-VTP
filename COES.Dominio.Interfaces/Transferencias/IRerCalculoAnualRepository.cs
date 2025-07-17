using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerCalculoAnualRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerCalculoAnualDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerCalculoAnualDTO entity);
        void Delete(int rercacodi);
        void DeleteByAnioVersion(int reravcodi, IDbConnection conn, DbTransaction tran);
        RerCalculoAnualDTO GetById(int rercacodi);
        List<RerCalculoAnualDTO> GetByAnnioAndVersion(int reravaniotarif, string reravversion);
        List<RerCalculoAnualDTO> List();
    }
}

