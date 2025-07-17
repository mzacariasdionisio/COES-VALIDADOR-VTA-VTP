using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PROGAUDI_ELEMENTO
    /// </summary>
    public class AudProgaudiElementoHelper : HelperBase
    {
        public AudProgaudiElementoHelper(): base(Consultas.AudProgaudiElementoSql)
        {
        }

        public string SqlGetByCriteriaPorAuditoria
        {
            get { return GetSqlXml("GetByCriteriaPorAuditoria"); }
        }

        public string SqlGetByElemcodi
        {
            get { return GetSqlXml("GetByElemcodi"); }
        }
        

        public AudProgaudiElementoDTO Create(IDataReader dr)
        {
            AudProgaudiElementoDTO entity = new AudProgaudiElementoDTO();

            int iProgaecodi = dr.GetOrdinal(this.Progaecodi);
            if (!dr.IsDBNull(iProgaecodi)) entity.Progaecodi = Convert.ToInt32(dr.GetValue(iProgaecodi));

            int iProgacodi = dr.GetOrdinal(this.Progacodi);
            if (!dr.IsDBNull(iProgacodi)) entity.Progacodi = Convert.ToInt32(dr.GetValue(iProgacodi));

            int iElemcodi = dr.GetOrdinal(this.Elemcodi);
            if (!dr.IsDBNull(iElemcodi)) entity.Elemcodi = Convert.ToInt32(dr.GetValue(iElemcodi));

            int iProgaeiniciorevision = dr.GetOrdinal(this.Progaeiniciorevision);
            if (!dr.IsDBNull(iProgaeiniciorevision)) entity.Progaeiniciorevision = dr.GetDateTime(iProgaeiniciorevision);

            int iProgaefinrevision = dr.GetOrdinal(this.Progaefinrevision);
            if (!dr.IsDBNull(iProgaefinrevision)) entity.Progaefinrevision = dr.GetDateTime(iProgaefinrevision);

            int iProgaetamanomuestra = dr.GetOrdinal(this.Progaetamanomuestra);
            if (!dr.IsDBNull(iProgaetamanomuestra)) entity.Progaetamanomuestra = Convert.ToInt32(dr.GetValue(iProgaetamanomuestra));

            int iProgaemuestraseleccionada = dr.GetOrdinal(this.Progaemuestraseleccionada);
            if (!dr.IsDBNull(iProgaemuestraseleccionada)) entity.Progaemuestraseleccionada = dr.GetString(iProgaemuestraseleccionada);

            int iProgaeprocedimientoprueba = dr.GetOrdinal(this.Progaeprocedimientoprueba);
            if (!dr.IsDBNull(iProgaeprocedimientoprueba)) entity.Progaeprocedimientoprueba = dr.GetString(iProgaeprocedimientoprueba);

            int iProgaeactivo = dr.GetOrdinal(this.Progaeactivo);
            if (!dr.IsDBNull(iProgaeactivo)) entity.Progaeactivo = dr.GetString(iProgaeactivo);

            int iProgaehistorico = dr.GetOrdinal(this.Progaehistorico);
            if (!dr.IsDBNull(iProgaehistorico)) entity.Progaehistorico = dr.GetString(iProgaehistorico);

            int iProgaeusucreacion = dr.GetOrdinal(this.Progaeusucreacion);
            if (!dr.IsDBNull(iProgaeusucreacion)) entity.Progaeusucreacion = dr.GetString(iProgaeusucreacion);

            int iProgaefechacreacion = dr.GetOrdinal(this.Progaefechacreacion);
            if (!dr.IsDBNull(iProgaefechacreacion)) entity.Progaefechacreacion = dr.GetDateTime(iProgaefechacreacion);

            int iProgaeusumodificacion = dr.GetOrdinal(this.Progaeusumodificacion);
            if (!dr.IsDBNull(iProgaeusumodificacion)) entity.Progaeusumodificacion = dr.GetString(iProgaeusumodificacion);

            int iProgaefechamodificacion = dr.GetOrdinal(this.Progaefechamodificacion);
            if (!dr.IsDBNull(iProgaefechamodificacion)) entity.Progaefechamodificacion = dr.GetDateTime(iProgaefechamodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Progaecodi = "PROGAECODI";
        public string Progacodi = "PROGACODI";
        public string Elemcodi = "ELEMCODI";
        public string Progaeiniciorevision = "PROGAEINICIOREVISION";
        public string Progaefinrevision = "PROGAEFINREVISION";
        public string Progaetamanomuestra = "PROGAETAMANOMUESTRA";
        public string Progaemuestraseleccionada = "PROGAEMUESTRASELECCIONADA";
        public string Progaeprocedimientoprueba = "PROGAEPROCEDIMIENTOPRUEBA";
        public string Progaeactivo = "PROGAEACTIVO";
        public string Progaehistorico = "PROGAEHISTORICO";
        public string Progaeusucreacion = "PROGAEUSUCREACION";
        public string Progaefechacreacion = "PROGAEFECHACREACION";
        public string Progaeusumodificacion = "PROGAEUSUMODIFICACION";
        public string Progaefechamodificacion = "PROGAEFECHAMODIFICACION";

        public string Proccodi = "PROCCODI";
        public string Elemdescripcion = "ELEMDESCRIPCION";
        public string Elemcodigo = "ELEMCODIGO";
        
        public string Audicodi = "AUDICODI";
        

        #endregion
    }
}
