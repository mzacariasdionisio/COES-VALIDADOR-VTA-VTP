using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DAI_CALENDARIOPAGO
    /// </summary>
    public class DaiCalendariopagoHelper : HelperBase
    {
        public DaiCalendariopagoHelper(): base(Consultas.DaiCalendariopagoSql)
        {
        }

        public string SqlReprocesar
        {
            get { return GetSqlXml("Reprocesar"); }
        }

        public string SqlLiquidar
        {
            get { return GetSqlXml("Liquidar"); }
        }

        public string SqlGetByCriteriaByAnio
        {
            get { return GetSqlXml("GetByCriteriaByAnio"); }
        }

        public DaiCalendariopagoDTO Create(IDataReader dr)
        {
            DaiCalendariopagoDTO entity = new DaiCalendariopagoDTO();

            int iCalecodi = dr.GetOrdinal(this.Calecodi);
            if (!dr.IsDBNull(iCalecodi)) entity.Calecodi = Convert.ToInt32(dr.GetValue(iCalecodi));

            int iAporcodi = dr.GetOrdinal(this.Aporcodi);
            if (!dr.IsDBNull(iAporcodi)) entity.Aporcodi = Convert.ToInt32(dr.GetValue(iAporcodi));

            int iCaleanio = dr.GetOrdinal(this.Caleanio);
            if (!dr.IsDBNull(iCaleanio)) entity.Caleanio = dr.GetString(iCaleanio);

            int iCalenroamortizacion = dr.GetOrdinal(this.Calenroamortizacion);
            if (!dr.IsDBNull(iCalenroamortizacion)) entity.Calenroamortizacion = Convert.ToInt32(dr.GetValue(iCalenroamortizacion));

            int iCalecapital = dr.GetOrdinal(this.Calecapital);
            if (!dr.IsDBNull(iCalecapital)) entity.Calecapital = dr.GetDecimal(iCalecapital);

            int iCaleinteres = dr.GetOrdinal(this.Caleinteres);
            if (!dr.IsDBNull(iCaleinteres)) entity.Caleinteres = dr.GetDecimal(iCaleinteres);

            int iCaleamortizacion = dr.GetOrdinal(this.Caleamortizacion);
            if (!dr.IsDBNull(iCaleamortizacion)) entity.Caleamortizacion = dr.GetDecimal(iCaleamortizacion);

            int iCaletotal = dr.GetOrdinal(this.Caletotal);
            if (!dr.IsDBNull(iCaletotal)) entity.Caletotal = dr.GetDecimal(iCaletotal);

            int iCalecartapago = dr.GetOrdinal(this.Calecartapago);
            if (!dr.IsDBNull(iCalecartapago)) entity.Calecartapago = dr.GetString(iCalecartapago);

            int iCaleactivo = dr.GetOrdinal(this.Caleactivo);
            if (!dr.IsDBNull(iCaleactivo)) entity.Caleactivo = dr.GetString(iCaleactivo);

            int iCaleusucreacion = dr.GetOrdinal(this.Caleusucreacion);
            if (!dr.IsDBNull(iCaleusucreacion)) entity.Caleusucreacion = dr.GetString(iCaleusucreacion);

            int iCalefeccreacion = dr.GetOrdinal(this.Calefeccreacion);
            if (!dr.IsDBNull(iCalefeccreacion)) entity.Calefeccreacion = dr.GetDateTime(iCalefeccreacion);

            int iCaleusumodificacion = dr.GetOrdinal(this.Caleusumodificacion);
            if (!dr.IsDBNull(iCaleusumodificacion)) entity.Caleusumodificacion = dr.GetString(iCaleusumodificacion);

            int iCalefecmodificacion = dr.GetOrdinal(this.Calefecmodificacion);
            if (!dr.IsDBNull(iCalefecmodificacion)) entity.Calefecmodificacion = dr.GetDateTime(iCalefecmodificacion);

            int iTabcdcodiestado = dr.GetOrdinal(this.Tabcdcodiestado);
            if (!dr.IsDBNull(iTabcdcodiestado)) entity.Tabcdcodiestado = Convert.ToInt32(dr.GetValue(iTabcdcodiestado)); 

            return entity;
        }


        #region Mapeo de Campos

        public string Calecodi = "CALECODI";
        public string Aporcodi = "APORCODI";
        public string Caleanio = "CALEANIO";
        public string Calenroamortizacion = "CALENROAMORTIZACION";
        public string Calecapital = "CALECAPITAL";
        public string Caleinteres = "CALEINTERES";
        public string Caleamortizacion = "CALEAMORTIZACION";
        public string Caletotal = "CALETOTAL";
        public string Calecartapago = "CALECARTAPAGO";
        public string Calechequeamortpago = "CALECHEQUEAMORTPAGO";
        public string Calechequeintpago = "CALECHEQUEINTPAGO";
        public string Caleactivo = "CALEACTIVO";
        public string Caleusucreacion = "CALEUSUCREACION";
        public string Calefeccreacion = "CALEFECCREACION";
        public string Caleusumodificacion = "CALEUSUMODIFICACION";
        public string Calefecmodificacion = "CALEFECMODIFICACION";
        public string Tabcdcodiestado = "Tabcdcodiestado";

        public string Tabddescripcion = "tabddescripcion";
        public string Presanio = "presanio";

        #endregion
    }
}
