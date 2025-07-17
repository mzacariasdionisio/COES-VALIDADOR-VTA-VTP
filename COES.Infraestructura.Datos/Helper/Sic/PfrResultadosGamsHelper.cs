using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_RESULTADOS_GAMS
    /// </summary>
    public class PfrResultadosGamsHelper : HelperBase
    {
        public PfrResultadosGamsHelper() : base(Consultas.PfrResultadosGamsSql)
        {
        }

        public PfrResultadosGamsDTO Create(IDataReader dr)
        {
            PfrResultadosGamsDTO entity = new PfrResultadosGamsDTO();

            int iPfrrgcodi = dr.GetOrdinal(this.Pfrrgcodi);
            if (!dr.IsDBNull(iPfrrgcodi)) entity.Pfrrgcodi = Convert.ToInt32(dr.GetValue(iPfrrgcodi));

            int iPfresccodi = dr.GetOrdinal(this.Pfresccodi);
            if (!dr.IsDBNull(iPfresccodi)) entity.Pfresccodi = Convert.ToInt32(dr.GetValue(iPfresccodi));

            int iPfrrgecodi = dr.GetOrdinal(this.Pfrrgecodi);
            if (!dr.IsDBNull(iPfrrgecodi)) entity.Pfrrgecodi = Convert.ToInt32(dr.GetValue(iPfrrgecodi));

            int iPfreqpcodi = dr.GetOrdinal(this.Pfreqpcodi);
            if (!dr.IsDBNull(iPfreqpcodi)) entity.Pfreqpcodi = Convert.ToInt32(dr.GetValue(iPfreqpcodi));

            int iPfrcgtcodi = dr.GetOrdinal(this.Pfrcgtcodi);
            if (!dr.IsDBNull(iPfrcgtcodi)) entity.Pfrcgtcodi = Convert.ToInt32(dr.GetValue(iPfrcgtcodi));

            int iPfrrgtipo = dr.GetOrdinal(this.Pfrrgtipo);
            if (!dr.IsDBNull(iPfrrgtipo)) entity.Pfrrgtipo = Convert.ToInt32(dr.GetValue(iPfrrgtipo));

            int iPfrrgresultado = dr.GetOrdinal(this.Pfrrgresultado);
            if (!dr.IsDBNull(iPfrrgresultado)) entity.Pfrrgresultado = dr.GetDecimal(iPfrrgresultado);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrrgcodi = "PFRRGCODI";
        public string Pfresccodi = "PFRESCCODI";
        public string Pfrrgecodi = "PFRRGECODI";
        public string Pfreqpcodi = "PFREQPCODI";
        public string Pfrcgtcodi = "PFRCGTCODI";
        public string Pfrrgtipo = "PFRRGTIPO";
        public string Pfrrgresultado = "PFRRGRESULTADO";

        #endregion

        public string SqlListaPorTipoYEscenario
        {
            get { return base.GetSqlXml("ListaPorTipoYEscenario"); }
        }
    }
}
