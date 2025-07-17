using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_SCADA_FILTRO_SP7
    /// </summary>
    public class MeScadaFiltroSp7Helper : HelperBase
    {
        public MeScadaFiltroSp7Helper(): base(Consultas.MeScadaFiltroSp7Sql)
        {
        }

        public MeScadaFiltroSp7DTO Create(IDataReader dr)
        {
            MeScadaFiltroSp7DTO entity = new MeScadaFiltroSp7DTO();

            int iFiltrocodi = dr.GetOrdinal(this.Filtrocodi);
            if (!dr.IsDBNull(iFiltrocodi)) entity.Filtrocodi = Convert.ToInt32(dr.GetValue(iFiltrocodi));

            int iFiltronomb = dr.GetOrdinal(this.Filtronomb);
            if (!dr.IsDBNull(iFiltronomb)) entity.Filtronomb = dr.GetString(iFiltronomb);

            int iFiltrouser = dr.GetOrdinal(this.Filtrouser);
            if (!dr.IsDBNull(iFiltrouser)) entity.Filtrouser = dr.GetString(iFiltrouser);

            int iScdfifeccreacion = dr.GetOrdinal(this.Scdfifeccreacion);
            if (!dr.IsDBNull(iScdfifeccreacion)) entity.Scdfifeccreacion = dr.GetDateTime(iScdfifeccreacion);

            int iScdfiusumodificacion = dr.GetOrdinal(this.Scdfiusumodificacion);
            if (!dr.IsDBNull(iScdfiusumodificacion)) entity.Scdfiusumodificacion = dr.GetString(iScdfiusumodificacion);

            int iScdfifecmodificacion = dr.GetOrdinal(this.Scdfifecmodificacion);
            if (!dr.IsDBNull(iScdfifecmodificacion)) entity.Scdfifecmodificacion = dr.GetDateTime(iScdfifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Filtrocodi = "FILTROCODI";
        public string Filtronomb = "FILTRONOMB";
        public string Filtrouser = "FILTROUSER";
        public string Scdfifeccreacion = "SCDFIFECCREACION";
        public string Scdfiusumodificacion = "SCDFIUSUMODIFICACION";
        public string Scdfifecmodificacion = "SCDFIFECMODIFICACION";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
