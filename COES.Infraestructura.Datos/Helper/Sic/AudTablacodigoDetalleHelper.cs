using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_TABLACODIGO_DETALLE
    /// </summary>
    public class AudTablacodigoDetalleHelper : HelperBase
    {
        public AudTablacodigoDetalleHelper(): base(Consultas.AudTablacodigoDetalleSql)
        {
        }

        public string GetByDescripcion
        {
            get { return base.GetSqlXml("GetByDescripcion"); }
        }

        public AudTablacodigoDetalleDTO Create(IDataReader dr)
        {
            AudTablacodigoDetalleDTO entity = new AudTablacodigoDetalleDTO();

            int iTabcdcodi = dr.GetOrdinal(this.Tabcdcodi);
            if (!dr.IsDBNull(iTabcdcodi)) entity.Tabcdcodi = Convert.ToInt32(dr.GetValue(iTabcdcodi));

            int iTabccodi = dr.GetOrdinal(this.Tabccodi);
            if (!dr.IsDBNull(iTabccodi)) entity.Tabccodi = Convert.ToInt32(dr.GetValue(iTabccodi));

            int iTabcddescripcion = dr.GetOrdinal(this.Tabcddescripcion);
            if (!dr.IsDBNull(iTabcddescripcion)) entity.Tabcddescripcion = dr.GetString(iTabcddescripcion);         
            

            int iTabcdvalor = dr.GetOrdinal(this.Tabcdvalor);
            if (!dr.IsDBNull(iTabcdvalor)) entity.Tabcdvalor = dr.GetString(iTabcdvalor);

            int iTaborden = dr.GetOrdinal(this.Tabcdorden);
            if (!dr.IsDBNull(iTaborden)) entity.Tabcdorden = Convert.ToInt32(iTaborden);

            int iTabcdactivo = dr.GetOrdinal(this.Tabcdactivo);
            if (!dr.IsDBNull(iTabcdactivo)) entity.Tabcdactivo = dr.GetString(iTabcdactivo);

            int iTabcdusucreacion = dr.GetOrdinal(this.Tabcdusucreacion);
            if (!dr.IsDBNull(iTabcdusucreacion)) entity.Tabcdusucreacion = dr.GetString(iTabcdusucreacion);

            int iTabcdfeccreacion = dr.GetOrdinal(this.Tabcdfeccreacion);
            if (!dr.IsDBNull(iTabcdfeccreacion)) entity.Tabcdfeccreacion = dr.GetDateTime(iTabcdfeccreacion);

            int iTabcdusumodificacion = dr.GetOrdinal(this.Tabcdusumodificacion);
            if (!dr.IsDBNull(iTabcdusumodificacion)) entity.Tabcdusumodificacion = dr.GetString(iTabcdusumodificacion);

            int iTabcdfecmodificacion = dr.GetOrdinal(this.Tabcdfecmodificacion);
            if (!dr.IsDBNull(iTabcdfecmodificacion)) entity.Tabcdfecmodificacion = dr.GetDateTime(iTabcdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tabcdcodi = "TABCDCODI";
        public string Tabccodi = "TABCCODI";
        public string Tabcddescripcion = "TABCDDESCRIPCION";
        public string Tabcdvalor = "TABCDVALOR";
        public string Tabcdorden = "TABCDORDEN";
        public string Tabcdactivo = "TABCDACTIVO";
        public string Tabcdusucreacion = "TABCDUSUCREACION";
        public string Tabcdfeccreacion = "TABCDFECCREACION";
        public string Tabcdusumodificacion = "TABCDUSUMODIFICACION";
        public string Tabcdfecmodificacion = "TABCDFECMODIFICACION";

        #endregion
    }
}
