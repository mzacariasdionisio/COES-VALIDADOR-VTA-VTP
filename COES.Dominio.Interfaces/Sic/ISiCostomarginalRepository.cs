using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_COSTOMARGINAL
    /// </summary>
    public interface ISiCostomarginalRepository
    {
        int Save(SiCostomarginalDTO entity);
        void Update(SiCostomarginalDTO entity);
        void Delete(int sicmcodi);
        SiCostomarginalDTO GetById(int sicmcodi);
        List<SiCostomarginalDTO> List();
        List<SiCostomarginalDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string barrcodi);
        void SaveSiCostomarginalMasivo(List<SiCostomarginalDTO> ListObj);
        int MaxSiCostomarginal();
        void DeleteSiCostomarginalXFecha(DateTime f_1, DateTime f_2);
        List<SiCostomarginalDTO> GetByCriteriaSiCostomarginalDet(DateTime fechahora);

        //11-03-2019
        void ProcesarTempCostoMarginal(int enviocodi, DateTime fechaini, DateTime fechafin, string usuario);

        List<SiCostomarginalDTO> ObtenerReporteValoresNulos(DateTime fechaInicio, DateTime fechaFin);

        #region SIOSEIN
        List<SiCostomarginalDTO> ObtenerCmgPromedioDiarioDeBarras(DateTime fechaInicio, DateTime fechaFin, string barrcodi);
        #endregion
    }
}
