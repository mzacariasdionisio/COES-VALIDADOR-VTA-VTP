using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_PERIODO
    /// </summary>
    public interface IPfrPeriodoRepository
    {
        int Save(PfrPeriodoDTO entity);
        void Update(PfrPeriodoDTO entity);
        void Delete(int pfrpercodi);
        PfrPeriodoDTO GetById(int pfrpercodi);
        List<PfrPeriodoDTO> List();
        List<PfrPeriodoDTO> GetByCriteria(int anio);
    }
}
