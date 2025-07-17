using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EMS_GENERACION
    /// </summary>
    public class EmsGeneracionHelper : HelperBase
    {
        public EmsGeneracionHelper(): base(Consultas.EmsGeneracionSql)
        {
        }

        public EmsGeneracionDTO Create(IDataReader dr)
        {
            EmsGeneracionDTO entity = new EmsGeneracionDTO();

            int iEmggencodi = dr.GetOrdinal(this.Emggencodi);
            if (!dr.IsDBNull(iEmggencodi)) entity.Emggencodi = Convert.ToInt32(dr.GetValue(iEmggencodi));

            int iEmsgenfecha = dr.GetOrdinal(this.Emsgenfecha);
            if (!dr.IsDBNull(iEmsgenfecha)) entity.Emsgenfecha = dr.GetDateTime(iEmsgenfecha);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmsgenoperativo = dr.GetOrdinal(this.Emsgenoperativo);
            if (!dr.IsDBNull(iEmsgenoperativo)) entity.Emsgenoperativo = dr.GetString(iEmsgenoperativo);

            int iEmsgenvalor = dr.GetOrdinal(this.Emsgenvalor);
            if (!dr.IsDBNull(iEmsgenvalor)) entity.Emsgenvalor = dr.GetDecimal(iEmsgenvalor);

            int iEmsgenpotmax = dr.GetOrdinal(this.Emsgenpotmax);
            if (!dr.IsDBNull(iEmsgenpotmax)) entity.Emsgenpotmax = dr.GetDecimal(iEmsgenpotmax);

            int iEmsgenusucreacion = dr.GetOrdinal(this.Emsgenusucreacion);
            if (!dr.IsDBNull(iEmsgenusucreacion)) entity.Emsgenusucreacion = dr.GetString(iEmsgenusucreacion);

            int iEmsgenfeccreacion = dr.GetOrdinal(this.Emsgenfeccreacion);
            if (!dr.IsDBNull(iEmsgenfeccreacion)) entity.Emsgenfeccreacion = dr.GetDateTime(iEmsgenfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Emggencodi = "EMGGENCODI";
        public string Emsgenfecha = "EMSGENFECHA";
        public string Equicodi = "EQUICODI";
        public string Emsgenoperativo = "EMSGENOPERATIVO";
        public string Emsgenvalor = "EMSGENVALOR";
        public string Emsgenusucreacion = "EMSGENUSUCREACION";
        public string Emsgenfeccreacion = "EMSGENFECCREACION";
        public string Emsgenpotmax = "EMSGENPOTMAX";
        public string Grupocodi = "GRUPOCODI";
        public string Indice = "INDICE";
        public string Tgenercodi = "TGENERCODI";

        #endregion

        public string SqlObtenerSupervisionDemanda
        {
            get { return base.GetSqlXml("ObtenerSupervisionDemanda"); }
        }
    }
}
