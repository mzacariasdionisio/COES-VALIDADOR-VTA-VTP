using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_COSTOMARGINAL
    /// </summary>
    public interface ICmCostomarginalRepository
    {
        int Save(CmCostomarginalDTO entity);
        void Update(CmCostomarginalDTO entity);
        void Delete(int cmgncodi);
        CmCostomarginalDTO GetById(int cmgncodi);
        List<CmCostomarginalDTO> List();
        List<CmCostomarginalDTO> GetByCriteria();
        int ObtenerMaxCorrelativo();
        List<CmCostomarginalDTO> ObtenerResultadoCostoMarginal(DateTime fecha);
        List<CmCostomarginalDTO> ObtenerResultadoCostoMarginalExtranet(DateTime fecha);
        List<CmCostomarginalDTO> ObtenerDatosCostoMarginalCorrida(int correlativo);
        List<CmCostomarginalDTO> ObtenerReporteCostosMarginales(DateTime fechaInicio, DateTime fechaFin, string estimador, string fuentepd, int version);
        List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesTR(DateTime fechaInicio, DateTime fechaFin);       
        int ObtenerIndicadorHora(int correlativo);
        void GrabarRepresentativo(int correlativo, decimal representativo, DateTime fechaProceso);
        //-Métodos para la parte web
        List<CmCostomarginalDTO> ObtenerResultadoCostoMarginalWeb(DateTime fecha, int version);
        List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesWeb(DateTime fechaInicio, DateTime fechaFin, string defecto);
        CmCostomarginalDTO ObtenerResumenCM();
        void EliminarCorridaCostoMarginal(int correlativo);
        List<CmCostomarginalDTO> ObtenerDatosCostoMarginalXPeriodos(DateTime fecha);

        #region Mejoras CMgN
        List<CmCostomarginalDTO> ObtenerComparativoCM(int cngbarcodi, DateTime fechaInicio, DateTime fechaFin);
        List<CmCostomarginalDTO> ObtenerUltimosProcesosCM(DateTime fecha);
        List<CmCostomarginalDTO> ObtenerUltimosProcesosCMPorVersion(DateTime fecha,int version);
        #endregion
    }
}
