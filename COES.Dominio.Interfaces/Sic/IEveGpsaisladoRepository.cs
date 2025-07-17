using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_GPSAISLADO
    /// </summary>
    public interface IEveGpsaisladoRepository
    {
        int Save(EveGpsaisladoDTO entity);
        void Update(EveGpsaisladoDTO entity);
        void Delete(int gpsaiscodi);
        void DeleteByIccodi(int iccodi);
        EveGpsaisladoDTO GetById(int gpsaiscodi);
        List<EveGpsaisladoDTO> List();
        List<EveGpsaisladoDTO> GetByCriteria(int iccodi);
    }
}
