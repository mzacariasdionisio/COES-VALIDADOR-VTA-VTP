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
    /// Clase que contiene el mapeo de la tabla RCG_COSTOMARGINAL_CAB
    /// </summary>
    public class RcgCostoMarginalCabHelper : HelperBase
    {
        public RcgCostoMarginalCabHelper()
            : base(Consultas.RcgCostoMarginalCabSql)
        {
        }

        public RcgCostoMarginalCabDTO Create(IDataReader dr)
        {
            RcgCostoMarginalCabDTO entity = new RcgCostoMarginalCabDTO();

            int iRCCMGCCODI = dr.GetOrdinal(this.RCCMGCCODI);
            if (!dr.IsDBNull(iRCCMGCCODI)) entity.Rccmgccodi = dr.GetInt32(iRCCMGCCODI);            

            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.Pericodi = dr.GetInt32(iPERICODI);

            int iRECACODI = dr.GetOrdinal(this.RECACODI);
            if (!dr.IsDBNull(iRECACODI)) entity.Recacodi = dr.GetInt32(iRECACODI);

            int iRCCMGCUSUCREACION = dr.GetOrdinal(this.RCCMGCUSUCREACION);
            if (!dr.IsDBNull(iRCCMGCUSUCREACION)) entity.Rccmgcusucreacion = dr.GetString(iRCCMGCUSUCREACION);

            int iRCCMGCFECCREACION = dr.GetOrdinal(this.RCCMGCFECCREACION);
            if (!dr.IsDBNull(iRCCMGCFECCREACION)) entity.Rccmgcfeccreacion = dr.GetDateTime(iRCCMGCFECCREACION);   

           
            return entity;

        }

        #region Mapeo de Campos

        public string RCCMGCCODI = "RCCMGCCODI";
        public string PERICODI = "PERICODI";
        public string RECACODI = "RECACODI";
        public string RCCMGCUSUCREACION = "RCCMGCUSUCREACION";
        public string RCCMGCFECCREACION = "RCCMGCFECCREACION";
        public string RCCMGCUSUMODIFICACION = "RCCMGCUSUMODIFICACION";
        public string RCCMGCFECMODIFICACION = "RCCMGCFECMODIFICACION";        

        #endregion

        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListCostoMarginalCab
        {
            get { return base.GetSqlXml("ListCostoMarginalCab"); }
        }
    }
}
