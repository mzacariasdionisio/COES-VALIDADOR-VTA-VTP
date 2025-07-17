using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_FAC_PER_MED
    /// </summary>
    public interface IRerFacPerMedRepository
    {
        int Save(RerFacPerMedDTO entity);
        void Update(RerFacPerMedDTO entity);
        void Delete(int rerFpmCodi);
        RerFacPerMedDTO GetById(int rerFpmCodi);
        List<RerFacPerMedDTO> List();
    }
}
