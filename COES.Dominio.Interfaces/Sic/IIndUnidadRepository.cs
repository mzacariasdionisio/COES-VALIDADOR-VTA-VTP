using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_UNIDAD
    /// </summary>
    public interface IIndUnidadRepository
    {
        int Save(IndUnidadDTO entity);
        void Update(IndUnidadDTO entity);
        void Delete(int iunicodi);
        IndUnidadDTO GetById(int iunicodi);
        List<IndUnidadDTO> List();
        List<IndUnidadDTO> GetByCriteria();
    }
}
