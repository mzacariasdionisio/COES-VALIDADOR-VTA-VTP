using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYBASE
    /// </summary>
    public interface ISiMigraquerybaseRepository
    {
        int Save(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int miqubacodi);
        SiMigraquerybaseDTO GetById(int miqubacodi);
        List<SiMigraquerybaseDTO> List();
        List<SiMigraquerybaseDTO> GetByCriteria();
        List<SiMigraquerybaseDTO> ListarSiQueryXMigraQueryTipo(int idTipoOperacionMigracion);

    }
}
