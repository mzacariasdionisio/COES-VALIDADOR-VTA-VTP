using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_CENTRAL_PMPO
    /// </summary>
    public interface IRerCentralPmpoRepository
    {
        int Save(RerCentralPmpoDTO entity);
        void Update(RerCentralPmpoDTO entity);
        void Delete(int rerCpmCodi);
        RerCentralPmpoDTO GetById(int rerCpmCodi);
        List<RerCentralPmpoDTO> List();
        List<RerCentralPmpoDTO> ListByRercencodi(int Rercencodi);
        void DeleteAllByRercencodi(int Rercencodi);
    }
}
