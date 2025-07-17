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
    /// Clase que contiene el mapeo de la tabla trn_trans_infobase
    /// </summary>    
    public class TransferenciaInformacionBaseHelper : HelperBase
    {
        public TransferenciaInformacionBaseHelper()
            : base(Consultas.TransferenciaInformacionBaseSql)
        {
        }
        public TransferenciaInformacionBaseDTO Create(IDataReader dr)
        {
            TransferenciaInformacionBaseDTO entity = new TransferenciaInformacionBaseDTO();

            int ITINFBCODI = dr.GetOrdinal(this.TINFBCODI);
            if (!dr.IsDBNull(ITINFBCODI)) entity.TinfbCodi = dr.GetInt32(ITINFBCODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iCOINFBCODI = dr.GetOrdinal(this.COINFBCODI);
            if (!dr.IsDBNull(iCOINFBCODI)) entity.CoInfbCodi = dr.GetInt32(iCOINFBCODI);

            int iTINFBCODIGO = dr.GetOrdinal(this.TINFBCODIGO);
            if (!dr.IsDBNull(iTINFBCODIGO)) entity.TinfbCodigo = dr.GetString(iTINFBCODIGO);

            int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
            if (!dr.IsDBNull(iEQUICODI)) entity.EquiCodi = dr.GetInt32(iEQUICODI);

            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

            int iTINFBVERSION = dr.GetOrdinal(this.TINFBVERSION);
            if (!dr.IsDBNull(iTINFBVERSION)) entity.TinfbVersion = dr.GetInt32(iTINFBVERSION);

            int iTINFBTIPOINFORMACION = dr.GetOrdinal(this.TINFBTIPOINFORMACION);
            if (!dr.IsDBNull(iTINFBTIPOINFORMACION)) entity.TinfbTipoInformacion = dr.GetString(iTINFBTIPOINFORMACION);

            int iTINFBESTADO = dr.GetOrdinal(this.TINFBESTADO);
            if (!dr.IsDBNull(iTINFBESTADO)) entity.TinfbEstado = dr.GetString(iTINFBESTADO);

            int iTINFBUSERNAME = dr.GetOrdinal(this.TINFBUSERNAME);
            if (!dr.IsDBNull(iTINFBUSERNAME)) entity.TinfbUserName = dr.GetString(iTINFBUSERNAME);

            int iTINFBFECINS = dr.GetOrdinal(this.TINFBFECINS);
            if (!dr.IsDBNull(iTINFBFECINS)) entity.TinfbFecIns = dr.GetDateTime(iTINFBFECINS);

            int iTINFBFECACT = dr.GetOrdinal(this.TINFBFECACT);
            if (!dr.IsDBNull(iTINFBFECACT)) entity.TinfbFecAct = dr.GetDateTime(iTINFBFECACT);

            int iTRNENVCODI = dr.GetOrdinal(this.TRNENVCODI);
            if (!dr.IsDBNull(iTRNENVCODI)) entity.TrnEnvCodi = dr.GetInt32(iTRNENVCODI);

            return entity;
        }

        #region Mapeo de Campos

        public string TINFBCODI = "TINFBCODI";
        public string COINFBCODI = "COINFBCODI";
        public string BARRCODI = "BARRCODI";
        public string PERICODI = "PERICODI";
        public string EMPRCODI = "EMPRCODI";
        public string EQUICODI = "EQUICODI";
        public string TINFBCODIGO = "TINFBCODIGO";
        public string TINFBVERSION = "TINFBVERSION";
        public string TINFBTIPOINFORMACION = "TINFBTIPOINFORMACION";
        public string TINFBESTADO = "TINFBESTADO";
        public string TINFBUSERNAME = "TINFBUSERNAME";
        public string TINFBFECINS = "TINFBFECINS";
        public string TINFBFECACT = "TINFBFECACT";
        public string EMPRNOMBRE = "EMPRNOMBRE";
        public string CENTGENENOMBRE = "CENTGENENOMBRE";
        public string BARRNOMBRE = "BARRBARRATRANSFERENCIA";
        public string TRNENVCODI = "TRNENVCODI";
        public string TOTAL = "TOTAL";

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
            get { return base.GetSqlXml("DeleteListaTransferenciaInfoBase"); }

        }

        public string SqlGetTransferenciaInfoBaseByCodigo
        {
            get { return base.GetSqlXml("GetTransferenciaInfoBaseByCodigo"); }

        }

        public string SqlGetTransferenciaInfoBaseByCodigoEnvio
        {
            get { return base.GetSqlXml("GetTransferenciaInfoBaseByCodigoEnvio"); }

        }

        public string SqlUpdateCodigo
        {
            get { return base.GetSqlXml("UpdateCodigo"); }

        }
    }

    
}
