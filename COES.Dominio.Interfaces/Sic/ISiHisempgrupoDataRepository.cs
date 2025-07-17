using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPGRUPO_DATA
    /// </summary>
    public interface ISiHisempgrupoDataRepository
    {
        int GetMaxId();
        int Save(SiHisempgrupoDataDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempgrupoDataDTO entity);
        void Delete(int hgrdatcodi);
        void Delete_UpdateAuditoria(int hgrdatcodi, string username);
        SiHisempgrupoDataDTO GetById(int hgrdatcodi);
        List<SiHisempgrupoDataDTO> List(string grupocodis);
        List<SiHisempgrupoDataDTO> GetByCriteria();

        void DeleteXAnulacionMigra(List<int> grupos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran);
        void UpdateGrupoActual(int grupocodiactual, int grupocodiold, int grupoAnterior, IDbConnection conn, DbTransaction tran);
    }
}
