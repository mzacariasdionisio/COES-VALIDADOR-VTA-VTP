using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_RELACION_INSUMOS
    /// </summary>
    public class PfRelacionInsumosHelper : HelperBase
    {
        public PfRelacionInsumosHelper(): base(Consultas.PfRelacionInsumosSql)
        {
        }

        public PfRelacionInsumosDTO Create(IDataReader dr)
        {
            PfRelacionInsumosDTO entity = new PfRelacionInsumosDTO();

            int iPfrptcodi = dr.GetOrdinal(this.Pfrptcodi);
            if (!dr.IsDBNull(iPfrptcodi)) entity.Pfrptcodi = Convert.ToInt32(dr.GetValue(iPfrptcodi));

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iPfrinscodi = dr.GetOrdinal(this.Pfrinscodi);
            if (!dr.IsDBNull(iPfrinscodi)) entity.Pfrinscodi = Convert.ToInt32(dr.GetValue(iPfrinscodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrptcodi = "PFRPTCODI";
        public string Pfverscodi = "PFVERSCODI";
        public string Pfrinscodi = "PFRINSCODI";

        //campos adicionales
        public string Pfrecucodi = "PFRECUCODI";

        #endregion
    }
}
