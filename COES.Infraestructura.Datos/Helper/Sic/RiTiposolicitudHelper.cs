using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_TIPOSOLICITUD
    /// </summary>
    public class RiTiposolicitudHelper : HelperBase
    {
        public RiTiposolicitudHelper(): base(Consultas.RiTiposolicitudSql)
        {
        }

        public RiTiposolicitudDTO Create(IDataReader dr)
        {
            RiTiposolicitudDTO entity = new RiTiposolicitudDTO();

            int iTisocodi = dr.GetOrdinal(this.Tisocodi);
            if (!dr.IsDBNull(iTisocodi)) entity.Tisocodi = Convert.ToInt32(dr.GetValue(iTisocodi));

            int iTisonombre = dr.GetOrdinal(this.Tisonombre);
            if (!dr.IsDBNull(iTisonombre)) entity.Tisonombre = dr.GetString(iTisonombre);

            int iTisoestado = dr.GetOrdinal(this.Tisoestado);
            if (!dr.IsDBNull(iTisoestado)) entity.Tisoestado = dr.GetString(iTisoestado);

            int iTisousucreacion = dr.GetOrdinal(this.Tisousucreacion);
            if (!dr.IsDBNull(iTisousucreacion)) entity.Tisousucreacion = dr.GetString(iTisousucreacion);

            int iTisofeccreacion = dr.GetOrdinal(this.Tisofeccreacion);
            if (!dr.IsDBNull(iTisofeccreacion)) entity.Tisofeccreacion = dr.GetDateTime(iTisofeccreacion);

            int iTisousumodificacion = dr.GetOrdinal(this.Tisousumodificacion);
            if (!dr.IsDBNull(iTisousumodificacion)) entity.Tisousumodificacion = dr.GetString(iTisousumodificacion);

            int iTisofecmodificacion = dr.GetOrdinal(this.Tisofecmodificacion);
            if (!dr.IsDBNull(iTisofecmodificacion)) entity.Tisofecmodificacion = dr.GetDateTime(iTisofecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tisocodi = "TISOCODI";
        public string Tisonombre = "TISONOMBRE";
        public string Tisoestado = "TISOESTADO";
        public string Tisousucreacion = "TISOUSUCREACION";
        public string Tisofeccreacion = "TISOFECCREACION";
        public string Tisousumodificacion = "TISOUSUMODIFICACION";
        public string Tisofecmodificacion = "TISOFECMODIFICACION";

        #endregion
    }
}
