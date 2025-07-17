using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class WbNotificacionHelper : HelperBase
    {
        public WbNotificacionHelper() : base(Consultas.WbNotificacionSql)
        {
        }

        public WbNotificacionDTO Create(IDataReader dr)
        {
            WbNotificacionDTO entity = new WbNotificacionDTO();

            int notiCodi = dr.GetOrdinal(this.NotiCodi);
            if (!dr.IsDBNull(notiCodi)) entity.NotiCodi = dr.GetInt32(notiCodi);

            int notiTitulo = dr.GetOrdinal(this.NotiTitulo);
            if (!dr.IsDBNull(notiTitulo)) entity.NotiTitulo = dr.GetString(notiTitulo);

            int notiDescripcion = dr.GetOrdinal(this.NotiDescripcion);
            if (!dr.IsDBNull(notiDescripcion)) entity.NotiDescripcion = dr.GetString(notiDescripcion);

            int notiStatus = dr.GetOrdinal(this.NotiStatus);
            if (!dr.IsDBNull(notiStatus)) entity.NotiStatus = dr.GetInt32(notiStatus);

            int notiEjecucion= dr.GetOrdinal(this.NotiEjecucion);
            if (!dr.IsDBNull(notiEjecucion)) entity.NotiEjecucion = dr.GetDateTime(notiEjecucion);

            return entity;
        }


        #region Mapeo de Campos

        public string NotiCodi = "NOTICODI";
        public string NotiTitulo = "NOTITITULO";
        public string NotiDescripcion = "NOTIDESCRIPCION";
        public string NotiStatus = "NOTISTATUS";
        public string NotiUsuCreacion = "NOTIUSUCREACION";
        public string NotiUsuModificacion = "NOTIUSUMODIFICACION";
        public string NotiEjecucion = "NOTIEJECUCION";
        public string Tiponoticodi = "TIPONOTICODI";
        public string Tiponotidesc = "TIPONOTIDESC";

        public string SqlCambiarEstadoNotificacion
        {
            get { return base.GetSqlXml("CambiarEstadoNotificacion"); }
        }

        public string SqlObtenerTipoNotificacionEventos
        {
            get { return base.GetSqlXml("ObtenerTipoNotificacionEventos"); }
        }


        #endregion
    }
}
