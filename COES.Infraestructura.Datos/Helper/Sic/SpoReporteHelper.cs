using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_REPORTE
    /// </summary>
    public class SpoReporteHelper : HelperBase
    {
        public SpoReporteHelper(): base(Consultas.SpoReporteSql)
        {
        }

        public SpoReporteDTO Create(IDataReader dr)
        {
            SpoReporteDTO entity = new SpoReporteDTO();

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iRepdiaapertura = dr.GetOrdinal(this.Repdiaapertura);
            if (!dr.IsDBNull(iRepdiaapertura)) entity.Repdiaapertura = Convert.ToInt32(dr.GetValue(iRepdiaapertura));

            int iRepdiaclausura = dr.GetOrdinal(this.Repdiaclausura);
            if (!dr.IsDBNull(iRepdiaclausura)) entity.Repdiaclausura = Convert.ToInt32(dr.GetValue(iRepdiaclausura));

            return entity;
        }


        #region Mapeo de Campos

        public string Repcodi = "REPCODI";
        public string Repdiaapertura = "REPDIAAPERTURA";
        public string Repdiaclausura = "REPDIACLAUSURA";

        #endregion
    }
}
