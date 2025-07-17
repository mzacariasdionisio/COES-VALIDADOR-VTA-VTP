using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_FACTOR_UTILIZACION
    /// </summary>
    public class CoFactorUtilizacionHelper : HelperBase
    {
        public CoFactorUtilizacionHelper(): base(Consultas.CoFactorUtilizacionSql)
        {
        }

        public CoFactorUtilizacionDTO Create(IDataReader dr)
        {
            CoFactorUtilizacionDTO entity = new CoFactorUtilizacionDTO();

            int iFacutibeta = dr.GetOrdinal(this.Facutibeta);
            if (!dr.IsDBNull(iFacutibeta)) entity.Facutibeta = dr.GetDecimal(iFacutibeta);

            int iFacutiusucreacion = dr.GetOrdinal(this.Facutiusucreacion);
            if (!dr.IsDBNull(iFacutiusucreacion)) entity.Facutiusucreacion = dr.GetString(iFacutiusucreacion);

            int iFacutifeccreacion = dr.GetOrdinal(this.Facutifeccreacion);
            if (!dr.IsDBNull(iFacutifeccreacion)) entity.Facutifeccreacion = dr.GetDateTime(iFacutifeccreacion);

            int iFacutiusumodificacion = dr.GetOrdinal(this.Facutiusumodificacion);
            if (!dr.IsDBNull(iFacutiusumodificacion)) entity.Facutiusumodificacion = dr.GetString(iFacutiusumodificacion);

            int iFacutifecmodificacion = dr.GetOrdinal(this.Facutifecmodificacion);
            if (!dr.IsDBNull(iFacutifecmodificacion)) entity.Facutifecmodificacion = dr.GetDateTime(iFacutifecmodificacion);

            int iFacuticodi = dr.GetOrdinal(this.Facuticodi);
            if (!dr.IsDBNull(iFacuticodi)) entity.Facuticodi = Convert.ToInt32(dr.GetValue(iFacuticodi));

            int iProdiacodi = dr.GetOrdinal(this.Prodiacodi);
            if (!dr.IsDBNull(iProdiacodi)) entity.Prodiacodi = Convert.ToInt32(dr.GetValue(iProdiacodi));

            int iFacutiperiodo = dr.GetOrdinal(this.Facutiperiodo);
            if (!dr.IsDBNull(iFacutiperiodo)) entity.Facutiperiodo = Convert.ToInt32(dr.GetValue(iFacutiperiodo));

            int iFacutialfa = dr.GetOrdinal(this.Facutialfa);
            if (!dr.IsDBNull(iFacutialfa)) entity.Facutialfa = dr.GetDecimal(iFacutialfa);

            return entity;
        }


        #region Mapeo de Campos

        public string Facutibeta = "FACUTIBETA";
        public string Facutiusucreacion = "FACUTIUSUCREACION";
        public string Facutifeccreacion = "FACUTIFECCREACION";
        public string Facutiusumodificacion = "FACUTIUSUMODIFICACION";
        public string Facutifecmodificacion = "FACUTIFECMODIFICACION";
        public string Facuticodi = "FACUTICODI";
        public string Prodiacodi = "PRODIACODI";
        public string Facutiperiodo = "FACUTIPERIODO";
        public string Facutialfa = "FACUTIALFA";
        public string Prodiafecha = "prodiafecha";
        public string Perprgvalor = "perprgvalor";

        #endregion

        public string SqlObtenerReporte
        {
            get { return base.GetSqlXml("ObtenerReporte"); }
        }

        public string SqlObtenerReporteDiario
        {
            get { return base.GetSqlXml("ObtenerReporteDiario"); }
        }

        public string SqlEliminarFactoresUtilizacion
        {
            get { return base.GetSqlXml("EliminarFactoresUtilizacion"); }
        }
        public string SqlObtenerReporteResultados
        {
            get { return base.GetSqlXml("ObtenerReporteResultados"); }
        }

        public string SqlGetByProdiacodiYPeriodo
        {
            get { return base.GetSqlXml("GetByProdiacodiYPeriodo"); }
        }
        
    }
}
