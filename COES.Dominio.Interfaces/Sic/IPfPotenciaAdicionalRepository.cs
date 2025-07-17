using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_POTENCIA_ADICIONAL
    /// </summary>
    public interface IPfPotenciaAdicionalRepository
    {
        int Save(PfPotenciaAdicionalDTO entity);
        void Update(PfPotenciaAdicionalDTO entity);
        void Delete(int pfpadicodi);
        PfPotenciaAdicionalDTO GetById(int pfpadicodi);
        List<PfPotenciaAdicionalDTO> List();
        List<PfPotenciaAdicionalDTO> GetByCriteria();
        List<PfPotenciaAdicionalDTO> ListarPotAdicionalFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId);
    }
}
