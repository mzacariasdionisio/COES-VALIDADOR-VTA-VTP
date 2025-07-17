using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInformHelper : HelperBase
    {
        public AudRequerimientoInformHelper(): base(Consultas.AudRequerimientoInformSql)
        {
        }

        public string SqlObtenerNroRegistroBusquedaByAuditoria
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusquedaByAuditoria"); }
        }

        public string SqlGetByCriteriaByAuditoria
        {
            get { return base.GetSqlXml("GetByCriteriaByAuditoria"); }
        }

        public AudRequerimientoInformDTO Create(IDataReader dr)
        {
            AudRequerimientoInformDTO entity = new AudRequerimientoInformDTO();

            int iReqicodi = dr.GetOrdinal(this.Reqicodi);
            if (!dr.IsDBNull(iReqicodi)) entity.Reqicodi = Convert.ToInt32(dr.GetValue(iReqicodi));

            int iProgaecodi = dr.GetOrdinal(this.Progaecodi);
            if (!dr.IsDBNull(iProgaecodi)) entity.Progaecodi = Convert.ToInt32(dr.GetValue(iProgaecodi));

            int iTabcdcodiestado = dr.GetOrdinal(this.Tabcdcodiestado);
            if (!dr.IsDBNull(iTabcdcodiestado)) entity.Tabcdcodiestado = Convert.ToInt32(dr.GetValue(iTabcdcodiestado));

            int iPercodiresponsable = dr.GetOrdinal(this.Percodiresponsable);
            if (!dr.IsDBNull(iPercodiresponsable)) entity.Percodiresponsable = Convert.ToInt32(dr.GetValue(iPercodiresponsable));

            int iArchcodirequerimiento = dr.GetOrdinal(this.Archcodirequerimiento);
            if (!dr.IsDBNull(iArchcodirequerimiento)) entity.Archcodirequerimiento = Convert.ToInt32(dr.GetValue(iArchcodirequerimiento));

            int iReqiplazo = dr.GetOrdinal(this.Reqiplazo);
            if (!dr.IsDBNull(iReqiplazo)) entity.Reqiplazo = dr.GetDateTime(iReqiplazo);

            int iReqirequerimiento = dr.GetOrdinal(this.Reqirequerimiento);
            if (!dr.IsDBNull(iReqirequerimiento)) entity.Reqirequerimiento = dr.GetString(iReqirequerimiento);

            int iReqifechasolicitada = dr.GetOrdinal(this.Reqifechasolicitada);
            if (!dr.IsDBNull(iReqifechasolicitada)) entity.Reqifechasolicitada = dr.GetDateTime(iReqifechasolicitada);

            int iReqifechapresentada = dr.GetOrdinal(this.Reqifechapresentada);
            if (!dr.IsDBNull(iReqifechapresentada)) entity.Reqifechapresentada = dr.GetDateTime(iReqifechapresentada);

            int iReqiactivo = dr.GetOrdinal(this.Reqiactivo);
            if (!dr.IsDBNull(iReqiactivo)) entity.Reqiactivo = dr.GetString(iReqiactivo);

            int iReqihistorico = dr.GetOrdinal(this.Reqihistorico);
            if (!dr.IsDBNull(iReqihistorico)) entity.Reqihistorico = dr.GetString(iReqihistorico);

            int iReqiusuregistro = dr.GetOrdinal(this.Reqiusuregistro);
            if (!dr.IsDBNull(iReqiusuregistro)) entity.Reqiusuregistro = dr.GetString(iReqiusuregistro);

            int iReqifecregistro = dr.GetOrdinal(this.Reqifecregistro);
            if (!dr.IsDBNull(iReqifecregistro)) entity.Reqifecregistro = dr.GetDateTime(iReqifecregistro);

            int iReqiusumodificacion = dr.GetOrdinal(this.Reqiusumodificacion);
            if (!dr.IsDBNull(iReqiusumodificacion)) entity.Reqiusumodificacion = dr.GetString(iReqiusumodificacion);

            int iReqifecmodificacion = dr.GetOrdinal(this.Reqifecmodificacion);
            if (!dr.IsDBNull(iReqifecmodificacion)) entity.Reqifecmodificacion = dr.GetDateTime(iReqifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reqicodi = "REQICODI";
        public string Progaecodi = "PROGAECODI";
        public string Tabcdcodiestado = "TABCDCODIESTADO";
        public string Percodiresponsable = "PERCODIRESPONSABLE";
        public string Archcodirequerimiento = "ARCHCODIREQUERIMIENTO";
        public string Reqiplazo = "REQIPLAZO";
        public string Reqirequerimiento = "REQIREQUERIMIENTO";
        public string Reqifechasolicitada = "REQIFECHASOLICITADA";
        public string Reqifechapresentada = "REQIFECHAPRESENTADA";
        public string Reqiactivo = "REQIACTIVO";
        public string Reqihistorico = "REQIHISTORICO";
        public string Reqiusuregistro = "REQIUSUREGISTRO";
        public string Reqifecregistro = "REQIFECREGISTRO";
        public string Reqiusumodificacion = "REQIUSUMODIFICACION";
        public string Reqifecmodificacion = "REQIFECMODIFICACION";

        public string Elemcodigo = "Elemcodigo";
        public string Elemdescripcion = "elemdescripcion";
        public string Estadodescripcion = "Estadodescripcion";

        public string Tienearchivo = "Tienearchivo";
        

        public string Usercode = "usercode";
        
        #endregion
    }
}
