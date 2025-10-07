using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_CFGDET
    /// </summary>
    public interface IPrRestricCfgdetRepository
    {
        int Save(PrRestricCfgdetDTO entity);
        void Update(PrRestricCfgdetDTO entity);
        void Delete(int resdetcodi);
        PrRestricCfgdetDTO GetById(int resdetcodi);
        List<PrRestricCfgdetDTO> List();
        List<PrRestricCfgdetDTO> GetByCriteria(string codigos);
    }
}
