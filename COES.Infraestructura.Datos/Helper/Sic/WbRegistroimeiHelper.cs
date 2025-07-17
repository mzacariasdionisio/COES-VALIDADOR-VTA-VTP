using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_REGISTROIMEI
    /// </summary>
    public class WbRegistroimeiHelper : HelperBase
    {
        public WbRegistroimeiHelper(): base(Consultas.WbRegistroimeiSql)
        {
        }

        public WbRegistroimeiDTO Create(IDataReader dr)
        {
            WbRegistroimeiDTO entity = new WbRegistroimeiDTO();

            int iRegimecodi = dr.GetOrdinal(this.Regimecodi);
            if (!dr.IsDBNull(iRegimecodi)) entity.Regimecodi = Convert.ToInt32(dr.GetValue(iRegimecodi));

            int iRegimeusuario = dr.GetOrdinal(this.Regimeusuario);
            if (!dr.IsDBNull(iRegimeusuario)) entity.Regimeusuario = dr.GetString(iRegimeusuario);

            int iRegimecodigoimei = dr.GetOrdinal(this.Regimecodigoimei);
            if (!dr.IsDBNull(iRegimecodigoimei)) entity.Regimecodigoimei = dr.GetString(iRegimecodigoimei);

            int iRegimeestado = dr.GetOrdinal(this.Regimeestado);
            if (!dr.IsDBNull(iRegimeestado)) entity.Regimeestado = dr.GetString(iRegimeestado);

            int iRegimeusucreacion = dr.GetOrdinal(this.Regimeusucreacion);
            if (!dr.IsDBNull(iRegimeusucreacion)) entity.Regimeusucreacion = dr.GetString(iRegimeusucreacion);

            int iRegimefeccreacion = dr.GetOrdinal(this.Regimefeccreacion);
            if (!dr.IsDBNull(iRegimefeccreacion)) entity.Regimefeccreacion = dr.GetDateTime(iRegimefeccreacion);

            int iRegimeusumodificacion = dr.GetOrdinal(this.Regimeusumodificacion);
            if (!dr.IsDBNull(iRegimeusumodificacion)) entity.Regimeusumodificacion = dr.GetString(iRegimeusumodificacion);

            int iRegimefecmodificacion = dr.GetOrdinal(this.Regimefecmodificacion);
            if (!dr.IsDBNull(iRegimefecmodificacion)) entity.Regimefecmodificacion = dr.GetDateTime(iRegimefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Regimecodi = "REGIMECODI";
        public string Regimeusuario = "REGIMEUSUARIO";
        public string Regimecodigoimei = "REGIMECODIGOIMEI";
        public string Regimeestado = "REGIMEESTADO";
        public string Regimeusucreacion = "REGIMEUSUCREACION";
        public string Regimefeccreacion = "REGIMEFECCREACION";
        public string Regimeusumodificacion = "REGIMEUSUMODIFICACION";
        public string Regimefecmodificacion = "REGIMEFECMODIFICACION";

        #endregion
    }
}
