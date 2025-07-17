using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_RELAREAITEMCFG
    /// </summary>
    public interface IFtExtRelareaitemcfgRepository
    {
        int Save(FtExtRelareaitemcfgDTO entity);
        void Update(FtExtRelareaitemcfgDTO entity);
        void Delete(int friacodi);
        FtExtRelareaitemcfgDTO GetById(int friacodi);
        List<FtExtRelareaitemcfgDTO> List();
        List<FtExtRelareaitemcfgDTO> GetByCriteria();
        List<FtExtRelareaitemcfgDTO> ListarPorAreas(string estado, string famercodis);
    }
}
