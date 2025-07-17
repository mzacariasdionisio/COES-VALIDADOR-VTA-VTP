using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_DATO
    /// </summary>
    public class FtExtEnvioDatoHelper : HelperBase
    {
        public FtExtEnvioDatoHelper() : base(Consultas.FtExtEnvioDatoSql)
        {
        }

        public FtExtEnvioDatoDTO Create(IDataReader dr)
        {
            FtExtEnvioDatoDTO entity = new FtExtEnvioDatoDTO();

            int iFteeqcodi = dr.GetOrdinal(this.Fteeqcodi);
            if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

            int iFtedatcodi = dr.GetOrdinal(this.Ftedatcodi);
            if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

            int iFitcfgcodi = dr.GetOrdinal(this.Fitcfgcodi);
            if (!dr.IsDBNull(iFitcfgcodi)) entity.Fitcfgcodi = Convert.ToInt32(dr.GetValue(iFitcfgcodi));

            int iFtedatvalor = dr.GetOrdinal(this.Ftedatvalor);
            if (!dr.IsDBNull(iFtedatvalor)) entity.Ftedatvalor = dr.GetString(iFtedatvalor);

            int iFtedatflagvalorconf = dr.GetOrdinal(this.Ftedatflagvalorconf);
            if (!dr.IsDBNull(iFtedatflagvalorconf)) entity.Ftedatflagvalorconf = dr.GetString(iFtedatflagvalorconf);

            int iFtedatcomentario = dr.GetOrdinal(this.Ftedatcomentario);
            if (!dr.IsDBNull(iFtedatcomentario)) entity.Ftedatcomentario = dr.GetString(iFtedatcomentario);

            int iFtedatflagsustentoconf = dr.GetOrdinal(this.Ftedatflagsustentoconf);
            if (!dr.IsDBNull(iFtedatflagsustentoconf)) entity.Ftedatflagsustentoconf = dr.GetString(iFtedatflagsustentoconf);

            int iFtedatflagmodificado = dr.GetOrdinal(this.Ftedatflagmodificado);
            if (!dr.IsDBNull(iFtedatflagmodificado)) entity.Ftedatflagmodificado = Convert.ToInt32(dr.GetValue(iFtedatflagmodificado));

            int iFtedatflageditable = dr.GetOrdinal(this.Ftedatflageditable);
            if (!dr.IsDBNull(iFtedatflageditable)) entity.Ftedatflageditable = dr.GetString(iFtedatflageditable);

            int iFtedatflagrevisable = dr.GetOrdinal(this.Ftedatflagrevisable);
            if (!dr.IsDBNull(iFtedatflagrevisable)) entity.Ftedatflagrevisable = dr.GetString(iFtedatflagrevisable);

            return entity;
        }

        #region Mapeo de Campos

        public string Fteeqcodi = "FTEEQCODI";
        public string Ftedatcodi = "FTEDATCODI";
        public string Fitcfgcodi = "FITCFGCODI";
        public string Ftedatvalor = "FTEDATVALOR";
        public string Ftedatflagvalorconf = "FTEDATFLAGVALORCONF";
        public string Ftedatcomentario = "FTEDATCOMENTARIO";
        public string Ftedatflagsustentoconf = "FTEDATFLAGSUSTENTOCONF";
        public string Ftedatflagmodificado = "FTEDATFLAGMODIFICADO";
        public string Ftedatflageditable = "FTEDATFLAGEDITABLE";
        public string Ftedatflagrevisable = "FTEDATFLAGREVISABLE";

        public string Ftitcodi = "FTITCODI";
        public string Ftitactivo = "FTITACTIVO";        
        public string Tipoelemento = "TIPOELEMENTO";
        public string Concepcodi = "CONCEPCODI";
        public string Propcodi = "PROPCODI";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Concepabrev = "CONCEPABREV";
        public string Propabrev = "PROPABREV";

        #endregion

        public string SqlUpdateXVersion
        {
            get { return GetSqlXml("UpdateXVersion"); }
        }
        public string SqlUpdateXEquipo
        {
            get { return GetSqlXml("UpdateXEquipo"); }
        }
        public string SqlUpdateXFtedatcodis
        {
            get { return GetSqlXml("UpdateXFtedatcodis"); }
        }
        public string SqlListarParametros
        {
            get { return GetSqlXml("ListarParametros"); }
        }
        
    }
}
