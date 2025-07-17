using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
    /// </summary>
    public class SmaIndisponibilidadTemporalHelper : HelperBase
    {
        public SmaIndisponibilidadTemporalHelper(): base(Consultas.SmaIndisponibilidadTemporalSql)
        {
        }

        public SmaIndisponibilidadTemporalDTO Create(IDataReader dr)
        {
            SmaIndisponibilidadTemporalDTO entity = new SmaIndisponibilidadTemporalDTO();

            int iSmaintcodi = dr.GetOrdinal(this.Smaintcodi);
            if (!dr.IsDBNull(iSmaintcodi)) entity.Smaintcodi = Convert.ToInt32(dr.GetValue(iSmaintcodi));

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iSmaintfecha = dr.GetOrdinal(this.Smaintfecha);
            if (!dr.IsDBNull(iSmaintfecha)) entity.Smaintfecha = dr.GetDateTime(iSmaintfecha);

            int iSmaintindexiste = dr.GetOrdinal(this.Smaintindexiste);
            if (!dr.IsDBNull(iSmaintindexiste)) entity.Smaintindexiste = dr.GetString(iSmaintindexiste);

            int iSmainttipo = dr.GetOrdinal(this.Smainttipo);
            if (!dr.IsDBNull(iSmainttipo)) entity.Smainttipo = dr.GetString(iSmainttipo);

            int iSmaintbanda = dr.GetOrdinal(this.Smaintbanda);
            if (!dr.IsDBNull(iSmaintbanda)) entity.Smaintbanda = dr.GetDecimal(iSmaintbanda);

            int iSmaintmotivo = dr.GetOrdinal(this.Smaintmotivo);
            if (!dr.IsDBNull(iSmaintmotivo)) entity.Smaintmotivo = dr.GetString(iSmaintmotivo);

            int iSmaintusucreacion = dr.GetOrdinal(this.Smaintusucreacion);
            if (!dr.IsDBNull(iSmaintusucreacion)) entity.Smaintusucreacion = dr.GetString(iSmaintusucreacion);

            int iSmaintfeccreacion = dr.GetOrdinal(this.Smaintfeccreacion);
            if (!dr.IsDBNull(iSmaintfeccreacion)) entity.Smaintfeccreacion = dr.GetDateTime(iSmaintfeccreacion);

            int iSmaintusumodificacion = dr.GetOrdinal(this.Smaintusumodificacion);
            if (!dr.IsDBNull(iSmaintusumodificacion)) entity.Smaintusumodificacion = dr.GetString(iSmaintusumodificacion);

            int iSmaintfecmodificacion = dr.GetOrdinal(this.Smaintfecmodificacion);
            if (!dr.IsDBNull(iSmaintfecmodificacion)) entity.Smaintfecmodificacion = dr.GetDateTime(iSmaintfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Smaintcodi = "SMAINTCODI";
        public string Urscodi = "URSCODI";
        public string Smaintfecha = "SMAINTFECHA";
        public string Smaintindexiste = "SMAINTINDEXISTE";
        public string Smainttipo = "SMAINTTIPO";
        public string Smaintbanda = "SMAINTBANDA";
        public string Smaintmotivo = "SMAINTMOTIVO";
        public string Smaintusucreacion = "SMAINTUSUCREACION";
        public string Smaintfeccreacion = "SMAINTFECCREACION";
        public string Smaintusumodificacion = "SMAINTUSUMODIFICACION";
        public string Smaintfecmodificacion = "SMAINTFECMODIFICACION";

        #endregion

        public string SqlListarPorFecha
        {
            get { return base.GetSqlXml("ListarPorFecha"); }
        }
        
    }
}
