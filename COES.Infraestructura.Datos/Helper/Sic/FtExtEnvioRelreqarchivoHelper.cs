using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELREQARCHIVO
    /// </summary>
    public class FtExtEnvioRelreqarchivoHelper : HelperBase
    {
        public FtExtEnvioRelreqarchivoHelper() : base(Consultas.FtExtEnvioRelreqarchivoSql)
        {
        }

        public FtExtEnvioRelreqarchivoDTO Create(IDataReader dr)
        {
            FtExtEnvioRelreqarchivoDTO entity = new FtExtEnvioRelreqarchivoDTO();

            int iFterracodi = dr.GetOrdinal(this.Fterracodi);
            if (!dr.IsDBNull(iFterracodi)) entity.Fterracodi = Convert.ToInt32(dr.GetValue(iFterracodi));

            int iFtereqcodi = dr.GetOrdinal(this.Ftereqcodi);
            if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

            int iFtearccodi = dr.GetOrdinal(this.Ftearccodi);
            if (!dr.IsDBNull(iFtearccodi)) entity.Ftearccodi = Convert.ToInt32(dr.GetValue(iFtearccodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Fterracodi = "FTERRACODI";
        public string Ftereqcodi = "FTEREQCODI";
        public string Ftearccodi = "FTEARCCODI";

        #endregion

        public string SqlGetByRequisitos
        {
            get { return GetSqlXml("GetByRequisitos"); }
        }
        
    }
}
