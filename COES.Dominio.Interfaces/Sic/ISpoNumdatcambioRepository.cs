using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMDATCAMBIO
    /// </summary>
    public interface ISpoNumdatcambioRepository
    {
        int Save(SpoNumdatcambioDTO entity);
        void Update(SpoNumdatcambioDTO entity);
        void Delete(int numdcbcodi);
        SpoNumdatcambioDTO GetById(int numdcbcodi);
        List<SpoNumdatcambioDTO> List();
        List<SpoNumdatcambioDTO> GetByCriteria(int version, int numecodi, DateTime periodo);
    }
}
