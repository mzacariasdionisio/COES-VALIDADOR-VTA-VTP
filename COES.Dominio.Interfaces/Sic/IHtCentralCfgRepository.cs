using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla HT_CENTRAL_CFG
    /// </summary>
    public interface IHtCentralCfgRepository
    {
        int Save(HtCentralCfgDTO entity);
        void Update(HtCentralCfgDTO entity);
        void Delete(int htcentcodi);
        HtCentralCfgDTO GetById(int htcentcodi);
        List<HtCentralCfgDTO> List();
        List<HtCentralCfgDTO> GetByCriteria();
    }
}
