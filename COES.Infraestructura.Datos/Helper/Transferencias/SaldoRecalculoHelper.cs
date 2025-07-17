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
    /// Clase que contiene el mapeo de la tabla trn_saldo_recalculo
    /// </summary>
    public class SaldoRecalculoHelper : HelperBase
    {
       public SaldoRecalculoHelper()
            : base(Consultas.SaldoRecalculoSql)
       {
       }
        public SaldoRecalculoDTO Create(IDataReader dr)
        {
            SaldoRecalculoDTO entity = new SaldoRecalculoDTO();

            int iSALRECCODI = dr.GetOrdinal(this.SALRECCODI);
            if (!dr.IsDBNull(iSALRECCODI)) entity.SalRecCodi = dr.GetInt32(iSALRECCODI);

            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

            int iEMPCODI = dr.GetOrdinal(this.EMPCODI);
            if (!dr.IsDBNull(iEMPCODI)) entity.EmpCodi = dr.GetInt32(iEMPCODI);

            int iRECACODI = dr.GetOrdinal(this.RECACODI);
            if (!dr.IsDBNull(iRECACODI)) entity.RecaCodi = dr.GetInt32(iRECACODI);

            int iSALRECSALDO = dr.GetOrdinal(this.SALRECSALDO);
            if (!dr.IsDBNull(iSALRECSALDO)) entity.SalRecSaldo = dr.GetDecimal(iSALRECSALDO);

            int iPERICODIDESTINO = dr.GetOrdinal(this.PERICODIDESTINO);
            if (!dr.IsDBNull(iPERICODIDESTINO)) entity.PeriCodiDestino = dr.GetInt32(iPERICODIDESTINO);

            int iSALRECUSERNAME = dr.GetOrdinal(this.SALRECUSERNAME);
            if (!dr.IsDBNull(iSALRECUSERNAME)) entity.SalRecUserName = dr.GetString(iSALRECUSERNAME);

            int iSALRECFECINS = dr.GetOrdinal(this.SALRECFECINS);
            if (!dr.IsDBNull(iSALRECFECINS)) entity.SalRecFecIns = dr.GetDateTime(iSALRECFECINS);

            return entity;
        }

        #region Mapeo de Campos

        public string SALRECCODI = "SALRECCODI";
        public string EMPCODI = "EMPRCODI";
        public string PERICODI = "PERICODI";
        public string RECACODI = "RECACODI";
        public string SALRECSALDO = "SALRECSALDO";
        public string PERICODIDESTINO = "PERICODIDESTINO";
        public string SALRECUSERNAME = "SALRECUSERNAME";
        public string SALRECFECINS = "SALRECFECINS";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetByPericodiDestino 
        {
            get { return base.GetSqlXml("GetByPericodiDestino"); }
        }

        public string SqlUpdatePericodiDestino
        {
            get { return base.GetSqlXml("UpdatePericodiDestino"); }
        }
    }
}
