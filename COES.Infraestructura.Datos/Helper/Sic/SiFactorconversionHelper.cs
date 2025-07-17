using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_FACTORCONVERSION
    /// </summary>
    public class SiFactorconversionHelper : HelperBase
    {
        public SiFactorconversionHelper() : base(Consultas.SiFactorconversionSql)
        {
        }

        public SiFactorconversionDTO Create(IDataReader dr)
        {
            SiFactorconversionDTO entity = new SiFactorconversionDTO();

            int iTconvcodi = dr.GetOrdinal(this.Tconvcodi);
            if (!dr.IsDBNull(iTconvcodi)) entity.Tconvcodi = Convert.ToInt32(dr.GetValue(iTconvcodi));

            int iTinforigen = dr.GetOrdinal(this.Tinforigen);
            if (!dr.IsDBNull(iTinforigen)) entity.Tinforigen = Convert.ToInt32(dr.GetValue(iTinforigen));

            int iTinfdestino = dr.GetOrdinal(this.Tinfdestino);
            if (!dr.IsDBNull(iTinfdestino)) entity.Tinfdestino = Convert.ToInt32(dr.GetValue(iTinfdestino));

            int iTconvfactor = dr.GetOrdinal(this.Tconvfactor);
            if (!dr.IsDBNull(iTconvfactor)) entity.Tconvfactor = dr.GetDecimal(iTconvfactor);

            return entity;
        }

        #region Mapeo de Campos

        public string Tconvcodi = "TCONVCODI";
        public string Tinforigen = "TINFORIGEN";
        public string Tinfdestino = "TINFDESTINO";
        public string Tconvfactor = "TCONVFACTOR";
        public string Tinforigenabrev = "TINFORIGENABREV";
        public string Tinfdestinoabrev = "TINFDESTINOABREV";
        public string Tinforigendesc = "TINFORIGENdesc";
        public string Tinfdestinodesc = "TINFDESTINOdesc";

        #endregion
    }
}
