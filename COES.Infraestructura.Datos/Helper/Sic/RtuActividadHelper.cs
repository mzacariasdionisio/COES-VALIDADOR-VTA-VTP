using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_ACTIVIDAD
    /// </summary>
    public class RtuActividadHelper : HelperBase
    {
        public RtuActividadHelper() : base(Consultas.RtuActividadSql)
        {
        }

        public RtuActividadDTO Create(IDataReader dr)
        {
            RtuActividadDTO entity = new RtuActividadDTO();

            int iRtuactcodi = dr.GetOrdinal(this.Rtuactcodi);
            if (!dr.IsDBNull(iRtuactcodi)) entity.Rtuactcodi = Convert.ToInt32(dr.GetValue(iRtuactcodi));

            int iRtuactdescripcion = dr.GetOrdinal(this.Rtuactdescripcion);
            if (!dr.IsDBNull(iRtuactdescripcion)) entity.Rtuactdescripcion = dr.GetString(iRtuactdescripcion);

            int iRtuactabreviatura = dr.GetOrdinal(this.Rtuactabreviatura);
            if (!dr.IsDBNull(iRtuactabreviatura)) entity.Rtuactabreviatura = dr.GetString(iRtuactabreviatura);

            int iRtuactestado = dr.GetOrdinal(this.Rtuactestado);
            if (!dr.IsDBNull(iRtuactestado)) entity.Rtuactestado = dr.GetString(iRtuactestado);

            int iRtuactusucreacion = dr.GetOrdinal(this.Rtuactusucreacion);
            if (!dr.IsDBNull(iRtuactusucreacion)) entity.Rtuactusucreacion = dr.GetString(iRtuactusucreacion);

            int iRtuactfeccreacion = dr.GetOrdinal(this.Rtuactfeccreacion);
            if (!dr.IsDBNull(iRtuactfeccreacion)) entity.Rtuactfeccreacion = dr.GetDateTime(iRtuactfeccreacion);

            int iRtuactusumodificacion = dr.GetOrdinal(this.Rtuactusumodificacion);
            if (!dr.IsDBNull(iRtuactusumodificacion)) entity.Rtuactusumodificacion = dr.GetString(iRtuactusumodificacion);

            int iRtuactfecmodificacion = dr.GetOrdinal(this.Rtuactfecmodificacion);
            if (!dr.IsDBNull(iRtuactfecmodificacion)) entity.Rtuactfecmodificacion = dr.GetDateTime(iRtuactfecmodificacion);

            int iRtuactreporte = dr.GetOrdinal(this.Rtuactreporte);
            if (!dr.IsDBNull(iRtuactreporte)) entity.Rtuactreporte = dr.GetString(iRtuactreporte);

            int iRturescodi = dr.GetOrdinal(this.Rturescodi);
            if (!dr.IsDBNull(iRturescodi)) entity.Rturescodi = Convert.ToInt32(dr.GetValue(iRturescodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Rtuactcodi = "RTUACTCODI";
        public string Rtuactdescripcion = "RTUACTDESCRIPCION";
        public string Rtuactabreviatura = "RTUACTABREVIATURA";
        public string Rtuactestado = "RTUACTESTADO";
        public string Rtuactusucreacion = "RTUACTUSUCREACION";
        public string Rtuactfeccreacion = "RTUACTFECCREACION";
        public string Rtuactusumodificacion = "RTUACTUSUMODIFICACION";
        public string Rtuactfecmodificacion = "RTUACTFECMODIFICACION";
        public string Rtuactreporte = "RTUACTREPORTE";
        public string Rturescodi = "RTURESCODI";
        public string Rturesdescripcion = "RTURESDESCRIPCION";
        public string Rturesrol = "RTURESROL";

        #endregion

        public string SqlObtenerTipoResponsable
        {
            get { return base.GetSqlXml("ObtenerTiposReponsables"); }
        }

        public string SqlObtenerActividadPorTipoInforme
        {
            get { return base.GetSqlXml("ObtenerActividadPorTipoInforme"); }
        }
    }
}
