using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DAI_TABLACODIGO
    /// </summary>
    public class DaiTablacodigoHelper : HelperBase
    {
        public DaiTablacodigoHelper(): base(Consultas.DaiTablacodigoSql)
        {
        }

        public DaiTablacodigoDTO Create(IDataReader dr)
        {
            DaiTablacodigoDTO entity = new DaiTablacodigoDTO();

            int iTabcodi = dr.GetOrdinal(this.Tabcodi);
            if (!dr.IsDBNull(iTabcodi)) entity.Tabcodi = Convert.ToInt32(dr.GetValue(iTabcodi));

            int iTabdescripcion = dr.GetOrdinal(this.Tabdescripcion);
            if (!dr.IsDBNull(iTabdescripcion)) entity.Tabdescripcion = dr.GetString(iTabdescripcion);

            int iTabactivo = dr.GetOrdinal(this.Tabactivo);
            if (!dr.IsDBNull(iTabactivo)) entity.Tabactivo = dr.GetString(iTabactivo);

            int iTabusucreacion = dr.GetOrdinal(this.Tabusucreacion);
            if (!dr.IsDBNull(iTabusucreacion)) entity.Tabusucreacion = dr.GetString(iTabusucreacion);

            int iTabfeccreacion = dr.GetOrdinal(this.Tabfeccreacion);
            if (!dr.IsDBNull(iTabfeccreacion)) entity.Tabfeccreacion = dr.GetDateTime(iTabfeccreacion);

            int iTabusumodificacion = dr.GetOrdinal(this.Tabusumodificacion);
            if (!dr.IsDBNull(iTabusumodificacion)) entity.Tabusumodificacion = dr.GetString(iTabusumodificacion);

            int iTabfecmodificacion = dr.GetOrdinal(this.Tabfecmodificacion);
            if (!dr.IsDBNull(iTabfecmodificacion)) entity.Tabfecmodificacion = dr.GetDateTime(iTabfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tabcodi = "TABCODI";
        public string Tabdescripcion = "TABDESCRIPCION";
        public string Tabactivo = "TABACTIVO";
        public string Tabusucreacion = "TABUSUCREACION";
        public string Tabfeccreacion = "TABFECCREACION";
        public string Tabusumodificacion = "TABUSUMODIFICACION";
        public string Tabfecmodificacion = "TABFECMODIFICACION";

        #endregion
    }
}
