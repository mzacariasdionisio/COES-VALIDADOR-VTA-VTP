using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_AUDITORIA
    /// </summary>
    public class AudAuditoriaHelper : HelperBase
    {
        public AudAuditoriaHelper(): base(Consultas.AudAuditoriaSql)
        {
        }
        
        public AudAuditoriaDTO Create(IDataReader dr)
        {
            AudAuditoriaDTO entity = new AudAuditoriaDTO();

            int iAudicodi = dr.GetOrdinal(this.Audicodi);
            if (!dr.IsDBNull(iAudicodi)) entity.Audicodi = Convert.ToInt32(dr.GetValue(iAudicodi));

            int iTabcdestadocodi = dr.GetOrdinal(this.Tabcdestadocodi);
            if (!dr.IsDBNull(iTabcdestadocodi)) entity.Tabcdestadocodi = Convert.ToInt32(dr.GetValue(iTabcdestadocodi));

            int iAudicodigo = dr.GetOrdinal(this.Audicodigo);
            if (!dr.IsDBNull(iAudicodigo)) entity.Audicodigo = dr.GetString(iAudicodigo);

            int iAudinombre = dr.GetOrdinal(this.Audinombre);
            if (!dr.IsDBNull(iAudinombre)) entity.Audinombre = dr.GetString(iAudinombre);

            int iAudiobjetivo = dr.GetOrdinal(this.Audiobjetivo);
            if (!dr.IsDBNull(iAudiobjetivo)) entity.Audiobjetivo = dr.GetString(iAudiobjetivo);

            
            
            int iAudifechainicio = dr.GetOrdinal(this.Audifechainicio);
            if (!dr.IsDBNull(iAudifechainicio)) entity.Audifechainicio = dr.GetDateTime(iAudifechainicio);

            int iAudifechafin = dr.GetOrdinal(this.Audifechafin);
            if (!dr.IsDBNull(iAudifechafin)) entity.Audifechafin = dr.GetDateTime(iAudifechafin);

            int iAudiactivo = dr.GetOrdinal(this.Audiactivo);
            if (!dr.IsDBNull(iAudiactivo)) entity.Audiactivo = dr.GetString(iAudiactivo);

            int iAudihistorico = dr.GetOrdinal(this.Audihistorico);
            if (!dr.IsDBNull(iAudihistorico)) entity.Audihistorico = dr.GetString(iAudihistorico);

            int iAudiusucreacion = dr.GetOrdinal(this.Audiusucreacion);
            if (!dr.IsDBNull(iAudiusucreacion)) entity.Audiusucreacion = dr.GetString(iAudiusucreacion);

            int iAudifeccreacion = dr.GetOrdinal(this.Audifeccreacion);
            if (!dr.IsDBNull(iAudifeccreacion)) entity.Audifeccreacion = dr.GetDateTime(iAudifeccreacion);

            int iAudiusumodificacion = dr.GetOrdinal(this.Audiusumodificacion);
            if (!dr.IsDBNull(iAudiusumodificacion)) entity.Audiusumodificacion = dr.GetString(iAudiusumodificacion);

            int iAudifecmodificacion = dr.GetOrdinal(this.Audifecmodificacion);
            if (!dr.IsDBNull(iAudifecmodificacion)) entity.Audifecmodificacion = dr.GetDateTime(iAudifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Audicodi = "AUDICODI";
        public string Tabcdestadocodi = "TABCDESTADOCODI";
        public string Audicodigo = "AUDICODIGO";
        public string Audinombre = "AUDINOMBRE";
        public string Audiobjetivo = "AUDIOBJETIVO";
        public string Audialcance = "AUDIALCANCE";
        public string Audifechainicio = "AUDIFECHAINICIO";
        public string Audifechafin = "AUDIFECHAFIN";
        public string Audiactivo = "AUDIACTIVO";
        public string Audihistorico = "AUDIHISTORICO";
        public string Audiusucreacion = "AUDIUSUCREACION";
        public string Audifeccreacion = "AUDIFECCREACION";
        public string Audiusumodificacion = "AUDIUSUMODIFICACION";
        public string Audifecmodificacion = "AUDIFECMODIFICACION";
        public string Audpcodi = "AUDPCODI";
        public string Plancodi = "PLANCODI";

        public string Audipsplanificado = "AUDIPSPLANIFICADO";
        public string Audppcodi = "AUDPPCODI";
        public string Proccodi = "PROCCODI";
        public string Areacodi = "AREACODI";

        public string Procdescripcion = "Procdescripcion";
        public string Tabcddescripcion = "Tabcddescripcion";
        public string Tabcdcodi = "Tabcdcodi";
        public string Progahdescripcion = "progahdescripcion";
        public string Elemdescripcion = "elemdescripcion";
        public string Elemcodigo = "elemcodigo";

        public string Progahaccionmejora = "PROGAHACCIONMEJORA";

        public string Areaabrev = "areaabrev";

        public string Estadodescripcion = "estadoDescripcion";
        #endregion


        public string SqlMostrarAnios
        {
            get { return base.GetSqlXml("MostrarAnios"); }
        }

        public string SqlMostrarAuditoriasEjectuar
        {
            get { return base.GetSqlXml("MostrarAuditoriasEjecutar"); }
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlVerResultados
        {
            get { return base.GetSqlXml("VerResultados"); }
        }

        public string SqlObtenerNroRegistroBusquedaResultados
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusquedaResultados"); }
        }
    }
}
