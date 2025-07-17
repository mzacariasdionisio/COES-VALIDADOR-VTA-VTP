using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_AUDPLANIFICADA_PROCESO
    /// </summary>
    public class AudAudplanificadaProcesoHelper : HelperBase
    {
        public AudAudplanificadaProcesoHelper(): base(Consultas.AudAudplanificadaProcesoSql)
        {
        }

        public string SqlDeleteByAudPlanificada
        {
            get { return GetSqlXml("DeleteByAudPlanificada"); }
        }

        public AudAudplanificadaprocesoDTO Create(IDataReader dr)
        {
            AudAudplanificadaprocesoDTO entity = new AudAudplanificadaprocesoDTO();

            int iAudppcodi = dr.GetOrdinal(this.Audppcodi);
            if (!dr.IsDBNull(iAudppcodi)) entity.Audppcodi = Convert.ToInt32(dr.GetValue(iAudppcodi));

            int iAudpcodi = dr.GetOrdinal(this.Audpcodi);
            if (!dr.IsDBNull(iAudpcodi)) entity.Audpcodi = Convert.ToInt32(dr.GetValue(iAudpcodi));

            int iProccodi = dr.GetOrdinal(this.Proccodi);
            if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

            int iAudppactivo = dr.GetOrdinal(this.Audppactivo);
            if (!dr.IsDBNull(iAudppactivo)) entity.Audppactivo = dr.GetString(iAudppactivo);

            int iAudpphistorico = dr.GetOrdinal(this.Audpphistorico);
            if (!dr.IsDBNull(iAudpphistorico)) entity.Audpphistorico = dr.GetString(iAudpphistorico);

            int iAudppusucreacion = dr.GetOrdinal(this.Audppusucreacion);
            if (!dr.IsDBNull(iAudppusucreacion)) entity.Audppusucreacion = dr.GetString(iAudppusucreacion);

            int iAudppfeccreacion = dr.GetOrdinal(this.Audppfeccreacion);
            if (!dr.IsDBNull(iAudppfeccreacion)) entity.Audppfeccreacion = dr.GetDateTime(iAudppfeccreacion);

            int iAudppusumodificacion = dr.GetOrdinal(this.Audppusumodificacion);
            if (!dr.IsDBNull(iAudppusumodificacion)) entity.Audppusumodificacion = dr.GetString(iAudppusumodificacion);

            int iAudppfecmodificacion = dr.GetOrdinal(this.Audppfecmodificacion);
            if (!dr.IsDBNull(iAudppfecmodificacion)) entity.Audppfecmodificacion = dr.GetDateTime(iAudppfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Audppcodi = "AUDPPCODI";
        public string Audpcodi = "AUDPCODI";
        public string Proccodi = "PROCCODI";
        public string Audppactivo = "AUDPPACTIVO";
        public string Audpphistorico = "AUDPPHISTORICO";
        public string Audppusucreacion = "AUDPPUSUCREACION";
        public string Audppfeccreacion = "AUDPPFECCREACION";
        public string Audppusumodificacion = "AUDPPUSUMODIFICACION";
        public string Audppfecmodificacion = "AUDPPFECMODIFICACION";

        public string Procdescripcion = "PROCDESCRIPCION";
        public string Areacodi = "AREACODI";

        #endregion
    }
}
