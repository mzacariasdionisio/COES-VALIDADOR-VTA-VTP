using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_AGRUPACION
    /// </summary>
    public class PrAgrupacionHelper : HelperBase
    {
        public PrAgrupacionHelper()
            : base(Consultas.PrAgrupacionSql)
        {
        }

        public PrAgrupacionDTO Create(IDataReader dr)
        {
            PrAgrupacionDTO entity = new PrAgrupacionDTO();

            int iAgrupcodi = dr.GetOrdinal(this.Agrupcodi);
            if (!dr.IsDBNull(iAgrupcodi)) entity.Agrupcodi = Convert.ToInt32(dr.GetValue(iAgrupcodi));

            int iAgrupnombre = dr.GetOrdinal(this.Agrupnombre);
            if (!dr.IsDBNull(iAgrupnombre)) entity.Agrupnombre = dr.GetString(iAgrupnombre);

            int iAgrupusucreacion = dr.GetOrdinal(this.Agrupusucreacion);
            if (!dr.IsDBNull(iAgrupusucreacion)) entity.Agrupusucreacion = dr.GetString(iAgrupusucreacion);

            int iAgrupfeccreacion = dr.GetOrdinal(this.Agrupfeccreacion);
            if (!dr.IsDBNull(iAgrupfeccreacion)) entity.Agrupfeccreacion = dr.GetDateTime(iAgrupfeccreacion);

            int iAgrupusumodificacion = dr.GetOrdinal(this.Agrupusumodificacion);
            if (!dr.IsDBNull(iAgrupusumodificacion)) entity.Agrupusumodificacion = dr.GetString(iAgrupusumodificacion);

            int iAgrupfecmodificacion = dr.GetOrdinal(this.Agrupfecmodificacion);
            if (!dr.IsDBNull(iAgrupfecmodificacion)) entity.Agrupfecmodificacion = dr.GetDateTime(iAgrupfecmodificacion);

            int iAgrupestado = dr.GetOrdinal(this.Agrupestado);
            if (!dr.IsDBNull(iAgrupestado)) entity.Agrupestado = dr.GetString(iAgrupestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Agrupcodi = "AGRUPCODI";
        public string Agrupnombre = "AGRUPNOMBRE";
        public string Agrupusucreacion = "AGRUPUSUCREACION";
        public string Agrupfeccreacion = "AGRUPFECCREACION";
        public string Agrupusumodificacion = "AGRUPUSUMODIFICACION";
        public string Agrupfecmodificacion = "AGRUPFECMODIFICACION";
        public string Agrupestado = "AGRUPESTADO";
        public string Agrupfuente = "AGRUPFUENTE";

        #endregion

    }
}
