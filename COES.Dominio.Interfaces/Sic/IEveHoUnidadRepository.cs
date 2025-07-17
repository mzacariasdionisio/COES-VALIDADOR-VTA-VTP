using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_HO_UNIDAD
    /// </summary>
    public interface IEveHoUnidadRepository
    {
        int Save(EveHoUnidadDTO entity);
        void Update(EveHoUnidadDTO entity);
        void Delete(int hopunicodi);
        EveHoUnidadDTO GetById(int hopunicodi);
        List<EveHoUnidadDTO> List();
        List<EveHoUnidadDTO> GetByCriteria();
    }
}
