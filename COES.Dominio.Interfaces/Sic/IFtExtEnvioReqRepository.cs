using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_REQ
    /// </summary>
    public interface IFtExtEnvioReqRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioReqDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioReqDTO entity);
        void Delete(int ftereqcodi);
        FtExtEnvioReqDTO GetById(int ftereqcodi);
        List<FtExtEnvioReqDTO> List();
        List<FtExtEnvioReqDTO> GetByCriteria();
        List<FtExtEnvioReqDTO> GetListByVersiones(string ftevercodis);
    }
}
