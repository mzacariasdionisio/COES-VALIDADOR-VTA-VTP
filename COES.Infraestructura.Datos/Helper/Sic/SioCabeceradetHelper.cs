using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_CABECERADET
    /// </summary>
    public class SioCabeceradetHelper : HelperBase
    {
        public SioCabeceradetHelper()
            : base(Consultas.SioCabeceradetSql)
        {
        }

        public SioCabeceradetDTO Create(IDataReader dr)
        {
            SioCabeceradetDTO entity = new SioCabeceradetDTO();

            int iCabpricodi = dr.GetOrdinal(this.Cabpricodi);
            if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

            int iTpriecodi = dr.GetOrdinal(this.Tpriecodi);
            if (!dr.IsDBNull(iTpriecodi)) entity.Tpriecodi = Convert.ToInt32(dr.GetValue(iTpriecodi));

            int iCabpriperiodo = dr.GetOrdinal(this.Cabpriperiodo);
            if (!dr.IsDBNull(iCabpriperiodo)) entity.Cabpriperiodo = dr.GetDateTime(iCabpriperiodo);

            int iCabpriusucreacion = dr.GetOrdinal(this.Cabpriusucreacion);
            if (!dr.IsDBNull(iCabpriusucreacion)) entity.Cabpriusucreacion = dr.GetString(iCabpriusucreacion);

            int iCabprifecha = dr.GetOrdinal(this.Cabprifeccreacion);
            if (!dr.IsDBNull(iCabprifecha)) entity.Cabprifeccreacion = dr.GetDateTime(iCabprifecha);

            int iCabpriversion = dr.GetOrdinal(this.Cabpriversion);
            if (!dr.IsDBNull(iCabpriversion)) entity.Cabpriversion = dr.GetInt32(iCabpriversion);

            int iCabpritieneregistros = dr.GetOrdinal(this.Cabpritieneregistros);
            if (!dr.IsDBNull(iCabpritieneregistros)) entity.Cabpritieneregistros = dr.GetInt32(iCabpritieneregistros);

            return entity;
        }


        #region Mapeo de Campos

        public string Cabpricodi = "CABPRICODI";
        public string Tpriecodi = "TPRIECODI";
        public string Cabpriperiodo = "CABPRIPERIODO";
        public string Cabpriusucreacion = "CABPRIUSUCREACION";
        public string Cabprifeccreacion = "CABPRIFECCREACION";
        public string Cabpriversion = "CABPRIVERSION";
        public string Cabpritieneregistros = "CABPRITIENEREGISTROS";

        public string SqlObtenerUltNroVersion
        {
            get { return GetSqlXml("ObtenerUltNroVersion"); }
        }

        public string SqlObtenerUltVersion
        {
            get { return GetSqlXml("ObtenerUltVersion"); }
        }

        public string SqlGetByCriteriaPeriodo
        {
            get { return GetSqlXml("GetByCriteriaPeriodo"); }
        }

        #endregion
    }
}
