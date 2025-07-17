using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_PROCESO
    /// </summary>
    public class WbProcesoHelper : HelperBase
    {
        public WbProcesoHelper() : base(Consultas.WbProcesoSql)
        {
        }

        public WbProcesoDTO Create(IDataReader dr)
        {
            WbProcesoDTO entity = new WbProcesoDTO();

            int iProcesocodi = dr.GetOrdinal(this.Procesocodi);
            if (!dr.IsDBNull(iProcesocodi)) entity.Procesocodi = Convert.ToInt32(dr.GetValue(iProcesocodi));

            int iProcesoname = dr.GetOrdinal(this.Procesoname);
            if (!dr.IsDBNull(iProcesoname)) entity.Procesoname = dr.GetString(iProcesoname);

            int iProcesoestado = dr.GetOrdinal(this.Procesoestado);
            if (!dr.IsDBNull(iProcesoestado)) entity.Procesoestado = dr.GetString(iProcesoestado);

            int iProcesousucreacion = dr.GetOrdinal(this.Procesousucreacion);
            if (!dr.IsDBNull(iProcesousucreacion)) entity.Procesousucreacion = dr.GetString(iProcesousucreacion);

            int iProcesousumodificacion = dr.GetOrdinal(this.Procesousumodificacion);
            if (!dr.IsDBNull(iProcesousumodificacion)) entity.Procesousumodificacion = dr.GetString(iProcesousumodificacion);

            int iProcesofeccreacion = dr.GetOrdinal(this.Procesofeccreacion);
            if (!dr.IsDBNull(iProcesofeccreacion)) entity.Procesofeccreacion = dr.GetDateTime(iProcesofeccreacion);

            int iProcesofecmodificacion = dr.GetOrdinal(this.Procesofecmodificacion);
            if (!dr.IsDBNull(iProcesofecmodificacion)) entity.Procesofecmodificacion = dr.GetDateTime(iProcesofecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Procesocodi = "PROCESOCODI";
        public string Procesoname = "PROCESONAME";
        public string Procesoestado = "PROCESOESTADO";
        public string Procesousucreacion = "PROCESOUSUCREACION";
        public string Procesousumodificacion = "PROCESOUSUMODIFICACION";
        public string Procesofeccreacion = "PROCESOFECCREACION";
        public string Procesofecmodificacion = "PROCESOFECMODIFICACION";

        #endregion
    }
}
