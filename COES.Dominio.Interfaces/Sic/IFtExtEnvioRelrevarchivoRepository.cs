using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELREVARCHIVO
    /// </summary>
    public interface IFtExtEnvioRelrevarchivoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioRelrevarchivoDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(FtExtEnvioRelrevarchivoDTO entity);
        void Update(FtExtEnvioRelrevarchivoDTO entity);
        void Delete(int ftrrvacodi);
        FtExtEnvioRelrevarchivoDTO GetById(int ftrrvacodi);
        List<FtExtEnvioRelrevarchivoDTO> List();
        List<FtExtEnvioRelrevarchivoDTO> GetByCriteria();
        List<FtExtEnvioRelrevarchivoDTO> GetByRevision(string strFtrevcodis);
    }
}
