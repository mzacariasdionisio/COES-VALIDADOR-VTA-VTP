using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELREQREV
    /// </summary>
    public class FtExtEnvioRelreqrevHelper : HelperBase
    {
        public FtExtEnvioRelreqrevHelper(): base(Consultas.FtExtEnvioRelreqrevSql)
        {
        }

        public FtExtEnvioRelreqrevDTO Create(IDataReader dr)
        {
            FtExtEnvioRelreqrevDTO entity = new FtExtEnvioRelreqrevDTO();

            int iFtrevcodi = dr.GetOrdinal(this.Ftrevcodi);
            if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

            int iFrrrevcodi = dr.GetOrdinal(this.Frrrevcodi);
            if (!dr.IsDBNull(iFrrrevcodi)) entity.Frrrevcodi = Convert.ToInt32(dr.GetValue(iFrrrevcodi));

            int iFtereqcodi = dr.GetOrdinal(this.Ftereqcodi);
            if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ftrevcodi = "FTREVCODI";
        public string Frrrevcodi = "FRRREVCODI";
        public string Ftereqcodi = "FTEREQCODI";

        #endregion

        public string SqlGetByRequisitos
        {
            get { return GetSqlXml("GetByRequisitos"); }
        }
    }
}
