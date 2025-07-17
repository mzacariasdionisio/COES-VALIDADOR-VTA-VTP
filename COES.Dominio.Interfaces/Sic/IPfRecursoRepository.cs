using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_RECURSO
    /// </summary>
    public interface IPfRecursoRepository
    {
        int Save(PfRecursoDTO entity);
        void Update(PfRecursoDTO entity);
        void Delete(int pfrecucodi);
        PfRecursoDTO GetById(int pfrecucodi);
        List<PfRecursoDTO> List();
        List<PfRecursoDTO> GetByCriteria();
    }
}
