using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_AREA
    /// </summary>
    public class FtExtEnvioAreaHelper : HelperBase
    {
        public FtExtEnvioAreaHelper(): base(Consultas.FtExtEnvioAreaSql)
        {
        }

        public FtExtEnvioAreaDTO Create(IDataReader dr)
        {
            FtExtEnvioAreaDTO entity = new FtExtEnvioAreaDTO();

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            int iFaremcodi = dr.GetOrdinal(this.Faremcodi);
            if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

            int iEnvarcodi = dr.GetOrdinal(this.Envarcodi);
            if (!dr.IsDBNull(iEnvarcodi)) entity.Envarcodi = Convert.ToInt32(dr.GetValue(iEnvarcodi));

            int iEnvarfecmaxrpta = dr.GetOrdinal(this.Envarfecmaxrpta);
            if (!dr.IsDBNull(iEnvarfecmaxrpta)) entity.Envarfecmaxrpta = dr.GetDateTime(iEnvarfecmaxrpta);

            int iEnvarestado = dr.GetOrdinal(this.Envarestado);
            if (!dr.IsDBNull(iEnvarestado)) entity.Envarestado = dr.GetString(iEnvarestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftevercodi = "FTEVERCODI";
        public string Faremcodi = "FAREMCODI";
        public string Envarcodi = "ENVARCODI";
        public string Envarfecmaxrpta = "ENVARFECMAXRPTA";
        public string Envarestado = "ENVARESTADO";

        public string Ftenvcodi = "FTENVCODI";
        public string Estenvcodi = "ESTENVCODI";
        
        #endregion

        public string SqlListarPorVersiones
        {
            get { return base.GetSqlXml("ListarPorVersiones"); }
        }

        public string SqlListarPorEnvioCarpetaYEstado
        {
            get { return base.GetSqlXml("ListarPorEnvioCarpetaYEstado"); }
        }

        public string SqlGetByVersionYArea
        {
            get { return base.GetSqlXml("GetByVersionYArea"); }
        }
        
    }
}
