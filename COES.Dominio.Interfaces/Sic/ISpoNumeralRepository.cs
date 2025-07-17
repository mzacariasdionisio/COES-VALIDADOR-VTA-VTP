using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMERAL
    /// </summary>
    public interface ISpoNumeralRepository
    {
        int Save(SpoNumeralDTO entity);
        void Update(SpoNumeralDTO entity);
        void Delete(int numecodi);
        SpoNumeralDTO GetById(int numecodi);
        List<SpoNumeralDTO> List();
        List<SpoNumeralDTO> GetByCriteria();
    }
}
