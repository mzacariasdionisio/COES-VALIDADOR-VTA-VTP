using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_CONFIGURACION
    /// </summary>
    public class RtuConfiguracionHelper : HelperBase
    {
        public RtuConfiguracionHelper() : base(Consultas.RtuConfiguracionSql)
        {
        }

        public RtuConfiguracionDTO Create(IDataReader dr)
        {
            RtuConfiguracionDTO entity = new RtuConfiguracionDTO();

            int iRtuconcodi = dr.GetOrdinal(this.Rtuconcodi);
            if (!dr.IsDBNull(iRtuconcodi)) entity.Rtuconcodi = Convert.ToInt32(dr.GetValue(iRtuconcodi));

            int iRtuconanio = dr.GetOrdinal(this.Rtuconanio);
            if (!dr.IsDBNull(iRtuconanio)) entity.Rtuconanio = Convert.ToInt32(dr.GetValue(iRtuconanio));

            int iRtuconmes = dr.GetOrdinal(this.Rtuconmes);
            if (!dr.IsDBNull(iRtuconmes)) entity.Rtuconmes = Convert.ToInt32(dr.GetValue(iRtuconmes));

            int iRtuconusucreacion = dr.GetOrdinal(this.Rtuconusucreacion);
            if (!dr.IsDBNull(iRtuconusucreacion)) entity.Rtuconusucreacion = dr.GetString(iRtuconusucreacion);

            int iRtuconfeccreacion = dr.GetOrdinal(this.Rtuconfeccreacion);
            if (!dr.IsDBNull(iRtuconfeccreacion)) entity.Rtuconfeccreacion = dr.GetDateTime(iRtuconfeccreacion);

            int iRtuconfecmodificacion = dr.GetOrdinal(this.Rtuconfecmodificacion);
            if (!dr.IsDBNull(iRtuconfecmodificacion)) entity.Rtuconfecmodificacion = dr.GetDateTime(iRtuconfecmodificacion);

            int iRtuconusumodificacion = dr.GetOrdinal(this.Rtuconusumodificacion);
            if (!dr.IsDBNull(iRtuconusumodificacion)) entity.Rtuconusumodificacion = dr.GetString(iRtuconusumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rtuconcodi = "RTUCONCODI";
        public string Rtuconanio = "RTUCONANIO";
        public string Rtuconmes = "RTUCONMES";
        public string Rtuconusucreacion = "RTUCONUSUCREACION";
        public string Rtuconfeccreacion = "RTUCONFECCREACION";
        public string Rtuconfecmodificacion = "RTUCONFECMODIFICACION";
        public string Rtuconusumodificacion = "RTUCONUSUMODIFICACION";
        public string Pernomb = "PERNOMB";
        public string Percodi = "PERCODI";
        public string Perorden = "PERORDEN";
        public string Grupocodi = "GRUPOCODI";
        public string Grupotipo = "GRUPOTIPO";
        public string Grupoorden = "GRUPOORDEN";

        #endregion

        public string SqlObtenerConfiguracion
        {
            get { return base.GetSqlXml("ObtenerConfiguracion"); }
        }

        public string SqlGetByAnioMes
        {
            get { return base.GetSqlXml("GetByAnioMes"); }
        }

        public string SqlObtenerConfiguracionReciente
        {
            get { return base.GetSqlXml("ObtenerConfiguracionReciente"); }
        }
    }
}
