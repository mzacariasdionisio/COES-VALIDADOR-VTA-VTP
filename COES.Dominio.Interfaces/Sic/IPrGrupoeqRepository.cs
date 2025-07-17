using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPOEQ
    /// </summary>
    public interface IPrGrupoeqRepository
    {
        int Save(PrGrupoeqDTO entity);
        void Update(PrGrupoeqDTO entity);
        void Delete(int geqcodi);
        PrGrupoeqDTO GetById(int geqcodi);
        List<PrGrupoeqDTO> List();
        List<PrGrupoeqDTO> GetByCriteria(int grupocodi, int equipadre);
    }
}
