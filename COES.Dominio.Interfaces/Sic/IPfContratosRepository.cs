using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_CONTRATOS
    /// </summary>
    public interface IPfContratosRepository
    {
        int Save(PfContratosDTO entity);
        void Update(PfContratosDTO entity);
        void Delete(int pfcontcodi);
        PfContratosDTO GetById(int pfcontcodi);
        List<PfContratosDTO> List();
        List<PfContratosDTO> GetByCriteria();
        List<PfContratosDTO> ListarContratosCVFiltro(int pericodi, int recacodi, int versionId);
    }
}
