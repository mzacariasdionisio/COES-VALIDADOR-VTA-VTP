using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_ERACMF_ZONA
    /// </summary>
    public interface IAfEracmfZonaRepository
    {
        int Save(AfEracmfZonaDTO entity);
        void Update(AfEracmfZonaDTO entity);
        void Delete(int aferaccodi);
        AfEracmfZonaDTO GetById(int aferaccodi);
        List<AfEracmfZonaDTO> List();
        List<AfEracmfZonaDTO> GetByCriteria();
    }
}
