using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_TABLACODIGO
    /// </summary>
    public class AudTablacodigoHelper : HelperBase
    {
        public AudTablacodigoHelper(): base(Consultas.AudTablacodigoSql)
        {
        }

        public AudTablacodigoDTO Create(IDataReader dr)
        {
            AudTablacodigoDTO entity = new AudTablacodigoDTO();

            int iTabccodi = dr.GetOrdinal(this.Tabccodi);
            if (!dr.IsDBNull(iTabccodi)) entity.Tabccodi = Convert.ToInt32(dr.GetValue(iTabccodi));

            int iTabcdescripcion = dr.GetOrdinal(this.Tabcdescripcion);
            if (!dr.IsDBNull(iTabcdescripcion)) entity.Tabcdescripcion = dr.GetString(iTabcdescripcion);

            int iTabcactivo = dr.GetOrdinal(this.Tabcactivo);
            if (!dr.IsDBNull(iTabcactivo)) entity.Tabcactivo = dr.GetString(iTabcactivo);

            int iTabcusucreacion = dr.GetOrdinal(this.Tabcusucreacion);
            if (!dr.IsDBNull(iTabcusucreacion)) entity.Tabcusucreacion = dr.GetString(iTabcusucreacion);

            int iTabcfeccreacion = dr.GetOrdinal(this.Tabcfeccreacion);
            if (!dr.IsDBNull(iTabcfeccreacion)) entity.Tabcfeccreacion = dr.GetDateTime(iTabcfeccreacion);

            int iTabcusumodificacion = dr.GetOrdinal(this.Tabcusumodificacion);
            if (!dr.IsDBNull(iTabcusumodificacion)) entity.Tabcusumodificacion = dr.GetString(iTabcusumodificacion);

            int iTabcfecmodificacion = dr.GetOrdinal(this.Tabcfecmodificacion);
            if (!dr.IsDBNull(iTabcfecmodificacion)) entity.Tabcfecmodificacion = dr.GetDateTime(iTabcfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tabccodi = "TABCCODI";
        public string Tabcdescripcion = "TABCDESCRIPCION";
        public string Tabcactivo = "TABCACTIVO";
        public string Tabcusucreacion = "TABCUSUCREACION";
        public string Tabcfeccreacion = "TABCFECCREACION";
        public string Tabcusumodificacion = "TABCUSUMODIFICACION";
        public string Tabcfecmodificacion = "TABCFECMODIFICACION";

        #endregion
    }
}
