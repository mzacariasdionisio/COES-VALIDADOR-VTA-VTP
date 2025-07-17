using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_EVENTO
    /// </summary>
    public interface IAfEventoRepository
    {
        int Save(AfEventoDTO entity);
        void Update(AfEventoDTO entity);
        void Delete(int afecodi);
        AfEventoDTO GetById(int afecodi);
        List<AfEventoDTO> List();
        List<AfEventoDTO> GetByCriteria();
    }
}
