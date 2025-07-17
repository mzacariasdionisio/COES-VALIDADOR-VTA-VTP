using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_RELACION_POTENCIA_FIRME
    /// </summary>
    public class PfrRelacionPotenciaFirmeHelper : HelperBase
    {
        public PfrRelacionPotenciaFirmeHelper(): base(Consultas.PfrRelacionPotenciaFirmeSql)
        {
        }

        public PfrRelacionPotenciaFirmeDTO Create(IDataReader dr)
        {
            PfrRelacionPotenciaFirmeDTO entity = new PfrRelacionPotenciaFirmeDTO();

            int iPfrrpfcodi = dr.GetOrdinal(this.Pfrrpfcodi);
            if (!dr.IsDBNull(iPfrrpfcodi)) entity.Pfrrpfcodi = Convert.ToInt32(dr.GetValue(iPfrrpfcodi));

            int iPfrrptcodi = dr.GetOrdinal(this.Pfrrptcodi);
            if (!dr.IsDBNull(iPfrrptcodi)) entity.Pfrrptcodi = Convert.ToInt32(dr.GetValue(iPfrrptcodi));

            int iPfrptcodi = dr.GetOrdinal(this.Pfrptcodi);
            if (!dr.IsDBNull(iPfrptcodi)) entity.Pfrptcodi = Convert.ToInt32(dr.GetValue(iPfrptcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrrpfcodi = "PFRRPFCODI";
        public string Pfrrptcodi = "PFRRPTCODI";
        public string Pfrptcodi = "PFRPTCODI";

        #endregion
    }
}
