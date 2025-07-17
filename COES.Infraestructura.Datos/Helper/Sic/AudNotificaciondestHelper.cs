using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_NOTIFICACIONDEST
    /// </summary>
    public class AudNotificaciondestHelper : HelperBase
    {
        public AudNotificaciondestHelper(): base(Consultas.AudNotificaciondestSql)
        {
        }

        public AudNotificaciondestDTO Create(IDataReader dr)
        {
            AudNotificaciondestDTO entity = new AudNotificaciondestDTO();

            int iNotdcodi = dr.GetOrdinal(this.Notdcodi);
            if (!dr.IsDBNull(iNotdcodi)) entity.Notdcodi = Convert.ToInt32(dr.GetValue(iNotdcodi));

            int iNoticodi = dr.GetOrdinal(this.Noticodi);
            if (!dr.IsDBNull(iNoticodi)) entity.Noticodi = Convert.ToInt32(dr.GetValue(iNoticodi));

            int iPercodidestinatario = dr.GetOrdinal(this.Percodidestinatario);
            if (!dr.IsDBNull(iPercodidestinatario)) entity.Percodidestinatario = Convert.ToInt32(dr.GetValue(iPercodidestinatario));

            int iTabcdcoditipodestinatario = dr.GetOrdinal(this.Tabcdcoditipodestinatario);
            if (!dr.IsDBNull(iTabcdcoditipodestinatario)) entity.Tabcdcoditipodestinatario = Convert.ToInt32(dr.GetValue(iTabcdcoditipodestinatario));

            int iNotdactivo = dr.GetOrdinal(this.Notdactivo);
            if (!dr.IsDBNull(iNotdactivo)) entity.Notdactivo = dr.GetString(iNotdactivo);

            int iNotdhistorico = dr.GetOrdinal(this.Notdhistorico);
            if (!dr.IsDBNull(iNotdhistorico)) entity.Notdhistorico = dr.GetString(iNotdhistorico);

            int iNotdusucreacion = dr.GetOrdinal(this.Notdusucreacion);
            if (!dr.IsDBNull(iNotdusucreacion)) entity.Notdusucreacion = dr.GetString(iNotdusucreacion);

            int iNotdfeccreacion = dr.GetOrdinal(this.Notdfeccreacion);
            if (!dr.IsDBNull(iNotdfeccreacion)) entity.Notdfeccreacion = dr.GetDateTime(iNotdfeccreacion);

            int iNotdusumodificacion = dr.GetOrdinal(this.Notdusumodificacion);
            if (!dr.IsDBNull(iNotdusumodificacion)) entity.Notdusumodificacion = dr.GetString(iNotdusumodificacion);

            int iNotdfecmodificacion = dr.GetOrdinal(this.Notdfecmodificacion);
            if (!dr.IsDBNull(iNotdfecmodificacion)) entity.Notdfecmodificacion = dr.GetDateTime(iNotdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Notdcodi = "NOTDCODI";
        public string Noticodi = "NOTICODI";
        public string Percodidestinatario = "PERCODIDESTINATARIO";
        public string Tabcdcoditipodestinatario = "TABCDCODITIPODESTINATARIO";
        public string Notdactivo = "NOTDACTIVO";
        public string Notdhistorico = "NOTDHISTORICO";
        public string Notdusucreacion = "NOTDUSUCREACION";
        public string Notdfeccreacion = "NOTDFECCREACION";
        public string Notdusumodificacion = "NOTDUSUMODIFICACION";
        public string Notdfecmodificacion = "NOTDFECMODIFICACION";

        #endregion
    }
}
