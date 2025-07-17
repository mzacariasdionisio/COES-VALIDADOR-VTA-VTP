using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_RESUMENMDDETALLE
    /// </summary>
    public interface IWbResumenmddetalleRepository
    {
        int Save(WbResumenmddetalleDTO entity);
        void Update(WbResumenmddetalleDTO entity);
        void Delete(int resmddcodi);
        void DeleteByMes(DateTime fechaPeriodo);
        WbResumenmddetalleDTO GetById(int resmddcodi);
        List<WbResumenmddetalleDTO> GetByIdMd(int resmdcodi);
        List<WbResumenmddetalleDTO> List();
        List<WbResumenmddetalleDTO> GetByCriteria();
    }
}
