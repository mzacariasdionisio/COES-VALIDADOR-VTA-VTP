using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPEQ_DATA
    /// </summary>
    public interface ISiHisempeqDataRepository
    {
        int GetMaxId();
        int Save(SiHisempeqDataDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempeqDataDTO entity);
        void Delete(int heqdatcodi);
        void Delete_UpdateAuditoria(int heqdatcodi, string username);
        SiHisempeqDataDTO GetById(int heqdatcodi);
        List<SiHisempeqDataDTO> List(string equicodis);
        List<SiHisempeqDataDTO> GetByCriteria();
        void DeleteXAnulacionMigra(List<int> equipos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran);

        void UpdateEquipoActual(int equicodiactual, int equicodiold, int equipoAnterior, IDbConnection conn, DbTransaction tran);
    }
}
