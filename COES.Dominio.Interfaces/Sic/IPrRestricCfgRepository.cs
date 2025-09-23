using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_CFG
    /// </summary>
    public interface IPrRestricCfgRepository
    {
        int Save(PrRestricCfgDTO entity);
        void Update(PrRestricCfgDTO entity);
        void Delete(int rescfgcodi);
        PrRestricCfgDTO GetById(int rescfgcodi);
        List<PrRestricCfgDTO> List();
        List<PrRestricCfgDTO> GetByCriteria(string tipos);

        void ActualizarEstadoRegistro(PrRestricCfgDTO entity);
    }
}
