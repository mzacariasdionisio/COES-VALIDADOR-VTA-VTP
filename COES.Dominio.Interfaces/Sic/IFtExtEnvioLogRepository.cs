using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_LOG
    /// </summary>
    public interface IFtExtEnvioLogRepository
    {
        int Save(FtExtEnvioLogDTO entity);
        int Save(FtExtEnvioLogDTO entity, IDbConnection conn, DbTransaction tran);               
        void Update(FtExtEnvioLogDTO entity);
        void Delete(int ftelogcodi);
        FtExtEnvioLogDTO GetById(int ftelogcodi);
        List<FtExtEnvioLogDTO> List();
        List<FtExtEnvioLogDTO> GetByCriteria(int ftenvcodi);
        List<FtExtEnvioLogDTO> GetByIdsEnvio(string ftenvcodistring);
        List<FtExtEnvioLogDTO> GetByIdsEnvioRevisionAreas(string ftenvcodis);
        
        List<FtExtEnvioLogDTO> ListarPorEnviosYEstados(string ftenvcodis, string estenvcodis);
        List<FtExtEnvioLogDTO> ListarLogsEnviosAmpliados(string ftenvcodis);
    }
}
