using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_SCADA_PTOFILTRO_SP7
    /// </summary>
    public class MeScadaPtofiltroSp7Helper : HelperBase
    {
        public MeScadaPtofiltroSp7Helper(): base(Consultas.MeScadaPtofiltroSp7Sql)
        {
        }

        public MeScadaPtofiltroSp7DTO Create(IDataReader dr)
        {
            MeScadaPtofiltroSp7DTO entity = new MeScadaPtofiltroSp7DTO();

            int iScdpficodi = dr.GetOrdinal(this.Scdpficodi);
            if (!dr.IsDBNull(iScdpficodi)) entity.Scdpficodi = Convert.ToInt32(dr.GetValue(iScdpficodi));

            int iFiltrocodi = dr.GetOrdinal(this.Filtrocodi);
            if (!dr.IsDBNull(iFiltrocodi)) entity.Filtrocodi = Convert.ToInt32(dr.GetValue(iFiltrocodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iScdpfiusucreacion = dr.GetOrdinal(this.Scdpfiusucreacion);
            if (!dr.IsDBNull(iScdpfiusucreacion)) entity.Scdpfiusucreacion = dr.GetString(iScdpfiusucreacion);

            int iScdpfifeccreacion = dr.GetOrdinal(this.Scdpfifeccreacion);
            if (!dr.IsDBNull(iScdpfifeccreacion)) entity.Scdpfifeccreacion = dr.GetDateTime(iScdpfifeccreacion);

            int iScdpfiusumodificacion = dr.GetOrdinal(this.Scdpfiusumodificacion);
            if (!dr.IsDBNull(iScdpfiusumodificacion)) entity.Scdpfiusumodificacion = dr.GetString(iScdpfiusumodificacion);

            int iScdpfifecmodificacion = dr.GetOrdinal(this.Scdpfifecmodificacion);
            if (!dr.IsDBNull(iScdpfifecmodificacion)) entity.Scdpfifecmodificacion = dr.GetDateTime(iScdpfifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Scdpficodi = "SCDPFICODI";
        public string Filtrocodi = "FILTROCODI";
        public string Canalcodi = "CANALCODI";
        public string Scdpfiusucreacion = "SCDPFIUSUCREACION";
        public string Scdpfifeccreacion = "SCDPFIFECCREACION";
        public string Scdpfiusumodificacion = "SCDPFIUSUMODIFICACION";
        public string Scdpfifecmodificacion = "SCDPFIFECMODIFICACION";
        public string Filtronomb = "FILTRONOMB";
        public string Canalnomb = "CANALNOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlDeleteFiltro
        {
            get { return GetSqlXml("DeleteFiltro"); }
        }

        #endregion
    }
}
