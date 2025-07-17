using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELREQREVAREA
    /// </summary>
    public class FtExtEnvioRelreqrevareaHelper : HelperBase
    {
        public FtExtEnvioRelreqrevareaHelper(): base(Consultas.FtExtEnvioRelreqrevareaSql)
        {
        }

        public FtExtEnvioRelreqrevareaDTO Create(IDataReader dr)
        {
            FtExtEnvioRelreqrevareaDTO entity = new FtExtEnvioRelreqrevareaDTO();

            int iRevarqcodi = dr.GetOrdinal(this.Revarqcodi);
            if (!dr.IsDBNull(iRevarqcodi)) entity.Revarqcodi = Convert.ToInt32(dr.GetValue(iRevarqcodi));

            int iRevacodi = dr.GetOrdinal(this.Revacodi);
            if (!dr.IsDBNull(iRevacodi)) entity.Revacodi = Convert.ToInt32(dr.GetValue(iRevacodi));

            int iFtereqcodi = dr.GetOrdinal(this.Ftereqcodi);
            if (!dr.IsDBNull(iFtereqcodi)) entity.Ftereqcodi = Convert.ToInt32(dr.GetValue(iFtereqcodi));

            int iEnvarcodi = dr.GetOrdinal(this.Envarcodi);
            if (!dr.IsDBNull(iEnvarcodi)) entity.Envarcodi = Convert.ToInt32(dr.GetValue(iEnvarcodi));

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Revarqcodi = "REVARQCODI";
        public string Revacodi = "REVACODI";
        public string Ftereqcodi = "FTEREQCODI";
        public string Envarcodi = "ENVARCODI";
        public string Ftevercodi = "FTEVERCODI";

        #endregion

        public string SqlDeletePorIds
        {
            get { return GetSqlXml("DeletePorIds"); }
        }

        public string SqlListarRelacionesPorVersionArea
        {
            get { return GetSqlXml("ListarRelacionesPorVersionArea"); }
        }
    }
}
