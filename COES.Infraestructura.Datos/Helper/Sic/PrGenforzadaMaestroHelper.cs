using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GENFORZADA_MAESTRO
    /// </summary>
    public class PrGenforzadaMaestroHelper : HelperBase
    {
        public PrGenforzadaMaestroHelper(): base(Consultas.PrGenforzadaMaestroSql)
        {
        }

        public PrGenforzadaMaestroDTO Create(IDataReader dr)
        {
            PrGenforzadaMaestroDTO entity = new PrGenforzadaMaestroDTO();

            int iGenformaecodi = dr.GetOrdinal(this.Genformaecodi);
            if (!dr.IsDBNull(iGenformaecodi)) entity.Genformaecodi = Convert.ToInt32(dr.GetValue(iGenformaecodi));

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iIndestado = dr.GetOrdinal(this.Indestado);
            if (!dr.IsDBNull(iIndestado)) entity.Indestado = dr.GetString(iIndestado);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iGenformaesimbolo = dr.GetOrdinal(this.Genformaesimbolo);
            if (!dr.IsDBNull(iGenformaesimbolo)) entity.Genformaesimbolo = dr.GetString(iGenformaesimbolo);

            int iGenfortipo = dr.GetOrdinal(this.Genfortipo);
            if (!dr.IsDBNull(iGenfortipo)) entity.Genfortipo = dr.GetString(iGenfortipo);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));


            return entity;
        }


        #region Mapeo de Campos

        public string Genformaecodi = "GENFORMAECODI";
        public string Relacioncodi = "RELACIONCODI";
        public string Indestado = "INDESTADO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Genformaesimbolo = "GENFORMAESIMBOLO";
        public string Genfortipo = "GENFORTIPO";
        public string Equinomb = "EQUINOMB";
        public string Nombarra = "NOMBARRA";
        public string Idgener = "IDGENER";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Subcausadesc = "SUBCAUSADESC";


        public string SqlValidarExistenciaPorRelacion
        {
            get { return base.GetSqlXml("ValidarExistenciaPorRelacion"); }
        }

        #endregion
    }
}
