using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_AREA
    /// </summary>
    public interface IFtExtEnvioAreaRepository
    {
        int Save(FtExtEnvioAreaDTO entity);
        int Save(FtExtEnvioAreaDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioAreaDTO entity);
        void Delete(int envarcodi);
        FtExtEnvioAreaDTO GetById(int envarcodi);
        FtExtEnvioAreaDTO GetByVersionYArea(int ftevercodi, int faremcodi);
        List<FtExtEnvioAreaDTO> List();
        List<FtExtEnvioAreaDTO> GetByCriteria();
        List<FtExtEnvioAreaDTO> ListarPorVersiones(string strVersiones);
        List<FtExtEnvioAreaDTO> ListarPorEnvioCarpetaYEstado(int estenvcodi, string strFtenvcodis, string envarestado);
    }
}
