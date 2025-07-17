using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_REQ
    /// </summary>
    public class FtExtEnvioReqHelper : HelperBase
    {
        public FtExtEnvioReqHelper() : base(Consultas.FtExtEnvioReqSql)
        {
        }

        public FtExtEnvioReqDTO Create(IDataReader dr)
        {
            FtExtEnvioReqDTO entity = new FtExtEnvioReqDTO();

            int iFtereqcodi = dr.GetOrdinal(this.Ftereqcodi);
            if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

            int iFevrqcodi = dr.GetOrdinal(this.Fevrqcodi);
            if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            int iFtereqflagarchivo = dr.GetOrdinal(this.Ftereqflagarchivo);
            if (!dr.IsDBNull(iFtereqflagarchivo)) entity.Ftereqflagarchivo = Convert.ToInt32(dr.GetValue(iFtereqflagarchivo));

            int iFtereqflageditable = dr.GetOrdinal(this.Ftereqflageditable);
            if (!dr.IsDBNull(iFtereqflageditable)) entity.Ftereqflageditable = dr.GetString(iFtereqflageditable);

            int iFtereqflagrevisable = dr.GetOrdinal(this.Ftereqflagrevisable);
            if (!dr.IsDBNull(iFtereqflagrevisable)) entity.Ftereqflagrevisable = dr.GetString(iFtereqflagrevisable);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftereqcodi = "FTEREQCODI";
        public string Fevrqcodi = "FEVRQCODI";
        public string Ftevercodi = "FTEVERCODI";
        public string Ftereqflagarchivo = "FTEREQFLAGARCHIVO";
        public string Ftereqflageditable = "Ftereqflageditable";
        public string Ftereqflagrevisable = "Ftereqflagrevisable";

        public string Fevrqliteral = "FEVRQLITERAL";
        
        #endregion

        public string SqlGetListByVersiones
        {
            get { return GetSqlXml("GetListByVersiones"); }
        }
    }
}
