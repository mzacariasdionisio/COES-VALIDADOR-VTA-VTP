using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELEEQREV
    /// </summary>
    public interface IFtExtEnvioReleeqrevRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioReleeqrevDTO entity);
        int SaveTransaccional(FtExtEnvioReleeqrevDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioReleeqrevDTO entity);
        void Delete(int freqrvcodi);
        FtExtEnvioReleeqrevDTO GetById(int freqrvcodi);
        List<FtExtEnvioReleeqrevDTO> List();
        List<FtExtEnvioReleeqrevDTO> GetByCriteria();
        List<FtExtEnvioReleeqrevDTO> GetByEquipos(string fteeqcodis);
    }
}
