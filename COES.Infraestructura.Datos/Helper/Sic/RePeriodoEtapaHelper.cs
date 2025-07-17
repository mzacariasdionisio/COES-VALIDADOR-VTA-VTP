using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_PERIODO_ETAPA
    /// </summary>
    public class RePeriodoEtapaHelper : HelperBase
    {
        public RePeriodoEtapaHelper(): base(Consultas.RePeriodoEtapaSql)
        {
        }

        public RePeriodoEtapaDTO Create(IDataReader dr)
        {
            RePeriodoEtapaDTO entity = new RePeriodoEtapaDTO();

            int iRepeetcodi = dr.GetOrdinal(this.Repeetcodi);
            if (!dr.IsDBNull(iRepeetcodi)) entity.Repeetcodi = Convert.ToInt32(dr.GetValue(iRepeetcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReetacodi = dr.GetOrdinal(this.Reetacodi);
            if (!dr.IsDBNull(iReetacodi)) entity.Reetacodi = Convert.ToInt32(dr.GetValue(iReetacodi));

            int iRepeetfecha = dr.GetOrdinal(this.Repeetfecha);
            if (!dr.IsDBNull(iRepeetfecha)) entity.Repeetfecha = dr.GetDateTime(iRepeetfecha);

            int iRepeetestado = dr.GetOrdinal(this.Repeetestado);
            if (!dr.IsDBNull(iRepeetestado)) entity.Repeetestado = dr.GetString(iRepeetestado);

            int iRepeetusucreacion = dr.GetOrdinal(this.Repeetusucreacion);
            if (!dr.IsDBNull(iRepeetusucreacion)) entity.Repeetusucreacion = dr.GetString(iRepeetusucreacion);

            int iRepeetfeccreacion = dr.GetOrdinal(this.Repeetfeccreacion);
            if (!dr.IsDBNull(iRepeetfeccreacion)) entity.Repeetfeccreacion = dr.GetDateTime(iRepeetfeccreacion);

            int iRepeetusumodificacion = dr.GetOrdinal(this.Repeetusumodificacion);
            if (!dr.IsDBNull(iRepeetusumodificacion)) entity.Repeetusumodificacion = dr.GetString(iRepeetusumodificacion);

            int iRepeetfecmodificacion = dr.GetOrdinal(this.Repeetfecmodificacion);
            if (!dr.IsDBNull(iRepeetfecmodificacion)) entity.Repeetfecmodificacion = dr.GetDateTime(iRepeetfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Repeetcodi = "REPEETCODI";
        public string Repercodi = "REPERCODI";
        public string Reetacodi = "REETACODI";
        public string Repeetfecha = "REPEETFECHA";
        public string Repeetestado = "REPEETESTADO";
        public string Repeetusucreacion = "REPEETUSUCREACION";
        public string Repeetfeccreacion = "REPEETFECCREACION";
        public string Repeetusumodificacion = "REPEETUSUMODIFICACION";
        public string Repeetfecmodificacion = "REPEETFECMODIFICACION";

        #endregion

        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }
    }
}
