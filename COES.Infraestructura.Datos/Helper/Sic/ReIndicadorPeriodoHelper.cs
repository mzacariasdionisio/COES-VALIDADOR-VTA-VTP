using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INDICADOR_PERIODO
    /// </summary>
    public class ReIndicadorPeriodoHelper : HelperBase
    {
        public ReIndicadorPeriodoHelper(): base(Consultas.ReIndicadorPeriodoSql)
        {
        }

        public ReIndicadorPeriodoDTO Create(IDataReader dr)
        {
            ReIndicadorPeriodoDTO entity = new ReIndicadorPeriodoDTO();

            int iReindcodi = dr.GetOrdinal(this.Reindcodi);
            if (!dr.IsDBNull(iReindcodi)) entity.Reindcodi = Convert.ToInt32(dr.GetValue(iReindcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iRecintcodi = dr.GetOrdinal(this.Recintcodi);
            if (!dr.IsDBNull(iRecintcodi)) entity.Recintcodi = Convert.ToInt32(dr.GetValue(iRecintcodi));

            int iReindki = dr.GetOrdinal(this.Reindki);
            if (!dr.IsDBNull(iReindki)) entity.Reindki = dr.GetDecimal(iReindki);

            int iReindni = dr.GetOrdinal(this.Reindni);
            if (!dr.IsDBNull(iReindni)) entity.Reindni = dr.GetDecimal(iReindni);

            int iReindusucreacion = dr.GetOrdinal(this.Reindusucreacion);
            if (!dr.IsDBNull(iReindusucreacion)) entity.Reindusucreacion = dr.GetString(iReindusucreacion);

            int iReindfeccreacion = dr.GetOrdinal(this.Reindfeccreacion);
            if (!dr.IsDBNull(iReindfeccreacion)) entity.Reindfeccreacion = dr.GetDateTime(iReindfeccreacion);

            int iReindusumodificacion = dr.GetOrdinal(this.Reindusumodificacion);
            if (!dr.IsDBNull(iReindusumodificacion)) entity.Reindusumodificacion = dr.GetString(iReindusumodificacion);

            int iReindfecmodificacion = dr.GetOrdinal(this.Reindfecmodificacion);
            if (!dr.IsDBNull(iReindfecmodificacion)) entity.Reindfecmodificacion = dr.GetDateTime(iReindfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reindcodi = "REINDCODI";
        public string Repercodi = "REPERCODI";
        public string Recintcodi = "RECINTCODI";
        public string Reindki = "REINDKI";
        public string Reindni = "REINDNI";
        public string Reindusucreacion = "REINDUSUCREACION";
        public string Reindfeccreacion = "REINDFECCREACION";
        public string Reindusumodificacion = "REINDUSUMODIFICACION";
        public string Reindfecmodificacion = "REINDFECMODIFICACION";

        #endregion

        public string SqlObtenerParaImportar
        {
            get { return base.GetSqlXml("ObtenerParaImportar"); }
        }
    }
}
