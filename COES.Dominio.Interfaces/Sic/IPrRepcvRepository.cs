using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_REPCV
    /// </summary>
    public interface IPrRepcvRepository
    {
        int Save(PrRepcvDTO entity);
        void Update(PrRepcvDTO entity);
        void Delete(int repcodi);
        PrRepcvDTO GetById(int repcodi);
        List<PrRepcvDTO> List();
        List<PrRepcvDTO> GetByCriteria(DateTime dFechaInicio, DateTime dFechaFin);
        List<PrRepcvDTO> ObtenerReporte(string repFechaIni, string repFechaFin);

        #region MigracionSGOCOES-GrupoB
        PrRepcvDTO GetByFechaAndTipo(DateTime fecha, string tipo);
        #endregion

        #region SIOSEIN2

        List<PrRepcvDTO> ObtenerReportecvYVariablesXPeriodo(string periodo);

        #endregion

        List<PrRepcvDTO> GetRepcvByEnvcodi(int cbenvcodi);
        
    }
}
