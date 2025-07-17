using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_REPCV
    /// </summary>
    public class PrRepcvHelper : HelperBase
    {
        public PrRepcvHelper(): base(Consultas.PrRepcvSql)
        {
        }

        public PrRepcvDTO Create(IDataReader dr)
        {
            PrRepcvDTO entity = new PrRepcvDTO();

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iRepfecha = dr.GetOrdinal(this.Repfecha);
            if (!dr.IsDBNull(iRepfecha)) entity.Repfecha = dr.GetDateTime(iRepfecha);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iRepobserva = dr.GetOrdinal(this.Repobserva);
            if (!dr.IsDBNull(iRepobserva)) entity.Repobserva = dr.GetString(iRepobserva);

            int iReptipo = dr.GetOrdinal(this.Reptipo);
            if (!dr.IsDBNull(iReptipo)) entity.Reptipo = dr.GetString(iReptipo);

            int iRepnomb = dr.GetOrdinal(this.Repnomb);
            if (!dr.IsDBNull(iRepnomb)) entity.Repnomb = dr.GetString(iRepnomb);

            int iRepdetalle = dr.GetOrdinal(this.Repdetalle);
            if (!dr.IsDBNull(iRepdetalle)) entity.Repdetalle = dr.GetString(iRepdetalle);

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = dr.GetString(iDeleted);

            int iRepfechafp = dr.GetOrdinal(this.Repfechafp);
            if (!dr.IsDBNull(iRepfechafp)) entity.Repfechafp = dr.GetDateTime(iRepfechafp);

            int iRepfechaem = dr.GetOrdinal(this.Repfechaem);
            if (!dr.IsDBNull(iRepfechaem)) entity.Repfechaem = dr.GetDateTime(iRepfechaem);

            return entity;
        }


        #region Mapeo de Campos

        public string Repcodi = "REPCODI";
        public string Repfecha = "REPFECHA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Repobserva = "REPOBSERVA";
        public string Reptipo = "REPTIPO";
        public string Repnomb = "REPNOMB";
        public string Repdetalle = "REPDETALLE";
        public string Deleted = "DELETED";
        public string Repfechafp = "REPFECHAFP";
        public string Repfechaem = "REPFECHAEM";

        #endregion

        public string ObtenerReporte
        {
            get { return base.GetSqlXml("ObtenerReporte"); }
        } 
        
        #region MigracionSGOCOES-GrupoB
        public string SqlGetByFechaAndTipo
        {
            get { return base.GetSqlXml("GetByFechaAndTipo"); }
        }
        #endregion

        #region SIOSEIN2

        public string SqlObtenerReportecvYVariablesXPeriodo
        {
            get { return GetSqlXml("ObtenerReportecvYVariablesXPeriodo"); }
        }

        public string CvBase = "CVBASE";
        public string CvMedia = "CVMEDIA";
        public string Grupopadre = "GRUPOPADRECODI";
        public string Grupocodi = "GRUPOCODI";
        public string CvPunta = "CVPUNTA";

        #endregion

        public string SqlGetRepcvByEnvcodi
        {
            get { return base.GetSqlXml("GetRepcvByEnvcodi"); }
        }
    }
}
