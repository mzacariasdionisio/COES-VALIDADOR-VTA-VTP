using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_EQ
    /// </summary>
    public interface IFtExtEnvioEqRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioEqDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioEqDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateEstado(string fteeqcodis, string estado, IDbConnection conn, DbTransaction tran);
        void Delete(int fteeqcodi);
        FtExtEnvioEqDTO GetById(int fteeqcodi);
        List<FtExtEnvioEqDTO> List();
        List<FtExtEnvioEqDTO> GetByCriteria(string ftevercodis, string estado);
        List<FtExtEnvioEqDTO> ListarPorEnvios(string strIdsEnvios);
        List<FtExtEnvioEqDTO> ListarPorIds(string fteeqcodis);
        List<FtExtEnvioEqDTO> GetByVersionYModificacion(string ftevercodis, int flagModificacion);
        int GetTotalXFormatoExtranet(int ftfmtcodi);
    }
}
