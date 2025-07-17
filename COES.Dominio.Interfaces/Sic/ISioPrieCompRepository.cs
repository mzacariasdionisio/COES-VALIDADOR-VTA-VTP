using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_PRIE_COMP
    /// </summary>
    public interface ISioPrieCompRepository
    {
        int Save(SioPrieCompDTO entity);
        void Update(SioPrieCompDTO entity);
        void Delete(DateTime fechaPeriodo);
        SioPrieCompDTO GetById(int tbcompcodi);
        List<SioPrieCompDTO> List();
        List<SioPrieCompDTO> GetByCriteria(DateTime fechaPeriodo);
    }
}
