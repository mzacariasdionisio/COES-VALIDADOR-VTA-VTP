using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAQUERYPARAMETRO
    /// </summary>
    public interface ISiMigraqueryparametroRepository
    {
        int GetMaxId();
        int Save(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int mgqparcodi);
        SiMigraqueryparametroDTO GetById(int mgqparcodi);
        List<SiMigraqueryparametroDTO> List();
        List<SiMigraqueryparametroDTO> GetByCriteria(int miqubacodi);
    }
}
