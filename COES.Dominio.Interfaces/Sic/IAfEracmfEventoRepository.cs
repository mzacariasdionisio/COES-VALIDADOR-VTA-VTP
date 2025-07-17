using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_ERACMF_EVENTO
    /// </summary>
    public interface IAfEracmfEventoRepository
    {
        int Save(AfEracmfEventoDTO entity);
        void Update(AfEracmfEventoDTO entity);
        void Delete(int eracmfcodi);
        AfEracmfEventoDTO GetById(int eracmfcodi);
        List<AfEracmfEventoDTO> List();
        List<AfEracmfEventoDTO> GetByCriteria();
        List<AfEracmfEventoDTO> GetByEvencodi(int evencodi);
        List<AfEracmfEventoDTO> GetByEvento(int emprcodi, int evencodi);
    }
}
