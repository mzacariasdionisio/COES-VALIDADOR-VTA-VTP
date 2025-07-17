using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PERIODOMEDIDOR
    /// </summary>
    public class MePeriodomedidorHelper : HelperBase
    {
        public MePeriodomedidorHelper()
            : base(Consultas.MePeriodomedidorSql)
        {
        }

        public MePeriodomedidorDTO Create(IDataReader dr)
        {
            MePeriodomedidorDTO entity = new MePeriodomedidorDTO();

            int iMedicodi = dr.GetOrdinal(this.Medicodi);
            if (!dr.IsDBNull(iMedicodi)) entity.Medicodi = Convert.ToInt32(dr.GetValue(iMedicodi));

            int iEarcodi = dr.GetOrdinal(this.Earcodi);
            if (!dr.IsDBNull(iEarcodi)) entity.Earcodi = Convert.ToInt32(dr.GetValue(iEarcodi));

            int iPermedifechaini = dr.GetOrdinal(this.Permedifechaini);
            if (!dr.IsDBNull(iPermedifechaini)) entity.Permedifechaini = dr.GetDateTime(iPermedifechaini);

            int iPermedifechafin = dr.GetOrdinal(this.Permedifechafin);
            if (!dr.IsDBNull(iPermedifechafin)) entity.Permedifechafin = dr.GetDateTime(iPermedifechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Medicodi = "MEDICODI";
        public string Earcodi = "ENVIOCODI";
        public string Permedifechaini = "PERMEDIFECHAINI";
        public string Permedifechafin = "PERMEDIFECHAFIN";

        public string Medinombre = "MEDINOMBRE";

        #endregion
        
        public string SqlGetByCriteriaRango
        {
            get { return base.GetSqlXml("GetByCriteriaRango"); }
        }
    }
}
