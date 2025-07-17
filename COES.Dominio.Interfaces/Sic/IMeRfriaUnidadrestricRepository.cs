using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_RFRIA_UNIDADRESTRIC
    /// </summary>
    public interface IMeRfriaUnidadrestricRepository
    {
        int Save(MeRfriaUnidadrestricDTO entity);
        void Update(MeRfriaUnidadrestricDTO entity);
        void Delete(int urfriacodi);
        MeRfriaUnidadrestricDTO GetById(int urfriacodi);
        List<MeRfriaUnidadrestricDTO> List();
        List<MeRfriaUnidadrestricDTO> GetByCriteria(DateTime fecha);
    }
}
