using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DAI_TABLACODIGO_DETALLE
    /// </summary>
    public class DaiTablacodigoDetalleHelper : HelperBase
    {
        public DaiTablacodigoDetalleHelper(): base(Consultas.DaiTablacodigoDetalleSql)
        {
        }

        public DaiTablacodigoDetalleDTO Create(IDataReader dr)
        {
            DaiTablacodigoDetalleDTO entity = new DaiTablacodigoDetalleDTO();

            int iTabdcodi = dr.GetOrdinal(this.Tabdcodi);
            if (!dr.IsDBNull(iTabdcodi)) entity.Tabdcodi = Convert.ToInt32(dr.GetValue(iTabdcodi));

            int iTabcodi = dr.GetOrdinal(this.Tabcodi);
            if (!dr.IsDBNull(iTabcodi)) entity.Tabcodi = Convert.ToInt32(dr.GetValue(iTabcodi));

            int iTabddescripcion = dr.GetOrdinal(this.Tabddescripcion);
            if (!dr.IsDBNull(iTabddescripcion)) entity.Tabddescripcion = dr.GetString(iTabddescripcion);

            int iTabdvalor = dr.GetOrdinal(this.Tabdvalor);
            if (!dr.IsDBNull(iTabdvalor)) entity.Tabdvalor = dr.GetString(iTabdvalor);

            int iTabdorden = dr.GetOrdinal(this.Tabdorden);
            if (!dr.IsDBNull(iTabdorden)) entity.Tabdorden = Convert.ToInt32(dr.GetValue(iTabdorden));

            int iTabdactivo = dr.GetOrdinal(this.Tabdactivo);
            if (!dr.IsDBNull(iTabdactivo)) entity.Tabdactivo = dr.GetString(iTabdactivo);

            int iTabdusucreacion = dr.GetOrdinal(this.Tabdusucreacion);
            if (!dr.IsDBNull(iTabdusucreacion)) entity.Tabdusucreacion = dr.GetString(iTabdusucreacion);

            int iTabdfeccreacion = dr.GetOrdinal(this.Tabdfeccreacion);
            if (!dr.IsDBNull(iTabdfeccreacion)) entity.Tabdfeccreacion = dr.GetDateTime(iTabdfeccreacion);

            int iTabdusumodificacion = dr.GetOrdinal(this.Tabdusumodificacion);
            if (!dr.IsDBNull(iTabdusumodificacion)) entity.Tabdusumodificacion = dr.GetString(iTabdusumodificacion);

            int iTabdfecmodificacion = dr.GetOrdinal(this.Tabdfecmodificacion);
            if (!dr.IsDBNull(iTabdfecmodificacion)) entity.Tabdfecmodificacion = dr.GetDateTime(iTabdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tabdcodi = "TABDCODI";
        public string Tabcodi = "TABCODI";
        public string Tabddescripcion = "TABDDESCRIPCION";
        public string Tabdvalor = "TABDVALOR";
        public string Tabdorden = "TABDORDEN";
        public string Tabdactivo = "TABDACTIVO";
        public string Tabdusucreacion = "TABDUSUCREACION";
        public string Tabdfeccreacion = "TABDFECCREACION";
        public string Tabdusumodificacion = "TABDUSUMODIFICACION";
        public string Tabdfecmodificacion = "TABDFECMODIFICACION";

        #endregion
    }
}
