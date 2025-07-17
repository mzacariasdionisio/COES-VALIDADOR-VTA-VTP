using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_ORIGEN
    /// </summary>
    public interface IRerOrigenRepository
    {
        int Save(RerOrigenDTO entity);
        void Update(RerOrigenDTO entity);
        void Delete(int reroricodi);
        RerOrigenDTO GetById(int reroricodi);
        List<RerOrigenDTO> List();
    }
}
