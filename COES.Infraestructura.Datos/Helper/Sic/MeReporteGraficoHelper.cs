using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_REPORTE_GRAFICO
    /// </summary>
    public class MeReporteGraficoHelper : HelperBase
    {
        public MeReporteGraficoHelper(): base(Consultas.MeReporteGraficoSql)
        {
        }

        public MeReporteGraficoDTO Create(IDataReader dr)
        {
            MeReporteGraficoDTO entity = new MeReporteGraficoDTO();

            int iRepgrcodi = dr.GetOrdinal(this.Repgrcodi);
            if (!dr.IsDBNull(iRepgrcodi)) entity.Repgrcodi = Convert.ToInt32(dr.GetValue(iRepgrcodi));

            int iRepgrname = dr.GetOrdinal(this.Repgrname);
            if (!dr.IsDBNull(iRepgrname)) entity.Repgrname = dr.GetString(iRepgrname);

            int iRepgrtype = dr.GetOrdinal(this.Repgrtype);
            if (!dr.IsDBNull(iRepgrtype)) entity.Repgrtype = dr.GetString(iRepgrtype);

            int iRepgryaxis = dr.GetOrdinal(this.Repgryaxis);
            if (!dr.IsDBNull(iRepgryaxis)) entity.Repgryaxis = Convert.ToInt32(dr.GetValue(iRepgryaxis));

            int iRepgrcolor = dr.GetOrdinal(this.Repgrcolor);
            if (!dr.IsDBNull(iRepgrcolor)) entity.Repgrcolor = dr.GetString(iRepgrcolor);

            int iReporcodi = dr.GetOrdinal(this.Reporcodi);
            if (!dr.IsDBNull(iReporcodi)) entity.Reporcodi = Convert.ToInt32(dr.GetValue(iReporcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Repgrcodi = "REPGRCODI";
        public string Repgrname = "REPGRNAME";
        public string Repgrtype = "REPGRTYPE";
        public string Repgryaxis = "REPGRYAXIS";
        public string Repgrcolor = "REPGRCOLOR";
        public string Reporcodi = "REPORCODI";
        public string Ptomedicodi = "PTOMEDICODI";

        #endregion
    }
}
