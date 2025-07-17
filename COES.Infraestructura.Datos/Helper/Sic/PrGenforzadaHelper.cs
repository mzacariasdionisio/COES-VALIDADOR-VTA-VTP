using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GENFORZADA
    /// </summary>
    public class PrGenforzadaHelper : HelperBase
    {
        public PrGenforzadaHelper(): base(Consultas.PrGenforzadaSql)
        {
        }

        public PrGenforzadaDTO Create(IDataReader dr)
        {
            PrGenforzadaDTO entity = new PrGenforzadaDTO();

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iGenforcodi = dr.GetOrdinal(this.Genforcodi);
            if (!dr.IsDBNull(iGenforcodi)) entity.Genforcodi = Convert.ToInt32(dr.GetValue(iGenforcodi));

            int iGenforinicio = dr.GetOrdinal(this.Genforinicio);
            if (!dr.IsDBNull(iGenforinicio)) entity.Genforinicio = dr.GetDateTime(iGenforinicio);

            int iGenforfin = dr.GetOrdinal(this.Genforfin);
            if (!dr.IsDBNull(iGenforfin)) entity.Genforfin = dr.GetDateTime(iGenforfin);

            int iGenforsimbolo = dr.GetOrdinal(this.Genforsimbolo);
            if (!dr.IsDBNull(iGenforsimbolo)) entity.Genforsimbolo = dr.GetString(iGenforsimbolo);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Relacioncodi = "RELACIONCODI";
        public string Genforcodi = "GENFORCODI";
        public string Genforinicio = "GENFORINICIO";
        public string Genforfin = "GENFORFIN";
        public string Genforsimbolo = "GENFORSIMBOLO";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Equinomb = "EQUINOMB";
        public string Nombarra = "NOMBARRA";
        public string Idgener = "IDGENER";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Subcausacmg = "SUBCAUSACMG";
        public string Codbarra = "CODBARRA";
        public string Nombretna = "NOMBRETNA";

        public string SqlObtenerGeneracionForzadaProceso
        {
            get { return base.GetSqlXml("ObtenerGeneracionForzadaProceso"); }
        }

        public string SqlObtenerGeneracionForzadaProcesoV2
        {
            get { return base.GetSqlXml("ObtenerGeneracionForzadaProcesoV2"); }
        }

        public string SqlObtenerUnidadesPasada
        {
            get { return base.GetSqlXml("ObtenerUnidadesPasada"); }
        }

        #endregion
    }
}
