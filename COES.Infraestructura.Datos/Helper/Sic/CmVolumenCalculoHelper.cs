using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_VOLUMEN_CALCULO
    /// </summary>
    public class CmVolumenCalculoHelper : HelperBase
    {
        public CmVolumenCalculoHelper() : base(Consultas.CmVolumenCalculoSql)
        {
        }

        public CmVolumenCalculoDTO Create(IDataReader dr)
        {
            CmVolumenCalculoDTO entity = new CmVolumenCalculoDTO();

            int iVolcalcodi = dr.GetOrdinal(this.Volcalcodi);
            if (!dr.IsDBNull(iVolcalcodi)) entity.Volcalcodi = Convert.ToInt32(dr.GetValue(iVolcalcodi));

            int iVolcalfecha = dr.GetOrdinal(this.Volcalfecha);
            if (!dr.IsDBNull(iVolcalfecha)) entity.Volcalfecha = dr.GetDateTime(iVolcalfecha);

            int iVolcalperiodo = dr.GetOrdinal(this.Volcalperiodo);
            if (!dr.IsDBNull(iVolcalperiodo)) entity.Volcalperiodo = Convert.ToInt32(dr.GetValue(iVolcalperiodo));

            int iVolcaltipo = dr.GetOrdinal(this.Volcaltipo);
            if (!dr.IsDBNull(iVolcaltipo)) entity.Volcaltipo = dr.GetString(iVolcaltipo);

            int iVolcalusucreacion = dr.GetOrdinal(this.Volcalusucreacion);
            if (!dr.IsDBNull(iVolcalusucreacion)) entity.Volcalusucreacion = dr.GetString(iVolcalusucreacion);

            int iVolcalfeccreacion = dr.GetOrdinal(this.Volcalfeccreacion);
            if (!dr.IsDBNull(iVolcalfeccreacion)) entity.Volcalfeccreacion = dr.GetDateTime(iVolcalfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Volcalcodi = "VOLCALCODI";
        public string Volcalfecha = "VOLCALFECHA";
        public string Volcalperiodo = "VOLCALPERIODO";
        public string Volcaltipo = "VOLCALTIPO";
        public string Volcalusucreacion = "VOLCALUSUCREACION";
        public string Volcalfeccreacion = "VOLCALFECCREACION";

        #endregion
    }
}
