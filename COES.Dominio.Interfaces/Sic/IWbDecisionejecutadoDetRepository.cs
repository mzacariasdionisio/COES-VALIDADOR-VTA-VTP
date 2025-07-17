using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_DECISIONEJECUTADO_DET
    /// </summary>
    public interface IWbDecisionejecutadoDetRepository
    {
        int Save(WbDecisionejecutadoDetDTO entity);
        void Update(WbDecisionejecutadoDetDTO entity);
        void Delete(int dejdetcodi);
        void DeleteItem(int dejdetcodi);
        WbDecisionejecutadoDetDTO GetById(int dejdetcodi);
        List<WbDecisionejecutadoDetDTO> List();
        List<WbDecisionejecutadoDetDTO> GetByCriteria(int desejecodi);
        void ActualizarDescripcion(int id, string descripcion);
    }
}

