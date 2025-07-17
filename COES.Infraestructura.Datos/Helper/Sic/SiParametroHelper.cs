using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PARAMETRO
    /// </summary>
    public class SiParametroHelper : HelperBase
    {
        public SiParametroHelper(): base(Consultas.SiParametroSql)
        {
        }

        public SiParametroDTO Create(IDataReader dr)
        {
            SiParametroDTO entity = new SiParametroDTO();

            int iSiparcodi = dr.GetOrdinal(this.Siparcodi);
            if (!dr.IsDBNull(iSiparcodi)) entity.Siparcodi = Convert.ToInt32(dr.GetValue(iSiparcodi));

            int iSiparabrev = dr.GetOrdinal(this.Siparabrev);
            if (!dr.IsDBNull(iSiparabrev)) entity.Siparabrev = dr.GetString(iSiparabrev);

            int iSipardescripcion = dr.GetOrdinal(this.Sipardescripcion);
            if (!dr.IsDBNull(iSipardescripcion)) entity.Sipardescripcion = dr.GetString(iSipardescripcion);

            int iSiparusucreacion = dr.GetOrdinal(this.Siparusucreacion);
            if (!dr.IsDBNull(iSiparusucreacion)) entity.Siparusucreacion = dr.GetString(iSiparusucreacion);

            int iSiparfeccreacion = dr.GetOrdinal(this.Siparfeccreacion);
            if (!dr.IsDBNull(iSiparfeccreacion)) entity.Siparfeccreacion = dr.GetDateTime(iSiparfeccreacion);

            int iSiparusumodificacion = dr.GetOrdinal(this.Siparusumodificacion);
            if (!dr.IsDBNull(iSiparusumodificacion)) entity.Siparusumodificacion = dr.GetString(iSiparusumodificacion);

            int iSiparfecmodificacion = dr.GetOrdinal(this.Siparfecmodificacion);
            if (!dr.IsDBNull(iSiparfecmodificacion)) entity.Siparfecmodificacion = dr.GetDateTime(iSiparfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Siparcodi = "SIPARCODI";
        public string Siparabrev = "SIPARABREV";
        public string Sipardescripcion = "SIPARDESCRIPCION";
        public string Siparusucreacion = "SIPARUSUCREACION";
        public string Siparfeccreacion = "SIPARFECCREACION";
        public string Siparusumodificacion = "SIPARUSUMODIFICACION";
        public string Siparfecmodificacion = "SIPARFECMODIFICACION";

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
