using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_EVALUACION
    /// </summary>
    public class RerEvaluacionHelper : HelperBase
    {
        #region Mapeo de Campos
        //table
        public string Rerevacodi = "REREVACODI";
        public string Rerrevcodi = "RERREVCODI";
        public string Rerevanumversion = "REREVANUMVERSION";
        public string Rerevaestado = "REREVAESTADO";
        public string Rerevausucreacion = "REREVAUSUCREACION";
        public string Rerevafeccreacion = "REREVAFECCREACION";
        public string Rerevausumodificacion = "REREVAUSUMODIFICACION";
        public string Rerevafecmodificacion = "REREVAFECMODIFICACION";

        //Additional
        public string Iperinombre = "IPERINOMBRE";
        public string Iperianio = "IPERIANIO";
        public string Iperimes = "IPERIMES";
        public string Rerrevnombre = "RERREVNOMBRE";
        #endregion

        public RerEvaluacionHelper() : base(Consultas.RerEvaluacionSql)
        {
        }

        public RerEvaluacionDTO Create(IDataReader dr)
        {
            RerEvaluacionDTO entity = new RerEvaluacionDTO();
            SetCreate(dr, entity);
            return entity;
        }

        public RerEvaluacionDTO CreateById(IDataReader dr)
        {
            RerEvaluacionDTO entity = new RerEvaluacionDTO();
            SetCreate(dr, entity);

            int iPerinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Iperinombre = dr.GetString(iPerinombre);

            int iRerrevnombre = dr.GetOrdinal(this.Rerrevnombre);
            if (!dr.IsDBNull(iRerrevnombre)) entity.Rerrevnombre = dr.GetString(iRerrevnombre);

            return entity;
        }

        public RerEvaluacionDTO CreateByCriteria(IDataReader dr)
        {
            RerEvaluacionDTO entity = new RerEvaluacionDTO();
            SetCreate(dr, entity);

            int iPerinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Iperinombre = dr.GetString(iPerinombre);

            int iRerrevnombre = dr.GetOrdinal(this.Rerrevnombre);
            if (!dr.IsDBNull(iRerrevnombre)) entity.Rerrevnombre = dr.GetString(iRerrevnombre);

            return entity;
        }

        public RerEvaluacionDTO CreateUltimaByEstadoEvaluacionByAnioTarifario(IDataReader dr)
        {
            RerEvaluacionDTO entity = new RerEvaluacionDTO();
            SetCreate(dr, entity);

            int iRerrevnombre = dr.GetOrdinal(this.Rerrevnombre);
            if (!dr.IsDBNull(iRerrevnombre)) entity.Rerrevnombre = dr.GetString(iRerrevnombre);

            int iIperianio = dr.GetOrdinal(this.Iperianio);
            if (!dr.IsDBNull(iIperianio)) entity.Iperianio = Convert.ToInt32(dr.GetValue(iIperianio));

            int iIperimes = dr.GetOrdinal(this.Iperimes);
            if (!dr.IsDBNull(iIperimes)) entity.Iperimes = Convert.ToInt32(dr.GetValue(iIperimes));



            return entity;
        }
        
        private void SetCreate(IDataReader dr, RerEvaluacionDTO entity)
        {
            int iRerevacodi = dr.GetOrdinal(this.Rerevacodi);
            if (!dr.IsDBNull(iRerevacodi)) entity.Rerevacodi = Convert.ToInt32(dr.GetValue(iRerevacodi));

            int iRerrevcodi = dr.GetOrdinal(this.Rerrevcodi);
            if (!dr.IsDBNull(iRerrevcodi)) entity.Rerrevcodi = Convert.ToInt32(dr.GetValue(iRerrevcodi));

            int iRerevanumversion = dr.GetOrdinal(this.Rerevanumversion);
            if (!dr.IsDBNull(iRerevanumversion)) entity.Rerevanumversion = Convert.ToInt32(dr.GetValue(iRerevanumversion));

            int iRerevaestado = dr.GetOrdinal(this.Rerevaestado);
            if (!dr.IsDBNull(iRerevaestado)) entity.Rerevaestado = dr.GetString(iRerevaestado);

            int iRerevausucreacion = dr.GetOrdinal(this.Rerevausucreacion);
            if (!dr.IsDBNull(iRerevausucreacion)) entity.Rerevausucreacion = dr.GetString(iRerevausucreacion);

            int iRerevafeccreacion = dr.GetOrdinal(this.Rerevafeccreacion);
            if (!dr.IsDBNull(iRerevafeccreacion)) entity.Rerevafeccreacion = dr.GetDateTime(iRerevafeccreacion);

            int iRerevausumodificacion = dr.GetOrdinal(this.Rerevausumodificacion);
            if (!dr.IsDBNull(iRerevausumodificacion)) entity.Rerevausumodificacion = dr.GetString(iRerevausumodificacion);

            int iRerevafecmodificacion = dr.GetOrdinal(this.Rerevafecmodificacion);
            if (!dr.IsDBNull(iRerevafecmodificacion)) entity.Rerevafecmodificacion = dr.GetDateTime(iRerevafecmodificacion);
        }

        public string SqlGetNextNumVersion
        {
            get { return base.GetSqlXml("GetNextNumVersion"); }
        }

        public string SqlGetByRevisionAndLastNumVersion
        {
            get { return base.GetSqlXml("GetByRevisionAndLastNumVersion"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }

        public string SqlUpdateEstadoAGenerado
        {
            get { return base.GetSqlXml("UpdateEstadoAGenerado"); }
        }

        public string SqlGetUltimaByEstadoEvaluacionByAnioTarifario
        {
            get { return base.GetSqlXml("GetUltimaByEstadoEvaluacionByAnioTarifario"); }
        }

        public string SqlGetCantidadEvaluacionValidado
        {
            get { return base.GetSqlXml("GetCantidadEvaluacionValidado"); }
        }
    }
}