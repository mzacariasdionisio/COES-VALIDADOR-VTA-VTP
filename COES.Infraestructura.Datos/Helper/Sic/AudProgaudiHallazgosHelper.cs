using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PROGAUDI_HALLAZGOS
    /// </summary>
    public class AudProgaudiHallazgosHelper : HelperBase
    {
        public AudProgaudiHallazgosHelper(): base(Consultas.AudProgaudiHallazgosSql)
        {
        }

        public string SqlGetByCriteriaPorAudi
        {
            get { return base.GetSqlXml("GetByCriteriaPorAudi"); }
        }

        public string SqlObtenerNroRegistroBusquedaByAuditoria
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusquedaByAuditoria"); }
        }
        
        public AudProgaudiHallazgosDTO Create(IDataReader dr)
        {
            AudProgaudiHallazgosDTO entity = new AudProgaudiHallazgosDTO();

            int iProgahcodi = dr.GetOrdinal(this.Progahcodi);
            if (!dr.IsDBNull(iProgahcodi)) entity.Progahcodi = Convert.ToInt32(dr.GetValue(iProgahcodi));

            int iProgaecodi = dr.GetOrdinal(this.Progaecodi);
            if (!dr.IsDBNull(iProgaecodi)) entity.Progaecodi = Convert.ToInt32(dr.GetValue(iProgaecodi));

            int iArchcodianalisiscausa = dr.GetOrdinal(this.Archcodianalisiscausa);
            if (!dr.IsDBNull(iArchcodianalisiscausa)) entity.Archcodianalisiscausa = Convert.ToInt32(dr.GetValue(iArchcodianalisiscausa));

            int iArchcodievidencia = dr.GetOrdinal(this.Archcodievidencia);
            if (!dr.IsDBNull(iArchcodievidencia)) entity.Archcodievidencia = Convert.ToInt32(dr.GetValue(iArchcodievidencia));

            int iTabcdcoditipohallazgo = dr.GetOrdinal(this.Tabcdcoditipohallazgo);
            if (!dr.IsDBNull(iTabcdcoditipohallazgo)) entity.Tabcdcoditipohallazgo = Convert.ToInt32(dr.GetValue(iTabcdcoditipohallazgo));

            int iProgaicodiresponsable = dr.GetOrdinal(this.Progaicodiresponsable);
            if (!dr.IsDBNull(iProgaicodiresponsable)) entity.Progaicodiresponsable = Convert.ToInt32(dr.GetValue(iProgaicodiresponsable));

            int iProgahdescripcion = dr.GetOrdinal(this.Progahdescripcion);
            if (!dr.IsDBNull(iProgahdescripcion)) entity.Progahdescripcion = dr.GetString(iProgahdescripcion);

            int iProgahplanaccion = dr.GetOrdinal(this.Progahplanaccion);
            if (!dr.IsDBNull(iProgahplanaccion)) entity.Progahplanaccion = dr.GetString(iProgahplanaccion);

            int iProgahaccionmejora = dr.GetOrdinal(this.Progahaccionmejora);
            if (!dr.IsDBNull(iProgahaccionmejora)) entity.Progahaccionmejora = dr.GetString(iProgahaccionmejora);

            int iProgahaccionmejoraplazo = dr.GetOrdinal(this.Progahaccionmejoraplazo);
            if (!dr.IsDBNull(iProgahaccionmejoraplazo)) entity.Progahaccionmejoraplazo = dr.GetDateTime(iProgahaccionmejoraplazo);

            int iTabcdestadocodi = dr.GetOrdinal(this.Tabcdestadocodi);
            if (!dr.IsDBNull(iTabcdestadocodi)) entity.Tabcdestadocodi = Convert.ToInt32(dr.GetValue(iTabcdestadocodi));

            int iProgahactivo = dr.GetOrdinal(this.Progahactivo);
            if (!dr.IsDBNull(iProgahactivo)) entity.Progahactivo = dr.GetString(iProgahactivo);

            int iProgahhistorico = dr.GetOrdinal(this.Progahhistorico);
            if (!dr.IsDBNull(iProgahhistorico)) entity.Progahhistorico = dr.GetString(iProgahhistorico);

            int iProgahusucreacion = dr.GetOrdinal(this.Progahusucreacion);
            if (!dr.IsDBNull(iProgahusucreacion)) entity.Progahusucreacion = dr.GetString(iProgahusucreacion);

            int iProgahfeccreacion = dr.GetOrdinal(this.Progahfeccreacion);
            if (!dr.IsDBNull(iProgahfeccreacion)) entity.Progahfeccreacion = dr.GetDateTime(iProgahfeccreacion);

            int iProgahusumodificacion = dr.GetOrdinal(this.Progahusumodificacion);
            if (!dr.IsDBNull(iProgahusumodificacion)) entity.Progahusumodificacion = dr.GetString(iProgahusumodificacion);

            int iProgahfecmodificacion = dr.GetOrdinal(this.Progahfecmodificacion);
            if (!dr.IsDBNull(iProgahfecmodificacion)) entity.Progahfecmodificacion = dr.GetDateTime(iProgahfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Progahcodi = "PROGAHCODI";
        public string Progaecodi = "PROGAECODI";
        public string Archcodianalisiscausa = "ARCHCODIANALISISCAUSA";
        public string Archcodievidencia = "ARCHCODIEVIDENCIA";
        public string Tabcdcoditipohallazgo = "TABCDCODITIPOHALLAZGO";
        public string Progaicodiresponsable = "PROGAICODIRESPONSABLE";
        public string Progahdescripcion = "PROGAHDESCRIPCION";
        public string Progahplanaccion = "PROGAHPLANACCION";
        public string Progahaccionmejora = "PROGAHACCIONMEJORA";
        public string Progahaccionmejoraplazo = "PROGAHACCIONMEJORAPLAZO";
        public string Tabcdestadocodi = "TABCDESTADOCODI";
        public string Progahactivo = "PROGAHACTIVO";
        public string Progahhistorico = "PROGAHHISTORICO";
        public string Progahusucreacion = "PROGAHUSUCREACION";
        public string Progahfeccreacion = "PROGAHFECCREACION";
        public string Progahusumodificacion = "PROGAHUSUMODIFICACION";
        public string Progahfecmodificacion = "PROGAHFECMODIFICACION";

        public string TipoHallazgo = "TIPOHALLAZGO";

        public string Progacodi = "Progacodi";
        public string Elemcodigo = "Elemcodigo";
        public string Elemdescripcion = "Elemdescripcion";
        public string Estadohallazgo = "Estadohallazgo";

        public string Usercode = "Usercode";
        #endregion
    }
}
