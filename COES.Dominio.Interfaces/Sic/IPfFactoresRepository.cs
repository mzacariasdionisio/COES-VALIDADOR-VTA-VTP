using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_FACTORES
    /// </summary>
    public interface IPfFactoresRepository
    {
        int Save(PfFactoresDTO entity);
        void Update(PfFactoresDTO entity);
        void Delete(int pffactcodi);
        PfFactoresDTO GetById(int pffactcodi);
        List<PfFactoresDTO> List();
        List<PfFactoresDTO> GetByCriteria();
        List<PfFactoresDTO> ListarFactoresFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId);
    }
}
