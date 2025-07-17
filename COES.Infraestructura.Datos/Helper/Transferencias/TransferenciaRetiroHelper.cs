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
    /// Clase que contiene el mapeo de la tabla trn_trans_retiro
    /// </summary>    
    public class TransferenciaRetiroHelper : HelperBase
    {
        public TransferenciaRetiroHelper()
            : base(Consultas.TransferenciaRetiroSql)
        {
        }

        public TransferenciaRetiroDTO Create(IDataReader dr)
        {
            TransferenciaRetiroDTO entity = new TransferenciaRetiroDTO();

            int iTRANRETICODI = dr.GetOrdinal(this.TRANRETICODI);
            if (!dr.IsDBNull(iTRANRETICODI)) entity.TranRetiCodi = dr.GetInt32(iTRANRETICODI);

            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iCLICODI = dr.GetOrdinal(this.CLICODI);
            if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

            int iTRETTABLA = dr.GetOrdinal(this.TRETTABLA);
            if (!dr.IsDBNull(iTRETTABLA)) entity.TretTabla = dr.GetString(iTRETTABLA);

            int iTRETCORESOCORESCCODI = dr.GetOrdinal(this.TRETCORESOCORESCCODI);
            if (!dr.IsDBNull(iTRETCORESOCORESCCODI)) entity.TRetCoresoCorescCodi = dr.GetInt32(iTRETCORESOCORESCCODI);

            int iSOLICODIRETICODIGO = dr.GetOrdinal(this.SOLICODIRETICODIGO);
            if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

            int iTRANRETIVERSION = dr.GetOrdinal(this.TRANRETIVERSION);
            if (!dr.IsDBNull(iTRANRETIVERSION)) entity.TranRetiVersion = dr.GetInt32(iTRANRETIVERSION);

            int iTRANRETITIPOINFORMACION = dr.GetOrdinal(this.TRANRETITIPOINFORMACION);
            if (!dr.IsDBNull(iTRANRETITIPOINFORMACION)) entity.TranRetiTipoInformacion = dr.GetString(iTRANRETITIPOINFORMACION);

            int iTRANRETIESTADO = dr.GetOrdinal(this.TRANRETIESTADO);
            if (!dr.IsDBNull(iTRANRETIESTADO)) entity.TranRetiEstado = dr.GetString(iTRANRETIESTADO);

            int iTRETUSERNAME = dr.GetOrdinal(this.TRETUSERNAME);
            if (!dr.IsDBNull(iTRETUSERNAME)) entity.TretUserName = dr.GetString(iTRETUSERNAME);

            int iTRANRETIFECINS = dr.GetOrdinal(this.TRANRETIFECINS);
            if (!dr.IsDBNull(iTRANRETIFECINS)) entity.TranRetiFecIns = dr.GetDateTime(iTRANRETIFECINS);

            int iTRANRETIFECACT = dr.GetOrdinal(this.TRANRETIFECACT);
            if (!dr.IsDBNull(iTRANRETIFECACT)) entity.TranRetiFecAct = dr.GetDateTime(iTRANRETIFECACT);

            int iTRNENVCODI = dr.GetOrdinal(this.TRNENVCODI);
            if (!dr.IsDBNull(iTRNENVCODI)) entity.TrnEnvCodi = dr.GetInt32(iTRNENVCODI);

            return entity;
        }

        #region Mapeo de Campos

        public string TRANRETICODI = "TRETCODI";
        public string PERICODI = "PERICODI";
        public string BARRCODI = "BARRCODI";
        public string EMPRCODI = "GENEMPRCODI";
        public string CLICODI = "CLIEMPRCODI";
        public string TRETTABLA = "TRETTABLA";
        public string TRETCORESOCORESCCODI = "TRETCORESOCORESCCODI";
        public string SOLICODIRETICODIGO = "TRETCODIGO";
        public string TRANRETIVERSION = "TRETVERSION";
        public string TRANRETITIPOINFORMACION = "TRETTIPOINFORMACION";
        public string TRANRETIESTADO = "TRETESTADO";
        public string TRETUSERNAME = "TRETUSERNAME";
        public string TRANRETIFECINS = "TRETFECINS";
        public string TRANRETIFECACT = "TRETFECACT";
        public string EMPRNOMBRE = "EMPRNOMBRE";
        public string CLINOMBRE = "CLINOMBRE";
        public string BARRNOMBRE = "BARRBARRATRANSFERENCIA";
        public string TOTAL = "TOTAL";
        public string TableName = "TRN_TRANS_RETIRO";
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

        public string SqlListByPeriodoVersionEmpresa
        {
            get { return base.GetSqlXml("ListByPeriodoVersionEmpresa"); }
        }

        public string SqlDeleteListaTransferenciaRetiro
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaRetiro"); }
        }

        public string SqlDeleteListaTransferenciaRetiroEmpresa
        {
            get { return base.GetSqlXml("DeleteListaTransferenciaRetiroEmpresa"); }
        }

        public string SqlGetTransferenciaRetiroByCodigo
        {
            get { return base.GetSqlXml("GetTransferenciaRetiroByCodigo"); }
        }

        public string SqlCodigoGeneradoDec
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        public string SqlCopiarRetiros
        {
            get { return base.GetSqlXml("CopiarRetiros"); }
        }

        public string SqlCopiarRetirosDetalle
        {
            get { return base.GetSqlXml("CopiarRetirosDetalle"); }
        }

        public string SqlCopiarTemporal
        {
            get { return base.GetSqlXml("CopiarTemporal"); }
        }

        public string SqlGetTransferenciaRetiroByCodigoEnvio
        {
            get { return base.GetSqlXml("GetTransferenciaRetiroByCodigoEnvio"); }
        }
        public string SqlUpdateEstadoINA
        {
            get { return base.GetSqlXml("UpdateEstadoINA"); }
        }

        //SqlGetTransferenciaRetiroBy
        public string SqlGetTransferenciaRetiroBy
        {
            get { return base.GetSqlXml("GetTransferenciaRetiroBy"); }
        }
    }
}
