using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYXTIPOOPERACION
    /// </summary>
    public interface ISiMigraqueryxtipooperacionRepository
    {
        int GetMaxId();
        int Save(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int mqxtopcodi);
        SiMigraqueryxtipooperacionDTO GetById(int mqxtopcodi);
        List<SiMigraqueryxtipooperacionDTO> List();
        List<SiMigraqueryxtipooperacionDTO> GetByCriteria(int miqubacodi);

        List<SiMigraqueryxtipooperacionDTO> ListarMqxXTipoMigracion(int idTipoMigraOperacion);
    }
}
