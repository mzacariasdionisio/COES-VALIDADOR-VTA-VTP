using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso al calculo de Rentas de Congestion
    /// </summary>
    public interface ITransferenciaRentaCongestionRepository
    {
        List<TransferenciaRecalculoDTO> ListPeriodosRentaCongestion(int pericodi, int recacodi, int regIni, int regFin);
        void CalcularRentasCongestionPeriodo(int pericodi, int recacodi, string nombreUsuario, int diasPeriodo);

        decimal GetTotalRentaCongestion(int pericodi, int recacodi);
        decimal GetTotalRentaNoAsignada(int pericodi, int recacodi);
        List<TransferenciaRentaCongestionDTO> ListRentaCongestion(int pericodi, int recacodi);
        List<TransferenciaRentaCongestionDTO> ListRentaCongestionDetalle(int pericodi, int recacodi);

        int ListPeriodosRentaCongestionCount();

        int DeleteDatosRentaCongestion(int pericodi, int recacodi);
        int DeleteDatosEntrega(int pericodi, int recacodi);
        int DeleteDatosRetiro(int pericodi, int recacodi);
        int DeleteDatosPeriodo(int pericodi, int recacodi);              

        int GetPeriodoMes(int pericodi);
        int GetMaximoEntregaId();
        long GetMaximoRetiroId();
        int GetMaximoPeriodoId();

        int SaveDetalleEntrega(int pericodi, int recacodi, int rcentdcodi, int perioaniomes, int diasPeriodo);
        int SaveDetalleRetiro(int pericodi, int recacodi, long rcretdcodi, int perioaniomes, int diasPeriodo);
        int SaveRentaPeriodo(int pericodi, int recacodi, int rcrpercodi, string nombreUsuario);
        int SaveRentaCongestionRetiro(int pericodi, int recacodi, string nombreUsuario);
        int SaveRentaCongestionIngresoCompensacion(int pericodi, int recacodi, int ingcomcodi, int cabcomcodi, string nombreUsuario);

        int DeleteDatosReparto(int pericodi, int recacodi);
        int GetMaximoRepartoId();
        List<int> GetPeriodoVersionReparto(int pericodi, int recacodi);
        decimal GetTotalReparto(int pericodi, int recacodi);
        int SaveDetalleReparto(int rcrrndcodi, int pericodi, int recacodi, decimal rentaTotal,
            string nombreUsuario, int porc_pericodi, int porc_version);

        int GetTotalPorcentajes(int pericodi, int recacodi);
        List<TransferenciaRentaCongestionDTO> ListErroresBarras(int pericodi, int recacodi, int perioaniomes);

        int GetMaximoIngresoCompensacionId();
        int GetMaximoCabeceraCompensacionId(int pericodi);

        int DeleteDatosingresocompensacion(int pericodi, int recacodi, int cabcomcodi);

        IDataReader ListCostosMarginales(int pericodi, int recacodi, int perianiomes);

        IDataReader ListEntregasRetiros(int pericodi, int recacodi, int perianiomes, int ultimoDia);

        IDataReader ListCostosMarginalesPorBarra(int pericodi, int recacodi, int perianiomes);

        IDataReader ListTotalRegistrosCostosMarginales(int pericodi, int recacodi);

        int DeleteDatosRcgCostoMarginalCab(int pericodi, int recacodi);
        int DeleteDatosRcgCostoMarginalDet(int rccmgccodi);
        int GetMaximoCostoMarginalDetalleId();

        int SaveCostoMarginalDetalleSEV(int rccmgdcodi, int rccmgccodi, int perioaniomes);
        int SaveCostoMarginalDetalleCalculoAnterior(int rccmgdcodi, int rccmgccodi, int pericodi, int recacodiAnterior);
        int ValidaCostoMarginal(int pericodi, int recacodi);

        //ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.
        void AjustarRGCEntregasRetiros(int rcentdcodi, long rccretdcodi, string sTrncmafecha, string sRcEntRetHora);
        List<TransferenciaEntregaDTO> listRGCEntregas(int rcentdcodi);
        List<TransferenciaRetiroDTO> listRGCRetiros(long rccretdcodi);
        void AjustarRGCEntregasDiaAnterior(int tentcodi, int pericodianterior, int versionanterior, string sTrncmafecha, string sRcEntRetHora);
        void AjustarRGCRetirosDiaAnterior(int tretcodi, int pericodianterior, int versionanterior, string sTrncmafecha, string sRcEntRetHora);
        void UpdateRGCEntregasRetiros(int rcentdcodi, long rccretdcodi);

    }
}
