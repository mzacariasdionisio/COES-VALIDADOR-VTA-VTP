using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_LOGPROCESO
    /// </summary>
    public class VtdLogProcesoHelper : HelperBase
    {
        public VtdLogProcesoHelper(): base(Consultas.VtdLogProcesoSql)
        {
        }

        public VtdLogProcesoDTO Create(IDataReader dr)
        {
            VtdLogProcesoDTO entity = new VtdLogProcesoDTO();

            int iLogpcodi = dr.GetOrdinal(this.Logpcodi);
            if (!dr.IsDBNull(iLogpcodi)) entity.Logpcodi = Convert.ToDecimal(dr.GetValue(iLogpcodi));

            int iValocodi = dr.GetOrdinal(this.Valocodi);
            if (!dr.IsDBNull(iValocodi)) entity.Valocodi = Convert.ToDecimal(dr.GetValue(iValocodi));

            int iLogpfecha = dr.GetOrdinal(this.Logpfecha);
            if (!dr.IsDBNull(iLogpfecha)) entity.Logpfecha = dr.GetDateTime(iLogpfecha);

            int iLogphorainicio = dr.GetOrdinal(this.Logphorainicio);
            if (!dr.IsDBNull(iLogphorainicio)) entity.Logphorainicio = dr.GetDateTime(iLogphorainicio);

            int iLogphorafin = dr.GetOrdinal(this.Logphorafin);
            if (!dr.IsDBNull(iLogphorafin)) entity.Logphorafin = dr.GetDateTime(iLogphorafin);

            int iLogplog = dr.GetOrdinal(this.Logplog);
            if (!dr.IsDBNull(iLogplog)) entity.Logplog = dr.GetString(iLogplog);

            int iLogptipo = dr.GetOrdinal(this.Logptipo);
            if (!dr.IsDBNull(iLogptipo)) entity.Logptipo = Convert.ToChar(dr.GetString(iLogptipo));

            int iLogpestado = dr.GetOrdinal(this.Logpestado);
            if (!dr.IsDBNull(iLogpestado)) entity.Logpestado = Convert.ToChar(dr.GetString(iLogpestado));

            int iLogpsucreacion = dr.GetOrdinal(this.Logpsucreacion);
            if (!dr.IsDBNull(iLogpsucreacion)) entity.Logpusucreacion = dr.GetString(iLogpsucreacion);

            int iLogpfeccreacion = dr.GetOrdinal(this.Logpfeccreacion);
            if (!dr.IsDBNull(iLogpfeccreacion)) entity.Logpfeccreacion = dr.GetDateTime(iLogpfeccreacion);

            int iLogpusumodificacion = dr.GetOrdinal(this.Logpusumodificacion);
            if (!dr.IsDBNull(iLogpusumodificacion)) entity.Logpusumodificacion = dr.GetString(iLogpusumodificacion);

            int iLogpfecmodificacion = dr.GetOrdinal(this.Logpfecmodificacion);
            if (!dr.IsDBNull(iLogpfecmodificacion)) entity.Logpfecmodificacion = dr.GetDateTime(iLogpfecmodificacion);

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Logpcodi = "LOGPCODI";
        public string Valocodi = "VALOCODI";
        public string Logpfecha = "LOGPFECHA";
        public string Logphorainicio = "LOGPHORAINICIO";
        public string Logphorafin = "LOGPHORAFIN";
        public string Logplog = "LOGPLOG";
        public string Logptipo = "LOGPTIPO";
        public string Logpestado = "LOGPESTADO";
        public string Logpsucreacion = "LOGPUSUCREACION";
        public string Logpfeccreacion = "LOGPFECCREACION";
        public string Logpusumodificacion = "LOGPUSUMODIFICACION";
        public string Logpfecmodificacion = "LOGPFECMODIFICACION";
        public string Valofecha = "VALOFECHA";


        public string GetListFullByDate
        {
            get { return base.GetSqlXml("GetListByDate"); }
        }
        public string GetListPagedByDate
        {
            get { return base.GetSqlXml("GetListPagedByDate"); }
        }
        

        #endregion
    }
}
