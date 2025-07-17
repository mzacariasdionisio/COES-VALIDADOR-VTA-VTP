using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_RELACION_INDISPONIBILIDADES
    /// </summary>
    public class PfRelacionIndisponibilidadesHelper : HelperBase
    {
        public PfRelacionIndisponibilidadesHelper() : base(Consultas.PfRelacionIndisponibilidadesSql)
        {
        }

        public PfRelacionIndisponibilidadesDTO Create(IDataReader dr)
        {
            PfRelacionIndisponibilidadesDTO entity = new PfRelacionIndisponibilidadesDTO();

            int iPfrptcodi = dr.GetOrdinal(this.Pfrptcodi);
            if (!dr.IsDBNull(iPfrptcodi)) entity.Pfrptcodi = Convert.ToInt32(dr.GetValue(iPfrptcodi));

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            int iPfrindcodi = dr.GetOrdinal(this.Pfrindcodi);
            if (!dr.IsDBNull(iPfrindcodi)) entity.Pfrindcodi = Convert.ToInt32(dr.GetValue(iPfrindcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Pfrptcodi = "PFRPTCODI";
        public string Irptcodi = "IRPTCODI";
        public string Pfrindcodi = "PFRINDCODI";

        public string Icuacodi = "ICUACODI";

        #endregion
    }
}
