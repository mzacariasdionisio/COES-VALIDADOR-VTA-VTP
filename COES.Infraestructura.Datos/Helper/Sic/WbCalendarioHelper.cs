using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_CALENDARIO
    /// </summary>
    public class WbCalendarioHelper : HelperBase
    {
        public WbCalendarioHelper(): base(Consultas.WbCalendarioSql)
        {
        }

        public WbCalendarioDTO Create(IDataReader dr)
        {
            WbCalendarioDTO entity = new WbCalendarioDTO();

            int iCalendicon = dr.GetOrdinal(this.Calendicon);
            if (!dr.IsDBNull(iCalendicon)) entity.Calendicon = dr.GetString(iCalendicon);

            int iCalendestado = dr.GetOrdinal(this.Calendestado);
            if (!dr.IsDBNull(iCalendestado)) entity.Calendestado = dr.GetString(iCalendestado);

            int iCalendusumodificacion = dr.GetOrdinal(this.Calendusumodificacion);
            if (!dr.IsDBNull(iCalendusumodificacion)) entity.Calendusumodificacion = dr.GetString(iCalendusumodificacion);

            int iCalendfecmodificacion = dr.GetOrdinal(this.Calendfecmodificacion);
            if (!dr.IsDBNull(iCalendfecmodificacion)) entity.Calendfecmodificacion = dr.GetDateTime(iCalendfecmodificacion);

            int iCalendcodi = dr.GetOrdinal(this.Calendcodi);
            if (!dr.IsDBNull(iCalendcodi)) entity.Calendcodi = Convert.ToInt32(dr.GetValue(iCalendcodi));

            int iCalenddescripcion = dr.GetOrdinal(this.Calenddescripcion);
            if (!dr.IsDBNull(iCalenddescripcion)) entity.Calenddescripcion = dr.GetString(iCalenddescripcion);

            int iCalendtitulo = dr.GetOrdinal(this.Calendtitulo);
            if (!dr.IsDBNull(iCalendtitulo)) entity.Calendtitulo = dr.GetString(iCalendtitulo);

            int iCalendfecinicio = dr.GetOrdinal(this.Calendfecinicio);
            if (!dr.IsDBNull(iCalendfecinicio)) entity.Calendfecinicio = dr.GetDateTime(iCalendfecinicio);

            int iCalendfecfin = dr.GetOrdinal(this.Calendfecfin);
            if (!dr.IsDBNull(iCalendfecfin)) entity.Calendfecfin = dr.GetDateTime(iCalendfecfin);

            int iCalendcolor = dr.GetOrdinal(this.Calendcolor);
            if (!dr.IsDBNull(iCalendcolor)) entity.Calendcolor = dr.GetString(iCalendcolor);

            int iCalendtipo = dr.GetOrdinal(this.Calendtipo);
            if (!dr.IsDBNull(iCalendtipo)) entity.Calendtipo = dr.GetString(iCalendtipo);

            int iTipcalcodi = dr.GetOrdinal(this.Tipcalcodi);
            if (!dr.IsDBNull(iTipcalcodi)) entity.Tipcalcodi = Convert.ToInt32(dr.GetValue(iTipcalcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Calendicon = "CALENDICON";
        public string Calendestado = "CALENDESTADO";
        public string Calendusumodificacion = "CALENDUSUMODIFICACION";
        public string Calendfecmodificacion = "CALENDFECMODIFICACION";
        public string Calendcodi = "CALENDCODI";
        public string Calenddescripcion = "CALENDDESCRIPCION";
        public string Calendtitulo = "CALENDTITULO";
        public string Calendfecinicio = "CALENDFECINICIO";
        public string Calendfecfin = "CALENDFECFIN";
        public string Calendcolor = "CALENDCOLOR";
        public string Calendtipo = "CALENDTIPO";
        public string Tipcalcodi = "TIPCALCODI";

        #endregion
    }
}
