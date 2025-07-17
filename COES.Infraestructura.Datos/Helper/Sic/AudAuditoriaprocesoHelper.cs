using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_AUDITORIAELEMENTO
    /// </summary>
    public class AudAuditoriaprocesoHelper : HelperBase
    {
        public AudAuditoriaprocesoHelper(): base(Consultas.AudAuditoriaprocesoSql)
        {
        }

        public string SqlDeleteAllAudAuditoriaelemento
        {
            get { return GetSqlXml("DeleteAllAudAuditoriaelemento"); }
        }

        public string SqlGetByAuditoriaElementoPorTipo
        {
            get { return GetSqlXml("GetByAuditoriaElementoPorTipo"); }
        }

        public string SqlGetByAudppcodi
        {
            get { return GetSqlXml("GetByAudppcodi"); }
        }

        
        public AudAuditoriaprocesoDTO Create(IDataReader dr)
        {
            AudAuditoriaprocesoDTO entity = new AudAuditoriaprocesoDTO();

            int iAudipsplanificado = dr.GetOrdinal(this.Audipsplanificado);
            if (!dr.IsDBNull(iAudipsplanificado)) entity.Audipsplanificado = dr.GetString(iAudipsplanificado);

            int iAudipactivo = dr.GetOrdinal(this.Audipactivo);
            if (!dr.IsDBNull(iAudipactivo)) entity.Audipactivo = dr.GetString(iAudipactivo);

            int iAudiphistorico = dr.GetOrdinal(this.Audiphistorico);
            if (!dr.IsDBNull(iAudiphistorico)) entity.Audiphistorico = dr.GetString(iAudiphistorico);

            int iAudipusucreacion = dr.GetOrdinal(this.Audipusucreacion);
            if (!dr.IsDBNull(iAudipusucreacion)) entity.Audipusucreacion = dr.GetString(iAudipusucreacion);

            int iAudipfeccreacion = dr.GetOrdinal(this.Audipfeccreacion);
            if (!dr.IsDBNull(iAudipfeccreacion)) entity.Audipfeccreacion = dr.GetDateTime(iAudipfeccreacion);

            int iAudipusumodificacion = dr.GetOrdinal(this.Audipusumodificacion);
            if (!dr.IsDBNull(iAudipusumodificacion)) entity.Audipusumodificacion = dr.GetString(iAudipusumodificacion);

            int iAudipfecmodificacion = dr.GetOrdinal(this.Audipfecmodificacion);
            if (!dr.IsDBNull(iAudipfecmodificacion)) entity.Audipfecmodificacion = dr.GetDateTime(iAudipfecmodificacion);

            int iAudipcodi = dr.GetOrdinal(this.Audipcodi);
            if (!dr.IsDBNull(iAudipcodi)) entity.Audipcodi = Convert.ToInt32(dr.GetValue(iAudipcodi));

            int iAudicodi = dr.GetOrdinal(this.Audicodi);
            if (!dr.IsDBNull(iAudicodi)) entity.Audicodi = Convert.ToInt32(dr.GetValue(iAudicodi));

            int iAudppcodi = dr.GetOrdinal(this.Audppcodi);
            if (!dr.IsDBNull(iAudppcodi)) entity.Audppcodi = Convert.ToInt32(dr.GetValue(iAudppcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Elemcodi = "ELEMCODI";
        public string Audipsplanificado = "AUDIPSPLANIFICADO";
        public string Audipactivo = "AUDIPACTIVO";
        public string Audiphistorico = "AUDIPHISTORICO";
        public string Audipusucreacion = "AUDIPUSUCREACION";
        public string Audipfeccreacion = "AUDIPFECCREACION";
        public string Audipusumodificacion = "AUDIPUSUMODIFICACION";
        public string Audipfecmodificacion = "AUDIPFECMODIFICACION";
        public string Audipcodi = "AUDIPCODI";
        public string Audicodi = "AUDICODI";
        public string Audppcodi = "AUDPPCODI";
        public string Proccodi = "PROCCODI";
        public string Elemdescripcion = "Elemdescripcion";

        public string Procdescripcion = "PROCDESCRIPCION";
        public string Areacodi = "AREACODI";

        #endregion
    }
}
