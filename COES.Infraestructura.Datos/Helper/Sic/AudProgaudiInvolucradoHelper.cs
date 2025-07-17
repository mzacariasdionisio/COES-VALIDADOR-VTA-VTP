using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PROGAUDI_INVOLUCRADO
    /// </summary>
    public class AudProgaudiInvolucradoHelper : HelperBase
    {
        public AudProgaudiInvolucradoHelper(): base(Consultas.AudProgaudiInvolucradoSql)
        {
        }

        public string SqlGetByIdinvolucrado
        {
            get { return GetSqlXml("GetByIdinvolucrado"); }
        }

        public AudProgaudiInvolucradoDTO Create(IDataReader dr)
        {
            AudProgaudiInvolucradoDTO entity = new AudProgaudiInvolucradoDTO();

            int iProgaicodi = dr.GetOrdinal(this.Progaicodi);
            if (!dr.IsDBNull(iProgaicodi)) entity.Progaicodi = Convert.ToInt32(dr.GetValue(iProgaicodi));

            int iProgacodi = dr.GetOrdinal(this.Progacodi);
            if (!dr.IsDBNull(iProgacodi)) entity.Progacodi = Convert.ToInt32(dr.GetValue(iProgacodi));

            int iTabcdcoditipoinvolucrado = dr.GetOrdinal(this.Tabcdcoditipoinvolucrado);
            if (!dr.IsDBNull(iTabcdcoditipoinvolucrado)) entity.Tabcdcoditipoinvolucrado = Convert.ToInt32(dr.GetValue(iTabcdcoditipoinvolucrado));

            int iPercodiinvolucrado = dr.GetOrdinal(this.Percodiinvolucrado);
            if (!dr.IsDBNull(iPercodiinvolucrado)) entity.Percodiinvolucrado = Convert.ToInt32(dr.GetValue(iPercodiinvolucrado));

            int iProgaiactivo = dr.GetOrdinal(this.Progaiactivo);
            if (!dr.IsDBNull(iProgaiactivo)) entity.Progaiactivo = dr.GetString(iProgaiactivo);

            int iProgaihistorico = dr.GetOrdinal(this.Progaihistorico);
            if (!dr.IsDBNull(iProgaihistorico)) entity.Progaihistorico = dr.GetString(iProgaihistorico);

            int iProgaiusuregistro = dr.GetOrdinal(this.Progaiusuregistro);
            if (!dr.IsDBNull(iProgaiusuregistro)) entity.Progaiusuregistro = dr.GetString(iProgaiusuregistro);

            int iProgaifecregistro = dr.GetOrdinal(this.Progaifecregistro);
            if (!dr.IsDBNull(iProgaifecregistro)) entity.Progaifecregistro = dr.GetDateTime(iProgaifecregistro);

            int iProgaiusumodificacion = dr.GetOrdinal(this.Progaiusumodificacion);
            if (!dr.IsDBNull(iProgaiusumodificacion)) entity.Progaiusumodificacion = dr.GetString(iProgaiusumodificacion);

            int iProgaifecmodificacion = dr.GetOrdinal(this.Progaifecmodificacion);
            if (!dr.IsDBNull(iProgaifecmodificacion)) entity.Progaifecmodificacion = dr.GetDateTime(iProgaifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Progaicodi = "PROGAICODI";
        public string Progacodi = "PROGACODI";
        public string Tabcdcoditipoinvolucrado = "TABCDCODITIPOINVOLUCRADO";
        public string Percodiinvolucrado = "PERCODIINVOLUCRADO";
        public string Progaiactivo = "PROGAIACTIVO";
        public string Progaihistorico = "PROGAIHISTORICO";
        public string Progaiusuregistro = "PROGAIUSUREGISTRO";
        public string Progaifecregistro = "PROGAIFECREGISTRO";
        public string Progaiusumodificacion = "PROGAIUSUMODIFICACION";
        public string Progaifecmodificacion = "PROGAIFECMODIFICACION";

        public string Responsable = "RESPONSABLE";
        public string Peremail = "PEREMAIL";
        public string Percodi = "PERCODI";

        public string Areacodi = "AREACODI";

        #endregion
    }
}
