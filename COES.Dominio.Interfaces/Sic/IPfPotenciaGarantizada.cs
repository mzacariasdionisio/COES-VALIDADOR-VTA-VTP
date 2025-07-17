using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_POTENCIAGARANTIZADA
    /// </summary>
    public interface IPfPotenciaGarantizadaRepository
    {
        int Save(PfPotenciaGarantizadaDTO entity);
        void Update(PfPotenciaGarantizadaDTO entity);
        void Delete(int pfpgarcodi);
        PfPotenciaGarantizadaDTO GetById(int pfpgarcodi);
        List<PfPotenciaGarantizadaDTO> List();
        List<PfPotenciaGarantizadaDTO> GetByCriteria();
        List<PfPotenciaGarantizadaDTO> ListarPotGarantizadaFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId);
    }
}
