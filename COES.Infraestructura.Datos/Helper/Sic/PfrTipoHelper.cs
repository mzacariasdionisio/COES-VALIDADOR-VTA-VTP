using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_TIPO
    /// </summary>
    public class PfrTipoHelper : HelperBase
    {
        public PfrTipoHelper(): base(Consultas.PfrTipoSql)
        {
        }

        public PfrTipoDTO Create(IDataReader dr)
        {
            PfrTipoDTO entity = new PfrTipoDTO();

            int iPfrcatcodi = dr.GetOrdinal(this.Pfrcatcodi);
            if (!dr.IsDBNull(iPfrcatcodi)) entity.Pfrcatcodi = Convert.ToInt32(dr.GetValue(iPfrcatcodi));

            int iPfrcatnomb = dr.GetOrdinal(this.Pfrcatnomb);
            if (!dr.IsDBNull(iPfrcatnomb)) entity.Pfrcatnomb = dr.GetString(iPfrcatnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrcatcodi = "PFRCATCODI";
        public string Pfrcatnomb = "PFRCATNOMB";

        #endregion
    }
}
