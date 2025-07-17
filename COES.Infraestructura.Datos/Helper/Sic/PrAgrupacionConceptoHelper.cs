using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_AGRUPACIONCONCEPTO
    /// </summary>
    public class PrAgrupacionConceptoHelper : HelperBase
    {
        public PrAgrupacionConceptoHelper()
            : base(Consultas.PrAgrupacionConceptoSql)
        {
        }

        public PrAgrupacionConceptoDTO Create(IDataReader dr)
        {
            PrAgrupacionConceptoDTO entity = new PrAgrupacionConceptoDTO();

            int iAgrconcodi = dr.GetOrdinal(this.Agrconcodi);
            if (!dr.IsDBNull(iAgrconcodi)) entity.Agrconcodi = Convert.ToInt32(dr.GetValue(iAgrconcodi));

            int iAgrupcodi = dr.GetOrdinal(this.Agrupcodi);
            if (!dr.IsDBNull(iAgrupcodi)) entity.Agrupcodi = Convert.ToInt32(dr.GetValue(iAgrupcodi));

            int iAgrconfecha = dr.GetOrdinal(this.Agrconfecha);
            if (!dr.IsDBNull(iAgrconfecha)) entity.Agrconfecha = dr.GetDateTime(iAgrconfecha);

            int iAgrconactivo = dr.GetOrdinal(this.Agrconactivo);
            if (!dr.IsDBNull(iAgrconactivo)) entity.Agrconactivo = Convert.ToInt32(dr.GetValue(iAgrconactivo));

            int iAgrconfeccreacion = dr.GetOrdinal(this.Agrconfeccreacion);
            if (!dr.IsDBNull(iAgrconfeccreacion)) entity.Agrconfeccreacion = dr.GetDateTime(iAgrconfeccreacion);

            int iAgrconusucreacion = dr.GetOrdinal(this.Agrconusucreacion);
            if (!dr.IsDBNull(iAgrconusucreacion)) entity.Agrconusucreacion = dr.GetString(iAgrconusucreacion);

            int iAgrconusumodificacion = dr.GetOrdinal(this.Agrconusumodificacion);
            if (!dr.IsDBNull(iAgrconusumodificacion)) entity.Agrconusumodificacion = dr.GetString(iAgrconusumodificacion);

            int iAgrfecmodificacion = dr.GetOrdinal(this.Agrconfecmodificacion);
            if (!dr.IsDBNull(iAgrfecmodificacion)) entity.Agrconfecmodificacion = dr.GetDateTime(iAgrfecmodificacion);

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Agrconcodi = "AGRCONCODI";
        public string Agrupcodi = "AGRUPCODI";
        public string Agrconfecha = "AGRCONFECHA";
        public string Agrconactivo = "AGRCONACTIVO";
        public string Agrconfeccreacion = "AGRCONFECCREACION";
        public string Agrconusucreacion = "AGRCONUSUCREACION";
        public string Agrconusumodificacion = "AGRCONUSUMODIFICACION";
        public string Agrconfecmodificacion = "AGRCONFECMODIFICACION";
        public string Concepcodi = "CONCEPCODI";
        public string Propcodi = "PROPCODI";

        public string Concepabrev = "CONCEPABREV";
        public string Concepdesc = "CONCEPDESC";
        public string Concepunid = "CONCEPUNID";
        public string Conceptipo = "CONCEPTIPO";
        public string Catenomb = "CATENOMB";
        public string Cateabrev = "CATEABREV";
        public string Catecodi = "Catecodi";
        public string Famcodi = "Famcodi";
        public string Concepnombficha = "CONCEPNOMBFICHA";

        #endregion

    }
}
