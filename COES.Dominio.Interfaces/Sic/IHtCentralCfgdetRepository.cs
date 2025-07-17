using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla HT_CENTRAL_CFGDET
    /// </summary>
    public interface IHtCentralCfgdetRepository
    {
        int Save(HtCentralCfgdetDTO entity);
        void Update(HtCentralCfgdetDTO entity);
        void Delete();
        HtCentralCfgdetDTO GetById();
        List<HtCentralCfgdetDTO> List();
        List<HtCentralCfgdetDTO> GetByCriteria(int htcentcodi);
    }
}
