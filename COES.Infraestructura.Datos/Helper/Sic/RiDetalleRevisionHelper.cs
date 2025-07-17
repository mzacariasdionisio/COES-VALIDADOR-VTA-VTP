using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_DETALLE_REVISION
    /// </summary>
    public class RiDetalleRevisionHelper : HelperBase
    {
        public RiDetalleRevisionHelper() : base(Consultas.RiDetalleRevisionSql)
        {
        }

        public RiDetalleRevisionDTO Create(IDataReader dr)
        {
            RiDetalleRevisionDTO entity = new RiDetalleRevisionDTO();

            int iDervcodi = dr.GetOrdinal(this.Dervcodi);
            if (!dr.IsDBNull(iDervcodi)) entity.Dervcodi = Convert.ToInt32(dr.GetValue(iDervcodi));

            int iDervcampo = dr.GetOrdinal(this.Dervcampo);
            if (!dr.IsDBNull(iDervcampo)) entity.Dervcampo = dr.GetString(iDervcampo);

            int iDervvalor = dr.GetOrdinal(this.Dervvalor);
            if (!dr.IsDBNull(iDervvalor)) entity.Dervvalor = dr.GetString(iDervvalor);

            int iDervobservacion = dr.GetOrdinal(this.Dervobservacion);
            if (!dr.IsDBNull(iDervobservacion)) entity.Dervobservacion = dr.GetString(iDervobservacion);

            int iDervadjunto = dr.GetOrdinal(this.Dervadjunto);
            if (!dr.IsDBNull(iDervadjunto)) entity.Dervadjunto = dr.GetString(iDervadjunto);

            int iDervvaloradjunto = dr.GetOrdinal(this.Dervvaloradjunto);
            if (!dr.IsDBNull(iDervvaloradjunto)) entity.Dervvaloradjunto = dr.GetString(iDervvaloradjunto);

            int iRevicodi = dr.GetOrdinal(this.Revicodi);
            if (!dr.IsDBNull(iRevicodi)) entity.Revicodi = Convert.ToInt32(dr.GetValue(iRevicodi));

            int iDervusucreacion = dr.GetOrdinal(this.Dervusucreacion);
            if (!dr.IsDBNull(iDervusucreacion)) entity.Dervusucreacion = dr.GetString(iDervusucreacion);

            int iDervfeccreacion = dr.GetOrdinal(this.Dervfeccreacion);
            if (!dr.IsDBNull(iDervfeccreacion)) entity.Dervfeccreacion = dr.GetDateTime(iDervfeccreacion);

            int iDervusumoficicacion = dr.GetOrdinal(this.Dervusumoficicacion);
            if (!dr.IsDBNull(iDervusumoficicacion)) entity.Dervusumoficicacion = dr.GetString(iDervusumoficicacion);

            int iDervfecmodificacion = dr.GetOrdinal(this.Dervfecmodificacion);
            if (!dr.IsDBNull(iDervfecmodificacion)) entity.Dervfecmodificacion = dr.GetDateTime(iDervfecmodificacion);

            return entity;
        }

        #region Consultas SQL
        public string SqlListByRevicodi
        {
            get { return base.GetSqlXml("ListByRevicodi"); }

        }
        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }


        #endregion

        #region Mapeo de Campos

        public string Dervcodi = "DERVCODI";
        public string Dervcampo = "DERVCAMPO";
        public string Dervvalor = "DERVVALOR";
        public string Dervobservacion = "DERVOBSERVACION";
        public string Dervadjunto = "DERVADJUNTO";
        public string Dervvaloradjunto = "DERVVALORADJUNTO";
        public string Revicodi = "REVICODI";
        public string Dervusucreacion = "DERVUSUCREACION";
        public string Dervfeccreacion = "DERVFECCREACION";
        public string Dervusumoficicacion = "DERVUSUMOFICICACION";
        public string Dervfecmodificacion = "DERVFECMODIFICACION";
        public string Dervestado = "DERVESTADO";

        #endregion
    }
}
