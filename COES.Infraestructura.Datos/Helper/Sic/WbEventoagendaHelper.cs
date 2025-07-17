using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_EVENTOAGENDA
    /// </summary>
    public class WbEventoagendaHelper : HelperBase
    {
        public WbEventoagendaHelper(): base(Consultas.WbEventoagendaSql)
        {
        }

        public WbEventoagendaDTO Create(IDataReader dr)
        {
            WbEventoagendaDTO entity = new WbEventoagendaDTO();

            int iEveagcodi = dr.GetOrdinal(this.Eveagcodi);
            if (!dr.IsDBNull(iEveagcodi)) entity.Eveagcodi = Convert.ToInt32(dr.GetValue(iEveagcodi));

            int iEveagtipo = dr.GetOrdinal(this.Eveagtipo);
            if (!dr.IsDBNull(iEveagtipo)) entity.Eveagtipo = Convert.ToInt32(dr.GetValue(iEveagtipo));

            int iEveagfechinicio = dr.GetOrdinal(this.Eveagfechinicio);
            if (!dr.IsDBNull(iEveagfechinicio)) entity.Eveagfechinicio = dr.GetDateTime(iEveagfechinicio);

            int iEveagfechfin = dr.GetOrdinal(this.Eveagfechfin);
            if (!dr.IsDBNull(iEveagfechfin)) entity.Eveagfechfin = dr.GetDateTime(iEveagfechfin);

            int iEveagubicacion = dr.GetOrdinal(this.Eveagubicacion);
            if (!dr.IsDBNull(iEveagubicacion)) entity.Eveagubicacion = dr.GetString(iEveagubicacion);

            int iEveagextension = dr.GetOrdinal(this.Eveagextension);
            if (!dr.IsDBNull(iEveagextension)) entity.Eveagextension = dr.GetString(iEveagextension);

            int iEveagusuariocreacion = dr.GetOrdinal(this.Eveagusuariocreacion);
            if (!dr.IsDBNull(iEveagusuariocreacion)) entity.Eveagusuariocreacion = dr.GetString(iEveagusuariocreacion);

            int iEveagfechacreacion = dr.GetOrdinal(this.Eveagfechacreacion);
            if (!dr.IsDBNull(iEveagfechacreacion)) entity.Eveagfechacreacion = dr.GetDateTime(iEveagfechacreacion);

            int iEveagusuarioupdate = dr.GetOrdinal(this.Eveagusuarioupdate);
            if (!dr.IsDBNull(iEveagusuarioupdate)) entity.Eveagusuarioupdate = dr.GetString(iEveagusuarioupdate);

            int iEveagfechaupdate = dr.GetOrdinal(this.Eveagfechaupdate);
            if (!dr.IsDBNull(iEveagfechaupdate)) entity.Eveagfechaupdate = dr.GetDateTime(iEveagfechaupdate);

            int iEveagtitulo = dr.GetOrdinal(this.Eveagtitulo);
            if (!dr.IsDBNull(iEveagtitulo)) entity.Eveagtitulo = dr.GetString(iEveagtitulo);

            int iEveagdescripcion = dr.GetOrdinal(this.Eveagdescripcion);
            if (!dr.IsDBNull(iEveagdescripcion)) entity.Eveagdescripcion = dr.GetString(iEveagdescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Eveagcodi = "EVEAGCODI";
        public string Eveagtipo = "EVEAGTIPO";
        public string Eveagfechinicio = "EVEAGFECHINICIO";
        public string Eveagfechfin = "EVEAGFECHFIN";
        public string Eveagubicacion = "EVEAGUBICACION";
        public string Eveagextension = "EVEAGEXTENSION";
        public string Eveagusuariocreacion = "EVEAGUSUARIOCREACION";
        public string Eveagfechacreacion = "EVEAGFECHACREACION";
        public string Eveagusuarioupdate = "EVEAGUSUARIOUPDATE";
        public string Eveagfechaupdate = "EVEAGFECHAUPDATE";
        public string Eveagtitulo = "EVEAGTITULO";
        public string Eveagdescripcion = "EVEAGDESCRIPCION";

        #endregion
    }
}
