using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELREVARCHIVO
    /// </summary>
    public class FtExtEnvioRelrevarchivoHelper : HelperBase
    {
        public FtExtEnvioRelrevarchivoHelper(): base(Consultas.FtExtEnvioRelrevarchivoSql)
        {
        }

        public FtExtEnvioRelrevarchivoDTO Create(IDataReader dr)
        {
            FtExtEnvioRelrevarchivoDTO entity = new FtExtEnvioRelrevarchivoDTO();

            int iFtearccodi = dr.GetOrdinal(this.Ftearccodi);
            if (!dr.IsDBNull(iFtearccodi)) entity.Ftearccodi = Convert.ToInt32(dr.GetValue(iFtearccodi));

            int iFtrevcodi = dr.GetOrdinal(this.Ftrevcodi);
            if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

            int iFtrrvacodi = dr.GetOrdinal(this.Ftrrvacodi);
            if (!dr.IsDBNull(iFtrrvacodi)) entity.Ftrrvacodi = Convert.ToInt32(dr.GetValue(iFtrrvacodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ftearccodi = "FTEARCCODI";
        public string Ftrevcodi = "FTREVCODI";
        public string Ftrrvacodi = "FTRRVACODI";

        #endregion

        public string SqlGetByRevision
        {
            get { return GetSqlXml("GetByRevision"); }
        }
        
    }
}
