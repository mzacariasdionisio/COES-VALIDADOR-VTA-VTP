using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ENSAYOMODEQUI
    /// </summary>
    public class EnEnsayomodequiHelper : HelperBase
    {
        public EnEnsayomodequiHelper()
            : base(Consultas.EnEnsayomodequiSql)
        {
        }

        public EnEnsayomodequiDTO Create(IDataReader dr)
        {
            EnEnsayomodequiDTO entity = new EnEnsayomodequiDTO();

            int iEnmodocodi = dr.GetOrdinal(this.Enmodocodi);
            if (!dr.IsDBNull(iEnmodocodi)) entity.Enmodocodi = Convert.ToInt32(dr.GetValue(iEnmodocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEnmoeqcodi = dr.GetOrdinal(this.Enmoeqcodi);
            if (!dr.IsDBNull(iEnmoeqcodi)) entity.Enmoeqcodi = Convert.ToInt32(dr.GetValue(iEnmoeqcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Enmodocodi = "ENMODOCODI";
        public string Equicodi = "EQUICODI";
        public string Enmoeqcodi = "ENMOEQCODI";

        #endregion
    }
}
