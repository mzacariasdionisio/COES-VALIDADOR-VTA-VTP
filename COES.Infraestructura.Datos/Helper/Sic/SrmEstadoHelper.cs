using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SRM_ESTADO
    /// </summary>
    public class SrmEstadoHelper : HelperBase
    {
        public SrmEstadoHelper(): base(Consultas.SrmEstadoSql)
        {
        }

        public SrmEstadoDTO Create(IDataReader dr)
        {
            SrmEstadoDTO entity = new SrmEstadoDTO();

            int iSrmstdcodi = dr.GetOrdinal(this.Srmstdcodi);
            if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

            int iSrmstddescrip = dr.GetOrdinal(this.Srmstddescrip);
            if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

            int iSrmstdcolor = dr.GetOrdinal(this.Srmstdcolor);
            if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

            int iSrmstdusucreacion = dr.GetOrdinal(this.Srmstdusucreacion);
            if (!dr.IsDBNull(iSrmstdusucreacion)) entity.Srmstdusucreacion = dr.GetString(iSrmstdusucreacion);

            int iSrmstdfeccreacion = dr.GetOrdinal(this.Srmstdfeccreacion);
            if (!dr.IsDBNull(iSrmstdfeccreacion)) entity.Srmstdfeccreacion = dr.GetDateTime(iSrmstdfeccreacion);

            int iSrmstdsumodificacion = dr.GetOrdinal(this.Srmstdsumodificacion);
            if (!dr.IsDBNull(iSrmstdsumodificacion)) entity.Srmstdsumodificacion = dr.GetString(iSrmstdsumodificacion);

            int iSrmstdfecmodificacion = dr.GetOrdinal(this.Srmstdfecmodificacion);
            if (!dr.IsDBNull(iSrmstdfecmodificacion)) entity.Srmstdfecmodificacion = dr.GetDateTime(iSrmstdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Srmstdcodi = "SRMSTDCODI";
        public string Srmstddescrip = "SRMSTDDESCRIP";
        public string Srmstdcolor = "SRMSTDCOLOR";
        public string Srmstdusucreacion = "SRMSTDUSUCREACION";
        public string Srmstdfeccreacion = "SRMSTDFECCREACION";
        public string Srmstdsumodificacion = "SRMSTDSUMODIFICACION";
        public string Srmstdfecmodificacion = "SRMSTDFECMODIFICACION";

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
