using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ENVCORREO_CONF
    /// </summary>
    public class MeEnvcorreoConfHelper : HelperBase
    {
        public MeEnvcorreoConfHelper(): base(Consultas.MeEnvcorreoConfSql)
        {
        }

        public MeEnvcorreoConfDTO Create(IDataReader dr)
        {
            MeEnvcorreoConfDTO entity = new MeEnvcorreoConfDTO();

            int iEcconfcodi = dr.GetOrdinal(this.Ecconfcodi);
            if (!dr.IsDBNull(iEcconfcodi)) entity.Ecconfcodi = Convert.ToInt32(dr.GetValue(iEcconfcodi));

            int iEcconfnombre = dr.GetOrdinal(this.Ecconfnombre);
            if (!dr.IsDBNull(iEcconfnombre)) entity.Ecconfnombre = dr.GetString(iEcconfnombre);

            int iEcconfcargo = dr.GetOrdinal(this.Ecconfcargo);
            if (!dr.IsDBNull(iEcconfcargo)) entity.Ecconfcargo = dr.GetString(iEcconfcargo);

            int iEcconfanexo = dr.GetOrdinal(this.Ecconfanexo);
            if (!dr.IsDBNull(iEcconfanexo)) entity.Ecconfanexo = dr.GetString(iEcconfanexo);

            int iEcconfestadonot = dr.GetOrdinal(this.Ecconfestadonot);
            if (!dr.IsDBNull(iEcconfestadonot)) entity.Ecconfestadonot = dr.GetString(iEcconfestadonot);

            int iEcconfhoraenvio = dr.GetOrdinal(this.Ecconfhoraenvio);
            if (!dr.IsDBNull(iEcconfhoraenvio)) entity.Ecconfhoraenvio = dr.GetString(iEcconfhoraenvio);

            int iEcconfusucreacion = dr.GetOrdinal(this.Ecconfusucreacion);
            if (!dr.IsDBNull(iEcconfusucreacion)) entity.Ecconfusucreacion = dr.GetString(iEcconfusucreacion);

            int iEcconffeccreacion = dr.GetOrdinal(this.Ecconffeccreacion);
            if (!dr.IsDBNull(iEcconffeccreacion)) entity.Ecconffeccreacion = dr.GetDateTime(iEcconffeccreacion);

            int iEcconfusumodificacion = dr.GetOrdinal(this.Ecconfusumodificacion);
            if (!dr.IsDBNull(iEcconfusumodificacion)) entity.Ecconfusumodificacion = dr.GetString(iEcconfusumodificacion);

            int iEcconffecmodificacion = dr.GetOrdinal(this.Ecconffecmodificacion);
            if (!dr.IsDBNull(iEcconffecmodificacion)) entity.Ecconffecmodificacion = dr.GetDateTime(iEcconffecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ecconfcodi = "ECCONFCODI";
        public string Ecconfnombre = "ECCONFNOMBRE";
        public string Ecconfcargo = "ECCONFCARGO";
        public string Ecconfanexo = "ECCONFANEXO";
        public string Ecconfestadonot = "ECCONFESTADONOT";
        public string Ecconfhoraenvio = "ECCONFHORAENVIO";
        public string Ecconfusucreacion = "ECCONFUSUCREACION";
        public string Ecconffeccreacion = "ECCONFFECCREACION";
        public string Ecconfusumodificacion = "ECCONFUSUMODIFICACION";
        public string Ecconffecmodificacion = "ECCONFFECMODIFICACION";

        #endregion
    }
}
