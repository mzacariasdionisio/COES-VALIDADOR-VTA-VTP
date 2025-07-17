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
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_ENVIO
    /// </summary>
    public class TrnEnvioHelper : HelperBase
    {
        #region Constructor
        public TrnEnvioHelper() : base(Consultas.TrnEnvioSql)
        {

        }
        #endregion

        #region Helpers Trn_Envio
        public TrnEnvioDTO Create(IDataReader dr)
        {
            TrnEnvioDTO entity = new TrnEnvioDTO();

            #region Campos Originales
            // TRNENVCODI
            int iTrnEnvCodi = dr.GetOrdinal(this.TrnEnvCodi);
            if (!dr.IsDBNull(iTrnEnvCodi)) entity.TrnEnvCodi = dr.GetInt32(iTrnEnvCodi);

            // PERICODI
            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            // RECACODI
            int iRecaCodi = dr.GetOrdinal(this.RecaCodi);
            if (!dr.IsDBNull(iRecaCodi)) entity.RecaCodi = dr.GetInt32(iRecaCodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            // TRNMODCODI
            int iTrnModCodi = dr.GetOrdinal(this.TrnModCodi);
            if (!dr.IsDBNull(iTrnModCodi)) entity.TrnModCodi = dr.GetInt32(iTrnModCodi);

            // TRNENVTIPINF
            int iTrnEnvTipInf = dr.GetOrdinal(this.TrnEnvTipInf);
            if (!dr.IsDBNull(iTrnEnvTipInf)) entity.TrnEnvTipInf = dr.GetString(iTrnEnvTipInf);

            // TRNENVPLAZO
            int iTrnEnvPlazo = dr.GetOrdinal(this.TrnEnvPlazo);
            if (!dr.IsDBNull(iTrnEnvPlazo)) entity.TrnEnvPlazo = dr.GetString(iTrnEnvPlazo);

            // TRNENVLIQVT
            int iTrnEnvLiqVt = dr.GetOrdinal(this.TrnEnvLiqVt);
            if (!dr.IsDBNull(iTrnEnvLiqVt)) entity.TrnEnvLiqVt = dr.GetString(iTrnEnvLiqVt);
                       
            // TRNENVUSEINS
            int iTrnEnvUseIns = dr.GetOrdinal(this.TrnEnvUseIns);
            if (!dr.IsDBNull(iTrnEnvUseIns)) entity.TrnEnvUseIns = dr.GetString(iTrnEnvUseIns);

            // TRNENVFECINS
            int iTrnEnvFecIns = dr.GetOrdinal(this.TrnEnvFecIns);
            if (!dr.IsDBNull(iTrnEnvFecIns)) entity.TrnEnvFecIns = dr.GetDateTime(iTrnEnvFecIns);

            // TRNENVUSEACT
            int iTrnEnvUseAct = dr.GetOrdinal(this.TrnEnvUseAct);
            if (!dr.IsDBNull(iTrnEnvUseAct)) entity.TrnEnvUseAct = dr.GetString(iTrnEnvUseAct);

            // TRNENVFECACT
            int iTrnEnvFecAct = dr.GetOrdinal(this.TrnEnvFecAct);
            if (!dr.IsDBNull(iTrnEnvFecAct)) entity.TrnEnvFecAct = dr.GetDateTime(iTrnEnvFecAct);

            // TRNENVUSECOES
            int iTrnEnvUseCoes = dr.GetOrdinal(this.TrnEnvUseCoes);
            if (!dr.IsDBNull(iTrnEnvUseCoes)) entity.TrnEnvUseCoes = dr.GetString(iTrnEnvUseCoes);

            // TRNENVFECCOES
            int iTrnEnvFecCoes = dr.GetOrdinal(this.TrnEnvFecCoes);
            if (!dr.IsDBNull(iTrnEnvFecCoes)) entity.TrnEnvFecCoes = dr.GetDateTime(iTrnEnvFecCoes);
            #endregion           

            return entity;
        }
        #endregion

        #region Mapeo de Campos Trn_Envio

        #region Campos Originales
        public string TrnEnvCodi = "trnenvcodi";
        public string PeriCodi = "pericodi";
        public string RecaCodi = "recacodi";
        public string EmprCodi = "emprcodi";
        public string TrnModCodi = "trnmodcodi";
        public string TrnEnvTipInf = "trnenvtipinf";
        public string TrnEnvPlazo = "trnenvplazo";
        public string TrnEnvLiqVt = "trnenvliqvt";
        public string TrnEnvUseIns = "trnenvuseins";
        public string TrnEnvFecIns = "trnenvfecins";
        public string TrnEnvUseAct = "trnenvuseact";
        public string TrnEnvFecAct = "trnenvfecact";
        public string TrnEnvUseCoes = "trnenvusecoes";
        public string TrnEnvFecCoes = "trnenvfeccoes";
        #endregion

        #region Campos Adicionales
        public string EmprNomb = "emprnomb";
        public string PeriNombre = "perinombre";
        public string TrnModNombre = "trnmodnombre";
        #endregion

        #endregion

        #region Querys SQL Trn_Envio
        public string SqlGetByIdPeriodoEmpresa
        {
            get { return base.GetSqlXml("GetByIdPeriodoEmpresa"); }
        }

        public string SqlUpdateByCriteriaTrnEnvio
        {
            get { return base.GetSqlXml("UpdateByCriteriaTrnEnvio"); }
        }

        public string SqlListIntranet
        {
            get { return base.GetSqlXml("ListIntranet"); }
        }

        public string SqlUpdateTrnEnvioLiquidacion
        {
            get { return base.GetSqlXml("UpdateTrnEnvioLiquidacion"); }
        }

        public string SqlUpdateEntregaLiquidacion
        {
            get { return base.GetSqlXml("UpdateEntregaLiquidacion"); }
        }

        public string SqlUpdateRetiroLiquidacion
        {
            get { return base.GetSqlXml("UpdateRetiroLiquidacion"); }
        }
        #endregion

    }
}