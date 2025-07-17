using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_EMPRESA
    /// </summary>
    public interface IAfEmpresaRepository
    {
        int Save(AfEmpresaDTO entity);
        void Update(AfEmpresaDTO entity);
        void Delete(int afemprcodi);
        AfEmpresaDTO GetById(int afemprcodi);
        List<AfEmpresaDTO> List();
        List<AfEmpresaDTO> GetByCriteria();
        AfEmpresaDTO GetByIdxEmprcodi(int emprcodi);
    }
}
