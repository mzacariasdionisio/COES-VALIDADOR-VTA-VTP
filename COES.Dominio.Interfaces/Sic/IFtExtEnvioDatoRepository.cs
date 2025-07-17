using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ENVIO_DATO
    /// </summary>
    public interface IFtExtEnvioDatoRepository
    {
        int GetMaxId();
        int Save(FtExtEnvioDatoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(FtExtEnvioDatoDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateXVersion(int ftevercodi, IDbConnection conn, DbTransaction tran);
        void UpdateXEquipo(int fteeqcodi, IDbConnection conn, DbTransaction tran);
        void UpdateXFtedatcodis(string ftedatcodis, IDbConnection conn, DbTransaction tran);
        void Delete(int ftedatcodi);
        FtExtEnvioDatoDTO GetById(int ftedatcodi);
        List<FtExtEnvioDatoDTO> List();
        List<FtExtEnvioDatoDTO> GetByCriteria(string fteeqcodis);

        List<FtExtEnvioDatoDTO> ListarParametros(string strFteeqcodis);
    }
}
