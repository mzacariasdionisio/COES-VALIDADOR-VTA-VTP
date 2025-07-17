using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYPLANTTOP
    /// </summary>
    public interface ISiMigraqueryplanttopRepository
    {
        int GetMaxId();
        int Save(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int miptopcodi);
        SiMigraqueryplanttopDTO GetById(int miptopcodi);
        List<SiMigraqueryplanttopDTO> List();
        List<SiMigraqueryplanttopDTO> GetByCriteria(int miqplacodi);
    }
}
