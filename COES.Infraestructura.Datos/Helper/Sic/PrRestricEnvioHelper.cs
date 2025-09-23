using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_ENVIO
    /// </summary>
    public class PrRestricEnvioHelper : HelperBase
    {
        public PrRestricEnvioHelper(): base(Consultas.PrRestricEnvioSql)
        {
        }

        public PrRestricEnvioDTO Create(IDataReader dr)
        {
            PrRestricEnvioDTO entity = new PrRestricEnvioDTO();

            int iResenvcodi = dr.GetOrdinal(this.Resenvcodi);
            if (!dr.IsDBNull(iResenvcodi)) entity.Resenvcodi = Convert.ToInt32(dr.GetValue(iResenvcodi));

            int iResenvfechaperiodo = dr.GetOrdinal(this.Resenvfechaperiodo);
            if (!dr.IsDBNull(iResenvfechaperiodo)) entity.Resenvfechaperiodo = dr.GetDateTime(iResenvfechaperiodo);

            int iResenvusucreacion = dr.GetOrdinal(this.Resenvusucreacion);
            if (!dr.IsDBNull(iResenvusucreacion)) entity.Resenvusucreacion = dr.GetString(iResenvusucreacion);

            int iResenvfeccreacion = dr.GetOrdinal(this.Resenvfeccreacion);
            if (!dr.IsDBNull(iResenvfeccreacion)) entity.Resenvfeccreacion = dr.GetDateTime(iResenvfeccreacion);

            int iResenvestado = dr.GetOrdinal(this.Resenvestado);
            if (!dr.IsDBNull(iResenvestado)) entity.Resenvestado = dr.GetString(iResenvestado);

            int iResfmtcodi = dr.GetOrdinal(this.Resfmtcodi);
            if (!dr.IsDBNull(iResfmtcodi)) entity.Resfmtcodi = Convert.ToInt32(dr.GetValue(iResfmtcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Resenvcodi = "RESENVCODI";
        public string Resenvfechaperiodo = "RESENVFECHAPERIODO";
        public string Resenvusucreacion = "RESENVUSUCREACION";
        public string Resenvfeccreacion = "RESENVFECCREACION";
        public string Resenvestado = "RESENVESTADO";
        public string Resfmtcodi = "RESFMTCODI";

        #endregion

        public string SqlObtenerEnvioActivo
        {
            get { return base.GetSqlXml("ObtenerEnvioActivo"); }
        }
    }
}
