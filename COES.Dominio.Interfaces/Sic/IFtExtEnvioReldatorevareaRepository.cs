using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELDATOREVAREA
    /// </summary>
    public interface IFtExtEnvioReldatorevareaRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioReldatorevareaDTO entity);
        int Save(FtExtEnvioReldatorevareaDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioReldatorevareaDTO entity);
        void Delete(int revadcodi);
        void DeletePorGrupo(string revadcodis);
        void DeletePorIds(string revadcodis, IDbConnection conn, DbTransaction tran);
        List<FtExtEnvioReldatorevareaDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi);
        FtExtEnvioReldatorevareaDTO GetById(int revadcodi);
        List<FtExtEnvioReldatorevareaDTO> List();
        List<FtExtEnvioReldatorevareaDTO> GetByCriteria();
    }
}
