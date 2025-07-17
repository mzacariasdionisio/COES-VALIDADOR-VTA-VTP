using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_COSTO_MARGINAL
    /// </summary>
    public interface ICostoMarginalRepository : IRepositoryBase
    {
        int Save(CostoMarginalDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 CostMargVersion);
        List<CostoMarginalDTO> List(int PeriCodi, int version);
        List<CostoMarginalDTO> GetByCriteria(int PeriCodi, string BarrCodi);
        List<CostoMarginalDTO> ListByBarrPeriodoVersion(int BarrCodi, int PeriCodi, int CostMargVersion);
        List<CostoMarginalDTO> GetByCodigo(int? pericodi);
        List<CostoMarginalDTO> GetByBarraTransferencia(int pericodi, int iVersion);

        List<CostoMarginalDTO> GetConsultaCostosMarginales(CostoMarginalDTO parametro);
        List<CostoMarginalDTO> GetBarrasMarginales(int pericodi, int version);
        List<CostoMarginalDTO> ListByFactorPerdida(int iFacPerCodi);
        List<CostoMarginalDTO> ListByReporte(int iPeriCodi, int iCostMargVersion);
        List<CostoMarginalDTO> ObtenerReporteCostoMarginalDTR(int barracodi, int pericodi, int version);

        // Inicio de Cambio 31-05-2017 - Sistema de Compensaciones
        // DSH 05-07-2017 : Se cambio por requerimiento
        List<ComboCompensaciones> ListCostoMarginalVersion(int pericodi);
        // Fin de Cambio - Sistema de Compensaciones
        CostoMarginalDTO GetByIdPorBarraDia(int PeriCodi, int iCostMargVersion, int BarrCodi, int CosMarDia);

        void BulkInsert(List<TrnCostoMarginalBullk> entitys);
        int GetCodigoGenerado();
        int GetCodigoGeneradoDec();

        #region MonitoreoMME
        List<CostoMarginalDTO> ListCostoMarginalWithGrupo(int pericodi, int version);
        #endregion

        #region Siosein2  
        List<CostoMarginalDTO> ListCostoMarginalByPeriodoVersionZona(int periodo, int version, string zona);
        #endregion

        #region AJUSTE COSTOS MARGINALES
        void AjustarCostosMarginales(string CosMarRec, int CosMarDiaRec, int PericodiRec, int CosMarVerRec, string CosMarEnt, int CosMarDiaEnt, int PericodiEnt, int CosMarVerEnt);
        #endregion
        //CU21
        List<CostoMarginalDTO> ListarByCodigoEntrega(int iPeriCodi, int iRecaCodi, int iCodEntCodi);

    }
}
