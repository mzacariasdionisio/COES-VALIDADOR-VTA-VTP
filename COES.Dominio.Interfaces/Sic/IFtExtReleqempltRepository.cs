using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_RELEQEMPLT
    /// </summary>
    public interface IFtExtReleqempltRepository
    {
        int Save(FtExtReleqempltDTO entity);
        void Update(FtExtReleqempltDTO entity);
        void Delete(int ftreqecodi);
        FtExtReleqempltDTO GetById(int ftreqecodi);
        List<FtExtReleqempltDTO> List();
        List<FtExtReleqempltDTO> GetByCriteria();
        List<FtExtReleqempltDTO> ListarPorEquipo(string strEquicodis);
        int Save(FtExtReleqempltDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtReleqempltDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
