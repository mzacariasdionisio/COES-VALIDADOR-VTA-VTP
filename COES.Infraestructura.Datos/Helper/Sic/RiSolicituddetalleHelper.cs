using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_SOLICITUDDETALLE
    /// </summary>
    public class RiSolicituddetalleHelper : HelperBase
    {
        public RiSolicituddetalleHelper() : base(Consultas.RiSolicituddetalleSql)
        {
        }

        public RiSolicituddetalleDTO Create(IDataReader dr)
        {
            RiSolicituddetalleDTO entity = new RiSolicituddetalleDTO();

            int iSdetvalor = dr.GetOrdinal(this.Sdetvalor);
            if (!dr.IsDBNull(iSdetvalor)) entity.Sdetvalor = dr.GetString(iSdetvalor);

            int iSdetadjunto = dr.GetOrdinal(this.Sdetadjunto);
            if (!dr.IsDBNull(iSdetadjunto)) entity.Sdetadjunto = dr.GetString(iSdetadjunto);

            int iSdetvaloradjunto = dr.GetOrdinal(this.Sdetvaloradjunto);
            if (!dr.IsDBNull(iSdetvaloradjunto)) entity.Sdetvaloradjunto = dr.GetString(iSdetvaloradjunto);

            int iSolicodi = dr.GetOrdinal(this.Solicodi);
            if (!dr.IsDBNull(iSolicodi)) entity.Solicodi = Convert.ToInt32(dr.GetValue(iSolicodi));

            int iSdetusucreacion = dr.GetOrdinal(this.Sdetusucreacion);
            if (!dr.IsDBNull(iSdetusucreacion)) entity.Sdetusucreacion = dr.GetString(iSdetusucreacion);

            int iSdetfeccreacion = dr.GetOrdinal(this.Sdetfeccreacion);
            if (!dr.IsDBNull(iSdetfeccreacion)) entity.Sdetfeccreacion = dr.GetDateTime(iSdetfeccreacion);

            int iSdetusumodificacion = dr.GetOrdinal(this.Sdetusumodificacion);
            if (!dr.IsDBNull(iSdetusumodificacion)) entity.Sdetusumodificacion = dr.GetString(iSdetusumodificacion);

            int iSdetfecmodificacion = dr.GetOrdinal(this.Sdetfecmodificacion);
            if (!dr.IsDBNull(iSdetfecmodificacion)) entity.Sdetfecmodificacion = dr.GetDateTime(iSdetfecmodificacion);

            int iSdetcodi = dr.GetOrdinal(this.Sdetcodi);
            if (!dr.IsDBNull(iSdetcodi)) entity.Sdetcodi = Convert.ToInt32(dr.GetValue(iSdetcodi));

            int iSdetcampo = dr.GetOrdinal(this.Sdetcampo);
            if (!dr.IsDBNull(iSdetcampo)) entity.Sdetcampo = dr.GetString(iSdetcampo);

            return entity;
        }


        #region Sqls
        public string SqlListBySoliCodi
        {
            get
            {
                return base.GetSqlXml("ListBySoliCodi");
            }
        }
        #endregion

        #region Mapeo de Campos

        public string Sdetvalor = "SDETVALOR";
        public string Sdetadjunto = "SDETADJUNTO";
        public string Sdetvaloradjunto = "SDETVALORADJUNTO";
        public string Solicodi = "SOLICODI";
        public string Sdetusucreacion = "SDETUSUCREACION";
        public string Sdetfeccreacion = "SDETFECCREACION";
        public string Sdetusumodificacion = "SDETUSUMODIFICACION";
        public string Sdetfecmodificacion = "SDETFECMODIFICACION";
        public string Sdetcodi = "SDETCODI";
        public string Sdetcampo = "SDETCAMPO";

        #endregion
    }
}
