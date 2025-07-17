using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_ACTIVACION_MOTIVO
    /// </summary>
    public class SmaActivacionMotivoHelper : HelperBase
    {
        public SmaActivacionMotivoHelper(): base(Consultas.SmaActivacionMotivoSql)
        {
        }

        public SmaActivacionMotivoDTO Create(IDataReader dr)
        {
            SmaActivacionMotivoDTO entity = new SmaActivacionMotivoDTO();

            int iSmaacmcodi = dr.GetOrdinal(this.Smaacmcodi);
            if (!dr.IsDBNull(iSmaacmcodi)) entity.Smaacmcodi = Convert.ToInt32(dr.GetValue(iSmaacmcodi));

            int iSmapaccodi = dr.GetOrdinal(this.Smapaccodi);
            if (!dr.IsDBNull(iSmapaccodi)) entity.Smapaccodi = Convert.ToInt32(dr.GetValue(iSmapaccodi));

            int iSmammcodi = dr.GetOrdinal(this.Smammcodi);
            if (!dr.IsDBNull(iSmammcodi)) entity.Smammcodi = Convert.ToInt32(dr.GetValue(iSmammcodi));

            int iSmaacmtiporeserva = dr.GetOrdinal(this.Smaacmtiporeserva);
            if (!dr.IsDBNull(iSmaacmtiporeserva)) entity.Smaacmtiporeserva = dr.GetString(iSmaacmtiporeserva);

            int iSmaacmusucreacion = dr.GetOrdinal(this.Smaacmusucreacion);
            if (!dr.IsDBNull(iSmaacmusucreacion)) entity.Smaacmusucreacion = dr.GetString(iSmaacmusucreacion);

            int iSmaacmfeccreacion = dr.GetOrdinal(this.Smaacmfeccreacion);
            if (!dr.IsDBNull(iSmaacmfeccreacion)) entity.Smaacmfeccreacion = dr.GetDateTime(iSmaacmfeccreacion);

            int iSmaacmusumodificacion = dr.GetOrdinal(this.Smaacmusumodificacion);
            if (!dr.IsDBNull(iSmaacmusumodificacion)) entity.Smaacmusumodificacion = dr.GetString(iSmaacmusumodificacion);

            int iSmaacmfecmodificacion = dr.GetOrdinal(this.Smaacmfecmodificacion);
            if (!dr.IsDBNull(iSmaacmfecmodificacion)) entity.Smaacmfecmodificacion = dr.GetDateTime(iSmaacmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Smaacmcodi = "SMAACMCODI";
        public string Smapaccodi = "SMAPACCODI";
        public string Smammcodi = "SMAMMCODI";
        public string Smaacmtiporeserva = "SMAACMTIPORESERVA";
        public string Smaacmusucreacion = "SMAACMUSUCREACION";
        public string Smaacmfeccreacion = "SMAACMFECCREACION";
        public string Smaacmusumodificacion = "SMAACMUSUMODIFICACION";
        public string Smaacmfecmodificacion = "SMAACMFECMODIFICACION";

        #endregion

        public string SqlObtenerPorActivacionesOferta
        {
            get { return base.GetSqlXml("ObtenerPorActivacionesOferta"); }
        }
    }
}
