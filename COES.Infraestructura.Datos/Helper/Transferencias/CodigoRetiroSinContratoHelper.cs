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
    /// Clase que contiene el mapeo de la tabla TRN_CODIGO_RETIRO_SINCONTRATO
    /// </summary>
    public class CodigoRetiroSinContratoHelper : HelperBase
    {
        public CodigoRetiroSinContratoHelper()   : base(Consultas.CodigoRetiroSinContratoSql)
        {
        }
        public CodigoRetiroSinContratoDTO Create(IDataReader dr)
        {
            CodigoRetiroSinContratoDTO entity = new CodigoRetiroSinContratoDTO();

            int iCODRETISINCONCODI = dr.GetOrdinal(this.CODRETISINCONCODI);
            if (!dr.IsDBNull(iCODRETISINCONCODI)) entity.CodRetiSinConCodi = dr.GetInt32(iCODRETISINCONCODI);

            int iCLICODI = dr.GetOrdinal(this.CLICODI);
            if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iCODRETISINCONCODIGO = dr.GetOrdinal(this.CODRETISINCONCODIGO);
            if (!dr.IsDBNull(iCODRETISINCONCODIGO)) entity.CodRetiSinConCodigo = dr.GetString(iCODRETISINCONCODIGO);

            int iCODRETISINCONIFECHAINICIO = dr.GetOrdinal(this.CODRETISINCONIFECHAINICIO);
            if (!dr.IsDBNull(iCODRETISINCONIFECHAINICIO)) entity.CodRetiSinConFechaInicio = dr.GetDateTime(iCODRETISINCONIFECHAINICIO);

            int iCODRETISINCONFECHAFIN = dr.GetOrdinal(this.CODRETISINCONFECHAFIN);
            if (!dr.IsDBNull(iCODRETISINCONFECHAFIN)) entity.CodRetiSinConFechaFin = dr.GetDateTime(iCODRETISINCONFECHAFIN);

            int iCODRETISINCONESTADO = dr.GetOrdinal(this.CODRETISINCONESTADO);
            if (!dr.IsDBNull(iCODRETISINCONESTADO)) entity.CodRetiSinConEstado = dr.GetString(iCODRETISINCONESTADO);

            int iCODRETISINCONUSERNAME = dr.GetOrdinal(this.CODRETISINCONUSERNAME);
            if (!dr.IsDBNull(iCODRETISINCONUSERNAME)) entity.CodRetiSinConUserName = dr.GetString(iCODRETISINCONUSERNAME);

            int iCODRETISINCONFECINS = dr.GetOrdinal(this.CODRETISINCONFECINS);
            if (!dr.IsDBNull(iCODRETISINCONFECINS)) entity.CodRetiSinConFecIns = dr.GetDateTime(iCODRETISINCONFECINS);

            int iCODRETISINCONFECACT = dr.GetOrdinal(this.CODRETISINCONFECACT);
            if (!dr.IsDBNull(iCODRETISINCONFECACT)) entity.CodRetiSinConFecAct = dr.GetDateTime(iCODRETISINCONFECACT);

            try
            {
                int iEMPRRUC = dr.GetOrdinal(this.EMPRRUC);
                if (!dr.IsDBNull(iEMPRRUC)) entity.CliRuc = dr.GetString(iEMPRRUC);
            }
            catch { }

            int iCLINOMBRE = dr.GetOrdinal(this.CLINOMBRE);
            if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);

            int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.BARRNOMBBARRTRAN);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

            int iTIPUSUCODI = dr.GetOrdinal(this.TIPUSUCODI);
            if (!dr.IsDBNull(iTIPUSUCODI)) entity.TipUsuCodi = dr.GetInt32(iTIPUSUCODI);

            int iTIPUSUNOMB = dr.GetOrdinal(this.TIPUSUNOMB);
            if (!dr.IsDBNull(iTIPUSUNOMB)) entity.TipUsuNomb = dr.GetString(iTIPUSUNOMB);

            return entity;

        }

        #region Mapeo de Campos

        public string CODRETISINCONCODI = "CORESCCODI";
        public string CLICODI = "CLIEMPRCODI";
        public string BARRCODI = "BARRCODI";
        public string CODRETISINCONCODIGO = "CORESCCODIGO";       
        public string CODRETISINCONIFECHAINICIO = "CORESCFECHAINICIO";
        public string CODRETISINCONFECHAFIN = "CORESCFECHAFIN";     
        public string CODRETISINCONESTADO = "CORESCESTADO";
        public string CODRETISINCONUSERNAME = "CORESCUSERNAME";
        public string CODRETISINCONFECINS = "CORESCFECINS";
        public string CODRETISINCONFECACT = "CORESCFECACT";
        public string GENEMPRCODI = "GENEMPRCODI";        
        public string EMPRRUC = "EMPRRUC";
        public string CLINOMBRE = "EMPRNOMB";
        public string BARRNOMBBARRTRAN = "BARRBARRATRANSFERENCIA";
        public string NROPAGINA = "NROPAGINA";
        public string PAGESIZE = "PAGESIZE";
        public string TIPUSUCODI = "TIPUSUCODI";
        public string TIPUSUNOMB = "TIPUSUNOMB";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlCodigoRetiroSinContratoCodigo
        {
            get { return base.GetSqlXml("GetByCodigoRetiroSinContratoCodigo"); }
        }

    }
}
