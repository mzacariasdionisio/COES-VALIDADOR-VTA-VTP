using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELDATOARCHIVO
    /// </summary>
    public class FtExtEnvioReldatoarchivoHelper : HelperBase
    {
        public FtExtEnvioReldatoarchivoHelper() : base(Consultas.FtExtEnvioReldatoarchivoSql)
        {
        }

        public FtExtEnvioReldatoarchivoDTO Create(IDataReader dr)
        {
            FtExtEnvioReldatoarchivoDTO entity = new FtExtEnvioReldatoarchivoDTO();

            int iFtedatcodi = dr.GetOrdinal(this.Ftedatcodi);
            if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

            int iFtearccodi = dr.GetOrdinal(this.Ftearccodi);
            if (!dr.IsDBNull(iFtearccodi)) entity.Ftearccodi = Convert.ToInt32(dr.GetValue(iFtearccodi));

            int iFterdacodi = dr.GetOrdinal(this.Fterdacodi);
            if (!dr.IsDBNull(iFterdacodi)) entity.Fterdacodi = Convert.ToInt32(dr.GetValue(iFterdacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftedatcodi = "FTEDATCODI";
        public string Ftearccodi = "FTEARCCODI";
        public string Fterdacodi = "FTERDACODI";

        #endregion
    }
}
