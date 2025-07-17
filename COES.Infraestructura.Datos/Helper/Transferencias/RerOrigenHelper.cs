using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_ORIGEN
    /// </summary>
    public class RerOrigenHelper : HelperBase
    {
        public RerOrigenHelper() : base(Consultas.RerOrigenSql)
        {
        }

        public RerOrigenDTO Create(IDataReader dr)
        {
            RerOrigenDTO entity = new RerOrigenDTO();

            int iReroricodi = dr.GetOrdinal(this.Reroricodi);
            if (!dr.IsDBNull(iReroricodi)) entity.Reroricodi = Convert.ToInt32(dr.GetValue(iReroricodi));

            int iReroridesc = dr.GetOrdinal(this.Reroridesc);
            if (!dr.IsDBNull(iReroridesc)) entity.Reroridesc = dr.GetString(iReroridesc);

            int iReroriusucreacion = dr.GetOrdinal(this.Reroriusucreacion);
            if (!dr.IsDBNull(iReroriusucreacion)) entity.Reroriusucreacion = dr.GetString(iReroriusucreacion);

            int iRerorifeccreacion = dr.GetOrdinal(this.Rerorifeccreacion);
            if (!dr.IsDBNull(iRerorifeccreacion)) entity.Rerorifeccreacion = dr.GetDateTime(iRerorifeccreacion);

            int iReroriusumodificacion = dr.GetOrdinal(this.Reroriusumodificacion);
            if (!dr.IsDBNull(iReroriusumodificacion)) entity.Reroriusumodificacion = dr.GetString(iReroriusumodificacion);

            int iRerorifecmodificacion = dr.GetOrdinal(this.Rerorifecmodificacion);
            if (!dr.IsDBNull(iRerorifecmodificacion)) entity.Rerorifecmodificacion = dr.GetDateTime(iRerorifecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Reroricodi = "RERORICODI";
        public string Reroridesc = "RERORIDESC";
        public string Reroriusucreacion = "RERORIUSUCREACION";
        public string Rerorifeccreacion = "RERORIFECCREACION";
        public string Reroriusumodificacion = "RERORIUSUMODIFICACION";
        public string Rerorifecmodificacion = "RERORIFECMODIFICACION";

        #endregion
    }
}