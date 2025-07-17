using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_ENVIORELCV
    /// </summary>
    public class CbEnviorelcvHelper : HelperBase
    {
        public CbEnviorelcvHelper(): base(Consultas.CbEnviorelcvSql)
        {
        }

        public CbEnviorelcvDTO Create(IDataReader dr)
        {
            CbEnviorelcvDTO entity = new CbEnviorelcvDTO();

            int iCbcvcodi = dr.GetOrdinal(this.Cbcvcodi);
            if (!dr.IsDBNull(iCbcvcodi)) entity.Cbcvcodi = Convert.ToInt32(dr.GetValue(iCbcvcodi));

            int iCbenvcodi = dr.GetOrdinal(this.Cbenvcodi);
            if (!dr.IsDBNull(iCbenvcodi)) entity.Cbenvcodi = Convert.ToInt32(dr.GetValue(iCbenvcodi));

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Cbcvcodi = "CBCVCODI";
        public string Cbenvcodi = "CBENVCODI";
        public string Repcodi = "REPCODI";

        #endregion
    }
}
