using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_TABLAPRIE
    /// </summary>
    public interface ISioTablaprieRepository
    {
        int Save(SioTablaprieDTO entity);
        void Update(SioTablaprieDTO entity);
        void Delete(int tpriecodi);
        SioTablaprieDTO GetById(int tpriecodi);
        List<SioTablaprieDTO> List();
        List<SioTablaprieDTO> GetByCriteria();
        List<SioTablaprieDTO> GetByPeriodo(DateTime periodo);
    }
}
