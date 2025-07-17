using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerCalculoMensualRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerCalculoMensualDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerCalculoMensualDTO entity);
        void Delete(int rercmcodi);
        void DeleteByAnioVersion(int reravcodi, IDbConnection conn, DbTransaction tran);
        RerCalculoMensualDTO GetById(int rercmcodi);
        List<RerCalculoMensualDTO> List();
        List<RerCalculoMensualDTO> GetByAnioTarifario(int reravcodi);
    }
}

