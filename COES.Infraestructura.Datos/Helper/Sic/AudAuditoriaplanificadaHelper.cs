using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_AUDITORIAPLANIFICADA
    /// </summary>
    public class AudAuditoriaplanificadaHelper : HelperBase
    {
        public AudAuditoriaplanificadaHelper(): base(Consultas.AudAuditoriaplanificadaSql)
        {
        }

        public string SqlDeleteByAudPlan
        {
            get { return GetSqlXml("DeleteByAudPlan"); }
        }

        public string GetByCriteria
        {
            get { return GetSqlXml("GetByCriteria"); }
        }

        public string SqlGetByAudPlanificadaValidacion
        {
            get { return GetSqlXml("GetByAudPlanificadaValidacion"); }
        }
        

        public AudAuditoriaplanificadaDTO Create(IDataReader dr)
        {
            AudAuditoriaplanificadaDTO entity = new AudAuditoriaplanificadaDTO();

            int iAudpcodi = dr.GetOrdinal(this.Audpcodi);
            if (!dr.IsDBNull(iAudpcodi)) entity.Audpcodi = Convert.ToInt32(dr.GetValue(iAudpcodi));

            int iPlancodi = dr.GetOrdinal(this.Plancodi);
            if (!dr.IsDBNull(iPlancodi)) entity.Plancodi = Convert.ToInt32(dr.GetValue(iPlancodi));

            int iAudpnombre = dr.GetOrdinal(this.Audpnombre);
            if (!dr.IsDBNull(iAudpnombre)) entity.Audpnombre = dr.GetString(iAudpnombre);

            int iAudpcodigo = dr.GetOrdinal(this.Audpcodigo);
            if (!dr.IsDBNull(iAudpcodigo)) entity.Audpcodigo = dr.GetString(iAudpcodigo);

            int iAudpmesinicio = dr.GetOrdinal(this.Audpmesinicio);
            if (!dr.IsDBNull(iAudpmesinicio)) entity.Audpmesinicio = dr.GetString(iAudpmesinicio);

            int iAudpmesfin = dr.GetOrdinal(this.Audpmesfin);
            if (!dr.IsDBNull(iAudpmesfin)) entity.Audpmesfin = dr.GetString(iAudpmesfin);

            int iAudpdactivo = dr.GetOrdinal(this.Audpdactivo);
            if (!dr.IsDBNull(iAudpdactivo)) entity.Audpdactivo = dr.GetString(iAudpdactivo);

            int iAudphistorico = dr.GetOrdinal(this.Audphistorico);
            if (!dr.IsDBNull(iAudphistorico)) entity.Audphistorico = dr.GetString(iAudphistorico);

            int iAudpusucreacion = dr.GetOrdinal(this.Audpusucreacion);
            if (!dr.IsDBNull(iAudpusucreacion)) entity.Audpusucreacion = dr.GetString(iAudpusucreacion);

            int iAudpfeccreacion = dr.GetOrdinal(this.Audpfeccreacion);
            if (!dr.IsDBNull(iAudpfeccreacion)) entity.Audpfeccreacion = dr.GetDateTime(iAudpfeccreacion);

            int iAudpusumodificacion = dr.GetOrdinal(this.Audpusumodificacion);
            if (!dr.IsDBNull(iAudpusumodificacion)) entity.Audpusumodificacion = dr.GetString(iAudpusumodificacion);

            int iAudpfecmodificacion = dr.GetOrdinal(this.Audpfecmodificacion);
            if (!dr.IsDBNull(iAudpfecmodificacion)) entity.Audpfecmodificacion = dr.GetDateTime(iAudpfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Audpcodi = "AUDPCODI";
        public string Plancodi = "PLANCODI";
        public string Audpnombre = "AUDPNOMBRE";
        public string Audpcodigo = "AUDPCODIGO";
        public string Audpmesinicio = "AUDPMESINICIO";
        public string Audpmesfin = "AUDPMESFIN";
        public string Audpdactivo = "AUDPDACTIVO";
        public string Audphistorico = "AUDPHISTORICO";
        public string Audpusucreacion = "AUDPUSUCREACION";
        public string Audpfeccreacion = "AUDPFECCREACION";
        public string Audpusumodificacion = "AUDPUSUMODIFICACION";
        public string Audpfecmodificacion = "AUDPFECMODIFICACION";

        public string Procesos = "procesos";
        public string Procesoareas = "procesoareas";

        public string Areacodi = "areacodi";
        public string Existeaudiproceso = "existeaudiproceso";

        public string Validacionmensaje = "Validacionmensaje";
        
        #endregion
    }
}
