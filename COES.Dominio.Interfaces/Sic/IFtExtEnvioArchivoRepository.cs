using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_ARCHIVO
    /// </summary>
    public interface IFtExtEnvioArchivoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int ftearccodi);
        void DeletePorIds(string ftearccodis, IDbConnection conn, DbTransaction tran);
        FtExtEnvioArchivoDTO GetById(int ftearccodi);
        List<FtExtEnvioArchivoDTO> List();
        List<FtExtEnvioArchivoDTO> ListByVersionYReq(int ftevercodi);
        List<FtExtEnvioArchivoDTO> ListByVersionYEq(int ftevercodi);
        List<FtExtEnvioArchivoDTO> ListByVersionYDato(int ftevercodi);
        List<FtExtEnvioArchivoDTO> GetByCriteria();
        List<FtExtEnvioArchivoDTO> ListarPorIds(string ftearccodis);
        List<FtExtEnvioArchivoDTO> ListByRevision(string ftrevcodis);
        List<FtExtEnvioArchivoDTO> ListByRevisionAreas(string revacodis);
        List<FtExtEnvioArchivoDTO> ListByVersionAreas(string ftvercodis);
        List<FtExtEnvioArchivoDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi);
        List<FtExtEnvioArchivoDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi);
        
    }
}
