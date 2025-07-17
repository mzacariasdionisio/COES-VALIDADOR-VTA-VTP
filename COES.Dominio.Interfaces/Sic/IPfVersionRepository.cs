using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_VERSION
    /// </summary>
    public interface IPfVersionRepository
    {
        int Save(PfVersionDTO entity);
        void Update(PfVersionDTO entity);
        void Delete(int pfverscodi);
        PfVersionDTO GetById(int pfverscodi);
        List<PfVersionDTO> List();
        List<PfVersionDTO> GetByCriteria(int recacodi, int recucodi);
    }
}
