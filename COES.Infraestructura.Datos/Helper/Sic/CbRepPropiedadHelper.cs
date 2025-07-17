using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_REP_PROPIEDAD
    /// </summary>
    public class CbRepPropiedadHelper : HelperBase
    {
        public CbRepPropiedadHelper(): base(Consultas.CbRepPropiedadSql)
        {
        }

        public CbRepPropiedadDTO Create(IDataReader dr)
        {
            CbRepPropiedadDTO entity = new CbRepPropiedadDTO();

            int iCbrprocodi = dr.GetOrdinal(this.Cbrprocodi);
            if (!dr.IsDBNull(iCbrprocodi)) entity.Cbrprocodi = Convert.ToInt32(dr.GetValue(iCbrprocodi));

            int iCbrpronombre = dr.GetOrdinal(this.Cbrpronombre);
            if (!dr.IsDBNull(iCbrpronombre)) entity.Cbrpronombre = dr.GetString(iCbrpronombre);

            int iCbrprovalor = dr.GetOrdinal(this.Cbrprovalor);
            if (!dr.IsDBNull(iCbrprovalor)) entity.Cbrprovalor = dr.GetString(iCbrprovalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbrprocodi = "CBRPROCODI";
        public string Cbrpronombre = "CBRPRONOMBRE";
        public string Cbrprovalor = "CBRPROVALOR";

        #endregion
    }
}
