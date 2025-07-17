using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_RELACION_INDISPONIBILIDAD
    /// </summary>
    public class PfrRelacionIndisponibilidadHelper : HelperBase
    {
        public PfrRelacionIndisponibilidadHelper(): base(Consultas.PfrRelacionIndisponibilidadSql)
        {
        }

        public PfrRelacionIndisponibilidadDTO Create(IDataReader dr)
        {
            PfrRelacionIndisponibilidadDTO entity = new PfrRelacionIndisponibilidadDTO();

            int iPfrrincodi = dr.GetOrdinal(this.Pfrrincodi);
            if (!dr.IsDBNull(iPfrrincodi)) entity.Pfrrincodi = Convert.ToInt32(dr.GetValue(iPfrrincodi));

            int iPfrrptcodi = dr.GetOrdinal(this.Pfrrptcodi);
            if (!dr.IsDBNull(iPfrrptcodi)) entity.Pfrrptcodi = Convert.ToInt32(dr.GetValue(iPfrrptcodi));

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            int iPfrrintipo = dr.GetOrdinal(this.Pfrrintipo);
            if (!dr.IsDBNull(iPfrrintipo)) entity.Pfrrintipo = Convert.ToInt32(dr.GetValue(iPfrrintipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrrincodi = "PFRRINCODI";
        public string Pfrrptcodi = "PFRRPTCODI";
        public string Irptcodi = "IRPTCODI";
        public string Pfrrintipo = "PFRRINTIPO";

        #endregion
    }
}
