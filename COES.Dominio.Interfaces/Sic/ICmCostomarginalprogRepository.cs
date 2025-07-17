using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_COSTOMARGINALPROG
    /// </summary>
    public interface ICmCostomarginalprogRepository
    {
        int Save(CmCostomarginalprogDTO entity);
        void Update(CmCostomarginalprogDTO entity);
        void Delete(int cmarprcodi);
        CmCostomarginalprogDTO GetById(int cmarprcodi);
        List<CmCostomarginalprogDTO> List();
        List<CmCostomarginalprogDTO> GetByCriteria();

        #region MonitoreoMME
        List<CmCostomarginalprogDTO> ListPeriodoCostoMarProg(DateTime fecha);
        #endregion

        #region SIOSEIN
        List<CmCostomarginalprogDTO> GetByBarratranferencia(string barrcodi, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region Ticket IMME

        void GrabarDatosBulk(List<CmCostomarginalprogDTO> entitys, DateTime fechaInicio, DateTime fechaFin);
        List<CmCostomarginalprogDTO> ListaTotalXDia(DateTime fechaInicio, DateTime fechaFin);
        #endregion
    }
}
