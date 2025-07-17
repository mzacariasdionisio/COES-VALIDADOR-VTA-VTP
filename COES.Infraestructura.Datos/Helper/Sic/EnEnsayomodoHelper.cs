using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ENSAYOMODO
    /// </summary>
    public class EnEnsayomodoHelper : HelperBase
    {
        public EnEnsayomodoHelper()
            : base(Consultas.EnEnsayomodoSql)
        {
        }

        public EnEnsayomodoDTO Create(IDataReader dr)
        {
            EnEnsayomodoDTO entity = new EnEnsayomodoDTO();

            int iEnsayocodi = dr.GetOrdinal(this.Ensayocodi);
            if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEnmodocodi = dr.GetOrdinal(this.Enmodocodi);
            if (!dr.IsDBNull(iEnmodocodi)) entity.Enmodocodi = Convert.ToInt32(dr.GetValue(iEnmodocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ensayocodi = "ENSAYOCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Enmodocodi = "ENMODOCODI";

        #endregion


        public string SqlListarEnsayoModo
        {
            get { return base.GetSqlXml("ListarEnsayosModo"); }
        }
    }
}
