using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_PROCESO_DIARIO
    /// </summary>
    public class CoProcesoDiarioHelper : HelperBase
    {
        public CoProcesoDiarioHelper(): base(Consultas.CoProcesoDiarioSql)
        {
        }

        public CoProcesoDiarioDTO Create(IDataReader dr)
        {
            CoProcesoDiarioDTO entity = new CoProcesoDiarioDTO();

            int iProdiacodi = dr.GetOrdinal(this.Prodiacodi);
            if (!dr.IsDBNull(iProdiacodi)) entity.Prodiacodi = Convert.ToInt32(dr.GetValue(iProdiacodi));

            int iProdiafecha = dr.GetOrdinal(this.Prodiafecha);
            if (!dr.IsDBNull(iProdiafecha)) entity.Prodiafecha = dr.GetDateTime(iProdiafecha);

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iPerprgcodi = dr.GetOrdinal(this.Perprgcodi);
            if (!dr.IsDBNull(iPerprgcodi)) entity.Perprgcodi = Convert.ToInt32(dr.GetValue(iPerprgcodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iProdiaindreproceso = dr.GetOrdinal(this.Prodiaindreproceso);
            if (!dr.IsDBNull(iProdiaindreproceso)) entity.Prodiaindreproceso = dr.GetString(iProdiaindreproceso);

            int iProdiatipo = dr.GetOrdinal(this.Prodiatipo);
            if (!dr.IsDBNull(iProdiatipo)) entity.Prodiatipo = dr.GetString(iProdiatipo);

            int iProdiaestado = dr.GetOrdinal(this.Prodiaestado);
            if (!dr.IsDBNull(iProdiaestado)) entity.Prodiaestado = dr.GetString(iProdiaestado);

            int iProdiausucreacion = dr.GetOrdinal(this.Prodiausucreacion);
            if (!dr.IsDBNull(iProdiausucreacion)) entity.Prodiausucreacion = dr.GetString(iProdiausucreacion);

            int iProdiafeccreacion = dr.GetOrdinal(this.Prodiafeccreacion);
            if (!dr.IsDBNull(iProdiafeccreacion)) entity.Prodiafeccreacion = dr.GetDateTime(iProdiafeccreacion);

            int iProdiausumodificacion = dr.GetOrdinal(this.Prodiausumodificacion);
            if (!dr.IsDBNull(iProdiausumodificacion)) entity.Prodiausumodificacion = dr.GetString(iProdiausumodificacion);

            int iProdiafecmodificacion = dr.GetOrdinal(this.Prodiafecmodificacion);
            if (!dr.IsDBNull(iProdiafecmodificacion)) entity.Prodiafecmodificacion = dr.GetDateTime(iProdiafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Prodiacodi = "PRODIACODI";
        public string Prodiafecha = "PRODIAFECHA";
        public string Copercodi = "COPERCODI";
        public string Perprgcodi = "PERPRGCODI";
        public string Covercodi = "COVERCODI";
        public string Prodiaindreproceso = "PRODIAINDREPROCESO";
        public string Prodiatipo = "PRODIATIPO";
        public string Prodiaestado = "PRODIAESTADO";
        public string Prodiausucreacion = "PRODIAUSUCREACION";
        public string Prodiafeccreacion = "PRODIAFECCREACION";
        public string Prodiausumodificacion = "PRODIAUSUMODIFICACION";
        public string Prodiafecmodificacion = "PRODIAFECMODIFICACION";

        #endregion

        public string SqlListarByPeriodoVersion
        {
            get { return base.GetSqlXml("ListarByPeriodoVersion"); }
        }

        public string SqlListarByPeriodo
        {
            get { return base.GetSqlXml("ListarByPeriodo"); }
        }

        public string SqlEliminarProcesosDiarios
        {
            get { return base.GetSqlXml("EliminarProcesosDiarios"); }
        }
        public string SqlListarByRango
        {
            get { return base.GetSqlXml("ListarByRango"); }
        }
        
        public string SqlObtenerProcesoDiario
        {
            get { return base.GetSqlXml("ObtenerProcesoDiario"); }
        }
    }
}
