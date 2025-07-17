using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_PERIODOCUADRO7
    /// </summary>
    public interface IIndPeriodocuadro7Repository
    {
        int Save(IndPeriodocuadro7DTO entity);
        void Update(IndPeriodocuadro7DTO entity);
        void Delete(int percu7codi);
        IndPeriodocuadro7DTO GetById(int percu7codi);
        List<IndPeriodocuadro7DTO> List();
        List<IndPeriodocuadro7DTO> GetByCriteria();
    }
}
