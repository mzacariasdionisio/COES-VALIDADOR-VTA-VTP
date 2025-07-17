using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DAI_CALENDARIOPAGO
    /// </summary>
    public interface IDaiCalendariopagoRepository
    {
        int Save(DaiCalendariopagoDTO entity);
        void Update(DaiCalendariopagoDTO entity);
        void Delete(int calecodi);
        void Reprocesar(DaiCalendariopagoDTO calendario);
        void Liquidar(DaiCalendariopagoDTO calendario);
        DaiCalendariopagoDTO GetById(int calecodi);
        List<DaiCalendariopagoDTO> List(int aporcodi, int estado);
        List<DaiCalendariopagoDTO> GetByCriteria(int aporcodi);
        List<DaiCalendariopagoDTO> GetByCriteriaAnio(int emprcodi);
    }
}
