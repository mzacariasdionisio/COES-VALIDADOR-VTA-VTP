using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELDATOREVAREA
    /// </summary>
    public class FtExtEnvioReldatorevareaHelper : HelperBase
    {
        public FtExtEnvioReldatorevareaHelper(): base(Consultas.FtExtEnvioReldatorevareaSql)
        {
        }

        public FtExtEnvioReldatorevareaDTO Create(IDataReader dr)
        {
            FtExtEnvioReldatorevareaDTO entity = new FtExtEnvioReldatorevareaDTO();

            int iRevadcodi = dr.GetOrdinal(this.Revadcodi);
            if (!dr.IsDBNull(iRevadcodi)) entity.Revadcodi = Convert.ToInt32(dr.GetValue(iRevadcodi));

            int iFtedatcodi = dr.GetOrdinal(this.Ftedatcodi);
            if (!dr.IsDBNull(iFtedatcodi)) entity.Ftedatcodi = Convert.ToInt32(dr.GetValue(iFtedatcodi));

            int iRevacodi = dr.GetOrdinal(this.Revacodi);
            if (!dr.IsDBNull(iRevacodi)) entity.Revacodi = Convert.ToInt32(dr.GetValue(iRevacodi));

            int iEnvarcodi = dr.GetOrdinal(this.Envarcodi);
            if (!dr.IsDBNull(iEnvarcodi)) entity.Envarcodi = Convert.ToInt32(dr.GetValue(iEnvarcodi));

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Revadcodi = "REVADCODI";
        public string Ftedatcodi = "FTEDATCODI";
        public string Revacodi = "REVACODI";
        public string Envarcodi = "ENVARCODI";
        public string Ftevercodi = "FTEVERCODI";

        #endregion

        public string SqlDeletePorGrupo
        {
            get { return GetSqlXml("DeletePorGrupo"); }
        }

        public string SqlDeletePorIds
        {
            get { return GetSqlXml("DeletePorIds"); }
        }

        public string SqlListarRelacionesPorVersionAreaYEquipo
        {
            get { return GetSqlXml("ListarRelacionesPorVersionAreaYEquipo"); }
        }
        
    }
}
