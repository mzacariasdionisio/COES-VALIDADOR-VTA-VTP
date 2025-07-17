using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_COMPARATIVO_DET
    /// </summary>
    public interface IRerComparativoDetRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerComparativoDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RerComparativoDetDTO entity);
        void Delete(int rercdtcodi);
        void DeleteByRerccbcodi(int rerccbcodi, IDbConnection conn, DbTransaction tran);
        RerComparativoDetDTO GetById(int rercdtcodi);
        List<RerComparativoDetDTO> List();
        List<RerComparativoDetDTO> GetByCriteria(int rerevacodi, int reresecodi, int rereeucodi);
        List<RerComparativoDetDTO> GetEEDRByCriteria(int rerevacodi, int reresecodi, int rereeucodi);
        List<RerComparativoDetDTO> GetComparativoAprobadaValidadByMes(DateTime dRerCenFecha);

    }
}
