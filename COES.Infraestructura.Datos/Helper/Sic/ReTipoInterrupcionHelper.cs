using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_TIPO_INTERRUPCION
    /// </summary>
    public class ReTipoInterrupcionHelper : HelperBase
    {
        public ReTipoInterrupcionHelper(): base(Consultas.ReTipoInterrupcionSql)
        {
        }

        public ReTipoInterrupcionDTO Create(IDataReader dr)
        {
            ReTipoInterrupcionDTO entity = new ReTipoInterrupcionDTO();

            int iRetintcodi = dr.GetOrdinal(this.Retintcodi);
            if (!dr.IsDBNull(iRetintcodi)) entity.Retintcodi = Convert.ToInt32(dr.GetValue(iRetintcodi));

            int iRetintnombre = dr.GetOrdinal(this.Retintnombre);
            if (!dr.IsDBNull(iRetintnombre)) entity.Retintnombre = dr.GetString(iRetintnombre);

            int iRetintestado = dr.GetOrdinal(this.Retintestado);
            if (!dr.IsDBNull(iRetintestado)) entity.Retintestado = dr.GetString(iRetintestado);

            int iRetintusucreacion = dr.GetOrdinal(this.Retintusucreacion);
            if (!dr.IsDBNull(iRetintusucreacion)) entity.Retintusucreacion = dr.GetString(iRetintusucreacion);

            int iRetintfeccreacion = dr.GetOrdinal(this.Retintfeccreacion);
            if (!dr.IsDBNull(iRetintfeccreacion)) entity.Retintfeccreacion = dr.GetDateTime(iRetintfeccreacion);

            int iRetintusumodificacion = dr.GetOrdinal(this.Retintusumodificacion);
            if (!dr.IsDBNull(iRetintusumodificacion)) entity.Retintusumodificacion = dr.GetString(iRetintusumodificacion);

            int iRetintfecmodificacion = dr.GetOrdinal(this.Retintfecmodificacion);
            if (!dr.IsDBNull(iRetintfecmodificacion)) entity.Retintfecmodificacion = dr.GetDateTime(iRetintfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Retintcodi = "RETINTCODI";
        public string Retintnombre = "RETINTNOMBRE";
        public string Retintestado = "RETINTESTADO";
        public string Retintusucreacion = "RETINTUSUCREACION";
        public string Retintfeccreacion = "RETINTFECCREACION";
        public string Retintusumodificacion = "RETINTUSUMODIFICACION";
        public string Retintfecmodificacion = "RETINTFECMODIFICACION";
        public string IndicadorEdicion = "INDICADOREDICION";

        #endregion

        public string SqlObtenerConfiguracion
        {
            get { return base.GetSqlXml("ObtenerConfiguracion"); }
        }
    }
}
