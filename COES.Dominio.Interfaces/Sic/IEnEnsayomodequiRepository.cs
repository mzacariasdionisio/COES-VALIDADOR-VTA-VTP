using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ENSAYOMODEQUI
    /// </summary>
    public interface IEnEnsayomodequiRepository
    {
        int Save(EnEnsayomodequiDTO entity);
        void Update(EnEnsayomodequiDTO entity);
        void Delete(int enmoeqcodi);
        EnEnsayomodequiDTO GetById(int enmoeqcodi);
        List<EnEnsayomodequiDTO> List();
        List<EnEnsayomodequiDTO> GetByCriteria();
    }
}