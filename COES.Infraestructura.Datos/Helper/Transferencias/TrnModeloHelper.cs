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
    /// Clase que contiene el mapeo de las tablas TRN_MODELO y TRN_MODELO_RETIRO
    /// </summary>
    public class TrnModeloHelper : HelperBase
    {
        #region Constructor
        public TrnModeloHelper() : base(Consultas.TrnModeloSql)
        {

        }
        #endregion

        #region Trn_Modelo
        #region Helpers Trn_Modelo
        public TrnModeloDTO CreateListTrnModelo(IDataReader dr)
        {
            TrnModeloDTO entity = new TrnModeloDTO();

            #region Campos Originales
            // TRNMODCODI
            int iTrnModCodi = dr.GetOrdinal(this.TrnModCodi);
            if (!dr.IsDBNull(iTrnModCodi)) entity.TrnModCodi = dr.GetInt32(iTrnModCodi);

            // TRNMODNOMBRE
            int iTrnModNombre = dr.GetOrdinal(this.TrnModNombre);
            if (!dr.IsDBNull(iTrnModNombre)) entity.TrnModNombre = dr.GetString(iTrnModNombre);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            // TRNMODUSEINS
            int iTrnModUseIns = dr.GetOrdinal(this.TrnModUseIns);
            if (!dr.IsDBNull(iTrnModUseIns)) entity.TrnModUseIns = dr.GetString(iTrnModUseIns);

            // TRNMODFECINS
            int iTrnModFecIns = dr.GetOrdinal(this.TrnModFecIns);
            if (!dr.IsDBNull(iTrnModFecIns)) entity.TrnModFecIns = dr.GetDateTime(iTrnModFecIns);

            // TRNMODUSEACT
            int iTrnModUseAct = dr.GetOrdinal(this.TrnModUseAct);
            if (!dr.IsDBNull(iTrnModUseAct)) entity.TrnModUseAct = dr.GetString(iTrnModUseAct);

            // TRNMODFECACT
            int iTrnModFecAct = dr.GetOrdinal(this.TrnModFecAct);
            if (!dr.IsDBNull(iTrnModFecAct)) entity.TrnModFecAct = dr.GetDateTime(iTrnModFecAct);
            #endregion

            #region Campos Adicionales
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);
            #endregion


            return entity;
        }

        public TrnModeloDTO CreateListarComboTrnModelo(IDataReader dr)
        {
            TrnModeloDTO entity = new TrnModeloDTO();

            #region Campos Originales
            // TRNMODCODI
            int iTrnModCodi = dr.GetOrdinal(this.TrnModCodi);
            if (!dr.IsDBNull(iTrnModCodi)) entity.TrnModCodi = dr.GetInt32(iTrnModCodi);

            // TRNMODNOMBRE
            int iTrnModNombre = dr.GetOrdinal(this.TrnModNombre);
            if (!dr.IsDBNull(iTrnModNombre)) entity.TrnModNombre = dr.GetString(iTrnModNombre);
            #endregion

            return entity;
        }
        #endregion

        #region Mapeo de Campos Trn_Modelo
        #region Campos Originales
        public string TrnModCodi = "trnmodcodi";
        public string TrnModNombre = "trnmodnombre";
        public string EmprCodi = "emprcodi";
        public string TrnModUseIns = "trnmoduseins";
        public string TrnModFecIns = "trnmodfecins";
        public string TrnModUseAct = "trnmoduseact";
        public string TrnModFecAct = "trnmodfecact";
        #endregion

        #region Campos Adicionales
        public string EmprNomb = "emprnomb";
        #endregion
        #endregion

        #region Querys SQL Trn_Modelo
        public string SqlGetMaxIdTrnModelo
        {
            get { return base.GetSqlXml("GetMaxIdTrnModelo"); }
        }

        public string SqlSaveTrnModelo
        {
            get { return base.GetSqlXml("SaveTrnModelo"); }
        }

        public string SqlUpdateTrnModelo
        {
            get { return base.GetSqlXml("UpdateTrnModelo"); }
        }

        public string SqlDeleteTrnModelo
        {
            get { return base.GetSqlXml("DeleteTrnModelo"); }
        }

        public string SqlListTrnModelo
        {
            get { return base.GetSqlXml("ListTrnModelo"); }
        }
        
        public string SqlListTrnModeloByEmpresa
        {
            get { return base.GetSqlXml("ListTrnModeloByEmpresa"); }
        }

        public string SqlListarComboTrnModelo
        {
            get { return base.GetSqlXml("ListarComboTrnModelo"); }
        }
        #endregion
        #endregion

        #region Trn_Modelo_Retiro
        #region Helpers Trn_Modelo_Retiro
        public TrnModeloRetiroDTO CreateListTrnModeloRetiro(IDataReader dr)
        {
            TrnModeloRetiroDTO entity = new TrnModeloRetiroDTO();

            #region Campos Originales
            // TRNMODRETCODI
            int iTrnMreCodi = dr.GetOrdinal(this.TrnMreCodi);
            if (!dr.IsDBNull(iTrnMreCodi)) entity.TrnMreCodi = dr.GetInt32(iTrnMreCodi);

            // TRNMODCODI
            int iTrnModCodi = dr.GetOrdinal(this.TrnModCodi);
            if (!dr.IsDBNull(iTrnModCodi)) entity.TrnModCodi = dr.GetInt32(iTrnModCodi);

            //BARRCODI
            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            // CORESOCODI
            int iCoresoCodi = dr.GetOrdinal(this.CoresoCodi);
            if (!dr.IsDBNull(iCoresoCodi)) entity.CoresoCodi = dr.GetInt32(iCoresoCodi);

            // CORESOCODIGO
            int iCoresoCodigo = dr.GetOrdinal(this.CoresoCodigo);
            if (!dr.IsDBNull(iCoresoCodigo)) entity.CoresoCodigo = dr.GetString(iCoresoCodigo);

            // TRNMODRETUSEINS
            int iTrnModRetUseIns = dr.GetOrdinal(this.TrnModRetUseIns);
            if (!dr.IsDBNull(iTrnModRetUseIns)) entity.TrnModRetUseIns = dr.GetString(iTrnModRetUseIns);

            // TRNMODRETFECINS
            int iTrnModRetFecIns = dr.GetOrdinal(this.TrnModRetFecIns);
            if (!dr.IsDBNull(iTrnModRetFecIns)) entity.TrnModRetFecIns = dr.GetDateTime(iTrnModRetFecIns);

            // TRNMODRETUSEACT
            int iTrnModRetUseAct = dr.GetOrdinal(this.TrnModRetUseAct);
            if (!dr.IsDBNull(iTrnModRetUseAct)) entity.TrnModRetUseAct = dr.GetString(iTrnModRetUseAct);

            // TRNMODRETFECACT
            int iTrnModRetFecAct = dr.GetOrdinal(this.TrnModRetFecAct);
            if (!dr.IsDBNull(iTrnModRetFecAct)) entity.TrnModRetFecAct = dr.GetDateTime(iTrnModRetFecAct);
            #endregion

            #region Campos Adicionales
            int iBarrBarraTransferencia = dr.GetOrdinal(this.BarrBarraTransferencia);
            if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.BarrBarraTransferencia = dr.GetString(iBarrBarraTransferencia);
            #endregion


            return entity;
        }

        public BarraDTO CreateListarComboBarras(IDataReader dr)
        {
            BarraDTO entity = new BarraDTO();

            #region Campos Originales
            // BARRCODI
            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            // BARRBARRATRANSFERENCIA
            int iBarrBarraTransferencia = dr.GetOrdinal(this.BarrBarraTransferencia);
            if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.BarrNombBarrTran = dr.GetString(iBarrBarraTransferencia);
            #endregion


            return entity;
        }

        public CodigoRetiroDTO CreateListComboCodigoSolicitudRetiro(IDataReader dr)
        {
            CodigoRetiroDTO entity = new CodigoRetiroDTO();

            #region Campos Originales
            // CORESOCODI
            int iCoresoCodi = dr.GetOrdinal(this.CoresoCodi);
            if (!dr.IsDBNull(iCoresoCodi)) entity.SoliCodiRetiCodi = dr.GetInt32(iCoresoCodi);

            // CORESOCODIGO
            int iCoresoCodigo = dr.GetOrdinal(this.CoresoCodigo);
            if (!dr.IsDBNull(iCoresoCodigo)) entity.SoliCodiRetiCodigo = dr.GetString(iCoresoCodigo);
            #endregion

            return entity;
        }
        #endregion

        #region Mapeo de Campos Trn_Modelo_Retiro
        #region Campos Originales
        public string TrnMreCodi = "trnmrecodi";
        public string BarrCodi = "barrcodi";
        public string CoresoCodi = "coresocodi";
        public string CoresoCodigo = "coresocodigo";
        public string TrnModRetUseIns = "trnmodretuseins";
        public string TrnModRetFecIns = "trnmodretfecins";
        public string TrnModRetUseAct = "trnmodretuseact";
        public string TrnModRetFecAct = "trnmodretfecact";
        #endregion

        #region Campos Adicionales        
        public string BarrBarraTransferencia = "barrbarratransferencia";
        #endregion
        #endregion

        #region Querys SQL Trn_Modelo_Retiro
        public string SqlGetMaxIdTrnModeloRetiro
        {
            get { return base.GetSqlXml("GetMaxIdTrnModeloRetiro"); }
        }

        public string SqlSaveTrnModeloRetiro
        {
            get { return base.GetSqlXml("SaveTrnModeloRetiro"); }
        }

        public string SqlUpdateTrnModeloRetiro
        {
            get { return base.GetSqlXml("UpdateTrnModeloRetiro"); }
        }

        public string SqlDeleteTrnModeloRetiro
        {
            get { return base.GetSqlXml("DeleteTrnModeloRetiro"); }
        }

        public string SqlListTrnModeloRetiro
        {
            get { return base.GetSqlXml("ListTrnModeloRetiro"); }
        }

        public string SqlListarComboBarras
        {
            get { return base.GetSqlXml("ListarComboBarras"); }
        }

        public string SqlListComboCodigoSolicitudRetiro
        {
            get { return base.GetSqlXml("ListComboCodigoSolicitudRetiro"); }
        }

        public string SqlTieneCodigosRetiroModelo
        {
            get { return base.GetSqlXml("TieneCodigosRetiroModelo"); }
        }
        #endregion
        #endregion
    }
}
