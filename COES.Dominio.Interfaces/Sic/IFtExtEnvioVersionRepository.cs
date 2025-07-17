using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_VERSION
    /// </summary>
    public interface IFtExtEnvioVersionRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateListaVersion(int ftenvcodiOrigen, int ftenvcodiDestino, IDbConnection conn, DbTransaction tran);
        void Delete(int ftevercodi);
        FtExtEnvioVersionDTO GetById(int ftevercodi);
        List<FtExtEnvioVersionDTO> List();
        List<FtExtEnvioVersionDTO> GetByCriteria(string ftenvcodis, string tipos);
    }
}
