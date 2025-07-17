using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_PERIODO
    /// </summary>
    public class RePeriodoHelper : HelperBase
    {
        public RePeriodoHelper(): base(Consultas.RePeriodoSql)
        {
        }

        public RePeriodoDTO Create(IDataReader dr)
        {
            RePeriodoDTO entity = new RePeriodoDTO();

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReperanio = dr.GetOrdinal(this.Reperanio);
            if (!dr.IsDBNull(iReperanio)) entity.Reperanio = Convert.ToInt32(dr.GetValue(iReperanio));

            int iRepertipo = dr.GetOrdinal(this.Repertipo);
            if (!dr.IsDBNull(iRepertipo)) entity.Repertipo = dr.GetString(iRepertipo);

            int iRepernombre = dr.GetOrdinal(this.Repernombre);
            if (!dr.IsDBNull(iRepernombre)) entity.Repernombre = dr.GetString(iRepernombre);

            int iReperpadre = dr.GetOrdinal(this.Reperpadre);
            if (!dr.IsDBNull(iReperpadre)) entity.Reperpadre = Convert.ToInt32(dr.GetValue(iReperpadre));

            int iReperrevision = dr.GetOrdinal(this.Reperrevision);
            if (!dr.IsDBNull(iReperrevision)) entity.Reperrevision = dr.GetString(iReperrevision);

            int iReperfecinicio = dr.GetOrdinal(this.Reperfecinicio);
            if (!dr.IsDBNull(iReperfecinicio)) entity.Reperfecinicio = dr.GetDateTime(iReperfecinicio);

            int iReperfecfin = dr.GetOrdinal(this.Reperfecfin);
            if (!dr.IsDBNull(iReperfecfin)) entity.Reperfecfin = dr.GetDateTime(iReperfecfin);

            int iReperestado = dr.GetOrdinal(this.Reperestado);
            if (!dr.IsDBNull(iReperestado)) entity.Reperestado = dr.GetString(iReperestado);

            int iReperorden = dr.GetOrdinal(this.Reperorden);
            if (!dr.IsDBNull(iReperorden)) entity.Reperorden = Convert.ToInt32(dr.GetValue(iReperorden));

            int iRepertcambio = dr.GetOrdinal(this.Repertcambio);
            if (!dr.IsDBNull(iRepertcambio)) entity.Repertcambio = dr.GetDecimal(iRepertcambio);

            int iReperfactorcomp = dr.GetOrdinal(this.Reperfactorcomp);
            if (!dr.IsDBNull(iReperfactorcomp)) entity.Reperfactorcomp = dr.GetDecimal(iReperfactorcomp);

            int iReperusucreacion = dr.GetOrdinal(this.Reperusucreacion);
            if (!dr.IsDBNull(iReperusucreacion)) entity.Reperusucreacion = dr.GetString(iReperusucreacion);

            int iReperfeccreacion = dr.GetOrdinal(this.Reperfeccreacion);
            if (!dr.IsDBNull(iReperfeccreacion)) entity.Reperfeccreacion = dr.GetDateTime(iReperfeccreacion);

            int iReperusumodificacion = dr.GetOrdinal(this.Reperusumodificacion);
            if (!dr.IsDBNull(iReperusumodificacion)) entity.Reperusumodificacion = dr.GetString(iReperusumodificacion);

            int iReperfecmodificacion = dr.GetOrdinal(this.Reperfecmodificacion);
            if (!dr.IsDBNull(iReperfecmodificacion)) entity.Reperfecmodificacion = dr.GetDateTime(iReperfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Repercodi = "REPERCODI";
        public string Reperanio = "REPERANIO";
        public string Repertipo = "REPERTIPO";
        public string Repernombre = "REPERNOMBRE";
        public string Reperpadre = "REPERPADRE";
        public string Reperrevision = "REPERREVISION";
        public string Reperfecinicio = "REPERFECINICIO";
        public string Reperfecfin = "REPERFECFIN";
        public string Reperestado = "REPERESTADO";
        public string Reperorden = "REPERORDEN";
        public string Repertcambio = "REPERTCAMBIO";
        public string Reperfactorcomp = "REPERFACTORCOMP";
        public string Reperusucreacion = "REPERUSUCREACION";
        public string Reperfeccreacion = "REPERFECCREACION";
        public string Reperusumodificacion = "REPERUSUMODIFICACION";
        public string Reperfecmodificacion = "REPERFECMODIFICACION";
        public string Repernombrepadre = "REPERNOMBREPADRE";

        #endregion

        public string SqlObtenerPeriodosPadre
        {
            get { return base.GetSqlXml("ObtenerPeriodosPadre"); }
        }

        public string SqlValidarNombre
        {
            get { return base.GetSqlXml("ValidarNombre"); }
        }

        public string SqlObtenerPeriodosCercanos
        {
            get { return base.GetSqlXml("ObtenerPeriodosCercanos"); }
        }
    }
}
