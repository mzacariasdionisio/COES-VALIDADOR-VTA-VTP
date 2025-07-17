using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_RIESGO
    /// </summary>
    public class AudRiesgoHelper : HelperBase
    {
        public AudRiesgoHelper(): base(Consultas.AudRiesgoSql)
        {
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }
        
        public AudRiesgoDTO Create(IDataReader dr)
        {
            AudRiesgoDTO entity = new AudRiesgoDTO();

            int iRiesactivo = dr.GetOrdinal(this.Riesactivo);
            if (!dr.IsDBNull(iRiesactivo)) entity.Riesactivo = dr.GetString(iRiesactivo);

            int iRieshistorico = dr.GetOrdinal(this.Rieshistorico);
            if (!dr.IsDBNull(iRieshistorico)) entity.Rieshistorico = dr.GetString(iRieshistorico);

            int iRiesusucreacion = dr.GetOrdinal(this.Riesusucreacion);
            if (!dr.IsDBNull(iRiesusucreacion)) entity.Riesusucreacion = dr.GetString(iRiesusucreacion);

            int iRiesfeccreacion = dr.GetOrdinal(this.Riesfeccreacion);
            if (!dr.IsDBNull(iRiesfeccreacion)) entity.Riesfeccreacion = dr.GetDateTime(iRiesfeccreacion);

            int iRiesusumodificacion = dr.GetOrdinal(this.Riesusumodificacion);
            if (!dr.IsDBNull(iRiesusumodificacion)) entity.Riesusumodificacion = dr.GetString(iRiesusumodificacion);

            int iRiesfecmodificacion = dr.GetOrdinal(this.Riesfecmodificacion);
            if (!dr.IsDBNull(iRiesfecmodificacion)) entity.Riesfecmodificacion = dr.GetDateTime(iRiesfecmodificacion);

            int iRiescodi = dr.GetOrdinal(this.Riescodi);
            if (!dr.IsDBNull(iRiescodi)) entity.Riescodi = Convert.ToInt32(dr.GetValue(iRiescodi));

            int iProccodi = dr.GetOrdinal(this.Proccodi);
            if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

            int iTabcdcodivaloracioninherente = dr.GetOrdinal(this.Tabcdcodivaloracioninherente);
            if (!dr.IsDBNull(iTabcdcodivaloracioninherente)) entity.Tabcdcodivaloracioninherente = Convert.ToInt32(dr.GetValue(iTabcdcodivaloracioninherente));

            int iTabcdcodivaloracionresidual = dr.GetOrdinal(this.Tabcdcodivaloracionresidual);
            if (!dr.IsDBNull(iTabcdcodivaloracionresidual)) entity.Tabcdcodivaloracionresidual = Convert.ToInt32(dr.GetValue(iTabcdcodivaloracionresidual));

            int iRiescodigo = dr.GetOrdinal(this.Riescodigo);
            if (!dr.IsDBNull(iRiescodigo)) entity.Riescodigo = dr.GetString(iRiescodigo);

            int iRiesdescripcion = dr.GetOrdinal(this.Riesdescripcion);
            if (!dr.IsDBNull(iRiesdescripcion)) entity.Riesdescripcion = dr.GetString(iRiesdescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Riesactivo = "RIESACTIVO";
        public string Rieshistorico = "RIESHISTORICO";
        public string Riesusucreacion = "RIESUSUCREACION";
        public string Riesfeccreacion = "RIESFECCREACION";
        public string Riesusumodificacion = "RIESUSUMODIFICACION";
        public string Riesfecmodificacion = "RIESFECMODIFICACION";
        public string Riescodi = "RIESCODI";
        public string Proccodi = "PROCCODI";
        public string Tabcdcodivaloracioninherente = "TABCDCODIVALORACIONINHERENTE";
        public string Tabcdcodivaloracionresidual = "TABCDCODIVALORACIONRESIDUAL";
        public string Riescodigo = "RIESCODIGO";
        public string Riesdescripcion = "RIESDESCRIPCION";

        public string Valoracioninherente = "valoracioninherente";
        public string Areacodi = "areacodi";
        #endregion
    }
}
