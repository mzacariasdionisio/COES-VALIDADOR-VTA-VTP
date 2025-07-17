using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_CONCEPTOCOMB
    /// </summary>
    public interface ICbConceptocombRepository
    {
        int Save(CbConceptocombDTO entity);
        void Update(CbConceptocombDTO entity);
        void Delete(int ccombcodi);
        CbConceptocombDTO GetById(int ccombcodi);
        List<CbConceptocombDTO> List();
        List<CbConceptocombDTO> GetByCriteria(int estcomcodi);
    }
}
