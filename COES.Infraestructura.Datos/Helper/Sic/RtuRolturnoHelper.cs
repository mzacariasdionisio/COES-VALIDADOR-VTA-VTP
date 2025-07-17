using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_ROLTURNO
    /// </summary>
    public class RtuRolturnoHelper : HelperBase
    {
        public RtuRolturnoHelper() : base(Consultas.RtuRolturnoSql)
        {
        }

        public RtuRolturnoDTO Create(IDataReader dr)
        {
            RtuRolturnoDTO entity = new RtuRolturnoDTO();

            int iRturolusucreacion = dr.GetOrdinal(this.Rturolusucreacion);
            if (!dr.IsDBNull(iRturolusucreacion)) entity.Rturolusucreacion = dr.GetString(iRturolusucreacion);

            int iRturolfeccreacion = dr.GetOrdinal(this.Rturolfeccreacion);
            if (!dr.IsDBNull(iRturolfeccreacion)) entity.Rturolfeccreacion = dr.GetDateTime(iRturolfeccreacion);

            int iRturolusumodificacion = dr.GetOrdinal(this.Rturolusumodificacion);
            if (!dr.IsDBNull(iRturolusumodificacion)) entity.Rturolusumodificacion = dr.GetString(iRturolusumodificacion);

            int iRturolfecmodificacion = dr.GetOrdinal(this.Rturolfecmodificacion);
            if (!dr.IsDBNull(iRturolfecmodificacion)) entity.Rturolfecmodificacion = dr.GetDateTime(iRturolfecmodificacion);

            int iRturolcodi = dr.GetOrdinal(this.Rturolcodi);
            if (!dr.IsDBNull(iRturolcodi)) entity.Rturolcodi = Convert.ToInt32(dr.GetValue(iRturolcodi));

            int iRturolanio = dr.GetOrdinal(this.Rturolanio);
            if (!dr.IsDBNull(iRturolanio)) entity.Rturolanio = Convert.ToInt32(dr.GetValue(iRturolanio));

            int iRturolmes = dr.GetOrdinal(this.Rturolmes);
            if (!dr.IsDBNull(iRturolmes)) entity.Rturolmes = Convert.ToInt32(dr.GetValue(iRturolmes));

            return entity;
        }

        #region Mapeo de Campos

        public string Rturolusucreacion = "RTUROLUSUCREACION";
        public string Rturolfeccreacion = "RTUROLFECCREACION";
        public string Rturolusumodificacion = "RTUROLUSUMODIFICACION";
        public string Rturolfecmodificacion = "RTUROLFECMODIFICACION";
        public string Rturolcodi = "RTUROLCODI";
        public string Rturolanio = "RTUROLANIO";
        public string Rturolmes = "RTUROLMES";

        public string Rtunrodia = "RTUNRODIA";
        public string Rtumodtrabajo = "RTUMODTRABAJO";
        public string Percodi = "PERCODI";
        public string Pernombre = "PERNOMBRE";
        public string Actcodi = "ACTCODI";
        public string Actnombre = "ACTNOMBRE";

        public string Rtuactabreviatura = "RTUACTABREVIATURA";

        #endregion

        public string SqlObtenerEstructura
        {
            get { return base.GetSqlXml("ObtenerEstructura"); }
        }

        public string SqlObtenerDatosPorFecha
        {
            get { return base.GetSqlXml("ObtenerDatosPorFecha"); }
        }
    }
}
