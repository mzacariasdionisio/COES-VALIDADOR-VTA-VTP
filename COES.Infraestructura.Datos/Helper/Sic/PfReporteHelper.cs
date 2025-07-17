using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_REPORTE
    /// </summary>
    public class PfReporteHelper : HelperBase
    {
        public PfReporteHelper(): base(Consultas.PfReporteSql)
        {
        }

        public PfReporteDTO Create(IDataReader dr)
        {
            PfReporteDTO entity = new PfReporteDTO();

            int iPfrptcodi = dr.GetOrdinal(this.Pfrptcodi);
            if (!dr.IsDBNull(iPfrptcodi)) entity.Pfrptcodi = Convert.ToInt32(dr.GetValue(iPfrptcodi));

            int iPfrptusucreacion = dr.GetOrdinal(this.Pfrptusucreacion);
            if (!dr.IsDBNull(iPfrptusucreacion)) entity.Pfrptusucreacion = dr.GetString(iPfrptusucreacion);

            int iPfrptfeccreacion = dr.GetOrdinal(this.Pfrptfeccreacion);
            if (!dr.IsDBNull(iPfrptfeccreacion)) entity.Pfrptfeccreacion = dr.GetDateTime(iPfrptfeccreacion);

            int iPfrptesfinal = dr.GetOrdinal(this.Pfrptesfinal);
            if (!dr.IsDBNull(iPfrptesfinal)) entity.Pfrptesfinal = Convert.ToInt32(dr.GetValue(iPfrptesfinal));

            int iPfrecacodi = dr.GetOrdinal(this.Pfrecacodi);
            if (!dr.IsDBNull(iPfrecacodi)) entity.Pfrecacodi = Convert.ToInt32(dr.GetValue(iPfrecacodi));

            int iPfcuacodi = dr.GetOrdinal(this.Pfcuacodi);
            if (!dr.IsDBNull(iPfcuacodi)) entity.Pfcuacodi = Convert.ToInt32(dr.GetValue(iPfcuacodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrptcodi = "PFRPTCODI";
        public string Pfrptusucreacion = "PFRPTUSUCREACION";
        public string Pfrptfeccreacion = "PFRPTFECCREACION";
        public string Pfrptesfinal = "PFRPTESFINAL";
        public string Pfrecacodi = "PFRECACODI";
        public string Pfcuacodi = "PFCUACODI";

        #endregion
    }
}
