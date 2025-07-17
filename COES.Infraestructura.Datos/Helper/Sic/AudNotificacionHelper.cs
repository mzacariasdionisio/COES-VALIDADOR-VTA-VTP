using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_NOTIFICACION
    /// </summary>
    public class AudNotificacionHelper : HelperBase
    {
        public AudNotificacionHelper(): base(Consultas.AudNotificacionSql)
        {
        }

        public AudNotificacionDTO Create(IDataReader dr)
        {
            AudNotificacionDTO entity = new AudNotificacionDTO();

            int iNoticodi = dr.GetOrdinal(this.Noticodi);
            if (!dr.IsDBNull(iNoticodi)) entity.Noticodi = Convert.ToInt32(dr.GetValue(iNoticodi));

            int iProgacodi = dr.GetOrdinal(this.Progacodi);
            if (!dr.IsDBNull(iProgacodi)) entity.Progacodi = Convert.ToInt32(dr.GetValue(iProgacodi));

            int iArchcodiarchivoadjunto = dr.GetOrdinal(this.Archcodiarchivoadjunto);
            if (!dr.IsDBNull(iArchcodiarchivoadjunto)) entity.Archcodiarchivoadjunto = Convert.ToInt32(dr.GetValue(iArchcodiarchivoadjunto));

            int iTabcdcoditiponotificacion = dr.GetOrdinal(this.Tabcdcoditiponotificacion);
            if (!dr.IsDBNull(iTabcdcoditiponotificacion)) entity.Tabcdcoditiponotificacion = Convert.ToInt32(dr.GetValue(iTabcdcoditiponotificacion));

            int iNotimensaje = dr.GetOrdinal(this.Notimensaje);
            if (!dr.IsDBNull(iNotimensaje)) entity.Notimensaje = dr.GetString(iNotimensaje);

            int iNotiactivo = dr.GetOrdinal(this.Notiactivo);
            if (!dr.IsDBNull(iNotiactivo)) entity.Notiactivo = dr.GetString(iNotiactivo);

            int iNotihistorico = dr.GetOrdinal(this.Notihistorico);
            if (!dr.IsDBNull(iNotihistorico)) entity.Notihistorico = dr.GetString(iNotihistorico);

            int iNotiusuregistro = dr.GetOrdinal(this.Notiusuregistro);
            if (!dr.IsDBNull(iNotiusuregistro)) entity.Notiusuregistro = dr.GetString(iNotiusuregistro);

            int iNotifecregistro = dr.GetOrdinal(this.Notifecregistro);
            if (!dr.IsDBNull(iNotifecregistro)) entity.Notifecregistro = dr.GetDateTime(iNotifecregistro);

            int iNotiusumodificacion = dr.GetOrdinal(this.Notiusumodificacion);
            if (!dr.IsDBNull(iNotiusumodificacion)) entity.Notiusumodificacion = dr.GetString(iNotiusumodificacion);

            int iNotifecmodificacion = dr.GetOrdinal(this.Notifecmodificacion);
            if (!dr.IsDBNull(iNotifecmodificacion)) entity.Notifecmodificacion = dr.GetDateTime(iNotifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Noticodi = "NOTICODI";
        public string Progacodi = "PROGACODI";
        public string Archcodiarchivoadjunto = "ARCHCODIARCHIVOADJUNTO";
        public string Tabcdcoditiponotificacion = "TABCDCODITIPONOTIFICACION";
        public string Notimensaje = "NOTIMENSAJE";
        public string Notiactivo = "NOTIACTIVO";
        public string Notihistorico = "NOTIHISTORICO";
        public string Notiusuregistro = "NOTIUSUREGISTRO";
        public string Notifecregistro = "NOTIFECREGISTRO";
        public string Notiusumodificacion = "NOTIUSUMODIFICACION";
        public string Notifecmodificacion = "NOTIFECMODIFICACION";

        #endregion
    }
}
