using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_ANIOVERSION
    /// </summary>
    public class RerAnioVersionHelper : HelperBase
    {
        public RerAnioVersionHelper() : base(Consultas.RerAnioVersionSql)
        {
        }

        public RerAnioVersionDTO Create(IDataReader dr)
        {
            RerAnioVersionDTO entity = new RerAnioVersionDTO();

            int iReravcodi = dr.GetOrdinal(this.Reravcodi);
            if (!dr.IsDBNull(iReravcodi)) entity.Reravcodi = Convert.ToInt32(dr.GetValue(iReravcodi));

            int iReravversion = dr.GetOrdinal(this.Reravversion);
            if (!dr.IsDBNull(iReravversion)) entity.Reravversion = dr.GetString(iReravversion);

            int iReravaniotarif = dr.GetOrdinal(this.Reravaniotarif);
            if (!dr.IsDBNull(iReravaniotarif)) entity.Reravaniotarif = Convert.ToInt32(dr.GetValue(iReravaniotarif));

            int iReravaniotarifdesc = dr.GetOrdinal(this.Reravaniotarifdesc);
            if (!dr.IsDBNull(iReravaniotarifdesc)) entity.Reravaniotarifdesc = dr.GetString(iReravaniotarifdesc);

            int iReravinflacion = dr.GetOrdinal(this.Reravinflacion);
            if (!dr.IsDBNull(iReravinflacion)) entity.Reravinflacion = dr.GetDecimal(iReravinflacion);

            int iReravestado = dr.GetOrdinal(this.Reravestado);
            if (!dr.IsDBNull(iReravestado)) entity.Reravestado = dr.GetString(iReravestado);

            int iReravusucreacion = dr.GetOrdinal(this.Reravusucreacion);
            if (!dr.IsDBNull(iReravusucreacion)) entity.Reravusucreacion = dr.GetString(iReravusucreacion);

            int iReravfeccreacion = dr.GetOrdinal(this.Reravfeccreacion);
            if (!dr.IsDBNull(iReravfeccreacion)) entity.Reravfeccreacion = dr.GetDateTime(iReravfeccreacion);

            int iReravusumodificacion = dr.GetOrdinal(this.Reravusumodificacion);
            if (!dr.IsDBNull(iReravusumodificacion)) entity.Reravusumodificacion = dr.GetString(iReravusumodificacion);

            int iReravfecmodificacion = dr.GetOrdinal(this.Reravfecmodificacion);
            if (!dr.IsDBNull(iReravfecmodificacion)) entity.Reravfecmodificacion = dr.GetDateTime(iReravfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Reravcodi = "RERAVCODI";
        public string Reravversion = "RERAVVERSION";
        public string Reravaniotarif = "RERAVANIOTARIF";
        public string Reravaniotarifdesc = "RERAVANIOTARIFDESC";
        public string Reravinflacion = "RERAVINFLACION";
        public string Reravestado = "RERAVESTADO";
        public string Reravusucreacion = "RERAVUSUCREACION";
        public string Reravfeccreacion = "RERAVFECCREACION";
        public string Reravusumodificacion = "RERAVUSUMODIFICACION";
        public string Reravfecmodificacion = "RERAVFECMODIFICACION";

        //Additional
        public string Reravversiondesc = "RERAVVERSIONDESC";
        #endregion

        public string SqlGetByAnioAndVersion
        {
            get { return base.GetSqlXml("GetByAnioAndVersion"); }
        }

        public string SqlGetByAnioVersion
        {
            get { return base.GetSqlXml("GetByAnioVersion"); }
        }

        public string SqlListRerAnioVersionesByAnio
        {
            get { return base.GetSqlXml("ListRerAnioVersionesByAnio"); }
        }
    }
}

