using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SRM_CRITICIDAD
    /// </summary>
    public class SrmCriticidadHelper : HelperBase
    {
        public SrmCriticidadHelper(): base(Consultas.SrmCriticidadSql)
        {
        }

        public SrmCriticidadDTO Create(IDataReader dr)
        {
            SrmCriticidadDTO entity = new SrmCriticidadDTO();

            int iSrmcrtcodi = dr.GetOrdinal(this.Srmcrtcodi);
            if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

            int iSrmcrtdescrip = dr.GetOrdinal(this.Srmcrtdescrip);
            if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

            int iSrmcrtcolor = dr.GetOrdinal(this.Srmcrtcolor);
            if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

            int iSrmcrtusucreacion = dr.GetOrdinal(this.Srmcrtusucreacion);
            if (!dr.IsDBNull(iSrmcrtusucreacion)) entity.Srmcrtusucreacion = dr.GetString(iSrmcrtusucreacion);

            int iSrmcrtfeccreacion = dr.GetOrdinal(this.Srmcrtfeccreacion);
            if (!dr.IsDBNull(iSrmcrtfeccreacion)) entity.Srmcrtfeccreacion = dr.GetDateTime(iSrmcrtfeccreacion);

            int iSrmcrtusumodificacion = dr.GetOrdinal(this.Srmcrtusumodificacion);
            if (!dr.IsDBNull(iSrmcrtusumodificacion)) entity.Srmcrtusumodificacion = dr.GetString(iSrmcrtusumodificacion);

            int iSrmcrtfecmodificacion = dr.GetOrdinal(this.Srmcrtfecmodificacion);
            if (!dr.IsDBNull(iSrmcrtfecmodificacion)) entity.Srmcrtfecmodificacion = dr.GetDateTime(iSrmcrtfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Srmcrtcodi = "SRMCRTCODI";
        public string Srmcrtdescrip = "SRMCRTDESCRIP";
        public string Srmcrtcolor = "SRMCRTCOLOR";
        public string Srmcrtusucreacion = "SRMCRTUSUCREACION";
        public string Srmcrtfeccreacion = "SRMCRTFECCREACION";
        public string Srmcrtusumodificacion = "SRMCRTUSUMODIFICACION";
        public string Srmcrtfecmodificacion = "SRMCRTFECMODIFICACION";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
