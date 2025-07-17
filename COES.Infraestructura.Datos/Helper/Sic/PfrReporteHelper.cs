using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_REPORTE
    /// </summary>
    public class PfrReporteHelper : HelperBase
    {
        public PfrReporteHelper(): base(Consultas.PfrReporteSql)
        {
        }

        public PfrReporteDTO Create(IDataReader dr)
        {
            PfrReporteDTO entity = new PfrReporteDTO();

            int iPfrrptcodi = dr.GetOrdinal(this.Pfrrptcodi);
            if (!dr.IsDBNull(iPfrrptcodi)) entity.Pfrrptcodi = Convert.ToInt32(dr.GetValue(iPfrrptcodi));

            int iPfrcuacodi = dr.GetOrdinal(this.Pfrcuacodi);
            if (!dr.IsDBNull(iPfrcuacodi)) entity.Pfrcuacodi = Convert.ToInt32(dr.GetValue(iPfrcuacodi));

            int iPfrreccodi = dr.GetOrdinal(this.Pfrreccodi);
            if (!dr.IsDBNull(iPfrreccodi)) entity.Pfrreccodi = Convert.ToInt32(dr.GetValue(iPfrreccodi));

            int iPfrrptesfinal = dr.GetOrdinal(this.Pfrrptesfinal);
            if (!dr.IsDBNull(iPfrrptesfinal)) entity.Pfrrptesfinal = Convert.ToInt32(dr.GetValue(iPfrrptesfinal));

            int iPfrrptusucreacion = dr.GetOrdinal(this.Pfrrptusucreacion);
            if (!dr.IsDBNull(iPfrrptusucreacion)) entity.Pfrrptusucreacion = dr.GetString(iPfrrptusucreacion);

            int iPfrrptfeccreacion = dr.GetOrdinal(this.Pfrrptfeccreacion);
            if (!dr.IsDBNull(iPfrrptfeccreacion)) entity.Pfrrptfeccreacion = dr.GetDateTime(iPfrrptfeccreacion);

            int iPfrrptcr = dr.GetOrdinal(this.Pfrrptcr);
            if (!dr.IsDBNull(iPfrrptcr)) entity.Pfrrptcr = dr.GetDecimal(iPfrrptcr);

            int iPfrrptca = dr.GetOrdinal(this.Pfrrptca);
            if (!dr.IsDBNull(iPfrrptca)) entity.Pfrrptca = dr.GetDecimal(iPfrrptca);

            int iPfrrptmr = dr.GetOrdinal(this.Pfrrptmr);
            if (!dr.IsDBNull(iPfrrptmr)) entity.Pfrrptmr = dr.GetDecimal(iPfrrptmr);

            int iPfrrptfecmd = dr.GetOrdinal(this.Pfrrptfecmd);
            if (!dr.IsDBNull(iPfrrptfecmd)) entity.Pfrrptfecmd = dr.GetDateTime(iPfrrptfecmd);

            int iPfrrptmd = dr.GetOrdinal(this.Pfrrptmd);
            if (!dr.IsDBNull(iPfrrptmd)) entity.Pfrrptmd = dr.GetDecimal(iPfrrptmd);

            int iPfrrptrevisionvtp = dr.GetOrdinal(this.Pfrrptrevisionvtp);
            if (!dr.IsDBNull(iPfrrptrevisionvtp)) entity.Pfrrptrevisionvtp = Convert.ToInt32(dr.GetValue(iPfrrptrevisionvtp));
            
            return entity;
        }


        #region Mapeo de Campos

        public string Pfrrptcodi = "PFRRPTCODI";
        public string Pfrcuacodi = "PFRCUACODI";
        public string Pfrreccodi = "PFRRECCODI";
        public string Pfrrptesfinal = "PFRRPTESFINAL";
        public string Pfrrptusucreacion = "PFRRPTUSUCREACION";
        public string Pfrrptfeccreacion = "PFRRPTFECCREACION";
        public string Pfrrptcr = "PFRRPTCR";
        public string Pfrrptca = "PFRRPTCA";
        public string Pfrrptmr = "PFRRPTMR";
        public string Pfrrptfecmd = "PFRRPTFECMD";
        public string Pfrrptmd = "PFRRPTMD";
        public string Pfrrptrevisionvtp = "PFRRPTREVISIONVTP";
        
        #endregion
    }
}
