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
    /// Clase que contiene el mapeo de la tabla TRN_CODIGO_INFOBASE
    /// </summary>
    public class CodigoInfoBaseHelper: HelperBase
    {
        public CodigoInfoBaseHelper() : base(Consultas.CodigoInfoBaseSql)
        {
        }

        public CodigoInfoBaseDTO Create(IDataReader dr)
        {
            CodigoInfoBaseDTO entity = new CodigoInfoBaseDTO();

            int iCOINFBCODI = dr.GetOrdinal(this.COINFBCODI);
            if (!dr.IsDBNull(iCOINFBCODI)) entity.CoInfBCodi = dr.GetInt32(iCOINFBCODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iCENTGENECODI = dr.GetOrdinal(this.CENTGENECODI);
            if (!dr.IsDBNull(iCENTGENECODI)) entity.CentGeneCodi = dr.GetInt32(iCENTGENECODI);

            int iCOINFBCODIGO = dr.GetOrdinal(this.COINFBCODIGO);
            if (!dr.IsDBNull(iCOINFBCODIGO)) entity.CoInfBCodigo = dr.GetString(iCOINFBCODIGO);

            int iCOINFBFECHAINICIO = dr.GetOrdinal(this.COINFBFECHAINICIO);
            if (!dr.IsDBNull(iCOINFBFECHAINICIO)) entity.CoInfBFechaInicio = dr.GetDateTime(iCOINFBFECHAINICIO);

            int iCOINFBFECHAFIN = dr.GetOrdinal(this.COINFBFECHAFIN);
            if (!dr.IsDBNull(iCOINFBFECHAFIN)) entity.CoInfBFechaFin = dr.GetDateTime(iCOINFBFECHAFIN);

            int iCOINFBESTADO = dr.GetOrdinal(this.COINFBESTADO);
            if (!dr.IsDBNull(iCOINFBESTADO)) entity.CoInfBEstado = dr.GetString(iCOINFBESTADO);

            int iCOINFBUSERNAME = dr.GetOrdinal(this.COINFBUSERNAME);
            if (!dr.IsDBNull(iCOINFBUSERNAME)) entity.CoInfBUserName = dr.GetString(iCOINFBUSERNAME);

            int iCOINFBFECINS = dr.GetOrdinal(this.COINFBFECINS);
            if (!dr.IsDBNull(iCOINFBFECINS)) entity.CoInfBFecIns = dr.GetDateTime(iCOINFBFECINS);

            int iCOINFBFECACT = dr.GetOrdinal(this.COINFBFECACT);
            if (!dr.IsDBNull(iCOINFBFECACT)) entity.CoInfBFecAct = dr.GetDateTime(iCOINFBFECACT);

            int iCENTGENENOMBRE = dr.GetOrdinal(this.CENTGENENOMBRE);
            if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneNombre = dr.GetString(iCENTGENENOMBRE);

            int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.BARRNOMBBARRTRAN);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

            int iEMPRNOMB = dr.GetOrdinal(this.EMPRNOMB);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

            return entity;
        }
    
            #region Mapeo de Campos
            public string COINFBCODI = "COINFBCODI";
            public string EMPRCODI = "EMPRCODI";
            public string BARRCODI = "BARRCODI";
            public string CENTGENECODI = "EQUICODI";
            public string COINFBCODIGO = "COINFBCODIGO";
            public string COINFBFECHAINICIO = "COINFBFECHAINICIO";
            public string COINFBFECHAFIN = "COINFBFECHAFIN";
            public string COINFBESTADO = "COINFBESTADO";
            public string COINFBUSERNAME = "COINFBESTADO";
            public string COINFBFECINS = "COINFBFECINS";
            public string COINFBFECACT = "COINFBFECACT";    
            public string CENTGENENOMBRE = "EQUINOMB";
            public string BARRNOMBBARRTRAN = "BARRBARRATRANSFERENCIA";
            public string EMPRNOMB = "EMPRNOMB";
            public string NROPAGINA = "NROPAGINA";
            public string PAGESIZE = "PAGESIZE";

            #endregion

            public string SqlCodigoGenerado
            {
                get { return base.GetSqlXml("GetMaxId"); }
            }

            public string SqlCodigoInfoBaseCodigo
            {
                get { return base.GetSqlXml("GetByCodigoInfoBaseCodigo"); }
            }

            public string SqlCodigoInfoBaseVigenteByPeriodo
            {
                get { return base.GetSqlXml("CodigoInfoBaseVigenteByPeriodo"); }
            }
    }
}
