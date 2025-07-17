using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPPTO_DATA
    /// </summary>
    public interface ISiHisempptoDataRepository
    {
        int GetMaxId();
        int Save(SiHisempptoDataDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempptoDataDTO entity);
        void Delete(int hptdatcodi);
        void Delete_UpdateAuditoria(int hptdatcodi, string username);
        SiHisempptoDataDTO GetById(int hptdatcodi);
        List<SiHisempptoDataDTO> List(string ptomedicodis);
        List<SiHisempptoDataDTO> GetByCriteria();

        void DeleteXAnulacionMigra(List<int> puntos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran);
        void UpdatePuntoActual(int ptomedicodiactual, int ptomedicodiold, int puntoAnterior, IDbConnection conn, DbTransaction tran);
    }
}
