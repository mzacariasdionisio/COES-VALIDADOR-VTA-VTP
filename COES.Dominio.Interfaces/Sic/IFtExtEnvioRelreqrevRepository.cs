using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELREQREV
    /// </summary>
    public interface IFtExtEnvioRelreqrevRepository
    {
        int GetMaxId();
        int SaveTransaccional(FtExtEnvioRelreqrevDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(FtExtEnvioRelreqrevDTO entity);
        void Update(FtExtEnvioRelreqrevDTO entity);
        void Delete(int frrrevcodi);
        FtExtEnvioRelreqrevDTO GetById(int frrrevcodi);
        List<FtExtEnvioRelreqrevDTO> List();
        List<FtExtEnvioRelreqrevDTO> GetByCriteria();
        List<FtExtEnvioRelreqrevDTO> GetByRequisitos(string ftereqcodis);
    }
}
