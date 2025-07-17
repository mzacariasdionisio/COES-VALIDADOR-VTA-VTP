using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_CIRCUITO
    /// </summary>
    public interface IEqCircuitoRepository
    {
        int Save(EqCircuitoDTO entity);
        void Update(EqCircuitoDTO entity);
        void Delete(int circodi);
        void Delete_UpdateAuditoria(int circodi, string username);
        EqCircuitoDTO GetById(int circodi);
        List<EqCircuitoDTO> List();
        List<EqCircuitoDTO> GetByCriteria(string emprcodi, string listaEquicodi, int estado);

        EqCircuitoDTO GetByEquicodi(int equicodi);
        List<EqCircuitoDTO> ObtenerListaCircuitos(string circodis);

        
    }
    
}
