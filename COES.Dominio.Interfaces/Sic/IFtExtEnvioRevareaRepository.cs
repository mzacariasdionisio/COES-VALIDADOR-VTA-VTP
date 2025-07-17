using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_REVAREA
    /// </summary>
    public interface IFtExtEnvioRevareaRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioRevareaDTO entity);
        int Save(FtExtEnvioRevareaDTO entity, IDbConnection conn, DbTransaction tran);

        void Update(FtExtEnvioRevareaDTO entity);
        void Delete(int revacodi);
        void DeletePorGrupo(string revacodis);
        void DeletePorIds(string revacodis, IDbConnection conn, DbTransaction tran);
        FtExtEnvioRevareaDTO GetById(int revacodi);
        List<FtExtEnvioRevareaDTO> List();
        List<FtExtEnvioRevareaDTO> GetByCriteria();
        
        List<FtExtEnvioRevareaDTO> ListByVersionYDato(int ftevercodi);
        List<FtExtEnvioRevareaDTO> ListarRevisionPorAreaVersionYReq(string faremcodi, int ftevercodi, string ftereqcodis);
        List<FtExtEnvioRevareaDTO> ListarRevisionPorAreaVersionYDatos(string faremcodi, int ftevercodi, string ftedatcodis);
        List<FtExtEnvioRevareaDTO> ListarRelacionesPorVersionAreaYEquipo(int ftevercodi, int faremcodi, int fteeqcodi);
        List<FtExtEnvioRevareaDTO> ListarRelacionesContenidoPorVersionArea(int ftevercodi, int faremcodi);
        
    }
}
