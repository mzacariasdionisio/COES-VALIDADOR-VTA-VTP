using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_CONCEPTO
    /// </summary>
    public class PfrConceptoHelper : HelperBase
    {
        public PfrConceptoHelper(): base(Consultas.PfrConceptoSql)
        {
        }

        public PfrConceptoDTO Create(IDataReader dr)
        {
            PfrConceptoDTO entity = new PfrConceptoDTO();

            int iPfrcnpcodi = dr.GetOrdinal(this.Pfrcnpcodi);
            if (!dr.IsDBNull(iPfrcnpcodi)) entity.Pfrcnpcodi = Convert.ToInt32(dr.GetValue(iPfrcnpcodi));

            int iPfrcnpnomb = dr.GetOrdinal(this.Pfrcnpnomb);
            if (!dr.IsDBNull(iPfrcnpnomb)) entity.Pfrcnpnomb = dr.GetString(iPfrcnpnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrcnpcodi = "PFRCNPCODI";
        public string Pfrcnpnomb = "PFRCNPNOMB";

        #endregion
    }
}
