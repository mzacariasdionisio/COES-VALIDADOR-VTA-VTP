using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_FORMATO
    /// </summary>
    public interface IFtExtFormatoRepository
    {
        int Save(FtExtFormatoDTO entity);
        int Save(FtExtFormatoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtFormatoDTO entity);
        void Update(FtExtFormatoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int ftfmtcodi);
        FtExtFormatoDTO GetById(int ftfmtcodi);
        FtExtFormatoDTO GetByEtapaYTipoEquipo(int fteqcodi, int ftetcodi);
        List<FtExtFormatoDTO> List();
        List<FtExtFormatoDTO> GetByCriteria();
    }
}
