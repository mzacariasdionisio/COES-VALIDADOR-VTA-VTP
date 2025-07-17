using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPGRUPO
    /// </summary>
    public interface ISiHisempgrupoRepository
    {
        int GetMaxId();
        int Save(SiHisempgrupoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempgrupoDTO entity);
        void DeleteLogico(SiHisempgrupoDTO entity);
        void Delete(int hempgrcodi);
        SiHisempgrupoDTO GetById(int hempgrcodi);
        List<SiHisempgrupoDTO> List();
        List<SiHisempgrupoDTO> GetByCriteria();

        void UpdateAnularTransf(int migracodi, IDbConnection conn, DbTransaction tran);
        List<SiHisempgrupoDTO> ListGrupsXMigracion(int migracodi);
        int ConsultarGrpsMigracion(int migracodi, int grupocodi, DateTime fechacorte);
    }
}
