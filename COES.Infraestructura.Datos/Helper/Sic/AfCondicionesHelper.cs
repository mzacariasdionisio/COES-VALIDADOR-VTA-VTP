using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_CONDICIONES
    /// </summary>
    public class AfCondicionesHelper : HelperBase
    {
        public AfCondicionesHelper() : base(Consultas.AfCondicionesSql)
        {
        }

        public AfCondicionesDTO Create(IDataReader dr)
        {
            AfCondicionesDTO entity = new AfCondicionesDTO();

            int iAfcondfecmodificacion = dr.GetOrdinal(this.Afcondfecmodificacion);
            if (!dr.IsDBNull(iAfcondfecmodificacion)) entity.Afcondfecmodificacion = dr.GetDateTime(iAfcondfecmodificacion);

            int iAfcondusumodificacion = dr.GetOrdinal(this.Afcondusumodificacion);
            if (!dr.IsDBNull(iAfcondusumodificacion)) entity.Afcondusumodificacion = dr.GetString(iAfcondusumodificacion);

            int iAfcondfeccreacion = dr.GetOrdinal(this.Afcondfeccreacion);
            if (!dr.IsDBNull(iAfcondfeccreacion)) entity.Afcondfeccreacion = dr.GetDateTime(iAfcondfeccreacion);

            int iAfcondusucreacion = dr.GetOrdinal(this.Afcondusucreacion);
            if (!dr.IsDBNull(iAfcondusucreacion)) entity.Afcondusucreacion = dr.GetString(iAfcondusucreacion);

            int iAfcondestado = dr.GetOrdinal(this.Afcondestado);
            if (!dr.IsDBNull(iAfcondestado)) entity.Afcondestado = Convert.ToInt32(dr.GetValue(iAfcondestado));

            int iAfcondzona = dr.GetOrdinal(this.Afcondzona);
            if (!dr.IsDBNull(iAfcondzona)) entity.Afcondzona = dr.GetString(iAfcondzona);

            int iAfcondnumetapa = dr.GetOrdinal(this.Afcondnumetapa);
            if (!dr.IsDBNull(iAfcondnumetapa)) entity.Afcondnumetapa = Convert.ToInt32(dr.GetValue(iAfcondnumetapa));

            int iAfcondfuncion = dr.GetOrdinal(this.Afcondfuncion);
            if (!dr.IsDBNull(iAfcondfuncion)) entity.Afcondfuncion = dr.GetString(iAfcondfuncion);

            int iAfcondcodi = dr.GetOrdinal(this.Afcondcodi);
            if (!dr.IsDBNull(iAfcondcodi)) entity.Afcondcodi = Convert.ToInt32(dr.GetValue(iAfcondcodi));

            int iAfecodi = dr.GetOrdinal(this.Afecodi);
            if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = Convert.ToInt32(dr.GetValue(iAfecodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Afcondfecmodificacion = "AFCONDFECMODIFICACION";
        public string Afcondusumodificacion = "AFCONDUSUMODIFICACION";
        public string Afcondfeccreacion = "AFCONDFECCREACION";
        public string Afcondusucreacion = "AFCONDUSUCREACION";
        public string Afcondestado = "AFCONDESTADO";
        public string Afcondzona = "AFCONDZONA";
        public string Afcondnumetapa = "AFCONDNUMETAPA";
        public string Afcondfuncion = "AFCONDFUNCION";
        public string Afcondcodi = "AFCONDCODI";
        public string Afecodi = "AFECODI";

        #endregion

        /// <summary>
        /// Retorna las sentencias SQL
        /// </summary>
        #region Sentencias SQL

        public string SqlListByAfecodi
        {
            get { return GetSqlXml("ListByAfecodi"); }
        }

        public string SqlDeleteByAfecodi
        {
            get { return GetSqlXml("DeleteByAfecodi"); }
        }

        #endregion
    }
}
