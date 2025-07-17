using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOARCHIVO
    /// </summary>
    public interface IFtExtEnvioReldatoarchivoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioReldatoarchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioReldatoarchivoDTO entity);
        void Delete(int fterdacodi);
        FtExtEnvioReldatoarchivoDTO GetById(int fterdacodi);
        List<FtExtEnvioReldatoarchivoDTO> List();
        List<FtExtEnvioReldatoarchivoDTO> GetByCriteria();
    }
}
