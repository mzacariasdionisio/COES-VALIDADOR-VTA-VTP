using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_CORREOAREA
    /// </summary>
    public interface IFtExtCorreoareaRepository
    {
        int Save(FtExtCorreoareaDTO entity);
        void Update(FtExtCorreoareaDTO entity);
        void Delete(int faremcodi);
        FtExtCorreoareaDTO GetById(int faremcodi);
        List<FtExtCorreoareaDTO> List();
        List<FtExtCorreoareaDTO> GetByCriteria();
        int Save(FtExtCorreoareaDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtCorreoareaDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<FtExtCorreoareaDTO> ListarPorParametros(string estadoRelacion, string strListaFtitcodis);
        List<FtExtCorreoareaDTO> ListarPorRequisitos(string estadoRelacion, string strListaFevrqcodis);
        List<FtExtCorreoareaDTO> ListarPorIds(string strFaremcodis);
    }
}
