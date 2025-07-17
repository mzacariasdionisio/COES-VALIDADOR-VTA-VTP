using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_REVISION
    /// </summary>
    public interface IFtExtEnvioRevisionRepository
    {
        int GetMaxId();
        int SaveTransaccional(FtExtEnvioRevisionDTO entity, IDbConnection conn, DbTransaction tran);       
        int Save(FtExtEnvioRevisionDTO entity);
        void Update(FtExtEnvioRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int ftrevcodi);
        FtExtEnvioRevisionDTO GetById(int ftrevcodi);
        List<FtExtEnvioRevisionDTO> List();
        List<FtExtEnvioRevisionDTO> GetByCriteria();
        List<FtExtEnvioRevisionDTO> ListByVersionYReq(int ftevercodi);
        List<FtExtEnvioRevisionDTO> ListByVersionYEq(int ftevercodi);
        List<FtExtEnvioRevisionDTO> ListByVersionYDato(int ftevercodi);


    }
}
