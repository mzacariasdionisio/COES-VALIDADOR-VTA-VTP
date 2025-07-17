using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_LOGCORREO
    /// </summary>
    public class ReLogcorreoHelper : HelperBase
    {
        public ReLogcorreoHelper(): base(Consultas.ReLogcorreoSql)
        {
        }

        public ReLogcorreoDTO Create(IDataReader dr)
        {
            ReLogcorreoDTO entity = new ReLogcorreoDTO();

            int iRelcorcodi = dr.GetOrdinal(this.Relcorcodi);
            if (!dr.IsDBNull(iRelcorcodi)) entity.Relcorcodi = Convert.ToInt32(dr.GetValue(iRelcorcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iRetcorcodi = dr.GetOrdinal(this.Retcorcodi);
            if (!dr.IsDBNull(iRetcorcodi)) entity.Retcorcodi = Convert.ToInt32(dr.GetValue(iRetcorcodi));

            int iRelcorasunto = dr.GetOrdinal(this.Relcorasunto);
            if (!dr.IsDBNull(iRelcorasunto)) entity.Relcorasunto = dr.GetString(iRelcorasunto);

            int iRelcorto = dr.GetOrdinal(this.Relcorto);
            if (!dr.IsDBNull(iRelcorto)) entity.Relcorto = dr.GetString(iRelcorto);

            int iRelcorcc = dr.GetOrdinal(this.Relcorcc);
            if (!dr.IsDBNull(iRelcorcc)) entity.Relcorcc = dr.GetString(iRelcorcc);

            int iRelcorbcc = dr.GetOrdinal(this.Relcorbcc);
            if (!dr.IsDBNull(iRelcorbcc)) entity.Relcorbcc = dr.GetString(iRelcorbcc);

            int iRelcorcuerpo = dr.GetOrdinal(this.Relcorcuerpo);
            if (!dr.IsDBNull(iRelcorcuerpo)) entity.Relcorcuerpo = dr.GetString(iRelcorcuerpo);

            int iRelcorusucreacion = dr.GetOrdinal(this.Relcorusucreacion);
            if (!dr.IsDBNull(iRelcorusucreacion)) entity.Relcorusucreacion = dr.GetString(iRelcorusucreacion);

            int iRelcorfeccreacion = dr.GetOrdinal(this.Relcorfeccreacion);
            if (!dr.IsDBNull(iRelcorfeccreacion)) entity.Relcorfeccreacion = dr.GetDateTime(iRelcorfeccreacion);

            int iRelcorempresa = dr.GetOrdinal(this.Relcorempresa);
            if (!dr.IsDBNull(iRelcorempresa)) entity.Relcorempresa = Convert.ToInt32(dr.GetValue(iRelcorempresa));

            int iRelcorarchivosnomb = dr.GetOrdinal(this.Relcorarchivosnomb);
            if (!dr.IsDBNull(iRelcorarchivosnomb)) entity.Relcorarchivosnomb = dr.GetString(iRelcorarchivosnomb);
            
            return entity;
        }


        #region Mapeo de Campos

        public string Relcorcodi = "RELCORCODI";
        public string Repercodi = "REPERCODI";
        public string Retcorcodi = "RETCORCODI";
        public string Relcorasunto = "RELCORASUNTO";
        public string Relcorto = "RELCORTO";
        public string Relcorcc = "RELCORCC";
        public string Relcorbcc = "RELCORBCC";
        public string Relcorcuerpo = "RELCORCUERPO";
        public string Relcorusucreacion = "RELCORUSUCREACION";
        public string Relcorfeccreacion = "RELCORFECCREACION"; 
        public string Relcorempresa = "RELCOREMPRESA"; 
        public string Relcorarchivosnomb = "RELCORARCHIVOSNOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlGetPorFechaYTipo
        {
            get { return base.GetSqlXml("GetPorFechaYTipo"); }
        }
    }
}
