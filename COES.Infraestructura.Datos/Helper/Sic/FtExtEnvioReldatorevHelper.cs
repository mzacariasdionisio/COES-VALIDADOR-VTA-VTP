using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELDATOREV
    /// </summary>
    public class FtExtEnvioReldatorevHelper : HelperBase
    {
        public FtExtEnvioReldatorevHelper(): base(Consultas.FtExtEnvioReldatorevSql)
        {
        }

        public FtExtEnvioReldatorevDTO Create(IDataReader dr)
        {
            FtExtEnvioReldatorevDTO entity = new FtExtEnvioReldatorevDTO();

            int iFtedatcodi = dr.GetOrdinal(this.Ftedatcodi);
            if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

            int iFtrevcodi = dr.GetOrdinal(this.Ftrevcodi);
            if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

            int iFrdrevcodi = dr.GetOrdinal(this.Frdrevcodi);
            if (!dr.IsDBNull(iFrdrevcodi)) entity.Frdrevcodi = Convert.ToInt32(dr.GetValue(iFrdrevcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ftedatcodi = "FTEDATCODI";
        public string Ftrevcodi = "FTREVCODI";
        public string Frdrevcodi = "FRDREVCODI";

        #endregion

        public string SqlGetByDatos
        {
            get { return GetSqlXml("GetByDatos"); }
        }
        
    }
}
