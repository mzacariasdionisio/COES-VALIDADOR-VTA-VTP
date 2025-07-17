using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_RECALCULO
    /// </summary>
    public interface IPfRecalculoRepository
    {
        int Save(PfRecalculoDTO entity);
        void Update(PfRecalculoDTO entity);
        void Delete(int pfrecacodi);
        PfRecalculoDTO GetById(int pfrecacodi);
        List<PfRecalculoDTO> List();
        List<PfRecalculoDTO> GetByCriteria(int pfPericodi, int anio, int mes);
    }
}
