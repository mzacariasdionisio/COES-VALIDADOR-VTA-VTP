using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPENTIDAD_DET
    /// </summary>
    public interface ISiHisempentidadDetRepository
    {
        int Save(SiHisempentidadDetDTO entity);
        void Update(SiHisempentidadDetDTO entity);
        void Delete(int hempedcodi);
        SiHisempentidadDetDTO GetById(int hempedcodi);
        List<SiHisempentidadDetDTO> List();
        List<SiHisempentidadDetDTO> GetByCriteria(int migracodi);
        List<SiHisempentidadDetDTO> GetByCriteriaXTabla(int migracodi, string tablename, string fieldid, string fielddesc, string fielddesc2, string fieldestado);
    }
}
