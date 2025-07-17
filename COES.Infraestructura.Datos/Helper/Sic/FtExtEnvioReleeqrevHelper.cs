using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELEEQREV
    /// </summary>
    public class FtExtEnvioReleeqrevHelper : HelperBase
    {
        public FtExtEnvioReleeqrevHelper(): base(Consultas.FtExtEnvioReleeqrevSql)
        {
        }

        public FtExtEnvioReleeqrevDTO Create(IDataReader dr)
        {
            FtExtEnvioReleeqrevDTO entity = new FtExtEnvioReleeqrevDTO();

            int iFreqrvcodi = dr.GetOrdinal(this.Freqrvcodi);
            if (!dr.IsDBNull(iFreqrvcodi)) entity.Freqrvcodi = Convert.ToInt32(dr.GetValue(iFreqrvcodi));

            int iFteeqcodi = dr.GetOrdinal(this.Fteeqcodi);
            if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

            int iFtrevcodi = dr.GetOrdinal(this.Ftrevcodi);
            if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Freqrvcodi = "FREQRVCODI";
        public string Fteeqcodi = "FTEEQCODI";
        public string Ftrevcodi = "FTREVCODI";

        #endregion

        public string SqlGetByEquipos
        {
            get { return GetSqlXml("GetByEquipos"); }
        }
        
    }
}
