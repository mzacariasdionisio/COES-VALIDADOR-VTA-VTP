using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_CONCEPTO
    /// </summary>
    public class SpoConceptoHelper : HelperBase
    {
        public SpoConceptoHelper(): base(Consultas.SpoConceptoSql)
        {
        }

        public SpoConceptoDTO Create(IDataReader dr)
        {
            SpoConceptoDTO entity = new SpoConceptoDTO();

            int iSconcodi = dr.GetOrdinal(this.Sconcodi);
            if (!dr.IsDBNull(iSconcodi)) entity.Sconcodi = Convert.ToInt32(dr.GetValue(iSconcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPtomedicodi2 = dr.GetOrdinal(this.Ptomedicodi2);
            if (!dr.IsDBNull(iPtomedicodi2)) entity.Ptomedicodi2 = Convert.ToInt32(dr.GetValue(iPtomedicodi2));

            int iNumccodi = dr.GetOrdinal(this.Numccodi);
            if (!dr.IsDBNull(iNumccodi)) entity.Numccodi = Convert.ToInt32(dr.GetValue(iNumccodi));

            int iSconnomb = dr.GetOrdinal(this.Sconnomb);
            if (!dr.IsDBNull(iSconnomb)) entity.Sconnomb = dr.GetString(iSconnomb);

            int iSconactivo = dr.GetOrdinal(this.Sconactivo);
            if (!dr.IsDBNull(iSconactivo)) entity.Sconactivo = Convert.ToInt32(dr.GetValue(iSconactivo));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Sconcodi = "SCONCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedicodi2 = "PTOMEDICODI2";
        public string Numccodi = "NUMCCODI";
        public string Sconnomb = "SCONNOMB";
        public string Sconactivo = "SCONACTIVO";
        public string Lectcodi = "LECTCODI";

        public string Ptomedicalculado1 = "Ptomedicalculado1";
        public string Ptomedicalculado2 = "Ptomedicalculado2";

        #endregion
    }
}
