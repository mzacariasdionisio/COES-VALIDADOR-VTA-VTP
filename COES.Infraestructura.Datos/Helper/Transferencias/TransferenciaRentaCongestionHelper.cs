using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// 
    /// </summary>    
    public class TransferenciaRentaCongestionHelper : HelperBase
    {
        public TransferenciaRentaCongestionHelper()
            : base(Consultas.TransferenciaRentaCongestionSql)
        {
        }
        //public TransferenciaInformacionBaseDTO Create(IDataReader dr)
        //{
        //    TransferenciaInformacionBaseDTO entity = new TransferenciaInformacionBaseDTO();

        //    int ITINFBCODI = dr.GetOrdinal(this.TINFBCODI);
        //    if (!dr.IsDBNull(ITINFBCODI)) entity.TinfbCodi = dr.GetInt32(ITINFBCODI);

        //    int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
        //    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

        //    int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
        //    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

        //    int iCOINFBCODI = dr.GetOrdinal(this.COINFBCODI);
        //    if (!dr.IsDBNull(iCOINFBCODI)) entity.CoInfbCodi = dr.GetInt32(iCOINFBCODI);

        //    int iTINFBCODIGO = dr.GetOrdinal(this.TINFBCODIGO);
        //    if (!dr.IsDBNull(iTINFBCODIGO)) entity.TinfbCodigo = dr.GetString(iTINFBCODIGO);

        //    int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
        //    if (!dr.IsDBNull(iEQUICODI)) entity.EquiCodi = dr.GetInt32(iEQUICODI);

        //    int iPERICODI = dr.GetOrdinal(this.PERICODI);
        //    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

        //    int iTINFBVERSION = dr.GetOrdinal(this.TINFBVERSION);
        //    if (!dr.IsDBNull(iTINFBVERSION)) entity.TinfbVersion = dr.GetInt32(iTINFBVERSION);

        //    int iTINFBTIPOINFORMACION = dr.GetOrdinal(this.TINFBTIPOINFORMACION);
        //    if (!dr.IsDBNull(iTINFBTIPOINFORMACION)) entity.TinfbTipoInformacion = dr.GetString(iTINFBTIPOINFORMACION);

        //    int iTINFBESTADO = dr.GetOrdinal(this.TINFBESTADO);
        //    if (!dr.IsDBNull(iTINFBESTADO)) entity.TinfbEstado = dr.GetString(iTINFBESTADO);

        //    int iTINFBUSERNAME = dr.GetOrdinal(this.TINFBUSERNAME);
        //    if (!dr.IsDBNull(iTINFBUSERNAME)) entity.TinfbUserName = dr.GetString(iTINFBUSERNAME);

        //    int iTINFBFECINS = dr.GetOrdinal(this.TINFBFECINS);
        //    if (!dr.IsDBNull(iTINFBFECINS)) entity.TinfbFecIns = dr.GetDateTime(iTINFBFECINS);

        //    int iTINFBFECACT = dr.GetOrdinal(this.TINFBFECACT);
        //    if (!dr.IsDBNull(iTINFBFECACT)) entity.TinfbFecAct = dr.GetDateTime(iTINFBFECACT);

        //    return entity;
        //}

        #region Mapeo de Campos Tabla TRN_RECALCULO

        public string PERICODI = "PERICODI";
        public string RECACODI = "RECACODI";
        public string RECADESCRIPCION = "RECADESCRIPCION";
        public string RECAHORALIMITE = "RECAHORALIMITE";
        public string RECAMASINFO = "RECAMASINFO";
        public string RECANOMBRE = "RECANOMBRE";
        public string RECANROINFORME = "RECANROINFORME";
        public string RECAFECHALIMITE = "RECAFECHALIMITE";
        public string RECAFECHAOBSERVACION = "RECAFECHAOBSERVACION";
        public string RECAFECHAVALORIZACION = "RECAFECHAVALORIZACION";
        public string RECAESTADO = "RECAESTADO";
        public string PERICODIDESTINO = "PERICODIDESTINO";
        public string PERIUSERNAME = "PERIUSERNAME";
        public string RECAUSERNAME = "RECAUSERNAME";
        public string RECAFECINS = "RECAFECINS";
        public string PERIFECINS = "PERIFECINS";
        public string RECAFECACT = "RECAFECACT";
        public string RECANOTA2 = "RECANOTA2";
        public string RECACUADRO1 = "RECACUADRO1";
        public string RECACUADRO2 = "RECACUADRO2";
        public string RECACUADRO3 = "RECACUADRO3";
        public string RECACUADRO4 = "RECACUADRO4";
        public string RECACUADRO5 = "RECACUADRO5";

        public string PERINOMBRE = "PERINOMBRE";
        public string PERIANIOMES = "PERIANIOMES";
        public string EMPRNOMB = "EMPRNOMB";
        public string RCRENCRENTA = "RCRENCRENTA";
        public string RENTATOTAL = "RENTATOTAL";
        public string REPARTO = "REPARTO";

        public string Qregistros = "Q_REGISTROS";

        //Campos reporte
        public string EMPRNOMBRECLIENTE = "EMPRNOMBRECLIENTE";
        public string BARRBARRATRANSFERENCIA = "BARRBARRATRANSFERENCIA";
        public string TRETCODIGO = "TRETCODIGO";
        public string LICITACION = "LICITACION";
        public string BILATERAL = "BILATERAL";
        public string OBSERVACION = "OBS";
        public string FECHAOBSERVACION = "FECHA";

        public string RCCMGCCODI = "RCCMGCCODI";

        //ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.
        public string TENTCCODI = "TENTCODI";
        public string TRETCCODI = "TRETCODI";

        #endregion

        public string SqlListPeriodosRentaCongestion
        {
            get { return base.GetSqlXml("ListPeriodosRentaCongestion"); }
        }
        public string SqlPeriodosRentaCongestionCount
        {
            get { return base.GetSqlXml("PeriodosRentaCongestionCount"); }
        }

        public string SqlDeleteDatosRentaCongestion
        {
            get { return base.GetSqlXml("DeleteDatosRentaCongestion"); }

        }
        public string SqlDeleteDatosRentaEntrega
        {
            get { return base.GetSqlXml("DeleteDatosRentaEntrega"); }

        }
        public string SqlDeleteDatosRentaRetiro
        {
            get { return base.GetSqlXml("DeleteDatosRentaRetiro"); }

        }
        public string SqlDeleteDatosRentaPeriodo
        {
            get { return base.GetSqlXml("DeleteDatosRentaPeriodo"); }

        }

        public string SqlDeleteDatosRentaReparto
        {
            get { return base.GetSqlXml("DeleteDatosRentaReparto"); }

        }

        public string SqlDeleteDatosIngresoCompensacion
        {
            get { return base.GetSqlXml("DeleteDatosIngresoCompensacion"); }

        }

        public string SqlGetPeriodoMes
        {
            get { return base.GetSqlXml("GetPeriodoMes"); }

        }
        public string SqlGetMaximoEntregaId
        {
            get { return base.GetSqlXml("GetMaximoEntregaId"); }

        }
        public string SqlGetMaximoRetiroId
        {
            get { return base.GetSqlXml("GetMaximoRetiroId"); }

        }
        public string SqlGetMaximoPeriodoId
        {
            get { return base.GetSqlXml("GetMaximoPeriodoId"); }

        }

        public string SqlGetMaximoRepartoId
        {
            get { return base.GetSqlXml("GetMaximoRepartoId"); }

        }

        public string SqlGetMaximoIngresoCompensacionId
        {
            get { return base.GetSqlXml("GetMaximoIngresoCompensacionId"); }

        }

        public string SqlGetMaximoCabeceraCompensacionId
        {
            get { return base.GetSqlXml("GetMaximoCabeceraCompensacionId"); }

        }
        public string SqlGetPeriodoVersionReparto
        {
            get { return base.GetSqlXml("GetPeriodoVersionReparto"); }

        }
        public string SqlGetTotalReparto
        {
            get { return base.GetSqlXml("GetTotalReparto"); }

        }
        public string SqlSaveDetalleReparto
        {
            get { return base.GetSqlXml("SaveDetalleReparto"); }

        }
        public string SqlSaveDetalleEntrega
        {
            get { return base.GetSqlXml("SaveDetalleEntrega"); }

        }
        public string SqlSaveDetalleRetiro
        {
            get { return base.GetSqlXml("SaveDetalleRetiro"); }

        }
        public string SqlSaveRentaPeriodo
        {
            get { return base.GetSqlXml("SaveRentaPeriodo"); }

        }
        public string SqlSaveRentaCongestionRetiro
        {
            get { return base.GetSqlXml("SaveRentaCongestionRetiro"); }

        }

        public string SqlSaveRentaCongestionIngresoCompensacion
        {
            get { return base.GetSqlXml("SaveRentaCongestionIngresoCompensacion"); }

        }

        public string SqlGetTotalRentaCongestion
        {
            get { return base.GetSqlXml("GetTotalRentaCongestion"); }

        }

        public string SqlGetTotalRentaNoAsignada
        {
            get { return base.GetSqlXml("GetTotalRentaNoAsignada"); }

        }
        public string SqlListRentaCongestion
        {
            get { return base.GetSqlXml("ListRentaCongestion"); }

        }
        public string SqlListRentaCongestionDetalle
        {
            get { return base.GetSqlXml("ListRentaCongestionDetalle"); }

        }

        public string SqlListTotalPorcentajes
        {
            get { return base.GetSqlXml("ListTotalPorcentajes"); }

        }

        public string SqlListErroresBarras
        {
            get { return base.GetSqlXml("ListErroresBarras"); }

        }

        public string SqlListCostosMarginales
        {
            get { return base.GetSqlXml("ListCostosMarginales"); }

        }

        public string SqlListEntregasRetiros
        {
            get { return base.GetSqlXml("ListEntregasRetiros"); }

        }

        public string SqlListCostosMarginalesPorBarra
        {
            get { return base.GetSqlXml("ListCostosMarginalesPorBarra"); }

        }

        public string SqlListTotalRegistrosCostosMarginales
        {
            get { return base.GetSqlXml("ListTotalRegistrosCostosMarginales"); }

        }

        public string SqlDeleteCostoMarginalCab
        {
            get { return base.GetSqlXml("DeleteCostoMarginalCab"); }
        }
        public string SqlDeleteDatosRcgCostoMarginalDet
        {
            get { return base.GetSqlXml("DeleteDatosRcgCostoMarginalDet"); }

        }

        public string SqlGetMaximoCostoMarginalDetalleId
        {
            get { return base.GetSqlXml("GetMaximoCostoMarginalDetalleId"); }

        }

        public string SqlSaveCostoMarginalDetSEV
        {
            get { return base.GetSqlXml("SaveCostoMarginalDetSEV"); }

        }
        public string SqlSaveCostoMarginalDetCalculoAnterior
        {
            get { return base.GetSqlXml("SaveCostoMarginalDetCalculoAnterior"); }

        }

        public string SqlValidarCostoMarginal
        {
            get { return base.GetSqlXml("ValidarCostoMarginal"); }

        }

        #region ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.
        public string SqlAjustarRGCEntregas
        {
            get { return base.GetSqlXml("AjustarRGCEntregas"); }

        }
        public string SqlAjustarRGCRetiros
        {
            get { return base.GetSqlXml("AjustarRGCRetiros"); }

        }

        public string SqlListRGCEntregas
        {
            get { return base.GetSqlXml("ListRGCEntregas"); }

        }
        public string SqlListRGCRetiros
        {
            get { return base.GetSqlXml("ListRGCRetiros"); }

        }

        public string SqlAjustarRGCEntregasDiaAterior
        {
            get { return base.GetSqlXml("AjustarRGCEntregasDiaAterior"); }

        }
        public string SqlAjustarRGCRetirosDiaAterior
        {
            get { return base.GetSqlXml("AjustarRGCRetirosDiaAterior"); }

        }

        public string SqlUpdateRGCEntregas
        {
            get { return base.GetSqlXml("UpdateRGCEntregas"); }

        }
        public string SqlUpdateRGCRetiros
        {
            get { return base.GetSqlXml("UpdateRGCRetiros"); }

        }
        #endregion
    }


}
