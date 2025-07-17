using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_RAEJECUTADADET
    /// </summary>
    public class CoRaejecutadadetHelper : HelperBase
    {
        public CoRaejecutadadetHelper(): base(Consultas.CoRaejecutadadetSql)
        {
        }

        public CoRaejecutadadetDTO Create(IDataReader dr)
        {
            CoRaejecutadadetDTO entity = new CoRaejecutadadetDTO();

            int iCoradecodi = dr.GetOrdinal(this.Coradecodi);
            if (!dr.IsDBNull(iCoradecodi)) entity.Coradecodi = Convert.ToInt32(dr.GetValue(iCoradecodi));

            int iCoradefecha = dr.GetOrdinal(this.Coradefecha);
            if (!dr.IsDBNull(iCoradefecha)) entity.Coradefecha = dr.GetDateTime(iCoradefecha);

            int iCoradeindice = dr.GetOrdinal(this.Coradeindice);
            if (!dr.IsDBNull(iCoradeindice)) entity.Coradeindice = Convert.ToInt32(dr.GetValue(iCoradeindice));

            int iCorademinutos = dr.GetOrdinal(this.Corademinutos);
            if (!dr.IsDBNull(iCorademinutos)) entity.Corademinutos = Convert.ToInt32(dr.GetValue(iCorademinutos));

            int iCoraderasub = dr.GetOrdinal(this.Coraderasub);
            if (!dr.IsDBNull(iCoraderasub)) entity.Coraderasub = dr.GetDecimal(iCoraderasub);

            int iCoraderabaj = dr.GetOrdinal(this.Coraderabaj);
            if (!dr.IsDBNull(iCoraderabaj)) entity.Coraderabaj = dr.GetDecimal(iCoraderabaj);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Coradecodi = "CORADECODI";
        public string Coradefecha = "CORADEFECHA";
        public string Coradeindice = "CORADEINDICE";
        public string Corademinutos = "CORADEMINUTOS";
        public string Coraderasub = "CORADERASUB";
        public string Coraderabaj = "CORADERABAJ";
        public string Grupocodi = "GRUPOCODI";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Gruponomb = "GRUPONOMB";

        #endregion

        public string SqlObtenerConsulta
        {
            get { return base.GetSqlXml("ObtenerConsulta"); }
        }
    }
}
