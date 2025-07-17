using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOREV
    /// </summary>
    public interface IFtExtEnvioReldatorevRepository
    {
        int GetMaxId();
        int SaveTransaccional(FtExtEnvioReldatorevDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(FtExtEnvioReldatorevDTO entity);
        void Update(FtExtEnvioReldatorevDTO entity);
        void Delete(int frdrevcodi);
        FtExtEnvioReldatorevDTO GetById(int frdrevcodi);
        List<FtExtEnvioReldatorevDTO> List();
        List<FtExtEnvioReldatorevDTO> GetByCriteria();
        List<FtExtEnvioReldatorevDTO> GetByDatos(string ftedatcodis);
    }
}
