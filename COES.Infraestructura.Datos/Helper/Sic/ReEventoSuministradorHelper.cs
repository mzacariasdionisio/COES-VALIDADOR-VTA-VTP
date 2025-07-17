using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_EVENTO_SUMINISTRADOR
    /// </summary>
    public class ReEventoSuministradorHelper : HelperBase
    {
        public ReEventoSuministradorHelper(): base(Consultas.ReEventoSuministradorSql)
        {
        }

        public ReEventoSuministradorDTO Create(IDataReader dr)
        {
            ReEventoSuministradorDTO entity = new ReEventoSuministradorDTO();

            int iReevsucodi = dr.GetOrdinal(this.Reevsucodi);
            if (!dr.IsDBNull(iReevsucodi)) entity.Reevsucodi = Convert.ToInt32(dr.GetValue(iReevsucodi));

            int iReevprcodi = dr.GetOrdinal(this.Reevprcodi);
            if (!dr.IsDBNull(iReevprcodi)) entity.Reevprcodi = Convert.ToInt32(dr.GetValue(iReevprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReevsuindcarga = dr.GetOrdinal(this.Reevsuindcarga);
            if (!dr.IsDBNull(iReevsuindcarga)) entity.Reevsuindcarga = dr.GetString(iReevsuindcarga);

            int iReevsuresarcimiento = dr.GetOrdinal(this.Reevsuresarcimiento);
            if (!dr.IsDBNull(iReevsuresarcimiento)) entity.Reevsuresarcimiento = dr.GetDecimal(iReevsuresarcimiento);

            int iReevsuestado = dr.GetOrdinal(this.Reevsuestado);
            if (!dr.IsDBNull(iReevsuestado)) entity.Reevsuestado = dr.GetString(iReevsuestado);

            int iReevsuusucreacion = dr.GetOrdinal(this.Reevsuusucreacion);
            if (!dr.IsDBNull(iReevsuusucreacion)) entity.Reevsuusucreacion = dr.GetString(iReevsuusucreacion);

            int iReevsufeccreacion = dr.GetOrdinal(this.Reevsufeccreacion);
            if (!dr.IsDBNull(iReevsufeccreacion)) entity.Reevsufeccreacion = dr.GetDateTime(iReevsufeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reevsucodi = "REEVSUCODI";
        public string Reevprcodi = "REEVPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reevsuindcarga = "REEVSUINDCARGA";
        public string Reevsuresarcimiento = "REEVSURESARCIMIENTO";
        public string Reevsuestado = "REEVSUESTADO";
        public string Reevsuusucreacion = "REEVSUUSUCREACION";
        public string Reevsufeccreacion = "REEVSUFECCREACION";
        public string Emprnomb = "EMPRNOMB";

        public string SqlObtenerSuministradoresPorEvento
        {
            get { return base.GetSqlXml("ObtenerSuministradoresPorEvento"); }
        }

        public string SqlObtenerSuministrador 
        {
            get { return base.GetSqlXml("ObtenerSuministrador"); }
        }

        public string SqlListarPorEvento
        {
            get { return base.GetSqlXml("ListarPorEvento"); }
        }
        

        #endregion
    }
}
