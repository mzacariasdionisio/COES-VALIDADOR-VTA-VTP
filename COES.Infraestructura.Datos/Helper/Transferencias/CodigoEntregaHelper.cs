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
    /// Clase que contiene el mapeo de la tabla TRN_CODIGO_ENTREGA
    /// </summary>
    public class CodigoEntregaHelper: HelperBase
    {
        public CodigoEntregaHelper() : base(Consultas.CodigoEntregaSql)
        {
        }

        public CodigoEntregaDTO Create(IDataReader dr)
        {
            CodigoEntregaDTO entity = new CodigoEntregaDTO();

            int iCODIENTRCODI = dr.GetOrdinal(this.CODIENTRCODI);
            if (!dr.IsDBNull(iCODIENTRCODI)) entity.CodiEntrCodi = dr.GetInt32(iCODIENTRCODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

            int iCENTGENECODI = dr.GetOrdinal(this.CENTGENECODI);
            if (!dr.IsDBNull(iCENTGENECODI)) entity.CentGeneCodi = dr.GetInt32(iCENTGENECODI);

            int iCODIENTRCODIGO = dr.GetOrdinal(this.CODIENTRCODIGO);
            if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntrCodigo = dr.GetString(iCODIENTRCODIGO);

            int iCODIENTRFECHAINICIO = dr.GetOrdinal(this.CODIENTRFECHAINICIO);
            if (!dr.IsDBNull(iCODIENTRFECHAINICIO)) entity.CodiEntrFechaInicio = dr.GetDateTime(iCODIENTRFECHAINICIO);

            int iCODIENTRFECHAFIN = dr.GetOrdinal(this.CODIENTRFECHAFIN);
            if (!dr.IsDBNull(iCODIENTRFECHAFIN)) entity.CodiEntrFechaFin = dr.GetDateTime(iCODIENTRFECHAFIN);

            int iCODIENTRESTADO = dr.GetOrdinal(this.CODIENTRESTADO);
            if (!dr.IsDBNull(iCODIENTRESTADO)) entity.CodiEntrEstado = dr.GetString(iCODIENTRESTADO);

            int iCODIENTRUSERNAME = dr.GetOrdinal(this.CODIENTRUSERNAME);
            if (!dr.IsDBNull(iCODIENTRUSERNAME)) entity.CodiEntrUserName = dr.GetString(iCODIENTRUSERNAME);

            int iCODIENTRFECINS = dr.GetOrdinal(this.CODIENTRFECINS);
            if (!dr.IsDBNull(iCODIENTRFECINS)) entity.CodiEntrFecIns = dr.GetDateTime(iCODIENTRFECINS);

            int iCODIENTRFECACT = dr.GetOrdinal(this.CODIENTRFECACT);
            if (!dr.IsDBNull(iCODIENTRFECACT)) entity.CodiEntrFecAct = dr.GetDateTime(iCODIENTRFECACT);

            int iCENTGENENOMBRE = dr.GetOrdinal(this.CENTGENENOMBRE);
            if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneNombre = dr.GetString(iCENTGENENOMBRE);

            int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.BARRNOMBBARRTRAN);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

            int iEMPRNOMB = dr.GetOrdinal(this.EMPRNOMB);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

            return entity;
        }


        #region Mapeo de Campos
        public string CODIENTRCODI = "CODENTCODI";
        public string EMPRCODI = "EMPRCODI";
        public string BARRCODI = "BARRCODI";
        public string CENTGENECODI = "EQUICODI";
        public string CODIENTRCODIGO = "CODENTCODIGO";
        public string CODIENTRFECHAINICIO = "CODENTFECHAINICIO";
        public string CODIENTRFECHAFIN = "CODENTFECHAFIN";
        public string CODIENTRESTADO = "CODENTESTADO";
        public string CODIENTRUSERNAME = "CODENTESTADO";
        public string CODIENTRFECINS = "CODENTFECINS";
        public string CODIENTRFECACT = "CODENTFECACT";
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

        public string SqlCodigoEntregaCodigo
        {
            get { return base.GetSqlXml("GetByCodigoEntregaCodigo"); }
        }

        public string SqlCodiEntrEmpresaCodigo
        {
            get { return base.GetSqlXml("GetByCodiEntrEmpresaCodigo"); }
        }

        public string SqlCodigoEntregaVigenteByPeriodo
        {
            get { return base.GetSqlXml("CodigoEntregaVigenteByPeriodo"); }
        }
    }
}
