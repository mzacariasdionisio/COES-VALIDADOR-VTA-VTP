using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_ESTACIONH
    /// </summary>
    public class PmoEstacionhHelper : HelperBase
    {
        public PmoEstacionhHelper() : base(Consultas.PmoEstacionhSql)
        {
        }

        public PmoEstacionhDTO Create(IDataReader dr)
        {
            PmoEstacionhDTO entity = new PmoEstacionhDTO();

            int iPmehcodi = dr.GetOrdinal(this.Pmehcodi);
            if (!dr.IsDBNull(iPmehcodi)) entity.Pmehcodi = Convert.ToInt32(dr.GetValue(iPmehcodi));

            int iPmehdesc = dr.GetOrdinal(this.Pmehdesc);
            if (!dr.IsDBNull(iPmehdesc)) entity.Pmehdesc = dr.GetString(iPmehdesc);

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iPmehreferencia = dr.GetOrdinal(this.Pmehreferencia);
            if (!dr.IsDBNull(iPmehreferencia)) entity.Pmehreferencia = dr.GetString(iPmehreferencia);

            int iPmehorden = dr.GetOrdinal(this.Pmehorden);
            if (!dr.IsDBNull(iPmehorden)) entity.Pmehorden = Convert.ToInt32(dr.GetValue(iPmehorden));

            int iPmehusucreacion = dr.GetOrdinal(this.Pmehusucreacion);
            if (!dr.IsDBNull(iPmehusucreacion)) entity.Pmehusucreacion = dr.GetString(iPmehusucreacion);

            int iPmehfeccreacion = dr.GetOrdinal(this.Pmehfeccreacion);
            if (!dr.IsDBNull(iPmehfeccreacion)) entity.Pmehfeccreacion = dr.GetDateTime(iPmehfeccreacion);

            int iPmehusumodificacion = dr.GetOrdinal(this.Pmehusumodificacion);
            if (!dr.IsDBNull(iPmehusumodificacion)) entity.Pmehusumodificacion = dr.GetString(iPmehusumodificacion);

            int iPmehfecmodificacion = dr.GetOrdinal(this.Pmehfecmodificacion);
            if (!dr.IsDBNull(iPmehfecmodificacion)) entity.Pmehfecmodificacion = dr.GetDateTime(iPmehfecmodificacion);

            int iPmehestado = dr.GetOrdinal(this.Pmehestado);
            if (!dr.IsDBNull(iPmehestado)) entity.Pmehestado = dr.GetString(iPmehestado);

            int iPmehnumversion = dr.GetOrdinal(this.Pmehnumversion);
            if (!dr.IsDBNull(iPmehnumversion)) entity.Pmehnumversion = Convert.ToInt32(dr.GetValue(iPmehnumversion));

            int iPmehintegrante = dr.GetOrdinal(this.Pmehintegrante);
            if (!dr.IsDBNull(iPmehintegrante)) entity.Pmehintegrante = dr.GetString(iPmehintegrante);

            return entity;
        }


        #region Mapeo de Campos

        public string Pmehcodi = "PMEHCODI";
        public string Pmehdesc = "PMEHDESC";
        public string Sddpcodi = "SDDPCODI";
        public string Pmehreferencia = "PMEHREFERENCIA";
        public string Pmehorden = "PMEHORDEN";
        public string Pmehusucreacion = "PMEHUSUCREACION";
        public string Pmehfeccreacion = "PMEHFECCREACION";
        public string Pmehusumodificacion = "PMEHUSUMODIFICACION";
        public string Pmehfecmodificacion = "PMEHFECMODIFICACION";
        public string Pmehestado = "PMEHESTADO";
        public string Pmehnumversion = "PMEHNUMVERSION";
        public string Pmehintegrante = "PMEHINTEGRANTE";

        #endregion

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }
        public string SqlUpdateOrden
        {
            get { return GetSqlXml("UpdateOrden"); }
        }
    }
}
