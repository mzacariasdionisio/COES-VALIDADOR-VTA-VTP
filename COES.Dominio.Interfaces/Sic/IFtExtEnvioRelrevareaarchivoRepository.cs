using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_RELREVAREAARCHIVO
    /// </summary>
    public interface IFtExtEnvioRelrevareaarchivoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioRelrevareaarchivoDTO entity);
        int Save(FtExtEnvioRelrevareaarchivoDTO entity, IDbConnection conn, DbTransaction tran);

        void Update(FtExtEnvioRelrevareaarchivoDTO entity);
        void Delete(int revaacodi);
        void DeletePorGrupo(string revaacodis);
        List<FtExtEnvioRelrevareaarchivoDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi);
        List<FtExtEnvioRelrevareaarchivoDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi);
        void DeletePorIds(string revaacodis, IDbConnection conn, DbTransaction tran);
        
        FtExtEnvioRelrevareaarchivoDTO GetById(int revaacodi);
        List<FtExtEnvioRelrevareaarchivoDTO> List();
        List<FtExtEnvioRelrevareaarchivoDTO> ListarPorVersiones(string ftevercodis);
        List<FtExtEnvioRelrevareaarchivoDTO> GetByCriteria();
    }
}
