using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_CIRCUITO_DET
    /// </summary>
    public interface IEqCircuitoDetRepository
    {
        int Save(EqCircuitoDetDTO entity);
        void Update(EqCircuitoDetDTO entity);
        void Delete(int circdtcodi);
        EqCircuitoDetDTO GetById(int circdtcodi);
        List<EqCircuitoDetDTO> List();
        List<EqCircuitoDetDTO> GetByCriteria(int circodi, string listaEquicodi, int estado);

        #region MCP
        List<EqCircuitoDetDTO> ObtenerListaCircuitosDet(string circodis);

        List<EqCircuitoDetDTO> GetDependientesAgrupados(int equicodi); 

        #endregion
    }
}
