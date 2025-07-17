using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_RESUMENMD
    /// </summary>
    public interface IWbResumenmdRepository
    {
        int Save(WbResumenmdDTO entity);
        void Update(WbResumenmdDTO entity);
        void Delete(int resmdcodi);
        void DeleteByMes(DateTime fechaPeriodo);
        WbResumenmdDTO GetById(int resmdcodi);
        List<WbResumenmdDTO> List();
        List<WbResumenmdDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        WbResumenmdDTO VerificarExistencia(DateTime fechaProceso);
        WbResumenmdDTO GetMaxMdvalor(DateTime fechaInicio, DateTime fechaFin);
    }
}
