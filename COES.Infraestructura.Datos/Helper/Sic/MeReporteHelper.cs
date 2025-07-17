using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_REPORTE
    /// </summary>
    public class MeReporteHelper : HelperBase
    {
        public MeReporteHelper()
            : base(Consultas.MeReporteSql)
        {
        }

        public MeReporteDTO Create(IDataReader dr)
        {
            MeReporteDTO entity = new MeReporteDTO();

            int iReporcodi = dr.GetOrdinal(this.Reporcodi);
            if (!dr.IsDBNull(iReporcodi)) entity.Reporcodi = Convert.ToInt32(dr.GetValue(iReporcodi));

            int iRepornombre = dr.GetOrdinal(this.Repornombre);
            if (!dr.IsDBNull(iRepornombre)) entity.Repornombre = dr.GetString(iRepornombre);

            int iRepordescrip = dr.GetOrdinal(this.Repordescrip);
            if (!dr.IsDBNull(iRepordescrip)) entity.Repordescrip = dr.GetString(iRepordescrip);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iReporusucreacion = dr.GetOrdinal(this.Reporusucreacion);
            if (!dr.IsDBNull(iReporusucreacion)) entity.Reporusucreacion = dr.GetString(iReporusucreacion);

            int iReporfeccreacion = dr.GetOrdinal(this.Reporfeccreacion);
            if (!dr.IsDBNull(iReporfeccreacion)) entity.Reporfeccreacion = dr.GetDateTime(iReporfeccreacion);

            int iReporusumodificacion = dr.GetOrdinal(this.Reporusumodificacion);
            if (!dr.IsDBNull(iReporusumodificacion)) entity.Reporusumodificacion = dr.GetString(iReporusumodificacion);

            int iReporfecmodificacion = dr.GetOrdinal(this.Reporfecmodificacion);
            if (!dr.IsDBNull(iReporfecmodificacion)) entity.Reporfecmodificacion = dr.GetDateTime(iReporfecmodificacion);

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iCabcodi = dr.GetOrdinal(this.Cabcodi);
            if (!dr.IsDBNull(iCabcodi)) entity.Cabcodi = Convert.ToInt32(dr.GetValue(iCabcodi));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iReporcheckempresa = dr.GetOrdinal(this.Reporcheckempresa);
            if (!dr.IsDBNull(iReporcheckempresa)) entity.Reporcheckempresa = Convert.ToInt32(dr.GetValue(iReporcheckempresa));

            int iReporcheckequipo = dr.GetOrdinal(this.Reporcheckequipo);
            if (!dr.IsDBNull(iReporcheckequipo)) entity.Reporcheckequipo = Convert.ToInt32(dr.GetValue(iReporcheckequipo));

            int ireporchecktipoequipo = dr.GetOrdinal(this.Reporchecktipoequipo);
            if (!dr.IsDBNull(ireporchecktipoequipo)) entity.Reporchecktipoequipo = Convert.ToInt32(dr.GetValue(ireporchecktipoequipo));

            int ireporchecktipomedida = dr.GetOrdinal(this.Reporchecktipomedida);
            if (!dr.IsDBNull(ireporchecktipomedida)) entity.Reporchecktipomedida = Convert.ToInt32(dr.GetValue(ireporchecktipomedida));

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iReporejey = dr.GetOrdinal(this.Reporejey);
            if (!dr.IsDBNull(iReporejey)) entity.Reporejey = dr.GetString(iReporejey);

            int iReporesgrafico = dr.GetOrdinal(this.Reporesgrafico);
            if (!dr.IsDBNull(iReporesgrafico)) entity.Reporesgrafico = dr.GetString(iReporesgrafico);

            return entity;
        }


        #region Mapeo de Campos

        public string Reporcodi = "REPORCODI";
        public string Repornombre = "REPORNOMBRE";
        public string Repordescrip = "REPORDESCRIP";
        public string Lectcodi = "LECTCODI";
        public string Reporusucreacion = "REPORUSUCREACION";
        public string Reporfeccreacion = "REPORFECCREACION";
        public string Reporusumodificacion = "REPORUSUMODIFICACION";
        public string Reporfecmodificacion = "REPORFECMODIFICACION";
        public string Modcodi = "MODCODI";
        public string Modnombre = "MODNOMBRE";
        public string Cabcodi = "CABCODI";
        public string Areacode = "AREACODE";
        public string Areaabrev = "AREAABREV";
        public string Areaname = "AREANAME";
        public string Reporcheckempresa = "REPORCHECKEMPRESA";
        public string Reporcheckequipo = "REPORCHECKEQUIPO";
        public string Reporchecktipoequipo = "REPORCHECKTIPOEQUIPO";
        public string Reporchecktipomedida = "REPORCHECKTIPOMEDIDA";
        public string Reptiprepcodi = "tmrepcodi";
        public string Mrepcodi = "Mrepcodi";
        public string Reporejey = "REPOREJEY";
        public string Reporesgrafico = "REPORESGRAFICO";

        #endregion

        public string SqlListarXModulo
        {
            get { return base.GetSqlXml("ListarXModulo"); }
        }

        public string SqlListarXArea
        {
            get { return base.GetSqlXml("ListarXArea"); }
        }

        public string SqlListarXAreaYModulo
        {
            get { return base.GetSqlXml("ListarXAreaYModulo"); }
        }
    }
}
