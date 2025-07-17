using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_PARAM_PROCESO
    /// </summary>
    public class SmaParamProcesoHelper : HelperBase
    {
        public SmaParamProcesoHelper(): base(Consultas.SmaParamProcesoSql)
        {
        }

        public SmaParamProcesoDTO Create(IDataReader dr)
        {
            SmaParamProcesoDTO entity = new SmaParamProcesoDTO();

            int iPapocodi = dr.GetOrdinal(this.Papocodi);
            if (!dr.IsDBNull(iPapocodi)) entity.Papocodi = Convert.ToInt32(dr.GetValue(iPapocodi));

            int iPapohorainicio = dr.GetOrdinal(this.Papohorainicio);
            if (!dr.IsDBNull(iPapohorainicio)) entity.Papohorainicio = dr.GetString(iPapohorainicio);

            int iPapohorafin = dr.GetOrdinal(this.Papohorafin);
            if (!dr.IsDBNull(iPapohorafin)) entity.Papohorafin = dr.GetString(iPapohorafin);

            int iPapousucreacion = dr.GetOrdinal(this.Papousucreacion);
            if (!dr.IsDBNull(iPapousucreacion)) entity.Papousucreacion = dr.GetString(iPapousucreacion);

            int iPapofeccreacion = dr.GetOrdinal(this.Papofeccreacion);
            if (!dr.IsDBNull(iPapofeccreacion)) entity.Papofeccreacion = dr.GetDateTime(iPapofeccreacion);

            int iPapofecmodificacion = dr.GetOrdinal(this.Papofecmodificacion);
            if (!dr.IsDBNull(iPapofecmodificacion)) entity.Papofecmodificacion = dr.GetDateTime(iPapofecmodificacion);

            int iPapousumodificacion = dr.GetOrdinal(this.Papousumodificacion);
            if (!dr.IsDBNull(iPapousumodificacion)) entity.Papousumodificacion = dr.GetString(iPapousumodificacion);

            int iPapohoraenvioncp = dr.GetOrdinal(this.Papohoraenvioncp);
            if (!dr.IsDBNull(iPapohoraenvioncp)) entity.Papohoraenvioncp = dr.GetString(iPapohoraenvioncp);

            int iPapoestado = dr.GetOrdinal(this.Papoestado);
            if (!dr.IsDBNull(iPapoestado)) entity.Papoestado = dr.GetString(iPapoestado);
            // STS 19 marzo 2018
            int iPapomaxdiasoferta = dr.GetOrdinal(this.Papomaxdiasoferta);
            if (!dr.IsDBNull(iPapomaxdiasoferta)) entity.Papomaxdiasoferta = dr.GetInt32(iPapomaxdiasoferta);

            return entity;
        }


        #region Mapeo de Campos

        public string Papocodi = "PAPOCODI";
        public string Papohorainicio = "PAPOHORAINICIO";
        public string Papohorafin = "PAPOHORAFIN";
        public string Papousucreacion = "PAPOUSUCREACION";
        public string Papofeccreacion = "PAPOFECCREACION";
        public string Papofecmodificacion = "PAPOFECMODIFICACION";
        public string Papousumodificacion = "PAPOUSUMODIFICACION";
        public string Papohoraenvioncp = "PAPOHORAENVIONCP";
        public string Papoestado = "PAPOESTADO";
        //STS 19 marzo 2018
        public string Papomaxdiasoferta = "PAPOMAXDIASOFERTA";

        #endregion

        public string SqlUpdateInactive
        {
            get { return base.GetSqlXml("UpdateInactive"); }
        }

        public string SqlGetValidRange
        {
            get { return base.GetSqlXml("GetValidRange"); }
        }

        public string SqlGetValidRangeNCP
        {
            get { return base.GetSqlXml("GetValidRangeNCP"); }
        }
    }
}
