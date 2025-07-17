using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMERALDAT
    /// </summary>
    public interface ISpoNumeraldatRepository
    {
        int Save(SpoNumeraldatDTO entity);
        void Update(SpoNumeraldatDTO entity);
        void Delete(int numdatcodi);
        SpoNumeraldatDTO GetById(int numdatcodi);
        List<SpoNumeraldatDTO> List();
        List<SpoNumeraldatDTO> GetDataNumerales(int numecodi, DateTime fechaini, DateTime fechafin);
        List<SpoNumeraldatDTO> GetByCriteria(int numecodi, DateTime fechaini, DateTime fechafin);
        List<SpoNumeraldatDTO> GetDataVAlorAgua(DateTime fechaini, DateTime fechafin);
    }
}
