using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELREQREVAREA
    /// </summary>
    public interface IFtExtEnvioRelreqrevareaRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioRelreqrevareaDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(FtExtEnvioRelreqrevareaDTO entity);
        void Update(FtExtEnvioRelreqrevareaDTO entity);
        void Delete(int revarqcodi);
        void DeletePorIds(string revaacodis, IDbConnection conn, DbTransaction tran);
        FtExtEnvioRelreqrevareaDTO GetById(int revarqcodi);
        List<FtExtEnvioRelreqrevareaDTO> List();
        List<FtExtEnvioRelreqrevareaDTO> GetByCriteria();
        List<FtExtEnvioRelreqrevareaDTO> ListarRelacionesPorVersionArea(int ftevercodi, int faremcodi);
    }
}
