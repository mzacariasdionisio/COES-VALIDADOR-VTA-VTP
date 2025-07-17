using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELREQARCHIVO
    /// </summary>
    public interface IFtExtEnvioRelreqarchivoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioRelreqarchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioRelreqarchivoDTO entity);
        void Delete(int fterracodi);
        FtExtEnvioRelreqarchivoDTO GetById(int fterracodi);
        List<FtExtEnvioRelreqarchivoDTO> List();
        List<FtExtEnvioRelreqarchivoDTO> GetByCriteria();
        List<FtExtEnvioRelreqarchivoDTO> GetByRequisitos(string ftereqcodis);
    }
}
