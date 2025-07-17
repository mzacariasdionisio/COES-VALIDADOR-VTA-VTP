using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_AMPLIACION_PLAZO
    /// </summary>
    public class SmaAmpliacionPlazoHelper : HelperBase
    {
        public SmaAmpliacionPlazoHelper(): base(Consultas.SmaAmpliacionPlazoSql)
        {
        }

        public SmaAmpliacionPlazoDTO Create(IDataReader dr)
        {
            SmaAmpliacionPlazoDTO entity = new SmaAmpliacionPlazoDTO();

            int iSmaapcodi = dr.GetOrdinal(this.Smaapcodi);
            if (!dr.IsDBNull(iSmaapcodi)) entity.Smaapcodi = Convert.ToInt32(dr.GetValue(iSmaapcodi));

            int iSmaapaniomes = dr.GetOrdinal(this.Smaapaniomes);
            if (!dr.IsDBNull(iSmaapaniomes)) entity.Smaapaniomes = dr.GetDateTime(iSmaapaniomes);

            int iSmaapplazodefecto = dr.GetOrdinal(this.Smaapplazodefecto);
            if (!dr.IsDBNull(iSmaapplazodefecto)) entity.Smaapplazodefecto = dr.GetDateTime(iSmaapplazodefecto);

            int iSmaapnuevoplazo = dr.GetOrdinal(this.Smaapnuevoplazo);
            if (!dr.IsDBNull(iSmaapnuevoplazo)) entity.Smaapnuevoplazo = dr.GetDateTime(iSmaapnuevoplazo);

            int iSmaapestado = dr.GetOrdinal(this.Smaapestado);
            if (!dr.IsDBNull(iSmaapestado)) entity.Smaapestado = dr.GetString(iSmaapestado);

            int iSmaapusucreacion = dr.GetOrdinal(this.Smaapusucreacion);
            if (!dr.IsDBNull(iSmaapusucreacion)) entity.Smaapusucreacion = dr.GetString(iSmaapusucreacion);

            int iSmaapfeccreacion = dr.GetOrdinal(this.Smaapfeccreacion);
            if (!dr.IsDBNull(iSmaapfeccreacion)) entity.Smaapfeccreacion = dr.GetDateTime(iSmaapfeccreacion);

            int iSmaapusumodificacion = dr.GetOrdinal(this.Smaapusumodificacion);
            if (!dr.IsDBNull(iSmaapusumodificacion)) entity.Smaapusumodificacion = dr.GetString(iSmaapusumodificacion);

            int iSmaapfecmodificacion = dr.GetOrdinal(this.Smaapfecmodificacion);
            if (!dr.IsDBNull(iSmaapfecmodificacion)) entity.Smaapfecmodificacion = dr.GetDateTime(iSmaapfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Smaapcodi = "SMAAPCODI";
        public string Smaapaniomes = "SMAAPANIOMES";
        public string Smaapplazodefecto = "SMAAPPLAZODEFECTO";
        public string Smaapnuevoplazo = "SMAAPNUEVOPLAZO";
        public string Smaapestado = "SMAAPESTADO";
        public string Smaapusucreacion = "SMAAPUSUCREACION";
        public string Smaapfeccreacion = "SMAAPFECCREACION";
        public string Smaapusumodificacion = "SMAAPUSUMODIFICACION";
        public string Smaapfecmodificacion = "SMAAPFECMODIFICACION";

        #endregion
    }
}
