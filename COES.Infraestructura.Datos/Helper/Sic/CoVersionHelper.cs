using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_VERSION
    /// </summary>
    public class CoVersionHelper : HelperBase
    {
        public CoVersionHelper(): base(Consultas.CoVersionSql)
        {
        }

        public CoVersionDTO Create(IDataReader dr)
        {
            CoVersionDTO entity = new CoVersionDTO();

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCoverdesc = dr.GetOrdinal(this.Coverdesc);
            if (!dr.IsDBNull(iCoverdesc)) entity.Coverdesc = dr.GetString(iCoverdesc);

            int iCoverfecinicio = dr.GetOrdinal(this.Coverfecinicio);
            if (!dr.IsDBNull(iCoverfecinicio)) entity.Coverfecinicio = dr.GetDateTime(iCoverfecinicio);

            int iCoverfecfin = dr.GetOrdinal(this.Coverfecfin);
            if (!dr.IsDBNull(iCoverfecfin)) entity.Coverfecfin = dr.GetDateTime(iCoverfecfin);

            int iCoverestado = dr.GetOrdinal(this.Coverestado);
            if (!dr.IsDBNull(iCoverestado)) entity.Coverestado = dr.GetString(iCoverestado);

            int iCovertipo = dr.GetOrdinal(this.Covertipo);
            if (!dr.IsDBNull(iCovertipo)) entity.Covertipo = dr.GetString(iCovertipo);

            int iCovebacodi = dr.GetOrdinal(this.Covebacodi);
            if (!dr.IsDBNull(iCovebacodi)) entity.Covebacodi = Convert.ToInt32(dr.GetValue(iCovebacodi));

            int iCoverusucreacion = dr.GetOrdinal(this.Coverusucreacion);
            if (!dr.IsDBNull(iCoverusucreacion)) entity.Coverusucreacion = dr.GetString(iCoverusucreacion);

            int iCoverfeccreacion = dr.GetOrdinal(this.Coverfeccreacion);
            if (!dr.IsDBNull(iCoverfeccreacion)) entity.Coverfeccreacion = dr.GetDateTime(iCoverfeccreacion);

            int iCoverusumodificacion = dr.GetOrdinal(this.Coverusumodificacion);
            if (!dr.IsDBNull(iCoverusumodificacion)) entity.Coverusumodificacion = dr.GetString(iCoverusumodificacion);

            int iCoverfecmodificacion = dr.GetOrdinal(this.Coverfecmodificacion);
            if (!dr.IsDBNull(iCoverfecmodificacion)) entity.Coverfecmodificacion = dr.GetDateTime(iCoverfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Covercodi = "COVERCODI";
        public string Copercodi = "COPERCODI";
        public string Coverdesc = "COVERDESC";
        public string Coverfecinicio = "COVERFECINICIO";
        public string Coverfecfin = "COVERFECFIN";
        public string Coverestado = "COVERESTADO";
        public string Covertipo = "COVERTIPO";
        public string Covebacodi = "COVEBACODI";
        public string Coverusucreacion = "COVERUSUCREACION";
        public string Coverfeccreacion = "COVERFECCREACION";
        public string Coverusumodificacion = "COVERUSUMODIFICACION";
        public string Coverfecmodificacion = "COVERFECMODIFICACION";
        public string Destipo = "DESTIPO";
        public string Coperestado = "COPERESTADO";

        #endregion

        public string SqlObtenerVersionPorFecha
        {
            get { return base.GetSqlXml("ObtenerVersionPorFecha"); }
        }

        public string SqlGetVersionesPorMes
        {
            get { return base.GetSqlXml("GetVersionesPorMes"); }
        }
    }
}
