using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_COLUMNAPRIE
    /// </summary>
    public interface ISioColumnaprieRepository
    {
        int Save(SioColumnaprieDTO entity);
        void Update(SioColumnaprieDTO entity);
        void Delete(int cpriecodi);
        SioColumnaprieDTO GetById(int cpriecodi);
        List<SioColumnaprieDTO> List();
        List<SioColumnaprieDTO> GetByCriteria(int tpriecodi);
    }
}
