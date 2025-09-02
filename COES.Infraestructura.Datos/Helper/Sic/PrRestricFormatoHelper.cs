using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_FORMATO
    /// </summary>
    public class PrRestricFormatoHelper : HelperBase
    {
        public PrRestricFormatoHelper(): base(Consultas.PrRestricFormatoSql)
        {
        }

        public PrRestricFormatoDTO Create(IDataReader dr)
        {
            PrRestricFormatoDTO entity = new PrRestricFormatoDTO();

            int iResfmtcodi = dr.GetOrdinal(this.Resfmtcodi);
            if (!dr.IsDBNull(iResfmtcodi)) entity.Resfmtcodi = Convert.ToInt32(dr.GetValue(iResfmtcodi));

            int iResfmtnombre = dr.GetOrdinal(this.Resfmtnombre);
            if (!dr.IsDBNull(iResfmtnombre)) entity.Resfmtnombre = dr.GetString(iResfmtnombre);

            int iResfmtusucreacion = dr.GetOrdinal(this.Resfmtusucreacion);
            if (!dr.IsDBNull(iResfmtusucreacion)) entity.Resfmtusucreacion = dr.GetString(iResfmtusucreacion);

            int iResfmtfeccreacion = dr.GetOrdinal(this.Resfmtfeccreacion);
            if (!dr.IsDBNull(iResfmtfeccreacion)) entity.Resfmtfeccreacion = dr.GetDateTime(iResfmtfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Resfmtcodi = "RESFMTCODI";
        public string Resfmtnombre = "RESFMTNOMBRE";
        public string Resfmtusucreacion = "RESFMTUSUCREACION";
        public string Resfmtfeccreacion = "RESFMTFECCREACION";

        #endregion
    }
}
