using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_EVENTO_LOGENVIO
    /// </summary>
    public class ReEventoLogenvioHelper : HelperBase
    {
        public ReEventoLogenvioHelper(): base(Consultas.ReEventoLogenvioSql)
        {
        }

        public ReEventoLogenvioDTO Create(IDataReader dr)
        {
            ReEventoLogenvioDTO entity = new ReEventoLogenvioDTO();

            int iReevlocodi = dr.GetOrdinal(this.Reevlocodi);
            if (!dr.IsDBNull(iReevlocodi)) entity.Reevlocodi = Convert.ToInt32(dr.GetValue(iReevlocodi));

            int iReevprcodi = dr.GetOrdinal(this.Reevprcodi);
            if (!dr.IsDBNull(iReevprcodi)) entity.Reevprcodi = Convert.ToInt32(dr.GetValue(iReevprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReevloindcarga = dr.GetOrdinal(this.Reevloindcarga);
            if (!dr.IsDBNull(iReevloindcarga)) entity.Reevloindcarga = dr.GetString(iReevloindcarga);

            int iReevlomotivocarga = dr.GetOrdinal(this.Reevlomotivocarga);
            if (!dr.IsDBNull(iReevlomotivocarga)) entity.Reevlomotivocarga = dr.GetString(iReevlomotivocarga);

            int iReevlousucreacion = dr.GetOrdinal(this.Reevlousucreacion);
            if (!dr.IsDBNull(iReevlousucreacion)) entity.Reevlousucreacion = dr.GetString(iReevlousucreacion);

            int iReevlofeccreacion = dr.GetOrdinal(this.Reevlofeccreacion);
            if (!dr.IsDBNull(iReevlofeccreacion)) entity.Reevlofeccreacion = dr.GetDateTime(iReevlofeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reevlocodi = "REEVLOCODI";
        public string Reevprcodi = "REEVPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reevloindcarga = "REEVLOINDCARGA";
        public string Reevlomotivocarga = "REEVLOMOTIVOCARGA";
        public string Reevlousucreacion = "REEVLOUSUCREACION";
        public string Reevlofeccreacion = "REEVLOFECCREACION";

        #endregion

        public string SqlObtenerEnvios
        {
            get { return base.GetSqlXml("ObtenerEnvios"); }
        }
    }
}
