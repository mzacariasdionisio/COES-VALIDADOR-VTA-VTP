using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_MAESTRO_MOTIVO
    /// </summary>
    public class SmaMaestroMotivoHelper : HelperBase
    {
        public SmaMaestroMotivoHelper(): base(Consultas.SmaMaestroMotivoSql)
        {
        }

        public SmaMaestroMotivoDTO Create(IDataReader dr)
        {
            SmaMaestroMotivoDTO entity = new SmaMaestroMotivoDTO();

            int iSmammcodi = dr.GetOrdinal(this.Smammcodi);
            if (!dr.IsDBNull(iSmammcodi)) entity.Smammcodi = Convert.ToInt32(dr.GetValue(iSmammcodi));

            int iSmammdescripcion = dr.GetOrdinal(this.Smammdescripcion);
            if (!dr.IsDBNull(iSmammdescripcion)) entity.Smammdescripcion = dr.GetString(iSmammdescripcion);

            int iSmammestado = dr.GetOrdinal(this.Smammestado);
            if (!dr.IsDBNull(iSmammestado)) entity.Smammestado = dr.GetString(iSmammestado);

            int iSmammusucreacion = dr.GetOrdinal(this.Smammusucreacion);
            if (!dr.IsDBNull(iSmammusucreacion)) entity.Smammusucreacion = dr.GetString(iSmammusucreacion);

            int iSmammfeccreacion = dr.GetOrdinal(this.Smammfeccreacion);
            if (!dr.IsDBNull(iSmammfeccreacion)) entity.Smammfeccreacion = dr.GetDateTime(iSmammfeccreacion);

            int iSmammusumodificacion = dr.GetOrdinal(this.Smammusumodificacion);
            if (!dr.IsDBNull(iSmammusumodificacion)) entity.Smammusumodificacion = dr.GetString(iSmammusumodificacion);

            int iSmammfecmodificacion = dr.GetOrdinal(this.Smammfecmodificacion);
            if (!dr.IsDBNull(iSmammfecmodificacion)) entity.Smammfecmodificacion = dr.GetDateTime(iSmammfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Smammcodi = "SMAMMCODI";
        public string Smammdescripcion = "SMAMMDESCRIPCION";
        public string Smammestado = "SMAMMESTADO";
        public string Smammusucreacion = "SMAMMUSUCREACION";
        public string Smammfeccreacion = "SMAMMFECCREACION";
        public string Smammusumodificacion = "SMAMMUSUMODIFICACION";
        public string Smammfecmodificacion = "SMAMMFECMODIFICACION";

        #endregion
    }
}
