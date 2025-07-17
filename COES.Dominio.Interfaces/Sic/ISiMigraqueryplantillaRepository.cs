using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYPLANTILLA
    /// </summary>
    public interface ISiMigraqueryplantillaRepository
    {
        int Save(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int miqplacodi);
        SiMigraqueryplantillaDTO GetById(int miqplacodi);
        List<SiMigraqueryplantillaDTO> List();
        List<SiMigraqueryplantillaDTO> GetByCriteria();
    }
}
