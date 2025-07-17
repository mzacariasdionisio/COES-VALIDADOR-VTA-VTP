using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_AREACONCEPTO
    /// </summary>
    public class PrAreaConceptoHelper : HelperBase
    {
        public PrAreaConceptoHelper()
            : base(Consultas.PrAreaConceptoSql)
        {
        }

        public PrAreaConceptoDTO Create(IDataReader dr)
        {
            PrAreaConceptoDTO entity = new PrAreaConceptoDTO();

            int iArconcodi = dr.GetOrdinal(this.Arconcodi);
            if (!dr.IsDBNull(iArconcodi)) entity.Arconcodi = Convert.ToInt32(dr.GetValue(iArconcodi));

            int iArconusucreacion = dr.GetOrdinal(this.Arconusucreacion);
            if (!dr.IsDBNull(iArconusucreacion)) entity.Arconusucreacion = dr.GetString(iArconusucreacion);

            int iArconfeccreacion = dr.GetOrdinal(this.Arconfeccreacion);
            if (!dr.IsDBNull(iArconfeccreacion)) entity.Arconfeccreacion = dr.GetDateTime(iArconfeccreacion);

            int iArconusumodificacion = dr.GetOrdinal(this.Arconusumodificacion);
            if (!dr.IsDBNull(iArconusumodificacion)) entity.Arconusumodificacion = dr.GetString(iArconusumodificacion);

            int iArconfecmodificacion = dr.GetOrdinal(this.Arconfecmodificacion);
            if (!dr.IsDBNull(iArconfecmodificacion)) entity.Arconfecmodificacion = dr.GetDateTime(iArconfecmodificacion);

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iArconactivo = dr.GetOrdinal(this.Arconactivo);
            if (!dr.IsDBNull(iArconactivo)) entity.Arconactivo = Convert.ToInt32(dr.GetValue(iArconactivo));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            return entity;
        }


        #region Mapeo de Campos

        public string Arconcodi = "ARCONCODI";
        public string Arconusucreacion = "ARCONUSUCREACION";
        public string Arconfeccreacion = "ARCONFECCREACION";
        public string Arconusumodificacion = "ARCONUSUMODIFICACION";
        public string Arconfecmodificacion = "ARCONFECMODIFICACION";
        public string Concepcodi = "CONCEPCODI";
        public string Arconactivo = "ARCONACTIVO";
        public string Areacode = "AREACODE";

        #endregion

        #region Mapeo de Consultas

        public string SqlListarConcepcodiRegistrados
        {
            get { return base.GetSqlXml("ListarConcepcodiRegistrados"); }
        }

        #endregion
    }
}
