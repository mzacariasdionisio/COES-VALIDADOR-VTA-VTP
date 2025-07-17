using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ITEMCFG
    /// </summary>
    public interface IFtExtItemcfgRepository
    {
        int Save(FtExtItemcfgDTO entity);
        int Save(FtExtItemcfgDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtItemcfgDTO entity);
        void Update(FtExtItemcfgDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int fitcfgcodi);
        void Delete(int fitcfgcodi, IDbConnection connection, IDbTransaction transaction);
        void DeletePorFormato(int ftfmtcodi);
        FtExtItemcfgDTO GetById(int fitcfgcodi);
        List<FtExtItemcfgDTO> List();
        List<FtExtItemcfgDTO> GetByCriteria();
        List<FtExtItemcfgDTO> ListarPorFormato(int ftfmtcodi);
        List<FtExtItemcfgDTO> ListarPorIds(string fitcfgcodis);
    }
}
