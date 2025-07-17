using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_VERSION
    /// </summary>
    public class MmmVersionHelper : HelperBase
    {
        public MmmVersionHelper()
            : base(Consultas.MmmVersionSql)
        {
        }

        public MmmVersionDTO Create(IDataReader dr)
        {
            MmmVersionDTO entity = new MmmVersionDTO();

            int iVermmcodi = dr.GetOrdinal(this.Vermmcodi);
            if (!dr.IsDBNull(iVermmcodi)) entity.Vermmcodi = Convert.ToInt32(dr.GetValue(iVermmcodi));

            int iVermmfechaperiodo = dr.GetOrdinal(this.Vermmfechaperiodo);
            if (!dr.IsDBNull(iVermmfechaperiodo)) entity.Vermmfechaperiodo = dr.GetDateTime(iVermmfechaperiodo);

            int iVermmusucreacion = dr.GetOrdinal(this.Vermmusucreacion);
            if (!dr.IsDBNull(iVermmusucreacion)) entity.Vermmusucreacion = dr.GetString(iVermmusucreacion);

            int iVermmestado = dr.GetOrdinal(this.Vermmestado);
            if (!dr.IsDBNull(iVermmestado)) entity.Vermmestado = Convert.ToInt32(dr.GetValue(iVermmestado));

            int iVermmfeccreacion = dr.GetOrdinal(this.Vermmfeccreacion);
            if (!dr.IsDBNull(iVermmfeccreacion)) entity.Vermmfeccreacion = dr.GetDateTime(iVermmfeccreacion);

            int iVermmusumodificacion = dr.GetOrdinal(this.Vermmusumodificacion);
            if (!dr.IsDBNull(iVermmusumodificacion)) entity.Vermmusumodificacion = dr.GetString(iVermmusumodificacion);

            int iVermmfecmodificacion = dr.GetOrdinal(this.Vermmfecmodificacion);
            if (!dr.IsDBNull(iVermmfecmodificacion)) entity.Vermmfecmodificacion = dr.GetDateTime(iVermmfecmodificacion);

            int iVermmmotivoportal = dr.GetOrdinal(this.Vermmmotivoportal);
            if (!dr.IsDBNull(iVermmmotivoportal)) entity.Vermmmotivoportal = Convert.ToInt32(dr.GetValue(iVermmmotivoportal));

            int iVermmfechageneracion = dr.GetOrdinal(this.Vermmfechageneracion);
            if (!dr.IsDBNull(iVermmfechageneracion)) entity.Vermmfechageneracion = dr.GetDateTime(iVermmfechageneracion);

            int iVermmfechaaprobacion = dr.GetOrdinal(this.Vermmfechaaprobacion);
            if (!dr.IsDBNull(iVermmfechaaprobacion)) entity.Vermmfechaaprobacion = dr.GetDateTime(iVermmfechaaprobacion);

            int iVermmmotivo = dr.GetOrdinal(this.Vermmmotivo);
            if (!dr.IsDBNull(iVermmmotivo)) entity.Vermmmotivo = dr.GetString(iVermmmotivo);

            int iVermmPorcentaje = dr.GetOrdinal(this.Vermmporcentaje);
            if (!dr.IsDBNull(iVermmPorcentaje)) entity.Vermmporcentaje = Convert.ToInt32(dr.GetValue(iVermmPorcentaje));

            int iVermmnumero = dr.GetOrdinal(this.Vermmnumero);
            if (!dr.IsDBNull(iVermmnumero)) entity.Vermmnumero = Convert.ToInt32(dr.GetValue(iVermmnumero));

            int iVermmmsjgeneracion = dr.GetOrdinal(this.Vermmmsjgeneracion);
            if (!dr.IsDBNull(iVermmmsjgeneracion)) entity.Vermmmsjgeneracion = dr.GetString(iVermmmsjgeneracion);

            return entity;
        }

        #region Mapeo de Campos

        public string Vermmcodi = "VERMMCODI";
        public string Vermmfechaperiodo = "VERMMFECHAPERIODO";
        public string Vermmusucreacion = "VERMMUSUCREACION";
        public string Vermmestado = "VERMMESTADO";
        public string Vermmfeccreacion = "VERMMFECCREACION";
        public string Vermmusumodificacion = "VERMMUSUMODIFICACION";
        public string Vermmfecmodificacion = "VERMMFECMODIFICACION";
        public string Vermmmotivoportal = "VERMMMOTIVOPORTAL";
        public string Vermmfechageneracion = "VERMMFECHAGENERACION";
        public string Vermmfechaaprobacion = "VERMMFECHAAPROBACION";
        public string Vermmporcentaje = "VERMMPORCENTAJE";
        public string Vermmmotivo = "VERMMMOTIVO";
        public string Vermmnumero = "VERMMNUMERO";
        public string Vermmmsjgeneracion = "VERMMMSJGENERACION";

        #endregion


        #region Monitoreo
        public string SqlUpdatePorcentaje
        {
            get { return GetSqlXml("UpdatePorcentaje"); }
        }

        public string SqlUpdateVersionEstado
        {
            get { return GetSqlXml("UpdateVersionEstado"); }
        }



        #endregion
    }
}
