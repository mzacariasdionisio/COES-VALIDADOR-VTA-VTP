using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_GRUPOCC
    /// </summary>
    public interface ICpReprogramaRepository
    {
        void Save(CpReprogramaDTO entity);
        void Update(CpReprogramaDTO entity);
        void Delete(int topcodi1);
        CpReprogramaDTO GetById(int topcodi1);
        List<CpReprogramaDTO> GetByCriteria(int topcodi1);
        List<CpReprogramaDTO> List();
        List<CpReprogramaDTO> ListTopPrincipal(int topcodi);
    }
}
