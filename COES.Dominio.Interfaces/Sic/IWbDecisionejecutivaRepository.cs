using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_DECISIONEJECUTIVA
    /// </summary>
    public interface IWbDecisionejecutivaRepository
    {
        int Save(WbDecisionejecutivaDTO entity);
        void Update(WbDecisionejecutivaDTO entity);
        void Delete(int desejecodi);
        WbDecisionejecutivaDTO GetById(int desejecodi);
        List<WbDecisionejecutivaDTO> List();
        List<WbDecisionejecutivaDTO> GetByCriteria(string tipo);
    }
}
