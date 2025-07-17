using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPENTIDAD
    /// </summary>
    public interface ISiHisempentidadRepository
    {
        int Save(SiHisempentidadDTO entity);
        void Update(SiHisempentidadDTO entity);
        void Delete(int hempencodi);
        SiHisempentidadDTO GetById(int hempencodi);
        List<SiHisempentidadDTO> List();
        List<SiHisempentidadDTO> GetByCriteria(int migracodi);
    }
}
