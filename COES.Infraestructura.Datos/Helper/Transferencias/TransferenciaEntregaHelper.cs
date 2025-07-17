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
    /// Clase que contiene el mapeo de la tabla trn_trans_entrega
    /// </summary>    
    public class TransferenciaEntregaHelper : HelperBase
    {
        public TransferenciaEntregaHelper()
            : base(Consultas.TransferenciaEntregaSql)
        {
        }

        public TransferenciaEntregaDTO Create(IDataReader dr)
        {
            TransferenciaEntregaDTO entity = new TransferenciaEntregaDTO();

            int iTRANENTRCODI = dr.GetOrdinal(this.TRANENTRCODI);
            if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranEntrCodi = dr.GetInt32(iTRANENTRCODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iCODENTCODI = dr.GetOrdinal(this.CODENTCODI);
            if (!dr.IsDBNull(iCODENTCODI)) entity.CodEntCodi = dr.GetInt32(iCODENTCODI);

            int iCODIENTRCODIGO = dr.GetOrdinal(this.CODIENTRCODIGO);
            if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntrCodigo = dr.GetString(iCODIENTRCODIGO);

            int iCENTGENECODI = dr.GetOrdinal(this.CENTGENECODI);
            if (!dr.IsDBNull(iCENTGENECODI)) entity.CentGeneCodi = dr.GetInt32(iCENTGENECODI);

            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

            int iTRANENTRVERSION = dr.GetOrdinal(this.TRANENTRVERSION);
            if (!dr.IsDBNull(iTRANENTRVERSION)) entity.TranEntrVersion = dr.GetInt32(iTRANENTRVERSION);

            int iTRANENTRTIPOINFORMACION = dr.GetOrdinal(this.TRANENTRTIPOINFORMACION);
            if (!dr.IsDBNull(iTRANENTRTIPOINFORMACION)) entity.TranEntrTipoInformacion = dr.GetString(iTRANENTRTIPOINFORMACION);

            int iTRANENTRESTADO = dr.GetOrdinal(this.TRANENTRESTADO);
            if (!dr.IsDBNull(iTRANENTRESTADO)) entity.TranEntrEstado = dr.GetString(iTRANENTRESTADO);

            int iTENTUSERNAME = dr.GetOrdinal(this.TENTUSERNAME);
            if (!dr.IsDBNull(iTENTUSERNAME)) entity.TentUserName = dr.GetString(iTENTUSERNAME);

            int iTRANENTRFECINS = dr.GetOrdinal(this.TRANENTRFECINS);
            if (!dr.IsDBNull(iTRANENTRFECINS)) entity.TranEntrFecIns = dr.GetDateTime(iTRANENTRFECINS);

            int iTRANENTRFECACT = dr.GetOrdinal(this.TRANENTRFECACT);
            if (!dr.IsDBNull(iTRANENTRFECACT)) entity.TranEntrFecAct = dr.GetDateTime(iTRANENTRFECACT);

            int iTRNENVCODI = dr.GetOrdinal(this.TRNENVCODI);
            if (!dr.IsDBNull(iTRNENVCODI)) entity.TrnEnvCodi = dr.GetInt32(iTRNENVCODI);

            return entity;
        }

        #region Mapeo de Campos

        public string TRANENTRCODI = "TENTCODI";
        public string CODENTCODI = "CODENTCODI";
        public string BARRCODI = "BARRCODI";
        public string PERICODI = "PERICODI";
        public string EMPRCODI = "EMPRCODI";
        public string CENTGENECODI = "EQUICODI";
        public string CODIENTRCODIGO = "TENTCODIGO";
        public string TRANENTRVERSION = "TENTVERSION";
        public string TRANENTRTIPOINFORMACION = "TENTTIPOINFORMACION";
        public string TRANENTRESTADO = "TENTESTADO";
        public string TENTUSERNAME = "TENTUSERNAME";
        public string TRANENTRFECINS = "TENTFECINS";
        public string TRANENTRFECACT = "TENTFECACT";
        public string EMPRNOMBRE = "EMPRNOMBRE";
        public string CENTGENENOMBRE = "CENTGENENOMBRE";
        public string BARRNOMBRE = "BARRBARRATRANSFERENCIA";
        public string TOTAL = "TOTAL";
        public string TableName = "TRN_TRANS_ENTREGA";
        public string TRNENVCODI = "TRNENVCODI";
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("ListByPeriodoVersion"); }
        }

        public string SqlDeleteListaTransferenciaEntrega
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaEntrega"); }
        }

        public string SqlGetTransferenciaEntregaByCodigo
        {
            get { return base.GetSqlXml("GetTransferenciaEntregaByCodigo"); }
        }

        public string SqlCodigoGeneradoDec
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        public string SqlCopiarEntregas
        {
            get { return base.GetSqlXml("CopiarEntregas"); }
        }

        public string SqlCopiarEntregasDetalle
        {
            get { return base.GetSqlXml("CopiarEntregasDetalle"); }
        }

        public string SqlCopiarTemporal
        {
            get { return base.GetSqlXml("CopiarTemporal"); }
        }

        public string SqlGetTransferenciaEntregaByCodigoEnvio
        {
            get { return base.GetSqlXml("GetTransferenciaEntregaByCodigoEnvio"); }
        }

        public string SqlUpdateEstadoINA
        {
            get { return base.GetSqlXml("UpdateEstadoINA"); }
        }
    }
}
