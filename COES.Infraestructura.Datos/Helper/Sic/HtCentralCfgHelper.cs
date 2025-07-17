using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla HT_CENTRAL_CFG
    /// </summary>
    public class HtCentralCfgHelper : HelperBase
    {
        public HtCentralCfgHelper(): base(Consultas.HtCentralCfgSql)
        {
        }

        public HtCentralCfgDTO Create(IDataReader dr)
        {
            HtCentralCfgDTO entity = new HtCentralCfgDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iHtcentcodi = dr.GetOrdinal(this.Htcentcodi);
            if (!dr.IsDBNull(iHtcentcodi)) entity.Htcentcodi = Convert.ToInt32(dr.GetValue(iHtcentcodi));

            int iHtcentfuente = dr.GetOrdinal(this.Htcentfuente);
            if (!dr.IsDBNull(iHtcentfuente)) entity.Htcentfuente = dr.GetString(iHtcentfuente);

            int iHtcentfecregistro = dr.GetOrdinal(this.Htcentfecregistro);
            if (!dr.IsDBNull(iHtcentfecregistro)) entity.Htcentfecregistro = dr.GetDateTime(iHtcentfecregistro);

            int iHtcentusuregistro = dr.GetOrdinal(this.Htcentusuregistro);
            if (!dr.IsDBNull(iHtcentusuregistro)) entity.Htcentusuregistro = dr.GetString(iHtcentusuregistro);

            int iHtcentfecmodificacion = dr.GetOrdinal(this.Htcentfecmodificacion);
            if (!dr.IsDBNull(iHtcentfecmodificacion)) entity.Htcentfecmodificacion = dr.GetDateTime(iHtcentfecmodificacion);

            int iHtcentusumodificacion = dr.GetOrdinal(this.Htcentusumodificacion);
            if (!dr.IsDBNull(iHtcentusumodificacion)) entity.Htcentusumodificacion = dr.GetString(iHtcentusumodificacion);

            int iHtcentactivo = dr.GetOrdinal(this.Htcentactivo);
            if (!dr.IsDBNull(iHtcentactivo)) entity.Htcentactivo = Convert.ToInt32(dr.GetValue(iHtcentactivo));

            return entity;
        }


        #region Mapeo de Campos

        public string Equicodi = "EQUICODI";
        public string Htcentcodi = "HTCENTCODI";
        public string Htcentfuente = "HTCENTFUENTE";
        public string Htcentfecregistro = "HTCENTFECREGISTRO";
        public string Htcentusuregistro = "HTCENTUSUREGISTRO";
        public string Htcentfecmodificacion = "HTCENTFECMODIFICACION";
        public string Htcentusumodificacion = "HTCENTUSUMODIFICACION";
        public string Htcentactivo = "HTCENTACTIVO";

        #endregion
    }
}
