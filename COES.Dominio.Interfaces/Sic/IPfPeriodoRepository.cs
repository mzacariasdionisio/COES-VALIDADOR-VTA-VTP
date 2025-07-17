using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_PERIODO
    /// </summary>
    public interface IPfPeriodoRepository
    {
        int Save(PfPeriodoDTO entity);
        void Update(PfPeriodoDTO entity);
        void Delete(int pfpericodi);
        PfPeriodoDTO GetById(int pfpericodi);
        List<PfPeriodoDTO> List();
        List<PfPeriodoDTO> GetByCriteria(int anio);
    }
}
