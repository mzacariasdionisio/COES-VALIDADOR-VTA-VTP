using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYPLANTPARAM
    /// </summary>
    public interface ISiMigraqueryplantparamRepository
    {
        int GetMaxId();
        int Save(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int miplprcodi);
        SiMigraqueryplantparamDTO GetById(int miplprcodi);
        List<SiMigraqueryplantparamDTO> List();
        List<SiMigraqueryplantparamDTO> GetByCriteria(int miqplacodi);
    }
}
