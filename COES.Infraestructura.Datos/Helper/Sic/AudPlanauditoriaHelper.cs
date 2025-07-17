using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PLANAUDITORIA
    /// </summary>
    public class AudPlanauditoriaHelper : HelperBase
    {
        public AudPlanauditoriaHelper(): base(Consultas.AudPlanauditoriaSql)
        {
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlGetByPlanValidacion
        {
            get { return base.GetSqlXml("GetByPlanValidacion"); }
        }


        public AudPlanauditoriaDTO Create(IDataReader dr)
        {
            AudPlanauditoriaDTO entity = new AudPlanauditoriaDTO();

            int iPlancodi = dr.GetOrdinal(this.Plancodi);
            if (!dr.IsDBNull(iPlancodi)) entity.Plancodi = Convert.ToInt32(dr.GetValue(iPlancodi));

            int iPlancodigo = dr.GetOrdinal(this.Plancodigo);
            if (!dr.IsDBNull(iPlancodigo)) entity.Plancodigo = dr.GetString(iPlancodigo);

            int iPlanano = dr.GetOrdinal(this.Planano);
            if (!dr.IsDBNull(iPlanano)) entity.Planano = dr.GetString(iPlanano);

            int iPlananovigencia = dr.GetOrdinal(this.Plananovigencia);
            if (!dr.IsDBNull(iPlananovigencia)) entity.Plananovigencia = dr.GetString(iPlananovigencia);

            int iPlanactivo = dr.GetOrdinal(this.Planactivo);
            if (!dr.IsDBNull(iPlanactivo)) entity.Planactivo = dr.GetString(iPlanactivo);

            int iPlanhistorico = dr.GetOrdinal(this.Planhistorico);
            if (!dr.IsDBNull(iPlanhistorico)) entity.Planhistorico = dr.GetString(iPlanhistorico);

            int iPlanusucreacion = dr.GetOrdinal(this.Planusucreacion);
            if (!dr.IsDBNull(iPlanusucreacion)) entity.Planusucreacion = dr.GetString(iPlanusucreacion);

            int iPlanfeccreacion = dr.GetOrdinal(this.Planfeccreacion);
            if (!dr.IsDBNull(iPlanfeccreacion)) entity.Planfeccreacion = dr.GetDateTime(iPlanfeccreacion);

            int iPlanusumodificacion = dr.GetOrdinal(this.Planusumodificacion);
            if (!dr.IsDBNull(iPlanusumodificacion)) entity.Planusumodificacion = dr.GetString(iPlanusumodificacion);

            int iPlanfecmodificacion = dr.GetOrdinal(this.Planfecmodificacion);
            if (!dr.IsDBNull(iPlanfecmodificacion)) entity.Planfecmodificacion = dr.GetDateTime(iPlanfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Plancodi = "PLANCODI";
        public string Plancodigo = "PLANCODIGO";
        public string Planano = "PLANANO";
        public string Plananovigencia = "PLANANOVIGENCIA";
        public string Planactivo = "PLANACTIVO";
        public string Planhistorico = "PLANHISTORICO";
        public string Planusucreacion = "PLANUSUCREACION";
        public string Planfeccreacion = "PLANFECCREACION";
        public string Planusumodificacion = "PLANUSUMODIFICACION";
        public string Planfecmodificacion = "PLANFECMODIFICACION";

        public string Validacionmensaje = "Validacionmensaje";
        
        #endregion
    }
}
